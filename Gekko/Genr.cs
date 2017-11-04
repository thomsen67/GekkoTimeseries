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
        public static readonly ScalarVal i165 = new ScalarVal(0d);
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
            p.SetText(@"¤1"); O.InitSmpl(smpl);
            List m166 = null; try
            {
                m166 = new List();
                for (smpl.bankNumber = 0; smpl.bankNumber < 1; smpl.bankNumber++)
                {
                    m166.Add(Functions.pch(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false), i165)));
                }
            }
            finally
            {
                smpl.bankNumber = 0;
            }
            O.Print(smpl, m166);

            //[[splitSTOP]]


        }
    }
}
