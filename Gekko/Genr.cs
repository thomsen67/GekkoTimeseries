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
p.SetText(@"¤27"); O.InitSmpl(smpl, p);

O.Read o0 = new O.Read();
o0.p = p;
o0.type = @"read";
o0.fileName = O.ConvertToString((new ScalarString("jul05")));


o0.Exe();

//[[commandEnd]]0


//[[commandStart]]1
p.SetText(@"¤27"); O.InitSmpl(smpl, p);

O.Time o1 = new O.Time();
o1.t1 = O.ConvertToDate(i1, O.GetDateChoices.FlexibleStart);
;
o1.t2 = O.ConvertToDate(i2, O.GetDateChoices.FlexibleEnd);
;

o1.Exe();

//[[commandEnd]]1


//[[commandStart]]2
p.SetText(@"¤28"); O.InitSmpl(smpl, p);

Program.options.interface_alias = true;
G.Writeln();
G.Writeln("option interface alias = " + ("yes").ToString().ToLower() + "");

//[[commandEnd]]2


//[[commandStart]]3
p.SetText(@"¤29"); O.InitSmpl(smpl, p);

O.Assignment o3 = new O.Assignment();
O.AdjustT0(smpl, -1);
IVariable ivTmpvar3 = O.ListDefHelper(O.ListDefHelper(O.HandleString(new ScalarString(@"fy")), null, O.HandleString(new ScalarString(@"c[a]")), null), null, O.ListDefHelper(O.HandleString(new ScalarString(@"fe")), null, O.HandleString(new ScalarString(@"c[b]")), null), null);
O.AdjustT0(smpl, 1);
O.Lookup(smpl, null, "global", "#alias", null, ivTmpvar3, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
;

//[[commandEnd]]3


//[[commandStart]]4
p.SetText(@"¤30"); O.InitSmpl(smpl, p);

O.Assignment o4 = new O.Assignment();
O.AdjustT0(smpl, -1);
IVariable ivTmpvar4 = Functions.series(smpl, i5);
O.AdjustT0(smpl, 1);
O.Lookup(smpl, null, null, "c", null, ivTmpvar4, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
;

//[[commandEnd]]4


//[[commandStart]]5
p.SetText(@"¤30"); O.InitSmpl(smpl, p);

O.Assignment o5 = new O.Assignment();
O.AdjustT0(smpl, -1);
IVariable ivTmpvar6 = i7;
O.AdjustT0(smpl, 1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "c", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar6, o5, new ScalarString("a"))
;

//[[commandEnd]]5


//[[commandStart]]6
p.SetText(@"¤30"); O.InitSmpl(smpl, p);

O.Assignment o6 = new O.Assignment();
O.AdjustT0(smpl, -1);
IVariable ivTmpvar8 = i9;
O.AdjustT0(smpl, 1);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "c", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar8, o6, new ScalarString("b"))
;

//[[commandEnd]]6


//[[commandStart]]7
p.SetText(@"¤31"); O.InitSmpl(smpl, p);

Func<GraphHelper, string> print7 = (gh) =>
{
O.Prt o7 = new O.Prt();
labelCounter = 0;o7.guiGraphIsRefreshing = gh.isRefreshing;
o7.guiGraphPrintCode = gh.printCode;
o7.guiGraphIsLogTransform = gh.isLogTransform;
o7.prtType = "prt";
ESeriesMissing r1_7 = Program.options.series_array_print_missing; ESeriesMissing r2_7 = Program.options.series_normal_print_missing; try {
O.HandleOptionBankRef1(o7.opt_bank, o7.opt_ref); O.HandleMissing1(o7.opt_missing);
{
List<int> bankNumbers = null;
O.Prt.Element ope7 = new O.Prt.Element();
ope7.labelGiven = new List<string>() { "fy|[@130,725:726='fy',<1246>,31:4]|[@130,725:726='fy',<1246>,31:4]"};
smpl = new GekkoSmpl(o7.t1, o7.t2); smpl.t0 = smpl.t0.Add(-2);
ope7.printCodesFinal = Program.GetElementPrintCodes(o7, ope7);bankNumbers = O.Prt.GetBankNumbers(null, ope7.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope7.variable[bankNumber] = O.Lookup(smpl, null, null, "fy", null, null, new  LookupSettings(), EVariableType.Var, null);
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope7);
}
smpl.bankNumber = 0;
o7.prtElements.Add(ope7);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope7 = new O.Prt.Element();
ope7.labelGiven = new List<string>() { "fe|[@133,729:730='fe',<1246>,31:8]|[@133,729:730='fe',<1246>,31:8]"};
smpl = new GekkoSmpl(o7.t1, o7.t2); smpl.t0 = smpl.t0.Add(-2);
ope7.printCodesFinal = Program.GetElementPrintCodes(o7, ope7);bankNumbers = O.Prt.GetBankNumbers(null, ope7.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope7.variable[bankNumber] = O.Lookup(smpl, null, null, "fe", null, null, new  LookupSettings(), EVariableType.Var, null);
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope7);
}
smpl.bankNumber = 0;
o7.prtElements.Add(ope7);
}

}
finally {
O.HandleOptionBankRef2(); O.HandleMissing2(r1_7, r2_7);
}
o7.counter = 1;
o7.printCsCounter = Globals.printCs.Count - 1;
o7.Exe();
return o7.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print7); 
print7(new GraphHelper());

//[[commandEnd]]7


//[[commandStart]]8
p.SetText(@"¤32"); O.InitSmpl(smpl, p);

Func<GraphHelper, string> print8 = (gh) =>
{
O.Prt o8 = new O.Prt();
labelCounter = 0;o8.guiGraphIsRefreshing = gh.isRefreshing;
o8.guiGraphPrintCode = gh.printCode;
o8.guiGraphIsLogTransform = gh.isLogTransform;
o8.prtType = "prt";
ESeriesMissing r1_8 = Program.options.series_array_print_missing; ESeriesMissing r2_8 = Program.options.series_normal_print_missing; try {
O.HandleOptionBankRef1(o8.opt_bank, o8.opt_ref); O.HandleMissing1(o8.opt_missing);
{
List<int> bankNumbers = null;
O.Prt.Element ope8 = new O.Prt.Element();
ope8.labelGiven = new List<string>() { "c[_[a]|[@138,738:738='c',<1246>,32:4]|[@141,743:743=']',<1197>,32:9]"};
smpl = new GekkoSmpl(o8.t1, o8.t2); smpl.t0 = smpl.t0.Add(-2);
ope8.printCodesFinal = Program.GetElementPrintCodes(o8, ope8);bankNumbers = O.Prt.GetBankNumbers(null, ope8.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope8.variable[bankNumber] = O.Indexer(O.Indexer2(smpl, O.EIndexerType.None,new ScalarString("a")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "c", null, null, new  LookupSettings(), EVariableType.Var, null), O.ReportLabel(smpl, new ScalarString("a"), "a|[@140,742:742='a',<765>,32:8]|[@140,742:742='a',<765>,32:8]"));
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope8);
}
smpl.bankNumber = 0;
o8.prtElements.Add(ope8);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope8 = new O.Prt.Element();
ope8.labelGiven = new List<string>() { "c[_[b]|[@144,746:746='c',<1246>,32:12]|[@147,751:751=']',<1197>,32:17]"};
smpl = new GekkoSmpl(o8.t1, o8.t2); smpl.t0 = smpl.t0.Add(-2);
ope8.printCodesFinal = Program.GetElementPrintCodes(o8, ope8);bankNumbers = O.Prt.GetBankNumbers(null, ope8.printCodesFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope8.variable[bankNumber] = O.Indexer(O.Indexer2(smpl, O.EIndexerType.None,new ScalarString("b")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "c", null, null, new  LookupSettings(), EVariableType.Var, null), O.ReportLabel(smpl, new ScalarString("b"), "b|[@146,750:750='b',<1246>,32:16]|[@146,750:750='b',<1246>,32:16]"));
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope8);
}
smpl.bankNumber = 0;
o8.prtElements.Add(ope8);
}

}
finally {
O.HandleOptionBankRef2(); O.HandleMissing2(r1_8, r2_8);
}
o8.counter = 2;
o8.printCsCounter = Globals.printCs.Count - 1;
o8.Exe();
return o8.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print8); 
print8(new GraphHelper());

//[[commandEnd]]8


//[[commandStart]]9
p.SetText(@"¤42"); O.InitSmpl(smpl, p);

O.SheetImport o9 = new O.SheetImport();

o9.t1 = O.ConvertToDate(i10, O.GetDateChoices.FlexibleStart);
;
o9.t2 = O.ConvertToDate(i11, O.GetDateChoices.FlexibleEnd);
;

o9.opt_cell = O.ConvertToString(O.HandleString(new ScalarString(@"C5")));

//(new ScalarString("frits1"))
o9.names = O.ExplodeIvariablesSeq(new List(new List<IVariable> {(new ScalarString("xx1")), (new ScalarString("xx3"))}));
o9.Exe();

//[[commandEnd]]9
}


public static readonly ScalarVal i1 = new ScalarVal(2006d);
public static readonly ScalarVal i2 = new ScalarVal(2010d);
public static readonly ScalarVal i5 = new ScalarVal(1d);
public static readonly ScalarVal i7 = new ScalarVal(100d);
public static readonly ScalarVal i9 = new ScalarVal(200d);
public static readonly ScalarVal i10 = new ScalarVal(2001d);
public static readonly ScalarVal i11 = new ScalarVal(2002d);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
