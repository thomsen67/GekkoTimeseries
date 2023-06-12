using ProtoBuf;
using System;
using System.Collections.Generic;
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
        public List<Trace2> traces2 = new List<Trace2>();
        public Trace()
        {
            //just for protobuf
        }        

        public static void Walker(Trace2 trace2, int d)
        {            
            using (Writeln txt = new Writeln())
            {
                txt.indent = G.Blanks(4 * d);
                txt.MainAdd("Variable: " + trace2.bankAndVarnameWithFreq);
                txt.MainNewLineTight();
                txt.MainAdd("Stamp: " + trace2.assignment);
                txt.MainNewLineTight();
                txt.MainAdd("Stamp: " + trace2.stamp);
                txt.MainNewLineTight();
                txt.MainAdd("File: " + trace2.filenameAndPathAndLine);
                txt.MainNewLineTight();
            }
            if (trace2.precedents != null)
            {
                foreach (Trace2 childTrace2 in trace2.precedents)
                {
                    Walker(childTrace2, d + 1);
                }
            }
        }
    }

    [ProtoContract]
    public class Trace2
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
        public List<Trace2> precedents = null;

        [ProtoMember(9)]
        public List<GekkoTime> periods = new List<GekkoTime>();

        public string ToString()
        {
            string s = this.assignment;
            if (G.NullOrEmpty(s)) s = "<Entry from .meta>";
            return this.t1 + "-" + this.t2 + ": " + this.assignment;
        }

        public Trace2()
        {            
            this.id = G.NextLong(Globals.random, 1, long.MaxValue - 1);  //collision is extremely unlikely
            //maybe here put it into dictionary with weak values
            //but where does that dictionary live? In a databank, no?
            //or maybe only make the dictionary when about 
        }

        public Trace2 DeepClone()
        {
            Trace2 trace2 = new Trace2();  //also creates id
            trace2.assignment=this.assignment;
            trace2.bankAndVarnameWithFreq = this.bankAndVarnameWithFreq;
            trace2.filenameAndPathAndLine= this.filenameAndPathAndLine;
            trace2.periods = new List<GekkoTime>();
            foreach (GekkoTime t in this.periods) trace2.periods.Add(t);
            trace2.t1 = this.t1;
            trace2.t2 = this.t2;            
            if (this.precedents != null)
            {
                trace2.precedents = new List<Trace2>();
                foreach (Trace2 trace2Clone in this.precedents) trace2.precedents.Add(trace2Clone.DeepClone());
            }
            return trace2;
        }
    }
}
