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
	/// <summary> Signals the lack of convergence of an iterative solver.
	/// It could be due to too stringent convergence criteria, too few iterations
	/// used, or simply due to a malformed problem. The number of iterations are
	/// stored, along with the reason (BREAKDOWN, DIVERGENCE, or something else).
	/// </summary>
	[Serializable]
	public class NotConvergedException : Exception
	{
		/// <summary> Gets the reason for this exception</summary>
		/// <returns> Either BREAKDOWN, DIVERGENCE or ITERATIONS.</returns>
		public virtual int Reason
		{
			get { return reason; }

		}

		public override String Message
		{
			get { return message; }

		}

		/// <summary> Gets the number of iterations used when this exception was thrown</summary>
		public virtual int Iterations
		{
			get { return iterations; }

		}

		/// <summary> Required too many iterations to converge</summary>
		public const int ITERATIONS_ = 1;

		/// <summary> Residual increased to divergence tolerance</summary>
		public const int DIVERGENCE = 2;

		/// <summary> Breakdown detected in iteration</summary>
		public const int BREAKDOWN = 3;

		/// <summary> Reason for the exception</summary>
		private int reason;

		/// <summary> Iteration count when this exception was thrown</summary>
		private int iterations;

		/// <summary> Message describing the exception.</summary>
		private String message;

		/// <summary> Constructor for NotConvergedException.</summary>
		/// <param name="reason">Reason for exception.</param>
		/// <param name="iterations">Iteration count when this exception was thrown.</param>
		public NotConvergedException(int reason, int iterations) : this(reason, iterations, "")
		{
		}

		/// <summary> Constructor for NotConvergedException.</summary>
		/// <param name="reason">Reason for exception.</param>
		/// <param name="iterations">Iteration count when this exception was thrown.</param>
		/// <param name="message">Message describing the exception.</param>
		public NotConvergedException(int reason, int iterations, String message) : base()
		{
			this.reason = reason;
			this.iterations = iterations;
			this.message = message;
		}
	}
}