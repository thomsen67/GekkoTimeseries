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
        public static readonly ScalarVal i6 = new ScalarVal(3d);
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
            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
                        
            IVariable ivTmpvar5 = O.Add(smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var), i6);

            if (true)
            {
                O.Assignment o0 = new O.Assignment();
                o0.opt_d = "yes";
                O.AssignmentHelper(smpl, ivTmpvar5, i6, o0);
            }

            O.Lookup(smpl, null, null, "x", null, ivTmpvar5, true, EVariableType.Var)
            ;


            //[[splitSTOP]]


        }
    }
}
