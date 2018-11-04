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
public static readonly ScalarVal i97 = new ScalarVal(2010d);
public static readonly ScalarVal i98 = new ScalarVal(2015d);
public static readonly ScalarVal i100 = new ScalarVal(2020d);
public static readonly ScalarVal i101 = new ScalarVal(2030d);
public static readonly ScalarVal i102 = new ScalarVal(5d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤1"); O.InitSmpl(smpl, p);

O.TimeFilter o0 = new O.TimeFilter();
O.TimeFilterHelper temp99 = new O.TimeFilterHelper();
temp99.from = O.ConvertToDate(i97, O.GetDateChoices.Strict);
temp99.to = O.ConvertToDate(i98, O.GetDateChoices.Strict);
o0.timeFilterPeriods.Add(temp99);

O.TimeFilterHelper temp103 = new O.TimeFilterHelper();
temp103.from = O.ConvertToDate(i100, O.GetDateChoices.Strict);
temp103.to = O.ConvertToDate(i101, O.GetDateChoices.Strict);
temp103.step = O.ConvertToInt(i102);
o0.timeFilterPeriods.Add(temp103);


o0.Exe();


//[[splitSTOP]]


}
}
}
