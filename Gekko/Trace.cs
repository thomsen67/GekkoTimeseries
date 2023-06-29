using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using static alglib;

namespace Gekko
{
    /// <summary>
    /// Used for the .trace field of timeseries
    /// </summary>    
    /// 
    public enum ETraceType
    {
        Parent,
        Child
    }

    public enum ETracePushType
    {
        Sibling,
        NewParent
    }

    public enum ETraceHelper
    {
        GetAllStuff,
        OnlyGetMeta
    }

    [ProtoContract]
    public class Trace
    {
        [ProtoMember(1)]
        public TraceID id = new TraceID();

        [ProtoMember(2)]
        private GekkoTime t1 = GekkoTime.tNull;

        [ProtoMember(3)]
        private GekkoTime t2 = GekkoTime.tNull;

        [ProtoMember(4)]
        public string bankAndVarnameWithFreq = null;        

        [ProtoMember(5)]
        public string filenameAndPathAndLine = null;

        [ProtoMember(6)]
        public string assignment = null;

        /// <summary>
        /// The "active" left-hand side periods for the current trace (that is, what the trace determines). 
        /// At some point, the periods inside should be compacted. For instance a list of GekkoTimes 1966-2022 is a
        /// waste of space. Some kind of interval logic could be implemented. For instance, updating 2000-2010 over the 
        /// 1966-2022 interval would break 1966-2022 into --> 1966-1999 and 2011-2022. Would save a lot of space.
        /// See maybe https://github.com/mbuchetics/RangeTree for ideas. But this tree is only for searching though.
        /// Perhaps allow combo of intervals (for > 3 dates) and single dates.
        /// </summary>
        [ProtoMember(7)]        
        public Periods periods = new Periods();

        [ProtoMember(8)]
        public Precedents precedents = new Precedents();        

        private Trace()
        {
            //Only for protobuf and DeepClone()
        }

        /// <summary>
        /// Construct a parent trace. For this, .t1 and .t2 will be null.
        /// </summary>
        /// <param name="type"></param>
        public Trace(ETraceType type)
        {
            if (type == ETraceType.Child) new Error("Trace constructor problem");
        }

        /// <summary>
        /// Child trace. Also sets stamp, traceversion and .t1 and .t2 (fills .periods with this range).
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public Trace(GekkoTime t1, GekkoTime t2)
        {
            //this.id = G.NextLong(Globals.random, 1, long.MaxValue - 1);  //collision is extremely unlikely
            //maybe here put it into dictionary with weak values
            //but where does that dictionary live? In a databank, no?
            //or maybe only make the dictionary when about
                        
            this.t1 = t1;
            this.t2 = t2;
            if (!this.t1.IsNull() && !this.t2.IsNull())
            {
                foreach (GekkoTime t in new GekkoTimeIterator(this.t1, this.t2)) this.periods.Add(t);  //add all
            }
        }

        public GekkoTime GetT1()
        {
            return this.t1;
        }

        public GekkoTime GetT2()
        {
            return this.t2;
        }        
        
        public string ToString()
        {
            string s = null;
            if (this.GetTraceType() == ETraceType.Parent) s = "------- meta parent entry: " + this.bankAndVarnameWithFreq + " -------";
            else s = this.t1 + "-" + this.t2 + ": " + this.assignment;
            return s;
        }        

        public void DeepTrace(TraceHelper th, Trace parent)
        {
            if (th.type == ETraceHelper.GetAllStuff)
            {
                th.traceCount++;
                if (!th.dict.ContainsKey(this)) th.dict.Add(this, this.precedents);
                if (!th.dict2.ContainsKey(this)) th.dict2.Add(this, this.id);
                if (this.precedents.Count() > 0)
                {
                    foreach (Trace trace in this.precedents.GetStorage())
                    {
                        trace.DeepTrace(th, this);
                    }
                }
            }            
        }        

        public Trace DeepClone(CloneHelper cloneHelper)
        {
            object known = null;
            Trace trace2 = null;
            if (cloneHelper != null)
            {
                cloneHelper.dict.TryGetValue(this, out known);
            }
            if (known == null)
            {
                trace2 = new Trace();
                trace2.t1 = this.t1;
                trace2.t2 = this.t2;
                trace2.bankAndVarnameWithFreq = this.bankAndVarnameWithFreq;                
                trace2.filenameAndPathAndLine = this.filenameAndPathAndLine;
                trace2.assignment = this.assignment;
                trace2.periods = new Periods();
                if (this.periods.Count() > 0)
                {
                    foreach (KeyValuePair<GekkoTime, byte> kvp in this.periods.GetStorage()) trace2.periods.Add(kvp.Key);
                }                
                trace2.precedents = this.precedents.DeepClone(cloneHelper);
                if (cloneHelper != null)
                {
                    cloneHelper.dict.Add(this, trace2);
                }
            }
            else
            {
                trace2 = known as Trace;
            }            
            return trace2;
        }

        public void PrintRecursive(int depth, List<string> output)
        {
            if (depth > 0)
            {
                string s = Text();
                output.Add("-" + G.Blanks(2 * (depth - 1)) + s);
            }
            if (this.precedents.Count() > 0)
            {
                foreach (Trace child in this.precedents.GetStorage()) child.PrintRecursive(depth + 1, output);
            }            
            if (depth == 0 && output.Count == 0) new Writeln("[No trace found]");
        }

        /// <summary>
        /// Pretty print periods and stamp.
        /// </summary>
        /// <returns></returns>
        public string PeriodsAndStamp()
        {
            string s = null;
            if (this.periods.Count() > 0)
            {
                foreach (GekkoTime t in this.periods.GetStorage().Keys) s += t.ToString() + ", ";
            }
            s += this.id.stamp.ToString("MM/dd/yyyy HH:mm:ss") + " | " + this.id.counter;
            return s;
        }

        /// <summary>
        /// Returns .Parent if .assignment == null.
        /// </summary>
        /// <returns></returns>
        public ETraceType GetTraceType()
        {
            ETraceType x = ETraceType.Child;
            if (this.assignment == null) x = ETraceType.Parent;
            return x;
        }

        public string Text()
        {
            string s = this.bankAndVarnameWithFreq;
            if (!this.t1.IsNull()) s += " " + this.t1 + "-" + this.t2;
            s += ": ";
            s += this.assignment;
            s += "              " + this.PeriodsAndStamp();
            return s;
        }

        /// <summary>
        /// Type == NewParent ---> Puts the new trace on top of the series traces. Reconnects the existing series trace(s) to the new trace.
        /// Type == Sibling ---> Puts it among siblings. Removes the date(s) from its siblings.
        /// </summary>
        /// <param name="ts"></param>
        public void PushIntoSeries(Series ts, ETracePushType type)
        {
            if (this.assignment == null) new Error("PushIntoSeries problem");
            if (ts.meta.trace == null) ts.meta.trace = new Trace(ETraceType.Parent);
            if (type == ETracePushType.NewParent)
            {                   
                this.precedents.AddRange(ts.meta.trace.precedents);
                ts.meta.trace.precedents = new Precedents();
                ts.meta.trace.precedents.Add(this);
            }
            else if (type == ETracePushType.Sibling)
            {
                if (ts.meta.trace.precedents.Count() > 0)
                {
                    if (ts.meta.trace.GetTraceType() != ETraceType.Parent) new Error("Trace type error");  //should never be possible huh???
                    List<Trace> toRemove = new List<Trace>();
                    foreach (Trace sibling in ts.meta.trace.precedents.GetStorage())
                    {
                        //We know that sibling's parent always has GetTraceType() == ETraceType.Parent
                        //So the siblings all belong to the same timeseries, and therefore it is ok
                        //to remove periods.
                        if (this.periods.Count() > 0)
                        {
                            int countStart = sibling.periods.Count();
                            foreach (GekkoTime t in this.periods.GetStorage().Keys)
                            {
                                sibling.periods.Remove(t);
                            }
                            if (countStart > 0 && sibling.periods.Count() == 0)
                            {
                                //last period has been removed
                                toRemove.Add(sibling);
                            }
                        }
                    }
                    if (toRemove.Count > 0)
                    {
                        foreach (Trace remove in toRemove)
                        {
                            remove.RemoveFromSeries(ts);
                        }
                    }
                }
                ts.meta.trace.precedents.Add(this);
            }
            else new Error("Trace");
        }

        /// <summary>
        /// Removes a particular trace from ts.meta.trace in a Series. Happens when a new Trace shadows other older traces.
        /// </summary>
        /// <param name="ts"></param>
        public void RemoveFromSeries(Series ts)
        {
            if (ts.meta.trace == null) return;
            if (ts.meta.trace.precedents.Count() > 0)
            {                
                ts.meta.trace.precedents.GetStorage().Remove(this);
            }
        }

        public static TraceHelper CollectAllTraces(Databank databank, ETraceHelper type)
        {            
            TraceHelper th1 = new TraceHelper();
            th1.type = type;
            foreach (KeyValuePair<string, IVariable> kvp in databank.storage)
            {
                kvp.Value.DeepTrace(th1);
            }
            return th1;
        }

        /// <summary>
        /// After deserializing a protobuf gbk, this method restores trace connections from flat dict (databank.traces).
        /// </summary>
        /// <param name="databank"></param>
        public static void HandleTraceRead1(Databank databank)
        {
            if (databank.traces != null)
            {
                TraceHelper th = Gekko.Trace.CollectAllTraces(databank, ETraceHelper.OnlyGetMeta);
                Dictionary<TraceID, Trace> dictInverted = new Dictionary<TraceID, Trace>();
                foreach (Trace kvp in databank.traces) dictInverted[kvp.id] = kvp;
                HandleTraceRead2(th.metas, dictInverted);
                databank.traces = null;
            }
        }

        /// <summary>
        /// After deserializing a protobuf gbk, this method restores trace connections from flat dict (databank.traces).
        /// </summary>
        public static void HandleTraceRead2(List<SeriesMetaInformation> metas, Dictionary<TraceID, Trace> dict1Inverted)
        {                         
            foreach (SeriesMetaInformation meta in metas)
            {
                meta.FromID(dict1Inverted);
            }
            foreach (Trace trace in dict1Inverted.Values)
            {
                trace.precedents.FromID(dict1Inverted);
            }
        }

        /// <summary>
        /// Before serializing a protobuf gbk, this method removes trace connections, and kind of packs the connections into a flat dict (databank.traces).
        /// </summary>
        /// <param name="databank"></param>
        /// <param name="th"></param>
        /// <param name="dict1Inverted"></param>
        public static void HandleTraceWrite(Databank databank, out TraceHelper th, out Dictionary<TraceID, Trace> dict1Inverted)
        {
            //gather lists
            th = Gekko.Trace.CollectAllTraces(databank, ETraceHelper.GetAllStuff);
            Dictionary<Trace, TraceID> dict1 = th.dict2;
            dict1Inverted = new Dictionary<TraceID, Trace>();
            foreach (KeyValuePair<Trace, TraceID> kvp in dict1)
            {
                dict1Inverted[kvp.Value] = kvp.Key;
                kvp.Key.precedents.ToID();  //remove links
            }
            foreach (SeriesMetaInformation meta in th.metas)
            {
                meta.ToID();
            }
            databank.traces = th.dict2.Keys.ToList();
        }
    }

    [ProtoContract]
    public class TraceID
    {
        /// <summary>
        /// Note: resolution is about 0.01 s.
        /// </summary>
        [ProtoMember(1)]
        public DateTime stamp = DateTime.Now;

        /// <summary>
        /// Used to distinguish traces, especially if these are pruned off. 
        /// When Gekko starts up, the counter starts at a random position between 0 and uint.MaxValue (4.3e9) and augments by 1 for each new trace.
        /// If the same Gekko session is used, there can be no collisions (cannot perform 4.3e9 calculations in less than 0.01s).
        /// With multiple Gekkos running at the same time, collisions would demand same stamp (unlikely) AND same counter (probability around 1e-9).
        /// (Even if it did happen and 2 traces had same TraceId, the name of the variable could probably distinguish).
        /// </summary>
        [ProtoMember(2)]
        public uint counter = ++Globals.traceCounter;

        public override bool Equals(object o)
        {
            TraceID other = o as TraceID;
            if (other != null && this.stamp == other.stamp && this.counter == other.counter) return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + this.stamp.GetHashCode();
            hash = hash * 31 + this.counter.GetHashCode();
            return hash;
        }
    }

    public class TraceHelper
    {
        public ETraceHelper type = ETraceHelper.GetAllStuff;
        public int varCount = 0;
        public int traceCount = 0;
        public Dictionary<Trace, Precedents> dict = new Dictionary<Trace, Precedents>();  //value is parent (may be null)
        public Dictionary<Trace, TraceID> dict2 = new Dictionary<Trace, TraceID>();
        public List<SeriesMetaInformation> metas = new List<SeriesMetaInformation>();
    }


    [ProtoContract]
    public class Precedents
    {
        [ProtoMember(1)]
        private List<Trace> storage = null;

        [ProtoMember(2)]
        public List<TraceID> storageID = null;  //used to recreate connections after protobuf

        public void AddRange(Precedents precedents)
        {            
            if (precedents.storage != null)
            {
                if (this.storage == null) this.storage = new List<Trace>();
                this.storage.AddRange(precedents.storage);
            }
        }

        /// <summary>
        /// Add a Trace to precedents list. Cannot add a "meta entry" to a Trace. These can only be set for .trace in SeriesMetaInformation objects.
        /// </summary>
        /// <param name="trace"></param>
        /// <exception cref="GekkoException"></exception>
        public void Add(Trace trace)
        {
            if (this.storage == null) this.storage = new List<Trace>();
            if (G.NullOrBlanks(trace.assignment)) throw new GekkoException();
            this.storage.Add(trace);
        }

        /// <summary>
        /// Only for iterators! REMEMBER to put an "if (xxx.precedents.Count() > 0) {... " before iterating!!
        /// </summary>
        /// <returns></returns>
        public List<Trace> GetStorage()
        {
            return this.storage;
        }

        /// <summary>
        /// Use this with care
        /// </summary>
        /// <param name="m"></param>
        public void SetStorage(List<Trace> m)
        {
            if (m != null && m.Count == 0) this.storage = null; //so it does not take up space
            else this.storage = m;
        }

        public  void ToID()
        {
            this.storageID = new List<TraceID>();
            if (this.Count() > 0)
            {
                foreach (Trace trace in this.GetStorage())
                {
                    this.storageID.Add(trace.id);
                }
            }
            this.SetStorage(null);
        }

        public void FromID(Dictionary<TraceID, Trace> dict2)
        {
            if (this.storageID != null && this.storageID.Count > 0)
            {
                this.storage = new List<Trace>();
                foreach (TraceID id in this.storageID)
                {                    
                    this.storage.Add(dict2[id]);
                }
            }
            this.storageID = null;
        }

        public int Count()
        {
            if (this.storage == null) return 0;
            return this.storage.Count;
        }

        public Trace this[int i]
        {
            get { return this.storage[i]; }
            set { this.storage[i] = value; }
        }

        public Precedents DeepClone(CloneHelper cloneHelper)
        {
            Precedents precedents = new Precedents();            
            if (this.storage != null)
            {
                precedents.storage = new List<Trace>();
                foreach (Trace trace in this.storage)
                {
                    precedents.storage.Add(trace.DeepClone(cloneHelper));
                }
            }
            return precedents;
        }

        public string ToString()
        {
            return "Traces = " + this.Count();
        }
    }

    [ProtoContract]
    public class Periods
    {
        private Dictionary<GekkoTime, byte> storage = null;

        public void Add(GekkoTime t)
        {
            if (this.storage == null) this.storage = new Dictionary<GekkoTime, byte>();
            this.storage.Add(t, 0);
        }
        public void Remove(GekkoTime t)
        {
            if (this.storage == null) return;
            this.storage.Remove(t);
        }

        public int Count()
        {
            if (this.storage == null) return 0;
            return this.storage.Count;
        }

        /// <summary>
        /// Only for iterators! REMEMBER to put an "if (xxx.periods.Count() > 0) {... " before iterating!!
        /// </summary>
        /// <returns></returns>
        public Dictionary<GekkoTime, byte> GetStorage()
        {
            return this.storage;
        }
    }
}
