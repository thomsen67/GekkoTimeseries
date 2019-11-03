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
        public static void C0(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]0
            p.SetText(@"¤3"); O.InitSmpl(smpl, p);


            O.Decomp2 o0 = new O.Decomp2();
            o0.type = @"ASTDECOMP2";
            o0.label = @"qrs[#a] in qrs";
            o0.t1 = Globals.globalPeriodStart;
            o0.t2 = Globals.globalPeriodEnd;

            o0.opt_prtcode = O.ConvertToString((new ScalarString("d")));



            o0.decompItems.Add(new DecompItems(null, O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("qrs").Concat(null, new ScalarString("[").Concat(null, O.Lookup(smpl, null, null, "#a", null, null, new LookupSettings(), EVariableType.Var, null)).Concat(null, new ScalarString("]"))) })), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("qrs") }))));

            o0.decompItems.Add(new DecompItems(null, O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("qrss").Concat(null, new ScalarString("[").Concat(null, O.Lookup(smpl, null, null, "#a", null, null, new LookupSettings(), EVariableType.Var, null)).Concat(null, new ScalarString("]"))) })), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("qrss") }))));


            o0.decompItems.Add(new DecompItems(null, O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("qrsm").Concat(null, new ScalarString("[").Concat(null, O.Lookup(smpl, null, null, "#a", null, null, new LookupSettings(), EVariableType.Var, null)).Concat(null, new ScalarString("]"))) })), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("qrsm") }))));



            var Evalcode41 = new List<Func<GekkoSmpl, IVariable>>();
            var xxx = new List<TwoStrings>();
            foreach (IVariable listloop_a40 in new O.GekkoListIterator(O.DecompLooper("#a")))
            {

                Evalcode41.Add((smpl42) =>
                {
                    return O.Subtract(smpl42, O.Indexer(O.Indexer2(smpl42, O.EIndexerType.None, listloop_a40), smpl42, O.EIndexerType.None, O.Lookup(smpl42, null, null, "Qrs", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a40), O.Add(smpl42, O.Indexer(O.Indexer2(smpl42, O.EIndexerType.None, listloop_a40), smpl42, O.EIndexerType.None, O.Lookup(smpl42, null, null, "Qrss", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a40), O.Indexer(O.Indexer2(smpl42, O.EIndexerType.None, listloop_a40), smpl42, O.EIndexerType.None, O.Lookup(smpl42, null, null, "Qrsm", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a40)));
                }
                );





                o0.Exe();

                //[[commandEnd]]0
            }
        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
