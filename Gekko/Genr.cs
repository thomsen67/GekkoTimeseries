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
        public static readonly ScalarVal i20 = new ScalarVal(1d);
        public static readonly ScalarVal i22 = new ScalarVal(5d);
        public static readonly ScalarVal i23 = new ScalarVal(5d);
        public static readonly ScalarVal i24 = new ScalarVal(2d);
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

            //[[splitSTART]]
            p.SetText(@"¤0"); O.InitSmpl(smpl);

            IVariable ivTmpvar19 = O.IvConvertTo(EVariableType.Var, i20);
            O.Lookup(smpl, null, null, "xx", null, ivTmpvar19, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl);

            IVariable ivTmpvar21 = O.IvConvertTo(EVariableType.Var, i24);
            O.DollarLookup(O.Equals(smpl, i22, i23)
            , smpl, null, null, "xx", null, ivTmpvar21, false, EVariableType.Var)
            ;


            //[[splitSTOP]]


        }
    }
}
