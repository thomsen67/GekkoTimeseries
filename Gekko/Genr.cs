using System;
using System.Collections.Generic;
using System.Text;
using
System.Windows.Forms;
using System.Drawing;
using Gekko.Parser;
namespace Gekko
{
    public class
    TranslatedCode
    {
        public static GekkoTime globalGekkoTimeIterator = GekkoTime.tNull;
        public static
        int labelCounter;
        public static void C0(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]0
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            O.Copy o0 = new
            O.Copy();
            o0.type = @"ASTPLACEHOLDER"; o0.t1 = GekkoTime.tNull;
            o0.t2 =
            GekkoTime.tNull;

            o0.opt_frombank = O.ConvertToString((new
            ScalarString("work")));

            o0.opt_tobank = O.ConvertToString((new
            ScalarString("ref")));



            o0.names1 = O.FlattenIVariablesSeq(false, new List(new List<IVariable>
{O.Lookup(smpl, null, null, "#x", null, null, new  LookupSettings(), EVariableType.Var,
null)}));
            o0.gekkocode = @"copy <frombank=work tobank=ref> {#x} to *";
            o0.p = p;
            o0.names2 =
            O.FlattenIVariablesSeq(false, new List(new List<IVariable> {new
ScalarString("*")}));
            o0.Exe();

            //[[commandEnd]]0
        }



        public static void CodeLines(P
        p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
