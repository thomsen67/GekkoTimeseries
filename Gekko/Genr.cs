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
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);
            IVariable listloopMovedStuff_44 = O.Lookup(smpl, null, null, "#a", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_45 = O.Lookup(smpl, null, null, "y", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_46 = O.Lookup(smpl, null, null, "#a", null, null, new LookupSettings(), EVariableType.Var, null);

            Func<IVariable> func49 = () => {
                var smplCommandRemember50 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp48 = new Series(ESeriesType.Normal, Program.options.freq, null); temp48.SetZero(smpl);

                foreach (IVariable listloop_a43 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null))) {
                    temp48.InjectAdd(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("b"), O.AddSpecial(smpl, listloop_a43, i47, true)), smpl, O.EIndexerType.None, listloopMovedStuff_45, O.ReportLabel(smpl, new ScalarString("b"), "b|[@12,18:18='b',<1295>,1:18]|[@12,18:18='b',<1295>,1:18]"), O.AddSpecial(smpl, listloop_a43, i47, true)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember50;
                return temp48;

            };


            Func<GraphHelper, string> print0 = (gh) =>
            {
                O.Prt o0 = new O.Prt();
                labelCounter = 0; o0.guiGraphIsRefreshing = gh.isRefreshing;
                o0.guiGraphOperator = gh.operator2;
                o0.guiGraphIsLogTransform = gh.isLogTransform;
                o0.prtType = "PRT";
                ESeriesMissing r1_0 = Program.options.series_array_print_missing; ESeriesMissing r2_0 = Program.options.series_normal_print_missing; try {
                    O.HandleOptionBankRef1(o0.opt_bank, o0.opt_ref); O.HandleMissing1(o0.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope0 = new O.Prt.Element();
                        ope0.labelGiven = new List<string>() { "sum¨(#¨a, y[_[b,#¨a-1])|[@2,4:6='sum',<1295>,1:4]|[@20,26:26=')',<1242>,1:26]" };
                        smpl = new GekkoSmpl(o0.t1, o0.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope0.operatorsFinal = Program.GetElementOperators(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope0.variable[bankNumber] = func49();
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
                        }
                        smpl.bankNumber = 0;
                        o0.prtElements.Add(ope0);
                    }

                }
                finally {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_0, r2_0);
                }
                o0.counter = 2;
                o0.printCsCounter = Globals.printCs.Count - 1;
                o0.Exe();
                return o0.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print0);
            print0(new GraphHelper());

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i47 = new ScalarVal(1d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
