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
        public static readonly ScalarVal i1 = new ScalarVal(10d);
        public static IVariable list4 = null;
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
            O.List o0 = new O.List();
            o0.name = O.GetString((new ScalarString("i")));
            o0.listItems = new List<string>();
            o0.p = p;
            o0.listItems = new List<string>();
            o0.listItems.AddRange(O.GetList((new ScalarString("a"))));

            o0.Exe();




            p.SetText(@"¤3");
            O.Series o1 = new O.Series();

            o1.lhs = null;
            o1.p = p;
            foreach (GekkoTime t2 in new GekkoTimeIterator(o1.t1, o1.t2))
            {
                t = t2;
                double data = O.GetVal(i1, t);
                if (o1.lhs == null) o1.lhs = O.GetTimeSeries(O.Indexer(t, O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x"))), 1, O.ECreatePossibilities.Can), true, new ScalarString(@"a"))
                 );
                o1.lhs.SetData(t, data);
            }
            t = Globals.tNull;
            o1.Exe();




            p.SetText(@"¤4");
            O.Genr o2 = new O.Genr();
            IVariable ts2 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("y"))), 1, O.ECreatePossibilities.Can);
            IVariable ts5 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x"))), 1);
            o2.t1 = Globals.globalPeriodStart;
            o2.t2 = Globals.globalPeriodEnd;

            o2.lhs = null;
            o2.p = p;
            foreach (GekkoTime t2 in new GekkoTimeIterator(o2.t1, o2.t2))
            {
                t = t2;
                IVariable temp8 = O.GetScalarFromCache(ref list4, "#i", false, false);

                double[] storage6 = new double[((MetaList)temp8).Count()];
                int counter7 = 0;
                {
                    GekkoTime t3 = t2;

                    foreach (IVariable listloop_i3 in new O.GekkoListIterator(temp8))
                    {
                        t = t3;
                        storage6[counter7] = O.GetVal(O.Indexer(t, ts5, false, listloop_i3), t);
                        counter7++;
                    }
                }
                IVariable temp12 = O.GetScalarFromCache(ref list4, "#i", false, false);


                double[] storage10 = new double[((MetaList)temp12).Count()];
                int counter11 = 0;
                {
                    GekkoTime t3 = t2;

                    foreach (IVariable listloop_i9 in new O.GekkoListIterator(temp12))
                    {
                        t = t3;
                        storage10[counter11] = O.GetVal(O.Indexer(t, ts5, false, listloop_i9), t);
                        counter11++;
                    }
                }
                double data = O.GetVal(O.Add(O.HandleSummations("sum", storage6), O.HandleSummations("sum", storage10), t), t);
                if (o2.lhs == null) o2.lhs = O.GetTimeSeries(ts2);
                o2.lhs.SetData(t, data);
            }
            t = Globals.tNull;
            o2.meta = @"ser y = sum(#i, x[#i]) + sum(#i, x[#i])";
            o2.Exe();





            p.SetText(@"¤5");
            O.Prt o3 = new O.Prt();
            o3.prtType = "print";

            {
                List<int> bankNumbers = null;
                O.Prt.Element ope3 = new O.Prt.Element();
                ope3.label = O.SubstituteScalarsAndLists("y", false);
                bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o3, ope3));
                foreach (int bankNumber in bankNumbers)
                {
                    IVariable ts13 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("y"))), bankNumber);
                    foreach (GekkoTime t2 in new GekkoTimeIterator(o3.t1.Add(-2), o3.t2))
                    {
                        t = t2;
                        O.GetVal777(ts13, bankNumber, ope3, t);
                    }
                    t = Globals.tNull;
                }
                o3.prtElements.Add(ope3);
            }


            o3.counter = 1;
            o3.Exe();




        }


        public static void CodeLines(P p)
        {
            GekkoTime t = Globals.tNull;

            C0(p);



        }
    }
}
