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
        public static readonly ScalarVal i17 = new ScalarVal(2d);
        public static void FunctionDef18()
        {


            //[[splitSTOP]]

            O.PrepareUfunction(1, "f");

            Globals.ufunctions1.Add("f", (GekkoSmpl smpl, P p, IVariable functionarg_15) =>
            {
                functionarg_15 = O.TypeCheck_val(functionarg_15, 1);

                p.SetText(@"¤5"); O.InitSmpl(smpl, p);

                IVariable ivTmpvar16 = O.TypeCheck_var(O.Add(smpl, functionarg_15, i17), -1);
                O.Lookup(smpl, null, "local", "%v", null, ivTmpvar16, true, EVariableType.Var)
                ;

                p.SetText(@"¤6"); O.InitSmpl(smpl, p);


                //[[splitSTOP]]
                return O.TypeCheck_val(O.Lookup(smpl, null, "local", "%v", null, null, false, EVariableType.Var), 0);

                //[[splitSTART]]


                return null;
            });


            //[[splitSTART]]

        }

        public static readonly ScalarVal i20 = new ScalarVal(777d);
        public static readonly ScalarVal i21 = new ScalarVal(100d);
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

            O.Mode o1 = new O.Mode();
            o1.mode = @"data"; o1.Exe();

            FunctionDef18();


            p.SetText(@"¤9"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar19 = O.TypeCheck_var(i20, -1);
            O.Lookup(smpl, null, "local", "%v", null, ivTmpvar19, true, EVariableType.Var)
            ;

            p.SetText(@"¤10"); O.InitSmpl(smpl, p);

            Func<GraphHelper, string> print6 = (gh) =>
            {
                O.Prt o6 = new O.Prt();
                labelCounter = 0; o6.guiGraphIsRefreshing = gh.isRefreshing;
                o6.guiGraphPrintCode = gh.printCode;
                o6.guiGraphIsLogTransform = gh.isLogTransform;
                o6.prtType = "prt";

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope6 = new O.Prt.Element();
                    ope6.labelGiven = new List<string>() { "%¨v|[@65,124:124='%',<1171>,10:4]|[@67,126:126='v',<1128>,10:6]" };
                    smpl = new GekkoSmpl(o6.t1.Add(-2), o6.t2);
                    ope6.printCodesFinal = Program.GetElementPrintCodes(o6, ope6); bankNumbers = O.Prt.GetBankNumbers(null, ope6.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope6.variable[bankNumber] = O.Lookup(smpl, null, null, "%v", null, null, false, EVariableType.Var);
                        O.PrtElementHandleLabel(smpl, ope6);
                    }
                    smpl.bankNumber = 0;
                    o6.prtElements.Add(ope6);
                }


                o6.counter = 7;
                o6.printCsCounter = Globals.printCs.Count - 1;
                o6.Exe();
                return o6.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print6);
            print6(new GraphHelper());

            p.SetText(@"¤12"); O.InitSmpl(smpl, p);

            Func<GraphHelper, string> print7 = (gh) =>
            {
                O.Prt o7 = new O.Prt();
                labelCounter = 0; o7.guiGraphIsRefreshing = gh.isRefreshing;
                o7.guiGraphPrintCode = gh.printCode;
                o7.guiGraphIsLogTransform = gh.isLogTransform;
                o7.prtType = "print";

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope7 = new O.Prt.Element();
                    ope7.labelGiven = new List<string>() { "f¨(100)|[@72,138:138='f',<1203>,12:6]|[@76,144:144=')',<1157>,12:12]" };
                    smpl = new GekkoSmpl(o7.t1.Add(-2), o7.t2);
                    ope7.printCodesFinal = Program.GetElementPrintCodes(o7, ope7); bankNumbers = O.Prt.GetBankNumbers(null, ope7.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope7.variable[bankNumber] =

                        O.FunctionLookup1("f")(smpl, p, i21).Add(smpl, O.FunctionLookup1("f")(smpl, p, i21));


                        O.PrtElementHandleLabel(smpl, ope7);
                    }
                    smpl.bankNumber = 0;
                    o7.prtElements.Add(ope7);
                }


                o7.counter = 8;
                o7.printCsCounter = Globals.printCs.Count - 1;
                o7.Exe();
                return o7.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print7);
            print7(new GraphHelper());

            p.SetText(@"¤13"); O.InitSmpl(smpl, p);

            Func<GraphHelper, string> print8 = (gh) =>
            {
                O.Prt o8 = new O.Prt();
                labelCounter = 0; o8.guiGraphIsRefreshing = gh.isRefreshing;
                o8.guiGraphPrintCode = gh.printCode;
                o8.guiGraphIsLogTransform = gh.isLogTransform;
                o8.prtType = "prt";

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope8 = new O.Prt.Element();
                    ope8.labelGiven = new List<string>() { "%¨v|[@81,152:152='%',<1171>,13:4]|[@83,154:154='v',<1128>,13:6]" };
                    smpl = new GekkoSmpl(o8.t1.Add(-2), o8.t2);
                    ope8.printCodesFinal = Program.GetElementPrintCodes(o8, ope8); bankNumbers = O.Prt.GetBankNumbers(null, ope8.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope8.variable[bankNumber] = O.Lookup(smpl, null, null, "%v", null, null, false, EVariableType.Var);
                        O.PrtElementHandleLabel(smpl, ope8);
                    }
                    smpl.bankNumber = 0;
                    o8.prtElements.Add(ope8);
                }


                o8.counter = 9;
                o8.printCsCounter = Globals.printCs.Count - 1;
                o8.Exe();
                return o8.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print8);
            print8(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
