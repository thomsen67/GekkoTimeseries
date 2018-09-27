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
        public static readonly ScalarVal i179 = new ScalarVal(1d);
        public static readonly ScalarVal i180 = new ScalarVal(0d);
        public static void FunctionDef183()
        {


            //[[splitSTOP]]

            O.PrepareUfunction(2, "plus");

            Globals.ufunctions2.Add("plus", (GekkoSmpl smpl, P p, IVariable functionarg_176, IVariable functionarg_177) =>

            {
                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal();
                try
                {
                    functionarg_176 = O.TypeCheck_list(functionarg_176, 1);
                    functionarg_177 = O.TypeCheck_val(functionarg_177, 2);


        //[[splitSTOP]]
        IVariable forloop_178 = null;
                    int counter182 = 0;
                    for (O.IterateStart(ref forloop_178, i179); O.IterateContinue(forloop_178, i179, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, i180
        ), smpl, O.EIndexerType.None, functionarg_176, i180
        ), null, ref counter182); O.IterateStep(forloop_178, i179, null, counter182))
                    {
                        ;
                        p.SetText(@"¤0"); O.InitSmpl(smpl, p);

                        O.Assignment o2 = new O.Assignment();
                        O.AdjustT0(smpl, -1);
                        IVariable ivTmpvar181 = O.Add(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, forloop_178
            ), smpl, O.EIndexerType.None, functionarg_176, forloop_178
            ), functionarg_177);
                        O.AdjustT0(smpl, 1);
                        O.IndexerSetData(smpl, functionarg_176, ivTmpvar181, o2, forloop_178
            )
            ;

                    };

        //[[splitSTART]]


        return null;
                }
                finally
                {
                    Program.databanks.local = local0; Program.databanks.localGlobal = lg0;
                }
            });


            //[[splitSTART]]

        }

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
            FunctionDef183();



            //[[splitSTOP]]


        }
    }
}
