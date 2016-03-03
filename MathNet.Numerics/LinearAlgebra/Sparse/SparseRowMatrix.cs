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
using  MathNet.Numerics.LinearAlgebra.Sparse.Utilities;

namespace MathNet.Numerics.LinearAlgebra.Sparse
{
	/// <summary> Sparse matrix stored as 2D ragged rows. For best performance during
	/// assembly, ensure that enough memory is allocated at construction time,
	/// as re-allocation may be time-consuming.
	/// </summary>
	[Serializable]
	public class SparseRowMatrix : AbstractMatrix, ISparseRowAccessMatrix, 
		IElementalAccessZeroRowMatrix, IMatrixAccess, IMatrix
	{
		/// <summary> Matrix data</summary>
		private double[][] data;

		/// <summary> Column indices. These are kept sorted within each row.</summary>
		private int[][] columnIndex;

		/// <summary> Number of indices in use on each row.</summary>
		private int[] used;

		/// <summary> Is the matrix compacted?</summary>
		private bool isCompact;

		/// <summary> Constructor for SparseRowMatrix, and copies the contents from the
		/// supplied matrix.
		/// </summary>
		/// <param name="nz">Initial number of non-zeros on each row.</param>
		public SparseRowMatrix(IMatrix A, int[] nz) : this(A.RowCount, A.ColumnCount, nz)
		{
			Blas.Default.Copy(A, this);
		}

		/// <summary> Constructor for SparseRowMatrix, and copies the contents from the
		/// supplied matrix.</summary>
		/// <param name="nz">Initial number of non-zeros on each row.</param>
		public SparseRowMatrix(IMatrix A, int nz) : this(A.RowCount, A.ColumnCount, nz)
		{
			Blas.Default.Copy(A, this);
		}

		/// <summary> Constructor for SparseRowMatrix, and copies the contents from the
		/// supplied matrix. Zero initial pre-allocation.</summary>
		public SparseRowMatrix(IMatrix A) : this(A, 0)
		{
		}

		/// <summary> Constructor for SparseRowMatrix.</summary>
		/// <param name="nz">Initial number of non-zeros on each row.</param>
		public SparseRowMatrix(int numRows, int numColumns, int[] nz) : base(numRows, numColumns)
		{
			data = new double[numRows][];
			columnIndex = new int[numRows][];
			used = new int[numRows];
			for (int i = 0; i < numRows; ++i)
			{
				data[i] = new double[nz[i]];
				columnIndex[i] = new int[nz[i]];
			}
		}

		/// <summary> Constructor for SparseRowMatrix.</summary>
		/// <param name="nz">Initial number of non-zeros on each row.</param>
		public SparseRowMatrix(int numRows, int numColumns, int nz) : base(numRows, numColumns)
		{
			data = new double[numRows][];
			for (int i = 0; i < numRows; i++)
			{
				data[i] = new double[nz];
			}
			columnIndex = new int[numRows][];
			for (int i2 = 0; i2 < numRows; i2++)
			{
				columnIndex[i2] = new int[nz];
			}
			used = new int[numRows];
		}

		/// <summary> Constructor for SparseRowMatrix. Zero initial pre-allocation</summary>
		public SparseRowMatrix(int numRows, int numColumns) : this(numRows, numColumns, 0)
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

		public virtual void ZeroRows(int[] row, double diagonal)
		{
			for (int i = 0; i < row.Length; ++i)
			{
				int rowI = row[i];

				if (rowI < column_count)
				{
					data[rowI][0] = diagonal;
					columnIndex[rowI][0] = rowI;
					used[rowI] = 1;
				}
				else
					used[rowI] = 0;
			}
			isCompact = false;
		}

		public virtual void AddValue(int i, int j, double val)
		{
			int index = GetColumnIndex(j, i);
			data[i][index] += val;
		}

		public virtual void SetValue(int i, int j, double val)
		{
			int index = GetColumnIndex(j, i);
			data[i][index] = val;
		}

		public virtual double GetValue(int row, int column)
		{
			int ind = Arrays.binarySearch(columnIndex[row], column, 0, used[row]);
			
			if (ind != - 1) return data[row][ind];
			else if (column < column_count && column >= 0) return 0.0;
			else throw new IndexOutOfRangeException("Row " + row + " Column " + column);
		}

		public virtual void AddValues(int[] row, int[] column, double[,] values)
		{
			for (int i = 0; i < row.Length; ++i)
			{
				for (int j = 0; j < column.Length; ++j)
				{
					int index = GetColumnIndex(column[j], row[i]);
					data[row[i]][index] += values[i, j];
				}
			}
		}

		public virtual void SetValues(int[] row, int[] column, double[,] values)
		{
			for (int i = 0; i < row.Length; ++i)
			{
				for (int j = 0; j < column.Length; ++j)
				{
					int index = GetColumnIndex(column[j], row[i]);
					data[row[i]][index] = values[i, j];
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

		/// <summary> Tries to find the col-index in the given row. If it is not found,
		/// a reallocation is done, and a new index is returned.</summary>
		private int GetColumnIndex(int col, int row)
		{
			int[] curCol = columnIndex[row];
			double[] curDat = data[row];

			// Try to find column index
			int ind = Arrays.binarySearchGreater(curCol, col, 0, used[row]);

			// Found
			if (ind < used[row] && curCol[ind] == col)
				return ind;

			// Not found, try to make room
			if (col < 0 || col >= column_count)
				throw new IndexOutOfRangeException("Row " + row + " Column " + col);
			used[row]++;

			// Check available memory
			if (used[row] > curDat.Length)
			{
				// If zero-length, use new length of 1, else double the bandwidth
				int newLength = 1;
				if (curDat.Length != 0)
					newLength = 2*curDat.Length;

				// Copy existing data into new arrays
				int[] newCol = new int[newLength];
				double[] newDat = new double[newLength];
				Array.Copy(curCol, 0, newCol, 0, curDat.Length);
				Array.Copy(curDat, 0, newDat, 0, curDat.Length);

				// Update pointers
				columnIndex[row] = newCol;
				data[row] = newDat;
				curCol = newCol;
				curDat = newDat;
			}

			// All ok, make room for insertion
			for (int i = used[row] - 1; i >= ind + 1; --i)
			{
				curCol[i] = curCol[i - 1];
				curDat[i] = curDat[i - 1];
			}

			// Put in new structure
			curCol[ind] = col;
			curDat[ind] = 0.0;
			return ind;
		}

		public virtual void SetRow(int i, IntDoubleVectorPair x)
		{
			columnIndex[i] = x.indices;
			data[i] = x.data;
			used[i] = x.data.Length;
			isCompact = false;
		}

		public virtual IntDoubleVectorPair GetRow(int i)
		{
			Compact(i);
			return new IntDoubleVectorPair(columnIndex[i], data[i]);
		}

		/// <summary> Compacts the row-indices and entries.</summary>
		private void Compact(int row)
		{
			if (used[row] < data[row].Length)
			{
				double[] newDat = new double[used[row]];
				Array.Copy(data[row], 0, newDat, 0, used[row]);
				int[] newInd = new int[used[row]];
				Array.Copy(columnIndex[row], 0, newInd, 0, used[row]);
				data[row] = newDat;
				columnIndex[row] = newInd;
			}
		}

		public virtual void Compact()
		{
			if (!isCompact)
			{
				for (int i = 0; i < row_count; ++i) Compact(i);
				isCompact = true;
			}
		}
	}
}