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
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o0 = new O.Assignment();
            o0.opt_trace = @"series <2010 %slutfmkort> work:fmqo!a              = fmkort:nL_inklOrlov!a[off]";
            smpl.t0 = O.ConvertToDate(i330, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t1 = O.ConvertToDate(i330, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t2 = O.ConvertToDate(O.Lookup(smpl, null, null, "%slutfmkort", null, null, new LookupSettings(), EVariableType.Var, null), O.GetDateChoices.FlexibleEnd);
            ;
            smpl.t3 = O.ConvertToDate(O.Lookup(smpl, null, null, "%slutfmkort", null, null, new LookupSettings(), EVariableType.Var, null), O.GetDateChoices.FlexibleEnd);
            ;




            Globals.precedentsSeries = null;
            Action assign_331 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar329 = O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("off")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, "fmkort", "nL_inklOrlov", "a", null, new LookupSettings(), EVariableType.Series, null), new ScalarString("off"));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, "work", "fmqo", "a", ivTmpvar329, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Series, o0)
                ;
            };
            Func<bool> check_331 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar329 = O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("off")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, "fmkort", "nL_inklOrlov", "a", null, new LookupSettings(), EVariableType.Series, null), new ScalarString("off"));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar329.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, "work", "fmqo", "a", ivTmpvar329, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Series, o0)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_331, check_331, o0, p);

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i330 = new ScalarVal(2010d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
