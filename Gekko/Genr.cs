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
public static readonly ScalarVal i3 = new ScalarVal(1d);
public static readonly ScalarVal i4 = new ScalarVal(2d);
public static readonly ScalarVal i5 = new ScalarVal(1d);
public static readonly ScalarVal i6 = new ScalarVal(2d);
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
IVariable ivTmpvar1 = O.ListDef(O.ListDef(new ScalarString(@"a"), new ScalarString(@"b")), new ScalarString(@"c"));
O.Lookup(smpl, null, "#m", null, true, ivTmpvar1)
;




p.SetText(@"¤0");
IVariable ivTmpvar2 = new ScalarString(@"x");
O.Indexer(smpl, O.Indexer(smpl, O.Lookup(smpl, null, "#m", null, true, null), false, null, i3
), true, ivTmpvar2, i4
)
;







}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);



}
}
}
