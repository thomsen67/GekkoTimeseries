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
public static readonly ScalarVal i3 = new ScalarVal(101d);
public static readonly ScalarVal i4 = new ScalarVal(102d);
public static readonly ScalarVal i5 = new ScalarVal(103d);
public static readonly ScalarVal i6 = new ScalarVal(104d);
public static readonly ScalarVal i7 = new ScalarVal(105d);
public static readonly ScalarVal i8 = new ScalarVal(106d);
public static readonly ScalarVal i9 = new ScalarVal(107d);
public static readonly ScalarVal i10 = new ScalarVal(108d);
public static readonly ScalarVal i11 = new ScalarVal(109d);
public static readonly ScalarVal i12 = new ScalarVal(110d);
public static readonly ScalarVal i13 = new ScalarVal(111d);
public static readonly ScalarVal i14 = new ScalarVal(1d);
public static readonly ScalarVal i18 = new ScalarVal(100d);
public static readonly ScalarVal i20 = new ScalarVal(2010d);
public static readonly ScalarVal i22 = new ScalarVal(1d);
public static readonly ScalarVal i25 = new ScalarVal(1d);
public static readonly ScalarVal i29 = new ScalarVal(1d);
public static readonly ScalarVal i31 = new ScalarVal(5d);
public static readonly ScalarVal i32 = new ScalarVal(5d);
public static readonly ScalarVal i34 = new ScalarVal(2d);
public static readonly ScalarVal i38 = new ScalarVal(1d);
public static readonly ScalarVal i40 = new ScalarVal(5d);
public static readonly ScalarVal i41 = new ScalarVal(6d);
public static readonly ScalarVal i43 = new ScalarVal(3d);
public static readonly ScalarVal i47 = new ScalarVal(1d);
public static readonly ScalarVal i49 = new ScalarVal(2010d);
public static readonly ScalarVal i50 = new ScalarVal(5d);
public static readonly ScalarVal i51 = new ScalarVal(5d);
public static readonly ScalarVal i53 = new ScalarVal(2d);
public static readonly ScalarVal i57 = new ScalarVal(1d);
public static readonly ScalarVal i59 = new ScalarVal(2010d);
public static readonly ScalarVal i60 = new ScalarVal(5d);
public static readonly ScalarVal i61 = new ScalarVal(6d);
public static readonly ScalarVal i63 = new ScalarVal(3d);
public static readonly ScalarVal i65 = new ScalarVal(1d);
public static readonly ScalarVal i66 = new ScalarVal(1d);
public static readonly ScalarVal i69 = new ScalarVal(0d);
public static readonly ScalarVal i72 = new ScalarVal(2d);
public static readonly ScalarVal i75 = new ScalarVal(0d);
public static readonly ScalarVal i78 = new ScalarVal(1d);
public static readonly ScalarVal i80 = new ScalarVal(1d);
public static readonly ScalarVal i81 = new ScalarVal(0d);
public static readonly ScalarVal i84 = new ScalarVal(0d);
public static readonly ScalarVal i87 = new ScalarVal(2d);
public static readonly ScalarVal i90 = new ScalarVal(0d);
public static readonly ScalarVal i93 = new ScalarVal(1d);
public static readonly ScalarVal i97 = new ScalarVal(1d);
public static readonly ScalarVal i98 = new ScalarVal(0d);
public static readonly ScalarVal i99 = new ScalarVal(1d);
public static readonly ScalarVal i103 = new ScalarVal(1d);
public static readonly ScalarVal i104 = new ScalarVal(2d);
public static readonly ScalarVal i105 = new ScalarVal(3d);
public static readonly ScalarVal i106 = new ScalarVal(4d);
public static readonly ScalarVal i107 = new ScalarVal(5d);
public static readonly ScalarVal i108 = new ScalarVal(6d);
public static readonly ScalarVal i109 = new ScalarVal(7d);
public static readonly ScalarVal i110 = new ScalarVal(5d);
public static readonly ScalarVal i111 = new ScalarVal(5d);
public static readonly ScalarVal i112 = new ScalarVal(10d);
public static readonly ScalarVal i113 = new ScalarVal(5d);
public static readonly ScalarVal i116 = new ScalarVal(100d);
public static readonly ScalarVal i117 = new ScalarVal(5d);
public static readonly ScalarVal i121 = new ScalarVal(1d);
public static readonly ScalarVal i122 = new ScalarVal(2d);
public static readonly ScalarVal i123 = new ScalarVal(3d);
public static readonly ScalarVal i124 = new ScalarVal(4d);
public static readonly ScalarVal i125 = new ScalarVal(5d);
public static readonly ScalarVal i126 = new ScalarVal(6d);
public static readonly ScalarVal i127 = new ScalarVal(7d);
public static readonly ScalarVal i128 = new ScalarVal(5d);
public static readonly ScalarVal i129 = new ScalarVal(5d);
public static readonly ScalarVal i130 = new ScalarVal(10d);
public static readonly ScalarVal i131 = new ScalarVal(5d);
public static readonly ScalarVal i134 = new ScalarVal(101d);
public static readonly ScalarVal i135 = new ScalarVal(102d);
public static readonly ScalarVal i136 = new ScalarVal(103d);
public static readonly ScalarVal i137 = new ScalarVal(104d);
public static readonly ScalarVal i138 = new ScalarVal(105d);
public static readonly ScalarVal i139 = new ScalarVal(106d);
public static readonly ScalarVal i140 = new ScalarVal(107d);
public static readonly ScalarVal i141 = new ScalarVal(108d);
public static readonly ScalarVal i142 = new ScalarVal(109d);
public static readonly ScalarVal i143 = new ScalarVal(110d);
public static readonly ScalarVal i144 = new ScalarVal(111d);
public static readonly ScalarVal i147 = new ScalarVal(5d);
public static readonly ScalarVal i151 = new ScalarVal(1d);
public static readonly ScalarVal i152 = new ScalarVal(2d);
public static readonly ScalarVal i153 = new ScalarVal(3d);
public static readonly ScalarVal i154 = new ScalarVal(4d);
public static readonly ScalarVal i155 = new ScalarVal(5d);
public static readonly ScalarVal i156 = new ScalarVal(6d);
public static readonly ScalarVal i157 = new ScalarVal(7d);
public static readonly ScalarVal i158 = new ScalarVal(5d);
public static readonly ScalarVal i159 = new ScalarVal(5d);
public static readonly ScalarVal i160 = new ScalarVal(10d);
public static readonly ScalarVal i161 = new ScalarVal(5d);
public static readonly ScalarVal i165 = new ScalarVal(1d);
public static readonly ScalarVal i166 = new ScalarVal(2d);
public static readonly ScalarVal i167 = new ScalarVal(3d);
public static readonly ScalarVal i168 = new ScalarVal(4d);
public static readonly ScalarVal i171 = new ScalarVal(1d);
public static readonly ScalarVal i172 = new ScalarVal(2d);
public static readonly ScalarVal i174 = new ScalarVal(100d);
public static readonly ScalarVal i175 = new ScalarVal(200d);
public static readonly ScalarVal i179 = new ScalarVal(1d);
public static readonly ScalarVal i180 = new ScalarVal(2d);
public static readonly ScalarVal i181 = new ScalarVal(3d);
public static readonly ScalarVal i182 = new ScalarVal(4d);
public static readonly ScalarVal i184 = new ScalarVal(1d);
public static readonly ScalarVal i185 = new ScalarVal(2d);
public static readonly ScalarVal i186 = new ScalarVal(1d);
public static readonly ScalarVal i188 = new ScalarVal(1d);
public static readonly ScalarVal i189 = new ScalarVal(2d);
public static readonly ScalarVal i190 = new ScalarVal(2d);
public static readonly ScalarVal i194 = new ScalarVal(1d);
public static readonly ScalarVal i195 = new ScalarVal(2d);
public static readonly ScalarVal i196 = new ScalarVal(3d);
public static readonly ScalarVal i197 = new ScalarVal(2d);
public static readonly ScalarVal i198 = new ScalarVal(3d);
public static readonly ScalarVal i201 = new ScalarVal(2d);
public static readonly ScalarVal i202 = new ScalarVal(3d);
public static readonly ScalarVal i204 = new ScalarVal(20d);
public static readonly ScalarVal i205 = new ScalarVal(30d);
public static readonly ScalarVal i219 = new ScalarVal(100d);
public static readonly ScalarVal i220 = new ScalarVal(1d);
public static IVariable MapDef_mapTmpvar214(GekkoSmpl smpl) {
Map mapTmpvar214 = new Map();
IVariable ivTmpvar215 = new ScalarString(@"a");
for (int iSmpl216 = 0; iSmpl216 < int.MaxValue; iSmpl216++) {
O.Lookup(smpl, mapTmpvar214, null, "%i1", null, ivTmpvar215, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl216); else break;
};

IVariable ivTmpvar217 = O.Add(smpl, i219, i220);
for (int iSmpl218 = 0; iSmpl218 < int.MaxValue; iSmpl218++) {
O.Lookup(smpl, mapTmpvar214, null, "%v", null, ivTmpvar217, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl218); else break;
};


return mapTmpvar214;
}public static IVariable MapDef_mapTmpvar233(GekkoSmpl smpl) {
Map mapTmpvar233 = new Map();
IVariable ivTmpvar234 = new ScalarString(@"b");
for (int iSmpl235 = 0; iSmpl235 < int.MaxValue; iSmpl235++) {
O.Lookup(smpl, mapTmpvar233, null, "%i1", null, ivTmpvar234, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl235); else break;
};

IVariable ivTmpvar236 = new ScalarString(@"c");
for (int iSmpl237 = 0; iSmpl237 < int.MaxValue; iSmpl237++) {
O.Lookup(smpl, mapTmpvar233, null, "%i2", null, ivTmpvar236, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl237); else break;
};


return mapTmpvar233;
}public static IVariable MapDef_mapTmpvar228(GekkoSmpl smpl) {
Map mapTmpvar228 = new Map();
IVariable ivTmpvar229 = new ScalarString(@"a");
for (int iSmpl230 = 0; iSmpl230 < int.MaxValue; iSmpl230++) {
O.Lookup(smpl, mapTmpvar228, null, "%i1", null, ivTmpvar229, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl230); else break;
};

IVariable ivTmpvar231 = MapDef_mapTmpvar233(smpl);
for (int iSmpl232 = 0; iSmpl232 < int.MaxValue; iSmpl232++) {
O.Lookup(smpl, mapTmpvar228, null, "#m", null, ivTmpvar231, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl232); else break;
};


return mapTmpvar228;
}public static readonly ScalarVal i246 = new ScalarVal(1d);
public static readonly ScalarVal i247 = new ScalarVal(2d);
public static readonly ScalarVal i248 = new ScalarVal(3d);
public static readonly ScalarVal i249 = new ScalarVal(4d);
public static readonly ScalarVal i250 = new ScalarVal(1d);
public static readonly ScalarVal i252 = new ScalarVal(2d);
public static readonly ScalarVal i255 = new ScalarVal(2d);
public static readonly ScalarVal i256 = new ScalarVal(1d);
public static readonly ScalarVal i257 = new ScalarVal(1d);
public static readonly ScalarVal i259 = new ScalarVal(10d);
public static readonly ScalarVal i260 = new ScalarVal(2d);
public static readonly ScalarVal i262 = new ScalarVal(2d);
public static readonly ScalarVal i263 = new ScalarVal(1d);
public static readonly ScalarVal i264 = new ScalarVal(1d);
public static readonly ScalarVal i269 = new ScalarVal(1d);
public static readonly ScalarVal i270 = new ScalarVal(2d);
public static readonly ScalarVal i272 = new ScalarVal(1d);
public static readonly ScalarVal i273 = new ScalarVal(2d);
public static readonly ScalarVal i289 = new ScalarVal(0d);
public static readonly ScalarVal i291 = new ScalarVal(1d);
public static readonly ScalarVal d292 = new ScalarVal(1e3d);
public static readonly ScalarVal i299 = new ScalarVal(0d);
public static readonly ScalarVal i301 = new ScalarVal(1d);
public static readonly ScalarVal d302 = new ScalarVal(1e3d);
public static readonly ScalarVal i308 = new ScalarVal(1d);
public static void FunctionDef309() {


//[[splitSTOP]]

Globals.ufunctions1.Add("add1", (GekkoSmpl smpl, IVariable functionarg_307) => { 
//[[splitSTOP]]
return O.Add(smpl, functionarg_307, i308);

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i312 = new ScalarVal(0d);
public static readonly ScalarVal i314 = new ScalarVal(1d);
public static readonly ScalarVal d315 = new ScalarVal(1e3d);
public static void FunctionDef322() {


//[[splitSTOP]]

Globals.ufunctions2.Add("add2", (GekkoSmpl smpl, IVariable functionarg_320, IVariable functionarg_321) => { 
//[[splitSTOP]]
return O.ListDef(functionarg_320, O.Add(smpl, functionarg_320, functionarg_321));

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i323 = new ScalarVal(10d);
public static readonly ScalarVal i324 = new ScalarVal(20d);
public static readonly ScalarVal i325 = new ScalarVal(2d);
public static void FunctionDef333() {


//[[splitSTOP]]

Globals.ufunctions1.Add("f", (GekkoSmpl smpl, IVariable functionarg_332) => { 
//[[splitSTOP]]
return O.Add(smpl, functionarg_332, functionarg_332);

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i336 = new ScalarVal(100d);
public static readonly ScalarVal i342 = new ScalarVal(2d);
public static readonly ScalarVal i345 = new ScalarVal(1d);
public static readonly ScalarVal i348 = new ScalarVal(2d);
public static readonly ScalarVal i351 = new ScalarVal(3d);
public static readonly ScalarVal i354 = new ScalarVal(4d);
public static readonly ScalarVal i356 = new ScalarVal(2010d);
public static readonly ScalarVal i358 = new ScalarVal(1000d);
public static IVariable temp366(GekkoSmpl smpl) {
TimeSeries temp366 = new TimeSeries(Program.options.freq, null); temp366.SetZero(smpl);

foreach (IVariable listloop_m1364 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null, false))) {
foreach (IVariable listloop_m2365 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null, false))) {
temp366.InjectAdd(smpl, temp366, O.Indexer(smpl, O.Lookup(smpl, null, null, "xx", null, null, false), listloop_m1364, listloop_m2365));

}
}
return temp366;

}
public static readonly ScalarVal i370 = new ScalarVal(0d);
public static IVariable temp371(GekkoSmpl smpl) {
MetaList temp371 = new MetaList();

foreach (IVariable listloop_m1368 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null, false))) {
foreach (IVariable listloop_m2369 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null, false))) {
temp371.Add(O.Add(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "xx", null, null, false), listloop_m1368, listloop_m2369), i370));

}
}
return temp371;

}
public static readonly ScalarVal i375 = new ScalarVal(1d);
public static readonly ScalarVal i378 = new ScalarVal(2d);
public static readonly ScalarVal i381 = new ScalarVal(3d);
public static readonly ScalarVal i384 = new ScalarVal(4d);
public static readonly ScalarVal i386 = new ScalarVal(2010d);
public static readonly ScalarVal i388 = new ScalarVal(1000d);
public static IVariable temp395(GekkoSmpl smpl) {
TimeSeries temp395 = new TimeSeries(Program.options.freq, null); temp395.SetZero(smpl);

foreach (IVariable listloop_m1393 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null, false))) {
foreach (IVariable listloop_m2394 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null, false))) {
temp395.InjectAdd(smpl, temp395, O.Lookup(smpl, null, (new ScalarString("xx", true, false)).Add(smpl, listloop_m1393).Add(smpl, listloop_m2394), null, false));

}
}
return temp395;

}
public static readonly ScalarVal i399 = new ScalarVal(0d);
public static IVariable temp400(GekkoSmpl smpl) {
MetaList temp400 = new MetaList();

foreach (IVariable listloop_m1397 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null, false))) {
foreach (IVariable listloop_m2398 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null, false))) {
temp400.Add(O.Add(smpl, O.Lookup(smpl, null, (new ScalarString("xx", true, false)).Add(smpl, listloop_m1397).Add(smpl, listloop_m2398), null, false), i399));

}
}
return temp400;

}
public static readonly ScalarVal i405 = new ScalarVal(0d);
public static IVariable temp406(GekkoSmpl smpl) {
MetaList temp406 = new MetaList();

foreach (IVariable listloop_m404 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), null, false))) {
temp406.Add(O.Add(smpl, O.Lookup(smpl, null, (listloop_m404), null, false), i405));

}
return temp406;

}
public static readonly ScalarVal i410 = new ScalarVal(100d);
public static IVariable MapDef_mapTmpvar418(GekkoSmpl smpl) {
Map mapTmpvar418 = new Map();
IVariable ivTmpvar419 = new ScalarString(@"b");
for (int iSmpl420 = 0; iSmpl420 < int.MaxValue; iSmpl420++) {
O.Lookup(smpl, mapTmpvar418, null, "%i1", null, ivTmpvar419, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl420); else break;
};

IVariable ivTmpvar421 = new ScalarString(@"c");
for (int iSmpl422 = 0; iSmpl422 < int.MaxValue; iSmpl422++) {
O.Lookup(smpl, mapTmpvar418, null, "%i2", null, ivTmpvar421, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl422); else break;
};


return mapTmpvar418;
}public static IVariable MapDef_mapTmpvar413(GekkoSmpl smpl) {
Map mapTmpvar413 = new Map();
IVariable ivTmpvar414 = new ScalarString(@"a");
for (int iSmpl415 = 0; iSmpl415 < int.MaxValue; iSmpl415++) {
O.Lookup(smpl, mapTmpvar413, null, "%i1", null, ivTmpvar414, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl415); else break;
};

IVariable ivTmpvar416 = MapDef_mapTmpvar418(smpl);
for (int iSmpl417 = 0; iSmpl417 < int.MaxValue; iSmpl417++) {
O.Lookup(smpl, mapTmpvar413, null, "#m", null, ivTmpvar416, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl417); else break;
};


return mapTmpvar413;
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
IVariable ivTmpvar1 = O.ListDef(i3, i4, i5, i6, i7, i8, i9, i10, i11, i12, i13);
for (int iSmpl2 = 0; iSmpl2 < int.MaxValue; iSmpl2++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar1, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl2); else break;
};




p.SetText(@"¤0");
for (int iSmpl15 = 0; iSmpl15 < int.MaxValue; iSmpl15++) {
O.Print(smpl, (O.Indexer(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false), O.Lookup(smpl, null, null, "xx", null, null, false)), O.Negate(smpl, i14)
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl15); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar16 = i18;
for (int iSmpl17 = 0; iSmpl17 < int.MaxValue; iSmpl17++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar16, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl17); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar19 = i22;
for (int iSmpl21 = 0; iSmpl21 < int.MaxValue; iSmpl21++) {
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true),  ivTmpvar19, i20
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl21); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar23 = i25;
for (int iSmpl24 = 0; iSmpl24 < int.MaxValue; iSmpl24++) {
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true),  ivTmpvar23, new ScalarDate(G.FromStringToDate("2012a1"))
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl24); else break;
};




p.SetText(@"¤0");
for (int iSmpl26 = 0; iSmpl26 < int.MaxValue; iSmpl26++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl26); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar27 = i29;
for (int iSmpl28 = 0; iSmpl28 < int.MaxValue; iSmpl28++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar27, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl28); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar30 = i34;
for (int iSmpl33 = 0; iSmpl33 < int.MaxValue; iSmpl33++) {
O.DollarLookup(O.Equals(smpl, i31,i32)
, smpl, null, null, "xx", null, ivTmpvar30, false)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl33); else break;
};




}

public static void C1(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
for (int iSmpl35 = 0; iSmpl35 < int.MaxValue; iSmpl35++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl35); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar36 = i38;
for (int iSmpl37 = 0; iSmpl37 < int.MaxValue; iSmpl37++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar36, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl37); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar39 = i43;
for (int iSmpl42 = 0; iSmpl42 < int.MaxValue; iSmpl42++) {
O.DollarLookup(O.Equals(smpl, i40,i41)
, smpl, null, null, "xx", null, ivTmpvar39, false)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl42); else break;
};




p.SetText(@"¤0");
for (int iSmpl44 = 0; iSmpl44 < int.MaxValue; iSmpl44++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl44); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar45 = i47;
for (int iSmpl46 = 0; iSmpl46 < int.MaxValue; iSmpl46++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar45, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl46); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar48 = i53;
for (int iSmpl52 = 0; iSmpl52 < int.MaxValue; iSmpl52++) {
O.DollarIndexerSetData(O.Equals(smpl, i50,i51)
, smpl, O.Lookup(smpl, null, null, "xx", null, null, false),  ivTmpvar48, i49
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl52); else break;
};




p.SetText(@"¤0");
for (int iSmpl54 = 0; iSmpl54 < int.MaxValue; iSmpl54++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl54); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar55 = i57;
for (int iSmpl56 = 0; iSmpl56 < int.MaxValue; iSmpl56++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar55, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl56); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar58 = i63;
for (int iSmpl62 = 0; iSmpl62 < int.MaxValue; iSmpl62++) {
O.DollarIndexerSetData(O.Equals(smpl, i60,i61)
, smpl, O.Lookup(smpl, null, null, "xx", null, null, false),  ivTmpvar58, i59
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl62); else break;
};




p.SetText(@"¤0");
for (int iSmpl64 = 0; iSmpl64 < int.MaxValue; iSmpl64++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl64); else break;
}


p.SetText(@"¤61");

}

public static void C2(P p) {

GekkoSmpl smpl = O.Smpl();

IVariable ivTmpvar67 = i69;
for (int iSmpl68 = 0; iSmpl68 < int.MaxValue; iSmpl68++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar67, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl68); else break;
};

IVariable ivTmpvar70 = i72;
for (int iSmpl71 = 0; iSmpl71 < int.MaxValue; iSmpl71++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar70, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl71); else break;
};


}

public static void C3(P p) {

GekkoSmpl smpl = O.Smpl();

IVariable ivTmpvar73 = i75;
for (int iSmpl74 = 0; iSmpl74 < int.MaxValue; iSmpl74++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar73, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl74); else break;
};

IVariable ivTmpvar76 = i78;
for (int iSmpl77 = 0; iSmpl77 < int.MaxValue; iSmpl77++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar76, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl77); else break;
};


}

public static void C4(P p) {

GekkoSmpl smpl = O.Smpl();




}

public static void C5(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
for (int iSmpl79 = 0; iSmpl79 < int.MaxValue; iSmpl79++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl79); else break;
}


p.SetText(@"¤64");

}

public static void C6(P p) {

GekkoSmpl smpl = O.Smpl();

IVariable ivTmpvar82 = i84;
for (int iSmpl83 = 0; iSmpl83 < int.MaxValue; iSmpl83++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar82, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl83); else break;
};

IVariable ivTmpvar85 = i87;
for (int iSmpl86 = 0; iSmpl86 < int.MaxValue; iSmpl86++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar85, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl86); else break;
};


}

public static void C7(P p) {

GekkoSmpl smpl = O.Smpl();

IVariable ivTmpvar88 = i90;
for (int iSmpl89 = 0; iSmpl89 < int.MaxValue; iSmpl89++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar88, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl89); else break;
};

IVariable ivTmpvar91 = i93;
for (int iSmpl92 = 0; iSmpl92 < int.MaxValue; iSmpl92++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar91, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl92); else break;
};


}

public static void C8(P p) {

GekkoSmpl smpl = O.Smpl();




p.SetText(@"¤0");
for (int iSmpl94 = 0; iSmpl94 < int.MaxValue; iSmpl94++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl94); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar95 = O.Dollar(smpl, i97, O.Equals(smpl, i98,i99)
);
for (int iSmpl96 = 0; iSmpl96 < int.MaxValue; iSmpl96++) {
O.Lookup(smpl, null, null, "%v", null, ivTmpvar95, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl96); else break;
};




p.SetText(@"¤0");
for (int iSmpl100 = 0; iSmpl100 < int.MaxValue; iSmpl100++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "%v", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl100); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar101 = O.ListDef(i103, i104, i105, i106, i107, i108, i109, i110, i111, i112, i113);
for (int iSmpl102 = 0; iSmpl102 < int.MaxValue; iSmpl102++) {
O.Lookup(smpl, null, null, "xx1", null, ivTmpvar101, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl102); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar114 = O.Dollar(smpl, i116, O.Equals(smpl, O.Lookup(smpl, null, null, "xx1", null, null, false),i117)
);
for (int iSmpl115 = 0; iSmpl115 < int.MaxValue; iSmpl115++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar114, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl115); else break;
};




p.SetText(@"¤0");
for (int iSmpl118 = 0; iSmpl118 < int.MaxValue; iSmpl118++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl118); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar119 = O.ListDef(i121, i122, i123, i124, i125, i126, i127, i128, i129, i130, i131);
for (int iSmpl120 = 0; iSmpl120 < int.MaxValue; iSmpl120++) {
O.Lookup(smpl, null, null, "xx1", null, ivTmpvar119, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl120); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar132 = O.ListDef(i134, i135, i136, i137, i138, i139, i140, i141, i142, i143, i144);
for (int iSmpl133 = 0; iSmpl133 < int.MaxValue; iSmpl133++) {
O.Lookup(smpl, null, null, "xx2", null, ivTmpvar132, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl133); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar145 = O.Dollar(smpl, O.Lookup(smpl, null, null, "xx2", null, null, false), O.Equals(smpl, O.Lookup(smpl, null, null, "xx1", null, null, false),i147)
);
for (int iSmpl146 = 0; iSmpl146 < int.MaxValue; iSmpl146++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar145, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl146); else break;
};




}

public static void C9(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
for (int iSmpl148 = 0; iSmpl148 < int.MaxValue; iSmpl148++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl148); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar149 = O.MatrixCol(O.MatrixRow(i151), O.MatrixRow(i152), O.MatrixRow(i153), O.MatrixRow(i154), O.MatrixRow(i155), O.MatrixRow(i156), O.MatrixRow(i157), O.MatrixRow(i158), O.MatrixRow(i159), O.MatrixRow(i160), O.MatrixRow(i161));
for (int iSmpl150 = 0; iSmpl150 < int.MaxValue; iSmpl150++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar149, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl150); else break;
};




p.SetText(@"¤0");
for (int iSmpl162 = 0; iSmpl162 < int.MaxValue; iSmpl162++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl162); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar163 = O.MatrixCol(O.MatrixRow(i165, i166), O.MatrixRow(i167, i168));
for (int iSmpl164 = 0; iSmpl164 < int.MaxValue; iSmpl164++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar163, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl164); else break;
};




p.SetText(@"¤0");
for (int iSmpl169 = 0; iSmpl169 < int.MaxValue; iSmpl169++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "#m", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl169); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar170 = O.MatrixCol(O.MatrixRow(i174), O.MatrixRow(i175));
for (int iSmpl173 = 0; iSmpl173 < int.MaxValue; iSmpl173++) {
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null, true),  ivTmpvar170, (new Range(i171, i172)))
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl173); else break;
};




p.SetText(@"¤0");
for (int iSmpl176 = 0; iSmpl176 < int.MaxValue; iSmpl176++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "#m", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl176); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar177 = O.MatrixCol(O.MatrixRow(i179, i180), O.MatrixRow(i181, i182));
for (int iSmpl178 = 0; iSmpl178 < int.MaxValue; iSmpl178++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar177, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl178); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar183 = O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (new Range(i188, i189)), i190
);
for (int iSmpl187 = 0; iSmpl187 < int.MaxValue; iSmpl187++) {
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null, true),  ivTmpvar183, (new Range(i184, i185)), i186
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl187); else break;
};




p.SetText(@"¤0");
for (int iSmpl191 = 0; iSmpl191 < int.MaxValue; iSmpl191++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "#m", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl191); else break;
}



}

public static void C10(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar192 = O.MatrixCol(O.MatrixRow(i194), O.MatrixRow(i195), O.MatrixRow(i196));
for (int iSmpl193 = 0; iSmpl193 < int.MaxValue; iSmpl193++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar192, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl193); else break;
};




p.SetText(@"¤0");
for (int iSmpl199 = 0; iSmpl199 < int.MaxValue; iSmpl199++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (new Range(i197, i198)))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl199); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar200 = O.MatrixCol(O.MatrixRow(i204), O.MatrixRow(i205));
for (int iSmpl203 = 0; iSmpl203 < int.MaxValue; iSmpl203++) {
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null, true),  ivTmpvar200, (new Range(i201, i202)))
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl203); else break;
};




p.SetText(@"¤0");
for (int iSmpl206 = 0; iSmpl206 < int.MaxValue; iSmpl206++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "#m", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl206); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar207 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"));
for (int iSmpl208 = 0; iSmpl208 < int.MaxValue; iSmpl208++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar207, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl208); else break;
};




p.SetText(@"¤0");
for (int iSmpl209 = 0; iSmpl209 < int.MaxValue; iSmpl209++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), new ScalarString(@"a")
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl209); else break;
}



p.SetText(@"¤0");
for (int iSmpl210 = 0; iSmpl210 < int.MaxValue; iSmpl210++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), new ScalarString(@"c")
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl210); else break;
}



p.SetText(@"¤0");
for (int iSmpl211 = 0; iSmpl211 < int.MaxValue; iSmpl211++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), new ScalarString(@"a*")
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl211); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar212 = MapDef_mapTmpvar214(smpl);
for (int iSmpl213 = 0; iSmpl213 < int.MaxValue; iSmpl213++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar212, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl213); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar221 = new ScalarString(@"b");
for (int iSmpl222 = 0; iSmpl222 < int.MaxValue; iSmpl222++) {
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null, true),  ivTmpvar221, (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))))
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl222); else break;
};




}

public static void C11(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
for (int iSmpl223 = 0; iSmpl223 < int.MaxValue; iSmpl223++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl223); else break;
}



p.SetText(@"¤0");
for (int iSmpl224 = 0; iSmpl224 < int.MaxValue; iSmpl224++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl224); else break;
}



p.SetText(@"¤0");
for (int iSmpl225 = 0; iSmpl225 < int.MaxValue; iSmpl225++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringPercent).Add(smpl, (new ScalarString("v", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl225); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar226 = MapDef_mapTmpvar228(smpl);
for (int iSmpl227 = 0; iSmpl227 < int.MaxValue; iSmpl227++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar226, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl227); else break;
};




p.SetText(@"¤0");
for (int iSmpl238 = 0; iSmpl238 < int.MaxValue; iSmpl238++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl238); else break;
}



p.SetText(@"¤0");
for (int iSmpl239 = 0; iSmpl239 < int.MaxValue; iSmpl239++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl239); else break;
}



p.SetText(@"¤0");
for (int iSmpl240 = 0; iSmpl240 < int.MaxValue; iSmpl240++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl240); else break;
}



p.SetText(@"¤0");
for (int iSmpl241 = 0; iSmpl241 < int.MaxValue; iSmpl241++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), new ScalarString(@"%i1")
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl241); else break;
}



p.SetText(@"¤0");
for (int iSmpl242 = 0; iSmpl242 < int.MaxValue; iSmpl242++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), new ScalarString(@"#m")
), new ScalarString(@"%i1")
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl242); else break;
}



p.SetText(@"¤0");
for (int iSmpl243 = 0; iSmpl243 < int.MaxValue; iSmpl243++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), new ScalarString(@"#m")
), new ScalarString(@"%i2")
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl243); else break;
}



}

public static void C12(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar244 = O.ListDef(new ScalarString(@"a"), O.MatrixCol(O.MatrixRow(i246, i247), O.MatrixRow(i248, i249)));
for (int iSmpl245 = 0; iSmpl245 < int.MaxValue; iSmpl245++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar244, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl245); else break;
};




p.SetText(@"¤0");
for (int iSmpl251 = 0; iSmpl251 < int.MaxValue; iSmpl251++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), i250
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl251); else break;
}



p.SetText(@"¤0");
for (int iSmpl253 = 0; iSmpl253 < int.MaxValue; iSmpl253++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), i252
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl253); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar254 = i259;
for (int iSmpl258 = 0; iSmpl258 < int.MaxValue; iSmpl258++) {
O.IndexerSetData(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, true), i255
),  ivTmpvar254, i256
, i257
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl258); else break;
};




p.SetText(@"¤0");
for (int iSmpl261 = 0; iSmpl261 < int.MaxValue; iSmpl261++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), i260
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl261); else break;
}



p.SetText(@"¤0");
for (int iSmpl265 = 0; iSmpl265 < int.MaxValue; iSmpl265++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), i262
), i263
, i264
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl265); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar266 = O.ListDef(O.ListDef(new ScalarString(@"a"), new ScalarString(@"b")), new ScalarString(@"c"));
for (int iSmpl267 = 0; iSmpl267 < int.MaxValue; iSmpl267++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar266, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl267); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar268 = new ScalarString(@"x");
for (int iSmpl271 = 0; iSmpl271 < int.MaxValue; iSmpl271++) {
O.IndexerSetData(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, true), i269
),  ivTmpvar268, i270
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl271); else break;
};




p.SetText(@"¤0");
for (int iSmpl274 = 0; iSmpl274 < int.MaxValue; iSmpl274++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), i272
), i273
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl274); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar275 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"));
for (int iSmpl276 = 0; iSmpl276 < int.MaxValue; iSmpl276++) {
O.Lookup(smpl, null, null, "#m1", null, ivTmpvar275, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl276); else break;
};




}

public static void C13(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar277 = O.ListDef(new ScalarString(@"x"), new ScalarString(@"y"));
for (int iSmpl278 = 0; iSmpl278 < int.MaxValue; iSmpl278++) {
O.Lookup(smpl, null, null, "#m2", null, ivTmpvar277, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl278); else break;
};




p.SetText(@"¤0");

}

public static void C14(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤0");
IVariable ivTmpvar287 = i289;
for (int iSmpl288 = 0; iSmpl288 < int.MaxValue; iSmpl288++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar287, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl288); else break;
};




p.SetText(@"¤0");

}

public static void C15(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤0");
for (int iSmpl296 = 0; iSmpl296 < int.MaxValue; iSmpl296++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl296); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar297 = i299;
for (int iSmpl298 = 0; iSmpl298 < int.MaxValue; iSmpl298++) {
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar297, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl298); else break;
};




p.SetText(@"¤0");

}

public static void C16(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤0");
for (int iSmpl306 = 0; iSmpl306 < int.MaxValue; iSmpl306++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "%sum", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl306); else break;
}



p.SetText(@"¤150");
FunctionDef309();





p.SetText(@"¤0");
IVariable ivTmpvar310 = i312;
for (int iSmpl311 = 0; iSmpl311 < int.MaxValue; iSmpl311++) {
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar310, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl311); else break;
};




}

public static void C17(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");

}

public static void C18(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤0");
for (int iSmpl319 = 0; iSmpl319 < int.MaxValue; iSmpl319++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "%sum", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl319); else break;
}



p.SetText(@"¤158");
FunctionDef322();





p.SetText(@"¤159");
for (int iSmpl326 = 0; iSmpl326 < int.MaxValue; iSmpl326++) {
O.Print(smpl, (O.Indexer(smpl, O.FunctionLookup2("add2")(smpl, i323, i324), i325
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl326); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar327 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"), new ScalarString(@"c"));
for (int iSmpl328 = 0; iSmpl328 < int.MaxValue; iSmpl328++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar327, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl328); else break;
};




p.SetText(@"¤0");

}

public static void C19(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤169");
FunctionDef333();





p.SetText(@"¤0");
IVariable ivTmpvar334 = i336;
for (int iSmpl335 = 0; iSmpl335 < int.MaxValue; iSmpl335++) {
O.Lookup(smpl, null, null, "%v1", null, ivTmpvar334, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl335); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar337 = O.FunctionLookup1("f")(smpl, O.FunctionLookup1("f")(smpl, O.Lookup(smpl, null, null, "%v1", null, null, false)));
for (int iSmpl338 = 0; iSmpl338 < int.MaxValue; iSmpl338++) {
O.Lookup(smpl, null, null, "%v2", null, ivTmpvar337, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl338); else break;
};




p.SetText(@"¤0");
for (int iSmpl339 = 0; iSmpl339 < int.MaxValue; iSmpl339++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "%v2", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl339); else break;
}



}

public static void C20(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar340 = Functions.series(smpl, i342);
for (int iSmpl341 = 0; iSmpl341 < int.MaxValue; iSmpl341++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar340, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl341); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar343 = i345;
for (int iSmpl344 = 0; iSmpl344 < int.MaxValue; iSmpl344++) {
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true),  ivTmpvar343, new ScalarString(@"a")
, new ScalarString(@"x")
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl344); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar346 = i348;
for (int iSmpl347 = 0; iSmpl347 < int.MaxValue; iSmpl347++) {
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true),  ivTmpvar346, new ScalarString(@"b")
, new ScalarString(@"x")
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl347); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar349 = i351;
for (int iSmpl350 = 0; iSmpl350 < int.MaxValue; iSmpl350++) {
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true),  ivTmpvar349, new ScalarString(@"a")
, new ScalarString(@"y")
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl350); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar352 = i354;
for (int iSmpl353 = 0; iSmpl353 < int.MaxValue; iSmpl353++) {
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true),  ivTmpvar352, new ScalarString(@"b")
, new ScalarString(@"y")
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl353); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar355 = i358;
for (int iSmpl357 = 0; iSmpl357 < int.MaxValue; iSmpl357++) {
O.IndexerSetData(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "xx", null, null, true), new ScalarString(@"b")
, new ScalarString(@"y")
),  ivTmpvar355, i356
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl357); else break;
};




p.SetText(@"¤0");
for (int iSmpl359 = 0; iSmpl359 < int.MaxValue; iSmpl359++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "xx", null, null, false), new ScalarString(@"b")
, new ScalarString(@"y")
)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl359); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar360 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"));
for (int iSmpl361 = 0; iSmpl361 < int.MaxValue; iSmpl361++) {
O.Lookup(smpl, null, null, "#m1", null, ivTmpvar360, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl361); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar362 = O.ListDef(new ScalarString(@"x"), new ScalarString(@"y"));
for (int iSmpl363 = 0; iSmpl363 < int.MaxValue; iSmpl363++) {
O.Lookup(smpl, null, null, "#m2", null, ivTmpvar362, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl363); else break;
};




p.SetText(@"¤188");
for (int iSmpl367 = 0; iSmpl367 < int.MaxValue; iSmpl367++) {
O.Print(smpl, (temp366(smpl)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl367); else break;
}



}

public static void C21(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤189");
for (int iSmpl372 = 0; iSmpl372 < int.MaxValue; iSmpl372++) {
O.Print(smpl, (temp371(smpl)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl372); else break;
}



p.SetText(@"¤0");
IVariable ivTmpvar373 = i375;
for (int iSmpl374 = 0; iSmpl374 < int.MaxValue; iSmpl374++) {
O.Lookup(smpl, null, null, "xxax", null, ivTmpvar373, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl374); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar376 = i378;
for (int iSmpl377 = 0; iSmpl377 < int.MaxValue; iSmpl377++) {
O.Lookup(smpl, null, null, "xxbx", null, ivTmpvar376, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl377); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar379 = i381;
for (int iSmpl380 = 0; iSmpl380 < int.MaxValue; iSmpl380++) {
O.Lookup(smpl, null, null, "xxay", null, ivTmpvar379, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl380); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar382 = i384;
for (int iSmpl383 = 0; iSmpl383 < int.MaxValue; iSmpl383++) {
O.Lookup(smpl, null, null, "xxby", null, ivTmpvar382, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl383); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar385 = i388;
for (int iSmpl387 = 0; iSmpl387 < int.MaxValue; iSmpl387++) {
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xxby", null, null, true),  ivTmpvar385, i386
)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl387); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar389 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"));
for (int iSmpl390 = 0; iSmpl390 < int.MaxValue; iSmpl390++) {
O.Lookup(smpl, null, null, "#m1", null, ivTmpvar389, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl390); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar391 = O.ListDef(new ScalarString(@"x"), new ScalarString(@"y"));
for (int iSmpl392 = 0; iSmpl392 < int.MaxValue; iSmpl392++) {
O.Lookup(smpl, null, null, "#m2", null, ivTmpvar391, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl392); else break;
};




p.SetText(@"¤199");
for (int iSmpl396 = 0; iSmpl396 < int.MaxValue; iSmpl396++) {
O.Print(smpl, (temp395(smpl)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl396); else break;
}



p.SetText(@"¤200");
for (int iSmpl401 = 0; iSmpl401 < int.MaxValue; iSmpl401++) {
O.Print(smpl, (temp400(smpl)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl401); else break;
}



}

public static void C22(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar402 = O.ListDef(new ScalarString(@"xxax"), new ScalarString(@"xxbx"), new ScalarString(@"xxay"), new ScalarString(@"xxby"));
for (int iSmpl403 = 0; iSmpl403 < int.MaxValue; iSmpl403++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar402, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl403); else break;
};




p.SetText(@"¤202");
for (int iSmpl407 = 0; iSmpl407 < int.MaxValue; iSmpl407++) {
O.Print(smpl, (temp406(smpl)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl407); else break;
}



p.SetText(@"¤0");
O.Reset o113 = new O.Reset();
o113.p = p;o113.Exe();




p.SetText(@"¤0");
IVariable ivTmpvar408 = i410;
for (int iSmpl409 = 0; iSmpl409 < int.MaxValue; iSmpl409++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar408, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl409); else break;
};




p.SetText(@"¤0");
IVariable ivTmpvar411 = MapDef_mapTmpvar413(smpl);
for (int iSmpl412 = 0; iSmpl412 < int.MaxValue; iSmpl412++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar411, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl412); else break;
};




p.SetText(@"¤0");
for (int iSmpl423 = 0; iSmpl423 < int.MaxValue; iSmpl423++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl423); else break;
}



p.SetText(@"¤0");
for (int iSmpl424 = 0; iSmpl424 < int.MaxValue; iSmpl424++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl424); else break;
}



p.SetText(@"¤0");
for (int iSmpl425 = 0; iSmpl425 < int.MaxValue; iSmpl425++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl425); else break;
}



p.SetText(@"¤211");
O.Write o119 = new O.Write();

o119.fileName = O.ConvertToString((new ScalarString("slet", true, false)));

o119.type = @"write";o119.Exe();




p.SetText(@"¤0");
O.Reset o120 = new O.Reset();
o120.p = p;o120.Exe();




}

public static void C23(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤213");
ClearTS(p);
O.Read o121 = new O.Read();
o121.p = p;
o121.type = @"read";
o121.fileName = O.ConvertToString((new ScalarString("slet", true, false)));


o121.Exe();




p.SetText(@"¤0");
for (int iSmpl426 = 0; iSmpl426 < int.MaxValue; iSmpl426++) {
O.Print(smpl, (O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl426); else break;
}



p.SetText(@"¤0");
for (int iSmpl427 = 0; iSmpl427 < int.MaxValue; iSmpl427++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl427); else break;
}



p.SetText(@"¤0");
for (int iSmpl428 = 0; iSmpl428 < int.MaxValue; iSmpl428++) {
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, null, "#m", null, null, false), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))))));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl428); else break;
}



p.SetText(@"¤0");
for (int iSmpl429 = 0; iSmpl429 < int.MaxValue; iSmpl429++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "xx", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl429); else break;
}



}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);

C1(p);

if(O.IsTrue(smpl, O.Equals(smpl, i65,i66))) {
C2(p);

}else {
C3(p);

}
C4(p);

C5(p);

if(O.IsTrue(smpl, O.Equals(smpl, i80,i81))) {
C6(p);

}else {
C7(p);

}
C8(p);

C9(p);

C10(p);

C11(p);

C12(p);

C13(p);

List<List<IVariable>> lists284 = new List<List<IVariable>>();
lists284.Add(O.ConvertToList(O.Lookup(smpl, null, null, "#m1", null, null, false)));
lists284.Add(O.ConvertToList(O.Lookup(smpl, null, null, "#m2", null, null, false)));
int max285 = O.ForListMax(lists284);
for (int i286 = 0; i286 < max285; i286 ++) {;
IVariable forloop_279 = lists284[0][i286];
IVariable forloop_280 = lists284[1][i286];
IVariable ivTmpvar281 = O.Add(smpl, forloop_279, forloop_280);
for (int iSmpl282 = 0; iSmpl282 < int.MaxValue; iSmpl282++) {
O.Lookup(smpl, null, null, "%s", null, ivTmpvar281, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl282); else break;
};

for (int iSmpl283 = 0; iSmpl283 < int.MaxValue; iSmpl283++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "%s", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl283); else break;
}
};

C14(p);

IVariable forloop_290 = null;
int counter295 = 0;
for (O.IterateStart(ref forloop_290, i291); O.IterateContinue(forloop_290, i291, d292, null, ref counter295); O.IterateStep(forloop_290, i291, null, counter295))
{;
IVariable ivTmpvar293 = O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false), forloop_290);
for (int iSmpl294 = 0; iSmpl294 < int.MaxValue; iSmpl294++) {
O.Lookup(smpl, null, null, "xx", null, ivTmpvar293, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl294); else break;
};

};

C15(p);

IVariable forloop_300 = null;
int counter305 = 0;
for (O.IterateStart(ref forloop_300, i301); O.IterateContinue(forloop_300, i301, d302, null, ref counter305); O.IterateStep(forloop_300, i301, null, counter305))
{;
IVariable ivTmpvar303 = O.Add(smpl, O.Lookup(smpl, null, null, "%sum", null, null, false), forloop_300);
for (int iSmpl304 = 0; iSmpl304 < int.MaxValue; iSmpl304++) {
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar303, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl304); else break;
};

};

C16(p);

C17(p);

IVariable forloop_313 = null;
int counter318 = 0;
for (O.IterateStart(ref forloop_313, i314); O.IterateContinue(forloop_313, i314, d315, null, ref counter318); O.IterateStep(forloop_313, i314, null, counter318))
{;
IVariable ivTmpvar316 = O.FunctionLookup1("add1")(smpl, O.Lookup(smpl, null, null, "%sum", null, null, false));
for (int iSmpl317 = 0; iSmpl317 < int.MaxValue; iSmpl317++) {
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar316, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl317); else break;
};

};

C18(p);

IVariable forloop_329 = null;
int counter331 = 0;
for (O.IterateStart(ref forloop_329, O.Lookup(smpl, null, null, "#m", null, null, false)); O.IterateContinue(forloop_329, O.Lookup(smpl, null, null, "#m", null, null, false), null, null, ref counter331); O.IterateStep(forloop_329, O.Lookup(smpl, null, null, "#m", null, null, false), null, counter331))
{;
for (int iSmpl330 = 0; iSmpl330 < int.MaxValue; iSmpl330++) {
O.Print(smpl, (forloop_329));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl330); else break;
}
};

C19(p);

C20(p);

C21(p);

C22(p);

C23(p);



}
}
}
