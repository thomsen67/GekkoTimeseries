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
        public static void C0(GekkoSmpl smpl, P p) {
            //[[commandStart]]0
            p.SetText(@"¤7"); O.InitSmpl(smpl, p);


            O.Time o0 = new O.Time();
            o0.t1 = O.ConvertToDate(i1, O.GetDateChoices.FlexibleStart);
            ;
            o0.t2 = O.ConvertToDate(i2, O.GetDateChoices.FlexibleEnd);
            ;

            o0.Exe();

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetText(@"¤8"); O.InitSmpl(smpl, p);

            O.Assignment o1 = new O.Assignment();
            o1.opt_source = @"<[code]>pop = series(3)";


            Action assign_5 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3 = Functions.series(smpl, null, null, i4);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "pop", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
            };
            Func<bool> check_5 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3 = Functions.series(smpl, null, null, i4);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar3.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "pop", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_5, check_5, o1);

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetText(@"¤9"); O.InitSmpl(smpl, p);

            O.Assignment o2 = new O.Assignment();
            o2.opt_source = @"<[code]>pop[20, m, dk] = 100, 101, 103";


            Action assign_8 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar6 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("100"), null, new ScalarString("101"), null, new ScalarString("103"), null }));
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar6, o2, i7
                , new ScalarString("m"), new ScalarString("dk"))
                ;
            };
            Func<bool> check_8 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar6 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("100"), null, new ScalarString("101"), null, new ScalarString("103"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar6.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar6, o2, i7
                , new ScalarString("m"), new ScalarString("dk"))
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_8, check_8, o2);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤10"); O.InitSmpl(smpl, p);

            O.Assignment o3 = new O.Assignment();
            o3.opt_source = @"<[code]>pop[20, m, se] = 200, 204, 202";


            Action assign_11 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar9 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("200"), null, new ScalarString("204"), null, new ScalarString("202"), null }));
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar9, o3, i10
                , new ScalarString("m"), new ScalarString("se"))
                ;
            };
            Func<bool> check_11 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar9 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("200"), null, new ScalarString("204"), null, new ScalarString("202"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar9.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar9, o3, i10
                , new ScalarString("m"), new ScalarString("se"))
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_11, check_11, o3);

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤11"); O.InitSmpl(smpl, p);

            O.Assignment o4 = new O.Assignment();
            o4.opt_source = @"<[code]>pop[20, k, dk] = 300, 303, 302";


            Action assign_14 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar12 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("300"), null, new ScalarString("303"), null, new ScalarString("302"), null }));
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar12, o4, i13
                , new ScalarString("k"), new ScalarString("dk"))
                ;
            };
            Func<bool> check_14 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar12 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("300"), null, new ScalarString("303"), null, new ScalarString("302"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar12.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar12, o4, i13
                , new ScalarString("k"), new ScalarString("dk"))
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_14, check_14, o4);

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤12"); O.InitSmpl(smpl, p);

            O.Assignment o5 = new O.Assignment();
            o5.opt_source = @"<[code]>pop[20, k, se] = 400, 402, 401";


            Action assign_17 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar15 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("400"), null, new ScalarString("402"), null, new ScalarString("401"), null }));
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar15, o5, i16
                , new ScalarString("k"), new ScalarString("se"))
                ;
            };
            Func<bool> check_17 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar15 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("400"), null, new ScalarString("402"), null, new ScalarString("401"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar15.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar15, o5, i16
                , new ScalarString("k"), new ScalarString("se"))
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_17, check_17, o5);

            //[[commandEnd]]5


            //[[commandStart]]6
            p.SetText(@"¤13"); O.InitSmpl(smpl, p);

            O.Assignment o6 = new O.Assignment();
            o6.opt_source = @"<[code]>pop[21, m, dk] = 500, 501, 502";


            Action assign_20 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar18 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("500"), null, new ScalarString("501"), null, new ScalarString("502"), null }));
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar18, o6, i19
                , new ScalarString("m"), new ScalarString("dk"))
                ;
            };
            Func<bool> check_20 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar18 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("500"), null, new ScalarString("501"), null, new ScalarString("502"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar18.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar18, o6, i19
                , new ScalarString("m"), new ScalarString("dk"))
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_20, check_20, o6);

            //[[commandEnd]]6


            //[[commandStart]]7
            p.SetText(@"¤14"); O.InitSmpl(smpl, p);

            O.Assignment o7 = new O.Assignment();
            o7.opt_source = @"<[code]>pop[21, m, se] = 600, 605, 603";


            Action assign_23 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar21 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("600"), null, new ScalarString("605"), null, new ScalarString("603"), null }));
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar21, o7, i22
                , new ScalarString("m"), new ScalarString("se"))
                ;
            };
            Func<bool> check_23 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar21 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("600"), null, new ScalarString("605"), null, new ScalarString("603"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar21.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar21, o7, i22
                , new ScalarString("m"), new ScalarString("se"))
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_23, check_23, o7);

            //[[commandEnd]]7


            //[[commandStart]]8
            p.SetText(@"¤15"); O.InitSmpl(smpl, p);

            O.Assignment o8 = new O.Assignment();
            o8.opt_source = @"<[code]>pop[21, k, dk] = 700, 702, 704";


            Action assign_26 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar24 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("700"), null, new ScalarString("702"), null, new ScalarString("704"), null }));
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar24, o8, i25
                , new ScalarString("k"), new ScalarString("dk"))
                ;
            };
            Func<bool> check_26 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar24 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("700"), null, new ScalarString("702"), null, new ScalarString("704"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar24.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar24, o8, i25
                , new ScalarString("k"), new ScalarString("dk"))
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_26, check_26, o8);

            //[[commandEnd]]8


            //[[commandStart]]9
            p.SetText(@"¤16"); O.InitSmpl(smpl, p);

            O.Assignment o9 = new O.Assignment();
            o9.opt_source = @"<[code]>pop[21, k, se] = 800, 801, 806";


            Action assign_29 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar27 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("800"), null, new ScalarString("801"), null, new ScalarString("806"), null }));
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar27, o9, i28
                , new ScalarString("k"), new ScalarString("se"))
                ;
            };
            Func<bool> check_29 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar27 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("800"), null, new ScalarString("801"), null, new ScalarString("806"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar27.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar27, o9, i28
                , new ScalarString("k"), new ScalarString("se"))
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_29, check_29, o9);

            //[[commandEnd]]9


            //[[commandStart]]10
            p.SetText(@"¤18"); O.InitSmpl(smpl, p);

            O.Assignment o10 = new O.Assignment();
            o10.opt_source = @"<[code]>#a = ('20', '21')";


            Action assign_31 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar30 = O.ListDefHelper(O.HandleString(new ScalarString(@"20")), null, O.HandleString(new ScalarString(@"21")), null);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "#a", null, ivTmpvar30, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o10)
                ;
            };
            Func<bool> check_31 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar30 = O.ListDefHelper(O.HandleString(new ScalarString(@"20")), null, O.HandleString(new ScalarString(@"21")), null);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar30.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "#a", null, ivTmpvar30, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o10)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_31, check_31, o10);

            //[[commandEnd]]10


            //[[commandStart]]11
            p.SetText(@"¤19"); O.InitSmpl(smpl, p);

            O.Assignment o11 = new O.Assignment();
            o11.opt_source = @"<[code]>#s = ('m', 'k')";


            Action assign_33 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar32 = O.ListDefHelper(O.HandleString(new ScalarString(@"m")), null, O.HandleString(new ScalarString(@"k")), null);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "#s", null, ivTmpvar32, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o11)
                ;
            };
            Func<bool> check_33 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar32 = O.ListDefHelper(O.HandleString(new ScalarString(@"m")), null, O.HandleString(new ScalarString(@"k")), null);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar32.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "#s", null, ivTmpvar32, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o11)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_33, check_33, o11);

            //[[commandEnd]]11


            //[[commandStart]]12
            p.SetText(@"¤20"); O.InitSmpl(smpl, p);

            O.Assignment o12 = new O.Assignment();
            o12.opt_source = @"<[code]>#o = ('dk', 'se')";


            Action assign_35 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar34 = O.ListDefHelper(O.HandleString(new ScalarString(@"dk")), null, O.HandleString(new ScalarString(@"se")), null);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "#o", null, ivTmpvar34, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o12)
                ;
            };
            Func<bool> check_35 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar34 = O.ListDefHelper(O.HandleString(new ScalarString(@"dk")), null, O.HandleString(new ScalarString(@"se")), null);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar34.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "#o", null, ivTmpvar34, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o12)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_35, check_35, o12);

            //[[commandEnd]]12


            //[[commandStart]]13
            p.SetText(@"¤22"); O.InitSmpl(smpl, p);

            O.Assignment o13 = new O.Assignment();
            o13.opt_source = @"<[code]>popsum = sum((#a, #s, #o), pop[#a, #s, #o])";

            Func<GekkoSmpl, IVariable> func41 = (GekkoSmpl smpl43) => {
                var smplCommandRemember42 = smpl43.command; smpl43.command = GekkoSmplCommand.Sum;
                Series temp40 = new Series(ESeriesType.Normal, Program.options.freq, null); temp40.SetZero(smpl43);

                foreach (IVariable listloop_a37 in new O.GekkoListIterator(O.Lookup(smpl43, null, ((O.scalarStringHash).Add(smpl43, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null))) {
                    foreach (IVariable listloop_s38 in new O.GekkoListIterator(O.Lookup(smpl43, null, ((O.scalarStringHash).Add(smpl43, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null))) {
                        foreach (IVariable listloop_o39 in new O.GekkoListIterator(O.Lookup(smpl43, null, ((O.scalarStringHash).Add(smpl43, (new ScalarString("o")))), null, new LookupSettings(), EVariableType.Var, null))) {
                            temp40.InjectAdd(smpl43, O.Indexer(O.Indexer2(smpl43, O.EIndexerType.None, listloop_a37, listloop_s38, listloop_o39), smpl43, O.EIndexerType.None, O.Lookup(smpl43, null, null, "pop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a37, listloop_s38, listloop_o39));

                            labelCounter++;
                        }
                    }
                }
                labelCounter = 0;
                smpl43.command = smplCommandRemember42;
                return temp40;

            };


            Action assign_44 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar36 = func41(smpl);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "popsum", null, ivTmpvar36, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o13)
                ;
            };
            Func<bool> check_44 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar36 = func41(smpl);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar36.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "popsum", null, ivTmpvar36, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o13)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_44, check_44, o13);

            //[[commandEnd]]13


            //[[commandStart]]14
            p.SetText(@"¤23"); O.InitSmpl(smpl, p);

            O.Assignment o14 = new O.Assignment();
            o14.opt_source = @"<[code]>popsum += 1, -2, 3";


            Action assign_46 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar45 = O.Add(smpl, O.Lookup(smpl, null, null, "popsum", null, null, new LookupSettings(), EVariableType.Var, null), O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("1"), null, ((new ScalarString("-")).Add(smpl, new ScalarString("2"))), null, new ScalarString("3"), null })));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "popsum", null, ivTmpvar45, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o14)
                ;
            };
            Func<bool> check_46 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar45 = O.Add(smpl, O.Lookup(smpl, null, null, "popsum", null, null, new LookupSettings(), EVariableType.Var, null), O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("1"), null, ((new ScalarString("-")).Add(smpl, new ScalarString("2"))), null, new ScalarString("3"), null })));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar45.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "popsum", null, ivTmpvar45, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o14)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_46, check_46, o14);

            //[[commandEnd]]14


            //[[commandStart]]15
            p.SetText(@"¤25"); O.InitSmpl(smpl, p);


            O.Clone o15 = new O.Clone();
            o15.Exe();

            //[[commandEnd]]15


            //[[commandStart]]16
            p.SetText(@"¤27"); O.InitSmpl(smpl, p);


            O.Model o16 = new O.Model();
            o16.p = p; o16.fileName = O.ConvertToString((new ScalarString("pop")));

            o16.opt_gms = "yes";

            o16.Exe();

            //[[commandEnd]]16


            //[[commandStart]]17
            p.SetText(@"¤29"); O.InitSmpl(smpl, p);

            Func<GekkoSmpl, IVariable> func53 = (GekkoSmpl smpl55) => {
                var smplCommandRemember54 = smpl55.command; smpl55.command = GekkoSmplCommand.Sum;
                Series temp52 = new Series(ESeriesType.Normal, Program.options.freq, null); temp52.SetZero(smpl55);

                foreach (IVariable listloop_a49 in new O.GekkoListIterator(O.Lookup(smpl55, null, ((O.scalarStringHash).Add(smpl55, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null))) {
                    foreach (IVariable listloop_s50 in new O.GekkoListIterator(O.Lookup(smpl55, null, ((O.scalarStringHash).Add(smpl55, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null))) {
                        foreach (IVariable listloop_o51 in new O.GekkoListIterator(O.Lookup(smpl55, null, ((O.scalarStringHash).Add(smpl55, (new ScalarString("o")))), null, new LookupSettings(), EVariableType.Var, null))) {
                            temp52.InjectAdd(smpl55, O.Indexer(O.Indexer2(smpl55, O.EIndexerType.None, listloop_a49, listloop_s50, listloop_o51), smpl55, O.EIndexerType.None, O.Lookup(smpl55, null, null, "pop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a49, listloop_s50, listloop_o51));

                            labelCounter++;
                        }
                    }
                }
                labelCounter = 0;
                smpl55.command = smplCommandRemember54;
                return temp52;

            };

            Func<GekkoSmpl, IVariable> Evalcode56 = (smpl57) => { return O.Add(smpl57, O.Lookup(smpl57, null, null, "popsum", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl57, (func53(smpl57))));
            };

            O.Decomp2 o17 = new O.Decomp2();
            o17.label = @"popsum in popsum = sum((#a, #s, #o), pop[#a, #s, #o])";
            o17.t1 = O.ConvertToDate(i47, O.GetDateChoices.FlexibleStart);
            ;
            o17.t2 = O.ConvertToDate(i48, O.GetDateChoices.FlexibleEnd);
            ;

            o17.opt_prtcode = O.ConvertToString((new ScalarString("xd")));



            o17.decompItems.Add(new DecompItems(Evalcode56, O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("popsum") })), null));

            o17.where.Add(new List<IVariable>() { O.HandleString(new ScalarString(@"se")), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("o")) })) });
            o17.where.Add(new List<IVariable>() { O.HandleString(new ScalarString(@"se")), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("o")) })) });

            o17.group.Add(new List<IVariable>() { O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("a")) })), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("a_agg")) })), O.HandleString(new ScalarString(@"10-year")), O.HandleString(new ScalarString(@"27")) });
            o17.group.Add(new List<IVariable>() { O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("a")) })), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("a_agg")) })), O.HandleString(new ScalarString(@"10-year")), O.HandleString(new ScalarString(@"27")) });

            o17.decompItems.Add(new DecompItems(null, O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("x1") })), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("e2") }))));


            o17.decompItems.Add(new DecompItems(null, O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("x3") })), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("e1") }))));



            o17.Exe();

            //[[commandEnd]]17
        }


        public static readonly ScalarVal i1 = new ScalarVal(2001d);
        public static readonly ScalarVal i2 = new ScalarVal(2003d);
        public static readonly ScalarVal i4 = new ScalarVal(3d);
        public static readonly ScalarVal i7 = new ScalarVal(20d);
        public static readonly ScalarVal i10 = new ScalarVal(20d);
        public static readonly ScalarVal i13 = new ScalarVal(20d);
        public static readonly ScalarVal i16 = new ScalarVal(20d);
        public static readonly ScalarVal i19 = new ScalarVal(21d);
        public static readonly ScalarVal i22 = new ScalarVal(21d);
        public static readonly ScalarVal i25 = new ScalarVal(21d);
        public static readonly ScalarVal i28 = new ScalarVal(21d);
        public static readonly ScalarVal i47 = new ScalarVal(2002d);
        public static readonly ScalarVal i48 = new ScalarVal(2003d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
