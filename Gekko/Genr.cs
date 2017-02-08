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
         readonly ScalarVal i1 = new ScalarVal(2010d);
        public static readonly ScalarVal i2 = new
         ScalarVal(2017d);
        public static readonly ScalarVal i3 = new ScalarVal(1d);
        public static readonly
         ScalarVal i4 = new ScalarVal(2d);
        public static readonly ScalarVal i5 = new ScalarVal(3d);
        public
         static readonly ScalarVal i6 = new ScalarVal(4d);
        public static readonly ScalarVal i7 = new
         ScalarVal(5d);
        public static readonly ScalarVal i8 = new ScalarVal(6d);
        public static readonly
         ScalarVal i9 = new ScalarVal(7d);
        public static readonly ScalarVal i10 = new ScalarVal(8d);
        public
         static readonly ScalarVal i13 = new ScalarVal(-0d);
        public static readonly ScalarVal i14 = new
         ScalarVal(2d);
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
            O.Time o2 = new O.Time();
            o2.t1
             = O.GetDate(i1, O.GetDateChoices.FlexibleStart);
            ;
            o2.t2 = O.GetDate(i2,
             O.GetDateChoices.FlexibleEnd);
            ;

            o2.Exe();




            p.SetText(@"¤0");
            O.Upd o3 = new
             O.Upd();
            o3.p = p;
            o3.meta = @"ser x = 1,2,3,4,5,6,7,8";
            o3.listItems = new
             List<string>();
            o3.listItems.AddRange(O.GetList((new ScalarString("x"))));

            o3.op = "=";
            o3.data
             = new double[8];
            o3.rep = new double[8];
            o3.data[0] = (i3).GetVal(t);
            o3.rep[0] = 1d;
            o3.data[1]
             = (i4).GetVal(t);
            o3.rep[1] = 1d;
            o3.data[2] = (i5).GetVal(t);
            o3.rep[2] = 1d;
            o3.data[3] =
             (i6).GetVal(t);
            o3.rep[3] = 1d;
            o3.data[4] = (i7).GetVal(t);
            o3.rep[4] = 1d;
            o3.data[5] =
             (i8).GetVal(t);
            o3.rep[5] = 1d;
            o3.data[6] = (i9).GetVal(t);
            o3.rep[6] = 1d;
            o3.data[7] =
             (i10).GetVal(t);
            o3.rep[7] = 1d;
            o3.Exe();





            p.SetText(@"¤5");
            O.Genr o4 = new
             O.Genr();
            IVariable ts11 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" +
             O.GetString((new ScalarString("y"))), 1, O.ECreatePossibilities.Can);
            IVariable ts12 =
             O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new
             ScalarString("x"))), 1);
            o4.t1 = new GekkoTime(EFreq.Annual, 2012, 1);
            o4.t2 =
             Globals.globalPeriodEnd;

            o4.lhs = null;
            o4.p = p;
            foreach (GekkoTime t2 in new
             GekkoTimeIterator(o4.t1, o4.t2))
            {
                t = t2;



                double[] storage15 = new double[0 - ((-O.GetInt(i14) + 1)) + 1];
                int counter16 = 0;
                foreach (GekkoTime t3 in new GekkoTimeIterator(t2.Add((-O.GetInt(i14) + 1)), t2.Add(0)))
                {
                    t = t3;

                    //==========================
                    double[] storage16 = new double[0 - ((-O.GetInt(i14) + 1)) + 1];
                    int counter17 = 0;
                    foreach (GekkoTime t4 in new GekkoTimeIterator(t3.Add((-O.GetInt(i14) + 1)), t3.Add(0)))
                    {
                        t = t4;
                        storage16[counter17] = O.GetVal(ts12, t);
                        counter17++;
                    }
                    
                    //==========================

                    storage15[counter16] =  O.GetVal(O.HandleLags("movsum", storage16, (-O.GetInt(i14) + 1), 0), t);
                    counter16++;
                }




                double data =  O.GetVal(O.HandleLags("movsum", storage15, (-O.GetInt(i14) + 1), 0), t);
                if (o4.lhs == null) o4.lhs = O.GetTimeSeries(ts11);
                o4.lhs.SetData(t, data);
            }
            t = Globals.tNull;
            o4.meta = @"ser y = 
 movsum(x+x[-1],5)";
            o4.Exe();





            p.SetText(@"¤6");
            O.Prt o5 = new O.Prt();
            o5.prtType =
             "prt";

            {
                List<int> bankNumbers = null;
                O.Prt.Element ope5 = new O.Prt.Element();
                ope5.label =
                 O.SubstituteScalarsAndLists("y", false);
                bankNumbers = O.Prt.GetBankNumbers(null,
                 Program.GetElementPrintCodes(o5, ope5));
                foreach (int bankNumber in bankNumbers)
                {
                    IVariable ts17 =
                     O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new
                     ScalarString("y"))), bankNumber);
                    foreach (GekkoTime t2 in new GekkoTimeIterator(o5.t1.Add(-2),
                     o5.t2))
                    {
                        t = t2;
                        O.GetVal777(ts17, bankNumber, ope5, t);
                    }
                    t = Globals.tNull;

                }
                o5.prtElements.Add(ope5);
            }


            o5.counter = 1;
            o5.Exe();




        }


        public static void
         CodeLines(P p)
        {
            GekkoTime t = Globals.tNull;

            C0(p);



        }
    }
}
