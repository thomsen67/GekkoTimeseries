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
            o0.opt_source = @"<[code]>#i = a, b";


            Action assign_48 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar47 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("a"), null, new ScalarString("b"), null }));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "#i", null, ivTmpvar47, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
            };
            Func<bool> check_48 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar47 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("a"), null, new ScalarString("b"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar47.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "#i", null, ivTmpvar47, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_48, check_48, o0);

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);


            O.Copy o1 = new O.Copy();
            o1.type = @"ASTPLACEHOLDER";
            o1.names1 = O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("x").Concat(null, new ScalarString("[").Add(null, O.Lookup(smpl, null, null, "#i", null, null, new LookupSettings(), EVariableType.Var, null)).Add(null, new ScalarString("]"))), new ScalarString("c") }));
            o1.Exe();

            //[[commandEnd]]1
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
