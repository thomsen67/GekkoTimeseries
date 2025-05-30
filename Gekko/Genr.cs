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
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            O.Rebase o0 = new O.Rebase();
            o0.names = O.FlattenIVariablesSeq(false, new List(new List<IVariable> { new ScalarString("x") }));
            o0.t1 = O.ConvertToDate(i1, O.GetDateChoices.Strict);
            o0.gekkocode = @"rebase x 2000";
            o0.p = p; o0.Exe();

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i1 = new ScalarVal(2000d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
