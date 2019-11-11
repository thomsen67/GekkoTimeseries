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
o0.opt_source = @"<[code]>%v = toExcelDate(1, 2, 3)";


Action assign_29 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar25 = Functions.toexceldate(smpl, null, null, i26, i27, i28);
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "%v", null, ivTmpvar25, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
;
};
Func<bool> check_29 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar25 = Functions.toexceldate(smpl, null, null, i26, i27, i28);
O.AdjustT0(smpl, 2);
if (ivTmpvar25.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.Lookup(smpl, null, null, "%v", null, ivTmpvar25, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_29, check_29, o0);

//[[commandEnd]]0
}


public static readonly ScalarVal i26 = new ScalarVal(1d, 0);
public static readonly ScalarVal i27 = new ScalarVal(2d, 0);
public static readonly ScalarVal i28 = new ScalarVal(3d, 0);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
