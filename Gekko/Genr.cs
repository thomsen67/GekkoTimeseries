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
            p.SetText(@"¤3"); O.InitSmpl(smpl, p);

            Func<GekkoSmpl, IVariable> Evalcode32 = (smpl5) =>
            {
                return O.Add(smpl5, O.Multiply(smpl5, O.Subtract(smpl5, i24, O.Lookup(smpl5, null, null, "Dphk", null, null, new LookupSettings(), EVariableType.Var, null)), O.Multiply(smpl5, O.Multiply(smpl5, O.Indexer(O.Indexer2(smpl5, O.EIndexerType.IndexerLag, O.Negate(smpl5, i25)
), smpl5, O.EIndexerType.IndexerLag, O.Lookup(smpl5, null, null, "phk", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl5, i25)
), Functions.exp(smpl5, O.Subtract(smpl5, O.Subtract(smpl5, O.Multiply(smpl5, d26, Functions.dlog(O.Smpl(smpl5, -1), smpl5, O.Divide(smpl5, O.Lookup(smpl5, null, null, "Cp4xh1", null, null, new LookupSettings(), EVariableType.Var, null), O.Multiply(smpl5, O.Lookup(smpl5, null, null, "U", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl5, null, null, "pcp4xhv1", null, null, new LookupSettings(), EVariableType.Var, null))))), O.Multiply(smpl5, d27, Functions.dlog(O.Smpl(smpl5, -1), smpl5, O.Divide(smpl5, O.Divide(smpl5, O.Lookup(smpl5, null, null, "pche", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl5, null, null, "phk", null, null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl5, null, null, "pcp4xhv1", null, null, new LookupSettings(), EVariableType.Var, null))))), O.Multiply(smpl5, d28, Functions.log(smpl5, O.Divide(smpl5, O.Indexer(O.Indexer2(smpl5, O.EIndexerType.IndexerLag, O.Negate(smpl5, i29)
), smpl5, O.EIndexerType.IndexerLag, O.Lookup(smpl5, null, null, "fKbh", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl5, i29)
), O.Indexer(O.Indexer2(smpl5, O.EIndexerType.IndexerLag, O.Negate(smpl5, i30)
), smpl5, O.EIndexerType.IndexerLag, O.Lookup(smpl5, null, null, "fKbhw", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl5, i30)
))))))), O.Add(smpl5, i31, O.Lookup(smpl5, null, null, "JRphk", null, null, new LookupSettings(), EVariableType.Var, null)))), O.Multiply(smpl5, O.Lookup(smpl5, null, null, "Dphk", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl5, null, null, "Zphk", null, null, new LookupSettings(), EVariableType.Var, null)));
            };

            Globals.expressionText = @"(1 - Dphk) * ((phk[-1] * exp(1.15436*Dlog(Cp4xh1/(U*pcp4xhv1))
                                           -0.407913*Dlog((pche/phk)/pcp4xhv1)
                                           -0.576546*Log(fKbh[-1]/fKbhw[-1])    )) * (1 + JRphk)) + Dphk * Zphk";
            Globals.expression = Evalcode32;

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i24 = new ScalarVal(1d);
        public static readonly ScalarVal i25 = new ScalarVal(1d);
        public static readonly ScalarVal d26 = new ScalarVal(1.15436d);
        public static readonly ScalarVal d27 = new ScalarVal(0.407913d);
        public static readonly ScalarVal d28 = new ScalarVal(0.576546d);
        public static readonly ScalarVal i29 = new ScalarVal(1d);
        public static readonly ScalarVal i30 = new ScalarVal(1d);
        public static readonly ScalarVal i31 = new ScalarVal(1d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
