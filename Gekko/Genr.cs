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

O.Decomp o0 = new O.Decomp();
o0.t1 = Globals.globalPeriodStart;
o0.t2 = Globals.globalPeriodEnd;

smpl = new GekkoSmpl(o0.t1.Add(-O.MaxLag()), o0.t2.Add(O.MaxLead()));
o0.expression = () => O.Add(smpl, O.Add(smpl, O.Multiply(smpl, i94, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None,new ScalarString("a")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new  LookupSettings(), EVariableType.Var, null), new ScalarString("a"))), O.Multiply(smpl, i95, O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag,O.Negate(smpl, i96)
), smpl, O.EIndexerType.IndexerLag, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None,new ScalarString("a")), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "x", null, null, new  LookupSettings(), EVariableType.Var, null), new ScalarString("a")), O.Negate(smpl, i96)
))), O.Lookup(smpl, null, null, "%v", null, null, new  LookupSettings(), EVariableType.Var, null));

o0.Exe();

//[[commandEnd]]0
}


public static readonly ScalarVal i94 = new ScalarVal(2d);
public static readonly ScalarVal i95 = new ScalarVal(3d);
public static readonly ScalarVal i96 = new ScalarVal(1d);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
