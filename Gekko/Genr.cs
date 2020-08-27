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


            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe(smpl);

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Time o1 = new O.Time();
            o1.t1 = O.ConvertToDate(i1, O.GetDateChoices.FlexibleStart);
            ;
            o1.t2 = O.ConvertToDate(i2, O.GetDateChoices.FlexibleEnd);
            ;

            o1.Exe();

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            O.Assignment o2 = new O.Assignment();
            o2.opt_source = @"<[code]>x=series(1)";


            Globals.precedentsSeries = null;
            Action assign_5 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3 = Functions.series(smpl, null, null, i4);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_5 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3 = Functions.series(smpl, null, null, i4);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar3.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_5, check_5, o2);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            O.Assignment o3 = new O.Assignment();
            o3.opt_source = @"<[code]>x[a]=1,2,3";


            Globals.precedentsSeries = null;
            Action assign_7 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar6 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("1"), null, new ScalarString("2"), null, new ScalarString("3"), null }));
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar6, o3, new ScalarString("a"))
                ;
            };
            Func<bool> check_7 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar6 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("1"), null, new ScalarString("2"), null, new ScalarString("3"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar6.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar6, o3, new ScalarString("a"))
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_7, check_7, o3);

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Time o4 = new O.Time();
            o4.t1 = O.ConvertToDate(i8, O.GetDateChoices.FlexibleStart);
            ;
            o4.t2 = O.ConvertToDate(i9, O.GetDateChoices.FlexibleEnd);
            ;

            o4.Exe();

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            O.Assignment o5 = new O.Assignment();
            o5.opt_source = @"<[code]>x[a] <dyn> = x[a][-1] + 1000";
            smpl.t0 = Globals.globalPeriodStart;
            smpl.t1 = Globals.globalPeriodStart;
            smpl.t2 = Globals.globalPeriodEnd;
            smpl.t3 = Globals.globalPeriodEnd;

            o5.opt_dyn = "yes";

            Globals.precedentsSeries = null;
            Action assign_13 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar10 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i11)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("a")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a")), O.Negate(smpl, i11)
                ), i12);
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar10, o5, new ScalarString("a"))
                ;
            };
            Func<bool> check_13 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar10 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i11)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("a")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a")), O.Negate(smpl, i11)
                ), i12);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar10.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar10, o5, new ScalarString("a"))
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_13, check_13, o5);

            //[[commandEnd]]5
        }


        public static readonly ScalarVal i1 = new ScalarVal(2001d, 0);
        public static readonly ScalarVal i2 = new ScalarVal(2003d, 0);
        public static readonly ScalarVal i4 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i8 = new ScalarVal(2002d, 0);
        public static readonly ScalarVal i9 = new ScalarVal(2003d, 0);
        public static readonly ScalarVal i11 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i12 = new ScalarVal(1000d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);

        }
    }
}
