﻿using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gekko
{
    /// <summary>
    /// Used for the .trace field of timeseries
    /// </summary>    

    [ProtoContract]
    public class Trace
    {
        //[ProtoMember(1)]
        //public long id = 0;  //when assigned it is a random number > 1 and < long.MaxValue --> extremely unlikely to have collisions ever

        [ProtoMember(2)]
        public GekkoTime t1 = GekkoTime.tNull;

        [ProtoMember(3)]
        public GekkoTime t2 = GekkoTime.tNull;

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
        public List<GekkoTime> periods = new List<GekkoTime>();

        //[ProtoMember(10)]
        //public int ID = -12345; //used to recreate connections after protobuf


        public string ToString()
        {
            string s = this.t1 + "-" + this.t2 + ": " + this.assignment;
            if (G.NullOrEmpty(this.assignment)) s = "------- meta entry: " + this.bankAndVarnameWithFreq + " -------";
            return s;
        }

        public Trace()
        {            
            //this.id = G.NextLong(Globals.random, 1, long.MaxValue - 1);  //collision is extremely unlikely
            //maybe here put it into dictionary with weak values
            //but where does that dictionary live? In a databank, no?
            //or maybe only make the dictionary when about 
        }

        public void DeepTrace(TraceHelper th, Trace parent)
        {
            th.traceCount++;
            if (!th.dict.ContainsKey(this)) th.dict.Add(this, this.precedents);
            new Writeln("+ " + this.assignment);
            if (this.precedents.Count() > 0)
            {
                foreach (Trace trace in this.precedents.GetStorage())
                {
                    trace.DeepTrace(th, this);
                }
            }
        }

        public static void RestoreTraceConnections(Databank databank)
        {
            if (databank.traces == null) return;
            foreach (KeyValuePair<Trace, Precedents> kvp in databank.traces)
            {
                kvp.Key.precedents = kvp.Value;
            }

            Series c = Program.databanks.GetFirst().GetIVariable("c!a") as Series;
            Trace trace = c.meta.trace;

            databank.traces = null;
        }

        public static void RemoveTraceConnections(Databank databank)
        {
            Series c = Program.databanks.GetFirst().GetIVariable("c!a") as Series;
            Trace trace = c.meta.trace;

            TraceHelper th1 = Trace.CollectAllTraces(databank, 0);
            databank.traces = th1.dict;
            foreach (KeyValuePair<Trace, Precedents> kvp in databank.traces)
            {
                kvp.Key.precedents = null;
            }
            
            Trace trace2 = c.meta.trace;
        }        

        public Trace DeepClone()
        {
            Trace trace2 = new Trace();  //also creates id
            trace2.assignment=this.assignment;
            trace2.bankAndVarnameWithFreq = this.bankAndVarnameWithFreq;
            trace2.filenameAndPathAndLine= this.filenameAndPathAndLine;
            trace2.periods = new List<GekkoTime>();
            foreach (GekkoTime t in this.periods) trace2.periods.Add(t);
            trace2.t1 = this.t1;
            trace2.t2 = this.t2;
            trace2.precedents = this.precedents.DeepClone();
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

        public string Text()
        {
            string s = this.bankAndVarnameWithFreq;
            if (!this.t1.IsNull()) s += " " + this.t1 + "-" + this.t2;
            s += ": ";
            s += this.assignment;
            return s;
        }

        /// <summary>
        /// Type == 1 ---> Puts the new trace on top of the series traces. Reconnects the existing series trace(s) to the new trace.
        /// Used for copying a whole object. Type == 2 ---> ???
        /// </summary>
        /// <param name="ts"></param>
        public void PushIntoSeries(Series ts, int type)
        {
            //Type == 1: Puts itself alone as a parent of existing, which becomes a child
            //Type == 2: Puts itself besides any exising precedent, as a sibling. Removes the date(s) from its siblings.
            if (ts.meta.trace == null) ts.meta.trace = new Trace();
            if (type == 1)
            {                   
                this.precedents.AddRange(ts.meta.trace.precedents);
                ts.meta.trace.precedents = new Precedents();
                ts.meta.trace.precedents.Add(this);
            }
            else if (type == 2)
            {
                if (ts.meta.trace.precedents.Count() > 0)
                {
                    foreach (Trace trace_other in ts.meta.trace.precedents.GetStorage())
                    {
                        foreach (GekkoTime t in this.periods)
                        {
                            trace_other.periods.Remove(t);
                        }
                    }
                }
                ts.meta.trace.precedents.Add(this);
            }
            else new Error("Trace");
        }

        public static TraceHelper CollectAllTraces(Databank databank, int type) //type==0 just counts. Type==1 collects connections and records them. Type==2 reestablishes connections.
        {
            TraceHelper th1 = new TraceHelper();
            foreach (KeyValuePair<string, IVariable> kvp in databank.storage)
            {
                kvp.Value.DeepTrace(th1);
            }
            return th1;
        }
    }

    public class TraceHelper
    {
        public int varCount = 0;
        public int traceCount = 0;
        public Dictionary<Trace, Precedents> dict = new Dictionary<Trace, Precedents>();  //value is parent (may be null)
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
                if (this.storage == null) this.storage = new List<Trace>();
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

        public Precedents DeepClone()
        {
            Precedents precedents = new Precedents();            
            if (this.storage != null)
            {
                precedents.storage = new List<Trace>();
                foreach (Trace trace in this.storage)
                {
                    precedents.storage.Add(trace.DeepClone());
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
