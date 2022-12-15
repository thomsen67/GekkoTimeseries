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
    /// Interaction logic for WindowIntellisense.xaml
    /// </summary>
    public partial class WindowIntellisense : System.Windows.Controls.Primitives.Popup
    {
        private System.Timers.Timer ClickTimer;
        private int ClickCounter;
        //public static int listBoxHelper = 0;
        public string lastSelected = null;

        public WindowIntellisense()
        {
            ClickTimer = new System.Timers.Timer(300);
            ClickTimer.Elapsed += new System.Timers.ElapsedEventHandler(EvaluateClicks);
            InitializeComponent();
        }

        private void EvaluateClicks(object source, System.Timers.ElapsedEventArgs e)
        {
            ClickTimer.Stop();            
            if (ClickCounter == 2)
            {                
                CrossThreadStuff.Intellisense();                
            }
            ClickCounter = 0;            
        }        

        //private void listBox1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    //Print(sender);
        //    ClickTimer.Stop();
        //    ClickCounter++;
        //    ClickTimer.Start();            
        //    //MessageBox.Show("enkelt");            
        //}

        public void listBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {            
            ClickTimer.Stop();
            ClickCounter++;
            ClickTimer.Start();            
        }

        public void listBoxItem_PreviewMouseEnter(object sender, MouseEventArgs e)
        {            
            //ListBoxItem xx = (ListBoxItem)sender;
            //xx.IsSelected = true;
        }

        private static void Print(object sender)
        {
            System.Windows.Controls.ListBox l = (System.Windows.Controls.ListBox)sender;
            System.Windows.Controls.ListBoxItem li = (System.Windows.Controls.ListBoxItem)l.SelectedItem;
            if (li != null) Console.WriteLine(li.Content.ToString() + "  " + l.SelectedIndex);
            //listBoxHelper = l.SelectedIndex;

            G.Writeln("---");
            foreach (System.Windows.Controls.ListBoxItem xx in l.Items)
            {
                G.Writeln("" + xx.IsSelected.ToString());
            }
            G.Writeln("...");
            foreach (System.Windows.Controls.ListBoxItem xx in l.SelectedItems)
            {
                G.Writeln("!");
            }
            G.Writeln("===");

        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string s = null;
            System.Windows.Controls.ListBox l = (System.Windows.Controls.ListBox)sender;
            System.Windows.Controls.ListBoxItem li = (System.Windows.Controls.ListBoxItem)l.SelectedItem;
            if (li != null)
            {
                s = li.Content.ToString();
                lastSelected = s;
                //G.Writeln(s);
            }

            //Print(sender);
        }        

        private void listBox1_MouseEnter_1(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("ENTERING");
        }
    }
}
