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
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MathNet.Numerics.LinearAlgebra.Sparse.Utilities
{
	// TODO: use Array.BinarySearch instead

	/// <summary> Miscellaneous array operations.
	/// Complements the search operations found in java.util.Arrays. This class
	/// cannot be instantiated.
	/// </summary>
	internal sealed class Arrays
	{
		/// <summary> No need to create an instance.</summary>
		private Arrays()
		{
		}

		/// <summary> Searches for a key in a sorted array, and returns an index to an
		/// element which is greater than or equal key.
		/// </summary>
		/// <param name="index">Sorted array of integers</param>
		/// <param name="key">Search for something equal or greater</param>
		/// <param name="begin">Start posisiton in the index</param>
		/// <param name="end">One past the end position in the index</param>
		/// <returns> end if nothing greater or equal was found, else an
		/// index satisfying the search criteria</returns>
		public static int binarySearchGreater(int[] index, int key, int begin, int end)
		{
			return binarySearchInterval(index, key, begin, end, true);
		}

		/// <summary> Searches for a key in a sorted array, and returns an index to an
		/// element which is greater than or equal key.
		/// </summary>
		/// <param name="index">Sorted array of integers</param>
		/// <param name="key">Search for something equal or greater</param>
		/// <returns> index.length if nothing greater or equal was found, else an
		/// index satisfying the search criteria</returns>
		public static int binarySearchGreater(int[] index, int key)
		{
			return binarySearchInterval(index, key, 0, index.Length, true);
		}

		/// <summary> Searches for a key in a sorted array, and returns an index to an
		/// element which is smaller than or equal key.
		/// </summary>
		/// <param name="index">Sorted array of integers</param>
		/// <param name="key">Search for something equal or greater
		/// </param>
		/// <param name="begin">Start posisiton in the index
		/// </param>
		/// <param name="end">One past the end position in the index
		/// </param>
		/// <returns> begin-1 if nothing smaller or equal was found, else an index
		/// satisfying the search criteria
		/// </returns>
		public static int binarySearchSmaller(int[] index, int key, int begin, int end)
		{
			return binarySearchInterval(index, key, begin, end, false);
		}

		/// <summary> Searches for a key in a sorted array, and returns an index to an
		/// element which is smaller than or equal key.
		/// </summary>
		/// <param name="index">Sorted array of integers
		/// </param>
		/// <param name="key">Search for something equal or greater
		/// </param>
		/// <returns> -1 if nothing smaller or equal was found, else an index
		/// satisfying the search criteria
		/// </returns>
		public static int binarySearchSmaller(int[] index, int key)
		{
			return binarySearchInterval(index, key, 0, index.Length, false);
		}

		/// <summary> Searches for a key in a subset of a sorted array.</summary>
		/// <param name="index">Sorted array of integers
		/// </param>
		/// <param name="key">Key to search for
		/// </param>
		/// <param name="begin">Start posisiton in the index
		/// </param>
		/// <param name="end">One past the end position in the index
		/// </param>
		/// <returns> Integer index to key. -1 if not found
		/// </returns>
		public static int binarySearch(int[] index, int key, int begin, int end)
		{
			end--;

			while (begin <= end)
			{
				int mid = (end + begin) >> 1;

				if (index[mid] < key)
					begin = mid + 1;
				else if (index[mid] > key)
					end = mid - 1;
				else
					return mid;
			}

			return - 1;
		}

		private static int binarySearchInterval(int[] index, int key, int begin, int end, bool greater)
		{
			// Zero length array?
			if (begin == end)
				if (greater)
					return end;
				else
					return begin - 1;

			end--; // Last index
			int mid = (end + begin) >> 1;

			// The usual binary search
			while (begin <= end)
			{
				mid = (end + begin) >> 1;

				if (index[mid] < key)
					begin = mid + 1;
				else if (index[mid] > key)
					end = mid - 1;
				else
					return mid;
			}

			// No direct match, but an inf/sup was found
			if ((greater && index[mid] >= key) || (!greater && index[mid] <= key))
				return mid;
				// No inf/sup, return at the end of the array
			else if (greater)
				return mid + 1;
				// One past end
			else
				return mid - 1; // One before start
		}
	}

	/// <summary>
	/// Contains conversion support elements such as classes, interfaces and static methods.
	/// </summary>
	internal class SupportClass
	{
		/*******************************/

		/// <summary>
		/// Performs an unsigned bitwise right shift with the specified number
		/// </summary>
		/// <param name="number">Number to operate on</param>
		/// <param name="bits">Ammount of bits to shift</param>
		/// <returns>The resulting number from the shift operation</returns>
		public static int URShift(int number, int bits)
		{
			if (number >= 0)
				return number >> bits;
			else
				return (number >> bits) + (2 << ~bits);
		}

		/// <summary>
		/// Performs an unsigned bitwise right shift with the specified number
		/// </summary>
		/// <param name="number">Number to operate on</param>
		/// <param name="bits">Ammount of bits to shift</param>
		/// <returns>The resulting number from the shift operation</returns>
		public static int URShift(int number, long bits)
		{
			return URShift(number, (int) bits);
		}

		/// <summary>
		/// Performs an unsigned bitwise right shift with the specified number
		/// </summary>
		/// <param name="number">Number to operate on</param>
		/// <param name="bits">Ammount of bits to shift</param>
		/// <returns>The resulting number from the shift operation</returns>
		public static long URShift(long number, int bits)
		{
			if (number >= 0)
				return number >> bits;
			else
				return (number >> bits) + (2L << ~bits);
		}

		/// <summary>
		/// Performs an unsigned bitwise right shift with the specified number
		/// </summary>
		/// <param name="number">Number to operate on</param>
		/// <param name="bits">Ammount of bits to shift</param>
		/// <returns>The resulting number from the shift operation</returns>
		public static long URShift(long number, long bits)
		{
			return URShift(number, (int) bits);
		}

		/*******************************/

		/// <summary>
		/// This method returns the literal value received
		/// </summary>
		/// <param name="literal">The literal to return</param>
		/// <returns>The received value</returns>
		public static long Identity(long literal)
		{
			return literal;
		}

		/// <summary>
		/// This method returns the literal value received
		/// </summary>
		/// <param name="literal">The literal to return</param>
		/// <returns>The received value</returns>
		public static ulong Identity(ulong literal)
		{
			return literal;
		}

		/// <summary>
		/// This method returns the literal value received
		/// </summary>
		/// <param name="literal">The literal to return</param>
		/// <returns>The received value</returns>
		public static float Identity(float literal)
		{
			return literal;
		}

		/// <summary>
		/// This method returns the literal value received
		/// </summary>
		/// <param name="literal">The literal to return</param>
		/// <returns>The received value</returns>
		public static double Identity(double literal)
		{
			return literal;
		}

		/*******************************/

		/// <summary>
		/// SupportClass for the HashSet class.
		/// </summary>
		[Serializable]
			public class HashSetSupport : ArrayList, SetSupport
		{
			public HashSetSupport() : base()
			{
			}

			public HashSetSupport(ICollection c)
			{
				this.AddAll(c);
			}

			public HashSetSupport(int capacity) : base(capacity)
			{
			}

			/// <summary>
			/// Adds a new element to the ArrayList if it is not already present.
			/// </summary>		
			/// <param name="obj">Element to insert to the ArrayList.</param>
			/// <returns>Returns true if the new element was inserted, false otherwise.</returns>
			new public virtual bool Add(object obj)
			{
				bool inserted;

				if ((inserted = this.Contains(obj)) == false)
				{
					base.Add(obj);
				}

				return !inserted;
			}

			/// <summary>
			/// Adds all the elements of the specified collection that are not present to the list.
			/// </summary>
			/// <param name="c">Collection where the new elements will be added</param>
			/// <returns>Returns true if at least one element was added, false otherwise.</returns>
			public bool AddAll(ICollection c)
			{
				IEnumerator e = new ArrayList(c).GetEnumerator();
				bool added = false;

				while (e.MoveNext() == true)
				{
					if (this.Add(e.Current) == true)
						added = true;
				}

				return added;
			}

			/// <summary>
			/// Returns a copy of the HashSet instance.
			/// </summary>		
			/// <returns>Returns a shallow copy of the current HashSet.</returns>
			public override object Clone()
			{
				return base.MemberwiseClone();
			}
		}


		/*******************************/

		/// <summary>
		/// Represents a collection ob objects that contains no duplicate elements.
		/// </summary>	
		public interface SetSupport : ICollection, IList
		{
			/// <summary>
			/// Adds a new element to the Collection if it is not already present.
			/// </summary>
			/// <param name="obj">The object to add to the collection.</param>
			/// <returns>Returns true if the object was added to the collection, otherwise false.</returns>
			new bool Add(object obj);

			/// <summary>
			/// Adds all the elements of the specified collection to the Set.
			/// </summary>
			/// <param name="c">Collection of objects to add.</param>
			/// <returns>true</returns>
			bool AddAll(ICollection c);
		}


		/*******************************/

		/// <summary>
		/// Writes the serializable fields to the SerializationInfo object, which stores all the data needed to serialize the specified object object.
		/// </summary>
		/// <param name="info">SerializationInfo parameter from the GetObjectData method.</param>
		/// <param name="context">StreamingContext parameter from the GetObjectData method.</param>
		/// <param name="instance">object to serialize.</param>
		public static void DefaultWriteObject(SerializationInfo info, StreamingContext context, object instance)
		{
			Type thisType = instance.GetType();
			MemberInfo[] mi = FormatterServices.GetSerializableMembers(thisType, context);
			for (int i = 0; i < mi.Length; i++)
			{
				info.AddValue(mi[i].Name, ((FieldInfo) mi[i]).GetValue(instance));
			}
		}


		/*******************************/

		/// <summary>
		/// Reads the serialized fields written by the DefaultWriteObject method.
		/// </summary>
		/// <param name="info">SerializationInfo parameter from the special deserialization constructor.</param>
		/// <param name="context">StreamingContext parameter from the special deserialization constructor</param>
		/// <param name="instance">object to deserialize.</param>
		public static void DefaultReadObject(SerializationInfo info, StreamingContext context, object instance)
		{
			Type thisType = instance.GetType();
			MemberInfo[] mi = FormatterServices.GetSerializableMembers(thisType, context);
			for (int i = 0; i < mi.Length; i++)
			{
				FieldInfo fi = (FieldInfo) mi[i];
				fi.SetValue(instance, info.GetValue(fi.Name, fi.FieldType));
			}
		}

		/*******************************/

		/// <summary>
		/// This class provides functionality not found in .NET collection-related interfaces.
		/// </summary>
		public class ICollectionSupport
		{
			/// <summary>
			/// Adds a new element to the specified collection.
			/// </summary>
			/// <param name="c">Collection where the new element will be added.</param>
			/// <param name="obj">object to add.</param>
			/// <returns>true</returns>
			public static bool Add(ICollection c, object obj)
			{
				bool added = false;
				//Reflection. Invoke either the "add" or "Add" method.
				MethodInfo method;
				try
				{
					//Get the "add" method for proprietary classes
					method = c.GetType().GetMethod("Add");
					if (method == null)
						method = c.GetType().GetMethod("add");
					int index = (int) method.Invoke(c, new object[] {obj});
					if (index >= 0)
						added = true;
				}
				catch (Exception e)
				{
					throw e;
				}
				return added;
			}

			/// <summary>
			/// Adds all of the elements of the "c" collection to the "target" collection.
			/// </summary>
			/// <param name="target">Collection where the new elements will be added.</param>
			/// <param name="c">Collection whose elements will be added.</param>
			/// <returns>Returns true if at least one element was added, false otherwise.</returns>
			public static bool AddAll(ICollection target, ICollection c)
			{
				IEnumerator e = new ArrayList(c).GetEnumerator();
				bool added = false;

				//Reflection. Invoke "addAll" method for proprietary classes
				MethodInfo method;
				try
				{
					method = target.GetType().GetMethod("addAll");

					if (method != null)
						added = (bool) method.Invoke(target, new object[] {c});
					else
					{
						method = target.GetType().GetMethod("Add");
						while (e.MoveNext() == true)
						{
							bool tempBAdded = (int) method.Invoke(target, new object[] {e.Current}) >= 0;
							added = added ? added : tempBAdded;
						}
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return added;
			}

			/// <summary>
			/// Removes all the elements from the collection.
			/// </summary>
			/// <param name="c">The collection to remove elements.</param>
			public static void Clear(ICollection c)
			{
				//Reflection. Invoke "Clear" method or "clear" method for proprietary classes
				MethodInfo method;
				try
				{
					method = c.GetType().GetMethod("Clear");

					if (method == null)
						method = c.GetType().GetMethod("clear");

					method.Invoke(c, new object[] {});
				}
				catch (Exception e)
				{
					throw e;
				}
			}

			/// <summary>
			/// Determines whether the collection contains the specified element.
			/// </summary>
			/// <param name="c">The collection to check.</param>
			/// <param name="obj">The object to locate in the collection.</param>
			/// <returns>true if the element is in the collection.</returns>
			public static bool Contains(ICollection c, object obj)
			{
				bool contains = false;

				//Reflection. Invoke "contains" method for proprietary classes
				MethodInfo method;
				try
				{
					method = c.GetType().GetMethod("Contains");

					if (method == null)
						method = c.GetType().GetMethod("contains");

					contains = (bool) method.Invoke(c, new object[] {obj});
				}
				catch (Exception e)
				{
					throw e;
				}

				return contains;
			}

			/// <summary>
			/// Determines whether the collection contains all the elements in the specified collection.
			/// </summary>
			/// <param name="target">The collection to check.</param>
			/// <param name="c">Collection whose elements would be checked for containment.</param>
			/// <returns>true id the target collection contains all the elements of the specified collection.</returns>
			public static bool ContainsAll(ICollection target, ICollection c)
			{
				IEnumerator e = c.GetEnumerator();

				bool contains = false;

				//Reflection. Invoke "containsAll" method for proprietary classes or "Contains" method for each element in the collection
				MethodInfo method;
				try
				{
					method = target.GetType().GetMethod("containsAll");

					if (method != null)
						contains = (bool) method.Invoke(target, new object[] {c});
					else
					{
						method = target.GetType().GetMethod("Contains");
						while (e.MoveNext() == true)
						{
							if ((contains = (bool) method.Invoke(target, new object[] {e.Current})) == false)
								break;
						}
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}

				return contains;
			}

			/// <summary>
			/// Removes the specified element from the collection.
			/// </summary>
			/// <param name="c">The collection where the element will be removed.</param>
			/// <param name="obj">The element to remove from the collection.</param>
			public static bool Remove(ICollection c, object obj)
			{
				bool changed = false;

				//Reflection. Invoke "remove" method for proprietary classes or "Remove" method
				MethodInfo method;
				try
				{
					method = c.GetType().GetMethod("remove");

					if (method != null)
						method.Invoke(c, new object[] {obj});
					else
					{
						method = c.GetType().GetMethod("Contains");
						changed = (bool) method.Invoke(c, new object[] {obj});
						method = c.GetType().GetMethod("Remove");
						method.Invoke(c, new object[] {obj});
					}
				}
				catch (Exception e)
				{
					throw e;
				}

				return changed;
			}

			/// <summary>
			/// Removes all the elements from the specified collection that are contained in the target collection.
			/// </summary>
			/// <param name="target">Collection where the elements will be removed.</param>
			/// <param name="c">Elements to remove from the target collection.</param>
			/// <returns>true</returns>
			public static bool RemoveAll(ICollection target, ICollection c)
			{
				ArrayList al = ToArrayList(c);
				IEnumerator e = al.GetEnumerator();

				//Reflection. Invoke "removeAll" method for proprietary classes or "Remove" for each element in the collection
				MethodInfo method;
				try
				{
					method = target.GetType().GetMethod("removeAll");

					if (method != null)
						method.Invoke(target, new object[] {al});
					else
					{
						method = target.GetType().GetMethod("Remove");
						MethodInfo methodContains = target.GetType().GetMethod("Contains");

						while (e.MoveNext() == true)
						{
							while ((bool) methodContains.Invoke(target, new object[] {e.Current}) == true)
								method.Invoke(target, new object[] {e.Current});
						}
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return true;
			}

			/// <summary>
			/// Retains the elements in the target collection that are contained in the specified collection
			/// </summary>
			/// <param name="target">Collection where the elements will be removed.</param>
			/// <param name="c">Elements to be retained in the target collection.</param>
			/// <returns>true</returns>
			public static bool RetainAll(ICollection target, ICollection c)
			{
				IEnumerator e = new ArrayList(target).GetEnumerator();
				ArrayList al = new ArrayList(c);

				//Reflection. Invoke "retainAll" method for proprietary classes or "Remove" for each element in the collection
				MethodInfo method;
				try
				{
					method = c.GetType().GetMethod("retainAll");

					if (method != null)
						method.Invoke(target, new object[] {c});
					else
					{
						method = c.GetType().GetMethod("Remove");

						while (e.MoveNext() == true)
						{
							if (al.Contains(e.Current) == false)
								method.Invoke(target, new object[] {e.Current});
						}
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}

				return true;
			}

			/// <summary>
			/// Returns an array containing all the elements of the collection.
			/// </summary>
			/// <returns>The array containing all the elements of the collection.</returns>
			public static object[] ToArray(ICollection c)
			{
				int index = 0;
				object[] objects = new object[c.Count];
				IEnumerator e = c.GetEnumerator();

				while (e.MoveNext())
					objects[index++] = e.Current;

				return objects;
			}

			/// <summary>
			/// Obtains an array containing all the elements of the collection.
			/// </summary>
			/// <param name="objects">The array into which the elements of the collection will be stored.</param>
			/// <returns>The array containing all the elements of the collection.</returns>
			public static object[] ToArray(ICollection c, object[] objects)
			{
				int index = 0;

				Type type = objects.GetType().GetElementType();
				object[] objs = (object[]) Array.CreateInstance(type, c.Count);

				IEnumerator e = c.GetEnumerator();

				while (e.MoveNext())
					objs[index++] = e.Current;

				//If objects is smaller than c then do not return the new array in the parameter
				if (objects.Length >= c.Count)
					objs.CopyTo(objects, 0);

				return objs;
			}

			/// <summary>
			/// Converts an ICollection instance to an ArrayList instance.
			/// </summary>
			/// <param name="c">The ICollection instance to be converted.</param>
			/// <returns>An ArrayList instance in which its elements are the elements of the ICollection instance.</returns>
			public static ArrayList ToArrayList(ICollection c)
			{
				ArrayList tempArrayList = new ArrayList();
				IEnumerator tempEnumerator = c.GetEnumerator();
				while (tempEnumerator.MoveNext())
					tempArrayList.Add(tempEnumerator.Current);
				return tempArrayList;
			}
		}


		/*******************************/

		public delegate void PropertyChangeEventHandler(object sender, PropertyChangingEventArgs e);

		/// <summary>
		/// EventArgs for support to the contrained properties.
		/// </summary>
		public class PropertyChangingEventArgs : PropertyChangedEventArgs
		{
			private object oldValue;
			private object newValue;

			/// <summary>
			/// Initializes a new PropertyChangingEventArgs instance.
			/// </summary>
			/// <param name="propertyName">Property name that fire the event.</param>
			public PropertyChangingEventArgs(String propertyName) : base(propertyName)
			{
			}

			/// <summary>
			/// Initializes a new PropertyChangingEventArgs instance.
			/// </summary>
			/// <param name="propertyName">Property name that fire the event.</param>
			/// <param name="oldVal">Property value to be replaced.</param>
			/// <param name="newVal">Property value to be set.</param>
			public PropertyChangingEventArgs(String propertyName, object oldVal, object newVal) : base(propertyName)
			{
				this.oldValue = oldVal;
				this.newValue = newVal;
			}

			/// <summary>
			/// Gets or sets the old value of the event.
			/// </summary>
			public object OldValue
			{
				get { return this.oldValue; }
				set { this.oldValue = value; }
			}

			/// <summary>
			/// Gets or sets the new value of the event.
			/// </summary>
			public object NewValue
			{
				get { return this.newValue; }
				set { this.newValue = value; }
			}
		}


		/*******************************/

		/// <summary>
		/// Summary description for EqualsSupport.
		/// </summary>
		public class EqualsSupport
		{
			/// <summary>
			/// Determines whether two Collections instances are equal.
			/// </summary>
			/// <param name="source">The first Collections to compare. </param>
			/// <param name="target">The second Collections to compare. </param>
			/// <returns>Return true if the first collection is the same instance as the second collection, otherwise returns false.</returns>
			public static bool Equals(ICollection source, ICollection target)
			{
				bool equal = true;

				ArrayList sourceInterfaces = new ArrayList(source.GetType().GetInterfaces());
				ArrayList targetInterfaces = new ArrayList(target.GetType().GetInterfaces());

				if (sourceInterfaces.Contains(Type.GetType("SupportClass+SetSupport")) &&
					!targetInterfaces.Contains(Type.GetType("SupportClass+SetSupport")))
					equal = false;
				else if (targetInterfaces.Contains(Type.GetType("SupportClass+SetSupport")) &&
					!sourceInterfaces.Contains(Type.GetType("SupportClass+SetSupport")))
					equal = false;

				if (equal)
				{
					IEnumerator sourceEnumerator = ReverseStack(source);
					IEnumerator targetEnumerator = ReverseStack(target);

					if (source.Count != target.Count)
						equal = false;

					while (sourceEnumerator.MoveNext() && targetEnumerator.MoveNext())
						if (!sourceEnumerator.Current.Equals(targetEnumerator.Current))
							equal = false;
				}

				return equal;
			}

			/// <summary>
			/// Determines if a Collection is equal to the object.
			/// </summary>
			/// <param name="source">The first Collections to compare.</param>
			/// <param name="target">The object to compare.</param>
			/// <returns>Return true if the first collection contains the same values of the second object, otherwise returns false.</returns>
			public static bool Equals(ICollection source, object target)
			{
				return (target is ICollection) ? Equals(source, (ICollection) target) : false;
			}

			/// <summary>
			/// Determines if a IDictionaryEnumerator is equal to the object.
			/// </summary>
			/// <param name="source">The first IDictionaryEnumerator to compare.</param>
			/// <param name="target">The second object to compare.</param>
			/// <returns>Return true if the first IDictionaryEnumerator contains the same values of the second object, otherwise returns false.</returns>
			public static bool Equals(IDictionaryEnumerator source, object target)
			{
				return (target is IDictionaryEnumerator) ? Equals(source, (IDictionaryEnumerator) target) : false;
			}

			/// <summary>
			/// Determines if a IDictionary is equal to the object.
			/// </summary>
			/// <param name="source">The first IDictionary to compare.</param>
			/// <param name="target">The second object to compare.</param>
			/// <returns>Return true if the first IDictionary contains the same values of the second object, otherwise returns false.</returns>
			public static bool Equals(IDictionary source, object target)
			{
				return (target is IDictionary) ? Equals(source, (IDictionary) target) : false;
			}

			/// <summary>
			/// Determines whether two IDictionaryEnumerator instances are equals.
			/// </summary>
			/// <param name="source">The first IDictionaryEnumerator to compare.</param>
			/// <param name="target">The second IDictionaryEnumerator to compare.</param>
			/// <returns>Return true if the first IDictionaryEnumerator contains the same values as the second IDictionaryEnumerator, otherwise return false.</returns>
			public static bool Equals(IDictionaryEnumerator source, IDictionaryEnumerator target)
			{
				while (source.MoveNext() && target.MoveNext())
					if (source.Key.Equals(target.Key))
						if (source.Value.Equals(target.Value))
							return true;
				return false;
			}

			/// <summary>
			/// Reverses the Stack Collection received.
			/// </summary>
			/// <param name="collection">The collection to reverse.</param>
			/// <returns>The collection received in reverse order if it was a System.Collections.Stack type, otherwise it does 
			/// nothing to the collection.</returns>
			public static IEnumerator ReverseStack(ICollection collection)
			{
				if ((collection.GetType()) == (typeof (Stack)))
				{
					ArrayList collectionStack = new ArrayList(collection);
					collectionStack.Reverse();
					return collectionStack.GetEnumerator();
				}
				else
					return collection.GetEnumerator();
			}

			/// <summary>
			/// Determines whether two IDictionary instances are equal.
			/// </summary>
			/// <param name="source">The first Collection to compare.</param>
			/// <param name="target">The second Collection to compare.</param>
			/// <returns>Return true if the first collection is the same instance as the second collection, otherwise return false.</returns>
			public static bool Equals(IDictionary source, IDictionary target)
			{
				Hashtable targetAux = new Hashtable(target);

				if (source.Count == targetAux.Count)
				{
					IEnumerator sourceEnum = source.Keys.GetEnumerator();
					while (sourceEnum.MoveNext())
						if (targetAux.Contains(sourceEnum.Current))
							targetAux.Remove(sourceEnum.Current);
						else
							return false;
				}
				else
					return false;
				if (targetAux.Count == 0)
					return true;
				else
					return false;
			}
		}


		/*******************************/

		/// <summary>
		/// Provides functionality for classes that implements the IList interface.
		/// </summary>
		public class IListSupport
		{
			/// <summary>
			/// Ensures the capacity of the list to be greater or equal than the specified.
			/// </summary>
			/// <param name="list">The list to be checked.</param>
			/// <param name="capacity">The expected capacity.</param>
			public static void EnsureCapacity(ArrayList list, int capacity)
			{
				if (list.Capacity < capacity) list.Capacity = 2*list.Capacity;
				if (list.Capacity < capacity) list.Capacity = capacity;
			}

			/// <summary>
			/// Adds all the elements contained into the specified collection, starting at the specified position.
			/// </summary>
			/// <param name="index">Position at which to add the first element from the specified collection.</param>
			/// <param name="list">The list used to extract the elements that will be added.</param>
			/// <returns>Returns true if all the elements were successfuly added. Otherwise returns false.</returns>
			public static bool AddAll(IList list, int index, ICollection c)
			{
				bool result = false;
				if (c != null)
				{
					IEnumerator tempEnumerator = new ArrayList(c).GetEnumerator();
					int tempIndex = index;

					while (tempEnumerator.MoveNext())
					{
						list.Insert(tempIndex++, tempEnumerator.Current);
						result = true;
					}
				}

				return result;
			}

			/// <summary>
			/// Returns an enumerator of the collection starting at the specified position.
			/// </summary>
			/// <param name="index">The position to set the iterator.</param>
			/// <returns>An IEnumerator at the specified position.</returns>
			public static IEnumerator GetEnumerator(IList list, int index)
			{
				if ((index < 0) || (index > list.Count))
					throw new IndexOutOfRangeException();

				IEnumerator tempEnumerator = list.GetEnumerator();
				if (index > 0)
				{
					int i = 0;
					while ((tempEnumerator.MoveNext()) && (i < index - 1))
						i++;
				}
				return tempEnumerator;
			}
		}


		/*******************************/

		/// <summary>
		/// Provides functionality not found in .NET map-related interfaces.
		/// </summary>
		public class MapSupport
		{
			/// <summary>
			/// Determines whether the SortedList contains a specific value.
			/// </summary>
			/// <param name="d">The dictionary to check for the value.</param>
			/// <param name="obj">The object to locate in the SortedList.</param>
			/// <returns>Returns true if the value is contained in the SortedList, false otherwise.</returns>
			public static bool ContainsValue(IDictionary d, object obj)
			{
				bool contained = false;
				Type type = d.GetType();

				//Classes that implement the SortedList class
				if (type == Type.GetType("System.Collections.SortedList"))
				{
					contained = ((SortedList) d).ContainsValue(obj);
				}
					//Classes that implement the Hashtable class
				else if (type == Type.GetType("System.Collections.Hashtable"))
				{
					contained = ((Hashtable) d).ContainsValue(obj);
				}
				else
				{
					//Reflection. Invoke "containsValue" method for proprietary classes
					try
					{
						MethodInfo method = type.GetMethod("containsValue");
						contained = (bool) method.Invoke(d, new object[] {obj});
					}
					catch (TargetInvocationException e)
					{
						throw e;
					}
					catch (Exception e)
					{
						throw e;
					}
				}

				return contained;
			}


			/// <summary>
			/// Determines whether the NameValueCollection contains a specific value.
			/// </summary>
			/// <param name="d">The dictionary to check for the value.</param>
			/// <param name="obj">The object to locate in the SortedList.</param>
			/// <returns>Returns true if the value is contained in the NameValueCollection, false otherwise.</returns>
			public static bool ContainsValue(NameValueCollection d, object obj)
			{
				bool contained = false;
				//Type type = d.GetType();

				for (int i = 0; i < d.Count && !contained; i++)
				{
					String[] values = d.GetValues(i);
					if (values != null)
					{
						foreach (String val in values)
						{
							if (val.Equals(obj))
							{
								contained = true;
								break;
							}
						}
					}
				}
				return contained;
			}

			/// <summary>
			/// Copies all the elements of d to target.
			/// </summary>
			/// <param name="target">Collection where d elements will be copied.</param>
			/// <param name="d">Elements to copy to the target collection.</param>
			public static void PutAll(IDictionary target, IDictionary d)
			{
				if (d != null)
				{
					ArrayList keys = new ArrayList(d.Keys);
					ArrayList values = new ArrayList(d.Values);

					for (int i = 0; i < keys.Count; i++)
						target[keys[i]] = values[i];
				}
			}

			/// <summary>
			/// Returns a portion of the list whose keys are less than the limit object parameter.
			/// </summary>
			/// <param name="l">The list where the portion will be extracted.</param>
			/// <param name="limit">The end element of the portion to extract.</param>
			/// <returns>The portion of the collection whose elements are less than the limit object parameter.</returns>
			public static SortedList HeadMap(SortedList l, object limit)
			{
				Comparer comparer = Comparer.Default;
				SortedList newList = new SortedList();

				for (int i = 0; i < l.Count; i++)
				{
					if (comparer.Compare(l.GetKey(i), limit) >= 0)
						break;

					newList.Add(l.GetKey(i), l[l.GetKey(i)]);
				}

				return newList;
			}

			/// <summary>
			/// Returns a portion of the list whose keys are greater that the lowerLimit parameter less than the upperLimit parameter.
			/// </summary>
			/// <param name="list">The list where the portion will be extracted.</param>
			/// <param name="lowerLimit">The start element of the portion to extract.</param>
			/// <param name="upperLimit">The end element of the portion to extract.</param>
			/// <returns>The portion of the collection.</returns>
			public static SortedList SubMap(SortedList list, object lowerLimit, object upperLimit)
			{
				Comparer comparer = Comparer.Default;
				SortedList newList = new SortedList();

				if (list != null)
				{
					if ((list.Count > 0) && (!(lowerLimit.Equals(upperLimit))))
					{
						int index = 0;
						while (comparer.Compare(list.GetKey(index), lowerLimit) < 0)
							index++;

						for (; index < list.Count; index++)
						{
							if (comparer.Compare(list.GetKey(index), upperLimit) >= 0)
								break;

							newList.Add(list.GetKey(index), list[list.GetKey(index)]);
						}
					}
				}

				return newList;
			}

			/// <summary>
			/// Returns a portion of the list whose keys are greater than the limit object parameter.
			/// </summary>
			/// <param name="list">The list where the portion will be extracted.</param>
			/// <param name="limit">The start element of the portion to extract.</param>
			/// <returns>The portion of the collection whose elements are greater than the limit object parameter.</returns>
			public static SortedList TailMap(SortedList list, object limit)
			{
				Comparer comparer = Comparer.Default;
				SortedList newList = new SortedList();

				if (list != null)
				{
					if (list.Count > 0)
					{
						int index = 0;
						while (comparer.Compare(list.GetKey(index), limit) < 0)
							index++;

						for (; index < list.Count; index++)
							newList.Add(list.GetKey(index), list[list.GetKey(index)]);
					}
				}

				return newList;
			}
		}


		/*******************************/

		/// <summary>
		/// SupportClass for the SortedSet interface.
		/// </summary>
		public interface SortedSetSupport : SetSupport
		{
			/// <summary>
			/// Returns a portion of the list whose elements are less than the limit object parameter.
			/// </summary>
			/// <param name="limit">The end element of the portion to extract.</param>
			/// <returns>The portion of the collection whose elements are less than the limit object parameter.</returns>
			SortedSetSupport HeadSet(object limit);

			/// <summary>
			/// Returns a portion of the list whose elements are greater that the lowerLimit parameter less than the upperLimit parameter.
			/// </summary>
			/// <param name="upperLimit">The start element of the portion to extract.</param>
			/// <param name="lowerLimit">The end element of the portion to extract.</param>
			/// <returns>The portion of the collection.</returns>
			SortedSetSupport SubSet(object lowerLimit, object upperLimit);

			/// <summary>
			/// Returns a portion of the list whose elements are greater than the limit object parameter.
			/// </summary>
			/// <param name="limit">The start element of the portion to extract.</param>
			/// <returns>The portion of the collection whose elements are greater than the limit object parameter.</returns>
			SortedSetSupport TailSet(object limit);
		}


		/*******************************/

		/// <summary>
		/// This class manages array operations.
		/// </summary>
		public class ArraySupport
		{
			/// <summary>
			/// Compares the entire members of one array whith the other one.
			/// </summary>
			/// <param name="array1">The array to be compared.</param>
			/// <param name="array2">The array to be compared with.</param>
			/// <returns>True if both arrays are equals otherwise it returns false.</returns>
			/// <remarks>Two arrays are equal if they contains the same elements in the same order.</remarks>
			public static bool Equals(Array array1, Array array2)
			{
				bool result = false;
				if ((array1 == null) && (array2 == null))
					result = true;
				else if ((array1 != null) && (array2 != null))
				{
					if (array1.Length == array2.Length)
					{
						int length = array1.Length;
						result = true;
						for (int index = 0; index < length; index++)
						{
							if (!(array1.GetValue(index).Equals(array2.GetValue(index))))
							{
								result = false;
								break;
							}
						}
					}
				}
				return result;
			}

			/// <summary>
			/// Fills the array with an specific value from an specific index to an specific index.
			/// </summary>
			/// <param name="array">The array to be filled.</param>
			/// <param name="fromindex">The first index to be filled.</param>
			/// <param name="toindex">The last index to be filled.</param>
			/// <param name="val">The value to fill the array with.</param>
			public static void Fill(Array array, int fromindex, int toindex, object val)
			{
				for(int i = fromindex; i < toindex; i++)
				{
					array.SetValue(val, i);
				}
			}

			/// <summary>
			/// Fills the array with an specific value.
			/// </summary>
			/// <param name="array">The array to be filled.</param>
			/// <param name="val">The value to fill the array with.</param>
			public static void Fill(Array array, object val)
			{
				Fill(array, 0, array.Length, val);
			}
		}


		/*******************************/

		/// <summary>
		/// Deserializes an object, or an entire graph of connected objects, and returns the object intance
		/// </summary>
		/// <param name="binaryReader">Reader instance used to read the object</param>
		/// <returns>The object instance</returns>
		public static object Deserialize(BinaryReader binaryReader)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			return formatter.Deserialize(binaryReader.BaseStream);
		}

		/*******************************/

		/// <summary>
		/// Writes an object to the specified Stream
		/// </summary>
		/// <param name="stream">The target Stream</param>
		/// <param name="objectToSend">The object to be sent</param>
		public static void Serialize(Stream stream, object objectToSend)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, objectToSend);
		}

		/// <summary>
		/// Writes an object to the specified BinaryWriter
		/// </summary>
		/// <param name="binaryWriter">The target BinaryWriter</param>
		/// <param name="objectToSend">The object to be sent</param>
		public static void Serialize(BinaryWriter binaryWriter, object objectToSend)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(binaryWriter.BaseStream, objectToSend);
		}

		/*******************************/
		//Provides access to a static System.Random class instance
		public static Random Random = new Random();

	}
}