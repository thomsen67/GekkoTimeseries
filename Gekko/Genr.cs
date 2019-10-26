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


            Action assign_21 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar16 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml19) => i17, (spml19) => null), new GekkoArg((spml20) => i18, (spml20) => null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%y1", null, ivTmpvar16, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
            };
            Func<bool> check_21 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar16 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml19) => i17, (spml19) => null), new GekkoArg((spml20) => i18, (spml20) => null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar16.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%y1", null, ivTmpvar16, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_21, check_21, o3);

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤7"); O.InitSmpl(smpl, p);

            O.Assignment o4 = new O.Assignment();
            o4.opt_source = @"<[code]>%y2 = f(3)";


            Action assign_25 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar22 = O.FunctionLookupNew3("f")(smpl, p, false, null, null, new GekkoArg((spml24) => i23, (spml24) => null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%y2", null, ivTmpvar22, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
            };
            Func<bool> check_25 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar22 = O.FunctionLookupNew3("f")(smpl, p, false, null, null, new GekkoArg((spml24) => i23, (spml24) => null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar22.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%y2", null, ivTmpvar22, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_25, check_25, o4);

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤9"); O.InitSmpl(smpl, p);


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

            List<bool> questions8 = new List<bool> { true };
            List<string> defaultValueCodes9 = new List<string> { "i6" };
            List<string> types10 = new List<string> { "val" };
            List<string> labelCodes11 = new List<string> { "O.HandleString(new ScalarString(@\"add\"))" };
            List<IVariable> promptResults = O.Prompt(questions8, defaultValueCodes9, types10, labelCodes11);
            O.PrepareUfunction(3, "f");

            Globals.ufunctionsNew3.Add("f", (GekkoSmpl smpl, P p, bool b, GekkoArg functionarg_xf7dke8cj_1_func, GekkoArg functionarg_xf7dke8cj_2_func, GekkoArg functionarg_xf7dke8cj_3_func) =>


            {
                G.Writeln("Hej 3");
                return null;
            });

            List<bool> questions12 = new List<bool> { true, true };
            List<string> defaultValueCodes13 = new List<string> { "i6", "i5" };
            List<string> types14 = new List<string> { "val", "val" };
            List<string> labelCodes15 = new List<string> { "O.HandleString(new ScalarString(@\"add\"))", "O.HandleString(new ScalarString(@\"x\"))" };
            List<IVariable> promptResults5 = O.Prompt(questions12, defaultValueCodes13, types14, labelCodes15);
            O.PrepareUfunction(2, "f");

            Globals.ufunctionsNew2.Add("f", (GekkoSmpl smpl, P p, bool b, GekkoArg functionarg_xf7dke8cj_1_func, GekkoArg functionarg_xf7dke8cj_2_func) =>


            {
                G.Writeln("Hej 2");
                return null;
            });

        }

        public static readonly ScalarVal i17 = new ScalarVal(3d, 0);
        public static readonly ScalarVal i18 = new ScalarVal(4d, 0);
        public static readonly ScalarVal i23 = new ScalarVal(3d, 0);

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
