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
public static void FunctionDef89() {


//[[splitSTOP]]

O.PrepareUfunction(0, "g");

Globals.ufunctions0.Add("g", (GekkoSmpl smpl, P p) => { 
p.SetText(@"¤1"); O.InitSmpl(smpl, p);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Hej", true, false))), false);
p.SetText(@"¤1"); O.InitSmpl(smpl, p);


//[[splitSTOP]]
return null;

//[[splitSTART]]


return null; 
});


//[[splitSTART]]

}

public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

//[[splitSTART]]
p.SetText(@"¤1"); O.InitSmpl(smpl, p);

FunctionDef89();


p.SetText(@"¤2"); O.InitSmpl(smpl, p);

O.FunctionLookup0("g")(smpl, p);


//[[splitSTOP]]


}
}
}
