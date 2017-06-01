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
         readonly ScalarVal i26 = new ScalarVal(2d);
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
            O.Genr o0 = new O.Genr();
            IVariable ts24 =
             O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new
             ScalarString("y"))), 1, O.ECreatePossibilities.Can);
            IVariable ts25 =
             O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new
             ScalarString("xx"))), 1);
            o0.t1 = Globals.globalPeriodStart;
            o0.t2 =
             Globals.globalPeriodEnd;

            o0.lhs = null;
            o0.p = p;
            foreach (GekkoTime t2 in new  GekkoTimeIterator(o0.t1, o0.t2))
            {
                t = t2;
                double[] storage27 = new double[2];  //changed
                int counter28 = 0;
                //foreach (GekkoTime t3 in new GekkoTimeIterator(t2.Add((-O.GetInt(i26) + 1)), t2.Add(0)))
                foreach (string s1177 in new List<string> { "a", "b" })  //changed
                {
                    //t = t3;
                    //storage27[counter28] = O.GetVal(ts25, t);
                    storage27[counter28] = O.GetVal(O.Indexer(t, ts25, false, new ScalarString(s1177)), t);  //changed
                    counter28++;
                    //t = t2;
                }
                double data = O.GetVal(O.HandleSummations("sum", storage27), t);
                if (o0.lhs == null) o0.lhs = O.GetTimeSeries(ts24);
                o0.lhs.SetData(t, data);
            }
            t
             = Globals.tNull;
            o0.meta = @"ser y= movavg(xx, 2)";
            o0.Exe();





        }


        public static
         void CodeLines(P p)
        {
            GekkoTime t = Globals.tNull;

            C0(p);



        }
    }
}

