using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Gekko
{
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
                G.Writeln2("+++ WARNING: The .json file does not seem correctly formatted.");
                G.Writeln("             " + e.Message);
                //throw;
            }

            // -------------------------------------------------------------

            string settings_index_filename = null;
            try { settings_index_filename = (string)jsonTree["index_filename"]; } catch { }
            if (settings_index_filename == null)
            {
                G.Writeln2("*** ERROR: JSON: index_filename not found"); throw new GekkoException();
            }

            string settings_list_filename = null;
            try { settings_list_filename = (string)jsonTree["list_filename"]; } catch { }
            if (settings_list_filename == null)
            {
                G.Writeln2("*** ERROR: JSON: list_filename not found"); throw new GekkoException();
            }

            string settings_find_filename = null;
            try { settings_find_filename = (string)jsonTree["find_filename"]; } catch { }
            if (settings_find_filename == null)
            {
                G.Writeln2("*** ERROR: JSON: find_filename not found"); throw new GekkoException();
            }

            string settings_css_filename = null;
            try { settings_css_filename = (string)jsonTree["css_filename"]; } catch { }
            if (settings_css_filename == null)
            {
                G.Writeln2("*** ERROR: JSON: css_filename not found"); throw new GekkoException();
            }

            string settings_dok_filename = null;
            try { settings_dok_filename = (string)jsonTree["dok_filename"]; } catch { }
            if (settings_dok_filename == null)
            {
                G.Writeln2("*** ERROR: JSON: dok_filename not found"); throw new GekkoException();
            }

            string settings_est_filename = null;
            try { settings_est_filename = (string)jsonTree["est_filename"]; } catch { }
            if (settings_est_filename == null)
            {
                G.Writeln2("*** ERROR: JSON: est_filename not found"); throw new GekkoException();
            }

            string settings_icon_filename = null;
            try { settings_icon_filename = (string)jsonTree["icon_filename"]; } catch { }
            if (settings_icon_filename == null)
            {
                G.Writeln2("*** ERROR: JSON: icon_filename not found"); throw new GekkoException();
            }

            string settings_vars_foldername = null;
            try { settings_vars_foldername = (string)jsonTree["vars_foldername"]; } catch { }
            if (settings_vars_foldername == null)
            {
                G.Writeln2("*** ERROR: JSON: vars_foldername not found"); throw new GekkoException();
            }

            string settings_commands = null;
            try { settings_commands = (string)jsonTree["commands"]; } catch { }
            if (settings_commands == null)
            {
                G.Writeln2("*** ERROR: JSON: commands not found"); throw new GekkoException();
            }

            string settings_plot_start = null;
            try { settings_plot_start = (string)jsonTree["plot_start"]; } catch { }
            if (settings_plot_start == null)
            {
                G.Writeln2("*** ERROR: JSON: plot_start not found"); throw new GekkoException();
            }

            string settings_plot_end = null;
            try { settings_plot_end = (string)jsonTree["plot_end"]; } catch { }
            if (settings_plot_end == null)
            {
                G.Writeln2("*** ERROR: JSON: plot_end not found"); throw new GekkoException();
            }

            string settings_plot_line = null;
            try { settings_plot_line = (string)jsonTree["plot_line"]; } catch { }
            if (settings_plot_line == null)
            {
                G.Writeln2("*** ERROR: JSON: plot_line not found"); throw new GekkoException();
            }

            string settings_print_start = null;
            try { settings_print_start = (string)jsonTree["print_start"]; } catch { }
            if (settings_print_start == null)
            {
                G.Writeln2("*** ERROR: JSON: print_start not found"); throw new GekkoException();
            }

            string settings_print_end = null;
            try { settings_print_end = (string)jsonTree["print_end"]; } catch { }
            if (settings_print_end == null)
            {
                G.Writeln2("*** ERROR: JSON: print_end not found"); throw new GekkoException();
            }

            string include_p_type = null;
            try { include_p_type = (string)jsonTree["include_p_type"]; } catch { }
            if (include_p_type == null)
            {
                G.Writeln2("*** ERROR: JSON: include_p_type"); throw new GekkoException();
            }

            bool settings_show_source = true;
            try { settings_show_source = (bool)jsonTree["show_source"]; } catch { }

            object[] settings_ekstrafiler = null;
            try { settings_ekstrafiler = (object[])jsonTree["ekstrafiler"]; } catch { }
            if (settings_ekstrafiler == null)
            {
                G.Writeln2("*** ERROR: JSON: ekstrafiler"); throw new GekkoException();
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
                    throw new GekkoException();
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
                    G.Writeln2("*** ERROR: JSON: ekstrafiler"); throw new GekkoException();
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
                    throw new GekkoException();
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

            string bank1 = Path.GetFileName(Program.databanks.GetFirst().FileNameWithPath);
            string bank2 = Path.GetFileName(Program.databanks.GetRef().FileNameWithPath);

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

            if (false)
            {
                List<string> ss9 = new List<string>();
                foreach (string s in vars)
                {
                    if (s.ToLower().StartsWith("phk")) ss9.Add(s);
                }
                //MessageBox.Show(ss9);                
                List<EquationHelper> p = Program.model.modelGekko.equationsNotRunAtAll;
                List<EquationHelper> yt = Program.model.modelGekko.equationsReverted;
                //here we could use .lhsvariable and .precedentsWithLagIndicator to
                //obtain variables for p-type equations.
                return;
            }

            if (Globals.browserLimit)
            {
                vars = new List<string> { "aaa", "fcp", "PHK", "jphk", "fee", "Jfee", "fy", "tg", "peesq", "ktiorn", "tfon", "phk2", "phk3", "JNTPPIK" };  //phk2 is t-type, phk3 is p-type and JNTPPIK is y-type. The y-type is not shown
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

            GekkoDictionary<string, List<Tuple<string, string>>> doc = new GekkoDictionary<string, List<Tuple<string, string>>>(StringComparer.OrdinalIgnoreCase);
            string dokFileName = Program.options.folder_working + "\\" + settings_dok_filename;
            string dok2 = Program.GetTextFromFileWithWait(dokFileName);
            List<string> dok = G.ExtractLinesFromText(dok2);
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

            GekkoDictionary<string, List<string>> est2 = new GekkoDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            string est = Program.GetTextFromFileWithWait(Program.options.folder_working + "\\" + settings_est_filename);
            List<string> lines = G.ExtractLinesFromText(est);
            int listI = -12345;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Trim().StartsWith(Globals.ols1))
                {
                    if (listI != -12345)
                    {

                        int fat = 5;
                        var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
                        var tags2 = new List<string>() { "//" };
                        string depLine = lines[listI + 1].Trim();
                        depLine = depLine.Replace(Globals.ols2, "").Trim();
                        List<TokenHelper> a = StringTokenizer.GetTokensWithLeftBlanks(depLine, fat, tags1, tags2, null, null).storage;
                        
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

                        List<string> xx = new List<string>();
                        for (int ii = listI; ii < i; ii++)
                        {
                            xx.Add(lines[ii]);
                        }
                        if (est2.ContainsKey(varLine))
                        {
                            List<string> lines2 = est2[varLine];
                            lines2.Add("");
                            lines2.AddRange(xx);
                        }
                        else
                        {
                            est2.Add(varLine, xx);
                        }
                    }
                    listI = i;
                }
            }

            foreach (string var in vars)
            {
                StringBuilder sb = new StringBuilder();
                Series ts1 = Program.databanks.GetFirst().GetIVariable(var + "!a") as Series;

                if (ts1 == null)
                {
                    G.Writeln2("*** ERROR: Could not find series " + var + " in databank " + Program.databanks.GetFirst().name);
                }

                Series ts2 = Program.databanks.GetRef().GetIVariable(var + "!a") as Series;
                string jName = null;  //name of possible j-led
                bool jNameAutoGen = false;

                sb.AppendLine("<table cellpadding = `0` cellspacing = `0` width = `800px` border = `0`>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td width = `80%`><big><b> " + var + "</b></big></td>");
                sb.AppendLine("<td width = `10%`><a href=`..\\" + settings_find_filename + "`>Søg</a></td>");
                sb.AppendLine("<td width = `10%`><a href=`..\\" + settings_index_filename + "`>Hjem</a></td>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</table>");

                //List<string> varExpl = Program.GetVariableExplanation(var);
                //string explanation2 = Program.GetVariableExplanationAugmented(var, G.ExtractOnlyVariableIgnoreLag(var, Globals.leftParenthesisIndicator)).Trim();
                List<string> varExpl = Program.GetVariableExplanationAugmented(var);

                foreach (string line in varExpl)
                {
                    if (line != "")
                    {
                        string line2 = Program.SpecialXmlChars(line);
                        WriteHtmlColor(sb, line2);
                    }
                }

                string explanation = null;
                if (varExpl != null && varExpl.Count > 0) explanation = G.HandleQuoteInQuote(varExpl[0], true);
                vars2.Add(var + "¤" + explanation);

                StringBuilder sb4 = new StringBuilder();

                if (true)
                {
                    EEndoOrExo type1 = Program.VariableTypeEndoExo(var);
                    string type = "";
                    if (type1 == EEndoOrExo.Exo) type = "Eksogen, ";
                    else if (type1 == EEndoOrExo.Endo) type = "Endogen, ";

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
                    else if (ts1.freq == EFreq.U)
                    {
                        freq = "Udateret";
                    }

                    bool noData = ts1.IsNullPeriod(); //We are opening up to this possibility of 'empty' data

                    //GekkoTime first = ts.GetPeriodFirst();
                    //GekkoTime last = ts.GetPeriodLast();

                    GekkoTime first = ts1.GetRealDataPeriodFirst();
                    GekkoTime last = ts1.GetRealDataPeriodLast();

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
                    WriteHtml(sb, sb4.ToString());

                }

                if (ts1.meta.label != null) WriteHtml(sb, "Label: " + ts1.meta.label);

                if (settings_show_source)
                {
                    if (ts1.meta.source != null)
                    {
                        //We keep the SERIES (or SER), there may be options etc. But we capitalize it.
                        string src2 = ts1.meta.source.Trim();
                        if (src2 != "")
                        {
                            int i = src2.IndexOf("(hash ");
                            if (i > -1)
                                src2 = src2.Substring(0, i);
                            WriteHtml(sb, "Seneste beregning: " + src2);
                        }
                    }
                }

                if (ts1.meta.units != null)
                {
                    //We keep the SERIES (or SER), there may be options etc. But we capitalize it.
                    string src2 = ts1.meta.units.Trim();
                    if (src2 != "")
                    {
                        WriteHtml(sb, "Enheder: " + src2);
                    }
                }

                List<Tuple<string, string>> tuples = null; doc.TryGetValue(var, out tuples);
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
                        //sb.Append("<td width = `1%`>" + s + "</td>");
                        //sb.Append("<td width = `99%`><a href = `" + tuple.Item1 + "`>" + tuple.Item2 + "</a></td>");
                        sb.Append("<td>" + s + "</td>");
                        sb.Append("<td><a href = `" + tuple.Item1 + "`>" + tuple.Item2 + "</a></td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                }


                if (G.HasModelGekko())
                {
                    List<string> list = new List<string>();
                    if (Program.model.modelGekko.dependents.ContainsKey(var))
                    {
                        Dictionary<string, string> d2 = Program.model.modelGekko.dependents[var].storage;
                        if (d2 != null)
                        {
                            foreach (string d3 in d2.Keys)
                            {
                                list.Add(d3);
                            }
                        }
                        list.Sort(StringComparer.InvariantCulture);
                    }

                    EquationHelper eq = Program.FindEquationByMeansOfVariableName(var);

                    if (eq == null)
                    {
                        for (int i = 0; i < Program.model.modelGekko.equationsReverted.Count; i++)
                        {
                            EquationHelper eh = Program.model.modelGekko.equationsReverted[i];
                            if (G.Equal(var, eh.lhs))
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
                            if (G.Equal(var, eh.lhs))
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
                                if (G.Contains(jvar, var))
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

                string xxx = null;
                if (est2.ContainsKey(var))
                {
                    List<string> xx = est2[var];
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

                List<string> datagen2 = null; datagen.TryGetValue(var, out datagen2);
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

                string l1 = bank1.ToLower().Replace(".gbk", "") + ":" + var;
                string l2 = bank2.ToLower().Replace(".gbk", "") + ":" + var;

                if (ts2 == null)
                {
                    //only plot the series from Work
                    Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " > " + var + " '" + l1 + "' file=" + subFolder + "\\" + var.ToLower() + ".svg;", "", 0, new P());
                    Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " yminhard = -100 ymaxhard = 100 yminsoft = -1 ymaxsoft = 1  p> " + var + " '" + l1 + "' file=" + subFolder + "\\" + var.ToLower() + "___p" + ".svg;", "", 0, new P());
                }
                else
                {
                    Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " > @" + var + " '" + l2 + "' <type = lines dashtype = '3'>, " + var + " '" + l1 + "' file=" + subFolder + "\\" + var.ToLower() + ".svg;", "", 0, new P());
                    Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " yminhard = -100 ymaxhard = 100 yminsoft = -1 ymaxsoft = 1  p> @" + var + " '" + l2 + "' <type = lines dashtype = '3'>, " + var + " '" + l1 + "' file=" + subFolder + "\\" + var.ToLower() + "___p" + ".svg;", "", 0, new P());
                }

                sb.AppendLine("<img src = `" + var.ToLower() + ".svg" + "`>");

                sb.AppendLine("<p/>");

                FoldingButtonStart(sb, "Vækst %");
                sb.AppendLine("<img src = `" + var.ToLower() + "___p.svg" + "`>");
                FoldingButtonEnd(sb);

                if (jName != null)
                {

                    FoldingButtonStart(sb, "J-led");
                    sb.AppendLine("<img src = `" + jName.ToLower() + ".svg" + "`>");
                    FoldingButtonEnd(sb);
                }

                if (true)
                {

                    StringBuilder sb3 = new StringBuilder();
                    sb3.AppendLine(bank1 + G.Blanks(30 - bank1.Length + gap) + bank2);
                    sb3.AppendLine();
                    sb3.AppendLine("Period        value        %  " + G.Blanks(gap) + "Period        value        %  ");
                    int counter = 0;
                    foreach (GekkoTime gt in new GekkoTimeIterator(print_start, print_end))
                    {
                        counter++;
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
                    }

                    WriteHtmlPreCode(sb, sb3.ToString());

                }

                StringBuilder x = new StringBuilder();
                x.AppendLine("<!DOCTYPE HTML PUBLIC `-//W3C//DTD HTML 4.01 Transitional//EN`>");
                x.AppendLine("<html>");
                x.AppendLine("  <head>");
                x.AppendLine("    <link rel=`stylesheet` href=`..\\" + settings_css_filename + @"` type=`text/css`>");
                x.AppendLine("    <link rel = `shortcut icon` href = `..\\" + settings_icon_filename + "` type = `image/vnd.microsoft.icon`>");
                x.AppendLine("    <meta http-equiv=`Content-Type` content=`text/html; charset=iso-8859-1`>");
                x.AppendLine("    <title>" + var + "</title>");
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

                string pathAndFilename = subFolder + "\\" + var.ToLower() + ".html";
                using (FileStream fs = Program.WaitForFileStream(pathAndFilename, Program.GekkoFileReadOrWrite.Write))
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
                List<string> varExpl = Program.GetVariableExplanation(var2);
                string expl = "";
                if (varExpl != null && varExpl.Count > 0) expl = Program.SpecialXmlChars(varExpl[0]);

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
            using (FileStream fs = Program.WaitForFileStream(pathAndFilename2, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter sw = G.GekkoStreamWriter(fs))
            {
                sw.Write(x2.Replace('`', '\"'));
            }


            // ----------------- find -------------------------------------

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
                var varnavn = [" + s1.ToLower() + @"];
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

                        " + write + @"(`<b><a href=" + settings_vars_foldername + @"/` + varnavn[i] + `.html style='text-decoration:none'>` + varnavn[i] + `</a></b>`);
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
                            " + write + @"(`<a href=" + settings_vars_foldername + @"/` + varnavn[i] + `.html style='text-decoration:none;'>` + varnavn[i] + `</a>`);
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
                    " + write + @"(`<b><a href=" + settings_vars_foldername + @"/` + varnavn[i] + `.html style='text-decoration:none'>` + varnavn[i] + `</a></b>`);
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
            using (FileStream fs = Program.WaitForFileStream(pathAndFilename3, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter sw = G.GekkoStreamWriter(fs))
            {
                sw.Write(x3.Replace('`', '\"'));
            }

            G.Writeln2("End of html browser generation, " + G.Seconds(dt0));

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
                                throw new GekkoException();
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

        private static GekkoDictionary<string, List<string>> BrowserDataGenerationExtract()
        {
            GekkoDictionary<string, List<string>> datagen = new GekkoDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            string genr = Program.GetTextFromFileWithWait(Program.options.folder_working + "\\" + "genr.gcm");

            int fat = 3;
            var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
            var tags2 = new List<string>() { "//" };
            List<TokenHelper> a = StringTokenizer.GetTokensWithLeftBlanks(genr, fat, tags1, tags2, null, null).storage;
            //List<TokenHelper> a = Program.GetTokensWithLeftBlanks(genr, fat, true);

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
                        string s3 = StringTokenizer.GetTextFromLeftBlanksTokens(th, 0, th.Count - 1).Trim();
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
                                string s3 = StringTokenizer.GetTextFromLeftBlanksTokens(th, 0, th.Count - 1).Trim();
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
                                //G.Writeln2("*** ERROR: Nested FOR not supported yet");
                                //throw new GekkoException();
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
            if (Program.options.freq == EFreq.A) sb3.Append((gt.super) + " ");
            else sb3.Append(gt.super + ts.freq.ToString() + gt.sub + " ");

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
            int fileWidthRemember = -12345;
            if (!html)
            {
                widthRemember = Program.options.print_width;
                fileWidthRemember = Program.options.print_filewidth;
                Program.options.print_width = int.MaxValue;
                Program.options.print_filewidth = int.MaxValue;
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
                    Program.options.print_filewidth = fileWidthRemember;
                }
            }
        }





    }
}
