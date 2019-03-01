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
            O.AdjustT0(smpl, -2);
            IVariable ivTmpvar1 = i2;
            O.AdjustT0(smpl, 2);
            O.Lookup(smpl, null, null, "x", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
            ;

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Assignment o1 = new O.Assignment();
            O.AdjustT0(smpl, -2);
            IVariable ivTmpvar3 = Functions.series(smpl, i4);
            O.AdjustT0(smpl, 2);
            O.Lookup(smpl, null, null, "x", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
            ;

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Assignment o2 = new O.Assignment();
            O.AdjustT0(smpl, -2);
            IVariable ivTmpvar5 = i6;
            O.AdjustT0(smpl, 2);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar5, o2, new ScalarString("a"), new ScalarString("b"))
            ;

            //[[commandEnd]]2
        }


        public static readonly ScalarVal i2 = new ScalarVal(100d);
        public static readonly ScalarVal i4 = new ScalarVal(2d);
        public static readonly ScalarVal i6 = new ScalarVal(100d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
