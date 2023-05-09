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
            o0.opt_source = @"<[code]>xx[a] $ (1==1) = 2";


            Globals.precedentsSeries = null;
            Action assign_49 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar45 = i48;
                O.AdjustT0(smpl, 2);
                O.DollarIndexerSetData(O.Equals(smpl, i46, i47)
                , smpl, O.Lookup(smpl, null, null, "xx", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar45, o0, new ScalarString("a"))
                ;
            };
            Func<bool> check_49 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar45 = i48;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar45.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.DollarIndexerSetData(O.Equals(smpl, i46, i47)
                , smpl, O.Lookup(smpl, null, null, "xx", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar45, o0, new ScalarString("a"))
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_49, check_49, o0);

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i46 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i47 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i48 = new ScalarVal(2d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
