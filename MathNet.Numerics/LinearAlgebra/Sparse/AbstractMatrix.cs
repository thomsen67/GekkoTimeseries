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
	/// <summary> Partial implementation of Matrix</summary>
	[Serializable]
	public abstract class AbstractMatrix : IMatrix
	{
		public virtual bool Square
		{
			get { return row_count == column_count; }

		}

		/// <summary> Matrix dimensions</summary>
		protected internal int row_count, column_count;

		/// <summary> Constructor for AbstractMatrix.</summary>
		public AbstractMatrix(int numRows, int numColumns)
		{
			if (numRows < 0 || numColumns < 0)
				throw new IndexOutOfRangeException("Matrix size cannot be negative");

			this.row_count = numRows;
			this.column_count = numColumns;
		}

		/// <summary> Constructor for AbstractMatrix, same size as A.</summary>
		public AbstractMatrix(IMatrix A) : this(A.RowCount, A.ColumnCount)
		{
		}

		public virtual int RowCount
		{
			get { return row_count; }
		}

		public virtual int ColumnCount
		{
			get { return column_count; }
		}
	}
}