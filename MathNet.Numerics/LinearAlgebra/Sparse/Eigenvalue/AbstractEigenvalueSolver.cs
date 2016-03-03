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

using System;

using MathNet.Numerics.LinearAlgebra.Sparse;
using MathNet.Numerics.LinearAlgebra.Sparse.Utilities;

namespace MathNet.Numerics.LinearAlgebra.Sparse.Eigenvalue
{
	/// <summary> Partial implementation of EigenvalueSolver.</summary>
	public abstract class AbstractEigenvalueSolver : IEigenvalueSolver
	{
		/// <summary> Iteration object</summary>
		protected internal IEigenvalueIteration iter;

		/// <summary> Spectrum transformation</summary>
		protected internal IEigenvalueTransformation et;

		/// <summary> Constructor for AbstractEigenvalueSolver.</summary>
		public AbstractEigenvalueSolver()
		{
			iter = new DefaultEigenvalueIteration();
			et = new NormalEigenvalueTransformation();
		}

		public virtual IEigenvalueIteration Iteration
		{
			get { return iter; }
			set { this.iter = value; }
		}

		public virtual IEigenvalueTransformation Transformation
		{
			get { return et; }
			set { this.et = value; }
		}

		public virtual void Solve(IMatrix A, double[] eig, IVector[] x)
		{
			CheckSolveArguments(A, eig, x);
			SolveI(A, eig, x);
		}

		private void CheckSolveArguments(IMatrix A, double[] eig, IVector[] x)
		{
			if (!A.Square)
				throw new IndexOutOfRangeException("!A.isSquare()");
			else if (eig.Length != x.Length)
				throw new IndexOutOfRangeException("eig.length != x.length");
			for (int i = 0; i < x.Length; ++i)
				if (x[i].Length != A.RowCount)
					throw new IndexOutOfRangeException("x[" + i + "].size() != A.numRows()");
		}

		protected internal abstract void SolveI(IMatrix A, double[] eig, IVector[] x);

//		/// <summary> Packs the vectors into a dense column matrix</summary>
//		protected internal virtual DenseColumnRowMatrix Pack(IVector[] x)
//		{
//			int n = x[0].Length;
//			DenseColumnRowMatrix A = new DenseColumnRowMatrix(n, n);
//
//			IntDoubleVectorPair Amat = A.Matrix;
//			for (int i = 0; i < x.Length; ++i)
//			{
//				if (!(x[i] is IDenseAccessVector))
//					throw new NotSupportedException();
//				double[] xdata = ((IDenseAccessVector) x[i]).Vector;
//				Array.Copy(xdata, 0, Amat.data, Amat.indices[i], xdata.Length);
//			}
//
//			return A;
//		}

		/// <summary> Uses the Rayleigh Quotient to compute real eigenvalues for the given
		/// eigenvectors.</summary>
		protected internal virtual void Eigenvalues(IMatrix A, double[] eigR, double[] eigI, IVector[] x)
		{
			SupportClass.ArraySupport.Fill(eigI, 0.0);
			IVector temp = Factory.createVector(x[0]);
			for (int i = 0; i < x.Length; ++i)
				eigR[i] = Blas.Default.Dot(x[i], Blas.Default.Mult(A, x[i], temp))/Blas.Default.Dot(x[i], x[i]);
		}

		/// <summary> Runs normalize on each array-component</summary>
		protected internal virtual IVector[] Normalize(IVector[] x)
		{
			for (int i = 0; i < x.Length; ++i)
				Normalize(x[i]);
			return x;
		}

		/// <summary> Normalizes the given vector using the 2-norm. If it is a zero-vector,
		/// its entries will be populated from a uniform distribution, then
		/// normalized.</summary>
		protected internal virtual IVector Normalize(IVector x)
		{
			double norm = Blas.Default.Norm(x, NORMS.NORM2);
			if (!IsZero(norm))
				return Blas.Default.Scale(1.0/norm, x);
			else
				return Random(x);
		}

		/// <summary> Normalizes the given vector using the 2-norm. If it is a zero-vector,
		/// nothing is done.</summary>
		protected internal virtual IVector Normalize0(IVector x)
		{
			double norm = Blas.Default.Norm(x, NORMS.NORM2);
			if (!IsZero(norm))
				return Blas.Default.Scale(1.0/norm, x);
			else
				return x;
		}

		/// <summary> Each entry of q is populated from a uniform distribution. The vector will
		/// have a 2-norm of 1.</summary>
		protected internal virtual IVector Random(IVector q)
		{
			if (!(q is IElementalAccessVector))
				throw new NotSupportedException();
			IElementalAccessVector qe = (IElementalAccessVector) q;
			for (int i = 0; i < qe.Length; ++i)
				qe.SetValue(i, SupportClass.Random.NextDouble());
			return Blas.Default.Scale(1.0/Blas.Default.Norm(q, NORMS.NORM2), q);
		}

		/// <summary> Returns true if x is approximativly zero, else false</summary>
		private static bool IsZero(double x)
		{
			return Math.Abs(x) < 1e-12; // poor design (inline magic value)
		}
	}
}