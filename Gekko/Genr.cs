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
            IVariable listloopMovedStuff_249 = O.Lookup(smpl, null, null, "#a", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_250 = O.Lookup(smpl, null, null, "vHH", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_251 = O.Lookup(smpl, null, null, "#a", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_253 = O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_254 = O.Lookup(smpl, null, null, "#a", null, null, new LookupSettings(), EVariableType.Var, null);

            Func<IVariable> func256 = () =>
            {
                var smplCommandRemember257 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp255 = new Series(ESeriesType.Normal, Program.options.freq, null); temp255.SetZero(smpl);

                foreach (IVariable listloop_a248 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    IVariable bug = null;
                    GekkoSmpl2 temp2 = O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("Pension"), O.AddSpecial(smpl, listloop_a248, i252, true));
                    IVariable temp = O.Indexer(temp2, smpl, O.EIndexerType.None, listloopMovedStuff_250, O.ReportLabel(smpl, new ScalarString("Pension"), "Pension|[@11,19:25='Pension',<1282>,1:19]|[@11,19:25='Pension',<1282>,1:19]"), bug);
                    temp255.InjectAdd(smpl, O.Multiply(smpl, temp, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a248), smpl, O.EIndexerType.None, listloopMovedStuff_253, listloop_a248)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember257;
                return temp255;

            };


            Func<GraphHelper, string> print0 = (gh) =>
            {
                O.Prt o0 = new O.Prt();
                labelCounter = 0; o0.guiGraphIsRefreshing = gh.isRefreshing;
                o0.guiGraphOperator = gh.operator2;
                o0.guiGraphIsLogTransform = gh.isLogTransform;
                o0.prtType = "PRT";
                ESeriesMissing r1_0 = Program.options.series_array_print_missing; ESeriesMissing r2_0 = Program.options.series_normal_print_missing; try
                {
                    O.HandleOptionBankRef1(o0.opt_bank, o0.opt_ref); O.HandleMissing1(o0.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope0 = new O.Prt.Element();
                        ope0.labelGiven = new List<string>() { "sum¨(#¨a,vHH[_[Pension,#¨a-1]*½nPop[_[#¨a])|[@2,4:6='sum',<1282>,1:4]|[@27,46:46=')',<1229>,1:46]" };
                        smpl = new GekkoSmpl(o0.t1, o0.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope0.operatorsFinal = Program.GetElementOperators(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope0.variable[bankNumber] = func256();
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
                o0.counter = 22;
                o0.printCsCounter = Globals.printCs.Count - 1;
                o0.Exe();
                return o0.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print0);
            print0(new GraphHelper());

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i252 = new ScalarVal(1d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
