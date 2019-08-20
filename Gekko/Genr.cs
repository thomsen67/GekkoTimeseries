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
        public static void C0(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_23)
        {
            IVariable forloop_xe7dke6cj_23 = xforloop_xe7dke6cj_23;

            //[[commandStart]]1
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            O.Assignment o1 = new O.Assignment();
            o1.opt_source = @"<[code]>%j=%i";


            Action assign_27 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar26 = forloop_xe7dke6cj_23;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%j", null, ivTmpvar26, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
            };
            Func<bool> check_27 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar26 = forloop_xe7dke6cj_23;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar26.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%j", null, ivTmpvar26, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_27, check_27, o1);

            //[[commandEnd]]1
            xforloop_xe7dke6cj_23 = forloop_xe7dke6cj_23;

        }


        public static readonly ScalarVal i24 = new ScalarVal(1d);
        public static readonly ScalarVal i25 = new ScalarVal(2d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[commandSpecial]]0
            IVariable forloop_xe7dke6cj_23 = null;
            int counter28 = 0;
            bool years29 = O.LoopYears("val", O.ELoopType.ForTo, i24, i25); for (O.IterateStart(years29, O.ELoopType.ForTo, ref forloop_xe7dke6cj_23, i24); O.IterateContinue(years29, O.ELoopType.ForTo, forloop_xe7dke6cj_23, i24, i25, null, ref counter28); O.IterateStep(years29, O.ELoopType.ForTo, ref forloop_xe7dke6cj_23, i24, null, counter28))
            {
                O.TypeCheck_val(forloop_xe7dke6cj_23, 0);
                ;

                C0(smpl, p, ref forloop_xe7dke6cj_23);

            };

            //[[commandEnd]]0



        }
    }
}
