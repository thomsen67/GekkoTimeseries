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
	// TODO: current implementation of CoordinateMatrix is very inefficient.
	// Accessing any element costs O(n) where 'n' is the number of non-zero elements.
	// A hastable should be used to store the non-zero indices.

	/// <summary> Coordinate storage matrix. The data is not kept sorted.</summary>
	[Serializable]
	public class CoordinateMatrix : AbstractMatrix, IMatrix, IElementalAccessMatrix, ICoordinateAccessMatrix
	{
		/// <summary> The rows</summary>
		private int[] row;

		/// <summary> The columns</summary>
		private int[] column;

		/// <summary> The data</summary>
		private double[] data;

		/// <summary> Current insertion offset</summary>
		private int offset;

		/// <summary> Constructor for CoordinateMatrix. No pre-allocation</summary>
		public CoordinateMatrix(int numRows, int numColumns) : this(numRows, numColumns, 0)
		{
		}

		/// <summary> Constructor for CoordinateMatrix.</summary>
		/// <param name="numEntries">Initial number of non-zero entries.</param>
		public CoordinateMatrix(int numRows, int numColumns, int numEntries) : base(numRows, numColumns)
		{
			column = new int[numEntries];
			row = new int[numEntries];
			data = new double[numEntries];
		}

		/// <summary> Constructor for CoordinateMatrix.</summary>
		public CoordinateMatrix(int numRows, int numColumns, int[] row, int[] column, double[] data) 
			: base(numRows, numColumns)
		{
			this.row = row;
			this.column = column;
			this.data = data;
			offset = data.Length;
		}

		public virtual void AddValue(int row, int column, double val)
		{
			int index = GetIndex(row, column);
			data[index] += val;
		}

		public virtual void SetValue(int row, int column, double val)
		{
			int index = GetIndex(row, column);
			data[index] = val;
		}

		public virtual double GetValue(int row, int column)
		{
			int index = Search(row, column);
			if (index >= 0)
				return data[index];
			else if (row >= 0 && row < row_count && column >= 0 && column < column_count)
				return 0.0;
			else
				throw new IndexOutOfRangeException("Row " + row + " Column " + column);
		}

		public virtual void AddValues(int[] row, int[] column, double[,] values)
		{
			for (int i = 0; i < row.Length; ++i)
				for (int j = 0; j < column.Length; ++j)
					AddValue(row[i], column[j], values[i, j]);
		}

		public virtual void SetValues(int[] row, int[] column, double[,] values)
		{
			for (int i = 0; i < row.Length; ++i)
				for (int j = 0; j < column.Length; ++j)
					SetValue(row[i], column[j], values[i, j]);
		}

		public virtual double[,] GetValues(int[] row, int[] column)
		{
			double[,] sub = new double[row.Length, column.Length];
			for (int i = 0; i < row.Length; ++i)
				for (int j = 0; j < column.Length; ++j)
					sub[i, j] = GetValue(row[i], column[j]);
			return sub;
		}

		public virtual double[] Data
		{
			get
			{
				Compact();
				return data;
			}
		}

		public virtual int[] RowIndices
		{
			get
			{
				Compact();
				return row;
			}
			set
			{
				this.row = value;
				this.offset = value.Length;
			}
		}

		public virtual int[] ColumnIndices
		{
			get
			{
				Compact();
				return column;
			}
			set
			{
				this.column = value;
				this.offset = value.Length;
			}
		}

		public virtual void SetData(double[] data)
		{
			this.data = data;
			this.offset = data.Length;
		}

		/// <summary> Tries to get the index, or allocates more space if needed</summary>
		private int GetIndex(int rowI, int columnI)
		{
			int index = Search(rowI, columnI);
			if (index >= 0)
				// Found
				return index;
			else
			{
				// Not found

				// Need to allocate more space
				if (offset >= data.Length)
					if (offset == 0)
						Realloc(1);
					else
						Realloc(2*offset);

				// Put in the new indices
				row[offset] = rowI;
				column[offset] = columnI;
				return offset++;
			}
		}

		/// <summary> Reallocates the internal arrays to a new size</summary>
		private void Realloc(int newSize)
		{
			int[] newRow = new int[newSize], newColumn = new int[newSize];
			double[] newData = new double[newSize];

			Array.Copy(row, 0, newRow, 0, offset);
			Array.Copy(column, 0, newColumn, 0, offset);
			Array.Copy(data, 0, newData, 0, offset);

			row = newRow;
			column = newColumn;
			data = newData;
		}

		/// <summary> Searches for the given entry. Returns -1 if it is not found</summary>
		private int Search(int rowI, int columnI)
		{
			for (int i = 0; i < offset; ++i)
				if (row[i] == rowI && column[i] == columnI)
					return i;
			return - 1;
		}

		public virtual void Compact()
		{
			if (offset < row.Length) Realloc(offset);
		}
	}
}