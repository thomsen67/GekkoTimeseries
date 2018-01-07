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
            //Globals.arithmentics[0] = (x1, x2) => x1 + x2;
            //public static Func<double, double, double>[] arithmentics = new Func<double, double, double>[20];

            Action<string> func1 = (printCode) =>
            {
                GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

                //[[splitSTART]]
                p.SetText(@"¤1"); O.InitSmpl(smpl, p);

                O.Prt o0 = new O.Prt();
                o0.prtType = "prt";
                                
                o0.interactivePrintCode = printCode;
                                
                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope0 = new O.Prt.Element();
                    ope0.label = O.SubstituteScalarsAndLists("xx", false);
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0); bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal); foreach (int bankNumber in bankNumbers)
                    {
                        smpl.bankNumber = bankNumber;
                        ope0.variable[bankNumber] = O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var);
                    }
                    smpl.bankNumber = 0;
                    o0.prtElements.Add(ope0);
                }

                o0.counter = 5;
                o0.Exe();

                ;
                

            };
            //[[splitSTOP]]

            func1(null);
                        
            func1("p");

        }
    }
}
