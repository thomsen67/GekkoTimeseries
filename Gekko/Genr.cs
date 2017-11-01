

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
         IVariable list10 = null;
        public static IVariable list11 = null;
        public static readonly ScalarVal
         i12 = new ScalarVal(2019d);
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P
         p)
        {
        }
        public static void C0(P p)
        {

            GekkoTime t =
             Globals.tNull;

            p.SetText(@"¤1");


            p.SetText(@"¤16");



            p.SetText(@"¤24");
            O.Reset o10 =
             new O.Reset();
            o10.p =
             p; o10.Exe();




            p.SetText(@"¤24");
            Program.Cls(""); Program.Cls("output");



            p.SetText(@"¤25");
            O.Create o12 = new O.Create();
            o12.listItems = new List<string>();
            o12.listItems.AddRange(O.GetList((new ScalarString("a"))));

            o12.listItems.AddRange(O.GetList((new ScalarString("b"))));

            o12.listItems.AddRange(O.GetList((new ScalarString("c"))));

            o12.p = p;
            o12.Exe();




            p.SetText(@"¤26");
            O.Create o13 = new O.Create();
            o13.listItems = new List<string>();
            o13.listItems.AddRange(O.GetList((new ScalarString("x"))));

            o13.listItems.AddRange(O.GetList((new ScalarString("y"))));

            o13.listItems.AddRange(O.GetList((new ScalarString("z"))));

            o13.p = p;
            o13.Exe();




            p.SetText(@"¤27");
            O.List o14 = new O.List();
            o14.name = O.GetString((new ScalarString("vars")));
            o14.listItems = new List<string>();
            o14.p = p;
            o14.listItems = new List<string>();
            o14.listItems.AddRange(O.GetList((new ScalarString("a"))));

            o14.listItems.AddRange(O.GetList((new ScalarString("b"))));

            o14.listItems.AddRange(O.GetList((new ScalarString("c"))));

            o14.Exe();




            p.SetText(@"¤28");
            O.List o15 = new O.List();
            o15.name = O.GetString((new ScalarString("endogenize")));
            o15.listItems = new List<string>();
            o15.p = p;
            o15.listItems = new List<string>();
            o15.listItems.AddRange(O.GetList((new ScalarString("x"))));

            o15.listItems.AddRange(O.GetList((new ScalarString("y"))));

            o15.listItems.AddRange(O.GetList((new ScalarString("z"))));

            o15.Exe();




            p.SetText(@"¤30");
            //O.SetStringData((new ScalarString("s")), UProc.simx(p, t, O.GetScalarFromCache(ref list10, "#vars", false, false), O.GetScalarFromCache(ref list11, "#endogenize", false, false), i12), false);




        }


        public static void CodeLines(P p)
        {
            GekkoTime t = Globals.tNull;

            C0(p);



        }
    }
}

