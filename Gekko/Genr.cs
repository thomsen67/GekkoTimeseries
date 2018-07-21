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
        public static readonly ScalarVal i28 = new ScalarVal(1d);
        public static readonly ScalarVal i30 = new ScalarVal(4d);
        public static readonly ScalarVal i32 = new ScalarVal(1d);
        public static readonly ScalarVal i34 = new ScalarVal(4d);
        public static readonly ScalarVal i36 = new ScalarVal(5d);
        public static readonly ScalarVal i38 = new ScalarVal(7d);
        public static readonly ScalarVal i40 = new ScalarVal(8d);
        public static readonly ScalarVal i42 = new ScalarVal(100d);
        public static readonly ScalarVal i44 = new ScalarVal(200d);
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

            IVariable ivTmpvar25 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), -1);
            O.Lookup(smpl, null, null, "%a", null, ivTmpvar25, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar26 = O.TypeCheck_var(O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"d", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"e", true, false))), -1);
            O.Lookup(smpl, null, null, "#d", null, ivTmpvar26, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar27 = O.TypeCheck_var(Functions.series(smpl, i28), -1);
            O.Lookup(smpl, null, null, "xab", null, ivTmpvar27, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar29 = O.TypeCheck_var(i30, -1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xab", null, null, true, EVariableType.Var), ivTmpvar29, new ScalarString("cd"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar31 = O.TypeCheck_var(Functions.series(smpl, i32), -1);
            O.Lookup(smpl, null, null, "xb", null, ivTmpvar31, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar33 = O.TypeCheck_var(i34, -1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xb", null, null, true, EVariableType.Var), ivTmpvar33, new ScalarString("d"))
            ;

            p.SetText(@"¤7"); O.InitSmpl(smpl, p);

            O.Clone o6 = new O.Clone();
            o6.Exe();

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar35 = O.TypeCheck_var(i36, -1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xab", null, null, true, EVariableType.Var), ivTmpvar35, new ScalarString("cd"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar37 = O.TypeCheck_var(i38, -1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xb", null, null, true, EVariableType.Var), ivTmpvar37, new ScalarString("d"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar39 = O.TypeCheck_var(i40, -1);
            O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "xb", null, null, true, EVariableType.Var), ivTmpvar39, new ScalarString("e"))
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar41 = O.TypeCheck_var(i42, -1);
            O.Lookup(smpl, null, null, "d", null, ivTmpvar41, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar43 = O.TypeCheck_var(i44, -1);
            O.Lookup(smpl, null, null, "e", null, ivTmpvar43, true, EVariableType.Var)
            ;

            p.SetText(@"¤17"); O.InitSmpl(smpl, p);
            Func<IVariable> func47 = () =>
            {
                var smplCommandRemember48 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp46 = new List();

                foreach (IVariable listloop_d45 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("d")))), null, false, EVariableType.Var)))
                {
                    O.ClearLabelHelper(smpl);
                    temp46.Add(O.Lookup(smpl, null, (O.ReportLabel(smpl, listloop_d45, "#¨d|[@105,288:288='#',<1159>,17:3]|[@107,290:290='d',<798>,17:5]")), null, false, EVariableType.Var));

                    O.AddLabelHelper(smpl);
                }
                smpl.command = smplCommandRemember48;
                return temp46;

            };


            Func<GraphHelper, string> print12 = (gh) =>
            {
                O.Prt o12 = new O.Prt();
                labelCounter = 0; o12.guiGraphIsRefreshing = gh.isRefreshing;
                o12.guiGraphPrintCode = gh.printCode;
                o12.guiGraphIsLogTransform = gh.isLogTransform;
                o12.prtType = "p";

                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope12 = new O.Prt.Element();
                    //ope12.label22 = "|||d|||{#¨d}|[@104,287:287='{',<1190>,17:2]|[@108,291:291='}',<1165>,17:6]";
                    ope12.label = "|||d|||[<{THIS IS A LABEL}>][@104,287:287='{',<1190>,17:2]";
                    smpl = new GekkoSmpl(o12.t1.Add(-2), o12.t2);
                    ope12.printCodesFinal = Program.GetElementPrintCodes(o12, ope12); bankNumbers = O.Prt.GetBankNumbers(null, ope12.printCodesFinal);
                    for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                    {
                        int bankNumber = bankNumbers[bankNumberI];
                        smpl.bankNumber = bankNumber;
                        ope12.variable[bankNumber] = func47();
                        O.PrtElementHandleLabel(smpl, ope12);
                    }
                    smpl.bankNumber = 0;
                    o12.prtElements.Add(ope12);
                }


                o12.counter = 3;
                o12.printCsCounter = Globals.printCs.Count - 1;
                o12.labelHelper2 = O.AddLabelHelper2(smpl);
                o12.labelHelper22 = O.AddLabelHelper22(smpl);
                o12.Exe();
                return o12.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print12);
            print12(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
