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

	/// <summary>
	/// Matrices and vectors, with basic linear algebra operations
	/// 
	/// <p>This namespace is originary based on a port  of
	/// <a href="http://www.math.uib.no/~bjornoh/jmp/">JMP</a>.</p>
	/// 
	/// <p><i>This port is still in very rough, the MathNet team
	/// encorage users to provide feedback.</i></p>
	/// 
	/// <p>The basic datastructures in MathNet are matrices and vectors, stored
	/// in double-precision (<c>double</c>). All the indices are
	/// 0-based, as is typical of C and C#, but different from
	/// Fortran's 1-based indexing. Complex numbers are not supported.</p>
	/// 
	/// <h3>Basic interface</h3>
	/// <p>The most fundamental interfaces are <see cref="IMatrix"/>
	/// and <see cref="IVector"/>. By themselves, they say little about the 
	/// underlying datastructure, as they only have size-information functions.</p>
	/// 
	/// <p>Extending Matrix is <see cref="IElementalAccessMatrix"/>, 
	/// which has simple assembly and retrieval functions, both elementwise and 
	/// blockwise. The block-operations are typically more efficient. There is a similiar
	/// interface, <see cref="IElementalAccessVector"/> which extends the basic vector.</p>
	/// 
	/// <p>Using these methods, most operations can be accomplished easily. There are 
	/// two more interfaces which are of interest, namely <see cref="IZeroColumnMatrix"/> 
	/// and <see cref="IZeroRowMatrix"/>. Matrices implementing these allow quick zeroing 
	/// of columns or rows such that boundary conditions for differential equations can 
	/// be explicitly handled. For convenience, the interfaces <see cref="IElementalAccessZeroRowMatrix"/>
	/// <see cref="IElementalAccessZeroColumnMatrix"/> extends on both the elemental access 
	/// interface and one of the zeroing interfaces, and can be used in application codes 
	/// to easier switch between different underlying storage formats (dense, sparse etc).</p>
	/// 
	/// <p>The aforementioned functions are sufficient for most applications, but MathNet
	/// provides other ways of accessing and changing the datastructures. This is typically 
	/// by direct access, and is generally not recommended except for developing linear
	/// algebra algorithms.</p>
	/// 
	/// <h3>Matrices and vectors</h3>
	/// <p>MathNet provides ten matrices and three vectors implementations. Of these, there 
	/// are five sparse matrices, four dense, and a block-structured matrix.  Amongst the
	/// sparse and dense matrices, there are both row- and column-oriented ones. Choosing 
	/// the right structure can have a significant impact on both performance and memory-usage. 
	/// Most of the matrices implement <see cref="IElementalAccessMatrix"/> and most of the vectors
	/// implement <see cref="IElementalAccessVector"/>. Row-oriented</p>
	/// <table>
	///		<tr>
	///			<th>Name</th>
	///			<th>Storage</th>
	///		</tr>
	///		<tr>
	///			<td>Dense matrix (data adapter for 
	///				<see cref="MathNet.Numerics.LinearAlgebra.Matrix"/> missing)</td>
	///			<td><c>double[,]</c>, row/column major</td>
	///		</tr>
	///		<tr>
	///			<td><see cref="SparseColumnMatrix"/></td>
	///			<td><c>int[,]/double[,]</c>, column major, growable</td>
	///		</tr>
	///		<tr>
	///			<td><see cref="SparseColumnRowMatrix"/></td>
	///			<td><c>int[]/double[]</c>, column major</td>
	///		</tr>
	///		<tr>
	///			<td><see cref="SparseRowMatrix"/></td>
	///			<td><c>int[,]/double[,]</c>, row major, growable</td>
	///		</tr>
	///		<tr>
	///			<td><see cref="SparseRowColumnMatrix"/></td>
	///			<td><c>int[]/double[]</c>, row major</td>
	///		</tr>
	///		<tr>
	///			<td><see cref="BlockMatrix"/></td>
	///			<td><c>int[,]/int[,]/Matrix[]</c></td>
	///		</tr>
	///		<tr>
	///			<td><see cref="BlockVector"/></td>
	///			<td><c>int[]/Vector[]</c></td>
	///		</tr>
	///		<tr>
	///			<td><see cref="CoordinateMatrix"/></td>
	///			<td><c>int[]/int[]/double[]</c></td>
	///		</tr>
	///		<tr>
	///			<td><see cref="DenseVector"/></td>
	///			<td><c>double[]</c></td>
	///		</tr>
	///		<tr>
	///			<td><see cref="SparseVector"/></td>
	///			<td><c>int[]/double[]</c>, growable</td>
	///		</tr>
	///	</table>
	///	<p>Notes:
	///	</p>
	///	<ol>
	///		<li>Sparse structures which are growable can exceed the initial
	///		bandwidth allocation, while those which are not growable are
	///		fixed, and over-allocation will cause an error</li>
	///		
	///		<li>Matrices which are column major typically perform better with
	///		column-oriented operations, and likewise for row major
	///		matrices. Matrix/vector multiplication is row-major, while transpose 
	///		multiplication is column-major.</li>
	///		
	///		<li>Matrices using one-dimensional storage typically exhibit
	///		higher performance than the matrices with two-dimensional storage </li>
	///	</ol>
	///	<p>Based on the original work of: Bjørn-Ove Heimsund (2003).</p>
	/// <p>Port: Joannes Vermorel (2004).</p>
	/// </summary>
	public class NamespaceDoc
	{
		// nothing, documentation only.
	}

}
