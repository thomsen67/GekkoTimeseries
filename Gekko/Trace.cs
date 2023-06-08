using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gekko
{
    /// <summary>
    /// Used for the .trace field of timeseries
    /// </summary>

    [ProtoContract]
    public class Trace
    {
        [ProtoMember(1)]
        public string mapOrBankName = null;

        [ProtoMember(2)]
        public string varnameWithFreq = null;

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

        public static void Walker(Trace trace, int d)
        {
            if (trace == null) return;
            //if (d >= 5) return;
            new Writeln("-- " + G.Blanks(d * 2) + trace.mapOrBankName + ":" + trace.varnameWithFreq);
            //Trace2 first = null;
            foreach (KeyValuePair<GekkoTime, Trace2> kvp in trace.storage)             
            {
                //if (first != null && kvp.Value == first) continue;
                //if (first == null) first = kvp.Value;
                Trace2 child = kvp.Value;
                new Writeln("-- " + G.Blanks(d * 2) + kvp.Key.ToString() + ": " + child.statement);
                if (child.precedents != null)
                {
                    foreach (Trace childTrace in child.precedents)
                    {
                        Walker(childTrace, d + 1);
                    }
                }
            }
        }
    }

    [ProtoContract]
    public class Trace2
    {
        [ProtoMember(1)]
        public string statement = null;

        [ProtoMember(2)]
        public List<Trace> precedents = null;

        public Trace2()
        {
            //just for protobuf
        }
    }
}
