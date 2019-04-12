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
        public bool isFromNakedList = false; //do not protobuf this

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
                                IVariable a = O.Lookup(null, null, null, Globals.symbolScalar + ss, null, null, new LookupSettings(), EVariableType.Var, null);
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
                            IVariable a = O.Lookup(null, null, null, Globals.symbolScalar + variable, null, null, new LookupSettings(), EVariableType.Var, null);
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
                                IVariable a = O.Lookup(null, null, null, Globals.symbolScalar + variable, null, null, new LookupSettings(), EVariableType.Var, null);

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

        public IVariable Indexer(GekkoSmpl smpl, O.EIndexerType indexerType, params IVariable[] indexes)
        {
            if (indexes.Length == 1)
            {
                IVariable index = indexes[0];
                //Indices run from 1, 2, 3, ... n. Element 0 is length of list.
                if (index.Type() == EVariableType.Val)
                {
                    int ival = O.ConvertToInt(index);
                    if (ival < 0)
                    {
                        G.Writeln2("*** ERROR: Illegal string indexer [" + ival + "]: negative number not allowed");
                        throw new GekkoException();
                    }
                    else if (ival == 0)
                    {
                        G.Writeln2("*** ERROR: Illegal [0] string indexing. Use the length() function for string length.");
                        throw new GekkoException();
                    }
                    else if (ival > this.string2.Length)
                    {
                        G.Writeln2("*** ERROR: Illegal string indexer [" + ival + "]: larger than length of string (" + this.string2.Length + ")");
                        throw new GekkoException();
                    }

                    return new ScalarString(this.string2[ival - 1].ToString());
                }
                else if (index.Type() == EVariableType.Range)
                {
                    Range index_range = index as Range;

                    //slice like %s[2..5], substring

                    int ival1 = O.ConvertToInt(index_range.first);
                    int ival2 = O.ConvertToInt(index_range.last);
                    if (ival1 > this.string2.Length || ival2 > this.string2.Length || ival2 < ival1 || ival1 < 1 || ival2 < 1)
                    {
                        G.Writeln2("*** ERROR: Invalid range, [" + ival1 + " .. " + ival2 + "]");
                        throw new GekkoException();
                    }

                    string s = this.string2.Substring(ival1 - 1, ival2 - ival1 + 1);
                    return new ScalarString(s);

                }
                else if (index.Type() == EVariableType.String)
                {
                    G.Writeln2("*** ERROR: You cannot use %s1[%s2], where %s1 and %s2 are strings. Perhaps see the search() function.");
                    throw new GekkoException();
                }
                else
                {
                    G.Writeln2("*** ERROR: Type mismatch regarding []-index");
                    throw new GekkoException();
                }
            }
            else
            {
                G.Writeln2("*** ERROR: Cannot use " + indexes.Length + "-dimensional indexer on string");
                throw new GekkoException();
            }
        }


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
            G.Writeln2("*** ERROR: Cannot extract a val from " + G.GetTypeString(this) + " type");
            double d = double.NaN; bool b = double.TryParse(this.string2, out d);
            if (this.isFromNakedList && b)
            {

                G.Writeln("           Note that the " + G.GetTypeString(this) + " '" + this.string2 + "' origins from a");
                G.Writeln("           'naked' list without parentheses, like #m = 1, 2, 3;. In such naked lists,");
                G.Writeln("           the items are always treated as strings. You should perhaps add enclosing");
                G.Writeln("           parentheses, corresponding to #m = (1, 2, 3);");
            }
            throw new GekkoException();
        }

        public string ConvertToString()
        {
            return this.string2;
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            G.Writeln2("*** ERROR: Could not convert the string '" + this.string2 + "' directly into a date.");
            G.Writeln("           You may try the date() conversion function.");
            GekkoTime gt = G.FromStringToDate(this.string2);            
            if (this.isFromNakedList && !gt.IsNull())
            {
                G.Writeln("           Note that the " + G.GetTypeString(this) + " '" + this.string2 + "' origins from a");
                G.Writeln("           'naked' list without parentheses, like #m = 2020q1, 2020q2, 2020q3;. In such");
                G.Writeln("           naked lists, the items are always treated as strings. You should perhaps ");
                G.Writeln("           add enclosing parentheses, corresponding to #m = (2020q1, 2020q2, 2020q3);");
            }
            throw new GekkoException();
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
                        G.Writeln2("*** ERROR: Adding a list and scalar with %s + #x is no longer legal");
                        G.Writeln("           Please use #x.prefix(%s) instead.");
                        throw new GekkoException();
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

        public IVariable Concat(GekkoSmpl t, IVariable x)
        {
            switch (x.Type())
            {
                case EVariableType.String:
                    {
                        return new ScalarString(this.string2 + ((ScalarString)x).string2);
                    }
                case EVariableType.List:
                    {
                        G.Writeln2("*** ERROR: Concat to string not allowed for this type: " + G.GetTypeString(x));
                        throw new GekkoException();
                        //This is only allowed for string scalar, for instance:
                        // %s + #m. Like this, we can use {'b:' + #m} or b:{#m} to compose.
                        //See also #786592387654
                        return Operators.ScalarList.Add(t, this, x, false);
                    }
                case EVariableType.Series:
                    {
                        G.Writeln2("*** ERROR: You cannot concatenate a string and a timeseries");
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
                        G.Writeln2("*** ERROR: Type error regarding concatenate.");
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

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims)
        {
            G.Writeln2("*** ERROR: You cannot use an indexer [] on a STRING");
            throw new GekkoException();
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            ScalarString ss = new ScalarString(this.string2);
            ss.isFromNakedList = this.isFromNakedList;
            return ss;
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
