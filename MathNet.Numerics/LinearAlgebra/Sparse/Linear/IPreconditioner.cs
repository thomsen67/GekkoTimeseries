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
	/// <summary> Preconditioner interface.
	/// Preconditioners are approximate solvers to some problem. They may be used
	/// in iterative solution procedures to speed up convergence.
	/// <p>
	/// The only method the user needs to call from this interface is setup.
	/// Before using the preconditioner in a LinearSolver, setup must have been
	/// called.
	/// The advantage of this is that the same internal structure of the
	/// preconditioner can be reused for repeated solves with the same matrix.
	/// This is particularly important for complicated preconditioners such as
	/// ILU or BlockDiagonalPreconditioner.
	/// </p>
	/// </summary>
	public interface IPreconditioner
	{
		/// <summary> Solves the approximate problem with the given right hand side.
		/// Result is stored in given vector.</summary>
		/// <param name="A">Matrix to precondition</param>
		/// <param name="b">Right hand side of problem</param>
		/// <param name="x">Result is stored here</param>
		/// <returns>x</returns>
		IVector Apply(IMatrix A, IVector b, IVector x);

		/// <summary> Solves the approximate transpose problem with the given right hand side.
		/// Result is stored in given vector.</summary>
		/// <param name="A">Matrix to precondition</param>
		/// <param name="b">Right hand side of problem</param>
		/// <param name="x">Result is stored here</param>
		/// <returns>x</returns>
		IVector TransApply(IMatrix A, IVector b, IVector x);

		/// <summary> Sets up the preconditioner on the given matrix. Must be called before
		/// a system preconditioning is applied.</summary>
		/// <param name="A">Matrix to work on</param>
		void Setup(IMatrix A);
	}
}