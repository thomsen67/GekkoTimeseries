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
        public static readonly ScalarVal i2 = new ScalarVal(2002d);
        public static readonly ScalarVal i3 = new ScalarVal(100d);
        public static void ClearTS(P p) {
        }
        public static void ClearScalar(P p) {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            //IVariable ivTmpvar1 = O.IvConvertTo(EVariableType.Var, i3);
            //O.IndexerSetData(smpl, O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
            //, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
            //), smpl, O.Lookup(smpl, null, null, "xx2", null, null, true, EVariableType.Var), , ), ivTmpvar1, i2
            //)
            //;


            //[[splitSTOP]]


        }
    }
}
