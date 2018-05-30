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
        public static readonly ScalarVal i1 = new ScalarVal(2001d);
        public static readonly ScalarVal i2 = new ScalarVal(2003d);
        public static readonly ScalarVal i6 = new ScalarVal(1d);
        public static readonly ScalarVal i8 = new ScalarVal(2d);
        public static readonly ScalarVal i10 = new ScalarVal(3d);
        public static readonly ScalarVal i12 = new ScalarVal(1d);
        public static readonly ScalarVal i14 = new ScalarVal(2d);
        public static readonly ScalarVal i16 = new ScalarVal(3d);
        public static readonly ScalarVal i18 = new ScalarVal(2d);
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

            p.SetText(@"¤2"); O.InitSmpl(smpl, p);

            O.Time o1 = new O.Time();
            o1.t1 = O.ConvertToDate(i1, O.GetDateChoices.FlexibleStart);
            ;
            o1.t2 = O.ConvertToDate(i2, O.GetDateChoices.FlexibleEnd);
            ;

            o1.Exe();

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar3 = O.IvConvertTo(EVariableType.Var, O.CreateListFromStrings(new string[] { "a1", "a2" }));
            O.Lookup(smpl, null, null, "#a", null, ivTmpvar3, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar4 = O.IvConvertTo(EVariableType.Var, new ScalarString(ScalarString.SubstituteScalarsInString(@"a3", true, false)));
            O.Lookup(smpl, null, null, "%s", null, ivTmpvar4, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar5 = O.IvConvertTo(EVariableType.Var, i6);
            O.Lookup(smpl, null, null, "a1", null, ivTmpvar5, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar7 = O.IvConvertTo(EVariableType.Var, i8);
            O.Lookup(smpl, null, null, "a2", null, ivTmpvar7, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar9 = O.IvConvertTo(EVariableType.Var, i10);
            O.Lookup(smpl, null, null, "a3", null, ivTmpvar9, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar11 = O.IvConvertTo(EVariableType.Var, i12);
            O.Lookup(smpl, null, null, "xa1", null, ivTmpvar11, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar13 = O.IvConvertTo(EVariableType.Var, i14);
            O.Lookup(smpl, null, null, "xa2", null, ivTmpvar13, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar15 = O.IvConvertTo(EVariableType.Var, i16);
            O.Lookup(smpl, null, null, "xa3", null, ivTmpvar15, true, EVariableType.Var)
            ;

            p.SetText(@"¤12"); O.InitSmpl(smpl, p);
            Func<IVariable> func20 = () =>
            {
                var smplCommandRemember21 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp19 = new List();

                foreach (IVariable listloop_a17 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, false, EVariableType.Var)))
                {
                    O.ClearLabelHelper(smpl);
                    temp19.Add(O.Add(smpl, O.Add(smpl, O.Lookup(smpl, null, (O.ReportInterior(smpl, listloop_a17, 0, labelCounter)), null, false, EVariableType.Var), Functions.dif(O.Smpl(smpl, -1), smpl, O.Lookup(smpl, null, (O.ReportInterior(smpl, listloop_a17, 0, labelCounter)), null, false, EVariableType.Var))), i18));

                    O.AddLabelHelper(smpl);
                }
                smpl.command = smplCommandRemember21;
                return temp19;

            };


            Func<GraphHelper, string> print10 = (gh) =>
            {
                O.Prt o10 = new O.Prt();
                int labelCounter = 0; o10.guiGraphIsRefreshing = gh.isRefreshing;
                o10.guiGraphPrintCode = gh.printCode;
                o10.guiGraphIsLogTransform = gh.isLogTransform;
                o10.prtType = "p";

                o10.t1 = Globals.globalPeriodStart;
                o10.t2 = Globals.globalPeriodEnd;

                o10.printCodes.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));



                {
                    List<int> bankNumbers = null;
                    O.Prt.Element ope10 = new O.Prt.Element();
                    ope10.label = O.SubstituteScalarsAndLists("|||a|||{#a}+dif({#a})+2", false);
                    smpl = new GekkoSmpl(o10.t1.Add(-2), o10.t2);
                    ope10.printCodesFinal = Program.GetElementPrintCodes(o10, ope10); bankNumbers = O.Prt.GetBankNumbers(null, ope10.printCodesFinal); foreach (int bankNumber in bankNumbers)
                    {
                        smpl.bankNumber = bankNumber;
                        ope10.variable[bankNumber] = func20();
                    }
                    smpl.bankNumber = 0;
                    o10.prtElements.Add(ope10);
                }


                o10.counter = 1;
                o10.printCsCounter = Globals.printCs.Count - 1;
                o10.labelHelper2 = O.AddLabelHelper2(smpl);
                o10.Exe();
                return o10.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print10);
            print10(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
