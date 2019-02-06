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
            p.SetText(@"¤3"); O.InitSmpl(smpl, p);


            O.Close o0 = new O.Close();
            o0.listItems = O.ExplodeIvariablesSeq(new List(new List<IVariable> { new ScalarString("*") }));
            o0.Exe();

            //[[commandEnd]]0


            //[[commandStart]]1
            p.SetText(@"¤4"); O.InitSmpl(smpl, p);


            O.Clear o1 = new O.Clear();
            o1.p = p; o1.Exe();

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetText(@"¤5"); O.InitSmpl(smpl, p);


            O.Open o2 = new O.Open();
            o2.openFileNames = O.ExplodeIvariables(new List(new List<IVariable> { (new ScalarString("kadam")) }));
            o2.openFileNamesAs = null;

            o2.Exe();

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤6"); O.InitSmpl(smpl, p);


            O.Open o3 = new O.Open();
            o3.opt_edit = "yes";

            o3.openFileNames = O.ExplodeIvariables(new List(new List<IVariable> { (new ScalarString("kadamk")) }));
            o3.openFileNamesAs = null;

            o3.Exe();

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤12"); O.InitSmpl(smpl, p);


            O.Index o4 = new O.Index();
            o4.opt_mute = "yes";

            o4.opt_showbank = O.ConvertToString((new ScalarString("no")));

            o4.opt_showfreq = O.ConvertToString((new ScalarString("no")));

            o4.names2 = O.ExplodeIvariablesSeq(new List(new List<IVariable> { (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("ALLEk")))) }));
            o4.type = @"series"; o4.names1 = O.ExplodeIvariablesSeq(new List(new List<IVariable> { O.HandleString(new ScalarString(@"K*")) }));
            o4.Exe();

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤13"); O.InitSmpl(smpl, p);


            O.Index o5 = new O.Index();
            o5.opt_mute = "yes";

            o5.opt_showbank = O.ConvertToString((new ScalarString("no")));

            o5.opt_showfreq = O.ConvertToString((new ScalarString("no")));

            o5.names2 = O.ExplodeIvariablesSeq(new List(new List<IVariable> { (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("slet")))) }));
            o5.type = @"series"; o5.names1 = O.ExplodeIvariablesSeq(new List(new List<IVariable> { new ScalarString("Kydeu"), new ScalarString("Kyneu"), new ScalarString("Kynneu"), new ScalarString("Kydneu"), new ScalarString("Kjusteu"), new ScalarString("Kdisceu") }));
            o5.Exe();

            //[[commandEnd]]5


            //[[commandStart]]6
            p.SetText(@"¤14"); O.InitSmpl(smpl, p);


            O.Assignment o6 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5113 = O.Subtract(smpl, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("allek")))), null, new LookupSettings(), EVariableType.List, null), O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("slet")))), null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("allek2")))), ivTmpvar5113, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.List, o6)
            ;

            //[[commandEnd]]6


            //[[commandStart]]7
            p.SetText(@"¤15"); O.InitSmpl(smpl, p);


            O.Assignment o7 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5114 = Functions.replaceinside(smpl, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("ALLEk2")))), null, new LookupSettings(), EVariableType.List, null), O.HandleString(new ScalarString(@"k")), O.HandleString(new ScalarString(@"")), i5115);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("ALLE")))), ivTmpvar5114, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.List, o7)
            ;

            //[[commandEnd]]7
        }
        public static void C1(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_5116)
        {
            IVariable forloop_xe7dke6cj_5116 = xforloop_xe7dke6cj_5116;

            //[[commandStart]]9
            p.SetText(@"¤18"); O.InitSmpl(smpl, p);


            O.Assignment o9 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5117 = O.Divide(smpl, O.Multiply(smpl, i5118, O.Lookup(smpl, null, (forloop_xe7dke6cj_5116), null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, (new ScalarString("k")).Add(smpl, forloop_xe7dke6cj_5116), null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, (new ScalarString("pk")).Add(smpl, forloop_xe7dke6cj_5116), ivTmpvar5117, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]9
            xforloop_xe7dke6cj_5116 = forloop_xe7dke6cj_5116;

        }
        public static void C2(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_5120)
        {
            IVariable forloop_xe7dke6cj_5120 = xforloop_xe7dke6cj_5120;

            //[[commandStart]]11
            p.SetText(@"¤22"); O.InitSmpl(smpl, p);


            O.Assignment o11 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5121 = O.Divide(smpl, O.Multiply(smpl, i5122, O.Lookup(smpl, null, (new ScalarString("G")).Add(smpl, forloop_xe7dke6cj_5120), null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, (new ScalarString("Gk")).Add(smpl, forloop_xe7dke6cj_5120), null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, (new ScalarString("Gpk")).Add(smpl, forloop_xe7dke6cj_5120), ivTmpvar5121, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]11
            xforloop_xe7dke6cj_5120 = forloop_xe7dke6cj_5120;

        }
        public static void C3(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_5124)
        {
            IVariable forloop_xe7dke6cj_5124 = xforloop_xe7dke6cj_5124;

            //[[commandStart]]13
            p.SetText(@"¤26"); O.InitSmpl(smpl, p);


            O.Assignment o13 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5125 = O.Divide(smpl, O.Multiply(smpl, i5126, O.Lookup(smpl, null, (new ScalarString("J")).Add(smpl, forloop_xe7dke6cj_5124), null, new LookupSettings(), EVariableType.Var, null)), O.Lookup(smpl, null, (new ScalarString("Jk")).Add(smpl, forloop_xe7dke6cj_5124), null, new LookupSettings(), EVariableType.Var, null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, (new ScalarString("Jpk")).Add(smpl, forloop_xe7dke6cj_5124), ivTmpvar5125, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]13
            xforloop_xe7dke6cj_5124 = forloop_xe7dke6cj_5124;

        }
        public static void C4(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]14
            p.SetText(@"¤37"); O.InitSmpl(smpl, p);


            O.Index o14 = new O.Index();
            o14.opt_mute = "yes";

            o14.opt_showbank = O.ConvertToString((new ScalarString("no")));

            o14.opt_showfreq = O.ConvertToString((new ScalarString("no")));

            o14.names2 = O.ExplodeIvariablesSeq(new List(new List<IVariable> { (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("AFT")))) }));
            o14.type = @"series"; o14.names1 = O.ExplodeIvariablesSeq(new List(new List<IVariable> { O.HandleString(new ScalarString(@"k*")), O.HandleString(new ScalarString(@"pk*")), O.HandleString(new ScalarString(@"Gk*")), O.HandleString(new ScalarString(@"Gpk*")) }));
            o14.Exe();

            //[[commandEnd]]14
        }
        public static void C5(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_5128)
        {
            IVariable forloop_xe7dke6cj_5128 = xforloop_xe7dke6cj_5128;

            //[[commandStart]]16
            p.SetText(@"¤40"); O.InitSmpl(smpl, p);


            O.Assignment o16 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5129 = O.Multiply(smpl, i5130, O.Subtract(smpl, O.Divide(smpl, O.Lookup(smpl, null, (forloop_xe7dke6cj_5128), null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5131)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, (forloop_xe7dke6cj_5128), null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i5131)
            )), i5132));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, (new ScalarString("R")).Add(smpl, forloop_xe7dke6cj_5128), ivTmpvar5129, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]16
            xforloop_xe7dke6cj_5128 = forloop_xe7dke6cj_5128;

        }
        public static void C6(GekkoSmpl smpl, P p, ref IVariable xforloop_xe7dke6cj_5134)
        {
            IVariable forloop_xe7dke6cj_5134 = xforloop_xe7dke6cj_5134;

            //[[commandStart]]18
            p.SetText(@"¤48"); O.InitSmpl(smpl, p);


            O.Assignment o18 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5135 = O.Multiply(smpl, i5136, O.Subtract(smpl, O.Divide(smpl, O.Lookup(smpl, null, (forloop_xe7dke6cj_5134), null, new LookupSettings(), EVariableType.Var, null), O.Indexer(O.Indexer2(smpl, O.EIndexerType.IndexerLag, O.Negate(smpl, i5137)
            ), smpl, O.EIndexerType.IndexerLag, O.Lookup(smpl, null, (forloop_xe7dke6cj_5134), null, new LookupSettings(), EVariableType.Var, null), O.Negate(smpl, i5137)
            )), i5138));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, (new ScalarString("V")).Add(smpl, forloop_xe7dke6cj_5134), ivTmpvar5135, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]18
            xforloop_xe7dke6cj_5134 = forloop_xe7dke6cj_5134;

        }
        public static void C7(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]19
            p.SetText(@"¤55"); O.InitSmpl(smpl, p);


            O.Close o19 = new O.Close();
            o19.listItems = O.ExplodeIvariablesSeq(new List(new List<IVariable> { new ScalarString("*") }));
            o19.Exe();

            //[[commandEnd]]19


            //[[commandStart]]20
            p.SetText(@"¤55"); O.InitSmpl(smpl, p);


            O.Clear o20 = new O.Clear();
            o20.p = p; o20.Exe();

            //[[commandEnd]]20


            //[[commandStart]]21
            p.SetText(@"¤56"); O.InitSmpl(smpl, p);


            O.Assignment o21 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5140 = null; // Functions.tostring(smpl, new GekkoArg((spml5141) => Functions.currentperend(spml5141), (spml5141) => null));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%slut", null, ivTmpvar5140, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o21)
            ;

            //[[commandEnd]]21


            //[[commandStart]]22
            p.SetText(@"¤57"); O.InitSmpl(smpl, p);


            O.Assignment o22 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5142 = Functions.substring(smpl, O.Lookup(smpl, null, null, "%slut", null, null, new LookupSettings(), EVariableType.Var, null), i5143, i5144);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%slutar", null, ivTmpvar5142, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o22)
            ;

            //[[commandEnd]]22


            //[[commandStart]]23
            p.SetText(@"¤58"); O.InitSmpl(smpl, p);


            O.Assignment o23 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5145 = Functions.substring(smpl, O.Lookup(smpl, null, null, "%slut", null, null, new LookupSettings(), EVariableType.Var, null), i5146, i5147);
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%slkv", null, ivTmpvar5145, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o23)
            ;

            //[[commandEnd]]23


            //[[commandStart]]24
            p.SetText(@"¤59"); O.InitSmpl(smpl, p);


            O.Open o24 = new O.Open();
            o24.openFileNames = O.ExplodeIvariables(new List(new List<IVariable> { (new ScalarString("knrsup")) }));
            o24.openFileNamesAs = null;

            o24.Exe();

            //[[commandEnd]]24


            //[[commandStart]]25
            p.SetText(@"¤61"); O.InitSmpl(smpl, p);


            O.FunctionLookupNew5("procedure___vakstbidrag")(smpl, p, new GekkoArg((spml5149) => O.HandleString(new ScalarString(@"kadamk")), (spml5149) => null), new GekkoArg((spml5150) => O.HandleString(new ScalarString(@"kadam")), (spml5150) => null), new GekkoArg((spml5151) => i5148, (spml5151) => null), new GekkoArg((spml5152) => O.Lookup(spml5152, null, null, "%slutar", null, null, new LookupSettings(), EVariableType.Var, null), (spml5152) => new ScalarString("%slutar")), new GekkoArg((spml5153) => O.Lookup(spml5153, null, null, "%slkv", null, null, new LookupSettings(), EVariableType.Var, null), (spml5153) => new ScalarString("%slkv")));

            //[[commandEnd]]25
        }


        public static readonly ScalarVal i5115 = new ScalarVal(1d);
        public static readonly ScalarVal i5118 = new ScalarVal(100d);
        public static readonly ScalarVal i5122 = new ScalarVal(100d);
        public static readonly ScalarVal i5126 = new ScalarVal(100d);
        public static readonly ScalarVal i5130 = new ScalarVal(100d);
        public static readonly ScalarVal i5131 = new ScalarVal(4d);
        public static readonly ScalarVal i5132 = new ScalarVal(1d);
        public static readonly ScalarVal i5136 = new ScalarVal(100d);
        public static readonly ScalarVal i5137 = new ScalarVal(1d);
        public static readonly ScalarVal i5138 = new ScalarVal(1d);
        public static readonly ScalarVal i5143 = new ScalarVal(1d);
        public static readonly ScalarVal i5144 = new ScalarVal(4d);
        public static readonly ScalarVal i5146 = new ScalarVal(6d);
        public static readonly ScalarVal i5147 = new ScalarVal(1d);
        public static readonly ScalarVal i5148 = new ScalarVal(1991d);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);


            //[[commandSpecial]]8
            IVariable forloop_xe7dke6cj_5116 = null;
            int counter5119 = 0;
            for (O.IterateStart(ref forloop_xe7dke6cj_5116, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("ALLE")))), null, new LookupSettings(), EVariableType.Var, null)); O.IterateContinue(forloop_xe7dke6cj_5116, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("ALLE")))), null, new LookupSettings(), EVariableType.Var, null), null, null, ref counter5119); O.IterateStep(forloop_xe7dke6cj_5116, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("ALLE")))), null, new LookupSettings(), EVariableType.Var, null), null, counter5119))
            {
                ;

                C1(smpl, p, ref forloop_xe7dke6cj_5116);

            };

            //[[commandEnd]]8


            //[[commandSpecial]]10
            IVariable forloop_xe7dke6cj_5120 = null;
            int counter5123 = 0;
            for (O.IterateStart(ref forloop_xe7dke6cj_5120, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("ALLE")))), null, new LookupSettings(), EVariableType.Var, null)); O.IterateContinue(forloop_xe7dke6cj_5120, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("ALLE")))), null, new LookupSettings(), EVariableType.Var, null), null, null, ref counter5123); O.IterateStep(forloop_xe7dke6cj_5120, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("ALLE")))), null, new LookupSettings(), EVariableType.Var, null), null, counter5123))
            {
                ;

                C2(smpl, p, ref forloop_xe7dke6cj_5120);

            };

            //[[commandEnd]]10


            //[[commandSpecial]]12
            IVariable forloop_xe7dke6cj_5124 = null;
            int counter5127 = 0;
            for (O.IterateStart(ref forloop_xe7dke6cj_5124, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("ALLE")))), null, new LookupSettings(), EVariableType.Var, null)); O.IterateContinue(forloop_xe7dke6cj_5124, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("ALLE")))), null, new LookupSettings(), EVariableType.Var, null), null, null, ref counter5127); O.IterateStep(forloop_xe7dke6cj_5124, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("ALLE")))), null, new LookupSettings(), EVariableType.Var, null), null, counter5127))
            {
                ;

                C3(smpl, p, ref forloop_xe7dke6cj_5124);

            };

            //[[commandEnd]]12


            C4(smpl, p);


            //[[commandSpecial]]15
            IVariable forloop_xe7dke6cj_5128 = null;
            int counter5133 = 0;
            for (O.IterateStart(ref forloop_xe7dke6cj_5128, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("AFT")))), null, new LookupSettings(), EVariableType.Var, null)); O.IterateContinue(forloop_xe7dke6cj_5128, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("AFT")))), null, new LookupSettings(), EVariableType.Var, null), null, null, ref counter5133); O.IterateStep(forloop_xe7dke6cj_5128, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("AFT")))), null, new LookupSettings(), EVariableType.Var, null), null, counter5133))
            {
                ;

                C5(smpl, p, ref forloop_xe7dke6cj_5128);

            };

            //[[commandEnd]]15


            //[[commandSpecial]]17
            IVariable forloop_xe7dke6cj_5134 = null;
            int counter5139 = 0;
            for (O.IterateStart(ref forloop_xe7dke6cj_5134, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("AFT")))), null, new LookupSettings(), EVariableType.Var, null)); O.IterateContinue(forloop_xe7dke6cj_5134, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("AFT")))), null, new LookupSettings(), EVariableType.Var, null), null, null, ref counter5139); O.IterateStep(forloop_xe7dke6cj_5134, O.Lookup(smpl, null, (O.scalarStringHash).Add(smpl, ((new ScalarString("listfile___"))).Add(smpl, (new ScalarString("AFT")))), null, new LookupSettings(), EVariableType.Var, null), null, counter5139))
            {
                ;

                C6(smpl, p, ref forloop_xe7dke6cj_5134);

            };

            //[[commandEnd]]17


            C7(smpl, p);



        }
    }
}
