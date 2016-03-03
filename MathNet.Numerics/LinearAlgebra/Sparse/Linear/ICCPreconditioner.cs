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
	/// <summary> An ICC preconditioner.
	/// Uses an incomplete Cholesky decomposition as preconditioner. Applicable to
	/// symmetrical, positive definite problems.
	/// </summary>
	public class ICCPreconditioner : DecompositionPreconditioner, IPreconditioner
	{
		/// <summary> Constructor for ICC.</summary>
		public ICCPreconditioner(ISparseRowAccessMatrix F) : base(F)
		{
		}

		public override IVector Apply(IMatrix A, IVector b, IVector x)
		{
			checkSizes(A, b, x);

			// Copy b into x, and get the array
			x = Blas.Default.Copy(b, x);
			double[] xdat = ((IDenseAccessVector) x).Vector;

			// First solve F^T y=b, then Fx=y
			SolveUT(xdat, true);
			SolveU(xdat, true);

			// Put xdat back into x
			((IDenseAccessVector) x).Vector = xdat;

			return x;
		}

		public override IVector TransApply(IMatrix A, IVector b, IVector x)
		{
			return Apply(A, b, x);
		}

		protected internal override void Factor()
		{
			double diagMod = initShift;
			int n = F.RowCount;
			this.diagInd = new int[n];
			double[] actRow = new double[n];

			//UPGRADE_NOTE: Label 'outer' was moved. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1014_3"'
			while (true)
			{
				// Copy A into F, and ensure non-zero diagonal
				Blas.Default.AddDiagonal(diagMod, Blas.Default.Copy(A, F));

				// Extract the upper triangular part
				for (int i = 0; i < n; ++i)
				{
					IntDoubleVectorPair curRow = F.GetRow(i);
					int diagInd = Array.BinarySearch(curRow.indices, i);
					if (diagInd < 0)
					{
						diagMod += shift;
						//UPGRADE_NOTE: Labeled continue statement was changed to a goto statement. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1015_3"'
						goto outer;
					}
					int[] index = new int[curRow.indices.Length - diagInd];
					double[] data = new double[curRow.data.Length - diagInd];
					Array.Copy(curRow.indices, diagInd, index, 0, index.Length);
					Array.Copy(curRow.data, diagInd, data, 0, data.Length);
					F.SetRow(i, new IntDoubleVectorPair(index, data));
				}

				// Factorize
				for (int i = 0; i < n; ++i)
				{
					// Get current row
					IntDoubleVectorPair curRow = F.GetRow(i);

					// We must have positive entries on the diagonal
					if (curRow.data.Length == 0 || curRow.data[0] <= 0.0)
					{
						diagMod += shift;
						//UPGRADE_NOTE: Labeled continue statement was changed to a goto statement. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1015_3"'
						goto outer;
					}

					// Divide by rooted diagonal
					double diag = curRow.data[0] = Math.Sqrt(curRow.data[0]);
					for (int j = 1; j < curRow.data.Length; ++j)
						curRow.data[j] /= diag;
					F.SetRow(i, curRow);

					// Reduce submatrix using current row
					for (int j = 1; j < curRow.indices.Length; ++j)
					{
						// Active row in dense format
						IntDoubleVectorPair actRowI = F.GetRow(curRow.indices[j]);
						actRow = Blas.Default.Scatter(actRowI, actRow);

						// Reduce active row
						double factor = curRow.data[j];
						for (int k = j; k < curRow.data.Length; ++k)
							actRow[curRow.indices[k]] -= factor*curRow.data[k];

						// Put updated row back into the factor matrix
						// We either keep or discard filled in values
						if (fill)
							F.SetRow(i, Blas.Default.Gather(actRow));
						else
							F.SetRow(i, Blas.Default.Gather(actRowI.indices, actRow));
					}
				}

				// No errors during factorization
				break;
				//UPGRADE_NOTE: Label 'outer' was moved. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1014_3"'

				outer:
				;
			}
		}
	}
}