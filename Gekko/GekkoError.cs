using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    public class GekkoError
    {
        public int t1Problem = 0; //always 0 or positive
        public int t2Problem = 0; //always 0 or positive
        
        public GekkoError(int t1Problem, int t2Problem)
        {
            G.WritelnGray("*** ERROR: TooSmallTooLarge: " + t1Problem + " " + t2Problem);
            G.Writeln2("*** ERROR: Unfortunately, you ran into a lag problem in Gekko 3.0.");
            G.Writeln("    This typically arises in expressions like x1[-1] + x2, where timeseries with lags are");
            G.Writeln("    involved. You may try to use an intermediate variable which will often provide a work-around");
            G.Writeln("    for the problem, for instance defining y = x1[-1] + x2, and then use y instead of the ");
            G.Writeln("    expression. For more on this problem, see this page: https://t-t.dk/gekko/the-lag-problem");
            throw new GekkoException();
            this.t1Problem = t1Problem;
            this.t2Problem = t2Problem;
        }               
    }
}

