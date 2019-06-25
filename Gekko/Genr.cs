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
        public static void C0(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]0
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            O.Assignment o0 = new O.Assignment();
            o0.opt_source = @"<[code]>pch(x) <q> =2";
            smpl.t0 = Globals.globalPeriodStart;
            smpl.t1 = Globals.globalPeriodStart;
            smpl.t2 = Globals.globalPeriodEnd;
            smpl.t3 = Globals.globalPeriodEnd;

            o0.opt_q = "yes";




            Action assign_55 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar53 = i54;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar53, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)                
                
                ;
            };
            Func<bool> check_55 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar53 = i54;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar53.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_55, check_55, o0);

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i54 = new ScalarVal(2d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
