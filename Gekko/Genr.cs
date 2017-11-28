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
        public static readonly ScalarVal i1 = new ScalarVal(2010d);
        public static readonly ScalarVal i2 = new ScalarVal(2012d);
        public static readonly ScalarVal i4 = new ScalarVal(1d);
        public static readonly ScalarVal i6 = new ScalarVal(1d);
        public static readonly ScalarVal i7 = new ScalarVal(2d);
        public static readonly ScalarVal i8 = new ScalarVal(3d);
        public static readonly ScalarVal i10 = new ScalarVal(4d);
        public static readonly ScalarVal i11 = new ScalarVal(5d);
        public static readonly ScalarVal i12 = new ScalarVal(6d);
        public static readonly ScalarVal i14 = new ScalarVal(7d);
        public static readonly ScalarVal i15 = new ScalarVal(8d);
        public static readonly ScalarVal i16 = new ScalarVal(9d);
        public static readonly ScalarVal i18 = new ScalarVal(14d);
        public static readonly ScalarVal i19 = new ScalarVal(15d);
        public static readonly ScalarVal i20 = new ScalarVal(16d);
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
            o0.p = p; o0.Exe(smpl);

            p.SetText(@"¤2"); O.InitSmpl(smpl);

            O.Time o1 = new O.Time();
            o1.t1 = O.ConvertToDate(i1, O.GetDateChoices.FlexibleStart);
            ;
            o1.t2 = O.ConvertToDate(i2, O.GetDateChoices.FlexibleEnd);
            ;

            o1.Exe();

            p.SetText(@"¤0"); O.InitSmpl(smpl);

            IVariable ivTmpvar3 = O.IvConvertTo(EVariableType.Var, Functions.series(smpl, i4));
            O.Lookup(smpl, null, null, "xx3", null, ivTmpvar3, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl);

            IVariable ivTmpvar5 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(i6, i7, i8));
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx3", null, null, true, EVariableType.Var), ivTmpvar5, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
            , new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false))
            )
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl);

            IVariable ivTmpvar9 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(i10, i11, i12));
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx3", null, null, true, EVariableType.Var), ivTmpvar9, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
            , new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false))
            )
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl);

            IVariable ivTmpvar13 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(i14, i15, i16));
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx3", null, null, true, EVariableType.Var), ivTmpvar13, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
            , new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
            )
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl);

            IVariable ivTmpvar17 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(i18, i19, i20));
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx3", null, null, true, EVariableType.Var), ivTmpvar17, new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))
            , new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))
            )
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl);

            IVariable ivTmpvar21 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))));
            O.Lookup(smpl, null, null, "#m1", null, ivTmpvar21, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl);

            IVariable ivTmpvar22 = O.IvConvertTo(EVariableType.Var, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"y", true, false))));
            O.Lookup(smpl, null, null, "#m2", null, ivTmpvar22, true, EVariableType.Var)
            ;

            p.SetText(@"¤27"); O.InitSmpl(smpl);
            Func<IVariable, IVariable> func26 = (IVariable listloop_m223) => {
                Series temp25 = new Series(ESeriesType.Normal, Program.options.freq, null); temp25.SetZero(smpl);

                foreach (IVariable listloop_m124 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1")))), null, false, EVariableType.Var))) {
                    temp25.InjectAdd(smpl, temp25, O.Indexer(O.Indexer2(smpl, listloop_m124, listloop_m223), smpl, O.Lookup(smpl, null, null, "xx3", null, null, false, EVariableType.Var), listloop_m124, listloop_m223));

                }
                return temp25;

            };

            Func<IVariable> func28 = () => {
                List temp27 = new List();

                foreach (IVariable listloop_m223 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m2")))), null, false, EVariableType.Var))) {
                    temp27.Add(func26(listloop_m223));

                }
                return temp27;

            };


            O.Prt o9 = new O.Prt();
            o9.prtType = "p";

            {
                List<int> bankNumbers = null;
                O.Prt.Element ope9 = new O.Prt.Element();
                ope9.label = O.SubstituteScalarsAndLists("unfold(#m2, sum(#m1, xx3[#m1, #m2]))", false);
                smpl = new GekkoSmpl(o9.t1.Add(-2), o9.t2);
                ope9.printCodesFinal = Program.GetElementPrintCodes(o9, ope9); bankNumbers = O.Prt.GetBankNumbers(null, ope9.printCodesFinal); foreach (int bankNumber in bankNumbers) {
                    smpl.bankNumber = bankNumber;
                    ope9.variable[bankNumber] = func28();
                }
                smpl.bankNumber = 0;
                o9.prtElements.Add(ope9);
            }


            o9.counter = 1;
            o9.Exe();


            //[[splitSTOP]]


        }
    }
}
