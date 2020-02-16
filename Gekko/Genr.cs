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
            o0.type = @"ASTDECOMP3";
            o0.label = @"y[18], y[19]";
            o0.t1 = Globals.globalPeriodStart;
            o0.t2 = Globals.globalPeriodEnd;

            o0.opt_prtcode = O.ConvertToString((new ScalarString("d")));



            o0.decompItems.Add(new DecompItems(null, O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("y").Concat(null, new ScalarString("[").Concat(null, new ScalarString("18")).Concat(null, new ScalarString("]"))), new ScalarString("y").Concat(null, new ScalarString("[").Concat(null, new ScalarString("19")).Concat(null, new ScalarString("]"))) })), null, null));

            o0.decompItems.Add(new DecompItems(null, null, O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("e1a") })), null));

            o0.endo.Add(O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("demand").Concat(null, new ScalarString("[").Concat(null, new ScalarString("18")).Concat(null, new ScalarString("]"))), new ScalarString("demand").Concat(null, new ScalarString("[").Concat(null, new ScalarString("19")).Concat(null, new ScalarString("]"))), new ScalarString("supply").Concat(null, new ScalarString("[").Concat(null, new ScalarString("18")).Concat(null, new ScalarString("]"))), new ScalarString("supply").Concat(null, new ScalarString("[").Concat(null, new ScalarString("19")).Concat(null, new ScalarString("]"))), new ScalarString("c").Concat(null, new ScalarString("[").Concat(null, new ScalarString("18")).Concat(null, new ScalarString("]"))), new ScalarString("c").Concat(null, new ScalarString("[").Concat(null, new ScalarString("19")).Concat(null, new ScalarString("]"))) })));

            o0.where.Add(new List<IVariable>() { O.HandleString(new ScalarString(@"0")), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("equ") })) });


            o0.rows.Add(O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("vars"), O.scalarStringHash.Concat(null, new ScalarString("a")), new ScalarString("lags") })));

            o0.cols.Add(O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("time") })));

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
