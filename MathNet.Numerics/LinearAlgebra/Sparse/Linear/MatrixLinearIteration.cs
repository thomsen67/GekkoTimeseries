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
	/// <summary> Linear iteration object based on matrix norms.
	/// Extends the default linear iteration object to compare with the norm of the
	/// system matrix and the right hand side. Can often be a better convergence
	/// criteria than the default, but requires the computation of the matrix norm.
	/// </summary>
	public class MatrixLinearIteration : DefaultLinearIteration, Iteration, ILinearIteration
	{
		/// <summary> Norm of the system matrix</summary>
		protected internal double normA;

		/// <summary> Norm of the right hand side</summary>
		protected internal double normb;

		/// <summary> Constructor for MatrixLinearIteration. Default is 100000 iterations at
		/// most, relative tolerance of 1e-5, absolute tolerance of 1e-50 and a
		/// divergence tolerance of 1e+5.
		/// </summary>
		public MatrixLinearIteration() : base()
		{
		}

		/// <summary> Sets the norms</summary>
		/// <param name="normA">Norm of the system matrix
		/// </param>
		/// <param name="normb">Norm of the right hand side
		/// </param>
		public virtual void setNorms(double normA, double normb)
		{
			this.normA = normA;
			this.normb = normb;
		}

		protected internal override bool ConvergedI(double r, IVector x)
		{
			// Store initial residual
			if (IsFirst)
				initR = r;

			// Check for convergence
			if (r < Math.Max(rtol*(normA*Blas.Default.Norm(x, normType_) + normb), atol))
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

		protected internal override bool ConvergedI(double r)
		{
			throw new NotSupportedException();
		}
	}
}