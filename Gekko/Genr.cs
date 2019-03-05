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


        public static readonly ScalarVal i5 = new ScalarVal(1d);
        public static readonly ScalarVal i6 = new ScalarVal(2d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[commandSpecial]]0
            IVariable forloop_xe7dke6cj_4 = null;
            int counter7 = 0;
            for (O.IterateStart(ref forloop_xe7dke6cj_4, O.ListDefHelper(i5, null, i6, null)); O.IterateContinue(forloop_xe7dke6cj_4, O.ListDefHelper(i5, null, i6, null), null, null, ref counter7); O.IterateStep(forloop_xe7dke6cj_4, O.ListDefHelper(i5, null, i6, null), null, counter7))
            {
                ;
            };

            //[[commandEnd]]0



        }
    }
}
