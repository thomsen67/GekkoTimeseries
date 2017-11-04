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
        public static readonly ScalarVal i81 = new ScalarVal(1d);
        public static readonly ScalarVal i82 = new ScalarVal(1d);
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
            List m83 = null; try
            {
                m83 = new List();
                for (smpl.bankNumber = 0; smpl.bankNumber < 1; smpl.bankNumber++)
                {
                    m83.Add(O.Indexer(O.Indexer2(smpl, O.Negate(smpl, i82)), smpl, O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false), i81), O.Negate(smpl, i82)
                    ));
                }
            }
            finally
            {
                smpl.bankNumber = 0;
            }
            O.Print(smpl, m83);

            //[[splitSTOP]]


        }
    }
}
