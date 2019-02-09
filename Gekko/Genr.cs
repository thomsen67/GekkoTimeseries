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


O.Table.SetValues o0 = new O.Table.SetValues();
o0.name = O.ConvertToString((new ScalarString("xx")));
o0.col = O.ConvertToInt(i32);
o0.t1 = O.ConvertToDate(i33, O.GetDateChoices.Strict);
o0.t2 = O.ConvertToDate(i34, O.GetDateChoices.Strict);
o0.operator2 = O.ConvertToString(O.HandleString(new ScalarString(@"n")));
o0.scale = O.ConvertToVal(d36);
o0.format = O.ConvertToString(O.HandleString(new ScalarString(@"f10.3")));
try {
O.isTableCall = true;
{
List<int> bankNumbers = O.Prt.CreateBankHelper(1);
O.Prt.Element ope0 = new O.Prt.Element();
ope0.labelGiven = new List<string>() {""};
smpl = new GekkoSmpl(o0.t1, o0.t2); smpl.t0 = smpl.t0.Add(-2);
bankNumbers = O.Prt.GetBankNumbers(Globals.tableOption, new List<string>(){o0.operator2});

for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope0.variable[bankNumber] = i35;
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
}
smpl.bankNumber = 0;
o0.prtElements.Add(ope0);
}
}
finally {
  O.isTableCall = false;
}
o0.Exe();

//[[commandEnd]]0
}


public static readonly ScalarVal i32 = new ScalarVal(1d);
public static readonly ScalarVal i33 = new ScalarVal(2000d);
public static readonly ScalarVal i34 = new ScalarVal(2010d);
public static readonly ScalarVal i35 = new ScalarVal(1d);
public static readonly ScalarVal d36 = new ScalarVal(0.001d);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
