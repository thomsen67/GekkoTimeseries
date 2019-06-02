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
p.SetText(@"¤9"); O.InitSmpl(smpl, p);

O.Assignment o0 = new O.Assignment();
o0.opt_source = @"<[code]>pop = series(3)";


Action assign_3 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar1 = Functions.series(smpl, null, null, i2);
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "pop", null, ivTmpvar1, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
;
};
Func<bool> check_3 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar1 = Functions.series(smpl, null, null, i2);
O.AdjustT0(smpl, 2);
if (ivTmpvar1.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.Lookup(smpl, null, null, "pop", null, ivTmpvar1, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_3, check_3, o0);

//[[commandEnd]]0


//[[commandStart]]1
p.SetText(@"¤10"); O.InitSmpl(smpl, p);

O.Assignment o1 = new O.Assignment();
o1.opt_source = @"<[code]>pop[20, m, dk] = 100";


Action assign_7 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar4 = i6;
O.AdjustT0(smpl, 2);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar4, o1, i5
, new ScalarString("m"), new ScalarString("dk"))
;
};
Func<bool> check_7 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar4 = i6;
O.AdjustT0(smpl, 2);
if (ivTmpvar4.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar4, o1, i5
, new ScalarString("m"), new ScalarString("dk"))
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_7, check_7, o1);

//[[commandEnd]]1


//[[commandStart]]2
p.SetText(@"¤11"); O.InitSmpl(smpl, p);

O.Assignment o2 = new O.Assignment();
o2.opt_source = @"<[code]>pop[20, m, se] = 200";


Action assign_11 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar8 = i10;
O.AdjustT0(smpl, 2);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar8, o2, i9
, new ScalarString("m"), new ScalarString("se"))
;
};
Func<bool> check_11 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar8 = i10;
O.AdjustT0(smpl, 2);
if (ivTmpvar8.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar8, o2, i9
, new ScalarString("m"), new ScalarString("se"))
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_11, check_11, o2);

//[[commandEnd]]2


//[[commandStart]]3
p.SetText(@"¤12"); O.InitSmpl(smpl, p);

O.Assignment o3 = new O.Assignment();
o3.opt_source = @"<[code]>pop[20, k, dk] = 300";


Action assign_15 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar12 = i14;
O.AdjustT0(smpl, 2);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar12, o3, i13
, new ScalarString("k"), new ScalarString("dk"))
;
};
Func<bool> check_15 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar12 = i14;
O.AdjustT0(smpl, 2);
if (ivTmpvar12.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar12, o3, i13
, new ScalarString("k"), new ScalarString("dk"))
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_15, check_15, o3);

//[[commandEnd]]3


//[[commandStart]]4
p.SetText(@"¤13"); O.InitSmpl(smpl, p);

O.Assignment o4 = new O.Assignment();
o4.opt_source = @"<[code]>pop[20, k, se] = 400";


Action assign_19 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar16 = i18;
O.AdjustT0(smpl, 2);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar16, o4, i17
, new ScalarString("k"), new ScalarString("se"))
;
};
Func<bool> check_19 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar16 = i18;
O.AdjustT0(smpl, 2);
if (ivTmpvar16.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar16, o4, i17
, new ScalarString("k"), new ScalarString("se"))
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_19, check_19, o4);

//[[commandEnd]]4


//[[commandStart]]5
p.SetText(@"¤14"); O.InitSmpl(smpl, p);

O.Assignment o5 = new O.Assignment();
o5.opt_source = @"<[code]>pop[21, m, dk] = 500";


Action assign_23 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar20 = i22;
O.AdjustT0(smpl, 2);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar20, o5, i21
, new ScalarString("m"), new ScalarString("dk"))
;
};
Func<bool> check_23 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar20 = i22;
O.AdjustT0(smpl, 2);
if (ivTmpvar20.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar20, o5, i21
, new ScalarString("m"), new ScalarString("dk"))
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_23, check_23, o5);

//[[commandEnd]]5


//[[commandStart]]6
p.SetText(@"¤15"); O.InitSmpl(smpl, p);

O.Assignment o6 = new O.Assignment();
o6.opt_source = @"<[code]>pop[21, m, se] = 600";


Action assign_27 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar24 = i26;
O.AdjustT0(smpl, 2);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar24, o6, i25
, new ScalarString("m"), new ScalarString("se"))
;
};
Func<bool> check_27 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar24 = i26;
O.AdjustT0(smpl, 2);
if (ivTmpvar24.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar24, o6, i25
, new ScalarString("m"), new ScalarString("se"))
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_27, check_27, o6);

//[[commandEnd]]6


//[[commandStart]]7
p.SetText(@"¤16"); O.InitSmpl(smpl, p);

O.Assignment o7 = new O.Assignment();
o7.opt_source = @"<[code]>pop[21, k, dk] = 700";


Action assign_31 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar28 = i30;
O.AdjustT0(smpl, 2);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar28, o7, i29
, new ScalarString("k"), new ScalarString("dk"))
;
};
Func<bool> check_31 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar28 = i30;
O.AdjustT0(smpl, 2);
if (ivTmpvar28.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar28, o7, i29
, new ScalarString("k"), new ScalarString("dk"))
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_31, check_31, o7);

//[[commandEnd]]7


//[[commandStart]]8
p.SetText(@"¤17"); O.InitSmpl(smpl, p);

O.Assignment o8 = new O.Assignment();
o8.opt_source = @"<[code]>pop[21, k, se] = 800";


Action assign_35 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar32 = i34;
O.AdjustT0(smpl, 2);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar32, o8, i33
, new ScalarString("k"), new ScalarString("se"))
;
};
Func<bool> check_35 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar32 = i34;
O.AdjustT0(smpl, 2);
if (ivTmpvar32.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null),  ivTmpvar32, o8, i33
, new ScalarString("k"), new ScalarString("se"))
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_35, check_35, o8);

//[[commandEnd]]8


//[[commandStart]]9
p.SetText(@"¤19"); O.InitSmpl(smpl, p);

O.Assignment o9 = new O.Assignment();
o9.opt_source = @"<[code]>#a = ('20', '21')";


Action assign_37 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar36 = O.ListDefHelper(O.HandleString(new ScalarString(@"20")), null, O.HandleString(new ScalarString(@"21")), null);
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "#a", null, ivTmpvar36, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o9)
;
};
Func<bool> check_37 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar36 = O.ListDefHelper(O.HandleString(new ScalarString(@"20")), null, O.HandleString(new ScalarString(@"21")), null);
O.AdjustT0(smpl, 2);
if (ivTmpvar36.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.Lookup(smpl, null, null, "#a", null, ivTmpvar36, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o9)
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_37, check_37, o9);

//[[commandEnd]]9


//[[commandStart]]10
p.SetText(@"¤20"); O.InitSmpl(smpl, p);

O.Assignment o10 = new O.Assignment();
o10.opt_source = @"<[code]>#s = ('m', 'k')";


Action assign_39 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar38 = O.ListDefHelper(O.HandleString(new ScalarString(@"m")), null, O.HandleString(new ScalarString(@"k")), null);
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "#s", null, ivTmpvar38, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o10)
;
};
Func<bool> check_39 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar38 = O.ListDefHelper(O.HandleString(new ScalarString(@"m")), null, O.HandleString(new ScalarString(@"k")), null);
O.AdjustT0(smpl, 2);
if (ivTmpvar38.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.Lookup(smpl, null, null, "#s", null, ivTmpvar38, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o10)
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_39, check_39, o10);

//[[commandEnd]]10


//[[commandStart]]11
p.SetText(@"¤21"); O.InitSmpl(smpl, p);

O.Assignment o11 = new O.Assignment();
o11.opt_source = @"<[code]>#o = ('dk', 'se')";


Action assign_41 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar40 = O.ListDefHelper(O.HandleString(new ScalarString(@"dk")), null, O.HandleString(new ScalarString(@"se")), null);
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "#o", null, ivTmpvar40, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o11)
;
};
Func<bool> check_41 = () => {
O.AdjustT0(smpl, -2);
IVariable ivTmpvar40 = O.ListDefHelper(O.HandleString(new ScalarString(@"dk")), null, O.HandleString(new ScalarString(@"se")), null);
O.AdjustT0(smpl, 2);
if (ivTmpvar40.Type() != EVariableType.Series) return false;
O.Dynamic1(smpl);
O.Lookup(smpl, null, null, "#o", null, ivTmpvar40, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o11)
;
return O.Dynamic2(smpl);
};
O.RunAssigmentMaybeDynamic(smpl, assign_41, check_41, o11);

//[[commandEnd]]11


//[[commandStart]]12
p.SetText(@"¤23"); O.InitSmpl(smpl, p);
IVariable listloopMovedStuff_47 = O.Lookup(smpl, null, null, "#a", null, null, new  LookupSettings(), EVariableType.Var, null);
IVariable listloopMovedStuff_48 = O.Lookup(smpl, null, null, "#s", null, null, new  LookupSettings(), EVariableType.Var, null);
IVariable listloopMovedStuff_49 = O.Lookup(smpl, null, null, "#o", null, null, new  LookupSettings(), EVariableType.Var, null);
IVariable listloopMovedStuff_50 = O.Lookup(smpl, null, null, "pop", null, null, new  LookupSettings(), EVariableType.Var, null);
IVariable listloopMovedStuff_51 = O.Lookup(smpl, null, null, "#a", null, null, new  LookupSettings(), EVariableType.Var, null);
IVariable listloopMovedStuff_52 = O.Lookup(smpl, null, null, "#s", null, null, new  LookupSettings(), EVariableType.Var, null);
IVariable listloopMovedStuff_53 = O.Lookup(smpl, null, null, "#o", null, null, new  LookupSettings(), EVariableType.Var, null);

Func<IVariable> func55 = () => {
var smplCommandRemember56 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
Series temp54 = new Series(ESeriesType.Normal, Program.options.freq, null); temp54.SetZero(smpl);

foreach (IVariable listloop_a44 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, new  LookupSettings(), EVariableType.Var, null))) {
foreach (IVariable listloop_s45 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, new  LookupSettings(), EVariableType.Var, null))) {
foreach (IVariable listloop_o46 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("o")))), null, new  LookupSettings(), EVariableType.Var, null))) {
temp54.InjectAdd(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None,listloop_a44, listloop_s45, listloop_o46), smpl, O.EIndexerType.None, listloopMovedStuff_50, listloop_a44, listloop_s45, listloop_o46));

labelCounter++;
}
}
}
labelCounter = 0;
smpl.command = smplCommandRemember56;
return temp54;

};

Func<GekkoSmpl, IVariable> Evalcode57 = (smpl5) => { return func55();
 };

O.Decomp2 o12 = new O.Decomp2();
o12.label = @"sum((#a, #s, #o), pop[#a, #s, #o])";
o12.t1 = O.ConvertToDate(i42, O.GetDateChoices.FlexibleStart);
;
o12.t2 = O.ConvertToDate(i43, O.GetDateChoices.FlexibleEnd);
;

o12.opt_prtcode = O.ConvertToString((new ScalarString("q")));



o12.expression = Evalcode57;

o12.where.Add(new List<IVariable>() {O.HandleString(new ScalarString(@"se")), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> {O.scalarStringHash.Concat(null, new ScalarString("o"))}))});
o12.where.Add(new List<IVariable>() {O.HandleString(new ScalarString(@"se")), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> {O.scalarStringHash.Concat(null, new ScalarString("o"))}))});

o12.agg.Add(new List<IVariable>() {O.ExplodeIvariablesSeq(false, new List(new List<IVariable> {O.scalarStringHash.Concat(null, new ScalarString("a"))})), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> {O.scalarStringHash.Concat(null, new ScalarString("a_agg"))})), O.HandleString(new ScalarString(@"10-year")), O.HandleString(new ScalarString(@"27"))});
o12.agg.Add(new List<IVariable>() {O.ExplodeIvariablesSeq(false, new List(new List<IVariable> {O.scalarStringHash.Concat(null, new ScalarString("a"))})), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> {O.scalarStringHash.Concat(null, new ScalarString("a_agg"))})), O.HandleString(new ScalarString(@"10-year")), O.HandleString(new ScalarString(@"27"))});


o12.Exe();

//[[commandEnd]]12
}


public static readonly ScalarVal i2 = new ScalarVal(3d);
public static readonly ScalarVal i5 = new ScalarVal(20d);
public static readonly ScalarVal i6 = new ScalarVal(100d);
public static readonly ScalarVal i9 = new ScalarVal(20d);
public static readonly ScalarVal i10 = new ScalarVal(200d);
public static readonly ScalarVal i13 = new ScalarVal(20d);
public static readonly ScalarVal i14 = new ScalarVal(300d);
public static readonly ScalarVal i17 = new ScalarVal(20d);
public static readonly ScalarVal i18 = new ScalarVal(400d);
public static readonly ScalarVal i21 = new ScalarVal(21d);
public static readonly ScalarVal i22 = new ScalarVal(500d);
public static readonly ScalarVal i25 = new ScalarVal(21d);
public static readonly ScalarVal i26 = new ScalarVal(600d);
public static readonly ScalarVal i29 = new ScalarVal(21d);
public static readonly ScalarVal i30 = new ScalarVal(700d);
public static readonly ScalarVal i33 = new ScalarVal(21d);
public static readonly ScalarVal i34 = new ScalarVal(800d);
public static readonly ScalarVal i42 = new ScalarVal(2010d);
public static readonly ScalarVal i43 = new ScalarVal(2012d);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
