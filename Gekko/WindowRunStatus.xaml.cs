using System;
using System.Collections.Generic;
using System.Text;
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
using System.Drawing;
using System.Runtime.CompilerServices;
//using System.Windows.Forms;

namespace Gekko
{
    /// <summary>
    /// Interaction logic for WindowRunStatus.xaml
    /// </summary>
    public partial class WindowRunStatus : Window
    {
        public WindowRunStatus()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            if (Gui.gui.p == null) return;
            Gui.gui.p.ReportToRunStatus(true);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Globals.windowRunStatus = null;  //we want the window to be a singleton window (only 1 possible at a time)
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (tabStack.IsSelected)
                {
                    Globals.windowRunStatusIsStackTab = true;
                }
                else if (tabStatus.IsSelected)
                {
                    Globals.windowRunStatusIsStackTab = false;
                }
            }
            if (Gui.gui.p == null) return;
            Gui.gui.p.ReportToRunStatus(true);
        }
    }

    
}
