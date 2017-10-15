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
public static readonly ScalarVal i630 = new ScalarVal(1d);
public static void FunctionDef631() {


//[[splitSTOP]]

Globals.ufunctions1.Add("add1", (GekkoSmpl smpl, IVariable functionarg_629) => { 
//[[splitSTOP]]
return O.Add(smpl, functionarg_629, i630);

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i634 = new ScalarVal(0d);
public static readonly ScalarVal i636 = new ScalarVal(1d);
public static readonly ScalarVal d637 = new ScalarVal(1e3d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = O.Smpl();


p.SetText(@"¤1");
FunctionDef631();





p.SetText(@"¤0");
IVariable ivTmpvar632 = i634;
for (int iSmpl633 = 0; iSmpl633 < int.MaxValue; iSmpl633++) {
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar632, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl633); else break;
};




p.SetText(@"¤0");

}

public static void C1(P p) {

GekkoSmpl smpl = O.Smpl();





p.SetText(@"¤0");
for (int iSmpl641 = 0; iSmpl641 < int.MaxValue; iSmpl641++) {
O.Print(smpl, (O.Lookup(smpl, null, null, "%sum", null, null, false)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl641); else break;
}



}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);

IVariable forloop_635 = null;
int counter640 = 0;
for (O.IterateStart(ref forloop_635, i636); O.IterateContinue(forloop_635, i636, d637, null, ref counter640); O.IterateStep(forloop_635, i636, null, counter640))
{;
IVariable ivTmpvar638 = O.FunctionLookup1("add1")(smpl, O.Lookup(smpl, null, null, "%sum", null, null, false));
for (int iSmpl639 = 0; iSmpl639 < int.MaxValue; iSmpl639++) {
O.Lookup(smpl, null, null, "%sum", null, ivTmpvar638, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl639); else break;
};

};

C1(p);



}
}
}
