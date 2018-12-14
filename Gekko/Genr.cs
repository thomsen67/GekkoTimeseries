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
            smpl.t0 = O.ConvertToDate(i7, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t1 = O.ConvertToDate(i7, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t2 = O.ConvertToDate(i8, O.GetDateChoices.FlexibleEnd);
            ;
            smpl.t3 = O.ConvertToDate(i8, O.GetDateChoices.FlexibleEnd);
            ;


            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar1 = O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "y1", null, null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i2)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "y1", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i2)
            )), O.Lookup(smpl, null, null, "y2", null, null, new LookupSettings(), EVariableType.Var, null)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i3)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "y2", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i3)
            )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, i4
            ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "y3", null, null, new LookupSettings(), EVariableType.Var, null), i4
            )), O.Lookup(smpl, null, null, "%x", null, null, new LookupSettings(), EVariableType.Var, null)), i5), i6);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "x1", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
            ;

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i2 = new ScalarVal(1d);
        public static readonly ScalarVal i3 = new ScalarVal(1d);
        public static readonly ScalarVal i4 = new ScalarVal(2000d);
        public static readonly ScalarVal i5 = new ScalarVal(1d);
        public static readonly ScalarVal i6 = new ScalarVal(2d);
        public static readonly ScalarVal i7 = new ScalarVal(2000d);
        public static readonly ScalarVal i8 = new ScalarVal(2020d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
