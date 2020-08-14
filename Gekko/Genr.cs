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


            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe(smpl);

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Time o1 = new O.Time();
            o1.t1 = O.ConvertToDate(i3, O.GetDateChoices.FlexibleStart);
            ;
            o1.t2 = O.ConvertToDate(i4, O.GetDateChoices.FlexibleEnd);
            ;

            o1.Exe();

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            O.Assignment o2 = new O.Assignment();
            o2.opt_source = @"<[code]>x = 3, 4, 5";


            Action assign_6 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar5 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("3"), null, new ScalarString("4"), null, new ScalarString("5"), null }));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar5, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_6 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar5 = O.ExplodeIvariablesSeq(true, new List(new List<IVariable> { new ScalarString("3"), null, new ScalarString("4"), null, new ScalarString("5"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar5.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar5, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_6, check_6, o2);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            Program.options.freq = G.GetFreq(O.ConvertToString((new ScalarString("m"))));
            G.Writeln();
            G.Writeln("option freq = " + (G.GetFreq(O.ConvertToString((new ScalarString("m"))))).ToString().ToLower() + "");
            Program.AdjustFreq();
            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Time o4 = new O.Time();
            o4.t1 = O.ConvertToDate(new ScalarDate(GekkoTime.FromStringToGekkoTime("2002m11")), O.GetDateChoices.FlexibleStart);
            ;
            o4.t2 = O.ConvertToDate(new ScalarDate(GekkoTime.FromStringToGekkoTime("2003m11")), O.GetDateChoices.FlexibleEnd);
            ;

            o4.Exe();

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            O.Assignment o5 = new O.Assignment();
            o5.opt_source = @"<[code]>y!a = pch(x!a + 0)";


            Action assign_9 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar7 = Functions.pch(O.Smpl(smpl, -1), smpl, null, null, O.Add(smpl, O.Lookup(smpl, null, null, "x", "a", null, new LookupSettings(), EVariableType.Var, null), i8));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y", "a", ivTmpvar7, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o5)
                ;
            };
            Func<bool> check_9 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar7 = Functions.pch(O.Smpl(smpl, -1), smpl, null, null, O.Add(smpl, O.Lookup(smpl, null, null, "x", "a", null, new LookupSettings(), EVariableType.Var, null), i8));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar7.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y", "a", ivTmpvar7, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o5)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_9, check_9, o5);

            //[[commandEnd]]5
        }


        public static readonly ScalarVal i3 = new ScalarVal(2001d, 0);
        public static readonly ScalarVal i4 = new ScalarVal(2003d, 0);
        public static readonly ScalarVal i8 = new ScalarVal(0d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
