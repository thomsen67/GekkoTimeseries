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
using MathNet.Numerics.LinearAlgebra.Sparse.Linear;

namespace MathNet.Numerics.LinearAlgebra.Sparse.Eigenvalue
{
	/// <summary> Shifts and inverts the operator</summary>
	public class ShiftInvertEigenvalueTransformation : 
		ShiftEigenvalueTransformation, IEigenvalueTransformation
	{
		/// <summary> Sets the linear solver to use. Default is GMRES.</summary>
		public virtual ILinearSolver Solver
		{
			set { this.solver_ = value; }

		}

		new protected internal double sigma;

		protected internal double sigmaOld;

		protected internal IMatrix Am;

		protected internal ILinearSolver solver_;

		/// <summary> Constructor for ShiftInvertEigenvalueTransformation. The matrix Am
		/// must be of the same size as A, and it is overwritten.
		/// </summary>
		public ShiftInvertEigenvalueTransformation(IMatrix Am, IMatrix A) : base()
		{
			this.Am = Blas.Default.Copy(A, Am);
			solver_ = new GMRESSolver();
		}

		public override IVector Apply(IMatrix A, IVector x, IVector y)
		{
			Am = Blas.Default.AddDiagonal(sigmaOld - sigma, Am);
			return solver_.Solve(Am, x, y);
		}

		public override double Eigenvalue(double e)
		{
			return sigma + 1.0/e;
		}

		public override double Shift
		{
			get { return base.Shift; }
			set
			{
				sigmaOld = this.sigma;
				base.Shift = sigma;
			}
		}
	}
}