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
            o0.opt_source = @"<[code]>y4[#i] $ (#i.val() > 10) = 1";


            foreach (IVariable listloop_i23 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o0)))
            {
                Action assign_27 = () =>
                {
                    O.AdjustT0(smpl, -2);
                    IVariable ivTmpvar24 = i26;
                    O.AdjustT0(smpl, 2);
                    O.DollarIndexerSetData(O.StrictlyLargerThan(smpl, Functions.val(smpl, null, null, O.Lookup(smpl, null, null, "#i", null, null, new LookupSettings(), EVariableType.Var, null)), i25)
                    , smpl, O.Lookup(smpl, null, null, "y4", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar24, o0, listloop_i23)
                    ;
                };
                Func<bool> check_27 = () =>
                {
                    O.AdjustT0(smpl, -2);
                    IVariable ivTmpvar24 = i26;
                    O.AdjustT0(smpl, 2);
                    if (ivTmpvar24.Type() != EVariableType.Series) return false;
                    O.Dynamic1(smpl);
                    O.DollarIndexerSetData(O.StrictlyLargerThan(smpl, Functions.val(smpl, null, null, O.Lookup(smpl, null, null, "#i", null, null, new LookupSettings(), EVariableType.Var, null)), i25)
                    , smpl, O.Lookup(smpl, null, null, "y4", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar24, o0, listloop_i23)
                    ;
                    return O.Dynamic2(smpl);
                };
                O.RunAssigmentMaybeDynamic(smpl, assign_27, check_27, o0);
            }

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i25 = new ScalarVal(10d, 0);
        public static readonly ScalarVal i26 = new ScalarVal(1d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
