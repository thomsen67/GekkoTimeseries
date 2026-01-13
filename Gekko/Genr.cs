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
        public static void C0(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_1)
        {
            IVariable forloop_xe7dke6cj_1 = xforloop_xe7dke6cj_1;

            //[[commandStart]]1
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            O.Tell o1 = new O.Tell();
            o1.s = forloop_xe7dke6cj_1;
            o1.Exe();

            //[[commandEnd]]1
            xforloop_xe7dke6cj_1 = forloop_xe7dke6cj_1;

        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            p.SetStack(@"¤1");


            //[[commandSpecial]]0
            IVariable xcodeStart3 = O.FunctionLookupNew2(p, null, "f")(smpl, p, false, null, null);
            IVariable xcodeEnd24 = O.FunctionLookupNew2(p, null, "g")(smpl, p, false, null, null);
            IVariable xcodeStep5 = O.FunctionLookupNew2(p, null, "g")(smpl, p, false, null, null);
            IVariable forloop_xe7dke6cj_1 = null;
            int counter2 = 0;
            bool years6 = O.LoopYears("string", O.ELoopType.ForTo, xcodeStart3, xcodeEnd24); for (O.IterateStart(years6, O.ELoopType.ForTo, ref forloop_xe7dke6cj_1, xcodeStart3); O.IterateContinue(years6, O.ELoopType.ForTo, forloop_xe7dke6cj_1, xcodeStart3, xcodeEnd24, xcodeStep5, ref counter2); O.IterateStep(years6, O.ELoopType.ForTo, ref forloop_xe7dke6cj_1, xcodeStart3, xcodeStep5, counter2))
            {
                ;
                O.TypeCheck_string(forloop_xe7dke6cj_1, 0);

                C0(smpl, p, ref forloop_xe7dke6cj_1);

            };

            //[[commandEnd]]0



        }
    }
}
