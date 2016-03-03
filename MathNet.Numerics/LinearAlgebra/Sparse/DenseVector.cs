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

namespace MathNet.Numerics.LinearAlgebra.Sparse
{
	/// <summary> Dense vector.</summary>
	[Serializable]
	public class DenseVector : AbstractVector, 
		IDenseAccessVector, IElementalAccessVector, IVectorAccess, IVector
	{
		/// <summary> Internal representation</summary>
		private double[] data;

		/// <summary> Constructor for DenseVector.</summary>
		public DenseVector(int size) : base(size)
		{
			data = new double[size];
		}

		/// <summary> Constructor for DenseVector, and copies the contents from the
		/// supplied vector.</summary>
		public DenseVector(IVector x) : this(x.Length)
		{
			Blas.Default.Copy(x, this);
		}

		public virtual double[] Vector
		{
			get { return data; }
			set { data = value; }
		}

		public virtual void AddValue(int index, double val)
		{
			data[index] += val;
		}

		public virtual void SetValue(int index, double val)
		{
			data[index] = val;
		}

		public virtual double GetValue(int index)
		{
			return data[index];
		}

		public virtual void AddValues(int[] index, double[] values)
		{
			for (int i = 0; i < index.Length; ++i)
					data[index[i]] += values[i];
		}

		public virtual void SetValues(int[] index, double[] values)
		{
			for (int i = 0; i < index.Length; ++i)
					data[index[i]] = values[i];
		}

		public virtual double[] GetValues(int[] index)
		{
			double[] ret = new double[index.Length];
			for (int i = 0; i < index.Length; ++i)
				ret[i] = data[index[i]];
			return ret;
		}

		public virtual double[] Data
		{
			get { return data; }
		}
	}
}