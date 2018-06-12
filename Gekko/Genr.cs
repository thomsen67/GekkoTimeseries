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
public static readonly ScalarVal i23 = new ScalarVal(1d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar22 = O.TypeCheck_var(O.Add(smpl, i23, O.FunctionLookup0("f")(smpl, p)), -1);
O.Lookup(smpl, null, null, "%y", null, ivTmpvar22, true, EVariableType.Var)
;


//[[splitSTOP]]


}
}
}
