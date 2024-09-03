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

            Functions.tic(smpl, null, null);

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetStack(@"¤3"); O.InitSmpl(smpl, p);

            O.Time o2 = new O.Time();
            o2.t1 = O.ConvertToDate(i1, O.GetDateChoices.FlexibleStart);
            ;
            o2.t2 = O.ConvertToDate(i2, O.GetDateChoices.FlexibleEnd);
            ;

            o2.Exe();

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetStack(@"¤4"); O.InitSmpl(smpl, p);

            Program.options.databank_trace = O.XBool("databank trace", (new ScalarString("no")));
            O.PrintOptions("Program.options.databank_trace", false);
            O.HandleOptions("Program.options.databank_trace", 0, p);

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetStack(@"¤5"); O.InitSmpl(smpl, p);
            O.Assignment o4 = new O.Assignment();
            o4.opt_trace = @"<95 2020> y1 = data('10 20 30 40 50 60 70 80 90 100 110 120 130 140 150 160 170 180 190 200 210 220 230 240 250 260')";
            smpl.t0 = O.ConvertToDate(i4, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t1 = O.ConvertToDate(i4, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t2 = O.ConvertToDate(i5, O.GetDateChoices.FlexibleEnd);
            ;
            smpl.t3 = O.ConvertToDate(i5, O.GetDateChoices.FlexibleEnd);
            ;




            Globals.precedentsSeries = null;
            Action assign_6 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3 = Functions.data(smpl, null, null, O.HandleString(new ScalarString(@"10 20 30 40 50 60 70 80 90 100 110 120 130 140 150 160 170 180 190 200 210 220 230 240 250 260")));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y1", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
            };
            Func<bool> check_6 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3 = Functions.data(smpl, null, null, O.HandleString(new ScalarString(@"10 20 30 40 50 60 70 80 90 100 110 120 130 140 150 160 170 180 190 200 210 220 230 240 250 260")));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar3.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y1", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_6, check_6, o4, p);

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetStack(@"¤6"); O.InitSmpl(smpl, p);
            O.Assignment o5 = new O.Assignment();
            o5.opt_trace = @"<95 2020> y2 = data('10 20 30 40 50 60 70 80 90 100 110 120 130 140 150 160 170 180 190 200 210 220 230 240 250 260')";
            smpl.t0 = O.ConvertToDate(i8, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t1 = O.ConvertToDate(i8, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t2 = O.ConvertToDate(i9, O.GetDateChoices.FlexibleEnd);
            ;
            smpl.t3 = O.ConvertToDate(i9, O.GetDateChoices.FlexibleEnd);
            ;




            Globals.precedentsSeries = null;
            Action assign_10 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar7 = Functions.data(smpl, null, null, O.HandleString(new ScalarString(@"10 20 30 40 50 60 70 80 90 100 110 120 130 140 150 160 170 180 190 200 210 220 230 240 250 260")));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y2", null, ivTmpvar7, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o5)
                ;
            };
            Func<bool> check_10 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar7 = Functions.data(smpl, null, null, O.HandleString(new ScalarString(@"10 20 30 40 50 60 70 80 90 100 110 120 130 140 150 160 170 180 190 200 210 220 230 240 250 260")));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar7.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y2", null, ivTmpvar7, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o5)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_10, check_10, o5, p);

            //[[commandEnd]]5


            //[[commandStart]]6
            p.SetStack(@"¤7"); O.InitSmpl(smpl, p);
            O.Assignment o6 = new O.Assignment();
            o6.opt_trace = @"<95 2020> y3 = data('10 20 30 40 50 60 70 80 90 100 110 120 130 140 150 160 170 180 190 200 210 220 230 240 250 260')";
            smpl.t0 = O.ConvertToDate(i12, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t1 = O.ConvertToDate(i12, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t2 = O.ConvertToDate(i13, O.GetDateChoices.FlexibleEnd);
            ;
            smpl.t3 = O.ConvertToDate(i13, O.GetDateChoices.FlexibleEnd);
            ;




            Globals.precedentsSeries = null;
            Action assign_14 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar11 = Functions.data(smpl, null, null, O.HandleString(new ScalarString(@"10 20 30 40 50 60 70 80 90 100 110 120 130 140 150 160 170 180 190 200 210 220 230 240 250 260")));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y3", null, ivTmpvar11, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o6)
                ;
            };
            Func<bool> check_14 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar11 = Functions.data(smpl, null, null, O.HandleString(new ScalarString(@"10 20 30 40 50 60 70 80 90 100 110 120 130 140 150 160 170 180 190 200 210 220 230 240 250 260")));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar11.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y3", null, ivTmpvar11, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o6)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_14, check_14, o6, p);

            //[[commandEnd]]6


            //[[commandStart]]7
            p.SetStack(@"¤8"); O.InitSmpl(smpl, p);
            O.Assignment o7 = new O.Assignment();
            o7.opt_trace = @"<95 2020> x1 = data('1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26')";
            smpl.t0 = O.ConvertToDate(i16, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t1 = O.ConvertToDate(i16, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t2 = O.ConvertToDate(i17, O.GetDateChoices.FlexibleEnd);
            ;
            smpl.t3 = O.ConvertToDate(i17, O.GetDateChoices.FlexibleEnd);
            ;




            Globals.precedentsSeries = null;
            Action assign_18 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar15 = Functions.data(smpl, null, null, O.HandleString(new ScalarString(@"1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26")));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x1", null, ivTmpvar15, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o7)
                ;
            };
            Func<bool> check_18 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar15 = Functions.data(smpl, null, null, O.HandleString(new ScalarString(@"1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26")));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar15.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x1", null, ivTmpvar15, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o7)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_18, check_18, o7, p);

            //[[commandEnd]]7


            //[[commandStart]]8
            p.SetStack(@"¤9"); O.InitSmpl(smpl, p);
            O.Assignment o8 = new O.Assignment();
            o8.opt_trace = @"val %k = 0";


            Globals.precedentsSeries = null;
            Action assign_21 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar19 = i20;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%k", null, ivTmpvar19, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o8)
                ;
            };
            Func<bool> check_21 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar19 = i20;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar19.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%k", null, ivTmpvar19, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o8)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_21, check_21, o8, p);

            //[[commandEnd]]8


            //[[commandStart]]9
            p.SetStack(@"¤10"); O.InitSmpl(smpl, p);
            O.Assignment o9 = new O.Assignment();
            o9.opt_trace = @"val %x  = 1";


            Globals.precedentsSeries = null;
            Action assign_24 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar22 = i23;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%x", null, ivTmpvar22, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o9)
                ;
            };
            Func<bool> check_24 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar22 = i23;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar22.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%x", null, ivTmpvar22, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o9)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_24, check_24, o9, p);

            //[[commandEnd]]9


            //[[commandStart]]10
            p.SetStack(@"¤11"); O.InitSmpl(smpl, p);
            O.Assignment o10 = new O.Assignment();
            o10.opt_trace = @"val %k1 = 0";


            Globals.precedentsSeries = null;
            Action assign_27 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar25 = i26;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%k1", null, ivTmpvar25, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o10)
                ;
            };
            Func<bool> check_27 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar25 = i26;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar25.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%k1", null, ivTmpvar25, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o10)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_27, check_27, o10, p);

            //[[commandEnd]]10


            //[[commandStart]]11
            p.SetStack(@"¤12"); O.InitSmpl(smpl, p);
            O.Assignment o11 = new O.Assignment();
            o11.opt_trace = @"val %k2 = 1e+5";


            Globals.precedentsSeries = null;
            Action assign_30 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar28 = d29;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%k2", null, ivTmpvar28, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o11)
                ;
            };
            Func<bool> check_30 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar28 = d29;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar28.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%k2", null, ivTmpvar28, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o11)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_30, check_30, o11, p);

            //[[commandEnd]]11
        }
        public static void C1(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]13
            p.SetStack(@"¤14"); O.InitSmpl(smpl, p);
            O.Assignment o13 = new O.Assignment();
            o13.opt_trace = @"ser <2000 2020> x1 = y1 + y1[-1] + y2 + y2[-1] + y3[2000] + %x + 1 + 2";
            smpl.t0 = O.ConvertToDate(i38, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t1 = O.ConvertToDate(i38, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t2 = O.ConvertToDate(i39, O.GetDateChoices.FlexibleEnd);
            ;
            smpl.t3 = O.ConvertToDate(i39, O.GetDateChoices.FlexibleEnd);
            ;




            Globals.precedentsSeries = null;
            Action assign_40 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar32 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "y1", null, null, new LookupSettings(), EVariableType.Series, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i33)
                ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "y1", null, null, new LookupSettings(), EVariableType.Series, null), O.Negate(smpl, i33)
                )), O.Lookup(smpl, null, null, "y2", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i34)
                ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "y2", null, null, new LookupSettings(), EVariableType.Series, null), O.Negate(smpl, i34)
                )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, i35
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "y3", null, null, new LookupSettings(), EVariableType.Series, null), i35
                )), O.Lookup(smpl, null, null, "%x", null, null, new LookupSettings(), EVariableType.Var, null)), i36), i37);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x1", null, ivTmpvar32, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Series, o13)
                ;
            };
            Func<bool> check_40 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar32 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "y1", null, null, new LookupSettings(), EVariableType.Series, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i33)
                ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "y1", null, null, new LookupSettings(), EVariableType.Series, null), O.Negate(smpl, i33)
                )), O.Lookup(smpl, null, null, "y2", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i34)
                ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "y2", null, null, new LookupSettings(), EVariableType.Series, null), O.Negate(smpl, i34)
                )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, i35
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "y3", null, null, new LookupSettings(), EVariableType.Series, null), i35
                )), O.Lookup(smpl, null, null, "%x", null, null, new LookupSettings(), EVariableType.Var, null)), i36), i37);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar32.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x1", null, ivTmpvar32, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Series, o13)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_40, check_40, o13, p);

            //[[commandEnd]]13
        }
        public static void C2(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]14
            p.SetStack(@"¤16"); O.InitSmpl(smpl, p);
            O.Assignment o14 = new O.Assignment();
            o14.opt_trace = @"%toc = toc()";


            Globals.precedentsSeries = null;
            Action assign_44 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar43 = Functions.toc(smpl, null, null);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%toc", null, ivTmpvar43, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o14)
                ;
            };
            Func<bool> check_44 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar43 = Functions.toc(smpl, null, null);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar43.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%toc", null, ivTmpvar43, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o14)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_44, check_44, o14, p);

            //[[commandEnd]]14


            //[[commandStart]]15
            p.SetStack(@"¤17"); O.InitSmpl(smpl, p);

            O.Tell o15 = new O.Tell();
            o15.s = O.Add(smpl, O.Add(smpl, O.HandleString(new ScalarString(@"")), O.Lookup(smpl, null, null, "%toc", null, null, new LookupSettings(), EVariableType.Var, null)), O.HandleString(new ScalarString(@" s")));
            o15.Exe();

            //[[commandEnd]]15


            //[[commandStart]]16
            p.SetStack(@"¤18"); O.InitSmpl(smpl, p);

            O.Tell o16 = new O.Tell();
            o16.s = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.HandleString(new ScalarString(@"")), O.Divide(smpl, O.Lookup(smpl, null, null, "%k2", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "%toc", null, null, new LookupSettings(), EVariableType.Var, null))), O.HandleString(new ScalarString(@" // "))), O.Divide(smpl, O.Divide(smpl, O.Lookup(smpl, null, null, "%k2", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "%toc", null, null, new LookupSettings(), EVariableType.Var, null)), i45)), O.HandleString(new ScalarString(@" k")));
            o16.Exe();

            //[[commandEnd]]16
        }


        public static readonly ScalarVal i1 = new ScalarVal(95d, 0);
        public static readonly ScalarVal i2 = new ScalarVal(2020d, 0);
        public static readonly ScalarVal i4 = new ScalarVal(95d, 0);
        public static readonly ScalarVal i5 = new ScalarVal(2020d, 0);
        public static readonly ScalarVal i8 = new ScalarVal(95d, 0);
        public static readonly ScalarVal i9 = new ScalarVal(2020d, 0);
        public static readonly ScalarVal i12 = new ScalarVal(95d, 0);
        public static readonly ScalarVal i13 = new ScalarVal(2020d, 0);
        public static readonly ScalarVal i16 = new ScalarVal(95d, 0);
        public static readonly ScalarVal i17 = new ScalarVal(2020d, 0);
        public static readonly ScalarVal i20 = new ScalarVal(0d, 0);
        public static readonly ScalarVal i23 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i26 = new ScalarVal(0d, 0);
        public static readonly ScalarVal d29 = new ScalarVal(1e+5d);
        public static readonly ScalarVal i33 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i34 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i35 = new ScalarVal(2000d, 0);
        public static readonly ScalarVal i36 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i37 = new ScalarVal(2d, 0);
        public static readonly ScalarVal i38 = new ScalarVal(2000d, 0);
        public static readonly ScalarVal i39 = new ScalarVal(2020d, 0);
        public static readonly ScalarVal i45 = new ScalarVal(1000d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);


            p.SetStack(@"¤13");


            //[[commandSpecial]]12
            IVariable forloop_xe7dke6cj_31 = null;
            int counter41 = 0;
            bool years42 = O.LoopYears("val", O.ELoopType.ForTo, O.Lookup(smpl, null, null, "%k1", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "%k2", null, null, new LookupSettings(), EVariableType.Var, null)); for (O.IterateStart(years42, O.ELoopType.ForTo, ref forloop_xe7dke6cj_31, O.Lookup(smpl, null, null, "%k1", null, null, new LookupSettings(), EVariableType.Var, null)); O.IterateContinue(years42, O.ELoopType.ForTo, forloop_xe7dke6cj_31, O.Lookup(smpl, null, null, "%k1", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "%k2", null, null, new LookupSettings(), EVariableType.Var, null), null, ref counter41); O.IterateStep(years42, O.ELoopType.ForTo, ref forloop_xe7dke6cj_31, O.Lookup(smpl, null, null, "%k1", null, null, new LookupSettings(), EVariableType.Var, null), null, counter41))
            {
                ;
                O.TypeCheck_val(forloop_xe7dke6cj_31, 0);

                C1(smpl, p);

            };

            //[[commandEnd]]12


            C2(smpl, p);



        }
    }
}
