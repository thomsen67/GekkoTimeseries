using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    /// <summary>One row in the numbered list: a databank (Datafiles state) or a folder (Folders state).</summary>
    public sealed class BankItem : INotifyPropertyChanged
    {
        static readonly Brush GreyNumber = BankColors.FromRgb(0xB4, 0xB4, 0xB4);

        int position;
        bool inComparison;
        bool hiddenInDiagram;
        bool isDragging;
        string contents;

        public BankItem(CompareBank bank)
        {
            Bank = bank;
        }

        /// <summary>A row for a folder (Folders state).</summary>
        public static BankItem ForFolder(string path)
        {
            return new BankItem((CompareBank)null) { FolderPath = path, IsFolderRow = true };
        }

        /// <summary>The empty row always at the end of the list (no bank or folder). Its [Recent] button adds one.</summary>
        public static BankItem CreatePlaceholder(bool folderList)
        {
            return new BankItem((CompareBank)null) { IsFolderRow = folderList };
        }

        public CompareBank Bank { get; }
        public string FolderPath { get; private set; }
        public bool IsFolderRow { get; private set; }
        public bool IsPlaceholder => Bank == null && FolderPath == null;

        /// <summary>The file or folder path that identifies the row (null for artificial data and the empty row).</summary>
        public string SourcePath => FolderPath ?? Bank?.FilePath;

        public string Name => Bank != null ? Bank.Name : FolderPath != null ? FolderResult.FolderName(FolderPath) : "";

        public string PathText
        {
            get
            {
                if (IsPlaceholder) return IsFolderRow ? "(empty: use Recent, or drop a folder here)" : "(empty: use Recent, or drop a file here)";
                return FolderPath ?? Bank.FilePath ?? "(artificial data)";
            }
        }

        /// <summary>Banks: e.g. "A 380, Q 200, M 70". Folders: e.g. "812 data files" (set when compared).</summary>
        public string SeriesText
        {
            get
            {
                if (IsPlaceholder) return "";
                if (Bank == null) return contents ?? "";
                return string.Join(", ", Bank.Frequencies.Select(f => f.ToString().Substring(0, 1) + " " + Bank.Count(f)));
            }
        }

        public void SetContents(string text)
        {
            contents = text;
            Raise(nameof(SeriesText));
        }

        /// <summary>The number badge and the remove (X) button are not shown on the empty row.</summary>
        public Visibility RowVisibility => IsPlaceholder ? Visibility.Collapsed : Visibility.Visible;

        public string RecentTip
        {
            get
            {
                string what = IsFolderRow ? "folder" : "databank";
                return IsPlaceholder
                    ? "Add one of the most recently compared " + what + "s"
                    : "Replace this " + what + " with one of the most recently compared " + what + "s";
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
                Raise(nameof(NumberBrush));
            }
        }

        /// <summary>True for the first 2 or 3 rows, which are the ones in the diagram.</summary>
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

        /// <summary>True when the row is compared but hidden (eye icon) or greyed out by a focus on another one.</summary>
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

        /// <summary>The number badge has the circle's color for compared rows that are shown, grey otherwise.</summary>
        public Brush NumberBrush => InComparison && !HiddenInDiagram ? BankColors.Solid(Position - 1) : GreyNumber;
        public double ItemOpacity => InComparison || IsPlaceholder ? 1.0 : 0.6;

        public event PropertyChangedEventHandler PropertyChanged;

        void Raise(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    /// <summary>One line in the Files tab (Folders state).</summary>
    public sealed class FileRow
    {
        public FileComparison File { get; set; }
        public string Text { get; set; }
        public Brush Foreground { get; set; }
        public string Tip { get; set; }
        public Cursor Cursor { get; set; }
    }

    /// <summary>
    /// Compares 2 or 3 databanks ("Datafiles" state: series inside) or 2 or 3 folders ("Folders" state: data files
    /// inside). Both states share the list at the top, the Venn diagram, the controls and the tabs; each state keeps
    /// its own list, comparison and selection, so switching back and forth loses nothing. Clicking a file found in
    /// several folders opens those files in the Datafiles state.
    /// </summary>
    public partial class WindowBankCompare : Window
    {
        enum CompareState { Datafiles, Folders }

        /// <summary>The view settings of one state, swapped in and out when switching state.</summary>
        sealed class ViewState
        {
            public int SelectedMask;
            public int SelectedBank = -1;
            public FlagFilter Found = FlagFilter.Any;
            public int FocusSlot = -1;
            public bool[] ActiveBeforeFocus;
            public bool[] Active = { true, true, true };
        }

        const string DragFormat = "GekkoBankCompareItem";
        static readonly Brush DropHighlight = BankColors.FromRgb(0x2E, 0x6D, 0xB4);
        static readonly Brush DropNormal = BankColors.FromRgb(0x99, 0x99, 0x99);
        static readonly Brush InvalidBack = BankColors.FromRgb(0xFB, 0xE3, 0xE3);
        static readonly Brush AutoPeriodBrush = BankColors.FromRgb(0x88, 0x88, 0x88);
        // Found names/files in the Names/Files tab, same dark red as the red numbers in the diagram.
        static readonly Brush DeviatingNameBrush = BankColors.DeviationText;
        static readonly Brush RecentPathBrush = BankColors.FromRgb(0x88, 0x88, 0x88);
        static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        // Length of the "Recent" lists (LRU, stored in the user settings).
        const int MaxRecentBanks = 20;
        const char RecentSeparator = '|';  // cannot occur in a Windows path

        // ---- The two states. 'banks' is the list of the current state, so the list code serves both.
        CompareState state = CompareState.Datafiles;
        readonly ObservableCollection<BankItem> bankItems = new ObservableCollection<BankItem>();
        readonly ObservableCollection<BankItem> folderItems = new ObservableCollection<BankItem>();
        readonly CompositeCollection bankListSource;
        readonly CompositeCollection folderListSource;
        readonly ViewState datafilesView = new ViewState();
        readonly ViewState foldersView = new ViewState();
        // What a state needs when it is shown again, after changes made while the other state was shown:
        // DirtyRecompute (the filter or a threshold changed) or DirtyList (its list changed: BanksChanged).
        const int DirtyNone = 0, DirtyRecompute = 1, DirtyList = 2;
        int datafilesDirty;
        int foldersDirty;

        ObservableCollection<BankItem> banks => state == CompareState.Folders ? folderItems : bankItems;
        ViewState View => state == CompareState.Folders ? foldersView : datafilesView;
        IVennModel Model => state == CompareState.Folders ? (IVennModel)folderResult : current;

        // ---- Settings shared by both states (the panel to the right of the diagram): compare first 2 or 3,
        // the check and its thresholds, and the name filter. Frequency and period are used by the Datafiles state;
        // the Folders state checks all frequencies and periods of each file.
        readonly CheckSettings settings = new CheckSettings();
        bool compare3Choice = true;  // the user's choice; "first 3" is used when the list has 3 or more
        bool[] active = { true, true, true };
        readonly DispatcherTimer recomputeTimer;
        bool ready;
        bool busy;

        VennResult current;          // Datafiles state
        FolderResult folderResult;   // Folders state
        CancellationTokenSource folderCts;
        int folderGeneration;
        bool folderComputing;
        string folderStatus;   // the last folder comparison's summary (or error), shown again with the Folders state
        bool folderFailed;     // the last folder comparison failed: do not start it again just by showing the state
        // DatabankLoader.Load is called both here (UI thread) and by the folder comparison (worker thread); this lock
        // makes sure only one file is read at a time, in case the loader uses shared state.
        static readonly object loaderLock = new object();

        static CompareBank LoadBank(string path)
        {
            lock (loaderLock) return DatabankLoader.Load(path);
        }

        // Summaries of data files and byte comparisons from earlier folder comparisons (keyed by path, size, time, filter).
        readonly ConcurrentDictionary<string, object> folderCache = new ConcurrentDictionary<string, object>();
        FileComparison openedFrom;   // the folder file the datafiles were opened from, if any

        // ---- Scope: which banks/folders are shown (eye icons) and the focus (target icon, Alt+1/2/3).
        // focusSlot is the only one shown, or -1. activeBeforeFocus is the hide/show state to return to.
        int focusSlot = -1;
        bool[] activeBeforeFocus;

        // ---- The check (tabs at the bottom of the controls panel, shared by both states) and the drill-down filters.
        //   Check:  Differences, Missings or Zeroes.
        //   Area:   one area (selectedMask != 0) or a whole bank/folder (selectedBank >= 0), or nothing.
        //   Period: userFrom..userTo (the From/To boxes, Datafiles only); null means the edge of the universal period.
        //   Found:  foundFilter, Any or Yes (the red number / a histogram bar): only what the check finds.
        CheckKind check = CheckKind.Differences;
        int selectedMask;
        int selectedBank = -1;
        FlagFilter foundFilter = FlagFilter.Any;
        int? userFrom;
        int? userTo;
        int? periodAnchor;  // the first bar clicked, for Shift+click ranges
        CompareFreq? periodFreq;

        Selection selection;                                              // Datafiles: the area filter, resolved
        List<SeriesComparison> shown = new List<SeriesComparison>();      // Datafiles: series passing all filters
        FileSelection fileSelection;                                      // Folders: the area filter, resolved
        List<FileComparison> shownFiles = new List<FileComparison>();     // Folders: files passing all filters

        List<SeriesComparison> navList = new List<SeriesComparison>();
        int navIndex = -1;
        List<FileComparison> fileNav = new List<FileComparison>();
        int fileNavIndex = -1;
        DeviationListing listing;
        const string NoSelectionMessage = "Click a number, an area or a box in the diagram.";
        string devMessage = NoSelectionMessage;

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
            bankListSource = new CompositeCollection
            {
                new CollectionContainer { Collection = bankItems },
                BankItem.CreatePlaceholder(false)
            };
            folderListSource = new CompositeCollection
            {
                new CollectionContainer { Collection = folderItems },
                BankItem.CreatePlaceholder(true)
            };
            BankList.ItemsSource = bankListSource;
            RestoreWindowPlacement();

            recomputeTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            recomputeTimer.Tick += (s, e) => Recompute();

            Venn.AreaClicked += Venn_AreaClicked;
            Venn.BankSelected += Venn_BankSelected;
            Venn.HideClicked += ToggleHidden;
            Venn.FocusClicked += ToggleFocus;
            Histogram.BarClicked += Bar_Clicked;
            Histogram.BarDoubleClicked += period => Tabs.SelectedItem = NamesTab;

            ReadThresholds();

            // Artificial demo banks. Dropped files also get artificial data, see DatabankLoader.
            bankItems.Add(new BankItem(ArtificialData.CreateBank("base_2024.gbk", null)));
            bankItems.Add(new BankItem(ArtificialData.CreateBank("base_2025.gbk", null)));
            bankItems.Add(new BankItem(ArtificialData.CreateBank("forecast_2025.gbk", null)));

            ready = true;
            UpdateStateControls();
            BanksChanged();
            SetStatus("Three databanks with artificial data are loaded. Drop files or folders to compare them, drag rows to reorder.");
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

            // The saved height is for 3 circles; shrink it if the window starts out comparing 2.
            UpdateLayout();
            if (Model != null) FitVennRow(Model.SlotCount);
        }

        /// <summary>
        /// Going from 3 to 2 circles removes the room the bottom circle took, and gives it to the tabs
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
            folderCts?.Cancel();
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

        // ---------------------------------------------------------------- the two states

        void StateButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!ready || busy) return;
            SwitchState(ReferenceEquals(sender, FoldersStateButton) ? CompareState.Folders : CompareState.Datafiles);
        }

        /// <summary>
        /// Switches between comparing databanks and comparing folders. Each state keeps its own list, comparison,
        /// selection, hidden banks/folders and check, so going back shows the same as before.
        /// </summary>
        void SwitchState(CompareState to)
        {
            if (to == state) return;
            if (recomputeTimer.IsEnabled)
            {
                // A recompute was pending (typing in the filter or a threshold box): do it when this state is shown again.
                recomputeTimer.Stop();
                MarkDirty(state, DirtyRecompute);
            }
            SaveView();
            state = to;
            LoadView();

            busy = true;
            try
            {
                BankList.ItemsSource = state == CompareState.Folders ? folderListSource : bankListSource;
                DatafilesStateButton.IsChecked = state == CompareState.Datafiles;
                FoldersStateButton.IsChecked = state == CompareState.Folders;
                Compare3Radio.IsEnabled = banks.Count >= 3;
                bool three = compare3Choice && banks.Count >= 3;
                Compare3Radio.IsChecked = three;
                Compare2Radio.IsChecked = !three;
            }
            finally
            {
                busy = false;
            }
            UpdateStateControls();

            int dirty = state == CompareState.Folders ? foldersDirty : datafilesDirty;
            if (dirty == DirtyList) BanksChanged();
            else if (dirty == DirtyRecompute) Recompute();
            else if (state == CompareState.Folders)
            {
                if (folderResult == null && folderItems.Count >= 2 && !folderComputing && !folderFailed) Recompute();
                else
                {
                    Refresh();
                    if (folderStatus != null && !folderComputing) SetStatus(folderStatus);
                }
            }
            else
            {
                if (current == null) Recompute();
                else Refresh();
            }
        }

        void MarkDirty(CompareState which, int level)
        {
            if (which == CompareState.Folders) foldersDirty = Math.Max(foldersDirty, level);
            else datafilesDirty = Math.Max(datafilesDirty, level);
        }

        void SaveView()
        {
            ViewState v = View;
            v.SelectedMask = selectedMask;
            v.SelectedBank = selectedBank;
            v.Found = foundFilter;
            v.FocusSlot = focusSlot;
            v.ActiveBeforeFocus = activeBeforeFocus;
            v.Active = active;
        }

        void LoadView()
        {
            ViewState v = View;
            selectedMask = v.SelectedMask;
            selectedBank = v.SelectedBank;
            foundFilter = v.Found;
            focusSlot = v.FocusSlot;
            activeBeforeFocus = v.ActiveBeforeFocus;
            active = v.Active;
        }

        /// <summary>
        /// Names tabs and columns for the current state. The settings panel is the same in both states; only frequency
        /// and period are greyed out for folders, since a folder comparison checks all frequencies and periods.
        /// </summary>
        void UpdateStateControls()
        {
            bool folders = state == CompareState.Folders;
            Title = folders ? "Compare folders" : "Compare databanks";
            DatafileControls.IsEnabled = !folders;
            DatafileControls.ToolTip = folders ? "Folders: all frequencies and all periods of each data file are checked." : null;
            if (folders) SpanText.Text = "Folders: all frequencies and periods are checked.";
            NamesText.Visibility = folders ? Visibility.Collapsed : Visibility.Visible;
            FilesList.Visibility = folders ? Visibility.Visible : Visibility.Collapsed;
            IgnoreDimsCheck.Visibility = folders ? Visibility.Collapsed : Visibility.Visible;
            ValuesTab.Header = folders ? "Details" : "Values";
            OnlyFoundCheck.Visibility = folders ? Visibility.Collapsed : Visibility.Visible;
            ContentsColumn.Header = folders ? "Data files" : "Series";
            UpdateOpenedFromText();
        }

        void UpdateOpenedFromText()
        {
            if (state == CompareState.Datafiles && openedFrom != null)
            {
                StateInfoText.Text = "Opened from the folders: " + openedFrom.RelativePath + ". Click Folders to go back.";
                StateInfoText.Visibility = Visibility.Visible;
            }
            else
            {
                StateInfoText.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// A file in the Files tab (Folders state) was clicked: compare its 2 or 3 copies as databanks.
        /// They replace the list of the Datafiles state, which starts with a fresh selection.
        /// </summary>
        void OpenFileInDatafiles(FileComparison fc)
        {
            if (!fc.IsComparison)
            {
                SetStatus(fc.RelativePath + " is only in folder " + (fc.Slots[0] + 1) + ", so there is nothing to compare it with.");
                return;
            }
            var loaded = new List<BankItem>();
            for (int i = 0; i < fc.FullPaths.Length; i++)
            {
                try
                {
                    loaded.Add(new BankItem(LoadBank(fc.FullPaths[i])));
                }
                catch (Exception ex)
                {
                    SetStatus("Could not read " + fc.FullPaths[i] + ": " + ex.Message);
                    return;
                }
            }

            // Prepare the Datafiles state before showing it, so the old list is not compared first.
            bankItems.Clear();
            foreach (BankItem item in loaded) bankItems.Add(item);
            datafilesView.SelectedMask = 0;
            datafilesView.SelectedBank = -1;
            datafilesView.Found = FlagFilter.Any;
            datafilesView.FocusSlot = -1;
            datafilesView.ActiveBeforeFocus = null;
            datafilesView.Active = new[] { true, true, true };
            userFrom = null;
            userTo = null;
            periodAnchor = null;
            openedFrom = fc;
            datafilesDirty = DirtyList;

            if (state == CompareState.Datafiles)
            {
                LoadView();
                BanksChanged();
            }
            else
            {
                SwitchState(CompareState.Datafiles);  // dirty: recomputes the new list
            }
            UpdateOpenedFromText();
            SetStatus("Comparing the series of " + fc.RelativePath + " in folders "
                      + string.Join(" and ", fc.Slots.Select(s => (s + 1).ToString(Inv))) + ". Click Folders (top left) to go back.");
        }

        // ---------------------------------------------------------------- recent databanks and folders

        static List<string> LoadRecent(bool folders)
        {
            string text = (folders ? Settings.CompareRecentFolders : Settings.CompareRecentBanks) ?? "";
            return text.Split(new[] { RecentSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        static void SaveRecent(bool folders, List<string> paths)
        {
            try
            {
                string text = string.Join(RecentSeparator.ToString(), paths.Take(MaxRecentBanks));
                if (folders) Settings.CompareRecentFolders = text;
                else Settings.CompareRecentBanks = text;
                Settings.Save();
            }
            catch (Exception)
            {
                // The list is a convenience; never fail loading a bank or folder because of it.
            }
        }

        /// <summary>Moves (or adds) the path to the top of the LRU list.</summary>
        static void TouchRecent(bool folders, string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            List<string> paths = LoadRecent(folders);
            paths.RemoveAll(p => string.Equals(p, path, StringComparison.OrdinalIgnoreCase));
            paths.Insert(0, path);
            SaveRecent(folders, paths);
        }

        static void ForgetRecent(bool folders, string path)
        {
            List<string> paths = LoadRecent(folders);
            if (paths.RemoveAll(p => string.Equals(p, path, StringComparison.OrdinalIgnoreCase)) > 0) SaveRecent(folders, paths);
        }

        static bool IsIn(ObservableCollection<BankItem> list, string path)
        {
            return list.Any(b => string.Equals(b.SourcePath, path, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// The [Recent] button on a row: a numbered list of the most recently compared databanks (or folders, in
        /// the Folders state); those already in the list are greyed out. On a filled row the chosen one replaces
        /// it; on the empty row it is added.
        /// </summary>
        void RowRecent_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var target = button.Tag as BankItem;
            if (target == null) return;
            bool folders = target.IsFolderRow;
            var menu = new ContextMenu { PlacementTarget = button, Placement = PlacementMode.Bottom };
            List<string> paths = LoadRecent(folders);
            if (paths.Count == 0)
            {
                menu.Items.Add(new MenuItem { Header = new TextBlock { Text = folders ? "No recent folders" : "No recent databanks" }, IsEnabled = false });
            }
            for (int i = 0; i < paths.Count; i++)
            {
                string path = paths[i];
                bool open = IsIn(banks, path);
                // TextBlocks rather than a string header: '_' in file names would otherwise become access keys.
                var header = new StackPanel { Orientation = Orientation.Horizontal, Opacity = open ? 0.45 : 1.0 };
                header.Children.Add(new TextBlock { Text = (i + 1) + ".", Width = 26 });
                header.Children.Add(new TextBlock
                {
                    Text = folders ? FolderResult.FolderName(path) : Path.GetFileName(path),
                    MinWidth = 160,
                    Margin = new Thickness(0, 0, 16, 0)
                });
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
            bool folders = target.IsFolderRow;
            if (IsIn(banks, path)) return;
            if (folders ? !Directory.Exists(path) : !File.Exists(path))
            {
                ForgetRecent(folders, path);
                SetStatus(path + " no longer exists and was removed from the recent list.");
                return;
            }
            int index = banks.IndexOf(target);
            if (target.IsPlaceholder || index < 0)
            {
                if (folders) AddFolders(new[] { path }, folderItems.Count);
                else AddFiles(new[] { path }, bankItems.Count);
                return;
            }
            try
            {
                BankItem item = folders ? BankItem.ForFolder(path) : new BankItem(LoadBank(path));
                banks[index] = item;
                TouchRecent(folders, path);
                if (!folders) openedFrom = null;
                BanksChanged();
                UpdateOpenedFromText();
                SetStatus(target.Name + " replaced by " + item.Name + ".");
            }
            catch (Exception ex)
            {
                SetStatus("Could not read " + Path.GetFileName(path) + ": " + ex.Message);
            }
        }

        // ---------------------------------------------------------------- recomputation

        /// <summary>Call when the list of the current state changes (added, removed, reordered) or the compare mode changes.</summary>
        void BanksChanged()
        {
            if (!ready) return;
            busy = true;
            try
            {
                // "first 3" whenever the user chose it and there are 3 or more (also after the list was shorter).
                bool three = compare3Choice && banks.Count >= 3;
                Compare3Radio.IsEnabled = banks.Count >= 3;
                Compare3Radio.IsChecked = three;
                Compare2Radio.IsChecked = !three;
                int n = SlotCount;
                for (int i = 0; i < banks.Count; i++)
                {
                    banks[i].Position = i + 1;
                    banks[i].InComparison = i < n;
                }
                for (int s = 0; s < active.Length; s++) active[s] = true;
                focusSlot = -1;
                activeBeforeFocus = null;
                if (state == CompareState.Datafiles)
                {
                    RefreshFrequencies();
                    datafilesDirty = DirtyNone;
                }
                else
                {
                    foldersDirty = DirtyNone;
                    for (int i = n; i < folderItems.Count; i++) folderItems[i].SetContents("");
                }
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
            List<CompareFreq> freqs = bankItems.Take(SlotCount).SelectMany(b => b.Bank.Frequencies).Distinct().OrderBy(f => f).ToList();
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

        /// <summary>The name filter (matched against series names, or against file names and relative paths).</summary>
        Func<string, bool> ReadFilter()
        {
            string error;
            Func<string, bool> filter = CompareEngine.BuildFilter(FilterBox.Text, RegexCheck.IsChecked == true, out error);
            if (error == null) FilterBox.ClearValue(Control.BackgroundProperty);
            else
            {
                FilterBox.Background = InvalidBack;
                SetStatus(error);
            }
            FilterBox.ToolTip = error;
            return filter;
        }

        /// <summary>
        /// Recomputes the comparison of the current state. Needed when the scope changes: the list, compare mode,
        /// hide/focus, frequency, name filter or a threshold. The check and the drill-down filters only need Refresh().
        /// </summary>
        void Recompute()
        {
            if (!ready) return;
            recomputeTimer.Stop();
            if (state == CompareState.Folders)
            {
                if (foldersDirty == DirtyRecompute) foldersDirty = DirtyNone;
                RecomputeFolders();
                return;
            }
            if (datafilesDirty == DirtyRecompute) datafilesDirty = DirtyNone;

            CompareFreq? freq = SelectedFreq;
            if (bankItems.Count < 2 || !freq.HasValue)
            {
                current = null;
                selectedMask = 0;
                selectedBank = -1;
                Venn.SetState(null, bankItems.Count == 0 ? "Drag databank files (or folders) into the drop field to begin."
                                    : bankItems.Count == 1 ? "Add one more databank to compare."
                                    : "The compared databanks contain no series.",
                              0, -1, FlagFilter.Any, -1);
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

            Func<string, bool> filter = ReadFilter();
            List<CompareBank> compared = bankItems.Take(SlotCount).Select(b => b.Bank).ToList();
            current = CompareEngine.Compute(compared, active, freq.Value, filter, settings);
            Refresh();
        }

        /// <summary>
        /// Compares the folders in the background (listing files and comparing contents can take a while).
        /// A newer comparison cancels an older one.
        /// </summary>
        async void RecomputeFolders()
        {
            folderCts?.Cancel();
            int generation = ++folderGeneration;
            folderResult = null;
            folderFailed = false;
            folderStatus = null;
            if (folderItems.Count < 2)
            {
                folderComputing = false;
                folderCts = null;
                Refresh();
                return;
            }

            Func<string, bool> filter = ReadFilter();
            string filterKey = (RegexCheck.IsChecked == true ? "regex:" : "wild:") + FilterBox.Text.Trim();
            List<string> paths = folderItems.Take(SlotCount).Select(f => f.FolderPath).ToList();
            bool[] shownFolders = (bool[])active.Clone();
            var cts = new CancellationTokenSource();
            folderCts = cts;
            folderComputing = true;
            Refresh();  // shows "Comparing the folders ..."
            Action<string> progress = text => Dispatcher.BeginInvoke(new Action(() =>
            {
                if (generation == folderGeneration && state == CompareState.Folders) SetStatus(text);
            }));

            FolderResult result = null;
            string error = null;
            try
            {
                // The files are read on a worker thread, one at a time (see LoadBank).
                result = await Task.Run(() => FolderCompareEngine.Compute(paths, shownFolders, filter, filterKey,
                                                                          LoadBank, progress, cts.Token, folderCache));
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            if (generation != folderGeneration) return;  // a newer comparison has started

            folderComputing = false;
            folderCts = null;
            folderResult = result;
            if (result != null)
            {
                for (int i = 0; i < folderItems.Count; i++)
                    folderItems[i].SetContents(i < result.SlotCount ? result.FileCount[i] + " data files" : "");
            }
            if (error != null)
            {
                folderFailed = true;
                folderStatus = "Could not compare the folders: " + error;
            }
            else
            {
                int files = result.Areas.Values.Sum(a => a.Files.Count);
                int errors = result.Areas.Values.Sum(a => a.Files.Count(f => f.Error != null));
                folderStatus = "Folders compared: " + files + " data files read"
                               + (errors > 0 ? ", " + errors + " could not be read" : "") + "."
                               + (result.Cancelled ? " (Stopped before all files were read.)" : "");
            }
            if (state != CompareState.Folders) return;  // the result and its status are shown when the Folders state is shown again
            Refresh();
            SetStatus(folderStatus);
        }

        int PeriodFrom => current != null && current.HasSpan ? (userFrom ?? current.SpanStart) : 0;
        int PeriodTo => current != null && current.HasSpan ? (userTo ?? current.SpanEnd) : -1;

        /// <summary>Applies the check and the drill-down filters to the current comparison and updates every view.</summary>
        void Refresh()
        {
            if (state == CompareState.Folders)
            {
                RefreshFolders();
                return;
            }
            if (current == null)
            {
                Recompute();
                return;
            }
            ClampPeriod();
            CompareEngine.UpdateCounts(current, check, PeriodFrom, PeriodTo);
            ValidateSelection();

            Venn.SetState(current, null, selectedMask, selectedBank, foundFilter, focusSlot);
            FitVennRow(current.SlotCount);

            selection = BuildSelection();
            ShowDetails();
            UpdatePeriodBoxes();
            UpdateListBadges();
        }

        void RefreshFolders()
        {
            if (folderResult == null)
            {
                // Keep the selection: while the folders are compared again (e.g. after hiding one) it is restored.
                string message = folderComputing ? "Comparing the folders ..."
                               : folderItems.Count == 0 ? "Drag 2 or 3 folders into the drop field to compare their data files."
                               : folderItems.Count == 1 ? "Add one more folder to compare."
                               : "The folders could not be compared. " + (folderStatus ?? "");
                Venn.SetState(null, message, 0, -1, FlagFilter.Any, -1);
            }
            else
            {
                folderResult.UpdateCounts(check, settings);
                ValidateSelection();
                Venn.SetState(folderResult, null, selectedMask, selectedBank, foundFilter, focusSlot);
                FitVennRow(folderResult.SlotCount);
            }
            fileSelection = BuildFileSelection();
            ShowDetails();
            UpdateListBadges();
        }

        /// <summary>A selected area or bank/folder that no longer exists (e.g. after hiding) is dropped.</summary>
        void ValidateSelection()
        {
            IVennModel m = Model;
            if (selectedBank >= 0 && (selectedBank >= m.SlotCount || !m.Active[selectedBank])) selectedBank = -1;
            if (selectedMask != 0 && !m.AreaMasks.Contains(selectedMask)) selectedMask = 0;
            if (selectedBank >= 0) selectedMask = 0;
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
            IVennModel m = Model;
            for (int i = 0; i < banks.Count; i++)
                banks[i].HiddenInDiagram = m != null && i < m.SlotCount && !m.Active[i];
        }

        // ---------------------------------------------------------------- the check and its thresholds

        /// <summary>The tabs at the bottom of the controls panel: Differences, Missings, Zeroes.</summary>
        void CheckTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ReferenceEquals(e.OriginalSource, CheckTabs)) return;  // ignore SelectionChanged bubbling from inside the tabs
            if (!ready || busy) return;
            var newCheck = (CheckKind)Math.Max(0, CheckTabs.SelectedIndex);
            if (newCheck == check) return;
            check = newCheck;
            Refresh();  // all checks are computed already (the other state applies it when shown)
        }

        /// <summary>Reads the thresholds into the settings. Returns false (and marks the box) if one is not valid.</summary>
        bool ReadThresholds()
        {
            bool ok = true;
            double diff, zero;
            if (CompareEngine.TryParseThreshold(DiffThresholdBox.Text, out diff))
            {
                settings.DiffThreshold = diff;
                DiffThresholdBox.ClearValue(Control.BackgroundProperty);
            }
            else
            {
                DiffThresholdBox.Background = InvalidBack;
                ok = false;
            }
            if (CompareEngine.TryParseThreshold(ZeroThresholdBox.Text, out zero))
            {
                settings.ZeroThreshold = zero;
                ZeroThresholdBox.ClearValue(Control.BackgroundProperty);
            }
            else
            {
                ZeroThresholdBox.Background = InvalidBack;
                ok = false;
            }
            settings.DiffRelative = DiffRelativeCheck.IsChecked == true;
            if (!ok) SetStatus("A threshold must be a number of 0 or more, e.g. 0.001.");
            return ok;
        }

        /// <summary>
        /// A threshold box or the "relative" box changed. Datafiles: recomputes (after a short pause while typing).
        /// Folders: re-evaluates at once from the stored summaries (no reading); the Datafiles state recomputes when shown.
        /// </summary>
        void Threshold_Changed(object sender, RoutedEventArgs e)
        {
            if (!ready) return;
            if (!ReadThresholds()) return;
            if (state == CompareState.Datafiles) ScheduleRecompute();
            else
            {
                MarkDirty(CompareState.Datafiles, DirtyRecompute);
                Refresh();
            }
        }

        // ---------------------------------------------------------------- the area filter

        /// <summary>Datafiles: the area filter, resolved: one area of the diagram, or a whole bank (its full circle).</summary>
        sealed class Selection
        {
            public string Description;              // e.g. "In banks 1 and 2, not in bank 3"
            public List<SeriesComparison> Series;   // all series in it (the black number)
            public int SlotMask;                    // banks in which Missings/Zeroes look (bit s = slot s)
            public bool IsComparison;               // false: series in one bank only, no differences to look for
        }

        /// <summary>Folders: the area filter, resolved.</summary>
        sealed class FileSelection
        {
            public string Description;
            public List<FileComparison> Files;
            public Func<FileComparison, bool> Found;   // area: the file's copies together; whole folder: that folder's copy
        }

        Selection BuildSelection()
        {
            if (current == null) return null;
            if (selectedBank >= 0)
            {
                // A whole bank: the areas inside its circle. Differences are those of its overlaps with other banks;
                // missing values and zeroes count in this bank only.
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
                    SlotMask = 1 << selectedBank,
                    IsComparison = series.Any(s => s.Slots.Length >= 2)
                };
            }
            VennArea area;
            if (selectedMask != 0 && current.Areas.TryGetValue(selectedMask, out area))
            {
                return new Selection
                {
                    Description = CompareEngine.DescribeArea(current, area.Mask),
                    Series = area.Series,
                    SlotMask = area.Mask,
                    IsComparison = area.IsComparison
                };
            }
            return null;
        }

        FileSelection BuildFileSelection()
        {
            if (folderResult == null) return null;
            if (selectedBank >= 0)
            {
                List<FileComparison> files = folderResult.FilesOfFolder(selectedBank);
                int slot = selectedBank;
                return new FileSelection
                {
                    Description = "All of folder " + (selectedBank + 1) + " (" + folderResult.SlotName(selectedBank) + ", "
                                  + files.Count + " files)",
                    Files = files,
                    Found = f => FolderResult.FoundInFolder(f, slot)
                };
            }
            FolderArea area;
            if (selectedMask != 0 && folderResult.Areas.TryGetValue(selectedMask, out area))
            {
                return new FileSelection
                {
                    Description = CompareEngine.DescribeArea(folderResult.SlotCount, folderResult.Active, area.Mask, "folder"),
                    Files = area.Files,
                    Found = f => f.Found
                };
            }
            return null;
        }

        // ---------------------------------------------------------------- clicks that set filters

        /// <summary>
        /// Inside an area: sets the area, keeps the found filter.
        /// Black number: the area, all of it. Red number: the area, only what the check finds.
        /// </summary>
        void Venn_AreaClicked(int mask, AreaClickKind kind, bool add)
        {
            IVennModel m = Model;
            if (m == null || !m.AreaMasks.Contains(mask)) return;
            selectedMask = mask;
            selectedBank = -1;
            if (kind == AreaClickKind.Total) foundFilter = FlagFilter.Any;
            if (kind == AreaClickKind.Deviating) foundFilter = FlagFilter.Yes;
            Refresh();  // the selected tab stays open
        }

        /// <summary>A box: the whole bank/folder as the area filter. Keeps the found filter.</summary>
        void Venn_BankSelected(int slot)
        {
            IVennModel m = Model;
            if (m == null || slot >= m.SlotCount || !m.Active[slot]) return;
            selectedBank = slot;
            selectedMask = 0;
            Refresh();  // the selected tab stays open
        }

        /// <summary>
        /// A histogram bar: sets the period (Shift: from the first clicked bar to this one) and shows only what the
        /// check finds. Keeps the area.
        /// </summary>
        void Bar_Clicked(int period, ModifierKeys keys)
        {
            if (state != CompareState.Datafiles || current == null || !current.HasSpan) return;
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
            foundFilter = FlagFilter.Yes;
            Refresh();
            SetStatus(shown.Count + " series " + CheckText.Phrase(check) + current.CountPeriodText
                      + ". Double-click a bar (or see the Names tab) to list them.");
        }

        // ---------------------------------------------------------------- hide and focus (scope)

        string BankLabel(int slot)
        {
            IVennModel m = Model;
            string word = m.SlotWord;
            return char.ToUpperInvariant(word[0]) + word.Substring(1) + " " + (slot + 1) + " (" + m.SlotName(slot) + ")";
        }

        static int MaskOf(bool[] flags, int slotCount)
        {
            int mask = 0;
            for (int s = 0; s < slotCount; s++) if (flags[s]) mask |= 1 << s;
            return mask;
        }

        /// <summary>
        /// Sets which banks/folders are shown, keeping a sensible area filter: "in all shown" stays "in all shown",
        /// an area drops the ones that disappear, a whole bank/folder stays selected unless it is hidden.
        /// Does not recompute.
        /// </summary>
        void ApplyActive(bool[] newActive)
        {
            int n = Model.SlotCount;
            int oldMask = MaskOf(active, n);
            for (int s = 0; s < active.Length; s++) active[s] = s >= n || newActive[s];
            int newMask = MaskOf(active, n);
            if (selectedMask != 0) selectedMask = selectedMask == oldMask ? newMask : selectedMask & newMask;
            if (selectedBank >= 0 && !active[selectedBank]) selectedBank = -1;
        }

        /// <summary>
        /// The eye icon. Without a focus: hide a shown bank/folder (as long as another one stays shown) or show a
        /// hidden one. With a focus: showing another one ends the focus; the focused one itself cannot be hidden.
        /// </summary>
        void ToggleHidden(int slot)
        {
            IVennModel m = Model;
            if (m == null || slot >= m.SlotCount) return;
            string label = BankLabel(slot);
            var next = (bool[])active.Clone();

            if (focusSlot >= 0)
            {
                if (slot == focusSlot)
                {
                    SetStatus(label + " is in focus and cannot be hidden. Click its target to show the others again.");
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
            else if (m.ActiveCount > 1)
            {
                next[slot] = false;
                message = label + " is hidden. Click its eye to show it again.";
            }
            else
            {
                SetStatus(label + " is the only one shown, so it cannot be hidden.");
                return;
            }
            ApplyActive(next);
            Recompute();
            SetStatus(message);
        }

        /// <summary>
        /// The target icon or Alt+1/2/3. Focus: only this bank/folder is shown, and all of it becomes the area filter.
        /// Again on the focused one: the ones shown before the focus come back, it stays selected.
        /// The check and the found and period filters are kept.
        /// </summary>
        void ToggleFocus(int slot)
        {
            IVennModel m = Model;
            if (m == null || slot >= m.SlotCount) return;
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
                SetStatus("Focus on " + label + " ended. It stays selected.");
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
            SetStatus("Focus on " + label + ": the others are greyed out. Click the target again (or Alt+"
                      + (slot + 1) + ") to show them.");
        }

        // ---------------------------------------------------------------- details: names/files, histogram, values/details

        /// <summary>Applies the filters to the selection and fills the tabs.</summary>
        void ShowDetails()
        {
            if (state == CompareState.Folders)
            {
                ShowFileDetails();
                return;
            }

            string keepName = navIndex >= 0 && navIndex < navList.Count ? navList[navIndex].Name : null;

            if (selection == null || current == null)
            {
                shown = new List<SeriesComparison>();
                ShowNames();
                ShowHistogram();
                navList = shown;
                navIndex = -1;
                devMessage = NoSelectionMessage;
                ShowCurrentItem();
                return;
            }

            int from = PeriodFrom, to = PeriodTo;
            int mask = selection.SlotMask;
            shown = selection.Series
                .Where(s => SeriesComparison.Passes(foundFilter, s.FoundIn(check, from, to, mask)))
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
            ShowCurrentItem();
        }

        /// <summary>
        /// The histogram: per period, the number of series in the area (or whole bank) found by the current check.
        /// It shows the whole universal period for the area only (not the found or period filters), so the time
        /// profile stays visible while drilling down. The selected periods are highlighted.
        /// </summary>
        void ShowHistogram()
        {
            if (state == CompareState.Folders)
            {
                Histogram.ShowMessage("There is no histogram for folders. Click a file in the Files tab to compare its series per period.");
                return;
            }
            if (selection == null || current == null)
            {
                Histogram.ShowMessage(NoSelectionMessage);
                return;
            }
            if (check == CheckKind.Differences && !selection.IsComparison)
            {
                Histogram.ShowMessage("Series that exist in only one bank have nothing to be compared with. "
                                      + "Choose an overlap, or the Missings or Zeroes check.");
                return;
            }
            if (!current.HasSpan)
            {
                Histogram.ShowMessage("No periods to show.");
                return;
            }
            int start = current.SpanStart, end = current.SpanEnd;
            int? hiFrom = current.IsPeriodRestricted ? PeriodFrom : (int?)null;
            int? hiTo = current.IsPeriodRestricted ? PeriodTo : (int?)null;
            string phrase = "series " + CheckText.Phrase(check);
            Histogram.SetData(selection.Description + ": number of " + phrase + ", per period",
                              current.Freq, start, CompareEngine.FoundCounts(selection.Series, check, selection.SlotMask, start, end),
                              phrase, null, hiFrom, hiTo);
        }

        /// <summary>Folders: the Files tab, the (absent) histogram and the Details tab.</summary>
        void ShowFileDetails()
        {
            string keepPath = fileNavIndex >= 0 && fileNavIndex < fileNav.Count ? fileNav[fileNavIndex].RelativePath : null;
            ShowHistogram();

            if (fileSelection == null)
            {
                shownFiles = new List<FileComparison>();
                NamesTab.Header = "Files";
                NamesHeader.Text = folderResult == null ? "" : "Click a number, an area or a folder's box in the diagram to list its files.";
                NamesHeader.ToolTip = null;
                FilesList.ItemsSource = null;
                namesPlainText = "";
                fileNav = shownFiles;
                fileNavIndex = -1;
                devMessage = NoSelectionMessage;
                ShowCurrentItem();
                return;
            }

            Func<FileComparison, bool> isFound = fileSelection.Found;
            shownFiles = fileSelection.Files.Where(f => SeriesComparison.Passes(foundFilter, isFound(f))).ToList();
            int found = shownFiles.Count(isFound);
            string phrase = CheckText.Phrase(check);
            string header = fileSelection.Description + ": " + shownFiles.Count
                            + (shownFiles.Count == fileSelection.Files.Count ? "" : " of " + fileSelection.Files.Count) + " files"
                            + (foundFilter == FlagFilter.Yes ? " " + phrase : "");
            if (found > 0 && foundFilter != FlagFilter.Yes) header += " (" + found + " " + phrase + ", in dark red)";
            header += ". Click a file to compare its series.";
            NamesHeader.Text = header;
            NamesHeader.ToolTip = header;
            NamesTab.Header = "Files (" + shownFiles.Count + ")";
            FilesList.ItemsSource = shownFiles.Select(f => MakeFileRow(f, isFound(f))).ToList();
            namesPlainText = string.Join(Environment.NewLine, shownFiles.Select(f => f.RelativePath));

            fileNav = shownFiles;
            fileNavIndex = shownFiles.Count == 0 ? -1 : 0;
            if (keepPath != null)
            {
                int i = fileNav.FindIndex(f => string.Equals(f.RelativePath, keepPath, StringComparison.OrdinalIgnoreCase));
                if (i >= 0) fileNavIndex = i;
            }
            devMessage = "No files match the current filters.";
            ShowCurrentItem();
        }

        FileRow MakeFileRow(FileComparison f, bool found)
        {
            string where = (f.Slots.Length == 1 ? "Only in folder " : "In folders ")
                           + string.Join(" and ", f.Slots.Select(s => (s + 1).ToString(Inv)));
            string tip = where + (found && f.Why.Length > 0 ? ": " + f.Why : "") + ".";
            tip += f.IsComparison ? "\nClick to compare its series." : "\nIn one folder only: nothing to compare it with.";
            return new FileRow
            {
                File = f,
                Text = f.RelativePath,
                Foreground = found ? DeviatingNameBrush : Brushes.Black,
                Tip = tip,
                Cursor = f.IsComparison ? Cursors.Hand : Cursors.Arrow
            };
        }

        /// <summary>A click on a file in the Files tab opens it in the Datafiles state.</summary>
        void FilesList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = FindAncestor<ListBoxItem>(e.OriginalSource as DependencyObject);
            var row = item?.DataContext as FileRow;
            if (row == null) return;
            fileNavIndex = fileNav.IndexOf(row.File);
            ShowCurrentItem();
            OpenFileInDatafiles(row.File);
        }

        void FilesList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            var row = FilesList.SelectedItem as FileRow;
            if (row == null) return;
            e.Handled = true;
            OpenFileInDatafiles(row.File);
        }

        // ---------------------------------------------------------------- period from/to (the period filter, Datafiles)

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
            if (ready && state == CompareState.Datafiles) CommitPeriod(ReferenceEquals(sender, FromBox));
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

        // ---------------------------------------------------------------- names tab (Datafiles)

        /// <summary>The series passing all filters. Those found by the check in the selected periods are dark red.</summary>
        void ShowNames()
        {
            if (selection == null || current == null)
            {
                NamesTab.Header = "Names";
                NamesHeader.Text = "Click a number, an area or a bank's box in the diagram to list its series.";
                NamesHeader.ToolTip = null;
                SetNamesText(new List<string>(), new HashSet<string>());
                return;
            }
            int from = PeriodFrom, to = PeriodTo;
            bool ignore = IgnoreDimsCheck.IsChecked == true;
            List<string> names = CompareEngine.NamesForDisplay(shown.Select(s => s.Name), ignore);
            List<string> found = shown.Where(s => s.FoundIn(check, from, to, selection.SlotMask)).Select(s => s.Name).ToList();
            var red = new HashSet<string>(CompareEngine.NamesForDisplay(found, ignore), StringComparer.OrdinalIgnoreCase);
            string phrase = CheckText.Phrase(check);

            string header = selection.Description + current.CountPeriodText + ": " + shown.Count
                            + (shown.Count == selection.Series.Count ? "" : " of " + selection.Series.Count) + " series"
                            + (foundFilter == FlagFilter.Yes ? " " + phrase : "");
            if (ignore) header += ", " + names.Count + " names without dimensions";
            if (found.Count > 0 && foundFilter != FlagFilter.Yes) header += " (" + found.Count + " " + phrase + ", in dark red)";
            NamesHeader.Text = header;
            NamesHeader.ToolTip = header;
            NamesTab.Header = "Names (" + shown.Count + ")";
            SetNamesText(names, red);
        }

        /// <summary>Comma-separated names; runs of consecutive found names become one dark red Run.</summary>
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
            if (ready && state == CompareState.Datafiles) ShowNames();
        }

        void CopyNames_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(namesPlainText)) return;
            try
            {
                Clipboard.SetText(namesPlainText);
                SetStatus(state == CompareState.Folders ? "File paths copied to the clipboard." : "Names copied to the clipboard.");
            }
            catch (Exception ex)
            {
                SetStatus("Could not copy to the clipboard: " + ex.Message);
            }
        }

        // ---------------------------------------------------------------- values tab (Datafiles) / details tab (Folders)

        void ShowCurrentItem()
        {
            if (state == CompareState.Folders) ShowCurrentFile();
            else ShowCurrentSeries();
        }

        /// <summary>One series of the Names list, for the selected periods. M = missing, * = found by the check.</summary>
        void ShowCurrentSeries()
        {
            bool has = navIndex >= 0 && navIndex < navList.Count && selection != null;
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
            int found = c.FoundPeriods(check, from, to, selection.SlotMask);
            string what = check == CheckKind.Missings ? " with M" : check == CheckKind.Zeroes ? " with zero" : " that differ";
            NavText.Text = c.Name + "   (" + (navIndex + 1) + " of " + navList.Count + "; "
                           + found + (found == 1 ? " period" : " periods") + what + current.CountPeriodText + ")";
            string star = check == CheckKind.Missings ? "M in a bank" : check == CheckKind.Zeroes ? "zero in a bank" : "differs";
            LegendText.Text = string.Join("   ", c.Slots.Select(s => "(" + (s + 1) + ") " + current.Banks[s].Name))
                              + "   M = missing   * = " + star;
            listing = CompareEngine.FormatComparison(c, current.Freq, from, to, check, selection.SlotMask, OnlyFoundCheck.IsChecked == true);
            DevText.Text = string.Join("\r\n", listing.Lines);
            DevText.Select(0, 0);
            DevText.ScrollToHome();
        }

        /// <summary>
        /// One file of the Files list: per folder its size, date, series and whether the check finds something; per
        /// pair of folders how they differ. Useful to see what a threshold does.
        /// </summary>
        void ShowCurrentFile()
        {
            bool has = fileNavIndex >= 0 && fileNavIndex < fileNav.Count && folderResult != null;
            PrevButton.IsEnabled = has && fileNavIndex > 0;
            NextButton.IsEnabled = has && fileNavIndex < fileNav.Count - 1;
            if (!has)
            {
                listing = null;
                NavText.Text = "";
                LegendText.Text = "";
                DevText.Text = devMessage;
                return;
            }

            FileComparison f = fileNav[fileNavIndex];
            NavText.Text = f.RelativePath + "   (" + (fileNavIndex + 1) + " of " + fileNav.Count + ")";
            LegendText.Text = f.IsComparison ? "Click the file in the Files tab to compare its series" : "In one folder only";
            string foundWord = check == CheckKind.Missings ? "has M" : check == CheckKind.Zeroes ? "has zero" : "";
            var lines = new List<string>
            {
                "folder".PadRight(8) + "size (bytes)".PadLeft(14) + "   " + "modified".PadRight(21) + "series".PadLeft(8)
                + (foundWord.Length > 0 ? "   " + foundWord.PadRight(9) : "   ") + "path"
            };
            for (int s = 0; s < folderResult.SlotCount; s++)
            {
                int i = Array.IndexOf(f.Slots, s);
                string label = ("(" + (s + 1) + ")").PadRight(8);
                if (i < 0)
                {
                    lines.Add(label + "not in this folder".PadLeft(14));
                    continue;
                }
                CopyStats c = f.Copies[i];
                string series = c == null || c.Error != null ? "?" : c.SeriesCount.ToString(Inv);
                string flag = foundWord.Length == 0 ? "" : ((f.FoundInCopy != null && f.FoundInCopy[i]) ? "yes" : "no").PadRight(9) + "   ";
                lines.Add(label + f.Sizes[i].ToString("N0", Inv).PadLeft(14) + "   "
                          + f.Modified[i].ToString("yyyy-MM-dd HH:mm:ss", Inv).PadRight(21) + series.PadLeft(8) + "   "
                          + flag + f.FullPaths[i]
                          + (f.SameAs[i] != i ? "   (identical to folder " + (f.Slots[f.SameAs[i]] + 1) + ")" : ""));
            }
            if (f.IsComparison)
            {
                lines.Add("");
                lines.Add("folders".PadRight(10) + "same series".PadRight(13) + "largest abs. diff.".PadLeft(20) + "largest rel. diff.".PadLeft(20) + "   differ");
                for (int i = 0; i < f.Slots.Length; i++)
                    for (int j = i + 1; j < f.Slots.Length; j++)
                    {
                        PairStats p = f.Pairs[i, j];
                        string pair = ((f.Slots[i] + 1) + " and " + (f.Slots[j] + 1)).PadRight(10);
                        if (p.Error != null)
                        {
                            lines.Add(pair + "could not be read: " + p.Error);
                            continue;
                        }
                        lines.Add(pair + (p.StructureDiffers ? "no" : "yes").PadRight(13)
                                  + FileComparison.FormatDiff(p, false).PadLeft(20) + FileComparison.FormatDiff(p, true).PadLeft(20)
                                  + "   " + (p.Differs(settings) ? "yes" : "no"));
                    }
            }
            lines.Add("");
            string error = f.Error;
            if (error != null) lines.Add("Could not be read: " + error);
            else if (f.Found) lines.Add("Found by the " + check.ToString().ToLowerInvariant() + " check: " + f.Why + ".");
            else lines.Add("Not found by the " + check.ToString().ToLowerInvariant() + " check.");
            lines.Add("All frequencies and periods of the (filtered) series are checked.");
            DevText.Text = string.Join("\r\n", lines);
            DevText.Select(0, 0);
            DevText.ScrollToHome();
        }

        void Navigate(int delta)
        {
            if (state == CompareState.Folders)
            {
                int k = fileNavIndex + delta;
                if (k < 0 || k >= fileNav.Count) return;
                fileNavIndex = k;
            }
            else
            {
                int i = navIndex + delta;
                if (i < 0 || i >= navList.Count) return;
                navIndex = i;
            }
            ShowCurrentItem();
        }

        int NavCount => state == CompareState.Folders ? fileNav.Count : navList.Count;

        void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            Navigate(-1);
        }

        void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Navigate(1);
        }

        void OnlyFound_Changed(object sender, RoutedEventArgs e)
        {
            if (ready) ShowCurrentItem();
        }

        /// <summary>
        /// Alt+1/2/3 focus on a bank/folder (again: show all). Ctrl+Up / Ctrl+Down step through the Values/Details
        /// tab; Ctrl+D hides or shows diagram and controls.
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
            if (NavCount == 0) return;
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
                if (Model != null) FitVennRow(Model.SlotCount);
            }
        }

        // ---------------------------------------------------------------- controls

        /// <summary>"first 2" / "first 3" is shared by both states.</summary>
        void CompareMode_Changed(object sender, RoutedEventArgs e)
        {
            if (!ready || busy) return;
            compare3Choice = ReferenceEquals(sender, Compare3Radio);
            // The other state only changes if its list is long enough for "first 3" to matter.
            ObservableCollection<BankItem> otherList = state == CompareState.Datafiles ? folderItems : bankItems;
            if (otherList.Count >= 3) MarkDirty(state == CompareState.Datafiles ? CompareState.Folders : CompareState.Datafiles, DirtyList);
            BanksChanged();
        }

        void FreqCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ready || busy) return;
            Recompute();
        }

        /// <summary>The name filter applies to both states; the other state is recomputed when it is shown again.</summary>
        void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!ready) return;
            MarkOtherStateDirty();
            ScheduleRecompute();
        }

        void RegexCheck_Changed(object sender, RoutedEventArgs e)
        {
            if (!ready) return;
            FilterHint.Text = RegexCheck.IsChecked == true
                ? "Regular expression, case-insensitive, e.g. ^x\\d+$"
                : "Wildcards * and ?, several patterns separated by spaces";
            MarkOtherStateDirty();
            Recompute();
        }

        /// <summary>The filter changed: the other state is recomputed (keeping its hidden banks/folders) when shown again.</summary>
        void MarkOtherStateDirty()
        {
            MarkDirty(state == CompareState.Datafiles ? CompareState.Folders : CompareState.Datafiles, DirtyRecompute);
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void SetStatus(string text)
        {
            StatusText.Text = text;
        }

        // ---------------------------------------------------------------- dropping files and folders

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
            HandleDrop(e.Data.GetData(DataFormats.FileDrop) as string[], banks.Count);
        }

        /// <summary>
        /// Dropped folders go to the folder list (and show the Folders state), dropped files to the databank list
        /// (and show the Datafiles state). insertAt is the position in the list shown when dropping.
        /// </summary>
        void HandleDrop(string[] paths, int insertAt)
        {
            if (paths == null || paths.Length == 0) return;
            string[] dirs = paths.Where(Directory.Exists).ToArray();
            string[] files = paths.Where(p => !Directory.Exists(p)).ToArray();
            if (dirs.Length > 0)
            {
                int at = state == CompareState.Folders ? insertAt : folderItems.Count;
                SwitchState(CompareState.Folders);
                AddFolders(dirs, at);
                if (files.Length > 0)
                {
                    AddFiles(files, bankItems.Count);
                    SetStatus(StatusText.Text + " The dropped files were added to the Datafiles list.");
                }
            }
            else
            {
                int at = state == CompareState.Datafiles ? insertAt : bankItems.Count;
                SwitchState(CompareState.Datafiles);
                AddFiles(files, at);
            }
        }

        /// <summary>Loads the databank files and inserts them in the Datafiles list at position insertAt (0-based).</summary>
        void AddFiles(string[] paths, int insertAt)
        {
            if (paths == null || paths.Length == 0) return;
            int at = Math.Max(0, Math.Min(insertAt, bankItems.Count));
            int added = 0;
            var skipped = new List<string>();
            string failure = null;
            foreach (string path in paths)
            {
                if (Directory.Exists(path) || IsIn(bankItems, path))
                {
                    skipped.Add(Path.GetFileName(path));
                    continue;
                }
                try
                {
                    bankItems.Insert(at++, new BankItem(LoadBank(path)));
                    TouchRecent(false, path);
                    added++;
                }
                catch (Exception ex)
                {
                    failure = "Could not read " + Path.GetFileName(path) + ": " + ex.Message;
                }
            }
            if (added > 0)
            {
                openedFrom = null;
                if (state == CompareState.Datafiles) BanksChanged();
                else MarkDirty(CompareState.Datafiles, DirtyList);
                UpdateOpenedFromText();
            }

            string status = added == 1 ? "1 databank added." : added + " databanks added.";
            if (skipped.Count > 0) status += " Skipped banks already in the list: " + string.Join(", ", skipped) + ".";
            if (failure != null) status += " " + failure;
            SetStatus(status);
        }

        /// <summary>Inserts folders in the Folders list at position insertAt (0-based).</summary>
        void AddFolders(string[] paths, int insertAt)
        {
            if (paths == null || paths.Length == 0) return;
            int at = Math.Max(0, Math.Min(insertAt, folderItems.Count));
            int added = 0;
            var skipped = new List<string>();
            foreach (string path in paths)
            {
                if (IsIn(folderItems, path))
                {
                    skipped.Add(FolderResult.FolderName(path));
                    continue;
                }
                folderItems.Insert(at++, BankItem.ForFolder(path));
                TouchRecent(true, path);
                added++;
            }
            if (added > 0)
            {
                if (state == CompareState.Folders) BanksChanged();
                else MarkDirty(CompareState.Folders, DirtyList);
            }
            string status = added == 1 ? "1 folder added." : added + " folders added.";
            if (skipped.Count > 0) status += " Skipped folders already in the list: " + string.Join(", ", skipped) + ".";
            if (!folderComputing) SetStatus(status);
        }

        void RemoveBank_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement)?.Tag as BankItem;
            if (item != null && !item.IsPlaceholder) RemoveBank(item);
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
            if (state == CompareState.Datafiles) openedFrom = null;
            UpdateOpenedFromText();
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
                HandleDrop(e.Data.GetData(DataFormats.FileDrop) as string[], gap);
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
