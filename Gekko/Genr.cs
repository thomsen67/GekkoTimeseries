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
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            Func<GekkoSmpl, IVariable> func60 = (GekkoSmpl smpl62) =>
            {
                var smplCommandRemember61 = smpl62.command; smpl62.command = GekkoSmplCommand.Sum;
                Series temp59 = new Series(ESeriesType.Normal, Program.options.freq, null); temp59.SetZero(smpl62);

                foreach (IVariable listloop_a56 in new O.GekkoListIterator(O.Lookup(smpl62, null, ((O.scalarStringHash).Add(smpl62, (new ScalarString("a")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    foreach (IVariable listloop_s57 in new O.GekkoListIterator(O.Lookup(smpl62, null, ((O.scalarStringHash).Add(smpl62, (new ScalarString("s")))), null, new LookupSettings(), EVariableType.Var, null)))
                    {
                        foreach (IVariable listloop_o58 in new O.GekkoListIterator(O.Lookup(smpl62, null, ((O.scalarStringHash).Add(smpl62, (new ScalarString("o")))), null, new LookupSettings(), EVariableType.Var, null)))
                        {
                            temp59.InjectAdd(smpl62, O.Indexer(O.Indexer2(smpl62, O.EIndexerType.None, listloop_a56, listloop_s57, listloop_o58), smpl62, O.EIndexerType.None, O.Lookup(smpl62, null, null, "pop", null, null, new LookupSettings(), EVariableType.Var, null), listloop_a56, listloop_s57, listloop_o58));

                            labelCounter++;
                        }
                    }
                }
                labelCounter = 0;
                smpl62.command = smplCommandRemember61;
                return temp59;

            };


            O.Decomp2 o0 = new O.Decomp2();
            o0.label = @"popsum in (-popsum + sum((#a, #s, #o), pop[#a, #s, #o]))";
            o0.t1 = O.ConvertToDate(i54, O.GetDateChoices.FlexibleStart);
            ;
            o0.t2 = O.ConvertToDate(i55, O.GetDateChoices.FlexibleEnd);
            ;

            o0.opt_prtcode = O.ConvertToString((new ScalarString("xd")));




            o0.where.Add(new List<IVariable>() { O.HandleString(new ScalarString(@"se")), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("o")) })) });
            o0.where.Add(new List<IVariable>() { O.HandleString(new ScalarString(@"se")), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("o")) })) });

            o0.group.Add(new List<IVariable>() { O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("a")) })), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("a_agg")) })), O.HandleString(new ScalarString(@"10-year")), O.HandleString(new ScalarString(@"27")) });
            o0.group.Add(new List<IVariable>() { O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("a")) })), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { O.scalarStringHash.Concat(null, new ScalarString("a_agg")) })), O.HandleString(new ScalarString(@"10-year")), O.HandleString(new ScalarString(@"27")) });

            o0.decompItems.Add(new DecompItems(null, O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("x1") })), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("e2") }))));


            o0.decompItems.Add(new DecompItems(null, O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("x3") })), O.ExplodeIvariablesSeq(false, new List(new List<IVariable> { new ScalarString("e1") }))));



            o0.Exe();

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i54 = new ScalarVal(2002d);
        public static readonly ScalarVal i55 = new ScalarVal(2003d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
