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


        public static void FunctionDef7()
        {

            O.PrepareUfunction(3, "f1");

            O.Add3_UfunctionSpecialName("p1", "f1", (GekkoSmpl smpl, P p, bool q8, GekkoArg functionarg_xf7dke8cj_4_func, GekkoArg functionarg_xf7dke8cj_5_func, GekkoArg functionarg_xf7dke8cj_6_func) =>


            {
                IVariable functionarg_xf7dke8cj_4 = O.TypeCheck_date(functionarg_xf7dke8cj_4_func, smpl, 1);
                IVariable functionarg_xf7dke8cj_5 = O.TypeCheck_date(functionarg_xf7dke8cj_5_func, smpl, 2);
                IVariable functionarg_xf7dke8cj_6 = O.TypeCheck_string(functionarg_xf7dke8cj_6_func.f1(smpl), 3);

                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f1"); p.SetLastFileSentToANTLR(O.LastText("f1")); p.Deeper();
                try
                {


                    p.SetText(@"¤6");


                    //[[commandSpecial]]1
                    return O.TypeCheck_string(O.Add(smpl, O.HandleString(new ScalarString(@"p1_f1_")), functionarg_xf7dke8cj_6), 0);

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

        public static void FunctionDef13()
        {

            O.PrepareUfunction(4, "f1");

            O.Add4_UfunctionSpecialName("p1", "f1", (GekkoSmpl smpl, P p, bool q14, GekkoArg functionarg_xf7dke8cj_9_func, GekkoArg functionarg_xf7dke8cj_10_func, GekkoArg functionarg_xf7dke8cj_11_func, GekkoArg functionarg_xf7dke8cj_12_func) =>


            {
                IVariable functionarg_xf7dke8cj_9 = O.TypeCheck_date(functionarg_xf7dke8cj_9_func, smpl, 1);
                IVariable functionarg_xf7dke8cj_10 = O.TypeCheck_date(functionarg_xf7dke8cj_10_func, smpl, 2);
                IVariable functionarg_xf7dke8cj_11 = O.TypeCheck_string(functionarg_xf7dke8cj_11_func.f1(smpl), 3);
                IVariable functionarg_xf7dke8cj_12 = O.TypeCheck_string(functionarg_xf7dke8cj_12_func.f1(smpl), 4);

                Databank local2 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg2 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f1"); p.SetLastFileSentToANTLR(O.LastText("f1")); p.Deeper();
                try
                {


                    p.SetText(@"¤8");


                    //[[commandSpecial]]3
                    return O.TypeCheck_string(O.Add(smpl, O.Add(smpl, O.HandleString(new ScalarString(@"p1_f1_")), functionarg_xf7dke8cj_11), functionarg_xf7dke8cj_12), 0);

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

            p.SetText(@"¤8");

            FunctionDef7();


            //[[commandEnd]]0


            p.SetText(@"¤8");

            FunctionDef13();


            //[[commandEnd]]2



        }
    }
}

