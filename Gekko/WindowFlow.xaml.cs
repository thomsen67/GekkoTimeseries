using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.WpfGraphControl;
using System.Windows.Media;
using Color = Microsoft.Msagl.Drawing.Color;
using ModifierKeys = System.Windows.Input.ModifierKeys;

namespace Gekko
{
    /// <summary>
    /// Interaction logic for WindowFlow.xaml
    /// </summary>
    public partial class WindowFlow : Window
    {

        public DecompFind decompFind = null;
        public bool rotate = false;

        public static readonly RoutedUICommand LoadSampleGraphCommand = new RoutedUICommand("Open File...", "OpenFileCommand",
                                                                                          typeof(WindowFlow));

        public static readonly RoutedUICommand HomeViewCommand = new RoutedUICommand("Home view...", "HomeViewCommand",
                                                                                        typeof(WindowFlow));

        private GraphViewer graphViewer = new GraphViewer();

        public WindowFlow(DecompFind decompFind)
        {
            InitializeComponent();
            this.decompFind = decompFind;
            this.PreviewKeyDown += new KeyEventHandler(CloseOnEscape);
            this.Closing += Window_Closing;
            SetupToolbar();
            graphViewerPanel.ClipToBounds = true;
            //mainGrid.Children.Add(toolBar);
            //toolBar.VerticalAlignment = VerticalAlignment.Top;
            graphViewer.ObjectUnderMouseCursorChanged += graphViewer_ObjectUnderMouseCursorChanged;            
            //graphViewer.MouseDown += WpfApplicationSample_MouseDown;

            //mainGrid.Children.Add(graphViewerPanel);
            graphViewer.BindToPanel(graphViewerPanel);

            SetStatusBar();
            graphViewer.MouseDown += WpfApplicationSample_MouseDown;

            Loaded += CreateAndLayoutAndDisplayGraph; // Event handler on Loaded event
            Title = "Gekko flowgraph";
            Content = mainGrid;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowState = WindowState.Normal;

            //SetupCommands();            
        }

        private void CreateAndLayoutAndDisplayGraph(object sender, RoutedEventArgs ee)
        {
            try
            {
                Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph();
                graphViewer.Graph = graph;
                //graph.LayoutAlgorithmSettings = new Microsoft.Msagl.Layout.MDS.MdsLayoutSettings();
                //double factor = 0.02;

                //GekkoDictionary<string, bool> alreadySeen = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

                string varName = this.decompFind.decompOptions2.new_select[0];
                int depth = 0;
                List<EqInfoSimple> temp = Decomp.GetSortedEquations(varName, new GekkoTime(EFreq.A, 2028, 1, 1), Program.model);
                string eqName = G.Chop_DimensionRemoveLast_FASTER(temp[0].eqName);

                //a varName points to --> an eqName
                //The eqName creates arrowsFromTo, (varName -> varName1), (varName -> varName2), ...
                WalkInfo wi = new WalkInfo();
                wi.t1 = this.decompFind.decompOptions2.t1;
                wi.t2 = this.decompFind.decompOptions2.t1;  //Note: using t1 here too!
                wi.alreadySeen = new GekkoDictionaryBlanks<string>();
                wi.maxDepth = Program.options.decomp_flowgraph_depth;
                wi.ignoreDJZ = true;
                wi.isGekkoModel = this.decompFind.model.modelCommon.GetModelSourceType() == EModelType.Gekko;
                wi.decompFind = this.decompFind;
                wi.removeSelfReferences = true;  //lags??
                wi.removeResidualIgnoredError = true;

                WalkNodes(depth, graph, varName, eqName, wi);

                if (rotate) graph.Attr.LayerDirection = LayerDirection.TB;
                else graph.Attr.LayerDirection = LayerDirection.RL;

                graphViewer.Graph = graph;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Loading of Gekko flowgraph Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    private static void WalkNodes(int depth, Microsoft.Msagl.Drawing.Graph graph, string varName, string eqName, WalkInfo walkInfo)
        {
            if (depth >= walkInfo.maxDepth) return;
            FlowInfo arrowsFromTo = Decomp.GetFlowInfoFromDecomp(walkInfo.t1, walkInfo.t2, varName, eqName, walkInfo.decompFind);

            GekkoDictionary<string, bool> same = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            for (int i = 1; i < arrowsFromTo.children.Count; i++)  //skips first
            {
                FlowItem flowChild = arrowsFromTo.children[i];

                if (Globals.runningOnTTComputer && G.Equal("vwhh[tot]", flowChild.from))
                {
                    MessageBox.Show("Several from vwhh[tot] to vPersIndx[tot]");
                }

                double share = flowChild.v / arrowsFromTo.children[0].v;
                if (walkInfo.removeResidualIgnoredError)
                {
                    if (G.StartsWith(flowChild.from, "Error")) continue;
                    if (G.StartsWith(flowChild.from, "Residual")) continue;
                    if (G.StartsWith(flowChild.from, "Ignored")) continue;
                }
                if (walkInfo.isGekkoModel && walkInfo.ignoreDJZ && (G.isNumericalError(share) || Math.Abs(share) <= 0.01d))
                {
                    if (G.Equal(flowChild.from, "d" + flowChild.to)) continue;
                    if (G.Equal(flowChild.from, "j" + flowChild.to)) continue;
                    if (G.Equal(flowChild.from, "jr" + flowChild.to)) continue;
                    if (G.Equal(flowChild.from, "jd" + flowChild.to)) continue;
                    if (G.Equal(flowChild.from, "z" + flowChild.to)) continue;
                }

                if (walkInfo.removeSelfReferences)
                {
                    if (G.Equal(flowChild.from, flowChild.to)) continue;
                    string s = (flowChild.from + "-->" + flowChild.to).Replace(" ", "");
                    if (same.ContainsKey(s))
                    {
                        continue;
                    }
                    else
                    {
                        same.Add(s, false);
                    }
                }

                Edge e = graph.AddEdge(flowChild.from, flowChild.to);

                Node nodeFrom = graph.FindNode(flowChild.from);
                nodeFrom.Attr.LabelMargin = 4;
                nodeFrom.Attr.Color = Color(0.3);

                Node nodeTo = graph.FindNode(flowChild.to);
                nodeTo.Attr.LabelMargin = 4;
                nodeTo.Attr.Color = Color(0.3);
                if (depth == 0) nodeTo.Attr.Color = Color(1.0);

                e.Attr.Color = Color(share);
                if (!walkInfo.alreadySeen.ContainsKey(flowChild.from)) walkInfo.alreadySeen.Add(flowChild.from, null);
                if (!walkInfo.alreadySeen.ContainsKey(flowChild.to)) walkInfo.alreadySeen.Add(flowChild.to, null);

                string varNameChild = flowChild.from;
                List<EqInfoSimple> temp = Decomp.GetSortedEquations(varNameChild, new GekkoTime(EFreq.A, 2028, 1, 1), Program.model);
                if (temp.Count > 0 && temp[0].score >= 100d)  //Only eqs that are found with checkbox "Name" in FIND window.
                {
                    string eqNameChild = G.Chop_DimensionRemoveLast_FASTER(temp[0].eqName);
                    WalkNodes(depth + 1, graph, varNameChild, eqNameChild, walkInfo);
                }
                else
                {
                    Node n = graph.FindNode(flowChild.from);
                    n.Attr.FillColor = new Color(230, 230, 230);
                }
            }
        }

        private static Color Color(double d2)
        {
            double d = Math.Abs(d2);
            if (d > 1) d = 1;
            else if (d < 0.20) d = 0.20;
            byte b = (byte)((1 - d) * 255);
            if (d2 >= -0.01) return new Color(255, b, b, b);
            else return new Color(255, 255, b, b);
        }

        private static double Width(double d, double factor)
        {
            return d * factor;
        }

        private void SetStatusBar()
        {
            var statusBar = new StatusBar();
            statusTextBox = new TextBox { Text = "" };  //{ Text = "No object" };            
            statusBar.Items.Add(statusTextBox);
            mainGrid.Children.Add(statusBar);
            statusBar.VerticalAlignment = VerticalAlignment.Bottom;
            //statusTextBox.Background = new System.Windows.Media.SolidColorBrush(Globals.GekkoModeYellow);
            statusTextBox.Background = new SolidColorBrush(G.Lighter(Globals.GekkoModeYellow, 0.70));
            statusTextBox.Visibility = Visibility.Hidden;
        }

        void graphViewer_ObjectUnderMouseCursorChanged(object sender, ObjectUnderMouseCursorChangedEventArgs e)
        {
            var node = graphViewer.ObjectUnderMouseCursor as IViewerNode;
            if (node != null)
            {
                statusTextBox.Visibility = Visibility.Visible;
                var drawingNode = (Node)node.DrawingObject;
                string label = Program.GetVariableExplanation1Line(drawingNode.Label.Text);                             
                statusTextBox.Text = label;
            }
            else
            {
                var edge = graphViewer.ObjectUnderMouseCursor as IViewerEdge;
                if (edge != null)
                {
                    statusTextBox.Visibility = Visibility.Visible;
                    statusTextBox.Text = ((Edge)edge.DrawingObject).SourceNode.Label.Text + " --> " + ((Edge)edge.DrawingObject).TargetNode.Label.Text;
                }
                else
                {
                    statusTextBox.Visibility = Visibility.Hidden;
                    statusTextBox.Text = "";  // "No object";                    
                }
            }
        }


        private void SetupToolbar()
        {
            SetupCommands();
            //DockPanel.SetDock(toolBar, Dock.Top);            
            SetMainMenu();
        }

        private void SetMainMenu()
        {
            //var mainMenu = new Menu { IsMainMenu = true };
            //toolBar.Items.Add(mainMenu);
            //SetFileMenu(mainMenu);
            //SetViewMenu(mainMenu);

        }

        private void CloseOnEscape(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Globals.windowsFlow != null && this != null) Globals.windowsFlow.Remove(this);
        }

        void WpfApplicationSample_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            statusTextBox.Text = "Flowgraph clicked...";
            //statusTextBox.Text = "";
        }

        private void SetViewMenu(Menu mainMenu)
        {
            var viewMenu = new MenuItem { Header = "_View" };
            var viewMenuItem = new MenuItem { Header = "_Home", Command = HomeViewCommand };
            viewMenu.Items.Add(viewMenuItem);
            mainMenu.Items.Add(viewMenu);

        }

        private void SetFileMenu(Menu mainMenu)
        {
            var fileMenu = new MenuItem { Header = "_File" };
            var openFileMenuItem = new MenuItem { Header = "_Load Sample Graph", Command = LoadSampleGraphCommand };
            fileMenu.Items.Add(openFileMenuItem);
            mainMenu.Items.Add(fileMenu);

        }

        
        private void SetupCommands()
        {
            CommandBindings.Add(new CommandBinding(LoadSampleGraphCommand, CreateAndLayoutAndDisplayGraph));
            CommandBindings.Add(new CommandBinding(HomeViewCommand, (a, b) => graphViewer.SetInitialTransform()));
            InputBindings.Add(new InputBinding(LoadSampleGraphCommand, new KeyGesture(Key.L, ModifierKeys.Control)));
            InputBindings.Add(new InputBinding(HomeViewCommand, new KeyGesture(Key.H, ModifierKeys.Control)));
        }

        private void ShowWindow()
        {
            this.ShowDialog(); // Show the window as a modal dialog
        }

        private void CheckBoxRotate_Checked(object sender, RoutedEventArgs e)
        {
            this.rotate = true;
            CreateAndLayoutAndDisplayGraph(sender, e);
        }

        private void CheckBoxRotate_Unchecked(object sender, RoutedEventArgs e)
        {
            this.rotate = false;
            CreateAndLayoutAndDisplayGraph(sender, e);
        }

    }

    public class WalkInfo
    {
        public GekkoTime t1;
        public GekkoTime t2;
        public GekkoDictionaryBlanks<string> alreadySeen;
        public int maxDepth;
        public bool ignoreDJZ;
        public bool isGekkoModel;
        public DecompFind decompFind;
        public bool removeSelfReferences;
        public bool removeResidualIgnoredError;
    }
}
