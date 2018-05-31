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
            Func<IVariable, IVariable, IVariable, IVariable> func76 = (IVariable listloop_a70, IVariable listloop_b71, IVariable listloop_c72) =>
            {
                var smplCommandRemember77 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp75 = new Series(ESeriesType.Normal, Program.options.freq, null); temp75.SetZero(smpl);

                foreach (IVariable listloop_a73 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, false, EVariableType.Var)))
                {
                    foreach (IVariable listloop_b74 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("b")))), null, false, EVariableType.Var)))
                    {
                        temp75.InjectAdd(smpl, temp75, O.Indexer(O.Indexer2(smpl, listloop_a73, listloop_b74, listloop_c72), smpl, O.Lookup(smpl, null, null, "x3", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_a73, 0, labelCounter), O.ReportInterior(smpl, listloop_b74, 1, labelCounter), O.ReportInterior(smpl, listloop_c72, 2, labelCounter)));

                        labelCounter++;
                    }
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember77;
                return temp75;

            };

            Func<IVariable> func79 = () =>
            {
                var smplCommandRemember80 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp78 = new List();

                foreach (IVariable listloop_a70 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, false, EVariableType.Var)))
                {
                    foreach (IVariable listloop_b71 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("b")))), null, false, EVariableType.Var)))
                    {
                        foreach (IVariable listloop_c72 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("c")))), null, false, EVariableType.Var)))
                        {
                            O.ClearLabelHelper(smpl);
                            temp78.Add(O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, listloop_a70), smpl, O.Lookup(smpl, null, (
                                O.Lookup(smpl, null, null, "%s", null, null, false, EVariableType.Var)
                                ), null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_a70, 0, labelCounter)), O.Indexer(O.Indexer2(smpl, listloop_b71), smpl, O.Lookup(smpl, null, null, "x2", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_b71, 0, labelCounter))), func76(listloop_a70, listloop_b71, listloop_c72)));

                            O.AddLabelHelper(smpl);
                        }
                    }
                }
                smpl.command = smplCommandRemember80;
                return temp78;

            };


            Func<GraphHelper, string> print0 = (gh) =>
            {
                O.Prt o0 = new O.Prt();
                int labelCounter = 0; o0.guiGraphIsRefreshing = gh.isRefreshing;
                o0.guiGraphPrintCode = gh.printCode;
                o0.guiGraphIsLogTransform = gh.isLogTransform;
                o0.prtType = "p";

                o0.t1 = Globals.globalPeriodStart;
                o0.t2 = Globals.globalPeriodEnd;

                o0.printCodes.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));



                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope0 = new O.Prt.Element();
                    ope0.label = "|||a, b, c|||{%s}[#a] + x2[#b] + sum((#a, #b), x3[#a, #b, #c])";
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = func79();
                        if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }


                o0.counter = 4;
                o0.printCsCounter = Globals.printCs.Count - 1;
                o0.labelHelper2 = O.AddLabelHelper2(smpl);
                o0.Exe();
                return o0.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print0);
            print0(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
