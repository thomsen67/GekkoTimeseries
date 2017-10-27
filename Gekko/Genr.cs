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
        public static readonly ScalarVal i88 = new ScalarVal(2001d);
        public static readonly ScalarVal i89 = new ScalarVal(2003d);
        public static readonly ScalarVal i92 = new ScalarVal(1d);
        public static readonly ScalarVal i93 = new ScalarVal(2d);
        public static readonly ScalarVal i94 = new ScalarVal(3d);
        public static readonly ScalarVal i97 = new ScalarVal(1d);
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);


            p.SetText(@"¤0"); O.InitSmpl(smpl);
            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe(smpl);




            p.SetText(@"¤1"); O.InitSmpl(smpl);
            O.Time o1 = new O.Time();
            o1.t1 = O.ConvertToDate(i88, O.GetDateChoices.FlexibleStart);
            ;
            o1.t2 = O.ConvertToDate(i89, O.GetDateChoices.FlexibleEnd);
            ;

            o1.Exe();




            p.SetText(@"¤0"); O.InitSmpl(smpl);
            IVariable ivTmpvar90 = O.ListDefHelper(i92, i93, i94);
            for (int iSmpl91 = 0; iSmpl91 < int.MaxValue; iSmpl91++)
            {
                O.Lookup(smpl, null, null, "xx", null, ivTmpvar90, true)
                ;

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl91); else break;
            };




            p.SetText(@"¤0"); O.InitSmpl(smpl);
            IVariable ivTmpvar95 = O.Indexer(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false), O.Lookup(smpl, null, null, "xx", null, null, false)), O.Negate(smpl, i97)
            );
            for (int iSmpl96 = 0; iSmpl96 < int.MaxValue; iSmpl96++)
            {
                O.Lookup(smpl, null, null, "xx2", null, ivTmpvar95, true)
                ;

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl96); else break;
            };




        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

            C0(p);



        }
    }
}
