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
        
        public WindowPlot(GraphOptions graphOptions)
        {
            this.graphOptions = graphOptions;
            InitializeComponent();
            if (graphOptions.code != null)
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
            webBrowser.Source = new Uri(graphOptions.emfName);
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

        }

        private void CheckBox_ref_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_shares_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_shares_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_index_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_index_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_log_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_log_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void radioButton_n1_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void radioButton_d_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void radioButton_p_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void radioButton_dp_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void radioButton_n2_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void radioButton_m_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void radioButton_q_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void radioButton_mp_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_copy(object sender, RoutedEventArgs e)
        {

        }

        private void Button_save(object sender, RoutedEventArgs e)
        {

        }

        private void Button_saveas(object sender, RoutedEventArgs e)
        {

        }

        private void Button_refresh(object sender, RoutedEventArgs e)
        {

        }

        private void CloseCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
