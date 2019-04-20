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
            o0.opt_source = @"<[code]>#m.x = #m.x[-1] + 1";


            Action check_32 = () =>
            {
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), Globals.scalarValMissing, o0, (new ScalarString("x")))
                ;
            };
            Action assign_32 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar29 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i30)
                ), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.Dot, (new ScalarString("x"))), smpl, O.EIndexerType.Dot, O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(), EVariableType.Var, null), (new ScalarString("x"))), O.Negate(smpl, i30)
                ), i31);
                O.AdjustT0(smpl, 2);
                if (O.Dynamic6(smpl)) return;
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar29, o0, (new ScalarString("x")))
                ;
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_32, check_32, o0);

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i30 = new ScalarVal(1d);
        public static readonly ScalarVal i31 = new ScalarVal(1d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
