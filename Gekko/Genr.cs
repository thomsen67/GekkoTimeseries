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
public static readonly ScalarVal i1 = new ScalarVal(200d);
public static void FunctionDef3() {


//[[splitSTOP]]

O.PrepareUfunction(1, "pipex");

Globals.ufunctions1.Add("pipex", (GekkoSmpl smpl, P p, IVariable functionarg_2) => { p.SetText(@"¤12"); O.InitSmpl(smpl);

O.Pipe o6 = new O.Pipe();
o6.opt_html = "yes";

o6.opt_append = "yes";

o6.fileName = O.ConvertToString(O.Add(smpl, O.Lookup(smpl, null, null, "%output", null, null, false, EVariableType.Var), functionarg_2));

o6.Exe();

p.SetText(@"¤13"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"<tr style=""page-break-inside:avoid; page-break-after:auto""><td><img src=""..\graphs\{i}.svg"" width=""30%""></td>", true, false))), false);
p.SetText(@"¤14"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"<td valign=""top"">", true, false))), false);
p.SetText(@"¤15"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"</td>", true, false))), false);
p.SetText(@"¤16"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"</tr>", true, false))), false);
p.SetText(@"¤17"); O.InitSmpl(smpl);

O.Pipe o11 = new O.Pipe();
o11.fileName = O.ConvertToString((new ScalarString("con")));

o11.Exe();

p.SetText(@"¤18"); O.InitSmpl(smpl);


//[[splitSTOP]]
return O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"Finished pipex to ", true, false)), functionarg_2);

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static void FunctionDef5() {


//[[splitSTOP]]

O.PrepareUfunction(1, "pipexx");

Globals.ufunctions1.Add("pipexx", (GekkoSmpl smpl, P p, IVariable functionarg_4) => { p.SetText(@"¤22"); O.InitSmpl(smpl);

O.Pipe o14 = new O.Pipe();
o14.opt_html = "yes";

o14.opt_append = "yes";

o14.fileName = O.ConvertToString(O.Add(smpl, O.Lookup(smpl, null, null, "%output", null, null, false, EVariableType.Var), functionarg_4));

o14.Exe();

p.SetText(@"¤23"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"<tr style=""page-break-inside:avoid; page-break-after:auto""><td><img src=""..\graphs\{i}_{u}.svg"" width=""30%""></td>", true, false))), false);
p.SetText(@"¤24"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"<td valign=""top"">", true, false))), false);
p.SetText(@"¤25"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"</td>", true, false))), false);
p.SetText(@"¤26"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"</tr>", true, false))), false);
p.SetText(@"¤27"); O.InitSmpl(smpl);

O.Pipe o19 = new O.Pipe();
o19.fileName = O.ConvertToString((new ScalarString("con")));

o19.Exe();

p.SetText(@"¤28"); O.InitSmpl(smpl);


//[[splitSTOP]]
return O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"Finished pipexx to ", true, false)), functionarg_4);

//[[splitSTART]]

 ; return null; });


//[[splitSTART]]

}

public static readonly ScalarVal i6 = new ScalarVal(2014d);
public static readonly ScalarVal i7 = new ScalarVal(2050d);
public static readonly ScalarVal d9 = new ScalarVal(0.0d);
public static readonly ScalarVal d10 = new ScalarVal(0.0d);
public static readonly ScalarVal d11 = new ScalarVal(0.0d);
public static readonly ScalarVal d12 = new ScalarVal(0.0d);
public static readonly ScalarVal i13 = new ScalarVal(10000d);
public static readonly ScalarVal d14 = new ScalarVal(0.0d);
public static readonly ScalarVal i15 = new ScalarVal(2014d);
public static readonly ScalarVal i17 = new ScalarVal(2015d);
public static readonly ScalarVal i19 = new ScalarVal(2016d);
public static readonly ScalarVal i21 = new ScalarVal(2017d);
public static readonly ScalarVal i23 = new ScalarVal(2018d);
public static readonly ScalarVal i25 = new ScalarVal(2019d);
public static readonly ScalarVal i27 = new ScalarVal(2020d);
public static readonly ScalarVal i29 = new ScalarVal(2021d);
public static readonly ScalarVal i31 = new ScalarVal(2022d);
public static readonly ScalarVal i33 = new ScalarVal(2023d);
public static readonly ScalarVal i35 = new ScalarVal(2024d);
public static readonly ScalarVal i37 = new ScalarVal(2025d);
public static readonly ScalarVal i39 = new ScalarVal(2030d);
public static readonly ScalarVal i41 = new ScalarVal(2040d);
public static readonly ScalarVal i43 = new ScalarVal(2050d);
public static readonly ScalarVal i45 = new ScalarVal(2060d);
public static readonly ScalarVal i47 = new ScalarVal(2100d);
public static readonly ScalarVal i49 = new ScalarVal(100d);
public static readonly ScalarVal i50 = new ScalarVal(3d);
public static readonly ScalarVal d54 = new ScalarVal(0.0d);
public static readonly ScalarVal d55 = new ScalarVal(0.0d);
public static readonly ScalarVal d56 = new ScalarVal(0.0d);
public static readonly ScalarVal d57 = new ScalarVal(0.0d);
public static readonly ScalarVal i58 = new ScalarVal(2014d);
public static readonly ScalarVal i60 = new ScalarVal(2015d);
public static readonly ScalarVal i62 = new ScalarVal(2016d);
public static readonly ScalarVal i64 = new ScalarVal(2017d);
public static readonly ScalarVal i66 = new ScalarVal(2018d);
public static readonly ScalarVal i68 = new ScalarVal(2019d);
public static readonly ScalarVal i70 = new ScalarVal(2020d);
public static readonly ScalarVal i72 = new ScalarVal(2021d);
public static readonly ScalarVal i74 = new ScalarVal(2022d);
public static readonly ScalarVal i76 = new ScalarVal(2023d);
public static readonly ScalarVal i78 = new ScalarVal(2024d);
public static readonly ScalarVal i80 = new ScalarVal(2025d);
public static readonly ScalarVal i82 = new ScalarVal(2030d);
public static readonly ScalarVal i84 = new ScalarVal(2040d);
public static readonly ScalarVal i86 = new ScalarVal(2050d);
public static readonly ScalarVal i88 = new ScalarVal(2060d);
public static readonly ScalarVal i90 = new ScalarVal(2100d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

//[[splitSTART]]
p.SetText(@"¤0"); O.InitSmpl(smpl);

O.Reset o0 = new O.Reset();
o0.p = p;o0.Exe(smpl);

p.SetText(@"¤2"); O.InitSmpl(smpl);

O.Mode o1 = new O.Mode();
o1.mode = @"data";o1.Exe();

p.SetText(@"¤4"); O.InitSmpl(smpl);

Program.options.databank_create_auto = true;
G.Writeln();
G.Writeln("option databank create auto = " + "yes" + "");

p.SetText(@"¤5"); O.InitSmpl(smpl);

Program.options.series_array_ignoremissing = true;
G.Writeln();
G.Writeln("option series array ignoremissing = " + "yes" + "");

p.SetText(@"¤6"); O.InitSmpl(smpl);

Program.options.print_width = 200;
G.Writeln();
G.Writeln("option print width = " + 200 + "");

p.SetText(@"¤11"); O.InitSmpl(smpl);

FunctionDef3();


p.SetText(@"¤21"); O.InitSmpl(smpl);

FunctionDef5();


p.SetText(@"¤33"); O.InitSmpl(smpl);

O.Time o21 = new O.Time();
o21.t1 = O.ConvertToDate(i6, O.GetDateChoices.FlexibleStart);
;
o21.t2 = O.ConvertToDate(i7, O.GetDateChoices.FlexibleEnd);
;

o21.Exe();

p.SetText(@"¤35"); O.InitSmpl(smpl);

ClearTS(p);
O.Read o22 = new O.Read();
o22.p = p;
o22.type = @"import";
o22.opt_gdx = "yes";

o22.opt_ref = "yes";

o22.fileName = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Calibration\GDX\calib.gdx", true, false)));


o22.Exe();

p.SetText(@"¤36"); O.InitSmpl(smpl);

ClearTS(p);
O.Read o23 = new O.Read();
o23.p = p;
o23.type = @"import";
o23.opt_gdx = "yes";

o23.fileName = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Calibration\GDX\shock.gdx", true, false)));


o23.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);

IVariable ivTmpvar8 = O.IvConvertTo(EVariableType.String, new ScalarString(ScalarString.SubstituteScalarsInString(@"analyze\output\", true, false)));
O.Lookup(smpl, null, null, "output", null, ivTmpvar8, true, EVariableType.String)
;

p.SetText(@"¤40"); O.InitSmpl(smpl);

Program.options.timefilter = false;
G.Writeln();
G.Writeln("option timefilter = " + "no" + "");

p.SetText(@"¤41"); O.InitSmpl(smpl);

O.Prt o26 = new O.Prt();
o26.prtType = "plot";

o26.t1 = Globals.globalPeriodStart;
o26.t2 = Globals.globalPeriodEnd;

o26.opt_title = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Noegletal", true, false)));

o26.opt_subtitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Grundforloeb", true, false)));

o26.opt_ytitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Pct", true, false)));

o26.printCodes.Add(new OptString("pch", O.ConvertToString(new ScalarString("yes"))));


o26.opt_pointsize = O.ConvertToVal(d9);


o26.opt_filename = O.ConvertToString((new ScalarString("Analyze")).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("Graphs"))).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("Supply_Use_base")).Add(smpl, new ScalarString(".")).Add(smpl, (new ScalarString("svg")))));


{
List<int> bankNumbers = null;
O.Prt.Element ope26 = new O.Prt.Element();
ope26.label = O.SubstituteScalarsAndLists("@qfGDP", false);
smpl = new GekkoSmpl(o26.t1.Add(-2), o26.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o26, ope26));
foreach(int bankNumber in bankNumbers) {
ope26.subElements = new List<O.Prt.SubElement>();
ope26.subElements.Add(new O.Prt.SubElement());
ope26.subElements[0].tsWork = O.Lookup(smpl, null, "REF", "qfGDP", null, null, false, EVariableType.Var);
}
o26.prtElements.Add(ope26);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope26 = new O.Prt.Element();
ope26.label = O.SubstituteScalarsAndLists("@qfC", false);
smpl = new GekkoSmpl(o26.t1.Add(-2), o26.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o26, ope26));
foreach(int bankNumber in bankNumbers) {
ope26.subElements = new List<O.Prt.SubElement>();
ope26.subElements.Add(new O.Prt.SubElement());
ope26.subElements[0].tsWork = O.Lookup(smpl, null, "REF", "qfC", null, null, false, EVariableType.Var);
}
o26.prtElements.Add(ope26);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope26 = new O.Prt.Element();
ope26.label = O.SubstituteScalarsAndLists("@qfG", false);
smpl = new GekkoSmpl(o26.t1.Add(-2), o26.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o26, ope26));
foreach(int bankNumber in bankNumbers) {
ope26.subElements = new List<O.Prt.SubElement>();
ope26.subElements.Add(new O.Prt.SubElement());
ope26.subElements[0].tsWork = O.Lookup(smpl, null, "REF", "qfG", null, null, false, EVariableType.Var);
}
o26.prtElements.Add(ope26);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope26 = new O.Prt.Element();
ope26.label = O.SubstituteScalarsAndLists("@qfI", false);
smpl = new GekkoSmpl(o26.t1.Add(-2), o26.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o26, ope26));
foreach(int bankNumber in bankNumbers) {
ope26.subElements = new List<O.Prt.SubElement>();
ope26.subElements.Add(new O.Prt.SubElement());
ope26.subElements[0].tsWork = O.Lookup(smpl, null, "REF", "qfI", null, null, false, EVariableType.Var);
}
o26.prtElements.Add(ope26);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope26 = new O.Prt.Element();
ope26.label = O.SubstituteScalarsAndLists("@qfM", false);
smpl = new GekkoSmpl(o26.t1.Add(-2), o26.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o26, ope26));
foreach(int bankNumber in bankNumbers) {
ope26.subElements = new List<O.Prt.SubElement>();
ope26.subElements.Add(new O.Prt.SubElement());
ope26.subElements[0].tsWork = O.Lookup(smpl, null, "REF", "qfM", null, null, false, EVariableType.Var);
}
o26.prtElements.Add(ope26);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope26 = new O.Prt.Element();
ope26.label = O.SubstituteScalarsAndLists("@qfX", false);
smpl = new GekkoSmpl(o26.t1.Add(-2), o26.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o26, ope26));
foreach(int bankNumber in bankNumbers) {
ope26.subElements = new List<O.Prt.SubElement>();
ope26.subElements.Add(new O.Prt.SubElement());
ope26.subElements[0].tsWork = O.Lookup(smpl, null, "REF", "qfX", null, null, false, EVariableType.Var);
}
o26.prtElements.Add(ope26);
}


o26.counter = 1;
o26.Exe();

p.SetText(@"¤42"); O.InitSmpl(smpl);

O.Prt o27 = new O.Prt();
o27.prtType = "plot";

o27.t1 = Globals.globalPeriodStart;
o27.t2 = Globals.globalPeriodEnd;

o27.opt_title = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Arbejdsmarkedet", true, false)));

o27.opt_subtitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Grundforloeb", true, false)));

o27.opt_ytitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"1000 personer", true, false)));

o27.opt_pointsize = O.ConvertToVal(d10);


o27.opt_filename = O.ConvertToString((new ScalarString("Analyze")).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("Graphs"))).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("Labor_market_base")).Add(smpl, new ScalarString(".")).Add(smpl, (new ScalarString("svg")))));


{
List<int> bankNumbers = null;
O.Prt.Element ope27 = new O.Prt.Element();
ope27.label = O.SubstituteScalarsAndLists("@nEmployed['tot']", false);
smpl = new GekkoSmpl(o27.t1.Add(-2), o27.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o27, ope27));
foreach(int bankNumber in bankNumbers) {
ope27.subElements = new List<O.Prt.SubElement>();
ope27.subElements.Add(new O.Prt.SubElement());
ope27.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
), smpl, O.Lookup(smpl, null, "REF", "nEmployed", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
);
}
o27.prtElements.Add(ope27);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope27 = new O.Prt.Element();
ope27.label = O.SubstituteScalarsAndLists("[<{THIS IS A LABEL}>]nUnemployed (hoejre)", false);
smpl = new GekkoSmpl(o27.t1.Add(-2), o27.t2);
ope27.y2 = O.ConvertToString("yes");

bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o27, ope27));
foreach(int bankNumber in bankNumbers) {
ope27.subElements = new List<O.Prt.SubElement>();
ope27.subElements.Add(new O.Prt.SubElement());
ope27.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
), smpl, O.Lookup(smpl, null, "REF", "nunemployed", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
);
}
o27.prtElements.Add(ope27);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope27 = new O.Prt.Element();
ope27.label = O.SubstituteScalarsAndLists("@nLaborForce['tot']", false);
smpl = new GekkoSmpl(o27.t1.Add(-2), o27.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o27, ope27));
foreach(int bankNumber in bankNumbers) {
ope27.subElements = new List<O.Prt.SubElement>();
ope27.subElements.Add(new O.Prt.SubElement());
ope27.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
), smpl, O.Lookup(smpl, null, "REF", "nLaborForce", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
);
}
o27.prtElements.Add(ope27);
}


o27.counter = 2;
o27.Exe();

p.SetText(@"¤43"); O.InitSmpl(smpl);

O.Prt o28 = new O.Prt();
o28.prtType = "plot";

o28.t1 = Globals.globalPeriodStart;
o28.t2 = Globals.globalPeriodEnd;

o28.opt_title = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Loen og priser", true, false)));

o28.opt_subtitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Grundforloeb", true, false)));

o28.opt_ytitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Pct", true, false)));

o28.opt_pointsize = O.ConvertToVal(d11);


o28.opt_filename = O.ConvertToString((new ScalarString("Analyze")).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("Graphs"))).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("Wages_prices_base")).Add(smpl, new ScalarString(".")).Add(smpl, (new ScalarString("svg")))));


{
List<int> bankNumbers = null;
O.Prt.Element ope28 = new O.Prt.Element();
ope28.label = O.SubstituteScalarsAndLists("@pL", false);
smpl = new GekkoSmpl(o28.t1.Add(-2), o28.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o28, ope28));
foreach(int bankNumber in bankNumbers) {
ope28.subElements = new List<O.Prt.SubElement>();
ope28.subElements.Add(new O.Prt.SubElement());
ope28.subElements[0].tsWork = O.Lookup(smpl, null, "REF", "pL", null, null, false, EVariableType.Var);
}
o28.prtElements.Add(ope28);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope28 = new O.Prt.Element();
ope28.label = O.SubstituteScalarsAndLists("@pC['tot']", false);
smpl = new GekkoSmpl(o28.t1.Add(-2), o28.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o28, ope28));
foreach(int bankNumber in bankNumbers) {
ope28.subElements = new List<O.Prt.SubElement>();
ope28.subElements.Add(new O.Prt.SubElement());
ope28.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
), smpl, O.Lookup(smpl, null, "REF", "pC", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
);
}
o28.prtElements.Add(ope28);
}


o28.counter = 3;
o28.Exe();

p.SetText(@"¤44"); O.InitSmpl(smpl);

O.Prt o29 = new O.Prt();
o29.prtType = "plot";

o29.t1 = Globals.globalPeriodStart;
o29.t2 = Globals.globalPeriodEnd;

o29.opt_title = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Demografi", true, false)));

o29.opt_subtitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Grundforloeb", true, false)));

o29.opt_ytitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"1000 personer", true, false)));

o29.opt_pointsize = O.ConvertToVal(d12);


o29.opt_filename = O.ConvertToString((new ScalarString("Analyze")).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("Graphs"))).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("Demographics_base")).Add(smpl, new ScalarString(".")).Add(smpl, (new ScalarString("svg")))));


{
List<int> bankNumbers = null;
O.Prt.Element ope29 = new O.Prt.Element();
ope29.label = O.SubstituteScalarsAndLists("@nPop['tot']", false);
smpl = new GekkoSmpl(o29.t1.Add(-2), o29.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o29, ope29));
foreach(int bankNumber in bankNumbers) {
ope29.subElements = new List<O.Prt.SubElement>();
ope29.subElements.Add(new O.Prt.SubElement());
ope29.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
), smpl, O.Lookup(smpl, null, "REF", "nPop", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
);
}
o29.prtElements.Add(ope29);
}


o29.counter = 4;
o29.Exe();

p.SetText(@"¤45"); O.InitSmpl(smpl);

O.Prt o30 = new O.Prt();
o30.prtType = "plot";

o30.t1 = Globals.globalPeriodStart;
o30.t2 = Globals.globalPeriodEnd;

o30.opt_title = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Indkomst og velstand", true, false)));

o30.opt_subtitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Grundforloeb", true, false)));

o30.opt_ytitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Pct", true, false)));

o30.printCodes.Add(new OptString("pch", O.ConvertToString(new ScalarString("yes"))));


o30.opt_yminhard = O.ConvertToVal(O.Negate(smpl, i13));

o30.opt_pointsize = O.ConvertToVal(d14);


o30.opt_filename = O.ConvertToString((new ScalarString("Analyze")).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("Graphs"))).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("Income_wealth_base")).Add(smpl, new ScalarString(".")).Add(smpl, (new ScalarString("svg")))));


{
List<int> bankNumbers = null;
O.Prt.Element ope30 = new O.Prt.Element();
ope30.label = O.SubstituteScalarsAndLists("@vDisp['tot']", false);
smpl = new GekkoSmpl(o30.t1.Add(-2), o30.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o30, ope30));
foreach(int bankNumber in bankNumbers) {
ope30.subElements = new List<O.Prt.SubElement>();
ope30.subElements.Add(new O.Prt.SubElement());
ope30.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
), smpl, O.Lookup(smpl, null, "REF", "vDisp", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
);
}
o30.prtElements.Add(ope30);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope30 = new O.Prt.Element();
ope30.label = O.SubstituteScalarsAndLists("@vWealth", false);
smpl = new GekkoSmpl(o30.t1.Add(-2), o30.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o30, ope30));
foreach(int bankNumber in bankNumbers) {
ope30.subElements = new List<O.Prt.SubElement>();
ope30.subElements.Add(new O.Prt.SubElement());
ope30.subElements[0].tsWork = O.Lookup(smpl, null, "REF", "vWealth", null, null, false, EVariableType.Var);
}
o30.prtElements.Add(ope30);
}


o30.counter = 5;
o30.Exe();

p.SetText(@"¤47"); O.InitSmpl(smpl);

O.TimeFilter o31 = new O.TimeFilter();
O.TimeFilterHelper temp16 = new O.TimeFilterHelper();
temp16.from = O.ConvertToDate(i15, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp16);

O.TimeFilterHelper temp18 = new O.TimeFilterHelper();
temp18.from = O.ConvertToDate(i17, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp18);

O.TimeFilterHelper temp20 = new O.TimeFilterHelper();
temp20.from = O.ConvertToDate(i19, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp20);

O.TimeFilterHelper temp22 = new O.TimeFilterHelper();
temp22.from = O.ConvertToDate(i21, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp22);

O.TimeFilterHelper temp24 = new O.TimeFilterHelper();
temp24.from = O.ConvertToDate(i23, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp24);

O.TimeFilterHelper temp26 = new O.TimeFilterHelper();
temp26.from = O.ConvertToDate(i25, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp26);

O.TimeFilterHelper temp28 = new O.TimeFilterHelper();
temp28.from = O.ConvertToDate(i27, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp28);

O.TimeFilterHelper temp30 = new O.TimeFilterHelper();
temp30.from = O.ConvertToDate(i29, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp30);

O.TimeFilterHelper temp32 = new O.TimeFilterHelper();
temp32.from = O.ConvertToDate(i31, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp32);

O.TimeFilterHelper temp34 = new O.TimeFilterHelper();
temp34.from = O.ConvertToDate(i33, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp34);

O.TimeFilterHelper temp36 = new O.TimeFilterHelper();
temp36.from = O.ConvertToDate(i35, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp36);

O.TimeFilterHelper temp38 = new O.TimeFilterHelper();
temp38.from = O.ConvertToDate(i37, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp38);

O.TimeFilterHelper temp40 = new O.TimeFilterHelper();
temp40.from = O.ConvertToDate(i39, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp40);

O.TimeFilterHelper temp42 = new O.TimeFilterHelper();
temp42.from = O.ConvertToDate(i41, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp42);

O.TimeFilterHelper temp44 = new O.TimeFilterHelper();
temp44.from = O.ConvertToDate(i43, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp44);

O.TimeFilterHelper temp46 = new O.TimeFilterHelper();
temp46.from = O.ConvertToDate(i45, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp46);

O.TimeFilterHelper temp48 = new O.TimeFilterHelper();
temp48.from = O.ConvertToDate(i47, O.GetDateChoices.Strict);
o31.timeFilterPeriods.Add(temp48);


o31.Exe();

p.SetText(@"¤48"); O.InitSmpl(smpl);

Program.options.table_html_font = O.ConvertToString((new ScalarString("Arial")));
G.Writeln();
G.Writeln("option table html font = " + O.ConvertToString((new ScalarString("Arial"))) + "");

p.SetText(@"¤50"); O.InitSmpl(smpl);

Program.options.table_html_fontsize = 100;
G.Writeln();
G.Writeln("option table html fontsize = " + 100 + "");

p.SetText(@"¤51"); O.InitSmpl(smpl);

Program.options.table_html_datawidth = 3;
G.Writeln();
G.Writeln("option table html datawidth = " + 3 + "");

p.SetText(@"¤56"); O.InitSmpl(smpl);

O.Pipe o35 = new O.Pipe();
o35.opt_html = "yes";

o35.fileName = O.ConvertToString(O.Add(smpl, O.Lookup(smpl, null, null, "%output", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"Model_representation.html", true, false))));

o35.Exe();

p.SetText(@"¤57"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"<div style=""-webkit-print-color-adjust:exact;"">", true, false))), false);
p.SetText(@"¤58"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"<div style=""font-size: 50%"">Man kan skrive noget tekst her</div>", true, false))), false);
p.SetText(@"¤60"); O.InitSmpl(smpl);

O.Pipe o38 = new O.Pipe();
o38.opt_stop = "yes";


o38.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);


//[[splitSTOP]]
IVariable forloop_51 = null;
int counter53 = 0;
for (O.IterateStart(ref forloop_51, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"Supply_Use_base", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Labor_market_base", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Wages_prices_base", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Demographics_base", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Income_wealth_base", true, false)))); O.IterateContinue(forloop_51, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"Supply_Use_base", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Labor_market_base", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Wages_prices_base", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Demographics_base", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Income_wealth_base", true, false))), null, null, ref counter53); O.IterateStep(forloop_51, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"Supply_Use_base", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Labor_market_base", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Wages_prices_base", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Demographics_base", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Income_wealth_base", true, false))), null, counter53))
{;
p.SetText(@"¤0"); O.InitSmpl(smpl);

IVariable ivTmpvar52 = O.IvConvertTo(EVariableType.Var, forloop_51);
O.Lookup(smpl, null, null, "%i", null, ivTmpvar52, true, EVariableType.Var)
;

p.SetText(@"¤65"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(O.FunctionLookup1("pipex")(smpl, p, new ScalarString(ScalarString.SubstituteScalarsInString(@"Model_representation.html", true, false)))), false);
};

//[[splitSTART]]

p.SetText(@"¤70"); O.InitSmpl(smpl);

Program.options.timefilter = false;
G.Writeln();
G.Writeln("option timefilter = " + "no" + "");

p.SetText(@"¤71"); O.InitSmpl(smpl);

O.Prt o43 = new O.Prt();
o43.prtType = "plot";

o43.t1 = Globals.globalPeriodStart;
o43.t2 = Globals.globalPeriodEnd;

o43.opt_title = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Noegletal", true, false)));

o43.opt_subtitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Stoed", true, false)));

o43.opt_ytitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Pct", true, false)));

o43.printCodes.Add(new OptString("q", O.ConvertToString(new ScalarString("yes"))));


o43.opt_pointsize = O.ConvertToVal(d54);


o43.opt_filename = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Analyze\Graphs\Supply_Use_shock.svg", true, false)));


{
List<int> bankNumbers = null;
O.Prt.Element ope43 = new O.Prt.Element();
ope43.label = O.SubstituteScalarsAndLists("qfGDP", false);
smpl = new GekkoSmpl(o43.t1.Add(-2), o43.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o43, ope43));
foreach(int bankNumber in bankNumbers) {
ope43.subElements = new List<O.Prt.SubElement>();
ope43.subElements.Add(new O.Prt.SubElement());
ope43.subElements[0].tsWork = O.Lookup(smpl, null, null, "qfGDP", null, null, false, EVariableType.Var);
}
o43.prtElements.Add(ope43);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope43 = new O.Prt.Element();
ope43.label = O.SubstituteScalarsAndLists("qfC", false);
smpl = new GekkoSmpl(o43.t1.Add(-2), o43.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o43, ope43));
foreach(int bankNumber in bankNumbers) {
ope43.subElements = new List<O.Prt.SubElement>();
ope43.subElements.Add(new O.Prt.SubElement());
ope43.subElements[0].tsWork = O.Lookup(smpl, null, null, "qfC", null, null, false, EVariableType.Var);
}
o43.prtElements.Add(ope43);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope43 = new O.Prt.Element();
ope43.label = O.SubstituteScalarsAndLists("qfG", false);
smpl = new GekkoSmpl(o43.t1.Add(-2), o43.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o43, ope43));
foreach(int bankNumber in bankNumbers) {
ope43.subElements = new List<O.Prt.SubElement>();
ope43.subElements.Add(new O.Prt.SubElement());
ope43.subElements[0].tsWork = O.Lookup(smpl, null, null, "qfG", null, null, false, EVariableType.Var);
}
o43.prtElements.Add(ope43);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope43 = new O.Prt.Element();
ope43.label = O.SubstituteScalarsAndLists("qfI", false);
smpl = new GekkoSmpl(o43.t1.Add(-2), o43.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o43, ope43));
foreach(int bankNumber in bankNumbers) {
ope43.subElements = new List<O.Prt.SubElement>();
ope43.subElements.Add(new O.Prt.SubElement());
ope43.subElements[0].tsWork = O.Lookup(smpl, null, null, "qfI", null, null, false, EVariableType.Var);
}
o43.prtElements.Add(ope43);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope43 = new O.Prt.Element();
ope43.label = O.SubstituteScalarsAndLists("qfM", false);
smpl = new GekkoSmpl(o43.t1.Add(-2), o43.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o43, ope43));
foreach(int bankNumber in bankNumbers) {
ope43.subElements = new List<O.Prt.SubElement>();
ope43.subElements.Add(new O.Prt.SubElement());
ope43.subElements[0].tsWork = O.Lookup(smpl, null, null, "qfM", null, null, false, EVariableType.Var);
}
o43.prtElements.Add(ope43);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope43 = new O.Prt.Element();
ope43.label = O.SubstituteScalarsAndLists("qfX", false);
smpl = new GekkoSmpl(o43.t1.Add(-2), o43.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o43, ope43));
foreach(int bankNumber in bankNumbers) {
ope43.subElements = new List<O.Prt.SubElement>();
ope43.subElements.Add(new O.Prt.SubElement());
ope43.subElements[0].tsWork = O.Lookup(smpl, null, null, "qfX", null, null, false, EVariableType.Var);
}
o43.prtElements.Add(ope43);
}


o43.counter = 6;
o43.Exe();

p.SetText(@"¤72"); O.InitSmpl(smpl);

O.Prt o44 = new O.Prt();
o44.prtType = "plot";

o44.t1 = Globals.globalPeriodStart;
o44.t2 = Globals.globalPeriodEnd;

o44.opt_title = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Arbejdsmarkedet", true, false)));

o44.opt_subtitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Stoed", true, false)));

o44.opt_ytitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"1000 personer", true, false)));

o44.opt_pointsize = O.ConvertToVal(d55);

o44.printCodes.Add(new OptString("m", O.ConvertToString(new ScalarString("yes"))));



o44.opt_filename = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Analyze\Graphs\Labor_market_shock.svg", true, false)));


{
List<int> bankNumbers = null;
O.Prt.Element ope44 = new O.Prt.Element();
ope44.label = O.SubstituteScalarsAndLists("nEmployed['tot']", false);
smpl = new GekkoSmpl(o44.t1.Add(-2), o44.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o44, ope44));
foreach(int bankNumber in bankNumbers) {
ope44.subElements = new List<O.Prt.SubElement>();
ope44.subElements.Add(new O.Prt.SubElement());
ope44.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
), smpl, O.Lookup(smpl, null, null, "nEmployed", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
);
}
o44.prtElements.Add(ope44);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope44 = new O.Prt.Element();
ope44.label = O.SubstituteScalarsAndLists("nunemployed['tot']", false);
smpl = new GekkoSmpl(o44.t1.Add(-2), o44.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o44, ope44));
foreach(int bankNumber in bankNumbers) {
ope44.subElements = new List<O.Prt.SubElement>();
ope44.subElements.Add(new O.Prt.SubElement());
ope44.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
), smpl, O.Lookup(smpl, null, null, "nunemployed", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
);
}
o44.prtElements.Add(ope44);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope44 = new O.Prt.Element();
ope44.label = O.SubstituteScalarsAndLists("nLaborForce['tot']", false);
smpl = new GekkoSmpl(o44.t1.Add(-2), o44.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o44, ope44));
foreach(int bankNumber in bankNumbers) {
ope44.subElements = new List<O.Prt.SubElement>();
ope44.subElements.Add(new O.Prt.SubElement());
ope44.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
), smpl, O.Lookup(smpl, null, null, "nLaborForce", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
);
}
o44.prtElements.Add(ope44);
}


o44.counter = 7;
o44.Exe();

p.SetText(@"¤73"); O.InitSmpl(smpl);

O.Prt o45 = new O.Prt();
o45.prtType = "plot";

o45.t1 = Globals.globalPeriodStart;
o45.t2 = Globals.globalPeriodEnd;

o45.opt_title = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Loen og priser", true, false)));

o45.opt_subtitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Stoed", true, false)));

o45.opt_ytitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Pct", true, false)));

o45.opt_pointsize = O.ConvertToVal(d56);

o45.printCodes.Add(new OptString("q", O.ConvertToString(new ScalarString("yes"))));



o45.opt_filename = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Analyze\Graphs\Wages_prices_shock.svg", true, false)));


{
List<int> bankNumbers = null;
O.Prt.Element ope45 = new O.Prt.Element();
ope45.label = O.SubstituteScalarsAndLists("pL", false);
smpl = new GekkoSmpl(o45.t1.Add(-2), o45.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o45, ope45));
foreach(int bankNumber in bankNumbers) {
ope45.subElements = new List<O.Prt.SubElement>();
ope45.subElements.Add(new O.Prt.SubElement());
ope45.subElements[0].tsWork = O.Lookup(smpl, null, null, "pL", null, null, false, EVariableType.Var);
}
o45.prtElements.Add(ope45);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope45 = new O.Prt.Element();
ope45.label = O.SubstituteScalarsAndLists("pC['tot']", false);
smpl = new GekkoSmpl(o45.t1.Add(-2), o45.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o45, ope45));
foreach(int bankNumber in bankNumbers) {
ope45.subElements = new List<O.Prt.SubElement>();
ope45.subElements.Add(new O.Prt.SubElement());
ope45.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
), smpl, O.Lookup(smpl, null, null, "pC", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
);
}
o45.prtElements.Add(ope45);
}


o45.counter = 8;
o45.Exe();

p.SetText(@"¤74"); O.InitSmpl(smpl);

O.Prt o46 = new O.Prt();
o46.prtType = "plot";

o46.t1 = Globals.globalPeriodStart;
o46.t2 = Globals.globalPeriodEnd;

o46.opt_title = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Indkomst og velstand", true, false)));

o46.opt_subtitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Stoed", true, false)));

o46.opt_ytitle = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Pct", true, false)));

o46.printCodes.Add(new OptString("q", O.ConvertToString(new ScalarString("yes"))));


o46.opt_pointsize = O.ConvertToVal(d57);


o46.opt_filename = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"Analyze\Graphs\Income_wealth_shock.svg", true, false)));


{
List<int> bankNumbers = null;
O.Prt.Element ope46 = new O.Prt.Element();
ope46.label = O.SubstituteScalarsAndLists("vDisp['tot']", false);
smpl = new GekkoSmpl(o46.t1.Add(-2), o46.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o46, ope46));
foreach(int bankNumber in bankNumbers) {
ope46.subElements = new List<O.Prt.SubElement>();
ope46.subElements.Add(new O.Prt.SubElement());
ope46.subElements[0].tsWork = O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
), smpl, O.Lookup(smpl, null, null, "vDisp", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"tot", true, false))
);
}
o46.prtElements.Add(ope46);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope46 = new O.Prt.Element();
ope46.label = O.SubstituteScalarsAndLists("vWealth", false);
smpl = new GekkoSmpl(o46.t1.Add(-2), o46.t2);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o46, ope46));
foreach(int bankNumber in bankNumbers) {
ope46.subElements = new List<O.Prt.SubElement>();
ope46.subElements.Add(new O.Prt.SubElement());
ope46.subElements[0].tsWork = O.Lookup(smpl, null, null, "vWealth", null, null, false, EVariableType.Var);
}
o46.prtElements.Add(ope46);
}


o46.counter = 9;
o46.Exe();

p.SetText(@"¤78"); O.InitSmpl(smpl);

O.TimeFilter o47 = new O.TimeFilter();
O.TimeFilterHelper temp59 = new O.TimeFilterHelper();
temp59.from = O.ConvertToDate(i58, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp59);

O.TimeFilterHelper temp61 = new O.TimeFilterHelper();
temp61.from = O.ConvertToDate(i60, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp61);

O.TimeFilterHelper temp63 = new O.TimeFilterHelper();
temp63.from = O.ConvertToDate(i62, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp63);

O.TimeFilterHelper temp65 = new O.TimeFilterHelper();
temp65.from = O.ConvertToDate(i64, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp65);

O.TimeFilterHelper temp67 = new O.TimeFilterHelper();
temp67.from = O.ConvertToDate(i66, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp67);

O.TimeFilterHelper temp69 = new O.TimeFilterHelper();
temp69.from = O.ConvertToDate(i68, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp69);

O.TimeFilterHelper temp71 = new O.TimeFilterHelper();
temp71.from = O.ConvertToDate(i70, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp71);

O.TimeFilterHelper temp73 = new O.TimeFilterHelper();
temp73.from = O.ConvertToDate(i72, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp73);

O.TimeFilterHelper temp75 = new O.TimeFilterHelper();
temp75.from = O.ConvertToDate(i74, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp75);

O.TimeFilterHelper temp77 = new O.TimeFilterHelper();
temp77.from = O.ConvertToDate(i76, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp77);

O.TimeFilterHelper temp79 = new O.TimeFilterHelper();
temp79.from = O.ConvertToDate(i78, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp79);

O.TimeFilterHelper temp81 = new O.TimeFilterHelper();
temp81.from = O.ConvertToDate(i80, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp81);

O.TimeFilterHelper temp83 = new O.TimeFilterHelper();
temp83.from = O.ConvertToDate(i82, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp83);

O.TimeFilterHelper temp85 = new O.TimeFilterHelper();
temp85.from = O.ConvertToDate(i84, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp85);

O.TimeFilterHelper temp87 = new O.TimeFilterHelper();
temp87.from = O.ConvertToDate(i86, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp87);

O.TimeFilterHelper temp89 = new O.TimeFilterHelper();
temp89.from = O.ConvertToDate(i88, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp89);

O.TimeFilterHelper temp91 = new O.TimeFilterHelper();
temp91.from = O.ConvertToDate(i90, O.GetDateChoices.Strict);
o47.timeFilterPeriods.Add(temp91);


o47.Exe();

p.SetText(@"¤79"); O.InitSmpl(smpl);

O.Pipe o48 = new O.Pipe();
o48.opt_html = "yes";

o48.opt_append = "yes";

o48.fileName = O.ConvertToString(O.Add(smpl, O.Lookup(smpl, null, null, "%output", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"Model_representation.html", true, false))));

o48.Exe();

p.SetText(@"¤80"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"<div style=""-webkit-print-color-adjust:exact;"">", true, false))), false);
p.SetText(@"¤81"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"<div style=""font-size: 50%"">Man kan skrive noget tekst her</div>", true, false))), false);
p.SetText(@"¤83"); O.InitSmpl(smpl);

O.Pipe o51 = new O.Pipe();
o51.fileName = O.ConvertToString((new ScalarString("con")));

o51.Exe();

p.SetText(@"¤0"); O.InitSmpl(smpl);


//[[splitSTOP]]
IVariable forloop_92 = null;
int counter94 = 0;
for (O.IterateStart(ref forloop_92, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"Supply_Use_shock", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Labor_market_shock", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Wages_prices_shock", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Income_wealth_shock", true, false)))); O.IterateContinue(forloop_92, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"Supply_Use_shock", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Labor_market_shock", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Wages_prices_shock", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Income_wealth_shock", true, false))), null, null, ref counter94); O.IterateStep(forloop_92, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"Supply_Use_shock", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Labor_market_shock", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Wages_prices_shock", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"Income_wealth_shock", true, false))), null, counter94))
{;
p.SetText(@"¤0"); O.InitSmpl(smpl);

IVariable ivTmpvar93 = O.IvConvertTo(EVariableType.Var, forloop_92);
O.Lookup(smpl, null, null, "%i", null, ivTmpvar93, true, EVariableType.Var)
;

p.SetText(@"¤87"); O.InitSmpl(smpl);

Program.Tell(O.ConvertToString(O.FunctionLookup1("pipex")(smpl, p, new ScalarString(ScalarString.SubstituteScalarsInString(@"Model_representation.html", true, false)))), false);
};

//[[splitSTART]]

p.SetText(@"¤89"); O.InitSmpl(smpl);

Program.options.timefilter = false;
G.Writeln();
G.Writeln("option timefilter = " + "no" + "");

p.SetText(@"¤91"); O.InitSmpl(smpl);

O.Sys o56 = new O.Sys();
o56.s = O.Add(smpl, O.Lookup(smpl, null, null, "%output", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"Model_representation.html", true, false)));
o56.Exe();

p.SetText(@"¤93"); O.InitSmpl(smpl);

Program.options.timefilter = false;
G.Writeln();
G.Writeln("option timefilter = " + "no" + "");

p.SetText(@"¤95"); O.InitSmpl(smpl);

O.Run o58 = new O.Run();
o58.fileName = O.ConvertToString((new ScalarString("ces")));
o58.p = p;
o58.Exe();


//[[splitSTOP]]


}
}
}
