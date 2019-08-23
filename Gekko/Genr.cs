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


            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe(smpl);

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);


            O.Time o1 = new O.Time();
            o1.t1 = O.ConvertToDate(i326, O.GetDateChoices.FlexibleStart);
            ;
            o1.t2 = O.ConvertToDate(i327, O.GetDateChoices.FlexibleEnd);
            ;

            o1.Exe();

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetText(@"¤3"); O.InitSmpl(smpl, p);

            O.Assignment o2 = new O.Assignment();
            o2.opt_source = @"<[code]>x = 5";


            Action assign_330 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar328 = i329;
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar328, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
            };
            Func<bool> check_330 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar328 = i329;
                O.AdjustT0(smpl, 2);
                if (ivTmpvar328.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "x", null, ivTmpvar328, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o2)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_330, check_330, o2);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);

            Series x1 = Program.databanks.GetFirst().GetIVariable("x!a") as Series;

            O.Write o3 = new O.Write();
            o3.type = @"write"; o3.fileName = o3.fileName = O.ConvertToString((new ScalarString("xxx")));
            o3.type = @"write"; o3.Exe();                        
            p.SetText(@"¤5"); O.InitSmpl(smpl, p);            
            O.Read o4 = new O.Read();
            o4.p = p;
            o4.type = @"read";
            o4.fileName = O.ConvertToString((new ScalarString("xxx")));
            o4.Exe();

            Series x2 = Program.databanks.GetFirst().GetIVariable("x!a") as Series;

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤6"); O.InitSmpl(smpl, p);

            O.Assignment o5 = new O.Assignment();
            o5.opt_source = @"<[code]>y = x[-1]";


            Action assign_333 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar331 = O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i332)
                ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i332)
                );
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar331, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o5)
                ;
            };
            Func<bool> check_333 = () =>
            {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar331 = O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i332)
                ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, null, "x", null, null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i332)
                );
                O.AdjustT0(smpl, 2);
                if (ivTmpvar331.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "y", null, ivTmpvar331, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o5)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_333, check_333, o5);
            
            Series x3 = Program.databanks.GetFirst().GetIVariable("x!a") as Series;
                        
            p.SetText(@"¤7"); O.InitSmpl(smpl, p);

            p.SetText(@"¤8"); O.InitSmpl(smpl, p);

            O.Write o7 = new O.Write();
            o7.type = @"write"; o7.fileName = o7.fileName = O.ConvertToString((new ScalarString("data")));
            ;
            o7.type = @"write"; o7.Exe();

            //[[commandEnd]]7


            //[[commandStart]]8
            p.SetText(@"¤9"); O.InitSmpl(smpl, p);


            O.Read o8 = new O.Read();
            o8.p = p;
            o8.type = @"read";
            o8.fileName = O.ConvertToString((new ScalarString("data")));
            o8.Exe();

            Series x4 = Program.databanks.GetFirst().GetIVariable("x!a") as Series;

            //[[commandEnd]]8


            //[[commandStart]]9
            p.SetText(@"¤10"); O.InitSmpl(smpl, p);


            //[[commandEnd]]9
        }


        public static readonly ScalarVal i326 = new ScalarVal(1966d);
        public static readonly ScalarVal i327 = new ScalarVal(2018d);
        public static readonly ScalarVal i329 = new ScalarVal(5d);
        public static readonly ScalarVal i332 = new ScalarVal(1d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
