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
        public static void C0(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]0
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Time o0 = new O.Time();
            o0.t1 = O.ConvertToDate(i1, O.GetDateChoices.FlexibleStart);
            ;
            o0.t2 = O.ConvertToDate(i2, O.GetDateChoices.FlexibleEnd);
            ;

            o0.Exe();

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);

            O.Assignment o1 = new O.Assignment();


            Action assign_4 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3 = O.ExplodeIvariablesSeq(new List(new List<IVariable> { new ScalarString("1"), new ScalarString("2") }));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "#m", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
            };
            Func<bool> check_4 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3 = O.ExplodeIvariablesSeq(new List(new List<IVariable> { new ScalarString("1"), new ScalarString("2") }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar3.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "#m", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_4, check_4, o1);

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetText(@"¤3"); O.InitSmpl(smpl, p);

            O.Assignment o2 = new O.Assignment();


            Action assign_6 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar5 = O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(), EVariableType.Var, null);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar5, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_6 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar5 = O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(), EVariableType.Var, null);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar5.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar5, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_6, check_6, o2);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);


            Func<GraphHelper, string> print3 = (gh) =>
            {
                O.Prt o3 = new O.Prt();
                labelCounter = 0; o3.guiGraphIsRefreshing = gh.isRefreshing;
                o3.guiGraphOperator = gh.operator2;
                o3.guiGraphIsLogTransform = gh.isLogTransform;
                o3.prtType = "p";
                ESeriesMissing r1_3 = Program.options.series_array_print_missing; ESeriesMissing r2_3 = Program.options.series_normal_print_missing; try
                {
                    O.HandleOptionBankRef1(o3.opt_bank, o3.opt_ref); O.HandleMissing1(o3.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope3 = new O.Prt.Element();
                        ope3.labelGiven = new List<string>() { "x|[@30,42:42='x',<776>,4:2]|[@30,42:42='x',<776>,4:2]" };
                        smpl = new GekkoSmpl(o3.t1, o3.t2); smpl.t0 = smpl.t0.Add(-2);
                        ope3.operatorsFinal = Program.GetElementOperators(o3, ope3); bankNumbers = O.Prt.GetBankNumbers(null, ope3.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope3.variable[bankNumber] = O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null);
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope3);
                        }
                        smpl.bankNumber = 0;
                        o3.prtElements.Add(ope3);
                    }

                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_3, r2_3);
                }
                o3.counter = 1;
                o3.printCsCounter = Globals.printCs.Count - 1;
                o3.Exe();
                return o3.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print3);
            print3(new GraphHelper());

            //[[commandEnd]]3
        }


        public static readonly ScalarVal i1 = new ScalarVal(2001d);
        public static readonly ScalarVal i2 = new ScalarVal(2002d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
