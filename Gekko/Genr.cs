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

            IVariable listloopMovedStuff_5 = O.Lookup(smpl, null, null, "#a", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_7 = O.Lookup(smpl, null, null, "#b", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_8 = O.Lookup(smpl, null, null, "x3", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_9 = O.Lookup(smpl, null, null, "#a", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_10 = O.Lookup(smpl, null, null, "#b", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_11 = O.Lookup(smpl, null, null, "#c", null, null, new LookupSettings(), EVariableType.Var, null);

            Func<IVariable, IVariable, IVariable, IVariable, IVariable> func13 = (IVariable listloop_a4, IVariable listloop_a1, IVariable listloop_b2, IVariable listloop_c3) =>
            {
                var smplCommandRemember14 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp12 = new Series(ESeriesType.Normal, Program.options.freq, null); temp12.SetZero(smpl);

                foreach (IVariable listloop_b6 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("b")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp12.InjectAdd(smpl, temp12, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a4, listloop_b6, listloop_c3), smpl, O.EIndexerType.None, listloopMovedStuff_8, listloop_a4, listloop_b6, O.ReportLabel(smpl, listloop_c3, "#¨c|[@52,65:65='#',<1211>,1:65]|[@54,67:67='c',<1252>,1:67]")));

                    labelCounter++;
                }
                smpl.command = smplCommandRemember14;
                return temp12;

            };

            
            Func<IVariable, IVariable, IVariable, IVariable> func16 = (IVariable listloop_a1, IVariable listloop_b2, IVariable listloop_c3) =>
            {
                var smplCommandRemember17 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp15 = new Series(ESeriesType.Normal, Program.options.freq, null); temp15.SetZero(smpl);

                foreach (IVariable listloop_a4 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp15.InjectAdd(smpl, temp15, func13(listloop_a4, listloop_a1, listloop_b2, listloop_c3));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember17;
                return temp15;

            };

            Func<IVariable> func19 = () =>
            {
                var smplCommandRemember20 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp18 = new List();

                foreach (IVariable listloop_a1 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    foreach (IVariable listloop_b2 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("b")))), null, new LookupSettings(), EVariableType.Var, null)))
                    {
                        foreach (IVariable listloop_c3 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, null)))
                        {
                            temp18.Add(O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a1), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x1", null, null, new LookupSettings(), EVariableType.Var, null), O.ReportLabel(smpl, listloop_a1, "#¨a|[@8,11:11='#',<1211>,1:11]|[@10,13:13='a',<769>,1:13]")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_b2), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x2", null, null, new LookupSettings(), EVariableType.Var, null), O.ReportLabel(smpl, listloop_b2, "#¨b|[@17,23:23='#',<1211>,1:23]|[@19,25:25='b',<1252>,1:25]"))), func16(listloop_a1, listloop_b2, listloop_c3)));

                        }
                    }
                }
                smpl.command = smplCommandRemember20;
                return temp18;

            };


            Func<GraphHelper, string> print0 = (gh) =>
            {
                O.Prt o0 = new O.Prt();
                labelCounter = 0; o0.guiGraphIsRefreshing = gh.isRefreshing;
                o0.guiGraphPrintCode = gh.printCode;
                o0.guiGraphIsLogTransform = gh.isLogTransform;
                o0.prtType = "p";
                o0.t1 = Globals.globalPeriodStart;
                o0.t2 = Globals.globalPeriodEnd;

                o0.printCodes.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));



                ESeriesMissing r1_0 = Program.options.series_array_print_missing; ESeriesMissing r2_0 = Program.options.series_normal_print_missing; try
                {
                    O.HandleOptionBankRef1(o0.opt_bank, o0.opt_ref); O.HandleMissing1(o0.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope0 = new O.Prt.Element();
                        ope0.labelGiven = new List<string>() { "|||a, b, c|||x1[_[#¨a] + x2[_[#¨b] + sum¨(#¨a, sum¨(#¨b, x3[_[#¨a, #¨b, #¨c]))|[@6,6:7='x1',<1252>,1:6]|[@57,70:70=')',<1200>,1:70]" };
                        smpl = new GekkoSmpl(o0.t1, o0.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope0.variable[bankNumber] = func19();
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
                        }
                        smpl.bankNumber = 0;
                        o0.prtElements.Add(ope0);
                    }

                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_0, r2_0);
                }
                o0.counter = 1;
                o0.printCsCounter = Globals.printCs.Count - 1;
                o0.Exe();
                return o0.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print0);
            print0(new GraphHelper());

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
