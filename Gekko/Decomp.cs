using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Gekko
{
    class FiveDouble
    {
        public double change;
        public double level;
        public double levelLag;
        public double levelRef;
        public double levelRefLag;

        public FiveDouble(double change, double level, double levelLag, double levelRef, double levelRefLag)
        {
            this.change = change;
            this.level = level;
            this.levelLag = levelLag;
            this.levelRef = levelRef;
            this.levelRefLag = levelRefLag;
        }
    }

    class Decomp
    {
        public enum EContribType
        {
            Unknown,
            N,
            RN,
            D,
            RD,
            M
        }

        public static void DecompStart(O.Decomp2 o)
        {
            
            //In general, uncontrolled sets produce a list of equations. Hard to prune these, it is a bit like the lag problem, only lazy 
            //  eval might help.
            //In an equation like y[#a] = x[#a] + 5, there will be 100 equations if #a is 1..100. For each of these, lags are tried. So
            //it is checked if x[31][2000] affects y[31][2001] --> a lag. If such a lag is detected, x[#a][-1] is added to the variables
            //that contribute.

            //DecompStart()                              --> This is the starting point
            //  DecompGetFuncExpressions()               --> May use Program.DecompEvalGams() or Program.DecompEval(), with I("EVAL ...") 
            //                                               DecompEvalGams() finds the equation, translates to Gekko, and returns a 
            //                                               ModelGamsEquation object with element.expressions containing the expression(s).
            //                                               In an eq like y[#i] = 2*x[#i], n expressions are returned corresponding to the elements of #i
            //  WindowDecomp.RecalcCellsWithNewType();   --> Can also be called when clicking
            //    DecompMain()                           --> Main calculation, calls lowLevel, Pivot and makeGui.
            //      DecompLowLevel()                     --> actual calculation of data, expression(s) is argument
            //      DecompPivotToTable()                 --> putting the data into a table
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
                decompOptions2.decompTablesFormat.showErrors = true; //
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
                    //pivotfix 
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

        public static Table DecompMain(GekkoSmpl smpl, GekkoTime per1, GekkoTime per2, string operator1, string isShares, DecompOptions2 decompOptions2, FrameLight frame, bool refresh, ref List<List<DecompData>> decompDatas)
        {            

            DateTime t0 = DateTime.Now;

            EContribType operatorOneOf3Types = DecompContribTypeHelper(decompOptions2.prtOptionLower);

            int perLag = -2;
            string lhsString = "Expression value";
            int parentI = 0;

            //MAIN varnames are: decompOptions2.link[parentI].varnames

            // decompDatas
            // Example: DECOMP x[#a] in e1 link y[#a] in e2
            // 1. dimension corresponds to main chosen decomp variables (x[#a], could be stated like x[18], x[19]).
            //    This dimension corresponds to super = 0, 1 here.
            // 2. dimension is the folded raw link equations (including main equation with number 0). The folded link
            //    equations are e1 and e2.
            // 3. dimension corresponds to uncontrolled lists like x[#a] or x[#a, #i] in each link equation (including the main equation)
            //    So 3. dimension unfolds the folded equations, for instance x[#a] in e1 and y[#a] in e2.
            // The 1. dimension (super) is kind of like link variables, but where the x[#a] variables have no "mother" equation
            // to be put into. Each super element is adjusted on its own, and stuff sums to 0. In reporting, this is
            // shown as "equ" to choose/pivot from

            int funcCounter = 0;
            G.Writeln2(">>>Before low level " + DateTime.Now.ToLongTimeString());

            if (decompDatas == null || refresh)  //signals a recalc of data, not a reuse
            {
                decompDatas = new List<List<DecompData>>();

                List<string> expressionTexts = new List<string>();
                int counter2 = -1;
                foreach (Link link in decompOptions2.link)  //including the "mother" non-linked equation
                {
                    counter2++;
                    string residualName = Program.GetDecompResidualName(counter2);
                    List<DecompData> temp = new List<DecompData>();

                    foreach (Func<GekkoSmpl, IVariable> expression in link.expressions)  //for each uncontrolled #i in x[#i]
                    {
                        DecompData dd = Decomp.DecompLowLevel(per1, per2, expression, DecompBanks(operator1), residualName, ref funcCounter);
                        temp.Add(dd);
                    }
                    decompDatas.Add(temp);
                }

                G.Writeln2(">>>After low level " + DateTime.Now.ToLongTimeString());

                List<DecompData> MAIN_decompData = decompDatas[0];  //this is where all the linking ends up. Clone it??

                if (decompOptions2.link[parentI].varnames == null)
                {
                    decompOptions2.link[parentI].varnames = new List<string>() { Globals.decompResidualName };
                }                                

                if (false)
                {
                    DecompPrintDatas(decompDatas, operatorOneOf3Types);
                }

                bool[] used = new bool[decompDatas.Count];
                used[0] = true;  //primary equation

                GekkoDictionary<string, bool> ignore = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

                //linking
                //linking
                //linking

                //------------------------
                //Example: e1: y = c + i + g  --> y - (c + i + g)
                //         e2: c = 0.8 * y    --> c - 0.8 * y
                //------------------------

                //Takes the link equations, skipping the first one (which is the "normal" equation)
                //Example: decomp y in e1 link c in e2
                //the link equation is e2

                List<string> linkVariables = new List<string>();

                for (int i = 1; i < decompOptions2.link.Count; i++)  //skips the MAIN equation
                {
                    //For each link variable (c) in the link equation (e2)
                    for (int n = 0; n < decompOptions2.link[i].varnames.Count; n++)
                    {
                        //adjust the table according to link variable, so it fits with the destination table
                        string linkVariable = Program.databanks.GetFirst().name + ":" + decompOptions2.link[i].varnames[n] + "¤[0]";
                        linkVariables.Add(linkVariable);

                        //if (!ignore.ContainsKey(linkVariable)) ignore.Add(linkVariable, true); //the if should not be necessary, just for safety

                        //TODO TODO
                        //TODO TODO if there > 1 hit here, error or warning should be issued
                        //TODO TODO
                        //looks in the uncontrolled eqs in link # i to find a match
                        int j = FindLinkJ(decompDatas, i, linkVariable, operatorOneOf3Types);  //Example: find row with c in table corresponding to e2

                        for (int parentJ = 0; parentJ < decompDatas[parentI].Count; parentJ++)
                        {
                            //The series below is the lhs series of the whole decomposition. If the rhs or a link contains the lhs variable,
                            //it will be altered, therefore the clone. For instance, in y = c + g and c = 0.8*y, a naive decomp for data where
                            //y changes with 1 each period will only show 0.2. This is corrected below, corresponding to y = 0.8*y + g --> 0.2 y = g --> y = 5*g.

                            //in y = c + i + g // c = 0.8 y
                            //DECOMP y in eq1 link c in eq2.
                            //linkparent would be c from eq1, and linkchild would be c from eq2.
                            //linkparent is always from first equation
                            Series linkParent = FindLinkSeries(decompDatas, parentI, parentJ, linkVariable, operatorOneOf3Types); //Example: decomposed c from e1                       
                                                                                                             //maybe check that all link equations are used, and report if they are not.
                            if (linkParent == null)
                            {
                                continue;
                            }
                            else
                            {
                                used[i] = true; //this link equation is somehow used, for some of its variables and one or more of the primary variables that are going to be decomposed (= super)
                            }
                            Series linkChild = FindLinkSeries(decompDatas, i, j, linkVariable, operatorOneOf3Types); //Example: decomposed c from e2

                            List<double> factors = new List<double>();
                            foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                            {
                                //Example: the factor is -(-1/1) = 1, so that adding the two tables would eliminate c in e1 equation.
                                // y - (c + i + g) + 1*(c - 0.8 * y) = y + i + g - 0.8 * y =  0.2 * y + i + g
                                // when showing this for y, the result must be multiplied by 5.
                                double dLinkParent = linkParent.GetDataSimple(t);
                                double dLinkChild = linkChild.GetDataSimple(t);
                                double factor = -dLinkParent / dLinkChild;  //recalculated for each kvp, but never mind, should not matter much
                                factors.Add(factor);
                            }

                            //for each period, find the variable value in the original equation, and compute
                            //  a correction factor for the sub-equation.     



                            foreach (KeyValuePair<string, Series> kvp in GetDecompDatas(decompDatas[i][j], operatorOneOf3Types).storage)
                            {
                                Series varParent = GetDecompDatas(decompDatas[parentI][parentJ], operatorOneOf3Types)[kvp.Key];  //will be created
                                Series varChild = kvp.Value;

                                int counter = -1;

                                foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                                {
                                    counter++;
                                    //if (G.Equal(kvp.Key, linkVariable)) continue;

                                    double dVarParent = varParent.GetDataSimple(t);
                                    if (G.isNumericalError(dVarParent)) dVarParent = 0d;  //it usually does not exist beforehand
                                    double dVarChild = varChild.GetDataSimple(t);
                                    double x = dVarParent + factors[counter] * dVarChild;
                                    GetDecompDatas(decompDatas[parentI][parentJ], operatorOneOf3Types)[kvp.Key].SetData(t, x);
                                }
                            }
                        }
                    }
                }

                //remove linked variables from result (are 0)
                List<string> problem = new List<string>();
                foreach (string linkVariable in linkVariables)
                {
                    for (int parentJ = 0; parentJ < decompDatas[parentI].Count; parentJ++)
                    {
                        DecompDict dd = GetDecompDatas(decompDatas[parentI][parentJ], operatorOneOf3Types);                        
                        if (IsAlmostZeroTimeseries(per1, per2, dd[linkVariable], 1e-10d))
                        {
                            bool b = dd.Remove(linkVariable);
                        }
                        else
                        {
                            problem.Add(linkVariable);
                        }
                    }
                }
                foreach (string linkVariable in problem)
                {
                    G.Writeln("NOTE: DECOMP: Variable " + linkVariable + " is not eliminated");
                }
                
                for (int parentJ = 0; parentJ < decompDatas[parentI].Count; parentJ++)
                {
                    List<string> remove = new List<string>();
                    DecompDict dd = GetDecompDatas(decompDatas[parentI][parentJ], operatorOneOf3Types);
                    foreach (KeyValuePair<string, Series> kvp in dd.storage)
                    {
                        string s = kvp.Key;                        
                        string[] ss = s.Split('¤');
                        string s2 = G.Chop_RemoveBank(ss[0], Program.databanks.GetFirst().name);
                        if (s2.StartsWith(Globals.decompResidualName))
                        {
                            //TODO TODO TODO
                            //TODO TODO TODO
                            //TODO TODO TODO
                            //TODO TODO TODO
                            //TODO TODO TODO
                            //TODO TODO TODO threshold should be decimals used in GUI!!
                            //TODO TODO TODO
                            //TODO TODO TODO
                            //TODO TODO TODO
                            //TODO TODO TODO
                            //TODO TODO TODO
                            //TODO TODO TODO
                            if (IsAlmostZeroTimeseries(per1, per2, kvp.Value, 1e-5d))
                            {
                                remove.Add(kvp.Key);
                            }
                        }
                    }
                    foreach(string s in remove)
                    {
                        bool b = dd.Remove(s);
                    }
                }

                //TODO: make sure that every lhs variable is found 1 and only 1 time
                //      in the decompDatas, and report error if not.
                //      To do this, FindLinkJ() and FindLinkSeries() need adjustments

                //correct if the lhs variable is not stated with an implicit 1, like
                //y = c + g, but instead 2*y = 2*c +2*g, or y = c + g, c = 0.8 * y ---> 0.2 * y = g,
                //the last one must be multiplied with 5.                
                
                //At this point, all linked equations i = 1, 2, ... have been merged into
                //the MAIN equation i = 0.

                for (int i = 0; i < decompDatas.Count; i++)
                {
                    if (used[i] != true)
                    {
                        G.Writeln2("+++ WARNING: did not use link-equation #" + i + " of " + (decompDatas.Count - 1) + " (is it superfluous?)");
                    }
                }
            }

            //decompDatas[parentI] is the main equation, the other ones are in-substituted. This decompDatas[parentI] has a member
            //for each uncontrolled set like #a. The main variables (MAIN_varnames) are normalized to 1.
            //decompData.cellsContribD contains keys like "Work:y[19]¤[+1]" with values as timeseries.
            //This example is split into y, #a, 1, t, value --> so we get a dataframe row like this:
            //eq=0, variable=y, #a = 19, lag=1, t=2010, 1.2345
            //We clone the data first, before calling DecompPivotToTable(), because they may be normalized etc. 
            List<DecompData> decompDatasSupremeClone = new List<DecompData>();
            foreach (DecompData dd in decompDatas[parentI]) decompDatasSupremeClone.Add(dd.DeepClone());                        
            Table table = Decomp.DecompPivotToTable(decompOptions2.link[parentI].varnames, per1, per2, decompDatasSupremeClone, decompOptions2.decompTablesFormat, operator1, isShares, smpl, lhsString, decompOptions2.link[parentI].expressionText, decompOptions2, frame, operatorOneOf3Types);  

            if (false)
            {
                DecompPrintDatas(decompDatas, operatorOneOf3Types);
                throw new GekkoException();
            }

            G.Writeln2("DECOMP took " + G.SecondsFormat((DateTime.Now - t0).TotalMilliseconds) + ", function evals = " + funcCounter);

            return table;
        }

        //public static DecompDict GetDecompDatas(List<List<DecompData>> decompDatas, int i, int j, EContribType operatorOneOf3Types)
        //{
        //    return GetDecompDatas(decompDatas[i][j], operatorOneOf3Types);
        //}

        public static DecompDict GetDecompDatas(DecompData decompData, EContribType operatorOneOf3Types)
        {
            if (operatorOneOf3Types == EContribType.D) return decompData.cellsContribD;
            else if (operatorOneOf3Types == EContribType.RD) return decompData.cellsContribDRef;
            else if (operatorOneOf3Types == EContribType.M) return decompData.cellsContribM;
            else
            {
                return decompData.cellsContribD;  //just to get something going when doing for instance "xn" option
            }
        }

        private static bool IsAlmostZeroTimeseries(GekkoTime per1, GekkoTime per2, Series xx, double eps)
        {
            bool isZero = true;
            foreach (GekkoTime t in new GekkoTimeIterator(per1.Add(Globals.decompPerLag), per2))
            {
                double d = xx.GetDataSimple(t);
                if (!G.isNumericalError(d) && Math.Abs(d) > eps)
                {
                    isZero = false;
                    break;
                }
            }

            return isZero;
        }

        public static void DecompGetFuncExpressionsAndRecalc(DecompOptions2 o)
        {
            DecompOptions2 decompOptions = (DecompOptions2)o;
            WindowDecomp w = null;
            w = new WindowDecomp(decompOptions);
            Globals.windowsDecomp2.Add(w);

            G.Writeln2(">>>getexpressions start " + DateTime.Now.ToLongTimeString());
            int count = -1;
            foreach (Link link in decompOptions.link)
            {
                count++;
                if (Program.modelGams != null)
                {
                    if (link.expressions.Count == 1 && link.expressions[0] == null)
                    {
                        ModelGamsEquation found = Program.DecompEvalGams(link.eqname, link.varnames[0]);  //if link.eqname != null, link.varnames[0] is not used at all
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
                    if (true)
                    {
                        if (link.expressions.Count == 1 && link.expressions[0] == null)
                        {
                            EquationHelper found = DecompEvalGekko(link.varnames[0]);
                            //decompOptions.expression = Globals.expressions[0];
                            link.expressions = found.expressions;
                            decompOptions.expressionOld = found.equationText;
                        }
                        else
                        {
                            //fix this...
                        }
                    }
                    else
                    {
                        EquationHelper found = Program.DecompEval(decompOptions.variable);
                        decompOptions.expression = Globals.expressions[0];
                        decompOptions.expressionOld = found.equationText;
                    }
                }
            }
            G.Writeln2(">>>getexpressions end " + DateTime.Now.ToLongTimeString());

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

            w.RecalcCellsWithNewType(true);
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

        public static DecompData DecompLowLevel(GekkoTime tt1, GekkoTime tt2, Func<GekkoSmpl, IVariable> expression, EDecompBanks workOrRefOrBoth, string residualName, ref int funcCounter)
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
            
            DateTime dt = DateTime.Now;

            GekkoSmpl smpl = new GekkoSmpl(tt1, tt2);
            IVariable y0a = null;
            IVariable y0aRef = null;
            
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
                                    double x_before = x_series.GetDataSimple(t1);

                                    try
                                    {
                                        double x_after = x_before + eps;
                                        x_series.SetData(t1, x_after);

                                        //Function call start --------------
                                        O.AdjustSmplForDecomp(smpl, 0);  //no reason to enlarge this smpl with 10 pers at both ends, since it is only t2 that is written afterwards
                                        if (j == 1) smpl.bankNumber = 1;
                                        IVariable y1 = null;

                                        if (true)  //this is what takes most of the time in DECOMP
                                        {
                                            y1 = expression(smpl); funcCounter++;  // <============================ THIS TAKES TIME!
                                        }

                                        if (j == 1) smpl.bankNumber = 0;
                                        O.AdjustSmplForDecomp(smpl, 1);
                                        //Function call end   --------------

                                        Series y1_series = y1 as Series;
                                        string nameOriginal = G.Chop_FreqRemove(dp.s, tt1.freq);

                                        if (true)  //this does not seem to cost any time...?
                                        {
                                            foreach (GekkoTime t2 in new GekkoTimeIterator(tt1.Add(Globals.decompPerLag), tt2.Add(0)))
                                            {
                                                double y0_double = y_series.GetDataSimple(t2);
                                                double y1_double = y1_series.GetDataSimple(t2);
                                                double grad = (y1_double - y0_double) / eps;

                                                if (!G.isNumericalError(grad) && grad != 0d)
                                                {
                                                    //For the gradient to be a real number <> 0, the expression must evaluate
                                                    //before shock (y0) in the year considered (t2)
                                                    //If it does evaluate, but there is no effect, it is skipped too.

                                                    int lag = -(GekkoTime.Observations(t1, t2) - 1);  //x[-1] --> lag = -1                                                                                        
                                                    string lag2 = null;
                                                    if (lag >= 1)
                                                    {
                                                        lag2 = "+" + lag.ToString();
                                                    }
                                                    else
                                                    {
                                                        lag2 = lag.ToString();
                                                    }
                                                    string name = nameOriginal + "¤[" + lag2 + "]";

                                                    if (lag == 0 || (lag < 0 && -lag <= Program.options.decomp_maxlag) || (lag > 0 && lag <= Program.options.decomp_maxlead))
                                                    {

                                                        if (j == 0)
                                                        {
                                                            d.cellsQuo[name].SetData(t2, x_before);
                                                        }
                                                        else
                                                        {
                                                            d.cellsRef[name].SetData(t2, x_before);
                                                        }

                                                        if (j == 0)
                                                        {
                                                            d.cellsGradQuo[name].SetData(t2, grad);
                                                        }
                                                        else
                                                        {
                                                            d.cellsGradRef[name].SetData(t2, grad);
                                                        }
                                                    }

                                                    if (!vars.ContainsKey(name))
                                                    {
                                                        //list of relevant variables to handle later on
                                                        //in decomp pivot
                                                        vars.Add(name, 0);
                                                    }
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

                            double vQuo = d.cellsQuo[s].GetDataSimple(t2);
                            double vQuoLag = d.cellsQuo[s].GetDataSimple(t2.Add(-1));
                            double vGradQuoLag = d.cellsGradQuo[s].GetDataSimple(t2.Add(-1));
                            //double vGradQuo = d.cellsGradQuo[s].GetData(smpl, t2); --> not used at the moment
                            double dContribD = vGradQuoLag * (vQuo - vQuoLag);
                            d.cellsContribD[s].SetData(t2, dContribD);

                            if (Globals.runningOnTTComputer && false) G.Writeln2(s + " quo " + vQuo + " quo.1 " + vQuoLag + " grad.1 " + vGradQuoLag + " " + dContribD);

                            if (mm.Contains(1))
                            {
                                double vRef = d.cellsRef[s].GetDataSimple(t2);
                                double vRefLag = d.cellsRef[s].GetDataSimple(t2.Add(-1));
                                double vGradRef = d.cellsGradRef[s].GetDataSimple(t2);
                                double vGradRefLag = d.cellsGradRef[s].GetDataSimple(t2.Add(-1));
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

            return d;

        }

        public static Table DecompPivotToTable(List<string> main_varnames, GekkoTime per1, GekkoTime per2, List<DecompData> decompDatasSupremeClone, DecompTablesFormat2 format, string operator1, string isShares, GekkoSmpl smpl, string lhs, string expressionText, DecompOptions2 decompOptions2, FrameLight frame, EContribType operatorOneOf3Types)
        {
            int parentI = 0;

            if (!operator1.StartsWith("x"))
            {
                DecompNormalize(per1, per2, decompOptions2, parentI, decompDatasSupremeClone, operatorOneOf3Types);
            }

            bool ageHierarchy = Globals.isAgeHierarchy;
            if (G.IsUnitTesting()) ageHierarchy = false;

            if (decompOptions2.rows.Count == 0 && decompOptions2.cols.Count == 0)
            {
                decompOptions2.rows = new List<string>() { "vars" };
                decompOptions2.cols = new List<string>() { "time" };
            }

            if (decompOptions2.filters == null)
            {
                decompOptions2.filters = new List<FrameFilter>();
                if (decompOptions2.where != null)
                {
                    foreach (List<string> filter in decompOptions2.where)
                    {
                        FrameFilter filter1 = new FrameFilter();
                        filter1.active = true;
                        filter1.name = filter[filter.Count - 1];
                        filter1.selected = filter.GetRange(0, filter.Count - 1);
                        decompOptions2.filters.Add(filter1);
                    }
                }
            }

            //The DataTable dt will get the following colums:
            //<t>:         time
            //<variable>:  variable name, like fy or pop
            //<lag>:       lag or lead
            //<#universe>: universal set for elements without domain info
            //#i:          set names, like #age, #sector, etc.
            //<value>:     data value

            string col_t = Globals.internalColumnIdentifyer + "t";
            string col_variable = Globals.internalColumnIdentifyer + "variable";
            string col_lag = Globals.internalColumnIdentifyer + "lag";
            string col_universe = Globals.internalColumnIdentifyer + "universe";
            string col_value = Globals.internalColumnIdentifyer + "value";
            string col_valueLevel = Globals.internalColumnIdentifyer + "valueLevel";
            string col_valueLevelLag = Globals.internalColumnIdentifyer + "valueLevelLag";
            string col_valueLevelRef = Globals.internalColumnIdentifyer + "valueLevelRef";
            string col_valueLevelRefLag = Globals.internalColumnIdentifyer + "valueLevelRefLag";
            string gekko_null = "null";
            string col_equ = Globals.internalColumnIdentifyer + "equ";

            frame.AddColName(col_t);
            frame.AddColName(col_value);
            frame.AddColName(col_valueLevel);
            frame.AddColName(col_valueLevelLag);
            frame.AddColName(col_valueLevelRef);
            frame.AddColName(col_valueLevelRefLag);
            frame.AddColName(col_variable);
            frame.AddColName(col_lag);
            frame.AddColName(col_universe);
            frame.AddColName(col_equ);
            if (ageHierarchy)
            {
                frame.AddColName(Globals.internalSetIdentifyer + Globals.ageHierarchyName);
            }

            int superN = decompDatasSupremeClone.Count;

            //adding frame rows, while also getting sets defined for variables (these are added as frame cols)

            for (int super = 0; super < superN; super++)  //equations, like if y[#a] = x[#a] + 5, superN will correspond to number of elements in #a.
            {
                int j = 0;
                foreach (GekkoTime t2 in new GekkoTimeIterator(per1, per2))
                {
                    j++;
                    int i = 0;
                    double lhsSum = 0d;
                    double rhsSum = 0d;

                    foreach (string varname in GetDecompDatas(decompDatasSupremeClone[super], operatorOneOf3Types).storage.Keys)
                    {
                        i++;

                        string dbName = null; string varName = null; string freq = null; string[] indexes = null;
                        string[] domains = null;

                        //See #876435924365

                        string lag = null;

                        //there is some repeated work done here, but not really bad
                        //problem is we prefer to do one period at a time, to sum up, adjust etc.

                        string[] ss = varname.Split('¤');
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
                                        domains[ii] = ConvertSetname(Globals.internalSetIdentifyer, col_universe, ts.mmi.parent.meta.domains[ii]);
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

                        double dLevel = double.NaN;
                        double dLevelLag = double.NaN;
                        double dLevelRef = double.NaN;
                        double dLevelRefLag = double.NaN;
                        
                        
                        if (operator1.StartsWith("x"))
                        {
                            if (varname.Contains(Globals.decompResidualName))
                            {
                                dLevel = double.NaN;
                            }
                            else
                            {

                                if (operatorOneOf3Types == EContribType.N || operatorOneOf3Types == EContribType.M || operatorOneOf3Types == EContribType.D)
                                {
                                    Series tsFirst = null;
                                    tsFirst = O.GetIVariableFromString(fullName, O.ECreatePossibilities.NoneReturnNull) as Series;
                                    if (tsFirst == null)
                                    {
                                        G.Writeln2("*** ERROR: Decomp #7093473984");
                                        throw new GekkoException();
                                    }
                                    dLevel = tsFirst.GetDataSimple(t2);
                                    dLevelLag = tsFirst.GetDataSimple(t2.Add(-1));
                                }

                                if (operatorOneOf3Types == EContribType.RN || operatorOneOf3Types == EContribType.M || operatorOneOf3Types == EContribType.RD)
                                {
                                    Series tsRef = null;
                                    tsRef = O.GetIVariableFromString(G.Chop_SetBank(fullName, "Ref"), O.ECreatePossibilities.NoneReturnNull) as Series;
                                    if (tsRef == null)
                                    {
                                        G.Writeln2("*** ERROR: Decomp #7093473985");
                                        throw new GekkoException();
                                    }
                                    dLevelRef = tsRef.GetDataSimple(t2);
                                    dLevelRefLag = tsRef.GetDataSimple(t2.Add(-1));
                                }
                            }
                        }                        

                        double d = DecomposePutIntoTable2HelperOperators(decompDatasSupremeClone[super], operator1, smpl, lhs, t2, varname);
                        
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
                        dr.Set(frame, col_valueLevel, new CellLight(dLevel));
                        dr.Set(frame, col_valueLevelLag, new CellLight(dLevelLag));
                        dr.Set(frame, col_valueLevelRef, new CellLight(dLevelRef));
                        dr.Set(frame, col_valueLevelRefLag, new CellLight(dLevelRefLag));

                        frame.rows.Add(dr);
                    }
                }
            }

            if (ageHierarchy && FrameLightRow.HasColumn(frame, Globals.internalSetIdentifyer + "a"))
            {
                foreach (FrameLightRow row in frame.rows)
                {
                    CellLight c = row.Get(frame, Globals.internalSetIdentifyer + "a");
                    string s = c.text;
                    int i = -12345;
                    string s2 = "null";
                    if (int.TryParse(s, out i))
                    {
                        s2 = G.GroupBy10(i);
                    }
                    //set a new column with aggregated ages
                    row.Set(frame, Globals.internalSetIdentifyer + Globals.ageHierarchyName, new CellLight(s2));
                }
            }

            if (Globals.decompUnitPivot)
            {
                WriteDatatableTocsv(frame);
            }

            Table tab = new Table();
            tab.writeOnce = true;

            DecomposeReplaceVars(decompOptions2.rows, col_t, col_variable, col_lag, col_universe, col_equ);
            DecomposeReplaceVars(decompOptions2.cols, col_t, col_variable, col_lag, col_universe, col_equ);
            DecomposeReplaceVars(decompOptions2.filters, col_t, col_variable, col_lag, col_universe, col_equ);

            List<string> rownames3 = new List<string>();
            List<string> colnames3 = new List<string>();
            GekkoDictionary<string, FiveDouble> agg = new GekkoDictionary<string, FiveDouble>(StringComparer.OrdinalIgnoreCase);

            //get the free values start
            bool getFreeValues = false;
            if (decompOptions2.freeValues == null)
            {
                decompOptions2.freeValues = new List<GekkoDictionary<string, string>>();
                getFreeValues = true;
            }
            int valueI = FrameLightRow.FindColumn(frame, G.HandleInternalIdentifyer2("value"));
            for (int i = 0; i < frame.colnames.Count; i++)
            {
                decompOptions2.freeValues.Add(new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase));
            }
            //get the free values end

            //Aggregation
            //Aggregation
            //Aggregation

            foreach (FrameLightRow row in frame.rows)
            {
                if (getFreeValues)
                {
                    for (int i = 0; i < frame.colnames.Count; i++)
                    {
                        if (i == valueI) continue;
                        string s = row.storage[i].text;
                        if (s == null) s = "null";
                        if (!decompOptions2.freeValues[i].ContainsKey(s)) decompOptions2.freeValues[i].Add(s, null);
                    }
                }

                bool skip = false;
                foreach (FrameFilter filter in decompOptions2.filters)
                {
                    CellLight c = row.Get(frame, filter.name);
                    if (c.type != ECellLightType.String && c.type != ECellLightType.None) throw new GekkoException();
                    string ss = c.text;
                    if (c.type == ECellLightType.None || !filter.selected.Contains(ss, StringComparer.OrdinalIgnoreCase))
                    {
                        //not part of the filter, is ignored
                        //if the row has a null value regarding the filter, the row is also ignored (for instance, if #a must be 18 and the row has #a = null, the row is ignored)
                        skip = true;
                        break;
                    }
                }
                if (skip) continue;

                string s1 = null;
                foreach (string s in decompOptions2.rows)
                {
                    s1 = DecompAddText(frame, row, s1, s);
                }
                if (s1 != null)
                    s1 = s1.Substring(Globals.pivotTableDelimiter.Length);

                string s2 = null;
                foreach (string s in decompOptions2.cols)
                {
                    s2 = DecompAddText(frame, row, s2, s);
                }
                if (s2 != null)
                    s2 = s2.Substring(Globals.pivotTableDelimiter.Length);
                string key = s1 + "¤" + s2;

                if (!rownames3.Contains(s1, StringComparer.OrdinalIgnoreCase)) rownames3.Add(s1);
                if (!colnames3.Contains(s2, StringComparer.OrdinalIgnoreCase)) colnames3.Add(s2);
                                
                double d = row.Get(frame, col_value).data;
                double dLevel = row.Get(frame, col_valueLevel).data;
                double dLevelLag = row.Get(frame, col_valueLevelLag).data;
                double dLevelRef = row.Get(frame, col_valueLevelRef).data;
                double dLevelRefLag = row.Get(frame, col_valueLevelRefLag).data;

                FiveDouble td = null;
                agg.TryGetValue(key, out td);
                if (td == null)
                {
                    agg.Add(key, new FiveDouble(d, dLevel, dLevelLag, dLevelRef, dLevelRefLag));
                }
                else
                {
                    td.change += d;
                    td.level += dLevel;
                    td.levelLag += dLevelLag;
                    td.levelRef += dLevelRef;
                    td.levelRefLag += dLevelRefLag;                    
                }
            }

            rownames3.Sort(StringComparer.OrdinalIgnoreCase);
            List<string> rownames2 = new List<string>();
            foreach (var rowname in rownames3.OrderBy(x => x, new G.NaturalComparer(G.NaturalComparerOptions.Default))) rownames2.Add(rowname);
            rownames3 = rownames2;

            colnames3.Sort(StringComparer.OrdinalIgnoreCase);
            List<string> colnames2 = new List<string>();
            foreach (var colname in colnames3.OrderBy(x => x, new G.NaturalComparer(G.NaturalComparerOptions.Default))) colnames2.Add(colname);
            colnames3 = colnames2;

            List<string> varnames = decompOptions2.link[parentI].varnames;
            bool orderNormalize = OrderNormalize(decompOptions2, varnames);

            List<string> rownames = new List<string>();
            List<string> colnames = new List<string>();

            string rownamesFirst = null;
            for (int i = 0; i < rownames3.Count; i++)
            {
                if (rownamesFirst == null && orderNormalize && DecompMatchWord(rownames3[i], varnames[0]))
                {
                    rownamesFirst = rownames3[i];
                }
                else
                {
                    rownames.Add(rownames3[i]);
                }
            }

            string colnamesFirst = null;
            for (int i = 0; i < colnames3.Count; i++)
            {
                if (colnamesFirst == null && orderNormalize && DecompMatchWord(colnames3[i], varnames[0]))
                {
                    colnamesFirst = colnames3[i];
                }
                else
                {
                    colnames.Add(colnames3[i]);
                }
            }

            if (rownamesFirst != null) rownames.Insert(0, rownamesFirst);
            if (colnamesFirst != null) colnames.Insert(0, colnamesFirst);
            rownames3 = null;
            colnames3 = null;

            if (orderNormalize && rownamesFirst == null && colnamesFirst == null)
            {
                MessageBox.Show("*** ERROR: Could not find row/col to put first for normalization");
            }

            for (int i = 0; i < rownames.Count; i++)
            {
                for (int j = 0; j < colnames.Count; j++)
                {
                    string key = rownames[i] + "¤" + colnames[j];                    
                    FiveDouble td = null;
                    agg.TryGetValue(key, out td);
                    double d = 0d;
                    double dLevel = 0d;
                    double dLevelLag = 0d;
                    double dLevelRef = 0d;
                    double dLevelRefLag = 0d;

                    if (td != null)
                    {

                        dLevel = td.level;
                        dLevelLag = td.levelLag;
                        dLevelRef = td.levelRef;
                        dLevelRefLag = td.levelRefLag;

                        if (operator1 == "xn")
                        {
                            d = dLevel;
                        }
                        else if (operator1 == "xrn")
                        {
                            d = dLevelRef;
                        }
                        else if (operator1 == "d")
                        {
                            d = td.change;
                        }
                        else if (operator1 == "m")
                        {
                            d = td.change;
                        }
                        else if (operator1 == "xd")
                        {
                            d = dLevel - dLevelLag;                            
                        }
                        else if (operator1 == "xp")
                        {
                            d = (dLevel - dLevelLag) / dLevelLag * 100d;
                        }
                        else if (operator1 == "xm")
                        {
                            d = dLevel - dLevelRef;
                        }
                        else if (operator1 == "xq")
                        {
                            d = (dLevel - dLevelRef) / dLevelRef * 100d;
                        }
                    }

                    int decimals = 0;
                    if (decompOptions2.decompTablesFormat.isPercentageType) decimals = decompOptions2.decompTablesFormat.decimalsPch;
                    else decimals = decompOptions2.decompTablesFormat.decimalsLevel;
                    string format2 = "f16." + decimals.ToString();                    
                    tab.SetNumber(i + 2, j + 2, d, format2);
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

        private static void DecompNormalize(GekkoTime per1, GekkoTime per2, DecompOptions2 decompOptions2, int parentI, List<DecompData> decompDatasSupremeClone, EContribType operatorOneOf3Types)
        {
            EDecompBanks edb = DecompBanks(decompOptions2.prtOptionLower);

            bool orderNormalize = OrderNormalize(decompOptions2, decompOptions2.link[parentI].varnames);
            //This normalizes the parent-link-variables so that they reflect their real values
            //Parent-link-variables are for instance x1, x2, x3 here: DECOMP x1, x2, x2 IN ...

            if (orderNormalize)
            {
                if (decompOptions2.link[parentI].varnames.Count != decompDatasSupremeClone.Count)
                {
                    G.Writeln2("*** ERROR: The number of variables and equations do not match. For istance, in ");
                    G.Writeln("           DECOMP x1, x2 in e_eqs, the equation e_eqs must contain 2 elements (that is,", System.Drawing.Color.Red);
                    G.Writeln("           it must be defined over one or more sets with 2 elements in all).", System.Drawing.Color.Red);
                    throw new GekkoException();
                }
                for (int j = 0; j < decompOptions2.link[parentI].varnames.Count; j++)
                {
                    string name = decompOptions2.link[parentI].varnames[j];

                    bool isResidualName = name == Globals.decompResidualName;

                    string name1 = Program.databanks.GetFirst().name + ":" + name + "¤[0]";  //what about lags in eqs??
                    string name2 = Program.databanks.GetFirst().name + ":" + name;
                    string name2Ref = Program.databanks.GetRef().name + ":" + name;

                    if (GetDecompDatas(decompDatasSupremeClone[j], operatorOneOf3Types).ContainsKey(name1))
                    {
                        Series lhs2 = GetDecompDatas(decompDatasSupremeClone[j], operatorOneOf3Types)[name1];
                        Series lhsReal = null;
                        Series lhsRealRef = null;
                        if (isResidualName)
                        {
                            //just keep lhsReal/lhsRealRef = null
                        }
                        else
                        {                            
                            if (edb == EDecompBanks.Work)
                            {
                                lhsReal = O.GetIVariableFromString(name2, O.ECreatePossibilities.NoneReportError) as Series;
                            }
                            else if (edb == EDecompBanks.Ref)
                            {
                                lhsRealRef = O.GetIVariableFromString(name2Ref, O.ECreatePossibilities.NoneReportError) as Series;
                            }
                            else if (edb == EDecompBanks.Both)
                            {
                                lhsReal = O.GetIVariableFromString(name2, O.ECreatePossibilities.NoneReportError) as Series;
                                lhsRealRef = O.GetIVariableFromString(name2Ref, O.ECreatePossibilities.NoneReportError) as Series;
                            }
                        }

                        DecompData d = decompDatasSupremeClone[j];
                        foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                        {
                            double d1 = lhs2.GetDataSimple(t);
                            double factor = 1d;

                            if (isResidualName)
                            {
                                //keep factor = 1
                            }
                            else
                            {
                                // --------------------------------------------
                                //TODO: other operators
                                //TODO: other operators
                                //TODO: other operators
                                //TODO: other operators, this is <d>
                                //TODO: other operators
                                //TODO: other operators
                                //TODO: other operators

                                //Stuff below does not always work: the variable to be shown may not even be in the first equation (for instance: DECOMP y[#a] in demand[#a] = supply[#a]...
                                //So for now, we allow it to be fetched from the databank
                                //Series temp = decompDatasSupremeClone[j].cellsQuo.storage[name1];
                                //double d2 = temp.GetDataSimple(t) - temp.GetDataSimple(t.Add(-1));        
                                
                                double d2 = double.NaN;

                                if (operatorOneOf3Types == EContribType.D)
                                {
                                    d2 = lhsReal.GetDataSimple(t) - lhsReal.GetDataSimple(t.Add(-1));
                                }
                                else if (operatorOneOf3Types == EContribType.RD)
                                {
                                    d2 = lhsRealRef.GetDataSimple(t) - lhsRealRef.GetDataSimple(t.Add(-1));
                                }
                                else if (operatorOneOf3Types == EContribType.M)
                                {
                                    d2 = lhsReal.GetDataSimple(t) - lhsRealRef.GetDataSimple(t);
                                }

                                // ----------------------------------------------

                                factor = d2 / d1;
                            }

                            if (true)
                            {
                                bool found = false;
                                foreach (KeyValuePair<string, Series> kvp in GetDecompDatas(d, operatorOneOf3Types).storage)
                                {
                                    if (G.Equal(kvp.Key, name1))
                                    {
                                        kvp.Value.SetData(t, factor * kvp.Value.GetDataSimple(t));
                                        found = true;
                                    }
                                    else
                                    {
                                        //switch sign!
                                        kvp.Value.SetData(t, -factor * kvp.Value.GetDataSimple(t));
                                    }
                                }
                                if (found == false)
                                {
                                    MessageBox.Show("*** ERROR: Did not find " + name1 + " for normalization");
                                }
                            }
                        }
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Could not find variable " + name1 + " in non-linked equation number " + j);
                        G.Writeln("           Beware of alignment: the names and equations must match.", System.Drawing.Color.Red);
                        throw new GekkoException();
                    }
                }
            }

            return;
        }

        public static EContribType DecompContribTypeHelper(string prtOptionLower)
        {
            string op = prtOptionLower;
            if (op.StartsWith("x")) op = op.Substring(1);
            if (op.StartsWith("s")) op = op.Substring(1);
            EContribType ect = DecompContribType(op);
            return ect;
        }

        public static EContribType DecompContribType(string op)
        {
            EContribType ect = EContribType.Unknown;

            if (op == "n")
            {
                ect = EContribType.N;
            }
            else if (op == "rn")
            {
                ect = EContribType.RN;
            }
            else if (op == "d" || op == "p" || op == "dp")
            {
                ect = EContribType.D;
            }
            else if (op == "rd" || op == "rp" || op == "rdp")
            {
                ect = EContribType.RD;
            }
            else if (op == "m" || op == "q" || op == "mp")
            {
                ect = EContribType.M;
            }
            return ect;
        }

        private static bool DecompMatchWord(string colnames3, string varnames)
        {
            return G.ContainsWord(colnames3, G.Chop_GetName(varnames));
        }

        private static bool OrderNormalize(DecompOptions2 decompOptions2, List<string> varnames)
        {            
            bool orderNormalize = false;
            if (decompOptions2.decompTablesFormat.showErrors)
            {
                if (varnames.Count == decompOptions2.link[0].expressions.Count)
                {
                    orderNormalize = true;
                }
                else
                {
                    MessageBox.Show("+++ WARNING: Normalization ordering not implemented for sets of equations");
                }
            }

            return orderNormalize;
        }

        private static void DecomposeReplaceVars(List<string> vars, string col_t, string col_variable, string col_lag, string col_universe, string col_equ)
        {
            for (int i = 0; i < vars.Count; i++)
            {
                if (G.Equal(vars[i], "time")) vars[i] = col_t;
                if (G.Equal(vars[i], "vars")) vars[i] = col_variable;
                if (G.Equal(vars[i], "lags")) vars[i] = col_lag;
                if (G.Equal(vars[i], "#uni")) vars[i] = col_universe;
                if (G.Equal(vars[i], "equ")) vars[i] = col_equ;
                if (vars[i].StartsWith("#")) vars[i] = Globals.internalSetIdentifyer + vars[i].Substring(1);
            }
        }

        public static void DecomposeReplaceVars(List<FrameFilter> vars, string col_t, string col_variable, string col_lag, string col_universe, string col_equ)
        {
            for (int i = 0; i < vars.Count; i++)
            {
                if (G.Equal(vars[i].name, "time")) vars[i].name = col_t;
                if (G.Equal(vars[i].name, "vars")) vars[i].name = col_variable;
                if (G.Equal(vars[i].name, "lags")) vars[i].name = col_lag;
                if (G.Equal(vars[i].name, "#uni")) vars[i].name = col_universe;
                if (G.Equal(vars[i].name, "equ")) vars[i].name = col_equ;
                if (vars[i].name.StartsWith("#")) vars[i].name = Globals.internalSetIdentifyer + vars[i].name.Substring(1);
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
                s1 += Globals.pivotTableDelimiter + "null";
            }
            else if (c.type == ECellLightType.String)
            {
                s1 += Globals.pivotTableDelimiter + c.text;
            }
            else
            {
                throw new GekkoException();
            }
            return s1;
        }

        public static void WriteDatatableTocsv(FrameLight dt)
        {
            StringBuilder sb = new StringBuilder();
            List<string> columnNames = new List<string>(dt.colnames);
            for (int i = 0; i < columnNames.Count; i++)
            {
                columnNames[i] = G.HandleInternalIdentifyer1(columnNames[i]);
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
            //File.WriteAllText(@"c:\Thomas\Gekko\regres\Models\Decomp\pivot.csv", sb.ToString());
            File.WriteAllText(Program.options.folder_working + "\\" + "decomp.csv", sb.ToString());
        }

        

        public static double DecomposePutIntoTable2HelperOperators(DecompData decompTables, string code1, GekkoSmpl smpl, string lhs, GekkoTime t2, string colname)
        {
            //double d = double.NaN;
            //if (code1 == "n" || code1 == "xn" || code1 == "x")
            //{
            //    d = decompTables.cellsQuo[colname].GetData(smpl, t2);  //for instance {"x¤2002", 2.5} or {"x[-1]¤2003", -1.5}
            //}
            //else if (code1 == "r" || code1 == "xr" || code1 == "xrn")
            //{
            //    d = decompTables.cellsRef[colname].GetData(smpl, t2);
            //}
            //else if (code1 == "d")
            //{
            //    d = decompTables.cellsContribD[colname].GetData(smpl, t2);
            //}
            //else if (code1 == "rd")
            //{
            //    d = decompTables.cellsContribDRef[colname].GetData(smpl, t2);
            //}
            //else if (code1 == "xd")
            //{
            //    double d1 = decompTables.cellsQuo[colname].GetData(smpl, t2);
            //    double d0 = decompTables.cellsQuo[colname].GetData(smpl, t2.Add(-1));
            //    d = d1 - d0;
            //}
            //else if (code1 == "xrd")
            //{
            //    double d1 = decompTables.cellsRef[colname].GetData(smpl, t2);
            //    double d0 = decompTables.cellsRef[colname].GetData(smpl, t2.Add(-1));
            //    d = d1 - d0;
            //}
            //else if (code1 == "m")
            //{
            //    d = decompTables.cellsContribM[colname].GetData(smpl, t2);
            //}
            //else if (code1 == "xm")
            //{
            //    double d1 = decompTables.cellsQuo[colname].GetData(smpl, t2);
            //    double d0 = decompTables.cellsRef[colname].GetData(smpl, t2);
            //    d = d1 - d0;
            //}
            //else if (code1 == "p")
            //{
            //    double dd = decompTables.cellsContribD[colname].GetData(smpl, t2);
            //    double dLhsLag = decompTables.cellsQuo[lhs].GetData(smpl, t2.Add(-1));
            //    d = (dd / dLhsLag) * 100d;
            //}
            //else if (code1 == "rp")
            //{
            //    double dd = decompTables.cellsContribDRef[colname].GetData(smpl, t2);
            //    double dLhsLag = decompTables.cellsRef[lhs].GetData(smpl, t2.Add(-1));
            //    d = (dd / dLhsLag) * 100d;
            //}
            //else if (code1 == "xp")
            //{
            //    double d1 = decompTables.cellsQuo[colname].GetData(smpl, t2);
            //    double d0 = decompTables.cellsQuo[colname].GetData(smpl, t2.Add(-1));
            //    d = (d1 / d0 - 1d) * 100d;
            //}
            //else if (code1 == "xrp")
            //{
            //    double d1 = decompTables.cellsRef[colname].GetData(smpl, t2);
            //    double d0 = decompTables.cellsRef[colname].GetData(smpl, t2.Add(-1));
            //    d = (d1 / d0 - 1d) * 100d;
            //}
            //else if (code1 == "q")
            //{
            //    double dd = decompTables.cellsContribM[colname].GetData(smpl, t2);
            //    double dLhsLag = decompTables.cellsRef[lhs].GetData(smpl, t2);
            //    d = (dd / dLhsLag) * 100d;
            //}
            //else if (code1 == "xq")
            //{
            //    double d1 = decompTables.cellsQuo[colname].GetData(smpl, t2);
            //    double d0 = decompTables.cellsRef[colname].GetData(smpl, t2);
            //    d = (d1 / d0 - 1d) * 100d;
            //}
            //else if (code1 == "dp")
            //{
            //    double dd = decompTables.cellsContribD[colname].GetData(smpl, t2);
            //    double dd_lag = decompTables.cellsContribD[colname].GetData(smpl, t2.Add(-1));
            //    double dLhsLag = decompTables.cellsQuo[lhs].GetData(smpl, t2.Add(-1));
            //    double dLhsLag_lag = decompTables.cellsQuo[lhs].GetData(smpl, t2.Add(-1).Add(-1));
            //    d = (dd / dLhsLag - dd_lag / dLhsLag_lag) * 100d;
            //}
            //else if (code1 == "rdp")
            //{
            //    double dd = decompTables.cellsContribDRef[colname].GetData(smpl, t2);
            //    double dd_lag = decompTables.cellsContribDRef[colname].GetData(smpl, t2.Add(-1));
            //    double dLhsLag = decompTables.cellsRef[lhs].GetData(smpl, t2.Add(-1));
            //    double dLhsLag_lag = decompTables.cellsRef[lhs].GetData(smpl, t2.Add(-1).Add(-1));
            //    d = (dd / dLhsLag - dd_lag / dLhsLag_lag) * 100d;
            //}
            //else if (code1 == "xdp")
            //{
            //    double d1 = decompTables.cellsQuo[colname].GetData(smpl, t2);
            //    double d1_lag = decompTables.cellsQuo[colname].GetData(smpl, t2.Add(-1));
            //    double d0 = decompTables.cellsQuo[colname].GetData(smpl, t2.Add(-1));
            //    double d0_lag = decompTables.cellsQuo[colname].GetData(smpl, t2.Add(-1).Add(-1));
            //    d = (d1 / d0 - 1d - (d1_lag / d0_lag - 1d)) * 100d;
            //}
            //else if (code1 == "xrdp")
            //{
            //    double d1 = decompTables.cellsRef[colname].GetData(smpl, t2);
            //    double d1_lag = decompTables.cellsRef[colname].GetData(smpl, t2.Add(-1));
            //    double d0 = decompTables.cellsRef[colname].GetData(smpl, t2.Add(-1));
            //    double d0_lag = decompTables.cellsRef[colname].GetData(smpl, t2.Add(-1).Add(-1));
            //    d = (d1 / d0 - 1d - (d1_lag / d0_lag - 1d)) * 100d;
            //}
            //else if (code1 == "mp")  // <p> - <rp>
            //{
            //    double dd = decompTables.cellsContribD[colname].GetData(smpl, t2);
            //    double dLhsLag = decompTables.cellsQuo[lhs].GetData(smpl, t2.Add(-1));

            //    double dd2 = decompTables.cellsContribDRef[colname].GetData(smpl, t2);
            //    double dLhsLag2 = decompTables.cellsRef[lhs].GetData(smpl, t2.Add(-1));
            //    d = (dd / dLhsLag - dd2 / dLhsLag2) * 100d;
            //}
            //else if (code1 == "xmp")
            //{
            //    double d1 = decompTables.cellsQuo[colname].GetData(smpl, t2);
            //    double d0 = decompTables.cellsQuo[colname].GetData(smpl, t2.Add(-1));
            //    double d1_ref = decompTables.cellsRef[colname].GetData(smpl, t2);
            //    double d0_ref = decompTables.cellsRef[colname].GetData(smpl, t2.Add(-1));
            //    d = (d1 / d0 - 1d - (d1_ref / d0_ref - 1d)) * 100d;
            //}
            //else
            //{
            //    MessageBox.Show("*** ERROR: Wrong operator");
            //    throw new GekkoException();
            //}

            double d = double.NaN;
            
            if (code1 == "d" || code1 == "p")
            {
                d = decompTables.cellsContribD[colname].GetData(smpl, t2);
            }
            else if (code1 == "rd" || code1 == "rp")
            {
                d = decompTables.cellsContribDRef[colname].GetData(smpl, t2);
            }            
            else if (code1 == "m" || code1 == "q")
            {
                d = decompTables.cellsContribM[colname].GetData(smpl, t2);
            }           
            else
            {
                //do nothing
            }

            return d;
        }

        public static EDecompBanks DecompBanks(string operator1)
        {
            EDecompBanks banks = EDecompBanks.Work;
            if (operator1 == "r" || operator1 == "xr" || operator1 == "xrn" || operator1 == "rd" || operator1 == "xrd" || operator1 == "rp" || operator1 == "xrp" || operator1 == "rdp" || operator1 == "xrdp") banks = EDecompBanks.Ref;
            if (operator1 == "m" || operator1 == "xm" || operator1 == "q" || operator1 == "xq" || operator1 == "mp" || operator1 == "xmp") banks = EDecompBanks.Both;
            return banks;
        }

        public static void DecompPrintDatas(List<List<DecompData>> decompDatas, EContribType operatorOneOf3Types)
        {
            int c1 = -1;
            foreach (List<DecompData> dd in decompDatas)
            {
                c1++;
                int c2 = -1;
                foreach (DecompData d in dd)
                {
                    c2++;
                    DecompDict dict = GetDecompDatas(d, operatorOneOf3Types);
                    foreach (KeyValuePair<string, Series> kvp in dict.storage)
                    {
                        string nme = kvp.Key;
                        Series ts = kvp.Value;
                        for (int i = 2022; i <= 2022; i++)
                        {
                            double v = ts.GetVal(new GekkoTime(EFreq.A, i, 1));
                            G.Writeln(c1 + " -- " + c2 + "  name " + nme + " " + i + " = " + v);
                        }
                    }
                }
            }
        }

        public static int FindLinkJ(List<List<DecompData>> decompDatas, int i, string linkVariable, EContribType operatorOneOf3Types)
        {
            if (linkVariable == null) return 0;  //for instance decomp of an expression            
            int parentJ = -12345;
            for (int j2 = 0; j2 < decompDatas[i].Count; j2++)
            {

                if (GetDecompDatas(decompDatas[i][j2], operatorOneOf3Types).ContainsKey(linkVariable))
                {
                    parentJ = j2;
                    break;
                }
            }

            if (parentJ == -12345)
            {
                string name = linkVariable;
                if (linkVariable.EndsWith("¤[0]")) name = linkVariable.Substring(0, linkVariable.Length - "¤[0]".Length);
                G.Writeln2("*** ERROR: Could not find variable " + name + " in linked equation #" + i + " of " + (decompDatas.Count - 1));
                throw new GekkoException();
            }

            return parentJ;
        }


        public static Series FindLinkSeries(List<List<DecompData>> decompDatas, int i, int j, string linkVariable, EContribType operatorOneOf3Types)
        {
            if (!GetDecompDatas(decompDatas[i][j], operatorOneOf3Types).ContainsKey(linkVariable))
            {
                return null;
            }
            Series linkParent = GetDecompDatas(decompDatas[i][j], operatorOneOf3Types)[linkVariable];
            return linkParent;
        }

        public static EquationHelper DecompEvalGekko(string variable)
        {
            EquationHelper found = Program.FindEquationByMeansOfVariableName(variable);
            if (found == null)
            {
                G.Writeln2("*** ERROR: DECOMP: Could not find variable '" + variable + "' as left-hand side in model");
                throw new GekkoException();
            }
            string[] ss = found.equationText.Split('=');

            string rhs = ss[1].Trim();

            string lhsText = ss[0].Trim();
            string[] ss0 = lhsText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (!G.Equal(ss0[0], "frml"))
            {
                G.Writeln2("*** ERROR: Model equation '" + variable + "': Equation does not start with 'frml'");
                throw new GekkoException();
            }

            string lhs = null;
            for (int i = 2; i < ss0.Length; i++)
            {
                lhs += ss0[i];
            }
            lhs = lhs.Trim();  //trimmed with no blanks                                                              

            if (rhs.EndsWith("$")) rhs = rhs.Substring(0, rhs.Length - 1) + ";";  //only replace last $, not other $

            rhs = rhs.Trim();
            if (rhs.EndsWith(";")) rhs = rhs.Substring(0, rhs.Length - 1);

            for (int i = 1; i < 20; i++)
            {
                rhs = rhs.Replace("(-" + i + ")", "[-" + i + "]");
                rhs = rhs.Replace("(+" + i + ")", "[+" + i + "]");
            }

            string type = "none";  //dlog, dif, diff, log
            if (lhs.StartsWith("dlog(", StringComparison.OrdinalIgnoreCase))
            {
                type = "dlog";
                rhs = found.lhs + "[-1] * exp(" + rhs + ")";
            }
            else if (lhs.StartsWith("dif(", StringComparison.OrdinalIgnoreCase))
            {
                type = "dif";
                rhs = found.lhs + "[-1] + (" + rhs + ")";
            }
            else if (lhs.StartsWith("diff(", StringComparison.OrdinalIgnoreCase))
            {
                type = "diff";
                rhs = found.lhs + "[-1] + (" + rhs + ")";
            }
            else if (lhs.StartsWith("diff(", StringComparison.OrdinalIgnoreCase))
            {
                type = "log";
                rhs = "exp(" + rhs + ")";
            }

            if (found.equationCodeJ != "" && found.equationCodeJ != "_" && found.equationCodeJ != "__")
            {
                if (found.equationCodeJadditive)
                {
                    rhs = rhs + " + " + found.Jname;
                }
                else if (found.equationCodeJmultiplicative)
                {
                    rhs = "(" + rhs + ") * (1 + " + found.Jname + ")";
                }
                else
                {
                    //should not happen
                    G.Writeln2("*** ERROR: Problem with J-factors in equation " + found.lhs);
                    throw new GekkoException();
                }
            }

            if (found.equationCodeD != "" && found.equationCodeD != "_")
            {
                rhs = "(1 - " + found.Dname + ") * (" + rhs + ") + " + found.Dname + " * " + found.Zname;
            }

            string temp2 = found.lhs + "-(" + rhs + ");";
            //rhs = found.lhs + " = " + rhs + ";";
            rhs = rhs + ";";
            //string tmp = rhs;
            string tmp = temp2;

            try
            {
                if (Globals.printAST)
                {
                    G.Writeln2("-------------- EVAL ---------------");
                    G.Writeln2("EVAL " + G.ReplaceGlueNew(tmp));
                    G.Writeln2("-----------------------------------");
                }

                Globals.expressions = null;  //maybe not necessary
                Program.obeyCommandCalledFromGUI("EVAL " + tmp, new P());  //produces Func<> Globals.expression with the expression
                found.expressions = new List<Func<GekkoSmpl, IVariable>>(Globals.expressions);  //probably needs cloning/copying as it is done here
                Globals.expressions = null;  //maybe not necessary
                

            }
            catch (Exception e)
            {

            }

            return found;
        }
    }
}
