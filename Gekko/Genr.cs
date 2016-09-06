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
         readonly ScalarVal i24 = new ScalarVal(5d);
        public static void ClearTS(P p)
        {
        }
        public static
         void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoTime t =
             Globals.tNull;


            p.SetText(@"¤1");
            //o0.p = p;
            O.Genr o0 = new O.Genr();
            IVariable ts23 =
             O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new
             ScalarString("xx"))), 1, O.ECreatePossibilities.Can);
            o0.t1 = Globals.globalPeriodStart;
            o0.t2 =
             Globals.globalPeriodEnd;

            o0.lhs = null;
            foreach (GekkoTime t2 in new GekkoTimeIterator(o0.t1,
             o0.t2))
            {
                t = t2;
                double data = O.GetVal(i24, t);
                if (o0.lhs == null) o0.lhs =
                  O.GetTimeSeries(ts23);
                o0.lhs.SetData(t, data);
            }
            t = Globals.tNull;
            o0.meta = @"ser 
 xx=5";
            o0.Exe();





        }


        public static void CodeLines(P p)
        {
            GekkoTime t =
             Globals.tNull; 

            C0(p);



        }
    }
}
