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
        public static readonly ScalarVal i6 = new ScalarVal(1d);
        public static readonly ScalarVal i8 = new ScalarVal(4d);
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
            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar5 = O.TypeCheck_var(Functions.series(smpl, i6), -1);
            O.Lookup(smpl, null, null, "xab", null, ivTmpvar5, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar7 = O.TypeCheck_var(i8, -1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xab", null, null, true, EVariableType.Var), ivTmpvar7, new ScalarString("cd"))
            ;

            p.SetText(@"¤3"); O.InitSmpl(smpl, p);

            Func<GraphHelper, string> print2 = (gh) =>
            {
                O.Prt o2 = new O.Prt();
                labelCounter = 0; o2.guiGraphIsRefreshing = gh.isRefreshing;
                o2.guiGraphPrintCode = gh.printCode;
                o2.guiGraphIsLogTransform = gh.isLogTransform;
                o2.prtType = "p";

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope2 = new O.Prt.Element();
                    ope2.label = "x{'a'+'b'}['c'+'d']";
                    smpl = new GekkoSmpl(o2.t1.Add(-2), o2.t2);
                    ope2.printCodesFinal = Program.GetElementPrintCodes(o2, ope2); bankNumbers = O.Prt.GetBankNumbers(null, ope2.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope2.variable[bankNumber] = O.Indexer(O.Indexer2(smpl, O.ReportLabel(smpl, O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"d", true, false)))
            , "'c'+'d'|[@27,47:49=''c'',<1154>,3:16]|[@29,51:53=''d'',<1154>,3:20]")), smpl, O.Lookup(smpl, null, (new ScalarString("x")).Add(smpl, O.ReportLabel(smpl, O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))), "'a'+'b'|[@22,36:38=''a'',<1154>,3:5]|[@24,40:42=''b'',<1154>,3:9]")), null, false, EVariableType.Var), O.ReportLabel(smpl, O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"d", true, false)))
            , "'c'+'d'|[@27,47:49=''c'',<1154>,3:16]|[@29,51:53=''d'',<1154>,3:20]"));
                        if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope2);
                    }
                    smpl.bankNumber = 0;
                    o2.prtElements.Add(ope2);
                }


                o2.counter = 2;
                o2.printCsCounter = Globals.printCs.Count - 1;
                o2.labelHelper2 = O.AddLabelHelper2(smpl);
                o2.labelHelper22 = O.AddLabelHelper22(smpl);
                o2.Exe();
                return o2.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print2);
            print2(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
