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
	/// <summary> Sparse matrix stored as one long vector, row major.</summary>
	[Serializable]
	public class SparseRowColumnMatrix : AbstractMatrix, IElementalAccessZeroRowMatrix, 
		ISparseRowColumnAccessMatrix, IVectorAccess, IMatrix
	{
		/// <summary> Matrix data</summary>
		private double[] data;

		/// <summary> Column indices. These are kept sorted within each row.</summary>
		private int[] columnIndices;

		/// <summary> Indices to the start of each row</summary>
		private int[] rowIndices;

		/// <summary> Number of indices in use on each row.</summary>
		private int[] used;

		/// <summary> Is the matrix compacted?</summary>
		private bool isCompact;

		/// <summary> Constructor for SparseRowColumnMatrix, and copies the contents from the
		/// supplied matrix.</summary>
		/// <param name="nz">Maximum number of non-zeros on each row.</param>
		public SparseRowColumnMatrix(IMatrix A, int[] nz) : this(A.RowCount, A.ColumnCount, nz)
		{
			Blas.Default.Copy(A, this);
		}

		/// <summary> Constructor for SparseRowColumnMatrix, and copies the contents from the
		/// supplied matrix.</summary>
		/// <param name="nz">Maximum number of non-zeros on each row</param>
		public SparseRowColumnMatrix(IMatrix A, int nz) : this(A.RowCount, A.ColumnCount, nz)
		{
			Blas.Default.Copy(A, this);
		}

		/// <summary> Constructor for SparseRowColumnMatrix.</summary>
		/// <param name="nz">Maximum number of non-zeros on each row</param>
		public SparseRowColumnMatrix(int numRows, int numColumns, int[] nz) : base(numRows, numColumns)
		{
			int nnz = 0;
			for (int i = 0; i < nz.Length; ++i) nnz += nz[i];

			rowIndices = new int[numRows + 1];
			columnIndices = new int[nnz];
			data = new double[nnz];
			used = new int[numRows];
			for (int i = 1; i < numRows + 1; ++i)
				rowIndices[i] = nz[i - 1] + rowIndices[i - 1];
		}

		/// <summary> Constructor for SparseRowColumnMatrix.</summary>
		/// <param name="nz">Maximum number of non-zeros on each row</param>
		public SparseRowColumnMatrix(int numRows, int numColumns, int nz) : base(numRows, numColumns)
		{
			int nnz = nz*numRows;

			rowIndices = new int[numRows + 1];
			columnIndices = new int[nnz];
			data = new double[nnz];
			used = new int[numRows];

			for (int i = 1; i < numRows + 1; ++i)
				rowIndices[i] = nz + rowIndices[i - 1];
		}

		public virtual void AddValue(int row, int column, double val)
		{
			int index = GetColumnIndex(column, row);
			data[rowIndices[row] + index] += val;
		}

		public virtual void SetValue(int row, int column, double val)
		{
			int index = GetColumnIndex(column, row);
			data[rowIndices[row] + index] = val;
		}

		public virtual double GetValue(int row, int column)
		{
			int ind = Arrays.binarySearch(columnIndices, column, rowIndices[row], rowIndices[row] + used[row]);
			
			if (ind >= 0) return data[ind];
			else if (column < column_count && column >= 0) return 0;
			else throw new IndexOutOfRangeException("Row " + row + " Column " + column);
		}

		public virtual void AddValues(int[] row, int[] column, double[,] values)
		{
			for (int i = 0; i < row.Length; ++i)
				for (int j = 0; j < column.Length; ++j)
				{
					int index = GetColumnIndex(column[j], row[i]);
					data[rowIndices[row[i]] + index] += values[i, j];
				}
		}

		public virtual void SetValues(int[] row, int[] column, double[,] values)
		{
			for (int i = 0; i < row.Length; ++i)
			{
				for (int j = 0; j < column.Length; ++j)
				{
					int index = GetColumnIndex(column[j], row[i]);
					data[rowIndices[row[i]] + index] = values[i, j];
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

		public virtual IntIntDoubleVectorTriple Matrix
		{
			get
			{
				Compact();
				return new IntIntDoubleVectorTriple(columnIndices, rowIndices, data);
			}
			set
			{
				data = value.data;
				columnIndices = value.major;
				rowIndices = value.minor;
			}
		}

		public virtual void Compact()
		{
			if (isCompact)
				return;

			int nnz = 0;
			for (int i = 0; i < row_count; ++i)
				nnz += used[i];

			if (nnz < data.Length)
			{
				int[] newRowIndex = new int[row_count + 1];
				int[] newColumnIndex = new int[nnz];
				double[] newData = new double[nnz];

				newRowIndex[0] = rowIndices[0];
				for (int i = 0; i < row_count; ++i)
				{
					newRowIndex[i + 1] = newRowIndex[i] + used[i];
					Array.Copy(data, rowIndices[i], newData, newRowIndex[i], used[i]);
					Array.Copy(columnIndices, rowIndices[i], newColumnIndex, newRowIndex[i], used[i]);
				}

				rowIndices = newRowIndex;
				columnIndices = newColumnIndex;
				data = newData;
			}

			isCompact = true;
		}


		public virtual void ZeroRows(int[] row, double diagonal)
		{
			for (int i = 0; i < row.Length; ++i)
			{
				int rowI = row[i];

				if (rowI < column_count)
				{
					data[rowIndices[rowI]] = diagonal;
					columnIndices[rowIndices[rowI]] = rowI;
					used[rowI] = 1;
				}
				else
					used[rowI] = 0;
			}

			isCompact = false;
		}

		/// <summary> Tries to find the col-index in the given row. If it is not found,
		/// a reallocation is done, and a new index is returned. If there is no more
		/// space for further allocation, an exception is thrown.</summary>
		private int GetColumnIndex(int col, int row)
		{
			int rowOffset = rowIndices[row];

			int ind = Arrays.binarySearchGreater(columnIndices, col, 
				rowOffset, rowOffset + used[row]) - rowOffset;

			// Found
			if (ind < used[row] && columnIndices[rowOffset + ind] == col)
				return ind;

			// Need to insert

			// Check column index
			if (col < 0 || col >= column_count)
				throw new IndexOutOfRangeException("Row " + row + " Column " + col);

			// Is the row full?
			if (rowOffset + used[row] >= data.Length || ((row + 1) < rowIndices.Length 
				&& rowOffset + used[row] >= rowIndices[row + 1]))
					throw new IndexOutOfRangeException("Too many non-zeros on row " + row);

			// Make room for insertion
			for (int i = used[row]; i >= ind + 1; --i)
			{
				columnIndices[rowOffset + i] = columnIndices[rowOffset + i - 1];
				data[rowOffset + i] = data[rowOffset + i - 1];
			}

			// Put in new structure
			used[row]++;
			columnIndices[rowOffset + ind] = col;
			data[rowOffset + ind] = 0;
			isCompact = false;

			return ind;
		}

		public virtual double[] Data
		{
			get
			{
				Compact();
				return data;
			}
		}
	}
}