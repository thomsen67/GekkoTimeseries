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
        //public GekkoDictionary<string, IVariable> storage = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);

        ////public IVariable this[string s]
        ////{
        ////    get => this.storage[s];
        ////    set => this.storage[s] = value;
        ////}
        
        //public string GetString(string key)
        //{
        //    return O.ConvertToString(this.storage[key]);
        //}

        //public void SetString(string key, string s)
        //{
        //    this.storage[key] = new ScalarString(s);
        //}

        //public double GetVal(string key)
        //{
        //    return O.ConvertToVal(this.storage[key]);
        //}

        //public void SetVal(string key, double v)
        //{
        //    this.storage[key] = new ScalarVal(v);
        //}

        //public int GetInt(string key)
        //{
        //    return O.ConvertToInt(this.storage[key]);
        //}

        //public void SetInt(string key, int i)
        //{
        //    this.storage[key] = new ScalarVal(i);
        //}

        //public bool GetBool(string key)
        //{
        //    IVariable iv = this.storage[key];
        //    if (Object.ReferenceEquals(iv, Globals.scalarStringYes)) return true;
        //    else if (Object.ReferenceEquals(iv, Globals.scalarStringNo)) return false;
        //    else
        //    {
        //        G.Writeln2("*** ERROR: Option is not of yes/no type");
        //        throw new GekkoException();
        //    }
        //}

        //public void SetBool(string key, string s)
        //{
        //    if (G.Equal(s, "yes")) this.storage[key] = Globals.scalarStringYes;
        //    else if (G.Equal(s, "no")) this.storage[key] = Globals.scalarStringNo;
        //    else
        //    {
        //        G.Writeln2("*** ERROR: Option is not of yes/no type");
        //        throw new GekkoException();
        //    }
        //}

        //public bool? GetBoolQ(string key)
        //{
        //    IVariable iv = this.storage[key];
        //    if (iv == null) return null;
        //    else if (Object.ReferenceEquals(iv, Globals.scalarStringYes)) return true;
        //    else if (Object.ReferenceEquals(iv, Globals.scalarStringNo)) return false;
        //    else
        //    {
        //        G.Writeln2("*** ERROR: Option is not of yes/no type");
        //        throw new GekkoException();
        //    }
        //}

        //public void SetBoolQ(string key, string s)
        //{
        //    if (s == null) this.storage[key] = null;
        //    else if (G.Equal(s, "yes")) this.storage[key] = Globals.scalarStringYes;
        //    else if (G.Equal(s, "no")) this.storage[key] = Globals.scalarStringNo;
        //    else
        //    {
        //        G.Writeln2("*** ERROR: Option is not of yes/no type");
        //        throw new GekkoException();
        //    }
        //}

        //!! do not use '_' inside an option -- the '_' corresponds to a blank in ANTLR            

        //!!!! NOTE: THESE LINE NUMBERSS CORRESPOND TO LINES IN Cmd3.g, line for line    

        //these are not mentioned in help
        public bool bugfix_import_export = false;             
        
        public bool bugfix_missing = true;  //if option true, m()==m() will be true, and m()<>m() false for series comparison

        //question
        
        
        public bool databank_create_auto = true;             
        public bool databank_file_copylocal = true;
        public bool databank_file_gbk_compress = true;
        public string databank_file_gbk_version = "1.2";  //decides what kind of .gbk file is written  
        public string databank_file_gbk_internal = "databank.data"; //change to "databank.data" in Gekko 2.2        
        public bool databank_search = true;
                
        public int decomp_maxlag = 10;
        public int decomp_maxlead = 10;

        public int fit_ols_rekur_dfmin = 10;

        //question
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

        //question
        public EFreq freq = EFreq.A;

        public string gams_exe_folder = "";
        public bool gams_fast = true; //use low-level api        
        public bool gams_time_detect_auto = false;  //will test if a dim looks like time. Only possible with gams_time_prefix != "".
        public string gams_time_freq = "a";  //could be u for undated
        public double gams_time_offset = 0;  //add to the integer after prefix, for instance t0 -> 2006
        public string gams_time_prefix = "";  //prefix of time set elements, if 't' time can be for instance t0
        public string gams_time_set = "t";  //name of the time set in GAMS                
        
        //question -- logic could be that interface assembles stuff that relates to the GUI, but also stuff like the help system which is 'passive' pages (unlike tables and menus).
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

        //question
        public string menu_startfile = "menu.html";

        //question
        public bool model_cache = true;  //if using cache on file or not        
        public int model_cache_max = 20;  //model options are non-solving options. How many fixed models are kept in RAM    
        public bool model_gams_dep_current = false;
        public string model_gams_dep_method = "lhs";  //lhs|eqname
        public string model_infofile = "yes";  //yes/no/temp
        public string model_type = "default";  //normal | gams

        //question
        public string plot_decimalseparator = "period";  //comma|period
        public int plot_elements_max = 200;
        public bool plot_lines_points = true;
        public string plot_using = ""; //a global template
        public string plot_xlabels_annual = "at"; //at|between
        public string plot_xlabels_nonannual = "between"; //at|between          
        public int plot_xlabels_digits = 4; // 4 or 2, only applies to 'between' type   

        //question
        public string print_collapse = "none";  //avg or total or none
        public int print_disp_maxlines = 3; //-1 means infinite, 0 means no data shown
        public int print_elements_max = 400;
        public int print_fields_ndec = 4;
        public int print_fields_nwidth = 13;
        public int print_fields_pdec = 2;
        public int print_fields_pwidth = 8;  //to make more room for labels
        public int print_filewidth = 130;        
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

        public string python_exe_folder = "";  //there will probably be more Python options later on

        public string r_exe_folder = "";  //there will probably be more R options later on
        public string r_exe_path = "";  //old name, delete at some point in 3.3.x series       

        
        

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

        //question
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

        //question
        public bool solve_data_create_auto = false;
        public bool solve_data_ignoremissing = false;  //for now, keep it here
        public bool solve_data_init = true;
        public bool solve_data_init_growth = true; //only has effect if solve_fast = true
        public double solve_data_init_growth_min = -0.02; //only has effect if solve_fast = true. Limit: -0.01 hurts.
        public double solve_data_init_growth_max = 0.06; //only has effect if solve_fast = true. Limit: it could be 0.05 without problems. But 0.04 hurts.
        public bool solve_failsafe = false;
        //question
        public bool solve_forward_dump = false;
        public string solve_forward_fair_conv = "conv1";
        public double solve_forward_fair_conv1_abs = 0.001d; //it checks abs OR rel, so abs is set really low (for instance, interest rates have low abs value)
        public double solve_forward_fair_conv1_rel = 0.001d;
        public double solve_forward_fair_conv2_tabs = 1.0d;
        public double solve_forward_fair_conv2_trel = 0.001d;
        public double solve_forward_fair_damp = 0.0; //redefined in 2.0
        public int solve_forward_fair_itermax = 200;        
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
        //question
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
        //question
        public bool solve_newton_backtrack = true;
        public double solve_newton_conv_abs = 0.0001;  //this is for a sum (really RMSQ) over all equations, so it is really low for most purposes. 
        public string solve_newton_invert = "lu"; //lu or iter, lu is more precise -- only problem is that the matrix is not sparse. Should maybe find sparse LU module.
        public int solve_newton_itermax = 200;
        public bool solve_newton_robust = false;
        public int solve_newton_updatefreq = 15;  //fast steps are so fast now that we relax this from 10 -> 15
        public bool solve_print_details = false;
        public bool solve_print_iter = false;
        public bool solve_static = false;

        public string string_interpolate_format_val = ""; //"0.000" for 3 dec, "12:0.000" 12 chars wide, "12:F3" the same, "-12:0.000" left-aligned, # can be used. //"0.000" for 3 dec, "12:0.000" 12 chars wide, "12:F3" the same, "-12:0.000" left-aligned, # can be used.

        public int system_code_split = 20; //20 seems good
        public bool system_clone = true; //y = f(#x); #x[2] = ...; No side-effect.

        //question        
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

        //question
        public bool timefilter = false;
        public string timefilter_type = "hide";  //"hide" or "avg"

        public static List<List<string>> Syntax()
        {
            List<List<string>> rv = new List<List<string>>();

            List<string> types = new List<string>();
            types.Add("bool");
            types.Add("string");
            types.Add("numberIntegerOrDouble");
            types.Add("numberIntegerOrDouble2String");
            types.Add("nameOrString");
            types.Add("nameOrStringOrFilename");
            types.Add("int");
            types.Add("name2Freq");
            types.Add("optionSeriesMissing");
            types.Add("plusminusint");

            void Add(params string[] ss)
            {
                //input array elements will be trimmed (and first element is set lowercase)
                if (!types.Contains(ss[1]))
                {
                    G.Writeln("*** ERROR: Option type error");
                    throw new GekkoException();
                }
                List<string> ss5 = new List<string>();
                string w = ss[0].Trim().ToLower();
                if (w.Contains("  "))
                {
                    G.Writeln("*** ERROR: Option type error, too many blanks");
                    throw new GekkoException();
                }
                ss5.Add(w); //also set lowercase
                for (int i = 1; i < ss.Length; i++) ss5.Add(ss[i].Trim());
                rv.Add(ss5);
            }

            //NOTE: the first string is written like this:
            // + capitals (only so it is easier to see visually)
            // + 1 blank to separate idents
            // + (..|..) for synonym, first one is used
            // + type can be: "", "", "", "", "", ""
            // 

            Add("BUGFIX IMPORT EXPORT", "bool");
            Add("BUGFIX GBK", "bool");
            Add("BUGFIX MISSING", "bool");
            Add("DATABANK COMPARE TABS", "numberIntegerOrDouble");
            Add("DATABANK COMPARE TREL", "numberIntegerOrDouble");
            Add("DATABANK CREATE AUTO", "bool");
            Add("DATABANK FILE COPYLOCAL", "bool");
            Add("DATABANK FILE GBK COMPRESS", "bool");            
            Add("DATABANK FILE GBK VERSION", "numberIntegerOrDouble2String");
            Add("DATABANK FILE GBK INTERNAL", "nameOrString");
            Add("DATABANK SEARCH", "bool");
            Add("DECOMP MAXLAG", "numberIntegerOrDouble");
            Add("DECOMP MAXLEAD", "numberIntegerOrDouble");
            Add("FIT OLS REKUR DFMIN", "numberIntegerOrDouble");
            Add("FOLDER", "bool");
            Add("FOLDER BANK", "nameOrStringOrFilename");
            Add("FOLDER BANK1", "nameOrStringOrFilename");
            Add("FOLDER BANK2", "nameOrStringOrFilename");
            Add("FOLDER COMMAND", "nameOrStringOrFilename");
            Add("FOLDER COMMAND1", "nameOrStringOrFilename");
            Add("FOLDER COMMAND2", "nameOrStringOrFilename");
            Add("FOLDER HELP", "nameOrStringOrFilename");
            Add("FOLDER MENU", "nameOrStringOrFilename");
            Add("FOLDER MODEL", "nameOrStringOrFilename");
            Add("FOLDER PIPE", "nameOrStringOrFilename");
            Add("FOLDER TABLE", "nameOrStringOrFilename");
            Add("FOLDER TABLE1", "nameOrStringOrFilename");
            Add("FOLDER TABLE2", "nameOrStringOrFilename");
            Add("FOLDER WORKING", "nameOrStringOrFilename");
            Add("FREQ", "name2Freq", "a", "q", "m", "d", "u");
            Add("GAMS EXE FOLDER", "nameOrStringOrFilename");
            Add("GAMS FAST", "bool");
            Add("GAMS TIME DETECT AUTO", "bool");
            Add("GAMS TIME FREQ", "nameOrString");
            Add("GAMS TIME OFFSET", "int");
            Add("GAMS TIME PREFIX", "nameOrString");
            Add("GAMS TIME SET", "nameOrString");
            Add("INTERFACE ALIAS", "bool");
            Add("INTERFACE CLIPBOARD DECIMALSEPARATOR", "nameOrString", "period", "comma");    //#kljsdfasfdlkj
            Add("INTERFACE CSV DECIMALSEPARATOR", "nameOrString", "period", "comma");          //#kljsdfasfdlkj
            Add("INTERFACE CSV DELIMITER", "nameOrString", "semicolon", "comma");
            Add("INTERFACE CSV NDEC", "int");
            Add("INTERFACE CSV PDEC", "int");
            Add("INTERFACE DEBUG", "nameOrString", "none", "dialog");
            Add("INTERFACE EDIT STYLE", "nameOrString", "gekko", "gekko2", "rstudio", "rstudio2");
            Add("INTERFACE EXCEL LANGUAGE", "nameOrString", "danish", "english");
            Add("INTERFACE EXCEL MODERNLOOK", "bool");
            Add("INTERFACE HELP COPYLOCAL", "bool");
            Add("INTERFACE MODE", "nameOrString", "mixed", "sim", "data");
            Add("INTERFACE MUTE", "nameOrString", "bool");
            Add("INTERFACE REMOTE", "bool");
            Add("INTERFACE REMOTE FILE", "nameOrStringOrFilename");
            Add("INTERFACE SOUND", "bool");
            Add("INTERFACE SOUND TYPE", "nameOrString", "bowl", "ding", "notify", "ring");
            Add("INTERFACE SOUND WAIT", "int");
            Add("INTERFACE SUGGESTIONS", "nameOrString", "none", "option");
            Add("INTERFACE TABLE OPERATORS", "bool");
            Add("INTERFACE ZOOM", "int");
            Add("MENU STARTFILE", "nameOrStringOrFilename");
            Add("MODEL CACHE MAX", "int");
            Add("MODEL CACHE", "bool");
            Add("MODEL GAMS DEP CURRENT", "bool");
            Add("MODEL GAMS DEP METHOD", "nameOrString", "lhs", "eqname");
            Add("MODEL INFOFILE", "nameOrString", "yes", "no", "temp");
            Add("MODEL TYPE", "nameOrString", "default", "gams");
            Add("PLOT ELEMENTS MAX", "int");
            Add("PLOT LINES POINTS", "bool");
            Add("PLOT XLABELS ANNUAL", "nameOrString", "at", "between");  //#hsfsksgsdfg
            Add("PLOT XLABELS DIGITS", "int");
            Add("PLOT XLABELS NONANNUAL", "nameOrString", "at", "between");    //#hsfsksgsdfg
            Add("PLOT DECIMALSEPARATOR", "nameOrString", "period", "comma");                   //#kljsdfasfdlkj
            Add("PLOT USING", "nameOrStringOrFilename");
            Add("PRINT COLLAPSE", "nameOrString", "avg", "total", "none");                      //#kllæksdfgsdg
            Add("PRINT ELEMENTS MAX", "int");
            Add("PRINT FREQ", "nameOrString", "simple", "pretty");
            Add("PRINT DISP MAXLINES", "plusminusint");
            Add("PRINT FIELDS NDEC", "int");
            Add("PRINT FIELDS NWIDTH", "int");
            Add("PRINT FIELDS PDEC", "int");
            Add("PRINT FIELDS PWIDTH", "int");
            Add("PRINT FILEWIDTH", "int");
            Add("PRINT MULPRT (GDIF|GDIFF)", "bool");
            Add("PRINT MULPRT ABS", "bool");
            Add("PRINT MULPRT LEV", "bool");
            Add("PRINT MULPRT PCH", "bool");
            Add("PRINT MULPRT V", "bool");
            Add("PRINT PRT (DIF|DIFF)", "bool");
            Add("PRINT PRT (GDIF|GDIFF)", "bool");
            Add("PRINT PRT ABS", "bool");
            Add("PRINT PRT PCH", "bool");
            Add("PRINT WIDTH", "int");
            Add("PRINT SPLIT", "bool");
            Add("PYTHON EXE FOLDER", "nameOrStringOrFilename");
            Add("R EXE FOLDER", "nameOrStringOrFilename");
            Add("R EXE PATH", "nameOrStringOrFilename");              //obsolete!
            Add("SERIES ARRAY IGNOREMISSING", "bool");
            Add("SERIES DATA IGNOREMISSING", "bool");
            Add("SERIES DYN", "bool");
            Add("SERIES DYN CHECK", "bool");
            Add("SERIES FAILSAFE", "bool");
            Add("SERIES NORMAL PRINT MISSING", "optionSeriesMissing", "ERROR", "M", "ZERO", "SKIP");    //#ljfdssdfgsh
            Add("SERIES NORMAL CALC MISSING", "optionSeriesMissing", "ERROR", "M", "ZERO", "SKIP");     //#ljfdssdfgsh
            Add("SERIES NORMAL TABLE MISSING", "optionSeriesMissing", "ERROR", "M", "ZERO", "SKIP");    //#ljfdssdfgsh
            Add("SERIES ARRAY PRINT MISSING", "optionSeriesMissing", "ERROR", "M", "ZERO", "SKIP");     //#ljfdssdfgsh
            Add("SERIES ARRAY CALC MISSING", "optionSeriesMissing", "ERROR", "M", "ZERO", "SKIP");      //#ljfdssdfgsh
            Add("SERIES ARRAY TABLE MISSING", "optionSeriesMissing", "ERROR", "M", "ZERO", "SKIP");     //#ljfdssdfgsh
            Add("SERIES DATA MISSING", "optionSeriesMissing", "ERROR", "M", "ZERO", "SKIP");            //#ljfdssdfgsh
            Add("SHEET COLLAPSE", "nameOrString", "avg", "total", "none");         //#kllæksdfgsdg
            Add("SHEET ENGINE", "nameOrString", "excel", "internal");
            Add("SHEET FREQ", "nameOrString", "simple", "pretty");
            Add("SHEET MULPRT (GDIF|GDIFF)", "bool");
            Add("SHEET MULPRT ABS", "bool");
            Add("SHEET MULPRT LEV", "bool");
            Add("SHEET MULPRT PCH", "bool");
            Add("SHEET MULPRT V", "bool");
            Add("SHEET PRT (DIF|DIFF)", "bool");
            Add("SHEET PRT (GDIF|GDIFF)", "bool");
            Add("SHEET PRT ABS", "bool");
            Add("SHEET PRT PCH", "bool");
            Add("SHEET ROWS", "bool");
            Add("SHEET COLS", "bool");
            Add("SOLVE DATA CREATE AUTO", "bool");
            Add("SOLVE DATA IGNOREMISSING", "bool");
            Add("SOLVE DATA INIT", "bool");
            Add("SOLVE DATA INIT GROWTH", "bool");
            Add("SOLVE DATA INIT GROWTH MAX", "numberIntegerOrDouble");
            Add("SOLVE DATA INIT GROWTH MIN", "numberIntegerOrDouble");
            Add("SOLVE FAILSAFE", "bool");
            Add("SOLVE FORWARD DUMP", "bool");
            Add("SOLVE FORWARD FAIR CONV", "nameOrString", "conv1", "conv2");   //#fxlsjffhsdks
            Add("SOLVE FORWARD FAIR CONV1 ABS", "numberIntegerOrDouble");
            Add("SOLVE FORWARD FAIR CONV1 REL", "numberIntegerOrDouble");
            Add("SOLVE FORWARD FAIR CONV2 TABS", "numberIntegerOrDouble");
            Add("SOLVE FORWARD FAIR CONV2 TREL", "numberIntegerOrDouble");
            Add("SOLVE FORWARD FAIR DAMP", "numberIntegerOrDouble");
            Add("SOLVE FORWARD FAIR ITERMAX", "int");
            Add("SOLVE FORWARD FAIR ITERMIN", "int");
            Add("SOLVE FORWARD NFAIR CONV", "nameOrString", "conv1", "conv2");   //#fxlsjffhsdks
            Add("SOLVE FORWARD NFAIR CONV1 ABS", "numberIntegerOrDouble");
            Add("SOLVE FORWARD NFAIR CONV1 REL", "numberIntegerOrDouble");
            Add("SOLVE FORWARD NFAIR CONV2 TABS", "numberIntegerOrDouble");
            Add("SOLVE FORWARD NFAIR CONV2 TREL", "numberIntegerOrDouble");
            Add("SOLVE FORWARD NFAIR DAMP", "numberIntegerOrDouble");
            Add("SOLVE FORWARD NFAIR ITERMAX", "int");
            Add("SOLVE FORWARD NFAIR ITERMIN", "int");
            Add("SOLVE FORWARD NFAIR UPDATEFREQ", "int");
            Add("SOLVE FORWARD STACKED HORIZON", "int");
            Add("SOLVE FORWARD METHOD", "nameOrString", "stacked", "fair", "nfair", "none");
            Add("SOLVE FORWARD TERMINAL", "nameOrString", "exo", "const", "growth");
            Add("SOLVE FORWARD TERMINAL FEED", "nameOrString", "internal", "external");
            Add("SOLVE GAUSS CONV", "nameOrString", "conv1", "conv2");   //#fxlsjffhsdks
            Add("SOLVE GAUSS CONV IGNOREVARS", "bool");
            Add("SOLVE GAUSS CONV1 ABS", "numberIntegerOrDouble");
            Add("SOLVE GAUSS CONV1 REL", "numberIntegerOrDouble");
            Add("SOLVE GAUSS CONV2 TABS", "numberIntegerOrDouble");
            Add("SOLVE GAUSS CONV2 TREL", "numberIntegerOrDouble");
            Add("SOLVE GAUSS DAMP", "numberIntegerOrDouble");
            Add("SOLVE GAUSS DUMP", "bool");
            Add("SOLVE GAUSS ITERMAX", "int");
            Add("SOLVE GAUSS ITERMIN", "int");
            Add("SOLVE GAUSS REORDER", "bool");
            Add("SOLVE METHOD", "nameOrString", "newton", "gauss");
            Add("SOLVE NEWTON BACKTRACK", "bool");
            Add("SOLVE NEWTON CONV ABS", "numberIntegerOrDouble");
            Add("SOLVE NEWTON INVERT", "nameOrString", "lu", "iter");
            Add("SOLVE NEWTON ROBUST", "bool");
            Add("SOLVE NEWTON ITERMAX", "int");
            Add("SOLVE NEWTON UPDATEFREQ", "int");
            Add("SOLVE PRINT DETAILS", "bool");
            Add("SOLVE PRINT ITER", "bool");
            Add("SOLVE STATIC", "bool");
            Add("STRING INTERPOLATE FORMAT VAL", "string");
            Add("SYSTEM CODE SPLIT", "int");
            Add("SYSTEM CLONE", "bool");
            Add("TABLE DECIMALSEPARATOR", "nameOrString", "period", "comma");                  //#kljsdfasfdlkj
            Add("TABLE HTML DATAWIDTH", "numberIntegerOrDouble");
            Add("TABLE HTML FIRSTCOLWIDTH", "numberIntegerOrDouble");
            Add("TABLE HTML FONT", "nameOrString");
            Add("TABLE HTML FONTSIZE", "numberIntegerOrDouble");
            Add("TABLE HTML SECONDCOLWIDTH", "numberIntegerOrDouble");
            Add("TABLE HTML SPECIALMINUS", "bool");
            Add("TABLE IGNOREMISSINGVARS", "bool");
            Add("TABLE MDATEFORMAT", "string");
            Add("TABLE STAMP", "bool");
            Add("TABLE THOUSANDSSEPARATOR", "bool");
            Add("TABLE TYPE", "nameOrString", "txt", "html");
            Add("TIMEFILTER", "bool");
            Add("TIMEFILTER TYPE", "nameOrString", "hide", "avg");

            return rv;

        }

        public List<string> Intellisense(string s)
        {
            List<string> rv = new List<string>();
            s = s.ToLower().Substring("option ".Length).Trim(); //must start with "option "
            
            string[] words1 = new string[0];
            if (!string.IsNullOrEmpty(s))
            {
                words1 = s.Split(' ');
            }

            foreach (List<string> ss in Globals.listSyntax)
            {
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
                    string w = words2[words1.Length]; //.Length - 1 would take the corresponding (last) element, but we take the next
                    if (!rv.Contains(w)) rv.Add(w); //this .Contains() is a little slow, but should be unnoticeable in the GUI
                }
                else if (words1.Length == words2.Length)
                {
                    if (ss.Count <= 2)
                    {
                        string w = ss[1];  //the type                    
                        if (!rv.Contains(w)) rv.Add(w);
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
