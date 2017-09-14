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
public static readonly ScalarVal i2 = new ScalarVal(1d);
public static readonly ScalarVal i3 = new ScalarVal(1d);
public static readonly ScalarVal i4 = new ScalarVal(1d);
public static readonly ScalarVal i5 = new ScalarVal(0d);
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
IVariable ivTmpvar1 = O.Add(smpl, O.Dollar(smpl, i2, O.Equals(smpl, i3,i4)
), i5);
O.Lookup(smpl, null, null, "%v", null, ivTmpvar1)
;




p.SetText(@"¤0");
O.Print(smpl, (O.Lookup(smpl, null, null, "%v", null, null)));




}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);



}
}
}
