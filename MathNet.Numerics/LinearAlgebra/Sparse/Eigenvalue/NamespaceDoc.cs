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

namespace MathNet.Numerics.LinearAlgebra.Sparse.Eigenvalue
{
	// TODO: testing suite missing for LinearAlgebra.Sparse.Iteration.Eigenvalue

	/// <summary>
	/// Iterative eigenvalue solvers. Solves
	/// <code>A x = eig x</code>
	/// <p>for both the eigenvector <c>x</c> and the eigenvalue <c>eig</c>. 
	/// However, since MathNet works only on double-precision numbers, only real 
	/// eigenvalues will be revealed.</p>
	/// 
	/// <h3>Iterative eigenvalue solvers</h3>
	/// <p>The following example illustrates the basic usage:</p>
	/// <code>
	/// double[] EigenvalueSolver(Matrix A, int n) 
	/// {
	///		EigenvalueSolver solver = new PowerIteration();
	///		Vector[] x = new Vector[n];
	///		double[] eig = new double[n];
	///		
	///		for (int i = 0; i &lt; n; ++i)
	///			x[i] = new DenseVector(A.RowCount);
	///		
	///		try 
	///		{
	///			solver.Solve(A, eig, x);
	///		} 
	///		catch (NotConvergedException e) 
	///		{
	///			Console.WriteLine("Eigenvalue solver did not converge");
	///			Console.WriteLine("Number of iterations = " + e.Iterations);
	///			Console.WriteLine("Reason = ");
	///			if (e.Reason == NotConvergedException.BREAKDOWN)
	///				Console.WriteLine("breakdown");
	///			else if (e.Reason == NotConvergedException.DIVERGENCE)
	///				Console.WriteLine("divergence");
	///			else if (e.Reason == NotConvergedException.ITERATIONS)
	///				Console.WriteLine("too many iterations");
	///		}
	///		return eig;
	///	}
	///	</code>
	///	
	///	<p>Note that the arrays holding the eigenvectors and the eigenvalues 
	///	must be allocated. There are two solvers</p>
	///	<ul>
	///		<li><see cref="PowerIterationSolver"/></li>
	///		<li><see cref="LanczosSolver"/></li>
	///	</ul>
	///	<p>Like the linear solvers, an iteration object can be attached. 
	///	The main difference is that there are multiple states to monitor,
	///	that is, many eigenvalue/eigenvector pairs. Consequently, the
	///	iteration monitors are slightly different.</p>
	///	
	///	<h3>Eigenvalue transformations</h3>
	///	<p>MathNet uses eigenvalue transformations to generate methods 
	///	such as the inverse iteration or Rayleigh quotient iteration. Use 
	///	<sse cref="IEigenvalueSolver.Transformation"/> to set a new 
	///	transformation. Transformations present include</p>
	///	<ul>
	///		<li><see cref="NormalEigenvalueTransformation"/> - the default. 
	///		No spectral transformation performed.</li>
	///		<li><see cref="ShiftEigenvalueTransformation"/> - shifts the 
	///		spectrum continuously, using the latest eigenvalue estimate.</li>
	///		<li><see cref="LockedShiftEigenvalueTransformation"/> - uses a 
	///		fixed shift.</li>
	///		<li><see cref="ShiftInvertEigenvalueTransformation"/> - shifts 
	///		the spectrum continuously, and applies the inverse matrix operator.</li>
	///		<li><see cref="LockedShiftInvertEigenvalueTransformation"/> - uses a 
	///		fixed shift, and thus the same inverted matrix.</li>
	///	</ul>
	///	<p>As an example, using <c>LockedShiftInvertEigenvalueTransformation</c> 
	///	with <c>PowerIteration</c> gives the inverse iteration, while using 
	///	<c>ShiftInvertEigenvalueTransformation</c> gives the Rayleigh quotient 
	///	iteration. Also, note that <c>ShiftInvertEigenvalueTransformation</c>
	///	does not work properly with <c>LanczosSolver</c></p>.
	/// </summary>
	public class NamespaceDoc
	{
		// for documentation only.
	}
}
