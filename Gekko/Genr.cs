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
            o0.opt_source = @"<[code]>y = log(x) + x";


            Action assign_60 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar59 = O.Add(smpl, Functions.log(smpl, null, null, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar59, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
            };
            Func<bool> check_60 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar59 = O.Add(smpl, Functions.log(smpl, null, null, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar59.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar59, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_60, check_60, o0);

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
