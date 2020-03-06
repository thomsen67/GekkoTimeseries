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
            p.SetText(@"¤5"); O.InitSmpl(smpl, p);

            O.Assignment o0 = new O.Assignment();
            o0.opt_source = @"<[code]>VAR2 deleteme  = qBNP * pBNP[-1] - (( pC[#cTot][-1] * qC[#cTot]
                              + pG[#gTot][-1] * qG[#gTot]
                              + pI[#iTot][-1] * qI[#iTot]
                              + pX[#xTot][-1] * qX[#xTot]
                              - pM[#sTot][-1] * qM[#sTot] ))";


            var Evalcode930 = new List<Func<GekkoSmpl, IVariable>>(); 
            
            foreach (IVariable listloop_cTot917 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("cTot")))), null, new LookupSettings(), EVariableType.Var, o0)))
            {
                foreach (IVariable listloop_gTot918 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("gTot")))), null, new LookupSettings(), EVariableType.Var, o0)))
                {
                    foreach (IVariable listloop_iTot919 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("iTot")))), null, new LookupSettings(), EVariableType.Var, o0)))
                    {
                        foreach (IVariable listloop_xTot920 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("xTot")))), null, new LookupSettings(), EVariableType.Var, o0)))
                        {
                            foreach (IVariable listloop_sTot921 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("sTot")))), null, new LookupSettings(), EVariableType.Var, o0)))
                            {
                                Evalcode930.Add((smpl931) => {
                                    return O.Subtract(smpl931, O.Multiply(smpl931, O.Lookup(smpl931, null, null, "qBNP", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl931, O.EIndexerType.IndexerLag, O.Negate(smpl931, i923)
                                    ), smpl931, O.EIndexerType.IndexerLag, O.Lookup(smpl931, null, null, "pBNP", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl931, i923)
                                    )), O.Subtract(smpl931, O.Add(smpl931, O.Add(smpl931, O.Add(smpl931, O.Multiply(smpl931, O.Indexer(O.Indexer2(smpl931, O.EIndexerType.IndexerLag, O.Negate(smpl931, i924)
                                    ), smpl931, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl931, O.EIndexerType.None, listloop_cTot917), smpl931, O.EIndexerType.None, O.Lookup(smpl931, null, null, "pC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_cTot917), O.Negate(smpl931, i924)
                                    ), O.Indexer(O.Indexer2(smpl931, O.EIndexerType.None, listloop_cTot917), smpl931, O.EIndexerType.None, O.Lookup(smpl931, null, null, "qC", null, null, new LookupSettings(), EVariableType.Var, null), listloop_cTot917)), O.Multiply(smpl931, O.Indexer(O.Indexer2(smpl931, O.EIndexerType.IndexerLag, O.Negate(smpl931, i925)
                                    ), smpl931, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl931, O.EIndexerType.None, listloop_gTot918), smpl931, O.EIndexerType.None, O.Lookup(smpl931, null, null, "pG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_gTot918), O.Negate(smpl931, i925)
                                    ), O.Indexer(O.Indexer2(smpl931, O.EIndexerType.None, listloop_gTot918), smpl931, O.EIndexerType.None, O.Lookup(smpl931, null, null, "qG", null, null, new LookupSettings(), EVariableType.Var, null), listloop_gTot918))), O.Multiply(smpl931, O.Indexer(O.Indexer2(smpl931, O.EIndexerType.IndexerLag, O.Negate(smpl931, i926)
                                    ), smpl931, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl931, O.EIndexerType.None, listloop_iTot919), smpl931, O.EIndexerType.None, O.Lookup(smpl931, null, null, "pI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_iTot919), O.Negate(smpl931, i926)
                                    ), O.Indexer(O.Indexer2(smpl931, O.EIndexerType.None, listloop_iTot919), smpl931, O.EIndexerType.None, O.Lookup(smpl931, null, null, "qI", null, null, new LookupSettings(), EVariableType.Var, null), listloop_iTot919))), O.Multiply(smpl931, O.Indexer(O.Indexer2(smpl931, O.EIndexerType.IndexerLag, O.Negate(smpl931, i927)
                                    ), smpl931, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl931, O.EIndexerType.None, listloop_xTot920), smpl931, O.EIndexerType.None, O.Lookup(smpl931, null, null, "pX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_xTot920), O.Negate(smpl931, i927)
                                    ), O.Indexer(O.Indexer2(smpl931, O.EIndexerType.None, listloop_xTot920), smpl931, O.EIndexerType.None, O.Lookup(smpl931, null, null, "qX", null, null, new LookupSettings(), EVariableType.Var, null), listloop_xTot920))), O.Multiply(smpl931, O.Indexer(O.Indexer2(smpl931, O.EIndexerType.IndexerLag, O.Negate(smpl931, i928)
                                    ), smpl931, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl931, O.EIndexerType.None, listloop_sTot921), smpl931, O.EIndexerType.None, O.Lookup(smpl931, null, null, "pM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sTot921), O.Negate(smpl931, i928)
                                    ), O.Indexer(O.Indexer2(smpl931, O.EIndexerType.None, listloop_sTot921), smpl931, O.EIndexerType.None, O.Lookup(smpl931, null, null, "qM", null, null, new LookupSettings(), EVariableType.Var, null), listloop_sTot921))));
                                });
                            }
                        }
                    }
                }
            }
            Globals.expressions = Evalcode930;

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i923 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i924 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i925 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i926 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i927 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i928 = new ScalarVal(1d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
