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


        public static readonly ScalarVal i38 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i39 = new ScalarVal(2d, 0);
        public static readonly ScalarVal i40 = new ScalarVal(100d, 0);
        public static void FunctionDef41() {

            O.PrepareUfunction(4, "f");

            Globals.ufunctionsNew4.Add("f", (GekkoSmpl smpl, P p, bool q42, GekkoArg functionarg_xf7dke8cj_34_func, GekkoArg functionarg_xf7dke8cj_35_func, GekkoArg functionarg_xf7dke8cj_36_func, GekkoArg functionarg_xf7dke8cj_37_func) =>


            { IVariable functionarg_xf7dke8cj_34 = O.TypeCheck_date(functionarg_xf7dke8cj_34_func, smpl, 1);
                IVariable functionarg_xf7dke8cj_35 = O.TypeCheck_date(functionarg_xf7dke8cj_35_func, smpl, 2);
                IVariable functionarg_xf7dke8cj_36 = O.TypeCheck_val(functionarg_xf7dke8cj_36_func.f1(smpl), 3);
                IVariable functionarg_xf7dke8cj_37 = O.TypeCheck_val(functionarg_xf7dke8cj_37_func.f1(smpl), 4);

                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
                try {


        //[[commandSpecial]]1
        return O.TypeCheck_val(O.Add(smpl, O.Multiply(smpl, i40, functionarg_xf7dke8cj_36), functionarg_xf7dke8cj_37), 0);

        //[[commandEnd]]1


        return null;
                }
                catch { p.Deeper(); throw; }
                finally {
                    Program.databanks.local = local0; Program.databanks.localGlobal = lg0; p.RemoveLast(); ;
                }
            });

            O.PrepareUfunction(3, "f");

            Globals.ufunctionsNew3.Add("f", (GekkoSmpl smpl, P p, bool q42, GekkoArg functionarg_xf7dke8cj_34_func, GekkoArg functionarg_xf7dke8cj_35_func, GekkoArg functionarg_xf7dke8cj_36_func) =>


            {

                if (q42) {

                    List<bool> questions43 = new List<bool> { q42 };
                    List<IVariable> defaultValueCodes44 = new List<IVariable> { i39 };
                    List<string> types45 = new List<string> { "val" };
                    List<IVariable> labelCodes46 = new List<IVariable> { O.HandleString(new ScalarString(@"add")) };
                    List<IVariable> promptResults47 = O.Prompt(questions43, defaultValueCodes44, types45, labelCodes46);
                    return O.FunctionLookupNew4("f")(smpl, p, false, functionarg_xf7dke8cj_34_func, functionarg_xf7dke8cj_35_func, functionarg_xf7dke8cj_36_func, new GekkoArg((spml48) => promptResults47[0], (spml48) => null));
                }

                else

                {

                    //return O.FunctionLookupNew4("f")(smpl, p, false, functionarg_xf7dke8cj_34_func, functionarg_xf7dke8cj_35_func, functionarg_xf7dke8cj_36_func i39);
                }


                return null; });

            O.PrepareUfunction(2, "f");

            Globals.ufunctionsNew2.Add("f", (GekkoSmpl smpl, P p, bool q42, GekkoArg functionarg_xf7dke8cj_34_func, GekkoArg functionarg_xf7dke8cj_35_func) =>


            {

                if (q42) {

                    List<bool> questions49 = new List<bool> { q42, q42 };
                    List<IVariable> defaultValueCodes50 = new List<IVariable> { i39, i38 };
                    List<string> types51 = new List<string> { "val", "val" };
                    List<IVariable> labelCodes52 = new List<IVariable> { O.HandleString(new ScalarString(@"add")), O.HandleString(new ScalarString(@"x")) };
                    List<IVariable> promptResults53 = O.Prompt(questions49, defaultValueCodes50, types51, labelCodes52);
                    return O.FunctionLookupNew4("f")(smpl, p, false, functionarg_xf7dke8cj_34_func, functionarg_xf7dke8cj_35_func, new GekkoArg((spml54) => promptResults53[1], (spml54) => null), new GekkoArg((spml55) => promptResults53[0], (spml55) => null));
                }

                else

                {

                    return O.FunctionLookupNew4("f")(smpl, p, false, functionarg_xf7dke8cj_34_func, functionarg_xf7dke8cj_35_func, new GekkoArg((spml55) => i38, (spml55) => null), new GekkoArg((spml55) => i38, (spml55) => null));
                }


                return null; });

        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
            FunctionDef41();


            //[[commandEnd]]0



        }
    }
}
