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
            o0.opt_source = @"<[code]>y[01]=x[01]";


            Action assign_15 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar12 = O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, i14
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), i14
                );
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar12, o0, i13
                )
                ;
            };
            Func<bool> check_15 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar12 = O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, i14
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), i14
                );
                O.AdjustT0(smpl, 2);
                if (ivTmpvar12.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "y", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar12, o0, i13
                )
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_15, check_15, o0);

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i13 = new ScalarVal(01d);
        public static readonly ScalarVal i14 = new ScalarVal(01d);
         
        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
