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

{
List<int> bankNumbers = null;
O.Prt.Element ope0 = new O.Prt.Element();
ope0.label = O.SubstituteScalarsAndLists("x:x", false);
smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o0, ope0));
foreach(int bankNumber in bankNumbers) {
ope0.subElements = new List<O.Prt.SubElement>();
ope0.subElements.Add(new O.Prt.SubElement());
ope0.subElements[0].tsWork = O.Lookup(smpl, null, "x", "x", null, null, false, EVariableType.Var);
}
o0.prtElements.Add(ope0);
}
smpl = null;

o0.counter = 4;
o0.Exe();


//[[splitSTOP]]


}
}
}
