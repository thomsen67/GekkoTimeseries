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
            IVariable listloopMovedStuff_208 = O.Lookup(smpl, null, null, "#i", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_209 = O.Lookup(smpl, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_210 = O.Lookup(smpl, null, null, "#i", null, null, new LookupSettings(), EVariableType.Var, null);

            Func<IVariable> func212 = () =>
            {
                var smplCommandRemember213 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp211 = new Series(ESeriesType.Normal, Program.options.freq, null); temp211.SetZero(smpl);

                foreach (IVariable listloop_i207 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp211.InjectAdd(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("a"), listloop_i207), smpl, O.EIndexerType.None, listloopMovedStuff_209, O.ReportLabel(smpl, new ScalarString("a"), "a|[@16,21:21='a',<815>,1:21]|[@16,21:21='a',<815>,1:21]"), listloop_i207));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember213;
                return temp211;

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

                o0.operators.Add(new OptString("m", O.ConvertToString(new ScalarString("yes"))));



                ESeriesMissing r1_0 = Program.options.series_array_print_missing; ESeriesMissing r2_0 = Program.options.series_normal_print_missing; try
                {
                    O.HandleOptionBankRef1(o0.opt_bank, o0.opt_ref); O.HandleMissing1(o0.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope0 = new O.Prt.Element();
                        ope0.labelGiven = new List<string>() { O.ConvertToString(O.HandleString(new ScalarString(@"sum"))) };
                        smpl = new GekkoSmpl(o0.t1, o0.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope0.operatorsFinal = Program.GetElementOperators(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope0.variable[bankNumber] = func212();
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
                o0.counter = 7;
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
