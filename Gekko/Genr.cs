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
        public static void C0(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_3)
        {
            IVariable forloop_xe7dke6cj_3 = xforloop_xe7dke6cj_3;

            //[[commandStart]]1
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            O.Tell o1 = new O.Tell();
            o1.s = forloop_xe7dke6cj_3;
            o1.Exe();

            //[[commandEnd]]1
            xforloop_xe7dke6cj_3 = forloop_xe7dke6cj_3;

        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            p.SetStack(@"¤1");


            //[[commandSpecial]]0
            IVariable forloop_xe7dke6cj_3 = null;
            int counter4 = 0;
            bool years5 = O.LoopYears("string", O.ELoopType.List, O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(), EVariableType.Var, null), null); for (O.IterateStart(years5, O.ELoopType.List, ref forloop_xe7dke6cj_3, O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(), EVariableType.Var, null)); O.IterateContinue(years5, O.ELoopType.List, forloop_xe7dke6cj_3, O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(), EVariableType.Var, null), null, null, ref counter4); O.IterateStep(years5, O.ELoopType.List, ref forloop_xe7dke6cj_3, O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(), EVariableType.Var, null), null, counter4))
            {
                ;
                O.TypeCheck_string(forloop_xe7dke6cj_3, 0);

                C0(smpl, p, ref forloop_xe7dke6cj_3);

            };

            //[[commandEnd]]0



        }
    }
}
