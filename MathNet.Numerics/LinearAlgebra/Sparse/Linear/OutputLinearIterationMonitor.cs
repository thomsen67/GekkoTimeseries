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
using System.Text;

using MathNet.Numerics.LinearAlgebra.Sparse;

namespace MathNet.Numerics.LinearAlgebra.Sparse.Linear
{
	/// <summary> Outputs iteration information to an output stream.</summary>
	public class OutputLinearIterationMonitor : ILinearIterationMonitor
	{
		/// <summary> Platform-dependent output</summary>
		private StreamWriter stream;

		/// <summary> Constructor for OutputLinearIterationMonitor</summary>
		/// <param name="stream">Writes iteration count and current residual here</param>
		public OutputLinearIterationMonitor(Stream stream)
		{
			StreamWriter temp_writer;
			temp_writer = new StreamWriter(stream, Encoding.Default);
			temp_writer.AutoFlush = true;
			this.stream = temp_writer;
		}

		/// <summary> Constructor for OutputLinearIterationMonitor, using System.err.</summary>
		public OutputLinearIterationMonitor() : this(Console.OpenStandardError())
		{
		}

		public virtual void Monitor(double r, int i)
		{
			//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaioPrintWriterprintln_javalangString_3"'
			stream.WriteLine(i + " " + r);
		}

		public virtual void Monitor(double r, IVector x, int i)
		{
			Monitor(r, i);
		}
	}
}