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



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[commandSpecial]]0
            IVariable forloop_xe7dke6cj_5121 = null;
            int counter5122 = 0;
            for (O.IterateStart(ref forloop_xe7dke6cj_5121, O.ExplodeIvariablesSeq(new List(new List<IVariable> { new ScalarString("a"), O.scalarStringPercent.Add(null, new ScalarString("b")) }))); O.IterateContinue(forloop_xe7dke6cj_5121, O.ExplodeIvariablesSeq(new List(new List<IVariable> { new ScalarString("a"), O.scalarStringPercent.Add(null, new ScalarString("b")) })), null, null, ref counter5122); O.IterateStep(forloop_xe7dke6cj_5121, O.ExplodeIvariablesSeq(new List(new List<IVariable> { new ScalarString("a"), O.scalarStringPercent.Add(null, new ScalarString("b")) })), null, counter5122))
            {
                ;
            };

            //[[commandEnd]]0



        }
    }
}
