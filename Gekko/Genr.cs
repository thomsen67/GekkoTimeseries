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
public static void C0(GekkoSmpl smpl, P p) {
//[[commandStart]]0
p.SetText(@"¤1"); O.InitSmpl(smpl, p);
Func<IVariable> func15 = () => {
var smplCommandRemember16 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
List temp14 = new List();

    //IVariable iv = O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, listloop_m13), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), O.ReportLabel(smpl, listloop_m13, "#¨m|[@4,6:6='#',<1210>,1:6]|[@6,8:8='m',<961>,1:8]"));

foreach (IVariable listloop_m13 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m")))), null, new  LookupSettings(), EVariableType.Var, null))) {
temp14.Add(O.Indexer(O.Indexer2(smpl, O.EIndexerType.None,O.Lookup(smpl, null, null, "#m", null, null, new  LookupSettings(), EVariableType.Var, null)
), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new  LookupSettings(), EVariableType.Var, null), O.ReportLabel(smpl, O.Lookup(smpl, null, null, "#m", null, null, new  LookupSettings(), EVariableType.Var, null), "#¨m|[@4,6:6='#',<1210>,1:6]|[@6,8:8='m',<961>,1:8]"))     );

}
smpl.command = smplCommandRemember16;
return temp14;

};


Func<GraphHelper, string> print0 = (gh) =>
{
O.Prt o0 = new O.Prt();
labelCounter = 0;o0.guiGraphIsRefreshing = gh.isRefreshing;
o0.guiGraphPrintCode = gh.printCode;
o0.guiGraphIsLogTransform = gh.isLogTransform;
o0.prtType = "p";
ESeriesMissing r1_0 = Program.options.series_array_print_missing; ESeriesMissing r2_0 = Program.options.series_normal_print_missing; try {
O.HandleOptionBankRef1(o0.opt_bank, o0.opt_ref); O.HandleMissing1(o0.opt_missing);
{
List<int> bankNumbers = null;
O.Prt.Element ope0 = new O.Prt.Element();
ope0.labelGiven = new List<string>() { "|||m|||x[_[#¨m]|[@2,2:2='x',<757>,1:2]|[@7,9:9=']',<1202>,1:9]"};
smpl = new GekkoSmpl(o0.t1, o0.t2); smpl.t0 = smpl.t0.Add(-2);
ope0.printCodesFinal = Program.GetElementPrintCodes(o0, ope0);bankNumbers = O.Prt.GetBankNumbers(null, ope0.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope0.variable[bankNumber] = O.Indexer(O.Indexer2(smpl, O.EIndexerType.None,O.Lookup(smpl, null, null, "#m", null, null, new  LookupSettings(), EVariableType.Var, null)
), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new  LookupSettings(), EVariableType.Var, null), O.ReportLabel(smpl, O.Lookup(smpl, null, null, "#m", null, null, new  LookupSettings(), EVariableType.Var, null)
, "#¨m|[@4,6:6='#',<1210>,1:6]|[@6,8:8='m',<961>,1:8]"));
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
}
smpl.bankNumber = 0;
o0.prtElements.Add(ope0);
}

}
finally {
O.HandleOptionBankRef2(); O.HandleMissing2(r1_0, r2_0);
}
o0.counter = 4;
o0.printCsCounter = Globals.printCs.Count - 1;
o0.Exe();
return o0.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print0); 
print0(new GraphHelper());

//[[commandEnd]]0
}



public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
