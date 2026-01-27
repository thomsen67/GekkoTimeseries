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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Gekko
{
    ///// <summary>
    ///// Interaction logic for WindowTrace.xaml
    ///// </summary>
    //public partial class WindowTrace : UserControl
    //{
    //    public WindowTrace()
    //    {
    //        InitializeComponent();
    //    }
    //}

    public partial class WindowTrace : Window
    {
        private ObservableCollection<TreeRow> _visibleItems = new ObservableCollection<TreeRow>();
        private List<TreeRow> _allItems = new List<TreeRow>();
        private TextBox _detailsBlock;

        public WindowTrace(List<string> input)
        {
            InitializeComponent();
            SetupUI();
            LoadData(input);
        }

        private void SetupUI()
        {
            this.Title = "Gekko data-trace";
            this.Width = 800;
            this.Height = 600;
            this.Top = 20;
            this.Left = 150;

            Grid rootGrid = new Grid();
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });

            // 1. ListView with GridView
            ListView listView = new ListView { ItemsSource = _visibleItems };
            listView.SelectionChanged += (s, e) => UpdateDetails(listView.SelectedItem as TreeRow);
            
            GridView gridView = new GridView();
                        
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Name",
                Width = 200,
                CellTemplate = CreateTreeCellTemplate(),                
                //DisplayMemberBinding = new Binding("Name")
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
            Grid.SetRow(listView, 0);
            rootGrid.Children.Add(listView);

            // Separator
            Separator sep = new Separator();
            Grid.SetRow(sep, 1);
            rootGrid.Children.Add(sep);

            // 2. Details Area
            _detailsBlock = new TextBox { Padding = new Thickness(10), Background = Brushes.LightYellow };
            Grid.SetRow(_detailsBlock, 2);
            rootGrid.Children.Add(_detailsBlock);

            this.Content = rootGrid;
        }

        private void LoadData(List<string> rawLines)
        {
            //// Example data based on your format
            //var rawLines = new[] {
            //    "0 -- Europe -- -- ",
            //    "1 -- Germany -- Volkswagen -- Golf",
            //    "2 -- Germany -- Volkswagen -- ID.4",
            //    "1 -- France -- Renault -- Clio",
            //    "0 -- Asia -- -- ",
            //    "1 -- Japan -- Toyota -- Corolla",
            //    "2 -- Japan -- Toyota -- Corolla Sport",
            //    "1 -- Korea -- Hyundai -- Ioniq"
            //};

            // 1. Parse lines into objects
            for (int i = 0; i < rawLines.Count; i++)
            {
                var parts = rawLines[i].Split(new[] { "¤" }, StringSplitOptions.None).Select(p => p.Trim()).ToArray();
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
            _detailsBlock.Text = string.Format("{0}\n--------------------------------------------------\nName: {1}\nPeriod: {2}\nFile: {3}\nDatafile: {4}\nStamp: {5}\nVars: {6}",
                Trace2.RemoveNewlines(selected.Code), selected.Name, selected.Period, selected.File, selected.DataFile, selected.Stamp, selected.Variables);
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
