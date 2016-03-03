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
        public static readonly ScalarVal d54 = new ScalarVal(1.0d);
        public static readonly ScalarVal d55 = new ScalarVal(0.0001d);
        public static readonly ScalarVal d56 = new ScalarVal(1.1d);
        public static readonly ScalarVal i57 = new ScalarVal(60d);
        public static readonly ScalarVal i58 = new ScalarVal(100d);
        public static readonly ScalarVal i59 = new ScalarVal(20d);
        public static readonly ScalarVal i60 = new ScalarVal(3d);
        public static readonly ScalarVal i61 = new ScalarVal(4d);
        public static readonly ScalarVal i62 = new ScalarVal(13d);
        public static readonly ScalarVal i63 = new ScalarVal(2d);
        public static readonly ScalarVal i64 = new ScalarVal(8d);
        public static readonly ScalarVal i65 = new ScalarVal(130d);
        public static readonly ScalarVal i66 = new ScalarVal(100d);
        public static readonly ScalarVal d67 = new ScalarVal(0.06d);
        public static readonly ScalarVal d68 = new ScalarVal(0.02d);
        public static readonly ScalarVal d69 = new ScalarVal(0.001d);
        public static readonly ScalarVal d70 = new ScalarVal(0.001d);
        public static readonly ScalarVal d71 = new ScalarVal(1.0d);
        public static readonly ScalarVal d72 = new ScalarVal(0.001d);
        public static readonly ScalarVal d73 = new ScalarVal(0.0d);
        public static readonly ScalarVal i74 = new ScalarVal(200d);
        public static readonly ScalarVal i75 = new ScalarVal(0d);
        public static readonly ScalarVal d76 = new ScalarVal(0.001d);
        public static readonly ScalarVal d77 = new ScalarVal(0.001d);
        public static readonly ScalarVal d78 = new ScalarVal(1.0d);
        public static readonly ScalarVal d79 = new ScalarVal(0.001d);
        public static readonly ScalarVal d80 = new ScalarVal(0.0d);
        public static readonly ScalarVal i81 = new ScalarVal(200d);
        public static readonly ScalarVal i82 = new ScalarVal(0d);
        public static readonly ScalarVal i83 = new ScalarVal(100d);
        public static readonly ScalarVal i84 = new ScalarVal(5d);
        public static readonly ScalarVal d85 = new ScalarVal(1.0d);
        public static readonly ScalarVal d86 = new ScalarVal(0.001d);
        public static readonly ScalarVal d87 = new ScalarVal(1.0d);
        public static readonly ScalarVal d88 = new ScalarVal(0.0001d);
        public static readonly ScalarVal d89 = new ScalarVal(0.5d);
        public static readonly ScalarVal i90 = new ScalarVal(200d);
        public static readonly ScalarVal i91 = new ScalarVal(10d);
        public static readonly ScalarVal d92 = new ScalarVal(0.0001d);
        public static readonly ScalarVal i93 = new ScalarVal(200d);
        public static readonly ScalarVal i94 = new ScalarVal(15d);
        public static readonly ScalarVal i95 = new ScalarVal(10d);
        public static readonly ScalarVal d96 = new ScalarVal(5.5d);
        public static readonly ScalarVal d97 = new ScalarVal(5.5d);
        public static readonly ScalarVal d98 = new ScalarVal(72.0d);
        public static readonly ScalarVal d99 = new ScalarVal(5.5d);
        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoTime t = Globals.tNull;


            p.SetText(@"¤1");
            Program.options.databank_compare_tabs = 1.0;
            G.Writeln();
            G.Writeln("option databank compare tabs = " + 1.0 + "");




            p.SetText(@"¤2");
            Program.options.databank_compare_trel = 0.0001;
            G.Writeln();
            G.Writeln("option databank compare trel = " + 0.0001 + "");




            p.SetText(@"¤3");
            Program.options.databank_create_auto = false;
            G.Writeln();
            G.Writeln("option databank create auto = " + "no" + "");




            p.SetText(@"¤4");
            Program.options.databank_create_message = true;
            G.Writeln();
            G.Writeln("option databank create message = " + "yes" + "");




            p.SetText(@"¤5");
            Program.options.databank_file_copylocal = false;
            G.Writeln();
            G.Writeln("option databank file copylocal = " + "no" + "");




            p.SetText(@"¤6");
            //Program.options.databank_file_tsdx_compress = true;
            G.Writeln();
            G.Writeln("option databank file tsdx compress = " + "yes" + "");




            p.SetText(@"¤7");
            //Program.options.databank_file_tsdx_version = O.GetString(d56);
            G.Writeln();
            G.Writeln("option databank file tsdx version = " + O.GetString(d56) + "");




            p.SetText(@"¤8");
            Program.options.databank_search = false;
            G.Writeln();
            G.Writeln("option databank search = " + "no" + "");




            p.SetText(@"¤9");
            Program.options.folder = true;
            G.Writeln();
            G.Writeln("option folder = " + "yes" + "");




        }

        public static void C1(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤10");
            Program.options.folder_bank = O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1))));
            G.Writeln();
            G.Writeln("option folder bank = " + O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1)))) + "");




            p.SetText(@"¤11");
            Program.options.folder_bank1 = O.GetString((new ScalarString("C")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Uadam16")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("bank")), t));
            G.Writeln();
            G.Writeln("option folder bank1 = " + O.GetString((new ScalarString("C")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Uadam16")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("bank")), t)) + "");




            p.SetText(@"¤12");
            Program.options.folder_bank2 = O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1))));
            G.Writeln();
            G.Writeln("option folder bank2 = " + O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1)))) + "");




            p.SetText(@"¤13");
            Program.options.folder_command = O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1))));
            G.Writeln();
            G.Writeln("option folder command = " + O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1)))) + "");




            p.SetText(@"¤14");
            Program.options.folder_command1 = O.GetString((new ScalarString("C")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Uadam16")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("command")), t));
            G.Writeln();
            G.Writeln("option folder command1 = " + O.GetString((new ScalarString("C")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Uadam16")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("command")), t)) + "");




            p.SetText(@"¤15");
            Program.options.folder_command2 = O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1))));
            G.Writeln();
            G.Writeln("option folder command2 = " + O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1)))) + "");




            p.SetText(@"¤16");
            Program.options.folder_help = O.GetString((new ScalarString("C")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Shared")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("help")), t));
            G.Writeln();
            G.Writeln("option folder help = " + O.GetString((new ScalarString("C")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Shared")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("help")), t)) + "");




            p.SetText(@"¤17");
            Program.options.folder_menu = O.GetString((new ScalarString("C")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Uadam16")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Command")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Menu")), t));
            G.Writeln();
            G.Writeln("option folder menu = " + O.GetString((new ScalarString("C")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Uadam16")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Command")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Menu")), t)) + "");
            CrossThreadStuff.RestartMenuBrowser();



            p.SetText(@"¤18");
            Program.options.folder_model = O.GetString((new ScalarString("C")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Uadam16")), t));
            G.Writeln();
            G.Writeln("option folder model = " + O.GetString((new ScalarString("C")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Uadam16")), t)) + "");




            p.SetText(@"¤19");
            Program.options.folder_pipe = O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1))));
            G.Writeln();
            G.Writeln("option folder pipe = " + O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1)))) + "");




        }

        public static void C2(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤20");
            Program.options.folder_table = O.GetString((new ScalarString("C")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Uadam16")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Table")), t));
            G.Writeln();
            G.Writeln("option folder table = " + O.GetString((new ScalarString("C")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Uadam16")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Table")), t)) + "");




            p.SetText(@"¤21");
            Program.options.folder_table1 = O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1))));
            G.Writeln();
            G.Writeln("option folder table1 = " + O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1)))) + "");




            p.SetText(@"¤22");
            Program.options.folder_table2 = O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1))));
            G.Writeln();
            G.Writeln("option folder table2 = " + O.GetString(O.MatrixCol(O.MatrixRow(O.GetTimeSeries(O.GetString(new ScalarString("[PRIMARY]")) + ":" + O.GetString((new ScalarString("empty"))), 1)))) + "");




            p.SetText(@"¤23");
            Program.options.folder_working = O.GetString((new ScalarString("c")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Uadam16")), t));
            G.Writeln();
            G.Writeln("option folder working = " + O.GetString((new ScalarString("c")).Add(new ScalarString(":\\"), t).Add((new ScalarString("Adam")), t).Add(new ScalarString("\\"), t).Add((new ScalarString("Uadam16")), t)) + "");
            CrossThreadStuff.WorkingFolder("");



            p.SetText(@"¤24");
            Program.options.freq = G.GetFreq("a");
            G.Writeln();
            G.Writeln("option freq = " + G.GetFreq("a") + "");
            ClearTS(p);
            Program.AdjustFreq();



            p.SetText(@"¤25");
            Program.options.interface_clipboard_decimalseparator = "period";
            G.Writeln();
            G.Writeln("option interface clipboard decimalseparator = " + "period" + "");




            p.SetText(@"¤26");
            Program.options.interface_csv_decimalseparator = "period";
            G.Writeln();
            G.Writeln("option interface csv decimalseparator = " + "period" + "");




            p.SetText(@"¤27");
            Program.options.interface_debug = "dialog";
            G.Writeln();
            G.Writeln("option interface debug = " + "dialog" + "");




            p.SetText(@"¤28");
            Program.options.interface_excel_language = "danish";
            G.Writeln();
            G.Writeln("option interface excel language = " + "danish" + "");




            p.SetText(@"¤29");
            Program.options.interface_excel_modernlook = true;
            G.Writeln();
            G.Writeln("option interface excel modernlook = " + "yes" + "");




        }

        public static void C3(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤30");
            Program.options.interface_help_copylocal = true;
            G.Writeln();
            G.Writeln("option interface help copylocal = " + "yes" + "");




            p.SetText(@"¤31");
            Program.options.interface_mode = "sim";
            G.Writeln();
            G.Writeln("option interface mode = " + "sim" + "");




            p.SetText(@"¤32");
            Program.options.interface_sound = false;
            G.Writeln();
            G.Writeln("option interface sound = " + "no" + "");




            p.SetText(@"¤33");
            Program.options.interface_sound_type = "bowl";
            G.Writeln();
            G.Writeln("option interface sound type = " + "bowl" + "");
            Program.PlaySound();



            p.SetText(@"¤34");
            Program.options.interface_sound_wait = 60;
            G.Writeln();
            G.Writeln("option interface sound wait = " + 60 + "");




            p.SetText(@"¤35");
            Program.options.interface_suggestions = "option";
            G.Writeln();
            G.Writeln("option interface suggestions = " + "option" + "");




            p.SetText(@"¤36");
            Program.options.interface_table_printcodes = true;
            G.Writeln();
            G.Writeln("option interface table printcodes = " + "yes" + "");




            p.SetText(@"¤37");
            Program.options.interface_zoom = 100;
            G.Writeln();
            G.Writeln("option interface zoom = " + 100 + "");
            CrossThreadStuff.Zoom();



            p.SetText(@"¤38");
            Program.options.menu_startfile = O.GetString((new ScalarString("menu")).Add(new ScalarString("."), t).Add((new ScalarString("html")), t));
            G.Writeln();
            G.Writeln("option menu startfile = " + O.GetString((new ScalarString("menu")).Add(new ScalarString("."), t).Add((new ScalarString("html")), t)) + "");
            CrossThreadStuff.RestartMenuBrowser();



            p.SetText(@"¤39");
            Program.options.model_cache = true;
            G.Writeln();
            G.Writeln("option model cache = " + "yes" + "");




        }

        public static void C4(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤40");
            Program.options.model_cache_max = 20;
            G.Writeln();
            G.Writeln("option model cache max = " + 20 + "");




            p.SetText(@"¤41");
            Program.options.plot_lines_points = true;
            G.Writeln();
            G.Writeln("option plot lines points = " + "yes" + "");




            p.SetText(@"¤42");
            Program.options.print_collapse = "none";
            G.Writeln();
            G.Writeln("option print collapse = " + "none" + "");




            p.SetText(@"¤43");
            Program.options.print_disp_maxlines = 3;
            G.Writeln();
            G.Writeln("option print disp maxlines = " + 3 + "");




            p.SetText(@"¤44");
            Program.options.print_fields_ndec = 4;
            G.Writeln();
            G.Writeln("option print fields ndec = " + 4 + "");




            p.SetText(@"¤45");
            Program.options.print_fields_nwidth = 13;
            G.Writeln();
            G.Writeln("option print fields nwidth = " + 13 + "");




            p.SetText(@"¤46");
            Program.options.print_fields_pdec = 2;
            G.Writeln();
            G.Writeln("option print fields pdec = " + 2 + "");




            p.SetText(@"¤47");
            Program.options.print_fields_pwidth = 8;
            G.Writeln();
            G.Writeln("option print fields pwidth = " + 8 + "");




            p.SetText(@"¤48");
            Program.options.print_filewidth = 130;
            G.Writeln();
            G.Writeln("option print filewidth = " + 130 + "");




            p.SetText(@"¤49");
            Program.options.print_freq = "pretty";
            G.Writeln();
            G.Writeln("option print freq = " + "pretty" + "");




        }

        public static void C5(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤50");
            Program.options.print_mulprt_abs = true;
            G.Writeln();
            G.Writeln("option print mulprt abs = " + "yes" + "");




            p.SetText(@"¤51");
            Program.options.print_mulprt_gdif = false;
            G.Writeln();
            G.Writeln("option print mulprt gdif = " + "no" + "");




            p.SetText(@"¤52");
            Program.options.print_mulprt_lev = true;
            G.Writeln();
            G.Writeln("option print mulprt lev = " + "yes" + "");




            p.SetText(@"¤53");
            Program.options.print_mulprt_pch = true;
            G.Writeln();
            G.Writeln("option print mulprt pch = " + "yes" + "");




            p.SetText(@"¤54");
            Program.options.print_mulprt_v = false;
            G.Writeln();
            G.Writeln("option print mulprt v = " + "no" + "");




            p.SetText(@"¤55");
            Program.options.print_prt_abs = true;
            G.Writeln();
            G.Writeln("option print prt abs = " + "yes" + "");




            p.SetText(@"¤56");
            Program.options.print_prt_dif = false;
            G.Writeln();
            G.Writeln("option print prt dif = " + "no" + "");




            p.SetText(@"¤57");
            Program.options.print_prt_gdif = false;
            G.Writeln();
            G.Writeln("option print prt gdif = " + "no" + "");




            p.SetText(@"¤58");
            Program.options.print_prt_pch = true;
            G.Writeln();
            G.Writeln("option print prt pch = " + "yes" + "");




            p.SetText(@"¤59");
            Program.options.print_width = 100;
            G.Writeln();
            G.Writeln("option print width = " + 100 + "");




        }

        public static void C6(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤60");
            Program.options.sheet_cols = false;
            G.Writeln();
            G.Writeln("option sheet cols = " + "no" + "");




            p.SetText(@"¤61");
            Program.options.sheet_mulprt_abs = true;
            G.Writeln();
            G.Writeln("option sheet mulprt abs = " + "yes" + "");




            p.SetText(@"¤62");
            Program.options.sheet_mulprt_gdif = false;
            G.Writeln();
            G.Writeln("option sheet mulprt gdif = " + "no" + "");




            p.SetText(@"¤63");
            Program.options.sheet_mulprt_lev = false;
            G.Writeln();
            G.Writeln("option sheet mulprt lev = " + "no" + "");




            p.SetText(@"¤64");
            Program.options.sheet_mulprt_pch = false;
            G.Writeln();
            G.Writeln("option sheet mulprt pch = " + "no" + "");




            p.SetText(@"¤65");
            Program.options.sheet_mulprt_v = false;
            G.Writeln();
            G.Writeln("option sheet mulprt v = " + "no" + "");




            p.SetText(@"¤66");
            Program.options.sheet_prt_abs = true;
            G.Writeln();
            G.Writeln("option sheet prt abs = " + "yes" + "");




            p.SetText(@"¤67");
            Program.options.sheet_prt_dif = false;
            G.Writeln();
            G.Writeln("option sheet prt dif = " + "no" + "");




            p.SetText(@"¤68");
            Program.options.sheet_prt_gdif = false;
            G.Writeln();
            G.Writeln("option sheet prt gdif = " + "no" + "");




            p.SetText(@"¤69");
            Program.options.sheet_prt_pch = false;
            G.Writeln();
            G.Writeln("option sheet prt pch = " + "no" + "");




        }

        public static void C7(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤70");
            Program.options.sheet_rows = true;
            G.Writeln();
            G.Writeln("option sheet rows = " + "yes" + "");




            p.SetText(@"¤71");
            Program.options.solve_data_create_auto = true;
            G.Writeln();
            G.Writeln("option solve data create auto = " + "yes" + "");




            p.SetText(@"¤72");
            Program.options.solve_data_ignoremissing = false;
            G.Writeln();
            G.Writeln("option solve data ignoremissing = " + "no" + "");




            p.SetText(@"¤73");
            Program.options.solve_data_init = true;
            G.Writeln();
            G.Writeln("option solve data init = " + "yes" + "");




            p.SetText(@"¤74");
            Program.options.solve_data_init_growth = true;
            G.Writeln();
            G.Writeln("option solve data init growth = " + "yes" + "");




            p.SetText(@"¤75");
            Program.options.solve_data_init_growth_max = 0.06;
            G.Writeln();
            G.Writeln("option solve data init growth max = " + 0.06 + "");




            p.SetText(@"¤76");
            Program.options.solve_data_init_growth_min = 0.02;
            G.Writeln();
            G.Writeln("option solve data init growth min = " + 0.02 + "");




            p.SetText(@"¤77");
            Program.options.solve_failsafe = false;
            G.Writeln();
            G.Writeln("option solve failsafe = " + "no" + "");




            p.SetText(@"¤78");
            Program.options.solve_forward_dump = false;
            G.Writeln();
            G.Writeln("option solve forward dump = " + "no" + "");




            p.SetText(@"¤79");
            Program.options.solve_forward_fair_conv = "conv1";
            G.Writeln();
            G.Writeln("option solve forward fair conv = " + "conv1" + "");




        }

        public static void C8(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤80");
            Program.options.solve_forward_fair_conv1_abs = 0.001;
            G.Writeln();
            G.Writeln("option solve forward fair conv1 abs = " + 0.001 + "");




            p.SetText(@"¤81");
            Program.options.solve_forward_fair_conv1_rel = 0.001;
            G.Writeln();
            G.Writeln("option solve forward fair conv1 rel = " + 0.001 + "");




            p.SetText(@"¤82");
            Program.options.solve_forward_fair_conv2_tabs = 1.0;
            G.Writeln();
            G.Writeln("option solve forward fair conv2 tabs = " + 1.0 + "");




            p.SetText(@"¤83");
            Program.options.solve_forward_fair_conv2_trel = 0.001;
            G.Writeln();
            G.Writeln("option solve forward fair conv2 trel = " + 0.001 + "");




            p.SetText(@"¤84");
            Program.options.solve_forward_fair_damp = 0.0;
            G.Writeln();
            G.Writeln("option solve forward fair damp = " + 0.0 + "");
            G.Writeln(); G.Writeln("+++ NOTE: Damping in Gekko 2.0 should be set to 1 minus damping in Gekko 1.8.");



            p.SetText(@"¤85");
            Program.options.solve_forward_fair_itermax = 200;
            G.Writeln();
            G.Writeln("option solve forward fair itermax = " + 200 + "");




            p.SetText(@"¤86");
            Program.options.solve_forward_fair_itermin = 0;
            G.Writeln();
            G.Writeln("option solve forward fair itermin = " + 0 + "");




            p.SetText(@"¤87");
            Program.options.solve_forward_method = "fair";
            G.Writeln();
            G.Writeln("option solve forward method = " + "fair" + "");




            p.SetText(@"¤88");
            Program.options.solve_forward_nfair_conv = "conv1";
            G.Writeln();
            G.Writeln("option solve forward nfair conv = " + "conv1" + "");




            p.SetText(@"¤89");
            Program.options.solve_forward_nfair_conv1_abs = 0.001;
            G.Writeln();
            G.Writeln("option solve forward nfair conv1 abs = " + 0.001 + "");




        }

        public static void C9(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤90");
            Program.options.solve_forward_nfair_conv1_rel = 0.001;
            G.Writeln();
            G.Writeln("option solve forward nfair conv1 rel = " + 0.001 + "");




            p.SetText(@"¤91");
            Program.options.solve_forward_nfair_conv2_tabs = 1.0;
            G.Writeln();
            G.Writeln("option solve forward nfair conv2 tabs = " + 1.0 + "");




            p.SetText(@"¤92");
            Program.options.solve_forward_nfair_conv2_trel = 0.001;
            G.Writeln();
            G.Writeln("option solve forward nfair conv2 trel = " + 0.001 + "");




            p.SetText(@"¤93");
            Program.options.solve_forward_nfair_damp = 0.0;
            G.Writeln();
            G.Writeln("option solve forward nfair damp = " + 0.0 + "");
            G.Writeln(); G.Writeln("+++ NOTE: Damping in Gekko 2.0 should be set to 1 minus damping in Gekko 1.8.");



            p.SetText(@"¤94");
            Program.options.solve_forward_nfair_itermax = 200;
            G.Writeln();
            G.Writeln("option solve forward nfair itermax = " + 200 + "");




            p.SetText(@"¤95");
            Program.options.solve_forward_nfair_itermin = 0;
            G.Writeln();
            G.Writeln("option solve forward nfair itermin = " + 0 + "");




            p.SetText(@"¤96");
            Program.options.solve_forward_nfair_updatefreq = 100;
            G.Writeln();
            G.Writeln("option solve forward nfair updatefreq = " + 100 + "");




            p.SetText(@"¤97");
            Program.options.solve_forward_stacked_horizon = 5;
            G.Writeln();
            G.Writeln("option solve forward stacked horizon = " + 5 + "");




            p.SetText(@"¤98");
            Program.options.solve_forward_terminal = "const";
            G.Writeln();
            G.Writeln("option solve forward terminal = " + "const" + "");




            p.SetText(@"¤99");
            Program.options.solve_forward_terminal_feed = "internal";
            G.Writeln();
            G.Writeln("option solve forward terminal feed = " + "internal" + "");




        }

        public static void C10(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤100");
            Program.options.solve_gauss_conv = "conv1";
            G.Writeln();
            G.Writeln("option solve gauss conv = " + "conv1" + "");




            p.SetText(@"¤101");
            Program.options.solve_gauss_conv_ignorevars = true;
            G.Writeln();
            G.Writeln("option solve gauss conv ignorevars = " + "yes" + "");




            p.SetText(@"¤102");
            Program.options.solve_gauss_conv1_abs = 1.0;
            G.Writeln();
            G.Writeln("option solve gauss conv1 abs = " + 1.0 + "");




            p.SetText(@"¤103");
            Program.options.solve_gauss_conv1_rel = 0.001;
            G.Writeln();
            G.Writeln("option solve gauss conv1 rel = " + 0.001 + "");




            p.SetText(@"¤104");
            Program.options.solve_gauss_conv2_tabs = 1.0;
            G.Writeln();
            G.Writeln("option solve gauss conv2 tabs = " + 1.0 + "");




            p.SetText(@"¤105");
            Program.options.solve_gauss_conv2_trel = 0.0001;
            G.Writeln();
            G.Writeln("option solve gauss conv2 trel = " + 0.0001 + "");




            p.SetText(@"¤106");
            Program.options.solve_gauss_damp = 0.5;
            G.Writeln();
            G.Writeln("option solve gauss damp = " + 0.5 + "");
            G.Writeln(); G.Writeln("+++ NOTE: Damping in Gekko 2.0 should be set to 1 minus damping in Gekko 1.8.");



            p.SetText(@"¤107");
            Program.options.solve_gauss_dump = false;
            G.Writeln();
            G.Writeln("option solve gauss dump = " + "no" + "");




            p.SetText(@"¤108");
            Program.options.solve_gauss_itermax = 200;
            G.Writeln();
            G.Writeln("option solve gauss itermax = " + 200 + "");




            p.SetText(@"¤109");
            Program.options.solve_gauss_itermin = 10;
            G.Writeln();
            G.Writeln("option solve gauss itermin = " + 10 + "");




        }

        public static void C11(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤110");
            Program.options.solve_gauss_reorder = false;
            G.Writeln();
            G.Writeln("option solve gauss reorder = " + "no" + "");
            G.Writeln(); G.Writeln("+++ NOTE: Reorder: you must issue a MODEL statement afterwards, for this option to take effect."); G.Writeln("+++       (In command files, place this option before any MODEL statements).");



            p.SetText(@"¤111");
            Program.options.solve_method = "gauss";
            G.Writeln();
            G.Writeln("option solve method = " + "gauss" + "");




            p.SetText(@"¤112");
            Program.options.solve_newton_backtrack = true;
            G.Writeln();
            G.Writeln("option solve newton backtrack = " + "yes" + "");




            p.SetText(@"¤113");
            Program.options.solve_newton_conv_abs = 0.0001;
            G.Writeln();
            G.Writeln("option solve newton conv abs = " + 0.0001 + "");




            p.SetText(@"¤114");
            Program.options.solve_newton_invert = "lu";
            G.Writeln();
            G.Writeln("option solve newton invert = " + "lu" + "");




            p.SetText(@"¤115");
            Program.options.solve_newton_itermax = 200;
            G.Writeln();
            G.Writeln("option solve newton itermax = " + 200 + "");




            p.SetText(@"¤116");
            Program.options.solve_newton_updatefreq = 15;
            G.Writeln();
            G.Writeln("option solve newton updatefreq = " + 15 + "");




            p.SetText(@"¤117");
            Program.options.solve_print_details = false;
            G.Writeln();
            G.Writeln("option solve print details = " + "no" + "");




            p.SetText(@"¤118");
            Program.options.solve_print_iter = false;
            G.Writeln();
            G.Writeln("option solve print iter = " + "no" + "");




            p.SetText(@"¤119");
            Program.options.solve_static = false;
            G.Writeln();
            G.Writeln("option solve static = " + "no" + "");




        }

        public static void C12(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤120");
            Program.options.system_code_split = 10;
            G.Writeln();
            G.Writeln("option system code split = " + 10 + "");




            p.SetText(@"¤121");
            Program.options.table_html_datawidth = 5.5;
            G.Writeln();
            G.Writeln("option table html datawidth = " + 5.5 + "");




            p.SetText(@"¤122");
            Program.options.table_html_firstcolwidth = 5.5;
            G.Writeln();
            G.Writeln("option table html firstcolwidth = " + 5.5 + "");




            p.SetText(@"¤123");
            Program.options.table_html_font = O.GetString((new ScalarString("Arial")));
            G.Writeln();
            G.Writeln("option table html font = " + O.GetString((new ScalarString("Arial"))) + "");




            p.SetText(@"¤124");
            Program.options.table_html_fontsize = 72.0;
            G.Writeln();
            G.Writeln("option table html fontsize = " + 72.0 + "");




            p.SetText(@"¤125");
            Program.options.table_html_secondcolwidth = 5.5;
            G.Writeln();
            G.Writeln("option table html secondcolwidth = " + 5.5 + "");




            p.SetText(@"¤126");
            Program.options.table_html_specialminus = false;
            G.Writeln();
            G.Writeln("option table html specialminus = " + "no" + "");




            p.SetText(@"¤127");
            Program.options.table_ignoremissingvars = true;
            G.Writeln();
            G.Writeln("option table ignoremissingvars = " + "yes" + "");




            p.SetText(@"¤128");
            Program.options.table_type = "html";
            G.Writeln();
            G.Writeln("option table type = " + "html" + "");




            p.SetText(@"¤129");
            Program.options.timefilter = false;
            G.Writeln();
            G.Writeln("option timefilter = " + "no" + "");




        }

        public static void C13(P p)
        {

            GekkoTime t = Globals.tNull;

            p.SetText(@"¤130");
            Program.options.timefilter_type = "hide";
            G.Writeln();
            G.Writeln("option timefilter type = " + "hide" + "");
            G.Writeln(); G.Writeln("+++ NOTE: Timefilter type = 'avg' only works for PRT and MULPRT.");



        }


        public static void CodeLines(P p)
        {
            GekkoTime t = Globals.tNull;

            C0(p);

            C1(p);

            C2(p);

            C3(p);

            C4(p);

            C5(p);

            C6(p);

            C7(p);

            C8(p);

            C9(p);

            C10(p);

            C11(p);

            C12(p);

            C13(p);



        }
    }
}

