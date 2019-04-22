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
            p.SetText(@"¤9"); O.InitSmpl(smpl, p);

            O.Assignment o5 = new O.Assignment();
            o5.opt_source = @"<[code]>#m1 = (1, 2, 3)";


            Action check_35 = () =>
            {
                O.Lookup(smpl, null, null, "#m1", null, Globals.scalarValMissing, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o5)
                ;
            };
            Action assign_35 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar31 = O.ListDefHelper(i32, null, i33, null, i34, null);
                O.AdjustT0(smpl, 2);
                if (O.Dynamic6(smpl)) return;
                O.Lookup(smpl, null, null, "#m1", null, ivTmpvar31, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o5)
                ;
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_35, check_35, false, o5);

            //[[commandEnd]]5


            //[[commandStart]]6
            p.SetText(@"¤10"); O.InitSmpl(smpl, p);

            O.Assignment o6 = new O.Assignment();

            IVariable iv1 = Program.databanks.GetFirst().GetIVariable("#m1");

            o6.opt_source = @"<[code]>#m1 = plus(#m1, 100)";

            Action check_40 = () =>
            {
                O.Lookup(smpl, null, null, "#m1", null, Globals.scalarValMissing, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o6)
                ;
            };
            Action assign_40 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar36 = O.FunctionLookupNew4("plus")(smpl, p, null, null, new GekkoArg((spml38) => O.Lookup(spml38, null, null, "#m1", null, null, new LookupSettings(), EVariableType.Var, null), (spml38) => new ScalarString("#m1")), new GekkoArg((spml39) => i37, (spml39) => null));
                O.AdjustT0(smpl, 2);
                if (O.Dynamic6(smpl)) return;
                O.Lookup(smpl, null, null, "#m1", null, ivTmpvar36, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o6)
                ;
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_40, check_40, false, o6);

            IVariable iv2 = Program.databanks.GetFirst().GetIVariable("#m1");

            //[[commandEnd]]6


            //[[commandStart]]7
            p.SetText(@"¤12"); O.InitSmpl(smpl, p);


            Func<GraphHelper, string> print7 = (gh) =>
            {
                O.Prt o7 = new O.Prt();
                labelCounter = 0; o7.guiGraphIsRefreshing = gh.isRefreshing;
                o7.guiGraphOperator = gh.operator2;
                o7.guiGraphIsLogTransform = gh.isLogTransform;
                o7.prtType = "print";
                ESeriesMissing r1_7 = Program.options.series_array_print_missing; ESeriesMissing r2_7 = Program.options.series_normal_print_missing; try
                {
                    O.HandleOptionBankRef1(o7.opt_bank, o7.opt_ref); O.HandleMissing1(o7.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope7 = new O.Prt.Element();
                        ope7.labelGiven = new List<string>() { "#¨m1|[@127,204:204='#',<1253>,12:6]|[@129,206:207='m1',<1295>,12:8]" };
                        smpl = new GekkoSmpl(o7.t1, o7.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope7.operatorsFinal = Program.GetElementOperators(o7, ope7); bankNumbers = O.Prt.GetBankNumbers(null, ope7.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope7.variable[bankNumber] = O.Lookup(smpl, null, null, "#m1", null, null, new LookupSettings(), EVariableType.Var, null);
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope7);
                        }
                        smpl.bankNumber = 0;
                        o7.prtElements.Add(ope7);
                    }

                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_7, r2_7);
                }
                o7.counter = 3;
                o7.printCsCounter = Globals.printCs.Count - 1;
                o7.Exe();
                return o7.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print7);
            print7(new GraphHelper());

            //[[commandEnd]]7
        }

        public static void CC0(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_25, ref IVariable xfunctionarg_xf7dke8cj_23, ref IVariable xfunctionarg_xf7dke8cj_24)
        {
            IVariable forloop_xe7dke6cj_25 = xforloop_xe7dke6cj_25;

            IVariable functionarg_xf7dke8cj_23 = xfunctionarg_xf7dke8cj_23;

            IVariable functionarg_xf7dke8cj_24 = xfunctionarg_xf7dke8cj_24;

            //[[commandStart]]3
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);

            O.Assignment o3 = new O.Assignment();
            o3.opt_source = @"<[code]>#m[%i] = #m[%i] + %v";


            Action check_28 = () =>
            {
                O.IndexerSetData(smpl, functionarg_xf7dke8cj_23, Globals.scalarValMissing, o3, forloop_xe7dke6cj_25
                )
                ;
            };
            Action assign_28 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar27 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, forloop_xe7dke6cj_25
                ), smpl, O.EIndexerType.None, functionarg_xf7dke8cj_23, forloop_xe7dke6cj_25
                ), functionarg_xf7dke8cj_24);
                O.AdjustT0(smpl, 2);
                if (O.Dynamic6(smpl)) return;
                O.IndexerSetData(smpl, functionarg_xf7dke8cj_23, ivTmpvar27, o3, forloop_xe7dke6cj_25
                )
                ;
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_28, check_28, false, o3);

            //[[commandEnd]]3
            xforloop_xe7dke6cj_25 = forloop_xe7dke6cj_25;

            xfunctionarg_xf7dke8cj_23 = functionarg_xf7dke8cj_23;

            xfunctionarg_xf7dke8cj_24 = functionarg_xf7dke8cj_24;

        }

        public static readonly ScalarVal i26 = new ScalarVal(1d);
        public static void FunctionDef30()
        {

            O.PrepareUfunction(4, "plus");

            Globals.ufunctionsNew4.Add("plus", (GekkoSmpl smpl, P p, GekkoArg functionarg_xf7dke8cj_21_func, GekkoArg functionarg_xf7dke8cj_22_func, GekkoArg functionarg_xf7dke8cj_23_func, GekkoArg functionarg_xf7dke8cj_24_func) =>

            {
                Databank local1 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg1 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("plus"); p.SetLastFileSentToANTLR(O.LastText("plus")); p.Deeper();
                try
                {
                    IVariable functionarg_xf7dke8cj_21 = O.TypeCheck_date(functionarg_xf7dke8cj_21_func, smpl, 1);
                    IVariable functionarg_xf7dke8cj_22 = O.TypeCheck_date(functionarg_xf7dke8cj_22_func, smpl, 2);
                    IVariable functionarg_xf7dke8cj_23 = O.TypeCheck_list(functionarg_xf7dke8cj_23_func.f1(smpl), 3);
                    IVariable functionarg_xf7dke8cj_24 = O.TypeCheck_val(functionarg_xf7dke8cj_24_func.f1(smpl), 4);


        //[[commandSpecial]]2
        IVariable forloop_xe7dke6cj_25 = null;
                    int counter29 = 0;
                    for (O.IterateStart(O.ELoopType.ForTo, ref forloop_xe7dke6cj_25, i26); O.IterateContinue(O.ELoopType.ForTo, forloop_xe7dke6cj_25, i26, Functions.len(smpl, null, null, functionarg_xf7dke8cj_23), null, ref counter29); O.IterateStep(O.ELoopType.ForTo, ref forloop_xe7dke6cj_25, i26, null, counter29))
                    {
                        ;

                        CC0(smpl, p, ref forloop_xe7dke6cj_25, ref functionarg_xf7dke8cj_23, ref functionarg_xf7dke8cj_24);

                    };

        //[[commandEnd]]2


        //[[commandSpecial]]4
        return O.TypeCheck_list(functionarg_xf7dke8cj_23, 0);

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

        public static readonly ScalarVal i32 = new ScalarVal(1d);
        public static readonly ScalarVal i33 = new ScalarVal(2d);
        public static readonly ScalarVal i34 = new ScalarVal(3d);
        public static readonly ScalarVal i37 = new ScalarVal(100d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);

            FunctionDef30();


            //[[commandEnd]]1


            C1(smpl, p);



        }
    }
}
