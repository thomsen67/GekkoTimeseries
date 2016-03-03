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

namespace MathNet.Numerics.LinearAlgebra.Sparse.Linear
{
	/// <summary> Chebyshev solver.
	/// Solves the symmetric positive definite linear system <c>Ax = b</c>
	/// using the Preconditioned Chebyshev Method. Chebyshev requires an acurate
	/// estimate on the bounds of the spectrum of the matrix.
	/// </summary>
	/// <author>  Templates </author>
	public class ChebyshevSolver : AbstractLinearSolver, ILinearSolver
	{
		/// <summary> Estimates for the eigenvalue of the matrix</summary>
		private double eigmin, eigmax;

		protected internal override void SolveI(IMatrix A, IVector b, IVector x)
		{
			double alpha = 0, beta = 0.0, c = 0.0, d = 0.0;
			IVector[] temp = Factory.createVectors(b, 3);
			IVector p = temp[0], z = temp[1], r = temp[2];

			r = Blas.Default.MultAdd(- 1.0, A, x, b, r);

			c = (eigmax - eigmin)/2.0;
			d = (eigmax + eigmin)/2.0;

			for (iter.Reset(); !iter.Converged(r, x); iter.MoveNext())
			{
				z = M.Apply(A, r, z); // apply preconditioner

				if (iter.IsFirst)
				{
					p = Blas.Default.Copy(z, p);
					alpha = 2.0/d;
				}
				else
				{
					beta = alpha*(c*c/4); // calculate new beta
					alpha = 1.0/(d - beta); // calculate new alpha
					p = Blas.Default.Add(z, beta, p); // update search direction
				}

				x = Blas.Default.Add(alpha, p, x); // update approximation vector
				r = Blas.Default.MultAdd(- 1.0, A, x, b, r); // compute residual
			}
		}

		/// <summary> Sets eigenvalue estimates</summary>
		/// <param name="eigmin">Smallest eigenvalue</param>
		/// <param name="eigmax">Largest eigenvalue</param>
		public virtual void SetEigenvalues(double eigmin, double eigmax)
		{
			this.eigmax = eigmax;
			this.eigmin = eigmin;
		}
	}
}