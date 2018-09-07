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
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);
            Func<IVariable> func24 = () =>
            {
                var smplCommandRemember25 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp23 = new List();

                foreach (IVariable listloop_i22 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, false, EVariableType.Var)))
                {
                    temp23.Add(O.Indexer(O.Indexer2(smpl, listloop_i22), smpl, O.Lookup(smpl, null, null, "y", null, null, false, EVariableType.Var), O.ReportLabel(smpl, listloop_i22, "#¨i|[@4,6:6='#',<1174>,1:6]|[@6,8:8='i',<1213>,1:8]")));

                }
                smpl.command = smplCommandRemember25;
                return temp23;

            };


            Func<GraphHelper, string> print0 = (gh) =>
            {
                O.Prt o0 = new O.Prt();
                labelCounter = 0; o0.guiGraphIsRefreshing = gh.isRefreshing;
                o0.guiGraphPrintCode = gh.printCode;
                o0.guiGraphIsLogTransform = gh.isLogTransform;
                o0.prtType = "p";
                ESeriesMissing r1_0 = Program.options.series_array_print_missing; ESeriesMissing r2_0 = Program.options.series_normal_print_missing; try
                {
                    O.HandleOptionBankRef1(o0.opt_bank, o0.opt_ref); O.HandleMissing1(o0.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope0 = new O.Prt.Element();
                        ope0.labelGiven = new List<string>() { "|||i|||y[_[#¨i]|[@2,2:2='y',<730>,1:2]|[@7,9:9=']',<1170>,1:9]" };
                        smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                        ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope0.variable[bankNumber] = func24();
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
                o0.counter = 2;
                o0.printCsCounter = Globals.printCs.Count - 1;
                o0.Exe();
                return o0.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print0);
            print0(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
