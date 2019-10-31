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
p.SetText(@"¤2"); O.InitSmpl(smpl, p);


Func<GraphHelper, string> print2 = (gh) =>
{
O.Prt o2 = new O.Prt();
labelCounter = 0;o2.guiGraphIsRefreshing = gh.isRefreshing;
o2.guiGraphOperator = gh.operator2;
o2.guiGraphIsLogTransform = gh.isLogTransform;
o2.prtType = "p";
ESeriesMissing r1_2 = Program.options.series_array_print_missing; ESeriesMissing r2_2 = Program.options.series_array_calc_missing; ESeriesMissing r3_2 = Program.options.series_data_missing; try {
O.HandleOptionBankRef1(o2.opt_bank, o2.opt_ref); O.HandleMissing1(o2.opt_missing);
{
List<int> bankNumbers = null;
O.Prt.Element ope2 = new O.Prt.Element();
ope2.labelGiven = new List<string>() {"f¨(y, 'y')|[@60,121:121='f',<1354>,2:2]|[@67,130:130=')',<1301>,2:11]"};
smpl = new GekkoSmpl(o2.t1, o2.t2); smpl.t0 = smpl.t0.Add(-2);
ope2.operatorsFinal = Program.GetElementOperators(o2, ope2);bankNumbers = O.Prt.GetBankNumbers(null, ope2.operatorsFinal);
for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
int bankNumber = bankNumbers[bankNumberI];
smpl.bankNumber = bankNumber;
ope2.variable[bankNumber] = O.FunctionLookupNew4("f")(smpl, p, false, null, null, new GekkoArg((spml100) => O.Lookup(spml100, null, null, "y", null, null, new  LookupSettings(), EVariableType.Var, null), (spml100) => new ScalarString("y")), new GekkoArg((spml101) => O.HandleString(new ScalarString(@"y")), (spml101) => null));
if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope2);
}
smpl.bankNumber = 0;
o2.prtElements.Add(ope2);
}

o2.printCsCounter = Globals.printCs.Count - 1;
o2.Exe();
}
finally {
O.HandleOptionBankRef2(); O.HandleMissing2(r1_2, r2_2, r3_2);
}
return o2.emfName;
};
Globals.printCs.Add(Globals.printCs.Count, print2); 
print2(new GraphHelper());

//[[commandEnd]]2
}


public static readonly ScalarVal i83 = new ScalarVal(2011d, 0);
public static readonly ScalarVal i84 = new ScalarVal(2011d, 0);
public static void FunctionDef85() {

O.PrepareUfunction(4, "f");

Globals.ufunctionsNew4.Add("f", (GekkoSmpl smpl, P p, bool q86, GekkoArg functionarg_xf7dke8cj_79_func, GekkoArg functionarg_xf7dke8cj_80_func, GekkoArg functionarg_xf7dke8cj_81_func, GekkoArg functionarg_xf7dke8cj_82_func) => 


{ IVariable functionarg_xf7dke8cj_79 = O.TypeCheck_date(functionarg_xf7dke8cj_79_func, smpl, 1);
IVariable functionarg_xf7dke8cj_80 = O.TypeCheck_date(functionarg_xf7dke8cj_80_func, smpl, 2);
IVariable functionarg_xf7dke8cj_81 = O.TypeCheck_name(functionarg_xf7dke8cj_81_func.f2(smpl), 3);
IVariable functionarg_xf7dke8cj_82 = O.TypeCheck_string(functionarg_xf7dke8cj_82_func.f1(smpl), 4);

Databank local0 = Program.databanks.local;
Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("f"); p.SetLastFileSentToANTLR(O.LastText("f")); p.Deeper();
try {


//[[commandSpecial]]1
return O.TypeCheck_val(O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None,i83
), smpl, O.EIndexerType.None, O.Lookup(smpl, null, (functionarg_xf7dke8cj_81), null, new  LookupSettings(), EVariableType.Var, null), i83
), O.Indexer(O.Indexer2(smpl, O.EIndexerType.None,i84
), smpl, O.EIndexerType.None, O.Lookup(smpl, null, (functionarg_xf7dke8cj_82), null, new  LookupSettings(), EVariableType.Var, null), i84
)), 0);

//[[commandEnd]]1


return null; 
} 
catch { p.Deeper(); throw; }
 finally {
Program.databanks.local = local0; Program.databanks.localGlobal = lg0;p.RemoveLast();;
} 
});

O.PrepareUfunction(3, "f");

Globals.ufunctionsNew3.Add("f", (GekkoSmpl smpl, P p, bool q86, GekkoArg functionarg_xf7dke8cj_79_func, GekkoArg functionarg_xf7dke8cj_80_func, GekkoArg functionarg_xf7dke8cj_81_func) => 


{ 

if(q86) {

List<bool> questions87 = new List<bool> { q86 };
List<IVariable> defaultValueCodes88 = new List<IVariable> { O.HandleString(new ScalarString(@"y")) };
List<string> types89 = new List<string> { "string" };
List<IVariable> labelCodes90 = new List<IVariable> { O.HandleString(new ScalarString(@"variabel")) };
List<IVariable> promptResults91 = O.Prompt(questions87, defaultValueCodes88, types89, labelCodes90);
return O.FunctionLookupNew4("f")(smpl, p, false , functionarg_xf7dke8cj_79_func, functionarg_xf7dke8cj_80_func, functionarg_xf7dke8cj_81_func , new GekkoArg((spml92) => promptResults91[0], (spml92) => null));
}

else

{

return O.FunctionLookupNew4("f")(smpl, p, false , functionarg_xf7dke8cj_79_func, functionarg_xf7dke8cj_80_func, functionarg_xf7dke8cj_81_func , new GekkoArg((spml92) => O.HandleString(new ScalarString(@"y")), (spml92) => null));
}


 return null; });

O.PrepareUfunction(2, "f");

Globals.ufunctionsNew2.Add("f", (GekkoSmpl smpl, P p, bool q86, GekkoArg functionarg_xf7dke8cj_79_func, GekkoArg functionarg_xf7dke8cj_80_func) => 


{ 

if(q86) {

List<bool> questions93 = new List<bool> { q86, q86 };
List<IVariable> defaultValueCodes94 = new List<IVariable> { O.HandleString(new ScalarString(@"y")), O.HandleString(new ScalarString(@"y")) };
List<string> types95 = new List<string> { "name", "string" };
List<IVariable> labelCodes96 = new List<IVariable> { O.HandleString(new ScalarString(@"variabel")), O.HandleString(new ScalarString(@"variabel")) };
List<IVariable> promptResults97 = O.Prompt(questions93, defaultValueCodes94, types95, labelCodes96);
return O.FunctionLookupNew4("f")(smpl, p, false , functionarg_xf7dke8cj_79_func, functionarg_xf7dke8cj_80_func , new GekkoArg((spml98) => promptResults97[0], (spml98) => null), new GekkoArg((spml99) => promptResults97[1], (spml99) => null));
}

else

{

return O.FunctionLookupNew4("f")(smpl, p, false , functionarg_xf7dke8cj_79_func, functionarg_xf7dke8cj_80_func , new GekkoArg((spml98) => O.HandleString(new ScalarString(@"y")), (spml98) => null), new GekkoArg((spml99) => O.HandleString(new ScalarString(@"y")), (spml99) => null));
}


 return null; });

}


public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
FunctionDef85();


//[[commandEnd]]0


C0(smpl, p);



}
}
}
