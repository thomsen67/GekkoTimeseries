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
        public static readonly ScalarVal i31 = new ScalarVal(1d);
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

            IVariable ivTmpvar30 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, (O.scalarStringPercent).Add(smpl, (new ScalarString("v1")))), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var, null), (O.scalarStringPercent).Add(smpl, (new ScalarString("v1")))), i31);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null, true, EVariableType.Var, null), ivTmpvar30, null    , (O.scalarStringPercent).Add(smpl, (new ScalarString("v1"))))
            ;


            //[[splitSTOP]]


        }
    }
}
