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
	/// <summary> Krylov subspace linear solver.
	/// Interface to a generic iterative Krylov based system solver. It solves the
	/// linear problem <code>Ax=b</code> for x, using preconditioning and convergence
	/// monitoring.
	/// </summary>
	public interface ILinearSolver
	{
		/// <summary>Gets or sets the preconditioner.</summary>
		IPreconditioner Preconditioner { get; set; }

		/// <summary>Gets or sets the iteration.</summary>
		ILinearIteration Iteration { get; set; }

		/// <summary> Solves the given problem, writing result into the vector.</summary>
		/// <param name="A">Matrix of the problem.</param>
		/// <param name="b">Right hand side.</param>
		/// <param name="x">Solution is stored here. Also used as initial guess.</param>
		/// <returns> The solution vector x.</returns>
		IVector Solve(IMatrix A, IVector b, IVector x);
	}
}