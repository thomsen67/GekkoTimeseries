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
public static void C0(GekkoSmpl smpl, P p) {
//[[commandStart]]0
p.SetText(@"¤1"); O.InitSmpl(smpl, p);


O.Assignment o0 = new O.Assignment();
O.AdjustT0(smpl, -1);
IVariable ivTmpvar108 = O.Add(smpl, O.Lookup(smpl, null, null, "y", null, null, new  LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "y", null, null, new  LookupSettings(), EVariableType.Var, null));
O.AdjustT0(smpl, 1);
O.Lookup(smpl, null, null, "y", null, ivTmpvar108, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
;

//[[commandEnd]]0
}



public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
