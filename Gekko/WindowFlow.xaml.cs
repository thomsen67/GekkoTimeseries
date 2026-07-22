using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using System.Windows.Media;
using Color = Microsoft.Msagl.Drawing.Color;
using ModifierKeys = System.Windows.Input.ModifierKeys;
using System.Threading;

namespace Gekko
{
    /// <summary>
    /// Interaction logic for WindowFlow.xaml
    /// </summary>
    public partial class WindowFlow : Window
    {
        public DecompFind decompFind = null;

        public bool isInitializing = false;

        int _depthNumValue = 0;

        public int DepthNumValue
        {
            get { return _depthNumValue; }
            set
            {
                _depthNumValue = value;
                depthNum.Text = value.ToString();
            }
        }

        int _ignoredNumValue = 0;
        public int IgnoredNumValue
        {
            get { return _ignoredNumValue; }
            set
            {
                _ignoredNumValue = value;
                ignoredNum.Text = value.ToString();
            }
        }

        public static readonly RoutedUICommand LoadSampleGraphCommand = new RoutedUICommand("Open File...", "OpenFileCommand",
                                                                                          typeof(WindowFlow));

        public static readonly RoutedUICommand HomeViewCommand = new RoutedUICommand("Home view...", "HomeViewCommand",
                                                                                        typeof(WindowFlow));

        private GraphViewer graphViewer = new GraphViewer();

        public WindowFlow(DecompFind decompFind)
        {
            this.isInitializing = true;
            if (true)
            {
                InitializeComponent();                
                if (G.IsNumericalError(decompFind.decompOptions2.ignore)) IgnoredNumValue = 0;
                else IgnoredNumValue = (int)decompFind.decompOptions2.ignore;
                DepthNumValue = decompFind.decompOptions2.flowgraphDepth;
            }
            this.isInitializing = false;
            this.decompFind = decompFind;
            this.Closing += Window_Closing;
            SetupToolbar();
            graphViewerPanel.ClipToBounds = true;
            graphViewer.ObjectUnderMouseCursorChanged += graphViewer_ObjectUnderMouseCursorChanged;            
            graphViewer.BindToPanel(graphViewerPanel);            
            graphViewer.MouseDown += WpfApplicationSample_MouseDown;            
            Loaded += CreateAndLayoutAndDisplayGraph; // Event handler on Loaded event
            Title = "Gekko flowgraph";
            Content = mainGrid;            
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowState = WindowState.Normal;            
        }

        private void CreateAndLayoutAndDisplayGraph(object sender, RoutedEventArgs ee)
        {
            try
            {
                this.decompFind.decompOptions2.guiFlowLagsOrLeadsWereEncountered = false;  //resetting
                Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph();
                graphViewer.Graph = graph;
                WalkInfo walkInfo = new WalkInfo();
                walkInfo.t1 = this.decompFind.decompOptions2.t1;
                walkInfo.t2 = this.decompFind.decompOptions2.t1;  //Note: using t1 here too!
                if (Program.options.bugfix_flow_use_full_period)
                {
                    walkInfo.t2 = this.decompFind.decompOptions2.t2;
                }
                //walkInfo.t1 = new GekkoTime(EFreq.A, 2032, 1);
                //walkInfo.t2 = new GekkoTime(EFreq.A, 2032, 1);
                walkInfo.visitedDepths = new Dictionary<DName, FlowInfo>(Multidim2Comparer.IgnoreCase);
                walkInfo.nodeNames = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);
                walkInfo.maxDepth = this.decompFind.decompOptions2.flowgraphDepth;
                walkInfo.ignoreDJZ = true;
                walkInfo.isGekkoModel = this.decompFind.model.modelCommon.GetModelSourceType() == EModelType.Gekko;
                walkInfo.operatorLower = this.decompFind.decompOptions2.decompOperator.OperatorLower();
                walkInfo.ignore = this.decompFind.decompOptions2.ignore;
                walkInfo.minLhsScore = Globals.lhsScore2; //Only those with name checkbox in FIND window
                walkInfo.removeSelfReferences = true;  //lags??
                walkInfo.removeResidualIgnoredError = true;
                walkInfo.ignoreLags = true;
                DName varName = this.decompFind.decompOptions2.guiFlowName;
                int depth = 0;

                if (false)
                {
                    //TODO: If decomp is called with a certain equation, and the user then clicks [Flow].
                    //The following was made between 31/1 2026 and 22/7 2026, with the old string name representation (no DName).
                    //#flowgraphproblem
                    
                    //string eqName = null;
                    //if (this.decompFind.decompOptions2.guiIsFlowUseEquationName)
                    //{
                    //    try
                    //    {
                    //        eqName = G.Chop_DimensionRemoveLast_FASTER(this.decompFind.decompOptions2.new_from[0]);
                    //    }
                    //    catch { }
                    //}

                    //if (eqName == null)
                    //{
                    //    try
                    //    {
                    //        List<EqInfoSimple> temp = GamsModel.GetSortedEquations(varName, GekkoTime.tNull, Program.model, false, false, false);
                    //        if (temp.Count > 0) //if .Count == 0, the window will be empty but not crash...
                    //        {
                    //            eqName = G.Chop_DimensionRemoveLast_FASTER(temp[0].eqName);
                    //        }
                    //    }
                    //    catch { }
                    //}

                    //if (!G.NullOrBlanks(eqName))
                    //{
                    //    WalkNodes(depth, graph, varName, eqName, walkInfo); //if problems, the window will be empty but not crash...
                    //}
                }

                List<EqInfoSimple> temp = GamsModel.GetSortedEquations(varName, GekkoTime.tNull, Program.model, false, false, false);
                //
                //
                // TODO: Lag: What if it is a leaded equ input. Bad lag hack, #osaf89dsafa
                //
                //
                DNameTime eqName = temp[0].eqName.RemoveTime().AddTime(new GekkoTime(EFreq.Lag, 0));
                WalkNodes(depth, graph, varName, eqName, walkInfo);
                if (walkInfo.lagsOrLeadsWereEncountered) this.decompFind.decompOptions2.guiFlowLagsOrLeadsWereEncountered = true;
                if (this.decompFind.decompOptions2.guiFlowRotate) graph.Attr.LayerDirection = LayerDirection.RL;
                else graph.Attr.LayerDirection = LayerDirection.TB;
                graphViewer.Graph = graph;
                SetStatusBar();
            }
            catch (Exception ex)
            {
                string s = null;
                if (Globals.runningOnTTComputer) s = ". TTH --> " + ex.ToString();
                MessageBox.Show("Loading of Gekko flowgraph failed" + s, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void WalkNodes(int depth, Microsoft.Msagl.Drawing.Graph graph, DName varName, DName eqName, WalkInfo walkInfo)
        {
            // This works regarding depth, but is wasteful, because redoing a branch entails new decomp calls.
            // Better to keep the results of the decomps (FlowInfo basically), so there is no double work.
            //
            //
            bool hasBeenSeenAlready = false;
            
            if (depth >= walkInfo.maxDepth) return;

            FlowInfo arrowsFromTo = null;

            if (!walkInfo.visitedDepths.ContainsKey(varName))
            {                
                hasBeenSeenAlready = false;
                try
                {
                    arrowsFromTo = Decomp.GetFlowInfoFromDecomp(walkInfo.t1, walkInfo.t2, varName, eqName, walkInfo);
                    walkInfo.visitedDepths.Add(varName, arrowsFromTo);
                }
                catch
                {
                    //No need to die on decomp error here
                    return;
                }
            }
            else if (depth < walkInfo.visitedDepths[varName].depth)
            {
                //It may have been seen before, but at a higher depth. If so, we try again.
                arrowsFromTo = walkInfo.visitedDepths[varName];
                hasBeenSeenAlready = true;
            }
            else
            {
                return;
            }

            arrowsFromTo.depth = depth;  //In all cases here, if seen before this depth is smaller

            GekkoDictionary<string, bool> same = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            for (int i = 1; i < arrowsFromTo.children.Count; i++)  //skips first
            {
                FlowItem flowChild = arrowsFromTo.children[i];
                if (walkInfo.removeSelfReferences && G.Equal(flowChild.from, flowChild.to)) continue;
                if (depth == 0) flowChild.to = arrowsFromTo.children[0].from;  //To get the first node capitalization right. The .to here will have wrong capitalization, but the .from has the correct one taken from eqs.
                //Here, we do not want for instance "qbnp" to be a different node than "qBNP"
                if (walkInfo.nodeNames.ContainsKey(flowChild.from)) flowChild.from = walkInfo.nodeNames[flowChild.from];
                else walkInfo.nodeNames.Add(flowChild.from, flowChild.from);
                if (walkInfo.nodeNames.ContainsKey(flowChild.to)) flowChild.to = walkInfo.nodeNames[flowChild.to];
                else walkInfo.nodeNames.Add(flowChild.to, flowChild.to);                                

                double share = flowChild.v / arrowsFromTo.children[0].v;
                if (walkInfo.removeResidualIgnoredError)
                {
                    if (G.StartsWith(flowChild.from.ToString(), "Error")) continue;
                    if (G.StartsWith(flowChild.from.ToString(), "Residual")) continue;
                    if (G.StartsWith(flowChild.from.ToString(), "Ignored")) continue;
                    if (G.StartsWith(flowChild.from.ToString(), Globals.decompResidualPrefix)) continue;
                }
                if (walkInfo.isGekkoModel && walkInfo.ignoreDJZ && (G.IsNumericalError(share) || Math.Abs(share) <= 0.01d))
                {
                    if (G.Equal(flowChild.from.ToString(), "d" + flowChild.to.ToString())) continue;
                    if (G.Equal(flowChild.from.ToString(), "j" + flowChild.to.ToString())) continue;
                    if (G.Equal(flowChild.from.ToString(), "jr" + flowChild.to.ToString())) continue;
                    if (G.Equal(flowChild.from.ToString(), "jd" + flowChild.to.ToString())) continue;
                    if (G.Equal(flowChild.from.ToString(), "z" + flowChild.to.ToString())) continue;
                }

                if (!hasBeenSeenAlready && graph != null)
                {
                    
                    Edge e = graph.AddEdge(flowChild.from.ToString(), flowChild.to.ToString());
                    e.Attr.Color = Color(share);
                    Node nodeFrom = graph.FindNode(flowChild.from.ToString());
                    nodeFrom.Attr.LabelMargin = 4;
                    nodeFrom.Attr.Color = Color(0.3);

                    Node nodeTo = graph.FindNode(flowChild.to.ToString());
                    nodeTo.Attr.LabelMargin = 4;
                    nodeTo.Attr.Color = Color(0.3);

                    if (depth == 0)
                    {
                        nodeTo.Attr.Color = Color(1.0);
                        nodeTo.Attr.FillColor = new Color(250, 242, 174);  //Same yellow as mode mixed
                    }
                }                
                
                DName varNameChild = flowChild.from;
                List<EqInfoSimple> temp = GamsModel.GetSortedEquations(varNameChild, GekkoTime.tNull, Program.model, true, false, false);
                if (temp.Count > 0 && temp[0].score >= walkInfo.minLhsScore)  //For instance only eqs that are found with checkbox "Name" in FIND window. We also do not show res_... nodes
                {
                    //Bad lag hack, #osaf89dsafa
                    DNameTime eqNameChild = temp[0].eqName.RemoveTime().AddTime(new GekkoTime(EFreq.Lag, 0));
                    WalkNodes(depth + 1, graph, varNameChild, eqNameChild, walkInfo);
                }
                else
                {
                    //TODO TODO TODO
                    //TODO TODO TODO
                    //TODO TODO TODO Find out if the eq is not found (bad eq name) or it is a .fx variable. Color differently.
                    //TODO TODO TODO
                    //TODO TODO TODO
                    if (!hasBeenSeenAlready && graph != null)
                    {
                        Node n = graph.FindNode(flowChild.from.ToString());
                        n.Attr.FillColor = new Color(238, 238, 238);
                    }
                }
            }
        }

        private static Color Color(double d3)
        {
            double d2 = d3;
            if (G.IsNumericalError(d3)) d2 = 0d;
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
            string s = null;
            if (this.decompFind.decompOptions2.guiFlowLagsOrLeadsWereEncountered) s = "Lags/leads encountered and ignored";
            statusTextBox = new TextBox { Text = Globals.flowGraphTextInfo + s };  //{ Text = "No object" };            
            statusTextBox.BorderThickness = new Thickness(0);
            statusTextBox.Opacity = 0.65;
            statusBar.Items.Add(statusTextBox);
            mainGrid.Children.Add(statusBar);
            statusBar.VerticalAlignment = VerticalAlignment.Bottom;            
            statusTextBox.Background = null;
        }
        
        void graphViewer_ObjectUnderMouseCursorChanged(object sender, ObjectUnderMouseCursorChangedEventArgs e)
        {
            var o = graphViewer.ObjectUnderMouseCursor;
            var node = o as IViewerNode;
            if (node != null)
            {
                statusTextBox.Background = new SolidColorBrush(G.Lighter(Globals.GekkoModeYellow, 0.70));
                statusTextBox.Opacity = 1.0;
                var drawingNode = (Node)node.DrawingObject;

                string label;

                if (true)
                {
                    label = Program.GetVariableExplanation1Line(Program.DName_HACK1(drawingNode.Label.Text), false);  //#cherrypick: ", false" added
                }
                else
                {
                    //If the node names have lags

                    //A bit of a hack, since we only store node names as flat plaintext                    
                    DName dname2 = Program.DName_HACK1NOLAG(drawingNode.Label.Text);                  
                    label = Program.GetVariableExplanation1Line(dname2, false); //#cherrypick: ", false" added
                }
                statusTextBox.Text = label;
            }
            else
            {
                var edge = graphViewer.ObjectUnderMouseCursor as IViewerEdge;
                if (edge != null)
                {                    
                    statusTextBox.Background = new SolidColorBrush(G.Lighter(Globals.GekkoModeYellow, 0.70));                    
                    statusTextBox.Opacity = 1.0;
                    statusTextBox.Text = ((Edge)edge.DrawingObject).SourceNode.Label.Text + " --> " + ((Edge)edge.DrawingObject).TargetNode.Label.Text;
                }
                else
                {                                    
                    statusTextBox.Text = Globals.flowGraphTextInfo;  // "No object";                    
                    statusTextBox.Background = null;
                    statusTextBox.Opacity = 0.65;
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

        private void CloseCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Globals.windowsFlow != null && this != null) Globals.windowsFlow.Remove(this);
        }

        public static void CreateWindowFlow(object o2)
        {
            DecompFind decompFindHere = o2 as DecompFind;
            DecompFind decompFindHereChild = decompFindHere.CreateChild(decompFindHere.decompOptions2.Clone(false), EDecompFindNavigation.Decomp, null, decompFindHere.model);
            WindowFlow w = new WindowFlow(decompFindHereChild);
            Globals.windowsFlow.Add(w);
            w.Title = decompFindHere.decompOptions2.guiFlowName + " - Gekko flowgraph";
            w.ShowDialog();            
        }

        void WpfApplicationSample_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            try
            {
                GraphViewer gv = sender as GraphViewer;
                if (gv == null) return;
                if (gv.ObjectUnderMouseCursor == null) return;
                string s = gv.ObjectUnderMouseCursor.DrawingObject.ToString();
                if (!s.Contains(" -> "))
                {
                    int i = s.IndexOf('"', 1);
                    string name = G.Substring(s, 1, i - 1);
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        DecompFind decompFindHere = this.decompFind;
                        DecompFind decompFindHereChild = decompFindHere.CreateChild(decompFindHere.decompOptions2.Clone(false), EDecompFindNavigation.Decomp, null, decompFindHere.model);
                        decompFindHereChild.children.Clear(); //This and the next line so we are sure to get a blank state DECOMP window: not much sense in linking via flowgraphs...
                        decompFindHereChild.parent = null;
                        List<EqInfoSimple> temp = GamsModel.GetSortedEquations(Program.DName_HACK1(name), GekkoTime.tNull, this.decompFind.model, false, false, false);
                        string eqName = temp[0].eqName.RemoveTime().ToString();
                        decompFindHereChild.decompOptions2.new_select = new List<DName> { Program.DName_HACK1(name) };
                        DName dnameLag = Program.DName_HACK1(eqName);
                        dnameLag = dnameLag.AddLag(0);
                        decompFindHereChild.decompOptions2.new_from = new List<DName>() { dnameLag };
                        decompFindHereChild.decompOptions2.new_endo = new List<DName>() { Program.DName_HACK1(name) };
                        Decomp.DecompGetFuncExpressionsAndRecalc(decompFindHereChild, null);
                    }
                    else
                    {
                        if (!isInitializing)
                        {
                            DecompFind decompFindHere = this.decompFind;
                            DecompFind decompFindHereChild = decompFindHere.CreateChild(decompFindHere.decompOptions2.Clone(false), EDecompFindNavigation.Decomp, null, decompFindHere.model);
                            decompFindHereChild.decompOptions2.guiFlowName = Program.DName_HACK1(name);
                            decompFindHereChild.decompOptions2.guiIsFlowUseEquationName = false; //hack
                            CallFlowGraph(decompFindHereChild);                            
                        }
                    }
                    e.Handled = true;
                }
            }
            catch { }
        }

        public static void CallFlowGraph(DecompFind decompFind)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(WindowFlow.CreateWindowFlow));
            thread.Name = "Flow";
            thread.SetApartmentState(ApartmentState.STA);
            thread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            thread.IsBackground = true;
            thread.Start(decompFind);
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
            this.decompFind.decompOptions2.guiFlowRotate = true;
            CreateAndLayoutAndDisplayGraph(sender, e);
        }

        private void CheckBoxRotate_Unchecked(object sender, RoutedEventArgs e)
        {
            this.decompFind.decompOptions2.guiFlowRotate = false;
            CreateAndLayoutAndDisplayGraph(sender, e);            
        }

        private void depthNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (depthNum == null) return;
            int i; bool b; Program.GuiHandleNum(depthNum.Text.Trim(), out i, out b);
            if (b && i >= 0 && i <= 100)
            {
                if (!this.isInitializing)
                {                    
                    _depthNumValue = i;
                    //DecompOptions2 remember = this.decompFind.decompOptions2;
                    try
                    {
                        //this.decompFind.decompOptions2 = this.decompFind.decompOptions2.Clone();
                        this.decompFind.decompOptions2.flowgraphDepth = i;
                        CreateAndLayoutAndDisplayGraph(sender, e);
                    }
                    finally
                    {
                        //this.decompFind.decompOptions2 = remember;
                    }
                }
            }
            else
            {
                depthNum.Text = _depthNumValue.ToString();
            }            
        }

        private void ignoredNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ignoredNum == null) return;
            int i; bool b; Program.GuiHandleNum(ignoredNum.Text.Trim(), out i, out b);
            if (b && i >= 0 && i <= 100)
            {
                if (!this.isInitializing)
                {                                
                    _ignoredNumValue = i;
                    //DecompOptions2 remember = this.decompFind.decompOptions2;
                    try
                    {
                        //this.decompFind.decompOptions2 = this.decompFind.decompOptions2.Clone();
                        this.decompFind.decompOptions2.ignore = i;
                        CreateAndLayoutAndDisplayGraph(sender, e);
                    }
                    finally
                    {
                        //this.decompFind.decompOptions2 = remember;
                    }
                }
            }
            else
            {
                ignoredNum.Text = _ignoredNumValue.ToString();
            }
        }


        private void depthUp_Click(object sender, RoutedEventArgs e)
        {
            if (DepthNumValue < 100) DepthNumValue++;
        }

        private void depthDown_Click(object sender, RoutedEventArgs e)
        {
            if (DepthNumValue > 0) DepthNumValue--;
        }

        private void ignoredUp_Click(object sender, RoutedEventArgs e)
        {
            if (IgnoredNumValue < 100) IgnoredNumValue++;
        }

        private void ignoredDown_Click(object sender, RoutedEventArgs e)
        {
            if (IgnoredNumValue > 0) IgnoredNumValue--;
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            Zoom(1d/1.2d);
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            Zoom(1.2d);
        }

        private void Zoom(double z)
        {
            var canvas = graphViewer.GraphCanvas;
            if (canvas.RenderTransform is MatrixTransform matrixTransform)
            {
                var matrix = matrixTransform.Matrix;
                double centerX = canvas.ActualWidth / 2;
                double centerY = canvas.ActualHeight / 2;
                matrix.ScaleAt(z, z, centerX, centerY);
                canvas.RenderTransform = new MatrixTransform(matrix);
            }
        }
    }

    public class WalkInfo
    {
        public GekkoTime t1;
        public GekkoTime t2;        
        public Dictionary<DName, FlowInfo> visitedDepths;
        public Dictionary<DName, DName> nodeNames;
        public int maxDepth;
        public bool ignoreDJZ;
        public bool isGekkoModel;
        //public DecompFind decompFind;
        public string operatorLower;
        public double ignore;
        public double minLhsScore = double.MinValue;
        public bool removeSelfReferences;
        public bool removeResidualIgnoredError;
        public bool ignoreLags;
        //return values:
        public bool lagsOrLeadsWereEncountered;
    }
}
