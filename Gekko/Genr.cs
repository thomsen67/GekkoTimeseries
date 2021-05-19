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


        public static readonly ScalarVal i27 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i28 = new ScalarVal(2d, 0);
        public static readonly ScalarVal i29 = new ScalarVal(100d, 0);
        public static void FunctionDef30()
        {

            O.PrepareUfunction(4, "f");

            O.Add4_UfunctionSpecialName(null, "f", (GekkoSmpl smpl, P p, bool q31, GekkoArg functionarg_xf7dke8cj_23_func, GekkoArg functionarg_xf7dke8cj_24_func, GekkoArg functionarg_xf7dke8cj_25_func, GekkoArg functionarg_xf7dke8cj_26_func) =>


            {
                IVariable functionarg_xf7dke8cj_23 = O.TypeCheck_date(functionarg_xf7dke8cj_23_func, smpl, 1);
                IVariable functionarg_xf7dke8cj_24 = O.TypeCheck_date(functionarg_xf7dke8cj_24_func, smpl, 2);
                IVariable functionarg_xf7dke8cj_25 = O.TypeCheck_val(functionarg_xf7dke8cj_25_func.f1(smpl), 3);
                IVariable functionarg_xf7dke8cj_26 = O.TypeCheck_val(functionarg_xf7dke8cj_26_func.f1(smpl), 4);

                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f", ""); p.SetLastFileSentToANTLR(O.LastText("f", "")); p.Deeper();
                try
                {


                    p.SetText(@"¤1");


                    //[[commandSpecial]]1
                    return O.TypeCheck_val(O.Add(smpl, O.Multiply(smpl, i29, functionarg_xf7dke8cj_25), functionarg_xf7dke8cj_26), 0);

                    //[[commandEnd]]1


                    return null;
                }
                catch { p.Deeper(); throw; }
                finally
                {
                    Program.databanks.local = local0; Program.databanks.localGlobal = lg0; p.RemoveLast(); ;
                }
            });

            O.PrepareUfunction(3, "f");

            O.Add3_UfunctionSpecialName(null, "f", (GekkoSmpl smpl, P p, bool q31, GekkoArg functionarg_xf7dke8cj_23_func, GekkoArg functionarg_xf7dke8cj_24_func, GekkoArg functionarg_xf7dke8cj_25_func) =>


            {

                if (q31)
                {

                    List<bool> questions32 = new List<bool> { q31 };
                    List<IVariable> defaultValueCodes33 = new List<IVariable> { i28 };
                    List<string> types34 = new List<string> { "val" };
                    List<IVariable> labelCodes35 = new List<IVariable> { O.HandleString(new ScalarString(@"x2")) };
                    List<IVariable> promptResults36 = O.Prompt(questions32, defaultValueCodes33, types34, labelCodes35);
                    return O.FunctionLookupNew4(null, null, "f")(smpl, p, false, functionarg_xf7dke8cj_23_func, functionarg_xf7dke8cj_24_func, functionarg_xf7dke8cj_25_func, new GekkoArg((spml37) => promptResults36[0], (spml37) => null));
                }

                else

                {

                    return O.FunctionLookupNew4(null, null, "f")(smpl, p, false, functionarg_xf7dke8cj_23_func, functionarg_xf7dke8cj_24_func, functionarg_xf7dke8cj_25_func, new GekkoArg((spml37) => i28, (spml37) => null));
                }


                return null;
            });

            O.PrepareUfunction(2, "f");

            O.Add2_UfunctionSpecialName(null, "f", (GekkoSmpl smpl, P p, bool q31, GekkoArg functionarg_xf7dke8cj_23_func, GekkoArg functionarg_xf7dke8cj_24_func) =>


            {

                if (q31)
                {

                    List<bool> questions38 = new List<bool> { q31, q31 };
                    List<IVariable> defaultValueCodes39 = new List<IVariable> { i27, i28 };
                    List<string> types40 = new List<string> { "val", "val" };
                    List<IVariable> labelCodes41 = new List<IVariable> { O.HandleString(new ScalarString(@"x1")), O.HandleString(new ScalarString(@"x2")) };
                    List<IVariable> promptResults42 = O.Prompt(questions38, defaultValueCodes39, types40, labelCodes41);
                    return O.FunctionLookupNew4(null, null, "f")(smpl, p, false, functionarg_xf7dke8cj_23_func, functionarg_xf7dke8cj_24_func, new GekkoArg((spml43) => promptResults42[0], (spml43) => null), new GekkoArg((spml44) => promptResults42[1], (spml44) => null));
                }

                else

                {

                    return O.FunctionLookupNew4(null, null, "f")(smpl, p, false, functionarg_xf7dke8cj_23_func, functionarg_xf7dke8cj_24_func, new GekkoArg((spml43) => i27, (spml43) => null), new GekkoArg((spml44) => i28, (spml44) => null));
                }


                return null;
            });

        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            p.SetText(@"¤1");

            FunctionDef30();


            //[[commandEnd]]0



        }
    }
}
