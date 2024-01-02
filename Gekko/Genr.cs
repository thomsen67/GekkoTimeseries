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
o0.opt_trace = @"x<2015 2024 dyn>=x.1+1";
smpl.t0 = O.ConvertToDate(i7, O.GetDateChoices.FlexibleStart);
;
smpl.t1 = O.ConvertToDate(i7, O.GetDateChoices.FlexibleStart);
;
smpl.t2 = O.ConvertToDate(i8, O.GetDateChoices.FlexibleEnd);
;
smpl.t3 = O.ConvertToDate(i8, O.GetDateChoices.FlexibleEnd);
;

o0.opt_dyn = "yes";




Globals.precedentsSeries = null;
Action assign_9 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar4 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.Dot,i5), smpl, O.EIndexerType.Dot, O.Lookup(smpl, null, null, "x", null, null, new  LookupSettings(), EVariableType.Var, null), i5), i6);
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "x", null, ivTmpvar4, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
;
};
Func<bool> check_9 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar4 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.Dot,i5), smpl, O.EIndexerType.Dot, O.Lookup(smpl, null, null, "x", null, null, new  LookupSettings(), EVariableType.Var, null), i5), i6);
O.AdjustT0(smpl, 2);
if (ivTmpvar4.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.Lookup(smpl, null, null, "x", null, ivTmpvar4, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_9, check_9, o0, p);

//[[commandEnd]]0
}


public static readonly ScalarVal i5 = new ScalarVal(1d, 0);
public static readonly ScalarVal i6 = new ScalarVal(1d, 0);
public static readonly ScalarVal i7 = new ScalarVal(2015d, 0);
public static readonly ScalarVal i8 = new ScalarVal(2024d, 0);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
