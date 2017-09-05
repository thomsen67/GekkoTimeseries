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
        public static GekkoTime globalGekkoTimeIterator = Globals.tNull;
        public static void FunctionDef2()
        {


            //[[splitSTOP]]

            Globals.ufunctions1.Add("f", (GekkoSmpl smpl, IVariable functionarg_1) =>
            {
                //[[splitSTOP]]
                return O.Add(smpl, functionarg_1, functionarg_1);

                //[[splitSTART]]

                ; return null;
            });


            //[[splitSTART]]

        }

        public static readonly ScalarVal i4 = new ScalarVal(100d);
        public static readonly ScalarVal i7 = new ScalarVal(1d);
        public static readonly ScalarVal i9 = new ScalarVal(2d);
        public static readonly ScalarVal i11 = new ScalarVal(3d);
        public static readonly ScalarVal i13 = new ScalarVal(4d);
        public static IVariable temp18(GekkoSmpl smpl)
        {
            TimeSeries temp18 = new TimeSeries(Program.options.freq, null); temp18.SetZero(smpl);

            foreach (IVariable listloop_m116 in new O.GekkoListIterator(O.Lookup(smpl, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null)))
            {
                foreach (IVariable listloop_m217 in new O.GekkoListIterator(O.Lookup(smpl, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null)))
                {
                    temp18.InjectAdd(smpl, temp18, O.Indexer(smpl, O.Lookup(smpl, null, "xx", null, false, null), false, listloop_m116, listloop_m217));

                }
            }
            return temp18;

        }
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoSmpl smpl = O.Smpl();


            p.SetText(@"¤0");
            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe();




            p.SetText(@"¤2");
            FunctionDef2();





            p.SetText(@"¤0");
            IVariable ivTmpvar3 = i4;
            O.Lookup(smpl, null, "%v1", null, true, ivTmpvar3)
            ;




            p.SetText(@"¤0");
            IVariable ivTmpvar5 = Globals.ufunctions1["f"](smpl, Globals.ufunctions1["f"](smpl, O.Lookup(smpl, null, "%v1", null, true, null)));
            O.Lookup(smpl, null, "%v2", null, true, ivTmpvar5)
            ;




            p.SetText(@"¤0");
            O.Print(smpl, (O.Lookup(smpl, null, "%v2", null, true, null)));




            p.SetText(@"¤0");
            IVariable ivTmpvar6 = i7;
            O.Lookup(smpl, null, "xx___a___x", null, false, ivTmpvar6)
            ;




            p.SetText(@"¤0");
            IVariable ivTmpvar8 = i9;
            O.Lookup(smpl, null, "xx___b___x", null, false, ivTmpvar8)
            ;




            p.SetText(@"¤0");
            IVariable ivTmpvar10 = i11;
            O.Lookup(smpl, null, "xx___a___y", null, false, ivTmpvar10)
            ;




            p.SetText(@"¤0");
            IVariable ivTmpvar12 = i13;
            O.Lookup(smpl, null, "xx___b___y", null, false, ivTmpvar12)
            ;




        }

        public static void C1(P p)
        {

            GekkoSmpl smpl = O.Smpl();

            p.SetText(@"¤0");
            IVariable ivTmpvar14 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"));
            O.Lookup(smpl, null, "#m1", null, true, ivTmpvar14)
            ;




            p.SetText(@"¤0");
            IVariable ivTmpvar15 = O.ListDef(new ScalarString(@"x"), new ScalarString(@"y"));
            O.Lookup(smpl, null, "#m2", null, true, ivTmpvar15)
            ;




            p.SetText(@"¤18");
            O.Print(smpl, (temp18(smpl)));




        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = O.Smpl();

            C0(p);

            C1(p);



        }
    }
}
