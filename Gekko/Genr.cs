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
public static IVariable MapDef_mapTmpvar221(GekkoSmpl smpl) {
Map mapTmpvar221 = new Map();
IVariable ivTmpvar222 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false)));
O.Lookup(smpl, mapTmpvar221, null, "%i1", null, ivTmpvar222, true, EVariableType.Var)
;

IVariable ivTmpvar223 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"c", true, false)));
O.Lookup(smpl, mapTmpvar221, null, "%i2", null, ivTmpvar223, true, EVariableType.Var)
;

IVariable ivTmpvar224 = O.IvConvertTo(EVariableType.Var, O.CreateListFromStrings(new string[] {"xx"}));
O.Lookup(smpl, mapTmpvar221, null, "ts", null, ivTmpvar224, true, EVariableType.Var)
;


return mapTmpvar221;
}public static IVariable MapDef_mapTmpvar218(GekkoSmpl smpl) {
Map mapTmpvar218 = new Map();
IVariable ivTmpvar219 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)));
O.Lookup(smpl, mapTmpvar218, null, "%i1", null, ivTmpvar219, true, EVariableType.Var)
;

IVariable ivTmpvar220 = O.IvConvertTo(EVariableType.Var, MapDef_mapTmpvar221(smpl));
O.Lookup(smpl, mapTmpvar218, null, "#mm", null, ivTmpvar220, true, EVariableType.Var)
;


return mapTmpvar218;
}public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl);

IVariable ivTmpvar217 = O.IvConvertTo(EVariableType.Var, MapDef_mapTmpvar218(smpl));
O.Lookup(smpl, null, null, "#m", null, ivTmpvar217, true, EVariableType.Var)
;


//[[splitSTOP]]


}
}
}
