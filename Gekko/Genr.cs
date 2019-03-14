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
        public static void Evalcode1()
        {
            //o0.expression = (smpl) => O.Lookup(smpl, null, null, "enl", null, null, new LookupSettings(), EVariableType.Var, null);
        }

        public static GekkoTime globalGekkoTimeIterator = GekkoTime.tNull;
        public static int labelCounter;
        public static void C0(GekkoSmpl smpl, P p) {
            //[[commandStart]]0
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            

            O.Decomp o0 = new O.Decomp();
            o0.label = @"enl";

            o0.smplForFunc = smpl;
            Evalcode1();

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
