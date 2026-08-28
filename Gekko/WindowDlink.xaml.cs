using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Gekko
{
    /// <summary>
    /// One row in the grid: a dropped data file, its computed status, and whatever we worked out
    /// about it (target .dlink path, whether it's a detected move, etc). Path/Status/
    /// DlinkPathDisplay are shown to the user; the rest is working state used by the Dlink
    /// button click handlers.
    /// </summary>
    public class DlinkImportRow : INotifyPropertyChanged
    {
        public string Path { get; set; }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }
        
        private string _dlinkPathDisplay;
        public string DlinkPathDisplay
        {
            get { return _dlinkPathDisplay; }
            set { _dlinkPathDisplay = value; OnPropertyChanged("DlinkPathDisplay"); }
        }

        public DlinkImportRowKind Kind;
        public string TargetDlinkPath;      //where this file's .dlink should live
        public string MoveFromDlinkPath;    //if Kind == Move, the orphaned .dlink to relocate here
        public string ComputedHash;
        public long ComputedSize;
        public DateTime ComputedStampUtc;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    public enum DlinkImportRowKind
    {
        Unresolved,
        OutsideRecognizedFolder,
        AlreadyInSync,
        NewOrChanged,
        Move
    }

    /// <summary>
    /// Drag-and-drop tool for producing/updating .dlink files for data files that were added or
    /// changed outside Gekko (e.g. a plain file copy). For each dropped file it works out one of:
    /// already in sync (nothing to do), new/changed (needs Blob()), or a detected move/rename (an
    /// existing, now-orphaned .dlink elsewhere in the tree has the exact same hash, since .dlink
    /// content depends only on the data, never on its path, so that .dlink can just be relocated
    /// rather than regenerated). Orphaned .dlink files not claimed by whatever's been processed so
    /// far are reported, not silently left behind -- see ReportLeftoverOrphans. Each row can be
    /// processed individually (its own [Dlink] button) or all at once ([Dlink all]).
    ///
    /// Deliberately synchronous/single-threaded: hashing happens on the UI thread as files are
    /// dropped, which is fine for the batch sizes this is meant for (a handful to a few dozen
    /// files at a time), but would visibly block the UI for a very large drop. Not addressed here
    /// to keep this first version simple; worth revisiting with a background worker if that
    /// becomes a real usage pattern.
    /// </summary>
    public partial class WindowDlink : Window
    {
        private readonly ObservableCollection<DlinkImportRow> _items = new ObservableCollection<DlinkImportRow>();

        // Orphan index: hash -> every existing .dlink file (under the data root) whose own data
        // file is currently missing. Built lazily, once per window session, the first time a
        // dropped file has no .dlink already sitting at its target location (see
        // BuildOrphanIndexIfNeeded) -- walking every .dlink file on disk is a real cost, not
        // something to redo per dropped file.
        private Dictionary<string, List<string>> _orphanIndex;
        private bool _orphanIndexBuilt;

        // New: resting vs. drag-hover colors for the drop zone's fill (not the dashed border,
        // which stays as-is), plus a matching text color swap so the hint text stays readable
        // against the blue fill.
        // New: resting vs. drag-hover colors for the drop zone's fill (not the dashed border,
        // which stays as-is). Both colors are light enough that the hint text's normal gray reads
        // fine against either, so no text-color swap is needed on hover.
        private static readonly Brush DropZoneRestFill = CreateFrozenBrush(0xF4, 0xF4, 0xF4);
        private static readonly Brush DropZoneHoverFill = CreateFrozenBrush(0xCC, 0xD5, 0xF0); //dusted blue

        private static Brush CreateFrozenBrush(byte r, byte g, byte b)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(r, g, b));
            brush.Freeze();
            return brush;
        }

        public WindowDlink()
        {
            InitializeComponent();
            FilesGrid.ItemsSource = _items;
        }
                
        private void DropZone_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            bool ok = e.Data.GetDataPresent(DataFormats.FileDrop);
            e.Effects = ok ? DragDropEffects.Copy : DragDropEffects.None;
            DropZoneBorder.Fill = ok ? DropZoneHoverFill : DropZoneRestFill;
            e.Handled = true;
        }
                
        private void DropZone_DragLeave(object sender, System.Windows.DragEventArgs e)
        {
            DropZoneBorder.Fill = DropZoneRestFill;
            e.Handled = true;
        }

        private void DropZone_Drop(object sender, System.Windows.DragEventArgs e)
        {
            DropZoneBorder.Fill = DropZoneRestFill;

            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] dropped = (string[])e.Data.GetData(DataFormats.FileDrop);

            List<string> allFiles = new List<string>();
            foreach (string p in dropped)
            {
                if (Directory.Exists(p))
                {                    
                    allFiles.AddRange(Directory.GetFiles(p, "*", SearchOption.AllDirectories));
                }
                else if (File.Exists(p))
                {
                    allFiles.Add(p);
                }
            }

            string dlinkExtension = "." + Program.options.databank_dlink_name;
            foreach (string f in allFiles)
            {
                //Never let a .dlink file itself be dropped in as if it were a data file
                if (f.EndsWith(dlinkExtension, StringComparison.OrdinalIgnoreCase)) continue;
                if (_items.Any(r => G.Equal(r.Path, f))) continue; //already in the list
                AddRow(f);
            }

            DlinkAllButton.IsEnabled = _items.Count > 0;
        }

        private void AddRow(string filePath)
        {
            DlinkImportRow row = new DlinkImportRow { Path = filePath, Status = "Checking...", DlinkPathDisplay = "" };
            _items.Add(row);
            try
            {
                ComputeStatus(row);
            }
            catch (Exception ex)
            {
                row.Kind = DlinkImportRowKind.Unresolved;
                row.Status = "Error while checking: " + ex.Message;
                row.DlinkPathDisplay = "<No correspondence>"; // New
            }
        }

        /// <summary>
        /// Works out what, if anything, needs to happen for one dropped file: already correct,
        /// new/changed (regenerate via Blob()), or a detected move (relocate an orphaned .dlink).
        /// </summary>
        private void ComputeStatus(DlinkImportRow row)
        {
            string targetDlink = DlinkAutoDlinkFiles.Dlink_FromDataFileToDlinkFile(row.Path);
            row.TargetDlinkPath = targetDlink;
            if (targetDlink == null)
            {
                row.Kind = DlinkImportRowKind.OutsideRecognizedFolder;
                row.Status = "Outside the recognized data folder structure -- cannot place a .dlink";
                row.DlinkPathDisplay = "<No correspondence>"; // New
                return;
            }
            row.DlinkPathDisplay = targetDlink; 

            FileInfo fi = new FileInfo(row.Path);
            row.ComputedSize = fi.Length;
            row.ComputedStampUtc = fi.LastWriteTimeUtc;
            //Reuses the exact same hashing (and on-disk LRU cache) the Git hooks use -- a file
            //processed here is already "warm" for the very next commit's pre-commit check.
            row.ComputedHash = DlinkHooks.GetFileHash(row.Path, fi.Length, fi.LastWriteTimeUtc);

            if (File.Exists(targetDlink))
            {
                DlinkFile existing = G.YamlReader<DlinkFile>(targetDlink);
                if (existing != null && G.Equal(existing.hash, row.ComputedHash))
                {
                    row.Kind = DlinkImportRowKind.AlreadyInSync;
                    row.Status = "Already in sync";
                    return;
                }
                row.Kind = DlinkImportRowKind.NewOrChanged;
                row.Status = "Will be updated (existing .dlink has a different hash)";
                return;
            }

            //No .dlink at the target yet -- check whether this is really a move/rename of a file
            //that's tracked (and orphaned) somewhere else in the tree, before treating it as new.
            BuildOrphanIndexIfNeeded();
            List<string> candidates;
            if (_orphanIndex.TryGetValue(row.ComputedHash, out candidates) && candidates.Count > 0)
            {
                row.Kind = DlinkImportRowKind.Move;
                row.MoveFromDlinkPath = candidates[0];
                row.Status = "Move detected, from: " + candidates[0];
            }
            else
            {
                row.Kind = DlinkImportRowKind.NewOrChanged;
                row.Status = "New";
            }
        }

        /// <summary>
        /// Walks every .dlink file under the data root once, recording (hash -> path) for any
        /// whose expected data file is currently missing -- i.e. a genuine orphan, most likely
        /// left behind by a move/rename performed outside this tool (e.g. in Total Commander).
        /// Built once per window session; a real cost for a large tree, so only triggered the
        /// first time it's actually needed (see ComputeStatus).
        /// </summary>
        private void BuildOrphanIndexIfNeeded()
        {
            if (_orphanIndexBuilt) return;

            StatusText.Text = "Scanning existing .dlink files for orphans (moved/renamed files)...";
            PumpUiMessages();

            Dictionary<string, List<string>> index = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            string dataRoot = G.CleanupFolderName(Program.options.databank_dlink_folder_data, false);
            string dlinkPattern = "*." + Program.options.databank_dlink_name;

            if (Directory.Exists(dataRoot))
            {
                string[] dlinkFiles = Directory.GetFiles(dataRoot, dlinkPattern, SearchOption.AllDirectories);
                foreach (string dlinkFile in dlinkFiles)
                {
                    try
                    {
                        string dataFile = DlinkHooks.Dlink_FromDlinkFileToDataFile(dlinkFile);
                        if (G.NullOrBlanks(dataFile)) continue;
                        if (File.Exists(dataFile)) continue; //not an orphan -- its data file is right there

                        DlinkFile parsed = G.YamlReader<DlinkFile>(dlinkFile);
                        if (parsed == null || G.NullOrBlanks(parsed.hash)) continue;

                        List<string> list;
                        if (!index.TryGetValue(parsed.hash, out list))
                        {
                            list = new List<string>();
                            index[parsed.hash] = list;
                        }
                        list.Add(dlinkFile);
                    }
                    catch
                    {
                        //One unreadable/corrupt .dlink file should not abort the whole scan
                    }
                }
            }

            _orphanIndex = index;
            _orphanIndexBuilt = true;
            StatusText.Text = "";
        }

        private void RemoveRow_Click(object sender, RoutedEventArgs e)
        {
            DlinkImportRow row = (DlinkImportRow)((Button)sender).Tag;
            _items.Remove(row);
            DlinkAllButton.IsEnabled = _items.Count > 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // New: right-click "Copy data file path" -- copies the plain data file path shown on the
        // top line of the row.
        private void CopyDataFilePath_Click(object sender, RoutedEventArgs e)
        {
            DlinkImportRow row = GetRowFromContextMenuSender(sender);
            if (row == null || G.NullOrBlanks(row.Path)) return;
            TryCopyToClipboard(row.Path, "data file path");
        }

        // New: right-click "Copy dlink file path" -- copies the actual target .dlink path, not
        // the "<No correspondence>" placeholder text shown in that case (there is nothing useful
        // to copy for that row).
        private void CopyDlinkFilePath_Click(object sender, RoutedEventArgs e)
        {
            DlinkImportRow row = GetRowFromContextMenuSender(sender);
            if (row == null || G.NullOrBlanks(row.TargetDlinkPath))
            {
                StatusText.Text = "No .dlink path available to copy for this row.";
                return;
            }
            TryCopyToClipboard(row.TargetDlinkPath, ".dlink path");
        }

        // New: a right-click menu attached via RowStyle is not part of the row's visual tree (it
        // opens as a separate popup), so the clicked MenuItem's DataContext is not the row --
        // PlacementTarget (set by WPF to whichever row was actually right-clicked) is how to get
        // back to it.
        private DlinkImportRow GetRowFromContextMenuSender(object sender)
        {
            MenuItem menuItem = sender as MenuItem;
            ContextMenu contextMenu = menuItem != null ? menuItem.Parent as ContextMenu : null;
            FrameworkElement placementTarget = contextMenu != null ? contextMenu.PlacementTarget as FrameworkElement : null;
            return placementTarget != null ? placementTarget.DataContext as DlinkImportRow : null;
        }

        // New: Clipboard access can occasionally throw (e.g. another process briefly holds it) --
        // worth a try/catch rather than letting a copy-to-clipboard action crash the window.
        private void TryCopyToClipboard(string text, string whatForStatusText)
        {
            try
            {
                Clipboard.SetText(text);
                StatusText.Text = "Copied " + whatForStatusText + " to clipboard.";
            }
            catch (Exception ex)
            {
                StatusText.Text = "Could not copy " + whatForStatusText + " to clipboard: " + ex.Message;
            }
        }
                
        private void DlinkAllButton_Click(object sender, RoutedEventArgs e)
        {
            DlinkAllButton.IsEnabled = false;
            CancelButton.IsEnabled = false;

            HashSet<string> claimedOrphans = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            List<string> errors = new List<string>();

            for (int i = 0; i < _items.Count; i++)
            {
                DlinkImportRow row = _items[i];
                StatusText.Text = "Processing " + (i + 1) + " of " + _items.Count + "...";
                PumpUiMessages();
                ProcessRow(row, claimedOrphans, errors);
            }

            StatusText.Text = "Done.";
            DlinkAllButton.IsEnabled = true;
            CancelButton.IsEnabled = true;

            ReportLeftoverOrphans(claimedOrphans);
            ShowErrorsIfAny(errors);
        }
                
        private void DlinkSingleRow_Click(object sender, RoutedEventArgs e)
        {
            DlinkImportRow row = (DlinkImportRow)((Button)sender).Tag;

            HashSet<string> claimedOrphans = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            List<string> errors = new List<string>();

            StatusText.Text = "Processing " + System.IO.Path.GetFileName(row.Path) + "...";
            PumpUiMessages();

            ProcessRow(row, claimedOrphans, errors);

            StatusText.Text = "Done.";
            ReportLeftoverOrphans(claimedOrphans);
            ShowErrorsIfAny(errors);
        }

        // New: the actual per-row work, shared between DlinkAllButton_Click (looped over every
        // row) and DlinkSingleRow_Click (just the one row clicked).
        private void ProcessRow(DlinkImportRow row, HashSet<string> claimedOrphans, List<string> errors)
        {
            if (row.Kind == DlinkImportRowKind.OutsideRecognizedFolder || row.Kind == DlinkImportRowKind.Unresolved)
            {
                return; //status already explains why nothing can be done
            }

            try
            {
                if (row.Kind == DlinkImportRowKind.AlreadyInSync)
                {
                    row.Status = "Done (already in sync)";
                }
                else if (row.Kind == DlinkImportRowKind.Move && File.Exists(row.MoveFromDlinkPath))
                {
                    //A pure move/rename: the data did not change, so the orphaned .dlink can
                    //just be relocated -- no rehashing needed, since .dlink content depends
                    //only on the data, never on its path.
                    string targetDir = System.IO.Path.GetDirectoryName(row.TargetDlinkPath);
                    if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);
                    File.Move(row.MoveFromDlinkPath, row.TargetDlinkPath);
                    claimedOrphans.Add(row.MoveFromDlinkPath);
                    row.Status = "Done (moved from " + row.MoveFromDlinkPath + ")";
                }
                else
                {
                    //New or changed: (re)generate the .dlink from the current file content,
                    //exactly like Gekko itself does when it reads/writes this file.
                    DlinkAutoDlinkFiles.Blob(row.Path, null, true);
                    row.Status = "Done";
                }
            }
            catch (Exception ex)
            {
                //One bad file (unreadable, corrupt, permissions, ...) should not abort a batch --
                //or, for a single-row click, the row simply reports its own failure. Record the
                //error against this row (and the caller's list) and move on.
                row.Status = "Error: " + ex.Message;
                errors.Add(row.Path + ": " + ex.Message);
            }
        }
                
        private void ShowErrorsIfAny(List<string> errors)
        {
            if (errors.Count > 0)
            {
                MessageBox.Show(
                    errors.Count + " file(s) could not be processed:\n\n" + string.Join("\n", errors),
                    "Dlink -- errors", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// After processing, points out any existing .dlink files that have no matching data file
        /// AND were not claimed as a move by whatever was just processed -- rather than letting
        /// them silently accumulate. Left as a visible list for the user to act on (e.g. delete
        /// via Git); deletion itself needs no dedicated tool, since data files aren't Git-tracked
        /// -- only their .dlink pointers are.
        /// </summary>
        private void ReportLeftoverOrphans(HashSet<string> claimedOrphans)
        {
            if (!_orphanIndexBuilt)
            {
                OrphanSummaryText.Visibility = Visibility.Collapsed;
                return;
            }

            List<string> leftovers = new List<string>();
            foreach (KeyValuePair<string, List<string>> kv in _orphanIndex)
            {
                foreach (string dlinkPath in kv.Value)
                {
                    if (!claimedOrphans.Contains(dlinkPath) && File.Exists(dlinkPath))
                    {
                        leftovers.Add(dlinkPath);
                    }
                }
            }

            if (leftovers.Count == 0)
            {
                OrphanSummaryText.Visibility = Visibility.Collapsed;
                return;
            }

            leftovers.Sort(StringComparer.OrdinalIgnoreCase);
            OrphanSummaryText.Visibility = Visibility.Visible;
            OrphanSummaryText.Text =
                leftovers.Count + " existing .dlink file(s) have no matching data file and were not claimed by "
                + "anything processed so far. If the data is genuinely gone, delete these via Git -- "
                + "otherwise a future sync will silently re-fetch the old content from storage:\n"
                + string.Join("\n", leftovers);
        }

        /// <summary>
        /// Lets the UI repaint (status text, grid) between synchronous steps of a batch, since
        /// everything here runs on the UI thread -- see the class-level remark about that.
        /// </summary>
        private void PumpUiMessages()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
        }
    }
}
