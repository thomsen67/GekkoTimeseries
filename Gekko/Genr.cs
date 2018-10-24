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
        public static void ClearTS(P p) {
        }
        public static void ClearScalar(P p) {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            Functions.setdomains(smpl, O.Lookup(smpl, null, null, "px_s", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(), EVariableType.Var, null));
        
//[[splitSTOP]]


        }
    }
}
