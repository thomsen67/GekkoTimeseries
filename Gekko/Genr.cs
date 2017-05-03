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
         IVariable list6 = null;
        public static IVariable scalar7 = null;
        public static readonly ScalarVal
         i8 = new ScalarVal(2d);
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
            O.List o0
             = new O.List();
            o0.name = O.GetString((new ScalarString("b1")));
            o0.listItems = new
             List<string>();
            o0.p = p;
            o0.listItems = new
             List<string>();
            o0.listItems.AddRange(O.GetList(O.Indexer(t, i8, false, new IVariablesFilterRange(i8,
             i8))));

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
