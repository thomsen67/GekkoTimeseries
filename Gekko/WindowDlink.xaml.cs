using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gekko
{
    /// <summary>
    /// One row in the grid: a dropped data file and its computed status. Path/Status/
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

        // The .dlink path shown underneath Path in the grid, in gray. "<No correspondence>" for a
        // row that errored or falls outside the recognized data folder structure.
        private string _dlinkPathDisplay;
        public string DlinkPathDisplay
        {
            get { return _dlinkPathDisplay; }
            set { _dlinkPathDisplay = value; OnPropertyChanged("DlinkPathDisplay"); }
        }

        public DlinkImportRowKind Kind;
        public string TargetDlinkPath;      //where this file's .dlink should live
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
        NewOrChanged
    }

    /// <summary>
    /// Drag-and-drop tool for producing/updating .dlink files for data files that were added or
    /// changed outside Gekko (e.g. a plain file copy). For each dropped file it works out one of:
    /// already in sync (nothing to do) or new/changed (needs Blob()). Each row can be processed
    /// individually (its own [Dlink] button) or all at once ([Dlink all]).
    ///
    /// Everything that can be slow -- walking a dropped folder's contents, hashing a file to
    /// compute its status, or (re)writing a .dlink file via Blob() -- runs on a background thread
    /// (via Task.Run), so a large dropped folder or a big batch of files never freezes the window.
    /// Only the ObservableCollection<> and the DlinkImportRow property setters (which raise
    /// PropertyChanged, consumed by WPF data binding) are touched on the UI thread.
    /// </summary>
    public partial class WindowDlink : Window
    {
        private readonly ObservableCollection<DlinkImportRow> _items = new ObservableCollection<DlinkImportRow>();
        private static readonly Brush DropZoneRestFill = CreateFrozenBrush(0xF4, 0xF4, 0xF4);
        private static readonly Brush DropZoneHoverFill = CreateFrozenBrush(0xCC, 0xD5, 0xF0); //dusted blue

        // Guards against a second drop, or clicking Dlink/Dlink all, while something is already
        // running in the background.
        private bool _isBusy;

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
            bool ok = !_isBusy && e.Data.GetDataPresent(DataFormats.FileDrop);
            e.Effects = ok ? DragDropEffects.Copy : DragDropEffects.None;
            DropZoneBorder.Fill = ok ? DropZoneHoverFill : DropZoneRestFill;
            e.Handled = true;
        }

        private void DropZone_DragLeave(object sender, System.Windows.DragEventArgs e)
        {
            DropZoneBorder.Fill = DropZoneRestFill;
            e.Handled = true;
        }

        private async void DropZone_Drop(object sender, System.Windows.DragEventArgs e)
        {
            DropZoneBorder.Fill = DropZoneRestFill;

            if (_isBusy) return; //ignore drops while a previous drop/process is still being handled
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] dropped = (string[])e.Data.GetData(DataFormats.FileDrop);

            //Snapshot of what's already in the grid, taken here on the UI thread -- the
            //ObservableCollection itself is never touched from the background thread below.
            HashSet<string> alreadyPresent = new HashSet<string>(_items.Select(r => r.Path), StringComparer.OrdinalIgnoreCase);
            string dlinkExtension = "." + Program.options.databank_dlink_name;

            _isBusy = true;
            DlinkAllButton.IsEnabled = false;
            CancelButton.IsEnabled = false;
            StatusText.Text = "Scanning dropped item(s)...";

            try
            {
                //Walking a big folder tree (Directory.EnumerateFiles over "AllDirectories") is the
                //part that used to freeze the window on a large drop -- now off the UI thread.
                List<string> newFiles = await Task.Run(() => EnumerateDroppedFiles(dropped, dlinkExtension, alreadyPresent));

                if (newFiles.Count == 0)
                {
                    StatusText.Text = "No new files found in the dropped item(s).";
                    return;
                }

                for (int i = 0; i < newFiles.Count; i++)
                {
                    string filePath = newFiles[i];
                    StatusText.Text = "Checking file " + (i + 1) + " of " + newFiles.Count + "...";

                    DlinkImportRow row = new DlinkImportRow { Path = filePath, Status = "Checking...", DlinkPathDisplay = "" };
                    _items.Add(row);

                    //Computing the status involves hashing the file's contents -- also potentially
                    //slow, so it's done off the UI thread. The row's own properties are only ever
                    //written back on the UI thread, via ApplyStatusResult below.
                    StatusResult result;
                    try
                    {
                        result = await Task.Run(() => ComputeStatus(filePath));
                    }
                    catch (Exception)
                    {
                        result = new StatusResult
                        {
                            Kind = DlinkImportRowKind.Unresolved,
                            Status = "Error while checking",
                            DlinkPathDisplay = "<No correspondence>"
                        };
                    }
                    ApplyStatusResult(row, result);
                }

                StatusText.Text = "Ready.";
            }
            finally
            {
                _isBusy = false;
                DlinkAllButton.IsEnabled = _items.Count > 0;
                CancelButton.IsEnabled = true;
            }
        }

        // Runs entirely off the UI thread: expands dropped folders recursively, drops any .dlink
        // files themselves (never treated as data files) and anything already in the grid.
        private static List<string> EnumerateDroppedFiles(string[] dropped, string dlinkExtension, HashSet<string> alreadyPresent)
        {
            List<string> result = new List<string>();
            foreach (string p in dropped)
            {
                IEnumerable<string> filesHere;
                if (Directory.Exists(p))
                {
                    filesHere = Directory.EnumerateFiles(p, "*", SearchOption.AllDirectories);
                }
                else if (File.Exists(p))
                {
                    filesHere = new[] { p };
                }
                else
                {
                    continue;
                }

                foreach (string f in filesHere)
                {
                    if (f.EndsWith(dlinkExtension, StringComparison.OrdinalIgnoreCase)) continue; //never a .dlink file itself
                    if (alreadyPresent.Contains(f)) continue; //already in the list
                    result.Add(f);
                }
            }
            return result;
        }

        // Plain data carrier for what ComputeStatus works out for one file. Kept separate from
        // DlinkImportRow so that background-thread code never sets a bound property directly --
        // only the UI thread does that, in ApplyStatusResult.
        private class StatusResult
        {
            public DlinkImportRowKind Kind;
            public string Status;
            public string DlinkPathDisplay;
            public string TargetDlinkPath;
            public string ComputedHash;
            public long ComputedSize;
            public DateTime ComputedStampUtc;
        }

        /// <summary>
        /// Works out what, if anything, needs to happen for one dropped file: already correct, or
        /// new/changed (regenerate via Blob()). Safe to call from a background thread -- it only
        /// reads/hashes the file on disk and returns a plain result, touching no UI-bound state.
        /// </summary>
        private static StatusResult ComputeStatus(string filePath)
        {
            StatusResult result = new StatusResult();

            string targetDlink = DlinkAutoDlinkFiles.Dlink_FromDataFileToDlinkFile(filePath);
            result.TargetDlinkPath = targetDlink;
            if (targetDlink == null)
            {
                result.Kind = DlinkImportRowKind.OutsideRecognizedFolder;
                result.Status = "Outside the recognized data folder structure -- cannot place a .dlink";
                result.DlinkPathDisplay = "<No correspondence>";
                return result;
            }
            result.DlinkPathDisplay = targetDlink;

            FileInfo fi = new FileInfo(filePath);
            result.ComputedSize = fi.Length;
            result.ComputedStampUtc = fi.LastWriteTimeUtc;
            //Reuses the exact same hashing (and on-disk LRU cache) the Git hooks use -- a file
            //processed here is already "warm" for the very next commit's pre-commit check.
            result.ComputedHash = DlinkHooks.GetFileHash(filePath, fi.Length, fi.LastWriteTimeUtc);

            if (File.Exists(targetDlink))
            {
                DlinkFile existing = G.YamlReader<DlinkFile>(targetDlink);
                if (existing != null && G.Equal(existing.hash, result.ComputedHash))
                {
                    result.Kind = DlinkImportRowKind.AlreadyInSync;
                    result.Status = "Already in sync";
                    return result;
                }
                result.Kind = DlinkImportRowKind.NewOrChanged;
                result.Status = "Will be updated (existing .dlink differs)";
                return result;
            }

            result.Kind = DlinkImportRowKind.NewOrChanged;
            result.Status = "New";
            return result;
        }

        // Only ever called on the UI thread: copies a background-computed StatusResult onto the
        // bound row, which is what actually updates the grid.
        private static void ApplyStatusResult(DlinkImportRow row, StatusResult result)
        {
            row.Kind = result.Kind;
            row.TargetDlinkPath = result.TargetDlinkPath;
            row.DlinkPathDisplay = result.DlinkPathDisplay;
            row.ComputedHash = result.ComputedHash;
            row.ComputedSize = result.ComputedSize;
            row.ComputedStampUtc = result.ComputedStampUtc;
            row.Status = result.Status;
        }

        private void RemoveRow_Click(object sender, RoutedEventArgs e)
        {
            if (_isBusy) return;
            DlinkImportRow row = (DlinkImportRow)((Button)sender).Tag;
            _items.Remove(row);
            DlinkAllButton.IsEnabled = _items.Count > 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CopyDataFilePath_Click(object sender, RoutedEventArgs e)
        {
            DlinkImportRow row = GetRowFromContextMenuSender(sender);
            if (row == null || G.NullOrBlanks(row.Path)) return;
            TryCopyToClipboard(row.Path, "data file path");
        }

        // Copies the actual target .dlink path, not the "<No correspondence>" placeholder text
        // shown in that case (there is nothing useful to copy for that row).
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

        private DlinkImportRow GetRowFromContextMenuSender(object sender)
        {
            MenuItem menuItem = sender as MenuItem;
            ContextMenu contextMenu = menuItem != null ? menuItem.Parent as ContextMenu : null;
            FrameworkElement placementTarget = contextMenu != null ? contextMenu.PlacementTarget as FrameworkElement : null;
            return placementTarget != null ? placementTarget.DataContext as DlinkImportRow : null;
        }

        // Clipboard access can occasionally throw (e.g. another process briefly holds it) --
        // worth a try/catch rather than letting a copy-to-clipboard action crash the window.
        private void TryCopyToClipboard(string text, string whatForStatusText)
        {
            try
            {
                Clipboard.SetText(text);
                StatusText.Text = "Copied " + whatForStatusText + " to clipboard.";
            }
            catch (Exception)
            {
                StatusText.Text = "Could not copy " + whatForStatusText + " to clipboard";
            }
        }

        private async void DlinkAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isBusy) return;
            _isBusy = true;
            DlinkAllButton.IsEnabled = false;
            CancelButton.IsEnabled = false;

            List<string> errors = new List<string>();

            for (int i = 0; i < _items.Count; i++)
            {
                DlinkImportRow row = _items[i];
                StatusText.Text = "Processing " + (i + 1) + " of " + _items.Count + "...";
                await ProcessRowAsync(row, errors);
            }

            StatusText.Text = "Done.";
            _isBusy = false;
            DlinkAllButton.IsEnabled = _items.Count > 0;
            CancelButton.IsEnabled = true;

            ShowErrorsIfAny(errors);
        }

        private async void DlinkSingleRow_Click(object sender, RoutedEventArgs e)
        {
            if (_isBusy) return;
            _isBusy = true;
            DlinkImportRow row = (DlinkImportRow)((Button)sender).Tag;

            List<string> errors = new List<string>();

            StatusText.Text = "Processing " + System.IO.Path.GetFileName(row.Path) + "...";

            await ProcessRowAsync(row, errors);

            StatusText.Text = "Done.";
            _isBusy = false;

            ShowErrorsIfAny(errors);
        }

        // The actual per-row work, shared between DlinkAllButton_Click (looped over every row)
        // and DlinkSingleRow_Click (just the one row clicked). Blob() does file I/O and hashing,
        // so it runs via Task.Run -- everything before/after that stays on the UI thread and is
        // free to touch the bound row/StatusText directly.
        private async Task ProcessRowAsync(DlinkImportRow row, List<string> errors)
        {
            if (row.Kind == DlinkImportRowKind.OutsideRecognizedFolder || row.Kind == DlinkImportRowKind.Unresolved)
            {
                return; //status already explains why nothing can be done
            }

            if (row.Kind == DlinkImportRowKind.AlreadyInSync)
            {
                row.Status = "Done (already in sync)";
                return;
            }

            try
            {
                //New or changed: (re)generate the .dlink from the current file content,
                //exactly like Gekko itself does when it reads/writes this file.
                //Note: done like this, for .gbk files the .dlink files will not get info on #vars and #series
                //To Claude: the above statement is wrong no, isn't it (since variables and series are part of the ReadInfo object now)
                await Task.Run(() => DlinkAutoDlinkFiles.Blob(row.Path, true));
                row.Status = "Done";
            }
            catch (Exception)
            {
                //One bad file (unreadable, corrupt, permissions, ...) should not abort a batch --
                //or, for a single-row click, the row simply reports its own failure. Record the
                //error against this row (and the caller's list) and move on.
                row.Status = "Error";
                errors.Add(row.Path);
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
    }
}
