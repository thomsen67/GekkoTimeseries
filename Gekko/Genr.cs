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
        public static void C0(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]0
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe(smpl);

            //[[commandEnd]]0
        }
        public static void C1(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]5
            p.SetText(@"¤8"); O.InitSmpl(smpl, p);


            Func<GraphHelper, string> print5 = (gh) =>
            {
                O.Prt o5 = new O.Prt();
                labelCounter = 0; o5.guiGraphIsRefreshing = gh.isRefreshing;
                o5.guiGraphOperator = gh.operator2;
                o5.guiGraphIsLogTransform = gh.isLogTransform;
                o5.prtType = "prt";
                ESeriesMissing r1_5 = Program.options.series_array_print_missing; ESeriesMissing r2_5 = Program.options.series_normal_print_missing; try
                {
                    O.HandleOptionBankRef1(o5.opt_bank, o5.opt_ref); O.HandleMissing1(o5.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope5 = new O.Prt.Element();
                        ope5.labelGiven = new List<string>() { "f¨(2)|[@70,151:151='f',<1285>,8:4]|[@74,155:155=')',<1232>,8:8]" };
                        smpl = new GekkoSmpl(o5.t1, o5.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope5.operatorsFinal = Program.GetElementOperators(o5, ope5); bankNumbers = O.Prt.GetBankNumbers(null, ope5.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope5.variable[bankNumber] = O.FunctionLookupNew3("f")(smpl, p, null, null, new GekkoArg((spml7) => i6, (spml7) => null));
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope5);
                        }
                        smpl.bankNumber = 0;
                        o5.prtElements.Add(ope5);
                    }

                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_5, r2_5);
                }
                o5.counter = 1;
                o5.printCsCounter = Globals.printCs.Count - 1;
                o5.Exe();
                return o5.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print5);
            print5(new GraphHelper());

            //[[commandEnd]]5
        }

        public static void CC0(GekkoSmpl smpl, P p, ref IVariable xfunctionarg_xf7dke8cj_1, ref IVariable xfunctionarg_xf7dke8cj_2)
        {
            IVariable functionarg_xf7dke8cj_1 = xfunctionarg_xf7dke8cj_1;

            IVariable functionarg_xf7dke8cj_2 = xfunctionarg_xf7dke8cj_2;

            //[[commandStart]]2
            p.SetText(@"¤3"); O.InitSmpl(smpl, p);


            Program.Tell(O.ConvertToString(O.HandleString(new ScalarString(@"")).Add(smpl, O.CurlyMethod(smpl, functionarg_xf7dke8cj_1)).Add(smpl, O.HandleString(new ScalarString(@"")))), false);
            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);


            Program.Tell(O.ConvertToString(O.HandleString(new ScalarString(@"")).Add(smpl, O.CurlyMethod(smpl, functionarg_xf7dke8cj_2)).Add(smpl, O.HandleString(new ScalarString(@"")))), false);
            //[[commandEnd]]3
            xfunctionarg_xf7dke8cj_1 = functionarg_xf7dke8cj_1;

            xfunctionarg_xf7dke8cj_2 = functionarg_xf7dke8cj_2;

        }

        public static readonly ScalarVal i4 = new ScalarVal(1d);
        public static void FunctionDef5()
        {

            O.PrepareUfunction(3, "f");

            Globals.ufunctionsNew3.Add("f", (GekkoSmpl smpl, P p, GekkoArg functionarg_xf7dke8cj_1_func, GekkoArg functionarg_xf7dke8cj_2_func, GekkoArg functionarg_xf7dke8cj_3_func) =>

            {
                Databank local1 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg1 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
                try
                {
                    IVariable functionarg_xf7dke8cj_1 = O.TypeCheck_date(functionarg_xf7dke8cj_1_func, smpl, 1);
                    IVariable functionarg_xf7dke8cj_2 = O.TypeCheck_date(functionarg_xf7dke8cj_2_func, smpl, 2);
                    IVariable functionarg_xf7dke8cj_3 = O.TypeCheck_val(functionarg_xf7dke8cj_3_func.f1(smpl), 3);


                    CC0(smpl, p, ref functionarg_xf7dke8cj_1, ref functionarg_xf7dke8cj_2);


        //[[commandSpecial]]4
        return O.TypeCheck_val(O.Add(smpl, functionarg_xf7dke8cj_3, i4), 0);

        //[[commandEnd]]4


        return null;
                }
                catch { p.Deeper(); throw; }
                finally
                {
                    Program.databanks.local = local1; Program.databanks.localGlobal = lg1; p.RemoveLast(); ;
                }
            });

        }

        public static readonly ScalarVal i6 = new ScalarVal(2d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);

            FunctionDef5();


            //[[commandEnd]]1


            C1(smpl, p);



        }
    }
}
