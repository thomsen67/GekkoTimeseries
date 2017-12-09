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
public static readonly ScalarVal i202 = new ScalarVal(1d);
public static readonly ScalarVal i203 = new ScalarVal(2000d);
public static readonly ScalarVal i204 = new ScalarVal(2000d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            smpl.t1 = O.ConvertToDate(i203, O.GetDateChoices.FlexibleStart);
;
smpl.t2 = O.ConvertToDate(i204, O.GetDateChoices.FlexibleEnd);
;


IVariable ivTmpvar201 = O.IvConvertTo(EVariableType.Var, i202);
O.Lookup(smpl, null, null, "yy", null, ivTmpvar201, true, EVariableType.Var)
;


//[[splitSTOP]]


}
}
}
