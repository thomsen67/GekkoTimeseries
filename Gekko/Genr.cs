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
p.SetText(@"¤0"); O.InitSmpl(smpl, p);

O.Assignment o0 = new O.Assignment();
O.AdjustT0(smpl, -1);
IVariable ivTmpvar3 = O.HandleString(new ScalarString(@"x"));
O.AdjustT0(smpl, 1);
O.Lookup(smpl, null, null, "%s1", null, ivTmpvar3, true, EVariableType.Var, o0)
;

p.SetText(@"¤0"); O.InitSmpl(smpl, p);

O.Assignment o1 = new O.Assignment();
O.AdjustT0(smpl, -1);
IVariable ivTmpvar4 = O.HandleString(new ScalarString(@"y"));
O.AdjustT0(smpl, 1);
O.Lookup(smpl, null, null, "%s2", null, ivTmpvar4, true, EVariableType.Var, o1)
;

p.SetText(@"¤4"); O.InitSmpl(smpl, p);

//Program.Tell(O.ConvertToString(O.HandleString(new ScalarString(@"a")).Add(O.Lookup(smpl, null, null, "%s1", null, null, false, EVariableType.Var, null)).Add(O.HandleString(new ScalarString(@"b"))).Add(O.Lookup(smpl, null, null, "%s2", null, null, false, EVariableType.Var, null)).Add(O.HandleString(new ScalarString(@"c")))), false);

//[[splitSTOP]]


}
}
}
