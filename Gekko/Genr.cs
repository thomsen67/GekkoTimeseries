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
public static readonly ScalarVal i407 = new ScalarVal(2020d);
public static readonly ScalarVal i408 = new ScalarVal(2030d);
public static readonly ScalarVal d411 = new ScalarVal(1.02d);
public static readonly ScalarVal d414 = new ScalarVal(1.02d);
public static readonly ScalarVal i454 = new ScalarVal(2025d);
public static readonly ScalarVal i455 = new ScalarVal(2027d);
public static readonly ScalarVal i456 = new ScalarVal(2025d);
public static readonly ScalarVal i459 = new ScalarVal(1d);
public static readonly ScalarVal i460 = new ScalarVal(2025d);
public static readonly ScalarVal i461 = new ScalarVal(2027d);
public static void C0(P p) {

GekkoTime t = Globals.tNull;


p.SetText(@"¤2");
O.Reset o0 = new O.Reset();
o0.p = p;o0.Exe();




p.SetText(@"¤2");
O.Time o1 = new O.Time();
o1.t1 = O.GetDate(i407, O.GetDateChoices.FlexibleStart);
;
o1.t2 = O.GetDate(i408, O.GetDateChoices.FlexibleEnd);
;

o1.Exe();




p.SetText(@"¤2");
O.Mode o2 = new O.Mode();
o2.mode = @"data";o2.Exe();




p.SetText(@"¤3");
O.Read o3 = new O.Read();
o3.p = p;
o3.type = @"read";
o3.opt_tsd = "yes";

o3.fileName = O.GetString((new ScalarString("jul05")));


o3.gekkocode = @"read <tsd> jul05";
o3.p = p;
o3.Exe();




p.SetText(@"¤5");
O.Genr o4 = new O.Genr();
Globals.traceContainer = new ListUnique<TimeSeries>();
Globals.hack_lhsOrRhs = 0;
IVariable ts409 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x1"))), 1, O.ECreatePossibilities.Can);
IVariable ts410 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("yf"))), 1);
o4.t1 = Globals.globalPeriodStart;
o4.t2 = Globals.globalPeriodEnd;

o4.lhs = null;
o4.p = p;
foreach (GekkoTime t2 in new GekkoTimeIterator(o4.t1, o4.t2))
{
  t = t2; 
  double data = O.GetVal(O.Multiply(ts410, d411, t), t);
if(o4.lhs == null) o4.lhs = O.GetTimeSeries(ts409);
o4.lhs.SetData(t, data);
}
t = Globals.tNull; 
o4.meta = @"ser x1 = yf * 1.02";
o4.Exe();
Globals.traceContainer = new ListUnique<TimeSeries>();
Globals.hack_lhsOrRhs = 0;





p.SetText(@"¤6");
O.Genr o5 = new O.Genr();
Globals.traceContainer = new ListUnique<TimeSeries>();
Globals.hack_lhsOrRhs = 0;
IVariable ts412 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x2"))), 1, O.ECreatePossibilities.Can);
IVariable ts413 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("fyf"))), 1);
o5.t1 = Globals.globalPeriodStart;
o5.t2 = Globals.globalPeriodEnd;

o5.lhs = null;
o5.p = p;
foreach (GekkoTime t2 in new GekkoTimeIterator(o5.t1, o5.t2))
{
  t = t2; 
  double data = O.GetVal(O.Multiply(ts413, d414, t), t);
if(o5.lhs == null) o5.lhs = O.GetTimeSeries(ts412);
o5.lhs.SetData(t, data);
}
t = Globals.tNull; 
o5.meta = @"ser x2 = fyf * 1.02";
o5.Exe();
Globals.traceContainer = new ListUnique<TimeSeries>();
Globals.hack_lhsOrRhs = 0;




p.SetText(@"¤12");



p.SetText(@"¤53");
O.Genr o20 = new O.Genr();
Globals.traceContainer = new ListUnique<TimeSeries>();
Globals.hack_lhsOrRhs = 0;
IVariable ts451 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("xx1"))), 1, O.ECreatePossibilities.Can);
IVariable ts452 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x1"))), 1);
IVariable ts453 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("x2"))), 1);
o20.t1 = Globals.globalPeriodStart;
o20.t2 = Globals.globalPeriodEnd;

o20.lhs = null;
o20.p = p;
foreach (GekkoTime t2 in new GekkoTimeIterator(o20.t1, o20.t2))
{
  t = t2; 
  //double data = O.GetVal(UProc.kaedepris2(p, t, ts452, ts453, i454, i455, i456), t);
if(o20.lhs == null) o20.lhs = O.GetTimeSeries(ts451);
//o20.lhs.SetData(t, data);
}
t = Globals.tNull; 
o20.meta = @"ser xx1 = kaedepris2(x1, x2, 2025, 2027, 2025)";
o20.Exe();
Globals.traceContainer = new ListUnique<TimeSeries>();
Globals.hack_lhsOrRhs = 0;





p.SetText(@"¤54");
O.Genr o21 = new O.Genr();
Globals.traceContainer = new ListUnique<TimeSeries>();
Globals.hack_lhsOrRhs = 0;
IVariable ts457 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("xx2"))), 1, O.ECreatePossibilities.Can);
IVariable ts458 = O.GetTimeSeries(O.GetString(new ScalarString("[FIRST]")) + ":" + O.GetString((new ScalarString("xx1"))), 1);
o21.t1 = Globals.globalPeriodStart;
o21.t2 = Globals.globalPeriodEnd;

o21.lhs = null;
o21.p = p;
foreach (GekkoTime t2 in new GekkoTimeIterator(o21.t1, o21.t2))
{
  t = t2; 
  double data = O.GetVal(O.Add(ts458, i459, t), t);
if(o21.lhs == null) o21.lhs = O.GetTimeSeries(ts457);
o21.lhs.SetData(t, data);
}
t = Globals.tNull; 
o21.meta = @"ser xx2 = xx1 + 1";
o21.Exe();
Globals.traceContainer = new ListUnique<TimeSeries>();
Globals.hack_lhsOrRhs = 0;





p.SetText(@"¤55");
O.Time o22 = new O.Time();
o22.t1 = O.GetDate(i460, O.GetDateChoices.FlexibleStart);
;
o22.t2 = O.GetDate(i461, O.GetDateChoices.FlexibleEnd);
;

o22.Exe();




}

public static void C1(P p) {

GekkoTime t = Globals.tNull;

p.SetText(@"¤56");
O.Trace o23 = new O.Trace();
o23.listItems = new List<string>();
o23.listItems.AddRange(O.GetList((new ScalarString("xx2"))));


o23.Exe();




}


public static void CodeLines(P p)
{
GekkoTime t = Globals.tNull;

C0(p);

C1(p);



}
}
}
