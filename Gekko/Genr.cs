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


public static void FunctionDef14() {

O.PrepareUfunction(3, "f");

O.Add3_UfunctionSpecialName(null, "f", (GekkoSmpl smpl, P p, bool q15, GekkoArg functionarg_xf7dke8cj_11_func, GekkoArg functionarg_xf7dke8cj_12_func, GekkoArg functionarg_xf7dke8cj_13_func) => 


{ IVariable functionarg_xf7dke8cj_11 = O.TypeCheck_date(functionarg_xf7dke8cj_11_func, smpl, 1);
IVariable functionarg_xf7dke8cj_12 = O.TypeCheck_date(functionarg_xf7dke8cj_12_func, smpl, 2);
IVariable functionarg_xf7dke8cj_13 = O.TypeCheck_series(functionarg_xf7dke8cj_13_func.f1(smpl), 3);

Databank local0 = Program.databanks.local;
Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
try {


return null; 
} 
catch { p.Deeper(); throw; }
 finally {
Program.databanks.local = local0; Program.databanks.localGlobal = lg0;p.RemoveLast();;
} 
});

}


public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

p.SetText(@"¤1");

FunctionDef14();


//[[commandEnd]]0



}
}
}
