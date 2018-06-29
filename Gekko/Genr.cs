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

O.Compare o0 = new O.Compare();
//o0.listItems = new List(O.ExplodeIvariables(new List(new List<IVariable> {(new ScalarString("xx")).Add(smpl, new ScalarString("[")).Add(smpl, new ScalarString("a")).Add(smpl, ", ").Add(smpl, new ScalarString("x")).Add(smpl, new ScalarString("]"))})));
o0.Exe();


//[[splitSTOP]]


}
}
}
