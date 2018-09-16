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
public static readonly ScalarVal i10 = new ScalarVal(100d);
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
IVariable ivTmpvar9 = i10;
O.Lookup(smpl, null, null, "xx", null, ivTmpvar9, true, EVariableType.Var, null)
;


//[[splitSTOP]]


}
}
}
