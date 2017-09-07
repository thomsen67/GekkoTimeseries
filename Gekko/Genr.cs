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




            p.SetText(@"¤0");
            IVariable ivTmpvar16 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"), new ScalarString(@"c"));
            O.Lookup(smpl, null, "#m", null, true, ivTmpvar16)
            ;




            p.SetText(@"¤0");

        }

        public static void C1(P p)
        {

            GekkoSmpl smpl = O.Smpl();





            p.SetText(@"¤0");
            IVariable ivTmpvar19 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"), new ScalarString(@"c"));
            O.Lookup(smpl, null, "#m", null, true, ivTmpvar19)
            ;




            p.SetText(@"¤0");

        }

        public static void C2(P p)
        {

            GekkoSmpl smpl = O.Smpl();





        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = O.Smpl();

            C0(p);

            IVariable forloop_17 = null;
            int counter18 = 0;
            for (O.IterateStart(ref forloop_17, O.Lookup(smpl, null, "#m", null, true, null)); O.IterateContinue(forloop_17, O.Lookup(smpl, null, "#m", null, true, null), null, null, ref counter18); O.IterateStep(forloop_17, O.Lookup(smpl, null, "#m", null, true, null), null, counter18))
            {
                ;
                O.Print(smpl, (forloop_17));

            };

            C1(p);

            IVariable forloop_20 = null;
            int counter21 = 0;
            for (O.IterateStart(ref forloop_20, O.Lookup(smpl, null, "#m", null, true, null)); O.IterateContinue(forloop_20, O.Lookup(smpl, null, "#m", null, true, null), null, null, ref counter21); O.IterateStep(forloop_20, O.Lookup(smpl, null, "#m", null, true, null), null, counter21))
            {
                ;
                O.Print(smpl, (forloop_20));

            };

            C2(p);



        }
    }
}
