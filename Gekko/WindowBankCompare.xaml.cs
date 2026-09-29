using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        public CompareBank Bank { get; }
        public string Name => Bank.Name;
        public string PathText => Bank.FilePath ?? "(artificial data)";

        /// <summary>E.g. "A 380, Q 200, M 70".</summary>
        public string SeriesText
        {
            get { return string.Join(", ", Bank.Frequencies.Select(f => f.ToString().Substring(0, 1) + " " + Bank.Count(f))); }
        }

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

        /// <summary>True when the bank is compared but hidden by clicking its label in the diagram.</summary>
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
        public double ItemOpacity => InComparison ? 1.0 : 0.6;

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

        readonly ObservableCollection<BankItem> banks = new ObservableCollection<BankItem>();
        readonly IEqualityCriterion criterion = new SimpleEqualityCriterion();
        readonly bool[] active = { true, true, true };
        readonly DispatcherTimer recomputeTimer;
        bool ready;
        bool busy;

        VennResult current;
        int selectedMask;
        bool selectedDeviations;
        VennArea selectedArea;
        List<SeriesComparison> navList = new List<SeriesComparison>();
        int navIndex = -1;
        int lastBarPeriod = int.MinValue;
        DeviationListing listing;
        const string DefaultDevMessage = "Click a deviation number in the diagram to list values and deviations.";
        string devMessage = DefaultDevMessage;

        // Chosen first/last period in the selected frequency. Null means: follow the universal period.
        int? userFrom;
        int? userTo;
        CompareFreq? periodFreq;

        bool diagramHidden;
        GridLength savedVennHeight = new GridLength(1.25, GridUnitType.Star);

        Point dragStart;
        BankItem dragCandidate;
        InsertionLineAdorner insertionLine;

        public WindowBankCompare()
        {
            InitializeComponent();
            BankList.ItemsSource = banks;

            recomputeTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            recomputeTimer.Tick += (s, e) => Recompute();

            Venn.AreaClicked += Venn_AreaClicked;
            Venn.BankClicked += Venn_BankClicked;
            Histogram.BarClicked += Histogram_BarClicked;

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

        void Recompute()
        {
            if (!ready) return;
            recomputeTimer.Stop();

            CompareFreq? freq = SelectedFreq;
            if (banks.Count < 2 || !freq.HasValue)
            {
                current = null;
                Venn.SetResult(null, banks.Count == 0 ? "Drag databank files into the drop field to begin."
                                   : banks.Count == 1 ? "Add one more databank to compare."
                                   : "The compared databanks contain no series.");
                selectedMask = 0;
                ClearDetails();
                UpdatePeriodBoxes();
                UpdateSelectionText();
                UpdateListBadges();
                return;
            }

            // Keep the chosen periods when switching frequency (2010q3 becomes 2010m7 or 2010, and so on).
            if (periodFreq.HasValue && periodFreq.Value != freq.Value)
            {
                if (userFrom.HasValue) userFrom = PeriodText.ChangeFrequency(userFrom.Value, periodFreq.Value, freq.Value, false);
                if (userTo.HasValue) userTo = PeriodText.ChangeFrequency(userTo.Value, periodFreq.Value, freq.Value, true);
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
            current = CompareEngine.Compute(compared, active, freq.Value, filter, criterion,
                                            userFrom ?? int.MinValue, userTo ?? int.MaxValue);
            Venn.SetResult(current, null);

            VennArea area;
            if (selectedMask != 0 && current.Areas.TryGetValue(selectedMask, out area))
            {
                Venn.SetSelection(selectedMask, selectedDeviations);
                ShowArea(area, true, false);
            }
            else
            {
                selectedMask = 0;
                Venn.SetSelection(0, false);
                ClearDetails();
            }
            UpdatePeriodBoxes();
            UpdateSelectionText();
            UpdateListBadges();
        }

        void UpdateListBadges()
        {
            for (int i = 0; i < banks.Count; i++)
                banks[i].HiddenInDiagram = current != null && i < current.SlotCount && !current.Active[i];
        }

        // ---------------------------------------------------------------- diagram clicks

        void Venn_AreaClicked(int mask, bool deviations)
        {
            VennArea area;
            if (current == null || !current.Areas.TryGetValue(mask, out area)) return;
            bool sameArea = mask == selectedMask;
            selectedMask = mask;
            selectedDeviations = deviations;
            Venn.SetSelection(mask, deviations);
            ShowArea(area, sameArea, true);
            UpdateSelectionText();
        }

        /// <summary>
        /// Clicking a shown bank hides it (as long as another bank is still shown); clicking a hidden bank shows it.
        /// Hiding one bank never brings back another one.
        /// </summary>
        void Venn_BankClicked(int slot)
        {
            if (current == null || current.SlotCount != 3) return;
            string label = "Bank " + (slot + 1) + " (" + current.Banks[slot].Name + ")";
            int oldActiveMask = current.ActiveMask;
            if (!active[slot])
            {
                active[slot] = true;
                SetStatus(label + " is shown again.");
            }
            else if (current.ActiveCount > 1)
            {
                active[slot] = false;
                SetStatus(label + " is hidden. Click its label to show it again.");
            }
            else
            {
                SetStatus(label + " is the only bank shown, so it cannot be hidden.");
                return;
            }

            int newActiveMask = 0;
            for (int s = 0; s < 3; s++) if (active[s]) newActiveMask |= 1 << s;
            // Keep a sensible selection: "in all shown banks" stays "in all shown banks".
            selectedMask = selectedMask == oldActiveMask ? newActiveMask : selectedMask & newActiveMask;
            Recompute();
        }

        void ShowArea(VennArea area, bool keepSeries, bool switchTab)
        {
            selectedArea = area;
            ShowNames();

            string description = CompareEngine.DescribeArea(current, area.Mask);
            string keepName = keepSeries && navIndex >= 0 && navIndex < navList.Count ? navList[navIndex].Name : null;

            if (area.IsComparison && area.Deviations.Count > 0)
            {
                string what = area.Slots.Length == 3
                    ? "number of series not equal in all three banks, per period"
                    : "number of deviating series, per period";
                Histogram.SetData(description + ": " + what, current.Freq, current.WindowStart,
                                  CompareEngine.Histogram(area, current.WindowStart, current.WindowEnd), true);
                navList = area.Deviations;
                navIndex = 0;
                if (keepName != null)
                {
                    int i = navList.FindIndex(c => string.Equals(c.Name, keepName, StringComparison.OrdinalIgnoreCase));
                    if (i >= 0) navIndex = i;
                }
            }
            else if (!area.IsComparison)
            {
                // One bank only: nothing to compare, so show how many of the series have an observation per period.
                if (current.HasWindow)
                    Histogram.SetData(description + ": number of series with an observation (not missing), per period",
                                      current.Freq, current.WindowStart,
                                      CompareEngine.ObservationCounts(current, area, current.WindowStart, current.WindowEnd), false);
                else
                    Histogram.ShowMessage("No periods to show.");
                navList = new List<SeriesComparison>();
                navIndex = -1;
            }
            else
            {
                Histogram.ShowMessage("No deviations in this area.");
                navList = new List<SeriesComparison>();
                navIndex = -1;
            }
            lastBarPeriod = int.MinValue;
            devMessage = area.IsComparison
                ? "No deviations in this area."
                : "Series that exist in only one bank have nothing to be compared with.";
            ShowCurrentSeries(null);

            // Deviation numbers and one-bank areas open the histogram; other totals open the name list.
            if (switchTab) Tabs.SelectedItem = selectedDeviations || !area.IsComparison ? HistogramTab : NamesTab;
        }

        void ClearDetails()
        {
            selectedArea = null;
            devMessage = DefaultDevMessage;
            ShowNames();
            Histogram.ShowMessage("Click a deviation number in the diagram to see deviations per period.");
            navList = new List<SeriesComparison>();
            navIndex = -1;
            ShowCurrentSeries(null);
        }

        /// <summary>When the diagram is hidden, the selected area is summarized next to the toggle button.</summary>
        void UpdateSelectionText()
        {
            VennArea area = null;
            if (current != null && selectedMask != 0) current.Areas.TryGetValue(selectedMask, out area);
            if (!diagramHidden || area == null)
            {
                SelectionText.Visibility = Visibility.Collapsed;
                return;
            }
            SelectionText.Text = "Selected: " + CompareEngine.SummarizeArea(current, selectedMask);
            SelectionText.ToolTip = SelectionText.Text;
            SelectionText.Visibility = Visibility.Visible;
        }

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
                DiagramToggle.Content = "\u25BE Show controls";
            }
            else
            {
                DiagramArea.Visibility = Visibility.Visible;
                VennSplitter.Visibility = Visibility.Visible;
                VennRow.Height = savedVennHeight;
                VennRow.MinHeight = 200;
                VennSplitterRow.Height = new GridLength(6);
                DiagramToggle.Content = "\u25B4 Hide controls";
            }
            UpdateSelectionText();
        }

        // ---------------------------------------------------------------- period from/to

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
            if (oldFrom != userFrom || oldTo != userTo) Recompute();
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
            UpdatePeriodBoxes();
            ScheduleRecompute();  // holding an arrow down steps quickly, so recompute once it settles
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
            Recompute();
        }

        // ---------------------------------------------------------------- names tab

        void ShowNames()
        {
            if (selectedArea == null || current == null)
            {
                NamesHeader.Text = "Click a number in the diagram to list the series in that area.";
                NamesText.Text = "";
                return;
            }
            bool ignore = IgnoreDimsCheck.IsChecked == true;
            List<string> names = CompareEngine.NamesForDisplay(selectedArea.Names, ignore);
            string header = CompareEngine.DescribeArea(current, selectedArea.Mask) + ": " + selectedArea.Names.Count + " series";
            if (ignore) header += ", " + names.Count + " names without dimensions";
            NamesHeader.Text = header;
            NamesText.Text = string.Join(", ", names);
            NamesText.ScrollToHome();
        }

        void IgnoreDims_Changed(object sender, RoutedEventArgs e)
        {
            if (ready) ShowNames();
        }

        void CopyNames_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NamesText.Text)) return;
            try
            {
                Clipboard.SetText(NamesText.Text);
                SetStatus("Names copied to the clipboard.");
            }
            catch (Exception ex)
            {
                SetStatus("Could not copy to the clipboard: " + ex.Message);
            }
        }

        // ---------------------------------------------------------------- deviations tab

        void ShowCurrentSeries(int? focusPeriod)
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

            SeriesComparison c = navList[navIndex];
            NavText.Text = c.Name + "   (" + (navIndex + 1) + " of " + navList.Count + ", "
                           + c.DeviatingPeriods + (c.DeviatingPeriods == 1 ? " deviating period)" : " deviating periods)");
            LegendText.Text = string.Join("   ", c.Slots.Select(s => "(" + (s + 1) + ") " + current.Banks[s].Name))
                              + "   M = no observation   * = deviates";
            listing = CompareEngine.FormatComparison(c, current.Freq, OnlyDevCheck.IsChecked == true);
            DevText.Text = string.Join("\r\n", listing.Lines);

            int line;
            if (focusPeriod.HasValue && listing.LineOfPeriod.TryGetValue(focusPeriod.Value, out line)) SelectLine(line);
            else
            {
                DevText.Select(0, 0);
                DevText.ScrollToHome();
            }
        }

        void SelectLine(int line)
        {
            int charIndex = 0;
            for (int i = 0; i < line; i++) charIndex += listing.Lines[i].Length + 2;
            DevText.Select(charIndex, listing.Lines[line].Length);
            // Scrolling needs layout, which is not done yet if the tab was just switched.
            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                try { DevText.ScrollToLine(line); }
                catch (ArgumentOutOfRangeException) { }
            }));
        }

        void Navigate(int delta)
        {
            int i = navIndex + delta;
            if (i < 0 || i >= navList.Count) return;
            navIndex = i;
            ShowCurrentSeries(null);
        }

        void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            Navigate(-1);
        }

        void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Navigate(1);
        }

        void OnlyDev_Changed(object sender, RoutedEventArgs e)
        {
            if (ready) ShowCurrentSeries(null);
        }

        /// <summary>Ctrl+Up / Ctrl+Down step through the deviating series; Ctrl+D hides or shows diagram and controls.</summary>
        void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control) return;
            if (e.Key == Key.D)
            {
                ToggleDiagram();
                e.Handled = true;
                return;
            }
            if (e.Key != Key.Up && e.Key != Key.Down) return;
            if (navList.Count == 0) return;
            if (!Equals(Tabs.SelectedItem, DeviationsTab)) Tabs.SelectedItem = DeviationsTab;
            Navigate(e.Key == Key.Down ? 1 : -1);
            e.Handled = true;
        }

        /// <summary>Clicking a bar shows the first series deviating in that period; clicking it again shows the next.</summary>
        void Histogram_BarClicked(int period)
        {
            if (navList.Count == 0) return;
            int from = period == lastBarPeriod ? navIndex + 1 : 0;
            int found = -1;
            for (int k = 0; k < navList.Count; k++)
            {
                int i = (from + k) % navList.Count;
                if (navList[i].DeviatesAt(period))
                {
                    found = i;
                    break;
                }
            }
            if (found < 0) return;

            lastBarPeriod = period;
            navIndex = found;
            Tabs.SelectedItem = DeviationsTab;
            ShowCurrentSeries(period);
            int count = navList.Count(c => c.DeviatesAt(period));
            SetStatus(count + (count == 1 ? " series deviates" : " series deviate") + " in " + PeriodText.Format(current.Freq, period)
                      + ". Showing " + navList[found].Name + (count > 1 ? "; click the bar again for the next one." : "."));
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
                if (Directory.Exists(path) || banks.Any(b => string.Equals(b.Bank.FilePath, path, StringComparison.OrdinalIgnoreCase)))
                {
                    skipped.Add(Path.GetFileName(path));
                    continue;
                }
                try
                {
                    banks.Insert(at++, new BankItem(DatabankLoader.Load(path)));
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
            if (item == null) return;
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
                var layer = System.Windows.Documents.AdornerLayer.GetAdornerLayer(BankList);
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
            var layer = System.Windows.Documents.AdornerLayer.GetAdornerLayer(BankList);
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
