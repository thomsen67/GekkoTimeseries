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
        public static GekkoTime globalGekkoTimeIterator = Globals.tNull;
        public static
         readonly ScalarVal i3 = new ScalarVal(5d);
        public static void ClearTS(P p)
        {
        }
        public static void
         ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoTime t =
             Globals.tNull;


            p.SetText(@"¤1");
            O.Reset o0 = new O.Reset();
            o0.p =
             p; o0.Exe();




            p.SetText(@"¤3");
            O.Series o1 = new O.Series();

            o1.lhs = null;
            o1.p =
             p;
            foreach (GekkoTime t2 in new GekkoTimeIterator(o1.t1, o1.t2))
            {
                t = t2;
                double data =
               O.GetVal(i3, t);
                
            }
            t = Globals.tNull;

            o1.Exe();




        }


        public static void CodeLines(P p)
        {
            GekkoTime t =
             Globals.tNull;

            C0(p);



        }
    }
}
