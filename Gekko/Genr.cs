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
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl);
List m80 = null; try { m80 = new List();
for (smpl.bankNumber = 0; smpl.bankNumber < 1; smpl.bankNumber++) {
m80.Add(O.Indexer(O.Indexer2(smpl, new ScalarDate(G.FromStringToDate("1900a1"))
), smpl, O.Add(smpl, O.Lookup(smpl, null, null, "xx", null, null, false), O.Lookup(smpl, null, null, "xx", null, null, false)), new ScalarDate(G.FromStringToDate("1900a1"))
));
}
}
finally
{
smpl.bankNumber = 0;
}
O.Print(smpl, m80);

//[[splitSTOP]]


}
}
}
