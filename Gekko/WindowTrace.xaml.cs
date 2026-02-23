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
        private TextBox _detailsBlock;

        public WindowTrace(Trace2 rootNode, string nameEtc)
        {
            SetupUI();

            if (true)
            {
                // Check if the root has children
                if (rootNode?.precedents?.storage != null)
                {
                    foreach (Trace2 child in rootNode.precedents.storage)
                    {
                        // Add children at Depth 0 so they appear at the left margin
                        _visibleItems.Insert(0, CreateRowFromNode(child, 0)); //was: _visibleItems.Add(CreateRowFromNode(child, 0));
                    }
                }
            }
            else
            {
                // Convert the starting node into the first visible row
                _visibleItems.Add(CreateRowFromNode(rootNode, 0));
            }

            int max, n; Trace2.GetNumberOfTracesAndDepth(rootNode, out max, out n);

            _detailsBlock.Text = nameEtc + " (" + n + " data-traces)\n\nGekko 2.5.4+ trace viewer (experimental). Some trace features from Gekko 3.x are ported, but bugs and limitations may occur, for instance there is limited time period 'shadowing' of existing traces. Traces with the LHS variable on the RHS (like x = x + y, x = x[-1] + y) may be somewhat scrambled trace-wise, and traces originating from the inside of a loop (like x[%t] = x[%t+1] * b[%t]) may only show one of the periods. All in all, the data-traces are probably useful, but should be taken as hints rather than the truth.\n\nClick '[+]' to unfold sub-traces. Click a row to see more trace info. Frequencies are indicated with '!'.";

            this.KeyDown += (s, e) => { if (e.Key == System.Windows.Input.Key.Escape) this.Close(); };
        }
        
        private TreeRow CreateRowFromNode(Trace2 node, int depth)
        {
            string prec, name, period, code, file, datafile, id;
            Trace2.TracePretty(node, out prec, out name, out period, out code, out file, out datafile, out id);
            //traceLines.Add((depth - 1) + d + name + d + period + d + code + d + prec + d + file + d + datafile + d + id + d + parent.traceContents.name + d + parent.traceContents.period + d + parent.traceContents.text + d + prec + d + parent.traceContents.commandFileAndLine + d + parent.traceContents.dataFile + d + parent.traceContents.id);

            var row = new TreeRow
            {
                Depth = depth,
                SourceNode = node, // Keep a reference to the data
                Name = name,
                Code = code,
                Period = period,
                Stamp = id,
                File = file,
                HasChildren = node.precedents.storage != null && node.precedents.storage.Count > 0,
                IsExpanded = false
            };

            // Hook into expansion logic
            row.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "IsExpanded")
                {
                    if (row.IsExpanded) Expand(row);
                    else Collapse(row);
                }
            };

            return row;
        }

        private void Expand(TreeRow parentRow)
        {
            int index = _visibleItems.IndexOf(parentRow);
            if (index == -1) return;
            
            List<Trace2> childrenNodes = parentRow.SourceNode.precedents.storage;
            if (childrenNodes == null) return;

            // By looping 0 to Count and always inserting at 'index + 1',
            // the last child (e.g. Child 3) will end up at the top of the sub-list.
            for (int i = 0; i < childrenNodes.Count; i++)
            {
                var childRow = CreateRowFromNode(childrenNodes[i], parentRow.Depth + 1);
                // Always insert at the spot immediately below the parent, was: _visibleItems.Insert(index + 1 + i, childRow);
                _visibleItems.Insert(index + 1, childRow);
            }
        }

        private void Collapse(TreeRow parentRow)
        {
            int index = _visibleItems.IndexOf(parentRow);
            if (index == -1) return;

            int removeAt = index + 1;
            while (removeAt < _visibleItems.Count && _visibleItems[removeAt].Depth > parentRow.Depth)
            {
                _visibleItems.RemoveAt(removeAt);
            }
        }

        private void UpdateDetails(TreeRow selected)
        {
            if (selected?.SourceNode == null) return;
            Trace2 n = selected.SourceNode;

            if (selected == null) return;
            string[] ss = n.traceContents.commandFileAndLine.Split('¤');
            string xx = ss[0];
            if (ss.Length > 1) xx += " line " + ss[1];
            xx = xx.Trim();
            //_detailsBlock.Text = string.Format("{0}\n--------------------------------------------------\nName: {1}\nPeriod: {2}\nFile: {3}\nDatafile: {4}\nStamp: {5}\nVars: {6}",
            //    selected.CodeLong, selected.NameLong, selected.PeriodLong, xx, selected.DataFileLong, selected.StampLong, selected.VariablesLong);

            string prec, name, period, code, file, datafile, id;
            Trace2.TracePretty(n, out prec, out name, out period, out code, out file, out datafile, out id);
            //traceLines.Add((depth - 1) + d + name + d + period + d + code + d + prec + d + file + d + datafile + d + id + d + parent.traceContents.name + d + parent.traceContents.period + d + parent.traceContents.text + d + prec + d + parent.traceContents.commandFileAndLine + d + parent.traceContents.dataFile + d + parent.traceContents.id);

            _detailsBlock.Text = $"{n.traceContents.text}\n--------------------------------------------------\nName: {n.traceContents.name}\nPeriod: {n.traceContents.period.ToString()}\nFile: {xx}\nDatafile: {n.traceContents.dataFile}\nStamp: {n.traceContents.id.ToString()}\nVars: {prec}";
        }

        private void SetupUI()
        {
            this.Title = "Gekko data-trace viewer";
            this.Width = 900; this.Height = 600;
            this.Top = 30;

            Grid rootGrid = new Grid();
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });

            ListView lv = new ListView { ItemsSource = _visibleItems };
            lv.SelectionChanged += (s, e) => UpdateDetails(lv.SelectedItem as TreeRow);

            GridView gv = new GridView();
            gv.Columns.Add(new GridViewColumn { Header = "Name", Width = 200, CellTemplate = CreateTreeCellTemplate() });
            gv.Columns.Add(new GridViewColumn { Header = "Code", Width = 400, DisplayMemberBinding = new Binding("Code") });
            gv.Columns.Add(new GridViewColumn { Header = "Period", Width = 80, DisplayMemberBinding = new Binding("Period") });
            gv.Columns.Add(new GridViewColumn { Header = "Stamp", Width = 80, DisplayMemberBinding = new Binding("Stamp") });
            gv.Columns.Add(new GridViewColumn { Header = "File", Width = 200, DisplayMemberBinding = new Binding("File") });
            lv.View = gv;

            Grid.SetRow(lv, 0); rootGrid.Children.Add(lv);

            GridSplitter gs = new GridSplitter { Height = 4, HorizontalAlignment = HorizontalAlignment.Stretch, Background = Brushes.Gray };
            Grid.SetRow(gs, 1); rootGrid.Children.Add(gs);

            _detailsBlock = new TextBox { IsReadOnly = true, Background = Brushes.LightYellow, TextWrapping = TextWrapping.Wrap, Padding = new Thickness(10), FontFamily = new FontFamily("Courier New") };
            Grid.SetRow(_detailsBlock, 2); rootGrid.Children.Add(_detailsBlock);

            this.Content = rootGrid;
        }

        private DataTemplate CreateTreeCellTemplate()
        {
            string xaml = @"
            <DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                <StackPanel Orientation='Horizontal' Margin='{Binding IndentMargin}'>
                    <ToggleButton Width='18' Height='18' IsChecked='{Binding IsExpanded, Mode=TwoWay}' 
                                  Visibility='{Binding ExpanderVisibility}' Margin='0,0,5,0'>
                        <ToggleButton.Style>
                            <Style TargetType='ToggleButton'>
                                <Setter Property='Content' Value='+'/>
                                <Style.Triggers>
                                    <Trigger Property='IsChecked' Value='True'><Setter Property='Content' Value='-'/></Trigger>
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
        public Trace2 SourceNode { get; set; } // The real data object
        public int Depth { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Period { get; set; }
        public string Stamp { get; set; }
        public string File { get; set; }
        public bool HasChildren { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged("IsExpanded"); }
        }

        public Thickness IndentMargin => new Thickness(Depth * 20, 0, 0, 0);
        public Visibility ExpanderVisibility => HasChildren ? Visibility.Visible : Visibility.Hidden;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
