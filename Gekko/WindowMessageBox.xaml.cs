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
        //public WindowMessageBox(bool copyButton)
        //{
        //    this.button2.IsEnabled = copyButton;
        //    InitializeComponent();
        //}
        
        public WindowMessageBox()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(this.textBox1.Text, System.Windows.Forms.TextDataFormat.Text);
        }
    }
}
