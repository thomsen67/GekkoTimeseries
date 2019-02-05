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
        public static void C0(GekkoSmpl smpl, P p) {
            //[[commandStart]]0
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Assignment o0 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar22 = null; // O.FunctionLookupNew2("mul")(smpl, p, new GekkoArg((smpl777) => O.FunctionLookupNew2("plus")(smpl777, p, new GekkoArg((smpl777777) => O.Lookup(smpl777777, null, null, "#m4", null, null, new LookupSettings(), EVariableType.Var, null), (smpl777777) => new ScalarString("#m4")), new GekkoArg((smpl777777) => i23, (smpl777777) => null)), (smpl777) => (new ScalarString("#m4")).Add(smpl777, new ScalarString("[")).Add(smpl777, O.FunctionLookupNew2("plus")(smpl777, p, new GekkoArg((smpl777777) => i23, (smpl777777) => null))).Add(smpl777, new ScalarString("]"))), new GekkoArg((smpl777) => i24, (smpl777) => null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "#m4", null, ivTmpvar22, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
            ;

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i23 = new ScalarVal(200d);
        public static readonly ScalarVal i24 = new ScalarVal(10000d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
