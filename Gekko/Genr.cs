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


            O.FunctionLookupNew4("procedure___fremn")(smpl, p, false, null, null, new GekkoArg((spml1406) => O.Lookup(spml1406, null, null, "gfy", "q", null, new LookupSettings(), EVariableType.Var, null), (spml1406) => new ScalarString("gfy!q")), new GekkoArg((spml1407) => i1405, (spml1407) => null));

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i1405 = new ScalarVal(1d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
