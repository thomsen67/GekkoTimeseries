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
            //[[commandStart]]6
            p.SetText(@"¤5"); O.InitSmpl(smpl, p);

            O.Assignment o6 = new O.Assignment();
            o6.opt_source = @"<[code]>%y2 = f(3, 4)";


            Action assign_37 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar32 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml35) => i33, (spml35) => null), new GekkoArg((spml36) => i34, (spml36) => null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%y1", null, ivTmpvar32, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o6)
                ;
            };
            Func<bool> check_37 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar32 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml35) => i33, (spml35) => null), new GekkoArg((spml36) => i34, (spml36) => null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar32.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%y1", null, ivTmpvar32, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o6)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_37, check_37, o6);

            //[[commandEnd]]6


            //[[commandStart]]7
            p.SetText(@"¤6"); O.InitSmpl(smpl, p);

            O.Assignment o7 = new O.Assignment();
            o7.opt_source = @"<[code]>%y2 = f(3)";


            Action assign_41 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar38 = O.FunctionLookupNew3("f")(smpl, p, false, null, null, new GekkoArg((spml40) => i39, (spml40) => null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%y2", null, ivTmpvar38, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o7)
                ;
            };
            Func<bool> check_41 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar38 = O.FunctionLookupNew3("f")(smpl, p, false, null, null, new GekkoArg((spml40) => i39, (spml40) => null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar38.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%y2", null, ivTmpvar38, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o7)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_41, check_41, o7);

            //[[commandEnd]]7


            //[[commandStart]]8
            p.SetText(@"¤7"); O.InitSmpl(smpl, p);

            O.Assignment o8 = new O.Assignment();
            o8.opt_source = @"<[code]>%y3 = g(3)";


            Action assign_45 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar42 = O.FunctionLookupNew3("g")(smpl, p, false, null, null, new GekkoArg((spml44) => i43, (spml44) => null));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%y3", null, ivTmpvar42, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o8)
                ;
            };
            Func<bool> check_45 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar42 = O.FunctionLookupNew3("g")(smpl, p, false, null, null, new GekkoArg((spml44) => i43, (spml44) => null));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar42.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%y3", null, ivTmpvar42, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o8)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_45, check_45, o8);

            //[[commandEnd]]8


            //[[commandStart]]9
            p.SetText(@"¤8"); O.InitSmpl(smpl, p);


            Program.Mem(null);

            //[[commandEnd]]9
        }


        public static void FunctionDef19()
        {

            O.PrepareUfunction(4, "f");

            Globals.ufunctionsNew4.Add("f", (GekkoSmpl smpl, P p, bool b, GekkoArg functionarg_xf7dke8cj_15_func, GekkoArg functionarg_xf7dke8cj_16_func, GekkoArg functionarg_xf7dke8cj_17_func, GekkoArg functionarg_xf7dke8cj_18_func) =>


            {
                IVariable functionarg_xf7dke8cj_15 = O.TypeCheck_date(functionarg_xf7dke8cj_15_func, smpl, 1);
                IVariable functionarg_xf7dke8cj_16 = O.TypeCheck_date(functionarg_xf7dke8cj_16_func, smpl, 2);
                IVariable functionarg_xf7dke8cj_17 = O.TypeCheck_val(functionarg_xf7dke8cj_17_func.f1(smpl), 3);
                IVariable functionarg_xf7dke8cj_18 = O.TypeCheck_val(functionarg_xf7dke8cj_18_func.f1(smpl), 4);

                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
                try
                {

                    //[[commandSpecial]]1
                    return O.TypeCheck_val(O.Add(smpl, functionarg_xf7dke8cj_17, functionarg_xf7dke8cj_18), 0);

                }
                catch { p.Deeper(); throw; }
                finally
                {
                    Program.databanks.local = local0; Program.databanks.localGlobal = lg0; p.RemoveLast(); ;
                }
            });

        }

        public static readonly ScalarVal i23 = new ScalarVal(2d, 0);
        public static void FunctionDef26()
        {

            O.PrepareUfunction(3, "f");

            Globals.ufunctionsNew3.Add("f", (GekkoSmpl smpl, P p, bool q, GekkoArg functionarg_xf7dke8cj_20_func, GekkoArg functionarg_xf7dke8cj_21_func, GekkoArg functionarg_xf7dke8cj_22_func) =>
            {


                List<bool> question = new List<bool> { true };
                List<string> inputtedValue = new List<string> { "100" };
                List<string> type = new List<string> { "val" };
                List<string> txt = new List<string> { "Tast!" };
                List<IVariable> d = new List<IVariable> { null };

                for (int i = 0; i < 1; i++)
                {
                    if (question[i])
                    {
                        string tmp = inputtedValue[i];
                        Program.InputBox("Input", txt[i], ref tmp);
                        inputtedValue[i] = tmp;
                    }
                    d[i] = O.AcceptHelper1(type[i], inputtedValue[i]);
                }

                return O.TypeCheck_val(O.FunctionLookupNew4("f")(smpl, p, false, functionarg_xf7dke8cj_20_func, functionarg_xf7dke8cj_21_func, functionarg_xf7dke8cj_22_func, new GekkoArg((spml25) => d[0], (spml25) => null)), 0);

            });

        }

        

        public static readonly ScalarVal i30 = new ScalarVal(2d, 0);
        public static void FunctionDef31()
        {

            O.PrepareUfunction(3, "g");

            Globals.ufunctionsNew3.Add("g", (GekkoSmpl smpl, P p, bool b, GekkoArg functionarg_xf7dke8cj_27_func, GekkoArg functionarg_xf7dke8cj_28_func, GekkoArg functionarg_xf7dke8cj_29_func) =>


            {
                IVariable functionarg_xf7dke8cj_27 = O.TypeCheck_date(functionarg_xf7dke8cj_27_func, smpl, 1);
                IVariable functionarg_xf7dke8cj_28 = O.TypeCheck_date(functionarg_xf7dke8cj_28_func, smpl, 2);
                IVariable functionarg_xf7dke8cj_29 = O.TypeCheck_val(functionarg_xf7dke8cj_29_func.f1(smpl), 3);

                Databank local4 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg4 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("g"); p.SetLastFileSentToANTLR(O.LastText("g")); p.Deeper();
                try
                {


        //[[commandSpecial]]5
        return O.TypeCheck_val(O.Add(smpl, functionarg_xf7dke8cj_29, i30), 0);

        //[[commandEnd]]5


        return null;
                }
                catch { p.Deeper(); throw; }
                finally
                {
                    Program.databanks.local = local4; Program.databanks.localGlobal = lg4; p.RemoveLast(); ;
                }
            });

        }

        public static readonly ScalarVal i33 = new ScalarVal(3d, 0);
        public static readonly ScalarVal i34 = new ScalarVal(4d, 0);
        public static readonly ScalarVal i39 = new ScalarVal(3d, 0);
        public static readonly ScalarVal i43 = new ScalarVal(3d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
            FunctionDef19();


            //[[commandEnd]]0

            FunctionDef26();


            //[[commandEnd]]2

            FunctionDef31();


            //[[commandEnd]]4


            C0(smpl, p);



        }
    }
}
