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
            IVariable listloopMovedStuff_186 = O.Lookup(smpl, null, null, "#m1", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_187 = O.Lookup(smpl, null, null, "#m2", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_188 = O.Lookup(smpl, null, null, "#m1", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_189 = O.Lookup(smpl, null, null, "#m2", null, null, new LookupSettings(), EVariableType.Var, null);

            Func<IVariable, IVariable, IVariable> func191 = (IVariable listloop_m1182, IVariable listloop_m2183) =>
            {
                var smplCommandRemember192 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp190 = new Series(ESeriesType.Normal, Program.options.freq, null); temp190.SetZero(smpl);

                foreach (IVariable listloop_m1184 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    foreach (IVariable listloop_m2185 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2")))), null, new LookupSettings(), EVariableType.Var, null)))
                    {
                        temp190.InjectAdd(smpl, O.Lookup(smpl, null, (new ScalarString("z")).Concat(smpl, listloop_m1184).Concat(smpl, listloop_m2185), null, new LookupSettings(), EVariableType.Var, null));

                        labelCounter++;
                    }
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember192;
                return temp190;

            };

            Func<IVariable> func194 = () =>
            {
                var smplCommandRemember195 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp193 = new List();

                foreach (IVariable listloop_m1182 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    foreach (IVariable listloop_m2183 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2")))), null, new LookupSettings(), EVariableType.Var, null)))
                    {
                        temp193.Add(func191(listloop_m1182, listloop_m2183));

                    }
                }
                smpl.command = smplCommandRemember195;
                return temp193;

            };


            Func<GraphHelper, string> print0 = (gh) =>
            {
                O.Prt o0 = new O.Prt();
                labelCounter = 0; o0.guiGraphIsRefreshing = gh.isRefreshing;
                o0.guiGraphOperator = gh.operator2;
                o0.guiGraphIsLogTransform = gh.isLogTransform;
                o0.prtType = "p";
                ESeriesMissing r1_0 = Program.options.series_array_print_missing; ESeriesMissing r2_0 = Program.options.series_normal_print_missing; try
                {
                    O.HandleOptionBankRef1(o0.opt_bank, o0.opt_ref); O.HandleMissing1(o0.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope0 = new O.Prt.Element();
                        ope0.labelGiven = new List<string>() { "|||m1, m2|||sum¨((#¨m1,#¨m2),z¨{#¨m1}¨{#¨m2})|[@2,2:4='sum',<1295>,1:2]|[@28,34:34=')',<1242>,1:34]" };
                        smpl = new GekkoSmpl(o0.t1, o0.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope0.operatorsFinal = Program.GetElementOperators(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope0.variable[bankNumber] = func194();
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
                        }
                        smpl.bankNumber = 0;
                        o0.prtElements.Add(ope0);
                    }

                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_0, r2_0);
                }
                o0.counter = 8;
                o0.printCsCounter = Globals.printCs.Count - 1;
                o0.Exe();
                return o0.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print0);
            print0(new GraphHelper());

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
