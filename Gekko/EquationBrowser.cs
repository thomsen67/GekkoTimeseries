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
                new Warning("The .json file does not seem correctly formatted. " + e.Message);
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
                else
                {
                    vars = new List<string> { "aaa", "fcp", "PHK", "jphk", "fee", "Jfee", "fy", "tg", "peesq", "ktiorn", "tfon", "phk2", "phk3", "JNTPPIK" };  //phk2 is t-type, phk3 is p-type and JNTPPIK is y-type. The y-type is not shown
                }
                Globals.browserLimit = false;  //for safety
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

            List<string> vars2 = new List<string>();

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

            foreach (string varnameWithoutFreq in vars)
            {
                string varnameWithFreq = varnameWithoutFreq + "!" + modelFrequencyString;
                StringBuilder sb = new StringBuilder();
                Series ts1 = Program.databanks.GetFirst().GetIVariable(varnameWithFreq) as Series;

                if (ts1 == null)
                {
                    new Error("Could not find series " + varnameWithFreq + " in databank " + Program.databanks.GetFirst().name);
                }

                Series ts2 = Program.databanks.GetRef().GetIVariable(varnameWithFreq) as Series;
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
                vars2.Add(varnameWithoutFreq + "¤" + explanation);

                // --------------------------------
                // html print info on ENDO/EXO, freq, data period
                // --------------------------------

                EEndoOrExo type1 = Program.VariableTypeEndoExo(varnameWithFreq);
                string type = "";
                if (type1 == EEndoOrExo.Exo) type = "Eksogen, ";
                else if (type1 == EEndoOrExo.Endo) type = "Endogen, ";

                //========================================================================================================
                //                          FREQUENCY LOCATION, indicates where to implement more frequencies
                //========================================================================================================

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
                sb4.Append(type);
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
                
                // --------------------------------
                // print link(s) to possible external documentation files
                // --------------------------------

                List<Tuple<string, string>> tuples = null; doc.TryGetValue(varnameWithoutFreq, out tuples);
                if (tuples != null)
                {
                    int counter = -1;
                    sb.Append("<table style=`margin: 0px; padding: 0px; border: 0px; width = 800px;`>");
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

                if (ts2 == null)
                {
                    //only plot the series from Work
                    Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " > " + varnameWithoutFreq + " '" + l1 + "' file=" + subFolder + "\\" + varnameWithoutFreq.ToLower() + ".svg;", "", 0, new P());
                    Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " yminhard = -100 ymaxhard = 100 yminsoft = -1 ymaxsoft = 1  p> " + varnameWithoutFreq + " '" + l1 + "' file=" + subFolder + "\\" + varnameWithoutFreq.ToLower() + "___p" + ".svg;", "", 0, new P());
                }
                else
                {
                    Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " > @" + varnameWithoutFreq + " '" + l2 + "' <type = lines dashtype = '3'>, " + varnameWithoutFreq + " '" + l1 + "' file=" + subFolder + "\\" + varnameWithoutFreq.ToLower() + ".svg;", "", 0, new P());
                    Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " yminhard = -100 ymaxhard = 100 yminsoft = -1 ymaxsoft = 1  p> @" + varnameWithoutFreq + " '" + l2 + "' <type = lines dashtype = '3'>, " + varnameWithoutFreq + " '" + l1 + "' file=" + subFolder + "\\" + varnameWithoutFreq.ToLower() + "___p" + ".svg;", "", 0, new P());
                }

                sb.AppendLine("<img src = `" + varnameWithoutFreq.ToLower() + ".svg" + "`>");

                sb.AppendLine("<p/>");

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

            vars2.Sort(StringComparer.OrdinalIgnoreCase);

            StringBuilder x3 = new StringBuilder();
            x3.AppendLine("<html>");
            x3.AppendLine("<head>");
            x3.AppendLine("<link rel = `stylesheet` href = `" + settings_css_filename + "` type = `text/css` >");
            x3.AppendLine("<link rel = `shortcut icon` href = `" + settings_icon_filename + "` type = `image/vnd.microsoft.icon`>");
            x3.AppendLine("</head>");

            //x3.AppendLine("<script LANGUAGE = `JavaScript` SRC = `variable.js` ></script>");
            x3.AppendLine("<script LANGUAGE = `JavaScript` > <!-- ");

            string s1 = null;
            string s2 = null;
            foreach (string s in vars2)
            {
                string[] ss = s.Split('¤');
                s1 += "`" + ss[0] + "`" + ", ";
                s2 += "`" + ss[1] + "`" + ", ";
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

            G.Writeln2("End of html browser generation, " + G.Seconds(dt0));

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
                        //skip a #x or %x                                 }
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

        private static void WriteHtmlPreCode(StringBuilder sb, string sb2)
        {
            sb.Append("<pre><code>"); sb.Append(sb2); sb.Append("</code></pre>");
        }

        private static string HtmlLink(string txt)
        {
            return HtmlLink(txt, txt.ToLower() + ".html");
        }

        private static string HtmlLink(string txt, string link)
        {
            return "<a href = \"" + link + "\" >" + txt + "</a>";
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
