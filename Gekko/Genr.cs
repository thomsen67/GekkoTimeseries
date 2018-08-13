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
        public static void ClearTS(P p) {
        }
        public static void ClearScalar(P p) {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[splitSTART]]
            p.SetText(@"¤0"); O.InitSmpl(smpl, p);
            Func<IVariable> func55 = () => {
                var smplCommandRemember56 = smpl.command; smpl.command = GekkoSmplCommand.Unfold;
                List temp54 = new List();

                foreach (IVariable listloop_s53 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("s")))), null, false, EVariableType.Var))) {
                    temp54.Add(O.Indexer(O.Indexer2(smpl, listloop_s53), smpl, O.Lookup(smpl, null, (O.ReportLabel(smpl, null, "%¨var_name|[@19,50:50='%',<1166>,1:50]|[@21,52:59='var_name',<1198>,1:52]")), null, false, EVariableType.Var), O.ReportLabel(smpl, listloop_s53, "#¨s|[@24,64:64='#',<1159>,1:64]|[@26,66:66='s',<1198>,1:66]")));

                }
                smpl.command = smplCommandRemember56;
                return temp54;

            };



            


    //[[splitSTART]]


    //[[splitSTOP]]


}
        }
    }

