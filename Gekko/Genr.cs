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
public static void FunctionDef2() {


//[[splitSTOP]]

O.PrepareUfunction(1, "gamy");

Globals.ufunctions1.Add("gamy", (GekkoSmpl smpl, IVariable functionarg_1) => { p.SetText(@"¤8"); O.InitSmpl(smpl);
O.Sys o1 = new O.Sys();
o1.s = O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%gamY", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@" ", true, false))), O.Lookup(smpl, null, null, "%parameters", null, null, false, EVariableType.Var));
o1.Exe();

p.SetText(@"¤9"); O.InitSmpl(smpl);

//[[splitSTOP]]
return O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"gamY finished running ", true, false)), O.Lookup(smpl, null, null, "%parameters", null, null, false, EVariableType.Var));

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static void FunctionDef8() {


//[[splitSTOP]]

O.PrepareUfunction(2, "simx");

Globals.ufunctions2.Add("simx", (GekkoSmpl smpl, IVariable functionarg_3, IVariable functionarg_4) => { p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar5 = O.IvConvertTo(EVariableType.String, new ScalarString(ScalarString.SubstituteScalarsInString(@"gamY\dist\gamY\gamY.exe", true, false)));
O.Lookup(smpl, null, null, "gamY", null, ivTmpvar5, true, EVariableType.String)
;

p.SetText(@"¤14"); O.InitSmpl(smpl);
O.Write o5 = new O.Write();

o5.opt_gdx = "yes";

o5.fileName = O.ConvertToString((new ScalarString("calibration", true, false)).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("gdx", true, false))).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("simRAW", true, false))));

o5.listItems = new List<string>();


o5.type = @"Export";o5.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar6 = O.IvConvertTo(EVariableType.String, O.Lookup(smpl, null, null, "%endogenize", null, null, false, EVariableType.Var));
O.Lookup(smpl, null, null, "endoNY", null, ivTmpvar6, true, EVariableType.String)
;

p.SetText(@"¤0"); O.InitSmpl(smpl);
IVariable ivTmpvar7 = O.IvConvertTo(EVariableType.String, O.Lookup(smpl, null, null, "%var1", null, null, false, EVariableType.Var));
O.Lookup(smpl, null, null, "varNY", null, ivTmpvar7, true, EVariableType.String)
;

p.SetText(@"¤17"); O.InitSmpl(smpl);
Program.Tell(O.ConvertToString(O.FunctionLookup1("gamy")(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"Calibration\sim.gms r=Saved\calib_gaps --var1=%varny --announce=2019 --endo1=%endoNY", true, false)))), false);
p.SetText(@"¤18"); O.InitSmpl(smpl);
O.Run o9 = new O.Run();
o9.fileName = O.ConvertToString((new ScalarString("sim_import", true, false)));
o9.p = p;
o9.Exe();

p.SetText(@"¤19"); O.InitSmpl(smpl);

//[[splitSTOP]]
return new ScalarString(ScalarString.SubstituteScalarsInString(@"", true, false));

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

//[[splitSTART]]
p.SetText(@"¤7"); O.InitSmpl(smpl);
FunctionDef2();


p.SetText(@"¤12"); O.InitSmpl(smpl);
FunctionDef8();



//[[splitSTOP]]


}
}
}
