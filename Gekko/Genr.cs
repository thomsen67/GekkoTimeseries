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
        public static readonly ScalarVal i94 = new ScalarVal(2d);
        public static readonly ScalarVal i95 = new ScalarVal(2d);
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
            List m96 = null; try
            {
                m96 = new List();
                for (smpl.bankNumber = 0; smpl.bankNumber < 1; smpl.bankNumber++)
                {
                    m96.Add(Functions.movsum(O.Smpl(smpl, O.Add(smpl, i95, new ScalarVal(-1d))), smpl, Functions.movsum(O.Smpl(smpl, O.Add(smpl, i94, new ScalarVal(-1d))), smpl, O.Lookup(smpl, null, null, "xx", null, null, false), i94), i95));
                }
            }
            finally
            {
                smpl.bankNumber = 0;
            }
            O.Print(smpl, m96);

            //[[splitSTOP]]


        }
    }
}
