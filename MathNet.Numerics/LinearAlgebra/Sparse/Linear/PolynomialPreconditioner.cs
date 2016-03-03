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
	/// <summary> Neumann series preconditioner.
	/// Expands the inverse of the matrix into a Neumann series, ie.
	/// <c>A<sup>-1</sup>=I + (I-A) + (I-A)<sup>2</sup> + ...</c>. This
	/// series only converge if the norm of A is less than 1. If the norm 
	/// is greater, a scaling factor may be supplied.
	/// </summary>
	public class PolynomialPreconditioner : AbstractPreconditioner, IPreconditioner
	{
		/// <summary> Sets matrix to contain I-wA</summary>
		public virtual IMatrix Matrix
		{
			set { this.Am = value; }

		}

		/// <summary> Number of terms to use in the expansion</summary>
		private int terms;

		/// <summary> The matrix I-wA</summary>
		private IMatrix Am;

		/// <summary> Scaling factor</summary>
		private double w;

		/// <summary> Creates preconditioner with a default of two terms and scaling equal 1.</summary>
		public PolynomialPreconditioner()
		{
			setParameters(1.0, 2);
		}

		/// <summary> Sets parameters</summary>
		/// <param name="w">Matrix-scaling.</param>
		/// <param name="terms">Number of terms in the expansion.</param>
		public virtual void setParameters(double w, int terms)
		{
			this.w = w;
			this.terms = terms;
		}

		public override IVector Apply(IMatrix A, IVector b, IVector x)
		{
			if (terms < 1)
				return x;

			checkSizes(A, b, x);

			IVector temp = Factory.createVector(b), acc = Factory.createVector(b);

			// The first term
			x = Blas.Default.Copy(b, x);

			// Terms involving multiplication
			for (int i = 1; i < terms; ++i)
			{
				// Compute (I-wA)^j b
				for (int j = 0; j < i; ++j)
				{
					temp = Blas.Default.Mult(A, acc, temp);
					acc = Blas.Default.Copy(temp, acc); // acc *= A'
				}

				// Add scaled result into x
				x = Blas.Default.Add(1.0/w, acc, x);
			}

			return x;
		}

		public override IVector TransApply(IMatrix A, IVector b, IVector x)
		{
			if (terms < 1)
				return x;

			checkSizes(A, b, x);

			IVector temp = Factory.createVector(b), acc = Factory.createVector(b);

			// The first term
			x = Blas.Default.Copy(b, x);

			// Terms involving multiplication
			for (int i = 1; i < terms; ++i)
			{
				// Compute (I-wA)^j b
				acc = Blas.Default.Copy(b, acc);
				for (int j = 0; j < i; ++j)
				{
					temp = Blas.Default.TransMult(A, acc, temp);
					acc = Blas.Default.Copy(temp, acc); // acc *= A'
				}

				// Add scaled result into x
				x = Blas.Default.Add(1.0/w, acc, x);
			}

			return x;
		}

		public override void Setup(IMatrix A)
		{
			// Am = I-wA
			if (terms > 1)
				Am = Blas.Default.AddDiagonal(1.0, Blas.Default.Scale(- w, Blas.Default.Copy(A, Am)));
		}
	}
}