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

            O.Library o0 = new O.Library();
            o0.p = p;
            o0.fileName = O.ReplaceSlash((new ScalarString("c")).Add(smpl, new ScalarString(":\\")).Add(smpl, (new ScalarString("Thomas"))).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("Gekko"))).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("regres"))).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("Libraries"))).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("p1"))));
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
