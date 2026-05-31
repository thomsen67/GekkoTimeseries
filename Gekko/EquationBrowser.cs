using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Gekko
{
    /// <summary>
    /// Used to store info on how label, unit, etc. is shown in EquationBrower (html). In this class so not to pollute anything...
    /// </summary>
    public class HtmlBrowserSettings
    {
        public bool isDanish = true;
        public bool show_source = false;
    }

    public class BrowserHelper
    {
        //public StringBuilder html = null;
        public int depthMax = -1;
        public int counter = 0;
        public int counterMax = -1;
        public int pixels = 0;
        public int pixelsAfterArrow = 30;
        public int firstColWidth = 200;
        public EFreq freq = EFreq.A;
        public int plotTypes = 2;  //2 = n and p
        public bool removeTx0Dollar = false;
        public int maxPages = int.MaxValue;
        // ---
        public EquationBrowser.EBrowserType type = EquationBrowser.EBrowserType.Makro;
        public StringBuilder text = null;
    }

    public class EquationNameAndNumber
    {
        public int i;
        public string name;
    }

    public class EquationBrowserHelper
    {
        public string s1;
        public string s2;
    }

    public static class EquationBrowser
    {
        public enum EBrowserType
        {
            Adam,
            Greu,
            Makro,
            MakroIdentitiesText            
        }
        
        public static void Browser()
        {
            bool isSimple = false;
            bool jsmFix = true;
            bool isDanish = true;

            string settings_index_filename = null;
            string settings_list_filename = null;
            string settings_find_filename = null;
            string settings_css_filename = null;
            string settings_dok_filename = null;
            string settings_est_filename = null;
            string settings_icon_filename = null;
            string settings_vars_foldername = null;
            string settings_commands = null;
            string settings_plot_start = null;
            string settings_plot_end = null;
            string settings_plot_line = null;
            string settings_print_start = null;
            string settings_print_end = null;
            string settings_include_p_type = null;
            bool settings_show_source = true;
            object[] settings_ekstrafiler = null;

            G.Writeln2("Starting html browser generation");
            DateTime dt0 = DateTime.Now;

            string pathAndFile = Program.options.folder_working + "\\" + "browser.json";
            string jsonCode = null;
            if (!File.Exists(pathAndFile))
            {
                isSimple = true;
                isDanish = false;
                new Note("A '" + pathAndFile + "' file does not seem to exist: because of this, a basic/default browser is generated");
                settings_index_filename = "index.html";
                settings_list_filename = "list.html";
                settings_find_filename = "find.html";
                settings_css_filename = "styles.css";
                settings_vars_foldername = "vars";
                settings_plot_start = Globals.globalPeriodStart.super.ToString();
                settings_plot_end = Globals.globalPeriodEnd.super.ToString();
                settings_print_start = Globals.globalPeriodStart.super.ToString();
                settings_print_end = Globals.globalPeriodEnd.super.ToString();
            }
            else
            {
                jsonCode = G.RemoveComments(Program.GetTextFromFileWithWait(pathAndFile));
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


                try { settings_index_filename = (string)jsonTree["index_filename"]; } catch { }
                if (settings_index_filename == null)
                {
                    new Error("JSON: index_filename not found");
                }

                try { settings_list_filename = (string)jsonTree["list_filename"]; } catch { }
                if (settings_list_filename == null)
                {
                    new Error("JSON: list_filename not found");
                }

                try { settings_find_filename = (string)jsonTree["find_filename"]; } catch { }
                if (settings_find_filename == null)
                {
                    new Error("JSON: Find_filename not found");
                }

                try { settings_css_filename = (string)jsonTree["css_filename"]; } catch { }
                if (settings_css_filename == null)
                {
                    new Error("JSON: css_filename not found");
                }

                try { settings_dok_filename = (string)jsonTree["dok_filename"]; } catch { }
                if (settings_dok_filename == null)
                {
                    new Error("JSON: dok_filename not found");
                }

                try { settings_est_filename = (string)jsonTree["est_filename"]; } catch { }
                if (settings_est_filename == null)
                {
                    new Error("JSON: est_filename not found");
                }

                try { settings_icon_filename = (string)jsonTree["icon_filename"]; } catch { }
                if (settings_icon_filename == null)
                {
                    new Error("JSON: icon_filename not found");
                }

                try { settings_vars_foldername = (string)jsonTree["vars_foldername"]; } catch { }
                if (settings_vars_foldername == null)
                {
                    new Error("JSON: vars_foldername not found");
                }

                try { settings_commands = (string)jsonTree["commands"]; } catch { }
                if (settings_commands == null)
                {
                    new Error("JSON: commands not found");
                }

                try { settings_plot_start = (string)jsonTree["plot_start"]; } catch { }
                if (settings_plot_start == null)
                {
                    new Error("JSON: plot_start not found");
                }

                try { settings_plot_end = (string)jsonTree["plot_end"]; } catch { }
                if (settings_plot_end == null)
                {
                    new Error("JSON: plot_end not found");
                }

                try { settings_plot_line = (string)jsonTree["plot_line"]; } catch { }
                if (settings_plot_line == null)
                {
                    new Error("Plot_line not found");
                }

                try { settings_print_start = (string)jsonTree["print_start"]; } catch { }
                if (settings_print_start == null)
                {
                    new Error("Print_start not found");
                }

                try { settings_print_end = (string)jsonTree["print_end"]; } catch { }
                if (settings_print_end == null)
                {
                    new Error("Print_end not found");
                }

                try { settings_include_p_type = (string)jsonTree["include_p_type"]; } catch { }
                if (settings_include_p_type == null)
                {
                    new Error("Include_p_type");
                }

                try { settings_show_source = (bool)jsonTree["show_source"]; } catch { }

                try { settings_ekstrafiler = (object[])jsonTree["ekstrafiler"]; } catch { }
                if (settings_ekstrafiler == null)
                {
                    new Error("JSON: ekstrafiler problem");
                }
            }

            // -------------------------------------------------------------
            // -------------------------------------------------------------
                        
            string ss1 = "Søg";
            string ss2 = "Hjem";
            if (!isDanish)
            {
                ss1 = "Search";
                ss2 = "Home";
            }

            string list_title = "Variabelliste (try Ctrl+F)";
            if (!isDanish) list_title = "Variable list (try Ctrl+F)";

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

            if (!isSimple)
            {

                //index.html and styles.css is copied to root folder of browser system
                List<string> filesToCopy = new List<string>();
                filesToCopy.Add(settings_index_filename);
                filesToCopy.Add(settings_css_filename);
                filesToCopy.Add(settings_icon_filename);
                if (settings_ekstrafiler != null)
                {
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
                }

                foreach (string fileToCopy in filesToCopy)
                {
                    if (fileToCopy == null) continue;
                    string fileNameIndex = Program.options.folder_working + "\\" + fileToCopy;
                    string fileNameIndex2 = rootFolder + "\\" + fileToCopy;
                    if (!File.Exists(fileNameIndex))
                    {
                        new Error("'" + fileNameIndex + "' was not found");
                    }
                    File.Copy(fileNameIndex, fileNameIndex2, true);
                }

                Program.RunGekkoCommands(settings_commands, "", 0, new P());
            }

            int gap = 20;

            GekkoTime plotStart = new GekkoTime(EFreq.A, G.IntParse(settings_plot_start), 1);
            GekkoTime plotEnd = new GekkoTime(EFreq.A, G.IntParse(settings_plot_end), 1);
            GekkoTime plot_line = GekkoTime.tNull;
            if (isSimple) plot_line = plotStart.Add(-100); //-100 so it does not show up
            else plot_line = new GekkoTime(EFreq.A, G.IntParse(settings_plot_line), 1);
            GekkoTime print_start = new GekkoTime(EFreq.A, G.IntParse(settings_print_start), 1);
            GekkoTime print_end = new GekkoTime(EFreq.A, G.IntParse(settings_print_end), 1);

            string bank1 = Path.GetFileName(Program.databanks.GetFirst().FileNameWithPathPretty);
            string bank2 = Path.GetFileName(Program.databanks.GetRef().FileNameWithPathPretty);

            List ml = O.GetIVariableFromString("#all", O.ECreatePossibilities.NoneReportError, true) as List;
            List<string> vars = Stringlist.GetListOfStringsFromIVariable(ml);

            if (G.Equal(settings_include_p_type, "yes"))
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
                if (isSimple || settings_index_filename.ToLower().Contains("adam"))
                {
                    vars = new List<string> { "fy", "ul", "pcp", "tg" };
                }
                else if (settings_index_filename.ToLower().Contains("mona"))
                {
                    vars = new List<string> { "FY", "FCB", "PCB_LA", "FCH", "PCH_LA", "FCQ", "PCQ_LA", "PCOV_LA", "FCOV", "PCOW_LA", "FCOW", "PIOV_LA", "FIOV", "FIPMXE", "PIPMXE_LA", "FIY", "PIY_LA", "FIEM", "PIEM_LA", "FIH", "PIH_LA", "FMY", "PMY_LA", "PY_LA" };
                }
                else
                {
                    //smec
                    vars = new List<string> { "aaa", "fcp", "PHK", "jphk", "fee", "Jfee", "fy", "tg", "peesq", "ktiorn", "tfon", "phk2", "phk3", "JNTPPIK" };  //phk2 is t-type, phk3 is p-type and JNTPPIK is y-type. The y-type is not shown
                }
            }
            else if (Globals.runningOnTTComputer)
            {
                DialogResult result = MessageBox.Show("Only a few vars?", "vars", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes)
                {
                    vars = new List<string> { "aaa", "fcp", "PHK", "jphk", "fee", "Jfee", "fy", "tg", "peesq", "ktiorn", "tfon" };
                }
            }

            vars.Sort(StringComparer.OrdinalIgnoreCase);

            List<EquationBrowserHelper> vars2 = new List<EquationBrowserHelper>();
            GekkoDictionary<string, List<string>> datagen = new GekkoDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            GekkoDictionary<string, List<Tuple<string, string>>> doc = new GekkoDictionary<string, List<Tuple<string, string>>>(StringComparer.OrdinalIgnoreCase);
            GekkoDictionary<string, List<string>> est2 = new GekkoDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);


            if (isSimple)
            {
                string s = $@"
<HTML><HEAD><TITLE>Equation browser</TITLE>
<link rel = `stylesheet` href = `styles.css` type = `text/css`>
</HEAD>
<BODY>
<P><b><FONT size = +2>Equation browser</font></a></b></P>
<P>Among other things, the equation browser shows how model equations affect each other. The browser also shows graphs and prints of variable values.</p>
<P><b><a href = find.html><FONT size = +0.5> {G.FirstCharToUpper(ss1)} </font></a></b></P>
<P><b><a href = list.html><FONT size = +0.5> List </font></a></b></P>
</BODY></HTML>
";

                string pathAndFilename = browserFolder + "\\" + "index.html";
                using (FileStream fs = Program.WaitForFileStream(pathAndFilename, null, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    sw.Write(s.Replace('`', '\"'));
                }

                s = @"
body, table {color: #000000;
  font-family: Verdana;
  font-size: 10pt;
  font-style: normal;
  font-variant: normal;
  background-color: white;
  padding:20px;
}

a {text-decoration: none;
}

a:link {color:#0645AD;
}

a:visited {color:#0645AD;
}

a:hover {color:#3366BB;
}

code {!background: hsl(220, 80%, 90%);
}


pre {height: auto;
    /* max-height: 200px; */
    max-width: 800px;
    overflow: auto;
    background-color: #f8f8f8;
    word-break: normal !important;
    word-wrap: normal !important;
    white-space: pre !important;
    padding-top: 10px;
    padding-bottom: 10px;
    padding-left: 10px;
    padding-right: 10px;
}

img {border-style: none;
    max-width: 425px;
    height: auto;
}
";





                pathAndFilename = browserFolder + "\\" + "styles.css";
                using (FileStream fs = Program.WaitForFileStream(pathAndFilename, null, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    sw.Write(s.Replace('`', '\"'));
                }
            }
            else
            {


                // -------------------------------------------
                // Data generation
                // -------------------------------------------

                datagen = BrowserDataGenerationExtract();

                // -------------------------------------------
                // Html
                // -------------------------------------------

                //Fetches info on external documents that contain read-more info on particular variables

                doc = new GekkoDictionary<string, List<Tuple<string, string>>>(StringComparer.OrdinalIgnoreCase);
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

                //Fetches estimation output
                est2 = new GekkoDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
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
            }

            string modelFrequencyString = GetModelFreq(vars);
            Program.options.freq = G.ConvertFreq(modelFrequencyString); //sets global freq

            if (Globals.browserLimit)
            {
                if (isSimple || settings_index_filename.ToLower().Contains("adam"))
                {
                }
                else if (settings_index_filename.ToLower().Contains("mona"))
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
                    if (isSimple || settings_index_filename.ToLower().Contains("adam"))
                    {
                    }
                    else if (settings_index_filename.ToLower().Contains("mona"))
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
                sb.AppendLine("<td width = `10%`><a href=`..\\" + settings_find_filename + "`>" + ss1 + "</a></td>");
                sb.AppendLine("<td width = `10%`><a href=`..\\" + settings_index_filename + "`>" + ss2 + "</a></td>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</table>");

                // --------------------------------
                // html print green explanations here. Includes name, label, source, units -- and may also include raw lines from external varlist.dat file (they are shown first, if present)
                // --------------------------------

                HtmlBrowserSettings htmlBrowserSettings = new HtmlBrowserSettings();
                htmlBrowserSettings.isDanish = isDanish;
                htmlBrowserSettings.show_source = settings_show_source;
                List<string> varExpl = Program.GetVariableExplanationAugmented(varnameWithFreq, htmlBrowserSettings, false);
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
                if (isDanish)
                {
                    if (type1 == EEndoOrExo.Exo) type = "Eksogen";
                    else if (type1 == EEndoOrExo.Endo) type = "Endogen";
                }
                else
                {
                    if (type1 == EEndoOrExo.Exo) type = "Exogenous";
                    else if (type1 == EEndoOrExo.Endo) type = "Endogenous";
                }

                //========================================================================================================
                //                          FREQUENCY LOCATION, indicates where to implement more frequencies
                //========================================================================================================

                if (ts1 != null)
                {
                    string freq = null;
                    if (isDanish)
                    {
                        freq = "[ukendt frekvens]";
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
                    }
                    else
                    {
                        freq = "[unknown freq]";
                        if (ts1.freq == EFreq.A)
                        {
                            freq = "Annual";
                        }
                        else if (ts1.freq == EFreq.Q)
                        {
                            freq = "Quarterly";
                        }
                        else if (ts1.freq == EFreq.M)
                        {
                            freq = "Monthly";
                        }
                        else if (ts1.freq == EFreq.W)
                        {
                            freq = "Weekly";
                        }
                        else if (ts1.freq == EFreq.D)
                        {
                            freq = "Daily";
                        }
                        else if (ts1.freq == EFreq.U)
                        {
                            freq = "Undated";
                        }
                    }

                    bool noData = ts1.IsNullPeriod(); //We are opening up to this possibility of 'empty' data

                    GekkoTime first = ts1.GetRealDataPeriodFirst();
                    GekkoTime last = ts1.GetRealDataPeriodLast();

                    StringBuilder sb4 = new StringBuilder();
                    sb4.Append(type + ", ");
                    string stamp = null;
                    string ss3 = "opdateret";
                    if (!isDanish) ss3 = "updated";
                    if (ts1.meta.stamp != null && ts1.meta.stamp != "") stamp = " (" + ss3 + ": " + ts1.meta.stamp + ")";
                    if (ts1.freq == EFreq.A || ts1.freq == EFreq.U)
                    {
                        if (noData || first.super == -12345 || last.super == -12345)
                        {
                            if (isDanish) sb4.Append(freq + ", ingen dataperiode");
                            else sb4.Append(freq + ", no data period");
                        }
                        else
                        {
                            //we don't want 1995a1 to 2005a1, instead 1995 to 2005
                            if (isDanish) sb4.Append(freq + " data fra " + first.super + " til " + last.super + stamp);
                            else sb4.Append(freq + " data from " + first.super + " to " + last.super + stamp);
                        }
                    }
                    else
                    {
                        if (noData || first.super == -12345 || last.super == -12345)
                        {
                            if (isDanish) sb4.Append(freq + ", ingen dataperiode");
                            else sb4.Append(freq + ", no data period");
                        }
                        else
                        {
                            if (isDanish) sb4.Append(freq + " data fra " + first.super + ts1.freq.ToString() + first.sub + " til " + last.super + ts1.freq.ToString() + last.sub + stamp);
                            else sb4.Append(freq + " data from " + first.super + ts1.freq.ToString() + first.sub + " to " + last.super + ts1.freq.ToString() + last.sub + stamp);
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

                BrowserDependents(varnameWithFreq, sb, isDanish, ref jName, ref jNameAutoGen);

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
                string l2 = null;
                if (bank2 != null) l2 = bank2.ToLower().Replace(".gbk", "") + ":" + varnameWithoutFreq;

                if (ts1 != null)
                {
                    if (ts2 == null)
                    {
                        //only plot the series from Work
                        Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " > " + varnameWithoutFreq + " '" + l1 + "' file='" + subFolder + "\\" + varnameWithoutFreq.ToLower() + ".svg';", "", 0, new P());
                        Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " yminhard = -100 ymaxhard = 100 yminsoft = -1 ymaxsoft = 1  p> " + varnameWithoutFreq + " '" + l1 + "' file='" + subFolder + "\\" + varnameWithoutFreq.ToLower() + "___p" + ".svg';", "", 0, new P());
                    }
                    else
                    {
                        //plot both
                        Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " > @" + varnameWithoutFreq + " '" + l2 + "' <type = lines dashtype = '3'>, " + varnameWithoutFreq + " '" + l1 + "' file='" + subFolder + "\\" + varnameWithoutFreq.ToLower() + ".svg';", "", 0, new P());
                        Program.RunGekkoCommands("plot <" + plotStart.ToString() + " " + plotEnd.ToString() + " " + "xlineafter = " + plot_line.ToString() + " yminhard = -100 ymaxhard = 100 yminsoft = -1 ymaxsoft = 1  p> @" + varnameWithoutFreq + " '" + l2 + "' <type = lines dashtype = '3'>, " + varnameWithoutFreq + " '" + l1 + "' file='" + subFolder + "\\" + varnameWithoutFreq.ToLower() + "___p" + ".svg';", "", 0, new P());
                    }

                    sb.AppendLine("<img src = `" + varnameWithoutFreq.ToLower() + ".svg" + "`>");

                    sb.AppendLine("<p>");

                    if (isDanish) FoldingButtonStart(sb, "Vækst %");
                    else FoldingButtonStart(sb, "Growth %");
                    sb.AppendLine("<img src = `" + varnameWithoutFreq.ToLower() + "___p.svg" + "`>");
                    FoldingButtonEnd(sb);

                    if (jName != null)
                    {
                        if (isDanish) FoldingButtonStart(sb, "J-led");
                        else FoldingButtonStart(sb, "J-factor");
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
                    if (ts1 == null && ts2 == null)
                    {
                        //do nothing
                    }
                    else if (ts1 == null || ts2 == null)
                    {
                        sb3.AppendLine("Period" + extra + "        value        %  ");
                    }
                    else
                    {
                        sb3.AppendLine("Period" + extra + "        value        %  " + G.Blanks(gap) + "Period" + extra + "        value        %  ");
                    }
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
                    if (isDanish) WriteHtmlPreCode(sb, "+++ Note: variablens data kunne ikke indlæses");
                    else WriteHtmlPreCode(sb, "+++ Note: The variable data could not be read");
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

            x2.AppendLine("  <table cellpadding = `0` cellspacing = `0` width = `800px` border = `0`> ");
            x2.AppendLine("  <tr>");
            x2.AppendLine("  <td width = `80 %` ><b><big>" + list_title + "</big></b></td>");
            x2.AppendLine("  <td width = `10 %` ><a href = `" + settings_find_filename + "` > " + ss1 + " </a></td >");
            x2.AppendLine("  <td width = `10 %` ><a href = `" + settings_index_filename + "` > " + ss2 + " </a></td >");
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
            
            write = "document.write";            

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

                " + write + @"(`"+ Language(isDanish, "Søgning efter variablen:", "Searching for the variable") + @": '` + tekst + `'<br><br>`);

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
                        if (tekst1.toUpperCase() != tekst.toUpperCase()) /* unclear why they may not be identical...? */
                        {
                            fundet = true;
                            " + write + @"(`<a href=" + settings_vars_foldername + @"/` + varnavn[i].toLowerCase() + `.html style='text-decoration:none;'>` + varnavn[i] + `</a>`);
                            " + write + @"(`<br>` + beskriv[i] + `<br><br>`);
                        } //endif
                    } //endif
                } //endfor

                if (fundet == false)
                {
                    " + write + @"(`... "+ Language(isDanish, "gav intet resultat", "gave no result") + @".<br>`);
                } //endif
                " + write + @"(`<br><br><a href=" + settings_find_filename + @">"+ Language(isDanish, "Søg igen", "Search again") + @"</a> <br> <a href=" + settings_index_filename + @">"+ Language(isDanish, "Gå til hovedside", "Go to main page") + @"</a>`);
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

            " + write + @"(`"+ Language(isDanish, "Søgning efter teksten", "Searching for the text") + @": '` + tekst + `' "+ Language(isDanish, "i variabelliste", "in the variable list") + @"<br><br>`);
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
                " + write + @"(`... " + Language(isDanish, "gav intet resultat", "gave no result") + @".<br>`);
            } //endif            
            " + write + @"(`<br><br><a href=" + settings_find_filename + @">" + Language(isDanish, "Søg igen", "Search again") + @"</a> <br> <a href=" + settings_index_filename + @">" + Language(isDanish, "Gå til hovedside", "Go to main page") + @"</a>`);
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
            //if (isDanish) x3.AppendLine("<p><b>Indtast søgeord:</b></p>");
            x3.AppendLine("  <table cellpadding = `0` cellspacing = `0` width = `800px` border = `0`> ");
            x3.AppendLine("  <tr>");
            x3.AppendLine("  <td width = `80 %` ><b><big>" + Language(isDanish, "Søg", "Search") + "</big></b></td>");
            x3.AppendLine("  <td width = `10 %` ><a href = `" + settings_list_filename + "` > " + "List" + " </a></td >");
            x3.AppendLine("  <td width = `10 %` ><a href = `" + settings_index_filename + "` > " + ss2 + " </a></td >");
            x3.AppendLine("  </tr>");
            x3.AppendLine("  </table>");            
            x3.AppendLine("");
            if (isDanish) x3.AppendLine("Søgning efter variabelnavn:");
            else x3.AppendLine("Search variable name:");
            x3.AppendLine("<FORM NAME = `form1` >");
            x3.AppendLine("<INPUT NAME=`tekst` SIZE=`50` TYPE=`text` onKeyPress=`return check(event)`>");
            x3.AppendLine("<INPUT TYPE = `submit` VALUE=`Søg` onClick=`findvarnavn()`>");
            x3.AppendLine("</FORM>");
            x3.AppendLine("<p>&nbsp;</p>");
            if (isDanish) x3.AppendLine("Fritekstsøgning i variabelbeskrivelserne:");
            else x3.AppendLine("Free text search in variable descriptions:");
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

        public static string Language(bool isDanish, string q1, string q2)
        {
            string q;
            if (isDanish) q = q1;
            else q = q2;
            return q;
        }

        public static void BrowserNew()
        {                        
            bool isSimple = false;            
            bool isDanish = true;
            string browserFolder = "Browser";            
            bool deleteFolder = true;  //if true, everything is wiped out first.            

            bool onlyHtml = false; //default: false
            bool onlyPlot = false; //default: false
            bool skip = false;  //only for debug            

            bool small = false; //default: false, only few eqs.
            bool flush = false;  //Not necessary to set true anymore
            bool ignoreMissing = true;  //quite a lot of missings observations in MAKRO, but what does this really do?           
            EFreq freq = EFreq.A;  //there is some method for this, looking at model or bank??

            string settings_index_filename = "index.html";
            string settings_list_filename = "list.html";
            string settings_find_filename = "find.html";
            string settings_css_filename = "styles.css";            
            string settings_icon_filename = null;
            string settings_vars_foldername = "vars";            
            string settings_commands = null;
            string settings_plot_start = Globals.globalPeriodStart.super.ToString();
            string settings_plot_end = Globals.globalPeriodEnd.super.ToString();
            string settings_plot_line = null;
            string settings_print_start = Globals.globalPeriodStart.super.ToString();
            string settings_print_end = Globals.globalPeriodEnd.super.ToString();
            string settings_include_p_type = "yes";
            bool settings_show_source = true;
            object[] settings_ekstrafiler = null;

            G.Writeln2("Starting html browser generation");
            DateTime dt0 = DateTime.Now;

            string pathAndFile = Program.options.folder_working + "\\" + "browser.json";
            string jsonCode = null;
            if (!File.Exists(pathAndFile))
            {
                isSimple = true;
                isDanish = false;
                new Note("A '" + pathAndFile + "' file does not seem to exist: because of this, a basic/default browser is generated");                
            }
            else
            {
                jsonCode = G.RemoveComments(Program.GetTextFromFileWithWait(pathAndFile));
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

                try { settings_index_filename = (string)jsonTree["index_filename"]; } catch { G.Warning("w4.3", "JSON: index_filename not found, used \"" + settings_index_filename + "\""); }
                try { settings_list_filename = (string)jsonTree["list_filename"]; } catch { G.Warning("w4.3", "JSON: list_filename not found, used \"" + settings_list_filename + "\""); }
                try { settings_find_filename = (string)jsonTree["find_filename"]; } catch { G.Warning("w4.3", "JSON: Find_filename not found, used \"" + settings_find_filename + "\""); }
                //TODO: create css if not found as file
                try { settings_css_filename = (string)jsonTree["css_filename"]; } catch { G.Warning("4.3", "JSON: css_filename not found, used \"" + settings_css_filename + "\""); }          
                //TODO: what to do?
                try { settings_icon_filename = (string)jsonTree["icon_filename"]; } catch { G.Warning("4.3", "JSON: icon_filename not found"); }
                try { settings_vars_foldername = (string)jsonTree["vars_foldername"]; } catch { G.Warning("w4.3", "JSON: vars_foldername not found, used \"" + settings_vars_foldername + "\""); }
                try { settings_commands = (string)jsonTree["commands"]; } catch { G.Warning("4.3", "JSON: commands not found, no commands used."); }
                try { settings_plot_start = (string)jsonTree["plot_start"]; } catch { G.Warning("4.3", "JSON: plot_start not found, used \"" + settings_plot_start + "\""); }
                try { settings_plot_end = (string)jsonTree["plot_end"]; } catch { G.Warning("4.3", "JSON: plot_end not found, used \"" + settings_plot_end + "\""); }
                try { settings_plot_line = (string)jsonTree["plot_line"]; } catch { G.Warning("4.3", "JSON: plot_line not found, used \"" + settings_plot_line + "\""); }
                try { settings_print_start = (string)jsonTree["print_start"]; } catch { G.Warning("4.3", "JSON: print_start not found, used \"" + settings_print_start + "\""); }
                try { settings_print_end = (string)jsonTree["print_end"]; } catch { G.Warning("4.3", "JSON: print_end not found, used \"" + settings_print_end + "\""); }
                try { settings_include_p_type = (string)jsonTree["include_p_type"]; } catch {G.Warning("4.3", "JSON: include_p_type not found, used \"" + settings_include_p_type + "\""); }                
            }

            // -------------------------------------------------------------
            // -------------------------------------------------------------

            string ss1 = "Søg";
            string ss2 = "Hjem";
            if (!isDanish)
            {
                ss1 = "Search";
                ss2 = "Home";
            }

            string list_title = "Variabelliste (try Ctrl+F)";
            if (!isDanish) list_title = "Variable list (try Ctrl+F)";            

            List<string> files = new List<string>();
            files.Add(settings_index_filename);
            files.Add(settings_find_filename);
            files.Add(settings_list_filename);
            files.Add(settings_css_filename);
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

            // ?????
            // ?????
            // ?????
            // ?????
            // ?????
            string path = rootFolder;

            BrowserCleanupFolders(rootFolder, subFolder);

            if (!isSimple)
            {

                //index.html and styles.css is copied to root folder of browser system
                List<string> filesToCopy = new List<string>();
                filesToCopy.Add(settings_index_filename);
                filesToCopy.Add(settings_css_filename);
                filesToCopy.Add(settings_icon_filename);
                if (settings_ekstrafiler != null)
                {
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
                }

                foreach (string fileToCopy in filesToCopy)
                {
                    if (fileToCopy == null) continue;
                    string fileNameIndex = Program.options.folder_working + "\\" + fileToCopy;
                    string fileNameIndex2 = rootFolder + "\\" + fileToCopy;
                    if (!File.Exists(fileNameIndex))
                    {
                        new Error("'" + fileNameIndex + "' was not found");
                    }
                    File.Copy(fileNameIndex, fileNameIndex2, true);
                }

                Program.RunGekkoCommands(settings_commands, "", 0, new P());
            }

            int gap = 20;

            GekkoTime plotStart = new GekkoTime(EFreq.A, G.IntParse(settings_plot_start), 1);
            GekkoTime plotEnd = new GekkoTime(EFreq.A, G.IntParse(settings_plot_end), 1);
            GekkoTime plot_line = GekkoTime.tNull;
            if (isSimple) plot_line = plotStart.Add(-100); //-100 so it does not show up
            else plot_line = new GekkoTime(EFreq.A, G.IntParse(settings_plot_line), 1);
            GekkoTime print_start = new GekkoTime(EFreq.A, G.IntParse(settings_print_start), 1);
            GekkoTime print_end = new GekkoTime(EFreq.A, G.IntParse(settings_print_end), 1);

            string bank1 = Path.GetFileName(Program.databanks.GetFirst().FileNameWithPathPretty);
            string bank2 = Path.GetFileName(Program.databanks.GetRef().FileNameWithPathPretty);

            BrowserHelper bh = null;
            if (false)
            {
                //GREU
                bh = new BrowserHelper();
                bh.depthMax = 3;   //4. MaxValue can easily produce > 500 MB files.
                bh.counterMax = int.MaxValue;  //traces, not good --> gives a lot of non-opening folders that are non-deep
                bh.pixels = 20;
                bh.pixelsAfterArrow = 12;
                bh.freq = freq;
                bh.firstColWidth = 200;
                bh.removeTx0Dollar = true;  //Removes line: "over sets: [t], with $-condition: ((tx0[t]))"
                bh.type = EBrowserType.Greu;
                bh.text = new StringBuilder();
                bh.maxPages = 5;
            }
            else
            {
                //MAKRO
                bh = new BrowserHelper();
                bh.depthMax = 3;   //4. MaxValue can easily produce > 500 MB files.
                bh.counterMax = int.MaxValue;  //traces, not good --> gives a lot of non-opening folders that are non-deep
                bh.pixels = 20;
                bh.pixelsAfterArrow = 12;
                bh.freq = freq;
                bh.firstColWidth = 200;
                bh.removeTx0Dollar = true;  //Removes line: "over sets: [t], with $-condition: ((tx0[t]))"
                //bh.type = EBrowserType.MakroIdentitiesText;
                bh.type = EBrowserType.Makro;
                bh.text = new StringBuilder();
            }

            if (bh.type==EBrowserType.Adam && G.Equal(settings_include_p_type, "yes"))
            {
                List ml = O.GetIVariableFromString("#all", O.ECreatePossibilities.NoneReportError, true) as List;
                List<string> vars = Stringlist.GetListOfStringsFromIVariable(ml);
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
                vars.Sort(StringComparer.OrdinalIgnoreCase);
            }            

            // ====================================================================================================
            // NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW
            // NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW
            // NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW
            // NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW
            // NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW NEW
            // ====================================================================================================

            // --------------------------------------------------------------------------------------------------------

            GekkoDictionary<string, bool> restrict = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            if (small)
            {
                restrict.Add("qbnp", false);
                restrict.Add("pbnp", false);
                restrict.Add("vbnp", false);
                restrict.Add("pC[cTot]", false);
                restrict.Add("pG[gTot]", false);
                restrict.Add("pI[iTot]", false);
                restrict.Add("pM[tot]", false);
                restrict.Add("pX[xTot]", false);
                restrict.Add("qC[cTot]", false);
                restrict.Add("qI[iTot]", false);
                restrict.Add("qM[tot]", false);
                restrict.Add("qX[xTot]", false);
            }

            if (deleteFolder && Directory.Exists(rootFolder))
            {
                DialogResult result = MessageBox.Show("About to delete " + rootFolder + " and subfolders. It this ok?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes)
                {
                    G.DeleteFolder(rootFolder, true);
                    new Writeln("Folder " + rootFolder + " and subfolders deleted");
                }
                else
                {
                    new Error("User abort");
                }
            }

            if (!Directory.Exists(rootFolder))
            {
                Directory.CreateDirectory(rootFolder);
            }
            if (!Directory.Exists(subFolder))
            {
                Directory.CreateDirectory(subFolder);
            }

            if (true)
            {
                //File.Copy(@"c:\Thomas\Gekko\regres\Doc_browser\MAKRO\index.html", path + "\\" + "index.html");
                //File.Copy(@"c:\Thomas\Gekko\regres\Doc_browser\MAKRO\styles.css", path + "\\" + "styles.css");
                //File.Copy(@"c:\Thomas\Gekko\regres\Doc_browser\MAKRO\header_MAKRO.svg", path + "\\" + "header_MAKRO.svg");
                //File.Copy(@"c:\Thomas\Gekko\regres\Doc_browser\MAKRO\DREAM_logo_500x70px.svg", path + "\\" + "DREAM_logo_500x70px.svg");
                //File.Copy(@"c:\Thomas\Gekko\GekkoCS\Gekko\bin\x64\Release\images\checked.png", path + "\\vars\\" + "checked.png");
                //File.Copy(@"c:\Thomas\Gekko\GekkoCS\Gekko\bin\x64\Release\images\normal.png", path + "\\vars\\" + "normal.png");
                //File.Copy(@"c:\Thomas\Gekko\GekkoCS\Gekko\bin\x64\Release\images\checked_red.png", path + "\\vars\\" + "checked_red.png");
                //File.Copy(@"c:\Thomas\Gekko\GekkoCS\Gekko\bin\x64\Release\images\normal_red.png", path + "\\vars\\" + "normal_red.png");
            }

            Globals.browser = true;  //Do not change, internal TTH popup
            Program.options.databank_search = false;

            if (true)
            {
                if (bh.type == EBrowserType.Adam)
                {
                    Program.options.folder_working = @"c:\Thomas\Desktop\gekko\testing";
                    Program.RunGekkoCommands("reset; time 2006 2010; model jul05; read jul05;", "", 0, new P());
                }
                else if (bh.type == EBrowserType.Makro)
                {
                    string f = null; if (flush) f = "flush(); ";
                    Program.options.folder_working = @"c:\Thomas\Desktop\gekko\testing";
                    //Program.RunGekkoCommands(f + "reset; read <gdx> previous_deep_calibration.gdx; time 2029 2034; option model gams scalar data = yes; model<gms>deep_dynamic_calibration.zip; " + @"open 'c:\Thomas\Desktop\gekko\testing\MAKRO\GitHub\Data\Makrobk\makrobk.gbk' as traces;", "", 0, new P());
                    //Program.RunGekkoCommands(f + "reset; read <gdx> baseline_2026May.gdx; time 2029 2034; option model gams scalar data = yes; model<gms>deep_dynamic_calibration_2026May.zip; " + @"open makrobk_2026May.gbk as traces;", "", 0, new P());
                    Program.RunGekkoCommands(f + "reset; read<gdx> previous_deep_calibration_2025December.gdx; time 2029 2034; model<gms> deep_dynamic_calibration_2025December.zip; " + @"open 'c:\Thomas\Desktop\gekko\testing\makrobk-2025-12-15.gbk' as traces;", "", 0, new P()); //hash: b4d0d93, makrobk fra 15/12 2025.                                     
                }
                else if (bh.type == EBrowserType.Greu)
                {
                    //TODO TODO
                    //USE THIS: G.GekkoExeFolder() + "\\images\\images.zip"
                    //USE THIS: G.GekkoExeFolder() + "\\images\\images.zip"
                    //USE THIS: G.GekkoExeFolder() + "\\images\\images.zip"
                    //USE THIS: G.GekkoExeFolder() + "\\images\\images.zip"
                    //USE THIS: G.GekkoExeFolder() + "\\images\\images.zip"
                    //USE THIS: G.GekkoExeFolder() + "\\images\\images.zip"
                    List<string> filesToCopy = new List<string> { "styles.css", "index.html", "DREAM_logo_500x70px.svg", "header_MAKRO.svg" };
                    foreach (string s in filesToCopy) File.Copy(@"c:\Tools\Xxx\styles.css", @"c:\Thomas\Desktop\gekko\testing\DREAM\GREU\Version1\Browser\" + s);                    
                    string f = null; if (flush) f = "flush(); ";
                    Program.options.folder_working = @"c:\Thomas\Desktop\gekko\testing\DREAM\GREU\Version1";
                    Program.RunGekkoCommands(f + "reset; greu(); option decomp equation style = gams; global:%t1 = 2023; global:%t2 = 2027; model <%t1 %t2 gms> GREU.zip; read <first> main_CGE; time %t1+2 %t2-1;", "", 0, new P());
                }
                else if (bh.type == EBrowserType.MakroIdentitiesText)
                {
                    string f = null; if (flush) f = "flush(); ";
                    Program.options.folder_working = @"c:\Thomas\Desktop\gekko\testing";
                    Program.RunGekkoCommands(f + "reset; time 2024 2024; model <gms> identities.zip; read <gdx> makrobk.gdx; " + @"open 'c:\Thomas\Desktop\gekko\testing\MAKRO\GitHub\Data\Makrobk\makrobk.gbk' as traces;", "", 0, new P());
                    onlyHtml = true;
                }
                else new Error("Hov");
            }

            if (skip) return;

            GekkoTime t1 = GekkoTime.tNull;
            GekkoTime t2 = GekkoTime.tNull;
            t1 = Globals.globalPeriodStart;
            t2 = Globals.globalPeriodEnd;

            Model model = Program.model;
            ModelGamsScalar modelGamsScalar = model.modelGamsScalar;
            //The following loads the databank values into the scalar model, for reuse for all the DECOMP
            //calculations later on.
            //BEWARE: should t1 have 2-3 periods subtraced for instance? But t1.Add(-3) does not seem to change anything.
            model.modelGamsScalar.MaybeLoadDataIntoModel(0, t1, t2, ignoreMissing, false);

            GekkoDictionary<string, List<EquationNameAndNumber>> combos = BrowserNewGetVariableAndEquationCombos(t1, modelGamsScalar, bh);
                        
            if (onlyHtml && onlyPlot) new Error("Hov");
            if (onlyHtml)
            {
                BrowserNewHtml(t1, t2, path, restrict, combos, bh, model, modelGamsScalar);
            }
            else if (onlyPlot)
            {
                BrowserNewPlots(combos, path, restrict);
            }
            else
            {
                BrowserNewHtml(t1, t2, path, restrict, combos, bh, model, modelGamsScalar);
                BrowserNewPlots(combos, path, restrict);
            }

            if (bh.type == EBrowserType.MakroIdentitiesText)
            {
                string pathAndFilename = path + "\\" + "equations.txt";
                using (FileStream fs = Program.WaitForFileStream(pathAndFilename, null, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    sw.Write(bh.text.ToString());
                }
            }

            return;
        }

        private static void BrowserNewHtml(GekkoTime t1, GekkoTime t2, string path, GekkoDictionary<string, bool> restrict, GekkoDictionary<string, List<EquationNameAndNumber>> combos, BrowserHelper bh, Model model, ModelGamsScalar modelGamsScalar)
        {
            //FIXME
            //FIXME
            //FIXME
            //FIXME
            //FIXME            
            string settings_css_filename = "style.css";
            string settings_vars_foldername = "vars";
            string modelFrequencyString = "a";
            string res = "res_";
            bool isDanish = false;
            string settings_find_filename = "find.html";
            string sub = "vars";

            DateTime dt1 = DateTime.UtcNow;
            int count = 0;
            GekkoTime tUsedHere = modelGamsScalar.Maybe2000GekkoTime(t1);

            if (bh.type == EBrowserType.MakroIdentitiesText)
            {
                List<TwoStrings> list = new List<TwoStrings>();
                int n = Program.model.modelGamsScalar.CountEqs(1);
                for (int i = 0; i < n; i++)
                {
                    string eqName = modelGamsScalar.dict_FromEqNumberToEqName[i];
                    if (eqName == "") continue;
                    ExtractTimeDimensionHelper helper2 = GamsModel.ExtractTimeDimension(true, EExtractTimeDimension.NoIndexListOfStrings, eqName, false);
                    var equationName = helper2.resultingFullName;

                    if (helper2.time.Equals(t1))
                    {
                        string s5, s6;
                        EquationNameAndNumber equationHelper5 = new EquationNameAndNumber();
                        //equationHelper5.name = GamsModel.ExtractTimeDimension(true, EExtractTimeDimension.NoIndexListOfStrings, eqName, false).resultingFullName;
                        equationHelper5.name = helper2.resultingFullName;
                        equationHelper5.i = i;
                        GetEquationText(t1, bh, equationHelper5, modelGamsScalar, tUsedHere, out s5, out s6);

                        EquationTextHelper helper = new EquationTextHelper();
                        helper.showTime = false;
                        List<string> precedentsTemp = modelGamsScalar.GetPrecedentsNames(i, helper, t1);
                        GekkoDictionary<string, bool> precedentsDict = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
                        foreach (string variableName in precedentsTemp)  //excluding any variables with lags/leads here
                        {                            
                            string variableNameWithoutLagOrLead = G.Chop_RemoveLagOrLead(variableName);
                            if (G.StartsWith(variableNameWithoutLagOrLead, res)) variableNameWithoutLagOrLead = "zzzzzzzzzz_" + variableNameWithoutLagOrLead;
                            if (!precedentsDict.ContainsKey(variableNameWithoutLagOrLead)) precedentsDict.Add(variableNameWithoutLagOrLead, false);
                        }
                        List<string> precedents = precedentsDict.Keys.ToList();
                        precedents.Sort(G.CompareNaturalIgnoreCase);                        
                        precedents = precedents.Select(s => s.Replace("zzzzzzzzzz_", "")).ToList();

                        StringBuilder text2 = new StringBuilder();
                        text2.AppendLine();
                        text2.AppendLine("EQUATION: " + equationHelper5.name);
                        text2.AppendLine();
                        text2.AppendLine(System.Text.RegularExpressions.Regex.Replace(s5.Replace("\r\n", " "), @"\s+", " "));  // 2 or more blanks --> 1 blank
                        text2.AppendLine();
                        text2.AppendLine(System.Text.RegularExpressions.Regex.Replace(s6.Replace("\r\n", " "), @"\s+", " "));  // 2 or more blanks --> 1 blank
                        text2.AppendLine();
                        text2.AppendLine("VARIABLES: " + Stringlist.GetListWithCommas(precedents));
                        text2.AppendLine();
                        text2.AppendLine("-------------------------------------------------------");
                        list.Add(new TwoStrings(equationHelper5.name, text2.ToString()));
                    }
                }
                List<TwoStrings> sortedList = list.OrderBy(o => o.s1).ToList();
                foreach (TwoStrings two in sortedList)
                {
                    bh.text.Append(two.s2);
                }
                return;
            }

            if (true) //list.html and find.html
            {
                List<EquationBrowserHelper> vars2 = new List<EquationBrowserHelper>();
                StringBuilder x2 = new StringBuilder();
                foreach (string s in CreateCss(false)) x2.AppendLine(s);
                x2.AppendLine("<body>");
                x2.AppendLine(LinkHome(false));
                WriteHtmlBold(x2, "Alphabetical list of variables (use Ctrl+F to search).");
                x2.AppendLine("<table style = `width:100%`>");

                List<string> vars = combos.Keys.ToList();
                vars.Sort(G.CompareNaturalIgnoreCase);

                foreach (string var2 in vars)
                {
                    if (G.StartsWith(var2, res)) continue;  //skip res_... variables.
                    string expl = Program.SpecialXmlChars(Program.GetVariableExplanation1Line(var2, false));
                    x2.Append("<tr>");
                    x2.Append("<td width = `20%`>");
                    x2.Append(HtmlLink(var2, settings_vars_foldername + "/" + SimplerName(var2) + ".html"));
                    x2.Append("</td>");
                    x2.Append("<td width = `80%` style=`color:gray`>");
                    x2.Append(expl);
                    x2.Append("</td>");
                    x2.AppendLine("</tr>");

                    EquationBrowserHelper ebh = new EquationBrowserHelper();
                    ebh.s1 = var2;
                    if (ebh.s1 != null) ebh.s1 = ebh.s1.Replace("`", "'"); //We use ` to represent "
                    ebh.s2 = G.ReplaceWhitespaceWith1Blank(expl);
                    if (ebh.s2 != null) ebh.s2 = ebh.s2.Replace("`", "'"); //We use ` to represent "
                    vars2.Add(ebh);
                }
                x2.AppendLine("</table>");

                x2.AppendLine("  </p>");
                x2.AppendLine("  </body>");
                x2.AppendLine("</html>");

                using (FileStream fs = Program.WaitForFileStream(path + "\\" + "list.html", null, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    sw.Write(x2.Replace('`', '\"'));
                }

                G.WritelnGray("Finished list.html");

                // ------------------------------------------------------------
                // ----------------- find -------------------------------------
                // ------------------------------------------------------------

                var sorted = vars2.OrderBy(o => o.s1, new G.NaturalComparer(G.NaturalComparerOptions.Default));

                StringBuilder x3 = new StringBuilder();
                x3.AppendLine("<html>");
                x3.AppendLine("<head>");
                x3.AppendLine("<link rel = `stylesheet` href = `" + settings_css_filename + "` type = `text/css` >");
                //x3.AppendLine("<link rel = `shortcut icon` href = `" + settings_icon_filename + "` type = `image/vnd.microsoft.icon`>");
                x3.AppendLine("</head>");                

                foreach (string s in CreateCss(false)) x3.AppendLine(s);

                x3.AppendLine("<script LANGUAGE = `JavaScript` > <!-- ");

                StringBuilder s1 = new StringBuilder(); s1.AppendLine();
                StringBuilder s2 = new StringBuilder(); s2.AppendLine();
                foreach (EquationBrowserHelper s in sorted)
                {
                    s1.AppendLine("\"" + G.HandleQuoteInQuote2(s.s1) + "\"" + ", ");
                    s2.AppendLine("\"" + G.HandleQuoteInQuote2(s.s2) + "\"" + ", ");
                }

                string write = null;
                string join = null;
                write = "document.write";

                string js = @"

            function varnames() {
                var varname = [" + s1.ToString() + @"];
                return varname;
            }

            function describes() {
                var describe = [" + s2.ToString() + @"];
                return describe;
            }

            // A function to convert the wildcard pattern to a regular expression
            function createRegexFromWildcard(pattern) {
                let regexPattern = pattern.replace(/[.+^${}()|[\]\\]/g, `\\$&`).replace(/\*/g, `.*`).replace(/\?/g, `.`);                
                return new RegExp(`^` + regexPattern + `$`, 'i');
            }

            function SimplerName(s) {                                
                if (!s) return ``;
                return s.replace(/ /g, `-`).replace(/[^a-zA-Z0-9\-_\[\],]/g, ``).toLowerCase();
            }

            //Note: almost same as below
            function findvarname(){
              event.preventDefault(); // Prevents the page from reloading
              const resultsContainer = document.getElementById('results-container');              
              var content = [];
              var varname = varnames();
              var describe = describes();
              number = varname.length;
              text = new String;
              text1 = new String;
              text = document.form1.text.value;
              found = 0;

              const myRegex = createRegexFromWildcard(text.replace(/ /g, '')); /* replace blanks with nothing, so that x[i, j, *] becomes x[i,j,*]. */
              
              let html = '';
              for (var i = 0; i < number; i++)
                {
                    text1 = varname[i];
                    //alert(text + '...' + text1);
                    if (myRegex.test(text1))
                    {
                        found++;
                        html += '<tr><td width = `20%`><a href =' + 'vars/' + SimplerName(varname[i]) + '.html>' + varname[i] + '</a></td><td width = `80 %` style =`color: gray`> ' + describe[i] + '</td></tr>';
                    } //endif
               } //endfor
               html += '</table>';
                if (found == 0)
                {
                  resultsContainer.innerHTML = '<br><hr><br><p style =`color:gray` > ...No results found...</p>';
                }
                else {
                  s = '';
                  if(found != 1) s = 's';
                  resultsContainer.innerHTML = '<br><hr><br><p>Found ' + found + ' matching variable' +s+ '</p>' + '<table style = `width:100%` > ' + html;
                }
            } 

            //Note: almost same as above
            function finddescribe(){
              event.preventDefault(); // Prevents the page from reloading
              const resultsContainer = document.getElementById('results-container');              
              var content = [];
              var varname = varnames();
              var describe = describes();
              number = describe.length;
              text = new String;
              text1 = new String;
              text = document.form2.text.value;
              text = '*' + text + '*';  //Extra wildcards
              found = 0;

              const myRegex = createRegexFromWildcard(text);
              
              let html = '';
              for (var i = 0; i < number; i++)
                {
                    text1 = describe[i];
                    //alert(text + '...' + text1);
                    if (myRegex.test(text1))
                    {
                        found++;
                        html += '<tr><td width = `20%`><a href =' + 'vars/' + SimplerName(varname[i]) + '.html>' + varname[i] + '</a></td><td width = `80 %` style =`color: gray`> ' + describe[i] + '</td></tr>';
                    } //endif
               } //endfor
               html += '</table>';
                if (found == 0)
                {
                  resultsContainer.innerHTML = '<br><hr><br><p style =`color:gray` > ...No results found...</p>';
                }
                else {
                  s = '';
                  if(found != 1) s = 's';
                  resultsContainer.innerHTML = '<br><hr><br><p>Found ' + found + ' matching variable' +s+ '</p>' + '<table style = `width:100%` > ' + html;
                }
            } 

        function check(event) {
            var charCode = (navigator.appName == `Netscape`) ? event.which : event.keyCode;
            if (charCode == 13) findvarname();
        }          

        function check2(event) {
            var charCode = (navigator.appName == `Netscape`) ? event.which : event.keyCode;
        if (charCode == 13) finddescribe();
        }  // endfunction

        ";

                x3.AppendLine(js);
                x3.AppendLine("// -->");
                x3.AppendLine("</script>");
                x3.AppendLine("<body onload = `document.form1.text.focus()`>");                
                x3.AppendLine(LinkHome(false));
                x3.AppendLine("<p style = `font-weight: bold;`>Search</p>");
                x3.AppendLine("");                
                x3.AppendLine("Search variable names (wildcards: * or ?):");
                x3.AppendLine("<FORM NAME = `form1`>");
                x3.AppendLine("<INPUT NAME=`text` SIZE=`50` TYPE=`text` onKeyPress=`return check(event)`>");
                x3.AppendLine("<INPUT TYPE = `submit` VALUE=`Search` onClick=`findvarname()`>");
                x3.AppendLine("</FORM>");                
                x3.AppendLine("Free text search in variable descriptions:");
                x3.AppendLine("<FORM NAME = `form2`>");
                x3.AppendLine("<INPUT NAME=`text` SIZE=`50` TYPE=`text` onKeyPress=`return check2(event)`>");
                x3.AppendLine("<INPUT TYPE = `submit` VALUE=`Search` onClick=`finddescribe()`>");
                x3.AppendLine("</FORM></center>");
                x3.AppendLine("<div id = `results-container`></div>");  
                x3.AppendLine("</body>");
                x3.AppendLine("</html>");

                string pathAndFilename3 = path + "\\" + settings_find_filename;
                using (FileStream fs = Program.WaitForFileStream(pathAndFilename3, null, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    sw.Write(x3.Replace('`', '\"'));
                }
                G.WritelnGray("Finished find.html");
            }

            //new Error("Stop");

            foreach (KeyValuePair<string, List<EquationNameAndNumber>> kvp in combos.Take(bh.maxPages))
            {
                count++;
                string variableName = kvp.Key;
                List<EquationNameAndNumber> equations = kvp.Value;
                if (restrict.Count > 0 && !restrict.ContainsKey(variableName)) continue;

                string fileName1 = SimplerName(variableName) + ".html";                

                if (count % 1000 == 0) new Writeln(" ========== " + count + " of " + combos.Count + " (" + G.FormatNumber((double)count / (double)combos.Count * 100d, "f10.2", false, false) + "%) ==========");
                
                StringBuilder html1 = new StringBuilder();

                foreach (EquationNameAndNumber equationHelper in equations)
                {
                    html1.Append("<div id = `#" + SimplerName(equationHelper.name) + "-1` class=`content`>");
                    // ------------------------------------------------------
                    // TITLE
                    // ------------------------------------------------------
                    html1.Append("<p style=`font-size: 1.25rem;`>");  //rem is relative to the root of the whole html, em is relative to parent container.
                    EquationBrowser.SpanHtmlColor(html1, variableName);
                    html1.Append(" from equation ");
                    EquationBrowser.SpanHtmlColor(html1, equationHelper.name);
                    html1.Append("</p>");
                    // ------------------------------------------------------

                    // ------------------------------------------------------
                    // EQUATIONS code and related variables
                    // ------------------------------------------------------                                        
                    string s5, s6;
                    GetEquationText(t1, bh, equationHelper, modelGamsScalar, tUsedHere, out s5, out s6);
                    html1.AppendLine("<br style=`line-height: 0.2rem;`>");
                    ToggleLink(html1, "Equation", "To see such equations in Gekko 3.x, you may use the following statements (or similar):");
                    html1.AppendLine("read &lt;gdx> forecast.gdx;");
                    html1.AppendLine("model &lt;gms> makro.zip;");
                    html1.AppendLine("time " + t1.ToString() + " " + t2.ToString() + ";");
                    html1.AppendLine("decomp &lt;d> " + variableName + " from " + equationHelper.name + ";");
                    html1.AppendLine();
                    html1.AppendLine("//NOTE: Gekko can merge decomp tables (link equations), and much more.");
                    html1.AppendLine("</code></pre></div>");  //must end the ToggleLink()
                    html1.Append("<hr>");
                    EquationBrowser.WriteHtmlPreCode(html1, s5);
                    html1.Append("<hr>");
                    EquationBrowser.WriteHtmlPreCode(html1, s6);
                    html1.Append("<hr>");

                    html1.Append("<br>");
                    EquationBrowser.WriteHtmlBold(html1, "Variables");
                    string vars2 = null;
                    html1.AppendLine("<table class = `table1`>");

                    html1.AppendLine("<tr>");
                    html1.Append("<td style=`font-weight: bold;`>" + EquationBrowser.HtmlLink(variableName, SimplerName(variableName) + ".html") + "</td>");
                    html1.Append("<td style=`color:gray; font-weight: bold;`>" + Program.SpecialXmlChars(Program.GetVariableExplanation1Line(variableName, false)) + "</td>");
                    html1.AppendLine("</tr>");

                    EquationTextHelper helper2 = new EquationTextHelper();
                    helper2.showTime = false;
                    List<string> precedent2 = modelGamsScalar.GetPrecedentsNames(equationHelper.i, helper2, t1);
                    precedent2.Sort(StringComparer.OrdinalIgnoreCase);
                    GekkoDictionary<string, bool> dict = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
                    foreach (string variableName2 in precedent2)
                    {
                        string varnameWithoutLag = G.Chop_RemoveLagOrLead(variableName2);
                        if (G.Equal(varnameWithoutLag, variableName)) continue;  //Shown at top
                        if (dict.ContainsKey(varnameWithoutLag)) continue;  //no dubles, for instance if lags.
                        html1.AppendLine("<tr>");
                        html1.Append("<td>" + EquationBrowser.HtmlLink(varnameWithoutLag, SimplerName(varnameWithoutLag) + ".html") + "</td>");
                        html1.Append("<td style=`color:gray`>" + Program.SpecialXmlChars(Program.GetVariableExplanation1Line(varnameWithoutLag, false)) + "</td>");
                        html1.AppendLine("</tr>");
                        dict.Add(varnameWithoutLag, false);
                    }
                    html1.AppendLine("</table>");
                    // ------------------------------------------------------
                    html1.Append("</div>");
                }

                if (true)
                {
                    html1.AppendLine("<div id = `hash-2` class=`content`>");
                    html1.Append("<br>");
                    EquationBrowser.WriteHtmlBold(html1, "Related variables");
                    string s8 = null;
                    List<EqInfoSimple> eqsContainingVariable = GamsModel.GetSortedEquations(variableName, tUsedHere, model, false, false, true);
                    List<string> dependentVarsList = Program.FindDependentVars(variableName, model, model.modelGams, modelGamsScalar, eqsContainingVariable);
                    bool first2 = true;
                    foreach (string s in dependentVarsList)
                    {
                        string tooltip = Program.SpecialXmlChars(Program.GetVariableExplanation1Line(s, false));
                        string link = EquationBrowser.HtmlLink(s, SimplerName(s) + ".html", tooltip);
                        if (!first2) s8 += ", ";
                        s8 += link;
                        first2 = false;
                    }
                    EquationBrowser.WriteHtml(html1, s8);
                    html1.AppendLine("</div>");
                }

                if (true)
                {
                    html1.AppendLine("<div id = `hash-1` class=`content`>");
                    html1.Append("<br>");
                    ToggleLink(html1, "Plot", "To see this plot in Gekko 3.x, you may use the following statements (or similar):");
                    html1.AppendLine("read &lt;gdx> forecast.gdx;");
                    html1.AppendLine("time " + t1.ToString() + " " + t2.ToString() + ";");
                    html1.AppendLine("plot " + variableName + "; //plot&lt;p> for growth");
                    html1.AppendLine("</code></pre></div>");  //must end the ToggleLink()
                    try
                    {
                        //only plot the series from Work                        
                        //Program.RunGekkoCommands("plot <" + t1.ToString() + " " + t2.ToString() + " > " + variableName + " file='" + path + variableName.ToLower() + ".svg';", "", 0, new P());
                        html1.AppendLine("<img style = `max-width: 425px;` src = `" + SimplerName(variableName) + ".svg" + "`>");
                        if (bh.plotTypes == 2) html1.AppendLine("<img style=`" + "margin-left: 50px; max-width: 425px;" + "` src = `" + SimplerName(variableName) + "__p.svg" + "`>");
                        html1.AppendLine("<p>");
                    }
                    catch
                    {
                    }
                    html1.AppendLine("</div>");
                }

                foreach (EquationNameAndNumber equationHelper in equations)
                {
                    html1.Append("<div id = `#" + SimplerName(equationHelper.name) + "-2` class=`content`>");
                    // ------------------------------------------------------
                    // EQUATIONS code and related variables
                    // ------------------------------------------------------
                    //Program.RunGekkoCommands("decomp <d> qbnp from e_qbnp endo qbnp;", "", 0, new P());
                    string table = BrowserDecompTable(t1, t2, variableName, equationHelper, model, modelGamsScalar);
                    if (table != null)
                    {
                        html1.AppendLine("<br>");
                        ToggleLink(html1, "Time-decomposition", "To see this decomposition in Gekko 3.x, you may use the following statements (or similar):");
                        html1.AppendLine("read &lt;gdx> forecast.gdx;");
                        html1.AppendLine("model &lt;gms> makro.zip;");
                        html1.AppendLine("time " + t1.ToString() + " " + t2.ToString() + ";");
                        html1.AppendLine("decomp &lt;d> " + variableName + " from " + equationHelper.name + "; //&lt;p> for growth, &lt;errors> for errors");
                        html1.AppendLine();
                        html1.AppendLine("//NOTE: Gekko can merge decomp tables (link equations), and much more.");
                        html1.AppendLine("</code></pre></div>");  //must end the ToggleLink()                            
                        html1.AppendLine(table);
                    }
                    // ------------------------------------------------------
                    html1.Append("</div>");
                }

                if (true)
                {
                    //Traces
                    Series ts = null;
                    try
                    {
                        ts = O.GetIVariableFromString(G.Chop_AddFreq(G.Chop_AddBank(variableName, "traces"), bh.freq), O.ECreatePossibilities.NoneReturnNullAlways) as Series;
                    }
                    catch { }

                    if (ts != null && ts?.meta?.trace2.GetPrecedents_BewareOnlyInternalUse().GetStorage() != null && ts.meta.trace2.GetPrecedents_BewareOnlyInternalUse().GetStorage().Count() > 0)
                    {
                        html1.AppendLine("<div id = `hash-3` class=`content`>");
                        html1.AppendLine(@"<br>");
                        html1.AppendLine(@"<br>");
                        html1.AppendLine(@"<hr>");

                        ToggleLink(html1, "Data-traces", "To see these data-traces in Gekko 3.x, you may use the following statements (or similar):");
                        html1.AppendLine("read makrobk.gbk; //.gdx has no data-traces");
                        html1.AppendLine("time " + t1.ToString() + " " + t2.ToString() + ";");
                        html1.AppendLine("trace " + variableName + ";");
                        html1.AppendLine("disp " + variableName + "; //click the trace link");
                        html1.AppendLine("</code></pre></div>");  //must end the ToggleLink()

                        GekkoTimeSpansSimple gtss = null;
                        Trace2 trace = ts.meta.trace2;
                        //th.html.AppendLine(@" <li class=`folder`>");                        
                        WalkTracesForHtml(trace, gtss, bh, 0, html1);
                        //th.html.AppendLine(@"</div>");
                        html1.AppendLine("<textarea id = `output` readonly>Click '>' to unfold sub-traces, and click a row to see more info. (Red-colored '>' at larger depths indicate that traces have been pruned off for space reasons).</textarea>");
                        html1.AppendLine("</div>");
                    }
                }

                if (true)
                {
                    //ADAMBK-precedents
                    GekkoDictionary<string, bool> found = null;
                    try
                    {
                        found = Program.TraceGetPrecedents(new ScalarString("traces:" + variableName), "adambk", 0, null);
                    }
                    catch { };
                    if (found != null && found.Count > 0)
                    {
                        List<string> names = found.Keys.ToList();
                        names.Sort(StringComparer.OrdinalIgnoreCase);
                        for (int i2 = 0; i2 < names.Count; i2++) names[i2] = G.Chop_RemoveFreq(names[i2]);
                        string s = Stringlist.GetListWithCommas(names);
                        html1.AppendLine("<div id = `hash-4` class=`content`>");
                        html1.AppendLine("<br>");
                        html1.AppendLine("<br>");
                        EquationBrowser.WriteHtmlBold(html1, "Direct and indirect ADAM-variable use");
                        EquationBrowser.WriteHtml(html1, s);
                        html1.AppendLine("</div>");
                    }
                }

                StringBuilder x; string js;
                BrowserNewCssAndJs(variableName, bh.firstColWidth, bh.pixels, bh.pixelsAfterArrow, equations, true, out x, out js);

                x.AppendLine("  <body>");
                x.Append(LinkHome(true));
                string html2 = BrowserNewSelector(t1, model, modelGamsScalar, variableName, tUsedHere, bh);
                x.Append(html2);
                x.Append(html1);
                x.AppendLine(js);
                x.AppendLine("  </body>");
                x.AppendLine("</html>");
                using (FileStream fs = Program.WaitForFileStream(path + "\\vars\\" + fileName1, null, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    //BEWARE: In JavaScript, it is legal to do y = `i am a string`;, where backticks indicate that {}-interpolation 
                    //        can be used. So if JavaScript with backticks is used, do a workaround.
                    sw.Write(x.Replace('`', '\"'));
                }
            }                        

            if (Globals.runningOnTTComputer) new Writeln("TTH: Html took: " + G.SecondsUtc(dt1));
            return;
        }

        private static string SimplerName(string s)
        {
            string result = s.Replace(" ", "-");            
            string pattern = @"[^a-zA-Z0-9\-_\[\],]";
            result = System.Text.RegularExpressions.Regex.Replace(result, pattern, "").ToLower();
            return result;
        }

        private static void GetEquationText(GekkoTime t1, BrowserHelper bh, EquationNameAndNumber equationHelper, ModelGamsScalar modelGamsScalar, GekkoTime tUsedHere, out string s5, out string s6)
        {            
            string s2 = G.Chop_DimensionAddLast(equationHelper.name, tUsedHere.ToString(), null);
            EquationTextHelper helper = new EquationTextHelper();
            GetEquationTextHelper helper22 = Program.model.GetEquationText(new List<string>() { s2 }, helper, tUsedHere);
            s5 = helper22.s_gamsOrFrnSyntax;
            if (bh.removeTx0Dollar) s5 = Tx0(s5);
            s6 = helper22.s_scalarModel;
            int index = s6.IndexOf("..");
            if (index >= 0) s6 = s6.Substring(index + "..".Length).Trim();
        }

        private static string LinkHome(bool levelUp)
        {
            string up = null;
            if (levelUp) up = "../";            
            return "<div style=`display: block; color: #0645AD; font-size: 0.8em;`>&larr;<a href = `" + up + "index.html`>Home</a></div>";
        }

        private static List<string> CreateCss(bool levelUp)
        {
            List<string> head = new List<string>();
            string up = null;
            if (levelUp) up = "../";
            head.Add("<!DOCTYPE HTML PUBLIC `-//W3C//DTD HTML 4.01 Transitional//EN`>");
            head.Add("<HTML>");
            head.Add("<HEAD>");
            head.Add("<TITLE>MAKRO equation browser</TITLE>");
            head.Add("<link rel = `stylesheet` href = `" + up + "styles.css` type = `text/css` >");
            head.Add("<link rel=`shortcut icon` href =`punkt-bomaerke.ico` type =`image/vnd.microsoft.icon` >");
            head.Add("<link rel=`icon` sizes =`32x32` type =`image/png` href =`https://dreamgroup.dk/Media/638097183650056393/indeks.png?width=32&amp;height=32` ><link rel=`icon` sizes =`16x16` type=`image/png` href=`https://dreamgroup.dk/Media/638097183650056393/indeks.png?width=16&amp;height=16`><link rel=`icon` sizes=`128x128` type=`image/png` href=`https://dreamgroup.dk/Media/638097183650056393/indeks.png?width=128&amp;height=128`><link rel=`icon` sizes=`196x196` type=`image/png` href=`https://dreamgroup.dk/Media/638097183650056393/indeks.png?width=196&amp;height=196`><link rel=`apple-touch-icon` sizes=`180x180` href=`https://dreamgroup.dk/Media/638097183650056393/indeks.png?width=180&amp;height=180`><link rel=`apple-touch-icon` sizes=`152x152` href=`https://dreamgroup.dk/Media/638097183650056393/indeks.png?width=152&amp;height=152`><link rel=`apple-touch-icon` sizes=`167x167` href=`https://dreamgroup.dk/Media/638097183650056393/indeks.png?width=167&amp;height=167`>");
            head.Add("<meta http-equiv=`Content-Type` content=`text/html; charset=utf-8`>");
            head.Add("</HEAD>");            
            return head;
        }

        /// <summary>
        /// Returns a dict where the keys are variable names, and the values are those equations that the variable is present in.
        /// Note: conditions on time t (starting period)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="modelGamsScalar"></param>
        /// <returns></returns>
        public static GekkoDictionary<string, List<EquationNameAndNumber>> BrowserNewGetVariableAndEquationCombos(GekkoTime t, ModelGamsScalar modelGamsScalar, BrowserHelper bh)
        {
            GekkoDictionary<string, List<EquationNameAndNumber>> combos = new GekkoDictionary<string, List<EquationNameAndNumber>>(StringComparer.OrdinalIgnoreCase);  //key:varname, value:equation names

            int n = modelGamsScalar.CountEqs(1);
            for (int i = 0; i < n; i++)
            {
                //if (i == 36)
                //{
                //}
                //if (combos.Count > bh.maxPages) break;
                string eqName = modelGamsScalar.dict_FromEqNumberToEqName[i];
                if (eqName == "") continue;
                ExtractTimeDimensionHelper helper2 = GamsModel.ExtractTimeDimension(true, EExtractTimeDimension.NoIndexListOfStrings, eqName, false);
                var equationName = helper2.resultingFullName;

                if (helper2.time.Equals(t))
                {
                    EquationTextHelper helper = new EquationTextHelper();
                    helper.showTime = false;
                    List<string> precedentsTemp = modelGamsScalar.GetPrecedentsNames(i, helper, t);

                    foreach (string variableName in precedentsTemp)  //excluding any variables with lags/leads here
                    {
                        string variableNameWithoutLagOrLead = G.Chop_RemoveLagOrLead(variableName);
                        if (!combos.ContainsKey(variableNameWithoutLagOrLead)) combos.Add(variableNameWithoutLagOrLead, new List<EquationNameAndNumber>());
                        combos[variableNameWithoutLagOrLead].Add(new EquationNameAndNumber() { i = i, name = equationName });
                    }
                }
            }

            return combos;
        }

        /// <summary>
        /// This is for mass-producing gnuplot files, for the html browser.
        /// Making around 15.000 svg files (from 15.000 .gp and .data files) takes &lt; 1 min, even in debug mode, so this is fast!
        /// </summary>
        /// <param name="combos"></param>
        private static void BrowserNewPlots(GekkoDictionary<string, List<EquationNameAndNumber>> combos, string browserPath, GekkoDictionary<string, bool>restrict)
        {
            double yminhard = -100d;
            double ymaxhard = 100d;
            DateTime dt0 = DateTime.UtcNow;
            List<string> m = new List<string>() { "n", "p" };
            foreach (string op in m)
            {

                Globals.browserPlotFiles = new List<string>(); //Directory.Delete(Globals.localTempFilesLocationGnuplot, true);
                try
                {
                    string gnuplotPath = Globals.localTempFilesLocationGnuplot + "\\tempfiles";

                    //Delete the master file
                    string fileNameWithPath = gnuplotPath + "\\" + "browser.gp";
                    try
                    {
                        File.Delete(fileNameWithPath);
                    }
                    catch { }

                    //Generate 1 file for gnuplot to chew on
                    O.Prt o0 = null;
                    foreach (KeyValuePair<string, List<EquationNameAndNumber>> kvp in combos)
                    {
                        if (restrict.Count > 0 && !restrict.ContainsKey(kvp.Key)) continue;

                        foreach (string s in new List<string>() { "gp", "dat" })
                        {
                            if (File.Exists(gnuplotPath + "\\" + "temp" + (Globals.browserPlotFiles.Count + 1) + "." + s))
                            {
                                try
                                {
                                    File.Delete(gnuplotPath + "\\" + "temp" + (Globals.browserPlotFiles.Count + 1) + "." + s);
                                }
                                catch { }
                            }
                        }

                        o0 = new O.Prt();
                        o0.operators = new List<OptString>();
                        o0.operators.Add(new OptString(op, "yes"));

                        if (op == "p")
                        {
                            //So we do not show too small or too large percentages
                            o0.opt_yminhard = yminhard;
                            o0.opt_ymaxhard = ymaxhard;
                            o0.opt_yminsoft = -1d;
                            o0.opt_ymaxsoft = 1d;
                            o0.opt_ytitle = "%"; //Produces very large .svg files -- strange...!
                        }

                        o0.isBrowser = true;
                        string extra = null;
                        string extra2 = null;
                        if (op != "n")
                        {
                            extra = "__" + op;
                            extra2 = " (%)";
                        }
                        o0.browserPath = browserPath + "\\vars\\" + SimplerName(kvp.Key) + extra + ".svg";
                        o0.prtType = "plot";
                        o0.opt_filename = "browser.svg";  //not used, but .svg indicates that .svg files are to be made                
                        O.Prt.Element ope0 = new O.Prt.Element();
                        ope0.labelGiven = new List<string>() { kvp.Key + extra2 };
                        ope0.labelRecordedPieces = new List<O.RecordedPieces>();
                        Program.GetElementOperators(o0, ope0, out ope0.operatorsFinal, out ope0.operatorsFinalAll);
                        ope0.variable[0] = O.GetIVariableFromString(kvp.Key, O.ECreatePossibilities.NoneReturnNullAlways) as Series;
                        if (ope0.variable[0] == null) continue;
                        o0.prtElements.Add(ope0);
                        try
                        {
                            o0.Exe();
                        }
                        catch { };  //May be a plot containing all missing value, and this should not stop everything.
                    }

                    using (FileStream fs = Program.WaitForFileStream(fileNameWithPath, null, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    {
                        foreach (string s in Globals.browserPlotFiles)
                        {
                            sw.WriteLine("reset session");
                            sw.WriteLine("load " + Globals.QT + (gnuplotPath + "\\" + s).Replace("\\", "\\\\") + Globals.QT);
                        }
                    }
                    new Writeln("Running " + fileNameWithPath);
                    Plot.CallGnuplot2(o0, 0, null, "browser.gp", null, gnuplotPath, null, null, 10080);  //minutes corresponding to 1 week
                }
                finally
                {
                    //Important: switches this off for normal PLOT use
                    Globals.browserPlotFiles = null; // Directory.Delete(Globals.localTempFilesLocationGnuplot, true); --> often fails because gnuplot sits on the folder            
                }
            }
            if (Globals.runningOnTTComputer) new Writeln("TTH: Plots took: " + G.SecondsUtc(dt0));
        }

        private static string BrowserNewSelector(GekkoTime t1, Model model, ModelGamsScalar modelGamsScalar, string variableName, GekkoTime tUsedHere, BrowserHelper th)
        {
            List<EqInfoSimple> eqsNew = GamsModel.GetSortedEquations(variableName, t1, model, false, false, true);
            StringBuilder html2 = new StringBuilder();
            html2.AppendLine("<div id = `no-hash` class=`content`>");            
            html2.Append("<p style=`font-size: 1.25rem;`>");  //rem is relative to the root of the whole html, em is relative to parent container.
            EquationBrowser.SpanHtmlColor(html2, variableName);
            html2.Append("</p>");
            //EquationBrowser.WriteHtmlColor(html2, variableName);
            EquationBrowser.WriteHtmlColorGray(html2, Program.SpecialXmlChars(Program.GetVariableExplanation1Line(variableName, false)));            
            html2.AppendLine("<br style=`line-height: 0.35rem;`>");
            EquationBrowser.WriteHtml(html2, "Select one of the following " + eqsNew.Count + " equations containing " + variableName + ":");            
            string table = "<table cellpadding=`5`>";
            int count = -1;
            foreach (EqInfoSimple eqHelper in eqsNew)
            {
                count++;
                table += "<tr>";
                EquationTextHelper helper = new EquationTextHelper();
                GetEquationTextHelper helper22 = Program.model.GetEquationText(new List<string>() { eqHelper.eqName }, helper, tUsedHere);                
                string link = EquationBrowser.HtmlLink(eqHelper.eqNameWithLag.Replace(" ", ""), SimplerName(variableName) + ".html" + "#" + SimplerName(G.Chop_RemoveLagOrLead(eqHelper.eqNameWithLag)));
                if (count == 0) link ="<b>" + link + "</b>";
                table += "<td style=`vertical-align:top`>";
                table += link;
                table += "</td>";
                table += "<td style=`vertical-align:top`>";
                string s = helper22.s_gamsOrFrnSyntax;
                if (th.removeTx0Dollar) s = Tx0(s);
                table += "<pre><code>" +s + "</code></pre>";
                table += "</td>";
                table += "</tr>";
            }
            table += "</table>";
            html2.AppendLine(table);            
            html2.AppendLine("</div>");
            return html2.ToString();
        }

        private static string Tx0(string s)
        {
            s = s.Replace("over sets: [t], with $-condition: ((tx0[t]))", "");
            return s;
        }

        private static string BrowserDecompTable(GekkoTime t1, GekkoTime t2, string variableName, EquationNameAndNumber equationHelper, Model model, ModelGamsScalar modelGamsScalar)
        {
            string equationName = equationHelper.name;
            string equationNameHash = "#" + SimplerName(equationName);
            DecompOptions2 decompOptions2 = new DecompOptions2();
            decompOptions2.t1 = t1;
            decompOptions2.t2 = t2;
            decompOptions2.decompOperator = new DecompOperator("d");
            decompOptions2.new_select = new List<string>() { variableName };
            decompOptions2.new_from = new List<string>() { equationName };
            decompOptions2.new_endo = new List<string>() { variableName };            

            decompOptions2.rows = new List<string>() { "vars", "lags" };
            decompOptions2.cols = new List<string>() { "time" };
            
            GekkoSmpl smpl = new GekkoSmpl(t1, t2);
            DecompDatas decompDatas = new DecompDatas();
            GekkoTime gt1, gt2;
            Gekko.Decomp.DecompMainInit(out gt1, out gt2, t1, t2, decompOptions2.decompOperator);            
            Gekko.Decomp.EContribType operatorOneOf3Types = decompOptions2.decompOperator.type;            
            string lhsString = "Expression value";            
            Gekko.Decomp.PrepareEquations(t1, t2, decompOptions2.decompOperator, decompOptions2, false, modelGamsScalar);
            if (decompDatas.storage == null) decompDatas.storage = new List<List<DecompData>>();
            decompDatas.MAIN_data = null;
            if (decompDatas.storage == null || decompDatas.storage.Count == 0) Gekko.Decomp.InitDecompDatas(decompOptions2, decompDatas, model);            
            string table = null;
            try
            {
                table += "<table style=`font-size:0.95rem`>" + G.NL;
                table += "<tr>" + G.NL;
                table += "<td style=`vertical-align:top;`>" + G.NL;
                table += "<form id=`" + equationNameHash + "-checkbox_op`>" + G.NL;
                table += "<input type=`radio` name=`myradio` value=`d` checked> Abs. time-change (d)" + G.NL;
                table += "<br>" + G.NL;
                table += "<input type=`radio` name=`myradio` value=`p`> Growth rate (p)" + G.NL;
                table += "</form>" + G.NL;
                table += "</td>" + G.NL;
                table += "<td style=`padding-left: 40px; vertical-align:top;`>" + G.NL;
                table += "<label>" + "<input type =`checkbox` id =`" + equationNameHash + "-checkbox_error`>" + " Show errors" + "</label>" + G.NL;
                table += "</td>" + G.NL;
                table += "</tr>" + G.NL;
                table += "</table>" + G.NL;

                List<string> combos_op = new List<string> { "d", "p" };
                List<string> combos_errors = new List<string> { "no", "yes" };

                foreach (string combo_op in combos_op)
                {
                    foreach (string combo_errors in combos_errors)
                    {                       

                        table += "<div id=`" + equationNameHash + "-decomp_" + combo_op + "_" + combo_errors + "` class=`table-container`>" + G.NL;
                        table += "<table>" + G.NL;
                        decompOptions2.decompOperator = new DecompOperator(combo_op);
                        if (combo_errors == "yes") decompOptions2.showErrors = true;
                        else decompOptions2.showErrors = false;
                        string residualName = Program.GetDecompResidualName(0, 1);
                        int funcCounter = 0;
                        DecompData dd = Gekko.Decomp.DecompLowLevelScalar(gt1, gt2, decompOptions2.link[0].GAMS_dsh[0], decompOptions2.decompOperator, residualName, ref funcCounter, decompOptions2.missingAsZero, model);
                        Decomp.DecompMainMergeOrAdd(decompDatas, dd, 0, 0);  //probably superfluous when looking a abs differences?
                        decompDatas.MAIN_data = dd; decompDatas.storage[0][0] = dd;
                        DecompOutput decompOutput = Decomp.DecompPivotToTable(smpl, t1, t2, dd, decompDatas, lhsString, decompOptions2.decompOperator, operatorOneOf3Types, decompOptions2, model);

                        Table decompTable = decompOutput.table;

                        table += "<thead>" + G.NL;
                        string percent = null;
                        if (combo_op == "p") percent = "<span style=`margin-right: 0.75em`>%</span>";
                        table += "<tr><th style=`text-align: right`>" + percent + "</th>" + G.NL;
                        for (int j2 = 2; j2 <= decompTable.GetColMaxNumber(); j2++)
                        {
                            double d = WindowDecomp.RedLampValue(decompOutput.red, j2 - 2, null);
                            string tooltip = WindowDecomp.RedLampText("row", "Try to click the 'Show errors' checkbox.", decompOutput.red[j2 - 2]);
                            string imageHtml = null;
                            if (d <= Globals.redThresholds[0]) imageHtml = "<div class=`transparentcircle` style=`float: right;` title=`" + tooltip + "`></div>";
                            else if (d > Globals.redThresholds[0] && d <= Globals.redThresholds[1]) imageHtml = "<div class=`yellowcircle` style=`float: right;` title=`" + tooltip + "`></div>";
                            else if (d > Globals.redThresholds[1] && d <= Globals.redThresholds[2]) imageHtml = "<div class=`orangecircle` style=`float: right` title=`" + tooltip + "`></div>";
                            else if (d > Globals.redThresholds[2]) imageHtml = "<div class=`redcircle` style=`float: right` title=`" + tooltip + "`></div>";
                            
                            Cell c = decompTable.Get(1, j2);
                            table += "<th>" + "" + "<span style=`text-align: left`>" + c.CellText.TextData[0] + "</span>" + "" + imageHtml + "</th>";
                        }

                        table += "</tr>" + G.NL;
                        table += "</thead>" + G.NL;
                        table += "<tbody>" + G.NL;

                        for (int i2 = 2; i2 <= decompTable.GetRowMaxNumber(); i2++)
                        {
                            Cell cellVariableName = decompTable.Get(i2, 1);
                            List<string> vars = new List<string>();
                            Cell cellFirstData = decompTable.Get(i2, 2);
                            string uniqueName = null;
                            if (cellFirstData != null)
                            {
                                vars = cellFirstData.vars_hack;
                                uniqueName = Decomp.HiddenVariableHelper(cellFirstData, true);
                            }
                            string sVarsInside = Stringlist.GetListWithCommas(vars).Replace("¤", "");
                            string title = null;
                            if (uniqueName != null) title += Program.SpecialXmlChars(Program.GetVariableExplanation1Line(uniqueName, false)) + "&#10;";  //the code gives a newline --> weird but works in Chrome and IE
                            if (!G.NullOrBlanks(sVarsInside)) title += sVarsInside;
                            string titleHtml = null;
                            if (!G.NullOrBlanks(title)) titleHtml = " title=`" + title.Trim() + "`";
                            string name = cellVariableName.CellText.TextData[0];
                            name = name.Replace(" | [0]", "");
                            name = name.Replace(" | ", "");
                            name = name.Trim();
                            string style = "style=`text-align: right`";
                            if (name == "Error")
                            {
                                style = "style=`text-align: right; background-color: #fff7ed`";  //like Gekko
                                titleHtml = " title=`" + Globals.decompErrorText + "`";
                            }
                            else if (name == "Residual")
                            {
                                style = "style=`text-align: right; background-color: #fefdef`";  //like Gekko
                                titleHtml = " title=`" + Globals.decompResidualText1 + Globals.decompResidualText2 + "`";
                            }
                            table += "<tr>";                           
                            table += "<th" + titleHtml + ">";











                            List<List<string>> black = decompOutput.black;
                            bool view = false;
                            foreach (List<string> b5 in black)
                            {
                                if (b5.Count > 1)
                                {
                                    view = true; break;
                                }
                            }
                            List<string> black2 = black[i2 - 2];
                            int n = black2.Count;
                            string imgBlack = "";
                            if (view)
                            {
                                if (n > 1)
                                {
                                    imgBlack = "<img class=`img-size` src=`" + "../normal.png" + "` style=`opacity:0.3; padding-right: 8px; position: relative; top: 3px` title = `Contains " + n + " aggregated variables`></img>";
                                }
                                else
                                {
                                    imgBlack = "<img class=`img-size` src=`" + "../normal.png" + "` style=`opacity:0.0; padding-right: 8px; position: relative; top: 3px` title = `Contains " + n + " aggregated variables`></img>";
                                }
                            }












                            if (i2 == 2) table += imgBlack + "<span style=`font-weight:bold`>" + name + "</span>";  //first data row
                            else table += imgBlack + name;
                            table += "</th>";
                            for (int j2 = 2; j2 <= decompTable.GetColMaxNumber(); j2++)
                            {
                                Cell cellData = decompTable.Get(i2, j2);
                                double value = cellData.number;
                                string dataHtml = " title=`" + value + "`";                            
                                table += "<td " + style + dataHtml + ">";
                                string valueFormatted = null;
                                if (percent == null) valueFormatted = G.FormatNumber(value, "f15.4", false, false).Trim();
                                else valueFormatted = G.FormatNumber(value, "f15.2", false, false).Trim();
                                table += valueFormatted;
                                table += "</td>";
                            }
                            table += "</tr>" + G.NL;
                        }
                        table += "</tbody>" + G.NL;
                        table += "</table>" + G.NL;
                        table += "</div>" + G.NL;
                    }
                }                                              
            }
            catch
            {
                table = null;
            }
            return table;
        }

        private static void ToggleLink(StringBuilder html1, string heading, string firstLine)
        {
            html1.AppendLine("<p><span style=`font-weight:bold`>" + heading + "</span>&nbsp;&nbsp;<span style=`font-size: 0.8em;`>");
            html1.Append("<a href=`#` class=`toggle-link`>Gekko code</a></span></p><div class=`toggle-content` style=`display: none;`><p>" + firstLine + "</p><pre style=`background-color: #fff3cd;`><code>");
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
        public static void WalkTracesForHtml(Trace2 trace, GekkoTimeSpansSimple gtss, BrowserHelper th, int depth, StringBuilder html)
        {
            th.counter++;
            int childrenCount = trace.GetPrecedents_BewareOnlyInternalUse().Count();
            if (depth > 0)            
            {
                TraceItem traceItem = trace.FromTraceToTreeViewItem(gtss);                
                html.AppendLine(@"<div class=`list-item-content`>");
                string visibility = null;
                string image = "../normal.png";
                string imageExtra = null;
                if (childrenCount == 0)
                {
                    visibility = "visibility: hidden; ";
                }
                else
                {
                    if (WalkTracesForHtmlIsPruned(th, depth))
                    {
                        //Has children but is pruned
                        image = "../normal_red.png";
                        imageExtra = " onclick = `alert('Sub-traces at this depth exist, but have been pruned off for space reasons in this html trace viewer.')` ";
                    }
                }

                html.AppendLine(@"<div class=`folder-label`><span class=`folder-icon`><img class=`img-size` src=`" + image + "` " + imageExtra + "style =`" + visibility + "margin-right: " + th.pixelsAfterArrow + "`></span><span>" + G.Chop_RemoveFreq(G.Chop_RemoveBank(traceItem.Name), th.freq) + @"</span></div>");
                html.AppendLine(@"<div style = `margin-left:" + (-(depth - 1) * th.pixels) + @"px`>" + traceItem.Code + @"</div>");
                html.AppendLine(@"<div>" + traceItem.Active + @"</div>");
                html.AppendLine(@"<div>" + traceItem.Stamp + @"</div>");
                html.AppendLine(@"<div>" + traceItem.File + @"</div>");
                html.AppendLine(@"</div>");
                string extra = Trace2.FromTraceItemToDetailedText(traceItem, true);                
                html.AppendLine(@"<div class=`extra-content`>" + extra + @"</div>");
            }
            if (childrenCount > 0)
            {
                if (WalkTracesForHtmlIsPruned(th, depth))
                {
                    //Do not generate anything
                }
                else
                {
                    if (depth == 0) html.AppendLine(@"<ul>");
                    else html.AppendLine(@"<ul class=`nested`>");
                    int counter = -1;
                    foreach (TraceAndPeriods2 traceAndPeriods in trace.GetPrecedents_BewareOnlyInternalUse().GetStorage())
                    {
                        counter++;
                        if (traceAndPeriods.trace.type == ETraceType.Divider) continue;
                        html.AppendLine(@"<li class=`folder`>");

                        if (depth == 0 && counter == 0)
                        {
                            html.AppendLine(@"<div class=`list-item-content`>");
                            html.AppendLine(@"<div class=`folder-label`><span class=`folder-icon`><img class=`img-size` src =`../normal.png` style =`visibility: hidden; margin-right: " + th.pixelsAfterArrow + "`></span><span style = `font-weight: bold;`>Name</span></div>");
                            html.AppendLine(@"<div style = `font-weight: bold;`>Code</div>");
                            html.AppendLine(@"<div style = `font-weight: bold;`>Active</div>");
                            html.AppendLine(@"<div style = `font-weight: bold;`>Stamp</div>");
                            html.AppendLine(@"<div style = `font-weight: bold;`>File</div>");
                            html.AppendLine(@"</div>");
                        }

                        WalkTracesForHtml(traceAndPeriods.trace, traceAndPeriods.periods, th, depth + 1, html);
                        html.AppendLine(@"</li>");
                    }
                    html.AppendLine(@"</ul>");
                }
            }
        }

        private static bool WalkTracesForHtmlIsPruned(BrowserHelper th, int depth)
        {
            return depth >= th.depthMax || th.counter >= th.counterMax;
        }

        ///// <summary>
        ///// Finds equations that contain the given variable name.
        ///// </summary>
        ///// <param name="variableName"></param>
        ///// <param name="tUsedHere"></param>
        ///// <param name="model"></param>
        ///// <param name="modelGamsScalar"></param>
        ///// <returns></returns>
        //public static List<EqInfoSimple> GetRelatedEquations(string variableName, GekkoTime tUsedHere, Model model)
        //{
        //    List<EqInfoSimple> eqsNew = new List<EqInfoSimple>();
        //    ModelGamsScalar modelGamsScalar = model.modelGamsScalar;
        //    int aNumber = modelGamsScalar.dict_FromVarNameToANumber.GetInt(variableName);
        //    if (aNumber == -12345) return eqsNew;
        //    int timeIndex = modelGamsScalar.FromGekkoTimeToTimeInteger(modelGamsScalar.Maybe2000GekkoTime(tUsedHere));
        //    PeriodAndVariable pav = new PeriodAndVariable(timeIndex, aNumber);
        //    List<int> eqNumbers = null; modelGamsScalar.dependents.TryGetValue(pav, out eqNumbers);
        //    if (eqNumbers == null)
        //    {
        //        G.WarningInternal("Eq browser: '" + variableName + "' returns 'null' for eqNumbers");
        //        eqNumbers = new List<int>();
        //    }
        //    eqsNew = Gekko.Decomp.FindEquationsThatContainGivenVariableSorted(variableName, tUsedHere, eqNumbers, model);
        //    return eqsNew;
        //}

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

        private static void BrowserDependents(string varnameMaybeWithFreq, StringBuilder sb, bool isDanish, ref string jName, ref bool jNameAutoGen)
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
                if (isDanish) sb5.Append("Påvirker: ");
                else sb5.Append("Influences: ");
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
                        if (!G.IsUnitTestingOrNotShowingGUI())
                        {
                            DialogResult result = MessageBox.Show("All " + files.Length + " files in '" + folder + "' will be deleted", "Gekko helper", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            if (result == DialogResult.Yes)
                            {
                                //ok
                            }
                            else
                            {
                                new Error("User abort");
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
            //sb.AppendLine("<p style=`color:#993300; font-weight: bold`>" + s + "</p>");
            sb.AppendLine("<p style=`color:#993300`>" + s + "</p>");
        }

        public static void SpanHtmlColor(StringBuilder sb, string s)
        {
            sb.AppendLine("<span style=`color:#993300; font-weight: bold`>" + s + "</span>");
        }

        public static void SpanHtmlBold(StringBuilder sb, string s)
        {             
            sb.AppendLine("<span style=`font-weight:bold`>" + s + "</span>");
        }

        public static void SpanHtmlColorGray(StringBuilder sb, string s)
        {
            sb.AppendLine("<span style=`color:gray;`>" + s + "</span>");
        }

        public static void WriteHtmlColorGray(StringBuilder sb, string s)
        {
            sb.AppendLine("<p style=`color:gray`>" + s + "</p>");
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
            return HtmlLink(txt, SimplerName(txt) + ".html");
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

        private static void BrowserNewCssAndJs(string variableName, int firstColWidth, int pixels, int pixelsAfterArrow, List<EquationNameAndNumber> equations, bool levelUp, out StringBuilder x, out string js)
        {
            string s = null;
            foreach (EquationNameAndNumber equation in equations)
            {
                s += "updateTable('#" + SimplerName(equation.name) + "');" + G.NL;  //activate checkbox listeners for each decomp table
            }

            string up = null;
            if (levelUp) up = "../";
            x = new StringBuilder();
            x.AppendLine("<!DOCTYPE HTML PUBLIC `-//W3C//DTD HTML 4.01 Transitional//EN`>");
            x.AppendLine("<html>");
            x.AppendLine("  <head>");
            x.AppendLine("    <link rel=`stylesheet` href=`" + up + "styles.css" + @"` type=`text/css`>");
            x.AppendLine("    <meta http-equiv=`Content-Type` content=`text/html; charset=utf-8`>");
            x.AppendLine("    <title>" + variableName + "</title>");

            string css = @"<style>        
        
        .content {
            display: none;
        }

        .active {
            display: block;
        }

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

        table, th, td {
            padding: 2px;
        }
    
        .img-size {
            height:1em;
        }   

        .redcircle {
            display: inline-block;
            width: 0.57em;
            height: 0.57em;
            border-radius: 50%;
            background-color: #f01e3c;
            border: 0.10em solid gray;
            margin-right: 0.25em;
            position: relative; top: 0.27em;
        }

        .orangecircle {
            display: inline-block;
            width: 0.57em;
            height: 0.57em;
            border-radius: 50%;
            background-color: #ffc914;
            border: 0.10em solid gray;
            margin-right: 0.25em;
            position: relative; top: 0.27em;
        }

        .yellowcircle {
            display: inline-block;
            width: 0.57em;
            height: 0.57em;
            border-radius: 50%;
            background-color: #fafa0f;
            border: 0.10em solid gray;
            margin-right: 0.25em;
            position: relative; top: 0.27em;
        }

        .transparentcircle {
            display: inline-block;
            width: 0.57em;
            height: 0.57em;
            border-radius: 50%;
            background-color: #ffffff00;
            border: 0.15em solid #d7d7d7;
            margin-right: 0.25em;
            position: relative; top: 0.27em;
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

        .table1 td {  /* Variables */
            padding-right: 20px;
        }         
        
        .table-container { /* DECOMP table */
            width: 100%;
            max-width: 1000px; /* Optional: Adjust width of the container */
            max-height: 500px; /* Optional: Adjust height of the container */
            overflow: auto;    /* Enable scrolling */
            position: relative;
            display: none;
        }

        .table-container table {
            border-collapse: collapse;
            width: 100%;
            table-layout: fixed; /* Fixed size cells */
            font-family: Consolas;  
            font-size:13px;
        }

        .table-container th, .table-container td {
            padding: 5px;
            border: 1px solid #d0d7e5;
            width: 100px;
            height: 0px;
            font-weight: normal;
        }

        .table-container td {            
            text-align: right;
        }

        .table-container th {            
            text-align: left;
        }

        /* Sticky First Row (Header) */
        .table-container thead th {
            position: sticky;
            top: 0;
            background-color: #f8f8f8;
            z-index: 2; /* Ensures the header is above the body rows */            
        }

        /* Sticky First Column */
        .table-container tbody th {
            position: sticky;
            left: 0;
            background-color: #f8f8f8;
            z-index: 1; /* Lower than the header row but above the body cells */
        }

        /* Empty Top-Left Cell */
        .table-container thead th:first-child {
            position: sticky;
            top: 0;
            left: 0;
            z-index: 3; /* Prevent overlap and keep it at the top-left */
            background-color: white; /* Set the upper-left cell to a different color (e.g., white) */            
            border-left: 0 !important; /* Remove the left border */
            border-top: 0 !important;  /* Remove the top border */
        }

        .table-container thead th:first-child, .table-container tbody th {
            width: 150px; /* First col */
        }

        .toggle-content {
          padding: 10px;                
          padding-left: 20px;
          background-color: #fff3cd;
          border:5px solid #ffe69c;
          color: #664d03;
          border-radius: 10px;
        }    

        .toggle-link:after {
          content: `\25BC`; /* Down arrow */
          display: inline-block;
          margin-left: 2px;
          transform: rotate(0deg);
          transition: transform 0.3s ease-in-out;
        }

        .toggle-link.expanded:after {
          transform: rotate(180deg); /* Up arrow */
         }
   
        /* --------- tooltips ------------- */
        
        /* not working
        .more-info {
          /* border-bottom: 1px dotted; */
          position: relative;
        }

        .more-info .title {
            position: absolute;
            top: 20px;
            background: yellow;
            padding: 4px;
            left: 0;
            white-space: nowrap;
            z-index: 1000;
        }
        */

    </style>";

            x.AppendLine(css);

            js = @"<script>
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
                this.innerHTML = '<img class=`img-size` src=`../checked.png` style =`margin-right: " + pixelsAfterArrow + @"`>';
            } else
                                {
                                    this.innerHTML = '<img class=`img-size` src =`../normal.png`  style =`margin-right: " + pixelsAfterArrow + @"`>';
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

    const toggleLinks = document.querySelectorAll(`.toggle-link`);
    const toggleContents = document.querySelectorAll(`.toggle-content`);
                            toggleLinks.forEach((link, index) => {
                            link.addEventListener(`click`, (event) => {
        event.preventDefault();
        toggleContents[index].style.display = toggleContents[index].style.display === `block` ? `none` : `block`;
        link.classList.toggle(`expanded`);
      });
    });

    function showContent() {
      // Hide all content initially
      const contents = document.querySelectorAll('.content');
      contents.forEach(content => content.classList.remove('active'));

      // Get the hash from the URL
      const hash = window.location.hash;

      if (!hash) {
          document.getElementById('no-hash').classList.add('active');
      }
      else {        
        divs = 2;
        hashes = 4;
        // Show the corresponding content
        for (let i = 1; i <= divs; i++) 
        {
          document.getElementById(hash + '-' + i).classList.add('active'); 
        }
        for (let i = 1; i <= hashes; i++) 
        {
          document.getElementById('hash' + '-' + i).classList.add('active'); 
        }
      }
    }

    // Call showContent when the page loads
    window.onload = showContent;

    // Listen for hash changes
    window.onhashchange = showContent;

    // ------------ DECOMP selector -------------------------------

        
        // Function to show the right div based on checkbox values
        function updateTable(i) {            
            
            const checkbox_error = document.getElementById(i + '-' + 'checkbox_error');
            
            const checkbox_op = document.getElementById(i + '-' + 'checkbox_op');
            const op = checkbox_op.querySelector('input[name=`myradio`]:checked').value;

            // Add event listeners to checkboxes (needless to do every time, but makes more simple code)
            addEventListenerOnce(checkbox_op, 'change', function(){ updateTable(i); });
            addEventListenerOnce(checkbox_error, 'change', function(){ updateTable(i); });
            //checkbox_op.addEventListener('change', function(){ updateTable(i); });
            //checkbox_error.addEventListener('change', function(){ updateTable(i); });

            // Get the decompDivs
            const decompDivs = {
            div_d_yes: document.getElementById(i + '-' + 'decomp_d_yes'),
            div_d_no: document.getElementById(i + '-' + 'decomp_d_no'),
            div_p_yes: document.getElementById(i + '-' + 'decomp_p_yes'),
            div_p_no: document.getElementById(i + '-' + 'decomp_p_no'),
            };

            // Hide all divs
            Object.values(decompDivs).forEach(div => div.style.display = 'none');

            // Show the correct div based on checkbox states
            if (!(op == `p`) && !checkbox_error.checked) {
                decompDivs.div_d_no.style.display = 'block';
            } else if (!(op == `p`) && checkbox_error.checked) {
                decompDivs.div_d_yes.style.display = 'block';
            } else if ((op == `p`) && !checkbox_error.checked) {
                decompDivs.div_p_no.style.display = 'block';
            } else if ((op == `p`) && checkbox_error.checked) {
                decompDivs.div_p_yes.style.display = 'block';
            }
        }   

        function addEventListenerOnce(element, eventType, callback) {
        if (!element.hasOwnProperty(`_${eventType}`)) {
        element[`_${eventType}`] = true;
        element.addEventListener(eventType, callback);
        }
        }

        // -------------- tooltips click (for mobile)

        /* DOES NOT WORK COMPLETELY, hidden by next row.
        document.querySelectorAll('.more-info').forEach(function(element) {
          element.addEventListener('click', function() {
          const titleElement = element.querySelector('.title');
          if (!titleElement) {
          const newTitleElement = document.createElement('span');
          newTitleElement.className = 'title';
          newTitleElement.textContent = element.title;
          element.appendChild(newTitleElement);
        } else {
          element.removeChild(titleElement);
        }
        });
        });
        */
        
        // --------------- Initialize the display (show the default table)
        " + s + @"
</script>";
            x.AppendLine("  </head>");
        }
    }
}
