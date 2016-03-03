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
	/// <summary> Complete LU factorization as preconditioner.
	/// It should only be used to precondition blocks of the matrix, as for
	/// instance in the BlockDiagonalPreconditioner. Transpose operation is not
	/// supported. </summary>
	public class LUPreconditioner : AbstractPreconditioner, IPreconditioner
	{
		/// <summary> The decomposition</summary>
		private LUDecomposition decomp;

		public override IVector Apply(IMatrix A, IVector b, IVector x)
		{
			double[] xs = (double[]) decomp.Solve(
				new Matrix(Blas.Default.GetArrayCopy(b), b.Length));
			return Blas.Default.SetVector(xs, x);
		}

		public override IVector TransApply(IMatrix A, IVector b, IVector x)
		{
			throw new NotSupportedException();
		}

		public override void Setup(IMatrix A)
		{
			decomp = new LUDecomposition(new Matrix(Blas.Default.GetArrayCopy(A)));
		}
	}
}