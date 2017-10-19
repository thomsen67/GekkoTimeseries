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
public static GekkoTime globalGekkoTimeIterator = Globals.tNull;
public static List PrintHelper_493(GekkoSmpl smpl) {List m494 = new List(); for (int iBankNumber = 0; iBankNumber < 2; iBankNumber++){
m494.Add(O.ListDefHelper(O.Lookup(smpl, null, null, "xx1", null, null, false, iBankNumber), O.Lookup(smpl, null, null, "xx2", null, null, false, iBankNumber), O.Lookup(smpl, null, (O.Lookup(smpl, null, null, "#m", null, null, false, iBankNumber)), null, false, iBankNumber)));
}
return m494;
}
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = O.Smpl();


p.SetText(@"¤0");
for (int iSmpl495 = 0; iSmpl495 < int.MaxValue; iSmpl495++) {
O.Print(smpl, (PrintHelper_493(smpl)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl495); else break;
}



}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);



}
}
}
