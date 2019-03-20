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
            IVariable listloopMovedStuff_11 = O.Lookup(smpl, null, null, "#i", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_12 = O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_13 = O.Lookup(smpl, null, null, "#i", null, null, new LookupSettings(), EVariableType.Var, null);

            Func<IVariable> func15 = () =>
            {
                var smplCommandRemember16 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp14 = new Series(ESeriesType.Normal, Program.options.freq, null); temp14.SetZero(smpl);

                foreach (IVariable listloop_i10 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp14.InjectAdd(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i10), smpl, O.EIndexerType.None, listloopMovedStuff_12, listloop_i10));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember16;
                return temp14;

            };



            //[[commandEnd]]0
        }


        public static Func<GekkoSmpl, IVariable> Evalcode17()
        {
            //return (smpl) => func15();
            return null;
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
