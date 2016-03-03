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
using System;

namespace MathNet.Numerics.LinearAlgebra.Sparse.Linear
{
	/// <summary> Conjugate Gradients squared solver.
	/// CGS solves the unsymmetric linear system <c>Ax = b</c> using the
	/// Conjugate Gradient Squared method.
	/// </summary>
	public class CGSSolver : AbstractLinearSolver, ILinearSolver
	{
		protected internal override void SolveI(IMatrix A, IVector b, IVector x)
		{
			double rho_1 = 0, rho_2 = 0, alpha = 0, beta = 0;
			IVector[] temp = Factory.createVectors(b, 10);
			IVector p = temp[0], q = temp[1], u = temp[2], phat = temp[3], qhat = temp[4], vhat = temp[5], uhat = temp[6], sum = temp[7], r = temp[8], rtilde = temp[9];

			r = Blas.Default.MultAdd(- 1.0, A, x, b, r);
			rtilde = Blas.Default.Copy(r, rtilde);

            int it = 0;

			for (iter.Reset(); !iter.Converged(r, x); iter.MoveNext())
			{
				rho_1 = Blas.Default.Dot(rtilde, r);  //two vectors multiplied, 2464 multiplications
				if (rho_1 == 0)
					throw new LinearNotConvergedException(NotConvergedException.BREAKDOWN, iter.IterationCount, "rho", Blas.Default.Norm(r, iter.NormType));
				if (iter.IsFirst)
				{
					u = Blas.Default.Copy(r, u);  //uses both a loop where a 2464 array is set to 0, and an arraycopy
					p = Blas.Default.Copy(u, p);
				}
				else
				{
					beta = rho_1/rho_2;
					u = Blas.Default.Add(beta, q, r, u);
					sum = Blas.Default.Add(beta, p, q, sum);
					p = Blas.Default.Add(beta, sum, u, p); // p = u + beta*(beta*p+q)
				}

				phat = M.Apply(A, p, phat);
				vhat = Blas.Default.Mult(A, phat, vhat);
				alpha = rho_1/Blas.Default.Dot(rtilde, vhat);
				q = Blas.Default.Add(- alpha, vhat, u, q);

				sum = Blas.Default.Add(u, q, sum); // sum = u + q 
				uhat = M.Apply(A, sum, uhat);
				x = Blas.Default.Add(alpha, uhat, x);
				qhat = Blas.Default.Mult(A, uhat, qhat);
				r = Blas.Default.Add(- alpha, qhat, r);

				rho_2 = rho_1;

                it++;
			}
            //Console.WriteLine("its " + it);
		}
	}
}