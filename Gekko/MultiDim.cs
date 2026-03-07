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

        /// <summary>
        /// For internal use: a DName can just be null, which is more logical. So no need to define a DName x = DName(). Used because of protobuf.
        /// </summary>
        /// <returns></returns>
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

    public enum EDNameQuotes
    {
        Normal,
        Quotes
    }

    public enum EDNameTime
    {
        None,
        Last,
        LastExceptLag0
    }

    public class DNameFormat
    {
        public EDNameQuotes format = EDNameQuotes.Normal;
        public EDNameTime separateTime = EDNameTime.None;
        public string separator = null;
        public bool showFreq = true;
        
        /// <summary>
        /// For protobuf: do not use this.
        /// </summary>
        public DNameFormat() 
        { 
        }
        
        public DNameFormat(EDNameQuotes format, EDNameTime separateTime, string separator, bool showFreq)
        {
            this.format = format;
            this.separateTime = separateTime;
            this.separator = separator;
            this.showFreq = showFreq;
        }
    }

    /// <summary>
    /// May or may not have frequency. May or may not have time.
    /// </summary>
    [ProtoContract]
    public class DName : Multidim2Element
    {
        private static readonly int _posName = 0; //hardcoded
        private static readonly int _posFreq = 1; //hardcoded
        private static readonly int _posIndex = 2; //hardcoded
        [ProtoMember(1)]
        public readonly int timePosition = -1; //-1 --> no time, if >= 0 it tells which dimension is time (pos >= _posIndex)        

        public DName() : base() { } // Protobuf only
                                     
        public DName(string name) : this(name, Array.Empty<StringOrTime>())
        {            
        }

        public DName(string name, EFreq freq) : this(name, freq, Array.Empty<StringOrTime>())
        {            
        }

        public DName(string name, StringOrTime[] indexes) : base(Construct(name, indexes))
        {
            for (int i = 0; i < this.GetLength(); i++)
            {
                if (this.Get(i).IsTime())
                {
                    GekkoTime t = this.Get(i).GetTime();
                    if (!(t.freq == EFreq.None || t.freq == EFreq.Age))
                    {                    
                        if (this.HasTime()) new Error("Only 1 time element allowed for DName");
                        this.timePosition = i;
                    }
                }
            }
        }

        public DName(string name, EFreq freq, StringOrTime[] indexes) : base(Construct(name, freq, indexes))
        {
            if (freq == EFreq.Lag) new Error("Lag not allowed as variable freq");
            for (int i = 0; i < this.GetLength(); i++)
            {
                if (this.Get(i).IsTime())
                {
                    GekkoTime t = this.Get(i).GetTime();
                    EFreq tFreq = t.freq;
                    if (!(tFreq == EFreq.None || tFreq == EFreq.Age))
                    {                    
                        if (this.HasTime()) new Error("Only 1 time element allowed for DName");                        
                        if (freq != EFreq.None && tFreq != freq) new Error("Variable freq " + tFreq.ToString() + " does not match period freq " + freq.ToString());
                        this.timePosition = i;
                    }
                }
            }
        }

        public static DNameFormat dNameFormatDefault = new DNameFormat();

        public string GetName()
        {
            return this.Get(DName._posName).GetString();
        }

        //Small time penalty, but we can live with it (dict lookup)
        public EFreq GetFreq()
        {
            return G.ConvertFreq(this.Get(DName._posFreq).GetString());
        }

        public string GetNameAndFreq()
        {
            return this.GetName() + Globals.freqIndicator + this.GetFreq();
        }

        private StringOrTime[] GetIndexes()
        {
            //Should be ok fast
            StringOrTime[] result = new StringOrTime[this.storage.Length - DName._posIndex];
            Array.Copy(this.storage, DName._posIndex, result, 0, result.Length);
            return result;
        }

        private StringOrTime[] GetIndexesExceptTime()
        {
            //Should be ok fast
            if (!this.HasTime())
            {
                return this.GetIndexes();
            }
            else
            {                
                List<StringOrTime> result = new List<StringOrTime>();
                for (int i = DName._posIndex; i < this.storage.Length; i++)
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
            return new DName(this.GetName(), this.GetFreq(), this.GetIndexesExceptTime());
        }

        public bool HasTime() 
        {
            if (this.timePosition == -1) return false;
            return true;
        }


        public bool HasIndex()
        {
            if (this.GetLength() > DName._posIndex) return true;
            return false;
        }

        public DName ConvertToLag(GekkoTime t)
        {
            if (!this.HasTime()) new Error("DName: cannot find time dimension to convert into lag/lead");
            GekkoTime thisT = this.GetTime();
            int lag = thisT.Subtract(t); //will fail if freq mismatch. Note: -2 means lagged 2 periods.
            StringOrTime[] elements = this.GetIndexes();
            elements[this.timePosition - DName._posIndex] = new GekkoTime(EFreq.Lag, lag);  //Note: -2 because elements is without name and freq.
            DName name = new DName(this.GetName(), this.GetFreq(), elements);
            return name;
        }

        public DName HACK_RemoveTime()
        {
            List<string> temp = this.HACK_IndexesWithoutTime();
            List<StringOrTime> temp2 = new List<StringOrTime>();
            foreach (string s in temp) temp2.Add(s);
            return new DName(this.GetName(), this.GetFreq(), temp2.ToArray());            
        }

        public string HACK_ToStringWithoutTime()
        {
            DName without = this.HACK_RemoveTime();
            return without.ToString();
        }

        public List<string> HACK_IndexesWithoutTime()
        {
            List<string> temp = new List<string>();
            for (int i = DName._posIndex; i < this.GetLength(); i++)
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
            for (int i = DName._posIndex; i < this.GetLength() - 1; i++)
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
            return new DName(this.GetName(), this.GetFreq(), temp.ToArray());
        }

        public DName HACK_AddString(string element)
        {
            List<StringOrTime> temp = new List<StringOrTime>();
            for (int i = DName._posIndex; i < this.GetLength(); i++) temp.Add(this.Get(i));            
            temp.Add(element);
            return new DName(this.GetName(), this.GetFreq(), temp.ToArray());
        }

        public DName HACK_AddTime(GekkoTime t)
        {
            if (this.HasTime()) new Error("Cannot add time to variable that already has time");
            List<StringOrTime> temp = new List<StringOrTime>();
            for (int i = DName._posIndex; i < this.GetLength(); i++) temp.Add(this.Get(i));            
            temp.Add(t);            
            return new DName(this.GetName(), this.GetFreq(), temp.ToArray());
        }

        public DName HACK_Prefix(string s)
        {
            //List<StringOrTime> temp = new List<StringOrTime>();
            //for (int i = DName._posIndex; i < this.GetLength(); i++) temp.Add(this.Get(i));            
            //return new DName(s + this.GetName(), this.GetFreq(), temp.ToArray());
            return new DName(s + this.GetName(), this.GetFreq(), this.GetIndexes());
        }

        /// <summary>
        /// May return GekkoTime.tNull if no time present.
        /// </summary>
        /// <returns></returns>
        public GekkoTime GetTime()
        {
            if (!this.HasTime()) return GekkoTime.tNull;            
            return this.Get(this.timePosition).GetTime();            
        }

        public int GetLag()
        {
            if (!this.HasTime()) new Error("No time part found");
            GekkoTime t = this.Get(this.timePosition).GetTime();
            if (t.freq != EFreq.Lag) new Error("Expected lag time type");
            return t.super;
        }

        private static StringOrTime[] Construct(string name, StringOrTime[] indexes)
        {
            return Construct(name, EFreq.None, indexes);
        }

        private static StringOrTime[] Construct(string name, EFreq freq, StringOrTime[] indexes)
        {
            int offset = DName._posIndex;
            var result = new StringOrTime[indexes.Length + offset];
            result[DName._posName] = name;
            result[DName._posFreq] = G.ConvertFreq(freq);
            Array.Copy(indexes, 0, result, offset, indexes.Length);
            return result;
        }

        public override string ToString()
        {
            return this.ToString(DName.dNameFormatDefault);
        }

        public string ToStringWithoutFreq()
        {            
            DNameFormat d = new DNameFormat();
            d.showFreq = false;
            return G.Chop_RemoveFreq(this.ToString(d));
        }

        public string ToString(DNameFormat format)
        {            
            List<string> temp = new List<string>();            
            for (int i = DName._posIndex; i < this.GetLength(); i++)
            {
                string s = null;
                StringOrTime stringOrTime = this.Get(i);
                if (format.separateTime != EDNameTime.None && stringOrTime.IsTime())
                {
                    continue;
                }
                if (format.format== EDNameQuotes.Quotes && stringOrTime.IsString())
                {
                    s = "'" + stringOrTime.ToString() + "'";
                }
                else s = stringOrTime.ToString();
                if (s != null) temp.Add(s);
            }
            string rv = null;
            string naf = null;
            if (format.showFreq && this.GetFreq() != EFreq.None) naf = this.GetNameAndFreq();
            else naf = this.GetName();
            if (temp.Count == 0) rv = naf;
            else rv = naf + "[" + Stringlist.GetListWithCommas(temp, format.separator) + "]";

            if (this.HasTime() && (format.separateTime == EDNameTime.Last || format.separateTime == EDNameTime.LastExceptLag0))
            {
                GekkoTime t = this.GetTime();
                if (format.separateTime == EDNameTime.LastExceptLag0 && t.freq == EFreq.Lag && t.super == 0)
                {
                    //ignore, so we do not get x[a,b][0] but x[a,b]
                }
                else
                {
                    rv += "[" + t.ToString() + "]";
                }
            }
            return rv;
        }

        public static List<DName> HACK1(List<string> ss)
        {
            List<DName> rv = new List<DName>();
            foreach (string s in ss)
            {
                rv.Add(DName.HACK1(s));
            }
            return rv;
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
                if (Globals.runningOnTTComputer) System.Windows.Forms.MessageBox.Show("HACK1 had turtle");
                string[] ss = s.Split('¤'); //#as7asdfkalsfdads
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
                return new DName(name, G.ConvertFreq(freq), m.ToArray());
            }            
        }

        public static List<DName> HACK1a(List<string> ss)
        {
            List<DName> rv = new List<DName>();
            foreach (string s in ss)
            {
                rv.Add(DName.HACK1a(s));
            }
            return rv;
        }

        /// <summary>
        /// Hacky, try to get rid of it when scalar model dicts are done
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DName HACK1a(string s)
        {
            string bank = null; string name = null; string freq = null; string[] indexes1 = null; string[] indexes2 = null;
            G.Chop_Chop_Jagged(s, out bank, out name, out freq, out indexes1, out indexes2);            
            List<StringOrTime> m = new List<StringOrTime>();
            bool hasTime = false;
            if (indexes1 != null)
            {
                foreach (string s2 in indexes1)
                {
                    if (G.LooksLikeYearOrQuarterOrMonth(s2))
                    {
                        new Error("Time period in the first [] in x[..., ...][...] not allowed");                        
                    }
                    else
                    {
                        m.Add(s2);
                    }
                }
            }
            GekkoTime t = new GekkoTime(EFreq.Lag, 0);
            if (indexes2 != null)
            {
                if (indexes2.Length != 1) new Error("Expected x[..., ...][...] pattern");
                t = new GekkoTime(EFreq.Lag, int.Parse(indexes2[0]));
                m.Add(t);
            }
            else
            {
                //t = new GekkoTime(EFreq.Lag, 0);
                m.Add(t);
            }
            return new DName(name, G.ConvertFreq(freq), m.ToArray());
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
