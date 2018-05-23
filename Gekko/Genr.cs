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
        public static readonly ScalarVal i111 = new ScalarVal(3d);
        public static readonly ScalarVal i113 = new ScalarVal(1d);
        public static readonly ScalarVal i115 = new ScalarVal(2d);
        public static readonly ScalarVal i117 = new ScalarVal(11d);
        public static readonly ScalarVal i119 = new ScalarVal(12d);
        public static readonly ScalarVal i121 = new ScalarVal(51d);
        public static readonly ScalarVal i123 = new ScalarVal(52d);
        public static readonly ScalarVal i125 = new ScalarVal(511d);
        public static readonly ScalarVal i127 = new ScalarVal(512d);
        public static readonly ScalarVal i129 = new ScalarVal(1d);
        public static readonly ScalarVal i131 = new ScalarVal(1d);
        public static readonly ScalarVal i133 = new ScalarVal(1d);
        public static readonly ScalarVal i135 = new ScalarVal(1d);
        public static readonly ScalarVal i137 = new ScalarVal(1d);
        public static readonly ScalarVal i139 = new ScalarVal(1d);
        public static readonly ScalarVal i141 = new ScalarVal(1d);
        public static readonly ScalarVal i143 = new ScalarVal(2d);
        public static readonly ScalarVal i154 = new ScalarVal(1d);
        public static readonly ScalarVal i156 = new ScalarVal(1d);
        public static readonly ScalarVal i158 = new ScalarVal(1d);
        public static readonly ScalarVal i163 = new ScalarVal(1d);
        public static readonly ScalarVal i165 = new ScalarVal(2d);
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

            IVariable ivTmpvar107 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))));
            O.Lookup(smpl, null, null, "#m", null, ivTmpvar107, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar108 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))));
            O.Lookup(smpl, null, null, "#n", null, ivTmpvar108, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar109 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"e", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"f", true, false))));
            O.Lookup(smpl, null, null, "#k", null, ivTmpvar109, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar110 = O.IvConvertTo(EVariableType.Var, Functions.series(smpl, i111));
            O.Lookup(smpl, null, null, "x", null, ivTmpvar110, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar112 = O.IvConvertTo(EVariableType.Var, i113);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar112, new ScalarString("a"), new ScalarString("x"), new ScalarString("e"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar114 = O.IvConvertTo(EVariableType.Var, i115);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar114, new ScalarString("b"), new ScalarString("x"), new ScalarString("e"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar116 = O.IvConvertTo(EVariableType.Var, i117);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar116, new ScalarString("a"), new ScalarString("y"), new ScalarString("e"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar118 = O.IvConvertTo(EVariableType.Var, i119);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar118, new ScalarString("b"), new ScalarString("y"), new ScalarString("e"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar120 = O.IvConvertTo(EVariableType.Var, i121);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar120, new ScalarString("a"), new ScalarString("x"), new ScalarString("f"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar122 = O.IvConvertTo(EVariableType.Var, i123);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar122, new ScalarString("b"), new ScalarString("x"), new ScalarString("f"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar124 = O.IvConvertTo(EVariableType.Var, i125);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar124, new ScalarString("a"), new ScalarString("y"), new ScalarString("f"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar126 = O.IvConvertTo(EVariableType.Var, i127);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar126, new ScalarString("b"), new ScalarString("y"), new ScalarString("f"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar128 = O.IvConvertTo(EVariableType.Var, Functions.series(smpl, i129));
            O.Lookup(smpl, null, null, "x1", null, ivTmpvar128, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar130 = O.IvConvertTo(EVariableType.Var, Functions.series(smpl, i131));
            O.Lookup(smpl, null, null, "x2", null, ivTmpvar130, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar132 = O.IvConvertTo(EVariableType.Var, i133);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x1", null, null, true, EVariableType.Var), ivTmpvar132, new ScalarString("a"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar134 = O.IvConvertTo(EVariableType.Var, i135);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x1", null, null, true, EVariableType.Var), ivTmpvar134, new ScalarString("b"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar136 = O.IvConvertTo(EVariableType.Var, i137);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x2", null, null, true, EVariableType.Var), ivTmpvar136, new ScalarString("x"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar138 = O.IvConvertTo(EVariableType.Var, i139);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x2", null, null, true, EVariableType.Var), ivTmpvar138, new ScalarString("y"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar140 = O.IvConvertTo(EVariableType.Var, i141);
            O.Lookup(smpl, null, null, "xa", null, ivTmpvar140, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar142 = O.IvConvertTo(EVariableType.Var, i143);
            O.Lookup(smpl, null, null, "xb", null, ivTmpvar142, true, EVariableType.Var)
            ;

            p.SetText(@"¤23"); O.InitSmpl(smpl, p);
            Func<IVariable, IVariable, IVariable> func148 = (IVariable listloop_m144, IVariable listloop_n145) =>
            {
                //SUM FUNCTION

                var smplCommandRemember149 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp147 = new Series(ESeriesType.Normal, Program.options.freq, null); temp147.SetZero(smpl);

                //lbl
                bool lblFirst = true;

                foreach (IVariable listloop_k146 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("k")))), null, false, EVariableType.Var)))
                {
                    //lbl
                    smpl.labelHelper.Clear();

                    temp147.InjectAdd(smpl, temp147, O.Indexer(O.Indexer2(smpl, listloop_m144, listloop_n145, listloop_k146), smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_m144, 0, lblFirst), O.ReportInterior(smpl, listloop_n145, 1, lblFirst), O.ReportInterior(smpl, listloop_k146, 2, lblFirst)));

                    //lbl
                    if (lblFirst)
                    {
                        O.AddLabelHelper(smpl);
                        lblFirst = false;
                    }
                    
                }
                smpl.command = smplCommandRemember149;
                return temp147;

            };

            Func<IVariable> func151 = () =>
            {
                //UNFOLD FUNCTION, always outermost

                var smplCommandRemember152 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp150 = new List();

                //lbl
                List<List<IVariable>> lblTemp1 = new List<List<IVariable>>();                                

                foreach (IVariable listloop_m144 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m")))), null, false, EVariableType.Var)))
                {
                    foreach (IVariable listloop_n145 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("n")))), null, false, EVariableType.Var)))
                    {
                        //lbl
                        smpl.labelHelper.Clear();
                        bool lblFirst = true;  //record them all

                        temp150.Add(O.Add(smpl, O.Add(smpl, O.Indexer(O.Indexer2(smpl, listloop_m144), smpl, O.Lookup(smpl, null, null, "x1", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_m144, 0, lblFirst)), O.Indexer(O.Indexer2(smpl, listloop_n145), smpl, O.Lookup(smpl, null, null, "x2", null, null, false, EVariableType.Var), O.ReportInterior(smpl, listloop_n145, 0, lblFirst))), func148(listloop_m144, listloop_n145)));

                        //lbl
                        O.AddLabelHelper(smpl);
                    }
                }
                smpl.command = smplCommandRemember152;
                return temp150;

            };


            Func<GraphHelper, string> print20 = (gh) =>
            {
                //PRT FUNCTION

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
                    ope20.printCodesFinal = Program.GetElementPrintCodes(o20, ope20); bankNumbers = O.Prt.GetBankNumbers(null, ope20.printCodesFinal); foreach (int bankNumber in bankNumbers)
                    {
                        smpl.bankNumber = bankNumber;

                        //lbl
                        smpl.labelHelper2.Clear();
                        smpl.labelHelper.Clear();

                        ope20.variable[bankNumber] = func151();
                    }
                    smpl.bankNumber = 0;
                    o20.prtElements.Add(ope20);
                }


                o20.counter = 4;
                o20.printCsCounter = Globals.printCs.Count - 1;
                o20.labelHelper = smpl.labelHelper;
                o20.Exe();
                return o20.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print20);
            print20(new GraphHelper());

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar153 = O.IvConvertTo(EVariableType.Var, i154);
            O.Lookup(smpl, null, null, "a", null, ivTmpvar153, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar155 = O.IvConvertTo(EVariableType.Var, i156);
            O.Lookup(smpl, null, null, "b", null, ivTmpvar155, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar157 = O.IvConvertTo(EVariableType.Var, i158);
            O.Lookup(smpl, null, null, "c", null, ivTmpvar157, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar159 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)));
            O.Lookup(smpl, null, null, "%s", null, ivTmpvar159, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar160 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"e", true, false)));
            O.Lookup(smpl, null, null, "%b", null, ivTmpvar160, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar161 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"f", true, false)));
            O.Lookup(smpl, null, null, "%e", null, ivTmpvar161, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar162 = O.IvConvertTo(EVariableType.Var, i163);
            O.Lookup(smpl, null, null, "a", null, ivTmpvar162, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar164 = O.IvConvertTo(EVariableType.Var, i165);
            O.Lookup(smpl, null, null, "af", null, ivTmpvar164, true, EVariableType.Var)
            ;

            p.SetText(@"¤44"); O.InitSmpl(smpl, p);

            O.Clone o29 = new O.Clone();
            o29.Exe();


            //[[splitSTOP]]


        }

        
    }
}
