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
p.SetStack(@"¤1"); O.InitSmpl(smpl, p);
O.Assignment o0 = new O.Assignment();
o0.opt_trace = @"x<2003 2001;option databank trace = no>=2";
smpl.t0 = O.ConvertToDate(i13, O.GetDateChoices.FlexibleStart);
;
smpl.t1 = O.ConvertToDate(i13, O.GetDateChoices.FlexibleStart);
;
smpl.t2 = O.ConvertToDate(i14, O.GetDateChoices.FlexibleEnd);
;
smpl.t3 = O.ConvertToDate(i14, O.GetDateChoices.FlexibleEnd);
;







var record15 = Program.options.databank_trace;

try {
Program.options.databank_trace = O.XBool("databank trace", (new ScalarString("no")));
O.HandleOptions("Program.options.databank_trace", 1, p);

Globals.precedentsSeries = null;
Action assign_16 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar11 = i12;
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "x", null, ivTmpvar11, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
;
};
Func<bool> check_16 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar11 = i12;
O.AdjustT0(smpl, 2);
if (ivTmpvar11.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.Lookup(smpl, null, null, "x", null, ivTmpvar11, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_16, check_16, o0, p);

}
finally {
Program.options.databank_trace = record15;
O.HandleOptions("Program.options.databank_trace", 2, p);

}

//[[commandEnd]]0
}


public static readonly ScalarVal i12 = new ScalarVal(2d, 0);
public static readonly ScalarVal i13 = new ScalarVal(2003d, 0);
public static readonly ScalarVal i14 = new ScalarVal(2001d, 0);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
