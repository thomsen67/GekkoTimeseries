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
	/// <summary> Uzawa algorithm for mixed problems. Requires a steplength, but uses length of
	/// <c>1</c> by default.</summary>
	public class UzawaSolver : AbstractMixedSolver
	{
		/// <summary> Sets steplength (alpha) to use in the algorithm</summary>
		public virtual double Steplength
		{
			set { this.alpha = value; }
		}

		/// <summary> Steplength (fixed)</summary>
		private double alpha;

		/// <summary> Constructor for Uzawa. Uses steplength of 1 by default.</summary>
		public UzawaSolver()
		{
			alpha = 1.0;
		}

		protected internal override void solveI(IMatrix A, IMatrix B, IMatrix Bt, 
			IMatrix C, IVector q, IVector u, IVector f, IVector g)
		{
			IVector[] tempq = Factory.createVectors(q, 1);
			IVector[] tempu = Factory.createVectors(u, 2);
			IVector qr = tempq[0], r = tempu[0], t = tempu[1];

			t = Blas.Default.MultAdd(C, u, - 1.0, g, t); // t = C*u - g
			r = Blas.Default.MultAdd(B, q, t, r); // r = B*q + t

			for (iter.Reset(); !iter.Converged(r); iter.MoveNext())
			{
				qr = Blas.Default.MultAdd(- 1.0, Bt, u, f, qr); // qr = f - Bt*u
				q = solver.Solve(A, qr, q); // q = A\qr

				t = Blas.Default.MultAdd(C, u, - 1.0, g, t); // t = C*u - g
				r = Blas.Default.MultAdd(B, q, t, r); // r = B*q + t
				u = Blas.Default.Add(alpha, r, u); // u = u + alpha*r
			}
		}
	}
}