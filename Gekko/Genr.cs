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
        public static readonly ScalarVal i28 = new ScalarVal(2d);
        public static readonly ScalarVal i29 = new ScalarVal(3d);
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

            //IVariable ivTmpvar27 = O.TypeCheck_series(O.ListDefHelper(i28, i29), -1);
            IVariable ivTmpvar27 = O.ListDefHelper(i28, i29);
            O.Lookup(smpl, null, null, "y", null, ivTmpvar27, true, EVariableType.Series)
            ;


            //[[splitSTOP]]


        }
    }
}
