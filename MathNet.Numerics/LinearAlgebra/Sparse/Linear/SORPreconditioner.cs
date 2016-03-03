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
	/// <summary> SOR preconditioner.
	/// Applies one sweep of SOR to the system matrix. Sequential. For best
	/// performance, a good choice of the overrelaxation parameter omega must be
	/// made (0 &lt; omega &lt; 2). Does not perform transpose operations.
	/// </summary>
	public class SORPreconditioner : AbstractPreconditioner, IPreconditioner
	{
		/// <summary> Sets over-relaxation parameter.</summary>
		public virtual double Parameters
		{
			set { this.omega = value; }

		}

		/// <summary> Overrelaxation parameter</summary>
		private double omega;

		/// <summary> Constructor for SOR. Uses omega=1 by default.</summary>
		public SORPreconditioner()
		{
			omega = 1;
		}

		public override IVector Apply(IMatrix A, IVector b, IVector x)
		{
			checkSizes(A, b, x);

			if (!(b is IDenseAccessVector) || !(x is IDenseAccessVector))
				throw new NotSupportedException();

			double[] bdata = ((IDenseAccessVector) b).Vector, xdata = ((IDenseAccessVector) x).Vector;

			if (A is ISparseRowAccessMatrix)
				sor((ISparseRowAccessMatrix) A, bdata, xdata);
			else if (A is ISparseRowColumnAccessMatrix)
				sor((ISparseRowColumnAccessMatrix) A, bdata, xdata);
			else if (A is IDenseRowAccessMatrix)
				sor((IDenseRowAccessMatrix) A, bdata, xdata);
			else if (A is IDenseRowColumnAccessMatrix)
				sor((IDenseRowColumnAccessMatrix) A, bdata, xdata);
			else
				throw new NotSupportedException();

			return x;
		}

		private void sor(ISparseRowAccessMatrix A, double[] bdata, double[] xdata)
		{
			for (int i = 0; i < A.RowCount; ++i)
			{
				IntDoubleVectorPair Arow = A.GetRow(i);

				double newVal = 0, diagVal = 0.0;
				for (int j = 0; j < Arow.indices.Length; ++j)
					if (Arow.indices[j] != i)
						newVal += Arow.data[j]*xdata[Arow.indices[j]];
					else
						diagVal = Arow.data[j];
				newVal = (bdata[i] - newVal)/diagVal;
				xdata[i] += omega*(newVal - xdata[i]);
			}
		}

		private void sor(ISparseRowColumnAccessMatrix A, double[] bdata, double[] xdata)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;

			for (int i = 0; i < A.RowCount; ++i)
			{
				double newVal = 0, diagVal = 0.0;
				for (int j = Amat.minor[i]; j < Amat.minor[i + 1]; ++j)
					if (Amat.major[j] != i)
						newVal += Amat.data[j]*xdata[Amat.major[j]];
					else
						diagVal = Amat.data[j];
				newVal = (bdata[i] - newVal)/diagVal;
				xdata[i] += omega*(newVal - xdata[i]);
			}
		}

		private void sor(IDenseRowAccessMatrix A, double[] bdata, double[] xdata)
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
				newVal = (bdata[i] - newVal)/diagVal;
				xdata[i] += omega*(newVal - xdata[i]);
			}
		}

		private void sor(IDenseRowColumnAccessMatrix A, double[] bdata, double[] xdata)
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
				newVal = (bdata[i] - newVal)/diagVal;
				xdata[i] += omega*(newVal - xdata[i]);
			}
		}

		public override IVector TransApply(IMatrix A, IVector b, IVector x)
		{
			throw new NotSupportedException();
		}

		public override void Setup(IMatrix A)
		{
		}
	}
}