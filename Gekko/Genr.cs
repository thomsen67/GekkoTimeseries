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
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe(smpl);

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            Program.options.freq = O.XNameOrString2Freq("freq", (new ScalarString("q")));
            O.PrintOptions("Program.options.freq", false);
            O.HandleOptions("Program.options.freq", 0, p);

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetStack(@"¤2"); O.InitSmpl(smpl, p);

            O.Time o2 = new O.Time();
            o2.t1 = O.ConvertToDate(new ScalarDate(GekkoTime.FromStringToGekkoTime("2001q1")), O.GetDateChoices.FlexibleStart);
            ;
            o2.t2 = O.ConvertToDate(new ScalarDate(GekkoTime.FromStringToGekkoTime("2002q4")), O.GetDateChoices.FlexibleEnd);
            ;

            o2.Exe();

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetStack(@"¤3"); O.InitSmpl(smpl, p);
            O.Assignment o3 = new O.Assignment();
            o3.opt_trace = @"x = 1";


            Globals.precedentsSeries = null;
            Action assign_3 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar1 = i2;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
            };
            Func<bool> check_3 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar1 = i2;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar1.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_3, check_3, o3, p);

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetStack(@"¤4"); O.InitSmpl(smpl, p);
            O.Assignment o4 = new O.Assignment();
            o4.opt_trace = @"%v = avgt(<2000q1 2003q4>, x/x)";


            Globals.precedentsSeries = null;
            Action assign_5 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar4 = Functions.avgt(smpl, new ScalarDate(GekkoTime.FromStringToGekkoTime("2000q1")), new ScalarDate(GekkoTime.FromStringToGekkoTime("2003q4")), O.Divide(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null)));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%v", null, ivTmpvar4, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
            };
            Func<bool> check_5 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar4 = Functions.avgt(smpl, new ScalarDate(GekkoTime.FromStringToGekkoTime("2000q1")), new ScalarDate(GekkoTime.FromStringToGekkoTime("2003q4")), O.Divide(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null)));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar4.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%v", null, ivTmpvar4, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_5, check_5, o4, p);

            //[[commandEnd]]4
        }


        public static readonly ScalarVal i2 = new ScalarVal(1d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
