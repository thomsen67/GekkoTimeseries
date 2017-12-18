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
public static readonly ScalarVal i212 = new ScalarVal(1d);
public static readonly ScalarVal i213 = new ScalarVal(2d);
public static readonly ScalarVal i214 = new ScalarVal(3d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar211 = O.IvConvertTo(EVariableType.Series, O.ListDefHelper(i212, i213, i214));
O.Lookup(smpl, null, null, "xx", null, ivTmpvar211, true, EVariableType.Series)
;


//[[splitSTOP]]


}
}
}
