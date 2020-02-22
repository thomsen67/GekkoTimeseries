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

            O.Assignment o0 = new O.Assignment();
            o0.opt_source = @"<[code]>VAR2 deleteme $ (#a15t100[#a]) = fDeltag[#a] - (uDeltag[#a] * (nSoegBase[#a] / nPop[#a]) / (nSoegBase[#a][%tbase] / nPop[#a][%tbase]))";
            
            var Evalcode12 = new List<Func<GekkoSmpl, IVariable>>(); 
            foreach (IVariable listloop_a9 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o0)))
            {
                ScalarVal v13 = O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a9), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#a15t100", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a9)
                 as ScalarVal;
                if (v13 != null && (v13 as ScalarVal).val == 0d) continue;
                Evalcode12.Add((smpl14) => {
                    return O.Subtract(smpl14, O.Indexer(O.Indexer2(smpl14, O.EIndexerType.None, listloop_a9), smpl14, O.EIndexerType.None, O.Lookup(smpl14, null, null, "fDeltag", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a9), O.Divide(smpl14, O.Multiply(smpl14, O.Indexer(O.Indexer2(smpl14, O.EIndexerType.None, listloop_a9), smpl14, O.EIndexerType.None, O.Lookup(smpl14, null, null, "uDeltag", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a9), O.Divide(smpl14, O.Indexer(O.Indexer2(smpl14, O.EIndexerType.None, listloop_a9), smpl14, O.EIndexerType.None, O.Lookup(smpl14, null, null, "nSoegBase", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a9), O.Indexer(O.Indexer2(smpl14, O.EIndexerType.None, listloop_a9), smpl14, O.EIndexerType.None, O.Lookup(smpl14, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a9))), O.Divide(smpl14, O.Indexer(O.Indexer2(smpl14, O.EIndexerType.None, O.Lookup(smpl14, null, null, "%tbase", null, null, new LookupSettings(), EVariableType.Var, null)
                    ), smpl14, O.EIndexerType.None, O.Indexer(O.Indexer2(smpl14, O.EIndexerType.None, listloop_a9), smpl14, O.EIndexerType.None, O.Lookup(smpl14, null, null, "nSoegBase", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a9), O.Lookup(smpl14, null, null, "%tbase", null, null, new LookupSettings(), EVariableType.Var, null)
                    ), O.Indexer(O.Indexer2(smpl14, O.EIndexerType.None, O.Lookup(smpl14, null, null, "%tbase", null, null, new LookupSettings(), EVariableType.Var, null)
                    ), smpl14, O.EIndexerType.None, O.Indexer(O.Indexer2(smpl14, O.EIndexerType.None, listloop_a9), smpl14, O.EIndexerType.None, O.Lookup(smpl14, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a9), O.Lookup(smpl14, null, null, "%tbase", null, null, new LookupSettings(), EVariableType.Var, null)
                    ))));
                });
            }
            Globals.expressions = Evalcode12;

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
