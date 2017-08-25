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
public static readonly ScalarVal i1 = new ScalarVal(5d);
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
            IVariable xxx = new ScalarString(@"a");
o0.p = p;
            IVariable xx = O.GetTimeSeries(smpl, O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("xx"))),1, O.ECreatePossibilities.Can);
            //o0.lhs = O.Indexer(smpl, xx, true, xxx);
o0.rhs = O.ConvertToTimeSeries(smpl, GekkoExpression1(smpl, 1, p));
o0.Exe();




}


public static IVariable GekkoExpression1(GekkoSmpl smpl, int bankNumber, P p) {

return i1;
}
public static void CodeLines(P p)
{
GekkoSmpl smpl = null;

C0(p);



}
}
}
