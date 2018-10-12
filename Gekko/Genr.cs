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
        public static void ClearTS(P p) {
        }
        public static void ClearScalar(P p) {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o0 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar24 = null;
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "#m", null, ivTmpvar24, O.ELookupType.LeftHandSide, EVariableType.Var, o0)
            ;


            //[[splitSTOP]]


        }
    }
}
