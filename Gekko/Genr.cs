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


            O.Ols o0 = new O.Ols();
            smpl.t0 = O.ConvertToDate(i33, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t1 = O.ConvertToDate(i33, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t2 = O.ConvertToDate(i34, O.GetDateChoices.FlexibleEnd);
            ;
            smpl.t3 = O.ConvertToDate(i34, O.GetDateChoices.FlexibleEnd);
            ;
            o0.t1 = O.ConvertToDate(i33, O.GetDateChoices.FlexibleStart);
            ;
            o0.t2 = O.ConvertToDate(i34, O.GetDateChoices.FlexibleEnd);
            ;

            o0.expressions = new List<IVariable>();
            o0.expressions.Add(Functions.dlog(O.Smpl(smpl, -1), smpl, O.Lookup(smpl, null, null, "lna1", null, null, new LookupSettings(), EVariableType.Var, null)));
            o0.expressions.Add(Functions.dlog(O.Smpl(smpl, -1), smpl, O.Lookup(smpl, null, null, "pcp", null, null, new LookupSettings(), EVariableType.Var, null)));
            o0.expressions.Add(Functions.dlog(O.Smpl(smpl, -1), smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.Dot, i35), smpl, O.EIndexerType.Dot, O.Lookup(smpl, null, null, "pcp", null, null, new LookupSettings(), EVariableType.Var, null), i35)));
            o0.expressions.Add(O.Lookup(smpl, null, null, "bul1", null, null, new LookupSettings(), EVariableType.Var, null));
            o0.expressions.Add(O.Indexer(O.Indexer2(smpl, O.EIndexerType.Dot, i36), smpl, O.EIndexerType.Dot, O.Lookup(smpl, null, null, "bul1", null, null, new LookupSettings(), EVariableType.Var, null), i36));
            o0.expressionsText = new List<string>();
            o0.expressionsText.Add(@"dlog¨(lna1)");
            o0.expressionsText.Add(@"dlog¨(pcp)");
            o0.expressionsText.Add(@"dlog¨(pcp£.1)");
            o0.expressionsText.Add(@"bul1");
            o0.expressionsText.Add(@"bul1£.1");
            o0.Exe();

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i33 = new ScalarVal(2000d);
        public static readonly ScalarVal i34 = new ScalarVal(2010d);
        public static readonly ScalarVal i35 = new ScalarVal(1d);
        public static readonly ScalarVal i36 = new ScalarVal(1d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
