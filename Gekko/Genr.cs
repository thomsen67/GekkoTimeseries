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
            o0.opt_source = @"<[code]>x=1";


            Action assign_3 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar1 = i2;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
            };
            Func<bool> check_3 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar1 = i2;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar1.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_3, check_3, o0);

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            O.Assignment o1 = new O.Assignment();
            o1.opt_source = @"<[code]>y=pch(x)";


            Action assign_5 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar4 = Functions.pch(O.Smpl(smpl, -1), smpl, null, null, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar4, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
            };
            Func<bool> check_5 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar4 = Functions.pch(O.Smpl(smpl, -1), smpl, null, null, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar4.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar4, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_5, check_5, o1);

            //[[commandEnd]]1
        }


        public static readonly ScalarVal i2 = new ScalarVal(1d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
