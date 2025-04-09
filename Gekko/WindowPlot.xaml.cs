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

namespace Gekko
{
    /// <summary>
    /// Interaction logic for WindowPlot.xaml
    /// </summary>
    public partial class WindowPlot : Window
    {
        public GraphOptions graphOptions = null;
        public bool _shown;

        public WindowPlot(GraphOptions graphOptions)
        {            
            this.graphOptions = graphOptions;            

            Globals.disableRadioButtons = 1;
            try
            {
                InitializeComponent();
            }
            finally
            {
                Globals.disableRadioButtons = 0;
            }
            
            this.Left = Globals.guiGraphWindowLeftDistance;
            this.Top = Globals.guiGraphWindowTopDistance;
            if (graphOptions.code != null)
            {
                Globals.disableRadioButtons = 1;
                try
                {
                    bool isR = false;
                    bool isL = false;
                    string opRawLower = graphOptions.code.ToLower();
                    if (opRawLower.EndsWith("l"))
                    {
                        opRawLower = opRawLower.Substring(0, opRawLower.Length - 1);
                        isL = true;
                    }
                    if (opRawLower.StartsWith("r"))
                    {
                        opRawLower = opRawLower.Substring(1);
                        isR = true;
                    }                    
                    if (isR) CheckBox_ref.IsChecked = true;
                    if (isL) CheckBox_log.IsChecked = true;
                    if (G.Equal(opRawLower, "n")) radioButton_n1.IsChecked = true;
                    else if (G.Equal(opRawLower, "d")) radioButton_d.IsChecked = true;
                    else if (G.Equal(opRawLower, "p")) radioButton_p.IsChecked = true;
                    else if (G.Equal(opRawLower, "dp")) radioButton_dp.IsChecked = true;
                    else if (G.Equal(opRawLower, "m")) radioButton_m.IsChecked = true;
                    else if (G.Equal(opRawLower, "q")) radioButton_q.IsChecked = true;
                    else if (G.Equal(opRawLower, "mp")) radioButton_mp.IsChecked = true;
                }
                finally
                {
                    Globals.disableRadioButtons = 0;
                }
            }
            webBrowser.Source = new Uri(graphOptions.emfName);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (_shown) return;
            _shown = true;
            this.graphOptions.windowIsShown = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            Globals.guiGraphWindowTopDistance = (int)Math.Max(1, this.Top);
            Globals.guiGraphWindowLeftDistance = (int)Math.Max(1, this.Left);
            try
            {
                if (Globals.windowsPlot != null && this != null) Globals.windowsPlot.Remove(this);
            }
            catch { }
        }

        private void CheckBox_ref_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_ref_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_shares_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_shares_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_index_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_index_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_log_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {                
                Refresh();
            }
        }        

        private void CheckBox_log_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_n1_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_d_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }        

        private void radioButton_p_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_dp_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_n2_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_m_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_q_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_mp_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void Button_copy(object sender, RoutedEventArgs e)
        {
            // Copy the .svg file to the clipboard for use in e.g. Word
            //string[] ss = new string[1];
            //ss[0] = this.graphOptions.emfName;
            //IDataObject iData = new DataObject(DataFormats.FileDrop, ss);
            //Clipboard.SetDataObject(iData, true);
            Clipboard.SetText(this.graphOptions.emfName + " (use Insert --> Pictures...)");
        }

        private void Button_save(object sender, RoutedEventArgs e)
        {
            //Copy the .svg file to file for later use in e.g. Word
            //File.
            string input = "gekkoplot";
            string inputLast = "svg";
            string name2 = Program.Add1ToFileName(input, inputLast, Program.options.folder_working);
            string enddir = Program.options.folder_working + "\\" + name2;
            Program.WaitForFileCopy(this.graphOptions.emfName, enddir);
            this.label1.Text = "File " + name2;
            this.label2.Text = "saved in working folder";
        }

        private void Button_saveas(object sender, RoutedEventArgs e)
        {
            string plotName = Refresh(new GraphHelper(GetOperator(), true, CheckBox_log.IsChecked == true, 1d / 2d, CheckBox_index.IsChecked == true));
            Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "svg files (*.svg)|*.svg|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                InitialDirectory = Program.options.folder_working
            };
            if (saveFileDialog1.ShowDialog() == true)
            {
                Program.WaitForFileCopy(plotName, saveFileDialog1.FileName);
                this.label1.Text = "File saved";
                this.label2.Text = "";
            }
        }

        private void Button_refresh(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CloseCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private string Refresh()
        {
            return Refresh(new GraphHelper(GetOperator(), true, CheckBox_log.IsChecked == true, 1d, CheckBox_index.IsChecked == true));
        }

        private string Refresh(GraphHelper gh)
        {
            string emfName = Globals.printStorageAsFunc[this.graphOptions.printStorageAsFuncCounter](gh);
            webBrowser.Source = new Uri(emfName);
            return emfName;
        }

        private string GetOperator()
        {
            string op = "n";
            if (radioButton_n1.IsChecked == true) op = "n";
            else if (radioButton_n2.IsChecked == true) op = "n";
            else if (radioButton_d.IsChecked == true) op = "d";
            else if (radioButton_p.IsChecked == true) op = "p";
            else if (radioButton_dp.IsChecked == true) op = "dp";
            else if (radioButton_m.IsChecked == true) op = "m";
            else if (radioButton_q.IsChecked == true) op = "q";
            else if (radioButton_mp.IsChecked == true) op = "mp";
            return op;
        }
    }
}
