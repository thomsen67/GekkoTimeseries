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
Func<IVariable> func56 = () => {
var smplCommandRemember57 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
Series temp55 = new Series(ESeriesType.Normal, Program.options.freq, null); temp55.SetZero(smpl);

foreach (IVariable listloop_i53 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("i")))), null, new  LookupSettings(), EVariableType.Var, null))) {
temp55.InjectAdd(smpl, temp55, O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None,listloop_i53), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new  LookupSettings(), EVariableType.Var, null), listloop_i53), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag,O.Negate(smpl, i54)
), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None,listloop_i53), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new  LookupSettings(), EVariableType.Var, null), listloop_i53), O.Negate(smpl, i54)
)));

labelCounter++;
}
labelCounter = 0;
smpl.command = smplCommandRemember57;
return temp55;

};


O.Decomp o0 = new O.Decomp();
o0.t1 = Globals.globalPeriodStart;
o0.t2 = Globals.globalPeriodEnd;

o0.opt_prtcode = O.ConvertToString((new ScalarString("n")));



o0.smplForFunc = smpl;
o0.expression = () => func56();

o0.Exe();

//[[commandEnd]]0
}


public static readonly ScalarVal i54 = new ScalarVal(1d);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
