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

using MathNet.Numerics.LinearAlgebra.Sparse;

namespace MathNet.Numerics.LinearAlgebra.Sparse.Eigenvalue
{
	/// <summary> Iteration for an eigenvalue problem</summary>
	public interface IEigenvalueIteration : Iteration
	{
		// TODO: merge IEigenvalueIteration with DefaultEigenValueIteration

		/// <summary>Gets or sets current iteration monitor</summary>
		IEigenvalueIterationMonitor IterationMonitor { get; set; }

		/// <summary> Gets or sets the vector-norm in use</summary>
		NORMS NormType { get; set; }

		/// <summary> Checks for convergence</summary>
		/// <param name="r">Residual-vectors</param>
		/// <param name="eig">Eigenvalues</param>
		/// <param name="x">Eigenvectors</param>
		/// <returns> True if converged</returns>
		bool Converged(IVector[] r, double[] eig, IVector[] x);

		/// <summary> Checks for convergence</summary>
		/// <param name="r">Residual-vectors</param>
		/// <param name="lambda">Eigenvalues</param>
		/// <returns> True if converged</returns>
		bool Converged(IVector[] r, double[] lambda);

		/// <summary> Checks for convergence</summary>
		/// <param name="r">Residual-vectors</param>
		/// <returns> True if converged</returns>
		bool Converged(IVector[] r);

		/// <summary> Checks for convergence</summary>
		/// <param name="r">Residual-vectors</param>
		/// <param name="eig">Eigenvalues</param>
		/// <param name="x">Eigenvectors</param>
		/// <returns> True if converged</returns>
		bool Converged(double[] r, double[] eig, IVector[] x);

		/// <summary> Checks for convergence</summary>
		/// <param name="r">Residual-vectors</param>
		/// <param name="eig">Eigenvalues</param>
		/// <returns> True if converged</returns>
		bool Converged(double[] r, double[] eig);

		/// <summary> Checks for convergence</summary>
		/// <param name="r">Residual-vectors</param>
		/// <returns> True if converged</returns>
		bool Converged(double[] r);
	}
}