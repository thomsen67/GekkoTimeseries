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

namespace MathNet.Numerics.LinearAlgebra.Sparse.Tests
{
	/// <summary> Test matrix assembly</summary>
	[TestFixture]
	public class BlasTests
	{
		private static int nmax = 100;
		private static int mmax = 100;
		private static int numax = 10;
		private static int repeat = 3;

		static Random r = new Random();

		private int GetNu(int n, int m)
		{
			return Math.Min(n, Math.Min(m, TesterUtilities.getInt(numax, r)));
		}

		/// <summary>Testing <see cref="SparseRowColumnMatrix"/> class.</summary>
		[Test] public virtual void SparseRowColumnMatrix()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseRowColumnMatrix(n, m, nu);
				check(A, TesterUtilities.SetAssembleRowMatrix(A, nu));

				A = new SparseRowColumnMatrix(n, m, nu);
				check(A, TesterUtilities.AddAssembleRowMatrix(A, nu));
			}
		}

		[Test] public virtual void SparseRowMatrix()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			IElementalAccessMatrix A = new SparseRowMatrix(n, m);
			repeatedAssembly(A, nu);
		}

		[Test] public virtual void SparseColumnRowMatrix()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < 10; ++i)
			{
				IElementalAccessMatrix A = new SparseColumnRowMatrix(n, m, nu);
				check(A, TesterUtilities.setAssembleColumnMatrix(A, nu));

				A = new SparseColumnRowMatrix(n, m, nu);
				check(A, TesterUtilities.addAssembleColumnMatrix(A, nu));
			}
		}

		[Test] public virtual void SparseColumnMatrix()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			IElementalAccessMatrix A = new SparseColumnMatrix(n, m);
			repeatedAssembly(A, nu);
		}


		[Test] public virtual void CoordinateMatrix()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			IElementalAccessMatrix A = new CoordinateMatrix(n, m, nu);
			repeatedAssembly(A, nu);
		}

		private void AreEqual(double[,] A, double[,] B)
		{
		
			Assert.AreEqual(A.GetLength(0), B.GetLength(0));
			Assert.AreEqual(A.GetLength(1), B.GetLength(1));

			for(int i = 0; i < A.GetLength(0); i++)
				for(int j = 0; j < A.GetLength(1); j++)
					Assert.AreEqual(B[i, j], A[i, j], 1e-12);
		}

		private void check(IElementalAccessMatrix A, double[,] real)
		{
			AreEqual( TesterUtilities.getElMatrix(A),  real);
			AreEqual( TesterUtilities.getMatrix(A),  real);
			AreEqual( TesterUtilities.getMatrixCopy(A),  real);
		}

		private void repeatedAssembly(IElementalAccessMatrix A, int nu)
		{
			for (int i = 0; i < repeat; ++i)
			{
				check(A, TesterUtilities.SetAssembleRowMatrix(A, nu));
				Blas.Default.Zero(A);

				check(A, TesterUtilities.setsAssembleRowMatrix(A, nu));
				Blas.Default.Zero(A);

				check(A, TesterUtilities.AddAssembleRowMatrix(A, nu));
				Blas.Default.Zero(A);

				check(A, TesterUtilities.addsAssembleRowMatrix(A, nu));
				Blas.Default.Zero(A);

				check(A, TesterUtilities.setAssembleColumnMatrix(A, nu));
				Blas.Default.Zero(A);

				check(A, TesterUtilities.setsAssembleColumnMatrix(A, nu));
				Blas.Default.Zero(A);

				check(A, TesterUtilities.addAssembleColumnMatrix(A, nu));
				Blas.Default.Zero(A);

				check(A, TesterUtilities.addsAssembleColumnMatrix(A, nu));
				Blas.Default.Zero(A);
			}
		}

		// TESTING MATRIX PRODUCTS

		[Test] public virtual void SparseRowColumnMult()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseRowColumnMatrix(n, m, nu);
				IElementalAccessVector x = new DenseVector(m), 
					y = new DenseVector(n), 
					z = new DenseVector(n);

				rowCheck(A, x, y, z, nu);
			}
		}

		[Test] public virtual void SparseRowMult()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseRowMatrix(n, m, nu);
				IElementalAccessVector x = new DenseVector(m), 
					y = new DenseVector(n), 
					z = new DenseVector(n);

				rowCheck(A, x, y, z, nu);
			}
		}

		[Test] public virtual void SparseColumnRowMult()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseColumnRowMatrix(n, m, nu);
				IElementalAccessVector x = new DenseVector(m), 
					y = new DenseVector(n), 
					z = new DenseVector(n);

				columnCheck(A, x, y, z, nu);
			}
		}

		[Test] public virtual void SparseColumnMult()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseColumnMatrix(n, m, nu);
				IElementalAccessVector x = new DenseVector(m), 
					y = new DenseVector(n), 
					z = new DenseVector(n);

				columnCheck(A, x, y, z, nu);
			}
		}

		[Test] public virtual void SparseRowColumnTransMult()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseRowColumnMatrix(n, m, nu);
				IElementalAccessVector x = new DenseVector(n), 
					y = new DenseVector(m), 
					z = new DenseVector(m);

				transRowCheck(A, x, y, z, nu);
			}
		}

		[Test] public virtual void SparseRowTransMult()
		{
			Random r = new Random();
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseRowMatrix(n, m, nu);
				IElementalAccessVector x = new DenseVector(n), 
					y = new DenseVector(m), 
					z = new DenseVector(m);

				transRowCheck(A, x, y, z, nu);
			}
		}

		[Test] public virtual void SparseColumnRowTransMult()
		{
			Random r = new Random();
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseColumnRowMatrix(n, m, nu);
				IElementalAccessVector x = new DenseVector(n), 
					y = new DenseVector(m), 
					z = new DenseVector(m);

				transColumnCheck(A, x, y, z, nu);
			}
		}

		[Test] public virtual void SparseColumnTransMult()
		{
			Random r = new Random();
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseColumnMatrix(n, m, nu);
				IElementalAccessVector x = new DenseVector(n), 
					y = new DenseVector(m), 
					z = new DenseVector(m);

				transColumnCheck(A, x, y, z, nu);
			}
		}

		private void rowCheck(IElementalAccessMatrix A, IElementalAccessVector x, 
			IElementalAccessVector y, IElementalAccessVector z, int nu)
		{
			double[,] Ad = TesterUtilities.SetAssembleRowMatrix(A, nu);
			check(A, x, y, z, Ad);
		}

		private void columnCheck(IElementalAccessMatrix A, IElementalAccessVector x, 
			IElementalAccessVector y, IElementalAccessVector z, int nu)
		{
			double[,] Ad = TesterUtilities.setAssembleColumnMatrix(A, nu);
			check(A, x, y, z, Ad);
		}

		private void transRowCheck(IElementalAccessMatrix A, IElementalAccessVector x, 
			IElementalAccessVector y, IElementalAccessVector z, int nu)
		{
			double[,] Ad = TesterUtilities.SetAssembleRowMatrix(A, nu);
			transCheck(A, x, y, z, Ad);
		}

		private void transColumnCheck(IElementalAccessMatrix A, IElementalAccessVector x, 
			IElementalAccessVector y, IElementalAccessVector z, int nu)
		{
			double[,] Ad = TesterUtilities.setAssembleColumnMatrix(A, nu);
			transCheck(A, x, y, z, Ad);
		}

		private void check(IElementalAccessMatrix A, IElementalAccessVector x, 
			IElementalAccessVector y, IElementalAccessVector z, double[,] Ad)
		{
			double[] xd = TesterUtilities.setAssembleVector(x), 
				yd = TesterUtilities.setAssembleVector(y), 
				zd = TesterUtilities.setAssembleVector(z);
			Random r = new Random();
			double alpha = r.NextDouble(), beta = r.NextDouble();


			Blas.Default.MultAdd(alpha, A, x, beta, y, z);
			multAdd(alpha, Ad, xd, beta, yd, zd);

			checkEqual(z, zd);
		}

		private void transCheck(IElementalAccessMatrix A, IElementalAccessVector x, 
			IElementalAccessVector y, IElementalAccessVector z, double[,] Ad)
		{
			double[] xd = TesterUtilities.setAssembleVector(x), 
				yd = TesterUtilities.setAssembleVector(y), 
				zd = TesterUtilities.setAssembleVector(z);
			Random r = new Random();
			double alpha = r.NextDouble(), beta = r.NextDouble();

			Blas.Default.TransMultAdd(alpha, A, x, beta, y, z);
			transMultAdd(alpha, Ad, xd, beta, yd, zd);

			checkEqual(z, zd);
		}

		private void multAdd(double alpha, double[,] A, 
			double[] x, double beta, double[] y, double[] z)
		{
			for (int i = 0; i < A.GetLength(0); ++i)
			{
				double val = 0.0;

				for (int j = 0; j < A.GetLength(1); ++j)
					val += A[i, j]*x[j];

				z[i] = alpha*val + beta*y[i];
			}
		}

		private void transMultAdd(double alpha, double[,] A, 
			double[] x, double beta, double[] y, double[] z)
		{
			int n = x.Length, m = y.Length;

			// Perform transposition
			double[,] At = new double[m, n];
//			for (int i = 0; i < m; i++)
//			{
//				At[i] = new double[n];
//			}
			for (int i = 0; i < m; ++i)
				for (int j = 0; j < n; ++j)
					At[i, j] = A[j, i];

			multAdd(alpha, At, x, beta, y, z);
		}

		private void checkEqual(IElementalAccessVector x, double[] data)
		{
			Assert.IsTrue(x.Length == data.Length);
			for (int i = 0; i < data.Length; ++i)
				Assert.AreEqual(x.GetValue(i), data[i], 1e-12);
		}

		// TESTING MATRIX TRANSPOSITIONS

		[Test] public virtual void SparseRowColumnTranspose()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseRowColumnMatrix(n, m, nu);
				double[,] Am = TesterUtilities.SetAssembleRowMatrix(A, nu);
				TransposeCheck(A, Am);
			}
		}

		[Test] public virtual void SparseRowTranspose()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseRowMatrix(n, m, nu);
				double[,] Am = TesterUtilities.SetAssembleRowMatrix(A, nu);
				TransposeCheck(A, Am);
			}
		}

		[Test] public virtual void SparseColumnTranspose()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseColumnMatrix(n, m, nu);
				double[,] Am = TesterUtilities.SetAssembleRowMatrix(A, nu);
				TransposeCheck(A, Am);
			}
		}

		[Test] public virtual void SparseColumnRowTranspose()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r), 
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new SparseColumnRowMatrix(n, m, nu);
				double[,] Am = TesterUtilities.setAssembleColumnMatrix(A, nu);
				TransposeCheck(A, Am);
			}
		}

		[Test] public virtual void CoordinateTranspose()
		{
			int n = TesterUtilities.getInt(nmax, r), 
				m = TesterUtilities.getInt(mmax, r),
				nu = GetNu(n, m);

			for (int i = 0; i < repeat; ++i)
			{
				IElementalAccessMatrix A = new CoordinateMatrix(n, m);
				double[,] Am = TesterUtilities.SetAssembleRowMatrix(A, nu);
				TransposeCheck(A, Am);
			}
		}

		private void TransposeCheck(IElementalAccessMatrix A, double[,] Am)
		{
			int n = A.RowCount, m = A.ColumnCount;

			HelperTranspose(A, new SparseRowMatrix(m, n), Am);
			HelperTranspose(A, new SparseColumnMatrix(m, n), Am);
			//HelperTranspose(A, new DenseRowMatrix(m, n), Am);
			//HelperTranspose(A, new DenseColumnMatrix(m, n), Am);
			//HelperTranspose(A, new DenseRowColumnMatrix(m, n), Am);
			//HelperTranspose(A, new DenseColumnRowMatrix(m, n), Am);
			HelperTranspose(A, new CoordinateMatrix(m, n), Am);
		}

		private void HelperTranspose(IElementalAccessMatrix A, IElementalAccessMatrix B, double[,] Am)
		{
			Blas.Default.Transpose(A, B);
			double[,] tmpArray = new double[A.ColumnCount, A.RowCount];
//			for (int i2 = 0; i2 < A.ColumnCount; i2++)
//			{
//				tmpArray[i2] = new double[A.RowCount];
//			}
			double[,] Bm = Transpose(Am, tmpArray);
			AreEqual(Blas.Default.GetArrayCopy(B), Bm);
		}

		private double[,] Transpose(double[,] A, double[,] B)
		{
			int n = A.GetLength(0), m = B.GetLength(0);
			for (int i = 0; i < n; ++i)
				for (int j = 0; j < m; ++j)
					B[j, i] = A[i, j];
			return B;
		}

		// VECTOR TESTING

		private int GetNu(int n)
		{
			return Math.Min(n, TesterUtilities.getInt(numax, r));
		}

		[Test] public virtual void DenseVector()
		{
			int n = TesterUtilities.getInt(nmax, r), nu = GetNu(n);
			IElementalAccessVector x = new DenseVector(n);
			repeatedAssembly(x, nu);
		}

		[Test] public virtual void SparseVector()
		{
			int n = TesterUtilities.getInt(nmax, r), nu = GetNu(n);
			IElementalAccessVector x = new SparseVector(n);
			repeatedAssembly(x, nu);
		}

		private void repeatedAssembly(IElementalAccessVector x, int nu)
		{
			for (int i = 0; i < repeat; ++i)
			{
				Check(x, TesterUtilities.setAssembleVector(x, nu));
				Blas.Default.Zero(x);

				Check(x, TesterUtilities.setsAssembleVector(x, nu));
				Blas.Default.Zero(x);

				Check(x, TesterUtilities.addAssembleVector(x, nu));
				Blas.Default.Zero(x);

				Check(x, TesterUtilities.addsAssembleVector(x, nu));
				Blas.Default.Zero(x);
			}
		}

		private void Check(IElementalAccessVector x, double[] real)
		{
			AreEqual(TesterUtilities.getElVector(x), real);
			AreEqual(TesterUtilities.getVector(x), real);
			AreEqual(TesterUtilities.getVectorCopy(x), real);
		}

		private void AreEqual(double[] A, double[] B)
		{
			Assert.IsTrue(A.Length == B.Length);
			for (int i = 0; i < A.Length; ++i)
				Assert.AreEqual(B[i], A[i], 1e-12);
		}
	}
}

#endif