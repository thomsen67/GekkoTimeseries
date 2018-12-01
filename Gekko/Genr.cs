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
        public static void C0(GekkoSmpl smpl, P p) {
            //[[commandStart]]0
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            O.Decomp o0 = new O.Decomp();
            o0.t1 = Globals.globalPeriodStart;
            o0.t2 = Globals.globalPeriodEnd;

            smpl = new GekkoSmpl(o0.t1.Add(O.MaxLag()), o0.t2.Add(O.MaxLead()));
            o0.expression = () => O.Add(smpl, O.Multiply(smpl, i32, O.Lookup(smpl, null, null, "x1", null, null, new LookupSettings(), EVariableType.Var, null)), O.Multiply(smpl, i33, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i34)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "x1", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i34)
            )));

            o0.Exe();

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i32 = new ScalarVal(2d);
        public static readonly ScalarVal i33 = new ScalarVal(3d);
        public static readonly ScalarVal i34 = new ScalarVal(1d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
