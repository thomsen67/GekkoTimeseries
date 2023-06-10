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
        [ProtoMember(100)]
        public X x = null;

        [ProtoMember(1)]
        public string mapOrBankName = null;  //What is this used for ???

        [ProtoMember(2)]
        public string varnameWithFreq = null;  //What is this used for ???

        [ProtoMember(3)]
        public Dictionary<GekkoTime, Trace2> storage = new Dictionary<GekkoTime, Trace2>();
        
        public Trace()
        {
            //just for protobuf
        }

        public Trace(string bankOrMapName, string s) 
        {
            this.varnameWithFreq = s;
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
        public string bankAndVarnameWithFreq = null;

        [ProtoMember(2)]
        public DateTime stamp = DateTime.MinValue;

        [ProtoMember(3)]
        public string filenameAndPathAndLine = null;

        [ProtoMember(5)]
        public string assignment = null;

        [ProtoMember(6)]
        public List<Trace2> precedents = null;

        public Trace2()
        {
            //just for protobuf
        }
    }
}
