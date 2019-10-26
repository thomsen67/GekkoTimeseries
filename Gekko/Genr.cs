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


O.Reset o0 = new O.Reset();
o0.p = p;o0.Exe(smpl);

//[[commandEnd]]0
}
public static void C1(GekkoSmpl smpl, P p) {
//[[commandStart]]7
p.SetText(@"¤6"); O.InitSmpl(smpl, p);

O.Assignment o7 = new O.Assignment();
o7.opt_source = @"<[code]>%y1 = f(3, 4)";


Action assign_25 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar20 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml23) => i21, (spml23) => null), new GekkoArg((spml24) => i22, (spml24) => null));
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "%y1", null, ivTmpvar20, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o7)
;
};
Func<bool> check_25 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar20 = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml23) => i21, (spml23) => null), new GekkoArg((spml24) => i22, (spml24) => null));
O.AdjustT0(smpl, 2);
if (ivTmpvar20.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.Lookup(smpl, null, null, "%y1", null, ivTmpvar20, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o7)
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_25, check_25, o7);

//[[commandEnd]]7


//[[commandStart]]8
p.SetText(@"¤7"); O.InitSmpl(smpl, p);

O.Assignment o8 = new O.Assignment();
o8.opt_source = @"<[code]>%y2 = f(3)";


Action assign_29 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar26 = O.FunctionLookupNew3("f")(smpl, p, false, null, null, new GekkoArg((spml28) => i27, (spml28) => null));
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "%y2", null, ivTmpvar26, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o8)
;
};
Func<bool> check_29 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar26 = O.FunctionLookupNew3("f")(smpl, p, false, null, null, new GekkoArg((spml28) => i27, (spml28) => null));
O.AdjustT0(smpl, 2);
if (ivTmpvar26.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.Lookup(smpl, null, null, "%y2", null, ivTmpvar26, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o8)
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_29, check_29, o8);

//[[commandEnd]]8


//[[commandStart]]9
p.SetText(@"¤8"); O.InitSmpl(smpl, p);

O.Assignment o9 = new O.Assignment();
o9.opt_source = @"<[code]>%y3 = g(3)";


Action assign_33 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar30 = O.FunctionLookupNew3("g")(smpl, p, false, null, null, new GekkoArg((spml32) => i31, (spml32) => null));
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "%y3", null, ivTmpvar30, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o9)
;
};
Func<bool> check_33 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar30 = O.FunctionLookupNew3("g")(smpl, p, false, null, null, new GekkoArg((spml32) => i31, (spml32) => null));
O.AdjustT0(smpl, 2);
if (ivTmpvar30.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.Lookup(smpl, null, null, "%y3", null, ivTmpvar30, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o9)
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_33, check_33, o9);

//[[commandEnd]]9


//[[commandStart]]10
p.SetText(@"¤9"); O.InitSmpl(smpl, p);


Program.Mem(null);

//[[commandEnd]]10
}


public static readonly ScalarVal i5 = new ScalarVal(666d, 0);
public static readonly ScalarVal i6 = new ScalarVal(777d, 0);
public static void FunctionDef7() {

O.PrepareUfunction(4, "f");

Globals.ufunctionsNew4.Add("f", (GekkoSmpl smpl, P p, bool b,  GekkoArg functionarg_xf7dke8cj_1_func,  GekkoArg functionarg_xf7dke8cj_2_func,  GekkoArg functionarg_xf7dke8cj_3_func,  GekkoArg functionarg_xf7dke8cj_4_func) => 


{ IVariable functionarg_xf7dke8cj_1 = O.TypeCheck_date(functionarg_xf7dke8cj_1_func, smpl, 1);
IVariable functionarg_xf7dke8cj_2 = O.TypeCheck_date(functionarg_xf7dke8cj_2_func, smpl, 2);
IVariable functionarg_xf7dke8cj_3 = O.TypeCheck_val(functionarg_xf7dke8cj_3_func.f1(smpl), 3);
IVariable functionarg_xf7dke8cj_4 = O.TypeCheck_val(functionarg_xf7dke8cj_4_func.f1(smpl), 4);

Databank local1 = Program.databanks.local;
Program.databanks.local = new Databank("Local"); LocalGlobal lg1 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
try {


//[[commandSpecial]]2
return O.TypeCheck_val(O.Add(smpl, functionarg_xf7dke8cj_3, functionarg_xf7dke8cj_4), 0);

//[[commandEnd]]2


return null; 
} 
catch { p.Deeper(); throw; }
 finally {
Program.databanks.local = local1; Program.databanks.localGlobal = lg1;p.RemoveLast();;
} 
});

O.PrepareUfunction(4, "f");

Globals.ufunctionsNew3.Add("f", (GekkoSmpl smpl, P p, bool b,  GekkoArg functionarg_xf7dke8cj_1_func,  GekkoArg functionarg_xf7dke8cj_2_func,  GekkoArg functionarg_xf7dke8cj_3_func) =>


{
    G.Writeln("Hej 3");
    return null; 
});

O.PrepareUfunction(4, "f");

Globals.ufunctionsNew2.Add("f", (GekkoSmpl smpl, P p, bool b,  GekkoArg functionarg_xf7dke8cj_1_func,  GekkoArg functionarg_xf7dke8cj_2_func) => 


{ G.Writeln("Hej 2");
return null; 
} 
);

}

public static readonly ScalarVal i11 = new ScalarVal(2d, 0);
public static void FunctionDef14() {

O.PrepareUfunction(3, "f");

Globals.ufunctionsNew3.Add("f", (GekkoSmpl smpl, P p, bool b,  GekkoArg functionarg_xf7dke8cj_8_func,  GekkoArg functionarg_xf7dke8cj_9_func,  GekkoArg functionarg_xf7dke8cj_10_func) => 


{ IVariable functionarg_xf7dke8cj_8 = O.TypeCheck_date(functionarg_xf7dke8cj_8_func, smpl, 1);
IVariable functionarg_xf7dke8cj_9 = O.TypeCheck_date(functionarg_xf7dke8cj_9_func, smpl, 2);
IVariable functionarg_xf7dke8cj_10 = O.TypeCheck_val(functionarg_xf7dke8cj_10_func.f1(smpl), 3);

Databank local3 = Program.databanks.local;
Program.databanks.local = new Databank("Local"); LocalGlobal lg3 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
try {


//[[commandSpecial]]4
return O.TypeCheck_val(O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml12) => functionarg_xf7dke8cj_10, (spml12) => null), new GekkoArg((spml13) => i11, (spml13) => null)), 0);

//[[commandEnd]]4


return null; 
} 
catch { p.Deeper(); throw; }
 finally {
Program.databanks.local = local3; Program.databanks.localGlobal = lg3;p.RemoveLast();;
} 
});

}

public static readonly ScalarVal i18 = new ScalarVal(2d, 0);
public static void FunctionDef19() {

O.PrepareUfunction(3, "g");

Globals.ufunctionsNew3.Add("g", (GekkoSmpl smpl, P p, bool b,  GekkoArg functionarg_xf7dke8cj_15_func,  GekkoArg functionarg_xf7dke8cj_16_func,  GekkoArg functionarg_xf7dke8cj_17_func) => 


{ IVariable functionarg_xf7dke8cj_15 = O.TypeCheck_date(functionarg_xf7dke8cj_15_func, smpl, 1);
IVariable functionarg_xf7dke8cj_16 = O.TypeCheck_date(functionarg_xf7dke8cj_16_func, smpl, 2);
IVariable functionarg_xf7dke8cj_17 = O.TypeCheck_val(functionarg_xf7dke8cj_17_func.f1(smpl), 3);

Databank local5 = Program.databanks.local;
Program.databanks.local = new Databank("Local"); LocalGlobal lg5 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("g"); p.SetLastFileSentToANTLR(O.LastText("g")); p.Deeper();
try {


//[[commandSpecial]]6
return O.TypeCheck_val(O.Add(smpl, functionarg_xf7dke8cj_17, i18), 0);

//[[commandEnd]]6


return null; 
} 
catch { p.Deeper(); throw; }
 finally {
Program.databanks.local = local5; Program.databanks.localGlobal = lg5;p.RemoveLast();;
} 
});

}

public static readonly ScalarVal i21 = new ScalarVal(3d, 0);
public static readonly ScalarVal i22 = new ScalarVal(4d, 0);
public static readonly ScalarVal i27 = new ScalarVal(3d, 0);
public static readonly ScalarVal i31 = new ScalarVal(3d, 0);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);

FunctionDef7();


//[[commandEnd]]1

FunctionDef14();


//[[commandEnd]]3

FunctionDef19();


//[[commandEnd]]5


C1(smpl, p);



}
}
}
