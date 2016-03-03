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
using System.IO;
using MathNet.Numerics.LinearAlgebra.Sparse;

namespace MathNet.Numerics.LinearAlgebra.Sparse.Eigenvalue
{
	// TODO: no usage of OutputEigenvalueIterationMonitor, get ride of it.

	/// <summary> Outputs residual information to a stream</summary>
	public class OutputEigenvalueIterationMonitor : IEigenvalueIterationMonitor
	{
		private StreamWriter outStream;

		/// <summary> Sets the output stream to use</summary>
		public virtual Stream OutputStream
		{
			set { this.outStream = new StreamWriter(value); }
		}

		/// <summary> Constructor for OutputEigenvalueIterationMonitor. 
		/// Uses System.err as default output.</summary>
		public OutputEigenvalueIterationMonitor()
		{
			outStream = new StreamWriter(Console.OpenStandardError());
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
			outStream.Write(i + "\t");
			for(int j = 0; j < r.Length; ++j)
				outStream.Write(r[j] + "\t");
			//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaioPrintWriterprintln_3"'
			outStream.WriteLine();
		}
	}
}