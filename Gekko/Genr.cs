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
        public static void ClearTS(P p) {
        }
        public static void ClearScalar(P p) {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

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
                    ope0.label22 = new List<string>() { "{'a'}|[@2,2:2='{',<1190>,1:2]|[@4,6:6='}',<1165>,1:6]" };
                    ope0.label = "[<{THIS IS A LABEL}>][@2,2:2='{',<1190>,1:2]";
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = O.Lookup(smpl, null, O.ReportLabel(smpl, null, "'a'|[@3,3:5=''a'',<1154>,1:3]|[@3,3:5=''a'',<1154>,1:3])"), null, false, EVariableType.Var);
                        O.PrtElementHandleLabel(smpl, ope0);
                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }


                o0.counter = 2;
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
