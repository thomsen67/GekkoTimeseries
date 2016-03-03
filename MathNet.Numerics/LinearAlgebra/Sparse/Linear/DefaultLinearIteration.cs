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

using MathNet.Numerics.LinearAlgebra.Sparse;

namespace MathNet.Numerics.LinearAlgebra.Sparse.Linear
{
	/// <summary> Default linear iteration object.
	/// This tester checks declares convergence if the absolute value of the residual
	/// norm is sufficiently small, or if the relative decrease is small.
	/// Divergence is decleared if too many iterations are spent, or the
	/// residual has grown too much. NaNs will also cause divergence to be flagged.
	/// </summary>
	public class DefaultLinearIteration : AbstractLinearIteration, Iteration, ILinearIteration
	{
		/// <summary> Initial residual</summary>
		protected internal double initR;

		/// <summary> Relative tolerance</summary>
		protected internal double rtol;

		/// <summary> Absolute tolerance</summary>
		protected internal double atol;

		/// <summary> Divergence tolerance</summary>
		protected internal double dtol;

		/// <summary> Maximum number of iterations</summary>
		protected internal int maxIter;

		/// <summary> Constructor for DefaultLinearIteration. Default is 100000 iterations at
		/// most, relative tolerance of 1e-5, absolute tolerance of 1e-50 and a
		/// divergence tolerance of 1e+5.
		/// </summary>
		public DefaultLinearIteration() : base()
		{            
            this.maxIter = 100000;
			this.rtol = 1e-5;
			this.atol = 1e-50;
			this.dtol = 1e+5;
		}

		/// <param name="rtol">New relative tolerance</param>
		/// <param name="atol">New absolute tolerance</param>
		/// <param name="dtol">New divergence tolerance</param>
		/// <param name="maxIter">Maximum number of iterations</param>
		public virtual void SetParameters(double rtol, double atol, double dtol, int maxIter)
		{
			this.maxIter = maxIter;
			this.rtol = rtol;
			this.atol = atol;
			this.dtol = dtol;
		}

		protected internal override bool ConvergedI(double r)
		{
			// Store initial residual
			if (IsFirst)
				initR = r;

			// Check for convergence
			if (r < Math.Max(rtol*initR, atol))
				return true;

			// Check for divergence
			if (r > dtol*initR)
				throw new LinearNotConvergedException(NotConvergedException.DIVERGENCE, iter, r);
			if (iter > maxIter)
				throw new LinearNotConvergedException(NotConvergedException.ITERATIONS_, iter, r);
			if (double.IsNaN(r))
				throw new LinearNotConvergedException(NotConvergedException.DIVERGENCE, iter, r);

			// Neither convergence nor divergence
			return false;
		}

		protected internal override bool ConvergedI(double r, IVector x)
		{
			return ConvergedI(r);
		}
	}
}