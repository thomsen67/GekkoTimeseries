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
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤1"); O.InitSmpl(smpl, p);

Func<GraphHelper, string> print0 = (gh) =>
{
O.Prt o0 = new O.Prt();
labelCounter = 0;o0.guiGraphIsRefreshing = gh.isRefreshing;
o0.guiGraphPrintCode = gh.printCode;
o0.guiGraphIsLogTransform = gh.isLogTransform;
o0.prtType = "p";




    IVariable ivTmpvar83 = O.TypeCheck_var(O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var).append(smpl, O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var)), -1);
    O.Lookup(smpl, null, null, "#n", null, ivTmpvar83, true, EVariableType.Var)
    ;



    o0.counter = 11;
o0.printCsCounter = Globals.printCs.Count - 1;
o0.labelHelper2 = O.AddLabelHelper2(smpl);
o0.Exe();
return o0.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print0); 
print0(new GraphHelper());


//[[splitSTOP]]


}
}
}
