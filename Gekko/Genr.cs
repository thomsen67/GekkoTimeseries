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
public static GekkoTime globalGekkoTimeIterator = Globals.tNull;
public static IVariable scalar3 = null;
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoTime t = Globals.tNull;


p.SetText(@"¤1");
Program.options.freq = G.GetFreq(O.GetString((O.GetScalarFromCache(ref scalar3, "s", false, false))));
G.Writeln();
//G.Writeln("option freq = " + G.GetFreq(O.GetString((O.GetScalarFromCache(ref scalar3, "s", "no", "no")))) + "");
ClearTS(p);
Program.AdjustFreq();



}


public static void CodeLines(P p)
{
GekkoTime t = Globals.tNull;

C0(p);



}
}
}
