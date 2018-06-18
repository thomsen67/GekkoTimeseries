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
public static readonly ScalarVal i2 = new ScalarVal(1d);
public static readonly ScalarVal i4 = new ScalarVal(6d);
public static readonly ScalarVal i5 = new ScalarVal(2000d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar1 = O.TypeCheck_var(Functions.series(smpl, i2), -1);
O.Lookup(smpl, null, null, "x", null, ivTmpvar1, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar3 = O.TypeCheck_var(i4, -1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var),  ivTmpvar3, new ScalarString(ScalarString.SubstituteScalarsInString(@"2000", true, false))
)
;

p.SetText(@"¤3"); O.InitSmpl(smpl, p);

Func<GraphHelper, string> print2 = (gh) =>
{
O.Prt o2 = new O.Prt();
labelCounter = 0;o2.guiGraphIsRefreshing = gh.isRefreshing;
o2.guiGraphPrintCode = gh.printCode;
o2.guiGraphIsLogTransform = gh.isLogTransform;
o2.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope2 = new O.Prt.Element();
ope2.label = "x[2000]";
smpl = new GekkoSmpl(o2.t1.Add(-2), o2.t2);
ope2.printCodesFinal = Program.GetElementPrintCodes(o2, ope2);bankNumbers = O.Prt.GetBankNumbers(null, ope2.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope2.variable[bankNumber] = O.Indexer(O.Indexer2(smpl, i5
), smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var), O.ReportInterior(smpl, i5
, 0, labelCounter));
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope2);
}
smpl.bankNumber = 0;
o2.prtElements.Add(ope2);
}


o2.counter = 1;
o2.printCsCounter = Globals.printCs.Count - 1;
o2.labelHelper2 = O.AddLabelHelper2(smpl);
o2.Exe();
return o2.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print2); 
print2(new GraphHelper());

p.SetText(@"¤4"); O.InitSmpl(smpl, p);

O.Model o3 = new O.Model();
o3.p = p;o3.fileName = O.ConvertToString((new ScalarString("model")));

o3.opt_gms = "yes";

o3.Exe();

p.SetText(@"¤5"); O.InitSmpl(smpl, p);

ClearTS(p);
O.Read o4 = new O.Read();
o4.p = p;
o4.type = @"read";
o4.opt_gdx = "yes";

o4.fileName = O.ConvertToString((new ScalarString("calib")));


o4.Exe();

p.SetText(@"¤6"); O.InitSmpl(smpl, p);

O.Disp o5 = new O.Disp();
labelCounter = 0;o5.iv = O.CreateListFromStrings(new string[] {"puk"});
o5.Exe();


//[[splitSTOP]]


}
}
}
