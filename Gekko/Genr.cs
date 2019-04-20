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
            o0.opt_source = @"<[code]>y = x";


            Action assign_23 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar22 = O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null);
                O.AdjustT0(smpl, 2);
                if (O.Dynamic6(smpl)) return;
                O.Lookup(smpl, null, null, "y", null, ivTmpvar22, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
            };
            Func<bool> check_23 = () =>
            {
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y", null, Globals.scalarValMissing, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
                O.Dynamic5(smpl);
                return O.Dynamic7(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_23, check_23, o0);

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
