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
            Func<IVariable> func152 = () =>
            {
                var smplCommandRemember153 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp151 = new Series(ESeriesType.Normal, Program.options.freq, null); temp151.SetZero(smpl);

                foreach (IVariable listloop_m1150 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1")))), null, false, EVariableType.Var)))
                {
                    GekkoSmpl2 xxx = O.Indexer2(smpl, listloop_m1150, O.ReportLabel(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false)), "'x'|[@21,28:30=''x'',<1154>,1:28]|[@21,28:30=''x'',<1154>,1:28]"));
                    //#m1
                    IVariable report1 = O.ReportLabel(smpl, listloop_m1150, "#¨m1|[@16,22:22='#',<1159>,1:22]|[@18,24:25='m1',<1198>,1:24]");
                    //'x'
                    IVariable report3 = O.ReportLabel(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false)), "'x'|[@21,28:30=''x'',<1154>,1:28]|[@21,28:30=''x'',<1154>,1:28]");
                    //'x'
                    IVariable report2 = O.ReportLabel(smpl, report3, "'x'|[@21,28:30=''x'',<1154>,1:28]|[@21,28:30=''x'',<1154>,1:28]");
                    temp151.InjectAdd(smpl, temp151, O.Indexer(xxx, smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), report1, report2));
                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember153;
                return temp151;

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
                    ope0.labelGiven = new List<string>() { "sum¨(#¨m1, xx[_[#¨m1, 'x'])|[@6,6:8='sum',<1198>,1:6]|[@23,32:32=')',<1152>,1:32]" };
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = func152();
                        O.PrtElementHandleLabel(smpl, ope0);
                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }


                o0.counter = 10;
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
