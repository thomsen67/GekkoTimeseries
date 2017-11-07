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
            List m857 = null; try
            {
                m857 = new List();
                for (smpl.bankNumber = 0; smpl.bankNumber < 1; smpl.bankNumber++)
                {
                    IVariable temp = (O.Lookup(smpl, null, null, "%s", null, null, false));
                    m857.Add(O.Lookup(smpl, null, temp, null, false));
                }
            }
            finally
            {
                smpl.bankNumber = 0;
            }
            O.Print(smpl, m857);

            //[[splitSTOP]]


        }
    }
}
