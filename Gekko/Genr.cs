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
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);

            O.Assignment o2 = new O.Assignment();
            o2.opt_source = @"<[code]>%y = f(1)";

            Action assign_11 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar8 = O.FunctionLookupNew3("f")(smpl, p, false, null, null, new GekkoArg((spml10) => i9, (spml10) => null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%y", null, ivTmpvar8, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_11 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar8 = O.FunctionLookupNew3("f")(smpl, p, false, null, null, new GekkoArg((spml10) => i9, (spml10) => null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar8.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%y", null, ivTmpvar8, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_11, check_11, o2);

            //[[commandEnd]]2
        }


        public static readonly ScalarVal i6 = new ScalarVal(1d, 0);
        public static void FunctionDef7()
        {

            O.PrepareUfunction(3, "f");

            Globals.ufunctionsNew3.Add("f", (GekkoSmpl smpl, P p, bool b, GekkoArg functionarg_xf7dke8cj_3_func, GekkoArg functionarg_xf7dke8cj_4_func, GekkoArg functionarg_xf7dke8cj_5_func) =>


            {
                IVariable functionarg_xf7dke8cj_3 = O.TypeCheck_date(functionarg_xf7dke8cj_3_func, smpl, 1);
                IVariable functionarg_xf7dke8cj_4 = O.TypeCheck_date(functionarg_xf7dke8cj_4_func, smpl, 2);
                IVariable functionarg_xf7dke8cj_5 = O.TypeCheck_val(functionarg_xf7dke8cj_5_func.f1(smpl), 3);

                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
                try
                {


        //[[commandSpecial]]1
        return O.TypeCheck_val(O.Add(smpl, functionarg_xf7dke8cj_5, i6), 0);

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

        public static readonly ScalarVal i9 = new ScalarVal(1d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
            FunctionDef7();


            //[[commandEnd]]0


            C0(smpl, p);



        }
    }
}
