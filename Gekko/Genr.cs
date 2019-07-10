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

            Func<GekkoSmpl, IVariable> func158 = (GekkoSmpl smpl160) =>
            {
                var smplCommandRemember159 = smpl160.command; smpl160.command = GekkoSmplCommand.Unfold;
                List temp157 = new List();

                foreach (IVariable listloop_a155 in new O.GekkoListIterator(O.Lookup(smpl160, null, ((O.scalarStringHash).Add(smpl160, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    foreach (IVariable listloop_e156 in new O.GekkoListIterator(O.Lookup(smpl160, null, ((O.scalarStringHash).Add(smpl160, (new ScalarString("e")))), null, new LookupSettings(), EVariableType.Var, null)))
                    {
                        temp157.Add(O.Indexer(O.Indexer2(smpl160, O.EIndexerType.None, listloop_e156, listloop_a155), smpl160, O.EIndexerType.None, O.Lookup(smpl160, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), O.ReportLabel(smpl160, listloop_e156, "#¨e|[@13,30:30='#',<1289>,1:30]|[@15,32:32='e',<1331>,1:32]"), O.ReportLabel(smpl160, listloop_a155, "#¨a|[@18,35:35='#',<1289>,1:35]|[@20,37:37='a',<824>,1:37]")));

                    }
                }
                smpl160.command = smplCommandRemember159;
                return temp157;

            };


            Func<GraphHelper, string> print0 = (gh) =>
            {
                O.Prt o0 = new O.Prt();
                labelCounter = 0; o0.guiGraphIsRefreshing = gh.isRefreshing;
                o0.guiGraphOperator = gh.operator2;
                o0.guiGraphIsLogTransform = gh.isLogTransform;
                o0.prtType = "PRT";
                o0.operators.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));

                o0.opt_missing = O.ConvertToString((new ScalarString("ignore")));

                ESeriesMissing r1_0 = Program.options.series_array_print_missing; ESeriesMissing r2_0 = Program.options.series_array_calc_missing; ESeriesMissing r3_0 = Program.options.series_data_missing;

                try
                {
                    O.HandleOptionBankRef1(o0.opt_bank, o0.opt_ref); O.HandleMissing1(o0.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope0 = new O.Prt.Element();
                        ope0.labelGiven = new List<string>() { "|||a, e|||x[_[#¨e, #¨a]|[@11,26:26='x',<813>,1:26]|[@21,38:38=']',<1281>,1:38]" };
                        smpl = new GekkoSmpl(o0.t1, o0.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope0.operatorsFinal = Program.GetElementOperators(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope0.variable[bankNumber] = func158(smpl);
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
                o0.counter = 8;
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
