using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Gekko
{
    /// <summary>One row in the numbered bank list.</summary>
    public sealed class BankItem : INotifyPropertyChanged
    {
        static readonly Brush GreyNumber = BankColors.FromRgb(0xB4, 0xB4, 0xB4);

        int position;
        bool inComparison;
        bool hiddenInDiagram;
        bool isDragging;

        public BankItem(CompareBank bank)
        {
            Bank = bank;
        }

        /// <summary>The empty row always at the end of the list (no bank). Its [Recent] button adds a bank.</summary>
        public static BankItem CreatePlaceholder()
        {
            return new BankItem(null);
        }

        public CompareBank Bank { get; }
        public bool IsPlaceholder => Bank == null;
        public string Name => IsPlaceholder ? "" : Bank.Name;
        public string PathText => IsPlaceholder ? "(empty: use Recent, or drop a file here)" : Bank.FilePath ?? "(artificial data)";

        /// <summary>E.g. "A 380, Q 200, M 70".</summary>
        public string SeriesText
        {
            get
            {
                if (IsPlaceholder) return "";
                return string.Join(", ", Bank.Frequencies.Select(f => f.ToString().Substring(0, 1) + " " + Bank.Count(f)));
            }
        }

        /// <summary>The number badge and the remove (X) button are not shown on the empty row.</summary>
        public Visibility RowVisibility => IsPlaceholder ? Visibility.Collapsed : Visibility.Visible;
        public string RecentTip => IsPlaceholder
            ? "Add one of the most recently compared databanks"
            : "Replace this databank with one of the most recently compared databanks";

        public int Position
        {
            get { return position; }
            set
            {
                if (position == value) return;
                position = value;
                Raise(nameof(Position));
                Raise(nameof(NumberBrush));
            }
        }

        /// <summary>True for the first 2 or 3 banks, which are the ones in the diagram.</summary>
        public bool InComparison
        {
            get { return inComparison; }
            set
            {
                if (inComparison == value) return;
                inComparison = value;
                Raise(nameof(InComparison));
                Raise(nameof(NumberBrush));
                Raise(nameof(ItemOpacity));
            }
        }

        /// <summary>True when the bank is compared but hidden (eye icon) or greyed out by a focus on another bank.</summary>
        public bool HiddenInDiagram
        {
            get { return hiddenInDiagram; }
            set
            {
                if (hiddenInDiagram == value) return;
                hiddenInDiagram = value;
                Raise(nameof(HiddenInDiagram));
                Raise(nameof(NumberBrush));
            }
        }

        /// <summary>True while the row is being dragged (shown in bold).</summary>
        public bool IsDragging
        {
            get { return isDragging; }
            set
            {
                if (isDragging == value) return;
                isDragging = value;
                Raise(nameof(IsDragging));
            }
        }

        /// <summary>The number badge has the circle's color for compared banks that are shown, grey otherwise.</summary>
        public Brush NumberBrush => InComparison && !HiddenInDiagram ? BankColors.Solid(Position - 1) : GreyNumber;
        public double ItemOpacity => InComparison || IsPlaceholder ? 1.0 : 0.6;

        public event PropertyChangedEventHandler PropertyChanged;

        void Raise(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public partial class WindowBankCompare : Window
    {
        const string DragFormat = "GekkoBankCompareItem";
        static readonly Brush DropHighlight = BankColors.FromRgb(0x2E, 0x6D, 0xB4);
        static readonly Brush DropNormal = BankColors.FromRgb(0x99, 0x99, 0x99);
        static readonly Brush InvalidBack = BankColors.FromRgb(0xFB, 0xE3, 0xE3);
        static readonly Brush AutoPeriodBrush = BankColors.FromRgb(0x88, 0x88, 0x88);
        // Deviating names in the Names tab, same dark red as the deviation numbers in the diagram.
        static readonly Brush DeviatingNameBrush = BankColors.DeviationText;
        static readonly Brush RecentPathBrush = BankColors.FromRgb(0x88, 0x88, 0x88);

        // Length of the "Recent" list (LRU, stored in the user settings).
        const int MaxRecentBanks = 20;
        const char RecentSeparator = '|';  // cannot occur in a Windows path

        readonly ObservableCollection<BankItem> banks = new ObservableCollection<BankItem>();
        // The list shows the banks plus an empty row at the end (never part of 'banks').
        readonly BankItem placeholder = BankItem.CreatePlaceholder();
        readonly IEqualityCriterion criterion = new SimpleEqualityCriterion();
        readonly bool[] active = { true, true, true };
        readonly DispatcherTimer recomputeTimer;
        bool ready;
        bool busy;

        VennResult current;

        // ---- Scope: which banks are shown (eye icons) and the focus (target icon, Alt+1/2/3).
        // focusSlot is the only bank shown, or -1. activeBeforeFocus is the hide/show state to return to.
        int focusSlot = -1;
        bool[] activeBeforeFocus;

        // ---- Drill-down filters (the "pivot"). Changing them never recomputes, it only re-filters.
        //   Area:     one area (selectedMask != 0) or a whole bank (selectedBank >= 0), or nothing.
        //   Period:   userFrom..userTo (the From/To boxes); null means the edge of the universal period.
        //   Flags:    deviatesFilter and missingFilter, each Any / Yes / No.
        // A series passes when it is in the area, and its flags in the selected periods match the flag filters.
        int selectedMask;
        int selectedBank = -1;
        FlagFilter deviatesFilter = FlagFilter.Any;
        FlagFilter missingFilter = FlagFilter.Any;
        int? userFrom;
        int? userTo;
        int? periodAnchor;  // the first bar clicked, for Shift+click ranges
        CompareFreq? periodFreq;

        Selection selection;                                              // the area filter, resolved
        List<SeriesComparison> shown = new List<SeriesComparison>();      // series passing all filters

        List<SeriesComparison> navList = new List<SeriesComparison>();
        int navIndex = -1;
        DeviationListing listing;
        const string NoSelectionMessage = "Click a number, an area or a bank in the diagram.";
        string devMessage = NoSelectionMessage;
        bool histogramShowsDeviations;  // which histogram is shown: deviations (2 or 3 circles) or missings (1 circle)

        // The Names tab is a RichTextBox (for the dark red names), so keep the plain text for Copy.
        string namesPlainText = "";

        bool diagramHidden;
        // The number of circles (2 or 3) the diagram row height is currently fitted to. See FitVennRow.
        int layoutSlots = 3;
        GridLength savedVennHeight = new GridLength(1.25, GridUnitType.Star);

        Point dragStart;
        BankItem dragCandidate;
        InsertionLineAdorner insertionLine;

        public WindowBankCompare()
        {
            InitializeComponent();
            BankList.ItemsSource = new CompositeCollection
            {
                new CollectionContainer { Collection = banks },
                placeholder
            };
            RestoreWindowPlacement();

            recomputeTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            recomputeTimer.Tick += (s, e) => Recompute();

            Venn.AreaClicked += Venn_AreaClicked;
            Venn.BankSelected += Venn_BankSelected;
            Venn.HideClicked += ToggleHidden;
            Venn.FocusClicked += ToggleFocus;
            Histogram.BarClicked += (period, keys) => Bar_Clicked(period, keys, histogramShowsDeviations);
            Histogram.BarDoubleClicked += period => Tabs.SelectedItem = NamesTab;

            // Artificial demo banks. Dropped files also get artificial data, see DatabankLoader.
            banks.Add(new BankItem(ArtificialData.CreateBank("base_2024.gbk", null)));
            banks.Add(new BankItem(ArtificialData.CreateBank("base_2025.gbk", null)));
            banks.Add(new BankItem(ArtificialData.CreateBank("forecast_2025.gbk", null)));

            ready = true;
            BanksChanged();
            SetStatus("Three databanks with artificial data are loaded. Drop files to add banks, drag rows to reorder.");
        }

        int SlotCount => Compare3Radio.IsChecked == true && banks.Count >= 3 ? 3 : 2;

        CompareFreq? SelectedFreq
        {
            get
            {
                var item = FreqCombo.SelectedItem as ComboBoxItem;
                return item == null ? (CompareFreq?)null : (CompareFreq)item.Tag;
            }
        }

        // ---------------------------------------------------------------- user settings

        // Adjust to however the existing UserSettings instance is reached in Gekko. Use the shared
        // instance rather than a new UserSettings(): two instances saving would overwrite each other.
        static UserSettings Settings => Globals.userSettings;

        /// <summary>Size and position from the last session, kept inside the screen(s) present now.</summary>
        void RestoreWindowPlacement()
        {
            UserSettings s = Settings;
            double w = s.CompareWindowWidth;
            double h = s.CompareWindowHeight;
            if (w <= 0 || h <= 0) return;  // never saved: keep the XAML defaults

            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;

            w = Math.Max(MinWidth, Math.Min(w, screenWidth));
            h = Math.Max(MinHeight, Math.Min(h, screenHeight));
            double left = Math.Max(screenLeft, Math.Min(s.CompareWindowLeftDistance, screenLeft + screenWidth - w));
            double top = Math.Max(screenTop, Math.Min(s.CompareWindowTopDistance, screenTop + screenHeight - h));

            WindowStartupLocation = WindowStartupLocation.Manual;
            Width = w;
            Height = h;
            Left = left;
            Top = top;
        }

        /// <summary>The splitters can only be set once the rows have their actual sizes.</summary>
        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserSettings s = Settings;

            double bankListHeight = s.CompareWindowSplitterDistance1;
            if (bankListHeight > 0)
            {
                // Leave room for the diagram (200), splitters and the tabs (150) below.
                double max = Math.Max(BankListRow.MinHeight, RootGrid.ActualHeight - 420);
                BankListRow.Height = new GridLength(Math.Max(BankListRow.MinHeight, Math.Min(bankListHeight, max)));
                UpdateLayout();
            }

            double vennHeight = s.CompareWindowSplitterDistance2;
            if (vennHeight > 0)
            {
                // Star sizes in the saved proportion, so both parts still grow when the window is resized.
                double total = VennRow.ActualHeight + DetailsRow.ActualHeight;
                double venn = Math.Max(VennRow.MinHeight, Math.Min(vennHeight, total - DetailsRow.MinHeight));
                if (venn > 0 && total - venn > 0)
                {
                    VennRow.Height = new GridLength(venn, GridUnitType.Star);
                    DetailsRow.Height = new GridLength(total - venn, GridUnitType.Star);
                }
            }

            // The saved height is for 3 circles; shrink it if the window starts out comparing 2 banks.
            UpdateLayout();
            if (current != null) FitVennRow(current.SlotCount);
        }

        /// <summary>
        /// Going from 3 to 2 circles removes the room the top circle took, and gives it to the tabs
        /// below; going back from 2 to 3 takes it back. The circles keep their size.
        /// </summary>
        void FitVennRow(int slots)
        {
            if (slots == layoutSlots || diagramHidden) return;
            if (Venn.ActualWidth <= 0 || Venn.ActualHeight <= 0) return;  // not laid out yet, Window_Loaded does it

            double radius = VennView.RadiusFor(Venn.ActualWidth, Venn.ActualHeight, layoutSlots);
            double change = VennView.ThirdCircleHeight(radius) * (slots == 2 ? -1 : 1);

            // Never smaller than the controls to the right of the diagram need.
            double controls = ControlsContent.ActualHeight + ControlsPanel.Padding.Top + ControlsPanel.Padding.Bottom
                              + ControlsPanel.BorderThickness.Top + ControlsPanel.BorderThickness.Bottom;
            double total = VennRow.ActualHeight + DetailsRow.ActualHeight;
            double min = Math.Max(VennRow.MinHeight, controls);
            double venn = Math.Max(min, Math.Min(VennRow.ActualHeight + change, total - DetailsRow.MinHeight));
            if (venn > 0 && total - venn > 0)
            {
                VennRow.Height = new GridLength(venn, GridUnitType.Star);
                DetailsRow.Height = new GridLength(total - venn, GridUnitType.Star);
            }
            layoutSlots = slots;
        }

        void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                UserSettings s = Settings;
                Rect r = WindowState == WindowState.Normal ? new Rect(Left, Top, ActualWidth, ActualHeight) : RestoreBounds;
                if (!r.IsEmpty)
                {
                    s.CompareWindowLeftDistance = r.Left;
                    s.CompareWindowTopDistance = r.Top;
                    s.CompareWindowWidth = r.Width;
                    s.CompareWindowHeight = r.Height;
                }
                s.CompareWindowSplitterDistance1 = BankListRow.ActualHeight;
                // Only when the controls are shown; otherwise keep the height from when they were.
                // Always saved as the height for 3 circles (Window_Loaded shrinks it again for 2).
                if (!diagramHidden)
                {
                    double venn = VennRow.ActualHeight;
                    if (layoutSlots == 2 && Venn.ActualWidth > 0)
                        venn += VennView.ThirdCircleHeight(VennView.RadiusFor(Venn.ActualWidth, Venn.ActualHeight, 2));
                    s.CompareWindowSplitterDistance2 = venn;
                }
                s.Save();
            }
            catch (Exception)
            {
                // Failing to save the layout must never prevent closing the window.
            }
        }

        // ---------------------------------------------------------------- recent databanks

        static List<string> LoadRecentBanks()
        {
            string text = Settings.CompareRecentBanks ?? "";
            return text.Split(new[] { RecentSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        static void SaveRecentBanks(List<string> paths)
        {
            try
            {
                Settings.CompareRecentBanks = string.Join(RecentSeparator.ToString(), paths.Take(MaxRecentBanks));
                Settings.Save();
            }
            catch (Exception)
            {
                // The list is a convenience; never fail loading a bank because of it.
            }
        }

        /// <summary>Moves (or adds) the path to the top of the LRU list.</summary>
        static void TouchRecentBank(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            List<string> paths = LoadRecentBanks();
            paths.RemoveAll(p => string.Equals(p, path, StringComparison.OrdinalIgnoreCase));
            paths.Insert(0, path);
            SaveRecentBanks(paths);
        }

        static void ForgetRecentBank(string path)
        {
            List<string> paths = LoadRecentBanks();
            if (paths.RemoveAll(p => string.Equals(p, path, StringComparison.OrdinalIgnoreCase)) > 0) SaveRecentBanks(paths);
        }

        bool IsInList(string path)
        {
            return banks.Any(b => string.Equals(b.Bank.FilePath, path, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// The [Recent] button on a row: a numbered list of the most recently compared databanks (banks already in
        /// the list are greyed out). On a bank's row the chosen bank replaces it; on the empty row it is added.
        /// </summary>
        void RowRecent_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var target = button.Tag as BankItem;
            if (target == null) return;
            var menu = new ContextMenu { PlacementTarget = button, Placement = PlacementMode.Bottom };
            List<string> paths = LoadRecentBanks();
            if (paths.Count == 0)
            {
                menu.Items.Add(new MenuItem { Header = new TextBlock { Text = "No recent databanks" }, IsEnabled = false });
            }
            for (int i = 0; i < paths.Count; i++)
            {
                string path = paths[i];
                bool open = IsInList(path);
                // TextBlocks rather than a string header: '_' in file names would otherwise become access keys.
                var header = new StackPanel { Orientation = Orientation.Horizontal, Opacity = open ? 0.45 : 1.0 };
                header.Children.Add(new TextBlock { Text = (i + 1) + ".", Width = 26 });
                header.Children.Add(new TextBlock { Text = Path.GetFileName(path), MinWidth = 160, Margin = new Thickness(0, 0, 16, 0) });
                header.Children.Add(new TextBlock { Text = path, Foreground = RecentPathBrush });
                var item = new MenuItem
                {
                    Header = header,
                    IsEnabled = !open,
                    ToolTip = open ? "Already in the list" : null
                };
                item.Click += (o, args) => OpenRecent(path, target);
                menu.Items.Add(item);
            }
            menu.IsOpen = true;
        }

        void OpenRecent(string path, BankItem target)
        {
            if (IsInList(path)) return;
            if (!File.Exists(path))
            {
                ForgetRecentBank(path);
                SetStatus(path + " no longer exists and was removed from the recent list.");
                return;
            }
            int index = banks.IndexOf(target);
            if (target.IsPlaceholder || index < 0)
            {
                AddFiles(new[] { path }, banks.Count);
                return;
            }
            try
            {
                var item = new BankItem(DatabankLoader.Load(path));
                banks[index] = item;
                TouchRecentBank(path);
                BanksChanged();
                SetStatus(target.Name + " replaced by " + item.Name + ".");
            }
            catch (Exception ex)
            {
                SetStatus("Could not read " + Path.GetFileName(path) + ": " + ex.Message);
            }
        }

        // ---------------------------------------------------------------- recomputation

        /// <summary>Call when banks are added, removed or reordered, or the compare mode changes.</summary>
        void BanksChanged()
        {
            if (!ready) return;
            busy = true;
            try
            {
                Compare3Radio.IsEnabled = banks.Count >= 3;
                if (banks.Count < 3 && Compare3Radio.IsChecked == true) Compare2Radio.IsChecked = true;
                int n = SlotCount;
                for (int i = 0; i < banks.Count; i++)
                {
                    banks[i].Position = i + 1;
                    banks[i].InComparison = i < n;
                }
                for (int s = 0; s < active.Length; s++) active[s] = true;
                focusSlot = -1;
                activeBeforeFocus = null;
                RefreshFrequencies();
            }
            finally
            {
                busy = false;
            }
            Recompute();
        }

        /// <summary>The dropdown only offers frequencies that exist in the compared banks.</summary>
        void RefreshFrequencies()
        {
            CompareFreq? previous = SelectedFreq;
            List<CompareFreq> freqs = banks.Take(SlotCount).SelectMany(b => b.Bank.Frequencies).Distinct().OrderBy(f => f).ToList();
            FreqCombo.Items.Clear();
            foreach (CompareFreq f in freqs) FreqCombo.Items.Add(new ComboBoxItem { Content = f.ToString(), Tag = f });
            int index = previous.HasValue ? freqs.IndexOf(previous.Value) : -1;
            FreqCombo.SelectedIndex = index >= 0 ? index : (freqs.Count > 0 ? 0 : -1);
        }

        void ScheduleRecompute()
        {
            recomputeTimer.Stop();
            recomputeTimer.Start();
        }

        /// <summary>
        /// Recomputes the comparison. Needed when the scope changes: banks, compare mode, hide/focus,
        /// frequency or name filter. The drill-down filters only need Refresh().
        /// </summary>
        void Recompute()
        {
            if (!ready) return;
            recomputeTimer.Stop();

            CompareFreq? freq = SelectedFreq;
            if (banks.Count < 2 || !freq.HasValue)
            {
                current = null;
                selectedMask = 0;
                selectedBank = -1;
                Venn.SetState(null, banks.Count == 0 ? "Drag databank files into the drop field to begin."
                                    : banks.Count == 1 ? "Add one more databank to compare."
                                    : "The compared databanks contain no series.",
                              0, -1, FlagFilter.Any, FlagFilter.Any, -1);
                selection = null;
                ShowDetails();
                UpdatePeriodBoxes();
                UpdateListBadges();
                return;
            }

            // Keep the chosen periods when switching frequency (2010q3 becomes 2010m7 or 2010, and so on).
            if (periodFreq.HasValue && periodFreq.Value != freq.Value)
            {
                if (userFrom.HasValue) userFrom = PeriodText.ChangeFrequency(userFrom.Value, periodFreq.Value, freq.Value, false);
                if (userTo.HasValue) userTo = PeriodText.ChangeFrequency(userTo.Value, periodFreq.Value, freq.Value, true);
                periodAnchor = null;
            }
            periodFreq = freq;

            string error;
            Func<string, bool> filter = CompareEngine.BuildFilter(FilterBox.Text, RegexCheck.IsChecked == true, out error);
            if (error == null) FilterBox.ClearValue(Control.BackgroundProperty);
            else
            {
                FilterBox.Background = InvalidBack;
                SetStatus(error);
            }
            FilterBox.ToolTip = error;

            List<CompareBank> compared = banks.Take(SlotCount).Select(b => b.Bank).ToList();
            current = CompareEngine.Compute(compared, active, freq.Value, filter, criterion);
            Refresh();
        }

        int PeriodFrom => current != null && current.HasSpan ? (userFrom ?? current.SpanStart) : 0;
        int PeriodTo => current != null && current.HasSpan ? (userTo ?? current.SpanEnd) : -1;

        /// <summary>Applies the drill-down filters to the current comparison and updates every view.</summary>
        void Refresh()
        {
            if (current == null)
            {
                Recompute();
                return;
            }
            ClampPeriod();
            CompareEngine.UpdateCounts(current, PeriodFrom, PeriodTo);

            if (selectedBank >= 0 && (selectedBank >= current.SlotCount || !current.Active[selectedBank])) selectedBank = -1;
            if (selectedMask != 0 && !current.Areas.ContainsKey(selectedMask)) selectedMask = 0;
            if (selectedBank >= 0) selectedMask = 0;

            Venn.SetState(current, null, selectedMask, selectedBank, missingFilter, deviatesFilter, focusSlot);
            FitVennRow(current.SlotCount);

            selection = BuildSelection();
            ShowDetails();
            UpdatePeriodBoxes();
            UpdateListBadges();
        }

        /// <summary>A chosen period outside the universal period (e.g. after hiding a bank) falls back to its edge.</summary>
        void ClampPeriod()
        {
            if (!current.HasSpan)
            {
                userFrom = null;
                userTo = null;
                return;
            }
            if (userFrom.HasValue && (userFrom.Value <= current.SpanStart || userFrom.Value > current.SpanEnd)) userFrom = null;
            if (userTo.HasValue && (userTo.Value >= current.SpanEnd || userTo.Value < current.SpanStart)) userTo = null;
            if (PeriodFrom > PeriodTo)
            {
                userFrom = null;
                userTo = null;
            }
        }

        /// <summary>Sets the period filter (and the From/To boxes). Does not refresh.</summary>
        void SetPeriod(int from, int to)
        {
            userFrom = from > current.SpanStart ? (int?)from : null;
            userTo = to < current.SpanEnd ? (int?)to : null;
        }

        void UpdateListBadges()
        {
            for (int i = 0; i < banks.Count; i++)
                banks[i].HiddenInDiagram = current != null && i < current.SlotCount && !current.Active[i];
        }

        // ---------------------------------------------------------------- the area filter

        /// <summary>The area filter, resolved: one area of the diagram, or a whole bank (its full circle).</summary>
        sealed class Selection
        {
            public string Description;              // e.g. "In banks 1 and 2, not in bank 3"
            public List<SeriesComparison> Series;   // all series in it (the black number)
            public int MissingMask;                 // banks in which an M counts as missing (bit s = slot s)
            public bool IsComparison;               // false: series in one bank only, nothing to compare
            public int Circles;                     // number of circles selected: 1 for a whole bank or a one-bank area
        }

        Selection BuildSelection()
        {
            if (current == null) return null;
            if (selectedBank >= 0)
            {
                // A whole bank: the areas inside its circle. Deviations are those of its overlaps with other banks;
                // missing values count in this bank only.
                List<SeriesComparison> series = current.Areas.Values
                    .Where(a => (a.Mask & (1 << selectedBank)) != 0)
                    .SelectMany(a => a.Series)
                    .OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase)
                    .ToList();
                return new Selection
                {
                    Description = "All of bank " + (selectedBank + 1) + " (" + current.Banks[selectedBank].Name + ", "
                                  + series.Count + " series)",
                    Series = series,
                    MissingMask = 1 << selectedBank,
                    IsComparison = series.Any(s => s.Slots.Length >= 2),
                    Circles = 1
                };
            }
            VennArea area;
            if (selectedMask != 0 && current.Areas.TryGetValue(selectedMask, out area))
            {
                return new Selection
                {
                    Description = CompareEngine.DescribeArea(current, area.Mask),
                    Series = area.Series,
                    MissingMask = area.Mask,
                    IsComparison = area.IsComparison,
                    Circles = area.Slots.Length
                };
            }
            return null;
        }

        // ---------------------------------------------------------------- clicks that set filters

        /// <summary>
        /// Inside an area: sets the area, keeps the flag filters.
        /// Black number: the area, both flags cleared. Red number: the area, Deviating = yes.
        /// The missing-value filter belongs to "All of bank x" (a bank's box), so it is always cleared here.
        /// </summary>
        void Venn_AreaClicked(int mask, AreaClickKind kind, bool add)
        {
            if (current == null || !current.Areas.ContainsKey(mask)) return;
            selectedMask = mask;
            selectedBank = -1;
            missingFilter = FlagFilter.Any;  // missing values are only shown for a whole bank ("All of bank x")
            switch (kind)
            {
                case AreaClickKind.Total:
                    deviatesFilter = FlagFilter.Any;
                    missingFilter = FlagFilter.Any;
                    break;
                case AreaClickKind.Deviating:
                    deviatesFilter = FlagFilter.Yes;
                    break;
            }
            Refresh();  // the selected tab stays open
        }

        /// <summary>A bank's box: the whole bank as the area filter. Keeps the flag filters.</summary>
        void Venn_BankSelected(int slot)
        {
            if (current == null || slot >= current.SlotCount || !current.Active[slot]) return;
            selectedBank = slot;
            selectedMask = 0;
            Refresh();  // the selected tab stays open
        }

        /// <summary>
        /// A histogram bar: sets the period (Shift: from the first clicked bar to this one) and the bar's flag
        /// (red: Deviating = yes, blue: Missing = yes; the other flag is cleared unless Ctrl). Keeps the area.
        /// </summary>
        void Bar_Clicked(int period, ModifierKeys keys, bool deviationBar)
        {
            if (current == null || !current.HasSpan) return;
            int from = period, to = period;
            if ((keys & ModifierKeys.Shift) != 0 && periodAnchor.HasValue)
            {
                from = Math.Min(periodAnchor.Value, period);
                to = Math.Max(periodAnchor.Value, period);
            }
            else
            {
                periodAnchor = period;
            }
            SetPeriod(from, to);

            bool add = (keys & ModifierKeys.Control) != 0;
            if (deviationBar)
            {
                deviatesFilter = FlagFilter.Yes;
                if (!add) missingFilter = FlagFilter.Any;
            }
            else
            {
                missingFilter = FlagFilter.Yes;
                if (!add) deviatesFilter = FlagFilter.Any;
            }
            Refresh();
            SetStatus(shown.Count + " series match" + current.CountPeriodText + ". Double-click a bar (or see the Names tab) to list them.");
        }

        // ---------------------------------------------------------------- hide and focus (scope)

        string BankLabel(int slot)
        {
            return "Bank " + (slot + 1) + " (" + current.Banks[slot].Name + ")";
        }

        static int MaskOf(bool[] flags, int slotCount)
        {
            int mask = 0;
            for (int s = 0; s < slotCount; s++) if (flags[s]) mask |= 1 << s;
            return mask;
        }

        /// <summary>
        /// Sets which banks are shown, keeping a sensible area filter: "in all shown banks" stays "in all shown
        /// banks", an area drops the banks that disappear, a whole bank stays selected unless it is hidden.
        /// Does not recompute.
        /// </summary>
        void ApplyActive(bool[] newActive)
        {
            int n = current.SlotCount;
            int oldMask = MaskOf(active, n);
            for (int s = 0; s < active.Length; s++) active[s] = s >= n || newActive[s];
            int newMask = MaskOf(active, n);
            if (selectedMask != 0) selectedMask = selectedMask == oldMask ? newMask : selectedMask & newMask;
            if (selectedBank >= 0 && !active[selectedBank]) selectedBank = -1;
        }

        /// <summary>
        /// The eye icon. Without a focus: hide a shown bank (as long as another one stays shown) or show a hidden one.
        /// With a focus: showing another bank ends the focus; the focused bank itself cannot be hidden.
        /// </summary>
        void ToggleHidden(int slot)
        {
            if (current == null || slot >= current.SlotCount) return;
            string label = BankLabel(slot);
            var next = (bool[])active.Clone();

            if (focusSlot >= 0)
            {
                if (slot == focusSlot)
                {
                    SetStatus(label + " is in focus and cannot be hidden. Click its target to show the other banks again.");
                    return;
                }
                focusSlot = -1;
                activeBeforeFocus = null;
                next[slot] = true;
                ApplyActive(next);
                Recompute();
                SetStatus(label + " is shown again, and the focus has ended.");
                return;
            }

            string message;
            if (!active[slot])
            {
                next[slot] = true;
                message = label + " is shown again.";
            }
            else if (current.ActiveCount > 1)
            {
                next[slot] = false;
                message = label + " is hidden. Click its eye to show it again.";
            }
            else
            {
                SetStatus(label + " is the only bank shown, so it cannot be hidden.");
                return;
            }
            ApplyActive(next);
            Recompute();
            SetStatus(message);
        }

        /// <summary>
        /// The target icon or Alt+1/2/3. Focus: only this bank is shown, and the whole bank becomes the area filter.
        /// Again on the focused bank: the banks shown before the focus come back, the bank stays selected.
        /// The flag and period filters are kept.
        /// </summary>
        void ToggleFocus(int slot)
        {
            if (current == null || slot >= current.SlotCount) return;
            string label = BankLabel(slot);

            if (focusSlot == slot)
            {
                bool[] restore = activeBeforeFocus ?? new[] { true, true, true };
                restore[slot] = true;
                focusSlot = -1;
                activeBeforeFocus = null;
                ApplyActive(restore);
                selectedMask = 0;
                selectedBank = slot;
                Recompute();
                SetStatus("Focus on " + label + " ended. The whole bank stays selected.");
                return;
            }

            if (focusSlot < 0) activeBeforeFocus = (bool[])active.Clone();
            focusSlot = slot;
            var only = new bool[3];
            only[slot] = true;
            ApplyActive(only);
            selectedMask = 0;
            selectedBank = slot;
            Recompute();
            SetStatus("Focus on " + label + ": the other banks are greyed out. Click the target again (or Alt+"
                      + (slot + 1) + ") to show them.");
        }

        // ---------------------------------------------------------------- details: names, histograms, values

        /// <summary>Applies the filters to the selection and fills the tabs.</summary>
        void ShowDetails()
        {
            string keepName = navIndex >= 0 && navIndex < navList.Count ? navList[navIndex].Name : null;

            if (selection == null || current == null)
            {
                shown = new List<SeriesComparison>();
                ShowNames();
                ShowHistogram();
                navList = shown;
                navIndex = -1;
                devMessage = NoSelectionMessage;
                ShowCurrentSeries();
                return;
            }

            int from = PeriodFrom, to = PeriodTo;
            int mask = selection.MissingMask;
            shown = selection.Series
                .Where(s => SeriesComparison.Passes(deviatesFilter, s.DeviatesIn(from, to))
                         && SeriesComparison.Passes(missingFilter, s.MissingIn(from, to, mask)))
                .ToList();

            ShowNames();
            ShowHistogram();

            navList = shown;
            navIndex = shown.Count == 0 ? -1 : 0;
            if (keepName != null)
            {
                int i = navList.FindIndex(c => string.Equals(c.Name, keepName, StringComparison.OrdinalIgnoreCase));
                if (i >= 0) navIndex = i;
            }
            devMessage = "No series match the current filters.";
            ShowCurrentSeries();
        }

        /// <summary>
        /// One histogram tab, whose meaning follows the selection:
        ///   "All of bank x" (a bank's box): "Missings histogram", blue, series with M in that bank per period;
        ///   an overlap area with the deviation filter on (red number or red bar): "Deviations histogram", red;
        ///   anything else: a message saying how to get a histogram.
        /// It shows the whole universal period for the area only (not the flag or period filters), so the time
        /// profile stays visible while drilling down. The selected periods are highlighted.
        /// </summary>
        void ShowHistogram()
        {
            histogramShowsDeviations = false;
            if (selection == null || current == null)
            {
                HistogramTab.Header = "Histogram";
                Histogram.ShowMessage(NoSelectionMessage);
                return;
            }

            bool missings = selectedBank >= 0;
            bool deviations = !missings && selection.IsComparison && deviatesFilter == FlagFilter.Yes;
            if (!missings && !deviations)
            {
                HistogramTab.Header = "Histogram";
                Histogram.ShowMessage(selection.IsComparison
                    ? "To see deviations, click the red number. To see missing values, click a rectangular databank box."
                    : "To see missing values, click a rectangular databank box.");
                return;
            }

            histogramShowsDeviations = deviations;
            HistogramTab.Header = deviations ? "Deviations histogram" : "Missings histogram";
            if (!current.HasSpan)
            {
                Histogram.ShowMessage("No periods to show.");
                return;
            }
            int start = current.SpanStart, end = current.SpanEnd;
            int? hiFrom = current.IsPeriodRestricted ? PeriodFrom : (int?)null;
            int? hiTo = current.IsPeriodRestricted ? PeriodTo : (int?)null;

            if (deviations)
            {
                Histogram.SetData(selection.Description + ": number of deviating series, per period",
                                  current.Freq, start, CompareEngine.DeviationCounts(selection.Series, start, end),
                                  true, null, hiFrom, hiTo);
            }
            else
            {
                int[][] perSlot;
                int[] missing = CompareEngine.MissingCounts(selection.Series, selection.MissingMask, start, end, out perSlot);
                Histogram.SetData(selection.Description + ": number of series with missing values (M), per period",
                                  current.Freq, start, missing, false, null, hiFrom, hiTo);
            }
        }

        // ---------------------------------------------------------------- period from/to (the period filter)

        void UpdatePeriodBoxes()
        {
            bool has = current != null && current.HasSpan;
            FromSpinner.IsEnabled = has;
            ToSpinner.IsEnabled = has;
            WholePeriodButton.IsEnabled = has && (userFrom.HasValue || userTo.HasValue);
            if (!has)
            {
                FromBox.Text = "";
                ToBox.Text = "";
                SpanText.Text = "";
                return;
            }
            SpanText.Text = "Universal period " + current.SpanText;
            ShowPeriod(FromBox, userFrom, current.SpanStart);
            ShowPeriod(ToBox, userTo, current.SpanEnd);
        }

        /// <summary>Grey text: follows the universal period. Black text: chosen by the user.</summary>
        void ShowPeriod(TextBox box, int? chosen, int automatic)
        {
            box.Text = PeriodText.Format(current.Freq, chosen ?? automatic);
            if (chosen.HasValue) box.ClearValue(Control.ForegroundProperty);
            else box.Foreground = AutoPeriodBrush;
            box.ClearValue(Control.BackgroundProperty);
        }

        /// <summary>Reads the typed period. Returns false (and marks the box) if it is not valid.</summary>
        bool TryCommitPeriod(bool isFrom)
        {
            if (current == null || !current.HasSpan) return false;
            TextBox box = isFrom ? FromBox : ToBox;
            string text = box.Text.Trim();

            int? value = null;
            if (text.Length > 0)
            {
                int p;
                if (!PeriodText.TryParse(text, current.Freq, !isFrom, out p))
                {
                    box.Background = InvalidBack;
                    SetStatus("\"" + text + "\" is not a " + current.Freq.ToString().ToLowerInvariant()
                              + " period. Type for example " + PeriodText.Format(current.Freq, current.SpanStart) + ".");
                    return false;
                }
                value = p;
            }

            int from = isFrom ? (value ?? current.SpanStart) : (userFrom ?? current.SpanStart);
            int to = isFrom ? (userTo ?? current.SpanEnd) : (value ?? current.SpanEnd);
            if (from > to)
            {
                box.Background = InvalidBack;
                SetStatus("The first period must not be after the last period.");
                return false;
            }

            box.ClearValue(Control.BackgroundProperty);
            if (isFrom) userFrom = value.HasValue && value.Value > current.SpanStart ? value : null;
            else userTo = value.HasValue && value.Value < current.SpanEnd ? value : null;
            return true;
        }

        void CommitPeriod(bool isFrom)
        {
            int? oldFrom = userFrom, oldTo = userTo;
            if (!TryCommitPeriod(isFrom)) return;
            if (oldFrom != userFrom || oldTo != userTo)
            {
                periodAnchor = null;
                Refresh();
            }
            else UpdatePeriodBoxes();
        }

        void StepPeriod(bool isFrom, int delta)
        {
            if (!TryCommitPeriod(isFrom)) return;
            int from = userFrom ?? current.SpanStart;
            int to = userTo ?? current.SpanEnd;
            if (isFrom)
            {
                from = Math.Max(current.SpanStart, Math.Min(from + delta, to));
                userFrom = from > current.SpanStart ? (int?)from : null;
            }
            else
            {
                to = Math.Min(current.SpanEnd, Math.Max(to + delta, from));
                userTo = to < current.SpanEnd ? (int?)to : null;
            }
            periodAnchor = null;
            Refresh();  // only re-filters, no recomputation
        }

        void SpinButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as FrameworkElement)?.Tag as string)
            {
                case "FromUp": StepPeriod(true, 1); break;
                case "FromDown": StepPeriod(true, -1); break;
                case "ToUp": StepPeriod(false, 1); break;
                case "ToDown": StepPeriod(false, -1); break;
            }
        }

        void PeriodBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.None) return;
            bool isFrom = ReferenceEquals(sender, FromBox);
            switch (e.Key)
            {
                case Key.Enter:
                    CommitPeriod(isFrom);
                    e.Handled = true;
                    break;
                case Key.Escape:
                    UpdatePeriodBoxes();
                    e.Handled = true;
                    break;
                case Key.Up:
                    StepPeriod(isFrom, 1);
                    e.Handled = true;
                    break;
                case Key.Down:
                    StepPeriod(isFrom, -1);
                    e.Handled = true;
                    break;
            }
        }

        void PeriodBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (ready) CommitPeriod(ReferenceEquals(sender, FromBox));
        }

        void PeriodBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            StepPeriod(ReferenceEquals(sender, FromBox), e.Delta > 0 ? 1 : -1);
            e.Handled = true;
        }

        void WholePeriod_Click(object sender, RoutedEventArgs e)
        {
            userFrom = null;
            userTo = null;
            periodAnchor = null;
            if (current != null) Refresh();
        }

        // ---------------------------------------------------------------- names tab

        /// <summary>Text for the flag filters, e.g. "deviating, without missing values". Empty when both are Any.</summary>
        string FlagText()
        {
            var parts = new List<string>();
            if (deviatesFilter == FlagFilter.Yes) parts.Add("deviating");
            if (deviatesFilter == FlagFilter.No) parts.Add("not deviating");
            if (missingFilter == FlagFilter.Yes) parts.Add("with missing values");
            if (missingFilter == FlagFilter.No) parts.Add("without missing values");
            return string.Join(", ", parts);
        }

        /// <summary>The series passing all filters. Those deviating in the selected periods are dark red.</summary>
        void ShowNames()
        {
            if (selection == null || current == null)
            {
                NamesTab.Header = "Names";
                NamesHeader.Text = "Click a number, an area or a bank in the diagram to list its series.";
                SetNamesText(new List<string>(), new HashSet<string>());
                return;
            }
            int from = PeriodFrom, to = PeriodTo;
            bool ignore = IgnoreDimsCheck.IsChecked == true;
            List<string> names = CompareEngine.NamesForDisplay(shown.Select(s => s.Name), ignore);
            List<string> deviating = shown.Where(s => s.DeviatesIn(from, to)).Select(s => s.Name).ToList();
            var red = new HashSet<string>(CompareEngine.NamesForDisplay(deviating, ignore), StringComparer.OrdinalIgnoreCase);

            string flags = FlagText();
            string header = selection.Description + current.CountPeriodText + ": " + shown.Count
                            + (shown.Count == selection.Series.Count ? "" : " of " + selection.Series.Count) + " series"
                            + (flags.Length > 0 ? " " + flags : "");
            if (ignore) header += ", " + names.Count + " names without dimensions";
            if (deviating.Count > 0 && deviatesFilter != FlagFilter.Yes) header += " (" + deviating.Count + " deviating, in dark red)";
            NamesHeader.Text = header;
            NamesHeader.ToolTip = header;
            NamesTab.Header = "Names (" + shown.Count + ")";
            SetNamesText(names, red);
        }

        /// <summary>Comma-separated names; runs of consecutive deviating names become one dark red Run.</summary>
        void SetNamesText(List<string> names, HashSet<string> red)
        {
            var paragraph = new Paragraph { Margin = new Thickness(0) };
            int i = 0;
            while (i < names.Count)
            {
                bool isRed = red.Contains(names[i]);
                var sb = new StringBuilder();
                int j = i;
                while (j < names.Count && red.Contains(names[j]) == isRed)
                {
                    if (j > 0) sb.Append(", ");
                    sb.Append(names[j]);
                    j++;
                }
                var run = new Run(sb.ToString());
                if (isRed) run.Foreground = DeviatingNameBrush;
                paragraph.Inlines.Add(run);
                i = j;
            }
            var document = new FlowDocument(paragraph)
            {
                PagePadding = new Thickness(0),
                FontFamily = NamesText.FontFamily,
                FontSize = NamesText.FontSize
            };
            NamesText.Document = document;
            NamesText.ScrollToHome();
            namesPlainText = string.Join(", ", names);
        }

        void IgnoreDims_Changed(object sender, RoutedEventArgs e)
        {
            if (ready) ShowNames();
        }

        void CopyNames_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(namesPlainText)) return;
            try
            {
                Clipboard.SetText(namesPlainText);
                SetStatus("Names copied to the clipboard.");
            }
            catch (Exception ex)
            {
                SetStatus("Could not copy to the clipboard: " + ex.Message);
            }
        }

        // ---------------------------------------------------------------- values tab

        /// <summary>One series of the Names list, for the selected periods. M = missing, * = deviates.</summary>
        void ShowCurrentSeries()
        {
            bool has = navIndex >= 0 && navIndex < navList.Count;
            PrevButton.IsEnabled = has && navIndex > 0;
            NextButton.IsEnabled = has && navIndex < navList.Count - 1;
            if (!has)
            {
                listing = null;
                NavText.Text = "";
                LegendText.Text = "";
                DevText.Text = devMessage;
                return;
            }

            int from = PeriodFrom, to = PeriodTo;
            SeriesComparison c = navList[navIndex];
            int dev = c.DeviatingPeriods(from, to);
            int miss = c.MissingPeriods(from, to, selection.MissingMask);
            NavText.Text = c.Name + "   (" + (navIndex + 1) + " of " + navList.Count + "; "
                           + (c.Slots.Length >= 2 ? dev + (dev == 1 ? " deviating period, " : " deviating periods, ") : "")
                           + miss + (miss == 1 ? " period" : " periods") + " with M" + current.CountPeriodText + ")";
            LegendText.Text = string.Join("   ", c.Slots.Select(s => "(" + (s + 1) + ") " + current.Banks[s].Name))
                              + "   M = missing   * = deviates";
            listing = CompareEngine.FormatComparison(c, current.Freq, from, to, OnlyFlaggedCheck.IsChecked == true);
            DevText.Text = string.Join("\r\n", listing.Lines);
            DevText.Select(0, 0);
            DevText.ScrollToHome();
        }

        void Navigate(int delta)
        {
            int i = navIndex + delta;
            if (i < 0 || i >= navList.Count) return;
            navIndex = i;
            ShowCurrentSeries();
        }

        void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            Navigate(-1);
        }

        void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Navigate(1);
        }

        void OnlyFlagged_Changed(object sender, RoutedEventArgs e)
        {
            if (ready) ShowCurrentSeries();
        }

        /// <summary>
        /// Alt+1/2/3 focus on a bank (again: show all). Ctrl+Up / Ctrl+Down step through the series in the Values tab;
        /// Ctrl+D hides or shows diagram and controls.
        /// </summary>
        void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Alt)
            {
                // With Alt held, WPF reports the key as Key.System and the real key in SystemKey.
                Key key = e.Key == Key.System ? e.SystemKey : e.Key;
                int slot = key == Key.D1 || key == Key.NumPad1 ? 0
                         : key == Key.D2 || key == Key.NumPad2 ? 1
                         : key == Key.D3 || key == Key.NumPad3 ? 2 : -1;
                if (slot >= 0)
                {
                    ToggleFocus(slot);
                    e.Handled = true;
                }
                return;
            }
            if (Keyboard.Modifiers != ModifierKeys.Control) return;
            if (e.Key == Key.D)
            {
                ToggleDiagram();
                e.Handled = true;
                return;
            }
            if (e.Key != Key.Up && e.Key != Key.Down) return;
            if (navList.Count == 0) return;
            if (!Equals(Tabs.SelectedItem, ValuesTab)) Tabs.SelectedItem = ValuesTab;
            Navigate(e.Key == Key.Down ? 1 : -1);
            e.Handled = true;
        }

        // ---------------------------------------------------------------- diagram toggle

        void DiagramToggle_Click(object sender, RoutedEventArgs e)
        {
            ToggleDiagram();
        }

        void ToggleDiagram()
        {
            diagramHidden = !diagramHidden;
            if (diagramHidden)
            {
                savedVennHeight = VennRow.Height;
                VennRow.MinHeight = 0;
                VennRow.Height = new GridLength(0);
                VennSplitterRow.Height = new GridLength(0);
                DiagramArea.Visibility = Visibility.Collapsed;
                VennSplitter.Visibility = Visibility.Collapsed;
                DiagramToggle.Content = "▾ Show controls";
            }
            else
            {
                DiagramArea.Visibility = Visibility.Visible;
                VennSplitter.Visibility = Visibility.Visible;
                VennRow.Height = savedVennHeight;
                VennRow.MinHeight = 200;
                VennSplitterRow.Height = new GridLength(6);
                DiagramToggle.Content = "▴ Hide controls";
                // "first 2" / "first 3" may have changed while the diagram was hidden.
                UpdateLayout();
                if (current != null) FitVennRow(current.SlotCount);
            }
        }

        // ---------------------------------------------------------------- controls

        void CompareMode_Changed(object sender, RoutedEventArgs e)
        {
            if (!ready || busy) return;
            BanksChanged();
        }

        void FreqCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ready || busy) return;
            Recompute();
        }

        void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ready) ScheduleRecompute();
        }

        void RegexCheck_Changed(object sender, RoutedEventArgs e)
        {
            if (!ready) return;
            FilterHint.Text = RegexCheck.IsChecked == true
                ? "Regular expression, case-insensitive, e.g. ^x\\d+$"
                : "Wildcards * and ?, several patterns separated by spaces";
            Recompute();
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void SetStatus(string text)
        {
            StatusText.Text = text;
        }

        // ---------------------------------------------------------------- dropping files

        void DropZone_DragEnter(object sender, DragEventArgs e)
        {
            RemoveInsertionLine();
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) DropZoneBorder.Stroke = DropHighlight;
        }

        void DropZone_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        void DropZone_DragLeave(object sender, DragEventArgs e)
        {
            DropZoneBorder.Stroke = DropNormal;
        }

        void DropZone_Drop(object sender, DragEventArgs e)
        {
            DropZoneBorder.Stroke = DropNormal;
            AddFiles(e.Data.GetData(DataFormats.FileDrop) as string[], banks.Count);
        }

        /// <summary>Loads the files and inserts them in the list at position insertAt (0-based).</summary>
        void AddFiles(string[] paths, int insertAt)
        {
            if (paths == null || paths.Length == 0) return;
            int at = Math.Max(0, Math.Min(insertAt, banks.Count));
            int added = 0;
            var skipped = new List<string>();
            string failure = null;
            foreach (string path in paths)
            {
                if (Directory.Exists(path) || IsInList(path))
                {
                    skipped.Add(Path.GetFileName(path));
                    continue;
                }
                try
                {
                    banks.Insert(at++, new BankItem(DatabankLoader.Load(path)));
                    TouchRecentBank(path);
                    added++;
                }
                catch (Exception ex)
                {
                    failure = "Could not read " + Path.GetFileName(path) + ": " + ex.Message;
                }
            }
            if (added > 0) BanksChanged();

            string status = added == 1 ? "1 databank added." : added + " databanks added.";
            if (skipped.Count > 0) status += " Skipped folders or banks already in the list: " + string.Join(", ", skipped) + ".";
            if (failure != null) status += " " + failure;
            SetStatus(status);
        }

        void RemoveBank_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement)?.Tag as BankItem;
            if (item != null) RemoveBank(item);
        }

        void BankList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete) return;
            var item = BankList.SelectedItem as BankItem;
            if (item == null || item.IsPlaceholder) return;
            RemoveBank(item);
            e.Handled = true;
        }

        void RemoveBank(BankItem item)
        {
            banks.Remove(item);
            BanksChanged();
            SetStatus(item.Name + " removed.");
        }

        // ---------------------------------------------------------------- reordering the list

        void BankList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragCandidate = null;
            var source = e.OriginalSource as DependencyObject;
            if (FindAncestor<Button>(source) != null) return;
            var row = FindAncestor<ListViewItem>(source);
            if (row == null) return;
            dragCandidate = row.DataContext as BankItem;
            if (dragCandidate != null && dragCandidate.IsPlaceholder) dragCandidate = null;
            dragStart = e.GetPosition(BankList);
        }

        void BankList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (dragCandidate == null || e.LeftButton != MouseButtonState.Pressed) return;
            Vector moved = e.GetPosition(BankList) - dragStart;
            if (Math.Abs(moved.X) < SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(moved.Y) < SystemParameters.MinimumVerticalDragDistance) return;
            BankItem item = dragCandidate;
            dragCandidate = null;
            item.IsDragging = true;
            try
            {
                DragDrop.DoDragDrop(BankList, new DataObject(DragFormat, item), DragDropEffects.Move);
            }
            finally
            {
                item.IsDragging = false;
                RemoveInsertionLine();
            }
        }

        void BankList_DragOver(object sender, DragEventArgs e)
        {
            bool isRow = e.Data.GetDataPresent(DragFormat);
            bool isFile = !isRow && e.Data.GetDataPresent(DataFormats.FileDrop);
            e.Effects = isRow ? DragDropEffects.Move : isFile ? DragDropEffects.Copy : DragDropEffects.None;
            if (isRow || isFile) ShowInsertionLine(InsertionIndex(e.GetPosition(BankList)));
            e.Handled = true;
        }

        void BankList_DragLeave(object sender, DragEventArgs e)
        {
            // DragLeave also arrives when moving between rows; only remove the line when really leaving the list.
            Point p = e.GetPosition(BankList);
            if (p.X <= 1 || p.Y <= 1 || p.X >= BankList.ActualWidth - 1 || p.Y >= BankList.ActualHeight - 1) RemoveInsertionLine();
        }

        void BankList_Drop(object sender, DragEventArgs e)
        {
            int gap = InsertionIndex(e.GetPosition(BankList));
            RemoveInsertionLine();
            e.Handled = true;
            if (e.Data.GetDataPresent(DragFormat))
            {
                var item = e.Data.GetData(DragFormat) as BankItem;
                int from = banks.IndexOf(item);
                if (item == null || from < 0) return;
                int to = gap > from ? gap - 1 : gap;
                if (to == from) return;
                banks.Move(from, to);
                BankList.SelectedItem = item;
                BanksChanged();
                SetStatus(item.Name + " moved to position " + (to + 1) + ".");
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                AddFiles(e.Data.GetData(DataFormats.FileDrop) as string[], gap);
            }
        }

        /// <summary>0..Count: the dragged row lands before the row with this index (Count = at the end).</summary>
        int InsertionIndex(Point position)
        {
            for (int i = 0; i < banks.Count; i++)
            {
                var row = BankList.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
                if (row == null) continue;
                Point top = row.TranslatePoint(new Point(0, 0), BankList);
                if (position.Y < top.Y + row.ActualHeight / 2) return i;
            }
            return banks.Count;
        }

        void ShowInsertionLine(int gap)
        {
            if (insertionLine == null)
            {
                var layer = AdornerLayer.GetAdornerLayer(BankList);
                if (layer == null) return;
                insertionLine = new InsertionLineAdorner(BankList);
                layer.Add(insertionLine);
            }

            bool below = gap >= banks.Count;
            int index = below ? banks.Count - 1 : gap;
            var row = index >= 0 ? BankList.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem : null;
            if (row == null)
            {
                insertionLine.HideLine();
                return;
            }
            Point top = row.TranslatePoint(new Point(0, 0), BankList);
            double y = below ? top.Y + row.ActualHeight : top.Y;
            double left = Math.Max(3, top.X);
            double right = Math.Min(BankList.ActualWidth - 3, top.X + row.ActualWidth);
            insertionLine.ShowLine(y, left, right);
        }

        void RemoveInsertionLine()
        {
            if (insertionLine == null) return;
            var layer = AdornerLayer.GetAdornerLayer(BankList);
            if (layer != null) layer.Remove(insertionLine);
            insertionLine = null;
        }

        static T FindAncestor<T>(DependencyObject d) where T : DependencyObject
        {
            while (d != null && !(d is T))
            {
                d = d is Visual || d is System.Windows.Media.Media3D.Visual3D
                    ? VisualTreeHelper.GetParent(d)
                    : LogicalTreeHelper.GetParent(d);
            }
            return d as T;
        }
    }
}
