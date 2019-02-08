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
            smpl.t0 = O.ConvertToDate(i21, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t1 = O.ConvertToDate(i21, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t2 = O.ConvertToDate(i22, O.GetDateChoices.FlexibleEnd);
            ;
            smpl.t3 = O.ConvertToDate(i22, O.GetDateChoices.FlexibleEnd);
            ;

            o0.opt_m = "yes";

            o0.opt_keep = O.ConvertToString("p");
            
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar19 = O.Add(smpl, O.Lookup(smpl, null, null, "v", null, null, new LookupSettings(), EVariableType.Var, null), i20);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "x", null, ivTmpvar19, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
            ;

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i20 = new ScalarVal(0d);
        public static readonly ScalarVal i21 = new ScalarVal(2001d);
        public static readonly ScalarVal i22 = new ScalarVal(2003d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
