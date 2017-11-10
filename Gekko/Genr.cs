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
public static readonly ScalarVal i83 = new ScalarVal(100d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl);
            IVariable ivTmpvar82 = null;
O.Lookup(smpl, null, null, "%v", null, ivTmpvar82, true, EVariableType.Var)
;


//[[splitSTOP]]


}
}
}
