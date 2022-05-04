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

namespace Gekko
{
    public partial class GuiDialogMakeBatfile : Form
    {
        public static bool aremos = false;  //use this functionality to do the same regarding AREMOS.
        public string programName = "";
        public string programShortcut = "";
        public string callingPath = null;

        public GuiDialogMakeBatfile(bool makeBatFileForAremos)
        {
            aremos = makeBatFileForAremos;
            callingPath = System.IO.Directory.GetCurrentDirectory();
            InitializeComponent();
            string path = System.Environment.GetEnvironmentVariable("path");
            string[] path2 = path.Split(';');
            string path3 = "";
            foreach (string s in path2)
            {
                path3 += s + G.NL;
            }
            programName = "Gekko";
            programShortcut = "gekko";
            if (aremos) programName = "AREMOS";
            if (aremos) programShortcut = "wa";
            string okhelper = "Click OK to proceed (the file dialog will open up in " + Environment.GetEnvironmentVariable("SystemRoot") + "). See a list of your Windows path folders below." + G.NL + G.NL;
            if (aremos) okhelper = "";
            string helperString = "If you click OK now, Gekko will ask you to locate the intended folder and will copy the .bat file to the " +
            "chosen location. The content of the 'gekko.bat' file will be:" + G.NL +
            G.NL +
            "  @echo off" + G.NL +
            "  start " + Globals.QT + Globals.QT + " " + Globals.QT + Application.ExecutablePath + Globals.QT + G.NL +
            "  exit" + G.NL +
            G.NL;
            if (aremos) helperString = "If you click OK now, Gekko will ask you to do the following two steps:" + G.NL + G.NL +
                " (a) locate the AREMOS exe file (for instance C:\\WAREM32\\warem32.exe)" + G.NL +
                " (b) locate the folder to copy the .bat file into (for instance C:\\Windows)." + G.NL + G.NL + G.NL;

            this.textBox1.Text = "In order for " + programName + " to be able to start up by typing \"" + programShortcut + "\" on a system command " +
            "prompt, a .bat file needs to be added in a folder somewhere in the Windows path, for instance in " +
            Environment.GetEnvironmentVariable("SystemRoot") + "." + G.NL + G.NL +
            "If such a '" + programShortcut + ".bat' file resides in a folder belonging to one of the Windows path folders, it will be " +
            "called whenever the user writes '" + programShortcut + "' on a system command prompt (system or DOS shell). " +
            "The benefit of this is that " + programName + " remembers the folder from where '" + programShortcut + "' was typed, and will use " +
            "this folder as it's working folder." + G.NL +
             helperString + "\n" +
            "NOTE: The Windows path is a so-called 'Environment variable' (Danish: 'miljøvariabel') if you need to change it. " +
            G.NL +
            G.NL +
            okhelper +
            "Windows path folders:" + G.NL +
            "----------------------" + G.NL +
            path3;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

            string exeFile = null;

            if (aremos)
            {
                //If we are making an AREMOS-bat file, ask where the AREMOS program folder is
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "Exe file (*.exe)|*.exe|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.Title = "AREMOS exe file: Please indicate location";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((openFileDialog1.OpenFile()) != null)
                        {
                            exeFile = openFileDialog1.FileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Problem with exe file when generating AREMOS bat file. Original error: " + ex.Message);
                        return;
                    }
                }
                else return;
            }


            //Ask where the .bat file should be put
            SaveFileDialog dlgSaveFile = new SaveFileDialog();
            dlgSaveFile.Title = "Bat-file: Please indicate the destination folder";
            dlgSaveFile.InitialDirectory = Environment.GetEnvironmentVariable("SystemRoot");
            dlgSaveFile.Filter = ".bat file (*.bat)|*.bat";
            dlgSaveFile.FileName = programShortcut + ".bat";
            DialogResult dlgResult = dlgSaveFile.ShowDialog();
            string m_strFileName = null;
            if (dlgResult == DialogResult.Cancel)
                return;


            //Write bat-file etc.
            try
            {
                if (aremos)
                {
                    m_strFileName = dlgSaveFile.FileName;
                    using (FileStream fs = Program.WaitForFileStream(m_strFileName, null, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter m_sw = G.GekkoStreamWriter(fs))
                    {
                        m_sw.WriteLine("@echo off");
                        m_sw.WriteLine("start " + Globals.QT + Globals.QT + " " + Globals.QT + Path.GetDirectoryName(Application.ExecutablePath) + "\\WA.exe" + Globals.QT + " " + Globals.QT + Path.GetDirectoryName(exeFile) + Globals.QT);  //changes aremos.opt file
                        //first arg. is name of dos window (irrelevant -- it is not opened), second arg is path to program, third argument is parameters...
                        m_sw.WriteLine("start " + Globals.QT + Globals.QT + " " + Globals.QT + exeFile + Globals.QT);
                        m_sw.WriteLine("exit");
                        m_sw.Close();
                    }
                }
                else
                {
                    m_strFileName = dlgSaveFile.FileName;
                    using (FileStream fs = Program.WaitForFileStream(m_strFileName, null, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter m_sw = G.GekkoStreamWriter(fs))
                    {
                        m_sw.WriteLine("@echo off");
                        //first arg. is name of dos window (irrelevant -- it is not opened), second arg is path to program, third argument is parameters...
                        m_sw.WriteLine("start " + Globals.QT + Globals.QT + " " + Globals.QT + Application.ExecutablePath + Globals.QT);
                        m_sw.WriteLine("exit");
                        m_sw.Close();
                    }
                }
            }
            catch (Exception err)
            {
                using (Error error = new Error())
                {                    
                    error.MainAdd("Problem when trying to write the " + programShortcut + ".bat file. The folder may be read-only. Maybe try to save");
                    error.MainAdd("the " + programShortcut + ".bat to some other folder (for instance your desktop) as as an intermediate step, and ");
                    error.MainAdd("afterwards move it to " + Environment.GetEnvironmentVariable("SystemRoot") + ".");
                    error.MainAdd("If you have administrator rights, this usually works out. Otherwise you will have to ");
                    error.MainAdd("look up the Windows path (it is a so-called 'Environment variable' (Danish: 'miljøvariabel')),");
                    error.MainAdd("and place gekko.bat in one of the folders belongning to the Windows path.");
                    error.ThrowNoException();
                }
                return;
            }
            G.Writeln("The " + programShortcut + ".bat file was successfully created here: " + m_strFileName);
            G.Writeln();

        }
    }
}