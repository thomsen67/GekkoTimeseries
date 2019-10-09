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
        public static void C0(P p)
        {

            GekkoTime t = Globals.tNull;


            p.SetText(@"¤1");
            O.Model o0 = new O.Model();
            o0.p = p; o0.helper = new List<IVariable>();
            o0.helper.Add((new ScalarString("m1")));
            o0.helper.Add((new ScalarString("removeblock")));
            o0.helper.Add((new ScalarString("m2")).Add(new ScalarString("."), t).Add((new ScalarString("frm")), t));
            o0.helper.Add((new ScalarString("setblock")));
            o0.listItems = new List<string>();
            o0.listItems.AddRange(O.GetList((new ScalarString("a"))));

            o0.listItems.AddRange(O.GetList((new ScalarString("b"))));

            o0.listItems0 = new List<string>();
            o0.listItems0.AddRange(O.GetList((new ScalarString("x"))));

            o0.listItems0.AddRange(O.GetList((new ScalarString("y"))));

            o0.opt_info = "yes";
            o0.Exe();




        }


        public static void CodeLines(P p)
        {
            GekkoTime t = Globals.tNull;

            C0(p);



        }
    }
}
