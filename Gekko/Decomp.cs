using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra.Sparse.Linear;
using MathNet.Numerics.LinearAlgebra.Sparse;
using MathNet.Numerics.LinearAlgebra.Sparse.Tests;
using ProtoBuf;

namespace Gekko
{    

    public class DecompOperator
    {
        public bool isPercentageType = false; //for formatting
        public string operatorLower = null;
        public bool isRaw = false;
        public bool isShares = false;
        public bool isDoubleDifQuo = false;  //codes that contain 'dp'
        public bool isDoubleDifRef = false;  //codes that contain 'rdp'
        public Decomp.ELowLevel lowLevel = Decomp.ELowLevel.Unknown; //.BothQuoAndRef --> <mp> or <xmp> type
        public List<int> lagData = new List<int>() { 0, 0 };
        public List<int> lagGradient = new List<int>() { 0, 0 };
        public Decomp.EContribType type = Decomp.EContribType.Unknown;

        public DecompOperator(string x)
        {
            if (x == "n" || x == "r" || x == "rn")
            {
                new Error("Please use operator 'x" + x + "' instead of '" + x + "'.");
            }
            bool good = false;
            if (x == "x" || x == "xn")
            {
                this.isRaw = true;                
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
            }
            else if (x == "xr" || x == "xrn")
            {
                this.isRaw = true;
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
            }

            // ----------------------------------------------------
            //col 1
            // ----------------------------------------------------

            else if (x == "xd")
            {
                this.isRaw = true;
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
                this.lagData = new List<int>() { -1, 0 };
            }
            else if (x == "xp")
            {
                this.isRaw = true;
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
                this.lagData = new List<int>() { -1, 0 };
                this.isPercentageType = true;
            }
            else if (x == "xdp")
            {
                this.isRaw = true;
                this.isDoubleDifQuo = true;
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
                this.lagData = new List<int>() { -2, 0 };
                this.isPercentageType = true;
            }

            // ----------------------------------------------------
            //col 2
            // ----------------------------------------------------
                        
            else if (x == "d" || x == "sd")
            {
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
                this.lagData = new List<int>() { -1, 0 };
                this.lagGradient = new List<int>() { -1, -1 };
                this.type = Decomp.EContribType.D;
                if (x.StartsWith("s")) { isShares = true; isPercentageType = true; }
            }
            else if (x == "p" || x == "sp")
            {
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
                this.lagData = new List<int>() { -1, 0 };
                this.lagGradient = new List<int>() { -1, -1 };
                this.type = Decomp.EContribType.D;
                this.isPercentageType = true;
                if (x.StartsWith("s")) isShares = true;
            }
            else if (x == "dp" || x == "sdp")
            {
                this.isDoubleDifQuo = true;
                this.lowLevel = Decomp.ELowLevel.OnlyQuo;
                this.lagData = new List<int>() { -2, 0 };
                this.lagGradient = new List<int>() { -2, -1 };
                this.type = Decomp.EContribType.D;
                this.isPercentageType = true;
                if (x.StartsWith("s")) isShares = true;
            }

            // ----------------------------------------------------
            //col 3
            // ----------------------------------------------------
            else if (x == "xm")
            {
                this.lowLevel = Decomp.ELowLevel.Multiplier;
                this.isRaw = true;
            }
            else if (x == "xq")
            {
                this.lowLevel = Decomp.ELowLevel.Multiplier;
                this.isRaw = true;
                this.isPercentageType = true;
            }
            else if (x == "xmp")
            {
                this.isRaw = true;
                this.lowLevel = Decomp.ELowLevel.BothQuoAndRef;
                this.lagData = new List<int>() { -1, 0 };
                this.isPercentageType = true;
            }

            // ----------------------------------------------------
            //col 4
            // ----------------------------------------------------
            else if (x == "m" || x == "sm")
            {
                this.lowLevel = Decomp.ELowLevel.Multiplier;
                this.type = Decomp.EContribType.M;
                if (x.StartsWith("s")) { isShares = true; isPercentageType = true; }
            }
            else if (x == "q" || x == "sq")
            {
                this.lowLevel = Decomp.ELowLevel.Multiplier;
                this.type = Decomp.EContribType.M;
                this.isPercentageType = true;
                if (x.StartsWith("s")) isShares = true;
            }
            else if (x == "mp" || x == "smp")
            {
                this.lowLevel = Decomp.ELowLevel.BothQuoAndRef;
                this.lagData = new List<int>() { -1, 0 };
                this.lagGradient = new List<int>() { -1, -1 };
                this.type = Decomp.EContribType.M;
                this.isPercentageType = true;
                if (x.StartsWith("s")) isShares = true;
            }


            // ----------------------------------------------------
            //ref col 1
            // ----------------------------------------------------

            else if (x == "xrd")
            {                   
                this.isRaw = true;
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
                this.lagData = new List<int>() { -1, 0 };
            }
            else if (x == "xrp")
            {
                this.isRaw = true;
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
                this.lagData = new List<int>() { -1, 0 };
                this.isPercentageType = true;
            }
            else if (x == "xrdp")
            {
                this.isRaw = true;
                this.isDoubleDifRef = true;
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
                this.lagData = new List<int>() { -2, 0 };
                this.isPercentageType = true;
            }

            // ----------------------------------------------------
            //ref col 2
            // ----------------------------------------------------

            else if (x == "rd" || x == "srd")
            {
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
                this.lagData = new List<int>() { -1, 0 };
                this.lagGradient = new List<int>() { -1, -1 };
                this.type = Decomp.EContribType.RD;
                if (x.StartsWith("s")) { isShares = true; isPercentageType = true; }
            }
            else if (x == "rp" || x == "srp")
            {
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
                this.lagData = new List<int>() { -1, 0 };
                this.lagGradient = new List<int>() { -1, -1 };
                this.type = Decomp.EContribType.RD;
                this.isPercentageType = true;
                if (x.StartsWith("s")) isShares = true;
            }
            else if (x == "rdp" || x == "srdp")
            {
                this.isDoubleDifRef = true;
                this.lowLevel = Decomp.ELowLevel.OnlyRef;
                this.lagData = new List<int>() { -2, 0 };
                this.lagGradient = new List<int>() { -2, -1 };
                this.type = Decomp.EContribType.RD;
                this.isPercentageType = true;
                if (x.StartsWith("s")) isShares = true;
            }
            
            // -------------------------------

            else
            {
                new Error("Illegal operator '" + x + "'");
            }
            this.operatorLower = x;
        }
    }

    class AggContainer
    {
        public double change;
        public double changeAlternative;
        public double level;
        public double levelLag;
        public double levelLag2;
        public double levelRef;
        public double levelRefLag;
        public double levelRefLag2;
        public int n;
        public List<string> fullVariableNames;

        public AggContainer(double change, double changeAlternative, double level, double levelLag, double levelLag2, double levelRef, double levelRefLag, double levelRefLag2, int n, List<string> fullVariableNames)
        {
            this.change = change;
            this.changeAlternative = changeAlternative;
            this.level = level;
            this.levelLag = levelLag;
            this.levelLag2 = levelLag2;
            this.levelRef = levelRef;
            this.levelRefLag = levelRefLag;
            this.levelRefLag2 = levelRefLag2;
            this.n = n;
            this.fullVariableNames = fullVariableNames;
        }
    }

    public class Decomp
    {
        public enum EContribType
        {
            Unknown,
            N,   //probably not used much
            RN,  //probably not used much
            D,
            RD,
            M
        }

        public enum ELowLevel
        {
            Unknown,
            OnlyQuo,
            OnlyRef,
            Multiplier,
            BothQuoAndRef  //only for <mp> and <xmp> type
        }

        /// <summary>
        /// Tells normalize method how to sum up
        /// </summary>
        public enum ENormalizeType
        {
            Normal,     //only takes y[0]
            Lags        //will sum up lags like y[-1], y, y[+1]
        }

        /// <summary>
        /// For any eval of GAMS scalar model, we must consider loading
        /// data into the model arrays. For now, it always returns true.
        /// Later on, keep track of First/Ref databank changes...
        /// </summary>
        /// <returns></returns>
        public static bool MustLoadDataIntoModel()
        {
            return true;
        }

        /// <summary>
        /// The starting point of DECOMP, collecting decomp options etc.
        /// </summary>
        /// <param name="o"></param>
        public static void DecompStart(O.Decomp2 o)
        {
            //In general, uncontrolled sets produce a list of equations. Hard to prune these, it is a bit like the lag problem, only lazy 
            //  eval might help.
            //In an equation like y[#a] = x[#a] + 5, there will be 100 equations if #a is 1..100. For each of these, lags are tried. So
            //it is checked if x[31][2000] affects y[31][2001] --> a lag. If such a lag is detected, x[#a][-1] is added to the variables
            //that contribute.

            //See source code documentation

            Globals.lastDecompTable = null;
            G.CheckLegalPeriod(o.t1, o.t2);

            if (G.NullOrEmpty(o.opt_prtcode)) o.opt_prtcode = "xn";

            DecompOptions2 decompOptions2 = null;
            if (o.decompFind != null)
            {
                decompOptions2 = o.decompFind.decompOptions2;
            }
            else
            {
                decompOptions2 = new DecompOptions2();
                decompOptions2.code = o.label + ";";
                decompOptions2.modelType = G.GetModelType();
                decompOptions2.decompTablesFormat.showErrors = true; //
                decompOptions2.t1 = o.t1;
                decompOptions2.t2 = o.t2;
                decompOptions2.expressionOld = o.label;
                decompOptions2.expression = o.expression;
                decompOptions2.prtOptionLower = o.opt_prtcode.ToLower();
                if (G.Equal(o.opt_dyn, "yes")) decompOptions2.dyn = true;
                if (G.Equal(o.opt_missing, "zero")) decompOptions2.missingAsZero = true;
                if (G.Equal(o.opt_count, "n")) decompOptions2.count = ECountType.N;
                else if (G.Equal(o.opt_count, "names")) decompOptions2.count = ECountType.Names;
                decompOptions2.name = o.name;
                decompOptions2.isNew = true;
                o.decompFind = new DecompFind(EDecompFindNavigation.Decomp, 0, decompOptions2, null);
            }

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
                if (liv.option != null) temp.option = liv.option.ConvertToString(); //"lead"
                decompOptions2.link.Add(temp);
            }

            if (decompOptions2.type == "ASTDECOMP3" || decompOptions2.modelType == EModelType.GAMSScalar)
            {
                //Here, for scalar we need to assemble the equations like this:
                // e1[a, 2001], e1[a, 2001], e1[b, 2002], e1[b, 2002], e2[x, 2001], e2[x, 2001], e2[y, 2002], e2[y, 2002]
                // Produces 2 Link objects, each consisting of a list of 2 sub-objects.
                // These sub-objects should provide params that makes it possible to call
                //       the for instance e1[a] by GekkoTime, so it can call
                //       e1[a][2001a1], e1[a][2002a1], etc.
                // Maybe use an array with distance from t0, and .Observations(...). Faster than dict lookup.

                decompOptions2.new_select = O.Restrict(o.select[0] as List, false, false, false, true);
                decompOptions2.new_from = O.Restrict(o.from[0] as List, false, false, false, true);  //eqs may be e[a, b] etc.
                decompOptions2.new_endo = O.Restrict(o.endo[0] as List, false, false, false, true);

                if (decompOptions2.modelType == EModelType.GAMSScalar)
                {
                    if (MustLoadDataIntoModel())
                    {
                        ModelGamsScalar.FlushAAndRArrays();
                        Program.model.modelGamsScalar.FromDatabankToAScalarModel(Program.databanks.GetFirst(), false);
                        Program.model.modelGamsScalar.FromDatabankToAScalarModel(Program.databanks.GetRef(), true);
                    }
                }
                else
                {
                    int counter = -1;
                    foreach (string s in decompOptions2.new_from)
                    {
                        counter++;
                        Link link = new Link();
                        link.eqname = s;
                        if (counter == 0)
                        {
                            link.endo = new List<string>();
                            link.endo.AddRange(decompOptions2.new_endo);
                            link.varnames = new List<string>();
                            link.varnames.AddRange(decompOptions2.new_select);
                        }
                        else
                        {
                            //is this still necessary?
                            link.varnames = new List<string>();
                            link.varnames.Add("<not used>"); //strange but necessary further on
                        }
                        link.expressions = new List<Func<GekkoSmpl, IVariable>>();
                        link.expressions.Add(null); //strange but necessary further on
                        decompOptions2.link.Add(link);
                    }
                }
            }

            if (true)
            {
                Decomp.DecompGetFuncExpressionsAndRecalc(o.decompFind, null);
            }
            else
            {
                //probably so that windows can be opened without waiting for other windows closing
                CrossThreadStuff.Decomp2(o.decompFind);
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

        /// <summary>
        /// Hooks up to GAMS scalar model
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="s"></param>
        /// <param name="equationName"></param>
        /// <param name="mmi"></param>
        /// <param name="element"></param>
        /// <param name="type"></param>
        /// <param name="doubleDif"></param>
        private static void FindEquationsForEachRelevantPeriod(GekkoTime t1, GekkoTime t2, string s, string equationName, MultidimItem mmi, DecompStartHelper element, DecompOperator op)
        {
            int deduct = op.lagGradient[0];
            if(op.isRaw) deduct = op.lagData[0];
            foreach (GekkoTime time in new GekkoTimeIterator(t1.Add(deduct), t2))
            {
                int i = GekkoTime.Observations(Program.model.modelGamsScalar.t0, time) - 1;
                if (i < 0 || i > element.periods.Length - 1)
                {
                    new Error("Period " + time.ToString() + " outside GAMS scalar model period (GAMS model runs over " + Program.model.modelGamsScalar.t0.ToString() + " to " + Program.model.modelGamsScalar.t2.ToString() + ").");
                }
                if (element.periods[i] != null) new Error("Dublet equation: " + equationName + mmi.GetName() + " in " + time.ToString());
                DecompStartHelperPeriod elementPeriod = new DecompStartHelperPeriod();
                //Below: must be string like "e1[2001]" or "e1[a, 2001]", etc.
                //

                //string s2 = AddTimeToIndexes(s, time);

                string s2 = G.Chop_DimensionAddLast(s, time.ToString(), false);

                int eqNumber = -12345;
                bool b = Program.model.modelGamsScalar.dict_FromEqNameToEqNumber.TryGetValue(s2, out eqNumber);
                if (!b)
                {
                    new Error("Could not find the equation '" + s2 + "'");
                }
                //int eqNumber = Program.model.modelGamsScalar.dict_FromEqNameToEqNumber[s2];
                elementPeriod.eqNumber = eqNumber;
                elementPeriod.t = time;
                element.periods[i] = elementPeriod;
            }
        }
        
        /// <summary>
        /// For a name like "x" and a list like {"a", "b"}, time is added and returns like for instance "x[a,b,2001]".
        /// Note no blanks.
        /// </summary>
        /// <param name="name2"></param>
        /// <param name="indexes2"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private static string AddTimeToIndexes(string name2, List<string> indexes2, GekkoTime time)
        {            
            indexes2.Add(time.ToString());
            string s2 = G.Chop_GetFullName(null, name2, null, indexes2.ToArray(), false);
            return s2;
        }

        /// <summary>
        /// Main entry to the math part of decomposition. Performs a lot of the hard stuff, including
        /// matrix inversion etc. Calls DecompLowLevel() a lot, where gradients etc. are calculated.
        /// The table returned is a pivot table.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="per1"></param>
        /// <param name="per2"></param>
        /// <param name="operator1"></param>
        /// <param name="isShares"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="frame"></param>
        /// <param name="refresh"></param>
        /// <param name="decompDatas"></param>
        /// <returns></returns>
        public static Table DecompMain(GekkoSmpl smpl, GekkoTime per1, GekkoTime per2, DecompOptions2 decompOptions2, FrameLight frame, bool refresh, ref DecompDatas decompDatas)
        {   
            GekkoTime gt1, gt2;
            DecompOperator op = DecompMainInit(out gt1, out gt2, per1, per2, decompOptions2.prtOptionLower);

            decompOptions2.decompTablesFormat.isPercentageType = op.isPercentageType;
            DateTime t0 = DateTime.Now;

            //EContribType operatorOneOf3Types = DecompContribTypeHelper(op);
            EContribType operatorOneOf3Types = op.type;

            int perLag = -2;
            string lhsString = "Expression value";
            int parentI = 0;

            //MAIN varnames are: decompOptions2.link[parentI].varnames

            // decompDatas THE TEXT BELOW IS OBSOLETE
            // Example: DECOMP x[#a] from e1, e2 endo x[#a], y[#a];
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
            //G.Writeln2(">>>Before low level " + DateTime.Now.ToLongTimeString());

            if (decompOptions2.modelType == EModelType.GAMSScalar)
            {
                PrepareEquations(per1, per2, op, decompOptions2);
            }

            if (true)  //signals a recalc of data, not a reuse (like pch or share showing)
            {
                if (decompDatas.storage == null) decompDatas.storage = new List<List<DecompData>>();
                decompDatas.MAIN_data = null;

                //MAYBE DO THIS BY LOOKING INSIDE DECOMPDATAS...
                //when putting in raw data (cellsQuo, cellsRef), maybe put them in for the full period (fast anyway)                

                if (decompDatas.storage == null || decompDatas.storage.Count == 0) InitDecompDatas(decompOptions2, decompDatas);

                List<string> expressionTexts = new List<string>();
                int ii = -1;
                foreach (Link link in decompOptions2.link)  //including the "mother" non-linked equation
                {
                    ii++;
                    string residualName = Program.GetDecompResidualName(ii);

                    int jj = -1;
                    if (decompOptions2.modelType == EModelType.GAMSScalar)
                    {
                        foreach (DecompStartHelper dsh in link.GAMS_dsh)  //unrolling: for each uncontrolled #i in x[#i]
                        {
                            jj++;  //will be = 0
                            DecompData dd = Decomp.DecompLowLevelScalar(gt1, gt2, jj, dsh, op, residualName, ref funcCounter);
                            DecompMainMergeOrAdd(decompDatas, dd, ii, jj);
                        }
                    }
                    else
                    {
                        foreach (Func<GekkoSmpl, IVariable> expression in link.expressions)  //unrolling: for each uncontrolled #i in x[#i]
                        {
                            jj++;
                            DecompData dd = Decomp.DecompLowLevel(per1, per2, expression, DecompBanks_OLDREMOVESOON(op), residualName, ref funcCounter);
                            DecompMainMergeOrAdd(decompDatas, dd, ii, jj);
                        }
                    }
                }

                if (operatorOneOf3Types == EContribType.D) decompDatas.hasD = true;
                else if (operatorOneOf3Types == EContribType.RD) decompDatas.hasRD = true;
                else if (operatorOneOf3Types == EContribType.M) decompDatas.hasM = true;

                //G.Writeln2(">>>After low level " + DateTime.Now.ToLongTimeString());

                if (decompOptions2.link[parentI].varnames == null)
                {
                    decompOptions2.link[parentI].varnames = new List<string>() { Globals.decompResidualName };
                }

                if (false)
                {
                    DecompPrintDatas(decompDatas.storage, operatorOneOf3Types);
                }

                bool[] used = new bool[decompDatas.storage.Count];
                used[0] = true;  //primary equation

                GekkoDictionary<string, bool> ignore = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

                //linking
                //linking
                //linking


                //------------------------
                //Example: e1: y = c + i + g  --> y - (c + i + g)
                //         e2: c = 0.8 * y    --> c - 0.8 * y
                //------------------------

                if (decompOptions2.type == "ASTDECOMP3")
                {
                    if (decompOptions2.modelType == EModelType.GAMSScalar)
                    {
                        if (decompOptions2.dyn)
                        {
                            //decomp over time, resolving lags/leads                            

                            if (op.lowLevel == ELowLevel.BothQuoAndRef)  //<mp>
                            {
                                DecompMainHelperInvertScalar(per1, per2, decompOptions2, decompDatas, EContribType.D, parentI, true, op);
                                DecompMainHelperInvertScalar(per1, per2, decompOptions2, decompDatas, EContribType.RD, parentI, false, op);  //Note: refreshObjects = false!
                            }
                            else
                            {
                                DecompMainHelperInvertScalar(per1, per2, decompOptions2, decompDatas, operatorOneOf3Types, parentI, true, op);
                            }
                        }
                        else
                        {
                            //decomp period by period, showing lags/leads.

                            if (op.lowLevel == ELowLevel.BothQuoAndRef)  //<mp>
                            {
                                bool refreshObjects = true;
                                foreach (GekkoTime gt in new GekkoTimeIterator(per1, per2))
                                {
                                    DecompMainHelperInvertScalar(gt, gt, decompOptions2, decompDatas, EContribType.D, parentI, refreshObjects, op);
                                    refreshObjects = false;
                                }
                                foreach (GekkoTime gt in new GekkoTimeIterator(per1, per2))
                                {
                                    DecompMainHelperInvertScalar(gt, gt, decompOptions2, decompDatas, EContribType.RD, parentI, refreshObjects, op);
                                }
                            }
                            else
                            {
                                int deduct = 0;
                                //why deduct not enough??
                                if (op.isDoubleDifQuo || op.isDoubleDifRef) deduct = -1;  //all the data are ready, so we can calc 1 period earlier, so that a 1-period decomp actually shows something for <dp> or <rdp>
                                bool refreshObjects = true;
                                foreach (GekkoTime gt in new GekkoTimeIterator(per1.Add(deduct), per2))
                                {
                                    DecompMainHelperInvertScalar(gt, gt, decompOptions2, decompDatas, operatorOneOf3Types, parentI, refreshObjects, op);
                                    refreshObjects = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        DecompMainHelperInvert(per1, per2, decompOptions2, decompDatas, operatorOneOf3Types, parentI);
                    }
                }
                else
                {
                    OBSOLETE_decomp2_stuff(per1, per2, decompOptions2, decompDatas, operatorOneOf3Types, parentI, used);
                }

                //At this point, all linked equations i = 1, 2, ... have been merged into
                //the MAIN equation i = 0.    

                if (false)
                {
                    int i = -1;
                    foreach (List<DecompData> x in decompDatas.storage)
                    {
                        i++;
                        int j = -1;
                        foreach (DecompData y in x)
                        {
                            j++;
                            new Writeln("COMBINATION =====> " + i + " " + j);
                            PrintDecompData(y);
                        }
                    }
                    if (true && decompDatas.MAIN_data != null && decompDatas.MAIN_data[0] != null)
                    {
                        new Writeln("...");
                        new Writeln("...");
                        new Writeln("MAIN MAIN MAIN MAIN MAIN MAIN MAIN MAIN ");
                        PrintDecompData(decompDatas.MAIN_data[0]);
                    }
                }


            }

            //decompDatas[parentI] is the main equation, the other ones are in-substituted. This decompDatas[parentI] has a member
            //for each uncontrolled set like #a. The main variables (MAIN_varnames) are normalized to 1.
            //decompData.cellsContribD contains keys like "Work:y[19]¤[+1]" with values as timeseries.
            //This example is split into y, #a, 1, t, value --> so we get a dataframe row like this:
            //eq=0, variable=y, #a = 19, lag=1, t=2010, 1.2345
            //We clone the data first, before calling DecompPivotToTable(), because they may be normalized etc. 

            //The decompDataMAINClone is a list, but ought to be 1 object (fix some time)
            //We are cloning this, because normalization may take place when doing the table
            List<DecompData> decompDataMAINClone = new List<DecompData>();
            foreach (DecompData dd in decompDatas.MAIN_data) decompDataMAINClone.Add(dd.DeepClone());

            Table table = Decomp.DecompPivotToTable(per1, per2, decompDataMAINClone, decompDatas, decompOptions2.decompTablesFormat, op, smpl, lhsString, decompOptions2.link[parentI].expressionText, decompOptions2, frame, operatorOneOf3Types);

            if (false)
            {
                DecompPrintDatas(decompDatas.storage, operatorOneOf3Types);
                throw new GekkoException();
            }

            G.Writeln2("DECOMP took " + G.SecondsFormat((DateTime.Now - t0).TotalMilliseconds) + ", function evals = " + funcCounter);

            return table;
        }

        public static DecompOperator DecompMainInit(out GekkoTime gt1, out GekkoTime gt2, GekkoTime per1, GekkoTime per2, string prtOption)
        {
            DecompOperator op = new DecompOperator(prtOption);
            gt1 = per1;
            gt2 = per2;
            if (op.isRaw)
            {
                gt1 = per1.Add(op.lagData[0]);
                gt2 = per2.Add(op.lagData[1]);
            }
            else
            {
                gt1 = per1.Add(op.lagGradient[0]);
                gt2 = per2.Add(op.lagGradient[1]);
            }
            return op;
        }

        private static void PrintDecompData(DecompData y)
        {
            new Writeln("cellsQuo --------------------------------");
            PrintDecompDict(y.cellsQuo);

            new Writeln("cellsRef --------------------------------");
            PrintDecompDict(y.cellsRef);

            new Writeln("cellsGradQuo --------------------------------");
            PrintDecompDict(y.cellsGradQuo);

            new Writeln("cellsGradRef --------------------------------");
            PrintDecompDict(y.cellsGradRef);

            new Writeln("cellsContribD --------------------------------");
            PrintDecompDict(y.cellsContribD);

            new Writeln("cellsContribDRef --------------------------------");
            PrintDecompDict(y.cellsContribDRef);

            new Writeln("cellsContribM --------------------------------");
            PrintDecompDict(y.cellsContribM);
        }

        private static void OBSOLETE_decomp2_stuff(GekkoTime per1, GekkoTime per2, DecompOptions2 decompOptions2, DecompDatas decompDatas, EContribType operatorOneOf3Types, int parentI, bool[] used)
        {
            //DECOMP2 (ASTDECOMP2), so non-simultaneous decomposition. Will be obsolete.

            //Takes the link equations, skipping the first one (which is the "normal" equation)
            //Example: decomp y in e1 link c in e2
            //the link equation is e2

            List<string> linkVariables = new List<string>();

            //We clone it so that the original decomp of first/main equation is not contaminated
            decompDatas.MAIN_data = new List<DecompData>();
            foreach (DecompData dd in decompDatas.storage[parentI]) decompDatas.MAIN_data.Add(dd.DeepClone());

            for (int i = 1; i < decompOptions2.link.Count; i++)  //skips the MAIN equation
            {
                // ---------------------------------------------------------------
                // ---------------------------------------------------------------
                // Decomp over time (see example here: #98750984325)
                // ---------------------------------------------------------------
                // ---------------------------------------------------------------

                bool isLead = G.Equal(decompOptions2.link[i].option, "lead");
                int add = 0;
                if (isLead)
                {
                    add = 1;

                    if (false)
                    {
                        List<List<DecompData>> delete = new List<List<DecompData>>();
                        delete.Add(decompDatas.MAIN_data);
                        DecompPrintDatas(delete, operatorOneOf3Types);
                        throw new GekkoException();
                    }
                }

                //For each link variable (c) in the link equation (e2)
                for (int n = 0; n < decompOptions2.link[i].varnames.Count; n++)
                {
                    //adjust the table according to link variable, so it fits with the destination table
                    string linkVariable = Program.databanks.GetFirst().name + ":" + ConvertToTurtleName(decompOptions2.link[i].varnames[n], 0);
                    string linkVariableHelper = linkVariable;
                    if (isLead) linkVariableHelper = Program.databanks.GetFirst().name + ":" + ConvertToTurtleName(decompOptions2.link[i].varnames[n], 1);
                    linkVariables.Add(linkVariableHelper);  //used to remove 0's later on

                    //TODO TODO
                    //TODO TODO if there > 1 hit here, error or warning should be issued
                    //TODO TODO
                    //looks in the uncontrolled eqs in link # i to find a match
                    int j = FindLinkJ(decompDatas.storage, i, linkVariable, operatorOneOf3Types);  //Example: find row with c in table corresponding to e2

                    for (int parentJ = 0; parentJ < decompDatas.MAIN_data.Count; parentJ++)
                    {
                        //The series below is the lhs series of the whole decomposition. If the rhs or a link contains the lhs variable,
                        //it will be altered, therefore the clone. For instance, in y = c + g and c = 0.8*y, a naive decomp for data where
                        //y changes with 1 each period will only show 0.2. This is corrected below, corresponding to y = 0.8*y + g --> 0.2 y = g --> y = 5*g.

                        //in y = c + i + g // c = 0.8 y
                        //DECOMP y in eq1 link c in eq2.
                        //linkparent would be c from eq1, and linkchild would be c from eq2.
                        //linkparent is always from first equation

                        Series linkParent = FindLinkSeries(decompDatas.MAIN_data, parentJ, linkVariableHelper, operatorOneOf3Types); //Example: decomposed c from e1                       
                                                                                                                                     //maybe check that all link equations are used, and report if they are not.
                        if (linkParent == null)
                        {
                            continue;
                        }
                        else
                        {
                            used[i] = true; //this link equation is somehow used, for some of its variables and one or more of the primary variables that are going to be decomposed (= super)
                        }
                        Series linkChild = FindLinkSeries(decompDatas.storage, i, j, linkVariable, operatorOneOf3Types); //Example: decomposed c from e2

                        List<double> factors = new List<double>();
                        foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                        {
                            //Example: the factor is -(-1/1) = 1, so that adding the two tables would eliminate c in e1 equation.
                            // y - (c + i + g) + 1*(c - 0.8 * y) = y + i + g - 0.8 * y =  0.2 * y + i + g
                            // when showing this for y, the result must be multiplied by 5.
                            double dLinkParent = double.NaN;

                            dLinkParent = linkParent.GetDataSimple(t);
                            double dLinkChild = linkChild.GetDataSimple(t.Add(add));

                            double factor = -dLinkParent / dLinkChild;  //recalculated for each kvp, but never mind, should not matter much
                            factors.Add(factor);
                        }

                        //for each period, find the variable value in the original equation, and compute
                        //  a correction factor for the sub-equation.     

                        foreach (KeyValuePair<string, Series> kvp in GetDecompDatas(decompDatas.storage[i][j], operatorOneOf3Types).storage)
                        {
                            string childVariableName = kvp.Key;

                            if (isLead)
                            {
                                int lag; string name;
                                ConvertFromTurtleName(childVariableName, true, out name, out lag);
                                childVariableName = ConvertToTurtleName(name, lag + 1);
                            }

                            Series varParent = GetDecompDatas(decompDatas.MAIN_data[parentJ], operatorOneOf3Types)[childVariableName];  //will be created
                            Series varChild = kvp.Value;

                            int counter2 = -1;

                            foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                            {
                                counter2++;
                                double dVarParent = varParent.GetDataSimple(t);
                                if (G.isNumericalError(dVarParent)) dVarParent = 0d;  //it usually does not exist beforehand
                                double dVarChild = varChild.GetDataSimple(t.Add(add));
                                double x = dVarParent + factors[counter2] * dVarChild;
                                GetDecompDatas(decompDatas.MAIN_data[parentJ], operatorOneOf3Types)[childVariableName].SetData(t, x);
                            }
                        }
                    }
                }
            }

            if (false)
            {
                List<List<DecompData>> delete = new List<List<DecompData>>();
                delete.Add(decompDatas.MAIN_data);
                DecompPrintDatas(delete, operatorOneOf3Types);
                throw new GekkoException();
            }

            //remove linked variables from result (are 0)
            List<string> problem = new List<string>();
            foreach (string linkVariable in linkVariables)
            {
                for (int parentJ = 0; parentJ < decompDatas.MAIN_data.Count; parentJ++)
                {
                    DecompDict dd = GetDecompDatas(decompDatas.MAIN_data[parentJ], operatorOneOf3Types);
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
                new Warning("DECOMP: Variable " + linkVariable + " is not eliminated");
            }

            DecompRemoveResidualsIfZero(per1, per2, decompDatas, operatorOneOf3Types);

            //TODO: make sure that every lhs variable is found 1 and only 1 time
            //      in the decompDatas, and report error if not.
            //      To do this, FindLinkJ() and FindLinkSeries() need adjustments

            //correct if the lhs variable is not stated with an implicit 1, like
            //y = c + g, but instead 2*y = 2*c +2*g, or y = c + g, c = 0.8 * y ---> 0.2 * y = g,
            //the last one must be multiplied with 5.                

            for (int i = 0; i < decompDatas.storage.Count; i++)
            {
                if (used[i] != true)
                {
                    new Warning("Did not use link-equation #" + i + " of " + (decompDatas.storage.Count - 1) + " (is it superfluous?)");
                }
            }
        }

        private static void InitDecompDatas(DecompOptions2 decompOptions2, DecompDatas decompDatas)
        {
            decompDatas.storage = new List<List<DecompData>>();
            int ii = -1;
            foreach (Link link in decompOptions2.link)  //including the "mother" non-linked equation
            {
                ii++;
                decompDatas.storage.Add(new List<DecompData>());
                if (decompOptions2.modelType == EModelType.GAMSScalar)
                {
                    foreach (DecompStartHelper dsh in link.GAMS_dsh)  //unrolling: for each uncontrolled #i in x[#i]
                    {
                        DecompData d = new DecompData();
                        DecompInitDict(d);
                        decompDatas.storage[ii].Add(d);                        
                    }
                }
                else
                {
                    foreach (Func<GekkoSmpl, IVariable> expression in link.expressions)  //unrolling: for each uncontrolled #i in x[#i]
                    {
                        DecompData d = new DecompData();
                        DecompInitDict(d);
                        decompDatas.storage[ii].Add(d);
                    }
                }
            }
        }

        /// <summary>
        /// Gathers info that makes it easier to use the equations for decomp later on.
        /// Only does it for the used equations, not all equations.
        /// Uses lists from .new_from, .new_endo and .new_select.
        /// </summary>
        /// <param name="per1"></param>
        /// <param name="per2"></param>
        /// <param name="operator1"></param>
        /// <param name="decompOptions2"></param>
        public static void PrepareEquations(GekkoTime per1, GekkoTime per2, DecompOperator operator1, DecompOptions2 decompOptions2)
        {
            decompOptions2.link = new List<Link>();
            GekkoDictionary<string, Dictionary<MultidimItem, DecompStartHelper>> equations = new GekkoDictionary<string, Dictionary<MultidimItem, DecompStartHelper>>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in decompOptions2.new_from)
            {
                //For each equation stated
                string equationName = null;
                string resultingFullName = null;
                List<string> indexes = null;
                //Actually there is no time extracted below: the s string hos no time element
                GekkoTime trash = GekkoTime.tNull;
                GamsModel.ExtractTimeDimension(s, false, ref equationName, ref trash, ref resultingFullName, out indexes);

                Dictionary<MultidimItem, DecompStartHelper> elements = null;
                equations.TryGetValue(equationName, out elements);
                if (elements == null)
                {
                    elements = new Dictionary<MultidimItem, DecompStartHelper>();
                    equations.Add(equationName, elements);
                }

                MultidimItem mmi = new MultidimItem(indexes.ToArray());
                DecompStartHelper element = null;
                elements.TryGetValue(mmi, out element);
                if (element == null)
                {
                    element = new DecompStartHelper();
                    element.name = equationName;
                    element.indexes = mmi;
                    element.fullName = element.name + element.indexes.GetName();
                    int periods = GekkoTime.Observations(Program.model.modelGamsScalar.t0, Program.model.modelGamsScalar.t2);
                    element.periods = new DecompStartHelperPeriod[periods];
                    elements.Add(mmi, element);
                }
                
                FindEquationsForEachRelevantPeriod(per1, per2, s, equationName, mmi, element, operator1);
            }

            int counter = -1;
            foreach (KeyValuePair<string, Dictionary<MultidimItem, DecompStartHelper>> kvp in equations)
            {
                //for each equation name
                counter++;
                Link link = new Link();
                link.GAMS_dsh = new List<DecompStartHelper>();
                foreach (KeyValuePair<MultidimItem, DecompStartHelper> kvp2 in kvp.Value)
                {
                    //for each index combination
                    link.GAMS_dsh.Add(kvp2.Value);
                    link.GAMS_eqNumber = counter;
                }

                //    O.Decomp2 o0 = new O.Decomp2();
                //    o0.type = @"ASTDECOMP3";
                //    o0.label = o.rv;
                //    o0.t1 = o.t1;
                //    o0.t2 = o.t2;
                //    o0.opt_prtcode = o.opt_prtcode;

                //    o0.decompItems = new List<DecompItems>();                    

                //    o0.select.Add(O.FlattenIVariablesSeq(false, new
                //     List(new List<IVariable> { new ScalarString(var) })));

                //    o0.from.Add(O.FlattenIVariablesSeq(false,
                //     new List(new List<IVariable> { new ScalarString(o.rv) })));

                //    o0.endo.Add(O.FlattenIVariablesSeq(false, new List(new
                //     List<IVariable> { new ScalarString(var) })));

                if (counter == 0)
                {
                    if (decompOptions2.new_endo != null)
                    {
                        link.endo = new List<string>();
                        link.endo.AddRange(decompOptions2.new_endo);
                    }
                    if (decompOptions2.new_select != null)
                    {
                        link.varnames = new List<string>();
                        link.varnames.AddRange(decompOptions2.new_select);
                    }
                }
                else
                {
                    //is this still necessary?
                    link.varnames = new List<string>();
                    link.varnames.Add("<not used>"); //strange but necessary further on
                }

                decompOptions2.link.Add(link);
            }
        }        

        private static void DecompMainHelperInvert(GekkoTime per1, GekkoTime per2, DecompOptions2 decompOptions2, DecompDatas decompDatas, EContribType operatorOneOf3Types, int parentI)
        {
            GekkoDictionary<string, int> endo = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in decompOptions2.link[0].endo)
            {
                string s2 = Program.databanks.GetFirst().name + ":" + ConvertToTurtleName(s, 0);
                if (!endo.ContainsKey(s2)) endo.Add(s2, endo.Count); //why if here?
            }

            DecompCheckNumberOfEqsAndEndo(decompDatas, endo);

            GekkoDictionary<string, int> exo = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < decompDatas.storage.Count; i++) //for each linked eq, including the first one
            {
                for (int j = 0; j < decompDatas.storage[i].Count; j++) //for each uncontrolled set in eq
                {
                    foreach (KeyValuePair<string, Series> kvp in GetDecompDatas(decompDatas.storage[i][j], operatorOneOf3Types).storage)
                    {
                        if (exo.ContainsKey(kvp.Key) || endo.ContainsKey(kvp.Key))
                        {
                        }
                        else
                        {
                            exo.Add(kvp.Key, exo.Count);
                        }
                    }
                }
            }

            int n = endo.Count + exo.Count;

            //now we have ENDO = decompOptions2.link[parentI].varnames, and EXO = exo

            //consider this: 
            //1 x1 + 2 x2 + 3 x3 + 4 x4 + 5 x5 = 0 
            //2 x1 + 3 x2 + 4 x3 + 5 x4 + 6 x5 = 0

            //now if x2, x4, x5 are exo, we skip these in Jacobi, getting:
            //
            // [1 3] [x1]  +  [2 4 5] [x2]   =   0
            // [2 4] [x3]     [3 5 6] [x4]     
            //                        [x5]
            //
            // [x1]  =  - [. .] [2 4 5] [x2]  
            // [x3]  =    [. .] [3 5 6] [x4]   
            //                          [x5]

            if (false)
            {
                DecompPrintDatas(decompDatas.storage, operatorOneOf3Types);
            }
            decompDatas.MAIN_data = new List<DecompData>();  //this is where the results end up

            foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
            {

                double[,] mEndo = new double[endo.Count, endo.Count];
                double[,] mExo = new double[endo.Count, exo.Count];

                int row = -1;
                for (int i = 0; i < decompDatas.storage.Count; i++) //for each linked eq, including the first one
                {
                    //In some cases, there is no interaction between the sets, for instance in
                    //an equation like y[#a] = c[#a] + g[#a]. In that case, we could solve for each age separately,
                    //but we cannot rule out eqs like y[#a] = (c[#a] + c[#a+1])/2 + g[#a]. This is still recursive,
                    //if we start to solve for the largest #a, but it illustrates the problem. Perhaps a sparse solver
                    //would not care anyway. Note that c[#a+1][+1] would be more common, and if this is treated as an
                    //exogenous, there is no age lead problem. Note also that combinations of #a-1, #a and #a+1 become
                    //simultaneous (like for time, t).
                    for (int j = 0; j < decompDatas.storage[i].Count; j++) //for each uncontrolled set in eq
                    {
                        row++;
                        foreach (KeyValuePair<string, Series> kvp in GetDecompDatas(decompDatas.storage[i][j], operatorOneOf3Types).storage)
                        {
                            double d = kvp.Value.GetDataSimple(t);
                            if (endo.ContainsKey(kvp.Key))
                            {
                                int col = endo[kvp.Key];
                                if (!(row < mEndo.GetLength(0) && col < mEndo.GetLength(1)))
                                {
                                    new Error("DECOMP matrix invert problem");
                                    //throw new GekkoException();
                                }
                                mEndo[row, col] = d;
                            }
                            else if (exo.ContainsKey(kvp.Key))
                            {
                                int col = exo[kvp.Key];
                                if (!(row < mExo.GetLength(0) && col < mExo.GetLength(1)))
                                {
                                    new Error("DECOMP matrix invert problem");
                                    //throw new GekkoException();
                                }
                                mExo[row, col] = d;
                            }
                            else
                            {
                                throw new GekkoException();
                            }
                        }
                    }
                }

                //ENDO  0 -- 0  demand[18]¤[0] 2021 = 8.88890000004245
                //ENDO  0 -- 0  supply[18]¤[0] 2021 = -8.88890000004245
                //ENDO  0 -- 1  demand[19]¤[0] 2021 = 3.33330000001592
                //ENDO  0 -- 1  supply[19]¤[0] 2021 = -3.33330000001592

                //ENDO  1 -- 0  c[18]¤[0]      2021 = -6.8889000000329
                //ENDO  1 -- 0  demand[18]¤[0] 2021 = 8.88890000004245
                //      1 -- 0  g[18]¤[0]      2021 = -2.00000000000955
                //ENDO  1 -- 1  c[19]¤[0]      2021 = -1.33330000000637
                //ENDO  1 -- 1  demand[19]¤[0] 2021 = 3.33330000001592
                //      1 -- 1  g[19]¤[0]      2021 = -2.00000000000955

                //ENDO  2 -- 0  supply[18]¤[0] 2021 = 8.88890000004245
                //ENDO  2 -- 0  y[18]¤[0]      2021 = -8.88890000004245
                //ENDO  2 -- 1  supply[19]¤[0] 2021 = 3.33330000001592
                //ENDO  2 -- 1  y[19]¤[0]      2021 = -3.33330000001592

                //ENDO  3 -- 0  c[18]¤[0]      2021 = 6.8889000000329
                //ENDO  3 -- 0  y[18]¤[0]      2021 = -3.55556000011804
                //      3 -- 0  y[19]¤[+1]     2021 = -3.33336000011065
                //ENDO  3 -- 1  c[19]¤[0]      2021 = 1.33330000000637
                //ENDO  3 -- 1  y[19]¤[0]      2021 = -1.33331999994953
                //      3 -- 1  y[20]¤[+1]     2021 = 0

                //      y18    y19    dem18    dem19   sup18   sup19   c18    c19
                // ---------------------------------------------------------------------
                //  1                 8.88             -8.88
                //  2                           3.33            -3.33
                //  3                 8.88                             -6.88                   -2 (g18)
                //  4                           3.33                          -1.33            -2 (g19)
                //  5 -8.88                              8.88
                //  6         -3.33                              3.33
                //  7 -3.55                                             6.88                   -3.33 (y19[+1])
                //  8         -1.33                                            1.33

                double[,] inverse = null;

                try
                {
                    double[,] temp = (double[,])mEndo.Clone();
                    inverse = Program.InvertMatrix(temp);
                }
                catch (Exception e)
                {
                    bool nan = false;
                    foreach (double d in mEndo)
                    {
                        if (G.isNumericalError(d))
                        {
                            nan = true;
                            break;
                        }
                    }
                    if (!nan)
                    {
                        new Error("Matrix inversion for DECOMP failed for period " + t.ToString(), false);
                        throw;
                    }
                    else
                    {
                        //We allow this, may just be some missing data
                        inverse = G.CreateArrayDouble(mEndo.GetLength(0), mEndo.GetLength(1), double.NaN);
                    }
                }

                double[,] effect = Program.MultiplyMatrices(inverse, mExo);

                //the effect matrix is #endo x #exo

                int varnamesCounter = -1;
                foreach (string s in decompOptions2.link[parentI].varnames)
                {
                    //these are the ones being reported. Is a subset of endo.

                    varnamesCounter++;

                    if (t.EqualsGekkoTime(per1))
                    {
                        DecompData dd = new DecompData();
                        DecompInitDict(dd);
                        decompDatas.MAIN_data.Add(dd);
                    }

                    string s3 = Program.databanks.GetFirst().name + ":" + ConvertToTurtleName(s, 0);

                    Series ts = GetDecompDatas(decompDatas.MAIN_data[varnamesCounter], operatorOneOf3Types)[s3];
                    ts.SetData(t, 1d);

                    int i = endo[s3];  //row
                    for (int j = 0; j < effect.GetLength(1); j++)
                    {
                        //this != 0 originates from the Gekko non-scalar decomp, and only makes sense when excact precedents are not known
                        //see also #sf94lkjsdjæ
                        if (decompOptions2.modelType == EModelType.GAMSScalar || effect[i, j] != 0d)
                        {
                            foreach (KeyValuePair<string, int> kvp in exo)
                            {
                                if (kvp.Value == j)
                                {
                                    Series ts2 = GetDecompDatas(decompDatas.MAIN_data[varnamesCounter], operatorOneOf3Types)[kvp.Key];
                                    ts2.SetData(t, effect[i, j]);
                                }
                            }
                        }
                    }
                }
            }   //foreach t

            DecompRemoveResidualsIfZero(per1, per2, decompDatas, operatorOneOf3Types);
        }

        /// <summary>
        /// Inversion of contributions, for GAMS scalar model
        /// </summary>
        /// <param name="per1"></param>
        /// <param name="per2"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="decompDatas"></param>
        /// <param name="operatorOneOf3Types"></param>
        /// <param name="parentI"></param>
        private static void DecompMainHelperInvertScalar(GekkoTime per1, GekkoTime per2, DecompOptions2 decompOptions2, DecompDatas decompDatas, EContribType operatorOneOf3Types, int parentI, bool refreshObjects, DecompOperator op)
        {
            GekkoDictionary<string, int> endo = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            GekkoDictionary<string, int> exo = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            Dictionary<int, string> endoReverse = new Dictionary<int, string>();  //just inverted
            Dictionary<int, string> exoReverse = new Dictionary<int, string>();  //just inverted

            foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
            {
                foreach (string s in decompOptions2.link[0].endo)
                {
                    //Transforms from for instance Work:x¤[+1] into Work:x¤[2002].
                    string x = Program.databanks.GetFirst().name + ":" + ConvertToTurtleName(s, 0, t);
                    if (!endo.ContainsKey(x))
                    {
                        int c = endo.Count;
                        endo.Add(x, c);
                        endoReverse.Add(c, x);
                    }
                }
            }

            //What about residuals here?????
            //What about residuals here?????  they are not part of precedents
            //What about residuals here?????

            double[,] mEndo = null;
            double[,] mExo = null;
            List<string> eqNames = new List<string>();

            //The loop here actually runs 2 times (over k). First time it just gathers elements for exo and exoReverse,
            //because the size of exo is used the second time.
            //Maybe a bit inefficient?

            int kMax = 2;
            if (op.isRaw) kMax = 1;

            for (int k = 0; k < kMax; k++)  //k=0 just counts endo/exo sizes, so the arrays can be defined
            {
                if (k == 0)
                {
                    //do nothing                    
                }
                else
                {
                    if (endo.Count != eqNames.Count)
                    {
                        using (Error txt = new Error())
                        {
                            txt.MainAdd("The numbers of total equations (" + eqNames.Count + ") and the number of endogenous variables (" + endo.Count + ") do not match");
                            List<string> temp2 = eqNames;
                            temp2.Sort(G.CompareNaturalIgnoreCase);
                            txt.MoreAdd("There are the following " + eqNames.Count + " equations given:");
                            txt.MoreNewLineTight();
                            txt.MoreAdd(Stringlist.GetListWithCommas(temp2));
                            txt.MoreNewLine();
                            List<string>temp1 = endo.Keys.ToList();
                            for (int i = 0; i < temp1.Count; i++) { temp1[i] = temp1[i].Replace("¤", ""); }
                            temp1.Sort(G.CompareNaturalIgnoreCase);
                            txt.MoreAdd("There are the following " + endo.Count + " endo variables given:");
                            txt.MoreNewLineTight();
                            txt.MoreAdd(Stringlist.GetListWithCommas(temp1));
                        }
                    }

                    mEndo = new double[endo.Count, endo.Count];
                    mExo = new double[endo.Count, exo.Count];
                }
                int row = -1;
                int ii = -1;
                foreach (Link link in decompOptions2.link)  //including the "mother" non-linked equation
                {
                    ii++;
                    int jj = -1;
                    foreach (DecompStartHelper dsh in link.GAMS_dsh)  //unrolling: for each uncontrolled #i in x[#i]
                    {
                        jj++;
                        DecompDict dd = null;
                        if (!op.isRaw) dd = GetDecompDatas(decompDatas.storage[ii][jj], operatorOneOf3Types);

                        foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                        {
                            row++;

                            //see also #as7f3læaf9
                            string eqName = AddTimeToIndexes(dsh.name, new List<string>(dsh.indexes.storage), t);
                            if (k == 0) eqNames.Add(eqName);
                            int eqNumber = Program.model.modelGamsScalar.dict_FromEqNameToEqNumber[eqName];

                            //foreach precedent variable
                            foreach (PeriodAndVariable dp in Program.model.modelGamsScalar.precedents[eqNumber].vars)
                            {
                                string varName = Program.model.modelGamsScalar.GetVarNameA(dp.variable);
                                int date = dp.date;
                                string x1 = Program.databanks.GetFirst().name + ":" + ConvertToTurtleName(varName, date, Program.model.modelGamsScalar.t0);
                                string x2 = Program.databanks.GetFirst().name + ":" + ConvertToTurtleName(varName, date - GekkoTime.Observations(Program.model.modelGamsScalar.t0, t) + 1);

                                if (k == 0)
                                {
                                    // -----------
                                    // First time
                                    // -----------

                                    if (exo.ContainsKey(x1) || endo.ContainsKey(x1))
                                    {
                                        //endo                                
                                    }
                                    else
                                    {
                                        //exo
                                        int c = exo.Count;
                                        exo.Add(x1, c);
                                        exoReverse.Add(c, x1);
                                    }
                                }
                                else
                                {
                                    // ------------
                                    // Second time
                                    // ------------

                                    //k == 1
                                    if (endo.ContainsKey(x1))
                                    {
                                        int col = endo[x1];
                                        if (!(row < mEndo.GetLength(0) && col < mEndo.GetLength(1)))
                                        {
                                            new Error("DECOMP matrix invert problem");
                                        }
                                        Series ts = dd.storage[x2];
                                        double d = ts.GetDataSimple(t);
                                        mEndo[row, col] = d;
                                    }
                                    else if (exo.ContainsKey(x1))
                                    {
                                        int col = exo[x1];
                                        if (!(row < mExo.GetLength(0) && col < mExo.GetLength(1)))
                                        {
                                            new Error("DECOMP matrix invert problem");
                                        }
                                        Series ts = dd.storage[x2];
                                        double d = ts.GetDataSimple(t);
                                        mExo[row, col] = d;
                                    }
                                    else
                                    {
                                        new Error("Decomp problem");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //TODO: check that number of endo and number of eqs match
            //TODO: check that number of endo and number of eqs match
            //TODO: check that number of endo and number of eqs match

            int n = endo.Count + exo.Count;

            if (refreshObjects)
            {
                decompDatas.MAIN_data = new List<DecompData>();  //this is where the results end up            
                DecompData dd2 = new DecompData();
                DecompInitDict(dd2);
                decompDatas.MAIN_data.Add(dd2);
            }

            double[,] inverse = null;
            double[,] effect = null;

            if (!op.isRaw)
            {

                try
                {
                    double[,] temp = (double[,])mEndo.Clone();
                    inverse = Program.InvertMatrix(temp);
                }
                catch (Exception e)
                {
                    bool nan = false;
                    foreach (double d in mEndo)
                    {
                        if (G.isNumericalError(d))
                        {
                            nan = true;
                            break;
                        }
                    }
                    if (!nan)
                    {
                        new Error("Matrix inversion for DECOMP failed for period " + per1.ToString() + "-" + per2.ToString(), false);
                        throw;
                    }
                    else
                    {
                        //We allow this, may just be some missing data
                        inverse = G.CreateArrayDouble(mEndo.GetLength(0), mEndo.GetLength(1), double.NaN);
                    }
                }

                effect = Program.MultiplyMatrices(inverse, mExo);  //endo.Count x exo.Count
                //the effect matrix is #endo x #exo    
            }

            for (int row = 0; row < endo.Count; row++)
            {
                //
                // Not extremely pretty, but what else to do?
                // The period of the variable is not checked/matched at all (only the name), 
                // but that is perhaps not
                // necessary, since the period has already been filtered by the DECOMP time period.
                int lag; string name;
                ConvertFromTurtleName(endoReverse[row], true, out name, out lag);                
                if (!decompOptions2.new_select.Contains(name.Split(':')[1], StringComparer.OrdinalIgnoreCase)) continue;

                for (int col = 0; col < exo.Count; col++)
                {
                    string endoName = endoReverse[row];
                    int etime; string ename;
                    ConvertFromTurtleName(endoName, true, out ename, out etime);

                    string exoName = exoReverse[col];
                    int xtime; string xname;
                    ConvertFromTurtleName(exoName, true, out xname, out xtime);

                    string enewName = ConvertToTurtleName(ename, 0);
                    int xlag = xtime - etime;
                    GekkoTime time = new GekkoTime(EFreq.A, etime, 1);
                    string xnewName = ConvertToTurtleName(xname, xlag);

                    int ZERO = 0;
                    DecompDict dd = null;
                    if (op.isRaw)
                    {
                        Tuple<Series, Series> tup1 = GetRealTimeseries(decompDatas, xnewName);
                        if (tup1.Item1 != null) decompDatas.MAIN_data[ZERO].cellsQuo[xnewName].SetData(time, tup1.Item1.GetDataSimple(time));
                        if (tup1.Item2 != null) decompDatas.MAIN_data[ZERO].cellsRef[xnewName].SetData(time, tup1.Item2.GetDataSimple(time));
                        if (col == 0)
                        {
                            Tuple<Series, Series> tup2 = GetRealTimeseries(decompDatas, enewName);
                            if (tup2.Item1 != null) decompDatas.MAIN_data[ZERO].cellsQuo[enewName].SetData(time, tup2.Item1.GetDataSimple(time));
                            if (tup2.Item2 != null) decompDatas.MAIN_data[ZERO].cellsRef[enewName].SetData(time, tup2.Item2.GetDataSimple(time));
                        }
                    }
                    else
                    {
                        dd = GetDecompDatas(decompDatas.MAIN_data[ZERO], operatorOneOf3Types);
                        Series ts2 = dd[xnewName];
                        ts2.SetData(time, effect[row, col]);
                        if (col == 0)  //just once
                        {                            
                            Series ts3 = dd[enewName];
                            ts3.SetData(time, 1d);
                        }
                    }
                }
            }

            //DecompRemoveResidualsIfZero(per1, per2, decompDatas, operatorOneOf3Types);
        }

        private static void DecompMainMergeOrAdd(DecompDatas decompDatas, DecompData dd, int ii, int jj)
        {            
            MergeDecompDict(dd.cellsContribD, decompDatas.storage[ii][jj].cellsContribD);
            MergeDecompDict(dd.cellsContribDRef, decompDatas.storage[ii][jj].cellsContribDRef);
            MergeDecompDict(dd.cellsContribM, decompDatas.storage[ii][jj].cellsContribM);
            MergeDecompDict(dd.cellsGradQuo, decompDatas.storage[ii][jj].cellsGradQuo);
            MergeDecompDict(dd.cellsGradRef, decompDatas.storage[ii][jj].cellsGradRef);
            MergeDecompDict(dd.cellsQuo, decompDatas.storage[ii][jj].cellsQuo);
            MergeDecompDict(dd.cellsRef, decompDatas.storage[ii][jj].cellsRef);
        }

        private static void MergeDecompDict(DecompDict dNew, DecompDict dOld)
        {
            foreach (KeyValuePair<string, Series> kvp in dNew.storage)
            {
                Series tsNew = kvp.Value;
                Series tsOld = dOld[kvp.Key]; //may be created
                GekkoTime t1 = tsNew.GetRealDataPeriodFirst();
                GekkoTime t2 = tsNew.GetRealDataPeriodLast();
                if (!t1.IsNull())
                {
                    foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
                    {
                        //the following if is probably not necessary
                        if (G.isNumericalError(tsOld.GetDataSimple(t))) tsOld.SetData(t, tsNew.GetDataSimple(t));
                    }
                }
            }
        }

        private static void PrintDecompDict(DecompDict d)
        {
            foreach (KeyValuePair<string, Series> kvp in d.storage)
            {
                Series ts = kvp.Value;
                GekkoTime t1 = ts.GetRealDataPeriodFirst();
                GekkoTime t2 = ts.GetRealDataPeriodLast();
                if (!t1.IsNull())
                {
                    int missings = 0;
                    foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
                    {
                        //the following if is probably not necessary
                        if (G.isNumericalError(ts.GetDataSimple(t))) missings++;
                    }
                    string m = null;
                    if (missings > 0) m = ", !!!!! missings = " + missings;
                    new Writeln(kvp.Key + " ---> data for " + t1.ToString() + "-" + t2.ToString() + m);
                }
                else
                {
                    new Writeln(kvp.Key + " ---> all missings");
                }
            }
        }

        private static void DecompRemoveResidualsIfZero(GekkoTime per1, GekkoTime per2, DecompDatas decompDatas, EContribType operatorOneOf3Types)
        {
            for (int parentJ = 0; parentJ < decompDatas.MAIN_data.Count; parentJ++)
            {
                List<string> remove = new List<string>();
                DecompDict dd = GetDecompDatas(decompDatas.MAIN_data[parentJ], operatorOneOf3Types);
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
                foreach (string s in remove)
                {
                    bool b = dd.Remove(s);
                }
            }
        }

        private static void DecompCheckNumberOfEqsAndEndo(DecompDatas decompDatas, GekkoDictionary<string, int> endo)
        {
            int nEqs = 0;
            for (int i = 0; i < decompDatas.storage.Count; i++) //for each linked eq, including the first one
            {
                for (int j = 0; j < decompDatas.storage[i].Count; j++) //for each uncontrolled set in eq
                {
                    nEqs++;
                }
            }

            if (nEqs != endo.Count)
            {
                using (Error e = new Error())
                {
                    e.MainAdd("The number of equations and endogenous variables do not match (" + nEqs + " vs " + endo.Count + "). ");
                    e.MainAdd("Equations (unrolled over sets):");
                    for (int i = 0; i < decompDatas.storage.Count; i++) //for each linked eq, including the first one
                    {
                        e.MainNewLineTight();
                        e.MainAdd("Equation #" + (i + 1) + " has " + decompDatas.storage[i].Count + " unrolled equations");
                    }
                }
            }
        }

        // ----------------------------
        // Turtle name start
        // ----------------------------

        private static string ConvertToTurtleName(string s, int lag, GekkoTime t)
        {
            return s + "¤[" + t.Add(lag).ToString() + "]";
        }

        private static string ConvertToTurtleName(string s, int lag)
        {
            string slag = lag.ToString();
            if (lag > 0) slag = "+" + slag;
            slag = "[" + slag + "]";
            return s + "¤" + slag;
        }

        /// <summary>
        /// Splits something like x[a, b]¤[-1] up into -1 and x[a, b]. If no ¤, the lag is 0.
        /// </summary>
        /// <param name="varname"></param>
        /// <param name="lag"></param>
        /// <param name="name"></param>
        public static void ConvertFromTurtleName(string varname, bool strict, out string name, out int lag)
        {
            lag = -12345;
            name = null;
            if (varname == null)
            {
                //do nothing
            }
            else
            {
                string[] ss = varname.Split('¤');
                if (strict && ss.Length != 2) new Error("Turtle error");
                if (ss.Length == 1)
                {
                    lag = 0;
                    name = varname;
                }
                else if (ss.Length == 2)
                {
                    lag = int.Parse(ss[1].Substring(1, ss[1].Length - 2));
                    name = ss[0];
                }
                else new Error("Turtle error");
            }
        }

        // ----------------------------
        // Turtle name end
        // ----------------------------

        public static DecompDict GetDecompDatas(DecompData decompData, EContribType operatorOneOf3Types)
        {
            if (operatorOneOf3Types == EContribType.D) return decompData.cellsContribD;
            else if (operatorOneOf3Types == EContribType.RD) return decompData.cellsContribDRef;
            else if (operatorOneOf3Types == EContribType.M) return decompData.cellsContribM;
            else
                new Error("Wrong type");            
            return null;
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

        /// <summary>
        /// Kind of an entry point for decomposition, also called when buttons are clicked etc.
        /// For a GAMS model, this calls DecompEvalGams(), and for a Gekko model, this calls 
        /// DecompEvalGekko(). Opens up a new window, unless windowDecomp != null. If windowDecomp is used,
        /// make sure that decompFind corresponds to the windowDecomp window (perhaps taken as windowDecomp.decompFind).
        /// </summary>
        /// <param name="o"></param>
        public static void DecompGetFuncExpressionsAndRecalc(DecompFind decompFind, WindowDecomp windowDecomp)
        {
            bool createNewWindow = windowDecomp == null;
            DecompOptions2 decompOptions = decompFind.decompOptions2;
            if (decompOptions.modelType == EModelType.Unknown)
            {
                new Error("DECOMP: A model is not loaded, cf. the MODEL command.");
            }

            if (createNewWindow) windowDecomp = new WindowDecomp(decompFind);
            Globals.windowsDecomp2.Add(windowDecomp);            

            //G.Writeln2(">>>getexpressions start " + DateTime.Now.ToLongTimeString());
            int count = -1;
            foreach (Link link in decompOptions.link)
            {
                count++;

                if (decompOptions.modelType == EModelType.Gekko)
                {
                    //Gekko model
                    //Gekko model
                    //Gekko model
                    //Gekko model
                    //Gekko model                    

                    if (link.expressions.Count != 1) new Error("Expected 1 link expression");
                    if (link.expressions[0] == null)
                    {
                        // NEW GEKKO MODEL DECOMP
                        // NEW GEKKO MODEL DECOMP
                        // NEW GEKKO MODEL DECOMP
                        EquationHelper found = DecompEvalGekko(link.varnames[0]);
                        link.expressions = found.expressions;
                        decompOptions.expressionOld = found.equationText;
                    }
                    else
                    {
                        new Error("Expected 1 link expression");
                    }
                }
                else if (decompOptions.modelType == EModelType.GAMSRaw)
                {
                    //GAMS model
                    //GAMS model
                    //GAMS model
                    //GAMS model
                    //GAMS model
                    if (link.expressions.Count != 1) new Error("Expected 1 link expression");
                    if (link.expressions[0] == null)
                    {
                        //
                        // NEW GAMS MODEL DECOMP
                        //
                        ModelGamsEquation found = GamsModel.DecompEvalGams(link.eqname, link.varnames[0]);  //if link.eqname != null, link.varnames[0] is not used at all
                        link.expressions = found.expressions;
                        link.expressionText = found.lhs + " = " + found.rhs;
                    }
                }
                else if (decompOptions.modelType == EModelType.GAMSScalar)
                {
                    //GAMS scalar model
                    //GAMS scalar model
                    //GAMS scalar model ----> no need to do anything??
                    //GAMS scalar model
                    //GAMS scalar model
                    //if (link.expressions.Count != 1) new Error("Expected 1 link expression");
                    //if (link.expressions[0] == null)
                    //{
                    //    //
                    //    // NEW GAMS SCALAR MODEL DECOMP
                    //    //

                    //    int eqNumber = Program.model.modelGamsScalar.GetEqNumber(link.eqname);
                    //    link.expressionText = link.eqname + " --> y = x [TODO]";
                    //    link.GAMS_eqNumber = eqNumber; //Inside Program.model.modelGamsScalar.functions, this i points to the right Func<int, double[], double[][], double[], int[][], int[][], double> expression.

                    //}
                }
                else new Error("Model type error");
            }
            //G.Writeln2(">>>getexpressions end " + DateTime.Now.ToLongTimeString());
            
            if (createNewWindow)
            {
                if (decompOptions.name == null)
                {
                    windowDecomp.Title = "Decompose expression";
                }
                else
                {
                    windowDecomp.Title = "Decompose " + decompOptions.variable + "";
                }
                windowDecomp.Tag = decompOptions;

                windowDecomp.isInitializing = true;  //so we don't get a recalc here because of setting radio buttons
                windowDecomp.SetRadioButtons();
                windowDecomp.isInitializing = false;

                windowDecomp.RecalcCellsWithNewType(true);
                decompOptions.numberOfRecalcs++;  //signal for Decomp() method to move on

                if (G.IsUnitTesting() && Globals.showDecompTable == false)
                {
                    Globals.windowsDecomp2.Clear();
                    windowDecomp = null;
                }
                else
                {
                    if (windowDecomp.isClosing)  //if something goes wrong, .isClosing will be true
                    {
                        //The line below removes the window from the global list of active windows.
                        //Without this line, this half-dead window will mess up automatic closing of windows (Window -> Close -> Close all...)
                        if (Globals.windowsDecomp2.Count > 0) Globals.windowsDecomp2.RemoveAt(Globals.windowsDecomp2.Count - 1);
                    }
                    else
                    {
                        windowDecomp.decompFind.SetWindow(windowDecomp);
                        windowDecomp.ShowDialog();
                        windowDecomp.Close();  //probably superfluous
                        windowDecomp = null;  //probably superfluous
                        if (Globals.showDecompTable)
                        {
                            Globals.showDecompTable = false;
                            new Error("Debug, tables aborted. Set Globals.showDecompTable = false.");
                        }
                    }
                }
            }
            else
            {
                windowDecomp.RecalcCellsWithNewType(true);
            }
        }

        /// <summary>
        /// Called by DecompMain() and performs the low-level math, obtaining and preparing data and
        /// calculating gradients. Used for "normal" Gekko models, possibly with folded equations.
        /// </summary>
        /// <param name="tt1"></param>
        /// <param name="tt2"></param>
        /// <param name="expression"></param>
        /// <param name="workOrRefOrBoth"></param>
        /// <param name="residualName"></param>
        /// <param name="funcCounter"></param>
        /// <returns></returns>
        public static DecompData DecompLowLevel(GekkoTime tt1, GekkoTime tt2, Func<GekkoSmpl, IVariable> expression, EDecompBanks workOrRefOrBoth, string residualName, ref int funcCounter)
        {
            //See #kljaf89usafasdf for scalar model
            //
            //
            //
            //                  Ref     -- m -->     Work
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
            else if (workOrRefOrBoth == EDecompBanks.Multiplier)
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
                DecompInitDict(d);

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


                //IMPORTANT
                //IMPORTANT
                //IMPORTANT
                Globals.precedents = null;  //!!! This is important: if not set to null, afterwards there will be a lot of superfluous lookup in the dictionary
                //IMPORTANT
                //IMPORTANT
                //IMPORTANT

                Series y0a_series = y0a as Series;
                if (y0a == null)
                {
                    new Error("DECOMP expects the expression to be of series type");
                    //throw new GekkoException();
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
                        new Error("DECOMP expects the expression to be of series type");
                        //throw new GekkoException();
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
                                        string nameOriginal = G.Chop_RemoveFreq(dp.s, tt1.freq);

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
                                                            d.cellsRef[name].SetData(t2, x_before);  // for j != 0, x_before is from Ref bank.
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

        /// <summary>
        /// Called by DecompMain() and performs the low-level math, obtaining and preparing data and
        /// calculating gradients. Used for GAMS scalar models.
        /// </summary>
        /// <param name="tt1"></param>
        /// <param name="tt2"></param>
        /// <param name="eq"></param>
        /// <param name="workOrRefOrBoth"></param>
        /// <param name="residualName"></param>
        /// <param name="funcCounter"></param>
        /// <returns></returns>
        public static DecompData DecompLowLevelScalar(GekkoTime gt1, GekkoTime gt2, int linkNumber, DecompStartHelper dsh, DecompOperator op, string residualName, ref int funcCounter)
        {
            // This gets called for each link equation, for instance e5[t]...  Then it is run over t, 
            // so we are evaluating e5[2001], e5[2002], etc. These t's determine the period of the
            // contributions, and if e5[2002] depends on x[2001], this will show up
            // as x[-1] in the 2002-contribution of e5. So if we visualize rows with x[-1], x, x[+1], z, etc.
            // and cols with 2001, 2002, 2003, etc., each atomic lowest level equation is actually present
            // separately in these columns.
            
            //See #kljaf89usafasdf for Gekko  model
            
            double eps = Globals.newtonSmallNumber;

            DecompData d = new DecompData();

            DecompInitDict(d);                                    

            GekkoDictionary<string, int> vars = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            
            if (true)
            {
                //foreach time period
                foreach (GekkoTime t in new GekkoTimeIterator(gt1, gt2))
                {
                    // TODO TODO TODO
                    // TODO TODO TODO
                    // TODO TODO TODO
                    // TODO TODO TODO skip via a dictionary if already done
                    // TODO TODO TODO
                    // TODO TODO TODO
                    // TODO TODO TODO

                    int timeIndex = Program.model.modelGamsScalar.FromGekkoTimeToTimeInteger(t);

                    int eqNumber = -12345;

                    string s = AddTimeToIndexes(dsh.name, new List<string>(dsh.indexes.storage), t);

                    bool b = Program.model.modelGamsScalar.dict_FromEqNameToEqNumber.TryGetValue(s, out eqNumber);
                    if (!b)
                    {
                        new Error("Could not find equation '" + s + "'");
                    }

                    double y0 = double.NaN;
                    double y1 = double.NaN;

                    //foreach precedent variable
                    int i = -1;
                    foreach (PeriodAndVariable dp in Program.model.modelGamsScalar.precedents[eqNumber].vars)
                    {
                        // --------------------------------------------
                        // This is where the decomposition takes place
                        // --------------------------------------------

                        i++;
                        string varName = Program.model.modelGamsScalar.GetVarNameA(dp.variable);

                        if (op.isRaw)
                        {
                            //raw data.
                            //a bit of a hack here, since all data is fetched (extra will contain all periods),
                            //and both quo and ref are fetched.
                            //but it should be fast anyway
                            //normal multiplier like <m>

                            if (i == 0)
                            {
                                y0 = Program.model.modelGamsScalar.Eval(dsh.periods[timeIndex].eqNumber, true, ref funcCounter);
                                d.cellsRef[residualName].SetData(t, y0);
                                y1 = Program.model.modelGamsScalar.Eval(dsh.periods[timeIndex].eqNumber, false, ref funcCounter);
                                d.cellsQuo[residualName].SetData(t, y1);
                            }
                            double x0 = Program.model.modelGamsScalar.GetData(dp.date, dp.variable, true);
                            double x1 = Program.model.modelGamsScalar.GetData(dp.date, dp.variable, false);
                            int lag2 = dp.date - timeIndex;
                            string name = Program.databanks.GetFirst().name + ":" + ConvertToTurtleName(varName, lag2);
                            d.cellsRef[name].SetData(t, x0);
                            d.cellsQuo[name].SetData(t, x1);
                            if (!vars.ContainsKey(name))  //for decomp pivot
                            {
                                vars.Add(name, 0);
                            }
                        }
                        else
                        {
                            if (op.lowLevel == ELowLevel.OnlyQuo || op.lowLevel == ELowLevel.BothQuoAndRef)
                            {
                                //work difference like <d> ... or the special <mp>
                                if (i == 0)
                                {
                                    y0 = Program.model.modelGamsScalar.Eval(dsh.periods[timeIndex].eqNumber, false, ref funcCounter);
                                    d.cellsQuo[residualName].SetData(t, y0);
                                    y1 = Program.model.modelGamsScalar.Eval(dsh.periods[timeIndex + 1].eqNumber, false, ref funcCounter);
                                    d.cellsQuo[residualName].SetData(t.Add(1), y1);
                                }
                                double x0_before = Program.model.modelGamsScalar.GetData(dp.date, dp.variable, false);
                                double x1 = Program.model.modelGamsScalar.GetData(dp.date + 1, dp.variable, false);

                                try
                                {
                                    double x0_after = x0_before + eps;
                                    Program.model.modelGamsScalar.SetData(dp.date, dp.variable, false, x0_after);
                                    double y0_after = Program.model.modelGamsScalar.Eval(dsh.periods[timeIndex].eqNumber, false, ref funcCounter);
                                    double grad = (y0_after - y0) / eps;

                                    //if (!G.isNumericalError(grad) && grad != 0d)        //this grad != 0 originates from the Gekko decomp, and only makes sense when excact precedents are not known
                                    //see also #sf94lkjsdjæ
                                    if (!G.isNumericalError(grad))
                                    {
                                        int lag2 = dp.date - timeIndex;
                                        string name = Program.databanks.GetFirst().name + ":" + ConvertToTurtleName(varName, lag2);
                                        d.cellsQuo[name].SetData(t, x0_before); //for decomp period <2002 2002>, this will be 2001
                                        d.cellsQuo[name].SetData(t.Add(1), x1); //for decomp period <2002 2002>, this will be 2002
                                        d.cellsGradQuo[name].SetData(t, grad);  //for decomp period <2002 2002>, this will be 2001
                                        if (!vars.ContainsKey(name))  //for decomp pivot
                                        {
                                            vars.Add(name, 0);
                                        }
                                    }
                                }
                                finally
                                {
                                    Program.model.modelGamsScalar.SetData(dp.date, dp.variable, false, x0_before);
                                }
                            }



                            if (op.lowLevel == ELowLevel.OnlyRef || op.lowLevel == ELowLevel.BothQuoAndRef)
                            {
                                //ref difference like <rd> ... or the special <mp>
                                if (i == 0)
                                {
                                    y0 = Program.model.modelGamsScalar.Eval(dsh.periods[timeIndex].eqNumber, true, ref funcCounter);
                                    d.cellsRef[residualName].SetData(t, y0);
                                    y1 = Program.model.modelGamsScalar.Eval(dsh.periods[timeIndex + 1].eqNumber, true, ref funcCounter);
                                    d.cellsRef[residualName].SetData(t.Add(1), y1);
                                }
                                double x0_before = Program.model.modelGamsScalar.GetData(dp.date, dp.variable, true);
                                double x1 = Program.model.modelGamsScalar.GetData(dp.date + 1, dp.variable, true);

                                try
                                {
                                    double x0_after = x0_before + eps;
                                    Program.model.modelGamsScalar.SetData(dp.date, dp.variable, true, x0_after);
                                    double y0_after = Program.model.modelGamsScalar.Eval(dsh.periods[timeIndex].eqNumber, true, ref funcCounter);
                                    double grad = (y0_after - y0) / eps;

                                    //if (!G.isNumericalError(grad) && grad != 0d)        //this grad != 0 originates from the Gekko decomp, and only makes sense when excact precedents are not known
                                    //see also #sf94lkjsdjæ
                                    if (!G.isNumericalError(grad))
                                    {
                                        int lag2 = dp.date - timeIndex;
                                        string name = Program.databanks.GetFirst().name + ":" + ConvertToTurtleName(varName, lag2);
                                        d.cellsRef[name].SetData(t, x0_before); //for decomp period <2002 2002>, this will be 2001
                                        d.cellsRef[name].SetData(t.Add(1), x1); //for decomp period <2002 2002>, this will be 2002
                                        d.cellsGradRef[name].SetData(t, grad);  //for decomp period <2002 2002>, this will be 2001
                                        if (!vars.ContainsKey(name))  //for decomp pivot
                                        {
                                            vars.Add(name, 0);
                                        }
                                    }
                                }
                                finally
                                {
                                    Program.model.modelGamsScalar.SetData(dp.date, dp.variable, true, x0_before);
                                }
                            }

                            if (op.lowLevel == ELowLevel.Multiplier)
                            {
                                //normal multiplier like <m>
                                if (i == 0)
                                {
                                    y0 = Program.model.modelGamsScalar.Eval(dsh.periods[timeIndex].eqNumber, true, ref funcCounter);
                                    d.cellsRef[residualName].SetData(t, y0);
                                    y1 = Program.model.modelGamsScalar.Eval(dsh.periods[timeIndex].eqNumber, false, ref funcCounter);
                                    d.cellsQuo[residualName].SetData(t, y1);
                                }
                                double x0_before = Program.model.modelGamsScalar.GetData(dp.date, dp.variable, true);
                                double x1 = Program.model.modelGamsScalar.GetData(dp.date, dp.variable, false);

                                try
                                {
                                    double x0_after = x0_before + eps;
                                    Program.model.modelGamsScalar.SetData(dp.date, dp.variable, true, x0_after);
                                    double y0_after = Program.model.modelGamsScalar.Eval(dsh.periods[timeIndex].eqNumber, true, ref funcCounter);
                                    double grad = (y0_after - y0) / eps;

                                    //if (!G.isNumericalError(grad) && grad != 0d)    //this grad != 0 originates from the Gekko decomp, and only makes sense when excact precedents are not known
                                    //see also #sf94lkjsdjæ
                                    if (!G.isNumericalError(grad))
                                    {
                                        int lag2 = dp.date - timeIndex;
                                        string name = Program.databanks.GetFirst().name + ":" + ConvertToTurtleName(varName, lag2);
                                        d.cellsRef[name].SetData(t, x0_before);
                                        d.cellsQuo[name].SetData(t, x1);
                                        d.cellsGradRef[name].SetData(t, grad);
                                        if (!vars.ContainsKey(name))  //for decomp pivot
                                        {
                                            vars.Add(name, 0);
                                        }
                                    }
                                }
                                finally
                                {
                                    Program.model.modelGamsScalar.SetData(dp.date, dp.variable, true, x0_before);
                                }
                            }


                        }
                    }
                }

                //Here, cellsQuo + cellsRef + cellsGradQuo + cellsGradRef are calculated.
                //Grad tells us which lags are actually active.
                //If we know that lags beforehand, we could limit the lag loop and save time here.

                if (!op.isRaw)
                {
                    foreach (GekkoTime t2 in new GekkoTimeIterator(gt1, gt2))
                    {
                        int add = 1; if (op.lowLevel == ELowLevel.Multiplier) add = 0;
                        GekkoTime t = t2.Add(add);
                        foreach (string s in vars.Keys)
                        {                            
                            if (op.lowLevel == ELowLevel.OnlyQuo || op.lowLevel == ELowLevel.BothQuoAndRef)
                            {
                                double vQuo = d.cellsQuo[s].GetDataSimple(t);
                                double vQuoLag = d.cellsQuo[s].GetDataSimple(t.Add(-1));
                                double vGradQuoLag = d.cellsGradQuo[s].GetDataSimple(t.Add(-1));
                                double dContribD = vGradQuoLag * (vQuo - vQuoLag);
                                d.cellsContribD[s].SetData(t, dContribD);
                            } 
                            
                            if (op.lowLevel == ELowLevel.OnlyRef || op.lowLevel == ELowLevel.BothQuoAndRef)
                            {
                                double vRef = d.cellsRef[s].GetDataSimple(t);
                                double vRefLag = d.cellsRef[s].GetDataSimple(t.Add(-1));
                                double vGradRefLag = d.cellsGradRef[s].GetDataSimple(t.Add(-1));
                                double dContribDRef = vGradRefLag * (vRef - vRefLag);
                                d.cellsContribDRef[s].SetData(t, dContribDRef);
                            }

                            if (op.lowLevel == ELowLevel.Multiplier)
                            {
                                double vQuo = d.cellsQuo[s].GetDataSimple(t);
                                double vRef = d.cellsRef[s].GetDataSimple(t);
                                double vGradRef = d.cellsGradRef[s].GetDataSimple(t);
                                double dContribM = vGradRef * (vQuo - vRef);
                                d.cellsContribM[s].SetData(t, dContribM);
                            }
                        }

                        if (op.lowLevel == ELowLevel.OnlyQuo || op.lowLevel == ELowLevel.BothQuoAndRef)
                        {
                            d.cellsContribD[residualName].SetData(t, -(d.cellsQuo[residualName].GetDataSimple(t) - d.cellsQuo[residualName].GetDataSimple(t.Add(-1))));
                        }

                        if (op.lowLevel == ELowLevel.OnlyRef || op.lowLevel == ELowLevel.BothQuoAndRef)
                        {
                            d.cellsContribDRef[residualName].SetData(t, -(d.cellsRef[residualName].GetDataSimple(t) - d.cellsRef[residualName].GetDataSimple(t.Add(-1))));
                        }

                        if (op.lowLevel == ELowLevel.Multiplier)
                        {
                            d.cellsContribM[residualName].SetData(t, -(d.cellsQuo[residualName].GetDataSimple(t) - d.cellsRef[residualName].GetDataSimple(t)));
                        }
                    }
                }
            }

            return d;
        }        

        private static void DecompInitDict(DecompData d)
        {
            if (d.cellsGradQuo == null) d.cellsGradQuo = new DecompDict();
            if (d.cellsQuo == null) d.cellsQuo = new DecompDict();
            if (d.cellsContribD == null) d.cellsContribD = new DecompDict();
            if (d.cellsGradRef == null) d.cellsGradRef = new DecompDict();
            if (d.cellsRef == null) d.cellsRef = new DecompDict();
            if (d.cellsContribDRef == null) d.cellsContribDRef = new DecompDict();
            if (d.cellsContribM == null) d.cellsContribM = new DecompDict();
        }

        /// <summary>
        /// Transforms a pivot table from DecompMain() into a table suitable for showing in the Gekko GUI.
        /// </summary>
        /// <param name="per1"></param>
        /// <param name="per2"></param>
        /// <param name="decompDataMAINClone"></param>
        /// <param name="format"></param>
        /// <param name="operator1"></param>
        /// <param name="isShares"></param>
        /// <param name="smpl"></param>
        /// <param name="lhs"></param>
        /// <param name="expressionText"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="frame"></param>
        /// <param name="operatorOneOf3Types"></param>
        /// 
        /// <returns></returns>
        public static Table DecompPivotToTable(GekkoTime per1, GekkoTime per2, List<DecompData> decompDataMAINClone, DecompDatas decompDatas, DecompTablesFormat2 format, DecompOperator op, GekkoSmpl smpl, string lhs, string expressionText, DecompOptions2 decompOptions2, FrameLight frame, EContribType operatorOneOf3Types)
        {
            int parentI = 0;

            if (decompOptions2.modelType == EModelType.GAMSScalar)
            {
                ENormalizeType normalize = ENormalizeType.Lags;
                if (op.lowLevel == ELowLevel.BothQuoAndRef)
                {
                    DecompNormalize(per1, per2, decompOptions2, parentI, decompDataMAINClone, decompDatas, EContribType.D, normalize, op);
                    DecompNormalize(per1, per2, decompOptions2, parentI, decompDataMAINClone, decompDatas, EContribType.RD, normalize, op);
                }
                else
                {
                    int deduct = 0;
                    if (op.isDoubleDifQuo || op.isDoubleDifRef) deduct = -1;
                    DecompNormalize(per1.Add(deduct), per2, decompOptions2, parentI, decompDataMAINClone, decompDatas, operatorOneOf3Types, normalize, op);
                }
            }            

            if (!op.isRaw)
            {
                if (decompOptions2.modelType != EModelType.GAMSScalar)                
                {
                    //Old and bad method, make it disappear soon!
                    DecompNormalizeOLD(per1, per2, decompOptions2, parentI, decompDataMAINClone, operatorOneOf3Types);
                }
            }

            bool ageHierarchy = Globals.isAgeHierarchy;
            if (G.IsUnitTesting()) ageHierarchy = false;

            if (decompOptions2.rows.Count == 0 && decompOptions2.cols.Count == 0)
            {
                decompOptions2.rows = new List<string>() { "vars", "lags" };
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
            string col_valueAlternative = Globals.internalColumnIdentifyer + "valueAlternative";
            string col_valueLevel = Globals.internalColumnIdentifyer + "valueLevel";
            string col_valueLevelLag = Globals.internalColumnIdentifyer + "valueLevelLag";
            string col_valueLevelLag2 = Globals.internalColumnIdentifyer + "valueLevelLag2";
            string col_valueLevelRef = Globals.internalColumnIdentifyer + "valueLevelRef";
            string col_valueLevelRefLag = Globals.internalColumnIdentifyer + "valueLevelRefLag";
            string col_valueLevelRefLag2 = Globals.internalColumnIdentifyer + "valueLevelRefLag2";
            string col_equ = Globals.internalColumnIdentifyer + "equ";
            string col_fullVariableName = Globals.internalColumnIdentifyer + "fullVariableName";
            string gekko_null = "null";            

            frame.AddColName(col_t);
            frame.AddColName(col_value);
            frame.AddColName(col_valueAlternative);
            frame.AddColName(col_valueLevel);
            frame.AddColName(col_valueLevelLag);
            frame.AddColName(col_valueLevelLag2);
            frame.AddColName(col_valueLevelRef);
            frame.AddColName(col_valueLevelRefLag);
            frame.AddColName(col_valueLevelRefLag2);
            frame.AddColName(col_variable);
            frame.AddColName(col_lag);
            frame.AddColName(col_universe);
            frame.AddColName(col_equ);
            frame.AddColName(col_fullVariableName);
            if (ageHierarchy)
            {
                frame.AddColName(Globals.internalSetIdentifyer + Globals.ageHierarchyName);
            }

            int superN = decompDataMAINClone.Count;

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

                    //second time, no loop..........

                    DecompDict dd = null;
                    if (op.isRaw)
                    {
                        //data is not used from here, it is just to get the list of
                        //relevant variables. For multiplier type, both if's are true,
                        //and in that case we just use the first.
                        dd = decompDataMAINClone[super].cellsQuo;
                        if (op.lowLevel == ELowLevel.OnlyRef) dd = decompDataMAINClone[super].cellsRef;
                    }                    
                    else
                    {
                        if (op.lowLevel == ELowLevel.BothQuoAndRef)
                        {
                            dd = GetDecompDatas(decompDataMAINClone[super], EContribType.D);  //could just as well be .RD, we are only using the keys
                        }
                        else
                        {
                            dd = GetDecompDatas(decompDataMAINClone[super], operatorOneOf3Types);
                        }
                    }

                    foreach (string dictName in dd.storage.Keys)
                    {
                        i++;

                        string dbName = null; string varName = null; string freq = null; string[] indexes = null;
                        string[] domains = null;

                        //See #876435924365

                        string lag = null;

                        //there is some repeated work done here, but not really bad
                        //problem is we prefer to do one period at a time, to sum up, adjust etc.

                        string[] ss = dictName.Split('¤');
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
                        double dLevelLag2 = double.NaN;
                        double dLevelRef = double.NaN;
                        double dLevelRefLag = double.NaN;
                        double dLevelRefLag2 = double.NaN;

                        if (true)
                        {
                            if (dictName.Contains(Globals.decompResidualName))
                            {
                                dLevel = double.NaN;
                            }
                            else
                            {

                                //MAybe turn this of for x-type...

                                if (op.operatorLower.StartsWith("x"))
                                {
                                    Tuple<Series, Series> tup = GetRealTimeseries(decompDatas, dictName);
                                    Series tsFirst = tup.Item1;
                                    Series tsRef = tup.Item2;
                                    dLevel = tsFirst.GetDataSimple(t2);
                                    dLevelLag = tsFirst.GetDataSimple(t2.Add(-1));
                                    dLevelLag2 = tsFirst.GetDataSimple(t2.Add(-2));
                                    dLevelRef = tsRef.GetDataSimple(t2);
                                    dLevelRefLag = tsRef.GetDataSimple(t2.Add(-1));
                                    dLevelRefLag2 = tsRef.GetDataSimple(t2.Add(-2));
                                }
                                else
                                {

                                    if (operatorOneOf3Types == EContribType.N || operatorOneOf3Types == EContribType.M || operatorOneOf3Types == EContribType.D)
                                    {
                                        Series tsFirst = null;
                                        tsFirst = O.GetIVariableFromString(fullName, O.ECreatePossibilities.NoneReturnNull) as Series;
                                        if (tsFirst == null)
                                        {
                                            new Error("Decomp #7093473984");
                                        }
                                        dLevel = tsFirst.GetDataSimple(t2);
                                        dLevelLag = tsFirst.GetDataSimple(t2.Add(-1));
                                        dLevelLag2 = tsFirst.GetDataSimple(t2.Add(-2));
                                    }

                                    if (operatorOneOf3Types == EContribType.RN || operatorOneOf3Types == EContribType.M || operatorOneOf3Types == EContribType.RD)
                                    {
                                        Series tsRef = null;
                                        tsRef = O.GetIVariableFromString(G.Chop_SetBank(fullName, "Ref"), O.ECreatePossibilities.NoneReturnNull) as Series;
                                        if (tsRef == null)
                                        {
                                            new Error("Decomp #7093473985");
                                        }
                                        dLevelRef = tsRef.GetDataSimple(t2);
                                        dLevelRefLag = tsRef.GetDataSimple(t2.Add(-1));
                                        dLevelRefLag2 = tsRef.GetDataSimple(t2.Add(-2));
                                    }
                                }
                            }
                        }

                        double d = double.NaN;
                        double dAlternative = double.NaN;
                        if (op.isDoubleDifQuo)  //dp
                        {
                            d = DecomposePutIntoTable2HelperOperators(decompDataMAINClone[super], "d", smpl, lhs, t2, dictName, decompOptions2.modelType == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                            dAlternative = DecomposePutIntoTable2HelperOperators(decompDataMAINClone[super], "d", smpl, lhs, t2.Add(-1), dictName, decompOptions2.modelType == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                        }
                        else if (op.isDoubleDifRef) //rdp
                        {
                            d = DecomposePutIntoTable2HelperOperators(decompDataMAINClone[super], "rd", smpl, lhs, t2, dictName, decompOptions2.modelType == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                            dAlternative = DecomposePutIntoTable2HelperOperators(decompDataMAINClone[super], "rd", smpl, lhs, t2.Add(-1), dictName, decompOptions2.modelType == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                        }
                        else if (op.lowLevel == ELowLevel.BothQuoAndRef) //mp
                        {
                            d = DecomposePutIntoTable2HelperOperators(decompDataMAINClone[super], "d", smpl, lhs, t2, dictName, decompOptions2.modelType == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                            dAlternative = DecomposePutIntoTable2HelperOperators(decompDataMAINClone[super], "rd", smpl, lhs, t2, dictName, decompOptions2.modelType == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                        }
                        else
                        {
                            d = DecomposePutIntoTable2HelperOperators(decompDataMAINClone[super], op.operatorLower, smpl, lhs, t2, dictName, decompOptions2.modelType == EModelType.GAMSScalar, decompOptions2.missingAsZero);
                            dAlternative = double.NaN;
                        }

                        FrameLightRow dr = new FrameLightRow(frame);
                        //dr.Set(frame, col_fullVariableName, new CellLight(G.Chop_RemoveBank(fullName)));

                        string dictName2 = dictName.Replace("Work:", "").Replace("¤[0]", "");                        

                        dr.Set(frame, col_fullVariableName, new CellLight(dictName2));
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
                        dr.Set(frame, col_valueAlternative, new CellLight(dAlternative));
                        dr.Set(frame, col_valueLevel, new CellLight(dLevel));
                        dr.Set(frame, col_valueLevelLag, new CellLight(dLevelLag));
                        dr.Set(frame, col_valueLevelLag2, new CellLight(dLevelLag2));
                        dr.Set(frame, col_valueLevelRef, new CellLight(dLevelRef));
                        dr.Set(frame, col_valueLevelRefLag, new CellLight(dLevelRefLag));
                        dr.Set(frame, col_valueLevelRefLag2, new CellLight(dLevelRefLag2));

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

            if (false && Globals.decompUnitCsvPivot)
            {
                WriteDatatableTocsv(frame);
            }

            Table tab = new Table();
            tab.writeOnce = true;

            int xlag = 0; string temp = null;
            ConvertFromTurtleName(decompDataMAINClone[parentI].lhs, true, out temp, out xlag);
            string normalizerVariableWithIndex = null;
            if (temp != null) normalizerVariableWithIndex = G.Chop_RemoveBank(temp);

            DecomposeReplaceVars(decompOptions2.rows, col_t, col_variable, col_lag, col_universe, col_equ);
            DecomposeReplaceVars(decompOptions2.cols, col_t, col_variable, col_lag, col_universe, col_equ);
            DecomposeReplaceVars(decompOptions2.filters, col_t, col_variable, col_lag, col_universe, col_equ);

            List<string> rownames3 = new List<string>();
            List<string> colnames3 = new List<string>();
            GekkoDictionary<string, AggContainer> agg = new GekkoDictionary<string, AggContainer>(StringComparer.OrdinalIgnoreCase);

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


            List<string> varnames = decompOptions2.link[parentI].varnames;

            bool decompHasLag = false;
            if (decompOptions2.rows.Contains(col_lag) || decompOptions2.cols.Contains(col_lag))
            {
                decompHasLag = true;
            }

            // ==============================================================================
            //Aggregation
            //Aggregation
            //Aggregation into table suitable for showing
            //Aggregation
            //Aggregation
            // ==============================================================================
                       

            foreach (FrameLightRow row in frame.rows)
            {                
                ENormalizerType normalizerType = ENormalizerType.None;
                
                if (G.Equal(normalizerVariableWithIndex, row.Get(frame, col_fullVariableName).text))
                {
                    if (row.Get(frame, col_lag).text == "[0]") normalizerType = ENormalizerType.Normalizer;
                    else normalizerType = ENormalizerType.NormalizerWithLagOrLead;
                }

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

                string more = null;
                if (decompOptions2.modelType == EModelType.GAMSScalar)
                {
                    //if (!decompHasLag && normalizerType == ENormalizerType.NormalizerWithLagOrLead)
                    //{
                    //    more = Globals.pivotHelper1;  //so that it is set apart
                    //}
                    if (normalizerType == ENormalizerType.NormalizerWithLagOrLead) more = Globals.pivotHelper1;  //so that it is set apart
                    else if (normalizerType == ENormalizerType.Normalizer) more = Globals.pivotHelper2;
                }

                string s1 = null;
                foreach (string s in decompOptions2.rows)
                {                    
                    s1 = DecompAddText(frame, row, s1, s);
                    if (s == col_variable) s1 += more;
                }
                if (s1 != null)
                {
                    s1 = s1.Substring(Globals.pivotTableDelimiter.Length);
                }               

                string s2 = null;
                foreach (string s in decompOptions2.cols)
                {
                    s2 = DecompAddText(frame, row, s2, s);
                    if (s == col_variable) s2 += more;
                }
                if (s2 != null)
                {
                    s2 = s2.Substring(Globals.pivotTableDelimiter.Length);
                }
                string key = s1 + "¤" + s2;  //row ¤ col                

                if (!rownames3.Contains(s1, StringComparer.OrdinalIgnoreCase)) rownames3.Add(s1);
                if (!colnames3.Contains(s2, StringComparer.OrdinalIgnoreCase)) colnames3.Add(s2);

                double d = row.Get(frame, col_value).data;
                double dAlternative = row.Get(frame, col_valueAlternative).data;
                double dLevel = row.Get(frame, col_valueLevel).data;
                double dLevelLag = row.Get(frame, col_valueLevelLag).data;
                double dLevelLag2 = row.Get(frame, col_valueLevelLag2).data;
                double dLevelRef = row.Get(frame, col_valueLevelRef).data;
                double dLevelRefLag = row.Get(frame, col_valueLevelRefLag).data;
                double dLevelRefLag2 = row.Get(frame, col_valueLevelRefLag2).data;
                string fullVariableName = row.Get(frame, col_fullVariableName).text;

                AggContainer td = null;
                agg.TryGetValue(key, out td);
                if (td == null)
                {
                    agg.Add(key, new AggContainer(d, dAlternative, dLevel, dLevelLag, dLevelLag2, dLevelRef, dLevelRefLag, dLevelRefLag2, 1, new List<string>() { fullVariableName }));
                }
                else
                {
                    td.change += d;
                    td.changeAlternative += dAlternative;
                    td.level += dLevel;
                    td.levelLag += dLevelLag;
                    td.levelLag2 += dLevelLag2;
                    td.levelRef += dLevelRef;
                    td.levelRefLag += dLevelRefLag;
                    td.levelRefLag2 += dLevelRefLag2;
                    td.n += 1;
                    //BEWARE
                    //BEWARE
                    //BEWARE Is this too time-consuming?
                    //BEWARE
                    //BEWARE
                    td.fullVariableNames.Add(fullVariableName);
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
                        
            bool orderNormalize = OrderNormalize(decompOptions2, varnames);

            List<string> rownames = new List<string>();
            List<string> colnames = new List<string>();

            string rownamesFirst = null;
            for (int i = 0; i < rownames3.Count; i++)
            {
                bool b1 = rownamesFirst == null && orderNormalize && DecompMatchWord(rownames3[i], varnames[0]);
                bool b2 = rownamesFirst == null && orderNormalize && rownames3[i].Contains(Globals.pivotHelper2);
                if ((decompOptions2.modelType != EModelType.GAMSScalar && b1) || (decompOptions2.modelType == EModelType.GAMSScalar && b2))
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
                bool b1 = colnamesFirst == null && orderNormalize && DecompMatchWord(colnames3[i], varnames[0]);
                bool b2 = colnamesFirst == null && orderNormalize && colnames3[i].Contains(Globals.pivotHelper2);
                if ((decompOptions2.modelType != EModelType.GAMSScalar && b1) || (decompOptions2.modelType == EModelType.GAMSScalar && b2))
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
                if (rownamesFirst == null && colnamesFirst == null)
                {
                    MessageBox.Show("*** ERROR: Could not find row/col to put first for normalization");
                }
                if (rownamesFirst != null && colnamesFirst != null)
                {
                    MessageBox.Show("*** ERROR: Both row and col are set first for normalization");
                }
            }

            for (int i = 0; i < rownames.Count; i++)
            {
                for (int j = 0; j < colnames.Count; j++)
                {
                    string key = rownames[i] + "¤" + colnames[j];

                    AggContainer td = null;
                    agg.TryGetValue(key, out td);
                    double d = 0d;
                    double dAlternative = 0d;
                    double dLevel = 0d;
                    double dLevelLag = 0d;
                    double dLevelLag2 = 0d;
                    double dLevelRef = 0d;
                    double dLevelRefLag = 0d;
                    double dLevelRefLag2 = 0d;
                    int n = 0;
                    List<string> fullVariableNames = null;

                    if (td != null)
                    {
                        dLevel = td.level;
                        dLevelLag = td.levelLag;
                        dLevelLag2 = td.levelLag2;
                        dLevelRef = td.levelRef;
                        dLevelRefLag = td.levelRefLag;
                        dLevelRefLag2 = td.levelRefLag2;
                        n = td.n;
                        fullVariableNames = td.fullVariableNames;

                        // ----- first start -----------------------------------------------
                        double dFirstLevel = double.NaN;
                        double dFirstLevelLag = double.NaN;
                        double dFirstLevelLag2 = double.NaN;
                        double dFirstLevelRef = double.NaN;
                        double dFirstLevelRefLag = double.NaN;
                        double dFirstLevelRefLag2 = double.NaN;
                        int dFirstN = 0;
                        List<string> dFirstFullVariableNames = null;
                        string keyFirst = null;
                        if (rownamesFirst != null) keyFirst = rownamesFirst + "¤" + colnames[j];
                        else if (colnamesFirst != null) keyFirst = rownames[i] + "¤" + colnamesFirst;
                        AggContainer tdFirst = null;
                        agg.TryGetValue(keyFirst, out tdFirst);
                        if (tdFirst != null)
                        {
                            dFirstLevel = tdFirst.level;
                            dFirstLevelLag = tdFirst.levelLag;
                            dFirstLevelLag2 = tdFirst.levelLag2;
                            dFirstLevelRef = tdFirst.levelRef;
                            dFirstLevelRefLag = tdFirst.levelRefLag;
                            dFirstLevelRefLag2 = tdFirst.levelRefLag2;
                            dFirstN = tdFirst.n;
                            dFirstFullVariableNames = tdFirst.fullVariableNames;
                        }
                        // ----- first end --------------------------------------------------

                        if (op.operatorLower == "n" || op.operatorLower == "xn")
                        {
                            d = dLevel;
                        }
                        else if (op.operatorLower == "rn" || op.operatorLower == "r" || op.operatorLower == "xrn" || op.operatorLower == "xr")
                        {
                            d = dLevelRef;
                        }
                        else if (op.operatorLower == "d" || op.operatorLower == "sd")
                        {
                            d = td.change;
                        }
                        else if (op.operatorLower == "p" || op.operatorLower == "sp")
                        {
                            d = td.change / dFirstLevelLag * 100d;
                        }
                        else if (op.operatorLower == "dp" || op.operatorLower == "sdp")
                        {
                            d = td.change / dFirstLevelLag * 100d - td.changeAlternative / dFirstLevelLag2 * 100d;
                        }
                        else if (op.operatorLower == "m" || op.operatorLower == "sm")
                        {
                            d = td.change;
                        }
                        else if (op.operatorLower == "q" || op.operatorLower == "sq")
                        {
                            d = td.change / dFirstLevelRef * 100d;
                        }
                        else if (op.operatorLower == "mp" || op.operatorLower == "smp")
                        {
                            d = td.change / dFirstLevelLag * 100d - td.changeAlternative / dFirstLevelRefLag * 100d;
                        }
                        else if (op.operatorLower == "xd")
                        {
                            d = dLevel - dLevelLag;
                        }
                        else if (op.operatorLower == "xp")
                        {
                            d = (dLevel - dLevelLag) / dLevelLag * 100d;
                        }
                        else if (op.operatorLower == "xdp")
                        {
                            d = (dLevel - dLevelLag) / dLevelLag * 100d - (dLevelLag - dLevelLag2) / dLevelLag2 * 100d;
                        }
                        else if (op.operatorLower == "xm")
                        {
                            d = dLevel - dLevelRef;
                        }
                        else if (op.operatorLower == "xq")
                        {
                            d = (dLevel - dLevelRef) / dLevelRef * 100d;
                        }
                        else if (op.operatorLower == "xmp")
                        {
                            d = (dLevel - dLevelLag) / dLevelLag * 100d - (dLevelRef - dLevelRefLag) / dLevelRefLag * 100d;
                        }
                        // -----------------
                        else if (op.operatorLower == "rd" || op.operatorLower == "srd")
                        {
                            d = td.change;
                        }
                        else if (op.operatorLower == "rp" || op.operatorLower == "srp")
                        {
                            d = td.change / dFirstLevelRefLag * 100d;
                        }
                        else if (op.operatorLower == "rdp" || op.operatorLower == "srdp")
                        {
                            d = td.change / dFirstLevelRefLag * 100d - td.changeAlternative / dFirstLevelRefLag2 * 100d;
                        }
                        else if (op.operatorLower == "xrd")
                        {
                            d = dLevelRef - dLevelRefLag;
                        }
                        else if (op.operatorLower == "xrp")
                        {
                            d = (dLevelRef - dLevelRefLag) / dLevelRefLag * 100d;
                        }
                        else if (op.operatorLower == "xrdp")
                        {
                            d = (dLevelRef - dLevelRefLag) / dLevelRefLag * 100d - (dLevelRefLag - dLevelRefLag2) / dLevelRefLag2 * 100d;
                        }
                    }

                    int decimals = 0;
                    if (decompOptions2.decompTablesFormat.isPercentageType) decimals = decompOptions2.decompTablesFormat.decimalsPch;
                    else decimals = decompOptions2.decompTablesFormat.decimalsLevel;
                    string format2 = "f16." + decimals.ToString();
                    
                    if (decompOptions2.count == ECountType.N)
                    {
                        tab.SetNumber(i + 2, j + 2, n, "f16.0");
                    }
                    else if (decompOptions2.count == ECountType.Names)
                    {
                        tab.Set(i + 2, j + 2, Stringlist.GetListWithCommas(fullVariableNames));
                    }
                    else
                    {                        
                        tab.SetNumber(i + 2, j + 2, d, format2);                        
                    }

                    Cell c = tab.Get(i + 2, j + 2);
                    c.vars_hack = fullVariableNames;
                }
            }

            for (int i = 0; i < rownames.Count; i++)
            {
                string s = rownames[i];
                if (decompOptions2.modelType == EModelType.GAMSScalar)
                {
                    s = s.Replace(Globals.pivotHelper1, "").Replace(Globals.pivotHelper2, "");
                }
                tab.Set(i + 2, 1, s);
            }

            for (int j = 0; j < colnames.Count; j++)
            {
                string s = colnames[j];
                if (decompOptions2.modelType == EModelType.GAMSScalar)
                {
                    s = s.Replace(Globals.pivotHelper1, "").Replace(Globals.pivotHelper2, "");
                }
                tab.Set(1, j + 2, s);
            }

            if (decompOptions2.decompTablesFormat.isPercentageType)
            {
                tab.Set(1, 1, "%" + "  ");
            }

            return tab;
        }

        /// <summary>
        /// The decomp provides a linearization where the contributions sum to 0.
        /// Here, this is "translated" into the normal decomp way of showing it.
        /// This method is old, bad and soon obsolete.
        /// </summary>
        /// <param name="per1"></param>
        /// <param name="per2"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="parentI"></param>
        /// <param name="decompDatasSupremeClone"></param>
        /// <param name="operatorOneOf3Types"></param>
        private static void DecompNormalizeOLD(GekkoTime per1, GekkoTime per2, DecompOptions2 decompOptions2, int parentI, List<DecompData> decompDatasSupremeClone, EContribType operatorOneOf3Types)
        {
            DecompOperator op = new DecompOperator(decompOptions2.prtOptionLower);
            EDecompBanks edb = DecompBanks_OLDREMOVESOON(op);

            bool orderNormalize = OrderNormalize(decompOptions2, decompOptions2.link[parentI].varnames);
            //This normalizes the parent-link-variables so that they reflect their real values
            //Parent-link-variables are for instance x1, x2, x3 here: DECOMP x1, x2, x2 IN ...

            if (orderNormalize)
            {
                if (decompOptions2.link[parentI].varnames.Count != decompDatasSupremeClone.Count)
                {
                    new Error("The number of variables and equations do not match. For istance, in DECOMP x1, x2 in e_eqs, the equation e_eqs must contain 2 elements (that is, it must be defined over one or more sets with 2 elements in all).");
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
                            else if (edb == EDecompBanks.Multiplier)
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
                        new Error("Could not find variable " + name1 + " in non-linked equation number " + j + ".Beware of alignment: the names and equations must match.");
                    }
                }
            }

            return;
        }
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO -----------------------------------------------------------------------------
        // TODO TODO why seach for these ref/quo timeseries, why not just have them in 1 dictionary?
        // TODO TODO for unrolled eqs, this searching may take time.
        // TODO TODO -----------------------------------------------------------------------------
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
        /// <summary>
        /// The decomp provides a linearization where the contributions sum to 0.
        /// Here, this is "translated" into the normal decomp way of showing it.
        /// </summary>
        /// <param name="per1"></param>
        /// <param name="per2"></param>
        /// <param name="decompOptions2"></param>
        /// <param name="parentI"></param>
        /// <param name="decompDatasSupremeClone"></param>
        /// <param name="operatorOneOf3Types"></param>
        private static void DecompNormalize(GekkoTime per1, GekkoTime per2, DecompOptions2 decompOptions2, int parentI, List<DecompData> decompDatasSupremeClone, DecompDatas decompDatas, EContribType operatorOneOf3Types, ENormalizeType normalize, DecompOperator op)
        {
            // Decomp provides a linearization where the contributions sum to 0. Here we identify those
            // vars (contributions) that are moved to the LHS.
            //
            //Decomp decomposes into "atoms", like x[a], x[b], x[a][-1], x[b][+1], etc.
            //Regarding the LHS of decompose, this can only meaningfully be an atom, so it makes no
            //particular sense to aggregate for instance x[a] and x[a][-1], or x[a] and x[b]. Regarding
            //the latter, these may no even have the same units!
            //Consider the equation x[a] + 500 * x[b] = 1000 * x[a][-1]/x[b][+1]. 
            //How would you meaningfully aggregate into x[a] and x[b] on LHS? If we are only decomposing
            //x[a], we can look at how x[a] - @x[a] can be decomposed, which is easy enough. Burt 
            //decomposing x[a] and x[b] at the same time on LHS? This would only make sense if the derivatives
            //of x[a] and x[b] are equal, for instance x[a] + x[b] = 1000 * x[a][-1]/x[b][+1]. Then we could
            //state the decomposition of x[a] + x[b] - (@x[a] + @x[b]).
            //If the user wants this, he should add a real equation x = x[a] + x[b] to the system, and then
            //decompose x, choosing if x[a] or x[b] is considered endogenous. 
            //Gekko could make an easy interface regarding that. If the equation really is 
            //x[a] + x[b] = 1000 * x[a][-1]/x[b][+1], the choice of x[a] or x[b] as endogenous will not matter.
            //Because of all this, we cannot decompose x[a] + 500 * x[b] = 1000 * x[a][-1]/x[b][+1] with
            //x[a] on the RHS, excluding lags. If we exclude lags, we will get x[a][0] on the LHS, 
            //and x[a][-1] and x[b] on the LHS, where lags only disappear regarding x[b].
            //
            //This is similar to ADAM-style xa = -500 * xb + 1000 * xa[-1]/xb[+1]. Here, aggregating the RHS
            //lags would only aggregate xb and xb[+1], not xa and xa[-1].
            
            if (decompOptions2.link[parentI].varnames.Count != 1)
            {
                new Error("Expected 1 variable for decomposition, got " + decompOptions2.link[parentI].varnames.Count);
            }
            
            int j = 0;
            DecompData d = decompDatasSupremeClone[j];
            string name = decompOptions2.link[parentI].varnames[j];
            d.lhs = Program.databanks.GetFirst().name + ":" + ConvertToTurtleName(name, 0);  //lag = 0

            if (!decompOptions2.prtOptionLower.StartsWith("x"))
            {

                Series lhs2 = GetDecompDatas(decompDatasSupremeClone[j], operatorOneOf3Types)[d.lhs];
                //bool isResidualName = name == Globals.decompResidualName;
                Tuple<Series, Series> ts = GetRealTimeseries(decompDatas, d.lhs);

                foreach (GekkoTime t in new GekkoTimeIterator(per1, per2))
                {
                    double d1 = lhs2.GetDataSimple(t);
                    double d2 = double.NaN;
                    if (operatorOneOf3Types == EContribType.D)
                    {
                        d2 = ts.Item1.GetDataSimple(t) - ts.Item1.GetDataSimple(t.Add(-1));
                    }
                    else if (operatorOneOf3Types == EContribType.RD)
                    {
                        d2 = ts.Item2.GetDataSimple(t) - ts.Item2.GetDataSimple(t.Add(-1));
                    }
                    else if (operatorOneOf3Types == EContribType.M)
                    {
                        d2 = ts.Item1.GetDataSimple(t) - ts.Item2.GetDataSimple(t);
                    }

                    // ----------------------------------------------

                    // HACK HACK HACK
                    // HACK HACK HACK
                    // HACK HACK HACK
                    // HACK HACK HACK
                    // HACK HACK HACK
                    // HACK HACK HACK
                    // HACK HACK HACK this only solves shares for d, rd, m
                    // HACK HACK HACK
                    // HACK HACK HACK
                    // HACK HACK HACK
                    // HACK HACK HACK
                    // HACK HACK HACK
                    // HACK HACK HACK
                    // HACK HACK HACK
                    double factor = d2 / d1;
                    if (op.isShares)
                    {
                        factor = 100; //--> fixes d, rd, m
                    }

                    bool found = false;
                    foreach (KeyValuePair<string, Series> kvp in GetDecompDatas(d, operatorOneOf3Types).storage)
                    {
                        if (G.Equal(kvp.Key, d.lhs))
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
                        MessageBox.Show("*** ERROR: Did not find " + name + " for normalization");
                    }
                }
            }

            return;
        }

        /// <summary>
        /// Search for real observed timeseries inside the DecompData objects (so as not having to find them in databanks).
        /// Returns a tuple with quo (Work) as first element, and ref (Ref) as last element.
        /// </summary>
        /// <param name="decompDatas"></param>
        /// <param name="operatorOneOf3Types"></param>
        /// <param name="s"></param>
        /// <param name="tsQuo"></param>
        /// <param name="tsRef"></param>
        private static Tuple<Series, Series> GetRealTimeseries(DecompDatas decompDatas, string s)
        {
            Series tsQuo = null;
            Series tsRef = null;

            //Find the real values of the series for normalization

            foreach (List<DecompData> temp in decompDatas.storage)
            {
                foreach (DecompData decompData in temp)
                {
                    decompData.cellsQuo.storage.TryGetValue(s, out tsQuo);
                    if (tsQuo != null) goto Label1;
                }
            }
        Label1:;

            foreach (List<DecompData> temp in decompDatas.storage)
            {
                foreach (DecompData decompData in temp)
                {
                    decompData.cellsRef.storage.TryGetValue(s, out tsRef);
                    if (tsRef != null) goto Label2;
                }
            }
        Label2:;

            Tuple<Series, Series> ts = new Tuple<Series, Series>(tsQuo, tsRef);
            return ts;
        }        

        private static bool DecompMatchWord(string colnames3, string varnames)
        {
            if (colnames3 == null) return false;
            return G.ContainsWord(colnames3, G.Chop_GetName(varnames));
        }

        private static bool OrderNormalize(DecompOptions2 decompOptions2, List<string> varnames)
        {
            bool orderNormalize = false;
            if (decompOptions2.decompTablesFormat.showErrors)
            {
                if (decompOptions2.modelType == EModelType.GAMSScalar)
                {
                    //HAAAAAAACK!
                    //if (decompOptions2.prtOptionLower.StartsWith("x")) return false;
                    //HMMMMM not sure about this and what it does, or if .GAMS_dsh.Count should be used
                    if (varnames.Count == decompOptions2.link[0].GAMS_dsh.Count)
                    {
                        orderNormalize = true;
                    }
                    else
                    {
                        new Warning("Normalization ordering not implemented for sets of equations");
                    }
                }
                else
                {
                    if (varnames.Count == decompOptions2.link[0].expressions.Count)
                    {
                        orderNormalize = true;
                    }
                    else
                    {
                        new Warning("Normalization ordering not implemented for sets of equations");
                    }
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
            File.WriteAllText(@"c:\Thomas\Gekko\regres\Models\Decomp\pivot.csv", sb.ToString());
            //File.WriteAllText(Program.options.folder_working + "\\" + "decomp.csv", sb.ToString());
        }

        public static double DecomposePutIntoTable2HelperOperators(DecompData decompTables, string operatorLower, GekkoSmpl smpl, string lhs, GekkoTime t2, string colname, bool isScalarModel, bool missingAsZero)
        {
            
            double d = double.NaN;

            if (operatorLower == "d" || operatorLower == "p" || operatorLower == "sd" || operatorLower == "sp")
            {
                d = decompTables.cellsContribD[colname].GetData(smpl, t2);
            }
            else if (operatorLower == "rd" || operatorLower == "rp" || operatorLower == "srd" || operatorLower == "srp")
            {
                d = decompTables.cellsContribDRef[colname].GetData(smpl, t2);
            }
            else if (operatorLower == "m" || operatorLower == "q" || operatorLower == "sm" || operatorLower == "sq")
            {
                d = decompTables.cellsContribM[colname].GetData(smpl, t2);
            }
            else
            {
                //do nothing
            }            
            
            if (missingAsZero && isScalarModel && G.isNumericalError(d)) d = 0d;

            return d;
        }

        public static EDecompBanks DecompBanks_OLDREMOVESOON(DecompOperator op)
        {
            EDecompBanks banks = EDecompBanks.Work;
            string operator1 = op.operatorLower;
            if (operator1 == "r" || operator1 == "xr" || operator1 == "xrn" || operator1 == "rd" || operator1 == "xrd" || operator1 == "rp" || operator1 == "xrp" || operator1 == "rdp" || operator1 == "xrdp") banks = EDecompBanks.Ref;
            if (operator1 == "m" || operator1 == "xm" || operator1 == "q" || operator1 == "xq" || operator1 == "mp" || operator1 == "xmp") banks = EDecompBanks.Multiplier;
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
                        for (int i = 2021; i <= 2022; i++)
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
                new Error("Could not find variable " + name + " in linked equation #" + i + " of " + (decompDatas.Count - 1));
                //throw new GekkoException();
            }

            return parentJ;
        }


        public static Series FindLinkSeries(List<DecompData> decompData, int j, string linkVariable, EContribType operatorOneOf3Types)
        {
            if (!GetDecompDatas(decompData[j], operatorOneOf3Types).ContainsKey(linkVariable))
            {
                return null;
            }
            Series linkParent = GetDecompDatas(decompData[j], operatorOneOf3Types)[linkVariable];
            return linkParent;
        }

        public static Series FindLinkSeries(List<List<DecompData>> decompDatas, int i, int j, string linkVariable, EContribType operatorOneOf3Types)
        {
            return FindLinkSeries(decompDatas[i], j, linkVariable, operatorOneOf3Types);
        }

        public static EquationHelper DecompEvalGekko(string variable)
        {
            EquationHelper found = Program.FindEquationByMeansOfVariableName(variable);
            if (found == null)
            {
                new Error("DECOMP: Could not find variable '" + variable + "' as left-hand side in model");
                //throw new GekkoException();
            }
            string[] ss = found.equationText.Split('=');

            string rhs = ss[1].Trim();

            string lhsText = ss[0].Trim();
            string[] ss0 = lhsText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (!G.Equal(ss0[0], "frml"))
            {
                new Error("Model equation '" + variable + "': Equation does not start with 'frml'");
                //throw new GekkoException();
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
                    new Error("Problem with J-factors in equation " + found.lhs);
                    //throw new GekkoException();
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
                    G.Writeln2("EVAL " + G.ReplaceGlueSymbols(tmp));
                    G.Writeln2("-----------------------------------");
                }

                Globals.expressions = null;  //maybe not necessary

                Program.CallEval(null, tmp);

                found.expressions = new List<Func<GekkoSmpl, IVariable>>(Globals.expressions);  //probably needs cloning/copying as it is done here
                Globals.expressions = null;  //maybe not necessary               

            }
            catch (Exception e)
            {

            }

            return found;
        }

        public static string Find(O.Find o)
        {
            //For scalar model            

            Globals.itemHandler = new ItemHandler();  //hack

            o.t0 = o.decompFind.decompOptions2.t1;  //selected time
            List<string> vars = O.Restrict(o.iv, false, false, false, true);
            int timeIndex = Program.model.modelGamsScalar.FromGekkoTimeToTimeInteger(o.t0);
            string variableName = vars[0];
            string variableName2 = variableName + "[" + o.t0.ToString() + "]";
            int aNumber = Program.model.modelGamsScalar.dict_FromVarNameToANumber[variableName];
            PeriodAndVariable pav = new PeriodAndVariable(timeIndex, aNumber);
            //new Writeln("Variable " + variableName2 + ":");

            string firstText = null;
            List<string> firstList = new List<string>();
            string firstEqName = null;

            int lineCounter = -1;
            int counter2 = 0;
            List<int> eqNumbers = Program.model.modelGamsScalar.dependents[pav];

            List<EqHelper> eqs = new List<EqHelper>();
            foreach (int eqNumber in eqNumbers)
            {
                string eqName = Program.model.modelGamsScalar.GetEqName(eqNumber);
                string eqName3 = G.Chop_DimensionSetLag(eqName, o.t0, false);
                EqHelper e = new EqHelper();
                e.eqName = eqName;
                e.eqName3 = eqName3;
                e.eqNumber = eqNumber;
                eqs.Add(e);
            }

            foreach (EqHelper helper in eqs)
            {
                

            }

            foreach (EqHelper helper in eqs)
            {
                lineCounter++;
                string eqName = helper.eqName;
                string eqName3 = helper.eqName3;

                List<string> precedents = Program.model.modelGamsScalar.GetPrecedentsNames(helper.eqNumber, o.decompFind.decompOptions2.showTime, o.t0);

                string bool1 = "";
                string bool2 = "";
                bool1 = Globals.protectSymbol;
                bool2 = Globals.protectSymbol;

                string tt = "tx0";                

                Globals.itemHandler.Add(new EquationListItem(eqName3, " " /*counter2 + " of " + 17*/ , bool1, bool2, tt, Stringlist.GetListWithCommas(precedents, true), "Black", lineCounter == 1, eqName));
                
                //List<ModelGamsEquation> xx2 = Program.model.modelGams.equationsByEqname[eqName];

                if (firstText == null)
                {
                    string equationText = Program.model.modelGamsScalar.GetEquationTextUnfolded(helper.eqNumber, o.decompFind.decompOptions2.showTime, o.t0);
                    firstText = equationText;
                    firstEqName = eqName;
                    firstList.AddRange(precedents);
                }
            }


            string rv = null;
            WindowFind windowFind = new WindowFind(o);
            //eb.findOptions.decompOptions2 = decompOptions2;
            //eb.findOptions = o;
            windowFind.Title = variableName + " - " + "Gekko equations";
            windowFind.EquationBrowserSetButtons(firstEqName, firstList);
            windowFind.EquationBrowserSetLabel(variableName);
            windowFind._activeEquation = firstEqName;
            windowFind._activeVariable = null;

            windowFind.EquationBrowserSetEquation(firstEqName, o.decompFind.decompOptions2.showTime, o.t0);
            windowFind.decompFind.SetWindow(windowFind);
            
            bool? b = windowFind.ShowDialog();
            rv = windowFind._activeEquation;
            if (b != true) rv = null;  //only when OK is pressed (or Enter)
            windowFind.Close();

            return rv;
        }

        public enum ENormalizerType
        {
            None,
            Normalizer,
            NormalizerWithLagOrLead
        }
    }

    /// <summary>
    /// This is probably only for equation names
    /// </summary>
    public class DecompStartHelper
    {
        public string name = null; //the "x" in "x[a, b, <time>]"
        public string fullName = null; //the "x[a, b]" in "x[a, b, <time>]"
        public MultidimItem indexes = null; //the ["a", "b"] in "x[a, b, <time>]"
        public DecompStartHelperPeriod[] periods = null; //all the <time> periods found        
    }

    public class DecompStartHelperPeriod
    {
        public GekkoTime t = GekkoTime.tNull;
        public int eqNumber = -12345;
    }

    /// <summary>
    /// Simple helper class
    /// </summary>
    [ProtoContract]    
    public class PeriodAndVariable
    {
        [ProtoMember(1)]
        public int date;

        [ProtoMember(2)]
        public int variable;

        public PeriodAndVariable() //for protobuf
        {
        }

        public PeriodAndVariable(int timeIndex, int aNumber)
        {
            this.date = timeIndex;
            this.variable = aNumber;
        }

        /// <summary>
        /// Converts from ints into something understandable
        /// </summary>
        /// <returns></returns>
        public Tuple<string, GekkoTime> GetVariableAndPeriod()
        {
            string varName = Program.model.modelGamsScalar.GetVarNameA(this.variable);
            GekkoTime gt = Program.model.modelGamsScalar.FromTimeIntegerToGekkoTime(this.date);
            Tuple<string, GekkoTime> tup = new Tuple<string, GekkoTime>(varName, gt);
            return tup;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + this.date; //the 17 and 31 is a trick (primes) to get the hashcodes as distinct as possible.
            hash = hash * 31 + this.variable;
            return hash;
        }

        public override bool Equals(object obj)
        {                        
            PeriodAndVariable other = (PeriodAndVariable)obj;
            if (other == null) return false;
            if (this.date == other.date && this.variable == other.variable) return true;
            return false;
        }
    }
}
