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
            this.t1Problem = t1Problem;
            this.t2Problem = t2Problem;
        }               
    }
}

