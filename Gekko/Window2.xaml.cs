/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2021, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.        
*/

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
//using System.Web.UI.WebControls;

namespace Gekko
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public P p;
        public List<string> NotUsed = new List<string>();
        public string type = "close";
        
        public Window2()
        {            
            InitializeComponent();
            textBox1.Background=new SolidColorBrush(Globals.LightGray);            
            this.Top = Globals.guiErrorWindowTopDistance;
            this.Left = Globals.guiErrorWindowLeftDistance;
        }        

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.ToString() == "Escape")
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            type = "stop";
            this.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            type = "skip";
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            type = "retry";
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Globals.guiErrorWindowTopDistance = Math.Max(1, (int)this.Top);
            Globals.guiErrorWindowLeftDistance = Math.Max(1, (int)this.Left);
            p.timeAtLastUserInteraction = DateTime.Now;  //clearing this
        }
    }
}
