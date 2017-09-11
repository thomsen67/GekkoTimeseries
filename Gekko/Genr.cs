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
        public static readonly ScalarVal i2 = new ScalarVal(5d);
        public static readonly ScalarVal i4 = new ScalarVal(1d);
        public static readonly ScalarVal i5 = new ScalarVal(2010d);
        public static readonly ScalarVal i6 = new ScalarVal(6d);
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
            O.Lookup(smpl, null, "xx", null, false, ivTmpvar1)
            ;




            p.SetText(@"¤0");
            IVariable ivTmpvar3 = i6;
            O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, "xx", null, false, ivTmpvar3), false, O.Negate(smpl, i4)
            ), false, i5
            )
            ;




        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = O.Smpl();

            C0(p);



        }
    }
}
