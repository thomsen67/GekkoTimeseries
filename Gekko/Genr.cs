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
public static List PrintHelper_112(GekkoSmpl smpl) {List m113 = new List(); for (int iBankNumber = 0; iBankNumber < 2; iBankNumber++){
m113.Add(O.Lookup(smpl, null, null, "#xx", null, null, false, iBankNumber));
}
return m113;
}
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = O.Smpl();


p.SetText(@"¤0");
for (int iSmpl114 = 0; iSmpl114 < int.MaxValue; iSmpl114++) {
O.Print(smpl, (O.Lookup(smpl, null, (PrintHelper_112(smpl)), null, false, iBankNumber)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl114); else break;
}



}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);



}
}
}
