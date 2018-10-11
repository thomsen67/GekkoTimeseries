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
public static int labelCounter;
public static readonly ScalarVal i18 = new ScalarVal(1d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl, p);

O.Assignment o0 = new O.Assignment();
smpl.t0 = Globals.globalPeriodStart;
smpl.t1 = Globals.globalPeriodStart;
smpl.t2 = Globals.globalPeriodEnd;
smpl.t3 = Globals.globalPeriodEnd;

o0.opt_rownames = O.GetList(O.Lookup(smpl, null, null, "#a", null, null, O.ELookupType.RightHandSide, EVariableType.Matrix, null));


O.AdjustT0(smpl, -1);
IVariable ivTmpvar17 = O.MatrixCol(O.MatrixRow(i18));
O.AdjustT0(smpl, 1);
O.Lookup(smpl, null, null, "#x", null, ivTmpvar17, O.ELookupType.LeftHandSide, EVariableType.Matrix, o0)
;


//[[splitSTOP]]


}
}
}
