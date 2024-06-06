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
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe(smpl);

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetStack(@"¤2"); O.InitSmpl(smpl, p);

            O.Create o1 = new O.Create();
            o1.names = O.FlattenIVariablesSeq(false, new List(new List<IVariable> { new ScalarString("x") }));
            o1.p = p;
            o1.Exe();

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetStack(@"¤3"); O.InitSmpl(smpl, p);
            O.Assignment o2 = new O.Assignment();
            o2.opt_trace = @"x<dyn>=x.1+1";
            smpl.t0 = Globals.globalPeriodStart;
            smpl.t1 = Globals.globalPeriodStart;
            smpl.t2 = Globals.globalPeriodEnd;
            smpl.t3 = Globals.globalPeriodEnd;

            o2.opt_dyn = "yes";




            Globals.precedentsSeries = null;
            Action assign_22 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar19 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.Dot, i20), smpl, O.EIndexerType.Dot, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), i20), i21);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar19, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_22 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar19 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.Dot, i20), smpl, O.EIndexerType.Dot, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), i20), i21);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar19.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar19, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_22, check_22, o2, p);

            //[[commandEnd]]2
        }


        public static readonly ScalarVal i20 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i21 = new ScalarVal(1d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
