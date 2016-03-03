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
	/// <summary> Partial implementation of a linear iteration object.</summary>
	public abstract class AbstractLinearIteration : AbstractIteration, ILinearIteration
	{
		public virtual NORMS NormType
		{
			get { return normType_; }

			set { this.normType_ = value; }

		}

		public virtual ILinearIterationMonitor Monitor
		{
			get { return monitor_; }

			set { this.monitor_ = value; }

		}

		/// <summary> Local reference</summary>
		protected internal IBLAS blas;

		/// <summary> Vector-norm</summary>
		protected internal NORMS normType_;

		/// <summary> Iteration monitor</summary>
		protected internal ILinearIterationMonitor monitor_;

		/// <summary> Constructor for AbstractLinearIteration. Default norm is the 2-norm with
		/// no monitoring.
		/// </summary>
		public AbstractLinearIteration() : base()
		{
			this.blas = Blas.Default;
			normType_ = NORMS.NORM2;
			monitor_ = new NullLinearIterationMonitor();
		}

		public virtual bool Converged(IVector r, IVector x)
		{
			return Converged(Blas.Default.Norm(r, normType_), x);
		}

		public virtual bool Converged(double r, IVector x)
		{
			monitor_.Monitor(r, x, iter);
			return ConvergedI(r, x);
		}

		public virtual bool Converged(double r)
		{
			monitor_.Monitor(r, iter);
			return ConvergedI(r);
		}

		protected internal abstract bool ConvergedI(double r, IVector x);

		protected internal abstract bool ConvergedI(double r);

		public virtual bool Converged(IVector r)
		{
			return Converged(Blas.Default.Norm(r, normType_));
		}
	}
}