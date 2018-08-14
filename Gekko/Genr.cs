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
public static readonly ScalarVal i1 = new ScalarVal(2001d);
public static readonly ScalarVal i2 = new ScalarVal(2003d);
public static readonly ScalarVal i4 = new ScalarVal(5d);
public static readonly ScalarVal i6 = new ScalarVal(6d);
public static readonly ScalarVal i10 = new ScalarVal(3d);
public static readonly ScalarVal i14 = new ScalarVal(1d);
public static readonly ScalarVal i16 = new ScalarVal(1d);
public static readonly ScalarVal i18 = new ScalarVal(2d);
public static readonly ScalarVal i20 = new ScalarVal(3d);
public static readonly ScalarVal i23 = new ScalarVal(1d);
public static readonly ScalarVal i25 = new ScalarVal(11d);
public static readonly ScalarVal i27 = new ScalarVal(22d);
public static readonly ScalarVal i29 = new ScalarVal(9d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl, p);

O.Reset o0 = new O.Reset();
o0.p = p;o0.Exe(smpl);

p.SetText(@"¤3"); O.InitSmpl(smpl, p);

O.Time o1 = new O.Time();
o1.t1 = O.ConvertToDate(i1, O.GetDateChoices.FlexibleStart);
;
o1.t2 = O.ConvertToDate(i2, O.GetDateChoices.FlexibleEnd);
;

o1.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar3 = O.TypeCheck_var(i4, -1);
O.Lookup(smpl, null, null, "z1", null, ivTmpvar3, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar5 = O.TypeCheck_var(i6, -1);
O.Lookup(smpl, null, null, "z2", null, ivTmpvar5, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar7 = O.TypeCheck_var(new List(O.ExplodeIvariables(new List(new List<IVariable> {new ScalarString("a1"), new ScalarString("a2")}))), -1);
O.Lookup(smpl, null, null, "#a", null, ivTmpvar7, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar8 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), -1);
O.Lookup(smpl, null, null, "%s", null, ivTmpvar8, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar9 = O.TypeCheck_var(i10, -1);
O.Lookup(smpl, null, null, "%i2", null, ivTmpvar9, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar11 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"i", true, false)), -1);
O.Lookup(smpl, null, null, "%s2", null, ivTmpvar11, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar12 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"s2", true, false)), -1);
O.Lookup(smpl, null, null, "%s3", null, ivTmpvar12, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar13 = O.TypeCheck_var(Functions.series(smpl, i14), -1);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar13, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar15 = O.TypeCheck_var(i16, -1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar15, new ScalarString("a1z"))
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar17 = O.TypeCheck_var(i18, -1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar17, new ScalarString("a2z"))
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar19 = O.TypeCheck_var(i20, -1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar19, new ScalarString("a3z"))
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar21 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false)), -1);
O.Lookup(smpl, null, null, "%s9", null, ivTmpvar21, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar22 = O.TypeCheck_var(Functions.series(smpl, i23), -1);
O.Lookup(smpl, null, null, "yy", null, ivTmpvar22, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar24 = O.TypeCheck_var(i25, -1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "yy", null, null, true, EVariableType.Var),  ivTmpvar24, new ScalarString("a"))
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar26 = O.TypeCheck_var(i27, -1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "yy", null, null, true, EVariableType.Var),  ivTmpvar26, new ScalarString("b"))
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar28 = O.TypeCheck_var(i29, -1);
O.Lookup(smpl, null, null, "xy", null, ivTmpvar28, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar30 = O.TypeCheck_list(O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"xx", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"yy", true, false))), -1);
O.Lookup(smpl, null, null, "m", null, ivTmpvar30, true, EVariableType.List)
;

p.SetText(@"¤27"); O.InitSmpl(smpl, p);
Func<IVariable> func33 = () => {
var smplCommandRemember34 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
List temp32 = new List();

foreach (IVariable listloop_m31 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m")))), null, false, EVariableType.Var))) {
temp32.Add(O.Lookup(smpl, null, (O.ReportLabel(smpl, listloop_m31, "#¨m|[@179,492:492='#',<1159>,27:9]|[@181,494:494='m',<919>,27:11]")), null, false, EVariableType.Var));

}
smpl.command = smplCommandRemember34;
return temp32;

};


Func<GraphHelper, string> print19 = (gh) =>
{
O.Prt o19 = new O.Prt();
labelCounter = 0;o19.guiGraphIsRefreshing = gh.isRefreshing;
o19.guiGraphPrintCode = gh.printCode;
o19.guiGraphIsLogTransform = gh.isLogTransform;
o19.prtType = "p";

o19.t1 = Globals.globalPeriodStart;
o19.t2 = Globals.globalPeriodEnd;

o19.printCodes.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));



{
List<int> bankNumbers = null;
O.Prt.Element ope19 = new O.Prt.Element();
ope19.labelGiven = new List<string>() { "|||m|||{#¨m}|[@178,491:491='{',<1190>,27:8]|[@182,495:495='}',<1165>,27:12]"};
smpl = new GekkoSmpl(o19.t1.Add(-2), o19.t2);
ope19.printCodesFinal = Program.GetElementPrintCodes(o19, ope19);bankNumbers = O.Prt.GetBankNumbers(null, ope19.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope19.variable[bankNumber] = func33();
O.PrtElementHandleLabel(smpl, ope19);
}
smpl.bankNumber = 0;
o19.prtElements.Add(ope19);
}


o19.counter = 1;
o19.printCsCounter = Globals.printCs.Count - 1;
o19.Exe();
return o19.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print19); 
print19(new GraphHelper());


//[[splitSTOP]]


}
}
}
