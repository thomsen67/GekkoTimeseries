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
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Hei fra remote", true, false))), false);
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);

            Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Hej fra remote", true, false))), false);
            p.SetText(@"¤3"); O.InitSmpl(smpl, p);

            O.Sys o2 = new O.Sys();
            o2.s = new ScalarString(ScalarString.SubstituteScalarsInString(@"dir", true, false));
            o2.Exe();


            //[[splitSTOP]]


        }
    }
}
