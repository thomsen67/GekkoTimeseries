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

            Func<GraphHelper, string> print0 = (gh) =>
            {
                O.Prt o0 = new O.Prt();
                labelCounter = 0; o0.guiGraphIsRefreshing = gh.isRefreshing;
                o0.guiGraphOperator = gh.operator2;
                o0.guiGraphIsLogTransform = gh.isLogTransform;
                o0.guiGraphFontScaling = gh.fontScaling;
                o0.guiGraphSizeScaling = gh.sizeScaling;
                o0.guiGraphIsButton = gh.isButton;
                o0.prtType = "plot";
                if (gh.isIndex != null) { if (gh.isIndex == true) { o0.opt_i = GekkoTime.tSimilarToNull; } else { o0.opt_i = GekkoTime.tNull; } }
                if (gh.isYoy != null) { if (gh.isYoy == true) { o0.opt_yoy = "yes"; } else { o0.opt_yoy = "no"; } }
                if (gh.isPoints != null) { if (gh.isPoints == true) { o0.opt_linetype = "linespoints"; } else { o0.opt_linetype = "lines"; } }
                O.GetPeriods2(o0, gh);
                ESeriesMissing r1_0 = Program.options.series_array_print_missing; ESeriesMissing r2_0 = Program.options.series_array_calc_missing; ESeriesMissing r3_0 = Program.options.series_data_missing; try
                {
                    O.HandleOptionBankRef1(o0.opt_bank, o0.opt_ref); O.HandleMissing1(o0.opt_missing);
                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope0 = new O.Prt.Element();
                        ope0.labelGiven = new List<string>() { "q|[@2,5:5='q',<1239>,1:5]|[@2,5:5='q',<1239>,1:5]" };
                        smpl = new GekkoSmpl(o0.t1, o0.t2); smpl.t0 = smpl.t0.Add(-2);
                        if (o0.opt_yoy != null && o0.opt_yoy.ToLower() == Globals.yes) { smpl.t0 = smpl.t0.Add(-13); }
                        Program.GetElementOperators(o0, ope0, out ope0.operatorsFinal, out ope0.operatorsFinalAll); bankNumbers = O.Prt.GetBankNumbers(null, ope0.operatorsFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope0.variable[bankNumber] = O.Lookup(smpl, null, null, "q", null, null, new LookupSettings(), EVariableType.Var, null);
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope0);
                        }
                        smpl.bankNumber = 0;
                        o0.prtElements.Add(ope0);
                    }

                    o0.printStorageAsFuncCounter = Globals.printStorageAsFunc.Count - 1;
                    o0.Exe();
                }
                finally
                {
                    O.HandleOptionBankRef2(); O.HandleMissing2(r1_0, r2_0, r3_0);
                }
                return o0.emfName;
            };
            Globals.printStorageAsFunc.Add(Globals.printStorageAsFunc.Count, print0);
            print0(new GraphHelper());

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}