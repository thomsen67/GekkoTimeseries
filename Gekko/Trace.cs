﻿using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

    [ProtoContract]
    public class Trace
    {
        //[ProtoMember(1)]
        //public long id = 0;  //when assigned it is a random number > 1 and < long.MaxValue --> extremely unlikely to have collisions ever

        [ProtoMember(1)]
        public short version = -12345;

        [ProtoMember(2)]
        private GekkoTime t1 = GekkoTime.tNull;

        [ProtoMember(3)]
        private GekkoTime t2 = GekkoTime.tNull;

        [ProtoMember(4)]
        public string bankAndVarnameWithFreq = null;

        [ProtoMember(5)]
        public DateTime stamp = DateTime.MinValue;

        [ProtoMember(6)]
        public string filenameAndPathAndLine = null;

        [ProtoMember(7)]
        public string assignment = null;

        [ProtoMember(8)]
        public Precedents precedents = new Precedents();

        [ProtoMember(9)]
        public Dictionary<GekkoTime, byte> periods = new Dictionary<GekkoTime, byte>();

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
            this.stamp = DateTime.Now;
            this.version = Globals.TraceVersion;
            this.t1 = t1;
            this.t2 = t2;
            if (!this.t1.IsNull() && !this.t2.IsNull())
            {
                foreach (GekkoTime t in new GekkoTimeIterator(this.t1, this.t2)) this.periods.Add(t, 0);  //add all
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
                if (!th.dict2.ContainsKey(this)) th.dict2.Add(this, th.dict2.Count);
                //new Writeln("+ " + this.assignment);
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
                trace2.version = this.version;
                trace2.t1 = this.t1;
                trace2.t2 = this.t2;
                trace2.bankAndVarnameWithFreq = this.bankAndVarnameWithFreq;
                trace2.stamp = this.stamp;
                trace2.version = this.version;
                trace2.filenameAndPathAndLine = this.filenameAndPathAndLine;
                trace2.assignment = this.assignment;
                trace2.periods = new Dictionary<GekkoTime, byte>();
                foreach (KeyValuePair<GekkoTime, byte> kvp in this.periods) trace2.periods.Add(kvp.Key, kvp.Value);
                
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
            foreach (GekkoTime t in this.periods.Keys) s += t.ToString() + ", ";
            s += this.stamp.ToString("MM/dd/yyyy HH:mm:ss");
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
                    foreach (Trace sibling in ts.meta.trace.precedents.GetStorage())
                    {
                        //We know that sibling's parent always has GetTraceType() == ETraceType.Parent
                        //So the siblings all belong to the same timeseries, and therefore it is ok
                        //to remove periods.
                        foreach (GekkoTime t in this.periods.Keys)
                        {                            
                            sibling.periods.Remove(t);
                        }
                    }
                }
                ts.meta.trace.precedents.Add(this);
            }
            else new Error("Trace");
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
                Trace[] dictInverted = new Trace[databank.traces.Count];
                foreach (KeyValuePair<Trace, int> kvp in databank.traces) dictInverted[kvp.Value] = kvp.Key;
                HandleTraceRead2(th.metas, dictInverted);
                databank.traces = null;
            }
        }

        /// <summary>
        /// After deserializing a protobuf gbk, this method restores trace connections from flat dict (databank.traces).
        /// </summary>
        public static void HandleTraceRead2(List<SeriesMetaInformation> metas, Trace[] dict1Inverted)
        {                         
            foreach (SeriesMetaInformation meta in metas)
            {
                meta.FromID(dict1Inverted);
                meta.traceID = -12345;
            }
            foreach (Trace trace in dict1Inverted)
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
        public static void HandleTraceWrite(Databank databank, out TraceHelper th, out Trace[] dict1Inverted)
        {
            //gather lists
            th = Gekko.Trace.CollectAllTraces(databank, ETraceHelper.GetAllStuff);
            Dictionary<Trace, int> dict1 = th.dict2;
            dict1Inverted = new Trace[dict1.Count];
            foreach (KeyValuePair<Trace, int> kvp in dict1)
            {
                dict1Inverted[kvp.Value] = kvp.Key;
                kvp.Key.precedents.ToID(dict1);  //remove links
            }
            foreach (SeriesMetaInformation meta in th.metas)
            {
                meta.ToID(dict1);
            }
            databank.traces = th.dict2;
        }


    }

    public enum ETraceHelper
    {
        GetAllStuff,
        OnlyGetMeta
    }

    public class TraceHelper
    {
        public ETraceHelper type = ETraceHelper.GetAllStuff;
        public int varCount = 0;
        public int traceCount = 0;
        public Dictionary<Trace, Precedents> dict = new Dictionary<Trace, Precedents>();  //value is parent (may be null)
        public Dictionary<Trace, int> dict2 = new Dictionary<Trace, int>();
        public List<SeriesMetaInformation> metas = new List<SeriesMetaInformation>();
    }


    [ProtoContract]
    public class Precedents
    {
        [ProtoMember(1)]
        private List<Trace> storage = null;

        [ProtoMember(2)]
        public List<int> storageID = null;  //used to recreate connections after protobuf

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
        /// Use this with care. For instance for iterators.
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

        public  void ToID(Dictionary<Trace, int> dict1)
        {
            this.storageID = new List<int>();
            if (this.Count() > 0)
            {
                foreach (Trace trace in this.GetStorage())
                {
                    this.storageID.Add(dict1[trace]);
                }
            }
            this.SetStorage(null);
        }

        public void FromID(Trace[] dict2)
        {
            if (this.storageID != null && this.storageID.Count > 0)
            {
                this.storage = new List<Trace>();
                foreach (int id in this.storageID)
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
}
