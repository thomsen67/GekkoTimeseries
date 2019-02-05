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
            //[[commandStart]]0
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe(smpl);

            //[[commandEnd]]0
        }
        public static void C1(GekkoSmpl smpl, P p) {
            //[[commandStart]]9
            p.SetText(@"¤16"); O.InitSmpl(smpl, p);


            O.Assignment o9 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar68 = O.ListDefHelper(i69, null, i70, null, i71, null);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "#m4", null, ivTmpvar68, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o9)
            ;

            //[[commandEnd]]9


            //[[commandStart]]10
            p.SetText(@"¤17"); O.InitSmpl(smpl, p);


            O.Assignment o10 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar72 = O.FunctionLookupNew2("mul")(smpl, p, new GekkoArg((spml78) => O.FunctionLookupNew2("plus")(spml78, p, new GekkoArg((spml75) => O.Lookup(spml75, null, null, "#m4", null, null, new LookupSettings(), EVariableType.Var, null), (spml75) => new ScalarString("#m4")), new GekkoArg((spml74) => i73, (spml74) => null)), (spml78) => (new ScalarString("#m4")).Add(spml78, new ScalarString("[")).Add(spml78, O.FunctionLookupNew2("plus")(spml78, p, null , new GekkoArg((spml74) => i73, (spml74) => null))).Add(spml78, new ScalarString("]"))), new GekkoArg((spml77) => i76, (spml77) => null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "#m4", null, ivTmpvar72, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o10)
            ;

            //[[commandEnd]]10
        }

        public static void CC0(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_56, ref IVariable xfunctionarg_xf7dke8cj_54, ref IVariable xfunctionarg_xf7dke8cj_55) {
            IVariable forloop_xe7dke6cj_56 = xforloop_xe7dke6cj_56;

            IVariable functionarg_xf7dke8cj_54 = xfunctionarg_xf7dke8cj_54;

            IVariable functionarg_xf7dke8cj_55 = xfunctionarg_xf7dke8cj_55;

            //[[commandStart]]3
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);


            O.Assignment o3 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar58 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, forloop_xe7dke6cj_56
            ), smpl, O.EIndexerType.None, functionarg_xf7dke8cj_54, forloop_xe7dke6cj_56
            ), functionarg_xf7dke8cj_55);
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, functionarg_xf7dke8cj_54, ivTmpvar58, o3, forloop_xe7dke6cj_56
            )
            ;

            //[[commandEnd]]3
            xforloop_xe7dke6cj_56 = forloop_xe7dke6cj_56;

            xfunctionarg_xf7dke8cj_54 = functionarg_xf7dke8cj_54;

            xfunctionarg_xf7dke8cj_55 = functionarg_xf7dke8cj_55;

        }
        public static void CC1(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_63, ref IVariable xfunctionarg_xf7dke8cj_61, ref IVariable xfunctionarg_xf7dke8cj_62) {
            IVariable forloop_xe7dke6cj_63 = xforloop_xe7dke6cj_63;

            IVariable functionarg_xf7dke8cj_61 = xfunctionarg_xf7dke8cj_61;

            IVariable functionarg_xf7dke8cj_62 = xfunctionarg_xf7dke8cj_62;

            //[[commandStart]]7
            p.SetText(@"¤11"); O.InitSmpl(smpl, p);


            O.Assignment o7 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar65 = O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, forloop_xe7dke6cj_63
            ), smpl, O.EIndexerType.None, functionarg_xf7dke8cj_61, forloop_xe7dke6cj_63
            ), functionarg_xf7dke8cj_62);
            O.AdjustT0(smpl, 1);
            O.IndexerSetData(smpl, functionarg_xf7dke8cj_61, ivTmpvar65, o7, forloop_xe7dke6cj_63
            )
            ;

            //[[commandEnd]]7
            xforloop_xe7dke6cj_63 = forloop_xe7dke6cj_63;

            xfunctionarg_xf7dke8cj_61 = functionarg_xf7dke8cj_61;

            xfunctionarg_xf7dke8cj_62 = functionarg_xf7dke8cj_62;

        }

        public static readonly ScalarVal i57 = new ScalarVal(1d);
        public static void FunctionDef60() {

            O.PrepareUfunction(2, "plus");

            Globals.ufunctionsNew2.Add("plus", (GekkoSmpl smpl, P p, GekkoArg functionarg_xf7dke8cj_54_func, GekkoArg functionarg_xf7dke8cj_55_func) =>

            { Databank local1 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg1 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("plus"); p.SetLastFileSentToANTLR(O.LastText("plus")); p.Deeper();
                try {
                    IVariable functionarg_xf7dke8cj_54 = O.TypeCheck_list(functionarg_xf7dke8cj_54_func.f1(smpl), 1);
                    IVariable functionarg_xf7dke8cj_55 = O.TypeCheck_val(functionarg_xf7dke8cj_55_func.f1(smpl), 2);


        //[[commandSpecial]]2
        IVariable forloop_xe7dke6cj_56 = null;
                    int counter59 = 0;
                    for (O.IterateStart(ref forloop_xe7dke6cj_56, i57); O.IterateContinue(forloop_xe7dke6cj_56, i57, Functions.len(smpl, functionarg_xf7dke8cj_54), null, ref counter59); O.IterateStep(forloop_xe7dke6cj_56, i57, null, counter59))
                    {;

                        CC0(smpl, p, ref forloop_xe7dke6cj_56, ref functionarg_xf7dke8cj_54, ref functionarg_xf7dke8cj_55);

                    };

        //[[commandEnd]]2


        //[[commandSpecial]]4
        return O.TypeCheck_list(functionarg_xf7dke8cj_54, 0);

        //[[commandEnd]]4


        return null;
                }
                catch { p.Deeper(); throw; }
                finally {
                    Program.databanks.local = local1; Program.databanks.localGlobal = lg1; p.RemoveLast(); ;
                }
            });

        }

        public static readonly ScalarVal i64 = new ScalarVal(1d);
        public static void FunctionDef67() {

            O.PrepareUfunction(2, "mul");

            Globals.ufunctionsNew2.Add("mul", (GekkoSmpl smpl, P p, GekkoArg functionarg_xf7dke8cj_61_func, GekkoArg functionarg_xf7dke8cj_62_func) =>

            { Databank local5 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg5 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("mul"); p.SetLastFileSentToANTLR(O.LastText("mul")); p.Deeper();
                try {
                    IVariable functionarg_xf7dke8cj_61 = O.TypeCheck_list(functionarg_xf7dke8cj_61_func.f1(smpl), 1);
                    IVariable functionarg_xf7dke8cj_62 = O.TypeCheck_val(functionarg_xf7dke8cj_62_func.f1(smpl), 2);


        //[[commandSpecial]]6
        IVariable forloop_xe7dke6cj_63 = null;
                    int counter66 = 0;
                    for (O.IterateStart(ref forloop_xe7dke6cj_63, i64); O.IterateContinue(forloop_xe7dke6cj_63, i64, Functions.len(smpl, functionarg_xf7dke8cj_61), null, ref counter66); O.IterateStep(forloop_xe7dke6cj_63, i64, null, counter66))
                    {;

                        CC1(smpl, p, ref forloop_xe7dke6cj_63, ref functionarg_xf7dke8cj_61, ref functionarg_xf7dke8cj_62);

                    };

        //[[commandEnd]]6


        //[[commandSpecial]]8
        return O.TypeCheck_list(functionarg_xf7dke8cj_61, 0);

        //[[commandEnd]]8


        return null;
                }
                catch { p.Deeper(); throw; }
                finally {
                    Program.databanks.local = local5; Program.databanks.localGlobal = lg5; p.RemoveLast(); ;
                }
            });

        }

        public static readonly ScalarVal i69 = new ScalarVal(1d);
        public static readonly ScalarVal i70 = new ScalarVal(2d);
        public static readonly ScalarVal i71 = new ScalarVal(3d);
        public static readonly ScalarVal i73 = new ScalarVal(200d);
        public static readonly ScalarVal i76 = new ScalarVal(10000d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);

            FunctionDef60();


            //[[commandEnd]]1

            FunctionDef67();


            //[[commandEnd]]5


            C1(smpl, p);



        }
    }
}
