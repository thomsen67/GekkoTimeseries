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

using MathNet.Numerics.LinearAlgebra.Sparse;
using MathNet.Numerics.LinearAlgebra.Sparse.Utilities;

namespace MathNet.Numerics.LinearAlgebra.Sparse.Linear
{
	/// <summary> ILU preconditioner.
	/// Performs an incomplete LU decomposition as a preconditioner. Applicable to
	/// unsymmetrical problems.
	/// </summary>
	public class ILUPreconditioner : DecompositionPreconditioner, IPreconditioner
	{
		/// <summary> Constructor for ILU.</summary>
		public ILUPreconditioner(ISparseRowAccessMatrix F) : base(F)
		{
		}

		public override IVector Apply(IMatrix A, IVector b, IVector x)
		{
			checkSizes(A, b, x);

			// Copy b into x, and get the array
			x = Blas.Default.Copy(b, x);
			double[] xdat = ((IDenseAccessVector) x).Vector;

			// Solve L*U
			solveL(xdat, false);
			SolveU(xdat, true);

			// Put xdat back into x
			((IDenseAccessVector) x).Vector = xdat;

			return x;
		}

		public override IVector TransApply(IMatrix A, IVector b, IVector x)
		{
			checkSizes(A, b, x);

			// Copy b into x, and get the array
			x = Blas.Default.Copy(b, x);
			double[] xdat = ((IDenseAccessVector) x).Vector;

			// Solve U'*L'
			SolveUT(xdat, true);
			SolveLT(xdat, false);

			// Put xdat back into x
			((IDenseAccessVector) x).Vector = xdat;

			return x;
		}

		protected internal override void Factor()
		{
			double diagMod = initShift;
			int n = A.RowCount;
			diagInd = new int[n];
			double[] curRow = new double[n];

			//UPGRADE_NOTE: Label 'outer' was moved. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1014_3"'
			while (true)
			{
				// Copy A into F, and ensure non-zero diagonal
				Blas.Default.AddDiagonal(diagMod, Blas.Default.Copy(A, F));

				// Clear cache of diagonal indices
				SupportClass.ArraySupport.Fill(diagInd, - 1);
				GetDiagIndex(0);

				// Row-based Gaussian elimination
				for (int i = 1; i < n; ++i)
				{
					// Get current row in dense format
					IntDoubleVectorPair curRowI = F.GetRow(i);
					curRow = Blas.Default.Scatter(curRowI, curRow);

					// Check for empty row
					if (curRowI.indices.Length == 0)
					{
						diagMod += shift;
						//UPGRADE_NOTE: Labeled continue statement was changed to a goto statement. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1015_3"'
						goto outer;
					}

					// Traverse all active rows
					for (int j = 0; j < curRowI.indices.Length && curRowI.indices[j] < i; ++j)
					{
						IntDoubleVectorPair actRow = F.GetRow(curRowI.indices[j]);

						// Check for missing or zero diagonal
						int diag = GetDiagIndex(curRowI.indices[j], actRow.indices);
						if (diag < 0 || actRow.data[diag] == 0)
						{
							diagMod += shift;
							//UPGRADE_NOTE: Labeled continue statement was changed to a goto statement. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1015_3"'
							goto outer;
						}

						// Get multiplier (to be stored in L)
						double mult = (- curRow[curRowI.indices[j]])/actRow.data[diag];
						curRow[curRowI.indices[j]] = - mult;

						// Reduce everything on the upper diagonal of current row
						for (int k = diag + 1; k < actRow.indices.Length; ++k)
							curRow[actRow.indices[k]] += mult*actRow.data[k];
					}

					// Put updated row back into the factor matrix
					// We either keep or discard filled in values
					if (fill)
						F.SetRow(i, Blas.Default.Gather(curRow));
					else
						F.SetRow(i, Blas.Default.Gather(curRowI.indices, curRow));
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