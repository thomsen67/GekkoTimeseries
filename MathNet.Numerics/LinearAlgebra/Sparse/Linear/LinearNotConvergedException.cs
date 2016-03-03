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

namespace MathNet.Numerics.LinearAlgebra.Sparse.Linear
{
	/// <summary> Exception for lack of convergence in a linear problem.
	/// Contains the final computed residual.
	/// </summary>
	[Serializable]
	public class LinearNotConvergedException : NotConvergedException
	{
		/// <summary> Returns final computed residual</summary>
		public virtual double Residual
		{
			get { return r; }

		}

		/// <summary> Final residual</summary>
		private double r;

		/// <summary> Constructor for LinearNotConvergedException.</summary>
		public LinearNotConvergedException(int reason, int iterations, double r) : base(reason, iterations)
		{
			this.r = r;
		}

		/// <summary> Constructor for LinearNotConvergedException.</summary>
		public LinearNotConvergedException(int reason, int iterations, String message, double r) : base(reason, iterations, message)
		{
			this.r = r;
		}
	}
}