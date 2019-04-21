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
            o2.opt_source = @"<[code]>x = 100";


            Action check_14 = () =>
            {
                O.Lookup(smpl, null, null, "x", null, Globals.scalarValMissing, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Action assign_14 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar12 = i13;
                O.AdjustT0(smpl, 2);
                if (O.Dynamic6(smpl)) return;
                O.Lookup(smpl, null, null, "x", null, ivTmpvar12, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_14, check_14, o2);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤6"); O.InitSmpl(smpl, p);

            O.Assignment o3 = new O.Assignment();
            o3.opt_source = @"<[code]>y = f()";


            Action check_16 = () =>
            {
                O.Lookup(smpl, null, null, "y", null, Globals.scalarValMissing, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
            };
            Action assign_16 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar15 = O.FunctionLookupNew2("f")(smpl, p, null, null);
                O.AdjustT0(smpl, 2);
                if (O.Dynamic6(smpl)) return;
                O.Lookup(smpl, null, null, "y", null, ivTmpvar15, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_16, check_16, o3);

            //[[commandEnd]]3
        }


        public static void FunctionDef11()
        {

            O.PrepareUfunction(2, "f");

            Globals.ufunctionsNew2.Add("f", (GekkoSmpl smpl, P p, GekkoArg functionarg_xf7dke8cj_9_func, GekkoArg functionarg_xf7dke8cj_10_func) =>

            {
                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
                try
                {
                    IVariable functionarg_xf7dke8cj_9 = O.TypeCheck_date(functionarg_xf7dke8cj_9_func, smpl, 1);
                    IVariable functionarg_xf7dke8cj_10 = O.TypeCheck_date(functionarg_xf7dke8cj_10_func, smpl, 2);


        //[[commandSpecial]]1
        return O.TypeCheck_series(O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), 0);

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

        public static readonly ScalarVal i13 = new ScalarVal(100d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
            FunctionDef11();


            //[[commandEnd]]0


            C0(smpl, p);



        }
    }
}
