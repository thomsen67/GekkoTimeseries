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
        int position;
        bool inComparison;

        public BankItem(CompareBank bank)
        {
            Bank = bank;
        }

        public CompareBank Bank { get; }
        public string Name => Bank.Name;

        public string Info
        {
            get
            {
                string where = Bank.FilePath ?? "artificial data";
                IEnumerable<string> parts = Bank.Frequencies.Select(f => Bank.Count(f) + " " + f.ToString().ToLowerInvariant());
                return where + "   (" + string.Join(", ", parts) + ")";
            }
        }

        public int Position
        {
            get { return position; }
            set
            {
                if (position == value) return;
                position = value;
                Raise(nameof(Position));
                Raise(nameof(BadgeBrush));
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
                Raise(nameof(BadgeBrush));
                Raise(nameof(ItemOpacity));
            }
        }

        public Brush BadgeBrush => BankColors.Solid(InComparison ? Position - 1 : -1);
        public double ItemOpacity => InComparison ? 1.0 : 0.55;

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
        static readonly Brush InvalidFilterBack = BankColors.FromRgb(0xFB, 0xE3, 0xE3);

        readonly ObservableCollection<BankItem> banks = new ObservableCollection<BankItem>();
        readonly IEqualityCriterion criterion = new SimpleEqualityCriterion();
        readonly bool[] active = { true, true, true };
        readonly DispatcherTimer filterTimer;
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

        Point dragStart;
        BankItem dragCandidate;

        public WindowBankCompare()
        {
            InitializeComponent();
            BankList.ItemsSource = banks;

            filterTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            filterTimer.Tick += (s, e) => Recompute();

            Venn.AreaClicked += Venn_AreaClicked;
            Venn.BankClicked += Venn_BankClicked;
            Histogram.BarClicked += Histogram_BarClicked;

            // Artificial demo banks. Dropped files also get artificial data, see DatabankLoader.
            banks.Add(new BankItem(ArtificialData.CreateBank("base_2024.gbk", null)));
            banks.Add(new BankItem(ArtificialData.CreateBank("base_2025.gbk", null)));
            banks.Add(new BankItem(ArtificialData.CreateBank("forecast_2025.gbk", null)));

            ready = true;
            BanksChanged();
            ClearDetails();
            SetStatus("Three databanks with artificial data are loaded. Drop files to add banks, drag list items to reorder.");
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

        void Recompute()
        {
            if (!ready) return;
            filterTimer.Stop();

            if (banks.Count < 2)
            {
                current = null;
                Venn.SetResult(null, banks.Count == 0 ? "Drag databank files into the drop field to begin." : "Add one more databank to compare.");
                ClearDetails();
                return;
            }
            CompareFreq? freq = SelectedFreq;
            if (!freq.HasValue)
            {
                current = null;
                Venn.SetResult(null, "The compared databanks contain no series.");
                ClearDetails();
                return;
            }

            string error;
            Func<string, bool> filter = CompareEngine.BuildFilter(FilterBox.Text, RegexCheck.IsChecked == true, out error);
            if (error == null) FilterBox.ClearValue(Control.BackgroundProperty);
            else
            {
                FilterBox.Background = InvalidFilterBack;
                SetStatus(error);
            }
            FilterBox.ToolTip = error;

            int n = SlotCount;
            current = CompareEngine.Compute(banks.Take(n).Select(b => b.Bank).ToList(), active, freq.Value, filter, criterion);
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
        }

        /// <summary>
        /// Clicking an active bank hides it. When only two are shown, clicking one of them swaps it
        /// with the hidden bank, so 1-2, 1-3 and 2-3 can be compared with single clicks.
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
            else if (current.ActiveCount == 3)
            {
                active[slot] = false;
                SetStatus(label + " is hidden. Click its label to show it again.");
            }
            else
            {
                for (int s = 0; s < 3; s++) active[s] = true;
                active[slot] = false;
                SetStatus(label + " is hidden, and the previously hidden bank is shown.");
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
                Histogram.SetData(description + ": " + what, current.Freq, current.SpanStart,
                                  CompareEngine.Histogram(area, current.SpanStart, current.SpanEnd));
                navList = area.Deviations;
                navIndex = 0;
                if (keepName != null)
                {
                    int i = navList.FindIndex(c => string.Equals(c.Name, keepName, StringComparison.OrdinalIgnoreCase));
                    if (i >= 0) navIndex = i;
                }
            }
            else
            {
                Histogram.ShowMessage(area.IsComparison
                    ? "No deviations in this area."
                    : "Series that exist in only one bank have nothing to be compared with.");
                navList = new List<SeriesComparison>();
                navIndex = -1;
            }
            lastBarPeriod = int.MinValue;
            ShowCurrentSeries(null);

            if (switchTab) Tabs.SelectedItem = selectedDeviations ? HistogramTab : NamesTab;
        }

        void ClearDetails()
        {
            selectedArea = null;
            ShowNames();
            Histogram.ShowMessage("Click a deviation number in the diagram to see deviations per period.");
            navList = new List<SeriesComparison>();
            navIndex = -1;
            ShowCurrentSeries(null);
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
                DevText.Text = "Click a deviation number in the diagram to list values and deviations.";
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

        /// <summary>Ctrl+Up / Ctrl+Down step through the deviating series from anywhere in the window.</summary>
        void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control) return;
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
            if (!ready) return;
            filterTimer.Stop();
            filterTimer.Start();
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
            AddFiles(e.Data.GetData(DataFormats.FileDrop) as string[]);
        }

        void AddFiles(string[] paths)
        {
            if (paths == null || paths.Length == 0) return;
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
                    banks.Add(new BankItem(DatabankLoader.Load(path)));
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
            if (item == null) return;
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
            var container = FindAncestor<ListBoxItem>(source);
            if (container == null) return;
            dragCandidate = container.DataContext as BankItem;
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
            DragDrop.DoDragDrop(BankList, new DataObject(DragFormat, item), DragDropEffects.Move);
        }

        void BankList_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DragFormat)) e.Effects = DragDropEffects.Move;
            else if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effects = DragDropEffects.Copy;
            else e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        void BankList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DragFormat))
            {
                var item = e.Data.GetData(DragFormat) as BankItem;
                var target = FindAncestor<ListBoxItem>(e.OriginalSource as DependencyObject);
                int from = banks.IndexOf(item);
                int to = target != null ? banks.IndexOf(target.DataContext as BankItem) : banks.Count - 1;
                if (item == null || from < 0 || to < 0 || from == to) return;
                banks.Move(from, to);
                BankList.SelectedItem = item;
                BanksChanged();
                SetStatus(item.Name + " moved to position " + (to + 1) + ".");
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                AddFiles(e.Data.GetData(DataFormats.FileDrop) as string[]);
            }
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
