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
public static IVariable temp17(GekkoSmpl smpl, IVariable listloop_j15) {
TimeSeries temp17 = new TimeSeries(Program.options.freq, null); temp17.SetZero(smpl);

foreach (IVariable listloop_i16 in new O.GekkoListIterator(O.Lookup(smpl, ((O.scalarStringHash).Add(smpl, (new ScalarString("i", true, false))))))) {
temp17.InjectAdd(smpl, temp17, O.Indexer(smpl, O.Lookup(smpl, ((new ScalarString("x", true, false)))), false, listloop_i16, new ScalarString(@"m")
, listloop_j15));

}
return temp17;

}
public static IVariable temp18(GekkoSmpl smpl) {
TimeSeries temp18 = new TimeSeries(Program.options.freq, null); temp18.SetZero(smpl);

foreach (IVariable listloop_j15 in new O.GekkoListIterator(O.Lookup(smpl, ((O.scalarStringHash).Add(smpl, (new ScalarString("j", true, false))))))) {
temp18.InjectAdd(smpl, temp18, temp17(smpl, null));

}
return temp18;

}
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = O.Smpl();


p.SetText(@"¤1");
O.Print(smpl, (temp18(smpl)));




}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);



}
}
}
