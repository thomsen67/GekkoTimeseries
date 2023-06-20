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
        }        

        public static string SubstituteScalarsInString(string s, string reportError, string avoidVal)
        {
            //Seems to be activated here: #0934580980 -- we live with it for the time being
            return SubstituteScalarsInString(s, G.Equal(reportError, "yes"), G.Equal(avoidVal, "yes"));
        }

        public static string SubstituteScalarsInString(string s, bool reportError, bool avoidVal)
        {
            //This method is probably only used when issuing a "PRT #m;", where #m is a matrix (cf. #lkjadsfkalsdfjaskl)
            //This might be %s = 'a'; PRT #{%a}; where the code will instert the 'a'.
            //It will also handle tilde.
            //For printing (PRT) of series, a whole other kind of logic is used, where the parser
            //helps inserting the right things in complicated a{...}b names. This logic ought to be
            //used for printing matrices and other variables, too.
            //But for now, we keed this method.

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
                        new Error("Illegal string indexer [" + ival + "]: negative number not allowed");
                        //throw new GekkoException();
                    }
                    else if (ival == 0)
                    {
                        new Error("Illegal [0] string indexing. Use the length() function for string length.");
                        //throw new GekkoException();
                    }
                    else if (ival > this.string2.Length)
                    {
                        new Error("Illegal string indexer [" + ival + "]: larger than length of string (" + this.string2.Length + ")");
                        //throw new GekkoException();
                    }

                    return new ScalarString(this.string2[ival - 1].ToString());
                }
                else if (index.Type() == EVariableType.Range)
                {
                    Range index_range = index as Range;

                    //slice like %s[2..5], substring, can also use %s[2..] or %s[..5]

                    int ival1 = 1;
                    int ival2 = this.string2.Length;

                    if (index_range.first != null) ival1 = O.ConvertToInt(index_range.first);
                    if (index_range.last != null) ival2 = O.ConvertToInt(index_range.last);

                    if (ival1 > this.string2.Length || ival2 > this.string2.Length || ival2 < ival1 || ival1 < 1 || ival2 < 1)
                    {
                        new Error("Invalid range, [" + ival1 + " .. " + ival2 + "]");
                        //throw new GekkoException();
                    }

                    string s = this.string2.Substring(ival1 - 1, ival2 - ival1 + 1);
                    return new ScalarString(s);

                }
                else if (index.Type() == EVariableType.String)
                {
                    new Error("You cannot use %s1[%s2], where %s1 and %s2 are strings. Perhaps see the index() function.");
                    return null;
                }
                else
                {
                    new Error("Type mismatch regarding []-index");
                    return null;
                }
            }
            else
            {
                new Error("Cannot use " + indexes.Length + "-dimensional indexer on string");
                return null;
                //throw new GekkoException();
            }
        }


        public IVariable Negate(GekkoSmpl t)
        {
            new Error("You cannot use minus on string"); return null;
            //throw new GekkoException();
        }

        public double GetValOLD(GekkoSmpl smpl)
        {

            //Conversion not allowed in for instance VAL x = %s, where s is a STRING pointing to a timeseries.
            new Error("You are trying to extract a numerical value from string '" + this.string2 + "'. A string s ('" + this.string2 + "') can refer to a timeseries name (" + this.string2 + "), but in that case you must use {%s} instead of %s."); return double.NaN;


            //throw new GekkoException();
        }

        public double GetVal(GekkoTime smpl)
        {
            new Error("Cannot extract a val from " + G.GetTypeString(this) + " type"); return double.NaN;
            //throw new GekkoException();
        }

        public double ConvertToVal()
        {
            using (Error e = new Error())
            {
                e.MainAdd("Cannot extract a val from " + G.GetTypeString(this) + " type ('" + this.string2 + "'). ");
                double d = double.NaN; bool b = double.TryParse(this.string2, out d);
                if (this.isFromNakedList && b)
                {
                    e.MainNewLine();
                    e.MainAdd("Note that the " + G.GetTypeString(this) + " '" + this.string2 + "' origins from a");
                    e.MainAdd("'naked' list without parentheses, like #m = 007, 1e5;. The items in a list like #m");
                    e.MainAdd("may look like values, but are treated as strings. You may use #m.vals() to convert the");
                    e.MainAdd("items into the numbers 7 and 100000. See more in the help on the 'Naked list' page");
                }
                else
                {
                    e.MainAdd(Globals.stringConversionNote);
                }
            }            
            
            throw new GekkoException();
        }

        public string ConvertToString()
        {
            return this.string2;
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            using (Error e = new Error())
            {
                e.MainAdd("Could not convert the string '" + this.string2 + "' directly into a date (try the date() function).");
                GekkoTime gt = GekkoTime.FromStringToGekkoTime(this.string2);
                if (this.isFromNakedList && !gt.IsNull())
                {
                    e.MainNewLine();
                    e.MainAdd("Note that the '" + this.string2 + "' origins from a");
                    e.MainAdd("'naked' list without parentheses, like #m = 2020q1, 2020q2, 2020q3;. In such");
                    e.MainAdd("naked lists, the items are treated as strings. You should perhaps");
                    e.MainAdd("add enclosing parentheses, #m = (2020q1, 2020q2, 2020q3);");
                }
            }
            return GekkoTime.tNull;
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
                        using (Error txt = new Error())
                        {                            
                            List.ScalarAndListErrorMessage(txt, true);
                        }
                        return null;
                    }
                case EVariableType.Series:
                    {
                        new Error("You cannot add a string and a timeseries. " + Globals.stringConversionNote);
                        return null;
                    }                    
                case EVariableType.Val:
                    {
                        return Operators.StringVal.Add(this, (ScalarVal)x, false);
                    }
                case EVariableType.Date:
                    {
                        return Operators.StringDate.Add(this, (ScalarDate)x, false);
                    }
                case EVariableType.Null:
                    {
                        return new ScalarString(this.string2);  //string + null = string, see #9785278992347
                    }
                default:
                    {
                        new Error("Type error regarding add."); return null;
                        //throw new GekkoException();
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
                        //This is only allowed for stuff like COPY b:{#m} etc.
                        //See also #786592387654
                        return Operators.ScalarList.Add(t, this, x, false);
                    }
                case EVariableType.Series:
                    {
                        new Error("You cannot concatenate a string and a timeseries"); return null;
                        //throw new GekkoException();
                    }
                case EVariableType.Val:
                    {
                        return Operators.StringVal.Add(this, (ScalarVal)x, false);
                    }
                case EVariableType.Date:
                    {
                        return Operators.StringDate.Add(this, (ScalarDate)x, false);
                    }
                case EVariableType.Null:
                    {
                        return new ScalarString(this.string2);  //string + null = string, see #9785278992347
                    }
                default:
                    {
                        new Error("Type error regarding concatenate."); return null;
                        //throw new GekkoException();
                    }
            }
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            new Error("Subtracting from a string ('" + this.string2 + "') is not allowed. " + Globals.stringConversionNote);
            return null;
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {            
            new Error("Multiplication involving a string ('" + this.string2 + "') is not allowed. " + Globals.stringConversionNote);
            return null;
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            new Error("Division involving a string ('" + this.string2 + "') is not allowed. " + Globals.stringConversionNote);
            return null;
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            new Error("Exponentiation involving a string ('" + this.string2 + "') is not allowed. " + Globals.stringConversionNote);
            return null;
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims)
        {
            new Error("You cannot use an indexer [] on a string"); return;
            //throw new GekkoException();
        }

        public IVariable DeepClone(GekkoSmplSimple truncate, CloneHelper cloneHelper)
        {
            ScalarString ss = new ScalarString(this.string2);
            ss.isFromNakedList = this.isFromNakedList;
            return ss;
        }

        public void DeepTrace(TraceHelper th)
        {
            //do nothing
        }

        public void DeepCount(Count count)
        {
            count.n += Globals.count1;
            count.n += Globals.count3 * this.string2.Length;
        }

        public void DeepTrim()
        {
            //do nothing, nothing to trim
        }

        public void DeepCleanup(TwoInts yearMinMax)
        {
            //do nothing
        }

    }

}
