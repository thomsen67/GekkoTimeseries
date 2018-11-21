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

O.Assignment o0 = new O.Assignment();
O.AdjustT0(smpl, -1);
IVariable ivTmpvar1 = i2;
O.AdjustT0(smpl, 1);
O.Lookup(smpl, null, null, "x", null, ivTmpvar1, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
;

//[[commandEnd]]0


//[[commandStart]]1
p.SetText(@"¤1"); O.InitSmpl(smpl, p);

Func<GraphHelper, string> print1 = (gh) =>
{
O.Prt o1 = new O.Prt();
labelCounter = 0;o1.guiGraphIsRefreshing = gh.isRefreshing;
o1.guiGraphPrintCode = gh.printCode;
o1.guiGraphIsLogTransform = gh.isLogTransform;
o1.prtType = "p";
ESeriesMissing r1_1 = Program.options.series_array_print_missing; ESeriesMissing r2_1 = Program.options.series_normal_print_missing; try {
O.HandleOptionBankRef1(o1.opt_bank, o1.opt_ref); O.HandleMissing1(o1.opt_missing);
{
List<int> bankNumbers = null;
O.Prt.Element ope1 = new O.Prt.Element();
//ope1.labelGiven = O.GetListOfStringsFromIVariable(new List<string>() { "x|[@6,6:6='x',<757>,1:6]|[@6,6:6='x',<757>,1:6]"});
smpl = new GekkoSmpl(o1.t1, o1.t2); smpl.t0 = smpl.t0.Add(-2);
ope1.printCodesFinal = Program.GetElementPrintCodes(o1, ope1);bankNumbers = O.Prt.GetBankNumbers(null, ope1.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope1.variable[bankNumber] = O.Lookup(smpl, null, null, "x", null, null, new  LookupSettings(), EVariableType.Var, null);
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope1);
}
smpl.bankNumber = 0;
o1.prtElements.Add(ope1);
}

}
finally {
O.HandleOptionBankRef2(); O.HandleMissing2(r1_1, r2_1);
}
o1.counter = 1;
o1.printCsCounter = Globals.printCs.Count - 1;
o1.Exe();
return o1.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print1); 
print1(new GraphHelper());

//[[commandEnd]]1
}


public static readonly ScalarVal i2 = new ScalarVal(5d);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
