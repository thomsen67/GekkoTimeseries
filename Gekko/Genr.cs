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
            Func<IVariable> func153 = () =>
            {
                var smplCommandRemember154 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp152 = new Series(ESeriesType.Normal, Program.options.freq, null); temp152.SetZero(smpl);

                foreach (IVariable listloop_m1150 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1")))), null, false, EVariableType.Var)))
                {
                    foreach (IVariable listloop_m2151 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2")))), null, false, EVariableType.Var)))
                    {
                        temp152.InjectAdd(smpl, temp152, O.Dollar(smpl, O.Indexer(O.Indexer2(smpl, listloop_m1150, listloop_m2151), smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), listloop_m1150, listloop_m2151), O.ListContains(O.Lookup(smpl, null, null, "#m3", null, null, false, EVariableType.Var), O.ReportLabel(smpl, listloop_m1150, "lkjdfalkdsjf"))
                        ));

                        labelCounter++;
                    }
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember154;
                return temp152;

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



                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope0 = new O.Prt.Element();
                    ope0.labelGiven = new List<string>() { "sum¨((#¨m1, #¨m2), xx[_[#¨m1, #¨m2] $ #¨m3[_[#¨m1])|[@6,6:8='sum',<1198>,1:6]|[@43,56:56=')',<1152>,1:56]" };
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = func153();
                        O.PrtElementHandleLabel(smpl, ope0);
                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }


                o0.counter = 8;
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
