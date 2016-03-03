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
	/// <summary> Quasi-Minimal Residual method.
	/// QMR solves the unsymmetric linear system <c>Ax = b</c> using the
	/// Quasi-Minimal Residual method. QMR uses two preconditioners, and by default
	/// these are the same preconditioner.
	/// </summary>
	/// <author>  Templates </author>
	public class QMRSolver : AbstractLinearSolver, ILinearSolver
	{
		/// <summary> Left preconditioner</summary>
		private IPreconditioner M1;

		/// <summary> Right preconditioner</summary>
		private IPreconditioner M2;

		public QMRSolver() : base()
		{
			M1 = M;
			M2 = M;
		}

		protected internal override void SolveI(IMatrix A, IVector b, IVector x)
		{
			double rho = 0, rho_1 = 0, xi = 0, gamma = 0, gamma_1 = 0, theta = 0, theta_1 = 0, eta = 0, delta = 0, ep = 0, beta = 0;
			IVector[] temp = Factory.createVectors(b, 14);
			IVector r = temp[0], y = temp[1], z = temp[2], v = temp[3], w = temp[4], p = temp[5], q = temp[6], d = temp[7], s = temp[8], v_tld = temp[9], w_tld = temp[10], y_tld = temp[11], z_tld = temp[12], p_tld = temp[13];

			r = Blas.Default.MultAdd(- 1.0, A, x, b, r);
			v_tld = Blas.Default.Copy(r, v_tld);
			y = M1.Apply(A, v_tld, y);
			rho = Blas.Default.Norm(y, NORMS.NORM2);
			w_tld = Blas.Default.Copy(r, w_tld);
			z = M2.TransApply(A, w_tld, z);
			xi = Blas.Default.Norm(z, NORMS.NORM2);
			gamma = 1.0;
			eta = - 1.0;
			theta = 0.0;

			for (iter.Reset(); !iter.Converged(r, x); iter.MoveNext())
			{

                DenseVector x2 = (DenseVector)x;
                DenseVector b2 = (DenseVector)b;
                DenseVector b2hat = new DenseVector(b2.Length);
                Blas.Default.Mult(A, x2, b2hat);

                //if(true)
                //{
                //    for (int i = 0; i < x2.Length; i++)
                //    {
                //        Console.WriteLine(i + " x " + x2.GetValue(i) + " b " + b2.GetValue(i) + " bhat " + (b2hat.GetValue(i)) + " resbhat " + (b2hat.GetValue(i) - b2.GetValue(i)));
                //    }
                //    Console.WriteLine();
                //}

                if (rho == 0.0)
					throw new LinearNotConvergedException(NotConvergedException.BREAKDOWN, iter.IterationCount, "rho", Blas.Default.Norm(r, iter.NormType));
				if (xi == 0.0)
					throw new LinearNotConvergedException(NotConvergedException.BREAKDOWN, iter.IterationCount, "xi", Blas.Default.Norm(r, iter.NormType));

				v = Blas.Default.ScaleCopy(1.0/rho, v_tld, v);
				y = Blas.Default.Scale(1.0/rho, y);
				w = Blas.Default.ScaleCopy(1.0/xi, w_tld, w);
				z = Blas.Default.Scale(1.0/xi, z);
				delta = Blas.Default.Dot(z, y);
				if (delta == 0.0)
					throw new LinearNotConvergedException(NotConvergedException.BREAKDOWN, iter.IterationCount, "delta", Blas.Default.Norm(r, iter.NormType));

				M2.Apply(A, y, y_tld);
				M1.TransApply(A, z, z_tld);
				if (iter.IsFirst)
				{
					p = Blas.Default.Copy(y_tld, p);
					q = Blas.Default.Copy(z_tld, q);
				}
				else
				{
					p = Blas.Default.Add(y_tld, (- xi)*delta/ep, p);
					q = Blas.Default.Add(z_tld, (- rho)*delta/ep, q);
				}

				p_tld = Blas.Default.Mult(A, p, p_tld);
				ep = Blas.Default.Dot(q, p_tld);
				if (ep == 0.0)
					throw new LinearNotConvergedException(NotConvergedException.BREAKDOWN, iter.IterationCount, "ep", Blas.Default.Norm(r, iter.NormType));
				beta = ep/delta;
				if (beta == 0.0)
					throw new LinearNotConvergedException(NotConvergedException.BREAKDOWN, iter.IterationCount, "beta", Blas.Default.Norm(r, iter.NormType));
				v_tld = Blas.Default.Add(- beta, v, p_tld, v_tld);
				M1.Apply(A, v_tld, y);
				rho_1 = rho;
				rho = Blas.Default.Norm(y, NORMS.NORM2);
				w_tld = Blas.Default.TransMultAdd(A, q, - beta, w, w_tld);
				M2.TransApply(A, w_tld, z);
				xi = Blas.Default.Norm(z, NORMS.NORM2);
				gamma_1 = gamma;
				theta_1 = theta;
				theta = rho/(gamma_1*beta);
				gamma = 1.0/Math.Sqrt(1.0 + theta*theta);
				if (gamma == 0.0)
					throw new LinearNotConvergedException(NotConvergedException.BREAKDOWN, iter.IterationCount, "gamma", Blas.Default.Norm(r, iter.NormType));
				eta = (- eta)*rho_1*gamma*gamma/(beta*gamma_1*gamma_1);
				if (iter.IsFirst)
				{
					d = Blas.Default.ScaleCopy(eta, p, d);
					s = Blas.Default.ScaleCopy(eta, p_tld, s);
				}
				else
				{
					double val = theta_1*theta_1*gamma*gamma;
					d = Blas.Default.Add(eta, p, val, d);
					s = Blas.Default.Add(eta, p_tld, val, s);
				}

				// update approximation vector
				x = Blas.Default.Add(d, x);
				r = Blas.Default.Add(- 1.0, s, r);
			}
		}

		/// <summary> Sets left preconditioner</summary>
		public virtual void setM1(IPreconditioner preconditioner)
		{
			M1 = preconditioner;
		}

		/// <summary> Sets right preconditioner</summary>
		public virtual void setM2(IPreconditioner preconditioner)
		{
			M2 = preconditioner;
		}
	}
}