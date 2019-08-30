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

            O.Analyze o0 = new O.Analyze();
            o0.x = new List<IVariable>();
            o0.x.Add(i7);
            o0.x.Add(i8);
            o0.expressionsText = new List<string>();
            o0.expressionsText.Add(@"1");
            o0.expressionsText.Add(@"2");
            o0.Exe();

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i7 = new ScalarVal(1d);
        public static readonly ScalarVal i8 = new ScalarVal(2d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
