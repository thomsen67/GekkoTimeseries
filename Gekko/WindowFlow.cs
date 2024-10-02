using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Win32;
using Color = Microsoft.Msagl.Drawing.Color;
//using LabelPlacement = Microsoft.Msagl.Core.Layout.LabelPlacement;

using ModifierKeys = System.Windows.Input.ModifierKeys;
using Size = System.Windows.Size;

namespace Gekko
{
    class WpfApplicationSample : Application
    {
        public bool rotate = false;

        public static readonly RoutedUICommand LoadSampleGraphCommand = new RoutedUICommand("Open File...", "OpenFileCommand",
                                                                                     typeof(WpfApplicationSample));
        public static readonly RoutedUICommand HomeViewCommand = new RoutedUICommand("Home view...", "HomeViewCommand",
                                                                                     typeof(WpfApplicationSample));



        Window appWindow;
        Grid mainGrid = new Grid();
        DockPanel graphViewerPanel = new DockPanel();
        ToolBar toolBar = new ToolBar();
        GraphViewer graphViewer = new GraphViewer();
        TextBox statusTextBox;

        void CreateAndLayoutAndDisplayGraph(object sender, ExecutedRoutedEventArgs ex)
        {
            try
            {
                Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph();
                if (true)
                {
                    Program.options.folder_working = @"c:\Thomas\Desktop\gekko\testing\Decomp\Decomp2";
                    Program.RunGekkoCommands("reset; time 2028 2035; model<gms>makro.zip; read makro1;", "", 0, new P());

                    string equationName = "E_qBNP";
                    string variableName = "qBNP";
                    GekkoTime t1 = new GekkoTime(EFreq.A, 2028, 1, 1);
                    GekkoTime t2 = new GekkoTime(EFreq.A, 2035, 1, 1);

                    FlowInfo flowInfo = new FlowInfo(); int two = 2;

                    flowInfo.variableName = variableName;
                    flowInfo.equationName = equationName;
                    flowInfo.period = t1.Add(two);  //2030

                    ModelGamsScalar modelGamsScalar = Program.model.modelGamsScalar;
                    Model model = Program.model;                    
                    modelGamsScalar.MaybeLoadDataIntoModel(0, t1, t2, false);                    
                    DecompOptions2 decompOptions2 = new DecompOptions2();
                    decompOptions2.t1 = t1;
                    decompOptions2.t2 = t2;
                    decompOptions2.decompOperator = new DecompOperator("d");
                    decompOptions2.new_select = new List<string>() { variableName };
                    decompOptions2.new_from = new List<string>() { equationName };
                    decompOptions2.new_endo = new List<string>() { variableName };
                    decompOptions2.rows = new List<string>() { "vars", "lags" };
                    decompOptions2.cols = new List<string>() { "time" };
                    GekkoSmpl smpl = new GekkoSmpl(t1, t2);
                    DecompDatas decompDatas = new DecompDatas();
                    GekkoTime gt1, gt2;
                    Gekko.Decomp.DecompMainInit(out gt1, out gt2, t1, t2, decompOptions2.decompOperator);
                    Gekko.Decomp.EContribType operatorOneOf3Types = decompOptions2.decompOperator.type;
                    string lhsString = "Expression value";
                    Gekko.Decomp.PrepareEquations(t1, t2, decompOptions2.decompOperator, decompOptions2, false, modelGamsScalar);
                    if (decompDatas.storage == null) decompDatas.storage = new List<List<DecompData>>();
                    decompDatas.MAIN_data = null;
                    if (decompDatas.storage == null || decompDatas.storage.Count == 0) Gekko.Decomp.InitDecompDatas(decompOptions2, decompDatas, model);
                    decompOptions2.decompOperator = new DecompOperator("d");
                    decompOptions2.showErrors = true;
                    string residualName = Program.GetDecompResidualName(0, 1);
                    int funcCounter = 0;
                    DecompData dd = Gekko.Decomp.DecompLowLevelScalar(gt1, gt2, 0, decompOptions2.link[0].GAMS_dsh[0], decompOptions2.decompOperator, residualName, ref funcCounter, decompOptions2.missingAsZero, model);
                    Decomp.DecompMainMergeOrAdd(decompDatas, dd, 0, 0);  //probably superfluous when looking a abs differences?
                    decompDatas.MAIN_data = dd; decompDatas.storage[0][0] = dd;
                    DecompOutput decompOutput = Decomp.DecompPivotToTable(t1, t2, dd, decompDatas, decompOptions2.decompOperator, smpl, lhsString, decompOptions2.link[0].expressionText, decompOptions2, operatorOneOf3Types, model);
                    Table decompTable = decompOutput.table;

                    for (int i2 = 2; i2 <= decompTable.GetRowMaxNumber(); i2++)
                    {
                        Cell cellVariableName = decompTable.Get(i2, 1);
                        List<string> vars = new List<string>();
                        Cell cellFirstData = decompTable.Get(i2, 2);
                        string uniqueName = null;
                        if (cellFirstData != null)
                        {
                            vars = cellFirstData.vars_hack;
                            uniqueName = Decomp.HiddenVariableHelper(cellFirstData, true);
                        }
                        string sVarsInside = Stringlist.GetListWithCommas(vars).Replace("¤", "");
                        string label = null;
                        if (uniqueName != null) label = Program.SpecialXmlChars(Program.GetVariableExplanation1Line(uniqueName));
                        string name = cellVariableName.CellText.TextData[0];
                        name = name.Replace(" | [0]", "");
                        name = name.Replace(" | ", "");
                        name = name.Trim();

                        FlowItem flowItem = new FlowItem();
                        flowItem.box1 = flowInfo.variableName;
                        flowItem.box2 = name;

                        for (int j2 = 2; j2 <= decompTable.GetColMaxNumber(); j2++)
                        {
                            Cell cellData = decompTable.Get(i2, j2);
                            double value = cellData.number;
                            if (j2 == 2 + two) flowItem.thickness = value;
                        }
                        flowInfo.children.Add(flowItem);
                    }

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
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Loading of Gekko flowgraph Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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

        protected override void OnStartup(StartupEventArgs e)
        {

            appWindow = new Window
            {
                Title = "Gekko flowgraph",
                Content = mainGrid,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowState = WindowState.Normal
            };

            SetupToolbar();
            graphViewerPanel.ClipToBounds = true;
            mainGrid.Children.Add(toolBar);
            toolBar.VerticalAlignment = VerticalAlignment.Top;
            graphViewer.ObjectUnderMouseCursorChanged += graphViewer_ObjectUnderMouseCursorChanged;

            mainGrid.Children.Add(graphViewerPanel);
            graphViewer.BindToPanel(graphViewerPanel);

            SetStatusBar();
            graphViewer.MouseDown += WpfApplicationSample_MouseDown;
            appWindow.Loaded += (a, b) => CreateAndLayoutAndDisplayGraph(null, null);
                        
            appWindow.Show();
        }

        void WpfApplicationSample_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            statusTextBox.Text = "there was a click...";
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

        void SetupCommands()
        {
            appWindow.CommandBindings.Add(new CommandBinding(LoadSampleGraphCommand, CreateAndLayoutAndDisplayGraph));
            appWindow.CommandBindings.Add(new CommandBinding(HomeViewCommand, (a, b) => graphViewer.SetInitialTransform()));
            appWindow.InputBindings.Add(new InputBinding(LoadSampleGraphCommand, new KeyGesture(Key.L, ModifierKeys.Control)));
            appWindow.InputBindings.Add(new InputBinding(HomeViewCommand, new KeyGesture(Key.H, ModifierKeys.Control)));
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

        //[STAThread]
        //static void Main(string[] args)
        //{
        //    new WpfApplicationSample { Args = args }.Run();
        //}

        //public string[] Args { get; set; }
    }
}