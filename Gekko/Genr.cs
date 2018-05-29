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
        public static readonly ScalarVal i25 = new ScalarVal(1d);
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
            Func<IVariable> func23 = () =>
            {
                var smplCommandRemember24 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp22 = new List();

                foreach (IVariable listloop_a21 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, false, EVariableType.Var)))
                {
                    O.ClearLabelHelper(smpl);
                    temp22.Add(O.Lookup(smpl, null, (new ScalarString("x")).Add(smpl, O.ReportInterior(smpl, listloop_a21, 0, labelCounter)), null, false, EVariableType.Var));

                    O.AddLabelHelper(smpl);
                }
                smpl.command = smplCommandRemember24;
                return temp22;

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

                //!!
                List<string> labels2 = null;

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope0 = new O.Prt.Element();
                    ope0.label = O.SubstituteScalarsAndLists("|||a|||x{#a}", false);
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);

                    //!!
                    labels2 = new List<string>();

                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal); foreach (int bankNumber in bankNumbers)
                    {   

                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = func23();
                        
                        //!!
                        Program.UnfoldLabels(ope0.label, ref labels2, smpl.labelHelper2);
                        ope0.label2 = labels2.ToArray();

                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }

                //!!
                labels2 = new List<string>();
                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope0 = new O.Prt.Element();
                    ope0.label = O.SubstituteScalarsAndLists("1", false);
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal); foreach (int bankNumber in bankNumbers)
                    {
                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = i25;

                        //!!
                        Program.UnfoldLabels(ope0.label, ref labels2, smpl.labelHelper2);
                        ope0.label2 = labels2.ToArray();
                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }


                o0.counter = 2;
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
