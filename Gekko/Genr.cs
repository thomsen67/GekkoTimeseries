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
public static readonly ScalarVal i33 = new ScalarVal(1d);
public static readonly ScalarVal i34 = new ScalarVal(2d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤1"); O.InitSmpl(smpl, p);

O.Assignment o0 = new O.Assignment();
O.AdjustT0(smpl, -1);
IVariable ivTmpvar32 = i34;
O.AdjustT0(smpl, 1);
            IVariable temp = O.Lookup(smpl, null, null, "d1i", null, null, new LookupSettings(), EVariableType.Var, null);
            //O.DollarLookup(O.Equals(smpl, O.Indexer(smpl, temp,  ivTmpvar32, o0, O.HandleString(new ScalarString(@"a")) ),i33) , smpl, null, null, "pI", null, ivTmpvar32, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null);


//[[splitSTOP]]


}
}
}
