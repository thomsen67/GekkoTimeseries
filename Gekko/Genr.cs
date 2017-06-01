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
         readonly ScalarVal i12 = new ScalarVal(2010d);
        public static readonly ScalarVal i13 = new
         ScalarVal(2012d);
        public static readonly ScalarVal i14 = new ScalarVal(1d);
        public static readonly
         ScalarVal i15 = new ScalarVal(2d);
        public static readonly ScalarVal i16 = new
         ScalarVal(3d);
        public static readonly ScalarVal i17 = new ScalarVal(2010d);
        public static readonly
         ScalarVal i18 = new ScalarVal(2012d);
        public static IVariable scalar22 = null;
        public static void
         ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoTime t = Globals.tNull;


            p.SetText(@"¤1");
            O.Time o0 = new O.Time();
            o0.t1 =
             O.GetDate(i12, O.GetDateChoices.FlexibleStart);
            ;
            o0.t2 = O.GetDate(i13,
             O.GetDateChoices.FlexibleEnd);
            ;

            o0.Exe();




            p.SetText(@"¤0");
            O.Upd o1 = new
             O.Upd();
            o1.p = p;
            o1.meta = @"ser xx=1,2,3";
            o1.listItems = new
             List<string>();
            o1.listItems.AddRange(O.GetList((new ScalarString("xx"))));

            o1.op =
             "=";
            o1.data = new double[3];
            o1.rep = new double[3];
            o1.data[0] = (i14).GetVal(t);
            o1.rep[0] =
             1d;
            o1.data[1] = (i15).GetVal(t);
            o1.rep[1] = 1d;
            o1.data[2] = (i16).GetVal(t);
            o1.rep[2] =
             1d;
            o1.Exe();


                double[] storage19 = new double[Math.Max(0, GekkoTime.Observations(O.GetDate(i17), O.GetDate(i18)))];
                int counter20 =
                 0;
                foreach (GekkoTime t2 in new GekkoTimeIterator(O.GetDate(i17), O.GetDate(i18)))
                {
                    t =
                     t2;
                    storage19[counter20] = O.GetVal(O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":"
                     + O.GetString((new ScalarString("xx"))), 1), t);
                    counter20++;
                    //             t = t1;
                }


                IVariable zzz = O.HandleLags("avgt", storage19);

           


            //find start of statement, and put zz stuff in there instead of in ()

            double tempDouble21 = (zzz).GetVal(t);
            
            O.SetValFromCache(ref scalar22, "v", tempDouble21);


        }


        public
         static void CodeLines(P p)
        {
            GekkoTime t = Globals.tNull;

            C0(p);



        }
    }
}
