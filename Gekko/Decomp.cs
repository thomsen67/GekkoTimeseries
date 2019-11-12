using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Gekko
{
    class Decomp
    {
        public static void DecompStart(O.Decomp2 o)
        {
            //This is the starting point
            //  O.Decomp2.Decomp2Helper                  --> may use Program.DecompEvalGams() or Program.DecompEval(), with I("EVAL ...") 
            //                                               DecompEvalGams() finds the equation, translates to Gekko, and returns a 
            //                                               ModelGamsEquation object with element.expressions containing the expression(s).
            //                                               In an eq like y[#i] = 2*x[#i], n expressions are returned corresponding to the elements of #i
            //  WindowDecomp.RecalcCellsWithNewType();
            //  O.Decomp2.Decompose2()                   --> actual calculation of data, expression(s) is argument
            //  Program.DecomposePutIntoTable2()         --> putting the data into a table
            //  WindowDecomp.MakeGuiTable2()             --> shows the table in GUI
            //
            //CLICKING: Mouse_Down(), cf. #98732498724
            //        
            // Consider this: e1   y[#a] = x1[#a] + x2[#a] + z;    #9807532957234
            //                e2   x1[#a] = b1 * u[#a];
            //                e3   x2[#a] = b2 * u[#a-1];
            //                e4   z = sum(#a, u[#a]):
            //
            //decomp <d> y[#a] in e1 link x1[#a] in e2, x2[#a] in e3, z in e4;                
            //
            // with #a = 20, 21, we have:
            //
            //        e1a       y[20] = x1[20] + x2[20] + z; <----+----------+   z goes into both
            //        e1b       y[21] = x1[21] + x2[21] + z; <----|----+-----+  
            //        e2a       x1[20] = b1 * u[20]; ---->--------+    |     |
            //        e2b       x1[21] = b1 * u[21]; ---->--------|----+     |
            //        e3a       x2[20] = b2 * u[19]; ---->--------+    |     |
            //        e3b       x2[21] = b2 * u[20]; ---->-------------+     |
            //        e4        z = u[20] + u[21];   ---->-------------------+
            //
            //decomp <d> y[20],y[21] in e1 link x1[20],x1[21] in e2, x2[20],x2[21] in e3, z in e4;                

            //In this: x[#a] = x1[#a] + x2[#a], #a = 20, 21, there is the uncontrolled set #a. We cannot be sure 
            //that all indexes [20] is the first equation, since there may be lags #a-1, or even single equations like
            //x1[20] = y[21]... . So in reality we have the equation x[20] = y[21] ... , and effects do not sum to
            //zero over each age. We may also have x[#i, #j] = ... where #i and #j are uncontrolled, for instance producing
            //the equations x[i1, j1], x[i1, j3], x[i2, j1], x[i2, j2]. So we could do "eq x[i1, j1]" to identify one of
            //the equations. In a lot of cases, like x[#a] = b[#a]*u[#a], the equations would match a set, so that in the
            //raw datatable, the column with eq and #a would correlate 100%. In that case, we could just eliminate the "eq...".
            //Perhaps also "eq x[i1, j1]", ... would sometimes correlate 100% with #i and #j, and in that case we could eliminate
            //the "eq..." too.
            //                    eq              #i            #j
            //                  -----------------------------------------
            //              eq x[i1, j1]          i1            j1
            //              eq x[i1, j2]          i1            j2
            //              eq x[i2, j1]          i2            j1

            //When linking, we do not link the sub-equations together, we only link into the primary equation
            //one by one (so link order can be important). In the above example, if we only want to look at
            //y[20], calculating e1b, e2b and e3b is a waste of time. If lazy eval was used, these would
            //never be calculated, and can they be omitted? That is probably hard, for instance the e2a equation
            //could depend on x2[21], and then we need the decomposed e3b equation. For instance this:
            //
            //                e1   y[#a] = x1[#a] + x2[#a];   links with x1[#a] and x2[#a]
            //                e2   x1[#a] = b1 * x2[#a+1];
            //                e3   x2[#a] = b2 * u[#a];       
            //
            //                e1a   y[20] = x1[20] + x2[20];
            //                e1b   y[21] = x1[21] + x2[21];
            //                e2a   x1[20] = b1 * x2[21]; -------- points to this one --------+
            //                e2b   x1[21] = b1 * x2[22];                                     |
            //                e3a   x2[20] = b2 * u[20];                                      |
            //                e3b   x2[21] = b2 * u[21];  <-----------------------------------+
            //
            // When e1 is linked with e2, we get a #a-lead in e1, and therefore we need e3b in the 
            // calculation. So it seems this problem is "hard", because we never really know the sets
            // (could in principle depend on a function, like x1[#a] = b1 * x2[randomlead(#a)], so
            // it seems a bit similar to the lag problem.
            // Also, e2 might be x1[#a2] = b1 * x2[#a2], where #a2 is an unknown set. In that case, how
            // to know which of the equations e2a or e2b is relevant, if #a2 = 21, 22...?
            // If we are calculating everything (all 7 unfolded eqs in the original example), the good thing
            // is that it will be switch between looking at e1a or e1b since everything is pre-calculated.
            //

            Globals.lastDecompTable = null;
            G.CheckLegalPeriod(o.t1, o.t2);
            if (G.NullOrEmpty(o.opt_prtcode)) o.opt_prtcode = "n";

            if (true)
            {                

                DecompOptions2 decompOptions2 = new DecompOptions2();
                decompOptions2.t1 = o.t1;
                decompOptions2.t2 = o.t2;
                decompOptions2.expressionOld = o.label;
                decompOptions2.expression = o.expression;
                decompOptions2.prtOptionLower = o.opt_prtcode.ToLower();
                decompOptions2.name = o.name;
                decompOptions2.isNew = true;

                if (o.rows.Count > 0) decompOptions2.rows = O.Restrict(o.rows[0] as List, false, true, false, false);
                if (o.cols.Count > 0) decompOptions2.cols = O.Restrict(o.cols[0] as List, false, true, false, false);

                decompOptions2.type = o.type;

                foreach (List<IVariable> liv in o.where)
                {
                    //'a' in #i
                    string x1 = O.ConvertToString(liv[0]);
                    List<string> x2 = O.Restrict(liv[1] as List, false, true, false, false);
                    decompOptions2.where.Add(new List<string>() { x1, x2[0] });
                }

                foreach (List<IVariable> liv in o.group)
                {
                    //
                    List<string> x1 = O.Restrict(liv[0] as List, false, true, false, false);
                    List<string> x2 = O.Restrict(liv[1] as List, false, true, false, false);
                    string x3 = O.ConvertToString(liv[2]);
                    string x4 = O.ConvertToString(liv[3]);
                    decompOptions2.group.Add(new List<string>() { x1[0], x2[0], x3, x4 });
                }

                foreach (DecompItems liv in o.decompItems)
                {
                    //
                    List<string> x1 = O.Restrict(liv.varnames as List, false, true, false, true);
                    List<string> x2 = O.Restrict(liv.eqname as List, false, true, false, false);
                    Link temp = new Link();
                    if (x1 != null)
                    {
                        temp.varnames = new List<string>();
                        temp.varnames.AddRange(x1);
                    }
                    if (x2 != null) temp.eqname = x2[0];
                    temp.expressions = new List<Func<GekkoSmpl, IVariable>>() { liv.expression };
                    decompOptions2.link.Add(temp);
                }

                CrossThreadStuff.Decomp2(decompOptions2);

                //Also see #9237532567
                //This stuff makes sure we wait for the window to open, before we move on with the code.
                for (int i = 0; i < 6000; i++)  //up to 60 s, then we move on anyway
                {
                    System.Threading.Thread.Sleep(10);  //0.01s
                    if (decompOptions2.numberOfRecalcs > 0)
                    {
                        break;
                    }
                }

            }
        }

        public static void Decomp2Helper(DecompOptions2 o)
        {
            DecompOptions2 decompOptions = (DecompOptions2)o;
            WindowDecomp w = null;

            w = new WindowDecomp(decompOptions);

            Globals.windowsDecomp2.Add(w);

            int count = -1;
            foreach (Link link in decompOptions.link)
            {
                count++;
                if (Program.modelGams != null)
                {

                    if (link.expressions.Count == 1 && link.expressions[0] == null)
                    {
                        ModelGamsEquation found = Program.DecompEvalGams(link.eqname, link.varnames[0]);
                        link.expressions = found.expressions;
                        link.expressionText = found.lhs + " = " + found.rhs;
                    }
                    else
                    {
                        //fix this...
                    }
                }
                else
                {

                    if (Program.model == null)
                    {
                        G.Writeln2("*** ERROR: DECOMP: A model is not loaded, cf. the MODEL command.");
                        throw new GekkoException();
                    }

                    EquationHelper found = Program.DecompEval(decompOptions.variable);
                    decompOptions.expression = Globals.expressions[0];
                    decompOptions.expressionOld = found.equationText;
                }
            }

            if (decompOptions.name == null)
            {
                w.Title = "Decompose expression";
            }
            else
            {
                w.Title = "Decompose " + decompOptions.variable + "";
            }
            w.Tag = decompOptions;

            w.isInitializing = true;  //so we don't get a recalc here because of setting radio buttons
            w.SetRadioButtons();
            w.isInitializing = false;

            w.RecalcCellsWithNewType();
            decompOptions.numberOfRecalcs++;  //signal for Decomp() method to move on

            if (G.IsUnitTesting() && Globals.showDecompTable == false)
            {
                Globals.windowsDecomp2.Clear();
                w = null;
            }
            else
            {
                if (w.isClosing)  //if something goes wrong, .isClosing will be true
                {
                    //The line below removes the window from the global list of active windows.
                    //Without this line, this half-dead window will mess up automatic closing of windows (Window -> Close -> Close all...)
                    if (Globals.windowsDecomp2.Count > 0) Globals.windowsDecomp2.RemoveAt(Globals.windowsDecomp2.Count - 1);
                }
                else
                {
                    w.ShowDialog();
                    w.Close();  //probably superfluous
                    w = null;  //probably superfluous
                    if (Globals.showDecompTable)
                    {
                        Globals.showDecompTable = false;
                        G.Writeln2("*** ERROR: Debug, tables aborted. Set Globals.showDecompTable = false.");
                        throw new GekkoException();
                    }
                }
            }
        }

        public static DecompData Decompose2(GekkoTime tt1, GekkoTime tt2, Func<GekkoSmpl, IVariable> expression, EDecompBanks workOrRefOrBoth, string residualName)
        {
            //
            //
            //
            //                 Ref   -- m -->      Work
            //
            //                   ^                    ^
            //                   |                    |
            //                  rd                    d
            //                   |                    |
            //
            //                 Ref[-1]             Work[-1]
            //
            //
            //DECOMP2 <2010 2012 q> sum((#a, #s), pop[#a, #s, #o]) 
            //  SELECT #a, #s
            //  WHERE  'se' in #o, 'se' in #o  // ... , date = 2011    
            //  AGG   #a as #a_agg level '10-year' zoom '27', #a as #a_agg level '10-year' zoom '27'  
            //  SORT #s, #a
            //  LINK   x1 from e2, x3 from e1    
            //  COLS  #a, #o;

            // DECOMP a where a in b agg x as y level 1 zoom 2 link a from b;
            // y   $   'a' in #a and 'b' in #j --> y[a, b]
            // comma could be used in $
            // level 3 --> level '5-year'
            // It should be possible to select agg level. Later on, more sophisticated opening of sub-nodes:
            //    level '10-year' zoom '25..29'   or   level '10-year' zoom '27'
            // when zooming, sibling nodes for zoomed level are shown, same for parents up to the current aggregation level.
            // Like this, we can both have an aggregation level and a deeper zoom.
            //
            // Per default, we have time in cols. It should be possible to put other dimensions on the cols (for fixed time).
            // even having time on rows should be possible
            //                        
            // #a_agg = nested list with this structure:            
            //
            //  '5-year'       --> level2
            //    '20..24'
            //      '20'
            //      '21'
            //      '22'
            //      '23'
            //      '24'
            //    '25..29'
            //      '25'
            //      '26'
            //      '27'
            //      '28'
            //      '29'
            // '10-year'        --> level 3
            //    '20..29'
            //      '20..24'
            //      '25..29'
            //    '30..39'
            //      '30..34'
            //      '35..39'
            // 'total'              --> level 4
            //   'tot'
            //     '20..29'
            //     '30..39'
            //
            //
            // 
            //
            // we could later on use groupbyavg, groupby is implicitly groupbysum here
            //
            //      50 51 52 53 54 55 56 57 58 59 60 61 62 63 64 65 66 67 68 69 70 71
            //       ------------   ------------  -------------   ------------   ----...
            //       ===========================  ============================   ====...
            //       +++++++++++++++++++++++++++++++++++++++++++++++++++++++++   ++++...
            //      
            //       -------------------------------------------------  -------------... arb.marked
            //
            // The real output from this are these dicts:            
            //   cellsContribD = new DecompDict();
            //   cellsContribDRef = new DecompDict();
            //   cellsContribM = new DecompDict();
            //
            // They may not all be created: depends upon workOrRefOrBoth parameter
            // If the equation is y = sum(#i, x[#i]) + x[c][+1] + z[-1], the keys will be:
            //   Work:y
            //   Work:x[a]
            //   Work:x[b]
            //   Work:x[c]¤[+1]            
            //   Work:z¤[-1]
            //
            // The contributions sum to zero. These may link up to other DecompTables. If we have this:
            //
            // x[a][-1] = z[-2] + x[c]
            //
            // we may put in x[a][-1] into x[a] in the former equation, but then we need to lead these contributions:
            //
            // x[a] = z[-2][-1] + x[c][+1]
            //
            // We can just use the same table where we add 1 to the ¤[...] lags, and change the offset in all the series in the DecompTables.
            // For instance:
            //
            //   Work:y             -18
            //   Work:x[a]           20
            //   Work:x[b]            5
            //   Work:x[c]¤[+1]     -12    
            //   Work:z¤[-1]          5
            // ---------------------------  NOTE: the table below has been leaded
            //   Work:x[a]          -10     --> we join on x[a]
            //   Work:z¤[-1]          3          
            //   Work:x[c]¤[+1]       7
            // ===========================
            //   Work:y             -10     --> divide with 10 to get the final effects on y
            //     Work:z¤[-1]        6     --> the x[a] = 20 is removed by multiplying the second table with 2 and adding the two tables     
            //     Work:x[c]¤[+1]    14
            //   Work:x[b]            5
            //   Work:x[c]¤[+1]     -12    
            //   Work:z¤[-1]          5

            List<int> mm = new List<int>();
            if (workOrRefOrBoth == EDecompBanks.Work) mm.Add(0);
            else if (workOrRefOrBoth == EDecompBanks.Ref) mm.Add(1);
            else if (workOrRefOrBoth == EDecompBanks.Both)
            {
                mm.Add(0);
                mm.Add(1);
            }

            DecompData d = new DecompData();

            int funcCounter = 0;

            DateTime dt = DateTime.Now;

            GekkoSmpl smpl = new GekkoSmpl(tt1, tt2);
            IVariable y0a = null;
            IVariable y0aRef = null;

            int perLag = -2;

            try
            {  //resets Globals.precedents afterwards

                d.cellsGradQuo = new DecompDict();
                d.cellsQuo = new DecompDict();
                d.cellsContribD = new DecompDict();
                d.cellsGradRef = new DecompDict();
                d.cellsRef = new DecompDict();
                d.cellsContribDRef = new DecompDict();
                d.cellsContribM = new DecompDict();

                Globals.precedents = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                //Function call start --------------
                O.AdjustSmplForDecomp(smpl, 0);

                //TODO: can be deleted, #p24234oi32
                string s5 = Globals.expressionText;

                y0a = expression(smpl); funcCounter++;  //this call fills Globals.precedents with variables
                O.AdjustSmplForDecomp(smpl, 1);
                //Function call end   --------------

                List<DecompPrecedent> decompPrecedents = new List<DecompPrecedent>();


                List<string> ss = Globals.precedents.Keys.ToList<string>();
                ss.Sort(StringComparer.OrdinalIgnoreCase);
                foreach (string s in ss)
                {
                    IVariable x = O.GetIVariableFromString(s, O.ECreatePossibilities.NoneReportError);

                    if (x.Type() == EVariableType.Series)
                    {
                        Series ivTemp_series = x as Series;
                        if (ivTemp_series.type == ESeriesType.ArraySuper) continue;  //skipped: we are only looking at sub-series
                        decompPrecedents.Add(new DecompPrecedent(s, x));
                    }
                    else if (x.Type() == EVariableType.Val)
                    {
                        decompPrecedents.Add(new DecompPrecedent(s, x));
                    }
                }


                //decompPrecedents.Add(new DecompPrecedent(Globals.decompExpressionName + "¤[0]", null));  //seems variable is not used anyway

                Globals.precedents = null;  //!!! This is important: if not set to null, afterwards there will be a lot of superfluous lookup in the dictionary

                Series y0a_series = y0a as Series;
                if (y0a == null)
                {
                    G.Writeln2("*** ERROR: DECOMP expects the expression to be of series type");
                    throw new GekkoException();
                }
                Series y0_series = y0a_series;
                if (y0a_series.type != ESeriesType.Light)
                {
                    y0_series = y0a.DeepClone(null) as Series;  //a lag like "DECOMP x[-1]" may just move a pointer to real timeseries x, and x is changed with shocks...
                }

                d.cellsQuo.storage.Add(residualName, y0_series);

                Series y0aRef_series = null;
                Series y0Ref_series = null;
                if (mm.Contains(1))
                {
                    //Function call start --------------
                    O.AdjustSmplForDecomp(smpl, 0);
                    smpl.bankNumber = 1;
                    y0aRef = expression(smpl); funcCounter++;
                    smpl.bankNumber = 0;
                    O.AdjustSmplForDecomp(smpl, 1);
                    //Function call end   --------------

                    y0aRef_series = y0aRef as Series;
                    if (y0aRef == null)
                    {
                        G.Writeln2("*** ERROR: DECOMP expects the expression to be of series type");
                        throw new GekkoException();
                    }
                    y0Ref_series = y0aRef_series;
                    if (y0aRef_series.type != ESeriesType.Light)
                    {
                        y0Ref_series = y0aRef.DeepClone(null) as Series;  //a lag like "DECOMP x[-1]" may just move a pointer to real timeseries x, and x is changed with shocks...
                    }
                    d.cellsRef.storage.Add(residualName, y0Ref_series);
                }

                double eps = Globals.newtonSmallNumber;

                if (decompPrecedents.Count > 0)
                {
                    GekkoDictionary<string, int> vars = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                    int iVar = -1;

                    foreach (DecompPrecedent dp in decompPrecedents)
                    {

                        iVar++;

                        Series xRef_series = null;
                        IVariable dpx = O.GetIVariableFromString(dp.s, O.ECreatePossibilities.NoneReportError);

                        if (dpx.Type() == EVariableType.Series)
                        {
                            if ((dpx as Series).type == ESeriesType.Timeless) continue;  //skip timeless series, #2983473298472
                                                                                         //could also use smpl.bankNumber = 1 to do this, but then GetIVariableFromString should use smpl.bankNumbe
                            if (mm.Contains(1))
                            {
                                xRef_series = O.GetIVariableFromString(G.Chop_SetBank(dp.s, "Ref"), O.ECreatePossibilities.NoneReportError) as Series;
                            }
                        }
                        else
                        {
                            //else what?
                        }

                        foreach (GekkoTime t1 in new GekkoTimeIterator(tt1.Add(-O.MaxLag()), tt2.Add(O.MaxLead())))
                        {

                            // --------------------------------------------
                            // This is where the decomposition takes place
                            // --------------------------------------------

                            foreach (int j in mm)
                            {
                                if (dpx.Type() == EVariableType.Series)
                                {
                                    Series x_series = null;
                                    Series y_series = null;
                                    if (j == 0)
                                    {
                                        x_series = dpx as Series;
                                        y_series = y0_series;
                                    }
                                    else
                                    {
                                        x_series = xRef_series;
                                        y_series = y0Ref_series;
                                    }
                                    double x_before = x_series.GetData(smpl, t1);
                                    try
                                    {
                                        double x_after = x_before + eps;
                                        x_series.SetData(t1, x_after);

                                        //Function call start --------------
                                        O.AdjustSmplForDecomp(smpl, 0);
                                        if (j == 1) smpl.bankNumber = 1;
                                        IVariable y1 = null;
                                        y1 = expression(smpl); funcCounter++;
                                        if (j == 1) smpl.bankNumber = 0;
                                        O.AdjustSmplForDecomp(smpl, 1);
                                        //Function call end   --------------

                                        Series y1_series = y1 as Series;

                                        foreach (GekkoTime t2 in new GekkoTimeIterator(tt1.Add(perLag), tt2.Add(0)))
                                        {
                                            double y0_double = y_series.GetData(smpl, t2);
                                            double y1_double = y1_series.GetData(smpl, t2);
                                            double grad = (y1_double - y0_double) / eps;
                                            int lag = -(GekkoTime.Observations(t1, t2) - 1);  //x[-1] --> lag = -1
                                            string name = G.Chop_FreqRemove(dp.s, tt1.freq);

                                            string lag2 = lag.ToString();
                                            if (lag >= 1) lag2 = "+" + lag;
                                            name += "¤[" + lag2 + "]";

                                            if (lag == 0 || (lag < 0 && -lag <= Program.options.decomp_maxlag) || (lag > 0 && lag <= Program.options.decomp_maxlead))
                                            {
                                                //SLACK
                                                //SLACK
                                                //SLACK fix this if decomp becomes too slow
                                                //SLACK
                                                //SLACK
                                                //slack: we get too many variants of name[-x] and name[+x] here
                                                //they are all just offsets of each other, no?
                                                if (j == 0)
                                                {
                                                    d.cellsQuo[name].SetData(t2, x_before);
                                                    //G.Writeln("quo " + name + " " + t2.ToString() + " " + x_before);
                                                }
                                                else
                                                {
                                                    d.cellsRef[name].SetData(t2, x_before);
                                                    //G.Writeln("ref " + name + " " + t2.ToString() + " " + x_before);
                                                }
                                            }

                                            if (!G.isNumericalError(grad) && grad != 0d)
                                            {
                                                if (j == 0)
                                                {
                                                    d.cellsGradQuo[name].SetData(t2, grad);
                                                }
                                                else
                                                {
                                                    d.cellsGradRef[name].SetData(t2, grad);
                                                }

                                                if (!vars.ContainsKey(name))
                                                {
                                                    vars.Add(name, 0);
                                                }
                                                else
                                                {
                                                }

                                            }
                                        }
                                    }
                                    finally
                                    {
                                        x_series.SetData(t1, x_before);
                                    }
                                }
                                else if (dpx.Type() == EVariableType.Val)
                                {
                                    //TODO
                                }
                                else
                                {
                                    //skip other types, this includes matrices
                                    //so an expression with a matrix that changes from Work to Ref is
                                    //not decomoposed as regards to this matrix
                                    //(we would have to shock each cell in the matrix...)
                                }
                            }
                        }
                    }

                    //Here, cellsQuo + cellsRef + cellsGradQuo + cellsGradRef are calculated.
                    //Grad tells us which lags are actually active.
                    //If we know that lags beforehand, we could limit the lag loop and save time here.

                    int i = 0;
                    foreach (GekkoTime t2 in new GekkoTimeIterator(tt1, tt2))
                    {
                        i++;
                        int j = 0;
                        foreach (string s in vars.Keys)
                        {
                            j++;

                            double vQuo = d.cellsQuo[s].GetData(smpl, t2);
                            double vQuoLag = d.cellsQuo[s].GetData(smpl, t2.Add(-1));
                            double vGradQuoLag = d.cellsGradQuo[s].GetData(smpl, t2.Add(-1));
                            //double vGradQuo = d.cellsGradQuo[s].GetData(smpl, t2); --> not used at the moment
                            double dContribD = vGradQuoLag * (vQuo - vQuoLag);
                            d.cellsContribD[s].SetData(t2, dContribD);

                            if (false) G.Writeln2(s + " quo " + vQuo + " quo.1 " + vQuoLag + " grad.1 " + vGradQuoLag + " " + dContribD);

                            if (mm.Contains(1))
                            {
                                double vRef = d.cellsRef[s].GetData(smpl, t2);
                                double vRefLag = d.cellsRef[s].GetData(smpl, t2.Add(-1));
                                double vGradRef = d.cellsGradRef[s].GetData(smpl, t2);
                                double vGradRefLag = d.cellsGradRef[s].GetData(smpl, t2.Add(-1));
                                double dContribM = vGradRef * (vQuo - vRef);
                                double dContribDRef = vGradRefLag * (vRef - vRefLag);
                                d.cellsContribM[s].SetData(t2, dContribM);
                                d.cellsContribDRef[s].SetData(t2, dContribDRef);
                            }
                        }
                        d.cellsContribD[residualName].SetData(t2, -(d.cellsQuo[residualName].GetDataSimple(t2) - d.cellsQuo[residualName].GetDataSimple(t2.Add(-1))));
                        d.cellsContribDRef[residualName].SetData(t2, -(d.cellsRef[residualName].GetDataSimple(t2) - d.cellsRef[residualName].GetDataSimple(t2.Add(-1))));
                        d.cellsContribM[residualName].SetData(t2, -(d.cellsQuo[residualName].GetDataSimple(t2) - d.cellsRef[residualName].GetDataSimple(t2)));
                    }
                }
            }
            finally
            {
                //Important: makes sure is is *always* nulled after a DECOMP
                Globals.precedents = null;
            }

            //if (funcCounter > 0)
            //{
            //    G.Writeln2("DECOMP took " + G.SecondsFormat((DateTime.Now - dt).TotalMilliseconds) + " --> " + funcCounter + " evals");
            //    G.Writeln("+++ NOTE: DECOMP only works well on simulated values -- a patch for 3.0 will fix this.");
            //}

            return d;

        }

        public static Gekko.Table DecomposePutIntoTable3(List<string> varnames, GekkoTime per1, GekkoTime per2, List<DecompData> decompDatas, DecompTablesFormat format, string code1, string isShares, GekkoSmpl smpl, string lhs, string expressionText, DecompOptions2 decompOptions2)
        {
            FrameLight frame = new FrameLight();  //light-weight Gekko dataframe
            List<string> select_rowvars = decompOptions2.rows;
            List<string> select_colvars = decompOptions2.cols;

            List<FrameFilter> filters = new List<FrameFilter>();
            if (decompOptions2.where != null)
            {
                foreach (List<string> filter in decompOptions2.where)
                {
                    FrameFilter filter1 = new FrameFilter();
                    filter1.active = true;
                    filter1.name = filter[filter.Count - 1];
                    filter1.selected = filter.GetRange(0, filter.Count - 1);
                    filters.Add(filter1);
                }
            }

            //The DataTable dt will get the following colums:
            //<t>:         time
            //<variable>:  variable name, like fy or pop
            //<lag>:       lag or lead
            //<#universe>: universal set for elements without domain info
            //#i:          set names, like #age, #sector, etc.
            //<value>:     data value

            string internalColumnIdentifyer = "gekkopivot__";
            string internalSetIdentifyer = "gekkoset__";
            string col_t = internalColumnIdentifyer + "t";
            string col_variable = internalColumnIdentifyer + "variable";
            string col_lag = internalColumnIdentifyer + "lag";
            string col_universe = internalColumnIdentifyer + "universe";
            string col_value = internalColumnIdentifyer + "value";
            string gekko_null = "null";
            string col_equ = internalColumnIdentifyer + "equ";

            frame.AddColName(col_t);
            frame.AddColName(col_value);
            frame.AddColName(col_variable);
            frame.AddColName(col_lag);
            frame.AddColName(col_universe);
            frame.AddColName(col_equ);

            int superN = decompDatas.Count;

            for (int super = 0; super < superN; super++)
            {

                int parentI = 0;
                List<string> vars2 = Program.DecompGetVars3(super, decompDatas, varnames, decompOptions2.link[parentI].expressionText);

                int j = 0;
                foreach (GekkoTime t2 in new GekkoTimeIterator(per1, per2))
                {
                    j++;
                    int i = 0;
                    double lhsSum = 0d;
                    double rhsSum = 0d;
                    foreach (string colname in vars2)
                    {
                        i++;

                        string dbName = null; string varName = null; string freq = null; string[] indexes = null;
                        string[] domains = null;

                        //See #876435924365

                        string lag = null;

                        if (true)
                        {
                            //there is some repeated work done here, but not really bad
                            //problem is we prefer to do one period at a time, to sum up, adjust etc.

                            string[] ss = colname.Split('¤');
                            string fullName = ss[0];
                            lag = ss[1];
                            if (lag == "[0]")
                            {
                                lag = null;
                            }

                            char firstChar;
                            O.Chop(fullName, out dbName, out varName, out freq, out indexes);

                            if (indexes != null) domains = new string[indexes.Length];

                            if (domains != null)
                            {
                                //Adding domain info. We may have x[18, gov] which is part of x[#a, #sector].
                                //So in this case, #a and #sector would be added as columns
                                IVariable iv = O.GetIVariableFromString(fullName, O.ECreatePossibilities.NoneReturnNull);
                                if (iv != null)
                                {
                                    Series ts = iv as Series;
                                    if (ts?.mmi?.parent?.meta?.domains != null)
                                    {
                                        for (int ii = 0; ii < ts.mmi.parent.meta.domains.Length; ii++)
                                        {
                                            domains[ii] = ConvertSetname(internalSetIdentifyer, col_universe, ts.mmi.parent.meta.domains[ii]);
                                        }
                                    }
                                }

                                foreach (string domain in domains)
                                {
                                    if (domain != null)
                                    {
                                        string setname = domain.ToLower();
                                        if (setname == null) setname = col_universe;
                                        frame.AddColName(setname);  //will .tolower() and ignore dublets
                                    }
                                }
                            }

                            //See #876435924365              
                            string bank2 = dbName;
                            if (G.Equal(Program.databanks.GetFirst().name, dbName)) bank2 = null;
                            string name2 = O.UnChop(null, varName, null, indexes);
                        }

                        double d = DecomposePutIntoTable2HelperOperators(decompDatas[super], code1, smpl, lhs, t2, colname);

                        if (i == 1)
                        {
                            lhsSum = d;
                        }
                        else
                        {
                            rhsSum += d;
                        }

                        FrameLightRow dr = new FrameLightRow(frame);
                        dr.Set(frame, col_equ, new CellLight(super.ToString()));
                        dr.Set(frame, col_t, new CellLight(t2.ToString()));
                        dr.Set(frame, col_variable, new CellLight(varName));

                        string lag2 = null;
                        if (true)
                        {
                            if (lag != null) lag2 = lag;
                            else lag2 = "[0]";
                        }
                        else
                        {
                            if (lag != null) lag2 = lag.Trim().Substring(1, lag.Trim().Length - 2);
                        }

                        dr.Set(frame, col_lag, new CellLight(lag2));

                        if (indexes != null)
                        {
                            for (int ii = 0; ii < indexes.Length; ii++)
                            {
                                if (domains != null)
                                {
                                    string domain = domains[ii];
                                    string index = indexes[ii];
                                    if (domain != null)
                                    {
                                        dr.Set(frame, domain, new CellLight(index));
                                    }
                                    else
                                    {
                                        dr.Set(frame, col_universe, new CellLight(index));
                                    }
                                }
                            }
                        }

                        dr.Set(frame, col_value, new CellLight(d));
                        frame.rows.Add(dr);
                    }
                }
            }

            if (Globals.decompUnitPivot)
            {
                WriteDatatableTocsv(frame, internalColumnIdentifyer, internalSetIdentifyer);
            }

            if (decompOptions2.rows.Count == 0 || decompOptions2.cols.Count == 0)
            {
                G.Writeln2("*** ERROR: rows and cols must be stated");
                throw new GekkoException();
            }

            Gekko.Table tab = new Gekko.Table();
            tab.writeOnce = true;

            DecomposeReplaceVars(select_rowvars, internalSetIdentifyer, col_t, col_variable, col_lag, col_universe, col_equ);
            DecomposeReplaceVars(select_colvars, internalSetIdentifyer, col_t, col_variable, col_lag, col_universe, col_equ);
            DecomposeReplaceVars(filters, internalSetIdentifyer, col_t, col_variable, col_lag, col_universe, col_equ);

            List<string> rownames = new List<string>();
            List<string> colnames = new List<string>();
            GekkoDictionary<string, double> agg = new GekkoDictionary<string, double>(StringComparer.OrdinalIgnoreCase);
            foreach (FrameLightRow row in frame.rows)
            {
                bool skip = false;
                foreach (FrameFilter filter in filters)
                {
                    CellLight c = row.Get(frame, filter.name);
                    if (c.type != ECellLightType.String) throw new GekkoException();
                    string ss = c.text;
                    if (!filter.selected.Contains(ss, StringComparer.OrdinalIgnoreCase))
                    {
                        skip = true;
                        break;
                    }
                }
                if (skip) continue;

                string s1 = null;
                foreach (string s in select_rowvars)
                {
                    s1 = DecompAddText(frame, row, s1, s);
                }
                if (s1 != null) s1 = s1.Substring(1);
                string s2 = null;
                foreach (string s in select_colvars)
                {
                    s2 = DecompAddText(frame, row, s2, s);
                }
                if (s2 != null) s2 = s2.Substring(1);
                string key = s1 + "¤" + s2;

                if (!rownames.Contains(s1, StringComparer.OrdinalIgnoreCase)) rownames.Add(s1);
                if (!colnames.Contains(s2, StringComparer.OrdinalIgnoreCase)) colnames.Add(s2);

                CellLight c3 = row.Get(frame, col_value);
                double d = c3.data;

                if (!agg.ContainsKey(key))
                {
                    agg.Add(key, d);
                }
                else
                {
                    agg[key] += d;
                }
            }
            rownames.Sort(StringComparer.OrdinalIgnoreCase);
            colnames.Sort(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < rownames.Count; i++)
            {
                for (int j = 0; j < colnames.Count; j++)
                {
                    string key = rownames[i] + "¤" + colnames[j];
                    double d = 0d;
                    agg.TryGetValue(key, out d);
                    tab.SetNumber(i + 2, j + 2, d, "f10.4");
                }
            }

            for (int i = 0; i < rownames.Count; i++)
            {
                tab.Set(i + 2, 1, rownames[i]);
            }

            for (int j = 0; j < colnames.Count; j++)
            {
                tab.Set(1, j + 2, colnames[j]);
            }

            return tab;
        }

        private static void DecomposeReplaceVars(List<string> vars, string internalSetIdentifyer, string col_t, string col_variable, string col_lag, string col_universe, string col_equ)
        {
            for (int i = 0; i < vars.Count; i++)
            {
                if (G.Equal(vars[i], "time")) vars[i] = col_t;
                if (G.Equal(vars[i], "vars")) vars[i] = col_variable;
                if (G.Equal(vars[i], "lags")) vars[i] = col_lag;
                if (G.Equal(vars[i], "#uni")) vars[i] = col_universe;
                if (G.Equal(vars[i], "equ")) vars[i] = col_equ;
                if (vars[i].StartsWith("#")) vars[i] = internalSetIdentifyer + vars[i].Substring(1);
            }
        }

        public static void DecomposeReplaceVars(List<FrameFilter> vars, string internalSetIdentifyer, string col_t, string col_variable, string col_lag, string col_universe, string col_equ)
        {
            for (int i = 0; i < vars.Count; i++)
            {
                if (G.Equal(vars[i].name, "time")) vars[i].name = col_t;
                if (G.Equal(vars[i].name, "vars")) vars[i].name = col_variable;
                if (G.Equal(vars[i].name, "lags")) vars[i].name = col_lag;
                if (G.Equal(vars[i].name, "#uni")) vars[i].name = col_universe;
                if (G.Equal(vars[i].name, "equ")) vars[i].name = col_equ;
                if (vars[i].name.StartsWith("#")) vars[i].name = internalSetIdentifyer + vars[i].name.Substring(1);
            }
        }

        public static string ConvertSetname(string internalSetIdentifyer, string col_universe, string domain)
        {
            string domain2 = domain;
            if (domain2 != null) domain2 = internalSetIdentifyer + domain2.Replace("#", "");
            string setname = col_universe; //"<...>" so that it does not collide with a variable name
            if (domain2 != null) setname = domain2;
            return setname;
        }

        public static string DecompAddText(FrameLight frame, FrameLightRow row, string s1, string s)
        {
            CellLight c = row.Get(frame, s);
            if (c.type == ECellLightType.None)
            {
                s1 += "," + "null";
            }
            else if (c.type == ECellLightType.String)
            {
                s1 += "," + c.text;
            }
            else
            {
                throw new GekkoException();
            }
            return s1;
        }

        public static void WriteDatatableTocsv(FrameLight dt, string internalColumnIdentifyer, string internalSetIdentifyer)
        {
            StringBuilder sb = new StringBuilder();
            List<string> columnNames = new List<string>(dt.colnames);
            for (int i = 0; i < columnNames.Count; i++)
            {
                columnNames[i] = columnNames[i].Replace(internalColumnIdentifyer, "");
                columnNames[i] = columnNames[i].Replace(internalSetIdentifyer, "#");
                if (columnNames[i] == "universe") columnNames[i] = "#uni";
            }
            sb.AppendLine(string.Join(";", columnNames));
            foreach (FrameLightRow row in dt.rows)
            {
                string s = null;
                foreach (CellLight c in row.storage)
                {
                    s += c.ToString() + "; ";
                }
                if (s != null) s = s.Substring(0, s.Length - "; ".Length);
                sb.AppendLine(s);
            }
            File.WriteAllText(@"c:\Thomas\Gekko\regres\Models\Decomp\pivot.csv", sb.ToString());
        }

        public static double DecomposePutIntoTable2HelperOperators(DecompData decompTables, string code1, GekkoSmpl smpl, string lhs, GekkoTime t2, string colname)
        {
            double d = double.NaN;
            if (code1 == "n" || code1 == "xn" || code1 == "x")
            {
                d = decompTables.cellsQuo[colname].GetData(smpl, t2);  //for instance {"x¤2002", 2.5} or {"x[-1]¤2003", -1.5}
            }
            else if (code1 == "r" || code1 == "xr" || code1 == "xrn")
            {
                d = decompTables.cellsRef[colname].GetData(smpl, t2);
            }
            else if (code1 == "d")
            {
                d = decompTables.cellsContribD[colname].GetData(smpl, t2);
            }
            else if (code1 == "rd")
            {
                d = decompTables.cellsContribDRef[colname].GetData(smpl, t2);
            }
            else if (code1 == "xd")
            {
                double d1 = decompTables.cellsQuo[colname].GetData(smpl, t2);
                double d0 = decompTables.cellsQuo[colname].GetData(smpl, t2.Add(-1));
                d = d1 - d0;
            }
            else if (code1 == "xrd")
            {
                double d1 = decompTables.cellsRef[colname].GetData(smpl, t2);
                double d0 = decompTables.cellsRef[colname].GetData(smpl, t2.Add(-1));
                d = d1 - d0;
            }
            else if (code1 == "m")
            {
                d = decompTables.cellsContribM[colname].GetData(smpl, t2);
            }
            else if (code1 == "xm")
            {
                double d1 = decompTables.cellsQuo[colname].GetData(smpl, t2);
                double d0 = decompTables.cellsRef[colname].GetData(smpl, t2);
                d = d1 - d0;
            }
            else if (code1 == "p")
            {
                double dd = decompTables.cellsContribD[colname].GetData(smpl, t2);
                double dLhsLag = decompTables.cellsQuo[lhs].GetData(smpl, t2.Add(-1));
                d = (dd / dLhsLag) * 100d;
            }
            else if (code1 == "rp")
            {
                double dd = decompTables.cellsContribDRef[colname].GetData(smpl, t2);
                double dLhsLag = decompTables.cellsRef[lhs].GetData(smpl, t2.Add(-1));
                d = (dd / dLhsLag) * 100d;
            }
            else if (code1 == "xp")
            {
                double d1 = decompTables.cellsQuo[colname].GetData(smpl, t2);
                double d0 = decompTables.cellsQuo[colname].GetData(smpl, t2.Add(-1));
                d = (d1 / d0 - 1d) * 100d;
            }
            else if (code1 == "xrp")
            {
                double d1 = decompTables.cellsRef[colname].GetData(smpl, t2);
                double d0 = decompTables.cellsRef[colname].GetData(smpl, t2.Add(-1));
                d = (d1 / d0 - 1d) * 100d;
            }
            else if (code1 == "q")
            {
                double dd = decompTables.cellsContribM[colname].GetData(smpl, t2);
                double dLhsLag = decompTables.cellsRef[lhs].GetData(smpl, t2);
                d = (dd / dLhsLag) * 100d;
            }
            else if (code1 == "xq")
            {
                double d1 = decompTables.cellsQuo[colname].GetData(smpl, t2);
                double d0 = decompTables.cellsRef[colname].GetData(smpl, t2);
                d = (d1 / d0 - 1d) * 100d;
            }
            else if (code1 == "dp")
            {
                double dd = decompTables.cellsContribD[colname].GetData(smpl, t2);
                double dd_lag = decompTables.cellsContribD[colname].GetData(smpl, t2.Add(-1));
                double dLhsLag = decompTables.cellsQuo[lhs].GetData(smpl, t2.Add(-1));
                double dLhsLag_lag = decompTables.cellsQuo[lhs].GetData(smpl, t2.Add(-1).Add(-1));
                d = (dd / dLhsLag - dd_lag / dLhsLag_lag) * 100d;
            }
            else if (code1 == "rdp")
            {
                double dd = decompTables.cellsContribDRef[colname].GetData(smpl, t2);
                double dd_lag = decompTables.cellsContribDRef[colname].GetData(smpl, t2.Add(-1));
                double dLhsLag = decompTables.cellsRef[lhs].GetData(smpl, t2.Add(-1));
                double dLhsLag_lag = decompTables.cellsRef[lhs].GetData(smpl, t2.Add(-1).Add(-1));
                d = (dd / dLhsLag - dd_lag / dLhsLag_lag) * 100d;
            }
            else if (code1 == "xdp")
            {
                double d1 = decompTables.cellsQuo[colname].GetData(smpl, t2);
                double d1_lag = decompTables.cellsQuo[colname].GetData(smpl, t2.Add(-1));
                double d0 = decompTables.cellsQuo[colname].GetData(smpl, t2.Add(-1));
                double d0_lag = decompTables.cellsQuo[colname].GetData(smpl, t2.Add(-1).Add(-1));
                d = (d1 / d0 - 1d - (d1_lag / d0_lag - 1d)) * 100d;
            }
            else if (code1 == "xrdp")
            {
                double d1 = decompTables.cellsRef[colname].GetData(smpl, t2);
                double d1_lag = decompTables.cellsRef[colname].GetData(smpl, t2.Add(-1));
                double d0 = decompTables.cellsRef[colname].GetData(smpl, t2.Add(-1));
                double d0_lag = decompTables.cellsRef[colname].GetData(smpl, t2.Add(-1).Add(-1));
                d = (d1 / d0 - 1d - (d1_lag / d0_lag - 1d)) * 100d;
            }
            else if (code1 == "mp")  // <p> - <rp>
            {
                double dd = decompTables.cellsContribD[colname].GetData(smpl, t2);
                double dLhsLag = decompTables.cellsQuo[lhs].GetData(smpl, t2.Add(-1));

                double dd2 = decompTables.cellsContribDRef[colname].GetData(smpl, t2);
                double dLhsLag2 = decompTables.cellsRef[lhs].GetData(smpl, t2.Add(-1));
                d = (dd / dLhsLag - dd2 / dLhsLag2) * 100d;
            }
            else if (code1 == "xmp")
            {
                double d1 = decompTables.cellsQuo[colname].GetData(smpl, t2);
                double d0 = decompTables.cellsQuo[colname].GetData(smpl, t2.Add(-1));
                double d1_ref = decompTables.cellsRef[colname].GetData(smpl, t2);
                double d0_ref = decompTables.cellsRef[colname].GetData(smpl, t2.Add(-1));
                d = (d1 / d0 - 1d - (d1_ref / d0_ref - 1d)) * 100d;
            }
            else
            {
                MessageBox.Show("*** ERROR: Wrong operator");
                throw new GekkoException();
            }

            return d;
        }

    }
}
