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
            //[[commandStart]]3
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);

            O.Assignment o3 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar20 = i21;
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%v", null, ivTmpvar20, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
            ;

            //[[commandEnd]]3
        }
        public static void C1(GekkoSmpl smpl, P p) {
            //[[commandStart]]5
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);

            Func<GraphHelper, string> print5 = (gh) =>
            {
                O.Prt o5 = new O.Prt();
                labelCounter = 0; o5.guiGraphIsRefreshing = gh.isRefreshing;
                o5.guiGraphPrintCode = gh.printCode;
                o5.guiGraphIsLogTransform = gh.isLogTransform;
                o5.prtType = "p";
                ESeriesMissing r1_5 = Program.options.series_array_print_missing; ESeriesMissing r2_5 = Program.options.series_normal_print_missing; try {
                    O.HandleOptionBankRef1(o5.opt_bank, o5.opt_ref); O.HandleMissing1(o5.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope5 = new O.Prt.Element();
                        ope5.labelGiven = new List<string>() { "%¨q|[@54,87:87='%',<1217>,4:2]|[@56,89:89='q',<1049>,4:4]" };
                        smpl = new GekkoSmpl(o5.t1, o5.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope5.printCodesFinal = Program.GetElementPrintCodes(o5, ope5); bankNumbers = O.Prt.GetBankNumbers(null, ope5.printCodesFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope5.variable[bankNumber] = O.Lookup(smpl, null, null, "%q", null, null, new LookupSettings(), EVariableType.Var, null);
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope5);
                        }
                        smpl.bankNumber = 0;
                        o5.prtElements.Add(ope5);
                    }

                }
                finally {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_5, r2_5);
                }
                o5.counter = 3;
                o5.printCsCounter = Globals.printCs.Count - 1;
                o5.Exe();
                return o5.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print5);
            print5(new GraphHelper());

            //[[commandEnd]]5
        }

        public static void CC0(GekkoSmpl smpl, P p) {
            //[[commandStart]]1
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            Program.Tell(O.ConvertToString(O.HandleString(new ScalarString(@"adsf"))), false);
            //[[commandEnd]]1
        }

        public static readonly ScalarVal i18 = new ScalarVal(100d);
        public static void FunctionDef19() {

            O.PrepareUfunction(2, "f");

            Globals.ufunctions2.Add("f", (GekkoSmpl smpl, P p, IVariable functionarg_xf7dke8cj_16, IVariable functionarg_xf7dke8cj_17) =>

            { Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal();
                try {
                    functionarg_xf7dke8cj_16 = O.TypeCheck_val(functionarg_xf7dke8cj_16, 1);
                    functionarg_xf7dke8cj_17 = O.TypeCheck_val(functionarg_xf7dke8cj_17, 2);


                    CC0(smpl, p);


        //[[commandSpecial]]2
        return O.TypeCheck_val(i18, 0);

        //[[commandEnd]]2


        return null;
                }
                finally {
                    Program.databanks.local = local0; Program.databanks.localGlobal = lg0;
                }
            });

        }

        public static readonly ScalarVal i21 = new ScalarVal(1d);
        public static readonly ScalarVal i22 = new ScalarVal(2d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
            FunctionDef19();


            //[[commandEnd]]0


            C0(smpl, p);

            O.FunctionLookup2("f")(smpl, p, O.Lookup(smpl, null, null, "%v", null, null, new LookupSettings(), EVariableType.Var, null), i22);
        //[[commandEnd]]4


C1(smpl, p);



        }
    }
}
