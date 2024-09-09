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
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o1 = new O.Assignment();
            o1.opt_trace = @"y=1";


            Globals.precedentsSeries = null;
            Action assign_34 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar32 = i33;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar32, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
            };
            Func<bool> check_34 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar32 = i33;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar32.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar32, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_34, check_34, o1, p);

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o2 = new O.Assignment();
            o2.opt_trace = @"y<2015 2024 dyn>=y[-1]+1";
            smpl.t0 = O.ConvertToDate(i38, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t1 = O.ConvertToDate(i38, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t2 = O.ConvertToDate(i39, O.GetDateChoices.FlexibleEnd);
            ;
            smpl.t3 = O.ConvertToDate(i39, O.GetDateChoices.FlexibleEnd);
            ;

            o2.opt_dyn = "yes";

            Globals.precedentsSeries = null;
            Action assign_40 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar35 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i36)
                ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "y", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i36)
                ), i37);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar35, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_40 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar35 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i36)
                ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "y", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i36)
                ), i37);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar35.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar35, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_40, check_40, o2, p);

            //[[commandEnd]]2
        }


        public static readonly ScalarVal i33 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i36 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i37 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i38 = new ScalarVal(2015d, 0);
        public static readonly ScalarVal i39 = new ScalarVal(2024d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);
        }
    }
}
