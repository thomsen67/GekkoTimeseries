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

    /// <summary>
    /// Contains helper methods called by dynamic code. A large chunk of the O class resides in Lookup.cs, to keep all the
    /// lookup code in one file.
    /// </summary>
    public static partial class O
    {
        //Common methods start
        //Common methods start
        //Common methods start

        //Careful, some of these are used in dynamic code
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

        /// <summary>
        /// Simple math overload
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Exp(double x)
        {
            return Math.Exp(x);
        }

        /// <summary>
        /// Simple math overload
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Abs(double x)
        {
            return Math.Abs(x);
        }

        /// <summary>
        /// /// Simple math overload
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static double Min(double x1, double x2)
        {
            return Math.Min(x1, x2);
        }

        /// <summary>
        /// /// Simple math overload
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static double Max(double x1, double x2)
        {
            return Math.Max(x1, x2);
        }

        /// <summary>
        /// Simple math overload
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Log(double x)
        {
            return Math.Log(x);
        }

        /// <summary>
        /// Simple math overload, used together with O.NewtonStartingValuesFixHelper1()
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Special_Log(double x)
        {
            if (x < 0)
            {
                double d = NewtonStartingValuesFixHelper1(x);
                return O.Log(d);
            }
            else return O.Log(x);
        }

        /// <summary>
        /// Simple math overload
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static double Pow(double x1, double x2)
        {
            //has special treatment of x^2
            return x2 == 2d ? x1 * x1 : Math.Pow(x1, x2);
        }

        /// <summary>
        /// Simple math overload, used together with O.NewtonStartingValuesFixHelper1()
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static double Special_Pow(double x1, double x2)
        {
            if (x1 < 0)
            {
                double d = NewtonStartingValuesFixHelper1(x1);
                return O.Pow(d, x2);
            }
            else return O.Pow(x1, x2);
        }

        /// <summary>
        /// Helper method to handle starting values
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
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

        //is used in dynamic code, do not remove
        public static bool isTableCall = false;

        /// <summary>
        /// Simple helper method to show dates
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for iterating a list of strings
        /// </summary>
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

        /// <summary>
        /// Add two IVariables
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IVariable Add(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return x.Add(smpl, y);
        }

        /// <summary>
        /// Add three IVariables
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static IVariable Add(GekkoSmpl smpl, IVariable x, IVariable y, IVariable z)
        {
            return x.Add(smpl, y).Add(smpl, z);
        }

        /// <summary>
        /// Subtract IVariables
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IVariable Subtract(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return x.Subtract(smpl, y);
        }

        /// <summary>
        /// Multiply IVariables
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IVariable Multiply(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return x.Multiply(smpl, y);
        }

        /// <summary>
        /// Divide IVariables
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IVariable Divide(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return x.Divide(smpl, y);
        }

        /// <summary>
        /// Used for %= operator
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IVariable Percent(GekkoSmpl smpl, IVariable x, IVariable y)
        {            
            Series x_series = x as Series;
            AssignmentError(x_series, "%= operator");
            return x.Divide(smpl, y);
        }

        /// <summary>
        /// Used for #= operator
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IVariable Hash(GekkoSmpl smpl, IVariable x, IVariable y)
        {            
            Series x_series = x as Series;
            AssignmentError(x_series, "#= operator");
            return x.Divide(smpl, y);
        }

        /// <summary>
        /// Used for ^= operator
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IVariable Hat(GekkoSmpl smpl, IVariable x, IVariable y)
        {            
            Series x_series = x as Series;            
            AssignmentError(x_series, "^= operator");
            foreach (GekkoTime t in smpl.Iterate03())
            {
                double x_lag = x_series.GetData(smpl, t.Add(-1));
            }
            return x.Divide(smpl, y);
        }

        /// <summary>
        /// Acquires an option
        /// </summary>
        /// <returns></returns>
        public static int MaxLag()
        {
            return Program.options.decomp_maxlag;
        }

        /// <summary>
        /// Acquires an option
        /// </summary>
        /// <returns></returns>
        public static int MaxLead()
        {
            return Program.options.decomp_maxlead;
        }
        
        /// <summary>
        /// The math power function
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IVariable Power(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return x.Power(smpl, y);
        }

        /// <summary>
        /// Change sign of IVariable
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static IVariable Negate(GekkoSmpl smpl, IVariable x)
        {
            return x.Negate(smpl);
        }
        
        /// <summary>
        /// A list function
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IVariable Union(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return Functions.union(smpl, null, null, x, y);
        }        

        /// <summary>
        /// A list function
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IVariable Intersect(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return Functions.intersect(smpl, null, null, x, y);
        }

        /// <summary>
        /// Replace "/" shash with "\"
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Replace "/" shash with "\"
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Special helper method for OPTION command.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Special helper method for OPTION command.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string XString(IVariable x)
        {
            string x_string = O.ConvertToString(x);
            return x_string;
        }

        /// <summary>
        /// Special helper method for OPTION command.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int XInt(IVariable x)
        {
            int x_int = O.ConvertToInt(x);
            if (x_int < 0)
            {
                G.Writeln2("*** ERROR: Expected integer >= 0, not " + x_int);
            }
            return x_int;
        }

        /// <summary>
        /// Special helper method for OPTION command.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int XSint(IVariable x)  //signed int
        {
            int x_int = O.ConvertToInt(x);            
            return x_int;
        }

        /// <summary>
        /// Special helper method for OPTION command.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double XVal(IVariable x)
        {
            double x_val = O.ConvertToVal(x);
            return x_val;
        }

        /// <summary>
        /// Special helper method for OPTION command.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string XVal2String(IVariable x)
        {
            double x_val = O.ConvertToVal(x);
            return x_val.ToString();
        }

        /// <summary>
        /// Special helper method for OPTION command.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string XNameOrString(IVariable x)
        {
            string x_string = O.ConvertToString(x);
            return x_string;
        }

        /// <summary>
        /// Special helper method for OPTION command.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string XNameOrStringOrFilename(IVariable x)
        { 
            return XNameOrString(x);
        }

        /// <summary>
        /// Special helper method for OPTION command.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static EFreq XNameOrString2Freq(IVariable x)
        {
            string x_string = O.ConvertToString(x);
            EFreq freq = G.ConvertFreq(x_string);
            return freq;
        }

        /// <summary>
        /// Special helper method for OPTION command.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static ESeriesMissing XOptionSeriesMissing(IVariable x)
        {
            string x_string = O.ConvertToString(x);
            return G.GetMissing(x_string);
        }

        // ============ helper methods for options, end

        /// <summary>
        /// Helper method regarding path
        /// </summary>
        /// <param name="fileName2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Print current options
        /// </summary>
        /// <param name="path"></param>
        public static void PrintOptions(string path)
        {
            Program.options.Write(path);
        }

        /// <summary>
        /// Used for OPTION command.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="isBlock"></param>
        /// <param name="p"></param>
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

        /// <summary>
        /// Flatten of list for FOR command
        /// </summary>
        /// <param name="isNaked"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static List FlattenIVariablesSeqFor(bool isNaked, IVariable iv)
        {
            List m = FlattenIVariablesSeq(isNaked, iv);
            m = Restrict2(m, true, false, true, true);  //no sigils
            return m;
        }

        /// <summary>
        /// Flatten IVariables (if these are lists). Will also handle naked lists.
        /// </summary>
        /// <param name="isNaked"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static List FlattenIVariablesSeq(bool isNaked, IVariable iv)
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
                m = new List(FlattenIVariablesHelper(iv));
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

        /// <summary>
        /// Used for naked lists, logic concerning values
        /// </summary>
        /// <param name="m"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Use for IVariable, -x
        /// </summary>
        /// <param name="iv"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Flatten lists
        /// </summary>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static List FlattenIVariables(IVariable iv)
        {
            return new List(FlattenIVariablesHelper(iv));
        }
        
        /// <summary>
        /// Recursive helper method
        /// </summary>
        /// <param name="iv"></param>
        /// <returns></returns>
        //is recursive
        private static List<IVariable> FlattenIVariablesHelper(IVariable iv)
        {            
            List<IVariable> temp = new List<IVariable>();
            if (iv.Type() == EVariableType.List)
            {
                foreach (IVariable temp2 in ((List)iv).list)
                {
                    if (temp2 != null && temp2.Type() == EVariableType.List)
                    {
                        List<IVariable> temp3 = FlattenIVariablesHelper(temp2);
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
        
        
        /// <summary>
        /// Helper for PRT/PLOT/SHEET
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="ope0"></param>
        public static void PrtElementHandleLabel(GekkoSmpl smpl, O.Prt.Element ope0)
        {            
            if (ope0.labelRecordedPieces == null) ope0.labelRecordedPieces = new List<RecordedPieces>();
            ope0.labelRecordedPieces.AddRange(smpl.labelRecordedPieces);
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
        
        /// <summary>
        /// Helper method for ENDO/EXO.
        /// </summary>
        /// <param name="global"></param>
        /// <param name="helper"></param>
        /// <param name="type"></param>
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

        /// <summary>
        /// Helper method for ENDO/EXO
        /// </summary>
        /// <param name="type"></param>
        public static void SetEndoExo(bool type)
        {

            Databank databank = Program.databanks.GetFirst();

            string endoOrExoPrefix = "endo"; if (!type) endoOrExoPrefix = "exo";

            //Clear all endo_ or exo_ variables            

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
                List<string> vars = Stringlist.GetListOfStringsFromList(h.varname);  //emits error if not string or list

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
                        string varNameWithFreq = varNameWithoutFreq + Globals.freqIndicator + G.ConvertFreq(Program.options.freq);

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

                                MultidimItem mmi = new MultidimItem(ss2.ToArray(), ts);
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

        /// <summary>
        /// Helper for 'bank' or 'ref' option
        /// </summary>
        /// <param name="opt_bank"></param>
        /// <param name="opt_ref"></param>
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

        /// <summary>
        /// Handle missing values, also in array-series
        /// </summary>
        /// <param name="s"></param>
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

        /// <summary>
        /// Helper for 'bank' and 'ref' option
        /// </summary>
        public static void HandleOptionBankRef2()
        {
            Program.databanks.optionBank = null;
            Program.databanks.optionRef = null;
        }

        /// <summary>
        /// Helper for missing values
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="r3"></param>
        public static void HandleMissing2(ESeriesMissing r1, ESeriesMissing r2, ESeriesMissing r3)
        {
            Program.options.series_array_print_missing = r1;
            Program.options.series_array_calc_missing = r2;
            Program.options.series_data_missing = r3;
        }
        
        /// <summary>
        /// Iteration, next step forwards.
        /// </summary>
        /// <param name="isYear"></param>
        /// <param name="loopType"></param>
        /// <param name="x"></param>
        /// <param name="start"></param>
        /// <param name="step"></param>
        /// <param name="counter"></param>
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
                    IVariable item = start_list.list[counter];
                    x = item.DeepClone(null);  //necessary to clone?? Note sure... but safest to do
                }
            }         
        }

        /// <summary>
        /// Helper method for "terminal" condition in forward-looking models.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static double Lead(double[] b, int i)
        {
            //int x = Program.model.modelGekko.m2.fromBNumberToEqNumber[i];
            //BTypeData data = null; model.varsBType.TryGetValue("y" + Globals.lagIndicator + "1", out data);
            //Program.model.modelGekko.m2.from
            //G.Writeln(x);

            double v = double.NaN;

            int type = 1;  //0 "exo" or "forward method none", 1 const, 2 growth

            if (Program.model.modelGekko.simulateResults[8] == 0d)
            {
                v = b[i];
            }
            else if (Program.model.modelGekko.simulateResults[8] == 1d)  //#375204390457
            {
                if (Program.model.modelGekko.terminalHelper == null)
                {
                    //This will switch off the smart terminal stuff, and perforn NFT
                    //just like in the old days.
                    //That typically means a lot of more iterations for terminal CONST,
                    //whereas terminal EXO is not affected.
                    v = b[i];  //use the normal one
                }
                else
                {
                    int distance = (int)Program.model.modelGekko.simulateResults[7];
                    int newI = -12345;

                    if (Program.model.modelGekko.terminalHelper.Count > distance)
                    {
                        Program.model.modelGekko.terminalHelper[distance].TryGetValue(i, out newI);
                    }

                    if (newI != -12345)
                    {
                        //found pointing to a period outside sim period, so we use another b[i]
                        v = b[newI];
                        //G.Writeln("used b[" + newI + "] " + b[newI] + " instead of real lead b[" + i + "] " + b[i] + ", distance " + distance);
                    }
                    else
                    {
                        //just use the normal one
                        v = b[i];
                    }
                }
            }
            else if (Program.model.modelGekko.simulateResults[8] == 2d)
            {
                G.Writeln2("*** ERROR: terminal 'growth' does not work at the moment");
                throw new GekkoException();
            }
            else throw new GekkoException();
            return v;
        }
        
        /// <summary>
        /// Start of iterator.
        /// </summary>
        /// <param name="isYear"></param>
        /// <param name="loopType"></param>
        /// <param name="x"></param>
        /// <param name="start"></param>
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

        /// <summary>
        /// Helper for FOR loops.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isForToOrListType"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper for FOR loop iteration.
        /// </summary>
        /// <param name="isYear"></param>
        /// <param name="isForToOrListType"></param>
        /// <param name="x"></param>
        /// <param name="start"></param>
        /// <param name="max"></param>
        /// <param name="step"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Small helper method.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static GekkoTimes HandleDates(GekkoTime t1, GekkoTime t2)
        {
            GekkoTimes gts = new GekkoTimes();
            gts.t1 = t1;
            gts.t2 = t2;
            return gts;
        }

        /// <summary>
        /// Overload.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        public static void HandleIndexer(IVariable y, params IVariable[] x)
        {
            HandleIndexerHelper(0, y, x);
        }

        /// <summary>
        /// Used to handle time periods and timeseries expressions
        /// </summary>
        /// <returns></returns>
        public static GekkoSmpl Smpl()
        {
            return new GekkoSmpl(Globals.globalPeriodStart, Globals.globalPeriodEnd);
        }

        /// <summary>
        /// LIST definition.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static List ListDefHelper(params IVariable[] x)
        {
            return ListDefHelper2(x);
        }

        /// <summary>
        /// List definition.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// True if a series x is of timeless type.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsTimelessSeries(IVariable x)
        {
            bool rv = false;
            Series x_series = x as Series;
            if (x_series != null && x_series.type == ESeriesType.Timeless) rv = true;
            return rv;
        }

        /// <summary>
        /// PAUSE command.
        /// </summary>
        /// <param name="arg"></param>
        public static void Pause(string arg)
        {
            arg = Program.HandleNewlines(arg);
            if (arg.Length > 0)
            {
                G.Writeln();
                G.Writeln(arg);
            }
            if (arg.Length > 0) arg += "\n" + "\n";
            arg += "Press [Enter] to continue";
            MessageBox.Show(arg);
        }

        /// <summary>
        /// INI command, and running gekko.ini.
        /// </summary>
        /// <param name="p"></param>
        public static void Ini(P p)
        {
            string s = "gekko.ini";

            List<string> folders = new List<string>();
            folders.Add(G.GetProgramDir());
            string fileName2 = Program.FindFile(s, folders, false);  //also calls CreateFullPathAndFileName()
            if (fileName2 == null)
            {
                G.Writeln2("No INI file '" + Globals.autoExecCmdFileName + "' found in program folder");
            }
            else
            {
                Globals.cmdPathAndFileName = fileName2;  //always contains a path, is used if there is a lexer error
                Globals.cmdFileName = Path.GetFileName(Globals.cmdPathAndFileName);
                Program.RunGekkoCommands("", fileName2, 0, p);
                G.Writeln();
                G.Writeln("Finished running INI file ('" + Path.GetFileName(Globals.cmdPathAndFileName) + "') from program folder");
            }

            folders = new List<string>();
            folders.Add(Program.options.folder_command);
            folders.Add(Program.options.folder_command1);
            folders.Add(Program.options.folder_command2);
            fileName2 = Program.FindFile(s, folders, true);  //also calls CreateFullPathAndFileName()
            if (fileName2 == null)
            {
                G.Writeln2("No INI file '" + Globals.autoExecCmdFileName + "' found in working folder");
                return;  //used for gekko.ini file
            }
            else
            {
                Globals.cmdPathAndFileName = fileName2;  //always contains a path, is used if there is a lexer error
                Globals.cmdFileName = Path.GetFileName(Globals.cmdPathAndFileName);
                Program.RunGekkoCommands("", fileName2, 0, p);
                G.Writeln();
                G.Writeln("Finished running INI file ('" + Path.GetFileName(Globals.cmdPathAndFileName) + "') from working folder");
            }
        }

        /// <summary>
        /// CLS command.
        /// </summary>
        /// <param name="tab"></param>
        public static void Cls(string tab)
        {
            CrossThreadStuff.Cls(tab);
        }

        /// <summary>
        /// Used for TABLE.
        /// </summary>
        /// <param name="name"></param>
        public static void CreateNewTable(string name)
        {
            if (Program.tables.ContainsKey(name))
            {
                Program.tables.Remove(name);
            }
            Gekko.Table temp = new Gekko.Table();
            temp.type = "table"; //not a "print" type table -- relevant regarding formatting
            Program.tables.Add(name, temp);
        }

        /// <summary>
        /// Helper for TABLE command.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Gekko.Table GetTable(string name)
        {
            Gekko.Table xx = null;
            if (Program.tables.TryGetValue(name, out xx))
            {
            }
            else
            {
                G.Writeln2("*** ERROR: Table '" + name + "' does not seem to exist");
            }
            return xx;
        }

        /// <summary>
        /// Helper for TABLE, printing
        /// </summary>
        /// <param name="tab"></param>
        public static void PrintTable(Gekko.Table tab)
        {
            PrintTable(tab, true, null);
        }

        /// <summary>
        /// Helper for TABLE, printing
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="type"></param>
        public static void PrintTable(Gekko.Table tab, string type)
        {
            PrintTable(tab, true, type);
        }

        /// <summary>
        /// Helper for TABLE, printing
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="printDateEtc"></param>
        /// <param name="printType"></param>
        public static void PrintTable(Gekko.Table tab, bool printDateEtc, string printType)
        {
            printType = Program.PrintTableHelper(tab, printDateEtc, printType);
        }

        /// <summary>
        /// UNFIX command.
        /// </summary>
        public static void Unfix()  //formerly ClearGoals()
        {
            if (G.Equal(Program.options.model_type, "gams"))
            {
                Unfix(Program.databanks.GetFirst(), "endo");
                Unfix(Program.databanks.GetFirst(), "exo");
            }
            else
            {
                if (G.HasModelGekko())
                {
                    if (Program.model.modelGekko.exogenized.Count == 0 && Program.model.modelGekko.endogenized.Count == 0)
                    {
                        G.Writeln2("No goals are set, so nothing to unfix");
                    }
                    else
                    {
                        string s = "Unfixed/cleared ";
                        if (Program.model.modelGekko.exogenized != null)
                        {
                            s += Program.model.modelGekko.exogenized.Count + " EXO and ";
                        }
                        if (Program.model.modelGekko.endogenized != null)
                        {
                            s += Program.model.modelGekko.endogenized.Count + " ENDO variables.";
                        }
                        Program.Endo(null);  //--> better than clearing as above, since hasBeenEndoExoStatementsSinceLastSim flag is set
                        Program.Exo(null);
                        G.Writeln2(s);
                        G.Writeln("Please note that only SIM<fix> (and not SIM) enforces the ENDO/EXO goals");
                    }
                }
                else
                {
                    G.Writeln2("No model defined -- not possible to clear/unfix goals");
                }
            }
        }

        /// <summary>
        /// Helper for O.Unfix().
        /// </summary>
        /// <param name="databank"></param>
        /// <param name="endoOrExoPrefix"></param>
        public static void Unfix(Databank databank, string endoOrExoPrefix)
        {
            List<string> delete = new List<string>();
            foreach (KeyValuePair<string, IVariable> kvp in databank.storage)
            {
                if (kvp.Key.StartsWith(endoOrExoPrefix + "_", StringComparison.OrdinalIgnoreCase) && kvp.Key.EndsWith(Globals.freqIndicator + G.ConvertFreq(Program.options.freq), StringComparison.OrdinalIgnoreCase))
                {
                    //starts with endo_ or exo_ and is of annual type
                    delete.Add(kvp.Key);
                }
            }
            int count = 0;
            foreach (string s in delete)
            {
                databank.RemoveIVariable(s);
                count++;
            }
            if (count > 0) G.Writeln2("Removed " + count + " " + endoOrExoPrefix + "_... variables");
        }        

        /// <summary>
        /// Used when the user changes frequency, printing info on this.
        /// </summary>
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

        /// <summary>
        /// Open up the help system.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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

        /// <summary>
        /// CUT command.
        /// </summary>
        public static void Cut()
        {
            Program.Cut(true);
        }

        /// <summary>
        /// TELL command.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="nocr"></param>
        public static void Tell(string text, bool nocr)
        {
            if (Globals.runningOnTTComputer && text == "arrow")
            {
                Arrow.Run();
            }
            
            if (nocr) G.Write(text);
            else G.Writeln(text);
        }

        /// <summary>
        /// EXIt command.
        /// </summary>
        public static void Exit()
        {
            Globals.applicationIsInProcessOfAborting = true;
            Globals.threadIsInProcessOfAborting = true;
            throw new GekkoException();
        }

        /// <summary>
        /// HDG command.
        /// </summary>
        /// <param name="text"></param>
        public static void Hdg(string text)
        {
            if (text.EndsWith(";")) text = text.Substring(0, text.Length - 1);  //Should be HDG 'text'; fixing it here
            Program.databanks.GetFirst().info1 = text;
            Program.databanks.GetFirst().isDirty = true;
            G.Writeln2("Databank heading for '" + Program.databanks.GetFirst().name + "' databank set to: '" + text + "'");
        }

        // #lkjadfaslkfja
        //public static void ListQuestion(string type)
        //{

        //    bool hasLargeModel = Program.IsLargeModel();
        //    List<string> a4 = new List<string>();
        //    foreach (KeyValuePair<string, IVariable> kvp in Program.databanks.GetFirst().storage)
        //    {
        //        if (kvp.Value.Type() == EVariableType.List)
        //        {
        //            string s = kvp.Key.Substring(1);
        //            a4.Add(s);
        //        }
        //    }

        //    a4.Sort(StringComparer.InvariantCultureIgnoreCase);  //invariant is better for sorting than ordinal

        //    List<string> user = new List<string>();
        //    List<string> system = new List<string>();
        //    foreach (string m in a4)
        //    {
        //        if (
        //        G.Equal(m, "exod") ||
        //        G.Equal(m, "exoj") ||
        //        G.Equal(m, "exoz") ||
        //        G.Equal(m, "exodjz") ||
        //        G.Equal(m, "exo") ||
        //        G.Equal(m, "exotrue") ||
        //        G.Equal(m, "endo") ||
        //        G.Equal(m, "all"))
        //        {
        //            system.Add(m);
        //        }
        //        else
        //        {
        //            user.Add(m);
        //        }
        //    }

        //    int count = a4.Count;

        //    if (type == "?")
        //    {
        //        G.Writeln();
        //        G.Write("There are " + user.Count + " user lists and " + system.Count + " model lists.");
        //        if (system.Count > 0)
        //        {
        //            G.Write(" Click ");
        //            G.WriteLink("here", "list:?_show_all_lists");
        //            G.Write(" to see model lists.");
        //        }
        //        G.Writeln();
        //        if (user.Count > 0)
        //        {
        //            foreach (string m in user)
        //            {
        //                Program.WriteListItems(m);
        //            }
        //        }
        //    }
        //    else //must be ?_show_all_lists
        //    {
        //        if (hasLargeModel) G.Writeln();
        //        foreach (string m in system)
        //        {
        //            if (hasLargeModel)
        //            {
        //                List<string> a1 = Stringlist.GetListOfStringsFromList(Program.databanks.GetFirst().GetIVariable(Globals.symbolCollection + m));
        //                G.Write("list #" + m + " = ["); G.WriteLink("show", "list:?_" + m); G.Writeln("]  (" + a1.Count + " elements from '" + a1[0] + "' to '" + a1[a1.Count - 1] + "')");
        //                G.Writeln();
        //            }
        //            else
        //            {
        //                Program.WriteListItems(m);
        //            }
        //        }
        //    }
        //}
        //public static void SeriesQuestion()
        //{
        //    foreach (Databank bank in Program.databanks.storage)
        //    {
        //        int a = 0;
        //        int q = 0;
        //        int m = 0;
        //        int u = 0;
        //        if (bank.storage.Count == 0)
        //        {
        //            G.Writeln2("Databank " + bank.name + " is empty");
        //            continue;
        //        }
        //        foreach (Series ts in bank.storage.Values)
        //        {
        //            if (ts.freq == EFreq.A) a++;
        //            else if (ts.freq == EFreq.Q) q++;
        //            else if (ts.freq == EFreq.M) m++;
        //            else if (ts.freq == EFreq.U) u++;
        //        }
        //        G.Writeln2("Databank " + bank.name + ":");
        //        if (a > 0) G.Writeln("  " + a + " annual timeseries");
        //        if (q > 0) G.Writeln("  " + q + " quarterly timeseries");
        //        if (m > 0) G.Writeln("  " + m + " monthly timeseries");
        //        if (u > 0) G.Writeln("  " + u + " undated timeseries");
        //    }
        //}

        /// <summary>
        /// MEM command. Note out-commended code that might be of use (#lkjadfaslkfja).
        /// </summary>
        /// <param name="tpe"></param>
        public static void Mem(string tpe)
        {
            //call with null, string, date, val --> will be lower-case when called

            //regarding list? or series?, see #lkjadfaslkfja, can maybe be of reuse

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
                        s += kvp.Value + " (" + G.GetFreqPretty(kvp.Key) + "), ";
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

        /// <summary>
        /// SIGN command.
        /// </summary>
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

        /// <summary>
        /// Read a listfile reference like #(listfile m).
        /// </summary>
        /// <param name="varname"></param>
        /// <returns></returns>
        private static List ReadListFile(string varname)
        {
            string fileName = varname.Substring((Globals.symbolCollection + Globals.listfile + "___").Length);
            fileName = G.AddExtension(fileName, "." + "lst");
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

        /// <summary>
        /// Helper method for choosing editor type in main Gekko GUI.
        /// </summary>
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
        
        /// <summary>
        /// Helper for Global and Local databank. Used in Program.SearchFromTo() searching hub.
        /// </summary>
        /// <param name="lg"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method can remove an IVarible given as a name as a string (like "b2:x!q").
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="reportError"></param>
        /// <returns></returns>
        public static IVariable RemoveIVariableFromString(string fullname, bool reportError)
        {
            string dbName, varName, freq; string[] indexes;
            O.Chop(fullname, out dbName, out varName, out freq, out indexes);
            IVariable iv = O.RemoveIVariableFromString(dbName, varName, freq, indexes, reportError);
            return iv;
        }

        /// <summary>
        /// Overload.
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="iv"></param>
        public static void AddIVariableWithOverwriteFromString(string fullname, IVariable iv)
        {
            string dbName, varName, freq; string[] indexes;

            if (Program.IsListfileArtificialName(fullname))
            {
                dbName = null;
                varName = fullname;
                freq = null;
                indexes = null;
            }
            else
            {
                O.Chop(fullname, out dbName, out varName, out freq, out indexes);
            }
            AddIVariableWithOverwriteFromString(dbName, varName, freq, indexes, iv);
        }

        /// <summary>
        /// This method can overwrite an IVarible given as a string (like "b2:x!q").
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="varName"></param>
        /// <param name="freq"></param>
        /// <param name="indexes"></param>
        /// <param name="iv"></param>
        public static void AddIVariableWithOverwriteFromString(string dbName, string varName, string freq, string[] indexes, IVariable iv)
        {
            if (Program.IsListfileArtificialName(varName))
            {
                //quick handling of this special case, and return afterwards
                //relevant for instance in INDEX * to #(listfile c:\tools\m.lst);
                O.WriteListFile(varName, iv);
                return;
            }

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

                    MultidimItem mmi = new MultidimItem(indexes);

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

        /// <summary>
        /// Deals with {'...'} ?
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for DECOMP eval function.
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static IVariable DecompLooper(string fullname)
        {
            IVariable iv = GetIVariableFromString(fullname, ECreatePossibilities.NoneReportError, true);            
            return iv;
        }

        /// <summary>
        /// Overload.
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IVariable GetIVariableFromString(string fullname, ECreatePossibilities type)
        {            
            return GetIVariableFromString(fullname, type, false);  //no searching per default
        }

        /// <summary>
        /// Get an IVariable from a string name like "b1:x!q".
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="type"></param>
        /// <param name="canSearch"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper method for the PREDICT command.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gt"></param>
        /// <param name="d"></param>
        public static void PredictSetValue(string name, GekkoTime gt, double d)
        {
            IVariable iv = O.GetIVariableFromString(name + Globals.freqIndicator + G.ConvertFreq(Program.options.freq), O.ECreatePossibilities.Can, false);
            Series ts = iv as Series;
            ts.SetData(gt, d);
        }

        /// <summary>
        /// Helper method for the PREDICT command.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gt"></param>
        /// <returns></returns>
        public static double PredictGetValue(string name, GekkoTime gt)
        {
            //We do not allow searching of vars in databanks
            IVariable iv = O.GetIVariableFromString(name + Globals.freqIndicator + G.ConvertFreq(Program.options.freq), O.ECreatePossibilities.NoneReturnNull, false);
            if (iv == null)
            {
                G.Writeln2("*** ERROR: PREDICT: Series '" + name + "' does not exist in first-position databank");
                throw new GekkoException();
            }
            Series ts = iv as Series;
            return ts.GetDataSimple(gt);
        }

        /// <summary>
        /// ACCEPT command.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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



        /// <summary>
        /// ACCEPT command.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper method to prompt for values in a procedure.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper method to prompt for values in a procedure.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="defaultValue"></param>
        /// <param name="type"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Remove an IVariable from a string name like "b1:x!q".
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="varName"></param>
        /// <param name="freq"></param>
        /// <param name="indexes"></param>
        /// <param name="reportError"></param>
        /// <returns></returns>
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

                    MultidimItem mmi = new MultidimItem(indexes);

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

        /// <summary>
        /// Looks at a list of variables (naked list) and restricts their types or indexes. Returns a Gekko List.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="allowBank"></param>
        /// <param name="allowSigil"></param>
        /// <param name="allowFreq"></param>
        /// <param name="allowIndexes"></param>
        /// <returns></returns>
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

        /// <summary>
        /// /// Looks at a list of variables (naked list) and restricts their types or indexes. Returns a C# list of strings.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="allowBank"></param>
        /// <param name="allowSigil"></param>
        /// <param name="allowFreq"></param>
        /// <param name="allowIndexes"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper method for O.Restrict() and O.Restrict2()
        /// </summary>
        /// <param name="allowBank"></param>
        /// <param name="allowSigil"></param>
        /// <param name="allowFreq"></param>
        /// <param name="allowIndexes"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
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

            if (Program.IsListfileArtificialName(s)) return s;  //do not chew on an artificial "listfile___..." name, used for #(listfile m) kinds of IVariables

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

        /// <summary>
        /// This method is called before each Gekko command is run (sets up GekkoSmpl object).
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="p"></param>
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

        /// <summary>
        /// Helper method regarding &lt;dyn&gt; option for series
        /// </summary>
        /// <param name="smpl"></param>
        public static void Dynamic1(GekkoSmpl smpl)
        {
            smpl.lhsAssignmentType = assignmantTypeLhs.Active;  //charged
        }

        /// <summary>
        /// Helper method regarding &lt;dyn&gt; option for series
        /// </summary>
        /// <param name="smpl"></param>
        /// <returns></returns>
        public static bool Dynamic2(GekkoSmpl smpl)
        {
            bool rv = false;
            if (smpl.lhsAssignmentType == assignmantTypeLhs.Series) rv = true;
            smpl.lhsAssignmentType = assignmantTypeLhs.Inactive;  //inactive            
            return rv;
        }

        /// <summary>
        /// Helper method regarding &lt;dyn&gt; option for series
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static bool CheckForDynamicSeries(IVariable i1, IVariable i2)
        {
            return i1.Type() == EVariableType.Series && !G.Chop_HasSigil(i2.ConvertToString());
        }

        /// <summary>
        /// Helper method regarding &lt;dyn&gt; option for series
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="assign_20"></param>
        /// <param name="check_20"></param>
        /// <param name="o"></param>
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

        /// <summary>
        /// Helper method regarding &lt;dyn&gt; option for series
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        //See also CheckDyn2()
        private static bool CheckDyn1(Assignment o)
        {
            //If this is true, and there are series on both sides of '=',
            //the assignment will be run period for period
            //It is true if either inside BLOCK series dyn = yes, or if using <dyn> local option.
            return Program.options.series_dyn == true || G.Equal(o.opt_dyn, "yes");
        }

        /// <summary>
        /// Helper method regarding &lt;dyn&gt; option for series
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
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

        /// <summary>
        /// For OPTION series failsafe, used for debugging Gekko programs.
        /// </summary>
        /// <param name="lhs_series"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
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

        /// <summary>
        /// Write a #(listfile m) kind of "variable", if it is on the left-hand side (LHS).
        /// </summary>
        /// <param name="varnameWithFreq"></param>
        /// <param name="rhs"></param>
        public static void WriteListFile(string varnameWithFreq, IVariable rhs)
        {
            //see also #98037532985

            string file = varnameWithFreq.Substring((Globals.symbolCollection + Globals.listfile + "___").Length);
            //List<string> temp = Stringlist.GetListOfStringsFromList(rhs);
            
            file = G.AddExtension(file, "." + "lst");
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

        // =====================================================================
        //                   Operator helper start, operators like y <p>= 5, y += 1, pch(y) = 5, etc.
        // =====================================================================

        /// <summary>
        /// Helper method regarding operators like: y &lt;p&gt;= 5, y += 1, pch(y) = 5, etc.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="lhs_series"></param>
        /// <param name="rhs_series"></param>
        /// <param name="operatorType"></param>
        private static void OperatorHelperSeries(GekkoSmpl smpl, Series lhs_series, Series rhs_series, ESeriesUpdTypes operatorType)
        {
            double[] rhsData, lhsData, lhsDataOriginal; //int offset = 1;
            OperatorHelper1(smpl, lhs_series, rhs_series, double.NaN, out lhsData, out lhsDataOriginal, out rhsData);
            OperatorHelper2(smpl, lhs_series.freq, operatorType, lhsData, lhsDataOriginal, rhsData);
            lhs_series.SetDataSequence(smpl.t1, smpl.t2, lhsData, Globals.smplOffset);
        }

        /// <summary>
        /// Helper method regarding operators like: y &lt;p&gt;= 5, y += 1, pch(y) = 5, etc.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="lhs_series"></param>
        /// <param name="rhsData"></param>
        /// <param name="operatorType"></param>
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

        /// <summary>
        /// Helper method regarding operators like: y &lt;p&gt;= 5, y += 1, pch(y) = 5, etc.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="lhs_series"></param>
        /// <param name="operatorType"></param>
        /// <param name="d"></param>
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

        /// <summary>
        /// Helper method regarding operators like: y &lt;p&gt;= 5, y += 1, pch(y) = 5, etc.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="lhs_series"></param>
        /// <param name="rhs_series"></param>
        /// <param name="rhs_scalar"></param>
        /// <param name="lhsData"></param>
        /// <param name="lhsDataOriginal"></param>
        /// <param name="rhsData"></param>
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

        /// <summary>
        /// Helper method regarding operators like: y &lt;p&gt;= 5, y += 1, pch(y) = 5, etc.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="lhs_series"></param>
        /// <param name="lhsData"></param>
        /// <param name="lhsDataOriginal"></param>
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

        /// <summary>
        /// Helper method regarding operators like: y &lt;p&gt;= 5, y += 1, pch(y) = 5, etc.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="lhs_series_freq"></param>
        /// <param name="operatorType"></param>
        /// <param name="lhsData"></param>
        /// <param name="lhsDataOriginal"></param>
        /// <param name="rhsData"></param>
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


        // =====================================================================
        //                   Operator helper end
        // =====================================================================

        // ==========================================================================================
        // ======================== flex freq start =================================================
        // ==========================================================================================

        /// <summary>
        /// Deals with flexible frequencies, for instance what to do with x!a &lt;2001q2 2003q3&gt; = 1, 2, 3;
        /// If there are problems with flexible freqs, these methods can be used for tracking. 
        /// See also O.FlexFreq() and O.Helper_Convert12(). [1 of 3].
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="desiredFreq"></param>
        /// <param name="t0"></param>
        /// <param name="t3"></param>
        public static void Helper_Convert03(GekkoSmpl smpl, EFreq desiredFreq, out GekkoTime t0, out GekkoTime t3)
        {
            //1 of 3
            //This method is special compared to Helper_Convert12. The thing is that Helper_Convert03 handles the lag problem,
            //and we have to deal with flexible freqs here.
            //This is just to keep the fleible freq stuff assembled in one place
            //Flexible freq stuff is for instance x!a <2001q2 2003q3> = 1, 2, 3;
            //If there are problems with flexible freqs, these methods can be used for tracking
            //See also O.UseFlexFreq() and Helper_Convert12()
            GekkoTime.Convert03(smpl, desiredFreq, out t0, out t3);
        }

        /// <summary>
        /// /// Deals with flexible frequencies, for instance what to do with x!a &lt;2001q2 2003q3&gt; = 1, 2, 3;
        /// If there are problems with flexible freqs, these methods can be used for tracking. 
        /// See also O.UseFlexFreq() and O.Helper_Convert03(). [2 of 3].
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="desiredFreq"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public static void Helper_Convert12(GekkoSmpl smpl, EFreq desiredFreq, out GekkoTime t1, out GekkoTime t2)
        {
            //2 of 3
            //This is just to keep the fleible freq stuff assembled in one place
            //Flexible freq stuff is for instance x!a <2001q2 2003q3> = 1, 2, 3;
            //If there are problems with flexible freqs, these methods can be used for tracking
            //See also O.FlexFreq() and Helper_Convert03()
            GekkoTime.Convert12(smpl, desiredFreq, out t1, out t2);
        }

        /// <summary>
        /// /// Deals with flexible frequencies, for instance what to do with x!a &lt;2001q2 2003q3&gt; = 1, 2, 3;
        /// If there are problems with flexible freqs, these methods can be used for tracking. 
        /// See also O.Helper_Convert03() and O.Helper_Convert12(). [3 of 3].
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="freq"></param>
        /// <returns></returns>
        public static bool UseFlexFreq(GekkoTime gt, EFreq freq)
        {
            //3 of 3
            //This is just to keep the fleible freq stuff assembled in one place
            //See also Helper_Convert12() and Helper_Convert03()
            return gt.freq != freq;
        }

        // ==========================================================================================
        // ======================== flex freq end ===================================================
        // ==========================================================================================

        /// <summary>
        /// Helper regarding GekkoSmpl object
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="i"></param>
        public static void AdjustT0(GekkoSmpl smpl, int i)
        {
            smpl.t0 = smpl.t0.Add(i);            
        }

        /// <summary>
        /// Put in an IVariable with overwrite if it is already there.
        /// </summary>
        /// <param name="ib"></param>
        /// <param name="varnameWithFreq"></param>
        /// <param name="removeFirstBeforeAdding"></param>
        /// <param name="lhsNew"></param>
        private static void AddIvariableWithOverwrite(IBank ib, string varnameWithFreq, bool removeFirstBeforeAdding, IVariable lhsNew)
        {
            if (removeFirstBeforeAdding) ib.RemoveIVariable(varnameWithFreq);
            ib.AddIVariable(varnameWithFreq, lhsNew);
        }

        /// <summary>
        /// For array-series this method makes it possible to write for instance
        /// x[#a+1] or x[#a-1], where #a could be an age (integer, stored as a string).
        /// In GAMS, this can also be done, but in GAMS there is no interpretation and conversion, 
        /// it just take the previous or next element from the set.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="minus"></param>
        /// <returns></returns>
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

        /// <summary>
        /// How does this relate to the G.Chop...() methods?
        /// </summary>
        /// <param name="s"></param>
        /// <param name="name"></param>
        /// <param name="rest"></param>
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

        /// <summary>
        /// How does this relate to the G.Chop...() methods?
        /// </summary>
        /// <param name="input2"></param>
        /// <param name="freq"></param>
        /// <param name="varName"></param>
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

        /// <summary>
        /// /// How does this relate to the G.Chop...() methods?
        /// </summary>
        /// <param name="input2"></param>
        /// <param name="dbName"></param>
        /// <param name="varName"></param>
        /// <param name="freq"></param>
        /// <param name="indexes"></param>
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

            freq = null;
            O.ChopFreq(varName, ref freq, ref varName);
        }

        /// <summary>
        /// How does this relate to the G.Chop...() methods?
        /// </summary>
        /// <param name="bank"></param>
        /// <param name="name"></param>
        /// <param name="freq"></param>
        /// <param name="index"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper for DECOMP command.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="i"></param>
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

        /// <summary>
        /// Handle indexers like x[...]
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="y"></param>
        /// <param name="x"></param>
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

        /// <summary>
        /// /// Handle indexers like x[...]
        /// </summary>
        /// <param name="logical"></param>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="options"></param>
        /// <param name="indexes"></param>
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

        /// <summary>
        /// Error message.
        /// </summary>
        private static void DollarLHSError()
        {
            G.Writeln2("*** ERROR: $-conditional on left-hand side only supports value or series type");
            throw new GekkoException();
        }

        /// <summary>
        /// Handle indexers like x[...]
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="options"></param>
        /// <param name="indexes"></param>
        public static void IndexerSetData(GekkoSmpl smpl, IVariable x, IVariable y, O.Assignment options, params IVariable[] indexes)
        {
            x.IndexerSetData(smpl, y, options, indexes);
        }

        /// <summary>
        /// Create a GekkoSmpl2 object, to fix the lag problem.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static GekkoSmpl2 Smpl(GekkoSmpl smpl, int i)
        {
            GekkoSmpl2 smplRemember = null;
            smplRemember = new GekkoSmpl2();
            smplRemember.t0 = smpl.t0;
            smplRemember.t3 = smpl.t3;
            smpl.t0 = smpl.t0.Add(i);
            return smplRemember;
        }

        /// <summary>
        /// Create a GekkoSmpl2 object, to fix the lag problem.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static GekkoSmpl2 Smpl(GekkoSmpl smpl, IVariable i)
        {
            GekkoSmpl2 smplRemember = null;
            smplRemember = new GekkoSmpl2();
            smplRemember.t0 = smpl.t0;
            smplRemember.t3 = smpl.t3;
            smpl.t0 = smpl.t0.Add(-O.ConvertToInt(i));
            return smplRemember;
        }

        /// <summary>
        /// Handle indexers like x[...]
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="indexerType"></param>
        /// <param name="indexes"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Handle indexers like x[...]
        /// </summary>
        /// <param name="smplRemember"></param>
        /// <param name="smpl"></param>
        /// <param name="indexerType"></param>
        /// <param name="x"></param>
        /// <param name="indexes"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Handle ranges like a..b
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static IVariable RangeGeneral(IVariable x1, IVariable x2)
        {
            Range r = new Range(x1, x2);
            List<IVariable> temp = new List<IVariable>();
            temp.Add(r);
            List m = new List(temp);
            List<string> mm = Program.Search(m, null, EVariableType.Var);            
            return new List(mm);
        }       

        /// <summary>
        /// Helper method for labels in PRT, but is it used??
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static IVariable ReportLabel(GekkoSmpl smpl, IVariable x, string s)
        {
            smpl.labelRecordedPieces.Add(new RecordedPieces(s, x));
            return x;
        }

        /// <summary>
        /// Helper method for labels in PRT, but is it used??
        /// </summary>
        /// <param name="smpl"></param>
        /// <returns></returns>
        public static List<List<LabelHelperIVariable>> AddLabelHelper2(GekkoSmpl smpl)
        {
            List<List<LabelHelperIVariable>> rv = null;            
            rv = new List<List<LabelHelperIVariable>>();
            return rv;
        }

        /// <summary>
        /// Handles string in quotes.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static IVariable HandleString(ScalarString x)
        {            
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

        /// <summary>
        /// List elements logic for naked lists.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper method for FOR lists (parallel loops)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Conversion.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
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

        
        // ========================================================================================
        // ========================================================================================
        // ============== Logical operations start ================================================
        // ========================================================================================
        // ========================================================================================

        /// <summary>
        /// Helper for logical AND
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="and"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Logical AND.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static IVariable LogicalAnd(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            return Helper_LogicalAndOr(smpl, x1, x2, true);
        }

        /// <summary>
        /// Logial OR.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static IVariable LogicalOr(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            return Helper_LogicalAndOr(smpl, x1, x2, false);
        }

        /// <summary>
        /// Logical NOT.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x1"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Handle $-conditionals in expressions and assignments. [1 of 3].
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="logical"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Handle $-conditionals in expressions and assignments. [2 of 3].
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="tmp"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Handle $-conditionals in expressions and assignments. [3 of 3].
        /// </summary>
        /// <param name="code"></param>
        /// <param name="vName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper method is old IF logic should be activated.
        /// </summary>
        /// <param name="b"></param>
        public static void UseOldIf(bool b)
        {
            Globals.if_old_helper = b;
        }

        /// <summary>
        /// If d != 0, returns true, else returns false.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsTrue(double d)
        {
            if (d != 0d) return true;
            else return false;
        }

        /// <summary>
        /// Logic regarding IF (... == ...), for instance IF (x == y);.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper method to detect and report where IF (x == y) type problems occur.
        /// </summary>
        /// <param name="p"></param>
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
            string s = G.ReplaceGlueSymbols(ss) + "  --->   " + originalFileName + ", line " + lineNumber;
            if (!Globals.bugfixMissing2.ContainsKey(s))
            {
                Globals.bugfixMissing1.Add(s);
                Globals.bugfixMissing2.Add(s, null);
            }
        }

        /// <summary>
        /// Handles IF (... &lt;&gt; ...)
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Handles IF( %x1 &lt; %x2 );
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Handles IF( %x1 &lt;= %x2 );
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Handles IF( %x1 &gt;= %x2 );
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Handles IF( %x1 &gt; %x2 );
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper for $ #i IN #i0 kind of syntax.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IVariable In(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return Functions.contains(smpl, null, null, y, x);
        }

        /// <summary>
        /// Evaluate some logical IF statement.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Check that the frequencies are the same.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void CheckFreq(IVariable x, IVariable y)
        {
            EFreq freq = EFreq.A;
            if (x.Type() == EVariableType.Series) freq = ((Series)x).freq;
            else freq = ((Series)y).freq;
            if (x.Type() == EVariableType.Series && y.Type() == EVariableType.Series)
            {
                if (((Series)x).freq != ((Series)y).freq)
                {
                    G.Writeln2("*** ERROR: You cannot logically compare two timeseries with freqs " + G.GetFreqPretty(((Series)x).freq) + " and " + G.GetFreqPretty(((Series)y).freq));
                    throw new GekkoException();
                }
            }
        }        

        /// <summary>
        /// Helper regarding user-defined functions.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="name"></param>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="n"></param>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string LastText(string s)
        {
            if (s.Contains(Globals.procedure)) s = "PROCEDURE " + s.Replace(Globals.procedure, "");
            else s = "FUNCTION " + s;
            return s;
        }

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for Gekko user-defined functions.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert a Gekko matrix into a timeseries.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="m"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Helper for avgt() and sumt() functions.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert from IVariable.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static GekkoTime ConvertToDate(IVariable x, GetDateChoices c)
        {
            return x.ConvertToDate(c);
        }

        /// <summary>
        /// Convert from IVariable.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static GekkoTime ConvertToDate(IVariable x)
        {
            return ConvertToDate(x, GetDateChoices.Strict);
        }

        /// <summary>
        /// Convert from IVariable.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string ConvertToString(IVariable a)
        {
            return a.ConvertToString();
        }

        /// <summary>
        /// Convert from string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertToString(string s)
        {
            return s;
        }

        /// <summary>
        /// Convert from IVariable.
        /// </summary>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static IVariable AlternativeConvertToString(IVariable iv)
        {
            return iv;
        }

        /// <summary>
        /// Convert from IVariable.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static List<IVariable> ConvertToList(IVariable a)
        {
            return a.ConvertToList();
        }

        /// <summary>
        /// Convert from IVariable.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert from IVariable.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert from IVariable.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert from IVariable.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert from IVariable.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static IVariable ConvertToMap(IVariable x)
        {
            if (x.Type() == EVariableType.Map) return x;
            else
            {
                G.Writeln2("*** ERROR: Cannot convert " + G.GetTypeString(x) + " into MAP type");
                throw new GekkoException();
            }
        }

        /// <summary>
        /// Convert from IVariable.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double ConvertToVal(GekkoTime t, IVariable a)
        {
            return a.GetVal(t);
        }

        /// <summary>
        /// Convert from IVariable.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double ConvertToVal(IVariable a)
        {
            return a.ConvertToVal();
        }

        // ------------------------------------------------------------------------------  
        // ------------------------------------------------------------------------------
        // ------------------------ converters end --------------------------------------
        // ------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="x_series"></param>
        /// <param name="s"></param>
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

        /// <summary>
        /// Get a matrix row.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IVariable[] MatrixRow(params IVariable[] list)
        {
            return list;
        }

        /// <summary>
        /// Get a matrix col. This is not so simple for nested lists.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Returns number of subperiods for current freq. Does not work for D freq.
        /// </summary>
        /// <returns></returns>
        public static int CurrentSubperiods()
        {
            int lag = 1;
            if (Program.options.freq == EFreq.Q) lag = Globals.freqQSubperiods;
            else if (Program.options.freq == EFreq.M) lag = Globals.freqMSubperiods;
            return lag;
        }
        
        /// <summary>
        /// Is this used?
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static List<string> GetList(List<string> l)
        {
            return l;
        }        

        /// <summary>
        /// Is this used?
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="a"></param>
        /// <param name="bankNumber"></param>
        /// <returns></returns>
        public static double GetVal(GekkoSmpl smpl, IVariable a, int bankNumber)  //used in PRT and similar, can accept a list that will show itself as a being an integer with ._isName set.
        {
            return a.GetValOLD(smpl);
        }
        

        // ------------------- type checks start ---------------------------
        // we do type checks as explicit functions since it is faster than using a switch
        // position -1: assignment like STRING %s = 123; where type fails
        // position 0: return value type is wrong, for instance "return 123" in a "function string f(...)"
        // position i > 0: function argument, for instance f(123) in a "function string f(string x)"

        /// <summary>
        /// Type checks for assignments, function return values, or function arguments.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static IVariable TypeCheck_void(IVariable x, int position)
        {
            return x;
        }

        /// <summary>
        /// Type checks for assignments, function return values, or function arguments.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Type checks for assignments, function return values, or function arguments.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Type checks for assignments, function return values, or function arguments.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Type checks for assignments, function return values, or function arguments.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Type checks for assignments, function return values, or function arguments.
        /// </summary>
        /// <param name="ga"></param>
        /// <param name="smpl"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Type checks for assignments, function return values, or function arguments.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Type checks for assignments, function return values, or function arguments.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Type checks for assignments, function return values, or function arguments.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Type error message.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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
                    Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPageOutput;
                    O.Cls("output");
                    string txt = "When counting arguments, a function like f(x1, x2, x3) is simple in the sense that x1 is argument #1, x2 is argument #2, and so on. But Gekko supports so-called UFCS (Uniform Function Call Syntax), so the function may be written as x1.f(x2, x3) instead. If written in that way, argument #1 is the variable or expression to the left of the dot (here: x1), whereas argument #2 is the first argument after the left parenthesis (here: x2), and so on. Another thing to keep in mind is that optional time period arguments inside <...> are ignored regarding the argument number count, so in a function call like f(<%t1 %t2>, x1, x2, x3) or equivalently x1.f(<%t1 %t2>, x2, x3), argument #1 is still x1, argument #2 is still x2, and so on.";
                    G.Writeln(txt, ETabs.Output);
                };
                string s = G.GetLinkAction("here", new GekkoAction(EGekkoActionTypes.Unknown, null, a));
                rv = "*** ERROR: Argument #" + (position - 2) + " should be " + type + " type (see more on argument number counting " + s + ")";
            }
            return rv;
        }

        /// <summary>
        /// Type checks for assignments, function return values, or function arguments.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Type checks for assignments, function return values, or function arguments.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static IVariable TypeCheck_var(IVariable x, int position)
        {
            //not possible???
            //no cloning done here...
            return x;  //no checks
        }

        // -------------------- type checks end ----------------------------

        /// <summary>
        /// STOP command.
        /// </summary>
        /// <param name="p"></param>
        public static void Stop(P p)
        {
            Globals.threadIsInProcessOfAborting = true;
            p.hasSeenStopCommand = 1;
            throw new GekkoException();
        }

        /// <summary>
        /// Helper.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="p"></param>
        public static void StopHelper(GekkoSmpl smpl, P p)
        {
            //Globals.threadIsInProcessOfAborting = true;
            p.hasSeenStopCommand = 1;
            O.FunctionLookupNew2(Globals.stopHelper)(smpl, p, false, null, null);
        }

        /// <summary>
        /// Conversion.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int ConvertToInt(IVariable a)
        {
            return ConvertToInt(a, true);
        }

        /// <summary>
        /// Conversion.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="reportError"></param>
        /// <returns></returns>
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

        // ---> The following is a lot of classes, each corresponding to a Gekko command.

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
                    if (!Program.databanks.GetFirst().ContainsIVariable(s + "!" + G.ConvertFreq(Program.options.freq))) onlyModelNotDatabank.Add(s);
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
                        Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPageOutput;
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
                        Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPageOutput;
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
                    EquationBrowser.Browser();
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
                Program.Itershow(Stringlist.GetListOfStringsFromList(this.names), this.t1, this.t2);
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
                if (this.names != null) names2 = Stringlist.GetListOfStringsFromList(this.names);
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

                //See also #87582903573828
                List<string> names = Program.Search(this.names1, opt_bank, type);

                if (isCountCommand)
                {
                    PrintFound(type, names);
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
                            names[i] = G.Chop_AddFreq(names[i], G.ConvertFreq(Program.options.freq));
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
                        PrintFound(type, names);
                    }
                }
            }

            private void PrintFound(EVariableType type, List<string> names)
            {
                G.Writeln2("Found " + names.Count + " matching items");

                if (names.Count == 0)
                {
                    //See also #87582903573828
                    SearchHelper1 helper = Program.SearchAllBanksAllFreqs(this.names1, this.opt_bank, type);
                    if (helper.allBanks.count > 0) G.Writeln("Note: " + helper.allBanks.name + " instead of " + helper.allBanks.nameOriginal + " --> " + helper.allBanks.count + " matches");
                    if (helper.allFreqs.count > 0) G.Writeln("Note: " + helper.allFreqs.name + " instead of " + helper.allFreqs.nameOriginal + " --> " + helper.allFreqs.count + " matches");
                    if (helper.allBanksAndFreqs.count > helper.allBanks.count + helper.allFreqs.count) G.Writeln("Note: " + helper.allBanksAndFreqs.name + " instead of " + helper.allBanksAndFreqs.nameOriginal + " --> " + helper.allBanksAndFreqs.count + " matches");
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

        /// <summary>
        /// Delete this? COUNT command uses O.Index.
        /// </summary>
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
                    TableStuff.XmlTable(tableFileName, this.opt_html, this.opt_window, p);
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
                    O.GetTable(this.name).CurRow.SetValues(this.col, this.prtElements[0].variable[0] as Series, this.prtElements[0].variable[1] as Series, null, this.t1, this.t2, Globals.tableOption, this.operator2, this.scale, this.format);
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
                        
            public int printStorageAsFuncCounter = -12345;

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

                //Flatten(): only lists at top-most level are preserved -- others are eliminated 
                //so for a prtElement, either the element is a non-List or a List (with only non-List items)
                //the number of items should correspond with .labels2.
                //After Flatten() it could be:
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

                if (allSeries) Flatten(); //unfolds any lists in the prtElements

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
                    List<List<MultidimItem>> check = new List<List<MultidimItem>>();
                    check.Add(new List<MultidimItem>());
                    check.Add(new List<MultidimItem>());

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
                                        lbl = Print.OPrintLabels(element.labelGiven, element.labelRecordedPieces, 1, bankNumber);
                                    }
                                    catch { lbl = new List<string>(); }

                                    FlattenArraySeriesHelper(element.variable[bankNumber] as Series, check, lbl[0], element.labelRecordedPieces, firstVariableFoundInFirstOrRef, bankNumber, unfold, labels);
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
                                        labelsUnfolded = Print.OPrintLabels(element.labelGiven, element.labelRecordedPieces, n, bankNumber);
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
                                            FlattenArraySeriesHelper(subElement_series, check, labelsUnfolded[k], element.labelRecordedPieces, firstVariableFoundInFirstOrRef, bankNumber, unfold, labels);
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

                if (G.Equal(this.opt_split, "yes") || Program.options.print_split || !allSeries || Print.IsGmulprt(this, Print.GetPrintType(this)))
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
                            Print.OPrint(this, null, labelOriginal);
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
                    Print.OPrint(this, null, labelOriginal);                    
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

            private void Flatten()
            {
                foreach (O.Prt.Element element in this.prtElements)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (element.variable[i] != null)
                        {
                            if (element.variable[i].Type() == EVariableType.List)
                            {
                                element.variable[i] = O.FlattenIVariables(element.variable[i]);
                                //now, any sub-list inside this list is gone
                            }
                            
                        }
                    }
                }
            }


            private static void FlattenArraySeriesHelper(Series tsFirst, List<List<MultidimItem>> check, string label2, List<O.RecordedPieces> recordedPieces, int firstVariableFoundInFirstOrRef, int bankNumber, List unfold, List<string> labels)
            {

                List<MultidimItem> keys = tsFirst.dimensionsStorage.storage.Keys.ToList();

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
                                        string[] ss = Stringlist.GetListOfStringsFromListOfIvariables((set as List).list.ToArray());
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
                keys.Sort(Multidim.CompareMultidimItems);

                //List mm0 = new List();
                foreach (MultidimItem key in keys)
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
                Estimation.Ols(this);
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
                string zfilename = Program.CreateFullPathAndFileName(G.AddExtension(this.fileName, "." + Globals.extensionCommand));
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
                string zfilename = Program.CreateFullPathAndFileName(G.AddExtension(this.fileName, "." + Globals.extensionPlot));
                
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
                string zfilename = Program.CreateFullPathAndFileName(G.AddExtension(this.fileName, extension));
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

