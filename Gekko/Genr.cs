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
            o1.t1 = O.ConvertToDate(i15, O.GetDateChoices.FlexibleStart);
            ;
            o1.t2 = O.ConvertToDate(i16, O.GetDateChoices.FlexibleEnd);
            ;

            o1.Exe();

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);

            O.Assignment o2 = new O.Assignment();
            o2.opt_source = @"<[code]>#i = ('10', '11')";


            Action assign_18 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar17 = O.ListDefHelper(O.HandleString(new ScalarString(@"10")), null, O.HandleString(new ScalarString(@"11")), null);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "#i", null, ivTmpvar17, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_18 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar17 = O.ListDefHelper(O.HandleString(new ScalarString(@"10")), null, O.HandleString(new ScalarString(@"11")), null);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar17.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "#i", null, ivTmpvar17, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_18, check_18, o2);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤3"); O.InitSmpl(smpl, p);

            O.Assignment o3 = new O.Assignment();
            o3.opt_source = @"<[code]>#i0 = ('11',)";


            Action assign_20 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar19 = O.ListDefHelper(O.HandleString(new ScalarString(@"11")), null);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "#i0", null, ivTmpvar19, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
            };
            Func<bool> check_20 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar19 = O.ListDefHelper(O.HandleString(new ScalarString(@"11")), null);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar19.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "#i0", null, ivTmpvar19, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_20, check_20, o3);

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);

            O.Assignment o4 = new O.Assignment();
            o4.opt_source = @"<[code]>o = series(1)";


            Action assign_23 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar21 = Functions.series(smpl, null, null, i22);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "o", null, ivTmpvar21, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
            };
            Func<bool> check_23 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar21 = Functions.series(smpl, null, null, i22);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar21.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "o", null, ivTmpvar21, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_23, check_23, o4);

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤5"); O.InitSmpl(smpl, p);

            O.Assignment o5 = new O.Assignment();
            o5.opt_source = @"<[code]>o[11] = timeless(1)";


            Action assign_27 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar24 = Functions.timeless(smpl, null, null, i26);
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "o", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar24, o5, i25
                )
                ;
            };
            Func<bool> check_27 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar24 = Functions.timeless(smpl, null, null, i26);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar24.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "o", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar24, o5, i25
                )
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_27, check_27, o5);

            //[[commandEnd]]5


            //[[commandStart]]6
            p.SetText(@"¤6"); O.InitSmpl(smpl, p);

            O.Assignment o6 = new O.Assignment();
            o6.opt_source = @"<[code]>x = series(1)";


            Action assign_30 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar28 = Functions.series(smpl, null, null, i29);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar28, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o6)
                ;
            };
            Func<bool> check_30 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar28 = Functions.series(smpl, null, null, i29);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar28.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar28, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o6)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_30, check_30, o6);

            //[[commandEnd]]6


            //[[commandStart]]7
            p.SetText(@"¤7"); O.InitSmpl(smpl, p);

            O.Assignment o7 = new O.Assignment();
            o7.opt_source = @"<[code]>x[#i-1] = #i.val()-5";


            foreach (IVariable listloop_i13 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o7)))
            {
                Action assign_34 = () =>
                {
                    O.AdjustT0(smpl, -2);
                    IVariable ivTmpvar31 = O.Subtract(smpl, Functions.val(smpl, null, null, listloop_i13), i33);
                    O.AdjustT0(smpl, 2);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar31, o7, O.AddSpecial(smpl, listloop_i13, i32, true))
                    ;
                };
                Func<bool> check_34 = () =>
                {
                    O.AdjustT0(smpl, -2);
                    IVariable ivTmpvar31 = O.Subtract(smpl, Functions.val(smpl, null, null, listloop_i13), i33);
                    O.AdjustT0(smpl, 2);
                    if (ivTmpvar31.Type() != EVariableType.Series) return false;
                    O.Dynamic1(smpl);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar31, o7, O.AddSpecial(smpl, listloop_i13, i32, true))
                    ;
                    return O.Dynamic2(smpl);
                };
                O.RunAssigmentMaybeDynamic(smpl, assign_34, check_34, o7);
            }

            //[[commandEnd]]7


            //[[commandStart]]8
            p.SetText(@"¤8"); O.InitSmpl(smpl, p);

            O.Assignment o8 = new O.Assignment();
            o8.opt_source = @"<[code]>y6 = series(1)";


            Action assign_37 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar35 = Functions.series(smpl, null, null, i36);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y6", null, ivTmpvar35, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o8)
                ;
            };
            Func<bool> check_37 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar35 = Functions.series(smpl, null, null, i36);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar35.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y6", null, ivTmpvar35, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o8)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_37, check_37, o8);

            //[[commandEnd]]8
        }
        public static void C1(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]10
            p.SetText(@"¤10"); O.InitSmpl(smpl, p);

            O.Assignment o10 = new O.Assignment();
            o10.opt_source = @"<[code]>y6[#i+1] $ (#i.val() > 10 and o[#i]) = 4";


            foreach (IVariable listloop_i14 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o10)))
            {
                Action assign_42 = () =>
                {
                    O.AdjustT0(smpl, -2);
                    IVariable ivTmpvar38 = i41;
                    O.AdjustT0(smpl, 2);
                    O.DollarIndexerSetData(O.LogicalAnd(smpl, O.StrictlyLargerThan(smpl, Functions.val(smpl, null, null, listloop_i14), i40), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i14), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "o", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i14))
                    , smpl, O.Lookup(smpl, null, null, "y6", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar38, o10, O.AddSpecial(smpl, listloop_i14, i39, false))
                    ;
                };
                Func<bool> check_42 = () =>
                {
                    O.AdjustT0(smpl, -2);
                    IVariable ivTmpvar38 = i41;
                    O.AdjustT0(smpl, 2);
                    if (ivTmpvar38.Type() != EVariableType.Series) return false;
                    O.Dynamic1(smpl);
                    O.DollarIndexerSetData(O.LogicalAnd(smpl, O.StrictlyLargerThan(smpl, Functions.val(smpl, null, null, listloop_i14), i40), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i14), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "o", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i14))
                    , smpl, O.Lookup(smpl, null, null, "y6", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar38, o10, O.AddSpecial(smpl, listloop_i14, i39, false))
                    ;
                    return O.Dynamic2(smpl);
                };
                O.RunAssigmentMaybeDynamic(smpl, assign_42, check_42, o10);
            }

            //[[commandEnd]]10
        }


        public static readonly ScalarVal i15 = new ScalarVal(2000d, 0);
        public static readonly ScalarVal i16 = new ScalarVal(2000d, 0);
        public static readonly ScalarVal i22 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i25 = new ScalarVal(11d, 0);
        public static readonly ScalarVal i26 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i29 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i32 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i33 = new ScalarVal(5d, 0);
        public static readonly ScalarVal i36 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i39 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i40 = new ScalarVal(10d, 0);
        public static readonly ScalarVal i41 = new ScalarVal(4d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);


            //[[commandStart]]9
            p.SetText(@"¤0"); O.InitSmpl(smpl, p);


            var record43 = Program.options.series_array_calc_missing;
            Program.options.series_array_calc_missing = G.GetMissing("zero");

            C1(smpl, p);

            Program.options.series_array_calc_missing = record43;

            //[[commandEnd]]9



        }
    }
}
