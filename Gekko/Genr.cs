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
public static readonly ScalarVal i7 = new ScalarVal(5d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = null;


p.SetText(@"¤1");
smpl = null;O.Series o0 = new O.Series();
o0.t1 = Globals.globalPeriodStart;o0.t2 = Globals.globalPeriodEnd;
smpl = new GekkoSmpl(o0.t1, o0.t2);
o0.p = p;
//o0.lhs = O.Indexer(smpl, O.FindTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("xx"))), 1, O.ECreatePossibilities.Can), true, new ScalarString(@"a"))
;
o0.rhs = O.ConvertToTimeSeriesLight(smpl, GekkoExpression1(smpl, 1, p));
o0.Exe();




}


public static IVariable GekkoExpression1(GekkoSmpl smpl, int bankNumber, P p) {

return i7;
}
public static void CodeLines(P p)
{
GekkoSmpl smpl = null;

C0(p);



}
}
}
