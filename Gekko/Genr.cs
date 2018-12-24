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
            o1.t1 = O.ConvertToDate(i24, O.GetDateChoices.FlexibleStart);
            ;
            o1.t2 = O.ConvertToDate(i25, O.GetDateChoices.FlexibleEnd);
            ;

            o1.Exe();

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);


            O.Assignment o2 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar26 = i27;
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "x1", null, ivTmpvar26, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
            ;

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤3"); O.InitSmpl(smpl, p);


            O.Assignment o3 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar28 = i29;
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "x2", null, ivTmpvar28, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
            ;

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);


            O.Assignment o4 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar30 = O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, Functions.date(smpl, O.HandleString(new ScalarString(@"2030")))), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x1", null, null, new LookupSettings(), EVariableType.Var, null), Functions.date(smpl, O.HandleString(new ScalarString(@"2030")))), O.Lookup(smpl, null, null, "x2", null, null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "y", null, ivTmpvar30, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
            ;

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤5"); O.InitSmpl(smpl, p);


            Func<GraphHelper, string> print5 = (gh) =>
            {
                O.Prt o5 = new O.Prt();
                labelCounter = 0; o5.guiGraphIsRefreshing = gh.isRefreshing;
                o5.guiGraphPrintCode = gh.printCode;
                o5.guiGraphIsLogTransform = gh.isLogTransform;
                o5.prtType = "p";
                ESeriesMissing r1_5 = Program.options.series_array_print_missing; ESeriesMissing r2_5 = Program.options.series_normal_print_missing; try
                {
                    O.HandleOptionBankRef1(o5.opt_bank, o5.opt_ref); O.HandleMissing1(o5.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope5 = new O.Prt.Element();
                        ope5.labelGiven = new List<string>() { "x1|[@43,78:79='x1',<1256>,5:2]|[@43,78:79='x1',<1256>,5:2]" };
                        smpl = new GekkoSmpl(o5.t1, o5.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope5.printCodesFinal = Program.GetElementPrintCodes(o5, ope5); bankNumbers = O.Prt.GetBankNumbers(null, ope5.printCodesFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope5.variable[bankNumber] = O.Lookup(smpl, null, null, "x1", null, null, new LookupSettings(), EVariableType.Var, null);
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope5);
                        }
                        smpl.bankNumber = 0;
                        o5.prtElements.Add(ope5);
                    }

                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope5 = new O.Prt.Element();
                        ope5.labelGiven = new List<string>() { "x2|[@46,82:83='x2',<1256>,5:6]|[@46,82:83='x2',<1256>,5:6]" };
                        smpl = new GekkoSmpl(o5.t1, o5.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope5.printCodesFinal = Program.GetElementPrintCodes(o5, ope5); bankNumbers = O.Prt.GetBankNumbers(null, ope5.printCodesFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope5.variable[bankNumber] = O.Lookup(smpl, null, null, "x2", null, null, new LookupSettings(), EVariableType.Var, null);
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope5);
                        }
                        smpl.bankNumber = 0;
                        o5.prtElements.Add(ope5);
                    }

                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope5 = new O.Prt.Element();
                        ope5.labelGiven = new List<string>() { "y|[@49,86:86='y',<761>,5:10]|[@49,86:86='y',<761>,5:10]" };
                        smpl = new GekkoSmpl(o5.t1, o5.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope5.printCodesFinal = Program.GetElementPrintCodes(o5, ope5); bankNumbers = O.Prt.GetBankNumbers(null, ope5.printCodesFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope5.variable[bankNumber] = O.Lookup(smpl, null, null, "y", null, null, new LookupSettings(), EVariableType.Var, null);
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope5);
                        }
                        smpl.bankNumber = 0;
                        o5.prtElements.Add(ope5);
                    }

                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_5, r2_5);
                }
                o5.counter = 4;
                o5.printCsCounter = Globals.printCs.Count - 1;
                o5.Exe();
                return o5.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print5);
            print5(new GraphHelper());

            //[[commandEnd]]5
        }


        public static readonly ScalarVal i24 = new ScalarVal(2010d);
        public static readonly ScalarVal i25 = new ScalarVal(2020d);
        public static readonly ScalarVal i27 = new ScalarVal(100d);
        public static readonly ScalarVal i29 = new ScalarVal(200d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
