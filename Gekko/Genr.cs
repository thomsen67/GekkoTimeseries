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
         readonly ScalarVal i11 = new ScalarVal(100d);
        public static readonly ScalarVal i14 = new
         ScalarVal(-1d);
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoTime t = Globals.tNull;


            p.SetText(@"¤2");
            O.Reset
             o0 = new O.Reset();
            o0.p = p; o0.Exe();




            p.SetText(@"¤2");
            O.Mode o1 = new
             O.Mode();
            o1.mode = @"data"; o1.Exe();




            p.SetText(@"¤3");
            O.Genr o2 = new
             O.Genr();
            IVariable ts10 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" +
             O.GetString((new ScalarString("x"))), 1, O.ECreatePossibilities.Can);
            o2.t1 =
             Globals.globalPeriodStart;
            o2.t2 = Globals.globalPeriodEnd;

            o2.lhs = null;
            o2.p = p;
            foreach
             (GekkoTime t2 in new GekkoTimeIterator(o2.t1, o2.t2))
            {
                t = t2;
                double data = O.GetVal(i11,
               t);
                if (o2.lhs == null) o2.lhs = O.GetTimeSeries(ts10);
                o2.lhs.SetData(t, data);
            }
            t =
             Globals.tNull;
            o2.meta = @"ser x = 100";
            o2.Exe();





            p.SetText(@"¤4");
            O.Genr o3 = new
             O.Genr();
            IVariable ts12 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" +
             O.GetString((new ScalarString("y"))), 1, O.ECreatePossibilities.Can);
            IVariable ts13 =
             O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new
             ScalarString("x"))), 1);
            o3.t1 = Globals.globalPeriodStart;
            o3.t2 =
             Globals.globalPeriodEnd;

            o3.lhs = null;
            o3.p = p;
            foreach (GekkoTime t2 in new
             GekkoTimeIterator(o3.t1, o3.t2))
            {
                t = t2;
                int lag1_15 = -1;
                int lag2_16 = 0;
                double[]
                 storage17 = new double[lag2_16 - lag1_15 + 1];
                int counter18 = 0;
                foreach (GekkoTime t3 in new
                 GekkoTimeIterator(t2.Add(-1), t2.Add(0)))
                {
                    t = t3;
                    storage17[counter18] = O.GetVal(O.Add(ts13,
                     O.Indexer(ts13, i14, t), t), t);
                    counter18++;
                }
                double data =
               O.GetVal(O.HandleLags(O.LagType.Movsum, storage17), t);
                if (o3.lhs == null) o3.lhs =
                  O.GetTimeSeries(ts12);
                o3.lhs.SetData(t, data);
            }
            t = Globals.tNull;
            o3.meta = @"ser y = 
 pch(x+x[-1])";
            o3.Exe();





        }


        public static void CodeLines(P p)
        {
            GekkoTime t =
             Globals.tNull;

            C0(p);



        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Text;
//using
// System.Windows.Forms;
//using System.Drawing;
//using Gekko.Parser;
//namespace Gekko
//{
//    public class
//     TranslatedCode
//    {
//        public static GekkoTime globalGekkoTimeIterator = Globals.tNull;
//        public static
//         readonly ScalarVal i7 = new ScalarVal(100d);
//        public static readonly ScalarVal i10 = new
//         ScalarVal(-1d);
//        public static void ClearTS(P p)
//        {
//        }
//        public static void ClearScalar(P p)
//        {
//        }
//        public static void C0(P p)
//        {

//            GekkoTime t = Globals.tNull;


//            p.SetText(@"¤2");
//            O.Reset
//             o0 = new O.Reset();
//            o0.p = p; o0.Exe();




//            p.SetText(@"¤2");
//            O.Mode o1 = new
//             O.Mode();
//            o1.mode = @"data"; o1.Exe();




//            p.SetText(@"¤3");
//            O.Genr o2 = new
//             O.Genr();

//            IVariable ts6 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" +
//             O.GetString((new ScalarString("x"))), 1, O.ECreatePossibilities.Can);
//            o2.t1 =
//             Globals.globalPeriodStart;
//            o2.t2 = Globals.globalPeriodEnd;

//            o2.lhs = null;
//            o2.p = p;
//            foreach
//             (GekkoTime t2 in new GekkoTimeIterator(o2.t1, o2.t2))
//            {
//                t = t2;
//                double data = O.GetVal(i7,
//               t);
//                if (o2.lhs == null) o2.lhs = O.GetTimeSeries(ts6);
//                o2.lhs.SetData(t, data);
//            }
//            t =
//             Globals.tNull;
//            o2.meta = @"ser x = 100";
//            o2.Exe();





//            p.SetText(@"¤4");
//            O.Genr o3 = new
//             O.Genr();
//            IVariable ts8 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" +
//             O.GetString((new ScalarString("y"))), 1, O.ECreatePossibilities.Can);
//            IVariable ts9 =
//             O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new
//             ScalarString("x"))), 1);
//            o3.t1 = Globals.globalPeriodStart;
//            o3.t2 =
//             Globals.globalPeriodEnd;

//            o3.lhs = null;
//            o3.p = p;

//            IVariable ts4 = null, ts3 = null, i5 = null;

//            foreach (GekkoTime t2 in new GekkoTimeIterator(o3.t1, o3.t2))
//            {

//                int lag1 = -4;
//                int lag2 = 0;
//                double[] storage = new double[lag2 - lag1 + 1];                
//                int counter = 0;
//                foreach (GekkoTime t3 in new GekkoTimeIterator(t2.Add(-5), t2))
//                {                    
//                    t = t3;
//                    storage[counter++] = O.GetVal(O.Multiply(i5, O.Add(ts6, O.Indexer(ts6, i7, t), t), t), t);
//                    counter++;
//                }
//                double data2 = storage[0]; //PUT INSIDE METHOD

//                t = t2;
//                double data = O.GetVal(O.Add(ts4, new ScalarVal(data2), t), t);
//                //double data = O.GetVal(O.Add(ts4, O.Multiply(i5, O.Add(ts6, O.Indexer(ts6, i7, t), t), t), t), t);
//                if (o3.lhs == null) o3.lhs =
//                  O.GetTimeSeries(ts3);
//                o3.lhs.SetData(t, data);

//                // z + 1*(x+x[-1]);
//            }

//            t = Globals.tNull;
//            o3.meta = @"ser y = x+x[-1]";
//            o3.Exe();





//        }


//        public static void
//         CodeLines(P p)
//        {
//            GekkoTime t = Globals.tNull;

//            C0(p);



//        }
//    }
//}