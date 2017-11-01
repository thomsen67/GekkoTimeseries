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
        public static readonly ScalarVal i113 = new ScalarVal(1d);
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
            Target115:
            List m114 = null; try
            {
                m114 = new List();
                for (smpl.bankNumber = 0; smpl.bankNumber < 1; smpl.bankNumber++)
                {
                    m114.Add(O.Indexer(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false), O.Lookup(smpl, null, null, "xx", null, null, false)), O.Negate(smpl, i113)
                    ));
                }
            }
            finally
            {
                smpl.bankNumber = 0;
            }
            O.Print(smpl, m114);
            if (smpl.HasError()) { O.TryNewSmpl(smpl); goto Target115; }

            //[[splitSTOP]]


        }
    }
}
