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

namespace MathNet.Numerics.LinearAlgebra.Sparse.Linear
{
	/// <summary> Partial preconditioner for decomposition based methods.
	/// Contains some methods to make incomplete decomposition based preconditioners
	/// easier to implement, such as ILU and ICC.</summary>
	public abstract class DecompositionPreconditioner : AbstractPreconditioner, IPreconditioner
	{
		/// <summary> Sets decomposition matrix</summary>
		public virtual ISparseRowAccessMatrix DecompositionMatrix
		{
			set { this.F = value; }
		}

		/// <summary> The decomposition matrix</summary>
		protected internal ISparseRowAccessMatrix F;

		/// <summary> Original matrix reference</summary>
		protected internal IMatrix A;

		/// <summary> Diagonal indices to the decomposition matrix F.</summary>
		protected internal int[] diagInd;

		/// <summary> Diagonal shift</summary>
		protected internal double shift;

		/// <summary> Initial diagonal shift</summary>
		protected internal double initShift;

		/// <summary> Wheter or not to allow fill</summary>
		protected internal bool fill;

		/// <summary> Constructor for DecompositionPreconditioner. Uses a default value of
		/// 0.1 as shift, and disallows fill.</summary>
		/// <param name="F">Matrix to contain the decomposition.</param>
		public DecompositionPreconditioner(ISparseRowAccessMatrix F)
		{
			this.F = F;
			shift = 0.1;
		}

		/// <summary> Sets preconditioner parameters</summary>
		/// <param name="initShift">Initial diagonal shift. Default is 0.</param>
		/// <param name="shift">Diagonal shift to apply in case of zero-pivot. 
		/// Default is <c>0.1</c>.</param>
		/// <param name="fill">Allow fill-in? Default is false.</param>
		public virtual void setParamters(double initShift, double shift, bool fill)
		{
			this.initShift = initShift;
			this.shift = shift;
			this.fill = fill;
		}

		public override void Setup(IMatrix A)
		{
			this.A = A;
			Factor();
			for (int i = 0; i < F.RowCount; ++i)
				GetDiagIndex(i);
		}

		/// <summary> This method should create the factor F of A. When it is called, A is set,
		/// but not A. After the call, diagInd will be set.</summary>
		protected internal abstract void Factor();

		/// <summary> F is lower-triangular, and it is solved for.</summary>
		/// <param name="data">Initially the right hand side. Overwritten with solution.</param>
		/// <param name="diagDiv">True if the diagonal will be divided with.</param>
		protected internal virtual void solveL(double[] data, bool diagDiv)
		{
			for (int i = 0; i < F.RowCount; ++i)
			{
				IntDoubleVectorPair cur = F.GetRow(i);
				int[] curInd = cur.indices;
				double[] curDat = cur.data;

				double sum = 0;
				int j = 0;
				for (; curInd[j] < i; ++j)
					sum += curDat[j]*data[curInd[j]];

				data[i] -= sum;

				// Divide by diagonal. The factorization guarantees its existence
				if (diagDiv)
					data[i] /= curDat[j];
			}
		}

		/// <summary> F is upper-triangular, and it is solved for.</summary>
		/// <param name="data">Initially the right hand side. Overwritten with solution.</param>
		/// <param name="diagDiv">True if the diagonal will be divided with.</param>
		protected internal virtual void SolveU(double[] data, bool diagDiv)
		{
			for (int i = F.RowCount - 1; i >= 0; --i)
			{
				IntDoubleVectorPair cur = F.GetRow(i);
				int[] curInd = cur.indices;
				double[] curDat = cur.data;

				double sum = 0;
				for (int j = diagInd[i] + 1; j < curInd.Length; ++j)
					sum += curDat[j]*data[curInd[j]];

				data[i] -= sum;

				// Divide by diagonal. The factorization guarantees its existence
				if (diagDiv)
					data[i] /= curDat[diagInd[i]];
			}
		}

		/// <summary> F is lower-triangular, and F<sup>T</sup> is solved for.</summary>
		/// <param name="data">Initially the right hand side. Overwritten with solution.</param>
		/// <param name="diagDiv">True if the diagonal will be divided with.</param>
		protected internal virtual void SolveLT(double[] data, bool diagDiv)
		{
			for (int i = F.RowCount - 1; i >= 0; --i)
			{
				IntDoubleVectorPair cur = F.GetRow(i);
				int[] curInd = cur.indices;
				double[] curDat = cur.data;

				// Solve at current position
				if (diagDiv)
					data[i] /= curDat[diagInd[i]];
				double val = data[i];

				// Move over to right hand side
				for (int j = 0; curInd[j] < i; ++j)
					data[curInd[j]] -= curDat[j]*val;
			}
		}

		/// <summary> F is upper-triangular, and F<sup>T</sup> is solved for.</summary>
		/// <param name="data">Initially the right hand side. Overwritten with solution.</param>
		/// <param name="diagDiv">True if the diagonal will be divided with.</param>
		protected internal virtual void SolveUT(double[] data, bool diagDiv)
		{
			for (int i = 0; i < F.RowCount; ++i)
			{
				IntDoubleVectorPair cur = F.GetRow(i);
				int[] curInd = cur.indices;
				double[] curDat = cur.data;

				// Solve at current position
				if (diagDiv)
					data[i] /= curDat[diagInd[i]];
				double val = data[i];

				// Move over to right hand side
				for (int j = diagInd[i] + 1; j < curInd.Length; ++j)
					data[curInd[j]] -= curDat[j]*val;
			}
		}

		/// <summary> Gets diagonal index from cache (if it exists there), or updates the
		/// cache with diagonal index.</summary>
		/// <param name="row">Row to get diagonal index for.</param>
		/// <returns> diagInd[row] </returns>
		protected internal virtual int GetDiagIndex(int row)
		{
			return GetDiagIndex(row, F.GetRow(row).indices);
		}

		/// <summary> Gets diagonal index from cache (if it exists there), or updates the
		/// cache with diagonal index.</summary>
		/// <param name="row">Row to get diagonal index for.</param>
		/// <param name="rowInd">The row indices to search.</param>
		/// <returns> diagInd[row] </returns>
		protected internal virtual int GetDiagIndex(int row, int[] rowInd)
		{
			if (diagInd[row] < 0)
				diagInd[row] = Array.BinarySearch(rowInd, row);
			return diagInd[row];
		}
	}
}