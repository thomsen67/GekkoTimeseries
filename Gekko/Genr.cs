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
        public static readonly ScalarVal i1 = new ScalarVal(2d);
        public static void ClearTS(P p) {
        }
        public static void ClearScalar(P p) {
        }
        public static void C0(P p) {

            GekkoTime t = Globals.tNull;


            p.SetText(@"¤1");




            p.SetText(@"¤2");
            //UProc.tt(p, t, i1);
        


p.SetText(@"¤8");
            Program.Mem(null);




        }


        public static void CodeLines(P p)
        {
            GekkoTime t = Globals.tNull;

            C0(p);



        }
    }
}
