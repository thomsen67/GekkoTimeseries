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
using MathNet.Numerics.LinearAlgebra.Sparse.Linear;

namespace MathNet.Numerics.LinearAlgebra.Sparse.Mixed
{
	/// <summary> Partial implementation of MixedSolver</summary>
	public abstract class AbstractMixedSolver : IMixedSolver
	{
		public virtual ILinearSolver SubSolver
		{
			get { return solver; }
			set { this.solver = value; }
		}

		public virtual ILinearIteration Iteration
		{
			get { return iter; }
			set { this.iter = value; }

		}

		/// <summary> Local reference</summary>
		protected internal IBLAS blas;

		/// <summary> LinearSolver to use</summary>
		protected internal ILinearSolver solver;

		/// <summary> Convergence tester</summary>
		protected internal ILinearIteration iter;

		/// <summary> Constructor for AbstractMixedSolver, with CG as solver and default
		/// linear iteration object.</summary>
		public AbstractMixedSolver()
		{
			this.iter = new DefaultLinearIteration();
			this.solver = new CGSolver();
			blas = Blas.Default;
		}

		public virtual void solve(IMatrix A, IMatrix B, IMatrix Bt, IMatrix C, 
			IVector q, IVector u, IVector f, IVector g)
		{
			checkSolveArguments(A, B, Bt, C, q, u, f, g);
			solveI(A, B, Bt, C, q, u, f, g);
		}

		/// <summary> Checks that all sizes conform, or else throws an exception.</summary>
		private void checkSolveArguments(IMatrix A, IMatrix B, IMatrix Bt, IMatrix C,
			IVector q, IVector u, IVector f, IVector g)
		{
			int m = B.RowCount;
			int n = B.ColumnCount;

			if (n != A.ColumnCount)
				throw new IndexOutOfRangeException("B.numColumns() != A.numColumns()");
			if (q.Length != n)
				throw new IndexOutOfRangeException("q.size() != B.numColumns()");
			if (u.Length != m)
				throw new IndexOutOfRangeException("u.size() != B.numRows()");
			if (f.Length != n)
				throw new IndexOutOfRangeException("f.size() != B.numColumns()");
			if (g.Length != m)
				throw new IndexOutOfRangeException("g.size() != B.numRows()");
			if (Bt.RowCount != n || Bt.ColumnCount != m)
				throw new IndexOutOfRangeException(
					"Bt.numRows() != B.numColumns() || Bt.numColumns() != B.numRows()");
			if (!A.Square)
				throw new IndexOutOfRangeException("!A.isSquare()");
			if (!C.Square)
				throw new IndexOutOfRangeException("!C.isSquare()");
			if (C.RowCount != m)
				throw new IndexOutOfRangeException("C.numRows() != B.numRows()");
		}

		protected internal abstract void solveI(IMatrix A, IMatrix B, IMatrix Bt, 
			IMatrix C, IVector q, IVector u, IVector f, IVector g);
	}
}