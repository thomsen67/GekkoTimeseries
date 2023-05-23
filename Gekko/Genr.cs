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

            O.Time o0 = new O.Time();
            o0.t1 = O.ConvertToDate(i5, O.GetDateChoices.FlexibleStart);
            ;
            o0.t2 = O.ConvertToDate(i6, O.GetDateChoices.FlexibleEnd);
            ;

            o0.Exe();

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o1 = new O.Assignment();
            o1.opt_source = @"<[code]>b = 102, 103, 102";


            Globals.precedentsSeries = null;
            Action assign_8 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar7 = O.FlattenIVariablesSeq(true, new List(new List<IVariable> { new ScalarString("102"), null, new ScalarString("103"), null, new ScalarString("102"), null }));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "b", null, ivTmpvar7, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
            };
            Func<bool> check_8 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar7 = O.FlattenIVariablesSeq(true, new List(new List<IVariable> { new ScalarString("102"), null, new ScalarString("103"), null, new ScalarString("102"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar7.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "b", null, ivTmpvar7, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_8, check_8, o1);

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o2 = new O.Assignment();
            o2.opt_source = @"<[code]>xx = 1";


            Globals.precedentsSeries = null;
            Action assign_11 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar9 = i10;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "xx", null, ivTmpvar9, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_11 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar9 = i10;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar9.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "xx", null, ivTmpvar9, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_11, check_11, o2);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o3 = new O.Assignment();
            o3.opt_source = @"<[code]>xx $ (b == 102) <2001 2002 d> = 2";
            smpl.t0 = O.ConvertToDate(i15, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t1 = O.ConvertToDate(i15, O.GetDateChoices.FlexibleStart);
            ;
            smpl.t2 = O.ConvertToDate(i16, O.GetDateChoices.FlexibleEnd);
            ;
            smpl.t3 = O.ConvertToDate(i16, O.GetDateChoices.FlexibleEnd);
            ;

            o3.opt_d = "yes";

            Globals.precedentsSeries = null;
            Action assign_17 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar12 = i14;
                O.AdjustT0(smpl, 2);
                O.DollarLookup(O.Equals(smpl, O.Lookup(smpl, null, null, "b", null, null, new LookupSettings(), EVariableType.Var, null), i13)
                , smpl, null, null, "xx", null, ivTmpvar12, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
                ;
            };
            Func<bool> check_17 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar12 = i14;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar12.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.DollarLookup(O.Equals(smpl, O.Lookup(smpl, null, null, "b", null, null, new LookupSettings(), EVariableType.Var, null), i13)
                , smpl, null, null, "xx", null, ivTmpvar12, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_17, check_17, o3);

            //[[commandEnd]]3
        }


        public static readonly ScalarVal i5 = new ScalarVal(2000d, 0);
        public static readonly ScalarVal i6 = new ScalarVal(2002d, 0);
        public static readonly ScalarVal i10 = new ScalarVal(1d, 0);
        public static readonly ScalarVal i13 = new ScalarVal(102d, 0);
        public static readonly ScalarVal i14 = new ScalarVal(2d, 0);
        public static readonly ScalarVal i15 = new ScalarVal(2001d, 0);
        public static readonly ScalarVal i16 = new ScalarVal(2002d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
