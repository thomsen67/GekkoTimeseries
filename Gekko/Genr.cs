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
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = O.Smpl();


p.SetText(@"¤0");
IVariable ivTmpvar69 = O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false)));
for (int iSmpl70 = 0; iSmpl70 < int.MaxValue; iSmpl70++) {
O.Lookup(smpl, null, null, "#m", null, ivTmpvar69, true)
;

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl70); else break;
};




p.SetText(@"¤0");

}

public static void C1(P p) {

GekkoSmpl smpl = O.Smpl();





}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);

IVariable forloop_71 = null;
int counter74 = 0;
for (O.IterateStart(ref forloop_71, O.Lookup(smpl, null, null, "#m", null, null, false)); O.IterateContinue(forloop_71, O.Lookup(smpl, null, null, "#m", null, null, false), null, null, ref counter74); O.IterateStep(forloop_71, O.Lookup(smpl, null, null, "#m", null, null, false), null, counter74))
{;
List m72 = null; try { m72 = new List();
for (smpl.bankNumber = 0; smpl.bankNumber < 1; smpl.bankNumber++) {
//forloop_71;
m72.Add(forloop_71);
}
}
finally
{
smpl.bankNumber = 0;
}
for (int iSmpl73 = 0; iSmpl73 < int.MaxValue; iSmpl73++) {
O.Print(smpl, m72);

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl73); else break;
}
};

C1(p);



}
}
}
