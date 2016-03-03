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
	public enum NORMS
	{
		/// <summary>1-norm, for matrices and vectors.</summary>
		NORM1,

		/// <summary>2-norm, for vectors.</summary>
		NORM2,

		/// <summary>Frobenius norm, for matrices.</summary>
		NORMF,

		/// <summary>Infinity norm, for matrices and vectors.</summary>
		NORMINF
	} ;

	/// <summary> Basic linear algebra subroutines. The methods present here are
	/// suitable for sparse matrices and iterative solvers, as such not all
	/// the <a href="http://www.netlib.org/blas">Fortran BLAS</a> methods
	/// have been implemented. Of particular note is the absence of
	/// matrix/matrix multiplication and triangular solvers.
	/// </summary>
	public interface IBLAS
	{
		//==========================================================================
		//==== Level 3, Matrix operations ==========================================
		//==========================================================================

		/// <summary> <c>B = A<sup>T</sup></c>.</summary>
		/// <returns> B </returns>
		IMatrix Transpose(IMatrix A, IMatrix B);

		/// <summary> <c>A = A + alpha*I</c>.</summary>
		/// <returns> A </returns>
		IMatrix AddDiagonal(double alpha, IMatrix A);

		/// <summary> Computes the specified norm of the matrix.</summary>
		/// <param name="normtype">Any of NORM1, NORMF or NORMINF.</param>
		double Norm(IMatrix A, NORMS normtype);

		/// <summary> <c>A = alpha*A</c>.</summary>
		/// <returns> A </returns>
		IMatrix Scale(double alpha, IMatrix A);

		/// <summary> <c>B = A</c>.</summary>
		/// <returns> B </returns>
		IMatrix Copy(IMatrix A, IMatrix B);

		/// <summary> <c>B = A(startRow:stopRow,startColumn:stopColumn)</c>.
		/// Start-indices are inclusive, stop-indices are exclusive.</summary>
		/// <returns> B </returns>
		IMatrix Copy(IMatrix A, IMatrix B, int startRow, int stopRow, int startColumn, int stopColumn);

		/// <summary> <c>A = 0</c>. The non-zero structure is preserved for sparse
		/// matrices, speeding up later reconstructions.</summary>
		/// <returns> A </returns>
		IMatrix Zero(IMatrix A);

		/// <summary> Number of non-zero entries of A</summary>
		int Cardinality(IMatrix A);

		//==========================================================================
		//==== Level 2, Matrix-Vector operations ===================================
		//==========================================================================

		/// <summary> <c>z = alpha*A*x + beta*y</c>. x can not be the same as y or z.</summary>
		/// <returns> z </returns>
		IVector MultAdd(double alpha, IMatrix A, IVector x, double beta, IVector y, IVector z);

		/// <summary> <c>z = A*x + beta*y</c>. x can not be the same as y or z.</summary>
		/// <returns> z </returns>
		IVector MultAdd(IMatrix A, IVector x, double beta, IVector y, IVector z);

		/// <summary> <c>z = alpha*A*x + y</c>. x can not be the same as y or z.</summary>
		/// <returns> z </returns>
		IVector MultAdd(double alpha, IMatrix A, IVector x, IVector y, IVector z);

		/// <summary> <c>z = A*x + y</c>. x can not be the same as y or z.</summary>
		/// <returns> z </returns>
		IVector MultAdd(IMatrix A, IVector x, IVector y, IVector z);

		/// <summary> <c>y = alpha*A*x + beta*y</c>. x can not be the same as y.</summary>
		/// <returns> y </returns>
		IVector MultAdd(double alpha, IMatrix A, IVector x, double beta, IVector y);

		/// <summary> <c>y = alpha*A*x + y</c>. x can not be the same as y.</summary>
		/// <returns> y </returns>
		IVector MultAdd(double alpha, IMatrix A, IVector x, IVector y);

		/// <summary> <c>y = A*x + beta*y</c>. x can not be the same as y.</summary>
		/// <returns> y </returns>
		IVector MultAdd(IMatrix A, IVector x, double beta, IVector y);

		/// <summary> <c>y = A*x + y</c>. x can not be the same as y.</summary>
		/// <returns> y </returns>
		IVector MultAdd(IMatrix A, IVector x, IVector y);

		/// <summary> <c>y = alpha*A*x</c>. x can not be the same as y</summary>
		/// <returns> y </returns>
		IVector Mult(double alpha, IMatrix A, IVector x, IVector y);

		/// <summary> <c>y = A*x</c>. x can not be the same as y</summary>
		/// <returns> y </returns>
		IVector Mult(IMatrix A, IVector x, IVector y);

		/// <summary> <c>z = alpha*A<sup>T</sup>*x + beta*y</c>. x can not be the same as
		/// y or z.</summary>
		/// <returns> z </returns>
		IVector TransMultAdd(double alpha, IMatrix A, IVector x, double beta, IVector y, IVector z);

		/// <summary> <c>z = A<sup>T</sup>*x + beta*y</c>. x can not be the same as y or
		/// z.</summary>
		/// <returns> z </returns>
		IVector TransMultAdd(IMatrix A, IVector x, double beta, IVector y, IVector z);

		/// <summary> <c>z = alpha*A<sup>T</sup>*x + y</c>. x can not be the same as y or
		/// z.</summary>
		/// <returns> z </returns>
		IVector TransMultAdd(double alpha, IMatrix A, IVector x, IVector y, IVector z);

		/// <summary> <c>z = A<sup>T</sup>*x + y</c>. x can not be the same as y or z.</summary>
		/// <returns> z </returns>
		IVector TransMultAdd(IMatrix A, IVector x, IVector y, IVector z);

		/// <summary> <c>y = alpha*A<sup>T</sup>*x + beta*y</c>. x can not be the same as
		/// y.</summary>
		/// <returns> y </returns>
		IVector TransMultAdd(double alpha, IMatrix A, IVector x, double beta, IVector y);

		/// <summary> <c>y = alpha*A<sup>T</sup>*x + y</c>. x can not be the same as y.</summary>
		/// <returns> y </returns>
		IVector TransMultAdd(double alpha, IMatrix A, IVector x, IVector y);

		/// <summary> <c>y = A<sup>T</sup>*x + beta*y</c>. x can not be the same as y.</summary>
		/// <returns> y </returns>
		IVector TransMultAdd(IMatrix A, IVector x, double beta, IVector y);

		/// <summary> <c>y = A<sup>T</sup>*x + y</c>. x can not be the same as y.</summary>
		/// <returns> y </returns>
		IVector TransMultAdd(IMatrix A, IVector x, IVector y);

		/// <summary> <c>y = alpha*A<sup>T</sup>*x</c>. x can not be the same as y</summary>
		/// <returns> y </returns>
		IVector TransMult(double alpha, IMatrix A, IVector x, IVector y);

		/// <summary> <c>y = A<sup>T</sup>*x</c>. x can not be the same as y</summary>
		/// <returns> y </returns>
		IVector TransMult(IMatrix A, IVector x, IVector y);

		/// <summary> <c>A = alpha*x*y<sup>T</sup> + A</c>.</summary>
		/// <returns> A </returns>
		IMatrix Rank1(double alpha, IVector x, IVector y, IMatrix A);

		/// <summary> <c>A = x*y<sup>T</sup> + A</c>.</summary>
		/// <returns> A </returns>
		IMatrix Rank1(IVector x, IVector y, IMatrix A);

		/// <summary> <c>A = alpha*x*x<sup>T</sup> + A</c>.</summary>
		/// <returns> A </returns>
		IMatrix Rank1(double alpha, IVector x, IMatrix A);

		/// <summary> <c>A = x*x<sup>T</sup> + A</c>.</summary>
		/// <returns> A </returns>
		IMatrix Rank1(IVector x, IMatrix A);

		//==========================================================================
		//==== Level 1, Vector operations ==========================================
		//==========================================================================

		/// <summary> <c>z = alpha*x + beta*y</c>.</summary>
		/// <returns> z </returns>
		IVector Add(double alpha, IVector x, double beta, IVector y, IVector z);

		/// <summary> <c>z = alpha*x + y</c>.</summary>
		/// <returns> z </returns>
		IVector Add(double alpha, IVector x, IVector y, IVector z);

		/// <summary> <c>z = x + beta*y</c>.</summary>
		/// <returns> z
		/// </returns>
		IVector Add(IVector x, double beta, IVector y, IVector z);

		/// <summary> <c>z = x + y</c>.</summary>
		/// <returns> z </returns>
		IVector Add(IVector x, IVector y, IVector z);

		/// <summary> <c>y = alpha*x + beta*y</c>.</summary>
		/// <returns> y </returns>
		IVector Add(double alpha, IVector x, double beta, IVector y);

		/// <summary> <c>y = alpha*x + y</c>.</summary>
		/// <returns> y </returns>
		IVector Add(double alpha, IVector x, IVector y);

		/// <summary> <c>y = x + beta*y</c>.</summary>
		/// <returns> y </returns>
		IVector Add(IVector x, double beta, IVector y);

		/// <summary> <c>y = x + y</c>.</summary>
		/// <returns> y </returns>
		IVector Add(IVector x, IVector y);

		/// <summary> <c>x<sup>T</sup>*y</c>.</summary>
		double Dot(IVector x, IVector y);

		/// <summary> Computes the specified norm of the vector.</summary>
		/// <param name="normtype">One of NORM1, NORM2 or NORMINF.</param>
		double Norm(IVector x, NORMS normtype);

		/// <summary> <c>y = alpha*x</c>.</summary>
		/// <returns> y </returns>
		IVector ScaleCopy(double alpha, IVector x, IVector y);

		/// <summary> <c>x = alpha*x</c>.</summary>
		/// <returns> x </returns>
		IVector Scale(double alpha, IVector x);

		/// <summary> <c>x = alpha</c>.</summary>
		/// <returns> x </returns>
		IVector SetVector(double alpha, IVector x);

		/// <summary> <c>y = x</c>.</summary>
		/// <returns> y </returns>
		IVector Copy(IVector x, IVector y);

		/// <summary> <c>y = x[start:stop)</c>.</summary>
		/// <param name="start">Inclusive index.</param>
		/// <param name="stop">Exclusive index.</param>
		/// <returns> y </returns>
		IVector Copy(IVector x, IVector y, int start, int stop);

		/// <summary> <c>x = 0</c>. The non-zero structure is preserved for sparse
		/// vectors, speeding up later reconstructions.</summary>
		/// <returns> x </returns>
		IVector Zero(IVector x);

		/// <summary> Number of non-zero entries of x.</summary>
		int Cardinality(IVector x);

		//==========================================================================
		//==== Miscellaneous =======================================================
		//==========================================================================

		/// <summary> Scatter x into the dense array y.</summary>
		/// <returns> y
		/// </returns>
		double[] Scatter(IntDoubleVectorPair x, double[] y);

		/// <summary> Gathers the non-zero entries of the dense array x.</summary>
		IntDoubleVectorPair Gather(double[] x);

		/// <summary> Gathers the given indices of xData.</summary>
		IntDoubleVectorPair Gather(int[] xIndex, double[] xData);

		/// <summary> Exports the matrix into a dense structure, row-oriented.</summary>
		double[,] GetArrayCopy(IMatrix A);

		/// <summary> Exports the vector into a dense structure.</summary>
		double[] GetArrayCopy(IVector x);

		/// <summary> <c>y = x</c>.</summary>
		/// <returns> x </returns>
		IVector SetVector(double[] x, IVector y);

	}

	/// <summary> Contains access to the default BLAS.</summary>
	public class Blas
	{
		/// <summary>Prevent instanciation.</summary>
		private Blas()
		{
		}

		/// <summary> Algebraic operations. Chose desired implementation (sequential, parallel)</summary>
		public static IBLAS blas = new SequentialBLAS();

		public static IBLAS Default
		{
			get { return blas; }
			set { blas = value; }
		}
	}
}