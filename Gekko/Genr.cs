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
        public static readonly ScalarVal i14 = new ScalarVal(5d);
        public static readonly ScalarVal i16 = new ScalarVal(6d);
        public static readonly ScalarVal i18 = new ScalarVal(2d);
        public static IVariable MapDef_mapTmpvar12(GekkoSmpl smpl)
        {


            return null;
        }
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            

            //[[splitSTOP]]


        }
    }
}
