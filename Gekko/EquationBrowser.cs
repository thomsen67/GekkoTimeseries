using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Gekko
{
    /// <summary>
    /// Used to store info on how label, unit, etc. is shown in EquationBrower (html). In this classe so not to pollute anything...
    /// </summary>
    public class HtmlBrowserSettings
    {
        public bool isDanish = true;
        public bool show_source = false;
    }

    public class EquationBrowserHelper
    {
        public string s1;
        public string s2;
    }

    public static class EquationBrowser
    {
        public static void Browser()
        {
            bool jsmFix = true;

            G.Writeln2("Starting html browser generation");
            DateTime dt0 = DateTime.Now;

            string pathAndFile = Program.options.folder_working + "\\" + "browser.json";

            string jsonCode = G.RemoveComments(Program.GetTextFromFileWithWait(pathAndFile));
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> jsonTree = null;
            try
            {
                jsonTree = (Dictionary<string, object>)serializer.DeserializeObject(jsonCode);
            }
            catch (Exception e)
            {
                G.Warning("w4.1", "The .json file does not seem correctly formatted. " + e.Message);
            }

            // -------------------------------------------------------------

            string settings_index_filename = null;
            try { settings_index_filename = (string)jsonTree["index_filename"]; } catch { }
            if (settings_index_filename == null)
            {
                new Error("JSON: index_filename not found");
            }

            string settings_list_filename = null;
            try { settings_list_filename = (string)jsonTree["list_filename"]; } catch { }
            if (settings_list_filename == null)
            {
                new Error("JSON: list_filename not found");
            }

            string settings_find_filename = null;
            try { settings_find_filename = (string)jsonTree["find_filename"]; } catch { }
            if (settings_find_filename == null)
            {
                new Error("Find_filename not found");
            }

            string settings_css_filename = null;
            try { settings_css_filename = (string)jsonTree["css_filename"]; } catch { }
            if (settings_css_filename == null)
            {
                new Error("JSON: css_filename not found");
            }

            string settings_dok_filename = null;
            try { settings_dok_filename = (string)jsonTree["dok_filename"]; } catch { }
            if (settings_dok_filename == null)
            {
                new Error("JSON: dok_filename not found");
            }

            string settings_est_filename = null;
            try { settings_est_filename = (string)jsonTree["est_filename"]; } catch { }
            if (settings_est_filename == null)
            {
                new Error("JSON: est_filename not found");
            }

            string settings_icon_filename = null;
            try { settings_icon_filename = (string)jsonTree["icon_filename"]; } catch { }
            if (settings_icon_filename == null)
            {
                new Error("JSON: icon_filename not found");
            }

            string settings_vars_foldername = null;
            try { settings_vars_foldername = (string)jsonTree["vars_foldername"]; } catch { }
            if (settings_vars_foldername == null)
            {
                new Error("JSON: vars_foldername not found");
            }

            string settings_commands = null;
            try { settings_commands = (string)jsonTree["commands"]; } catch { }
            if (settings_commands == null)
            {
                new Error("JSON: commands not found");
            }

            string settings_plot_start = null;
            try { settings_plot_start = (string)jsonTree["plot_start"]; } catch { }
            if (settings_plot_start == null)
            {
                new Error("JSON: plot_start not found");
            }

            string settings_plot_end = null;
            try { settings_plot_end = (string)jsonTree["plot_end"]; } catch { }
            if (settings_plot_end == null)
            {
                new Error("JSON: plot_end not found");
            }

            string settings_plot_line = null;
            try { settings_plot_line = (string)jsonTree["plot_line"]; } catch { }
            if (settings_plot_line == null)
            {
                new Error("Plot_line not found");
            }

            string settings_print_start = null;
            try { settings_print_start = (string)jsonTree["print_start"]; } catch { }
            if (settings_print_start == null)
            {
                new Error("Print_start not found");
            }

            string settings_print_end = null;
            try { settings_print_end = (string)jsonTree["print_end"]; } catch { }
            if (settings_print_end == null)
            {
                new Error("Print_end not found");
            }

            string include_p_type = null;
            try { include_p_type = (string)jsonTree["include_p_type"]; } catch { }
            if (include_p_type == null)
            {
                new Error("Include_p_type");
            }

            bool settings_show_source = true;
            try { settings_show_source = (bool)jsonTree["show_source"]; } catch { }

            object[] settings_ekstrafiler = null;
            try { settings_ekstrafiler = (object[])jsonTree["ekstrafiler"]; } catch { }
            if (settings_ekstrafiler == null)
            {
                new Error("JSON: ekstrafiler problem");
            }

            // -------------------------------------------------------------

            string list_title = "Variabelliste. Søg i browseren med Ctrl + F(find)";

            string browserFolder = "browser";

            List<string> files = new List<string>();
            files.Add(settings_index_filename);
            files.Add(settings_find_filename);
            files.Add(settings_list_filename);
            files.Add(settings_css_filename);
            files.Add(settings_dok_filename);
            files.Add(settings_est_filename);
            files.Add(settings_icon_filename);
            files.Add(browserFolder);
            files.Add(settings_vars_foldername);
            foreach (string file in files)
            {
                if (file == null) continue;
                if (file.Contains("/") || file.Contains("\\"))
                {
                    new Error("'" + file + "' should not contain '/' or '\\'");
                }
            }

            string rootFolder = Program.options.folder_working + "\\" + browserFolder;
            string subFolder = Program.options.folder_working + "\\" + browserFolder + "\\" + settings_vars_foldername;

            BrowserCleanupFolders(rootFolder, subFolder);

            //index.html and styles.css is copied to root folder of browser system
            List<string> filesToCopy = new List<string>();
            filesToCopy.Add(settings_index_filename);
            filesToCopy.Add(settings_css_filename);
            filesToCopy.Add(settings_icon_filename);
            foreach (object o in settings_ekstrafiler)
            {
                string s = null;
                try
                {
                    s = (string)o;
                }
                catch (Exception e)
                {
                    new Error("JSON: ekstrafiler problem");
                }
                if (s != null) filesToCopy.Add(s);
            }

            foreach (string fileToCopy in filesToCopy)
            {
                string fileNameIndex = Program.options.folder_working + "\\" + fileToCopy;
                string fileNameIndex2 = rootFolder + "\\" + fileToCopy;
                if (!File.Exists(fileNameIndex))
                {
                    new Error("'" + fileNameIndex + "' was not found");
                }
                File.Copy(fileNameIndex, fileNameIndex2, true);
            }

            Program.RunGekkoCommands(settings_commands, "", 0, new P());

            int gap = 20;

            GekkoTime plotStart = new GekkoTime(EFreq.A, G.IntParse(settings_plot_start), 1);
            GekkoTime plotEnd = new GekkoTime(EFreq.A, G.IntParse(settings_plot_end), 1);
            GekkoTime plot_line = new GekkoTime(EFreq.A, G.IntParse(settings_plot_line), 1);
            GekkoTime print_start = new GekkoTime(EFreq.A, G.IntParse(settings_print_start), 1);
            GekkoTime print_end = new GekkoTime(EFreq.A, G.IntParse(settings_print_end), 1);

            string bank1 = Path.GetFileName(Program.databanks.GetFirst().FileNameWithPathPretty);
            string bank2 = Path.GetFileName(Program.databanks.GetRef().FileNameWithPathPretty);

            List ml = O.GetIVariableFromString("#all", O.ECreatePossibilities.NoneReportError, true) as List;
            List<string> vars = Stringlist.GetListOfStringsFromIVariable(ml);

            if (G.Equal(include_p_type, "yes"))
            {
                GekkoDictionary<string, string> temp = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (string s in vars) temp.Add(s, null);
                foreach (EquationHelper eh in Program.model.modelGekko.equationsNotRunAtAll)
                {
                    if (eh.equationType != EEquationType.RevertedP) continue;
                    if (!temp.ContainsKey(eh.lhs)) temp.Add(eh.lhs, null);
                    foreach (string s12 in eh.precedentsWithLagIndicator.Keys)
                    {
                        string s13 = G.ExtractOnlyVariableIgnoreLag(s12);
                        if (!temp.ContainsKey(s13)) temp.Add(s13, null);
                    }
                }
                vars.Clear();
                foreach (string s14 in temp.Keys) vars.Add(s14);
            }

            if (Globals.browserLimit)
            {
                if (settings_index_filename.ToLower().Contains("mona"))
                {
                    vars = new List<string> { "FY", "FCB", "PCB_LA", "FCH", "PCH_LA", "FCQ", "PCQ_LA", "PCOV_LA", "FCOV", "PCOW_LA", "FCOW", "PIOV_LA", "FIOV", "FIPMXE", "PIPMXE_LA", "FIY", "PIY_LA", "FIEM", "PIEM_LA", "FIH", "PIH_LA", "FMY", "PMY_LA", "PY_LA" };
                }
                else if (settings_index_filename.ToLower().Contains("adam"))
                {
                    vars = new List<string> { "fy", "ul", "pcp", "tg" };
                }
                else
                {
                    //smec
                    vars = new List<string> { "aaa", "fcp", "PHK", "jphk", "fee", "Jfee", "fy", "tg", "peesq", "ktiorn", "tfon", "phk2", "phk3", "JNTPPIK" };  //phk2 is t-type, phk3 is p-type and JNTPPIK is y-type. The y-type is not shown
                }
            }
            else if (Globals.runningOnTTComputer)
            {
                DialogResult result = MessageBox.Show("Only a few vars?", "Vars", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes)
                {
                    vars = new List<string> { "aaa", "fcp", "PHK", "jphk", "fee", "Jfee", "fy", "tg", "peesq", "ktiorn", "tfon" };
                }
            }

            vars.Sort(StringComparer.OrdinalIgnoreCase);

            // -------------------------------------------
            // Data generation
            // -------------------------------------------

            GekkoDictionary<string, List<string>> datagen = BrowserDataGenerationExtract();

            // -------------------------------------------
            // Html
            // -------------------------------------------

            //Fetches info on external documents that contain read-more info on particular variables
            GekkoDictionary<string, List<Tuple<string, string>>> doc = new GekkoDictionary<string, List<Tuple<string, string>>>(StringComparer.OrdinalIgnoreCase);
            string dokFileName = Program.options.folder_working + "\\" + settings_dok_filename;
            string dok2 = Program.GetTextFromFileWithWait(dokFileName);
            List<string> dok = Stringlist.ExtractLinesFromText(dok2);
            for (int i = 0; i < dok.Count; i++)
            {
                string line = dok[i].Trim();
                if (line.StartsWith("!")) continue;
                string[] ss = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length < 3) continue;
                string varname = ss[0];
                string path = ss[1];
                string descr = null;
                for (int ii = 2; ii < ss.Length; ii++)
                {
                    descr += ss[ii] + " ";
                }
                if (!doc.ContainsKey(varname))
                {
                    List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();
                    doc.Add(varname, tuples);
                }
                doc[varname].Add(new Tuple<string, string>(path, descr));
            }

            List<EquationBrowserHelper> vars2 = new List<EquationBrowserHelper>();                        

            //Fetches estimation output
            GekkoDictionary<string, List<string>> est2 = new GekkoDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            string est = Program.GetTextFromFileWithWait(Program.options.folder_working + "\\" + settings_est_filename);
            List<string> lines = Stringlist.ExtractLinesFromText(est);

            for (int i = 0; i < lines.Count; i++)
            {
                //must be first
                if (lines[i].Trim().StartsWith(Globals.ols1))
                {
                    int fat = 5;
                    var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
                    var tags2 = new List<string>() { "//" };
                    string depLine = lines[i + 1].Trim();
                    depLine = depLine.Replace(Globals.ols2, "").Trim();
                    List<TokenHelper> a = StringTokenizer.GetTokensWithLeftBlanks(depLine, fat, tags1, tags2, null, null).storage;
                    string varLine = BrowserGetVariable(a);

                    List<string> olsLines = new List<string>();
                    for (int j = i; j < lines.Count; j++)
                    {
                        olsLines.Add(lines[j]);
                        if (lines[j].Contains(Globals.ols3a) && lines[j].Contains(Globals.ols3b) && lines[j].Contains(Globals.ols3c))
                        {
                            if (est2.ContainsKey(varLine))
                            {
                                List<string> lines2 = est2[varLine];
                                lines2.Add("");
                                lines2.AddRange(olsLines);
                            }
                            else
                            {
                                est2.Add(varLine, olsLines);
                            }

                            i = j;  //then i will start at j+1 next time
                            break;
                        }
                    }
                }
            }


            string modelFrequencyString = GetModelFreq(vars);
            Program.options.freq = G.ConvertFreq(modelFrequencyString); //sets global freq

            if (Globals.browserLimit)
            {
                if (settings_index_filename.ToLower().Contains("mona"))
                {
                }
                else if (settings_index_filename.ToLower().Contains("adam"))
                {
                }
                else
                {
                    //smec
                    //Program.databanks.GetRef().RemoveIVariable("tfon!a");
                    Program.databanks.GetFirst().RemoveIVariable("tfon!a");
                }
            }

            int missingFirst = 0;
            int missingRef = 0;

            foreach (string varnameWithoutFreq in vars)
            {
                string varnameWithFreq = varnameWithoutFreq + "!" + modelFrequencyString;
                StringBuilder sb = new StringBuilder();

                Series ts1 = Program.databanks.GetFirst().GetIVariable(varnameWithFreq) as Series;
                if (ts1 == null) missingFirst++;               

                if (ts1 != null && Globals.browserLimit)
                {
                    if (settings_index_filename.ToLower().Contains("mona"))
                    {
                    }
                    else if (settings_index_filename.ToLower().Contains("adam"))
                    {
                    }
                    else
                    {
                        //smec
                        if (G.Equal(ts1.GetName(), "ktiorn!a"))
                        {
                            //Test that a null-label is ok (ktiorn is also removed from varlist.dat)
                            ts1.meta.label = null;
                            ts1.meta.source = null;
                            ts1.meta.units = null;
                        }
                    }
                }

                Series ts2 = Program.databanks.GetRef().GetIVariable(varnameWithFreq) as Series;
                if (ts2 == null) missingRef++;

                string jName = null;  //name of possible j-led
                bool jNameAutoGen = false;

                sb.AppendLine("<table cellpadding = `0` cellspacing = `0` width = `800px` border = `0`>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td width = `80%`><big><b> " + varnameWithoutFreq + "</b></big></td>");
                sb.AppendLine("<td width = `10%`><a href=`..\\" + settings_find_filename + "`>Søg</a></td>");
                sb.AppendLine("<td width = `10%`><a href=`..\\" + settings_index_filename + "`>Hjem</a></td>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</table>");

                // --------------------------------
                // html print green explanations here. Includes name, label, source, units -- and may also include raw lines from external varlist.dat file (they are shown first, if present)
                // --------------------------------

                HtmlBrowserSettings htmlBrowserSettings = new HtmlBrowserSettings();
                htmlBrowserSettings.isDanish = true;
                htmlBrowserSettings.show_source = settings_show_source;
                List<string> varExpl = Program.GetVariableExplanationAugmented(varnameWithFreq, htmlBrowserSettings);
                foreach (string line in varExpl)
                {
                    if (line != "")
                    {
                        WriteHtmlColor(sb, Program.SpecialXmlChars(line));
                    }
                }

                // --------------------------------
                // stash explanations for later use in JavaScript find component
                // --------------------------------

                string explanation = null;
                if (varExpl != null && varExpl.Count > 0)
                {
                    foreach (string varExpl2 in varExpl)
                    {
                        if (varExpl2.Trim().StartsWith("Series: " + varnameWithoutFreq, StringComparison.OrdinalIgnoreCase)) continue;  //not interesting here
                        explanation += G.HandleQuoteInQuote(varExpl2, true) + ". ";  //see also #324lkj2342
                    }
                }
                EquationBrowserHelper ebh = new EquationBrowserHelper();
                ebh.s1 = varnameWithoutFreq; 
                if (ebh.s1 != null) ebh.s1 = ebh.s1.Replace("`", "'"); //We use ` to represent "
                ebh.s2 = G.ReplaceWhitespaceWith1Blank(explanation);
                if (ebh.s2 != null) ebh.s2 = ebh.s2.Replace("`", "'"); //We use ` to represent "
                vars2.Add(ebh);

                // --------------------------------
                // html print info on ENDO/EXO, freq, data period
                // --------------------------------

                EEndoOrExo type1 = Program.VariableTypeEndoExo(varnameWithFreq);
                string type = "";
                if (type1 == EEndoOrExo.Exo) type = "Eksogen";
                else if (type1 == EEndoOrExo.Endo) type = "Endogen";

                //========================================================================================================
                //                          FREQUENCY LOCATION, indicates where to implement more frequencies
                //========================================================================================================

                if (ts1 != null)                
                { 

                    string freq = "[ukendt frekvens]";
                    if (ts1.freq == EFreq.A)
                    {
                        freq = "Årlig";
                    }
                    else if (ts1.freq == EFreq.Q)
                    {
                        freq = "Kvartalsvis";
                    }
                    else if (ts1.freq == EFreq.M)
                    {
                        freq = "Månedlig";
                    }
                    else if (ts1.freq == EFreq.W)
                    {
                        freq = "Ugentlig";
                    }
                    else if (ts1.freq == EFreq.D)
                    {
                        freq = "Daglig";
                    }
                    else if (ts1.freq == EFreq.U)
                    {
                        freq = "Udateret";
                    }

                    bool noData = ts1.IsNullPeriod(); //We are opening up to this possibility of 'empty' data

                    GekkoTime first = ts1.GetRealDataPeriodFirst();
                    GekkoTime last = ts1.GetRealDataPeriodLast();

                    StringBuilder sb4 = new StringBuilder();
                    sb4.Append(type + ", ");
                    string stamp = null;
                    if (ts1.meta.stamp != null && ts1.meta.stamp != "") stamp = " (opdateret: " + ts1.meta.stamp + ")";
                    if (ts1.freq == EFreq.A || ts1.freq == EFreq.U)
                    {
                        if (noData || first.super == -12345 || last.super == -12345)
                        {
                            sb4.Append(freq + ", ingen dataperiode");
                        }
                        else
                        {
                            //we don't want 1995a1 to 2005a1, instead 1995 to 2005
                            sb4.Append(freq + " data fra " + first.super + " til " + last.super + stamp);
                        }
                    }
                    else
                    {
                        if (noData || first.super == -12345 || last.super == -12345)
                        {
                            sb4.Append(freq + ", ingen dataperiode");
                        }
                        else
                        {
                            sb4.Append(freq + " data fra " + first.super + ts1.freq.ToString() + first.sub + " til " + last.super + ts1.freq.ToString() + last.sub + stamp);
                        }
                    }
                    WriteHtml(sb, sb4.ToString());  //for instance: Endogen: Årlige data fra 1966 til 2030 (opdateret: 23-09-2021)
                }
                else
                {
                    WriteHtml(sb, type);
                }

                // --------------------------------
                // print link(s) to possible external documentation files
                // --------------------------------

                List<Tuple<string, string>> tuples = null; doc.TryGetValue(varnameWithoutFreq, out tuples);
                if (tuples != null)
                {
                    int counter = -1;
                    sb.Append("<table style=`margin: 0px; padding: 0px; border: 0px; width: 800px;`>");
                    foreach (Tuple<string, string> tuple in tuples)
                    {
                        counter++;
                        string s = null;
                        if (counter == 0) s = "Dokumentation:&nbsp;&nbsp;";
                        sb.Append("<tr>");
                        sb.Append("<td>" + s + "</td>");
                        sb.Append("<td><a href = `" + tuple.Item1 + "`>" + tuple.Item2 + "</a></td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                }

                // --------------------------------
                // html print dependents etc.
                // --------------------------------

                BrowserDependents(varnameWithFreq, sb, ref jName, ref jNameAutoGen);

                // --------------------------------
                // html print estimation output etc.
                // --------------------------------

                string xxx = null;
                if (est2.ContainsKey(varnameWithoutFreq))
                {
                    List<string> xx = est2[varnameWithoutFreq];
                    foreach (string s in xx)
                    {
                        xxx += s + G.NL;
                    }
                }
                if (xxx != null)
                {
                    FoldingButtonStart(sb, "Estimationsoutput");
                    WriteHtmlPreCode(sb, xxx);
                    FoldingButtonEnd(sb);
                }

                // --------------------------------
                // html print data generation info
                // --------------------------------

                List<string> datagen2 = null; datagen.TryGetValue(varnameWithoutFreq, out datagen2);
                if (datagen2 != null)
                {
                    WriteHtml(sb, "Datagenerering:");
                    string s5 = null;
                    foreach (string s in datagen2)
                    {
                        s5 += s + G.NL;
                    }
                    WriteHtmlPreCode(sb, s5);
                }                

                bool hasFilter = false; if (Program.options.timefilter && Globals.globalPeriodTimeFilters2.Count > 0) hasFilter = true;

                int max = Program.options.print_disp_maxlines;
                if (hasFilter || Program.options.print_disp_maxlines == -1) max = int.MaxValue;

                // --------------------------------
                // make plots
                // --------------------------------

                string l1 = bank1.ToLower().Replace(".gbk", "") + ":" + varnameWithoutFreq;
                string l2 = bank2.ToLower().Replace(".gbk", "") + ":" + varnameWithoutFreq;

                if (ts1 != null)
                {
                    if (ts2 == null)
                    {
                        //only plot the series from Work
                        Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " > " + varnameWithoutFreq + " '" + l1 + "' file=" + subFolder + "\\" + varnameWithoutFreq.ToLower() + ".svg;", "", 0, new P());
                        Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " yminhard = -100 ymaxhard = 100 yminsoft = -1 ymaxsoft = 1  p> " + varnameWithoutFreq + " '" + l1 + "' file=" + subFolder + "\\" + varnameWithoutFreq.ToLower() + "___p" + ".svg;", "", 0, new P());
                    }
                    else
                    {
                        //plot both
                        Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " > @" + varnameWithoutFreq + " '" + l2 + "' <type = lines dashtype = '3'>, " + varnameWithoutFreq + " '" + l1 + "' file=" + subFolder + "\\" + varnameWithoutFreq.ToLower() + ".svg;", "", 0, new P());
                        Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " yminhard = -100 ymaxhard = 100 yminsoft = -1 ymaxsoft = 1  p> @" + varnameWithoutFreq + " '" + l2 + "' <type = lines dashtype = '3'>, " + varnameWithoutFreq + " '" + l1 + "' file=" + subFolder + "\\" + varnameWithoutFreq.ToLower() + "___p" + ".svg;", "", 0, new P());
                    }

                    sb.AppendLine("<img src = `" + varnameWithoutFreq.ToLower() + ".svg" + "`>");

                    sb.AppendLine("</p>");

                    FoldingButtonStart(sb, "Vækst %");
                    sb.AppendLine("<img src = `" + varnameWithoutFreq.ToLower() + "___p.svg" + "`>");
                    FoldingButtonEnd(sb);

                    if (jName != null)
                    {
                        FoldingButtonStart(sb, "J-led");
                        sb.AppendLine("<img src = `" + jName.ToLower() + ".svg" + "`>");
                        FoldingButtonEnd(sb);
                    }

                    // --------------------------------
                    // html print data values of series
                    // --------------------------------

                    StringBuilder sb3 = new StringBuilder();
                    string extra = ""; if (modelFrequencyString != "a") extra = "  ";  //for instance, 2020q3 is 6 chars, 2020 is only 4. Will not work good for months...
                    sb3.AppendLine(bank1 + G.Blanks(30 - bank1.Length + gap) + extra + bank2);
                    sb3.AppendLine();
                    sb3.AppendLine("Period" + extra + "        value        %  " + G.Blanks(gap) + "Period" + extra + "        value        %  ");
                    int counter6 = 0;
                    foreach (GekkoTime gt in new GekkoTimeIterator(GekkoTime.ConvertFreqsFirst(G.ConvertFreq(modelFrequencyString), print_start, null), GekkoTime.ConvertFreqsLast(G.ConvertFreq(modelFrequencyString), print_end)))
                    {
                        counter6++;
                        if (hasFilter)  //some periods are set via TIMEFILTER
                        {
                            if (Program.ShouldFilterPeriod(gt)) continue;
                        }

                        int counter2 = -1;
                        foreach (Series ts in new List<Series> { ts1, ts2 })
                        {
                            counter2++;
                            if (ts == null)
                            {
                                //ignore it
                            }
                            else
                            {
                                BrowserWritePrintLine(ts, sb3, gt);
                                if (counter2 == 0) sb3.Append(G.Blanks(gap + 1));
                            }
                        }

                        sb3.AppendLine();
                        if (gt.freq == EFreq.Q && gt.sub == Globals.freqQSubperiods) sb3.AppendLine();  //prettier
                        if (gt.freq == EFreq.M && gt.sub == Globals.freqMSubperiods) sb3.AppendLine();  //prettier
                    }

                    WriteHtmlPreCode(sb, sb3.ToString());
                }
                else
                {
                    WriteHtmlPreCode(sb, "+++ Note: variablens data kunne ikke indlæses");
                }

                StringBuilder x = new StringBuilder();
                x.AppendLine("<!DOCTYPE HTML PUBLIC `-//W3C//DTD HTML 4.01 Transitional//EN`>");
                x.AppendLine("<html>");
                x.AppendLine("  <head>");
                x.AppendLine("    <link rel=`stylesheet` href=`..\\" + settings_css_filename + @"` type=`text/css`>");
                x.AppendLine("    <link rel = `shortcut icon` href = `..\\" + settings_icon_filename + "` type = `image/vnd.microsoft.icon`>");
                x.AppendLine("    <meta http-equiv=`Content-Type` content=`text/html; charset=iso-8859-1`>");
                x.AppendLine("    <title>" + varnameWithoutFreq + "</title>");
                x.AppendLine("  </head>");

                x.AppendLine("  <script LANGUAGE = `JavaScript`> <!--");
                x.AppendLine("  function hide(id) {");
                x.AppendLine("  var x = document.getElementById(`b` + id);");
                x.AppendLine("  if (x.style.display === `none`)");
                x.AppendLine("  {");
                x.AppendLine("      x.style.display = `block`; ");
                x.AppendLine("  }");
                x.AppendLine("   else");
                x.AppendLine("   {");
                x.AppendLine("      x.style.display = `none`; ");
                x.AppendLine("   }");
                x.AppendLine("  }");
                x.AppendLine("  // -->");
                x.AppendLine("  </script >");

                x.AppendLine("  <body>");
                x.Append(sb);
                x.AppendLine("  </body>");
                x.AppendLine("</html>");

                string pathAndFilename = subFolder + "\\" + varnameWithoutFreq.ToLower() + ".html";
                using (FileStream fs = Program.WaitForFileStream(pathAndFilename, null, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    sw.Write(x.Replace('`', '\"'));
                }
            }

            StringBuilder x2 = new StringBuilder();
            x2.AppendLine("<!DOCTYPE HTML PUBLIC `-//W3C//DTD HTML 4.01 Transitional//EN`>");
            x2.AppendLine("<html>");
            x2.AppendLine("  <head>");
            x2.AppendLine("    <link rel=`stylesheet` href=`" + settings_css_filename + "` type=`text/css`>");
            x2.AppendLine("    <link rel = `shortcut icon` href = `" + settings_icon_filename + "` type = `image/vnd.microsoft.icon`>");
            x2.AppendLine("    <meta http-equiv=`Content-Type` content=`text/html; charset=iso-8859-1`>");
            x2.AppendLine("    <title>List of vars</title>");
            x2.AppendLine("  </head>");
            x2.AppendLine("  <body>");
            //x2.AppendLine("  <p><big><b>SMECdok, take two. Søg i browseren med Ctrl+F (find)</b></big></p>");

            x2.AppendLine("  <table cellpadding = `0` cellspacing = `0` width = `1000px` border = `0`> ");
            x2.AppendLine("  <tr>");
            x2.AppendLine("  <td width = `70 %` ><b><big>" + list_title + "</big></b></td>");
            x2.AppendLine("  <td width = `10 %` ><a href = `" + settings_find_filename + "` > Søg </a></td >");
            x2.AppendLine("  <td width = `20 %` ><a href = `" + settings_index_filename + "` > Hjem </a></td >");
            x2.AppendLine("  </tr>");
            x2.AppendLine("  </table>");

            x2.AppendLine("  <p>&nbsp;</p>");

            x2.AppendLine("<table style = `width:100%`>");

            foreach (string var2 in vars)
            {
                List<string> varExpl = Program.GetVariableExplanationFromExternalFile(var2);
                string expl = "";
                if (varExpl != null && varExpl.Count > 0) expl = varExpl[0];
                if (expl != null) expl = expl.Trim();
                Series ts1 = Program.databanks.GetFirst().GetIVariable(var2 + "!" + modelFrequencyString) as Series;
                if (ts1 != null && ts1.meta != null && !string.IsNullOrWhiteSpace(ts1.meta.label))
                {
                    if (!string.IsNullOrWhiteSpace(expl)) expl += ". "; //see also #324lkj2342
                    expl += ts1.meta.label;
                }
                expl = Program.SpecialXmlChars(expl);

                x2.Append("<tr>");
                x2.Append("<td width = `20%`>");
                x2.Append(HtmlLink(var2, settings_vars_foldername + "/" + var2.ToLower() + ".html"));
                x2.Append("</td>");
                x2.Append("<td width = `80%`>");
                x2.Append(expl);
                x2.Append("</td>");
                x2.Append("</tr>");
            }
            x2.AppendLine("</table>");

            x2.AppendLine("  </p>");
            x2.AppendLine("  </body>");
            x2.AppendLine("</html>");
            string pathAndFilename2 = rootFolder + "\\" + settings_list_filename;
            using (FileStream fs = Program.WaitForFileStream(pathAndFilename2, null, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter sw = G.GekkoStreamWriter(fs))
            {
                sw.Write(x2.Replace('`', '\"'));
            }

            // ------------------------------------------------------------
            // ----------------- find -------------------------------------
            // ------------------------------------------------------------

            var sorted = vars2.OrderBy(o => o.s1, StringComparer.OrdinalIgnoreCase);

            StringBuilder x3 = new StringBuilder();
            x3.AppendLine("<html>");
            x3.AppendLine("<head>");
            x3.AppendLine("<link rel = `stylesheet` href = `" + settings_css_filename + "` type = `text/css` >");
            x3.AppendLine("<link rel = `shortcut icon` href = `" + settings_icon_filename + "` type = `image/vnd.microsoft.icon`>");
            x3.AppendLine("</head>");
                        
            x3.AppendLine("<script LANGUAGE = `JavaScript` > <!-- ");

            string s1 = G.NL;
            string s2 = G.NL;
            foreach (EquationBrowserHelper s in sorted)
            {                
                s1 += "\"" + s.s1 + "\"" + ", " + G.NL;
                s2 += "\"" + s.s2 + "\"" + ", " + G.NL;
            }

            string write = null;
            string join = null;
            if (jsmFix)
            {
                write = "document.write";
            }
            else
            {
                write = "content.push";
                join = "document.body.innerHTML = content.join(``);";
            }

            string js = @"

            function varnavns() {
                var varnavn = [" + s1 + @"];
                return varnavn;
            }

            function beskrivs() {
                var beskriv = [" + s2 + @"];
                return beskriv;
            }

            function findvarnavn(){
                var content = [];
                var varnavn = varnavns();
                var beskriv = beskrivs();
                antal = varnavn.length;
                tekst = new String;
                tekst1 = new String;
                tekst = document.form1.tekst.value;
                fundet = false;

                " + write + @"(`Søgning efter variablen: '` + tekst + `'<br><br>`);

                for (var i = 0; i < antal; i++)
                {
                    tekst1 = varnavn[i];
                    if (tekst1.toUpperCase() == tekst.toUpperCase())
                    {
                        fundet = true;

                        " + write + @"(`<b><a href=" + settings_vars_foldername + @"/` + varnavn[i].toLowerCase() + `.html style='text-decoration:none'>` + varnavn[i] + `</a></b>`);
                        " + write + @"(`<br>` + beskriv[i] + `<br><hr><br>`);
                    } //endif
                } //endfor

                for (var i = 0; i < antal; i++)
                {
                    tekst1 = varnavn[i];
                    if (tekst1.toUpperCase().indexOf(tekst.toUpperCase()) != -1)
                    {
                        if (tekst1.toUpperCase() != tekst.toUpperCase())
                        {
                            fundet = true;
                            " + write + @"(`<a href=" + settings_vars_foldername + @"/` + varnavn[i].toLowerCase() + `.html style='text-decoration:none;'>` + varnavn[i] + `</a>`);
                            " + write + @"(`<br>` + beskriv[i] + `<br><br>`);
                        } //endif
                    } //endif
                } //endfor

                if (fundet == false)
                {
                    " + write + @"(`... gav intet resultat.<br>`);
                } //endif
                " + write + @"(`<br><br><a href=" + settings_find_filename + @">Søg igen</a> <br> <a href=" + settings_index_filename + @">Gå til hovedside</a>`);
                tekst1.free;
                tekst.free;
                " + join + @"
            }  //endfunction

            function check(event) {
            var charCode = (navigator.appName == `Netscape`) ? event.which : event.keyCode;
        if (charCode == 13) findvarnavn();
        }  // endfunction

        function findbeskriv()
        {
            var content = [];
            var varnavn = varnavns();
            var beskriv = beskrivs();
            antal = varnavn.length;
            tekst = new String;
            tekst2 = new String;
            tekst = document.form2.tekst.value;

            " + write + @"(`Søgeresultat<br>Søgning efter teksten: '` + tekst + `' i variabelliste<br><br>`);
            fundet = false;
            for (var i = 0; i < antal; i++)
            {
                tekst2 = beskriv[i];
                if (tekst2.toUpperCase().indexOf(tekst.toUpperCase()) != -1)
                {
                    fundet = true;
                    " + write + @"(`<b><a href=" + settings_vars_foldername + @"/` + varnavn[i].toLowerCase() + `.html style='text-decoration:none'>` + varnavn[i] + `</a></b>`);
                    " + write + @"(`<br>` + beskriv[i] + `<br><br>`);
                } //endif
            } //endfor
            if (fundet == false)
            {
                " + write + @"(`... gav intet resultat.<br>`);
            } //endif
            " + write + @"(`<br><br><a href=" + settings_find_filename + @">Søg igen</a> <br> <a href=" + settings_index_filename + @">Gå til hovedside</a>`);
            tekst.free;
            tekst2.free;

            " + join + @"
        }  //endfunction

        function check2(event) {
            var charCode = (navigator.appName == `Netscape`) ? event.which : event.keyCode;
        if (charCode == 13) findbeskriv();
        }  // endfunction

        ";

            x3.AppendLine(js);
            x3.AppendLine("// -->");
            x3.AppendLine("</script>");
            x3.AppendLine("<body onload = `document.form1.tekst.focus()`>");
            x3.AppendLine("<table width=`100 % `><tr><td>");
            x3.AppendLine("<p><b>Indtast søgeord</b></p>");
            //x3.AppendLine("<p>Angiv mnemoteknisk variabelnavn eller foretag fritekstsøgning i variabelbeskrivelserne</p>");
            //x3.AppendLine("<p>&nbsp;</p>");
            x3.AppendLine("");
            x3.AppendLine("Søgning efter variabelnavn:");
            x3.AppendLine("<FORM NAME = `form1` >");
            x3.AppendLine("<INPUT NAME=`tekst` SIZE=`50` TYPE=`text` onKeyPress=`return check(event)`>");
            x3.AppendLine("<INPUT TYPE = `submit` VALUE=`Søg` onClick=`findvarnavn()`>");
            x3.AppendLine("</FORM>");
            x3.AppendLine("<p>&nbsp;</p>");
            x3.AppendLine("Fritekstsøgning i variabelbeskrivelserne:");
            x3.AppendLine("<FORM NAME = `form2`>");
            x3.AppendLine("<INPUT NAME=`tekst` SIZE=`50` TYPE=`text` onKeyPress=`return check2(event)`>");
            x3.AppendLine("<INPUT TYPE = `submit` VALUE=`Søg` onClick=`findbeskriv()`>");
            x3.AppendLine("</FORM></center>");
            x3.AppendLine("</td></tr></table>");
            x3.AppendLine("</body>");
            x3.AppendLine("</html>");

            string pathAndFilename3 = rootFolder + "\\" + settings_find_filename;
            using (FileStream fs = Program.WaitForFileStream(pathAndFilename3, null, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter sw = G.GekkoStreamWriter(fs))
            {
                sw.Write(x3.Replace('`', '\"'));
            }

            if (missingFirst + missingRef > 0)
            {
                new Note("Regarding the model variables, there were " + missingFirst + " missing variable" + G.S(missingFirst) + " in the first-position databank and " + missingRef + " missing variable" + G.S(missingRef) + " in the refefence databank. Data, labels etc. for these are therefore not show.");
            }

            new Writeln("End of html browser generation, " + G.Seconds(dt0));

        }

        public static void BrowserNew()
        {
            string op = "d";
            EFreq freq = EFreq.A;  //there is some method for this, looking at model or bank??
            int max = 1; // int.MaxValue;
            int depthMax = 5;
            int countMax = 50;
            int pixels = 20;
            int pixelsAfterArrow = 12;
            int firstColWidth = 200;
            string path = @"c:\Thomas\Desktop\gekko\testing\Browser\";
            G.DeleteFolder(path, "css", false);
            File.Copy(@"c:\Thomas\Gekko\GekkoCS\Gekko\bin\x64\Release\images\checked.png", path + "checked.png");
            File.Copy(@"c:\Thomas\Gekko\GekkoCS\Gekko\bin\x64\Release\images\normal.png", path + "normal.png");
            bool adam = false;
            bool showGUI = false;
            bool pivot = true;  //also calculates pivot table (only relevant when showGUI == false)

            Program.options.databank_search = false;

            if (true)
            {
                if (adam)
                {
                    Program.options.folder_working = @"c:\Thomas\Desktop\gekko\testing";
                    Program.RunGekkoCommands("reset;", "", 0, new P());
                    Program.RunGekkoCommands("model jul05;", "", 0, new P());
                    Program.RunGekkoCommands("read jul05;", "", 0, new P());
                }
                else
                {
                    Program.options.folder_working = @"c:\Thomas\Desktop\gekko\testing\Decomp\Decomp2";
                    Program.RunGekkoCommands("reset;", "", 0, new P());
                    Program.RunGekkoCommands("model<gms>makro.zip;", "", 0, new P());
                    Program.RunGekkoCommands("read makro1;", "", 0, new P());                    
                    Program.RunGekkoCommands(@"open 'c:\Thomas\Desktop\gekko\testing\MAKRO\2024-01-10-c2f2447\Data\Makrobk\makrobk.gbk' as traces;", "", 0, new P());
                    Program.RunGekkoCommands(@"unlock traces;", "", 0, new P());
                    Program.RunGekkoCommands("traces:tNetAfg_y[tje, tje] <2020 2020> = 12345;", "", 0, new P());
                }
            }
            O.Decomp2 o = new O.Decomp2();
            o.type = @"ASTDECOMP3";
            if (adam)
            {
                o.t1 = new GekkoTime(EFreq.A, 2006, 1, 1);
                o.t2 = new GekkoTime(EFreq.A, 2010, 1, 1);
            }
            else
            {
                o.t1 = new GekkoTime(EFreq.A, 2028, 1, 1);
                o.t2 = new GekkoTime(EFreq.A, 2035, 1, 1);
            }
            o.opt_prtcode = O.ConvertToString((new ScalarString(op)));

            Model model = Program.model;
            ModelGamsScalar modelGamsScalar = model.modelGamsScalar;

            int count = 0;
            GekkoDictionary<string, bool> seen = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            GekkoDictionary<string, bool> seenPlot = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            int n = modelGamsScalar.CountEqs(1);
            for (int i = 0; i < n; i++)
            {
                string eqName27 = modelGamsScalar.dict_FromEqNumberToEqName[i];
                ExtractTimeDimensionHelper helper2 = GamsModel.ExtractTimeDimension(true, EExtractTimeDimension.NoIndexListOfStrings, eqName27, false);
                //var x = helper2.name;
                var eqName2 = helper2.resultingFullName;

                if (helper2.time.Equals(o.t1))
                {
                    new Writeln(i + " of " + n + " (" + G.FormatNumber((double)i / (double)n * 100d, "f10.2", false, false) + "%)");
                    count++;
                    if (count > max) return;

                    List<string> precedents = new List<string>();
                    foreach (PeriodAndVariable dp in modelGamsScalar.precedents[i].vars)
                    {
                        //foreach precedent variable                            
                        string variableName = modelGamsScalar.GetVarNameA(dp.variable);
                        precedents.Add(variableName);
                    }

                    foreach (string variableName in precedents)
                    {
                        string fileName1 = eqName2 + "__" + variableName + ".html";

                        //foreach precedent variable                                                    
                        GekkoTime tUsedHere = o.t1;
                        if (!seen.ContainsKey(fileName1))
                        {
                            seen.Add(fileName1, false);
                            new Writeln(fileName1);
                            DecompOptions2 decompOptions2 = new DecompOptions2();
                            decompOptions2.t1 = o.t1;
                            decompOptions2.t2 = o.t2;
                            decompOptions2.decompOperator = new DecompOperator(o.opt_prtcode.ToLower());
                            decompOptions2.new_select = new List<string>() { variableName };
                            decompOptions2.new_from = new List<string>() { eqName2 };
                            decompOptions2.new_endo = new List<string>() { variableName };

                            GekkoTime per1 = decompOptions2.t1;
                            GekkoTime per2 = decompOptions2.t2;
                            GekkoSmpl smpl = new GekkoSmpl(per1, per2);
                            DecompDatas decompDatas = new DecompDatas();

                            GekkoTime gt1, gt2;
                            Gekko.Decomp.DecompMainInit(out gt1, out gt2, per1, per2, decompOptions2.decompOperator);

                            DateTime t0 = DateTime.Now;

                            Gekko.Decomp.EContribType operatorOneOf3Types = decompOptions2.decompOperator.type;

                            int perLag = -2;
                            string lhsString = "Expression value";
                            int parentI = 0;

                            int funcCounter = 0;

                            Gekko.Decomp.PrepareEquations(per1, per2, decompOptions2.decompOperator, decompOptions2, false, modelGamsScalar);

                            if (decompDatas.storage == null) decompDatas.storage = new List<List<DecompData>>();
                            decompDatas.MAIN_data = null;

                            if (decompDatas.storage == null || decompDatas.storage.Count == 0) Gekko.Decomp.InitDecompDatas(decompOptions2, decompDatas, model);

                            string residualName = Program.GetDecompResidualName(0, 1);
                            string table = null;
                            try
                            {
                                table += "<table cellpadding=`10`>";
                                DecompData dd = Gekko.Decomp.DecompLowLevelScalar(gt1, gt2, 0, decompOptions2.link[0].GAMS_dsh[0], decompOptions2.decompOperator, residualName, ref funcCounter, decompOptions2.missingAsZero, model);

                                table += "<tr><td></td>";
                                foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                                {
                                    table += "<td align = `right`>" + t.ToString() + "</td>";
                                }
                                table += "</tr>";

                                foreach (KeyValuePair<string, Series> kvp in dd.cellsContribD.storage)
                                {
                                    int lag; string name;
                                    Gekko.Decomp.ConvertFromTurtleName(kvp.Key, true, out name, out lag);
                                    string name2 = G.Chop_RemoveBank(name).Replace("zzzzzzzzy", "RESIDUAL");
                                    table += "<tr>";
                                    table += "<td>";
                                    table += name2;
                                    table += "</td>";
                                    foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                                    {
                                        table += "<td align = `right`>";
                                        double value = kvp.Value.GetDataSimple(t);
                                        table += G.FormatNumber(value, "f15.4", true, false);
                                        table += "</td>";
                                    }
                                    table += "</tr>";
                                }
                                table += "</table>";
                            }
                            catch
                            {
                                table = null;
                            }

                            tUsedHere = modelGamsScalar.Maybe2000GekkoTime(decompOptions2.t1);
                            string s2 = G.Chop_DimensionAddLast(eqName2, tUsedHere.ToString(), false);
                            EquationTextHelper helper = new EquationTextHelper();
                            GetEquationTextHelper helper22 = Program.model.GetEquationText(new List<string>() { s2 }, helper, tUsedHere);

                            StringBuilder html1 = new StringBuilder();
                            EquationBrowser.WriteHtml(html1, "VARIABLE: <span style=`color: green`>" + variableName + "</span>");
                            EquationBrowser.WriteHtml(html1, Program.SpecialXmlChars(Program.GetVariableExplanation1Line(variableName)));
                            EquationBrowser.WriteHtml(html1, "EQUATION: <span style=`color: green`>" + eqName2 + "</span>");

                            string s5 = helper22.s_gamsOrFrnSyntax;
                            string s6 = helper22.s_scalarModel;
                            int index = s6.IndexOf("..");
                            if (index >= 0) s6 = s6.Substring(index + "..".Length).Trim();
                            foreach (string variableName2 in precedents)
                            {
                                //s5 = G.Replace(s5, variableName2, EquationBrowser.HtmlLink(variableName2, variableName2 + ".html", Program.SpecialXmlChars(Program.GetVariableExplanation1Line(variableName2))), StringComparison.OrdinalIgnoreCase, 0);
                                //s6 = G.Replace(s6, variableName2, EquationBrowser.HtmlLink(variableName2, variableName2 + ".html", Program.SpecialXmlChars(Program.GetVariableExplanation1Line(variableName2))), StringComparison.OrdinalIgnoreCase, 0);
                                //s6 = G.Replace(s6, variableName2, EquationBrowser.HtmlLink(variableName2, variableName2 + ".html"), StringComparison.OrdinalIgnoreCase, 0);
                            }

                            html1.Append("<hr>");
                            //EquationBrowser.WriteHtml(html1, s5);
                            EquationBrowser.WriteHtmlPreCode(html1, s5);
                            html1.Append("<hr>");
                            //EquationBrowser.WriteHtml(html1, s6);
                            EquationBrowser.WriteHtmlPreCode(html1, s6);
                            html1.Append("<hr>");

                            EquationBrowser.WriteHtml(html1, "Variables: ");
                            string vars2 = null;
                            foreach (string variableName2 in precedents)
                            {
                                EquationBrowser.WriteHtml(html1, EquationBrowser.HtmlLink(variableName2) + " " + Program.SpecialXmlChars(Program.GetVariableExplanation1Line(variableName2)));
                            }

                            try
                            {
                                if (!seenPlot.ContainsKey(variableName))
                                {
                                    seenPlot.Add(variableName, false);
                                    //only plot the series from Work
                                    Program.RunGekkoCommands("plot <" + per1.ToString() + " " + per2.ToString() + " > " + variableName + " file='" + path + variableName + ".svg';", "", 0, new P());
                                }
                                else
                                {
                                    //Just reference it
                                }
                                html1.AppendLine("<img src = `" + variableName + ".svg" + "`>");
                                html1.AppendLine("<p/>");
                            }
                            catch
                            {
                            }

                            if (false)
                            {
                                EquationBrowser.WriteHtml(html1, "--> decomp " + variableName + " from " + eqName2);
                            }

                            EquationBrowser.WriteHtml(html1, "Related equations:");
                            bool first2 = true;
                            string s8 = null;
                            foreach (EqHelper eqHelper in GetRelatedEquations(variableName, tUsedHere, model, modelGamsScalar))
                            {
                                string eqNameWithLagNoBlanks = eqHelper.eqNameWithLag.Replace(" ", "");
                                string link = EquationBrowser.HtmlLink(eqNameWithLagNoBlanks, eqNameWithLagNoBlanks + "__" + variableName + ".html");
                                if (!first2) s8 += ", ";
                                s8 += link;
                                first2 = false;
                            }
                            EquationBrowser.WriteHtml(html1, s8);

                            if (table != null)
                            {
                                html1.AppendLine("<hr>");
                                EquationBrowser.WriteHtmlBold(html1, "Time-decomposition (absolute changes):");
                                html1.AppendLine(table);
                            }

                            if (true)
                            {
                                //Traces
                                Series ts = O.GetIVariableFromString(G.Chop_AddFreq(G.Chop_AddBank(variableName, "traces"), freq), O.ECreatePossibilities.NoneReturnNullAlways) as Series;

                                if (ts != null && ts?.meta?.trace2.GetPrecedents_BewareOnlyInternalUse().GetStorage() != null && ts.meta.trace2.GetPrecedents_BewareOnlyInternalUse().GetStorage().Count() > 0)
                                {
                                    html1.AppendLine("<hr>");
                                    html1.AppendLine(@"<p style=`font-weight: bold;`>Data traces</p>");

                                    GekkoTimeSpansSimple gtss = null;
                                    Trace2 trace = ts.meta.trace2;
                                    TraceHelper2 th = new TraceHelper2();
                                    th.html = html1;
                                    th.depthMax = depthMax;
                                    th.counterMax = countMax;
                                    th.pixels = pixels;
                                    th.pixelsAfterArrow = pixelsAfterArrow;
                                    th.freq = freq;
                                    if (false)
                                    {                                        
                                        th.html.AppendLine(@"<ul>");
                                        th.html.AppendLine(@"<li class=`folder`>");
                                        th.html.AppendLine(@"<div class=`list-item-content`>");
                                        th.html.AppendLine(@"<div class=`folder-label`><span class=`folder-icon`><img class=`img-size` src =`normal.png` style =`visibility: hidden; margin-right: " + pixelsAfterArrow + ";`></span><span style = `font-weight: bold;`>Name</span></div>");
                                        th.html.AppendLine(@"<div style = `margin-left:" + pixels + "px; font-weight: bold;`>Code</div>");
                                        th.html.AppendLine(@"<div style = `font-weight: bold;`>Active</div>");
                                        th.html.AppendLine(@"<div style = `font-weight: bold;`>Stamp</div>");
                                        th.html.AppendLine(@"<div style = `font-weight: bold;`>File</div>");
                                        th.html.AppendLine(@"</div>");
                                        th.html.AppendLine(@"<div class=`extra-content`></div>");
                                        th.html.AppendLine(@"</li>");
                                    }
                                    //th.html.AppendLine(@" <li class=`folder`>");
                                    WalkTracesForHtml(trace, gtss, th, 0);
                                    //th.html.AppendLine(@"</div>");
                                    if (false)
                                    {
                                        th.html.AppendLine(@"</ul>");                                        
                                    }
                                }
                            }

                            StringBuilder x = new StringBuilder();
                            x.AppendLine("<!DOCTYPE HTML PUBLIC `-//W3C//DTD HTML 4.01 Transitional//EN`>");
                            x.AppendLine("<html>");
                            x.AppendLine("  <head>");
                            x.AppendLine("    <link rel=`stylesheet` href=`" + "styles.css" + @"` type=`text/css`>");
                            x.AppendLine("    <meta http-equiv=`Content-Type` content=`text/html; charset=iso-8859-1`>");
                            x.AppendLine("    <title>" + "EQUATION " + eqName2 + " (endo " + variableName + ")" + "</title>");

                            string css = @"<style>        
        html {
            font-family: Verdana, Geneva, Tahoma, sans-serif;
            font-size:12px;
        }
    
        ul {
            list-style: none;
            padding-left: 20px;
        }
        li {            
            cursor: pointer;
        }        
    
        .img-size {
            height:1em;
        }        
    
        .nested {
            display: none;
        }
        .open > .nested {
            display: block;
        }
    
        .list-item-content {
            display: flex;
            justify-content: flex-start;
            width: 100%;
            font-size:12px;
        }
    
        /* Set different widths for the columns */
        .list-item-content > div:nth-child(1) {
            width: " + firstColWidth + @"px;
            padding: 5px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;            
        }
        
        .list-item-content > div:nth-child(2) {
            width: 400px;
            padding: 5px;
            padding-left: 8;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
            border-left: 1px solid #ccc; 
        }
        
        .list-item-content > div:nth-child(3) {
            width: 90px;
            padding: 5px;
            padding-left: 8;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
            border-left: 1px solid #ccc; 
        }

        .list-item-content > div:nth-child(4) {
            width: 80px;
            padding: 5px;
            padding-left: 8;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
            border-left: 1px solid #ccc; 
        }

        .list-item-content > div:nth-child(5) {
            width: 200px;
            padding: 5px;
            padding-left: 8;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
            border-left: 1px solid #ccc;             
        }
    
        /* Textbox at the bottom */
        textarea {
            width: 1000px;
            height: 150px;
            margin-top: 5px;
            margin-left: 20px;
            padding: 10px;
            font-family: Consolas;  
            font-size:13px;
            background-color: #fefce7;
            overflow: auto;
        }
    
        .extra-content {
            display: none;
        }
    
        .selected {
            background-color: #0078d7;
            color: white;
        }
    </style>";

                            x.AppendLine(css);

                            string js = @"<script>
    let currentSelected = null;

    // Function to calculate the deepest level of visible list items
    function calculateMaxIndentation() {
        let maxIndentationLevel = 0;

        // Loop through all visible .list-item-content elements
        document.querySelectorAll('.list-item-content').forEach(item => {
            // Check visibility of current item
            if (isElementVisible(item)) {
                const level = calculateIndentationLevel(item);
                if (level > maxIndentationLevel) {
                    maxIndentationLevel = level;
                }
            }
        });

        // Adjust the width of the first column based on the maximum indentation level
        const firstColumnWidth = " + firstColWidth + @" + maxIndentationLevel * " + pixels + @";
        document.querySelectorAll('.list-item-content > div:nth-child(1)').forEach(div => {
            div.style.width = `` + firstColumnWidth + `px`;
        });
    }

    // Helper function to check if an element is visible
    function isElementVisible(item) {
        // An item is visible if all its parent folders are open
        let parentFolder = item.closest('li.folder');
        while (parentFolder) {
            if (!parentFolder.classList.contains('open')) {
                return false; // Not visible if a parent folder is closed
            }
            parentFolder = parentFolder.closest('ul').closest('li.folder');
        }
        return true;
    }

    // Function to calculate the indentation level of a given list item
    function calculateIndentationLevel(item) {
        let level = 0;
        let currentElement = item.closest('li');

        while (currentElement && currentElement.closest('ul')) {
            level++;
            currentElement = currentElement.closest('ul').closest('li');
        }

        return level;
    }

    // Handle folder icon click (expand/collapse)
    document.querySelectorAll('.folder-icon').forEach(icon => {
        icon.addEventListener('click', function(e) {
            const folder = this.closest('.folder');
            folder.classList.toggle('open');
            
            // Change folder icon
            if (folder.classList.contains('open')) {
                this.innerHTML = '<img class=`img-size` src=`checked.png` style =`margin-right: " + pixelsAfterArrow + @"`>';
            } else
                                {
                                    this.innerHTML = '<img class=`img-size` src =`normal.png`  style =`margin-right: " + pixelsAfterArrow + @"`>';
                                }

                                // Recalculate the column width
                                calculateMaxIndentation();

                                e.stopPropagation();
                            });
            });

            // Handle row selection
            document.querySelectorAll('.list-item-content').forEach(item => {
                item.addEventListener('click', function(e) {
                    if (e.target.closest('.folder-icon'))
                    {
                        return;
                    }

                    if (currentSelected)
                    {
                        currentSelected.classList.remove('selected');
                    }

                    currentSelected = this;
                    currentSelected.classList.add('selected');

                    let extraContent = this.parentElement.querySelector('.extra-content').textContent.trim();
                    extraContent = extraContent.replace(/\\n/g, '\n');
                    document.getElementById('output').value = extraContent;

                    e.stopPropagation();
                });
        });

    // Keyboard navigation
    document.addEventListener('keydown', function(e)
        {
            if (!currentSelected) return;
            let nextRow = null;
            if (e.key === 'ArrowDown')
            {
                nextRow = currentSelected.parentElement.nextElementSibling?.querySelector('.list-item-content');
            }
            else if (e.key === 'ArrowUp')
            {
                nextRow = currentSelected.parentElement.previousElementSibling?.querySelector('.list-item-content');
            }
            if (nextRow)
            {
                currentSelected.classList.remove('selected');
                currentSelected = nextRow;
                currentSelected.classList.add('selected');

                let extraContent = currentSelected.parentElement.querySelector('.extra-content').textContent.trim();
                extraContent = extraContent.replace(/\\n/g, '\n');
                document.getElementById('output').value = extraContent;
            }
        });

    // Initial column width calculation on load
    calculateMaxIndentation();
</script>";

                            x.AppendLine("  </head>");
                            x.AppendLine("  <body>");
                            x.Append(html1);
                            x.Append("<textarea id = `output` readonly>Click '>' to unfold sub-traces. Click a row to see more trace info.</textarea>");
                            x.AppendLine(js);
                            x.AppendLine("  </body>");
                            x.AppendLine("</html>");
                            using (FileStream fs = Program.WaitForFileStream(path + fileName1, null, Program.GekkoFileReadOrWrite.Write))
                            using (StreamWriter sw = G.GekkoStreamWriter(fs))
                            {
                                //BEWARE: In JavaScript, it is legal to do y = `i am a string';, where backticks indicate that {}-interpolation 
                                //        can be used. So if JavaScript with backticks is used, do a workaround.
                                sw.Write(x.Replace('`', '\"'));
                            }
                        }

                        string fileName2 = variableName + ".html";
                        if (!seen.ContainsKey(fileName2))
                        {
                            seen.Add(fileName2, false);
                            new Writeln(fileName1);
                            List<EqHelper> eqsNew = GetRelatedEquations(variableName, tUsedHere, model, modelGamsScalar);
                            StringBuilder html2 = new StringBuilder();
                            EquationBrowser.WriteHtml(html2, variableName + " occurs in the following equations:");
                            EquationBrowser.WriteHtml(html2, Program.SpecialXmlChars(Program.GetVariableExplanation1Line(variableName)));
                            string table = "<table cellpadding=`10`>";
                            foreach (EqHelper eqHelper in eqsNew)
                            {
                                table += "<tr>";
                                EquationTextHelper helper = new EquationTextHelper();
                                GetEquationTextHelper helper22 = Program.model.GetEquationText(new List<string>() { eqHelper.eqName }, helper, tUsedHere);
                                string eqNameWithLagNoBlanks = eqHelper.eqNameWithLag.Replace(" ", "");
                                string link = EquationBrowser.HtmlLink(eqNameWithLagNoBlanks, eqNameWithLagNoBlanks + "__" + variableName + ".html");
                                table += "<td style=`vertical-align:top`>";
                                table += link;
                                table += "</td>";
                                table += "<td style=`vertical-align:top`>";
                                table += helper22.s_gamsOrFrnSyntax;
                                table += "</td>";
                                table += "</tr>";
                            }
                            html2.AppendLine(table);

                            StringBuilder x2 = new StringBuilder();
                            x2.AppendLine("<!DOCTYPE HTML PUBLIC `-//W3C//DTD HTML 4.01 Transitional//EN`>");
                            x2.AppendLine("<html>");
                            x2.AppendLine("  <head>");
                            x2.AppendLine("    <link rel=`stylesheet` href=`" + "styles.css" + @"` type=`text/css`>");
                            x2.AppendLine("    <meta http-equiv=`Content-Type` content=`text/html; charset=iso-8859-1`>");
                            x2.AppendLine("    <title>" + "EQUATION " + eqName2 + " (endo " + variableName + ")" + "</title>");
                            x2.AppendLine("  </head>");
                            x2.AppendLine("  <body>");
                            x2.Append(html2);
                            x2.AppendLine("  </body>");
                            x2.AppendLine("</html>");
                            using (FileStream fs = Program.WaitForFileStream(path + fileName2, null, Program.GekkoFileReadOrWrite.Write))
                            using (StreamWriter sw = G.GekkoStreamWriter(fs))
                            {
                                sw.Write(x2.Replace('`', '\"'));
                            }
                        }
                    }
                }
            }

            return;

            //BEWARE: omits time dimension, so 

            {

                G.CheckLegalPeriod(o.t1, o.t2);

                DecompOptions2 decompOptions2 = new DecompOptions2();
                decompOptions2.t1 = o.t1;
                decompOptions2.t2 = o.t2;
                decompOptions2.decompOperator = new DecompOperator(o.opt_prtcode.ToLower());

                decompOptions2.isNew = true;
                o.decompFind = new DecompFind(EDecompFindNavigation.Decomp, 0, decompOptions2, null, model);

                Gekko.Decomp.ResetRowsColsSelection(decompOptions2);

                decompOptions2.type = o.type;

                string lhsName = null;
                string eqName = null;

                if (adam)
                {
                    lhsName = "fY";
                    eqName = "e_" + lhsName;  //small "e"
                }
                else
                {
                    lhsName = "vtKommune[tot]";
                    eqName = "E_ftKommune_tot";
                }

                decompOptions2.new_select = new List<string>() { lhsName };
                decompOptions2.new_from = new List<string>() { eqName };
                decompOptions2.new_endo = new List<string>() { lhsName };

                for (int i = 0; i < decompOptions2.new_select.Count; i++) decompOptions2.new_select[i] = G.HandleBlanksRemove(decompOptions2.new_select[i]);
                for (int i = 0; i < decompOptions2.new_from.Count; i++) decompOptions2.new_from[i] = G.HandleBlanksRemove(decompOptions2.new_from[i]);
                for (int i = 0; i < decompOptions2.new_endo.Count; i++) decompOptions2.new_endo[i] = G.HandleBlanksRemove(decompOptions2.new_endo[i]);

                modelGamsScalar.MaybeLoadDataIntoModel(o.decompFind.depth, decompOptions2.t1, decompOptions2.t2, false);
                //Gekko.Decomp.DecompGetFuncExpressionsAndRecalc(o.decompFind, null);

                // =========

                {

                    GekkoTime per1 = decompOptions2.t1;
                    GekkoTime per2 = decompOptions2.t2;
                    GekkoSmpl smpl = new GekkoSmpl(per1, per2);
                    DecompDatas decompDatas = new DecompDatas();

                    GekkoTime gt1, gt2;
                    Gekko.Decomp.DecompMainInit(out gt1, out gt2, per1, per2, decompOptions2.decompOperator);

                    DateTime t0 = DateTime.Now;

                    Gekko.Decomp.EContribType operatorOneOf3Types = decompOptions2.decompOperator.type;

                    int perLag = -2;
                    string lhsString = "Expression value";
                    int parentI = 0;

                    int funcCounter = 0;

                    Gekko.Decomp.PrepareEquations(per1, per2, decompOptions2.decompOperator, decompOptions2, true, modelGamsScalar);

                    if (decompDatas.storage == null) decompDatas.storage = new List<List<DecompData>>();
                    decompDatas.MAIN_data = null;

                    if (decompDatas.storage == null || decompDatas.storage.Count == 0) Gekko.Decomp.InitDecompDatas(decompOptions2, decompDatas, model);

                    string residualName = Program.GetDecompResidualName(0, 1);
                    DecompData dd = Gekko.Decomp.DecompLowLevelScalar(gt1, gt2, 0, decompOptions2.link[0].GAMS_dsh[0], decompOptions2.decompOperator, residualName, ref funcCounter, decompOptions2.missingAsZero, model);

                    List<string> vars = new List<string>();
                    foreach (string var in dd.cellsContribD.storage.Keys)
                    {
                        int lag; string name;
                        Gekko.Decomp.ConvertFromTurtleName(var, true, out name, out lag);
                        vars.Add(G.Chop_RemoveBank(name));
                    }

                    GekkoTime tUsedHere = decompOptions2.t1;
                    tUsedHere = modelGamsScalar.Maybe2000GekkoTime(decompOptions2.t1);
                    string s2 = G.Chop_DimensionAddLast(eqName, tUsedHere.ToString(), false);
                    EquationTextHelper helper = new EquationTextHelper();
                    GetEquationTextHelper helper2 = Program.model.GetEquationText(new List<string>() { s2 }, helper, tUsedHere);

                    string html1 = null;
                    html1 += "vtKommune[tot] from E_ftKommune_tot" + G.NL + G.NL;
                    html1 += helper2.s_gamsOrFrnSyntax + G.NL + G.NL + helper2.s_scalarModel;
                    new Writeln(html1);

                    // ==================================================
                    //  Klikker vtKommune[15]
                    // ==================================================
                    string variableName = "vtKommune[15]";
                    int aNumber = modelGamsScalar.dict_FromVarNameToANumber.GetInt(variableName);
                    if (aNumber == -12345) new Error("Hov");
                    int timeIndex = modelGamsScalar.FromGekkoTimeToTimeInteger(modelGamsScalar.Maybe2000GekkoTime(tUsedHere));
                    PeriodAndVariable pav = new PeriodAndVariable(timeIndex, aNumber);
                    List<int> eqNumbers = null; modelGamsScalar.dependents.TryGetValue(pav, out eqNumbers);
                    if (eqNumbers == null) new Error("Hov");
                    List<EqHelper> eqsNew = Gekko.Decomp.FindEquationsThatContainGivenVariableSorted(variableName, tUsedHere, eqNumbers, model);

                    string html2 = null;
                    html2 += "Equations containing " + variableName + ":" + G.NL;
                    foreach (EqHelper eqHelper in eqsNew)
                    {
                        html2 += eqHelper.eqNameWithLag;
                        html2 += G.NL;
                    }
                    new Writeln(html2);

                    if (pivot)
                    {
                        Gekko.Decomp.DecompMainMergeOrAdd(decompDatas, dd, 0, 0);
                        DecompOutput decompOutput = MakePivot_DeleteMeAtSomePoint(model, decompOptions2, per1, per2, smpl, decompDatas, operatorOneOf3Types, lhsString, parentI);
                    }

                }
            }

            return;
        }

        /// <summary>
        /// Helper for html browser, calling the DECOMP methods.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="per1"></param>
        /// <param name="per2"></param>
        /// <param name="smpl"></param>
        /// <param name="decompDatas"></param>
        /// <param name="operatorOneOf3Types"></param>
        /// <param name="lhsString"></param>
        /// <param name="parentI"></param>
        /// <returns></returns>
        private static DecompOutput MakePivot_DeleteMeAtSomePoint(Model model, DecompOptions2 decompOptions2, GekkoTime per1, GekkoTime per2, GekkoSmpl smpl, DecompDatas decompDatas, Decomp.EContribType operatorOneOf3Types, string lhsString, int parentI)
        {
            if (operatorOneOf3Types == Gekko.Decomp.EContribType.D) decompDatas.hasD = true;
            else if (operatorOneOf3Types == Gekko.Decomp.EContribType.RD) decompDatas.hasRD = true;
            else if (operatorOneOf3Types == Gekko.Decomp.EContribType.M) decompDatas.hasM = true;

            if (decompOptions2.link[parentI].varnames == null)
            {
                //does this ever happen?
                decompOptions2.link[parentI].varnames = Globals.decompResidualName;
            }

            bool[] used = new bool[decompDatas.storage.Count];
            used[0] = true;  //primary equation

            GekkoDictionary<string, bool> ignore = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            //decomp period by period, showing lags/leads.

            if (decompOptions2.decompOperator.lowLevel == Gekko.Decomp.ELowLevel.BothQuoAndRef)  //<mp>
            {
                bool refreshObjects = true;
                foreach (GekkoTime gt in new GekkoTimeIterator(per1, per2))
                {
                    Gekko.Decomp.DecompMainHelperInvertScalar(gt, gt, decompOptions2, decompDatas, Gekko.Decomp.EContribType.D, parentI, refreshObjects, decompOptions2.decompOperator, model.modelGamsScalar);
                    refreshObjects = false;
                }
                foreach (GekkoTime gt in new GekkoTimeIterator(per1, per2))
                {
                    Gekko.Decomp.DecompMainHelperInvertScalar(gt, gt, decompOptions2, decompDatas, Gekko.Decomp.EContribType.RD, parentI, refreshObjects, decompOptions2.decompOperator, model.modelGamsScalar);
                }
            }
            else
            {
                int deduct = 0;
                //why deduct not enough??
                if (decompOptions2.decompOperator.isDoubleDifQuo || decompOptions2.decompOperator.isDoubleDifRef) deduct = -1;  //all the data are ready, so we can calc 1 period earlier, so that a 1-period decomp actually shows something for <dp> or <rdp>
                bool refreshObjects = true;
                foreach (GekkoTime gt in new GekkoTimeIterator(per1.Add(deduct), per2))
                {
                    Gekko.Decomp.DecompMainHelperInvertScalar(gt, gt, decompOptions2, decompDatas, operatorOneOf3Types, parentI, refreshObjects, decompOptions2.decompOperator, model.modelGamsScalar);
                    refreshObjects = false;
                }
            }

            DecompData decompDataMAINClone = decompDatas.MAIN_data.DeepClone();

            return Gekko.Decomp.DecompPivotToTable(per1, per2, decompDataMAINClone, decompDatas, decompOptions2.decompOperator, smpl, lhsString, decompOptions2.link[parentI].expressionText, decompOptions2, operatorOneOf3Types, model);
        }

        /// <summary>
        /// Walks through nested data traces, producing html while doing so. This is similar to what is done regarding
        /// WindowTreeViewWithTable in CallTraceViewer(). (But in that window the walking is done while the user folds/unfolds, whereas here,
        /// we have to do all the walking in one go).
        /// </summary>
        /// <param name="trace"></param>
        /// <param name="gtss"></param>
        /// <param name="th"></param>
        /// <param name="depth"></param>
        public static void WalkTracesForHtml(Trace2 trace, GekkoTimeSpansSimple gtss, TraceHelper2 th, int depth)
        {
            th.counter++;
            if (depth > 0)
            {
                TraceItem traceItem = trace.FromTraceToTreeViewItem(gtss);
                th.html.AppendLine(@"<div class=`list-item-content`>");
                th.html.AppendLine(@"<div class=`folder-label`><span class=`folder-icon`><img class=`img-size` src=`normal.png` style =`margin-right: " + th.pixelsAfterArrow + "`></span><span>" + G.Chop_RemoveFreq(G.Chop_RemoveBank(traceItem.Name), th.freq) + @"</span></div>");
                th.html.AppendLine(@"<div style = `margin-left:" + (-(depth - 1) * th.pixels) + @"px`>" + traceItem.Code + @"</div>");
                th.html.AppendLine(@"<div>" + traceItem.Active + @"</div>");
                th.html.AppendLine(@"<div>" + traceItem.Stamp + @"</div>");
                th.html.AppendLine(@"<div>" + traceItem.File + @"</div>");
                th.html.AppendLine(@"</div>");
                string extra = Trace2.FromTraceItemToDetailedText(traceItem, true);
                th.html.AppendLine(@"<div class=`extra-content`>" + extra + @"</div>");
            }
            if (trace.GetPrecedents_BewareOnlyInternalUse().Count() > 0)
            {
                if (depth >= th.depthMax || th.counter >= th.counterMax)
                {
                    //th.html.AppendLine(@"<p>TRUNCATED</p>");
                }
                else
                {
                    if (depth == 0) th.html.AppendLine(@"<ul>");
                    else th.html.AppendLine(@"<ul class=`nested`>");
                    foreach (TraceAndPeriods2 traceAndPeriods in trace.GetPrecedents_BewareOnlyInternalUse().GetStorage())
                    {
                        if (traceAndPeriods.trace.type == ETraceType.Divider) continue;
                        th.html.AppendLine(@"<li class=`folder`>");
                        WalkTracesForHtml(traceAndPeriods.trace, traceAndPeriods.periods, th, depth + 1);
                        th.html.AppendLine(@"</li>");
                    }
                    th.html.AppendLine(@"</ul>");
                }
            }
        }

        /// <summary>
        /// Finds equations that contain the given variable name.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="tUsedHere"></param>
        /// <param name="model"></param>
        /// <param name="modelGamsScalar"></param>
        /// <returns></returns>
        private static List<EqHelper> GetRelatedEquations(string variableName, GekkoTime tUsedHere, Model model, ModelGamsScalar modelGamsScalar)
        {
            int aNumber = modelGamsScalar.dict_FromVarNameToANumber.GetInt(variableName);
            if (aNumber == -12345) new Error("Hov");
            int timeIndex = modelGamsScalar.FromGekkoTimeToTimeInteger(modelGamsScalar.Maybe2000GekkoTime(tUsedHere));
            PeriodAndVariable pav = new PeriodAndVariable(timeIndex, aNumber);
            List<int> eqNumbers = null; modelGamsScalar.dependents.TryGetValue(pav, out eqNumbers);
            if (eqNumbers == null) new Error("Hov");
            List<EqHelper> eqsNew = Gekko.Decomp.FindEquationsThatContainGivenVariableSorted(variableName, tUsedHere, eqNumbers, model);
            return eqsNew;
        }

        private static string BrowserGetVariable(List<TokenHelper> a)
        {
            string varLine = null;
            for (int i2 = 0; i2 < a.Count; i2++)
            {
                if (a[i2].type == ETokenType.Word)
                {
                    if (i2 - 1 >= 0 && a[i2].leftblanks == 0 && (a[i2 - 1].s == Globals.symbolCollection.ToString() || a[i2 - 1].s == Globals.symbolScalar.ToString()))
                    {
                        //skip a #x or %x
                        continue;
                    }
                    if (a[i2 + 1].s == "(")
                    {
                        //function call, skip it
                        continue;
                    }
                    varLine = a[i2].s;
                    break;
                }
            }

            return varLine;
        }

        /// <summary>
        /// Gets the frequency of the model by means of looking at the databank (frequencies there).
        /// This is in principle approximate, in practice pretty waterproof, and so we do not need
        /// an additional frequency setting in the .json.
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        private static string GetModelFreq(List<string> vars)
        {
            //We taste the variables in order to know the probable frequency of
            //the model.
            List<string> allFreqs = new List<string>() { "a", "q", "m", "w", "d", "u" };
            int[] allFreqsCounter = new int[allFreqs.Count];
            for (int i = 0; i < allFreqs.Count; i++)
            {
                foreach (string var in vars)
                {
                    Series ts1 = Program.databanks.GetFirst().GetIVariable(var + "!" + allFreqs[i]) as Series;
                    if (ts1 != null) allFreqsCounter[i]++;
                }
            }
            int maxValue = allFreqsCounter.Max();
            int maxIndex = allFreqsCounter.ToList().IndexOf(maxValue);
            return allFreqs[maxIndex];
        }

        private static void BrowserDependents(string varnameMaybeWithFreq, StringBuilder sb, ref string jName, ref bool jNameAutoGen)
        {
            string varnameWithoutFreq = G.Chop_RemoveFreq(varnameMaybeWithFreq);
            if (G.GetModelSourceType() == EModelType.Gekko)
            {
                List<string> list = new List<string>();
                if (Program.model.modelGekko.dependents.ContainsKey(varnameWithoutFreq))
                {
                    Dictionary<string, string> d2 = Program.model.modelGekko.dependents[varnameWithoutFreq].storage;
                    if (d2 != null)
                    {
                        foreach (string d3 in d2.Keys)
                        {
                            list.Add(d3);
                        }
                    }
                    list.Sort(StringComparer.InvariantCulture);
                }

                EquationHelper eq = Program.FindEquationByMeansOfVariableName(varnameWithoutFreq);

                if (eq == null)
                {
                    for (int i = 0; i < Program.model.modelGekko.equationsReverted.Count; i++)
                    {
                        EquationHelper eh = Program.model.modelGekko.equationsReverted[i];
                        if (G.Equal(varnameWithoutFreq, eh.lhs))
                        {
                            eq = eh;
                            break;
                        }
                    }
                }

                if (eq == null)
                {
                    for (int i = 0; i < Program.model.modelGekko.equationsNotRunAtAll.Count; i++)
                    {
                        EquationHelper eh = Program.model.modelGekko.equationsNotRunAtAll[i];
                        if (G.Equal(varnameWithoutFreq, eh.lhs))
                        {
                            eq = eh;
                            break;
                        }
                    }
                }

                if (eq != null && eq.equationCode != null)
                {
                    foreach (string s in eq.precedentsWithLagIndicator.Keys)
                    {
                        string jvar = null;
                        int lag = 0;
                        G.ExtractVariableAndLag(s, out jvar, out lag);
                        if (jvar.StartsWith("j", StringComparison.OrdinalIgnoreCase))
                        {
                            if (G.Contains(jvar, varnameWithoutFreq))
                            {
                                jName = jvar;
                                if (!G.Contains(eq.equationText, jvar))
                                {
                                    jNameAutoGen = true;
                                }
                                break;
                            }
                        }
                    }
                }

                if (eq != null && eq.modelBlock != null && eq.modelBlock != "" && eq.modelBlock != "Unnamed")
                {
                    WriteHtml(sb, "Modelblock: " + eq.modelBlock);
                }

                StringBuilder sb5 = new StringBuilder();
                sb5.Append("Påvirker: ");
                if (list.Count == 0) sb5.Append("<none>");
                else
                {

                    int counter = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string s = list[i];


                        sb5.Append(HtmlLink(s));


                        if (i < list.Count - 1) sb5.Append(", ");


                    }
                    sb5.AppendLine();

                }
                WriteHtml(sb, sb5.ToString());

                if (eq != null)
                {
                    StringBuilder sb2 = new StringBuilder();
                    if (eq.equationType == EEquationType.RevertedAutoGenerated || eq.equationType == EEquationType.RevertedP || eq.equationType == EEquationType.RevertedT || eq.equationType == EEquationType.RevertedY)
                    {
                        sb2.AppendLine("----------------------------------------------");
                        sb2.AppendLine("    Note that this equation is run *after*");
                        sb2.AppendLine("    the model itself is solved.");
                        sb2.AppendLine("----------------------------------------------");
                        sb2.AppendLine("");
                    }
                    string equationText = eq.equationText;
                    if (jNameAutoGen) equationText += G.NL + G.NL + "J-led: " + jName;
                    InsertLinksIntoEquation(equationText, true, sb2);
                    WriteHtmlPreCode(sb, sb2.ToString());
                }

            }
        }

        private static void BrowserCleanupFolders(string rootFolder, string varsFolder)
        {
            List<string> folders = new List<string> { rootFolder, varsFolder };

            foreach (string folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                else
                {
                    string[] files = Directory.GetFiles(folder + "\\");
                    if (files.Length > 0)
                    {
                        if (!G.IsUnitTesting())
                        {
                            DialogResult result = MessageBox.Show("All " + files.Length + " files in '" + folder + "' will be deleted", "Gekko helper", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            if (result == DialogResult.Yes)
                            {
                                //ok
                            }
                            else
                            {
                                new Error("User abort");
                                //throw new GekkoException();
                            }
                        }
                    }

                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        private static void FoldingButtonEnd(StringBuilder sb)
        {
            sb.AppendLine("</div>");
        }

        private static void FoldingButtonStart(StringBuilder sb, string buttonText)
        {
            int buttonId = Globals.foldingButtonCounter++;
            sb.AppendLine("<button onclick = `hide(" + buttonId + ")` style = `border-radius: 4px; padding: 4px; background-color: #009933; border: none; color: white; text-align: center; text-decoration: none; display: inline-block; font-size: 12px;  color:;`>" + buttonText + "</button>");
            sb.AppendLine("<div id = `b" + buttonId + "` style = `display: none;`>");
        }

        /// <summary>
        /// Extracts info from data generation file, to show as source (for instance y = x/z;).
        /// </summary>
        /// <returns></returns>
        private static GekkoDictionary<string, List<string>> BrowserDataGenerationExtract()
        {
            GekkoDictionary<string, List<string>> datagen = new GekkoDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            string genr = Program.GetTextFromFileWithWait(Program.options.folder_working + "\\" + "genr.gcm");

            int fat = 3;
            var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
            var tags2 = new List<string>() { "//" };
            List<TokenHelper> a = StringTokenizer.GetTokensWithLeftBlanks(genr, fat, tags1, tags2, null, null).storage;

            List<List<TokenHelper>> statements = new List<List<TokenHelper>>();

            int n = a.Count - fat;
            int start = 0;
            for (int i = 0; i < n; i++)
            {
                for (int ii = i; ii < n; ii++)
                {
                    if (a[ii].s == ";")
                    {
                        int i1 = i; //start token, may be EOL
                        for (int iii = i; iii <= ii; iii++)
                        {
                            if (a[iii].type != ETokenType.EOL && a[iii].type != ETokenType.Comment)
                            {
                                i1 = iii;
                                break;
                            }
                        }

                        int i2 = ii;  //end token, will be ';'

                        List<TokenHelper> th = new List<TokenHelper>();
                        for (int i3 = i1; i3 <= i2; i3++)
                        {
                            if (a[i3].s == null || a[i3].s == "") continue;
                            th.Add(a[i3]);
                        }
                        th.Add(new TokenHelper());
                        th.Add(new TokenHelper());
                        th.Add(new TokenHelper());
                        statements.Add(th);

                        i = ii;
                        break;
                    }
                }
            }

            for (int j = 0; j < statements.Count; j++)
            {
                List<TokenHelper> th = statements[j];

                if (IsNonSeriesStatement(th))
                {
                    continue;
                }

                Tuple<int, int> opt = StringTokenizer.FindOptionFieldInSeriesAssignment(th);
                int hasSeriesKeyword = 0;
                if (G.Equal(th[0].s, "ser") || G.Equal(th[0].s, "series")) hasSeriesKeyword = 1;
                string temp = null;
                int nameStart = -12345;
                int nameEnd = -12345;
                if (opt.Item1 == -12345)
                {
                    //no option field, "x = 1" OR "ser x = 1"
                    //if we start at hasSeriesKeyword, it is: "x = 1"
                    nameStart = hasSeriesKeyword;
                    nameEnd = StringTokenizer.FindS(th, nameStart + 1, "=");
                    if (nameEnd != -12345) nameEnd--;
                }
                else
                {
                    //option field, "<...> x = 1" OR "x <...> = 1" OR "ser <...> x = 1" OR "ser x <...> = 1"
                    //if we start at hasSeriesKeyword, it is: "<...> x = 1" OR "x <...> = 1"
                    if (th[hasSeriesKeyword].s == "<")
                    {
                        //option field before variable, for instance <...> x =  OR series <...> x = 
                        nameStart = opt.Item2 + 1;
                        nameEnd = StringTokenizer.FindS(th, nameStart + 1, "=");
                        if (nameEnd != -12345) nameEnd--;
                    }
                    else
                    {
                        //option field after variable, for instance x <...> = OR series x <...> = 
                        nameStart = hasSeriesKeyword;
                        nameEnd = StringTokenizer.FindS(th, nameStart + 1, "<");
                        if (nameEnd != -12345) nameEnd--;
                    }
                }

                if (nameStart != -12345 && nameEnd != -12345)
                {
                    bool lhsFunction = false;
                    if (nameEnd > nameStart + 1 && G.Equal(th[nameStart + 1].s, "(") && th[nameStart + 1].leftblanks == 0 && G.Equal(th[nameEnd].s, ")"))
                    {
                        if (G.Equal(th[nameStart].s, "log") || G.Equal(th[nameStart].s, "dlog") || G.Equal(th[nameStart].s, "dif") || G.Equal(th[nameStart].s, "diff"))
                        {
                            lhsFunction = true;
                            nameStart += 2;
                            nameEnd--;
                        }
                    }

                    if (nameStart == nameEnd && th[nameStart].type == ETokenType.Word)
                    {
                        //simple name
                        string name = th[nameStart].s;
                        string s3 = StringTokenizer.GetTextFromLeftBlanksTokens(th, 0, th.Count - 1, false).Trim();
                        BrowserAddItem(datagen, name.Trim(), s3.Trim());
                    }
                    else
                    {
                        //may be a composed name like x%i, x{%i}, x{i} or x[2000]

                        if (true)
                        {
                            //finding scalar vars in lhs name
                            GekkoDictionary<string, List<string>> scalarsOnLhsInSerStatement = new GekkoDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
                            for (int i = nameStart; i <= nameEnd; i++)
                            {
                                string mem = null;
                                if (BrowserIsScalar(th, i))
                                {
                                    //a%i or a{%i} or a{i}
                                    scalarsOnLhsInSerStatement.Add(th[i + 1].s, null);
                                }
                            }

                            //finding lists corresponding to scalar names

                            for (int jj = j - 1; jj >= 0; jj--)
                            {
                                List<TokenHelper> th2 = statements[jj];
                                if (G.Equal(th2[0].s, "for") && G.Equal(th2[1].s, "string") && th2[2].s == "%" && th2[3].type == ETokenType.Word && th2[4].s == "=" && scalarsOnLhsInSerStatement.ContainsKey(th2[3].s))
                                {
                                    //We have found the definition of one of the scalars in the lhs SERIES name.
                                    List<string> rhsVars = new List<string>();
                                    for (int i2 = 5; i2 < th2.Count; i2++)
                                    {
                                        if ((th2[i2].type == ETokenType.Word || th2[i2].type == ETokenType.QuotedString) && (th2[i2 + 1].s == "," || th2[i2 + 1].s == ";"))
                                        {
                                            rhsVars.Add(G.StripQuotes(th2[i2].s));
                                        }
                                    }
                                    scalarsOnLhsInSerStatement[th2[3].s] = rhsVars;
                                }
                            }

                            List<KeyValuePair<string, List<string>>> xx = new List<KeyValuePair<string, List<string>>>();
                            foreach (KeyValuePair<string, List<string>> xxx in scalarsOnLhsInSerStatement) xx.Add(xxx);

                            if (scalarsOnLhsInSerStatement.Count == 0)
                            {
                                //probably nothing to add, complicated name but no scalars found, for instance fy[2000] = ...
                                string name = th[nameStart].s;
                                string s3 = StringTokenizer.GetTextFromLeftBlanksTokens(th, 0, th.Count - 1, false).Trim();
                                BrowserAddItem(datagen, name.Trim(), s3.Trim());
                            }
                            else if (scalarsOnLhsInSerStatement.Count == 1 && xx[0].Value != null)
                            {
                                foreach (string listItem in xx[0].Value)
                                {

                                    string s7 = null;
                                    for (int i = nameStart; i <= nameEnd; i++)
                                    {
                                        if (th[i].s == "{" && th[i + 1].s == Globals.symbolScalar.ToString() && th[i + 2].type == ETokenType.Word && th[i + 2].leftblanks == 0 && th[i + 3].s == "}" && G.Equal(th[i + 2].s, xx[0].Key))
                                        {
                                            s7 += listItem;  //no blanks
                                            i += 3;
                                        }
                                        else
                                        {
                                            s7 += th[i].s;
                                        }
                                    }

                                    string s8 = null;
                                    for (int i = 0; i < th.Count; i++)
                                    {
                                        if (th[i].s == "{" && th[i + 1].s == Globals.symbolScalar.ToString() && th[i + 2].type == ETokenType.Word && th[i + 2].leftblanks == 0 && th[i + 3].s == "}" && G.Equal(th[i + 2].s, xx[0].Key))
                                        {
                                            s8 += G.Blanks(th[i].leftblanks) + listItem;  //with blanks
                                            i += 3;
                                        }
                                        else
                                        {
                                            s8 += G.Blanks(th[i].leftblanks) + th[i].s;
                                        }
                                    }

                                    BrowserAddItem(datagen, s7.Trim(), s8.Trim());
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
            return datagen;
        }

        public static bool IsNonSeriesStatement(List<TokenHelper> th)
        {
            return !G.Equal(th[0].s, "ser") && !G.Equal(th[0].s, "series") && Globals.commandNames.Contains(th[0].s, StringComparer.OrdinalIgnoreCase);
        }

        private static void BrowserAddItem(GekkoDictionary<string, List<string>> datagen, string name, string s3)
        {
            string[] ss = name.Split('[');
            if (ss.Length > 1) name = ss[0];

            if (datagen.ContainsKey(name))
            {
                datagen[name].Add(s3);
            }
            else
            {
                datagen.Add(name, new List<string>() { s3 });
            }
        }

        private static bool BrowserIsScalar(List<TokenHelper> th, int i)
        {
            //if token i+1 is a scalar name, %i or {i}
            if ((th[i].s == Globals.symbolScalar.ToString() && th[i + 1].type == ETokenType.Word && th[i + 1].leftblanks == 0) || (th[i].s == "{" && th[i + 1].type == ETokenType.Word && th[i + 2].s == "}"))
            {
                return true;
            }
            return false;
        }

        public static void WriteHtml(StringBuilder sb, string s)
        {
            sb.AppendLine("<p>" + s + "</p>");
        }

        public static void WriteHtmlBold(StringBuilder sb, string s)
        {
            sb.AppendLine("<p style=`font-weight: bold;`>" + s + "</p>");
        }

        public static void WriteHtmlColor(StringBuilder sb, string s)
        {
            sb.AppendLine("<p><font color=\"#009933\">" + s + "</font></p>");
        }


        private static void BrowserWritePrintLine(Series ts, StringBuilder sb3, GekkoTime gt)
        {
            //freq location
            //if (Program.options.freq == EFreq.A) sb3.Append((gt.super) + " ");
            //else sb3.Append(gt.super + ts.freq.ToString() + gt.sub + " ");
            sb3.Append(gt.ToString() + " ");

            double n1 = ts.GetDataSimple(gt);
            double n0 = ts.GetDataSimple(gt.Add(-1));

            double level1 = n1;
            double pch1 = ((n1 / n0 - 1) * 100d);

            if (n1 == n0) pch1 = 0d;

            string levelFormatted;
            string pchFormatted;
            Program.ConvertToPrintFormat(level1, pch1, out levelFormatted, out pchFormatted);

            sb3.Append(levelFormatted + " " + pchFormatted + " ");
        }

        public static void WriteHtmlPreCode(StringBuilder sb, string sb2)
        {
            sb.Append("<pre><code>"); sb.Append(sb2); sb.Append("</code></pre>");
        }

        public static string HtmlLink(string txt)
        {
            return HtmlLink(txt, txt.ToLower() + ".html");
        }

        public static string HtmlLink(string txt, string link)
        {
            return HtmlLink(txt, link, null);
        }

        public static string HtmlLink(string txt, string link, string tooltip)
        {
            string s = null;
            if (tooltip != null) s = " title = \"" + tooltip + "\"";
            return "<a href = \"" + link + "\"" + s + ">" + txt + "</a>";
        }

        private static void InsertLinksIntoEquation(string equationText, bool html, StringBuilder sb)
        {
            int widthRemember = -12345;
            if (!html)
            {
                widthRemember = Program.options.print_width;
                Program.options.print_width = int.MaxValue;
            }
            try
            {
                int fat = 20;
                List<TokenHelper> a = StringTokenizer.GetTokensWithLeftBlanks(equationText, fat, null, null, null, null).storage;
                //List<TokenHelper> a = GetTokensWithLeftBlanks(equationText, 20, false);

                int counter = -1;
                for (int i = 0; i < a.Count; i++)
                {

                    counter++;
                    //string s = tokens[i].s;
                    //if (s == "£") G.Writeln();


                    if (a[i].leftblanks > 0)
                    {
                        if (!html)
                        {
                            G.Write(G.Blanks(a[i].leftblanks));
                        }
                        else
                        {
                            sb.Append(G.Blanks(a[i].leftblanks));
                        }
                    }
                    if (counter > 1 && a[i].type == ETokenType.Word && Program.model.modelGekko.varsAType.ContainsKey(a[i].s))
                    {
                        if (!html)
                        {
                            G.WriteLink(a[i].s, "disp:" + a[i].s);
                        }
                        else
                        {
                            sb.Append(HtmlLink(a[i].s));
                        }
                    }
                    else
                    {
                        if (!html)
                        {
                            G.Write(a[i].s);
                        }
                        else
                        {
                            sb.Append(a[i].s);
                        }
                    }

                }
                if (!html)
                {
                    G.Writeln();
                }
            }
            finally
            {
                if (!html)
                {
                    //resetting, also if there is an error
                    Program.options.print_width = widthRemember;
                }
            }
        }
    }
}
