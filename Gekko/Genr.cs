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
        public static readonly ScalarVal i80 = new ScalarVal(1d);
        public static readonly ScalarVal i81 = new ScalarVal(2d);
        public static readonly ScalarVal d82 = new ScalarVal(1.0d);
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

            //[[splitSTART]]
            p.SetText(@"¤1"); O.InitSmpl(smpl);
            O.Table.SetValues o0 = new O.Table.SetValues();
            o0.name = O.ConvertToString((new ScalarString("tab", true, false)));
            o0.col = O.ConvertToInt(O.Lookup(smpl, null, null, "%__c2", null, null, false, EVariableType.Var));
            o0.t1 = O.ConvertToDate(O.Lookup(smpl, null, null, "%__t1", null, null, false, EVariableType.Var), O.GetDateChoices.Strict);
            o0.t2 = O.ConvertToDate(O.Lookup(smpl, null, null, "%__t2", null, null, false, EVariableType.Var), O.GetDateChoices.Strict);
            o0.printcode = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"n", true, false)));
            o0.scale = O.ConvertToVal(d82);
            o0.format = O.ConvertToString(new ScalarString(ScalarString.SubstituteScalarsInString(@"f15.2", true, false)));
            try
            {
                O.isTableCall = true;
                {
                    List<int> bankNumbers = O.Prt.CreateBankHelper(1);
                    O.Prt.Element ope0 = new O.Prt.Element();
                    ope0.label = O.SubstituteScalarsAndLists("", false);
                    smpl = new GekkoSmpl(o0.t1.Add(-2), o0.t2);
                    bankNumbers = O.Prt.GetBankNumbers(Globals.tableOption, new List<string>() { o0.printcode });
                    foreach (int bankNumber in bankNumbers)
                    {
                        ope0.subElements = new List<O.Prt.SubElement>();
                        ope0.subElements.Add(new O.Prt.SubElement());
                        ope0.subElements[0].tsWork = O.Multiply(smpl, O.Indexer(O.Indexer2(smpl, O.Negate(smpl, i80)
                        ), smpl, O.Indexer(O.Indexer2(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
                        ), smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var), new ScalarString(ScalarString.SubstituteScalarsInString(@"a", true, false))
                        ), O.Negate(smpl, i80)
                        ), i81);
                    }
                    o0.prtElements.Add(ope0);
                }
                smpl = null;
            }
            finally
            {
                O.isTableCall = false;
            }
            o0.Exe();


            //[[splitSTOP]]


        }
    }
}
