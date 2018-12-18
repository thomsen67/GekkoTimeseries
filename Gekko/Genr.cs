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

        public static void CC0(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]1
            p.SetText(@"¤10"); O.InitSmpl(smpl, p);


            O.Assignment o1 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar1 = Functions.errorhelper(smpl, O.HandleString(new ScalarString(@"1a")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%v", null, ivTmpvar1, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]1


            //[[commandStart]]2
            p.SetText(@"¤11"); O.InitSmpl(smpl, p);


            O.FunctionLookup0("procedure___x2")(smpl, p);

            //[[commandEnd]]2


            //[[commandStart]]3
            p.SetText(@"¤20"); O.InitSmpl(smpl, p);


            O.Assignment o3 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar2 = Functions.errorhelper(smpl, O.HandleString(new ScalarString(@"1b")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%v", null, ivTmpvar2, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]3


            //[[commandStart]]4
            p.SetText(@"¤21"); O.InitSmpl(smpl, p);


            O.FunctionLookup0("procedure___x5")(smpl, p);

            //[[commandEnd]]4


            //[[commandStart]]5
            p.SetText(@"¤28"); O.InitSmpl(smpl, p);


            O.Assignment o5 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar3 = Functions.errorhelper(smpl, O.HandleString(new ScalarString(@"1c")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%v", null, ivTmpvar3, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]5
        }
        public static void CC1(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]7
            p.SetText(@"¤40"); O.InitSmpl(smpl, p);


            O.Assignment o7 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar5 = Functions.errorhelper(smpl, O.HandleString(new ScalarString(@"2a")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%v", null, ivTmpvar5, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]7


            //[[commandStart]]8
            p.SetText(@"¤41"); O.InitSmpl(smpl, p);


            O.FunctionLookup0("procedure___x3")(smpl, p);

            //[[commandEnd]]8


            //[[commandStart]]9
            p.SetText(@"¤50"); O.InitSmpl(smpl, p);


            O.Assignment o9 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar6 = Functions.errorhelper(smpl, O.HandleString(new ScalarString(@"2b")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%v", null, ivTmpvar6, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]9


            //[[commandStart]]10
            p.SetText(@"¤51"); O.InitSmpl(smpl, p);


            O.FunctionLookup0("procedure___x4")(smpl, p);

            //[[commandEnd]]10


            //[[commandStart]]11
            p.SetText(@"¤60"); O.InitSmpl(smpl, p);


            O.Assignment o11 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar7 = Functions.errorhelper(smpl, O.HandleString(new ScalarString(@"2c")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%v", null, ivTmpvar7, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]11
        }
        public static void CC2(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]13
            p.SetText(@"¤70"); O.InitSmpl(smpl, p);


            O.Assignment o13 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar9 = Functions.errorhelper(smpl, O.HandleString(new ScalarString(@"3")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%v", null, ivTmpvar9, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]13
        }
        public static void CC3(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]15
            p.SetText(@"¤80"); O.InitSmpl(smpl, p);


            O.Assignment o15 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar11 = Functions.errorhelper(smpl, O.HandleString(new ScalarString(@"4")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%v", null, ivTmpvar11, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]15
        }
        public static void CC4(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]17
            p.SetText(@"¤90"); O.InitSmpl(smpl, p);


            O.Assignment o17 = new O.Assignment();
            O.AdjustT0(smpl, -1);
            IVariable ivTmpvar13 = Functions.errorhelper(smpl, O.HandleString(new ScalarString(@"5")));
            O.AdjustT0(smpl, 1);
            O.Lookup(smpl, null, null, "%v", null, ivTmpvar13, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null)
            ;

            //[[commandEnd]]17
        }

        public static void FunctionDef4()
        {

            O.PrepareUfunction(0, "procedure___x1");

            Globals.ufunctions0.Add("procedure___x1", (GekkoSmpl smpl, P p) =>

            {
                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("procedure___x1"); p.SetLastFileSentToANTLR(O.LastText("procedure___x1")); p.Deeper();
                try
                {


                    CC0(smpl, p);


                    return null;
                }
                finally
                {
                    Program.databanks.local = local0; Program.databanks.localGlobal = lg0;
                }
                p.RemoveLast();
            });

        }

        public static void FunctionDef8()
        {

            O.PrepareUfunction(0, "procedure___x2");

            Globals.ufunctions0.Add("procedure___x2", (GekkoSmpl smpl, P p) =>

            {
                Databank local6 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg6 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("procedure___x2"); p.SetLastFileSentToANTLR(O.LastText("procedure___x2")); p.Deeper();
                try
                {


                    CC1(smpl, p);


                    return null;
                }
                finally
                {
                    Program.databanks.local = local6; Program.databanks.localGlobal = lg6;
                }
                p.RemoveLast();
            });

        }

        public static void FunctionDef10()
        {

            O.PrepareUfunction(0, "procedure___x3");

            Globals.ufunctions0.Add("procedure___x3", (GekkoSmpl smpl, P p) =>

            {
                Databank local12 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg12 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("procedure___x3"); p.SetLastFileSentToANTLR(O.LastText("procedure___x3")); p.Deeper();
                try
                {


                    CC2(smpl, p);


                    return null;
                }
                finally
                {
                    Program.databanks.local = local12; Program.databanks.localGlobal = lg12;
                }
                p.RemoveLast();
            });

        }

        public static void FunctionDef12()
        {

            O.PrepareUfunction(0, "procedure___x4");

            Globals.ufunctions0.Add("procedure___x4", (GekkoSmpl smpl, P p) =>

            {
                Databank local14 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg14 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("procedure___x4"); p.SetLastFileSentToANTLR(O.LastText("procedure___x4")); p.Deeper();
                try
                {


                    CC3(smpl, p);


                    return null;
                }
                finally
                {
                    Program.databanks.local = local14; Program.databanks.localGlobal = lg14;
                }
                p.RemoveLast();
            });

        }

        public static void FunctionDef14()
        {

            O.PrepareUfunction(0, "procedure___x5");

            Globals.ufunctions0.Add("procedure___x5", (GekkoSmpl smpl, P p) =>

            {
                Databank local16 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg16 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = O.LastText("procedure___x5"); p.SetLastFileSentToANTLR(O.LastText("procedure___x5")); p.Deeper();
                try
                {


                    CC4(smpl, p);


                    return null;
                }
                finally
                {
                    Program.databanks.local = local16; Program.databanks.localGlobal = lg16;
                }
                p.RemoveLast();
            });

        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
            FunctionDef4();


            //[[commandEnd]]0

            FunctionDef8();


            //[[commandEnd]]6

            FunctionDef10();


            //[[commandEnd]]12

            FunctionDef12();


            //[[commandEnd]]14

            FunctionDef14();


            //[[commandEnd]]16



        }
    }
}
