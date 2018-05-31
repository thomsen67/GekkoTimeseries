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
        public static void FunctionDef38()
        {


            //[[splitSTOP]]

            O.PrepareUfunction(1, "plotx");

            Globals.ufunctions1.Add("plotx", (GekkoSmpl smpl, P p, IVariable functionarg_36) =>
            {
                p.SetText(@"¤0"); O.InitSmpl(smpl, p);

                IVariable ivTmpvar37 = O.IvConvertTo(EVariableType.Var, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%output_path", null, null, false, EVariableType.Var), functionarg_36), new ScalarString(ScalarString.SubstituteScalarsInString(@".svg", true, false))));
                O.Lookup(smpl, null, null, "%fp", null, ivTmpvar37, true, EVariableType.Var)
                ;

                p.SetText(@"¤10"); O.InitSmpl(smpl, p);

                Func<GraphHelper, string> print3 = (gh) =>
                {
                    O.Prt o3 = new O.Prt();
                    int labelCounter = 0; o3.guiGraphIsRefreshing = gh.isRefreshing;
                    o3.guiGraphPrintCode = gh.printCode;
                    o3.guiGraphIsLogTransform = gh.isLogTransform;
                    o3.prtType = "PLOT";

                    o3.t1 = Globals.globalPeriodStart;
                    o3.t2 = Globals.globalPeriodEnd;

                    o3.opt_title = O.ConvertToString(functionarg_36);


                    o3.opt_filename = O.ConvertToString((O.scalarStringPercent).Add(smpl, (new ScalarString("fp"))));


                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope3 = new O.Prt.Element();
                        ope3.label = "{%x}";
                        smpl = new GekkoSmpl(o3.t1.Add(-2), o3.t2);
                        ope3.printCodesFinal = Program.GetElementPrintCodes(o3, ope3); bankNumbers = O.Prt.GetBankNumbers(null, ope3.printCodesFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope3.variable[bankNumber] = O.Lookup(smpl, null, (O.ReportInterior(smpl, functionarg_36, 0, labelCounter)), null, false, EVariableType.Var);
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope3);
                        }
                        smpl.bankNumber = 0;
                        o3.prtElements.Add(ope3);
                    }


                    o3.counter = 13;
                    o3.printCsCounter = Globals.printCs.Count - 1;
                    o3.labelHelper2 = O.AddLabelHelper2(smpl);
                    o3.Exe();
                    return o3.emfName;
                };
                Globals.printCs.Add(Globals.printCs.Count, print3);
                print3(new GraphHelper());

                p.SetText(@"¤12"); O.InitSmpl(smpl, p);

                Func<GraphHelper, string> print4 = (gh) =>
                {
                    O.Prt o4 = new O.Prt();
                    int labelCounter = 0; o4.guiGraphIsRefreshing = gh.isRefreshing;
                    o4.guiGraphPrintCode = gh.printCode;
                    o4.guiGraphIsLogTransform = gh.isLogTransform;
                    o4.prtType = "PLOT";

                    o4.t1 = Globals.globalPeriodStart;
                    o4.t2 = Globals.globalPeriodEnd;

                    o4.opt_title = O.ConvertToString(functionarg_36);


                    o4.opt_filename = O.ConvertToString((O.ReportInterior(smpl, O.Lookup(smpl, null, null, "%fp", null, null, false, EVariableType.Var), 0, labelCounter)));


                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope4 = new O.Prt.Element();
                        ope4.label = "{%x}";
                        smpl = new GekkoSmpl(o4.t1.Add(-2), o4.t2);
                        ope4.printCodesFinal = Program.GetElementPrintCodes(o4, ope4); bankNumbers = O.Prt.GetBankNumbers(null, ope4.printCodesFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope4.variable[bankNumber] = O.Lookup(smpl, null, (O.ReportInterior(smpl, functionarg_36, 0, labelCounter)), null, false, EVariableType.Var);
                            if (bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope4);
                        }
                        smpl.bankNumber = 0;
                        o4.prtElements.Add(ope4);
                    }


                    o4.counter = 14;
                    o4.printCsCounter = Globals.printCs.Count - 1;
                    o4.labelHelper2 = O.AddLabelHelper2(smpl);
                    o4.Exe();
                    return o4.emfName;
                };
                Globals.printCs.Add(Globals.printCs.Count, print4);
                print4(new GraphHelper());

                p.SetText(@"¤14"); O.InitSmpl(smpl, p);


                //[[splitSTOP]]
                return new ScalarString(ScalarString.SubstituteScalarsInString(@"", true, false));

                //[[splitSTART]]

                ; return null;
            });


            //[[splitSTART]]

        }

        public static readonly ScalarVal i40 = new ScalarVal(1d);
        public static readonly ScalarVal i42 = new ScalarVal(1d);
        public static readonly ScalarVal i44 = new ScalarVal(2d);
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

            IVariable ivTmpvar35 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"Analysis\Graphs\", true, false)));
            O.Lookup(smpl, null, null, "%output_path", null, ivTmpvar35, true, EVariableType.Var)
            ;

            p.SetText(@"¤6"); O.InitSmpl(smpl, p);

            FunctionDef38();


            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar39 = O.IvConvertTo(EVariableType.Var, Functions.series(smpl, i40));
            O.Lookup(smpl, null, null, "qc", null, ivTmpvar39, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar41 = O.IvConvertTo(EVariableType.Var, i42);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qc", null, null, true, EVariableType.Var), ivTmpvar41, new ScalarString("a"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar43 = O.IvConvertTo(EVariableType.Var, i44);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "qc", null, null, true, EVariableType.Var), ivTmpvar43, new ScalarString("b"))
            ;

            p.SetText(@"¤22"); O.InitSmpl(smpl, p);

            Program.Tell(O.ConvertToString(O.FunctionLookup1("plotx")(smpl, p, new ScalarString(ScalarString.SubstituteScalarsInString(@"qC", true, false)))), false);

            //[[splitSTOP]]


        }
    }
}
