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
public static int labelCounter;
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
labelCounter = 0;o0.guiGraphIsRefreshing = gh.isRefreshing;
o0.guiGraphPrintCode = gh.printCode;
o0.guiGraphIsLogTransform = gh.isLogTransform;
o0.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope0 = new O.Prt.Element();
ope0.label = "x{'a'+'b'}";
smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0);bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope0.variable[bankNumber] = O.Lookup(smpl, null, (new ScalarString("x")).Add(smpl, O.ReportLabel(smpl, O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))), "'a'+'b'|[@5,5:7=''a'',<1154>,1:5]|[@7,9:11=''b'',<1154>,1:9]")), null, false, EVariableType.Var);
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
}
smpl.bankNumber = 0;
o0.prtElements.Add(ope0);
}


o0.counter = 3;
o0.printCsCounter = Globals.printCs.Count - 1;
o0.labelHelper2 = O.AddLabelHelper2(smpl);
o0.Exe();
return o0.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print0); 
print0(new GraphHelper());

p.SetText(@"¤2"); O.InitSmpl(smpl, p);

Func<GraphHelper, string> print1 = (gh) =>
{
O.Prt o1 = new O.Prt();
labelCounter = 0;o1.guiGraphIsRefreshing = gh.isRefreshing;
o1.guiGraphPrintCode = gh.printCode;
o1.guiGraphIsLogTransform = gh.isLogTransform;
o1.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope1 = new O.Prt.Element();
ope1.label = "x['a'+'b']";
smpl = new GekkoSmpl(o1.t1.Add(-2), o1.t2);
ope1.printCodesFinal = Program.GetElementPrintCodes(o1, ope1);bankNumbers = O.Prt.GetBankNumbers(null, ope1.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope1.variable[bankNumber] = O.Indexer(O.Indexer2(smpl, O.ReportLabel(smpl, O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false)))
, "'a'+'b'|[@15,22:24=''a'',<1154>,2:6]|[@17,26:28=''b'',<1154>,2:10]")), smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var), O.ReportInterior(smpl, O.ReportLabel(smpl, O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false)))
, "'a'+'b'|[@15,22:24=''a'',<1154>,2:6]|[@17,26:28=''b'',<1154>,2:10]"), 0, labelCounter));
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope1);
}
smpl.bankNumber = 0;
o1.prtElements.Add(ope1);
}


o1.counter = 4;
o1.printCsCounter = Globals.printCs.Count - 1;
o1.labelHelper2 = O.AddLabelHelper2(smpl);
o1.Exe();
return o1.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print1); 
print1(new GraphHelper());


//[[splitSTOP]]


}
}
}
