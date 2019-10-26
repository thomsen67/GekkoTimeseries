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


            Action assign_23 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar18 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml21) => i19, (spml21) => null), new GekkoArg((spml22) => i20, (spml22) => null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%y1", null, ivTmpvar18, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
            };
            Func<bool> check_23 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar18 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml21) => i19, (spml21) => null), new GekkoArg((spml22) => i20, (spml22) => null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar18.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%y1", null, ivTmpvar18, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_23, check_23, o3);

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤7"); O.InitSmpl(smpl, p);

            O.Assignment o4 = new O.Assignment();
            o4.opt_source = @"<[code]>%y2 = f(3)";


            Action assign_27 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar24 = O.FunctionLookupNew3("f")(smpl, p, false, null, null, new GekkoArg((spml26) => i25, (spml26) => null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%y2", null, ivTmpvar24, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
            };
            Func<bool> check_27 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar24 = O.FunctionLookupNew3("f")(smpl, p, false, null, null, new GekkoArg((spml26) => i25, (spml26) => null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar24.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%y2", null, ivTmpvar24, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_27, check_27, o4);

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤11"); O.InitSmpl(smpl, p);


            Program.Mem(null);

            //[[commandEnd]]5
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

                List<bool> questions8 = new List<bool> { false };
                List<string> defaultValueCodes9 = new List<string> { "i6" };
                List<string> types10 = new List<string> { "val" };
                List<string> labelCodes11 = new List<string> { "O.HandleString(new ScalarString(@\"add\"))" };
                List<IVariable> promptResults12 = O.Prompt(questions8, defaultValueCodes9, types10, labelCodes11);
                return O.FunctionLookupNew4("f")(smpl, p, false, functionarg_xf7dke8cj_1_func, functionarg_xf7dke8cj_2_func, functionarg_xf7dke8cj_3_func, new GekkoArg((spml25) => promptResults12[0], (spml25) => null));

                return null;
            });

            

        }

        public static readonly ScalarVal i19 = new ScalarVal(3d, 0);
        public static readonly ScalarVal i20 = new ScalarVal(4d, 0);
        public static readonly ScalarVal i25 = new ScalarVal(3d, 0);

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
