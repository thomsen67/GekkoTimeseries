using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Windows.Forms.Integration;

namespace Gekko
{
    public partial class Gui
    {
        
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gui));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new Gekko.SplitContainerFix();
            this.textBox2 = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxAutoComplete = new Gekko.GListBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBoxTooltip = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.setWorkingDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteTempFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.editorStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gekkoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gekko2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rS2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commandHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCommandHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.utilitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.makebatFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comparecheckDatabanksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comparecheckEquationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareModeldatabankvarlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pCIMConvertersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertPCIMTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertPCIMMenusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSPUtilitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSPImportEqsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSPImportDataToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCurrentOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreUserSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewDatabanksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.allPPLOTUDVALGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allPPLOTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allUDVALGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.showMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allPPLOTAndUDVALGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allPPLOTToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.allUDVALGToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3a = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new System.Drawing.Point(0, 64);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(15, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new System.Drawing.Size(974, 608);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(966, 575);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.ToolTipText = "Main window for running Gekko (Ctrl+M)";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBox2);
            this.splitContainer1.Panel2.Controls.Add(this.listBoxAutoComplete);
            this.splitContainer1.Size = new System.Drawing.Size(966, 575);
            this.splitContainer1.SplitterDistance = 441;
            this.splitContainer1.SplitterWidth = 9;
            this.splitContainer1.TabIndex = 6;
            this.splitContainer1.TabStop = false;
            // 
            // textBox2
            // 
            this.textBox2.AcceptsTab = true;
            this.textBox2.AllowDrop = true;
            this.textBox2.AutoWordSelection = true;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.ContextMenuStrip = this.contextMenuStrip1;
            this.textBox2.DetectUrls = false;
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(0, 0);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(966, 125);
            this.textBox2.TabIndex = 0;
            this.textBox2.Text = "";
            this.textBox2.WordWrap = false;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            this.textBox2.VisibleChanged += new System.EventHandler(this.textBox2_VisibleChanged);
            this.textBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyDown);
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            this.textBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator1,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator2,
            this.selectAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(222, 196);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(221, 30);
            this.undoToolStripMenuItem.Text = "Undo (Ctrl+Z)";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(221, 30);
            this.redoToolStripMenuItem.Text = "Redo (Ctrl+Y)";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(218, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(221, 30);
            this.cutToolStripMenuItem.Text = "Cut (Ctrl+X)";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(221, 30);
            this.copyToolStripMenuItem.Text = "Copy (Ctrl+C)";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(221, 30);
            this.pasteToolStripMenuItem.Text = "Paste (Ctrl-V)";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(218, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(221, 30);
            this.selectAllToolStripMenuItem.Text = "Select All (Ctrl+A)";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // listBoxAutoComplete
            // 
            this.listBoxAutoComplete.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxAutoComplete.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxAutoComplete.ImageList = this.imageList1;
            this.listBoxAutoComplete.Location = new System.Drawing.Point(204, 443);
            this.listBoxAutoComplete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBoxAutoComplete.Name = "listBoxAutoComplete";
            this.listBoxAutoComplete.Size = new System.Drawing.Size(311, 80);
            this.listBoxAutoComplete.TabIndex = 3;
            this.listBoxAutoComplete.Visible = false;
            this.listBoxAutoComplete.SelectedIndexChanged += new System.EventHandler(this.listBoxAutoComplete_SelectedIndexChanged);
            this.listBoxAutoComplete.DoubleClick += new System.EventHandler(this.listBoxAutoComplete_DoubleClick);
            this.listBoxAutoComplete.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxAutoComplete_KeyDown);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Lime;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Transparent;
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(966, 575);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Output";
            this.tabPage2.ToolTipText = "Output of bulky stuff like long lists etc. (Ctrl+O)";
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.webBrowser);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage4.Size = new System.Drawing.Size(966, 575);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Menu";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(4, 5);
            this.webBrowser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(958, 565);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            this.webBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(641, 358);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Help";
            this.tabPage3.ToolTipText = "Gekko help system (Ctrl+H)";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBoxTooltip
            // 
            this.textBoxTooltip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(225)))));
            this.textBoxTooltip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTooltip.Location = new System.Drawing.Point(552, 443);
            this.textBoxTooltip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxTooltip.Multiline = true;
            this.textBoxTooltip.Name = "textBoxTooltip";
            this.textBoxTooltip.ReadOnly = true;
            this.textBoxTooltip.Size = new System.Drawing.Size(149, 30);
            this.textBoxTooltip.TabIndex = 5;
            this.textBoxTooltip.Visible = false;
            this.textBoxTooltip.Enter += new System.EventHandler(this.textBoxTooltip_Enter);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton5,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton6});
            this.toolStrip1.Location = new System.Drawing.Point(0, 33);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(974, 31);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Enabled = false;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "Browse backwards";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Enabled = false;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.ToolTipText = "Browse forwards";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Enabled = false;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton5.Text = "toolStripButton5";
            this.toolStripButton5.ToolTipText = "Browse to start";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Enabled = false;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton3.Text = "Stop current job";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click_1);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Enabled = false;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton4.Text = "Copy last PRT/MULPRT, SHOW or table to clipboard (for spreadsheet pasting)";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton6.Text = "Close all PLOT and DECOMP windows";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.editToolStripMenuItem,
            this.utilitiesToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(974, 33);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setWorkingDirToolStripMenuItem,
            this.recentFoldersToolStripMenuItem,
            this.toolStripSeparator7,
            this.deleteTempFilesToolStripMenuItem,
            this.toolStripSeparator3,
            this.closeToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(50, 29);
            this.toolStripMenuItem1.Text = "File";
            // 
            // setWorkingDirToolStripMenuItem
            // 
            this.setWorkingDirToolStripMenuItem.Name = "setWorkingDirToolStripMenuItem";
            this.setWorkingDirToolStripMenuItem.Size = new System.Drawing.Size(254, 30);
            this.setWorkingDirToolStripMenuItem.Text = "Set working folder...";
            this.setWorkingDirToolStripMenuItem.Click += new System.EventHandler(this.setWorkingDirToolStripMenuItem_Click);
            // 
            // recentFoldersToolStripMenuItem
            // 
            this.recentFoldersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.toolStripMenuItem12,
            this.toolStripMenuItem14,
            this.toolStripMenuItem15,
            this.toolStripMenuItem16,
            this.toolStripMenuItem17,
            this.toolStripMenuItem18,
            this.toolStripMenuItem19,
            this.toolStripMenuItem20,
            this.toolStripMenuItem21,
            this.toolStripMenuItem22,
            this.toolStripMenuItem23});
            this.recentFoldersToolStripMenuItem.Name = "recentFoldersToolStripMenuItem";
            this.recentFoldersToolStripMenuItem.Size = new System.Drawing.Size(254, 30);
            this.recentFoldersToolStripMenuItem.Text = "Recent folders";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem3.Text = "1. ";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem4.Text = "2. ";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem5.Text = "3. ";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem6.Text = "4. ";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem7.Text = "5. ";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem8.Text = "6. ";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem9.Text = "7. ";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem9_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem10.Text = "8. ";
            this.toolStripMenuItem10.Click += new System.EventHandler(this.toolStripMenuItem10_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem11.Text = "9. ";
            this.toolStripMenuItem11.Click += new System.EventHandler(this.toolStripMenuItem11_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem12.Text = "10. ";
            this.toolStripMenuItem12.Click += new System.EventHandler(this.toolStripMenuItem12_Click);
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem14.Text = "11.";
            this.toolStripMenuItem14.Click += new System.EventHandler(this.toolStripMenuItem14_Click);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem15.Text = "12.";
            this.toolStripMenuItem15.Click += new System.EventHandler(this.toolStripMenuItem15_Click);
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem16.Text = "13.";
            this.toolStripMenuItem16.Click += new System.EventHandler(this.toolStripMenuItem16_Click);
            // 
            // toolStripMenuItem17
            // 
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            this.toolStripMenuItem17.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem17.Text = "14.";
            this.toolStripMenuItem17.Click += new System.EventHandler(this.toolStripMenuItem17_Click);
            // 
            // toolStripMenuItem18
            // 
            this.toolStripMenuItem18.Name = "toolStripMenuItem18";
            this.toolStripMenuItem18.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem18.Text = "15.";
            this.toolStripMenuItem18.Click += new System.EventHandler(this.toolStripMenuItem18_Click);
            // 
            // toolStripMenuItem19
            // 
            this.toolStripMenuItem19.Name = "toolStripMenuItem19";
            this.toolStripMenuItem19.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem19.Text = "16.";
            this.toolStripMenuItem19.Click += new System.EventHandler(this.toolStripMenuItem19_Click);
            // 
            // toolStripMenuItem20
            // 
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            this.toolStripMenuItem20.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem20.Text = "17.";
            this.toolStripMenuItem20.Click += new System.EventHandler(this.toolStripMenuItem20_Click);
            // 
            // toolStripMenuItem21
            // 
            this.toolStripMenuItem21.Name = "toolStripMenuItem21";
            this.toolStripMenuItem21.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem21.Text = "18.";
            this.toolStripMenuItem21.Click += new System.EventHandler(this.toolStripMenuItem21_Click);
            // 
            // toolStripMenuItem22
            // 
            this.toolStripMenuItem22.Name = "toolStripMenuItem22";
            this.toolStripMenuItem22.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem22.Text = "19.";
            this.toolStripMenuItem22.Click += new System.EventHandler(this.toolStripMenuItem22_Click);
            // 
            // toolStripMenuItem23
            // 
            this.toolStripMenuItem23.Name = "toolStripMenuItem23";
            this.toolStripMenuItem23.Size = new System.Drawing.Size(125, 30);
            this.toolStripMenuItem23.Text = "20.";
            this.toolStripMenuItem23.Click += new System.EventHandler(this.toolStripMenuItem23_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(251, 6);
            // 
            // deleteTempFilesToolStripMenuItem
            // 
            this.deleteTempFilesToolStripMenuItem.Name = "deleteTempFilesToolStripMenuItem";
            this.deleteTempFilesToolStripMenuItem.Size = new System.Drawing.Size(254, 30);
            this.deleteTempFilesToolStripMenuItem.Text = "Delete temp files...";
            this.deleteTempFilesToolStripMenuItem.Click += new System.EventHandler(this.deleteTempFilesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(251, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(254, 30);
            this.closeToolStripMenuItem.Text = "Exit";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem1,
            this.redoToolStripMenuItem1,
            this.toolStripSeparator4,
            this.cutToolStripMenuItem1,
            this.copyToolStripMenuItem1,
            this.pasteToolStripMenuItem1,
            this.toolStripSeparator5,
            this.selectAllToolStripMenuItem1,
            this.toolStripSeparator6,
            this.editorStyleToolStripMenuItem,
            this.clearCommandHistoryToolStripMenuItem,
            this.commandHistoryToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem1
            // 
            this.undoToolStripMenuItem1.Name = "undoToolStripMenuItem1";
            this.undoToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+Z";
            this.undoToolStripMenuItem1.Size = new System.Drawing.Size(280, 30);
            this.undoToolStripMenuItem1.Text = "Undo";
            this.undoToolStripMenuItem1.Click += new System.EventHandler(this.undoToolStripMenuItem1_Click);
            // 
            // redoToolStripMenuItem1
            // 
            this.redoToolStripMenuItem1.Name = "redoToolStripMenuItem1";
            this.redoToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+Y";
            this.redoToolStripMenuItem1.Size = new System.Drawing.Size(280, 30);
            this.redoToolStripMenuItem1.Text = "Redo";
            this.redoToolStripMenuItem1.Click += new System.EventHandler(this.redoToolStripMenuItem1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(277, 6);
            // 
            // cutToolStripMenuItem1
            // 
            this.cutToolStripMenuItem1.Name = "cutToolStripMenuItem1";
            this.cutToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+X";
            this.cutToolStripMenuItem1.Size = new System.Drawing.Size(280, 30);
            this.cutToolStripMenuItem1.Text = "Cut";
            this.cutToolStripMenuItem1.Click += new System.EventHandler(this.cutToolStripMenuItem1_Click);
            // 
            // copyToolStripMenuItem1
            // 
            this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            this.copyToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+C";
            this.copyToolStripMenuItem1.Size = new System.Drawing.Size(280, 30);
            this.copyToolStripMenuItem1.Text = "Copy";
            this.copyToolStripMenuItem1.Click += new System.EventHandler(this.copyToolStripMenuItem1_Click);
            // 
            // pasteToolStripMenuItem1
            // 
            this.pasteToolStripMenuItem1.Name = "pasteToolStripMenuItem1";
            this.pasteToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+V";
            this.pasteToolStripMenuItem1.Size = new System.Drawing.Size(280, 30);
            this.pasteToolStripMenuItem1.Text = "Paste";
            this.pasteToolStripMenuItem1.Click += new System.EventHandler(this.pasteToolStripMenuItem1_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(277, 6);
            // 
            // selectAllToolStripMenuItem1
            // 
            this.selectAllToolStripMenuItem1.Name = "selectAllToolStripMenuItem1";
            this.selectAllToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+A";
            this.selectAllToolStripMenuItem1.Size = new System.Drawing.Size(280, 30);
            this.selectAllToolStripMenuItem1.Text = "Select All";
            this.selectAllToolStripMenuItem1.Click += new System.EventHandler(this.selectAllToolStripMenuItem1_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(277, 6);
            // 
            // editorStyleToolStripMenuItem
            // 
            this.editorStyleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gekkoToolStripMenuItem,
            this.gekko2ToolStripMenuItem,
            this.rSToolStripMenuItem,
            this.rS2ToolStripMenuItem});
            this.editorStyleToolStripMenuItem.Name = "editorStyleToolStripMenuItem";
            this.editorStyleToolStripMenuItem.Size = new System.Drawing.Size(280, 30);
            this.editorStyleToolStripMenuItem.Text = "Editor style";
            this.editorStyleToolStripMenuItem.ToolTipText = "Choose how the lower part of the main window works";
            // 
            // gekkoToolStripMenuItem
            // 
            this.gekkoToolStripMenuItem.Name = "gekkoToolStripMenuItem";
            this.gekkoToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.gekkoToolStripMenuItem.Text = "Gekko";
            this.gekkoToolStripMenuItem.ToolTipText = "[Enter] executes, [Ctrl+Enter] issues new line. No new line after [Enter]";
            this.gekkoToolStripMenuItem.Click += new System.EventHandler(this.gekkoToolStripMenuItem_Click);
            // 
            // gekko2ToolStripMenuItem
            // 
            this.gekko2ToolStripMenuItem.Name = "gekko2ToolStripMenuItem";
            this.gekko2ToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.gekko2ToolStripMenuItem.Text = "Gekko2";
            this.gekko2ToolStripMenuItem.ToolTipText = "[Enter] executes, [Ctrl+Enter] issues new line. New line after [Enter]";
            this.gekko2ToolStripMenuItem.Click += new System.EventHandler(this.gekko2ToolStripMenuItem_Click);
            // 
            // rSToolStripMenuItem
            // 
            this.rSToolStripMenuItem.Name = "rSToolStripMenuItem";
            this.rSToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.rSToolStripMenuItem.Text = "RS";
            this.rSToolStripMenuItem.ToolTipText = "[Enter] issues new line, [Ctrl+Enter] executes. New line after [Ctrl+Enter]";
            this.rSToolStripMenuItem.Click += new System.EventHandler(this.rSToolStripMenuItem_Click);
            // 
            // rS2ToolStripMenuItem
            // 
            this.rS2ToolStripMenuItem.Name = "rS2ToolStripMenuItem";
            this.rS2ToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.rS2ToolStripMenuItem.Text = "RS2";
            this.rS2ToolStripMenuItem.ToolTipText = "[Enter] issues new line, [Ctrl+Enter] executes. No new line after [Ctrl+Enter]";
            this.rS2ToolStripMenuItem.Click += new System.EventHandler(this.rS2ToolStripMenuItem_Click);
            // 
            // commandHistoryToolStripMenuItem
            // 
            this.commandHistoryToolStripMenuItem.Name = "commandHistoryToolStripMenuItem";
            this.commandHistoryToolStripMenuItem.Size = new System.Drawing.Size(280, 30);
            this.commandHistoryToolStripMenuItem.Text = "Command history...";
            this.commandHistoryToolStripMenuItem.Click += new System.EventHandler(this.commandHistoryToolStripMenuItem_Click);
            // 
            // clearCommandHistoryToolStripMenuItem
            // 
            this.clearCommandHistoryToolStripMenuItem.Name = "clearCommandHistoryToolStripMenuItem";
            this.clearCommandHistoryToolStripMenuItem.Size = new System.Drawing.Size(280, 30);
            this.clearCommandHistoryToolStripMenuItem.Text = "Clear command history";
            this.clearCommandHistoryToolStripMenuItem.Click += new System.EventHandler(this.clearCommandHistoryToolStripMenuItem_Click);
            // 
            // utilitiesToolStripMenuItem
            // 
            this.utilitiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.makebatFileToolStripMenuItem,
            this.comparecheckDatabanksToolStripMenuItem,
            this.comparecheckEquationsToolStripMenuItem,
            this.compareModeldatabankvarlistToolStripMenuItem,
            this.convertersToolStripMenuItem,
            this.runStatusToolStripMenuItem});
            this.utilitiesToolStripMenuItem.Name = "utilitiesToolStripMenuItem";
            this.utilitiesToolStripMenuItem.Size = new System.Drawing.Size(81, 29);
            this.utilitiesToolStripMenuItem.Text = "Utilities";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(480, 30);
            this.toolStripMenuItem2.Text = "Make shortcut (.lnk file) on your desktop folder...";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // makebatFileToolStripMenuItem
            // 
            this.makebatFileToolStripMenuItem.Name = "makebatFileToolStripMenuItem";
            this.makebatFileToolStripMenuItem.Size = new System.Drawing.Size(480, 30);
            this.makebatFileToolStripMenuItem.Text = "Make .bat file for easy Gekko startup...";
            this.makebatFileToolStripMenuItem.Click += new System.EventHandler(this.makebatFileToolStripMenuItem_Click);
            // 
            // comparecheckDatabanksToolStripMenuItem
            // 
            this.comparecheckDatabanksToolStripMenuItem.Name = "comparecheckDatabanksToolStripMenuItem";
            this.comparecheckDatabanksToolStripMenuItem.Size = new System.Drawing.Size(480, 30);
            this.comparecheckDatabanksToolStripMenuItem.Text = "Compare two databanks...";
            this.comparecheckDatabanksToolStripMenuItem.Click += new System.EventHandler(this.comparecheckDatabanksToolStripMenuItem_Click);
            // 
            // comparecheckEquationsToolStripMenuItem
            // 
            this.comparecheckEquationsToolStripMenuItem.Name = "comparecheckEquationsToolStripMenuItem";
            this.comparecheckEquationsToolStripMenuItem.Size = new System.Drawing.Size(480, 30);
            this.comparecheckEquationsToolStripMenuItem.Text = "Check residuals...";
            this.comparecheckEquationsToolStripMenuItem.Click += new System.EventHandler(this.comparecheckEquationsToolStripMenuItem_Click);
            // 
            // compareModeldatabankvarlistToolStripMenuItem
            // 
            this.compareModeldatabankvarlistToolStripMenuItem.Name = "compareModeldatabankvarlistToolStripMenuItem";
            this.compareModeldatabankvarlistToolStripMenuItem.Size = new System.Drawing.Size(480, 30);
            this.compareModeldatabankvarlistToolStripMenuItem.Text = "Compare model/databank/varlist...";
            this.compareModeldatabankvarlistToolStripMenuItem.Click += new System.EventHandler(this.compareModeldatabankvarlistToolStripMenuItem_Click);
            // 
            // convertersToolStripMenuItem
            // 
            this.convertersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pCIMConvertersToolStripMenuItem,
            this.tSPUtilitiesToolStripMenuItem});
            this.convertersToolStripMenuItem.Name = "convertersToolStripMenuItem";
            this.convertersToolStripMenuItem.Size = new System.Drawing.Size(480, 30);
            this.convertersToolStripMenuItem.Text = "Converters";
            // 
            // pCIMConvertersToolStripMenuItem
            // 
            this.pCIMConvertersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertPCIMTablesToolStripMenuItem,
            this.convertPCIMMenusToolStripMenuItem});
            this.pCIMConvertersToolStripMenuItem.Name = "pCIMConvertersToolStripMenuItem";
            this.pCIMConvertersToolStripMenuItem.Size = new System.Drawing.Size(225, 30);
            this.pCIMConvertersToolStripMenuItem.Text = "PCIM converters";
            // 
            // convertPCIMTablesToolStripMenuItem
            // 
            this.convertPCIMTablesToolStripMenuItem.Name = "convertPCIMTablesToolStripMenuItem";
            this.convertPCIMTablesToolStripMenuItem.Size = new System.Drawing.Size(275, 30);
            this.convertPCIMTablesToolStripMenuItem.Text = "Convert PCIM tables...";
            this.convertPCIMTablesToolStripMenuItem.Click += new System.EventHandler(this.convertPCIMTablesToolStripMenuItem_Click_1);
            // 
            // convertPCIMMenusToolStripMenuItem
            // 
            this.convertPCIMMenusToolStripMenuItem.Name = "convertPCIMMenusToolStripMenuItem";
            this.convertPCIMMenusToolStripMenuItem.Size = new System.Drawing.Size(275, 30);
            this.convertPCIMMenusToolStripMenuItem.Text = "Convert PCIM menus...";
            this.convertPCIMMenusToolStripMenuItem.Click += new System.EventHandler(this.convertPCIMMenusToolStripMenuItem_Click_1);
            // 
            // tSPUtilitiesToolStripMenuItem
            // 
            this.tSPUtilitiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSPImportEqsToolStripMenuItem,
            this.tSPImportDataToolStripMenuItem1});
            this.tSPUtilitiesToolStripMenuItem.Name = "tSPUtilitiesToolStripMenuItem";
            this.tSPUtilitiesToolStripMenuItem.Size = new System.Drawing.Size(225, 30);
            this.tSPUtilitiesToolStripMenuItem.Text = "TSP converters";
            // 
            // tSPImportEqsToolStripMenuItem
            // 
            this.tSPImportEqsToolStripMenuItem.Name = "tSPImportEqsToolStripMenuItem";
            this.tSPImportEqsToolStripMenuItem.Size = new System.Drawing.Size(236, 30);
            this.tSPImportEqsToolStripMenuItem.Text = "TSP import eqs...";
            this.tSPImportEqsToolStripMenuItem.Click += new System.EventHandler(this.tSPImportEqsToolStripMenuItem_Click_1);
            // 
            // tSPImportDataToolStripMenuItem1
            // 
            this.tSPImportDataToolStripMenuItem1.Name = "tSPImportDataToolStripMenuItem1";
            this.tSPImportDataToolStripMenuItem1.Size = new System.Drawing.Size(236, 30);
            this.tSPImportDataToolStripMenuItem1.Text = "TSP import data...";
            this.tSPImportDataToolStripMenuItem1.Click += new System.EventHandler(this.tSPImportDataToolStripMenuItem1_Click_1);
            // 
            // runStatusToolStripMenuItem
            // 
            this.runStatusToolStripMenuItem.Name = "runStatusToolStripMenuItem";
            this.runStatusToolStripMenuItem.Size = new System.Drawing.Size(480, 30);
            this.runStatusToolStripMenuItem.Text = "Run status...";
            this.runStatusToolStripMenuItem.Click += new System.EventHandler(this.runStatusToolStripMenuItem_Click_1);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showCurrentOptionsToolStripMenuItem,
            this.restoreUserSettingsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(88, 29);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // showCurrentOptionsToolStripMenuItem
            // 
            this.showCurrentOptionsToolStripMenuItem.Name = "showCurrentOptionsToolStripMenuItem";
            this.showCurrentOptionsToolStripMenuItem.Size = new System.Drawing.Size(272, 30);
            this.showCurrentOptionsToolStripMenuItem.Text = "Show current options";
            this.showCurrentOptionsToolStripMenuItem.Click += new System.EventHandler(this.showCurrentOptionsToolStripMenuItem_Click);
            // 
            // restoreUserSettingsToolStripMenuItem
            // 
            this.restoreUserSettingsToolStripMenuItem.Name = "restoreUserSettingsToolStripMenuItem";
            this.restoreUserSettingsToolStripMenuItem.Size = new System.Drawing.Size(272, 30);
            this.restoreUserSettingsToolStripMenuItem.Text = "Restore user settings...";
            this.restoreUserSettingsToolStripMenuItem.Click += new System.EventHandler(this.restoreUserSettingsToolStripMenuItem_Click);
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewDatabanksToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // viewDatabanksToolStripMenuItem
            // 
            this.viewDatabanksToolStripMenuItem.Name = "viewDatabanksToolStripMenuItem";
            this.viewDatabanksToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.viewDatabanksToolStripMenuItem.Size = new System.Drawing.Size(263, 30);
            this.viewDatabanksToolStripMenuItem.Text = "View databanks...";
            this.viewDatabanksToolStripMenuItem.Click += new System.EventHandler(this.viewDatabanksToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem1,
            this.toolStripSeparator8,
            this.showMenuToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(90, 29);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // closeToolStripMenuItem1
            // 
            this.closeToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allPPLOTUDVALGToolStripMenuItem,
            this.allPPLOTToolStripMenuItem,
            this.allUDVALGToolStripMenuItem});
            this.closeToolStripMenuItem1.Name = "closeToolStripMenuItem1";
            this.closeToolStripMenuItem1.Size = new System.Drawing.Size(231, 30);
            this.closeToolStripMenuItem1.Text = "Close";
            this.closeToolStripMenuItem1.ToolTipText = "Closes all windows of specific type";
            // 
            // allPPLOTUDVALGToolStripMenuItem
            // 
            this.allPPLOTUDVALGToolStripMenuItem.Name = "allPPLOTUDVALGToolStripMenuItem";
            this.allPPLOTUDVALGToolStripMenuItem.Size = new System.Drawing.Size(290, 30);
            this.allPPLOTUDVALGToolStripMenuItem.Text = "Close all PLOT+DECOMP";
            this.allPPLOTUDVALGToolStripMenuItem.ToolTipText = "Close all PLOT and DECOMP windows";
            this.allPPLOTUDVALGToolStripMenuItem.Click += new System.EventHandler(this.allPPLOTUDVALGToolStripMenuItem_Click);
            // 
            // allPPLOTToolStripMenuItem
            // 
            this.allPPLOTToolStripMenuItem.Name = "allPPLOTToolStripMenuItem";
            this.allPPLOTToolStripMenuItem.Size = new System.Drawing.Size(290, 30);
            this.allPPLOTToolStripMenuItem.Text = "Close all PLOT";
            this.allPPLOTToolStripMenuItem.ToolTipText = "Close all PLOT windows";
            this.allPPLOTToolStripMenuItem.Click += new System.EventHandler(this.allPPLOTToolStripMenuItem_Click);
            // 
            // allUDVALGToolStripMenuItem
            // 
            this.allUDVALGToolStripMenuItem.Name = "allUDVALGToolStripMenuItem";
            this.allUDVALGToolStripMenuItem.Size = new System.Drawing.Size(290, 30);
            this.allUDVALGToolStripMenuItem.Text = "Close all DECOMP";
            this.allUDVALGToolStripMenuItem.ToolTipText = "Close all DECOMP windows";
            this.allUDVALGToolStripMenuItem.Click += new System.EventHandler(this.allUDVALGToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(228, 6);
            // 
            // showMenuToolStripMenuItem
            // 
            this.showMenuToolStripMenuItem.Name = "showMenuToolStripMenuItem";
            this.showMenuToolStripMenuItem.Size = new System.Drawing.Size(231, 30);
            this.showMenuToolStripMenuItem.Text = "Restart Menu tab";
            this.showMenuToolStripMenuItem.ToolTipText = "Restart Menu system (same as using \'Browse to start\' button while on \'Menu\' tab)." +
    "";
            this.showMenuToolStripMenuItem.Click += new System.EventHandler(this.showMenuToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(250, 30);
            this.manualToolStripMenuItem.Text = "Gekko Help File";
            this.manualToolStripMenuItem.Click += new System.EventHandler(this.manualToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(250, 30);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allPPLOTAndUDVALGToolStripMenuItem,
            this.allPPLOTToolStripMenuItem1,
            this.allUDVALGToolStripMenuItem1});
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.updateToolStripMenuItem.Text = "Update";
            this.updateToolStripMenuItem.ToolTipText = "Updates/refreshes all windows of specific type";
            // 
            // allPPLOTAndUDVALGToolStripMenuItem
            // 
            this.allPPLOTAndUDVALGToolStripMenuItem.Name = "allPPLOTAndUDVALGToolStripMenuItem";
            this.allPPLOTAndUDVALGToolStripMenuItem.Size = new System.Drawing.Size(305, 30);
            this.allPPLOTAndUDVALGToolStripMenuItem.Text = "Update all PLOT+DECOMP";
            this.allPPLOTAndUDVALGToolStripMenuItem.ToolTipText = "Updates/refreshes all PLOT and DECOMP windows";
            this.allPPLOTAndUDVALGToolStripMenuItem.Click += new System.EventHandler(this.allPPLOTAndUDVALGToolStripMenuItem_Click);
            // 
            // allPPLOTToolStripMenuItem1
            // 
            this.allPPLOTToolStripMenuItem1.Name = "allPPLOTToolStripMenuItem1";
            this.allPPLOTToolStripMenuItem1.Size = new System.Drawing.Size(305, 30);
            this.allPPLOTToolStripMenuItem1.Text = "Update all PLOT";
            this.allPPLOTToolStripMenuItem1.ToolTipText = "Updates/refreshes all PLOT windows";
            this.allPPLOTToolStripMenuItem1.Click += new System.EventHandler(this.allPPLOTToolStripMenuItem1_Click);
            // 
            // allUDVALGToolStripMenuItem1
            // 
            this.allUDVALGToolStripMenuItem1.Name = "allUDVALGToolStripMenuItem1";
            this.allUDVALGToolStripMenuItem1.Size = new System.Drawing.Size(305, 30);
            this.allUDVALGToolStripMenuItem1.Text = "Update all DECOMP";
            this.allUDVALGToolStripMenuItem1.ToolTipText = "Updates/refreshes all DECOMP windows";
            this.allUDVALGToolStripMenuItem1.Click += new System.EventHandler(this.allUDVALGToolStripMenuItem1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel3a});
            this.statusStrip1.Location = new System.Drawing.Point(0, 672);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(974, 28);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(899, 23);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Status field";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(0, 23);
            this.toolStripStatusLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.AutoSize = false;
            this.toolStripStatusLabel5.AutoToolTip = true;
            this.toolStripStatusLabel5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(23, 23);
            this.toolStripStatusLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripStatusLabel5.ToolTipText = "Number of goals set (ENDO/EXO variables)";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 23);
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.AutoSize = false;
            this.toolStripStatusLabel3.AutoToolTip = true;
            this.toolStripStatusLabel3.DoubleClickEnabled = true;
            this.toolStripStatusLabel3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel3.Image")));
            this.toolStripStatusLabel3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(17, 23);
            this.toolStripStatusLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripStatusLabel3.ToolTipText = "Job status: green = ready // yellow = working // red = aborted\nDouble click to op" +
    "en \'Run status\' window.";
            this.toolStripStatusLabel3.DoubleClick += new System.EventHandler(this.toolStripStatusLabel3_DoubleClick);
            // 
            // toolStripStatusLabel3a
            // 
            this.toolStripStatusLabel3a.AutoSize = false;
            this.toolStripStatusLabel3a.AutoToolTip = true;
            this.toolStripStatusLabel3a.Name = "toolStripStatusLabel3a";
            this.toolStripStatusLabel3a.Size = new System.Drawing.Size(12, 23);
            this.toolStripStatusLabel3a.Text = " ";
            this.toolStripStatusLabel3a.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(90, 42);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 23);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // Gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 700);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBoxTooltip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Gui";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Gekko";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.commands_Load);
            this.Shown += new System.EventHandler(this.Gui_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Gekko_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public TabControl tabControl1;
        public System.Windows.Forms.RichTextBox textBox2;        
        private GListBox listBoxAutoComplete;
        private GListBox testGL;

        //private System.Windows.Forms.TreeView treeViewItems;
        private System.Windows.Forms.TextBox textBoxTooltip;
        private System.Windows.Forms.ImageList imageList1 = new ImageList();

        private System.Windows.Forms.ToolStrip toolStrip1;
        public System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        public System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripMenuItem utilitiesToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;

        //private RichTextBox textBox2;
        //private AutoComplete textBox2;
        private SplitContainerFix splitContainer1;

        private ToolStripMenuItem setWorkingDirToolStripMenuItem;
        private ToolStripMenuItem makebatFileToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem restoreUserSettingsToolStripMenuItem;              

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            //MessageBox.Show(e.CloseReason.ToString() + " " + Globals.applicationIsInProcessOfAborting.ToString());

            if (e.CloseReason == CloseReason.UserClosing || (e.CloseReason == CloseReason.ApplicationExitCall && !Globals.applicationIsInProcessOfAborting))  //Globals.applicationIsInProcessOfAborting is issued with an EXIT command
            {
                
                //Hitting X on the window, or closing via file menu including Alt-F4.
                if (MessageBox.Show("Quit Gekko session?", "Gekko", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                //could be EXIT command, or a Windows reboot
            }
        }

        private void richTextBox777_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.Control && e.KeyCode == Keys.C) || e.KeyCode == Keys.C)
            {
                //the above is equal to just e.KeyCode == Keys.C. The thing is, if a plain c or C is entered, it is caught elsewhere, since focus
                //is set to lower panel. So this more or less catches the aftermath of a Ctrl-C.
                //Ctrl-C
                //See also #98075243587
                string s = Clipboard.GetText(TextDataFormat.Text);
                TokenList ths = StringTokenizer2.GetTokensWithLeftBlanks(s, 10);
                List<string> x = new List<string>();
                x.Add("disp");
                x.Add("disp2");
                x.Add("disp3");
                x.Add("help");
                x.Add("stacktrace");
                for (int i = 0; i < ths.storage.Count; i++)
                {
                    if (ths[i].s == "#" && ths[i + 1].leftblanks == 0 && x.Contains(ths[i + 1].s.ToLower()) && ths[i + 2].leftblanks == 0 && ths[i + 2].s == ":")
                    {
                        ths[i].s = "";
                        ths[i + 1].s = "";
                        ths[i + 2].s = "";
                        ths[i + 3].s = "";
                    }
                }
                string ss = null;
                foreach (TokenHelper th in ths.storage)
                {
                    ss += G.Blanks(th.leftblanks);
                    ss += th.s;
                }
                Clipboard.SetText(ss, TextDataFormat.Text);
                e.Handled = true;                
            }            
        }

        private void richTextBox777_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //if (e.Control && e.KeyCode == Keys.C)
            //{
            //    //Ctrl-C
            //    string s = Clipboard.GetText(TextDataFormat.Text);
            //    Clipboard.SetText(s, TextDataFormat.Text);
            //    e.Handled = true;
            //}
            //else if (!(e.Alt || e.Control || e.Shift))

            if (!(e.Alt || e.Control || e.Shift))
            {
                if ((e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z) || (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9))
                {
                    //like this, the letter will be transferred for instance if it is a new command.
                    //other keys like +, -, etc. will not be transferred but are lost. Seems to work
                    //sort of okay. Typically, the user ends a command with enter, and then clicks on the
                    //top window. The logic here avoids the next sequence of chars getting lost, since the first
                    //char is typically a letter (first letter of a command). So for most purposes it works ok.
                    //Ctrl+C etc. in the upper window work ok.
                    string s = ("" + (char)e.KeyValue).ToLower();
                    this.textBox2.SelectedText = s;
                    this.textBox2.Focus();
                }
            }
        }

        private void richTextBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            ////It is possible to catch key events here, since textbox2 typically has the focus.
            //// Determine whether the key entered is the F1 key. If it is, display Help.
            //if (e.KeyCode == Keys.F1 && (e.Alt || e.Control || e.Shift))
            //{
            //    // Display a pop-up Help topic to assist the user.
            //    Help.ShowPopup(textBox1, "Enter your name.", new Point(textBox1.Bottom, textBox1.Right));
            //}
            //else if (e.KeyCode == Keys.F2 && e.Modifiers == Keys.Alt)
            //{
            //    // Display a pop-up Help topic to provide additional assistance to the user.
            //    Help.ShowPopup(textBox1, "Enter your first name followed by your last name. Middle name is optional.",
            //        new Point(textBox1.Top, this.textBox1.Left));
            //}

            bool isLessThanSign = false;
            if (!e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.OemBackslash) isLessThanSign = true;

            bool isNormalSpace = false;
            if (!e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.Space) isNormalSpace = true;

            bool isCtrlSpace = false;
            if (e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.Space) isCtrlSpace = true;

            if (e.KeyCode == Keys.F1)
            {
                Program.Help("I_OVERVIEW");
            }
            else if ((e.KeyData == (Keys.ControlKey | Keys.Enter)))
            {
                string insertText = "\n";
                int selectionIndex = textBox2.SelectionStart;
                textBox2.Text = textBox2.Text.Insert(selectionIndex, insertText);
                textBox2.SelectionStart = selectionIndex + insertText.Length;
            }
            else if (e.KeyCode == Keys.F2)
            {
                WindowOpenDatabanks wd = new WindowOpenDatabanks();
                wd.ShowDialog();
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                textBox2.Paste(DataFormats.GetFormat(DataFormats.Text));  //to avoid formatting, colors etc., when pasting from e.g. Word examples, mails etc.
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                if (this.textBox2.SelectedText != null)
                {
                    Clipboard.SetText(this.textBox2.SelectedText);     //to avoid formatting, colors etc. when pasting to Word, in a mail
                }
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.M)
            {
                CrossThreadStuff.SetTab("main", true);
            }
            //else if (e.Control && e.KeyCode == Keys.H)
            //{
            //    CrossThreadStuff.SetTab("help", true);
            //}
            else if (e.Control && e.KeyCode == Keys.O)
            {
                CrossThreadStuff.SetTab("output", true);
            }
            else if (e.Control && e.KeyCode == Keys.U)
            {
                CrossThreadStuff.SetTab("menu", true);
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                e.Handled = true;  //ignore -- else will right-justify
            }
            else if (e.Control && e.KeyCode == Keys.L)
            {
                e.Handled = true;  //ignore -- else will left-justify
            }
            else if (!e.Control && e.KeyCode == Keys.Return)  //Pure enter (Ctrl+Enter is equal to F2)
            {
                if (Globals.windowIntellisense != null && Globals.windowIntellisense.IsOpen && Globals.windowIntellisense.listBox1.SelectedItem != null)
                {
                    textBox2.SelectedText = ((System.Windows.Controls.ListBoxItem)Globals.windowIntellisense.listBox1.SelectedItem).Content.ToString();
                    Globals.windowIntellisense.IsOpen = false;
                    e.Handled = true;
                }
                else
                {
                    if (Globals.windowIntellisense != null) Globals.windowIntellisense.IsOpen = false; //if intellise window was open but nothing was selected before enter was hit

                    Globals.startOfLinePositionWhenLastEnterPressed = -12345;
                    Globals.endOfLinePositionWhenLastEnterPressed = -12345;

                    //Program.ShowPeriodInStatusField("");

                    string s2 = null;

                    string selected = textBox2.SelectedText;

                    int line2;
                    int firstChar;
                    int column2;
                    TextInputHelper(out line2, out firstChar, out column2);
                    Globals.startOfLinePositionWhenLastEnterPressed = firstChar;
                    Globals.commandMemory.lengthWhenLastEnterPressed = Globals.commandMemory.storage.ToString().Length;


                    if (selected != "")
                    {
                        s2 = selected;
                        e.Handled = true;  //otherwise the text block is deleted
                    }
                    else
                    {
                        //not in selection mode
                        //does not (and should not) handle & sign in relation to line breaks
                        //Strange: needed after the textbox is rtf type.......

                        //TODO: can give error with multi-line input (line>size)
                        if (textBox2.Lines.Length > line2)  //must be so, otherwise we get error
                        {
                            s2 = textBox2.Lines[line2];
                            if (Globals.startOfLinePositionWhenLastEnterPressed != -12345) Globals.endOfLinePositionWhenLastEnterPressed = Globals.startOfLinePositionWhenLastEnterPressed + s2.Length;
                            bool isAtLastColumn = false;
                            if (s2.Length == column2) isAtLastColumn = true;
                            bool isAtLastRow = false;
                            if (line2 + 1 == textBox2.Lines.Length) isAtLastRow = true;
                            if (!isAtLastColumn)
                            {                                
                                //in the middle of a line --> never no new line
                                e.Handled = true;
                            }
                            else if (!isAtLastRow)
                            {
                                //at the end of the non-last line -> no new line, but maybe semicolon
                                if (s2.Trim() != "" && !s2.Trim().EndsWith(";"))
                                {
                                    textBox2.SelectedText = ";";
                                    s2 = s2 + ";";
                                }
                                e.Handled = true;
                            }
                            else
                            {
                                //at the end of the last line -> new line and semicolon
                                if (s2.Trim() != "" && !s2.Trim().EndsWith(";"))
                                {
                                    textBox2.SelectedText = ";";
                                    s2 = s2 + ";";
                                }
                            }
                        }
                    }  //if selection mode

                    if (s2 != null)
                    {
                        //try
                        //{
                            //Commands.obeyCommand(s2);
                            this.StartThread(s2, true);
                        //}
                        //catch (Exception e2)
                        //{
                        //    Program.PrintExceptionAndFinishThread(e2, s2, null);
                        //}
                        //Program.ShowPeriodInStatusField("");
                    }
                    else
                    {
                        //Just silently ignore this empty command
                    }
                }  //if intellisense open
            }  //if enter key (both command return and intellisense select)
            else if (isNormalSpace || isLessThanSign)
            {
                bool ok = true;
                string keyword = null;
                if (isNormalSpace) keyword = "space";
                else if (isLessThanSign) keyword = "less";
                else ok = false;
                if (ok) StartIntellisense(keyword, Program.options.interface_suggestions);
            }
            else if (isCtrlSpace)
            {
                //StartIntellisense("space", "all");
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (Globals.windowIntellisense != null && Globals.windowIntellisense.IsOpen)
                {
                    if (Globals.windowIntellisense.listBox1.SelectedIndex < Globals.windowIntellisense.listBox1.Items.Count - 1)
                    {
                        Globals.windowIntellisense.listBox1.SelectedIndex++;
                        Globals.windowIntellisense.listBox1.ScrollIntoView(Globals.windowIntellisense.listBox1.SelectedItem);
                    }
                    e.Handled = true;

                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (Globals.windowIntellisense != null && Globals.windowIntellisense.IsOpen)
                {
                    if (Globals.windowIntellisense.listBox1.SelectedIndex > 0)
                    {
                        Globals.windowIntellisense.listBox1.SelectedIndex--;
                        Globals.windowIntellisense.listBox1.ScrollIntoView(Globals.windowIntellisense.listBox1.SelectedItem);
                    }
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Return)
            {
                //see above
            }
            else
            {
                if (Globals.windowIntellisense != null) Globals.windowIntellisense.IsOpen = false;
            }
        }

        private void StartIntellisense(string keyword, string type)
        {
            int line2;
            int firstChar;
            int column2;
            TextInputHelper(out line2, out firstChar, out column2);
            string s2 = null;
            try
            {
                s2 = textBox2.Lines[line2].Substring(0, column2);
            }
            catch
            {
                if (line2 == 0 && column2 == 0)
                {
                    //an enter at the very beginning before any lines are made in textbox
                }
                else return;  //should not be possible, but we fail silently.
            }

            if (s2 == null) s2 = "";
            s2 = s2.Trim();

            if (keyword == "space") s2 += " ";
            else if (keyword == "less") s2 += "<";

            if (G.Equal(type, "none"))
            {
                return;
            }
            else if (G.Equal(type, "option"))
            {
                if (keyword == "space")
                {
                    bool ok = false;
                    if (s2.StartsWith("option ", StringComparison.OrdinalIgnoreCase))
                    {
                        ok = true;
                    }
                    if (ok == false) return;
                }
                else if (keyword == "less")
                {
                    return;
                }
            }
            else if (G.Equal(type, "some"))
            {
                if (keyword == "space")
                {
                    bool ok = false;
                    if (s2.StartsWith("option ", StringComparison.OrdinalIgnoreCase))
                    {
                        ok = true;
                    }
                    if (s2.Contains("<") && !s2.Contains(">"))
                    {
                        ok = true;
                    }
                    if (ok == false) return;
                }
            }
            else if (G.Equal(type, "all"))
            {
                //do nothing
            }


            List<string> suggestions = ParserOLD.IntelliParserNew(s2);

            if (suggestions.Count > 0)
            {

                if (Globals.windowIntellisense == null)
                {
                    Globals.windowIntellisense = new WindowIntellisense();
                }
                Globals.windowIntellisense.listBox1.Items.Clear();
                foreach (string suggest in suggestions)
                {
                    System.Windows.Controls.ListBoxItem li = new System.Windows.Controls.ListBoxItem();
                    li.Content = suggest;
                    li.MouseEnter += new System.Windows.Input.MouseEventHandler(Globals.windowIntellisense.listBoxItem_PreviewMouseEnter);
                    li.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(Globals.windowIntellisense.listBoxItem_PreviewMouseDown);
                    Globals.windowIntellisense.listBox1.Items.Add(li);
                }

                // Find the position of the caret
                Point point = this.textBox2.GetPositionFromCharIndex(textBox2.SelectionStart);
                point.Y += (int)Math.Ceiling(this.textBox2.Font.GetHeight()) + 2 + 2;
                point.X += 2; // for Courier, may need a better method
                Point location = this.textBox2.PointToScreen(Point.Empty);
                Globals.windowIntellisense.VerticalOffset = point.Y + location.Y;
                Globals.windowIntellisense.HorizontalOffset = point.X + location.X;
                Globals.windowIntellisense.IsOpen = true;
                Globals.windowIntellisense.listBox1.SelectedIndex = 0;
                Globals.windowIntellisense.listBox1.ScrollIntoView(Globals.windowIntellisense.listBox1.SelectedItem);
            }
        }

        private void TextInputHelper(out int line2, out int firstChar, out int column2)
        {
            line2 = textBox2.GetLineFromCharIndex(textBox2.GetFirstCharIndexOfCurrentLine());
            firstChar = textBox2.GetFirstCharIndexFromLine(line2);
            column2 = textBox2.SelectionStart - firstChar;
        }

        private void richTextBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Hide the listview and the tooltip
            this.textBoxTooltip.Hide();
            this.listBoxAutoComplete.Hide();
        }

        private void listBoxAutoComplete_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Ignore any keys being pressed on the listview
            this.textBox2.Focus();
        }

        private void listBoxAutoComplete_DoubleClick(object sender, System.EventArgs e)
        {
            // Item double clicked, select it
            if (this.listBoxAutoComplete.SelectedItems.Count == 1)
            {
                this.wordMatched = true;
                this.selectItem();
                this.listBoxAutoComplete.Hide();
                this.textBox2.Focus();
                this.wordMatched = false;
            }
        }

        private void listBoxAutoComplete_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Make sure when an item is selected, control is returned back to the richtext
            this.textBox2.Focus();
        }

        private void textBoxTooltip_Enter(object sender, System.EventArgs e)
        {
            // Stop the fake tooltip's text being selected
            this.textBox2.Focus();
        }

        /// <summary>
        /// Called when a "." is pressed - the previous word is found,
        /// and if matched in the treeview, the members listbox is
        /// populated with items from the tree, which are first sorted.
        /// </summary>
        /// <returns>Whether an items are found for the word</returns>
        private bool populateListBox(string s)
        {
            return true;
            //bool result = true;
            //VBScriptParser parser = new VBScriptParser();
            //StringReader reader = new StringReader(s);
            //NonTerminalNode syntaxNode = parser.Parse(reader) as NonTerminalNode;
            //string exp = "";
            //string[] s5 = new string[TreeBuilder.VBScriptParser.expectedStatement.Count];
            //int i = 0;
            //this.listBoxAutoComplete.Items.Clear();
            //foreach (string s3 in TreeBuilder.VBScriptParser.expectedStatement)
            //{
            //    exp = exp + s3 + " ";
            //    s5[i] = (string)s3;
            //    this.listBoxAutoComplete.Items.Add(new GListBoxItem(s3, 0));
            //    i++;
            //}
            //Program.ShowPeriodInStatusField(" | Expects: " + exp);  //also updates expected statements
            //if (syntaxNode != null)
            //{
            //}
            //else
            //{
            //}
            ////this.listBoxAutoComplete.Items.Add(new GListBoxItem("genr", -1));
            ////this.listBoxAutoComplete.Items.Add(new GListBoxItem("upd", -1));
            //return result;
        }

        /// <summary>
        /// Autofills the selected item in the member listbox, by
        /// taking everything before and after the "." in the richtextbox,
        /// and appending the word in the middle.
        /// </summary>
        private void selectItem()
        {
            if (this.wordMatched)
            {
                int selstart = this.textBox2.SelectionStart;
                int prefixend = this.textBox2.SelectionStart - typed.Length;
                int suffixstart = this.textBox2.SelectionStart + typed.Length;

                if (suffixstart >= this.textBox2.Text.Length)
                {
                    suffixstart = this.textBox2.Text.Length;
                }

                string prefix = this.textBox2.Text.Substring(0, prefixend);
                string fill = this.listBoxAutoComplete.SelectedItem.ToString();
                string suffix = this.textBox2.Text.Substring(suffixstart, this.textBox2.Text.Length - suffixstart);

                this.textBox2.Text = prefix + fill + suffix;
                this.textBox2.SelectionStart = prefix.Length + fill.Length;
            }
        }
        public ToolStripButton toolStripButton2;
        private ToolStripMenuItem comparecheckDatabanksToolStripMenuItem;
        private ToolStripMenuItem comparecheckEquationsToolStripMenuItem;
        private ToolStripMenuItem compareModeldatabankvarlistToolStripMenuItem;
        public TabPage tabPage1;
        public TabPage tabPage2;
        public TabPage tabPage3;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem showCurrentOptionsToolStripMenuItem;
        private ToolStripMenuItem recentFoldersToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        private ToolStripMenuItem toolStripMenuItem10;
        private ToolStripMenuItem toolStripMenuItem11;
        private ToolStripMenuItem toolStripMenuItem12;
        private ToolStripMenuItem toolStripMenuItem2;
        public ToolStripStatusLabel toolStripStatusLabel3;
        public ToolStripStatusLabel toolStripStatusLabel3a;
        private ToolStripStatusLabel toolStripStatusLabel4;
        private ToolStripStatusLabel toolStripStatusLabel5;
        private PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        public ToolStripButton toolStripButton3;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem convertersToolStripMenuItem;
        private ToolStripMenuItem tSPUtilitiesToolStripMenuItem;
        private ToolStripMenuItem tSPImportEqsToolStripMenuItem;
        private ToolStripMenuItem tSPImportDataToolStripMenuItem1;
        public TabPage tabPage4;
        public WebBrowser webBrowser;
        //private ElementHost elementHost1;
        //private Menu menu1;
        private ToolStripMenuItem pCIMConvertersToolStripMenuItem;
        private ToolStripMenuItem convertPCIMTablesToolStripMenuItem;
        private ToolStripMenuItem convertPCIMMenusToolStripMenuItem;
        private ToolStripMenuItem manualToolStripMenuItem;
        private ToolStripMenuItem dataToolStripMenuItem;
        private ToolStripMenuItem viewDatabanksToolStripMenuItem;
        private ToolStripMenuItem runStatusToolStripMenuItem;
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem1;
        private ToolStripMenuItem allPPLOTUDVALGToolStripMenuItem;
        private ToolStripMenuItem allPPLOTToolStripMenuItem;
        private ToolStripMenuItem allUDVALGToolStripMenuItem;
        private ToolStripMenuItem updateToolStripMenuItem;
        private ToolStripMenuItem allPPLOTAndUDVALGToolStripMenuItem;
        private ToolStripMenuItem allPPLOTToolStripMenuItem1;
        private ToolStripMenuItem allUDVALGToolStripMenuItem1;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem cutToolStripMenuItem1;
        private ToolStripMenuItem copyToolStripMenuItem1;
        private ToolStripMenuItem pasteToolStripMenuItem1;
        private ToolStripMenuItem commandHistoryToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem1;
        private ToolStripMenuItem redoToolStripMenuItem1;
        private ToolStripMenuItem selectAllToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem clearCommandHistoryToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem14;
        private ToolStripMenuItem toolStripMenuItem15;
        private ToolStripMenuItem toolStripMenuItem16;
        private ToolStripMenuItem toolStripMenuItem17;
        private ToolStripMenuItem toolStripMenuItem18;
        private ToolStripMenuItem toolStripMenuItem19;
        private ToolStripMenuItem toolStripMenuItem20;
        private ToolStripMenuItem toolStripMenuItem21;
        private ToolStripMenuItem toolStripMenuItem22;
        private ToolStripMenuItem toolStripMenuItem23;
        public ToolStripButton toolStripButton4;
        public ToolStripButton toolStripButton6;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem deleteTempFilesToolStripMenuItem;
        public ToolStripButton toolStripButton5;
        private ToolStripMenuItem showMenuToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem editorStyleToolStripMenuItem;
        public ToolStripMenuItem gekkoToolStripMenuItem;
        public ToolStripMenuItem gekko2ToolStripMenuItem;
        public ToolStripMenuItem rSToolStripMenuItem;
        public ToolStripMenuItem rS2ToolStripMenuItem;
        //private Settings settings1 = new Settings();
    }
}
