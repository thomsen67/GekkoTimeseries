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

            O.Copy o0 = new O.Copy();
            o0.type = @"ASTPLACEHOLDER";
            o0.names1 = new List(O.ExplodeIvariables(new List(new List<IVariable> { (new ScalarString(ScalarString.SubstituteScalarsInString(@"x*", true, false))) })));
            o0.Exe();


            //[[splitSTOP]]


        }
    }
}
