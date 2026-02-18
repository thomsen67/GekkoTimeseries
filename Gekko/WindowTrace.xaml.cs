using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace Gekko
{    

    public partial class WindowTrace : Window
    {
        private ObservableCollection<TreeRow> _visibleItems = new ObservableCollection<TreeRow>();
        private List<TreeRow> _allItems = new List<TreeRow>();
        private TextBox _detailsBlock;

        public WindowTrace(List<string> input, string nameEtc)
        {
            InitializeComponent();
            SetupUI();
            LoadData(input);
            this.KeyDown += MainWindow_KeyDown;
            _detailsBlock.Text = nameEtc + " (" + input.Count + " data-traces, possibly with dublets)\n\nGekko 2.5.4+ trace viewer (experimental). Frequencies are indicated with '!'.\nSome trace features from Gekko 3.x are ported, but bugs and limitations may occur.\nFor instance, time period 'shadowing' of existing traces is not handled well.\n\nClick '[+]' to unfold sub-traces. Click a row to see more trace info.";
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.Close();
            }
        }

        private void SetupUI()
        {
            this.Title = "Gekko data-trace (experimental)";
            this.Width = 900;
            this.Height = 600;
            this.Top = 20;
            this.Left = 150;

            // 1. Define the Grid and Rows
            Grid rootGrid = new Grid();

            // Top Row (Tree) - "*" means it takes available space
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star), MinHeight = 100 });

            // Middle Row (The Splitter handle) - "Auto" fits the splitter's height
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Bottom Row (Details) - Fixed initial height, but resizable
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150), MinHeight = 50 });

            // 2. The ListView (Tree View)
            ListView listView = new ListView
            {
                ItemsSource = _visibleItems,
                //FontFamily = new FontFamily("Segoe UI"),
                //FontSize = 13
            };
            listView.SelectionChanged += (s, e) => UpdateDetails(listView.SelectedItem as TreeRow);

            GridView gridView = new GridView();

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Name",
                Width = 200,
                CellTemplate = CreateTreeCellTemplate(),
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Code",
                Width = 400,
                DisplayMemberBinding = new Binding("Code")
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Period",
                Width = 80,
                DisplayMemberBinding = new Binding("Period")
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Stamp",
                Width = 80,
                DisplayMemberBinding = new Binding("Stamp")
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "File",
                Width = 140,
                DisplayMemberBinding = new Binding("File")
            });

            listView.View = gridView;

            Grid.SetRow(listView, 0); // Put in Row 0
            rootGrid.Children.Add(listView);

            // 3. The GridSplitter (The Draggable Divider)
            GridSplitter splitter = new GridSplitter
            {
                Height = 5,                          // Thickness of the handle
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.Gainsboro,      // Color of the bar
                ShowsPreview = true                  // Shows a ghost line while dragging
            };
            Grid.SetRow(splitter, 1); // Put in Row 1
            rootGrid.Children.Add(splitter);

            // 4. The Details Area (Wrapped in a ScrollViewer in case text is long)
            ScrollViewer scrollBox = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            _detailsBlock = new TextBox
            {
                Padding = new Thickness(10),
                Background = Brushes.LightYellow,
                FontFamily = new FontFamily("Consolas"), // Monospace looks better for "Code: Value" pairs
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap
            };
            scrollBox.Content = _detailsBlock;

            Grid.SetRow(scrollBox, 2); // Put in Row 2
            rootGrid.Children.Add(scrollBox);

            this.Content = rootGrid;
        }

        private void LoadData(List<string> rawLines)
        {
            // 1. Parse lines into objects
            for (int i = 0; i < rawLines.Count; i++)
            {
                var parts = rawLines[i].Split(new[] { "{tce}" }, StringSplitOptions.None).Select(p => p.Trim()).ToArray();
                var item = new TreeRow
                {
                    Depth = int.Parse(parts[0]),
                    Name = parts.Length > 1 ? parts[1] : "",
                    Period = parts.Length > 2 ? parts[2] : "",
                    Code = parts.Length > 3 ? parts[3] : "",
                    Variables = parts.Length > 4 ? parts[4] : "",
                    File = parts.Length > 5 ? parts[5] : "",
                    DataFile = parts.Length > 6 ? parts[6] : "",
                    Stamp = parts.Length > 7 ? parts[7] : "",
                    // ---
                    NameLong = parts.Length > 8 ? parts[8] : "",
                    PeriodLong = parts.Length > 9 ? parts[9] : "",
                    CodeLong = parts.Length > 10 ? parts[10] : "",
                    VariablesLong = parts.Length > 11 ? parts[11] : "",
                    FileLong = parts.Length > 12 ? parts[12] : "",
                    DataFileLong = parts.Length > 13 ? parts[13] : "",
                    StampLong = parts.Length > 14 ? parts[14] : "",
                    // ---
                    IsExpanded = false // Default to expanded
                };

                // Subscribe to expansion changes
                item.PropertyChanged += (s, e) => {
                    if (e.PropertyName == "IsExpanded") RefreshVisibleItems();
                };

                _allItems.Add(item);
            }

            // 2. Determine if items have children (if the NEXT item is deeper)
            for (int i = 0; i < _allItems.Count; i++)
            {
                if (i + 1 < _allItems.Count)
                    _allItems[i].HasChildren = _allItems[i + 1].Depth > _allItems[i].Depth;
            }

            RefreshVisibleItems();
        }

        private void RefreshVisibleItems()
        {
            _visibleItems.Clear();
            int skipUntilDepth = -1;

            foreach (var item in _allItems)
            {
                // If we are currently skipping children of a collapsed parent
                if (skipUntilDepth != -1)
                {
                    if (item.Depth > skipUntilDepth) continue;
                    else skipUntilDepth = -1; // We reached a sibling or a higher parent
                }

                _visibleItems.Add(item);

                // If this item has children but is collapsed, skip everything until we hit same depth
                if (item.HasChildren && !item.IsExpanded)
                {
                    skipUntilDepth = item.Depth;
                }
            }
        }

        private void UpdateDetails(TreeRow selected)
        {
            if (selected == null) return;            
            string[] ss = selected.FileLong.Split('¤');
            string xx = ss[0];
            if (ss.Length > 1) xx += " line " + ss[1];
            xx = xx.Trim();
            _detailsBlock.Text = string.Format("{0}\n--------------------------------------------------\nName: {1}\nPeriod: {2}\nFile: {3}\nDatafile: {4}\nStamp: {5}\nVars: {6}",
                selected.CodeLong, selected.NameLong, selected.PeriodLong, xx, selected.DataFileLong, selected.StampLong, selected.VariablesLong);
        }

        private DataTemplate CreateTreeCellTemplate()
        {
            // Note: TwoWay binding on IsExpanded is crucial for the ToggleButton to work
            string xaml = @"
            <DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                <StackPanel Orientation='Horizontal' Margin='{Binding IndentMargin}'>
                    <ToggleButton Width='18' Height='18' Margin='0,0,5,0' 
                                  IsChecked='{Binding IsExpanded, Mode=TwoWay}' 
                                  Visibility='{Binding ExpanderVisibility}'>
                        <ToggleButton.Style>
                            <Style TargetType='ToggleButton'>
                                <Setter Property='Content' Value='+'/>
                                <Style.Triggers>
                                    <Trigger Property='IsChecked' Value='True'>
                                        <Setter Property='Content' Value='-'/>
                                    </Trigger>
                                </Style.Triggers>
                            </Style>
                        </ToggleButton.Style>
                    </ToggleButton>
                    <TextBlock Text='{Binding Name}' VerticalAlignment='Center'/>
                </StackPanel>
            </DataTemplate>";
            return (DataTemplate)System.Windows.Markup.XamlReader.Parse(xaml);
        }
    }

    public class TreeRow : INotifyPropertyChanged
    {
        public int Depth { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Period { get; set; }
        public string Stamp { get; set; }
        public string File { get; set; }
        public string DataFile { get; set; }
        public string Variables { get; set; }

        // -------
                
        public string NameLong { get; set; }
        public string CodeLong { get; set; }
        public string PeriodLong { get; set; }
        public string StampLong { get; set; }
        public string FileLong { get; set; }
        public string DataFileLong { get; set; }
        public string VariablesLong { get; set; }

        // -------

        public bool HasChildren { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { _isExpanded = value; OnPropertyChanged("IsExpanded"); }
        }

        // UI Helpers
        public Thickness IndentMargin { get { return new Thickness(Depth * 20, 0, 0, 0); } }

        public Visibility ExpanderVisibility
        {
            get { return HasChildren ? Visibility.Visible : Visibility.Hidden; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
