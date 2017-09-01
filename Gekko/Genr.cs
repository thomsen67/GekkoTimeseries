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
        public static IVariable temp7_XXX(GekkoSmpl smpl)
        {
            TimeSeries temp7 = new TimeSeries(Program.options.freq, null); temp7.SetZero(smpl);

            foreach (IVariable listloop_i6 in new O.GekkoListIterator(O.Lookup(smpl, ((O.scalarStringHash).Add(smpl, (new ScalarString("i", true, false)))))))
            {
                temp7.InjectAdd(smpl, temp7, O.Add(smpl, O.Indexer(smpl, O.Lookup(smpl, ((new ScalarString("x", true, false)))), false, listloop_i6), O.Indexer(smpl, O.Lookup(smpl, ((new ScalarString("x", true, false)))), false, listloop_i6)));

            }
            return temp7;

        }

        public static IVariable temp7(GekkoSmpl smpl)
        {
            MetaList temp7 = new MetaList();
            
            foreach (IVariable listloop_i6 in new O.GekkoListIterator(O.Lookup(smpl, ((O.scalarStringHash).Add(smpl, (new ScalarString("i", true, false)))))))
            {
                temp7.Add(O.Add(smpl, O.Indexer(smpl, O.Lookup(smpl, ((new ScalarString("x", true, false)))), false, listloop_i6), O.Indexer(smpl, O.Lookup(smpl, ((new ScalarString("x", true, false)))), false, listloop_i6)));
            }
            return temp7;

        }

        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoSmpl smpl = O.Smpl();


            p.SetText(@"¤1");
            O.Print(smpl, (temp7(smpl)));




        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = O.Smpl();

            C0(p);



        }
    }
}
