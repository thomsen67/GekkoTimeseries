using System;
using System.Collections.Generic;
using System.Text;

namespace Gekko.Parser
{
    public class AST
    {
        public static void AST_Prt(List<string> input)
        {            
            G.Writeln2("PRTSTART");
            foreach (string s in input)
            {
                G.Writeln("--> " + s);
            }
            G.Writeln("PRTEND");
        }

        public static string AST_GetName(string s)
        {
            return s;
        }
    }
}
