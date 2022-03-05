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
public static void C0(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_41) {
IVariable forloop_xe7dke6cj_41 = xforloop_xe7dke6cj_41;

//[[commandStart]]1
p.SetStack(@"¤2"); O.InitSmpl(smpl, p);

Func<GraphHelper, string> print1 = (gh) =>
{
O.Prt o1 = new O.Prt();
labelCounter = 0;o1.guiGraphIsRefreshing = gh.isRefreshing;
o1.guiGraphOperator = gh.operator2;
o1.guiGraphIsLogTransform = gh.isLogTransform;
o1.prtType = "p";
ESeriesMissing r1_1 = Program.options.series_array_print_missing; ESeriesMissing r2_1 = Program.options.series_array_calc_missing; ESeriesMissing r3_1 = Program.options.series_data_missing; try {
O.HandleOptionBankRef1(o1.opt_bank, o1.opt_ref); O.HandleMissing1(o1.opt_missing);
{
List<int> bankNumbers = null;
O.Prt.Element ope1 = new O.Prt.Element();
ope1.labelGiven = new List<string>() {"%¨d£.type¨()|[@37,63:63='%',<1359>,2:8]|[@45,74:74=')',<1338>,2:19]"};
smpl = new GekkoSmpl(o1.t1, o1.t2); smpl.t0 = smpl.t0.Add(-2);
ope1.operatorsFinal = Program.GetElementOperators(o1, ope1);bankNumbers = O.Prt.GetBankNumbers(null, ope1.operatorsFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope1.variable[bankNumber] = Functions.type(smpl, null, null, forloop_xe7dke6cj_41);
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope1);
}
smpl.bankNumber = 0;
o1.prtElements.Add(ope1);
}

o1.printStorageAsFuncCounter = Globals.printStorageAsFunc.Count - 1;
o1.Exe();
}
finally {
O.HandleOptionBankRef2(); O.HandleMissing2(r1_1, r2_1, r3_1);
}
return o1.emfName;
};
Globals.printStorageAsFunc.Add(Globals.printStorageAsFunc.Count, print1); 
print1(new GraphHelper());

//[[commandEnd]]1
xforloop_xe7dke6cj_41 = forloop_xe7dke6cj_41;

}


public static readonly ScalarVal i43 = new ScalarVal(1d, 0);
public static readonly ScalarVal i44 = new ScalarVal(3d, 0);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

p.SetStack(@"¤1");


//[[commandSpecial]]0
List<List<IVariable>> lists45 = new List<List<IVariable>>();
lists45.Add(O.ConvertToList(O.Lookup(smpl, null, null, "#datelist", null, null, new  LookupSettings(), EVariableType.Var, null)));
lists45.Add(O.ConvertToList(Functions.seq(smpl, null, null, i43, i44)));
int max46 = O.ForListMax(lists45);
for (int i47 = 0; i47 < max46; i47 ++) {;
//O.TypeCheck_date(forloop_xe7dke6cj_41, 0);
IVariable forloop_xe7dke6cj_41 = lists45[0][i47];
//O.TypeCheck_val(forloop_xe7dke6cj_42, 0);
IVariable forloop_xe7dke6cj_42 = lists45[1][i47];

C0(smpl, p, ref forloop_xe7dke6cj_41);

};

//[[commandEnd]]0



}
}
}
