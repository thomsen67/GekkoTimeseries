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
            //[[commandStart]]3
            p.SetText(@"¤6"); O.InitSmpl(smpl, p);

            O.Assignment o3 = new O.Assignment();
            o3.opt_source = @"<[code]>%y1 = f(3, 4)";


            Action assign_26 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar21 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml24) => i22, (spml24) => null), new GekkoArg((spml25) => i23, (spml25) => null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%y1", null, ivTmpvar21, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
            };
            Func<bool> check_26 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar21 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml24) => i22, (spml24) => null), new GekkoArg((spml25) => i23, (spml25) => null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar21.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%y1", null, ivTmpvar21, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_26, check_26, o3);

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤7"); O.InitSmpl(smpl, p);

            O.Assignment o4 = new O.Assignment();
            o4.opt_source = @"<[code]>%y2 = f(3)";


            Action assign_30 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar27 = O.FunctionLookupNew3("f")(smpl, p, false, null, null, new GekkoArg((spml29) => i28, (spml29) => null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%y2", null, ivTmpvar27, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
            };
            Func<bool> check_30 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar27 = O.FunctionLookupNew3("f")(smpl, p, false, null, null, new GekkoArg((spml29) => i28, (spml29) => null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar27.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%y2", null, ivTmpvar27, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_30, check_30, o4);

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤8"); O.InitSmpl(smpl, p);

            O.Assignment o5 = new O.Assignment();
            o5.opt_source = @"<[code]>%y3 = f()";


            Action assign_32 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar31 = O.FunctionLookupNew2("f")(smpl, p, false, null, null);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%y3", null, ivTmpvar31, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o5)
                ;
            };
            Func<bool> check_32 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar31 = O.FunctionLookupNew2("f")(smpl, p, false, null, null);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar31.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%y3", null, ivTmpvar31, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o5)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_32, check_32, o5);

            //[[commandEnd]]5


            //[[commandStart]]6
            p.SetText(@"¤12"); O.InitSmpl(smpl, p);


            Program.Mem(null);

            //[[commandEnd]]6
        }


        public static readonly ScalarVal i5 = new ScalarVal(666d, 0);
        public static readonly ScalarVal i6 = new ScalarVal(777d, 0);
        public static void FunctionDef7()
        {

            O.PrepareUfunction(4, "f");

            Globals.ufunctionsNew4.Add("f", (GekkoSmpl smpl, P p, bool b, GekkoArg functionarg_xf7dke8cj_1_func, GekkoArg functionarg_xf7dke8cj_2_func, GekkoArg functionarg_xf7dke8cj_3_func, GekkoArg functionarg_xf7dke8cj_4_func) =>


            {
                IVariable functionarg_xf7dke8cj_1 = O.TypeCheck_date(functionarg_xf7dke8cj_1_func, smpl, 1);
                IVariable functionarg_xf7dke8cj_2 = O.TypeCheck_date(functionarg_xf7dke8cj_2_func, smpl, 2);
                IVariable functionarg_xf7dke8cj_3 = O.TypeCheck_val(functionarg_xf7dke8cj_3_func.f1(smpl), 3);
                IVariable functionarg_xf7dke8cj_4 = O.TypeCheck_val(functionarg_xf7dke8cj_4_func.f1(smpl), 4);

                Databank local1 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg1 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
                try
                {


                    //[[commandSpecial]]2
                    return O.TypeCheck_val(O.Add(smpl, functionarg_xf7dke8cj_3, functionarg_xf7dke8cj_4), 0);

                    //[[commandEnd]]2


                    return null;
                }
                catch { p.Deeper(); throw; }
                finally
                {
                    Program.databanks.local = local1; Program.databanks.localGlobal = lg1; p.RemoveLast(); ;
                }
            });

            O.PrepareUfunction(3, "f");

            Globals.ufunctionsNew3.Add("f", (GekkoSmpl smpl, P p, bool b, GekkoArg functionarg_xf7dke8cj_1_func, GekkoArg functionarg_xf7dke8cj_2_func, GekkoArg functionarg_xf7dke8cj_3_func) =>


            {

                //We need to use this to produce ParserGek code similar,
                //and afterwards do somthing about prompting, but his will make default args work.

                List<bool> questions8 = new List<bool> { false };
                List<IVariable> defaultValueCodes9 = new List<IVariable> { i6 };
                List<string> types10 = new List<string> { "val" };
                List<IVariable> labelCodes11 = new List<IVariable> { O.HandleString(new ScalarString("add")) };
                List<IVariable> promptResults12 = O.Prompt(questions8, defaultValueCodes9, types10, labelCodes11);
                return O.FunctionLookupNew4("f")(smpl, p, false, functionarg_xf7dke8cj_1_func, functionarg_xf7dke8cj_2_func, functionarg_xf7dke8cj_3_func, new GekkoArg((spml13) => promptResults12[0], (spml13) => null));

                return null;
            });

            

        }

        public static readonly ScalarVal i22 = new ScalarVal(3d, 0);
        public static readonly ScalarVal i23 = new ScalarVal(4d, 0);
        public static readonly ScalarVal i28 = new ScalarVal(3d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);

            FunctionDef7();


            //[[commandEnd]]1


            C1(smpl, p);



        }
    }
}
