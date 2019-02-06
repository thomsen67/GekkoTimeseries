using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gekko
{

    public class Functions
    {
        //NOTE:
        //NOTE: All function names should be with lower-case only!!
        //NOTE: All function helpers should be PRIVATE!!
        //        --> or if the function must have access from CS code,
        //            call it HELPER_methodname().
        //NOTE:

        public enum EElementByElementType {
            Times,
            Divide
        }

        public enum ESumDim {
            Rows,
            Cols
        }

        public enum ESumType {
            Min,
            Max,
            Sum,
            Avg
        }

        // ===========================================================================================================================
        // ========================= functions to manipulate dates start =============================================================
        // ===========================================================================================================================

        public static IVariable date(GekkoSmpl smpl, IVariable iv1, IVariable iv2, IVariable iv3)
        {
            if (iv1.Type() == EVariableType.Val)
            {
                int yy = O.ConvertToInt(iv1);
                string ff = O.ConvertToString(iv2);
                int ss = O.ConvertToInt(iv3);
                GekkoTime gt = new GekkoTime(G.GetFreq(ff), yy, ss);
                return new ScalarDate(gt);
            }
            else
            {
                GekkoTime dd = O.ConvertToDate(iv1);
                string ff = O.ConvertToString(iv2);
                string startEnd2 = O.ConvertToString(iv3);
                GekkoTime gt = Program.ConvertFreq(dd, G.GetFreq(ff), startEnd2);
                return new ScalarDate(gt);
            }
        }

        public static IVariable getyear(GekkoSmpl smpl, IVariable ths)
        {
            if (ths.Type() != EVariableType.Date)
            {
                G.Writeln2("*** ERROR: getyear() expects date input");
                throw new GekkoException();
            }
            
            GekkoTime gt = (ths as ScalarDate).date;
            return new ScalarVal(gt.super);
        }

        public static IVariable getsubper(GekkoSmpl smpl, IVariable ths)
        {
            if (ths.Type() != EVariableType.Date)
            {
                G.Writeln2("*** ERROR: getsubper() expects date input");
                throw new GekkoException();
            }

            GekkoTime gt = (ths as ScalarDate).date;
            return new ScalarVal(gt.sub);
        }

        public static IVariable getquarter(GekkoSmpl smpl, IVariable ths)
        {
            if (ths.Type() != EVariableType.Date)
            {
                G.Writeln2("*** ERROR: getyear() expects date input");
                throw new GekkoException();
            }

            GekkoTime gt = (ths as ScalarDate).date;
            if (gt.freq != EFreq.Q)
            {
                G.Writeln2("*** ERROR: getquarter() expects quarterly date");
                throw new GekkoException();
            }
            return new ScalarVal(gt.sub);
        }

        public static IVariable getmonth(GekkoSmpl smpl, IVariable ths)
        {
            if (ths.Type() != EVariableType.Date)
            {
                G.Writeln2("*** ERROR: getmonth() expects date input");
                throw new GekkoException();
            }

            GekkoTime gt = (ths as ScalarDate).date;
            if (gt.freq != EFreq.M)
            {
                G.Writeln2("*** ERROR: getmonth() expects monthly date");
                throw new GekkoException();
            }
            return new ScalarVal(gt.sub);
        }                

        // ===========================================================================================================================
        // ========================= functions to manipulate dates end ===============================================================
        // ===========================================================================================================================





        // ===========================================================================================================================
        // ========================= functions to manipulate bankvarnames with indexes start =========================================
        // ===========================================================================================================================


        //See equivalent method in G.cs
        public static IVariable getbank(GekkoSmpl smpl, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(getbank(smpl, item));
                return rv;
            }
            else
            {
                string ss = G.Chop_GetBank(O.ConvertToString(ths));
                return new ScalarString(ss);
            }
        }


        //See equivalent method in G.cs
        public static IVariable getname(GekkoSmpl smpl, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(getname(smpl, item));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_GetName(O.ConvertToString(ths)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable getfreq(GekkoSmpl smpl, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(getfreq(smpl, item));
                return rv;
            }
            else if (ths.Type() == EVariableType.Date)
            {
                GekkoTime gt = (ths as ScalarDate).date;
                return new ScalarString(G.GetFreq(gt.freq));
            }
            else
            {
                return new ScalarString(G.Chop_GetFreq(O.ConvertToString(ths)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable getnameandfreq(GekkoSmpl smpl, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(getnameandfreq(smpl, item));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_GetNameAndFreq(O.ConvertToString(ths)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable getindex(GekkoSmpl smpl, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(getindex(smpl, item));
                return rv;
            }
            else
            {
                return new List(G.Chop_GetIndex(O.ConvertToString(ths)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable getfullname(GekkoSmpl smpl, IVariable ivbank, IVariable ivname, IVariable ivfreq)
        {
            return getfullname(smpl, ivbank, ivname, ivfreq, null);
        }

        //See equivalent method in G.cs
        public static IVariable getfullname(GekkoSmpl smpl, IVariable ivbank, IVariable ivname, IVariable ivfreq, IVariable ivindex)
        {
            string bank = O.ConvertToString(ivbank);
            string name = O.ConvertToString(ivname);
            string freq = O.ConvertToString(ivfreq);
            string[] index = null;
            if (ivindex != null) index = Program.GetListOfStringsFromListOfIvariables(O.ConvertToList(ivindex).ToArray());
            string s = G.Chop_GetFullName(bank, name, freq, index);
            return new ScalarString(s);
        }

        //See equivalent method in G.cs
        public static IVariable addbank(GekkoSmpl smpl, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(addbank(smpl, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_AddBank(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable setbank(GekkoSmpl smpl, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(setbank(smpl, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_SetBank(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable removebank(GekkoSmpl smpl, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(removebank(smpl, item));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_RemoveBank(O.ConvertToString(ths)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable removebank(GekkoSmpl smpl, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(removebank(smpl, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_RemoveBank(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable replacebank(GekkoSmpl smpl, IVariable ths, IVariable x2, IVariable x3)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(replacebank(smpl, item, x2, x3));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_ReplaceBank(O.ConvertToString(ths), O.ConvertToString(x2), O.ConvertToString(x3)));
            }
        }



        //See equivalent method in G.cs
        public static IVariable addfreq(GekkoSmpl smpl, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(addfreq(smpl, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_AddFreq(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable setfreq(GekkoSmpl smpl, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(setfreq(smpl, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_SetFreq(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable removefreq(GekkoSmpl smpl, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(removefreq(smpl, item));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_RemoveFreq(O.ConvertToString(ths)));
            }
        }

        public static IVariable removeindex(GekkoSmpl smpl, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(removeindex(smpl, item));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_RemoveIndex(O.ConvertToString(ths)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable removefreq(GekkoSmpl smpl, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(removefreq(smpl, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_RemoveFreq(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable replacefreq(GekkoSmpl smpl, IVariable ths, IVariable x2, IVariable x3)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(replacefreq(smpl, item, x2, x3));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_ReplaceFreq(O.ConvertToString(ths), O.ConvertToString(x2), O.ConvertToString(x3)));
            }
        }

        //See equivalent method in G.cs
        public static IVariable setname(GekkoSmpl smpl, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(setname(smpl, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_SetName(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }

        public static IVariable setnameprefix(GekkoSmpl smpl, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(setnameprefix(smpl, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_SetNamePrefix(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }

        public static IVariable setnamesuffix(GekkoSmpl smpl, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(setnamesuffix(smpl, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_SetNameSuffix(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        // ===========================================================================================================================
        // ========================= functions to manipulate bankvarnames with indexes end ===========================================
        // ===========================================================================================================================


        public static IVariable rotate(GekkoSmpl smpl, IVariable x1, IVariable dim)
        {
            int iDim = O.ConvertToInt(dim);

            Series ts = x1 as Series;
            if (ts == null || ts.type != ESeriesType.ArraySuper)
            {
                G.Writeln2("*** ERROR: You must use a array-timeseries variable");
                throw new GekkoException();
            }

            if (iDim > ts.dimensions || iDim < 1)
            {
                G.Writeln2("*** ERROR: Array-series does not have a dimension #" + iDim);
                throw new GekkoException();
            }

            Series tsRotated = new Series(EFreq.U, G.Chop_SetFreq(ts.name, G.GetFreq(EFreq.U)));
            tsRotated.meta.label = ts.meta.label;
            tsRotated.SetArrayTimeseries(ts.dimensions + 1, true);

            foreach (KeyValuePair<MapMultidimItem, IVariable> kvp in ts.dimensionsStorage.storage)
            {
                //foreach array-subseries, for instance x[#age] over the ages 18-100
                //must be converted into y[#t] where #t is for instance 1950-2100, and the timeperiod is undated 18-100

                MapMultidimItem map = kvp.Key;
                string s = map.storage[iDim - 1];
                int a = G.IntParse(s);
                if (a == -12345)
                {
                    G.Writeln2("+++ NOTE: Could not parse '" + s + "' as an integer, skipped");
                    continue;
                }

                Series tsSub = kvp.Value as Series;
                if (tsSub == null)
                {
                    G.Writeln2("*** ERROR: Element is not a series");  //should not be possible
                    throw new GekkoException();
                }

                if (tsSub.type == ESeriesType.Timeless)
                {
                    G.Writeln2("*** ERROR: Sub-series is timeless ... conversion will be fixed later on");
                    throw new GekkoException();
                }

                foreach (GekkoTime t in new GekkoTimeIterator(tsSub.GetRealDataPeriodFirst(), tsSub.GetRealDataPeriodLast()))
                {
                    MapMultidimItem mapRotated = map.Clone();
                    map.parent = tsRotated;
                    mapRotated.storage[iDim - 1] = t.ToString();

                    Series tsRotatedSub = null;
                    IVariable iv2 = null; tsRotated.dimensionsStorage.TryGetValue(mapRotated, out iv2);
                    if (iv2 == null)
                    {
                        tsRotatedSub = new Series(EFreq.U, null);
                        tsRotated.dimensionsStorage.AddIVariableWithOverwrite(mapRotated, tsRotatedSub);
                    }
                    else
                    {
                        tsRotatedSub = iv2 as Series;
                    }
                    GekkoTime tu = new GekkoTime(EFreq.U, a, 1);
                    tsRotatedSub.SetData(tu, tsSub.GetVal(t));
                }
            }

            return tsRotated;
        }

        public static IVariable bankname(GekkoSmpl smpl, IVariable x1)
        {
            if (x1.Type() == EVariableType.String)
            {
                string s = x1.ConvertToString();
                if (G.Equal(s, Globals.First))
                {
                    return new ScalarString(Program.databanks.GetFirst().name);
                }
                else if (G.Equal(s, Globals.Ref))
                {
                    return new ScalarString(Program.databanks.GetRef().name);
                }
                else
                {
                    G.Writeln2("*** ERROR: bankname() accepts strings 'first' or 'ref'");
                    throw new GekkoException();
                }
            }
            else if (x1.Type() == EVariableType.Val)
            {

                int x = O.ConvertToInt(x1);
                Databank db = null;
                if (x < 0)
                {
                    G.Writeln2("*** ERROR: bankname() must be called with value >= 0");
                    throw new GekkoException();
                }
                else if (x == 0)
                {
                    return new ScalarVal(Program.databanks.storage.Count - 1);  //number of open banks (except Ref)
                }
                else if (x >= Program.databanks.storage.Count)
                {
                    G.Writeln2("*** ERROR: bankname() must be called with < " + Program.databanks.storage.Count);
                    throw new GekkoException();
                }
                else if (x == 1) return new ScalarString(Program.databanks.GetFirst().name);
                else
                {
                    return new ScalarString(Program.databanks.storage[x].name);
                }
            }
            else
            {
                G.Writeln2("*** ERROR: bankname() only accepts string or val");
                throw new GekkoException();
            }

        }

        public static IVariable getendoexo(GekkoSmpl smpl)
        {
            Databank databank = Program.databanks.GetFirst();

            List<string> fixes = new List<string>();
            foreach (string name in new string[] { "endo", "exo" })
            {
                foreach (KeyValuePair<string, IVariable> kvp in databank.storage)
                {
                    if (kvp.Key.StartsWith(name + "_", StringComparison.OrdinalIgnoreCase) && kvp.Key.EndsWith(Globals.freqIndicator + G.GetFreq(Program.options.freq), StringComparison.OrdinalIgnoreCase))
                    {
                        //starts with endo_ or exo_ and is of annual type
                        fixes.Add(G.Chop_RemoveFreq(kvp.Key));
                    }
                }
            }
            fixes.Sort();
            List fix = new List(fixes);
            return fix;
        }

        public static IVariable refname(GekkoSmpl smpl)
        {
            return new ScalarString(Program.databanks.GetRef().name);
        }

        public static IVariable bankfilename(GekkoSmpl smpl, IVariable x1)
        {
            return bankfilename(smpl, x1, new ScalarString(""));
        }

        public static IVariable bankfilename(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            string y1 = x1.ConvertToString();
            string rv = null;
            Databank db = Program.databanks.GetDatabank(y1);
            if (db == null)
            {
                G.Writeln2("*** ERROR: No open databank has the name '" + y1 + "'");
                throw new GekkoException();
            }

            string y2 = x2.ConvertToString();
            if (G.Equal(y2, "fullpath"))
            {
                rv = db.FileNameWithPath;
            }
            else
            {
                rv = Program.GetDatabankFilename(db);
            }
            return new ScalarString(rv);
        }

        ////just to test against user defined function
        ////is this used at all???
        //public static IVariable sum_test_method(GekkoSmpl smpl, IVariable x1, IVariable x2)
        //{
        //    double y1 = x1.GetValOLD(null);//uuu
        //    double y2 = x2.GetValOLD(null);//uuu            
        //    double y = y1 + y2;
        //    return new ScalarVal(y);
        //}

        public static IVariable test(GekkoSmpl t, IVariable x1)
        {
            return x1.Indexer(t, O.EIndexerType.IndexerLag, new ScalarVal(-2d));
        }

        public static IVariable errorhelper(GekkoSmpl t, IVariable x)
        {
            string s = O.ConvertToString(x);
            if (s == Globals.errorHelper)
            {
                G.Writeln2("*** ERROR: ErrorHelper #" + s);
                throw new GekkoException();
            }
            return Globals.scalarVal0;
        }

        public static IVariable isopen(GekkoSmpl smpl, IVariable x1)
        {
            string s1 = O.ConvertToString(x1);
            if (G.Equal(s1, Globals.Ref) || G.Equal(s1, Globals.Work) || G.Equal(s1, Globals.Global) || G.Equal(s1, Globals.Local)) return Globals.scalarVal1;

            //Work is typically at pos = 0, but not always so. Ref is always at pos = 1, but we just probe storage[1] for simplicity
            for (int i = 0; i < Program.databanks.storage.Count; i++)
            {
                Databank db = Program.databanks.storage[i];
                if (G.Equal(db.name, s1)) return Globals.scalarVal1;
            }
            return Globals.scalarVal0;
        }

        public static IVariable concat(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            //same as %s1 + %s2 anyway.
            string s1 = O.ConvertToString(x1);
            string s2 = O.ConvertToString(x2);
            return new ScalarString(s1 + s2);
        }

        public static IVariable piece(GekkoSmpl smpl, IVariable x1, IVariable x2, IVariable x3)
        {
            G.Writeln2("*** ERROR: Rename: please use substring() instead of piece()");
            throw new GekkoException();
        }
                
        public static IVariable substring(GekkoSmpl smpl, IVariable x1, IVariable x2, IVariable x3)
        {
            string s = null;
            string s1 = O.ConvertToString(x1);
            int i2 = O.ConvertToInt(x2);
            int i3 = O.ConvertToInt(x3);
            if (i3 < 0)
            {
                //AREMOS supports this...
                G.Writeln2("*** ERROR: piece(): size < 0 not supported.");
                throw new GekkoException();
            }
            int a, b = 0;
            if (i2 < 0)
            {
                a = s1.Length + i2;
                b = i3;
            }
            else
            {
                a = i2 - 1;
                b = i3;
            }
            if (a < 0 || a > s1.Length - 1 || a + b > s1.Length)
            {
                G.Writeln2("*** ERROR: piece() function with start " + (a + 1) + " and size " + b + " is not possible");
                G.Writeln("           on a string of size " + s1.Length + ".", System.Drawing.Color.Red);
                throw new GekkoException();
            }
            s = s1.Substring(a, b);
            return new ScalarString(s);
        }

        //public static void HELPER_HandleLasp(GekkoTuple.Tuple2 tuple, IVariable p, IVariable q) {
        //    //This is pretty bad style, but the content of the tuple is put into p and q...

        //    Series tsp1 = O.GetTimeSeries(p);
        //    Series tsq1 = O.GetTimeSeries(q);

        //    Series tsp2 = O.GetTimeSeries(tuple.tuple0);
        //    Series tsq2 = O.GetTimeSeries(tuple.tuple1);

        //    tsp2.name = tsp1.name;
        //    tsq2.name = tsq1.name;

        //    tsp1.meta.parentDatabank.RemoveVariable(tsp1.name);
        //    tsq1.meta.parentDatabank.RemoveVariable(tsq1.name);

        //    tsp1.meta.parentDatabank.AddVariable(tsp2);
        //    tsq1.meta.parentDatabank.AddVariable(tsq2);
        //}

        public static IVariable laspchain(GekkoSmpl smpl, IVariable plist, IVariable xlist, IVariable date)
        {
            IVariable result = Program.Laspeyres("laspchain", plist, xlist, date.ConvertToDate(O.GetDateChoices.Strict), Globals.globalPeriodStart, Globals.globalPeriodEnd);
            return result;
        }

        public static IVariable laspchain(GekkoSmpl smpl, IVariable t1, IVariable t2, IVariable plist, IVariable xlist, IVariable date)
        {
            IVariable result = Program.Laspeyres("laspchain", plist, xlist, date.ConvertToDate(O.GetDateChoices.Strict), t1.ConvertToDate(O.GetDateChoices.Strict), t2.ConvertToDate(O.GetDateChoices.Strict));
            return result;
        }

        public static IVariable laspfixed(GekkoSmpl smpl, IVariable plist, IVariable xlist, IVariable date)
        {
            IVariable result = Program.Laspeyres("laspfixed", plist, xlist, date.ConvertToDate(O.GetDateChoices.Strict), Globals.globalPeriodStart, Globals.globalPeriodEnd);
            return result;
        }

        public static IVariable laspfixed(GekkoSmpl smpl, IVariable t1, IVariable t2, IVariable plist, IVariable xlist, IVariable date)
        {
            IVariable result = Program.Laspeyres("laspfixed", plist, xlist, date.ConvertToDate(O.GetDateChoices.Strict), t1.ConvertToDate(O.GetDateChoices.Strict), t2.ConvertToDate(O.GetDateChoices.Strict));
            return result;
        }

        public static IVariable hpfilter(GekkoSmpl t, IVariable rightSide, IVariable ilambda)
        {
            return hpfilter(t, rightSide, null, null, ilambda, Globals.scalarVal0);
        }

        public static IVariable hpfilter(GekkoSmpl t, IVariable rightSide, IVariable per1, IVariable per2, IVariable ilambda)
        {
            return hpfilter(t, rightSide, per1, per2, ilambda, Globals.scalarVal0);
        }

        public static IVariable hpfilter(GekkoSmpl t, IVariable rightSide, IVariable ilambda, IVariable ilog)
        {
            return hpfilter(t, rightSide, null, null, ilambda, ilog);
        }

        public static IVariable hpfilter(GekkoSmpl smpl, IVariable rightSide, IVariable per1, IVariable per2, IVariable ilambda, IVariable ilog)
        {
            GekkoTime tStart = GekkoTime.tNull;
            GekkoTime tEnd = GekkoTime.tNull;
            if (per1 == null && per2 == null)
            {
                tStart = Globals.globalPeriodStart;
                tEnd = Globals.globalPeriodEnd;
            }
            else
            {
                tStart = O.ConvertToDate(per1);
                tEnd = O.ConvertToDate(per2);
            }

            int obs = GekkoTime.Observations(tStart, tEnd);

            double lambda = O.ConvertToVal(ilambda);
            double log = O.ConvertToVal(ilog);

            Series rhs = O.ConvertToSeries(rightSide) as Series;

            Series lhs = new Series(ESeriesType.Light, smpl.t0, smpl.t3);

            bool isLog = false;
            if (log == 0d)
            {
                isLog = false;
            }
            else if (log == 1d)
            {
                isLog = true;
            }
            else
            {
                G.Writeln2("*** ERROR: hpfilter() logarithm argument should be 0 or 1");
                throw new GekkoException();
            }

            if (obs < 2)
            {
                G.Writeln2("*** ERROR: hpfilter() needs at least two observations to make sense");
                throw new GekkoException();
            }

            double[] input = new double[obs];

            int counter = -1;
            foreach (GekkoTime gt in new GekkoTimeIterator(tStart, tEnd))
            {
                counter++;
                if (isLog)
                {
                    input[counter] = Math.Log(rhs.GetData(smpl, gt));
                }
                else
                {
                    input[counter] = rhs.GetData(smpl, gt);
                }
            }

            HPfilter hpf = new HPfilter();
            double[] output = hpf.HPFilter(input, lambda);

            counter = -1;
            foreach (GekkoTime gt in new GekkoTimeIterator(tStart, tEnd))
            {
                counter++;
                if (isLog)
                {
                    lhs.SetData(gt, Math.Exp(output[counter]));
                }
                else
                {
                    lhs.SetData(gt, output[counter]);
                }
            }

            return lhs;
        }

        // ====================== matrix stuff ===============================
        // ====================== matrix stuff ===============================
        // ====================== matrix stuff ===============================

        public static IVariable t(GekkoSmpl smpl, IVariable x1)
        {
            if (x1.Type() != EVariableType.Matrix)
            {
                G.Writeln("*** ERROR: t(): transpose can only be used on matrices");
                throw new GekkoException();
            }
            Matrix x = (Matrix)x1;
            int d1 = x.data.GetLength(0);
            int d2 = x.data.GetLength(1);
            Matrix y = new Matrix(d2, d1);
            for (int i = 0; i < d1; i++)
            {
                for (int j = 0; j < d2; j++)
                {
                    y.data[j, i] = x.data[i, j];
                }
            }
            return y;
        }

        //Converts timeseries to matrix
        public static IVariable pack(GekkoSmpl smpl, params IVariable[] vars)
        {
            GekkoTime gt1 = Globals.globalPeriodStart;
            GekkoTime gt2 = Globals.globalPeriodEnd;
            int offset = 0;
            int obs = PackHelper(vars, ref gt1, ref gt2, ref offset);

            List<IVariable> temp = new List<IVariable>();
            for (int i = offset; i < vars.Length; i++)
            {
                temp.Add(vars[i]);
            }

            List<Series> tss = Program.UnfoldAsSeries(smpl, temp);

            //List<Series> tss = new List<Series>();
            //for (int j = offset; j < vars.Length; j++)
            //{
            //    if (vars[j].Type() == EVariableType.List)
            //    {
            //        foreach (IVariable iv in ((List)vars[j]).list)
            //        {
            //            string s = O.ConvertToString(iv);
            //            Series tmp = Program.GetTimeSeriesFromString(s, O.ECreatePossibilities.NoneReturnNull);
            //            tss.Add(tmp);
            //        }
            //    }
            //    else if (vars[j].Type() == EVariableType.Series)
            //    {
            //        //LIGHTFIXME
            //        tss.Add((Series)vars[j]);
            //    }
            //    else
            //    {
            //        G.Writeln2("*** ERROR: Expected timeseries or list as argument");
            //        throw new GekkoException();
            //    }
            //}

            int n = tss.Count;
            if (n < 1)
            {
                G.Writeln2("*** ERROR: Number of items is " + n);
                throw new GekkoException();
            }

            Matrix m = new Matrix(obs, n);

            //    List<Series> tss = Program.GetTimeSeriesFromStringWildcard(s);

            int varcount = -1;
            foreach (Series ts in tss)
            {
                varcount++;
                int counter = -1;
                foreach (GekkoTime gt in new GekkoTimeIterator(gt1, gt2))
                {
                    counter++;
                    m.data[counter, varcount] = ts.GetData(smpl, gt);
                }
            }
            return m;
        }

        public static IVariable det(GekkoSmpl smpl, IVariable x)
        {
            Matrix m = O.ConvertToMatrix(x);
            double d = alglib.rmatrixdet(m.data);
            return new ScalarVal(d);
        }

        public static IVariable rows(GekkoSmpl smpl, IVariable x1)
        {
            Matrix m = O.ConvertToMatrix(x1);
            return new ScalarVal(m.data.GetLength(0));
        }

        public static IVariable cols(GekkoSmpl smpl, IVariable x1)
        {
            Matrix m = O.ConvertToMatrix(x1);
            return new ScalarVal(m.data.GetLength(1));
        }

        public static IVariable multiply(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            return ElementByElementHelper(EElementByElementType.Times, smpl, x1, x2);
        }

        public static IVariable divide(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            return ElementByElementHelper(EElementByElementType.Divide, smpl, x1, x2);
        }

        public static IVariable zeroes(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            return zeros(smpl, x1, x2);
        }


        public static IVariable series(GekkoSmpl smpl, params IVariable[] x)
        {
            //series() normal series with current freq
            //series(0) normal series with current freq
            //series('a') normal annual series
            //series(3) 3-dim series with current freq
            //series('a', 3) 3-dim annual series            
            Series ts = HELPER_seriesAndTimeless("series", x);
            return ts;
        }

        public static IVariable timeless(GekkoSmpl smpl, params IVariable[] x)
        {
            //timeless() normal timeless with current freq
            //timeless(200) normal timeless with value 200
            //timeless('a') normal annual timeless            
            //timeless('a', 200) annual timeless with value 200
            Series ts = HELPER_seriesAndTimeless("timeless", x);
            return ts;
        }

        private static Series HELPER_seriesAndTimeless(string type, IVariable[] x)
        {
            Series ts = null;
            if (G.Equal(type, "series"))
            {
                if (x.Length == 0)
                {
                    ts = new Series(Program.options.freq, null);
                }
                else if (x.Length == 1)
                {
                    if (x[0].Type() == EVariableType.String)
                    {
                        //frequency
                        EFreq freq = G.GetFreq(O.ConvertToString(x[0]));
                        ts = new Series(freq, null);
                    }
                    else if (x[0].Type() == EVariableType.Val)
                    {
                        ts = new Series(Program.options.freq, null);
                        ts.dimensionsStorage = new MapMultidim();
                        ts.dimensions = O.ConvertToInt(x[0]);
                        ts.type = ESeriesType.ArraySuper;
                        ts.meta = new SeriesMetaInformation();
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Expected argument 1 in series() to be VAL or STRING");
                        throw new GekkoException();
                    }
                }
                else if (x.Length == 2)
                {
                    if (x[0].Type() == EVariableType.String)
                    {
                        //frequency
                        EFreq freq = G.GetFreq(O.ConvertToString(x[0]));
                        ts = new Series(freq, null);
                        ts.dimensions = O.ConvertToInt(x[1]);
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: series() with 2 arguments must have STRING as first argument");
                        throw new GekkoException();
                    }
                }
                else
                {

                    G.Writeln2("*** ERROR: series() does not accept > 2 arguments");
                    throw new GekkoException();
                }

            }
            else
            {
                if (x.Length == 0)
                {
                    ts = new Series(ESeriesType.Timeless, Program.options.freq, null, double.NaN);
                }
                else if (x.Length == 1)
                {
                    if (x[0].Type() == EVariableType.String)
                    {
                        //frequency
                        EFreq freq = G.GetFreq(O.ConvertToString(x[0]));
                        ts = new Series(ESeriesType.Timeless, freq, null, double.NaN);
                    }
                    else if (x[0].Type() == EVariableType.Val)
                    {
                        ts = new Series(ESeriesType.Timeless, Program.options.freq, null, x[0].ConvertToVal());
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Expected argument 1 in timeless() to be VAL or STRING");
                        throw new GekkoException();
                    }
                }
                else if (x.Length == 2)
                {
                    if (x[0].Type() == EVariableType.String)
                    {
                        //frequency
                        EFreq freq = G.GetFreq(O.ConvertToString(x[0]));
                        double d = x[1].ConvertToVal();
                        ts = new Series(ESeriesType.Timeless, freq, null, d);
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: timeless() with 2 arguments must have STRING as first argument");
                        throw new GekkoException();
                    }
                }
                else
                {

                    G.Writeln2("*** ERROR: series() does not accept > 2 arguments");
                    throw new GekkoException();
                }
            }

            return ts;
        }

        public static IVariable i(GekkoSmpl smpl, IVariable x)
        {
            int n = O.ConvertToInt(x);
            Matrix m = new Matrix(n, n);
            for (int i = 0; i < n; i++)
            {
                m.data[i, i] = 1d;
            }
            return m;
        }

        public static IVariable diag(GekkoSmpl smpl, IVariable x)
        {
            Matrix m = null;
            Matrix xx = O.ConvertToMatrix(x);
            //for a 1x1 matrix, either of the two cases below yields the same!
            if (xx.data.GetLength(1) == 1)
            {
                int n = xx.data.GetLength(0);  //rows
                m = new Matrix(n, n);
                for (int i = 0; i < n; i++)
                {
                    m.data[i, i] = xx.data[i, 0];
                }
            }
            else
            {
                int n = CheckSquare(xx);
                m = new Matrix(n, 1);
                for (int i = 0; i < n; i++)
                {
                    m.data[i, 0] = xx.data[i, i];
                }
            }
            return m;
        }

        public static IVariable trace(GekkoSmpl smpl, IVariable x)
        {
            Matrix m = O.ConvertToMatrix(x);
            int n = CheckSquare(m);
            double d = 0d;
            for (int i = 0; i < n; i++)
            {
                d += m.data[i, i];
            }
            return new ScalarVal(d);
        }

        private static int CheckSquare(Matrix m)
        {
            if (m.data.GetLength(0) != m.data.GetLength(1))
            {
                G.Writeln("*** ERROR: The matrix is not square (rows = " + m.data.GetLength(0) + ", cols = " + m.data.GetLength(1));
                throw new GekkoException();
            }
            return m.data.GetLength(0);
        }

        public static IVariable inv(GekkoSmpl smpl, IVariable x)
        {
            Matrix m = O.ConvertToMatrix(x);
            int n = CheckSquare(m);
            Matrix clone = m.Clone();
            clone.data = Program.InvertMatrix(clone.data);
            return clone;
        }

        public static IVariable zeros(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            int n1 = O.ConvertToInt(x1);
            int n2 = O.ConvertToInt(x2);
            Matrix m = new Matrix(n1, n2);
            return m;
        }

        public static IVariable sumr(GekkoSmpl smpl, IVariable x)
        {
            return SumHelper(smpl, x, ESumDim.Rows, ESumType.Sum);
        }

        public static IVariable sumc(GekkoSmpl smpl, IVariable x)
        {
            return SumHelper(smpl, x, ESumDim.Cols, ESumType.Sum);
        }

        public static IVariable avgr(GekkoSmpl smpl, IVariable x)
        {
            return SumHelper(smpl, x, ESumDim.Rows, ESumType.Avg);
        }

        public static IVariable avgc(GekkoSmpl smpl, IVariable x)
        {
            return SumHelper(smpl, x, ESumDim.Cols, ESumType.Avg);
        }

        public static IVariable minr(GekkoSmpl smpl, IVariable x)
        {
            return SumHelper(smpl, x, ESumDim.Rows, ESumType.Min);
        }

        public static IVariable minc(GekkoSmpl smpl, IVariable x)
        {
            return SumHelper(smpl, x, ESumDim.Cols, ESumType.Min);
        }

        public static IVariable maxr(GekkoSmpl smpl, IVariable x)
        {
            return SumHelper(smpl, x, ESumDim.Rows, ESumType.Max);
        }

        public static IVariable m(GekkoSmpl smpl)
        {
            //alias
            return miss(smpl);
        }

        public static IVariable m(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            //alias
            return miss(smpl, x1, x2);
        }

        //missing value
        public static IVariable miss(GekkoSmpl smpl)
        {
            return Globals.scalarValMissing;
        }

        public static IVariable miss(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            int n1 = O.ConvertToInt(x1);
            int n2 = O.ConvertToInt(x2);
            Matrix m = new Matrix(n1, n2, double.NaN);
            return m;
        }

        public static IVariable ismiss(GekkoSmpl smpl, IVariable x)
        {
            if (x.Type() == EVariableType.Series)
            {
                G.Writeln2("*** ERROR: SERIES not expected in ismiss()");
                throw new GekkoException();
            }
            double d = x.ConvertToVal();
            bool b = G.isNumericalError(d);
            if (b) return Globals.scalarVal1;
            return Globals.scalarVal0;
        }

        public static IVariable maxc(GekkoSmpl smpl, IVariable x)
        {
            return SumHelper(smpl, x, ESumDim.Cols, ESumType.Max);
        }

        private static IVariable SumHelper(GekkoSmpl smpl, IVariable x, ESumDim dim, ESumType type)
        {
            Matrix m = O.ConvertToMatrix(x);
            int rows = m.data.GetLength(0);
            int cols = m.data.GetLength(1);

            int max = -12345;
            if (dim == ESumDim.Rows) max = rows;
            else if (dim == ESumDim.Rows) max = cols;

            Matrix m2 = null;
            if (dim == ESumDim.Cols)
            {
                if (type == ESumType.Min)
                {
                    m2 = new Matrix(1, cols, double.MaxValue);
                }
                else if (type == ESumType.Max)
                {
                    m2 = new Matrix(1, cols, double.MinValue);
                }
                else if (type == ESumType.Sum || type == ESumType.Avg)
                {
                    m2 = new Matrix(1, cols);
                }
                else throw new GekkoException();
            }
            else if (dim == ESumDim.Rows)
            {
                if (type == ESumType.Min)
                {
                    m2 = new Matrix(rows, 1, double.MaxValue);
                }
                else if (type == ESumType.Max)
                {
                    m2 = new Matrix(rows, 1, double.MinValue);
                }
                else if (type == ESumType.Sum || type == ESumType.Avg)
                {
                    m2 = new Matrix(rows, 1);
                }
                else throw new GekkoException();
            }
            else throw new GekkoException();

            if (dim == ESumDim.Cols)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (type == ESumType.Min)
                        {
                            if (m2.data[0, j] > m.data[i, j]) m2.data[0, j] = m.data[i, j];
                        }
                        else if (type == ESumType.Max)
                        {
                            if (m2.data[0, j] < m.data[i, j]) m2.data[0, j] = m.data[i, j];
                        }
                        else if (type == ESumType.Sum || type == ESumType.Avg)
                        {
                            m2.data[0, j] += m.data[i, j];
                        }
                        else throw new GekkoException();
                    }
                }
                if (type == ESumType.Avg)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        m2.data[0, j] = m2.data[0, j] / (double)rows;
                    }
                }
            }
            else if (dim == ESumDim.Rows)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (type == ESumType.Min)
                        {
                            if (m2.data[i, 0] > m.data[i, j]) m2.data[i, 0] = m.data[i, j];
                        }
                        else if (type == ESumType.Max)
                        {
                            if (m2.data[i, 0] < m.data[i, j]) m2.data[i, 0] = m.data[i, j];
                        }
                        else if (type == ESumType.Sum || type == ESumType.Avg)
                        {
                            m2.data[i, 0] += m.data[i, j];
                        }
                        else throw new GekkoException();
                    }
                }
                if (type == ESumType.Avg)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        m2.data[i, 0] = m2.data[i, 0] / (double)cols;
                    }
                }
            }
            else throw new GekkoException();
            return m2;
        }

        public static IVariable ones(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            int n1 = O.ConvertToInt(x1);
            int n2 = O.ConvertToInt(x2);
            Matrix m = new Matrix(n1, n2, 1d);
            return m;
        }

        //Multiplication element by element
        private static IVariable ElementByElementHelper(EElementByElementType type, GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            Matrix m1 = O.ConvertToMatrix(x1);
            Matrix m2 = O.ConvertToMatrix(x2);
            bool fixedRow1 = false;
            bool fixedCol1 = false;
            if (m1.data.GetLength(0) != m2.data.GetLength(0))
            {
                if (m2.data.GetLength(0) != 1)
                {
                    G.Writeln2("*** ERROR: " + type.ToString() + "(): There are " + m1.data.GetLength(0) + " and " + m2.data.GetLength(0) + " rows in the matrices");
                    throw new GekkoException();
                }
                else
                {
                    //for instance div ( 2x3 , 1x3 ) --> use fixed row 1 instead of i
                    fixedRow1 = true;
                }
            }
            if (m1.data.GetLength(1) != m2.data.GetLength(1))
            {
                if (m2.data.GetLength(1) != 1)
                {
                    G.Writeln2("*** ERROR: " + type.ToString() + "(): There are " + m1.data.GetLength(1) + " and " + m2.data.GetLength(1) + " cols in the matrices");
                    throw new GekkoException();
                }
                else
                {
                    //for instance div ( 2x3 , 2x1 ) --> use fixed column 1 instead of j
                    fixedCol1 = true;
                }
            }
            Matrix m = new Matrix(m1.data.GetLength(0), m1.data.GetLength(1));
            for (int i = 0; i < m1.data.GetLength(0); i++)
            {
                for (int j = 0; j < m1.data.GetLength(1); j++)
                {
                    int ii = i;
                    int jj = j;
                    if (fixedRow1) ii = 0; //internal arrays are 0-based
                    if (fixedCol1) jj = 0; //internal arrays are 0-based
                    if (type == EElementByElementType.Times)
                    {
                        m.data[i, j] = m1.data[i, j] * m2.data[ii, jj];
                    }
                    else if (type == EElementByElementType.Divide)
                    {
                        m.data[i, j] = m1.data[i, j] / m2.data[ii, jj];
                    }
                }
            }
            return m;
        }

        public static IVariable unpack(GekkoSmpl smpl, IVariable x)
        {
            return unpack(smpl, null, null, x);
        }

        //Converts matrix to timeseries
        public static IVariable unpack(GekkoSmpl smpl, IVariable t1, IVariable t2, IVariable x)
        {
            //from matrix to timeseries
            //smpl is not used
            GekkoTime gt1 = Globals.globalPeriodStart;
            GekkoTime gt2 = Globals.globalPeriodEnd;
            if (t1 != null)
            {
                gt1 = O.ConvertToDate(t1);
                gt2 = O.ConvertToDate(t2);
            }

            int obs = GekkoTime.Observations(gt1, gt2);

            Matrix m = O.ConvertToMatrix(x);

            if (m.data.GetLength(1) > 1)
            {
                G.Writeln2("*** ERROR: The matrix provided should have 1 column only");
                throw new GekkoException();
            }

            if (m.data.GetLength(0) != obs)
            {
                G.Writeln2("*** ERROR: You provided " + obs + " periods for a matrix with " + m.data.GetLength(0) + " rows");
                throw new GekkoException();
            }

            return O.CreateTimeSeriesFromMatrix(new GekkoSmpl(gt1, gt2), m);

            //GekkoTime gt1 = Globals.globalPeriodStart;
            //GekkoTime gt2 = Globals.globalPeriodEnd;
            //int offset = 0;
            //int obs = PackHelper(vars, ref gt1, ref gt2, ref offset);

            //int n = vars.Length - offset;
            //if (n < 1)
            //{
            //    G.Writeln2("*** ERROR: No matrix given");
            //    throw new GekkoException();
            //}
            //else if (n > 1)
            //{
            //    G.Writeln2("*** ERROR: Only 1 matrix should be given");
            //    throw new GekkoException();
            //}
            //Matrix m = O.GetMatrix(vars[offset]);

            //if (m.data.GetLength(1) > 1)
            //{
            //    G.Writeln2("*** ERROR: The matrix provided should have 1 column only");
            //    throw new GekkoException();
            //}

            //if (m.data.GetLength(0) != obs)
            //{
            //    G.Writeln2("*** ERROR: You provided " + obs + " periods for a matrix with " + m.data.GetLength(0) + " rows");
            //    throw new GekkoException();
            //}

            //Series ts = new Series(Program.options.freq, null);
            //int counter = -1;
            //foreach (GekkoTime gt in new GekkoTimeIterator(gt1, gt2))
            //{
            //    counter++;
            //    ts.SetData(gt, m.data[counter, 0]);
            //}
            //return new MetaTimeSeries(ts);
        }

        private static int PackHelper(IVariable[] vars, ref GekkoTime gt1, ref GekkoTime gt2, ref int offset)
        {
            if (vars.Length > 2)  //must be at least 1 variable
            {
                if ((vars[0].Type() == EVariableType.Date || vars[0].Type() == EVariableType.Val) || (vars[1].Type() == EVariableType.Date || vars[1].Type() == EVariableType.Val))
                {
                    //seems like two dates first
                    offset = 2;
                    gt1 = O.ConvertToDate(vars[0]);
                    gt2 = O.ConvertToDate(vars[1]);
                }
            }
            int obs = GekkoTime.Observations(gt1, gt2);
            if (obs < 1)
            {
                G.Writeln2("*** ERROR: Number of observations is " + obs);
                throw new GekkoException();
            }
            return obs;
        }

        public static IVariable len(GekkoSmpl smpl, IVariable x1)
        {
            return length(smpl, x1);
        }

        public static IVariable length(GekkoSmpl smpl, IVariable x1)
        {
            if (x1.Type() == EVariableType.String)
            {
                string s1 = O.ConvertToString(x1);
                return new ScalarVal(s1.Length);
            }
            else if (x1.Type() == EVariableType.List)
            {
                List l1 = x1 as List;
                return new ScalarVal(l1.list.Count);
            }
            else
            {
                FunctionError("length", x1);
                return null;
            }
        }

        public static IVariable chol(GekkoSmpl smpl, IVariable x)
        {
            return chol(smpl, x, new ScalarString("upper"));
        }

        public static IVariable chol(GekkoSmpl smpl, IVariable x, IVariable type)
        {
            if (x.Type() != EVariableType.Matrix)
            {
                G.Writeln2("*** ERROR: Chol() only accepts a matrix");
                throw new GekkoException();
            }

            if (type.Type() != EVariableType.String)
            {
                G.Writeln2("*** ERROR: Chol() only accepts a string as type");
                throw new GekkoException();
            }

            double[,] y = ((Matrix)x).data;

            string s = ((ScalarString)type).string2;

            bool upper = true;
            if (G.Equal(s, "upper"))
            {

            }
            else if (G.Equal(s, "lower"))
            {
                upper = false;
            }
            else
            {
                G.Writeln2("*** ERROR: Type must be 'upper' or 'lower'");
                throw new GekkoException();
            }

            double[,] z = Cholesky(y, upper);
            Matrix rv = new Matrix();
            rv.data = z;
            return rv;
        }

        public static IVariable rseed(GekkoSmpl smpl, IVariable seed)
        {
            double seed2 = O.ConvertToVal(seed);
            int i = (int)seed2;
            Globals.random = new Random(i);
            return new ScalarVal(i);
        }

        public static IVariable rnorm(GekkoSmpl t, IVariable means, IVariable vcov)
        {
            //Maybe it is stupid that we are using stddev versus matrix of covariance

            if (means.Type() == EVariableType.Matrix && vcov.Type() == EVariableType.Matrix)
            {
                //This will be SLOW, because of cholesky decomp for each drawing
                //To circumvent this, we would have to define an object to store to fixed cover matrix

                //Otherwise, we could output a n x sims matrix instead of n x 1.

                Matrix m = (Matrix)vcov;
                int n = m.data.GetLength(0);
                if (m.data.GetLength(1) != n)
                {
                    G.Writeln2("*** ERROR: Covar matrix is not square");
                    throw new GekkoException();
                }

                Matrix mean = (Matrix)means;
                if (mean.data.GetLength(0) != n || mean.data.GetLength(1) != 1)
                {
                    G.Writeln2("*** ERROR: Mean matrix does not correspond to covar matrix");
                    throw new GekkoException();
                }

                double[,] tmp = Cholesky(m.data, false);
                //after this, #m = t(#tmp)*#tmp

                double[,] randoms = new double[n, 1];
                //https://en.wikipedia.org/wiki/Multivariate_normal_distribution#Drawing_values_from_the_distribution
                for (int i = 0; i < n; i++)
                {
                    double random = O.ConvertToVal(rnorm(t, Globals.scalarVal0, Globals.scalarVal1)); //could be sped up by interfacing to the interior of the method
                    randoms[i, 0] = random;
                }

                Matrix rv = new Matrix();
                rv.data = O.AddMatrixMatrix(mean.data, Program.MultiplyMatrices(tmp, randoms), n, 1);

                return rv;


            }
            else
            {
                double mean = O.ConvertToVal(means);
                double stdDev = Math.Sqrt(O.ConvertToVal(vcov));
                Random rand = new Random(); //reuse this if you are generating many
                double u1 = Globals.random.NextDouble(); //these are uniform(0,1) random doubles
                double u2 = Globals.random.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
                return new ScalarVal(randNormal);
            }
        }

        private static double[,] Cholesky(double[,] m, bool upper)
        {
            if (m.GetLength(0) != m.GetLength(1))
            {
                G.Writeln2("*** ERROR: Matrix must be square");
                throw new GekkoException();
            }
            int n = m.GetLength(0);
            double[,] tmp = new double[n, n];

            if (upper)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = i; j < n; j++)
                    {
                        tmp[i, j] = m[i, j];
                    }
                }
            }
            else
            {
                for (int j = 0; j < n; j++)
                {
                    for (int i = j; i < n; i++)
                    {
                        tmp[i, j] = m[i, j];
                    }
                }
            }

            bool result = alglib.spdmatrixcholesky(ref tmp, n, upper);

            if (!result)
            {
                G.Writeln2("*** ERROR: Could not perform Cholesky decomposition");
                throw new GekkoException();
            }

            return tmp;
        }

        public static IVariable runif(GekkoSmpl smpl)
        {
            double u2 = Globals.random.NextDouble();
            return new ScalarVal(u2);
        }

        public static IVariable contains(GekkoSmpl smpl, IVariable y, IVariable x)
        {
            string x_string = x.ConvertToString();
            List<IVariable> y_list = y.ConvertToList();
            foreach (IVariable iv in y_list)
            {
                ScalarString temp = iv as ScalarString;
                if (temp != null && G.Equal(temp.string2, x_string)) return Globals.scalarVal1;
            }
            return Globals.scalarVal0;
        }
        
        public static void setdomains(GekkoSmpl smpl, IVariable x, IVariable m)
        {
            Series x_series = x as Series;
            if (x_series == null || x_series.type != ESeriesType.ArraySuper)
            {
                G.Writeln2("*** ERROR: setdomains(): Expected array-series");
                throw new GekkoException();
            }
            List m_list = m as List;
            if (m_list == null)
            {
                G.Writeln2("*** ERROR: setdomains(): Expected list of strings");
                throw new GekkoException();
            }
            string[] ss = Program.GetListOfStringsFromListOfIvariables(m_list.list.ToArray());
            x_series.meta.domains = ss;            
        }

        public static List getdomains(GekkoSmpl smpl, IVariable x)
        {
            Series x_series = x as Series;
            if (x_series.meta.domains == null) return new List(new List<string>());  //empty
            if (x_series == null || x_series.type != ESeriesType.ArraySuper)
            {
                G.Writeln2("*** ERROR: setdomains(): Expected array-series");
                throw new GekkoException();
            }
            List<string> ss = new List<string>();
            for (int i = 0; i < x_series.meta.domains.Length; i++) ss.Add(x_series.meta.domains[i]);  //cloning for safety
            return new List(ss);
        }

        public static IVariable count(GekkoSmpl smpl, IVariable ths, IVariable y)
        {
            string s = O.ConvertToString(y);
            if (ths.Type() != EVariableType.List)
            {
                FunctionError("count", ths);
            }
            int c = 0;
            foreach (IVariable iv in (ths as List).list)
            {
                ScalarString ss = iv as ScalarString;
                if (ss != null)
                {
                    string s2 = O.ConvertToString(ss);
                    if (G.Equal(s, s2)) c++;
                }
            }
            return new ScalarVal(c);
        }

        public static IVariable remove(GekkoSmpl smpl, IVariable ths, IVariable y)
        {
            string s = O.ConvertToString(y);
            if (ths.Type() != EVariableType.List)
            {
                FunctionError("remove", ths);
            }
            List m = new List();

            foreach (IVariable iv in (ths as List).list)
            {
                ScalarString ss = iv as ScalarString;
                if (ss != null)
                {
                    string s2 = O.ConvertToString(ss);
                    if (G.Equal(s, s2))
                    {
                        //discard                        
                    }
                    else
                    {
                        m.Add(iv);
                    }
                }
            }
            return m;
        }

        public static IVariable pop(GekkoSmpl smpl, IVariable ths, IVariable i)
        {
            int ii = O.ConvertToInt(i);
            return PopHelper(ths, ii);
        }

        public static IVariable pop(GekkoSmpl smpl, IVariable ths)
        {
            return PopHelper(ths, -12345);
        }

        private static IVariable PopHelper(IVariable ths, int ii)
        {
            //ii is 1-based
            if (ths.Type() != EVariableType.List)
            {
                FunctionError("pop", ths);
            }
            List m = new List();

            if (ii == -12345)
            {
                ii = (ths as List).Count();
            }

            if (ii < 1 || ii > (ths as List).Count())
            {
                G.Writeln2("*** ERROR: Cannot pop() at index " + ii);
                throw new GekkoException();
            }

            int c = -1;  //0-based
            foreach (IVariable iv in (ths as List).list)
            {
                c++;
                if (ii != c + 1) m.Add(iv);
            }
            return m;
        }

        public static IVariable sum(GekkoSmpl smpl, params IVariable[] items)
        {
            IVariable tsl2 = HelperSum(smpl.t0, smpl.t3, items, false);
            return tsl2;
        }

        public static IVariable sumt(GekkoSmpl smpl, IVariable x)
        {
            IVariable iv = O.ConvertToSeriesMaybeConstant(smpl, x);
            double d = 0d;
            foreach (GekkoTime t in new GekkoTimeIterator(smpl.t1, smpl.t2))
            {
                d += (iv as Series).GetData(smpl, t);
            }
            return new ScalarVal(d);
        }

        public static IVariable sumt(GekkoSmpl smpl, IVariable x, IVariable d1, IVariable d2)
        {
            GekkoTime t1 = O.ConvertToDate(d1);
            GekkoTime t2 = O.ConvertToDate(d2);

            GekkoSmpl smplHere = new GekkoSmpl(t1, t2);
            IVariable iv = O.ConvertToSeriesMaybeConstant(smplHere, x);
            double d = 0d;
            foreach (GekkoTime t in new GekkoTimeIterator(smplHere.t1, smplHere.t2))
            {
                d += (iv as Series).GetData(smplHere, t);
            }
            return new ScalarVal(d);
        }

        public static IVariable avgt(GekkoSmpl smpl, IVariable x)
        {
            IVariable iv = O.ConvertToSeriesMaybeConstant(smpl, x);
            double d = 0d;
            foreach (GekkoTime t in new GekkoTimeIterator(smpl.t1, smpl.t2))
            {
                d += (iv as Series).GetData(smpl, t);
            }
            return new ScalarVal(d / GekkoTime.Observations(smpl.t1, smpl.t2));
        }

        public static IVariable avgt(GekkoSmpl smpl, IVariable x, IVariable d1, IVariable d2)
        {
            GekkoTime t1 = O.ConvertToDate(d1);
            GekkoTime t2 = O.ConvertToDate(d2);

            GekkoSmpl smplHere = new GekkoSmpl(t1, t2);
            IVariable iv = O.ConvertToSeriesMaybeConstant(smplHere, x);
            double d = 0d;
            foreach (GekkoTime t in new GekkoTimeIterator(smplHere.t1, smplHere.t2))
            {
                d += (iv as Series).GetData(smplHere, t);
            }
            return new ScalarVal(d / GekkoTime.Observations(smplHere.t1, smplHere.t2));
        }

        public static IVariable avg(GekkoSmpl smpl, params IVariable[] items)
        {
            IVariable tsl2 = HelperSum(smpl.t0, smpl.t3, items, true);            
            return tsl2;
        }

        private static IVariable HelperSum(GekkoTime t0, GekkoTime t3, IVariable[] items, bool avg)
        {
            List m = O.ExplodeIvariables(new List(items));
            
            bool hasSeries = false;
            foreach (IVariable item in m.list)
            {
                if (item.Type() == EVariableType.Series)
                {
                    hasSeries = true;
                    break;
                }
            }

            IVariable rv = null;
            if (hasSeries)
            {
                rv = new Series(ESeriesType.Light, t0, t3); //will have small dataarray            
                foreach (GekkoTime t in new GekkoTimeIterator(t0, t3)) (rv as Series).SetData(t, 0d);
            }
            else
            {
                rv = new ScalarVal(0d);
            }

            GekkoSmpl smpl = new GekkoSmpl(t0, t3);
            foreach (IVariable item in m.list)
            {
                IVariable xx = null;
                if (hasSeries) xx = O.ConvertToSeriesMaybeConstant(smpl, item);
                else xx = new ScalarVal(O.ConvertToVal(item));  //will not happen often
                rv = O.Add(smpl, rv, xx);
            }

            if(avg) rv = O.Divide(smpl, rv, new ScalarVal(m.list.Count));

            return rv;
        }
    

        public static IVariable percentile(GekkoSmpl t, IVariable x1, IVariable percent)
        {
            //Mimics Excel's percentile function, see unit tests
            if (G.IsGekkoNull(x1)) return x1;
            GekkoTime t1 = Globals.globalPeriodStart;
            GekkoTime t2 = Globals.globalPeriodEnd;

            Series ts = O.ConvertToSeries(x1) as Series;
            double percent2 = O.ConvertToVal(percent);

            int index1 = -12345;
            int index2 = -12345;
            double[] data = ts.GetDataSequence(out index1, out index2, t1, t2);

            //double[] data2 = new double[index2 - index1 + 1];  //we copy the array, to avoid mishaps if it is altered in the median method (= a little bit slack)
            //for (int i = index1; i <= index2; i++)
            //{
            //    data2[i - index1] = data[i];
            //}

            double median = Program.Percentile(data, percent2);

            ScalarVal z2 = new ScalarVal(median);
            return z2;
        }

        public static IVariable abs(GekkoSmpl smpl, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            IVariable rv = null;
            if (x1.Type() == EVariableType.Val)
            {
                double d = O.ConvertToVal(x1);
                rv = new ScalarVal(Math.Abs(d));
            }
            else if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeries(smpl, x1 as Series, Globals.arithmentics1[1]); // (x1) => Math.Abs(x1);
            }
            else if (x1.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x1);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Abs(m.data[i, j]);
                    }
                }
            }            
            else
            {
                G.Writeln2("*** ERROR: abs(): type " + x1.Type().ToString() + " not supported");
            }
            return rv;
        }


        public static IVariable time(GekkoTime t)
        {
            if (t.freq == EFreq.A || t.freq == EFreq.U)
            {
                return new ScalarVal(t.super);
            }
            else if (t.freq == EFreq.Q)
            {
                return new ScalarVal(t.super + 1d / 4d / 2d + 1d / 4d * (t.sub - 1));
            }
            else if (t.freq == EFreq.M)
            {
                return new ScalarVal(t.super + 1d / 12d / 2d + 1d / 12d * (t.sub - 1));
            }
            throw new GekkoException();
        }

        public static IVariable time(GekkoSmpl smpl)
        {
            Series x = new Series(ESeriesType.Light, smpl.t0.freq, null);
            foreach (GekkoTime t in smpl.Iterate03())
            {
                x.SetData(t, time(t).ConvertToVal());
            }
            return x;
        }

        public static IVariable iif(GekkoSmpl smpl, IVariable i1, IVariable op, IVariable i2, IVariable o1, IVariable o2)
        {

            Series result = new Series(ESeriesType.Light, smpl.t1, smpl.t2);

            if (!IsValOrTimeseries(i1))
            {
                G.Writeln2("*** ERROR: iif(): arg 1, type " + i1.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            if (!IsValOrTimeseries(i2))
            {
                G.Writeln2("*** ERROR: iif(): arg 3, type " + i2.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            if (!IsValOrTimeseries(o1))
            {
                G.Writeln2("*** ERROR: iif(): arg 4, type " + o1.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            if (!IsValOrTimeseries(o2))
            {
                G.Writeln2("*** ERROR: iif(): arg 5, type " + o2.Type().ToString() + " not supported");
                throw new GekkoException();
            }

            Series di1 = new Series(ESeriesType.Light, smpl.t1, smpl.t2);
            Series di2 = new Series(ESeriesType.Light, smpl.t1, smpl.t2);
            Series do1 = new Series(ESeriesType.Light, smpl.t1, smpl.t2);
            Series do2 = new Series(ESeriesType.Light, smpl.t1, smpl.t2);

            foreach (GekkoTime gt in smpl.Iterate12())
            {
                if (i1.Type() == EVariableType.Series) di1.SetData(gt, ((Series)i1).GetData(smpl, gt));
                else di1.SetData(gt, ((ScalarVal)i1).val);

                if (i2.Type() == EVariableType.Series) di2.SetData(gt, ((Series)i2).GetData(smpl, gt));
                else di2.SetData(gt, ((ScalarVal)i2).val);

                if (o1.Type() == EVariableType.Series) do1.SetData(gt, ((Series)o1).GetData(smpl, gt));
                else do1.SetData(gt, ((ScalarVal)o1).val);

                if (o2.Type() == EVariableType.Series) do2.SetData(gt, ((Series)o2).GetData(smpl, gt));
                else do2.SetData(gt, ((ScalarVal)o2).val);

            }
            
            string x = O.ConvertToString(op).Trim();

            foreach (GekkoTime gt in smpl.Iterate12())
            {

                if (x == "==")
                {
                    if (di1.GetData(smpl, gt) == di2.GetData(smpl, gt))
                    {                        
                        result.SetData(gt, do1.GetData(smpl, gt));
                    }
                    else
                    {                        
                        result.SetData(gt, do2.GetData(smpl, gt));
                    }
                }
                else if (x == "<>")
                {
                    if (di1.GetData(smpl, gt) != di2.GetData(smpl, gt))
                    {                        
                        result.SetData(gt, do1.GetData(smpl, gt));
                    }
                    else
                    {                        
                        result.SetData(gt, do2.GetData(smpl, gt));
                    }
                }
                else if (x == ">")
                {
                    if (di1.GetData(smpl, gt) > di2.GetData(smpl, gt))
                    {                        
                        result.SetData(gt, do1.GetData(smpl, gt));
                    }
                    else
                    {                        
                        result.SetData(gt, do2.GetData(smpl, gt));
                    }
                }
                else if (x == ">=")
                {
                    if (di1.GetData(smpl, gt) >= di2.GetData(smpl, gt))
                    {                        
                        result.SetData(gt, do1.GetData(smpl, gt));
                    }
                    else
                    {                        
                        result.SetData(gt, do2.GetData(smpl, gt));
                    }
                }
                else if (x == "<")
                {
                    if (di1.GetData(smpl, gt) < di2.GetData(smpl, gt))
                    {                        
                        result.SetData(gt, do1.GetData(smpl, gt));
                    }
                    else
                    {                        
                        result.SetData(gt, do2.GetData(smpl, gt));
                    }
                }
                else if (x == "<=")
                {
                    if (di1.GetData(smpl, gt) <= di2.GetData(smpl, gt))
                    {                        
                        result.SetData(gt, do1.GetData(smpl, gt));
                    }
                    else
                    {                        
                        result.SetData(gt, do2.GetData(smpl, gt));
                    }
                }
                else
                {
                    G.Writeln2("*** ERROR: iif(): Expected operator '==', '<>', '<', '<=', '>' or '>='");
                    throw new GekkoException();
                }
            }
            return result;            
        }

        public static IVariable data(GekkoSmpl smpl, IVariable x)
        {
            string s = O.ConvertToString(x);
            string[] ss = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List m = new List();
            foreach (string s2 in ss)
            {
                double d = Functions.HelperValConvertFromString(s2);
                m.Add(new ScalarVal(d));
            }
            return m;
        }

        //!! practical for empty lists and singletons, for instance #m = list(); or #m = list('a');
        //!! for other cases, ('a', 'b') is shorter than list('a', 'b') but yields the same.
        public static IVariable list(GekkoSmpl smpl, params IVariable[] x)
        {
            if (x == null || x.Length == 0)
            {
                //empty list
                List rv = new List();
                rv.list = new List<IVariable>();
                return rv;
            }
            else
            {
                List rv = new List(new List<IVariable>(x));
                return rv;
            }
        }

        public static IVariable map(GekkoSmpl smpl)
        {
            //empty map
            Map rv = new Map();
            rv.storage = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);
            return rv;
        }

        public static IVariable log(GekkoSmpl smpl, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            IVariable rv = null;
            if (x1.Type() == EVariableType.Val)
            {
                rv = new ScalarVal(Math.Log(x1.ConvertToVal()));
            }
            else if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeries(smpl, x1 as Series, Globals.arithmentics1[2]); // (x1) => Math.Log(x1);
            }
            else if (x1.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x1);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Log(m.data[i, j]);
                    }
                }
            }
            else
            {
                G.Writeln2("*** ERROR: log(): type " + x1.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            return rv;            
        }

        public static IVariable exp(GekkoSmpl smpl, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            IVariable rv = null;
            if (x1.Type() == EVariableType.Val)
            {
                double d = O.ConvertToVal(x1);
                rv = new ScalarVal(Math.Exp(d));
            }
            else if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeries(smpl, x1 as Series, Globals.arithmentics1[3]); // (x1) => Math.Exp(x1);
            }
            else if (x1.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x1);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Exp(m.data[i, j]);
                    }
                }
            }
            else
            {
                G.Writeln2("*** ERROR: exp(): type " + x1.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            return rv;
        }

        public static IVariable sqrt(GekkoSmpl smpl, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            IVariable rv = null;
            if (x1.Type() == EVariableType.Val)
            {
                double d = O.ConvertToVal(x1);
                rv = new ScalarVal(Math.Sqrt(d));
            }
            else if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeries(smpl, x1 as Series, Globals.arithmentics1[4]); // (x1) => Math.Sqrt(x1);
            }
            else if (x1.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x1);
                rv = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        ((Matrix)rv).data[i, j] = Math.Sqrt(m.data[i, j]);
                    }
                }
            }
            else
            {
                G.Writeln2("*** ERROR: abs(): type " + x1.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            return rv;
        }

        private static bool IsValOrTimeseries(IVariable x)
        {
            return x.Type() == EVariableType.Val || x.Type() == EVariableType.Series;
        }        

        //same as power()
        public static IVariable pow(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            if (G.IsGekkoNull(x1)) return x1;
            return O.Power(smpl, x1, x2);            
        }

        //same as pow()
        public static IVariable power(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            return pow(smpl, x1, x2);
        }

        public static IVariable nothing(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable x1)
        {            
            Program.RevertSmpl(smplOriginal, smpl);
            return x1;
        }

        [MyCustom(Lag = "lag=1")]
        public static IVariable pch(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smplOriginal, smpl);
            if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeriesLag(smpl, x1 as Series, Globals.arithmentics[10]);  //(x, x.1) => (x / x.1 - 1d) * 100d;                
            }
            else
            {
                G.Writeln2("*** ERROR: pch() function only valid for time series arguments");
                throw new GekkoException();
            }
            return null;
        }

        public static IVariable seq(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            List<IVariable> mm = new List<IVariable>();
            if (x1.Type() == EVariableType.Val && x2.Type() == EVariableType.Val)
            {
                int i1 = O.ConvertToInt(x1);
                int i2 = O.ConvertToInt(x2);
                //List m = new List()                
                for (int i = i1; i <= i2; i++)
                {
                    ScalarVal d = new ScalarVal(i);
                    mm.Add(d);
                }
            }
            else if (x1.Type() == EVariableType.Date && x2.Type() == EVariableType.Date)
            {
                GekkoTime i1 = O.ConvertToDate(x1);
                GekkoTime i2 = O.ConvertToDate(x2);
                if (GekkoTime.Observations(i1, i2) > 0)
                {
                    foreach (GekkoTime t in new GekkoTimeIterator(i1, i2))  //fails with error if different freqs
                    {
                        mm.Add(new ScalarDate(t));
                    }
                }
            }
            else
            {
                G.Writeln2("*** ERROR: seq() must be fed with two values or two dates");
                throw new GekkoException();
            }
            return new List(mm);
        }

        [MyCustom(Lag = "lag=1")]
        public static IVariable dlog(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smplOriginal, smpl);
            if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeriesLag(smpl, x1 as Series, Globals.arithmentics[11]); // (x, x.1) => Math.Log(x / x.1);
            }
            else
            {
                G.Writeln2("*** ERROR: pch() function only valid for time series arguments");
                throw new GekkoException();
            }
            return null;
        }

        [MyCustom(Lag = "lag=1")]
        public static IVariable diff(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable x1)
        {
            return dif(smplOriginal, smpl, x1);
        }


        [MyCustom(Lag = "lag=1")]
        public static IVariable dif(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smplOriginal, smpl);
            if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeriesLag(smpl, x1 as Series, Globals.arithmentics[2]); // (x, x.1) => x - x.1;
            }
            else
            {
                G.Writeln2("*** ERROR: pch() function only valid for time series arguments");
                throw new GekkoException();
            }
            return null;
        }

        [MyCustom(Lag = "lag=[2]")]  //remember Program.RevertSmpl(), remember: -1-based, starts at -1, then 0, then 1, ...
        public static IVariable lag(GekkoSmpl2 smpl2, GekkoSmpl smpl, IVariable x1, IVariable ilag)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smpl2, smpl);
            return O.Indexer(smpl2, smpl, O.EIndexerType.IndexerLag, x1, O.Negate(smpl, ilag));
        }

        [MyCustom(Lag = "lag=[2]-1")]  //remember Program.RevertSmpl(), remember: -1-based, starts at -1, then 0, then 1, ...
        public static IVariable movsum(GekkoSmpl2 smpl2, GekkoSmpl smpl, IVariable x1, IVariable ilags)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smpl2, smpl);
            return MovAvgSum(smpl, x1, ilags, false);            
        }

        [MyCustom(Lag = "lag=[2]-1")]  //remember Program.RevertSmpl(), remember: -1-based, starts at -1, then 0, then 1, ...
        public static IVariable movavg(GekkoSmpl2 smpl2, GekkoSmpl smpl, IVariable x1, IVariable ilags)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smpl2, smpl);
            return MovAvgSum(smpl, x1, ilags, true);
        }

        private static IVariable MovAvgSum(GekkoSmpl smpl, IVariable x, IVariable ilags, bool avg)
        {
            IVariable rv = null;
            int d = O.ConvertToInt(ilags);
            double divide = 1d;
            if (avg) divide = (double)d;
            if (x.Type() == EVariableType.Series)
            {
                Series ts = (Series)x;
                Series z = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                foreach (GekkoTime gt in smpl.Iterate03())
                {
                    double sum = 0d;
                    for (int i = 0; i < d; i++)   //movsum(x, 2) is m + x[-1], so d is always the number of elements.
                    {
                        sum += ts.GetData(smpl, gt.Add(-i));
                    }
                    z.SetData(gt, sum / divide);
                }
                rv = z;
            }
            else if (x.Type() == EVariableType.Val)
            {
                rv = new ScalarVal(d / divide * ((ScalarVal)x).val);
            }
            else
            {
                G.Writeln2("*** ERROR: movsum() only works for SERIES");
                throw new GekkoException();
            }

            return rv;
        }
               
        public static IVariable pchy(GekkoSmpl t, IVariable x1)
        {
            return null;
        }
               
        public static IVariable dlogy(GekkoSmpl t, IVariable x1)
        {
            return null;
        }
                
        public static IVariable dify(GekkoSmpl t, IVariable x1)
        {
            return null;
        }
        
        public static IVariable format(GekkoSmpl t, IVariable x1, IVariable x2)
        {
            double d = O.ConvertToVal(x1); //#875324397
            string format2 = O.ConvertToString(x2);
            string x = Program.NumberFormat(d, format2);
            ScalarString ss = new ScalarString(x);
            return ss;
        }

        public static IVariable round(GekkoSmpl smpl, IVariable x1, IVariable round)
        {
            if (G.IsGekkoNull(x1)) return x1;
            double d2 = O.ConvertToVal(round);          
            int aaa1 = 0;
            if (!G.ConvertToInt(out aaa1, d2))
            {
                G.Writeln("*** ERROR: Could not convert decimals variable to integer");
                throw new GekkoException();
            }
            int decimals = aaa1;
            if (decimals < 0)
            {
                G.Writeln2("*** ERROR: number of decimals in round() must be positive");
                throw new GekkoException();
            }
                        
            if (x1.Type() == EVariableType.Val)
            {
                double d = O.ConvertToVal(x1);
                return new ScalarVal(Math.Round(d, decimals, MidpointRounding.AwayFromZero));
            }
            else if (x1.Type() == EVariableType.Series)
            {                  
                return Series.ArithmeticsSeriesVal(smpl, x1 as Series, decimals, Globals.arithmentics[12]);  //(x1, x2) => Math.Round(x1, (int)x2);                
            }
            else if (x1.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x1);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Round(m.data[i, j], decimals, MidpointRounding.AwayFromZero);
                    }
                }
                return m2;
            }
            else
            {
                G.Writeln2("*** ERROR: round() does not support type " + x1.Type().ToString());
                throw new GekkoException();
            }
        }

        public static IVariable search(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            string s1 = O.ConvertToString(x1);
            string s2 = O.ConvertToString(x2);
            int i = s1.IndexOf(s2);            
            return new ScalarVal(i + 1);
        }

        public static IVariable startswith(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            string s1 = O.ConvertToString(x1);
            string s2 = O.ConvertToString(x2);
            int i = 0;
            if (s1.StartsWith(s2, StringComparison.OrdinalIgnoreCase)) i = 1;
            return new ScalarVal(i);
        }

        public static IVariable endswith(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            string s1 = O.ConvertToString(x1);
            string s2 = O.ConvertToString(x2);
            int i = 0;
            if (s1.EndsWith(s2, StringComparison.OrdinalIgnoreCase)) i = 1;
            return new ScalarVal(i);
        }

        public static IVariable getfile(GekkoSmpl smpl, IVariable x1)
        {
            string s1 = O.ConvertToString(x1);
            string txt = Program.GetTextFromFileWithWait(s1);
            return new ScalarString(txt);
        }

        public static IVariable strip(GekkoSmpl smpl, IVariable x1)
        {            
            string s1 = O.ConvertToString(x1);
            return new ScalarString(s1.Trim());
        }

        public static IVariable stripstart(GekkoSmpl smpl, IVariable x1)
        {
            string s1 = O.ConvertToString(x1);
            return new ScalarString(s1.TrimStart());
        }

        public static IVariable stripend(GekkoSmpl smpl, IVariable x1)
        {
            string s1 = O.ConvertToString(x1);            
            return new ScalarString(s1.TrimEnd());
        }

        public static IVariable upper(GekkoSmpl smpl, IVariable ths)
        {
            //does not exist in AREMOS

            if (ths.Type() == EVariableType.String)
            {
                string s1 = O.ConvertToString(ths);
                string s4 = s1.ToUpper();
                return new ScalarString(s4);
            }
            else if (ths.Type() == EVariableType.List)
            {
                List m = ths as List;                
                List tmp = new List();
                foreach (IVariable iv in m.list)
                {                    
                    if (iv.Type() == EVariableType.String)
                    {                        
                        tmp.Add(new ScalarString(iv.ConvertToString().ToUpper()));
                    }
                    else
                    {
                        tmp.Add(iv);
                    }
                }
                return tmp;
            }
            else
            {
                FunctionError("upper", ths);  //throws exception
                return null;
            }
        }

        public static IVariable lower(GekkoSmpl smpl, IVariable ths)
        {
            //does not exist in AREMOS

            if (ths.Type() == EVariableType.String)
            {
                string s1 = O.ConvertToString(ths);
                string s4 = s1.ToLower();
                return new ScalarString(s4);
            }
            else if (ths.Type() == EVariableType.List)
            {
                List m = ths as List;
                List tmp = new List();
                foreach (IVariable iv in m.list)
                {
                    if (iv.Type() == EVariableType.String)
                    {
                        tmp.Add(new ScalarString(iv.ConvertToString().ToLower()));
                    }
                    else
                    {
                        tmp.Add(iv);
                    }
                }
                return tmp;
            }
            else
            {
                FunctionError("upper", ths);  //throws exception
                return null;
            }
        }

        public static IVariable sort(GekkoSmpl smpl, IVariable x1)
        {
            List<IVariable> tmp = O.ConvertToList(x1);
            List<string> xx = new List<string>(Program.GetListOfStringsFromListOfIvariables(tmp.ToArray()));
            xx.Sort(StringComparer.OrdinalIgnoreCase);
            List m = new List();
            foreach (string s in xx)
            {
                m.Add(new ScalarString(s));
            }
            return m;
        }

        public static IVariable flatten(GekkoSmpl smpl, IVariable x1)
        {
            if (x1.Type() == EVariableType.List)
            {                
                return O.ExplodeIvariables(x1);
            }
            else
            {
                return x1;  //not touched
            }
        }

        public static IVariable unique(GekkoSmpl smpl, IVariable x1)
        {
            List<IVariable> tmp = O.ConvertToList(x1);
            List<string> xx = new List<string>(Program.GetListOfStringsFromListOfIvariables(tmp.ToArray()));
            List m = new List();
            GekkoDictionary<string, string> dict = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in xx)
            {
                if (!dict.ContainsKey(s))
                {
                    dict.Add(s, null);
                    m.Add(new ScalarString(s));
                }
                else
                {
                    //ignore
                }
            }            
            
            return m;
        }

        public static IVariable strip(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            return replace(smpl, x1, x2, new ScalarString(""));
        }

        public static IVariable tostring(GekkoSmpl smpl, IVariable x)  //'string' not allowed as method name
        {
            string s = null;
            if (x.Type() == EVariableType.Val)
            {
                double v = ((ScalarVal)x).val;
                if (G.isNumericalError(v)) s = "M";
                else s = v.ToString();
            }
            else if (x.Type() == EVariableType.Date)
            {
                GekkoTime gt = ((ScalarDate)x).date;
                s = G.FromDateToString(gt);
            }
            else if (x.Type() == EVariableType.String)
            {
                s = ((ScalarString)x).string2;  //maybe could just return x here, but maybe that is not safe
            }
            else if (x.Type() == EVariableType.List)
            {
                List<IVariable> l = ((List)x).list;
                foreach (IVariable iv in l)
                {
                    IVariable iv2 = tostring(smpl, iv);
                    string ss = O.ConvertToString(iv2);
                    s += ss + ", ";
                }                
                if (s.EndsWith(", ")) s = s.Substring(0, s.Length - 2);
            }
            else if (x.Type() == EVariableType.Series)
            {
                G.Writeln2("*** ERROR: Cannot convert a SERIES to a STRING");
                throw new GekkoException();
            }
            return new ScalarString(s);
        }

        public static IVariable strings(GekkoSmpl smpl, IVariable x)
        {
            string s = null;
            if (x.Type() == EVariableType.List)
            {
                List m = x as List;
                List<IVariable> m2 = new List<IVariable>();
                foreach (IVariable iv in m.list)
                {
                    IVariable iv2 = tostring(smpl, iv);
                    m2.Add(iv2);
                }
                return new List(m2);
            }
            else 
            {
                G.Writeln2("*** ERROR: Expected a LIST variable as argument");
                throw new GekkoException();
            }
            return new ScalarString(s);
        }

        public static IVariable vals(GekkoSmpl smpl, IVariable x)
        {
            string s = null;
            if (x.Type() == EVariableType.List)
            {
                List m = x as List;
                List<IVariable> m2 = new List<IVariable>();
                foreach (IVariable iv in m.list)
                {
                    IVariable iv2 = val(smpl, iv);
                    m2.Add(iv2);
                }
                return new List(m2);
            }
            else
            {
                G.Writeln2("*** ERROR: Expected a LIST variable as argument");
                throw new GekkoException();
            }
            return new ScalarString(s);
        }

        public static IVariable dates(GekkoSmpl smpl, IVariable x)
        {
            string s = null;
            if (x.Type() == EVariableType.List)
            {
                List m = x as List;
                List<IVariable> m2 = new List<IVariable>();
                foreach (IVariable iv in m.list)
                {
                    IVariable iv2 = date(smpl, iv);
                    m2.Add(iv2);
                }
                return new List(m2);
            }
            else
            {
                G.Writeln2("*** ERROR: Expected a LIST variable as argument");
                throw new GekkoException();
            }
            return new ScalarString(s);
        }

        public static IVariable date(GekkoSmpl smpl, IVariable x)  //'string' not allowed as method name
        {
            GekkoTime d = GekkoTime.tNull;
            if (x.Type() == EVariableType.Val)
            {
                d = O.ConvertToDate(x);  //already has auto-conversion from VAL to DATE
            }
            else if (x.Type() == EVariableType.Date)
            {
                d = ((ScalarDate)x).date;                
            }
            else if (x.Type() == EVariableType.String)
            {
                string s = ((ScalarString)x).string2;
                d = G.FromStringToDate(s);
            }
            else if (x.Type() == EVariableType.List)
            {
                G.Writeln2("*** ERROR: Cannot convert a LIST to a DATE");
                throw new GekkoException();
            }
            else if (x.Type() == EVariableType.Series)
            {
                G.Writeln2("*** ERROR: Cannot convert a SERIES to a DATE");
                throw new GekkoException();
            }
            return new ScalarDate(d);
        }

        public static IVariable val(GekkoSmpl smpl, IVariable x1)  //'string' not allowed as method name
        {
            if (G.IsGekkoNull(x1)) return x1;
            double v = double.NaN;
            if (x1.Type() == EVariableType.Val)
            {
                v = ((ScalarVal)x1).val;
            }
            else if (x1.Type() == EVariableType.Date)
            {
                ScalarDate sd = (ScalarDate)x1;
                if (sd.date.freq == EFreq.A || sd.date.freq == EFreq.U)
                {
                    v = sd.date.super;
                }
                else
                {
                    G.Writeln2("*** ERROR: Cannot only convert annual or undated DATE to VAL");
                    throw new GekkoException();
                }
            }
            else if (x1.Type() == EVariableType.String)
            {
                string s = ((ScalarString)x1).string2;
                v = HelperValConvertFromString(s);
            }
            else if (x1.Type() == EVariableType.List)
            {
                G.Writeln2("*** ERROR: Cannot convert a LIST to a VAL");
                throw new GekkoException();
            }
            else if (x1.Type() == EVariableType.Series)
            {
                G.Writeln2("*** ERROR: Cannot convert a SERIES to a DATE");
                throw new GekkoException();
            }
            return new ScalarVal(v);
        }

        private static double HelperValConvertFromString(string s)
        {
            double v;
            s = s.Trim();
            if (G.Equal(s, "m") || G.Equal(s, "m()") || G.Equal(s, "miss()") || G.Equal(s, "nan"))
            {
                v = double.NaN;
            }
            else
            {
                if (G.TryParseIntoDouble(s, out v))
                {
                    //ok
                }
                else
                {
                    G.Writeln2("*** ERROR: Could not convert STRING '" + s + "' to VAL");
                    throw new GekkoException();
                }
            }

            return v;
        }

        public static IVariable replaceinside(GekkoSmpl smpl, IVariable ths, IVariable x2, IVariable x3, IVariable max)
        {
            return replace(smpl, ths, x2, x3, true, max);
        }

        public static IVariable replaceinside(GekkoSmpl smpl, IVariable ths, IVariable x2, IVariable x3)
        {            
            return replace(smpl, ths, x2, x3, true, Globals.scalarVal0);
        }

        public static IVariable replace(GekkoSmpl smpl, IVariable ths, IVariable x2, IVariable x3, IVariable max)
        {
            return replace(smpl, ths, x2, x3, false, max);
        }                

        public static IVariable replace(GekkoSmpl smpl, IVariable ths, IVariable x2, IVariable x3)
        {
            return replace(smpl, ths, x2, x3, false, Globals.scalarVal0);
        }

        public static IVariable replace(GekkoSmpl smpl, IVariable ths, IVariable x2, IVariable x3, bool isInside, IVariable max)
        {
            int imax = O.ConvertToInt(max);

            if (ths.Type() == EVariableType.String)
            {
                if (isInside)
                {
                    G.Writeln2("*** ERROR: replaceinside() is for list argument only");
                    throw new GekkoException();
                }
                string s1 = O.ConvertToString(ths);
                string s2 = O.ConvertToString(x2);
                string s3 = O.ConvertToString(x3);
                string s4 = G.Replace(s1, s2, s3, StringComparison.OrdinalIgnoreCase, imax);
                //string s4 = Regex.Replace(s1, s2, s3, RegexOptions.IgnoreCase);
                return new ScalarString(s4);
            }
            else if (ths.Type() == EVariableType.List)
            {
                List m = ths as List;
                string s2 = O.ConvertToString(x2);
                string s3 = O.ConvertToString(x3);
                List tmp = new List();
                foreach (IVariable iv in m.list)
                {
                    bool hit = false;
                    if (iv.Type() == EVariableType.String)
                    {
                        string s = O.ConvertToString(iv);
                        if (isInside)
                        {
                            if (G.Contains(s, s2))
                            {
                                hit = true;
                                tmp.Add(new ScalarString(G.Replace(s, s2, s3, StringComparison.OrdinalIgnoreCase, imax)));
                                //tmp.Add(new ScalarString(Regex.Replace(s, s2, s3, RegexOptions.IgnoreCase)));                                
                            }
                        }
                        else
                        {
                            if (imax <= 0 || imax == int.MaxValue)
                            {
                                //good
                            }
                            else
                            {
                                G.Writeln2("*** ERROR: You cannot use max argument with replace() on a list");
                                throw new GekkoException();
                            }
                            if (G.Equal(s, s2))
                            {
                                hit = true;
                                tmp.Add(new ScalarString(s3));
                            }
                        }
                    }
                    if (!hit) tmp.Add(iv);
                }
                return tmp;
            }
            else if (ths.Type() == EVariableType.Series)
            {
                if (isInside == false && max.ConvertToVal() == 0d)
                {
                    Series ths_series = ths as Series;
                    
                    //good
                    double d2 = O.ConvertToVal(x2);
                    double d3 = O.ConvertToVal(x3);

                    Series lhs = new Series(ESeriesType.Light, smpl.t0, smpl.t3);

                    if (ths_series.type == ESeriesType.ArraySuper)
                    {
                        G.Writeln2("*** ERROR: Replace(): you cannot use array-series as argument");
                        throw new GekkoException();
                    }

                    if (ths_series.type == ESeriesType.Timeless)
                    {
                        G.Writeln2("*** ERROR: Replace(): you cannot use timeless series as argument");
                        throw new GekkoException();
                    }

                    foreach (GekkoTime t in smpl.Iterate12())
                    {
                        //will only replace for current sample, not outside of it! So PRT dif(replace(x, m(), 1)) may spring a surprise since relpace does not replace before sample start. But not intended for that though.
                        double d = ths_series.GetDataSimple(t);
                        if (double.IsNaN(d2) && double.IsNaN(d))
                        {
                            lhs.SetData(t, d3);
                        }
                        else if (d == d2)
                        {
                            lhs.SetData(t, d3);
                        }
                        else
                        {
                            //replicate
                            lhs.SetData(t, d);
                        }
                    }

                    return lhs;

                }
                else
                {
                    G.Writeln2("*** ERROR: Replace(): you cannot use series type with 'inside' or 'max'");
                    throw new GekkoException();
                }
            }
            else
            {
                FunctionError("replace", ths);  //throws exception
                return null;
            }
        }

        public static IVariable gekkoversion(GekkoSmpl smpl)
        {            
            return new ScalarString(Globals.gekkoVersion);
        }

        public static IVariable currentfreq(GekkoSmpl smpl)
        {
            return new ScalarString(G.GetFreq(Program.options.freq));
        }

        public static IVariable currentperstart(GekkoSmpl smpl)
        {
            return new ScalarDate(Globals.globalPeriodStart);
        }

        public static IVariable currentperend(GekkoSmpl smpl)
        {
            return new ScalarDate(Globals.globalPeriodEnd);
        }

        public static IVariable currentdatetime(GekkoSmpl smpl)
        {
            return new ScalarString(Program.GetDateTimeStamp());
        }

        public static IVariable currenttime(GekkoSmpl smpl)
        {
            return new ScalarString(Program.GetTimeStamp());
        }

        public static IVariable currentdate(GekkoSmpl smpl)
        {
            //See also #80927435209843
            return new ScalarString(Program.GetDateStamp());
        }

        public static IVariable filteredperiods(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            GekkoTime t1 = GekkoTime.tNull;
            GekkoTime t2 = GekkoTime.tNull;
            try
            {
                t1 = O.ConvertToDate(x1);
                t2 = O.ConvertToDate(x2);
            }
            catch
            {
                G.Writeln("*** ERROR: Function 'filteredperiods' takes two dates as arguments.");
                throw new GekkoException();
            }

            ScalarVal z2 = null;

            if (Globals.globalPeriodTimeFilters2.Count == 0)
            {
                //for a quick return in most cases
                z2 = Globals.scalarVal0;
            }
            else
            {
                int counter = 0;
                foreach (GekkoTime gt in new GekkoTimeIterator(t1, t2))
                {
                    if (Program.ShouldFilterPeriod(gt)) counter++;
                }
                z2 = new ScalarVal((double)counter);
            }
            return z2;
        }
        
        public static IVariable exist(GekkoSmpl smpl, IVariable x1)
        {
            double d = 0d;
            IVariable y = O.Lookup(smpl, null, x1, null, new LookupSettings(O.ELookupType.RightHandSide, O.ECreatePossibilities.NoneReportError, true), EVariableType.Var, false, null); //will use search settings (data, sim mode) if not bank is given
            if (y != null) d = 1d;            
            ScalarVal v = new ScalarVal(d);
            return v;
        }

        public static IVariable fromseries(GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            Series ts = null;

            if (x1.Type() == EVariableType.Series)
            {
                ts = x1 as Series;
            }
            else if (x1.Type() == EVariableType.String)
            {
                ts = O.GetIVariableFromString(x1.ConvertToString(), O.ECreatePossibilities.NoneReportError, true) as Series;
                //ts = O.Lookup(null, null, x1, null, O.ELookupType.RightHandSide, EVariableType.Var, null) as Series;
            }
            else
            {
                G.Writeln2("*** ERROR: fromseries(): expected first argument to be series or string type");
                throw new GekkoException();
            }

            string s2 = O.ConvertToString(x2);                       
            
            if (ts == null)
            {
                G.Writeln2("*** ERROR: Variable is not of series type");
                throw new GekkoException();
            }

            if (G.Equal(s2, "name"))
            {
                return new ScalarString(ts.name);
            }
            else if (G.Equal(s2, "bank"))
            {
                string s = "";
                string ss = ts?.meta?.parentDatabank.name;
                if (ss != null) s = ss;
                return new ScalarString(ss);
            }
            else if (G.Equal(s2, "label"))
            {
                return new ScalarString(ts.meta.label);
            }
            else if (G.Equal(s2, "source"))
            {
                return new ScalarString(ts.meta.source);
            }
            else if (G.Equal(s2, "stamp"))
            {
                return new ScalarString(ts.meta.stamp);
            }
            else if (G.Equal(s2, "units"))
            {
                return new ScalarString(ts.meta.units);
            }
            else if (G.Equal(s2, "perStart"))
            {
                return new ScalarDate(ts.GetPeriodFirst());
            }
            else if (G.Equal(s2, "dataStart"))
            {
                return new ScalarDate(ts.GetRealDataPeriodFirst());
            }
            else if (G.Equal(s2, "perEnd"))
            {
                return new ScalarDate(ts.GetPeriodLast());
            }
            else if (G.Equal(s2, "dataEnd"))
            {
                return new ScalarDate(ts.GetRealDataPeriodLast());
            }
            else if (G.Equal(s2, "freq"))
            {
                return new ScalarString(G.GetFreq(ts.freq));
            }
            else
            {
                G.Writeln2("*** ERROR: fromSeries(): Argument '" + s2 + "' not recognized.");
                throw new GekkoException();
            }
        }

        // -----------------------------------
        // LIST functions start
        // -----------------------------------

        public static IVariable union(GekkoSmpl t, IVariable x1, IVariable x2)
        {
            //tager dem der nu er i a (inkl. dubletter) og tilføjer dem fra b (uden dubletter). Hvis dubletter i b skal med, skal der bruges komma...
            List<string> lx1 = Program.GetListOfStringsFromList(x1);
            List<string> lx2 = Program.GetListOfStringsFromList(x2);
            List<string> union = new List<string>();
            union.AddRange(lx1);
            GekkoDictionary<string, bool> result = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in lx1)
            {
                if (!result.ContainsKey(s)) result.Add(s, true);
            }
            foreach (string s in lx2)
            {
                if (!result.ContainsKey(s))
                {
                    result.Add(s, true);
                    union.Add(s);
                }
            }
            //union.Sort(StringComparer.InvariantCulture);  //or maybe only sort when printing/reporting/iterating?
            return new List(union);
        }               

        public static IVariable except(GekkoSmpl t, IVariable x1, IVariable x2)
        {
            //tager dem der nu er i a (inkl. dubletter) og retainer dem hvis ikke i b.
            List<string> lx1 = Program.GetListOfStringsFromList(x1);
            List<string> lx2 = Program.GetListOfStringsFromList(x2);
            List<string> difference = new List<string>();
            GekkoDictionary<string, bool> temp = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in lx2)
            {
                if (!temp.ContainsKey(s)) temp.Add(s, true);
            }
            foreach (string s in lx1)
            {
                if (!temp.ContainsKey(s))
                {
                    difference.Add(s);
                }
            }
            //difference.Sort(StringComparer.InvariantCulture);  //or maybe only sort when printing/reporting/iterating?
            return new List(difference);
        }

        public static IVariable intersect(GekkoSmpl t, IVariable x1, IVariable x2)
        {
            //tager dem der nu er i a (inkl. dubletter) og retainer dem hvis også i b.
            List<string> lx1 = Program.GetListOfStringsFromList(x1);
            List<string> lx2 = Program.GetListOfStringsFromList(x2);
            List<string> intersection = new List<string>();
            if (lx1.Count > lx2.Count)  //for speedup, we do the heaviest looping on the smaller list.
            {
                ListMultiplyHelper(lx1, lx2, intersection);
            }
            else
            {
                ListMultiplyHelper(lx2, lx1, intersection);
            }
            //intersection.Sort(StringComparer.InvariantCulture);  //or maybe only sort when printing/reporting/iterating?
            return new List(intersection);
        }

        private static void ListMultiplyHelper(List<string> x1, List<string> x2, List<string> intersection)
        {
            GekkoDictionary<string, bool> temp = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in x1)
            {
                if (!temp.ContainsKey(s))  //there can be dublets
                {
                    temp.Add(s, true);
                }
            }
            GekkoDictionary<string, bool> result = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in x2)
            {
                if (temp.ContainsKey(s))
                {
                    if (!result.ContainsKey(s))
                    {
                        result.Add(s, true);
                    }
                }
            }
            intersection.AddRange(result.Keys);
        }

        // -----------------------------------
        // LIST functions end
        // -----------------------------------

        //SOME HARDCODED FUNCTIONS FOR MODELS:
        //See #09875209837532

        public static double CES_UC(double p1rel, double p2rel, double theta, double sigma)
        {
            double c = Math.Pow(theta * Math.Pow(p1rel, 1 - sigma) + (1 - theta) * Math.Pow(p2rel, 1 - sigma), 1 / (1 - sigma));
            return c;
        }

        public static double CES_XL(double yrel, double p1rel, double p2rel, double theta, double sigma)
        {
            double uc = CES_UC(p1rel, p2rel, theta, sigma);
            return yrel * Math.Pow(uc / p1rel, sigma);
        }

        public static double CES_XR(double yrel, double p1rel, double p2rel, double theta, double sigma)
        {
            double uc = CES_UC(p1rel, p2rel, theta, sigma);
            return yrel * Math.Pow(uc / p2rel, sigma);
        }

        //See also #9823750983752

        public static double ces_costs(double y, double p1, double p2, double kappa, double phi, double sigma)
        {
            return y / kappa * Math.Pow(Math.Pow((Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), sigma) * Math.Pow(p1, 1 - sigma) + Math.Pow(1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), sigma) * Math.Pow(p2, 1 - sigma), 1 / (1 - sigma));
        }

        public static double ces_ac(double p1, double p2, double kappa, double phi, double sigma)
        {
            return 1d / kappa * Math.Pow(Math.Pow((Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), sigma) * Math.Pow(p1, 1 - sigma) + Math.Pow(1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), sigma) * Math.Pow(p2, 1 - sigma), 1 / (1 - sigma));
        }

        public static double ces_factor1(double y, double p1, double p2, double kappa, double phi, double sigma)
        {
            return Math.Pow((Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), (sigma / (1 - sigma))) * y / kappa * Math.Pow((Math.Pow((p2 / p1), (1 - sigma)) * Math.Pow(((1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma)))) / (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma)))), sigma) + 1), (sigma / (1 - sigma)));
        }

        public static double ces_factor2(double y, double p1, double p2, double kappa, double phi, double sigma)
        {
            return Math.Pow((1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma)))), (sigma / (1 - sigma))) * y / kappa * Math.Pow((Math.Pow((p1 / p2), (1 - sigma)) * Math.Pow((((Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma)))) / (1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))))), sigma) + 1), (sigma / (1 - sigma)));
        }

        // ====================== object methods =======================================

        //see also the other append method
        public static IVariable append(GekkoSmpl smpl, IVariable ths, IVariable x)
        {
            //FIX: type checks etc.!
            List temp = ths.DeepClone(null) as List;
            temp.Add(x);
            return temp;
        }

        //see also the other append method
        public static IVariable append(GekkoSmpl smpl, IVariable ths, IVariable index, IVariable x)
        {
            //FIX: type checks etc.!
            int i = O.ConvertToInt(index, true);
            List temp = ths.DeepClone(null) as List;
            if (i - 1 < 0 || i - 1 > temp.list.Count)
            {
                G.Writeln2("*** ERROR: Cannot insert at position " + i);
                throw new GekkoException();
            }
            temp.list.Insert(i - 1, x);            
            return temp;
        }
                

        //see also the other extend() method
        public static IVariable extend(GekkoSmpl smpl, IVariable ths, IVariable index, IVariable x)
        {
            if (ths.Type() != EVariableType.List) FunctionError("extend", x);
            int i = O.ConvertToInt(index, true);
            List temp = ths.DeepClone(null) as List;
            if (i - 1 < 0 || i - 1 > temp.list.Count)
            {
                G.Writeln2("*** ERROR: Cannot insert at position " + i);
                throw new GekkoException();
            }
            if (x.Type() == EVariableType.List)
            {
                List x_list = x as List;
                temp = temp.DeepClone(null) as List;
                temp.list.InsertRange(i - 1, x_list.list);
            }
            else
            {
                FunctionError("extend", x);
            }
            return temp;
        }

        //see also the other extend() method
        public static IVariable extend(GekkoSmpl smpl, IVariable ths, IVariable x)
        {
            if (ths.Type() != EVariableType.List) FunctionError("extend", x);
            List temp = ths as List;
            if (x.Type() == EVariableType.List)
            {
                List x_list = x as List;                
                temp = temp.DeepClone(null) as List;
                temp.list.AddRange(x_list.list);
            }            
            else
            {                
                FunctionError("extend", x);
            }
            return temp;
        }

        //See also suffix()
        public static IVariable prefix(GekkoSmpl smpl, IVariable ths, IVariable x)
        {
            IVariable rv = null;
            string x_string = O.ConvertToString(x);
            if (ths.Type() == EVariableType.String)
            {
                string s = O.ConvertToString(ths);
                rv = new ScalarString(x_string + s);
            }
            else if (ths.Type() == EVariableType.List)
            {
                List<string> xx = new List<string>(Program.GetListOfStringsFromListOfIvariables((ths as List).list.ToArray()));
                List rv2 = new List();
                foreach (string s in xx)
                {
                    rv2.Add(new ScalarString(x_string + s));
                }
                rv = rv2;
            }
            else
            {
                FunctionError("prefix", ths);
                throw new GekkoException();
            }
            return rv;            
        }

        //See also prefix()
        public static IVariable suffix(GekkoSmpl smpl, IVariable ths, IVariable x)
        {
            IVariable rv = null;
            string x_string = O.ConvertToString(x);
            if (ths.Type() == EVariableType.String)
            {
                string s = O.ConvertToString(ths);
                rv = new ScalarString(s + x_string);
            }
            else if (ths.Type() == EVariableType.List)
            {
                List<string> xx = new List<string>(Program.GetListOfStringsFromListOfIvariables((ths as List).list.ToArray()));
                List rv2 = new List();
                foreach (string s in xx)
                {
                    rv2.Add(new ScalarString(s + x_string));
                }
                rv = rv2;
            }
            else
            {
                FunctionError("prefix", ths);
                throw new GekkoException();
            }
            return rv;
        }

        

        private static void FunctionError(string s, IVariable x)
        {
            G.Writeln2("*** ERROR: Object method ." + s + "() does not allow a " + G.GetTypeString(x) + " variable");
            throw new GekkoException();
        }

        //public static IVariable extend_naked(GekkoSmpl smpl, List ths, IVariable x)
        //{
        //    List x_list = x as List;
        //    if (x_list == null)
        //    {
        //        G.Writeln2("*** ERROR: Object method .extend() expects a LIST argument, got " + G.GetTypeString(x));
        //        throw new GekkoException();
        //    }
        //    ths.list.AddRange(x_list.list);
        //    return new GekkoNull();
        //}

    }
}


