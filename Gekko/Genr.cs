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
            O.Assignment o0 = new O.Assignment();
            o0.opt_trace = @"#modul=map()";


            Globals.precedentsSeries = null;
            Action assign_20 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar19 = Functions.map(smpl, null, null);
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "#modul", null, ivTmpvar19, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
            };
            Func<bool> check_20 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar19 = Functions.map(smpl, null, null);
                O.AdjustT0(smpl, 2);
                if (ivTmpvar19.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "#modul", null, ivTmpvar19, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_20, check_20, o0, p);

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetStack(@"¤2"); O.InitSmpl(smpl, p);
            O.Assignment o1 = new O.Assignment();
            o1.opt_trace = @"#modul.%lister = 'c:\tools'";


            Globals.precedentsSeries = null;
            Action assign_22 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar21 = O.HandleString(new ScalarString(@"c:\tools"));
                O.AdjustT0(smpl, 2);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#modul", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar21, o1, (O.scalarStringPercent).Concat(smpl, (new ScalarString("lister"))))
                ;
            };
            Func<bool> check_22 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar21 = O.HandleString(new ScalarString(@"c:\tools"));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar21.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#modul", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null), ivTmpvar21, o1, (O.scalarStringPercent).Concat(smpl, (new ScalarString("lister"))))
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_22, check_22, o1, p);

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetStack(@"¤3"); O.InitSmpl(smpl, p);
            O.Assignment o2 = new O.Assignment();
            o2.opt_trace = @"#(listfile {#modul.%lister}\xx) = a,b";


            Globals.precedentsSeries = null;
            Action assign_24 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar23 = O.FlattenIVariablesSeq(true, new List(new List<IVariable> { new ScalarString("a"), null, new ScalarString("b"), null }));
                O.AdjustT0(smpl, 2);
//                O.Lookup(smpl, null, (O.scalarStringHash).Concat(smpl, ((new ScalarString("listfile___"))).Concat(smpl, O.ReplaceSlash((O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#modul", null, null, new LookupSettings(), EVariableType.Var, null), ivTmpvar23, o2, (O.scalarStringPercent).Concat(smpl, (new ScalarString("lister"))))).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("xx")))))), ivTmpvar23, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_24 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar23 = O.FlattenIVariablesSeq(true, new List(new List<IVariable> { new ScalarString("a"), null, new ScalarString("b"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar23.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
//                O.Lookup(smpl, null, (O.scalarStringHash).Concat(smpl, ((new ScalarString("listfile___"))).Concat(smpl, O.ReplaceSlash((O.IndexerSetData(smpl, O.Lookup(smpl, null, null, "#modul", null, null, new LookupSettings(), EVariableType.Var, null), ivTmpvar23, o2, (O.scalarStringPercent).Concat(smpl, (new ScalarString("lister"))))).Add(smpl, new ScalarString("\\")).Add(smpl, (new ScalarString("xx")))))), ivTmpvar23, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_24, check_24, o2, p);

            //[[commandEnd]]2
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
