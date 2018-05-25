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
        public static readonly ScalarVal i48 = new ScalarVal(1d);
        public static readonly ScalarVal i50 = new ScalarVal(1d);
        public static readonly ScalarVal i52 = new ScalarVal(1d);
        public static readonly ScalarVal i57 = new ScalarVal(1d);
        public static readonly ScalarVal i59 = new ScalarVal(2d);
        public static void ClearTS(P p) {
        }
        public static void ClearScalar(P p) {
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

            p.SetText(@"¤23"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable, IVariable> func42 = (IVariable listloop_m38, IVariable listloop_n39) => {
                var smplCommandRemember43 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp41 = new Series(ESeriesType.Normal, Program.options.freq, null); temp41.SetZero(smpl);

                int labelCounter = 0;
                foreach (IVariable listloop_k40 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, false, EVariableType.Var))) {
                    //temp41.InjectAdd(smpl, temp41, O.Indexer(O.Indexer2(smpl, listloop_m38, listloop_n39, listloop_k40), smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_m38, 0, labelCounter)O.ReportInterior(smpl, listloop_n39, 1, labelCounter)O.ReportInterior(smpl, listloop_k40, 2, labelCounter)));

                    labelCounter++;
                }
                smpl.command = smplCommandRemember43;
                return temp41;

            };

            Func<IVariable> func45 = () => {
                var smplCommandRemember46 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp44 = new List();

                int labelCounter = 0;
                foreach (IVariable listloop_m38 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m")))), null, false, EVariableType.Var))) {
                    foreach (IVariable listloop_n39 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("n")))), null, false, EVariableType.Var))) {
                        O.ClearLabelHelper(smpl);
                        temp44.Add(O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, listloop_m38), smpl, O.Lookup(smpl, null, null, "x1", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_m38, 0, labelCounter)), O.Indexer(O.Indexer2(smpl, listloop_n39), smpl, O.Lookup(smpl, null, null, "x2", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_n39, 0, labelCounter))), func42(listloop_m38, listloop_n39)));

                        O.AddLabelHelper(smpl);
                    }
                }
                smpl.command = smplCommandRemember46;
                return temp44;

            };


            Func<GraphHelper, string> print20 = (gh) =>
            {
                O.Prt o20 = new O.Prt();
                o20.guiGraphIsRefreshing = gh.isRefreshing;
                o20.guiGraphPrintCode = gh.printCode;
                o20.guiGraphIsLogTransform = gh.isLogTransform;
                o20.prtType = "p";

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope20 = new O.Prt.Element();
                    ope20.label = O.SubstituteScalarsAndLists("|||m, n|||x1[#m]+x2[#n]+sum(#k,x[#m,#n,#k])", false);
                    smpl = new GekkoSmpl(o20.t1.Add(-2), o20.t2);
                    ope20.printCodesFinal = Program.GetElementPrintCodes(o20, ope20); bankNumbers = O.Prt.GetBankNumbers(null, ope20.printCodesFinal); foreach (int bankNumber in bankNumbers) {
                        smpl.bankNumber = bankNumber;
                        ope20.variable[bankNumber] = func45();
                    }
                    smpl.bankNumber = 0;
                    o20.prtElements.Add(ope20);
                }


                o20.counter = 1;
                o20.printCsCounter = Globals.printCs.Count - 1;
                o20.labelHelper2 = smpl.labelHelper2;
                o20.Exe();
                return o20.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print20);
            print20(new GraphHelper());

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar47 = O.IvConvertTo(EVariableType.Var, i48);
            O.Lookup(smpl, null, null, "a", null, ivTmpvar47, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar49 = O.IvConvertTo(EVariableType.Var, i50);
            O.Lookup(smpl, null, null, "b", null, ivTmpvar49, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar51 = O.IvConvertTo(EVariableType.Var, i52);
            O.Lookup(smpl, null, null, "c", null, ivTmpvar51, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar53 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)));
            O.Lookup(smpl, null, null, "%s", null, ivTmpvar53, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar54 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"e", true, false)));
            O.Lookup(smpl, null, null, "%b", null, ivTmpvar54, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar55 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"f", true, false)));
            O.Lookup(smpl, null, null, "%e", null, ivTmpvar55, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar56 = O.IvConvertTo(EVariableType.Var, i57);
            O.Lookup(smpl, null, null, "a", null, ivTmpvar56, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar58 = O.IvConvertTo(EVariableType.Var, i59);
            O.Lookup(smpl, null, null, "af", null, ivTmpvar58, true, EVariableType.Var)
            ;

            p.SetText(@"¤44"); O.InitSmpl(smpl, p);

            O.Clone o29 = new O.Clone();
            o29.Exe();


            //[[splitSTOP]]


        }
    }
}
