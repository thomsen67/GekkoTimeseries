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
	/// <summary> An index/data pairing with integer indices and double-precision data.
	/// Note that the length of the indices and the data need not be equal.
	/// Furthermore, the indices must be kept sorted in most cases.</summary>
	public class IntDoubleVectorPair
	{
		/// <summary> Integer indices</summary>
		public int[] indices;

		/// <summary> Double-precision data</summary>
		public double[] data;

		/// <summary> Constructor for IntDoubleVectorPair.</summary>
		/// <param name="indices">Indices</param>
		/// <param name="data">Data</param>
		public IntDoubleVectorPair(int[] indices, double[] data)
		{
			this.indices = indices;
			this.data = data;
		}

		/// <summary> Deep copy of the object</summary>
		public virtual IntDoubleVectorPair Clone()
		{
			int[] newIndex = new int[indices.Length];
			double[] newData = new double[data.Length];
			Array.Copy(indices, 0, newIndex, 0, indices.Length);
			Array.Copy(data, 0, newData, 0, data.Length);
			return new IntDoubleVectorPair(newIndex, newData);
		}

		/// <summary> Copy of the data between index[i] and index[i+1]</summary>
		public virtual double[] Copy(int i)
		{
			double[] copy = new double[indices[i + 1] - indices[i]];
			Array.Copy(data, indices[i], copy, 0, copy.Length);
			return copy;
		}
	}
}