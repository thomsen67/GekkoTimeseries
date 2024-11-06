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

        void WpfApplicationSample_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            statusTextBox.Text = "there was a click...";
        }

        private void CreateAndLayoutAndDisplayGraph(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph();
                graphViewer.Graph = graph;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Loading of Gekko flowgraph Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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