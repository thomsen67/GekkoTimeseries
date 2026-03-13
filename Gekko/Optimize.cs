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
        const int N = 3;

        static double[,] A =
        {
        {10,20,30},
        {20,10,40},
        {30,40,10}
        };

        static double[,] W =
        {
        {1,1,1},
        {1,1,1},
        {1,1,1}
        };

        static double[] rowTarget = { 80, 70, 90 };
        static double[] colTarget = { 90, 80, 70 };

        public static void RAS()
        {
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

            int extra = 1;

            // linear constraints
            int constraints = 2 * N + extra;

            double[,] C = new double[constraints, vars + 1];

            // row constraints
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    C[i, i * N + j] = 1;

                C[i, vars] = rowTarget[i];
            }

            // column constraints
            for (int j = 0; j < N; j++)
            {
                for (int i = 0; i < N; i++)
                    C[N + j, i * N + j] = 1;

                C[N + j, vars] = colTarget[j];
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

            Console.WriteLine("Balanced matrix:");

            Print(x);

            G.Writeln();

            double[,] y = RAS(A, rowTarget, colTarget, 1000, 1e-10);
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

    public class Optimize
    {
        // Initial Matrix (A) - Flattened
        private static readonly double[] _a = { 10.0, 2.0, 1.0,
                                            1.0, 15.0, 3.0,
                                            0.5, 1.0, 20.0 };

        public static void RAS()
        {
            // Target Sums (Constraints)
            double[] rowSums = { 20.0, 30.0, 15.0 };
            double[] colSums = { 18.0, 25.0, 22.0 };

            // 1. Setup Initial Guess (x0)
            // We start at 'a'. Note: We add a tiny epsilon to ensure no exact 0s hit the Log
            double[] x0 = (double[])_a.Clone();
            for (int i = 0; i < x0.Length; i++) if (x0[i] == 0) x0[i] = 1e-9;

            // 2. Initialize BLEIC Solver (9 variables)
            alglib.minbleicstate state;
            alglib.minbleiccreate(x0, out state);

            // 3. Set Linear Equality Constraints (Ax = B)
            // Format: [coeff_x0, coeff_x1... coeff_x8, target_value]
            // Constraint type 1 is Equality (=)

            // Row Sums
            //alglib.minbleicsetlc(state, new double[,] { { 1, 1, 1, 0, 0, 0, 0, 0, 0, rowSums[0] } }, 1);
            //alglib.minbleicsetlc(state, new double[,] { { 0, 0, 0, 1, 1, 1, 0, 0, 0, rowSums[1] } }, 1);
            //alglib.minbleicsetlc(state, new double[,] { { 0, 0, 0, 0, 0, 0, 1, 1, 1, rowSums[2] } }, 1);

            // Column Sums
            //alglib.minbleicsetlc(state, new double[,] { { 1, 0, 0, 1, 0, 0, 1, 0, 0, colSums[0] } }, 1);
            //alglib.minbleicsetlc(state, new double[,] { { 0, 1, 0, 0, 1, 0, 0, 1, 0, colSums[1] } }, 1);
            //alglib.minbleicsetlc(state, new double[,] { { 0, 0, 1, 0, 0, 1, 0, 0, 1, colSums[2] } }, 1);
            
            // 4. Set Boundary Constraints (x > 0)
            // Entropy is undefined for x <= 0. We use a tiny positive floor.
            double[] bLow = new double[9];
            for (int i = 0; i < 9; i++) bLow[i] = 1e-12;
            alglib.minbleicsetbc(state, bLow, new double[0]); // No high bounds

            // 5. Optimization Settings
            //alglib.minbleicsetinnercond(state, 1e-10, 0, 0); // Precision targets
            //alglib.minbleicsetoutercond(state, 1e-10, 0, 0);

            // 6. Run the solver using the Gradient function
            // Arguments: state, function/gradient delegate, progress delegate, obj
            alglib.minbleicoptimize(state, LossAndGradientFunction, null, null);

            // 7. Extract Results
            double[] resultX;
            alglib.minbleicreport rep;
            alglib.minbleicresults(state, out resultX, out rep);

            // --- Output Results ---
            Console.WriteLine($"Status: {rep.terminationtype} (Success if > 0)");
            Console.WriteLine($"Final Loss: {CalculateFinalLoss(resultX):F6}");
            Console.WriteLine("\nOptimized 3x3 Matrix:");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"{resultX[i * 3]:F4}\t{resultX[i * 3 + 1]:F4}\t{resultX[i * 3 + 2]:F4}");
            }
        }

        /// <summary>
        /// Combined Function and Gradient calculation.
        /// ALGLIB requires this specific signature for analytic optimization.
        /// </summary>
        private static void LossAndGradientFunction(double[] x, ref double func, double[] grad, object obj)
        {
            func = 0;
            for (int i = 0; i < x.Length; i++)
            {
                // Safety: if a[i] is 0, RAS dictates x[i] must be 0. 
                // We ignore these to avoid log(0).
                if (_a[i] > 0)
                {
                    double ratio = x[i] / _a[i];

                    // Objective: f = sum( x * ln(x/a) )
                    func += x[i] * Math.Log(ratio);

                    // Gradient: df/dx = ln(x/a) + 1
                    grad[i] = Math.Log(ratio) + 1;
                }
                else
                {
                    grad[i] = 0; // Or a very high penalty if x[i] > 0
                }
            }
        }

        private static double CalculateFinalLoss(double[] x)
        {
            double f = 0;
            for (int i = 0; i < x.Length; i++)
                if (_a[i] > 0 && x[i] > 0) f += x[i] * Math.Log(x[i] / _a[i]);
            return f;
        }
    }
}

