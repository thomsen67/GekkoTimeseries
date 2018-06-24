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
public static readonly ScalarVal i4 = new ScalarVal(2d);
public static readonly ScalarVal i6 = new ScalarVal(1d);
public static readonly ScalarVal i7 = new ScalarVal(2d);
public static readonly ScalarVal i8 = new ScalarVal(3d);
public static readonly ScalarVal i10 = new ScalarVal(4d);
public static readonly ScalarVal i11 = new ScalarVal(5d);
public static readonly ScalarVal i12 = new ScalarVal(6d);
public static readonly ScalarVal i14 = new ScalarVal(7d);
public static readonly ScalarVal i15 = new ScalarVal(8d);
public static readonly ScalarVal i16 = new ScalarVal(9d);
public static readonly ScalarVal i18 = new ScalarVal(14d);
public static readonly ScalarVal i19 = new ScalarVal(15d);
public static readonly ScalarVal i20 = new ScalarVal(16d);
public static readonly ScalarVal i24 = new ScalarVal(2d);
public static readonly ScalarVal i25 = new ScalarVal(2001d);
public static readonly ScalarVal i26 = new ScalarVal(2003d);
public static readonly ScalarVal i30 = new ScalarVal(3d);
public static readonly ScalarVal i34 = new ScalarVal(1d);
public static readonly ScalarVal i36 = new ScalarVal(2d);
public static readonly ScalarVal i38 = new ScalarVal(3d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤1"); O.InitSmpl(smpl, p);

O.Time o0 = new O.Time();
o0.t1 = O.ConvertToDate(i1, O.GetDateChoices.FlexibleStart);
;
o0.t2 = O.ConvertToDate(i2, O.GetDateChoices.FlexibleEnd);
;

o0.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar3 = O.TypeCheck_var(Functions.series(smpl, i4), -1);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar3, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar5 = O.TypeCheck_var(O.ListDefHelper(i6, i7, i8), -1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar5, new ScalarString("a"), new ScalarString("x"))
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar9 = O.TypeCheck_var(O.ListDefHelper(i10, i11, i12), -1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar9, new ScalarString("b"), new ScalarString("x"))
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar13 = O.TypeCheck_var(O.ListDefHelper(i14, i15, i16), -1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar13, new ScalarString("a"), new ScalarString("y"))
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar17 = O.TypeCheck_var(O.ListDefHelper(i18, i19, i20), -1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar17, new ScalarString("b"), new ScalarString("y"))
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar21 = O.TypeCheck_var(O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))), -1);
O.Lookup(smpl, null, null, "#m1", null, ivTmpvar21, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar22 = O.TypeCheck_var(O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))), -1);
O.Lookup(smpl, null, null, "#m2", null, ivTmpvar22, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar23 = O.TypeCheck_var(Functions.series(smpl, i24), -1);
O.Lookup(smpl, null, null, "yy", null, ivTmpvar23, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

O.Reset o9 = new O.Reset();
o9.p = p;o9.Exe(smpl);

p.SetText(@"¤29"); O.InitSmpl(smpl, p);

O.Time o10 = new O.Time();
o10.t1 = O.ConvertToDate(i25, O.GetDateChoices.FlexibleStart);
;
o10.t2 = O.ConvertToDate(i26, O.GetDateChoices.FlexibleEnd);
;

o10.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar27 = null; // O.TypeCheck_var(O.ExplodeIvariables(new List(new List<IVariable> {new ScalarString("a1"), new ScalarString("a2")})), -1);
O.Lookup(smpl, null, null, "#a", null, ivTmpvar27, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar28 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), -1);
O.Lookup(smpl, null, null, "%s", null, ivTmpvar28, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar29 = O.TypeCheck_var(i30, -1);
O.Lookup(smpl, null, null, "%i2", null, ivTmpvar29, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar31 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"i", true, false)), -1);
O.Lookup(smpl, null, null, "%s2", null, ivTmpvar31, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar32 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"s2", true, false)), -1);
O.Lookup(smpl, null, null, "%s3", null, ivTmpvar32, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar33 = O.TypeCheck_var(i34, -1);
O.Lookup(smpl, null, null, "a1z", null, ivTmpvar33, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar35 = O.TypeCheck_var(i36, -1);
O.Lookup(smpl, null, null, "a2z", null, ivTmpvar35, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar37 = O.TypeCheck_var(i38, -1);
O.Lookup(smpl, null, null, "a3z", null, ivTmpvar37, true, EVariableType.Var)
;

p.SetText(@"¤38"); O.InitSmpl(smpl, p);
Func<IVariable> func41 = () => {
var smplCommandRemember42 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
List temp40 = new List();

foreach (IVariable listloop_a39 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, false, EVariableType.Var))) {
O.ClearLabelHelper(smpl);
temp40.Add(O.Lookup(smpl, null, (O.ReportInterior(smpl, O.AddSpecial(smpl, listloop_a39, new ScalarString(ScalarString.SubstituteScalarsInString(@"z", true, false)), false), 0, labelCounter)), null, false, EVariableType.Var));

O.AddLabelHelper(smpl);
}
smpl.command = smplCommandRemember42;
return temp40;

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
ope19.label = "|||a|||{#a+'z'}";
smpl = new GekkoSmpl(o19.t1.Add(-2), o19.t2);
ope19.printCodesFinal = Program.GetElementPrintCodes(o19, ope19);bankNumbers = O.Prt.GetBankNumbers(null, ope19.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope19.variable[bankNumber] = func41();
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope19);
}
smpl.bankNumber = 0;
o19.prtElements.Add(ope19);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope19 = new O.Prt.Element();
ope19.label = "{%s+%{%{''+%s3}+''}2+'z'}";
smpl = new GekkoSmpl(o19.t1.Add(-2), o19.t2);
ope19.printCodesFinal = Program.GetElementPrintCodes(o19, ope19);bankNumbers = O.Prt.GetBankNumbers(null, ope19.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope19.variable[bankNumber] = O.Lookup(smpl, null, (O.ReportInterior(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%s", null, null, false, EVariableType.Var), O.Lookup(smpl, null, (O.scalarStringPercent).Add(smpl, (O.Add(smpl, O.Lookup(smpl, null, (O.scalarStringPercent).Add(smpl, (O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"", true, false)), O.Lookup(smpl, null, null, "%s3", null, null, false, EVariableType.Var)))), null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"", true, false)))).Add(smpl, new ScalarString("2"))), null, false, EVariableType.Var)), new ScalarString(ScalarString.SubstituteScalarsInString(@"z", true, false))), 0, labelCounter)), null, false, EVariableType.Var);
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope19);
}
smpl.bankNumber = 0;
o19.prtElements.Add(ope19);
}


o19.counter = 1;
o19.printCsCounter = Globals.printCs.Count - 1;
o19.labelHelper2 = O.AddLabelHelper2(smpl);
o19.Exe();
return o19.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print19); 
print19(new GraphHelper());


//[[splitSTOP]]


}
}
}
