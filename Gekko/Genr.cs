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

            p.SetText(@"¤8"); O.InitSmpl(smpl, p);
            Func<IVariable> func13 = () =>
            {
                var smplCommandRemember14 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp12 = new List();

                foreach (IVariable listloop_a11 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("a")))), null, false, EVariableType.Var)))
                {
                    O.ClearLabelHelper(smpl);
                    temp12.Add(O.Lookup(smpl, null, (O.ReportInterior(smpl, listloop_a11, 0, labelCounter)), null, false, EVariableType.Var));

                    O.AddLabelHelper(smpl);
                }
                smpl.command = smplCommandRemember14;
                return temp12;

            };


            Func<GraphHelper, string> print7 = (gh) =>
            {
                O.Prt o7 = new O.Prt();
                int labelCounter = 0; o7.guiGraphIsRefreshing = gh.isRefreshing;
                o7.guiGraphPrintCode = gh.printCode;
                o7.guiGraphIsLogTransform = gh.isLogTransform;
                o7.prtType = "p";

                o7.t1 = Globals.globalPeriodStart;
                o7.t2 = Globals.globalPeriodEnd;

                o7.printCodes.Add(new OptString("n", O.ConvertToString(new ScalarString("yes"))));



    {
    List<int> bankNumbers = null;
    O.Prt.Element ope7 = new O.Prt.Element();
    ope7.label = "|||a|||{#a}";
    smpl = new GekkoSmpl(o7.t1.Add(-2), o7.t2);
    ope7.printCodesFinal = Program.GetElementPrintCodes(o7, ope7);bankNumbers = O.Prt.GetBankNumbers(null, ope7.printCodesFinal);
    for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {
                int bankNumber = bankNumbers[bankNumberI];
                smpl.bankNumber = bankNumber;
                ope7.variable[bankNumber] = func13();
                if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope7);
            }
    smpl.bankNumber = 0;
    o7.prtElements.Add(ope7);
    }

    //{
    //List<int> bankNumbers = null;
    //O.Prt.Element ope7 = new O.Prt.Element();
    //ope7.label = "{%s}";
    //smpl = new GekkoSmpl(o7.t1.Add(-2), o7.t2);
    //ope7.printCodesFinal = Program.GetElementPrintCodes(o7, ope7);bankNumbers = O.Prt.GetBankNumbers(null, ope7.printCodesFinal);
    //for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++; ) {
    //int bankNumber = bankNumbers[bankNumberI];
    //smpl.bankNumber = bankNumber;
    //ope7.variable[bankNumber] = O.Lookup(smpl, null, (O.ReportInterior(smpl, O.Lookup(smpl, null, null, "%s", null, null, false, EVariableType.Var), 0, labelCounter)), null, false, EVariableType.Var);
    //if(bankNumberI == 0) O.PrtElementHandleLabel(smpl, ope7);}
    //smpl.bankNumber = 0;
    //o7.prtElements.Add(ope7);
    //}


    o7.counter = 1;
                o7.printCsCounter = Globals.printCs.Count - 1;
                o7.labelHelper2 = O.AddLabelHelper2(smpl);
                o7.Exe();
                return o7.emfName;
            };
            Globals.printCs.Add(Globals.printCs.Count, print7);
            print7(new GraphHelper());


            //[[splitSTOP]]


        }
    }
}
