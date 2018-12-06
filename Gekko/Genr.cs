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
            foreach (IVariable listloop_i24 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, o0)))
            {
                foreach (IVariable listloop_j25 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("j")))), null, new LookupSettings(), EVariableType.Var, o0)))
                {
                    O.AdjustT0(smpl, -1);
                    IVariable ivTmpvar26 = O.Add(smpl, O.Add(smpl, i27, O.Dollar(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i24, listloop_j25), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "y", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i24, listloop_j25), O.In(smpl, listloop_i24, O.Lookup(smpl, null, null, "#i0", null, null, new LookupSettings(), EVariableType.Var, null))
                    )), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_j25), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "z", null, null, new LookupSettings(), EVariableType.Var, null), listloop_j25));
                    O.AdjustT0(smpl, 1);
                    O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar26, o0, listloop_i24, listloop_j25)
                    ;
                }
            }

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i27 = new ScalarVal(1d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
