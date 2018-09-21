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
        public static readonly ScalarVal i59 = new ScalarVal(100d);
        public static readonly ScalarVal i60 = new ScalarVal(10d);
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
            IVariable ivTmpvar58 = null;// O.Dollar(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var, null), i59), O.StrictlySmallerThan(smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var, null), i60)
            //);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "y1", null, ivTmpvar58, true, EVariableType.Var, o0)
            ;


            //[[splitSTOP]]


        }
    }
}
