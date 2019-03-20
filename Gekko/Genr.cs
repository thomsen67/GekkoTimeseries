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
            IVariable listloopMovedStuff_4 = O.Lookup(smpl, null, null, "#i", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_5 = O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_6 = O.Lookup(smpl, null, null, "#i", null, null, new LookupSettings(), EVariableType.Var, null);

            Func<IVariable> func8 = () =>
            {
                var smplCommandRemember9 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp7 = new Series(ESeriesType.Normal, Program.options.freq, null); temp7.SetZero(smpl);

                foreach (IVariable listloop_i3 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp7.InjectAdd(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3), smpl, O.EIndexerType.None, listloopMovedStuff_5, listloop_i3));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember9;
                return temp7;

            };

            Func<GekkoSmpl, IVariable> Evalcode10 = (smpl5) =>
            {
                return func8();
            };


            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
