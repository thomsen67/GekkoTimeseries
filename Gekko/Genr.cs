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

O.Collapse o0 = new O.Collapse();
o0.lhs = new List(O.ExplodeIvariables(new List(new List<IVariable> {new ScalarString("x")})));
o0.rhs = new List(O.ExplodeIvariables(new List(new List<IVariable> {(new ScalarString("x")).Add(smpl, new ScalarString("[")).Add(smpl, (new ScalarString("q"))).Add(smpl, new ScalarString("]"))})));
o0.type = null;
o0.Exe();


//[[splitSTOP]]


}
}
}
