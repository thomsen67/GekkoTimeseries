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
public static GekkoTime globalGekkoTimeIterator = Globals.tNull;
public static readonly ScalarVal i1 = new ScalarVal(2000d);
public static readonly ScalarVal i2 = new ScalarVal(2000d);
public static readonly ScalarVal i3 = new ScalarVal(5d);
public static readonly ScalarVal i4 = new ScalarVal(6d);
public static readonly ScalarVal i5 = new ScalarVal(2001d);
public static readonly ScalarVal i6 = new ScalarVal(2001d);
public static readonly ScalarVal i7 = new ScalarVal(15d);
public static readonly ScalarVal i8 = new ScalarVal(3d);
public static readonly ScalarVal i9 = new ScalarVal(2000d);
public static readonly ScalarVal i10 = new ScalarVal(2001d);
public static IVariable list12 = null;
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoTime t = Globals.tNull;


p.SetText(@"¤1");
O.Reset o0 = new O.Reset();
o0.p = p;o0.Exe();




p.SetText(@"¤1");
O.Mode o1 = new O.Mode();
o1.mode = @"data";o1.Exe();




p.SetText(@"¤2");
O.Time o2 = new O.Time();
o2.t1 = O.GetDate(i1, O.GetDateChoices.FlexibleStart);
;
o2.t2 = O.GetDate(i2, O.GetDateChoices.FlexibleEnd);
;

o2.Exe();




p.SetText(@"¤3");
O.Series o3 = new O.Series();

o3.lhs = null;
o3.p = p;
foreach (GekkoTime t2 in new GekkoTimeIterator(o3.t1, o3.t2))
{
  t = t2; 
  double data = O.GetVal(i3, t);
if(o3.lhs == null) o3.lhs = O.GetTimeSeries(O.Indexer(t, O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x"))), 1, O.ECreatePossibilities.Can), true, new ScalarString(@"a"))
);
o3.lhs.SetData(t, data);
}
t = Globals.tNull; 
o3.Exe();




p.SetText(@"¤4");
O.Series o4 = new O.Series();

o4.lhs = null;
o4.p = p;
foreach (GekkoTime t2 in new GekkoTimeIterator(o4.t1, o4.t2))
{
  t = t2; 
  double data = O.GetVal(i4, t);
if(o4.lhs == null) o4.lhs = O.GetTimeSeries(O.Indexer(t, O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x"))), 1, O.ECreatePossibilities.Can), true, new ScalarString(@"b"))
);
o4.lhs.SetData(t, data);
}
t = Globals.tNull; 
o4.Exe();




p.SetText(@"¤5");
O.Time o5 = new O.Time();
o5.t1 = O.GetDate(i5, O.GetDateChoices.FlexibleStart);
;
o5.t2 = O.GetDate(i6, O.GetDateChoices.FlexibleEnd);
;

o5.Exe();




p.SetText(@"¤6");
O.Series o6 = new O.Series();

o6.lhs = null;
o6.p = p;
foreach (GekkoTime t2 in new GekkoTimeIterator(o6.t1, o6.t2))
{
  t = t2; 
  double data = O.GetVal(i7, t);
if(o6.lhs == null) o6.lhs = O.GetTimeSeries(O.Indexer(t, O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x"))), 1, O.ECreatePossibilities.Can), true, new ScalarString(@"a"))
);
o6.lhs.SetData(t, data);
}
t = Globals.tNull; 
o6.Exe();




p.SetText(@"¤7");
O.Series o7 = new O.Series();

o7.lhs = null;
o7.p = p;
foreach (GekkoTime t2 in new GekkoTimeIterator(o7.t1, o7.t2))
{
  t = t2; 
  double data = O.GetVal(i8, t);
if(o7.lhs == null) o7.lhs = O.GetTimeSeries(O.Indexer(t, O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x"))), 1, O.ECreatePossibilities.Can), true, new ScalarString(@"b"))
);
o7.lhs.SetData(t, data);
}
t = Globals.tNull; 
o7.Exe();




p.SetText(@"¤8");
O.Time o8 = new O.Time();
o8.t1 = O.GetDate(i9, O.GetDateChoices.FlexibleStart);
;
o8.t2 = O.GetDate(i10, O.GetDateChoices.FlexibleEnd);
;

o8.Exe();




}

public static void C1(P p) {

GekkoTime t = Globals.tNull;

p.SetText(@"¤9");
O.List o9 = new O.List();
o9.name = O.GetString((new ScalarString("i")));
o9.listItems = new List<string>();
o9.p = p;
o9.listItems = new List<string>();
o9.listItems.AddRange(O.GetList((new ScalarString("a"))));

o9.listItems.AddRange(O.GetList((new ScalarString("b"))));

o9.Exe();




p.SetText(@"¤10");
O.Genr o10 = new O.Genr();
IVariable ts11 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("y"))), 1, O.ECreatePossibilities.Can);
IVariable ts13 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x"))), 1);
o10.t1 = Globals.globalPeriodStart;
o10.t2 = Globals.globalPeriodEnd;

o10.lhs = null;
o10.p = p;
foreach (GekkoTime t2 in new GekkoTimeIterator(o10.t1, o10.t2))
{
  t = t2; 
double[] storage16 = new double[2];
int counter17 = 0;
GekkoTime t3 = t2;

foreach (string s1177 in new List<string> { "a", "b" })
{
t = t3;
double[] storage14 = new double[0 - (-1) + 1];
int counter15 = 0;
foreach (GekkoTime t4 in new GekkoTimeIterator(t3.Add(-1), t3.Add(0)))
{
t = t4;
storage14[counter15] = O.GetVal(O.Indexer(t, ts13, false, new ScalarString(s1177)), t);
counter15++;
t = t3;
}
storage16[counter17] = O.GetVal(O.HandleSummations("dif", storage14), t);
counter17++;
}
  double data = O.GetVal(O.HandleSummations("sum", storage16), t);
if(o10.lhs == null) o10.lhs = O.GetTimeSeries(ts11);
o10.lhs.SetData(t, data);
}
t = Globals.tNull; 
o10.meta = @"ser y = sum(#i, dif(x[#i]))";
o10.Exe();





p.SetText(@"¤12");
O.Prt o11 = new O.Prt();
o11.prtType = "p";

o11.t1 = Globals.globalPeriodStart;
o11.t2 = Globals.globalPeriodEnd;

o11.printCodes.Add(new OptString("n", O.GetString(new ScalarString("yes"))));



{
List<int> bankNumbers = null;
O.Prt.Element ope11 = new O.Prt.Element();
ope11.label = O.SubstituteScalarsAndLists("x['a']", false);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o11, ope11));
foreach(int bankNumber in bankNumbers) {
IVariable ts18 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x"))), bankNumber);
foreach (GekkoTime t2 in new GekkoTimeIterator(o11.t1.Add(-2), o11.t2))
{
  t = t2; 
O.GetVal777(O.Indexer(t, ts18, false, new ScalarString(@"a")), bankNumber, ope11, t);
}
t = Globals.tNull; 
}
o11.prtElements.Add(ope11);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope11 = new O.Prt.Element();
ope11.label = O.SubstituteScalarsAndLists("x['b']", false);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o11, ope11));
foreach(int bankNumber in bankNumbers) {
IVariable ts19 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x"))), bankNumber);
foreach (GekkoTime t2 in new GekkoTimeIterator(o11.t1.Add(-2), o11.t2))
{
  t = t2; 
O.GetVal777(O.Indexer(t, ts19, false, new ScalarString(@"b")), bankNumber, ope11, t);
}
t = Globals.tNull; 
}
o11.prtElements.Add(ope11);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope11 = new O.Prt.Element();
ope11.label = O.SubstituteScalarsAndLists("x['a']+x['b']", false);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o11, ope11));
foreach(int bankNumber in bankNumbers) {
IVariable ts20 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x"))), bankNumber);
foreach (GekkoTime t2 in new GekkoTimeIterator(o11.t1.Add(-2), o11.t2))
{
  t = t2; 
O.GetVal777(O.Add(O.Indexer(t, ts20, false, new ScalarString(@"a")), O.Indexer(t, ts20, false, new ScalarString(@"b")), t), bankNumber, ope11, t);
}
t = Globals.tNull; 
}
o11.prtElements.Add(ope11);
}

{
List<int> bankNumbers = null;
O.Prt.Element ope11 = new O.Prt.Element();
ope11.label = O.SubstituteScalarsAndLists("y", false);
bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o11, ope11));
foreach(int bankNumber in bankNumbers) {
IVariable ts21 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("y"))), bankNumber);
foreach (GekkoTime t2 in new GekkoTimeIterator(o11.t1.Add(-2), o11.t2))
{
  t = t2; 
O.GetVal777(ts21, bankNumber, ope11, t);
}
t = Globals.tNull; 
}
o11.prtElements.Add(ope11);
}


o11.counter = 1;
o11.Exe();




}


public static void CodeLines(P p)
{
GekkoTime t = Globals.tNull;

C0(p);

C1(p);



}
}
}
