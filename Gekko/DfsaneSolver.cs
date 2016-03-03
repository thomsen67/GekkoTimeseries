/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2016, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.        
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Gekko
{

    public struct Ls_ret
    {
        public double[] Fnew;
        public double[] pnew;
        public double[] xnew;
        public double fune;
        public double fcnt;
        public double bl;
        public double lsflag;
    }

    public class DfsaneSolver
    {

        public static double quad(double[] f)
        {
            double sum = 0;
            for (int i = 0; i < f.Length; i++)
            {
                sum += f[i] * f[i];
            }
            return sum;
        }

        public static bool problem(double[] f)
        {
            bool r = false;
            for (int i = 0; i < f.Length; i++)
            {
                if (double.IsNaN(f[i]) || double.IsInfinity(f[i])) r = true;
            }
            return r;
        }

        public static double[] function2(double[] p)
        {
            int n = p.Length;
            double[] f = new double[n];
            for (int i = 1; i < n; i++)
            {
                double xx = Math.Exp(-p[i - 1] * p[i - 1]);
                f[i - 1] = i / 10d * (1d - p[i - 1] * p[i - 1] - xx);
            }
            f[n - 1] = n / 10d * (1d - Math.Exp(-p[n - 1] * p[n - 1]));
            return f;
        }

        public static double[] function(double[] p, Type assembly)
        {            
            double[] f = new double[p.Length];
            //f[0] = p[1] - p[0];
            //f[1] = p[0] * p[0] + p[1] * p[1] - 1;
            int numericalProblem = 0;
            Program.rssFunction2(out numericalProblem, f, p, assembly);
            for (int i = 0; i < f.Length; i++)
            {
              if (numericalProblem == 1)
            {

            }
              if(double.IsNaN(f[i]) || double.IsInfinity(f[i])) {

              }
            }
          
            return f;
        }

        public static Ls_ret lsm(double[] x, double[] F, double fval, double alfa, int M, double[] lastfv, double eta, double fcnt, double bl, Type assembly)
        {
            int n = x.Length;
            Ls_ret ls_ret = new Ls_ret();           

            double maxbl = 100;
            double gamma = 1e-04;
            double sigma1 = 0.1;
            double sigma2 = 0.5;
            double lam1 = 1;
            double lam2 = 1;
            double cbl = 0;

            double fmax = double.NegativeInfinity;
            for (int i = 0; i < lastfv.Length; i++)
            {
                fmax = Math.Max(fmax, lastfv[i]);
            }

            double[] xnew = new double[n];
            double[] Fnew = new double[n];
            while (cbl < maxbl)
            {
                double[] d = new double[n];
                for (int i = 0; i < n; i++)
                {
                    d[i] = -alfa * F[i];
                }

                xnew = new double[n];
                for (int i = 0; i < n; i++)
                {
                    xnew[i] = x[i] + lam1 * d[i];
                }

                Fnew = function(xnew, assembly);

                fcnt++;

                if (problem(Fnew))
                {
                    ls_ret.xnew = null;
                    ls_ret.Fnew = null;
                    ls_ret.fcnt = fcnt;
                    ls_ret.bl = bl;
                    ls_ret.lsflag = 1;
                    ls_ret.fune = double.NaN;
                    return ls_ret;
                }

                double fune1 = quad(Fnew);

                if (fune1 <= (fmax + eta - (lam1 * lam1 * gamma * fval)))
                {
                    if (cbl >= 1)
                        bl = bl + 1;

                    ls_ret.xnew = xnew;
                    ls_ret.Fnew = Fnew;
                    ls_ret.fcnt = fcnt;
                    ls_ret.bl = bl;
                    ls_ret.lsflag = 0;
                    ls_ret.fune = fune1;
                    return ls_ret;
                    //  return(list(xnew = xnew, Fnew = Fnew, fcnt = fcnt, 
                    //bl = bl, lsflag = 0, fune = fune1))
                }

                for (int i = 0; i < n; i++)
                {
                    xnew[i] = x[i] - lam2 * d[i];
                }

                Fnew = function(xnew, assembly);

                fcnt++;

                if (problem(Fnew))
                {
                    ls_ret.xnew = null;
                    ls_ret.Fnew = null;
                    ls_ret.fcnt = fcnt;
                    ls_ret.bl = bl;
                    ls_ret.lsflag = 1;
                    ls_ret.fune = double.NaN;
                    return ls_ret;
                }

                double fune2 = quad(Fnew);

                if (fune2 <= (fmax + eta - (lam2 * lam2 * gamma * fval)))
                {
                    if (cbl >= 1)
                        bl = bl + 1;

                    ls_ret.xnew = xnew;
                    ls_ret.Fnew = Fnew;
                    ls_ret.fcnt = fcnt;
                    ls_ret.bl = bl;
                    ls_ret.lsflag = 0;
                    ls_ret.fune = fune2;
                    return ls_ret;
                    //  return(list(xnew = xnew, Fnew = Fnew, fcnt = fcnt, 
                    //bl = bl, lsflag = 0, fune = fune2))
                }

                double lamc = (2d * fval * lam1 *lam1) / (2d * (fune1 + (2d * lam1 - 1d) * fval));
                double c1 = sigma1 * lam1;
                double c2 = sigma2 * lam1;
                if (lamc < c1)
                    lam1 = c1;
                else if (lamc > c2)
                    lam1 = c2;
                else lam1 = lamc;
                lamc = (2d * fval * lam2 * lam2) / (2d * (fune2 + (2d * lam2 - 1d) * fval));
                c1 = sigma1 * lam2;
                c2 = sigma2 * lam2;
                if (lamc < c1)
                    lam2 = c1;
                else if (lamc > c2)
                    lam2 = c2;
                else lam2 = lamc;
                cbl = cbl + 1;


            }
            ls_ret.xnew = xnew;
            ls_ret.Fnew = Fnew;
            ls_ret.fcnt = fcnt;
            ls_ret.bl = bl;
            ls_ret.lsflag = 2;
            ls_ret.fune = double.NaN;  //TTH: set to NaN
            return ls_ret;
            //return(list(xnew = xnew, Fnew = Fnew, fcnt = fcnt, bl = bl, 
            //lsflag = 2, fune = fune))

        }

        public static void Solve(double[] par, Type assembly)
        {
            //double[] x0 = new double[] { 0.3235007, 0.7100014, 0.4234151, 0.4153759, 0.1978808, 0.8531196, 0.4555675, 0.5234385, 0.2557746, 0.5328845 };

            //Iteration:  0  ||F(x0)||:  0.08864655 
            //iteration:  10  ||F(xn)|| =   0.1041679 
            //iteration:  20  ||F(xn)|| =   0.02157386 
            //iteration:  30  ||F(xn)|| =   0.003530724 
            //iteration:  40  ||F(xn)|| =   8.22081e-05 
            //iteration:  50  ||F(xn)|| =   8.77939e-07 
            //    ctrl <- list(maxit = 1500, M = 10, tol = 1e-07, trace = TRUE, 
            //triter = 10, noimp = 100, NM=FALSE, BFGS=FALSE)

          string conv = "";
            
          int method = 2;

            int M = 10;  //should be between 5-20, very hard stuff kan use > 20.
            double maxit = 15000;
            double tol = 1e-07;
            bool trace = true;
            double triter = 10;  //when to print iterations
            double noimp = 100;  //when to stop (no improvement)

            double n = par.Length;
            double fcnt = 0;
            int iter = 0;
            double bl = 0;
            double alfa = 1;
            double eta = 1;
            double eps = 1e-10;
            double[] lastfv = new double[M];

            double[] F = function(par, assembly);
            fcnt++;
            double F0 = Math.Sqrt(quad(F));
            double normF = F0;
            G.Writeln("Iteration 0, ||F(x0)||: " + F0 / Math.Sqrt(n));

            double[] pbest = par;
            double normF_best = normF;
            lastfv[0] = normF * normF;
            double flag = 0;
            double knoimp = 0;

            double alfa1 = 0;
            double alfa2 = 0;
            while (normF / Math.Sqrt(n) > tol && iter <= maxit)
            {
                if ((Math.Abs(alfa) <= eps) || (Math.Abs(alfa) >= 1 / eps))
                {
                    if (normF > 1)
                        alfa = 1;
                    else if (normF >= 1e-05 && normF <= 1)
                        alfa = 1 / normF;
                    else if (normF < 1e-05)
                        alfa = 1e+05;
                }

                
                if (iter == 0)
                {
                    alfa = Math.Min(1d / normF, 1d);
                    alfa1 = alfa;
                    alfa2 = alfa;
                }

                double temp = alfa2;
                alfa2 = alfa;
                if (normF <= 0.01) alfa = alfa1;   // retard scheme
                alfa1 = temp;

                Ls_ret ls_ret = lsm(par, F, normF * normF, alfa, M, lastfv, eta, fcnt, bl, assembly);

                fcnt = ls_ret.fcnt;
                bl = ls_ret.bl;
                flag = ls_ret.lsflag;

                if (flag > 0)
                    break;

                double[] Fnew = ls_ret.Fnew;
                double[] pnew = ls_ret.xnew;
                double fune = ls_ret.fune;

                double pF = 0;
                double pp = 0;
                double FF = 0;
                for (int i = 0; i < n; i++)
                {
                    pF += ((pnew[i] - par[i]) * (Fnew[i] - F[i]));
                    pp += ((pnew[i] - par[i]) * (pnew[i] - par[i]));
                    FF += ((Fnew[i] - F[i]) * (Fnew[i] - F[i]));
                }

                if (method == 1)
                    alfa = pp / pF;
                else if (method == 2)
                    alfa = pF / FF;
                else if (method == 3)
                    alfa = Math.Sign(pF) * Math.Sqrt(pp / FF);

                if (double.IsNaN(alfa))
                    alfa = eps;

                par = pnew;
                F = Fnew;
                double fun = fune;
                normF = Math.Sqrt(fun);

                if (normF < normF_best)
                {
                    pbest = par;
                    normF_best = normF;
                    knoimp = 0;
                }
                else knoimp = knoimp + 1;

                iter++;
                lastfv[(1 + iter % M) - 1] = fun;

                eta = F0 / ((iter + 1) * (iter + 1));

                G.Writeln("Iteration " + iter + " ||F(xn)||: " + normF);
                if (knoimp == noimp)
                {
                    flag = 3;
                    break;
                }

                
                if (flag == 0)
                {
                    if (normF_best / Math.Sqrt(n) <= tol)
                        conv = "Successful convergence";
                    else if (iter > maxit)
                        conv = "Maximum limit for iterations exceeded";
                    else
                        conv = "Method stagnated";
                }
                else if (flag == 1)
                    conv = "Failure: Error in function evaluation";
                else if (flag == 2)
                    conv = "Failure: Maximum limit on steplength reductions exceeded";
                else if (flag == 3)
                    conv = "Lack of improvement in objective function";



            }

          G.Writeln(conv);

        }
    }
}
