using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Windows.Forms;
//using static alglib;

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
        GetAllMetasAndTracesAndDepths,
        GetAllMetasAndTraces,
        OnlyGetMetas
    }
        
    [ProtoContract]
    public class TraceContents
    {
        [ProtoMember(1)]
        private GekkoTime t1 = GekkoTime.tNull;

        [ProtoMember(2)]
        private GekkoTime t2 = GekkoTime.tNull;

        [ProtoMember(3)]
        public string bankAndVarnameWithFreq = null;

        [ProtoMember(4)]
        public string commandFileAndLine = null;

        /// <summary>
        /// An extra char in this .text will take up 2 bytes or 16 bits.
        /// </summary>
        [ProtoMember(5)]
        public string text = null;

        /// <summary>
        /// For instance the file from where data was imported. Will often be null.
        /// </summary>
        [ProtoMember(6)]
        public string dataFile = null;

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

        public GekkoTime GetT1()
        {
            return this.t1;
        }

        public GekkoTime GetT2()
        {
            return this.t2;
        }

        public TraceContents DeepClone()
        {
            TraceContents trace2 = new TraceContents();
            trace2.t1 = this.t1;
            trace2.t2 = this.t2;
            trace2.bankAndVarnameWithFreq = this.bankAndVarnameWithFreq;
            trace2.commandFileAndLine = this.commandFileAndLine;
            trace2.text = this.text;
            trace2.dataFile = this.dataFile;
            trace2.periods = new Periods();
            if (this.periods.Count() > 0)
            {
                foreach (KeyValuePair<GekkoTime, byte> kvp in this.periods.GetStorage()) trace2.periods.Add(kvp.Key);
            }
            return trace2;
        }

        public TraceContents()
        {
            //for protobuf
        }

        public TraceContents(GekkoTime t1, GekkoTime t2)
        {
            this.t1 = t1;
            this.t2 = t2;
            if (!this.t1.IsNull() && !this.t2.IsNull())
            {
                foreach (GekkoTime t in new GekkoTimeIterator(this.t1, this.t2)) this.periods.Add(t);  //add all
            }
        }
    }

    /// <summary>    
    /// We may have: 
    /// (1) contents != null and id.counter > 0 (normal).
    /// (2) contents == null and id.counter &lt; 0 (pruned off to external file).
    /// (3) contents == null and id.counter > 0 (temporarily prepared for protobuf).
    /// </summary>
    [ProtoContract]
    public class Trace
    {
        [ProtoMember(1)]
        public TraceID id = new TraceID();

        [ProtoMember(2)]
        public TraceContents contents = null;

        [ProtoMember(3)]
        public Precedents precedents = new Precedents();        

        private Trace()
        {
            //Only for protobuf and DeepClone()
        }

        /// <summary>
        /// Construct a parent trace. For this, .contents will be == null.
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
            this.contents = new TraceContents(t1, t2);            
        }
        
        
        public string ToString()
        {
            string s = null;
            if (this.GetTraceType() == ETraceType.Parent) s = "------- meta parent entry: " + this.contents.bankAndVarnameWithFreq + " -------";
            else s = this.contents.GetT1() + "-" + this.contents.GetT2() + ": " + this.contents.text;
            return s;
        }        

        public void DeepTrace(TraceHelper th, Trace parent, int depth)
        {
            if (th.type == ETraceHelper.GetAllMetasAndTraces || th.type == ETraceHelper.GetAllMetasAndTracesAndDepths)
            {
                th.traceCount++;
                if (!th.traces.ContainsKey(this)) th.traces.Add(this, this.precedents);
                if (th.type == ETraceHelper.GetAllMetasAndTracesAndDepths)
                {
                    if (!th.tracesDepth.ContainsKey(this)) th.tracesDepth.Add(this, depth);
                    else if (depth < th.tracesDepth[this]) th.tracesDepth[this] = depth;                    
                }
                if (this.precedents.Count() > 0)
                {
                    foreach (Trace trace in this.precedents.GetStorage())
                    {
                        trace.DeepTrace(th, this, depth + 1);
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
                if (this.contents != null)
                {
                    trace2.contents = this.contents.DeepClone();
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
            if (this.contents.periods.Count() > 0)
            {
                foreach (GekkoTime t in this.contents.periods.GetStorage().Keys) s += t.ToString() + ", ";
            }
            s += this.id.stamp.ToString("MM/dd/yyyy HH:mm:ss") + "|" + this.id.counter;
            return s;
        }

        /// <summary>
        /// Returns .Parent if .contents == null.
        /// </summary>
        /// <returns></returns>
        public ETraceType GetTraceType()
        {
            ETraceType x = ETraceType.Child;
            if (this.contents == null) x = ETraceType.Parent;
            return x;
        }

        public string Text()
        {
            string s = null;
            if (!this.contents.GetT1().IsNull()) s += "" + this.contents.GetT1() + "-" + this.contents.GetT2();
            s += ": ";
            s += this.contents.text;
            s += "          ";
            if (this.contents.bankAndVarnameWithFreq != null) s += " || lhs=" + this.contents.bankAndVarnameWithFreq;
            if (this.contents.dataFile != null) s += " || data=" + this.contents.dataFile;
            if (this.contents.commandFileAndLine != null) s += " || gcm=" + this.contents.commandFileAndLine;
            s += " || " + this.PeriodsAndStamp();
            return s;
        }

        /// <summary>
        /// Type == NewParent ---> Puts the new trace on top of the series traces. Reconnects the existing series trace(s) to the new trace.
        /// Type == Sibling ---> Puts it among siblings. Removes the date(s) from its siblings.
        /// </summary>
        /// <param name="ts"></param>
        public static void PushIntoSeries(Series ts, Trace ths, ETracePushType type)
        {
            if (ths.contents.text == null) new Error("PushIntoSeries problem");
            if (ts.meta.trace == null) ts.meta.trace = new Trace(ETraceType.Parent);
            if (type == ETracePushType.NewParent)
            {                   
                ths.precedents.AddRange(ts.meta.trace.precedents);
                ts.meta.trace.precedents = new Precedents();
                ts.meta.trace.precedents.Add(ths);
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
                        if (ths.contents.periods.Count() > 0)
                        {
                            int countStart = sibling.contents.periods.Count();
                            foreach (GekkoTime t in ths.contents.periods.GetStorage().Keys)
                            {
                                sibling.contents.periods.Remove(t);
                            }
                            if (countStart > 0 && sibling.contents.periods.Count() == 0)
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
                            Trace.RemoveFromSeries(ts, remove);
                        }
                    }
                }
                ts.meta.trace.precedents.Add(ths);
            }
            else new Error("Trace");
        }

        /// <summary>
        /// Removes a particular trace from ts.meta.trace in a Series. Happens when a new Trace shadows other older traces.
        /// </summary>
        /// <param name="ts"></param>
        public static void RemoveFromSeries(Series ts, Trace ths)
        {
            if (ts.meta.trace == null) return;
            if (ts.meta.trace.precedents.Count() > 0)
            {                
                ts.meta.trace.precedents.GetStorage().Remove(ths);
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
        /// After deserializing a protobuf gbk, this method restores trace connections from flat list (databank.traces).
        /// </summary>
        /// <param name="databank"></param>
        public static void HandleTraceRead1(Databank databank)
        {
            if (databank.traces != null)
            {
                TraceHelper th = Gekko.Trace.CollectAllTraces(databank, ETraceHelper.OnlyGetMetas);
                Dictionary<TraceID, Trace> dictInverted = new Dictionary<TraceID, Trace>();
                foreach (Trace trace in databank.traces) dictInverted[trace.id] = trace;
                HandleTraceRead2(th.metas, dictInverted);
                databank.traces = null;  //important!
            }
        }

        /// <summary>
        /// After deserializing a protobuf gbk, this method restores trace connections from flat list (databank.traces).
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
        /// Before serializing a protobuf gbk, this method removes trace connections, and kind of packs the connections into a flat list (databank.traces).
        /// </summary>
        /// <param name="databank"></param>
        /// <param name="th"></param>
        /// <param name="dict1Inverted"></param>
        public static void HandleTraceWrite(Databank databank, out TraceHelper th, out Dictionary<TraceID, Trace> dict1Inverted)
        {
            //gather lists
            th = Gekko.Trace.CollectAllTraces(databank, ETraceHelper.GetAllMetasAndTraces);
            dict1Inverted = new Dictionary<TraceID, Trace>();
            foreach (Trace trace in th.traces.Keys)
            {
                dict1Inverted[trace.id] = trace;
                trace.precedents.ToID();  //remove links
            }
            foreach (SeriesMetaInformation meta in th.metas)
            {
                meta.ToID();
            }
            databank.traces = th.traces.Keys.ToList();
            if (Globals.runningOnTTComputer) new Writeln("TTH: " + databank.traces.Count + " traces written");
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
        /// Used to distinguish traces, especially if these are pruned off. Will be numerically > 0, and when counter is < 0 it means that the trace is stored in en external file (pruned off).
        /// When Gekko starts up, the counter starts at a random position between 1 and 99% of long.MaxValue (9e18) and augments by 1 for each new trace.
        /// If the same Gekko session is used, there can be no collisions realistically, neither any overflow (that would demand > 9e16 calculations).
        /// With multiple Gekkos running at the same time, collisions would demand same stamp (unlikely) AND same counter (probability around 1e-19).
        /// Should never happen.
        /// </summary>
        [ProtoMember(2)]
        public long counter = ++Globals.traceCounter;

        public override bool Equals(object o)
        {
            TraceID other = o as TraceID;
            if (other != null && this.stamp == other.stamp && this.counter == other.counter) return true;
            return false;
        }

        public override string ToString()
        {
            return this.stamp.ToString() + "|" + this.counter;
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
        public ETraceHelper type = ETraceHelper.GetAllMetasAndTraces;
        public int varCount = 0; //number of series found (probably often equatl to meta count)
        public int traceCount = 0; //will include combinations, traces will not
        public Dictionary<Trace, Precedents> traces = new Dictionary<Trace, Precedents>();  //value is parent (may be null)
        public Dictionary<Trace, int> tracesDepth = new Dictionary<Trace, int>(); //value is depth
        public List<SeriesMetaInformation> metas = new List<SeriesMetaInformation>();
    }

    /// <summary>
    /// Is basically a List&lt;Trace>.
    /// </summary>
    [ProtoContract]
    public class Precedents
    {
        [ProtoMember(1)]
        private List<Trace> storage = null;

        /// <summary>
        /// Pretty innocuous: using this, we can set .storage = null before protobuf.
        /// </summary>
        [ProtoMember(2)]
        public List<TraceID> storageIDTemporary = null;  //used to recreate connections after protobuf. Will not take up space in general.

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
            if (trace.contents == null) throw new GekkoException();
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
            this.storageIDTemporary = new List<TraceID>();
            if (this.Count() > 0)
            {
                foreach (Trace trace in this.GetStorage())
                {
                    this.storageIDTemporary.Add(trace.id);
                }
            }
            this.SetStorage(null);
        }

        public void FromID(Dictionary<TraceID, Trace> dict2)
        {
            if (this.storageIDTemporary != null && this.storageIDTemporary.Count > 0)
            {
                this.storage = new List<Trace>();
                foreach (TraceID id in this.storageIDTemporary)
                {
                    if (id.counter < 0) new Error("This trace is not stored in the databank, but has been pruned off: " + id.ToString());
                    Trace trace = null; dict2.TryGetValue(id, out trace);
                    if (trace == null) new Error("Could not find this trace in databank: " + id.ToString());
                    this.storage.Add(trace);
                }
            }
            this.storageIDTemporary = null;
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
