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
	public class LinearSolverTests
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

		[Test] public virtual void SparseRowMatrix()
		{
			Random r = new Random();
			int n = Math.Max(TesterUtilities.getInt(nmax, r), 4);

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
					new CGSolver(), 
					new CGSSolver(), 
					new GMRESSolver(), 
					new QMRSolver()
				};

			IPreconditioner[] M = new IPreconditioner[]
				{
					new IdentityPreconditioner(),
					new ILUPreconditioner(new SparseRowMatrix(A.RowCount, A.ColumnCount))
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

			assemble(A, b, K, f);
			boundary(A, b, x, gl, gr);
			double[] ans = solve(A, b, x, solver, M, iter);
			double[] ref_Renamed = reference(b.Length - 1, K, f, gl, gr);

			checkEqual(ans, ref_Renamed);

            Console.WriteLine("TT hej");

			Blas.Default.Zero(A);
			Blas.Default.Zero(b);
			Blas.Default.Zero(x);
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

		private double[] solve(IMatrix A, IVector b, IVector x, ILinearSolver solver, 
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