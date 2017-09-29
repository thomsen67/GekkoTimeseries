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
public static readonly ScalarVal i2 = new ScalarVal(100d);
public static readonly ScalarVal i4 = new ScalarVal(2010d);
public static readonly ScalarVal i5 = new ScalarVal(1d);
public static readonly ScalarVal i7 = new ScalarVal(1d);
public static readonly ScalarVal i9 = new ScalarVal(1d);
public static readonly ScalarVal i11 = new ScalarVal(5d);
public static readonly ScalarVal i12 = new ScalarVal(5d);
public static readonly ScalarVal i13 = new ScalarVal(2d);
public static readonly ScalarVal i15 = new ScalarVal(1d);
public static readonly ScalarVal i17 = new ScalarVal(5d);
public static readonly ScalarVal i18 = new ScalarVal(6d);
public static readonly ScalarVal i19 = new ScalarVal(3d);
public static readonly ScalarVal i21 = new ScalarVal(1d);
public static readonly ScalarVal i23 = new ScalarVal(2010d);
public static readonly ScalarVal i24 = new ScalarVal(5d);
public static readonly ScalarVal i25 = new ScalarVal(5d);
public static readonly ScalarVal i26 = new ScalarVal(2d);
public static readonly ScalarVal i28 = new ScalarVal(1d);
public static readonly ScalarVal i30 = new ScalarVal(2010d);
public static readonly ScalarVal i31 = new ScalarVal(5d);
public static readonly ScalarVal i32 = new ScalarVal(6d);
public static readonly ScalarVal i33 = new ScalarVal(3d);
public static readonly ScalarVal i34 = new ScalarVal(1d);
public static readonly ScalarVal i35 = new ScalarVal(1d);
public static readonly ScalarVal i37 = new ScalarVal(0d);
public static readonly ScalarVal i39 = new ScalarVal(2d);
public static readonly ScalarVal i41 = new ScalarVal(0d);
public static readonly ScalarVal i43 = new ScalarVal(1d);
public static readonly ScalarVal i44 = new ScalarVal(1d);
public static readonly ScalarVal i45 = new ScalarVal(0d);
public static readonly ScalarVal i47 = new ScalarVal(0d);
public static readonly ScalarVal i49 = new ScalarVal(2d);
public static readonly ScalarVal i51 = new ScalarVal(0d);
public static readonly ScalarVal i53 = new ScalarVal(1d);
public static readonly ScalarVal i55 = new ScalarVal(1d);
public static readonly ScalarVal i56 = new ScalarVal(0d);
public static readonly ScalarVal i57 = new ScalarVal(1d);
public static readonly ScalarVal i59 = new ScalarVal(1d);
public static readonly ScalarVal i60 = new ScalarVal(2d);
public static readonly ScalarVal i61 = new ScalarVal(3d);
public static readonly ScalarVal i62 = new ScalarVal(4d);
public static readonly ScalarVal i63 = new ScalarVal(5d);
public static readonly ScalarVal i64 = new ScalarVal(6d);
public static readonly ScalarVal i65 = new ScalarVal(7d);
public static readonly ScalarVal i66 = new ScalarVal(5d);
public static readonly ScalarVal i67 = new ScalarVal(5d);
public static readonly ScalarVal i68 = new ScalarVal(10d);
public static readonly ScalarVal i69 = new ScalarVal(5d);
public static readonly ScalarVal i71 = new ScalarVal(100d);
public static readonly ScalarVal i72 = new ScalarVal(5d);
public static readonly ScalarVal i74 = new ScalarVal(1d);
public static readonly ScalarVal i75 = new ScalarVal(2d);
public static readonly ScalarVal i76 = new ScalarVal(3d);
public static readonly ScalarVal i77 = new ScalarVal(4d);
public static readonly ScalarVal i78 = new ScalarVal(5d);
public static readonly ScalarVal i79 = new ScalarVal(6d);
public static readonly ScalarVal i80 = new ScalarVal(7d);
public static readonly ScalarVal i81 = new ScalarVal(5d);
public static readonly ScalarVal i82 = new ScalarVal(5d);
public static readonly ScalarVal i83 = new ScalarVal(10d);
public static readonly ScalarVal i84 = new ScalarVal(5d);
public static readonly ScalarVal i86 = new ScalarVal(101d);
public static readonly ScalarVal i87 = new ScalarVal(102d);
public static readonly ScalarVal i88 = new ScalarVal(103d);
public static readonly ScalarVal i89 = new ScalarVal(104d);
public static readonly ScalarVal i90 = new ScalarVal(105d);
public static readonly ScalarVal i91 = new ScalarVal(106d);
public static readonly ScalarVal i92 = new ScalarVal(107d);
public static readonly ScalarVal i93 = new ScalarVal(108d);
public static readonly ScalarVal i94 = new ScalarVal(109d);
public static readonly ScalarVal i95 = new ScalarVal(110d);
public static readonly ScalarVal i96 = new ScalarVal(111d);
public static readonly ScalarVal i98 = new ScalarVal(5d);
public static readonly ScalarVal i100 = new ScalarVal(1d);
public static readonly ScalarVal i101 = new ScalarVal(2d);
public static readonly ScalarVal i102 = new ScalarVal(3d);
public static readonly ScalarVal i103 = new ScalarVal(4d);
public static readonly ScalarVal i104 = new ScalarVal(5d);
public static readonly ScalarVal i105 = new ScalarVal(6d);
public static readonly ScalarVal i106 = new ScalarVal(7d);
public static readonly ScalarVal i107 = new ScalarVal(5d);
public static readonly ScalarVal i108 = new ScalarVal(5d);
public static readonly ScalarVal i109 = new ScalarVal(10d);
public static readonly ScalarVal i110 = new ScalarVal(5d);
public static readonly ScalarVal i112 = new ScalarVal(1d);
public static readonly ScalarVal i113 = new ScalarVal(2d);
public static readonly ScalarVal i114 = new ScalarVal(3d);
public static readonly ScalarVal i115 = new ScalarVal(4d);
public static readonly ScalarVal i117 = new ScalarVal(1d);
public static readonly ScalarVal i118 = new ScalarVal(2d);
public static readonly ScalarVal i119 = new ScalarVal(100d);
public static readonly ScalarVal i120 = new ScalarVal(200d);
public static readonly ScalarVal i122 = new ScalarVal(1d);
public static readonly ScalarVal i123 = new ScalarVal(2d);
public static readonly ScalarVal i124 = new ScalarVal(3d);
public static readonly ScalarVal i125 = new ScalarVal(4d);
public static readonly ScalarVal i127 = new ScalarVal(1d);
public static readonly ScalarVal i128 = new ScalarVal(2d);
public static readonly ScalarVal i129 = new ScalarVal(1d);
public static readonly ScalarVal i130 = new ScalarVal(1d);
public static readonly ScalarVal i131 = new ScalarVal(2d);
public static readonly ScalarVal i132 = new ScalarVal(2d);
public static readonly ScalarVal i134 = new ScalarVal(1d);
public static readonly ScalarVal i135 = new ScalarVal(2d);
public static readonly ScalarVal i136 = new ScalarVal(3d);
public static readonly ScalarVal i137 = new ScalarVal(2d);
public static readonly ScalarVal i138 = new ScalarVal(3d);
public static readonly ScalarVal i140 = new ScalarVal(2d);
public static readonly ScalarVal i141 = new ScalarVal(3d);
public static readonly ScalarVal i142 = new ScalarVal(20d);
public static readonly ScalarVal i143 = new ScalarVal(30d);
public static readonly ScalarVal i149 = new ScalarVal(100d);
public static readonly ScalarVal i150 = new ScalarVal(1d);
public static IVariable MapDef_mapTmpvar146(GekkoSmpl smpl) {
Map mapTmpvar146 = new Map();
IVariable ivTmpvar147 = new ScalarString(@"a");
O.Lookup(smpl, mapTmpvar146, null, "%i1", null, ivTmpvar147)
;

IVariable ivTmpvar148 = O.Add(smpl, i149, i150);
O.Lookup(smpl, mapTmpvar146, null, "%v", null, ivTmpvar148)
;


return mapTmpvar146;
}public static IVariable MapDef_mapTmpvar156(GekkoSmpl smpl) {
Map mapTmpvar156 = new Map();
IVariable ivTmpvar157 = new ScalarString(@"b");
O.Lookup(smpl, mapTmpvar156, null, "%i1", null, ivTmpvar157)
;

IVariable ivTmpvar158 = new ScalarString(@"c");
O.Lookup(smpl, mapTmpvar156, null, "%i2", null, ivTmpvar158)
;


return mapTmpvar156;
}public static IVariable MapDef_mapTmpvar153(GekkoSmpl smpl) {
Map mapTmpvar153 = new Map();
IVariable ivTmpvar154 = new ScalarString(@"a");
O.Lookup(smpl, mapTmpvar153, null, "%i1", null, ivTmpvar154)
;

IVariable ivTmpvar155 = MapDef_mapTmpvar156(smpl);
O.Lookup(smpl, mapTmpvar153, null, "#m", null, ivTmpvar155)
;


return mapTmpvar153;
}public static readonly ScalarVal i160 = new ScalarVal(1d);
public static readonly ScalarVal i161 = new ScalarVal(2d);
public static readonly ScalarVal i162 = new ScalarVal(3d);
public static readonly ScalarVal i163 = new ScalarVal(4d);
public static readonly ScalarVal i164 = new ScalarVal(1d);
public static readonly ScalarVal i165 = new ScalarVal(2d);
public static readonly ScalarVal i167 = new ScalarVal(2d);
public static readonly ScalarVal i168 = new ScalarVal(1d);
public static readonly ScalarVal i169 = new ScalarVal(1d);
public static readonly ScalarVal i170 = new ScalarVal(10d);
public static readonly ScalarVal i171 = new ScalarVal(2d);
public static readonly ScalarVal i172 = new ScalarVal(2d);
public static readonly ScalarVal i173 = new ScalarVal(1d);
public static readonly ScalarVal i174 = new ScalarVal(1d);
public static readonly ScalarVal i177 = new ScalarVal(1d);
public static readonly ScalarVal i178 = new ScalarVal(2d);
public static readonly ScalarVal i179 = new ScalarVal(1d);
public static readonly ScalarVal i180 = new ScalarVal(2d);
public static readonly ScalarVal i190 = new ScalarVal(0d);
public static readonly ScalarVal i192 = new ScalarVal(1d);
public static readonly ScalarVal d193 = new ScalarVal(1e3d);
public static readonly ScalarVal i197 = new ScalarVal(0d);
public static readonly ScalarVal i199 = new ScalarVal(1d);
public static readonly ScalarVal d200 = new ScalarVal(1e3d);
public static readonly ScalarVal i204 = new ScalarVal(1d);
public static void FunctionDef205() {


//[[splitSTOP]]

Globals.ufunctions1.Add("add1", (GekkoSmpl smpl, IVariable functionarg_203) => { 
//[[splitSTOP]]
return O.Add(smpl, functionarg_203, i204);

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i207 = new ScalarVal(0d);
public static readonly ScalarVal i209 = new ScalarVal(1d);
public static readonly ScalarVal d210 = new ScalarVal(1e3d);
public static void FunctionDef215() {


//[[splitSTOP]]

Globals.ufunctions2.Add("add2", (GekkoSmpl smpl, IVariable functionarg_213, IVariable functionarg_214) => { 
//[[splitSTOP]]
return O.ListDef(functionarg_213, O.Add(smpl, functionarg_213, functionarg_214));

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i216 = new ScalarVal(10d);
public static readonly ScalarVal i217 = new ScalarVal(20d);
public static readonly ScalarVal i218 = new ScalarVal(2d);
public static void FunctionDef223() {


//[[splitSTOP]]

Globals.ufunctions1.Add("f", (GekkoSmpl smpl, IVariable functionarg_222) => { 
//[[splitSTOP]]
return O.Add(smpl, functionarg_222, functionarg_222);

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i225 = new ScalarVal(100d);
public static readonly ScalarVal i228 = new ScalarVal(12345d);
public static readonly ScalarVal i230 = new ScalarVal(1d);
public static readonly ScalarVal i232 = new ScalarVal(2d);
public static readonly ScalarVal i234 = new ScalarVal(3d);
public static readonly ScalarVal i236 = new ScalarVal(4d);
public static readonly ScalarVal i238 = new ScalarVal(2010d);
public static readonly ScalarVal i239 = new ScalarVal(1000d);
public static IVariable temp244(GekkoSmpl smpl) {
TimeSeries temp244 = new TimeSeries(Program.options.freq, null); temp244.SetZero(smpl);

foreach (IVariable listloop_m1242 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null))) {
foreach (IVariable listloop_m2243 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null))) {
temp244.InjectAdd(smpl, temp244, O.Indexer(smpl, O.Lookup(smpl, null, null, "xx", null, null), listloop_m1242, listloop_m2243));

}
}
return temp244;

}
public static readonly ScalarVal i247 = new ScalarVal(0d);
public static IVariable temp248(GekkoSmpl smpl) {
MetaList temp248 = new MetaList();

foreach (IVariable listloop_m1245 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null))) {
foreach (IVariable listloop_m2246 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null))) {
temp248.Add(O.Add(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "xx", null, null), listloop_m1245, listloop_m2246), i247));

}
}
return temp248;

}
public static readonly ScalarVal i250 = new ScalarVal(1d);
public static readonly ScalarVal i252 = new ScalarVal(2d);
public static readonly ScalarVal i254 = new ScalarVal(3d);
public static readonly ScalarVal i256 = new ScalarVal(4d);
public static readonly ScalarVal i258 = new ScalarVal(2010d);
public static readonly ScalarVal i259 = new ScalarVal(1000d);
public static IVariable temp264(GekkoSmpl smpl) {
TimeSeries temp264 = new TimeSeries(Program.options.freq, null); temp264.SetZero(smpl);

foreach (IVariable listloop_m1262 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null))) {
foreach (IVariable listloop_m2263 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null))) {
temp264.InjectAdd(smpl, temp264, O.Lookup(smpl, null, (new ScalarString("xx", true, false)).Add(smpl, listloop_m1262).Add(smpl, listloop_m2263), null));

}
}
return temp264;

}
public static readonly ScalarVal i267 = new ScalarVal(0d);
public static IVariable temp268(GekkoSmpl smpl) {
MetaList temp268 = new MetaList();

foreach (IVariable listloop_m1265 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null))) {
foreach (IVariable listloop_m2266 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null))) {
temp268.Add(O.Add(smpl, O.Lookup(smpl, null, (new ScalarString("xx", true, false)).Add(smpl, listloop_m1265).Add(smpl, listloop_m2266), null), i267));

}
}
return temp268;

}
public static readonly ScalarVal i271 = new ScalarVal(0d);
public static IVariable temp272(GekkoSmpl smpl) {
MetaList temp272 = new MetaList();

foreach (IVariable listloop_m270 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), null))) {
temp272.Add(O.Add(smpl, O.Lookup(smpl, null, (listloop_m270), null), i271));

}
return temp272;

}
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = O.Smpl();


p.SetText(@"¤0");
O.Reset o0 = new O.Reset();
o0.p = p;o0.Exe();




p.SetText(@"¤0");
IVariable ivTmpvar1 = i2;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar1)
;




p.SetText(@"¤0");
IVariable ivTmpvar3 = i5;
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null),  ivTmpvar3, i4
)
;




p.SetText(@"¤0");
IVariable ivTmpvar6 = i7;
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null),  ivTmpvar6, new ScalarDate(G.FromStringToDate("2012a1"))
)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar8 = i9;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar8)
;




p.SetText(@"¤0");
IVariable ivTmpvar10 = i13;
O.DollarLookup(O.Equals(smpl, i11,i12)
, smpl, null, null, "xx", null, ivTmpvar10)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar14 = i15;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar14)
;




}

public static void C1(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar16 = i19;
O.DollarLookup(O.Equals(smpl, i17,i18)
, smpl, null, null, "xx", null, ivTmpvar16)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar20 = i21;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar20)
;




p.SetText(@"¤0");
IVariable ivTmpvar22 = i26;
O.DollarIndexerSetData(O.Equals(smpl, i24,i25)
, smpl, O.Lookup(smpl, null, null, "xx", null, null),  ivTmpvar22, i23
)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar27 = i28;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar27)
;




p.SetText(@"¤0");
IVariable ivTmpvar29 = i33;
O.DollarIndexerSetData(O.Equals(smpl, i31,i32)
, smpl, O.Lookup(smpl, null, null, "xx", null, null),  ivTmpvar29, i30
)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null)));



p.SetText(@"¤52");

}

public static void C2(P p) {

GekkoSmpl smpl = O.Smpl();

IVariable ivTmpvar36 = i37;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar36)
;

IVariable ivTmpvar38 = i39;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar38)
;


}

public static void C3(P p) {

GekkoSmpl smpl = O.Smpl();

IVariable ivTmpvar40 = i41;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar40)
;

IVariable ivTmpvar42 = i43;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar42)
;


}

public static void C4(P p) {

GekkoSmpl smpl = O.Smpl();




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null)));



p.SetText(@"¤55");

}

public static void C5(P p) {

GekkoSmpl smpl = O.Smpl();

IVariable ivTmpvar46 = i47;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar46)
;

IVariable ivTmpvar48 = i49;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar48)
;


}

public static void C6(P p) {

GekkoSmpl smpl = O.Smpl();

IVariable ivTmpvar50 = i51;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar50)
;

IVariable ivTmpvar52 = i53;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar52)
;


}

public static void C7(P p) {

GekkoSmpl smpl = O.Smpl();




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null)));




}

public static void C8(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar54 = O.Dollar(smpl, i55, O.Equals(smpl, i56,i57)
);
O.Lookup(smpl, null, null, "%v", null, ivTmpvar54)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "%v", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar58 = O.ListDef(i59, i60, i61, i62, i63, i64, i65, i66, i67, i68, i69);
O.Lookup(smpl, null, null, "xx1", null, ivTmpvar58)
;




p.SetText(@"¤0");
IVariable ivTmpvar70 = O.Dollar(smpl, i71, O.Equals(smpl, O.Lookup(smpl, null, null, "xx1", null, null),i72)
);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar70)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar73 = O.ListDef(i74, i75, i76, i77, i78, i79, i80, i81, i82, i83, i84);
O.Lookup(smpl, null, null, "xx1", null, ivTmpvar73)
;




p.SetText(@"¤0");
IVariable ivTmpvar85 = O.ListDef(i86, i87, i88, i89, i90, i91, i92, i93, i94, i95, i96);
O.Lookup(smpl, null, null, "xx2", null, ivTmpvar85)
;




p.SetText(@"¤0");
IVariable ivTmpvar97 = O.Dollar(smpl, O.Lookup(smpl, null, null, "xx2", null, null), O.Equals(smpl, O.Lookup(smpl, null, null, "xx1", null, null),i98)
);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar97)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar99 = O.MatrixCol(O.MatrixRow(i100), O.MatrixRow(i101), O.MatrixRow(i102), O.MatrixRow(i103), O.MatrixRow(i104), O.MatrixRow(i105), O.MatrixRow(i106), O.MatrixRow(i107), O.MatrixRow(i108), O.MatrixRow(i109), O.MatrixRow(i110));
O.Lookup(smpl, null, null, "xx", null, ivTmpvar99)
;




}

public static void C9(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar111 = O.MatrixCol(O.MatrixRow(i112, i113), O.MatrixRow(i114, i115));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar111)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "#m", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar116 = O.MatrixCol(O.MatrixRow(i119), O.MatrixRow(i120));
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null),  ivTmpvar116, (new Range(i117, i118)))
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "#m", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar121 = O.MatrixCol(O.MatrixRow(i122, i123), O.MatrixRow(i124, i125));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar121)
;




p.SetText(@"¤0");
IVariable ivTmpvar126 = O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), (new Range(i130, i131)), i132
);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null),  ivTmpvar126, (new Range(i127, i128)), i129
)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "#m", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar133 = O.MatrixCol(O.MatrixRow(i134), O.MatrixRow(i135), O.MatrixRow(i136));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar133)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), (new Range(i137, i138)))));




}

public static void C10(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar139 = O.MatrixCol(O.MatrixRow(i142), O.MatrixRow(i143));
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null),  ivTmpvar139, (new Range(i140, i141)))
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "#m", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar144 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar144)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), new ScalarString(@"a")
)));




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), new ScalarString(@"c")
)));




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), new ScalarString(@"a*")
)));




p.SetText(@"¤0");
IVariable ivTmpvar145 = MapDef_mapTmpvar146(smpl);
O.Lookup(smpl, null, null, "#m", null, ivTmpvar145)
;




p.SetText(@"¤0");
IVariable ivTmpvar151 = new ScalarString(@"b");
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null),  ivTmpvar151, (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))))
;




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))))));




}

public static void C11(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), (O.scalarStringPercent).Add(smpl, (new ScalarString("v", true, false))))));




p.SetText(@"¤0");
IVariable ivTmpvar152 = MapDef_mapTmpvar153(smpl);
O.Lookup(smpl, null, null, "#m", null, ivTmpvar152)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))))));




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), new ScalarString(@"%i1")
)));




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), new ScalarString(@"#m")
), new ScalarString(@"%i1")
)));




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), new ScalarString(@"#m")
), new ScalarString(@"%i2")
)));




p.SetText(@"¤0");
IVariable ivTmpvar159 = O.ListDef(new ScalarString(@"a"), O.MatrixCol(O.MatrixRow(i160, i161), O.MatrixRow(i162, i163)));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar159)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), i164
)));




}

public static void C12(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), i165
)));




p.SetText(@"¤0");
IVariable ivTmpvar166 = i170;
O.IndexerSetData(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), i167
),  ivTmpvar166, i168
, i169
)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), i171
)));




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), i172
), i173
, i174
)));




p.SetText(@"¤0");
IVariable ivTmpvar175 = O.ListDef(O.ListDef(new ScalarString(@"a"), new ScalarString(@"b")), new ScalarString(@"c"));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar175)
;




p.SetText(@"¤0");
IVariable ivTmpvar176 = new ScalarString(@"x");
O.IndexerSetData(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), i177
),  ivTmpvar176, i178
)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null), i179
), i180
)));




p.SetText(@"¤0");
IVariable ivTmpvar181 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"));
O.Lookup(smpl, null, null, "#m1", null, ivTmpvar181)
;




p.SetText(@"¤0");
IVariable ivTmpvar182 = O.ListDef(new ScalarString(@"x"), new ScalarString(@"y"));
O.Lookup(smpl, null, null, "#m2", null, ivTmpvar182)
;




p.SetText(@"¤0");

}

public static void C13(P p) {

GekkoSmpl smpl = O.Smpl();





}

public static void C14(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar189 = i190;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar189)
;




p.SetText(@"¤0");

}

public static void C15(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar196 = i197;
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar196)
;




p.SetText(@"¤0");

}

public static void C16(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "%sum", null, null)));




p.SetText(@"¤136");
FunctionDef205();





p.SetText(@"¤0");
IVariable ivTmpvar206 = i207;
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar206)
;




p.SetText(@"¤0");

}

public static void C17(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "%sum", null, null)));




}

public static void C18(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤143");
FunctionDef215();





p.SetText(@"¤144");
O.Print(smpl, (O.Indexer(smpl, Globals.ufunctions2["add2"](smpl, i216, i217), i218
)));




p.SetText(@"¤0");
IVariable ivTmpvar219 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"), new ScalarString(@"c"));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar219)
;




p.SetText(@"¤0");

}

public static void C19(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤152");
FunctionDef223();





p.SetText(@"¤0");
IVariable ivTmpvar224 = i225;
O.Lookup(smpl, null, null, "%v1", null, ivTmpvar224)
;




p.SetText(@"¤0");
IVariable ivTmpvar226 = Globals.ufunctions1["f"](smpl, Globals.ufunctions1["f"](smpl, O.Lookup(smpl, null, null, "%v1", null, null)));
O.Lookup(smpl, null, null, "%v2", null, ivTmpvar226)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "%v2", null, null)));




p.SetText(@"¤0");
IVariable ivTmpvar227 = O.Negate(smpl, i228);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar227)
;




p.SetText(@"¤0");
IVariable ivTmpvar229 = i230;
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null),  ivTmpvar229, new ScalarString(@"a")
, new ScalarString(@"x")
)
;




}

public static void C20(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar231 = i232;
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null),  ivTmpvar231, new ScalarString(@"b")
, new ScalarString(@"x")
)
;




p.SetText(@"¤0");
IVariable ivTmpvar233 = i234;
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null),  ivTmpvar233, new ScalarString(@"a")
, new ScalarString(@"y")
)
;




p.SetText(@"¤0");
IVariable ivTmpvar235 = i236;
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null),  ivTmpvar235, new ScalarString(@"b")
, new ScalarString(@"y")
)
;




p.SetText(@"¤0");
IVariable ivTmpvar237 = i239;
O.IndexerSetData(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "xx", null, null), new ScalarString(@"b")
, new ScalarString(@"y")
),  ivTmpvar237, i238
)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "xx", null, null), new ScalarString(@"b")
, new ScalarString(@"x")
)));




p.SetText(@"¤0");
IVariable ivTmpvar240 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"));
O.Lookup(smpl, null, null, "#m1", null, ivTmpvar240)
;




p.SetText(@"¤0");
IVariable ivTmpvar241 = O.ListDef(new ScalarString(@"x"), new ScalarString(@"y"));
O.Lookup(smpl, null, null, "#m2", null, ivTmpvar241)
;




p.SetText(@"¤171");
O.Print(smpl, (temp244(smpl)));




p.SetText(@"¤172");
O.Print(smpl, (temp248(smpl)));




p.SetText(@"¤0");
IVariable ivTmpvar249 = i250;
O.Lookup(smpl, null, null, "xxax", null, ivTmpvar249)
;




}

public static void C21(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar251 = i252;
O.Lookup(smpl, null, null, "xxbx", null, ivTmpvar251)
;




p.SetText(@"¤0");
IVariable ivTmpvar253 = i254;
O.Lookup(smpl, null, null, "xxay", null, ivTmpvar253)
;




p.SetText(@"¤0");
IVariable ivTmpvar255 = i256;
O.Lookup(smpl, null, null, "xxby", null, ivTmpvar255)
;




p.SetText(@"¤0");
IVariable ivTmpvar257 = i259;
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xxby", null, null),  ivTmpvar257, i258
)
;




p.SetText(@"¤0");
IVariable ivTmpvar260 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"));
O.Lookup(smpl, null, null, "#m1", null, ivTmpvar260)
;




p.SetText(@"¤0");
IVariable ivTmpvar261 = O.ListDef(new ScalarString(@"x"), new ScalarString(@"y"));
O.Lookup(smpl, null, null, "#m2", null, ivTmpvar261)
;




p.SetText(@"¤182");
O.Print(smpl, (temp264(smpl)));




p.SetText(@"¤183");
O.Print(smpl, (temp268(smpl)));




p.SetText(@"¤0");
IVariable ivTmpvar269 = O.ListDef(new ScalarString(@"xxax"), new ScalarString(@"xxbx"), new ScalarString(@"xxay"), new ScalarString(@"xxby"));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar269)
;




p.SetText(@"¤185");
O.Print(smpl, (temp272(smpl)));




}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);

C1(p);

if(O.IsTrue(smpl, O.Equals(smpl, i34,i35))) {
C2(p);

}else {
C3(p);

}
C4(p);

if(O.IsTrue(smpl, O.Equals(smpl, i44,i45))) {
C5(p);

}else {
C6(p);

}
C7(p);

C8(p);

C9(p);

C10(p);

C11(p);

C12(p);

List<List<IVariable>> lists186 = new List<List<IVariable>>();
//O.ConvertToList(lists186.Add((O.Lookup(smpl, null, null, "#m1", null, null))));
//O.ConvertToList(lists186.Add((O.Lookup(smpl, null, null, "#m2", null, null))));
int max187 = O.ForListMax(lists186);
for (int i188 = 0; i188 < max187; i188 ++) {;
IVariable forloop_183 = lists186[0][i188];
IVariable forloop_184 = lists186[1][i188];
IVariable ivTmpvar185 = O.Add(smpl, forloop_183, forloop_184);
O.Lookup(smpl, null, null, "%s", null, ivTmpvar185)
;

O.Print(smpl, (O.Lookup(smpl, null, null, "%s", null, null)));

};

C13(p);

C14(p);

IVariable forloop_191 = null;
int counter195 = 0;
for (O.IterateStart(ref forloop_191, i192); O.IterateContinue(forloop_191, i192, d193, null, ref counter195); O.IterateStep(forloop_191, i192, null, counter195))
{;
IVariable ivTmpvar194 = O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null), forloop_191);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar194)
;

};

C15(p);

IVariable forloop_198 = null;
int counter202 = 0;
for (O.IterateStart(ref forloop_198, i199); O.IterateContinue(forloop_198, i199, d200, null, ref counter202); O.IterateStep(forloop_198, i199, null, counter202))
{;
IVariable ivTmpvar201 = O.Add(smpl, O.Lookup(smpl, null, null, "%sum", null, null), forloop_198);
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar201)
;

};

C16(p);

IVariable forloop_208 = null;
int counter212 = 0;
for (O.IterateStart(ref forloop_208, i209); O.IterateContinue(forloop_208, i209, d210, null, ref counter212); O.IterateStep(forloop_208, i209, null, counter212))
{;
IVariable ivTmpvar211 = Globals.ufunctions1["add1"](smpl, O.Lookup(smpl, null, null, "%sum", null, null));
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar211)
;

};

C17(p);

C18(p);

IVariable forloop_220 = null;
int counter221 = 0;
for (O.IterateStart(ref forloop_220, O.Lookup(smpl, null, null, "#m", null, null)); O.IterateContinue(forloop_220, O.Lookup(smpl, null, null, "#m", null, null), null, null, ref counter221); O.IterateStep(forloop_220, O.Lookup(smpl, null, null, "#m", null, null), null, counter221))
{;
O.Print(smpl, (forloop_220));

};

C19(p);

C20(p);

C21(p);



}
}
}
