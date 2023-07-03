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
            o0.opt_source = @"<[code]>c_{%name}_{#m[1][%col]} <%t1 %t2> = #m[%row][%col]";
            smpl.t0 = O.ConvertToDate(O.Lookup(smpl, null, null, "%t1", null, null, new LookupSettings(), EVariableType.Var, null), O.GetDateChoices.FlexibleStart);
            ;
            smpl.t1 = O.ConvertToDate(O.Lookup(smpl, null, null, "%t1", null, null, new LookupSettings(), EVariableType.Var, null), O.GetDateChoices.FlexibleStart);
            ;
            smpl.t2 = O.ConvertToDate(O.Lookup(smpl, null, null, "%t2", null, null, new LookupSettings(), EVariableType.Var, null), O.GetDateChoices.FlexibleEnd);
            ;
            smpl.t3 = O.ConvertToDate(O.Lookup(smpl, null, null, "%t2", null, null, new LookupSettings(), EVariableType.Var, null), O.GetDateChoices.FlexibleEnd);
            ;




            Globals.precedentsSeries = null;
            Action assign_24 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar22 = O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "%col", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "%row", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "%row", null, null, new LookupSettings(), EVariableType.Var, null)
                ), O.Lookup(smpl, null, null, "%col", null, null, new LookupSettings(), EVariableType.Var, null)
                );
                O.AdjustT0(smpl, 2);
                //O.Lookup(smpl, null, (new ScalarString("c_")).Concat(smpl, O.Lookup(smpl, null, null, "%name", null, ivTmpvar22, new LookupSettings(), EVariableType.Var, null)).Concat(smpl, new ScalarString("_")).Concat(smpl, O.IndexerSetData(smpl, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, i23
                //), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(), EVariableType.Var, null), i23
                //), ivTmpvar22, o0, O.Lookup(smpl, null, null, "%col", null, null, new LookupSettings(), EVariableType.Var, null)
                //)), ivTmpvar22, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
                ;
            };
            Func<bool> check_24 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar22 = O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "%col", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Indexer(O.Indexer2(smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "%row", null, null, new LookupSettings(), EVariableType.Var, null)
                ), smpl, O.EIndexerType.None, O.Lookup(smpl, null, null, "#m", null, null, new LookupSettings(), EVariableType.Var, null), O.Lookup(smpl, null, null, "%row", null, null, new LookupSettings(), EVariableType.Var, null)
                ), O.Lookup(smpl, null, null, "%col", null, null, new LookupSettings(), EVariableType.Var, null)
                );
                O.AdjustT0(smpl, 2);
                if (ivTmpvar22.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);                
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_24, check_24, o0, p);

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i23 = new ScalarVal(1d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
