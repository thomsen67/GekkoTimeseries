using System;
using System.Collections.Generic;
using ProtoBuf;
using System.Globalization;

namespace Gekko
{

    [ProtoContract]
    public class Multidim
    {
        [ProtoMember(1)]
        public Dictionary<MultidimElement, IVariable> storage = new Dictionary<MultidimElement, IVariable>();

        public Multidim()
        {
            //only for protobuf use
        }

        public bool TryGetValue(MultidimElement gmi, out IVariable iv)
        {
            return this.storage.TryGetValue(gmi, out iv);
        }

        public void AddIVariableWithOverwrite(MultidimElement mmi, IVariable iv)
        {
            if (iv.Type() == EVariableType.Series && ((Series)iv).type == ESeriesType.ArraySuper || ((Series)iv).type == ESeriesType.Light)
            {
                throw new GekkoException(); //Sanity check, best to keep it here for the time being!
            }
            if (this.storage.ContainsKey(mmi)) this.storage.Remove(mmi);
            this.storage.Add(mmi, iv);
            Series ts = iv as Series;  //always so
            if (ts != null)
            {
                ts.mmi = mmi;  //so that the sub-series points to the mmi object, which in turn points to the array-series
                ts.name = Globals.seriesArraySubName + Globals.freqIndicator + G.ConvertFreq(ts.freq); //We have to overwrite it here, else it could be a name like x!a if a normal timeseries is copied into an array series
            }
            if (mmi.parent != null) mmi.parent.SetDirty(true);  //Gekko 4.0: mmi.parent probably never null
        }

        public void RemoveIVariable(MultidimElement mmi)
        {
            if (this.storage.ContainsKey(mmi))
            {
                this.storage.Remove(mmi);
            }
            else
            {
                new Error("Could not remove variable");
            }
            if (mmi.parent != null) mmi.parent.SetDirty(true);  //Gekko 4.0: mmi.parent probably never null
        }

        /// <summary>
        /// Helper method for the sorting of array-series indexes. For instance, x[b, c] should be shown before x[c, a].
        /// Also uses G.CompareNatural() internally (showing x[a2] before x[a10]).
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int CompareMultidimElements(MultidimElement left, MultidimElement right)
        {
            if (left.storage.Length != right.storage.Length)
            {
                new Error("#9843298473");
            }
            for (int i = 0; i < left.storage.Length; i++)
            {
                string sleft = left.storage[i];
                string sright = right.storage[i];
                int ii = G.CompareNatural(sleft, sright, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
                if (ii != 0) return ii;
            }
            return 0;
        }
    }

    [ProtoContract]
    public class MultidimElement
    {
        [ProtoMember(1)]
        public string[] storage = null;

        public Series parent = null;  //do not store in protobuf

        private MultidimElement()
        {
            //only because protobuf needs it, not for outside use
        }

        //Only used for lookup purposes, is going to be discarded afterwards
        public MultidimElement(string[] s)
        {
            this.storage = s;
        }

        //Used for permanent storage, so the mmi must point to its parent
        public MultidimElement(string[] s, Series parent)
        {
            this.storage = s;
            this.parent = parent;
        }

        public override string ToString()
        {
            //TODO Gekko 4.0: use Stringlist.GetListWithCommas()
            string first = null;
            foreach (string s in this.storage)
            {
                first += s + ",";
            }
            if (this.storage.Length > 0) first = first.Substring(0, first.Length - ",".Length);
            return first;
        }

        public string GetName()
        {
            string s = null;
            if (this.parent != null) s = this.parent.name;
            return s + "[" + this.ToString() + "]";
        }

        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < storage.Length; i++)
            {
                hash = hash * 31 + storage[i].ToLower().GetHashCode();  //the 17 and 31 is a trick (primes) to get the hashcodes as distinct as possible. We need ToLower() so that 'aB' and 'Ab' are equal
            }
            return hash;
        }

        public override bool Equals(object obj)
        {
            //This will run fastest if the strings are interned (cf. string.Intern). But it seems they are so when getting deflated
            //from protobuf file anyway.
            //Hmmm maybe not so important, since the strings will have mixed cases. Maybe in principle we should store all of them as
            //lower-case..... ??

            if (obj == null || obj.GetType() != typeof(MultidimElement)) return false;
            MultidimElement other = (MultidimElement)obj;
            if (this.storage.Length != other.storage.Length) return false;
            for (int i = 0; i < this.storage.Length; i++)
            {
                if (!G.Equal(this.storage[i], other.storage[i])) return false;
            }
            return true;
        }

        public MultidimElement Clone()
        {
            string[] ss = new string[this.storage.Length];
            Array.Copy(this.storage, ss, this.storage.Length);
            MultidimElement mmi = new MultidimElement(ss, this.parent);
            return mmi;
        }
    }

    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================
    // ================================================================================

    public class Multidim2Comparer : IEqualityComparer<Multidim2Element>
    {
        private readonly bool _ignoreCase;

        public Multidim2Comparer(bool ignoreCase)
        {
            _ignoreCase = ignoreCase;
        }

        public bool Equals(Multidim2Element x, Multidim2Element y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            if (x.GetHashCode() != y.GetHashCode()) return false;
            if (x._elements.Length != y._elements.Length) return false;            
            for (int i = 0; i < x._elements.Length; i++)
            {
                if (x._elements[i].isInt != y._elements[i].isInt) return false;
                if (x._elements[i].isInt)
                {
                    if (x._elements[i].IntValue != y._elements[i].IntValue) return false;
                }
                else
                {
                    int result;
                    if (_ignoreCase)
                    {
                        if (!string.Equals(x._elements[i].StringValue, y._elements[i].StringValue, StringComparison.OrdinalIgnoreCase)) return false;
                    }
                    else
                    {
                        if (!string.Equals(x._elements[i].StringValue, y._elements[i].StringValue, StringComparison.Ordinal)) return false;
                    }
                }                
            }
            return true;
        }

        public int GetHashCode(Multidim2Element obj)
        {
            return obj.GetHashCode();
        }
    }

    public class MultidimSortComparer : IComparer<Multidim2Element>
    {
        private readonly bool _ignoreCase;

        public MultidimSortComparer(bool ignoreCase)
        {
            _ignoreCase = ignoreCase;
        }

        public int Compare(Multidim2Element x, Multidim2Element y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            if (x._elements.Length != y._elements.Length) return -1;
            var comparison = _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            for (int i = 0; i < x._elements.Length; i++)
            {
                if (x._elements[i].isInt != y._elements[i].isInt) return -1;
                if (x._elements[i].isInt)
                {
                    if (x._elements[i].IntValue < y._elements[i].IntValue) return -1;
                }
                else
                {
                    int result;
                    if (_ignoreCase) result = G.CompareNatural(x._elements[i].StringValue, y._elements[i].StringValue, CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase);
                    else result = G.CompareNatural(x._elements[i].StringValue, y._elements[i].StringValue, CultureInfo.InvariantCulture, CompareOptions.Ordinal);
                    if (result != 0) return result;
                }
            }
            return 0;
        }
    }

    [ProtoContract]
    public class Multidim2Element
    {
        [ProtoMember(1)]
        public readonly KeyElement[] _elements;

        [ProtoMember(2)]
        private readonly int _cachedHash;

        private Multidim2Element()
        {
            //only because protobuf needs it, not for outside use
        }

        public Multidim2Element(KeyElement[] elements)
        {
            _elements = elements;
            // Calculate hash once at birth
            int hash = 17;
            foreach (var el in _elements)
            {
                if (el.isInt) hash = hash * 31 + el.IntValue;
                else
                {
                    if (el.StringValue != null) hash = hash * 31 + StringComparer.Ordinal.GetHashCode(el.StringValue.ToLower());  //most use will be case-insensitive comparisons
                }
            }
            _cachedHash = hash;
        }

        public override int GetHashCode() => _cachedHash;

        public override string ToString()
        {
            List<string> temp = new List<string>();
            foreach (KeyElement s in _elements)
            {
                temp.Add(s.ToString());
            }
            return Stringlist.GetListWithCommas(temp, "");
        }

        public Multidim2Element Clone()
        {
            string[] ss = new string[this._elements.Length];
            Array.Copy(this._elements, ss, this._elements.Length);
            Multidim2Element mmi = new Multidim2Element(_elements); //will compute hash again            
            return mmi;
        }


    }

    public struct KeyElement
    {
        public readonly bool isInt = false;
        public readonly int IntValue;
        public readonly string StringValue;

        // Constructor for Int
        public KeyElement(int value)
        {
            IntValue = value;
            StringValue = null;
            this.isInt = true;
        }

        // Constructor for String
        public KeyElement(string value)
        {
            IntValue = 0;
            StringValue = value;
        }

    }

    public sealed class MultidimKey
    {
        private readonly KeyElement[] _elements;
        private readonly int _cachedHash;

        public MultidimKey(KeyElement[] elements)
        {
            _elements = elements;

            // Calculate hash once at birth
            int hash = 17;
            foreach (var el in _elements)
            {
                hash = hash * 31 + el.IntValue;
                if (el.StringValue != null)
                    hash = hash * 31 + StringComparer.Ordinal.GetHashCode(el.StringValue);
            }
            _cachedHash = hash;
        }

        public override int GetHashCode() => _cachedHash;

        public override bool Equals(object obj) => Equals(obj as MultidimKey);

        public bool Equals(MultidimKey other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other == null || _cachedHash != other._cachedHash) return false;
            if (_elements.Length != other._elements.Length) return false;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].Equals(other._elements[i])) return false;
            }
            return true;
        }
    }
}
