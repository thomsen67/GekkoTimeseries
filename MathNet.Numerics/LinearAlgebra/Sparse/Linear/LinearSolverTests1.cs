#region Copyright ©2004 Joannes Vermorel

// MathNet Numerics, part of MathNet
//
// Copyright (c) 2004,	Joannes Vermorel, http://www.vermorel.com
// Based on JMP , Copyright (c) 2003 Bjørn-Ove Heimsund
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public 
// License along with this program; if not, write to the Free Software
// Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

#endregion

#if DEBUG

using System;
using NUnit.Framework;

using MathNet.Numerics.LinearAlgebra.Sparse;
using MathNet.Numerics.LinearAlgebra.Sparse.Linear;
using MathNet.Numerics.LinearAlgebra.Sparse.Tests;


namespace MathNet.Numerics.LinearAlgebra.Sparse.Linear.Tests
{
	/// <summary> Test of the iterative linear solvers</summary>
	[TestFixture]
	public class LinearSolverTests1
	{
		private static int nmax = 100;

		[Test] public virtual void SparseColumnRowMatrix()
		{
			Random r = new Random();
			int n = Math.Max(TesterUtilities.getInt(nmax, r), 4);

			IElementalAccessMatrix A = new SparseColumnRowMatrix(n, n, 3);
			IElementalAccessVector b = new DenseVector(n), x = new DenseVector(n);

			Helper1(A, b, x);
		}

		[Test] public virtual void SparseColumnMatrix()
		{
			Random r = new Random();
			int n = Math.Max(TesterUtilities.getInt(nmax, r), 4);

			IElementalAccessMatrix A = new SparseColumnMatrix(n, n, 3);
			IElementalAccessVector b = new DenseVector(n), x = new DenseVector(n);

			Helper1(A, b, x);
		}

		[Test] public virtual void SparseRowColumnMatrix()
		{
			Random r = new Random();
			int n = Math.Max(TesterUtilities.getInt(nmax, r), 4);

			IElementalAccessMatrix A = new SparseRowColumnMatrix(n, n, 3);
			IElementalAccessVector b = new DenseVector(n), x = new DenseVector(n);

			Helper1(A, b, x);
		}


        //public void solveModelSlet()
        //{
        //    int n = 5;
        //    IElementalAccessMatrix A = new SparseRowMatrix(n, n, 5);
        //    IElementalAccessVector b = new DenseVector(n), x = new DenseVector(n);
        //    x.SetValue(0, 0.1);
        //    x.SetValue(1, 0.2);
        //    x.SetValue(2, 0.3);
        //    x.SetValue(3, 0.4);
        //    x.SetValue(4, 0.5);
        //    b = new DenseVector(x.Length);

        //    for (int it = 0; it < 100; it++)
        //    {
        //        model(b, x);  //b is residuals
        //        grad(A, x);
        //        IElementalAccessVector dx = new DenseVector(n);
        //        ILinearSolver solver = new BiCGSolver();
        //        IPreconditioner M = new IdentityPreconditioner();
        //        DefaultLinearIteration iter = new DefaultLinearIteration();
        //        iter.SetParameters(1e-10, 1e-50, 1e+5, 1000000);
        //        double[] ans = solve(A, b, dx, solver, M, iter);
        //        for (int i = 0; i < x.Length; i++)
        //        {
        //            x.AddValue(i,-0.2*dx.GetValue(i));  //0.2 damp
        //        } 
        //        //int iii = 1;
        //    }
           



        //}

        [Test]
        public virtual void SparseRowMatrix()
		{
			Random r = new Random();
			//int n = Math.Max(TesterUtilities.getInt(nmax, r), 4);
            int n = 3;
			IElementalAccessMatrix A = new SparseRowMatrix(n, n, 3);
			IElementalAccessVector b = new DenseVector(n), x = new DenseVector(n);
            
            
			Helper1(A, b, x);
		}

		private void Helper1(IElementalAccessMatrix A, IElementalAccessVector b, IElementalAccessVector x)
		{
			// Test of usual solvers and preconditoners
			ILinearSolver[] solver = new ILinearSolver[] 
				{
					new BiCGSolver(), 
					new BiCGstabSolver(), 
					//new CGSolver(), 
					new CGSSolver(), 
					new GMRESSolver(), 
					new QMRSolver()
				};

			IPreconditioner[] M = new IPreconditioner[]
				{
					new IdentityPreconditioner(),
					//new ILUPreconditioner(new SparseRowMatrix(A.RowCount, A.ColumnCount))
				};

			DefaultLinearIteration iter = new DefaultLinearIteration();
			iter.SetParameters(1e-10, 1e-50, 1e+5, 1000000);

			for (int i = 0; i < solver.Length; ++i)
				for (int j = 0; j < M.Length; ++j)
					Helper2(A, b, x, solver[i], M[j], iter);
		}

		private void Helper2(IElementalAccessMatrix A, IElementalAccessVector b, 
			IElementalAccessVector x, ILinearSolver solver, IPreconditioner M, ILinearIteration iter)
		{
			Random r = new Random();
			double K = r.NextDouble(), f = r.NextDouble(), gl = r.NextDouble(), gr = r.NextDouble();

			//assemble(A, b, K, f);
			//boundary(A, b, x, gl, gr);

            A.SetValue(0, 0, 1);
            A.SetValue(0, 1, 1);
            A.SetValue(0, 2, 1.3);
            A.SetValue(1, 0, -0.5);
            A.SetValue(1, 1, -0.25);
            A.SetValue(1, 2, -1.25);
            A.SetValue(2, 0, 2.5);
            A.SetValue(2, 1, 3.25);
            A.SetValue(2, 2, -1.25);
            b.SetValue(0, 1.25);
            b.SetValue(1, 3.75); 
            b.SetValue(1, -2.35);


			//double[] ans = solve(A, b, x, solver, M, iter);
			//double[] ref_Renamed = reference(b.Length - 1, K, f, gl, gr);
            //checkEqual(ans, ref_Renamed);

            Console.WriteLine("!:  " + solver + " " + M + " " + iter + "    " + x.GetValue(0) + " " + x.GetValue(1) + " " + x.GetValue(2));

            Blas.Default.Zero(A);
			Blas.Default.Zero(b);
			Blas.Default.Zero(x);
		}


        public static void model11(IElementalAccessVector y, IElementalAccessVector x)
        {
            //y er residualer, mens x er endogene

            //for (int i = 0; i < WindowsApplication1.Program.varsBigEndoNoLag.Count; i++)
            //{

            //}


            y.SetValue(0, x.GetValue(0) - x.GetValue(3) * x.GetValue(3) - 1);
            y.SetValue(1, x.GetValue(1) + x.GetValue(2) - 1);
            y.SetValue(2, x.GetValue(0) + x.GetValue(1) + 2);
            y.SetValue(3, x.GetValue(4) * x.GetValue(3) - 1);
            y.SetValue(4, x.GetValue(3) + 2 - x.GetValue(0));
        }

        public static void grad11(IElementalAccessMatrix dy, IElementalAccessVector x)
        {
            IElementalAccessVector y0 = new DenseVector(x.Length);
            IElementalAccessVector y = new DenseVector(x.Length);
            //model(y0, x);
            for (int i = 0; i < x.Length; i++)
            {
                x.AddValue(i,0.000001);
                //model(y, x);
                for (int j = 0; j < x.Length; j++)
                {
                    dy.SetValue(j, i, (y.GetValue(j) - y0.GetValue(j))/0.000001);
                }
                x.AddValue(i, -0.000001);
            }
        }
        
        private double[] reference(int n, double K, double f, double gl, double gr)
		{
			double[] ret = new double[n + 1];
			double x = 0, dx = 1.0/((double) n);

			ret[0] = gl;

			for (int i = 1; i < n; ++i)
			{
				x += dx;
				ret[i] = ((- f)/(2.0*K))*x*x + (gr - gl + f/(2.0*K))*x + gl;
			}

			ret[n] = gr;

			return ret;
		}

		private void checkEqual(double[] A, double[] B)
		{
			Assert.IsTrue(A.Length == B.Length);
			for (int i = 0; i < A.Length; ++i)
				Assert.AreEqual(B[i], A[i], 1e-8);
		}

		private void assemble(IElementalAccessMatrix A, IElementalAccessVector b, double K, double f)
		{
			int n = b.Length - 1;
			double h = 1.0/((double) n);

			double[,] Ae = new double[,] { {K/h, (- K)/h},  {(- K)/h, K/h}};
			double[] be = new double[] {f*h/2.0, f*h/2.0};

			for (int i = 0; i < n; ++i)
			{
				int[] ind = new int[] {i, i + 1};
				A.AddValues(ind, ind, Ae);
				b.AddValues(ind, be);
                //Console.WriteLine(i+" ind " + ind + " Ae " + Ae + " be " + be);
			}
		}

		private void boundary(IElementalAccessMatrix A, IElementalAccessVector b, 
			IElementalAccessVector x, double gl, double gr)
		{
			int n = b.Length - 1;
			int[] boundary = new int[] {0, n};
			double[] boundaryV = new double[] {gl, gr};

			x.SetValues(boundary, boundaryV);
			b.SetValues(boundary, boundaryV);

			A.SetValue(0, 0, 1);
			A.SetValue(0, 1, 0);
			A.SetValue(n, n, 1);
			A.SetValue(n, n - 1, 0);
		}

		public static double[] solveSlet(IMatrix A, IVector b, IVector x, ILinearSolver solver, 
			IPreconditioner M, ILinearIteration iter)
		{
			M.Setup(A);
			solver.Preconditioner = M;
			solver.Iteration = iter;
			return Blas.Default.GetArrayCopy(solver.Solve(A, b, x));
		}
	}
}

#endif