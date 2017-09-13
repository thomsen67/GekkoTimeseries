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
        public static readonly ScalarVal i48 = new ScalarVal(2d);
        public static readonly ScalarVal i49 = new ScalarVal(1d);
        public static readonly ScalarVal i50 = new ScalarVal(2d);
        public static readonly ScalarVal i51 = new ScalarVal(77d);
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoSmpl smpl = O.Smpl();

            Map m = new Map();
            O.Lookup(smpl, null, "%d", null, true, new ScalarVal(2d));
            O.Lookup(smpl, null, "%s", null, true, new ScalarString("a"));


        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = O.Smpl();

            C0(p);



        }
    }
}
