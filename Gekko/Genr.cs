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
            o0.opt_source = @"<[code]>#m=(%x=2)";

            o0.opt_source = @"<[code]>%x=2";

            Func<GekkoSmpl, Map> MapDef_mapTmpvar42 = (smpl46) =>
            {
                Map mapTmpvar42 = new Map();
                Action assign_45 = () =>
                {
                    O.AdjustT0(smpl46, -2);
                    IVariable ivTmpvar43 = i44;
                    O.AdjustT0(smpl46, 2);
                    O.Lookup(smpl46, mapTmpvar42, null, "%x", null, ivTmpvar43, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
                    ;
                };
                Func<bool> check_45 = () =>
                {
                    O.AdjustT0(smpl46, -2);
                    IVariable ivTmpvar43 = i44;
                    O.AdjustT0(smpl46, 2);
                    if (ivTmpvar43.Type() != EVariableType.Series) return false;
                    O.Dynamic1(smpl46);
                    O.Lookup(smpl46, mapTmpvar42, null, "%x", null, ivTmpvar43, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
                    ;
                    return O.Dynamic2(smpl46);
                };
                O.RunAssigmentMaybeDynamic(smpl46, assign_45, check_45, o0);


                return mapTmpvar42;
            };


            Action assign_47 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar41 = MapDef_mapTmpvar42(smpl);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "#m", null, ivTmpvar41, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
            };
            Func<bool> check_47 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar41 = MapDef_mapTmpvar42(smpl);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar41.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "#m", null, ivTmpvar41, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_47, check_47, o0);

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i44 = new ScalarVal(2d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
