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
using MathNet.Numerics.LinearAlgebra.Sparse.Utilities;

namespace MathNet.Numerics.LinearAlgebra.Sparse
{
	/// <summary> Block vector</summary>
	[Serializable]
	public class BlockVector : AbstractVector, IVector, IBlockAccessVector
	{
		/// <summary> Sub-vectors (blocks)</summary>
		private IVector[] x;

		/// <summary> Indices for each block</summary>
		private int[][] index;

		/// <summary> Dense representation (only valid after assemble())</summary>
		private double[] dense;

		/// <summary> Constructor for BlockVector.</summary>
		public BlockVector(int size, int numBlocks) : base(size)
		{
			x = new IVector[numBlocks];
			index = new int[numBlocks][];
			dense = new double[size];
		}

		public virtual double[] Array
		{
			get
			{
				SupportClass.ArraySupport.Fill(dense, 0.0);
				return dense;
			}
		}

		public virtual int BlockCount
		{
			get { return x.Length; }
		}

		public virtual IVector GetBlock(int i)
		{
			return x[i];
		}

		public virtual int[] GetBlockIndices(int i)
		{
			return index[i];
		}

		public virtual void SetBlock(int i, int[] index, IVector x)
		{
			this.index[i] = index;
			this.x[i] = x;
		}
	}
}