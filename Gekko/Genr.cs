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
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
        public static void C0(P p)
        {

            GekkoSmpl smpl = O.Smpl();


            p.SetText(@"¤0");
            for (int iSmpl16 = 0; iSmpl16 < int.MaxValue; iSmpl16++)
            {
                List l43 = PrintHelper(smpl);

                O.Print(smpl, l43);


                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl16); else break;
            }



        }

        private static List PrintHelper(GekkoSmpl smpl)
        {
            List l43 = new List();
            for (int i987 = 0; i987 < 2; i987++)
            {
                l43.Add(O.ListDefHelper(O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false, i987), O.Lookup(smpl, null, null, "xx", null, null, false, i987)), O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false, i987), O.Lookup(smpl, null, null, "xx", null, null, false, i987))));
            }

            return l43;
        }

        public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);



}
}
}
