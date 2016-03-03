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

namespace MathNet.Numerics.LinearAlgebra.Sparse.Linear
{
	/// <summary> Iterative Refinement.
	/// IR solves the unsymmetric linear system <c>Ax = b</c> using
	/// Iterative Refinement (preconditioned Richardson iteration).
	/// </summary>
	/// <author>  Templates </author>
	public class IRSolver : AbstractLinearSolver, ILinearSolver
	{
		protected internal override void SolveI(IMatrix A, IVector b, IVector x)
		{
			IVector[] temp = Factory.createVectors(b, 2);
			IVector z = temp[0], r = temp[1];

			r = Blas.Default.MultAdd(- 1.0, A, x, b, r);

			for (iter.Reset(); !iter.Converged(r, x); iter.MoveNext())
			{
				z = M.Apply(A, r, z);
				x = Blas.Default.Add(z, x);
				r = Blas.Default.MultAdd(- 1.0, A, x, b, r);
			}
		}
	}
}