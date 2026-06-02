using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gekko
{

    using System;
    //using alglib;    

    public class OptimizerOptions
    {
        public EOptimizeType type = EOptimizeType.Ras; //default
        public double toleranceAbsolute = 0.0001d;  //absolute
        public bool treatNaNAs0 = false;
        public int rasGrasMaxIterations = int.MaxValue;
        public int rasGrasMinIterations = 0;
        public string hack = null;
        public double epsilon = 0.0001d; //for hack 
    }

    public enum EOptimizeType
    {
        Ras,
        Gras,
        Entropy,
        Entropy2003,
        SqDif,
        SqRel,
        DistDif,
        DistRel,
        
    }

    class Optimize
    {        

        public static IVariable Optimize1(GekkoTime t1, GekkoTime t2, IVariable input, IVariable rowSums, IVariable colSums, IVariable rowNames, IVariable colNames, IVariable other)
        {            
            Series adjusted = input.DeepClone(0, null, null) as Series;
            if (adjusted == null) new Error("Expected series input as first argument");
            if (adjusted.dimensions == 0) new Error("Expected array-series as first argument");
            List<string> rowNames_list = Stringlist.GetListOfStringsFromIVariable(rowNames);
            List<string> colNames_list = Stringlist.GetListOfStringsFromIVariable(colNames);
            Series rowSums_series = O.ConvertToSeries(rowSums) as Series;
            if (rowSums_series.dimensions == 0) new Error("Expected series with row sums to be array-series");
            Series colSums_series = O.ConvertToSeries(colSums) as Series;
            if (colSums_series.dimensions == 0) new Error("Expected series with column sums to be array-series");

            OptimizerOptions o = new OptimizerOptions();
            List constraints = null;
            List weights = null;
            List exo = null;
            string hack = null;

            if (other != null)
            {
                //Options                
                if (other.Type() == EVariableType.Map)
                {
                    IVariable temp = null;
                    Map options_map = other as Map;
                    //Type can be 'fast', 
                    if (options_map.storage.TryGetValue("%type", out temp))
                    {
                        string s = O.ConvertToString(temp);
                        if (G.Equal(s, "ras")) o.type = EOptimizeType.Ras;
                        else if (G.Equal(s, "gras")) o.type = EOptimizeType.Gras;
                        else if (G.Equal(s, "entropy")) o.type = EOptimizeType.Entropy;
                        else if (G.Equal(s, "entropy2003")) o.type = EOptimizeType.Entropy2003;
                        else if (G.Equal(s, "sqdif")) o.type = EOptimizeType.SqDif;
                        else if (G.Equal(s, "sqrel")) o.type = EOptimizeType.SqRel;
                        else if (G.Equal(s, "distdif")) o.type = EOptimizeType.DistDif;
                        else if (G.Equal(s, "distrel")) o.type = EOptimizeType.DistRel;
                        else new Error("Expected type 'default', 'entropy', 'sqdif', 'sqrel', 'distdif' or 'distrel'");
                    }

                    if (options_map.storage.TryGetValue("#constraints", out temp))
                    {
                        constraints = temp as List;
                    }

                    if (options_map.storage.TryGetValue("#weights", out temp))
                    {
                        weights = temp as List;
                    }

                    if (options_map.storage.TryGetValue("#exo", out temp))
                    {
                        exo = temp as List;
                    }

                    if (options_map.storage.TryGetValue("%hack", out temp))
                    {
                        o.hack = O.ConvertToString(temp);
                    }

                    if (options_map.storage.TryGetValue("%tol", out temp))
                    {
                        o.toleranceAbsolute = O.ConvertToVal(temp);
                    }

                    if (options_map.storage.TryGetValue("%itermin", out temp))
                    {
                        o.rasGrasMinIterations = O.ConvertToInt(temp);
                    }

                    if (options_map.storage.TryGetValue("%itermax", out temp))
                    {
                        o.rasGrasMaxIterations = O.ConvertToInt(temp);
                    }                    
                }
            }   

            foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
            {
                double[,] a_array = new double[rowNames_list.Count(), colNames_list.Count()];
                double[] rowSums_array = new double[rowNames_list.Count()];
                double[] colSums_array = new double[colNames_list.Count()];

                int ni = -1;
                int nj = -1;
                foreach (string si in rowNames_list)
                {
                    ni++;
                    nj = -1;
                    foreach (string sj in colNames_list)
                    {
                        nj++;
                        double d = (adjusted.dimensionsStorage.storage[new MultidimElement(new string[] { si, sj })] as Series).GetDataSimple(t);
                        a_array[ni, nj] = d;
                    }
                }

                ni = -1;
                foreach (string si in rowNames_list)
                {
                    ni++;
                    double d = (rowSums_series.dimensionsStorage.storage[new MultidimElement(new string[] { si })] as Series).GetDataSimple(t);
                    rowSums_array[ni] = d;
                }

                nj = -1;
                foreach (string sj in colNames_list)
                {
                    nj++;
                    double d = (colSums_series.dimensionsStorage.storage[new MultidimElement(new string[] { sj })] as Series).GetDataSimple(t);
                    colSums_array[nj] = d;
                }                

                double[,] xResult = Optimize2(a_array, rowSums_array, colSums_array, rowNames_list, colNames_list, constraints, weights, exo, t.ToString(), o);

                ni = -1;
                nj = -1;
                foreach (string si in rowNames_list)
                {
                    ni++;
                    nj = -1;
                    foreach (string sj in colNames_list)
                    {
                        nj++;
                        double d = xResult[ni, nj];
                        (adjusted.dimensionsStorage.storage[new MultidimElement(new string[] { si, sj })] as Series).SetData(t, d);
                    }
                }
            }            
            return adjusted;
        }

        public static double[,] Optimize2(double[,] a, double[] rowSums, double[] colSums, List<string> rowNames, List<string> colNames, IVariable constraints3, IVariable weights3, IVariable exo3, string period, OptimizerOptions o)
        {
            //We could have strings like "[a,b] + [a,c] - 2*x[a,d] = 500". But maybe a more generic approach is better:
            //                           (('a','b'), ('a','c'), ('a', 'd', -2), 500),
            //                           (('b','a', 2), ('b','e'), 0)
            //
            //                           (('a','b', 2), ('b','e', 100)
            //            
            //            

            int nWeights = 0;
            int nExo_OLD = 0;
            int nExo = 0;
            double[,] weights = null;
            //bool[,] exo = null;
            int ni = a.GetLength(0);
            int nj = a.GetLength(1);
            int nr = rowSums.Length; //rowTotals run over i
            int nc = colSums.Length; //colTotals run over j
            if (ni != nr) new Error("Cells have " + ni + " rows, row totals have " + nr + " rows");
            if (nj != nc) new Error("Cells have " + nj + " cols, col totals have " + nr + " cols");
            double toti = 0d; //sum of row sums
            double totj = 0d; //sum of col sums
            for (int i = 0; i < ni; i++)
            {
                if (o.treatNaNAs0 && G.IsNumericalError(rowSums[i])) rowSums[i] = 0d;
                toti += rowSums[i];
                for (int j = 0; j < nj; j++)
                {
                    if (o.treatNaNAs0 && G.IsNumericalError(a[i, j])) a[i, j] = 0d;
                    if (i == 0)
                    {
                        if (o.treatNaNAs0 && G.IsNumericalError(colSums[j])) colSums[j] = 0d;
                        totj += colSums[j];
                    }
                    //if (G.IsNumericalError(weights[i, j])) weights[i, j] = 1d;
                }
            }
            if (Math.Abs(toti - totj) > o.toleranceAbsolute) new Error("Rows sum to " + toti + ", whereas cols sum to " + totj + ". Tolerance " + o.toleranceAbsolute + " exceeded");

            int ONE = 1;
            int niPlusNj = ni + nj;
            int niPlusNjMinus1 = ni + nj - ONE; //dropping last constraing
            int niMultiplyNj = ni * nj;

            int nExtraConstraints = 0;
            List<double[]> storage1 = new List<double[]>();
            List<double> storage2 = new List<double>();
            List<double[]> storage1_exo = new List<double[]>();
            List<double> storage2_exo = new List<double>();

            if (G.Equal(o.hack, "hack1"))
            {
                for (int k = 0; k < niMultiplyNj; k++)
                {
                    int i = k / nj;
                    int j = k % nj;
                    //if (i == 20 && j == 21)
                    //{
                    //    //ignore
                    //}
                    //else
                    {                        
                        a[i, j] = Math.Max(o.epsilon, a[i, j]);
                    }
                }
            }

            double[] boundsFactor = G.CreateArrayDouble(niMultiplyNj, 1d); //only for 'ras' at the moment
            double[] boundsLower = new double[niMultiplyNj];
            double[] boundsUpper = new double[niMultiplyNj];
            for (int i = 0; i < ni; i++)
            {
                for (int j = 0; j < nj; j++)
                {
                    int k = i * nj + j;
                    double aij = a[i, j];
                    if (false && o.type == EOptimizeType.Entropy)
                    {
                        if (aij > 0)
                        {
                            boundsLower[k] = 1e-12;       // Must stay positive
                            boundsUpper[k] = double.PositiveInfinity;
                        }
                        else if (aij < 0)
                        {
                            boundsLower[k] = double.NegativeInfinity;
                            boundsUpper[k] = -1e-12;      // Must stay negative
                        }
                        else
                        {
                            boundsLower[k] = 0;           // Structural zero
                            boundsUpper[k] = 0;
                        }

                    }
                    else
                    {
                        boundsLower[k] = double.NegativeInfinity; // 1e-6; --> probably not much gain even if we say all cells must be positive
                        boundsUpper[k] = double.PositiveInfinity;
                    }
                }
            }

            if (exo3 != null)
            {
                //These may in principle be inconsistent regarding the more "normal" constraints.
                //Using #exo = (('a', 'b'),) amounts to #weights = (('a', 'b', 1), io[a, b][2020]),)
                //if we are exogenizing that cell. So exo notation is much easier for this.
                //exo = new bool[ni, nj];
                List<IVariable> exo2 = O.ConvertToList(exo3);
                foreach (IVariable temp1 in exo2)
                {
                    //('a', 'b')
                    //nExo_OLD++;
                    List<IVariable> temp2 = O.ConvertToList(temp1);
                    if (temp2.Count < 2 || temp2.Count > 3) 
                    {                    
                        new Error("Expected 2 or 3 elements regarding exo variables");
                    }
                    string s0 = O.ConvertToString(temp2[0]);
                    int i0 = rowNames.FindIndex(x => G.Equal(x, s0));
                    if (i0 < 0) new Error("Constraint: could not find '" + s0 + "' as row name");
                    string s1 = O.ConvertToString(temp2[1]);
                    int i1 = colNames.FindIndex(x => G.Equal(x, s1));
                    if (i1 < 0) new Error("Constraint: could not find '" + s1 + "' as col name");                    
                    int k = i0 * nj + i1;                    
                    boundsLower[k] = a[i0, i1];
                    boundsUpper[k] = a[i0, i1];
                    if (temp2.Count == 3)
                    {
                        if (o.type != EOptimizeType.Ras) new Error("#exo with factor only implemented for 'ras' at the moment");
                        double d = O.ConvertToVal(temp2[2]);
                        boundsFactor[k] = d;
                    }
                }
            }

            if (constraints3 != null)
            {
                List<IVariable> contraints2 = O.ConvertToList(constraints3);
                int counter = -1;
                foreach (IVariable temp1 in contraints2)
                {
                    //(('a', 'b'), ('a', 'c'), ('a', 'd', -2), 500)
                    counter++;
                    storage1.Add(new double[niMultiplyNj]);
                    storage2.Add(0d); //So if not indicated, is implicitly understood as 0!
                    List<IVariable> temp2 = O.ConvertToList(temp1);
                    int c = -1;
                    foreach (IVariable temp3 in temp2)
                    {
                        //('a', 'd', -2) or
                        //500
                        c++;
                        if (temp3.Type() == EVariableType.Val)
                        {
                            //check last
                            if (c != temp2.Count - 1) new Error("Expected value to be last element");
                            double d = O.ConvertToVal(temp3);
                            storage2[counter] = d;
                        }
                        else
                        {
                            List<IVariable> temp4 = O.ConvertToList(temp3);
                            string s0 = O.ConvertToString(temp4[0]);
                            int i0 = rowNames.IndexOf(s0);
                            if (i0 < 0) new Error("Constraint: could not find '" + s0 + "' as row name");
                            string s1 = O.ConvertToString(temp4[1]);
                            int i1 = colNames.IndexOf(s1);
                            if (i1 < 0) new Error("Constraint: could not find '" + s1 + "' as col name");
                            double d = 1d; //coefficient
                            if (temp4.Count == 2)
                            {                                
                                //ok
                            }
                            else if (temp4.Count == 3)
                            {                                
                                d = O.ConvertToVal(temp4[2]);
                            }
                            else new Error("Expected list with 2 or 3 elements");
                            storage1[counter][i0 * nj + i1] = d;
                        }
                    }                    
                }
                nExtraConstraints = counter + 1;
            }            

            if (weights3 != null)
            {
                weights = new double[ni, nj];
                for (int i = 0; i < ni; i++)
                {
                    for (int j = 0; j < nj; j++)
                    {
                        weights[i, j] = 1d; //default
                    }
                }
                List<IVariable> weights2 = O.ConvertToList(weights3);
                foreach (IVariable temp1 in weights2)
                {
                    //('a', 'b', 2)
                    nWeights++;
                    List<IVariable> temp2 = O.ConvertToList(temp1);
                    if (temp2.Count != 3) new Error("Expected 3 elements regarding constraint");
                    string s0 = O.ConvertToString(temp2[0]);
                    int i0 = rowNames.FindIndex(x => G.Equal(x, s0));
                    if (i0 < 0) new Error("Constraint: could not find '" + s0 + "' as row name");                    
                    string s1 = O.ConvertToString(temp2[1]);                    
                    int i1 = colNames.FindIndex(x => G.Equal(x, s1));
                    if (i1 < 0) new Error("Constraint: could not find '" + s1 + "' as col name");                    
                    double d = O.ConvertToVal(temp2[2]);
                    weights[i0, i1] = d;
                }
            }

            double[,] constraints = new double[niPlusNjMinus1 + nExtraConstraints + nExo_OLD, niMultiplyNj + 1]; // +1 because it is a column wherein to put constants (if x[a,a]+x[a,b]=100, we put the 100 there)

            //Normal constraints
            for (int i = 0; i < nExtraConstraints; i++)
            {
                for (int j = 0; j < storage1[i].Length; j++) 
                {
                    constraints[niPlusNjMinus1 + i, j] = storage1[i][j];
                }
                constraints[niPlusNjMinus1 + i, niMultiplyNj] = storage2[i];  //The constant column that is last
            }
            
            // row constraints
            for (int i = 0; i < ni; i++)
            {
                for (int j = 0; j < nj; j++) //dropping last col
                {
                    constraints[i, i * nj + j] = 1;
                }
                constraints[i, niMultiplyNj] = rowSums[i];
            }            

            // column constraints
            for (int j = 0; j < nj - ONE; j++)
            {
                for (int i = 0; i < ni; i++)  //dropping last one
                {
                    constraints[ni + j, i * nj + j] = 1;
                }
                constraints[ni + j, niMultiplyNj] = colSums[j];
            }

            int[] constraintsType = new int[niPlusNjMinus1 + nExtraConstraints + nExo_OLD];
            for (int i = 0; i < niPlusNjMinus1; i++)
            {
                constraintsType[i] = 0; //equality
            }

            for (int i = 0; i < nExtraConstraints; i++)
            {
                constraintsType[niPlusNjMinus1 + i] = 0; //equality
            }

            for (int i = 0; i < nExo_OLD; i++)
            {
                constraintsType[niPlusNjMinus1 + nExtraConstraints + i] = 0; //equality
            }

            double[,] xResult = null;

            if (boundsLower != null && boundsUpper != null)
            {
                if (G.Equal(o.hack, "hack1"))
                {
                    for (int k = 0; k < niMultiplyNj; k++)
                    {
                        int i = k / nj;
                        int j = k % nj;
                        if (G.IsNumericalError(boundsLower[k]))
                        {
                            boundsLower[k] = o.epsilon;
                        }
                    }
                }
            }

            if (o.type == EOptimizeType.Ras)
            {
                if (nExtraConstraints > 0) new Error("You cannot use constraints with RAS (but #exo is possible)");
                if (nWeights > 0) new Error("You cannot use weights with RAS (but #exo is possible)");
                DateTime t3 = DateTime.Now;
                int iterations; double error;
                xResult = RAS(a, rowSums, colSums, boundsLower, boundsUpper, boundsFactor, o.rasGrasMinIterations - 1, o.rasGrasMaxIterations - 1, o.toleranceAbsolute, out iterations, out error);
                string sExtra = null;
                if (nExo_OLD > 0) sExtra = " with " + nWeights + " constraints" + G.S(nExo_OLD);
                if (iterations == -1) new Error("Optimization " + period + " (" + o.type + ") failed on " + ni + "x" + nj + " cells" + sExtra + " using " + o.rasGrasMaxIterations + " iteration" + G.S(iterations) + " with error " + error.ToString("G8") + " in " + G.Seconds(t3));
                G.Writeln2("Optimized " + period + " (" + o.type + ") " + ni + "x" + nj + " cells" + sExtra + " using " + iterations + " iteration" + G.S(iterations) + " with error " + error.ToString("G8") + " in " + G.Seconds(t3));
            }
            else if (o.type == EOptimizeType.Gras)
            {
                if (nExtraConstraints > 0) new Error("You cannot use constraints with GRAS (but #exo is possible)");
                if (nWeights > 0) new Error("You cannot use weights with GRAS (but #exo is possible)");
                DateTime t3 = DateTime.Now;
                int iterations; double error;
                xResult = GRAS(a, rowSums, colSums, boundsLower, boundsUpper, boundsFactor, o.rasGrasMinIterations - 1, o.rasGrasMaxIterations - 1, o.toleranceAbsolute, out iterations, out error);
                string sExtra = null;
                if (nExo_OLD > 0) sExtra = " with " + nWeights + " constraints" + G.S(nExo_OLD);
                if (iterations == -1) new Error("Optimization " + period + " (" + o.type + ") failed on " + ni + "x" + nj + " cells" + sExtra + " using " + o.rasGrasMaxIterations + " iteration" + G.S(iterations) + " with error " + error.ToString("G8") + " in " + G.Seconds(t3));
                G.Writeln2("Optimized " + period + " (" + o.type + ") " + ni + "x" + nj + " cells" + sExtra + " using " + iterations + " iteration" + G.S(iterations) + " with error " + error.ToString("G8") + " in " + G.Seconds(t3));
            }
            else
            {
                double[] x1d = new double[niMultiplyNj];
                for (int i = 0; i < ni; i++)
                {
                    for (int j = 0; j < nj; j++)
                    {
                        x1d[i * nj + j] = a[i, j];
                    }
                }

                alglib.minbleicstate state;
                alglib.minbleicreport rep;
                alglib.minbleiccreate(x1d, out state);
                alglib.minbleicsetbc(state, boundsLower, boundsUpper); //limits, similar to GAMS lower and upper.
                alglib.minbleicsetlc(state, constraints, constraintsType); //0 means exact, <= or >= are possible.
                alglib.minbleicsetcond(state, 1e-10, 0, 0, 0);
                DateTime t2 = DateTime.Now;
                alglib.minbleicoptimize(state, F, null, null);
                alglib.minbleicresults(state, out x1d, out rep);

                string sExtra = null;
                if (nExtraConstraints > 0 && nWeights == 0) sExtra = " with " + nExtraConstraints + " constraint" + G.S(nExtraConstraints);
                else if (nExtraConstraints == 0 && nWeights > 0) sExtra = " with " + nWeights + " weight" + G.S(nWeights);
                else if (nExtraConstraints > 0 && nWeights > 0) sExtra = " with " + nExtraConstraints + " constraint" + G.S(nExtraConstraints) + " and " + nWeights + " weight" + G.S(nWeights);

                string s = null;
                if (rep.terminationtype == -7) s = "Gradient verification failed. See MinBLEICSetGradientCheck() for more information";
                else if (rep.terminationtype == -3) s = "Inconsistent constraints. Feasible point is either nonexistent or too hard to find. Try to restart optimizer with better initial approximation";
                else if (rep.terminationtype == 1) s = "Relative function improvement is no more than EpsF";
                else if (rep.terminationtype == 2) s = "Relative step is no more than EpsX";
                else if (rep.terminationtype == 4) s = "Gradient norm is no more than EpsG";
                else if (rep.terminationtype == 5) s = "MaxIts steps was taken";
                else if (rep.terminationtype == 7) s = "Stopping conditions are too stringent, further improvement is impossible";

                //7 is probably bad...
                if (rep.terminationtype == 1 || rep.terminationtype == 2 || rep.terminationtype == 4 || rep.terminationtype == 7)
                {
                    G.Writeln2("Optimized " + period + " (" + o.type + ") " + ni + "x" + nj + " cells" + sExtra + " using " + rep.iterationscount + " iteration" + G.S(rep.iterationscount) + " in " + G.Seconds(t2) + ", termination type " + rep.terminationtype + ".");
                }
                else
                {
                    string s2 = null;
                    if (s != null) s2 = "Solver message: " + s;
                    new Error("Optimization " + period + " (" + o.type + ") failed on " + ni + "x" + nj + " cells" + sExtra + " using " + rep.iterationscount + " iteration" + G.S(rep.iterationscount) + " in " + G.Seconds(t2) + ". " + s2 + ". Termination type " + rep.terminationtype + ".");
                }

                xResult = new double[ni, nj];
                for (int k = 0; k < x1d.Length; k++)
                {
                    int i = k / nj;
                    int j = k % nj;
                    xResult[i, j] = x1d[k];
                }
            }

            return xResult;

            void F(double[] x, ref double f, double[] g, object obj)
            {
                f = 0;
                for (int i = 0; i < ni; i++)
                {
                    for (int j = 0; j < nj; j++)
                    {
                        int k = i * nj + j;
                        double xij = x[k];
                        double aij = a[i, j];
                        double ratio = xij / aij;

                        if (aij == 0)
                        {
                            g[k] = 0;
                            continue;
                        }

                        if (ratio <= 0)
                        {
                            ratio = 1e-15; //Solver may have been overshooting beyound plus minus boundary
                        }

                        double lratio = Math.Log(ratio);

                        if (o.type == EOptimizeType.Entropy)
                        {
                            //2013 paper (note)
                            //Seems to give the same as RAS, and the same as (false), for positive cells.
                            if (weights == null)
                            {
                                //Temurshoev, Miller, and Bouwmeester (2013): "A Note on the GRAS Method"
                                //abs(xij) * log(xij/aij) --> abs(aij) * (xij/aij log(xij/aij) - xij/aij + 1)
                                //So for aij > 0 it reduces to:                                    
                                //xij log(xij / aij) - xij + aij
                                //The last 2 terms tend to cancel out in RAS procedure.                                    
                                f += Math.Abs(aij) * Theta(ratio);
                                g[k] = Math.Sign(aij) * Thetadiff(ratio);
                            }
                            else
                            {
                                f += weights[i, j] * Theta(ratio);
                                g[k] = weights[i, j] * Thetadiff(ratio);
                            }
                        }
                        else if (o.type == EOptimizeType.Entropy2003)
                        {
                            //2003 paper
                            if (weights == null)
                            {
                                //Without weights
                                f += Math.Abs(xij) * lratio; //Note: it sees taking abs(xij) is the GRAS modification. Regarding log, cells should never be able to cross the plus/minus boundary.
                                g[k] = Math.Sign(xij) * (lratio + 1d);
                            }
                            else
                            {
                                //With weights. Note: weights are always > 0.
                                f += weights[i, j] * Math.Abs(xij) * lratio;
                                g[k] = weights[i, j] * Math.Sign(xij) * (lratio + 1d);
                            }
                        }
                        else if (o.type == EOptimizeType.SqDif)
                        {
                            double dif = xij - aij;
                            if (weights == null)
                            {
                                f += dif * dif;
                                g[k] = 2d * dif;
                            }
                            else
                            {
                                f += weights[i, j] * dif * dif;
                                g[k] = 2d * weights[i, j] * dif;
                            }
                        }
                        else if (o.type == EOptimizeType.SqRel)
                        {
                            double diff = xij - aij;
                            double a2 = aij * aij;

                            if (weights == null)
                            {
                                f += (diff * diff) / a2;
                                g[k] = (2 * diff) / a2;
                            }
                            else
                            {
                                f += weights[i, j] * (diff * diff) / a2;
                                g[k] = weights[i, j] * (2 * diff) / a2;
                            }
                        }
                        else if (o.type == EOptimizeType.DistDif)
                        {
                            double dif = xij - aij;
                            if (weights == null)
                            {
                                f += Math.Abs(dif);
                                if (dif == 0d) g[k] = 0d; //hack
                                else g[k] = Math.Sign(dif);
                            }
                            else
                            {
                                f += weights[i, j] * Math.Abs(dif);
                                if (dif == 0d) g[k] = 0d; //hack
                                else g[k] = weights[i, j] * Math.Sign(dif);
                            }
                        }
                        else if (o.type == EOptimizeType.DistRel)
                        {
                            double dif = xij - aij;
                            double absA = Math.Abs(aij);
                            if (weights == null)
                            {
                                if (absA != 0d)
                                {
                                    f += Math.Abs(dif) / absA;
                                    g[k] = Math.Sign(dif) / absA;
                                }
                                else
                                {
                                    g[k] = 0d; //hack
                                }
                            }
                            else
                            {
                                if (absA != 0d)
                                {
                                    f += weights[i, j] * Math.Abs(dif) / absA;
                                    g[k] = weights[i, j] * Math.Sign(dif) / absA;
                                }
                                else
                                {
                                    g[k] = 0d; //hack
                                }
                            }
                        }
                    }
                }
            }
        }

        public static double Theta(double x)
        {
            return x * Math.Log(x) - x + 1;
        }

        public static double Thetadiff(double x)
        {
            return Math.Log(x);
        }

        public static void RAS()
        {
            int N;
            double[,] A;
            double[,] W;
            double[] rowTotals;
            double[] colTotals;

            N = 30; // size of the matrix
            double minValue = 50.0;
            double maxValue = 100.0;
            double pert = 0.10d; //0.10 is 10%

            Random rnd = new Random();

            // 1. Generate random positive NxN input matrix
            A = new double[N, N];
            W = new double[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    A[i, j] = minValue + (maxValue - minValue) * rnd.NextDouble();
                    W[i, j] = 1d;
                }
            }

            // 2. Generate random row and column totals
            rowTotals = new double[N];
            colTotals = new double[N];
            for (int i = 0; i < N; i++)
            {
                double rowSum = 0;
                for (int j = 0; j < N; j++)
                    rowSum += A[i, j];

                // Apply small relative perturbation (±5%)
                double perturb = pert * rowSum;
                rowTotals[i] = rowSum + (2 * rnd.NextDouble() - 1) * perturb;
            }

            for (int j = 0; j < N; j++)
            {
                double colSum = 0;
                for (int i = 0; i < N; i++)
                    colSum += A[i, j];

                // Apply small relative perturbation (±5%)
                double perturb = pert * colSum;
                colTotals[j] = colSum + (2 * rnd.NextDouble() - 1) * perturb;
            }

            // 3. Adjust column totals to match total sum of rowTotals
            double totalRows = 0, totalCols = 0;
            for (int i = 0; i < N; i++) totalRows += rowTotals[i];
            for (int j = 0; j < N; j++) totalCols += colTotals[j];
            double scale = totalRows / totalCols;
            for (int j = 0; j < N; j++)
                colTotals[j] *= scale;


            if (false)
            {

                A = new double[,]  {
        {10,20,30},
        {20,10,40},
        {30,40,10}
        };

                W = new double[,]
                {
        {1,1,1},
        {1,1,1},
        {1,1,1}
            };

                rowTotals = new double[] { 80, 70, 90 };
                colTotals = new double[] { 90, 80, 70 };                
            }

            G.Writeln("Input matrix A:");
            if (N <= 10)
            {
                Print2(A);
            }
            G.Writeln();



            int vars = N * N;

            double[] x0 = new double[vars];

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    x0[i * N + j] = A[i, j];

            // bounds (xij > 0)
            double[] bndl = new double[vars];
            double[] bndu = new double[vars];

            for (int i = 0; i < vars; i++)
            {
                bndl[i] = 1e-6;
                bndu[i] = double.PositiveInfinity;
            }

            int extra = 0;

            // linear constraints
            int constraints = 2 * N + extra;

            double[,] C = new double[constraints, vars + 1];

            // row constraints
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    C[i, i * N + j] = 1;

                C[i, vars] = rowTotals[i];
            }

            // column constraints
            for (int j = 0; j < N; j++)
            {
                for (int i = 0; i < N; i++)
                    C[N + j, i * N + j] = 1;

                C[N + j, vars] = colTotals[j];
            }

            int[] ct = new int[constraints];
            for (int i = 0; i < constraints; i++)
                ct[i] = 0; // equality

            if (extra == 1)
            {
                // --- extra constraint: x11 + x12 = x31 + x33 ---
                // vectorized indices: x11=0, x12=1, x31=6, x33=8
                C[2 * N, 0] = 1;  // x11
                C[2 * N, 1] = 1;  // x12
                C[2 * N, 6] = -1; // x31
                C[2 * N, 8] = -1; // x33
                C[2 * N, vars] = 0; // right-hand side
            }

            alglib.minbleicstate state;
            alglib.minbleicreport rep;

            alglib.minbleiccreate(x0, out state);

            alglib.minbleicsetbc(state, bndl, bndu);
            alglib.minbleicsetlc(state, C, ct);
            alglib.minbleicsetcond(state, 1e-10, 0, 0, 0);

            DateTime t2 = DateTime.Now;
            alglib.minbleicoptimize(state, TestF, null, null);
            double[] x;
            alglib.minbleicresults(state, out x, out rep);
            G.Writeln("BLEIC " + N + "x" + N + " done " + G.Seconds(t2) + " iterations " + rep.iterationscount);
            G.Writeln(x[0] + "  " + x[1] + " " + x[2] + "  " + x[3]);

            if (N <= 10)
            {
                Print(x, N);
            }

            G.Writeln();
            DateTime t3 = DateTime.Now;
            int iterations = -1; double error = double.NaN;
            double[,] y = RAS(A, rowTotals, colTotals, null, null, null, 0, 1000, 1e-10, out iterations, out error);
            G.Writeln("RAS " + N + "x" + N + " done " + G.Seconds(t3) + " in " + iterations + " iterations");
            G.Writeln(y[0, 0] + "  " + y[0, 1] + " " + y[0, 2] + "  " + y[0, 3]);
            if (N <= 10)
            {
                Print2(y);
            }

            void TestF(double[] x, ref double f, double[] g, object obj)
            {
                f = 0;
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        int k = i * N + j;
                        double xij = x[k];
                        double aij = A[i, j];
                        double ratio = xij / aij;
                        f += xij * Math.Log(ratio);
                        g[k] = Math.Log(ratio) + 1;
                    }
                }
            }

            void TestFSquared(double[] x, ref double f, double[] g, object obj)
            {
                //int N = ...; // matrix size
                //double[,] A = ...; // input
                //double[,] W = ...; // weights

                f = 0;
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                    {
                        int k = i * N + j;
                        double diff = x[k] - A[i, j];
                        f += diff * diff / (2 * W[i, j]);
                        g[k] = diff / W[i, j];
                    }
            }

        }

        private static void Print2(double[,] x)
        {
            for (int i = 0; i < x.GetLength(0); i++)
            {
                for (int j = 0; j < x.GetLength(1); j++)
                    G.Write($"{x[i, j]:F4} ");
                G.Writeln();
            }
        }

        private static void Print(double[] x, int N)
        {
            for (int i = 0; i < x.Length; i++)
            {
                for (int j = 0; j < x.Length; j++)
                    G.Write($"{x[i * x.Length + j]:F4} ");
                G.Writeln();
            }
        }

        static double[,] RAS(double[,] a, double[] r, double[] c, double[] boundsLower, double[] boundsUpper, double[] boundsFactor, int iterMin, int iterMax, double tol, out int iterations, out double max)
        {
            iterations = -1; //signals failure
            int ni = a.GetLength(0);
            int nj = a.GetLength(1);

            double[] r2 = (double[])r.Clone();
            double[] c2 = (double[])c.Clone();
            double[,] x = (double[,])a.Clone();

            double[,] exo = Bounds2Exo(boundsLower, boundsUpper, boundsFactor, ni, nj);
            ExoRemove(true, x, r2, c2, exo, ni, nj);

            max = double.NaN;
            for (int iter = 0; iter < iterMax; iter++)
            {
                RasScaleRows(x, r2, ni, nj);
                RasScaleCols(x, c2, ni, nj);
                max = RasErrors(x, r2, c2, ni, nj);
                if (max < tol && iter >= iterMin)
                {
                    iterations = iter + 1;
                    break;
                }
            }

            ExoRemove(false, x, null, null, exo, ni, nj);

            return x;
        }

        static double[,] GRAS(double[,] a, double[] r3, double[] c3, double[] boundsLower, double[] boundsUpper, double[] boundsFactor, int iterMin, int iterMax, double tol, out int iterations, out double error)
        {
            iterations = -1; //signals failure
            int ni = a.GetLength(0);
            int nj = a.GetLength(1);

            double[] rowTarget = (double[])r3.Clone();
            double[] colTarget = (double[])c3.Clone();
            double[,] x0 = (double[,])a.Clone();

            double[,] exo = Bounds2Exo(boundsLower, boundsUpper, boundsFactor, ni, nj);
            ExoRemove(true, x0, rowTarget, colTarget, exo, ni, nj);

            int nRows = x0.GetLength(0);
            int nCols = x0.GetLength(1);

            // Split into positive and negative components
            double[,] positive = new double[nRows, nCols]; //All positive values, are never changed after construction
            double[,] negative = new double[nRows, nCols]; //All negative values, are never changed after construction
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    if (x0[i, j] >= 0) positive[i, j] = x0[i, j];
                    else negative[i, j] = Math.Abs(x0[i, j]);
                }
            }

            //Initial setup
            double[] row1 = new double[nRows];
            for (int i = 0; i < nRows; i++) row1[i] = 1.0;
            double[] col1 = new double[nCols];
            double[] col2 = new double[nCols];
            col1 = ScaleRowOrColumn(positive, negative, row1, colTarget, false); //col update
            row1 = ScaleRowOrColumn(positive, negative, col1, rowTarget, true); //row udate
            col2 = ScaleRowOrColumn(positive, negative, row1, colTarget, false); //col update
                        
            error = GRASError(nCols, col1, col2);

            bool converged = false;
            int iter;
            for (iter = 1; iter < iterMax; iter++)
            {
                if (iter >= iterMin && error <= tol)
                {
                    converged = true;
                    break;
                }
                Array.Copy(col2, col1, nCols);
                //Scale row
                row1 = ScaleRowOrColumn(positive, negative, col1, rowTarget, true);
                //Scale column
                col2 = ScaleRowOrColumn(positive, negative, row1, colTarget, false);
                error = GRASError(nCols, col1, col2);                
            }

            if (!converged)
            {
                //Note: Max iterations reached without convergence
            }

            // --- Final Matrix Construction ---
            double[,] x = new double[nRows, nCols];
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    double pos_part = row1[i] * positive[i, j] * col2[j];
                    double inv_r = row1[i] == 0 ? 1.0 : row1[i];
                    double inv_s = col2[j] == 0 ? 1.0 : col2[j];
                    double neg_part = negative[i, j] / (inv_r * inv_s);
                    x[i, j] = pos_part - neg_part;
                }
            }

            ExoRemove(false, x, null, null, exo, ni, nj);
            iterations = iter;
            return x;
        }

        private static double GRASError(int nn, double[] s1, double[] s2)
        {
            double error = 0;
            for (int j = 0; j < nn; j++) error = Math.Max(error, Math.Abs(s2[j] - s1[j]));
            return error;
        }

        /// <summary>
        /// Findes the factor (solving a second order equation if the signs diverge) that makes the
        /// row or column match its target.
        /// </summary>
        private static double[] ScaleRowOrColumn(double[,] positive, double[,] negative, double[] rowOrColumn, double[] target, bool rowMode)
        {            
            int ni = rowMode ? positive.GetLength(0) : positive.GetLength(1);
            int nj = rowMode ? positive.GetLength(1) : positive.GetLength(0);
            double[] result = new double[ni]; //a little bit wasteful
            
            double[] invV = new double[nj]; //a little bit wasteful
            for (int j = 0; j < nj; j++) invV[j] = 1.0 / rowOrColumn[j]; //faster to reuse the division later on in hot loop           

            for (int i = 0; i < ni; i++)
            {
                double positiveSum = 0;
                double negativeSum = 0;                

                for (int j = 0; j < nj; j++)
                {                    
                    int row = rowMode ? i : j;
                    int col = rowMode ? j : i;
                    double v = rowOrColumn[j];
                    double vInverted = v == 0 ? 1.0 : invV[j]; //1 is arbitray here
                    positiveSum += positive[row, col] * v;
                    negativeSum += negative[row, col] * vInverted;
                }

                double targetI = target[i];                
                if (positiveSum != 0)
                {
                    if (negativeSum != 0d)
                    {
                        //positiveSum != 0, negativeSum != 0 (both positives and negatives)
                        result[i] = (targetI + Math.Sqrt(targetI * targetI + 4 * positiveSum * negativeSum)) / (2 * positiveSum);
                    }
                    else
                    {
                        //positiveSum != 0, negativeSum == 0 (no negatives)
                        if (targetI <= 0d) result[i] = 0d; 
                        else result[i] = targetI / positiveSum;
                    }
                }
                else
                {
                    //positiveSum == 0, negativeSum != 0 (no positives, possibly all zeros)
                    //positiveSum == 0, negativeSum == 0
                    if (targetI == 0d)
                    {
                        if (positiveSum == 0 && negativeSum == 0) result[i] = 1d; // neutral scaling
                        else result[i] = 0d; // or small epsilon
                    }
                    else
                    {
                        result[i] = -negativeSum / targetI;
                    }
                }
            }
            return result;
        }

        private static double RasErrors(double[,] x, double[] r2, double[] c2, int ni, int nj)
        {
            // ---- Convergence test ----
            double max = 0;

            // Note: For checking error, we must mentally "add back" the fixed cells
            // which is equivalent to checking active sum against adjR/adjC
            for (int i = 0; i < ni; i++)
            {
                double sum = 0;
                for (int j = 0; j < nj; j++) sum += x[i, j];
                max = Math.Max(max, Math.Abs(sum - r2[i]));
            }

            for (int j = 0; j < nj; j++)
            {
                double sum = 0;
                for (int i = 0; i < ni; i++) sum += x[i, j];
                max = Math.Max(max, Math.Abs(sum - c2[j]));
            }

            return max;
        }

        private static void RasScaleCols(double[,] x, double[] c2, int ni, int nj)
        {
            // ---- Column scaling ----
            for (int j = 0; j < nj; j++)
            {
                double sum = 0;
                for (int i = 0; i < ni; i++) sum += x[i, j];

                if (sum > 0)
                {
                    double factor = c2[j] / sum;
                    for (int i = 0; i < ni; i++) x[i, j] *= factor;
                }
            }
        }

        private static void RasScaleRows(double[,] x, double[] r2, int ni, int nj)
        {
            // ---- Row scaling ----
            for (int i = 0; i < ni; i++)
            {
                double sum = 0;
                for (int j = 0; j < nj; j++) sum += x[i, j];

                if (sum > 0) // Avoid division by zero if all cells in row are fixed/zero
                {
                    double factor = r2[i] / sum;
                    for (int j = 0; j < nj; j++) x[i, j] *= factor;
                }
            }
        }

        private static void ExoRemove2(int ni, int nj, double[,] x, double[,] exo)
        {
            if (exo != null)
            {
                // Restore fixed cells
                for (int i = 0; i < ni; i++)
                {
                    for (int j = 0; j < nj; j++)
                    {
                        if (!G.IsNumericalError(exo[i, j]))
                        {
                            x[i, j] = exo[i, j];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes inside x (setting to 0) and restores exogenized cells, while adjusting row and col sums
        /// </summary>
        private static void ExoRemove(bool remove, double[,] x, double[] adjR, double[] adjC, double[,] exo, int ni, int nj)
        {            
            if (exo != null)
            {
                if (remove)
                {
                    for (int i = 0; i < ni; i++)
                    {
                        for (int j = 0; j < nj; j++)
                        {
                            if (!G.IsNumericalError(exo[i, j]))
                            {
                                adjR[i] -= exo[i, j];
                                adjC[j] -= exo[i, j];
                                x[i, j] -= exo[i, j]; // Temporarily 0, if factor is 1
                            }
                        }
                    }
                }
                else
                {
                    // Restore fixed cells
                    for (int i = 0; i < ni; i++)
                    {
                        for (int j = 0; j < nj; j++)
                        {
                            if (!G.IsNumericalError(exo[i, j]))
                            {
                                x[i, j] += exo[i, j];
                            }
                        }
                    }
                }
            }
        }

        private static double[,] Bounds2Exo(double[] boundsLower, double[] boundsUpper, double[] boundsFactor, int ni, int nj)
        {
            double[,] exo = null;

            if (boundsLower != null && boundsUpper != null && boundsFactor != null)
            {
                exo = new double[ni, nj];
                if (boundsLower.Length != boundsUpper.Length) new Error("Bounds upper/lower sizes do not match");
                if (boundsLower.Length != boundsFactor.Length) new Error("Bounds upper/lower/factor sizes do not match");
                for (int k = 0; k < boundsLower.Length; k++)
                {
                    int i = k / nj;
                    int j = k % nj;
                    exo[i, j] = double.NaN;
                    if (G.IsNumericalError(boundsLower[k]) || G.IsNumericalError(boundsUpper[k]))
                    {
                        //do nothing
                    }
                    else
                    {
                        double factor = boundsFactor[k]; //1d per default                        
                        if (G.IsNumericalError(factor)) new Error("Bounds factor with missing value");
                        if (boundsLower[k] == boundsUpper[k])
                        {
                            exo[i, j] = factor * boundsLower[k];
                        }
                        else new Error("Bounds upper/lower are different");
                    }
                }
            }

            return exo;
        }

    }
}

