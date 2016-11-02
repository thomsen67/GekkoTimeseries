/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2016, Thomas Thomsen, T-T Analyse.

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



using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.IO;


namespace Gekko
{

    public delegate void CloseDelegate();

    public class Graph : Form
    {
        //public string csCode = null;

        public RadioButton radioButton1;
        public RadioButton radioButton2;
        public RadioButton radioButton3;
        public RadioButton radioButton5;
        public RadioButton radioButton8;
        public CheckBox checkBox1;
        private GroupBox groupBox1;

        //public string emfName = "";
        //public List<string> graphVars;
        //public List<string> graphVarsNames;
        //public GekkoTime tStart;
        //public List<Dictionary<string, string>> precedents;
        //public int tSubStart;

        private ToolTip toolTip1;
        private IContainer components;

        private Button button1;
        private Button button2;
        private Button button3;
        private System.Windows.Forms.Label label1;
        private Button button4;
        //public GekkoTime tEnd;
        //public int tSubEnd;
        public GraphOptions graphOptions = null;
        public Table dd;
        //public string csSnippet = null;
        public bool graphErrors = false;

        public Graph(GraphOptions input)
        {
            this.graphOptions = input;
            this.StartPosition = FormStartPosition.Manual;
            //if (this.graphOptions.csSnippet == null) this.graphOptions.csSnippet = Globals.lastPrtCsSnippet;
            //if (this.graphOptions.csSnippetHeader == null) this.graphOptions.csSnippetHeader = Globals.lastPrtCsSnippetHeader;

            InitializeComponent();
            Shown += Form1_Shown;

            Text = "Gekko graph";
            if (input.title != null && input.title != "") Text = input.title;

            if (true)
            {
                this.Left = Globals.guiGraphWindowLeftDistance;
                this.Top = Globals.guiGraphWindowTopDistance;
            }            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {            
            this.graphOptions.windowIsShown = true;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Graph));
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.BackColor = System.Drawing.Color.Transparent;
            this.radioButton1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton1.Location = new System.Drawing.Point(15, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(58, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Normal";
            this.toolTip1.SetToolTip(this.radioButton1, "No data transformation. Key = \'n\'");
            this.radioButton1.UseVisualStyleBackColor = false;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            this.radioButton1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.radioButton1_KeyDown);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.BackColor = System.Drawing.Color.Transparent;
            this.radioButton2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton2.Location = new System.Drawing.Point(15, 39);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(43, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Log";
            this.toolTip1.SetToolTip(this.radioButton2, "Log of variable: log(x). Key = \'l\'");
            this.radioButton2.UseVisualStyleBackColor = false;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.radioButton2_KeyDown);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton3.Location = new System.Drawing.Point(100, 39);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(79, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Percent (%)";
            this.toolTip1.SetToolTip(this.radioButton3, "Percent change: (x/x(-1))*100. In multiplier: (x/y-1)*100. Key = \'p\'");
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.radioButton3_KeyDown);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.BackColor = System.Drawing.Color.Transparent;
            this.radioButton5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton5.Location = new System.Drawing.Point(100, 16);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(74, 17);
            this.radioButton5.TabIndex = 4;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Difference";
            this.toolTip1.SetToolTip(this.radioButton5, "Absolute difference: x-x(-1). In multiplier: x-x0. Key = \'d\'");
            this.radioButton5.UseVisualStyleBackColor = false;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            this.radioButton5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.radioButton5_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.radioButton8);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton5);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.groupBox1.Location = new System.Drawing.Point(91, 493);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(210, 94);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Graph type";
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton8.Location = new System.Drawing.Point(100, 62);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(93, 17);
            this.radioButton8.TabIndex = 4;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "Log difference";
            this.toolTip1.SetToolTip(this.radioButton8, "Log difference: log(x/x(-1). In multiplier: log(x/y). Key = \'D\'");
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged_1);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 15000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.Location = new System.Drawing.Point(335, 510);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(67, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Multiplier";
            this.toolTip1.SetToolTip(this.checkBox1, "Whether to show relative to lagged variable, or relative to reference values. \r\nD" +
        "oes not apply to \"Normal\" and \"Log\" types. Key = \'m\'");
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(446, 503);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Copy";
            this.toolTip1.SetToolTip(this.button1, "Copies the graph to clipboard as a \r\n.emf file, for use in e.g. Word");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(563, 564);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Refresh";
            this.toolTip1.SetToolTip(this.button2, "Update values from databanks (changing graph type also refreshes values from data" +
        "banks)");
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(446, 532);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(62, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Save";
            this.toolTip1.SetToolTip(this.button3, "Saves the graph as a .emf file for later use in e.g. Word. \r\nThe name is gekkogra" +
        "[number].emf");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(446, 564);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(72, 23);
            this.button4.TabIndex = 13;
            this.button4.Text = "Save As...";
            this.toolTip1.SetToolTip(this.button4, "Saves the graph as a .emf file, for later use\r\n in e.g. Word  (the user chooses a" +
        " file name)");
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(447, 598);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "          ";
            // 
            // Graph
            // 
            this.ClientSize = new System.Drawing.Size(700, 626);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Graph";
            this.Text = "Graph";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Graph_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            bool ok = true;
            if (!File.Exists(this.graphOptions.emfName)) ok = false;
            FileInfo fi = new FileInfo(this.graphOptions.emfName);
            if (fi.Length == 0) ok = false;
            if (ok == false)
            {
                //will just show a blank graph
                //printing an error message or messagebox here is difficult, since OnPaint() gets repeated
                //many times.
                //The issue is typically that all values are missing for all timeseries, so scaling becomes impossible.
                return;
            }

            Graphics g = e.Graphics;

            LinearGradientBrush gradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(Width, Width), Color.FromArgb(Globals.graphBackground, Globals.graphBackground, Globals.graphBackground), Color.FromArgb(Globals.graphBackground, Globals.graphBackground, Globals.graphBackground));

            g.FillRectangle(gradientBrush, ClientRectangle);

            Metafile myMetafile = null;

            try
            {
                myMetafile = new Metafile(this.graphOptions.emfName);
                g.DrawImage(myMetafile, 0, 0, 660, 495);
                myMetafile = null;
            }
            catch (Exception e1)
            {
                MessageBox.Show("*** ERROR: Something went wrong in GnuPlot, when trying to make .emf file");                
            }            
        }

        private void refresh()
        {
            bool isLevel = radioButton1.Checked;
            bool isLog = radioButton2.Checked;
            bool isPch = radioButton3.Checked;
            bool isDiff = radioButton5.Checked;
            bool isDlog = radioButton8.Checked;
            bool isMultiplier = checkBox1.Checked;

            long counter = this.graphOptions.counter;
            string code1 = Globals.prtCsSnippets[counter];
            string code2 = Globals.prtCsSnippetsHeaders[counter];


            string cs = code1;
            if (isMultiplier)
            {
                cs = cs.Replace("O.Prt.GetBankNumbers(null,", "O.Prt.GetBankNumbers(`m`,");
            }
            this.graphOptions.o = Program.PrtSnippet(cs, code2);
            if (this.graphOptions.o == null) return;  //If so, an error box has been shown.    
            this.graphOptions.o.printCodes = new List<OptString>();

            string s = null;
            if (isLevel) s = "n";
            else if (isLog) s = "n";
            else if (isPch && isMultiplier) s = "q";
            else if (isPch && !isMultiplier) s = "p";
            else if (isDiff && isMultiplier) s = "m";
            else if (isDiff && !isMultiplier) s = "d";
            else if (isDlog && isMultiplier) s = "m";
            else if (isDlog && !isMultiplier) s = "d";

            if (isDlog || isLog) this.graphOptions.o.guiGraphIsLogTransform = true;                        
            this.graphOptions.o.printCodes.Add(new OptString(s, "yes"));
            this.graphOptions.o.guiGraphIsRefreshing = true;
            this.graphOptions.o.Exe();            
            this.graphOptions.emfName = this.graphOptions.o.guiGraphRefreshingFilename;            
            Invalidate();
        }

        private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
        {

            if (radioButton1.Checked == true)
            {
                if (Globals.disableRationButtons == 0) refresh();
            }
        }

        private void radioButton2_CheckedChanged(object sender, System.EventArgs e)
        {

            if (radioButton2.Checked == true)
            {
                if (Globals.disableRationButtons == 0) refresh();
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
            {
                if (Globals.disableRationButtons == 0) refresh();
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                if (Globals.disableRationButtons == 0) refresh();
            }
        }

        private void radioButton8_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton8.Checked == true)
            {
                if (Globals.disableRationButtons == 0) refresh();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (Globals.disableRationButtons == 0) refresh();
        }

        private void radioButton1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void radioButton2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void radioButton5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void radioButton3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void radioButton8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateGraph();
        }

        public void UpdateGraph()
        {
            if (Globals.disableRationButtons == 0)
            {
                this.graphOptions.localBanks = null;  //clearing this, forcing window to use vales from Gekko databanks
                try
                {                    
                    refresh();
                }
                catch
                {
                    MessageBox.Show("*** ERROR: Graph update failed: maybe some variables or databanks are non-available?");
                }                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Copy the .emf file to the clipboard for use in e.g. Word
            string[] ss = new string[1];
            ss[0] = this.graphOptions.emfName;
            IDataObject iData = new DataObject(DataFormats.FileDrop, ss);
            Clipboard.SetDataObject(iData, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Copy the .emf file to file for later use in e.g. Word
            //File.

            string input = "gekkogra";
            string inputLast = "emf";

            string name2 = Program.Add1ToFileName(input, inputLast, Program.options.folder_working);

            string enddir = Program.options.folder_working + "\\" + name2;
            Program.WaitForFileCopy(this.graphOptions.emfName, enddir);
            this.label1.Text = "File " + name2 + " saved in working folder";            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "emf files (*.emf)|*.emf|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = Program.options.folder_working;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.WaitForFileCopy(this.graphOptions.emfName, saveFileDialog1.FileName);
                this.label1.Text = "File saved";
            }
        }

        //Close the graph with Escape key, no matter the focus
        //Should work on any form, and regardless of focus.
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            try
            {
                if (msg.WParam.ToInt32() == (int)Keys.Escape)
                {                    
                    this.Close();
                }
                else
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Key Overrided Events Error:" + Ex.Message);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Graph_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.guiGraphWindowTopDistance = Math.Max(1, this.Top);
            Globals.guiGraphWindowLeftDistance = Math.Max(1, this.Left);
            Globals.windowsGraph.Remove(this);
        }
    }

    public class GraphOptions
    {
        public O.Prt o = null;        
        public string emfName = "";
        public PrtPplotHelper pph = null;
        public PrtOptionsHelper po = null;
        public List<string> graphVars;
        public List<string> graphVarsNames;
        public List<Dictionary<string, string>> precedents = null;
        public GekkoTime tStart;
        public GekkoTime tEnd;
        public LocalBanks localBanks = new LocalBanks();
        public string title;
        //public string csSnippet = null;
        //public string csSnippetHeader = null;
        public bool windowIsShown = false;
        public long counter = -12345;
        
        public GraphOptions()
        {
            this.localBanks.localWork = new Databank("localWork");
            this.localBanks.localBase = new Databank("localBase");
        }
        
    }
}
