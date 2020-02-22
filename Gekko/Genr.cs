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
            
            foreach (IVariable listloop_a20 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, o0)))
            {
                ScalarVal v = O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a20), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#a15t100", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a20) as ScalarVal;
                if (v != null && (v as ScalarVal).val == 0d) continue;

                IVariable ivTmpvar21 = O.Subtract(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a20), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "fDeltag", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a20), O.Divide(smpl, O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a20), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "uDeltag", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a20), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a20), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nSoegBase", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a20), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a20), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a20))), O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "%tbase", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a20), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nSoegBase", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a20), O.Lookup(smpl, null, null, "%tbase", null, null, new LookupSettings(), EVariableType.Var, null)
                ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "%tbase", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a20), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "nPop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a20), O.Lookup(smpl, null, null, "%tbase", null, null, new LookupSettings(), EVariableType.Var, null)
                ))));

                //O.DollarLookup(O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a20), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#a15t100", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a20)
                //, smpl, null, null, "deleteme", null, ivTmpvar21, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
                ;
            }

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
