using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Gekko
{
    /// <summary>
    /// This class acts as an interface that all method calls from dynamic code goes through. 
    /// The purpose is to make the calls more transparent, and to make refactoring easier.
    /// For instance, changing the name of "Prt()" here would entail searching for all "O.Prt" strings
    /// in the code.    
    /// </summary>
    /// 
    public class DecompPrecedent
    {
        public string s = null;
        
        public DecompPrecedent(string s, IVariable iv)
        {
            this.s = s;
            //this.x = iv;
        }
    }

    public class MissingMemory
    {
        public ESeriesMissing print;
        public ESeriesMissing calc;
        public ESeriesMissing data;
    }

    public class Link
    {
        public string expressionText = null;
        public List<Func<GekkoSmpl, IVariable>> expressions = null;
        public List<string> varnames = null;
        public List<string> endo = null; //for links item after the first one, varnames may be 
        public string eqname = null;
        public string option = null;
    }

    public class DecompItems
    {
        public Func<GekkoSmpl, IVariable> expression = null;
        public IVariable varnames = null;
        public IVariable eqname = null;
        public IVariable option = null;

        public DecompItems(Func<GekkoSmpl, IVariable> expression, IVariable name1, IVariable name2, IVariable option)
        {
            this.expression = expression;
            this.varnames = name1;
            this.eqname = name2;
            this.option = option;
        }
    }

    public class LookupSettings
    {
        public O.ECreatePossibilities create = O.ECreatePossibilities.NoneReportError;
        public O.ELookupType type = O.ELookupType.RightHandSide;
        public bool canSearch = true;
        public short depth = 0; //used to avoid recursions with #alias list. #6324987324234       

        public LookupSettings()
        {            
        }

        public LookupSettings(O.ELookupType type)
        {
            this.type = type;            
        }

        public LookupSettings(O.ELookupType type, O.ECreatePossibilities create)
        {
            this.type = type;
            this.create = create;            
        }

        public LookupSettings(O.ELookupType type, O.ECreatePossibilities create, bool canSearch)
        {
            this.type = type;
            this.create = create;
            this.canSearch = canSearch;
        }
               
    }

    public static class O
    {
        //Common methods start
        //Common methods start
        //Common methods start

        public static ScalarString scalarStringPercent = new ScalarString(Globals.symbolScalar.ToString());
        public static ScalarString scalarStringHash = new ScalarString(Globals.symbolCollection.ToString());
        public static ScalarString scalarStringTilde = new ScalarString(Globals.freqIndicator.ToString());
        public static ScalarString scalarStringColon = new ScalarString(Globals.symbolBankColon.ToString());
        public static ScalarString scalarStringExclamation = new ScalarString(Globals.freqIndicator.ToString());

        public enum ELoopType
        {
            ForTo,
            List
        }

        public enum EIndexerType
        {
            None,
            IndexerLag,
            IndexerLead,
            Dot
        }

        public enum ELookupType
        {
            LeftHandSide,
            RightHandSide,
            //RightHandSideLoneVariable
        }

        public enum ECreateType
        {
            Find,        //error if not found
            Create,      //created if not found
            Overwrite    //overwritten if existing, else created
        }

        public enum LagType
        {
            Pch,
            Pchy,
            Dif,
            Dify,
            Dlog,
            Dlogy,
            Movavg,
            Movsum,
            Lag
        }

        public static double Exp(double x)
        {
            return Math.Exp(x);
        }

        public static double Abs(double x)
        {
            return Math.Abs(x);
        }

        public static double Log(double x)
        {
            return Math.Log(x);
        }

        public static double Special_Log(double x)
        {
            if (x < 0)
            {
                double d = NewtonStartingValuesFixHelper1(x);
                return O.Log(d);
            }
            else return O.Log(x);
        }

        public static double Pow(double x1, double x2)
        {
            //special treatment of x^2
            return x2 == 2d ? x1 * x1 : Math.Pow(x1, x2);
        }

        public static double Special_Pow(double x1, double x2)
        {
            if (x1 < 0)
            {
                double d = NewtonStartingValuesFixHelper1(x1);
                return O.Pow(d, x2);
            }
            else return O.Pow(x1, x2);
        }

        private static double NewtonStartingValuesFixHelper1(double x)
        {
            //x is < 0 here
            if (Globals.newtonRobustHelper1 == -12345) return x;
            double distance = -x + Globals.newtonRobustHelper3;  //distance will be > 0
            if (Globals.newtonRobustHelper1 < Globals.newtonRobustHelper2.Length)
            {
                Globals.newtonRobustHelper2[Globals.newtonRobustHelper1] = distance;
                Globals.newtonRobustHelper1++;
            }
            else
            {
                //ignore recording of this distance
            }
            double d = Globals.newtonRobustHelper3;
            return d;
        }


        public static bool isTableCall = false;

        public static string ShowDatesAsString(GekkoTime t1, GekkoTime t2)
        {
            string s = null;
            if (t1.IsNull() || t2.IsNull())
            {
            }
            else
            {
                s = G.FromDateToString(t1) + "-" + G.FromDateToString(t2) + ": ";
            }
            return s;
        }        

        public class HandleEndoHelper2
        {
            public GekkoTimes global = null;
            public List<HandleEndoHelper> helper = null;
        }

        public class LabelHelperIVariable
        {
            public int index;
            public IVariable iv;
            public LabelHelperIVariable(int index, IVariable iv)
            {
                this.index = index;
                this.iv = iv;
            }
        }

        public class RecordedPieces
        {
            public string s;
            public IVariable iv;
            public RecordedPieces(string s, IVariable iv)
            {
                this.s = s;
                this.iv = iv;
            }
        }

        public class HandleEndoHelper
        {
            public GekkoTimes local = null;
            public IVariable varname = null;
            public List<IVariable> indices = null;
        }

        public class GekkoListIterator : IEnumerable<ScalarString>
        {
            private List _ml = null;

            public GekkoListIterator(IVariable list)
            {
                if (list.Type() != EVariableType.List)
                {
                    G.Writeln2("*** ERROR: Expected a list in iterator");
                    throw new GekkoException();
                }
                _ml = (List)list;
            }

            public IEnumerator<ScalarString> GetEnumerator()
            {
                //#98073245243875
                foreach (IVariable iv in _ml.list)
                {
                    string s = O.ConvertToString(iv);
                    yield return new ScalarString(s);
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                G.Writeln("*** ERROR: iterator problem");
                throw new GekkoException();
            }
        }

        public static void ListQuestion(string type)
        {

            bool hasLargeModel = Program.IsLargeModel();
            List<string> a4 = new List<string>();
            foreach (KeyValuePair<string, IVariable> kvp in Program.databanks.GetFirst().storage)
            {
                if (kvp.Value.Type() == EVariableType.List)
                {
                    string s = kvp.Key.Substring(1);
                    a4.Add(s);
                }
            }

            a4.Sort(StringComparer.InvariantCultureIgnoreCase);  //invariant is better for sorting than ordinal

            List<string> user = new List<string>();
            List<string> system = new List<string>();
            foreach (string m in a4)
            {
                if (
                G.Equal(m, "exod") ||
                G.Equal(m, "exoj") ||
                G.Equal(m, "exoz") ||
                G.Equal(m, "exodjz") ||
                G.Equal(m, "exo") ||
                G.Equal(m, "exotrue") ||
                G.Equal(m, "endo") ||
                G.Equal(m, "all"))
                {
                    system.Add(m);
                }
                else
                {
                    user.Add(m);
                }
            }

            int count = a4.Count;

            if (type == "?")
            {
                G.Writeln();
                G.Write("There are " + user.Count + " user lists and " + system.Count + " model lists.");
                if (system.Count > 0)
                {
                    G.Write(" Click ");
                    G.WriteLink("here", "list:?_show_all_lists");
                    G.Write(" to see model lists.");
                }
                G.Writeln();
                if (user.Count > 0)
                {
                    foreach (string m in user)
                    {
                        Program.WriteListItems(m);
                    }
                }
            }
            else //must be ?_show_all_lists
            {
                if (hasLargeModel) G.Writeln();
                foreach (string m in system)
                {
                    if (hasLargeModel)
                    {
                        List<string> a1 = Program.GetListOfStringsFromList(Program.databanks.GetFirst().GetIVariable(Globals.symbolCollection + m));
                        G.Write("list #" + m + " = ["); G.WriteLink("show", "list:?_" + m); G.Writeln("]  (" + a1.Count + " elements from '" + a1[0] + "' to '" + a1[a1.Count - 1] + "')");
                        G.Writeln();
                    }
                    else
                    {
                        Program.WriteListItems(m);
                    }
                }
            }
        }

        public static IVariable Add(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return x.Add(smpl, y);
        }

        public static IVariable Add(GekkoSmpl smpl, IVariable x, IVariable y, IVariable z)
        {
            return x.Add(smpl, y).Add(smpl, z);
        }

        public static IVariable Subtract(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return x.Subtract(smpl, y);
        }

        public static IVariable Multiply(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return x.Multiply(smpl, y);
        }

        public static IVariable Divide(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return x.Divide(smpl, y);
        }

        public static IVariable Percent(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            //G.Writeln2("%");
            Series x_series = x as Series;
            AssignmentError(x_series, "%= operator");
            return x.Divide(smpl, y);
        }

        public static IVariable Hash(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            //G.Writeln2("#");
            Series x_series = x as Series;
            AssignmentError(x_series, "#= operator");
            return x.Divide(smpl, y);
        }

        public static IVariable Hat(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            //G.Writeln2("^");
            Series x_series = x as Series;            
            AssignmentError(x_series, "^= operator");
            foreach (GekkoTime t in smpl.Iterate03())
            {
                double x_lag = x_series.GetData(smpl, t.Add(-1));
            }
            return x.Divide(smpl, y);
        }

        public static int MaxLag()
        {
            return Program.options.decomp_maxlag;
        }

        public static int MaxLead()
        {
            return Program.options.decomp_maxlead;
        }

        //private static void AssignmentError(Series x_series, string s)
        //{
        //    if (x_series == null)
        //    {
        //        G.Writeln2("*** ERROR: You can only use " + s + " operator on series type");
        //        throw new GekkoException();
        //    }
        //    if (x_series.type != ESeriesType.Normal)
        //    {
        //        G.Writeln2("*** ERROR: You can only use " + s + " operator on a normal series type");
        //        throw new GekkoException();
        //    }
        //}

        public static IVariable Power(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return x.Power(smpl, y);
        }

        public static IVariable Negate(GekkoSmpl smpl, IVariable x)
        {
            return x.Negate(smpl);
        }

        //public static IVariable AndAdd(GekkoSmpl smpl, IVariable x, IVariable y)
        //{
        //    return Functions.union(smpl, x, y);
        //}

        //public static IVariable AndSubtract(GekkoSmpl smpl, IVariable x, IVariable y)
        //{
        //    return Functions.difference(smpl, x, y);
        //}

        //public static IVariable AndMultiply(GekkoSmpl smpl, IVariable x, IVariable y)
        //{
        //    return Functions.intersect(smpl, x, y);
        //}

        public static IVariable Union(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return Functions.union(smpl, null, null, x, y);
        }        

        public static IVariable Intersect(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return Functions.intersect(smpl, null, null, x, y);
        }

        public static IVariable ReplaceSlashHelper(IVariable x)
        {
            if (x.Type() != EVariableType.String) return x;
            ScalarString ss = x as ScalarString;
            if (ss.string2.Contains("/"))
            {
                x = new ScalarString(ss.string2.Replace("/", "\\"));
            }
            return x;
        }

        public static IVariable ReplaceSlash(IVariable x)
        {
            if (x.Type() == EVariableType.List)
            {
                List temp = new List();
                foreach (IVariable iv in (x as List).list)
                {
                    temp.Add(ReplaceSlashHelper(iv));
                }
                return temp;
            }
            else
            {
                return ReplaceSlashHelper(x);
            }
        }

        // ============ helper methods for options, start
        // See #jkafjkaddasfas                

        public static bool XBool(IVariable x)
        {
            string x_string = O.ConvertToString(x);
            if (G.Equal(x_string, "yes")) return true;
            else if (G.Equal(x_string, "no")) return false;
            else
            {
                G.Writeln2("*** ERROR: Value expected to be 'yes' or 'no', not '" + x_string + "'");
                throw new GekkoException();
            }
        }

        public static string XString(IVariable x)
        {
            string x_string = O.ConvertToString(x);
            return x_string;
        }

        public static int XInt(IVariable x)
        {
            int x_int = O.ConvertToInt(x);
            if (x_int < 0)
            {
                G.Writeln2("*** ERROR: Expected integer >= 0, not " + x_int);
            }
            return x_int;
        }

        public static int XSint(IVariable x)  //signed int
        {
            int x_int = O.ConvertToInt(x);            
            return x_int;
        }

        public static double XVal(IVariable x)
        {
            double x_val = O.ConvertToVal(x);
            return x_val;
        }

        public static string XVal2String(IVariable x)
        {
            double x_val = O.ConvertToVal(x);
            return x_val.ToString();
        }

        public static string XNameOrString(IVariable x)
        {
            string x_string = O.ConvertToString(x);
            return x_string;
        }

        public static string XNameOrStringOrFilename(IVariable x)
        { 
            return XNameOrString(x);
        }

        public static EFreq XNameOrString2Freq(IVariable x)
        {
            string x_string = O.ConvertToString(x);
            EFreq freq = G.GetFreq(x_string);
            return freq;
        }

        public static ESeriesMissing XOptionSeriesMissing(IVariable x)
        {
            string x_string = O.ConvertToString(x);
            return G.GetMissing(x_string);
        }


        // ============ helper methods for options, end

        public static string ResolvePath(string fileName2)
        {
            string rv = null;
            try
            {
                rv = Path.GetFullPath(fileName2);
            }
            catch { };
            if (rv == null)
            {
                G.Writeln2("+++ NOTE: Could not resolve the path '" + fileName2 + "'");
                rv = fileName2;
            }
            return rv;
        }

        public static void PrintOptions(string path)
        {
            Program.options.Write(path);
        }

        //isBlock = 0 (not), 1 (first time), 2 (second time)
        public static void HandleOptions(string s, int isBlock, P p)
        {
            string s2 = s.Replace("Program.options.", "");
            if (G.Equal(s2, "freq"))
            {
                //see also #89073589324
                O.AdjustFreq();
            }
            else if (isBlock == 0 && G.Equal(s2, "interface_sound_type"))
            {
                if (!p.hasBeenCmdFile)
                {
                    Program.PlaySound();
                }
            }
            else if (G.Equal(s2, "interface_edit_style"))
            {
                CrossThreadStuff.SetChecked();  //sets checkmarks regarding editor choice
            }
            else if (G.Equal(s2, "folder_menu") || G.Equal(s2, "menu_startfile"))
            {
                CrossThreadStuff.RestartMenuBrowser();
            }
            else if (G.Equal(s2, "interface_zoom"))
            {
                CrossThreadStuff.Zoom();
            }
            else if (G.Equal(s2, "folder_working"))
            {
                CrossThreadStuff.WorkingFolder("");
            }
            else if (G.Equal(s2, "interface_remote"))
            {
                Program.RemoteInit();
            }
            else if (isBlock == 0 && G.Equal(s2, "solve_gauss_reorder"))
            {
                G.Writeln();
                G.Writeln("+++ NOTE: Reorder: you must issue a MODEL statement afterwards, for this option to take effect.");
                G.Writeln("+++       (In command files, place this option before any MODEL statements).");
            }
            else if (isBlock == 0 && G.Equal(s2, "series_dyn"))
            {
                G.Writeln();
                G.Writeln("*** ERROR: Deprecated option");
                G.Writeln();
                G.Writeln("+++ NOTE: The 'dyn' option has been deprecated. Instead, you may use <dyn> on individual series");
                G.Writeln("+++       statements, or use 'BLOCK series dyn = yes; ... ; END;' to set the option for several");
                G.Writeln("+++       series statemens. See more in the help, under the BLOCK command.");
                G.Writeln();
                throw new GekkoException();
            }
            else if (isBlock == 0 && G.Equal(s2, "timefilter_type"))  //TODO: only issue if really avg
            {
                G.Writeln2("+++ NOTE: Timefilter type = 'avg' only works for PRT and MULPRT.");
            }                        
        }

        public static List ExplodeIvariablesSeqFor(bool isNaked, IVariable iv)
        {
            List m = ExplodeIvariablesSeq(isNaked, iv);
            m = Restrict2(m, true, false, true, true);  //no sigils
            return m;
        }

        public static List ExplodeIvariablesSeq(bool isNaked, IVariable iv)
        {
            List m = null;
            if (isNaked)
            {
                m = iv as List;
                if (m == null)
                {
                    G.Writeln2("*** ERROR: Naked list internal error");
                    throw new GekkoException();
                }
                bool hasLlist = false;
                for (int i = 0; i < m.list.Count; i += 2)
                {
                    if (m.list[i].Type() == EVariableType.List)
                    {
                        hasLlist = true;
                        break;
                    }
                }
                
                if (hasLlist)
                {
                    List mm = new List();
                    for (int i = 0; i < m.list.Count; i += 2)
                    {                        
                        if (m.list[i].Type() == EVariableType.List)
                        {
                            if (m.list[i + 1] != null)
                            {
                                G.Writeln2("*** ERROR: Rep not allowed for list inside naked list");
                                throw new GekkoException();
                            }
                            foreach (IVariable x in (m.list[i] as List).list)
                            {
                                mm.Add(x);
                                mm.Add(null);
                            }
                        }
                        else
                        {
                            mm.Add(m.list[i]);
                            mm.Add(m.list[i + 1]);
                        }
                    }
                    m = mm;
                }
            }
            else
            {
                m = new List(ExplodeIvariablesHelper(iv));
            }            
            
            if (isNaked)
            {
                bool allNumbers = IsListAllNumbers(m, 2);

                if (allNumbers)
                {
                    for (int i = 0; i < m.list.Count; i += 2)
                    {
                        if (m.list[i].Type() == EVariableType.Val) continue;
                        if (m.list[i].Type() == EVariableType.String)
                        {
                            double d = double.Parse((m.list[i] as ScalarString).string2);  //must parse
                            m.list[i] = new ScalarVal(d);
                        }
                    }
                }

                m = ListDefHelper2(m.list.ToArray());  //ok that it is new
                //m.isFromNakedList = true;
            }

            if (isNaked)
            {
                //check that the items have same type
                for (int i = 0; i < m.list.Count; i++)
                {
                    if (i > 0 && m.list[i - 1].Type() != m.list[i].Type())
                    {
                        G.Writeln2("*** ERROR: Naked list elements #" + ((i - 1) + 1) + " and #" + (i + 1) + " have different type");
                        G.Writeln("           Naked lists do not allow this, to avoid confusion. Please use a normal list definition.");
                        throw new GekkoException();
                    }
                    ScalarString child_string = m.list[i] as ScalarString;  //should always be so
                    if (child_string != null)
                    {
                        child_string.isFromNakedList = true;
                    }
                }
            }

            return m;

        }

        private static bool IsListAllNumbers(List m, int skip)
        {
            //if it returns true, all elements in the list should be understood as numbers
            bool allNumbers = true;
            for (int i = 0; i < m.list.Count; i += skip)
            {
                if (m.list[i].Type() != EVariableType.Val && m.list[i].Type() != EVariableType.String)
                {
                    G.Writeln2("*** ERROR: Naked lists only support val or string types");
                    throw new GekkoException();
                }
                if (m.list[i].Type() == EVariableType.Val) continue;
                if (m.list[i].Type() == EVariableType.String)
                {
                    string ss = (m.list[i] as ScalarString).string2.Trim();
                    double d; if (double.TryParse(ss, out d))
                    {

                        if (!ss.Contains("."))
                        {
                            if (G.IsInteger(ss, true, true))
                            {
                                //good
                            }
                            else
                            {
                                //stuff like 1e5
                                allNumbers = false;
                                break;
                            }
                        }

                        if (ss.StartsWith("0") && ss.Length > 1 && char.IsDigit(ss[1]))
                        {
                            //we do not allow 07 as a number here. If present, all the elements will become strings, and have to be converted to values when put into series (or remain as strings if put into list)
                            allNumbers = false;
                            break;
                        }

                        if (ss.StartsWith("-") && ss.Length > 2 && ss[1] == '0' && char.IsDigit(ss[2]))
                        {
                            //we do not allow -07 as a number here. If present, all the elements will become strings, and have to be converted to values when put into series (or remain as strings if put into list)
                            allNumbers = false;
                            break;
                        }
                        continue;
                    }
                    else
                    {
                        allNumbers = false;
                        break;
                    }
                }
                allNumbers = false;
                break;
            }

            return allNumbers;
        }

        public static ScalarVal Minus(IVariable iv)
        {
            ScalarVal x = iv as ScalarVal;
            if (x == null)
            {
                G.Writeln("*** ERROR: trying to use -x on a variable x that is not a value");
                throw new GekkoException();
            }
            return new ScalarVal(-x.val);
        }

        public static List ExplodeIvariables(IVariable iv)
        {
            return new List(ExplodeIvariablesHelper(iv));
        }
        
        //is recursive
        private static List<IVariable> ExplodeIvariablesHelper(IVariable iv)
        {            
            List<IVariable> temp = new List<IVariable>();
            if (iv.Type() == EVariableType.List)
            {
                foreach (IVariable temp2 in ((List)iv).list)
                {
                    if (temp2 != null && temp2.Type() == EVariableType.List)
                    {
                        List<IVariable> temp3 = ExplodeIvariablesHelper(temp2);
                        temp.AddRange(temp3);
                    }
                    else
                    {
                        temp.Add(temp2);
                    }
                }
            }
            else temp.Add(iv);
            return temp;
        }
        
        public static void SeriesQuestion()
        {
            foreach (Databank bank in Program.databanks.storage)
            {
                int a = 0;
                int q = 0;
                int m = 0;
                int u = 0;
                if (bank.storage.Count == 0)
                {
                    G.Writeln2("Databank " + bank.name + " is empty");
                    continue;
                }
                foreach (Series ts in bank.storage.Values)
                {
                    if (ts.freq == EFreq.A) a++;
                    else if (ts.freq == EFreq.Q) q++;
                    else if (ts.freq == EFreq.M) m++;
                    else if (ts.freq == EFreq.U) u++;
                }
                G.Writeln2("Databank " + bank.name + ":");
                if (a > 0) G.Writeln("  " + a + " annual timeseries");
                if (q > 0) G.Writeln("  " + q + " quarterly timeseries");
                if (m > 0) G.Writeln("  " + m + " monthly timeseries");
                if (u > 0) G.Writeln("  " + u + " undated timeseries");
            }
        }

        public static void PrtElementHandleLabel(GekkoSmpl smpl, O.Prt.Element ope0)
        {
            //ope0.label2 = null;
            if (ope0.labelRecordedPieces == null) ope0.labelRecordedPieces = new List<RecordedPieces>();
            ope0.labelRecordedPieces.AddRange(smpl.labelRecordedPieces);
            //Program.UnfoldLabels(ope0.label, ref ope0.label2, O.AddLabelHelper2(smpl));            
        }

        public static void GetFromAndToDatabanks(string opt_from, string opt_to, ref Databank fromBank, ref Databank toBank)
        {
            if (opt_from != null)
            {
                fromBank = Program.databanks.GetDatabank(opt_from);
                if (fromBank == null)
                {
                    G.Writeln2("*** ERROR: Databank '" + opt_from + "' could not be found");
                    throw new GekkoException();
                }
            }

            if (opt_to != null)
            {
                toBank = Program.databanks.GetDatabank(opt_to);
                if (toBank == null)
                {
                    G.Writeln2("*** ERROR: Databank '" + opt_to + "' could not be found");
                    throw new GekkoException();
                }
            }
        }

        //==============================================================
        //==============================================================
        //==============================================================

        public enum EZContext
        {
            ScalarExpression,
            Genr,
            Indexer
        }

        public enum ECreatePossibilities
        {
            NoneReturnNull,
            NoneReportError,
            Can,
            Must
        }


        public enum GetDateChoices
        {
            Strict,
            FlexibleStart,
            FlexibleEnd
        }

        //public static ScalarVal SetValData(GekkoSmpl smpl, IVariable name, IVariable rhs)
        //{
        //    //Returns the IVariable it finds here (or creates)            
        //    string name2 = name.ConvertToString();            
        //    double value = rhs.GetValOLD(smpl);
        //    IVariable lhs = null;
        //    if (Program.scalars.TryGetValue(name2, out lhs))
        //    {
        //        //Scalar is already existing                
        //        if (lhs.Type() == EVariableType.Val)
        //        {
        //            //Already existing lhs is a VAL, inject into it. Injecting is faster than recreating an object.
        //            ((ScalarVal)lhs).val = value;                    
        //        }
        //        else
        //        {                    
        //            Program.scalars.Remove(name2);
        //            lhs = new ScalarVal(value);
        //            Program.scalars.Add(name2, lhs);                    
        //        }
        //    }
        //    else
        //    {
        //        //Scalar does not exist beforehand                   
        //        lhs = new ScalarVal(value);
        //        Program.scalars.Add(name2, lhs);                
        //    }
        //    return (ScalarVal)lhs;
        //}

        public static void HandleEndoExo(GekkoTimes global, List<HandleEndoHelper> helper, bool type)
        {

            if (type)
            {
                Globals.endo = new HandleEndoHelper2();
                Globals.endo.global = global;
                Globals.endo.helper = helper;
            }
            else
            {
                Globals.exo = new HandleEndoHelper2();
                Globals.exo.global = global;
                Globals.exo.helper = helper;
            }
            SetEndoExo(type);
        }

        public static void SetEndoExo(bool type)
        {

            Databank databank = Program.databanks.GetFirst();

            string endoOrExoPrefix = "endo"; if (!type) endoOrExoPrefix = "exo";

            //Clear all endo_ or exo_ variables
            if (false)
            {
                Program.Unfix(databank, endoOrExoPrefix);
            }


            GekkoTimes global = null;
            List<HandleEndoHelper> helper = null;

            if (type)
            {
                global = Globals.endo.global;
                helper = Globals.endo.helper;
            }
            else
            {
                global = Globals.exo.global;
                helper = Globals.exo.helper;
            }

            int count = 0;
            
            List<string> simModeVariables = new List<string>();  //only for sim-mode

            foreach (HandleEndoHelper h in helper)
            {
                List<string> vars = Program.GetListOfStringsFromList(h.varname);  //emits error if not string or list

                foreach (string s in vars)
                {

                    count++; //array-series updates like x[#i, #j] will only count as 1.                       
                    if (!G.IsSimpleToken(s))
                    {
                        G.Writeln2("*** ERROR: The name '" + s + "' is not a simple series name");
                        throw new GekkoException();
                    }

                    if (!G.Equal(Program.options.model_type, "gams"))
                    {
                        simModeVariables.Add(s);
                    }
                    else
                    {

                        List<List<string>> ss = new List<List<string>>();

                        if (h.indices != null)
                        {
                            int depth = 0;
                            Stack<string> stack = new Stack<string>();
                            // [a, #i, b, #j] is unfolded/combined into many items in ss
                            Program.Combine(h.indices, ss, depth, stack);
                        }

                        string varNameWithoutFreq = endoOrExoPrefix + "_" + s;
                        string varNameWithFreq = varNameWithoutFreq + Globals.freqIndicator + G.GetFreq(Program.options.freq);

                        GekkoTimes gts = global;
                        if (h.local != null) gts = h.local;

                        if (gts == null)
                        {
                            G.Writeln2("*** ERROR: No time period given for variable '" + s + "'");
                            throw new GekkoException();
                        }

                        Series ts2 = null;

                        Series ts = databank.GetIVariable(varNameWithFreq) as Series;

                        if (ss.Count > 0)
                        {
                            foreach (List<string> ss2 in ss)
                            {

                                if (!Program.options.databank_create_auto)
                                {
                                    //The following xx is not used, just used to check existence
                                    //IVariable xx = O.GetIVariableFromString(null, s, null, ss2.ToArray(), ECreatePossibilities.NoneReportError);
                                    IVariable xx = O.GetIVariableFromString(s, O.ECreatePossibilities.NoneReportError);
                                }

                                //Multi-dim timeseries
                                //What about timeless??                        

                                if (ts == null)
                                {
                                    ts = new Series(Program.options.freq, varNameWithFreq);
                                    ts.SetArrayTimeseries(ss2.Count + 1, true);
                                    databank.AddIVariable(ts.name, ts);
                                }

                                MapMultidimItem mmi = new MapMultidimItem(ss2.ToArray(), ts);
                                IVariable iv = null; ts.dimensionsStorage.TryGetValue(mmi, out iv);
                                if (iv == null)
                                {
                                    ts2 = new Series(Program.options.freq, null);
                                    ts.dimensionsStorage.AddIVariableWithOverwrite(mmi, ts2);
                                }
                                else
                                {
                                    ts2 = iv as Series;
                                }
                                foreach (GekkoTime t in new GekkoTimeIterator(gts.t1, gts.t2))
                                {
                                    ts2.SetData(t, 1d);
                                }
                            }
                        }
                        else
                        {
                            //Normal 0-dim timeseries
                            //What about timeless??


                            //The following xx is not used, just used to check existence
                            //IVariable xx = O.GetIVariableFromString(null, s, null, null, ECreatePossibilities.NoneReportError);
                            IVariable xx = O.GetIVariableFromString(s, O.ECreatePossibilities.NoneReportError);

                            ts2 = ts;
                            if (ts2 == null)
                            {
                                ts2 = new Series(Program.options.freq, varNameWithFreq);
                                databank.AddIVariable(ts2.name, ts2);
                            }
                            foreach (GekkoTime t in new GekkoTimeIterator(gts.t1, gts.t2))
                            {
                                ts2.SetData(t, 1d);
                            }
                        }
                    }
                }                
            }

            if (!G.Equal(Program.options.model_type, "gams"))
            {
                if (type) Program.Endo(simModeVariables);
                else Program.Exo(simModeVariables);
            }
            else
            {
                G.Writeln2("Set " + count + " " + endoOrExoPrefix + "_... variables");
            }
        }

        

        public static void HandleOptionBankRef1(string opt_bank, string opt_ref)
        {
            if (opt_bank != null)
            {
                Program.databanks.optionBank = Program.databanks.GetDatabank(opt_bank, true);
            }
            if (opt_ref != null)
            {
                Program.databanks.optionRef = Program.databanks.GetDatabank(opt_ref, true);
            }
        }        

        public static void HandleMissing1(string s)
        {
            if (s != null)
            {
                ESeriesMissing missing = G.GetMissing(s);
                if (missing == ESeriesMissing.Ignore)
                {
                    Program.options.series_array_print_missing = ESeriesMissing.Skip;
                    Program.options.series_array_calc_missing = ESeriesMissing.Zero;
                    Program.options.series_data_missing = ESeriesMissing.Zero;
                }
                else
                {
                    //These options are not good, need to be reworked
                    //but kept for legacy in 3.0
                    Program.options.series_array_print_missing = missing;
                    Program.options.series_array_calc_missing = missing;
                    Program.options.series_data_missing = missing;
                }
            }
        }

        public static void HandleOptionBankRef2()
        {
            Program.databanks.optionBank = null;
            Program.databanks.optionRef = null;
        }

        public static void HandleMissing2(ESeriesMissing r1, ESeriesMissing r2, ESeriesMissing r3)
        {
            Program.options.series_array_print_missing = r1;
            Program.options.series_array_calc_missing = r2;
            Program.options.series_data_missing = r3;
        }
        
        public static void IterateStep(bool isYear, ELoopType loopType, ref IVariable x, IVariable start, IVariable step, int counter)
        {
            if (loopType == O.ELoopType.ForTo)
            {

                if (x.Type() == EVariableType.Val)
                {
                    ScalarVal step_val = null;
                    if (step == null) step_val = Globals.scalarVal1;
                    else step_val = step as ScalarVal;
                    ScalarVal x_val = x as ScalarVal;
                    x_val.val += step_val.val;
                }
                else if (x.Type() == EVariableType.Date)
                {
                    ScalarVal step_val = null;
                    if (step == null) step_val = Globals.scalarVal1;
                    else step_val = step as ScalarVal;
                    ScalarDate x_date = x as ScalarDate;
                    x_date.date = x_date.date.Add(G.ConvertToInt(step_val.val));
                }
                else
                {
                    G.Writeln2("*** ERROR: Loop type problem");
                    throw new GekkoException();
                }
            }
            else
            {

                //it is tested previously that start is list
                List start_list = start as List;
                if (counter >= start_list.list.Count)
                {
                    //do nothing, this x will not be used
                }
                else
                {
                    //ScalarString x_string = x as ScalarString;
                    //ScalarString item = start_list.list[counter] as ScalarString;
                    //if (item == null)
                    //{
                    //    G.Writeln2("*** ERROR: list element " + (counter + 1) + " is not a STRING");
                    //    throw new GekkoException();
                    //}
                    //x_string.string2 = item.string2;

                    IVariable item = start_list.list[counter];
                    x = item.DeepClone(null);  //necessary to clone?? Note sure... but safest to do
                }
            }         
        }

        public static void IterateStart(bool isYear, ELoopType loopType, ref IVariable x, IVariable start)
        {
            if (x == null)
            {
                if (loopType == ELoopType.ForTo)
                {

                    if (start.Type() == EVariableType.Val)
                    {
                        if (isYear)
                        {
                            x = new ScalarDate(new GekkoTime(EFreq.A, O.ConvertToInt(start), 1));
                        }
                        else
                        {
                            x = new ScalarVal(((ScalarVal)start).val);
                        }
                    }
                    else if (start.Type() == EVariableType.Date)
                    {
                        x = new ScalarDate(((ScalarDate)start).date);
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: FOR ... = ... TO ... loop must begin with val or date, not " + start.Type().ToString().ToLower());
                        throw new GekkoException();
                    }
                }
                else
                {

                    if (start.Type() == EVariableType.List)
                    {

                        List start_list = start as List;
                        if (start_list.list.Count == 0)
                        {
                            G.Writeln2("*** ERROR: Empty list");
                            throw new GekkoException();
                        }
                        x = start_list.list[0].DeepClone(null);//x = start_list.list[0];  ----------------> FAIL, sideeffect because then the first item in the list will change when x changes....!!!

                    }
                    else
                    {
                        G.Writeln2("*** ERROR: FOR ... = ... loop must have a list to iterate over, not a " + start.Type().ToString().ToLower());
                        throw new GekkoException();
                    }
                }
            }
        }

        public static bool LoopYears(string type, ELoopType isForToOrListType, IVariable start, IVariable end)
        {
            if (isForToOrListType == ELoopType.List) return false;  //do not do this for FOR val %i = (1, 2, 3)...
            if (!G.Equal(type, "date")) return false;  //only FOR date ...
            bool rv = false;
            //Now we know that it is of this kind: FOR date ... = ... TO ...
            EVariableType type1 = start.Type();
            EVariableType type2 = end.Type();
            if (type1 == EVariableType.Val && type2 == EVariableType.Val)
            {
                //FOR date %d = 1990 to 2000;
                int i1 = O.ConvertToInt(start, false);
                int i2 = O.ConvertToInt(end, false);
                if (i1 >= 0 && i2 >= 0) rv = true; //will be -12345 if not integer
            }
            else if (type1 == EVariableType.Val && type2 == EVariableType.Date)
            {
                int i1 = O.ConvertToInt(start, false);
                if (i1 >= 0) rv = true; //will be -12345 if not integer
            }
            else if (type1 == EVariableType.Date && type2 == EVariableType.Val)
            {
                int i2 = O.ConvertToInt(end, false);
                if (i2 >= 0) rv = true; //will be -12345 if not integer
            }
            return rv;
        }

        public static bool IterateContinue(bool isYear, ELoopType isForToOrListType, IVariable x, IVariable start, IVariable max, IVariable step, ref int counter)
        {
            counter++;
            bool rv = false;

            if (isForToOrListType == ELoopType.List)
            {
                //looping over a list like FOR <type> %i = (<item1>, item2, ...)

                List start_list = start as List;
                if (start_list == null)
                {
                    G.Writeln2("*** ERROR: Expected FOR to loop over list, not a " + G.GetTypeString(start) + " type");
                    throw new GekkoException();
                }                
                if (counter <= start_list.list.Count) rv = true;                
            }
            else
            {
                //looping from ... to ... by
                if (x.Type() == EVariableType.Val)
                {
                    ScalarVal x_val = x as ScalarVal;
                    ScalarVal max_val = max as ScalarVal;
                    if (max_val == null)
                    {
                        G.Writeln2("*** ERROR: Expected max value to be VAL type, you may try the val() function");
                        throw new GekkoException();
                    }
                    ScalarVal step_val = null;
                    if (step == null) step_val = Globals.scalarVal1;
                    else step_val = step as ScalarVal;
                    if (step_val == null)
                    {
                        G.Writeln2("*** ERROR: Expected step value to be VAL type, you may try the val() function");
                        throw new GekkoException();
                    }
                    if (step_val.val > 0)
                    {
                        //for instance: FOR VAL i = 1 to 11 by 2; (1, 3, 5, 7, 9, 11)
                        //max typically has step/1000000 added, so it might be 11.000002
                        rv = x_val.val <= max_val.val + step_val.val / 1000000d;
                    }
                    else
                    {
                        rv = x_val.val >= max_val.val + step_val.val / 1000000;
                        //for instance: FOR VAL i = 11 to 1 by -2; (11, 9, 7, 5, 3, 1)
                        //max typically has step/1000000 added, so it might be 0.999998
                    }
                }
                else if (x.Type() == EVariableType.Date)
                {
                    ScalarDate x_date = x as ScalarDate;
                    ScalarDate max_date = max as ScalarDate;
                    GekkoTime gt_max = GekkoTime.tNull;
                    if (max_date == null)
                    {
                        if (isYear)
                        {
                            int dd = O.ConvertToInt(max);
                            gt_max = new GekkoTime(EFreq.A, dd, 1);
                        }
                        else
                        {
                            G.Writeln2("*** ERROR: Expected max value to be DATE type, you may try the date() function");
                            throw new GekkoException();
                        }
                    }
                    else
                    {
                        gt_max = max_date.date;
                    }
                    ScalarVal step_val = null;
                    if (step == null) step_val = Globals.scalarVal1;
                    else step_val = step as ScalarVal;
                    if (step_val == null)
                    {
                        G.Writeln2("*** ERROR: Expected step value to be VAL type, you may try the val() function");
                        throw new GekkoException();
                    }
                    int step_int = O.ConvertToInt(step_val);
                    if (step_int == 0)
                    {
                        G.Writeln2("*** ERROR: Step value cannot be 0");
                        throw new GekkoException();
                    }
                                        
                    if (step_val.val > 0)
                    {
                        return x_date.date.SmallerThanOrEqual(gt_max);
                    }
                    else
                    {
                        return x_date.date.LargerThanOrEqual(gt_max);
                    }
                }
                else
                {
                    G.Writeln2("*** ERROR: Expected iterator in FOR ... = ... TO ... to be of val or date type");
                    throw new GekkoException();
                }       
            }
            return rv;
        }



        //public static bool ContinueIterating(int loopType, double i, double max, double step) {
        //    if (step > 0)
        //    {
        //        //for instance: FOR VAL i = 1 to 11 by 2; (1, 3, 5, 7, 9, 11)
        //        //max typically has step/1000000 added, so it might be 11.000002
        //        return i <= max;
        //    }
        //    else
        //    {
        //        return i >= max;
        //        //for instance: FOR VAL i = 11 to 1 by -2; (11, 9, 7, 5, 3, 1)
        //        //max typically has step/1000000 added, so it might be 0.999998
        //    }
        //}

        ////used in "FOR date d = ..."
        //public static bool ContinueIterating(int loopType, GekkoTime x, GekkoTime y, int step)
        //{
        //    bool rv = false;
        //    if (step > 0)
        //    {
        //        if (x.SmallerThanOrEqual(y)) rv = true;
        //    }
        //    else
        //    {
        //        if (x.LargerThanOrEqual(y)) rv = true;
        //    }
        //    return rv;
        //}

        

        public static GekkoTimes HandleDates(GekkoTime t1, GekkoTime t2)
        {
            GekkoTimes gts = new GekkoTimes();
            gts.t1 = t1;
            gts.t2 = t2;
            return gts;
        }

        public static void HandleIndexer(IVariable y, params IVariable[] x)
        {
            HandleIndexerHelper(0, y, x);
        }

        public static GekkoSmpl Smpl()
        {
            return new GekkoSmpl(Globals.globalPeriodStart, Globals.globalPeriodEnd);
        }

        public static List ListDefHelper(params IVariable[] x)
        {
            return ListDefHelper2(x);
        }

        public static List ListDefHelper2(IVariable[] x)
        {
            //Note, these come in pairs:
            //#m = (1 rep 2, 3, 4) --> 1, 2, 3, null, 4, null
            List<IVariable> m = new List<Gekko.IVariable>();

            for (int i = 0; i < x.Length; i += 2)
            {
                IVariable iv = x[i];
                if (x[i + 1] != null)
                {
                    ScalarString ss = x[i + 1] as ScalarString;
                    if (ss != null && ss.ConvertToString() == "*")
                    {
                        if (i + 2 < x.Length)
                        {
                            G.Writeln2("*** ERROR: You can only use 'REP *' on the last element in a list");
                            throw new GekkoException();
                        }
                        ScalarVal sv = iv as ScalarVal;
                        if (sv == null)
                        {
                            G.Writeln2("*** ERROR: You can only use 'REP *' toghether with values");
                            throw new GekkoException();
                        }
                        ScalarVal sv2 = new ScalarVal(sv.val);
                        sv2.hasRepStar = true;
                        m.Add(sv2);
                    }
                    else
                    {
                        int rep = O.ConvertToInt(x[i + 1], true);
                        for (int ii = 0; ii < rep; ii++)
                        {
                            m.Add(iv);
                        }
                    }
                }
                else
                {
                    m.Add(iv);
                }
                
            }
            return new List(m);
        }

        public static bool IsTimelessSeries(IVariable x)
        {
            bool rv = false;
            Series x_series = x as Series;
            if (x_series != null && x_series.type == ESeriesType.Timeless) rv = true;
            return rv;
        }

        

        // LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START
        // LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START
        // LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START
        // LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START
        // LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START
        // LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START

        //NOTE: Must have same signature as Lookup(), #89075234532
        public static void DollarLookup(IVariable logical, GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, LookupSettings isLeftSideVariable, EVariableType type, O.Assignment options)
        {
            //Only encountered on the LHS
            if (logical == null)
            {
                Lookup(smpl, map, dbName, varname, freq, rhsExpression, isLeftSideVariable, type, options);
            }
            if (logical.Type() == EVariableType.Val)
            {
                if (IsTrue(((ScalarVal)logical).val))
                {
                    Lookup(smpl, map, dbName, varname, freq, rhsExpression, isLeftSideVariable, type, options);
                }
                else
                {
                    //skip it!
                }
            }
            else if (logical.Type() == EVariableType.Series)
            {
                //This deviates a bit from GAMS: when logical is 0 here, a 0 will also be set for the LHS, it is not just skipped.
                //See also #6238454
                IVariable y = Conditional1Of3(smpl, rhsExpression, logical);
                Lookup(smpl, map, dbName, varname, freq, y, isLeftSideVariable, type, options);
            }
            else
            {
                DollarLHSError();
            }
        }

        public static IVariable Lookup(GekkoSmpl smpl, Map map, IVariable x, IVariable rhsExpression, LookupSettings isLeftSideVariable, EVariableType type, O.Assignment options)
        {
            //overload
            return Lookup(smpl, map, x, rhsExpression, isLeftSideVariable, type, true, options);
        }

        public static IVariable NameLookup(GekkoSmpl smpl, Map map, IVariable x, IVariable rhsExpression, LookupSettings isLeftSideVariable, EVariableType type, O.Assignment options)
        {
            return x;
        }

        //NOTE: Must have same signature as DollarLookup(), #89075234532
        public static IVariable Lookup(GekkoSmpl smpl, Map map, IVariable x, IVariable rhsExpression, LookupSettings settings, EVariableType type, bool errorIfNotFound, O.Assignment options)
        {
            //This calls the more general Lookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression)

            if (x.Type() == EVariableType.String)
            {

                IVariable rv = null;
                string x_string = (x as ScalarString).string2;


                string dbName, varName, freq; string[] indexes; char firstChar;
                if (x_string.StartsWith(Globals.symbolCollection + Globals.listfile))
                {
                    dbName = null; varName = x_string; freq = null; indexes = null; firstChar = varName[0];
                }
                else
                {
                    Chop(x_string, out dbName, out varName, out freq, out indexes);
                }

                LookupSettings settingsTemp = settings;
                if (indexes != null)
                {
                    settingsTemp = new LookupSettings(); //normal abort if array-super-series is not found, cannot just be created
                    settingsTemp.depth = settings.depth;  //no recursion for #alias
                }

                IVariable iv = Lookup(smpl, map, dbName, varName, freq, rhsExpression, settingsTemp, type, errorIfNotFound, options);

                if (indexes != null)
                {
                    Series iv_series = iv as Series;
                    if (iv_series == null || iv_series.type != ESeriesType.ArraySuper)
                    {
                        G.Writeln2("*** ERROR: Expected array-series variable");
                        throw new GekkoException();
                    }

                    rv = iv_series.FindArraySeries(smpl, Program.GetListOfIVariablesFromListOfStrings(indexes), false, false, settings);  //last arg. not used

                    //rv = iv.Indexer(smpl, O.EIndexerType.None, Program.GetListOfIVariablesFromListOfStrings(indexes));
                }
                else
                {
                    rv = iv;
                }

                return rv;
            }
            else if (x.Type() == EVariableType.List)
            {
                //for instance PRT {('a', 'b')}. A controlled unfold like PRT {#m} will not get here.
                List x_list = x as List;
                string[] items = Program.GetListOfStringsFromListOfIvariables(x_list.list.ToArray());
                if (items == null)
                {
                    G.Writeln2("*** ERROR: The list contains non-string elements");
                    throw new GekkoException();
                }
                else
                {
                    List<IVariable> rv = new List<IVariable>();
                    foreach (string s in items)
                    {
                        IVariable iv = GetIVariableFromString(s, ECreatePossibilities.NoneReportError, true);
                        rv.Add(iv);
                    }
                    List m = new List(rv);
                    return m;
                }
            }
            else
            {
                G.Writeln2("*** ERROR: Expected variable name to be a string, but it is of " + G.GetTypeString(x) + " type");
                throw new GekkoException();
            }
            return x;
        }

        public static IVariable Lookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, LookupSettings isLeftSideVariable, EVariableType type, O.Assignment options)
        {
            //overload
            return Lookup(smpl, map, dbName, varname, freq, rhsExpression, isLeftSideVariable, type, true, options);
        }

        public static IVariable NameLookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, LookupSettings isLeftSideVariable, EVariableType type, O.Assignment options)
        {
            if (dbName != null || freq != null)
            {
                G.Writeln2("*** ERROR: Expected a simple variable name without bank or frequency");
                throw new GekkoException();
            }

            return new ScalarString(varname);
        }

        //Also see #8093275432098
        public static IVariable Lookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, LookupSettings settings, EVariableType type, bool errorIfNotFound, O.Assignment options)
        {
            // =============================================================================================
            // =============================================================================================
            // =============================================================================================
            // =============================================================================================
            // ==================== THIS IS THE CENTRAL LOOKUP METHOD ======================================
            // ====================== everything passes through here =======================================
            // =============================================================================================
            // =============================================================================================
            // =============================================================================================

            //map != null:             the variable is found in the MAP, otherwise, the variable is found in a databank
            //rhsExpression != null:   it is an assignment of the left-hand side

            //only adds freq if not there. No sigil is added for lhs vars here.
            string varnameWithFreq = G.AddFreq(varname, freq, type, settings.type);
                                    
            if (Program.options.interface_alias)
            {
                bool foundAlias = true;

                if (Program.alias == null)
                {
                    IVariable alias2 = Program.databanks.GetGlobal().GetIVariable("#alias");

                    if (alias2 == null)
                    {
                        foundAlias = false;
                    }
                    else
                    {

                        if (alias2 == null || alias2.Type() != EVariableType.List)
                        {
                            G.Writeln2("*** ERROR: No global:#alias list was found, even though");
                            G.Writeln("           OPTION interface alias = yes.", Color.Red);
                            throw new GekkoException();
                        }

                        GekkoDictionary<string, string> alias3 = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        List<IVariable> alias_list = (alias2 as List).list;
                        foreach (IVariable iv in alias_list)
                        {
                            if (iv.Type() != EVariableType.List)
                            {
                                G.Writeln2("*** ERROR: global:#alias must be a list of lists");
                                throw new GekkoException();
                            }
                            List<IVariable> element_list = (iv as List).list;
                            if (element_list.Count != 2)
                            {
                                G.Writeln2("*** ERROR: the elements of global:#alias must contain two strings");
                                throw new GekkoException();
                            }
                            string s1 = G.Chop_FreqAdd(O.ConvertToString(element_list[0]), Program.options.freq);
                            string s2 = G.Chop_FreqAdd(O.ConvertToString(element_list[1]), Program.options.freq);
                            
                            if (alias3.ContainsKey(s1))
                            {
                                G.Writeln2("*** ERROR: the string " + s1 + " appears several times in global:#alias");
                                throw new GekkoException();
                            }
                            alias3.Add(s1, s2);
                        }
                        Program.alias = alias3;  //we wait until global:#alias has finished looping, so that only if global:#alias is ok, will Program.alias be != null
                    }
                }

                if (foundAlias)
                {
                    if (settings.depth == 0)
                    {
                        //use the dict
                        string var2 = null; Program.alias.TryGetValue(varnameWithFreq, out var2);
                        if (var2 != null)
                        {
                            varnameWithFreq = var2;
                            //varname = G.Chop_RemoveFreq(varnameWithFreq);

                            settings.depth++;  //will be 1
                            return O.Lookup(smpl, map, new ScalarString(varnameWithFreq), rhsExpression, settings, type, options);

                        }
                    }
                    else
                    {

                    }
                }
                
            }

            if (settings.type == ELookupType.LeftHandSide)
            {
                //ASSIGNMENT OF LEFT-HAND SIDE
                //ASSIGNMENT OF LEFT-HAND SIDE
                //ASSIGNMENT OF LEFT-HAND SIDE
                //ASSIGNMENT OF LEFT-HAND SIDE
                //ASSIGNMENT OF LEFT-HAND SIDE

                IBank ib = null;
                if (map != null)
                {
                    ib = map;
                }
                else
                {
                    Databank db = null;

                    if (IsAllSpecialDatabank(dbName))
                    {
                        G.Writeln("*** ERROR: all:x = ... is not supported, use first:x = ... instead to circumvent LOCAL/GLOBAL<all>.");
                        throw new GekkoException();
                    }
                    else
                    {

                        if (dbName == null)
                        {
                            LocalGlobal.ELocalGlobalType lg = Program.databanks.localGlobal.GetValue(varname);  //varname is always without freq 
                            db = HandleLocalGlobalBank(lg);
                        }
                        else
                        {
                            db = Program.databanks.GetDatabank(dbName, true);
                        }
                    }
                    ib = db;
                }

                if (rhsExpression != null)
                {
                    //direct assignment, like x = 5, or %s = 'a'
                    //in these cases, the LHS can be created if it is not already existing
                    //ScalarString ss = rhsExpression as ScalarString;                    
                    LookupHelperLeftside(smpl, ib, varnameWithFreq, freq, rhsExpression, type, options);
                    return null;
                }
                else
                {
                    //indexers on lhs, for instance x['a'] = ... or x[2000] = 5 or #x.%s = ...
                    //in this case, the x variable must exist
                    //NOTE: no databank search is allowed!
                    //NOTE: sigils cannot be omitted here. VAL x['v'] = 100 or VAL x.v = 100 will not access a %v variable.

                    //Not necessary, only used in assign and assign has no <bank=..>
                    //if (ib.BankType() == EBankType.Normal)
                    //{
                    //    LocalGlobal.ELocalGlobalType lg = Program.databanks.localGlobal.GetValue(varname);  //varname is always without freq
                    //    if (lg != LocalGlobal.ELocalGlobalType.None)
                    //    {
                    //    }
                    //    else if (Program.databanks.optionBank != null)
                    //    {
                    //        ib = Program.databanks.optionBank;
                    //    }
                    //}

                    IVariable ivar2 = ib.GetIVariable(varnameWithFreq);
                    if (ivar2 == null)
                    {
                        G.Writeln2("*** ERROR: Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq, true) + " for use in dot- or []-indexing");
                        throw new GekkoException();
                    }
                    else
                    {
                        return ivar2;
                    }
                }
            }
            else
            {
                //SIMPLE LOOKUP ON RIGHT-HAND SIDE
                //SIMPLE LOOKUP ON RIGHT-HAND SIDE
                //SIMPLE LOOKUP ON RIGHT-HAND SIDE
                //SIMPLE LOOKUP ON RIGHT-HAND SIDE
                //SIMPLE LOOKUP ON RIGHT-HAND SIDE

                //NOTE: databank search may be allowed!
                return LookupHelperRightside(smpl, map, dbName, varnameWithFreq, varname, settings);
            }
        }

        //Also see #8093275432098
        public static string NameLookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, LookupSettings isLeftSideVariable, EVariableType type, bool errorIfNotFound, O.Assignment options)
        {
            return varname;
        }

        // LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END 
        // LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END 
        // LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END 
        // LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END 
        // LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END 
        // LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END 


        //private static IVariable LookupHelperRightside(GekkoSmpl smpl, Map map, string dbName, string varnameWithFreq, string varname, LookupSettings settings)
        //{
        //    return LookupHelperRightside(smpl, map, dbName, varnameWithFreq, varname, settings);
        //}

        private static IVariable LookupHelperRightside(GekkoSmpl smpl, Map map, string dbName, string varnameWithFreq, string varname, LookupSettings settings)
        {
            //varname is used for local/global stuff, faster than chopping up varnameWithFreq up now
            //Can either look up stuff in a Map, or in a databank

            bool isAllSpecialDatabank = IsAllSpecialDatabank(dbName); // all:x should be understood as just x, circumventing any local<all> or global<all>

            bool errorIfNotFound = settings.create == ECreatePossibilities.NoneReportError;  //else it will return null

            IVariable rv = null;
            string frombank = null;

            if (Program.CheckIfLooksLikeWildcard2(dbName) || Program.CheckIfLooksLikeWildcard(varnameWithFreq))
            {
                //a pattern like {'a*'} or rather {'a*!a'} is caught here

                if (dbName != null)
                {
                    varnameWithFreq = G.Chop_AddBank(varnameWithFreq, dbName);
                }

                List<string> names = Program.Search(new List(new List<string>() { varnameWithFreq }), frombank, EVariableType.Var);

                if (Globals.fixWildcardLabel && smpl != null)
                {
                    smpl.labelRecordedPieces = new List<RecordedPieces>();
                    for (int i = 0; i < names.Count; i++)
                    {
                        RecordedPieces r = new RecordedPieces("wildcard", new ScalarString(names[i]));
                        smpl.labelRecordedPieces.Add(r);
                    }
                }

                List<IVariable> names2 = new List<IVariable>();
                foreach (string name in names)
                {
                    names2.Add(O.GetIVariableFromString(name, ECreatePossibilities.NoneReportError));
                }

                rv = new List(names2);

                //if (isLeftSideVariable != ELookupType.RightHandSideLoneVariable)  //We allow #m = {'x*a'} to return a list of strings, not a list of series. This is because it is similar to #m = a, b, c; --> envision that {'x*a'} returns more than 1 element. So this is the only case where #m = ... can work like seqOfBankVarNames even if it is only 1 element.
                //{
                //    rv = Program.GetListOfIVariablesFromListOfScalarStrings(rv);  //transforms til List<IVariable>
                //}
            }
            else
            {
                if (map == null)
                {
                    //It must be a databank then
                    if (dbName == null || isAllSpecialDatabank)
                    {
                        //No explicit databank name is provided, or an all:x

                        if (varname.StartsWith(Globals.symbolCollection + Globals.listfile + "___"))
                        {
                            //special case: #(listfile m)

                            List ml = ReadListFile(varname);
                            return ml;
                        }
                        else
                        {

                            //databank name not given, for instance "PRT x", or it is given as "PRT all:x"
                            //Searching only if:
                            //  (1) OPTION databank search = yes
                            //  (2) settings.canSearch = true (deactivated for commands like DOC, TRUNCATE and others)
                            //  (3) the name is not found in Local or Global databanks
                            //  (4) it is on the right-hand side (which is the case here)
                            //  With Program.options.databank_search == false, it will skip banks opened with OPEN (but search local/global)

                            if (smpl != null && smpl.bankNumber == 1 && !G.StartsWithSigil(varnameWithFreq))
                            {
                                //Ref lookup, also overrules local/global
                                Databank db = null;
                                db = Program.databanks.GetRef();
                                rv = LookupHelperFindVariableInSpecificBank(varnameWithFreq, settings, db);
                            }
                            else
                            {
                                //non-Ref lookup         

                                LocalGlobal.ELocalGlobalType lg = Program.databanks.localGlobal.GetValue(varname);  //varname is always without freq

                                if (lg != LocalGlobal.ELocalGlobalType.None && !isAllSpecialDatabank)
                                {
                                    //the variable x has been stated with LOCAL or GLOBAL keyword, and all:x is not used
                                    //Really should be handled under lookup in specific bank, since the logic
                                    //is just as if local:x had been stated, and not just x.

                                    Databank db = HandleLocalGlobalBank(lg);
                                    rv = LookupHelperFindVariableInSpecificBank(varnameWithFreq, settings, db);
                                    if (rv == null)
                                    {
                                        //this error message is perhaps already done in LookupHelperFindVariableInSpecificBank(), but for safety it is here, too
                                        G.Writeln2("*** ERROR: Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in databank '" + db.name + "'");
                                        throw new GekkoException();
                                    }
                                }
                                else
                                {

                                    bool canSearch = Program.options.databank_search && settings.canSearch;
                                    rv = Program.databanks.GetVariableWithSearch(varnameWithFreq, canSearch);

                                    if (rv == null)
                                    {
                                        if (settings.create == ECreatePossibilities.NoneReportError)
                                        {
                                            //     Local
                                            //  0. Work
                                            //  1. Ref
                                            //  2. OPEN1
                                            //  3. OPEN2
                                            //     Global
                                            //
                                            string s = null;
                                            string ss = null;
                                            string sss = null;
                                            if (!canSearch)
                                            {
                                                if (Program.databanks.GetLocal().storage.Count > 0 || Program.databanks.GetGlobal().storage.Count > 0) sss = " (or Local/Global)";
                                                ss = "the first-position" + sss + " databank";
                                            }
                                            else
                                            {
                                                ss = "any open databank";
                                                if (Program.databanks.GetRef().storage.Count() > 0) s = " (excluding Ref)";
                                            }
                                            G.Writeln2("*** ERROR: Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in " + ss + s);
                                            throw new GekkoException();
                                        }
                                        else if (settings.create == ECreatePossibilities.Can || settings.create == ECreatePossibilities.Must)
                                        {
                                            ////This should actually not be possible, since calling with noSearch=false and .Can or .Must is caught as an error earlier on in GetIVariableFromString()
                                            ////This is a variable x without bankcolon, and with autosearch true. In that case, we refuse to create it.
                                            //G.Writeln2("*** ERROR: Internal error #98253298");
                                            //throw new GekkoException();
                                            //it is probably ok to create it here like this                                            
                                            //never mind...
                                            rv = new Series(G.GetFreq(G.Chop_GetFreq(varnameWithFreq)), varnameWithFreq);  //brand new
                                            Program.databanks.GetFirst().AddIVariableWithOverwrite(rv);
                                        }
                                        else
                                        {
                                            //just return the null
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //We have an explicit databank given, like "PRT bank1:x"
                        //If we have bankNumber = 1 (Ref bank, used for PRT), we put in the Ref bank instead
                        //In that way, "MULPRT x" and "MULPRT work:x" will give the same (as it should).
                        if (smpl != null && smpl.bankNumber == 1 && !G.StartsWithSigil(varnameWithFreq))
                        {
                            //only for series type
                            rv = LookupHelperFindVariableInSpecificBank(varnameWithFreq, settings, Program.databanks.GetRef());
                        }
                        else
                        {
                            //databank name is given explicitly, and we are not doing bankNumber stuff
                            Databank db = Program.databanks.GetDatabank(dbName, true); //we know that dbName is not null
                            rv = LookupHelperFindVariableInSpecificBank(varnameWithFreq, settings, db);
                        }
                    }
                }
                else
                {
                    //We use the IBank interface here
                    rv = LookupHelperRightside2(map, dbName, varnameWithFreq);
                    if (rv == null)
                    {
                        G.Writeln2("*** ERROR: Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in map collection");
                        throw new GekkoException();
                    }
                }
            }
            return rv;
        }

        private static bool IsAllSpecialDatabank(string dbName)
        {
            return G.Equal(dbName, Globals.All);
        }

        public static void Cls(string tab)
        {
            CrossThreadStuff.Cls(tab);
        }

        public static void AdjustFreq()
        {
            //hash #980432

            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            Tuple<GekkoTime, GekkoTime> freqs = Program.ConvertFreqs(Globals.globalPeriodStart, Globals.globalPeriodEnd, Program.options.freq);

            if (Program.options.freq == EFreq.A)
            {
                G.Writeln("Freq changed to annual (A)");
                Globals.globalPeriodStart = freqs.Item1;
                Globals.globalPeriodEnd = freqs.Item2;
            }
            else if (Program.options.freq == EFreq.Q)
            {
                G.Writeln("Freq changed to quarterly (Q) -- note that start/end quarters have been translated from " + Globals.globalPeriodStart.freq.ToString() + " freq");
                Globals.globalPeriodStart = freqs.Item1;
                Globals.globalPeriodEnd = freqs.Item2;
            }
            else if (Program.options.freq == EFreq.M)
            {
                G.Writeln("Freq changed to monthly (M) -- note that start/end months have been translated from " + Globals.globalPeriodStart.freq.ToString() + " freq");
                Globals.globalPeriodStart = freqs.Item1;
                Globals.globalPeriodEnd = freqs.Item2;
            }
            else if (Program.options.freq == EFreq.D)
            {
                G.Writeln("Freq changed to daily (D) -- note that start/end months have been translated from " + Globals.globalPeriodStart.freq.ToString() + " freq");
                Globals.globalPeriodStart = freqs.Item1;
                Globals.globalPeriodEnd = freqs.Item2;
            }
            else if (Program.options.freq == EFreq.U)
            {

                G.Writeln("Frequency changed to undated (U)");
                Globals.globalPeriodStart = new GekkoTime(EFreq.U, Globals.globalPeriodStart.super, 1);
                Globals.globalPeriodEnd = new GekkoTime(EFreq.U, Globals.globalPeriodEnd.super, 1);
            }

        }

        public static bool Help(string s)
        {

            if (s == null)
            {
                s = Globals.helpStartPage;
            }
            string s2 = s;
            if (!s2.EndsWith(".htm", StringComparison.OrdinalIgnoreCase)) s2 += ".htm";  //called from command line
            List<string> folders = new List<string>();
            if (Program.options.interface_help_copylocal) folders.Add(Globals.localTempFilesLocation + "\\"); //try here first, the file is copied from the path below (helpful if StartupPath is on a network drive)
            folders.Add(Program.options.folder_help);  //looks here first, will actually before anything else look in working folder (which should not contain any help files)
            folders.Add(Application.StartupPath + "\\helpfiles\\"); //most often and probably best, the helpfiles are found here, tied to the gekko version

            string path = Program.FindFile("gekko.chm", folders);  //calls CreateFullPathAndFileName()

            if (path == null)
            {
                G.Writeln();
                G.Writeln("Sorry: could not find the help system file ('gekko.chm').");
                return false;
            }

            try
            {
                System.Windows.Forms.Help.ShowHelp(null, path, s2);  //seems to give the same                
            }
            catch (Exception e)
            {
                G.Writeln2("*** ERROR: It seems the help system is blocked -- maybe it is opened in another program?");
                G.Writeln("           file: " + path);
                throw new GekkoException();
            }
            return true;
        }


        public static void Cut()
        {
            Program.Cut(true);
        }

        public static void Tell(string text, bool nocr)
        {
            if (Globals.runningOnTTComputer && text == "arrow")
            {
                Arrow.Run();
            }

            if (nocr) G.Write(text);
            else G.Writeln(text);
        }

        public static void Exit()
        {
            Globals.applicationIsInProcessOfAborting = true;
            Globals.threadIsInProcessOfAborting = true;
            throw new GekkoException();
        }

        public static void Hdg(string text)
        {
            if (text.EndsWith(";")) text = text.Substring(0, text.Length - 1);  //Should be HDG 'text'; fixing it here
            Program.databanks.GetFirst().info1 = text;
            G.Writeln2("Databank heading for '" + Program.databanks.GetFirst().name + "' databank set to: '" + text + "'");
        }

        public static void Mem(string tpe)
        {
            //call with null, string, date, val --> will be lower-case when called

            bool foundSomething = false;

            List<Databank> banks = new List<Databank>();
            banks.Add(Program.databanks.GetLocal());
            banks.AddRange(Program.databanks.storage);
            banks.Add(Program.databanks.GetGlobal());

            if (tpe == null || tpe == "val" || tpe == "date" || tpe == "string")
            {   //scalars

                foreach (Databank db in banks)
                {
                    int counter = 0;

                    List<string> keys = new List<string>();

                    foreach (KeyValuePair<string, IVariable> kvp in db.storage)
                    {
                        if (kvp.Value.Type() == EVariableType.Val || kvp.Value.Type() == EVariableType.Date || kvp.Value.Type() == EVariableType.String)
                        {
                            if (tpe == null)
                            {
                                keys.Add(kvp.Key);
                            }
                            else
                            {
                                if (tpe == "val" && kvp.Value.Type() == EVariableType.Val) keys.Add(kvp.Key);
                                else if (tpe == "date" && kvp.Value.Type() == EVariableType.Date) keys.Add(kvp.Key);
                                else if (tpe == "string" && kvp.Value.Type() == EVariableType.String) keys.Add(kvp.Key);
                            }
                        }
                    }

                    if (keys.Count() == 0) continue;

                    foundSomething = true;

                    keys.Sort(StringComparer.OrdinalIgnoreCase);

                    Gekko.Table tab = new Gekko.Table();
                    int row = 1;
                    tab.SetBorder(row, 1, row, 3, BorderType.Top);
                    tab.Set(row, 1, "type      ");
                    tab.Set(row, 2, "name    ");  //blanks to get some spacing
                    tab.Set(row, 3, "value    ");
                    tab.SetBorder(row, 1, row, 3, BorderType.Bottom);
                    row++;
                    foreach (string s in keys)
                    {
                        IVariable a = db.storage[s];
                        string value = "";
                        if (a.Type() == EVariableType.Date)
                        {
                            if (tpe != null && tpe != "date") continue;
                            value = G.FromDateToString(a.ConvertToDate(O.GetDateChoices.Strict));
                        }
                        else if (a.Type() == EVariableType.String)
                        {
                            if (tpe != null && tpe != "string") continue;
                            value = "'" + a.ConvertToString() + "'";
                        }
                        else if (a.Type() == EVariableType.Val)
                        {
                            if (tpe != null && tpe != "val") continue;
                            value = a.ConvertToVal().ToString();
                            if (value == "NaN") value = "M";
                        }

                        string type = a.Type().ToString().ToUpper();

                        tab.Set(row, 1, type);
                        tab.Set(row, 2, s);
                        tab.Set(row, 3, value);
                        row++;
                        counter++;
                    }
                    tab.SetBorder(row - 1, 1, row - 1, 3, BorderType.Bottom);

                    string tpe2 = "";
                    if (tpe != null) tpe2 = " " + tpe.ToUpper();
                    G.Writeln2(db.name + " databank: " + counter + tpe2 + " scalar(s) found");
                    foreach (string s in tab.Print()) G.Writeln(s);
                }
                if (!foundSomething)
                {
                    if (tpe == null) G.Writeln2("No scalars found in any open databank");
                    else G.Writeln2("No " + tpe.ToUpper() + " scalar(s) found in any open databank");
                }
            }
            else if (tpe == "ser" || tpe == "series")
            {
                Gekko.Table tab = new Gekko.Table();
                int row = 1;
                tab.SetBorder(row, 1, row, 2, BorderType.Top);

                bool hit = false;

                foreach (Databank db in banks)
                {
                    string s = null;
                    Dictionary<EFreq, long> count = new Dictionary<EFreq, long>();
                    foreach (KeyValuePair<string, IVariable> kvp in db.storage)
                    {
                        if (kvp.Value.Type() != EVariableType.Series) continue;
                        Series ts = kvp.Value as Series;
                        if (!count.ContainsKey(ts.freq)) count.Add(ts.freq, 0);
                        count[ts.freq]++; hit = true;
                    }
                    foreach (KeyValuePair<EFreq, long> kvp in count)
                    {
                        s += kvp.Value + " (" + G.GetFreqString(kvp.Key) + "), ";
                    }
                    if (s != null)
                    {
                        s = s.Substring(0, s.Length - 2);
                        tab.Set(row, 1, db.name);
                        tab.Set(row, 2, s);
                        row++;
                    }
                }
                tab.SetBorder(row - 1, 1, row - 1, 2, BorderType.Bottom);

                if (hit)
                {
                    G.Writeln();
                    foreach (string s in tab.Print()) G.Writeln(s);
                }
                else
                {
                    G.Writeln2("No series found in any open databank");
                }
            }
            else  //list, map, matrix
            {
                foreach (Databank db in banks)
                {
                    int counter = 0;

                    List<string> keys = new List<string>();

                    foreach (KeyValuePair<string, IVariable> kvp in db.storage)
                    {
                        if (tpe == "list" && kvp.Value.Type() == EVariableType.List) keys.Add(kvp.Key);
                        else if (tpe == "map" && kvp.Value.Type() == EVariableType.Map) keys.Add(kvp.Key);
                        else if (tpe == "matrix" && kvp.Value.Type() == EVariableType.Matrix) keys.Add(kvp.Key);
                    }

                    if (keys.Count() == 0) continue;

                    foundSomething = true;

                    keys.Sort(StringComparer.OrdinalIgnoreCase);

                    Gekko.Table tab = new Gekko.Table();
                    int row = 1;
                    tab.SetBorder(row, 1, row, 4, BorderType.Top);
                    tab.Set(row, 1, "type    ");
                    tab.Set(row, 2, "name    ");
                    tab.Set(row, 3, "size    ");  //blanks to get some spacing                    
                    tab.SetBorder(row, 1, row, 4, BorderType.Bottom);
                    row++;
                    foreach (string s in keys)
                    {
                        IVariable a = db.storage[s];
                        string value = "";
                        string modelList = "";
                        if (a.Type() == EVariableType.List)
                        {
                            List list = a as List;
                            value = list.list.Count().ToString();
                            if (G.Equal(db.name, "Global"))
                            {
                                if (G.Equal(s, "#all") || G.Equal(s, "#endo") || G.Equal(s, "#exo") || G.Equal(s, "#exod") || G.Equal(s, "#exodjz") || G.Equal(s, "#exoj") || G.Equal(s, "#exotrue") || G.Equal(s, "#exoz"))
                                {
                                    modelList = "(model list)";
                                }
                            }
                        }
                        else if (a.Type() == EVariableType.Map)
                        {
                            Map map = a as Map;
                            value = map.storage.Count().ToString();
                        }
                        else if (a.Type() == EVariableType.Matrix)
                        {
                            Matrix matrix = a as Matrix;
                            value = matrix.DimensionsAsString();
                        }

                        string type = a.Type().ToString().ToUpper();

                        tab.Set(row, 1, type);
                        tab.Set(row, 2, s);
                        tab.Set(row, 3, value);
                        tab.Set(row, 4, modelList);
                        row++;
                        counter++;
                    }
                    tab.SetBorder(row - 1, 1, row - 1, 4, BorderType.Bottom);

                    G.Writeln2(db.name + " databank: " + counter + " " + tpe.ToUpper() + "s found");
                    foreach (string s in tab.Print()) G.Writeln(s);
                }
                if (!foundSomething)
                {
                    G.Writeln2("No " + tpe.ToUpper() + " variables found in any open databank");
                }
            }
        }


        public static void Sign()
        {
            StringBuilder sb = new StringBuilder();
            if (!G.HasModelGekko())
            {
                G.Writeln2("*** ERROR: It seems no model is defined. See MODEL command.");
                throw new GekkoException();
            }
            if (Program.model.modelGekko.signatureStatus == ESignatureStatus.SignatureNotFoundInModelFile)
            {
                sb.AppendLine();
                sb.AppendLine("You may add a signature to the model file by means of");
                sb.AppendLine("the following line somewhere in the beginning of the model file:");
                sb.AppendLine();
                sb.AppendLine("  // Signature: " + Program.model.modelGekko.modelHashTrue);
                sb.AppendLine();
                sb.AppendLine("NOTE: You may use '()' instead of '//'.");
            }
            if (Program.model.modelGekko.signatureStatus == ESignatureStatus.SignaturesDoNotMatch)
            {
                sb.AppendLine();
                sb.AppendLine("You may (a) revert the model equations back to their original state,");
                sb.AppendLine("or (b) insert the true hash code as a new signature in the model file.");
            }
            if (true)
            {
                sb.AppendLine();
                sb.AppendLine("The signature is a so-called MD5 hash code, that is, a string of");
                sb.AppendLine("characters representing the whole model file. The hash code can be");
                sb.AppendLine("thought of as a check-sum or fingerprint.");
                sb.AppendLine();
                sb.AppendLine("When computing the hash code, Gekko ignores any empty lines, or");
                sb.AppendLine("lines starting with the comment symbol ('//' or '()'). So you");
                sb.AppendLine("may add or remove (whole-line) commentaries as you like, without ");
                sb.AppendLine("altering the hash code, but changing or reordering the equations");
                sb.AppendLine("in any way will result in a new hash code.");
                sb.AppendLine();
                sb.AppendLine("Any variable list after the VARLIST$ or VARLIST; tag will also be ignored");
                sb.AppendLine("when computing the hash code.");
            }
            Program.LinkContainer lc = new Program.LinkContainer(sb.ToString());
            Globals.linkContainer.Add(lc.counter, lc);

            G.Writeln();
            string s = Program.model.modelGekko.signatureFoundInFileHeader;
            if (Program.model.modelGekko.signatureStatus == ESignatureStatus.SignatureNotFoundInModelFile)
            {
                s = "[not found]";
                G.Write("No signature was found in model file");
            }
            else if (Program.model.modelGekko.signatureStatus == ESignatureStatus.Ok)
            {
                G.Write("The signature matches the true hash code of the model file");
            }
            else if (Program.model.modelGekko.signatureStatus == ESignatureStatus.SignaturesDoNotMatch)
            {
                G.Write("The signature does not match the true hash code of the model file");
            }
            G.Write(" ("); G.WriteLink("more", "outputtab:" + lc.counter); G.Write(")"); G.Writeln();
            G.Writeln("- Signature in model file      : " + s);
            G.Writeln("- True model file hash code    : " + Program.model.modelGekko.modelHashTrue);
        }



        private static List ReadListFile(string varname)
        {
            string fileName = varname.Substring((Globals.symbolCollection + Globals.listfile + "___").Length);
            fileName = Program.AddExtension(fileName, "." + "lst");
            List<string> folders = new List<string>();
            string fileNameTemp = Program.FindFile(fileName, folders);
            if (fileNameTemp == null)
            {
                G.Writeln2("*** ERROR: Listfile " + fileName + " could not be found");
                throw new GekkoException();
            }            

            List ml = GetRawListElements(fileName);
            
            return ml;
        }

        public static void SetChecked()
        {
            Gui.gui.gekkoToolStripMenuItem.Checked = false;
            Gui.gui.gekko2ToolStripMenuItem.Checked = false;
            Gui.gui.rSToolStripMenuItem.Checked = false;
            Gui.gui.rS2ToolStripMenuItem.Checked = false;
            string s = Program.options.interface_edit_style;
            if (G.Equal(s, "gekko"))
            {
                Gui.gui.gekkoToolStripMenuItem.Checked = true;
            }
            else if (G.Equal(s, "gekko2"))
            {
                Gui.gui.gekko2ToolStripMenuItem.Checked = true;
            }
            else if (G.Equal(s, "rstudio"))
            {
                Gui.gui.rSToolStripMenuItem.Checked = true;
            }
            else if (G.Equal(s, "rstudio2"))
            {
                Gui.gui.rS2ToolStripMenuItem.Checked = true;
            }
            else
            {
                G.Writeln2("*** ERROR: type should be 'gekko', 'gekko2', 'rstudio', or 'rstudio2'");
                throw new GekkoException();
            }
        }



        private static IVariable LookupHelperFindVariableInSpecificBank(string varnameWithFreq, LookupSettings settings, Databank db)
        {
            bool create = false;
            IVariable rv = db.GetIVariable(varnameWithFreq);
            if (rv == null)
            {
                if (settings.create == ECreatePossibilities.NoneReturnNull) return rv;
                if (settings.create == ECreatePossibilities.NoneReportError)
                {
                    G.Writeln2("*** ERROR: Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in databank '" + db.name + "'");
                    throw new GekkoException();
                }
                else if (settings.create == ECreatePossibilities.Must || settings.create == ECreatePossibilities.Can)
                {
                    if (G.Chop_HasSigil(varnameWithFreq))
                    {
                        G.Writeln2("*** ERROR: Internal error #982437532");
                        throw new GekkoException();
                    }
                    else
                    {
                        //series
                        create = true;
                    }
                }
            }
            else
            {
                if (settings.create == ECreatePossibilities.Must) create = true;
            }
            if (create)
            {
                rv = new Series(G.GetFreq(G.Chop_GetFreq(varnameWithFreq)), varnameWithFreq);  //brand new
                db.AddIVariableWithOverwrite(rv);
            }
            return rv;
        }

        public static List NOTUSED_HandleWildcards(string varnameWithFreq, string frombank)
        {
            List<ToFrom> matches = Program.SearchFromTo(new List(new List<string>() { varnameWithFreq }), null, frombank, null, EWildcardSearchType.Search, null);
            List rv = new List();
            foreach (ToFrom two in matches)
            {
                ScalarString ss = new ScalarString(two.s1);
                rv.Add(ss);
            }
            return rv;

        }


        private static Databank HandleLocalGlobalBank(LocalGlobal.ELocalGlobalType lg)
        {
            Databank db;
            if (lg == LocalGlobal.ELocalGlobalType.None)
            {
                db = Program.databanks.GetFirst();
            }
            else if (lg == LocalGlobal.ELocalGlobalType.Local)
            {
                db = Program.databanks.local;
            }
            else if (lg == LocalGlobal.ELocalGlobalType.Global)
            {
                db = Program.databanks.global;
            }
            else
            {
                G.Writeln2("*** ERROR: #8097432857");
                throw new GekkoException();
            }

            return db;
        }

        public static string HandleLocalGlobalBank2(LocalGlobal.ELocalGlobalType lg)
        {
            string db = null;
            if (lg == LocalGlobal.ELocalGlobalType.None)
            {
                db = Program.databanks.GetFirst().name;
            }
            else if (lg == LocalGlobal.ELocalGlobalType.Local)
            {
                db = Program.databanks.local.name;
            }
            else if (lg == LocalGlobal.ELocalGlobalType.Global)
            {
                db = Program.databanks.global.name;
            }
            else
            {
                G.Writeln2("*** ERROR: #8097432857");
                throw new GekkoException();
            }
            return db;
        }

        public static IVariable RemoveIVariableFromString(string fullname, bool reportError)
        {
            string dbName, varName, freq; string[] indexes;
            O.Chop(fullname, out dbName, out varName, out freq, out indexes);
            IVariable iv = O.RemoveIVariableFromString(dbName, varName, freq, indexes, reportError);
            return iv;
        }

        public static void AddIVariableWithOverwriteFromString(string fullname, IVariable iv)
        {
            string dbName, varName, freq; string[] indexes;
            O.Chop(fullname, out dbName, out varName, out freq, out indexes);
            AddIVariableWithOverwriteFromString(dbName, varName, freq, indexes, iv);
        }

        public static void AddIVariableWithOverwriteFromString(string dbName, string varName, string freq, string[] indexes, IVariable iv)
        {
            string nameWithFreq = G.AddFreqToName(varName, freq);

            Databank bank = null;
            if (dbName == null) bank = Program.databanks.GetFirst();
            else bank = Program.databanks.GetDatabank(dbName, true);

            if (!bank.editable)
            {
                Program.ProtectError("You cannot add a variable to a non-editable databank, see OPEN<edit> or UNLOCK");
            }

            if (G.Chop_HasSigil(nameWithFreq))
            {
                //%x or #x
                if (indexes != null)
                {
                    G.Writeln2("*** ERROR: Name like " + nameWithFreq + "[" + G.GetListWithCommas(indexes) + "]" + " not allowed");
                    throw new GekkoException();
                }
                else
                {
                    //just add it                    
                    bank.AddIVariableWithOverwrite(nameWithFreq, iv);
                }
            }
            else
            {
                //series name, not starting with % or #

                if (indexes != null)
                {
                    //array-series

                    MapMultidimItem mmi = new MapMultidimItem(indexes);

                    //now we know that the series exists

                    Series iv_series = iv as Series;

                    if (iv_series.type == ESeriesType.ArraySuper)
                    {
                        G.Writeln2("*** ERROR: Series with the name " + nameWithFreq + " from '" + dbName + "' databank is not an array-series");
                        throw new GekkoException();
                    }

                    iv_series.dimensionsStorage.AddIVariableWithOverwrite(mmi, iv);

                }
                else
                {
                    //normal series, not array-series                    
                    bank.AddIVariableWithOverwrite(nameWithFreq, iv);
                }
            }
            return;
        }

        public static IVariable CurlyMethod(GekkoSmpl smpl, IVariable x)
        {
            IVariable rv = null;
            if (x.Type() == EVariableType.Val && Program.options.string_interpolate_format_val != "")
            {                
                rv = Functions.format(smpl, null, null, x, new ScalarString(Program.options.string_interpolate_format_val));                
            }
            else rv = x;
            return rv;
        }

        public static IVariable DecompLooper(string fullname)
        {
            IVariable iv = GetIVariableFromString(fullname, ECreatePossibilities.NoneReportError, true);            
            return iv;
        }

        public static IVariable GetIVariableFromString(string fullname, ECreatePossibilities type)
        {            
            return GetIVariableFromString(fullname, type, false);  //no searching per default
        }

        public static IVariable GetIVariableFromString(string fullname, ECreatePossibilities type, bool canSearch)
        {
            //canSearch = true will only have effect if also "OPTION databank search = yes".
            
            //if noSearch = true, type .Can and .Must can be used, 
            //else there will be a crash (too dangerous to create a series when the exact bank is not known)

            //Note: Please do not use GetIVariableFromString(... , ... , false), use GetIVariableFromString(..., ...) instead.
            //      This makes it easier to find the places where searching is active.

            if (canSearch == true && (type == ECreatePossibilities.Can || type == ECreatePossibilities.Must))
            {
                G.Writeln2("*** ERROR: Internal error #80975234985");
                throw new GekkoException();
            }

            LookupSettings settings = new LookupSettings(ELookupType.RightHandSide, type, canSearch);
            IVariable iv2 = O.Lookup(null, null, new ScalarString(fullname), null, settings, EVariableType.Var, null);
            return iv2;
            
        }

        public static void PredictSetValue(string name, GekkoTime gt, double d)
        {
            IVariable iv = O.GetIVariableFromString(name + Globals.freqIndicator + G.GetFreq(Program.options.freq), O.ECreatePossibilities.Can, false);
            Series ts = iv as Series;
            ts.SetData(gt, d);
        }

        public static double PredictGetValue(string name, GekkoTime gt)
        {
            //We do not allow searching of vars in databanks
            IVariable iv = O.GetIVariableFromString(name + Globals.freqIndicator + G.GetFreq(Program.options.freq), O.ECreatePossibilities.NoneReturnNull, false);
            if (iv == null)
            {
                G.Writeln2("*** ERROR: PREDICT: Series '" + name + "' does not exist in first-position databank");
                throw new GekkoException();
            }
            Series ts = iv as Series;
            return ts.GetDataSimple(gt);
        }

        //public static IVariable GetIVariableFromString(string dbName, string varName, string freq, string[] indexes, ECreatePossibilities type)
        //{
        //    //type is only relevant for series, ignored for others

        //    string nameWithFreq = G.AddFreqToName(varName, freq);

        //    int[] dimMismatch = null;

        //    Databank bank = null;
        //    if (dbName == null) bank = Program.databanks.GetFirst();
        //    else bank = Program.databanks.GetDatabank(dbName, true);

        //    IVariable iv = bank.GetIVariable(nameWithFreq);

        //    if (G.Chop_HasSigil(nameWithFreq))
        //    {
        //        //%x or #x
        //        if (indexes != null)
        //        {
        //            G.Writeln2("*** ERROR: Name like " + nameWithFreq + "[" + G.GetListWithCommas(indexes) + "]" + " not allowed");
        //            throw new GekkoException();
        //        }
        //        else
        //        {
        //            //just return iv
        //        }
        //    }
        //    else
        //    {
        //        //series name, not starting with % or #

        //        if (indexes != null)
        //        {
        //            //array-series

        //            MapMultidimItem mmi = new MapMultidimItem(indexes);

        //            if (iv == null)
        //            {
        //                G.Writeln2("*** ERROR: Series with the name " + nameWithFreq + " does not exist in '" + bank.name + "' databank");
        //                throw new GekkoException();
        //            }

        //            //now we know that the series exists

        //            Series iv_series = iv as Series;

        //            if (iv_series.type != ESeriesType.ArraySuper)
        //            {
        //                G.Writeln2("*** ERROR: Series with the name " + nameWithFreq + " from '" + bank.name + "' databank is not an array-series");
        //                throw new GekkoException();
        //            }

        //            IVariable iv2 = null; iv_series.dimensionsStorage.TryGetValue(mmi, out iv2);

        //            if (iv2 == null)
        //            {
        //                if (type == ECreatePossibilities.Can || type == ECreatePossibilities.Must)
        //                {
        //                    Series ts = new Series(G.GetFreq(G.Chop_GetFreq(nameWithFreq)), null);
        //                    iv_series.dimensionsStorage.AddIVariableWithOverwrite(mmi, ts);
        //                    iv = ts;
        //                }
        //                else
        //                {
        //                    if (iv_series.dimensions != indexes.Length)
        //                    {
        //                        dimMismatch = new int[2];
        //                        dimMismatch[0] = iv_series.dimensions;
        //                        dimMismatch[1] = indexes.Length;
        //                    }
        //                    iv = iv2;  //that is: iv = null;
        //                }
        //            }
        //            else
        //            {
        //                //it does exist
        //                if (type == ECreatePossibilities.Must)
        //                {
        //                    //overwriting
        //                    Series ts = new Series(G.GetFreq(G.Chop_GetFreq(nameWithFreq)), null);
        //                    iv_series.dimensionsStorage.AddIVariableWithOverwrite(mmi, ts);
        //                    iv = ts;
        //                }
        //                else
        //                {
        //                    //do nothing
        //                    iv = iv2;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //normal series, not array-series
        //            if (iv == null)
        //            {
        //                if (type == ECreatePossibilities.Can || type == ECreatePossibilities.Must)
        //                {
        //                    Series ts = new Series(G.GetFreq(G.Chop_GetFreq(nameWithFreq)), nameWithFreq);
        //                    bank.AddIVariable(nameWithFreq, ts);
        //                    iv = ts;
        //                }
        //                else
        //                {
        //                    //do nothing, a null is returned
        //                }
        //            }
        //            else
        //            {
        //                //it does exist
        //                if (type == ECreatePossibilities.Must)
        //                {
        //                    //overwriting
        //                    Series iv_series = iv as Series;
        //                    Series ts = new Series(iv_series.freq, nameWithFreq);
        //                    bank.RemoveIVariable(nameWithFreq);
        //                    bank.AddIVariable(nameWithFreq, ts);
        //                    iv = ts;
        //                }
        //                else
        //                {
        //                    //do nothing
        //                }
        //            }
        //        }

        //    }

        //    if (iv == null && type == ECreatePossibilities.NoneReportError)
        //    {
        //        //G.Writeln2("*** ERROR: Could not find ")
        //        string vname = varName;
        //        if (freq != null) vname += Globals.freqIndicator + freq;
        //        if (dbName != null) vname = dbName + Globals.symbolBankColon2 + vname;
        //        string s = null;
        //        if (indexes != null)
        //        {
        //            s = "[" + G.GetListWithCommas(indexes) + "]";
        //        }
        //        vname = vname + s;
        //        if (dimMismatch == null)
        //        {
        //            G.Writeln2("*** ERROR: Could not find variable '" + vname + "'");
        //        }
        //        else
        //        {
        //            G.Writeln2("*** ERROR: '" + vname + "' has " + dimMismatch[1] + " dimensions, expected " + dimMismatch[0] + " dimensions");
        //        }
        //        throw new GekkoException();
        //    }

        //    return iv;
        //}

        public static string AcceptHelper2(string type, IVariable iv)
        {
            
            string s = null;
            if (iv == null)
            {
                //do nothing, null is returned
            }
            else
            {
                if (G.Equal(type, "val"))
                {
                    s = ((ScalarVal)iv).val.ToString();
                }
                else if (G.Equal(type, "date"))
                {
                    s = ((ScalarDate)iv).date.ToString();
                }
                else if (G.Equal(type, "string"))
                {
                    s = "'" + iv.ConvertToString() + "'";
                }
                else if (G.Equal(type, "name"))
                {
                    s = iv.ConvertToString(); //no quotes
                }
            }
            return s;
        }

        public static IVariable ProcedureAccept(string type, string message)
        {
            string inputtedValue = null;
            IVariable iv2 = null;
            if (Program.InputBox("Input", message, ref inputtedValue) == DialogResult.OK)
            {
                iv2 = O.AcceptHelper1(type, inputtedValue);
                string s = O.AcceptHelper2(type, iv2);  //send back to gui, s is = inputtedValue?
            }
            return iv2;
        }

        public static List<IVariable> PromptOLD(List<bool> question, List<string> defaultValue, List<string> type, List<string> txt)
        {
            List<IVariable> promptResults = new List<IVariable>();

            for (int i = 0; i < type.Count; i++)
            {
                if (question[i])
                {
                    string tmp = defaultValue[i];
                    DialogResult result = Program.InputBox("Input", txt[i], ref tmp);
                    if (result == DialogResult.OK)
                    {
                        //not if cancel button or escape key
                        defaultValue[i] = tmp;
                        break;  //all the following will attain their default values, similar to putting a ";" in AREMOS.
                    }
                }
                promptResults.Add(O.AcceptHelper1(type[i], defaultValue[i]));
            }

            return promptResults;
        }

        public static List<IVariable> Prompt(List<bool> question, List<IVariable> defaultValue, List<string> type, List<IVariable> txt)
        {
            List<IVariable> promptResults = new List<IVariable>();

            if (type.Count != txt.Count)
            {
                G.Writeln2("*** ERROR: Prompting: one or more of the optional parameters have no labels");
                throw new GekkoException();
            }

            for (int i = 0; i < type.Count; i++)
            {
                if (G.Equal(type[i], "val") || G.Equal(type[i], "date") || G.Equal(type[i], "string") || G.Equal(type[i], "name"))
                {
                    //good
                }
                else
                {
                    G.Writeln2("*** ERROR: Promting is only allowed for types val, date, string or name at the moment.");
                    G.Writeln("    This restriction may be removed in a future Gekko version.", Color.Red);
                    G.Writeln("    The type '" + type[i] + "' is not valid for prompting.", Color.Red);                    
                    throw new GekkoException();
                }

                if (question[i])
                {
                    IVariable tmp = defaultValue[i];
                    string rv = null;

                    DialogResult result = DialogResult.OK;
                    if (G.IsUnitTesting())
                    {
                        rv = Globals.unitTestsPromtingHelper[i];
                    }
                    else
                    {
                        try
                        {                            
                            rv = AcceptHelper2(type[i], tmp);                            
                        }
                        catch { }

                        result = Program.InputBox("Input", txt[i].ConvertToString(), ref rv);
                    }

                    if (result == DialogResult.OK)
                    {
                        string rvTrim = rv.Trim();
                        if (rvTrim == ";")
                        {
                            for (int j = i; j < type.Count; j++)
                            {
                                promptResults.Add(defaultValue[j]);
                            }
                            break; //this and all the following will attain their default values, similar to AREMOS.
                        }
                        else
                        {                            
                            defaultValue[i] = AcceptHelper1(type[i], rvTrim);
                        }                        
                    }
                }
                promptResults.Add(defaultValue[i]);
            }

            return promptResults;
        }


        public static IVariable AcceptHelper1(string type, string value)
        {
            IVariable iv = null;

            if (G.Equal(type, "val"))
            {
                try
                {
                    double v = G.ParseIntoDouble(value.Trim(), true);
                    iv = new ScalarVal(v);
                }
                catch
                {
                    G.Writeln2("*** ERROR: Could not convert '" + value + "' into a VAL");
                    throw new GekkoException();
                }
            }
            else if (G.Equal(type, "string"))
            {
                try
                {
                    string v = value.Trim();
                    v = G.StripQuotes(v);
                    iv = new ScalarString(v);
                }
                catch
                {
                    G.Writeln2("*** ERROR: Could not convert '" + value + "' into a STRING");
                    throw new GekkoException();
                }
            }
            else if (G.Equal(type, "name"))
            {
                try
                {
                    string v = value.Trim();
                    v = G.StripQuotes(v);
                    iv = new ScalarString(v);
                }
                catch
                {
                    G.Writeln2("*** ERROR: Could not convert '" + value + "' into a NAME");
                    throw new GekkoException();
                }
            }
            else if (G.Equal(type, "date"))
            {
                try
                {
                    GekkoTime gt = GekkoTime.FromStringToGekkoTime(value.Trim());
                    iv = new ScalarDate(gt);

                }
                catch
                {
                    G.Writeln2("*** ERROR: Could not convert '" + value + "' into a DATE");
                    throw new GekkoException();
                }
            }

            return iv;
        }

        

        public static IVariable RemoveIVariableFromString(string dbName, string varName, string freq, string[] indexes, bool reportError)
        {
            string nameWithFreq = G.AddFreqToName(varName, freq);
                        
            Databank bank = null;
            if (dbName == null)
            {
                bank = Program.databanks.GetFirst();
            }
            else
            {
                bank = Program.databanks.GetDatabank(dbName, true);
            }

            if (!bank.editable) Program.ProtectError("You cannot remove a variable to a non-editable databank, see OPEN<edit> or UNLOCK");

            IVariable iv = bank.GetIVariable(nameWithFreq);

            if (iv == null)
            {
                if (reportError)
                {
                    G.Writeln2("*** ERROR: Variable " + dbName + Globals.symbolBankColon + nameWithFreq + " does not exist for deletion");
                    throw new GekkoException();
                }
                else
                {
                    G.Writeln("+++ WARNING: Variable " + dbName + Globals.symbolBankColon + nameWithFreq + " does not exist for deletion");
                    return iv;
                }
            }

            if (G.Chop_HasSigil(nameWithFreq))
            {
                //%x or #x
                if (indexes != null)
                {
                    G.Writeln2("*** ERROR: Name like " + nameWithFreq + "[" + G.GetListWithCommas(indexes) + "]" + " not allowed");
                    throw new GekkoException();
                }
                else
                {
                    //just remove it
                    bank.RemoveIVariable(nameWithFreq);
                }
            }
            else
            {
                //series name, not starting with % or #

                if (indexes != null)
                {
                    //array-series

                    MapMultidimItem mmi = new MapMultidimItem(indexes);

                    //now we know that the series exists

                    Series iv_series = iv as Series;

                    if (iv_series.type == ESeriesType.ArraySuper)
                    {
                        G.Writeln2("*** ERROR: Series with the name " + nameWithFreq + " from '" + dbName + "' databank is not an array-series");
                        throw new GekkoException();
                    }

                    IVariable iv2 = null; iv_series.dimensionsStorage.TryGetValue(mmi, out iv2);

                    if (iv2 == null)
                    {
                        G.Writeln2("*** ERROR: Array-series " + nameWithFreq + "[" + G.GetListWithCommas(indexes) + "]" + " does not exist");
                        throw new GekkoException();
                    }
                    else
                    {
                        iv_series.dimensionsStorage.RemoveIVariable(mmi);
                    }

                }
                else
                {
                    //normal series, not array-series
                    bank.RemoveIVariable(nameWithFreq);
                }
            }
            return iv;
        }



        //See also Restrict()
        public static List Restrict2(List m, bool allowBank, bool allowSigil, bool allowFreq, bool allowIndexes)
        {
            if (m == null) return null;
            foreach (IVariable iv in m.list)
            {
                string s = RestrictHelper(allowBank, allowSigil, allowFreq, allowIndexes, iv);
                //don't use the string, just test it
            }
            return m;  //untouched
        }

        //See also Restrict2()
        public static List<string> Restrict(List m, bool allowBank, bool allowSigil, bool allowFreq, bool allowIndexes)
        {
            if (m == null) return null;
            List<string> rv = new List<string>();
            foreach (IVariable iv in m.list)
            {
                string s = RestrictHelper(allowBank, allowSigil, allowFreq, allowIndexes, iv);
                rv.Add(s);
            }
            return rv;
        }

        private static string RestrictHelper(bool allowBank, bool allowSigil, bool allowFreq, bool allowIndexes, IVariable iv)
        {
            if (iv.Type() == EVariableType.Range)
            {
                Range iv_range = iv as Range;
                string s1 = iv_range.first.ConvertToString().Trim();
                string s2 = iv_range.last.ConvertToString().Trim();
                return s1 + ".." + s2;
            }

            string s = iv.ConvertToString();
            if (!allowBank && s.Contains(Globals.symbolBankColon))
            {
                G.Writeln2("*** ERROR: Bankname not accepted as part of name");
                throw new GekkoException();
            }
            if (!allowSigil && s.Contains(Globals.symbolScalar))
            {
                G.Writeln2("*** ERROR: Scalar symbol (" + Globals.symbolScalar + ") not accepted, use {%x} instead of %x");
                throw new GekkoException();
            }
            if (!allowSigil && s.Contains(Globals.symbolCollection))
            {
                G.Writeln2("*** ERROR: Collection symbol (" + Globals.symbolCollection + ") not accepted, you may use {#x} instead of #x");
                G.Writeln("    If you are concatenating lists, use '+' to add the elements of one list to another", Color.Red);
                throw new GekkoException();
            }
            if (!allowFreq && s.Contains(Globals.freqIndicator))
            {
                G.Writeln2("*** ERROR: Frequency (" + Globals.freqIndicator + ") not accepted as part of name");
                throw new GekkoException();
            }
            if (!allowIndexes && (s.Contains("[") || s.Contains("]")))
            {
                G.Writeln2("*** ERROR: Index [...] not accepted as part of name");
                throw new GekkoException();
            }

            return s;
        }

        public static List CreateListFromStrings(string[] input)
        {
            List m = new List(new List<string>(input));
            return m;
        }

        private static IVariable LookupHelperRightside2(Map map, string dbName, string varnameWithFreq)
        {
            IVariable rv;
            IBank ib = null;
            if (map != null) ib = map;
            else ib = Program.databanks.GetDatabank(dbName, true);
            rv = ib.GetIVariable(varnameWithFreq);
            if (rv == null)
            {

                G.Writeln2("*** ERROR: Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in " + ib.Message());
                throw new GekkoException();
            }
            return rv;
        }        

        public static void InitSmpl(GekkoSmpl smpl, P p)
        {
            //called before each command is run
            if (smpl != null)
            {
                smpl.t0 = Globals.globalPeriodStart.Add(Globals.smplInitStart);
                smpl.t1 = Globals.globalPeriodStart;
                smpl.t2 = Globals.globalPeriodEnd;
                smpl.t3 = Globals.globalPeriodEnd.Add(Globals.smplInitEnd);
                smpl.gekkoError = null;
                //smpl.gekkoErrorI = 0;
                smpl.bankNumber = 0;
                //p.numberOfServiceMessages = 0;
                smpl.p = p;
            }
        }


        public static bool Dynamic2(GekkoSmpl smpl)
        {
            bool rv = false;
            if (smpl.lhsAssignmentType == assignmantTypeLhs.Series) rv = true;
            smpl.lhsAssignmentType = assignmantTypeLhs.Inactive;  //inactive            
            return rv;
        }

        public static void Dynamic1(GekkoSmpl smpl)
        {
            smpl.lhsAssignmentType = assignmantTypeLhs.Active;  //charged
        }




        public static IVariable LookupHelperLeftsideOLD(GekkoSmpl smpl, IBank ib, string varnameWithFreq, string freq, IVariable rhsExpression)
        {
            //This is an assignment, for instance %x = 5, or x = (1, 2, 3), or bank:x = bank:y

            IVariable lhs = ib.GetIVariable(varnameWithFreq);

            if (varnameWithFreq[0] != Globals.symbolScalar && varnameWithFreq[0] != Globals.symbolCollection)
            {
                //first, if it is a SERIES on lhs (no sigil), we try to convert the RHS directly (so x = ... will work, not necessary with SERIES x = ...)
                rhsExpression = O.ConvertToTimeSeries(smpl, rhsExpression);  //for instance, x = (1, 2, 3) will have (1, 2, 3) converted to series
            }

            LookupTypeCheck(rhsExpression, varnameWithFreq);

            if (lhs == null)
            {
                //LEFT-HAND SIDE DOES NOT EXIST
                //LEFT-HAND SIDE DOES NOT EXIST
                //LEFT-HAND SIDE DOES NOT EXIST
                if (varnameWithFreq[0] == Globals.symbolScalar)
                {
                    //VAL, STRING, DATE                                                
                    ib.AddIVariable(varnameWithFreq, rhsExpression.DeepClone(null));
                }
                else if (varnameWithFreq[0] == Globals.symbolCollection)
                {
                    //LIST, DICT, MATRIX                                                
                    ib.AddIVariable(varnameWithFreq, rhsExpression.DeepClone(null));
                }
                else
                {
                    //SERIES
                    //check if it can be created          
                    if (ib.BankType() == EBankType.Normal && !Program.options.databank_create_auto && !varnameWithFreq.StartsWith("xx", StringComparison.OrdinalIgnoreCase))
                    {
                        G.Writeln2("*** ERROR: Cannot create timeseries '" + varnameWithFreq);
                        G.Writeln("    You may use CREATE " + varnameWithFreq + ", or use MODE data, or use a name starting with 'xx'", Color.Red);
                        throw new GekkoException();
                    }
                }
            }

            if (true)
            {
                //LEFT-HAND SIDE EXISTS
                //LEFT-HAND SIDE EXISTS   or can be created if it is a series name
                //LEFT-HAND SIDE EXISTS
                if (varnameWithFreq[0] == Globals.symbolScalar)
                {
                    //VAL, STRING, DATE
                    if (lhs.Type() == rhsExpression.Type())
                    {
                        //fast, especially in loops!
                        if (lhs.Type() == EVariableType.Val) ((ScalarVal)lhs).val = ((ScalarVal)rhsExpression).val;
                        else if (lhs.Type() == EVariableType.Date) ((ScalarDate)lhs).date = ((ScalarDate)rhsExpression).date;
                        else if (lhs.Type() == EVariableType.String) ((ScalarString)lhs).string2 = ((ScalarString)rhsExpression).string2;
                    }
                    else
                    {
                        ib.RemoveIVariable(varnameWithFreq);
                        ib.AddIVariable(varnameWithFreq, rhsExpression.DeepClone(null));
                    }
                }
                else if (varnameWithFreq[0] == Globals.symbolCollection)
                {
                    //LIST, MAP, MATRIX, variable already exists
                    if (lhs.Type() == rhsExpression.Type())
                    {
                        //TODO: Here we could copy the inside of the object, and put this inside into existing object
                        //      Hence, it would not need to be removed and added to the dictionary, and a new object is not needed.
                    }
                    //this is safe, but a little slow in some cases --> see above
                    ib.RemoveIVariable(varnameWithFreq);
                    ib.AddIVariable(varnameWithFreq, rhsExpression.DeepClone(null));
                }
                else
                {
                    //SERIES
                    Series tsLhs = null;
                    if (rhsExpression.Type() == EVariableType.Series)
                    {
                        Series tsRhs = rhsExpression as Series;
                        //LIGHTFIX, speed  

                        if (tsLhs == null)
                        {
                            tsLhs = new Series(ESeriesType.Normal, tsRhs.freq, varnameWithFreq);
                            ib.AddIVariable(tsLhs.name, tsLhs);
                        }

                        //NOW, we have xx2 = xx1, where these may be different as regards whether they are array-timeseries.

                        if (tsLhs.type != ESeriesType.ArraySuper && tsRhs.type != ESeriesType.ArraySuper)
                        {
                            //Inject the data, only for the current
                            //This must run fast
                            //LIGHTFIX, speed
                            foreach (GekkoTime t in smpl.Iterate12())
                            {
                                tsLhs.SetData(t, tsRhs.GetData(smpl, t));
                            }
                        }
                        else
                        {
                            //types are differnet (array and non-array), or both are array (not likely...). In these cases, we wipe out the existing timeseries
                            ib.RemoveIVariable(varnameWithFreq);
                            Series tsTmp = (Series)rhsExpression.DeepClone(null);
                            tsTmp.name = varnameWithFreq;
                            if (ib.GetType() == typeof(Databank))
                            {
                                if (tsTmp.type == ESeriesType.Light) tsTmp.meta = new SeriesMetaInformation();  //so that parentDatabank can be put in in ib.AddIVariable                               
                            }
                            tsTmp.meta = new SeriesMetaInformation();
                            ib.AddIVariable(tsTmp.name, tsTmp);
                        }

                    }
                    else if (rhsExpression.Type() == EVariableType.Val)
                    {
                        if (tsLhs == null)
                        {
                            tsLhs = new Series(ESeriesType.Normal, Program.options.freq, varnameWithFreq);
                            ib.AddIVariable(tsLhs.name, tsLhs);
                        }
                        ScalarVal sv = rhsExpression as ScalarVal;
                        //LIGHTFIX, speed
                        foreach (GekkoTime t in smpl.Iterate12())
                        {
                            tsLhs.SetData(t, sv.val);
                        }
                    }
                    //TODO
                }
            }
            return lhs;
        }

        public static void LookupHelperLeftside(GekkoSmpl smpl, IBank ib, string varnameWithFreq, string freq, IVariable rhs, EVariableType type, O.Assignment options)
        {
            //normal use
            LookupHelperLeftside(smpl, ib, varnameWithFreq, freq, rhs, null, type, options);
        }

        public static void LookupHelperLeftside(GekkoSmpl smpl, Series arraySubSeries, IVariable rhs, EVariableType type, O.Assignment options)
        {
            //use for array-series, for instance xx['a'] = ...
            LookupHelperLeftside(smpl, null, null, null, rhs, arraySubSeries, type, options);
        }

        private static void LookupHelperLeftside(GekkoSmpl smpl, IBank ib, string varnameWithFreq, string freq, IVariable rhs, Series arraySubSeries, EVariableType lhsType, O.Assignment o)
        {
            //This is an assignment, for instance %x = 5, or x = (1, 2, 3), or bank:x = bank:y, or #m.x = (1, 2, 3).
            //Assignment is the hardest part of Lookup()

            bool isArraySubSeries = false;
            if (arraySubSeries != null) isArraySubSeries = true;

            if (smpl.lhsAssignmentType == assignmantTypeLhs.Active)
            {
                //active
                if (isArraySubSeries)
                {
                    //in this case, varnameWithFreq will be null, but we are sure it is a series
                    smpl.lhsAssignmentType = assignmantTypeLhs.Series;
                }
                else
                {
                    if (G.Chop_HasSigil(varnameWithFreq)) smpl.lhsAssignmentType = assignmantTypeLhs.Nonseries;
                    else smpl.lhsAssignmentType = assignmantTypeLhs.Series;
                }
                return;  //is just a probe on the type of the lhs, so we return without changing anything!
            }

            if (ib != null && ib.BankType() == EBankType.Normal)
            {
                //ib can be == null with an indexer on the lhs, like #m.#n.%s
                Databank ib_databank = ib as Databank;
                if (!ib_databank.editable) Program.ProtectError("You cannot add/change a variable in non-editable databank ('" + ib_databank.name + "'), see OPEN<edit> or UNLOCK");
                ib_databank.isDirty = true;
            }

            if (rhs.Type() == EVariableType.List)
            {
                List rhs_list = rhs as List;
                
            }
            
            if (varnameWithFreq != null && varnameWithFreq.StartsWith(Globals.symbolCollection + Globals.listfile + "___"))
            {
                WriteListFile(varnameWithFreq, rhs);
            }
            else
            {
                IVariable lhs = null;
                if (ib != null)
                {
                    //ib can be == null with an indexer on the lhs, like #m.#n.%s
                    lhs = ib.GetIVariable(varnameWithFreq); //may return null
                }

                //We divide into three groups depending on LHS name:
                //  A. Starts with '%'
                //  B. Starts with '#'
                //  C. No sigil (or isArraySubSeries == true)

                //For each A, B, C, we also have the 7 possible types of the RHS, for instace ... = 2012q1 (date type)
                //  And for each of these 7 types, we may have a LHS type indicator, for instance DATE %d = ...  (should become date)

                if (!isArraySubSeries && varnameWithFreq[0] == Globals.symbolScalar)
                {
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // Starts with '%'

                    //smpl.omitDynamicSeries = true;

                    if (lhsType == EVariableType.Val || lhsType == EVariableType.String || lhsType == EVariableType.Date || lhsType == EVariableType.Var)
                    {
                        //good
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Name '" + varnameWithFreq + "' with '" + Globals.symbolScalar + "' symbol cannot be of " + lhsType.ToString().ToUpper() + " type");
                        throw new GekkoException();
                    }

                    switch (rhs.Type())
                    {
                        case EVariableType.Series:
                            {
                                //%x = SERIES

                                Series rhsExpression_series = rhs as Series;
                                switch (rhsExpression_series.type)
                                {
                                    case ESeriesType.Timeless:
                                        {
                                            //---------------------------------------------------------
                                            // %x = Series Timeless
                                            //---------------------------------------------------------
                                            if (lhsType == EVariableType.Val || lhsType == EVariableType.Var)
                                            {
                                                // VAL %x = Series Timeless
                                                IVariable lhsNew = new ScalarVal(rhsExpression_series.GetTimelessData());
                                                AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                                G.ServiceMessage("VAL " + varnameWithFreq + " updated ", smpl.p);
                                            }
                                            else
                                            {
                                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                                            }
                                        }
                                        break;
                                    default:
                                        {
                                            //---------------------------------------------------------
                                            // %x = Series Normal
                                            //---------------------------------------------------------                                        
                                            ReportTypeError(varnameWithFreq, rhs, lhsType);
                                        }
                                        break;
                                }
                            }
                            break;
                        case EVariableType.Val:
                            {
                                //---------------------------------------------------------
                                // %x = VAL
                                //---------------------------------------------------------
                                //TODO: date %d = 2010.

                                if (lhsType == EVariableType.Val || lhsType == EVariableType.Var)
                                {
                                    IVariable lhsNew = new ScalarVal(((ScalarVal)rhs).val);
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                    G.ServiceMessage("VAL " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else if (lhsType == EVariableType.Date)
                                {
                                    IVariable lhsNew = new ScalarDate(rhs.ConvertToDate(GetDateChoices.Strict));
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                    G.ServiceMessage("DATE " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    //STRING command will fail
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        case EVariableType.String:
                            {
                                //---------------------------------------------------------
                                // %x = STRING
                                //---------------------------------------------------------                            

                                if (lhsType == EVariableType.String || lhsType == EVariableType.Var)
                                {
                                    IVariable lhsNew = new ScalarString(((ScalarString)rhs).string2);
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                    G.ServiceMessage("STRING " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    //DATE and VAL commands will fail
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }

                            }
                            break;
                        case EVariableType.Date:
                            {
                                //---------------------------------------------------------
                                // %x = DATE
                                //---------------------------------------------------------

                                if (lhsType == EVariableType.Date || lhsType == EVariableType.Var)
                                {
                                    IVariable lhsNew = new ScalarDate(((ScalarDate)rhs).date);
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                    G.ServiceMessage("DATE " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    //STRING and VAL commands will fail
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }

                            }
                            break;
                        case EVariableType.List:
                            {
                                //---------------------------------------------------------
                                // %x = LIST
                                //---------------------------------------------------------
                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                            }
                            break;
                        case EVariableType.Map:
                            {
                                //---------------------------------------------------------
                                // %x = MAP
                                //---------------------------------------------------------

                                ReportTypeError(varnameWithFreq, rhs, lhsType);

                            }
                            break;
                        case EVariableType.Matrix:
                            {
                                //---------------------------------------------------------
                                // %x = MATRIX
                                //---------------------------------------------------------                            
                                if (lhsType == EVariableType.Val || lhsType == EVariableType.Var)
                                {
                                    IVariable lhsNew = new ScalarVal(rhs.ConvertToVal());  //only 1x1 matrix will become VAL
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                    G.ServiceMessage("VAL " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {                                    
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        case EVariableType.Null:
                            {
                                //---------------------------------------------------------
                                // %x = NULL
                                //---------------------------------------------------------                            
                                G.Writeln2("*** ERROR: Null-value on right-hand side");
                                throw new GekkoException();
                            }
                            break;
                        default:
                            {
                                G.Writeln2("*** ERROR: Expected variable to be series, val, date, string, list, map or matrix");
                                throw new GekkoException();
                            }
                            break;
                    }
                }
                else if (!isArraySubSeries && varnameWithFreq[0] == Globals.symbolCollection)
                {
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // Starts with '#'

                    //smpl.omitDynamicSeries = true;

                    if (lhsType == EVariableType.List || lhsType == EVariableType.Matrix || lhsType == EVariableType.Map || lhsType == EVariableType.Var)
                    {
                        //good
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Name '" + varnameWithFreq + "' with '" + Globals.symbolCollection + "' symbol cannot be of " + lhsType.ToString().ToUpper() + " type");
                        throw new GekkoException();
                    }

                    switch (rhs.Type())
                    {
                        case EVariableType.Series:
                            {
                                Series rhs_series = rhs as Series;
                                switch (rhs_series.type)
                                {
                                    case ESeriesType.Normal:
                                        {
                                            //---------------------------------------------------------
                                            // #x = Series Normal --> not allowed, but MATRIX #m = Series Normal is ok
                                            //---------------------------------------------------------

                                            //if (lhsType == EVariableType.Matrix || lhsType == EVariableType.Var)
                                            if (lhsType == EVariableType.Matrix)
                                                {

                                                // array    smpl          destination
                                                // source
                                                //         
                                                //           o   i1=-1    y 0             --> will become NaN
                                                //   x 0     o            y 1
                                                //   x 1     o            y 2
                                                //   x 2     o            y 3
                                                //   x 3     o            y 4
                                                //           o   i2 = 4   y 5             --> will become NaN
                                                //                                        

                                                //method will only work if smpl freq is same as series freq
                                                int n = smpl.Observations12();
                                                //int i1 = rhs_series.FromGekkoTimeToArrayIndex(smpl.t1);
                                                //int i2 = rhs_series.FromGekkoTimeToArrayIndex(smpl.t2);                                                
                                                //double[] source = rhs_series.GetDataArray();

                                                int i1; int i2;
                                                double[] source = rhs_series.GetDataSequenceUnsafePointerReadOnlyBEWARE(out i1, out i2, smpl.t1, smpl.t2);

                                                Matrix m = new Matrix(1, n);
                                                double[,] destination = m.data;

                                                int destinationStart = 0;
                                                
                                                Buffer.BlockCopy(source, 8 * i1, destination, 8 * destinationStart, 8 * (i2 - i1 + 1));
                                                IVariable lhsNew = m;

                                                if (Series.MissingZero(rhs_series)) G.ReplaceNaNWith0(m.data);

                                                AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);

                                                G.ServiceMessage("MATRIX " + varnameWithFreq + " updated ", smpl.p);
                                            }
                                            else
                                            {
                                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                                            }
                                        }
                                        break;
                                    case ESeriesType.Light:
                                        {

                                            //---------------------------------------------------------
                                            // #x = Series Light --> not allowed, but MATRIX #m = Series Light is ok
                                            //---------------------------------------------------------

                                            //if (lhsType == EVariableType.Matrix || lhsType == EVariableType.Var)
                                            if (lhsType == EVariableType.Matrix)
                                            {

                                                //method will only work if smpl freq is same as series freq
                                                int n = smpl.Observations12();
                                                Matrix m = new Matrix(1, n);
                                                int ii1 = rhs_series.FromGekkoTimeToArrayIndex(smpl.t1);
                                                int ii2 = rhs_series.FromGekkoTimeToArrayIndex(smpl.t2);

                                                int tooSmall = 0; int tooLarge = 0;
                                                rhs_series.TooSmallOrTooLarge(ii1, ii2, out tooSmall, out tooLarge);
                                                if (tooSmall > 0 || tooLarge > 0)
                                                {
                                                    if (smpl.gekkoError == null) smpl.gekkoError = new GekkoError(tooSmall, tooLarge);
                                                    return;
                                                }

                                                int destinationStart = 0;
                                                double[,] destination = m.data;
                                                double[] source = rhs_series.GetDataSequenceUnsafePointerReadOnlyBEWARE();
                                                //see #0985324985237
                                                Buffer.BlockCopy(source, 8 * ii1, destination, 8 * destinationStart, 8 * (ii2 - ii1 + 1));
                                                IVariable lhsNew = m;
                                                //if (Series.MissingZero()) G.ReplaceNaNWith0(m.data); --> NO! Series light do not get replacement
                                                AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                                G.ServiceMessage("MATRIX " + varnameWithFreq + " updated ", smpl.p);
                                            }
                                            else
                                            {
                                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                                            }
                                        }
                                        break;
                                    case ESeriesType.Timeless:
                                        {
                                            //---------------------------------------------------------
                                            // #x = Series Timeless --> not allowed, but MATRIX #m = Series Timeless is ok
                                            //---------------------------------------------------------

                                            //if (lhsType == EVariableType.Matrix || lhsType == EVariableType.Var)
                                            if (lhsType == EVariableType.Matrix)
                                                {
                                                int n = smpl.Observations12();
                                                double d = rhs_series.GetDataSequenceUnsafePointerAlterBEWARE()[0];
                                                if (Series.MissingZero(rhs_series) && G.isNumericalError(d)) d = 0d;
                                                Matrix m = new Matrix(1, n, d);  //expanded as if it was a real timeseries                                       
                                                AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, m);
                                                G.ServiceMessage("MATRIX " + varnameWithFreq + " updated ", smpl.p);
                                            }
                                            else
                                            {
                                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                                            }
                                        }
                                        break;
                                    case ESeriesType.ArraySuper:
                                        {
                                            //---------------------------------------------------------
                                            // #x = Series Array Super
                                            //---------------------------------------------------------
                                            {
                                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                                            }
                                        }
                                        break;
                                    default:
                                        {
                                            G.Writeln2("*** ERROR: Expected SERIES to be 1 of 4 types");
                                            throw new GekkoException();
                                        }
                                        break;
                                }
                            }
                            break;
                        case EVariableType.Val:
                            {
                                //---------------------------------------------------------
                                // #x = VAL
                                //---------------------------------------------------------
                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                            }
                            break;
                        case EVariableType.String:
                            {
                                //---------------------------------------------------------
                                // #x = STRING
                                //---------------------------------------------------------

                                ReportTypeError(varnameWithFreq, rhs, lhsType);

                            }
                            break;
                        case EVariableType.Date:
                            {
                                //---------------------------------------------------------
                                // #x = DATE
                                //---------------------------------------------------------

                                ReportTypeError(varnameWithFreq, rhs, lhsType);

                            }
                            break;
                        case EVariableType.List:
                            {
                                //---------------------------------------------------------
                                // #x = LIST
                                //---------------------------------------------------------         
                                if (lhsType == EVariableType.List || lhsType == EVariableType.Var)
                                {
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, rhs.DeepClone(null));
                                    G.ServiceMessage("LIST " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        case EVariableType.Map:
                            {
                                //---------------------------------------------------------
                                // #x = MAP
                                //---------------------------------------------------------

                                if (lhsType == EVariableType.Map || lhsType == EVariableType.Var)
                                {
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, rhs.DeepClone(null));
                                    G.ServiceMessage("MAP " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        case EVariableType.Matrix:
                            {
                                //---------------------------------------------------------
                                // #x = MATRIX
                                //---------------------------------------------------------
                                if (lhsType == EVariableType.Matrix || lhsType == EVariableType.Var)
                                {
                                    Matrix m = rhs.DeepClone(null) as Matrix;
                                    if (o.opt_colnames != null) m.colnames = new List<string>(Program.GetListOfStringsFromListOfIvariables(O.ConvertToList(o.opt_colnames).ToArray()));
                                    if (o.opt_rownames != null) m.rownames = new List<string>(Program.GetListOfStringsFromListOfIvariables(O.ConvertToList(o.opt_rownames).ToArray()));
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, m);
                                    G.ServiceMessage("MATRIX " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        default:
                            {
                                G.Writeln2("*** ERROR: Expected IVariable to be 1 of 7 types");
                                throw new GekkoException();
                            }
                            break;
                    }
                }
                else
                {
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    //name is of series type (no sigils), or we have that isArraySubSeries == true (or both)

                    if (lhs == null && !isArraySubSeries && !varnameWithFreq.StartsWith("xx", StringComparison.OrdinalIgnoreCase))
                    {
                        //nonexisting series
                        if (!Program.options.databank_create_auto)
                        {
                            //#07549843254
                            G.Writeln2("*** ERROR: Cannot auto-create series " + varnameWithFreq + ". See the CREATE command.");
                            G.Writeln("           You may change the settings with the following option:", Color.Red);
                            G.Writeln("           OPTION databank create auto = yes;", Color.Red);
                            G.Writeln("           Alternatively, use 'MODE data;' or 'MODE mixed;'.", Color.Red);
                            throw new GekkoException();
                        }
                    }

                    //The indicated LHS type can only be series or var type, for instance SERIES x = ...  or VAR x = ...  or x = ...  . 
                    if (lhsType == EVariableType.Series || lhsType == EVariableType.Var)
                    {
                        //good
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Name '" + varnameWithFreq + "' without '" + Globals.symbolScalar + "' or '" + Globals.symbolCollection + "' symbol cannot be of " + lhsType.ToString().ToUpper() + " type");
                        throw new GekkoException();
                    }

                    //Now we know that it is either SERIES x = ...  or VAR x = ...  or x = ...   

                    bool removeFirst = true;

                    Series lhs_series = null;
                    if (isArraySubSeries) lhs_series = arraySubSeries;
                    else lhs_series = lhs as Series;

                    //TODO: error if more than 1 is set
                    ESeriesUpdTypes operatorType = GetOperatorType(o);
                    bool keep = false; if (o != null && G.Equal(o.opt_keep, "p")) keep = true;

                    Series original = null;
                    if (keep || false)
                    {
                        original = (Series)lhs_series.DeepClone(null);
                    }

                    bool create = CreateSeriesIfNotExisting(varnameWithFreq, freq, ref lhs_series);

                    if (!isArraySubSeries)
                    {
                        lhs_series.meta.stamp = Program.GetDateStamp();
                        if (o?.opt_label != null) lhs_series.meta.label = O.ConvertToString(o.opt_label);
                        if (o?.opt_source != null) lhs_series.meta.source = O.ConvertToString(o.opt_source);
                        if (o?.opt_units != null) lhs_series.meta.units = O.ConvertToString(o.opt_units);
                        if (o?.opt_stamp != null) lhs_series.meta.stamp = O.ConvertToString(o.opt_stamp);  //will override

                        if (!G.NullOrEmpty(lhs_series.meta.source))
                        {
                            if (lhs_series.meta.source.StartsWith("<[code]>"))
                            {
                                lhs_series.meta.source = smpl.t1.ToString() + "-" + smpl.t2.ToString() + ": " + lhs_series.meta.source.Replace("<[code]>", "");
                            }
                        }
                    }
                        
                    switch (rhs.Type())
                    {
                        case EVariableType.Series:
                            {
                                Series rhs_series_beware = rhs as Series;
                                string freq_rhs = G.GetFreq(rhs_series_beware.freq);
                                if (varnameWithFreq != null && !varnameWithFreq.ToLower().EndsWith(Globals.freqIndicator + freq_rhs))  //null if it is a subseries under an array-superseries
                                {
                                    G.Writeln2("*** ERROR: Frequency: illegal series name '" + varnameWithFreq + "', should end with '" + Globals.freqIndicator + freq_rhs + "'");
                                    throw new GekkoException();
                                }

                                if (Program.options.series_dyn_check)
                                {
                                    if (CheckDyn2(o))
                                    {
                                        //Neither <dyn> nor BLOCK series dyn have not been set
                                        //options can be == null, in that case there is no <...>-field
                                        if (Globals.precedentsSeries != null)
                                        {
                                            if (Globals.precedentsSeries.ContainsKey(lhs_series))
                                            {
                                                int obs = smpl.Observations12();
                                                if (obs > 1)
                                                {
                                                    G.Writeln2("*** ERROR: It seems the left-hand side variable appears with a lag on the right-hand side.");
                                                    G.Writeln("           When 'OPTION series dyn check = yes' (default), in such dynamic statements you", Color.Red);
                                                    G.Writeln("           must use <dyn> or <dyn = no> tags, or put the expression inside a", Color.Red);
                                                    G.Writeln("           'BLOCK series dyn = yes|no'.", Color.Red);
                                                    G.Writeln();
                                                    Action a = () =>
                                                    {
                                                        O.Help("i_dynamic_statements");
                                                    };
                                                    G.Writeln("           Read more about this error " + G.GetLinkAction("here", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ". If you are uprading from a Gekko version < 3.1.7 to a", Color.Red);
                                                    G.Writeln("           Gekko version >= 3.1.7, this error may come out of the blue. In that case, see the", Color.Red);
                                                    G.Writeln("           'Backwards incompatibility, or how to ignore' section in the above link.", Color.Red);
                                                    G.Writeln();
                                                    throw new GekkoException(); 
                                                }
                                            }
                                        }
                                    }
                                }

                                switch (rhs_series_beware.type)
                                {

                                    case ESeriesType.Normal:
                                    case ESeriesType.Light:
                                        {
                                            //---------------------------------------------------------
                                            // x = Series Normal or Light
                                            //---------------------------------------------------------

                                            if (operatorType == ESeriesUpdTypes.none || operatorType == ESeriesUpdTypes.n)
                                            {
                                                //this runs fast

                                                GekkoTime tt1 = GekkoTime.tNull;
                                                GekkoTime tt2 = GekkoTime.tNull;
                                                GekkoTime.ConvertFreqs(G.GetFreq(freq, true), smpl.t1, smpl.t2, ref tt1, ref tt2);  //converts smpl.t1 and smpl.t2 to tt1 and tt2 in freq frequency
                                                                                                                                    //bool create = CreateSeriesIfNotExisting(varnameWithFreq, freq, ref lhs_series);
                                                                                                                                    //Now the smpl window runs from tt1 to tt2
                                                                                                                                    //We copy in from that window
                                                if (lhs_series.freq != rhs_series_beware.freq)
                                                {
                                                    G.Writeln2("*** ERROR: Frequency mismatch. Left-hand series is " + G.GetFreqString(lhs_series.freq) + ",");
                                                    G.Writeln("           whereas right-hand series is " + G.GetFreqString(lhs_series.freq), Color.Red);
                                                    throw new GekkoException();
                                                }

                                                if (rhs_series_beware.type == ESeriesType.Light)
                                                {
                                                    int tooSmall = 0; int tooLarge = 0;
                                                    rhs_series_beware.TooSmallOrTooLarge(rhs_series_beware.GetArrayIndex(tt1), rhs_series_beware.GetArrayIndex(tt2), out tooSmall, out tooLarge);
                                                    if (tooSmall > 0 || tooLarge > 0)
                                                    {
                                                        if (smpl.gekkoError == null) smpl.gekkoError = new GekkoError(tooSmall, tooLarge);
                                                        return;
                                                    }
                                                }

                                                int index1, index2;
                                                //may enlarge the array with NaNs first and last
                                                double[] data_beware_do_not_alter = rhs_series_beware.GetDataSequenceUnsafePointerReadOnlyBEWARE(out index1, out index2, tt1, tt2);                                                                                                                                                
                                                lhs_series.SetDataSequence(tt1, tt2, data_beware_do_not_alter, index1, Series.MissingZero(rhs_series_beware));
                                            }
                                            else
                                            {
                                                //not so fast running, could be improved
                                                OperatorHelperSeries(smpl, lhs_series, rhs_series_beware, operatorType);
                                            }
                                            //G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                                            LookupHelperLeftside_message(smpl, lhs_series.freq, varnameWithFreq);
                                        }
                                        break;
                                    case ESeriesType.Timeless:
                                        {
                                            //---------------------------------------------------------
                                            // x = Series Timeless
                                            //---------------------------------------------------------
                                            // stuff below also handles array-timeseries just fine  
                                            
                                            if (create)
                                            {                                                
                                                lhs_series = rhs_series_beware.DeepClone(null) as Series;  //so that it becomes timeless, too                                                
                                                lhs_series.name = varnameWithFreq; ;
                                                double[] temp = lhs_series.GetDataSequenceUnsafePointerAlterBEWARE();  //sets dirty, but it *is* dirty
                                                if (Series.MissingZero(rhs_series_beware) && G.isNumericalError(temp[0]))
                                                {
                                                    temp[0] = 0d;
                                                }
                                            }
                                            else
                                            {
                                                double d = double.NaN;
                                                if (rhs_series_beware.GetDataSequenceUnsafePointerReadOnlyBEWARE() != null) d = rhs_series_beware.GetDataSequenceUnsafePointerReadOnlyBEWARE()[0];
                                                if (Series.MissingZero(rhs_series_beware) && G.isNumericalError(d))
                                                {
                                                    d = 0d;
                                                }

                                                if (operatorType == ESeriesUpdTypes.none || operatorType == ESeriesUpdTypes.n)
                                                {
                                                    if (O.UseFlexFreq(smpl.t1, lhs_series.freq))
                                                    {
                                                        foreach (GekkoTime t in smpl.Iterate12(lhs_series.freq))
                                                        {
                                                            lhs_series.SetData(t, d);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        foreach (GekkoTime t in smpl.Iterate12())
                                                        {
                                                            lhs_series.SetData(t, d);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    OperatorHelperScalar(smpl, lhs_series, operatorType, d);
                                                }
                                            }
                                            //G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                                            LookupHelperLeftside_message(smpl, lhs_series.freq, varnameWithFreq);

                                        }
                                        break;
                                    case ESeriesType.ArraySuper:
                                        {
                                            //---------------------------------------------------------
                                            // x = Series Array Super
                                            //---------------------------------------------------------

                                            create = true;  //always create a fresh one, if there is an array-series on the RHS. Does not make sense to merge into existing array-series

                                            if (isArraySubSeries)
                                            {
                                                G.Writeln2("*** ERROR: You cannot put an array-series inside an array-series");
                                                throw new GekkoException();
                                            }

                                            if (operatorType != ESeriesUpdTypes.none && operatorType != ESeriesUpdTypes.n)
                                            {
                                                G.Writeln2("*** ERROR: Operators cannot be used for array-series (yet)");
                                                throw new GekkoException();
                                            }

                                            lhs_series = rhs.DeepClone(null) as Series;
                                            lhs_series.name = varnameWithFreq;
                                            //!we need to make all the subseries point to the superseries, this pointer is used in DECOMP and other places
                                            foreach (KeyValuePair<MapMultidimItem, IVariable> kvp in lhs_series.dimensionsStorage.storage)
                                            {
                                                kvp.Key.parent = lhs_series;
                                                (kvp.Value as Series).mmi.parent = lhs_series;
                                            }
                                            removeFirst = lhs != null;
                                            //lhs_series = clone;
                                            //AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, clone);
                                            //G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                                            LookupHelperLeftside_message(smpl, lhs_series.freq, varnameWithFreq);
                                        }
                                        break;
                                    default:
                                        {
                                            G.Writeln2("*** ERROR: Expected SERIES to be 1 of 4 types");
                                            throw new GekkoException();
                                        }
                                        break;
                                }
                            }
                            break;
                        case EVariableType.Val:
                            {
                                //---------------------------------------------------------
                                // x = VAL
                                //---------------------------------------------------------       
                                // stuff below also handles array-timeseries just fine                     
                                double d = ((ScalarVal)rhs).val;
                                //bool create = CreateSeriesIfNotExisting(varnameWithFreq, freq, ref lhs_series);

                                if (operatorType == ESeriesUpdTypes.none || operatorType == ESeriesUpdTypes.n)
                                {
                                    //this is very similar to the same code regarding 1 x 1 MATRIX
                                    if (O.UseFlexFreq(smpl.t1, lhs_series.freq))
                                    {
                                        //different freqs, for instance x!q = 2 when global freq is !a                                        
                                        foreach (GekkoTime t in smpl.Iterate12(lhs_series.freq)) lhs_series.SetData(t, d);                                        
                                    }
                                    else
                                    {
                                        //same freq
                                        foreach (GekkoTime t in smpl.Iterate12()) lhs_series.SetData(t, d);
                                    }
                                }
                                else
                                {
                                    OperatorHelperScalar(smpl, lhs_series, operatorType, d);
                                }

                                //G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                                LookupHelperLeftside_message(smpl, lhs_series.freq, varnameWithFreq);
                                
                            }
                            break;
                        
                        case EVariableType.Date:
                            {
                                //---------------------------------------------------------
                                // x = DATE
                                //---------------------------------------------------------
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        case EVariableType.String:
                            {
                                //---------------------------------------------------------
                                // x = STRING
                                //---------------------------------------------------------
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType, 1);                                    
                                }
                            }
                            break;
                        case EVariableType.List:
                            {
                                //---------------------------------------------------------
                                // x = LIST
                                //---------------------------------------------------------
                                // stuff below also handles array-timeseries just fine 

                                List rhs_list = rhs as List;

                                HelperListdata(smpl, lhs_series, operatorType, rhs_list);

                                //G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                                LookupHelperLeftside_message(smpl, lhs_series.freq, varnameWithFreq);
                            }
                            break;
                        case EVariableType.Map:
                            {
                                //---------------------------------------------------------
                                // x = MAP
                                //---------------------------------------------------------
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        case EVariableType.Matrix:
                            {
                                //---------------------------------------------------------
                                // x = MATRIX
                                //---------------------------------------------------------

                                // stuff below also handles array-timeseries just fine     

                                Matrix rhs_matrix = rhs as Matrix;

                                if (rhs_matrix.data.Length == 1)
                                {
                                    double d = rhs.ConvertToVal();  //will fail with error if not 1x1                            

                                    if (operatorType == ESeriesUpdTypes.none || operatorType == ESeriesUpdTypes.n)
                                    {
                                        //this is very similar to the same code regarding VAL
                                        if (O.UseFlexFreq(smpl.t1, lhs_series.freq))
                                        {
                                            //different freqs, for instance x!q = 2 when global freq is !a                                        
                                            foreach (GekkoTime t in smpl.Iterate12(lhs_series.freq)) lhs_series.SetData(t, d);                                            
                                        }
                                        else
                                        {
                                            //same freq
                                            foreach (GekkoTime t in smpl.Iterate12()) lhs_series.SetData(t, d);
                                        }

                                    }
                                    else
                                    {
                                        OperatorHelperScalar(smpl, lhs_series, operatorType, d);
                                    }
                                }
                                else
                                {                                    
                                    GekkoTime t1 = smpl.t1;
                                    GekkoTime t2 = smpl.t2;
                                    if (O.UseFlexFreq(t1, lhs_series.freq)) O.Helper_Convert12(smpl, lhs_series.freq, out t1, out t2);
                                    int n = GekkoTime.Observations(t1, t2);

                                    if (n != rhs_matrix.data.GetLength(0) || 1 != rhs_matrix.data.GetLength(1))
                                    {
                                        G.Writeln2("*** ERROR: Expected " + n + " x 1 matrix, got " + rhs_matrix.data.GetLength(0) + " x " + rhs_matrix.data.GetLength(1));
                                        throw new GekkoException();
                                    }

                                    if (operatorType == ESeriesUpdTypes.none || operatorType == ESeriesUpdTypes.n)
                                    {
                                        for (int i = 0; i < rhs_matrix.data.GetLength(0); i++)
                                        {
                                            lhs_series.SetData(t1.Add(i), rhs_matrix.data[i, 0]);
                                        }
                                    }
                                    else
                                    {
                                        //rhs_matrix.data[i, 0]

                                        //int offset = 1;                                    
                                        double[] rhsData = new double[n + Globals.smplOffset];
                                        for (int i = 0; i < n; i++)
                                        {
                                            rhsData[i + Globals.smplOffset] = rhs_matrix.data[i, 0];
                                        }
                                        for (int i = 0; i < Globals.smplOffset; i++)
                                        {
                                            //just safety, probably not necessary
                                            rhsData[i] = double.NaN;
                                        }
                                        OperatorHelperSequence(smpl, lhs_series, rhsData, operatorType);
                                    }
                                }
                                
                                LookupHelperLeftside_message(smpl, lhs_series.freq, varnameWithFreq);

                            }
                            break;
                        default:
                            {
                                G.Writeln2("*** ERROR: Expected IVariable to be 1 of 7 types");
                                throw new GekkoException();
                            }
                            break;
                    }  //end switch

                    if (create)
                    {
                        AddIvariableWithOverwrite(ib, varnameWithFreq, removeFirst, lhs_series);
                    }
                    else
                    {
                        //nothing to do, either already existing in bank/map or array-subseries
                    }

                    if (keep)
                    {
                        GekkoTime tLast = lhs_series.GetRealDataPeriodLast();

                        GekkoTime t3 = smpl.t3; //why t3 and not t2? Never mind, t2 and t3 are equal most of the time
                        if (O.UseFlexFreq(t3, lhs_series.freq)) t3 = GekkoTime.ConvertFreqsLast(lhs_series.freq, t3);

                        foreach (GekkoTime t in new GekkoTimeIterator(t3.Add(1), tLast)) 
                        {
                            //runs after the <...> period or globals period until data ends
                            //so the updates outside of sample.
                            double rel = original.GetData(smpl, t) / original.GetData(smpl, t.Add(-1));
                            lhs_series.SetData(t, lhs_series.GetData(smpl, t.Add(-1)) * rel);
                        }
                    }

                    if (Program.options.series_failsafe)
                    {
                        //only for debugging                        
                        ReportSeriesMissingValue(lhs_series, smpl.t1, smpl.t2);
                    }
                }
            }

            return;

        }        

        private static void LookupHelperLeftside_message(GekkoSmpl smpl, EFreq lhs_series_freq, string varnameWithFreq)
        {
            string s;
            GekkoTime t1 = smpl.t1;
            GekkoTime t2 = smpl.t2;
            if (O.UseFlexFreq(t1, lhs_series_freq)) O.Helper_Convert12(smpl, lhs_series_freq, out t1, out t2);
            s = t1 + "-" + t2;
            G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + s + " ", smpl.p);            
        }

        public static void ReportSeriesMissingValue(Series lhs_series, GekkoTime t1, GekkoTime t2)
        {
            foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
            {
                if (G.isNumericalError(lhs_series.GetDataSimple(t)))
                {
                    G.Writeln2("*** ERROR: Missing value encountered in series " + lhs_series.GetNameAndFreqPretty(true) + ", period: " + t.ToString());
                    if (!t1.IsSamePeriod(t2))
                    {
                        G.Writeln("Values:");
                        foreach (GekkoTime tt in new GekkoTimeIterator(t1, t2))
                        {
                            G.Writeln(tt.ToString() + "    " + G.levelFormat(lhs_series.GetDataSimple(tt), 20));
                        }
                    }
                    G.Writeln();
                    G.Writeln("+++ NOTE: To ignore such errors: set OPTION series failsafe = no;");
                    G.Writeln();
                    throw new GekkoException();
                }
            }
        }

        public static void WriteListFile(string varnameWithFreq, IVariable rhs)
        {
            //see also #98037532985

            string file = varnameWithFreq.Substring((Globals.symbolCollection + Globals.listfile + "___").Length);
            //List<string> temp = Program.GetListOfStringsFromList(rhs);
            
            file = Program.AddExtension(file, "." + "lst");
            string pathAndFilename = Program.CreateFullPathAndFileNameFromFolder(file, null);

            List rhs_list = rhs as List;
            if (rhs_list == null)
            {
                G.Writeln2("*** ERROR: Listfile writing: expected list input");
                throw new GekkoException();
            }


            using (FileStream fs = Program.WaitForFileStream(pathAndFilename, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter res = G.GekkoStreamWriter(fs))
            {
                //This is quite strict, only names like a38 will be interpreted as 
                //simple tokens that can avoid quotes. Any 007 or 1e5 string will
                //fail here, even though they would be read as strings. Never mind.

                bool allSimpleStringTokens = true;
                foreach (IVariable iv in rhs_list.list)
                {
                    if (iv.Type() == EVariableType.List)
                    {
                        foreach (IVariable sub in (iv as List).list)
                        {
                            if (sub.Type() == EVariableType.String && G.IsSimpleToken(O.ConvertToString(sub)))
                            {
                                //ok
                            }
                            else
                            {
                                allSimpleStringTokens = false;
                                break;
                            }
                        }
                    }
                    else if (iv.Type() == EVariableType.String && G.IsSimpleToken(O.ConvertToString(iv)))
                    {
                        //ok
                    }
                    else
                    {
                        allSimpleStringTokens = false;
                        break;
                    }
                    if (allSimpleStringTokens == false) break;
                }

                foreach (IVariable iv in rhs_list.list)
                {
                    if (iv.Type() == EVariableType.List)
                    {
                        foreach (IVariable sub in (iv as List).list)
                        {
                            if (sub.Type() == EVariableType.String)
                            {
                                if (allSimpleStringTokens) res.Write(sub.ConvertToString() + "; ");
                                else res.Write("'" + sub.ConvertToString() + "'; ");
                            }
                            else if (sub.Type() == EVariableType.Date)
                            {
                                res.Write("'" + sub.ConvertToDate(GetDateChoices.Strict) + "'; ");
                            }
                            else if (sub.Type() == EVariableType.Val)
                            {
                                res.Write(sub.ConvertToVal() + "; ");
                            }
                            else
                            {
                                G.Writeln2("*** ERROR: Expected sub-list elements to be string, date, or val");
                                throw new GekkoException();
                            }
                        }
                    }
                    else if (iv.Type() == EVariableType.String)
                    {
                        if (allSimpleStringTokens) res.Write(iv.ConvertToString());
                        else res.Write("'" + iv.ConvertToString() + "'");
                    }
                    else if (iv.Type() == EVariableType.Date)
                    {
                        res.Write("'" + iv.ConvertToDate(GetDateChoices.Strict) + "'");
                    }
                    else if (iv.Type() == EVariableType.Val)
                    {
                        res.Write(iv.ConvertToVal());
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Expected list elements to be string, date, val, or list");
                        throw new GekkoException();
                    }
                    res.WriteLine();

                }
                res.Flush();
                res.Close();
            }

            string listfileName = G.TransformListfileName(varnameWithFreq);

            G.ServiceMessage("LIST " + listfileName + " updated ", null);
        }

        public static bool CheckForDynamicSeries(IVariable i1, IVariable i2)
        {
            return i1.Type() == EVariableType.Series && !G.Chop_HasSigil(i2.ConvertToString());
        }

        public static void RunAssigmentMaybeDynamic(GekkoSmpl smpl, Action assign_20, Func<bool> check_20, O.Assignment o)
        {

            MissingMemory missing = null;
            if (o.opt_missing != null)
            {
                missing = new MissingMemory();
                missing.print = Program.options.series_array_print_missing;
                missing.calc = Program.options.series_array_calc_missing;
                missing.data = Program.options.series_data_missing;
                HandleMissing1(o.opt_missing);
            }

            try
            {
                //check_20() just checks if the RHS is a timeseries or not. If not, there is no point
                //in looping over periods anyway.
                //
                if (CheckDyn1(o) && check_20())  //o.opt_dyn can be null
                {
                    GekkoTime tt1_20 = smpl.t1;
                    GekkoTime tt2_20 = smpl.t2;
                    foreach (GekkoTime t_20 in new GekkoTimeIterator(smpl.t1, smpl.t2))
                    {
                        smpl.t1 = t_20;
                        smpl.t2 = t_20;
                        assign_20();
                    }
                    smpl.t1 = tt1_20;
                    smpl.t2 = tt2_20;
                }
                else
                {
                    assign_20();
                }
            }
            finally
            {
                if (o.opt_missing != null)
                {
                    missing.print = Program.options.series_array_print_missing;
                    missing.calc = Program.options.series_array_calc_missing;
                    missing.data = Program.options.series_data_missing;
                    HandleMissing2(missing.print, missing.calc, missing.data);
                }
            }
        }

        //See also CheckDyn2()
        private static bool CheckDyn1(Assignment o)
        {
            //If this is true, and there are series on both sides of '=',
            //the assignment will be run period for period
            //It is true if either inside BLOCK series dyn = yes, or if using <dyn> local option.
            return Program.options.series_dyn == true || G.Equal(o.opt_dyn, "yes");
        }

        //See also CheckDyn1()
        private static bool CheckDyn2(Assignment o)
        {
            //If Program.options.series_dyn_check = true, and the condition below is true,
            //Gekko will fail with an error if it meets lagged endogenous like x = x[-1] + 1.
            //The condition is true if there is no BLOCK series dyn = yes|no set and there
            //is no <dyn> or <dyn = yes|no> set.
            //So BLOCK series dyn = yes|no or any <dyn> or <dyn = yes|no> will trigger condition = false.
            //Therefore the condition is true if nothing has been done regarding dynamics.
            return Program.options.series_dyn == null && (o == null || o.opt_dyn == null);
        }

        private static void HelperListdata(GekkoSmpl smpl, Series lhs_series, ESeriesUpdTypes operatorType, List rhs_list)
        {
            GekkoTime t1 = smpl.t1;
            GekkoTime t2 = smpl.t2;
            if (O.UseFlexFreq(t1, lhs_series.freq)) O.Helper_Convert12(smpl, lhs_series.freq, out t1, out t2);

            bool lastElementStar = false;
            IVariable last = rhs_list.list[rhs_list.list.Count - 1];
            ScalarVal last_val = last as ScalarVal;
            if (last_val != null)
            {
                lastElementStar = last_val.hasRepStar;
            }

            int n = GekkoTime.Observations(t1, t2);

            if (rhs_list.list.Count < n)
            {
                //lacking elements
                if (!lastElementStar)
                {
                    G.Writeln2("*** ERROR: Expected " + n + " list items, got " + rhs_list.list.Count);
                    throw new GekkoException(); 
                }
            }
            else if (rhs_list.list.Count > n)
            {
                G.Writeln2("*** ERROR: Expected " + n + " list items, got " + rhs_list.list.Count);
                throw new GekkoException();
            }

            //int offset = 1;
            double[] rhs_data = new double[n + Globals.smplOffset];
            for (int i = 0; i < rhs_list.list.Count; i++)
            {
                rhs_data[i + Globals.smplOffset] = rhs_list.list[i].ConvertToVal();
            }
            for (int i = 0; i < Globals.smplOffset; i++)
            {
                rhs_data[i] = double.NaN;
            }

            if (rhs_list.list.Count < n)
            {
                //then lastElementStar = true
                for (int i = rhs_list.list.Count; i < n; i++)
                {
                    rhs_data[i + Globals.smplOffset] = rhs_list.list[rhs_list.list.Count - 1].ConvertToVal();
                }
            }

            if (operatorType == ESeriesUpdTypes.none || operatorType == ESeriesUpdTypes.n)
            {
                for (int i = 0; i < n; i++)
                {
                    lhs_series.SetData(t1.Add(i), rhs_data[i + Globals.smplOffset]);
                }
            }
            else
            {
                OperatorHelperSequence(smpl, lhs_series, rhs_data, operatorType);
            }
        }

        // =====================================================================
        //                   Operator helper start
        // =====================================================================

        private static void OperatorHelperSeries(GekkoSmpl smpl, Series lhs_series, Series rhs_series, ESeriesUpdTypes operatorType)
        {
            double[] rhsData, lhsData, lhsDataOriginal; //int offset = 1;
            OperatorHelper1(smpl, lhs_series, rhs_series, double.NaN, out lhsData, out lhsDataOriginal, out rhsData);
            OperatorHelper2(smpl, lhs_series.freq, operatorType, lhsData, lhsDataOriginal, rhsData);
            lhs_series.SetDataSequence(smpl.t1, smpl.t2, lhsData, Globals.smplOffset);
        }

        private static void OperatorHelperSequence(GekkoSmpl smpl, Series lhs_series, double[] rhsData, ESeriesUpdTypes operatorType)
        {
            //rhsData must include offset at first position(s), so if offset = 1, rhsData[0] must be = NaN.
            //int offset = 1;
            double[] lhsData = null, lhsDataOriginal = null;
            OperatorHelper1a(smpl, lhs_series, out lhsData, out lhsDataOriginal);
            OperatorHelper2(smpl, lhs_series.freq, operatorType, lhsData, lhsDataOriginal, rhsData);
            GekkoTime t1 = smpl.t1;
            GekkoTime t2 = smpl.t2;
            if (O.UseFlexFreq(t1, lhs_series.freq)) O.Helper_Convert12(smpl, lhs_series.freq, out t1, out t2);
            lhs_series.SetDataSequence(t1, t2, lhsData, Globals.smplOffset);
        }

        private static void OperatorHelperScalar(GekkoSmpl smpl, Series lhs_series, ESeriesUpdTypes operatorType, double d)
        {
            double[] rhsData, lhsData, lhsDataOriginal; //int offset = 1;
            OperatorHelper1(smpl, lhs_series, null, d, out lhsData, out lhsDataOriginal, out rhsData);
            OperatorHelper2(smpl, lhs_series.freq, operatorType, lhsData, lhsDataOriginal, rhsData);

            GekkoTime t1 = smpl.t1;
            GekkoTime t2 = smpl.t2;
            if (O.UseFlexFreq(t1, lhs_series.freq)) O.Helper_Convert12(smpl, lhs_series.freq, out t1, out t2);            

            lhs_series.SetDataSequence(t1, t2, lhsData, Globals.smplOffset);
        }

        private static void OperatorHelper1(GekkoSmpl smpl, Series lhs_series, Series rhs_series, double rhs_scalar, out double[] lhsData, out double[] lhsDataOriginal, out double[] rhsData)
        {
            GekkoTime t1 = smpl.t1;
            GekkoTime t2 = smpl.t2;
            if (O.UseFlexFreq(t1, lhs_series.freq)) O.Helper_Convert12(smpl, lhs_series.freq, out t1, out t2);                        

            rhsData = new double[GekkoTime.Observations(t1, t2) + Globals.smplOffset];
            lhsDataOriginal = new double[GekkoTime.Observations(t1, t2) + Globals.smplOffset];
            lhsData = new double[GekkoTime.Observations(t1, t2) + Globals.smplOffset];

            int i = 0;
            foreach (GekkoTime t in new GekkoTimeIterator(t1.Add(-Globals.smplOffset), t2))
            {
                //slack: could be array-copy
                if (rhs_series == null)
                {
                    rhsData[i] = rhs_scalar;
                }
                else
                {
                    rhsData[i] = rhs_series.GetData(smpl, t);  //rhs_series could be a light series
                }
                lhsDataOriginal[i] = lhs_series.GetDataSimple(t);  //simple, because it cannot be expression on LHS
                lhsData[i] = lhsDataOriginal[i];
                i++;
            }
        }

        private static void OperatorHelper1a(GekkoSmpl smpl, Series lhs_series, out double[] lhsData, out double[] lhsDataOriginal)
        {            
            GekkoTime t1 = smpl.t1;
            GekkoTime t2 = smpl.t2;
            if (O.UseFlexFreq(t1, lhs_series.freq)) O.Helper_Convert12(smpl, lhs_series.freq, out t1, out t2);            

            lhsDataOriginal = new double[GekkoTime.Observations(t1, t2) + Globals.smplOffset];
            lhsData = new double[GekkoTime.Observations(t1, t2) + Globals.smplOffset];
         
            int i = 0;
            foreach (GekkoTime t in new GekkoTimeIterator(t1.Add(-Globals.smplOffset), t2))
            {                
                lhsDataOriginal[i] = lhs_series.GetDataSimple(t);  //lhs_series cannot be light (expression)
                lhsData[i] = lhsDataOriginal[i];
                i++;
            }
        }

        private static void OperatorHelper2(GekkoSmpl smpl, EFreq lhs_series_freq, ESeriesUpdTypes operatorType, double[] lhsData, double[] lhsDataOriginal, double[] rhsData)
        {            
            GekkoTime t1 = smpl.t1;
            GekkoTime t2 = smpl.t2;
            if (O.UseFlexFreq(t1, lhs_series_freq)) O.Helper_Convert12(smpl, lhs_series_freq, out t1, out t2);            

            int i = Globals.smplOffset;  //offset = 2
            foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
            {
                double d = double.NaN;
                if (operatorType == ESeriesUpdTypes.m)  //+
                {
                    lhsData[i] += rhsData[i];
                }
                else if (operatorType == ESeriesUpdTypes.d)  //+
                {
                    lhsData[i] = lhsData[i - 1] + rhsData[i];
                }
                else if (operatorType == ESeriesUpdTypes.q)  //*
                {
                    lhsData[i] *= 1 + rhsData[i] / 100d;
                }
                else if (operatorType == ESeriesUpdTypes.p)  //%
                {
                    lhsData[i] = lhsData[i - 1] * (1 + rhsData[i] / 100d);
                }
                else if (operatorType == ESeriesUpdTypes.mp)  //%
                {
                    lhsData[i] = lhsData[i - 1] * (lhsDataOriginal[i] / lhsDataOriginal[i - 1] + rhsData[i] / 100d);
                }
                else if (operatorType == ESeriesUpdTypes.dl)  //dlog(x) = ...
                {
                    //dlog(y) = x --> log(y) = log(y.1) + x --> y = y.1 * exp(x)
                    lhsData[i] = lhsData[i - 1] * Math.Exp(rhsData[i]);
                }
                else if (operatorType == ESeriesUpdTypes.l)  //log(x) = ...
                {                    
                    lhsData[i] = Math.Exp(rhsData[i]);
                }
                i++;
            }

            return;
        }

        public static void Helper_Convert03(GekkoSmpl smpl, EFreq desiredFreq, out GekkoTime t0, out GekkoTime t3)
        {
            //1 of 3
            //This method is special compared to Helper_Convert12. The thing is that Helper_Convert03 handles the lag problem,
            //and we have to deal with flexible freqs here.
            //This is just to keep the fleible freq stuff assembled in one place
            //Flexible freq stuff is for instance x!a <2001q2 2003q3> = 1, 2, 3;
            //If there are problems with flexible freqs, these methods can be used for tracking
            //See also O.FlexFreq() and Helper_Convert12()
            GekkoTime.Convert03(smpl, desiredFreq, out t0, out t3);
        }

        public static void Helper_Convert12(GekkoSmpl smpl, EFreq desiredFreq, out GekkoTime t1, out GekkoTime t2)
        {
            //2 of 3
            //This is just to keep the fleible freq stuff assembled in one place
            //Flexible freq stuff is for instance x!a <2001q2 2003q3> = 1, 2, 3;
            //If there are problems with flexible freqs, these methods can be used for tracking
            //See also O.FlexFreq() and Helper_Convert03()
            GekkoTime.Convert12(smpl, desiredFreq, out t1, out t2);
        }

        public static bool UseFlexFreq(GekkoTime gt, EFreq freq)
        {
            //3 of 3
            //This is just to keep the fleible freq stuff assembled in one place
            //See also Helper_Convert12() and Helper_Convert03()
            return gt.freq != freq;
        }

        // =====================================================================
        //                   Operator helper end
        // =====================================================================

        private static ESeriesUpdTypes GetOperatorType(Assignment options)
        {
            if (options == null) return ESeriesUpdTypes.none;  //will this ever happen?
            ESeriesUpdTypes operatorType = ESeriesUpdTypes.none;
            if (G.Equal(options.opt_d, "yes")) operatorType = ESeriesUpdTypes.d;
            else if (G.Equal(options.opt_p, "yes")) operatorType = ESeriesUpdTypes.p;
            else if (G.Equal(options.opt_m, "yes")) operatorType = ESeriesUpdTypes.m;
            else if (G.Equal(options.opt_q, "yes")) operatorType = ESeriesUpdTypes.q;
            else if (G.Equal(options.opt_mp, "yes")) operatorType = ESeriesUpdTypes.mp;
            else if (G.Equal(options.opt_n, "yes")) operatorType = ESeriesUpdTypes.n;
            else if (G.Equal(options.opt_l, "yes")) operatorType = ESeriesUpdTypes.l;
            else if (G.Equal(options.opt_dl, "yes")) operatorType = ESeriesUpdTypes.dl;

            if (options.opt_lsfunc != null)
            {
                if (operatorType != ESeriesUpdTypes.none)
                {
                    G.Writeln2("*** ERROR: You cannot use a left-side function ('" + options.opt_lsfunc + "') together with operators");
                    throw new GekkoException();
                }
                if (G.Equal(options.opt_lsfunc, "dif") || G.Equal(options.opt_lsfunc, "diff"))
                {
                    operatorType = ESeriesUpdTypes.d;
                }
                else if (G.Equal(options.opt_lsfunc, "pch"))
                {
                    operatorType = ESeriesUpdTypes.p;
                }
                else if (G.Equal(options.opt_lsfunc, "log"))
                {
                    operatorType = ESeriesUpdTypes.l;
                }
                else if (G.Equal(options.opt_lsfunc, "dlog"))
                {
                    operatorType = ESeriesUpdTypes.dl;
                }
                else
                {
                    G.Writeln2("*** ERROR: Left-side function ('" + options.opt_lsfunc + "') is not implemented");
                    throw new GekkoException();
                }
            }
            return operatorType;
        }

        public static void AdjustT0(GekkoSmpl smpl, int i)
        {
            smpl.t0 = smpl.t0.Add(i);            
        }

        private static void ReportTypeError(string varnameWithFreq, IVariable rhs, EVariableType type)
        {
            ReportTypeError(varnameWithFreq, rhs, type, 0);
        }

        private static void ReportTypeError(string varnameWithFreq, IVariable rhs, EVariableType type, int extra)
        {
            G.Writeln2("*** ERROR: " + type.ToString().ToUpper() + " " + varnameWithFreq + " has a " + rhs.Type().ToString().ToUpper() + " on right-hand side");
            if (extra == 1)
            {
                G.Writeln(Globals.stringConversionNote);
            }
            throw new GekkoException();
        }

        private static bool CreateSeriesIfNotExisting(string varnameWithFreq, string freq, ref Series lhs_series)
        {
            bool create = false;
            if (lhs_series != null && (lhs_series.type == ESeriesType.Normal || lhs_series.type == ESeriesType.Timeless))
            {
                //do nothing, use it
            }
            else
            {
                //create it
                create = true;
                lhs_series = new Series(ESeriesType.Normal, G.GetFreq(freq, true), varnameWithFreq);
            }

            return create;
        }

        private static void AddIvariableWithOverwrite(IBank ib, string varnameWithFreq, bool removeFirstBeforeAdding, IVariable lhsNew)
        {
            if (removeFirstBeforeAdding) ib.RemoveIVariable(varnameWithFreq);
            ib.AddIVariable(varnameWithFreq, lhsNew);
        }

        private static void LookupTypeCheck(IVariable rhs, string varName)
        {
            if (varName[0] == Globals.symbolScalar)
            {
                //VAL, STRING, DATE                        
                if (rhs.Type() != EVariableType.Val && rhs.Type() != EVariableType.String && rhs.Type() != EVariableType.Date)
                {
                    G.Writeln2("*** ERROR: A %-variable cannot be of type " + rhs.Type().ToString().ToUpper());
                    throw new GekkoException();
                }
            }
            else if (varName[0] == Globals.symbolCollection)
            {
                //LIST, DICT, MATRIX                        
                if (rhs.Type() != EVariableType.Matrix && rhs.Type() != EVariableType.List && rhs.Type() != EVariableType.Map)
                {
                    G.Writeln2("*** ERROR: A #-variable cannot be of type " + rhs.Type().ToString().ToUpper());
                    throw new GekkoException();
                }
            }
            else
            {
                if (rhs.Type() != EVariableType.Series && rhs.Type() != EVariableType.Val)
                {
                    //TODO: rhs as MATRIX (vector) should be possible if sample fits.
                    G.Writeln2("*** ERROR: Could not convert right-hand side (" + rhs.Type().ToString() + ") to SERIES");
                    throw new GekkoException();
                }
            }
        }

        public static void Print(GekkoSmpl smpl, IVariable x)
        {
            //Program.OPrint(smpl, x);
        }

        public static IVariable AddSpecial(GekkoSmpl smpl, IVariable x1, IVariable x2, bool minus)
        {
            bool specialAdd = false;
            if (x1.Type() == EVariableType.String && x2.Type() == EVariableType.Val)
            {
                //This would normally produce an error, but we allow it for array-series with
                //elements that are integers stored as strings. For instance x['30']. In that case,
                //running a loop over #i = ('30', '31') is possible via x[#i], but here we allow 
                //x[#i+1] or x[#i-1] instead of the more tedious x[(#i.val()+1).string()].
                string x1_string = ((ScalarString)x1).string2;
                bool x1IsInteger = false;
                int i1 = 0;
                if (int.TryParse(x1_string, out i1))
                {
                    x1IsInteger = true;
                }

                double x2_val = ((ScalarVal)x2).val;
                bool x2IsInteger = false;
                int i2 = 0;

                if (G.ConvertToInt(out i2, x2_val))
                {
                    x2IsInteger = true;
                }

                if (x1IsInteger && x2IsInteger)
                {
                    int ii = i1 + i2;
                    if (minus) ii = i1 - i2;
                    return new ScalarString(ii.ToString());
                }
            }
            if (minus)
            {
                return Subtract(smpl, x1, x2);
            }
            else
            {
                return Add(smpl, x1, x2);
            }

        }


        public static void ChopIndexer(string s, out string name, out string rest)
        {
            name = s.Trim();
            string[] ss = s.Split('[');
            rest = null;
            if (ss.Length > 1)
            {
                name = ss[0].Trim();
                rest = "[" + ss[1].Trim();
            }
        }

        public static void ChopFreq(string input2, ref string freq, ref string varName)
        {
            if (input2 == null) return;
            string input = input2.Trim();

            string[] ss2 = input.Split(Globals.freqIndicator);
            if (ss2.Length > 2)
            {
                G.Writeln2("*** ERROR: More than 1 freq indicators ('!') in '" + input + "'");
                throw new GekkoException();
            }
            else if (ss2.Length == 2)
            {
                varName = ss2[0].Trim();
                freq = ss2[1].Trim();
            }
            else if (ss2.Length == 1)
            {
                varName = input;
                freq = null;
            }
            return;
        }

        //See also Chop()
        public static string UnChop(string bank, string name, string freq, string[] index)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                G.Writeln2("*** ERROR: Name cannot be null");
                throw new GekkoException();
            }
            string s = name.Trim();
            if (bank != null) s = bank.Trim() + Globals.symbolBankColon + s;
            if (freq != null) s = s + Globals.freqIndicator + freq;
            if (index != null && index.Length > 0)
            {
                s += "[";
                s += G.GetListWithCommas(index);
                s += "]";
            }
            return s;
        }

        public static void AdjustSmplForDecomp(GekkoSmpl smpl, int i)
        {           
            
            int add2a = O.MaxLag();
            int add2b = O.MaxLead();

            //add2a = 0; //seems ok, but check with large lag/lead to see if really ok!   <---- HMM why should this work??
            //add2b = 0; //seems ok, but check with large lag/lead to see if really ok!

            if (i == 0)
            {
                smpl.t0 = smpl.t0.Add(-add2a + Globals.decompPerLag);
                smpl.t1 = smpl.t1.Add(-add2a + Globals.decompPerLag);
                smpl.t2 = smpl.t2.Add(add2b);
                smpl.t3 = smpl.t3.Add(add2b);
            }
            else
            {
                smpl.t0 = smpl.t0.Add(add2a - Globals.decompPerLag);
                smpl.t1 = smpl.t1.Add(add2a - Globals.decompPerLag);
                smpl.t2 = smpl.t2.Add(-add2b);
                smpl.t3 = smpl.t3.Add(-add2b);
            }
        }


        //See also UnChop()
        public static void Chop(string input2, out string dbName, out string varName, out string freq, out string[] indexes)
        {
            indexes = null;

            string input, rest;
            ChopIndexer(input2, out input, out rest);

            if (rest != null)
            {
                if (!(rest.StartsWith("[") && rest.EndsWith("]")))
                {
                    G.Writeln2("*** ERROR: Expected indexer to start with '[' and end with ']'");
                    throw new GekkoException();
                }
                indexes = rest.Substring(1, rest.Length - 2).Split(',');
                for (int i = 0; i < indexes.Length; i++)
                {
                    indexes[i] = indexes[i].Trim();
                }
                for (int i = 0; i < indexes.Length; i++)
                {
                    indexes[i] = G.StripQuotes(indexes[i]);  //probably not relevant now, but may be later on
                }
            }

            //When it returns, all returned strings are guaranteed not to contain colon or !.
            string[] ss = input.Split(Globals.symbolBankColon2);
            if (ss.Length > 2)
            {
                G.Writeln2("*** ERROR: More than 1 colons (':') in '" + input + "'");
                throw new GekkoException();
            }
            dbName = null;
            varName = null;

            if (ss.Length == 1) varName = ss[0].Trim();
            else if (ss.Length == 2)
            {
                dbName = ss[0]; varName = ss[1].Trim();
            }
            //firstChar = varName[0];

            //if (dbName != null && (dbName.StartsWith(Globals.symbolScalar.ToString()) || dbName.StartsWith(Globals.symbolCollection.ToString())))
            //{
            //    G.Writeln2("*** ERROR: Bankname '" + dbName + "' should not contain symbols '%' or '#'");
            //    throw new GekkoException();
            //}

            freq = null;
            O.ChopFreq(varName, ref freq, ref varName);
            //varName = DecorateWithTilde(varName, freq);
        }

        public static void HandleIndexerHelper(int depth, IVariable y, params IVariable[] x)
        {
            if (depth >= x.Length)
            {
                //HandleIndexerHelper2()
            }
            List m = (List)x[depth];
            foreach (IVariable iv in m.list)
            {
                string s = O.ConvertToString(iv);
                G.Writeln2(depth + " " + s);
                HandleIndexerHelper(depth + 1, y, x);
            }
        }

        public static void DollarIndexerSetData(IVariable logical, GekkoSmpl smpl, IVariable x, IVariable y, O.Assignment options, params IVariable[] indexes)
        {
            //Only encountered on the LHS
            if (logical == null)
            {
                x.IndexerSetData(smpl, y, options, indexes);
            }
            if (logical.Type() == EVariableType.Val)
            {
                if (IsTrue(((ScalarVal)logical).val))
                {
                    x.IndexerSetData(smpl, y, options, indexes);
                }
                else
                {
                    //skip it!
                }
            }
            else if (logical.Type() == EVariableType.Series)
            {
                //This deviates a bit from GAMS: when logical is 0 here, a 0 will also be set for the LHS, it is not just skipped.
                //See also #6238454
                IVariable z = Conditional1Of3(smpl, y, logical);
                x.IndexerSetData(smpl, z, options, indexes);
            }
            else
            {
                DollarLHSError();
                return;
            }
        }

        private static void DollarLHSError()
        {
            G.Writeln2("*** ERROR: $-conditional on left-hand side only supports value or series type");
            throw new GekkoException();
        }

        public static void IndexerSetData(GekkoSmpl smpl, IVariable x, IVariable y, O.Assignment options, params IVariable[] indexes)
        {
            x.IndexerSetData(smpl, y, options, indexes);
        }

        public static GekkoSmpl2 Smpl(GekkoSmpl smpl, int i)
        {
            GekkoSmpl2 smplRemember = null;
            smplRemember = new GekkoSmpl2();
            smplRemember.t0 = smpl.t0;
            smplRemember.t3 = smpl.t3;
            smpl.t0 = smpl.t0.Add(i);
            return smplRemember;
        }

        public static GekkoSmpl2 Smpl(GekkoSmpl smpl, IVariable i)
        {
            GekkoSmpl2 smplRemember = null;
            smplRemember = new GekkoSmpl2();
            smplRemember.t0 = smpl.t0;
            smplRemember.t3 = smpl.t3;
            smpl.t0 = smpl.t0.Add(-O.ConvertToInt(i));
            return smplRemember;
        }

        //See Indexer() below
        public static GekkoSmpl2 Indexer2(GekkoSmpl smpl, O.EIndexerType indexerType, params IVariable[] indexes)
        {
            GekkoSmpl2 smplRemember = null;
            int i = -12345; GekkoTime t = GekkoTime.tNull;
            Series.FindLagLeadOrFixedPeriod(ref i, ref t, indexes);
            if (i != -12345)
            {
                smplRemember = new GekkoSmpl2();
                smplRemember.t0 = smpl.t0;
                smplRemember.t3 = smpl.t3;

                if (indexerType == O.EIndexerType.IndexerLag || indexerType == O.EIndexerType.IndexerLead)
                {
                    smpl.t0 = smpl.t0.Add(i);
                    smpl.t3 = smpl.t3.Add(i);
                }
                else if (indexerType == O.EIndexerType.Dot)
                {
                    smpl.t0 = smpl.t0.Add(-i);
                    smpl.t3 = smpl.t3.Add(-i);
                }
                else
                {
                    smpl.t0 = new GekkoTime(EFreq.A, i, 1);
                    smpl.t3 = new GekkoTime(EFreq.A, i, 1);
                }
            }
            else if (!t.IsNull())
            {
                smplRemember = new GekkoSmpl2();
                smplRemember.t0 = smpl.t0;
                smplRemember.t3 = smpl.t3;

                smpl.t0 = t;
                smpl.t3 = t;
            }
            return smplRemember;
        }

        //See Indexer2() above. The first argument should be Indexer2(smpl, indexes)
        public static IVariable Indexer(GekkoSmpl2 smplRemember, GekkoSmpl smpl, O.EIndexerType indexerType, IVariable x, params IVariable[] indexes)
        {
            Program.RevertSmpl(smplRemember, smpl);
                        
            //x[y]
            //a[1] or #a['q*']
            //#x[1, 2]                 
            //x['nz', 'w']    
            //x[-1] or x[+1]
            
            IVariable rv = x.Indexer(smpl, indexerType, indexes);
            return rv;
        }

        public static IVariable RangeGeneral(IVariable x1, IVariable x2)
        {
            Range r = new Range(x1, x2);
            List<IVariable> temp = new List<IVariable>();
            temp.Add(r);
            List m = new List(temp);
            List<string> mm = Program.Search(m, null, EVariableType.Var);
            
            return new List(mm);
        }       

        public static IVariable ReportLabel(GekkoSmpl smpl, IVariable x, string s)
        {
            smpl.labelRecordedPieces.Add(new RecordedPieces(s, x));
            return x;
        }

        public static List<List<LabelHelperIVariable>> AddLabelHelper2(GekkoSmpl smpl)
        {
            List<List<LabelHelperIVariable>> rv = null;
            //if(smpl.labelHelper2!=null && smpl.labelHelper2.Count>0)
            //{
            //    rv = smpl.labelHelper2;
            //}
            //else
            //{
            //    rv = new List<List<LabelHelperIVariable>>();
            //    //rv.Add(smpl.labelHelper);
            //}
            rv = new List<List<LabelHelperIVariable>>();
            return rv;
        }

        public static IVariable HandleString(ScalarString x)
        {
            //if (!x.string2.Contains("~") & !x.string2.Contains("\""))
            if (!x.string2.Contains("~"))
            {
                //fast, covers most cases
                return x;
            }
            string s = x.string2;
            s = s.Replace("~'", "'");
            s = s.Replace("~{", "{");
            //s = s.Replace("~~", "~");
            //s = s.Replace("\"", "\"\"");
            return new ScalarString(s);  //costs a bit of time, but only if the string contains ~ or ".
        }

        //========================================
        //======================================== Z() variants start
        //========================================

        //public static IVariable ZScalar(string name)
        //{
        //    IVariable a = null;
        //    if (Program.scalars.TryGetValue(name, out a))
        //    {

        //        //VAL y = %x; <-- %x is a VAL
        //        //expected to be of VAL or DATE type (dates can have periods added/subtracted)
        //    }
        //    else
        //    {
        //        G.Writeln2("*** ERROR: Memory variable '" + Globals.symbolScalar + name + "' was not found");
        //        throw new GekkoException();
        //    }
        //    return a;
        //}

        //public static IVariable ZListFile(string fileName)
        //{
        //    fileName = Program.AddExtension(fileName, "." + "lst");
        //    List<string> folders = new List<string>();
        //    string fileNameTemp = Program.FindFile(fileName, folders);
        //    if (fileNameTemp == null)
        //    {
        //        G.Writeln2("*** ERROR: Listfile " + fileName + " could not be found");
        //        throw new GekkoException();
        //    }
        //    string listFile = Program.GetTextFromFileWithWait(fileNameTemp);
        //    List<string> input = G.ExtractLinesFromText(listFile);

        //    List<string> rhs = new List<string>();
        //    List<string> result = new List<string>();

        //    GetRawListElements(fileName, input, result);
        //    if (result.Count == 1 && G.Equal(result[0], "null"))
        //    {
        //        //LIST mylist = null; ---> empty list
        //        result = new List<string>();
        //    }
        //    List ml = new List(result);
        //    ml.isNameList = true;
        //    return ml;
        //}

        private static void GetRawListElementsOLD(string fileName, List<string> input, List<string> result)
        {
            //quoted elements are not allowed, too
            //also out-commented items, and items with minus are ok
            int counter = 0;
            foreach (string ss in input)
            {
                counter++;
                string s = ss.Trim();  //will also remove any newline characters!

                bool quotes = false;
                string s2 = G.StripQuotes(s);
                if (s2.Length != s.Length)
                {
                    s = s2;
                    quotes = true;
                }

                if (fileName != null && s == "")
                {
                    G.Writeln2("*** ERROR in listfile '" + fileName + "': empty line [" + counter + "]");
                    G.Writeln("    Note: you may use comments (//), but not completely empty lines. This is to");
                    G.Writeln("    keep the list files reasonably tidy.");
                    throw new GekkoException();
                }

                //if (s.StartsWith("//")) continue;  //allow comments, /* ... */ not supported though
                int idx = s.IndexOf("//");
                if (idx >= 0)
                {
                    s = s.Substring(0, idx);
                    s = s.Trim();
                }

                if (s == null || s == "")
                {
                    if (fileName == null)
                    {
                        G.Writeln2("*** ERROR in <direct> list: empty element");
                        throw new GekkoException();
                    }
                    else
                    {
                        continue;  //ignore a comment
                    }
                }

                if (quotes == false)  //with quotes, anything is accepted
                {

                    int colonCounter = 0;
                    for (int i = 0; i < s.Length; i++)
                    {
                        char c = s[i];
                        if (i == 0 && (c == '-'))  //starting with # not considered ok, only simple elements (perhaps with minus) allowed
                        {
                            //ok
                        }
                        else
                        {
                            if (G.IsLetterOrDigitOrUnderscore(c))
                            {
                                //ok
                            }
                            else if (c.ToString() == Globals.symbolBankColon)
                            {
                                colonCounter++;
                                if (colonCounter > 1)
                                {
                                    //probably very rare, but we check here
                                    G.Writeln2("*** ERROR in list: at most 1 colon allowed: '" + s + "'");
                                    throw new GekkoException();
                                }
                            }
                            else
                            {
                                if (fileName == null)
                                {
                                    G.Writeln2("*** ERROR in <direct> list, item = '" + s + "'");
                                    G.Writeln("    Items should only contain numbers, digits, '_', ':' (or start with '-', or be enclosed in quotes).", Color.Red);
                                    throw new GekkoException();
                                }
                                else
                                {
                                    G.Writeln2("*** ERROR in listfile '" + fileName + "', line [" + counter + "], item = '" + s + "'");
                                    G.Writeln("    Items should only contain numbers, digits, '_', ':' (or start with '-', or be enclosed in quotes).", Color.Red);
                                    throw new GekkoException();
                                }
                            }
                        }
                    }
                }
                result.Add(s);
            }
            return;
        }

        private static List GetRawListElements(string fileName)
        {
            //see also #98037532985

            //Naked list in Gekko:
            //
            // 12, 02 --> '12', '02', not 12, 2
            // 12, 1e5 --> '12', '1e5'
            // 0<digits> is not value. But 0 is value.
            // And <int>E<int> is not value. But 2.0e5 isa value.
            //
            // Only tricky thing is that a list containg 007 or 1e5 will 
            // be transformed into strings, not values. Especially regarding 
            // 1e5 this can be confusing. But then the .vals() function can be used.

            TableLight table = Program.ReadCsvPrn(EDataFormat.Csv, fileName);
            
            int iMax = table.GetRowMaxNumber();
            int jMax = table.GetColMaxNumber();
            
            List temp = new List();
            for (int i = 1; i <= iMax; i++)
            {
                for (int j = 1; j <= jMax; j++)
                {
                    CellLight cell = table.Get(i, j);
                    if (cell.type == ECellLightType.String)
                    {                        
                        temp.Add(new ScalarString(cell.text));                        
                    }
                    else if (cell.type == ECellLightType.Double)
                    {
                        temp.Add(new ScalarVal(cell.data));
                    }
                    else if (cell.type == ECellLightType.None)
                    {
                        //skip    
                    }
                }
            }
            bool interpretAsNumbers = IsListAllNumbers(temp, 1);

            List m = new List();
            for (int i = 1; i <= iMax; i++)
            {

                List m2 = new List();

                bool emptyFound = false;
                bool problem = false;
                for (int j = 1; j <= jMax; j++)
                {
                    
                    CellLight cell = table.Get(i, j);
                    if (cell.type == ECellLightType.String)
                    {
                        if (emptyFound) problem = true;

                        if (cell.hasQuotes)
                        {
                            m2.Add(new ScalarString(cell.text));  //always a string, even if it looks like a value
                        }
                        else
                        {
                            if(interpretAsNumbers)
                            {
                                double d;
                                if (G.TryParseIntoDouble(cell.text, out d))
                                {
                                    m2.Add(new ScalarVal(d));
                                }
                                else
                                {
                                    G.Writeln2("*** ERROR: Internal error #8977436372887324");
                                    throw new GekkoException();
                                }
                            }
                            else
                            {
                                m2.Add(new ScalarString(cell.text));
                            }
                            
                        }
                    }
                    else if (cell.type == ECellLightType.Double)
                    {
                        if (emptyFound) problem = true;
                        m2.Add(new ScalarVal(cell.data));
                    }
                    else if (cell.type == ECellLightType.None)
                    {
                        //skip                                      
                        emptyFound = true;          
                    }                    
                }
                if (problem)
                {
                    G.Writeln2("*** ERROR: Empty cell found in middle of row elements");
                    throw new GekkoException();
                }
                if (m2.Count() == 0)
                {
                    //skip
                }
                else if (m2.Count() == 1)
                {
                    m.Add(m2.list[0]);
                }
                else
                {
                    m.Add(m2);
                }
            }
            return m;                       
        }

        public static List<string> GetListOfStringsFromIVariable(IVariable x)
        {
            if (x.Type() == EVariableType.String)
            {
                return new List<string>() { x.ConvertToString() };
            }
            else if (x.Type() == EVariableType.List)
            {
                return Program.GetListOfStringsFromList(x);
            }
            else
            {
                G.Writeln2("*** Expected string of list of strings");
                throw new GekkoException();
            }
        }


        private static IVariable MaybeStringify(IVariable x, bool dollarStringify)
        {
            IVariable rv = null;
            if (dollarStringify)
            {
                if (x.Type() == EVariableType.String)
                {
                    ScalarString ss = (ScalarString)x;
                    ScalarString ss2 = new ScalarString(ss.string2, false);
                    rv = ss2;
                }
                else if (x.Type() == EVariableType.List)
                {
                    List ml = (List)x;
                    List ml2 = new List(ml.list);
                    ml2.isNameList = false;
                    rv = ml2;
                }
            }
            else rv = x;
            return rv;
        }

        public static List<string> AddBankToListItems(List<string> input, string bank)
        {
            for (int i = 0; i < input.Count; i++)
            {
                input[i] = bank + ":" + input[i];
            }
            return input;
        }

        public static int ForListMax(List<List<IVariable>> x)
        {
            int n = -1;
            foreach (List<IVariable> m in x)
            {
                if (n == -1) n = m.Count;
                else
                {
                    if (n != m.Count)
                    {
                        string s = null;
                        foreach (List<IVariable> mm in x)
                        {
                            s += mm.Count + ", ";
                        }
                        s = s.Substring(0, s.Length - 2);
                        G.Writeln2("*** ERROR: Parallel FOR loop with different number of items.");
                        G.Writeln("           Number of items: " + s + ".");
                        throw new GekkoException();
                    }
                }
            }
            return n;
        }

        //see also GetScalarFromCache()
        //public static void RemoveScalar(string originalName)
        //{
        //    //If a pointer also needs to be cleared, put that as second argument (else null)
        //    if (Program.scalars.ContainsKey(originalName))
        //    {
        //        Program.scalars.Remove(originalName);
        //    }
        //    //if (a != null) a = null;
        //}      


        public static List GetList(IVariable a)
        {
            if (a == null)
            {
                G.Writeln2("*** ERROR: Could not find variable");
                throw new GekkoException();
            }
            if (a.Type() != EVariableType.List)
            {
                G.Writeln2("*** ERROR: List of strings expected");
                throw new GekkoException();
            }
            return (List)a;
        }        

        public static IVariable GetListWithBankPrefix(IVariable x, IVariable y, int bankNumber)
        {
            string bankName = O.ConvertToString(x);
            List<string> items = Program.GetListOfStringsFromList(y);
            List<string> newList = new List<string>();
            foreach (string s in items)
            {
                newList.Add(bankName + ":" + s);
            }
            return new List(newList);
        }

        // ========================================================================================
        // ========================================================================================
        // ============== Logical operations start ================================================
        // ========================================================================================
        // ========================================================================================

        private static IVariable Helper_LogicalAndOr(GekkoSmpl smpl, IVariable x1, IVariable x2, bool and)
        {
            //same logic in Equals(), StrictlyLargerThan() etc.
            //hmm, comparing two 1x1 matrices will fail            
            IVariable rv = Globals.scalarVal0;
            if ((x1.Type() == EVariableType.Val || O.IsTimelessSeries(x1)) && (x2.Type() == EVariableType.Val || O.IsTimelessSeries(x2)))
            {
                //must return a VAL, therefore special treatment
                if (and)
                {
                    if (O.ConvertToVal(x1) == 1d && O.ConvertToVal(x2) == 1d) rv = Globals.scalarVal1;
                    else rv = Globals.scalarVal0;
                }
                else
                {
                    if (O.ConvertToVal(x1) == 1d || O.ConvertToVal(x2) == 1d) rv = Globals.scalarVal1;
                    else rv = Globals.scalarVal0;
                }
            }
            else if (x1.Type() == EVariableType.Series || x2.Type() == EVariableType.Series)
            {
                CheckFreq(x1, x2);  //checks freqs
                Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                rv = rv_series;
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    //if x or y does not have frequency corresponding to t, we will get an error here
                    if (and)
                    {
                        if (x1.GetVal(t) == 1d && x2.GetVal(t) == 1d) rv_series.SetData(t, 1d);
                        else rv_series.SetData(t, 0d);  //else it would be missing
                    }
                    else
                    {
                        if (x1.GetVal(t) == 1d || x2.GetVal(t) == 1d) rv_series.SetData(t, 1d);
                        else rv_series.SetData(t, 0d);  //else it would be missing
                    }

                }
            }
            else
            {
                string x = "or";
                if (and) x = "and";
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types " + G.GetTypeString(x1) + " and " + G.GetTypeString(x2) + " do not match for " + x.ToUpper() + " logical compare");
                throw new GekkoException();
            }
            return rv;
        }

        public static IVariable LogicalAnd(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            return Helper_LogicalAndOr(smpl, x1, x2, true);
        }

        public static IVariable LogicalOr(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            return Helper_LogicalAndOr(smpl, x1, x2, false);
        }

        public static IVariable LogicalNot(GekkoSmpl smpl, IVariable x1)
        {
            IVariable rv = Globals.scalarVal0;
            if ((x1.Type() == EVariableType.Val || O.IsTimelessSeries(x1)))
            {
                //must return a VAL, therefore special treatment                
                if (O.ConvertToVal(x1) == 1d) rv = Globals.scalarVal0;
                else rv = Globals.scalarVal1;
            }
            else if (x1.Type() == EVariableType.Series)
            {
                Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                rv = rv_series;
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    //if x or y does not have frequency corresponding to t, we will get an error here
                    if (x1.GetVal(t) == 1d) rv_series.SetData(t, 0d);
                    else rv_series.SetData(t, 1d);  //else it would be missing                    
                }
            }
            else
            {
                G.Writeln2("*** ERROR: Variable typs " + G.GetTypeString(x1) + " cannot be used with logical NOT");
                throw new GekkoException();
            }
            return rv;
        }

        // =========================================================================
        // =========================================================================
        // ============ conditional logic start ====================================
        // =========================================================================
        // =========================================================================
        
        public static IVariable Conditional1Of3(GekkoSmpl smpl, IVariable x, IVariable logical)
        {
            //Code located here to keep all conditional code in one place 
            //logical is 1 for true, and false otherwise
            IVariable rv = null;
            if (x.Type() == EVariableType.Series)
            {
                Series x_series = x as Series;
                Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                rv = rv_series;
                if (logical.Type() == EVariableType.Series)
                {
                    Series logical_series = logical as Series;
                    foreach (GekkoTime t in smpl.Iterate03())
                    {
                        if (IsTrue(logical_series.GetData(smpl, t)))
                        {
                            rv_series.SetData(t, x_series.GetData(smpl, t));
                        }
                        else
                        {
                            rv_series.SetData(t, 0d);
                        }
                    }
                }
                else if (logical.Type() == EVariableType.Val)
                {
                    ScalarVal logical_val = logical as ScalarVal;
                    //LIGHTFIXME, could be array copy
                    foreach (GekkoTime t in smpl.Iterate03())
                    {
                        if (IsTrue(logical_val.val)) rv_series.SetData(t, x_series.GetData(smpl, t));
                        else rv_series.SetData(t, 0d);
                    }
                }
                else
                {
                    G.Writeln2("*** ERROR: You cannot use the type " + x.Type().ToString().ToUpper() + " on right side in $-conditional");
                    throw new GekkoException();
                }
            }
            else if (x.Type() == EVariableType.Val)
            {
                if (logical.Type() == EVariableType.Series)
                {
                    //we have to convert the VAL to a SERIES here
                    ScalarVal x_val = x as ScalarVal;
                    Series logical_series = logical as Series;
                    Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                    rv = rv_series;
                    foreach (GekkoTime t in smpl.Iterate03())
                    {
                        if (IsTrue(logical_series.GetData(smpl, t))) rv_series.SetData(t, x_val.val);
                        else rv_series.SetData(t, 0d);
                    }
                }
                else if (logical.Type() == EVariableType.Val)
                {
                    ScalarVal logical_val = logical as ScalarVal;
                    if (IsTrue(logical_val.val))
                    {
                        rv = new ScalarVal(((ScalarVal)x).val);
                    }
                    else
                    {
                        rv = Globals.scalarVal0;
                    }
                }
                else
                {
                    G.Writeln2("*** ERROR: You cannot use the type " + x.Type().ToString().ToUpper() + " on right side in $-conditional");
                    throw new GekkoException();
                }
            }
            else
            {
                G.Writeln2("*** ERROR: You cannot use the type " + x.Type().ToString().ToUpper() + " on left side in $-conditional");
                throw new GekkoException();
            }
            return rv;
        }

        public static double Conditional2Of3(GekkoSmpl smpl, IVariable tmp)
        {
            //Code located here to keep all conditional code in one place 
            double v = double.NaN;
            if (tmp.Type() == EVariableType.Series && (tmp as Series).type != ESeriesType.Timeless)
            {
                if (Globals.holesFix)
                {
                    double v2 = 0d;
                    Series tmp_series = tmp as Series;
                    foreach (GekkoTime t in smpl.Iterate12())
                    {
                        double v3 = tmp_series.GetDataSimple(t);
                        if (v3 == 1d)
                        {
                            v = v3;
                            break;
                        }
                    }
                }
                else
                {
                    G.Writeln2("*** ERROR: $-conditional returns a (non-timeless) series, not a scalar.");
                    G.Writeln("    Time-varying logical conditions are not implemented in Gekko yet", Color.Red);
                    throw new GekkoException();
                }
            }
            else
            {
                v = tmp.ConvertToVal();
            }
            return v;
        }

        public static string Conditional3Of3(string code, string vName)
        {
            //Code located here to keep all conditional code in one place 
            //return "ScalarVal " + vName + " = " + code + " as ScalarVal" + ";" + G.NL + "if (" + vName + " != null && (" + vName + " as ScalarVal).val == 0d) continue" + ";";
            return "double " + vName + " = O.Conditional2Of3(" + Globals.smpl + ", " + code + ");" + G.NL + "if (" + vName + " != 1d) continue" + ";";
        }

        // =========================================================================
        // =========================================================================
        // ============ conditional logic end ======================================
        // =========================================================================
        // =========================================================================

        public static void UseOldIf(bool b)
        {
            Globals.if_old_helper = b;
        }

        public static bool IsTrue(double d)
        {
            if (d != 0d) return true;
            else return false;
        }

        public static IVariable Equals(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            //same logic in LogicalOr() and LogicalAnd()
            //hmm, comparing two 1x1 matrices will fail
            IVariable rv = Globals.scalarVal0;
            if ((x.Type() == EVariableType.Val || O.IsTimelessSeries(x)) && (y.Type() == EVariableType.Val || O.IsTimelessSeries(y)))
            {
                //must return a VAL, not a SERIES                
                double d1 = x.ConvertToVal(); double d2 = y.ConvertToVal();
                bool b = G.Equals(d1, d2);
                if(b) rv = Globals.scalarVal1;
            }
            else if (x.Type() == EVariableType.Series || y.Type() == EVariableType.Series)
            {
                CheckFreq(x, y);  //checks freqs
                Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                rv = rv_series;
                foreach (GekkoTime t in smpl.Iterate03())
                {

                    // ---------------------------------------------
                    if (Program.options.bugfix_missing == false || Globals.if_old_helper)
                    {
                        if (x.GetVal(t) == y.GetVal(t)) rv_series.SetData(t, 1d);
                        else rv_series.SetData(t, 0d);  //else it would be missing                                                
                    }
                    else
                    {
                        if (G.Equals(x.GetVal(t), y.GetVal(t)) != (x.GetVal(t) == y.GetVal(t)))
                        {
                            MissingProblem(smpl.p);
                        }
                        if (G.Equals(x.GetVal(t), y.GetVal(t))) rv_series.SetData(t, 1d);
                        else rv_series.SetData(t, 0d);
                    }
                }
            }
            else if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (O.ConvertToDate(x).IsSamePeriod(O.ConvertToDate(y))) rv = Globals.scalarVal1;
            }
            else if (x.Type() == EVariableType.String && y.Type() == EVariableType.String)
            {
                if (G.Equal(x.ConvertToString(), y.ConvertToString())) rv = Globals.scalarVal1;
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types " + G.GetTypeString(x) + " and " + G.GetTypeString(y) + " do not match for '==' compare");
                throw new GekkoException();
            }
            return rv;
        }

        private static void MissingProblem(P p)
        {
            //G.Writeln2("+++ WARNING: missing problem " + p.lastFileSentToANTLR + " ");
            int lineNumber; string originalFileName; List<string> commandLines;
            Program.GetErrorLineAndText(p, p.GetDepth(), out lineNumber, out originalFileName, out commandLines);
            string ss = null;
            try
            {
                ss = commandLines[lineNumber - 1];
            }
            catch { }
            string s = G.ReplaceGlueNew(ss) + "  --->   " + originalFileName + ", line " + lineNumber;
            if (!Globals.bugfixMissing2.ContainsKey(s))
            {
                Globals.bugfixMissing1.Add(s);
                Globals.bugfixMissing2.Add(s, null);
            }

            //throw new GekkoException();
        }

        public static IVariable NonEquals(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            //hmm, comparing two 1x1 matrices will fail
            IVariable rv = Globals.scalarVal0;            
            if ((x.Type() == EVariableType.Val || O.IsTimelessSeries(x)) && (y.Type() == EVariableType.Val || O.IsTimelessSeries(y)))
            {
                //must return a VAL, not a SERIES                
                double d1 = x.ConvertToVal(); double d2 = y.ConvertToVal();
                bool b = G.Equals(d1, d2);
                if (!b) rv = Globals.scalarVal1;
            }
            else if (x.Type() == EVariableType.Series || y.Type() == EVariableType.Series)
            {
                CheckFreq(x, y);  //checks freqs
                Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                rv = rv_series;
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    //if x or y does not have frequency corresponding to t, we will get an error here
                    if (x.GetVal(t) != y.GetVal(t)) rv_series.SetData(t, 1d);
                    else rv_series.SetData(t, 0d);  //else it would be missing                                                         
                    // ---------------------------------------------
                    if (Program.options.bugfix_missing == false || Globals.if_old_helper)
                    {
                        if (x.GetVal(t) != y.GetVal(t)) rv_series.SetData(t, 1d);
                        else rv_series.SetData(t, 0d);  //else it would be missing  
                    }
                    else
                    {                        
                        if (!G.Equals(x.GetVal(t), y.GetVal(t)) != (x.GetVal(t) != y.GetVal(t)))
                        {
                            MissingProblem(smpl.p);
                        }
                        if (!G.Equals(x.GetVal(t), y.GetVal(t))) rv_series.SetData(t, 1d);
                        else rv_series.SetData(t, 0d);
                    }
                }
            }
            else if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (!O.ConvertToDate(x).IsSamePeriod(O.ConvertToDate(y))) rv = Globals.scalarVal1;
            }
            else if (x.Type() == EVariableType.String && y.Type() == EVariableType.String)
            {
                if (!G.Equal(x.ConvertToString(), y.ConvertToString())) rv = Globals.scalarVal1;
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types " + G.GetTypeString(x) + " and " + G.GetTypeString(y) + " do not match for '<>' compare");
                throw new GekkoException();
            }
            return rv;
        }

        public static IVariable StrictlySmallerThan(GekkoSmpl smpl, IVariable x, IVariable y)
        {            
            //hmm, comparing two 1x1 matrices will fail
            IVariable rv = Globals.scalarVal0;
            if ((x.Type() == EVariableType.Val || O.IsTimelessSeries(x)) && (y.Type() == EVariableType.Val || O.IsTimelessSeries(y)))
            {
                //must return a VAL, not a SERIES
                double d1 = x.ConvertToVal(); double d2 = y.ConvertToVal();                
                if (d1 < d2) rv = Globals.scalarVal1; ;                
            }
            else if (x.Type() == EVariableType.Series || y.Type() == EVariableType.Series)
            {
                CheckFreq(x, y);  //checks freqs
                Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                rv = rv_series;
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    //if x or y does not have frequency corresponding to t, we will get an error here
                    if (x.GetVal(t) < y.GetVal(t)) rv_series.SetData(t, 1d);
                    else rv_series.SetData(t, 0d);  //else it would be missing
                }
            }
            else if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {                
                if (O.ConvertToDate(x).StrictlySmallerThan(O.ConvertToDate(y))) rv = Globals.scalarVal1;
            }            
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types " + G.GetTypeString(x) + " and " + G.GetTypeString(y) + " do not match for '<' compare");
                throw new GekkoException();
            }
            return rv;                        
        }

        public static IVariable SmallerThanOrEqual(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            //hmm, comparing two 1x1 matrices will fail
            IVariable rv = Globals.scalarVal0;
            if ((x.Type() == EVariableType.Val || O.IsTimelessSeries(x)) && (y.Type() == EVariableType.Val || O.IsTimelessSeries(y)))
            {
                //must return a VAL, not a SERIES
                double d1 = x.ConvertToVal(); double d2 = y.ConvertToVal();
                if (d1 <= d2) rv = Globals.scalarVal1; ;
            }
            else if (x.Type() == EVariableType.Series || y.Type() == EVariableType.Series)
            {
                CheckFreq(x, y);  //checks freqs
                Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                rv = rv_series;
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    //if x or y does not have frequency corresponding to t, we will get an error here
                    if (x.GetVal(t) <= y.GetVal(t)) rv_series.SetData(t, 1d);
                    else rv_series.SetData(t, 0d);  //else it would be missing
                }
            }
            else if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (O.ConvertToDate(x).SmallerThanOrEqual(O.ConvertToDate(y))) rv = Globals.scalarVal1;
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types " + G.GetTypeString(x) + " and " + G.GetTypeString(y) + " do not match for '<=' compare");
                throw new GekkoException();
            }
            return rv;
        }

        public static IVariable LargerThanOrEqual(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            //hmm, comparing two 1x1 matrices will fail
            IVariable rv = Globals.scalarVal0;
            if ((x.Type() == EVariableType.Val || O.IsTimelessSeries(x)) && (y.Type() == EVariableType.Val || O.IsTimelessSeries(y)))
            {
                //must return a VAL, not a SERIES
                double d1 = x.ConvertToVal(); double d2 = y.ConvertToVal();
                if (d1 >= d2) rv = Globals.scalarVal1; ;
            }
            else if (x.Type() == EVariableType.Series || y.Type() == EVariableType.Series)
            {
                CheckFreq(x, y);  //checks freqs
                Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                rv = rv_series;
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    //if x or y does not have frequency corresponding to t, we will get an error here
                    if (x.GetVal(t) >= y.GetVal(t)) rv_series.SetData(t, 1d);
                    else rv_series.SetData(t, 0d);  //else it would be missing
                }
            }
            else if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (O.ConvertToDate(x).LargerThanOrEqual(O.ConvertToDate(y))) rv = Globals.scalarVal1;
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types " + G.GetTypeString(x) + " and " + G.GetTypeString(y) + " do not match for '>=' compare");
                throw new GekkoException();
            }
            return rv;
        }

        public static IVariable StrictlyLargerThan(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            //hmm, comparing two 1x1 matrices will fail
            IVariable rv = Globals.scalarVal0;
            if ((x.Type() == EVariableType.Val || O.IsTimelessSeries(x)) && (y.Type() == EVariableType.Val || O.IsTimelessSeries(y)))
            {
                //must return a VAL, not a SERIES
                double d1 = x.ConvertToVal(); double d2 = y.ConvertToVal();
                if (d1 > d2) rv = Globals.scalarVal1; ;
            }
            else if (x.Type() == EVariableType.Series || y.Type() == EVariableType.Series)
            {
                CheckFreq(x, y);  //checks freqs
                Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                rv = rv_series;
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    //if x or y does not have frequency corresponding to t, we will get an error here
                    if (x.GetVal(t) > y.GetVal(t)) rv_series.SetData(t, 1d);
                    else rv_series.SetData(t, 0d);  //else it would be missing
                }
            }
            else if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (O.ConvertToDate(x).StrictlyLargerThan(O.ConvertToDate(y))) rv = Globals.scalarVal1;
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types " + G.GetTypeString(x) + " and " + G.GetTypeString(y) + " do not match for '>' compare");
                throw new GekkoException();
            }
            return rv;
        }

        public static IVariable In(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return Functions.contains(smpl, null, null, y, x);
        }

        public static bool IsTrue(GekkoSmpl smpl, IVariable x)
        {
            if (x.Type() == EVariableType.Val || O.IsTimelessSeries(x))
            {
                if (IsTrue(x.ConvertToVal())) return true;
            }
            else if (x.Type() == EVariableType.Matrix)
            {
                //is this even possible??
                Matrix m = x as Matrix;
                if (m.data.GetLength(0) == 1 && m.data.GetLength(1) == 1)
                {
                    if (IsTrue(m.data[0, 0])) return true;
                }
            }
            else if (x.Type() == EVariableType.Series)
            {
                Series ts = x as Series;
                bool allOk = true;
                foreach (GekkoTime t in smpl.Iterate12())
                {
                    if (!IsTrue(ts.GetData(smpl, t)))
                    {
                        allOk = false;
                        break;
                    }
                }
                if (allOk) return true;
            }
            else
            {
                G.Writeln2("*** ERROR: Wrong type " + G.GetTypeString(x) + " for IF(...)");
                throw new GekkoException();
            }
            return false;
        }

        // ========================================================================================
        // ========================================================================================
        // ============== Logical operations end ==================================================
        // ========================================================================================
        // ========================================================================================

        private static void CheckFreq(IVariable x, IVariable y)
        {
            EFreq freq = EFreq.A;
            if (x.Type() == EVariableType.Series) freq = ((Series)x).freq;
            else freq = ((Series)y).freq;
            if (x.Type() == EVariableType.Series && y.Type() == EVariableType.Series)
            {
                if (((Series)x).freq != ((Series)y).freq)
                {
                    G.Writeln2("*** ERROR: You cannot logically compare two timeseries with freqs " + G.GetFreqString(((Series)x).freq) + " and " + G.GetFreqString(((Series)y).freq));
                    throw new GekkoException();
                }
            }
        }        

        public static void PrepareUfunction(int number, string name)
        {
            //If the user has defined a procedure MYPROC, and Gekko later implements a MYPROC command,
            //we will get an error here, since Gekko will refuse to load a procedure with that name.
            //This guards agains compatibility issues with new Gekko versions.

            if (number > 13)
            {
                G.Writeln2("*** ERROR: More than 13 user function/procedure arguments is not allowed at the moment.");
                G.Writeln("           You may consider using a MAP argument to work around this restriction.", Color.Red);
                throw new GekkoException();
            }
            if (Globals.gekkoInbuiltFunctions.ContainsKey(name))
            {
                G.Writeln2("*** ERROR: Loading of user function/procedure '" + name + "' failed, since this is also the name of an");
                G.Writeln("           in-built Gekko function. Please use another name.", Color.Red);
                throw new GekkoException();
            }
            foreach (string s in Globals.commandNames)
            {
                if (G.Equal(s, name))
                {
                    G.Writeln2("*** ERROR: Loading of user function/procedure '" + name + "' failed, since this is also the name of an");
                    G.Writeln("           in-built Gekko command. Please use another name.", Color.Red);
                    throw new GekkoException();
                }
            }

            if (true)
            {

                if (number == 0)
                {
                    if (Globals.ufunctionsNew0.ContainsKey(name)) Globals.ufunctionsNew0.Remove(name);
                }
                else if (number == 1)
                {
                    if (Globals.ufunctionsNew1.ContainsKey(name)) Globals.ufunctionsNew1.Remove(name);
                }
                else if (number == 2)
                {
                    if (Globals.ufunctionsNew2.ContainsKey(name)) Globals.ufunctionsNew2.Remove(name);
                }
                else if (number == 3)
                {
                    if (Globals.ufunctionsNew3.ContainsKey(name)) Globals.ufunctionsNew3.Remove(name);
                }
                else if (number == 4)
                {
                    if (Globals.ufunctionsNew4.ContainsKey(name)) Globals.ufunctionsNew4.Remove(name);
                }
                else if (number == 5)
                {
                    if (Globals.ufunctionsNew5.ContainsKey(name)) Globals.ufunctionsNew5.Remove(name);
                }
                else if (number == 6)
                {
                    if (Globals.ufunctionsNew6.ContainsKey(name)) Globals.ufunctionsNew6.Remove(name);
                }
                else if (number == 7)
                {
                    if (Globals.ufunctionsNew7.ContainsKey(name)) Globals.ufunctionsNew7.Remove(name);
                }
                else if (number == 8)
                {
                    if (Globals.ufunctionsNew8.ContainsKey(name)) Globals.ufunctionsNew8.Remove(name);
                }
                else if (number == 9)
                {
                    if (Globals.ufunctionsNew9.ContainsKey(name)) Globals.ufunctionsNew9.Remove(name);
                }
                else if (number == 10)
                {
                    if (Globals.ufunctionsNew10.ContainsKey(name)) Globals.ufunctionsNew10.Remove(name);
                }
                else if (number == 11)
                {
                    if (Globals.ufunctionsNew11.ContainsKey(name)) Globals.ufunctionsNew11.Remove(name);
                }
                else if (number == 12)
                {
                    if (Globals.ufunctionsNew12.ContainsKey(name)) Globals.ufunctionsNew12.Remove(name);
                }
                else if (number == 13)
                {
                    if (Globals.ufunctionsNew13.ContainsKey(name)) Globals.ufunctionsNew13.Remove(name);
                }            }
            
        }

        // USER FUNCTION STUFF START
        // USER FUNCTION STUFF START
        // USER FUNCTION STUFF START
        // USER FUNCTION STUFF START ------------------------------------------------------------------------------------------
        // USER FUNCTION STUFF START
        // USER FUNCTION STUFF START
        // USER FUNCTION STUFF START

        private static void FunctionErrorMessage(string name, int n)
        {
            if (name.StartsWith(Globals.procedure))
            {
                G.Writeln2("*** ERROR: Cannot find procedure '" + name.Substring(Globals.procedure.Length) + "' with " + (n - 2) + " arguments");
            }
            else
            {
                if (name == Globals.stopHelper)
                {
                    G.Writeln2("-------------------------------------------------------------", Color.Red);
                    G.Writeln("------------ The job was stopped by STOP command ------------", Color.Red);
                    G.Writeln("-------------------------------------------------------------", Color.Red);
                    G.Writeln();
                }
                else
                {
                    G.Writeln2("*** ERROR: Cannot find user function '" + name + "()' with " + (n - 2) + " arguments");
                }
            }
        }


        public static string LastText(string s)
        {
            if (s.Contains(Globals.procedure)) s = "PROCEDURE " + s.Replace(Globals.procedure, "");
            else s = "FUNCTION " + s;
            return s;
        }

        public static Func<GekkoSmpl, P, bool, IVariable> FunctionLookupNew0(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, IVariable> rv = null;
            Globals.ufunctionsNew0.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 0);
                throw new GekkoException();
            }
            return rv;
        }


        public static Func<GekkoSmpl, P, bool, GekkoArg, IVariable> FunctionLookupNew1(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew1.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 1);
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, IVariable> FunctionLookupNew2(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew2.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 2);
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, IVariable> FunctionLookupNew3(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew3.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 3);
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> FunctionLookupNew4(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew4.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 4);
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> FunctionLookupNew5(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew5.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 5);
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> FunctionLookupNew6(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew6.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 6);
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> FunctionLookupNew7(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew7.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 7);
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> FunctionLookupNew8(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew8.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 8);
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> FunctionLookupNew9(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew9.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 9);
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> FunctionLookupNew10(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew10.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 10);
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> FunctionLookupNew11(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew11.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 11);
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> FunctionLookupNew12(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew12.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 12);
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> FunctionLookupNew13(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> rv = null;
            Globals.ufunctionsNew13.TryGetValue(name, out rv);
            if (rv == null)
            {
                FunctionErrorMessage(name, 13);
                throw new GekkoException();
            }
            return rv;
        }



        // USER FUNCTION STUFF END
        // USER FUNCTION STUFF END
        // USER FUNCTION STUFF END
        // USER FUNCTION STUFF END -------------------------------------------------------------------------------------------------
        // USER FUNCTION STUFF END
        // USER FUNCTION STUFF END
        // USER FUNCTION STUFF END

        public static Series CreateTimeSeriesFromMatrix(GekkoSmpl smpl, Matrix m)
        {
            if (m.data.GetLength(1) != 1)
            {
                G.Writeln2("*** ERROR: Expected matrix with 1 column");
                throw new GekkoException();
            }
            if (m.data.GetLength(0) < 1)
            {
                G.Writeln2("*** ERROR: Expected > 0 rows in matrix");
                throw new GekkoException();
            }
            int n = GekkoTime.Observations(smpl.t0, smpl.t3);
            if (n != m.data.GetLength(0))
            {
                G.Writeln2("*** ERROR: Expected " + n + " rows in matrix");
                throw new GekkoException();
            }
            Series tsl = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
            double[] temp = tsl.GetDataSequenceUnsafePointerReadOnlyBEWARE(); //do not set it to 'Alter', because it then tries to set the series dirty, and it is a light series that cannot be dirty.
            for (int i = 0; i < temp.Length; i++)  //we will not convert NaN in the matrix to 0 in the series
            {
                temp[i] = m.data[i, 0];
            }
            return tsl;
        }

        //public static IVariable ListContains(IVariable x, IVariable y)
        //{

        //    if (x.Type() != EVariableType.List || y.Type() != EVariableType.String)
        //    {
        //        G.Writeln2("*** ERROR: Expected syntax like ... $ #a['b'], with list and string");
        //        throw new GekkoException();
        //    }
        //    List ml = (List)x;
        //    ScalarString ss = (ScalarString)y;

        //    bool b = false;
        //    foreach (IVariable iv in ml.list)
        //    {
        //        string s = O.ConvertToString(iv);
        //        if (G.Equal(ss.string2, s))
        //        {
        //            b = true;
        //            break;
        //        }
        //    }
        //    if (b) return Globals.scalarVal1;
        //    else return Globals.scalarVal0;

        //}

        // =================================== end comparisons ==================================        

        public static string SubstituteScalarsAndLists(string label, bool reportError)
        {
            return label;
        }

        //public static List<string> SearchWildcard(IVariable a)
        //{
        //    string s = O.ConvertToString(a);
        //    List<string> l = new List<string>();
        //    l = Program.MatchWildcardInDatabank(s, Program.databanks.GetFirst());            
        //    return l;
        //}

        //public static Series IndirectionHelper(GekkoSmpl smpl, string variable)
        //{
        //    //In that case, we are inside a GENR/PRT implicit time loop                        
        //    //Code below implicitly calls Program.ExtractBankAndRest and Program.FindOrCreateTimeseries()
        //    //So stuff in banks down the list will be found in data mode
        //    Series ats = O.GetTimeSeries(smpl, variable, 0);
        //    return ats;
        //}

        //public static IVariable GetScalar(string name)
        //{
        //    return GetScalar(name, true);
        //}

        //public static IVariable GetScalar(string name, bool transformationAllowed)
        //{
        //    return GetScalar(name, transformationAllowed, false);
        //}

        //public static IVariable GetScalar(string name, bool transformationAllowed, bool dollarStringify)
        //{
        //    IVariable a = null;
        //    if (Program.scalars.TryGetValue(name, out a))
        //    {
        //        //bool didTransform = false;
        //        //a = MaybeStringify(a, dollarStringify);
        //        //a = MaybeTransform(ref didTransform, a, transformationAllowed);
        //        return a;
        //    }
        //    else
        //    {
        //        G.Writeln2("*** ERROR: Memory variable '" + Globals.symbolScalar + name + "' was not found");
        //        throw new GekkoException();
        //    }
        //}

        public static GekkoTime GetDate(GekkoTime x)
        {
            //used for avgt() or sumt() without period indication
            return x;
        }

        // ------------------------------------------------------------------------------  
        // ------------------------------------------------------------------------------
        // --------------------- converters start ---------------------------------------
        // ------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------

        public static GekkoTime ConvertToDate(IVariable x, GetDateChoices c)
        {
            return x.ConvertToDate(c);
        }

        public static GekkoTime ConvertToDate(IVariable x)
        {
            return ConvertToDate(x, GetDateChoices.Strict);
        }

        public static string ConvertToString(IVariable a)
        {
            return a.ConvertToString();
        }

        public static string ConvertToString(string s)
        {
            return s;
        }
        public static IVariable AlternativeConvertToString(IVariable iv)
        {
            return iv;
        }

        public static List<IVariable> ConvertToList(IVariable a)
        {
            return a.ConvertToList();
        }


        public static Matrix ConvertToMatrix(IVariable a)
        {
            //O.GetListFromCache(
            if (a.Type() != EVariableType.Matrix)
            {
                G.Writeln2("*** ERROR: This variable is not a matrix");
                throw new GekkoException();
            }
            Matrix m = (Matrix)a;
            return m;
        }

        // -------------- series converters start ---------------
                
        public static Series ConvertToSeriesMaybeConstant(GekkoSmpl smpl, IVariable x)
        {
            if (x.Type() == EVariableType.Series)
            {
                return x as Series;
            }
            else
            {
                //try to see if x can be a constant value
                Series tsl = new Series(ESeriesType.Light, smpl.t0, smpl.t3); //will have small dataarray            
                double x_val = O.ConvertToVal(x);
                if (Series.MissingZero() && G.isNumericalError(x_val)) x_val = 0d;
                double[] temp = tsl.GetDataSequenceUnsafePointerAlterBEWARE();
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = x_val; //constant, will issue error if not a value
                }
                return tsl;
            }
        }

        //how relates to ConvertToTimeSeries()??
        public static IVariable ConvertToSeries(IVariable x)
        {
            if (x.Type() == EVariableType.Series) return x;
            else
            {
                G.Writeln2("*** ERROR: Cannot convert " + G.GetTypeString(x) + " into SERIES type");
                throw new GekkoException();
            }
        }

        //how relates to ConvertToSeries()??
        public static IVariable ConvertToTimeSeries(GekkoSmpl smpl, IVariable x)
        {
            if (x.Type() == EVariableType.Series || x.Type() == EVariableType.Val) return x;
            else if (x.Type() == EVariableType.Matrix)
            {
                int n = smpl.Observations12();
                Matrix m = x as Matrix;
                if (m.data.GetLength(0) == 1 && m.data.GetLength(1) == 1)
                {
                    return new ScalarVal(m.data[0, 0]);
                }
                else if (m.data.GetLength(0) == n && m.data.GetLength(1) == 1)
                {
                    Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                    int counter = -1;
                    foreach (GekkoTime t in smpl.Iterate12())
                    {
                        counter++;
                        rv_series.SetData(t, m.data[counter, 0]);  //column vector
                    }
                    return rv_series;
                }
                else if (m.data.GetLength(0) == 1 && m.data.GetLength(1) == n)
                {
                    G.Writeln2("*** ERROR: Please use a column vector to transform MATRIX to SERIES. Cf. the t() transpose function");
                    throw new GekkoException();
                }
                else
                {
                    G.Writeln2("*** ERROR: Cannot convert " + m.data.GetLength(0) + " x " + m.data.GetLength(1) + " MATRIX to " + n + " obs SERIES");
                    throw new GekkoException();
                }
            }
            else if (x.Type() == EVariableType.List)
            {
                int n = smpl.Observations12();
                List m = x as List;
                if (m.list.Count() == 1)
                {
                    ScalarVal mi_val = m.list[0] as ScalarVal;
                    if (mi_val == null)
                    {
                        G.Writeln2("*** ERROR: Expected item 1 in LIST to be VAL type");
                        throw new GekkoException();
                    }
                    return new ScalarVal(mi_val.val);
                }
                else if (m.list.Count() == n)
                {
                    Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                    int counter = -1;
                    foreach (GekkoTime t in smpl.Iterate12())
                    {
                        counter++;
                        ScalarVal mi_val = m.list[counter] as ScalarVal;
                        if (mi_val == null)
                        {
                            G.Writeln2("*** ERROR: Expected item " + (counter + 1) + " in LIST to be VAL type");
                            throw new GekkoException();
                        }
                        rv_series.SetData(t, mi_val.val);
                    }
                    return rv_series;
                }
                else
                {
                    G.Writeln2("*** ERROR: Cannot convert " + m.list.Count() + " LIST elements to " + n + " obs SERIES");
                    throw new GekkoException();
                }

            }
            G.Writeln2("*** ERROR: Cannot convert " + G.GetTypeString(x) + " to SERIES");
            throw new GekkoException();
        }

        // -------------- series converters end ---------------

        public static IVariable ConvertToMap(IVariable x)
        {
            if (x.Type() == EVariableType.Map) return x;
            else
            {
                G.Writeln2("*** ERROR: Cannot convert " + G.GetTypeString(x) + " into MAP type");
                throw new GekkoException();
            }
        }

        public static double ConvertToVal(GekkoTime t, IVariable a)
        {
            return a.GetVal(t);
        }

        public static double ConvertToVal(IVariable a)
        {
            return a.ConvertToVal();
        }

        // ------------------------------------------------------------------------------  
        // ------------------------------------------------------------------------------
        // ------------------------ converters end --------------------------------------
        // ------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------
        private static void AssignmentError(Series x_series, string s)
        {
            if (x_series == null)
            {
                G.Writeln2("*** ERROR: You can only use " + s + " operator on series type");
                throw new GekkoException();
            }
            if (x_series.type != ESeriesType.Normal)
            {
                G.Writeln2("*** ERROR: You can only use " + s + " operator on a normal series type");
                throw new GekkoException();
            }
        }


        

        //public static Matrix GetMatrixFromString(IVariable name)
        //{
        //    string name2 = name.ConvertToString();
        //    IVariable lhs = null;            
        //    if (Program.scalars.TryGetValue(Globals.symbolCollection + name2, out lhs))
        //    {
        //        //Scalar is already existing                
        //        if (lhs.Type() == EVariableType.Matrix)
        //        {
        //            //fine
        //        }
        //        else
        //        {
        //            G.Writeln2("*** ERROR: " + Globals.symbolCollection + name2 + " is not a matrix");
        //            throw new GekkoException();
        //        }
        //    }
        //    else
        //    {
        //        G.Writeln2("*** ERROR: " + Globals.symbolCollection + name2 + " could not be found");
        //        throw new GekkoException();
        //    }
        //    return (Matrix)lhs;
        //}

        public static double[,] MultiplyMatrixScalar(double[,] a, double b, int m, int k)
        {
            double[,] c = new double[m, k];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    c[i, j] = a[i, j] * b;
                }
            }
            return c;
        }

        public static double[,] AddMatrixMatrix(double[,] a, double[,] b, int m, int k)
        {
            double[,] c = new double[m, k];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    c[i, j] = a[i, j] + b[i, j];
                }
            }
            return c;
        }

        public static IVariable[] MatrixRow(params IVariable[] list)
        {
            return list;
        }

        public static IVariable MatrixCol(params IVariable[][] list)
        {
            int[,] dimsR = null;
            int[,] dimsC = null;
            int FirstDim = -12345;
            int SecondDim = -12345;

            try
            {
                FirstDim = list.Length;
                SecondDim = list.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular
                dimsR = new int[FirstDim, SecondDim];
                dimsC = new int[FirstDim, SecondDim];
            }
            catch
            {
                G.Writeln2("*** ERROR: Matrix concatenation source is not rectangular");
                throw new GekkoException();
            }

            if (FirstDim == 1 && SecondDim == 1)
            {
                //An 1x1 matrix, defined like [...].
                IVariable iv = list[0][0];
                if (iv.Type() == EVariableType.String)
                {
                    string s = O.ConvertToString(iv);
                    List<string> mm = Program.Search(new List(new List<string>() { s }), null, EVariableType.Var);
                    return new List(mm);
                }
            }

            //loops over rows
            for (int i = 0; i < FirstDim; i++)
            {
                IVariable[] irow = list[i];
                //loops inside row elements
                for (int j = 0; j < SecondDim; j++)
                {
                    IVariable iv = list[i][j];
                    int rows = -12345;
                    int cols = -12345;
                    if (iv.Type() == EVariableType.Val)
                    {
                        rows = 1;
                        cols = 1;
                    }
                    else if (iv.Type() == EVariableType.Matrix)
                    {
                        rows = ((Matrix)iv).data.GetLength(0);
                        cols = ((Matrix)iv).data.GetLength(1);
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Matrix element is not VAL or MATRIX");
                        throw new GekkoException();
                    }
                    dimsR[i, j] = rows;
                    dimsC[i, j] = cols;
                }
            }

            int allRows = 0;
            int allCols = 0;

            for (int i = 0; i < FirstDim; i++)
            {
                for (int j = 0; j < SecondDim; j++)
                {
                    if (j > 0 && dimsR[i, j - 1] != dimsR[i, j])
                    {
                        G.Writeln2("*** ERROR: Trying to concatenate matrices with " + dimsR[i, j - 1] + " and " + dimsR[i, j] + " rows");
                        throw new GekkoException();
                    }
                    if (i > 0 && dimsC[i - 1, j] != dimsC[i, j])
                    {
                        G.Writeln2("*** ERROR: Trying to concatenate matrices with " + dimsC[i - 1, j] + " and " + dimsC[i, j] + " cols");
                        throw new GekkoException();
                    }
                    if (i == 0) allCols += dimsC[i, j];
                    if (j == 0) allRows += dimsR[i, j];
                }
            }

            double[,] m = new double[allRows, allCols];

            int rowCounter = 0;
            for (int i = 0; i < FirstDim; i++)
            {
                int colCounter = 0;
                for (int j = 0; j < SecondDim; j++)
                {
                    IVariable iv = list[i][j];
                    if (iv.Type() == EVariableType.Val)
                    {
                        m[rowCounter, colCounter] = ((ScalarVal)iv).val;
                    }
                    else if (iv.Type() == EVariableType.Matrix)
                    {
                        double[,] data = ((Matrix)iv).data;
                        for (int ii = 0; ii < data.GetLength(0); ii++)
                        {
                            for (int jj = 0; jj < data.GetLength(1); jj++)
                            {
                                m[rowCounter + ii, colCounter + jj] = data[ii, jj];
                            }
                        }
                    }
                    colCounter += dimsC[0, j];
                }
                rowCounter += dimsR[i, 0];
            }
            Matrix mat = new Matrix();
            mat.data = m;
            return mat;
        }

        //public static void TryNewSmpl(GekkoSmpl smpl)
        //{
        //    smpl.gekkoErrorI++;            
        //    if (smpl.gekkoError.t1Problem > 0)
        //    {
        //        smpl.t0 = smpl.t0.Add(-smpl.gekkoError.t1Problem * smpl.gekkoErrorI);
        //    }
        //    if (smpl.gekkoError.t2Problem > 0)
        //    {
        //        smpl.t3 = smpl.t3.Add(smpl.gekkoError.t2Problem * smpl.gekkoErrorI);
        //    }
        //    smpl.gekkoError = null;  //we try again
        //}

        public static double[,] SubtractMatrixMatrix(double[,] a, double[,] b, int m, int k)  //a - b
        {
            double[,] c = new double[m, k];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    c[i, j] = a[i, j] - b[i, j];
                }
            }
            return c;
        }

        public static int CurrentSubperiods()
        {
            int lag = 1;
            if (Program.options.freq == EFreq.Q) lag = Globals.freqQSubperiods;
            else if (Program.options.freq == EFreq.M) lag = Globals.freqMSubperiods;
            return lag;
        }

        public static IVariable HandleSummations(string type, double[] storage)
        {
            //i1 and i2 are often not used
            double data = double.NaN;
            switch (type)
            {
                case "sum":  //this is the GAMS-like sum function, for instance sum(#i, x[#i]). It does not exist in an avg() version.
                case "movavg":
                case "movsum":
                case "avgt":
                case "sumt":
                    {
                        double sum = 0d;
                        for (int i = 0; i < storage.Length; i++)
                        {
                            sum += storage[i];
                        }
                        if (type == "movavg" || type == "avgt") data = sum / (double)storage.Length;
                        else data = sum;
                    }
                    break;
                case "pch":
                case "pchy":
                    {
                        data = (storage[storage.Length - 1] / storage[0] - 1d) * 100d;
                    }
                    break;
                case "dlog":
                case "dlogy":
                    {
                        data = Math.Log(storage[storage.Length - 1] / storage[0]);
                    }
                    break;
                case "dif":
                case "diff":
                case "dify":
                case "diffy":
                    {
                        data = storage[storage.Length - 1] - storage[0];
                    }
                    break;
                case "lag":
                    {
                        data = storage[0];
                    }
                    break;
                default:
                    {
                        G.Writeln2("*** ERROR: Function " + type + " not recognized as a lag function");
                        throw new GekkoException();
                    }
                    break;

            }
            return new ScalarVal(data);
        }

        public static List<string> GetList(List<string> l)
        {
            return l;
        }

        //public static IVariable IvConvertTo(EVariableType type, IVariable a)
        //{
        //    switch (type)
        //    {
        //        //case EVariableType.Val:
        //        //    {
        //        //        if (a.Type() == EVariableType.Val) return a;
        //        //        else return new ScalarVal(a.ConvertToVal());
        //        //    }
        //        //    break;
        //        //case EVariableType.String:
        //        //    {
        //        //        if (a.Type() == EVariableType.String) return a;
        //        //        else return new ScalarString(a.ConvertToString());
        //        //    }
        //        //    break;
        //        //case EVariableType.Date:
        //        //    {
        //        //        if (a.Type() == EVariableType.Date) return a;
        //        //        else return new ScalarDate(a.ConvertToDate(GetDateChoices.Strict));
        //        //    }
        //        //    break;
        //        //case EVariableType.Series:
        //        //    {
        //        //        if (a.Type() == EVariableType.Series) return a;
        //        //        else
        //        //        {
        //        //            G.Writeln2("*** ERROR: Could not transform " + G.GetTypeString(a) + " into SERIES");
        //        //            throw new GekkoException();
        //        //        }
        //        //    }
        //        //    break;
        //        //case EVariableType.List:
        //        //    {
        //        //        if (a.Type() == EVariableType.List) return a;
        //        //        else return new List(a.ConvertToList());
        //        //    }
        //        //    break;
        //        //case EVariableType.Matrix:
        //        //    {
        //        //        if (a.Type() == EVariableType.Matrix) return a;
        //        //        else
        //        //        {
        //        //            G.Writeln2("*** ERROR: Could not transform " + G.GetTypeString(a) + " into MATRIX");
        //        //            throw new GekkoException();
        //        //        }
        //        //    }
        //        //    break;
        //        case EVariableType.Map:
        //            {
        //                if (a.Type() == EVariableType.Map) return a;
        //                else
        //                {
        //                    G.Writeln2("*** ERROR: Could not transform " + G.GetTypeString(a) + " into MAP");
        //                    throw new GekkoException();
        //                }
        //            }
        //            break;
        //        case EVariableType.Var:
        //            {
        //                return a;
        //            }
        //            break;
        //        default: throw new GekkoException();  //should not be possible
        //    }
        //}        



        //public static void GetVal777(GekkoSmpl smpl, IVariable a, int bankNumber, O.Prt.Element e)  //used in PRT and similar, can accept a list that will show itself as a being an integer with ._isName set.
        //{
        //    G.Writeln2("*** ERROR: Obsolete");
        //    throw new GekkoException();
        //    if (a.Type() == EVariableType.List)
        //    {
        //        List<string> items = O.GetStringList((List)a);
        //        double[] d = new double[items.Count];

        //        if (e.subElements == null)
        //        {
        //            e.subElements = new List<O.Prt.SubElement>();
        //            for (int i = 0; i < items.Count; i++)
        //            {
        //                O.Prt.SubElement opeSub0 = new O.Prt.SubElement();
        //                e.subElements.Add(opeSub0);
        //            }
        //        }                                

        //        for (int i = 0; i < items.Count; i++)
        //        {
        //            string s = items[i];
        //            double dd = O.ConvertToVal(O.GetTimeSeries(smpl, s, bankNumber)); //#875324397                     
        //            if (bankNumber == 1)
        //            {
        //                //if (e.subElements[i].tsWork == null) e.subElements[i].tsWork = new Series(Program.options.freq, null);
        //                //e.subElements[i].tsWork.SetData(t.t1, dd); //uuu
        //            }
        //            else
        //            {
        //                //if (e.subElements[i].tsBase == null) e.subElements[i].tsBase = new Series(Program.options.freq, null);
        //                //e.subElements[i].tsBase.SetData(t.t1, dd); //uuu
        //            }
        //            if (e.subElements[i].label == null) e.subElements[i].label = s;  //this is a bit slow because it gets repeated for each t, but PRT is slow anyways, and it only slows down list-unfolding
        //        }                

        //        return;
        //    }
        //    else
        //    {
        //        if (e.subElements == null)
        //        {
        //            e.subElements = new List<O.Prt.SubElement>();                    
        //            O.Prt.SubElement opeSub0 = new O.Prt.SubElement();
        //            e.subElements.Add(opeSub0);                    
        //        }                                                

        //        double dd = a.GetValOLD(smpl);                

        //        if (bankNumber == 1)
        //        {
        //            //if (e.subElements[0].tsWork == null) e.subElements[0].tsWork = new Series(Program.options.freq, null);
        //            //e.subElements[0].tsWork.SetData(t.t1, dd); //uuu
        //        }
        //        else
        //        {
        //            //if (e.subElements[0].tsBase == null) e.subElements[0].tsBase = new Series(Program.options.freq, null);
        //            //e.subElements[0].tsBase.SetData(t.t1, dd); //uuu
        //        }

        //        //The return value is not used, but we keep it for now...                
        //        return;                
        //    }            
        //}

        public static double GetVal(GekkoSmpl smpl, IVariable a, int bankNumber)  //used in PRT and similar, can accept a list that will show itself as a being an integer with ._isName set.
        {
            return a.GetValOLD(smpl);
        }

        //public static Series GetTimeSeries(IVariable a)
        //{
        //    if (a.Type() == EVariableType.Series)
        //    {
        //        return (Series)a;
        //    }
        //    else if (a.Type() == EVariableType.String)
        //    {
        //        ScalarString ss = (ScalarString)a;
        //        if (false)
        //        {
        //            Series ts = O.FindTimeSeries(ss.string2, 1);
        //            return ts;
        //        }
        //        else
        //        {
        //            G.Writeln2("*** ERROR: Cannot convert STRING into SERIES, use a NAME-scalar or {}-braces");
        //            throw new GekkoException();
        //        }
        //    }
        //    else
        //    {
        //        G.Writeln2("*** ERROR: Cannot convert variable of " + G.GetTypeString(a) + " type into SERIES");
        //        throw new GekkoException();
        //    }
        //}

        // ------------------- type checks start ---------------------------
        // we do type checks as explicit functions since it is faster than using a switch
        // position -1: assignment like STRING %s = 123; where type fails
        // position 0: return value type is wrong, for instance "return 123" in a "function string f(...)"
        // position i > 0: function argument, for instance f(123) in a "function string f(string x)"

        public static IVariable TypeCheck_void(IVariable x, int position)
        {
            return x;
        }

        public static IVariable TypeCheck_series(IVariable x, int position)
        {
            if (x.Type() != EVariableType.Series)
            {
                try
                {
                    x = O.ConvertToSeries(x); //ConvertToTimeSeries() is more complicated
                }
                catch (Exception e)
                {
                    G.Writeln(TypeErrorString(position, "SERIES"));
                    throw;
                }
            }
            else
            {
                //cloning is probably reasonably fast, given data stored in arrays
                if (position > 0 && Program.options.system_clone) x = x.DeepClone(null);
            }
            return x;
        }

        public static IVariable TypeCheck_val(IVariable x, int position)
        {
            if (x.Type() != EVariableType.Val)
            {
                try
                {
                    x = new ScalarVal(O.ConvertToVal(x));
                }
                catch (Exception e)
                {
                    G.Writeln(TypeErrorString(position, "VAL"));
                    throw;
                }
            }
            return x;
        }

        public static IVariable TypeCheck_string(IVariable x, int position)
        {
            if (x.Type() != EVariableType.String)
            {
                try
                {
                    x = new ScalarString(O.ConvertToString(x));
                }
                catch (Exception e)
                {
                    G.Writeln(TypeErrorString(position, "STRING"));
                    throw;
                }
            }
            return x;
        }

        public static IVariable TypeCheck_name(IVariable x, int position)
        {
            if (x == null)
            {
                //hmmm, x == null??
                G.Writeln(TypeErrorString(position, "NAME"));
                throw new GekkoException();
            }
            if (x.Type() != EVariableType.String)
            {
                try
                {
                    x = new ScalarString(O.ConvertToString(x));
                }
                catch (Exception e)
                {
                    G.Writeln(TypeErrorString(position, "NAME"));
                    throw new GekkoException();
                }
            }
            return x;
        }

        public static IVariable TypeCheck_date(GekkoArg ga, GekkoSmpl smpl, int position)
        {
            if (ga == null)
            {
                if (position == 1)
                {
                    return new ScalarDate(smpl.t1);
                }
                else if (position == 2)
                {
                    return new ScalarDate(smpl.t2);
                }
                else
                {
                    G.Writeln2("*** ERROR: Internal error #8073437598232");
                    throw new GekkoException();
                }
            }
            else
            {
                IVariable x = ga.f1(smpl);
                return TypeCheck_date(x, position);
            }
        }

        public static IVariable TypeCheck_date(IVariable x, int position)
        {
            if (x.Type() != EVariableType.Date)
            {
                try
                {
                    x = new ScalarDate(O.ConvertToDate(x));
                    //x= new ScalarDate(x.ConvertToDate(GetDateChoices.Strict)); same same
                }
                catch (Exception e)
                {
                    G.Writeln(TypeErrorString(position, "DATE"));
                    throw;
                }
            }
            return x;
        }

        public static IVariable TypeCheck_list(IVariable x, int position)
        {
            if (x.Type() != EVariableType.List)
            {
                try
                {
                    x = new List(O.ConvertToList(x));
                }
                catch (Exception e)
                {
                    G.Writeln(TypeErrorString(position, "LIST"));
                    throw;
                }
            }
            else
            {
                if (position > 0 && Program.options.system_clone) x = x.DeepClone(null);
            }
            return x;
        }

        public static IVariable TypeCheck_matrix(IVariable x, int position)
        {            
            if (x.Type() != EVariableType.Matrix)
            {
                try
                {
                    x = O.ConvertToMatrix(x);
                }
                catch (Exception e)
                {
                    G.Writeln(TypeErrorString(position, "MATRIX"));
                    throw;
                }
            }
            else
            {
                if (position > 0 && Program.options.system_clone) x = x.DeepClone(null);
            }
            return x;
        }

        private static string TypeErrorString(int position, string type)
        {
            string rv = null;
            if (position == -1)
            {
                rv = "***ERROR: The right-hand side should be " + type + " type";
            }
            else if (position == 0)
            {
                rv = "*** ERROR: The return type should be " + type + " type";
            }
            else if (position == 1)
            {
                rv = "*** ERROR: The start date %t1 in a f(<%t1 %t2>, ...) call should be " + type + " type";
            }
            else if (position == 2)
            {
                rv = "*** ERROR: The end date %t2 in a f(<%t1 %t2>, ...) call should be " + type + " type";
            }
            else  //3 or larger, corresponding to argument 1 and so on. However, using UFCS, position = 3 is the variable before dot, and position = 4, 5, 6... are the first, second etc. arguments inside the parenthesis.
            {
                Action a = () =>
                {
                    Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPage2;
                    O.Cls("output");
                    string txt = "When counting arguments, a function like f(x1, x2, x3) is simple in the sense that x1 is argument #1, x2 is argument #2, and so on. But Gekko supports so-called UFCS (Uniform Function Call Syntax), so the function may be written as x1.f(x2, x3) instead. If written in that way, argument #1 is the variable or expression to the left of the dot (here: x1), whereas argument #2 is the first argument after the left parenthesis (here: x2), and so on. Another thing to keep in mind is that optional time period arguments inside <...> are ignored regarding the argument number count, so in a function call like f(<%t1 %t2>, x1, x2, x3) or equivalently x1.f(<%t1 %t2>, x2, x3), argument #1 is still x1, argument #2 is still x2, and so on.";
                    G.Writeln(txt, ETabs.Output);
                };
                string s = G.GetLinkAction("here", new GekkoAction(EGekkoActionTypes.Unknown, null, a));
                rv = "*** ERROR: Argument #" + (position - 2) + " should be " + type + " type (see more on argument number counting " + s + ")";
            }
            return rv;
        }

        public static IVariable TypeCheck_map(IVariable x, int position)
        {
            if (x.Type() != EVariableType.Map)
            {
                try
                {
                    x = O.ConvertToMap(x);
                }
                catch (Exception e)
                {
                    G.Writeln(TypeErrorString(position, "MAP"));
                    throw;
                }
            }
            else
            {
                if (position > 0 && Program.options.system_clone) x = x.DeepClone(null);
            }
            return x;
        }

        public static IVariable TypeCheck_var(IVariable x, int position)
        {
            //not possible???
            //no cloning done here...
            return x;  //no checks
        }

        // -------------------- type checks end ----------------------------

        public static void Stop(P p)
        {
            Globals.threadIsInProcessOfAborting = true;
            p.hasSeenStopCommand = 1;
            throw new GekkoException();
        }

        public static void StopHelper(GekkoSmpl smpl, P p)
        {
            //Globals.threadIsInProcessOfAborting = true;
            p.hasSeenStopCommand = 1;
            O.FunctionLookupNew2(Globals.stopHelper)(smpl, p, false, null, null);
        }

        public static int ConvertToInt(IVariable a)
        {
            return ConvertToInt(a, true);
        }

        public static int ConvertToInt(IVariable a, bool reportError)
        {
            bool problem = false;
            //GetInt() is really just GetVal() converted to int afterwards.
            if (a.Type() == EVariableType.Series)
            {
                if (reportError)
                {
                    G.Writeln2("*** ERROR: Using GetInt() on timeseries.");
                    G.Writeln("           Did you forget []-brackets to pick out an observation, for instance x[2020]?");
                    throw new GekkoException();
                }
                problem = true;
            }
            double d = ConvertToVal(a);
            int intValue = -12345;
            if (!G.ConvertToInt(out intValue, d))
            {
                if (reportError)
                {
                    G.Writeln2("*** ERROR: Could not convert value '" + d + "' into integer");
                    throw new GekkoException();
                }
                problem = true;
            }
            if (!reportError && problem) intValue = int.MaxValue;  //signals a problem with the conversion
            return intValue;
        }
        
        //Common methods end
        //Common methods end
        //Common methods end

        public class Read
        {
            //also covers IMPORT
            public GekkoTime t1 = GekkoTime.tNull;
            public GekkoTime t2 = GekkoTime.tNull;
            public string fileName = null;
            public string readTo = null;
            public string opt_px = null; //pc-axis
            public string opt_tsd = null;
            public string opt_tsdx = null;
            public string opt_gbk = null;
            public string opt_gdx = null;
            public string opt_gdxopt = null;
            public string opt_tsp = null;
            public string opt_csv = null;
            public string opt_prn = null;
            public string opt_pcim = null;
            public string opt_xls = null;
            public string opt_xlsx = null;
            public string opt_merge = null;
            public string opt_cols = null;
            public string opt_prim = null;  //obsolete
            public string opt_first = null;
            public string opt_ref = null;
            public string opt_respect = null;
            public string opt_array = null;
            public string opt_flat = null;
            public string opt_aremos = null;
            public string opt_cell = null;
            public string opt_datecell = null;
            public string opt_namecell = null;
            public string opt_method = null;
            public string opt_collapse = null;
            public string opt_sheet = null;
            public string opt_all = null;            
            public string type = null;  //read or import
            public string opt_dateformat = null;
            public string opt_datetype = null;



            public P p = null;
            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);

                if (opt_collapse != null)
                {
                    Program.CollapsePoints(this);
                }
                else
                {
                    GekkoSmplSimple truncate = Program.HandleRespectPeriod(this.t1, this.t2, this.opt_respect, this.opt_all, this.type);
                    
                    ReadOpenMulbkHelper hlp = new ReadOpenMulbkHelper();  //This is a bit confusing, using an old object to store the stuff.
                    if (truncate != null)
                    {
                        hlp.t1 = truncate.t1;
                        hlp.t2 = truncate.t2;
                    }
                    hlp.dateformat = this.opt_dateformat;
                    hlp.datetype = this.opt_datetype;
                    hlp.sheet = this.opt_sheet;

                    bool isRead = false; if (G.Equal(this.type, "read")) isRead = true;

                    if (isRead && this.opt_all != null)
                    {
                        G.Writeln2("*** ERROR: READ<all> is not allowed");
                        throw new GekkoException();
                    }

                    if (!isRead && this.opt_respect != null)
                    {
                        G.Writeln2("*** ERROR: IMPORT<respect> is not allowed");
                        throw new GekkoException();
                    }

                    if (this.opt_prim != null)
                    {
                        if (isRead == false)
                        {   //import
                            G.Writeln2("*** ERROR: IMPORT<prim> is obsolete, use IMPORT.");
                            throw new GekkoException();
                        }
                        else
                        {   //read
                            G.Writeln2("*** ERROR: READ<prim> is obsolete, use READ<first>.");
                            throw new GekkoException();
                        }
                    }

                    if (isRead == false)  //import
                    {
                        if (this.opt_first != null)
                        {
                            G.Writeln2("*** ERROR: IMPORT<first> is not legal syntax, just use IMPORT.");
                            throw new GekkoException();
                        }
                        if (this.opt_merge != null)
                        {
                            G.Writeln2("*** ERROR: IMPORT<merge> is not legal syntax, IMPORT merges already.");
                            throw new GekkoException();
                        }
                        hlp.Merge = true;               //this is so for IMPORT
                        hlp.openType = EOpenType.First;  //this is so for IMPORT                    
                    }

                    bool isTo = false; if (this.readTo != null) isTo = true;
                    hlp.FileName = this.fileName;
                    if (G.Equal(this.opt_csv, "yes")) hlp.Type = EDataFormat.Csv;
                    if (G.Equal(this.opt_prn, "yes")) hlp.Type = EDataFormat.Prn;
                    if (G.Equal(this.opt_pcim, "yes")) hlp.Type = EDataFormat.Pcim;
                    if (G.Equal(this.opt_tsd, "yes")) hlp.Type = EDataFormat.Tsd;
                    if (G.Equal(this.opt_gbk, "yes")) hlp.Type = EDataFormat.Gbk;
                    if (G.Equal(this.opt_tsdx, "yes")) hlp.Type = EDataFormat.Tsdx;
                    if (G.Equal(this.opt_tsp, "yes")) hlp.Type = EDataFormat.Tsp;
                    if (G.Equal(this.opt_xls, "yes")) hlp.Type = EDataFormat.Xls;
                    if (G.Equal(this.opt_xlsx, "yes")) hlp.Type = EDataFormat.Xlsx;
                    if (G.Equal(this.opt_gdx, "yes")) hlp.Type = EDataFormat.Gdx;
                    if (G.Equal(this.opt_px, "yes")) hlp.Type = EDataFormat.Px;
                    if (G.Equal(this.opt_flat, "yes")) hlp.Type = EDataFormat.Flat;
                    if (G.Equal(this.opt_aremos, "yes")) hlp.Type = EDataFormat.Aremos;
                    if (G.Equal(this.opt_cols, "yes")) hlp.Orientation = "cols";

                    hlp.gdxopt = this.opt_gdxopt;

                    bool isSimple = false;

                    if (isTo)
                    {
                        //READ...TO  ==> same as OPEN
                        if (this.opt_merge != null)
                        {
                            G.Writeln2("*** ERROR: you cannot mix <merge> with TO keyword");
                        }
                        if (this.opt_first != null)
                        {
                            G.Writeln2("*** ERROR: you cannot mix <first> with TO keyword");
                        }
                        if (this.opt_ref != null)
                        {
                            G.Writeln2("*** ERROR: you cannot mix <ref> with TO keyword");
                        }
                    }
                    else
                    {
                        //READ or IMPORT
                        if (G.Equal(this.opt_merge, "yes")) hlp.Merge = true;
                        if (G.Equal(this.opt_first, "yes")) hlp.openType = EOpenType.First;
                        if (G.Equal(this.opt_ref, "yes")) hlp.openType = EOpenType.Ref;
                        if (hlp.openType == EOpenType.Normal) isSimple = true;  //in that case, a CLONE is done afterwards
                        if (isRead)
                        {
                            if (hlp.openType == EOpenType.First)
                            {
                                if (!Program.databanks.GetFirst().editable)
                                {
                                    G.Writeln2("*** ERROR: Cannot READ<first>, since first-position databank is non-editable");
                                    throw new GekkoException();
                                }
                            }
                            else if (hlp.openType == EOpenType.Ref)
                            {
                                if (!Program.databanks.GetRef().editable)
                                {
                                    G.Writeln2("*** ERROR: Cannot READ<ref>, since ref databank is non-editable");
                                    throw new GekkoException();
                                }
                            }
                            else
                            {
                                if (!Program.databanks.GetFirst().editable)
                                {
                                    G.Writeln2("*** ERROR: Cannot READ, since first-position databank is non-editable");
                                    throw new GekkoException();
                                }
                                if (!Program.databanks.GetRef().editable)
                                {
                                    G.Writeln2("*** ERROR: Cannot READ, since ref databank is non-editable");
                                    throw new GekkoException();
                                }
                            }
                        }
                        else
                        {
                            //IMPORT
                            if (!Program.databanks.GetFirst().editable)
                            {
                                G.Writeln2("*** ERROR: Cannot IMPORT, since first-position databank is non-editable");
                                throw new GekkoException();
                            }
                        }
                    }

                    if (G.Equal(Program.options.interface_mode, "data"))
                    {
                        if (isRead && isSimple)
                        {
                            if (Globals.modeIntendedWarning)
                            {
                                G.Writeln2("+++ WARNING: General READ is not intended for data-mode.");
                                G.Writeln("             Please use IMPORT, or consider READ<first>", Color.Red);
                            }
                        }
                        if (isRead && !isTo && hlp.openType == EOpenType.Ref)
                        {
                            if (Globals.modeIntendedWarning)
                            {
                                G.Writeln2("+++ WARNING: READ<ref> is not intended for data-mode.");
                                //G.Writeln("             Please use IMPORT, or consider READ<first>", Color.Red);
                                //throw new GekkoException();
                            }
                        }
                    }

                    List<Program.ReadInfo> readInfos = new List<Program.ReadInfo>();

                    //if (Globals.excelDna)
                    //{
                    //    MessageBox.Show("fname: " + hlp.FileName);
                    //}

                    bool open = false;
                    if (isTo)
                    {
                        //is in reality an OPEN                    
                        open = true;
                        hlp.Merge = false;  //but mixing <merge> and TO give error above anyway                
                        hlp.editable = false;  //superfluous but for safety
                        hlp.openType = EOpenType.Normal;
                        if (readTo == "*")
                        {
                            readTo = Path.GetFileNameWithoutExtension(hlp.FileName);
                        }
                        hlp.openFileNames = new List<List<string>>();
                        hlp.openFileNames.Add(new List<string>() { hlp.FileName, readTo });
                    }

                    bool wipeDatabankBeforeInsertingData = false;

                    if (isRead && !hlp.Merge && !isTo)
                    {
                        //See #987432529835
                        //READ, not IMPORT
                        //No READ<merge>
                        //No READ ... TO
                        wipeDatabankBeforeInsertingData = true;
                    }

                    hlp.array = this.opt_array;

                    CellOffset offset = new CellOffset();
                    offset.cell = this.opt_cell;
                    offset.namecell = this.opt_namecell;
                    offset.datecell = this.opt_datecell;

                    Program.OpenOrRead(offset, wipeDatabankBeforeInsertingData, hlp, open, readInfos, false);
                    Program.ReadInfo readInfo = readInfos[0];
                    readInfo.shouldMerge = hlp.Merge;

                    if (readInfo.abortedStar) return;  //an aborted READ *

                    if (G.Equal(opt_ref, "yes"))
                    {
                        readInfo.dbName = Program.databanks.GetRef().name;
                    }
                    else
                    {
                        readInfo.dbName = Program.databanks.GetFirst().name;
                    }

                    if (isTo)
                    {
                        readInfo.open = true;
                        if (readTo != null && readTo == "*") readTo = Path.GetFileNameWithoutExtension(readInfo.fileName);
                        readInfo.dbName = readTo;
                    }

                    G.Writeln();
                    readInfo.Print();

                    if (isRead && isSimple)
                    {
                        //See #987432529835
                        //isSimple can never be true with READ ... TO ...
                        //Do not do this with READ<first> or READ<ref>, only with READ.                    
                        Program.MulbkClone();
                        if (G.HasModelGekko() && (G.Equal(Program.options.interface_mode, "sim") || G.Equal(Program.options.interface_mode, "mixed")))
                        {
                            //only in sim or mixed mode, if a model is existing
                            CreateMissingModelVariables();
                        }
                    }

                    try
                    {
                        //Program.ShowPeriodInStatusField("");
                    }
                    catch (Exception e)
                    {
                        //ignore
                    }
                }

            }

            private static void CreateMissingModelVariables()
            {
                List<string> onlyDatabankNotModel = new List<string>();
                List<string> onlyModelNotDatabank = new List<string>();
                foreach (KeyValuePair<string, IVariable> kvp in Program.databanks.GetFirst().storage)
                {
                    string s = kvp.Key;
                    if (kvp.Value.Type() != EVariableType.Series) continue;  //only series
                    if (G.GetFreqFromName(s) != Program.options.freq) continue;
                    string s2 = G.Chop_RemoveFreq(s);
                    if (!Program.model.modelGekko.varsAType.ContainsKey(s2)) onlyDatabankNotModel.Add(s2);
                }
                foreach (string s in Program.model.modelGekko.varsAType.Keys)
                {
                    if (!Program.databanks.GetFirst().ContainsIVariable(s + "!" + G.GetFreq(Program.options.freq))) onlyModelNotDatabank.Add(s);
                    //if (!Program.databanks.GetFirst().ContainsIVariable(s + "!" + "a")) onlyModelNotDatabank.Add(s);
                }

                //See #8904327598432
                if (onlyDatabankNotModel.Count > 0)
                {
                    Action a = () =>
                    {
                        string s = null;
                        s += G.NL; //to avoid annoying visible blank
                        s += "Note: You may use 'DELETE<nonmodel>;' to remove the following superfluous timeseries:" + G.NL;
                        s += G.NL;
                        onlyDatabankNotModel.Sort(StringComparer.OrdinalIgnoreCase);
                        foreach (string ss in onlyDatabankNotModel)
                        {
                            s += ss + G.NL;
                        }
                        Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPage2;
                        O.Cls("output");
                        G.Writeln(s, ETabs.Output);
                    };
                    G.Writeln("+++ NOTE: There are " + onlyDatabankNotModel.Count + " non-model timeseries in the databank (" + G.GetLinkAction("show", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ")");
                }

                if (onlyModelNotDatabank.Count > 0)
                {
                    Action a = () =>
                    {
                        string s = null;
                        s += G.NL; //to avoid annoying visible blank
                        s += "Note: You may use 'CREATE #all;' to create the following timeseries:" + G.NL;
                        s += G.NL;
                        onlyModelNotDatabank.Sort(StringComparer.OrdinalIgnoreCase);
                        foreach (string ss in onlyModelNotDatabank)
                        {
                            s += ss + G.NL;
                        }
                        Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPage2;
                        O.Cls("output");
                        G.Writeln(s, ETabs.Output);
                    };
                    G.Writeln("+++ NOTE: There are " + onlyModelNotDatabank.Count + " non-databank timeseries in the model (" + G.GetLinkAction("show", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ")");
                }
            }

            private static Program.LinkContainer ListContainer(List<string> onlyDatabankNotModel)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string s in onlyDatabankNotModel)
                {
                    sb.AppendLine(s);
                }
                Program.LinkContainer lc = new Program.LinkContainer(sb.ToString());
                Globals.linkContainer.Add(lc.counter, lc);
                return lc;
            }
        }

        public class Download
        {
            public string dbUrl = null;  //path to server
            public string fileName = null;  //json file for statistikbanken
            public string fileName2 = null;  //dump file (px for statistikbanken, csv for jobindsats)            
            public string opt_array = null;  //arrays yes or no (statistikbanken)
            public string opt_key = null;  //only used for jobindsats
            public void Exe()
            {
                if ( G.Contains(this.dbUrl, "jobindsats"))
                {
                    OnlineDatabanks.DownloadJobindsats(this);
                }
                else
                {
                    OnlineDatabanks.Download(this);
                }
            }
        }

        //public class Import
        //{
        //    // ====== OBSOLETE ============
        //    // ====== OBSOLETE ============
        //    // ====== OBSOLETE ============            

        //    public string fileName = null;
        //    public string importTo = null;
        //    public string opt_tsd = null;
        //    public string opt_tsdx = null;
        //    public string opt_tsp = null;
        //    public string opt_csv = null;
        //    public string opt_prn = null;
        //    public string opt_pcim = null;
        //    public string opt_xls = null;
        //    public string opt_xlsx = null;            
        //    public string opt_cols = null;
        //    public string opt_ref = null;   
        //    public P p = null;
        //    public void Exe()
        //    {
        //        bool isImportOpen = false; if (this.importTo != null) isImportOpen = true;

        //        ReadOpenMulbkHelper hlp = new ReadOpenMulbkHelper();  //This is a bit confusing, using an old object to store the stuff.
        //        hlp.FileName = this.fileName;
        //        if (G.equal(this.opt_csv, "yes")) hlp.Type = EDataFormat.Csv;
        //        if (G.equal(this.opt_prn, "yes")) hlp.Type = EDataFormat.Prn;
        //        if (G.equal(this.opt_pcim, "yes")) hlp.Type = EDataFormat.Pcim;
        //        if (G.equal(this.opt_tsd, "yes")) hlp.Type = EDataFormat.Tsd;
        //        if (G.equal(this.opt_tsdx, "yes")) hlp.Type = EDataFormat.Tsdx;
        //        if (G.equal(this.opt_tsp, "yes")) hlp.Type = EDataFormat.Tsp;
        //        if (G.equal(this.opt_xls, "yes")) hlp.Type = EDataFormat.Xls;
        //        if (G.equal(this.opt_xlsx, "yes")) hlp.Type = EDataFormat.Xlsx;                
        //        if (G.equal(this.opt_cols, "yes")) hlp.Orientation = "cols";

        //        if (!isImportOpen)
        //        {
        //            hlp.Merge = true;  //this is so for IMPORT                    
        //            hlp.openType = EOpenType.First;  //this is so for IMPORT
        //            if (G.equal(opt_ref, "yes")) hlp.openType = EOpenType.Ref;  // <sec> can override
        //        }

        //        List<Program.ReadInfo> readInfos = new List<Program.ReadInfo>();

        //        bool open = false;
        //        if (isImportOpen)
        //        {
        //            //is in reality an OPEN
        //            if (hlp.openType == EOpenType.First)
        //            {
        //                G.Writeln2("*** ERROR: You cannot use IMPORT ... TO ... together with <first>");
        //                throw new GekkoException();
        //            }
        //            if (hlp.openType == EOpenType.Ref)
        //            {
        //                G.Writeln2("*** ERROR: You cannot use IMPORT ... TO ... together with <ref>");
        //                throw new GekkoException();
        //            }
        //            open = true;
        //            hlp.Merge = false;
        //            hlp.protect = true;  //superfluous but for safety
        //            hlp.openType = EOpenType.Normal;

        //            if (importTo == "ASTBANKISSTARCHEATCODE")
        //            {
        //                importTo = Path.GetFileNameWithoutExtension(hlp.FileName);
        //            }

        //            hlp.openFileNames = new List<List<string>>();
        //            hlp.openFileNames.Add(new List<string>() { hlp.FileName, importTo });
        //        }

        //        Program.OpenOrRead(hlp, open, readInfos);
        //        Program.ReadInfo readInfo = readInfos[0];
        //        readInfo.shouldMerge = hlp.Merge;
        //        if (readInfo.abortedStar) return;  //an aborted READ *
        //        if (G.equal(opt_ref, "yes"))
        //        {
        //            readInfo.dbName = Program.databanks.GetRef().aliasName;
        //        }
        //        else
        //        {
        //            readInfo.dbName = Program.databanks.GetFirst().aliasName;
        //        }

        //        if (isImportOpen)
        //        {
        //            readInfo.open = true;
        //            if (importTo != null && importTo == "*") importTo = Path.GetFileNameWithoutExtension(readInfo.fileName);
        //            readInfo.dbName = importTo;
        //        }
        //        G.Writeln();
        //        readInfo.Print();                

        //        try
        //        {
        //            Program.ShowPeriodInStatusField("");
        //        }
        //        catch (Exception e)
        //        {
        //            //ignore
        //        }
        //    }
        //}

        //See O.Prt for SHEET in export mode
        public class SheetImport
        {
            public string fileName = null;
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            //public List<string> listItems = null;
            public List names = null;
            public string opt_sheet = null;
            public string opt_cell = null;
            public string opt_rows = null;
            public string opt_cols = null;
            public string opt_matrix = null;
            public string opt_list = null;
            public string opt_map = null;
            public string opt_missing = null;  //used for matrix 
            // -------------
            public string opt_xls = null;
            public string opt_xlsx = null;
            public string opt_csv = null;
            public string opt_prn = null;
            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                Program.SheetImport(this);
            }
        }

        public class Pipe
        {
            public string fileName = null;
            public string opt_html = null;
            public string opt_append = null;
            public string opt_pause = null;
            public string opt_continue = null;
            public string opt_stop = null;

            public void Exe()
            {
                Program.Pipe(this);
            }
        }

        public class Clone
        {
            public void Exe()
            {
                Program.MulbkClone();
                Databank first = Program.databanks.GetFirst();
                int number = first.storage.Count;
                G.Writeln();
                G.Writeln("Cleared reference databank ('" + Program.databanks.GetRef().name + "') and copied " + number + " variables from first-position ('" + Program.databanks.GetFirst().name + "') to reference ('" + Program.databanks.GetRef().name + "') databank");
                if (G.Equal(Program.options.interface_mode, "data"))
                {
                    if (Globals.modeIntendedWarning)
                    {
                        G.Writeln2("+++ WARNING: CLONE is not intended for data-mode (cf. MODE)");
                    }
                }
            }
        }

        //public class Mulbk
        //{
        //    //Is this used anymore?????
        //    public string fileName = null;
        //    public string opt_tsd = null;
        //    public string opt_tsdx = null;
        //    public string opt_csv = null;
        //    public string opt_prn = null;
        //    public string opt_pcim = null;
        //    public string opt_xls = null;
        //    public string opt_xlsx = null;            
        //    public string opt_cols = null;
        //    public string opt_merge = null;
        //    public void Exe()
        //    {
        //        ReadOpenMulbkHelper hlp = new ReadOpenMulbkHelper();  //This is a bit confusing, using an old object to store the stuff.
        //        hlp.FileName = this.fileName;  //if null, it is a MULBK command without argument.
        //        if (this.opt_csv == "yes") hlp.Type = EDataFormat.Csv;
        //        if (this.opt_prn == "yes") hlp.Type = EDataFormat.Prn;
        //        if (this.opt_pcim == "yes") hlp.Type = EDataFormat.Pcim;
        //        if (this.opt_tsd == "yes") hlp.Type = EDataFormat.Tsd;
        //        if (this.opt_tsdx == "yes") hlp.Type = EDataFormat.Tsdx;
        //        if (this.opt_xls == "yes") hlp.Type = EDataFormat.Xls;
        //        if (this.opt_xlsx == "yes") hlp.Type = EDataFormat.Xlsx;                
        //        if (this.opt_cols == "yes") hlp.Orientation = "cols";
        //        hlp.openType = EOpenType.Ref;

        //        if (hlp.FileName == null)
        //        {
        //            Program.MulbkClone();
        //            Databank work = Program.databanks.GetFirst();
        //            int number = work.storage.Count;
        //            G.Writeln();
        //            G.Writeln("Cleared " + Globals.Ref + " databank and copied " + number + " variables from Work databank into " + Globals.Ref + " databank");
        //        }
        //        else
        //        {
        //            DateTime dt1 = DateTime.Now;
        //            List<Program.ReadInfo> readInfos = new List<Program.ReadInfo>();
        //            Program.OpenOrRead(hlp, false, readInfos);
        //            Program.ReadInfo readInfo = readInfos[0];
        //            readInfo.shouldMerge = hlp.Merge;

        //            //Globals.originalDataFileBaseline = helper.fileName;
        //            DateTime dt2 = DateTime.Now;
        //            double time = (dt2 - dt1).TotalMilliseconds;
        //            readInfo.dbName = Program.databanks.GetRef().aliasName;
        //            readInfo.Print();
        //            G.Writeln("+++ NOTE: You may use 'MULBK' without an argument to create a " + Globals.Ref + " databank.");
        //        }
        //    }
        //}

        public class Clear
        {
            public List names = null;
            public P p = null;
            public string opt_first = null;
            public string opt_ref = null;
            public void Exe()
            {
                Program.Clear(this, p);
            }
        }

        

        public class Restart
        {
            public P p = null;
            public void Exe(GekkoSmpl smpl)
            {
                Program.Re(smpl, "restart", p);
            }
        }

        public class Reset
        {
            public P p = null;
            public void Exe(GekkoSmpl smpl)
            {
                Program.Re(smpl, "reset", p);
            }
        }

        public class Smooth
        {
            //public IVariable rhs = null;
            //public IVariable lhs = null;

            public List names0;
            public List names1;
            public List names2;

            public string opt_spline = null;
            public string opt_geometric = null;
            public string opt_linear = null;
            public string opt_repeat = null;
            public string opt_overlay = null;
            public P p = null;
            public void Exe()
            {
                //If the method fails, no timeseries are touched. That is good!

                //all this could be sped up by means of using the internal arrays (GetDataSequence).
                //so some room for improvement if it becomes a bottleneck.
                //only done for missings enclosed by real numbers (no end-missings will be filled)  

                List<string> listItems0 = Restrict(this.names0, true, false, true, true);
                List<string> listItems1 = Restrict(this.names1, true, false, true, true);
                List<string> listItems2 = null;
                if (this.names2 != null) listItems2 = Restrict(this.names2, true, false, true, true);

                if (listItems0.Count != 1 || listItems1.Count != 1)
                {
                    G.Writeln2("*** ERROR: SMOOTH only supports one variable at the time, not lists (for now)");
                    throw new GekkoException();
                }

                IVariable ivOld = O.GetIVariableFromString(listItems1[0], ECreatePossibilities.NoneReportError, true);
                IVariable ivLhs = O.GetIVariableFromString(listItems0[0], ECreatePossibilities.Can);

                Series oldSeries = O.ConvertToSeries(ivOld) as Series;
                Series lhs = O.ConvertToSeries(ivLhs) as Series;

                //if (oldSeries.name.ToLower().Contains("d74200000001"))
                //{

                //}

                Series newSeriesTemp = oldSeries.DeepClone(null) as Series;  //brand new object, not present in Work (yet)                

                ESmoothTypes type = ESmoothTypes.Spline;  //what is the default in AREMOS??
                if (G.Equal(opt_geometric, "yes")) type = ESmoothTypes.Geometric;
                if (G.Equal(opt_linear, "yes")) type = ESmoothTypes.Linear;
                if (G.Equal(opt_spline, "yes")) type = ESmoothTypes.Spline;
                if (G.Equal(opt_repeat, "yes")) type = ESmoothTypes.Repeat;
                if (G.Equal(opt_overlay, "yes")) type = ESmoothTypes.Overlay;

                GekkoTime realStart = oldSeries.GetRealDataPeriodFirst();
                GekkoTime realEnd = oldSeries.GetRealDataPeriodLast();

                if (realStart.IsNull())
                {
                    //do nothing, the lhs series is not touched (but may be created here)
                    G.Writeln2("Smooth of '" + oldSeries.name + "', method = " + type.ToString().ToLower() + " (" + oldSeries.name + " has no data)");                    
                }
                else
                {
                    //this works okay if the rhs series has only 1 observation
                    if (type == ESmoothTypes.Spline)
                    {
                        int counter = -1;  //this can actually be an arbitrary number, but we start with 0.
                        List<double> xx = new List<double>();
                        List<double> yy = new List<double>();
                        List<int> missings = new List<int>();
                        List<GekkoTime> missingsDates = new List<GekkoTime>();
                        foreach (GekkoTime gt in new GekkoTimeIterator(realStart, realEnd))
                        {
                            counter++;
                            double data = oldSeries.GetDataSimple(gt);
                            if (G.isNumericalError(data))
                            {
                                missings.Add(counter);
                                missingsDates.Add(gt);
                                continue;  //ignore this observation
                            }
                            yy.Add(oldSeries.GetDataSimple(gt));
                            xx.Add(counter);
                        }

                        double[] x = xx.ToArray();
                        double[] y = yy.ToArray();

                        alglib.spline1dinterpolant s;
                        alglib.spline1dbuildcubic(x, y, out s);
                        for (int i = 0; i < missings.Count; i++)
                        {
                            newSeriesTemp.SetData(missingsDates[i], alglib.spline1dcalc(s, missings[i]));
                        }

                        //AREMOS spline, fits parabola nicely. Gekko gives the same on this input data (left column)
                        //2000       1.000000000000000     1.000000000000000
                        //2001                      NC     0.562500000000000
                        //2002       0.250000000000000     0.250000000000000
                        //2003                      NC     0.062500000000000
                        //2004       0.000000000000000     0.000000000000000
                        //2005                      NC     0.062500000000000
                        //2006       0.250000000000000     0.250000000000000
                        //2007                      NC     0.562500000000000
                        //2008       1.000000000000000     1.000000000000000                 
                    }
                    else if (type == ESmoothTypes.Linear || type == ESmoothTypes.Repeat || type == ESmoothTypes.Geometric)
                    {
                        GekkoTime missingStart = GekkoTime.tNull;
                        bool recording = false;
                        foreach (GekkoTime gt in new GekkoTimeIterator(realStart, realEnd))
                        {
                            //realStart and realEnd can not be tNull here
                            double z = oldSeries.GetDataSimple(gt);
                            if (G.isNumericalError(z))
                            {
                                if (!recording)
                                {
                                    missingStart = gt;
                                    recording = true;
                                }
                                continue;
                            }

                            if (recording)
                            {
                                GekkoTime t1 = missingStart.Add(-1);
                                GekkoTime t2 = gt;
                                double z1 = oldSeries.GetDataSimple(t1);
                                double z2 = oldSeries.GetDataSimple(t2);
                                double n = GekkoTime.Observations(t1, t2) - 1;
                                if (type == ESmoothTypes.Geometric)
                                {
                                    if (z1 <= 0d || z2 <= 0d)
                                    {
                                        G.Writeln2("*** ERROR: Geometric smoothing not intended for numbers <= 0");
                                        throw new GekkoException();
                                    }
                                }
                                double counterLinear = z1;
                                double counterLinearA = (z2 - z1) / n;
                                double counterGeometric = z1;
                                double counterGeometricA = Math.Pow((z2 / z1), 1d / n);

                                foreach (GekkoTime gt2 in new GekkoTimeIterator(t1.Add(1), t2.Add(-1)))
                                {
                                    if (type == ESmoothTypes.Repeat)
                                    {
                                        newSeriesTemp.SetData(gt2, z1);
                                    }
                                    else if (type == ESmoothTypes.Linear)
                                    {
                                        counterLinear += counterLinearA;
                                        newSeriesTemp.SetData(gt2, counterLinear);
                                    }
                                    else if (type == ESmoothTypes.Geometric)
                                    {
                                        counterLinear *= counterGeometricA;
                                        newSeriesTemp.SetData(gt2, counterLinear);
                                    }
                                    else throw new GekkoException();
                                }
                                recording = false;
                                missingStart = GekkoTime.tNull;
                            }
                        }
                    }
                    else if (type == ESmoothTypes.Overlay)
                    {
                        Series overlay = O.GetIVariableFromString(listItems2[0], ECreatePossibilities.NoneReportError, true) as Series;
                        GekkoTime realStartOverlay = overlay.GetRealDataPeriodFirst();
                        GekkoTime realEndOverlay = overlay.GetRealDataPeriodLast();

                        if (realStartOverlay.IsNull())
                        {
                            G.Writeln2("+++ WARNING: The overlay series '" + overlay.GetName() + "' has no observations");
                        }
                        else
                        {
                            foreach (GekkoTime gt in new GekkoTimeIterator(realStartOverlay, realEndOverlay))
                            {
                                //realStart and realEnd can not be tNull here
                                double z = oldSeries.GetDataSimple(gt);
                                if (G.isNumericalError(z))
                                {
                                    newSeriesTemp.SetData(gt, overlay.GetDataSimple(gt));
                                }
                            }
                        }
                    }
                    else throw new GekkoException();

                    foreach (GekkoTime gt in new GekkoTimeIterator(realStart, realEnd))
                    {
                        //This is not terribly efficient, and we could use array copy etc.
                        //And we do create and clone a whole new timeseries (newSeriesTemp).
                        //But it works, and speed is probably not an issue with SMOOTH.
                        lhs.SetData(gt, newSeriesTemp.GetDataSimple(gt));
                    }
                    lhs.Stamp();
                    G.Writeln2("Smooth of '" + oldSeries.name + "', method = " + type.ToString().ToLower() + ", " + realStart.ToString() + "-" + realEnd.ToString());
                }
            }
        }

        public class Splice
        {



            public List names0 = null;
            public List names1 = null;
            public List names2 = null;


            public GekkoTime date = GekkoTime.tNull;
            public void Exe()
            {
                List<string> listItems0 = Restrict(names0, true, false, true, true);
                List<string> listItems1 = Restrict(names1, true, false, true, true);
                List<string> listItems2 = Restrict(names2, true, false, true, true);

                bool useSecondPartLevels = true;  //like aremos

                if (listItems0.Count != 1 || listItems1.Count != 1 || listItems2.Count != 1)
                {
                    G.Writeln2("*** ERROR: SPLICE only supports one variable at a time, not lists (for now)");
                    throw new GekkoException();
                }

                IVariable iv1 = O.GetIVariableFromString(listItems1[0], ECreatePossibilities.NoneReportError, true);
                IVariable iv2 = O.GetIVariableFromString(listItems2[0], ECreatePossibilities.NoneReportError, true);
                IVariable iv3 = O.GetIVariableFromString(listItems0[0], ECreatePossibilities.Can);  //left side

                Series ts1 = O.ConvertToSeries(iv1) as Series;
                Series ts2 = O.ConvertToSeries(iv2) as Series;
                Series ts3 = O.ConvertToSeries(iv3) as Series;

                if (ts1.freq != ts2.freq)
                {
                    G.Writeln2("*** ERROR: Different freq for the two timerseries");
                    throw new GekkoException();
                }
                GekkoTime t1a = ts1.GetRealDataPeriodFirst();
                if (t1a.IsNull())
                {
                    G.Writeln2("*** ERROR: No data in first timeseries");
                    throw new GekkoException();
                }
                GekkoTime t1b = ts1.GetRealDataPeriodLast();
                GekkoTime t2a = ts2.GetRealDataPeriodFirst();
                if (t2a.IsNull())
                {
                    G.Writeln2("*** ERROR: No data in second timeseries");
                    throw new GekkoException();
                }
                GekkoTime t2b = ts2.GetRealDataPeriodLast();
                if (!date.IsNull())
                {
                    if (date.freq != ts1.freq || date.freq != ts2.freq)
                    {
                        G.Writeln2("*** ERROR: Wrong freq for indicated period");
                        throw new GekkoException();
                    }
                    t1b = date;
                    t2a = date;
                }
                int obs = GekkoTime.Observations(t2a, t1b);
                if (obs < 1)
                {
                    G.Writeln2("*** ERROR: No overlapping periods for SPLICE");
                    throw new GekkoException();
                }


                //          ts1        ts2
                //2002      2.000000                 t1a = 2002
                //2003      3.000000              
                //2004      4.000000   41.000000     t2a = 2004
                //2005      5.000000   42.000000  
                //2006      6.000000   43.000000     t1b = 2006
                //2007                 44.000000  
                //2008                 45.000000  
                //2009                 46.000000  
                //2010                 46.000000     t2b = 2010


                double count = 0d;
                double sum1 = 0d;
                double sum2 = 0d;
                foreach (GekkoTime gt in new GekkoTimeIterator(t2a, t1b))
                {
                    count++;
                    sum1 += ts1.GetDataSimple(gt);
                    sum2 += ts2.GetDataSimple(gt);
                }
                double avg1 = sum1 / count;
                double avg2 = sum2 / count;

                if (useSecondPartLevels)
                {

                    if (avg2 == 0d)
                    {
                        G.Writeln2("*** ERROR: Avg = 0 for second timeseries over common period " + t2a + "-" + t1b);
                        throw new GekkoException();
                    }
                    double relative = avg1 / avg2;
                    if (G.isNumericalError(relative))
                    {
                        G.Writeln2("*** ERROR: Seems there are missing data for common period " + t2a + "-" + t1b);
                        throw new GekkoException();
                    }
                    foreach (GekkoTime gt in new GekkoTimeIterator(t1a, t2a.Add(-1)))
                    {
                        ts3.SetData(gt, ts1.GetDataSimple(gt) / relative);
                    }
                    foreach (GekkoTime gt in new GekkoTimeIterator(t2a, t2b))
                    {
                        ts3.SetData(gt, ts2.GetDataSimple(gt));
                    }
                }
                else
                {

                    if (avg2 == 0d)
                    {
                        G.Writeln2("*** ERROR: Avg = 0 for second timeseries over common period " + t2a + "-" + t1b);
                        throw new GekkoException();
                    }
                    double relative = avg1 / avg2;
                    if (G.isNumericalError(relative))
                    {
                        G.Writeln2("*** ERROR: Seems there are missing data for common period " + t2a + "-" + t1b);
                        throw new GekkoException();
                    }
                    foreach (GekkoTime gt in new GekkoTimeIterator(t1a, t1b))
                    {
                        ts3.SetData(gt, ts1.GetDataSimple(gt));
                    }
                    foreach (GekkoTime gt in new GekkoTimeIterator(t1b.Add(1), t2b))
                    {
                        ts3.SetData(gt, ts2.GetDataSimple(gt) * relative);
                    }
                }
                ts3.Stamp();
                G.Writeln2("Spliced '" + ts3.name + "' by means of " + obs + " common observations");
            }
        }

        public class Lock
        {
            public string bank = null;
            public void Exe()
            {
                if (G.Equal(bank, Globals.Work))
                {
                    G.Writeln2("*** ERROR: Work databank cannot be set non-editable");
                    throw new GekkoException();
                }
                if (G.Equal(bank, Globals.Ref))
                {
                    G.Writeln2("*** ERROR: Ref databank cannot be set non-editable");
                    throw new GekkoException();
                }
                Databank db = Program.databanks.GetDatabank(this.bank);
                if (db == null)
                {
                    G.Writeln2("*** ERROR: Databank '" + this.bank + "' is not open, cf. OPEN command");
                    throw new GekkoException();
                }
                if (db.editable == false)
                {
                    G.Writeln2("Databank '" + this.bank + "' is already non-editable");
                }
                else
                {
                    db.editable = false;
                    G.Writeln2("Databank '" + this.bank + "' set non-editable");
                }
            }
        }

        public class Unlock
        {
            public string bank = null;
            public void Exe()
            {
                Databank db = Program.databanks.GetDatabank(this.bank);
                if (db == null)
                {
                    G.Writeln2("*** ERROR: Databank '" + this.bank + "' is not open, cf. OPEN command");
                    throw new GekkoException();
                }
                if (db.editable == true)
                {
                    G.Writeln2("Databank '" + this.bank + "' is already editable");
                }
                else
                {
                    db.editable = true;
                    G.Writeln2("Databank '" + this.bank + "' set editable");
                }
            }
        }

        public class Close
        {
            public string name = null;  //only if '*' is indicated, not used otherwise
            public List listItems = null;
            public string opt_save = null;
            public void Exe()
            {

                List<string> databanks = new List<string>();
                if (this.listItems == null)
                {
                    if (this.name == "*")
                    {
                        foreach (Databank db in Program.databanks.storage)
                        {
                            if (G.Equal(db.name, Globals.Work) || G.Equal(db.name, Globals.Ref))
                            {
                                //skip it
                            }
                            else databanks.Add(db.name);
                        }
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Internal error #7983264234");
                        throw new GekkoException();
                    }
                }
                else
                {
                    List<string> names = Restrict(this.listItems, false, false, false, false);

                    if (names.Count == 1 && names[0] == "*")
                    {
                        foreach (Databank db in Program.databanks.storage)
                        {
                            if (G.Equal(db.name, Globals.Work) || G.Equal(db.name, Globals.Ref) || G.Equal(db.name, Globals.Local) || G.Equal(db.name, Globals.Global))
                            {
                                //skip it
                            }
                            else databanks.Add(db.name);
                        }
                    }
                    else
                    {

                        foreach (string dbName in names)
                        {
                            if (G.Equal(dbName, Globals.Work) || G.Equal(dbName, Globals.Ref))
                            {
                                G.Writeln2("*** ERROR: Databanks '" + Globals.Work + "' or '" + Globals.Ref + "' cannot be closed (see CLEAR command)");
                                throw new GekkoException();
                            }
                            databanks.Add(dbName);
                        }
                    }
                }
                foreach (string databank in databanks)
                {
                    if (Program.databanks.GetDatabank(databank) == null)
                    {
                        G.Writeln2("*** ERROR: Trying to close non-existing databank '" + databank + "'");
                        throw new GekkoException();
                    }
                    Databank removed = Program.databanks.RemoveDatabank(databank);
                    if (G.Equal(opt_save, "no"))
                    {
                        //do nothing, a CLOSE<save=no>
                    }
                    else
                    {
                        Program.MaybeWriteOpenDatabank(removed);
                    }
                }
                if (databanks.Count > 0)
                {
                    if (databanks.Count == 1)
                    {
                        G.Writeln2("Closed databank '" + databanks[0] + "'");
                    }
                    else
                    {
                        G.Writeln2("Closed " + databanks.Count + " databanks: ");
                        G.PrintListWithCommas(databanks, false);
                    }
                    if (G.Equal(opt_save, "no"))
                    {
                        G.Writeln("+++ NOTE: CLOSE<save=no> was used.");
                    }
                }
                else
                {
                    G.Writeln2("There were no open databanks to close (Work and " + Globals.Ref + " cannot be closed)");
                }
            }



        }

        public class Open
        {
            //public List<List<string>> openFileNames = new List<List<string>>();
            //public string fileName = null;

            public IVariable openFileNames2 = null;
            public IVariable openFileNamesAs2 = null;

            public string opt_tsd = null;
            public string opt_gbk = null;
            public string opt_gdx = null;
            public string opt_gdxopt = null;
            public string opt_tsdx = null;
            public string opt_csv = null;
            public string opt_prn = null;
            public string opt_pcim = null;
            public string opt_xls = null;
            public string opt_xlsx = null;
            public string opt_px = null;
            public string opt_cols = null;
            //public string as2 = null;
            public string opt_prim = null;  //obsolete but gives warning
            public string opt_first = null;
            public string opt_sec = null;
            public string opt_last = null;
            public string opt_ref = null;
            public string opt_prot = null;  //obsolete but gives warning
            public string opt_edit = null;
            public string opt_save = null;
            public double opt_pos = double.NaN;
            public string opt_create = null;  //may use OPEN b1, where b1.gbk does not exist (like OPEN<edit>b1).

            public void Exe()
            {
                List openFileNames = null;
                List openFileNamesAs = null;
                if (this.openFileNames2 != null) openFileNames = this.openFileNames2 as List;
                if (this.openFileNamesAs2 != null) openFileNamesAs = this.openFileNamesAs2 as List;

                if (G.Equal(opt_prot, "yes"))
                {
                    G.Writeln2("+++ NOTE: The OPEN<prot> option is obsolete and can be omitted here.");
                }

                if (G.Equal(opt_prim, "yes"))
                {
                    G.Writeln2("*** ERROR: OPEN<prim> is obsolete. In Gekko 2.1.1 and onwards, you should");
                    G.Writeln("           use OPEN<edit> instead of OPEN<prim>, if you intend to change", Color.Red);
                    G.Writeln("           data in the databank.", Color.Red);
                    throw new GekkoException();
                }

                ReadOpenMulbkHelper hlp = new ReadOpenMulbkHelper();  //This is a bit confusing, using an old object to store the stuff.

                bool create = false;
                if (G.Equal(opt_create, "yes"))
                {
                    create = true;
                }

                //hlp.openFileNames = this.openFileNames;
                hlp.openFileNames = new List<List<string>>();

                if (openFileNamesAs != null && openFileNames.list.Count != openFileNamesAs.list.Count)
                {
                    G.Writeln2("*** ERROR: Provided " + openFileNames.list.Count + " filenames, but got only " + openFileNamesAs.list.Count + " alias (AS) names");
                    throw new GekkoException();
                }

                for (int i = 0; i < openFileNames.list.Count; i++)
                {
                    List<string> temp = new List<string>();
                    temp.Add(openFileNames.list[i].ConvertToString());
                    if (openFileNamesAs != null)
                    {
                        temp.Add(openFileNamesAs.list[i].ConvertToString());
                    }
                    else
                    {
                        temp.Add(null);
                    }
                    hlp.openFileNames.Add(temp);
                }

                if (this.opt_csv == "yes") hlp.Type = EDataFormat.Csv;
                if (this.opt_prn == "yes") hlp.Type = EDataFormat.Prn;
                if (this.opt_pcim == "yes") hlp.Type = EDataFormat.Pcim;
                if (this.opt_tsd == "yes") hlp.Type = EDataFormat.Tsd;
                if (this.opt_tsdx == "yes") hlp.Type = EDataFormat.Tsdx;
                if (this.opt_gbk == "yes") hlp.Type = EDataFormat.Gbk;
                if (this.opt_xls == "yes") hlp.Type = EDataFormat.Xls;
                if (this.opt_xlsx == "yes") hlp.Type = EDataFormat.Xlsx;
                if (this.opt_gdx == "yes") hlp.Type = EDataFormat.Gdx;
                if (this.opt_px == "yes") hlp.Type = EDataFormat.Px;
                if (this.opt_cols == "yes") hlp.Orientation = "cols";
                //if (this.as2 != null) hlp.As = this.as2;

                hlp.gdxopt = this.opt_gdxopt;

                int posCounter = 0;
                if (G.Equal(opt_first, "yes")) posCounter++;
                if (G.Equal(opt_ref, "yes")) posCounter++;
                if (G.Equal(opt_last, "yes")) posCounter++;
                if (!G.isNumericalError(this.opt_pos)) posCounter++;

                if (posCounter > 1)
                {
                    G.Writeln2("*** ERROR: You are using > 1 of first/last/pos/ref designations inside <>-field");
                    throw new GekkoException();
                }
                if (G.Equal(opt_edit, "yes") && posCounter > 0)
                {
                    G.Writeln2("*** ERROR: You cannot mix 'edit' with first/last/pos/ref designations inside <>-field");
                    throw new GekkoException();
                }
                if (G.Equal(opt_first, "yes"))
                {
                    hlp.openType = EOpenType.First;
                }
                if (G.Equal(opt_last, "yes"))
                {
                    hlp.openType = EOpenType.Last;
                }
                if (G.Equal(opt_edit, "yes"))
                {
                    hlp.openType = EOpenType.Edit;
                    hlp.editable = true;  //will override the born false value of the field
                }
                if (G.Equal(opt_ref, "yes"))
                {
                    G.Writeln2("*** ERROR: OPEN<ref> is not allowed in Gekko 3.0, please use READ<ref> or IMPORT<ref> instead");
                    throw new GekkoException();
                    hlp.openType = EOpenType.Ref;
                }
                if (G.Equal(opt_sec, "yes"))
                {
                    hlp.openType = EOpenType.Sec;
                }
                if (!G.isNumericalError(this.opt_pos))
                {
                    hlp.openType = EOpenType.Pos;
                    if (G.ConvertToInt(out hlp.openTypePosition, opt_pos) == false)
                    {
                        G.Writeln2("*** ERROR: OPEN<pos=...> should be integer value");
                        throw new GekkoException();
                    }
                }

                CellOffset offset = new CellOffset();

                List<Program.ReadInfo> readInfos = new List<Program.ReadInfo>();
                Program.OpenOrRead(offset, false, hlp, true, readInfos, create);

                foreach (Program.ReadInfo readInfo in readInfos)
                {
                    readInfo.open = true;
                    if (readInfo.abortedStar) return;  //an aborted OPEN *

                    if (G.Equal(opt_save, "no"))
                    {
                        readInfo.databank.save = false;
                    }

                    readInfo.Print();
                }

                if (G.Equal(Program.options.interface_mode, "sim"))
                {
                    G.Writeln2("+++ WARNING: READ ... TO ... is recommended instead of OPEN in sim-mode (cf. MODE).");
                    G.Writeln("             For instance, 'READ databk TO *;' instead of 'OPEN databk;'", Globals.warningColor);
                }
            }
        }

        public class Run
        {
            public string fileName = null;
            public double opt_skip = 0d;
            public P p = null;
            public void Exe()
            {
                Program.Run(this);
            }
        }                

        public class Doc
        {
            public string opt_browser = null;
            public string opt_label = null;
            public string opt_source = null;
            public string opt_units = null;
            public string opt_stamp = null;
            public List names = null;

            public void Exe()
            {
                if (G.Equal(this.opt_browser, "yes"))
                {
                    Program.Browser();
                    return;
                }
                else
                {
                    List<string> vars = Restrict(this.names, true, false, true, false);
                    foreach (string s in vars)
                    {
                        IVariable iv = GetIVariableFromString(s, ECreatePossibilities.NoneReportError);  //no searching!
                        Series iv_series = iv as Series;
                        if (iv_series == null)
                        {
                            G.Writeln2("*** ERROR: Only series are allowed as input varname");
                            throw new GekkoException();
                        }
                        if (opt_label != null) iv_series.meta.label = opt_label;
                        if (opt_source != null) iv_series.meta.source = opt_source;
                        if (opt_units != null) iv_series.meta.units = opt_units;
                        if (opt_stamp != null) iv_series.meta.stamp = opt_stamp;
                        iv_series.meta.SetDirty(true);
                    }
                }
            }
        }

        public class Assignment
        {
            public string opt_n = null;
            public string opt_d = null;
            public string opt_p = null;
            public string opt_m = null;
            public string opt_q = null;
            public string opt_mp = null;
            public string opt_dl = null;
            public string opt_l = null;


            public string opt_keep = null;
            public IVariable opt_rownames = null;
            public IVariable opt_colnames = null;
            public IVariable label = null;
            public string opt_label = null;
            public string opt_source = null;
            public string opt_units = null;
            public string opt_stamp = null;
            public string opt_dyn = null;            
            public string opt_lsfunc = null;  //left-side function
            public string opt_missing = null;
        }

        public class Accept
        {
            public string type = null;
            public IVariable name = null;
            public IVariable message = null;
            public void Exe()
            {
                string inputValue = "";
                string msg = O.ConvertToString(message);
                msg = Program.HandleNewlines(msg);
                string varname = name.ConvertToString();
                
                string txt = null;

                if (Program.InputBox("Accept", msg, ref inputValue) == DialogResult.OK)
                {
                    IVariable iv = O.AcceptHelper1(this.type, inputValue);
                    string s = O.AcceptHelper2(this.type, iv);
                    Program.databanks.GetFirst().AddIVariableWithOverwrite(varname, iv);
                    G.Writeln2(this.type.ToString().ToUpper() + " " + varname + " = " + s);
                }
            }            
        }

        public class Analyze
        {

            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set      
            public double lag = double.NaN;
            public List<IVariable> x = null;
            public List<string> expressionsText = null;
            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                Program.Analyze(this);
            }
        }

        public class Copy
        {
            public GekkoTime t1 = GekkoTime.tNull;
            public GekkoTime t2 = GekkoTime.tNull;
            public string opt_respect = null;
            public List names1 = null;
            public List names2 = null;
            public string opt_bank = null;
            public string opt_frombank = null;
            public string opt_tobank = null;
            public string opt_error = null;  //note: COPY will stil error if a wildcard/range does not match anything
            public string opt_print = null;
            public string opt_type = null;
            public string type = null;

            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                Program.Copy(this);
            }            

            public static Databank SetPossibleToBank(Databank localOptionToBank)
            {
                Databank toBank = null;
                if (localOptionToBank != null) toBank = localOptionToBank;
                return toBank;
            }
        }

        public class Compare
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public List listItems = null;
            public string opt_sort = null;
            public string opt_dump = null;
            public string fileName = null;
            public double opt_abs = 0d; //important that this i 0
            public double opt_rel = 0d; //important that this i 0
            public double opt_pch = 0d; //important that this i 0
            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                Program.Compare(this);
            }
        }

        public class Truncate
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public List names = null;

            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                List<string> listItems = Program.Search(this.names, null, EVariableType.Series);
                int counter = 0;
                foreach (string s in listItems)
                {
                    IVariable iv = O.GetIVariableFromString(s, ECreatePossibilities.NoneReportError); //no searching!
                    Series ts = iv as Series;
                    ts.Truncate(this.t1, this.t2);
                    counter++;
                    ts.Stamp();
                }
                if (counter == 0)
                {
                    G.Writeln2("Did not match any variables to truncate");
                    throw new GekkoException();
                }
                G.Writeln2("Truncated " + counter + " series to " + t1 + "-" + t2 + "");
            }
        }

        public class Rename
        {

            public List names0 = null;
            public List names1 = null;

            public string opt_bank = null;
            public string opt_frombank = null;
            public string opt_tobank = null;
            public string opt_print = null;
            public string opt_type = null;

            public string type = null;

            public void Exe()
            {
                //actually also allows "rename" from one bank to another

                Program.Rename(this);
            }
        }

        public class Create
        {
            //public List<string> listItems = null;
            public List names = null;
            public bool question = false;
            public P p = null;
            public void Exe()
            {
                Program.Create(this.names, this.question, this);
            }
        }

        //public class CreateExpression
        //{
        //    public IVariable lhs = null;
        //    public IVariable rhs = null;
        //    public void Exe()
        //    {
        //        Series tlhs = O.GetTimeSeries(lhs);

        //        Databank bankName = tlhs.meta.parentDatabank;
        //        string varName = tlhs.name;

        //        Series trhs = O.GetTimeSeries(rhs);
        //        trhs.name = varName;

        //        bankName.RemoveVariable(varName);
        //        bankName.AddVariable(trhs);

        //        trhs.Stamp(); //for instance chain index function results in new date stamp                

        //    }
        //}

        public class Delete
        {
            public List names = null;
            public string opt_nonmodel = null;
            public P p = null;
            public void Exe()
            {
                if (G.Equal(opt_nonmodel, "yes"))
                {
                    if (this.names != null)
                    {
                        G.Writeln2("*** ERROR: You cannot mix <nonmodel> and variables");
                        throw new GekkoException();
                    }
                    Program.Trimvars();
                }
                else
                {
                    Program.Delete(this.names);
                }
            }
        }

        public class Predict
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set            
            public List iv = null;
            //public string type = null;

            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                List<string> vars = O.Restrict(this.iv, false, false, false, false);

                if (!G.HasModelGekko())
                {
                    G.Writeln2("*** ERROR: PREDICT does not work without a Gekko model (cf. MODEL)");
                    throw new GekkoException();
                }

                List<EquationHelper> eqs = new List<EquationHelper>();

                List<List<EquationHelper>> lists = new List<List<EquationHelper>>();
                lists.Add(Program.model.modelGekko.equations);
                lists.Add(Program.model.modelGekko.equationsReverted);
                lists.Add(Program.model.modelGekko.equationsNotRunAtAll);

                foreach (string s in vars)
                {
                    bool found = false;
                    foreach (List<EquationHelper> equs in lists)
                    {                        
                        foreach (EquationHelper eh in equs)
                        {
                            if (G.Equal(eh.lhs, s))
                            {
                                eqs.Add(eh);
                                found = true;
                                goto label1;
                            }
                        }                    
                    }

                label1:;

                    if (!found)
                    {
                        G.Writeln2("*** ERROR: PREDICT could not find equation corresponding to variable '" + s + "'");
                        throw new GekkoException();
                    }
                }

                bool ok = true; //if all are already "Action"-ed, there is no need to compile etc.          
                foreach (EquationHelper eh in eqs)
                {
                    if (eh.predictAction == null)
                    {
                        ok = false;
                        break;
                    }
                }

                if (!ok)
                {
                    
                    int counter = -1;
                    StringBuilder sb = new StringBuilder();
                    foreach (EquationHelper eh in eqs)
                    {
                        counter++;
                        string s = "Globals.predictActions[" + counter + "] = (name, gt) => { double d = " + eh.csCodeRhsLongVersion + "; O.PredictSetValue(name, gt, d); };";
                        sb.AppendLine(s);
                    }

                    Globals.predictActions = new List<Action<string, GekkoTime>>();
                    foreach (EquationHelper eh in eqs)
                    {
                        Globals.predictActions.Add(null);
                    }
                    Program.CreatePredictActions(sb.ToString());

                    counter = -1;
                    foreach (EquationHelper eh in eqs)
                    {
                        counter++;
                        eh.predictAction = Globals.predictActions[counter];
                    }
                }
                                
                foreach (EquationHelper eh in eqs)
                {                    
                    foreach (GekkoTime gt in new GekkoTimeIterator(this.t1, this.t2))
                    {
                        eh.predictAction(eh.lhs, gt);
                    }
                }
            }            
        }

        public class Disp
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            //public List<string> listItems = null;
            public List iv = null;
            public string searchName = null;
            public string opt_info = null;
            public string type = null;

            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                                
                if (G.Equal(this.opt_info, "yes"))
                {
                    Program.Info(this.t1, this.t2, iv);                    
                }
                else
                {
                    if (this.searchName == null)
                    {
                        Program.Disp(this.t1, this.t2, null, this);
                    }
                    else
                    {
                        Program.DispSearch(this.searchName);
                    }
                }
            }
        }

        public class Find
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set            
            public List iv = null;            
            public string opt_prtcode = null;
            public string rv = null; //return value

            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                this.rv = Program.Find(this);
            }
        }

        public class Show
        {
            public IVariable input = null;
            public string label = null;
            public void Exe()
            {
                if (input.Type() == EVariableType.Matrix)
                {
                    Matrix a = (Matrix)input;
                    Program.ShowMatrix(a, this.label);
                }
                else
                {
                    G.Writeln2("*** ERROR: Unsupported type (" + input.Type().ToString() + "), for SHOW");
                    throw new GekkoException();
                }
            }
        }

        public class Decomp1
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            //public GekkoSmpl smplForFunc = null;
            public string variable = null;
            public string expressionCs = null;
            public Func<GekkoSmpl, IVariable> expression = null;
            public string opt_prtcode = null;
            public string label = null;
            public IVariable name = null;  //name given from ASTDECOMPITEMS2, is only active if DECOMP x, DECOM x[a] and the like (a name, no expression)
            
            
            public void Exe()
            {
                Globals.lastDecompTable = null;
                G.CheckLegalPeriod(this.t1, this.t2);
                if (G.NullOrEmpty(this.opt_prtcode)) this.opt_prtcode = "n";
                //Gekko.Table tab = Program.Decompose(this);
                Program.Decomp(this);
            }
        }


        public class Decomp2
        {
            //See source code documentation

            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set  
            public string type = null;
            public string opt_prtcode = null;
            public string variable = null;
            public string expressionCs = null;
            public Func<GekkoSmpl, IVariable> expression = null;            
            public string label = null;
            public IVariable name = null;  //name given from ASTDECOMPITEMS2, is only active if DECOMP x, DECOM x[a] and the like (a name, no expression)
            public List<List<IVariable>> where = new List<List<IVariable>>();
            public List<List<IVariable>> group = new List<List<IVariable>>();            
            public List<DecompItems> decompItems = new List<DecompItems>();  //second, third etc. elements are links
            public List<IVariable> rows = new List<IVariable>();
            public List<IVariable> cols = new List<IVariable>();

            //DECOMP3 special fields
            public List<IVariable> select = new List<IVariable>();
            public List<IVariable> from = new List<IVariable>();
            public List<IVariable> endo = new List<IVariable>();

            public void Exe()
            {
                //See source code documentation
                Decomp.DecompStart(this);
            }

        }

        public class Itershow
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public List names = null;
            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                Program.Itershow(Program.GetListOfStringsFromList(this.names), this.t1, this.t2);
            }
        }

        public class Efter
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set            
            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                SolveCommon.Efter(this.t1, this.t2);
            }
        }

        public class Checkoff
        {
            public List names = null;
            public string type = null;
            public void Exe()
            {
                List<string> names2 = null;
                if (this.names != null) names2 = Program.GetListOfStringsFromList(this.names);
                Program.Checkoff(names2, type);
            }
        }

        public class Endo
        {
            public List<string> listItems = null;
            public GekkoTimes gts = null;
            public bool question = false;
            public void Exe()
            {
                if (question) Program.PrintEndoExoLists();
                else Program.Endo(this.listItems);
            }
        }

        public class Exo
        {
            public List<string> listItems = null;
            public GekkoTime t1 = GekkoTime.tNull;
            public GekkoTime t2 = GekkoTime.tNull;
            public bool question = false;
            public void Exe()
            {
                if (question) Program.PrintEndoExoLists();
                else Program.Exo(this.listItems);
            }
        }

        public class Findmissingdata
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set            
            public List names = null;
            public bool question = false;
            public double opt_replace = double.NaN;
            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                Program.FindMissingData(this);
            }
        }

        public class ForString
        {
            public List<string> listItems = null;
        }


        //public class ListDef
        //{
        //    public string name = null;
        //    public string listFile = null;
        //    public List<string> listItems = null;
        //    public string listPrefix = null;
        //    public string listSuffix = null;
        //    public string listStrip = null;
        //    public string listSort = null;
        //    public string listTrim = null;
        //    public bool direct = false;
        //    public string rawfood = null;
        //    public P p = null;
        //    public void Exe()
        //    {
        //        if (this.direct)
        //        {
        //            //Here, we get the elements from the raw text ('rawfood'), and handle them a bit like
        //            //listfiles, #(listfile ...).
        //            //If needed, LIST<direct> could be made more general, allowing all sorts of characters.
        //            List<string> input = new List<string>();
        //            string[] ss = rawfood.Split(',');
        //            input.AddRange(ss);
        //            List<string> result = new List<string>();
        //            GetRawListElements(null, input, result);
        //            this.listItems = result;
        //        }

        //        if (listPrefix != null)
        //        {
        //            for (int i = 0; i < this.listItems.Count; i++)
        //            {
        //                this.listItems[i] = listPrefix + this.listItems[i];
        //            }
        //        }
        //        if (listSuffix != null)
        //        {
        //            for (int i = 0; i < this.listItems.Count; i++)
        //            {
        //                this.listItems[i] = this.listItems[i] + listSuffix;
        //            }
        //        }
        //        if (listStrip != null)
        //        {
        //            //Maybe a STRIPPREFIX, STRIPSUFFIX could be practical, using StartsWith()/EndsWith()
        //            for (int i = 0; i < this.listItems.Count; i++)
        //            {
        //                //Case-insensitive replacing
        //                this.listItems[i] = G.ReplaceString(this.listItems[i], listStrip, "", false);
        //            }
        //        }

        //        if (G.Equal(listSort, "yes"))  //listSort = null is okay in G.equal()
        //        {
        //            //TODO: What about strings starting with "-"?
        //            //TODO: What about æøå strings, what happens??
        //            this.listItems.Sort(StringComparer.OrdinalIgnoreCase);
        //        }

        //        if (G.Equal(listTrim, "yes"))  //listTrim = null is okay in G.equal()
        //        {
        //            //Todo: if it has just been sorted, trimming is easy. But we do it the general way here.
        //            GekkoDictionary<string, bool> xx = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        //            List<string> newList = new List<string>();
        //            foreach (string s in this.listItems)
        //            {
        //                if (xx.ContainsKey(s))
        //                {
        //                    //ignore
        //                }
        //                else
        //                {
        //                    newList.Add(s);
        //                    xx.Add(s, true);
        //                }
        //            }
        //            this.listItems = newList;
        //        }

        //        Program.PutListIntoListOrListfile(this.listItems, this.name, this.listFile);

        //    }



        //    public static void Q(string s)
        //    {
        //        Program.WriteListItems(s);
        //    }
        //}

        public class Genr
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public string lhsFunction = null;
            public Series lhs = null;
            public string meta = null;
            public P p = null;
            public void Exe()
            {
                if (this.meta != null)
                {
                    //For instance, "SERIES y = 2 * x;" --> meta = "SERIES y = 2 * x" (without the semicolon)    
                    string s = ShowDatesAsString(this.t1, this.t2);
                    lhs.meta.source = s + this.meta;
                    lhs.SetDirty(true);
                }
                lhs.Stamp();
            }
        }


        //public class SeriesDef
        //{
        //    public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
        //    public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
        //    public string lhsFunction = null;
        //    public Series lhs = null;
        //    public Series rhs = null;
        //    public string meta = null;
        //    public P p = null;
        //    public void Exe()
        //    {
        //        G.CheckLegalPeriod(this.t1, this.t2);
        //        if (true)

        //        {
        //            //a full copy of the data
        //            //int i1 = -12345;
        //            //int i2 = -12345;
        //            //double[] dataPointer = ts.GetDataSequence(out i1, out i2, smpl.t1, smpl.t2);
        //            //this.lhs.storage = new double[i2 - i1 + 1];
        //            //Array.Copy(dataPointer, i1, storage, 0, (i2 - i1 + 1));
        //            //this.anchorPeriodPositionInArray = 0;
        //            //this.anchorPeriod = smpl.t1;

        //            int i = Series.FromGekkoTimeToArrayIndex(this.t1, new GekkoTime(this.rhs.freq, this.rhs.data.anchorPeriod.sub, this.rhs.data.anchorPeriod.super), this.rhs.GetAnchorPeriodPositionInArray());
        //            int n = GekkoTime.Observations(this.t1, this.t2);

        //            //TODO TODO TODO, should not be possible
        //            if (i < 0 || i >= this.rhs.data.dataArray.Length)
        //            {
        //                G.Writeln2("*** ERROR: Sample error #9876201872");
        //                throw new GekkoException();
        //            }

        //            int index1; int index2;
        //            double[] dataArray = lhs.GetDataSequenceUnsafePointerAlter(out index1, out index2, this.t1, this.t2); //Method will resize the double[] array if it is too small
        //            if (index2 - index1 + 1 != n)
        //            {
        //                G.Writeln2("*** ERROR: Sample error #9376201872");
        //                throw new GekkoException();
        //            }
        //            Array.Copy(this.rhs.data.dataArray, i, dataArray, index1, n);
        //        }

        //        if (this.meta != null)
        //        {
        //            //For instance, "SERIES y = 2 * x;" --> meta = "SERIES y = 2 * x" (without the semicolon)    
        //            string s = ShowDatesAsString(this.t1, this.t2);
        //            lhs.meta.source = s + this.meta;
        //            lhs.SetDirty(true);
        //        }
        //        lhs.Stamp();

        //    }
        //}

        public class Index
        {
            // INDEX <mute frombank=.... showbank=... showfreq=...> [type] [wildcard] to [#list]
                        
            public string opt_mute = null;            
            public string opt_bank = null;
            public string opt_showbank = null;
            public string opt_showfreq = null;
            public string type = null;
            public List names1 = null; //the wildcard(s)
            public List names2 = null; //destination list   
            public bool isCountCommand = false;

            public void Exe()
            {
                if (this.type == "ASTPLACEHOLDER") this.type = null;
                EVariableType type = EVariableType.Var;
                if (this.type != null) type = G.GetVariableType(this.type);
                
                List<string> names = Program.Search(this.names1, opt_bank, type);

                if (isCountCommand)
                {
                    G.Writeln2("Found " + names.Count + " matching items");
                }
                else
                {

                    if (G.Equal(opt_showbank, "all"))
                    {
                        for (int i = 0; i < names.Count; i++)
                        {
                            //This will add first-name (e.g. 'Work') if it is not there
                            names[i] = G.Chop_AddBank(names[i], Program.databanks.GetFirst().name);
                        }
                    }
                    else if (G.Equal(opt_showbank, "no"))
                    {
                        for (int i = 0; i < names.Count; i++)
                        {
                            //This will remove any bank
                            names[i] = G.Chop_RemoveBank(names[i]);
                        }
                    }
                    else if (G.Equal(opt_showbank, "yes") || opt_showbank == null)
                    {
                        //this is default
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: showbank must be = yes, no or all");
                        throw new GekkoException();
                    }

                    if (G.Equal(opt_showfreq, "all"))
                    {
                        for (int i = 0; i < names.Count; i++)
                        {
                            //This will add current freq (e.g. '!a') if it is not there
                            names[i] = G.Chop_AddFreq(names[i], G.GetFreq(Program.options.freq));
                        }
                    }
                    else if (G.Equal(opt_showfreq, "no"))
                    {
                        for (int i = 0; i < names.Count; i++)
                        {
                            //This will remove any freq
                            names[i] = G.Chop_RemoveFreq(names[i]);
                        }
                    }
                    else if (G.Equal(opt_showfreq, "yes") || opt_showfreq == null)
                    {
                        //this is default
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: showfreq must be = yes, no or all");
                        throw new GekkoException();
                    }

                    if (!G.Equal(this.opt_mute, "yes"))
                    {
                        if (names.Count > 0)
                        {
                            G.Writeln();
                            G.Writeln(G.GetListWithCommas(names));
                        }
                    }

                    if (this.names2 != null)
                    {
                        List<string> dest = O.Restrict(this.names2, true, true, false, false);
                        if (dest.Count > 1)
                        {
                            G.Writeln2("*** ERROR: Expected 1 item as destination list");
                            throw new GekkoException();
                        }
                        O.AddIVariableWithOverwriteFromString(dest[0], new List(names));

                        G.Writeln2("Put " + names.Count + " matching items into list " + G.TransformListfileName(dest[0]));
                    }
                    else
                    {
                        G.Writeln2("Found " + names.Count + " matching items");
                    }
                }
            }

            

        }


        public class Rebase
        {
            public List names = null;
            public GekkoTime t1 = GekkoTime.tNull;
            public GekkoTime t2 = GekkoTime.tNull;
            public string opt_prefix = null;
            public string opt_bank = null;  //obsolete
            public string opt_frombank = null;
            public string opt_tobank = null;
            public double opt_index = 100d;
            public void Exe()
            {
                if (t2.IsNull())
                {
                    t2 = t1;
                }
                G.CheckLegalPeriod(this.t1, this.t2);
                if (t1.IsNull())
                {
                    G.Writeln2("*** ERROR: The index date does not seem to exist");  //probably cannot happen
                    throw new GekkoException();
                }                
                if (t1.freq != t2.freq)
                {
                    G.Writeln2("*** ERROR: The two index dates have different frequencies");
                    throw new GekkoException();
                }
                if (t1.StrictlyLargerThan(t2))
                {
                    G.Writeln2("*** ERROR: The first date must not be later than the last date");  //probably cannot happen
                    throw new GekkoException();
                }

                List<string> listItems = Restrict(this.names, true, false, true, true);

                int counter = 0;
                int count = 0;

                Databank optionToBank_databank = null;
                if (opt_tobank != null) optionToBank_databank = Program.databanks.GetDatabank(opt_tobank, true);                

                for (int i = 0; i < listItems.Count; i++)
                {
                    //#098098q3453
                    //maybe this.opt_bank should be <frombank=...> not <bank=...>
                    //and we could have a <tobank=...>

                    string frombank = null;
                    if (this.opt_bank != null) frombank = this.opt_bank;
                    if (this.opt_frombank != null) frombank = this.opt_frombank;

                    string varnameWithBank = G.Chop_AddBank(listItems[i], frombank); //this.opt_bank can be null, no problem                 

                    IVariable iv = O.GetIVariableFromString(varnameWithBank, ECreatePossibilities.NoneReportError);

                    Series ts = iv as Series;

                    if (ts.Type() != EVariableType.Series)
                    {
                        G.Writeln2("*** ERROR: Expected series type");
                        throw new GekkoException();
                    }                    

                    GekkoTime ddate1 = t1;
                    GekkoTime ddate2 = t2;

                    if (t1.freq == EFreq.A && (ts.freq == EFreq.Q || ts.freq == EFreq.M))
                    {
                        //if a year is used for a quarterly series, q1-q4 is used.
                        ddate1 = new GekkoTime(ts.freq, t1.super, 1);
                        int end = -12345;
                        if (ts.freq == EFreq.Q)
                        {
                            end = Globals.freqQSubperiods;
                        }
                        else if (ts.freq == EFreq.M)
                        {
                            end = Globals.freqMSubperiods;
                        }
                        else
                        {
                            G.Writeln2("*** ERROR: freq error #903853245");
                            throw new GekkoException();
                        }
                        ddate2 = new GekkoTime(ts.freq, t1.super, end);
                    }

                    if (ddate1.freq != ts.freq || ddate2.freq != ts.freq)
                    {
                        G.Writeln2("*** ERROR: frequency of timeseries and frequency of period(s) do not match");
                        throw new GekkoException();
                    }

                    double sum = 0d;
                    double n = 0d;
                    foreach (GekkoTime t in new GekkoTimeIterator(ddate1, ddate2))
                    {
                        sum += ts.GetDataSimple(t);
                        n++;
                    }

                    if (G.isNumericalError(sum))
                    {
                        G.Writeln2("*** ERROR: Series " + ts.meta.parentDatabank + ":" + ts.name + " from " + ddate1.ToString() + "-" + ddate2.ToString() + " contains missing values");
                        throw new GekkoException();
                    }
                    if (sum == 0d)
                    {
                        G.Writeln2("*** ERROR: Series " + ts.meta.parentDatabank + ":" + ts.name + " from " + ddate1.ToString() + "-" + ddate2.ToString() + " sums to 0, cannot rebase");
                        throw new GekkoException();
                    }

                    Series tsNew = null;
                    if (opt_prefix != null || opt_tobank != null)  ////#098098q3453 or a tobank is set
                    {
                        Databank tobank = ts.meta.parentDatabank;
                        if (optionToBank_databank != null) tobank = optionToBank_databank;  //overriding if designated tobank is there
                                                                        
                        tsNew = ts.DeepClone(null) as Series; //parentDatabank for tsNew will be null here 
                        tsNew.name = opt_prefix + ts.name;

                        //Necessary, otherwise it only fails when trying to write the databank to file (better to catch the problem here)
                        if (!tobank.editable) Program.ProtectError("You cannot change/add a timeseries in a non-editable databank (" + tobank.name + "), see OPEN<edit> or UNLOCK");
                        if (tobank.ContainsIVariable(tsNew.name))
                        {
                            tobank.RemoveIVariable(tsNew.name);
                            counter++;
                        }
                        tobank.AddIVariable(tsNew.name, tsNew);
                    }
                    else
                    {
                        //Necessary, otherwise it only fails when trying to write the databank to file (better to catch the problem here)
                        tsNew = ts;
                        if (!tsNew.meta.parentDatabank.editable) Program.ProtectError("You cannot change/add a timeseries in a non-editable databank (" + tsNew.meta.parentDatabank.name + "), see OPEN<edit> or UNLOCK");                        
                    }                    

                    double[] data = tsNew.GetDataSequenceUnsafePointerAlterBEWARE();  //do not optionally change NaN to 0
                    for (int ii = 0; ii < data.Length; ii++)
                    {
                        //could use ts.firstPeriodPositionInArray etc., but better to do it for all since ts.ts.firstPeriodPositionInArray is not always correct
                        data[ii] = data[ii] / (sum / n) * opt_index;
                    }
                    count++;

                }
                G.Writeln2("Rebased " + count + " series");
                //if (counter > 0) G.Writeln("+++ NOTE: Prefix names replaced " + counter + " existing variables");
            }
        }

        public class Count
        {
            //Is this just an INDEX<mute>?
            public string wildCard1 = null;
            public string wildCard2 = null;  //only active if range
            public List<string> listItems = null;
            public void Exe()
            {
                G.Writeln2("Found " + this.listItems.Count + " matching items");
            }
        }

        //public class Upd
        //{
        //    public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
        //    public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
        //    //public Series lhs = null;
        //    public List<string> listItems = null;
        //    public double[] data = null;
        //    public double[] rep = null;
        //    public string op = null;
        //    public bool opDollar = false;
        //    public string opt_d = null;
        //    public string opt_p = null;
        //    public string opt_m = null;
        //    public string opt_q = null;
        //    public string opt_mp = null;
        //    public string opt_n = null;
        //    public string opt_keep = null;
        //    public string meta = null;
        //    public P p = null;
        //    public void Exe()
        //    {
        //        G.CheckLegalPeriod(this.t1, this.t2);
        //        if (this.op.EndsWith(Globals.symbolDollar))
        //        {
        //            this.opDollar = true;
        //            this.op = this.op.Substring(0, this.op.Length - 1);
        //        }                
        //        Program.Upd(this);
                
        //    }
        //}

        public class Time
        {
            public GekkoTime t1 = GekkoTime.tNull;
            public GekkoTime t2 = GekkoTime.tNull;
            public void Exe()
            {
                //G.CheckLegalPeriod(this.t1, this.t2);
                Program.Time(t1, t2);
            }
            public static void Q()
            {
                G.Writeln2("Global time is: " + G.FromDateToString(Globals.globalPeriodStart) + " to " + G.FromDateToString(Globals.globalPeriodEnd));
            }
        }

        public class TimeFilterHelper
        {
            public GekkoTime from = GekkoTime.tNull;
            public GekkoTime to = GekkoTime.tNull;
            public int step = 1;
        }

        public class TimeFilter
        {
            public List<TimeFilterHelper> timeFilterPeriods = new List<TimeFilterHelper>();         
            public void Exe()
            {
                Program.TimeFilter(this);
            }
        }

        public class Table
        {
            public class CallTableFile
            {
                public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
                public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
                public string fileName = null;
                public string opt_html = null;
                public string opt_window = null;                
                
                public List<OptString> operators = new List<OptString>();   

                public string opt_mp = null;                

                public bool menuTable = false;
                //FIXMEFIXME
                //FIXMEFIXME
                //FIXMEFIXME
                //FIXMEFIXME
                //FIXMEFIXME
                public P p = null;
                
                public void Exe() 
                {
                    G.CheckLegalPeriod(this.t1, this.t2);
                    Globals.tableOption = "n";                    
                    
                    foreach (OptString os in this.operators)
                    {
                        if (G.Equal(os.s2, "yes"))
                        {
                            Globals.tableOption = os.s1;
                            break;
                        }
                    }
                    
                    Program.databanks.GetFirst().AddIVariableWithOverwrite(Globals.symbolScalar + "__tabletimestart", new ScalarDate(this.t1));
                    Program.databanks.GetFirst().AddIVariableWithOverwrite(Globals.symbolScalar + "__tabletimeend", new ScalarDate(this.t2));

                    string tableFileName = Program.TableHelper(this.fileName, this.menuTable);

                    Globals.lastCalledMenuTable = tableFileName;
                    Program.XmlTable(tableFileName, this.opt_html, this.opt_window, p);
                    O.Run o = new O.Run();
                    o.fileName = Globals.localTempFilesLocation + "\\" + "tablecode." + Globals.defaultCommandFileExtension;
                    o.p = p;
                    Program.Run(o);
                }
            }
            
            public class SetValues
            {
                public string name = null;
                public int col;
                public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
                public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set                
                public string operator2 = null;
                public double scale = 1d;
                public string format = null;
                public List<Prt.Element> prtElements = new List<Prt.Element>();
                public void Exe() {
                    G.CheckLegalPeriod(this.t1, this.t2);
                    Program.GetTable(this.name).CurRow.SetValues(this.col, this.prtElements[0].variable[0] as Series, this.prtElements[0].variable[1] as Series, null, this.t1, this.t2, Globals.tableOption, this.operator2, this.scale, this.format);
                }
            }
        }

        public class Prt
        {
            public bool guiGraphIsRefreshing = false;
            public string guiGraphRefreshingFilename = null;
            public bool guiGraphIsLogTransform = false;            
            public string prtType = null; //PRT, MULPRT, GMULPRT, PLOT, SHEET, CLIP, 
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            //public string rows = null;            
            public List<Element> prtElements = new List<Element>();                        
            public List<OptString> operators = new List<OptString>();
            public string emfName = null;  //name of produced emf file if PLOT
                        
            public int printCsCounter = -12345;

            public string guiGraphOperator = null;  //clicking in the PLOT window
                  
            public string timefilter = null;
            //public string heading = null;
            public string opt_nomax = null;
                      
            public double opt_width = -12345;
            public double opt_dec = -12345;
            public double opt_nwidth = -12345;
            public double opt_ndec = -12345;
            public double opt_pwidth = -12345;
            public double opt_pdec = -12345;

            public double opt_ymax = double.NaN;
            public double opt_ymin = double.NaN;
            public double opt_y2max = double.NaN;
            public double opt_y2min = double.NaN;

            public string opt_title = null;
            public string opt_subtitle = null;
            public string opt_stamp = null;
            public string opt_sheet = null;
            public string opt_cell = null;
            public string opt_dates = null;
            public string opt_dump = null;
            public string opt_names = null;
            public string opt_colors = null;
            public string opt_rows = null;
            public string opt_cols = null;
            public string opt_append = null;
            public string opt_collapse = null;
            public string opt_plotcode = null; //only for PLOT
            public string opt_using = null; //only for PLOT
            public string opt_filename = null;

            public string opt_size = null;
            public string opt_font = null;
            public double opt_fontsize = double.NaN;
            public string opt_bold = null;
            public string opt_italic = null;

            public string opt_tics = null;
            public string opt_grid = null;
            public string opt_gridstyle = null;
            public string opt_key = null;
            public string opt_palette = null;
            public string opt_stack = null;
            public double opt_boxwidth = double.NaN;
            public double opt_boxgap = double.NaN;
            public string opt_separate = null;
            public GekkoTime opt_xline = GekkoTime.tNull;
            public GekkoTime opt_xlinebefore = GekkoTime.tNull;
            public GekkoTime opt_xlineafter = GekkoTime.tNull;
            public string opt_ymirror = null;
            public string opt_ytitle = null;
            public string opt_y2title = null;
            public double opt_yline = double.NaN;
            public double opt_y2line = double.NaN;
            public string opt_xzeroaxis = null;
            public string opt_x2zeroaxis = null;            
            
            public double opt_ymaxhard = double.NaN;
            public double opt_y2maxhard = double.NaN;
            public double opt_ymaxsoft = double.NaN;
            public double opt_y2maxsoft = double.NaN;
            public double opt_yminhard = double.NaN;
            public double opt_y2minhard = double.NaN;
            public double opt_yminsoft = double.NaN;
            public double opt_y2minsoft = double.NaN;

            public string opt_linetype = null;
            public string opt_dashtype = null;
            public double opt_linewidth = double.NaN;
            public string opt_linecolor = null;
            public string opt_pointtype = null;
            public double opt_pointsize = double.NaN;
            public string opt_fillstyle = null;

            public string opt_bank = null;
            public string opt_ref = null;

            public long counter = -12345;

            public string opt_split = "no";       //split "PRT x, y;" into "PRT x, PRT y;"
            public string opt_missing = null;

            public string opt_dateformat = null;
            public string opt_datetype = null;

            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);                
                OprintStart();
            }

            private void OprintStart()
            {
                //series or vals can be printed in one table. This includes array-series without indexers (these are unfolded).
                //if all (including contents in lists) are series or vals (at least one series),
                //print normally in columns
                //else print them one by one separately like separate print commands.

                //We may have this: PRT {('a', 'b')} where a and b are array-series
                //.labels2 will contain list('a', 'b'). The print could be:
                //.variable[0] will contain list(a, b).
                //a[i1], a[i2], b[j1], b[j2], b[j3]

                //Explode(): only lists at top-most level are preserved -- others are eliminated 
                //so for a prtElement, either the element is a non-List or a List (with only non-List items)
                //the number of items should correspond with .labels2.
                //After Explode() it could be:
                //prtElement0 --> with labels2 = ('a', 'b', 'c')
                //  arrayseries --> label 'a'
                //    a[i1]
                //    a[i2]
                //  List
                //    arrayseries --> label 'b'
                //      b[i1]
                //      b[i2]
                //    arrayseries --> label 'c'
                //      c[i1]
                //      c[i2]
                //      c[i3]

                bool fixProblem = true;

                bool allSeries = AllSeries();

                if (allSeries) Explode(); //unfolds any lists in the prtElements

                List<string> labelOriginal = new List<string>();

                // ---------------------------------------------------------------------------------------------
                // ----- Unfolding of array-series start -------------------------------------------------------             
                // ---------------------------------------------------------------------------------------------

                // PRT a, {#a}, s, {#s}, where #a = list including array-series and #s = list of series
                // We need to unfold the items inside the commas.
                // PRT accept an item to be a list of series, so it is a question of (a) exploding raw array-series
                // and those list elements that are array-series. All other stuff is not touched.
                // PRT a,        {#a},                               s,   {#s}
                // PRT a1, a2,   (a[x], a[y], b[x], b[y], c1, c2),   s,   (s1, s2)

                //for each comma:
                foreach (O.Prt.Element element in this.prtElements)
                {
                    labelOriginal.Add(element.labelGiven[0]);
                    List<List<MapMultidimItem>> check = new List<List<MapMultidimItem>>();
                    check.Add(new List<MapMultidimItem>());
                    check.Add(new List<MapMultidimItem>());

                    int firstVariableFoundInFirstOrRef = 0; //for each comma in PRT, counter is 1 when the first non-null variable is found (often in first, but could be in ref)

                    for (int bankNumber = 0; bankNumber < 2; bankNumber++)
                    {

                        List tempVariables = null;
                        List<string> tempLabels = null;

                        //IVariable variable = element.variable[i];
                        if (element.variable[bankNumber] != null)
                        {
                            firstVariableFoundInFirstOrRef++; //for each comma in PRT, counter is 1 when the first non-null variable is found (often in first, but could be in ref)

                            if (element.variable[bankNumber].Type() == EVariableType.Series)
                            {
                                //if (firstVariableFoundInFirstOrRef == 1) jj++;
                                if (((Series)element.variable[bankNumber]).type == ESeriesType.ArraySuper)
                                {
                                    List unfold = new List();
                                    List<string> labels = new List<string>();

                                    List<string> lbl = new List<string>();
                                    try
                                    {
                                        lbl = Program.OPrintLabels(element.labelGiven, element.labelRecordedPieces, 1, bankNumber);
                                    }
                                    catch { lbl = new List<string>(); }

                                    ExplodeArraySeriesHelper(element.variable[bankNumber] as Series, check, lbl[0], element.labelRecordedPieces, firstVariableFoundInFirstOrRef, bankNumber, unfold, labels);
                                    element.variable[bankNumber] = unfold;
                                    if (firstVariableFoundInFirstOrRef == 1) element.labelGiven = labels;
                                }
                            }
                            else if (element.variable[bankNumber].Type() == EVariableType.List)
                            {
                                int n = ((List)element.variable[bankNumber]).list.Count();
                                List<string> labelsUnfolded = null;
                                if (firstVariableFoundInFirstOrRef == 1)
                                {
                                    try
                                    {
                                        labelsUnfolded = Program.OPrintLabels(element.labelGiven, element.labelRecordedPieces, n, bankNumber);
                                    }
                                    catch { labelsUnfolded = new List<string>(); }
                                }

                                bool arraySeriesExploded = false;
                                for (int k = 0; k < n; k++)
                                {
                                    bool handled = false;
                                    IVariable subElement = (((List)element.variable[bankNumber]).list[k]);
                                    if (subElement.Type() == EVariableType.Series)
                                    {
                                        Series subElement_series = (Series)subElement;
                                        //if (firstVariableFoundInFirstOrRef == 1) jj++;
                                        if (subElement_series.type == ESeriesType.ArraySuper)
                                        {
                                            List unfold = new List();
                                            List<string> labels = new List<string>();
                                            ExplodeArraySeriesHelper(subElement_series, check, labelsUnfolded[k], element.labelRecordedPieces, firstVariableFoundInFirstOrRef, bankNumber, unfold, labels);
                                            if (tempVariables == null) tempVariables = new List();
                                            tempVariables.list.AddRange(unfold.list);
                                            if (firstVariableFoundInFirstOrRef == 1)
                                            {
                                                if (tempLabels == null) tempLabels = new List<string>();
                                                tempLabels.AddRange(labels);
                                            }
                                            handled = true;
                                            arraySeriesExploded = true;
                                        }
                                    }
                                    if (!handled)
                                    {
                                        //other types like normal series, scalars, etc.
                                        if (tempVariables == null) tempVariables = new List();
                                        tempVariables.list.Add(subElement);
                                        if (firstVariableFoundInFirstOrRef == 1)
                                        {
                                            if (tempLabels == null) tempLabels = new List<string>();
                                            if (labelsUnfolded != null && k < labelsUnfolded.Count) tempLabels.Add(labelsUnfolded[k]);
                                        }

                                    }
                                }
                                if (!arraySeriesExploded)
                                {
                                    //resetting these so we do not use these
                                    tempVariables = null;
                                    tempLabels = null;
                                }
                            }

                        }
                        if (tempVariables != null) element.variable[bankNumber] = tempVariables;
                        if (tempLabels != null) element.labelGiven = tempLabels;
                    }
                }

                // ---------------------------------------------------------------------------------------------
                // ----- Unfolding of array-series end ---------------------------------------------------------
                // ---------------------------------------------------------------------------------------------

                if (G.Equal(this.opt_split, "yes") || Program.options.print_split || !allSeries || Program.IsGmulprt(this, Program.GetPrintType(this)))
                {
                    //Some of the vars are not series or val, so not possible to print them 
                    //meaningfully in one table. One or more of the vars may be array-series (non-indexed)
                    List<Element> prtElementsRemember = this.prtElements;
                    for (int i = 0; i < prtElementsRemember.Count; i++)
                    {

                        this.prtElements = new List<Element>();
                        this.prtElements.Add(prtElementsRemember[i]); //seen from Oprint(), it looks like there is only 1 variable to print

                        if (AllSeries())  //note that here, this.prtElements contains only 1 element (the current)!
                        {
                            Program.OPrint(this, null, labelOriginal);
                        }
                        else
                        {
                            //not series (including array-series and vals)                                                        
                            if (this.prtElements[0].variable[0].Type() == EVariableType.List && ((List)this.prtElements[0].variable[0]).list.Count == 0)
                            {
                                //G.Writeln2(Program.RemoveSplitter(this.prtElements[0].labelGiven[0]));
                                G.Writeln2("[empty list]");
                            }
                            else if (this.prtElements[0].variable[0] == null || this.prtElements[0].variable[1] != null)
                            {
                                G.Writeln2("+++ WARNING: Skipped one variable for printing");
                            }
                            else
                            {
                                Program.NonSeriesHandling(this);
                            }
                        }
                        this.prtElements = prtElementsRemember;
                    }
                }
                else
                {
                    //All vars are series or val (series may be x[#i] or x[%i]).
                    Program.OPrint(this, null, labelOriginal);
                }

                if (G.Equal(prtType, "mulprt") && G.Equal(Program.options.interface_mode, "data"))
                {
                    if (Globals.modeIntendedWarning)
                    {
                        G.Writeln2("+++ WARNING: MULPRT is not intended for data mode, please use PRT (cf. the MODE command).");
                    }
                }
                                
            }

            private bool AllSeries()
            {
                bool nonSeries = false;
                int seriesCounter = 0;
                foreach (O.Prt.Element element in this.prtElements)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        IVariable x = element.variable[i];
                        if (x != null)
                        {
                            Program.AllSeriesCheckRecursive(x, ref nonSeries, ref seriesCounter);
                            if (nonSeries) break;
                        }
                    }
                }
                bool allSeries = false;
                if (!nonSeries && seriesCounter > 0) allSeries = true;
                return allSeries;
            }

            private void Explode()
            {
                foreach (O.Prt.Element element in this.prtElements)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (element.variable[i] != null)
                        {
                            if (element.variable[i].Type() == EVariableType.List)
                            {
                                element.variable[i] = O.ExplodeIvariables(element.variable[i]);
                                //now, any sub-list inside this list is gone
                            }
                            //else
                            //{
                            //    List temp = new List();
                            //    temp.Add(element.variable[i]);
                            //    element.variable[i] = temp;
                            //}
                        }
                    }
                }
            }


            private static void ExplodeArraySeriesHelper(Series tsFirst, List<List<MapMultidimItem>> check, string label2, List<O.RecordedPieces> recordedPieces, int firstVariableFoundInFirstOrRef, int bankNumber, List unfold, List<string> labels)
            {

                List<MapMultidimItem> keys = tsFirst.dimensionsStorage.storage.Keys.ToList();

                string[] domains = tsFirst.meta.domains;

                List<List<string>> restrict = null;

                if (domains != null)
                {
                    LookupSettings settings = new LookupSettings(ELookupType.RightHandSide, ECreatePossibilities.NoneReturnNull, true);
                    IVariable def = O.Lookup(null, null, null, "#default", null, null, settings, EVariableType.Var, false, null);
                    if (def != null)
                    {
                        restrict = new List<List<string>>();
                        int dimI = -1;
                        foreach (string domain in domains)
                        {
                            dimI++;

                            restrict.Add(new List<string>());

                            if (def.Type() == EVariableType.Map)
                            {
                                Map def_map = def as Map;
                                IVariable set = null; def_map.storage.TryGetValue(domain, out set);
                                if (set != null && set.Type() == EVariableType.List)
                                {
                                    try
                                    {
                                        string[] ss = Program.GetListOfStringsFromListOfIvariables((set as List).list.ToArray());
                                        restrict[dimI].AddRange(ss);
                                    }
                                    catch
                                    {
                                        G.Writeln2("*** ERROR: The map #default should contain lists of strings");
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }

                if (keys.Count == 0)
                {
                    G.Writeln2("Array-series " + G.GetNameAndFreqPretty(tsFirst.name) + " has no elements");
                    throw new GekkoException();
                }
                keys.Sort(Program.CompareMapMultidimItems);

                //List mm0 = new List();
                foreach (MapMultidimItem key in keys)
                {
                    if (restrict != null)
                    {
                        for (int i = 0; i < key.storage.Length; i++)
                        {
                            if (restrict[i].Count > 0)
                            {
                                if (restrict[i].Contains(key.storage[i], StringComparer.OrdinalIgnoreCase))
                                {
                                    //show this one
                                }
                                else
                                {
                                    //skip this one
                                    goto Flag1;
                                }
                            }
                        }
                    }
                    unfold.Add(tsFirst.dimensionsStorage.storage[key]);
                    check[bankNumber].Add(key);

                    string bankName = null;

                    bool isSimple = true;

                    foreach (char c in label2)
                    {
                        if (G.IsLetterOrDigitOrUnderscore(c) || c == ':' || c == '@' || c == Globals.freqIndicator)
                        {
                            //ok
                        }
                        else
                        {
                            isSimple = false;
                            break;
                        }
                    }

                    string blanks = " ";
                    if (isSimple) blanks = "";

                    if (firstVariableFoundInFirstOrRef == 1)
                    {
                        labels.Add(label2 + blanks + "[" + key.ToString() + "]");
                    }
                    Flag1: { };

                }
            }

            

            public static List<int> GetBankNumbers(string tableOrGraphGlobalOperator, List<string> operators)
            {               
                
                List<int> rv = new List<int>();

                //TODO: CLEAN THIS UP!!                                                
                //if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_n))
                //{
                //    rv = new List<int>(); rv.Add(0);
                //}
                if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_d))
                {
                    rv = new List<int>(); rv.Add(0);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_p))
                {
                    rv = new List<int>(); rv.Add(0);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_dp))
                {
                    rv = new List<int>(); rv.Add(0);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_r))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_rn))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_rd))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_rp))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_rdp))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_m))
                {
                    rv = new List<int>(); rv.Add(0); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_q))
                {
                    rv = new List<int>(); rv.Add(0); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_mp))
                {
                    rv = new List<int>(); rv.Add(0); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_l))
                {
                    rv = new List<int>(); rv.Add(0);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_dl))
                {
                    rv = new List<int>(); rv.Add(0);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_rl))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalOperator, Globals.operator_rdl))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else
                {
                    bool usesBase = false;
                    bool usesWork = false;
                    foreach (string operator2 in operators)  //could use break in loop, but this is not speed critical
                    {
                        if (Program.IsOperatorShortMultiplier(operator2))
                        {
                            usesWork = true;
                            usesBase = true;
                        }
                        if (Program.IsOperatorShortBase(operator2)) usesBase = true;
                        if (Program.IsOperatorShortWork(operator2)) usesWork = true;
                    }
                    
                    if (usesWork)
                    {
                        rv.Add(0);
                    }
                    if (usesBase)
                    {
                        rv.Add(1);                    
                    }                    
                }
                return rv;
            }

            //1 for Work, 2 for Base, 12 for both.
            public static List<int> CreateBankHelper(int i)
            {
                List<int> banks = new List<int>();
                if (i == 1)
                {
                    banks.Add(1);
                }
                else if (i == 2)
                {
                    banks.Add(2);
                }
                else if (i == 12)
                {
                    banks.Add(1);
                    banks.Add(2);
                }
                else throw new GekkoException();
                return banks;
            }
            public static Databank GetDatabank(string s)
            {
                Databank db = Program.databanks.GetDatabank(s);
                if (db == null)
                {
                    G.Writeln2("*** ERROR: Databank '" + s + " 'does not seem to be open");
                    throw new GekkoException();
                }
                return db;
            }            

            public class BankHelper
            {
                public Databank db;
                public bool isWork;
            }

            public class GekkoDictionaryOverwrite
            {
                GekkoDictionary<string, string> _storage = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                public string this[string key]
                {
                    get
                    {
                        return _storage[key];
                    }
                    set
                    {
                        if (_storage.ContainsKey(key)) _storage.Remove(key);
                        _storage.Add(key, value);
                    }
                }
                public bool ContainsKey(string key)
                {
                    return _storage.ContainsKey(key);
                }
                public void Clear()
                {
                    _storage.Clear();
                }
            }



            public class Element
            {
                public List<O.RecordedPieces> labelRecordedPieces = null;

                //public List labelGiven2 = null;  //this one is inputted                 
                public List<string> labelGiven = null;  //this one is created from the one above
                
                public List<string> labelOLD = null;  //unfolded labels, for instance x{#m} unfolded into xa and xb.                
                public IVariable[] variable = new IVariable[2];  //first and ref
                public string endoExoIndicator = null;
                //-- layout
                public List<OptString> operators = new List<OptString>();

                public List<string> operatorsFinal = null;
                public string operatorFinal = null;

                public int width = -12345;
                public int dec = -12345;
                public int nwidth = -12345;
                public int ndec = -12345;
                public int pwidth = -12345;
                public int pdec = -12345;

                public int widthFinal = -12345;
                public int decFinal = -12345;

                //--- plot
                public string linetype = null;
                public string dashtype = null;
                public double linewidth = double.NaN;
                public string linecolor = null;
                public string pointtype = null;
                public double pointsize = double.NaN;
                public string fillstyle = null;
                public string y2 = null;
                //--- errors
                public List<string> errors = new List<string>();

                //public double min = double.MaxValue;
                //public double max = double.MinValue;
                                
            }

            
        }


        public class PrtContainer
        {
            //Items that are unfolded via lists
            
            public IVariable[] variable = new IVariable[2];

            public string operator2 = null;
            public string label = null;
            public double min = double.MaxValue;
            public double max = double.MinValue;

            public string linetypes = null;
            public string dashtypes = null;
            public double linewidths = double.NaN;
            public string linecolors = null;
            public string pointtypes = null;
            public double pointsizes = double.NaN;
            public string fillstyles = null;
            public string y2s = null;

            public int numberOfTableRows = 0;  //used to insert M's in the file
        }

        public class Ols
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set    
            public string name = null;
            public IVariable impose = null;
            public string opt_constant = null;
            public string opt_dump = null;
            public string opt_dumpoptions = null;
            public IVariable opt_rekur = null;  //List
            public List<IVariable> expressions = null;
            public List<string> expressionsText = null;
            public List opt_xtrend = null;
            public List opt_xflat = null;
            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                Program.Ols(this);
            }            
        }

        public class Sim
        {            
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set                        
            public string opt_fix = null;
            public string opt_static = null;
            public string opt_after = null;
            public string opt_res = null;
            public P p = null;

            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                SolveCommon.Sim(this);
                if (G.Equal(Program.options.interface_mode, "data"))
                {
                    if (Globals.modeIntendedWarning)
                    {
                        G.Writeln2("+++ WARNING: SIM is not intended for data-mode (cf. MODE).");
                    }
                }
            }
        }

        public class Res
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set                        
            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                Program.Res(this);
            }
        }

        public class Model
        {
            public string fileName = null;
            public string opt_info = null;
            public string opt_gms = null;
            public string opt_dump = null;
            public IVariable opt_dep = null;
            public P p = null;
            public void Exe()
            {                
                Program.Model(this);
                if (G.Equal(Program.options.interface_mode, "data"))
                {
                    if (Globals.modeIntendedWarning)
                    {
                        G.Writeln2("+++ WARNING: MODEL is not intended for data-mode (cf. MODE).");
                    }
                }
            }
        }

        public class Local
        {
            public List names = null;
            public string opt_all = null;
            public P p = null;
            public void Exe()
            {
                List<string> vars = Restrict(this.names, false, true, false, false);
                if (this.opt_all == null)
                {
                    if (vars == null)
                    {
                        G.Writeln2("*** ERROR: No variables given");
                        throw new GekkoException();
                    }

                    foreach (string var in vars)
                    {
                        Program.databanks.localGlobal.Add(var, LocalGlobal.ELocalGlobalType.Local);
                    }
                }
                else
                {
                    if (vars != null)
                    {
                        G.Writeln2("*** ERROR: Option <all> cannot be used with a list of variables");
                        throw new GekkoException();
                    }
                    if (G.Equal(this.opt_all, "yes")) Program.databanks.localGlobal.SetAllLocal();
                }
            }
        }

        public class Global
        {
            public List names = null;
            public string opt_all = null;
            public P p = null;
            public void Exe()
            {
                List<string> vars = Restrict(this.names, false, true, false, false);
                if (this.opt_all == null)
                {
                    if (vars == null)
                    {
                        G.Writeln2("*** ERROR: No variables given");
                        throw new GekkoException();
                    }

                    foreach (string var in vars)
                    {
                        Program.databanks.localGlobal.Add(var, LocalGlobal.ELocalGlobalType.Global);
                    }

                }
                else
                {
                    if (vars != null)
                    {
                        G.Writeln2("*** ERROR: Option <all> cannot be used with a list of variables");
                        throw new GekkoException();
                    }
                    if (G.Equal(this.opt_all, "yes")) Program.databanks.localGlobal.SetAllGlobal();
                }
            }
        }

        public class Collapse
        {            
            public List lhs = null;
            public List rhs = null;
            public string type = null;
            public P p;
            public void Exe()
            {                
                Program.Collapse(this.lhs, this.rhs, this.type, this.p);                
            }
        }

        public class Interpolate
        {
            public List lhs = null;
            public List rhs = null;
            public string type = null;
            public P p;
            public void Exe()
            {
                Program.Interpolate(this.lhs, this.rhs, this.type, this.p);
            }
        }
        

        public class Edit
        {
            public string fileName = null;
            public P p = null;
            public void Exe()
            {
                
                bool cancel = false;
                if (this.fileName == "*")
                {
                    Program.SelectFile(Globals.extensionCommand, ref this.fileName, ref cancel);                    
                }
                if (cancel) return;
                string zfilename = Program.CreateFullPathAndFileName(Program.AddExtension(this.fileName, "." + Globals.extensionCommand));
                System.Diagnostics.Process.Start("notepad.exe", zfilename);
            }
        }

        public class XEdit
        {
            public string fileName = null;
            public P p = null;
            public void Exe()
            {
                bool cancel = false;
                if (this.fileName == "*")
                {
                    Program.SelectFile(Globals.extensionPlot, ref this.fileName, ref cancel);
                }
                if (cancel) return;
                string zfilename = Program.CreateFullPathAndFileName(Program.AddExtension(this.fileName, "." + Globals.extensionPlot));
                
                Process p = new Process();
                p.StartInfo.FileName = Application.StartupPath + "\\XmlNotepad\\XmlNotepad.exe";
                //NOTE: quotes added because this path may contain blanks                
                p.StartInfo.Arguments = Globals.QT + zfilename + Globals.QT;
                bool msg = false;
                bool exited = false;
                try
                {
                    p.Start();                    
                }
                catch (Exception e)
                {
                    MessageBox.Show("*** ERROR: There was a internal problem calling XmlNotepad." + G.NL + "ERROR: " + e.Message);
                    throw;
                }
                p.Close();
            }
        }

        public class Mode
        {            
            public string mode = null;            
            public void Exe()
            {                
                if (G.Equal(mode, "sim"))
                {
                    Program.options.interface_mode = "sim";
                    Program.options.databank_search = false;
                    Program.options.databank_create_auto = false;
                    Program.options.solve_data_create_auto = true;
                    G.Writeln2("OPTION interface mode = sim;");
                    G.Writeln("OPTION databank search = no;");
                    G.Writeln("OPTION databank create auto = no;");
                    G.Writeln("OPTION solve data create auto = yes;");
                }
                else if (G.Equal(mode, "data"))
                {
                    Program.options.interface_mode = "data";
                    Program.options.databank_search = true;
                    Program.options.databank_create_auto = true;
                    Program.options.solve_data_create_auto = false;
                    G.Writeln2("OPTION interface mode = data;");
                    G.Writeln("OPTION databank search = yes;");
                    G.Writeln("OPTION databank create auto = yes;");
                    G.Writeln("OPTION solve data create auto = no;");
                }
                else if (G.Equal(mode, "mixed"))
                {
                    Program.options.interface_mode = "mixed";
                    Program.options.databank_search = true;
                    Program.options.databank_create_auto = true;
                    Program.options.solve_data_create_auto = true;
                    G.Writeln2("OPTION interface mode = mixed;");
                    G.Writeln("OPTION databank search = yes;");
                    G.Writeln("OPTION databank create auto = yes;");
                    G.Writeln("OPTION solve data create auto = yes;");
                    G.Writeln("");
                    G.Writeln("Please note that this mode is more flexible, but also has more room for errors, if care ", Globals.warningColor);
                    G.Writeln("is not taken (for instance whether a variable is a model variable, or whether a variable ", Globals.warningColor);
                    G.Writeln("is from the first-position databank or stems from some other open databank).", Globals.warningColor);
                }
                else
                {
                    throw new GekkoException();
                }
                Mode.Q();
            }
            public static void Q()
            {
                CrossThreadStuff.Mode();
                G.Writeln2("Mode is set to: " + Program.options.interface_mode);                
            }
        }
        
        public class R_file
        {            
            public string fileName = null;
            public void Exe()
            {                
                Globals.r_fileContent = G.ExtractLinesFromText(Program.GetTextFromFileWithWait(this.fileName));
            }
        }

        public class R_export
        {
            public List names = null;
            public string opt_target = null;
            public void Exe()
            {
                Program.ROrPythonExport(this.names, this.opt_target, 0);
            }            
        }

        public class R_run
        {
            public string opt_mute = null;
            public string opt_target = null; //new in 3.1.8
            public string fileName = null; //new in 3.1.8
            public List names = null; //new in 3.1.8
            public void Exe()
            {
                try
                {
                    Program.RunR(this);
                }
                finally
                {
                    //Globals.r_fileContent = null;
                }
            }
        }        

        public class Python_run
        {
            public string opt_mute = null;
            public string opt_target = null; //new in 3.1.8
            public string fileName = null; //new in 3.1.8
            public List names = null; //new in 3.1.8
            public void Exe()
            {
                try
                {
                    Program.RunPython(this);
                }
                finally
                {
                    //Globals.python_fileContent = null;
                }
            }
        }

        public class Sys
        {
            public string opt_mute = null;
            public IVariable s = null;
            public void Exe()
            {            
                if (s == null)
                {
                    Process myProcess = Process.Start("cmd", "/K");
                }
                else
                {
                    string ss = O.ConvertToString(s);
                    Program.ExecuteShellCommand(ss, G.Equal(this.opt_mute, "yes"));
                }
            }
        }
        

        public class X12a
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            //public List<string> listItems = null;
            public List names = null;
            public string opt_bank = null;
            public string opt_param = null;
            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);
                Program.X12a(this);
            }
        }

        public class Write
        {
            public GekkoTime t1 = GekkoTime.tNull; //default, if not explicitely set
            public GekkoTime t2 = GekkoTime.tNull; //default, if not explicitely set
            public string fileName = null;
            public List list1 = null;
            public List list2 = null;
            //public List<string> listItems = null;
            
            public string opt_tsd = null;
            public string opt_tsdx = null;
            public string opt_gbk = null;
            public string opt_csv = null;
            public string opt_frombank = null;
            public string opt_prn = null;
            public string opt_r = null;
            public string opt_tsp = null;
            public string opt_xls = null;
            public string opt_xlsx = null;
            public string opt_gdx = null;
            public string opt_gnuplot= null;            
            public string opt_caps = null;
            public string opt_gcm = null;
            public string opt_flat = null;
            public string opt_python = null;
            public string opt_arrow = null;
            public string opt_cols = null;
            public string opt_respect = null;
            public string opt_op = null;
            public string opt_all = null;            
            public string type = null; //write or export
            public string opt_dateformat = null;
            public string opt_datetype = null;

            public void Exe()
            {
                G.CheckLegalPeriod(this.t1, this.t2);

                GekkoSmplSimple truncate = Program.HandleRespectPeriod(this.t1, this.t2, this.opt_respect, this.opt_all, this.type);
                if (truncate != null)
                {
                    this.t1 = truncate.t1;
                    this.t2 = truncate.t2;
                }

                if (false && Globals.runningOnTTComputer)
                {
                    GekkoDataFrame f = new GekkoDataFrame(123d);
                    Program.databanks.GetFirst().AddIVariable("#df", f);
                }
                
                Program.Write(this);
            }
        }

        public class Translate
        {
            public string fileName = null;
            public string opt_gekko18 = null;
            public string opt_gekko20 = null;
            public string opt_aremos = null;
            public string opt_move = null;
            public string opt_remove = null;
            public void Exe()
            {
                if (opt_aremos == null && opt_gekko18 == null && opt_gekko20 == null && opt_move == null && opt_remove == null)
                {
                    G.Writeln2("*** ERROR: Please use <gekko20>, <gekko18>, <aremos>, <move>, <remove>");
                    throw new GekkoException();
                }
                string extension = ".gcm";
                if (G.Equal(opt_aremos, "yes")) extension = ".cmd";
                string zfilename = Program.CreateFullPathAndFileName(Program.AddExtension(this.fileName, extension));
                string xx = Program.GetTextFromFileWithWait(zfilename);
                List<string> xxx = G.ExtractLinesFromText(xx);                
                if (zfilename.ToLower().EndsWith(".cmd") || zfilename.ToLower().EndsWith("." + Globals.extensionCommand)) 
                    zfilename = zfilename.Substring(0, zfilename.Length - 4);
                string zz = zfilename + "_translate." + Globals.extensionCommand;
                if (File.Exists(zz) && !Globals.runningOnTTComputer)
                {
                    G.Writeln2("*** ERROR: The destination file '" + zz + "' already exists:");                    
                    throw new GekkoException();
                }
                if (G.Equal(opt_gekko18, "yes"))
                {
                    G.Writeln2("*** ERROR: The translator from Gekko 1.8 to 2.0 is no longer available for Gekko 3.1.x.");
                    G.Writeln("*** ERROR: Please use Gekko 3.0 or 2.x.x to do this.");
                    throw new GekkoException();
                }
                else if (G.Equal(opt_gekko20, "yes"))
                {
                    string ss = Translator_Gekko20_Gekko30.Translate(xx);
                    using (FileStream fs = Program.WaitForFileStream(zz, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    {
                        sw.Write(ss);
                        sw.Flush();
                        sw.Close();
                    }
                    G.Writeln2("Translated file into: " + zz);
                    G.Writeln("Translate comments: see /* TRANSLATE: .... */");
                }
                else if (G.Equal(opt_remove, "yes"))
                {
                    string ss = Translator_Gekko20_Gekko30.Remove(xx);
                    using (FileStream fs = Program.WaitForFileStream(zz, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    {
                        sw.Write(ss);
                        sw.Flush();
                        sw.Close();
                    }
                    G.Writeln2("Translated file into: " + zz);
                    G.Writeln("Translate comments: see /* TRANSLATE: .... */");
                }
                else if (G.Equal(opt_move, "yes"))
                {
                    string ss = Translator_Gekko20_Gekko30.Move(xx);
                    using (FileStream fs = Program.WaitForFileStream(zz, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    {
                        sw.Write(ss);
                        sw.Flush();
                        sw.Close();
                    }
                    G.Writeln2("Translated file into: " + zz);
                    G.Writeln("Translate comments: see /* TRANSLATE: .... */");
                }
                else if (G.Equal(opt_aremos, "yes"))
                {
                    string ss = Translator_AREMOS_Gekko30.Translate(xx);
                    using (FileStream fs = Program.WaitForFileStream(zz, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    {
                        sw.Write(ss);
                        sw.Flush();
                        sw.Close();
                    }
                    G.Writeln2("Translated file into: " + zz);
                    G.Writeln("Translate comments: see /* TRANSLATE: .... */");
                    G.Writeln("Note that <dyn> is not automatically set in SERIES (like in the Gekko 2.0 translator).");
                }
                else
                {
                    G.Writeln2("*** ERROR: Problem in <> language type");
                    throw new GekkoException();
                }
            }
        }        
    }
}

