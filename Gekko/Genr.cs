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
            p.SetText(@"¤5"); O.InitSmpl(smpl, p);

            O.Assignment o2 = new O.Assignment();


            Action assign_29 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar26 = O.FunctionLookupNew3("sq")(smpl, p, null, null, new GekkoArg((spml28) => i27, (spml28) => null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%y", null, ivTmpvar26, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_29 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar26 = O.FunctionLookupNew3("sq")(smpl, p, null, null, new GekkoArg((spml28) => i27, (spml28) => null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar26.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%y", null, ivTmpvar26, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_29, check_29, o2);

            //[[commandEnd]]2
        }


        public static void FunctionDef25()
        {

            O.PrepareUfunction(3, "sq");

            Globals.ufunctionsNew3.Add("sq", (GekkoSmpl smpl, P p, GekkoArg functionarg_xf7dke8cj_22_func, GekkoArg functionarg_xf7dke8cj_23_func, GekkoArg functionarg_xf7dke8cj_24_func) =>

            {
                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("sq"); p.SetLastFileSentToANTLR(O.LastText("sq")); p.Deeper();
                try
                {
                    IVariable functionarg_xf7dke8cj_22 = O.TypeCheck_date(functionarg_xf7dke8cj_22_func, smpl, 1);
                    IVariable functionarg_xf7dke8cj_23 = O.TypeCheck_date(functionarg_xf7dke8cj_23_func, smpl, 2);
                    IVariable functionarg_xf7dke8cj_24 = O.TypeCheck_val(functionarg_xf7dke8cj_24_func.f1(smpl), 3);


        //[[commandSpecial]]1
        return O.TypeCheck_val(O.Multiply(smpl, functionarg_xf7dke8cj_24, functionarg_xf7dke8cj_24), 0);

        //[[commandEnd]]1


        return null;
                }
                catch { p.Deeper(); throw; }
                finally
                {
                    Program.databanks.local = local0; Program.databanks.localGlobal = lg0; p.RemoveLast();
                }
            });

        }

        public static readonly ScalarVal i27 = new ScalarVal(4d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
            FunctionDef25();


            //[[commandEnd]]0


            C0(smpl, p);



        }
    }
}
