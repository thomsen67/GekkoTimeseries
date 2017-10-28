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
        
        public GekkoError()
        {
            G.Writeln2("*** ERROR: Internal error #7329843");
            throw new GekkoException();
        }               
    }
}

