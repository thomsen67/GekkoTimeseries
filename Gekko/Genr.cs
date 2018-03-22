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
public static readonly ScalarVal i8 = new ScalarVal(5d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl, p);

foreach (IVariable listloop_i5 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, false, EVariableType.Var))) {
foreach (IVariable listloop_j6 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("j")))), null, false, EVariableType.Var))) {
IVariable ivTmpvar7 = O.IvConvertTo(EVariableType.Var, i8);
O.DollarIndexerSetData(O.ListContains(O.Lookup(smpl, null, null, "#i", null, ivTmpvar7, false, EVariableType.Var),new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)))
, smpl, O.Lookup(smpl, null, null, "xxx", null, null, true, EVariableType.Var),  ivTmpvar7, listloop_i5, listloop_j6)
;
}
}


//[[splitSTOP]]


}
}
}
