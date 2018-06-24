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
Func<IVariable> func69 = () => {
var smplCommandRemember70 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
List temp68 = new List();

foreach (IVariable listloop_a67 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, false, EVariableType.Var))) {
O.ClearLabelHelper(smpl);
temp68.Add(O.Indexer(O.Indexer2(smpl, O.AddSpecial(smpl, listloop_a67, new ScalarString(ScalarString.SubstituteScalarsInString(@"z", true, false)), false)), smpl, O.Lookup(smpl, null, (O.ReportInterior(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%s9", null, null, false, EVariableType.Var), O.Lookup(smpl, null, null, "%s9", null, null, false, EVariableType.Var)), 0, labelCounter)), null, false, EVariableType.Var), O.ReportInterior(smpl, O.AddSpecial(smpl, listloop_a67, new ScalarString(ScalarString.SubstituteScalarsInString(@"z", true, false)), false), 0, labelCounter)));

O.AddLabelHelper(smpl);
}
smpl.command = smplCommandRemember70;
return temp68;

};


Func<GraphHelper, string> print0 = (gh) =>
{
O.Prt o0 = new O.Prt();
labelCounter = 0;o0.guiGraphIsRefreshing = gh.isRefreshing;
o0.guiGraphPrintCode = gh.printCode;
o0.guiGraphIsLogTransform = gh.isLogTransform;
o0.prtType = "p";

o0.t1 = Globals.globalPeriodStart;
o0.t2 = Globals.globalPeriodEnd;

o0.printCodes.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));



{
List<int> bankNumbers = null;
O.Prt.Element ope0 = new O.Prt.Element();
ope0.label = "|||a|||{%s9+%s9}[#a+'z']";
smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0);bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope0.variable[bankNumber] = func69();
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
}
smpl.bankNumber = 0;
o0.prtElements.Add(ope0);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope0 = new O.Prt.Element();
ope0.label = "{%s9+%s9}[%s+%{%{''+%s3}+''}2+'z']";
smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0);bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope0.variable[bankNumber] = O.Indexer(O.Indexer2(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%s", null, null, false, EVariableType.Var), O.Lookup(smpl, null, (O.scalarStringPercent).Add(smpl, (O.Add(smpl, O.Lookup(smpl, null, (O.scalarStringPercent).Add(smpl, (O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"", true, false)), O.Lookup(smpl, null, null, "%s3", null, null, false, EVariableType.Var)))), null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"", true, false)))).Add(smpl, new ScalarString("2"))), null, false, EVariableType.Var)), new ScalarString(ScalarString.SubstituteScalarsInString(@"z", true, false)))
), smpl, O.Lookup(smpl, null, (O.ReportInterior(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%s9", null, null, false, EVariableType.Var), O.Lookup(smpl, null, null, "%s9", null, null, false, EVariableType.Var)), 0, labelCounter)), null, false, EVariableType.Var), O.ReportInterior(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%s", null, null, false, EVariableType.Var), O.Lookup(smpl, null, (O.scalarStringPercent).Add(smpl, (O.Add(smpl, O.Lookup(smpl, null, (O.scalarStringPercent).Add(smpl, (O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"", true, false)), O.Lookup(smpl, null, null, "%s3", null, null, false, EVariableType.Var)))), null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"", true, false)))).Add(smpl, new ScalarString("2"))), null, false, EVariableType.Var)), new ScalarString(ScalarString.SubstituteScalarsInString(@"z", true, false)))
, 0, labelCounter));
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
}
smpl.bankNumber = 0;
o0.prtElements.Add(ope0);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope0 = new O.Prt.Element();
ope0.label = "f({%s9+%s9}, 'a3z')";
smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0);bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope0.variable[bankNumber] = O.FunctionLookup2("f")(smpl, p, O.Lookup(smpl, null, (O.ReportInterior(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%s9", null, null, false, EVariableType.Var), O.Lookup(smpl, null, null, "%s9", null, null, false, EVariableType.Var)), 0, labelCounter)), null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"a3z", true, false)));
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


//[[splitSTOP]]


}
}
}
