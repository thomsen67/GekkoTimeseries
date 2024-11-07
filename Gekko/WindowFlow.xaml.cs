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

        public WindowFlow()
        {
            InitializeComponent();

            this.PreviewKeyDown += new KeyEventHandler(CloseOnEscape);
            this.Closing += Window_Closing;
            graphViewer.ObjectUnderMouseCursorChanged += graphViewer_ObjectUnderMouseCursorChanged;
            graphViewer.MouseDown += WpfApplicationSample_MouseDown;

            SetupCommands();
        }

        private void CreateAndLayoutAndDisplayGraph(object sender, RoutedEventArgs ee)
        {
            // Omitted Logic
        }

        private void SetStatusBar()
        {
            // No longer needed, elements defined in XAML
        }

        private void SetupToolbar()
        {
            SetMainMenu();
        }

        private void SetMainMenu()
        {
            var mainMenu = new Menu { IsMainMenu = true };
            toolBar.Items.Add(mainMenu);
            SetFileMenu(mainMenu);
            SetViewMenu(mainMenu);

        }

        private void CloseOnEscape(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
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

        void graphViewer_ObjectUnderMouseCursorChanged(object sender, ObjectUnderMouseCursorChangedEventArgs e)
        {
            var node = graphViewer.ObjectUnderMouseCursor as IViewerNode;
            if (node != null)
            {
                var drawingNode = (Node)node.DrawingObject;
                string label = Program.GetVariableExplanation1Line(drawingNode.Label.Text);
                //string s = drawingNode.Label.Text;                
                //if (!G.NullOrBlanks(label)) s += ": " + label;                
                statusTextBox.Text = label;
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

        private void SetupCommands()
        {
            CommandBindings.Add(new CommandBinding(LoadSampleGraphCommand, CreateAndLayoutAndDisplayGraph));
            CommandBindings.Add(new CommandBinding(HomeViewCommand, (a, b) => graphViewer.SetInitialTransform()));
            InputBindings.Add(new InputBinding(LoadSampleGraphCommand, new KeyGesture(Key.L, ModifierKeys.Control)));
            InputBindings.Add(new InputBinding(HomeViewCommand, new KeyGesture(Key.H, ModifierKeys.Control)));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Implement logic for closing the window (optional)
        }

        void WpfApplicationSample_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            statusTextBox.Text = "there was a click...";
        }
    }
}
