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
        public static IVariable list213 = null;
        public static IVariable list214 = null;
        public static IVariable scalar219 = null;
        public static IVariable scalar220 = null;
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
            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe();




            p.SetText(@"¤2");
            O.List o1 = new O.List();
            o1.name = O.GetString((new ScalarString("m1")));
            o1.listItems = new List<string>();
            o1.p = p;
            o1.listItems = new List<string>();
            o1.listItems.AddRange(O.GetList((new ScalarString("a1"))));

            o1.listItems.AddRange(O.GetList((new ScalarString("b1"))));

            o1.Exe();




            p.SetText(@"¤3");
            O.List o2 = new O.List();
            o2.name = O.GetString((new ScalarString("m2")));
            o2.listItems = new List<string>();
            o2.p = p;
            o2.listItems = new List<string>();
            o2.listItems.AddRange(O.GetList((new ScalarString("a2"))));

            o2.listItems.AddRange(O.GetList((new ScalarString("b2"))));

            o2.Exe();




            p.SetText(@"¤4");
            O.List o3 = new O.List();
            o3.name = O.GetString((new ScalarString("m3")));
            o3.listItems = new List<string>();
            o3.p = p;
            o3.listItems = new List<string>();
            o3.listItems.AddRange(O.GetList((new ScalarString("a3"))));

            o3.listItems.AddRange(O.GetList((new ScalarString("b3"))));

            o3.Exe();



            p.SetText(@"¤5");

        }

        public static void C1(P p)
        {

            GekkoTime t = Globals.tNull;


            p.SetText(@"¤7");
            Program.Tell(O.GetString(new ScalarString(@"hej")), false);



        }

        public static void C2(P p)
        {

            GekkoTime t = Globals.tNull;




        }


        public static void CodeLines(P p)
        {
            GekkoTime t = Globals.tNull;

            C0(p);

            O.ForString o4 = new O.ForString();
            List<List<string>> test215 = new List<List<string>>();
            List<string> test216 = new List<string>();
            o4.listItems = new List<string>();
            o4.listItems.AddRange(O.GetList(O.GetScalarFromCache(ref list213, "#m1", false, false)));

            List<string> x4_0 = o4.listItems;
            test215.Add(x4_0);
            test216.Add("i");
            o4.listItems = new List<string>();
            o4.listItems.AddRange(O.GetList(O.GetScalarFromCache(ref list214, "#m2", false, false)));

            List<string> x4_1 = o4.listItems;
            test215.Add(x4_1);
            test216.Add("j");
            try
            {
                int test217 = O.ForListMax(test215);
                O.ForListCheck(test216);
                for (int i = 0; i < test217; i++)
                {
                    O.SetStringFromCache(ref scalar219, "i", x4_0[i], true);

                    O.SetStringFromCache(ref scalar220, "j", x4_1[i], true);

                    C1(p);

                }
            } //end of try
            finally
            {
                O.RemoveScalar("i");
                O.RemoveScalar("j");
            }  //end of finally

            C2(p);

        }
    }
}
