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

namespace MathNet.Numerics.LinearAlgebra.Sparse.Mixed
{
	/// <summary> Uzawa algorithm using the CG method</summary>
	public class CGUzawaSolver : AbstractMixedSolver
	{
		protected internal override void solveI(IMatrix A, IMatrix B, IMatrix Bt,
			IMatrix C, IVector q, IVector u, IVector f, IVector g)
		{
			IVector[] tempq = Factory.createVectors(q, 3);
			IVector[] tempu = Factory.createVectors(u, 4);
			IVector ur = tempq[0], p = tempq[1], h = tempq[2];
			IVector r = tempu[0], rhat = tempu[1], d = tempu[2], t = tempu[3];

			ur = Blas.Default.MultAdd(- 1.0, Bt, u, f, ur); // ur = f - Bt*u
			q = solver.Solve(A, ur, q); // q = A \ ur = A \ (f - Bt*u)

			t = Blas.Default.MultAdd(- 1.0, C, u, g, t); // t = g - C*u
			r = Blas.Default.MultAdd(- 1.0, B, q, t, r); // r = t - B*q = g - B*q - C*u
			d = Blas.Default.ScaleCopy(- 1.0, r, d); // d = -r

			for (iter.Reset(); !iter.Converged(r); iter.MoveNext())
			{
				p = Blas.Default.Mult(Bt, d, p); // p = Bt*d
				h = solver.Solve(A, p, h); // h = A\p

				double alpha = Blas.Default.Dot(r, r)/Blas.Default.Dot(p, h);
				u = Blas.Default.Add(alpha, d, u); // u = u + alpha*d
				q = Blas.Default.Add(- alpha, h, q); // q = q - alpha*h

				rhat = Blas.Default.Copy(r, rhat); // rhat = r
				t = Blas.Default.MultAdd(- 1.0, C, u, g, t); // t = g - C*u
				r = Blas.Default.MultAdd(- 1.0, B, q, t, r); // r = t - B*q = g - B*q - C*u
				double beta = Blas.Default.Dot(r, r)/Blas.Default.Dot(rhat, rhat);
				d = Blas.Default.Add(- 1.0, r, beta, d); // d = -r + beta*d
			}
		}
	}
}