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
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe(smpl);

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetStack(@"¤2"); O.InitSmpl(smpl, p);
            O.Assignment o1 = new O.Assignment();
            o1.opt_trace = @"@x = 1";


            Globals.precedentsSeries = null;
            Action assign_3 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar1 = i2;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, "REF", "x", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
            };
            Func<bool> check_3 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar1 = i2;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar1.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, "REF", "x", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_3, check_3, o1, p);

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetStack(@"¤3"); O.InitSmpl(smpl, p);
            O.Assignment o2 = new O.Assignment();
            o2.opt_trace = @"%s=1";


            Globals.precedentsSeries = null;
            Action assign_6 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar4 = i5;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%s", null, ivTmpvar4, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_6 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar4 = i5;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar4.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%s", null, ivTmpvar4, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_6, check_6, o2, p);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetStack(@"¤4"); O.InitSmpl(smpl, p);

            Func<GraphHelper, string> print3 = (gh) =>
            {
                O.Prt o3 = new O.Prt();
                O.DatabankSearchHelper1();
                labelCounter = 0; o3.guiGraphIsRefreshing = gh.isRefreshing;
                o3.guiGraphOperator = gh.operator2;
                o3.guiGraphIsLogTransform = gh.isLogTransform;
                o3.guiGraphFontScaling = gh.fontScaling;
                o3.guiGraphSizeScaling = gh.sizeScaling;
                o3.guiGraphIsButton = gh.isButton;
                o3.prtType = "plot";
                if (gh.isIndex != null) { if (gh.isIndex == true) { o3.opt_i = GekkoTime.tSimilarToNull; } else { o3.opt_i = GekkoTime.tNull; } }
                if (gh.isYoy != null) { if (gh.isYoy == true) { o3.opt_yoy = "yes"; } else { o3.opt_yoy = "no"; } }
                if (gh.isPoints != null) { if (gh.isPoints == true) { o3.opt_linetype = "linespoints"; } else { o3.opt_linetype = "lines"; } }
                if (gh.fileName != null) { o3.opt_filename = gh.fileName; }
                O.GetPeriods2(o3, gh);
                ESeriesMissing r1_3 = Program.options.series_array_print_missing; ESeriesMissing r2_3 = Program.options.series_array_calc_missing; ESeriesMissing r3_3 = Program.options.series_data_missing; try
                {
                    O.HandleOptionBankRef1(o3.opt_bank, o3.opt_ref); O.HandleMissing1(o3.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope3 = new O.Prt.Element();
                        ope3.labelGiven = new List<string>() { "x|[@21,31:31='x',<901>,4:5]|[@21,31:31='x',<901>,4:5]" };
                        smpl = new GekkoSmpl(o3.t1, o3.t2); smpl.t0 = smpl.t0.Add(-2);
                        if (o3.opt_yoy != null && o3.opt_yoy.ToLower() == Globals.yes) { smpl.t0 = smpl.t0.Add(-13); }
                        Program.GetElementOperators(o3, ope3, out ope3.operatorsFinal, out ope3.operatorsFinalAll); bankNumbers = O.Prt.GetBankNumbers(null, ope3.operatorsFinal);
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

                    o3.printStorageAsFuncCounter = Globals.printStorageAsFunc.Count - 1;
                    o3.Exe();
                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_3, r2_3, r3_3);
                }
                return o3.emfName;
            };
            Globals.printStorageAsFunc.Add(Globals.printStorageAsFunc.Count, print3);
            print3(new GraphHelper());

            //[[commandEnd]]3
        }


        public static readonly ScalarVal i2 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i5 = new ScalarVal(1d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
