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
public static readonly ScalarVal i16 = new ScalarVal(2010d);
public static readonly ScalarVal i17 = new ScalarVal(2010d);
public static readonly ScalarVal i18 = new ScalarVal(2011d);
public static readonly ScalarVal i19 = new ScalarVal(2011d);
public static readonly ScalarVal i20 = new ScalarVal(2012d);
public static readonly ScalarVal i21 = new ScalarVal(2012d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar15 = O.IvConvertTo(EVariableType.Var, O.CreateListFromStrings(new string[] {"a", "b", "c"}));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar15, true, EVariableType.Var)
;

p.SetText(@"¤4"); O.InitSmpl(smpl, p);

GekkoTimes gt = O.HandleDates(O.ConvertToDate(i16, O.GetDateChoices.FlexibleStart)
,O.ConvertToDate(i17, O.GetDateChoices.FlexibleEnd)
 );; 
O.HandleEndoHelper helper = new O.HandleEndoHelper();
List<IVariable> l1 = new List<IVariable>();
l1.Add(O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var)
);
l1.Add(new ScalarString(ScalarString.SubstituteScalarsInString(@"k1", true, false))
);
helper.local = O.HandleDates(O.ConvertToDate(i18, O.GetDateChoices.FlexibleStart)
,O.ConvertToDate(i19, O.GetDateChoices.FlexibleEnd)
 );;
helper.varname = O.NameLookup(smpl, null, (new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false))), null, false, EVariableType.Var);
helper.indices = l1;
O.HandleEndo(gt, helper);


//[[splitSTOP]]


}
}
}
