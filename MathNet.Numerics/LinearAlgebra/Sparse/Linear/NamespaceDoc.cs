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

namespace MathNet.Numerics.LinearAlgebra.Sparse.Linear
{
	/// <summary>
	/// Iterative linear solvers and preconditioners. Solves the linear problem
	/// <code>A x = b</code>
	/// <p>with respect to <c>x</c>. A must be an <c>n*n</c>
	/// non-singular matrix, and <c>x</c> and <c>b</c> are
	/// <c>n</c>-vectors. Note that the value of <c>x</c> is
	/// used as an initial guess, and the convergence can be improved by
	/// using a good guess.</p>
	/// 
	/// <h3>Iterative linear solvers</h3>
	/// <p>The solvers are modified versions of the 
	/// <a href="http://www.netlib.org/templates">Templates</a>
	/// versions. All the solvers implement the interface 
	/// <see cref="ILinearSolver"/>, and its usage can be
	/// illustrated by an example:</p>
	/// 
	/// <code>
	/// double[] Solver(Matrix A, Vector x, Vector b) 
	/// {
	///		LinearSolver solver = new CGSolver();
	///		try 
	///		{
	///			x = solver.Solve(A, x, b);
	///		} 
	///		catch (LinearNotConvergedException e) 
	///		{
	///			Console.WriteLine("Linear solver did not converge");
	///			Console.WriteLine("Final residual = " + e.Residual);
	///			Console.WriteLine("Number of iterations = " + e.Iterations);
	///			Console.WriteLine("Reason = ");
	///			if (e.Reason == NotConvergedException.BREAKDOWN)
	///				Console.WriteLine("breakdown");
	///			else if (e.Reason == NotConvergedException.DIVERGENCE)
	///				Console.WriteLine("divergence");
	///			else if (e.getReason() == NotConvergedException.ITERATIONS)
	///				Console.WriteLine("too many iterations");
	///		}
	///		return Blas.Default.GetArrayCopy(x);
	///	}
	///	</code>
	///	
	///	<p>In this example, the solver <see cref="CGSolver"/> was
	///	used. Other solvers include the following:</p>
	///	
	///	<table>
	///	<tr>
	///		<th>Solver </th>
	///		<th>SPD </th>
	///		<th>Transpose </th>
	///		<th>Notes </th>
	///	</tr>
	///	<tr>
	///		<td><see cref="BiCGSolver"/></td>
	///		<td> </td>
	///		<td>Yes </td>
	///		<td> </td>
	///	</tr>
	///	<tr>
	///		<td><see cref="BiCGstabSolver"/></td>
	///		<td> </td>
	///		<td> </td>
	///		<td> </td>
	///	</tr>
	///	<tr>
	///		<td><see cref="CGSolver"/></td>
	///		<td>Yes </td>
	///		<td> </td>
	///		<td> </td>
	///	</tr>
	///	<tr>
	///		<td><see cref="CGSSolver"/></td>
	///		<td> </td>
	///		<td> </td>
	///		<td> </td>
	///	</tr>
	///	<tr>
	///		<td><see cref ="ChebyshevSolver"/></td>
	///		<td>Yes </td>
	///		<td> </td>
	///		<td>Needs extremal eigenvalues </td>
	///	</tr>
	///	<tr>
	///		<td><see cref="GMRESSolver"/></td>
	///		<td> </td>
	///		<td> </td>
	///		<td>Restarted version </td>
	///	</tr>
	///	<tr>
	///		<td><see cref="IRSolver"/></td>
	///		<td> </td>
	///		<td> </td>
	///		<td> </td>
	///	</tr>
	///	<tr>
	///		<td><see cref="QMRSolver"/></td>
	///		<td> </td>
	///		<td>Yes </td>
	///		<td>Uses left and right preconditioning </td>
	///	</tr>
	///	</table>
	///	
	///	<p>SPD means that the matrix must be symmetrical, positive
	///	definite, while transpose means that transpose matrix/vector
	///	multiplication and preconditioning is necessary.</p>
	///	
	///	<h3>Convergence criteria</h3>
	///	<p><see cref="ILinearIterationMonitor"/> monitors the iteration, 
	///	and provides both convergence tracking and detection. There are 
	///	two implementations of <c>ILinearIterationMonitor</c>:
	///	</p>
	///	<ul>
	///		<li><see cref="DefaultLinearIteration"/> declares convergence 
	///		when the current residual has decrease by a certain factor 
	///		compared to the initial residual.</li>
	///		<li><see cref="MatrixLinearIteration"/> a <c>DefaultLinearIteration</c>, 
	///		but the matrix-norm to scale the initial residual.</li>
	///	</ul>
	///	<p>The method <see cref="ILinearSolver.Iteration"/> is used to set a 
	///	<c>ILinearIteration</c>.</p>
	///	
	///	<p>Another use of the iteration objects is in the monitoring they
	///	can perform.  By default, no monitoring is done, but the
	///	method <see cref="ILinearIteration.Monitor"/> allows one to attach a
	///	monitor. The following are available:</p>
	///	<ul>
	///		<li><see cref="ArrayLinearIterationMonitor"/> stores the iteration 
	///		progress in an array.</li>
	///		<li><see cref="OutputLinearIterationMonitor"/> outputs the information 
	///		to a stream, by default <c>System.Console.Err</c>.</li>
	///	</ul>
	///	<p>An easy way to attach a monitor without changing the iteration
	///	object is the following:</p>
	///	<code>
	///	LinearSolver solver = new CGSolver();
	///	solver.Iteration.Monitor = new OutputLinearIterationMonitor();
	///	</code>
	///	
	///	<h3>Preconditioning</h3>
	///	<p>To speed up convergence of the iterative solvers,
	///	preconditioners are often necessary. Use the method 
	///	<see cref="ILinearSolver.Preconditioner"/> to set the 
	///	preconditioner. Available preconditoners are:</p>
	///	<table>
	///	<tr>
	///		<th>Preconditioner</th>
	///		<th>Notes</th>
	///	</tr>
	///	<tr>
	///		<td><see cref="GaussSeidelPreconditioner"/></td>
	///		<td></td>
	///	</tr>
	///	<tr>
	///		<td><see cref="ICCPreconditioner"/></td>
	///		<td>Optional fill-in and diagonal scaling.</td>
	///	</tr>
	///	<tr>
	///		<td><see cref="ILUPreconditioner"/></td>
	///		<td>Optional fill-in and diagonal scaling.</td>
	///	</tr>
	///	<tr>
	///		<td><see cref="PolynomialPreconditioner"/></td>
	///		<td></td>
	///	</tr>
	///	<tr>
	///		<td><see cref="SORPreconditioner"/></td>
	///		<td></td>
	///	</tr>
	///	<tr>
	///		<td><see cref="SSORPreconditioner"/></td>
	///		<td></td>
	///	</tr>
	///	<tr>
	///		<td><see cref="MultigridPreconditioner"/></td>
	///		<td>Geometrical variant.</td>
	///	</tr>
	///	<tr>
	///		<td><see cref="CholeskyPreconditioner"/></td>
	///		<td>For sub-problems only.</td>
	///	</tr>
	///	<tr>
	///		<td><see cref="LUPreconditioner"/></td>
	///		<td>For sub-problems only</td>
	///	</tr>
	///	<tr>
	///		<td><see cref="QRPreconditioner"/></td>
	///		<td>For sub-problems only</td>
	///	</tr>
	///	<tr>
	///		<td><see cref="CompositePreconditioner"/></td>
	///		<td>Applies a sequence of preconditioners</td>
	///	</tr>
	///	<tr>
	///		<td><see cref="IdentityPreconditioner"/></td>
	///		<td>Default preconditioner, does nothing</td>
	///	</tr>
	///	<tr>
	///		<td><see cref="IterativeSolverPreconditioner"/></td>
	///		<td>Has slack convergence criteria</td>
	///	</tr>
	///	</table>
	///	
	///	<p>For further details on the preconditioners, see the 
	///	<a href="http://www.netlib.org/templates">Templates</a> page.</p>
	///	
	/// </summary>
	public class NamespaceDoc
	{
		// for documentation only
	}
}
