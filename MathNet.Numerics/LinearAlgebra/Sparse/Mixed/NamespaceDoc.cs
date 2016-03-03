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

namespace MathNet.Numerics.LinearAlgebra.Sparse.Mixed
{
	/// <summary>
	/// Iterative mixed solvers. The mixed problem is
	/// <code>
	/// A q + B' u = f
	/// B q + C  u = g
	/// </code>
	/// <p>where <c>A</c> is symmetrical, positive definite. While a
	/// mixed problem can be formulated as a linear problem, a mixed
	/// solver can often be more efficient.</p>
	/// <p>The basic interface is <see cref="IMixedSolver"/> and there 
	/// are three implementations:</p>
	/// <ul>
	///		<li><see cref="UzawaSolver"/> - basic Uzawa algorithm, requires a steplength.</li>
	///		<li><see cref="GradientUzawaSolver"/> - Uzawa algorithm with gradient descent.</li>
	///		<li><see cref="CGUzawaSolver"/> - Uzawa algorithm using a conjugate gradient descent.</li>
	///	</ul>
	///	<p>The last method is often the fastest. All these use a solver for	the <c>A</c> matrix, 
	///	by default <see cref="MathNet.Numerics.LinearAlgebra.Sparse.Linear.CGSolver"/> 
	///	is used, but different solvers can be set using <see cref="IMixedSolver.SubSolver"/>.</p>
	/// </summary>
	public class NamespaceDoc
	{
		// for documentation only
	}
}
