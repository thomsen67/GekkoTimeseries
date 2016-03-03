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
	/// <summary> Composite preconditioner.
	/// Applies several preconditioners in sequence serially. If the preconditioners
	/// have already been set up, it is not necessary to call the setup method
	/// in this class, as it simply calls setup() on every preconditioner.
	/// </summary>
	public class CompositePreconditioner : AbstractPreconditioner, IPreconditioner
	{
		/// <summary> The preconditioners which will be applied</summary>
		private IPreconditioner[] preconditioner;

		/// <summary> Constructor for CompositePreconditioner.</summary>
		/// <param name="preconditioner">The preconditioners to apply.</param>
		public CompositePreconditioner(IPreconditioner[] preconditioner)
		{
			this.preconditioner = preconditioner;
		}

		public override IVector Apply(IMatrix A, IVector b, IVector x)
		{
			for (int i = 0; i < preconditioner.Length; ++i)
				x = preconditioner[i].Apply(A, b, x);
			return x;
		}

		public override IVector TransApply(IMatrix A, IVector b, IVector x)
		{
			for (int i = 0; i < preconditioner.Length; ++i)
				x = preconditioner[i].TransApply(A, b, x);
			return x;
		}

		public override void Setup(IMatrix A)
		{
			for (int i = 0; i < preconditioner.Length; ++i)
				preconditioner[i].Setup(A);
		}
	}
}