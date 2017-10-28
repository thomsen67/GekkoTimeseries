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
        public static readonly ScalarVal i97 = new ScalarVal(2d);
        public static readonly ScalarVal i98 = new ScalarVal(3d);
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
            List m99 = null; try
            {
                m99 = new List();
                for (smpl.bankNumber = 0; smpl.bankNumber < 2; smpl.bankNumber++)
                {
                    m99.Add(O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (new Range(i97, i98))));
                }
            }
            finally
            {
                smpl.bankNumber = 0;
            }
            for (int iSmpl100 = 0; iSmpl100 < int.MaxValue; iSmpl100++)
            {
                O.Print(smpl, m99);

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl100); else break;
            }



        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

            C0(p);



        }
    }
}
