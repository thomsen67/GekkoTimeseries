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


            //[[commandStart]]1
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);

            O.Assignment o1 = new O.Assignment();
            o1.opt_source = @"<[code]>local:%x = 3";


            Action assign_3 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar1 = i2;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, "local", "%x", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
            };
            Func<bool> check_3 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar1 = i2;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar1.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, "local", "%x", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_3, check_3, o1);

            //[[commandEnd]]1
        }
        public static void C1(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]4
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);

            O.Assignment o4 = new O.Assignment();
            o4.opt_source = @"<[code]>%a = f(%x)";


            Action assign_11 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar9 = O.FunctionLookupNew3("f")(smpl, p, null, null, new GekkoArg((spml10) => O.Lookup(spml10, null, null, "%x", null, null, new LookupSettings(), EVariableType.Var, null), (spml10) => new ScalarString("%x")));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%a", null, ivTmpvar9, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
            };
            Func<bool> check_11 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar9 = O.FunctionLookupNew3("f")(smpl, p, null, null, new GekkoArg((spml10) => O.Lookup(spml10, null, null, "%x", null, null, new LookupSettings(), EVariableType.Var, null), (spml10) => new ScalarString("%x")));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar9.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%a", null, ivTmpvar9, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_11, check_11, o4);

            //[[commandEnd]]4
        }


        public static readonly ScalarVal i2 = new ScalarVal(3d, 0);
        public static readonly ScalarVal i7 = new ScalarVal(2d, 0);
        public static void FunctionDef8()
        {

            O.PrepareUfunction(3, "f");

            Globals.ufunctionsNew3.Add("f", (GekkoSmpl smpl, P p, GekkoArg functionarg_xf7dke8cj_4_func, GekkoArg functionarg_xf7dke8cj_5_func, GekkoArg functionarg_xf7dke8cj_6_func) =>

            {
                IVariable functionarg_xf7dke8cj_4 = O.TypeCheck_date(functionarg_xf7dke8cj_4_func, smpl, 1);
                IVariable functionarg_xf7dke8cj_5 = O.TypeCheck_date(functionarg_xf7dke8cj_5_func, smpl, 2);
                IVariable functionarg_xf7dke8cj_6 = O.TypeCheck_val(functionarg_xf7dke8cj_6_func.f1(smpl), 3);

                Databank local2 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg2 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
                try
                {
                    //IVariable functionarg_xf7dke8cj_4 = O.TypeCheck_date(functionarg_xf7dke8cj_4_func, smpl, 1);
                    //IVariable functionarg_xf7dke8cj_5 = O.TypeCheck_date(functionarg_xf7dke8cj_5_func, smpl, 2);
                    //IVariable functionarg_xf7dke8cj_6 = O.TypeCheck_val(functionarg_xf7dke8cj_6_func.f1(smpl), 3);


        //[[commandSpecial]]3
        return O.TypeCheck_val(O.Multiply(smpl, i7, functionarg_xf7dke8cj_6), 0);

        //[[commandEnd]]3


        return null;
                }
                catch { p.Deeper(); throw; }
                finally
                {
                    Program.databanks.local = local2; Program.databanks.localGlobal = lg2; p.RemoveLast(); ;
                }
            });

        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);

            FunctionDef8();


            //[[commandEnd]]2


            C1(smpl, p);



        }
    }
}
