using System;
using System.Collections.Generic;
using ProtoBuf;
using System.Globalization;
using System.Linq;

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
        public static readonly Multidim2Comparer IgnoreCase = new Multidim2Comparer(true); //For faster reuse
        public static readonly Multidim2Comparer MatchCase = new Multidim2Comparer(false); //For faster reuse

        private readonly bool _ignoreCase;

        private Multidim2Comparer(bool ignoreCase)
        {
            _ignoreCase = ignoreCase;
        }

        public bool Equals(Multidim2Element x, Multidim2Element y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            if (x.GetHashCode(_ignoreCase) != y.GetHashCode(_ignoreCase)) return false; //actually redundant for dictionaries, but we keep it for now
            if (x.GetLength() != y.GetLength()) return false;
            for (int i = 0; i < x.GetLength(); i++)
            {
                var elX = x.Get(i);
                var elY = y.Get(i);

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
            //if (x.GetLength() != y.GetLength()) return x.GetLength().CompareTo(y.GetLength());

            int xLen = x.GetLength();
            int yLen = y.GetLength();
            int maxLength = Math.Max(xLen, yLen);

            for (int i = 0; i < maxLength; i++)
            {
                // 2. If we are past the end of x, but y still has values
                if (i >= xLen) return -1; // x is shorter

                // 3. If we are past the end of y, but x still has values
                if (i >= yLen) return 1;  // y is shorter

                var xi = x.Get(i);
                var yi = y.Get(i);
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
        protected readonly StringOrTime[] storage = null; //The whole object is considered null if .storage is == null                       

        [ProtoMember(2)]
        private readonly int sensitiveHash;

        [ProtoMember(3)]
        private readonly int insensitiveHash;        

        public Multidim2Element()
        {
            //Empty object, kind of null
        }

        public int GetLength()
        {
            return this.storage.Length;
        }

        public StringOrTime Get(int i)
        {
            return this.storage[i];
        }

        public Multidim2Element(StringOrTime[] elements)
        {
            this.storage = elements;

            //We now calculate hash once at birth, and .timePosition is also found (if any)
            int sHash = 17;
            int iHash = 17;

            for (int i = 0; i < storage.Length; i++)
            {
                var si = storage[i];
                if (si.IsTime())
                {                    
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

        public bool IsNull() //Same as .ToString() == null
        {            
            if (this.storage == null) return true;
            return false;
        }

        private StringOrTime[] GetStorage()
        {
            return this.storage;
        }

        public override bool Equals(object obj) => throw new InvalidOperationException("Use Multidim2Comparer explicitly");

        public override int GetHashCode() => throw new InvalidOperationException("Use Multidim2Comparer explicitly");

        public int GetHashCode(bool ignoreCase) => ignoreCase ? insensitiveHash : sensitiveHash;

        public override string ToString()
        {
            if (this.storage == null) return null;
            List<string> temp = new List<string>();
            for (int i = 0; i < this.storage.Length; i++)
            {
                temp.Add(this.storage[i].ToString());
            }
            return Stringlist.GetListWithCommas(temp, " ");
        }

        public StringOrTime[] DeepClone()
        {
            return (StringOrTime[])this.storage.Clone(); //Uses C# .Clone()
        }
    }

    /// <summary>
    /// Has no frequency. May or may not have time
    /// </summary>
    [ProtoContract]
    public class DName : Multidim2Element
    {
        private readonly int posName = 0; //hardcoded
        private readonly int posIndex = 1; //hardcoded
        [ProtoMember(1)]
        public readonly int timePosition = -1; //-1 --> no time, if >= 0 it tells which dimension is time (pos >= 1)

        public DName() : base() { } // Protobuf only

        public DName(string name, StringOrTime[] indexes) : base(Construct(name, indexes)) 
        {
            for(int i = 0;i<this.GetLength();i++)
            {
                if (this.Get(i).IsTime())
                {
                    GekkoTime t = this.Get(i).GetTime();
                    if (t.freq == EFreq.None || t.freq == EFreq.Age)
                    {
                        //These are not considered "time" (neither are .Empty{i} enum slots if any)
                    }
                    else
                    {
                        if (this.timePosition != -1) new Error("Only 1 time element allowed for DName");
                        this.timePosition = i;                        
                    }
                }
            }
        }

        public string GetName()
        {
            return this.Get(this.posName).GetString();
        }

        private StringOrTime[] DeepCloneExceptFirst()
        {
            //Should be ok fast
            StringOrTime[] result = new StringOrTime[this.storage.Length - 1];
            Array.Copy(this.storage, 1, result, 0, result.Length);
            return result;
        }

        private StringOrTime[] DeepCloneExceptFirstAndTime()
        {
            //Should be ok fast
            if (this.timePosition == -1)
            {
                return this.DeepCloneExceptFirst();
            }
            else
            {
                //StringOrTime[] result = new StringOrTime[this.storage.Length - 2];
                List<StringOrTime> result = new List<StringOrTime>();
                for (int i = 1; i < this.storage.Length; i++)
                {
                    StringOrTime element = this.storage[i];
                    if (element.IsTime()) continue;
                    result.Add(element);                    
                }
                return result.ToArray();
            }            
        }

        public DName RemoveTime()
        {
            return new DName(this.GetName(), this.DeepCloneExceptFirstAndTime());
        }

        public DName ConvertToLag(GekkoTime t)
        {
            if (this.timePosition == -1) new Error("DName: cannot find time dimension to convert into lag/lead");
            GekkoTime thisT = this.GetTime();
            int lag = thisT.Subtract(t); //will fail if freq mismatch. Note: -2 means lagged 2 periods.
            StringOrTime[] elements = this.DeepCloneExceptFirst();
            elements[this.timePosition - 1] = new GekkoTime(EFreq.Lag, lag);  //Note: -1 because elements has first element removed
            DName name = new DName(this.Get(0).GetString(), elements);
            return name;
        }

        public string HACK_ToStringWithoutTime()
        {
            List<string> temp = this.HACK_IndexesWithoutTime();            
            if (temp.Count == 0) return this.GetName();
            else return this.GetName() + "[" + Stringlist.GetListWithCommas(temp, "") + "]";            
        }

        public List<string> HACK_IndexesWithoutTime()
        {
            List<string> temp = new List<string>();
            for (int i = this.posIndex; i < this.GetLength(); i++)
            {
                if (i == this.timePosition) continue;
                temp.Add(this.Get(i).ToString());
            }
            return temp;            
        }

        //Removes last index if s==null. If s != null last index is removed if it is == s.
        public DName HACK_NameWithoutLast(string s)
        {
            List<StringOrTime> temp = new List<StringOrTime>();            
            for (int i = this.posIndex; i < this.GetLength() - 1; i++)
            {
                temp.Add(this.Get(i));
            }
            if (s != null)
            {
                StringOrTime xx = this.Get(this.GetLength() - 1);
                if (xx.IsString() && G.Equal(s, xx.GetString()))
                {
                    //do not add it
                }
                else
                {
                    temp.Add(xx);
                }
            }
            return new DName(this.GetName(), temp.ToArray());
        }

        public DName HACK_AddIndex(StringOrTime element)
        {
            List<StringOrTime> temp = new List<StringOrTime>();
            for (int i = this.posIndex; i < this.GetLength(); i++)
            {
                temp.Add(this.Get(i));
            }
            temp.Add(element);
            return new DName(this.GetName(), temp.ToArray());
        }

        /// <summary>
        /// May return GekkoTime.tNull if no time present.
        /// </summary>
        /// <returns></returns>
        public GekkoTime GetTime()
        {
            if (this.timePosition == -1) return GekkoTime.tNull;            
            return this.Get(this.timePosition).GetTime();            
        }

        private static StringOrTime[] Construct(string name, StringOrTime[] indexes)
        {
            int offset = 1;
            var result = new StringOrTime[indexes.Length + offset];
            result[0] = name;
            Array.Copy(indexes, 0, result, offset, indexes.Length);
            return result;
        }

        public override string ToString()
        {
            if (this.IsNull()) return null;
            string name = this.Get(this.posName).GetString();
            List<string> temp = new List<string>();
            for (int i = this.posIndex; i < this.GetLength(); i++)
            {
                string s = this.Get(i).ToString();
                if (s != null) temp.Add(s);
            }
            if (temp.Count == 0) return name;
            else return name + "[" + Stringlist.GetListWithCommas(temp, "") + "]";
        }

        /// <summary>
        /// Hacky, try to get rid of it when scalar model dicts are done
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DName HACK1(string s)
        {
            if (s.Contains("¤"))
            {
                string[] ss = s.Split('¤');
                string s0 = ss[0].Trim();
                string bank; string name; string freq; string[] indexes;
                G.Chop_Chop(s0, out bank, out name, out freq, out indexes);

                string s1 = ss[1].Trim();
                if (!(s1.StartsWith("[") && s1.EndsWith("]"))) new Error("DName problem1");
                string s1a = s1.Substring(1, s1.Length - 2);
                if (!G.LooksLikeYearOrQuarterOrMonth(s1a))
                {
                    new Error("DName problem2");
                }
                GekkoTime gt = GekkoTime.FromStringToGekkoTime(s1a, false, true, false);
                List<StringOrTime> m = new List<StringOrTime>();
                if (indexes != null)
                {
                    foreach (string s2 in indexes)
                    {
                        if (G.LooksLikeYearOrQuarterOrMonth(s2)) new Error("DName problem3");
                        m.Add(s2);
                    }
                }
                //m.Add(new GekkoTime(EFreq.A, int.Parse(s1a), 1));
                m.Add(gt);
                return new DName(name, m.ToArray());
            }
            else
            {
                string bank; string name; string freq; string[] indexes;
                G.Chop_Chop(s, out bank, out name, out freq, out indexes);
                List<StringOrTime> m = new List<StringOrTime>();
                if (indexes != null)
                {
                    foreach (string s2 in indexes)
                    {
                        if (G.LooksLikeYearOrQuarterOrMonth(s2))
                        {
                            m.Add(GekkoTime.FromStringToGekkoTime(s2, false, true, false));
                        }
                        else
                        {
                            m.Add(s2);
                        }
                    }
                }
                return new DName(name.Replace("¤", ""), m.ToArray());
            }            
        }

        public bool HACKHASINDEX()
        {
            if (this.GetLength() - 1 >= this.posIndex) return true;
            return false;            
        }

        public string HACKGETNAME()
        {            
            return this.Get(0).GetString();
            return null;
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

        public bool IsString()
        {
            return !this.isTime;
        }

        public string GetString()
        {
            if (this.isTime) new Error("Error_GetString");
            return this.stringValue;
        }

        public GekkoTime GetTime()
        {
            if (!this.isTime) new Error("Error_GetTime");
            return this.timeValue;
        }

        public override string ToString()
        {
            if (this.isTime) return this.GetTime().ToString();
            else return this.GetString();
        }

        public static implicit operator StringOrTime(string s) => new StringOrTime(s);
        public static implicit operator StringOrTime(GekkoTime t) => new StringOrTime(t);
    }

}
