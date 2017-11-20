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
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl);
            Func<IVariable> temp33 = () =>
            {
                List temp3 = new List();

                foreach (IVariable listloop_i2 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, false, EVariableType.Var)))
                {
                    temp3.Add(O.Indexer(O.Indexer2(smpl, listloop_i2), smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var), listloop_i2));

                }
                return temp3;

            };


IVariable ivTmpvar1 = O.IvConvertTo(EVariableType.Var, temp33());
O.Lookup(smpl, null, null, "xx", null, ivTmpvar1, true, EVariableType.Var)
;


//[[splitSTOP]]


}
}
}
