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
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);


            O.Decomp o0 = new O.Decomp();
            o0.label = @"(fKbhe[-1]/fKbh[-1])*(Vh+(pch-ahch*pxh)*fCh)
                                                      + phk * fKnbhe[-1]
                                                          * ((1 - tsuih) * iwbz + bfinvbh - 0.50 * rpibhe)
                                                      + tsuih * Yrphs + Siqejh * fKnbhe[-2] / fKnbh[-2] + Ssyej";
        
o0.smplForFunc = smpl;
            o0.expression = () => O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Multiply(smpl, O.Divide(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i1)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "fKbhe", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i1)
            ), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i2)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "fKbh", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i2)
            )), O.Add(smpl, O.Lookup(smpl, null, null, "Vh", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Subtract(smpl, O.Lookup(smpl, null, null, "pch", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl, O.Lookup(smpl, null, null, "ahch", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "pxh", null, null, new LookupSettings(), EVariableType.Var, null))), O.Lookup(smpl, null, null, "fCh", null, null, new LookupSettings(), EVariableType.Var, null)))), O.Multiply(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "phk", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i3)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "fKnbhe", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i3)
            )), O.Subtract(smpl, O.Add(smpl, O.Multiply(smpl, O.Subtract(smpl, i4, O.Lookup(smpl, null, null, "tsuih", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "iwbz", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, null, "bfinvbh", null, null, new LookupSettings(), EVariableType.Var, null)), O.Multiply(smpl, d5, O.Lookup(smpl, null, null, "rpibhe", null, null, new LookupSettings(), EVariableType.Var, null))))), O.Multiply(smpl, O.Lookup(smpl, null, null, "tsuih", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "Yrphs", null, null, new LookupSettings(), EVariableType.Var, null))), O.Divide(smpl, O.Multiply(smpl, O.Lookup(smpl, null, null, "Siqejh", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i6)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "fKnbhe", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i6)
            )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i7)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "fKnbh", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i7)
            ))), O.Lookup(smpl, null, null, "Ssyej", null, null, new LookupSettings(), EVariableType.Var, null));

            o0.Exe();

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i1 = new ScalarVal(1d);
        public static readonly ScalarVal i2 = new ScalarVal(1d);
        public static readonly ScalarVal i3 = new ScalarVal(1d);
        public static readonly ScalarVal i4 = new ScalarVal(1d);
        public static readonly ScalarVal d5 = new ScalarVal(0.50d);
        public static readonly ScalarVal i6 = new ScalarVal(2d);
        public static readonly ScalarVal i7 = new ScalarVal(2d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
