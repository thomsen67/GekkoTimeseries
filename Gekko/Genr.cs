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
        public static void FunctionDef84()
        {


            //[[splitSTOP]]

            O.PrepareUfunction(2, "f");

            Globals.ufunctions2.Add("f", (GekkoSmpl smpl, P p, IVariable functionarg_82, IVariable functionarg_83) =>
            {
                functionarg_82 = O.TypeCheck_series(functionarg_82, 1);
                functionarg_83 = O.TypeCheck_string(functionarg_83, 2);

                p.SetText(@"¤1"); O.InitSmpl(smpl, p);


                //[[splitSTOP]]
                return O.TypeCheck_series(O.Indexer(O.Indexer2(smpl, O.ReportLabel(smpl, functionarg_83
                , "%¨s|[@22,51:51='%',<1166>,1:51]|[@24,53:53='s',<1198>,1:53]")), smpl, functionarg_82, O.ReportLabel(smpl, functionarg_83
                , "%¨s|[@22,51:51='%',<1166>,1:51]|[@24,53:53='s',<1198>,1:53]")), 0);

                //[[splitSTART]]


                return null;
            });


            //[[splitSTART]]

        }

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
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            FunctionDef84();


            p.SetText(@"¤2"); O.InitSmpl(smpl, p);
            Func<IVariable> func87 = () =>
            {
                var smplCommandRemember88 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp86 = new List();

                foreach (IVariable listloop_a85 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, false, EVariableType.Var)))
                {
                    temp86.Add(O.Indexer(O.Indexer2(smpl, O.AddSpecial(smpl, listloop_a85, new ScalarString(ScalarString.SubstituteScalarsInString(@"z", true, false)), false)), smpl, O.Lookup(smpl, null, (O.ReportLabel(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%s9", null, null, false, EVariableType.Var), O.Lookup(smpl, null, null, "%s9", null, null, false, EVariableType.Var)), "%¨s9+%¨s9|[@38,82:82='%',<1166>,2:19]|[@44,89:90='s9',<1198>,2:26]")), null, false, EVariableType.Var), O.ReportLabel(smpl, O.AddSpecial(smpl, listloop_a85, new ScalarString(ScalarString.SubstituteScalarsInString(@"z", true, false)), false), "#¨a+'z'|[@47,95:95='#',<1159>,2:32]|[@51,99:101=''z'',<1154>,2:36]")));

                }
                smpl.command = smplCommandRemember88;
                return temp86;

            };


            Func<GraphHelper, string> print2 = (gh) =>
            {
                O.Prt o2 = new O.Prt();
                labelCounter = 0; o2.guiGraphIsRefreshing = gh.isRefreshing;
                o2.guiGraphPrintCode = gh.printCode;
                o2.guiGraphIsLogTransform = gh.isLogTransform;
                o2.prtType = "p";

                o2.t1 = Globals.globalPeriodStart;
                o2.t2 = Globals.globalPeriodEnd;

                o2.printCodes.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));



                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope2 = new O.Prt.Element();
                    ope2.labelGiven = new List<string>() { "|||a|||{%¨s9+%¨s9}[_[#¨a+'z']|[@37,81:81='{',<1190>,2:18]|[@52,102:102=']',<1155>,2:39]" };
                    smpl = new GekkoSmpl(o2.t1.Add(-2), o2.t2);
                    ope2.printCodesFinal = Program.GetElementPrintCodes(o2, ope2); bankNumbers = O.Prt.GetBankNumbers(null, ope2.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope2.variable[bankNumber] = func87();
                        O.PrtElementHandleLabel(smpl, ope2);
                    }
                    smpl.bankNumber = 0;
                    o2.prtElements.Add(ope2);
                }

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope2 = new O.Prt.Element();
                    ope2.labelGiven = new List<string>() { "{%¨s9+%¨s9}[_[%¨s+%¨{%¨{''+%¨s3}+''}¨2+'z']|[@55,105:105='{',<1190>,2:42]|[@88,147:147=']',<1155>,2:84]" };
                    smpl = new GekkoSmpl(o2.t1.Add(-2), o2.t2);
                    ope2.printCodesFinal = Program.GetElementPrintCodes(o2, ope2); bankNumbers = O.Prt.GetBankNumbers(null, ope2.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope2.variable[bankNumber] = O.Indexer(O.Indexer2(smpl, O.ReportLabel(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%s", null, null, false, EVariableType.Var), O.Lookup(smpl, null, (O.scalarStringPercent).Add(smpl, (O.Add(smpl, O.Lookup(smpl, null, (O.scalarStringPercent).Add(smpl, (O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"", true, false)), O.Lookup(smpl, null, null, "%s3", null, null, false, EVariableType.Var)))), null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"", true, false)))).Add(smpl, new ScalarString("2"))), null, false, EVariableType.Var)), new ScalarString(ScalarString.SubstituteScalarsInString(@"z", true, false)))
            , "%¨s+%¨{%¨{''+%¨s3}+''}¨2+'z'|[@65,119:119='%',<1166>,2:56]|[@87,144:146=''z'',<1154>,2:81]")), smpl, O.Lookup(smpl, null, (O.ReportLabel(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%s9", null, null, false, EVariableType.Var), O.Lookup(smpl, null, null, "%s9", null, null, false, EVariableType.Var)), "%¨s9+%¨s9|[@56,106:106='%',<1166>,2:43]|[@62,113:114='s9',<1198>,2:50]")), null, false, EVariableType.Var), O.ReportLabel(smpl, O.ReportLabel(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%s", null, null, false, EVariableType.Var), O.Lookup(smpl, null, (O.scalarStringPercent).Add(smpl, (O.Add(smpl, O.Lookup(smpl, null, (O.scalarStringPercent).Add(smpl, (O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"", true, false)), O.Lookup(smpl, null, null, "%s3", null, null, false, EVariableType.Var)))), null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"", true, false)))).Add(smpl, new ScalarString("2"))), null, false, EVariableType.Var)), new ScalarString(ScalarString.SubstituteScalarsInString(@"z", true, false)))
            , "%¨s+%¨{%¨{''+%¨s3}+''}¨2+'z'|[@65,119:119='%',<1166>,2:56]|[@87,144:146=''z'',<1154>,2:81]"), "%¨s+%¨{%¨{''+%¨s3}+''}¨2+'z'|[@65,119:119='%',<1166>,2:56]|[@87,144:146=''z'',<1154>,2:81]"));
                        O.PrtElementHandleLabel(smpl, ope2);
                    }
                    smpl.bankNumber = 0;
                    o2.prtElements.Add(ope2);
                }

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope2 = new O.Prt.Element();
                    ope2.labelGiven = new List<string>() { "f¨({%¨s9+%¨s9}, 'a3z')|[@91,150:150='f',<1198>,2:87]|[@106,171:171=')',<1152>,2:108]" };
                    smpl = new GekkoSmpl(o2.t1.Add(-2), o2.t2);
                    ope2.printCodesFinal = Program.GetElementPrintCodes(o2, ope2); bankNumbers = O.Prt.GetBankNumbers(null, ope2.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope2.variable[bankNumber] = O.FunctionLookup2("f")(smpl, p, O.Lookup(smpl, null, (O.ReportLabel(smpl, O.Add(smpl, O.Lookup(smpl, null, null, "%s9", null, null, false, EVariableType.Var), O.Lookup(smpl, null, null, "%s9", null, null, false, EVariableType.Var)), "%¨s9+%¨s9|[@95,154:154='%',<1166>,2:91]|[@101,161:162='s9',<1198>,2:98]")), null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"a3z", true, false)));
                        O.PrtElementHandleLabel(smpl, ope2);
                    }
                    smpl.bankNumber = 0;
                    o2.prtElements.Add(ope2);
                }


                o2.counter = 7;
                o2.printCsCounter = Globals.printCs.Count - 1;
                o2.Exe();
                return o2.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print2);
            print2(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
