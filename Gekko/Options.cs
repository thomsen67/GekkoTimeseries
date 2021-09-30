/*
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2021, Thomas Thomsen, T-T Analyse.

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
using System.Linq;

namespace Gekko
{
    public class Options
    {
        //These options need to be callable in a fast way, so it would be very inefficient let them all be
        //IVariables. In that case, for a simple boolean yes/no, we could not just check true/false, but would
        //have to unbox the IVariable, and see if it is "yes" or "no". Inside a tight loop, this would be 
        //hopeless. Therefore, all these option variables are C# primitives.

        //bugfix options (options starting with "bugfix_" are not shown in user manual or in "option?"
        public bool bugfix_import_export = false;  //not mentioned in help                     
        public bool bugfix_missing = true;  //not mentioned in help. If option true, m()==m() will be true, and m()<>m() false for series comparison        
        // ---
        public bool databank_create_auto = true;             
        public bool databank_file_copylocal = true;
        public bool databank_file_gbk_compress = true;
        public string databank_file_gbk_version = "1.2";  //decides what kind of .gbk file is written  
        public string databank_file_gbk_internal = "databank.data"; //change to "databank.data" in Gekko 2.2        
        public bool databank_search = true;
        // ---
        public int decomp_maxlag = 10;
        public int decomp_maxlead = 10;
        // ---
        public int fit_ols_rekur_dfmin = 10;
        // ---
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
        // ---
        public EFreq freq = EFreq.A;
        // ---
        public string gams_exe_folder = "";
        public bool gams_fast = true; //use low-level api        
        public bool gams_time_detect_auto = false;  //will test if a dim looks like time. Only possible with gams_time_prefix != "".
        public string gams_time_freq = "a";  //could be u for undated
        public double gams_time_offset = 0;  //add to the integer after prefix, for instance t0 -> 2006
        public string gams_time_prefix = "";  //prefix of time set elements, if 't' time can be for instance t0
        public string gams_time_set = "t";  //name of the time set in GAMS                
        // --- interface assembles stuff that relates to the GUI, but also stuff like the help system which is 'passive' pages (unlike tables and menus).        
        public bool interface_alias = false;  //reacts to globals.#alias list        
        public string interface_clipboard_decimalseparator = "period";
        public string interface_csv_decimalseparator = "period";  //has to do with Windows interface, so ok here
        public string interface_csv_delimiter = "semicolon";      //--> we put it next to the decimalseparator
        public int interface_csv_ndec = 100;
        public int interface_csv_pdec = 100;        
        public string interface_debug = "dialog";  //or "none"  
        public string interface_edit_style = "gekko";  // gekko | gekko2 | rs | rs2
        public string interface_excel_language = "danish";
        public bool interface_excel_modernlook = true;
        public bool interface_help_copylocal = true;        
        public string interface_mode = "mixed";  //sim, data, mixed. Mixed since 3.1.12.
        public string interface_mute = "no";  //yes, no
        public bool interface_remote = false;  //remote control via remote.gcm
        public string interface_remote_file = "";
        public bool interface_sound = false;  //overall sound switch
        public string interface_sound_type = "bowl";  //bowl, ding, notify, ring
        public int interface_sound_wait = 60; //seconds command files run to get a sound        
        public string interface_suggestions = "option"; //option or some or none or all   ---> //in the longer run: none, little, some, many, all
        public bool interface_table_operators = true;
        public int interface_zoom = 100;
        // ---
        public bool library_cache = true;  //if using cache on file or not        
        // ---
        public string menu_startfile = "menu.html";
        // ---
        public bool model_cache = true;  //if using cache on file or not        
        public int model_cache_max = 20;  //model options are non-solving options. How many fixed models are kept in RAM    
        public bool model_gams_dep_current = false;
        public string model_gams_dep_method = "lhs";  //lhs|eqname
        public string model_infofile = "yes";  //yes/no/temp
        public string model_type = "default";  //normal | gams
        // ---
        public string plot_decimalseparator = "period";  //comma|period
        public int plot_elements_max = 200;
        public bool plot_lines_points = true;
        public string plot_using = ""; //a global template
        public string plot_xlabels_annual = "at"; //at|between
        public string plot_xlabels_nonannual = "between"; //at|between          
        public int plot_xlabels_digits = 4; // 4 or 2, only applies to 'between' type   
        // ---
        public string print_collapse = "none";  //avg or total or none
        public int print_disp_maxlines = 3; //-1 means infinite, 0 means no data shown
        public int print_elements_max = 400;
        public int print_fields_ndec = 4;
        public int print_fields_nwidth = 13;
        public int print_fields_pdec = 2;
        public int print_fields_pwidth = 8;  //to make more room for labels
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
        // ---
        public string python_exe_folder = "";  //there will probably be more Python options later on
        // ---
        public string r_exe_folder = "";  //there will probably be more R options later on        
        // ---
        public bool? series_dyn = null;  //must be able to attain null value. After an error, null is set. And after a BLOCK series dyn; ... ; END;, it will also be null.
        public bool series_dyn_check = true;
        public bool series_failsafe = false;  //with 'yes', will abort with error if a missing value is put into a series
        public ESeriesMissing series_normal_print_missing = ESeriesMissing.Error;
        public ESeriesMissing series_normal_calc_missing = ESeriesMissing.Error;           //for sum, zero = skip
        public ESeriesMissing series_normal_table_missing = ESeriesMissing.M;
        public ESeriesMissing series_array_print_missing = ESeriesMissing.Error;
        public ESeriesMissing series_array_calc_missing = ESeriesMissing.Error;           //for sum, zero = skip                
        public ESeriesMissing series_array_table_missing = ESeriesMissing.Error;          //not used at the moment
        public ESeriesMissing series_data_missing = ESeriesMissing.M;  //M or Zero, last one only when accessing a series from an open databank, not in other cases. Not implemented for SIM (has its own solve option for that)
        // ---
        public string sheet_collapse = "none";  //avg or total or none
        public string sheet_engine = "internal";        
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
        // ---
        public bool solve_data_create_auto = false;
        public bool solve_data_ignoremissing = false;  //for now, keep it here
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
        public int solve_forward_fair_itermin = 0;
        public string solve_forward_method = "fair";  //or "nfair" or "none"  //"stacked" is removed   
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
        public bool solve_newton_robust = false;
        public int solve_newton_updatefreq = 15;  //fast steps are so fast now that we relax this from 10 -> 15
        public bool solve_print_details = false;
        public bool solve_print_iter = false;
        public bool solve_static = false;
        // ---
        public string string_interpolate_format_val = ""; //"0.000" for 3 dec, "12:0.000" 12 chars wide, "12:F3" the same, "-12:0.000" left-aligned, # can be used. //"0.000" for 3 dec, "12:0.000" 12 chars wide, "12:F3" the same, "-12:0.000" left-aligned, # can be used.
        // ---
        public int system_code_split = 20; //20 seems good
        public bool system_clone = true; //y = f(#x); #x[2] = ...; No side-effect.
        // ---
        public string table_decimalseparator = "period";  //comma|period        
        public double table_html_datawidth = 5.5;  //in 'em' units
        public double table_html_firstcolwidth = 5.5;  //in 'em' units
        public string table_html_font = "Arial";
        public double table_html_fontsize = 72;  //in %                        
        public double table_html_secondcolwidth = 5.5;  //in 'em' units
        public bool table_html_specialminus = false;
        public bool table_ignoremissingvars = true;   //obsolete, delete in 3.3.x versions			
        public string table_mdateformat = "";  //"danish-short" or "english-short" are possible. (could perhaps have a "-s" for small?)
        public bool table_stamp = true;
        public bool table_thousandsseparator = false;
        public string table_type = "html";  //txt or html
        // ---
        public bool timefilter = false;
        public string timefilter_type = "hide";  //"hide" or "avg"


        public static List<List<string>> Syntax()
        {
            //All these have corresponding methods like O.XBool(), O.XString(), ... , cf. #jkafjkaddasfas
            //Also, these names are used in ParserGekWalkASTAndEmit, cf. #jkafjkaddasfas
            //Change all 3 places if a name is changed, or a new type is introduced.
            
            List<List<string>> rv = new List<List<string>>();

            List<string> types = new List<string>();
            types.Add(Globals.xbool);
            types.Add(Globals.xstring);
            types.Add(Globals.xint); // >= 0
            types.Add(Globals.xval);
            types.Add(Globals.xval2String);
            types.Add(Globals.xnameOrString);
            types.Add(Globals.xnameOrStringOrFilename);            
            types.Add(Globals.xnameOrString2Freq);
            types.Add(Globals.xoptionSeriesMissing);
            types.Add(Globals.xsint);  //signed int

            void Add(params string[] ss)
            {
                //input array elements will be trimmed (and first element is set lowercase)
                if (!types.Contains(ss[1]))
                {
                    new Error("Option type error");
                }
                List<string> ss5 = new List<string>();
                string w = ss[0].Trim().ToLower();
                if (w.Contains("  "))
                {
                    new Error("Option type error, too many blanks");
                }
                //if (w.Contains("("))
                //{
                //    string[] sss = w.Split(' ');
                //    for(int i =0;i<sss.Length;i++)

                //}
                ss5.Add(w); //is also lowercase
                for (int i = 1; i < ss.Length; i++) ss5.Add(ss[i].Trim());
                rv.Add(ss5);               

            }

            void Alias(string s1, string s2)
            {
                Globals.listSyntaxAlias.Add(new List<string>() { s1.ToLower().Trim(), s2.ToLower().Trim() });
            }

            //NOTE: the first string is written like this:
            // capitals (only so it is easier to see visually)
            // 1 blank to separate idents                        
            // option folder <enter> suggests = , but after =, there is bool but also other options.
            
            Add("BUGFIX IMPORT EXPORT", Globals.xbool);
            Add("BUGFIX MISSING", Globals.xbool);
            Add("DATABANK CREATE AUTO", Globals.xbool);
            Add("DATABANK FILE COPYLOCAL", Globals.xbool);
            Add("DATABANK FILE GBK COMPRESS", Globals.xbool);
            Add("DATABANK FILE GBK INTERNAL", Globals.xnameOrStringOrFilename);
            Add("DATABANK FILE GBK VERSION", Globals.xval2String);            
            Add("DATABANK SEARCH", Globals.xbool);
            Add("DECOMP MAXLAG", Globals.xint);
            Add("DECOMP MAXLEAD", Globals.xint);
            Add("FIT OLS REKUR DFMIN", Globals.xint);
            Add("FOLDER", Globals.xbool);
            Add("FOLDER BANK", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER BANK1", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER BANK2", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER COMMAND", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER COMMAND1", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER COMMAND2", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER HELP", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER MENU", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER MODEL", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER PIPE", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER TABLE", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER TABLE1", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER TABLE2", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER WORKING", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FREQ", Globals.xnameOrString2Freq, "a", "q", "m", "w", "d", "u");
            Add("GAMS EXE FOLDER", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("GAMS FAST", Globals.xbool);
            Add("GAMS TIME DETECT AUTO", Globals.xbool);
            Add("GAMS TIME FREQ", Globals.xnameOrString);
            Add("GAMS TIME OFFSET", Globals.xint);
            Add("GAMS TIME PREFIX", Globals.xnameOrString);
            Add("GAMS TIME SET", Globals.xnameOrString);
            Add("INTERFACE ALIAS", Globals.xbool);
            Add("INTERFACE CLIPBOARD DECIMALSEPARATOR", Globals.xnameOrString, "period", "comma");    //#kljsdfasfdlkj
            Add("INTERFACE CSV DECIMALSEPARATOR", Globals.xnameOrString, "period", "comma");          //#kljsdfasfdlkj
            Add("INTERFACE CSV DELIMITER", Globals.xnameOrString, "semicolon", "comma");
            Add("INTERFACE CSV NDEC", Globals.xint);
            Add("INTERFACE CSV PDEC", Globals.xint);
            Add("INTERFACE DEBUG", Globals.xnameOrString, "none", "dialog");
            Add("INTERFACE EDIT STYLE", Globals.xnameOrString, "gekko", "gekko2", "rstudio", "rstudio2");
            Add("INTERFACE EXCEL LANGUAGE", Globals.xnameOrString, "danish", "english");
            Add("INTERFACE EXCEL MODERNLOOK", Globals.xbool);
            Add("INTERFACE HELP COPYLOCAL", Globals.xbool);
            Add("INTERFACE MODE", Globals.xnameOrString, "mixed", "sim", "data");
            Add("INTERFACE MUTE", Globals.xnameOrString, Globals.xbool);
            Add("INTERFACE REMOTE", Globals.xbool);
            Add("INTERFACE REMOTE FILE", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("INTERFACE SOUND", Globals.xbool);
            Add("INTERFACE SOUND TYPE", Globals.xnameOrString, "bowl", "ding", "notify", "ring");
            Add("INTERFACE SOUND WAIT", Globals.xint);
            Add("INTERFACE SUGGESTIONS", Globals.xnameOrString, "none", "option");
            Add("INTERFACE TABLE OPERATORS", Globals.xbool);
            Add("INTERFACE ZOOM", Globals.xint);
            Add("LIBRARY CACHE", Globals.xbool);
            Add("MENU STARTFILE", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("MODEL CACHE", Globals.xbool);
            Add("MODEL CACHE MAX", Globals.xint);            
            Add("MODEL GAMS DEP CURRENT", Globals.xbool);
            Add("MODEL GAMS DEP METHOD", Globals.xnameOrString, "lhs", "eqname");
            Add("MODEL INFOFILE", Globals.xnameOrString, "yes", "no", "temp");
            Add("MODEL TYPE", Globals.xnameOrString, "default", "gams");
            Add("PLOT DECIMALSEPARATOR", Globals.xnameOrString, "period", "comma");                   //#kljsdfasfdlkj
            Add("PLOT ELEMENTS MAX", Globals.xint);
            Add("PLOT LINES POINTS", Globals.xbool);
            Add("PLOT USING", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("PLOT XLABELS ANNUAL", Globals.xnameOrString, "at", "between");  //#hsfsksgsdfg
            Add("PLOT XLABELS DIGITS", Globals.xint);
            Add("PLOT XLABELS NONANNUAL", Globals.xnameOrString, "at", "between");    //#hsfsksgsdfg                        
            Add("PRINT COLLAPSE", Globals.xnameOrString, "avg", "total", "none");                      //#kllæksdfgsdg
            Add("PRINT DISP MAXLINES", Globals.xsint);
            Add("PRINT ELEMENTS MAX", Globals.xint);            
            Add("PRINT FIELDS NDEC", Globals.xint);
            Add("PRINT FIELDS NWIDTH", Globals.xint);
            Add("PRINT FIELDS PDEC", Globals.xint);
            Add("PRINT FIELDS PWIDTH", Globals.xint);
            Add("PRINT FREQ", Globals.xnameOrString, "simple", "pretty");
            Add("PRINT MULPRT ABS", Globals.xbool);
            {
                Add("PRINT MULPRT GDIF", Globals.xbool);
                Alias("PRINT MULPRT GDIFF", "PRINT MULPRT GDIF");
            }            
            Add("PRINT MULPRT LEV", Globals.xbool);
            Add("PRINT MULPRT PCH", Globals.xbool);
            Add("PRINT MULPRT V", Globals.xbool);
            {
                Add("PRINT PRT DIF", Globals.xbool);
                Alias("PRINT PRT DIFF", "PRINT PRT DIF");
            }
            {
                Add("PRINT PRT GDIF", Globals.xbool);
                Alias("PRINT PRT GDIFF", "PRINT PRT GDIF");
            }
            Add("PRINT PRT ABS", Globals.xbool);
            Add("PRINT PRT PCH", Globals.xbool);
            Add("PRINT SPLIT", Globals.xbool);
            Add("PRINT WIDTH", Globals.xint);            
            Add("PYTHON EXE FOLDER", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("R EXE FOLDER", Globals.xnameOrStringOrFilename); //cf. #jsadklgasj4j                                    
            Add("SERIES DYN", Globals.xbool);
            Add("SERIES DYN CHECK", Globals.xbool);
            Add("SERIES FAILSAFE", Globals.xbool);
            Add("SERIES NORMAL PRINT MISSING", Globals.xoptionSeriesMissing, "ERROR", "M", "ZERO", "SKIP");    //#ljfdssdfgsh
            Add("SERIES NORMAL CALC MISSING", Globals.xoptionSeriesMissing, "ERROR", "M", "ZERO");             //#ljfdssdfgsh
            Add("SERIES NORMAL TABLE MISSING", Globals.xoptionSeriesMissing, "ERROR", "M", "ZERO", "SKIP");    //#ljfdssdfgsh
            Add("SERIES ARRAY PRINT MISSING", Globals.xoptionSeriesMissing, "ERROR", "M", "ZERO", "SKIP");     //#ljfdssdfgsh
            Add("SERIES ARRAY CALC MISSING", Globals.xoptionSeriesMissing, "ERROR", "M", "ZERO");              //#ljfdssdfgsh
            Add("SERIES ARRAY TABLE MISSING", Globals.xoptionSeriesMissing, "ERROR", "M", "ZERO", "SKIP");     //#ljfdssdfgsh
            Add("SERIES DATA MISSING", Globals.xoptionSeriesMissing, "M", "ZERO");                             //#ljfdssdfgsh
            Add("SHEET COLLAPSE", Globals.xnameOrString, "avg", "total", "none");         //#kllæksdfgsdg
            Add("SHEET ENGINE", Globals.xnameOrString, "excel", "internal");
            Add("SHEET FREQ", Globals.xnameOrString, "simple", "pretty");
            {
                Add("SHEET MULPRT GDIF", Globals.xbool);
                Alias("SHEET MULPRT GDIFF", "SHEET MULPRT GDIF");
            }
            Add("SHEET MULPRT ABS", Globals.xbool);
            Add("SHEET MULPRT LEV", Globals.xbool);
            Add("SHEET MULPRT PCH", Globals.xbool);
            Add("SHEET MULPRT V", Globals.xbool);            
            {
                Add("SHEET PRT DIF", Globals.xbool);
                Alias("SHEET PRT DIFF", "SHEET PRT DIF");
            }            
            {
                Add("SHEET PRT GDIF", Globals.xbool);
                Alias("SHEET PRT GDIFF", "SHEET PRT GDIF");
            }
            Add("SHEET PRT ABS", Globals.xbool);
            Add("SHEET PRT PCH", Globals.xbool);
            Add("SHEET ROWS", Globals.xbool);
            Add("SHEET COLS", Globals.xbool);
            Add("SOLVE DATA CREATE AUTO", Globals.xbool);
            Add("SOLVE DATA IGNOREMISSING", Globals.xbool);
            Add("SOLVE DATA INIT", Globals.xbool);
            Add("SOLVE DATA INIT GROWTH", Globals.xbool);
            Add("SOLVE DATA INIT GROWTH MAX", Globals.xval);
            Add("SOLVE DATA INIT GROWTH MIN", Globals.xval);
            Add("SOLVE FAILSAFE", Globals.xbool);
            Add("SOLVE FORWARD DUMP", Globals.xbool);
            Add("SOLVE FORWARD FAIR CONV", Globals.xnameOrString, "conv1", "conv2");   //#fxlsjffhsdks
            Add("SOLVE FORWARD FAIR CONV1 ABS", Globals.xval);
            Add("SOLVE FORWARD FAIR CONV1 REL", Globals.xval);
            Add("SOLVE FORWARD FAIR CONV2 TABS", Globals.xval);
            Add("SOLVE FORWARD FAIR CONV2 TREL", Globals.xval);
            Add("SOLVE FORWARD FAIR DAMP", Globals.xval);
            Add("SOLVE FORWARD FAIR ITERMAX", Globals.xint);
            Add("SOLVE FORWARD FAIR ITERMIN", Globals.xint);
            Add("SOLVE FORWARD NFAIR CONV", Globals.xnameOrString, "conv1", "conv2");   //#fxlsjffhsdks
            Add("SOLVE FORWARD NFAIR CONV1 ABS", Globals.xval);
            Add("SOLVE FORWARD NFAIR CONV1 REL", Globals.xval);
            Add("SOLVE FORWARD NFAIR CONV2 TABS", Globals.xval);
            Add("SOLVE FORWARD NFAIR CONV2 TREL", Globals.xval);
            Add("SOLVE FORWARD NFAIR DAMP", Globals.xval);
            Add("SOLVE FORWARD NFAIR ITERMAX", Globals.xint);
            Add("SOLVE FORWARD NFAIR ITERMIN", Globals.xint);
            Add("SOLVE FORWARD NFAIR UPDATEFREQ", Globals.xint);
            Add("SOLVE FORWARD STACKED HORIZON", Globals.xint);
            Add("SOLVE FORWARD METHOD", Globals.xnameOrString, "fair", "nfair", "none");
            Add("SOLVE FORWARD TERMINAL", Globals.xnameOrString, "exo", "const", "growth");
            Add("SOLVE FORWARD TERMINAL FEED", Globals.xnameOrString, "internal", "external");
            Add("SOLVE GAUSS CONV", Globals.xnameOrString, "conv1", "conv2");   //#fxlsjffhsdks
            Add("SOLVE GAUSS CONV IGNOREVARS", Globals.xbool);
            Add("SOLVE GAUSS CONV1 ABS", Globals.xval);
            Add("SOLVE GAUSS CONV1 REL", Globals.xval);
            Add("SOLVE GAUSS CONV2 TABS", Globals.xval);
            Add("SOLVE GAUSS CONV2 TREL", Globals.xval);
            Add("SOLVE GAUSS DAMP", Globals.xval);
            Add("SOLVE GAUSS DUMP", Globals.xbool);
            Add("SOLVE GAUSS ITERMAX", Globals.xint);
            Add("SOLVE GAUSS ITERMIN", Globals.xint);
            Add("SOLVE GAUSS REORDER", Globals.xbool);
            Add("SOLVE METHOD", Globals.xnameOrString, "newton", "gauss");
            Add("SOLVE NEWTON BACKTRACK", Globals.xbool);
            Add("SOLVE NEWTON CONV ABS", Globals.xval);
            Add("SOLVE NEWTON INVERT", Globals.xnameOrString, "lu", "iter");
            Add("SOLVE NEWTON ROBUST", Globals.xbool);
            Add("SOLVE NEWTON ITERMAX", Globals.xint);
            Add("SOLVE NEWTON UPDATEFREQ", Globals.xint);
            Add("SOLVE PRINT DETAILS", Globals.xbool);
            Add("SOLVE PRINT ITER", Globals.xbool);
            Add("SOLVE STATIC", Globals.xbool);
            Add("STRING INTERPOLATE FORMAT VAL", Globals.xstring);
            Add("SYSTEM CODE SPLIT", Globals.xint);
            Add("SYSTEM CLONE", Globals.xbool);
            Add("TABLE DECIMALSEPARATOR", Globals.xnameOrString, "period", "comma");                  //#kljsdfasfdlkj
            Add("TABLE HTML DATAWIDTH", Globals.xval);
            Add("TABLE HTML FIRSTCOLWIDTH", Globals.xval);
            Add("TABLE HTML FONT", Globals.xnameOrString);
            Add("TABLE HTML FONTSIZE", Globals.xval);
            Add("TABLE HTML SECONDCOLWIDTH", Globals.xval);
            Add("TABLE HTML SPECIALMINUS", Globals.xbool);
            Add("TABLE IGNOREMISSINGVARS", Globals.xbool);
            Add("TABLE MDATEFORMAT", Globals.xstring);
            Add("TABLE STAMP", Globals.xbool);
            Add("TABLE THOUSANDSSEPARATOR", Globals.xbool);
            Add("TABLE TYPE", Globals.xnameOrString, "txt", "html");
            Add("TIMEFILTER", Globals.xbool);
            Add("TIMEFILTER TYPE", Globals.xnameOrString, "hide", "avg");

            //sort by the first string
            rv.Sort((x, y) => String.Compare(x.FirstOrDefault(), y.FirstOrDefault()));

            Type type = typeof(Options); // Get type pointer
            FieldInfo[] fields = type.GetFields(); // Obtain all fields
            List<string> lines = new List<string>();
            foreach (var field in fields) // Loop through fields
            {
                string name = field.Name; // Get string name
                name = name.Replace("_", " ");
                lines.Add(name);
            }
            lines.Sort();
            
            int i1 = 0;
            int i2 = 0;
            //This is just to check full correspondence between the lines returned and the fields in Options.cs.
            //When adding or removing options, this is helpful, so that this is kept synchronized.
            //Options in Options.cs that are undocumented ("hidden") can have the check skipped below.
            while (true)
            {
                string s_handmadeList = rv[i1][0];  //the hand-made list
                string s_listFromReflection = lines[i2];  //from C# object
                
                if (s_handmadeList != s_listFromReflection)
                {
                    new Error("Mismatch regarding options, cf. Options.Syntax()");
                }
                i1++;
                i2++;
                if (i1 >= rv.Count && i2 >= lines.Count) break;  //else it will crash, and we will know that something is wrong
            }

            return rv;

        }

        public List<string> Intellisense(string s)
        {
            bool hasSeenEqual = false;
            List<string> rv = new List<string>();
            s = s.ToLower().Substring("option ".Length).Trim(); //must start with "option "
            if (s.EndsWith("="))
            {
                s = s.Substring(0, s.Length - 1).Trim();
                hasSeenEqual = true;
            }
            
            string[] words1 = new string[0];
            if (!string.IsNullOrEmpty(s))
            {
                words1 = s.Split(' ');
            }

            foreach (List<string> ss in Globals.listSyntax)
            {
                if (ss[0].StartsWith("bugfix ")) continue;  //do not intellisense "option bugfix ..." options.

                string[] words2 = ss[0].Split(' ');

                //if we are typing "OPTION folder ", just after this blank, we have that 
                //words1 = ["folder"]
                //words2 = ["folder", "working"] for instance
                //if words1.Length < words2.Length, we add "working" to output
                //if same length, we add the type to output
                //if words1.Length > words2.Length, we don't add anything (not matching)

                if (words1.Length > words2.Length)
                {
                    continue;
                }

                //words1 has <= number of elements compared to words2

                for (int i = 0; i < words1.Length; i++)
                {
                    if (words1[i] != words2[i]) goto Lbl1;  //all elements must match
                }

                if (words1.Length < words2.Length)
                {
                    if (hasSeenEqual) goto Lbl1;  //for instance "option folder =" + [blank] --> should only match yes|no, not "option folder working" etc.
                    string w = words2[words1.Length]; //.Length - 1 would take the corresponding (last) element, but we take the next
                    if (!rv.Contains(w)) rv.Add(w); //this .Contains() is a little slow, but should be unnoticeable in the GUI
                }
                else if (words1.Length == words2.Length)
                {
                    if (!hasSeenEqual && !rv.Contains("=")) rv.Add("=");
                    if (ss.Count <= 2)
                    {
                        string w = ss[1];  //the type           
                        string desc = null;

                        if (w == Globals.xbool)
                        {
                            desc = "[yes | no]";
                        }
                        else if (w == Globals.xstring)
                        {
                            desc = "[string]";
                        }
                        else if (w == Globals.xint)
                        {
                            desc = "[integer]";  //will fail if < 0
                        }
                        else if (w == Globals.xval)
                        {
                            desc = "[value]";
                        }
                        else if (w == Globals.xval2String)
                        {
                            desc = "[value]";
                        }
                        else if (w == Globals.xnameOrString)
                        {
                            desc = "[name | string]";
                        }
                        else if (w == Globals.xnameOrStringOrFilename)
                        {
                            desc = "[filename]";
                        }
                        else if (w == Globals.xnameOrString2Freq)
                        {
                            desc = "[frequency]";
                        }
                        else if (w == Globals.xoptionSeriesMissing)
                        {
                            desc = "[missing options]";
                        }
                        else if (w == Globals.xsint)
                        {
                            desc = "[integer]";
                        }                       

                        if (!rv.Contains(w)) rv.Add(desc);
                    }
                    else
                    {
                        for (int i = 2; i < ss.Count; i++)
                        {
                            string w = ss[i];
                            if (!rv.Contains(w)) rv.Add(w);
                        }
                    }
                }
            
            Lbl1:
                int ii = 0;  //useless statement, just used for the label

            }
            return rv;
        }

        public void Write()
        {
            Write(null);
        }

        public void Write(string optionName5)
        {
            string optionName = null;
            if (optionName5 != null) optionName = optionName5.Replace("Program.options.", "");
            //G.Writeln();
            //G.writeln("------------------------------------------------------");
            Type type = typeof(Options); // Get type pointer
            FieldInfo[] fields = type.GetFields(); // Obtain all fields

            List<string> lines = new List<string>();

            //TOTO TODO TODO
            //TOTO TODO TODO
            //TOTO TODO TODO what about plings in string names? What if they contain blanks?
            //TOTO TODO TODO
            //TOTO TODO TODO

            bool solveOptionSkipped = false;
            foreach (var field in fields) // Loop through fields
            {
                string line = "";
                string name = field.Name; // Get string name
                if (name == "series_dyn")
                {
                    continue;  //do not show this as an option
                }                
                
                if (optionName != null && name != optionName) continue;

                if (optionName == null && name.StartsWith("bugfix_")) continue;  //do not show bugfix options in list

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
                    if (value == "") value = "''";
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
                    string d = G.ConvertFreq(value);
                    line += name + " = " + d + ";";
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

                lines.Add(line);

            }

            lines.Sort(StringComparer.InvariantCulture);

            G.Writeln();
            foreach (string s in lines)
            {       
                G.Writeln(s);
            }            

            if (optionName == null)
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
                G.Writeln();
                G.Write("Advice on finding and setting options ");
                G.WriteLink("here", "outputtab:" + lc.counter); G.Writeln(".");
            }
        }
    }
}
