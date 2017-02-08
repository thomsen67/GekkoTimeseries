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
         readonly ScalarVal i18 = new ScalarVal(2010d);
        public static readonly ScalarVal i19 = new
         ScalarVal(2017d);
        public static readonly ScalarVal i20 = new ScalarVal(1d);
        public static readonly
         ScalarVal i21 = new ScalarVal(2d);
        public static readonly ScalarVal i22 = new
         ScalarVal(3d);
        public static readonly ScalarVal i23 = new ScalarVal(4d);
        public static readonly
         ScalarVal i24 = new ScalarVal(5d);
        public static readonly ScalarVal i25 = new
         ScalarVal(6d);
        public static readonly ScalarVal i26 = new ScalarVal(7d);
        public static readonly
         ScalarVal i27 = new ScalarVal(8d);
        public static readonly ScalarVal i30 = new
         ScalarVal(-1d);
        public static readonly ScalarVal i31 = new ScalarVal(2d);
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


            p.SetText(@"¤2");
            O.Reset o0 = new O.Reset();
            o0.p =
             p; o0.Exe();




            p.SetText(@"¤2");
            O.Mode o1 = new O.Mode();
            o1.mode =
             @"data"; o1.Exe();




            p.SetText(@"¤3");
            O.Time o2 = new O.Time();
            o2.t1 = O.GetDate(i18,
             O.GetDateChoices.FlexibleStart);
            ;
            o2.t2 = O.GetDate(i19,
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
            o3.data[0] = (i20).GetVal(t);
            o3.rep[0] =
             1d;
            o3.data[1] = (i21).GetVal(t);
            o3.rep[1] = 1d;
            o3.data[2] = (i22).GetVal(t);
            o3.rep[2] =
             1d;
            o3.data[3] = (i23).GetVal(t);
            o3.rep[3] = 1d;
            o3.data[4] = (i24).GetVal(t);
            o3.rep[4] =
             1d;
            o3.data[5] = (i25).GetVal(t);
            o3.rep[5] = 1d;
            o3.data[6] = (i26).GetVal(t);
            o3.rep[6] =
             1d;
            o3.data[7] = (i27).GetVal(t);
            o3.rep[7] = 1d;
            o3.Exe();





            p.SetText(@"¤5");
            O.Genr
             o4 = new O.Genr();
            IVariable ts28 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":"
             + O.GetString((new ScalarString("y"))), 1, O.ECreatePossibilities.Can);
            IVariable ts29 =
             O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new
             ScalarString("x"))), 1);
            o4.t1 = Globals.globalPeriodStart;
            o4.t2 =
             Globals.globalPeriodEnd;

            o4.lhs = null;
            o4.p = p;
            foreach (GekkoTime t2 in new
             GekkoTimeIterator(o4.t1, o4.t2))
            {
                t = t2;
                double[] storage32 = new double[(-O.GetInt(i31) +
                 1) - (0) + 1];
                int counter33 = 0;
                foreach (GekkoTime t3 in new
                 GekkoTimeIterator(t2.Add((-O.GetInt(i31) + 1)), t2.Add(0)))
                {
                    t = t3;
                    storage32[counter33] =
                     O.GetVal(O.Add(ts29, O.Indexer(ts29, i30, t), t), t);
                    counter33++;
                }
                double data =
               O.GetVal(O.HandleLags("movsum", storage32, (-O.GetInt(i31) + 1), 0), t);
                if (o4.lhs == null) o4.lhs
                  = O.GetTimeSeries(ts28);
                o4.lhs.SetData(t, data);
            }
            t = Globals.tNull;
            o4.meta = @"ser y = 
 movsum(x+x[-1],2)";
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
                    IVariable ts34 =
                     O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new
                     ScalarString("y"))), bankNumber);
                    foreach (GekkoTime t2 in new GekkoTimeIterator(o5.t1.Add(-2),
                     o5.t2))
                    {
                        t = t2;
                        O.GetVal777(ts34, bankNumber, ope5, t);
                    }
                    t = Globals.tNull;

                }
                o5.prtElements.Add(ope5);
            }


            o5.counter = 2;
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

