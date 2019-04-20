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
            o0.opt_source = @"<[code]>y=y.1+1";


            Func<bool> check_28 = () =>
            {
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y", null, Globals.scalarValMissing, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
                O.Dynamic5(smpl);
                return O.Dynamic7(smpl);
            };
            Action assign_28 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar25 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.Dot, i26), smpl, O.EIndexerType.Dot, O.Lookup(smpl, null, null, "y", null, null, new LookupSettings(), EVariableType.Var, null), i26), i27);
                O.AdjustT0(smpl, 2);
                if (O.Dynamic6(smpl)) return;
                O.Lookup(smpl, null, null, "y", null, ivTmpvar25, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_28, check_28, o0);

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i26 = new ScalarVal(1d);
        public static readonly ScalarVal i27 = new ScalarVal(1d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
