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

            Func<GekkoSmpl, IVariable> func319 = (GekkoSmpl smpl321) =>
            {
                var smplCommandRemember320 = smpl321.command; smpl321.command = GekkoSmplCommand.Sum;
                Series temp318 = new Series(ESeriesType.Normal, Program.options.freq, null); temp318.SetZero(smpl321);

                foreach (IVariable listloop_i317 in new O.GekkoListIterator(O.Lookup(smpl321, null, ((O.scalarStringHash).Add(smpl321, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp318.InjectAdd(smpl321, O.Indexer(O.Indexer2(smpl321, O.EIndexerType.None, listloop_i317), smpl321, O.EIndexerType.None, O.Lookup(smpl321, null, null, "y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i317));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl321.command = smplCommandRemember320;
                return temp318;

            };


            Func<GraphHelper, string> print0 = (gh) =>
            {
                O.Prt o0 = new O.Prt();
                labelCounter = 0; o0.guiGraphIsRefreshing = gh.isRefreshing;
                o0.guiGraphOperator = gh.operator2;
                o0.guiGraphIsLogTransform = gh.isLogTransform;
                o0.prtType = "prt";
                o0.t1 = Globals.globalPeriodStart;
                o0.t2 = Globals.globalPeriodEnd;

                o0.opt_missing = O.ConvertToString((new ScalarString("zero")));


                ESeriesMissing r1_0 = Program.options.series_array_print_missing; ESeriesMissing r2_0 = Program.options.series_normal_print_missing; ESeriesMissing r3_0 = Program.options.series_data_missing; try
                {
                    O.HandleOptionBankRef1(o0.opt_bank, o0.opt_ref); O.HandleMissing1(o0.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope0 = new O.Prt.Element();
                        ope0.labelGiven = new List<string>() { "sum¨(#¨i, y[_[#¨i])|[@7,18:20='sum',<1331>,1:18]|[@21,36:36=')',<1278>,1:36]" };
                        smpl = new GekkoSmpl(o0.t1, o0.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope0.operatorsFinal = Program.GetElementOperators(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope0.variable[bankNumber] = func319(smpl);
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
                        }
                        smpl.bankNumber = 0;
                        o0.prtElements.Add(ope0);
                    }

                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_0, r2_0, r3_0);
                }
                o0.counter = 10;
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
