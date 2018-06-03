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
public static readonly ScalarVal i3 = new ScalarVal(1d);
public static void FunctionDef4() {


//[[splitSTOP]]

O.PrepareUfunction(1, "f");

Globals.ufunctions1.Add("f", (GekkoSmpl smpl, P p, IVariable functionarg_1) => { functionarg_1 = O.TypeCheck_val(functionarg_1, 1);

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar2 = O.TypeCheck_var(O.Add(smpl, functionarg_1, i3), -1);
//functionarg_1
;

p.SetText(@"¤1"); O.InitSmpl(smpl, p);


//[[splitSTOP]]
return O.TypeCheck_val(functionarg_1, 0);

//[[splitSTART]]


return null; 
});


//[[splitSTART]]

}

public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤1"); O.InitSmpl(smpl, p);

FunctionDef4();



//[[splitSTOP]]


}
}
}
