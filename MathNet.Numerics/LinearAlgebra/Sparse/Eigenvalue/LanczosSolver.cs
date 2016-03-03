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

namespace MathNet.Numerics.LinearAlgebra.Sparse.Eigenvalue
{
	/// <summary> Lanczos method. Note that the current implementation is not very advanced</summary>
	public class LanczosSolver : AbstractEigenvalueSolver
	{
		private bool fullOrthogonalization;

		/// <summary> Constructor for Lanczos</summary>
		/// <param name="fullOrthogonalization">True for full orthogonalization, false for partial.</param>
		public LanczosSolver(bool fullOrthogonalization)
		{
			this.fullOrthogonalization = fullOrthogonalization;
		}

		/// <summary> Constructor for Lanczos, using partial orthogonalization</summary>
		public LanczosSolver()
		{
			fullOrthogonalization = false;
		}

		protected internal override void SolveI(IMatrix A, double[] eig, IVector[] x)
		{
			// Number of eigenvalues wanted and the maximum factorization size
			int n = eig.Length, m = A.RowCount;

			// Current factorization size, increment, and new size
			int nv = n, inc = 2, nvn = Math.Min(nv*inc, m);

			// These holds the matrix factorization
			IVector[] q = Factory.createVectors(x);
			for (int i = 0; i < nv; ++i)
				q[i] = Blas.Default.Copy(x[i], q[i]);
			double[] alpha = new double[nv], beta = new double[nv];

			// Work-vector
			IVector z = Factory.createVector(q[0]);

			// Initial factorization
			Lanczos(A, q, alpha, beta, z, 0);

			// Compute the eigenvalues, and estimate residuals
			double[] r = new double[n];
			int[] ind = new int[n];
			EigenvalueDecomposition evd = new EigenvalueDecomposition(alpha, beta);
			EstimateResidual(nv, beta, evd, r, ind);

			// Continue while not converged or until the factorization is complete
			// Increase the size of the factorization at each iteration
			for (iter.Reset(); !iter.Converged(r, eig, x) && nv < m; 
				iter.MoveNext(), nv = nvn, nvn = Math.Min(nv*inc, m))
			{
				// Allocate more memory for larger factorization
				IVector[] qn = new IVector[nvn];
				double[] alphan = new double[nvn], betan = new double[nvn];
				Expand(q, alpha, beta, qn, alphan, betan);
				q = qn;
				alpha = alphan;
				beta = betan;

				// Compute larger factorization, starting at previous offset
				Lanczos(A, q, alpha, beta, z, nv - 1);

				// Get eigenvalues and the residuals
				evd = new EigenvalueDecomposition(alpha, beta);
				EstimateResidual(nv, beta, evd, r, ind);

				Console.Error.WriteLine(nvn);
			}

			// Get the eigenvalues (only real)
			double[] eigLoc = evd.RealEigenvalues;
			for (int i = 0; i < eig.Length; ++i)
				eig[i] = eigLoc[ind[i]];

			// Compute the eigenvectors (x=q*V)
			IDenseAccessVector[] qa = new IDenseAccessVector[x.Length], 
				xa = new IDenseAccessVector[x.Length];
			for (int i = 0; i < x.Length; ++i)
			{
				qa[i] = (IDenseAccessVector) q[ind[i]];
				xa[i] = (IDenseAccessVector) x[i];
			}
			//Prod(qa, evd.getV(), xa); TODO: clean this
			Prod(qa, evd.EigenVectors, xa);

			// Sort by the eigenvalues
			IntDoubleVectorPair[] s = new IntDoubleVectorPair[eig.Length];
			for (int i = 0; i < s.Length; ++i)
				s[i] = new IntDoubleVectorPair(this, eig[i], i, xa[i]);
			Array.Sort(s);

			// Extract and return
			for (int i = 0; i < s.Length; ++i)
			{
				eig[i] = s[i].d;
				x[i] = s[i].v;
			}
			et.Eigenvalue(eig);
		}

		/// <returns> r = q*v, for constructing eigenvectors.</returns>
		private IVector[] Prod(IDenseAccessVector[] q, double[,] V, IDenseAccessVector[] r)
		{
			int m = q[0].Length, n = q.Length;

			// Simple multiplication. Not the most efficient, but it'll do
			for (int i = 0; i < m; ++i)
				for (int j = 0; j < n; ++j)
				{
					double val = 0.0;
					for (int l = 0; l < n; ++l)
						val += q[l].Vector[i]*V[l, j];
					r[j].Vector[i] = val;
				}

			return r;
		}

		/// <summary> Computes a Lanczos decomposition</summary>
		/// <param name="A">Matrix</param>
		/// <param name="q">q[offset] is the initial vector. The rest are overwritten.</param>
		/// <param name="alpha">Diagonal of the factorization.</param>
		/// <param name="beta">Sub/super diagonals of the factorization.</param>
		/// <param name="z">Work vector.</param>
		/// <param name="offset">For continued factorization.</param>
		private void Lanczos(IMatrix A, IVector[] q, double[] alpha, double[] beta, IVector z, int offset)
		{
			int k = q.Length;

			// Normalize the initial vector
			if (offset == 0)
				q[0] = Normalize(q[0]);

			for (int i = offset; i < k; ++i)
			{
				z = et.Apply(A, q[i], z); // Either z=A*q[i] or z=(A-sI)\q[i]
				alpha[i] = Blas.Default.Dot(q[i], z);

				// Reorthogonalization
				if (fullOrthogonalization)
				{
					z = GramSchmidt(z, q, i - 1);
					z = GramSchmidt(z, q, i - 1);
				}
				else
					z = LanczosOrthogonalization(z, q, alpha, beta, i);

				// Expand factorization somehow
				if (i + 1 < q.Length)
				{
					beta[i] = Blas.Default.Norm(z, NORMS.NORM2);
					if (beta[i] != 0.0)
						q[i + 1] = Blas.Default.ScaleCopy(1.0/beta[i], z, q[i + 1]);
					else
						q[i + 1] = Random(q[i + 1]);
				}
			}
		}

		/// <summary> Applies the Gram-Schmidt procedure to orthogonalize z against the
		/// set of vectors q. Stops at index ql, exclusive.</summary>
		private IVector GramSchmidt(IVector z, IVector[] q, int ql)
		{
			if (ql < 0)
				return z;
			double[] r = new double[ql];
			for (int i = 0; i < ql; ++i)
				r[i] = Blas.Default.Dot(z, q[i]);
			for (int i = 0; i < ql; ++i)
				z = Blas.Default.Add(- r[i], q[i], z);
			return z;
		}

		/// <summary> The usual Lanczos orthogonalization</summary>
		private IVector LanczosOrthogonalization(IVector z, IVector[] q, double[] alpha, double[] beta, int i)
		{
			z = Blas.Default.Add(- alpha[i], q[i], z);
			if (i > 0)
				z = Blas.Default.Add(- beta[i - 1], q[i - 1], z);
			return z;
		}

		/// <summary> Eigenvalue residual estimation</summary>
		private void EstimateResidual(int nv, double[] beta, EigenvalueDecomposition evd, double[] r, int[] ind)
		{
			//double[,] V = evd.getV(); TODO: Clean this
			Matrix V = evd.EigenVectors;

			double[] e = evd.RealEigenvalues;

			// Get all the residuals
			double[] rl = new double[beta.Length];
			for (int i = 0; i < beta.Length; ++i)
				rl[i] = Math.Abs(beta[i]*V[nv - 1, i])/Math.Abs(e[i]);

			// Get the smallest residuals and their indices
			Sort(rl, r, ind);
		}

		/// <summary> Sorts a, buts the smallest values into outD and the indices into outI</summary>
		private void Sort(double[] a, double[] outD, int[] outI)
		{
			// Sort both data and indices
			IntDoublePair[] ra = new IntDoublePair[a.Length];
			for (int i = 0; i < a.Length; ++i)
				ra[i] = new IntDoublePair(this, a[i], i);

			Array.Sort(ra);

			// Extract the smallest
			for (int i = 0; i < outD.Length; ++i)
			{
				outD[i] = ra[i].d;
				outI[i] = ra[i].i;
			}
		}

		/// <summary> Pair of an int and a double, comparisons by the double value</summary>
		private class IntDoublePair : IComparable
		{
			private LanczosSolver solver;

			internal double d;
			internal int i;

			public IntDoublePair(LanczosSolver solver, double d, int i)
			{
				InitBlock(solver);
				this.d = d;
				this.i = i;
			}

			private void InitBlock(LanczosSolver solver)
			{
				this.solver = solver;
			}

			public LanczosSolver Solver
			{
				get { return solver; }

			}			

			public virtual int CompareTo(object o)
			{
				IntDoublePair ob = (IntDoublePair) o;
				if (this.d > ob.d) return 1;
				else if (this.d < ob.d) return - 1;
				else return 0;
			}
		}

		/// <summary> Pairing of an int, a double and a vector, comparisons by the double value</summary>
		private class IntDoubleVectorPair : IntDoublePair
		{
			internal IVector v;

			public IntDoubleVectorPair(LanczosSolver solver, double d, int i, IVector v) 
				: base(solver, d, i)
			{
				this.v = v;
			}
		}

		/// <summary> Copies q, alpha and beta over to qn, alphan and betan.</summary>
		private void Expand(IVector[] q, double[] alpha, double[] beta, 
			IVector[] qn, double[] alphan, double[] betan)
		{
			for (int i = 0; i < q.Length; ++i)
				qn[i] = q[i];
			for (int i = q.Length; i < qn.Length; ++i)
				qn[i] = Factory.createVector(q[0]);
			Array.Copy(alpha, 0, alphan, 0, q.Length);
			Array.Copy(beta, 0, betan, 0, q.Length - 1);
		}
	}
}