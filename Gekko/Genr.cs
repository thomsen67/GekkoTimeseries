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
            //[[commandStart]]1
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);

            O.Assignment o1 = new O.Assignment();
            o1.opt_source = @"<[code]>x = sum(#i, xx[a, #i]+xx[a, #i]+xx[a, #i]+xx[a, #i]+xx[a, #i]+xx[a, #i]+xx[a, #i]+xx[a, #i]+xx[a, #i]+xx[a, #i])";

            Func<GekkoSmpl, IVariable> func77 = (GekkoSmpl smpl79) =>
            {
                var smplCommandRemember78 = smpl79.command; smpl79.command = GekkoSmplCommand.Sum;
                Series temp76 = new Series(ESeriesType.Normal, Program.options.freq, null); temp76.SetZero(smpl79);

                foreach (IVariable listloop_i75 in new O.GekkoListIterator(O.Lookup(smpl79, null, ((O.scalarStringHash).Add(smpl79, (new ScalarString("i")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp76.InjectAdd(smpl79, O.Add(smpl79, O.Add(smpl79, O.Add(smpl79, O.Add(smpl79, O.Add(smpl79, O.Add(smpl79, O.Add(smpl79, O.Add(smpl79, O.Add(smpl79, O.Indexer(O.Indexer2(smpl79, O.EIndexerType.None, new ScalarString("a"), listloop_i75), smpl79, O.EIndexerType.None, O.Lookup(smpl79, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a"), listloop_i75), O.Indexer(O.Indexer2(smpl79, O.EIndexerType.None, new ScalarString("a"), listloop_i75), smpl79, O.EIndexerType.None, O.Lookup(smpl79, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a"), listloop_i75)), O.Indexer(O.Indexer2(smpl79, O.EIndexerType.None, new ScalarString("a"), listloop_i75), smpl79, O.EIndexerType.None, O.Lookup(smpl79, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a"), listloop_i75)), O.Indexer(O.Indexer2(smpl79, O.EIndexerType.None, new ScalarString("a"), listloop_i75), smpl79, O.EIndexerType.None, O.Lookup(smpl79, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a"), listloop_i75)), O.Indexer(O.Indexer2(smpl79, O.EIndexerType.None, new ScalarString("a"), listloop_i75), smpl79, O.EIndexerType.None, O.Lookup(smpl79, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a"), listloop_i75)), O.Indexer(O.Indexer2(smpl79, O.EIndexerType.None, new ScalarString("a"), listloop_i75), smpl79, O.EIndexerType.None, O.Lookup(smpl79, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a"), listloop_i75)), O.Indexer(O.Indexer2(smpl79, O.EIndexerType.None, new ScalarString("a"), listloop_i75), smpl79, O.EIndexerType.None, O.Lookup(smpl79, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a"), listloop_i75)), O.Indexer(O.Indexer2(smpl79, O.EIndexerType.None, new ScalarString("a"), listloop_i75), smpl79, O.EIndexerType.None, O.Lookup(smpl79, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a"), listloop_i75)), O.Indexer(O.Indexer2(smpl79, O.EIndexerType.None, new ScalarString("a"), listloop_i75), smpl79, O.EIndexerType.None, O.Lookup(smpl79, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a"), listloop_i75)), O.Indexer(O.Indexer2(smpl79, O.EIndexerType.None, new ScalarString("a"), listloop_i75), smpl79, O.EIndexerType.None, O.Lookup(smpl79, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null), new ScalarString("a"), listloop_i75)));

                    labelCounter++;
                }
                labelCounter = 0;
                smpl79.command = smplCommandRemember78;
                return temp76;

            };


            Action assign_80 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar74 = func77(smpl);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar74, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
            };
            Func<bool> check_80 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar74 = func77(smpl);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar74.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar74, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_80, check_80, o1);

            //[[commandEnd]]1
        }


        public static readonly ScalarVal i72 = new ScalarVal(1d);
        public static readonly ScalarVal i73 = new ScalarVal(30000d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            //[[commandSpecial]]0
            IVariable forloop_xe7dke6cj_71 = null;
            int counter81 = 0;
            for (O.IterateStart(O.ELoopType.ForTo, ref forloop_xe7dke6cj_71, i72); O.IterateContinue(O.ELoopType.ForTo, forloop_xe7dke6cj_71, i72, i73, null, ref counter81); O.IterateStep(O.ELoopType.ForTo, ref forloop_xe7dke6cj_71, i72, null, counter81))
            {
                ;

                C0(smpl, p);

            };

            //[[commandEnd]]0



        }
    }
}
