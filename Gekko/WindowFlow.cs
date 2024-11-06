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
    class MainWindow : Window
    {
        public bool rotate = false;
        public static readonly RoutedUICommand LoadSampleGraphCommand = new RoutedUICommand("Open File...", "OpenFileCommand",
                                                                                       typeof(MainWindow));
        public static readonly RoutedUICommand HomeViewCommand = new RoutedUICommand("Home view...", "HomeViewCommand",
                                                                                        typeof(MainWindow));



        private Grid mainGrid = new Grid();
        private DockPanel graphViewerPanel = new DockPanel();
        private ToolBar toolBar = new ToolBar();
        private GraphViewer graphViewer = new GraphViewer();
        private TextBox statusTextBox = new TextBox();

        public MainWindow()
        {
            //InitializeComponent(); // Removed - no XAML used here
            this.PreviewKeyDown += new KeyEventHandler(CloseOnEscape);
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
            //only work with showdialog ........ HMMMMMMMMMMMMMMMM!
            if (e.Key == Key.Escape)
            {
                Close();
            }
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

                if (true)
                {
                    //Program.options.folder_working = @"c:\Thomas\Desktop\gekko\testing\Decomp\Decomp2";
                    //Program.RunGekkoCommands("reset; time 2028 2035; model<gms>makro.zip; read makro1;", "", 0, new P());

                    GekkoDictionary<string, string> matches = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    string supreme = "vtKilde";
                    matches.Add("vtKilde", "E_vtKilde");
                    //matches.Add("vtKommune[tot]", "E_vtkommune_tot");
                    //matches.Add("vtBund[tot]", "E_vtbund_tot");
                    //matches.Add("vtAktie[tot]", "E_vtaktie_tot");

                    Edge e = null;
                    Node n = null;

                    GekkoDictionary<string, bool> vars = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
                    foreach (KeyValuePair<string, string> match in matches)
                    {

                        GekkoTime t1 = new GekkoTime(EFreq.A, 2028, 1, 1);
                        GekkoTime t2 = new GekkoTime(EFreq.A, 2035, 1, 1);

                        FlowInfo flowInfo = Decomp.GetFlowInfoFromDecomp(t1, t2, match.Key, match.Value, "d", 2);

                        foreach (FlowItem xx in flowInfo.children)
                        {
                            if (G.Equal(xx.from, "vtKommune[tot]")) xx.from = "vtKommune";
                            if (G.Equal(xx.to, "vtKommune[tot]")) xx.to = "vtKommune";
                            if (G.Equal(xx.from, "vtBunt[tot]")) xx.from = "vtBund";
                            if (G.Equal(xx.to, "vtBund[tot]")) xx.to = "vtBund";
                            if (G.Equal(xx.from, "vtAktie[tot]")) xx.from = "vtAktie";
                            if (G.Equal(xx.to, "vtAktie[tot]")) xx.to = "vtAktie";
                        }

                        double factor = 0.02;

                        //graph.LayoutAlgorithmSettings = new Microsoft.Msagl.Layout.MDS.MdsLayoutSettings();                    

                        FlowItem flowParent = flowInfo.children[0];
                        for (int i = 1; i < flowInfo.children.Count; i++)  //skips first
                        {
                            FlowItem flowChild = flowInfo.children[i];
                            if (G.Equal(flowChild.from, "Error")) continue;
                            if (G.Equal(flowChild.from, "Residual")) continue;
                            e = graph.AddEdge(flowChild.from, flowChild.to);
                            e.Attr.Color = Color(flowChild.v / flowParent.v);
                            if (!vars.ContainsKey(flowChild.from)) vars.Add(flowChild.from, false);
                            if (!vars.ContainsKey(flowChild.to)) vars.Add(flowChild.to, false);
                        }
                    }

                    foreach (string s in vars.Keys)
                    {
                        n = graph.FindNode(s);
                        n.Attr.LabelMargin = 4;
                        n.Attr.Color = Color(0.3);
                        if (G.Equal(s, supreme)) n.Attr.FillColor = Color(0.3);
                    }

                    if (rotate) graph.Attr.LayerDirection = LayerDirection.TB;
                    else graph.Attr.LayerDirection = LayerDirection.RL;

                }
                else if (false)
                {

                    Edge e = null;
                    Node n = null;

                    double factor = 0.02;

                    //graph.LayoutAlgorithmSettings = new Microsoft.Msagl.Layout.MDS.MdsLayoutSettings();                    


                    e = graph.AddEdge("vtAktie", "vtKilde");
                    e.Attr.Color = Color(0.10);

                    //e = graph.AddEdge("vtKilde", "vtAktie");
                    //e.Attr.Color = Color(0.10);

                    e = graph.AddEdge("vtKommune", "vtKilde");
                    e.Attr.Color = Color(0.73);

                    e = graph.AddEdge("vtBund", "vtKilde");
                    e.Attr.Color = Color(0.35);

                    e = graph.AddEdge("vSkatteplInd", "vtKommune");
                    e.Attr.Color = Color(1.08);

                    e = graph.AddEdge("vPersFradrag", "vtKommune");
                    e.Attr.Color = Color(-0.09);

                    e = graph.AddEdge("vPersInd", "vtBund");
                    e.Attr.Color = Color(1.08);

                    e = graph.AddEdge("vPersFradrag", "vtBund");
                    e.Attr.Color = Color(-0.08);

                    e = graph.AddEdge("vRealiseretAktieOmv", "vtAktie");
                    e.Attr.Color = Color(0.40);

                    e = graph.AddEdge("vHh[-1]", "vtAktie");
                    e.Attr.Color = Color(0.60);

                    e = graph.AddEdge("vWHh", "vPersInd");
                    e.Attr.Color = Color(1.21);

                    e = graph.AddEdge("vPensIndb", "vPersInd");
                    e.Attr.Color = Color(-0.11);

                    e = graph.AddEdge("vtHhAM", "vPersInd");
                    e.Attr.Color = Color(-0.10);

                    e = graph.AddEdge("vSatsIndeks", "vPersFradrag");
                    e.Attr.Color = Color(1.00);

                    e = graph.AddEdge("vPersInd", "vSkatteplInd");
                    e.Attr.Color = Color(1.10);

                    e = graph.AddEdge("vBeskFradrag", "vSkatteplInd");
                    e.Attr.Color = Color(-0.07);

                    e = graph.AddEdge("vWHh", "vBeskFradrag");
                    e.Attr.Color = Color(1.00);

                    foreach (string s in new string[] { "vtKilde", "vtAktie", "vtKommune", "vtBund", "vSkatteplInd", "vPersFradrag", "vRealiseretAktieOmv", "vHh[-1]", "vPersInd", "vWHh", "vPensIndb", "vtHhAM", "vSatsIndeks", "vBeskFradrag" })
                    {
                        n = graph.FindNode(s);
                        n.Attr.LabelMargin = 4;
                        n.Attr.Color = Color(0.3);
                        if (s == "vtKilde") n.Attr.FillColor = Color(0.3);
                    }

                    if (rotate) graph.Attr.LayerDirection = LayerDirection.TB;
                    else graph.Attr.LayerDirection = LayerDirection.RL;
                }
                else if (true)
                {
                    //graph.LayoutAlgorithmSettings=new MdsLayoutSettings();                    

                    graph.AddEdge("1", "2");
                    graph.AddEdge("1", "3");
                    var e = graph.AddEdge("4", "5");
                    e.LabelText = "Some edge label";
                    e.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    e.Attr.LineWidth *= 2;

                    graph.AddEdge("4", "6");
                    e = graph.AddEdge("7", "8");
                    e.Attr.LineWidth *= 2;
                    e.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;

                    graph.AddEdge("7", "9");
                    e = graph.AddEdge("5", "7");
                    e.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    e.Attr.LineWidth *= 2;

                    graph.AddEdge("2", "7");
                    graph.AddEdge("10", "11");
                    graph.AddEdge("10", "12");
                    graph.AddEdge("2", "10");
                    graph.AddEdge("8", "10");
                    graph.AddEdge("5", "10");
                    graph.AddEdge("13", "14");
                    graph.AddEdge("13", "15");
                    graph.AddEdge("8", "13");
                    graph.AddEdge("2", "13");
                    graph.AddEdge("5", "13");
                    graph.AddEdge("16", "17");
                    graph.AddEdge("16", "18");
                    graph.AddEdge("16", "18");
                    graph.AddEdge("19", "20");
                    graph.AddEdge("19", "21");
                    graph.AddEdge("17", "19");
                    graph.AddEdge("2", "19");
                    graph.AddEdge("22", "23");

                    e = graph.AddEdge("22", "24");
                    e.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    e.Attr.LineWidth *= 2;

                    e = graph.AddEdge("8", "22");
                    e.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    e.Attr.LineWidth *= 2;

                    graph.AddEdge("20", "22");
                    graph.AddEdge("25", "26");
                    graph.AddEdge("25", "27");
                    graph.AddEdge("20", "25");
                    graph.AddEdge("28", "29");
                    graph.AddEdge("28", "30");
                    graph.AddEdge("31", "32");
                    graph.AddEdge("31", "33");
                    graph.AddEdge("5", "31");
                    graph.AddEdge("8", "31");
                    graph.AddEdge("2", "31");
                    graph.AddEdge("20", "31");
                    graph.AddEdge("17", "31");
                    graph.AddEdge("29", "31");
                    graph.AddEdge("34", "35");
                    graph.AddEdge("34", "36");
                    graph.AddEdge("20", "34");
                    graph.AddEdge("29", "34");
                    graph.AddEdge("5", "34");
                    graph.AddEdge("2", "34");
                    graph.AddEdge("8", "34");
                    graph.AddEdge("17", "34");
                    graph.AddEdge("37", "38");
                    graph.AddEdge("37", "39");
                    graph.AddEdge("29", "37");
                    graph.AddEdge("5", "37");
                    graph.AddEdge("20", "37");
                    graph.AddEdge("8", "37");
                    graph.AddEdge("2", "37");
                    graph.AddEdge("40", "41");
                    graph.AddEdge("40", "42");
                    graph.AddEdge("17", "40");
                    graph.AddEdge("2", "40");
                    graph.AddEdge("8", "40");
                    graph.AddEdge("5", "40");
                    graph.AddEdge("20", "40");
                    graph.AddEdge("29", "40");
                    graph.AddEdge("43", "44");
                    graph.AddEdge("43", "45");
                    graph.AddEdge("8", "43");
                    graph.AddEdge("2", "43");
                    graph.AddEdge("20", "43");
                    graph.AddEdge("17", "43");
                    graph.AddEdge("5", "43");
                    graph.AddEdge("29", "43");
                    graph.AddEdge("46", "47");
                    graph.AddEdge("46", "48");
                    graph.AddEdge("29", "46");
                    graph.AddEdge("5", "46");
                    graph.AddEdge("17", "46");
                    graph.AddEdge("49", "50");
                    graph.AddEdge("49", "51");
                    graph.AddEdge("5", "49");
                    graph.AddEdge("2", "49");
                    graph.AddEdge("52", "53");
                    graph.AddEdge("52", "54");
                    graph.AddEdge("17", "52");
                    graph.AddEdge("20", "52");
                    graph.AddEdge("2", "52");
                    graph.AddEdge("50", "52");
                    graph.AddEdge("55", "56");
                    graph.AddEdge("55", "57");
                    graph.AddEdge("58", "59");
                    graph.AddEdge("58", "60");
                    graph.AddEdge("20", "58");
                    graph.AddEdge("29", "58");
                    graph.AddEdge("5", "58");
                    graph.AddEdge("47", "58");

                    var subgraph = new Subgraph("subgraph 1");
                    graph.RootSubgraph.AddSubgraph(subgraph);
                    subgraph.AddNode(graph.FindNode("47"));
                    subgraph.AddNode(graph.FindNode("58"));

                    graph.AddEdge(subgraph.Id, "55");

                    var node = graph.FindNode("5");
                    node.LabelText = "Label of node 5";
                    node.Label.FontSize = 5;
                    node.Label.FontName = "New Courier";
                    node.Label.FontColor = Microsoft.Msagl.Drawing.Color.Blue;

                    node = graph.FindNode("55");

                    graph.Attr.LayerDirection = LayerDirection.LR;

                    //graph.LayoutAlgorithmSettings.EdgeRoutingSettings.RouteMultiEdgesAsBundles = true;
                    //graph.LayoutAlgorithmSettings.EdgeRoutingSettings.EdgeRoutingMode = EdgeRoutingMode.SplineBundling;
                    //layout the graph and draw it                    
                }

                else
                {
                    graph.AddEdge("47", "58");
                    graph.AddEdge("70", "71");
                    var tn = graph.AddNode("test");

                    graph.AddEdge("test", "47");

                    var subgraph = new Subgraph("subgraph1");
                    subgraph.Label.Text = "Outer";
                    graph.RootSubgraph.AddSubgraph(subgraph);
                    subgraph.AddNode(graph.FindNode("47"));
                    subgraph.AddNode(graph.FindNode("58"));

                    var subgraph2 = new Subgraph("subgraph2");
                    subgraph2.Label.Text = "Inner";
                    subgraph2.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                    subgraph2.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;
                    //subgraph2.Attr.ClusterLabelMargin = LabelPlacement.Bottom;
                    subgraph2.AddNode(graph.FindNode("70"));
                    subgraph2.AddNode(graph.FindNode("71"));
                    subgraph.AddSubgraph(subgraph2);
                    graph.AddEdge("58", subgraph2.Id);

                    graph.Attr.LayerDirection = LayerDirection.LR;
                    //graph.LayoutAlgorithmSettings.EdgeRoutingSettings.EdgeRoutingMode = EdgeRoutingMode.Rectilinear;

                    var global = (SugiyamaLayoutSettings)graph.LayoutAlgorithmSettings;
                    var local = (SugiyamaLayoutSettings)global.Clone();
                    local.Transformation = PlaneTransformation.Rotation(-Math.PI / 2);
                    subgraph2.LayoutSettings = local;   // for Collapsing\Expanding
                                                        //global.ClusterSettings.Add(subgraph2, local);

                }
                graphViewer.Graph = graph;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Loading of Gekko flowgraph Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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