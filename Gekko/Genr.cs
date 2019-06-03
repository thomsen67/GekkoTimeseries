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

            Func<IVariable, IVariable, IVariable, IVariable, IVariable> func54 = (IVariable listloop_a45, IVariable listloop_a42, IVariable listloop_b43, IVariable listloop_c44) =>
            {
                var smplCommandRemember55 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp53 = new Series(ESeriesType.Normal, Program.options.freq, null); temp53.SetZero(smpl);

                foreach (IVariable listloop_b47 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("b")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    //temp53.InjectAdd(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a45, listloop_b47, listloop_c44), smpl, O.EIndexerType.None, listloopMovedStuff_49, listloop_a45, listloop_b47, O.ReportLabel(smpl, listloop_c44, "#¨c|[@52,65:65='#',<1276>,1:65]|[@54,67:67='c',<1318>,1:67]")));

                    labelCounter++;
                }
                smpl.command = smplCommandRemember55;
                return temp53;

            };

            Func<IVariable, IVariable, IVariable, IVariable> func57 = (IVariable listloop_a42, IVariable listloop_b43, IVariable listloop_c44) =>
            {
                IVariable listloopMovedStuff_46 = O.Lookup(smpl, null, null, "#a", null, null, new LookupSettings(), EVariableType.Var, null);
                IVariable listloopMovedStuff_48 = O.Lookup(smpl, null, null, "#b", null, null, new LookupSettings(), EVariableType.Var, null);
                IVariable listloopMovedStuff_49 = O.Lookup(smpl, null, null, "x3", null, null, new LookupSettings(), EVariableType.Var, null);
                IVariable listloopMovedStuff_50 = O.Lookup(smpl, null, null, "#a", null, null, new LookupSettings(), EVariableType.Var, null);
                IVariable listloopMovedStuff_51 = O.Lookup(smpl, null, null, "#b", null, null, new LookupSettings(), EVariableType.Var, null);
                IVariable listloopMovedStuff_52 = O.Lookup(smpl, null, null, "#c", null, null, new LookupSettings(), EVariableType.Var, null);

                var smplCommandRemember58 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp56 = new Series(ESeriesType.Normal, Program.options.freq, null); temp56.SetZero(smpl);

                foreach (IVariable listloop_a45 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp56.InjectAdd(smpl, func54(listloop_a45, listloop_a42, listloop_b43, listloop_c44));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember58;
                return temp56;

            };

            Func<IVariable> func60 = () =>
            {
                var smplCommandRemember61 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp59 = new List();

                foreach (IVariable listloop_a42 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    foreach (IVariable listloop_b43 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("b")))), null, new LookupSettings(), EVariableType.Var, null)))
                    {
                        foreach (IVariable listloop_c44 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, new LookupSettings(), EVariableType.Var, null)))
                        {
                            temp59.Add(O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a42), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x1", null, null, new LookupSettings(), EVariableType.Var, null), O.ReportLabel(smpl, listloop_a42, "#¨a|[@8,11:11='#',<1276>,1:11]|[@10,13:13='a',<815>,1:13]")), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_b43), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x2", null, null, new LookupSettings(), EVariableType.Var, null), O.ReportLabel(smpl, listloop_b43, "#¨b|[@17,23:23='#',<1276>,1:23]|[@19,25:25='b',<1318>,1:25]"))), func57(listloop_a42, listloop_b43, listloop_c44)));

                        }
                    }
                }
                smpl.command = smplCommandRemember61;
                return temp59;

            };


            Func<GraphHelper, string> print0 = (gh) =>
            {
                O.Prt o0 = new O.Prt();
                labelCounter = 0; o0.guiGraphIsRefreshing = gh.isRefreshing;
                o0.guiGraphOperator = gh.operator2;
                o0.guiGraphIsLogTransform = gh.isLogTransform;
                o0.prtType = "p";
                o0.t1 = Globals.globalPeriodStart;
                o0.t2 = Globals.globalPeriodEnd;

                o0.operators.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));



                ESeriesMissing r1_0 = Program.options.series_array_print_missing; ESeriesMissing r2_0 = Program.options.series_normal_print_missing; try
                {
                    O.HandleOptionBankRef1(o0.opt_bank, o0.opt_ref); O.HandleMissing1(o0.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope0 = new O.Prt.Element();
                        ope0.labelGiven = new List<string>() { "|||a, b, c|||x1[_[#¨a] + x2[_[#¨b] + sum¨(#¨a, sum¨(#¨b, x3[_[#¨a, #¨b, #¨c]))|[@6,6:7='x1',<1318>,1:6]|[@57,70:70=')',<1265>,1:70]" };
                        smpl = new GekkoSmpl(o0.t1, o0.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope0.operatorsFinal = Program.GetElementOperators(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope0.variable[bankNumber] = func60();
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
