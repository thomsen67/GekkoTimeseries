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
            Func<IVariable> func28 = () =>
            {
                var smplCommandRemember29 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp27 = new List();

                foreach (IVariable listloop_m125 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1")))), null, false, EVariableType.Var)))
                {
                    foreach (IVariable listloop_m226 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2")))), null, false, EVariableType.Var)))
                    {
                        temp27.Add(O.Indexer(O.Indexer2(smpl, listloop_m125, listloop_m226), smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), listloop_m125, listloop_m226));

                    }
                }
                smpl.command = smplCommandRemember29;
                return temp27;

            };


            O.Prt o0 = new O.Prt();
            o0.prtType = "p";

            {
                List<int> bankNumbers = null;
                O.Prt.Element ope0 = new O.Prt.Element();
                ope0.label = O.SubstituteScalarsAndLists("|||m1, m2|||xx[#m1, #m2]", false);
                smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal); foreach (int bankNumber in bankNumbers)
                {
                    smpl.bankNumber = bankNumber;
                    ope0.variable[bankNumber] = func28();
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
