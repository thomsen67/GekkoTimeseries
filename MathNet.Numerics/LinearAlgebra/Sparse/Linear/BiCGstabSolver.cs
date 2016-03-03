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
	/// <summary> BiCG stablized solver.
	/// BiCGstab solves the unsymmetric linear system <code>Ax = b</code>
	/// using the Preconditioned BiConjugate Gradient Stabilized method
	/// </summary>
	/// <author>Templates</author>
	public class BiCGstabSolver : AbstractLinearSolver, ILinearSolver
	{
		protected internal override void SolveI(IMatrix A, IVector b, IVector x)
		{
			double rho_1 = 1.0, rho_2 = 1.0, alpha = 1.0, beta = 1.0, omega = 1.0;
			IVector[] tempv = Factory.createVectors(b, 9);
			IVector p = tempv[0], s = tempv[1], phat = tempv[2], shat = tempv[3], t = tempv[4], v = tempv[5], temp = tempv[6], r = tempv[7], rtilde = tempv[8];

			r = Blas.Default.MultAdd(- 1.0, A, x, b, r);
			rtilde = Blas.Default.Copy(r, rtilde);

			for (iter.Reset(); !iter.Converged(r, x); iter.MoveNext())
			{
				rho_1 = Blas.Default.Dot(rtilde, r);
				if (rho_1 == 0)
					throw new LinearNotConvergedException(NotConvergedException.BREAKDOWN, iter.IterationCount, "rho", Blas.Default.Norm(r, iter.NormType));
				if (omega == 0)
					throw new LinearNotConvergedException(NotConvergedException.BREAKDOWN, iter.IterationCount, "omega", Blas.Default.Norm(r, iter.NormType));

				if (iter.IsFirst)
					p = Blas.Default.Copy(r, p);
				else
				{
					beta = (rho_1/rho_2)*(alpha/omega);
					temp = Blas.Default.Add(- omega, v, p, temp);
					p = Blas.Default.Add(beta, temp, r, p);
				}
				phat = M.Apply(A, p, phat);
				v = Blas.Default.Mult(A, phat, v);
				alpha = rho_1/Blas.Default.Dot(rtilde, v);
				s = Blas.Default.Add(- alpha, v, r, s);
				if (iter.Converged(s, x))
				{
					x = Blas.Default.Add(alpha, phat, x);
					return;
				}
				shat = M.Apply(A, s, shat);
				t = Blas.Default.Mult(A, shat, t);
				omega = Blas.Default.Dot(t, s)/Blas.Default.Dot(t, t);
				temp = Blas.Default.Add(alpha, phat, omega, shat, temp);
				x = Blas.Default.Add(temp, x);
				r = Blas.Default.Add(- omega, t, s, r);

				rho_2 = rho_1;
			}
		}
	}
}