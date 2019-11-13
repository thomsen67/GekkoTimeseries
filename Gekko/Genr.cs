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
            o0.opt_source = @"<[code]>x[2019m12d24]=2";


            Action assign_10 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar8 = i9;
                O.AdjustT0(smpl, 2);
                IVariable xxx = O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null);
                GekkoTime xx = G.FromStringToDate("2019m12d24");
                O.IndexerSetData(smpl, xxx, ivTmpvar8, o0, new ScalarDate(xx))                ;
            };
            Func<bool> check_10 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar8 = i9;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar8.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                IVariable xxx = O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null);
                GekkoTime xx = G.FromStringToDate("2019m12d24");

                O.IndexerSetData(smpl, xxx, ivTmpvar8, o0, new ScalarDate(xx));
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_10, check_10, o0);

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i9 = new ScalarVal(2d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
