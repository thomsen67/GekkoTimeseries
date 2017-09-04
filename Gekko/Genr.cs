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
public static void FunctionDef11() {


//[[splitSTOP]]

Globals.ufunctions1.Add("f", (GekkoSmpl smpl, IVariable functionarg_10) => { 
//[[splitSTOP]]
return null;

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i12 = new ScalarVal(100d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = O.Smpl();


p.SetText(@"¤0");
O.Reset o0 = new O.Reset();
o0.p = p;o0.Exe();




p.SetText(@"¤2");
FunctionDef11();





p.SetText(@"¤0");
O.Assignment(smpl, ((O.scalarStringPercent).Add(smpl, (new ScalarString("v", true, false))))
, i12);




p.SetText(@"¤0");
O.Assignment(smpl, ((O.scalarStringPercent).Add(smpl, (new ScalarString("v", true, false))))
, Globals.ufunctions1["f"](smpl, O.Lookup(smpl, ((O.scalarStringPercent).Add(smpl, (new ScalarString("v", true, false)))))));




}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);



}
}
}
