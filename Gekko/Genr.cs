/*
 * 
 * 
 ASTRESET [1]
  ASTFUNCTIONDEF2 [1]
    void [1]
    ASTIDENT [1]
      expand [1]
    ASTPLACEHOLDER [1]
      ASTPLACEHOLDER [1]
        series [1]
        ASTPLACEHOLDER [0]
          ASTPLACEHOLDER [0]
          ASTIDENT [1]
            x [1]
        ASTPLACEHOLDER [0]
        ASTPLACEHOLDER [0]
    ASTFUNCTIONDEFCODE [1]
      ASTASSIGNMENT¤x = 3 [1]
        ASTLEFTSIDE [0]
          ASTBANKVARNAME [0]
            ASTPLACEHOLDER [0]
            ASTVARNAME [0]
              ASTPLACEHOLDER [0]
              ASTPLACEHOLDER [1]
                ASTNAME [1]
                  ASTIDENT [1]
                    x [1]
              ASTPLACEHOLDER [0]
        ASTINTEGER [1]
          3 [1]
        ASTPLACEHOLDER [0]
        ASTPLACEHOLDER [0]
        ASTPLACEHOLDER [0]
      ASTASSIGNMENT¤%¨s = type¨(x) [1]
        ASTLEFTSIDE [0]
          ASTBANKVARNAME [0]
            ASTPLACEHOLDER [0]
            ASTVARNAME [0]
              ASTPLACEHOLDER [0]
                ASTPERCENT [0]
              ASTPLACEHOLDER [1]
                ASTNAME [1]
                  ASTIDENT [1]
                    s [1]
              ASTPLACEHOLDER [0]
        ASTFUNCTION [1]
          ASTPLACEHOLDER [1]
            ASTIDENT [1]
              type [1]
            ASTPLACEHOLDER [0]
          ASTSPECIALARGS [0]
          ASTBANKVARNAME [0]
            ASTPLACEHOLDER [0]
            ASTVARNAME [0]
              ASTPLACEHOLDER [0]
              ASTPLACEHOLDER [1]
                ASTNAME [1]
                  ASTIDENT [1]
                    x [1]
              ASTPLACEHOLDER [0]
        ASTPLACEHOLDER [0]
        ASTPLACEHOLDER [0]
        ASTPLACEHOLDER [0]
      ASTMEM [1]
  ASTTIME [1]
    ASTDATES [1]
      ASTINTEGER [1]
        2001 [1]
      ASTINTEGER [1]
        2004 [1]
  ASTASSIGNMENT¤x = 1, 2, m¨(), 4 [1]
    ASTLEFTSIDE [0]
      ASTBANKVARNAME [0]
        ASTPLACEHOLDER [0]
        ASTVARNAME [0]
          ASTPLACEHOLDER [0]
          ASTPLACEHOLDER [1]
            ASTNAME [1]
              ASTIDENT [1]
                x [1]
          ASTPLACEHOLDER [0]
    ASTNAKEDLIST [0]
      ASTNAKEDLISTITEM [0]
        ASTSEQ7 [0]
          ASTPLACEHOLDER [0]
          ASTPLACEHOLDER [1]
            ASTIDENTDIGIT [1]
              1 [1]
          ASTPLACEHOLDER [0]
      ASTNAKEDLISTITEM [0]
        ASTSEQ7 [0]
          ASTPLACEHOLDER [0]
          ASTPLACEHOLDER [1]
            ASTIDENTDIGIT [1]
              2 [1]
          ASTPLACEHOLDER [0]
      ASTNAKEDLISTMISS [1]
        ASTIDENT [1]
          m [1]
      ASTNAKEDLISTITEM [0]
        ASTSEQ7 [0]
          ASTPLACEHOLDER [0]
          ASTPLACEHOLDER [1]
            ASTIDENTDIGIT [1]
              4 [1]
          ASTPLACEHOLDER [0]
    ASTPLACEHOLDER [0]
    ASTPLACEHOLDER [0]
    ASTPLACEHOLDER [0]
  ASTFUNCTIONNAKED [1]
    ASTPLACEHOLDER [1]
      ASTIDENT [1]
        expand [1]
      ASTPLACEHOLDER [0]
    ASTSPECIALARGS [0]
    ASTBANKVARNAME [0]
      ASTPLACEHOLDER [0]
      ASTVARNAME [0]
        ASTPLACEHOLDER [0]
        ASTPLACEHOLDER [1]
          ASTNAME [1]
            ASTIDENT [1]
              x [1]
        ASTPLACEHOLDER [0]
   [0]
 * 
 * 
 * */



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
        }
        public static void C1(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]5
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            O.Time o5 = new O.Time();
            o5.t1 = O.ConvertToDate(i11, O.GetDateChoices.FlexibleStart);
            ;
            o5.t2 = O.ConvertToDate(i12, O.GetDateChoices.FlexibleEnd);
            ;

            o5.Exe();

            //[[commandEnd]]5


            //[[commandStart]]6
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o6 = new O.Assignment();
            o6.opt_trace = @"x = 1, 2, m(), 4";


            Globals.precedentsSeries = null;
            Action assign_14 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar13 = O.FlattenIVariablesSeq(true, new List(new List<IVariable> { new ScalarString("1"), null, new ScalarString("2"), null, Globals.scalarValMissing, null, new ScalarString("4"), null }));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar13, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o6)
                ;
            };
            Func<bool> check_14 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar13 = O.FlattenIVariablesSeq(true, new List(new List<IVariable> { new ScalarString("1"), null, new ScalarString("2"), null, Globals.scalarValMissing, null, new ScalarString("4"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar13.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar13, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o6)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_14, check_14, o6, p);

            //[[commandEnd]]6


            //[[commandStart]]7
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            O.FunctionLookupNew3(p, null, "expand")(smpl, p, false, null, null, new GekkoArg((spml15) => O.Lookup(spml15, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), (spml15) => new ScalarString("x")));

            //[[commandEnd]]7
        }

        public static void CC0(GekkoSmpl smpl, P p, ref IVariable xfunctionarg_xf7dke8cj_3)
        {
            IVariable functionarg_xf7dke8cj_3 = xfunctionarg_xf7dke8cj_3;

            //[[commandStart]]2
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o2 = new O.Assignment();
            o2.opt_trace = @"x = 3";


            Globals.precedentsSeries = null;
            Action assign_6 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar4 = i5;
                O.AdjustT0(smpl, 2);
                functionarg_xf7dke8cj_3 = ivTmpvar4;

                ;
            };
            Func<bool> check_6 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar4 = i5;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar4.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                functionarg_xf7dke8cj_3 = ivTmpvar4;

                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_6, check_6, o2, p);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);
            O.Assignment o3 = new O.Assignment();
            o3.opt_trace = @"%s = type(x)";


            Globals.precedentsSeries = null;
            Action assign_8 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar7 = Functions.type(smpl, null, null, functionarg_xf7dke8cj_3);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "%s", null, ivTmpvar7, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
            };
            Func<bool> check_8 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar7 = Functions.type(smpl, null, null, functionarg_xf7dke8cj_3);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar7.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "%s", null, ivTmpvar7, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o3)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_8, check_8, o3, p);

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            O.Mem(null);

            //[[commandEnd]]4
            xfunctionarg_xf7dke8cj_3 = functionarg_xf7dke8cj_3;

        }

        public static readonly ScalarVal i5 = new ScalarVal(3d, 0);
        public static void FunctionDef9()
        {

            O.PrepareUfunction(3, "expand");

            O.Add3_UfunctionSpecialName(null, "expand", (GekkoSmpl smpl, P p, bool q10, GekkoArg functionarg_xf7dke8cj_1_func, GekkoArg functionarg_xf7dke8cj_2_func, GekkoArg functionarg_xf7dke8cj_3_func) =>
            {
                IVariable functionarg_xf7dke8cj_1 = O.TypeCheck_date(functionarg_xf7dke8cj_1_func, smpl, 1);
                IVariable functionarg_xf7dke8cj_2 = O.TypeCheck_date(functionarg_xf7dke8cj_2_func, smpl, 2);
                IVariable functionarg_xf7dke8cj_3 = O.TypeCheck_series(functionarg_xf7dke8cj_3_func.f1(smpl), 3);

                Databank local1 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg1 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("expand", @""); p.SetLastFileSentToANTLR(O.LastText("expand", @"")); p.SetCurrentLibrary(null); p.Deeper();
                try
                {
                    CC0(smpl, p, ref functionarg_xf7dke8cj_3);
                    return null;
                }
                catch { p.Deeper(); throw; }
                finally
                {
                    Program.databanks.local = local1; Program.databanks.localGlobal = lg1; p.RemoveLast(); ;
                }
            });

        }

        public static readonly ScalarVal i11 = new ScalarVal(2001d, 0);
        public static readonly ScalarVal i12 = new ScalarVal(2004d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);

            p.SetStack(@"¤1");

            FunctionDef9();

            //[[commandEnd]]1

            C1(smpl, p);

        }
    }
}
