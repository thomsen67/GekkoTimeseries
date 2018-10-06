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
        public static readonly ScalarVal i39 = new ScalarVal(1d);
        public static readonly ScalarVal i40 = new ScalarVal(1000d);
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
            IVariable ivTmpvar38 = i40;
            O.AdjustT0(smpl, 1);
            IVariable xx = O.Lookup(smpl, null, null, "#m", null, null, O.ELookupType.RightHandSide, EVariableType.Var, null);
            //O.Lookup(smpl, null, (O.IndexerSetData(smpl, xx, ivTmpvar38, o0, i39)), ivTmpvar38, O.ELookupType.LeftHandSide, EVariableType.Var, o0)
            ;


            //[[splitSTOP]]


        }
    }
}
