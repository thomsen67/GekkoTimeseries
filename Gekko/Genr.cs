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


            Program.options.series_data_missing = G.GetMissing("zero");
            G.Writeln();
            G.Writeln("option series data missing = " + (G.GetMissing("zero")).ToString().ToLower() + "");

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);

            O.Assignment o1 = new O.Assignment();
            o1.opt_source = @"<[code]>y=x";


            Action assign_56 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar55 = O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar55, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
            };
            Func<bool> check_56 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar55 = O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar55.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar55, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o1)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_56, check_56, o1);

            //[[commandEnd]]1
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
