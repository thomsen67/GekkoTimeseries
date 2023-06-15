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
        public List<Trace> precedents = null;

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
            if (this.precedents != null)
            {
                trace2.precedents = new List<Trace>();
                foreach (Trace trace2Clone in this.precedents) trace2.precedents.Add(trace2Clone.DeepClone());
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
            if (this.precedents != null)
            {
                foreach (Trace child in this.precedents) child.PrintRecursive(depth + 1, output);
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
            //Could code for type 1 and 2 be merged somehow
            if (ts.meta.trace == null) ts.meta.trace = new Trace();
            if (type == 1)
            {
                this.precedents = new List<Trace>();
                if (ts.meta.trace.precedents != null) this.precedents.AddRange(ts.meta.trace.precedents);
                ts.meta.trace.precedents = new List<Trace> { this };
            }
            else if (type == 2)
            {
                if (ts.meta.trace.precedents == null) ts.meta.trace.precedents = new List<Trace>();
                foreach (Trace trace_other in ts.meta.trace.precedents)
                {
                    foreach (GekkoTime t in this.periods)
                    {
                        trace_other.periods.Remove(t);
                    }
                }
                ts.meta.trace.precedents.Add(this);
            }
            else new Error("Trace");
        }

        /// <summary>
        /// Puts the new trace on top of the series traces. 
        /// First it puts any traces from the extraToAdd series (may remove periods from these).
        /// Then it adds the existing series trace(s). Used for copy-inject into existing object.
        /// </summary>
        /// <param name="extraToAdd"></param>
        /// <param name="ts"></param>
        /// <param name="newTrace"></param>
        public void PushIntoSeries(Series ts, Series extraToAdd)
        {
            if (ts.meta.trace == null) ts.meta.trace = new Trace();
            this.precedents = new List<Trace>();
            if (extraToAdd.meta.trace.precedents != null) this.precedents.AddRange(extraToAdd.meta.trace.precedents);
            foreach (Trace trace_other in ts.meta.trace.precedents)
            {
                foreach (GekkoTime t in this.periods)
                {
                    trace_other.periods.Remove(t);
                }
            }
            if (ts.meta.trace.precedents == null) ts.meta.trace.precedents = new List<Trace>();
            ts.meta.trace.precedents.Add(this);
        }
    }
}
