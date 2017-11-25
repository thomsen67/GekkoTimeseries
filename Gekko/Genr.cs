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
            Func<IVariable> func59 = () =>
            {
                List temp58 = new List();

                foreach (IVariable listloop_m57 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m")))), null, false, EVariableType.Var)))
                {
                    temp58.Add(O.Indexer(O.Indexer2(smpl, listloop_m57), smpl, O.Lookup(smpl, null, null, "xx1", null, null, false, EVariableType.Var), listloop_m57));

                }
                return temp58;

            };


            O.Prt o0 = new O.Prt();
            o0.prtType = "prt";

            {
                List<int> bankNumbers = null;
                O.Prt.Element ope0 = new O.Prt.Element();
                ope0.label = O.SubstituteScalarsAndLists("unfold(#m,xx1[#m])", false);
                smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal); foreach (int bankNumber in bankNumbers)
                {
                    smpl.bankNumber = bankNumber;
                    ope0.variable[bankNumber] = func59();
                }
                smpl.bankNumber = 0;
                o0.prtElements.Add(ope0);
            }


            o0.counter = 17;
            o0.Exe();


            //[[splitSTOP]]


        }
    }
}
