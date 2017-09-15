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
public static readonly ScalarVal i15 = new ScalarVal(2d);
public static readonly ScalarVal i16 = new ScalarVal(2d);
public static readonly ScalarVal i17 = new ScalarVal(100d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = O.Smpl();


p.SetText(@"¤0");
IVariable ivTmpvar14 = i17;
O.Dollar(smpl, O.Lookup(smpl, null, null, "xx", null, ivTmpvar14), O.Equals(smpl, i15,i16)
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
