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
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoTime t = Globals.tNull;


p.SetText(@"¤1");
O.Prt o0 = new O.Prt();
o0.prtType = "prt";

{
List<int> bankNumbers = null;
O.Prt.Element ope0 = new O.Prt.Element();
ope0.label = O.SubstituteScalarsAndLists("fe", false);
GekkoSmpl smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o0, ope0));
foreach(int bankNumber in bankNumbers) {
IVariable ts10 = O.GetTimeSeries(smpl, O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("fe"))), bankNumber);
ope0.subElements.Add(new O.Prt.SubElement());
ope0.subElements[0].tsWork = (TimeSeriesLight)(ts10);
}
o0.prtElements.Add(ope0);
}


o0.counter = 10;
o0.Exe();




}


public static void CodeLines(P p)
{
GekkoTime t = Globals.tNull;

C0(p);



}
}
}
