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
        public static readonly ScalarVal i3 = new ScalarVal(1d);
        public static readonly ScalarVal i5 = new ScalarVal(1d);
        public static readonly ScalarVal i7 = new ScalarVal(2d);
        public static readonly ScalarVal i9 = new ScalarVal(1d);
        public static readonly ScalarVal i11 = new ScalarVal(2d);
        public static readonly ScalarVal i14 = new ScalarVal(1d);
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

            IVariable ivTmpvar2 = O.IvConvertTo(EVariableType.Var, Functions.series(smpl, i3));
            O.Lookup(smpl, null, null, "x", null, ivTmpvar2, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar4 = O.IvConvertTo(EVariableType.Var, i5);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar4, new ScalarString("a"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar6 = O.IvConvertTo(EVariableType.Var, i7);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "x", null, null, true, EVariableType.Var), ivTmpvar6, new ScalarString("b"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar8 = O.IvConvertTo(EVariableType.Var, i9);
            O.Lookup(smpl, null, null, "xa", null, ivTmpvar8, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar10 = O.IvConvertTo(EVariableType.Var, i11);
            O.Lookup(smpl, null, null, "xb", null, ivTmpvar10, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar12 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)));
            O.Lookup(smpl, null, null, "%s", null, ivTmpvar12, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar13 = O.IvConvertTo(EVariableType.Var, i14);
            O.Lookup(smpl, null, null, "a", null, ivTmpvar13, true, EVariableType.Var)
            ;

            p.SetText(@"¤18"); O.InitSmpl(smpl, p);

            Func<GraphHelper, string> print8 = (gh) =>
            {
                O.Prt o8 = new O.Prt();
                o8.guiGraphIsRefreshing = gh.isRefreshing;
                o8.guiGraphPrintCode = gh.printCode;
                o8.guiGraphIsLogTransform = gh.isLogTransform;
                
                o8.prtType = "p";

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope8 = new O.Prt.Element();
                    ope8.label = O.SubstituteScalarsAndLists("{'a'}", false);
                    smpl = new GekkoSmpl(o8.t1.Add(-2), o8.t2);
                    ope8.printCodesFinal = Program.GetElementPrintCodes(o8, ope8); bankNumbers = O.Prt.GetBankNumbers(null, ope8.printCodesFinal); foreach (int bankNumber in bankNumbers)
                    {
                        smpl.bankNumber = bankNumber;
                        ope8.variable[bankNumber] = O.Lookup(smpl, null, (O.ReportInterior(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)))), null, false, EVariableType.Var);
                    }
                    smpl.bankNumber = 0;
                    o8.prtElements.Add(ope8);
                }

                o8.labelHelper = smpl.labelHelper;

                o8.counter = 1;
                o8.printCsCounter = Globals.printCs.Count - 1;
                o8.Exe();
                return o8.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print8);
            print8(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
