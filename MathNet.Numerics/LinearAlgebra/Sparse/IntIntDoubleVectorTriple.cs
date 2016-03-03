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
	/// <summary> Dual indices and data paired together. Used by some of the faster 
	/// matrices. major contains the index for each data-entry, while minor is either 
	/// uses as row- or column-delimiters. Within two minor-indices, the major-indices
	/// must be sorted for most applications.</summary>
	public class IntIntDoubleVectorTriple
	{
		/// <summary> Major indices, as long as data</summary>
		public int[] major;

		/// <summary> Minor indices, shorter than data</summary>
		public int[] minor;

		/// <summary> Double-precision data</summary>
		public double[] data;

		/// <summary> Constructor for IntIntDoubleVectorTriple.</summary>
		public IntIntDoubleVectorTriple(int[] major, int[] minor, double[] data)
		{
			this.major = major;
			this.minor = minor;
			this.data = data;
		}

		/// <summary> Deep copy of the object</summary>
		public virtual IntIntDoubleVectorTriple Clone()
		{
			int[] newMajor = new int[major.Length], newMinor = new int[minor.Length];
			double[] newData = new double[data.Length];
			Array.Copy(major, 0, newMajor, 0, major.Length);
			Array.Copy(minor, 0, newMinor, 0, minor.Length);
			Array.Copy(data, 0, newData, 0, data.Length);
			return new IntIntDoubleVectorTriple(newMajor, newMinor, newData);
		}

		/// <summary> Copy of the index/data pair between minor[i] and minor[i+1]</summary>
		public virtual IntDoubleVectorPair copy(int i)
		{
			int length = minor[i + 1] - minor[i];
			int[] index = new int[length];
			double[] newData = new double[length];
			Array.Copy(major, minor[i], index, 0, length);
			Array.Copy(data, minor[i], newData, 0, length);
			return new IntDoubleVectorPair(index, newData);
		}
	}
}