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

        public static void CC0(GekkoSmpl smpl, P p, ref IVariable xfunctionarg_xf7dke8cj_290, ref IVariable xfunctionarg_xf7dke8cj_291)
        {
            IVariable functionarg_xf7dke8cj_290 = xfunctionarg_xf7dke8cj_290;

            IVariable functionarg_xf7dke8cj_291 = xfunctionarg_xf7dke8cj_291;

            //[[commandStart]]1
            p.SetText(@"¤3"); O.InitSmpl(smpl, p);


            Func<GraphHelper, string> print1 = (gh) =>
            {
                O.Prt o1 = new O.Prt();
                labelCounter = 0; o1.guiGraphIsRefreshing = gh.isRefreshing;
                o1.guiGraphPrintCode = gh.printCode;
                o1.guiGraphIsLogTransform = gh.isLogTransform;
                o1.prtType = "p";
                ESeriesMissing r1_1 = Program.options.series_array_print_missing; ESeriesMissing r2_1 = Program.options.series_normal_print_missing; try
                {
                    O.HandleOptionBankRef1(o1.opt_bank, o1.opt_ref); O.HandleMissing1(o1.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope1 = new O.Prt.Element();
                        ope1.labelGiven = new List<string>() { "abc|[@70,146:148='abc',<1255>,3:2]|[@70,146:148='abc',<1255>,3:2]" };
                        smpl = new GekkoSmpl(o1.t1, o1.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope1.printCodesFinal = Program.GetElementPrintCodes(o1, ope1); bankNumbers = O.Prt.GetBankNumbers(null, ope1.printCodesFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope1.variable[bankNumber] = O.Lookup(smpl, null, null, "abc", null, null, new LookupSettings(), EVariableType.Var, null);
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope1);
                        }
                        smpl.bankNumber = 0;
                        o1.prtElements.Add(ope1);
                    }

                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_1, r2_1);
                }
                o1.counter = 2;
                o1.printCsCounter = Globals.printCs.Count - 1;
                o1.Exe();
                return o1.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print1);
            print1(new GraphHelper());

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetText(@"¤18"); O.InitSmpl(smpl, p);


            O.Close o2 = new O.Close();
            o2.listItems = O.ExplodeIvariablesSeq(new List(new List<IVariable> { new ScalarString("*") }));
            o2.Exe();

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤18"); O.InitSmpl(smpl, p);


            O.Clear o3 = new O.Clear();
            o3.p = p; o3.Exe();

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤20"); O.InitSmpl(smpl, p);


            Databank local4 = Program.databanks.local;
            Program.databanks.local = new Databank("Local"); LocalGlobal lg4 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.Deeper();
            try
            {
                O.Run o4 = new O.Run();
                o4.fileName = O.ConvertToString((new ScalarString("aarbank")));
                o4.p = p;
                o4.Exe();
            }
            finally
            {
                Program.databanks.local = local4; Program.databanks.localGlobal = lg4; p.RemoveLast();
            }

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤20"); O.InitSmpl(smpl, p);


            Databank local5 = Program.databanks.local;
            Program.databanks.local = new Databank("Local"); LocalGlobal lg5 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.Deeper();
            try
            {
                O.Run o5 = new O.Run();
                o5.fileName = O.ConvertToString((new ScalarString("kvtbank")));
                o5.p = p;
                o5.Exe();
            }
            finally
            {
                Program.databanks.local = local5; Program.databanks.localGlobal = lg5; p.RemoveLast();
            }

            //[[commandEnd]]5


            //[[commandStart]]6
            p.SetText(@"¤22"); O.InitSmpl(smpl, p);


            Program.options.freq = G.GetFreq(O.ConvertToString((new ScalarString("q"))));
            G.Writeln();
            G.Writeln("option freq = " + (G.GetFreq(O.ConvertToString((new ScalarString("q"))))).ToString().ToLower() + "");
            Program.AdjustFreq();
            //[[commandEnd]]6


            //[[commandStart]]7
            p.SetText(@"¤23"); O.InitSmpl(smpl, p);


            O.Time o7 = new O.Time();
            o7.t1 = O.ConvertToDate(i292, O.GetDateChoices.FlexibleStart);
            ;
            o7.t2 = O.ConvertToDate(Functions.date(smpl, O.HandleString(new ScalarString(@"")).Add(smpl, functionarg_xf7dke8cj_290).Add(smpl, O.HandleString(new ScalarString(@"q"))).Add(smpl, functionarg_xf7dke8cj_291).Add(smpl, O.HandleString(new ScalarString(@"")))), O.GetDateChoices.FlexibleEnd);
            ;

            o7.Exe();

            //[[commandEnd]]7
            xfunctionarg_xf7dke8cj_290 = functionarg_xf7dke8cj_290;

            xfunctionarg_xf7dke8cj_291 = functionarg_xf7dke8cj_291;

        }
        public static void CC1(GekkoSmpl smpl, P p, ref IVariable xfunctionarg_xf7dke8cj_290)
        {
            IVariable functionarg_xf7dke8cj_290 = xfunctionarg_xf7dke8cj_290;

            //[[commandStart]]9
            p.SetText(@"¤26"); O.InitSmpl(smpl, p);


            O.Assignment o9 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar294 = O.Subtract(smpl, functionarg_xf7dke8cj_290, i295);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%slarhel", null, ivTmpvar294, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]9
            xfunctionarg_xf7dke8cj_290 = functionarg_xf7dke8cj_290;

        }
        public static void CC2(GekkoSmpl smpl, P p, ref IVariable xfunctionarg_xf7dke8cj_290)
        {
            IVariable functionarg_xf7dke8cj_290 = xfunctionarg_xf7dke8cj_290;

            //[[commandStart]]10
            p.SetText(@"¤28"); O.InitSmpl(smpl, p);


            O.Assignment o10 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar296 = functionarg_xf7dke8cj_290;
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%slarhel", null, ivTmpvar296, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]10
            xfunctionarg_xf7dke8cj_290 = functionarg_xf7dke8cj_290;

        }
        public static void CC3(GekkoSmpl smpl, P p, ref IVariable xfunctionarg_xf7dke8cj_285, ref IVariable xfunctionarg_xf7dke8cj_288)
        {
            IVariable functionarg_xf7dke8cj_285 = xfunctionarg_xf7dke8cj_285;

            IVariable functionarg_xf7dke8cj_288 = xfunctionarg_xf7dke8cj_288;

            //[[commandStart]]11
            p.SetText(@"¤31"); O.InitSmpl(smpl, p);


            O.Assignment o11 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar297 = O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("dump_b1")))), null, new LookupSettings(), EVariableType.Var, null);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "#b1", null, ivTmpvar297, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]11


            //[[commandStart]]12
            p.SetText(@"¤32"); O.InitSmpl(smpl, p);


            O.Assignment o12 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar298 = O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("dump_b2")))), null, new LookupSettings(), EVariableType.Var, null);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "#b2", null, ivTmpvar298, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]12


            //[[commandStart]]13
            p.SetText(@"¤33"); O.InitSmpl(smpl, p);


            O.Assignment o13 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar299 = O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("dump_b3")))), null, new LookupSettings(), EVariableType.Var, null);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "#b3", null, ivTmpvar299, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]13


            //[[commandStart]]14
            p.SetText(@"¤34"); O.InitSmpl(smpl, p);


            O.Assignment o14 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar300 = O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("dump_b70")))), null, new LookupSettings(), EVariableType.Var, null);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "#b70", null, ivTmpvar300, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]14


            //[[commandStart]]15
            p.SetText(@"¤35"); O.InitSmpl(smpl, p);


            O.Assignment o15 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar301 = O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("dump_b74")))), null, new LookupSettings(), EVariableType.Var, null);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "#b74", null, ivTmpvar301, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]15


            //[[commandStart]]16
            p.SetText(@"¤37"); O.InitSmpl(smpl, p);


            O.Open o16 = new O.Open();
            o16.openFileNames = O.ExplodeIvariables(new List(new List<IVariable> { (functionarg_xf7dke8cj_285) }));
            o16.openFileNamesAs = null;

            o16.Exe();

            //[[commandEnd]]16


            //[[commandStart]]17
            p.SetText(@"¤38"); O.InitSmpl(smpl, p);


            O.Assignment o17 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar302 = Functions.replaceinside(smpl, O.Lookup(smpl, null, null, "#B1", null, null, new LookupSettings(), EVariableType.List, null), functionarg_xf7dke8cj_288, O.HandleString(new ScalarString(@"")), i303);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("alle1")))), ivTmpvar302, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.List, null)
            ;

            //[[commandEnd]]17


            //[[commandStart]]18
            p.SetText(@"¤39"); O.InitSmpl(smpl, p);


            O.Assignment o18 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar304 = Functions.replaceinside(smpl, O.Lookup(smpl, null, null, "#B2", null, null, new LookupSettings(), EVariableType.List, null), O.HandleString(new ScalarString(@"")).Add(smpl, functionarg_xf7dke8cj_288).Add(smpl, O.HandleString(new ScalarString(@"52"))), O.HandleString(new ScalarString(@"")), i305);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("alle2")))), ivTmpvar304, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.List, null)
            ;

            //[[commandEnd]]18


            //[[commandStart]]19
            p.SetText(@"¤40"); O.InitSmpl(smpl, p);


            O.Assignment o19 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar306 = Functions.replaceinside(smpl, O.Lookup(smpl, null, null, "#B3", null, null, new LookupSettings(), EVariableType.List, null), O.HandleString(new ScalarString(@"")).Add(smpl, functionarg_xf7dke8cj_288).Add(smpl, O.HandleString(new ScalarString(@"5223"))), O.HandleString(new ScalarString(@"")), i307);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("alle3")))), ivTmpvar306, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.List, null)
            ;

            //[[commandEnd]]19


            //[[commandStart]]20
            p.SetText(@"¤41"); O.InitSmpl(smpl, p);


            O.Assignment o20 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar308 = Functions.replaceinside(smpl, O.Lookup(smpl, null, null, "#B70", null, null, new LookupSettings(), EVariableType.List, null), O.HandleString(new ScalarString(@"")).Add(smpl, functionarg_xf7dke8cj_288).Add(smpl, O.HandleString(new ScalarString(@"70"))), O.HandleString(new ScalarString(@"")), i309);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("alle70")))), ivTmpvar308, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.List, null)
            ;

            //[[commandEnd]]20


            //[[commandStart]]21
            p.SetText(@"¤42"); O.InitSmpl(smpl, p);


            O.Assignment o21 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar310 = Functions.replaceinside(smpl, O.Lookup(smpl, null, null, "#B74", null, null, new LookupSettings(), EVariableType.List, null), O.HandleString(new ScalarString(@"")).Add(smpl, functionarg_xf7dke8cj_288).Add(smpl, O.HandleString(new ScalarString(@"74"))), O.HandleString(new ScalarString(@"")), i311);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("alle74")))), ivTmpvar310, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.List, null)
            ;

            //[[commandEnd]]21


            //[[commandStart]]22
            p.SetText(@"¤43"); O.InitSmpl(smpl, p);


            O.Close o22 = new O.Close();
            o22.listItems = O.ExplodeIvariablesSeq(new List(new List<IVariable> { (functionarg_xf7dke8cj_285) }));
            o22.Exe();

            //[[commandEnd]]22


            //[[commandStart]]23
            p.SetText(@"¤43"); O.InitSmpl(smpl, p);


            O.Clear o23 = new O.Clear();
            o23.p = p; o23.Exe();

            //[[commandEnd]]23
            xfunctionarg_xf7dke8cj_285 = functionarg_xf7dke8cj_285;

            xfunctionarg_xf7dke8cj_288 = functionarg_xf7dke8cj_288;

        }
        public static void CC4(GekkoSmpl smpl, P p, ref IVariable xfunctionarg_xf7dke8cj_290, ref IVariable xfunctionarg_xf7dke8cj_285, ref IVariable xfunctionarg_xf7dke8cj_288)
        {
            IVariable functionarg_xf7dke8cj_290 = xfunctionarg_xf7dke8cj_290;

            IVariable functionarg_xf7dke8cj_285 = xfunctionarg_xf7dke8cj_285;

            IVariable functionarg_xf7dke8cj_288 = xfunctionarg_xf7dke8cj_288;

            //[[commandStart]]25
            p.SetText(@"¤46"); O.InitSmpl(smpl, p);


            Program.options.freq = G.GetFreq(O.ConvertToString((new ScalarString("a"))));
            G.Writeln();
            G.Writeln("option freq = " + (G.GetFreq(O.ConvertToString((new ScalarString("a"))))).ToString().ToLower() + "");
            Program.AdjustFreq();
            //[[commandEnd]]25


            //[[commandStart]]26
            p.SetText(@"¤47"); O.InitSmpl(smpl, p);


            O.Time o26 = new O.Time();
            o26.t1 = O.ConvertToDate(i312, O.GetDateChoices.FlexibleStart);
            ;
            o26.t2 = O.ConvertToDate(functionarg_xf7dke8cj_290, O.GetDateChoices.FlexibleEnd);
            ;

            o26.Exe();

            //[[commandEnd]]26


            //[[commandStart]]27
            p.SetText(@"¤48"); O.InitSmpl(smpl, p);


            O.Open o27 = new O.Open();
            o27.openFileNames = O.ExplodeIvariables(new List(new List<IVariable> { (functionarg_xf7dke8cj_285) }));
            o27.openFileNamesAs = null;

            o27.Exe();

            //[[commandEnd]]27


            //[[commandStart]]28
            p.SetText(@"¤49"); O.InitSmpl(smpl, p);


            O.Open o28 = new O.Open();
            o28.opt_edit = "yes";

            o28.openFileNames = O.ExplodeIvariables(new List(new List<IVariable> { (new ScalarString("mdperx")) }));
            o28.openFileNamesAs = null;

            o28.Exe();

            //[[commandEnd]]28


            //[[commandStart]]29
            p.SetText(@"¤50"); O.InitSmpl(smpl, p);


            O.Clear o29 = new O.Clear();
            o29.p = p; o29.names = O.ExplodeIvariablesSeq(new List(new List<IVariable> { (new ScalarString("mdperx")) }));
            o29.Exe();

            //[[commandEnd]]29


            //[[commandStart]]30
            p.SetText(@"¤51"); O.InitSmpl(smpl, p);


            O.Index o30 = new O.Index();
            o30.opt_mute = "yes";

            o30.opt_showbank = O.ConvertToString((new ScalarString("no")));

            o30.opt_showfreq = O.ConvertToString((new ScalarString("no")));

            o30.names2 = O.ExplodeIvariablesSeq(new List(new List<IVariable> { (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("AF")))) }));
            o30.type = @"series"; o30.names1 = O.ExplodeIvariablesSeq(new List(new List<IVariable> { (O.HandleString(new ScalarString(@"")).Add(smpl, functionarg_xf7dke8cj_285).Add(smpl, O.HandleString(new ScalarString(@":"))).Add(smpl, functionarg_xf7dke8cj_288).Add(smpl, O.HandleString(new ScalarString(@"*!q")))) }));
            o30.Exe();

            //[[commandEnd]]30
            xfunctionarg_xf7dke8cj_290 = functionarg_xf7dke8cj_290;

            xfunctionarg_xf7dke8cj_285 = functionarg_xf7dke8cj_285;

            xfunctionarg_xf7dke8cj_288 = functionarg_xf7dke8cj_288;

        }
        public static void CC5(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_313, ref IVariable xfunctionarg_xf7dke8cj_285)
        {
            IVariable forloop_xe7dke6cj_313 = xforloop_xe7dke6cj_313;

            IVariable functionarg_xf7dke8cj_285 = xfunctionarg_xf7dke8cj_285;

            //[[commandStart]]32
            p.SetText(@"¤53"); O.InitSmpl(smpl, p);


            O.Collapse o32 = new O.Collapse();
            o32.lhs = O.ExplodeIvariablesSeq(new List(new List<IVariable> { (forloop_xe7dke6cj_313) }));
            o32.rhs = O.ExplodeIvariablesSeq(new List(new List<IVariable> { ((functionarg_xf7dke8cj_285)).Add(smpl, new ScalarString(":")).Add(smpl, ((forloop_xe7dke6cj_313)).Add(smpl, O.scalarStringTilde).Add(smpl, (new ScalarString("q")))) }));
            o32.type = O.ConvertToString((new ScalarString("total")));
            o32.Exe();

            //[[commandEnd]]32
            xforloop_xe7dke6cj_313 = forloop_xe7dke6cj_313;

            xfunctionarg_xf7dke8cj_285 = functionarg_xf7dke8cj_285;

        }
        public static void CC6(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]33
            p.SetText(@"¤55"); O.InitSmpl(smpl, p);


            O.Close o33 = new O.Close();
            o33.listItems = O.ExplodeIvariablesSeq(new List(new List<IVariable> { (new ScalarString("mdperx")) }));
            o33.Exe();

            //[[commandEnd]]33


            //[[commandStart]]34
            p.SetText(@"¤55"); O.InitSmpl(smpl, p);


            O.Clear o34 = new O.Clear();
            o34.p = p; o34.Exe();

            //[[commandEnd]]34
        }
        public static void CC7(GekkoSmpl smpl, P p, ref IVariable xfunctionarg_xf7dke8cj_290, ref IVariable xfunctionarg_xf7dke8cj_291, ref IVariable xfunctionarg_xf7dke8cj_285)
        {
            IVariable functionarg_xf7dke8cj_290 = xfunctionarg_xf7dke8cj_290;

            IVariable functionarg_xf7dke8cj_291 = xfunctionarg_xf7dke8cj_291;

            IVariable functionarg_xf7dke8cj_285 = xfunctionarg_xf7dke8cj_285;

            //[[commandStart]]35
            p.SetText(@"¤57"); O.InitSmpl(smpl, p);


            Program.options.freq = G.GetFreq(O.ConvertToString((new ScalarString("q"))));
            G.Writeln();
            G.Writeln("option freq = " + (G.GetFreq(O.ConvertToString((new ScalarString("q"))))).ToString().ToLower() + "");
            Program.AdjustFreq();
            //[[commandEnd]]35


            //[[commandStart]]36
            p.SetText(@"¤58"); O.InitSmpl(smpl, p);


            O.Time o36 = new O.Time();
            o36.t1 = O.ConvertToDate(new ScalarDate(G.FromStringToDate("1990q1")), O.GetDateChoices.FlexibleStart);
            ;
            o36.t2 = O.ConvertToDate(O.Lookup(smpl, null, (functionarg_xf7dke8cj_290).Add(smpl, new ScalarString("q")).Add(smpl, functionarg_xf7dke8cj_291), null, new LookupSettings(), EVariableType.Var, null), O.GetDateChoices.FlexibleEnd);
            ;

            o36.Exe();

            //[[commandEnd]]36


            //[[commandStart]]37
            p.SetText(@"¤59"); O.InitSmpl(smpl, p);


            O.Open o37 = new O.Open();
            o37.openFileNames = O.ExplodeIvariables(new List(new List<IVariable> { (functionarg_xf7dke8cj_285) }));
            o37.openFileNamesAs = null;

            o37.Exe();

            //[[commandEnd]]37


            //[[commandStart]]38
            p.SetText(@"¤61"); O.InitSmpl(smpl, p);


            O.Write o38 = new O.Write();
            o38.type = @"write"; o38.fileName = o38.fileName = O.ConvertToString((new ScalarString("mdperx")));
            ;
            o38.list1 = O.ExplodeIvariablesSeq(new List(new List<IVariable> { (O.HandleString(new ScalarString(@"")).Add(smpl, functionarg_xf7dke8cj_285).Add(smpl, O.HandleString(new ScalarString(@":*!*")))) }))
            ;
            o38.list2 = O.ExplodeIvariablesSeq(new List(new List<IVariable> { (O.HandleString(new ScalarString(@"")).Add(smpl, functionarg_xf7dke8cj_285).Add(smpl, O.HandleString(new ScalarString(@":*!*")))) }))
            ;
            o38.Exe();

            //[[commandEnd]]38


            //[[commandStart]]39
            p.SetText(@"¤62"); O.InitSmpl(smpl, p);


            O.Close o39 = new O.Close();
            o39.listItems = O.ExplodeIvariablesSeq(new List(new List<IVariable> { (functionarg_xf7dke8cj_285) }));
            o39.Exe();

            //[[commandEnd]]39


            //[[commandStart]]40
            p.SetText(@"¤62"); O.InitSmpl(smpl, p);


            O.Clear o40 = new O.Clear();
            o40.p = p; o40.Exe();

            //[[commandEnd]]40
            xfunctionarg_xf7dke8cj_290 = functionarg_xf7dke8cj_290;

            xfunctionarg_xf7dke8cj_291 = functionarg_xf7dke8cj_291;

            xfunctionarg_xf7dke8cj_285 = functionarg_xf7dke8cj_285;

        }

        public static readonly ScalarVal i292 = new ScalarVal(1990d);
        public static readonly ScalarVal i293 = new ScalarVal(1d);
        public static readonly ScalarVal i295 = new ScalarVal(1d);
        public static readonly ScalarVal i303 = new ScalarVal(1d);
        public static readonly ScalarVal i305 = new ScalarVal(1d);
        public static readonly ScalarVal i307 = new ScalarVal(1d);
        public static readonly ScalarVal i309 = new ScalarVal(1d);
        public static readonly ScalarVal i311 = new ScalarVal(1d);
        public static readonly ScalarVal i312 = new ScalarVal(1990d);
        public static void FunctionDef315()
        {

            O.PrepareUfunction(9, "procedure___dand");

            Globals.ufunctions9.Add("procedure___dand", (GekkoSmpl smpl, P p, IVariable functionarg_xf7dke8cj_283, IVariable functionarg_xf7dke8cj_284, IVariable functionarg_xf7dke8cj_285, IVariable functionarg_xf7dke8cj_286, IVariable functionarg_xf7dke8cj_287, IVariable functionarg_xf7dke8cj_288, IVariable functionarg_xf7dke8cj_289, IVariable functionarg_xf7dke8cj_290, IVariable functionarg_xf7dke8cj_291) =>

            {
                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.Deeper();
                try
                {
                    functionarg_xf7dke8cj_283 = O.TypeCheck_string(functionarg_xf7dke8cj_283, 1);
                    functionarg_xf7dke8cj_284 = O.TypeCheck_string(functionarg_xf7dke8cj_284, 2);
                    functionarg_xf7dke8cj_285 = O.TypeCheck_string(functionarg_xf7dke8cj_285, 3);
                    functionarg_xf7dke8cj_286 = O.TypeCheck_string(functionarg_xf7dke8cj_286, 4);
                    functionarg_xf7dke8cj_287 = O.TypeCheck_val(functionarg_xf7dke8cj_287, 5);
                    functionarg_xf7dke8cj_288 = O.TypeCheck_string(functionarg_xf7dke8cj_288, 6);
                    functionarg_xf7dke8cj_289 = O.TypeCheck_val(functionarg_xf7dke8cj_289, 7);
                    functionarg_xf7dke8cj_290 = O.TypeCheck_val(functionarg_xf7dke8cj_290, 8);
                    functionarg_xf7dke8cj_291 = O.TypeCheck_val(functionarg_xf7dke8cj_291, 9);


                    CC0(smpl, p, ref functionarg_xf7dke8cj_290, ref functionarg_xf7dke8cj_291);


        //[[commandSpecial]]8
        if (O.IsTrue(smpl, O.LogicalAnd(smpl, O.Equals(smpl, functionarg_xf7dke8cj_286, O.HandleString(new ScalarString(@"q"))), O.Equals(smpl, functionarg_xf7dke8cj_291, i293))))
                    {
                        CC1(smpl, p, ref functionarg_xf7dke8cj_290);

                    }
                    else
                    {
                        CC2(smpl, p, ref functionarg_xf7dke8cj_290);

                    }
        //[[commandEnd]]8


        CC3(smpl, p, ref functionarg_xf7dke8cj_285, ref functionarg_xf7dke8cj_288);


        //[[commandSpecial]]24
        if (O.IsTrue(smpl, O.Equals(smpl, functionarg_xf7dke8cj_286, O.HandleString(new ScalarString(@"a")))))
                    {
                        CC4(smpl, p, ref functionarg_xf7dke8cj_290, ref functionarg_xf7dke8cj_285, ref functionarg_xf7dke8cj_288);


            //[[commandSpecial]]31
            IVariable forloop_xe7dke6cj_313 = null;
                        int counter314 = 0;
                        for (O.IterateStart(ref forloop_xe7dke6cj_313, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("AF")))), null, new LookupSettings(), EVariableType.Var, null)); O.IterateContinue(forloop_xe7dke6cj_313, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("AF")))), null, new LookupSettings(), EVariableType.Var, null), null, null, ref counter314); O.IterateStep(forloop_xe7dke6cj_313, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("AF")))), null, new LookupSettings(), EVariableType.Var, null), null, counter314))
                        {
                            ;

                            CC5(smpl, p, ref forloop_xe7dke6cj_313, ref functionarg_xf7dke8cj_285);

                        };

            //[[commandEnd]]31


            CC6(smpl, p);

                    }
                    else
                    {
                        CC7(smpl, p, ref functionarg_xf7dke8cj_290, ref functionarg_xf7dke8cj_291, ref functionarg_xf7dke8cj_285);

                    }
        //[[commandEnd]]24


        return null;
                }
                finally
                {
                    Program.databanks.local = local0; Program.databanks.localGlobal = lg0; p.RemoveLast();
                }
            });

        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
            FunctionDef315();


            //[[commandEnd]]0



        }
    }
}
