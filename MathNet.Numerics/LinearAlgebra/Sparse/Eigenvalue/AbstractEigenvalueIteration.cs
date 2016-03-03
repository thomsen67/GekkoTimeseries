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
	/// <summary> Partial implementation of EigenvalueIteration</summary>
	public abstract class AbstractEigenvalueIteration : AbstractIteration, IEigenvalueIteration
	{
		// TODO: merge AbstractEigenvalueIteration and DefaultEigenvalueIteration ?

		public virtual IEigenvalueIterationMonitor IterationMonitor
		{
			get { return monitor; }
			set { this.monitor = value; }
		}

		public virtual NORMS NormType
		{
			get { return norm_type; }
			set { this.norm_type = value; }
		}

		/// <summary> Local reference</summary>
		protected internal IBLAS blas;

		/// <summary> Iteration monitor</summary>
		protected internal IEigenvalueIterationMonitor monitor;

		/// <summary> Vector-norm</summary>
		protected internal NORMS norm_type;

		/// <summary> Constructor for AbstractEigenvalueIteration.</summary>
		public AbstractEigenvalueIteration()
		{
			this.blas = Blas.Default;
			monitor = new NullEigenvalueIterationMonitor();
			norm_type = NORMS.NORM2;
		}

		public virtual bool Converged(IVector[] r, double[] eig, IVector[] x)
		{
			return Converged(ComputeNorms(r), eig, x);
		}

		public virtual bool Converged(IVector[] r, double[] eig)
		{
			return Converged(ComputeNorms(r), eig);
		}

		public virtual bool Converged(IVector[] r)
		{
			return Converged(ComputeNorms(r));
		}

		public virtual bool Converged(double[] r, double[] eig, IVector[] x)
		{
			monitor.Monitor(r, eig, x, iter);
			return ConvergedI(r);
		}

		public virtual bool Converged(double[] r, double[] eig)
		{
			monitor.Monitor(r, eig, iter);
			return ConvergedI(r);
		}

		public virtual bool Converged(double[] r)
		{
			monitor.Monitor(r, iter);
			return ConvergedI(r);
		}

		protected internal abstract bool ConvergedI(double[] r);

		private double[] ComputeNorms(IVector[] r)
		{
			double[] rs = new double[r.Length];
			for (int i = 0; i < r.Length; ++i)
				rs[i] = Blas.Default.Norm(r[i], norm_type);
			return rs;
		}
	}
}