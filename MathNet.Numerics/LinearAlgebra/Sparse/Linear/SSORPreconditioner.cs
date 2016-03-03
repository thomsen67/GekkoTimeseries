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

namespace MathNet.Numerics.LinearAlgebra.Sparse.Linear
{
	/// <summary> SSOR preconditioner.
	/// Uses a symmetrical SOR as a preconditioner. Meant for symmetrical matrices.
	/// For best performance, omega must be carefully chosen (between 0 and 2).
	/// </summary>
	public class SSORPreconditioner : AbstractPreconditioner, IPreconditioner
	{
		/// <summary> Sets over-relaxation parameter.</summary>
		public virtual double Parameters
		{
			set { this.omega = value; }

		}

		/// <summary> Overrelaxation parameter</summary>
		private double omega;

		/// <summary> All diagonal entries of the matrix</summary>
		private double[] diag;

		/// <summary> Indices to the diagonal entries of the matrix</summary>
		private int[] diagInd;

		/// <summary> Constructor for SSOR. Uses omega=1 by default.</summary>
		public SSORPreconditioner()
		{
			omega = 1;
		}

		public override IVector Apply(IMatrix A, IVector b, IVector x)
		{
			checkSizes(A, b, x);

			if (!(x is IDenseAccessVector))
				throw new NotSupportedException();

			// Copy b over to x, and get a dense array representation
			x = Blas.Default.Copy(b, x);
			double[] xdata = ((IDenseAccessVector) x).Vector;

			if (A is ISparseRowAccessMatrix)
				ssor((ISparseRowAccessMatrix) A, xdata);
			else if (A is ISparseRowColumnAccessMatrix)
				ssor((ISparseRowColumnAccessMatrix) A, xdata);
			else if (A is IDenseRowAccessMatrix)
				ssor((IDenseRowAccessMatrix) A, xdata);
			else if (A is IDenseRowColumnAccessMatrix)
				ssor((IDenseRowColumnAccessMatrix) A, xdata);
			else
				throw new NotSupportedException();

			return x;
		}

		private void ssor(ISparseRowAccessMatrix A, double[] xdata)
		{
			int n = A.RowCount;

			// M = (D+L) D^{-1} (D+L)^T

			// Solves (1/omega)*(D+L) y = b
			for (int i = 0; i < n; ++i)
			{
				// Do nothing if we get a divide by zero
				if (Math.Abs(diag[i]) == 0.0)
					continue;

				IntDoubleVectorPair Arow = A.GetRow(i);
				double[] Adat = Arow.data;
				int[] Aind = Arow.indices;

				double sum = 0;
				for (int j = 0; j < Aind.Length && Aind[j] < i; ++j)
					sum += Adat[j]*xdata[Aind[j]];
				xdata[i] = (omega/diag[i])*(xdata[i] - sum);
			}

			// Solves (omega/(2-omega))*D^{-1} z = y
			for (int i = 0; i < n; ++i)
			{
				// No need to do anything
				if (Math.Abs(diag[i]) == 0.0)
					continue;

				xdata[i] = (2 - omega)/omega*diag[i]*xdata[i];
			}

			// Solves (1/omega)*(D+L)^T x = z
			for (int i = n - 1; i >= 0; --i)
			{
				// Do nothing if we get a divide by zero
				if (Math.Abs(diag[i]) == 0.0)
					continue;

				IntDoubleVectorPair Arow = A.GetRow(i);
				double[] Adat = Arow.data;
				int[] Aind = Arow.indices;

				double sum = 0;
				for (int j = diagInd[i] + 1; j < Aind.Length; ++j)
					sum += Adat[j]*xdata[Aind[j]];
				xdata[i] = (omega/diag[i])*(xdata[i] - sum);
			}
		}

		private void ssor(ISparseRowColumnAccessMatrix A, double[] xdata)
		{
			int n = A.RowCount;

			IntIntDoubleVectorTriple Amat = A.Matrix;

			// M = (D+L) D^{-1} (D+L)^T

			// Solves (1/omega)*(D+L) y = b
			for (int i = 0; i < n; ++i)
			{
				// Do nothing if we get a divide by zero
				if (Math.Abs(diag[i]) == 0.0)
					continue;

				double sum = 0;
				for (int j = Amat.minor[i]; j < Amat.minor[i + 1] && Amat.major[j] < i; ++j)
					sum += Amat.data[j]*xdata[Amat.major[j]];
				xdata[i] = (omega/diag[i])*(xdata[i] - sum);
			}

			// Solves (omega/(2-omega))*D^{-1} z = y
			for (int i = 0; i < n; ++i)
			{
				// No need to do anything
				if (Math.Abs(diag[i]) == 0.0)
					continue;

				xdata[i] = (2 - omega)/omega*diag[i]*xdata[i];
			}

			// Solves (1/omega)*(D+L)^T x = z
			for (int i = n - 1; i >= 0; --i)
			{
				// Do nothing if we get a divide by zero
				if (Math.Abs(diag[i]) == 0.0)
					continue;

				double sum = 0;
				for (int j = diagInd[i] + 1; j < Amat.minor[i + 1]; ++j)
					sum += Amat.data[j]*xdata[Amat.major[j]];
				xdata[i] = (omega/diag[i])*(xdata[i] - sum);
			}
		}

		private void ssor(IDenseRowAccessMatrix A, double[] xdata)
		{
			int n = A.RowCount;

			// M = (D+L) D^{-1} (D+L)^T

			// Solves (1/omega)*(D+L) y = b
			for (int i = 0; i < n; ++i)
			{
				// Do nothing if we get a divide by zero
				if (Math.Abs(diag[i]) == 0.0)
					continue;

				double[] Arow = A.GetRow(i);

				double sum = 0;
				for (int j = 0; j < i; ++j)
					sum += Arow[j]*xdata[j];
				xdata[i] = (omega/diag[i])*(xdata[i] - sum);
			}

			// Solves (omega/(2-omega))*D^{-1} z = y
			for (int i = 0; i < n; ++i)
			{
				// No need to do anything
				if (Math.Abs(diag[i]) == 0.0)
					continue;

				xdata[i] = (2 - omega)/omega*diag[i]*xdata[i];
			}

			// Solves (1/omega)*(D+L)^T x = z
			for (int i = n - 1; i >= 0; --i)
			{
				// Do nothing if we get a divide by zero
				if (Math.Abs(diag[i]) == 0.0)
					continue;

				double[] Arow = A.GetRow(i);

				double sum = 0;
				for (int j = i + 1; j < n; ++j)
					sum += Arow[j]*xdata[j];
				xdata[i] = (omega/diag[i])*(xdata[i] - sum);
			}
		}

		private void ssor(IDenseRowColumnAccessMatrix A, double[] xdata)
		{
			int n = A.RowCount;

			IntDoubleVectorPair Amat = A.Matrix;

			// M = (D+L) D^{-1} (D+L)^T

			// Solves (1/omega)*(D+L) y = b
			for (int i = 0; i < n; ++i)
			{
				// Do nothing if we get a divide by zero
				if (Math.Abs(diag[i]) == 0.0)
					continue;

				double sum = 0;
				for (int j = Amat.indices[i], k = 0; j < diagInd[i]; ++j, ++k)
					sum += Amat.data[j]*xdata[k];
				xdata[i] = (omega/diag[i])*(xdata[i] - sum);
			}

			// Solves (omega/(2-omega))*D^{-1} z = y
			for (int i = 0; i < n; ++i)
			{
				// No need to do anything
				if (Math.Abs(diag[i]) == 0.0)
					continue;

				xdata[i] = (2 - omega)/omega*diag[i]*xdata[i];
			}

			// Solves (1/omega)*(D+L)^T x = z
			for (int i = n - 1; i >= 0; --i)
			{
				// Do nothing if we get a divide by zero
				if (Math.Abs(diag[i]) == 0.0)
					continue;

				double sum = 0;
				for (int j = diagInd[i] + 1, k = i + 1; j < n; ++j, ++k)
					sum += Amat.data[j]*xdata[k];
				xdata[i] = (omega/diag[i])*(xdata[i] - sum);
			}
		}

		public override IVector TransApply(IMatrix A, IVector b, IVector x)
		{
			return Apply(A, b, x);
		}

		public override void Setup(IMatrix A)
		{
			int n = A.RowCount;
			diag = new double[n];
			diagInd = new int[n];

			// Get the diagonal and its indices
			if (A is ISparseRowAccessMatrix)
			{
				for (int i = 0; i < n; ++i)
				{
					IntDoubleVectorPair Arow = ((ISparseRowAccessMatrix) A).GetRow(i);
					int ind = Array.BinarySearch(Arow.indices, i);
					if (ind < 0)
						throw new ArgumentException("Diagonal not present");
					diag[i] = Arow.data[ind];
					diagInd[i] = ind;
				}
			}
			else if (A is ISparseRowColumnAccessMatrix)
			{
				IntIntDoubleVectorTriple Amat = ((ISparseRowColumnAccessMatrix) A).Matrix;
				for (int i = 0; i < n; ++i)
				{
					int ind = Arrays.binarySearch(Amat.major, i, Amat.minor[i], Amat.minor[i + 1]);
					if (ind < 0)
						throw new ArgumentException("Diagonal not present");
					diag[i] = Amat.data[ind];
					diagInd[i] = ind;
				}
			}
			else if (A is IDenseRowColumnAccessMatrix)
			{
				IntDoubleVectorPair Amat = ((IDenseRowColumnAccessMatrix) A).Matrix;
				for (int i = 0; i < n; ++i)
				{
					diagInd[i] = Amat.indices[i] + i;
					diag[i] = Amat.data[diagInd[i]];
				}
			}
			else if (A is IDenseRowAccessMatrix)
			{
				for (int i = 0; i < n; ++i)
				{
					double[] Arow = ((IDenseRowAccessMatrix) A).GetRow(i);
					diagInd[i] = i;
					diag[i] = Arow[i];
				}
			}
			else throw new NotSupportedException();
		}
	}
}