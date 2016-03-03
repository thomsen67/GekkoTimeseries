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

using MathNet.Numerics.LinearAlgebra.Sparse.Utilities;

namespace MathNet.Numerics.LinearAlgebra.Sparse
{
	/// <summary> Sparse vector.</summary>
	[Serializable]
	public class SparseVector : AbstractVector, IElementalAccessVector, ISparseAccessVector, IVectorAccess, IVector
	{
		/// <summary> Data</summary>
		private double[] data;

		/// <summary> Indices to data</summary>
		private int[] ind;

		/// <summary> How much has been used</summary>
		private int used;

		/// <summary> Constructor for SparseVector.</summary>
		/// <param name="nz">Initial number of non-zeros.</param>
		public SparseVector(int size, int nz) : base(size)
		{
			data = new double[nz];
			ind = new int[nz];
		}

		/// <summary> Constructor for SparseVector, and copies the contents from the
		/// supplied vector.</summary>
		/// <param name="nz">Initial number of non-zeros.</param>
		public SparseVector(IVector x, int nz) : this(x.Length, nz)
		{
			Blas.Default.Copy(x, this);
		}

		/// <summary> Constructor for SparseVector, and copies the contents from the
		/// supplied vector. Zero initial pre-allocation.</summary>
		public SparseVector(IVector x) : this(x.Length)
		{
			Blas.Default.Copy(x, this);
		}

		/// <summary> Constructor for SparseVector. Zero initial pre-allocation</summary>
		public SparseVector(int size) : this(size, 0)
		{
		}

		public virtual void AddValue(int index, double val)
		{
			int i = GetIndex(index);
			data[i] += val;
		}

		public virtual void SetValue(int index, double val)
		{
			int i = GetIndex(index);
			data[i] = val;
		}

		public virtual double GetValue(int index)
		{
			int in_Renamed = Arrays.binarySearch(ind, index, 0, used);
			if (in_Renamed != - 1)
				return data[in_Renamed];
			else if (index < length_ && index >= 0)
				return 0.0;
			else
				throw new IndexOutOfRangeException("Index " + index);
		}

		public virtual void AddValues(int[] index, double[] values)
		{
			for (int i = 0; i < index.Length; ++i)
				AddValue(index[i], values[i]);
		}

		public virtual void SetValues(int[] index, double[] values)
		{
			for (int i = 0; i < index.Length; ++i)
				SetValue(index[i], values[i]);
		}

		public virtual double[] GetValues(int[] index)
		{
			double[] ret = new double[index.Length];
			for (int i = 0; i < index.Length; ++i)
				ret[i] = GetValue(index[i]);
			return ret;
		}

		/// <summary> Tries to find the index. If it is not found, a reallocation is done, and
		/// a new index is returned.</summary>
		private int GetIndex(int index)
		{
			// Try to find column index
			int i = Arrays.binarySearchGreater(ind, index, 0, used);

			// Found
			if (i < used && ind[i] == index)
				return i;

			// Not found, try to make room
			if (index < 0 || index >= length_)
				throw new IndexOutOfRangeException("Index " + index);
			used++;

			// Check available memory
			if (used > data.Length)
			{
				// If zero-length, use new length of 1, else double the bandwidth
				int newLength = 1;
				if (data.Length != 0)
					newLength = 2*data.Length;

				// Copy existing data into new arrays
				int[] newInd = new int[newLength];
				double[] newDat = new double[newLength];
				Array.Copy(ind, 0, newInd, 0, data.Length);
				Array.Copy(data, 0, newDat, 0, data.Length);

				// Update pointers
				ind = newInd;
				data = newDat;
			}

			// All ok, make room for insertion
			for (int j = used - 1; j >= i + 1; --j)
			{
				ind[j] = ind[j - 1];
				data[j] = data[j - 1];
			}

			// Put in new structure
			ind[i] = index;
			data[i] = 0.0;
			return i;
		}

		public virtual IntDoubleVectorPair Vector
		{
			get
			{
				Compact();
				return new IntDoubleVectorPair(ind, data);
			}
			set
			{
				data = value.data;
				ind = value.indices;
			}
		}

		public virtual double[] Data
		{
			get
			{
				Compact();
				return data;
			}
		}

		public virtual void Compact()
		{
			if (used < ind.Length)
			{
				double[] newData = new double[used];
				Array.Copy(data, 0, newData, 0, used);
				int[] newIndex = new int[used];
				Array.Copy(ind, 0, newIndex, 0, used);
				data = newData;
				ind = newIndex;
			}
		}
	}
}