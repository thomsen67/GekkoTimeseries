using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Gekko.Parser;
namespace Gekko
{
    public class TranslatedCode
    {
        public static GekkoTime globalGekkoTimeIterator = GekkoTime.tNull;
        public static int labelCounter;
        public static void C0(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]0
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            O.Decomp o0 = new O.Decomp();
            o0.t1 = Globals.globalPeriodStart;
            o0.t2 = Globals.globalPeriodEnd;

            o0.smplForFunc = smpl;
                        
            o0.expression = () => O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("a")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("b")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("b")));
            

            o0.Exe();

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
