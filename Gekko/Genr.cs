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
public static readonly ScalarVal i81 = new ScalarVal(1d);
public static readonly ScalarVal i82 = new ScalarVal(2d);
public static readonly ScalarVal i83 = new ScalarVal(1d);
public static readonly ScalarVal i84 = new ScalarVal(2d);
public static readonly ScalarVal i94 = new ScalarVal(0d);
public static readonly ScalarVal i96 = new ScalarVal(1d);
public static readonly ScalarVal d97 = new ScalarVal(1e3d);
public static readonly ScalarVal i101 = new ScalarVal(0d);
public static readonly ScalarVal i103 = new ScalarVal(1d);
public static readonly ScalarVal d104 = new ScalarVal(1e3d);
public static readonly ScalarVal i108 = new ScalarVal(1d);
public static void FunctionDef109() {


//[[splitSTOP]]

Globals.ufunctions1.Add("add1", (GekkoSmpl smpl, IVariable functionarg_107) => { 
//[[splitSTOP]]
return O.Add(smpl, functionarg_107, i108);

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i111 = new ScalarVal(0d);
public static readonly ScalarVal i113 = new ScalarVal(1d);
public static readonly ScalarVal d114 = new ScalarVal(1e3d);
public static void FunctionDef119() {


//[[splitSTOP]]

Globals.ufunctions2.Add("add2", (GekkoSmpl smpl, IVariable functionarg_117, IVariable functionarg_118) => { 
//[[splitSTOP]]
return O.ListDef(functionarg_117, O.Add(smpl, functionarg_117, functionarg_118));

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i120 = new ScalarVal(10d);
public static readonly ScalarVal i121 = new ScalarVal(20d);
public static readonly ScalarVal i122 = new ScalarVal(2d);
public static void FunctionDef127() {


//[[splitSTOP]]

Globals.ufunctions1.Add("f", (GekkoSmpl smpl, IVariable functionarg_126) => { 
//[[splitSTOP]]
return O.Add(smpl, functionarg_126, functionarg_126);

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i129 = new ScalarVal(100d);
public static readonly ScalarVal i132 = new ScalarVal(12345d);
public static readonly ScalarVal i134 = new ScalarVal(1d);
public static readonly ScalarVal i136 = new ScalarVal(2d);
public static readonly ScalarVal i138 = new ScalarVal(3d);
public static readonly ScalarVal i140 = new ScalarVal(4d);
public static IVariable temp145(GekkoSmpl smpl) {
TimeSeries temp145 = new TimeSeries(Program.options.freq, null); temp145.SetZero(smpl);

foreach (IVariable listloop_m1143 in new O.GekkoListIterator(O.Lookup(smpl, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null))) {
foreach (IVariable listloop_m2144 in new O.GekkoListIterator(O.Lookup(smpl, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null))) {
temp145.InjectAdd(smpl, temp145, O.Indexer(smpl, O.Lookup(smpl, null, "xx", null, false, null), false, null, listloop_m1143, listloop_m2144));

}
}
return temp145;

}
public static IVariable temp148(GekkoSmpl smpl) {
MetaList temp148 = new MetaList();

foreach (IVariable listloop_m1146 in new O.GekkoListIterator(O.Lookup(smpl, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1", true, false)))), null))) {
foreach (IVariable listloop_m2147 in new O.GekkoListIterator(O.Lookup(smpl, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2", true, false)))), null))) {
temp148.Add(O.Indexer(smpl, O.Lookup(smpl, null, "xx", null, false, null), false, null, listloop_m1146, listloop_m2147));

}
}
return temp148;

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
IVariable ivTmpvar79 = O.ListDef(O.ListDef(new ScalarString(@"a"), new ScalarString(@"b")), new ScalarString(@"c"));
O.Lookup(smpl, null, "#m", null, true, ivTmpvar79)
;




p.SetText(@"¤0");
IVariable ivTmpvar80 = new ScalarString(@"x");
O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, "#m", null, true, null), false, null, i81
), true, ivTmpvar80, i82
)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, "#m", null, true, null), false, null, i83
), false, null, i84
)));




p.SetText(@"¤0");
IVariable ivTmpvar85 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"));
O.Lookup(smpl, null, "#m1", null, true, ivTmpvar85)
;




p.SetText(@"¤0");
IVariable ivTmpvar86 = O.ListDef(new ScalarString(@"x"), new ScalarString(@"y"));
O.Lookup(smpl, null, "#m2", null, true, ivTmpvar86)
;




p.SetText(@"¤0");

}

public static void C1(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤0");
IVariable ivTmpvar93 = i94;
O.Lookup(smpl, null, "xx", null, false, ivTmpvar93)
;




p.SetText(@"¤0");

}

public static void C2(P p) {

GekkoSmpl smpl = O.Smpl();





}

public static void C3(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, "xx", null, false, null)));




p.SetText(@"¤0");
IVariable ivTmpvar100 = i101;
O.Lookup(smpl, null, "%sum", null, true, ivTmpvar100)
;




p.SetText(@"¤0");

}

public static void C4(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, "%sum", null, true, null)));




p.SetText(@"¤62");
FunctionDef109();





p.SetText(@"¤0");
IVariable ivTmpvar110 = i111;
O.Lookup(smpl, null, "%sum", null, true, ivTmpvar110)
;




p.SetText(@"¤0");

}

public static void C5(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, "%sum", null, true, null)));




p.SetText(@"¤69");
FunctionDef119();





p.SetText(@"¤70");
O.Print(smpl, (O.Indexer(smpl, Globals.ufunctions2["add2"](smpl, i120, i121), false, null, i122
)));




}

public static void C6(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar123 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"), new ScalarString(@"c"));
O.Lookup(smpl, null, "#m", null, true, ivTmpvar123)
;




p.SetText(@"¤0");

}

public static void C7(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤78");
FunctionDef127();





p.SetText(@"¤0");
IVariable ivTmpvar128 = i129;
O.Lookup(smpl, null, "%v1", null, true, ivTmpvar128)
;




p.SetText(@"¤0");
IVariable ivTmpvar130 = Globals.ufunctions1["f"](smpl, Globals.ufunctions1["f"](smpl, O.Lookup(smpl, null, "%v1", null, true, null)));
O.Lookup(smpl, null, "%v2", null, true, ivTmpvar130)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, "%v2", null, true, null)));




p.SetText(@"¤0");
IVariable ivTmpvar131 = O.Negate(smpl, i132);
O.Lookup(smpl, null, "xx", null, false, ivTmpvar131)
;




p.SetText(@"¤0");
IVariable ivTmpvar133 = i134;
O.Lookup(smpl, null, "xx___a___x", null, false, ivTmpvar133)
;




p.SetText(@"¤0");
IVariable ivTmpvar135 = i136;
O.Lookup(smpl, null, "xx___b___x", null, false, ivTmpvar135)
;




p.SetText(@"¤0");
IVariable ivTmpvar137 = i138;
O.Lookup(smpl, null, "xx___a___y", null, false, ivTmpvar137)
;




}

public static void C8(P p) {

GekkoSmpl smpl = O.Smpl();

p.SetText(@"¤0");
IVariable ivTmpvar139 = i140;
O.Lookup(smpl, null, "xx___b___y", null, false, ivTmpvar139)
;




p.SetText(@"¤0");
IVariable ivTmpvar141 = O.ListDef(new ScalarString(@"a"), new ScalarString(@"b"));
O.Lookup(smpl, null, "#m1", null, true, ivTmpvar141)
;




p.SetText(@"¤0");
IVariable ivTmpvar142 = O.ListDef(new ScalarString(@"x"), new ScalarString(@"y"));
O.Lookup(smpl, null, "#m2", null, true, ivTmpvar142)
;




p.SetText(@"¤94");
O.Print(smpl, (temp145(smpl)));




p.SetText(@"¤96");
O.Print(smpl, (temp148(smpl)));




}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);

List<List<IVariable>> lists90 = new List<List<IVariable>>();
lists90.Add((O.Lookup(smpl, null, "#m1", null, true, null)).GetList());
lists90.Add((O.Lookup(smpl, null, "#m2", null, true, null)).GetList());
int max91 = O.ForListMax(lists90);
for (int i92 = 0; i92 < max91; i92 ++) {;
IVariable forloop_87 = lists90[0][i92];
IVariable forloop_88 = lists90[1][i92];
IVariable ivTmpvar89 = O.Add(smpl, forloop_87, forloop_88);
O.Lookup(smpl, null, "%s", null, true, ivTmpvar89)
;

O.Print(smpl, (O.Lookup(smpl, null, "%s", null, true, null)));

};

C1(p);

IVariable forloop_95 = null;
int counter99 = 0;
for (O.IterateStart(ref forloop_95, i96); O.IterateContinue(forloop_95, i96, d97, null, ref counter99); O.IterateStep(forloop_95, i96, null, counter99))
{;
IVariable ivTmpvar98 = O.Add(smpl, O.Lookup(smpl, null, "xx", null, false, null), forloop_95);
O.Lookup(smpl, null, "xx", null, false, ivTmpvar98)
;

};

C2(p);

C3(p);

IVariable forloop_102 = null;
int counter106 = 0;
for (O.IterateStart(ref forloop_102, i103); O.IterateContinue(forloop_102, i103, d104, null, ref counter106); O.IterateStep(forloop_102, i103, null, counter106))
{;
IVariable ivTmpvar105 = O.Add(smpl, O.Lookup(smpl, null, "%sum", null, true, null), forloop_102);
O.Lookup(smpl, null, "%sum", null, true, ivTmpvar105)
;

};

C4(p);

IVariable forloop_112 = null;
int counter116 = 0;
for (O.IterateStart(ref forloop_112, i113); O.IterateContinue(forloop_112, i113, d114, null, ref counter116); O.IterateStep(forloop_112, i113, null, counter116))
{;
IVariable ivTmpvar115 = Globals.ufunctions1["add1"](smpl, O.Lookup(smpl, null, "%sum", null, true, null));
O.Lookup(smpl, null, "%sum", null, true, ivTmpvar115)
;

};

C5(p);

C6(p);

IVariable forloop_124 = null;
int counter125 = 0;
for (O.IterateStart(ref forloop_124, O.Lookup(smpl, null, "#m", null, true, null)); O.IterateContinue(forloop_124, O.Lookup(smpl, null, "#m", null, true, null), null, null, ref counter125); O.IterateStep(forloop_124, O.Lookup(smpl, null, "#m", null, true, null), null, counter125))
{;
O.Print(smpl, (forloop_124));

};

C7(p);

C8(p);



}
}
}
