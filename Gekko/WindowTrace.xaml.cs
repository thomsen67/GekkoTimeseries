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
        private TextBlock _detailsBlock;

        public WindowTrace()
        {
            InitializeComponent();
            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            this.Title = "Explorer Tree View";
            this.Width = 700;
            this.Height = 500;

            Grid rootGrid = new Grid();
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });

            // 1. ListView with GridView
            ListView listView = new ListView { ItemsSource = _visibleItems };
            listView.SelectionChanged += (s, e) => UpdateDetails(listView.SelectedItem as TreeRow);

            GridView gridView = new GridView();

            // Column 1: Country (with Tree Controls)
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Country",
                Width = 250,
                CellTemplate = CreateTreeCellTemplate()
            });

            // Column 2: Company
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Company",
                Width = 150,
                DisplayMemberBinding = new Binding("Company")
            });

            // Column 3: Product
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Product",
                Width = 150,
                DisplayMemberBinding = new Binding("Product")
            });

            listView.View = gridView;
            Grid.SetRow(listView, 0);
            rootGrid.Children.Add(listView);

            // Separator
            Separator sep = new Separator();
            Grid.SetRow(sep, 1);
            rootGrid.Children.Add(sep);

            // 2. Details Area
            _detailsBlock = new TextBlock { Padding = new Thickness(10), Background = Brushes.White };
            Grid.SetRow(_detailsBlock, 2);
            rootGrid.Children.Add(_detailsBlock);

            this.Content = rootGrid;
        }

        private void LoadData()
        {
            // Example data based on your format
            var rawLines = new[] {
                "0 -- Europe -- -- ",
                "1 -- Germany -- Volkswagen -- Golf",
                "2 -- Germany -- Volkswagen -- ID.4",
                "1 -- France -- Renault -- Clio",
                "0 -- Asia -- -- ",
                "1 -- Japan -- Toyota -- Corolla",
                "2 -- Japan -- Toyota -- Corolla Sport",
                "1 -- Korea -- Hyundai -- Ioniq"
            };

            // 1. Parse lines into objects
            for (int i = 0; i < rawLines.Length; i++)
            {
                var parts = rawLines[i].Split(new[] { "--" }, StringSplitOptions.None).Select(p => p.Trim()).ToArray();
                var item = new TreeRow
                {
                    Depth = int.Parse(parts[0]),
                    Country = parts.Length > 1 ? parts[1] : "",
                    Company = parts.Length > 2 ? parts[2] : "",
                    Product = parts.Length > 3 ? parts[3] : "",
                    IsExpanded = true // Default to expanded
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
            _detailsBlock.Text = string.Format("Country: {0}\nCompany: {1}\nProduct: {2}",
                selected.Country, selected.Company, selected.Product);
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
                    <TextBlock Text='{Binding Country}' VerticalAlignment='Center' FontWeight='SemiBold'/>
                </StackPanel>
            </DataTemplate>";
            return (DataTemplate)System.Windows.Markup.XamlReader.Parse(xaml);
        }
    }

    public class TreeRow : INotifyPropertyChanged
    {
        public int Depth { get; set; }
        public string Country { get; set; }
        public string Company { get; set; }
        public string Product { get; set; }
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
