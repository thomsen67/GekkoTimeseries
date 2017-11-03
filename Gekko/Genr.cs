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
        public static readonly ScalarVal i111 = new ScalarVal(1d);
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
            //Target112:
            IVariable ivTmpvar110 = O.Indexer(O.Indexer2(smpl, O.Negate(smpl, i111)), smpl, O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false), O.Lookup(smpl, null, null, "xx", null, null, false)), O.Negate(smpl, i111)
            );
            O.Lookup(smpl, null, null, "xx", null, ivTmpvar110, true)
            ;

            //if (smpl.HasError()) { O.TryNewSmpl(smpl); goto Target112; }

            //[[splitSTOP]]


        }
    }
}
