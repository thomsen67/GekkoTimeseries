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
            if (x.storage.Length != y.storage.Length) return false;

            var comparison = _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            for (int i = 0; i < x.storage.Length; i++)
            {
                if (!string.Equals(x.storage[i], y.storage[i], comparison))
                    return false;
            }
            return true;
        }

        public int GetHashCode(Multidim2Element obj)
        {
            if (obj == null) return 0;

            int hash = 17;
            foreach (var s in obj.storage)
            {
                // If ignoring case, we must hash the lowercase version of the string
                string val = _ignoreCase ? s?.ToLowerInvariant() : s;
                hash = hash * 31 + (val?.GetHashCode() ?? 0);
            }
            return hash;
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
            if (x.storage.Length != y.storage.Length) new Error("#9843298473");                        
            var comparison = _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            for (int i = 0; i < x.storage.Length; i++)
            {
                int result;
                if (_ignoreCase) result = G.CompareNatural(x.storage[i], y.storage[i], CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase);
                else result = G.CompareNatural(x.storage[i], y.storage[i], CultureInfo.InvariantCulture, CompareOptions.Ordinal);
                if (result != 0) return result;
            }
            return 0;
        }
    }




    /*

    [ProtoContract]
    public class Multidim2<T>
    {
        [ProtoMember(1)]
        public Dictionary<Multidim2Element, T> storage = new Dictionary<Multidim2Element, T>();

        [ProtoMember(2)]
        bool caseSensitive = false;

        public Multidim2()
        {
            //only for protobuf use
        }

        /// <summary>
        /// Default is false.
        /// </summary>        
        public Multidim2(bool caseSensitive)
        {
            this.caseSensitive = caseSensitive;
        }

        public bool TryGetValue(Multidim2Element gmi, out T iv)
        {
            return this.storage.TryGetValue(gmi, out iv);
        }

        public void AddWithOverwrite(Multidim2Element mmi, T iv)
        {            
            mmi.caseSensitive = this.caseSensitive;  //inherits it
            if (this.storage.ContainsKey(mmi)) this.storage.Remove(mmi);
            this.storage.Add(mmi, iv);         
        }

        public void Remove(Multidim2Element mmi)
        {
            if (this.storage.ContainsKey(mmi))
            {
                this.storage.Remove(mmi);
            }
            else
            {
                new Error("Could not remove from multidim object");
            }
        }

        /// <summary>
        /// Helper method for the sorting of array-series indexes. For instance, x[b, c] should be shown before x[c, a].
        /// Also uses G.CompareNatural() internally (showing x[a2] before x[a10]).
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int CompareMultidim2Elements(Multidim2Element left, Multidim2Element right)
        {
            if (left.storage.Length != right.storage.Length)
            {
                new Error("#9843298473");
            }
            for (int i = 0; i < left.storage.Length; i++)
            {
                string sleft = left.storage[i];
                string sright = right.storage[i];
                if (left.caseSensitive != right.caseSensitive) new Error("Differing case-sensitivities in multidim element");
                int ii = -12345;
                if (left.caseSensitive) ii = G.CompareNatural(sleft, sright, CultureInfo.InvariantCulture, CompareOptions.None);
                else ii = G.CompareNatural(sleft, sright, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
                if (ii != 0) return ii;
            }
            return 0;
        }
    }

    */

    [ProtoContract]
    public class Multidim2Element
    {
        [ProtoMember(1)]
        public string[] storage = null;
                
        private Multidim2Element()
        {
            //only because protobuf needs it, not for outside use
        }
                
        public Multidim2Element(string[] m)
        {
            this.storage = m;
        }

        public Multidim2Element(List<string> m)
        {
            this.storage = m.ToArray();
        }

        public override string ToString()
        {            
            return Stringlist.GetListWithCommas(storage, "");            
        }

        //public override int GetHashCode()
        //{
        //    int hash = 17;
        //    for (int i = 0; i < storage.Length; i++)
        //    {
        //        //the 17 and 31 is a trick (primes) to get the hashcodes as distinct as possible. For the latter we need ToLower() so that 'aB' and 'Ab' are equal
        //        if (this.caseSensitive) hash = hash * 31 + storage[i].GetHashCode();
        //        else hash = hash * 31 + storage[i].ToLower().GetHashCode();  
        //    }
        //    return hash;
        //}

        //public override bool Equals(object obj)
        //{            
        //    if (obj == null || obj.GetType() != typeof(Multidim2Element)) return false;
        //    Multidim2Element other = (Multidim2Element)obj;
        //    if (this.storage.Length != other.storage.Length) return false;
        //    for (int i = 0; i < this.storage.Length; i++)
        //    {
        //        if (this.caseSensitive) { if (this.storage[i] != other.storage[i]) return false; }
        //        else { if (!G.Equal(this.storage[i], other.storage[i])) return false; }
        //    }
        //    return true;
        //}

        public Multidim2Element Clone()
        {
            string[] ss = new string[this.storage.Length];
            Array.Copy(this.storage, ss, this.storage.Length);
            Multidim2Element mmi = new Multidim2Element(ss);            
            return mmi;
        }
    }
}
