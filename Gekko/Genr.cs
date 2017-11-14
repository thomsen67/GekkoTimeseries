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
public static readonly ScalarVal i8 = new ScalarVal(100d);
public static IVariable MapDef_mapTmpvar13(GekkoSmpl smpl) {
Map mapTmpvar13 = new Map();
IVariable ivTmpvar14 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false)));
O.Lookup(smpl, mapTmpvar13, null, "%i1", null, ivTmpvar14, true, EVariableType.Var)
;

IVariable ivTmpvar15 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false)));
O.Lookup(smpl, mapTmpvar13, null, "%i2", null, ivTmpvar15, true, EVariableType.Var)
;

IVariable ivTmpvar16 = O.IvConvertTo(EVariableType.Var, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var));
O.Lookup(smpl, mapTmpvar13, null, "ts", null, ivTmpvar16, true, EVariableType.Var)
;


return mapTmpvar13;
}public static IVariable MapDef_mapTmpvar10(GekkoSmpl smpl) {
Map mapTmpvar10 = new Map();
IVariable ivTmpvar11 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)));
O.Lookup(smpl, mapTmpvar10, null, "%i1", null, ivTmpvar11, true, EVariableType.Var)
;

IVariable ivTmpvar12 = O.IvConvertTo(EVariableType.Var, MapDef_mapTmpvar13(smpl));
O.Lookup(smpl, mapTmpvar10, null, "#mm", null, ivTmpvar12, true, EVariableType.Var)
;


return mapTmpvar10;
}public static readonly ScalarVal i18 = new ScalarVal(101d);
public static readonly ScalarVal i19 = new ScalarVal(102d);
public static readonly ScalarVal i20 = new ScalarVal(103d);
public static readonly ScalarVal i21 = new ScalarVal(104d);
public static readonly ScalarVal i22 = new ScalarVal(105d);
public static readonly ScalarVal i23 = new ScalarVal(106d);
public static readonly ScalarVal i24 = new ScalarVal(107d);
public static readonly ScalarVal i25 = new ScalarVal(108d);
public static readonly ScalarVal i26 = new ScalarVal(109d);
public static readonly ScalarVal i27 = new ScalarVal(110d);
public static readonly ScalarVal i28 = new ScalarVal(111d);
public static readonly ScalarVal i29 = new ScalarVal(1d);
public static readonly ScalarVal i31 = new ScalarVal(100d);
public static readonly ScalarVal i33 = new ScalarVal(2010d);
public static readonly ScalarVal i34 = new ScalarVal(1d);
public static readonly ScalarVal i36 = new ScalarVal(1d);
public static readonly ScalarVal i38 = new ScalarVal(1d);
public static readonly ScalarVal i40 = new ScalarVal(5d);
public static readonly ScalarVal i41 = new ScalarVal(5d);
public static readonly ScalarVal i42 = new ScalarVal(2d);
public static readonly ScalarVal i44 = new ScalarVal(1d);
public static readonly ScalarVal i46 = new ScalarVal(5d);
public static readonly ScalarVal i47 = new ScalarVal(6d);
public static readonly ScalarVal i48 = new ScalarVal(3d);
public static readonly ScalarVal i50 = new ScalarVal(1d);
public static readonly ScalarVal i52 = new ScalarVal(2010d);
public static readonly ScalarVal i53 = new ScalarVal(5d);
public static readonly ScalarVal i54 = new ScalarVal(5d);
public static readonly ScalarVal i55 = new ScalarVal(2d);
public static readonly ScalarVal i57 = new ScalarVal(1d);
public static readonly ScalarVal i59 = new ScalarVal(2010d);
public static readonly ScalarVal i60 = new ScalarVal(5d);
public static readonly ScalarVal i61 = new ScalarVal(6d);
public static readonly ScalarVal i62 = new ScalarVal(3d);
public static readonly ScalarVal i63 = new ScalarVal(1d);
public static readonly ScalarVal i64 = new ScalarVal(1d);
public static readonly ScalarVal i66 = new ScalarVal(0d);
public static readonly ScalarVal i68 = new ScalarVal(2d);
public static readonly ScalarVal i70 = new ScalarVal(0d);
public static readonly ScalarVal i72 = new ScalarVal(1d);
public static readonly ScalarVal i73 = new ScalarVal(1d);
public static readonly ScalarVal i74 = new ScalarVal(0d);
public static readonly ScalarVal i76 = new ScalarVal(0d);
public static readonly ScalarVal i78 = new ScalarVal(2d);
public static readonly ScalarVal i80 = new ScalarVal(0d);
public static readonly ScalarVal i82 = new ScalarVal(1d);
public static readonly ScalarVal i84 = new ScalarVal(1d);
public static readonly ScalarVal i85 = new ScalarVal(0d);
public static readonly ScalarVal i86 = new ScalarVal(1d);
public static readonly ScalarVal i88 = new ScalarVal(1d);
public static readonly ScalarVal i89 = new ScalarVal(2d);
public static readonly ScalarVal i90 = new ScalarVal(3d);
public static readonly ScalarVal i91 = new ScalarVal(4d);
public static readonly ScalarVal i92 = new ScalarVal(5d);
public static readonly ScalarVal i93 = new ScalarVal(6d);
public static readonly ScalarVal i94 = new ScalarVal(7d);
public static readonly ScalarVal i95 = new ScalarVal(5d);
public static readonly ScalarVal i96 = new ScalarVal(5d);
public static readonly ScalarVal i97 = new ScalarVal(10d);
public static readonly ScalarVal i98 = new ScalarVal(5d);
public static readonly ScalarVal i100 = new ScalarVal(100d);
public static readonly ScalarVal i101 = new ScalarVal(5d);
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
public static readonly ScalarVal i115 = new ScalarVal(101d);
public static readonly ScalarVal i116 = new ScalarVal(102d);
public static readonly ScalarVal i117 = new ScalarVal(103d);
public static readonly ScalarVal i118 = new ScalarVal(104d);
public static readonly ScalarVal i119 = new ScalarVal(105d);
public static readonly ScalarVal i120 = new ScalarVal(106d);
public static readonly ScalarVal i121 = new ScalarVal(107d);
public static readonly ScalarVal i122 = new ScalarVal(108d);
public static readonly ScalarVal i123 = new ScalarVal(109d);
public static readonly ScalarVal i124 = new ScalarVal(110d);
public static readonly ScalarVal i125 = new ScalarVal(111d);
public static readonly ScalarVal i127 = new ScalarVal(5d);
public static readonly ScalarVal i129 = new ScalarVal(1d);
public static readonly ScalarVal i130 = new ScalarVal(2d);
public static readonly ScalarVal i131 = new ScalarVal(3d);
public static readonly ScalarVal i132 = new ScalarVal(4d);
public static readonly ScalarVal i133 = new ScalarVal(5d);
public static readonly ScalarVal i134 = new ScalarVal(6d);
public static readonly ScalarVal i135 = new ScalarVal(7d);
public static readonly ScalarVal i136 = new ScalarVal(5d);
public static readonly ScalarVal i137 = new ScalarVal(5d);
public static readonly ScalarVal i138 = new ScalarVal(10d);
public static readonly ScalarVal i139 = new ScalarVal(5d);
public static readonly ScalarVal i141 = new ScalarVal(1d);
public static readonly ScalarVal i142 = new ScalarVal(2d);
public static readonly ScalarVal i143 = new ScalarVal(3d);
public static readonly ScalarVal i144 = new ScalarVal(4d);
public static readonly ScalarVal i146 = new ScalarVal(1d);
public static readonly ScalarVal i147 = new ScalarVal(2d);
public static readonly ScalarVal i148 = new ScalarVal(100d);
public static readonly ScalarVal i149 = new ScalarVal(200d);
public static readonly ScalarVal i151 = new ScalarVal(1d);
public static readonly ScalarVal i152 = new ScalarVal(2d);
public static readonly ScalarVal i153 = new ScalarVal(3d);
public static readonly ScalarVal i154 = new ScalarVal(4d);
public static readonly ScalarVal i156 = new ScalarVal(1d);
public static readonly ScalarVal i157 = new ScalarVal(2d);
public static readonly ScalarVal i158 = new ScalarVal(1d);
public static readonly ScalarVal i159 = new ScalarVal(1d);
public static readonly ScalarVal i160 = new ScalarVal(2d);
public static readonly ScalarVal i161 = new ScalarVal(2d);
public static readonly ScalarVal i163 = new ScalarVal(1d);
public static readonly ScalarVal i164 = new ScalarVal(2d);
public static readonly ScalarVal i165 = new ScalarVal(3d);
public static readonly ScalarVal i166 = new ScalarVal(2d);
public static readonly ScalarVal i167 = new ScalarVal(3d);
public static readonly ScalarVal i169 = new ScalarVal(2d);
public static readonly ScalarVal i170 = new ScalarVal(3d);
public static readonly ScalarVal i171 = new ScalarVal(20d);
public static readonly ScalarVal i172 = new ScalarVal(30d);
public static readonly ScalarVal i178 = new ScalarVal(100d);
public static readonly ScalarVal i179 = new ScalarVal(1d);
public static IVariable MapDef_mapTmpvar175(GekkoSmpl smpl) {
Map mapTmpvar175 = new Map();
IVariable ivTmpvar176 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)));
O.Lookup(smpl, mapTmpvar175, null, "%i1", null, ivTmpvar176, true, EVariableType.Var)
;

IVariable ivTmpvar177 = O.IvConvertTo(EVariableType.Var, O.Add(smpl, i178, i179));
O.Lookup(smpl, mapTmpvar175, null, "%v", null, ivTmpvar177, true, EVariableType.Var)
;


return mapTmpvar175;
}public static IVariable MapDef_mapTmpvar185(GekkoSmpl smpl) {
Map mapTmpvar185 = new Map();
IVariable ivTmpvar186 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false)));
O.Lookup(smpl, mapTmpvar185, null, "%i1", null, ivTmpvar186, true, EVariableType.Var)
;

IVariable ivTmpvar187 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false)));
O.Lookup(smpl, mapTmpvar185, null, "%i2", null, ivTmpvar187, true, EVariableType.Var)
;


return mapTmpvar185;
}public static IVariable MapDef_mapTmpvar182(GekkoSmpl smpl) {
Map mapTmpvar182 = new Map();
IVariable ivTmpvar183 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)));
O.Lookup(smpl, mapTmpvar182, null, "%i1", null, ivTmpvar183, true, EVariableType.Var)
;

IVariable ivTmpvar184 = O.IvConvertTo(EVariableType.Var, MapDef_mapTmpvar185(smpl));
O.Lookup(smpl, mapTmpvar182, null, "#m", null, ivTmpvar184, true, EVariableType.Var)
;


return mapTmpvar182;
}public static readonly ScalarVal i189 = new ScalarVal(1d);
public static readonly ScalarVal i190 = new ScalarVal(2d);
public static readonly ScalarVal i191 = new ScalarVal(3d);
public static readonly ScalarVal i192 = new ScalarVal(4d);
public static readonly ScalarVal i193 = new ScalarVal(1d);
public static readonly ScalarVal i194 = new ScalarVal(2d);
public static readonly ScalarVal i196 = new ScalarVal(2d);
public static readonly ScalarVal i197 = new ScalarVal(1d);
public static readonly ScalarVal i198 = new ScalarVal(1d);
public static readonly ScalarVal i199 = new ScalarVal(10d);
public static readonly ScalarVal i200 = new ScalarVal(2d);
public static readonly ScalarVal i201 = new ScalarVal(2d);
public static readonly ScalarVal i202 = new ScalarVal(1d);
public static readonly ScalarVal i203 = new ScalarVal(1d);
public static readonly ScalarVal i206 = new ScalarVal(1d);
public static readonly ScalarVal i207 = new ScalarVal(2d);
public static readonly ScalarVal i208 = new ScalarVal(1d);
public static readonly ScalarVal i209 = new ScalarVal(2d);
public static readonly ScalarVal i219 = new ScalarVal(0d);
public static readonly ScalarVal i221 = new ScalarVal(1d);
public static readonly ScalarVal d222 = new ScalarVal(1e3d);
public static readonly ScalarVal i226 = new ScalarVal(0d);
public static readonly ScalarVal i228 = new ScalarVal(1d);
public static readonly ScalarVal d229 = new ScalarVal(1e3d);
public static readonly ScalarVal i233 = new ScalarVal(1d);
public static void FunctionDef234() {


//[[splitSTOP]]

O.PrepareUfunction(1, "add1");

Globals.ufunctions1.Add("add1", (GekkoSmpl smpl, P p, IVariable functionarg_232) => { p.SetText(@"¤162"); O.InitSmpl(smpl);

//[[splitSTOP]]
return O.Add(smpl, functionarg_232, i233);

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i236 = new ScalarVal(0d);
public static readonly ScalarVal i238 = new ScalarVal(1d);
public static readonly ScalarVal d239 = new ScalarVal(1e3d);
public static void FunctionDef244() {


//[[splitSTOP]]

O.PrepareUfunction(2, "add2");

Globals.ufunctions2.Add("add2", (GekkoSmpl smpl, P p, IVariable functionarg_242, IVariable functionarg_243) => { p.SetText(@"¤169"); O.InitSmpl(smpl);

//[[splitSTOP]]
return O.ListDefHelper(functionarg_242, O.Add(smpl, functionarg_242, functionarg_243));

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i245 = new ScalarVal(10d);
public static readonly ScalarVal i246 = new ScalarVal(20d);
public static readonly ScalarVal i247 = new ScalarVal(2d);
public static void FunctionDef252() {


//[[splitSTOP]]

O.PrepareUfunction(1, "f");

Globals.ufunctions1.Add("f", (GekkoSmpl smpl, P p, IVariable functionarg_251) => { p.SetText(@"¤179"); O.InitSmpl(smpl);

//[[splitSTOP]]
return O.Add(smpl, functionarg_251, functionarg_251);

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i254 = new ScalarVal(100d);
public static readonly ScalarVal i257 = new ScalarVal(2d);
public static readonly ScalarVal i259 = new ScalarVal(1d);
public static readonly ScalarVal i261 = new ScalarVal(2d);
public static readonly ScalarVal i263 = new ScalarVal(3d);
public static readonly ScalarVal i265 = new ScalarVal(4d);
public static readonly ScalarVal i267 = new ScalarVal(2010d);
public static readonly ScalarVal i268 = new ScalarVal(1000d);
public static IVariable temp273(GekkoSmpl smpl) {
Series temp273 = new Series(ESeriesType.Normal, Program.options.freq, null); temp273.SetZero(smpl);

foreach (IVariable listloop_m1271 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null, false, EVariableType.Var))) {
foreach (IVariable listloop_m2272 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null, false, EVariableType.Var))) {
temp273.InjectAdd(smpl, temp273, O.Indexer(O.Indexer2(smpl, listloop_m1271, listloop_m2272), smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), listloop_m1271, listloop_m2272));

}
}
return temp273;

}
public static readonly ScalarVal i276 = new ScalarVal(0d);
public static IVariable temp277(GekkoSmpl smpl) {
List temp277 = new List();

foreach (IVariable listloop_m1274 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null, false, EVariableType.Var))) {
foreach (IVariable listloop_m2275 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null, false, EVariableType.Var))) {
temp277.Add(O.Add(smpl, O.Indexer(O.Indexer2(smpl, listloop_m1274, listloop_m2275), smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), listloop_m1274, listloop_m2275), i276));

}
}
return temp277;

}
public static IVariable MapDef_mapTmpvar279(GekkoSmpl smpl) {
Map mapTmpvar279 = new Map();
IVariable ivTmpvar280 = O.IvConvertTo(EVariableType.Var, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var));
O.Lookup(smpl, mapTmpvar279, null, "ts", null, ivTmpvar280, true, EVariableType.Var)
;


return mapTmpvar279;
}public static IVariable MapDef_mapTmpvar282(GekkoSmpl smpl) {
Map mapTmpvar282 = new Map();
IVariable ivTmpvar283 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"Hej", true, false)));
O.Lookup(smpl, mapTmpvar282, null, "%s", null, ivTmpvar283, true, EVariableType.Var)
;

IVariable ivTmpvar284 = O.IvConvertTo(EVariableType.Var, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var));
O.Lookup(smpl, mapTmpvar282, null, "#m5", null, ivTmpvar284, true, EVariableType.Var)
;


return mapTmpvar282;
}public static IVariable temp287(GekkoSmpl smpl) {
Series temp287 = new Series(ESeriesType.Normal, Program.options.freq, null); temp287.SetZero(smpl);

foreach (IVariable listloop_m1285 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null, false, EVariableType.Var))) {
foreach (IVariable listloop_m2286 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null, false, EVariableType.Var))) {
temp287.InjectAdd(smpl, temp287, O.Indexer(O.Indexer2(smpl, listloop_m1285, listloop_m2286), smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), listloop_m1285, listloop_m2286));

}
}
return temp287;

}
public static readonly ScalarVal i290 = new ScalarVal(0d);
public static IVariable temp291(GekkoSmpl smpl) {
List temp291 = new List();

foreach (IVariable listloop_m1288 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null, false, EVariableType.Var))) {
foreach (IVariable listloop_m2289 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null, false, EVariableType.Var))) {
temp291.Add(O.Add(smpl, O.Indexer(O.Indexer2(smpl, listloop_m1288, listloop_m2289), smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), listloop_m1288, listloop_m2289), i290));

}
}
return temp291;

}
public static readonly ScalarVal i293 = new ScalarVal(1d);
public static readonly ScalarVal i295 = new ScalarVal(2d);
public static readonly ScalarVal i297 = new ScalarVal(3d);
public static readonly ScalarVal i299 = new ScalarVal(4d);
public static readonly ScalarVal i301 = new ScalarVal(2010d);
public static readonly ScalarVal i302 = new ScalarVal(1000d);
public static IVariable temp307(GekkoSmpl smpl) {
Series temp307 = new Series(ESeriesType.Normal, Program.options.freq, null); temp307.SetZero(smpl);

foreach (IVariable listloop_m1305 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null, false, EVariableType.Var))) {
foreach (IVariable listloop_m2306 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null, false, EVariableType.Var))) {
temp307.InjectAdd(smpl, temp307, O.Lookup(smpl, null, (new ScalarString("xx", true, false)).Add(smpl, listloop_m1305).Add(smpl, listloop_m2306), null, false, EVariableType.Var));

}
}
return temp307;

}
public static readonly ScalarVal i310 = new ScalarVal(0d);
public static IVariable temp311(GekkoSmpl smpl) {
List temp311 = new List();

foreach (IVariable listloop_m1308 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null, false, EVariableType.Var))) {
foreach (IVariable listloop_m2309 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null, false, EVariableType.Var))) {
temp311.Add(O.Add(smpl, O.Lookup(smpl, null, (new ScalarString("xx", true, false)).Add(smpl, listloop_m1308).Add(smpl, listloop_m2309), null, false, EVariableType.Var), i310));

}
}
return temp311;

}
public static readonly ScalarVal i314 = new ScalarVal(0d);
public static IVariable temp315(GekkoSmpl smpl) {
List temp315 = new List();

foreach (IVariable listloop_m313 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), null, false, EVariableType.Var))) {
temp315.Add(O.Add(smpl, O.Lookup(smpl, null, (listloop_m313), null, false, EVariableType.Var), i314));

}
return temp315;

}
public static IVariable temp318(GekkoSmpl smpl) {
Series temp318 = new Series(ESeriesType.Normal, Program.options.freq, null); temp318.SetZero(smpl);

foreach (IVariable listloop_m1316 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null, false, EVariableType.Var))) {
foreach (IVariable listloop_m2317 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null, false, EVariableType.Var))) {
temp318.InjectAdd(smpl, temp318, O.Lookup(smpl, null, (new ScalarString("xx", true, false)).Add(smpl, listloop_m1316).Add(smpl, listloop_m2317), null, false, EVariableType.Var));

}
}
return temp318;

}
public static readonly ScalarVal i321 = new ScalarVal(0d);
public static IVariable temp322(GekkoSmpl smpl) {
List temp322 = new List();

foreach (IVariable listloop_m1319 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null, false, EVariableType.Var))) {
foreach (IVariable listloop_m2320 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null, false, EVariableType.Var))) {
temp322.Add(O.Add(smpl, O.Lookup(smpl, null, (new ScalarString("xx", true, false)).Add(smpl, listloop_m1319).Add(smpl, listloop_m2320), null, false, EVariableType.Var), i321));

}
}
return temp322;

}
public static readonly ScalarVal i324 = new ScalarVal(0d);
public static IVariable temp325(GekkoSmpl smpl) {
List temp325 = new List();

foreach (IVariable listloop_m323 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), null, false, EVariableType.Var))) {
temp325.Add(O.Add(smpl, O.Lookup(smpl, null, (listloop_m323), null, false, EVariableType.Var), i324));

}
return temp325;

}
public static readonly ScalarVal i327 = new ScalarVal(2d);
public static readonly ScalarVal i329 = new ScalarVal(5d);
public static readonly ScalarVal i331 = new ScalarVal(2010d);
public static readonly ScalarVal i332 = new ScalarVal(50d);
public static readonly ScalarVal i334 = new ScalarVal(6d);
public static readonly ScalarVal i336 = new ScalarVal(100d);
public static readonly ScalarVal i339 = new ScalarVal(1000d);
public static readonly ScalarVal i341 = new ScalarVal(1d);
public static readonly ScalarVal i343 = new ScalarVal(300d);
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

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar7 = O.IvConvertTo(EVariableType.Var, i8);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar7, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar9 = O.IvConvertTo(EVariableType.Var, MapDef_mapTmpvar10(smpl));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar9, true, EVariableType.Var)
;

p.SetText(@"¤41"); O.InitSmpl(smpl);
O.Prt o3 = new O.Prt();
o3.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope3 = new O.Prt.Element();
ope3.label = O.SubstituteScalarsAndLists("#m.%i1", false);
smpl = new GekkoSmpl(o3.t1.Add(-2), o3.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o3, ope3));
foreach(int bankNumber in bankNumbers) {
ope3.subElements = new List<O.Prt.SubElement>();
ope3.subElements.Add(new O.Prt.SubElement());
ope3.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))));
}
o3.prtElements.Add(ope3);
}
smpl = null;

o3.counter = 1;
o3.Exe();

p.SetText(@"¤42"); O.InitSmpl(smpl);
O.Prt o4 = new O.Prt();
o4.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope4 = new O.Prt.Element();
ope4.label = O.SubstituteScalarsAndLists("#m.#mm.%i1", false);
smpl = new GekkoSmpl(o4.t1.Add(-2), o4.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o4, ope4));
foreach(int bankNumber in bankNumbers) {
ope4.subElements = new List<O.Prt.SubElement>();
ope4.subElements.Add(new O.Prt.SubElement());
ope4.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false)))), smpl, O.Indexer(O.Indexer2(smpl, (O.scalarStringHash).Add(smpl, (new ScalarString("mm", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringHash).Add(smpl, (new ScalarString("mm", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))));
}
o4.prtElements.Add(ope4);
}
smpl = null;

o4.counter = 2;
o4.Exe();

p.SetText(@"¤43"); O.InitSmpl(smpl);
O.Prt o5 = new O.Prt();
o5.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope5 = new O.Prt.Element();
ope5.label = O.SubstituteScalarsAndLists("#m.#mm.%i2", false);
smpl = new GekkoSmpl(o5.t1.Add(-2), o5.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o5, ope5));
foreach(int bankNumber in bankNumbers) {
ope5.subElements = new List<O.Prt.SubElement>();
ope5.subElements.Add(new O.Prt.SubElement());
ope5.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false)))), smpl, O.Indexer(O.Indexer2(smpl, (O.scalarStringHash).Add(smpl, (new ScalarString("mm", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringHash).Add(smpl, (new ScalarString("mm", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))));
}
o5.prtElements.Add(ope5);
}
smpl = null;

o5.counter = 3;
o5.Exe();

p.SetText(@"¤44"); O.InitSmpl(smpl);
O.Prt o6 = new O.Prt();
o6.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope6 = new O.Prt.Element();
ope6.label = O.SubstituteScalarsAndLists("#m.#mm.ts", false);
smpl = new GekkoSmpl(o6.t1.Add(-2), o6.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o6, ope6));
foreach(int bankNumber in bankNumbers) {
ope6.subElements = new List<O.Prt.SubElement>();
ope6.subElements.Add(new O.Prt.SubElement());
ope6.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (new ScalarString("ts", true, false))), smpl, O.Indexer(O.Indexer2(smpl, (O.scalarStringHash).Add(smpl, (new ScalarString("mm", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringHash).Add(smpl, (new ScalarString("mm", true, false)))), (new ScalarString("ts", true, false)));
}
o6.prtElements.Add(ope6);
}
smpl = null;

o6.counter = 4;
o6.Exe();

p.SetText(@"¤45"); O.InitSmpl(smpl);
O.Write o7 = new O.Write();

o7.fileName = O.ConvertToString((new ScalarString("slet", true, false)));

o7.type = @"write";o7.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
O.Reset o8 = new O.Reset();
o8.p = p;o8.Exe(smpl);

p.SetText(@"¤47"); O.InitSmpl(smpl);
ClearTS(p);
O.Read o9 = new O.Read();
o9.p = p;
o9.type = @"read";
o9.fileName = O.ConvertToString((new ScalarString("slet", true, false)));


o9.Exe();

p.SetText(@"¤48"); O.InitSmpl(smpl);
O.Prt o10 = new O.Prt();
o10.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope10 = new O.Prt.Element();
ope10.label = O.SubstituteScalarsAndLists("#m.%i1", false);
smpl = new GekkoSmpl(o10.t1.Add(-2), o10.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o10, ope10));
foreach(int bankNumber in bankNumbers) {
ope10.subElements = new List<O.Prt.SubElement>();
ope10.subElements.Add(new O.Prt.SubElement());
ope10.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))));
}
o10.prtElements.Add(ope10);
}
smpl = null;

o10.counter = 5;
o10.Exe();

p.SetText(@"¤49"); O.InitSmpl(smpl);
O.Prt o11 = new O.Prt();
o11.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope11 = new O.Prt.Element();
ope11.label = O.SubstituteScalarsAndLists("#m.#mm.%i1", false);
smpl = new GekkoSmpl(o11.t1.Add(-2), o11.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o11, ope11));
foreach(int bankNumber in bankNumbers) {
ope11.subElements = new List<O.Prt.SubElement>();
ope11.subElements.Add(new O.Prt.SubElement());
ope11.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false)))), smpl, O.Indexer(O.Indexer2(smpl, (O.scalarStringHash).Add(smpl, (new ScalarString("mm", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringHash).Add(smpl, (new ScalarString("mm", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))));
}
o11.prtElements.Add(ope11);
}
smpl = null;

o11.counter = 6;
o11.Exe();

p.SetText(@"¤50"); O.InitSmpl(smpl);
O.Prt o12 = new O.Prt();
o12.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope12 = new O.Prt.Element();
ope12.label = O.SubstituteScalarsAndLists("#m.#mm.%i2", false);
smpl = new GekkoSmpl(o12.t1.Add(-2), o12.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o12, ope12));
foreach(int bankNumber in bankNumbers) {
ope12.subElements = new List<O.Prt.SubElement>();
ope12.subElements.Add(new O.Prt.SubElement());
ope12.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false)))), smpl, O.Indexer(O.Indexer2(smpl, (O.scalarStringHash).Add(smpl, (new ScalarString("mm", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringHash).Add(smpl, (new ScalarString("mm", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))));
}
o12.prtElements.Add(ope12);
}
smpl = null;

o12.counter = 7;
o12.Exe();

p.SetText(@"¤51"); O.InitSmpl(smpl);
O.Prt o13 = new O.Prt();
o13.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope13 = new O.Prt.Element();
ope13.label = O.SubstituteScalarsAndLists("#m.#mm.ts", false);
smpl = new GekkoSmpl(o13.t1.Add(-2), o13.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o13, ope13));
foreach(int bankNumber in bankNumbers) {
ope13.subElements = new List<O.Prt.SubElement>();
ope13.subElements.Add(new O.Prt.SubElement());
ope13.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (new ScalarString("ts", true, false))), smpl, O.Indexer(O.Indexer2(smpl, (O.scalarStringHash).Add(smpl, (new ScalarString("mm", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringHash).Add(smpl, (new ScalarString("mm", true, false)))), (new ScalarString("ts", true, false)));
}
o13.prtElements.Add(ope13);
}
smpl = null;

o13.counter = 8;
o13.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar17 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(i18, i19, i20, i21, i22, i23, i24, i25, i26, i27, i28));
O.Lookup(smpl, null, null, "xx", null, ivTmpvar17, true, EVariableType.Var)
;

p.SetText(@"¤56"); O.InitSmpl(smpl);
O.Prt o15 = new O.Prt();
o15.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope15 = new O.Prt.Element();
ope15.label = O.SubstituteScalarsAndLists("(xx+xx)[-1]", false);
smpl = new GekkoSmpl(o15.t1.Add(-2), o15.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o15, ope15));
foreach(int bankNumber in bankNumbers) {
ope15.subElements = new List<O.Prt.SubElement>();
ope15.subElements.Add(new O.Prt.SubElement());
ope15.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, O.Negate(smpl, i29)
), smpl, O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var)), O.Negate(smpl, i29)
);
}
o15.prtElements.Add(ope15);
}
smpl = null;

o15.counter = 9;
o15.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar30 = O.IvConvertTo(EVariableType.Var, i31);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar30, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar32 = O.IvConvertTo(EVariableType.Var, i34);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar32, i33
)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar35 = O.IvConvertTo(EVariableType.Var, i36);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar35, new ScalarDate(G.FromStringToDate("2012a1"))
)
;

p.SetText(@"¤61"); O.InitSmpl(smpl);
O.Prt o19 = new O.Prt();
o19.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope19 = new O.Prt.Element();
ope19.label = O.SubstituteScalarsAndLists("xx", false);
smpl = new GekkoSmpl(o19.t1.Add(-2), o19.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o19, ope19));
foreach(int bankNumber in bankNumbers) {
ope19.subElements = new List<O.Prt.SubElement>();
ope19.subElements.Add(new O.Prt.SubElement());
ope19.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var);
}
o19.prtElements.Add(ope19);
}
smpl = null;

o19.counter = 10;
o19.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar37 = O.IvConvertTo(EVariableType.Var, i38);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar37, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar39 = O.IvConvertTo(EVariableType.Var, i42);
O.DollarLookup(O.Equals(smpl, i40,i41)
, smpl, null, null, "xx", null, ivTmpvar39, false, EVariableType.Var)
;

p.SetText(@"¤66"); O.InitSmpl(smpl);
O.Prt o22 = new O.Prt();
o22.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope22 = new O.Prt.Element();
ope22.label = O.SubstituteScalarsAndLists("xx", false);
smpl = new GekkoSmpl(o22.t1.Add(-2), o22.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o22, ope22));
foreach(int bankNumber in bankNumbers) {
ope22.subElements = new List<O.Prt.SubElement>();
ope22.subElements.Add(new O.Prt.SubElement());
ope22.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var);
}
o22.prtElements.Add(ope22);
}
smpl = null;

o22.counter = 11;
o22.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar43 = O.IvConvertTo(EVariableType.Var, i44);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar43, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar45 = O.IvConvertTo(EVariableType.Var, i48);
O.DollarLookup(O.Equals(smpl, i46,i47)
, smpl, null, null, "xx", null, ivTmpvar45, false, EVariableType.Var)
;

p.SetText(@"¤68"); O.InitSmpl(smpl);
O.Prt o25 = new O.Prt();
o25.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope25 = new O.Prt.Element();
ope25.label = O.SubstituteScalarsAndLists("xx", false);
smpl = new GekkoSmpl(o25.t1.Add(-2), o25.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o25, ope25));
foreach(int bankNumber in bankNumbers) {
ope25.subElements = new List<O.Prt.SubElement>();
ope25.subElements.Add(new O.Prt.SubElement());
ope25.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var);
}
o25.prtElements.Add(ope25);
}
smpl = null;

o25.counter = 12;
o25.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar49 = O.IvConvertTo(EVariableType.Var, i50);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar49, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar51 = O.IvConvertTo(EVariableType.Var, i55);
O.DollarIndexerSetData(O.Equals(smpl, i53,i54)
, smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var),  ivTmpvar51, i52
)
;

p.SetText(@"¤70"); O.InitSmpl(smpl);
O.Prt o28 = new O.Prt();
o28.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope28 = new O.Prt.Element();
ope28.label = O.SubstituteScalarsAndLists("xx", false);
smpl = new GekkoSmpl(o28.t1.Add(-2), o28.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o28, ope28));
foreach(int bankNumber in bankNumbers) {
ope28.subElements = new List<O.Prt.SubElement>();
ope28.subElements.Add(new O.Prt.SubElement());
ope28.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var);
}
o28.prtElements.Add(ope28);
}
smpl = null;

o28.counter = 13;
o28.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar56 = O.IvConvertTo(EVariableType.Var, i57);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar56, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar58 = O.IvConvertTo(EVariableType.Var, i62);
O.DollarIndexerSetData(O.Equals(smpl, i60,i61)
, smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var),  ivTmpvar58, i59
)
;

p.SetText(@"¤72"); O.InitSmpl(smpl);
O.Prt o31 = new O.Prt();
o31.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope31 = new O.Prt.Element();
ope31.label = O.SubstituteScalarsAndLists("xx", false);
smpl = new GekkoSmpl(o31.t1.Add(-2), o31.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o31, ope31));
foreach(int bankNumber in bankNumbers) {
ope31.subElements = new List<O.Prt.SubElement>();
ope31.subElements.Add(new O.Prt.SubElement());
ope31.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var);
}
o31.prtElements.Add(ope31);
}
smpl = null;

o31.counter = 14;
o31.Exe();

p.SetText(@"¤75"); O.InitSmpl(smpl);

//[[splitSTOP]]
if(O.IsTrue(smpl, O.Equals(smpl, i63,i64))) {
//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar65 = O.IvConvertTo(EVariableType.Var, i66);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar65, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar67 = O.IvConvertTo(EVariableType.Var, i68);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar67, true, EVariableType.Var)
;


//[[splitSTOP]]
}else {
//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar69 = O.IvConvertTo(EVariableType.Var, i70);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar69, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar71 = O.IvConvertTo(EVariableType.Var, i72);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar71, true, EVariableType.Var)
;


//[[splitSTOP]]
}
//[[splitSTART]]

p.SetText(@"¤76"); O.InitSmpl(smpl);
O.Prt o37 = new O.Prt();
o37.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope37 = new O.Prt.Element();
ope37.label = O.SubstituteScalarsAndLists("xx", false);
smpl = new GekkoSmpl(o37.t1.Add(-2), o37.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o37, ope37));
foreach(int bankNumber in bankNumbers) {
ope37.subElements = new List<O.Prt.SubElement>();
ope37.subElements.Add(new O.Prt.SubElement());
ope37.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var);
}
o37.prtElements.Add(ope37);
}
smpl = null;

o37.counter = 15;
o37.Exe();

p.SetText(@"¤78"); O.InitSmpl(smpl);

//[[splitSTOP]]
if(O.IsTrue(smpl, O.Equals(smpl, i73,i74))) {
//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar75 = O.IvConvertTo(EVariableType.Var, i76);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar75, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar77 = O.IvConvertTo(EVariableType.Var, i78);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar77, true, EVariableType.Var)
;


//[[splitSTOP]]
}else {
//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar79 = O.IvConvertTo(EVariableType.Var, i80);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar79, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar81 = O.IvConvertTo(EVariableType.Var, i82);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar81, true, EVariableType.Var)
;


//[[splitSTOP]]
}
//[[splitSTART]]

p.SetText(@"¤79"); O.InitSmpl(smpl);
O.Prt o43 = new O.Prt();
o43.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope43 = new O.Prt.Element();
ope43.label = O.SubstituteScalarsAndLists("xx", false);
smpl = new GekkoSmpl(o43.t1.Add(-2), o43.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o43, ope43));
foreach(int bankNumber in bankNumbers) {
ope43.subElements = new List<O.Prt.SubElement>();
ope43.subElements.Add(new O.Prt.SubElement());
ope43.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var);
}
o43.prtElements.Add(ope43);
}
smpl = null;

o43.counter = 16;
o43.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar83 = O.IvConvertTo(EVariableType.Var, O.Dollar(smpl, i84, O.Equals(smpl, i85,i86)
));
O.Lookup(smpl, null, null, "%v", null, ivTmpvar83, true, EVariableType.Var)
;

p.SetText(@"¤82"); O.InitSmpl(smpl);
O.Prt o45 = new O.Prt();
o45.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope45 = new O.Prt.Element();
ope45.label = O.SubstituteScalarsAndLists("%v", false);
smpl = new GekkoSmpl(o45.t1.Add(-2), o45.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o45, ope45));
foreach(int bankNumber in bankNumbers) {
ope45.subElements = new List<O.Prt.SubElement>();
ope45.subElements.Add(new O.Prt.SubElement());
ope45.subElements[0].tsWork = O.Lookup(smpl, null, null, "%v", null, null, false, EVariableType.Var);
}
o45.prtElements.Add(ope45);
}
smpl = null;

o45.counter = 17;
o45.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar87 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(i88, i89, i90, i91, i92, i93, i94, i95, i96, i97, i98));
O.Lookup(smpl, null, null, "xx1", null, ivTmpvar87, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar99 = O.IvConvertTo(EVariableType.Var, O.Dollar(smpl, i100, O.Equals(smpl, O.Lookup(smpl, null, null, "xx1", null, null, false, EVariableType.Var),i101)
));
O.Lookup(smpl, null, null, "xx", null, ivTmpvar99, true, EVariableType.Var)
;

p.SetText(@"¤87"); O.InitSmpl(smpl);
O.Prt o48 = new O.Prt();
o48.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope48 = new O.Prt.Element();
ope48.label = O.SubstituteScalarsAndLists("xx", false);
smpl = new GekkoSmpl(o48.t1.Add(-2), o48.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o48, ope48));
foreach(int bankNumber in bankNumbers) {
ope48.subElements = new List<O.Prt.SubElement>();
ope48.subElements.Add(new O.Prt.SubElement());
ope48.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var);
}
o48.prtElements.Add(ope48);
}
smpl = null;

o48.counter = 18;
o48.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar102 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(i103, i104, i105, i106, i107, i108, i109, i110, i111, i112, i113));
O.Lookup(smpl, null, null, "xx1", null, ivTmpvar102, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar114 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(i115, i116, i117, i118, i119, i120, i121, i122, i123, i124, i125));
O.Lookup(smpl, null, null, "xx2", null, ivTmpvar114, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar126 = O.IvConvertTo(EVariableType.Var, O.Dollar(smpl, O.Lookup(smpl, null, null, "xx2", null, null, false, EVariableType.Var), O.Equals(smpl, O.Lookup(smpl, null, null, "xx1", null, null, false, EVariableType.Var),i127)
));
O.Lookup(smpl, null, null, "xx", null, ivTmpvar126, true, EVariableType.Var)
;

p.SetText(@"¤92"); O.InitSmpl(smpl);
O.Prt o52 = new O.Prt();
o52.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope52 = new O.Prt.Element();
ope52.label = O.SubstituteScalarsAndLists("xx", false);
smpl = new GekkoSmpl(o52.t1.Add(-2), o52.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o52, ope52));
foreach(int bankNumber in bankNumbers) {
ope52.subElements = new List<O.Prt.SubElement>();
ope52.subElements.Add(new O.Prt.SubElement());
ope52.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var);
}
o52.prtElements.Add(ope52);
}
smpl = null;

o52.counter = 19;
o52.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar128 = O.IvConvertTo(EVariableType.Var, O.MatrixCol(O.MatrixRow(i129), O.MatrixRow(i130), O.MatrixRow(i131), O.MatrixRow(i132), O.MatrixRow(i133), O.MatrixRow(i134), O.MatrixRow(i135), O.MatrixRow(i136), O.MatrixRow(i137), O.MatrixRow(i138), O.MatrixRow(i139)));
O.Lookup(smpl, null, null, "xx", null, ivTmpvar128, true, EVariableType.Var)
;

p.SetText(@"¤95"); O.InitSmpl(smpl);
O.Prt o54 = new O.Prt();
o54.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope54 = new O.Prt.Element();
ope54.label = O.SubstituteScalarsAndLists("xx", false);
smpl = new GekkoSmpl(o54.t1.Add(-2), o54.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o54, ope54));
foreach(int bankNumber in bankNumbers) {
ope54.subElements = new List<O.Prt.SubElement>();
ope54.subElements.Add(new O.Prt.SubElement());
ope54.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var);
}
o54.prtElements.Add(ope54);
}
smpl = null;

o54.counter = 20;
o54.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar140 = O.IvConvertTo(EVariableType.Var, O.MatrixCol(O.MatrixRow(i141, i142), O.MatrixRow(i143, i144)));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar140, true, EVariableType.Var)
;

p.SetText(@"¤99"); O.InitSmpl(smpl);
O.Prt o56 = new O.Prt();
o56.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope56 = new O.Prt.Element();
ope56.label = O.SubstituteScalarsAndLists("#m", false);
smpl = new GekkoSmpl(o56.t1.Add(-2), o56.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o56, ope56));
foreach(int bankNumber in bankNumbers) {
ope56.subElements = new List<O.Prt.SubElement>();
ope56.subElements.Add(new O.Prt.SubElement());
ope56.subElements[0].tsWork = O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var);
}
o56.prtElements.Add(ope56);
}
smpl = null;

o56.counter = 21;
o56.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar145 = O.IvConvertTo(EVariableType.Var, O.MatrixCol(O.MatrixRow(i148), O.MatrixRow(i149)));
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null, true, EVariableType.Var),  ivTmpvar145, (new Range(i146, i147)))
;

p.SetText(@"¤101"); O.InitSmpl(smpl);
O.Prt o58 = new O.Prt();
o58.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope58 = new O.Prt.Element();
ope58.label = O.SubstituteScalarsAndLists("#m", false);
smpl = new GekkoSmpl(o58.t1.Add(-2), o58.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o58, ope58));
foreach(int bankNumber in bankNumbers) {
ope58.subElements = new List<O.Prt.SubElement>();
ope58.subElements.Add(new O.Prt.SubElement());
ope58.subElements[0].tsWork = O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var);
}
o58.prtElements.Add(ope58);
}
smpl = null;

o58.counter = 22;
o58.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar150 = O.IvConvertTo(EVariableType.Var, O.MatrixCol(O.MatrixRow(i151, i152), O.MatrixRow(i153, i154)));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar150, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar155 = O.IvConvertTo(EVariableType.Var, O.Indexer(O.Indexer2(smpl, (new Range(i159, i160)), i161
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (new Range(i159, i160)), i161
));
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null, true, EVariableType.Var),  ivTmpvar155, (new Range(i156, i157)), i158
)
;

p.SetText(@"¤104"); O.InitSmpl(smpl);
O.Prt o61 = new O.Prt();
o61.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope61 = new O.Prt.Element();
ope61.label = O.SubstituteScalarsAndLists("#m", false);
smpl = new GekkoSmpl(o61.t1.Add(-2), o61.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o61, ope61));
foreach(int bankNumber in bankNumbers) {
ope61.subElements = new List<O.Prt.SubElement>();
ope61.subElements.Add(new O.Prt.SubElement());
ope61.subElements[0].tsWork = O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var);
}
o61.prtElements.Add(ope61);
}
smpl = null;

o61.counter = 23;
o61.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar162 = O.IvConvertTo(EVariableType.Var, O.MatrixCol(O.MatrixRow(i163), O.MatrixRow(i164), O.MatrixRow(i165)));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar162, true, EVariableType.Var)
;

p.SetText(@"¤106"); O.InitSmpl(smpl);
O.Prt o63 = new O.Prt();
o63.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope63 = new O.Prt.Element();
ope63.label = O.SubstituteScalarsAndLists("#m[2..3]", false);
smpl = new GekkoSmpl(o63.t1.Add(-2), o63.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o63, ope63));
foreach(int bankNumber in bankNumbers) {
ope63.subElements = new List<O.Prt.SubElement>();
ope63.subElements.Add(new O.Prt.SubElement());
ope63.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (new Range(i166, i167))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (new Range(i166, i167)));
}
o63.prtElements.Add(ope63);
}
smpl = null;

o63.counter = 24;
o63.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar168 = O.IvConvertTo(EVariableType.Var, O.MatrixCol(O.MatrixRow(i171), O.MatrixRow(i172)));
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null, true, EVariableType.Var),  ivTmpvar168, (new Range(i169, i170)))
;

p.SetText(@"¤108"); O.InitSmpl(smpl);
O.Prt o65 = new O.Prt();
o65.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope65 = new O.Prt.Element();
ope65.label = O.SubstituteScalarsAndLists("#m", false);
smpl = new GekkoSmpl(o65.t1.Add(-2), o65.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o65, ope65));
foreach(int bankNumber in bankNumbers) {
ope65.subElements = new List<O.Prt.SubElement>();
ope65.subElements.Add(new O.Prt.SubElement());
ope65.subElements[0].tsWork = O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var);
}
o65.prtElements.Add(ope65);
}
smpl = null;

o65.counter = 25;
o65.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar173 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar173, true, EVariableType.Var)
;

p.SetText(@"¤112"); O.InitSmpl(smpl);
O.Prt o67 = new O.Prt();
o67.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope67 = new O.Prt.Element();
ope67.label = O.SubstituteScalarsAndLists("#m['a']", false);
smpl = new GekkoSmpl(o67.t1.Add(-2), o67.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o67, ope67));
foreach(int bankNumber in bankNumbers) {
ope67.subElements = new List<O.Prt.SubElement>();
ope67.subElements.Add(new O.Prt.SubElement());
ope67.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
);
}
o67.prtElements.Add(ope67);
}
smpl = null;

o67.counter = 26;
o67.Exe();

p.SetText(@"¤113"); O.InitSmpl(smpl);
O.Prt o68 = new O.Prt();
o68.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope68 = new O.Prt.Element();
ope68.label = O.SubstituteScalarsAndLists("#m['c']", false);
smpl = new GekkoSmpl(o68.t1.Add(-2), o68.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o68, ope68));
foreach(int bankNumber in bankNumbers) {
ope68.subElements = new List<O.Prt.SubElement>();
ope68.subElements.Add(new O.Prt.SubElement());
ope68.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false))
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false))
);
}
o68.prtElements.Add(ope68);
}
smpl = null;

o68.counter = 27;
o68.Exe();

p.SetText(@"¤114"); O.InitSmpl(smpl);
O.Prt o69 = new O.Prt();
o69.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope69 = new O.Prt.Element();
ope69.label = O.SubstituteScalarsAndLists("#m['a*']", false);
smpl = new GekkoSmpl(o69.t1.Add(-2), o69.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o69, ope69));
foreach(int bankNumber in bankNumbers) {
ope69.subElements = new List<O.Prt.SubElement>();
ope69.subElements.Add(new O.Prt.SubElement());
ope69.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a*", true, false))
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"a*", true, false))
);
}
o69.prtElements.Add(ope69);
}
smpl = null;

o69.counter = 28;
o69.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar174 = O.IvConvertTo(EVariableType.Var, MapDef_mapTmpvar175(smpl));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar174, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar180 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false)));
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#m", null, null, true, EVariableType.Var),  ivTmpvar180, (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))))
;

p.SetText(@"¤118"); O.InitSmpl(smpl);
O.Prt o72 = new O.Prt();
o72.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope72 = new O.Prt.Element();
ope72.label = O.SubstituteScalarsAndLists("#m.%i1", false);
smpl = new GekkoSmpl(o72.t1.Add(-2), o72.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o72, ope72));
foreach(int bankNumber in bankNumbers) {
ope72.subElements = new List<O.Prt.SubElement>();
ope72.subElements.Add(new O.Prt.SubElement());
ope72.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))));
}
o72.prtElements.Add(ope72);
}
smpl = null;

o72.counter = 29;
o72.Exe();

p.SetText(@"¤119"); O.InitSmpl(smpl);
O.Prt o73 = new O.Prt();
o73.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope73 = new O.Prt.Element();
ope73.label = O.SubstituteScalarsAndLists("#m.%i2", false);
smpl = new GekkoSmpl(o73.t1.Add(-2), o73.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o73, ope73));
foreach(int bankNumber in bankNumbers) {
ope73.subElements = new List<O.Prt.SubElement>();
ope73.subElements.Add(new O.Prt.SubElement());
ope73.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))));
}
o73.prtElements.Add(ope73);
}
smpl = null;

o73.counter = 30;
o73.Exe();

p.SetText(@"¤120"); O.InitSmpl(smpl);
O.Prt o74 = new O.Prt();
o74.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope74 = new O.Prt.Element();
ope74.label = O.SubstituteScalarsAndLists("#m.%v", false);
smpl = new GekkoSmpl(o74.t1.Add(-2), o74.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o74, ope74));
foreach(int bankNumber in bankNumbers) {
ope74.subElements = new List<O.Prt.SubElement>();
ope74.subElements.Add(new O.Prt.SubElement());
ope74.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("v", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringPercent).Add(smpl, (new ScalarString("v", true, false))));
}
o74.prtElements.Add(ope74);
}
smpl = null;

o74.counter = 31;
o74.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar181 = O.IvConvertTo(EVariableType.Var, MapDef_mapTmpvar182(smpl));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar181, true, EVariableType.Var)
;

p.SetText(@"¤123"); O.InitSmpl(smpl);
O.Prt o76 = new O.Prt();
o76.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope76 = new O.Prt.Element();
ope76.label = O.SubstituteScalarsAndLists("#m.%i1", false);
smpl = new GekkoSmpl(o76.t1.Add(-2), o76.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o76, ope76));
foreach(int bankNumber in bankNumbers) {
ope76.subElements = new List<O.Prt.SubElement>();
ope76.subElements.Add(new O.Prt.SubElement());
ope76.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))));
}
o76.prtElements.Add(ope76);
}
smpl = null;

o76.counter = 32;
o76.Exe();

p.SetText(@"¤124"); O.InitSmpl(smpl);
O.Prt o77 = new O.Prt();
o77.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope77 = new O.Prt.Element();
ope77.label = O.SubstituteScalarsAndLists("#m.#m.%i1", false);
smpl = new GekkoSmpl(o77.t1.Add(-2), o77.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o77, ope77));
foreach(int bankNumber in bankNumbers) {
ope77.subElements = new List<O.Prt.SubElement>();
ope77.subElements.Add(new O.Prt.SubElement());
ope77.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false)))), smpl, O.Indexer(O.Indexer2(smpl, (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i1", true, false))));
}
o77.prtElements.Add(ope77);
}
smpl = null;

o77.counter = 33;
o77.Exe();

p.SetText(@"¤125"); O.InitSmpl(smpl);
O.Prt o78 = new O.Prt();
o78.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope78 = new O.Prt.Element();
ope78.label = O.SubstituteScalarsAndLists("#m.#m.%i2", false);
smpl = new GekkoSmpl(o78.t1.Add(-2), o78.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o78, ope78));
foreach(int bankNumber in bankNumbers) {
ope78.subElements = new List<O.Prt.SubElement>();
ope78.subElements.Add(new O.Prt.SubElement());
ope78.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false)))), smpl, O.Indexer(O.Indexer2(smpl, (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (O.scalarStringHash).Add(smpl, (new ScalarString("m", true, false)))), (O.scalarStringPercent).Add(smpl, (new ScalarString("i2", true, false))));
}
o78.prtElements.Add(ope78);
}
smpl = null;

o78.counter = 34;
o78.Exe();

p.SetText(@"¤127"); O.InitSmpl(smpl);
O.Prt o79 = new O.Prt();
o79.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope79 = new O.Prt.Element();
ope79.label = O.SubstituteScalarsAndLists("#m['%i1']", false);
smpl = new GekkoSmpl(o79.t1.Add(-2), o79.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o79, ope79));
foreach(int bankNumber in bankNumbers) {
ope79.subElements = new List<O.Prt.SubElement>();
ope79.subElements.Add(new O.Prt.SubElement());
ope79.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"%i1", true, false))
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"%i1", true, false))
);
}
o79.prtElements.Add(ope79);
}
smpl = null;

o79.counter = 35;
o79.Exe();

p.SetText(@"¤128"); O.InitSmpl(smpl);
O.Prt o80 = new O.Prt();
o80.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope80 = new O.Prt.Element();
ope80.label = O.SubstituteScalarsAndLists("#m['#m']['%i1']", false);
smpl = new GekkoSmpl(o80.t1.Add(-2), o80.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o80, ope80));
foreach(int bankNumber in bankNumbers) {
ope80.subElements = new List<O.Prt.SubElement>();
ope80.subElements.Add(new O.Prt.SubElement());
ope80.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"%i1", true, false))
), smpl, O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"#m", true, false))
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"#m", true, false))
), new ScalarString(ScalarString.SubstituteScalarsInString(@"%i1", true, false))
);
}
o80.prtElements.Add(ope80);
}
smpl = null;

o80.counter = 36;
o80.Exe();

p.SetText(@"¤129"); O.InitSmpl(smpl);
O.Prt o81 = new O.Prt();
o81.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope81 = new O.Prt.Element();
ope81.label = O.SubstituteScalarsAndLists("#m['#m']['%i2']", false);
smpl = new GekkoSmpl(o81.t1.Add(-2), o81.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o81, ope81));
foreach(int bankNumber in bankNumbers) {
ope81.subElements = new List<O.Prt.SubElement>();
ope81.subElements.Add(new O.Prt.SubElement());
ope81.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"%i2", true, false))
), smpl, O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"#m", true, false))
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"#m", true, false))
), new ScalarString(ScalarString.SubstituteScalarsInString(@"%i2", true, false))
);
}
o81.prtElements.Add(ope81);
}
smpl = null;

o81.counter = 37;
o81.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar188 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), O.MatrixCol(O.MatrixRow(i189, i190), O.MatrixRow(i191, i192))));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar188, true, EVariableType.Var)
;

p.SetText(@"¤133"); O.InitSmpl(smpl);
O.Prt o83 = new O.Prt();
o83.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope83 = new O.Prt.Element();
ope83.label = O.SubstituteScalarsAndLists("#m[1]", false);
smpl = new GekkoSmpl(o83.t1.Add(-2), o83.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o83, ope83));
foreach(int bankNumber in bankNumbers) {
ope83.subElements = new List<O.Prt.SubElement>();
ope83.subElements.Add(new O.Prt.SubElement());
ope83.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, i193
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), i193
);
}
o83.prtElements.Add(ope83);
}
smpl = null;

o83.counter = 38;
o83.Exe();

p.SetText(@"¤134"); O.InitSmpl(smpl);
O.Prt o84 = new O.Prt();
o84.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope84 = new O.Prt.Element();
ope84.label = O.SubstituteScalarsAndLists("#m[2]", false);
smpl = new GekkoSmpl(o84.t1.Add(-2), o84.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o84, ope84));
foreach(int bankNumber in bankNumbers) {
ope84.subElements = new List<O.Prt.SubElement>();
ope84.subElements.Add(new O.Prt.SubElement());
ope84.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, i194
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), i194
);
}
o84.prtElements.Add(ope84);
}
smpl = null;

o84.counter = 39;
o84.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar195 = O.IvConvertTo(EVariableType.Var, i199);
O.IndexerSetData(smpl, O.Indexer(O.Indexer2(smpl, i196
), smpl, O.Lookup(smpl, null, null, "#m", null, null, true, EVariableType.Var), i196
),  ivTmpvar195, i197
, i198
)
;

p.SetText(@"¤136"); O.InitSmpl(smpl);
O.Prt o86 = new O.Prt();
o86.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope86 = new O.Prt.Element();
ope86.label = O.SubstituteScalarsAndLists("#m[2]", false);
smpl = new GekkoSmpl(o86.t1.Add(-2), o86.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o86, ope86));
foreach(int bankNumber in bankNumbers) {
ope86.subElements = new List<O.Prt.SubElement>();
ope86.subElements.Add(new O.Prt.SubElement());
ope86.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, i200
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), i200
);
}
o86.prtElements.Add(ope86);
}
smpl = null;

o86.counter = 40;
o86.Exe();

p.SetText(@"¤137"); O.InitSmpl(smpl);
O.Prt o87 = new O.Prt();
o87.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope87 = new O.Prt.Element();
ope87.label = O.SubstituteScalarsAndLists("#m[2][1,1]", false);
smpl = new GekkoSmpl(o87.t1.Add(-2), o87.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o87, ope87));
foreach(int bankNumber in bankNumbers) {
ope87.subElements = new List<O.Prt.SubElement>();
ope87.subElements.Add(new O.Prt.SubElement());
ope87.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, i202
, i203
), smpl, O.Indexer(O.Indexer2(smpl, i201
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), i201
), i202
, i203
);
}
o87.prtElements.Add(ope87);
}
smpl = null;

o87.counter = 41;
o87.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar204 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))), new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false))));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar204, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar205 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false)));
O.IndexerSetData(smpl, O.Indexer(O.Indexer2(smpl, i206
), smpl, O.Lookup(smpl, null, null, "#m", null, null, true, EVariableType.Var), i206
),  ivTmpvar205, i207
)
;

p.SetText(@"¤141"); O.InitSmpl(smpl);
O.Prt o90 = new O.Prt();
o90.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope90 = new O.Prt.Element();
ope90.label = O.SubstituteScalarsAndLists("#m[1][2]", false);
smpl = new GekkoSmpl(o90.t1.Add(-2), o90.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o90, ope90));
foreach(int bankNumber in bankNumbers) {
ope90.subElements = new List<O.Prt.SubElement>();
ope90.subElements.Add(new O.Prt.SubElement());
ope90.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, i209
), smpl, O.Indexer(O.Indexer2(smpl, i208
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), i208
), i209
);
}
o90.prtElements.Add(ope90);
}
smpl = null;

o90.counter = 42;
o90.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar210 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))));
O.Lookup(smpl, null, null, "#m1", null, ivTmpvar210, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar211 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))));
O.Lookup(smpl, null, null, "#m2", null, ivTmpvar211, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);

//[[splitSTOP]]
List<List<IVariable>> lists215 = new List<List<IVariable>>();
lists215.Add(O.ConvertToList(O.Lookup(smpl, null, null, "#m1", null, null, false, EVariableType.Var)));
lists215.Add(O.ConvertToList(O.Lookup(smpl, null, null, "#m2", null, null, false, EVariableType.Var)));
int max216 = O.ForListMax(lists215);
for (int i217 = 0; i217 < max216; i217 ++) {;
IVariable forloop_212 = lists215[0][i217];
IVariable forloop_213 = lists215[1][i217];
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar214 = O.IvConvertTo(EVariableType.Var, O.Add(smpl, forloop_212, forloop_213));
O.Lookup(smpl, null, null, "%s", null, ivTmpvar214, true, EVariableType.Var)
;

p.SetText(@"¤147"); O.InitSmpl(smpl);
O.Prt o95 = new O.Prt();
o95.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope95 = new O.Prt.Element();
ope95.label = O.SubstituteScalarsAndLists("%s", false);
smpl = new GekkoSmpl(o95.t1.Add(-2), o95.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o95, ope95));
foreach(int bankNumber in bankNumbers) {
ope95.subElements = new List<O.Prt.SubElement>();
ope95.subElements.Add(new O.Prt.SubElement());
ope95.subElements[0].tsWork = O.Lookup(smpl, null, null, "%s", null, null, false, EVariableType.Var);
}
o95.prtElements.Add(ope95);
}
smpl = null;

o95.counter = 43;
o95.Exe();

};

//[[splitSTART]]

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar218 = O.IvConvertTo(EVariableType.Var, i219);
O.Lookup(smpl, null, null, "xx", null, ivTmpvar218, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);

//[[splitSTOP]]
IVariable forloop_220 = null;
int counter224 = 0;
for (O.IterateStart(ref forloop_220, i221); O.IterateContinue(forloop_220, i221, d222, null, ref counter224); O.IterateStep(forloop_220, i221, null, counter224))
{;
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar223 = O.IvConvertTo(EVariableType.Var, O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), forloop_220));
O.Lookup(smpl, null, null, "xx", null, ivTmpvar223, true, EVariableType.Var)
;

};

//[[splitSTART]]

p.SetText(@"¤154"); O.InitSmpl(smpl);
O.Prt o99 = new O.Prt();
o99.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope99 = new O.Prt.Element();
ope99.label = O.SubstituteScalarsAndLists("xx", false);
smpl = new GekkoSmpl(o99.t1.Add(-2), o99.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o99, ope99));
foreach(int bankNumber in bankNumbers) {
ope99.subElements = new List<O.Prt.SubElement>();
ope99.subElements.Add(new O.Prt.SubElement());
ope99.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var);
}
o99.prtElements.Add(ope99);
}
smpl = null;

o99.counter = 44;
o99.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar225 = O.IvConvertTo(EVariableType.Var, i226);
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar225, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);

//[[splitSTOP]]
IVariable forloop_227 = null;
int counter231 = 0;
for (O.IterateStart(ref forloop_227, i228); O.IterateContinue(forloop_227, i228, d229, null, ref counter231); O.IterateStep(forloop_227, i228, null, counter231))
{;
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar230 = O.IvConvertTo(EVariableType.Var, O.Add(smpl, O.Lookup(smpl, null, null, "%sum", null, null, false, EVariableType.Var), forloop_227));
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar230, true, EVariableType.Var)
;

};

//[[splitSTART]]

p.SetText(@"¤160"); O.InitSmpl(smpl);
O.Prt o103 = new O.Prt();
o103.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope103 = new O.Prt.Element();
ope103.label = O.SubstituteScalarsAndLists("%sum", false);
smpl = new GekkoSmpl(o103.t1.Add(-2), o103.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o103, ope103));
foreach(int bankNumber in bankNumbers) {
ope103.subElements = new List<O.Prt.SubElement>();
ope103.subElements.Add(new O.Prt.SubElement());
ope103.subElements[0].tsWork = O.Lookup(smpl, null, null, "%sum", null, null, false, EVariableType.Var);
}
o103.prtElements.Add(ope103);
}
smpl = null;

o103.counter = 45;
o103.Exe();

p.SetText(@"¤162"); O.InitSmpl(smpl);
FunctionDef234();


p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar235 = O.IvConvertTo(EVariableType.Var, i236);
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar235, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);

//[[splitSTOP]]
IVariable forloop_237 = null;
int counter241 = 0;
for (O.IterateStart(ref forloop_237, i238); O.IterateContinue(forloop_237, i238, d239, null, ref counter241); O.IterateStep(forloop_237, i238, null, counter241))
{;
p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar240 = O.IvConvertTo(EVariableType.Var, O.FunctionLookup1("add1")(smpl, p, O.Lookup(smpl, null, null, "%sum", null, null, false, EVariableType.Var)));
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar240, true, EVariableType.Var)
;

};

//[[splitSTART]]

p.SetText(@"¤167"); O.InitSmpl(smpl);
O.Prt o109 = new O.Prt();
o109.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope109 = new O.Prt.Element();
ope109.label = O.SubstituteScalarsAndLists("%sum", false);
smpl = new GekkoSmpl(o109.t1.Add(-2), o109.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o109, ope109));
foreach(int bankNumber in bankNumbers) {
ope109.subElements = new List<O.Prt.SubElement>();
ope109.subElements.Add(new O.Prt.SubElement());
ope109.subElements[0].tsWork = O.Lookup(smpl, null, null, "%sum", null, null, false, EVariableType.Var);
}
o109.prtElements.Add(ope109);
}
smpl = null;

o109.counter = 46;
o109.Exe();

p.SetText(@"¤169"); O.InitSmpl(smpl);
FunctionDef244();


p.SetText(@"¤170"); O.InitSmpl(smpl);
O.Prt o112 = new O.Prt();
o112.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope112 = new O.Prt.Element();
ope112.label = O.SubstituteScalarsAndLists("add2(10, 20)[2]", false);
smpl = new GekkoSmpl(o112.t1.Add(-2), o112.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o112, ope112));
foreach(int bankNumber in bankNumbers) {
ope112.subElements = new List<O.Prt.SubElement>();
ope112.subElements.Add(new O.Prt.SubElement());
ope112.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, i247
), smpl, O.FunctionLookup2("add2")(smpl, p, i245, i246), i247
);
}
o112.prtElements.Add(ope112);
}
smpl = null;

o112.counter = 47;
o112.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar248 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false))));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar248, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);

//[[splitSTOP]]
IVariable forloop_249 = null;
int counter250 = 0;
for (O.IterateStart(ref forloop_249, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var)); O.IterateContinue(forloop_249, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), null, null, ref counter250); O.IterateStep(forloop_249, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), null, counter250))
{;
p.SetText(@"¤174"); O.InitSmpl(smpl);
O.Prt o115 = new O.Prt();
o115.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope115 = new O.Prt.Element();
ope115.label = O.SubstituteScalarsAndLists("%i", false);
smpl = new GekkoSmpl(o115.t1.Add(-2), o115.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o115, ope115));
foreach(int bankNumber in bankNumbers) {
ope115.subElements = new List<O.Prt.SubElement>();
ope115.subElements.Add(new O.Prt.SubElement());
ope115.subElements[0].tsWork = forloop_249;
}
o115.prtElements.Add(ope115);
}
smpl = null;

o115.counter = 48;
o115.Exe();

};

//[[splitSTART]]

p.SetText(@"¤178"); O.InitSmpl(smpl);
FunctionDef252();


p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar253 = O.IvConvertTo(EVariableType.Var, i254);
O.Lookup(smpl, null, null, "%v1", null, ivTmpvar253, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar255 = O.IvConvertTo(EVariableType.Var, O.FunctionLookup1("f")(smpl, p, O.FunctionLookup1("f")(smpl, p, O.Lookup(smpl, null, null, "%v1", null, null, false, EVariableType.Var))));
O.Lookup(smpl, null, null, "%v2", null, ivTmpvar255, true, EVariableType.Var)
;

p.SetText(@"¤184"); O.InitSmpl(smpl);
O.Prt o120 = new O.Prt();
o120.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope120 = new O.Prt.Element();
ope120.label = O.SubstituteScalarsAndLists("%v2", false);
smpl = new GekkoSmpl(o120.t1.Add(-2), o120.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o120, ope120));
foreach(int bankNumber in bankNumbers) {
ope120.subElements = new List<O.Prt.SubElement>();
ope120.subElements.Add(new O.Prt.SubElement());
ope120.subElements[0].tsWork = O.Lookup(smpl, null, null, "%v2", null, null, false, EVariableType.Var);
}
o120.prtElements.Add(ope120);
}
smpl = null;

o120.counter = 49;
o120.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar256 = O.IvConvertTo(EVariableType.Var, Functions.series(smpl, i257));
O.Lookup(smpl, null, null, "xx", null, ivTmpvar256, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar258 = O.IvConvertTo(EVariableType.Var, i259);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar258, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false))
)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar260 = O.IvConvertTo(EVariableType.Var, i261);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar260, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false))
)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar262 = O.IvConvertTo(EVariableType.Var, i263);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar262, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar264 = O.IvConvertTo(EVariableType.Var, i265);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar264, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar266 = O.IvConvertTo(EVariableType.Var, i268);
O.IndexerSetData(smpl, O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
), smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
),  ivTmpvar266, i267
)
;

p.SetText(@"¤193"); O.InitSmpl(smpl);
O.Prt o127 = new O.Prt();
o127.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope127 = new O.Prt.Element();
ope127.label = O.SubstituteScalarsAndLists("xx['b', 'y']", false);
smpl = new GekkoSmpl(o127.t1.Add(-2), o127.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o127, ope127));
foreach(int bankNumber in bankNumbers) {
ope127.subElements = new List<O.Prt.SubElement>();
ope127.subElements.Add(new O.Prt.SubElement());
ope127.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
), smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
);
}
o127.prtElements.Add(ope127);
}
smpl = null;

o127.counter = 50;
o127.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar269 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))));
O.Lookup(smpl, null, null, "#m1", null, ivTmpvar269, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar270 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))));
O.Lookup(smpl, null, null, "#m2", null, ivTmpvar270, true, EVariableType.Var)
;

p.SetText(@"¤196"); O.InitSmpl(smpl);
O.Prt o130 = new O.Prt();
o130.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope130 = new O.Prt.Element();
ope130.label = O.SubstituteScalarsAndLists("sum((#m1, #m2), xx[#m1, #m2])", false);
smpl = new GekkoSmpl(o130.t1.Add(-2), o130.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o130, ope130));
foreach(int bankNumber in bankNumbers) {
ope130.subElements = new List<O.Prt.SubElement>();
ope130.subElements.Add(new O.Prt.SubElement());
ope130.subElements[0].tsWork = temp273(smpl);
}
o130.prtElements.Add(ope130);
}
smpl = null;

o130.counter = 51;
o130.Exe();

p.SetText(@"¤197"); O.InitSmpl(smpl);
O.Prt o131 = new O.Prt();
o131.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope131 = new O.Prt.Element();
ope131.label = O.SubstituteScalarsAndLists("unfold((#m1, #m2), xx[#m1, #m2] + 0)", false);
smpl = new GekkoSmpl(o131.t1.Add(-2), o131.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o131, ope131));
foreach(int bankNumber in bankNumbers) {
ope131.subElements = new List<O.Prt.SubElement>();
ope131.subElements.Add(new O.Prt.SubElement());
ope131.subElements[0].tsWork = temp277(smpl);
}
o131.prtElements.Add(ope131);
}
smpl = null;

o131.counter = 52;
o131.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar278 = O.IvConvertTo(EVariableType.Var, MapDef_mapTmpvar279(smpl));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar278, true, EVariableType.Var)
;

p.SetText(@"¤199"); O.InitSmpl(smpl);
O.Prt o133 = new O.Prt();
o133.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope133 = new O.Prt.Element();
ope133.label = O.SubstituteScalarsAndLists("#m.ts['b', 'y']", false);
smpl = new GekkoSmpl(o133.t1.Add(-2), o133.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o133, ope133));
foreach(int bankNumber in bankNumbers) {
ope133.subElements = new List<O.Prt.SubElement>();
ope133.subElements.Add(new O.Prt.SubElement());
ope133.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
), smpl, O.Indexer(O.Indexer2(smpl, (new ScalarString("ts", true, false))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (new ScalarString("ts", true, false))), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
);
}
o133.prtElements.Add(ope133);
}
smpl = null;

o133.counter = 53;
o133.Exe();

p.SetText(@"¤200"); O.InitSmpl(smpl);
O.Prt o134 = new O.Prt();
o134.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope134 = new O.Prt.Element();
ope134.label = O.SubstituteScalarsAndLists("#m['ts']['b', 'y']", false);
smpl = new GekkoSmpl(o134.t1.Add(-2), o134.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o134, ope134));
foreach(int bankNumber in bankNumbers) {
ope134.subElements = new List<O.Prt.SubElement>();
ope134.subElements.Add(new O.Prt.SubElement());
ope134.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
), smpl, O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"ts", true, false))
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"ts", true, false))
), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
);
}
o134.prtElements.Add(ope134);
}
smpl = null;

o134.counter = 54;
o134.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar281 = O.IvConvertTo(EVariableType.Var, MapDef_mapTmpvar282(smpl));
O.Lookup(smpl, null, null, "#m3", null, ivTmpvar281, true, EVariableType.Var)
;

p.SetText(@"¤202"); O.InitSmpl(smpl);
O.Prt o136 = new O.Prt();
o136.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope136 = new O.Prt.Element();
ope136.label = O.SubstituteScalarsAndLists("#m3.%s", false);
smpl = new GekkoSmpl(o136.t1.Add(-2), o136.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o136, ope136));
foreach(int bankNumber in bankNumbers) {
ope136.subElements = new List<O.Prt.SubElement>();
ope136.subElements.Add(new O.Prt.SubElement());
ope136.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("s", true, false)))), smpl, O.Lookup(smpl, null, null, "#m3", null, null, false, EVariableType.Var), (O.scalarStringPercent).Add(smpl, (new ScalarString("s", true, false))));
}
o136.prtElements.Add(ope136);
}
smpl = null;

o136.counter = 55;
o136.Exe();

p.SetText(@"¤203"); O.InitSmpl(smpl);
O.Prt o137 = new O.Prt();
o137.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope137 = new O.Prt.Element();
ope137.label = O.SubstituteScalarsAndLists("#m3.#m5.ts['b', 'y']", false);
smpl = new GekkoSmpl(o137.t1.Add(-2), o137.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o137, ope137));
foreach(int bankNumber in bankNumbers) {
ope137.subElements = new List<O.Prt.SubElement>();
ope137.subElements.Add(new O.Prt.SubElement());
ope137.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
), smpl, O.Indexer(O.Indexer2(smpl, (new ScalarString("ts", true, false))), smpl, O.Indexer(O.Indexer2(smpl, (O.scalarStringHash).Add(smpl, (new ScalarString("m5", true, false)))), smpl, O.Lookup(smpl, null, null, "#m3", null, null, false, EVariableType.Var), (O.scalarStringHash).Add(smpl, (new ScalarString("m5", true, false)))), (new ScalarString("ts", true, false))), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
);
}
o137.prtElements.Add(ope137);
}
smpl = null;

o137.counter = 56;
o137.Exe();

p.SetText(@"¤204"); O.InitSmpl(smpl);
O.Write o138 = new O.Write();

o138.fileName = O.ConvertToString((new ScalarString("slet", true, false)));

o138.type = @"write";o138.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
O.Reset o139 = new O.Reset();
o139.p = p;o139.Exe(smpl);

p.SetText(@"¤206"); O.InitSmpl(smpl);
ClearTS(p);
O.Read o140 = new O.Read();
o140.p = p;
o140.type = @"read";
o140.fileName = O.ConvertToString((new ScalarString("slet", true, false)));


o140.Exe();

p.SetText(@"¤207"); O.InitSmpl(smpl);
O.Prt o141 = new O.Prt();
o141.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope141 = new O.Prt.Element();
ope141.label = O.SubstituteScalarsAndLists("xx['b', 'y']", false);
smpl = new GekkoSmpl(o141.t1.Add(-2), o141.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o141, ope141));
foreach(int bankNumber in bankNumbers) {
ope141.subElements = new List<O.Prt.SubElement>();
ope141.subElements.Add(new O.Prt.SubElement());
ope141.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
), smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
);
}
o141.prtElements.Add(ope141);
}
smpl = null;

o141.counter = 57;
o141.Exe();

p.SetText(@"¤208"); O.InitSmpl(smpl);
O.Prt o142 = new O.Prt();
o142.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope142 = new O.Prt.Element();
ope142.label = O.SubstituteScalarsAndLists("sum((#m1, #m2), xx[#m1, #m2])", false);
smpl = new GekkoSmpl(o142.t1.Add(-2), o142.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o142, ope142));
foreach(int bankNumber in bankNumbers) {
ope142.subElements = new List<O.Prt.SubElement>();
ope142.subElements.Add(new O.Prt.SubElement());
ope142.subElements[0].tsWork = temp287(smpl);
}
o142.prtElements.Add(ope142);
}
smpl = null;

o142.counter = 58;
o142.Exe();

p.SetText(@"¤209"); O.InitSmpl(smpl);
O.Prt o143 = new O.Prt();
o143.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope143 = new O.Prt.Element();
ope143.label = O.SubstituteScalarsAndLists("unfold((#m1, #m2), xx[#m1, #m2] + 0)", false);
smpl = new GekkoSmpl(o143.t1.Add(-2), o143.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o143, ope143));
foreach(int bankNumber in bankNumbers) {
ope143.subElements = new List<O.Prt.SubElement>();
ope143.subElements.Add(new O.Prt.SubElement());
ope143.subElements[0].tsWork = temp291(smpl);
}
o143.prtElements.Add(ope143);
}
smpl = null;

o143.counter = 59;
o143.Exe();

p.SetText(@"¤210"); O.InitSmpl(smpl);
O.Prt o144 = new O.Prt();
o144.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope144 = new O.Prt.Element();
ope144.label = O.SubstituteScalarsAndLists("#m.ts['b', 'y']", false);
smpl = new GekkoSmpl(o144.t1.Add(-2), o144.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o144, ope144));
foreach(int bankNumber in bankNumbers) {
ope144.subElements = new List<O.Prt.SubElement>();
ope144.subElements.Add(new O.Prt.SubElement());
ope144.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
), smpl, O.Indexer(O.Indexer2(smpl, (new ScalarString("ts", true, false))), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), (new ScalarString("ts", true, false))), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
);
}
o144.prtElements.Add(ope144);
}
smpl = null;

o144.counter = 60;
o144.Exe();

p.SetText(@"¤211"); O.InitSmpl(smpl);
O.Prt o145 = new O.Prt();
o145.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope145 = new O.Prt.Element();
ope145.label = O.SubstituteScalarsAndLists("#m['ts']['b', 'y']", false);
smpl = new GekkoSmpl(o145.t1.Add(-2), o145.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o145, ope145));
foreach(int bankNumber in bankNumbers) {
ope145.subElements = new List<O.Prt.SubElement>();
ope145.subElements.Add(new O.Prt.SubElement());
ope145.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
), smpl, O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"ts", true, false))
), smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"ts", true, false))
), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
);
}
o145.prtElements.Add(ope145);
}
smpl = null;

o145.counter = 61;
o145.Exe();

p.SetText(@"¤212"); O.InitSmpl(smpl);
O.Prt o146 = new O.Prt();
o146.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope146 = new O.Prt.Element();
ope146.label = O.SubstituteScalarsAndLists("#m3.%s", false);
smpl = new GekkoSmpl(o146.t1.Add(-2), o146.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o146, ope146));
foreach(int bankNumber in bankNumbers) {
ope146.subElements = new List<O.Prt.SubElement>();
ope146.subElements.Add(new O.Prt.SubElement());
ope146.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, (O.scalarStringPercent).Add(smpl, (new ScalarString("s", true, false)))), smpl, O.Lookup(smpl, null, null, "#m3", null, null, false, EVariableType.Var), (O.scalarStringPercent).Add(smpl, (new ScalarString("s", true, false))));
}
o146.prtElements.Add(ope146);
}
smpl = null;

o146.counter = 62;
o146.Exe();

p.SetText(@"¤213"); O.InitSmpl(smpl);
O.Prt o147 = new O.Prt();
o147.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope147 = new O.Prt.Element();
ope147.label = O.SubstituteScalarsAndLists("#m3.#m5.ts['b', 'y']", false);
smpl = new GekkoSmpl(o147.t1.Add(-2), o147.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o147, ope147));
foreach(int bankNumber in bankNumbers) {
ope147.subElements = new List<O.Prt.SubElement>();
ope147.subElements.Add(new O.Prt.SubElement());
ope147.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
), smpl, O.Indexer(O.Indexer2(smpl, (new ScalarString("ts", true, false))), smpl, O.Indexer(O.Indexer2(smpl, (O.scalarStringHash).Add(smpl, (new ScalarString("m5", true, false)))), smpl, O.Lookup(smpl, null, null, "#m3", null, null, false, EVariableType.Var), (O.scalarStringHash).Add(smpl, (new ScalarString("m5", true, false)))), (new ScalarString("ts", true, false))), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
);
}
o147.prtElements.Add(ope147);
}
smpl = null;

o147.counter = 63;
o147.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar292 = O.IvConvertTo(EVariableType.Var, i293);
O.Lookup(smpl, null, null, "xxax", null, ivTmpvar292, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar294 = O.IvConvertTo(EVariableType.Var, i295);
O.Lookup(smpl, null, null, "xxbx", null, ivTmpvar294, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar296 = O.IvConvertTo(EVariableType.Var, i297);
O.Lookup(smpl, null, null, "xxay", null, ivTmpvar296, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar298 = O.IvConvertTo(EVariableType.Var, i299);
O.Lookup(smpl, null, null, "xxby", null, ivTmpvar298, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar300 = O.IvConvertTo(EVariableType.Var, i302);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xxby", null, null, true, EVariableType.Var),  ivTmpvar300, i301
)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar303 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))));
O.Lookup(smpl, null, null, "#m1", null, ivTmpvar303, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar304 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))));
O.Lookup(smpl, null, null, "#m2", null, ivTmpvar304, true, EVariableType.Var)
;

p.SetText(@"¤224"); O.InitSmpl(smpl);
O.Prt o155 = new O.Prt();
o155.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope155 = new O.Prt.Element();
ope155.label = O.SubstituteScalarsAndLists("sum((#m1, #m2), xx{#m1}{#m2})", false);
smpl = new GekkoSmpl(o155.t1.Add(-2), o155.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o155, ope155));
foreach(int bankNumber in bankNumbers) {
ope155.subElements = new List<O.Prt.SubElement>();
ope155.subElements.Add(new O.Prt.SubElement());
ope155.subElements[0].tsWork = temp307(smpl);
}
o155.prtElements.Add(ope155);
}
smpl = null;

o155.counter = 64;
o155.Exe();

p.SetText(@"¤225"); O.InitSmpl(smpl);
O.Prt o156 = new O.Prt();
o156.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope156 = new O.Prt.Element();
ope156.label = O.SubstituteScalarsAndLists("unfold((#m1, #m2), xx{#m1}{#m2} + 0)", false);
smpl = new GekkoSmpl(o156.t1.Add(-2), o156.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o156, ope156));
foreach(int bankNumber in bankNumbers) {
ope156.subElements = new List<O.Prt.SubElement>();
ope156.subElements.Add(new O.Prt.SubElement());
ope156.subElements[0].tsWork = temp311(smpl);
}
o156.prtElements.Add(ope156);
}
smpl = null;

o156.counter = 65;
o156.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar312 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"xxax", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"xxbx", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"xxay", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"xxby", true, false))));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar312, true, EVariableType.Var)
;

p.SetText(@"¤227"); O.InitSmpl(smpl);
O.Prt o158 = new O.Prt();
o158.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope158 = new O.Prt.Element();
ope158.label = O.SubstituteScalarsAndLists("unfold(#m, {#m} + 0)", false);
smpl = new GekkoSmpl(o158.t1.Add(-2), o158.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o158, ope158));
foreach(int bankNumber in bankNumbers) {
ope158.subElements = new List<O.Prt.SubElement>();
ope158.subElements.Add(new O.Prt.SubElement());
ope158.subElements[0].tsWork = temp315(smpl);
}
o158.prtElements.Add(ope158);
}
smpl = null;

o158.counter = 66;
o158.Exe();

p.SetText(@"¤228"); O.InitSmpl(smpl);
O.Write o159 = new O.Write();

o159.fileName = O.ConvertToString((new ScalarString("slet", true, false)));

o159.type = @"write";o159.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
O.Reset o160 = new O.Reset();
o160.p = p;o160.Exe(smpl);

p.SetText(@"¤230"); O.InitSmpl(smpl);
ClearTS(p);
O.Read o161 = new O.Read();
o161.p = p;
o161.type = @"read";
o161.fileName = O.ConvertToString((new ScalarString("slet", true, false)));


o161.Exe();

p.SetText(@"¤231"); O.InitSmpl(smpl);
O.Prt o162 = new O.Prt();
o162.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope162 = new O.Prt.Element();
ope162.label = O.SubstituteScalarsAndLists("sum((#m1, #m2), xx{#m1}{#m2})", false);
smpl = new GekkoSmpl(o162.t1.Add(-2), o162.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o162, ope162));
foreach(int bankNumber in bankNumbers) {
ope162.subElements = new List<O.Prt.SubElement>();
ope162.subElements.Add(new O.Prt.SubElement());
ope162.subElements[0].tsWork = temp318(smpl);
}
o162.prtElements.Add(ope162);
}
smpl = null;

o162.counter = 67;
o162.Exe();

p.SetText(@"¤232"); O.InitSmpl(smpl);
O.Prt o163 = new O.Prt();
o163.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope163 = new O.Prt.Element();
ope163.label = O.SubstituteScalarsAndLists("unfold((#m1, #m2), xx{#m1}{#m2} + 0)", false);
smpl = new GekkoSmpl(o163.t1.Add(-2), o163.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o163, ope163));
foreach(int bankNumber in bankNumbers) {
ope163.subElements = new List<O.Prt.SubElement>();
ope163.subElements.Add(new O.Prt.SubElement());
ope163.subElements[0].tsWork = temp322(smpl);
}
o163.prtElements.Add(ope163);
}
smpl = null;

o163.counter = 68;
o163.Exe();

p.SetText(@"¤233"); O.InitSmpl(smpl);
O.Prt o164 = new O.Prt();
o164.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope164 = new O.Prt.Element();
ope164.label = O.SubstituteScalarsAndLists("unfold(#m, {#m} + 0)", false);
smpl = new GekkoSmpl(o164.t1.Add(-2), o164.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o164, ope164));
foreach(int bankNumber in bankNumbers) {
ope164.subElements = new List<O.Prt.SubElement>();
ope164.subElements.Add(new O.Prt.SubElement());
ope164.subElements[0].tsWork = temp325(smpl);
}
o164.prtElements.Add(ope164);
}
smpl = null;

o164.counter = 69;
o164.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
O.Reset o165 = new O.Reset();
o165.p = p;o165.Exe(smpl);

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar326 = O.IvConvertTo(EVariableType.Var, Functions.series(smpl, i327));
O.Lookup(smpl, null, null, "xx", null, ivTmpvar326, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar328 = O.IvConvertTo(EVariableType.Var, i329);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar328, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar330 = O.IvConvertTo(EVariableType.Var, i332);
O.IndexerSetData(smpl, O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
), smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
),  ivTmpvar330, i331
)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar333 = O.IvConvertTo(EVariableType.Var, i334);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var),  ivTmpvar333, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false))
)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar335 = O.IvConvertTo(EVariableType.Var, i336);
O.Lookup(smpl, null, null, "xx2", null, ivTmpvar335, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar337 = O.IvConvertTo(EVariableType.Var, Functions.timeless(smpl));
O.Lookup(smpl, null, null, "xx3", null, ivTmpvar337, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar338 = O.IvConvertTo(EVariableType.Var, i339);
O.Lookup(smpl, null, null, "xx3", null, ivTmpvar338, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar340 = O.IvConvertTo(EVariableType.Var, Functions.series(smpl, i341));
O.Lookup(smpl, null, null, "xx4", null, ivTmpvar340, true, EVariableType.Var)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar342 = O.IvConvertTo(EVariableType.Var, i343);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx4", null, null, true, EVariableType.Var),  ivTmpvar342, new ScalarString(ScalarString.SubstituteScalarsInString(@"q", true, false))
)
;

p.SetText(@"¤245"); O.InitSmpl(smpl);
O.Write o175 = new O.Write();

o175.opt_gdx = "yes";

o175.fileName = O.ConvertToString((new ScalarString("slet", true, false)));

o175.type = @"write";o175.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
O.Reset o176 = new O.Reset();
o176.p = p;o176.Exe(smpl);

p.SetText(@"¤247"); O.InitSmpl(smpl);
ClearTS(p);
O.Read o177 = new O.Read();
o177.p = p;
o177.type = @"read";
o177.opt_gdx = "yes";

o177.fileName = O.ConvertToString((new ScalarString("slet", true, false)));


o177.Exe();

p.SetText(@"¤248"); O.InitSmpl(smpl);
O.Prt o178 = new O.Prt();
o178.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope178 = new O.Prt.Element();
ope178.label = O.SubstituteScalarsAndLists("xx['a', 'b']", false);
smpl = new GekkoSmpl(o178.t1.Add(-2), o178.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o178, ope178));
foreach(int bankNumber in bankNumbers) {
ope178.subElements = new List<O.Prt.SubElement>();
ope178.subElements.Add(new O.Prt.SubElement());
ope178.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
), smpl, O.Lookup(smpl, null, null, "xx", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
);
}
o178.prtElements.Add(ope178);
}
smpl = null;

o178.counter = 70;
o178.Exe();

p.SetText(@"¤249"); O.InitSmpl(smpl);
O.Prt o179 = new O.Prt();
o179.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope179 = new O.Prt.Element();
ope179.label = O.SubstituteScalarsAndLists("xx2", false);
smpl = new GekkoSmpl(o179.t1.Add(-2), o179.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o179, ope179));
foreach(int bankNumber in bankNumbers) {
ope179.subElements = new List<O.Prt.SubElement>();
ope179.subElements.Add(new O.Prt.SubElement());
ope179.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx2", null, null, false, EVariableType.Var);
}
o179.prtElements.Add(ope179);
}
smpl = null;

o179.counter = 71;
o179.Exe();

p.SetText(@"¤250"); O.InitSmpl(smpl);
O.Prt o180 = new O.Prt();
o180.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope180 = new O.Prt.Element();
ope180.label = O.SubstituteScalarsAndLists("xx3", false);
smpl = new GekkoSmpl(o180.t1.Add(-2), o180.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o180, ope180));
foreach(int bankNumber in bankNumbers) {
ope180.subElements = new List<O.Prt.SubElement>();
ope180.subElements.Add(new O.Prt.SubElement());
ope180.subElements[0].tsWork = O.Lookup(smpl, null, null, "xx3", null, null, false, EVariableType.Var);
}
o180.prtElements.Add(ope180);
}
smpl = null;

o180.counter = 72;
o180.Exe();

p.SetText(@"¤251"); O.InitSmpl(smpl);
O.Prt o181 = new O.Prt();
o181.prtType = "p";

{
List<int> bankNumbers = null;
O.Prt.Element ope181 = new O.Prt.Element();
ope181.label = O.SubstituteScalarsAndLists("xx4['q']", false);
smpl = new GekkoSmpl(o181.t1.Add(-2), o181.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o181, ope181));
foreach(int bankNumber in bankNumbers) {
ope181.subElements = new List<O.Prt.SubElement>();
ope181.subElements.Add(new O.Prt.SubElement());
ope181.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"q", true, false))
), smpl, O.Lookup(smpl, null, null, "xx4", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"q", true, false))
);
}
o181.prtElements.Add(ope181);
}
smpl = null;

o181.counter = 73;
o181.Exe();


//[[splitSTOP]]


}
}
}
