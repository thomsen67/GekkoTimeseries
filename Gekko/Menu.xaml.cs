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

namespace Gekko
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();            
        }

        private void webBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Uri != null && (e.Uri.AbsoluteUri.ToLower().EndsWith("." + Globals.extensionTable) || e.Uri.AbsoluteUri.ToLower().EndsWith("." + "tab")))
            {
                //cancel Navigation                
                string file = e.Uri.AbsoluteUri;
                if (file.ToLower().StartsWith("file:///"))
                {
                    file = file.Substring(8);
                    file = file.Replace("/", "\\");
                    Gui.gui.StartThread("table " + file, true);  //to get a worker thread started
                    CrossThreadStuff.SetTab("main", true);
                    e.Cancel = true;
                }
                else
                {
                    MessageBox.Show("*** ERROR: Expected table file to be on local file system");
                }
            }
            //Gui.gui.GuiBrowseArrowsStuff(null, ETabs.Tables);  //has no effect, new page has not been loaded yet!
        }

        private void webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Gui.gui.GuiBrowseArrowsStuff(null, false, ETabs.Menu);  //has no effect, new page has not been loaded yet!
        }
    }
}
