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
public static readonly ScalarVal i1 = new ScalarVal(2000d);
public static readonly ScalarVal i2 = new ScalarVal(2003d);
public static readonly ScalarVal i4 = new ScalarVal(10d);
public static readonly ScalarVal i5 = new ScalarVal(11d);
public static readonly ScalarVal i6 = new ScalarVal(12d);
public static readonly ScalarVal i7 = new ScalarVal(13d);
public static readonly ScalarVal i8 = new ScalarVal(2001d);
public static readonly ScalarVal i9 = new ScalarVal(2003d);
public static readonly ScalarVal i11 = new ScalarVal(2001d);
public static readonly ScalarVal i12 = new ScalarVal(2003d);
public static readonly ScalarVal i14 = new ScalarVal(1d);
public static readonly ScalarVal i15 = new ScalarVal(2d);
public static readonly ScalarVal i16 = new ScalarVal(3d);
public static readonly ScalarVal i18 = new ScalarVal(4d);
public static readonly ScalarVal i19 = new ScalarVal(5d);
public static readonly ScalarVal i20 = new ScalarVal(6d);
public static readonly ScalarVal i22 = new ScalarVal(1d);
public static readonly ScalarVal i23 = new ScalarVal(2d);
public static readonly ScalarVal i24 = new ScalarVal(3d);
public static readonly ScalarVal i25 = new ScalarVal(4d);
public static readonly ScalarVal i26 = new ScalarVal(5d);
public static readonly ScalarVal i27 = new ScalarVal(6d);
public static readonly ScalarVal i28 = new ScalarVal(7d);
public static readonly ScalarVal i29 = new ScalarVal(8d);
public static readonly ScalarVal i30 = new ScalarVal(9d);
public static readonly ScalarVal i31 = new ScalarVal(10d);
public static readonly ScalarVal i32 = new ScalarVal(11d);
public static readonly ScalarVal i33 = new ScalarVal(12d);
public static readonly ScalarVal i35 = new ScalarVal(1d);
public static readonly ScalarVal i36 = new ScalarVal(2d);
public static readonly ScalarVal i37 = new ScalarVal(3d);
public static readonly ScalarVal i38 = new ScalarVal(4d);
public static readonly ScalarVal i39 = new ScalarVal(5d);
public static readonly ScalarVal i40 = new ScalarVal(6d);
public static readonly ScalarVal i41 = new ScalarVal(7d);
public static readonly ScalarVal i42 = new ScalarVal(8d);
public static readonly ScalarVal i43 = new ScalarVal(9d);
public static readonly ScalarVal i44 = new ScalarVal(10d);
public static readonly ScalarVal i45 = new ScalarVal(11d);
public static readonly ScalarVal i46 = new ScalarVal(12d);
public static readonly ScalarVal i47 = new ScalarVal(13d);
public static readonly ScalarVal i48 = new ScalarVal(14d);
public static readonly ScalarVal i49 = new ScalarVal(15d);
public static readonly ScalarVal i50 = new ScalarVal(16d);
public static readonly ScalarVal i51 = new ScalarVal(17d);
public static readonly ScalarVal i52 = new ScalarVal(18d);
public static readonly ScalarVal i53 = new ScalarVal(19d);
public static readonly ScalarVal i54 = new ScalarVal(20d);
public static readonly ScalarVal i55 = new ScalarVal(21d);
public static readonly ScalarVal i56 = new ScalarVal(22d);
public static readonly ScalarVal i57 = new ScalarVal(23d);
public static readonly ScalarVal i58 = new ScalarVal(24d);
public static readonly ScalarVal i59 = new ScalarVal(25d);
public static readonly ScalarVal i60 = new ScalarVal(26d);
public static readonly ScalarVal i61 = new ScalarVal(27d);
public static readonly ScalarVal i62 = new ScalarVal(18d);
public static readonly ScalarVal i63 = new ScalarVal(29d);
public static readonly ScalarVal i64 = new ScalarVal(30d);
public static readonly ScalarVal i65 = new ScalarVal(31d);
public static readonly ScalarVal i66 = new ScalarVal(32d);
public static readonly ScalarVal i67 = new ScalarVal(33d);
public static readonly ScalarVal i68 = new ScalarVal(34d);
public static readonly ScalarVal i69 = new ScalarVal(35d);
public static readonly ScalarVal i70 = new ScalarVal(36d);
public static readonly ScalarVal i73 = new ScalarVal(1000d);
public static readonly ScalarVal i75 = new ScalarVal(1000d);
public static readonly ScalarVal i77 = new ScalarVal(1000d);
public static readonly ScalarVal i79 = new ScalarVal(1000d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl);
O.Reset o0 = new O.Reset();
o0.p = p;o0.Exe(smpl);

p.SetText(@"¤2"); O.InitSmpl(smpl);
O.Time o1 = new O.Time();
o1.t1 = O.ConvertToDate(i1, O.GetDateChoices.FlexibleStart);
;
o1.t2 = O.ConvertToDate(i2, O.GetDateChoices.FlexibleEnd);
;

o1.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar3 = O.ListDefHelper(i4, i5, i6, i7);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar3, true)
;

p.SetText(@"¤4"); O.InitSmpl(smpl);
O.Time o3 = new O.Time();
o3.t1 = O.ConvertToDate(i8, O.GetDateChoices.FlexibleStart);
;
o3.t2 = O.ConvertToDate(i9, O.GetDateChoices.FlexibleEnd);
;

o3.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar10 = O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"xx", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"xx", true, false)));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar10, true)
;

p.SetText(@"¤7"); O.InitSmpl(smpl);

//[[splitSTOP]]
return;

//[[splitSTART]]

p.SetText(@"¤0"); O.InitSmpl(smpl);
O.Reset o6 = new O.Reset();
o6.p = p;o6.Exe(smpl);

p.SetText(@"¤10"); O.InitSmpl(smpl);
O.Time o7 = new O.Time();
o7.t1 = O.ConvertToDate(i11, O.GetDateChoices.FlexibleStart);
;
o7.t2 = O.ConvertToDate(i12, O.GetDateChoices.FlexibleEnd);
;

o7.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar13 = O.ListDefHelper(i14, i15, i16);
O.Lookup(smpl, null, null, "xx1", null, ivTmpvar13, true)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar17 = O.ListDefHelper(i18, i19, i20);
O.Lookup(smpl, null, null, "xx2", null, ivTmpvar17, true)
;

p.SetText(@"¤13"); O.InitSmpl(smpl);
Program.options.freq = G.GetFreq("q");
G.Writeln();
G.Writeln("option freq = " + G.GetFreq("q") + "");
ClearTS(p);
Program.AdjustFreq();
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar21 = O.ListDefHelper(i22, i23, i24, i25, i26, i27, i28, i29, i30, i31, i32, i33);
O.Lookup(smpl, null, null, "xx3", null, ivTmpvar21, true)
;

p.SetText(@"¤15"); O.InitSmpl(smpl);
Program.options.freq = G.GetFreq("m");
G.Writeln();
G.Writeln("option freq = " + G.GetFreq("m") + "");
ClearTS(p);
Program.AdjustFreq();
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar34 = O.ListDefHelper(i35, i36, i37, i38, i39, i40, i41, i42, i43, i44, i45, i46, i47, i48, i49, i50, i51, i52, i53, i54, i55, i56, i57, i58, i59, i60, i61, i62, i63, i64, i65, i66, i67, i68, i69, i70);
O.Lookup(smpl, null, null, "xx4", null, ivTmpvar34, true)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar71 = O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"xx3!q", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"xx4!m", true, false)));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar71, true)
;

p.SetText(@"¤20"); O.InitSmpl(smpl);
O.Write o15 = new O.Write();

o15.fileName = O.ConvertToString((new ScalarString("slet", true, false)));

o15.type = @"write";o15.Exe();

p.SetText(@"¤21"); O.InitSmpl(smpl);
ClearTS(p);
O.Read o16 = new O.Read();
o16.p = p;
o16.type = @"read";
o16.fileName = O.ConvertToString((new ScalarString("slet", true, false)));


o16.Exe();

p.SetText(@"¤22"); O.InitSmpl(smpl);
Program.options.freq = G.GetFreq("a");
G.Writeln();
G.Writeln("option freq = " + G.GetFreq("a") + "");
ClearTS(p);
Program.AdjustFreq();
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar72 = O.Add(smpl, O.Lookup(smpl, null, null, "xx1", null, null, false), i73);
O.Lookup(smpl, null, null, "xx1", null, ivTmpvar72, true)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar74 = O.Add(smpl, O.Lookup(smpl, null, null, "xx2", null, null, false), i75);
O.Lookup(smpl, null, null, "xx2", null, ivTmpvar74, true)
;

p.SetText(@"¤25"); O.InitSmpl(smpl);
Program.options.freq = G.GetFreq("q");
G.Writeln();
G.Writeln("option freq = " + G.GetFreq("q") + "");
ClearTS(p);
Program.AdjustFreq();
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar76 = O.Add(smpl, O.Lookup(smpl, null, null, "xx3", null, null, false), i77);
O.Lookup(smpl, null, null, "xx3", null, ivTmpvar76, true)
;

p.SetText(@"¤27"); O.InitSmpl(smpl);
Program.options.freq = G.GetFreq("m");
G.Writeln();
G.Writeln("option freq = " + G.GetFreq("m") + "");
ClearTS(p);
Program.AdjustFreq();
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar78 = O.Add(smpl, O.Lookup(smpl, null, null, "xx4", null, null, false), i79);
O.Lookup(smpl, null, null, "xx4", null, ivTmpvar78, true)
;

p.SetText(@"¤29"); O.InitSmpl(smpl);
Program.options.freq = G.GetFreq("a");
G.Writeln();
G.Writeln("option freq = " + G.GetFreq("a") + "");
ClearTS(p);
Program.AdjustFreq();
p.SetText(@"¤30"); O.InitSmpl(smpl);
O.Prt o25 = new O.Prt();
o25.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope25 = new O.Prt.Element();
ope25.label = O.SubstituteScalarsAndLists("(xx1,{#m})", false);
smpl = new GekkoSmpl(o25.t1.Add(-2), o25.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o25, ope25));
foreach(int bankNumber in bankNumbers) {
ope25.subElements = new List<O.Prt.SubElement>();
ope25.subElements.Add(new O.Prt.SubElement());
ope25.subElements[0].tsWork = O.ListDefHelper(O.Lookup(smpl, null, null, "xx1", null, null, false), O.Lookup(smpl, null, (O.Lookup(smpl, null, null, "#m", null, null, false)), null, false));
}
o25.prtElements.Add(ope25);
}
smpl = null;

o25.counter = 1;
o25.Exe();


//[[splitSTOP]]


}
}
}
