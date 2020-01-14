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


O.Decomp2 o0 = new O.Decomp2();
o0.type = @"ASTDECOMP3";
o0.label = @"folk2[#a, #s, #o, #n] in f";
o0.t1 = Globals.globalPeriodStart;
o0.t2 = Globals.globalPeriodEnd;

o0.opt_prtcode = O.ConvertToString((new ScalarString("d")));



o0.decompItems.Add(new DecompItems(null, O.ExplodeIvariablesSeq(false, new List(new List<IVariable> {new ScalarString("folk2").Concat(null, new ScalarString("[").Concat(null, O.Lookup(smpl, null, null, "#a", null, null, new  LookupSettings(), EVariableType.Var, null)).Concat(null, new ScalarString(",")).Concat(null, O.Lookup(smpl, null, null, "#s", null, null, new  LookupSettings(), EVariableType.Var, null)).Concat(null, new ScalarString(",")).Concat(null, O.Lookup(smpl, null, null, "#o", null, null, new  LookupSettings(), EVariableType.Var, null)).Concat(null, new ScalarString(",")).Concat(null, O.Lookup(smpl, null, null, "#n", null, null, new  LookupSettings(), EVariableType.Var, null)).Concat(null, new ScalarString("]")))})), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> {new ScalarString("f")}))));






o0.Exe();

//[[commandEnd]]0
}



public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
