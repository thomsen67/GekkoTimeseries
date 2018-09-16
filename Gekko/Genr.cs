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
            IVariable ivTmpvar53 = O.Hat(smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var, null), O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var, null));
            O.Lookup(smpl, null, null, "x", null, ivTmpvar53, true, EVariableType.Var, null)
            ;


            //[[splitSTOP]]


        }
    }
}
