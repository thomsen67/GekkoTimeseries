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
        public static GekkoTime globalGekkoTimeIterator = Globals.tNull;
        public static readonly ScalarVal i1 = new ScalarVal(2001d);
        public static readonly ScalarVal i2 = new ScalarVal(2003d);
        public static readonly ScalarVal i5 = new ScalarVal(1d);
        public static readonly ScalarVal i6 = new ScalarVal(2d);
        public static readonly ScalarVal i7 = new ScalarVal(3d);
        public static readonly ScalarVal i10 = new ScalarVal(11d);
        public static readonly ScalarVal i11 = new ScalarVal(12d);
        public static readonly ScalarVal i12 = new ScalarVal(13d);
        public static readonly ScalarVal i15 = new ScalarVal(4d);
        public static readonly ScalarVal i18 = new ScalarVal(12d);
        public static readonly ScalarVal i23 = new ScalarVal(100d);
        public static readonly ScalarVal i26 = new ScalarVal(100d);
        public static readonly ScalarVal i29 = new ScalarVal(100d);
        public static readonly ScalarVal i32 = new ScalarVal(100d);
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);


            p.SetText(@"¤0"); O.InitSmpl(smpl);
            O.Reset o0 = new O.Reset();
            o0.p = p; o0.Exe(null);




            p.SetText(@"¤2"); O.InitSmpl(smpl);
            O.Time o1 = new O.Time();
            o1.t1 = O.ConvertToDate(i1, O.GetDateChoices.FlexibleStart);
            ;
            o1.t2 = O.ConvertToDate(i2, O.GetDateChoices.FlexibleEnd);
            ;

            o1.Exe();




            p.SetText(@"¤0"); O.InitSmpl(smpl);
            IVariable ivTmpvar3 = O.ListDefHelper(i5, i6, i7);
            for (int iSmpl4 = 0; iSmpl4 < int.MaxValue; iSmpl4++)
            {
                O.Lookup(smpl, null, null, "xx1", null, ivTmpvar3, true)
                ;

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl4); else break;
            };




            p.SetText(@"¤0"); O.InitSmpl(smpl);
            IVariable ivTmpvar8 = O.ListDefHelper(i10, i11, i12);
            for (int iSmpl9 = 0; iSmpl9 < int.MaxValue; iSmpl9++)
            {
                O.Lookup(smpl, null, null, "xx2", null, ivTmpvar8, true)
                ;

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl9); else break;
            };




            p.SetText(@"¤5"); O.InitSmpl(smpl);
            Program.options.freq = G.GetFreq("q");
            G.Writeln();
            G.Writeln("option freq = " + G.GetFreq("q") + "");
            ClearTS(p);
            Program.AdjustFreq();



            p.SetText(@"¤0"); O.InitSmpl(smpl);
            IVariable ivTmpvar13 = i15;
            for (int iSmpl14 = 0; iSmpl14 < int.MaxValue; iSmpl14++)
            {
                O.Lookup(smpl, null, null, "xx3", null, ivTmpvar13, true)
                ;

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl14); else break;
            };




            p.SetText(@"¤7"); O.InitSmpl(smpl);
            Program.options.freq = G.GetFreq("m");
            G.Writeln();
            G.Writeln("option freq = " + G.GetFreq("m") + "");
            ClearTS(p);
            Program.AdjustFreq();



            p.SetText(@"¤0"); O.InitSmpl(smpl);
            IVariable ivTmpvar16 = i18;
            for (int iSmpl17 = 0; iSmpl17 < int.MaxValue; iSmpl17++)
            {
                O.Lookup(smpl, null, null, "xx4", null, ivTmpvar16, true)
                ;

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl17); else break;
            };




            p.SetText(@"¤0"); O.InitSmpl(smpl);
            IVariable ivTmpvar19 = O.ListDefHelper(new ScalarString(ScalarString.SubstituteScalarsInString(@"xx3", true, false)), new ScalarString(ScalarString.SubstituteScalarsInString(@"xx4", true, false)));
            for (int iSmpl20 = 0; iSmpl20 < int.MaxValue; iSmpl20++)
            {
                O.Lookup(smpl, null, null, "#m", null, ivTmpvar19, true)
                ;

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl20); else break;
            };




        }

        public static void C1(P p)
        {

            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

            p.SetText(@"¤13"); O.InitSmpl(smpl);
            O.Write o9 = new O.Write();

            o9.fileName = O.ConvertToString((new ScalarString("slet", true, false)));

            o9.type = @"write"; o9.Exe();




            p.SetText(@"¤14"); O.InitSmpl(smpl);
            ClearTS(p);
            O.Read o10 = new O.Read();
            o10.p = p;
            o10.type = @"read";
            o10.fileName = O.ConvertToString((new ScalarString("slet", true, false)));


            o10.Exe();




            p.SetText(@"¤15"); O.InitSmpl(smpl);
            Program.options.freq = G.GetFreq("a");
            G.Writeln();
            G.Writeln("option freq = " + G.GetFreq("a") + "");
            ClearTS(p);
            Program.AdjustFreq();



            p.SetText(@"¤0"); O.InitSmpl(smpl);
            IVariable ivTmpvar21 = O.Add(smpl, O.Lookup(smpl, null, null, "xx1", null, null, false), i23);
            for (int iSmpl22 = 0; iSmpl22 < int.MaxValue; iSmpl22++)
            {
                O.Lookup(smpl, null, null, "xx1", null, ivTmpvar21, true)
                ;

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl22); else break;
            };




            p.SetText(@"¤0"); O.InitSmpl(smpl);
            IVariable ivTmpvar24 = O.Add(smpl, O.Lookup(smpl, null, null, "xx2", null, null, false), i26);
            for (int iSmpl25 = 0; iSmpl25 < int.MaxValue; iSmpl25++)
            {
                O.Lookup(smpl, null, null, "xx2", null, ivTmpvar24, true)
                ;

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl25); else break;
            };




            p.SetText(@"¤18"); O.InitSmpl(smpl);
            Program.options.freq = G.GetFreq("q");
            G.Writeln();
            G.Writeln("option freq = " + G.GetFreq("q") + "");
            ClearTS(p);
            Program.AdjustFreq();



            p.SetText(@"¤0"); O.InitSmpl(smpl);
            IVariable ivTmpvar27 = O.Add(smpl, O.Lookup(smpl, null, null, "xx3", "q", null, false), i29);
            for (int iSmpl28 = 0; iSmpl28 < int.MaxValue; iSmpl28++)
            {
                O.Lookup(smpl, null, null, "xx3", null, ivTmpvar27, true)
                ;

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl28); else break;
            };




            p.SetText(@"¤20"); O.InitSmpl(smpl);
            Program.options.freq = G.GetFreq("m");
            G.Writeln();
            G.Writeln("option freq = " + G.GetFreq("m") + "");
            ClearTS(p);
            Program.AdjustFreq();



            p.SetText(@"¤0"); O.InitSmpl(smpl);
            IVariable ivTmpvar30 = O.Add(smpl, O.Lookup(smpl, null, null, "xx4", "m", null, false), i32);
            for (int iSmpl31 = 0; iSmpl31 < int.MaxValue; iSmpl31++)
            {
                O.Lookup(smpl, null, null, "xx4", null, ivTmpvar30, true)
                ;

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl31); else break;
            };




            p.SetText(@"¤0"); O.InitSmpl(smpl);
            List m33 = null; try
            {
                m33 = new List();
                for (smpl.bankNumber = 0; smpl.bankNumber < 2; smpl.bankNumber++)
                {
                    m33.Add(O.ListDefHelper(O.Lookup(smpl, null, null, "xx1", null, null, false), O.Lookup(smpl, null, null, "xx2", null, null, false), O.Lookup(smpl, null, (O.Lookup(smpl, null, null, "#m", null, null, false)), null, false)));
                }
            }
            finally
            {
                smpl.bankNumber = 0;
            }
            for (int iSmpl34 = 0; iSmpl34 < int.MaxValue; iSmpl34++)
            {
                O.Print(smpl, m33);

                if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl34); else break;
            }



        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl);

            C0(p);

            C1(p);



        }
    }
}
