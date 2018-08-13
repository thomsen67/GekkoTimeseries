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
        public static readonly ScalarVal i38 = new ScalarVal(1d);
        public static readonly ScalarVal i40 = new ScalarVal(1d);
        public static readonly ScalarVal i43 = new ScalarVal(1d);
        public static readonly ScalarVal i45 = new ScalarVal(1d);
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

            IVariable ivTmpvar37 = O.TypeCheck_var(Functions.series(smpl, i38), -1);
            O.Lookup(smpl, null, null, "a", null, ivTmpvar37, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar39 = O.TypeCheck_var(Functions.series(smpl, i40), -1);
            O.Lookup(smpl, null, null, "b", null, ivTmpvar39, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar41 = O.TypeCheck_var(O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"s1", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"s2", true, false))), -1);
            O.Lookup(smpl, null, null, "#s", null, ivTmpvar41, true, EVariableType.Var)
            ;

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            foreach (IVariable listloop_s35 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, false, EVariableType.Var)))
            {
                IVariable ivTmpvar42 = O.TypeCheck_var(i43, -1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "a", null, null, true, EVariableType.Var), ivTmpvar42, listloop_s35)
                ;
            }

            p.SetText(@"¤0"); O.InitSmpl(smpl, p);

            foreach (IVariable listloop_s36 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, false, EVariableType.Var)))
            {
                IVariable ivTmpvar44 = O.TypeCheck_var(i45, -1);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "b", null, null, true, EVariableType.Var), ivTmpvar44, listloop_s36)
                ;
            }

            //p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            //Func<IVariable> func49 = () =>
            //{
            //    var smplCommandRemember50 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
            //    List temp48 = new List();

            //    foreach (IVariable listloop_s47 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, false, EVariableType.Var)))
            //    {
            //        temp48.Add(O.Indexer(O.Indexer2(smpl, listloop_s47), smpl, O.Lookup(smpl, null, (O.ReportLabel(smpl, forloop_46, "%¨var_name|[@79,134:134='%',<1166>,13:7]|[@81,136:143='var_name',<1198>,13:9]")), null, false, EVariableType.Var), O.ReportLabel(smpl, listloop_s47, "#¨s|[@84,148:148='#',<1159>,13:21]|[@86,150:150='s',<1198>,13:23]")));

            //    }
            //    smpl.command = smplCommandRemember50;
            //    return temp48;

            //};



            //[[splitSTOP]]
            IVariable forloop_46 = null;
            int counter51 = 0;
            for (O.IterateStart(ref forloop_46, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false)))); O.IterateContinue(forloop_46, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))), null, null, ref counter51); O.IterateStep(forloop_46, O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"b", true, false))), null, counter51))
            {
                ;
                p.SetText(@"¤13"); O.InitSmpl(smpl, p);
                Func<IVariable> func49 = () =>
                {
                    var smplCommandRemember50 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                    List temp48 = new List();

                    foreach (IVariable listloop_s47 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, false, EVariableType.Var)))
                    {
                        temp48.Add(O.Indexer(O.Indexer2(smpl, listloop_s47), smpl, O.Lookup(smpl, null, (O.ReportLabel(smpl, forloop_46, "%¨var_name|[@79,134:134='%',<1166>,13:7]|[@81,136:143='var_name',<1198>,13:9]")), null, false, EVariableType.Var), O.ReportLabel(smpl, listloop_s47, "#¨s|[@84,148:148='#',<1159>,13:21]|[@86,150:150='s',<1198>,13:23]")));

                    }
                    smpl.command = smplCommandRemember50;
                    return temp48;

                };


                Func<GraphHelper, string> print6 = (gh) =>
                {
                    O.Prt o6 = new O.Prt();
                    labelCounter = 0; o6.guiGraphIsRefreshing = gh.isRefreshing;
                    o6.guiGraphPrintCode = gh.printCode;
                    o6.guiGraphIsLogTransform = gh.isLogTransform;
                    o6.prtType = "PLOT";

                    {
                        List<int> bankNumbers = null;
                        O.Prt.Element ope6 = new O.Prt.Element();
                        ope6.labelGiven = new List<string>() { "|||s|||{%¨var_name}[_[#¨s]|[@78,133:133='{',<1190>,13:6]|[@87,151:151=']',<1155>,13:24]" };
                        smpl = new GekkoSmpl(o6.t1.Add(-2), o6.t2);
                        ope6.printCodesFinal = Program.GetElementPrintCodes(o6, ope6); bankNumbers = O.Prt.GetBankNumbers(null, ope6.printCodesFinal);
                        for (int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++)
                        {
                            int bankNumber = bankNumbers[bankNumberI];
                            smpl.bankNumber = bankNumber;
                            ope6.variable[bankNumber] = func49();
                            O.PrtElementHandleLabel(smpl, ope6);
                        }
                        smpl.bankNumber = 0;
                        o6.prtElements.Add(ope6);
                    }


                    o6.counter = 2;
                    o6.printCsCounter = Globals.printCs.Count - 1;
                    o6.Exe();
                    return o6.emfName;
                };
                Globals.printCs.Add(Globals.printCs.Count, print6);
                print6(new GraphHelper());

            };

            //[[splitSTART]]


            //[[splitSTOP]]


        }
    }
}
