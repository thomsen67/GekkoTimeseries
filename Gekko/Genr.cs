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
            Func<IVariable> func95 = () =>
            {
                var smplCommandRemember96 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp94 = new List();

                foreach (IVariable listloop_d93 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("d")))), null, false, EVariableType.Var)))
                {
                    O.ClearLabelHelper(smpl);
                    temp94.Add(O.Lookup(smpl, null, (O.ReportLabel(smpl, listloop_d93, "#¨d|[@3,3:3='#',<1159>,1:3]|[@5,5:5='d',<798>,1:5]")), null, false, EVariableType.Var));

                    O.AddLabelHelper(smpl);
                }
                smpl.command = smplCommandRemember96;
                return temp94;

            };

            Func<IVariable> func99 = () =>
            {
                var smplCommandRemember100 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp98 = new List();

                foreach (IVariable listloop_x97 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, false, EVariableType.Var)))
                {
                    O.ClearLabelHelper(smpl);
                    temp98.Add(O.Lookup(smpl, null, (O.ReportLabel(smpl, listloop_x97, "#¨x|[@9,9:9='#',<1159>,1:9]|[@11,11:11='x',<721>,1:11]")), null, false, EVariableType.Var));

                    O.AddLabelHelper(smpl);
                }
                smpl.command = smplCommandRemember100;
                return temp98;

            };

            Func<IVariable> func104 = () =>
            {
                var smplCommandRemember105 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp103 = new List();

                foreach (IVariable listloop_d101 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("d")))), null, false, EVariableType.Var)))
                {
                    foreach (IVariable listloop_x102 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("x")))), null, false, EVariableType.Var)))
                    {
                        O.ClearLabelHelper(smpl);
                        temp103.Add(O.Lookup(smpl, null, (O.ReportLabel(smpl, listloop_d101, "#¨d|[@15,15:15='#',<1159>,1:15]|[@17,17:17='d',<798>,1:17]")).Add(smpl, O.ReportLabel(smpl, listloop_x102, "#¨x|[@21,21:21='#',<1159>,1:21]|[@23,23:23='x',<721>,1:23]")), null, false, EVariableType.Var));

                        O.AddLabelHelper(smpl);
                    }
                }
                smpl.command = smplCommandRemember105;
                return temp103;

            };


            Func<GraphHelper, string> print0 = (gh) =>
            {
                O.Prt o0 = new O.Prt();
                labelCounter = 0; o0.guiGraphIsRefreshing = gh.isRefreshing;
                o0.guiGraphPrintCode = gh.printCode;
                o0.guiGraphIsLogTransform = gh.isLogTransform;
                o0.prtType = "p";

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope0 = new O.Prt.Element();
                    ope0.label22 = new List<string>() { "|||d|||{#¨d}|[@2,2:2='{',<1190>,1:2]|[@6,6:6='}',<1165>,1:6]" };
                    ope0.label = "|||d|||[<{THIS IS A LABEL}>][@2,2:2='{',<1190>,1:2]";
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = func95();
                        O.PrtElementHandleLabel(smpl, ope0);
                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope0 = new O.Prt.Element();
                    ope0.label22 = new List<string>() { "|||x|||{#¨x}|[@8,8:8='{',<1190>,1:8]|[@12,12:12='}',<1165>,1:12]" };
                    ope0.label = "|||x|||[<{THIS IS A LABEL}>][@8,8:8='{',<1190>,1:8]";
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = func99();
                        O.PrtElementHandleLabel(smpl, ope0);
                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope0 = new O.Prt.Element();
                    ope0.label22 = new List<string>() { "|||d, x|||{#¨d}¨{#¨x}|[@14,14:14='{',<1190>,1:14]|[@24,24:24='}',<1165>,1:24]" };
                    ope0.label = "|||d, x|||[<{THIS IS A LABEL}>][@14,14:14='{',<1190>,1:14]";
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = func104();
                        O.PrtElementHandleLabel(smpl, ope0);
                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }


                o0.counter = 3;
                o0.printCsCounter = Globals.printCs.Count - 1;
                o0.labelHelper2 = O.AddLabelHelper2(smpl);
                o0.labelHelper22 = O.AddLabelHelper22(smpl);
                o0.Exe();
                return o0.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print0);
            print0(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
