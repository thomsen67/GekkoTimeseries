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
	/// <summary> Partial implementation of Preconditioner</summary>
	public abstract class AbstractPreconditioner : IPreconditioner
	{
		/// <summary> Points to jmp.State.BLAS</summary>
		protected internal IBLAS blas;

		/// <summary> Constructor for AbstractPreconditioner.</summary>
		public AbstractPreconditioner()
		{
			this.blas = Blas.Default;
		}

		/// <summary> Checks sizes of input data</summary>
		protected internal virtual void checkSizes(IMatrix A, IVector b, IVector x)
		{
			if (!A.Square)
				throw new IndexOutOfRangeException("!A.isSquare()");
			if (b.Length != A.RowCount)
				throw new IndexOutOfRangeException("b.size() != A.numRows()");
			if (b.Length != x.Length)
				throw new IndexOutOfRangeException("b.size() != x.size()");
		}

		public abstract IVector Apply(IMatrix param1, IVector param2, IVector param3);
		public abstract void Setup(IMatrix param1);
		public abstract IVector TransApply(IMatrix param1, IVector param2, IVector param3);
	}
}