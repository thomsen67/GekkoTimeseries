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
public static readonly ScalarVal i8 = new ScalarVal(1d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o0 = new O.Assignment();
            smpl.t0 = Globals.globalPeriodStart;
            smpl.t1 = Globals.globalPeriodStart;
            smpl.t2 = Globals.globalPeriodEnd;
            smpl.t3 = Globals.globalPeriodEnd;

            o0.opt_d = "yes";


            smpl.t1 = smpl.t1.Add(-1);
            smpl.t0 = smpl.t0.Add(-1);
            IVariable ivTmpvar7 = O.Add(smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var, null), i8);
            smpl.t1 = smpl.t1.Add(1);
            smpl.t0 = smpl.t0.Add(1);
            O.Lookup(smpl, null, null, "y", null, ivTmpvar7, true, EVariableType.Var, o0)
;


            //[[splitSTOP]]


        }
}
}
