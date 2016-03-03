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
	/// <summary> Sparse matrix stored as 2D ragged columns. For best performance during
	/// assembly, ensure that enough memory is allocated at construction time,
	/// as re-allocation may be time-consuming.
	/// </summary>
	[Serializable]
	public class SparseColumnMatrix : AbstractMatrix, ISparseColumnAccessMatrix, 
		IElementalAccessZeroColumnMatrix, IMatrixAccess, IMatrix
	{
		/// <summary> Matrix data</summary>
		private double[][] data;

		/// <summary> Row indices. These are kept sorted within each column.</summary>
		private int[][] rowIndex;

		/// <summary> Number of indices in use on each column.</summary>
		private int[] used;

		/// <summary> Is the matrix compacted?</summary>
		private bool isCompact;

		/// <summary> Constructor for SparseColumnMatrix.</summary>
		/// <param name="nz">Initial number of non-zeros on each column</param>
		public SparseColumnMatrix(int numRows, int numColumns, int[] nz) : base(numRows, numColumns)
		{
			data = new double[numColumns][];
			rowIndex = new int[numColumns][];
;
			for (int i = 0; i < numColumns; ++i)
			{
				data[i] = new double[nz[i]];
				rowIndex[i] = new int[nz[i]];
			}
			used = new int[numColumns];
		}

		/// <summary> Constructor for SparseColumnMatrix.</summary>
		/// <param name="nz">Initial number of non-zeros on each column</param>
		public SparseColumnMatrix(int numRows, int numColumns, int nz) : base(numRows, numColumns)
		{
			data = new double[numColumns][];
			rowIndex = new int[numColumns][];

			for (int i = 0; i < numColumns; ++i)
			{
				data[i] = new double[nz];
				rowIndex[i] = new int[nz];
			}
			used = new int[numColumns];
		}

		/// <summary> Constructor for SparseColumnMatrix. Zero initial pre-allocation</summary>
		public SparseColumnMatrix(int numRows, int numColumns) : this(numRows, numColumns, 0)
		{
		}

		/// <summary> Constructor for SparseColumnMatrix, and copies the contents from the
		/// supplied matrix.</summary>
		/// <param name="nz">Initial number of non-zeros on each column.</param>
		public SparseColumnMatrix(IMatrix A, int[] nz) : this(A.RowCount, A.ColumnCount, nz)
		{
			Blas.Default.Copy(A, this);
		}

		/// <summary> Constructor for SparseColumnMatrix, and copies the contents from the
		/// supplied matrix.</summary>
		/// <param name="nz">Initial number of non-zeros on each column.</param>
		public SparseColumnMatrix(IMatrix A, int nz) : this(A.RowCount, A.ColumnCount, nz)
		{
			Blas.Default.Copy(A, this);
		}

		/// <summary> Constructor for SparseColumnMatrix, and copies the contents from the
		/// supplied matrix.</summary>
		public SparseColumnMatrix(IMatrix A) : this(A, 0)
		{
		}

		public virtual double[][] Data
		{
			get
			{
				Compact();
				return data;
			}
		}

		public virtual IntDoubleVectorPair GetColumn(int i)
		{
			Compact(i);
			return new IntDoubleVectorPair(rowIndex[i], data[i]);
		}

		public virtual void SetColumn(int i, IntDoubleVectorPair x)
		{
			rowIndex[i] = x.indices;
			data[i] = x.data;
			used[i] = x.data.Length;
			isCompact = false;
		}

		public virtual void ZeroColumns(int[] column, double diagonal)
		{
			for (int i = 0; i < column.Length; ++i)
			{
				int col = column[i];

				if (col < row_count)
				{
					data[col][0] = diagonal;
					rowIndex[col][0] = col;
					used[col] = 1;
				}
				else used[col] = 0;
			}

			isCompact = false;
		}

		public virtual void AddValue(int row, int column, double val)
		{
			int index = GetRowIndex(row, column);
			data[column][index] += val;
		}

		public virtual void SetValue(int row, int column, double val)
		{
			int index = GetRowIndex(row, column);
			data[column][index] = val;
		}

		public virtual double GetValue(int row, int column)
		{
			int ind = Arrays.binarySearch(rowIndex[column], row, 0, used[column]);
			
			if (ind != - 1) return data[column][ind];
			else if (row < row_count && row >= 0) return 0.0;
			else throw new IndexOutOfRangeException("Row " + row + " Column " + column);
		}

		public virtual void AddValues(int[] row, int[] column, double[,] values)
		{
			for (int i = 0; i < column.Length; ++i)
			{
				for (int j = 0; j < row.Length; ++j)
				{
					int index = GetRowIndex(row[j], column[i]);
					data[column[i]][index] += values[j, i];
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
					data[column[i]][index] = values[j, i];
				}
			}
		}

		public virtual double[,] GetValues(int[] row, int[] column)
		{
			double[,] sub = new double[row.Length, column.Length];
//			for (int i = 0; i < row.Length; i++)
//			{
//				sub[i] = new double[column.Length];
//			}
			for (int i = 0; i < row.Length; ++i)
				for (int j = 0; j < column.Length; ++j)
					sub[i, j] = GetValue(row[i], column[j]);
			return sub;
		}

		public virtual void Compact()
		{
			if (!isCompact)
			{
				for (int i = 0; i < column_count; ++i) Compact(i);
				isCompact = true;
			}
		}

		/// <summary> Compacts the column-indices and entries.</summary>
		private void Compact(int column)
		{
			if (used[column] < data[column].Length)
			{
				double[] newData = new double[used[column]];
				Array.Copy(data[column], 0, newData, 0, used[column]);
				int[] newInd = new int[used[column]];
				Array.Copy(rowIndex[column], 0, newInd, 0, used[column]);
				data[column] = newData;
				rowIndex[column] = newInd;
			}
		}

		/// <summary> Tries to find the row-index in the given column. If it is not found,
		/// a reallocation is done, and a new index is returned.</summary>
		private int GetRowIndex(int row, int col)
		{
			int[] curRow = rowIndex[col];
			double[] curDat = data[col];

			// Try to find column index
			int ind = Arrays.binarySearchGreater(curRow, row, 0, used[col]);

			// Found
			if (ind < used[col] && curRow[ind] == row)
				return ind;

			// Not found, try to make room
			if (row < 0 || row >= row_count)
				throw new IndexOutOfRangeException(" Row " + row + " Column " + col);
			used[col]++;

			// Check available memory
			if (used[col] > curDat.Length)
			{
				// If zero-length, use new length of 1, else double the bandwidth
				int newLength = 1;
				if (curDat.Length != 0)
					newLength = 2*curDat.Length;

				// Copy existing data into new arrays
				int[] newRow = new int[newLength];
				double[] newDat = new double[newLength];
				Array.Copy(curRow, 0, newRow, 0, curDat.Length);
				Array.Copy(curDat, 0, newDat, 0, curDat.Length);

				// Update pointers
				rowIndex[col] = newRow;
				data[col] = newDat;
				curRow = newRow;
				curDat = newDat;
			}

			// All ok, make room for insertion
			for (int i = used[col] - 1; i >= ind + 1; --i)
			{
				curRow[i] = curRow[i - 1];
				curDat[i] = curDat[i - 1];
			}

			// Put in new structure
			curRow[ind] = row;
			curDat[ind] = 0.0;
			isCompact = false;

			return ind;
		}
	}
}