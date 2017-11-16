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
        public static IVariable temp60(GekkoSmpl smpl)
        {
            List temp60 = new List();

            foreach (IVariable listloop_xx59 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("xx")))), null, false, EVariableType.Var)))
            {
                temp60.Add(O.Indexer(O.Indexer2(smpl, listloop_xx59), smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var), listloop_xx59));

            }
            return temp60;

        }
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);
            if (true)
            {
                Func<IVariable> func1 = () =>
                {
                    List temp60 = new List();
                    foreach (IVariable listloop_xx59 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("xx")))), null, false, EVariableType.Var)))
                    {
                        temp60.Add(O.Indexer(O.Indexer2(smpl, listloop_xx59), smpl, O.Lookup(smpl, null, null, "x", null, null, false, EVariableType.Var), listloop_xx59));
                    }
                    return temp60;
                };
                p.SetText(@"¤0"); O.InitSmpl(smpl);
                IVariable ivTmpvar58 = O.IvConvertTo(EVariableType.Var, func1());
                O.Lookup(smpl, null, null, "xx", null, ivTmpvar58, true, EVariableType.Var)
                ;
            }
            else
            {

                //[[splitSTART]]
                p.SetText(@"¤0"); O.InitSmpl(smpl);
                IVariable ivTmpvar58 = O.IvConvertTo(EVariableType.Var, temp60(smpl));
                O.Lookup(smpl, null, null, "xx", null, ivTmpvar58, true, EVariableType.Var)
                ;
            }

            //[[splitSTOP]]


        }
    }
}