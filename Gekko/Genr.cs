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


Program.options.freq = G.GetFreq(O.ConvertToString((new ScalarString("d"))));
G.Writeln();
G.Writeln("option freq = " + (G.GetFreq(O.ConvertToString((new ScalarString("d"))))).ToString().ToLower() + "");
Program.AdjustFreq();
//[[commandEnd]]0


//[[commandStart]]1
p.SetText(@"¤2"); O.InitSmpl(smpl, p);


O.Time o1 = new O.Time();
o1.t1 = O.ConvertToDate(i21, O.GetDateChoices.FlexibleStart);
;
o1.t2 = O.ConvertToDate(i22, O.GetDateChoices.FlexibleEnd);
;

o1.Exe();

//[[commandEnd]]1
}


public static readonly ScalarVal i21 = new ScalarVal(2000d, 0);
public static readonly ScalarVal i22 = new ScalarVal(2012d, 0);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
