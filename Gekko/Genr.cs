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
            //[[commandStart]]2
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o2 = new O.Assignment();
            o2.opt_source = @"<[code]>val %q1 = 3";


            Globals.precedentsSeries = null;
            Action assign_32 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar30 = i31;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%q1", null, ivTmpvar30, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o2)
                ;
            };
            Func<bool> check_32 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar30 = i31;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar30.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%q1", null, ivTmpvar30, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_32, check_32, o2);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o3 = new O.Assignment();
            o3.opt_source = @"<[code]>val %q2 = 4";


            Globals.precedentsSeries = null;
            Action assign_35 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar33 = i34;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%q2", null, ivTmpvar33, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o3)
                ;
            };
            Func<bool> check_35 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar33 = i34;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar33.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%q2", null, ivTmpvar33, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o3)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_35, check_35, o3);

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o4 = new O.Assignment();
            o4.opt_source = @"<[code]>val %q3 = f(%q1, %q2)";


            Globals.precedentsSeries = null;
            Action assign_39 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar36 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml37) => O.Lookup(spml37, null, null, "%q1", null, null, new LookupSettings(), EVariableType.Var, null), (spml37) => new ScalarString("%q1")), new GekkoArg((spml38) => O.Lookup(spml38, null, null, "%q2", null, null, new LookupSettings(), EVariableType.Var, null), (spml38) => new ScalarString("%q2")));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%q3", null, ivTmpvar36, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o4)
                ;
            };
            Func<bool> check_39 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar36 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml37) => O.Lookup(spml37, null, null, "%q1", null, null, new LookupSettings(), EVariableType.Var, null), (spml37) => new ScalarString("%q1")), new GekkoArg((spml38) => O.Lookup(spml38, null, null, "%q2", null, null, new LookupSettings(), EVariableType.Var, null), (spml38) => new ScalarString("%q2")));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar36.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%q3", null, ivTmpvar36, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o4)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_39, check_39, o4);

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o5 = new O.Assignment();
            o5.opt_source = @"<[code]>val %q4 = f(f(%q1, %q2)+0, f(%q1, %q2))";


            Globals.precedentsSeries = null;
            Action assign_48 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar40 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml46) => O.Add(spml46, O.FunctionLookupNew4("f")(spml46, p, false, null, null, new GekkoArg((spml41) => O.Lookup(spml41, null, null, "%q1", null, null, new LookupSettings(), EVariableType.Var, null), (spml41) => new ScalarString("%q1")), new GekkoArg((spml42) => O.Lookup(spml42, null, null, "%q2", null, null, new LookupSettings(), EVariableType.Var, null), (spml42) => new ScalarString("%q2"))), i43), (spml46) => null), new GekkoArg((spml47) => O.FunctionLookupNew4("f")(spml47, p, false, null, null, new GekkoArg((spml44) => O.Lookup(spml44, null, null, "%q1", null, null, new LookupSettings(), EVariableType.Var, null), (spml44) => new ScalarString("%q1")), new GekkoArg((spml45) => O.Lookup(spml45, null, null, "%q2", null, null, new LookupSettings(), EVariableType.Var, null), (spml45) => new ScalarString("%q2"))), (spml47) => null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%q4", null, ivTmpvar40, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o5)
                ;
            };
            Func<bool> check_48 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar40 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml46) => O.Add(spml46, O.FunctionLookupNew4("f")(spml46, p, false, null, null, new GekkoArg((spml41) => O.Lookup(spml41, null, null, "%q1", null, null, new LookupSettings(), EVariableType.Var, null), (spml41) => new ScalarString("%q1")), new GekkoArg((spml42) => O.Lookup(spml42, null, null, "%q2", null, null, new LookupSettings(), EVariableType.Var, null), (spml42) => new ScalarString("%q2"))), i43), (spml46) => null), new GekkoArg((spml47) => O.FunctionLookupNew4("f")(spml47, p, false, null, null, new GekkoArg((spml44) => O.Lookup(spml44, null, null, "%q1", null, null, new LookupSettings(), EVariableType.Var, null), (spml44) => new ScalarString("%q1")), new GekkoArg((spml45) => O.Lookup(spml45, null, null, "%q2", null, null, new LookupSettings(), EVariableType.Var, null), (spml45) => new ScalarString("%q2"))), (spml47) => null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar40.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%q4", null, ivTmpvar40, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Val, o5)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_48, check_48, o5);

            //[[commandEnd]]5
        }


        public static void FunctionDef28()
        {

            O.PrepareUfunction(4, "f");

            O.Add4_UfunctionSpecialName(null, "f", (GekkoSmpl smpl, P p, bool q29, GekkoArg functionarg_xf7dke8cj_24_func, GekkoArg functionarg_xf7dke8cj_25_func, GekkoArg functionarg_xf7dke8cj_26_func, GekkoArg functionarg_xf7dke8cj_27_func) =>


            {
                IVariable functionarg_xf7dke8cj_24 = O.TypeCheck_date(functionarg_xf7dke8cj_24_func, smpl, 1);
                IVariable functionarg_xf7dke8cj_25 = O.TypeCheck_date(functionarg_xf7dke8cj_25_func, smpl, 2);
                IVariable functionarg_xf7dke8cj_26 = O.TypeCheck_val(functionarg_xf7dke8cj_26_func.f1(smpl), 3);
                IVariable functionarg_xf7dke8cj_27 = O.TypeCheck_val(functionarg_xf7dke8cj_27_func.f1(smpl), 4);

                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
                try
                {


                    p.SetText(@"¤1");


                    //[[commandSpecial]]1
                    return O.TypeCheck_val(O.Multiply(smpl, functionarg_xf7dke8cj_26, functionarg_xf7dke8cj_27), 0);

                    //[[commandEnd]]1


                    return null;
                }
                catch { p.Deeper(); throw; }
                finally
                {
                    Program.databanks.local = local0; Program.databanks.localGlobal = lg0; p.RemoveLast(); ;
                }
            });

        }

        public static readonly ScalarVal i31 = new ScalarVal(3d, 0);
        public static readonly ScalarVal i34 = new ScalarVal(4d, 0);
        public static readonly ScalarVal i43 = new ScalarVal(0d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            p.SetText(@"¤1");

            FunctionDef28();


            //[[commandEnd]]0


            C0(smpl, p);



        }
    }
}
