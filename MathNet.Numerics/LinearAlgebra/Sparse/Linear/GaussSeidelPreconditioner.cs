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
	/// <summary> Gauss-Seidel preconditioner.
	/// Applies one sweep of Gauss-Seidel to the system matrix.
	/// Does not perform transpose operations.
	/// </summary>
	public class GaussSeidelPreconditioner : AbstractPreconditioner, IPreconditioner
	{
		public override IVector Apply(IMatrix A, IVector b, IVector x)
		{
			checkSizes(A, b, x);

			if (!(b is IDenseAccessVector) || !(x is IDenseAccessVector))
				throw new NotSupportedException();

			double[] bdata = ((IDenseAccessVector) b).Vector, xdata = ((IDenseAccessVector) x).Vector;

			if (A is ISparseRowAccessMatrix)
				gaussSeidel((ISparseRowAccessMatrix) A, bdata, xdata);
			else if (A is ISparseRowColumnAccessMatrix)
				gaussSeidel((ISparseRowColumnAccessMatrix) A, bdata, xdata);
			else if (A is IDenseRowAccessMatrix)
				gaussSeidel((IDenseRowAccessMatrix) A, bdata, xdata);
			else if (A is IDenseRowColumnAccessMatrix)
				gaussSeidel((IDenseRowColumnAccessMatrix) A, bdata, xdata);
			else
				throw new NotSupportedException();

			return x;
		}

		public override IVector TransApply(IMatrix A, IVector b, IVector x)
		{
			throw new NotSupportedException();
		}

		public override void Setup(IMatrix A)
		{
		}

		private void gaussSeidel(ISparseRowAccessMatrix A, double[] bdat, double[] xdat)
		{
			for (int i = 0; i < A.RowCount; ++i)
			{
				IntDoubleVectorPair Arow = A.GetRow(i);
				int[] ArowInd = Arow.indices;
				double[] ArowDat = Arow.data;

				double newVal = 0, diagVal = 0.0;
				for (int j = 0; j < ArowInd.Length; ++j)
					if (ArowInd[j] != i)
						newVal += ArowDat[j]*xdat[ArowInd[j]];
					else
						diagVal = ArowDat[j];

				xdat[i] = (bdat[i] - newVal)/diagVal;
			}
		}

		private void gaussSeidel(ISparseRowColumnAccessMatrix A, double[] bdat, double[] xdat)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;

			for (int i = 0; i < A.RowCount; ++i)
			{
				double newVal = 0, diagVal = 0.0;
				for (int j = Amat.minor[i]; j < Amat.minor[i + 1]; ++j)
					if (Amat.major[j] != i)
						newVal += Amat.data[j]*xdat[Amat.major[j]];
					else
						diagVal = Amat.data[j];

				xdat[i] = (bdat[i] - newVal)/diagVal;
			}
		}

		private void gaussSeidel(IDenseRowAccessMatrix A, double[] bdata, double[] xdata)
		{
			for (int i = 0; i < A.RowCount; ++i)
			{
				double[] Arow = A.GetRow(i);

				double newVal = 0, diagVal = 0.0;
				for (int j = 0; j < Arow.Length; ++j)
					if (j != i)
						newVal += Arow[j]*xdata[j];
					else
						diagVal = Arow[j];

				xdata[i] = (bdata[i] - newVal)/diagVal;
			}
		}

		private void gaussSeidel(IDenseRowColumnAccessMatrix A, double[] bdata, double[] xdata)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			for (int i = 0; i < A.RowCount; ++i)
			{
				double newVal = 0, diagVal = 0.0;
				for (int j = Amat.indices[i], k = 0; j < Amat.indices[i + 1]; ++j, ++k)
					if (k != i)
						newVal += Amat.data[j]*xdata[k];
					else
						diagVal = Amat.data[j];

				xdata[i] = (bdata[i] - newVal)/diagVal;
			}
		}
	}
}