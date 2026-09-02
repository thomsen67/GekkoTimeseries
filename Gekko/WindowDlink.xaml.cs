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

        private long _computedSize;
        public long ComputedSize
        {
            get { return _computedSize; }
            //New: was a plain field -- needs to raise PropertyChanged now that SizeMB (bound in the
            //grid's MB column) derives from it.
            set { _computedSize = value; OnPropertyChanged("ComputedSize"); OnPropertyChanged("SizeMB"); }
        }

        // New: bound to the grid's MB column. Left unrounded here -- the column's StringFormat
        // handles display rounding to 1 decimal, while sorting still compares this full-precision
        // value, which is more correct/stable than sorting on a pre-rounded number.
        public double SizeMB
        {
            get { return ComputedSize / (1024.0 * 1024.0); }
        }

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
    /// changed outside Gekko (e.g. a plain file copy).    
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
            UpdateFooter(); //New: initializes "Files = 0"
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
                List<string> newFiles = await Task.Run(() => EnumerateDroppedFiles(dropped, dlinkExtension, alreadyPresent));

                if (newFiles.Count == 0)
                {
                    StatusText.Text = "No new files found in the dropped item(s).";
                    return;
                }

                for (int i = 0; i < newFiles.Count; i++)
                {
                    string filePath = newFiles[i];
                    //New: full path, matching DlinkSyncFiles() in Dlink.cs
                    StatusText.Text = "Checking " + (i + 1) + " of " + newFiles.Count + ": " + filePath;

                    DlinkImportRow row = new DlinkImportRow { Path = filePath, Status = "Checking...", DlinkPathDisplay = "" };
                    _items.Add(row);
                    UpdateFooter(); //New: keeps "Files = ..." live as rows stream in, not just once at the end
             
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
                await Task.Run(() => DlinkHashCache.Save());
                _isBusy = false;
                UpdateFooter();
                CancelButton.IsEnabled = true;
            }
        }

        // New: renamed from UpdateButtonEnabledStates -- now also keeps the "Files = ..." counter
        // current. Called everywhere _items' count can change (a drop, a remove, a delete).
        private void UpdateFooter()
        {
            bool hasRows = _items.Count > 0;
            DlinkAllButton.IsEnabled = hasRows;
            RemoveInSyncButton.IsEnabled = hasRows;
            FileCountText.Text = "Files = " + _items.Count;
        }

        // Runs entirely off the UI thread.
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
        /// Works out what, if anything, needs to happen for one dropped file.
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
                result.Status = "Not in sync";
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
            UpdateFooter();
        }
        
        private void RemoveInSyncButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isBusy) return;
            List<DlinkImportRow> toRemove = _items.Where(r => r.Kind == DlinkImportRowKind.AlreadyInSync || r.Status == "Done").ToList();
            foreach (DlinkImportRow row in toRemove)
            {
                _items.Remove(row);
            }
            UpdateFooter();
            StatusText.Text = toRemove.Count == 0
                ? "No rows already in sync to remove."
                : "Removed " + toRemove.Count + " row" + G.S(toRemove.Count) + " already in sync.";
        }

        // Removes every currently-selected row -- not just the one that was right-clicked, so
        // Shift/Ctrl-click a range first, then right-click anywhere within it (see
        // Row_PreviewMouseRightButtonDown, which makes sure right-clicking doesn't collapse an
        // existing multi-selection down to just the clicked row).
        private void DeleteSelectedRows_Click(object sender, RoutedEventArgs e)
        {
            if (_isBusy) return;
            List<DlinkImportRow> toRemove = FilesGrid.SelectedItems.Cast<DlinkImportRow>().ToList();
            foreach (DlinkImportRow row in toRemove)
            {
                _items.Remove(row);
            }
            UpdateFooter();
        }

        // Right-clicking a row that isn't part of the current multi-selection selects just
        // that row (replacing whatever was selected before) -- e.g. Windows Explorer does the same.
        // Right-clicking WITHIN an existing multi-selection leaves it untouched, which is what makes
        // "Shift/Ctrl-click a range, then right-click it" work for Delete selected row(s).
        private void Row_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            if (row != null && !row.IsSelected)
            {
                FilesGrid.SelectedItems.Clear();
                row.IsSelected = true;
            }
        }

        // New: explicit Up/Down handling for row navigation. Handled at the DataGrid level via
        // PreviewKeyDown (tunneling), so it fires before -- and takes priority over -- whatever a
        // focused child control (e.g. one of the Dlink/Remove buttons) would otherwise do with the
        // key, and regardless of why the grid's own built-in arrow-key navigation wasn't doing this.
        // Moves/replaces the selection by one row in the CURRENTLY DISPLAYED order, so this still
        // makes sense after the user has sorted the grid (e.g. by clicking the MB column header).
        private void FilesGrid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Up && e.Key != System.Windows.Input.Key.Down) return;
            if (FilesGrid.Items.Count == 0) return;

            int currentIndex = FilesGrid.SelectedIndex;
            int newIndex;
            if (e.Key == System.Windows.Input.Key.Up)
            {
                newIndex = currentIndex <= 0 ? 0 : currentIndex - 1;
            }
            else
            {
                newIndex = currentIndex < 0 ? 0 : Math.Min(currentIndex + 1, FilesGrid.Items.Count - 1);
            }

            FilesGrid.SelectedItems.Clear();
            FilesGrid.SelectedIndex = newIndex;
            FilesGrid.ScrollIntoView(FilesGrid.Items[newIndex]);
            e.Handled = true;
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
            RemoveInSyncButton.IsEnabled = false;
            CancelButton.IsEnabled = false;

            List<string> errors = new List<string>();

            //New: full path + throttled to at most one status update per 100ms, matching
            //DlinkSyncFiles() in Dlink.cs -- otherwise a big batch of small/fast rows pays a UI
            //update cost on every single one.
            System.Diagnostics.Stopwatch progressStopwatch = System.Diagnostics.Stopwatch.StartNew();
            const int progressReportIntervalMs = 100;

            for (int i = 0; i < _items.Count; i++)
            {
                DlinkImportRow row = _items[i];
                bool isLastRow = (i == _items.Count - 1);
                if (isLastRow || progressStopwatch.ElapsedMilliseconds >= progressReportIntervalMs)
                {
                    StatusText.Text = "Processing " + (i + 1) + " of " + _items.Count + ": " + row.Path;
                    progressStopwatch.Restart();
                }
                await ProcessRowAsync(row, errors);
            }
            
            await Task.Run(() => DlinkHashCache.Save());

            StatusText.Text = "Done.";
            _isBusy = false;
            UpdateFooter();
            CancelButton.IsEnabled = true;

            ShowErrorsIfAny(errors);
        }

        private async void DlinkSingleRow_Click(object sender, RoutedEventArgs e)
        {
            if (_isBusy) return;
            _isBusy = true;
            DlinkImportRow row = (DlinkImportRow)((Button)sender).Tag;

            List<string> errors = new List<string>();

            StatusText.Text = "Processing " + row.Path + "..."; //New: full path, was Path.GetFileName(row.Path)

            await ProcessRowAsync(row, errors);

            await Task.Run(() => DlinkHashCache.Save()); //New: see the note in DlinkAllButton_Click above

            StatusText.Text = "Done.";
            _isBusy = false;

            ShowErrorsIfAny(errors);
        }

        // The actual per-row work, shared between DlinkAllButton_Click (looped over every row)
        // and DlinkSingleRow_Click (just the one row clicked).
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
                await Task.Run(() => DlinkAutoDlinkFiles.Blob(row.Path, true));
                row.Status = "Done";
            }
            catch (Exception)
            {
                //One bad file (unreadable, corrupt, permissions, ...) should not abort a batch.
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
