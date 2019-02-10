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
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);


            O.Assignment o2 = new O.Assignment();
            O.AdjustT0(smpl, -2);
            IVariable ivTmpvar8 = O.FunctionLookupNew1("f")(smpl, p, new GekkoArg((spml10) => i9, (spml10) => null));
            O.AdjustT0(smpl, 2);
            O.Lookup(smpl, null, null, "#m", null, ivTmpvar8, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
            ;

            //[[commandEnd]]2
        }


        public static readonly ScalarVal i4 = new ScalarVal(2d);
        public static readonly ScalarVal i6 = new ScalarVal(3d);
        public static void FunctionDef7()
        {


            O.PrepareUfunction(1, "f");

            Globals.ufunctionsNew1.Add("f", (GekkoSmpl smpl, P p, GekkoArg functionarg_xf7dke8cj_1_func) =>

            {
                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
                try
                {
                    IVariable functionarg_xf7dke8cj_1 = O.TypeCheck_val(functionarg_xf7dke8cj_1_func.f1(smpl), 1);


                    Func<Map> MapDef_mapTmpvar2 = () =>
                    {
                        Map mapTmpvar2 = new Map();
                        O.AdjustT0(smpl, -2);
                        IVariable ivTmpvar3 = O.Multiply(smpl, i4, functionarg_xf7dke8cj_1);
                        O.AdjustT0(smpl, 2);
                        O.Lookup(smpl, mapTmpvar2, null, "x1", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

                        O.AdjustT0(smpl, -2);
                        IVariable ivTmpvar5 = O.Multiply(smpl, i6, functionarg_xf7dke8cj_1);
                        O.AdjustT0(smpl, 2);
                        O.Lookup(smpl, mapTmpvar2, null, "x2", null, ivTmpvar5, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;


                        return mapTmpvar2;
                    };



        //[[commandSpecial]]1
        return O.TypeCheck_map(MapDef_mapTmpvar2(), 0);

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

        public static readonly ScalarVal i9 = new ScalarVal(7d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            


            FunctionDef7();


            //[[commandEnd]]0


            C0(smpl, p);



        }
    }
}
