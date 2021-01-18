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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Gekko
{
    public partial class GuiCompareUtility3Way : Form
    {
        public GuiCompareUtility3Way()
        {
            InitializeComponent();
            this.ActiveControl = button1;
            button1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (this.radioButton1.Checked) Program.SamE1();
            //else if (this.radioButton2.Checked) Program.SamE2();
            //else if (this.radioButton3.Checked) Program.SamE3();
            Program.CompareModelDatabankVarlist();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
