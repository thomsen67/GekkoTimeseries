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
         void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoTime t = Globals.tNull;


            p.SetText(@"¤1");
            O.Series o0 = new O.Series();
            o0.t1 =
             Globals.globalPeriodStart; o0.t2 = Globals.globalPeriodEnd;
            GekkoSmpl smpl = new GekkoSmpl(o0.t1,
             o0.t2);
            o0.p = p;
            IVariable ts4 = O.GetTimeSeries(smpl, O.GetString(new ScalarString("[FIRST]")) +
             ":" + O.GetString((new ScalarString("xx"))), 1, O.ECreatePossibilities.Can);
            IVariable ts5 =
             O.GetTimeSeries(smpl, O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new
             ScalarString("fe"))), 1);
            o0.lhs = null;
            ;
            o0.rhs = O.ConvertToTimeSeriesLight(O.Add(smpl, ts5,
             ts5));
            o0.Exe();




        }


        public static IVariable GekkoExpression1(GekkoSmpl smpl, int
         bankNumber, P p)
        {
            IVariable ts4 = O.GetTimeSeries(smpl, O.GetString(new ScalarString("[FIRST]")) +
             ":" + O.GetString((new ScalarString("xx"))), 1, O.ECreatePossibilities.Can);
            IVariable ts5 =
             O.GetTimeSeries(smpl, O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new
             ScalarString("fe"))), 1);

            return O.Add(smpl, ts5, ts5);
        }
        public static void CodeLines(P
         p)
        {
            GekkoTime t = Globals.tNull;

            C0(p);



        }
    }
}
