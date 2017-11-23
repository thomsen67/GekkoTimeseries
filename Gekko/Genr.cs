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
            Func<IVariable> func47 = () =>
            {
                List temp46 = new List();

                foreach (IVariable listloop_m45 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m")))), null, false, EVariableType.Var)))
                {
                    temp46.Add(O.Lookup(smpl, null, (listloop_m45), null, false, EVariableType.Var));

                }
                return temp46;

            };


            O.Prt o0 = new O.Prt();
            o0.prtType = "p";

            {
                List<int> bankNumbers = null;
                O.Prt.Element ope0 = new O.Prt.Element();
                ope0.label = O.SubstituteScalarsAndLists("unfold(#m, {#m})", false);
                smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal); foreach (int bankNumber in bankNumbers)
                {
                    smpl.bankNumber = bankNumber;
                    ope0.variable[bankNumber] = func47();
                }
                smpl.bankNumber = 0;
                o0.prtElements.Add(ope0);
            }


            o0.counter = 3;
            o0.Exe();


            //[[splitSTOP]]


        }
    }
}
