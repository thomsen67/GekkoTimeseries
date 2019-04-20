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
            o0.opt_source = @"<[code]>x{#i}=x{#i}+1";


            foreach (IVariable listloop_i12 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o0)))
            {
                Action check_15 = () =>
                {
                    O.Dynamic1(smpl);
                    O.Lookup(smpl, null, (new ScalarString("x")).Concat(smpl, listloop_i12), Globals.scalarValMissing, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                    ;
                    O.Dynamic5(smpl);
                };
                Action assign_15 = () =>
                {
                    O.AdjustT0(smpl, -2);
                    IVariable ivTmpvar13 = O.Add(smpl, O.Lookup(smpl, null, (new ScalarString("x")).Concat(smpl, listloop_i12), null, new LookupSettings(), EVariableType.Var, null), i14);
                    O.AdjustT0(smpl, 2);
                    if (O.Dynamic6(smpl)) return;
                    O.Lookup(smpl, null, (new ScalarString("x")).Concat(smpl, listloop_i12), ivTmpvar13, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                    ;
                };
                O.RunAssigmentMaybeDynamic(smpl, assign_15, check_15, o0);
            }

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i14 = new ScalarVal(1d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
