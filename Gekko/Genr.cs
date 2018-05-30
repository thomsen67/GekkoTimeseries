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
            Func<IVariable> func69 = () =>
            {
                var smplCommandRemember70 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp68 = new List();

                foreach (IVariable listloop_a67 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, false, EVariableType.Var)))
                {
                    O.ClearLabelHelper(smpl);
                    temp68.Add(O.Lookup(smpl, null, (O.ReportInterior(smpl, listloop_a67, 0, labelCounter)), null, false, EVariableType.Var));

                    O.AddLabelHelper(smpl);
                }
                smpl.command = smplCommandRemember70;
                return temp68;

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
                    ope0.label = O.SubstituteScalarsAndLists("|||a|||{#a}", false);
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal); foreach (int bankNumber in bankNumbers)
                    {
                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = func69();
                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope0 = new O.Prt.Element();
                    ope0.label = O.SubstituteScalarsAndLists("{%s}", false);
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal); foreach (int bankNumber in bankNumbers)
                    {
                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = O.Lookup(smpl, null, (O.ReportInterior(smpl, O.Lookup(smpl, null, null, "%s", null, null, false, EVariableType.Var), 0, labelCounter)), null, false, EVariableType.Var);
                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }


                o0.counter = 9;
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
