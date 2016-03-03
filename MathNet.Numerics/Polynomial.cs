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
	/// <summary>
	/// Polynomial represents a finite order polynomial
	/// with positive powers and constant real coefficients.
	/// </summary>
	public class Polynomial : IComparable, ICloneable
	{
		private double[] coefficients;

		#region Constructors
		/// <summary>Create a new polynomial by order</summary>
		/// <param name="order">The highest power. Example: 2*x^3+x-3 has order 3.</param>
		public Polynomial(int order)
		{
			coefficients = new double[order+1];
		}

		/// <summary>Create a new polynomial by coefficients</summary>
		/// <param name="coefficients">The coefficients vector. The coefficient index denotes the related power (c[0]*z^0+c[1]*z^1+..)</param>
		public Polynomial(double[] coefficients)
		{
			this.coefficients = new double[coefficients.Length];
			SetPolynomial(coefficients);
		}

		/// <summary>Create a new polynomial by copy</summary>
		/// <param name="copy">A polynomial to copy from.</param>
		public Polynomial(Polynomial copy)
		{
			coefficients = new double[copy.coefficients.Length];
			SetPolynomial(copy.coefficients);
		}
		#endregion

		#region .NET Integration: Hashing, Equality, Ordering, Cloning
		public override int GetHashCode()
		{
			return coefficients.GetHashCode ();
		}
		public override bool Equals(Object obj)
		{
			if (obj == null || !(obj is Polynomial))
				return false;
			return Equals((Polynomial)obj);
		}
		public bool Equals(Polynomial polynomial)
		{
			return CompareTo(polynomial) == 0;
		}
		public static bool Equals(Polynomial polynomial1, Polynomial polynomial2)
		{
			return polynomial1.Equals(polynomial2);
		}
		public int CompareTo(object obj)
		{
			if(obj == null)
				return 1;
			if(!(obj is Polynomial))
				throw new ArgumentException("Type mismatch: polynomial expected.","obj");
			return CompareTo((Polynomial)obj);
		}
		public int CompareTo(Polynomial polynomial)
		{
			int i = this.coefficients.Length - 1;
			int j = polynomial.coefficients.Length - 1;

			while(i != j)
			{
				if(i>j)
				{
					if(this.coefficients[i--] != 0)
						return 1;
				}
				else
				{
					if(polynomial.coefficients[j--] != 0)
						return -1;
				}
			}

			while(i >= 0)
			{
				if(this.coefficients[i] > polynomial.coefficients[i])
					return 1;
				if(this.coefficients[i] < polynomial.coefficients[i])
					return -1;
				i--;
			}

			return 0;
		}
		object ICloneable.Clone()
		{
			return Clone();
		}
		public Polynomial Clone()
		{
			return new Polynomial(this);
		}
		#endregion

		#region String Formatting and Parsing
		public string ToString(string baseVariable)
		{
			StringBuilder builder = new StringBuilder();
			for(int i=coefficients.Length-1; i>=0; i--)
			{
				double coeff = coefficients[i];
				if(coeff == 0d)
					continue;
				if(builder.Length > 0)
					builder.Append(coeff > 0d ? " + " : " - ");
				else
					builder.Append(coeff < 0d ? "-" : "");
				if(coeff != 1d && coeff != -1d || i==0)
					builder.Append(Math.Abs(coeff));
				builder.Append(i > 0 ? " " + baseVariable : "");
				if(i > 1)
				{
					builder.Append("^");
					builder.Append(i);
				}
			}
			if(builder.Length == 0)
				builder.Append("0");
			return builder.ToString();
		}
		public override string ToString()
		{
			return ToString("x");
		}
		#endregion

		#region Accessors
		private void SetPolynomial(double[] newCoefficients)
		{
			if(newCoefficients.Length != coefficients.Length)
				coefficients = new double[newCoefficients.Length];
			for(int i=0;i<newCoefficients.Length;i++)
				coefficients[i] = newCoefficients[i];
		}
		public void Normalize()
		{
			for(int i=Order;i>=0;i--)
			{
				if(this[i] != 0d)
				{
					Order = i;
					return;
				}
			}
			Order = 0;
		}
		public int Order
		{
			get {return coefficients.Length-1;}
			set
			{
				if(value == coefficients.Length-1)
					return;
				double[] newCoeff = new double[value+1];
				for(int i=0;i<Math.Min(newCoeff.Length,coefficients.Length);i++)
					newCoeff[i] = coefficients[i];
			}
		}
		public double this[int power]
		{
			get
			{
				if(power < 0)
					throw new ArgumentOutOfRangeException("power",power,"Power must not be negative.");
				if(power >= coefficients.Length)
					return 0d;
				else
					return coefficients[power];
			}
			set
			{
				if(power < 0)
					throw new ArgumentOutOfRangeException("power",power,"Power must not be negative.");
				if(power >= coefficients.Length)
					this.Order = power;
				coefficients[power] = value;
			}
		}
		#endregion

		#region Operators
		public static bool operator== (Polynomial polynomial1, Polynomial polynomial2)
		{
			return polynomial1.Equals(polynomial2);
		}
		public static bool operator!= (Polynomial polynomial1, Polynomial polynomial2)
		{
			return !polynomial1.Equals(polynomial2);
		}
		public static bool operator> (Polynomial polynomial1, Polynomial polynomial2)
		{
			return polynomial1.CompareTo(polynomial2) == 1;
		}
		public static bool operator< (Polynomial polynomial1, Polynomial polynomial2)
		{
			return polynomial1.CompareTo(polynomial2) == -1;
		}
		public static bool operator>= (Polynomial polynomial1, Polynomial polynomial2)
		{
			int res = polynomial1.CompareTo(polynomial2);
			return res == 1 || res == 0;
		}
		public static bool operator<= (Polynomial polynomial1, Polynomial polynomial2)
		{
			int res = polynomial1.CompareTo(polynomial2);
			return res == -1 || res == 0;
		}

		public static Polynomial operator+ (Polynomial polynomial1, Polynomial polynomial2)
		{
			Polynomial ret = new Polynomial(polynomial1);
			ret.AddInplace(polynomial2);
			return ret;
		}
		public static Polynomial operator+ (Polynomial polynomial, double n)
		{
			Polynomial ret = new Polynomial(polynomial);
			ret.AddInplace(n);
			return ret;
		}
		public static Polynomial operator+ (double n, Polynomial polynomial)
		{
			Polynomial ret = new Polynomial(polynomial);
			ret.AddInplace(n);
			return ret;
		}
		public static Polynomial operator+ (Polynomial polynomial)
		{
			return polynomial;
		}

		public static Polynomial operator- (Polynomial polynomial1, Polynomial polynomial2)
		{
			Polynomial ret = new Polynomial(polynomial1);
			ret.SubtractInplace(polynomial2);
			return ret;
		}
		public static Polynomial operator- (Polynomial polynomial, double n)
		{
			Polynomial ret = new Polynomial(polynomial);
			ret.SubtractInplace(n);
			return ret;
		}
		public static Polynomial operator- (double n, Polynomial polynomial)
		{
			Polynomial ret = new Polynomial(polynomial);
			ret.NegateInplace();
			ret.AddInplace(n);
			return ret;
		}
		public static Polynomial operator- (Polynomial polynomial)
		{
			Polynomial ret = new Polynomial(polynomial);
			ret.NegateInplace();
			return ret;
		}

		public static Polynomial operator* (Polynomial polynomial1, Polynomial polynomial2)
		{
			return polynomial1.Multiply(polynomial2);
		}
		public static Polynomial operator* (Polynomial polynomial, double n)
		{
			Polynomial ret = new Polynomial(polynomial);
			ret.MultiplyInplace(n);
			return ret;
		}
		public static Polynomial operator* (double n, Polynomial polynomial)
		{
			Polynomial ret = new Polynomial(polynomial);
			ret.MultiplyInplace(n);
			return ret;
		}

		/// <exception cref="System.DivideByZeroException" />
		public static Polynomial operator/ (Polynomial polynomial, double n)
		{
			Polynomial ret = new Polynomial(polynomial);
			ret.DivideInplace(n);
			return ret;
		}
		#endregion

		#region Inplace Arithmetic Methods
		public void AddInplace(Polynomial polynomial)
		{
			if(polynomial.Order > Order)
				Order = polynomial.Order;
            for(int i=0; i<Math.Min(coefficients.Length,polynomial.coefficients.Length); i++)
				coefficients[i] += polynomial.coefficients[i];
		}
		public void AddInplace(double n)
		{
			this[0] += n;
		}

		public void SubtractInplace(Polynomial polynomial)
		{
			if(polynomial.Order > Order)
				Order = polynomial.Order;
			for(int i=0; i<Math.Min(coefficients.Length,polynomial.coefficients.Length); i++)
				coefficients[i] -= polynomial.coefficients[i];
		}
		public void SubtractInplace(double n)
		{
			this[0] -= n;
		}
		public void NegateInplace()
		{
			for(int i=0;i<coefficients.Length;i++)
                coefficients[i] = -coefficients[i];
		}

		public Polynomial Multiply(Polynomial polynomial)
		{
			double[] coeff = new double[1 + Order + polynomial.Order];
			for(int i=0;i<Order;i++)
				for(int j=0;j<polynomial.Order;j++)
					coeff[i+j] += coefficients[i]*polynomial.coefficients[j];
			return new Polynomial(coeff);
		}
		public void MultiplyInplace(double n)
		{
			for(int i=0;i<coefficients.Length;i++)
				coefficients[i] = n*coefficients[i];
		}

        public Rational Divide(Polynomial polynomial)
		{
			return new Rational(Clone(),polynomial.Clone());
		}
		public void DivideInplace(double n)
		{
			if(n == 0d)
				throw new DivideByZeroException();
			for(int i=0;i<coefficients.Length;i++)
				coefficients[i] = coefficients[i]/n;
		}
		#endregion

		#region Evaluation
		public double Evaluate(double value)
		{
			double pow = 1;
			double ret = coefficients[0];
			for(int i=0;i<coefficients.Length;i++)
			{
				pow *= value;
				ret += pow*coefficients[i];
			}
			return ret;
		}
		#endregion
	}
}
