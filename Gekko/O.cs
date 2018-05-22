﻿using System;
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

        public static ScalarString scalarStringPercent = new ScalarString(Globals.symbolScalar.ToString());
        public static ScalarString scalarStringHash = new ScalarString(Globals.symbolCollection.ToString());
        public static ScalarString scalarStringTilde = new ScalarString(Globals.freqIndicator.ToString());
        public static ScalarString scalarStringColon = new ScalarString(Globals.symbolBankColon.ToString());

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

        public class HandleEndoHelper2
        {
            public GekkoTimes global = null;            
            public List<HandleEndoHelper> helper = null;
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

        //public static void AssignVal(IVariable lhs, IVariable rhs)
        //{
        //    string s = O.ConvertToString(lhs);
        //    double d = O.ConvertToVal(null, rhs);
        //    IVariable iv = null; Program.scalars.TryGetValue(s, out iv);
        //    if (iv != null)
        //    {
        //        if (iv.Type() == EVariableType.Val)
        //        {
        //            ((ScalarVal)iv).val = d;
        //        }
        //        else
        //        {
        //            Program.scalars.Remove(s);
        //            Program.scalars.Add(s, new ScalarVal(d));
        //        }

        //    }
        //    else
        //    {
        //        Program.scalars.Add(s, new ScalarVal(d));
        //    }
        //}

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

        public static IVariable Power(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return x.Power(smpl, y);
        }

        public static IVariable Negate(GekkoSmpl smpl, IVariable x)
        {
            return x.Negate(smpl);
        }

        public static IVariable AndAdd(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return Functions.union(smpl, x, y);
        }

        public static IVariable AndSubtract(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return Functions.difference(smpl, x, y);
        }

        public static IVariable AndMultiply(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            return Functions.intersect(smpl, x, y);
        }

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
                    if (ts.freq == EFreq.Annual) a++;
                    else if (ts.freq == EFreq.Quarterly) q++;
                    else if (ts.freq == EFreq.Monthly) m++;
                    else if (ts.freq == EFreq.Undated) u++;
                }
                G.Writeln2("Databank " + bank.name + ":");
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

            string name = "endo"; if (!type) name = "exo";

            //Clear all endo_ or exo_ variables
            if (true)
            {
                List<string> delete = new List<string>();
                foreach (KeyValuePair<string, IVariable> kvp in databank.storage)
                {
                    if (kvp.Key.StartsWith(name + "_", StringComparison.OrdinalIgnoreCase) && kvp.Key.EndsWith(Globals.freqIndicator + G.GetFreq(Program.options.freq), StringComparison.OrdinalIgnoreCase))
                    {
                        //starts with endo_ or exo_ and is of annual type
                        delete.Add(kvp.Key);
                    }
                }
                foreach (string s in delete)
                {
                    databank.RemoveIVariable(s);
                }
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
            

            foreach (HandleEndoHelper h in helper)
            {
                string s = h.varname.ConvertToString();
                if (!G.IsSimpleToken(s))
                {
                    G.Writeln2("*** ERROR: The name '" + s + "' is not a simple series name");
                    throw new GekkoException();
                }
                List<string> ss = new List<string>();
                if (h.indices != null)
                {
                    foreach (IVariable iv in h.indices)
                    {
                        if (iv.Type() != EVariableType.String)
                        {
                            G.Writeln2("*** ERROR: Expected indices of '" + s + "' to be of string type");
                            G.Writeln("           List type will be supported soon.");
                            throw new GekkoException();
                        }
                        ss.Add(iv.ConvertToString());
                    }
                }
                                                
                string varNameWithoutFreq = name + "_" + s;
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
                    //Multi-dim timeseries
                    //What about timeless??                        

                    if (ts == null)
                    {
                        ts = new Series(Program.options.freq, varNameWithFreq);
                        ts.SetArrayTimeseries(ss.Count + 1, true);
                        databank.AddIVariable(ts.name, ts);
                    }

                    MapMultidimItem mmi = new MapMultidimItem(ss.ToArray(), ts);
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
                }
                else
                {
                    //Normal 0-dim timeseries
                    //What about timeless??
                    ts2 = ts;
                    if (ts2 == null)
                    {
                        ts2 = new Series(Program.options.freq, varNameWithFreq);
                        databank.AddIVariable(ts2.name, ts2);
                    }
                }

                foreach (GekkoTime t in new GekkoTimeIterator(gts.t1, gts.t2))
                {
                    ts2.SetData(t, 1d);
                }
            }
        }

        public static ScalarString SetStringData(IVariable name, IVariable rhs, bool isName)
        {
            //Returns the IVariable it finds here (or creates)
            string name2 = name.ConvertToString();
            string value = rhs.ConvertToString();
            IVariable lhs = null;
            if (Program.scalars.TryGetValue(name2, out lhs))
            {
                //Scalar is already existing                
                if (lhs.Type() == EVariableType.String)
                {
                    //Already existing lhs is a STRING, inject into it. Injecting is faster than recreating an object.                    
                    ((ScalarString)lhs).string2 = value;
                    //((ScalarString)lhs)._isName = isName;
                }
                else
                {
                    //The object has to die and be recreated, since it is of a wrong type.                                
                    Program.scalars.Remove(name2);
                    lhs = new ScalarString(value);
                    Program.scalars.Add(name2, lhs);
                }
            }
            else
            {
                //Scalar does not exist beforehand  
                lhs = new ScalarString(value);
                Program.scalars.Add(name2, lhs);
            }
            return (ScalarString)lhs;
        }

        public static ScalarDate SetDateData(IVariable name, IVariable rhs)
        {
            //Returns the IVariable it finds here (or creates)
            string name2 = name.ConvertToString();
            GekkoTime value = O.ConvertToDate(rhs);
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
                    Program.scalars.Add(name2, lhs);
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

        public static void IterateStep(IVariable x, IVariable start, IVariable step, int counter)
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
            else if (x.Type() == EVariableType.String)
            {
                //it is tested previously that step = null and start is metalist
                List start_list = start as List;
                if (counter >= start_list.list.Count)
                {
                    //do nothing, this x will not be used
                }
                else
                {
                    ScalarString x_string = x as ScalarString;
                    ScalarString item = start_list.list[counter] as ScalarString;
                    if (item == null)
                    {
                        G.Writeln2("*** ERROR: list element " + (counter + 1) + " is not a STRING");
                        throw new GekkoException();
                    }
                    x_string.string2 = item.string2;
                }
            }
            else throw new GekkoException();
        }

        public static void IterateStart(ref IVariable x, IVariable start)
        {
            if (x == null)
            {
                if (start.Type() == EVariableType.Val)
                {
                    x = new ScalarVal(((ScalarVal)start).val);
                }
                else if (start.Type() == EVariableType.Date)
                {
                    x = new ScalarDate(((ScalarDate)start).date);
                }
                else if (start.Type() == EVariableType.String)
                {
                    x = new ScalarString(((ScalarString)start).string2);
                }
                else if (start.Type() == EVariableType.List)
                {
                    List start_list = start as List;
                    if (start_list.list.Count == 0)
                    {
                        G.Writeln2("*** ERROR: Empty list");
                        throw new GekkoException();
                    }
                    ScalarString xx = start_list.list[0] as ScalarString;
                    if (xx == null)
                    {
                        G.Writeln2("*** ERROR: list element 1 is not a STRING");
                        throw new GekkoException();
                    }
                    x = new Gekko.ScalarString(xx.string2);
                    //x = start_list.list[0];  ----------------> FAIL, sideeffect because then the first item in the list will change when x changes....!!!
                }
                else throw new GekkoException();
            }
        }

        public static bool IterateContinue(IVariable x, IVariable start, IVariable max, IVariable step, ref int counter)
        {
            counter++;
            bool rv = false;
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
                if (max_date == null)
                {
                    G.Writeln2("*** ERROR: Expected max value to be DATE type, you may try the date() function");
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
                int step_int = O.ConvertToInt(step_val);
                if (step_int == 0)
                {
                    G.Writeln2("*** ERROR: Step value cannot be 0");
                    throw new GekkoException();
                }

                //GekkoTime gt = x_date.date.Add(step_int);
                if (step_val.val > 0)
                {
                    return x_date.date.SmallerThanOrEqual(max_date.date);
                }    
                else
                {                    
                    return x_date.date.LargerThanOrEqual(max_date.date);
                }            
            }
            else if (x.Type() == EVariableType.String)
            {
                if (max != null)
                {
                    G.Writeln2("*** ERROR: string loops do not have TO argument");
                    throw new GekkoException();
                }
                if (step != null)
                {
                    G.Writeln2("*** ERROR: string loops do not have STEP/BY argument");
                    throw new GekkoException();
                }
                List start_list = start as List;

                if (start_list == null)
                {
                    G.Writeln2("*** ERROR: Expected FOR to loop over list, not a " + G.GetTypeString(start) + " type");
                    throw new GekkoException();
                }

                if (counter <= start_list.list.Count) rv = true;

            }
            else throw new GekkoException();
            return rv;
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
            List<IVariable> m = new List<Gekko.IVariable>();
            foreach (IVariable iv in x)
            {
                m.Add(iv);
            }
            return new List(m);
        }

        public static IVariable Dollar(GekkoSmpl smpl, IVariable x, IVariable logical)
        {
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

        public static bool IsTrue(double d)
        {
            if (d != 0d) return true;
            else return false;
        }

        //NOTE: Must have same signature as Lookup(), #89075234532
        public static void DollarLookup(IVariable logical, GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, bool isLeftSideVariable, EVariableType type)
        {
            //Only encountered on the LHS
            if (logical == null)
            {
                Lookup(smpl, map, dbName, varname, freq, rhsExpression, isLeftSideVariable, type);
            }
            if (logical.Type() == EVariableType.Val)
            {
                if (IsTrue(((ScalarVal)logical).val))
                {
                    Lookup(smpl, map, dbName, varname, freq, rhsExpression, isLeftSideVariable, type);
                }
                else
                {
                    //skip it!
                }
            }
            else
            {
                DollarLHSError();
            }
        }

        public static IVariable Lookup(GekkoSmpl smpl, Map map, IVariable x, IVariable rhsExpression, bool isLeftSideVariable, EVariableType type)
        {
            return Lookup(smpl, map, x, rhsExpression, isLeftSideVariable, type, true);
        }

        public static IVariable NameLookup(GekkoSmpl smpl, Map map, IVariable x, IVariable rhsExpression, bool isLeftSideVariable, EVariableType type)
        {
            return x;
        }

        //NOTE: Must have same signature as DollarLookup(), #89075234532
        public static IVariable Lookup(GekkoSmpl smpl, Map map, IVariable x, IVariable rhsExpression, bool isLeftSideVariable, EVariableType type, bool errorIfNotFound)
        {
            //This calls the more general Lookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression)

            if (x.Type() == EVariableType.String)
            {                
                string dbName, varName, freq; char firstChar; Chop((x as ScalarString).string2, out dbName, out varName, out freq);                
                IVariable iv = Lookup(smpl, map, dbName, varName, freq, rhsExpression, isLeftSideVariable, type, errorIfNotFound);
                return iv;
            }
            else if (x.Type() == EVariableType.List)
            {
                //for instance PRT {#m}, where #m is a list of strings. Then #m will already have been looked up, when {} is called,
                //so Lookup is called again, this time with a list as "key"
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
                    foreach(string s in items)
                    {
                        string dbName, varName, freq; char firstChar; Chop(s, out dbName, out varName, out freq);
                        IVariable iv = Lookup(smpl, map, dbName, varName, freq, rhsExpression, isLeftSideVariable, type, errorIfNotFound);
                        rv.Add(iv);
                    }
                    List m = new List(rv);
                    return m;
                }
            }
            else
            {

                G.Writeln2("*** ERROR: Var type error when looking up in databank");
                throw new GekkoException();

            }
            return x;
        }

        public static IVariable Lookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, bool isLeftSideVariable, EVariableType type)
        {
            return Lookup(smpl, map, dbName, varname, freq, rhsExpression, isLeftSideVariable, type, true);
        }

        public static IVariable NameLookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, bool isLeftSideVariable, EVariableType type)
        {
            if (dbName != null || freq != null)
            {
                G.Writeln2("*** ERROR: Expected a simple variable name without bank or frequency");
                throw new GekkoException();
            }

            return new ScalarString(varname);
        }

        //Also see #8093275432098
        public static IVariable Lookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, bool isLeftSideVariable, EVariableType type, bool errorIfNotFound)
        {
            //map != null:             the variable is found in the MAP, otherwise, the variable is found in a databank
            //rhsExpression != null:   it is an assignment of the left-hand side

            //only adds freq if not there. No sigil is added for lhs vars here.
            string varnameWithFreq = AddFreq(varname, freq, type, isLeftSideVariable);

            if (isLeftSideVariable)
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
                    if (dbName == null) db = Program.databanks.GetFirst();  //on the LHS, we can only look in the first databank, if databank is not stated
                    else db = Program.databanks.GetDatabank(dbName);
                    ib = db;
                }

                if (rhsExpression != null)
                {
                    //direct assignment, like x = 5, or %s = 'a'
                    //in these cases, the LHS can be created if it is not already existing
                    //ScalarString ss = rhsExpression as ScalarString;                    
                    LookupHelperLeftside(smpl, ib, varnameWithFreq, freq, rhsExpression, type);
                    return null;
                }
                else
                {
                    //indexers on lhs, for instance x['a'] = ... or x[2000] = 5 or #x.%s = ...
                    //in this case, the x variable must exist
                    //NOTE: no databank search is allowed!
                    //NOTE: sigils cannot be omitted here. VAL x['v'] = 100 or VAL x.v = 100 will not access a %v variable.
                    IVariable ivar2 = ib.GetIVariable(varnameWithFreq);
                    if (ivar2 == null)
                    {
                        G.Writeln2("*** ERROR: Could not find variable '" + varnameWithFreq + "' for use in dot- or []-indexing");
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
                return LookupHelperRightside(smpl, map, dbName, varnameWithFreq, errorIfNotFound);
            }
        }
                
        //Also see #8093275432098
        public static string NameLookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, bool isLeftSideVariable, EVariableType type, bool errorIfNotFound)
        {            
            return varname;            
        }

        ////Also see #8093275432098
        //public static string LookupOnlyGetName(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, bool isLeftSideVariable, EVariableType type)
        //{
        //    return LookupOnlyGetName(smpl, map, dbName, varname, freq, rhsExpression, isLeftSideVariable, type, true);
        //}

        public static string AddFreq(string varname, string freq, EVariableType type, bool isLeftSideVariable)
        {
            //freq is added for all no-sigil rhs
            //freq is added for lhs if it is no-sigil AND the type is SERIES or VAR

            bool hasSigil = G.HasSigil(varname);

            string varnameWithFreq = varname;

            if ((!isLeftSideVariable && !hasSigil) || (isLeftSideVariable && !hasSigil && (type == EVariableType.Var || type == EVariableType.Series)))
            {
                //Series has '!' added
                //In VAL v = 100, there will be no freq added.
                if (!varname.Contains(Globals.freqIndicator.ToString()))
                {
                    if (freq != null) varnameWithFreq = varname + Globals.freqIndicator + freq;
                    else varnameWithFreq = varname + Globals.freqIndicator + G.GetFreq(Program.options.freq);
                }
            }

            return varnameWithFreq;
        }        

        private static IVariable LookupHelperRightside(GekkoSmpl smpl, Map map, string dbName, string varnameWithFreq)
        {
            return LookupHelperRightside(smpl, map, dbName, varnameWithFreq, true);
        }

        private static IVariable LookupHelperRightside(GekkoSmpl smpl, Map map, string dbName, string varnameWithFreq, bool errorIfNotFound)
        {
            //Can either look up stuff in a Map, or in a databank

            //if (Globals.runningOnTTComputer)
            //{
            //    G.Writeln2("---> LABELS(varname) = " + dbName + ":" + varnameWithFreq);
            //}
                       
            IVariable rv = null;
            if (map == null)
            {
                //It must be a databank then
                if (dbName == null)
                {
                    //databank name not given, for instance "PRT x"
                    if (Program.options.databank_search)
                    {
                        //DATA mode
                        //Search if on the right-hand side (rhs), in data mode, and no bank is indicated
                        if (smpl != null && smpl.bankNumber == 1 && !G.StartsWithSigil(varnameWithFreq))
                        {
                            //we only do this for timeseries!
                            rv = Program.databanks.GetRef().GetIVariable(varnameWithFreq);
                            if (rv == null && errorIfNotFound)
                            {
                                G.Writeln2("*** ERROR: Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in databank 'Ref'");
                                throw new GekkoException();
                            }
                        }
                        else
                        {
                            rv = GetVariableSearch(rv, varnameWithFreq);
                            if (rv == null && errorIfNotFound)
                            {
                                G.Writeln2("*** ERROR: Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in any open databank (excluding Ref)");
                                throw new GekkoException();
                            }
                        }
                    }
                    else
                    {
                        //SIM mode, can only fetch it in the primary databank (unless bankNumber is active)
                        if (smpl != null && smpl.bankNumber == 1 && !G.StartsWithSigil(varnameWithFreq))
                        {
                            //at the moment, this logic also includes VALs etc. !!!!!!!!!!!!!!!!!!!! (why not, really?)
                            rv = Program.databanks.GetRef().GetIVariable(varnameWithFreq);
                            if (rv == null && errorIfNotFound)
                            {
                                G.Writeln2("*** ERROR: Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in databank 'Ref'");
                                throw new GekkoException();
                            }
                        }
                        else
                        {
                            rv = Program.databanks.GetFirst().GetIVariable(varnameWithFreq);
                            if (rv == null && errorIfNotFound)
                            {
                                G.Writeln2("*** ERROR: Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in the first-position databank ('" + Program.databanks.GetFirst().name + "')");
                                throw new GekkoException();
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
                        rv = Program.databanks.GetRef().GetIVariable(varnameWithFreq);
                        if (rv == null && errorIfNotFound)
                        {
                            G.Writeln2("*** ERROR: Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in databank 'Ref'");
                            throw new GekkoException();
                        }
                    }
                    else
                    {
                        //databank name is given explicitly, and we are not doing bankNumber stuff
                        //We use the IBank interface here (map==null)
                        rv = LookupHelperRightside2(map, dbName, varnameWithFreq);
                        if (rv == null && errorIfNotFound)
                        {
                            G.Writeln2("*** ERROR: Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in databank '" + dbName + "'");
                            throw new GekkoException();
                        }
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
            
            return rv;
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
            else ib = GetDatabankNoSearch(dbName);
            if (ib == null)
            {
                G.Writeln2("*** ERROR: Databank '" + dbName + "' does not seem to be open.");
                throw new GekkoException();
            }
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
            InitSmpl(smpl, 0, p);
        }

        public static void InitSmpl(GekkoSmpl smpl, int i, P p)
        {
            //called before each command is run
            if (smpl != null)
            {
                smpl.t0 = Globals.globalPeriodStart.Add(Globals.smplInitStart);
                smpl.t1 = Globals.globalPeriodStart;
                smpl.t2 = Globals.globalPeriodEnd;
                smpl.t3 = Globals.globalPeriodEnd.Add(Globals.smplInitEnd);
                smpl.gekkoError = null;
                smpl.gekkoErrorI = 0;
                smpl.bankNumber = 0;
                //p.numberOfServiceMessages = 0;
                smpl.p = p;
            }
        }        

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
                    G.Writeln2("*** ERROR: Cannot convert " + m.data.GetLength(0) + "x" + m.data.GetLength(1) + " MATRIX to " + n + " obs SERIES");
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
                else if(m.list.Count() == n)
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
                    ib.AddIVariable(varnameWithFreq, rhsExpression.DeepClone());
                }
                else if (varnameWithFreq[0] == Globals.symbolCollection)
                {
                    //LIST, DICT, MATRIX                                                
                    ib.AddIVariable(varnameWithFreq, rhsExpression.DeepClone());
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
                        ib.AddIVariable(varnameWithFreq, rhsExpression.DeepClone());
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
                    ib.AddIVariable(varnameWithFreq, rhsExpression.DeepClone());
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
                            Series tsTmp = (Series)rhsExpression.DeepClone();
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

        public static void LookupHelperLeftside(GekkoSmpl smpl, IBank ib, string varnameWithFreq, string freq, IVariable rhs, EVariableType type)
        {
            //normal use
            LookupHelperLeftside(smpl, ib, varnameWithFreq, freq, rhs, null, type);        
        }

        public static void LookupHelperLeftside(GekkoSmpl smpl, Series arraySubSeries, IVariable rhs, EVariableType type)
        {
            //use for array-series, for instance xx['a'] = ...
            LookupHelperLeftside(smpl, null, null, null, rhs, arraySubSeries, type);
        }

        private static void LookupHelperLeftside(GekkoSmpl smpl, IBank ib, string varnameWithFreq, string freq, IVariable rhs, Series arraySubSeries, EVariableType type)
        {
            //This is an assignment, for instance %x = 5, or x = (1, 2, 3), or bank:x = bank:y
            //Assignment is the hardest part of Lookup()

            bool isArraySubSeries = false;
            if (arraySubSeries != null) isArraySubSeries = true;

            varnameWithFreq = G.AddSigil(varnameWithFreq, type);

            IVariable lhs = null;
            if (ib != null) lhs = ib.GetIVariable(varnameWithFreq); //may return null

            if (!isArraySubSeries && varnameWithFreq[0] == Globals.symbolScalar)
            {
                switch (rhs.Type())
                {
                    case EVariableType.Series:
                        {
                            Series rhsExpression_series = rhs as Series;
                            switch (rhsExpression_series.type)
                            {
                                case ESeriesType.Normal:
                                    {
                                        //---------------------------------------------------------
                                        // %x = Series Normal
                                        //---------------------------------------------------------

                                        G.Writeln2("*** ERROR: Type mismatch");
                                        throw new GekkoException();
                                    }
                                    break;
                                case ESeriesType.Light:
                                    {
                                        //---------------------------------------------------------
                                        // %x = Series Light
                                        //---------------------------------------------------------
                                        G.Writeln2("*** ERROR: Type mismatch");
                                        throw new GekkoException();
                                    }
                                    break;
                                case ESeriesType.Timeless:
                                    {
                                        //---------------------------------------------------------
                                        // %x = Series Timeless
                                        //---------------------------------------------------------
                                        IVariable lhsNew = new ScalarVal(rhsExpression_series.GetTimelessData());
                                        AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                        G.ServiceMessage("VAL " + varnameWithFreq + " updated ", smpl.p);

                                    }
                                    break;
                                case ESeriesType.ArraySuper:
                                    {
                                        //---------------------------------------------------------
                                        // %x = Series Array Super
                                        //---------------------------------------------------------
                                        G.Writeln2("*** ERROR: Type mismatch");
                                        throw new GekkoException();
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
                            // %x = VAL
                            //---------------------------------------------------------
                            //TODO: can be injected if exist and is val                            
                            IVariable lhsNew = new ScalarVal(((ScalarVal)rhs).val);
                            AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);                                                        
                            G.ServiceMessage("VAL " + varnameWithFreq + " updated ", smpl.p);
                        }
                        break;
                    case EVariableType.String:
                        {
                            //---------------------------------------------------------
                            // %x = STRING
                            //---------------------------------------------------------                            
                            IVariable lhsNew = new ScalarString(((ScalarString)rhs).string2);
                            AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                            G.ServiceMessage("STRING " + varnameWithFreq + " updated ", smpl.p); 
                        }
                        break;
                    case EVariableType.Date:
                        {
                            //---------------------------------------------------------
                            // %x = DATE
                            //---------------------------------------------------------
                            IVariable lhsNew = new ScalarDate(((ScalarDate)rhs).date);
                            AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                            G.ServiceMessage("DATE " + varnameWithFreq + " updated ", smpl.p);
                        }
                        break;
                    case EVariableType.List:
                        {
                            //---------------------------------------------------------
                            // %x = LIST
                            //---------------------------------------------------------

                            G.Writeln2("*** ERROR: Type mismatch");
                            throw new GekkoException();
                        }
                        break;
                    case EVariableType.Map:
                        {
                            //---------------------------------------------------------
                            // %x = MAP
                            //---------------------------------------------------------

                            G.Writeln2("*** ERROR: Type mismatch");
                            throw new GekkoException();

                        }
                        break;
                    case EVariableType.Matrix:
                        {
                            //---------------------------------------------------------
                            // %x = MATRIX
                            //---------------------------------------------------------                            
                            IVariable lhsNew = new ScalarVal(rhs.ConvertToVal());  //only 1x1 matrix will become VAL
                            AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                            G.ServiceMessage("VAL " + varnameWithFreq + " updated ", smpl.p);
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
            else if (!isArraySubSeries && varnameWithFreq[0] == Globals.symbolCollection)
            {
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
                                        // #x = Series Normal
                                        //---------------------------------------------------------

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
                                        int i1 = rhs_series.FromGekkoTimeToArrayIndex(smpl.t1);
                                        int i2 = rhs_series.FromGekkoTimeToArrayIndex(smpl.t2);
                                        Matrix m = new Matrix(1, n);
                                        double[,] destination = m.data;
                                        double[] source = rhs_series.data.dataArray;

                                        int destinationStart = 0;
                                        int ii1 = Math.Max(0, i1);
                                        int ii2 = Math.Min(source.Length - 1, i2);
                                        for (int j = i1; j < 0; j++)
                                        {
                                            destination[1, j - i1 + destinationStart] = double.NaN;
                                        }
                                        for (int j = i2; j >= source.Length; j--)
                                        {
                                            destination[1, j - i1 + destinationStart] = double.NaN;
                                        }
                                        //see also #0985324985237
                                        Buffer.BlockCopy(source, 8 * ii1, destination, 8 * destinationStart, 8 * (ii2 - ii1 + 1));
                                        IVariable lhsNew = m;
                                        AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                        G.ServiceMessage("MATRIX " + varnameWithFreq + " updated ", smpl.p);
                                    }
                                    break;
                                case ESeriesType.Light:
                                    {
                                        //---------------------------------------------------------
                                        // #x = Series Light
                                        //---------------------------------------------------------
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
                                        double[] source = rhs_series.data.dataArray;
                                        //see #0985324985237
                                        Buffer.BlockCopy(source, 8 * ii1, destination, 8 * destinationStart, 8 * (ii2 - ii1 + 1));
                                        IVariable lhsNew = m;
                                        AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                        G.ServiceMessage("MATRIX " + varnameWithFreq + " updated ", smpl.p);
                                    }
                                    break;
                                case ESeriesType.Timeless:
                                    {
                                        //---------------------------------------------------------
                                        // #x = Series Timeless
                                        //---------------------------------------------------------
                                        int n = smpl.Observations12();
                                        double d = rhs_series.data.dataArray[0];
                                        Matrix m = new Matrix(1, n, d);  //expanded as if it was a real timeseries                                       
                                        AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, m);
                                        G.ServiceMessage("MATRIX " + varnameWithFreq + " updated ", smpl.p);
                                    }
                                    break;
                                case ESeriesType.ArraySuper:
                                    {
                                        //---------------------------------------------------------
                                        // #x = Series Array Super
                                        //---------------------------------------------------------
                                        {
                                            G.Writeln2("*** ERROR: Type mismatch");
                                            throw new GekkoException();
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
                            G.Writeln2("*** ERROR: Type mismatch");
                            throw new GekkoException();
                        }
                        break;
                    case EVariableType.String:
                        {
                            //---------------------------------------------------------
                            // #x = STRING
                            //---------------------------------------------------------
                            {
                                G.Writeln2("*** ERROR: Type mismatch");
                                throw new GekkoException();
                            }
                        }
                        break;
                    case EVariableType.Date:
                        {
                            //---------------------------------------------------------
                            // #x = DATE
                            //---------------------------------------------------------
                            {
                                G.Writeln2("*** ERROR: Type mismatch");
                                throw new GekkoException();
                            }
                        }
                        break;
                    case EVariableType.List:
                        {
                            //---------------------------------------------------------
                            // #x = LIST
                            //---------------------------------------------------------                                                        
                            AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, rhs.DeepClone());
                            G.ServiceMessage("LIST " + varnameWithFreq + " updated ", smpl.p);
                        }
                        break;
                    case EVariableType.Map:
                        {
                            //---------------------------------------------------------
                            // #x = MAP
                            //---------------------------------------------------------
                            AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, rhs.DeepClone());
                            G.ServiceMessage("MAP " + varnameWithFreq + " updated ", smpl.p);
                        }
                        break;
                    case EVariableType.Matrix:
                        {
                            //---------------------------------------------------------
                            // #x = MATRIX
                            //---------------------------------------------------------
                            AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, rhs.DeepClone());
                            G.ServiceMessage("MATRIX " + varnameWithFreq + " updated ", smpl.p);
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
                //name is of Series type, can have !isArraySubSeries == true.    

                Series lhs_series = null;
                if (isArraySubSeries) lhs_series = arraySubSeries;
                else lhs_series = lhs as Series;

                switch (rhs.Type())
                {
                    case EVariableType.Series:
                        {
                            Series rhs_series = rhs as Series;
                            string freq_rhs = G.GetFreq(rhs_series.freq);
                            if (varnameWithFreq != null && !varnameWithFreq.ToLower().EndsWith(Globals.freqIndicator + freq_rhs))  //null if it is a subseries under an array-superseries
                            {
                                G.Writeln2("*** ERROR: Frequency: illegal series name '" + varnameWithFreq + "', should end with '" + Globals.freqIndicator + freq_rhs + "'");
                                throw new GekkoException();
                            }
                            switch (rhs_series.type)
                            {
                                
                                case ESeriesType.Normal:
                                case ESeriesType.Light:
                                    {
                                        //---------------------------------------------------------
                                        // x = Series Normal or Light
                                        //---------------------------------------------------------

                                        GekkoTime tt1 = GekkoTime.tNull;
                                        GekkoTime tt2 = GekkoTime.tNull;
                                        GekkoTime.ConvertFreqs(G.GetFreq(freq, true), smpl.t1, smpl.t2, ref tt1, ref tt2);  //converts smpl.t1 and smpl.t2 to tt1 and tt2 in freq frequency
                                        bool create = CreateSeriesIfNotExisting(varnameWithFreq, freq, ref lhs_series);
                                        //Now the smpl window runs from tt1 to tt2
                                        //We copy in from that window
                                        if (lhs_series.freq != rhs_series.freq)
                                        {
                                            G.Writeln2("*** ERROR: Frequency mismatch");
                                            throw new GekkoException();
                                        }

                                        if (rhs_series.type == ESeriesType.Light)
                                        {
                                            int tooSmall = 0; int tooLarge = 0;
                                            rhs_series.TooSmallOrTooLarge(rhs_series.GetArrayIndex(tt1), rhs_series.GetArrayIndex(tt2), out tooSmall, out tooLarge);
                                            if (tooSmall > 0 || tooLarge > 0)
                                            {
                                                if (smpl.gekkoError == null) smpl.gekkoError = new GekkoError(tooSmall, tooLarge);
                                                return;
                                            }
                                        }

                                        int index1, index2;
                                        //may enlarge the array with NaNs first and last
                                        double[] data_beware_do_not_alter = rhs_series.GetDataSequenceUnsafePointerReadOnly(out index1, out index2, tt1, tt2);
                                        //may enlarge the array with NaNs first and last
                                        lhs_series.SetDataSequence(tt1, tt2, data_beware_do_not_alter, index1);
                                        if (create) AddIvariableWithOverwrite(ib, varnameWithFreq, true, lhs_series);
                                        G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                                    }
                                    break;
                                case ESeriesType.Timeless:
                                    {
                                        //---------------------------------------------------------
                                        // x = Series Timeless
                                        //---------------------------------------------------------
                                        // stuff below also handles array-timeseries just fine   
                                        double d = double.NaN;
                                        if (rhs_series.data.dataArray != null) d = rhs_series.data.dataArray[0];
                                        bool create = CreateSeriesIfNotExisting(varnameWithFreq, freq, ref lhs_series);
                                        foreach (GekkoTime t in smpl.Iterate12())
                                        {
                                            lhs_series.SetData(t, d);
                                        }
                                        if (create) AddIvariableWithOverwrite(ib, varnameWithFreq, true, lhs_series);
                                        G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                                    }
                                    break;
                                case ESeriesType.ArraySuper:
                                    {
                                        //---------------------------------------------------------
                                        // x = Series Array Super
                                        //---------------------------------------------------------
                                        if (isArraySubSeries)
                                        {
                                            G.Writeln2("*** ERROR: You cannot put an array-series inside an array-series");
                                            throw new GekkoException();
                                        }
                                        IVariable clone = rhs.DeepClone();
                                        ((Series)clone).name = varnameWithFreq;
                                        AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, clone);
                                        G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
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
                            bool create = CreateSeriesIfNotExisting(varnameWithFreq, freq, ref lhs_series);
                            foreach (GekkoTime t in smpl.Iterate12())
                            {
                                lhs_series.SetData(t, d);
                            }
                            if (create)
                            {
                                AddIvariableWithOverwrite(ib, varnameWithFreq, true, lhs_series);
                            }
                            else
                            {
                                //nothing to do, either already existing in bank/map or array-subseries
                            }
                            G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                        }
                        break;
                    case EVariableType.String:
                        {
                            //---------------------------------------------------------
                            // x = STRING
                            //---------------------------------------------------------
                            {
                                G.Writeln2("*** ERROR: Type mismatch");
                                throw new GekkoException();
                            }
                        }
                        break;
                    case EVariableType.Date:
                        {
                            //---------------------------------------------------------
                            // x = DATE
                            //---------------------------------------------------------
                            {
                                G.Writeln2("*** ERROR: Type mismatch");
                                throw new GekkoException();
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
                            int n = smpl.Observations12();
                            if (n != rhs_list.list.Count)
                            {
                                G.Writeln2("*** ERROR: Expected " + n + " list items, got " + rhs_list.list.Count);
                                throw new GekkoException();
                            }
                            bool create = CreateSeriesIfNotExisting(varnameWithFreq, freq, ref lhs_series);
                            for (int i = 0; i < rhs_list.list.Count; i++)
                            {
                                lhs_series.SetData(smpl.t1.Add(i), rhs_list.list[i].ConvertToVal());
                            }
                            if (create)
                            {
                                AddIvariableWithOverwrite(ib, varnameWithFreq, true, lhs_series);
                            }
                            else
                            {
                                //nothing to do, either already existing in bank/map or array-subseries
                            }
                            G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                        }
                        break;
                    case EVariableType.Map:
                        {
                            //---------------------------------------------------------
                            // x = MAP
                            //---------------------------------------------------------
                            {
                                G.Writeln2("*** ERROR: Type mismatch");
                                throw new GekkoException();
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
                            bool create = CreateSeriesIfNotExisting(varnameWithFreq, freq, ref lhs_series);

                            if (rhs_matrix.data.Length == 1)
                            {
                                double d = rhs.ConvertToVal();  //will fail with error if not 1x1                            

                                foreach (GekkoTime t in smpl.Iterate12())
                                {
                                    lhs_series.SetData(t, d);
                                }

                            }
                            else
                            {
                                int n = smpl.Observations12();
                                if (n != lhs_series.data.dataArray.GetLength(0))
                                {
                                    G.Writeln2("*** ERROR: Expected " + n + " list items, got " + lhs_series.data.dataArray.GetLength(0));
                                    throw new GekkoException();
                                }
                                for (int i = 0; i < lhs_series.data.dataArray.GetLength(0); i++)
                                {
                                    lhs_series.SetData(smpl.t1.Add(i), rhs_matrix.data[i, 0]);
                                }

                            }
                            if (create)
                            {
                                AddIvariableWithOverwrite(ib, varnameWithFreq, true, lhs_series);
                            }
                            else
                            {
                                //nothing to do, either already existing in bank/map or array-subseries
                            }
                            G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);

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

            return;

        }        

        private static bool CreateSeriesIfNotExisting(string varnameWithFreq, string freq, ref Series lhs_series)
        {            
            bool create = false;
            if (lhs_series != null && lhs_series.type == ESeriesType.Normal)
            {
                //do nothing, use it
            }
            else
            {                
                //create it
                create = true;
                lhs_series = new Series(ESeriesType.Normal, G.GetFreq(freq,true), varnameWithFreq);
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

        private static IVariable GetVariableSearch(IVariable lhs, string varName)
        {
            for (int i = 0; i < Program.databanks.storage.Count; i++)
            {
                if (i == 1) continue;  //The Ref databank IS NEVER SEARCHED!!
                Databank db2 = Program.databanks.storage[i];
                lhs = db2.GetIVariable(varName);
                if (lhs != null) break;
            }

            return lhs;
        }

        private static Databank GetDatabankNoSearch(string dbName)
        {
            Databank db = null;
            if (dbName == null) db = Program.databanks.GetFirst();
            else db = Program.databanks.GetDatabank(dbName);
            return db;
        }

        public static void Print(GekkoSmpl smpl, IVariable x)
        {
            //Program.OPrint(smpl, x);
        }

        public static IVariable AddSpecial(IVariable x1, IVariable x2, bool minus)
        {
            ScalarString s1 = x1 as ScalarString;
            if (s1 == null)
            {
                G.Writeln2("*** ERROR: Expected list element to be of string type");
                throw new GekkoException();
            }
            string s = s1.string2;
            int i = -12345;
            if (int.TryParse(s, out i))
            {

            }
            else
            {
                G.Writeln2("*** ERROR: Could not convert '" + s + "' into integer");
                throw new GekkoException();
            }
                        
            int i2 = O.ConvertToInt(x2);

            int ii = i + i2;
            if (minus) ii = i - i2;

            return new ScalarVal(ii);
        }
        

        //private static string DecorateWithTilde(string varName, string freq)
        //{
        //    if (varName[0] != Globals.symbolMemvar && varName[0] != Globals.symbolList)
        //    {
        //        if (freq == null) varName += Globals.symbolTilde + G.GetFreq(Program.options.freq);
        //    }
        //    return varName;
        //}

        //private static Series GetLeftSideVariable(string dbName, string varName)
        //{
        //    Databank db = Program.databanks.GetFirst();
        //    if (dbName != null) db = Program.databanks.GetDatabank(dbName);
        //    Series ts = db.GetVariable(varName);
        //    if (ts == null)
        //    {
        //        ts = new Series(Program.options.freq, varName);
        //        db.AddVariable(ts);
        //    }

        //    return ts;
        //}

        public static void Chop(string input, out string dbName, out string varName, out string freq)
        {
            //When it returns, all returned strings are guaranteed not to contain colon or !.
            string[] ss = input.Split(Globals.symbolBankColon2);
            if (ss.Length > 2)
            {
                G.Writeln2("*** ERROR: More than 1 colons (':') in '" + input + "'");
                throw new GekkoException();
            }
            dbName = null;
            varName = null;

            if (ss.Length == 1) varName = ss[0];
            else if (ss.Length == 2)
            {
                dbName = ss[0]; varName = ss[1];
            }
            //firstChar = varName[0];

            freq = null;
            G.ChopFreq(varName, ref freq, ref varName);
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

        public static void DollarIndexerSetData(IVariable logical, GekkoSmpl smpl, IVariable x, IVariable y, params IVariable[] indexes)
        {
            //Only encountered on the LHS
            if (logical == null)
            {
                x.IndexerSetData(smpl, y, indexes);
            }
            if (logical.Type() == EVariableType.Val)
            {
                if (IsTrue(((ScalarVal)logical).val))
                {
                    x.IndexerSetData(smpl, y, indexes);
                }
                else
                {
                    //skip it!
                }
            }
            else
            {
                DollarLHSError();
                return;
            }
        }

        private static void DollarLHSError()
        {
            G.Writeln2("*** ERROR: $-conditional on left-hand side only supports VAL type");
            throw new GekkoException();
        }

        public static void IndexerSetData(GekkoSmpl smpl, IVariable x, IVariable y, params IVariable[] indexes)
        {
            x.IndexerSetData(smpl, y, indexes);
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
        public static GekkoSmpl2 Indexer2(GekkoSmpl smpl, params IVariable[] indexes)
        {
            GekkoSmpl2 smplRemember = null;
            int i = -12345; GekkoTime t = GekkoTime.tNull;
            Series.FindLagLeadFixed(ref i, ref t, indexes);
            if (i != -12345)
            {
                smplRemember = new GekkoSmpl2();
                smplRemember.t0 = smpl.t0;
                smplRemember.t3 = smpl.t3;

                if (Series.IsLagOrLead(i))
                {
                    smpl.t0 = smpl.t0.Add(i);
                    smpl.t3 = smpl.t3.Add(i);
                }
                else
                {
                    smpl.t0 = new GekkoTime(EFreq.Annual, i, 1);
                    smpl.t3 = new GekkoTime(EFreq.Annual, i, 1);
                }
            }
            else if (!t.IsNull())
            {
                smpl.t0 = t;
                smpl.t3 = t;
            }
            return smplRemember;
        }

        //See Indexer2() above. The first argument should be Indexer2(smpl, indexes)
        public static IVariable Indexer(GekkoSmpl2 smplRemember, GekkoSmpl smpl, IVariable x, params IVariable[] indexes)
        {
            Program.RevertSmpl(smplRemember, smpl);

            if (x == null)
            {
                if (indexes.Length == 1)
                {
                    //[y]
                    //['q*']
                    ScalarString ss = new ScalarString(Globals.indexerAloneCheatString);  //a bit cheating, but we save an interface method, and performance is not really an issue when indexing whole databanks
                    return ss.Indexer(smpl, indexes);
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

            if (true)
            {
                string s = null;
                foreach (IVariable iv in indexes)
                {
                    if (iv.Type() == EVariableType.String)
                    {
                        s += ((ScalarString)iv).string2 + " ";
                    }
                }
                //if (Globals.runningOnTTComputer)
                //{
                //    G.Writeln2("---> LABELS(indexes) = " + s);
                //}
            }

            IVariable rv = x.Indexer(smpl, indexes);
            return rv;

        }

        public static IVariable ReportInterior(GekkoSmpl smpl, IVariable x)
        {
            if (smpl.labelHelper == null) smpl.labelHelper = new List<IVariable>();
            smpl.labelHelper.Add(x);
            return x;
        }

        public static IVariable IndexerPlus(GekkoSmpl smpl, IVariable x, bool isLhs, IVariable y)
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
                return x.Indexer(smpl, new IVariable[] { y });
            }
        }

        //public static IVariable Indexer(GekkoSmpl smpl, IVariable x, bool isLhs, IVariablesFilterRange y)
        //{            
        //    if (x == null)
        //    {
        //        //[y], where y is y1..y2
        //        //['fx'..'fy']
        //        ScalarString ss = new ScalarString(Globals.indexerAloneCheatString);  //a bit cheating, but we save an interface method, and performance is not really an issue when indexing whole databanks
        //        return ss.Indexer(smpl, y);
        //    }
        //    else
        //    {
        //        //x[y], where y is y1..y2
        //        //a[1..3] or #a['fx'..'fy'] or #a[fx..fy]
        //        return x.Indexer(smpl, y);
        //    }
        //}

        //public static IVariable Indexer(GekkoSmpl smpl, IVariable x, bool isLhs, IVariablesFilterRange y1, IVariablesFilterRange y2)
        //{
        //    if (x == null)
        //    {
        //        G.Writeln2("*** ERROR: Invalid syntax");
        //        throw new GekkoException();                
        //    }
        //    else
        //    {                
        //        //a[1..3, 2..5] or a[1..3, 5] or a[1, 2..5]                
        //        return x.Indexer(smpl, y1, y2);
        //    }
        //}

        //public static IVariable Indexer(GekkoSmpl smpl, IVariable x, bool isLhs, IVariable y1, IVariablesFilterRange y2)
        //{
        //    if (x == null)
        //    {
        //        G.Writeln2("*** ERROR: Invalid syntax");
        //        throw new GekkoException();
        //    }
        //    else
        //    {
        //        //a[1..3, 2..5] or a[1..3, 5] or a[1, 2..5]                
        //        return x.Indexer(smpl, y1, y2);
        //    }
        //}

        //public static IVariable Indexer(GekkoSmpl smpl, IVariable x, bool isLhs, IVariablesFilterRange y1, IVariable y2)
        //{
        //    if (x == null)
        //    {
        //        G.Writeln2("*** ERROR: Invalid syntax");
        //        throw new GekkoException();
        //    }
        //    else
        //    {
        //        //a[1..3, 2..5] or a[1..3, 5] or a[1, 2..5]                
        //        return x.Indexer(smpl, y1, y2);
        //    }
        //}        

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
                G.Writeln2("*** ERROR: Memory variable '" + Globals.symbolScalar + name + "' was not found");
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
            if (result.Count == 1 && G.Equal(result[0], "null"))
            {
                //LIST mylist = null; ---> empty list
                result = new List<string>();
            }
            List ml = new List(result);
            ml.isNameList = true;
            return ml;
        }

        private static void GetRawListElements(string fileName, List<string> input, List<string> result)
        {
            //quoted elements are not allowed, too
            //also out-commented items, and items with minus are ok
            int counter = 0;
            foreach (string ss in input)
            {
                counter++;
                string s = ss.Trim();  //will also remove any newline characters!

                bool quotes = false;     
                string s2 = Program.StripQuotes(s);
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

        public static IVariable ZList(IVariable iv)
        {
            string name = ConvertToString(iv);
            IVariable a = null;
            if (Program.scalars.TryGetValue(Globals.symbolCollection + name, out a))
            {
            }
            else
            {
                G.Writeln2("*** ERROR: List '" + Globals.symbolCollection + name + "' was not found");
                throw new GekkoException();
            }
            return a;
        }

        //public static IVariable ZGenr(string name)
        //{            
        //    IVariable a = null;
        //    if (Program.scalars.TryGetValue(name, out a))
        //    {
        //        if (a.Type() == EVariableType.String)
        //        {
        //            //GENR y = %s; <-- %s is a STRING
        //            Series ts = Program.databanks.GetFirst().GetVariable(((ScalarString)a).string2);
        //            //a = new MetaTimeSeries(ts, null, null);
        //            a = ts;
        //        }
        //        else
        //        {
        //            //GENR y = %x; <-- %x is a VAL
        //            //expected to be of VAL type, 
        //        }
        //    }
        //    else
        //    {
        //        G.Writeln2("*** ERROR: Memory variable '" + Globals.symbolScalar + name + "' was not found");
        //        throw new GekkoException();
        //    }
        //    return a;
        //}        

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
                    ((ScalarString)a).string2 = s;
                    //((ScalarString)a)._isName = isName;
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
                        ((ScalarString)x).string2 = s;
                        //((ScalarString)x)._isName = isName;
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
                    if (originalName.StartsWith(char.ToString(Globals.symbolCollection)))
                    {
                        G.Writeln2("*** ERROR: Could not find list or matrix '" + originalName + "'");
                        if (Program.scalars.ContainsKey(originalName.Substring(1)))
                        {
                            G.Writeln("    Did you intend to refer to the scalar '" + Globals.symbolScalar + originalName.Substring(1) + "'", Color.Red);
                        }
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Could not find scalar '" + originalName + "'");
                    }
                    throw new GekkoException();
                }
                //bool didTransform = false;
                //a = x;
                //x = MaybeStringify(x, stringify); //must be after fast pointer, so that a itself is not stringifyed
                //x = MaybeTransform(ref didTransform, x, transformationAllowed);
                //if (didTransform) a = x; //fast pointer to that object              
                //return x;
                return x;
            }            
        }

        //public static IVariable MaybeTransform(ref bool didTransform, IVariable x, bool transformationAllowed)
        //{
        //    if (x == null)
        //    {
        //        G.Writeln2("*** ERROR: Illegal transformation of variable");
        //        throw new GekkoException();
        //    }
        //    if (transformationAllowed && x.Type() == EVariableType.String)
        //    {
        //        ScalarString ss = (ScalarString)x;
        //        if (ss._isName)
        //        {
        //            MetaTimeSeries mts = O.GetTimeSeries(ss._string2, 1);
        //            x = mts;
        //            didTransform = true;
        //        }
        //    }
        //    return x;
        //}

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
        public static void RemoveScalar(string originalName)
        {
            //If a pointer also needs to be cleared, put that as second argument (else null)
            if (Program.scalars.ContainsKey(originalName))
            {
                Program.scalars.Remove(originalName);
            }
            //if (a != null) a = null;
        }        

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
        
        public static IVariable GetTimeSeriesFromCache(GekkoSmpl smpl, ref IVariable a, string originalName, int bankNumber)
        {
            return GetTimeSeriesFromCache(smpl, ref a, originalName, bankNumber, ECreatePossibilities.None);
        }

        public static IVariable GetTimeSeriesFromCache(GekkoSmpl smpl, ref IVariable a, string originalName, int bankNumber, ECreatePossibilities autoCreate)
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
                Series ats = GetTimeSeries(smpl, originalName, bankNumber, autoCreate);
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
                    Series ats = GetTimeSeries(smpl, originalName, bankNumber, autoCreate);
                    a = ats;  //sets the pointer
                    return ats;
                }
            }
        }        

        public static Series GetTimeSeriesFromList(GekkoSmpl smpl, IVariable list, IVariable index, int bankNumber)
        {
            if (list.Type() == EVariableType.List)
            {
                ScalarString x = (ScalarString)list.Indexer(smpl, new IVariable[] { index });  //will return ScalarString with .isName = true.
                Series mts = O.GetTimeSeries(smpl, x.string2, 1);  //always from work....
                return mts;
            }
            else
            {
                G.Writeln2("*** ERROR #398754");
                throw new GekkoException();
            }            
        }

        //public static IVariable GetValFromStringIndexer(GekkoSmpl smpl, string name, IVariable index, int bank)
        //{
        //    //Used to pick out a value from a list item, like #m[2][2015], where index=2015
        //    Series mts = O.GetTimeSeries(smpl, name, bank);  //always from work....
        //    IVariable result = O.Indexer(smpl, mts, index);
        //    return result;
        //}

        public static Series GetTimeSeries(GekkoSmpl smpl, string originalName, int bankNumber)
        {
            return GetTimeSeries(smpl, originalName, bankNumber, ECreatePossibilities.None);
        }

        //public static MetaTimeSeries GetTimeSeries(string originalName, int bankNumber)
        //{
        //    return GetTimeSeries(originalName, bankNumber, ECreatePossibilities.None);
        //}

        public static Series GetTimeSeries(GekkoSmpl smpl, string originalName, int bankNumber, ECreatePossibilities canAutoCreate)
        {
            Series ts = FindTimeSeries(originalName, bankNumber, canAutoCreate);
            //Series mts = new Series(smpl, ts);
            return ts;
        }
        

        public static Series FindTimeSeries(string originalName, int bankNumber, ECreatePossibilities canAutoCreate)
        {
            ExtractBankAndRestHelper h = Program.ExtractBankAndRest(originalName, EExtrackBankAndRest.OnlyStrings);
            if (h.bank == Globals.firstCheatString)
            {
                h.bank = Program.databanks.GetFirst().name;
            }
            if (bankNumber == 2)
            {
                h.bank = Program.databanks.GetRef().name;  //overrides the bank name given
                h.hasColon = true;  //signals later on that this bank is explicitely given, so we cannot search for the timeseries
            }
            Series ts = Program.FindOrCreateTimeseries(h.bank, h.name, canAutoCreate, h.hasColon, false);
            return ts;
        }

        public static Series FindTimeSeries(string originalName, int bankNumber)
        {
            return FindTimeSeries(originalName, bankNumber, ECreatePossibilities.None);
        }


        //public static MetaTimeSeries GetTimeSeries(string originalName, int bankNumber, ECreatePossibilities canAutoCreate)
        //{
        //    ExtractBankAndRestHelper h = Program.ExtractBankAndRest(originalName, EExtrackBankAndRest.OnlyStrings);

        //    if (h.bank == Globals.firstCheatString)
        //    {
        //        h.bank = Program.databanks.GetFirst().aliasName;
        //    }

        //    if (bankNumber == 2)
        //    {
        //        h.bank = Program.databanks.GetRef().aliasName;  //overrides the bank name given
        //        h.hasColon = true;  //signals later on that this bank is explicitely given, so we cannot search for the timeseries
        //    }
        //    Series ts = Program.FindOrCreateTimeseries(h.bank, h.name, canAutoCreate, h.hasColon, false);
        //    MetaTimeSeries mts = new MetaTimeSeries(ts);
        //    return mts;
        //}

        //public static MetaTimeSeries GetArrayTimeSeries(MetaTimeSeries mts, ECreatePossibilities create, params IVariable[] indexes)
        //{
        //    if (indexes.Length == 0) return mts;  //fast return for normal timeseries
        //    Series ts = O.GetArrayTimeSeries(mts.ts, create, indexes);            
        //    return new MetaTimeSeries(ts);
        //}

        //public static Series GetArrayTimeSeries(Series its, ECreatePossibilities create, IVariable[] indexes)
        //{
        //    //When a timeseries is on rhs, it will have crete=none. In that case, we expect dimension to fit. If dimension is -12345,
        //    //the timeseries does exist but has never been written to (is that possible)? 

        //    if (create == ECreatePossibilities.None)
        //    {
        //        //rhs
        //        if (its.dimensions == -12345)
        //        {
        //            //should not be possible?
        //            G.Writeln2("*** ERROR: Series " + its.variableName + " has no dimensions and no data");
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

        //    Series ts = null;

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
        //        //    its.dim.timeSeriesArray = new GekkoDictionary<string, Series>(StringComparer.OrdinalIgnoreCase);
        //        //    its.dimensions = indexes.Length;
        //        //}                               

        //        //we know that dimension >= 1                

        //        its.dim.timeSeriesArray.TryGetValue(hash, out ts);                                

        //        if (ts == null)
        //        {
        //            if (create != ECreatePossibilities.None)
        //            {
        //                ts = new Series(its.freqEnum, its.variableName + "[]");  //the name is not really used, but we could put in the indices...?
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
            string bankName = O.ConvertToString(x);
            List<string> items = O.GetStringList(y);
            List<string> newList = new List<string>();
            foreach (string s in items)
            {
                newList.Add(bankName + ":" + s);
            }
            return new List(newList);
        }

        //public static IVariable ZIndexer(string name)
        //{
        //    IVariable a = null;
        //    if (Program.scalars.TryGetValue(name, out a))
        //    {
        //        //VAL y = %s[2000]; <-- %s is a STRING
        //        //GENR y = %s[2000]; <-- %s is a STRING
        //        Series ts = Program.databanks.GetFirst().GetVariable(((ScalarString)a).string2);
        //        //a = new MetaTimeSeries(ts, null, null);
        //        a = ts;
        //    }
        //    else
        //    {
        //        G.Writeln2("*** ERROR: Memory variable '" + Globals.symbolScalar + name + "' was not found");
        //        throw new GekkoException();
        //    }
        //    return a;
        //}

        //========================================
        //======================================== Z() variants end
        //========================================
        
        //public static IVariable Z_OLD(string name)
        //{
        //    IVariable a = GetScalar(name);
        //    return a;
        //}


        // =================================== start comparisons ==================================

        public static bool StrictlySmallerThan(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            bool rv = false;
            if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (O.ConvertToDate(x).StrictlySmallerThan(O.ConvertToDate(y))) rv = true;
            }
            else if ((x.Type() == EVariableType.Series || x.Type() == EVariableType.Val) && (y.Type() == EVariableType.Series || y.Type() == EVariableType.Val))
            {
                //if (x.GetValOLD(smpl) < y.GetValOLD(smpl)) rv = true; //#8097534320985
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types do not match for '<' compare");
                throw new GekkoException();
            }
            return rv;
        }

        public static bool SmallerThanOrEqual(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            bool rv = false;
            if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (O.ConvertToDate(x).SmallerThanOrEqual(O.ConvertToDate(y))) rv = true;
            }
            else if ((x.Type() == EVariableType.Series || x.Type() == EVariableType.Val) && (y.Type() == EVariableType.Series || y.Type() == EVariableType.Val))
            {
                //if (x.GetValOLD(smpl) <= y.GetValOLD(smpl)) rv = true;  // //#8097534320985
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types do not match for '<=' compare");
                throw new GekkoException();
            }
            return rv;
        }

        public static bool IsTrue(GekkoSmpl smpl, IVariable x)
        {
            if (x.Type() == EVariableType.Val)
            {
                if (IsTrue(((ScalarVal)x).val)) return true;
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
                    if (IsTrue(ts.GetData(smpl, t)))
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

        public static IVariable Equals(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            //hmm, comparing two 1x1 matrices will fail
            IVariable rv = Globals.scalarVal0;            
            if (x.Type() == EVariableType.Val && y.Type() == EVariableType.Val)
            {
                //must return a VAL, not a SERIES
                double d1 = ((ScalarVal)x).val; double d2 = ((ScalarVal)y).val;
                if (G.isNumericalError(d1) && G.isNumericalError(d2))
                {
                    rv = Globals.scalarVal1;
                }
                else
                {
                    if (d1 == d2) rv = Globals.scalarVal1; ;
                }
            }
            else if (x.Type() == EVariableType.Series || y.Type() == EVariableType.Series)
            {
                CheckFreqAndCreateSeries(x, y);  //checks freqs
                Series rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                rv = rv_series;
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    //if x or y does not have frequency corresponding to t, we will get an error here
                    if (x.GetVal(t) == y.GetVal(t)) rv_series.SetData(t, 1d);
                    else rv_series.SetData(t, 0d);  //else it would be missing
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
                G.Writeln2("*** ERROR: Variable types " + G.GetTypeString(x) + " and " + G.GetTypeString(x) + " do not match for '==' compare");
                throw new GekkoException();
            }
            return rv;
        }

        private static void CheckFreqAndCreateSeries(IVariable x, IVariable y)
        {
            EFreq freq = EFreq.Annual;
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

        public static bool LargerThanOrEqual(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            bool rv = false;
            if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (O.ConvertToDate(x).LargerThanOrEqual(O.ConvertToDate(y))) rv = true;
            }
            else if ((x.Type() == EVariableType.Series || x.Type() == EVariableType.Val) && (y.Type() == EVariableType.Series || y.Type() == EVariableType.Val))
            {
                //if (x.GetValOLD(smpl) >= y.GetValOLD(smpl)) rv = true;  // //#8097534320985
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types do not match for '>=' compare");
                throw new GekkoException();
            }
            return rv;
        }

        public static bool StrictlyLargerThan(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            bool rv = false;
            if (x.Type() == EVariableType.Date && y.Type() == EVariableType.Date)
            {
                if (O.ConvertToDate(x).StrictlyLargerThan(O.ConvertToDate(y))) rv = true;
            }
            else if ((x.Type() == EVariableType.Series || x.Type() == EVariableType.Val) && (y.Type() == EVariableType.Series || y.Type() == EVariableType.Val))
            {
                //if (x.GetValOLD(smpl) > y.GetValOLD(smpl)) rv = true;   //#8097534320985
            }
            else
            {
                G.Writeln();
                G.Writeln2("*** ERROR: Variable types do not match for '>' compare");
                throw new GekkoException();
            }
            return rv;
        }
                

        public static Series CreateTimeSeriesFromVal(GekkoSmpl smpl, double d)
        {
            Series tsl = new Series(ESeriesType.Light, smpl.t0, smpl.t3); //will have small dataarray            
            for (int i = 0; i < tsl.data.dataArray.Length; i++)
            {
                tsl.data.dataArray[i] = d;
            }
            return tsl;
        }

        public static void PrepareUfunction(int number, string name)
        {
            if (number > 10)
            {
                G.Writeln2("*** ERROR: More than 10 user function arguments is not allowed at the moment.");
                G.Writeln("           You may consider using a MAP argument to work around this restriction.", Color.Red);
                throw new GekkoException();
            }
            if (Globals.gekkoInbuiltFunctions.ContainsKey(name))
            {
                G.Writeln2("*** ERROR: Loading of user function '" + name + "' failed, since this is also the name of an");
                G.Writeln("           in-built Gekko function. Please rename your user function.", Color.Red);
                throw new GekkoException();
            }
            if (number == 0)
            {
                if (Globals.ufunctions0.ContainsKey(name)) Globals.ufunctions0.Remove(name);
            }
            else if (number == 1)
            {
                if (Globals.ufunctions1.ContainsKey(name)) Globals.ufunctions1.Remove(name);
            }
            else if (number == 2)
            {
                if (Globals.ufunctions2.ContainsKey(name)) Globals.ufunctions2.Remove(name);
            }
            else if (number == 3)
            {
                if (Globals.ufunctions3.ContainsKey(name)) Globals.ufunctions3.Remove(name);
            }
            else if (number == 4)
            {
                if (Globals.ufunctions4.ContainsKey(name)) Globals.ufunctions4.Remove(name);
            }
            else if (number == 5)
            {
                if (Globals.ufunctions5.ContainsKey(name)) Globals.ufunctions5.Remove(name);
            }
            else if (number == 6)
            {
                if (Globals.ufunctions6.ContainsKey(name)) Globals.ufunctions6.Remove(name);
            }
            else if (number == 7)
            {
                if (Globals.ufunctions7.ContainsKey(name)) Globals.ufunctions7.Remove(name);
            }
            else if (number == 8)
            {
                if (Globals.ufunctions8.ContainsKey(name)) Globals.ufunctions8.Remove(name);
            }
            else if (number == 9)
            {
                if (Globals.ufunctions9.ContainsKey(name)) Globals.ufunctions9.Remove(name);
            }
            else if (number == 10)
            {
                if (Globals.ufunctions10.ContainsKey(name)) Globals.ufunctions10.Remove(name);
            }
        }

        // USER FUNCTION STUFF START
        // USER FUNCTION STUFF START
        // USER FUNCTION STUFF START
        // USER FUNCTION STUFF START ------------------------------------------------------------------------------------------
        // USER FUNCTION STUFF START
        // USER FUNCTION STUFF START
        // USER FUNCTION STUFF START

        public static Func<GekkoSmpl, P, IVariable> FunctionLookup0(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, IVariable> rv = null;
            Globals.ufunctions0.TryGetValue(name, out rv);
            if (rv == null)
            {
                G.Writeln2("*** ERROR: Cannot find user function '" + name + "()' with 0 arguments");
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, IVariable, IVariable> FunctionLookup1(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, IVariable, IVariable> rv = null;
            Globals.ufunctions1.TryGetValue(name, out rv);
            if (rv == null)
            {
                G.Writeln2("*** ERROR: Cannot find user function '" + name + "()' with 1 argument");
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, IVariable, IVariable, IVariable> FunctionLookup2(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, IVariable, IVariable, IVariable> rv = null;
            Globals.ufunctions2.TryGetValue(name, out rv);
            if (rv == null)
            {
                G.Writeln2("*** ERROR: Cannot find user function '" + name + "()' with 2 arguments");
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable> FunctionLookup3(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable> rv = null;
            Globals.ufunctions3.TryGetValue(name, out rv);
            if (rv == null)
            {
                G.Writeln2("*** ERROR: Cannot find user function '" + name + "()' with 3 arguments");
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable> FunctionLookup4(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable> rv = null;
            Globals.ufunctions4.TryGetValue(name, out rv);
            if (rv == null)
            {
                G.Writeln2("*** ERROR: Cannot find user function '" + name + "()' with 4 arguments");
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable> FunctionLookup5(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable> rv = null;
            Globals.ufunctions5.TryGetValue(name, out rv);
            if (rv == null)
            {
                G.Writeln2("*** ERROR: Cannot find user function '" + name + "()' with 5 arguments");
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable> FunctionLookup6(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable> rv = null;
            Globals.ufunctions6.TryGetValue(name, out rv);
            if (rv == null)
            {
                G.Writeln2("*** ERROR: Cannot find user function '" + name + "()' with 6 arguments");
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable> FunctionLookup7(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable> rv = null;
            Globals.ufunctions7.TryGetValue(name, out rv);
            if (rv == null)
            {
                G.Writeln2("*** ERROR: Cannot find user function '" + name + "()' with 7 arguments");
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable> FunctionLookup8(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable> rv = null;
            Globals.ufunctions8.TryGetValue(name, out rv);
            if (rv == null)
            {
                G.Writeln2("*** ERROR: Cannot find user function '" + name + "()' with 8 arguments");
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable> FunctionLookup9(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable> rv = null;
            Globals.ufunctions9.TryGetValue(name, out rv);
            if (rv == null)
            {
                G.Writeln2("*** ERROR: Cannot find user function '" + name + "()' with 9 arguments");
                throw new GekkoException();
            }
            return rv;
        }

        public static Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable> FunctionLookup10(string name)
        {
            //NOTE: the number of args is hardcoded two places below
            Func<GekkoSmpl, P, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable, IVariable> rv = null;
            Globals.ufunctions10.TryGetValue(name, out rv);
            if (rv == null)
            {
                G.Writeln2("*** ERROR: Cannot find user function '" + name + "()' with 10 arguments");
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
            for (int i = 0; i < tsl.data.dataArray.Length; i++)
            {
                tsl.data.dataArray[i] = m.data[i, 0];
            }
            return tsl;
        }

        public static IVariable ListContains(IVariable x, IVariable y)
        {

            if (x.Type() != EVariableType.List || y.Type() != EVariableType.String)
            {
                G.Writeln2("*** ERROR: Expected syntax like ... $ #a['b'], with list and string");
                throw new GekkoException();
            }
            List ml = (List)x;
            ScalarString ss = (ScalarString)y;

            bool b = false;
            foreach (IVariable iv in ml.list)
            {
                string s = O.ConvertToString(iv);
                if(G.Equal(ss.string2,s))
                {
                    b = true;
                    break;
                }
            }
            if (b) return Globals.scalarVal1;
            else return Globals.scalarVal0;

        }

        // =================================== end comparisons ==================================        

        public static string SubstituteScalarsAndLists(string label, bool reportError)
        {
            return label;
        }
        
        public static List<string> SearchWildcard(IVariable a)
        {
            string s = O.ConvertToString(a);
            List<string> l = new List<string>();
            l = Program.MatchWildcardInDatabank(s, Program.databanks.GetFirst());            
            return l;
        }

        public static Series IndirectionHelper(GekkoSmpl smpl, string variable)
        {
            //In that case, we are inside a GENR/PRT implicit time loop                        
            //Code below implicitly calls Program.ExtractBankAndRest and Program.FindOrCreateTimeseries()
            //So stuff in banks down the list will be found in data mode
            Series ats = O.GetTimeSeries(smpl, variable, 0);            
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
                //bool didTransform = false;
                //a = MaybeStringify(a, dollarStringify);
                //a = MaybeTransform(ref didTransform, a, transformationAllowed);
                return a;
            }
            else
            {
                G.Writeln2("*** ERROR: Memory variable '" + Globals.symbolScalar + name + "' was not found");
                throw new GekkoException();
            }
        }

        public static GekkoTime GetDate(GekkoTime x)
        {
            //used for avgt() or sumt() without period indication
            return x;
        }        

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

        public static List<IVariable> ConvertToList(IVariable a)
        {
            return a.ConvertToList();
        }

        public static List<string> GetStringList(IVariable a)
        {
            if (a.Type() == EVariableType.String)
            {
                List<string> mm = new List<string>();
                mm.Add(a.ConvertToString());
                return mm;
            }
            else if (a.Type() == EVariableType.List)
            {
                List<IVariable> m = a.ConvertToList();
                List<string> mm = new List<string>();
                foreach (IVariable iv in m)
                {
                    string s = O.ConvertToString(iv);
                    mm.Add(s);
                }
                return mm;
            }
            else
            {
                G.Writeln2("*** ERROR: input must b a string or list of strings");
                throw new GekkoException();
            }

        }

        public static Matrix GetMatrixFromString(IVariable name)
        {
            string name2 = name.ConvertToString();
            IVariable lhs = null;            
            if (Program.scalars.TryGetValue(Globals.symbolCollection + name2, out lhs))
            {
                //Scalar is already existing                
                if (lhs.Type() == EVariableType.Matrix)
                {
                    //fine
                }
                else
                {
                    G.Writeln2("*** ERROR: " + Globals.symbolCollection + name2 + " is not a matrix");
                    throw new GekkoException();
                }
            }
            else
            {
                G.Writeln2("*** ERROR: " + Globals.symbolCollection + name2 + " could not be found");
                throw new GekkoException();
            }
            return (Matrix)lhs;
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
            if (Program.options.freq == EFreq.Quarterly) lag = Globals.freqQSubperiods;
            else if (Program.options.freq == EFreq.Monthly) lag = Globals.freqMSubperiods;
            return lag;
        }

        public static IVariable HandleSummations(string type, double[] storage)
        {
            //i1 and i2 are often not used
            double data = double.NaN;
            switch(type)
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

       
        public static IVariable IvConvertTo(EVariableType type, IVariable a)
        {
            switch (type)
            {
                case EVariableType.Val:
                    {
                        if (a.Type() == EVariableType.Val) return a;
                        else return new ScalarVal(a.ConvertToVal());
                    }
                    break;
                case EVariableType.String:
                    {
                        if (a.Type() == EVariableType.String) return a;
                        else return new ScalarString(a.ConvertToString());
                    }
                    break;
                case EVariableType.Date:
                    {
                        if (a.Type() == EVariableType.Date) return a;
                        else return new ScalarDate(a.ConvertToDate(GetDateChoices.Strict));
                    }
                    break;
                case EVariableType.Series:
                    {
                        if (a.Type() == EVariableType.Series) return a;
                        else
                        {
                            G.Writeln2("*** ERROR: Could not transform " + G.GetTypeString(a) + " into SERIES");
                            throw new GekkoException();
                        }
                    }
                    break;
                case EVariableType.List:
                    {
                        if (a.Type() == EVariableType.List) return a;
                        else return new List(a.ConvertToList());
                    }
                    break;
                case EVariableType.Matrix:
                    {
                        if (a.Type() == EVariableType.Matrix) return a;
                        else
                        {
                            G.Writeln2("*** ERROR: Could not transform " + G.GetTypeString(a) + " into MATRIX");
                            throw new GekkoException();
                        }
                    }
                    break;
                case EVariableType.Map:
                    {
                        if (a.Type() == EVariableType.Map) return a;
                        else
                        {
                            G.Writeln2("*** ERROR: Could not transform " + G.GetTypeString(a) + " into MAP");
                            throw new GekkoException();
                        }
                    }
                    break;
                case EVariableType.Var:
                    {
                        return a;
                    }
                    break;
                default: throw new GekkoException();  //should not be possible
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

        public static Series GetTimeSeries(IVariable a)
        {
            if (a.Type() == EVariableType.Series)
            {
                return (Series)a;
            }
            else if (a.Type() == EVariableType.String)
            {
                ScalarString ss = (ScalarString)a;
                if (false)
                {
                    Series ts = O.FindTimeSeries(ss.string2, 1);
                    return ts;
                }
                else
                {
                    G.Writeln2("*** ERROR: Cannot convert STRING into SERIES, use a NAME-scalar or {}-braces");
                    throw new GekkoException();
                }
            }
            else
            {
                G.Writeln2("*** ERROR: Cannot convert variable of " + G.GetTypeString(a) + " type into SERIES");
                throw new GekkoException();
            }
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
            public string opt_array = null;
            public string opt_ser = null;
            public string type = null;  //read or import
            
            public P p = null;
            public void Exe()
            {
                ReadOpenMulbkHelper hlp = new ReadOpenMulbkHelper();  //This is a bit confusing, using an old object to store the stuff.
                hlp.t1 = this.t1;
                hlp.t2 = this.t2;
                
                bool isRead = false; if (G.Equal(this.type, "read")) isRead = true;

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
                if (G.Equal(this.opt_ser, "yes")) hlp.Type = EDataFormat.Ser;
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

                if (G.Equal(Program.options.interface_mode, "data"))
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
                    if (readTo == "*")
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

                hlp.array = this.opt_array;

                Program.OpenOrRead(wipeDatabankBeforeInsertingData, hlp, open, readInfos);
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
                    if (Program.model != null && (G.Equal(Program.options.interface_mode, "sim") || G.Equal(Program.options.interface_mode, "mixed")))
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
                    if (G.GetFreqFromName(s) != Program.options.freq) continue;
                    string s2 = G.RemoveFreqFromName(s);
                    if (!Program.model.varsAType.ContainsKey(s2)) onlyDatabankNotModel.Add(s2);
                }
                foreach (string s in Program.model.varsAType.Keys)
                {                    
                    if (!Program.databanks.GetFirst().ContainsVariable(s)) onlyModelNotDatabank.Add(s);
                }
                if (G.Equal(Program.options.interface_mode, "sim"))
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
                G.Writeln("Cleared reference databank ('" + Program.databanks.GetRef().name + "') and copied " + number + " variables from first-position ('" + Program.databanks.GetFirst().name + "') to reference ('" + Program.databanks.GetRef().name + "') databank");
                if (G.Equal(Program.options.interface_mode, "data"))
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
                    G.Writeln2("*** ERROR: VAL " + Globals.symbolScalar.ToString() + s + " was not found");
                    throw new GekkoException();
                }
                string ss = a.GetValOLD(null).ToString();
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
                if (a == null || a.Type() != EVariableType.String)
                {
                    G.Writeln2("*** ERROR: STRING "+ Globals.symbolScalar.ToString()  + s + " was not found");
                    throw new GekkoException();
                }
                G.Writeln2("STRING " + s + " = '" + a.ConvertToString() + "'");
            }
            public static void Q()
            {
                Program.Mem("string");
            }
        }

        public class Matrix2
        {
            public List<string> opt_rownames = null;
            public List<string> opt_colnames = null;

            public void Exe(IVariable name, IVariable rhs)
            {
                if (rhs == null && opt_rownames == null && opt_colnames == null)
                {
                    G.Writeln2("*** ERROR: You must indicate either <rownames=...> or <colnames=...>");
                    throw new GekkoException();
                }

                Matrix value = null;
                if (rhs != null) value = O.ConvertToMatrix(rhs);

                if (value != null)
                {
                    if (opt_rownames != null) value.rownames = opt_rownames;
                    if (opt_colnames != null) value.colnames = opt_colnames;
                }

                IVariable lhs = null;

                string name2 = name.ConvertToString();
                if (Program.scalars.TryGetValue(Globals.symbolCollection + name2, out lhs))
                {
                    //Matrix is already existing, may inherit the row/columnames               
                    if (lhs.Type() == EVariableType.Matrix)
                    {
                        if (value == null)
                        {
                            //do not touch the .data
                        }
                        else
                        {
                            //Already existing lhs is a MATRIX, inject into it. Injecting is faster than recreating an object in the dictionary
                            ((Matrix)lhs).data = value.data;                            
                        }
                        if (opt_rownames != null) ((Matrix)lhs).rownames = opt_rownames;
                        if (opt_colnames != null) ((Matrix)lhs).colnames = opt_colnames;
                    }
                    else
                    {
                        if (value == null)
                        {
                            G.Writeln2("*** ERROR: No matrix with name '" + Globals.symbolCollection + name2 + "' exists");
                            throw new GekkoException();
                        }
                        //The object has to die and be recreated, since it is of a wrong type.                                
                        Program.scalars.Remove(Globals.symbolCollection + name2);
                        //lhs = new ScalarDate(value);
                        Program.scalars.Add(Globals.symbolCollection + name2, value);
                    }
                }
                else
                {
                    if (value == null)
                    {
                        G.Writeln2("*** ERROR: No matrix with name '" + Globals.symbolCollection + name2 + "' exists");
                        throw new GekkoException();
                    }
                    //Scalar does not exist beforehand            
                    //lhs = new ScalarDate(value);
                    Program.scalars.Add(Globals.symbolCollection + name2, value);
                }
                //return (Matrix)lhs;
            }

            public static void Q(string s)
            {                
                Show ss = new Show();
                IVariable iv = null; Program.scalars.TryGetValue(Globals.symbolCollection.ToString() + s, out iv);
                if (iv == null)
                {
                    G.Writeln2("Matrix " + Globals.symbolCollection.ToString() + s + " not found");
                    return;
                }
                ss.input = iv;
                ss.label = Globals.symbolCollection.ToString() + s;
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
                if (a == null || a.Type() != EVariableType.String)
                {
                    G.Writeln2("*** ERROR: NAME " + Globals.symbolScalar.ToString() + s + " was not found");
                }
                G.Writeln2("NAME " + s + " = '" + a.ConvertToString() + "'");
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
                    G.Writeln2("*** ERROR: DATE " + Globals.symbolScalar.ToString() + s + " was not found");
                }
                G.Writeln2("DATE " + s + " = " + G.FromDateToString(O.ConvertToDate(a)));
            }
            public static void Q()
            {
                Program.Mem("date");
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

                Series oldSeries = O.FindTimeSeries(this.listItems1[0], 1);
                Series lhs = O.FindTimeSeries(this.listItems0[0], 1);
                                
                Series newSeriesTemp = oldSeries.DeepClone() as Series;  //brand new object, not present in Work (yet)                

                ESmoothTypes type = ESmoothTypes.Spline;  //what is the default in AREMOS??
                if (G.Equal(opt_geometric, "yes")) type = ESmoothTypes.Geometric;
                if (G.Equal(opt_linear, "yes")) type = ESmoothTypes.Linear;
                if (G.Equal(opt_spline, "yes")) type = ESmoothTypes.Spline;
                if (G.Equal(opt_repeat, "yes")) type = ESmoothTypes.Repeat;                

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
                        double data = oldSeries.GetData(null, gt);
                        if (G.isNumericalError(data))
                        {
                            missings.Add(counter);
                            missingsDates.Add(gt);
                            continue;  //ignore this observation
                        }
                        yy.Add(oldSeries.GetData(null, gt));
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
                        double z = oldSeries.GetData(null, gt);
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
                            double z1 = oldSeries.GetData(null, t1);
                            double z2 = oldSeries.GetData(null, t2);
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
                else throw new GekkoException();
                foreach (GekkoTime gt in new GekkoTimeIterator(realStart, realEnd))
                {
                    //This is not terribly efficient, and we could use array copy etc.
                    //And we do create and clone a whole new timeseries (newSeriesTemp).
                    //But it works, and speed is probably not an issue with SMOOTH.
                    lhs.SetData(gt, newSeriesTemp.GetData(null, gt));
                }
                lhs.Stamp();
            }            
        }

        public class Splice
        {
            
            public List<string> listItems0;
            public List<string> listItems1;
            public List<string> listItems2;

            public GekkoTime date = GekkoTime.tNull;
            public void Exe()
            {
                bool useSecondPartLevels = true;  //like aremos

                if (listItems0.Count != 1 || listItems1.Count != 1 || listItems2.Count != 1)
                {
                    G.Writeln2("*** ERROR: SPLICE only supports one variable at a time, not lists (for now)");
                    throw new GekkoException();
                }

                Series ts1 = O.FindTimeSeries(listItems1[0], 1);
                Series ts2 = O.FindTimeSeries(listItems2[0], 1);
                Series ts3 = O.FindTimeSeries(listItems0[0], 1);
                //if (ts3 == null)
                //{
                //    ts3 = new Series(Program.options.freq, lhs);
                //    Program.databanks.GetPrim().AddVariable(ts3);
                //}
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
                    sum1 += ts1.GetData(null, gt);
                    sum2 += ts2.GetData(null, gt);
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
                        ts3.SetData(gt, ts1.GetData(null, gt) / relative);
                    }
                    foreach (GekkoTime gt in new GekkoTimeIterator(t2a, t2b))
                    {
                        ts3.SetData(gt, ts2.GetData(null, gt));
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
                        ts3.SetData(gt, ts1.GetData(null, gt));
                    }
                    foreach (GekkoTime gt in new GekkoTimeIterator(t1b.Add(1), t2b))
                    {
                        ts3.SetData(gt, ts2.GetData(null, gt) * relative);
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
            public string name = null;  //only if '*' is indicated, not used otherwise
            public List<string> listItems = null;
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
                    foreach (string dbName in this.listItems)
                    {
                        if (G.Equal(dbName, Globals.Work) || G.Equal(dbName, Globals.Ref))
                        {
                            G.Writeln2("*** ERROR: Databanks '" + Globals.Work + "' or '" + Globals.Ref + "' cannot be closed (see CLEAR command)");
                            throw new GekkoException();
                        }
                        databanks.Add(dbName);
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
                if (G.Equal(opt_prot, "yes"))
                {
                    G.Writeln2("*** ERROR: OPEN<prot> is obsolete. In Gekko 2.1.1 and onwards, databanks");
                    G.Writeln("           are always opened as 'protected' by default, unless you use", Color.Red);
                    G.Writeln("           OPEN<edit>, or unless you afterwards use the UNLOCK command", Color.Red);
                    G.Writeln("           to make the databank editable.", Color.Red);                    
                    throw new GekkoException();
                }

                if (G.Equal(opt_prim, "yes"))
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
                    hlp.protect = false;  //will override the born true value of the field
                }
                if (G.Equal(opt_ref, "yes"))
                {
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
                
                List<Program.ReadInfo> readInfos = new List<Program.ReadInfo>();
                Program.OpenOrRead(false, hlp, true, readInfos);                

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
            public P p = null;
            public void Exe()
            {
                if (false && Globals.runningOnTTComputer)
                {
                    //Globals.globalPeriodStart = new GekkoTime(EFreq.Annual, 2007, 1);
                    //Globals.globalPeriodEnd = new GekkoTime(EFreq.Annual, 2017, 1);
                    Program.Run("tt.gcm", this.p);
                }
                else Program.Run(this.fileName, this.p);
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
            public string opt_units = null;
            public string opt_stamp = null;            
            public List<string> listItems0 = null;  //left side     
            public void Exe()
            {
                foreach (string s in listItems0)
                {
                    ExtractBankAndRestHelper h = Program.ExtractBankAndRest(s, EExtrackBankAndRest.GetDatabankAndTimeSeries);
                    if (opt_label != null) h.ts.meta.label = opt_label;
                    if (opt_source != null) h.ts.meta.source = opt_source;
                    if (opt_units != null) h.ts.meta.units = opt_units;
                    if (opt_stamp != null) h.ts.meta.stamp = opt_stamp;
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
                string msg = O.ConvertToString(message);
                if (Program.InputBox("Accept", msg, ref value) == DialogResult.OK)
                {
                    string varname = name.ConvertToString();

                    varname = G.AddSigil(varname, this.type);  //see also #980753275

                    if (G.Equal(type, "val"))
                    {                        
                        try
                        {
                            double v = double.Parse(value.Trim());                            
                            Program.databanks.GetFirst().AddIVariableWithOverwrite(varname, new ScalarVal(v));
                            G.Writeln2("VAL " + varname + " = " + v);
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
                            v = Program.StripQuotes(v);
                            Program.databanks.GetFirst().AddIVariableWithOverwrite(varname, new ScalarString(v));
                            G.Writeln2("STRING " + varname + " = '" + v + "'");
                        }
                        catch
                        {
                            G.Writeln2("*** ERROR: Could not convert '" + value + "' into a STRING");
                            throw new GekkoException();
                        }
                    }                    
                    else if (G.Equal(type, "date"))
                    {                        
                        try
                        {
                            GekkoTime gt = G.FromStringToDate(value.Trim());
                            Program.databanks.GetFirst().AddIVariableWithOverwrite(varname, new ScalarDate(gt));
                            G.Writeln2("DATE " + varname + " = " + gt.ToString());
                        }
                        catch
                        {
                            G.Writeln2("*** ERROR: Could not convert '" + value + "' into a DATE");
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
            public IVariable x = null;
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
                        if (localOptionToBank != null) bankName2 = localOptionToBank.name;
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
                    if (localOptionFromBank != null) bankName = localOptionFromBank.name;

                    bool ignoreErrors = false; if (G.Equal(opt_error, "no")) ignoreErrors = true;

                    List<Series> tss = Program.GetTimeSeriesFromStringWildcard(listItems0[i], bankName, ignoreErrors);  //gets these from the 'fromBank', so ExtractBankAndRest() gets called two times, but never mind

                    if (tss.Count == 0 || (tss.Count == 1 && tss[0] == null))  //the first one is if a wildcard does not match at all, and the second is a normal timeseries name is not found, and COPY<error=no> is used.
                    {
                        string s = listItems0[i].Replace(Globals.firstCheatString, "");                        
                        if (ignoreErrors)
                        {
                            errorCounter++;
                            G.Writeln2("+++ NOTE: COPY: Could not find this timeseries: " + s + "");
                            continue;
                        }
                        else
                        {
                            G.Writeln2("*** ERROR: COPY: Could not find this timeseries: " + s + "");
                            throw new GekkoException();
                        }
                    }

                    foreach (Series ts in tss)
                    {
                        if (ts == null) continue;  //for safety, this should not be possible since there is a continue above.

                        //If a wildcard like "COPY a*;" is used, tss may have > 1 elements
                        string newName = null;
                        if (listItems1 == null)
                        {
                            //Stuff like "COPY adbk:fx*;"  (copies to first)                            
                            newName = ts.name;  //same name is used
                        }
                        else
                        {
                            if (isUsingPlaceholder)
                            {
                                //Stuff like "COPY #m TO simbk:a_*;"
                                //here we use item #0 (there can only be 1 such item)
                                ExtractBankAndRestHelper h = Program.ExtractBankAndRest(listItems1[0], EExtrackBankAndRest.GetDatabank);
                                newName = placeholderPrefix + ts.name + placeholderSuffix;
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
                            Series ts2 = toBank.GetVariable(newName);
                            if (ts2 != null)
                            {
                                if (G.Equal(ts.meta.parentDatabank.name, ts2.meta.parentDatabank.name))
                                {
                                    if (G.Equal(ts.name, ts2.name))
                                    {
                                        G.Writeln2("*** ERROR: You are trying to copy the timeseries '" + ts.name + "' from databank '" + ts.meta.parentDatabank.name + "' to itself");
                                        throw new GekkoException();
                                    }
                                }
                            }
                        }

                        if (G.Equal(this.opt_respect, "yes"))
                        {
                            //Truncate time period
                            Series ts2 = toBank.GetVariable(newName);
                            if (ts2 != null)
                            {
                                //Type 1
                                //inject into existing
                                //this is the fastest way, in reality via arraycopy. But beware to check carefully if you
                                //  change anything here.
                                type1++;
                                int index1, index2;
                                double[] values_beware_do_not_alter = ts.GetDataSequenceUnsafePointerReadOnly(out index1, out index2, this.t1, this.t2);
                                ts2.SetDataSequence(t1, t2, values_beware_do_not_alter, index1);
                                ts2.Stamp();  //will get a new date stamp, since it is not cloning here
                            }
                            else
                            {
                                //Type 2
                                type2++;
                                ts2 = ts.DeepClone() as Series;
                                ts2.name = newName;
                                ts2.Truncate(this.t1, this.t2);
                                toBank.AddVariable(ts2);
                            }
                        }
                        else
                        {
                            //No truncate of time period
                            Series ts2 = ts.DeepClone() as Series;  //will inherit the date stamp                          
                            if (listItems1 != null)
                            {
                                ts2.name = newName;
                            }
                            if (toBank.ContainsVariable(newName))
                            {
                                //Type 3
                                //wipe out any existing Series completely, and put in the clone instead
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
                    G.Writeln("+++ WARNING: COPY: Note that " + errorCounter + " variables were not copied");
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
            public List listItems = null;
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
                    List<Series> tss = Program.GetTimeSeriesFromStringWildcard(s);
                    //Now we are sure all the series are from Work
                    foreach (Series ts in tss)
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
                                     
                    List<Series> tss = Program.GetTimeSeriesFromStringWildcard(s1, opt_bank);
                    foreach (Series ts in tss)
                    {
                        //There is probably always only 1 here
                        if (ts.meta.parentDatabank.ContainsVariable(s2))
                        {
                            G.Writeln2("*** ERROR: Databank " + ts.meta.parentDatabank.name + " already contains timeseries '" + s2 + "'");
                            throw new GekkoException();
                        }
                        ts.meta.parentDatabank.RemoveVariable(ts.name);                        
                        ts.name = s2;
                        ts.meta.parentDatabank.AddVariable(ts);
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
                Series tlhs = O.GetTimeSeries(lhs);

                Databank bankName = tlhs.meta.parentDatabank;
                string varName = tlhs.name;

                Series trhs = O.GetTimeSeries(rhs);
                trhs.name = varName;

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
                if (G.Equal(opt_nonmodel, "yes"))
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
            //public List<string> listItems = null;
            public IVariable iv = null;          
            public string searchName = null;
            public string opt_info = null;
            public void Exe()
            {
                if (this.searchName == null)
                {
                    //List<string> m = new List<string>();
                    
                    //foreach (IVariable iv in iv.list)
                    //{
                    //    m.Add(iv.ConvertToString());
                    //}
                    Program.Disp(this.t1, this.t2, null, this);
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
             

        public class ListDef
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

                if (G.Equal(listSort, "yes"))  //listSort = null is okay in G.equal()
                {
                    //TODO: What about strings starting with "-"?
                    //TODO: What about æøå strings, what happens??
                    this.listItems.Sort(StringComparer.OrdinalIgnoreCase);
                }

                if (G.Equal(listTrim, "yes"))  //listTrim = null is okay in G.equal()
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


        public class SeriesDef
        {
            public GekkoTime t1 = Globals.globalPeriodStart;  //default, if not explicitely set
            public GekkoTime t2 = Globals.globalPeriodEnd;    //default, if not explicitely set
            public string lhsFunction = null;
            public Series lhs = null;
            public Series rhs = null;
            public string meta = null;
            public P p = null;
            public void Exe()
            {
                if (true)

                {
                    //a full copy of the data
                    //int i1 = -12345;
                    //int i2 = -12345;
                    //double[] dataPointer = ts.GetDataSequence(out i1, out i2, smpl.t1, smpl.t2);
                    //this.lhs.storage = new double[i2 - i1 + 1];
                    //Array.Copy(dataPointer, i1, storage, 0, (i2 - i1 + 1));
                    //this.anchorPeriodPositionInArray = 0;
                    //this.anchorPeriod = smpl.t1;

                    int i = Series.FromGekkoTimeToArrayIndex(this.t1, new GekkoTime(this.rhs.freq, this.rhs.data.anchorPeriod.sub, this.rhs.data.anchorPeriod.super), this.rhs.GetAnchorPeriodPositionInArray());
                    int n = GekkoTime.Observations(this.t1, this.t2);

                    //TODO TODO TODO, should not be possible
                    if (i < 0 || i >= this.rhs.data.dataArray.Length)
                    {
                        G.Writeln2("*** ERROR: Sample error #9876201872");
                        throw new GekkoException();
                    }

                    int index1; int index2;
                    double[] dataArray = lhs.GetDataSequenceUnsafePointerAlter(out index1, out index2, this.t1, this.t2); //Method will resize the double[] array if it is too small
                    if (index2 - index1 + 1 != n)
                    {
                        G.Writeln2("*** ERROR: Sample error #9376201872");
                        throw new GekkoException();
                    }
                    Array.Copy(this.rhs.data.dataArray, i, dataArray, index1, n);
                }

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
                bool addbank = false; if (G.Equal(this.opt_addbank, "yes")) addbank = true;
                List<string> names = new List<string>();

                if (G.Equal(this.opt_bank, "yes"))
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

                if (!G.Equal(this.opt_mute, "yes"))
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
            public GekkoTime date1 = GekkoTime.tNull;
            public GekkoTime date2 = GekkoTime.tNull;
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
                    List<Series> tss = Program.GetTimeSeriesFromStringWildcard(this.listItems[i], opt_bank);
                    foreach (Series ts in tss)
                    {
                        if (ts.meta.parentDatabank.protect) Program.ProtectError("You cannot change/add a timeseries in a non-editable databank (" + ts.meta.parentDatabank + ")");
                        
                        GekkoTime ddate1 = date1;
                        GekkoTime ddate2 = date2;

                        if (date1.freq == EFreq.Annual && (ts.freq == EFreq.Quarterly || ts.freq == EFreq.Monthly))
                        {
                            //if a year is used for a quarterly series, q1-q4 is used.
                            ddate1 = new GekkoTime(ts.freq, date1.super, 1);
                            int end = -12345;
                            if (ts.freq == EFreq.Quarterly)
                            {
                                end = Globals.freqQSubperiods;
                            }
                            else if (ts.freq == EFreq.Monthly)
                            {
                                end = Globals.freqMSubperiods;
                            }
                            else
                            {
                                G.Writeln2("*** ERROR: freq error #903853245");
                                throw new GekkoException();
                            }
                            ddate2 = new GekkoTime(ts.freq, date1.super, end);
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
                            sum += ts.GetData(null, t);
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
                        if (opt_prefix != null)
                        {
                            tsNew = ts.DeepClone() as Series;
                            tsNew.name = opt_prefix + ts.name;
                            if (ts.meta.parentDatabank == null)
                            {
                                G.Writeln2("*** ERROR: Internal error #8796357826435");
                                throw new GekkoException();
                            }


                            if (ts.meta.parentDatabank.ContainsVariable(tsNew.name))
                            {
                                ts.meta.parentDatabank.RemoveVariable(tsNew.name);
                                counter++;
                            }
                            ts.meta.parentDatabank.AddVariable(tsNew);
                        }
                        else tsNew = ts;
                        
                        double[] data = tsNew.data.dataArray;
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
            //public Series lhs = null;
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
                
            }
        }

        public class Time
        {
            public GekkoTime t1 = GekkoTime.tNull;
            public GekkoTime t2 = GekkoTime.tNull;
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
                    Program.GetTable(this.name).CurRow.SetValues(this.col, this.prtElements[0].variable[0] as Series, this.prtElements[0].variable[1] as Series, null, this.t1, this.t2, Globals.tableOption, this.printcode, this.scale, this.format);
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
            public string emfName = null;  //name of produced emf file if PLOT
                        
            public int printCsCounter = -12345;

            public string guiGraphPrintCode = null;  //clicking in the PLOT window
                  
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
            public List<IVariable> labelHelper = null;  //comes from smpl.labelHelper. For PRT<m> for instance, there will be 2*n elements, where the n's are identical. This is because the expression is run 2 times for <m>, <q> and the like.
                        
            public long counter = -12345;

            public void Exe()
            {

                Program.OPrint(this);
                if (G.Equal(prtType, "mulprt") && G.Equal(Program.options.interface_mode, "data"))
                {
                    G.Writeln2("+++ WARNING: MULPRT is not intended for data mode, please use PRT (cf. the MODE command).");
                }

            }
            
            public static List<int> GetBankNumbers(string tableOrGraphGlobalPrintCode, List<string> printCodes)
            {               
                
                List<int> rv = new List<int>();

                //TODO: CLEAN THIS UP!!                                                
                if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_n))
                {
                    rv = new List<int>(); rv.Add(0);
                }
                else if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_d))
                {
                    rv = new List<int>(); rv.Add(0);
                }
                else if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_p))
                {
                    rv = new List<int>(); rv.Add(0);
                }
                else if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_dp))
                {
                    rv = new List<int>(); rv.Add(0);
                }
                else if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_r))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_rn))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_rd))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_rp))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_rdp))
                {
                    rv = new List<int>(); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_m))
                {
                    rv = new List<int>(); rv.Add(0); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_q))
                {
                    rv = new List<int>(); rv.Add(0); rv.Add(1);
                }
                else if (G.Equal(tableOrGraphGlobalPrintCode, Globals.printCode_mp))
                {
                    rv = new List<int>(); rv.Add(0); rv.Add(1);
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
                //public List<SubElement> subElements = null;

                //public IVariable tsWork = null;
                //public IVariable tsBase = null;
                public IVariable[] variable = new IVariable[2];  //first and ref
                public string label = null;
                public string[] label2 = null;
                //public string originalLabel = null;
                public string endoExoIndicator = null;
                //-- layout
                public List<OptString> printCodes = new List<OptString>();

                public List<string> printCodesFinal = null;
                public string printCodeFinal = null;

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

                public double min = double.MaxValue;
                public double max = double.MinValue;
                                
            }

            
        }


        public class PrtContainer
        {
            //Items that are unfolded via lists
            
            public IVariable[] variable = new IVariable[2];

            public string printCode = null;
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
            public IVariable lhs = null;
            public IVariable rhs = null;  
            //public List<O.Prt.Element> prtElements = new List<O.Prt.Element>();
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
                if (G.Equal(Program.options.interface_mode, "data"))
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
            public string opt_info = null;
            public string opt_gms = null;
            public P p = null;
            public void Exe()
            {                
                Program.Model(this);
                if (G.Equal(Program.options.interface_mode, "data"))
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
                    IVariable iv = null; Program.scalars.TryGetValue(Globals.symbolCollection + s, out iv);
                    if (iv != null && iv.Type() == EVariableType.Matrix)
                    {
                        Matrix m = (Matrix)iv;
                        string xxx = Program.MatrixFromGekkoToR<double>(s, m.data);
                        all += xxx + G.NL;
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Could not find matrix " + Globals.symbolCollection + s);
                        throw new GekkoException();
                    }
                }
                if (this.opt_target == null)
                {
                    //insert at top
                    List<string> l2 = new List<string>();
                    l2.Add(all);
                    if (Globals.r_fileContent != null) l2.AddRange(Globals.r_fileContent);
                    Globals.r_fileContent = l2;
                }
                else
                {
                    bool hit = false;
                    List<string> l2 = new List<string>();
                    if (Globals.r_fileContent == null)
                    {
                        G.Writeln2("*** ERROR: the R_FILE is empty");
                        throw new GekkoException();
                    }
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
                                    if(G.Equal(this.opt_target,foundBlock)) {
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
            public GekkoTime t1 = GekkoTime.tNull; //default, if not explicitely set
            public GekkoTime t2 = GekkoTime.tNull; //default, if not explicitely set
            public string fileName = null;
            public IVariable list = null;
            public List<string> listItems = null;
            
            public string opt_tsd = null;
            public string opt_tsdx = null;
            public string opt_gbk = null;
            public string opt_csv = null;
            public string opt_prn = null;
            public string opt_r = null;
            public string opt_tsp = null;
            public string opt_xls = null;
            public string opt_xlsx = null;
            public string opt_gdx = null;
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
                if (G.Equal(opt_gekko18, "yes"))
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
                else if (G.Equal(opt_aremos, "yes"))
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
