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
	/// <summary> Sparse matrix stored as a vector, column major</summary>
	[Serializable]
	public class SparseColumnRowMatrix : AbstractMatrix, 
		ISparseColumnRowAccessMatrix, IElementalAccessZeroColumnMatrix, IVectorAccess, IMatrix
	{
		/// <summary> Matrix data</summary>
		private double[] data;

		/// <summary> Row indices. These are kept sorted within each column.</summary>
		private int[] rowIndex;

		/// <summary> Indices to the start of each column</summary>
		private int[] columnIndex;

		/// <summary> Number of indices in use on each column.</summary>
		private int[] used;

		/// <summary> Is the matrix compacted?</summary>
		private bool isCompact;

		/// <summary> Constructor for SparseColumnRowMatrix.</summary>
		/// <param name="nz">Maximum number of non-zeros on each row.</param>
		public SparseColumnRowMatrix(int numRows, int numColumns, int[] nz) : base(numRows, numColumns)
		{
			int nnz = 0;
			for (int i = 0; i < nz.Length; ++i) nnz += nz[i];

			data = new double[nnz];
			rowIndex = new int[nnz];
			columnIndex = new int[numColumns + 1];
			used = new int[numColumns];
			for (int i = 1; i < numColumns + 1; ++i)
				columnIndex[i] = nz[i - 1] + columnIndex[i - 1];
		}

		/// <summary> Constructor for SparseColumnRowMatrix.</summary>
		/// <param name="nz">Maximum number of non-zeros on each row.</param>
		public SparseColumnRowMatrix(int numRows, int numColumns, int nz) : base(numRows, numColumns)
		{
			int nnz = nz*numColumns;

			data = new double[nnz];
			rowIndex = new int[nnz];
			columnIndex = new int[numColumns + 1];
			used = new int[numColumns];
			for (int i = 1; i < numColumns + 1; ++i)
				columnIndex[i] = nz + columnIndex[i - 1];
		}

		/// <summary> Constructor for SparseColumnRowMatrix, and copies the contents from the
		/// supplied matrix.</summary>
		/// <param name="nz">Maximum number of non-zeros on each row.</param>
		public SparseColumnRowMatrix(IMatrix A, int nz) : this(A.RowCount, A.ColumnCount, nz)
		{
			Blas.Default.Copy(A, this);
		}

		/// <summary> Constructor for SparseColumnRowMatrix, and copies the contents from the
		/// supplied matrix.</summary>
		/// <param name="nz">Maximum number of non-zeros on each row.</param>
		public SparseColumnRowMatrix(IMatrix A, int[] nz) : this(A.RowCount, A.ColumnCount, nz)
		{
			Blas.Default.Copy(A, this);
		}

		public virtual IntIntDoubleVectorTriple Matrix
		{
			get
			{
				Compact();
				return new IntIntDoubleVectorTriple(rowIndex, columnIndex, data);
			}
			set
			{
				data = value.data;
				rowIndex = value.major;
				columnIndex = value.minor;
			}
		}

		public virtual void ZeroColumns(int[] column, double diagonal)
		{
			for (int i = 0; i < column.Length; ++i)
			{
				int col = column[i];

				if (col < row_count)
				{
					data[columnIndex[col]] = diagonal;
					rowIndex[columnIndex[col]] = col;
					used[col] = 1;
				}
				else
					used[col] = 0;
			}

			isCompact = false;
		}

		public virtual void AddValue(int row, int column, double val)
		{
			int index = GetRowIndex(row, column);
			data[columnIndex[column] + index] += val;
		}

		public virtual void SetValue(int row, int column, double val)
		{
			int index = GetRowIndex(row, column);
			data[columnIndex[column] + index] = val;
		}

		public virtual double GetValue(int row, int column)
		{
			int ind = Arrays.binarySearch(rowIndex, row, 
				columnIndex[column], columnIndex[column] + used[column]);

			if (ind >= 0) return data[ind];
			else if (row < row_count && row >= 0) return 0;
			else throw new IndexOutOfRangeException("Row " + row + " Column " + column);
		}

		public virtual void AddValues(int[] row, int[] column, double[,] values)
		{
			for (int i = 0; i < column.Length; ++i)
			{
				for (int j = 0; j < row.Length; ++j)
				{
					int index = GetRowIndex(row[j], column[i]);
					data[columnIndex[column[i]] + index] += values[j, i];
				}
			}
		}

		public virtual void SetValues(int[] row, int[] column, double[,] values)
		{
			for (int i = 0; i < column.Length; ++i)
			{
				for (int j = 0; j < row.Length; ++j)
				{
					int index = GetRowIndex(row[j], column[i]);
					data[columnIndex[column[i]] + index] = values[j, i];
				}
			}
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

		public virtual void Compact()
		{
			if (isCompact) return;

			int nnz = 0;
			for (int i = 0; i < column_count; ++i) nnz += used[i];

			if (nnz < data.Length)
			{
				double[] newData = new double[nnz];
				int[] newRowIndex = new int[nnz];
				int[] newColumnIndex = new int[column_count + 1];

				newColumnIndex[0] = columnIndex[0];
				for (int i = 0; i < column_count; ++i)
				{
					newColumnIndex[i + 1] = newColumnIndex[i] + used[i];
					Array.Copy(data, columnIndex[i], newData, newColumnIndex[i], used[i]);
					Array.Copy(rowIndex, columnIndex[i], newRowIndex, newColumnIndex[i], used[i]);
				}

				data = newData;
				rowIndex = newRowIndex;
				columnIndex = newColumnIndex;
			}

			isCompact = true;
		}

		/// <summary> Tries to find the col-index in the given row. If it is not found,
		/// a reallocation is done, and a new index is returned. If there is no more
		/// space for further allocation, an exception is thrown.
		/// </summary>
		private int GetRowIndex(int row, int col)
		{
			int columnOffset = columnIndex[col];

			int ind = Arrays.binarySearchGreater(rowIndex, row, columnOffset, columnOffset + used[col]) - columnOffset;

			// Found
			if (ind < used[col] && rowIndex[columnOffset + ind] == row)
				return ind;

			// Need to insert

            
            // Check row index
            if (row < 0 || row >= row_count)
                throw new IndexOutOfRangeException("Row " + row + " Column " + col);
            
			// Is the column full?
			if (columnOffset + used[col] >= data.Length || ((col + 1) < columnIndex.Length && columnOffset + used[col] >= columnIndex[col + 1]))
				throw new IndexOutOfRangeException("Too many non-zeros on column " + col);

			// Make room for insertion
			for (int i = used[col]; i >= ind + 1; --i)
			{
				rowIndex[columnOffset + i] = rowIndex[columnOffset + i - 1];
				data[columnOffset + i] = data[columnOffset + i - 1];
			}

			// Put in new structure
			used[col]++;
			rowIndex[columnOffset + ind] = row;
			data[columnOffset + ind] = 0;
			isCompact = false;

			return ind;
		}
	}
}