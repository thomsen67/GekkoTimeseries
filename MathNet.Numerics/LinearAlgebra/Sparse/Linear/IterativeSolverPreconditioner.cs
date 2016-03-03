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
	/// <summary> Applies an iterative linear solver as preconditioner.
	/// Only for transpose-free problems. The convergence criteria for the
	/// preconditioner should not be too stringent as that can defeat the purpose of
	/// the preconditioner.
	/// <p>
	/// Note that if the solver doesn't converge, the exceptions are caught
	/// and promptly ignored.
	/// </p>
	/// </summary>
	public class IterativeSolverPreconditioner : AbstractPreconditioner, IPreconditioner
	{
		/// <summary> LinearSolver to use</summary>
		private ILinearSolver preconditioner;

		/// <summary> Assigns the solver.</summary>
		public IterativeSolverPreconditioner(ILinearSolver preconditioner)
		{
			this.preconditioner = preconditioner;
		}

		public override IVector Apply(IMatrix A, IVector b, IVector x)
		{
			try
			{
				return preconditioner.Solve(A, b, x);
			}
			catch (NotConvergedException)
			{
				// This is ignored. In fact, we probably don't want nor need
				// convergence here.
			}
			return x;
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