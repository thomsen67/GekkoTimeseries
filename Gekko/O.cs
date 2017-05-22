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

  
    public static class O
    {
        //Common methods start
        //Common methods start
        //Common methods start
                

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

        public class GekkoListIterator : IEnumerable<ScalarString>
        {            
            private MetaList _ml = null;

            public GekkoListIterator(IVariable list)
            {
                if(list.Type() != EVariableType.List)
                {
                    G.Writeln2("*** ERROR: Expected a list in iterator");
                    throw new GekkoException();
                }
                _ml = (MetaList)list;
            }

            public IEnumerator<ScalarString> GetEnumerator()
            {
                foreach(string s in _ml.list)
                {
                    yield return new ScalarString(s);
                }                                         
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                G.Writeln("*** ERROR: iterator problem");
                throw new GekkoException();
            }
        }

        public static IVariable Add(IVariable x, IVariable y, GekkoTime t)
        {
            return x.Add(y, t);
        }

        public static IVariable Add(IVariable x, IVariable y, IVariable z, GekkoTime t)
        {
            return x.Add(y, t).Add(z, t);
        }

        public static IVariable Subtract(IVariable x, IVariable y, GekkoTime t)
        {
            return x.Subtract(y, t);
        }

        public static IVariable Multiply(IVariable x, IVariable y, GekkoTime t)
        {
            return x.Multiply(y, t);
        }

        public static IVariable Divide(IVariable x, IVariable y, GekkoTime t)
        {
            return x.Divide(y, t);
        }

        public static IVariable Power(IVariable x, IVariable y, GekkoTime t)
        {
            return x.Power(y, t);
        }

        public static IVariable Negate(IVariable x, GekkoTime t)
        {
            return x.Negate(t);
        }

        public static IVariable AndAdd(IVariable x, IVariable y, GekkoTime t)
        {
            return Functions.union(t, x, y);
        }

        public static IVariable AndSubtract(IVariable x, IVariable y, GekkoTime t)
        {
            return Functions.difference(t, x, y);
        }

        public static IVariable AndMultiply(IVariable x, IVariable y, GekkoTime t)
        {
            return Functions.intersect(t, x, y);
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
                    G.Writeln2("Databank " + bank.aliasName + " is empty");
                    continue;
                }
                foreach (TimeSeries ts in bank.storage.Values)  
                {
                    if (ts.freqEnum == EFreq.Annual) a++;
                    else if (ts.freqEnum == EFreq.Quarterly) q++;
                    else if (ts.freqEnum == EFreq.Monthly) m++;
                    else if (ts.freqEnum == EFreq.Undated) u++;                    
                }
                G.Writeln2("Databank " + bank.aliasName + ":");
                if (a > 0) G.Writeln("  " + a + " annual timeseries");
                if (q > 0) G.Writeln("  " + q + " quarterly timeseries");
                if (m > 0) G.Writeln("  " + m + " monthly timeseries");
                if (u > 0) G.Writeln("  " + u + " undated timeseries");
            }
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
            None,
            Can,
            Must
        }


        public enum GetDateChoices
        {
            Strict,
            FlexibleStart,
            FlexibleEnd
        }

        public static ScalarVal SetValData(IVariable name, IVariable rhs, GekkoTime t)
        {
            //Returns the IVariable it finds here (or creates)            
            string name2 = name.GetString();            
            double value = rhs.GetVal(t);
            IVariable lhs = null;
            if (Program.scalars.TryGetValue(name2, out lhs))
            {
                //Scalar is already existing                
                if (lhs.Type() == EVariableType.Val)
                {
                    //Already existing lhs is a VAL, inject into it. Injecting is faster than recreating an object.
                    ((ScalarVal)lhs).val = value;                    
                }
                else
                {                    
                    Program.scalars.Remove(name2);
                    lhs = new ScalarVal(value);
                    Program.scalars.Add(name2, lhs);                    
                }
            }
            else
            {
                //Scalar does not exist beforehand                   
                lhs = new ScalarVal(value);
                Program.scalars.Add(name2, lhs);                
            }
            return (ScalarVal)lhs;
        }

        public static ScalarString SetStringData(IVariable name, IVariable rhs, bool isName)
        {
            //Returns the IVariable it finds here (or creates)
            string name2 = name.GetString();
            string value = rhs.GetString();            
            IVariable lhs = null;
            if (Program.scalars.TryGetValue(name2, out lhs))
            {
                //Scalar is already existing                
                if (lhs.Type() == EVariableType.String)
                {
                    //Already existing lhs is a STRING, inject into it. Injecting is faster than recreating an object.                    
                    ((ScalarString)lhs)._string2 = value;
                    ((ScalarString)lhs)._isName = isName;
                }
                else
                {
                    //The object has to die and be recreated, since it is of a wrong type.                                
                    Program.scalars.Remove(name2);
                    lhs = new ScalarString(value, isName, false);
                    Program.scalars.Add(name2, lhs);
                }
            }
            else
            {
                //Scalar does not exist beforehand  
                lhs = new ScalarString(value, isName, false);
                Program.scalars.Add(name2, lhs);
            }
            return (ScalarString)lhs;
        }

        public static ScalarDate SetDateData(IVariable name, IVariable rhs)
        {
            //Returns the IVariable it finds here (or creates)
            string name2 = name.GetString();
            GekkoTime value = O.GetDate(rhs);
            IVariable lhs = null;
            if (Program.scalars.TryGetValue(name2, out lhs))
            {
                //Scalar is already existing                
                if (lhs.Type() == EVariableType.Date)
                {
                    //Already existing lhs is a DATE, inject into it. Injecting is faster than recreating an object.
                    ((ScalarDate)lhs).date = value;
                }
                else
                {
                    //The object has to die and be recreated, since it is of a wrong type.                                
                    Program.scalars.Remove(name2);
                    lhs = new ScalarDate(value);
                    Program.scalars.Add(name2,lhs );
                }
            }
            else
            {
                //Scalar does not exist beforehand            
                lhs = new ScalarDate(value);
                Program.scalars.Add(name2, lhs);
            }
            return (ScalarDate)lhs;
        }

        public static Matrix SetMatrixData(IVariable name, IVariable rhs)
        {
            //Returns the IVariable it finds here (or creates)
            
            Matrix value = O.GetMatrix(rhs);
            IVariable lhs = null;

            string name2 = name.GetString();
            if (Program.scalars.TryGetValue(Globals.symbolList + name2, out lhs))
            {
                //Scalar is already existing                
                if (lhs.Type() == EVariableType.Matrix)
                {
                    //Already existing lhs is a MATRIX, inject into it. Injecting is faster than recreating an object.
                    ((Matrix)lhs).data = value.data;
                }
                else
                {
                    //The object has to die and be recreated, since it is of a wrong type.                                
                    Program.scalars.Remove(Globals.symbolList + name2);
                    //lhs = new ScalarDate(value);
                    Program.scalars.Add(Globals.symbolList + name2, value);
                }
            }
            else
            {
                //Scalar does not exist beforehand            
                //lhs = new ScalarDate(value);
                Program.scalars.Add(Globals.symbolList + name2, value);
            }
            return (Matrix)lhs;
        }                

        public static bool ContinueIterating(double i, double max, double step) {
            if (step > 0)
            {
                //for instance: FOR VAL i = 1 to 11 by 2; (1, 3, 5, 7, 9, 11)
                //max typically has step/1000000 added, so it might be 11.000002
                return i <= max;  
            }
            else
            {
                return i >= max;  
                //for instance: FOR VAL i = 11 to 1 by -2; (11, 9, 7, 5, 3, 1)
                //max typically has step/1000000 added, so it might be 0.999998
            }
        }

        //used in "FOR date d = ..."
        public static bool ContinueIterating(GekkoTime x, GekkoTime y, int step)
        {
            bool rv = false;
            if (step > 0)
            {
                if (x.SmallerThanOrEqual(y)) rv = true;
            }
            else
            {
                if (x.LargerThanOrEqual(y)) rv = true;
            }
            return rv;
        }

        public static void HandleIndexer(IVariable y, params IVariable[] x)
        {
            HandleIndexerHelper(0, y, x);            
        }

        public static void HandleIndexerHelper(int depth, IVariable y, params IVariable[] x)
        {
            if (depth >= x.Length)
            {
                //HandleIndexerHelper2()
            }          
            MetaList m = (MetaList)x[depth];
            foreach (string s in m.list)
            {
                G.Writeln2(depth + " " + s);
                HandleIndexerHelper(depth + 1, y, x);
            }
        }

        public static IVariable Indexer(GekkoTime t, IVariable x, bool isLhs, params IVariable[] indexes)
        {
            if (x == null)
            {
                if (indexes.Length == 1)
                {
                    //[y]
                    //['q*']
                    ScalarString ss = new ScalarString(Globals.indexerAloneCheatString);  //a bit cheating, but we save an interface method, and performance is not really an issue when indexing whole databanks
                    return ss.Indexer(t, false, indexes);
                }
                else
                {
                    G.Writeln2("*** ERROR: Stand-alone indexer with pattern [... , ... ] not possible");
                    throw new GekkoException();
                }
            }

            //x[y]
            //a[1] or #a['q*']
            //#x[1, 2]                 
            //x['nz', 'w']           
            return x.Indexer(t, isLhs, indexes);
                        
        }               

        public static IVariable IndexerPlus(GekkoTime t, IVariable x, bool isLhs, IVariable y)
        {
            //isLhs will always be false
            if (x == null)
            {
                G.Writeln2("*** ERROR: You cannot use '+' as first character inside a [] wildcard");
                throw new GekkoException();
            }
            else
            {
                //x[+y], #a[+'q*'], hmmmmmmmmmmmmmmm
                //a[+1] ok
                return x.Indexer(t, isLhs, new IVariable[] { y });
            }
        }

        public static IVariable Indexer(GekkoTime t, IVariable x, bool isLhs, IVariablesFilterRange y)
        {            
            if (x == null)
            {
                //[y], where y is y1..y2
                //['fx'..'fy']
                ScalarString ss = new ScalarString(Globals.indexerAloneCheatString);  //a bit cheating, but we save an interface method, and performance is not really an issue when indexing whole databanks
                return ss.Indexer(y, t);
            }
            else
            {
                //x[y], where y is y1..y2
                //a[1..3] or #a['fx'..'fy'] or #a[fx..fy]
                return x.Indexer(y, t);
            }
        }

        public static IVariable Indexer(GekkoTime t, IVariable x, bool isLhs, IVariablesFilterRange y1, IVariablesFilterRange y2)
        {
            if (x == null)
            {
                G.Writeln2("*** ERROR: Invalid syntax");
                throw new GekkoException();                
            }
            else
            {                
                //a[1..3, 2..5] or a[1..3, 5] or a[1, 2..5]                
                return x.Indexer(y1, y2, t);
            }
        }

        public static IVariable Indexer(GekkoTime t, IVariable x, bool isLhs, IVariable y1, IVariablesFilterRange y2)
        {
            if (x == null)
            {
                G.Writeln2("*** ERROR: Invalid syntax");
                throw new GekkoException();
            }
            else
            {
                //a[1..3, 2..5] or a[1..3, 5] or a[1, 2..5]                
                return x.Indexer(y1, y2, t);
            }
        }

        public static IVariable Indexer(GekkoTime t, IVariable x, bool isLhs, IVariablesFilterRange y1, IVariable y2)
        {
            if (x == null)
            {
                G.Writeln2("*** ERROR: Invalid syntax");
                throw new GekkoException();
            }
            else
            {
                //a[1..3, 2..5] or a[1..3, 5] or a[1, 2..5]                
                return x.Indexer(y1, y2, t);
            }
        }        

        //========================================
        //======================================== Z() variants start
        //========================================

        public static IVariable ZScalar(string name)
        {
            IVariable a = null;
            if (Program.scalars.TryGetValue(name, out a))
            {
                
                    //VAL y = %x; <-- %x is a VAL
                    //expected to be of VAL or DATE type (dates can have periods added/subtracted)
            }            
            else
            {
                G.Writeln2("*** ERROR: Memory variable '" + Globals.symbolMemvar + name + "' was not found");
                throw new GekkoException();
            }
            return a;
        }
        
        public static IVariable ZListFile(string fileName)
        {            
            fileName = Program.AddExtension(fileName, "." + "lst");
            List<string> folders = new List<string>();
            string fileNameTemp = Program.SearchForFile(fileName, folders);
            if (fileNameTemp == null)
            {
                G.Writeln2("*** ERROR: Listfile " + fileName + " could not be found");
                throw new GekkoException();
            }
            string listFile = Program.GetTextFromFileWithWait(fileNameTemp);
            List<string> input = G.ExtractLinesFromText(listFile);
            
            List<string> rhs = new List<string>();
            List<string> result = new List<string>();

            GetRawListElements(fileName, input, result);
            if (result.Count == 1 && G.equal(result[0], "null"))
            {
                //LIST mylist = null; ---> empty list
                result = new List<string>();
            }
            MetaList ml = new MetaList(result);
            ml.isNameList = true;
            return ml;
        }

        private static void GetRawListElements(string fileName, List<string> input, List<string> result)
        {
            int counter = 0;
            foreach (string ss in input)
            {
                counter++;
                string s = ss.Trim();  //will also remove any newline characters!

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
                        else if(c.ToString() == Globals.symbolBankColon)
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
                                G.Writeln("    Items should only contain numbers, digits, '_', ':' (or start with '-').", Color.Red);
                                throw new GekkoException();
                            }
                            else
                            {
                                G.Writeln2("*** ERROR in listfile '" + fileName + "', line [" + counter + "], item = '" + s + "'");
                                G.Writeln("    Items should only contain numbers, digits, '_', ':' (or start with '-').", Color.Red);
                                throw new GekkoException();
                            }
                        }
                    }
                }
                result.Add(s);
            }
            return;
        }

        public static IVariable ZList(IVariable iv)
        {
            string name = GetString(iv);
            IVariable a = null;
            if (Program.scalars.TryGetValue(Globals.symbolList + name, out a))
            {
            }
            else
            {
                G.Writeln2("*** ERROR: List '" + Globals.symbolList + name + "' was not found");
                throw new GekkoException();
            }
            return a;
        }

        public static IVariable ZGenr(string name)
        {            
            IVariable a = null;
            if (Program.scalars.TryGetValue(name, out a))
            {
                if (a.Type() == EVariableType.String)
                {
                    //GENR y = %s; <-- %s is a STRING
                    TimeSeries ts = Program.databanks.GetFirst().GetVariable(((ScalarString)a)._string2);
                    //a = new MetaTimeSeries(ts, null, null);
                    a = new MetaTimeSeries(ts);
                }
                else
                {
                    //GENR y = %x; <-- %x is a VAL
                    //expected to be of VAL type, 
                }
            }
            else
            {
                G.Writeln2("*** ERROR: Memory variable '" + Globals.symbolMemvar + name + "' was not found");
                throw new GekkoException();
            }
            return a;
        }        

        public static IVariable CreateValFromCache(ref IVariable a, string originalName)
        {
            //stuff like VAL x = ...
            IVariable x = null;
            if (Globals.useCache && a != null)
            {
                //fast pointer is available (scalar with that name should always point to the same object).
                x = a;                
            }
            else
            {
                //we do not have a fast pointer to the scalar
                Program.scalars.TryGetValue(originalName, out x);
                if (x == null)
                {
                    //create it
                    //if (Globals.runningOnTTComputer) G.Writeln("CreateValFromCache: Created in scalar Dict: " + originalName);
                    //no fast pointer, and not available among scalars
                    x = new ScalarVal(double.NaN);  //create the object
                    Program.scalars.Add(originalName, x);  //add it to the scalars (it will get a value later on)
                    a = x; //point the fast pointer to the new object
                }
                else
                {
                    //no fast pointer, but available among scalars
                    //if (Globals.runningOnTTComputer) G.Writeln("CreateValFromCache: Found in scalar Dict: " + originalName);
                    a = x;  //point the fast pointer to the found object
                }
            }
            if (x.Type() != EVariableType.Val)
            {
                G.Writeln2("*** ERROR: Type change of scalars not yet allowed");
                throw new GekkoException();
            }
            return x;
        }

        public static void SetStringFromCache(ref IVariable a, string originalName, string s, bool isName)
        {
            //stuff like VAL x = ...
            bool createNew = false;
            if (Globals.useCache && a != null)
            {
                //The 'x' scalar has a fast pointer already
                if (a.Type() == EVariableType.String)
                {
                    ((ScalarString)a)._string2 = s;
                    ((ScalarString)a)._isName = isName;
                }
                else
                {
                    //wrong type, does not happen often
                    Program.scalars.Remove(originalName);
                    createNew = true;
                }
            }
            else
            {
                //we do not have a fast pointer to it: ask the dictionary
                IVariable x = null; Program.scalars.TryGetValue(originalName, out x);
                if (x == null)
                {
                    //does not exist beforehand
                    createNew = true;
                }
                else
                {
                    //does exist in the dictionary
                    if (x.Type() == EVariableType.String)
                    {
                        ((ScalarString)x)._string2 = s;
                        ((ScalarString)x)._isName = isName;
                        a = x;
                    }
                    else
                    {
                        //does exist in the dictionary, but has wrong type
                        Program.scalars.Remove(originalName);
                        createNew = true;
                    }
                }
            }
            if (createNew)
            {
                ScalarString a2 = new ScalarString(s, isName);
                Program.scalars.Add(originalName, a2);
                a = a2;
            }
        }

        public static void SetDateFromCache(ref IVariable a, string originalName, GekkoTime gt)
        {
            //stuff like DATE d = ...
            bool createNew = false;
            if (Globals.useCache && a != null)
            {
                //The 'x' scalar has a fast pointer already
                if (a.Type() == EVariableType.Date)
                {
                    ((ScalarDate)a).date = gt;
                }
                else
                {
                    //wrong type, does not happen often
                    Program.scalars.Remove(originalName);
                    createNew = true;
                }
            }
            else
            {
                //we do not have a fast pointer to it: ask the dictionary
                IVariable x = null; Program.scalars.TryGetValue(originalName, out x);
                if (x == null)
                {
                    //does not exist beforehand
                    createNew = true;
                }
                else
                {
                    //does exist in the dictionary
                    if (x.Type() == EVariableType.Date)
                    {
                        ((ScalarDate)x).date = gt;
                        a = x;
                    }
                    else
                    {
                        //does exist in the dictionary, but has wrong type
                        Program.scalars.Remove(originalName);
                        createNew = true;
                    }
                }
            }
            if (createNew)
            {
                ScalarDate a2 = new ScalarDate(gt);
                Program.scalars.Add(originalName, a2);
                a = a2;
            }
        }

        public static void SetValFromCache(ref IVariable a, string originalName, double v)
        {
            //stuff like VAL x = ...
            bool createNew = false;
            if (Globals.useCache && a != null)
            {
                //The 'x' scalar has a fast pointer already
                if (a.Type() == EVariableType.Val)
                {
                    ((ScalarVal)a).val = v;
                }
                else
                {
                    //wrong type, does not happen often
                    Program.scalars.Remove(originalName);
                    createNew = true;
                }
            }
            else
            {                
                //we do not have a fast pointer to it: ask the dictionary
                IVariable x = null; Program.scalars.TryGetValue(originalName, out x);
                if (x == null)
                {
                    //does not exist beforehand
                    createNew = true;
                }
                else
                {
                    //does exist in the dictionary
                    if (x.Type() == EVariableType.Val)
                    {
                        ((ScalarVal)x).val = v;
                        a = x;
                    }
                    else
                    {
                        //does exist in the dictionary, but has wrong type
                        Program.scalars.Remove(originalName);
                        createNew = true;
                    }            
                }                
            }
            if (createNew)
            {
                ScalarVal a2 = new ScalarVal(v);
                Program.scalars.Add(originalName, a2);
                a = a2;
            }
        }

        public static IVariable GetScalarFromCache(ref IVariable a, string originalName)
        {
            return GetScalarFromCache(ref a, originalName, true);
        }

        public static IVariable GetScalarFromCache(ref IVariable a, string originalName, bool transformationAllowed)
        {
            return GetScalarFromCache(ref a, originalName, transformationAllowed, false);
        }

        //see also RemoveScalar()
        public static IVariable GetScalarFromCache(ref IVariable a, string originalName, bool transformationAllowed, bool stringify) 
        {
                       
            //stuff like ... + %x + ...
            if (Globals.useCache && a != null)
            {
                //The typical case, so we try it first.
                return a;
            }
            else
            {                
                //we do not have a fast pointer to it
                IVariable x = null; Program.scalars.TryGetValue(originalName, out x);
                if (x == null)
                {
                    if (originalName.StartsWith(char.ToString(Globals.symbolList)))
                    {
                        G.Writeln2("*** ERROR: Could not find list or matrix '" + originalName + "'");
                        if (Program.scalars.ContainsKey(originalName.Substring(1)))
                        {
                            G.Writeln("    Did you intend to refer to the scalar '" + Globals.symbolMemvar + originalName.Substring(1) + "'", Color.Red);
                        }
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Could not find scalar '" + originalName + "'");
                    }
                    throw new GekkoException();
                }
                bool didTransform = false;
                a = x;
                x = MaybeStringify(x, stringify); //must be after fast pointer, so that a itself is not stringifyed
                x = MaybeTransform(ref didTransform, x, transformationAllowed);
                if (didTransform) a = x; //fast pointer to that object              
                return x;
            }            
        }

        public static IVariable MaybeTransform(ref bool didTransform, IVariable x, bool transformationAllowed)
        {
            if (x == null)
            {
                G.Writeln2("*** ERROR: Illegal transformation of variable");
                throw new GekkoException();
            }
            if (transformationAllowed && x.Type() == EVariableType.String)
            {
                ScalarString ss = (ScalarString)x;
                if (ss._isName)
                {
                    MetaTimeSeries mts = O.GetTimeSeries(ss._string2, 1);
                    x = mts;
                    didTransform = true;
                }
            }
            return x;
        }

        private static IVariable MaybeStringify(IVariable x, bool dollarStringify)
        {
            IVariable rv = null;
            if (dollarStringify)
            {
                if (x.Type() == EVariableType.String)
                {
                    ScalarString ss = (ScalarString)x;
                    ScalarString ss2 = new ScalarString(ss._string2, false);
                    rv = ss2;
                }
                else if (x.Type() == EVariableType.List)
                {
                    MetaList ml = (MetaList)x;
                    MetaList ml2 = new MetaList(ml.list);
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

        public static void ForListCheck(List<string> names)
        {            
            if (names.Count == 2)
            {
                //speeding up the n=2 case. The other cases n>2 are rare.
                if (G.equal(names[0], names[1]))
                {
                    G.Writeln2("*** ERROR: A parallel FOR loop must have different loop variable names.");
                    throw new GekkoException();
                }
            }
            else
            {
                GekkoDictionary<string, string> hashset = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var name in names)
                {
                    if (hashset.ContainsKey(name))
                    {
                        G.Writeln2("*** ERROR: A parallel FOR loop must have different loop variable names.");
                        throw new GekkoException();
                    }
                    else hashset.Add(name, "");
                }
            }
        }

        public static int ForListMax(List<List<string>> x)
        {
            int n = -1;
            foreach (List<string> m in x)
            {
                if (n == -1) n = m.Count;
                else
                {
                    if (n != m.Count)
                    {
                        string s = null;
                        foreach (List<string> mm in x)
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
        public static void RemoveScalar(string originalName)
        {
            //If a pointer also needs to be cleared, put that as second argument (else null)
            if (Program.scalars.ContainsKey(originalName))
            {
                Program.scalars.Remove(originalName);
            }
            //if (a != null) a = null;
        }        

        public static MetaList GetMetaList(IVariable a)
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
            return (MetaList)a;
        }
        
        public static IVariable GetTimeSeriesFromCache(ref IVariable a, string originalName, int bankNumber)
        {
            return GetTimeSeriesFromCache(ref a, originalName, bankNumber, ECreatePossibilities.None);
        }

        public static IVariable GetTimeSeriesFromCache(ref IVariable a, string originalName, int bankNumber, ECreatePossibilities autoCreate)
        {
            //For bankNumber = 2, no cache will ever be used to avoid confusion. Cache is only for bankNumber = 1 (Work).
            //Using bankNumber = 2 is only done in PRT type statements, and these are slow anyway. GENR should NOT use bankNumber = 2,
            //  because it overrides any bank given (so GENR y = x + mybank:z ---> GENR base:y = base:x + mybank:z).
            //  maybe a GENR<base> or GENR<bank=xxx> should simply err if banks are given in expression.
            if (bankNumber != 1)  //maybe a bit faster if bankNumber was a boolean??
            {
                //When quering Base databank, never use a pointer, and never change a pointer!
                //The pointers are only for Work databank objects, and only those with simple names.
                //So matrices will also be covered by that, and they should reside in Work to be fast.
                MetaTimeSeries ats = GetTimeSeries(originalName, bankNumber, autoCreate);
                return ats;
            }
            else
            {
                //stuff like ... + fy + ...
                if (Globals.useCache && a != null)
                {
                    //The typical case, so we try it first.
                    return a;
                }
                else
                {
                    //Should not happen too often...                
                    MetaTimeSeries ats = GetTimeSeries(originalName, bankNumber, autoCreate);
                    a = ats;  //sets the pointer
                    return ats;
                }
            }
        }        

        public static MetaTimeSeries GetTimeSeriesFromList(IVariable list, IVariable index, int bankNumber, GekkoTime t)
        {
            if (list.Type() == EVariableType.List)
            {
                ScalarString x = (ScalarString)list.Indexer(t, false, new IVariable[] { index });  //will return ScalarString with .isName = true.
                MetaTimeSeries mts = O.GetTimeSeries(x._string2, 1);  //always from work....
                return mts;
            }
            else
            {
                G.Writeln2("*** ERROR #398754");
                throw new GekkoException();
            }            
        }

        public static IVariable GetValFromStringIndexer(string name, IVariable index, int bank, GekkoTime t)
        {
            //Used to pick out a value from a list item, like #m[2][2015], where index=2015
            MetaTimeSeries mts = O.GetTimeSeries(name, bank);  //always from work....
            IVariable result = O.Indexer(t, mts, false, index);
            return result;
        }

        public static MetaTimeSeries GetTimeSeries(string originalName, int bankNumber)
        {
            return GetTimeSeries(originalName, bankNumber, ECreatePossibilities.None);
        }

        public static MetaTimeSeries GetTimeSeries(string originalName, int bankNumber, ECreatePossibilities canAutoCreate)
        {
            ExtractBankAndRestHelper h = Program.ExtractBankAndRest(originalName, EExtrackBankAndRest.OnlyStrings);

            if (h.bank == Globals.firstCheatString)
            {
                h.bank = Program.databanks.GetFirst().aliasName;
            }

            if (bankNumber == 2)
            {
                h.bank = Program.databanks.GetRef().aliasName;  //overrides the bank name given
                h.hasColon = true;  //signals later on that this bank is explicitely given, so we cannot search for the timeseries
            }
            TimeSeries ts = Program.FindOrCreateTimeseries(h.bank, h.name, canAutoCreate, h.hasColon, false);            
            MetaTimeSeries mts = new MetaTimeSeries(ts);
            return mts;
        }
                
        //public static MetaTimeSeries GetArrayTimeSeries(MetaTimeSeries mts, ECreatePossibilities create, params IVariable[] indexes)
        //{
        //    if (indexes.Length == 0) return mts;  //fast return for normal timeseries
        //    TimeSeries ts = O.GetArrayTimeSeries(mts.ts, create, indexes);            
        //    return new MetaTimeSeries(ts);
        //}

        //public static TimeSeries GetArrayTimeSeries(TimeSeries its, ECreatePossibilities create, IVariable[] indexes)
        //{
        //    //When a timeseries is on rhs, it will have crete=none. In that case, we expect dimension to fit. If dimension is -12345,
        //    //the timeseries does exist but has never been written to (is that possible)? 

        //    if (create == ECreatePossibilities.None)
        //    {
        //        //rhs
        //        if (its.dimensions == -12345)
        //        {
        //            //should not be possible?
        //            G.Writeln2("*** ERROR: Timeseries " + its.variableName + " has no dimensions and no data");
        //            throw new GekkoException();
        //        }
        //        if (its.dimensions != indexes.Length)
        //        {
        //            G.Writeln2("*** ERROR: The timeseries " + its.variableName + " has " + its.dimensions + " dimensions,");
        //            G.Writeln("           but the []-indexer has " + indexes.Length + " dimensions", Color.Red);
        //            throw new GekkoException();
        //        }
        //    }
        //    else
        //    {
        //        //lhs
        //        if (its.dimensions != -12345 && (its.dimensions != indexes.Length))
        //        {
        //            G.Writeln2("*** ERROR: The timeseries " + its.variableName + " has " + its.dimensions + " dimensions,");
        //            G.Writeln("           but the []-indexer has " + indexes.Length + " dimensions");
        //            throw new GekkoException();
        //        }
        //    }

        //    TimeSeries ts = null;

        //    if (indexes.Length == 0)
        //    {
        //        ts = its;  //relevant for normal timeseries
        //    }
        //    else
        //    {
        //        //We know that indexes.Length >= 1.
        //        //Now dimension may be (a) -12345 [only for create=yes LHS] or (b) same as indexes.Length.                             

        //        string hash = null;
        //        //this produces a string like "b,nz,w"
        //        for (int i = 0; i < indexes.Length; i++)
        //        {
        //            if (indexes[i].Type() != EVariableType.String)
        //            {
        //                G.Writeln2("*** ERROR: Expected " + its.variableName + "[] indexer element #" + (i + 1) + " to be STRING");
        //                throw new GekkoException();
        //            }                    
        //            hash += ((ScalarString)indexes[i])._string2;
        //            if (i < indexes.Length - 1) hash += Globals.symbolTurtle; //ok as delimiter
        //        }

        //        //if (its.dimensions == -12345)
        //        //{
        //        //    //can only be so for LHS type, RHS would give an exception above
        //        //    its.dim = new Gekko.Dim();
        //        //    its.dim.timeSeriesArray = new GekkoDictionary<string, TimeSeries>(StringComparer.OrdinalIgnoreCase);
        //        //    its.dimensions = indexes.Length;
        //        //}                               
                
        //        //we know that dimension >= 1                
                                
        //        its.dim.timeSeriesArray.TryGetValue(hash, out ts);                                
                
        //        if (ts == null)
        //        {
        //            if (create != ECreatePossibilities.None)
        //            {
        //                ts = new TimeSeries(its.freqEnum, its.variableName + "[]");  //the name is not really used, but we could put in the indices...?
        //                its.dim.timeSeriesArray.Add(hash, ts);  //put it in
        //            }
        //            else
        //            {
        //                G.Writeln2("*** ERROR: Array-timeseries " + its.variableName + "[" + G.PrettifyTimeseriesHash(hash) + "] not found");
        //            }
        //        }
        //    }

        //    return ts;
        //}

        public static IVariable GetListWithBankPrefix(IVariable x, IVariable y, int bankNumber)
        {
            string bankName = O.GetString(x);
            List<string> items = O.GetList(y);
            List<string> newList = new List<string>();
            foreach (string s in items)
            {
                newList.Add(bankName + ":" + s);
            }
            return new MetaList(newList);
        }

        public static IVariable ZIndexer(string name)
        {
            IVariable a = null;
            if (Program.scalars.TryGetValue(name, out a))
            {
                //VAL y = %s[2000]; <-- %s is a STRING
                //GENR y = %s[2000]; <-- %s is a STRING
                TimeSeries ts = Program.databanks.GetFirst().GetVariable(((ScalarString)a)._string2);
                //a = new MetaTimeSeries(ts, null, null);
                a = new MetaTimeSeries(ts);
            }
            else
            {
                G.Writeln2("*** ERROR: Memory variable '" + Globals.symbolMemvar + name + "' was not found");
                throw new GekkoException();
            }
            return a;
        }

        //========================================
        //======================================== Z() variants end
        //========================================
        
        public static IVariable Z_OLD(string name)
        {
            IVariable a = GetScalar(name);
            return a;
        }


        // =================================== start comparisons ==================================

        public static bool StrictlySmallerThan(IVariable x, IVariable y, GekkoTime t)
        {
            bool rv = false;
            if (x.Type() == EVariableType.Date && x.Type() == EVariableType.Date)
            {
                if (O.GetDate(x).StrictlySmallerThan(O.GetDate(y))) rv = true;
            }
            else if (x.Type() == EVariableType.Val && y.Type() == EVariableType.Val)
            {
                if (x.GetVal(t) < y.GetVal(t)) rv = true;
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types do not match for '<' compare");
                throw new GekkoException();
            }
            return rv;
        }

        public static bool SmallerThanOrEqual(IVariable x, IVariable y, GekkoTime t)
        {
            bool rv = false;
            if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (O.GetDate(x).SmallerThanOrEqual(O.GetDate(y))) rv = true;
            }
            else if (x.Type() == EVariableType.Val && y.Type() == EVariableType.Val)
            {
                if (x.GetVal(t) <= y.GetVal(t)) rv = true;
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types do not match for '<=' compare");
                throw new GekkoException();
            }
            return rv;
        }

        public static bool Equals(IVariable x, IVariable y, GekkoTime t)
        {
            bool rv = false;
            if (x.Type() == EVariableType.Val && y.Type() == EVariableType.Val)
            {
                if (x.GetVal(t) == y.GetVal(t)) rv = true;
            }
            else if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (O.GetDate(x).IsSamePeriod(O.GetDate(y))) rv = true;
            }
            else if (x.Type() == EVariableType.String && y.Type() == EVariableType.String)
            {
                if (G.equal(x.GetString(), y.GetString())) rv = true;
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types do not match for '==' compare");
                throw new GekkoException();
            }
            return rv;
        }

        public static bool ListContains(IVariable x, IVariable y)
        {
            
            if (x.Type() != EVariableType.List || y.Type() != EVariableType.String)
            {
                G.Writeln2("*** ERROR: Expected syntax like ... $ #a['b'], with list and string");
                throw new GekkoException();
            }
            MetaList ml = (MetaList)x;
            ScalarString ss = (ScalarString)y;

            return ml.list.Contains(ss._string2);           
            
        }

        public static bool LargerThanOrEqual(IVariable x, IVariable y, GekkoTime t)
        {
            bool rv = false;
            if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (O.GetDate(x).LargerThanOrEqual(O.GetDate(y))) rv = true;
            }
            else if (x.Type() == EVariableType.Val && y.Type() == EVariableType.Val)
            {
                if (x.GetVal(t) >= y.GetVal(t)) rv = true;
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types do not match for '>=' compare");
                throw new GekkoException();
            }
            return rv;
        }

        public static bool StrictlyLargerThan(IVariable x, IVariable y, GekkoTime t)
        {
            bool rv = false;
            if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (O.GetDate(x).StrictlyLargerThan(O.GetDate(y))) rv = true;
            }
            else if (x.Type() == EVariableType.Val && y.Type() == EVariableType.Val)
            {
                if (x.GetVal(t) > y.GetVal(t)) rv = true;
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types do not match for '>' compare");
                throw new GekkoException();
            }
            return rv;
        }        
        
        // =================================== end comparisons ==================================        

        public static string SubstituteScalarsAndLists(string label, bool reportError)
        {
            return label;
        }
        
        public static List<string> SearchWildcard(IVariable a)
        {
            string s = O.GetString(a);
            List<string> l = new List<string>();
            l = Program.MatchWildcardInDatabank(s, Program.databanks.GetFirst());            
            return l;
        }

        public static MetaTimeSeries IndirectionHelper(string variable)
        {
            //In that case, we are inside a GENR/PRT implicit time loop                        
            //Code below implicitly calls Program.ExtractBankAndRest and Program.FindOrCreateTimeseries()
            //So stuff in banks down the list will be found in data mode
            MetaTimeSeries ats = O.GetTimeSeries(variable, 0);            
            return ats;
        }        

        public static IVariable GetScalar(string name)
        {
            return GetScalar(name, true);
        }

        public static IVariable GetScalar(string name, bool transformationAllowed)
        {
            return GetScalar(name, transformationAllowed, false);
        }

        public static IVariable GetScalar(string name, bool transformationAllowed, bool dollarStringify)
        {
            IVariable a = null;
            if (Program.scalars.TryGetValue(name, out a))
            {
                bool didTransform = false;
                a = MaybeStringify(a, dollarStringify);
                a = MaybeTransform(ref didTransform, a, transformationAllowed);
                return a;
            }
            else
            {
                G.Writeln2("*** ERROR: Memory variable '" + Globals.symbolMemvar + name + "' was not found");
                throw new GekkoException();
            }
        }                

        public static GekkoTime GetDate(IVariable x, GetDateChoices c)
        {            
            return x.GetDate(c);                        
        }

        public static GekkoTime GetDate(IVariable x)
        {
            return GetDate(x, GetDateChoices.Strict);
        }

        public static string GetString(IVariable a)
        {
            return a.GetString();            
        }

        public static string GetString(string s)
        {
            return s;
        }

        public static List<string> GetList(IVariable a)
        {
            return a.GetList();
        }

        public static Matrix GetMatrixFromString(IVariable name)
        {
            string name2 = name.GetString();
            IVariable lhs = null;            
            if (Program.scalars.TryGetValue(Globals.symbolList + name2, out lhs))
            {
                //Scalar is already existing                
                if (lhs.Type() == EVariableType.Matrix)
                {
                    //fine
                }
                else
                {
                    G.Writeln2("*** ERROR: " + Globals.symbolList + name2 + " is not a matrix");
                    throw new GekkoException();
                }
            }
            else
            {
                G.Writeln2("*** ERROR: " + Globals.symbolList + name2 + " could not be found");
                throw new GekkoException();
            }
            return (Matrix)lhs;
        }


        public static Matrix GetMatrix(IVariable a)
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
            int FirstDim=-12345;
            int SecondDim=-12345;

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
            if (Program.options.freq == EFreq.Quarterly) lag = Globals.freqQSubperiods;
            else if (Program.options.freq == EFreq.Monthly) lag = Globals.freqMSubperiods;
            return lag;
        }

        public static IVariable HandleLags(string type, double[] storage, int i1, int i2)
        {
            //i1 and i2 are often not used
            double data = double.NaN;
            switch(type)
            {
                case "movavg":
                case "movsum":
                    {
                        double sum = 0d;
                        for (int i = 0; i < storage.Length; i++)
                        {
                            sum += storage[i];
                        }
                        if (type == "movavg") data = sum / (double)storage.Length;
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

        public static List<string> GetList(List<string>l)
        {
            return l;
        }

        public static double GetVal(IVariable a, GekkoTime t)
        {
            return a.GetVal(t);            
        }

        public static void GetVal777(IVariable a, int bankNumber, O.Prt.Element e, GekkoTime t)  //used in PRT and similar, can accept a list that will show itself as a being an integer with ._isName set.
        {
            if (a.Type() == EVariableType.List)
            {
                List<string> items = ((MetaList)a).list;
                double[] d = new double[items.Count];
                
                if (e.subElements == null)
                {
                    e.subElements = new List<O.Prt.SubElement>();
                    for (int i = 0; i < items.Count; i++)
                    {
                        O.Prt.SubElement opeSub0 = new O.Prt.SubElement();
                        e.subElements.Add(opeSub0);
                    }
                }                                

                for (int i = 0; i < items.Count; i++)
                {
                    string s = items[i];
                    double dd = O.GetVal(O.GetTimeSeries(s, bankNumber), t);                    
                    if (bankNumber == 1)
                    {
                        if (e.subElements[i].tsWork == null) e.subElements[i].tsWork = new TimeSeries(Program.options.freq, null);
                        e.subElements[i].tsWork.SetData(t, dd);
                    }
                    else
                    {
                        if (e.subElements[i].tsBase == null) e.subElements[i].tsBase = new TimeSeries(Program.options.freq, null);
                        e.subElements[i].tsBase.SetData(t, dd);
                    }
                    if (e.subElements[i].label == null) e.subElements[i].label = s;  //this is a bit slow because it gets repeated for each t, but PRT is slow anyways, and it only slows down list-unfolding
                }                

                return;
            }
            else
            {
                if (e.subElements == null)
                {
                    e.subElements = new List<O.Prt.SubElement>();                    
                    O.Prt.SubElement opeSub0 = new O.Prt.SubElement();
                    e.subElements.Add(opeSub0);                    
                }                                                
                
                double dd = a.GetVal(t);                

                if (bankNumber == 1)
                {
                    if (e.subElements[0].tsWork == null) e.subElements[0].tsWork = new TimeSeries(Program.options.freq, null);
                    e.subElements[0].tsWork.SetData(t, dd);
                }
                else
                {
                    if (e.subElements[0].tsBase == null) e.subElements[0].tsBase = new TimeSeries(Program.options.freq, null);
                    e.subElements[0].tsBase.SetData(t, dd);
                }

                //The return value is not used, but we keep it for now...                
                return;                
            }            
        }

        public static double GetVal(IVariable a, int bankNumber, GekkoTime t)  //used in PRT and similar, can accept a list that will show itself as a being an integer with ._isName set.
        {            
            return a.GetVal(t);            
        }               
                
        public static TimeSeries GetTimeSeries(IVariable a)
        {
            if (a.Type() == EVariableType.TimeSeries)
            {
                return ((MetaTimeSeries)a).ts;
            }
            else if (a.Type() == EVariableType.String)
            {
                ScalarString ss = (ScalarString)a;
                if (ss._isName)
                {
                    MetaTimeSeries mts = O.GetTimeSeries(ss._string2, 1);
                    return mts.ts;
                }
                else
                {
                    G.Writeln2("*** ERROR: Cannot convert STRING into SERIES, use a NAME-scalar or {}-braces");
                    throw new GekkoException();
                }
            }
            else throw new GekkoException();
        }

        public static int GetInt(IVariable a)
        {
            //GetInt() is really just GetVal() converted to int afterwards.
            if (a.Type() == EVariableType.TimeSeries)
            {
                G.Writeln2("*** ERROR: Using GetInt() on timeseries.");
                G.Writeln("           Did you forget []-brackets to pick out an observation, for instance x[2020]?");
                throw new GekkoException();
            }
            double d = GetVal(a, Globals.tNull);
            int intValue = -12345;
            if (!G.Round(out intValue, d))
            {
                G.Writeln2("*** ERROR: Could not convert value '" + d + "' into integer");
                throw new GekkoException();
            }
            return intValue;
        }

        //Common methods end
        //Common methods end
        //Common methods end

        public class Read
        {
            //also covers IMPORT
            public GekkoTime t1 = Globals.tNull;
            public GekkoTime t2 = Globals.tNull;
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
            public string type = null;  //read or import
            public P p = null;
            public void Exe()
            {
                ReadOpenMulbkHelper hlp = new ReadOpenMulbkHelper();  //This is a bit confusing, using an old object to store the stuff.
                hlp.t1 = this.t1;
                hlp.t2 = this.t2;
                
                bool isRead = false; if (G.equal(this.type, "read")) isRead = true;

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
                if (G.equal(this.opt_csv, "yes")) hlp.Type = EDataFormat.Csv;
                if (G.equal(this.opt_prn, "yes")) hlp.Type = EDataFormat.Prn;
                if (G.equal(this.opt_pcim, "yes")) hlp.Type = EDataFormat.Pcim;
                if (G.equal(this.opt_tsd, "yes")) hlp.Type = EDataFormat.Tsd;
                if (G.equal(this.opt_gbk, "yes")) hlp.Type = EDataFormat.Gbk;
                if (G.equal(this.opt_tsdx, "yes")) hlp.Type = EDataFormat.Tsdx;
                if (G.equal(this.opt_tsp, "yes")) hlp.Type = EDataFormat.Tsp;
                if (G.equal(this.opt_xls, "yes")) hlp.Type = EDataFormat.Xls;
                if (G.equal(this.opt_xlsx, "yes")) hlp.Type = EDataFormat.Xlsx;
                if (G.equal(this.opt_gdx, "yes")) hlp.Type = EDataFormat.Gdx;
                if (G.equal(this.opt_px, "yes")) hlp.Type = EDataFormat.Px;
                if (G.equal(this.opt_cols, "yes")) hlp.Orientation = "cols";

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
                    if (G.equal(this.opt_merge, "yes")) hlp.Merge = true;
                    if (G.equal(this.opt_first, "yes")) hlp.openType = EOpenType.First;
                    if (G.equal(this.opt_ref, "yes")) hlp.openType = EOpenType.Ref;
                    if (hlp.openType == EOpenType.Normal) isSimple = true;  //in that case, a CLONE is done afterwards
                    if (isRead)
                    {
                        if (hlp.openType == EOpenType.First)
                        {
                            if (Program.databanks.GetFirst().protect)
                            {
                                G.Writeln2("*** ERROR: Cannot READ<first>, since first-position databank is non-editable");
                                throw new GekkoException();
                            }
                        }
                        else if (hlp.openType == EOpenType.Ref)
                        {
                            if (Program.databanks.GetRef().protect)
                            {
                                G.Writeln2("*** ERROR: Cannot READ<ref>, since ref databank is non-editable");
                                throw new GekkoException();
                            }
                        }
                        else
                        {
                            if (Program.databanks.GetFirst().protect)
                            {
                                G.Writeln2("*** ERROR: Cannot READ, since first-position databank is non-editable");
                                throw new GekkoException();
                            }
                            if (Program.databanks.GetRef().protect)
                            {
                                G.Writeln2("*** ERROR: Cannot READ, since ref databank is non-editable");
                                throw new GekkoException();
                            }
                        }
                    }
                    else
                    {
                        //IMPORT
                        if (Program.databanks.GetFirst().protect)
                        {
                            G.Writeln2("*** ERROR: Cannot IMPORT, since first-position databank is non-editable");
                            throw new GekkoException();
                        }
                    }
                }                

                if (G.equal(Program.options.interface_mode, "data"))
                {
                    if (isRead && isSimple)
                    {
                        G.Writeln2("+++ WARNING: General READ is not intended for data-mode.");
                        G.Writeln("             Please use IMPORT, or consider READ<first>", Color.Red);                        
                    }
                    if (isRead && !isTo && hlp.openType == EOpenType.Ref)
                    {
                        G.Writeln2("+++ WARNING: READ<ref> is not intended for data-mode.");
                        //G.Writeln("             Please use IMPORT, or consider READ<first>", Color.Red);
                        //throw new GekkoException();
                    }
                }                                                              

                List<Program.ReadInfo> readInfos = new List<Program.ReadInfo>();

                bool open = false;
                if (isTo)                
                {
                    //is in reality an OPEN                    
                    open = true;
                    hlp.Merge = false;  //but mixing <merge> and TO give error above anyway                
                    hlp.protect = true;  //superfluous but for safety
                    hlp.openType = EOpenType.Normal;
                    if (readTo == "ASTBANKISSTARCHEATCODE")
                    {
                        readTo = Path.GetFileNameWithoutExtension(hlp.FileName);
                    }
                    hlp.openFileNames = new List<List<string>>();
                    hlp.openFileNames.Add(new List<string>() {hlp.FileName, readTo});                    
                }

                bool wipeDatabankBeforeInsertingData = false;

                if(isRead && !hlp.Merge && !isTo)
                {
                    //See #987432529835
                    //READ, not IMPORT
                    //No READ<merge>
                    //No READ ... TO
                    wipeDatabankBeforeInsertingData = true;
                }

                Program.OpenOrRead(wipeDatabankBeforeInsertingData, hlp, open, readInfos);
                Program.ReadInfo readInfo = readInfos[0];
                readInfo.shouldMerge = hlp.Merge;                

                if (readInfo.abortedStar) return;  //an aborted READ *

                if (G.equal(opt_ref, "yes"))
                {
                    readInfo.dbName = Program.databanks.GetRef().aliasName;
                }
                else
                {
                    readInfo.dbName = Program.databanks.GetFirst().aliasName;
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
                    if (Program.model != null && (G.equal(Program.options.interface_mode, "sim") || G.equal(Program.options.interface_mode, "mixed")))
                    {
                        //only in sim or mixed mode, if a model is existing
                        CreateMissingModelVariables();
                    }
                }                

                try
                {
                    Program.ShowPeriodInStatusField("");
                }
                catch (Exception e)
                {
                    //ignore
                }

            }

            private static void CreateMissingModelVariables()
            {
                List<string> onlyDatabankNotModel = new List<string>();
                List<string> onlyModelNotDatabank = new List<string>();
                foreach (string s in Program.databanks.GetFirst().storage.Keys)
                {
                    if (G.GetFreqFromKey(s) != Program.options.freq) continue;
                    string s2 = G.RemoveFreqFromKey(s);
                    if (!Program.model.varsAType.ContainsKey(s2)) onlyDatabankNotModel.Add(s2);
                }
                foreach (string s in Program.model.varsAType.Keys)
                {                    
                    if (!Program.databanks.GetFirst().ContainsVariable(s)) onlyModelNotDatabank.Add(s);
                }
                if (G.equal(Program.options.interface_mode, "sim"))
                {
                    //See #8904327598432
                    if (onlyDatabankNotModel.Count > 0)
                    {
                        G.Writeln("+++ WARNING: There are " + onlyDatabankNotModel.Count + " non-model timeseries in the databank");
                        G.Writeln("             You may use 'DELETE<nonmodel>' to remove those superfluous timeseries.");
                    }
                    if (onlyModelNotDatabank.Count > 0)
                    {
                        G.Writeln("+++ WARNING: There are " + onlyModelNotDatabank.Count + " non-databank timeseries in the model");
                        G.Writeln("             You may use 'CREATE #all;' to create these timeseries.");
                    }
                }
                else
                {
                    //See #8904327598432
                    if (onlyDatabankNotModel.Count > 0)
                    {
                        G.Writeln("+++ NOTE: There are " + onlyDatabankNotModel.Count + " non-model timeseries in the databank");
                        G.Writeln("          You may use 'DELETE<nonmodel>' to remove those superfluous timeseries.");
                    }
                    if (onlyModelNotDatabank.Count > 0)
                    {
                        G.Writeln("+++ NOTE: There are " + onlyModelNotDatabank.Count + " non-databank timeseries in the model");
                        G.Writeln("          You may use 'CREATE #all;' to create these timeseries.");
                    }
                }
            }
        }

        public class Download
        {
            public string dbUrl = null;  //path to server
            public string fileName = null;  //json file
            public string fileName2 = null;  //dump file (for instance px)            
            public string opt_array = null;  //not in use
            public void Exe()
            {
                OnlineDatabanks.Download(this);
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
            public List<string> listItems = null;
            public string opt_sheet = null;
            public string opt_cell = null;            
            public string opt_rows = null;
            public string opt_cols = null;
            public string opt_matrix = null;
            public string opt_missing = null;  //used for matrix   
            public void Exe()
            {
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
                G.Writeln("Cleared reference databank ('" + Program.databanks.GetRef().aliasName + "') and copied " + number + " variables from first-position ('" + Program.databanks.GetFirst().aliasName + "') to reference ('" + Program.databanks.GetRef().aliasName + "') databank");
                if (G.equal(Program.options.interface_mode, "data"))
                {
                    G.Writeln2("+++ WARNING: CLONE is not intended for data-mode (cf. MODE)");
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
            public string name = null;
            public P p = null;
            public string opt_first = null;
            public string opt_ref = null;
            public void Exe()
            {
                Program.Clear(this, p);
            }
        }

        public class Val
        {            
            public static void Q(string s)
            {                
                IVariable a = null; Program.scalars.TryGetValue(s, out a);
                if (a == null || a.Type() != EVariableType.Val)
                {
                    G.Writeln2("*** ERROR: VAL " + Globals.symbolMemvar.ToString() + s + " was not found");
                    throw new GekkoException();
                }
                string ss = a.GetVal(Globals.tNull).ToString();
                if (ss == "NaN") ss = "M";
                G.Writeln2("VAL " + s + " = " + ss);                
            }
            public static void Q()
            {
                Program.Mem("val");
            }
        }

        public class String2
        {
            public static void Q(string s)
            {                
                IVariable a = null; Program.scalars.TryGetValue(s, out a);
                if (a == null || a.Type() != EVariableType.String || ((ScalarString)a)._isName == true)
                {
                    G.Writeln2("*** ERROR: STRING "+ Globals.symbolMemvar.ToString()  + s + " was not found");
                    throw new GekkoException();
                }
                G.Writeln2("STRING " + s + " = '" + a.GetString() + "'");
            }
            public static void Q()
            {
                Program.Mem("string");
            }
        }

        public class Matrix2
        {
            public static void Q(string s)
            {                
                Show ss = new Show();
                IVariable iv = null; Program.scalars.TryGetValue(Globals.symbolList.ToString() + s, out iv);
                if (iv == null)
                {
                    G.Writeln2("Matrix " + Globals.symbolList.ToString() + s + " not found");
                    return;
                }
                ss.input = iv;
                ss.label = Globals.symbolList.ToString() + s;
                ss.Exe();
            }
            public static void Q()
            {
                bool first = true;
                int count = 0;
                foreach (string s in Program.scalars.Keys)
                {
                    IVariable a = Program.scalars[s];  //no need for tryget                    
                    if (a.Type() == EVariableType.Matrix)
                    {
                        Matrix m = (Matrix)a;
                        if (first) G.Writeln();
                        G.Writeln("MATRIX " + s + " with dimension " + m.data.GetLength(0) + " x " + m.data.GetLength(1));
                        first = false;
                        count++;
                    }                    
                }
                if (count == 0) G.Writeln2("Did not find any matrices");
            }
        }

        public class Name
        {
            public static void Q(string s)
            {
                IVariable a = null; Program.scalars.TryGetValue(s, out a);
                if (a == null || a.Type() != EVariableType.String || ((ScalarString)a)._isName == false)
                {
                    G.Writeln2("*** ERROR: NAME " + Globals.symbolMemvar.ToString() + s + " was not found");
                }
                G.Writeln2("NAME " + s + " = '" + a.GetString() + "'");
            }
            public static void Q()
            {
                Program.Mem("name");
            }
        }

        public class Date
        {
            public static void Q(string s)
            {
                IVariable a = null; Program.scalars.TryGetValue(s, out a);
                if (a == null || a.Type() != EVariableType.Date)
                {
                    G.Writeln2("*** ERROR: DATE " + Globals.symbolMemvar.ToString() + s + " was not found");
                }
                G.Writeln2("DATE " + s + " = " + G.FromDateToString(O.GetDate(a)));
            }
            public static void Q()
            {
                Program.Mem("date");
            }
        }        

        public class Restart
        {
            public P p = null;
            public void Exe()
            {
                Program.Re("restart", p);
            }
        }

        public class Reset
        {
            public P p = null;
            public void Exe()
            {
                Program.Re("reset", p);
            }
        }

        public class Smooth
        {
            //public IVariable rhs = null;
            //public IVariable lhs = null;

            public List<string> listItems0;
            public List<string> listItems1;
            public string opt_spline = null;
            public string opt_geometric = null;
            public string opt_linear = null;
            public string opt_repeat = null;
            public P p = null;
            public void Exe()
            {
                //If the method fails, no timeseries are touched. That is good!
                
                //all this could be sped up by means of using the internal arrays (GetDataSequence).
                //so some room for improvement if it becomes a bottleneck.
                //only done for missings enclosed by real numbers (no end-missings will be filled)  

                if (listItems0.Count != 1 || listItems1.Count != 1)
                {
                    G.Writeln2("*** ERROR: SMOOTH only supports one variable at the time, not lists (for now)");
                    throw new GekkoException();
                }

                TimeSeries oldSeries = O.GetTimeSeries(O.GetTimeSeries(this.listItems1[0], 1));
                TimeSeries lhs = O.GetTimeSeries(O.GetTimeSeries(this.listItems0[0], 1));
                                
                TimeSeries newSeriesTemp = oldSeries.Clone();  //brand new object, not present in Work (yet)                

                ESmoothTypes type = ESmoothTypes.Spline;  //what is the default in AREMOS??
                if (G.equal(opt_geometric, "yes")) type = ESmoothTypes.Geometric;
                if (G.equal(opt_linear, "yes")) type = ESmoothTypes.Linear;
                if (G.equal(opt_spline, "yes")) type = ESmoothTypes.Spline;
                if (G.equal(opt_repeat, "yes")) type = ESmoothTypes.Repeat;                

                GekkoTime realStart = oldSeries.GetRealDataPeriodFirst();
                GekkoTime realEnd = oldSeries.GetRealDataPeriodLast();
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
                        double data = oldSeries.GetData(gt);
                        if (G.isNumericalError(data))
                        {
                            missings.Add(counter);
                            missingsDates.Add(gt);
                            continue;  //ignore this observation
                        }
                        yy.Add(oldSeries.GetData(gt));
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
                    GekkoTime missingStart = Globals.tNull;
                    bool recording = false;
                    foreach (GekkoTime gt in new GekkoTimeIterator(realStart, realEnd))
                    {
                        double z = oldSeries.GetData(gt);
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
                            double z1 = oldSeries.GetData(t1);
                            double z2 = oldSeries.GetData(t2);
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
                            missingStart = Globals.tNull;
                        }
                    }
                }
                else throw new GekkoException();
                foreach (GekkoTime gt in new GekkoTimeIterator(realStart, realEnd))
                {
                    //This is not terribly efficient, and we could use array copy etc.
                    //And we do create and clone a whole new timeseries (newSeriesTemp).
                    //But it works, and speed is probably not an issue with SMOOTH.
                    lhs.SetData(gt, newSeriesTemp.GetData(gt));
                }
                lhs.Stamp();
            }            
        }

        public class Splice
        {
            
            public List<string> listItems0;
            public List<string> listItems1;
            public List<string> listItems2;

            public GekkoTime date = Globals.tNull;
            public void Exe()
            {
                bool useSecondPartLevels = true;  //like aremos

                if (listItems0.Count != 1 || listItems1.Count != 1 || listItems2.Count != 1)
                {
                    G.Writeln2("*** ERROR: SPLICE only supports one variable at a time, not lists (for now)");
                    throw new GekkoException();
                }

                TimeSeries ts1 = O.GetTimeSeries(O.GetTimeSeries(listItems1[0], 1));
                TimeSeries ts2 = O.GetTimeSeries(O.GetTimeSeries(listItems2[0], 1));
                TimeSeries ts3 = O.GetTimeSeries(O.GetTimeSeries(listItems0[0], 1));
                //if (ts3 == null)
                //{
                //    ts3 = new TimeSeries(Program.options.freq, lhs);
                //    Program.databanks.GetPrim().AddVariable(ts3);
                //}
                if (ts1.freqEnum != ts2.freqEnum)
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
                    if (date.freq != ts1.freqEnum || date.freq != ts2.freqEnum)
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
                    sum1 += ts1.GetData(gt);
                    sum2 += ts2.GetData(gt);
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
                        ts3.SetData(gt, ts1.GetData(gt) / relative);
                    }
                    foreach (GekkoTime gt in new GekkoTimeIterator(t2a, t2b))
                    {
                        ts3.SetData(gt, ts2.GetData(gt));
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
                        ts3.SetData(gt, ts1.GetData(gt));
                    }
                    foreach (GekkoTime gt in new GekkoTimeIterator(t1b.Add(1), t2b))
                    {
                        ts3.SetData(gt, ts2.GetData(gt) * relative);
                    }                    
                }
                ts3.Stamp();
                G.Writeln2("Spliced '" + ts3.variableName + "' by means of " + obs + " common observations");
            }
        }

        public class Lock
        {
            public string bank = null;
            public void Exe()
            {
                if (G.equal(bank, Globals.Work))
                {
                    G.Writeln2("*** ERROR: Work databank cannot be set non-editable");
                    throw new GekkoException();
                }
                if (G.equal(bank, Globals.Ref))
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
                if (db.protect == true)
                {
                    G.Writeln2("Databank '" + this.bank + "' is already non-editable");
                }
                else
                {
                    db.protect = true;
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
                if (db.protect == false)
                {
                    G.Writeln2("Databank '" + this.bank + "' is already editable");
                }
                else
                {
                    db.protect = false;
                    G.Writeln2("Databank '" + this.bank + "' set editable");
                }
            }
        }

        public class Close
        {
            public string name = null;
            public string opt_save = null;
            public void Exe()
            {
                
                List<string> databanks = new List<string>();
                if (this.name == "*")
                {
                    foreach (Databank db in Program.databanks.storage)
                    {
                        if (G.equal(db.aliasName, Globals.Work) || G.equal(db.aliasName, Globals.Ref))
                        {
                        }
                        else databanks.Add(db.aliasName);
                    }
                }
                else
                {
                    if (G.equal(this.name, Globals.Work) || G.equal(this.name, Globals.Ref))
                    {
                        G.Writeln2("*** ERROR: Databanks '" + Globals.Work + "' or '" + Globals.Ref + "' cannot be closed (see CLEAR command)");
                        throw new GekkoException();
                    }
                    databanks.Add(this.name);
                }
                foreach (string databank in databanks)
                {
                    if (Program.databanks.GetDatabank(databank) == null)
                    {
                        G.Writeln2("*** ERROR: Trying to close non-existing databank '" + databank + "'");
                        throw new GekkoException();
                    }
                    Databank removed = Program.databanks.RemoveDatabank(databank);
                    if (G.equal(opt_save, "no"))
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
                    if (G.equal(opt_save, "no"))
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
            public List<List<string>> openFileNames = new List<List<string>>();
            //public string fileName = null;
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
            public void Exe()
            {
                if (G.equal(opt_prot, "yes"))
                {
                    G.Writeln2("*** ERROR: OPEN<prot> is obsolete. In Gekko 2.1.1 and onwards, databanks");
                    G.Writeln("           are always opened as 'protected' by default, unless you use", Color.Red);
                    G.Writeln("           OPEN<edit>, or unless you afterwards use the UNLOCK command", Color.Red);
                    G.Writeln("           to make the databank editable.", Color.Red);                    
                    throw new GekkoException();
                }

                if (G.equal(opt_prim, "yes"))
                {
                    G.Writeln2("*** ERROR: OPEN<prim> is obsolete. In Gekko 2.1.1 and onwards, you should");
                    G.Writeln("           use OPEN<edit> instead of OPEN<prim>, if you intend to change", Color.Red);
                    G.Writeln("           data in the databank.", Color.Red);                    
                    throw new GekkoException();
                }

                ReadOpenMulbkHelper hlp = new ReadOpenMulbkHelper();  //This is a bit confusing, using an old object to store the stuff.
                hlp.openFileNames = this.openFileNames;                
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
                if (G.equal(opt_first, "yes")) posCounter++;
                if (G.equal(opt_ref, "yes")) posCounter++;
                if (G.equal(opt_last, "yes")) posCounter++;
                if (!G.isNumericalError(this.opt_pos)) posCounter++;
                
                if (posCounter > 1)
                {
                    G.Writeln2("*** ERROR: You are using > 1 of first/last/pos/ref designations inside <>-field");
                    throw new GekkoException();
                }
                if (G.equal(opt_edit, "yes") && posCounter > 0)
                {
                    G.Writeln2("*** ERROR: You cannot mix 'edit' with first/last/pos/ref designations inside <>-field");
                    throw new GekkoException();
                }
                if (G.equal(opt_first, "yes"))
                {
                    hlp.openType = EOpenType.First;
                }
                if (G.equal(opt_last, "yes"))
                {
                    hlp.openType = EOpenType.Last;
                }
                if (G.equal(opt_edit, "yes"))
                {
                    hlp.openType = EOpenType.Edit;
                    hlp.protect = false;  //will override the born true value of the field
                }
                if (G.equal(opt_ref, "yes"))
                {
                    hlp.openType = EOpenType.Ref;
                }
                if (G.equal(opt_sec, "yes"))
                {
                    hlp.openType = EOpenType.Sec;
                }
                if (!G.isNumericalError(this.opt_pos))
                {
                    hlp.openType = EOpenType.Pos;                    
                    if (G.Round(out hlp.openTypePosition, opt_pos) == false)
                    {
                        G.Writeln2("*** ERROR: OPEN<pos=...> should be integer value");
                        throw new GekkoException();
                    }
                }                                
                
                List<Program.ReadInfo> readInfos = new List<Program.ReadInfo>();
                Program.OpenOrRead(false, hlp, true, readInfos);                

                foreach (Program.ReadInfo readInfo in readInfos)
                {                    
                    readInfo.open = true;
                    if (readInfo.abortedStar) return;  //an aborted OPEN *

                    if (G.equal(opt_save, "no"))
                    {
                        readInfo.databank.save = false;
                    }                    

                    readInfo.Print();
                }

                if (G.equal(Program.options.interface_mode, "sim"))
                {
                    G.Writeln2("+++ WARNING: READ ... TO ... is recommended instead of OPEN in sim-mode (cf. MODE).");
                    G.Writeln("             For instance, 'READ databk TO *;' instead of 'OPEN databk;'", Globals.warningColor);
                }
            }            
        }

        public class Run
        {
            public string fileName = null;
            public P p = null;
            public void Exe()
            {
                Program.Run(this.fileName, this.p);
            }
        }

        public class Library
        {
            public string fileName = null;
            public P p = null;
            public void Exe()
            {
                Program.Library(this.fileName, this.p);
            }
        }

        public class Doc
        {
            public string opt_label = null;
            public string opt_source = null;
            public string opt_stamp = null;            
            public List<string> listItems0 = null;  //left side     
            public void Exe()
            {
                foreach (string s in listItems0)
                {
                    ExtractBankAndRestHelper h = Program.ExtractBankAndRest(s, EExtrackBankAndRest.GetDatabankAndTimeSeries);
                    if (opt_label != null) h.ts.label = opt_label;
                    if (opt_source != null) h.ts.source = opt_source;
                    if (opt_stamp != null) h.ts.stamp = opt_stamp;
                    h.ts.SetDirty(true);
                }
            }         
        }

        public class Accept
        {
            public string type = null;
            public IVariable name = null;
            public IVariable message = null;
            public void Exe()
            {
                string value = "";
                string msg = O.GetString(message);
                if (Program.InputBox("Accept", msg, ref value) == DialogResult.OK)
                {                    
                    string nme = O.GetString(name);
                    if (G.equal(type, "val"))
                    {                          
                        try
                        {
                            double v = double.Parse(value.Trim());
                            if (Program.scalars.ContainsKey(nme)) Program.scalars.Remove(nme);
                            Program.scalars.Add(nme, new ScalarVal(v));
                            G.Writeln2("VAL " + nme + " = " + v);
                        }
                        catch
                        {
                            G.Writeln2("*** ERROR: Could not convert '" + value + "' into a VAL");
                            throw new GekkoException();
                        }                        
                    }
                    else if (G.equal(type, "string"))
                    {                                               
                        
                        try
                        {
                            string v = value.Trim();
                            v = Program.StripQuotes(v);
                            if (Program.scalars.ContainsKey(nme)) Program.scalars.Remove(nme);
                            Program.scalars.Add(nme, new ScalarString(v, false));
                            G.Writeln2("STRING " + nme + " = '" + v + "'");
                        }
                        catch
                        {
                            G.Writeln2("*** ERROR: Could not convert '" + value + "' into a STRING");
                            throw new GekkoException();
                        }
                    }
                    else if (G.equal(type, "name"))
                    {
                        try
                        {
                            string v = value.Trim();
                            v = Program.StripQuotes(v);
                            if (Program.scalars.ContainsKey(nme)) Program.scalars.Remove(nme);
                            Program.scalars.Add(nme, new ScalarString(v, true));
                            G.Writeln2("NAME " + nme + " = '" + v + "'");
                        }
                        catch
                        {
                            G.Writeln2("*** ERROR: Could not convert '" + value + "' into a NAME");
                            throw new GekkoException();
                        }
                    }
                    else if (G.equal(type, "date"))
                    {
                        try
                        {
                            GekkoTime gt = G.FromStringToDate(value.Trim());                            
                            if (Program.scalars.ContainsKey(nme)) Program.scalars.Remove(nme);
                            Program.scalars.Add(nme, new ScalarDate(gt));
                            G.Writeln2("DATE " + nme + " = " + gt.ToString());
                        }
                        catch
                        {
                            G.Writeln2("*** ERROR: Could not convert '" + value + "' into a DATE");
                            throw new GekkoException();
                        }
                    }
                    else if (G.equal(type, "list"))
                    {
                        try
                        {
                            string v = value.Trim();
                            string[] vv = v.Split(',');
                            List<string> xx = new List<string>();
                            foreach (string s in vv)
                            {
                                string ss = s.Trim();
                                ss = Program.StripQuotes(ss);
                                if (!G.IsSimpleToken(ss))
                                {
                                    G.Writeln2("*** ERROR: Element '" + ss + "' is not a simple name");
                                    throw new GekkoException();
                                }
                                xx.Add(ss);
                            }

                            if (Program.scalars.ContainsKey(Globals.symbolList + nme)) Program.scalars.Remove(Globals.symbolList + nme);
                            Program.scalars.Add(Globals.symbolList + nme, new MetaList(xx));
                            G.Writeln2("LIST " + nme + " = " + G.GetListWithCommas(xx));
                        }
                        catch
                        {
                            G.Writeln2("*** ERROR: Could not convert '" + value + "' into a LIST");
                            throw new GekkoException();
                        }
                    }
                }
            }
        }

        public class Analyze
        {
                        
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set      
            public double lag = double.NaN;
            public List<O.Prt.Element> prtElements = new List<O.Prt.Element>();
            public void Exe()
            {
                Program.Analyze(this);                
            }
        }

        public class Copy
        {
            public string opt_respect = null;
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set            
            //public List<string> listItems = null;  //only temporary storage, use namesList1+2
            public List<string> listItems0 = null;  //left side
            public List<string> listItems1 = null;  //right side
            public string opt_from = null;
            public string opt_to = null;
            public string opt_error = null;
            public void Exe()
            {                
                //------------ Types -----------------------------------------------------------------------------------------------
                //1. if RESPECT keyword and timeseries exists beforehand            -> inject data inside [t1, t2] in existing series
                //2. if RESPECT keyword and timeseries does not exist beforehand    -> clone, truncate to [t1, t2], and add to databank
                //3. if no RESPECT keyword and timeseries exists beforehand         -> delete existing timeseries, clone, and add the new one to databank
                //4. if no RESPECT keyword and timeseries does not exist beforehand -> clone and add to databank                
                //------------------------------------------------------------------------------------------------------------------

                //COPY ...;           short for COPY ... TO work:*;
                //COPY ... TO x:*;    redirects to x bank instead of work
                //COPY ... TO y*z;    copies with prefix/suffix
                //COPY ... TO x:y*z;  redirects to x bank with prefix/suffix
                //--> note you cannot use these in combinations with strings!
                //--> and COPY *.fy would not make sense (would give name collisions. So
                //    no wildcards for banks, not even *. (But PRT *:fy would be ok, and later on *.sol)

                //These <FROM = ...> and <TO = ...> can be overridden by individual bank names like "COPY <FROM = adbk> a, b, simbk:c, d;". In that case, 'c' is taken from simbk.
                //Note that <FROM/TO = @> is legal!

                Databank localOptionFromBank = null; //is != null if <FROM = ...>
                Databank localOptionToBank = null; //is != null if <TO  = ...>
                GetFromAndToDatabanks(this.opt_from, this.opt_to, ref localOptionFromBank, ref localOptionToBank);

                int errorCounter = 0;

                bool wild0 = false;
                foreach (string s in listItems0)
                {
                    if (s.Contains("*") || s.Contains("?") || s.Contains("..")) wild0 = true;
                }

                bool isUsingPlaceholder = false;                
                string placeholderPrefix = null;
                string placeholderSuffix = null;
                if (listItems1 != null)
                {
                    foreach (string s in listItems1)
                    {
                        ExtractBankAndRestHelper h = Program.ExtractBankAndRest(s, EExtrackBankAndRest.OnlyStrings);                        
                        string name = h.name;

                        if (name.Contains(".."))
                        {
                            G.Writeln2("*** ERROR: In COPY, you can only use '..' on the left side of TO (the right side is not a wildcard)");
                            throw new GekkoException();
                        }

                        if (name.Contains("?"))
                        {
                            G.Writeln2("*** ERROR: In COPY, you can only use '?' on the left side of TO (the right side is not a wildcard)");
                            throw new GekkoException();
                        }

                        if (name.Contains("*"))
                        {
                            if (listItems1.Count > 1)
                            {
                                G.Writeln2("*** ERROR: In COPY, if you use '*' on the right side of TO, you can only use one list element (there are " + listItems1.Count + ")");
                                throw new GekkoException();
                            }

                            if (G.CountCharsInString(name, "*") > 1)
                            {
                                G.Writeln2("*** ERROR: In COPY, you cannot use more than one '*' in a list item on the right side of TO");
                                throw new GekkoException();
                            }
                            string[] split = name.Split('*');
                            isUsingPlaceholder = true;
                            placeholderPrefix = split[0];
                            placeholderSuffix = split[1];
                        }
                    }

                    if (wild0 && !isUsingPlaceholder)
                    {
                        G.Writeln2("*** ERROR: When using '*', '?' or '..' on the left side of TO, the right side of TO must contain a '*'");
                        throw new GekkoException();
                    }

                    if (!wild0 && !isUsingPlaceholder)
                    {
                        if (listItems0.Count != listItems1.Count)
                        {
                            G.Writeln2("*** ERROR: Difference between number of list items to the left and right of TO (" + listItems0.Count + " versus " + listItems1.Count + ")");
                            throw new GekkoException();
                        }
                    }
                }

                //When we get here, if there are wildcards or ranges on the left of TO, the right of TO is either empty or a single list item containing one and only one '*'
                //If there are no wildcards on left and right of TO, the lists have same size.
                                
                int type1 = 0;
                int type2 = 0;
                int type3 = 0;
                int type4 = 0;                

                for (int i = 0; i < listItems0.Count; i++)  //regarding this list, #-lists like "COPY #m;" are unfolded, but any wildcards or ranges are not.
                {
                    //Databank fromBank = null;
                    Databank toBank = SetPossibleToBank(localOptionToBank);

                    if (listItems1 == null)
                    {
                        //Stuff like "COPY adbk:fx*;"  (copies to first)
                        //Hmmm: gets run for each item to the left of TO, but the following line runs fast anyway.
                        if (toBank == null) toBank = Program.databanks.GetFirst();
                    }
                    else
                    {
                        string ss = null;
                        if (isUsingPlaceholder)
                        {
                            //Stuff like "COPY bank1:#m TO bank2:a*;"
                            //here we use item #0 (there can only be 1 such item)
                            ss = listItems1[0];
                        }
                        else
                        {
                            //Stuff like "COPY bank1:#m TO bank2:#m;"
                            ss = listItems1[i];
                        }                        
                        ExtractBankAndRestHelper h = Program.ExtractBankAndRest(ss, EExtrackBankAndRest.GetDatabank);

                        string bankName2 = null;
                        if (localOptionToBank != null) bankName2 = localOptionToBank.aliasName;
                        string toBankName = Program.PerhapsOverrideWithDefaultBankName(bankName2, h.hasColon, h.bank);
                        toBank = Program.databanks.GetDatabank(toBankName);
                        if (toBank == null)
                        {
                            G.Writeln2("Databank " + toBankName + " is not open");
                            throw new GekkoException();
                        }
                    }

                    //listItems0[i] may be with or without wildcards ('*' or '?') or ranges ('..'), and with or without bank (bank:varname)
                    //but lists are unfolded to names here.
                    //We use the .aliasName here. Actually could use h.bank from above

                    string bankName = null;
                    if (localOptionFromBank != null) bankName = localOptionFromBank.aliasName;

                    List<TimeSeries> tss = Program.GetTimeSeriesFromStringWildcard(listItems0[i], bankName);  //gets these from the 'fromBank', so ExtractBankAndRest() gets called two times, but never mind

                    if (tss.Count == 0)
                    {
                        string s = listItems0[i].Replace(Globals.firstCheatString, "");

                        if (s.Contains("*") || s.Contains("?") || s.Contains(".."))
                        {                            
                            G.Writeln2("+++ ERROR: COPY: The following item was not found: " + s);
                            throw new GekkoException();                            
                        }
                        else
                        {
                            if (G.equal(opt_error, "no"))
                            {
                                errorCounter++;                                
                                G.Writeln("Note: the following item was not found: " + s);
                                continue;
                            }
                            else
                            {
                                G.Writeln2("*** ERROR: COPY: Could not find this timeseries: '" + s + "'");
                                throw new GekkoException();
                            }
                        }                        
                    }

                    foreach (TimeSeries ts in tss)
                    {
                        //If a wildcard like "COPY a*;" is used, tss may have > 1 elements
                        string newName = null;
                        if (listItems1 == null)
                        {                            
                            //Stuff like "COPY adbk:fx*;"  (copies to first)                            
                            newName = ts.variableName;  //same name is used
                        }
                        else
                        {                            
                            if (isUsingPlaceholder)
                            {
                                //Stuff like "COPY #m TO simbk:a_*;"
                                //here we use item #0 (there can only be 1 such item)
                                ExtractBankAndRestHelper h = Program.ExtractBankAndRest(listItems1[0], EExtrackBankAndRest.GetDatabank);                            
                                newName = placeholderPrefix + ts.variableName + placeholderSuffix;
                            }
                            else
                            {
                                //Stuff like "COPY bank1:#m1 TO bank2:#m2;"
                                //new names are provided with TO keyword (in a list with same number of items)                                
                                ExtractBankAndRestHelper h = Program.ExtractBankAndRest(listItems1[i], EExtrackBankAndRest.GetDatabank);                                
                                newName = h.name;
                            }
                        }

                        // ----------------------------------------------------------------------------
                        // Now follows the actual copying
                        // ----------------------------------------------------------------------------

                        if (true)
                        {
                            //TODO: do the actual copying as a block AFTER this is checked! (do some recording of what is to be copied, deleted, etc., and execute that at the end)
                            //Testing that we are not copying timeseries to themselves
                            TimeSeries ts2 = toBank.GetVariable(newName);
                            if (ts2 != null)
                            {
                                if (G.equal(ts.parentDatabank.aliasName, ts2.parentDatabank.aliasName))
                                {
                                    if (G.equal(ts.variableName, ts2.variableName))
                                    {
                                        G.Writeln2("*** ERROR: You are trying to copy the timeseries '" + ts.variableName + "' from databank '" + ts.parentDatabank.aliasName + "' to itself");
                                        throw new GekkoException();
                                    }
                                }
                            }
                        }
                        
                        if (G.equal(this.opt_respect, "yes"))
                        {                                                       
                            //Truncate time period
                            TimeSeries ts2 = toBank.GetVariable(newName);
                            if (ts2 != null)
                            {
                                //Type 1
                                //inject into existing
                                //this is the fastest way, in reality via arraycopy. But beware to check carefully if you
                                //  change anything here.
                                type1++;
                                int index1, index2;
                                double[] values = ts.GetDataSequence(out index1, out index2, this.t1, this.t2);
                                ts2.SetDataSequence(t1, t2, values, index1);
                                ts2.Stamp();  //will get a new date stamp, since it is not cloning here
                            }
                            else
                            {
                                //Type 2
                                type2++;
                                ts2 = ts.Clone();
                                ts2.variableName = newName;
                                ts2.Truncate(this.t1, this.t2);
                                toBank.AddVariable(ts2);
                            }
                        }
                        else
                        {
                            //No truncate of time period
                            TimeSeries ts2 = ts.Clone();  //will inherit the date stamp                          
                            if (listItems1 != null)
                            {                                
                                ts2.variableName = newName;
                            }
                            if (toBank.ContainsVariable(newName))
                            {
                                //Type 3
                                //wipe out any existing TimeSeries completely, and put in the clone instead
                                type3++;
                                toBank.RemoveVariable(newName);
                            }
                            else
                            {
                                type4++;
                                //Type 4
                            }
                            toBank.AddVariable(ts2);
                        }
                    }
                }

                //1. if RESPECT keyword and timeseries exists beforehand            -> inject data inside [t1, t2] in existing series
                //2. if RESPECT keyword and timeseries does not exist beforehand    -> clone, truncate to [t1, t2], and add to databank
                //3. if no RESPECT keyword and timeseries exists beforehand         -> delete existing timeseries, clone, and add the new one to databank
                //4. if no RESPECT keyword and timeseries does not exist beforehand -> clone and add to databank                

                G.Writeln();
                if (type1 > 0)
                {
                    G.Writeln("Put data for " + t1 + "-" + t2 + " into " + type1 + " existing timeseries");
                }
                if (type2 > 0)
                {
                    G.Writeln("Put data for " + t1 + "-" + t2 + " into " + type2 + " new timeseries");
                }
                if (type3 > 0)
                {
                    G.Writeln("Replaced data for all periods in " + type3 + " existing timeseries");
                }
                if (type4 > 0)
                {
                    G.Writeln("Put data for all periods into " + type4 + " new timeseries");
                }
                if(errorCounter > 0)
                {
                    G.Writeln2("+++ WARNING: COPY: Note that " + errorCounter + " variables were not copied");
                }
            }

            private static Databank SetPossibleToBank2(Databank toBank, ExtractBankAndRestHelper h)
            {
                if (h.hasColon) toBank = h.databank;  //overrides                            
                return toBank;
            }

            private static Databank SetPossibleToBank(Databank localOptionToBank)
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
            public List<string> listItems = null;
            public string opt_abs = null;
            public string fileName = null;
            public void Exe()
            {
                Program.Compare(this);
            }
        }

        public class Truncate
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public List<string> listItems = null;
            public void Exe()
            {
                int counter = 0;
                foreach (string s in this.listItems)
                {
                    List<TimeSeries> tss = Program.GetTimeSeriesFromStringWildcard(s);
                    //Now we are sure all the series are from Work
                    foreach (TimeSeries ts in tss)
                    {
                        ts.Truncate(this.t1, this.t2);
                        counter++;
                        ts.Stamp();
                    }                    
                }
                G.Writeln2("Truncated " + counter + " timeseries to " + t1 + "-" + t2 + "");
            }
        }

        public class Rename
        {            
            //public List<string> listItems = null;  //only temporary storage, use namesList1+2
            public List<string> listItems0 = null;
            public List<string> listItems1 = null;
            public string opt_bank = null;
            public void Exe()
            {
                //listItems = null;  //just for safety, should not be used.
                
                if (true)
                {
                    if (listItems0.Count != listItems1.Count)
                    {
                        G.Writeln2("*** ERROR: unequal number of items before and after AS");
                        throw new GekkoException();
                    }
                    for (int i = 0; i < listItems0.Count; i++)
                    {
                        if (listItems0[i].Contains("*") || listItems0[i].Contains("?") || listItems1[i].Contains("*") || listItems1[i].Contains("?"))
                        {
                            G.Writeln2("*** ERROR: RENAME: Wildcards not allowed");
                            throw new GekkoException();
                        }
                    }
                }
                //we are now sure lists are of same length and have no wildcards.                                

                int counter = 0;
                for (int i = 0; i < listItems0.Count; i++)
                {
                    string s1 = listItems0[i];
                    string s2 = listItems1[i];
                    if (s1.Contains(":") || s2.Contains(":"))
                    {
                        G.Writeln2("*** ERROR: Banknames not yet allowed for RENAME command.");
                        G.Writeln("           You may try the <bank=...> option.");
                        throw new GekkoException();
                    }
                    //the string may be with or without bank (bank:varname)
                                     
                    List<TimeSeries> tss = Program.GetTimeSeriesFromStringWildcard(s1, opt_bank);
                    foreach (TimeSeries ts in tss)
                    {
                        //There is probably always only 1 here
                        if (ts.parentDatabank.ContainsVariable(s2))
                        {
                            G.Writeln2("*** ERROR: Databank " + ts.parentDatabank.aliasName + " already contains timeseries '" + s2 + "'");
                            throw new GekkoException();
                        }
                        ts.parentDatabank.RemoveVariable(ts.variableName);                        
                        ts.variableName = s2;
                        ts.parentDatabank.AddVariable(ts);
                        counter++;
                    }
                }                
                G.Writeln2("Renamed " + counter + " timeseries in Work databank");
            }
        }

        public class Create
        {
            public List<string> listItems = null;
            public bool question = false;
            public P p = null;
            public void Exe()
            {
                Program.Create(this.listItems, this.question, this);
            }
        }

        public class CreateExpression
        {
            public IVariable lhs = null;
            public IVariable rhs = null;
            public void Exe()
            {
                TimeSeries tlhs = O.GetTimeSeries(lhs);

                Databank bankName = tlhs.parentDatabank;
                string varName = tlhs.variableName;

                TimeSeries trhs = O.GetTimeSeries(rhs);
                trhs.variableName = varName;

                bankName.RemoveVariable(varName);
                bankName.AddVariable(trhs);

                trhs.Stamp(); //for instance chain index function results in new date stamp                

            }
        }

        public class Delete
        {
            public List<string> listItems = null;
            public string opt_nonmodel = null;
            public void Exe()
            {
                if (G.equal(opt_nonmodel, "yes"))
                {
                    if (this.listItems != null)
                    {
                        G.Writeln2("*** ERROR: You cannot mix <nonmodel> and list items");
                        throw new GekkoException();
                    }
                    Program.Trimvars();
                }
                else
                {
                    Program.Delete(this.listItems);
                }
            }
        }

        public class Disp
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public List<string> listItems = null;            
            public string searchName = null;
            public string opt_info = null;
            public void Exe()
            {
                if (this.searchName == null)
                {
                    Program.Disp(this.t1, this.t2, this.listItems, this);
                }
                else
                {
                    Program.DispSearch(this.searchName);
                }
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

        public class Decomp
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public string variable = null;
            public string expressionCs = null;
            public void Exe()
            {
                Program.Decomp(null, this.t1, this.t2, null, null, null, variable, expressionCs);
            }
        } 
        
        public class Info
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public List<string> listItems = null;
            public void Exe()
            {
                Program.Info(this.t1, this.t2, this.listItems);
            }
        }

        public class Itershow
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public List<string> listItems = null;
            public void Exe()
            {                
                Program.Itershow(this.listItems, this.t1, this.t2);
            }
        }

        public class Efter
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set            
            public void Exe()
            {                
                Program.Efter(this.t1, this.t2);
            }
        }

        public class Checkoff
        {
            public List<string> listItems = null;
            public string type = null;
            public void Exe()
            {                
                Program.Checkoff(this.listItems, type);
            }
        }

        public class Endo
        {
            public List<string> listItems = null;
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
            public List<string> listItems = null;
            public bool question = false;
            public double opt_replace = double.NaN;
            public void Exe()
            {
                Program.FindMissingData(this);
            }
        }

        public class ForString
        {
            public List<string> listItems = null;
        }
             

        public class List
        {
            public string name = null;
            public string listFile = null;
            public List<string> listItems = null;
            public string listPrefix = null;
            public string listSuffix = null;
            public string listStrip = null;
            public string listSort = null;
            public string listTrim = null;
            public bool direct = false;
            public string rawfood = null;
            public P p = null;
            public void Exe()
            {
                if (this.direct)
                {
                    //Here, we get the elements from the raw text ('rawfood'), and handle them a bit like
                    //listfiles, #(listfile ...).
                    //If needed, LIST<direct> could be made more general, allowing all sorts of characters.
                    List<string> input = new List<string>();
                    string[] ss = rawfood.Split(',');
                    input.AddRange(ss);
                    List<string> result = new List<string>();
                    GetRawListElements(null, input, result);
                    this.listItems = result;
                }

                if (listPrefix != null)
                {
                    for (int i = 0; i < this.listItems.Count; i++)
                    {
                        this.listItems[i] = listPrefix + this.listItems[i];
                    }
                }
                if (listSuffix != null)
                {
                    for (int i = 0; i < this.listItems.Count; i++)
                    {
                        this.listItems[i] = this.listItems[i] + listSuffix;
                    }
                }
                if (listStrip != null)
                {
                    //Maybe a STRIPPREFIX, STRIPSUFFIX could be practical, using StartsWith()/EndsWith()
                    for (int i = 0; i < this.listItems.Count; i++)
                    {
                        //Case-insensitive replacing
                        this.listItems[i] = G.ReplaceString(this.listItems[i], listStrip, "", false);
                    }
                }                

                if (G.equal(listSort, "yes"))  //listSort = null is okay in G.equal()
                {
                    //TODO: What about strings starting with "-"?
                    //TODO: What about æøå strings, what happens??
                    this.listItems.Sort(StringComparer.OrdinalIgnoreCase);
                }

                if (G.equal(listTrim, "yes"))  //listTrim = null is okay in G.equal()
                {
                    //Todo: if it has just been sorted, trimming is easy. But we do it the general way here.
                    GekkoDictionary<string, bool> xx = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
                    List<string> newList = new List<string>();
                    foreach(string s in this.listItems)
                    {
                        if(xx.ContainsKey(s))
                        {
                            //ignore
                        }
                        else
                        {
                            newList.Add(s);
                            xx.Add(s, true);
                        }
                    }
                    this.listItems = newList;
                }

                Program.PutListIntoListOrListfile(this.listItems, this.name, this.listFile);

                if (this.p.IsSimple())
                {
                    G.Write2("Created 1 list from " + this.listItems.Count + " elements "); G.ServiceMessage();
                }
            }

            

            public static void Q(string s)
            {
                Program.WriteListItems(s);
            }
        }
            
        public class Genr
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public string lhsFunction = null;
            public TimeSeries lhs = null;
            public string meta = null;
            public P p = null;
            public void Exe()
            {
                if (this.meta != null)
                {                    
                    //For instance, "SERIES y = 2 * x;" --> meta = "SERIES y = 2 * x" (without the semicolon)    
                    string s = ShowDatesAsString(this.t1, this.t2);
                    lhs.source = s + this.meta;                    
                    lhs.SetDirtyGhost(true, false);
                }
                lhs.Stamp();
                if (this.p.IsSimple())
                {
                    G.Write2("1 series updated " + t1.ToString() + "-" + t2.ToString() + " "); G.ServiceMessage();
                }
            }            
        }


        public class Series
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public string lhsFunction = null;
            public TimeSeries lhs = null;
            public string meta = null;
            public P p = null;
            public void Exe()
            {
                if (this.meta != null)
                {
                    //For instance, "SERIES y = 2 * x;" --> meta = "SERIES y = 2 * x" (without the semicolon)    
                    string s = ShowDatesAsString(this.t1, this.t2);
                    lhs.source = s + this.meta;                    
                    lhs.SetDirtyGhost(true, false);
                }
                lhs.Stamp();
                if (this.p.IsSimple())
                {
                    G.Write2("1 series updated " + t1.ToString() + "-" + t2.ToString() + " "); G.ServiceMessage();
                }
            }
        }

        public class Index
        {
            public string name = null;
            public string opt_mute = null;
            public string opt_addbank = null;
            public string opt_bank = null;
            //public string listFile = null; //make it work
            public string wildCard1 = null; //--> delete??
            public string wildCard2 = null;  //only active if range   //--> delete??
            public List<string> listItems = null;
            public string listFile = null;
            public void Exe()
            {
                bool addbank = false; if (G.equal(this.opt_addbank, "yes")) addbank = true;
                List<string> names = new List<string>();

                if (G.equal(this.opt_bank, "yes"))
                {
                    //For safety, remove in Gekko 2.4 or 2.6
                    G.Writeln2("+++ ERROR: In Gekko 2.2, INDEX<bank=yes> is INDEX<addbank=yes>.");
                    throw new GekkoException();
                }                

                foreach (string s in this.listItems)
                {
                    List<BankNameVersion> xx = Program.GetInfoFromStringWildcard(s, this.opt_bank);
                    foreach (BankNameVersion bnv in xx)
                    {                        
                        if (addbank)
                        {
                            names.Add(bnv.bank + Globals.symbolBankColon + bnv.name);
                        }
                        else
                        {
                            names.Add(bnv.name);  //probably would never be null. Culd have option to keep banknames!!!!!!
                        }
                    }
                }

                if (!G.equal(this.opt_mute, "yes"))
                {
                    G.Writeln();
                    if (names.Count > 0)
                    {
                        G.Writeln(G.GetListWithCommas(names));
                    }
                }

                if (name != null)
                {
                    Program.PutListIntoListOrListfile(names, this.name, this.listFile);
                    G.Writeln2("Put " + names.Count + " matching items into list #" + name);
                }
                else if (listFile != null)
                {
                    Program.PutListIntoListOrListfile(names, this.name, this.listFile);
                    G.Writeln2("Put " + names.Count + " matching items into external file " + Program.AddExtension(listFile, "." + "lst"));
                }
                else
                {
                    G.Writeln2("Found " + names.Count + " matching items");
                }              
                
            }
        }

        public class Rebase
        {
            public List<string> listItems = null;
            public GekkoTime date1 = Globals.tNull;
            public GekkoTime date2 = Globals.tNull;
            public string opt_prefix = null;
            public string opt_bank = null;
            public double opt_index = 100d;
            public void Exe()
            {
                if (date1.IsNull())
                {
                    G.Writeln2("*** ERROR: The index date does not seem to exist");  //probably cannot happen
                    throw new GekkoException();
                }
                if (date2.IsNull())
                {
                    date2 = date1;
                }
                if (date1.freq != date2.freq)
                {
                    G.Writeln2("*** ERROR: The two index dates have different frequencies");
                    throw new GekkoException();
                }
                if (date1.StrictlyLargerThan(date2))
                {
                    G.Writeln2("*** ERROR: The first date must not be later than the last date");  //probably cannot happen
                    throw new GekkoException();
                }
                int counter = 0;
                int count = 0;
                for (int i = 0; i < this.listItems.Count; i++)
                {
                    List<TimeSeries> tss = Program.GetTimeSeriesFromStringWildcard(this.listItems[i], opt_bank);
                    foreach (TimeSeries ts in tss)
                    {
                        if (ts.parentDatabank.protect) Program.ProtectError("You cannot change/add a timeseries in a non-editable databank (" + ts.parentDatabank + ")");
                        
                        GekkoTime ddate1 = date1;
                        GekkoTime ddate2 = date2;

                        if (date1.freq == EFreq.Annual && (ts.freqEnum == EFreq.Quarterly || ts.freqEnum == EFreq.Monthly))
                        {
                            //if a year is used for a quarterly series, q1-q4 is used.
                            ddate1 = new GekkoTime(ts.freqEnum, date1.super, 1);
                            int end = -12345;
                            if (ts.freqEnum == EFreq.Quarterly)
                            {
                                end = Globals.freqQSubperiods;
                            }
                            else if (ts.freqEnum == EFreq.Monthly)
                            {
                                end = Globals.freqMSubperiods;
                            }
                            else
                            {
                                G.Writeln2("*** ERROR: freq error #903853245");
                                throw new GekkoException();
                            }
                            ddate2 = new GekkoTime(ts.freqEnum, date1.super, end);
                        }

                        if (ddate1.freq != ts.freqEnum || ddate2.freq != ts.freqEnum)
                        {
                            G.Writeln2("*** ERROR: frequency of timeseries and frequency of period(s) do not match");
                            throw new GekkoException();
                        }

                        double sum = 0d;
                        double n = 0d;
                        foreach (GekkoTime t in new GekkoTimeIterator(ddate1, ddate2))
                        {
                            sum += ts.GetData(t);
                            n++;
                        }

                        if (G.isNumericalError(sum))
                        {
                            G.Writeln2("*** ERROR: Series " + ts.parentDatabank + ":" + ts.variableName + " from " + ddate1.ToString() + "-" + ddate2.ToString() + " contains missing values");
                            throw new GekkoException();
                        }
                        if (sum == 0d)
                        {
                            G.Writeln2("*** ERROR: Series " + ts.parentDatabank + ":" + ts.variableName + " from " + ddate1.ToString() + "-" + ddate2.ToString() + " sums to 0, cannot rebase");
                            throw new GekkoException();
                        }                                               

                        TimeSeries tsNew = null;
                        if (opt_prefix != null)
                        {
                            tsNew = ts.Clone();
                            tsNew.variableName = opt_prefix + ts.variableName;
                            if (ts.parentDatabank == null)
                            {
                                G.Writeln2("*** ERROR: Internal error #8796357826435");
                                throw new GekkoException();
                            }


                            if (ts.parentDatabank.ContainsVariable(tsNew.variableName))
                            {
                                ts.parentDatabank.RemoveVariable(tsNew.variableName);
                                counter++;
                            }
                            ts.parentDatabank.AddVariable(tsNew);
                        }
                        else tsNew = ts;
                        
                        double[] data = tsNew.dataArray;
                        for (int ii = 0; ii < data.Length; ii++)
                        {
                            //could use ts.firstPeriodPositionInArray etc., but better to do it for all since ts.ts.firstPeriodPositionInArray is not always correct
                            data[ii] = data[ii] / (sum / n) * opt_index;
                        }
                        count++;
                    }
                }
                G.Writeln2("Rebased " + count + " variables");
                //if (counter > 0) G.Writeln("+++ NOTE: Prefix names replaced " + counter + " existing variables");
            }
        }

        public class Count
        {            
            public string wildCard1 = null;
            public string wildCard2 = null;  //only active if range
            public List<string> listItems = null;
            public void Exe()
            {
                G.Writeln2("Found " + this.listItems.Count + " matching items");
            }
        }

        public class Upd
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            //public TimeSeries lhs = null;
            public List<string> listItems = null;
            public double[] data = null;
            public double[] rep = null;
            public string op = null;
            public bool opDollar = false;
            public string opt_d = null;
            public string opt_p = null;
            public string opt_m = null;
            public string opt_q = null;
            public string opt_mp = null;
            public string opt_n = null;
            public string opt_keep = null;
            public string meta = null;
            public P p = null;
            public void Exe()
            {
                if (this.op.EndsWith(Globals.symbolDollar))
                {
                    this.opDollar = true;
                    this.op = this.op.Substring(0, this.op.Length - 1);
                }                
                Program.Upd(this);
                if (this.p.IsSimple())
                {
                    G.Write2(listItems.Count + " series updated " + t1.ToString() + "-" + t2.ToString() + " "); G.ServiceMessage();
                }
            }
        }

        public class Time
        {
            public GekkoTime t1 = Globals.tNull;
            public GekkoTime t2 = Globals.tNull;
            public void Exe()
            {
                Program.Time(t1, t2);

                if (false)
                {
                    //to be used later on for list(...) function
                    GekkoList<string> x = new GekkoList<string>();
                    x.Add("a").Add("b");
                    GekkoList<string> y = new GekkoList<string>();
                    y.Add("c").Add("d");
                    GekkoList<string> z = new GekkoList<string>();
                    z.AddRange(x).AddRange(y);
                    Console.WriteLine("d");
                    GekkoList<string> zz = GekkoList<string>.Construct().Add("a").Add("b").AddRange(x);
                }


            }
            public static void Q()
            {
                G.Writeln2("Global time is: " + G.FromDateToString(Globals.globalPeriodStart) + " to " + G.FromDateToString(Globals.globalPeriodEnd));
            }
        }

        public class TimeFilterHelper
        {
            public GekkoTime from = Globals.tNull;
            public GekkoTime to = Globals.tNull;
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
                
                public List<OptString> printCodes = new List<OptString>();   

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
                    Globals.tableOption = "n";                    
                    
                    foreach (OptString os in this.printCodes)
                    {
                        if (G.equal(os.s2, "yes"))
                        {
                            Globals.tableOption = os.s1;
                            break;
                        }
                    } 

                    string tt1 = "__tabletimestart";
                    string tt2 = "__tabletimeend";

                    if (Program.scalars.ContainsKey(tt1)) Program.scalars.Remove(tt1);
                    if (Program.scalars.ContainsKey(tt2)) Program.scalars.Remove(tt2);
                    Program.scalars.Add(tt1, new ScalarDate(this.t1));
                    Program.scalars.Add(tt2, new ScalarDate(this.t2));

                    string tableFileName = Program.TableHelper(this.fileName, this.menuTable);

                    Globals.lastCalledMenuTable = tableFileName;
                    Program.XmlTable(tableFileName, this.opt_html, this.opt_window, p);
                    Program.Run(Globals.localTempFilesLocation + "\\" + "tablecode." + Globals.defaultCommandFileExtension, p);
                }
            }
            
            public class SetValues
            {
                public string name = null;
                public int col;
                public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
                public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set                
                public string printcode = null;
                public double scale = 1d;
                public string format = null;
                public List<Prt.Element> prtElements = new List<Prt.Element>();
                public void Exe() {
                    Program.GetTable(this.name).CurRow.SetValues(this.col, this.prtElements[0].subElements[0].tsWork, this.prtElements[0].subElements[0].tsBase, null, this.t1, this.t2, Globals.tableOption, this.printcode, this.scale, this.format);
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
            public List<OptString> printCodes = new List<OptString>();            
            public string timefilter = null;
            //public string heading = null;
                      
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
            public GekkoTime opt_xline = Globals.tNull;
            public GekkoTime opt_xlinebefore = Globals.tNull;
            public GekkoTime opt_xlineafter = Globals.tNull;
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
            
            
            public long counter = -12345;

            public void Exe()
            {                                
                Program.PrtNew(this);
                if (G.equal(prtType, "mulprt") && G.equal(Program.options.interface_mode, "data"))
                {
                    G.Writeln2("+++ WARNING: MULPRT is not intended for data mode, please use PRT (cf. the MODE command).");
                }
            }
            
            public static List<int> GetBankNumbers(string tableOrGraphGlobalPrintCode, List<string> printCodes)
            {               
                
                List<int> rv = new List<int>();

                //TODO: CLEAN THIS UP!!                                                
                if (G.equal(tableOrGraphGlobalPrintCode, "n"))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.equal(tableOrGraphGlobalPrintCode, "d"))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.equal(tableOrGraphGlobalPrintCode, "p"))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.equal(tableOrGraphGlobalPrintCode, "dp"))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.equal(tableOrGraphGlobalPrintCode, Globals.printCode_s))
                {
                    rv = new List<int>(); rv.Add(2);
                }
                else if (G.equal(tableOrGraphGlobalPrintCode, Globals.printCode_sn))
                {
                    rv = new List<int>(); rv.Add(2);
                }
                else if (G.equal(tableOrGraphGlobalPrintCode, Globals.printCode_sd))
                {
                    rv = new List<int>(); rv.Add(2);
                }
                else if (G.equal(tableOrGraphGlobalPrintCode, Globals.printCode_sp))
                {
                    rv = new List<int>(); rv.Add(2);
                }
                else if (G.equal(tableOrGraphGlobalPrintCode, Globals.printCode_sdp))
                {
                    rv = new List<int>(); rv.Add(2);
                }
                else if (G.equal(tableOrGraphGlobalPrintCode, "m"))
                {
                    rv = new List<int>(); rv.Add(1); rv.Add(2);
                }
                else if (G.equal(tableOrGraphGlobalPrintCode, "q"))
                {
                    rv = new List<int>(); rv.Add(1); rv.Add(2);
                }
                else if (G.equal(tableOrGraphGlobalPrintCode, "mp"))
                {
                    rv = new List<int>(); rv.Add(1); rv.Add(2);
                }                
                else
                {
                    bool usesBase = false;
                    bool usesWork = false;
                    foreach (string printCode in printCodes)  //could use break in loop, but this is not speed critical
                    {
                        if (Program.IsPrintCodeShortMultiplier(printCode))
                        {
                            usesWork = true;
                            usesBase = true;
                        }
                        if (Program.IsPrintCodeShortBase(printCode)) usesBase = true;
                        if (Program.IsPrintCodeShortWork(printCode)) usesWork = true;
                    }
                    
                    if (usesWork)
                    {
                        rv.Add(1);
                    }
                    if (usesBase)
                    {
                        rv.Add(2);                    
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
                public List<SubElement> subElements = null;
            
                public string label = null;
                //public string originalLabel = null;
                public string endoExoIndicator = null;
                //-- layout
                public List<OptString> printCodes = new List<OptString>();
                public int width = -12345;
                public int dec = -12345;
                public int nwidth = -12345;
                public int ndec = -12345;
                public int pwidth = -12345;
                public int pdec = -12345;
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
            }

            public class SubElement
            {
                //Items that are unfolded via lists
                public TimeSeries tsWork = null;
                public TimeSeries tsBase = null;
                public string label;
            }
        }

        public class Ols
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set    
            public string name = null;        
            public List<O.Prt.Element> prtElements = new List<O.Prt.Element>();
            public IVariable impose = null;
            public string opt_constant = null;                
            public void Exe()
            {
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
            public void Exe()
            {                
                Program.Sim(this);
                if (G.equal(Program.options.interface_mode, "data"))
                {
                    G.Writeln2("+++ WARNING: SIM is not intended for data-mode (cf. MODE).");
                }
            }
        }

        public class Res
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set                        
            public void Exe()
            {
                Program.Res(this);
            }
        }

        public class Model
        {
            public string fileName = null;
            public bool info = false;
            public P p = null;
            public void Exe()
            {                
                Program.Model(this);
                if (G.equal(Program.options.interface_mode, "data"))
                {
                    G.Writeln2("+++ WARNING: MODEL is not intended for data-mode (cf. MODE).");
                }
            }
        }

        public class Collapse
        {
            public string b0 = null;
            public string b1 = null;
            public string v0 = null;
            public string v1 = null;
            public string type = null;            
            public void Exe()
            {
                Program.Collapse(this.b1,this.v1, this.b0, this.v0, type);                
            }
        }

        public class Interpolate
        {
            public string b0 = null;
            public string b1 = null;
            public string v0 = null;
            public string v1 = null;
            public string type = null;
            public void Exe()
            {
                Program.Interpolate(this.b1, this.v1, this.b0, this.v0, type);
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
                if (G.equal(mode, "sim"))
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
                else if (G.equal(mode, "data"))
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
                else if (G.equal(mode, "mixed"))
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
                //Globals.r_fileName = this.fileName;
                Globals.r_fileContent = G.ExtractLinesFromText(Program.GetTextFromFileWithWait(this.fileName));
            }
        }

        public class R_export
        {
            public List<string> r_exportItems = null;
            public string opt_target = null;
            public void Exe()
            {
                string all = null;
                foreach (string s in this.r_exportItems)
                {
                    IVariable iv = null; Program.scalars.TryGetValue(Globals.symbolList + s, out iv);
                    if (iv != null && iv.Type() == EVariableType.Matrix)
                    {
                        Matrix m = (Matrix)iv;
                        string xxx = Program.MatrixFromGekkoToR<double>(s, m.data);
                        all += xxx + G.NL;
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Could not find matrix " + Globals.symbolList + s);
                        throw new GekkoException();
                    }
                }
                if (this.opt_target == null)
                {
                    //insert at top
                    List<string> l2 = new List<string>();
                    l2.Add(all);
                    l2.AddRange(Globals.r_fileContent);
                    Globals.r_fileContent = l2;
                }
                else
                {
                    bool hit = false;
                    List<string> l2 = new List<string>();
                    foreach (string line in Globals.r_fileContent)
                    {
                        l2.Add(line);                        
                        if (line.TrimStart().ToLower().StartsWith("gekkoimport "))
                        {
                            string[] ss = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (ss.Length > 1)
                            {
                                if (G.IsIdent(ss[1]))
                                {
                                    string foundBlock = ss[1];
                                    if(G.equal(this.opt_target,foundBlock)) {
                                        l2.Add(all);
                                        hit = true;
                                    }
                                }
                            }
                        }                        
                    }
                    if (hit == false)
                    {
                        G.Writeln2("*** ERROR: Could not find statement 'gekkoimport " + this.opt_target + "' in the R file");
                        throw new GekkoException();
                    }
                    Globals.r_fileContent = l2;
                }
            }
        }
        public class R_run
        {
            public string opt_mute = null;
            public void Exe()
            {
                Program.RunR(this);
            }
        }


        public class X12a
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public List<string> listItems = null;
            public string opt_bank = null;
            public string opt_param = null;
            public void Exe()
            {
                Program.X12a(this);
            }
        }

        public class Write
        {
            public GekkoTime t1 = Globals.tNull; //default, if not explicitely set
            public GekkoTime t2 = Globals.tNull; //default, if not explicitely set
            public string fileName = null;
            public List<string> listItems = null;
            public string opt_tsd = null;
            public string opt_tsdx = null;
            public string opt_gbk = null;
            public string opt_csv = null;
            public string opt_prn = null;
            public string opt_tsp = null;
            public string opt_xls = null;
            public string opt_xlsx = null;
            public string opt_gnuplot= null;            
            public string opt_caps = null;
            public string opt_series = null;
            public string type = null;  //THIS IS NOT WORKING PROPERLY!!
            public void Exe()
            {                
                Program.Write(this);
            }
        }

        public class Translate
        {
            public string fileName = null;
            public string opt_gekko18 = null;
            public string opt_aremos = null;
            public void Exe()
            {
                if (opt_aremos == null && opt_gekko18 == null)
                {
                    G.Writeln2("*** ERROR: Please use <gekko18> or <aremos> to state the language");
                    throw new GekkoException();
                }
                string zfilename = Program.CreateFullPathAndFileName(Program.AddExtension(this.fileName, ".cmd"));
                string xx = Program.GetTextFromFileWithWait(zfilename);
                List<string> xxx = G.ExtractLinesFromText(xx);
                //string s1 = null;
                //string s2 = null;
                //G.ExtractNameAndExtension(zfilename, out s1, out s2);
                if (zfilename.ToLower().EndsWith(".cmd") || zfilename.ToLower().EndsWith("." + Globals.extensionCommand)) 
                    zfilename = zfilename.Substring(0, zfilename.Length - 4);
                string zz = zfilename + "." + Globals.extensionCommand;
                if (File.Exists(zz))
                {
                    G.Writeln2("*** ERROR: The destination file '" + zz + "' already exists:");                    
                    throw new GekkoException();
                }                
                if (G.equal(opt_gekko18, "yes"))
                {                    
                    string ss = Translators.Translate1(true, xxx);
                    using (FileStream fs = Program.WaitForFileStream(zz, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    {
                        sw.Write(ss);
                        sw.Flush();
                        sw.Close();
                    }
                    G.Writeln2("Translated file into: " + zz);
                }
                else if (G.equal(opt_aremos, "yes"))
                {
                    string ss = Translator2.Translate2(true, xxx);
                    using (FileStream fs = Program.WaitForFileStream(zz, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    {
                        sw.Write(ss);
                        sw.Flush();
                        sw.Close();
                    }
                    G.Writeln2("Translated file into: " + zz);
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
