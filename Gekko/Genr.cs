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

        public static void CC0(GekkoSmpl smpl, P p, ref IVariable xfunctionarg_xf7dke8cj_13, ref IVariable xfunctionarg_xf7dke8cj_14, ref IVariable xfunctionarg_xf7dke8cj_15)
        {
            IVariable functionarg_xf7dke8cj_13 = xfunctionarg_xf7dke8cj_13;

            IVariable functionarg_xf7dke8cj_14 = xfunctionarg_xf7dke8cj_14;

            IVariable functionarg_xf7dke8cj_15 = xfunctionarg_xf7dke8cj_15;

            //[[commandStart]]1
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            Program.Tell(O.ConvertToString(O.HandleString(new ScalarString(@"")).Add(smpl, O.CurlyMethod(smpl, functionarg_xf7dke8cj_13)).Add(smpl, O.HandleString(new ScalarString(@" "))).Add(smpl, O.CurlyMethod(smpl, functionarg_xf7dke8cj_14)).Add(smpl, O.HandleString(new ScalarString(@" "))).Add(smpl, O.CurlyMethod(smpl, functionarg_xf7dke8cj_15)).Add(smpl, O.HandleString(new ScalarString(@"")))), false);
            //[[commandEnd]]1
            xfunctionarg_xf7dke8cj_13 = functionarg_xf7dke8cj_13;

            xfunctionarg_xf7dke8cj_14 = functionarg_xf7dke8cj_14;

            xfunctionarg_xf7dke8cj_15 = functionarg_xf7dke8cj_15;

        }

        public static readonly ScalarVal i16 = new ScalarVal(5d, 0);
        public static void FunctionDef17()
        {

            O.PrepareUfunction(5, "f");
            Globals.ufunctionsNew5.Add("f", (GekkoSmpl smpl, P p, GekkoArg functionarg_xf7dke8cj_11_func, GekkoArg functionarg_xf7dke8cj_12_func, GekkoArg functionarg_xf7dke8cj_13_func, GekkoArg functionarg_xf7dke8cj_14_func, GekkoArg functionarg_xf7dke8cj_15_func) =>
            {
                //function val f(val % x1, val % x2, val % x3); tell'{%1} {%3} {%3}'; return 5; end;
                //calling %v = f(1, 2, 3);

                IVariable functionarg_xf7dke8cj_11 = O.TypeCheck_date(functionarg_xf7dke8cj_11_func, smpl, 1);
                IVariable functionarg_xf7dke8cj_12 = O.TypeCheck_date(functionarg_xf7dke8cj_12_func, smpl, 2);
                IVariable functionarg_xf7dke8cj_13 = O.TypeCheck_val(functionarg_xf7dke8cj_13_func.f1(smpl), 3); //x1
                IVariable functionarg_xf7dke8cj_14 = O.TypeCheck_val(functionarg_xf7dke8cj_14_func.f1(smpl), 4); //x2
                IVariable functionarg_xf7dke8cj_15 = O.TypeCheck_val(functionarg_xf7dke8cj_15_func.f1(smpl), 5); //x3

                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
                try
                {
                    CC0(smpl, p, ref functionarg_xf7dke8cj_13, ref functionarg_xf7dke8cj_14, ref functionarg_xf7dke8cj_15);
                    return O.TypeCheck_val(i16, 0);
                }
                catch { p.Deeper(); throw; }
                finally
                {
                    Program.databanks.local = local0; Program.databanks.localGlobal = lg0; p.RemoveLast(); ;
                }
            });

            O.PrepareUfunction(3, "f");
            Globals.ufunctionsNew3.Add("f", (GekkoSmpl smpl, P p, GekkoArg functionarg_xf7dke8cj_11_func, GekkoArg functionarg_xf7dke8cj_12_func, GekkoArg functionarg_xf7dke8cj_13_func) =>
            {                
                //Optional parameters must be used to create overloads and potential questions.

                //drops 2
                //instead of calling %v = f(1, 2, 3);
                //--> calling %v = f(1); which becomes f(1, 11, 22).
                //if called with %v = f?(1), it will ask.                

                IVariable iv2 = null;
                IVariable iv3 = null;

                bool question = true;
                IVariable iv2Default = new ScalarVal(111d);
                IVariable iv3Default = new ScalarVal(222d);

                if (question)
                {
                    string type2 = "val";
                    string d2 = null;
                    string s2 = O.AcceptHelper2(type2, iv2Default);
                    if (s2 != null) d2 = " (default = " + s2 + ")";
                    string message2 = "Tast2" + d2;
                    iv2 = O.ProcedureAccept(type2, message2);

                    string type3 = "val";
                    string d3 = null;
                    string s3 = O.AcceptHelper2(type3, iv3Default);
                    if (s3 != null) d3 = " (default = " + s3 + ")";
                    string message3 = "Tast3" + d3;
                    iv3 = O.ProcedureAccept(type3, message3);

                }
                else
                {
                    iv2 = iv2Default;
                    iv3 = iv3Default;
                }

                GekkoArg ga2 = new GekkoArg((smpl777) => iv2, null);
                GekkoArg ga3 = new GekkoArg((smpl777) => iv3, null);

                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar11 = O.FunctionLookupNew5("f")(smpl, p, 
                    functionarg_xf7dke8cj_11_func, 
                    functionarg_xf7dke8cj_12_func, 
                    functionarg_xf7dke8cj_13_func, 
                    ga2, 
                    ga3
                );
                O.AdjustT0(smpl, 2);

                return ivTmpvar11;

            });



                                 
        }

        

        private static GekkoArg GA3()
        {
            return new GekkoArg((spml14) => O.Lookup(spml14, null, null, "%x3",
                             null, null, new LookupSettings(), EVariableType.Var, null), (spml14) => new
                            ScalarString("%x3"));
        }

        private static GekkoArg GA2()
        {
            return new GekkoArg((spml13) =>
                             O.Lookup(spml13, null, null, "%x2", null, null, new LookupSettings(), EVariableType.Var, null),
                             (spml13) => new ScalarString("%x2"));
        }

        private static GekkoArg GA1()
        {
            return new
                             GekkoArg((spml12) => O.Lookup(spml12, null, null, "%x1", null, null, new LookupSettings(),
                             EVariableType.Var, null), (spml12) => new ScalarString("%x1"));
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
            FunctionDef17();


            //[[commandEnd]]0



        }
    }
}
