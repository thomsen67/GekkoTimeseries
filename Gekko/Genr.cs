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
            p.SetText(@"¤2"); O.InitSmpl(smpl, p);


            Program.Tell(O.ConvertToString(O.HandleString(new ScalarString(@"1"))), false);
            //[[commandEnd]]1
        }
        public static void CC1(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]4
            p.SetText(@"¤7"); O.InitSmpl(smpl, p);


            Program.Tell(O.ConvertToString(O.HandleString(new ScalarString(@"2"))), false);
            //[[commandEnd]]4
        }

        public static void FunctionDef7()
        {

            O.PrepareUfunction(0, "procedure___proc1");

            Globals.ufunctions0.Add("procedure___proc1", (GekkoSmpl smpl, P p) =>

            {
                Databank local0 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg0 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = "procedure/function: procedure___proc1"; p.SetLastFileSentToANTLR("procedure/function: procedure___proc1"); p.Deeper();
                try
                {


                    CC0(smpl, p);

                    O.FunctionLookup0("procedure___proc2")(smpl, p);

        //[[commandEnd]]2


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

            O.PrepareUfunction(0, "procedure___proc2");

            Globals.ufunctions0.Add("procedure___proc2", (GekkoSmpl smpl, P p) =>

            {
                Databank local3 = Program.databanks.local;
                Program.databanks.local = new Databank("Local"); LocalGlobal lg3 = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); p.lastFileSentToANTLR = "procedure/function: procedure___proc2"; p.SetLastFileSentToANTLR("procedure/function: procedure___proc2"); p.Deeper();
                try
                {


                    CC1(smpl, p);

                    O.FunctionLookup0("procedure___proc3")(smpl, p);

        //[[commandEnd]]5


        return null;
                }
                finally
                {
                    Program.databanks.local = local3; Program.databanks.localGlobal = lg3;
                }
                p.RemoveLast();
            });

        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);
            FunctionDef7();


            //[[commandEnd]]0

            FunctionDef8();


            //[[commandEnd]]3

            O.FunctionLookup0("procedure___proc1")(smpl, p);

            //[[commandEnd]]6



        }
    }
}
