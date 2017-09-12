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
public static readonly ScalarVal i48 = new ScalarVal(2d);
public static readonly ScalarVal i49 = new ScalarVal(1d);
public static readonly ScalarVal i50 = new ScalarVal(2d);
public static readonly ScalarVal i51 = new ScalarVal(77d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = O.Smpl();


p.SetText(@"¤0");
IVariable ivTmpvar47 = i51;
(O.Indexer(smpl, O.Lookup(smpl, null, "#m", null, true, null), null, i48
)).SetData(ivTmpvar47, (new Range(i49, i50)))
;




}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);



}
}
}
