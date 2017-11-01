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
        public static readonly ScalarVal i89 = new ScalarVal(2005d);
        public static readonly ScalarVal i92 = new ScalarVal(1d);
        public static readonly ScalarVal i93 = new ScalarVal(2d);
        public static readonly ScalarVal i94 = new ScalarVal(3d);
        public static readonly ScalarVal i95 = new ScalarVal(4d);
        public static readonly ScalarVal i96 = new ScalarVal(5d);
        public static readonly ScalarVal i99 = new ScalarVal(1d);
        public static readonly ScalarVal i102 = new ScalarVal(1d);
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
            IVariable ivTmpvar90 = O.ListDefHelper(i92, i93, i94, i95, i96);
            for (int iSmpl91 = 0; iSmpl91 < int.MaxValue; iSmpl91++)
            {
                O.Lookup(smpl, null, null, "xx1", null, ivTmpvar90, true)
                    
                ;
                break;

              
            };

            

            p.SetText(@"¤0"); O.InitSmpl(smpl);

            Desti:

            IVariable xxx = O.Add(smpl, O.Lookup(smpl, null, null, "xx1", null, null, false), O.Lookup(smpl, null, null, "xx1", null, null, false));
            IVariable ivTmpvar100 = O.Indexer(smpl, xxx, O.Negate(smpl, i102));
            O.Lookup(smpl, null, null, "xx3", null, ivTmpvar100, true);

            if (smpl.HasError())
            {
                O.TryNewSmpl(smpl);
                goto Desti;
            }
        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

            C0(p);



        }
    }
}
