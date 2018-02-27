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
public static readonly ScalarVal i7 = new ScalarVal(2012d);
public static readonly ScalarVal i8 = new ScalarVal(2012d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl, p);

IVariable ivTmpvar6 = O.IvConvertTo(EVariableType.Var, O.CreateListFromStrings(new string[] {"a", "b", "c"}));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar6, true, EVariableType.Var)
;

p.SetText(@"¤4"); O.InitSmpl(smpl, p);

GekkoTimes gt = null; 
gt = null; 
List<O.HandleEndoHelper> l0 = new List<O.HandleEndoHelper>(); 
O.HandleEndoHelper helper1 = new O.HandleEndoHelper();
List<IVariable> l1 = new List<IVariable>();
l1.Add(O.Lookup(smpl, null, null, "#m", null, null, false, EVariableType.Var)
);
l1.Add(new ScalarString(ScalarString.SubstituteScalarsInString(@"k1", true, false))
);
helper1.local = null;
helper1.varname = O.NameLookup(smpl, null, (new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false))), null, false, EVariableType.Var);
helper1.indices = l1;
l0.Add(helper1);
O.HandleEndoHelper helper2 = new O.HandleEndoHelper();
List<IVariable> l2 = new List<IVariable>();
l2.Add(new ScalarString(ScalarString.SubstituteScalarsInString(@"e", true, false))
);
l2.Add(new ScalarString(ScalarString.SubstituteScalarsInString(@"k2", true, false))
);
helper2.local = O.HandleDates(O.ConvertToDate(i7, O.GetDateChoices.FlexibleStart)
,O.ConvertToDate(i8, O.GetDateChoices.FlexibleEnd)
 );;
helper2.varname = O.NameLookup(smpl, null, null, "y", null, null, false, EVariableType.Var);
helper2.indices = l2;
l0.Add(helper2);
O.HandleEndo(gt, l0);


//[[splitSTOP]]


}
}
}
