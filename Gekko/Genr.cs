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
Func<IVariable> func51 = () => {
Series temp50 = new Series(ESeriesType.Normal, Program.options.freq, null); temp50.SetZero(smpl);

foreach (IVariable listloop_m148 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1")))), null, false, EVariableType.Var))) {
foreach (IVariable listloop_m249 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2")))), null, false, EVariableType.Var))) {
temp50.InjectAdd(smpl, temp50, O.Dollar(smpl, O.Indexer(O.Indexer2(smpl, listloop_m148, listloop_m249), smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), listloop_m148, listloop_m249),null ));

}
}
return temp50;

};


O.Prt o0 = new O.Prt();
o0.prtType = "p";

o0.t1 = Globals.globalPeriodStart;
o0.t2 = Globals.globalPeriodEnd;

o0.printCodes.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));



{
List<int> bankNumbers = null;
O.Prt.Element ope0 = new O.Prt.Element();
ope0.label = O.SubstituteScalarsAndLists("sum((#m1, #m2), xx[#m1, #m2] $ #m3[#m1])", false);
smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0);bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);foreach(int bankNumber in bankNumbers) {
smpl.bankNumber = bankNumber;
ope0.variable[bankNumber] = func51();
}
smpl.bankNumber = 0;
o0.prtElements.Add(ope0);
}


o0.counter = 7;
o0.Exe();


//[[splitSTOP]]


}
}
}
