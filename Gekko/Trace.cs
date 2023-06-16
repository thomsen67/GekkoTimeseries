using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
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
        [ProtoMember(1)]
        public long id = 0;  //when assigned it is a random number > 1 and < long.MaxValue --> extremely unlikely to have collisions ever

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

        public string ToString()
        {
            string s = this.assignment;
            if (G.NullOrEmpty(s)) s = "<Entry from .meta>";
            return this.t1 + "-" + this.t2 + ": " + this.assignment;
        }

        public Trace()
        {            
            this.id = G.NextLong(Globals.random, 1, long.MaxValue - 1);  //collision is extremely unlikely
            //maybe here put it into dictionary with weak values
            //but where does that dictionary live? In a databank, no?
            //or maybe only make the dictionary when about 
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
                foreach (Trace child in this.precedents.GetStorageForIteration()) child.PrintRecursive(depth + 1, output);
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
                    foreach (Trace trace_other in ts.meta.trace.precedents.GetStorageForIteration())
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
    }


    [ProtoContract]
    public class Precedents
    {
        [ProtoMember(1)]
        public List<Trace> storage = null;

        public void AddRange(Precedents precedents)
        {
            
            if (precedents.storage != null)
            {
                if (this.storage == null) this.storage = new List<Trace>();
                this.storage.AddRange(precedents.storage);
            }
        }

        public void Add(Trace trace)
        {
            if (this.storage == null) this.storage = new List<Trace>();
            this.storage.Add(trace);
        }

        /// <summary>
        /// Only use this for iterators, to keep .storage encapsulated
        /// </summary>
        /// <returns></returns>
        public List<Trace> GetStorageForIteration()
        {
            return this.storage;
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

    }
}
