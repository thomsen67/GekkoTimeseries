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
            IVariable listloopMovedStuff_43 = O.Lookup(smpl, null, null, "#m1", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_44 = O.Lookup(smpl, null, null, "xx", null, null, new LookupSettings(), EVariableType.Var, null);
            IVariable listloopMovedStuff_45 = O.Lookup(smpl, null, null, "#m1", null, null, new LookupSettings(), EVariableType.Var, null);

            O.Assignment o0 = new O.Assignment();

            Func<IVariable> func47 = () =>
            {
                var smplCommandRemember48 = smpl.command; smpl.command = GekkoSmplCommand.Sum;
                Series temp46 = new Series(ESeriesType.Normal, Program.options.freq, null); temp46.SetZero(smpl);

                foreach (IVariable listloop_m142 in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString("m1")))), null, new LookupSettings(), EVariableType.Var, null)))
                {
                    temp46.InjectAdd(smpl, listloopMovedStuff_43);

                    labelCounter++;
                }
                labelCounter = 0;
                smpl.command = smplCommandRemember48;
                return temp46;

            };


            Action assign_49 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar41 = func47();
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar41, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
            };
            Func<bool> check_49 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar41 = func47();
                O.AdjustT0(smpl, 2);
                if (ivTmpvar41.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar41, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_49, check_49, o0);

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
