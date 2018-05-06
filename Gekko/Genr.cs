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
public static readonly ScalarVal i21 = new ScalarVal(2d);
public static readonly ScalarVal i22 = new ScalarVal(2d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤1"); O.InitSmpl(smpl, p);

Func<GraphHelper, string> print0 = (gh) =>
{
O.Prt o0 = new O.Prt();
o0.guiGraphIsRefreshing = gh.isRefreshing;
o0.guiGraphPrintCode = gh.printCode;
o0.guiGraphIsLogTransform = gh.isLogTransform;
o0.prtType = "prt";

{
List<int> bankNumbers = null;
O.Prt.Element ope0 = new O.Prt.Element();
ope0.label = O.SubstituteScalarsAndLists("2+2", false);
smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0);bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);foreach(int bankNumber in bankNumbers) {
smpl.bankNumber = bankNumber;
ope0.variable[bankNumber] = O.Add(smpl, i21, i22);
}
smpl.bankNumber = 0;
o0.prtElements.Add(ope0);
}


o0.counter = 4;
o0.printCsCounter = Globals.printCs.Count - 1;
o0.Exe();
return o0.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print0); 
print0(new GraphHelper());


//[[splitSTOP]]


}
}
}
