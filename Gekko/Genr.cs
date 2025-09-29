
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
            //[[commandStart]]0
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            Program.options.missing =
            O.XOptionSeriesMissing("missing", (new
            ScalarString("m")));
            O.PrintOptions("Program.options.missing",
            false);
            O.HandleOptions("Program.options.missing", 0, p);

            //[[commandEnd]]0
        }



        public
        static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl,
            p);



        }
    }
}