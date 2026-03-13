using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gekko
{

    using System;
    //using alglib;

    class Program2
    {
        static int N;
        static double[,] A;
        static double[,] W;
        static double[] rowTotals;
        static double[] colTotals;

        public static void RAS()
        {

            N = 3; // size of the matrix
            double minValue = 1.0;
            double maxValue = 100.0;

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
                rowTotals[i] = N * minValue + (N * maxValue - N * minValue) * rnd.NextDouble();

            for (int j = 0; j < N; j++)
                colTotals[j] = N * minValue + (N * maxValue - N * minValue) * rnd.NextDouble();



            A =      new double[,]  {
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


            G.Writeln("Input matrix A:");
            Print2(A);
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

            alglib.minbleicoptimize(state, FuncGrad, null, null);

            double[] x;
            alglib.minbleicresults(state, out x, out rep);

            Print(x);

            G.Writeln();
            G.Writeln("RAS");
            double[,] y = RAS(A, rowTotals, colTotals, 1000, 1e-10);
            Print2(y);

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

