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
            Func<IVariable, IVariable> func6 = (IVariable listloop_i3) =>
            {
                var smplCommandRemember7 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp5 = new Series(ESeriesType.Normal, Program.options.freq, null); temp5.SetZero(smpl);

                foreach (IVariable listloop_j4 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("j")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp5.InjectAdd(smpl, temp5, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_i3, listloop_j4), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i3, listloop_j4));

                    labelCounter++;
                }
                smpl.command = smplCommandRemember7;
                return temp5;

            };

            Func<IVariable> func9 = () =>
            {
                var smplCommandRemember10 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp8 = new Series(ESeriesType.Normal, Program.options.freq, null); temp8.SetZero(smpl);

                foreach (IVariable listloop_i3 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp8.InjectAdd(smpl, temp8, func6(listloop_i3));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember10;
                return temp8;

            };

            //x = 1 + sum(#i, sum(#j, x[#i, #j]));

            O.Assignment o0 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar1 = O.Add(smpl, i2, func9());
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "x", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
            ;

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i2 = new ScalarVal(1d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
