#region MathNet Numerics, Copyright ©2004 Christoph Ruegg 

// MathNet Numerics, part of MathNet
//
// Copyright (c) 2004,	Christoph Ruegg, http://www.cdrnet.net,
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
using System.Text;

namespace MathNet.Numerics
{
	public struct Quaternion : IComparable, ICloneable
	{
		private readonly double qx, qy, qz;
		private readonly double qw;
		private readonly double qn;

        #region Constructors
		public Quaternion(double imagX, double imagY, double imagZ, double real)
		{
			this.qx = imagX;
			this.qy = imagY;
			this.qz = imagZ;
			this.qw = real;
			double un = qx*qx+qy*qy+qz*qz;
			this.qn = un+qw*qw;
		}
		internal Quaternion(double imagX, double imagY, double imagZ, double real, double norm)
		{
			this.qx = imagX;
			this.qy = imagY;
			this.qz = imagZ;
			this.qw = real;
			this.qn = norm;
		}
		#endregion

		#region .NET Integration: Hashing, Equality, Ordering, Cloning

		public int CompareTo(object obj)
		{
			// TODO:  Implementierung von Quaternion.CompareTo hinzufügen
			return 0;
		}

		public object Clone()
		{
			// TODO:  Implementierung von Quaternion.Clone hinzufügen
			return null;
		}

		#endregion

		#region String Formatting and Parsing
		#endregion

		#region Accessors
		/// <summary>Gets the real part of the quaternion.</summary>
		public double Real
		{
			get {return qw;}
			//set {qw = value;}
		}
		/// <summary>Gets the imaginary X part (coefficient of complex I) of the quaternion.</summary>
		public double ImagX
		{
			get {return qx;}
			//set {qx = value;}
		}
		/// <summary>Gets the imaginary Y part (coefficient of complex J) of the quaternion.</summary>
		public double ImagY
		{
			get {return qy;}
			//set {qy = value;}
		}
		/// <summary>Gets the imaginary Z part (coefficient of complex K) of the quaternion.</summary>
		public double ImagZ
		{
			get {return qz;}
			//set {qz = value;}
		}
		/// <summary>Gets the norm n(q) = ||q||^2 of the quaternion q.</summary>
		public double Norm
		{
			get {return qn;}
		}
		#endregion

		#region Operators
		#endregion

		#region Outplace Arithmetic Methods
		public Quaternion Conjugate()
		{
			return new Quaternion(-qx,-qy,-qz,qw,qn);
		}
		public Quaternion Inverse()
		{
			if(qn == 1)
				return new Quaternion(-qx,-qy,-qz,qw);
			else
				return new Quaternion(-qx/qn,-qy/qn,-qz/qn,qw/qn);
		}

		public Quaternion Add(Quaternion q)
		{
			return new Quaternion(qx+q.qx,qy+q.qy,qz+q.qz,qw+q.qw);
		}
		public Quaternion Add(double r)
		{
			return new Quaternion(qx,qy,qz,qw+r);
		}
		public Quaternion Subtract(Quaternion q)
		{
			return new Quaternion(qx-q.qx,qy-q.qy,qz-q.qz,qw-q.qw);
		}
		public Quaternion Subtract(double r)
		{
			return new Quaternion(qx,qy,qz,qw-r);
		}

		public Quaternion Multiply(Quaternion q)
		{
			double ci = +qx*q.qw +qy*q.qz -qz*q.qy +qw*q.qx;
			double cj = -qx*q.qz +qy*q.qw +qz*q.qx +qw*q.qy;
			double ck = +qx*q.qy -qy*q.qx +qz*q.qw +qw*q.qz;
			double cr = -qx*q.qx -qy*q.qy -qz*q.qz +qw*q.qw;
			return new Quaternion(ci,cj,ck,cr,qn*q.qn);
		}
		public Quaternion Multiply(double d)
		{
			return new Quaternion(d*qx,d*qy,d*qz,d*qw);
		}
		#endregion

		// TODO: testing suite for Quaternion
	}
}
