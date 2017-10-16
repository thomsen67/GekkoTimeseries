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
public static GekkoTime globalGekkoTimeIterator = Globals.tNull;
public static readonly ScalarVal i3 = new ScalarVal(100d);
public static IVariable MapDef_mapTmpvar11(GekkoSmpl smpl) {
Map mapTmpvar11 = new Map();
IVariable ivTmpvar12 = new ScalarString(@"b");
for (int iSmpl13 = 0; iSmpl13 < int.MaxValue; iSmpl13++) {
O.Lookup(smpl, mapTmpvar11, null, "%i1", null, ivTmpvar12, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl13); else break;
};

IVariable ivTmpvar14 = new ScalarString(@"c");
for (int iSmpl15 = 0; iSmpl15 < int.MaxValue; iSmpl15++) {
O.Lookup(smpl, mapTmpvar11, null, "%i2", null, ivTmpvar14, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl15); else break;
};


return mapTmpvar11;
}public static IVariable MapDef_mapTmpvar6(GekkoSmpl smpl) {
            Map mapTmpvar6 = new Map();
IVariable ivTmpvar7 = new ScalarString(@"a");
for (int iSmpl8 = 0; iSmpl8 < int.MaxValue; iSmpl8++) {
O.Lookup(smpl, mapTmpvar6, null, "%i1", null, ivTmpvar7, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl8); else break;
};

IVariable ivTmpvar9 = MapDef_mapTmpvar11(smpl);
for (int iSmpl10 = 0; iSmpl10 < int.MaxValue; iSmpl10++) {
O.Lookup(smpl, mapTmpvar6, null, "#m", null, ivTmpvar9, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl10); else break;
};


return mapTmpvar6;
}public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = O.Smpl();


p.SetText(@"¤0");
O.Reset o0 = new O.Reset();
o0.p = p;o0.Exe();




p.SetText(@"¤0");
IVariable ivTmpvar1 = i3;
for (int iSmpl2 = 0; iSmpl2 < int.MaxValue; iSmpl2++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar1, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl2); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar4 = MapDef_mapTmpvar6(smpl);
for (int iSmpl5 = 0; iSmpl5 < int.MaxValue; iSmpl5++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar4, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl5); else break;
};




p.SetText(@"¤0");
for (int iSmpl16 = 0; iSmpl16 < int.MaxValue; iSmpl16++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl16); else break;
}



p.SetText(@"¤0");
for (int iSmpl17 = 0; iSmpl17 < int.MaxValue; iSmpl17++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl17); else break;
}



p.SetText(@"¤0");
for (int iSmpl18 = 0; iSmpl18 < int.MaxValue; iSmpl18++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl18); else break;
}



p.SetText(@"¤43");
O.Write o6 = new O.Write();

o6.fileName = O.ConvertToString((new ScalarString("slet", true, false)));

o6.type = @"write";o6.Exe();




p.SetText(@"¤0");
O.Reset o7 = new O.Reset();
o7.p = p;o7.Exe();




p.SetText(@"¤45");
ClearTS(p);
O.Read o8 = new O.Read();
o8.p = p;
o8.type = @"read";
o8.fileName = O.ConvertToString((new ScalarString("slet", true, false)));


o8.Exe();




}

public static void C1(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
for (int iSmpl19 = 0; iSmpl19 < int.MaxValue; iSmpl19++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl19); else break;
}



p.SetText(@"¤0");
for (int iSmpl20 = 0; iSmpl20 < int.MaxValue; iSmpl20++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl20); else break;
}



p.SetText(@"¤0");
for (int iSmpl21 = 0; iSmpl21 < int.MaxValue; iSmpl21++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl21); else break;
}



p.SetText(@"¤0");
for (int iSmpl22 = 0; iSmpl22 < int.MaxValue; iSmpl22++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl22); else break;
}



}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);

C1(p);



}
}
}
