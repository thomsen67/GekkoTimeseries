using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    public class GekkoError
    {
        public int uoverflow = 0;

        public GekkoError()
        {
            G.Writeln2("*** ERROR: Internal error #7329843");
            throw new GekkoException();
        }

        public GekkoError(int uoverflow)
        {
            G.Writeln2("+++ UOVERFLOW constructed: " + uoverflow);
            this.uoverflow = uoverflow;
        }        
    }
}

