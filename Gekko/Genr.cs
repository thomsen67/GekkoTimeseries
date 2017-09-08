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
            IVariable ivTmpvar1 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"));
            O.Lookup(smpl, null, "#m1", null, true, ivTmpvar1)
            ;




            p.SetText(@"¤0");
            IVariable ivTmpvar2 = O.ListDef(new ScalarString(@"x"), new ScalarString(@"y"));
            O.Lookup(smpl, null, "#m2", null, true, ivTmpvar2)
            ;




            p.SetText(@"¤0");

        }

        public static void C1(P p)
        {

            GekkoSmpl smpl = O.Smpl();





        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = O.Smpl();

            C0(p);

            List<List<IVariable>> lists6 = new List<List<IVariable>>();
            lists6.Add((O.Lookup(smpl, null, "#m1", null, true, null)).GetList());
            lists6.Add((O.Lookup(smpl, null, "#m2", null, true, null)).GetList());
            int max7 = O.ForListMax(lists6);
            for (int i8 = 0; i8 < max7; i8++)
            {
                ;
                IVariable forloop_3 = lists6[0][i8];
                IVariable forloop_4 = lists6[1][i8];
                IVariable ivTmpvar5 = O.Add(smpl, forloop_3, forloop_4);
                O.Lookup(smpl, null, "%s", null, true, ivTmpvar5)
                ;

                O.Print(smpl, (O.Lookup(smpl, null, "%s", null, true, null)));

            };

            C1(p);



        }
    }
}
