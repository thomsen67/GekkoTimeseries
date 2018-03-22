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
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤1"); O.InitSmpl(smpl, p);
Func<IVariable> func20 = () => {
var smplCommandRemember21 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
List temp19 = new List();

foreach (IVariable listloop_s18 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, false, EVariableType.Var))) {
temp19.Add(O.Indexer(O.Indexer2(smpl, listloop_s18), smpl, O.Lookup(smpl, null, null, "v", null, null, false, EVariableType.Var), listloop_s18));

}
smpl.command = smplCommandRemember21;
return temp19;

};


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
ope0.label = O.SubstituteScalarsAndLists("|||s|||v[#s]", false);
smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0);bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);foreach(int bankNumber in bankNumbers) {
smpl.bankNumber = bankNumber;
ope0.variable[bankNumber] = func20();
}
smpl.bankNumber = 0;
o0.prtElements.Add(ope0);
}


o0.counter = 3;
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
