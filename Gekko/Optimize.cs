using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gekko
{

    using System;
    //using alglib;

    class Optimize
    {
        static int N;
        static double[,] A;
        static double[,] W;
        static double[] rowTotals;
        static double[] colTotals;

        public class OptimizerOptions
        {
            public string type = "default"; // default | fast
            public double totalTolerance = 0.001;  //1 promille
            public bool treatNaNAs0 = true;
        }

        public static IVariable Ras1(GekkoTime t1, GekkoTime t2, IVariable input, IVariable rowSums, IVariable colSums, IVariable rowNames, IVariable colNames, IVariable constraints3, IVariable weights3, IVariable options)
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

            //Options
            OptimizerOptions o = new OptimizerOptions();            
            if (options.Type() != EVariableType.Map) new Error("Options should be stated as a map variable type");
            Map options_map = options as Map;
            //Type can be 'fast', 
            IVariable temp; if (options_map.storage.TryGetValue("%type", out temp)) { o.type = O.ConvertToString(temp); }

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

                double[,] xResult = Ras2(a_array, rowSums_array, colSums_array, rowNames_list, colNames_list, null, null, o);

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

        public static double[,] Ras2(double[,] a, double[] rowSums, double[] colSums, List<string> rowNames, List<string> colNames, IVariable constraints3, IVariable weights3, OptimizerOptions o)
        {
            //We could have strings like "[a,b] + [a,c] - 2*x[a,d] = 500". But maybe a more generic approach is better:
            //                           (('a','b'), ('a','c'), ('a', 'd', -2), 500),
            //                           (('b','a', 2), ('b','e'), 0)
            //
            //                           (('a','b', 2), ('b','e', 100)
            //            
            //            

            double[,] weights = null;
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
            if (Math.Abs(toti / totj - 1d) > o.totalTolerance) new Error("Rows sum to " + toti + ", whereas cols sum to " + totj + ". Tolerance " + o.totalTolerance + " exceeded");

            if (false)
            {
                List<IVariable> contraints2 = O.ConvertToList(constraints3);
                foreach (IVariable temp1 in contraints2)
                {
                    //(('a', 'b'), ('a', 'c'), ('a', 'd', -2), 500)
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
                            if (c != temp2.Count) new Error("Expected value to be last element");
                            double d = O.ConvertToVal(temp3);
                        }
                        else
                        {
                            List<IVariable> temp4 = O.ConvertToList(temp3);
                            if (temp4.Count == 2)
                            {
                                string s0 = O.ConvertToString(temp4[0]);
                                string s1 = O.ConvertToString(temp4[1]);
                                double d = 1d;
                            }
                            else if (temp4.Count == 3)
                            {
                                string s0 = O.ConvertToString(temp4[0]);
                                string s1 = O.ConvertToString(temp4[1]);
                                double d = O.ConvertToVal(temp4[2]);
                            }
                            else new Error("Expected list with 2 or 3 elements");
                        }

                    }
                }

                List<IVariable> weights2 = O.ConvertToList(weights3);
                foreach (IVariable temp1 in weights2)
                {
                    //('a', 'b', 2)
                    List<IVariable> temp2 = O.ConvertToList(temp1);
                    if (temp2.Count != 3) new Error("Expected 3 elements regarding constraint");
                    string s0 = O.ConvertToString(temp2[0]);
                    string s1 = O.ConvertToString(temp2[1]);
                    double d = O.ConvertToVal(temp2[2]);
                    //Handle
                }
            }

            int niPlusNj = ni + nj;
            int niMultiplyNj = ni * nj;

            double[,] constraints = new double[niPlusNj, niMultiplyNj + 1];

            // row constraints
            for (int i = 0; i < ni; i++)
            {
                for (int j = 0; j < nj; j++)
                {
                    constraints[i, i * nj + j] = 1;
                }
                constraints[i, niMultiplyNj] = rowSums[i];
            }

            // column constraints
            for (int j = 0; j < nj; j++)
            {
                for (int i = 0; i < ni; i++)
                {
                    constraints[ni + j, i * nj + j] = 1;
                }
                constraints[ni + j, niMultiplyNj] = colSums[j];
            }

            int[] ct = new int[niPlusNj];
            for (int i = 0; i < niPlusNj; i++)
            {
                ct[i] = 0; // equality
            }

            double[,] xResult = null;

            if (G.Equal(o.type, "fast"))
            {                
                xResult = RAS(a, rowSums, colSums, 1000, o.totalTolerance);
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

                // bounds (xij > 0)
                double[] bndl = new double[niMultiplyNj];
                double[] bndu = new double[niMultiplyNj];

                for (int i = 0; i < niMultiplyNj; i++)
                {
                    bndl[i] = 1e-6;
                    bndu[i] = double.PositiveInfinity;
                }

                double[] x = new double[ni * nj];

                alglib.minbleicstate state;
                alglib.minbleicreport rep;
                alglib.minbleiccreate(x1d, out state);
                alglib.minbleicsetbc(state, bndl, bndu);
                alglib.minbleicsetlc(state, constraints, ct);
                alglib.minbleicsetcond(state, 1e-10, 0, 0, 0);
                DateTime t2 = DateTime.Now;
                alglib.minbleicoptimize(state, F, null, null);
                alglib.minbleicresults(state, out x1d, out rep);
                G.Writeln2("Optimized " + ni + "x" + nj + " cells done " + G.Seconds(t2) + " iterations " + rep.iterationscount);

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
                        double lratio = Math.Log(xij / aij);
                        //What are the f values used for? Can their calculation be dropped or set constant??
                        if (weights == null)
                        {
                            //Without weights
                            f += xij * lratio;
                            g[k] = lratio + 1;
                        }
                        else
                        {
                            //With weights
                            f += weights[i, j] * xij * lratio;
                            g[k] = weights[i, j] * (lratio + 1);
                        }
                    }
                }
            }
        }


        public static void RAS()
        {            
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
            alglib.minbleicoptimize(state, FuncGrad, null, null);
            double[] x;
            alglib.minbleicresults(state, out x, out rep);
            G.Writeln("BLEIC " + N + "x" + N + " done " + G.Seconds(t2) + " iterations " + rep.iterationscount);
            G.Writeln(x[0] + "  " + x[1] + " " + x[2] + "  " + x[3]);

            if (N <= 10)
            {
                Print(x);
            }

            G.Writeln();
            DateTime t3 = DateTime.Now;
            double[,] y = RAS(A, rowTotals, colTotals, 1000, 1e-10);            
            G.Writeln("RAS " + N + "x" + N + " done " + G.Seconds(t3));
            G.Writeln(y[0, 0] + "  " + y[0, 1] + " " + y[0, 2] + "  " + y[0, 3]);
            if (N <= 10)
            {
                Print2(y);
            }

        }

        private static void Print2(double[,] x)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    G.Write($"{x[i, j]:F4} ");
                G.Writeln();
            }
        }

        private static void Print(double[] x)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    G.Write($"{x[i * N + j]:F4} ");
                G.Writeln();
            }
        }

        static double[,] RAS(double[,] A, double[] r, double[] c, int maxIter, double tol)
        {
            int n = A.GetLength(0);
            int m = A.GetLength(1);

            double[,] X = (double[,])A.Clone();

            for (int iter = 0; iter < maxIter; iter++)
            {
                // ---- Row scaling ----
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;

                    for (int j = 0; j < m; j++)
                        sum += X[i, j];

                    double factor = r[i] / sum;

                    for (int j = 0; j < m; j++)
                        X[i, j] *= factor;
                }

                // ---- Column scaling ----
                for (int j = 0; j < m; j++)
                {
                    double sum = 0;

                    for (int i = 0; i < n; i++)
                        sum += X[i, j];

                    double factor = c[j] / sum;

                    for (int i = 0; i < n; i++)
                        X[i, j] *= factor;
                }

                // ---- Convergence test ----
                double maxError = 0;

                // row errors
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;

                    for (int j = 0; j < m; j++)
                        sum += X[i, j];

                    maxError = Math.Max(maxError, Math.Abs(sum - r[i]));
                }

                // column errors
                for (int j = 0; j < m; j++)
                {
                    double sum = 0;

                    for (int i = 0; i < n; i++)
                        sum += X[i, j];

                    maxError = Math.Max(maxError, Math.Abs(sum - c[j]));
                }

                if (maxError < tol)
                {
                    G.Writeln($"Converged in {iter + 1} iterations");
                    break;
                }
            }

            return X;
        }

        static void FuncGrad(double[] x, ref double f, double[] g, object obj)
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

        static void FuncGradGRAS(double[] x, ref double f, double[] g, object obj)
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
}

