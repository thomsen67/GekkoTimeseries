using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ProtoBuf;

namespace Gekko
{
    [ProtoContract]
    public class ScalarString : IVariable
    {
        [ProtoMember(1)]
        public string string2;
        //public IVariable pointerTo = null;  //used in case the string points to for instance a timeseries. Only used inside a GENR/PRT statement, inside the implicit timeloop (where we can be sure that the string itself does not change value). Is set to null again after the time loop.
        //public bool _isName = false;
        
        private ScalarString()
        {
            //only because protobuf needs it, not for outside use
        }

        public ScalarString(string s)
        {
            Initialize(s, false);  //last arg cannot be true --> too many errors regarding varnames etc.
        }
        

        public ScalarString(string s, bool substitute)
        {
            Initialize(s, substitute);
        }

        public void Initialize(string s, bool substitute)
        {
            //for instance, for a #x variable that returns a string because of an implicit list loop. This string is indicated as being a name, not a string.
            if (s == null) s = "";  //for instance, a label or source from a timeseries that is null. If later search() or similar is used, it is better to use "" than null.
            if (substitute) s = SubstituteScalarsInString(s, true, false);
            string2 = s;
            //_isName = isName;
        }
        // ----------------------------------------------------
        // --------------object functions start----------------
        // ----------------------------------------------------

        public IVariable append(bool isLhs, GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("*** ERROR: Object method .append() not available for type " + G.GetTypeString(this));
            throw new GekkoException();
            List<string> xx = null;
        }

        public IVariable extend(bool isLhs, GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("*** ERROR: Object method .extend() not available for type " + G.GetTypeString(this));
            throw new GekkoException();
        }

        // ----------------------------------------------------
        // --------------object functions end------------------
        // ----------------------------------------------------


        public static string SubstituteScalarsInString(string s, string reportError, string avoidVal)
        {
            //Seems to be activated here: #0934580980 -- we live with it for the time being
            return SubstituteScalarsInString(s, G.Equal(reportError, "yes"), G.Equal(avoidVal, "yes"));
        }

        public static string SubstituteScalarsInString(string s, bool reportError, bool avoidVal)
        {
            //UPDATE: we will no longer replace %animal in 'the %animal ran', only 'the {%animal} ran' or 'the {animal} ran'
            //UPDATE: also replaces $ and $%, for instance $n or $%n. NOTE!! -> $ scalars are removed not

            //New logic: we just look for {...}, and try to find a %-variable inside. To avoid, use ~{...
            //Concatenation is not used anymore

            //Tilde is used for ~{ and ~'

            //We may have a problem with labels, when the user does this: PRT x%i instead of x{%i}
            //What to do about this? Also x#i and x[#i]. Fix it. 

            if (s == null) return null;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {                
                if (IsLeftCurly(s, i))
                {
                    //search for matching
                    for (int j = i + 1; j < s.Length; j++)
                    {
                        if (IsLeftCurly(s, j)) break;  //nested tilde, no good --> we abort
                        if (IsRightCurly(s, j))
                        {
                            //now we have left curly at i, and right curly at j
                            string ss = s.Substring(i + 1, j - i - 1).Trim();
                            if (ss.StartsWith(Globals.symbolScalar.ToString()))
                            {
                                ss = ss.Substring(1);  //remove it
                            }
                            if (G.IsSimpleToken(ss, false))
                            {
                                //look up the scalar
                                IVariable a = O.Lookup(null, null, null, Globals.symbolScalar + ss, null, null, false, EVariableType.Var);
                                string s2 = a.ConvertToString();
                                sb.Append(s2);
                                i = j;  //to jump forwards
                                goto Flag1;

                            }
                            else
                            {
                                //just ignore it
                            }
                        }
                    }
                }
                sb.Append(s[i]);
                Flag1: i = i;
            }

            sb.Replace("~'", "'");
            sb.Replace("~{", "{");

            return sb.ToString();
        }

        private static bool IsLeftCurly(string s, int i)
        {
            return s[i] == Globals.symbolLeftCurly && !(i > 0 && s[i - 1] == Globals.symbolTilde);
        }

        private static bool IsRightCurly(string s, int i)
        {
            return s[i] == Globals.symbolRightCurly && !(i > 0 && s[i - 1] == Globals.symbolTilde);
        }

        public static string SubstituteScalarsInStringOLDDELETE(string s, bool reportError, bool avoidVal)
        {
            //UPDATE: also replaces $ and $%, for instance $n or $%n. NOTE!! -> $ scalars are removed not
            //Will look for '%' and find alphanumeric + underscore after that, to construct a scalar name.
            //So STRING s = 'abc%de,%f' will find two scalars. If tilde is used, scalars are not found:
            //STRING s = 'abc~%de,~%f' will return abc%d,%f
            //Concatenation can be used:
            //STRING s = 'abc%d|e' will look for %d, not %de. The '|' is removed afterwards, like the '~'
            //Maybe we could use the tilde as escape character more generally?? So '~|' will show the '|'. And '~~' will show one '~'.
            if (s == null) return null;
            string s2 = null;
            bool hit = false;
            int lastEnd = -1;
            for (int j = 0; j < s.Length - 1; j++)
            {
                bool tilde = (j > 0 && s[j - 1] == Globals.symbolTilde);  // ~%x or ~$x or ~{x}
                //bool isDollarPercent = false;
                //bool isDollar = false;
                bool isCurly = false;
                //if (j > 0 && s[j - 1] == Globals.symbolDollar[0] && s[j - 0] == Globals.symbolMemvar) isDollarPercent = true;
                //if (j > 1 && s[j - 2] == Globals.symbolTilde && isDollarPercent) tilde = true;  // ~$%x
                //if (s[j] == Globals.symbolDollar[0]) isDollar = true;
                if (s[j] == '{') isCurly = true;
                //if (j > 0 && s[j - 1] == Globals.symbolTilde && isDollar) tilde = true;  // ~$x                                
                if ((s[j] == Globals.symbolScalar) && !tilde)
                {
                    string variable = null;
                    int end = -1;
                    for (int jj = j + 1; jj < s.Length; jj++)
                    {
                        if (G.IsLetterOrDigitOrUnderscore(s[jj]))
                        {
                            //do nothing, loop goes on
                        }
                        else
                        {
                            end = jj;
                            break;
                        }
                    }
                    if (end == -1) end = s.Length;
                    variable = s.Substring(j + 1, end - (j + 1));
                    if (variable.Length > 0)
                    {
                        try
                        {
                            IVariable a = O.Lookup(null, null, null, Globals.symbolScalar + variable, null, null, false, EVariableType.Var);
                            if (a.Type() == EVariableType.String || a.Type() == EVariableType.Date || a.Type() == EVariableType.Val)
                            {
                                bool valfail = false;
                                if (a.Type() == EVariableType.Val)
                                {
                                    if (j == 0)
                                    {
                                        valfail = true;  //for instance PRT %v, where v is a VAL, should no in-substitute
                                    }
                                    else
                                    {
                                        if (!G.IsLetterOrDigitOrUnderscore(s[j - 1])) valfail = true;  //for instance, PRT ab%v should in-substitute
                                        else
                                        {
                                            //for instance, PRT ab%v, where %v = 2, should print as ab2                                            
                                        }
                                    }
                                }
                                if (!avoidVal) valfail = false;  //overriding
                                if (!valfail)
                                {
                                    IVariable b = new ScalarString("");
                                    IVariable c = b.Add(null, a);
                                    string s3 = c.ConvertToString();
                                    int x = 0;
                                    //if (isDollarPercent) x = 1;
                                    string s4 = s.Substring(lastEnd + 1, j - lastEnd - 1 - x);
                                    s2 += s4 + s3;
                                    hit = true;
                                }
                            }
                            else
                            {
                                //should not be possible regarding %-vars
                            }
                        }
                        catch
                        {
                            if (reportError) throw new GekkoException();
                        }
                        lastEnd = end - 1;
                        j = lastEnd;
                    }
                }
                else if (isCurly && !tilde)  //curly is at position j
                {
                    string variable = null;
                    int end = -1;
                    for (int jj = j + 1; jj < s.Length; jj++)
                    {
                        if (s[jj] == '}')
                        {
                            end = jj;
                            break;
                        }
                    }
                    if (end != -1)
                    {
                        variable = s.Substring(j + 1, end - (j + 1)).Trim();
                        if (variable.StartsWith(Globals.symbolScalar.ToString())) variable = variable.Substring(1);
                        if (G.IsSimpleToken(variable))
                        {
                            try
                            {
                                IVariable a = O.Lookup(null, null, null, Globals.symbolScalar + variable, null, null, false, EVariableType.Var);

                                if (a.Type() == EVariableType.String || a.Type() == EVariableType.Date || a.Type() == EVariableType.Val)
                                {
                                    IVariable b = new ScalarString("");
                                    IVariable c = b.Add(null, a);
                                    string s3 = c.ConvertToString();
                                    string s4 = s.Substring(lastEnd + 1, j - lastEnd - 1);
                                    s2 += s4 + s3;
                                    hit = true;
                                }
                                else
                                {
                                    //should not be possible regarding %-vars
                                }
                            }
                            catch
                            {
                                if (reportError) throw new GekkoException();
                            }
                            lastEnd = end;
                            j = lastEnd;
                        }
                    }
                }
            }
            if (hit)
            {
                s2 += s.Substring(lastEnd + 1, s.Length - lastEnd - 1);
                s = s2;
            }
            string tp = new string(new char[] { Globals.symbolTilde, Globals.symbolScalar });
            string p = new string(Globals.symbolScalar, 1);
            s = s.Replace(tp, p);
            //string tp2 = new string(new char[] { Globals.symbolTilde, Globals.symbolDollar[0] });
            //string p2 = new string(Globals.symbolDollar[0], 1);
            //s = s.Replace(tp2, p2);
            string tp3 = new string(new char[] { '{', Globals.symbolTilde });  //a{~n}b
            string p3 = new string('{', 1);
            s = s.Replace(tp3, p3);

            s = s.Replace("~'", "'");

            //Hmmm, in 'Hej~%s|du', this will become 'Hej%sdu', not 'Hej%s|du'
            //This is maybe not too good, but never mind            
            string concat = new string(Globals.symbolConcatenation, 1);
            //The following 3 lines remove single '|', but not double '||'.
            //Could use regex, but this is ok.
            //This means that PRT f|e will be fe, but show [1 || 2] keeps the '||'.
            s = s.Replace(concat + concat, "[<{2concats}>]");
            s = s.Replace(concat, "");
            s = s.Replace("[<{2concats}>]", concat + concat);

            return s;
        }

        //public bool IsName() {
        //    return _isName;
        //}

        public EVariableType Type()
        {
            return EVariableType.String;
        }                       

        public IVariable Indexer(GekkoSmpl smpl, params IVariable[] indexes)
        {
            if (indexes.Length == 1)
            {
                IVariable index = indexes[0];
                IVariable rv = null;
                if (this.string2 == Globals.indexerAloneCheatString)
                {
                    //corresponds to empty indexer like ['fy*'], different from #a['fy*']
                    if (index.Type() == EVariableType.String)
                    {
                        //string vars = null;                    
                        //ExtractBankAndRestHelper h = Program.ExtractBankAndRest(((ScalarString)index).string2, EExtrackBankAndRest.GetDatabank);
                        //List<string> output = Program.MatchWildcardInDatabank(h.name, h.databank);
                        //rv = new List(output);
                        string s = ((ScalarString)index).string2;
                        if (s.StartsWith(Globals.firstCheatString + Globals.symbolBankColon2))
                        {
                            s = s.Substring((Globals.firstCheatString + Globals.symbolBankColon2).Length);
                        }
                        rv = O.HandleWildcards(s, null);
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: The inside of a free-standing [...] list should not be a");
                        G.Writeln("    VAL, DATE or the like, for instance PRT [2.3] or PRT [2010q5].");
                        G.Writeln("    The right use is PRT [gd*] and similar.");
                        throw new GekkoException();
                    }
                }                
                else
                {
                    G.Writeln2("*** ERROR: You cannot use indexer on a string, for instance %s[2],");
                    G.Writeln("    but you may use the string as a name instead: {%s}[2015].");
                    throw new GekkoException();
                }
                return rv;
            }
            else
            {
                G.Writeln2("*** ERROR: Cannot use " + indexes.Length + "-dimensional indexer on STRING or NAME");
                throw new GekkoException();
            }

        }

        //public IVariable Indexer(GekkoSmpl t, IVariablesFilterRange indexRange1, IVariablesFilterRange indexRange2)
        //{
        //    throw new GekkoException();
        //}

        //public IVariable Indexer(GekkoSmpl t, IVariablesFilterRange indexRange)
        //{
        //    if (this._string2 == Globals.indexerAloneCheatString)
        //    {
        //        //corresponds to empty index range like ['fx'..'fy'], different from #a['fx'..'fy']
        //        IVariable iv1 = indexRange.first;
        //        IVariable iv2 = indexRange.last;
        //        string s1 = O.ConvertToString(iv1);
        //        string s2 = O.ConvertToString(iv2);                
        //        ExtractBankAndRestHelper h = Program.ExtractBankAndRest(s1, EExtrackBankAndRest.GetDatabank);                
        //        List<string> temp = Program.MatchRangeInDatabank(h.name, s2, h.databank);
        //        return new List(temp);
        //    }
        //    else
        //    {
        //        G.Writeln2("*** ERROR: You cannot use []-index on string");
        //        throw new GekkoException();
        //    }            
        //}

        //public IVariable Indexer(GekkoSmpl t, IVariable index, IVariablesFilterRange indexRange)
        //{
        //    throw new GekkoException();
        //}

        //public IVariable Indexer(GekkoSmpl t, IVariablesFilterRange indexRange, IVariable index)
        //{
        //    throw new GekkoException();
        //}

        public IVariable Negate(GekkoSmpl t)
        {
            G.Writeln2("*** ERROR: You cannot use minus on string");
            throw new GekkoException();
        }

        //public void InjectAdd(GekkoSmpl t, IVariable x, IVariable y)
        //{
        //    G.Writeln2("*** ERROR: You cannot use add on string");
        //    throw new GekkoException();
        //}

        public double GetValOLD(GekkoSmpl smpl)
        {

            //Conversion not allowed in for instance VAL x = %s, where s is a STRING pointing to a timeseries.
            G.Writeln2("*** ERROR: You are trying to extract a numerical value from STRING '" + this.string2 + "'");
            G.Writeln("           A STRING s ('" + this.string2 + "') can refer to a timeseries name (" + this.string2 + "), but in");
            G.Writeln("           that case you must use {s} or {%s} instead of %s.");
            throw new GekkoException();
        }

        public double GetVal(GekkoTime smpl)
        {
            G.Writeln2("*** ERROR: Cannot extract a scalar value from " + G.GetTypeString(this) + " type");
            throw new GekkoException();
        }

        public double ConvertToVal()
        {
            G.Writeln2("*** ERROR: Cannot extract a VAL from " + G.GetTypeString(this) + " type");
            throw new GekkoException();
        }

        public string ConvertToString()
        {
            return this.string2;
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            G.Writeln2("*** ERROR: Could not convert the STRING " + this.string2 + " directly into a DATE.");
            G.Writeln("           You may try the date() conversion function.");            
            throw new GekkoException();
        }

        public List<IVariable> ConvertToList()
        {
            //for instance for list elements, where a string is considered a 1-item list.
            return new List<IVariable>() { new ScalarString(this.string2) };  //always make a copy, so no risk of side effects
        }
        
        public IVariable Add(GekkoSmpl t, IVariable x)
        {
            switch (x.Type())
            {
                case EVariableType.String:
                    {
                        return new ScalarString(this.string2 + ((ScalarString)x).string2);
                    }                    
                case EVariableType.List:
                    {
                        return Operators.StringList.Add(this, (List)x, false);
                    }                    
                case EVariableType.Series:
                    {
                        G.Writeln2("*** ERROR: You cannot add a string and a timeseries");
                        throw new GekkoException();
                    }                    
                case EVariableType.Val:
                    {
                        return Operators.StringVal.Add(this, (ScalarVal)x, false);
                    }
                case EVariableType.Date:
                    {
                        return Operators.StringDate.Add(this, (ScalarDate)x, false);
                    }                  
                default:
                    {
                        G.Writeln2("*** ERROR: Type error regarding add.");                        
                        throw new GekkoException();
                    }
            }
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: %x -%y (minus) is not allowed if %x is a STRING scalar.");
            throw new GekkoException();
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: %x*%y (multiply) is not allowed if %x is a STRING scalar.");
            throw new GekkoException();            
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: %x/%y (divide) is not allowed if %x is a STRING scalar.");
            throw new GekkoException();            
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: %x^%y or %x**%y (power) is not allowed if %x is a STRING scalar.");
            throw new GekkoException();            
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, params IVariable[] dims)
        {
            G.Writeln2("*** ERROR: You cannot use an indexer [] on a STRING");
            throw new GekkoException();
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            return new ScalarString(this.string2);
        }

        public void DeepTrim()
        {
            //do nothing, nothing to trim
        }

        public void DeepCleanup()
        {
            //do nothing
        }

    }

}
