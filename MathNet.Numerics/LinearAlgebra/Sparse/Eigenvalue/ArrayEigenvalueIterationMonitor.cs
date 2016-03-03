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

using System.Collections;

using MathNet.Numerics.LinearAlgebra.Sparse;
using MathNet.Numerics.LinearAlgebra.Sparse.Utilities;

namespace MathNet.Numerics.LinearAlgebra.Sparse.Eigenvalue
{
	/// <summary> Registers iteration information into an array</summary>
	public class ArrayEigenvalueIterationMonitor : IEigenvalueIterationMonitor
	{
		/// <summary> 
		/// Retrives all the residuals in an array. Array index is the iteration number.
		/// </summary>
		/// <returns> Double array of residuals.</returns>
		public virtual double[][] Array
		{
			get
			{
				object[] arr = SupportClass.ICollectionSupport.ToArray(array);
				double[][] ret = new double[arr.Length][];
				for (int i = 0; i < arr.Length; ++i)
					ret[i] = (double[]) arr[i];

				return ret;
			}

		}

		/// <summary> Contains all the residuals</summary>
		private IList array;

		/// <summary> Constructor for ArrayEigenvalueIterationMonitor.</summary>
		public ArrayEigenvalueIterationMonitor()
		{
			array = ArrayList.Synchronized(new ArrayList(10));
		}

		public virtual void Monitor(double[] r, double[] eig, IVector[] x, int i)
		{
			Monitor(r, i);
		}

		public virtual void Monitor(double[] r, double[] eig, int i)
		{
			Monitor(r, i);
		}

		public virtual void Monitor(double[] r, int i)
		{
			array.Insert(i, r);
		}
	}
}