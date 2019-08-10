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

            var Evalcode863 = new List<Func<GekkoSmpl, IVariable>>(); Evalcode863.Add((smpl2) => {
                return O.Subtract(smpl2, O.Indexer(O.Indexer2(smpl2, O.EIndexerType.None, i858
), smpl2, O.EIndexerType.None, O.Lookup(smpl2, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), i858
), O.Add(smpl2, O.Multiply(smpl2, O.Indexer(O.Indexer2(smpl2, O.EIndexerType.None, i859
), smpl2, O.EIndexerType.None, O.Lookup(smpl2, null, null, "k", null, null, new LookupSettings(), EVariableType.Var, null), i859
), O.Indexer(O.Indexer2(smpl2, O.EIndexerType.IndexerLag, O.Negate(smpl2, i861)
), smpl2, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl2, O.EIndexerType.None, i860
), smpl2, O.EIndexerType.None, O.Lookup(smpl2, null, null, "y", null, null, new LookupSettings(), EVariableType.Var, null), i860
), O.Negate(smpl2, i861)
)), O.Indexer(O.Indexer2(smpl2, O.EIndexerType.None, i862
), smpl2, O.EIndexerType.None, O.Lookup(smpl2, null, null, "z", null, null, new LookupSettings(), EVariableType.Var, null), i862
)));
            });
            Globals.expressionText = @"x[1] - (k[1] * y[1][-2] + z[1])";
            Globals.expressions = Evalcode863;
            Globals.freeIndexedListsDecomp = null;

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i858 = new ScalarVal(1d);
        public static readonly ScalarVal i859 = new ScalarVal(1d);
        public static readonly ScalarVal i860 = new ScalarVal(1d);
        public static readonly ScalarVal i861 = new ScalarVal(2d);
        public static readonly ScalarVal i862 = new ScalarVal(1d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
