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
    /// Interaction logic for WindowMessageBox.xaml
    /// </summary>
    public partial class WindowMessageBox : Window
    {
        public EMessageBox type = EMessageBox.Normal;
        
        public WindowMessageBox(EMessageBox type)
        {
            InitializeComponent();
            this.type = type;
            if (this.type == EMessageBox.Pause)
            {                
                this.Title = "Pause";
                this.textBox1.Text = "Pausing current Gekko job";
                this.button1.Content = "Continue";
                this.button2.Content = "Stop";
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (this.type == EMessageBox.Pause)
            {
                new Error("Execution stopped by user");
            }
            else
            {
                System.Windows.Forms.Clipboard.SetText(this.textBox1.Text, System.Windows.Forms.TextDataFormat.Text);
            }
        }
    }
}
