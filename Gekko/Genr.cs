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

            Func<IVariable> func122 = () =>
            {
                IVariable listloopMovedStuff_118 = O.Lookup(smpl, null, null, "#i", null, null, new LookupSettings(), EVariableType.Var, null);
                IVariable listloopMovedStuff_119 = O.Lookup(smpl, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null);
                IVariable listloopMovedStuff_120 = O.Lookup(smpl, null, null, "#i", null, null, new LookupSettings(), EVariableType.Var, null);

                var smplCommandRemember123 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp121 = new Series(ESeriesType.Normal, Program.options.freq, null); temp121.SetZero(smpl);

                foreach (IVariable listloop_i117 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp121.InjectAdd(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, new ScalarString("a"), listloop_i117), smpl, O.EIndexerType.None, listloopMovedStuff_119, new ScalarString("a"), listloop_i117));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember123;
                return temp121;

            };

            Func<GekkoSmpl, IVariable> Evalcode124 = (smpl5) =>
            {
                return func122();
            };

            O.Decomp o0 = new O.Decomp();
            o0.label = @"sum(#i, xx[a,#i])";

            o0.expression = Evalcode124;

            o0.Exe();

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
