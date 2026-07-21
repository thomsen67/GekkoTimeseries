using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gekko
{
 
    /// <summary>
    /// Contains warning messsages that may be many in number, and similar.
    /// </summary>
    public class WarningPool
    {
        //See #lafh7h3bbkahfd

        public GekkoDictionary<string, WarningInfo> storage = new GekkoDictionary<string, WarningInfo>(StringComparer.OrdinalIgnoreCase);
        public int counter = 0;
        public Dictionary<string, bool> ignore0 = new Dictionary<string, bool>(); //no dot, like '3' or '5'.
        public Dictionary<string, bool> ignore1 = new Dictionary<string, bool>(); //one dot, like '2.3' or '5.2'.        

        public Dictionary<string, string> warningStrings = new Dictionary<string, string>()
        {            
            //    +-------------------------------------------------------------------------------+
            //    | NEW WARNING: Always augment and use a new number!                             |
            //    |              For a new type when "{x}.{y}" is last, add "{x+1}.1" etc.        |
            //    |              For a new {x} subtype when "{x}.{y}" is last, add "{x}.{y+1}".   |
            //    | Do not mess with existing numbers. Changing the text is ok, use no dots (".").|
            //    | Keep messages as short as reasonably possible.                                |
            //    +-------------------------------------------------------------------------------+
            //
            // =========================================================
            // =========================================================
            {"", "Unknown type" },  //This should never happen...
            // =========================================================
            // =========================================================
            {"w1", "GAMS raw model file reading" },
            // ---------------------------------------------------------
            {"w1.1", "Could not find '=e=' in eq def" },
            {"w1.2", "Could not find ending ';' in eq def" },
            {"w1.3", "Eq name with '__'" },
            {"w1.4", "Eq name without '_'" },
            {"w1.5", "Eq name with no 'e_'" },
            {"w1.6", "Eq name invalid" },
            {"w1.7", "Parsing error" },            
            // =========================================================
            // =========================================================
            {"w2", "Tsd file reading/writing" },
            // ---------------------------------------------------------
            {"w2.1", "Reading: empty string" },
            {"w2.2", "Reading: small number" },
            {"w2.3", "Reading: missing values" },
            {"w2.4", "Writing: internal error" },
            // =========================================================
            // =========================================================
            {"w3", "Databank" },
            // ---------------------------------------------------------
            {"w3.1", "OPEN<ref> problem" },
            {"w3.2", "Missing variable" },
            {"w3.3", "Variable names with '___'" },
            {"w3.4", "Filed to write a non-editable databank, even though it contents seems changed. You may use UNLOCK to unlock a databank." },
            // =========================================================
            // =========================================================
            {"w4", "Equation html browser" },
            // ---------------------------------------------------------
            {"w4.1", "Json file" },
            {"w4.2", "Parse problem" },
            {"w4.3", "Missing setting" },
            // =========================================================
            // =========================================================
            {"w5", "Function asbRename()" },
            // ---------------------------------------------------------
            {"w5.1", "Dublet problem" },
            // =========================================================
            // =========================================================
            {"w6", "GAMS parsing" },
            // ---------------------------------------------------------
            {"w6.1", "GAMS parse error" },
            {"w6.2", "GAMS parse error (other type)" },
            // =========================================================
            // =========================================================
            {"w7", "Decomp" },
            // ---------------------------------------------------------
            {"w7.1", "Variable problem (GAMS)" },
            {"w7.2", "Operator problem (Gekko)" },
            // =========================================================
            // =========================================================
            {"w8", "Interface" },
            // ---------------------------------------------------------
            {"w8.1", "Links" },
            // =========================================================
            // =========================================================
            {"w9", "Library" },
            // ---------------------------------------------------------
            {"w9.1", "Name problem" },
            {"w9.2", "Name collision" },
            // =========================================================
            // =========================================================
            {"w10", "File system" },
            // ---------------------------------------------------------
            {"w10.1", "Path problem" },
            {"w10.2", "Protobuffer file problem" },
            {"w10.3", "File non-existence" },
            // =========================================================
            // =========================================================
            {"w11", "Mode" },
            // ---------------------------------------------------------
            {"w11.1", "Databanks" },
            {"w11.2", "Printing" },
            {"w11.3", "Models" },
            // =========================================================
            // =========================================================
            {"w12", "Printing" },
            // ---------------------------------------------------------
            {"w12.1", "Skip variable" },
            {"w12.2", "Unknown type" },
            {"w12.3", "Illegal label type" },
            // =========================================================
            // =========================================================
            {"w13", "Download" },
            // ---------------------------------------------------------
            {"w13.1", "Json file" },
            {"w13.2", "File access" },
            // =========================================================
            // =========================================================
            {"w14", "Plot" },
            // ---------------------------------------------------------
            {"w14.1", "File problem" },
            // =========================================================
            // =========================================================
            {"w15", "GAMS raw model file reading" },
            // ---------------------------------------------------------
            {"w15.1", "Missing file" },
            // =========================================================
            // =========================================================
            {"w16", "Smooth" },
            // ---------------------------------------------------------
            {"w16.1", "Missing data" },
            // =========================================================
            // =========================================================
            {"w17", "Wildcard" },
            // ---------------------------------------------------------
            {"w17.1", "Frequency problem" },
            // =========================================================
            // =========================================================
            {"w18", "Pipe" },
            // ---------------------------------------------------------
            {"w18.1", "Syntax suggestion" },
            // =========================================================
            // =========================================================
            {"w19", "Zip" },
            // ---------------------------------------------------------
            {"w19.1", "File problem" },
            // =========================================================
            // =========================================================
            {"w20", "Time periods" },
            // ---------------------------------------------------------
            {"w20.1", "Filtering" },
            // =========================================================
            // =========================================================
            {"w21", "Shell" },
            // ---------------------------------------------------------
            {"w21.1", "System commmand" },
            // =========================================================
            // =========================================================
            {"w22", "Csv file reading" },
            // ---------------------------------------------------------
            {"w22.1", "Format problem" },
            // =========================================================
            // =========================================================
            {"w23", "Prn file reading" },
            // ---------------------------------------------------------
            {"w23.1", "Format problem" },
            // =========================================================
            // =========================================================
            {"w24", "Model" },
            // ---------------------------------------------------------
            {"w24.1", "Formula code" },
            {"w24.2", "Model type problem" },
            {"w24.3", "LHS problem" },
            {"w24.4", "Damping variable" },
            // =========================================================
            // =========================================================
            {"w25", "Solve" },
            // ---------------------------------------------------------
            {"w25.1", "Fair-Taylor convergence" },
            {"w25.2", "Ordering problem" },
            // =========================================================
            // =========================================================
            {"w26", "Translate" },
            // ---------------------------------------------------------
            {"w26.1", "Investigation" },
            {"w26.2", "AREMOS problem" },
            // =========================================================
            // =========================================================
            {"w27", "Fencing" },
            // ---------------------------------------------------------
            {"w27.1", "Illegal folder" },
            // =========================================================
            // =========================================================
            {"w28", "Data-tracing" },
            // ---------------------------------------------------------
            {"w28.1", "Version problem" },
            {"w28.2", "Write problem" },
            // =========================================================
            // =========================================================
            {"w29", "Seasonal adjustment" },
            // ---------------------------------------------------------
            {"w29.1", "Calculation problem" },
            {"w29.2", "Laspchain() function: note that incoming prices are probably 0 for some periods, and that this is handled differently i Gekko >= 3.1.20" },
            // =========================================================
            // =========================================================
            {"w30", "Copying" },
            // ---------------------------------------------------------
            {"w30.1", "Missing TO part" },
            // =========================================================
            // =========================================================
            {"w31", "Compare" },
            // ---------------------------------------------------------
            {"w31.1", "Missing values" },            
            // =========================================================
            // =========================================================
            {"w32", "Cache file reading" },
            // ---------------------------------------------------------
            {"w32.1", "Reading problem (protobuf)" },
            // =========================================================
            // =========================================================
            {"w33", "AREMOS file reading" },
            // ---------------------------------------------------------
            {"w33.1", "Problem reading file" },
            // =========================================================
            // =========================================================
            {"w34", "PC-AXIS file reading" },
            // ---------------------------------------------------------
            {"w34.1", "Could not read file" },
            {"w34.2", "PC-AXIS time problem" },
            {"w34.3", "PC-AXIS time gaps" },
            // =========================================================
            // =========================================================
            {"w35", "PCIM file reading" },
            // ---------------------------------------------------------
            {"w35.1", "Problem reading file" },
            // =========================================================
            // =========================================================
            {"w36", "Gdx file reading" },
            // ---------------------------------------------------------
            {"w36.1", "Problem with gdx (GAMS) file" },
            {"w36.2", "Could not create GAMS/gdx environment" },            
            // =========================================================
            // =========================================================
            {"w37", "Table" },
            // ---------------------------------------------------------
            {"w37.1", "Operator problem" },
            {"w37.2", "Integrity" },
            // =========================================================
            // =========================================================
            {"w38", "OLS estimation" },
            // ---------------------------------------------------------
            //{"w38.1", "..." },
            // =========================================================
            // =========================================================
            {"w39", "Menu" },
            // ---------------------------------------------------------
            {"w39.1", "Style sheet" },
            {"w39.2", "File name" },            
            // =========================================================
            // =========================================================
            {"w40", "TSP" },
            // ---------------------------------------------------------
            {"w40.1", "Converter" },
            // =========================================================
            // =========================================================
            {"w41", "Interpolate" },
            // ---------------------------------------------------------
            {"w41.1", "Denton: inconsistent levels" },
            {"w41.2", "Denton: Please consider using Denton-Cholette ('cholette'), which is a much better method unless you are collapsing extremely long timeseries." },
            // =========================================================
            // =========================================================
            {"w42", "Deprecated" },
            // ---------------------------------------------------------
            {"w42.1", "Using explicit time parameters like laspchain(%t1, %t2, ...) will be removed in Gekko 4.0. Please use laspchain(<%t1, %t2>, ...) instead." },
            // =========================================================
            // =========================================================
            {"w43", "Gdx file writing" },
            // ---------------------------------------------------------
            {"w43.1", "Could not create GAMS/gdx environment" },
            // =========================================================
            // =========================================================
            {"w44", "Parquet file reading" },
            // ---------------------------------------------------------
            {"w44.1", "Consistency" },
            // =========================================================
            // =========================================================
            // =========================================================
            // =========================================================
            // =========================================================
            // =========================================================
            // =============== INTERNAL WARNINGS =======================
            // =========================================================
            // =========================================================
            // =========================================================
            // =========================================================
            // =========================================================
            // =========================================================
            {"w0", Globals.internalGekkoWarningString },
            // ---------------------------------------------------------
            {"w0.1", "System problem" },  //See #khssjksd7j
        };

        /// <summary>
        /// The info string may be null. Else info is small warning information bit, like left-hand side variable etc. Should be rather small in size.
        /// First argument always "x.y"! 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="info"></param>
        public void WAdd(string s2, string info, bool isUsingType, out bool shouldPrint)  //WAdd() so it is easier to find by search like .Wadd("1.1"
        {
            string s = s2;
            bool isInternal = false;
            if (G.Equal(s, Globals.INTERNAL))
            {
                s = "w0.1";
                if (Globals.runningOnTTComputer || G.IsUnitTestingOrNotShowingGUI())
                {
                    isInternal = true;  //For Gekko developer
                }
                else
                {
                    shouldPrint = false;
                    return;  //Ignore this completely if it is a "normal" user calling it
                }
            }

            // ============= Limits ====================================

            this.counter++;
            WarningInfo wi = null;
            this.storage.TryGetValue(s, out wi);
            int n1 = 0;
            int n2 = 0;
            if (wi != null)
            {
                n1 = wi.storage.Count;
                n2 = wi.printCounter;  //how many of this type have already been printed?
                wi.printCounter++;  //has been called 1 more time: we limit this print at 5.
            }

            bool setting1of3_add = false;
            bool setting2of3_print = Program.options.global_warnings_print;  //normally true
            int setting3of3_popup = 0;  //1:normal popup, 2:find-popup.
            bool lastPrintedWarningOfThisType = false;

            if (Program.options.global_warnings_limit >= 0)
            {
                //For adding to the dictionary (for printing last), we look at n, counting different variants of types.
                if (n1 < Program.options.global_warnings_limit)  //limit like e.g. 5
                {
                    setting1of3_add = true;
                }

                //For printing on screen, we look at n2, counting each time a type is issued
                if (n2 >= Program.options.global_warnings_limit)  //limit like e.g. 5                
                {
                    setting2of3_print = false;
                }

                if (Program.options.global_warnings_limit >= 3 && n2 == Program.options.global_warnings_limit - 1)  //limit like e.g. 5                
                {
                    lastPrintedWarningOfThisType = true;
                }
            }
            else if (Program.options.global_warnings_limit == -1)  //add/print all, same as int.MaxValue
            {
                setting1of3_add = true;
                setting2of3_print = true;
            }
            else if (Program.options.global_warnings_limit == -2)  //pause each
            {
                setting3of3_popup = 1;
                setting1of3_add = true;
                setting2of3_print = true;
            }
            else
            {
                new Error("Expected option global warnings limit to be >= -2.");
            }

            string s1, s1s2; WarningPool.SplitByDot(s, out s1, out s1s2);

            if (this.ignore0.ContainsKey(s1) || this.ignore1.ContainsKey(s1s2))
            {
                setting1of3_add = false;
                setting2of3_print = false;
            }

            if (!G.NullOrBlanks(Program.options.global_warnings_pauseat))
            {
                if (G.Contains(this.GetWarningText(s, info), Program.options.global_warnings_pauseat.Trim())) setting3of3_popup = 2;
            }

            if (isInternal)
            {
                //No matter options, these are added and printed with no popup.
                setting1of3_add = true;
                setting2of3_print = true;
                setting3of3_popup = 0;
            }

            // ------------------------------------------------
            // ------------------------------------------------
            // ----- setting_... are processed ----------------
            // ------------------------------------------------
            // ------------------------------------------------

            if (setting1of3_add)
            {
                if (wi == null)
                {
                    wi = new WarningInfo();
                    wi.storage.Add(info, counter);
                    this.storage.Add(s, wi);
                }
                else
                {
                    if (!wi.storage.ContainsKey(info))
                    {
                        wi.storage.Add(info, counter);
                    }
                    else
                    {
                        //already seen
                    }
                }
            }

            shouldPrint = setting2of3_print;  //This bool is not used if isUsingType==false.
            if (setting2of3_print)
            {
                if (!isUsingType)
                {
                    string skip = null;
                    if (lastPrintedWarningOfThisType) skip = ". --> Further warnings of this type are not printed (cf. option global warnings limit).";
                    new Warning(EWarningType.NoUsing, this.GetWarningText(s, info + skip));
                }
            }

            if (setting3of3_popup == 1)
            {
                WindowMessageBox w = new WindowMessageBox(EMessageBox.Pause);
                w.textBox1.Text = "+++ WARNING: " + this.GetWarningText(s, info) + "." + G.NL + G.NL + "Press [Enter] to continue";
                w.ShowDialog();
            }
            else if (setting3of3_popup == 2)
            {
                WindowMessageBox w = new WindowMessageBox(EMessageBox.Pause);
                w.textBox1.Text = "Warning text '" + Program.options.global_warnings_pauseat + "' encountered as part of the warning message '" + this.GetWarningText(s, info) + "'." + G.NL + G.NL + "To switch such pausing off, use: option interface pause = '';" + G.NL + G.NL + "Press [Enter] to continue";
                w.ShowDialog();
            }
        }

        private string GetWarningText(string s, string info)
        {
            string w1, w2;
            this.GetText(s, null, out w1, out w2);
            string warningText = w1 + " " + w2 + " " + info;
            return warningText;
        }

        public void GetIgnores()
        {
            this.ignore0 = new Dictionary<string, bool>();
            this.ignore1 = new Dictionary<string, bool>();
            if (!G.NullOrBlanks(Program.options.global_warnings_ignore))
            {
                string[] ss = Program.options.global_warnings_ignore.Split(',');
                Dictionary<string, bool> ignore2 = new Dictionary<string, bool>();
                foreach (string s2 in ss)
                {
                    string s = s2.Trim();
                    //Element must be something like "w2" or "w2.3".
                    if (G.NullOrBlanks(s)) new Error("Empty element: option global warnings ignore = '" + Program.options.global_warnings_ignore + "'");
                    int n = G.Count(s, ".");
                    if (n > 1) new Error("Element '" + s + "' with > 1 dots ('.'): option global warnings ignore = '" + Program.options.global_warnings_ignore + "'");
                    if (!G.Equal(s.Substring(0, 1), "w")) new Error("Invalid element '" + s + "': option global warnings ignore = '" + Program.options.global_warnings_ignore + "'");
                    if (!G.IsInteger(s.Substring(1).Replace(".", ""))) new Error("Invalid element '" + s + "': option global warnings ignore = '" + Program.options.global_warnings_ignore + "'");

                    if (n == 0)
                    {
                        if (this.ignore0.ContainsKey(s)) new Error("Dublets encountered: option global warnings ignore = '" + Program.options.global_warnings_ignore + "'");
                        this.ignore0.Add(s, false);
                    }
                    else if (n == 1)
                    {
                        if (this.ignore1.ContainsKey(s)) new Error("Dublets encountered: option global warnings ignore = '" + Program.options.global_warnings_ignore + "'");
                        this.ignore1.Add(s, false);
                    }
                }
            }
        }

        public void Report()
        {
            if (this.storage.Count > 0)
            {
                //#lafh7h3bbkahfd

                //1. GAMS raw file reading.                  precooked
                //  1.1 Could not find '=e=',                precooked          this.storage.Count
                //    File problem in line 117 pos 10.       random             this.storage["..."].storage.Count
                //
                //It gets stored with "1.1" as key, and value as a dict containing details like
                //"File problem in line 117 pos 10".

                using (Writeln txt = new Writeln())
                {
                    txt.color = Globals.warningColor;
                    Dictionary<string, bool> level2Numbers = new Dictionary<string, bool>();

                    foreach (KeyValuePair<string, WarningInfo> kvp in this.storage)
                    {
                        string w1, w2;
                        this.GetText(kvp.Key, level2Numbers, out w1, out w2);
                    }

                    Action<GAO> a3 = (gao) =>
                    {
                        List<WarningPoolHelper> m = new List<WarningPoolHelper>();
                        foreach (KeyValuePair<string, WarningInfo> kvp in this.storage)
                        {
                            string w1, w2;
                            this.GetText(kvp.Key, null, out w1, out w2);
                            foreach (KeyValuePair<string, int> kvp2 in kvp.Value.storage)
                            {
                                string w3 = kvp2.Key.Trim();
                                if (!w3.EndsWith(".")) w3 += ".";
                                string ss = w1 + " " + w2;
                                if (w1 == null && w2 == null) ss = "[Warning text problem].";  //should not happen
                                string s9 = ss + " " + w3;
                                if (w3 == ".") s9 = ss;
                                m.Add(new WarningPoolHelper() { s = s9, id = kvp.Key, i = kvp2.Value });
                            }
                        }
                        this.PrintWarnings(m, false);
                    };

                    int n = 0;
                    foreach (KeyValuePair<string, WarningInfo> kvp in this.storage)
                    {
                        n += kvp.Value.storage.Count;
                    }

                    string s5 = "There were " + n + " distinct WARNING messages";
                    if (n == 1) s5 = "There was " + n + " distinct WARNING message";
                    txt.MainAdd(s5 + " while running the job (" + G.GetLinkAction("show warnings", new GekkoAction(EGekkoActionTypes.Unknown, null, a3)) + ")");

                    if (Globals.runningOnTTComputer || G.IsUnitTestingOrNotShowingGUI())
                    {
                        bool hasInternalWarnings = false;
                        foreach (KeyValuePair<string, WarningInfo> kvp in this.storage)
                        {
                            string w1, w2;
                            this.GetText(kvp.Key, null, out w1, out w2);
                            if (w1 != null && w1.Contains(Globals.internalGekkoWarningString)) hasInternalWarnings = true;
                        }
                        if (hasInternalWarnings)
                        {
                            txt.MainNewLine();
                            txt.MainAdd("=====> TTH: Internal warnings, See list!");
                            txt.MainNewLineTight();
                            txt.MainAdd("=====> TTH: Internal warnings, See list!");
                            txt.MainNewLineTight();
                            txt.MainAdd("=====> TTH: Internal warnings, See list!");
                            txt.MainNewLineTight();
                        }
                    }
                }
            }
        }

        public void PrintWarnings(List<WarningPoolHelper> m, bool showId)
        {
            Action<GAO> a = (gao) =>
            {
                PrintWarnings(m, true);
            };

            using (Writeln txt3 = new Writeln())
            {
                txt3.tab = ETabs.Output;
                List<WarningPoolHelper> m2 = m.OrderBy(o => o.i).ToList(); //sort chronologically                            
                if (showId)
                {
                    txt3.MainAdd("Turn off particular warning id's with syntax like this: option global warnings ignore = 'w2.1, w3.2';. See {a{option¤option.htm}a}.");
                }
                else
                {
                    txt3.MainAdd("Click " + G.GetLinkAction("here", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + " to show messages with id numbers (you may use id's to turn off particular warnings). See also these: option global warnings ignore, option global warnings limit, option global warnings pauseat, option global warnings print (cf. {a{option¤option.htm}a}).");
                }
                txt3.MainNewLine();
                txt3.MainAdd("-----------------------------------------------------------------------");
                txt3.MainNewLine();

                GekkoDictionary<string, int> dublets = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                foreach (WarningPoolHelper wph in m2)
                {
                    if (wph.id == "w0.1")  //See #khssjksd7j
                    {
                        if (!Globals.runningOnTTComputer)
                        {                            
                            continue;  //skip internal warnings for normal users.
                        }
                    }
                    
                    if (showId) txt3.MainAdd(wph.id + ": " + wph.s);
                    else txt3.MainAdd(wph.s);
                    txt3.MainNewLine();
                    if (dublets.ContainsKey(wph.id)) dublets[wph.id] += 1;
                    else dublets.Add(wph.id, 1);
                    if (Program.options.global_warnings_limit > 0 && dublets[wph.id] >= Program.options.global_warnings_limit)
                    {
                        txt3.MainAdd("...[possibly more of above type, cf. 'option global warnings limit']...");
                        txt3.MainNewLine();
                    }
                }
            }
        }

        /// <summary>
        /// For a string s like "2.3" it will spit out w1 as text for "2" and w2 as text for "3". 
        /// If level1 and level2 are non-null, it will also "2" and "2.3" into level1 and level2 respectively.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="level1"></param>
        /// <param name="level2"></param>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        private void GetText(string s, Dictionary<string, bool> level2, out string w1, out string w2)
        {
            //kvp.Key is alway something like "1.1", "5.3" and so on. Each of these have 1 or more elements.  
            string s1, s1s2; WarningPool.SplitByDot(s, out s1, out s1s2);
            if (level2 != null && !level2.ContainsKey(s1s2)) level2.Add(s1s2, false); //"1.1", "3.2", etc.
            this.GetTextHelper(s1, s1s2, out w1, out w2);
        }

        /// <summary>
        /// For a string s like "w2.3", it returns "w2" and "w2.3". For "w2" it will return "w2" and "w2.0". 
        /// Will handle blanks etc. If illegal, it will return "0" and "0.0".
        /// </summary>
        /// <param name="s"></param>
        /// <param name="s1"></param>
        /// <param name="s1s2"></param>
        public static void SplitByDot(string s, out string s1, out string s1s2)
        {
            if (G.NullOrBlanks(s))
            {
                s1 = "0"; s1s2 = "0.0";
                return;
            }
            string[] ss = s.Split('.');
            if (ss.Length == 1) ss = new string[2] { s.Trim(), "0" };
            else if (ss.Length != 2) ss = new string[2] { "0", "0" };  //so it does not crash (length == 0, 3, 4, ...)
            s1 = ss[0].Trim();
            string s2 = ss[1].Trim();
            s1s2 = s1 + "." + s2;
        }

        /// <summary>
        /// For input numbers like "2" and "2.3" it returns the two corresponding labels for type and sub-type. Returned labels end with ".".
        /// May return null for returned labels if not found.
        /// </summary>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        private void GetTextHelper(string number1, string number2, out string w1, out string w2)
        {
            w1 = null;
            this.warningStrings.TryGetValue(number1, out w1);
            w2 = null;
            this.warningStrings.TryGetValue(number2, out w2);

            if (w1 != null)
            {
                w1 = w1.Trim();
                if (!w1.EndsWith(".")) w1 += ".";
            }
            if (w2 != null)
            {
                w2 = w2.Trim();
                if (!w2.EndsWith(".")) w2 += ".";
            }
        }
    }

    /// <summary>
    /// Small helper class.
    /// </summary>
    public class WarningPoolHelper
    {
        public string s;
        public string id;
        public int i;
    }

    /// <summary>
    /// Small warning information bits, like left-hand side variable etc. The added strings should be rather small in size. 
    /// When adding, at counter is added as value (always increment by 1). This is for easier sorting when reporting.
    /// </summary>
    public class WarningInfo
    {
        //value is not used
        public GekkoDictionary<string, int> storage = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public int printCounter = 1;
    }
}
