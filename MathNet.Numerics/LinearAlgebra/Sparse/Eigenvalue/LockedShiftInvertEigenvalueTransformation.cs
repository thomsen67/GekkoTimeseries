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

namespace MathNet.Numerics.LinearAlgebra.Sparse.Eigenvalue
{
	/// <summary> Like ShiftInvertEigenvalueTransformation, but locks the shift</summary>
	public class LockedShiftInvertEigenvalueTransformation : 
		ShiftInvertEigenvalueTransformation, IEigenvalueTransformation
	{
		/// <summary> Constructor for LockedShiftInvertEigenvalueTransformation. The matrix Am
		/// must be of the same size as A, and it is overwritten. The given shift
		/// is fixed.</summary>
		public LockedShiftInvertEigenvalueTransformation(IMatrix Am, IMatrix A, double shift) : base(Am, A)
		{
			this.sigma = shift;
			Am = Blas.Default.AddDiagonal(- sigma, Am);
		}

		public override IVector Apply(IMatrix A, IVector x, IVector y)
		{
			return solver_.Solve(Am, x, y);
		}

		public override double Shift
		{
			get { return base.Shift; }
			set
			{
			}
		}
	}
}