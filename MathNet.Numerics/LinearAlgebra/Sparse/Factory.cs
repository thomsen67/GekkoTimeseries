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
using System.Reflection;
using MathNet.Numerics.LinearAlgebra.Sparse;

namespace MathNet.Numerics.LinearAlgebra.Sparse
{
	// TODO: change the vector design to avoid reflection in Factory.

	/// <summary> Convenience factory methods for the iterative methods. 
	/// Creates empty work-vectors.</summary>
	internal class Factory
	{
		/// <summary> No need to create an instance</summary>
		private Factory()
		{
		}

		/// <summary> Creates a new, empty vector based upon the given template</summary>
		public static IVector createVector(IVector template)
		{
			return createVector(template, template.Length);
		}

		/// <summary> Creates a new, empty vector based upon the given template with the
		/// given size.</summary>
		public static IVector createVector(IVector template, int size)
		{
			return createVectors(template, size, 1)[0];
		}

		/// <summary> Creates new, empty vectors based upon the given templates</summary>
		public static IVector[] createVectors(IVector[] template)
		{
			IVector[] ret = new IVector[template.Length];
			for (int i = 0; i < ret.Length; ++i)
				ret[i] = createVector(template[i]);
			return ret;
		}

		/// <summary> Creates an array of new, empty vectors based upon the given template</summary>
		public static IVector[] createVectors(IVector template, int num)
		{
			// Special handling of block-vectors
			if (template is IBlockAccessVector)
			{
				IBlockAccessVector templateB = (IBlockAccessVector) template;
				int numBlocks = templateB.BlockCount;

				ConstructorInfo[] cons = template.GetType().GetConstructors();

				object[] args = new object[2];
				args[0] = template.Length;
				args[1] = numBlocks;

				for (int i = 0; i < cons.Length; ++i)
				{
					ParameterInfo[] pinfos = cons[i].GetParameters();
					Type[] c = new Type[pinfos.Length];
					for (int j = 0; j < pinfos.Length; j++)
						c[j] = pinfos[j].ParameterType;

					// UPGRADE_TODO: invalid java reflection
					if (c.Length == 2 && c[0].FullName.Equals("int") && c[1].FullName.Equals("int"))
					{
						IBlockAccessVector[] ret = new IBlockAccessVector[num];
						for (int j = 0; j < num; ++j)
						{
							ret[j] = (IBlockAccessVector) cons[i].Invoke(args);

							// Copy indices and create empty internal vector
							for (int k = 0; k < numBlocks; ++k)
							{
								int[] index = templateB.GetBlockIndices(k), newIndex = new int[index.Length];
								Array.Copy(index, 0, newIndex, 0, index.Length);
								IVector newVector = createVector(templateB.GetBlock(k));

								ret[j].SetBlock(k, newIndex, newVector);
							}
						}
						return ret;
					}
				}
			}

			// Not a block-vector
			return createVectors(template, template.Length, num);
		}

		/// <summary> Creates an array of new, empty vectors based upon the given template
		/// with the given size of the output vectors.</summary>
		public static IVector[] createVectors(IVector template, int size, int num)
		{
			Type[] types = { typeof(int) };
			ConstructorInfo ci = template.GetType().GetConstructor(
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance,
				null, 
				CallingConventions.HasThis,
				types, null);

			if(ci == null) 
				throw new InvalidOperationException("Vector should implement a ctor(int).");

			object[] args = { size };

			IVector[] vectors = new IVector[num];
			for(int i = 0; i < num; i++)
			{
				vectors[i] = (IVector) ci.Invoke(args);
			}

			return vectors;
		}
	}
}