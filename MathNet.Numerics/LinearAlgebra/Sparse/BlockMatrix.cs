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

namespace MathNet.Numerics.LinearAlgebra.Sparse
{
	/// <summary> Block matrix</summary>
	[Serializable]
	public class BlockMatrix : AbstractMatrix, IMatrix, IBlockAccessMatrix
	{
		/// <summary> Sub-matrices (blocks)</summary>
		private IMatrix[] A;

		/// <summary> Row-indices for each block</summary>
		private int[][] row;

		/// <summary> Column-indices for each block</summary>
		private int[][] column;


		/// <summary> Constructor for BlockMatrix.</summary>
		public BlockMatrix(int numRows, int numColumns, int numBlocks) : base(numRows, numColumns)
		{
			A = new IMatrix[numBlocks];
			row = new int[numBlocks][];
			column = new int[numBlocks][];
		}

		public virtual int BlockCount
		{
			get { return A.Length; }
		}

		public virtual IMatrix GetBlock(int i)
		{
			return A[i];
		}

		public virtual int[] GetBlockRowIndices(int i)
		{
			return row[i];
		}

		public virtual int[] GetBlockColumnIndices(int i)
		{
			return column[i];
		}

		public virtual void SetBlock(int i, int[] row, int[] column, IMatrix A)
		{
			this.row[i] = row;
			this.column[i] = column;
			this.A[i] = A;
		}
	}
}