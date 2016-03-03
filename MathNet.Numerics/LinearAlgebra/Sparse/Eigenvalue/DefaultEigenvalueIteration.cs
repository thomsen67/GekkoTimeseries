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
	/// <summary> Default eigenvalue iteration object</summary>
	internal class DefaultEigenvalueIteration : AbstractEigenvalueIteration, IEigenvalueIteration
	{
		private double ctol, dtol;

		private int maxIter;

		/// <summary> Constructor for DefaultEigenvalueIteration. Default is 100000 iterations
		/// at most, convergence tolerance of 1e-3 and divergence tolerance of 1e+5.
		/// </summary>
		public DefaultEigenvalueIteration() : base()
		{
			this.maxIter = 100000;
			this.ctol = 1e-5;
			this.dtol = 1e+5;
		}

		/// <param name="ctol">New convergence tolerance.</param>
		/// <param name="dtol">New divergence tolerance.</param>
		/// <param name="maxIter">Maximum number of iterations.</param>
		public virtual void setParameters(double ctol, double dtol, int maxIter)
		{
			this.maxIter = maxIter;
			this.ctol = ctol;
			this.dtol = dtol;
		}

		protected internal override bool ConvergedI(double[] r)
		{
			bool converged = true;

			// Too many iterations?
			if (iter > maxIter)
				throw new NotConvergedException(NotConvergedException.ITERATIONS_, iter);

			for (int i = 0; i < r.Length; ++i)
			{
				// Check for convergence
				converged &= r[i] < ctol;

				// Check for divergence
				if (r[i] > dtol) throw new NotConvergedException(NotConvergedException.DIVERGENCE, iter);
				if (double.IsNaN(r[i]))
					throw new NotConvergedException(NotConvergedException.DIVERGENCE, iter);
			}

			return converged;
		}
	}
}