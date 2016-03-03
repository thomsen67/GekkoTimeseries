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

namespace MathNet.Numerics.LinearAlgebra.Sparse
{
	/// <summary> Basic matrix interface.</summary>
	public interface IMatrix
	{
		/// <summary>Returns true if the matrix is square.</summary>
		bool Square { get; }

		int RowCount { get; }

		int ColumnCount { get; }
	}

	/// <summary>Dense matrix or vector.</summary>
	public interface IDense
	{

	}

	/// <summary>Sparse matrix or vector.</summary>
	public interface ISparse
	{
		/// <summary> Compacts the storage. May require re-allocation, 
		/// so it can fail if there is insufficient memory available.</summary>
		void Compact();
	}

	/// <summary> Data-access without index-access</summary>
	public interface IMatrixAccess
	{
		/// <summary> Returns a (possibly ragged) 2D array containing the data of the object,
		/// without the indices. No ordering may be assumed.</summary>
		double[][] Data { get; }
	}

	/// <summary> Matrix consisting of sub-matrices</summary>
	public interface IBlockAccessMatrix : IMatrix
	{
		/// <summary>Gets the number of block.</summary>
		int BlockCount { get; }

		/// <summary> Returns the matrix in block i</summary>
		IMatrix GetBlock(int i);

		/// <summary> Returns the row-indices of block i</summary>
		int[] GetBlockRowIndices(int i);

		/// <summary> Returns the column-indices of block i</summary>
		int[] GetBlockColumnIndices(int i);

		/// <summary> Sets block i to the given matrix, with row and column indices</summary>
		void SetBlock(int i, int[] row, int[] column, IMatrix A);
	}

	// SPARSE MATRICES

	/// <summary> Matrix with access by row/column vectors</summary>
	public interface ICoordinateAccessMatrix : IMatrix, IVectorAccess, ISparse
	{
		/// <summary>Gets or sets the row indices.</summary>
		int[] RowIndices { get; set; }

		/// <summary>Gets or sets the column indices.</summary>
		int[] ColumnIndices { get; set; }

		/// <summary> Sets the data</summary>
		void SetData(double[] data);
	}

	/// <summary> Sparse matrix with fast column access.</summary>
	public interface ISparseColumnAccessMatrix : ISparse, IMatrix
	{
		/// <summary> Returns a sparse representation of the given column. The view is directly
		/// backed by the matrix, so changes in it changes the matrix.
		/// </summary>
		IntDoubleVectorPair GetColumn(int i);

		/// <summary> Sets column i equal x. The indices must be sorted.</summary>
		void SetColumn(int i, IntDoubleVectorPair x);
	}

	/// <summary> Sparse matrix with fast access to the whole structure, with column-major
	/// indices.</summary>
	public interface ISparseColumnRowAccessMatrix : ISparse, IMatrix
	{
		/// <summary> Returns a sparse representation of the given row. The view is directly
		/// backed by the matrix, so changes in it changes the matrix.</summary>
		/// <remarks>When setting the whole,  The indices must be sorted.</remarks>
		IntIntDoubleVectorTriple Matrix { get; set; }
	}

	/// <summary> Sparse matrix with fast row-based access.</summary>
	public interface ISparseRowAccessMatrix : ISparse, IMatrix
	{
		/// <summary> Returns a sparse representation of the given row</summary>
		IntDoubleVectorPair GetRow(int i);

		/// <summary> Sets row i equal x</summary>
		void SetRow(int i, IntDoubleVectorPair x);
	}

	/// <summary> Sparse matrix with fast access to the whole structure, with row-major
	/// indices.</summary>
	public interface ISparseRowColumnAccessMatrix : ISparse, IMatrix
	{
		/// <summary>Gets or sets a sparse representation of the matrix (backed by the matrix).</summary>
		/// <remarks>When setting the whole matrix, the indices must be sorted.</remarks>
		IntIntDoubleVectorTriple Matrix { get; set; }
	}

	/// <summary> Matrix with elemental access operations. The block-wise operations will
	/// typically be faster than the elementwise operations, and the extractions
	/// methods present here are not likely to be as fast as the custom accessors
	/// the matrix may implement.</summary>
	public interface IElementalAccessMatrix : IMatrix
	{
		/// <summary> A(row,column) += value</summary>
		void AddValue(int row, int column, double val);

		/// <summary> A(row,column) = value</summary>
		void SetValue(int row, int column, double val);

		/// <summary> Returns A(row,column)</summary>
		double GetValue(int row, int column);

		// TODO: use Matrix in IElementalAccessMatrix

		/// <summary> A(row,column) += values, blockwise</summary>
		void AddValues(int[] row, int[] column, double[,] values);

		/// <summary> A(row,column) = values, blockwise</summary>
		void SetValues(int[] row, int[] column, double[,] values);

		/// <summary> Returns the block A(row,column)</summary>
		double[,] GetValues(int[] row, int[] column);
	}

	/// <summary> Matrix supporting zeroing of columns</summary>
	public interface IZeroColumnMatrix : IMatrix
	{
		/// <summary> Zeros each given column, and modifies the diagonal. Useful for enforcing
		/// essential boundary conditions (such as Dirichlet).</summary>
		void ZeroColumns(int[] column, double diagonal);
	}

	/// <summary> Matrix supporting zeroing of rows</summary>
	public interface IZeroRowMatrix : IMatrix
	{
		/// <summary> Zeros each given row, and modifies the diagonal. Useful for enforcing
		/// essential boundary conditions (such as Dirichlet).</summary>
		void ZeroRows(int[] row, double diagonal);
	}

	/// <summary> Matrix with elemental access and supporting zeroing of columns.</summary>
	public interface IElementalAccessZeroColumnMatrix : IElementalAccessMatrix, IZeroColumnMatrix
	{
	}

	/// <summary> Matrix with elemental access and supporting zeroing of rows.</summary>
	public interface IElementalAccessZeroRowMatrix : IElementalAccessMatrix, IZeroRowMatrix
	{
	}

	// DENSE MATRICES

	/// <summary> Matrix with fast dense column access.</summary>
	public interface IDenseColumnAccessMatrix : IDense, IMatrix
	{
		/// <summary> Returns the given column. The array is backed directly by the matrix
		/// (changing it changes the matrix).</summary>
		double[] GetColumn(int i);

		/// <summary> Sets column i equal x. No deep copying is performed.</summary>
		void SetColumn(int i, double[] x);
	}

	/// <summary>Dense matrix with fast access to the whole structure, 
	/// with column-major indices.</summary>
	public interface IDenseColumnRowAccessMatrix : IDense, IMatrix
	{
		/// <summary>Gets or sets a dense representation of the matrix, with the indices denoting
		/// column-offsets (backed by the matrix).</summary>
		IntDoubleVectorPair Matrix { get; set; }
	}

	/// <summary> Matrix with fast dense row access.</summary>
	public interface IDenseRowAccessMatrix : IDense, IMatrix
	{
		/// <summary> Returns the given row. The array is backed directly by the matrix
		/// (changing it changes the matrix).</summary>
		double[] GetRow(int i);

		/// <summary> Sets row i equal x. No deep copying is performed.</summary>
		void SetRow(int i, double[] x);
	}

	/// <summary> Dense matrix with fast access to the whole structure, with row-major indices.</summary>
	public interface IDenseRowColumnAccessMatrix : IDense, IMatrix
	{
		/// <summary>Gets or sets a dense representation of the matrix, with 
		/// the indices denoting row-offsets. The array is backed directly 
		/// by the matrix (changing it changes the matrix).</summary>
		IntDoubleVectorPair Matrix { get; set; }
	}
}