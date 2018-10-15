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
        public static readonly ScalarVal i50 = new ScalarVal(1d);
        public static void ClearTS(P p) {
        }
        public static void ClearScalar(P p) {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            O.Assignment o0 = new O.Assignment();
            foreach (IVariable listloop_i47 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(O.ELookupType.RightHandSide, O.ECreatePossibilities.NoneReportError), EVariableType.Var, o0))) {
                foreach (IVariable listloop_j48 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("j")))), null, new LookupSettings(O.ELookupType.RightHandSide, O.ECreatePossibilities.NoneReportError), EVariableType.Var, o0))) {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar49 = O.Add(smpl, O.Add(smpl, i50, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i47, listloop_j48), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i47, listloop_j48)), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_j48), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "z", null, null, new LookupSettings(), EVariableType.Var, null), listloop_j48));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar49, o0, listloop_i47, listloop_j48)
                    ;
                }
            }


            //[[splitSTOP]]


        }
    }
}
