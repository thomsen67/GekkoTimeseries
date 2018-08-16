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

            O.Mode o1 = new O.Mode();
            o1.mode = @"data"; o1.Exe();

            p.SetText(@"¤3"); O.InitSmpl(smpl, p);

            IVariable ivTmpvar1 = O.TypeCheck_var(new ScalarString(ScalarString.SubstituteScalarsInString(@"t1", true, false)), -1);
            O.Lookup(smpl, null, "local", "%s", null, ivTmpvar1, true, EVariableType.Var)
            ;

            p.SetText(@"¤4"); O.InitSmpl(smpl, p);

            Program.Tell(O.ConvertToString(O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"t1--> ", true, false)), O.Lookup(smpl, null, null, "%s", null, null, false, EVariableType.Var))), false);
            p.SetText(@"¤5"); O.InitSmpl(smpl, p);

            Databank local = Program.databanks.local;
            Program.databanks.local = new Databank("Local");

            try
            {
                O.Run o4 = new O.Run();
                o4.fileName = O.ConvertToString((new ScalarString("t2")));
                o4.p = p;
                o4.Exe();
            }
            finally
            {
                Program.databanks.local = local;
            }

            

            p.SetText(@"¤6"); O.InitSmpl(smpl, p);

            Program.Tell(O.ConvertToString(O.Add(smpl, new ScalarString(ScalarString.SubstituteScalarsInString(@"t1--> ", true, false)), O.Lookup(smpl, null, null, "%s", null, null, false, EVariableType.Var))), false);

            //[[splitSTOP]]


        }
    }
}
