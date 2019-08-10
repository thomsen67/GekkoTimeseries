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

            var Evalcode573 = new List<Func<GekkoSmpl, IVariable>>();
            //IVariable xx = O.Lookup(smpl574, null, ((O.scalarStringHash).Add(smpl574, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o0);
            IVariable xx = O.GetIVariableFromString("#a", O.ECreatePossibilities.NoneReportError, true);
            foreach (IVariable listloop_a571 in new O.GekkoListIterator(xx))
            {
                Evalcode573.Add((smpl574) =>
                {
                    return O.Subtract(smpl574, O.Indexer(O.Indexer2(smpl574, O.EIndexerType.None, listloop_a571), smpl574, O.EIndexerType.None, O.Lookup(smpl574, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a571), O.Add(smpl574, O.Multiply(smpl574, O.Indexer(O.Indexer2(smpl574, O.EIndexerType.None, listloop_a571), smpl574, O.EIndexerType.None, O.Lookup(smpl574, null, null, "k", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a571), O.Indexer(O.Indexer2(smpl574, O.EIndexerType.IndexerLag, O.Negate(smpl574, i572)
                    ), smpl574, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl574, O.EIndexerType.None, listloop_a571), smpl574, O.EIndexerType.None, O.Lookup(smpl574, null, null, "y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a571), O.Negate(smpl574, i572)
                    )), O.Indexer(O.Indexer2(smpl574, O.EIndexerType.None, listloop_a571), smpl574, O.EIndexerType.None, O.Lookup(smpl574, null, null, "z", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a571)));
                });
            }
            Globals.expressionText = @"x[#a] - (k[#a] * y[#a][-2] + z[#a])";
            Globals.expressions = Evalcode573;
            //Globals.freeIndexedListsDecomp = null;

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i572 = new ScalarVal(2d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
