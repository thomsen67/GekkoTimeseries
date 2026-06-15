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
            string first = null;            
            if (this.storage.Length > 0) first = string.Join(",", this.storage);
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
            if (x.GetHashCode(_ignoreCase) != y.GetHashCode(_ignoreCase)) return false; //actually redundant for dictionaries, but we keep it for now
            if (x.storage.Length != y.storage.Length) return false;            
            for (int i = 0; i < x.storage.Length; i++)
            {
                var elX = x.storage[i];
                var elY = y.storage[i];

                if (elX.isTime != elY.isTime) return false;
                if (elX.isTime)
                {
                    if (elX.timeValue.CompareTo(elY.timeValue) != 0) return false;
                }
                else
                {
                    int result;
                    if (_ignoreCase)
                    {
                        if (!string.Equals(elX.stringValue, elY.stringValue, StringComparison.OrdinalIgnoreCase)) return false;
                    }
                    else
                    {
                        if (!string.Equals(elX.stringValue, elY.stringValue, StringComparison.Ordinal)) return false;
                    }
                }                
            }
            return true;
        }

        public int GetHashCode(Multidim2Element obj)
        {
            if (obj == null) return 0;            
            return obj.GetHashCode(_ignoreCase); //pick the right one
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
            if (x.storage.Length != y.storage.Length) return x.storage.Length.CompareTo(y.storage.Length);
            
            for (int i = 0; i < x.storage.Length; i++)
            {
                var xi = x.storage[i];
                var yi = y.storage[i];
                if (xi.isTime != yi.isTime) return xi.isTime ? -1 : 1;
                if (xi.isTime)
                {                    
                    int compare = xi.timeValue.CompareTo(yi.timeValue);
                    if (compare != 0) return compare;
                }
                else
                {
                    int compare;
                    if (_ignoreCase) compare = G.CompareNatural(xi.stringValue, yi.stringValue, CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase);
                    else compare = G.CompareNatural(xi.stringValue, yi.stringValue, CultureInfo.InvariantCulture, CompareOptions.Ordinal);
                    if (compare != 0) return compare;
                }
            }
            return 0;
        }
    }

    [ProtoContract]
    public class Multidim2Element
    {
        [ProtoMember(1)]
        public readonly StringOrTime[] storage;

        [ProtoMember(2)]
        private readonly int _sensitiveHash;

        [ProtoMember(3)]
        private readonly int _insensitiveHash;

        private Multidim2Element()
        {
            //only because protobuf needs it, not for outside use
        }

        public Multidim2Element(StringOrTime[] elements)
        {
            storage = elements;

            // Calculate hash once at birth
            int sHash = 17;
            int iHash = 17;

            for (int i = 0; i < storage.Length; i++)
            {
                var si = storage[i];
                if (si.isTime)
                {
                    int tHash = si.timeValue.GetHashCode();
                    sHash = sHash * 31 + tHash;
                    iHash = iHash * 31 + tHash;
                }
                else if (si.stringValue != null)
                {                    
                    sHash = sHash * 31 + StringComparer.Ordinal.GetHashCode(si.stringValue);
                    iHash = iHash * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(si.stringValue);
                }
            }
            _sensitiveHash = sHash;
            _insensitiveHash = iHash;
        }
        
        public override bool Equals(object obj) => throw new InvalidOperationException("Use Multidim2Comparer explicitly");
        
        public override int GetHashCode() => throw new InvalidOperationException("Use Multidim2Comparer explicitly");
                
        public int GetHashCode(bool ignoreCase) => ignoreCase ? _insensitiveHash : _sensitiveHash;        

        public override string ToString()
        {
            List<string> temp = new List<string>();
            foreach (StringOrTime s in storage)
            {
                temp.Add(s.ToString());
            }
            return Stringlist.GetListWithCommas(temp, "");
        }        
    }

    public struct StringOrTime
    {
        public readonly bool isTime = false;
        public readonly GekkoTime timeValue;
        public readonly string stringValue;
                
        public StringOrTime(GekkoTime value)
        {
            this.isTime = true;
            timeValue = value;
            stringValue = null;            
        }
                
        public StringOrTime(string value)
        {
            timeValue = GekkoTime.tNull;
            stringValue = value;
        }

        public static implicit operator StringOrTime(string s) => new StringOrTime(s);
        public static implicit operator StringOrTime(GekkoTime t) => new StringOrTime(t);
    }

    /// <summary>
    /// This is faster sorting for use in data hash
    /// </summary>
    public sealed class StringArrayOrdinalIgnoreCaseComparer : IComparer<string[]>
    {
        public static readonly StringArrayOrdinalIgnoreCaseComparer Instance = new();

        public int Compare(string[]? x, string[]? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;

            int len = Math.Min(x.Length, y.Length);

            for (int i = 0; i < len; i++)
            {
                int cmp = CompareOrdinalIgnoreCase(x[i], y[i]);
                if (cmp != 0)
                    return cmp;
            }

            return x.Length.CompareTo(y.Length);
        }

        private static int CompareOrdinalIgnoreCase(string? a, string? b)
        {
            if (ReferenceEquals(a, b)) return 0;
            if (a is null) return -1;
            if (b is null) return 1;

            int len = Math.Min(a.Length, b.Length);

            for (int i = 0; i < len; i++)
            {
                char ca = a[i];
                char cb = b[i];

                // fast ASCII case folding (avoids ToUpper/ToLower allocations)
                if ((uint)(ca - 'a') <= 25) ca = (char)(ca - 32);
                if ((uint)(cb - 'a') <= 25) cb = (char)(cb - 32);

                if (ca != cb)
                    return ca.CompareTo(cb);
            }

            return a.Length.CompareTo(b.Length);
        }
    }
}
