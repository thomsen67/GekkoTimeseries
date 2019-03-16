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


            O.Model o0 = new O.Model();
            o0.p = p; o0.fileName = O.ConvertToString((new ScalarString("jul05a")));

            o0.Exe();

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);


            O.Read o1 = new O.Read();
            o1.p = p;
            o1.type = @"read";
            o1.fileName = O.ConvertToString((new ScalarString("jul05")));


            o1.Exe();

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetText(@"¤3"); O.InitSmpl(smpl, p);


            O.Time o2 = new O.Time();
            o2.t1 = O.ConvertToDate(i1, O.GetDateChoices.FlexibleStart);
            ;
            o2.t2 = O.ConvertToDate(i2, O.GetDateChoices.FlexibleEnd);
            ;

            o2.Exe();

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);

            O.Assignment o3 = new O.Assignment();


            Action assign_7 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3 = O.ListDefHelper(i4, null, i5, null, i6, null);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
            };
            Func<bool> check_7 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3 = O.ListDefHelper(i4, null, i5, null, i6, null);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar3.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_7, check_7, o3);

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤5"); O.InitSmpl(smpl, p);

            O.Assignment o4 = new O.Assignment();


            Action assign_10 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar8 = O.Multiply(smpl, i9, Functions.dlog(O.Smpl(smpl, -1), smpl, O.Add(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null))));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar8, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
            };
            Func<bool> check_10 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar8 = O.Multiply(smpl, i9, Functions.dlog(O.Smpl(smpl, -1), smpl, O.Add(smpl, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null))));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar8.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar8, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o4)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_10, check_10, o4);

            //[[commandEnd]]4
        }


        public static readonly ScalarVal i1 = new ScalarVal(2001d);
        public static readonly ScalarVal i2 = new ScalarVal(2003d);
        public static readonly ScalarVal i4 = new ScalarVal(1d);
        public static readonly ScalarVal i5 = new ScalarVal(2d);
        public static readonly ScalarVal i6 = new ScalarVal(3d);
        public static readonly ScalarVal i9 = new ScalarVal(2d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
