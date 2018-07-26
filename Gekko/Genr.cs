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

O.Delete o0 = new O.Delete();
o0.names = new List(O.ExplodeIvariables(new List(new List<IVariable> {new ScalarString("a")})));
o0.p = p;
o0.Exe();


//[[splitSTOP]]


}
}
}
