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
using System.IO;
using System.Text;

namespace MathNet.Numerics.LinearAlgebra.Sparse.Linear
{
	/// <summary> BiCG solver.
	/// BiCG solves the unsymmetric linear system <c>Ax = b</c> using the
	/// Preconditioned BiConjugate Gradient method.
	/// </summary>
	/// <author>  Templates </author>
	public class BiCGSolver : AbstractLinearSolver, ILinearSolver
	{
		protected internal override void SolveI(IMatrix A, IVector b, IVector x)
		{
			double rho_1 = 1.0, rho_2 = 1.0, alpha = 1.0, beta = 1.0;
			IVector[] temp = Factory.createVectors(b, 8);
			IVector z = temp[0], p = temp[1], q = temp[2], r = temp[3], ztilde = temp[4], ptilde = temp[5], qtilde = temp[6], rtilde = temp[7];

			r = Blas.Default.MultAdd(- 1.0, A, x, b, r);
			rtilde = Blas.Default.Copy(r, rtilde);

            int it = 0;

			for (iter.Reset(); !iter.Converged(r, x); iter.MoveNext())
			{
                it++;
                
                z = M.Apply(A, r, z);
				ztilde = M.TransApply(A, rtilde, ztilde);
				rho_1 = Blas.Default.Dot(z, rtilde);

				if (rho_1 == 0.0)
					throw new LinearNotConvergedException(NotConvergedException.BREAKDOWN, iter.IterationCount, "rho", Blas.Default.Norm(r, iter.NormType));

				if (iter.IsFirst)
				{
					p = Blas.Default.Copy(z, p);
					ptilde = Blas.Default.Copy(ztilde, ptilde);
				}
				else
				{
					beta = rho_1/rho_2;
					p = Blas.Default.Add(z, beta, p);
					ptilde = Blas.Default.Add(ztilde, beta, ptilde);
				}

				q = Blas.Default.Mult(A, p, q);
				qtilde = Blas.Default.TransMult(A, ptilde, qtilde);

				alpha = rho_1/Blas.Default.Dot(ptilde, q);
				x = Blas.Default.Add(alpha, p, x);
				r = Blas.Default.Add(- alpha, q, r);
				rtilde = Blas.Default.Add(- alpha, qtilde, rtilde);

				rho_2 = rho_1;

                //if(it>20)break;
			}
            Console.WriteLine("iteration " + it);
		}
	}
}