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
         IVariable list3 = null;
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoTime t = Globals.tNull;


            p.SetText(@"¤1");
            O.Genr o0
             = new O.Genr();
            IVariable ts1 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" +
             O.GetString((new ScalarString("y"))), 1, O.ECreatePossibilities.Can);
            IVariable ts2 =
             O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new
             ScalarString("x"))), 1);
            o0.t1 = Globals.globalPeriodStart;
            o0.t2 =
             Globals.globalPeriodEnd;

            o0.lhs = null;
            o0.p = p;
            foreach (GekkoTime t2 in new
             GekkoTimeIterator(o0.t1, o0.t2))
            {
                t = t2;
                double data = O.GetVal((true
              ) ? (ts2) : (new
               ScalarVal(0d)), t);
                if (o0.lhs == null) o0.lhs = O.GetTimeSeries(ts1);
                o0.lhs.SetData(t,
                 data);
            }
            t = Globals.tNull;
            o0.meta = @"ser y = x $
 #m['a']";
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
