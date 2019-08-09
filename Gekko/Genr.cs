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
        public static GekkoTime globalGekkoTimeIterator = GekkoTime.tNull;
        public static int labelCounter;
        public static void C0(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]0
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Decomp2 o0 = new O.Decomp2();
            o0.label = @"x[1] in e_x";
            o0.t1 = Globals.globalPeriodStart;
            o0.t2 = Globals.globalPeriodEnd;

            o0.opt_prtcode = O.ConvertToString((new ScalarString("d")));



            o0.decompItems.Add(new DecompItems(null, O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("x").Concat(null, new ScalarString("[").Add(null, new ScalarString("1")).Add(null, new ScalarString("]"))) })), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("e_x") }))));




            o0.Exe();

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
