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
        public static void FunctionDef3()
        {


            //[[splitSTOP]]

            O.PrepareUfunction(2, "f");

            Globals.ufunctions2.Add("f", (GekkoSmpl smpl, P p, IVariable functionarg_1, IVariable functionarg_2) =>
            {
                functionarg_1 = O.TypeCheck_val(functionarg_1, 0);
                functionarg_2 = O.TypeCheck_val(functionarg_2, 0);

                p.SetText(@"¤3"); O.InitSmpl(smpl, p);


                //[[splitSTOP]]
                return O.Add(smpl, functionarg_1, functionarg_2);

                //[[splitSTART]]


                return null;
            });


            //[[splitSTART]]

        }

        public static readonly ScalarVal i4 = new ScalarVal(2d);
        public static readonly ScalarVal i5 = new ScalarVal(3d);
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

            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe(smpl);

            p.SetText(@"¤2"); O.InitSmpl(smpl, p);

            FunctionDef3();


            p.SetText(@"¤6"); O.InitSmpl(smpl, p);

            Func<GraphHelper, string> print3 = (gh) =>
            {
                O.Prt o3 = new O.Prt();
                int labelCounter = 0; o3.guiGraphIsRefreshing = gh.isRefreshing;
                o3.guiGraphPrintCode = gh.printCode;
                o3.guiGraphIsLogTransform = gh.isLogTransform;
                o3.prtType = "p";

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope3 = new O.Prt.Element();
                    ope3.label = "f(2, 3)";
                    smpl = new GekkoSmpl(o3.t1.Add(-2), o3.t2);
                    ope3.printCodesFinal = Program.GetElementPrintCodes(o3, ope3); bankNumbers = O.Prt.GetBankNumbers(null, ope3.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope3.variable[bankNumber] = O.FunctionLookup2("f")(smpl, p, i4, i5);
                        if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope3);
                    }
                    smpl.bankNumber = 0;
                    o3.prtElements.Add(ope3);
                }


                o3.counter = 1;
                o3.printCsCounter = Globals.printCs.Count - 1;
                o3.labelHelper2 = O.AddLabelHelper2(smpl);
                o3.Exe();
                return o3.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print3);
            print3(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
