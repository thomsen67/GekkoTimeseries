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
p.SetText(@"¤1"); O.InitSmpl(smpl);
O.Disp o0 = new O.Disp();
o0.list = O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"xx", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"yy", true, false)));
o0.Exe();


//[[splitSTOP]]


}
}
}
