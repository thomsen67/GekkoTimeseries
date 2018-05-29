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
        public static readonly ScalarVal i5 = new ScalarVal(3d);
        public static readonly ScalarVal i7 = new ScalarVal(1d);
        public static readonly ScalarVal i9 = new ScalarVal(2d);
        public static readonly ScalarVal i11 = new ScalarVal(11d);
        public static readonly ScalarVal i13 = new ScalarVal(12d);
        public static readonly ScalarVal i15 = new ScalarVal(51d);
        public static readonly ScalarVal i17 = new ScalarVal(52d);
        public static readonly ScalarVal i19 = new ScalarVal(511d);
        public static readonly ScalarVal i21 = new ScalarVal(512d);
        public static readonly ScalarVal i23 = new ScalarVal(1d);
        public static readonly ScalarVal i25 = new ScalarVal(1d);
        public static readonly ScalarVal i27 = new ScalarVal(1d);
        public static readonly ScalarVal i29 = new ScalarVal(1d);
        public static readonly ScalarVal i31 = new ScalarVal(1d);
        public static readonly ScalarVal i33 = new ScalarVal(1d);
        public static readonly ScalarVal i35 = new ScalarVal(1d);
        public static readonly ScalarVal i37 = new ScalarVal(2d);
        public static readonly ScalarVal i54 = new ScalarVal(1d);
        public static readonly ScalarVal i56 = new ScalarVal(1d);
        public static readonly ScalarVal i58 = new ScalarVal(1d);
        public static readonly ScalarVal i63 = new ScalarVal(1d);
        public static readonly ScalarVal i65 = new ScalarVal(2d);
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar1 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))));
            O.Lookup(smpl, null, null, "#m", null, ivTmpvar1, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar2 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))));
            O.Lookup(smpl, null, null, "#n", null, ivTmpvar2, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar3 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"e", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"f", true, false))));
            O.Lookup(smpl, null, null, "#k", null, ivTmpvar3, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar4 = O.IvConvertTo(EVariableType.Var, Functions.series(smpl, i5));
            O.Lookup(smpl, null, null, "x", null, ivTmpvar4, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar6 = O.IvConvertTo(EVariableType.Var, i7);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar6, new ScalarString("a"), new ScalarString("x"), new ScalarString("e"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar8 = O.IvConvertTo(EVariableType.Var, i9);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar8, new ScalarString("b"), new ScalarString("x"), new ScalarString("e"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar10 = O.IvConvertTo(EVariableType.Var, i11);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar10, new ScalarString("a"), new ScalarString("y"), new ScalarString("e"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar12 = O.IvConvertTo(EVariableType.Var, i13);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar12, new ScalarString("b"), new ScalarString("y"), new ScalarString("e"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar14 = O.IvConvertTo(EVariableType.Var, i15);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar14, new ScalarString("a"), new ScalarString("x"), new ScalarString("f"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar16 = O.IvConvertTo(EVariableType.Var, i17);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar16, new ScalarString("b"), new ScalarString("x"), new ScalarString("f"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar18 = O.IvConvertTo(EVariableType.Var, i19);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar18, new ScalarString("a"), new ScalarString("y"), new ScalarString("f"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar20 = O.IvConvertTo(EVariableType.Var, i21);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar20, new ScalarString("b"), new ScalarString("y"), new ScalarString("f"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar22 = O.IvConvertTo(EVariableType.Var, Functions.series(smpl, i23));
            O.Lookup(smpl, null, null, "x1", null, ivTmpvar22, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar24 = O.IvConvertTo(EVariableType.Var, Functions.series(smpl, i25));
            O.Lookup(smpl, null, null, "x2", null, ivTmpvar24, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar26 = O.IvConvertTo(EVariableType.Var, i27);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x1", null, null, true, EVariableType.Var), ivTmpvar26, new ScalarString("a"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar28 = O.IvConvertTo(EVariableType.Var, i29);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x1", null, null, true, EVariableType.Var), ivTmpvar28, new ScalarString("b"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar30 = O.IvConvertTo(EVariableType.Var, i31);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x2", null, null, true, EVariableType.Var), ivTmpvar30, new ScalarString("x"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar32 = O.IvConvertTo(EVariableType.Var, i33);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x2", null, null, true, EVariableType.Var), ivTmpvar32, new ScalarString("y"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar34 = O.IvConvertTo(EVariableType.Var, i35);
            O.Lookup(smpl, null, null, "xa", null, ivTmpvar34, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar36 = O.IvConvertTo(EVariableType.Var, i37);
            O.Lookup(smpl, null, null, "xb", null, ivTmpvar36, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar38 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)));
            O.Lookup(smpl, null, null, "%s", null, ivTmpvar38, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar39 = O.IvConvertTo(EVariableType.List, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"xa", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"xb", true, false))));
            O.Lookup(smpl, null, null, "m3", null, ivTmpvar39, true, EVariableType.List)
            ;

            p.SetText(@"¤29"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable, IVariable, IVariable> func45 = (IVariable listloop_m42, IVariable listloop_m40, IVariable listloop_n41) =>
            {
                var smplCommandRemember46 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp44 = new Series(ESeriesType.Normal, Program.options.freq, null); temp44.SetZero(smpl);

                foreach (IVariable listloop_k43 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, false, EVariableType.Var)))
                {
                    temp44.InjectAdd(smpl, temp44, O.Indexer(O.Indexer2(smpl, listloop_m42, listloop_n41, listloop_k43), smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_m42, 0, labelCounter), O.ReportInterior(smpl, listloop_n41, 1, labelCounter), O.ReportInterior(smpl, listloop_k43, 2, labelCounter)));

                    labelCounter++;
                }
                smpl.command = smplCommandRemember46;
                return temp44;

            };

            Func<IVariable, IVariable, IVariable> func48 = (IVariable listloop_m40, IVariable listloop_n41) =>
            {
                var smplCommandRemember49 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp47 = new Series(ESeriesType.Normal, Program.options.freq, null); temp47.SetZero(smpl);

                foreach (IVariable listloop_m42 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m")))), null, false, EVariableType.Var)))
                {
                    temp47.InjectAdd(smpl, temp47, func45(listloop_m42, listloop_m40, listloop_n41));

                    labelCounter++;
                }
                smpl.command = smplCommandRemember49;
                return temp47;

            };

            Func<IVariable> func51 = () =>
            {
                var smplCommandRemember52 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp50 = new List();

                foreach (IVariable listloop_m40 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m")))), null, false, EVariableType.Var)))
                {
                    foreach (IVariable listloop_n41 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("n")))), null, false, EVariableType.Var)))
                    {
                        O.ClearLabelHelper(smpl);
                        temp50.Add(O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, listloop_m40), smpl, O.Lookup(smpl, null, null, "x1", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_m40, 0, labelCounter)), O.Indexer(O.Indexer2(smpl, listloop_n41), smpl, O.Lookup(smpl, null, null, "x2", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_n41, 0, labelCounter))), func48(listloop_m40, listloop_n41)));

                        O.AddLabelHelper(smpl);
                    }
                }
                smpl.command = smplCommandRemember52;
                return temp50;

            };


            Func<GraphHelper, string> print22 = (gh) =>
            {
                O.Prt o22 = new O.Prt();
                int labelCounter = 0; o22.guiGraphIsRefreshing = gh.isRefreshing;
                o22.guiGraphPrintCode = gh.printCode;
                o22.guiGraphIsLogTransform = gh.isLogTransform;
                o22.prtType = "p";

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope22 = new O.Prt.Element();
                    ope22.label = O.SubstituteScalarsAndLists("|||m, n|||x1[#m]+x2[#n]+sum(#m, sum(#k,x[#m,#n,#k]))", false);
                    smpl = new GekkoSmpl(o22.t1.Add(-2), o22.t2);
                    ope22.printCodesFinal = Program.GetElementPrintCodes(o22, ope22); bankNumbers = O.Prt.GetBankNumbers(null, ope22.printCodesFinal); foreach (int bankNumber in bankNumbers)
                    {
                        smpl.bankNumber = bankNumber;
                        ope22.variable[bankNumber] = func51();
                    }
                    smpl.bankNumber = 0;
                    o22.prtElements.Add(ope22);
                }


                o22.counter = 1;
                o22.printCsCounter = Globals.printCs.Count - 1;
                o22.labelHelper2 = O.AddLabelHelper2(smpl);
                o22.Exe();
                return o22.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print22);
            print22(new GraphHelper());

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar53 = O.IvConvertTo(EVariableType.Var, i54);
            O.Lookup(smpl, null, null, "a", null, ivTmpvar53, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar55 = O.IvConvertTo(EVariableType.Var, i56);
            O.Lookup(smpl, null, null, "b", null, ivTmpvar55, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar57 = O.IvConvertTo(EVariableType.Var, i58);
            O.Lookup(smpl, null, null, "c", null, ivTmpvar57, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar59 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)));
            O.Lookup(smpl, null, null, "%s", null, ivTmpvar59, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar60 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"e", true, false)));
            O.Lookup(smpl, null, null, "%b", null, ivTmpvar60, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar61 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"f", true, false)));
            O.Lookup(smpl, null, null, "%e", null, ivTmpvar61, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar62 = O.IvConvertTo(EVariableType.Var, i63);
            O.Lookup(smpl, null, null, "a", null, ivTmpvar62, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar64 = O.IvConvertTo(EVariableType.Var, i65);
            O.Lookup(smpl, null, null, "af", null, ivTmpvar64, true, EVariableType.Var)
            ;

            p.SetText(@"¤51"); O.InitSmpl(smpl, p);

            O.Clone o31 = new O.Clone();
            o31.Exe();


            //[[splitSTOP]]


        }
    }
}
