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
//[[commandStart]]2
p.SetText(@"¤1"); O.InitSmpl(smpl, p);

O.Assignment o2 = new O.Assignment();
O.AdjustT0(smpl, -1);
//IVariable ivTmpvar6 = O.FunctionLookup0("f")(smpl, p);
O.AdjustT0(smpl, 1);
//O.Lookup(smpl, null, null, "%v", null, ivTmpvar6, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
;

//[[commandEnd]]2
}

public static void CC0(GekkoSmpl smpl, P p) {
//[[commandStart]]1
p.SetText(@"¤1"); O.InitSmpl(smpl, p);

Program.Tell(O.ConvertToString(O.HandleString(new ScalarString(@"a1y"))), false);
//[[commandEnd]]1
}

public static void FunctionDef5() {

O.PrepareUfunction(0, "f");

Globals.ufunctions0.Add("f", (GekkoSmpl smpl, P p) => 

{ Databank local0 = Program.databanks.local;
Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal();
try {


CC0(smpl, p);


return null; 
} 
finally {
Program.databanks.local = local0; Program.databanks.localGlobal = lg0;
}
});

}


public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
FunctionDef5();


//[[commandEnd]]0


C0(smpl, p);



}
}
}
