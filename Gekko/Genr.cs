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
public static readonly ScalarVal i26 = new ScalarVal(1d);
public static readonly ScalarVal i28 = new ScalarVal(4d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar25 = O.TypeCheck_var(Functions.series(smpl, i26), -1);
O.Lookup(smpl, null, null, "xab", null, ivTmpvar25, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar27 = O.TypeCheck_var(i28, -1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xab", null, null, true, EVariableType.Var),  ivTmpvar27, new ScalarString("cd"))
;

p.SetText(@"¤3"); O.InitSmpl(smpl, p);

O.Clone o2 = new O.Clone();
o2.Exe();

p.SetText(@"¤4"); O.InitSmpl(smpl, p);

Func<GraphHelper, string> print3 = (gh) =>
{
O.Prt o3 = new O.Prt();
labelCounter = 0;o3.guiGraphIsRefreshing = gh.isRefreshing;
o3.guiGraphPrintCode = gh.printCode;
o3.guiGraphIsLogTransform = gh.isLogTransform;
o3.prtType = "p";

o3.t1 = Globals.globalPeriodStart;
o3.t2 = Globals.globalPeriodEnd;

o3.printCodes.Add(new OptString("m", O.ConvertToString(new ScalarString("yes"))));



{
List<int> bankNumbers = null;
O.Prt.Element ope3 = new O.Prt.Element();
ope3.label22 = "x¨{'a'+'b'}[_['c'+'d']|[@25,44:44='x',<721>,4:5]|[@36,65:65=']',<1155>,4:26]";
ope3.label = "[<{THIS IS A LABEL}>][@25,44:44='x',<721>,4:5]";
smpl = new GekkoSmpl(o3.t1.Add(-2), o3.t2);
ope3.printCodesFinal = Program.GetElementPrintCodes(o3, ope3);bankNumbers = O.Prt.GetBankNumbers(null, ope3.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope3.variable[bankNumber] = O.Indexer(O.Indexer2(smpl, O.ReportLabel(smpl, O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"d", true, false)))
, "'c'+'d'|[@33,58:60=''c'',<1154>,4:19]|[@35,62:64=''d'',<1154>,4:23]")), smpl, O.Lookup(smpl, null, (new ScalarString("x")).Add(smpl, O.ReportLabel(smpl, O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))), "'a'+'b'|[@28,47:49=''a'',<1154>,4:8]|[@30,51:53=''b'',<1154>,4:12]")), null, false, EVariableType.Var), O.ReportLabel(smpl, O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"d", true, false)))
, "'c'+'d'|[@33,58:60=''c'',<1154>,4:19]|[@35,62:64=''d'',<1154>,4:23]"));
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope3);
}
smpl.bankNumber = 0;
o3.prtElements.Add(ope3);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope3 = new O.Prt.Element();
ope3.label22 = "x¨{'a'+'b'}[_['c'+'d']|[@39,68:68='x',<721>,4:29]|[@50,89:89=']',<1155>,4:50]";
ope3.label = "[<{THIS IS A LABEL}>][@39,68:68='x',<721>,4:29]";
smpl = new GekkoSmpl(o3.t1.Add(-2), o3.t2);
ope3.printCodesFinal = Program.GetElementPrintCodes(o3, ope3);bankNumbers = O.Prt.GetBankNumbers(null, ope3.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope3.variable[bankNumber] = O.Indexer(O.Indexer2(smpl, O.ReportLabel(smpl, O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"d", true, false)))
, "'c'+'d'|[@47,82:84=''c'',<1154>,4:43]|[@49,86:88=''d'',<1154>,4:47]")), smpl, O.Lookup(smpl, null, (new ScalarString("x")).Add(smpl, O.ReportLabel(smpl, O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))), "'a'+'b'|[@42,71:73=''a'',<1154>,4:32]|[@44,75:77=''b'',<1154>,4:36]")), null, false, EVariableType.Var), O.ReportLabel(smpl, O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"d", true, false)))
, "'c'+'d'|[@47,82:84=''c'',<1154>,4:43]|[@49,86:88=''d'',<1154>,4:47]"));
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope3);
}
smpl.bankNumber = 0;
o3.prtElements.Add(ope3);
}


o3.counter = 7;
o3.printCsCounter = Globals.printCs.Count - 1;
o3.labelHelper2 = O.AddLabelHelper2(smpl);
o3.labelHelper22 = O.AddLabelHelper22(smpl);
o3.Exe();
return o3.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print3); 
print3(new GraphHelper());


//[[splitSTOP]]


}
}
}
