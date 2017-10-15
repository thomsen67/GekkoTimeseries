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
        public static GekkoTime globalGekkoTimeIterator = Globals.tNull;
        public static readonly ScalarVal i2 = new ScalarVal(101d);
        public static readonly ScalarVal i3 = new ScalarVal(102d);
        public static readonly ScalarVal i4 = new ScalarVal(103d);
        public static readonly ScalarVal i5 = new ScalarVal(104d);
        public static readonly ScalarVal i6 = new ScalarVal(105d);
        public static readonly ScalarVal i7 = new ScalarVal(106d);
        public static readonly ScalarVal i8 = new ScalarVal(107d);
        public static readonly ScalarVal i9 = new ScalarVal(108d);
        public static readonly ScalarVal i10 = new ScalarVal(109d);
        public static readonly ScalarVal i11 = new ScalarVal(110d);
        public static readonly ScalarVal i12 = new ScalarVal(111d);
        public static readonly ScalarVal i13 = new ScalarVal(1d);
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoSmpl smpl = O.Smpl();


            p.SetText(@"¤0");
            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe();




            p.SetText(@"¤0");
            for (int iSmpl = 0; iSmpl < int.MaxValue; iSmpl++)
            {
                IVariable ivTmpvar1 = O.ListDef(i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12);
                O.Lookup(smpl, null, null, "xx", null, ivTmpvar1, false);
                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl);
                else break;
            }

            p.SetText(@"¤0");
            smpl = O.Smpl();

            for (int iSmpl = 0; iSmpl < int.MaxValue; iSmpl++)
            {
                O.Print(smpl, (O.Indexer(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false), O.Lookup(smpl, null, null, "xx", null, null, false)), O.Negate(smpl, i13))));
                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl);
                else break;
            }
        }
        

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = O.Smpl();

            C0(p);



        }
    }
}
