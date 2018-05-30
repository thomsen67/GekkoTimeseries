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
        public static readonly ScalarVal i99 = new ScalarVal(1d);
        public static void FunctionDef100()
        {


            //[[splitSTOP]]

            O.PrepareUfunction(0, "f");

            Globals.ufunctions0.Add("f", (GekkoSmpl smpl, P p) =>
            {
                p.SetText(@"¤6"); O.InitSmpl(smpl, p);


                //[[splitSTOP]]
                return i99;

                //[[splitSTART]]

                ; return null;
            });


            //[[splitSTART]]

        }

        public static readonly ScalarVal i101 = new ScalarVal(2001d);
        public static readonly ScalarVal i102 = new ScalarVal(2003d);
        public static readonly ScalarVal i106 = new ScalarVal(1d);
        public static readonly ScalarVal i108 = new ScalarVal(2d);
        public static readonly ScalarVal i110 = new ScalarVal(3d);
        public static readonly ScalarVal i112 = new ScalarVal(1d);
        public static readonly ScalarVal i114 = new ScalarVal(2d);
        public static readonly ScalarVal i116 = new ScalarVal(3d);
        public static readonly ScalarVal i118 = new ScalarVal(1d);
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

            FunctionDef100();


            p.SetText(@"¤9"); O.InitSmpl(smpl, p);

            O.Time o3 = new O.Time();
            o3.t1 = O.ConvertToDate(i101, O.GetDateChoices.FlexibleStart);
            ;
            o3.t2 = O.ConvertToDate(i102, O.GetDateChoices.FlexibleEnd);
            ;

            o3.Exe();

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar103 = O.IvConvertTo(EVariableType.Var, O.CreateListFromStrings(new string[] { "a1", "a2" }));
            O.Lookup(smpl, null, null, "#a", null, ivTmpvar103, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar104 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"a3", true, false)));
            O.Lookup(smpl, null, null, "%s", null, ivTmpvar104, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar105 = O.IvConvertTo(EVariableType.Var, i106);
            O.Lookup(smpl, null, null, "a1", null, ivTmpvar105, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar107 = O.IvConvertTo(EVariableType.Var, i108);
            O.Lookup(smpl, null, null, "a2", null, ivTmpvar107, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar109 = O.IvConvertTo(EVariableType.Var, i110);
            O.Lookup(smpl, null, null, "a3", null, ivTmpvar109, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar111 = O.IvConvertTo(EVariableType.Var, i112);
            O.Lookup(smpl, null, null, "xa1", null, ivTmpvar111, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar113 = O.IvConvertTo(EVariableType.Var, i114);
            O.Lookup(smpl, null, null, "xa2", null, ivTmpvar113, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar115 = O.IvConvertTo(EVariableType.Var, i116);
            O.Lookup(smpl, null, null, "xa3", null, ivTmpvar115, true, EVariableType.Var)
            ;

            p.SetText(@"¤19"); O.InitSmpl(smpl, p);
            Func<IVariable> func120 = () =>
            {
                var smplCommandRemember121 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp119 = new List();

                foreach (IVariable listloop_a117 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, false, EVariableType.Var)))
                {
                    O.ClearLabelHelper(smpl);
                    temp119.Add(O.Add(smpl, O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, (O.ReportInterior(smpl, listloop_a117, 0, labelCounter)), null, false, EVariableType.Var), Functions.dif(O.Smpl(smpl, -1), smpl, O.Lookup(smpl, null, (O.ReportInterior(smpl, listloop_a117, 0, labelCounter)), null, false, EVariableType.Var))), O.FunctionLookup0("f")(smpl, p)), i118));

                    O.AddLabelHelper(smpl);
                }
                smpl.command = smplCommandRemember121;
                return temp119;

            };


            Func<GraphHelper, string> print12 = (gh) =>
            {
                O.Prt o12 = new O.Prt();
                int labelCounter = 0; o12.guiGraphIsRefreshing = gh.isRefreshing;
                o12.guiGraphPrintCode = gh.printCode;
                o12.guiGraphIsLogTransform = gh.isLogTransform;
                o12.prtType = "p";

                o12.t1 = Globals.globalPeriodStart;
                o12.t2 = Globals.globalPeriodEnd;

                o12.printCodes.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));



                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope12 = new O.Prt.Element();
                    ope12.label = O.SubstituteScalarsAndLists("|||a|||{#a} + dif({#a}) + f()  + 1", false);
                    smpl = new GekkoSmpl(o12.t1.Add(-2), o12.t2);
                    ope12.printCodesFinal = Program.GetElementPrintCodes(o12, ope12); bankNumbers = O.Prt.GetBankNumbers(null, ope12.printCodesFinal); foreach (int bankNumber in bankNumbers)
                    {
                        smpl.bankNumber = bankNumber;
                        ope12.variable[bankNumber] = func120();
                    }
                    smpl.bankNumber = 0;
                    o12.prtElements.Add(ope12);
                }


                o12.counter = 5;
                o12.printCsCounter = Globals.printCs.Count - 1;
                o12.labelHelper2 = O.AddLabelHelper2(smpl);
                o12.Exe();
                return o12.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print12);
            print12(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
