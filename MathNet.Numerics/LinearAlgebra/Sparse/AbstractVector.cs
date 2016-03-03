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
	/// <summary> Partial implementation of Vector.</summary>
	[Serializable]
	public abstract class AbstractVector : IVector
	{
		/// <summary> Size of the vector</summary>
		protected internal int length_;

		/// <summary> Constructor for AbstractVector.</summary>
		public AbstractVector(int length)
		{
			if (length < 0)
				throw new IndexOutOfRangeException("Vector length cannot be negative");
			this.length_ = length;
		}

		/// <summary> Constructor for AbstractVector, same size as x</summary>
		public AbstractVector(IVector x)
		{
			this.length_ = x.Length;
		}

		public virtual int Length
		{
			get { return length_; }
		}
	}
}