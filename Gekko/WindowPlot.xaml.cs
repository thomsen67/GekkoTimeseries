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

                    string codeWithoutR = G.Replace(graphOptions.code, "r", "", StringComparison.OrdinalIgnoreCase, 0);
                    bool isR = false; if (codeWithoutR.Length < graphOptions.code.Length) isR = true;
                    if (isR) CheckBox_ref.IsChecked = true;
                    if (G.Equal(codeWithoutR, "n")) radioButton_n1.IsChecked = true;
                    else if (G.Equal(codeWithoutR, "d")) radioButton_d.IsChecked = true;
                    else if (G.Equal(codeWithoutR, "p")) radioButton_p.IsChecked = true;
                    else if (G.Equal(codeWithoutR, "dp")) radioButton_dp.IsChecked = true;
                    else if (G.Equal(codeWithoutR, "m")) radioButton_m.IsChecked = true;
                    else if (G.Equal(codeWithoutR, "q")) radioButton_q.IsChecked = true;
                    else if (G.Equal(codeWithoutR, "mp")) radioButton_mp.IsChecked = true;
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
            }
        }

        private void CheckBox_ref_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
            }
        }

        private void CheckBox_shares_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
            }
        }

        private void CheckBox_shares_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
            }
        }

        private void CheckBox_index_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
            }
        }

        private void CheckBox_index_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
            }
        }

        private void CheckBox_log_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
            }
        }

        private void CheckBox_log_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
            }
        }

        private void radioButton_n1_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh(new GraphHelper("n", true, CheckBox_log.IsChecked == true, 1d, CheckBox_index.IsChecked == true, CheckBox_shares.IsChecked == true));
            }
        }

        private void radioButton_d_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh(new GraphHelper("d", true, CheckBox_log.IsChecked == true, 1d, CheckBox_index.IsChecked == true, CheckBox_shares.IsChecked == true));
            }
        }        

        private void radioButton_p_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh(new GraphHelper("p", true, CheckBox_log.IsChecked == true, 1d, CheckBox_index.IsChecked == true, CheckBox_shares.IsChecked == true));
            }
        }

        private void radioButton_dp_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh(new GraphHelper("dp", true, CheckBox_log.IsChecked == true, 1d, CheckBox_index.IsChecked == true, CheckBox_shares.IsChecked == true));
            }
        }

        private void radioButton_n2_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh(new GraphHelper("n", true, CheckBox_log.IsChecked == true, 1d, CheckBox_index.IsChecked == true, CheckBox_shares.IsChecked == true));
            }
        }

        private void radioButton_m_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh(new GraphHelper("m", true, CheckBox_log.IsChecked == true, 1d, CheckBox_index.IsChecked == true, CheckBox_shares.IsChecked == true));
            }
        }

        private void radioButton_q_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh(new GraphHelper("q", true, CheckBox_log.IsChecked == true, 1d, CheckBox_index.IsChecked == true, CheckBox_shares.IsChecked == true));
            }
        }

        private void radioButton_mp_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh(new GraphHelper("mp", true, CheckBox_log.IsChecked == true, 1d, CheckBox_index.IsChecked == true, CheckBox_shares.IsChecked == true));
            }
        }

        private void Button_copy(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                
            }
        }

        private void Button_save(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {

            }
        }

        private void Button_saveas(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {

            }
        }

        private void Button_refresh(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {

            }
        }

        private void CloseCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void Refresh(GraphHelper gh)
        {
            string emfName = Globals.printStorageAsFunc[this.graphOptions.printStorageAsFuncCounter](gh);
            webBrowser.Source = new Uri(emfName);
        }
    }
}
