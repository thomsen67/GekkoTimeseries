using System;
using System.Collections.Generic;
using System.Text;
using
System.Windows.Forms;
using System.Drawing;
using Gekko.Parser;
namespace Gekko
{
    public class
    TranslatedCode
    {
        public static GekkoTime globalGekkoTimeIterator = GekkoTime.tNull;
        public static
        int labelCounter;
        public static void C0(GekkoSmpl smpl, P p)
        {
            Action
assign_156 = () => {
    O.AdjustT0(smpl, -2);
    IVariable ivTmpvar153 = O.FunctionLookupNew3(p, null, "loog")(smpl, p, false, null, null, new GekkoArg((spml155) => new ScalarVal(2d), (spml155) => null));
    O.AdjustT0(smpl, 2);
    
    ;
};

        }



        public static void CodeLines(P
        p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
