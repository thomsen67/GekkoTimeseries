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
        public static void C0(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_1)
        {
            IVariable forloop_xe7dke6cj_1 = xforloop_xe7dke6cj_1;

            //[[commandStart]]1
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);
            Func<IVariable> func5 = () =>
            {
                var smplCommandRemember6 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp4 = new List();

                foreach (IVariable listloop_a2 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    foreach (IVariable listloop_i3 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                    {
                        temp4.Add(O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_a2, listloop_i3), smpl, O.EIndexerType.None, O.Lookup(smpl, null, (O.ReportLabel(smpl, forloop_xe7dke6cj_1, "%¨ts|[@19,32:32='%',<1211>,1:32]|[@21,34:35='ts',<1245>,1:34]")), null, new LookupSettings(), EVariableType.Var, null), O.ReportLabel(smpl, listloop_a2, "#¨a|[@24,40:40='#',<1204>,1:40]|[@26,42:42='a',<765>,1:42]"), O.ReportLabel(smpl, listloop_i3, "#¨i|[@29,45:45='#',<1204>,1:45]|[@31,47:47='i',<1245>,1:47]")));

                    }
                }
                smpl.command = smplCommandRemember6;
                return temp4;

            };


            Func<GraphHelper, string> print1 = (gh) =>
            {
                O.Prt o1 = new O.Prt();
                labelCounter = 0; o1.guiGraphIsRefreshing = gh.isRefreshing;
                o1.guiGraphPrintCode = gh.printCode;
                o1.guiGraphIsLogTransform = gh.isLogTransform;
                o1.prtType = "mulprt";
                ESeriesMissing r1_1 = Program.options.series_array_print_missing; ESeriesMissing r2_1 = Program.options.series_normal_print_missing; try
                {
                    O.HandleOptionBankRef1(o1.opt_bank, o1.opt_ref); O.HandleMissing1(o1.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope1 = new O.Prt.Element();
                        ope1.labelGiven = new List<string>() { "|||a, i|||{%¨ts}[_[#¨a, #¨i]|[@18,31:31='{',<1237>,1:31]|[@32,48:48=']',<1196>,1:48]" };
                        smpl = new GekkoSmpl(o1.t1, o1.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope1.printCodesFinal = Program.GetElementPrintCodes(o1, ope1); bankNumbers = O.Prt.GetBankNumbers(null, ope1.printCodesFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope1.variable[bankNumber] = func5();
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope1);
                        }
                        smpl.bankNumber = 0;
                        o1.prtElements.Add(ope1);
                    }

                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_1, r2_1);
                }
                o1.counter = 1;
                o1.printCsCounter = Globals.printCs.Count - 1;
                o1.Exe();
                return o1.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print1);
            print1(new GraphHelper());

            //[[commandEnd]]1
            xforloop_xe7dke6cj_1 = forloop_xe7dke6cj_1;

        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[commandSpecial]]0
            IVariable forloop_xe7dke6cj_1 = null;
            int counter7 = 0;
            for (O.IterateStart(ref forloop_xe7dke6cj_1, O.ExplodeIvariablesSeq(new List(new List<IVariable> { (new ScalarString("x")), (new ScalarString("y")) }))); O.IterateContinue(forloop_xe7dke6cj_1, O.ExplodeIvariablesSeq(new List(new List<IVariable> { (new ScalarString("x")), (new ScalarString("y")) })), null, null, ref counter7); O.IterateStep(forloop_xe7dke6cj_1, O.ExplodeIvariablesSeq(new List(new List<IVariable> { (new ScalarString("x")), (new ScalarString("y")) })), null, counter7))
            {
                ;

                C0(smpl, p, ref forloop_xe7dke6cj_1);

            };

            //[[commandEnd]]0



        }
    }
}
