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
        public static readonly ScalarVal i34 = new ScalarVal(5d);
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

            IVariable ivTmpvar33 = O.IvConvertTo(EVariableType.Var, i34);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var), ivTmpvar33, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
            , new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
            )
            ;


            //[[splitSTOP]]


        }
    }
}
