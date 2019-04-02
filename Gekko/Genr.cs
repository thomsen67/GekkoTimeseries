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



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
            Functions.setdomains(smpl, null, null, O.Lookup(smpl, null, null, "a", null, null, new LookupSettings(), EVariableType.Var, null), O.ListDefHelper(O.HandleString(new ScalarString(@"#s")), null));

            //[[commandEnd]]0



        }
    }
}
