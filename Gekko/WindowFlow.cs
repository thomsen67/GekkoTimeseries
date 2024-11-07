using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.WpfGraphControl;
using Color = Microsoft.Msagl.Drawing.Color;
using ModifierKeys = System.Windows.Input.ModifierKeys;

namespace Gekko
{
    public class WindowFlow : Window
    {
        public bool rotate = false;
        public static readonly RoutedUICommand LoadSampleGraphCommand = new RoutedUICommand("Open File...", "OpenFileCommand",
                                                                                       typeof(WindowFlow));
        public static readonly RoutedUICommand HomeViewCommand = new RoutedUICommand("Home view...", "HomeViewCommand",
                                                                                        typeof(WindowFlow));



        private Grid mainGrid = new Grid();
        private DockPanel graphViewerPanel = new DockPanel();
        private ToolBar toolBar = new ToolBar();
        private GraphViewer graphViewer = new GraphViewer();
        private TextBox statusTextBox = new TextBox();

        public WindowFlow()
        {
            //InitializeComponent(); // Removed - no XAML used here
            this.PreviewKeyDown += new KeyEventHandler(CloseOnEscape);
            this.Closing += Window_Closing;
            SetupToolbar();
            graphViewerPanel.ClipToBounds = true;
            mainGrid.Children.Add(toolBar);
            toolBar.VerticalAlignment = VerticalAlignment.Top;
            graphViewer.ObjectUnderMouseCursorChanged += graphViewer_ObjectUnderMouseCursorChanged;

            mainGrid.Children.Add(graphViewerPanel);
            graphViewer.BindToPanel(graphViewerPanel);

            SetStatusBar();
            graphViewer.MouseDown += WpfApplicationSample_MouseDown;

            Loaded += CreateAndLayoutAndDisplayGraph; // Event handler on Loaded event
            Title = "Gekko flowgraph";
            Content = mainGrid;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowState = WindowState.Normal;
        }

        private void CloseOnEscape(object sender, KeyEventArgs e)
        {
            //only work with showdialog ........ HMMMMMMMMMMMMMMMM! ---> Well, it seems to work with .Show(), so what is the problem?
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
            statusTextBox.Text = "there was a click...";
        }

        private void CreateAndLayoutAndDisplayGraph(object sender, RoutedEventArgs ee)
        {
            try
            {
                Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph();
                graphViewer.Graph = graph;
                //graph.LayoutAlgorithmSettings = new Microsoft.Msagl.Layout.MDS.MdsLayoutSettings();
                //double factor = 0.02;
                //
                GekkoTime t1 = new GekkoTime(EFreq.A, 2028, 1, 1);
                GekkoTime t2 = new GekkoTime(EFreq.A, 2035, 1, 1);
                GekkoDictionary<string, bool> alreadySeen = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

                //string varName = "vtKilde";
                //string varName = "vtTop[tot]";
                string varName = "fy";
                int depth = 0;

                List<EqInfoSimple> temp = Decomp.GetSortedEquations(varName, new GekkoTime(EFreq.A, 2028, 1, 1), Program.model);
                string eqName = G.Chop_DimensionRemoveLast_FASTER(temp[0].eqName);

                //a varName points to --> an eqName
                //The eqName creates arrowsFromTo, (varName -> varName1), (varName -> varName2), ...

                WalkNodes(depth, Program.options.decomp_flowgraph_depth, graph, t1, t2, alreadySeen, varName, eqName);

                Node n = null;
                foreach (string s in alreadySeen.Keys)
                {
                    n = graph.FindNode(s);
                    n.Attr.LabelMargin = 4;
                    n.Attr.Color = Color(0.3);
                    if (G.Equal(s, varName)) n.Attr.FillColor = Color(0.3);
                }

                if (rotate) graph.Attr.LayerDirection = LayerDirection.TB;
                else graph.Attr.LayerDirection = LayerDirection.RL;

                graphViewer.Graph = graph;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Loading of Gekko flowgraph Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void WalkNodes(int depth, int maxDepth, Microsoft.Msagl.Drawing.Graph graph, GekkoTime t1, GekkoTime t2, GekkoDictionary<string, bool> alreadySeen, string varName, string eqName)
        {
            if (depth >= maxDepth) return;
            FlowInfo arrowsFromTo = Decomp.GetFlowInfoFromDecomp(t1, t2, varName, eqName, "d", 2);
            for (int i = 1; i < arrowsFromTo.children.Count; i++)  //skips first
            {
                FlowItem flowChild = arrowsFromTo.children[i];
                if (G.Equal(flowChild.from, "Error")) continue;
                if (G.Equal(flowChild.from, "Residual")) continue;
                Edge e = graph.AddEdge(flowChild.from, flowChild.to);
                e.Attr.Color = Color(flowChild.v / arrowsFromTo.children[0].v);
                if (!alreadySeen.ContainsKey(flowChild.from)) alreadySeen.Add(flowChild.from, false);
                if (!alreadySeen.ContainsKey(flowChild.to)) alreadySeen.Add(flowChild.to, false);

                string varNameChild = flowChild.from;
                List<EqInfoSimple> temp = Decomp.GetSortedEquations(varNameChild, new GekkoTime(EFreq.A, 2028, 1, 1), Program.model);
                string eqNameChild = G.Chop_DimensionRemoveLast_FASTER(temp[0].eqName);

                WalkNodes(depth + 1, maxDepth, graph, t1, t2, alreadySeen, varNameChild, eqNameChild);
            }
        }

        private static Color Color(double d2)
        {
            double d = Math.Abs(d2);
            if (d > 1) d = 1;
            else if (d < 0.20) d = 0.20;
            byte b = (byte)((1 - d) * 255);
            if (d2 > 0) return new Color(255, b, b, b);
            else return new Color(255, 255, b, b);
        }

        private static double Width(double d, double factor)
        {
            return d * factor;
        }

        void SetStatusBar()
        {
            var statusBar = new StatusBar();
            statusTextBox = new TextBox { Text = "" };  //{ Text = "No object" };            
            statusBar.Items.Add(statusTextBox);
            mainGrid.Children.Add(statusBar);
            statusBar.VerticalAlignment = VerticalAlignment.Bottom;
            statusTextBox.Background = new System.Windows.Media.SolidColorBrush(Globals.GekkoModeYellow);
        }

        void graphViewer_ObjectUnderMouseCursorChanged(object sender, ObjectUnderMouseCursorChangedEventArgs e)
        {
            var node = graphViewer.ObjectUnderMouseCursor as IViewerNode;
            if (node != null)
            {
                var drawingNode = (Node)node.DrawingObject;
                statusTextBox.Text = drawingNode.Label.Text;
                if (statusTextBox.Text == "vtKilde") statusTextBox.Text = "Kildeskatter";
                else if (statusTextBox.Text == "vtAktie") statusTextBox.Text = "Aktieskatter";
                else if (statusTextBox.Text == "vtBund") statusTextBox.Text = "Bundskatter";
                else if (statusTextBox.Text == "vtKommune") statusTextBox.Text = "Kommunale indkomstskatter";
                else if (statusTextBox.Text == "vPersFradrag") statusTextBox.Text = "Imputeret personfradrag";
                else if (statusTextBox.Text == "vSkatteplInd") statusTextBox.Text = "Skattepligtig indkomst";
                else if (statusTextBox.Text == "vPersInd") statusTextBox.Text = "Personlig indkomst";
                else if (statusTextBox.Text == "vPensIndb") statusTextBox.Text = "Pensionsindbetalinger";
                else if (statusTextBox.Text == "vWHh") statusTextBox.Text = "Årsløn per beskæftiget";
                else if (statusTextBox.Text == "vtHhAM") statusTextBox.Text = "Arbejdsmarkedsbidrag betalt af husholdningerne";
                else if (statusTextBox.Text == "vRealiseretAktieOmv") statusTextBox.Text = "Skøn over realiseret gevinst ved salg af aktier";
                else if (statusTextBox.Text == "vHh[-1]") statusTextBox.Text = "Husholdningernes finansielle portefølje";
                else if (statusTextBox.Text == "vSatsIndeks") statusTextBox.Text = "Satsregulering";
                else if (statusTextBox.Text == "vBeskFradrag") statusTextBox.Text = "Imputeret beskæftigelsesfradrag";
            }
            else
            {
                var edge = graphViewer.ObjectUnderMouseCursor as IViewerEdge;
                if (edge != null)
                    statusTextBox.Text = ((Edge)edge.DrawingObject).SourceNode.Label.Text + " --> " +
                                         ((Edge)edge.DrawingObject).TargetNode.Label.Text;
                else
                    statusTextBox.Text = "";  // "No object";
            }
        }

        void SetupToolbar()
        {
            SetupCommands();
            DockPanel.SetDock(toolBar, Dock.Top);
            SetMainMenu();
        }

        void SetMainMenu()
        {
            var mainMenu = new Menu { IsMainMenu = true };
            toolBar.Items.Add(mainMenu);
            SetFileMenu(mainMenu);
            SetViewMenu(mainMenu);
        }

        void SetViewMenu(Menu mainMenu)
        {
            var viewMenu = new MenuItem { Header = "_View" };
            var viewMenuItem = new MenuItem { Header = "_Home", Command = HomeViewCommand };
            viewMenu.Items.Add(viewMenuItem);
            mainMenu.Items.Add(viewMenu);
        }

        void SetFileMenu(Menu mainMenu)
        {
            var fileMenu = new MenuItem { Header = "_File" };
            var openFileMenuItem = new MenuItem { Header = "_Load Sample Graph", Command = LoadSampleGraphCommand };
            fileMenu.Items.Add(openFileMenuItem);
            mainMenu.Items.Add(fileMenu);
        }

        void SetupCommands()
        {
            //appWindow.CommandBindings.Add(new CommandBinding(LoadSampleGraphCommand, CreateAndLayoutAndDisplayGraph));
            //appWindow.CommandBindings.Add(new CommandBinding(HomeViewCommand, (a, b) => graphViewer.SetInitialTransform()));
            //appWindow.InputBindings.Add(new InputBinding(LoadSampleGraphCommand, new KeyGesture(Key.L, ModifierKeys.Control)));
            //appWindow.InputBindings.Add(new InputBinding(HomeViewCommand, new KeyGesture(Key.H, ModifierKeys.Control)));

            CommandBindings.Add(new CommandBinding(LoadSampleGraphCommand, CreateAndLayoutAndDisplayGraph));
            CommandBindings.Add(new CommandBinding(HomeViewCommand, (a, b) => graphViewer.SetInitialTransform()));
            InputBindings.Add(new InputBinding(LoadSampleGraphCommand, new KeyGesture(Key.L, ModifierKeys.Control)));
            InputBindings.Add(new InputBinding(HomeViewCommand, new KeyGesture(Key.H, ModifierKeys.Control)));
        }
    

        //void SetMainMenu()
        //{
        //    var mainMenu = new Menu { IsMainMenu = true };
        //    toolBar.Items.Add(mainMenu);
        //    SetFileMenu(mainMenu);
        //    SetViewMenu(mainMenu);
        //}

        //void SetViewMenu(Menu mainMenu)
        //{
        //    var viewMenu = new MenuItem { Header = "_View" };
        //    var viewMenuItem = new MenuItem { Header = "_Home", Command = HomeViewCommand };
        //    viewMenu.Items.Add(viewMenuItem);
        //    mainMenu.Items.Add(viewMenu);
        //}

        //void SetFileMenu(Menu mainMenu)
        //{
        //    var fileMenu = new MenuItem { Header = "_File" };
        //    var openFileMenuItem = new MenuItem { Header = "_Load Sample Graph", Command = LoadSampleGraphCommand };
        //    fileMenu.Items.Add(openFileMenuItem);
        //    mainMenu.Items.Add(fileMenu);
        //}


        private void ShowWindow()
        {
            this.ShowDialog(); // Show the window as a modal dialog
        }

        //public static void Main(string[] args)
        //{
        //    MainWindow window = new MainWindow();
        //    window.ShowWindow(); // Call the ShowWindow method to display the window
        //}

    }
}