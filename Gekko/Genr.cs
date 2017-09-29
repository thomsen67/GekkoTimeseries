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
        public static readonly ScalarVal i17 = new ScalarVal(1d);
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
            IVariable ivTmpvar16 = O.Add(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "xx", null, null), O.Negate(smpl, i17)
            ), O.Lookup(smpl, null, null, "xx", null, null));
            O.Lookup(smpl, null, null, "xx2", null, ivTmpvar16)
            ;

        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = O.Smpl();

            C0(p);

        }
    }
}
