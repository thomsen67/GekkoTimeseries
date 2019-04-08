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

            O.Assignment o0 = new O.Assignment();

            Func<Map> MapDef_mapTmpvar2 = () =>
            {
                Map mapTmpvar2 = new Map();
                Action assign_4 = () =>
                {
                    O.AdjustT0(smpl, -2);
                    IVariable ivTmpvar3 = O.HandleString(new ScalarString(@"a"));
                    O.AdjustT0(smpl, 2);
                    O.Lookup(smpl, mapTmpvar2, null, "%i1", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
                    ;
                };
                Func<bool> check_4 = () =>
                {
                    O.AdjustT0(smpl, -2);
                    IVariable ivTmpvar3 = O.HandleString(new ScalarString(@"a"));
                    O.AdjustT0(smpl, 2);
                    if (ivTmpvar3.Type() != EVariableType.Series) return false;
                    O.Dynamic1(smpl);
                    O.Lookup(smpl, mapTmpvar2, null, "%i1", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
                    ;
                    return O.Dynamic2(smpl);
                };
                O.RunAssigmentMaybeDynamic(smpl, assign_4, check_4, o0);

                Action assign_7 = () =>
                {
                    O.AdjustT0(smpl, -2);
                    IVariable ivTmpvar5 = i6;
                    O.AdjustT0(smpl, 2);
                    O.Lookup(smpl, mapTmpvar2, null, "%v1", null, ivTmpvar5, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
                    ;
                };
                Func<bool> check_7 = () =>
                {
                    O.AdjustT0(smpl, -2);
                    IVariable ivTmpvar5 = i6;
                    O.AdjustT0(smpl, 2);
                    if (ivTmpvar5.Type() != EVariableType.Series) return false;
                    O.Dynamic1(smpl);
                    O.Lookup(smpl, mapTmpvar2, null, "%v1", null, ivTmpvar5, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
                    ;
                    return O.Dynamic2(smpl);
                };
                O.RunAssigmentMaybeDynamic(smpl, assign_7, check_7, o0);


                return mapTmpvar2;
            };


            Action assign_8 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar1 = MapDef_mapTmpvar2();
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "#m", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
            };
            Func<bool> check_8 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar1 = MapDef_mapTmpvar2();
                O.AdjustT0(smpl, 2);
                if (ivTmpvar1.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "#m", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_8, check_8, o0);

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i6 = new ScalarVal(100d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
