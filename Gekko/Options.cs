/*
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2016, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Gekko
{
    public class Options
    {
        
        //!! do not use '_' inside an option -- the '_' corresponds to a blank in ANTLR        
                
        public bool bugfix_import_export = false;
        public bool bugfix_download = true;
        
        public bool bugfix_gbk = false;  //set true to activate old 7zip writer

        public bool databank_create_auto = true;
        public bool databank_create_message = true;  //this option could be removed now
        public double databank_compare_tabs = 1.0d;
        public double databank_compare_trel = 0.0001d;
        public bool databank_file_copylocal = true;
        public bool databank_file_gbk_compress = true;
        public string databank_file_gbk_version = "1.2";  //decides what kind of .gbk file is written  
        public string databank_file_gbk_internal = "databank.data"; //change to "databank.data" in Gekko 2.2        
        public bool databank_search = true;

        public int decomp_maxlag = 10;
        public int decomp_maxlead = 10;

        public bool folder = true;
        public string folder_bank = "";
        public string folder_bank1 = "";
        public string folder_bank2 = "";
        public string folder_command = "";
        public string folder_command1 = "";
        public string folder_command2 = "";
        public string folder_help = "";
        public string folder_menu = "";
        public string folder_model = "";
        public string folder_pipe = "";
        public string folder_table = "";
        public string folder_table1 = "";
        public string folder_table2 = "";
        public string folder_working = "";

        public EFreq freq = EFreq.A;

        public bool gams_fast = true; //use low-level api
        public string gams_exe_folder = "";
        public string gams_time_freq = "a";  //could be u for undated
        public string gams_time_set = "t";  //name of the time set in GAMS
        public string gams_time_prefix = "";  //prefix of time set elements, if 't' time can be for instance t0
        public double gams_time_offset = 0;  //add to the integer after prefix, for instance t0 -> 2006

        public bool gams_time_detect_auto = false;  //will test if a dim looks like time. Only possible with gams_time_prefix != "".

        //logic could be that interface assembles stuff that relates to the GUI, but also stuff like the help system which is 'passive' pages (unlike tables and menus).
        public bool interface_alias = false;  //reacts to globals.#alias list
        //public string interface_alias_file = "";
        public string interface_clipboard_decimalseparator = "period";
        public string interface_csv_decimalseparator = "period";  //has to do with Windows interface, so ok here
        public string interface_csv_delimiter = "semicolon";      //--> we put it next to the decimalseparator
        public bool interface_databank_swap = false; //a bit dangerous...
        public string interface_debug = "dialog";  //or "none"        
        public string interface_excel_language = "danish";
        public bool interface_excel_modernlook = true;
        public bool interface_help_copylocal = true;
        public bool interface_lagfix = true;
        public string interface_mode = "data";  //sim, data, mixed
        public string interface_mute = "no";  //yes, no
        public bool interface_remote = false;  //remote control via remote.gcm
        public string interface_remote_file = "";
        public bool interface_sound = false;  //overall sound switch
        public string interface_sound_type = "bowl";  //bowl, ding, notify, ring
        public int interface_sound_wait = 60; //seconds command files run to get a sound        
        public string interface_suggestions = "option"; //option or some or none or all   ---> //in the longer run: none, little, some, many, all
        public bool interface_table_operators = true;
        public int interface_zoom = 100;
        public string menu_startfile = "menu.html";
        public bool model_cache = true;  //if using cache on file or not        
        public int model_cache_max = 20;  //model options are non-solving options. How many fixed models are kept in RAM    

        public bool model_gams_dep_current = false;
        public string model_gams_dep_method = "lhs";  //lhs|eqname

        public string model_infofile = "yes";  //yes/no/temp
        public string model_type = "default";  //normal | gams

        public string plot_decimalseparator = "period";  //comma|period
        public int plot_elements_max = 200;
        public bool plot_lines_points = true;
        public bool plot_new = true;
        //public string plot_xlabels_between_truncate = "auto";     //digits|skip|digitsskip|skipdigits|none  --> when all too pressed we use label at.
        public string plot_xlabels_annual = "at"; //at|between
        public string plot_xlabels_nonannual = "between"; //at|between          
        public int plot_xlabels_digits = 4; // 4 or 2, only applies to 'between' type   
        public string plot_using = ""; //a global template


        public int print_disp_maxlines = 3; //-1 means infinite, 0 means no data shown
        public int print_elements_max = 400;
        public int print_fields_ndec = 4;
        public int print_fields_nwidth = 13;
        public int print_fields_pdec = 2;
        public int print_fields_pwidth = 8;  //to make more room for labels
        public int print_filewidth = 130;
        public string print_collapse = "none";  //avg or total or none
        public string print_freq = "pretty";  //pretty or simple
        public bool print_mulprt_lev = false;  //n
        public bool print_mulprt_abs = true;  //m
        public bool print_mulprt_pch = true;  //q
        public bool print_mulprt_gdif = false;  //mp
        public bool print_mulprt_v = false;  //gmulprt, will override the others
        public bool print_prt_abs = true;  //n
        public bool print_prt_dif = false;  //d
        public bool print_prt_pch = true;  //p
        public bool print_prt_gdif = false;  //dp        
        public int print_width = 100;  //so that eqs look ok in DISP
        public bool print_split = false;  //splits PRT x, y; into PRT x; PRT y;
        
        public string r_exe_folder = "";  //there will probably be more R options later on
        public string r_exe_path = "";  //old name, delete at some point

        public string python_exe_folder = "";  //there will probably be more Python options later on

        //DEAD!!
        public bool series_array_ignoremissing = false;  //returns 0 (or skips) a non-existing array-series


        // -------- new options regarding missings start ---------------------

        public bool series_dyn = false;

        public bool series_failsafe = false;  //with 'yes', will abort with error if a missing value is put into a series

        //public bool series_data_ignoremissing = false;  //returns 0 instead of NaN (missing) for an observation in an existing series or arrayseries.
        
        public ESeriesMissing series_normal_print_missing = ESeriesMissing.Error;
        public ESeriesMissing series_normal_calc_missing = ESeriesMissing.Error;           //for sum, zero = skip
        public ESeriesMissing series_normal_table_missing = ESeriesMissing.M;

        public ESeriesMissing series_array_print_missing = ESeriesMissing.Error;
        public ESeriesMissing series_array_calc_missing = ESeriesMissing.Error;           //for sum, zero = skip
                
        public ESeriesMissing series_data_missing = ESeriesMissing.M;  //M or Zero, last one only when accessing a series from an open databank, not in other cases. Not implemented for SIM (has its own solve option for that)

        // -------- new options regarding missings end ---------------------

        public string sheet_engine = "internal";
        public string sheet_collapse = "none";  //avg or total or none
        public string sheet_freq = "simple";  //pretty or simple
        public bool sheet_mulprt_lev = false;  //n
        public bool sheet_mulprt_abs = true;  //m
        public bool sheet_mulprt_pch = false;  //q
        public bool sheet_mulprt_gdif = false;  //mp
        public bool sheet_mulprt_v = false;  //gmulprt, will override the others
        public bool sheet_prt_abs = true;  //n
        public bool sheet_prt_dif = false;  //d
        public bool sheet_prt_pch = false;  //p
        public bool sheet_prt_gdif = false;  //dp
        public bool sheet_cols = false;
        public bool sheet_rows = true;
        public bool solve_data_create_auto = false;

        //for now, we keep it here...
        public bool solve_data_ignoremissing = false;

        public bool solve_data_init = true;
        public bool solve_data_init_growth = true; //only has effect if solve_fast = true
        public double solve_data_init_growth_min = -0.02; //only has effect if solve_fast = true. Limit: -0.01 hurts.
        public double solve_data_init_growth_max = 0.06; //only has effect if solve_fast = true. Limit: it could be 0.05 without problems. But 0.04 hurts.
        public bool solve_failsafe = false;
        public bool solve_forward_dump = false;
        public string solve_forward_fair_conv = "conv1";
        public double solve_forward_fair_conv1_abs = 0.001d; //it checks abs OR rel, so abs is set really low (for instance, interest rates have low abs value)
        public double solve_forward_fair_conv1_rel = 0.001d;
        public double solve_forward_fair_conv2_tabs = 1.0d;
        public double solve_forward_fair_conv2_trel = 0.001d;
        public double solve_forward_fair_damp = 0.0; //redefined in 2.0
        public int solve_forward_fair_itermax = 200;
        public bool solve_newton_robust = false;
        public int solve_forward_fair_itermin = 0;
        public string solve_forward_method = "fair";  //or "stacked" or "nfair" or "none"        
        public string solve_forward_nfair_conv = "conv1";
        public double solve_forward_nfair_conv1_abs = 0.001d;
        public double solve_forward_nfair_conv1_rel = 0.001d;
        public double solve_forward_nfair_conv2_tabs = 1.0d;
        public double solve_forward_nfair_conv2_trel = 0.001d;
        public double solve_forward_nfair_damp = 0.0; //redefined in 2.0    
        public int solve_forward_nfair_itermax = 200;
        public int solve_forward_nfair_itermin = 0;
        public int solve_forward_nfair_updatefreq = 100; //Or 1        
        public int solve_forward_stacked_horizon = 5;
        public string solve_forward_terminal = "const";  //or exo or growth (growth does not work at the moment)
        public string solve_forward_terminal_feed = "internal";  //or external
        public string solve_gauss_conv = "conv1";
        public double solve_gauss_conv1_abs = 0.0001d; //perhaps decrease, what about an interest rate...? It checks abs OR rel, so abs is set really low (for instance, interest rates have low abs value)
        public double solve_gauss_conv1_rel = 0.0001d;
        public double solve_gauss_conv2_tabs = 1.0d;
        public double solve_gauss_conv2_trel = 0.0001d;
        public bool solve_gauss_conv_ignorevars = true;
        public double solve_gauss_damp = 0.5;  //redefined in 2.0
        public bool solve_gauss_dump = false;
        public int solve_gauss_itermax = 200;
        public int solve_gauss_itermin = 10;
        public bool solve_gauss_reorder = false;  //false since Gekko 1.5.11. Setting true should theoretically give fewer iterations, but may also provoke some starting value problems not seen with 'false'. On the other hand, on some problems, reorder=true seems to yield convergence, so we keep it as default.
        public string solve_method = "gauss";  //gauss, newton
        public bool solve_newton_backtrack = true;
        public double solve_newton_conv_abs = 0.0001;  //this is for a sum (really RMSQ) over all equations, so it is really low for most purposes. 
        public string solve_newton_invert = "lu"; //lu or iter, lu is more precise -- only problem is that the matrix is not sparse. Should maybe find sparse LU module.
        public int solve_newton_itermax = 200;
        public int solve_newton_updatefreq = 15;  //fast steps are so fast now that we relax this from 10 -> 15
        public bool solve_print_details = false;
        public bool solve_print_iter = false;
        public bool solve_static = false;

        public string string_interpolate_format_val = "";  //"0.000" for 3 dec, "12:0.000" 12 chars wide, "12:F3" the same, "-12:0.000" left-aligned, # can be used.
        
        public int system_code_split = 20; //20 seems good
        public bool system_clone = true; //y = f(#x); #x[2] = ...; No side-effect.

        public string table_mdateformat = "";  //"danish-short" or "english-short" are possible. (could perhaps have a "-s" for small?)
        public string table_decimalseparator = "period";  //comma|period
        public bool table_thousandsseparator = false;
        public bool table_stamp = true;

        public double table_html_datawidth = 5.5;  //in 'em' units
        public double table_html_firstcolwidth = 5.5;  //in 'em' units
        public string table_html_font = "Arial";
        public double table_html_fontsize = 72;  //in %                        
        public double table_html_secondcolwidth = 5.5;  //in 'em' units
        public bool table_html_specialminus = false;

        //DEAD!!
        public bool table_ignoremissingvars = true;

        public string table_type = "html";  //txt or html
        public bool timefilter = false;
        public string timefilter_type = "hide";  //"hide" or "avg"

        public void Write()
        {
            Write("Program.options");
        }

        public void Write(string path)
        {
            G.Writeln();
            //G.writeln("------------------------------------------------------");
            Type type = typeof(Options); // Get type pointer
            FieldInfo[] fields = type.GetFields(); // Obtain all fields

            List<string> lines = new List<string>();

            foreach (var field in fields) // Loop through fields
            {
                string line = "";
                string name = field.Name; // Get string name
                string longName = "Program_options_" + name;
                string path2 = path.Replace(".", "_");
                if (!longName.Contains(path2)) continue;
                line += "option ";
                name = name.Replace("_", " ");
                object temp = field.GetValue(this); // Get value
                if (temp is int) // See if it is an integer.
                {
                    int value = (int)temp;
                    line += name + " = " + value + ";";
                }
                else if (temp is string) // See if it is a string.
                {
                    string value = temp as string;
                    if (value == "") value = "[empty]";
                    line += name + " = " + value + ";";
                }
                else if (temp is bool) // See if it is a string.
                {
                    bool value = (bool)temp;
                    string yesno = "no";
                    if (value) yesno = "yes";
                    line += name + " = " + yesno + ";";
                }
                else if (temp is double) // See if it is a string.
                {
                    double value = (double)temp;
                    string s = value.ToString();
                    s = Program.MaybeAddPointAndZero(s);
                    line += name + " = " + s + ";";
                }
                else if (temp is EFreq) // See if it is a freq.
                {
                    EFreq value = (EFreq)temp;
                    string s = "???";
                    if (value == EFreq.A) s = "a";
                    else if (value == EFreq.Q) s = "q";
                    else if (value == EFreq.M) s = "m";
                    else if (value == EFreq.U) s = "u";
                    line += name + " = " + s + ";";
                }
                else if (temp is ESeriesMissing) 
                {
                    ESeriesMissing value = (ESeriesMissing)temp;
                    string s = value.ToString().ToLower();
                    line += name + " = " + s + ";";
                }
                else
                {
                    line += name + " = ???;";
                }
                if (G.Equal(Program.options.interface_mode, "data") && line.StartsWith("option solve ", StringComparison.OrdinalIgnoreCase))
                {
                    //do nothing
                }
                else
                {
                    lines.Add(line);
                }
            }

            lines.Sort(StringComparer.InvariantCulture);
            foreach (string s in lines)
            {
                if (s.Contains("option r exe path")) continue; //renamed to folder
                G.Writeln(s);
            }
            G.Writeln();
            if (G.Equal(Program.options.interface_mode, "data"))
            {
                G.Writeln("+++ NOTE: option solve... are not listed in data-mode");
            }

            if (path == "Program.options")
            {
                StringBuilder sb = new StringBuilder();
                if (true)
                {
                    sb.AppendLine();
                    sb.AppendLine("Some tips on finding and setting options in Gekko.");
                    sb.AppendLine();
                    sb.AppendLine("It is planned to provide a graphical representation of the");
                    sb.AppendLine("different Gekko options. Until this is implemented, some tips");
                    sb.AppendLine("may be helpful.");
                    sb.AppendLine();
                    sb.AppendLine("Option explanations are in the helpfile: 'HELP option'.");
                    sb.AppendLine();
                    sb.AppendLine("If 'OPTION interface suggestions = option' is set (this is");
                    sb.AppendLine("default), a small suggestions-window will pop up when you press");
                    sb.AppendLine("[space] while writing an option command. This window will");
                    sb.AppendLine("show the different possibilities, and you may select the preferred");
                    sb.AppendLine("sub-option and press [enter] or double-click your choice. This way,");
                    sb.AppendLine("you can see what options are possible at any location in the");
                    sb.AppendLine("option tree.");
                    sb.AppendLine();
                    sb.AppendLine("To see what options are set, you may use 'OPTION ?', or you can also");
                    sb.AppendLine("have only the first level of sub-options shown, by means of for instance");
                    sb.AppendLine("'OPTION solve ?'. If you need to change a particular option, you may");
                    sb.AppendLine("copy-paste it from the listing and change the value. If unsure about");
                    sb.AppendLine("legal values for a particular option, try removing the current value");
                    sb.AppendLine("and press [space]. For instance, consider 'OPTION solve method = gauss'.");
                    sb.AppendLine("Try removing 'gauss' and press [space]. This will pop up a small window");
                    sb.AppendLine("showing the legal choices.");
                }
                Program.LinkContainer lc = new Program.LinkContainer(sb.ToString());
                Globals.linkContainer.Add(lc.counter, lc);
                G.Write("Advice on finding and setting options ");
                G.WriteLink("here", "outputtab:" + lc.counter); G.Writeln(".");
            }
        }
    }
}
