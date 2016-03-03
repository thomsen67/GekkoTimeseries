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
	/// <summary> Multigrid preconditioner.
	/// Uses a geometric multigrid, thus the user needs to create matrix operators
	/// and restriction operators. The preconditioner supports different kinds
	/// of cycles (V, W etc), and the smoothers can be chosen by the user.
	/// </summary>
	public class MultigridPreconditioner : AbstractPreconditioner, IPreconditioner
	{
		/// <summary> Restriction operators</summary>
		private IMatrix[] restrict;

		/// <summary> Matrices on each grid (finer to coarser)</summary>
		private IMatrix[] subA;

		/// <summary> Solvers on each grid</summary>
		private IPreconditioner[] subSolver;

		/// <summary> Number of pre-, mid, and postsmoothings</summary>
		private int pre, mid, post;

		/// <summary> Number of sublevels</summary>
		private int numLevels;

		/// <summary> Constructor for Multigrid.</summary>
		/// <param name="subA">Coefficient matrix for each level (including the finest).</param>
		/// <param name="restrict">Restriction operators.</param>
		/// <param name="subSolver">Solvers at each level.</param>
		/// <param name="pre">Number of pre-smoothings.</param>
		/// <param name="mid">Number of descents (1=V-cycle, 2=W-cycle etc).</param>
		/// <param name="post">Number of post-smoothings.</param>
		public MultigridPreconditioner(IMatrix[] subA, IMatrix[] restrict, 
			IPreconditioner[] subSolver, int pre, int mid, int post)
		{
			this.subA = subA;
			this.restrict = restrict;
			this.subSolver = subSolver;
			this.pre = pre;
			this.mid = mid;
			this.post = post;
			numLevels = subA.Length;

			check();
		}

		/// <summary> Constructor for Multigrid. Uses pre=mid=post=1.</summary>
		/// <param name="subA">Coefficient matrix for each level (including the finest).</param>
		/// <param name="restrict">Restriction operators.</param>
		/// <param name="subSolver">Solvers at each level.</param>
		public MultigridPreconditioner(IMatrix[] subA, IMatrix[] restrict, IPreconditioner[] subSolver) 
			: this(subA, restrict, subSolver, 1, 1, 1)
		{
		}

		/// <summary> Constructor for Multigrid. Uses pre=mid=post=1 and GaussSeidel
		/// subsolvers.</summary>
		/// <param name="subA">Coefficient matrix for each level (including the finest).</param>
		/// <param name="restrict">Restriction operators.</param>
		public MultigridPreconditioner(IMatrix[] subA, IMatrix[] restrict)
		{
			this.subA = subA;
			this.restrict = restrict;
			pre = mid = post = 1;
			numLevels = subA.Length;
			subSolver = new GaussSeidelPreconditioner[numLevels];
			for (int i = 0; i < numLevels; ++i)
				subSolver[i] = new GaussSeidelPreconditioner();

			check();
		}

		/// <summary> Checks for internal consistency</summary>
		private void check()
		{
			// Check that we have the correct number of everything
			if (subA.Length != numLevels)
				throw new IndexOutOfRangeException("subA.length != numLevels");
			if (restrict.Length != numLevels - 1)
				throw new IndexOutOfRangeException("restrict.length != numLevels-1");
			if (subSolver.Length != numLevels)
				throw new IndexOutOfRangeException("subSolver.length != numLevels");

			// Every subA matrix must be square
			for (int i = 0; i < numLevels; ++i)
				if (!subA[i].Square)
					throw new IndexOutOfRangeException("subA[i].isSquare() != true");

			// Check that the sizes of the restriction operators are correct
			for (int i = 0; i < numLevels - 1; ++i)
				if (restrict[i].RowCount != subA[i + 1].RowCount)
					throw new IndexOutOfRangeException("restrict[i].numRows() != subA[i + 1].numRows()");
				else if (restrict[i].ColumnCount != subA[i].ColumnCount)
					throw new IndexOutOfRangeException("restrict[i].numColumns() != subA[i - 1].numColumns()");

			// Check that the matrix sizes decrease
			for (int i = 0; i < numLevels - 1; ++i)
				if (subA[i].RowCount < subA[i + 1].RowCount)
					throw new IndexOutOfRangeException("subA[i].numRows() < subA[i + 1].numRows() ");
		}

		public override IVector Apply(IMatrix A, IVector b, IVector x)
		{
			checkSizes(A, b, x);
			cycle(0, b, x);
			return x;
		}

		public override IVector TransApply(IMatrix A, IVector b, IVector x)
		{
			checkSizes(A, b, x);
			transCycle(0, b, x);
			return x;
		}

		public override void Setup(IMatrix A)
		{
			for (int i = 0; i < numLevels; ++i)
				subSolver[i].Setup(subA[i]);
		}

		/// <summary> Performs recursive multigrid cycle</summary>
		private void cycle(int level, IVector b, IVector x)
		{
			// Pre-smoothings
			for (int i = 0; i < pre; ++i)
				subSolver[level].Apply(subA[level], b, x);

			// Decend if possible
			if (level < numLevels - 1)
			{
				IVector r = Factory.createVector(b), rs = Factory.createVector(b, restrict[level].RowCount), xs = Factory.createVector(b, restrict[level].RowCount);

				// Compute residual, and restrict it
				r = Blas.Default.MultAdd(- 1.0, subA[level], x, b, r);
				rs = Blas.Default.Mult(restrict[level], r, rs);

				// Smooth residual
				for (int i = 0; i < mid; ++i)
					cycle(level + 1, rs, xs);

				// Prolongate and apply correction
				x = Blas.Default.TransMultAdd(restrict[level], xs, x);
			}

			// Post-smoothings
			for (int i = 0; i < post; ++i)
				subSolver[level].Apply(subA[level], b, x);
		}

		/// <summary> Performs recursive multigrid cycle for transpose problem</summary>
		private void transCycle(int level, IVector b, IVector x)
		{
			// Pre-smoothings
			for (int i = 0; i < pre; ++i)
				subSolver[level].TransApply(subA[level], b, x);

			// Decend if possible
			if (level < numLevels)
			{
				IVector r = Factory.createVector(b), rs = Factory.createVector(b, restrict[level].RowCount), xs = Factory.createVector(b, restrict[level].RowCount);

				// Compute residual, and restrict it
				r = Blas.Default.TransMultAdd(- 1.0, subA[level], x, b, r);
				rs = Blas.Default.Mult(restrict[level], r, rs);

				// Smooth residual
				for (int i = 0; i < mid; ++i)
					transCycle(level + 1, rs, xs);

				// Prolongate and apply correction
				x = Blas.Default.TransMultAdd(restrict[level], xs, x);
			}

			// Post-smoothings
			for (int i = 0; i < post; ++i)
				subSolver[level].TransApply(subA[level], b, x);
		}
	}
}