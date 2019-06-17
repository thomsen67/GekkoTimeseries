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

using System;
using System.Collections.Generic;
using System.Text;
//using Rtf.Core;
//using Rtf.Util;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.ObjectModel;




namespace Gekko
{
    public class CrossThreadStuff
    {

        //weird delegate pattern, but it works!
        delegate void Decomp2Callback(DecompOptions2 o);
        public static void Decomp2(DecompOptions2 o)
        {
            if (Gui.gui != null && Gui.gui.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new Decomp2Callback(Decomp2), new object[] { o });
            }
            else
            {
                // It's on the same thread, no need for Invoke

                DecompOptions2 decompOptions = (DecompOptions2)o;
                WindowDecomp w = null;
                w = new WindowDecomp(decompOptions);
                Globals.windowsDecomp2.Add(w);

                int count = -1;
                foreach (DecompItemsString decompItem in decompOptions.link)
                {
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    count++;
                    if (count > 0) continue;
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                    if (Program.modelGams != null)
                    {

                        if (decompItem.expression == null)
                        {
                            if (Program.modelGams.equations != null)
                            {
                                ModelGamsEquation found = Program.DecompEvalGams(decompItem.varname);
                                decompItem.expression = Globals.expression;
                                decompItem.expressionText = found.lhs + " = " + found.rhs;
                            }
                            else
                            {
                                G.Writeln2("*** ERROR: No GAMS equations given");
                                throw new GekkoException();
                            }
                        }
                        else
                        {
                            //fix this...
                        }


                    }
                    else
                    {

                        if (Program.model == null)
                        {
                            G.Writeln2("*** ERROR: DECOMP: A model is not loaded, cf. the MODEL command.");
                            throw new GekkoException();
                        }

                        EquationHelper found = Program.DecompEval(decompOptions.variable);
                        decompOptions.expression = Globals.expression;
                        decompOptions.expressionOld = found.equationText;
                    }
                }


                if (decompOptions.name == null)
                {
                    w.Title = "Decompose expression";
                }
                else
                {
                    w.Title = "Decompose " + decompOptions.variable + "";
                }
                w.Tag = decompOptions;

                w.SetRadioButtons();
                w.RecalcCellsWithNewType();
                decompOptions.numberOfRecalcs++;  //signal for Decomp() method to move on

                if (!G.IsUnitTesting())
                {
                    if (w.isClosing)  //if something goes wrong, .isClosing will be true
                    {
                        //The line below removes the window from the global list of active windows.
                        //Without this line, this half-dead window will mess up automatic closing of windows (Window -> Close -> Close all...)
                        if (Globals.windowsDecomp2.Count > 0) Globals.windowsDecomp2.RemoveAt(Globals.windowsDecomp2.Count - 1);
                    }
                    else
                    {
                        w.ShowDialog();
                        w.Close();  //probably superfluous
                        w = null;  //probably superfluous
                    }
                }
                else
                {
                    Globals.windowsDecomp2.Clear();
                    w = null;
                }
            }
        }

        //weird delegate pattern, but it works!
        delegate void SetTextInputCallback(string text, string type2);
        public static void SetTextInput(string text, string type2)
        {
            if (Gui.gui.textBox2.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new SetTextInputCallback(SetTextInput), new object[] { text, type2 });
            }
            else
            {
                // It's on the same thread, no need for Invoke

                RichTextBox textBox = Gui.gui.textBox2;
                string path = "";
                string f1 = Path.GetFileNameWithoutExtension(text);
                string p0 = Program.options.folder_working.TrimEnd('\\');
                string p1 = Path.GetFullPath(Path.GetDirectoryName(text)).TrimEnd('\\');
                string pdif = G.ReplaceString(p1, p0, "", true);
                //p0 may be c:\temp\
                //p1 may be c:\temp\sub
                //----> in that case, the pdif is "\sub".
                string s = "";
                if (pdif.Length > 0 && !pdif.EndsWith("\\")) s = "\\";
                path = pdif + s + f1;
                if (path.Contains(" ") || path.Contains("-")) path = "'" + path + "'";
                int startOld = textBox.SelectionStart;
                textBox.Select(Globals.startOfLinePositionWhenLastEnterPressed, Globals.endOfLinePositionWhenLastEnterPressed - Globals.startOfLinePositionWhenLastEnterPressed);

                string select = textBox.SelectedText;
                int count = select.Length - select.Replace("*", "").Length;

                if (count != 1)
                {
                    textBox.Select(startOld, 0);  //do nothing, too difficult to replace the *'s!
                }
                else
                {
                    //below here, we are certain there is only one '*'
                    string temp = textBox.SelectedText;
                    if (!temp.Contains(" *")) temp = G.ReplaceString(temp, "*", " *", true);  //READ* --> READ *, otherwise the blank will be lacking when the '*' is substituted
                    temp = G.ReplaceString(temp, "*", path, true);
                    int oldLength = textBox.SelectedText.Length;
                    textBox.SelectedText = temp;
                    int dif = temp.Length - oldLength;
                    textBox.Select(startOld + dif, 0);  //a little bit wrong if cursor is left of '*', but typically it is not.
                }

                //doing the same for commandhistory
                string s2 = Globals.commandMemory.storage.ToString();
                string select2Start = s2.Substring(0, Globals.commandMemory.lengthWhenLastEnterPressed);
                string select2 = s2.Substring(Globals.commandMemory.lengthWhenLastEnterPressed);
                int count2 = select2.Length - select2.Replace("*", "").Length;
                if (count2 == 1)
                {
                    if (!select2.Contains(" *")) select2 = G.ReplaceString(select2, "*", " *", true);  //READ* --> READ *, otherwise the blank will be lacking when the '*' is substituted
                    select2 = G.ReplaceString(select2, "*", path, true);
                    Globals.commandMemory.storage = new StringBuilder(select2Start + select2);
                    Globals.commandMemory.lengthWhenLastEnterPressed = Globals.commandMemory.storage.ToString().Length;  //probably superfluous, will be set later on
                }
            }
        }

        //weird delegate pattern, but it works!
        delegate void ZoomCallback();
        public static void Zoom()
        {
            if (G.IsUnitTesting())
            {
                return;
            }
            if (Gui.gui.textBoxTab3.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new ZoomCallback(Zoom), new object[] { });
            }
            else
            {
                Gui.gui.textBox1.Font = new System.Drawing.Font("Courier New", (float)((double)Program.options.interface_zoom / 100d) * 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Gui.gui.textBox2.Font = new System.Drawing.Font("Courier New", (float)((double)Program.options.interface_zoom / 100d) * 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Gui.gui.textBoxTab2.Font = new System.Drawing.Font("Courier New", (float)((double)Program.options.interface_zoom / 100d) * 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Gui.gui.textBoxTab3.Font = new System.Drawing.Font("Courier New", (float)((double)Program.options.interface_zoom / 100d) * 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }

        //weird delegate pattern, but it works!
        delegate void SetReadOnlyCallback(bool b);
        public static void SetReadOnly(bool b)
        {
            if (Gui.gui.textBox1.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new SetReadOnlyCallback(SetReadOnly), new object[] { b });
            }
            else
            {
                Gui.gui.textBox2.ReadOnly = b;
                if (b) Gui.gui.textBox2.BackColor = Color.LightGray;
                else Gui.gui.textBox2.BackColor = Color.White;                
            }
        }

        //weird delegate pattern, but it works!
        delegate void SetTabCallback(string text, bool refreshArrows);
        public static void SetTab(string text, bool refreshArrows)
        {
            if (G.IsUnitTesting()) return;  //ignore the tab shift            
            if (Gui.gui.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new SetTabCallback(SetTab), new object[] { text, refreshArrows });
            }
            else
            {
                if (G.Equal(text, "main"))
                {
                    Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPage1;
                    if (refreshArrows) Gui.gui.GuiBrowseArrowsStuff(null, false, ETabs.Main);
                }
                if (G.Equal(text, "output"))
                {
                    Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPage2;
                    if (refreshArrows) Gui.gui.GuiBrowseArrowsStuff(null, false, ETabs.Output);
                }
                if (G.Equal(text, "help"))
                {
                    Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPage3;
                    if (refreshArrows) Gui.gui.GuiBrowseArrowsStuff(null, false, ETabs.Help);
                }
                if (G.Equal(text, "menu"))
                {
                    Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPage4;
                    if (refreshArrows) Gui.gui.GuiBrowseArrowsStuff(null, false, ETabs.Menu);
                }
            }
        }

        //weird delegate pattern, but it works!
        delegate void ModeCallback();
        public static void Mode()
        {
            if (G.IsUnitTesting())
            {
                return;
            }
            if (Gui.gui.textBox2.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new ModeCallback(Mode), new object[] { });
            }
            else
            {

                if (G.Equal(Program.options.interface_mode, "sim"))
                {
                    //double r = 191; double g = 221; double b = 162; double alpha = 0.00d;
                    //a little more fresh color
                    //double r = 191; double g = 231; double b = 157; double alpha = 0.00d;
                    double r = 191; double g = 234; double b = 154; double alpha = 0.00d;
                    Gui.gui.statusStrip1.BackColor = System.Drawing.Color.FromArgb((int)(r * (1d - alpha) + 255d * alpha), (int)(g * (1d - alpha) + 255d * alpha), (int)(b * (1d - alpha) + 255d * alpha));
                }
                else if (G.Equal(Program.options.interface_mode, "data"))
                {
                    double r = 191; double g = 205; double b = 219; double alpha = 0.00d;                    
                    Gui.gui.statusStrip1.BackColor = System.Drawing.Color.FromArgb((int)(r * (1d - alpha) + 255d * alpha), (int)(g * (1d - alpha) + 255d * alpha), (int)(b * (1d - alpha) + 255d * alpha));
                }
                else if (G.Equal(Program.options.interface_mode, "mixed"))
                {
                    //double r = 255; double g = 232; double b = 166; double alpha = 0.20d;                    
                    //double r = 251; double g = 229; double b = 58; double alpha = 0.60d;
                    double r = 253; double g = 245; double b = 176; double alpha = 0.00d;                    
                    Gui.gui.statusStrip1.BackColor = System.Drawing.Color.FromArgb((int)(r * (1d - alpha) + 255d * alpha), (int)(g * (1d - alpha) + 255d * alpha), (int)(b * (1d - alpha) + 255d * alpha));
                }
                else Gui.gui.textBox2.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                
            }
        }

        //weird delegate pattern, but it works!
        delegate void ClsCallback(string text);
        public static void Cls(string text)
        {
            if (G.IsUnitTesting())
            {
                Globals.unitTestScreenOutput = new StringBuilder();
                //just ignore it: do not clear the stuff
                //Globals.unitTestWindow.Clear();
                //Console.Clear();
                return;
            }
            if (Gui.gui.textBoxTab3.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new ClsCallback(Cls), new object[] { text });
            }
            else
            {
                if (text == "" || text == "main")
                {
                    Gui.gui.textBox1.Clear();
                    Gui.gui.textBox1.AppendText(Globals.blankUsedAsPadding);  //to simulate a previous carriage return with 1 character indent
                    {
                        //To save memory, these links can no longer be clicked.
                        Globals.linkContainerCounter = 0L;
                        Globals.linkContainer = new Dictionary<long, Program.LinkContainer>();
                        Globals.outputTabTextCounter = 0L;
                        Globals.outputTabTextContainer = new Dictionary<string, Program.ErrorContainer>(StringComparer.OrdinalIgnoreCase);
                    }
                    //G.Writeln("Output window cleared");
                }
                else if (text == "output")
                {
                    Gui.gui.textBoxTab2.Clear();
                    Gui.gui.textBoxTab2.AppendText(Globals.blankUsedAsPadding);
                }
                else if (text == "help")
                {
                    Gui.gui.textBoxTab3.Clear();
                    Gui.gui.textBoxTab3.AppendText(Globals.blankUsedAsPadding);
                }
            }
        }


        //weird delegate pattern, but it works!
        delegate void WorkingFolderCallback(string text);
        public static void WorkingFolder(string text)  //called only from dynamic code
        {
            if (!G.IsUnitTesting() && Gui.gui.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new WorkingFolderCallback(WorkingFolder), new object[] { text });
            }
            else
            {
                Gui.ChangeWorkingFolder(Program.options.folder_working);
            }
        }

        //weird delegate pattern, but it works!
        delegate void StackTraceCallback(ObservableCollection<RunStatusData> list);
        public static void StackTrace(ObservableCollection<RunStatusData> list)
        {
            try
            {
                if (Gui.gui.InvokeRequired)
                {
                    // It's on a different thread, so use Invoke.
                    Gui.gui.Invoke(new StackTraceCallback(StackTrace), new object[] { list });
                }
                else
                {
                    Globals.windowRunStatus.listView.ItemsSource = list;
                }
            }
            catch { };  //fail silently
        }

        //weird delegate pattern, but it works!
        delegate void RunStatusCallback(ObservableCollection<RunStatusData> list);
        public static void RunStatus(ObservableCollection<RunStatusData> list)
        {
            try
            {
                if (Gui.gui.InvokeRequired)
                {
                    // It's on a different thread, so use Invoke.
                    Gui.gui.Invoke(new RunStatusCallback(RunStatus), new object[] { list });
                }
                else
                {
                    Globals.windowRunStatus.listView2.ItemsSource = list;
                }
            }
            catch { };  //fail silently
        }


        delegate void IntellisenseCallback();
        public static void Intellisense()
        {
            if (Gui.gui.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new IntellisenseCallback(Intellisense), new object[] { });
            }
            else
            {
                if (Globals.windowIntellisense.lastSelected.Trim().StartsWith("[") && Globals.windowIntellisense.lastSelected.Trim().EndsWith("]"))
                {
                    //ignore stuff like [variable]
                }
                else
                {
                    Gui.gui.textBox2.SelectedText = Globals.windowIntellisense.lastSelected;
                }
                Globals.windowIntellisense.IsOpen = false;
            }
        }

        
        delegate void ShowPeriodInStatusFieldCallback();
        public static void ShowPeriodInStatusField()
        {
            if (Gui.gui.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new ShowPeriodInStatusFieldCallback(ShowPeriodInStatusField), new object[] { });
            }
            else
            {
                Program.ShowPeriodInStatusField("");
            }
        }

        //weird delegate pattern, but it works!
        delegate void CloseGraphCallback(Graph g);
        public static void CloseGraph(Graph g)
        {
            try
            {
                if (g.InvokeRequired)
                {
                    // It's on a different thread, so use Invoke.
                    g.Invoke(new CloseGraphCallback(CloseGraph), new object[] { g });
                }
                else
                {
                    Globals.ch.windowsGraphCloseCounter++;
                    g.Invoke(new CloseDelegate(g.Close));  //Why not just g.Close() ??
                }
            }
            catch {  };  //fail silently
        }

        //weird delegate pattern, but it works!
        delegate void CloseDecompCallback(Window1 w);
        public static void CloseDecomp(Window1 w)
        {
            try
            {
                if (!w.Dispatcher.CheckAccess())
                {
                    // It's on a different thread, so use Invoke.
                    w.Dispatcher.Invoke(new CloseDecompCallback(CloseDecomp), new object[] { w });
                }
                else
                {
                    //w.Close();
                    Globals.ch.windowsDecompCloseCounter++;
                    w.Dispatcher.Invoke(new CloseDelegate(w.Close));  //Why not just w.Close() ??                    
                }
            }
            catch { };  //fail silently
        }

        //weird delegate pattern, but it works!
        delegate void CloseDecompCallback2(WindowDecomp w);
        public static void CloseDecomp2(WindowDecomp w)
        {
            try
            {
                if (!w.Dispatcher.CheckAccess())
                {
                    // It's on a different thread, so use Invoke.
                    w.Dispatcher.Invoke(new CloseDecompCallback2(CloseDecomp2), new object[] { w });
                }
                else
                {
                    //w.Close();
                    Globals.ch.windowsDecompCloseCounter++;
                    w.Dispatcher.Invoke(new CloseDelegate(w.Close));  //Why not just w.Close() ??                    
                }
            }
            catch { };  //fail silently
        }

        //weird delegate pattern, but it works!
        delegate void UpdateGraphCallback(Graph g);
        public static void UpdateGraph(Graph g)
        {
            try
            {
                if (g.InvokeRequired)
                {
                    // It's on a different thread, so use Invoke.
                    g.Invoke(new UpdateGraphCallback(UpdateGraph), new object[] { g });
                }
                else
                {
                    Globals.ch.windowsGraphUpdateCounter++;
                    g.UpdateGraph();
                }
            }
            catch
            {
                Globals.ch.windowsGraphUpdateFailedCounter++;
            }
        }

        //weird delegate pattern, but it works!
        delegate void UpdateDecompCallback(Window1 w);
        public static void UpdateDecomp(Window1 w)
        {
            try
            {
                if (!w.Dispatcher.CheckAccess())
                {
                    // It's on a different thread, so use Invoke.
                    w.Dispatcher.Invoke(new UpdateDecompCallback(UpdateDecomp), new object[] { w });
                }
                else
                {
                    Globals.ch.windowsDecompUpdateCounter++;
                    w.UpdateDecomp();
                }
            }
            catch
            {
                Globals.ch.windowsDecompUpdateFailedCounter++;
            }
        }



        //weird delegate pattern, but it works!
        delegate void CopyButtonCallbackEnabled(bool status);
        public static void CopyButtonEnabled(bool status)
        {
            if (G.IsUnitTesting()) return;
            if (Gui.gui.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new CopyButtonCallbackEnabled(CopyButtonEnabled), new object[] { status });
            }
            else
            {
                Gui.gui.toolStripButton4.Enabled = status;
            }
        }

        //weird delegate pattern, but it works!
        delegate void CutButtonCallbackEnabled(bool status);
        public static void CutButtonEnabled(bool status)
        {
            if (G.IsUnitTesting()) return;
            if (Gui.gui.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new CutButtonCallbackEnabled(CutButtonEnabled), new object[] { status });
            }
            else
            {
                Gui.gui.toolStripButton6.Enabled = status;
            }
        }

        //weird delegate pattern, but it works!
        delegate void BlinkCallback();
        public static void Blink()
        {
            if (G.IsUnitTesting()) return;
            if (Gui.gui.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new BlinkCallback(Blink), new object[] { });
            }
            else
            {
                if (Globals.guiTimerCounter % 4 == 0) Gui.gui.toolStripStatusLabel3a.Text = " ";
                if (Globals.guiTimerCounter % 4 == 3) Gui.gui.toolStripStatusLabel3a.Text = "+";
                Globals.guiTimerCounter++;                
            }
        }

        //weird delegate pattern, but it works!
        delegate void PulseCallback();
        public static void Pulse()
        {
            if (G.IsUnitTesting()) return;
            if (Gui.gui.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new PulseCallback(Pulse), new object[] { });
            }
            else
            {
                //The guiTimer2 runs all the time from when the GUI starts
                //remote.gcm should be run in two cases:
                // 1. If it already exists and has just been changed
                // 2. If it pops into existence 
                //It should not be run just because af RESET/RESTART or change of working folder location.

                //At start, Globals.remoteFileStamp is = new DateTime(0l), kind of like = null.
                
                if (Program.options.interface_remote && Globals.remoteIsInvestigating == false)
                {
                    bool abort = false;
                    Globals.remoteIsInvestigating = true;
                    try
                    {
                        string remoteFile = Program.options.folder_working + "\\remote.gcm";

                        if (Program.options.interface_remote_file != "")
                        {
                            if (G.NullOrEmpty(Path.GetFileName(Program.options.interface_remote_file)))
                            {
                                //#89743298473
                                G.Writeln2("*** ERROR: You must indicate a filename in OPTION interface remote file = ...");
                                abort = true;
                            }
                            remoteFile = Program.options.interface_remote_file;
                        }

                        //see if there is new stuff from the remote
                        if (File.Exists(remoteFile))
                        {
                            DateTime dt = File.GetLastWriteTime(remoteFile);
                            if (Globals.remoteExists == 0)
                            {
                                Globals.remoteExists = 1;
                                //suddently pops into existence, then it MUST be run no matter stamps
                                Globals.remoteFileStamp = dt;
                                RunRemoteFile(remoteFile);

                            }
                            else {

                                Globals.remoteExists = 1;
                                                                
                                if (Globals.remoteFileStamp.Ticks != 0l && dt.CompareTo(Globals.remoteFileStamp) != 0)
                                {
                                    //run it
                                    Globals.remoteFileStamp = dt;
                                    //Program.Run(remoteFile, new P());
                                    RunRemoteFile(remoteFile);                                    
                                }
                                else
                                {
                                    Globals.remoteFileStamp = dt;
                                }
                            }
                        }
                        else
                        {
                            //do nothing
                            //Globals.remoteFileStamp = new DateTime(0l);
                            Globals.remoteExists = 0;
                        }
                    }
                    catch
                    {

                    } //do not let an error crash the whole thing.
                    finally
                    {
                        Globals.remoteIsInvestigating = false;  //make sure it is false on exit no matter what happens                        
                    }
                    if (abort)
                    {
                        Globals.guiTimer2.Stop();  //without this, we get infinite many error messages, #89743298473
                        throw new GekkoException();
                    }
                }
                Program.ShowPeriodInStatusField("");
            }
        }

        private static void RunRemoteFile(string remoteFile)
        {
            string s = Program.GetTextFromFileWithWait(remoteFile);
            s = s.Trim();
            //int i = G.CountLines(s);
            //if (i == 1 && s.EndsWith(G.NL)) s = s.Substring(0, s.Length - G.NL.Length);
            Gui.gui.StartThread(s, true);
            //Gui.gui.StartThread("RUN " + remoteFile + "; ", true);
        }

        //weird delegate pattern, but it works!
        delegate void RestartMenuBrowserCallback();
        public static void RestartMenuBrowser()
        {
            if (G.IsUnitTesting()) return;
            if (Gui.gui.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new RestartMenuBrowserCallback(RestartMenuBrowser), new object[] {});
            }
            else
            {
                Gui.gui.RestartMenuBrowser();
            }
        }

        //weird delegate pattern, but it works!
        delegate void RefreshArrowsEtcCallback();
        public static void RefreshArrowsEtc()
        {
            if (G.IsUnitTesting()) return;
            if (Gui.gui.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                Gui.gui.Invoke(new RefreshArrowsEtcCallback(RefreshArrowsEtc), new object[] { });
            }
            else
            {
                if (Gui.gui.tabControl1.SelectedIndex == 0)  //main
                {
                    if (Globals.guiHomeMainEnabled) Gui.gui.toolStripButton5.Enabled = true;
                    else Gui.gui.toolStripButton5.Enabled = false;
                    if (Program.guiBrowseNumber >= 2) Gui.gui.toolStripButton1.Enabled = true;
                    else Gui.gui.toolStripButton1.Enabled = false;
                    if (Program.guiBrowseNumber < Program.guiBrowseHistory.Count) Gui.gui.toolStripButton2.Enabled = true;
                    else Gui.gui.toolStripButton2.Enabled = false;
                }
                else if (Gui.gui.tabControl1.SelectedIndex == 1)  //output
                {
                    Gui.gui.toolStripButton5.Enabled = false;
                    Gui.gui.toolStripButton1.Enabled = false;
                    Gui.gui.toolStripButton2.Enabled = false;
                }
                else if (Gui.gui.tabControl1.SelectedIndex == 2)  //menu
                {
                    bool isStart = false;
                    string s = Program.AddExtension(Program.options.menu_startfile, ".html");
                    if (Gui.gui.webBrowser.Url != null && (Gui.gui.webBrowser.Url.AbsoluteUri.Contains("/" + s) || Gui.gui.webBrowser.Url.AbsoluteUri.Contains("\\" + s)))
                    {
                        //ends with "/menu.html" or "\menu.html" (the latter will probably not occur, so just for safety)
                        isStart = true;
                    }
                    if (isStart) Gui.gui.toolStripButton5.Enabled = false;
                    else Gui.gui.toolStripButton5.Enabled = true;
                    if (Gekko.Gui.gui.webBrowser.CanGoBack && isStart == false) Gui.gui.toolStripButton1.Enabled = true;
                    else Gui.gui.toolStripButton1.Enabled = false;
                    if (Gekko.Gui.gui.webBrowser.CanGoForward) Gui.gui.toolStripButton2.Enabled = true;
                    else Gui.gui.toolStripButton2.Enabled = false;
                }
            }
        }
    }
}
