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
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o0 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar22 = Functions.extend(smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var, null), O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "#m", null, ivTmpvar22, true, EVariableType.Var, o0)
            ;


            //[[splitSTOP]]


        }
    }
}
