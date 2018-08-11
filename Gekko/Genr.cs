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
        public static readonly ScalarVal i106 = new ScalarVal(2001d);
        public static readonly ScalarVal i107 = new ScalarVal(2003d);
        public static readonly ScalarVal i111 = new ScalarVal(3d);
        public static readonly ScalarVal i115 = new ScalarVal(1d);
        public static readonly ScalarVal i117 = new ScalarVal(1d);
        public static readonly ScalarVal i119 = new ScalarVal(2d);
        public static readonly ScalarVal i121 = new ScalarVal(3d);
        public static readonly ScalarVal i124 = new ScalarVal(9d);
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

            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe(smpl);

            p.SetText(@"¤3"); O.InitSmpl(smpl, p);

            O.Time o1 = new O.Time();
            o1.t1 = O.ConvertToDate(i106, O.GetDateChoices.FlexibleStart);
            ;
            o1.t2 = O.ConvertToDate(i107, O.GetDateChoices.FlexibleEnd);
            ;

            o1.Exe();

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar108 = O.TypeCheck_var(new List(O.ExplodeIvariables(new List(new List<IVariable> { new ScalarString("a1"), new ScalarString("a2") }))), -1);
            O.Lookup(smpl, null, null, "#a", null, ivTmpvar108, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar109 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), -1);
            O.Lookup(smpl, null, null, "%s", null, ivTmpvar109, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar110 = O.TypeCheck_var(i111, -1);
            O.Lookup(smpl, null, null, "%i2", null, ivTmpvar110, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar112 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"i", true, false)), -1);
            O.Lookup(smpl, null, null, "%s2", null, ivTmpvar112, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar113 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"s2", true, false)), -1);
            O.Lookup(smpl, null, null, "%s3", null, ivTmpvar113, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar114 = O.TypeCheck_var(Functions.series(smpl, i115), -1);
            O.Lookup(smpl, null, null, "xx", null, ivTmpvar114, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar116 = O.TypeCheck_var(i117, -1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var), ivTmpvar116, new ScalarString("a1z"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar118 = O.TypeCheck_var(i119, -1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var), ivTmpvar118, new ScalarString("a2z"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar120 = O.TypeCheck_var(i121, -1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xx", null, null, true, EVariableType.Var), ivTmpvar120, new ScalarString("a3z"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar122 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"x", true, false)), -1);
            O.Lookup(smpl, null, null, "%s9", null, ivTmpvar122, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar123 = O.TypeCheck_var(i124, -1);
            O.Lookup(smpl, null, null, "xy", null, ivTmpvar123, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar125 = O.TypeCheck_list(O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"xx", true, false))), -1);
            O.Lookup(smpl, null, null, "m", null, ivTmpvar125, true, EVariableType.List)
            ;

            p.SetText(@"¤17"); O.InitSmpl(smpl, p);
            Func<IVariable> func128 = () =>
            {
                var smplCommandRemember129 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp127 = new List();

                foreach (IVariable listloop_m126 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m")))), null, false, EVariableType.Var)))
                {
                    temp127.Add(O.Lookup(smpl, null, (O.ReportLabel(smpl, listloop_m126, "#¨m|[@129,335:335='#',<1159>,17:3]|[@131,337:337='m',<919>,17:5]")), null, false, EVariableType.Var));

                }
                smpl.command = smplCommandRemember129;
                return temp127;

            };


            Func<GraphHelper, string> print14 = (gh) =>
            {
                O.Prt o14 = new O.Prt();
                labelCounter = 0; o14.guiGraphIsRefreshing = gh.isRefreshing;
                o14.guiGraphPrintCode = gh.printCode;
                o14.guiGraphIsLogTransform = gh.isLogTransform;
                o14.prtType = "p";

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope14 = new O.Prt.Element();
                    ope14.labelGiven = new List<string>() { "|||m|||{#¨m}|[@128,334:334='{',<1190>,17:2]|[@132,338:338='}',<1165>,17:6]" };
                    smpl = new GekkoSmpl(o14.t1.Add(-2), o14.t2);
                    ope14.printCodesFinal = Program.GetElementPrintCodes(o14, ope14); bankNumbers = O.Prt.GetBankNumbers(null, ope14.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope14.variable[bankNumber] = func128();
                        O.PrtElementHandleLabel(smpl, ope14);
                    }
                    smpl.bankNumber = 0;
                    o14.prtElements.Add(ope14);
                }


                o14.counter = 8;
                o14.printCsCounter = Globals.printCs.Count - 1;
                o14.Exe();
                return o14.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print14);
            print14(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
