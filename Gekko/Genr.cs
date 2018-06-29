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
            Func<IVariable> func178 = () =>
            {
                var smplCommandRemember179 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp177 = new List();

                foreach (IVariable listloop_i175 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, false, EVariableType.Var)))
                {
                    foreach (IVariable listloop_s176 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, false, EVariableType.Var)))
                    {
                        O.ClearLabelHelper(smpl);
                        IVariable xxx = O.Indexer(O.Indexer2(smpl, listloop_i175, new ScalarString("ENE")), smpl, O.Lookup(smpl, null, null, "sI_y", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_i175, 0, labelCounter), O.ReportInterior(smpl, new ScalarString("ENE"), 1, labelCounter));
                        IVariable yyy = O.Indexer(O.Indexer2(smpl, listloop_i175, listloop_s176), smpl, O.Lookup(smpl, null, null, "sI_y", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_i175, 0, labelCounter), O.ReportInterior(smpl, listloop_s176, 1, labelCounter));
                        temp177.Add(O.Add(smpl, xxx, yyy));

                        O.AddLabelHelper(smpl);
                    }
                }
                smpl.command = smplCommandRemember179;
                return temp177;

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
                    ope0.label = "|||i, s|||sI_y[#i,ENE] + sI_y[#i,#s]";
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = func178();
                        if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }


                o0.counter = 30;
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
