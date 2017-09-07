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
        public static readonly ScalarVal i2 = new ScalarVal(0d);
        public static readonly ScalarVal i4 = new ScalarVal(1d);
        public static readonly ScalarVal i5 = new ScalarVal(1000d);
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
            IVariable ivTmpvar1 = i2;
            O.Lookup(smpl, null, "%sum", null, true, ivTmpvar1)
            ;




            p.SetText(@"¤0");

        }

        public static void C1(P p)
        {

            GekkoSmpl smpl = O.Smpl();





            p.SetText(@"¤0");
            O.Print(smpl, (O.Lookup(smpl, null, "%sum", null, true, null)));




            p.SetText(@"¤0");
            O.Print(smpl, (O.Lookup(smpl, null, "%i", null, true, null)));




        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = O.Smpl();

            C0(p);

            IVariable forloop_3 = null;
            for (O.IterateStart(ref forloop_3, i4); O.IterateContinue(forloop_3, i5, null); O.IterateStep(forloop_3, null))
            {
                ;
                IVariable ivTmpvar6 = O.Add(smpl, O.Lookup(smpl, null, "%sum", null, true, null), forloop_3);
                O.Lookup(smpl, null, "%sum", null, true, ivTmpvar6)
                ;

            };

            C1(p);

        }
    }
}
