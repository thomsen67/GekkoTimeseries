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
            o2.opt_source = @"<[code]>x = 3,4,5";


            Action assign_4 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("3"), null, new ScalarString("4"), null, new ScalarString("5"), null }));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_4 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("3"), null, new ScalarString("4"), null, new ScalarString("5"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar3.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_4, check_4, o2);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            Program.options.freq = G.GetFreq(O.ConvertToString((new ScalarString("m"))));
            G.Writeln();
            G.Writeln("option freq = " + (G.GetFreq(O.ConvertToString((new ScalarString("m"))))).ToString().ToLower() + "");
            Program.AdjustFreq();
            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Time o4 = new O.Time();
            o4.t1 = O.ConvertToDate(new ScalarDate(GekkoTime.FromStringToGekkoTime("2002m11")), O.GetDateChoices.FlexibleStart);
            ;
            o4.t2 = O.ConvertToDate(new ScalarDate(GekkoTime.FromStringToGekkoTime("2003m11")), O.GetDateChoices.FlexibleEnd);
            ;

            o4.Exe();

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            Func<GraphHelper, string> print5 = (gh) =>
            {
                O.Prt o5 = new O.Prt();
                labelCounter = 0; o5.guiGraphIsRefreshing = gh.isRefreshing;
                o5.guiGraphOperator = gh.operator2;
                o5.guiGraphIsLogTransform = gh.isLogTransform;
                o5.prtType = "prt";
                ESeriesMissing r1_5 = Program.options.series_array_print_missing; ESeriesMissing r2_5 = Program.options.series_array_calc_missing; ESeriesMissing r3_5 = Program.options.series_data_missing; try
                {
                    O.HandleOptionBankRef1(o5.opt_bank, o5.opt_ref); O.HandleMissing1(o5.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope5 = new O.Prt.Element();
                        ope5.labelGiven = new List<string>() { "pch¨(x¨!¨a)|[@35,73:75='pch',<1139>,1:73]|[@43,83:83=')',<1322>,1:83]" };
                        smpl = new GekkoSmpl(o5.t1, o5.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope5.operatorsFinal = Program.GetElementOperators(o5, ope5); bankNumbers = O.Prt.GetBankNumbers(null, ope5.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope5.variable[bankNumber] = Functions.pch(O.Smpl(smpl, -1), smpl, null, null, O.Lookup(smpl, null, null, "x", "a", null, new LookupSettings(), EVariableType.Var, null));
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope5);
                        }
                        smpl.bankNumber = 0;
                        o5.prtElements.Add(ope5);
                    }

                    o5.printCsCounter = Globals.printCs.Count - 1;
                    o5.Exe();
                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_5, r2_5, r3_5);
                }
                return o5.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print5);
            print5(new GraphHelper());

            //[[commandEnd]]5
        }


        public static readonly ScalarVal i1 = new ScalarVal(2001d, 0);
        public static readonly ScalarVal i2 = new ScalarVal(2003d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
