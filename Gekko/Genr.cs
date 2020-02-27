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

            O.Assignment o0 = new O.Assignment();
            o0.opt_source = @"<[code]>y4 = sum((#i, #j) $ (i0[#i, #j]), x[#i, #j])";

            Func<GekkoSmpl, IVariable> func5 = (GekkoSmpl smpl8) =>
            {
                var smplCommandRemember6 = smpl8.command; smpl8.command = GekkoSmplCommand.Sum;
                Series temp4 = new Series(ESeriesType.Normal, Program.options.freq, null); temp4.SetZero(smpl8);

                foreach (IVariable listloop_i2 in new O.GekkoListIterator(O.Lookup(smpl8, null, ((O.scalarStringHash).Add(smpl8, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    foreach (IVariable listloop_j3 in new O.GekkoListIterator(O.Lookup(smpl8, null, ((O.scalarStringHash).Add(smpl8, (new ScalarString("j")))), null, new LookupSettings(), EVariableType.Var, null)))
                    {
                        IVariable tmp = O.Indexer(O.Indexer2(smpl8, O.EIndexerType.None, listloop_i2, listloop_j3), smpl8, O.EIndexerType.None, O.Lookup(smpl8, null, null, "i00", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i2, listloop_j3);
                        double v7 = O.Conditional2Of3(tmp);
                        if (v7 != 1d) continue;
                        temp4.InjectAdd(smpl8, O.Indexer(O.Indexer2(smpl8, O.EIndexerType.None, listloop_i2, listloop_j3), smpl8, O.EIndexerType.None, O.Lookup(smpl8, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), listloop_i2, listloop_j3));

                        labelCounter++;
                    }
                }
                labelCounter = 0;
                smpl8.command = smplCommandRemember6;
                return temp4;

            };


            Action assign_9 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar1 = func5(smpl);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y4", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
            };
            Func<bool> check_9 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar1 = func5(smpl);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar1.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y4", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_9, check_9, o0);

            //[[commandEnd]]0
        }

        

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);
                       
        }
    }
}
