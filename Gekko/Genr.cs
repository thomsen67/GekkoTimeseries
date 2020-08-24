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
            o0.opt_source = @"<[code]>x = x[-1]+1";


            Globals.precedentsSeries = null;
            Action assign_23 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar20 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i21)
                ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i21)
                ), i22);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar20, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
            };
            Func<bool> check_23 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar20 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i21)
                ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i21)
                ), i22);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar20.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar20, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_23, check_23, o0);

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i21 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i22 = new ScalarVal(1d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
