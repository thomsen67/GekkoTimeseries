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

            O.Tell(O.ConvertToString(forloop_xe7dke6cj_1), false);
            //[[commandEnd]]1
            xforloop_xe7dke6cj_1 = forloop_xe7dke6cj_1;

        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            p.SetStack(@"¤1");


            //[[commandSpecial]]0
            IVariable forloop_xe7dke6cj_1 = null;
            int counter2 = 0;
            bool years3 = O.LoopYears("val", O.ELoopType.List, O.FlattenIVariablesSeqFor(true, new List(new List<IVariable> { new ScalarString("1"), null })), null); for (O.IterateStart(years3, O.ELoopType.List, ref forloop_xe7dke6cj_1, O.FlattenIVariablesSeqFor(true, new List(new List<IVariable> { new ScalarString("1"), null }))); O.IterateContinue(years3, O.ELoopType.List, forloop_xe7dke6cj_1, O.FlattenIVariablesSeqFor(true, new List(new List<IVariable> { new ScalarString("1"), null })), null, null, ref counter2); O.IterateStep(years3, O.ELoopType.List, ref forloop_xe7dke6cj_1, O.FlattenIVariablesSeqFor(true, new List(new List<IVariable> { new ScalarString("1"), null })), null, counter2))
            {
                ;
                O.TypeCheck_val(forloop_xe7dke6cj_1, 0);

                C0(smpl, p, ref forloop_xe7dke6cj_1);

            };

            //[[commandEnd]]0



        }
    }
}
