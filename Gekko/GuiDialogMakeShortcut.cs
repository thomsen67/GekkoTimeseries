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
using System.IO;
//using VBIDE = Microsoft.Vbe.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Reflection;
using IWshRuntimeLibrary;

namespace Gekko
{
    public partial class GuiDialogMakeShortcut : Form
    {
        public GuiDialogMakeShortcut()
        {
            InitializeComponent();            
            this.textBox1.Text = "You may create a shortcut and place it in e.g. your desktop folder." +
             G.NL +
            "If you click OK now, Gekko will ask you to locate the intended folder and will copy the .lnk file to your " +
            "chosen destination. The file dialog will open up in the desktop folder." + G.NL;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            SaveFileDialog dlgSaveFile = new SaveFileDialog();
            //dlgSaveFile.InitialDirectory = Environment.GetEnvironmentVariable("SystemRoot");
            dlgSaveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dlgSaveFile.Filter = ".lnk file (*.lnk)|*.lnk";
            dlgSaveFile.FileName = "Gekko.lnk";
            DialogResult dlgResult = dlgSaveFile.ShowDialog();
            string m_strFileName = null;
            m_strFileName = dlgSaveFile.FileName;

            if (dlgResult == DialogResult.Cancel)
                return;
            try
            {
                //laver en link-fil
                if (true)
                {
                    // Create a new instance of WshShellClass
                    WshShellClass WshShell = new WshShellClass();
                    // Create the shortcut
                    IWshRuntimeLibrary.IWshShortcut MyShortcut;
                    // Choose the path for the shortcut
                    MyShortcut = (IWshRuntimeLibrary.IWshShortcut)WshShell.CreateShortcut(m_strFileName);
                    // Where the shortcut should point to
                    MyShortcut.TargetPath = Application.StartupPath + "\\Gekko.exe"; // " + "display hejsa;display hovsa";
                    // Description for the shortcut
                    MyShortcut.WorkingDirectory = Application.StartupPath;
                    MyShortcut.Description = "Link to Gekko";
                    // Location for the shortcut's icon
                    MyShortcut.IconLocation = Application.StartupPath + "\\Gekko.ico";
                    // Create the shortcut at the given path
                    MyShortcut.Save();
                    G.Writeln("Short-cut to Gekko copied to your desktop folder");                    
                }
            }
            catch (Exception err)
            {
                new Error("Problem when trying to write the gekko.lnk file to the desktop folder.");
            }
            G.Writeln("The gekko.lnk file was successfully created here: " + m_strFileName);
            G.Writeln();
        }
    }
}