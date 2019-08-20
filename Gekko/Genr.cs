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

            Func<GekkoSmpl, IVariable> func57 = (GekkoSmpl smpl59) =>
            {
                var smplCommandRemember58 = smpl59.command; smpl59.command = GekkoSmplCommand.Unfold;
                List temp56 = new List();

                foreach (IVariable listloop_m55 in new O.GekkoListIterator(O.Lookup(smpl59, null, ((O.scalarStringHash).Add(smpl59, (new ScalarString("m")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp56.Add(O.Lookup(smpl59, null, (O.ReportLabel(smpl59, listloop_m55, "#¨m|[@7,9:9='#',<1290>,1:9]|[@9,11:11='m',<1031>,1:11]")), null, new LookupSettings(), EVariableType.Var, null));

                }
                smpl59.command = smplCommandRemember58;
                return temp56;

            };


            Func<GraphHelper, string> print0 = (gh) =>
            {
                O.Prt o0 = new O.Prt();
                labelCounter = 0; o0.guiGraphIsRefreshing = gh.isRefreshing;
                o0.guiGraphOperator = gh.operator2;
                o0.guiGraphIsLogTransform = gh.isLogTransform;
                o0.prtType = "PRT";
                o0.t1 = Globals.globalPeriodStart;
                o0.t2 = Globals.globalPeriodEnd;

                o0.operators.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));



                ESeriesMissing r1_0 = Program.options.series_array_print_missing; ESeriesMissing r2_0 = Program.options.series_array_calc_missing; ESeriesMissing r3_0 = Program.options.series_data_missing; try
                {
                    O.HandleOptionBankRef1(o0.opt_bank, o0.opt_ref); O.HandleMissing1(o0.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope0 = new O.Prt.Element();
                        ope0.labelGiven = new List<string>() { "|||m|||{#¨m}|[@6,8:8='{',<1323>,1:8]|[@10,12:12='}',<1283>,1:12]" };
                        smpl = new GekkoSmpl(o0.t1, o0.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope0.operatorsFinal = Program.GetElementOperators(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope0.variable[bankNumber] = func57(smpl);
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
                        }
                        smpl.bankNumber = 0;
                        o0.prtElements.Add(ope0);
                    }

                    o0.printCsCounter = Globals.printCs.Count - 1;
                    o0.Exe();
                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_0, r2_0, r3_0);
                }
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
