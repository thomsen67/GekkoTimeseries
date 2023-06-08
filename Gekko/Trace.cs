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
    public class Trace
    {
        public Dictionary<GekkoTime, Trace2> storage = new Dictionary<GekkoTime, Trace2>();
    }

    public class Trace2
    {
        public string statement = null;
        public List<Trace> precedents = null;
    }
}
