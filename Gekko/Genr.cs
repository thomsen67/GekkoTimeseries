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
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

            //[[splitSTART]]
            p.SetText(@"¤1"); O.InitSmpl(smpl);
            Func<IVariable> func30 = () =>
            {
                List temp29 = new List();

                foreach (IVariable listloop_m127 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1")))), null, false, EVariableType.Var)))
                {
                    foreach (IVariable listloop_m228 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2")))), null, false, EVariableType.Var)))
                    {
                        temp29.Add(O.Add(smpl, O.Add(smpl, O.ListDefHelper(O.ListDefHelper(O.Lookup(smpl, null, null, "#m1", null, null, false, EVariableType.Var), O.Lookup(smpl, null, null, "#m2", null, null, false, EVariableType.Var)), O.Indexer(O.Indexer2(smpl, listloop_m127, listloop_m228), smpl, O.Lookup(smpl, null, null, "xx3", null, null, false, EVariableType.Var), listloop_m127, listloop_m228)), O.Indexer(O.Indexer2(smpl, listloop_m127, new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false))
                        ), smpl, O.Lookup(smpl, null, null, "xx3", null, null, false, EVariableType.Var), listloop_m127, new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false))
                        )), O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
                        , listloop_m228), smpl, O.Lookup(smpl, null, null, "xx3", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
                        , listloop_m228)));

                    }
                }
                return temp29;

            };


            O.Prt o0 = new O.Prt();
            o0.prtType = "p";

            o0.t1 = Globals.globalPeriodStart;
            o0.t2 = Globals.globalPeriodEnd;

            o0.printCodes.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));



            {
                List<int> bankNumbers = null;
                O.Prt.Element ope0 = new O.Prt.Element();
                ope0.label = O.SubstituteScalarsAndLists("|||m1, m2|||((#m1, #m2), xx3[#m1, #m2]) + xx3[#m1, 'x'] + xx3['a', #m2]", false);
                smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal); foreach (int bankNumber in bankNumbers)
                {
                    smpl.bankNumber = bankNumber;
                    ope0.variable[bankNumber] = func30();
                }
                smpl.bankNumber = 0;
                o0.prtElements.Add(ope0);
            }


            o0.counter = 2;
            o0.Exe();


            //[[splitSTOP]]


        }
    }
}
