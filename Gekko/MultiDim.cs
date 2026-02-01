using System;
using System.Collections.Generic;
using ProtoBuf;
using System.Globalization;

namespace Gekko
{

    public enum EMultiDimType
    {
        None,
        NameAndIndexFreq,
        NameAndIndexNoFreq
    }

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
            if (x.GetHashCode(_ignoreCase) != y.GetHashCode(_ignoreCase)) return false; //actually redundant for dictionaries, but we keep it for now
            if (x.storage.Length != y.storage.Length) return false;
            for (int i = 0; i < x.storage.Length; i++)
            {
                var elX = x.storage[i];
                var elY = y.storage[i];

                if (elX.IsTime() != elY.IsTime()) return false;
                if (elX.IsTime())
                {
                    if (elX.GetTime().CompareTo(elY.GetTime()) != 0) return false;
                }
                else
                {
                    int result;
                    if (_ignoreCase)
                    {
                        if (!string.Equals(elX.GetString(), elY.GetString(), StringComparison.OrdinalIgnoreCase)) return false;
                    }
                    else
                    {
                        if (!string.Equals(elX.GetString(), elY.GetString(), StringComparison.Ordinal)) return false;
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
                if (xi.IsTime() != yi.IsTime()) return xi.IsTime() ? -1 : 1;
                if (xi.IsTime())
                {
                    int compare = xi.GetTime().CompareTo(yi.GetTime());
                    if (compare != 0) return compare;
                }
                else
                {
                    int compare;
                    if (_ignoreCase) compare = G.CompareNatural(xi.GetString(), yi.GetString(), CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase);
                    else compare = G.CompareNatural(xi.GetString(), yi.GetString(), CultureInfo.InvariantCulture, CompareOptions.Ordinal);
                    if (compare != 0) return compare;
                }
            }
            return 0;
        }
    }

    [ProtoContract]
    [ProtoInclude(101, typeof(DName))] //101 to not collide with other member numbers
    public class Multidim2Element
    {
        [ProtoMember(1)]
        public readonly StringOrTime[] storage = null;

        [ProtoMember(2)]
        public readonly int timePosition = -1; //-1 --> no time, if >= 0 it tells which dimension is time.

        [ProtoMember(3)]
        public readonly EMultiDimType type = EMultiDimType.None;

        [ProtoMember(4)]
        private readonly int sensitiveHash;

        [ProtoMember(5)]
        private readonly int insensitiveHash;

        bool useFreq = false;

        public Multidim2Element()
        {
            //Empty object, kind of null
        }

        public Multidim2Element(StringOrTime[] elements) : this(elements, EMultiDimType.None)
        {
            //new Error("Forbidden at the moment");
        }

        public Multidim2Element(StringOrTime[] elements, EMultiDimType type)
        {
            this.storage = elements;
            this.type = type;

            // Calculate hash once at birth
            int sHash = 17;
            int iHash = 17;

            for (int i = 0; i < storage.Length; i++)
            {
                var si = storage[i];
                if (si.IsTime())
                {
                    if (this.timePosition != -1) new Error("Only 1 time element allowed");
                    this.timePosition = i;
                    int tHash = si.GetTime().GetHashCode();
                    sHash = sHash * 31 + tHash;
                    iHash = iHash * 31 + tHash;
                }
                else if (si.GetString() != null)
                {
                    sHash = sHash * 31 + StringComparer.Ordinal.GetHashCode(si.GetString());
                    iHash = iHash * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(si.GetString());
                }
            }
            sensitiveHash = sHash;
            insensitiveHash = iHash;
        }

        public bool HasContents()
        {
            if (this.storage == null || this.storage.Length == 0) return false;
            return true;
        }

        public override bool Equals(object obj) => throw new InvalidOperationException("Use Multidim2Comparer explicitly");

        public override int GetHashCode() => throw new InvalidOperationException("Use Multidim2Comparer explicitly");

        public int GetHashCode(bool ignoreCase) => ignoreCase ? insensitiveHash : sensitiveHash;

        public override string ToString()
        {
            if (this.storage.Length == 0) new Error("hov");
            string name = this.storage[0].GetString();
            if (this.useFreq) name += "!a";
            List<string> temp = new List<string>();
            for (int i = 1; i < this.storage.Length; i++)
            {
                temp.Add(this.storage[i].ToString());
            }
            return name + "[" + Stringlist.GetListWithCommas(temp, "") + "]";
        }
    }

    [ProtoContract]
    public struct StringOrTime
    {
        [ProtoMember(1)]
        private readonly bool isTime = false;

        [ProtoMember(2)]
        private readonly GekkoTime timeValue;

        [ProtoMember(3)]
        private readonly string stringValue;

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

        public bool IsTime()
        {
            return this.isTime;
        }

        public string GetString()
        {
            if (this.isTime) new Error("Hov");
            return this.stringValue;
        }

        public GekkoTime GetTime()
        {
            if (!this.isTime) new Error("Hov");
            return this.timeValue;
        }

        public static implicit operator StringOrTime(string s) => new StringOrTime(s);
        public static implicit operator StringOrTime(GekkoTime t) => new StringOrTime(t);
    }

    /// <summary>
    /// Has no frequency. May or may not have time
    /// </summary>
    [ProtoContract]    
    public class DName : Multidim2Element 
    {
        private readonly int posName = 0;
        //private readonly int posFreq = 1;
        private readonly int posIndex = 1;
        
        public DName() : base() { } // Protobuf only

        public DName(string name, StringOrTime[] indexes) : base(Construct(name, indexes)) { }
                
        public string GetName() => this.storage[this.posName].GetString();

        //public string GetFreq() => this.storage[this.posFreq].GetString();

        public GekkoTime GetTime() => this.storage[this.timePosition].GetTime();
                
        private static StringOrTime[] Construct(string name, StringOrTime[] indexes)
        {
            int offset = 1;
            var result = new StringOrTime[indexes.Length + offset];
            result[0] = name;
            //result[1] = freq;            
            Array.Copy(indexes, 0, result, offset, indexes.Length);
            return result;
        }

        /// <summary>
        /// Hacky, try to get rid of it when scalar model dicts are done
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DName HACK1(string s)
        {
            string bank; string name; string freq; string[] indexes;
            G.Chop_Chop(s, out bank, out name, out freq, out indexes);
            List<StringOrTime> m = new List<StringOrTime>();
            //string s3 = name;
            //if (freq != null) s3 += "!" + freq;
            //m.Add(s3);
            if (indexes != null)
            {
                foreach (string s2 in indexes)
                {
                    if (G.LooksLikeYear(s2))
                    {
                        m.Add(GekkoTime.FromStringToGekkoTime(s2));
                    }
                    else
                    {
                        m.Add(s2);
                    }
                }
            }
            return new DName(name, m.ToArray());
        }

        public bool HACKHASINDEX()
        {
            if (this.storage.Length - 1 >= this.posIndex) return true;
            return false;            
        }

        public string HACKGETNAME()
        {            
            return this.storage[0].GetString();
            return null;
        }
    }
}
