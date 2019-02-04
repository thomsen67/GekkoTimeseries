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
        public static void C0(GekkoSmpl smpl, P p) {
            //[[commandStart]]2
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);


            O.Assignment o2 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar4 = i5;
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "y", null, ivTmpvar4, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
            ;

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤3"); O.InitSmpl(smpl, p);


            Func<GraphHelper, string> print3 = (gh) =>
            {
                O.Prt o3 = new O.Prt();
                labelCounter = 0; o3.guiGraphIsRefreshing = gh.isRefreshing;
                o3.guiGraphPrintCode = gh.printCode;
                o3.guiGraphIsLogTransform = gh.isLogTransform;
                o3.prtType = "prt";
                ESeriesMissing r1_3 = Program.options.series_array_print_missing; ESeriesMissing r2_3 = Program.options.series_normal_print_missing; try {
                    O.HandleOptionBankRef1(o3.opt_bank, o3.opt_ref); O.HandleMissing1(o3.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope3 = new O.Prt.Element();
                        ope3.labelGiven = new List<string>() { "f¨(y)|[@34,64:64='f',<1264>,3:4]|[@38,68:68=')',<1211>,3:8]" };
                        smpl = new GekkoSmpl(o3.t1, o3.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope3.printCodesFinal = Program.GetElementPrintCodes(o3, ope3); bankNumbers = O.Prt.GetBankNumbers(null, ope3.printCodesFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope3.variable[bankNumber] = O.FunctionLookupNew1("f")(smpl, p, new GekkoArg((smpl777) => O.Lookup(smpl777, null, null, "y", null, null, new LookupSettings(), EVariableType.Var, null), (smpl777) => new ScalarString("y")));
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope3);
                        }
                        smpl.bankNumber = 0;
                        o3.prtElements.Add(ope3);
                    }

                }
                finally {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_3, r2_3);
                }
                o3.counter = 1;
                o3.printCsCounter = Globals.printCs.Count - 1;
                o3.Exe();
                return o3.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print3);
            print3(new GraphHelper());

            //[[commandEnd]]3
        }


        public static readonly ScalarVal i2 = new ScalarVal(2d);
        public static void FunctionDef3() {

            O.PrepareUfunction(1, "f");

            Globals.ufunctionsNew1.Add("f", (GekkoSmpl smpl, P p, GekkoArg functionarg_xf7dke8cj_1_func) => 

{ Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
                try {
                    IVariable functionarg_xf7dke8cj_1 = O.TypeCheck_series(functionarg_xf7dke8cj_1_func.f1(smpl), 1);
                    functionarg_xf7dke8cj_1 = O.TypeCheck_series(functionarg_xf7dke8cj_1, 1);


                    //[[commandSpecial]]1
                    return O.TypeCheck_series(O.Multiply(smpl, i2, functionarg_xf7dke8cj_1), 0);

                    //[[commandEnd]]1


                    return null;
                }
                catch { p.Deeper(); throw; }
                finally {
                    Program.databanks.local = local0; Program.databanks.localGlobal = lg0; p.RemoveLast(); ;
                }
            });

        }

        public static readonly ScalarVal i5 = new ScalarVal(100d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
            FunctionDef3();


            //[[commandEnd]]0


            C0(smpl, p);



        }
    }
}
