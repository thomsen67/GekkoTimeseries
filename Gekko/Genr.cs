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
            //[[commandStart]]1
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);


            O.Index o1 = new O.Index();
            o1.opt_mute = "yes";

            o1.names2 = O.ExplodeIvariablesSeqFor(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("mmm")) }));
            o1.type = @"ASTPLACEHOLDER"; o1.names1 = O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("*") }));
            o1.Exe();

            //[[commandEnd]]1
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[commandSpecial]]0
            IVariable forloop_xe7dke6cj_310 = null;
            int counter311 = 0;
            bool years312 = O.LoopYears("string", O.ELoopType.List, O.ExplodeIvariablesSeqFor(true, new List(new List<IVariable> { new ScalarString("a"), null, new ScalarString("b"), null, new ScalarString("c"), null })), null); for (O.IterateStart(years312, O.ELoopType.List, ref forloop_xe7dke6cj_310, O.ExplodeIvariablesSeqFor(true, new List(new List<IVariable> { new ScalarString("a"), null, new ScalarString("b"), null, new ScalarString("c"), null }))); O.IterateContinue(years312, O.ELoopType.List, forloop_xe7dke6cj_310, O.ExplodeIvariablesSeqFor(true, new List(new List<IVariable> { new ScalarString("a"), null, new ScalarString("b"), null, new ScalarString("c"), null })), null, null, ref counter311); O.IterateStep(years312, O.ELoopType.List, ref forloop_xe7dke6cj_310, O.ExplodeIvariablesSeqFor(true, new List(new List<IVariable> { new ScalarString("a"), null, new ScalarString("b"), null, new ScalarString("c"), null })), null, counter311))
            {
                ;
                O.TypeCheck_string(forloop_xe7dke6cj_310, 0);

                C0(smpl, p);

            };

            //[[commandEnd]]0



        }
    }
}
