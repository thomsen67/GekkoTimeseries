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
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

//[[splitSTART]]
p.SetText(@"¤1"); O.InitSmpl(smpl);

O.Prt o0 = new O.Prt();
o0.prtType = "p";

o0.t1 = Globals.globalPeriodStart;
o0.t2 = Globals.globalPeriodEnd;

o0.printCodes.Add(new OptString("r", O.ConvertToString(new ScalarString("yes"))));



{
List<int> bankNumbers = null;
O.Prt.Element ope0 = new O.Prt.Element();
ope0.label = O.SubstituteScalarsAndLists("xx1", false);
smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0);bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
                foreach (int bankNumber in bankNumbers) {
ope0.variable[bankNumber] = O.Lookup(smpl, null, null, "xx1", null, null, false, EVariableType.Var);
}
o0.prtElements.Add(ope0);
}


o0.counter = 3;
o0.Exe();


//[[splitSTOP]]


}
}
}
