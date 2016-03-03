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
	/// <summary> Generic power iteration eigenvalue solver. 
	/// Using <see cref="NormalEigenvalueTransformation"/> gives the Power method,
	/// <see cref="LockedShiftInvertEigenvalueTransformation"/> will produce 
	/// the Inverse iteration, while <see cref="ShiftInvertEigenvalueTransformation"/> 
	/// gives the Rayleigh quotient iteration.</summary>
	public class PowerIterationSolver : AbstractEigenvalueSolver, IEigenvalueSolver
	{
		protected internal override void SolveI(IMatrix A, double[] eig, IVector[] x)
		{
			int n = x.Length;
			IVector Ax = Factory.createVector(x[0]);

			// Residual vectors
			IVector[] r = Factory.createVectors(x);
			for (int i = 0; i < n; ++i)
				r[i] = Blas.Default.SetVector(1.0, r[i]);

			x = Normalize(x);

			for (iter.Reset(); !iter.Converged(r, eig, x); iter.MoveNext())
			{
				// One step for each eigenvector
				for (int i = 0; i < n; ++i)
				{
					// Improve on the eigenvector, and compute the eigenvalue
					x[i] = Normalize0(x[i]);
					et.Shift = eig[i];
					Ax = et.Apply(A, x[i], Ax);
					eig[i] = Blas.Default.Dot(x[i], Ax);

					// Compute residual, and cycle vectors
					r[i] = Blas.Default.Add(Ax, - eig[i], x[i], r[i]); // r = A*x-eig*x
					x[i] = Blas.Default.Copy(Ax, x[i]); // x = A*x
				}
			}

			// Untransform the eigenvalues
			et.Eigenvalue(eig);
		}
	}
}