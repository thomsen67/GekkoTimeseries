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
using System.Collections;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;
using System.Threading;
using System.CodeDom;
using System.CodeDom.Compiler;


namespace Gekko
{

    /// <summary>
    /// Contains global variables, settings etc.
    /// </summary>
    public class Globals
    {
        public const string smpl = "งคฃ";  //this line must be at top
        public const string libraryDriveCheatString = "library___name___";
        public static bool HANDLE_LIBRARY = false;

        public static DayOfWeek weeklyWeekDayDefaultTsd = DayOfWeek.Friday;
        public static DayOfWeek weeklyFirstDayWhenPrinting = DayOfWeek.Monday;
        public static DayOfWeek weeklyLastDayWhenPrinting = DayOfWeek.Sunday;
        public static bool collapseFlexOverride = false; //must always be false
        
        // -------------------------------------------------
        // pink stuff start
        // -------------------------------------------------
        public static bool pink = false;
        public static bool pink2 = false;  //keeps track of .lst and .tsd, .prn, .csv files
        public static bool pink3 = false;  //tracks SYS commands
        public static List<string> datopgek_errors = null;
        public static List<string> datopgek_banks = null;
        public static List<string> datopgek_otherBanks = null;
        public static List<string> datopgek_listfiles = null;
        public static List<string> datopgek_sysCalls = null;
        public static List<string> datopgek_otherTypes = new List<string>() { "tsd", "prn", "csv" };
        public static List<string> datopgek_otherTypes2 = new List<string>() { "tsd", "prn", "csv", "lst" };
        // -------------------------------------------------
        // pink stuff end. Use this to track down the pink stuff when it should be deleted.
        // -------------------------------------------------               

        public const string globalLibraryString = "Global";
        public const string localLibraryString = "Local";  //--> maybe used later
        public const string gekkoLibraryString = "Gekko";  //--> maybe used later
        public const string thisLibraryString = "this";   //--> maybe used later
        public const string nullLibraryString = "null";   //--> maybe used later
        public const string dataLibraryString = "data";   //--> reserved
        public const string metaLibraryString = "meta";   //--> reserved

        public const string functionSpecialName1 = "_UfunctionSpecialName";
        public const string functionSpecialName2 = "O.FunctionLookupNew";

        public static string versionInternal = "";
        
        public static bool fixPxProblem = true;

        public static bool if_old_helper = false;

        public const string languageDa = "da";
        public const string languageEn = "en";
        public const string languageDaDK = "da-DK";
        public const string languageEnUS = "en-US";

        public const string funnyFileName = "delete_ksajrhdfjdssdj.txt";

        //Must be near the top of Globals.cs
        public static int counter = 0;  //used when emitting C# code to avoid name collisions
        public static int tempFilesCounter = 0;  //used when unzipping files

        public static ItemHandler itemHandler = null;  //hack

        public static bool decompSubstitute = false;

        public static StreamWriter sw = null;        
        
        public static List<string> unitTestsPromtingHelper = null;

        public static bool excelDna = false;  //true for use with ExcelDna solution
        public static StringBuilder excelDnaOutput = null;
        public static string excelDnaPath = null;  //used when compiling, to find ANTLR
        public static ExcelDnaData excelDnaData = null;
        public static string excelDnaName = "Gekcel";

        public static bool decompUnitCsvPivot = false;  //can activate xlsx pivot writing   

        public static string internalColumnIdentifyer = "gekkopivot__";
        public static string internalSetIdentifyer = "gekkoset__";
        public static string internalPivotRows = "Rows";
        public static string internalPivotCols = "Columns";
        public static string internalPivotFilters = "Filters";
        public static string internalPivotRowColor = "#ffededed"; // "#fff8f8f8"; //same as this: #982354320985

        // ----------------------------------------------------------------
        // GRADIENT
        // ----------------------------------------------------------------

        public static bool gradientSolve = false;
        public static double naiveGradient = double.NaN; //0.000002d;  //NaN to switch off
        public static double naiveMomentum = double.NaN; //0.9d;  //NaN to switch off
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // ROBUST 
        // ----------------------------------------------------------------
        //public static bool newtonStartingValuesFix = true;
        public static int newtonRobustHelper1 = -12345;
        public static double[] newtonRobustHelper2 = new double[1000];
        public const double newtonRobustHelper3 = 0.000001d;
        // ----------------------------------------------------------------

        //Using GekkoArg instead of IVariable as function parameters
        //with both false and true: below code is about 12.6 sec in debug mode --> 166.000 per second
        //CODE: function val f(val %x); return %x + 1; end; %y = 0; for(val %i = 1 to 2e6); %y = f(%y); end; prt %y;
        public static bool browserLimit = false;

        public static string equationCodeY = "y";
        public static string equationCodeT = "t";
        public static string equationCodeP = "p";

        public static DecompOptions2 uglyHack_decompOptions2 = null;
        public static string uglyHack_name = null;
        public static bool isAgeHierarchy = true;
        public static string ageHierarchyDivider = "..";
        public static string ageHierarchyName = "a10";
        public static string ageName = "a";
        public static string pivotTableDelimiter = " | ";

        public static DateTime tictoc = DateTime.Now;

        public static bool eliminateConcatenator = true;

        public static bool modeIntendedWarning = false;        

        public static string blockHelper = "<[time]>";

        //The following call a procedure or function: astprocedure, astfunctionnaked, astfunction, astobjectfunction        
        public static Dictionary<string, string> special = new Dictionary<string, string>() { { "ASTEXIT", "" }, { "ASTFOR", "" }, { "ASTFUNCTIONDEF2", "" }, { "ASTGOTO", "" }, { "ASTIF", "" }, { "ASTIF_OLD", "" }, { "ASTPROCEDUREDEF", "" }, { "ASTRETURN", "" }, { "ASTSTOP", "" }, { "ASTTARGET", "" }, { "ASTDOTORINDEXER", "" } };

        public static string errorHelper = null;        

        public static string objFunctionPlaceholder = "[obj-function-placeholder]";

        public static string isAProto = "Is_a_protobuffer_file";

        public const int smplOffset = 2;       //<2026 2200 p> x = pch(@x); --> had to set it from 1 to 2...! 
        public const int smplInitStart = 0;  //could be -2
        public const int smplInitEnd = 0;

        public static int foldingButtonCounter = 0;

        public static int decompPerLag = -2;

        public static GekkoDictionary<string, int> precedents = null;  //used in DECOMP, important that it starts out as null
        public static Dictionary<Series, int> precedentsSeries = null;  //used in SERIES, important that it starts out as null
        
        public static string extensionPlot = "gpt";
        public static string extensionCommand = "gcm";
        public const string defaultCommandFileExtension = "gcm";  //merge this with the above...
        public static string extensionDatabank = "gbk";
        public static string extensionTable = "gtb";

        public static List<string> tsdxVersions = new List<string> { "1.0", "1.1", "1.2" };  //1.0 = zipped tsd, 1.1 = protobuffers, 1.2 = Gekko 3.0 protobuffers.
        public static string currentGbkVersion = "1.2";

        public static string serviceMessage = "[service message]";
        public static string serviceMessageTruncated = "[further service messages truncated]";
        
        public static Dictionary<string, string> parentheses = new Dictionary<string, string> { { "(", ")" }, { "[", "]" }, { "{", "}" } };
        public static Dictionary<string, string> parenthesesInvert = new Dictionary<string, string> { { ")", "(" }, { "]", "[" }, { "}", "{" } };
        
        public static string splitStart = "//[[commandStart]]";
        public static string splitBit = "//[[command";
        public static string splitSpecial = "//[[commandSpecial]]";
        public static string splitEnd = "//[[commandEnd]]";

        public static string artificial = "artificial_parent_at_the_top_of_the_node_tree";
        
        public static Func<double, double, double>[] arithmentics = new Func<double, double, double>[20];
        public static Func<double, double>[] arithmentics1 = new Func<double, double>[10];

        /// <summary>
        /// BEWARE: This is used in dynamic C# code, no rename please! Or change the dynamic code...
        /// </summary>
        public static Dictionary<int, Func<GraphHelper, string>> printStorageAsFunc = new Dictionary<int, Func<GraphHelper, string>>();
        
        //maybe 14 is max??
        public static Dictionary<string, string> gamsFunctions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "log", null }, { "exp", null }, { "sum", null }, { "power", null }, { "sqr", "sqrt" } };

        public const string procedure = "procedure___";        

        public static string databankformatUrl = @"www.t-t.dk/gekko/databankformat";

        public static int graphBackground = 255; //221 before

        public const string brandNewFile = "brand new file";

        public const string bankNumberiName = "bankNumber";
        public const string bankNumberiMax = "1";

        public static bool showTimings = false;  //use comand TIMINGS                        

        public static string protobufFileName = "databank.bin";
        public static string protobufFileName2 = "databank.data"; //In Gekko 2.2 it might be wise to change to for instance databank.data, this setting is only for reading, and it tests Program.options.databank_file_gbk_internal too
        
        public static List<string> r_fileContent = null;
        public static List<string> python_fileContent = null;

        public const bool UNITTESTFOLLOWUP = false;
        public const bool UNITTESTFOLLOWUP_important = false;

        public static bool holesFix = true;

        public static string restartSnippet = "reset";

        public static int firstPeriodPositionInArrayNull = int.MaxValue;
        public static int lastPeriodPositionInArrayNull = int.MinValue;

        public static List<string> unitTestDependents = null;

        public const string stringConversionNote = "NOTE: You can use a string %s as a variable name with {%s}";
        
        public const string Work = "Work";
        public const string Ref = "Ref";
        public const string First = "First";
        public const string All = "All";  //circumvent a LOCAL<all>
        public const string Local = "Local";
        public const string Global = "Global";

        //only used as keys in switches etc.
        public const string ref_name = "ref";
        public const string first_name = "first";
        public const string local_name = "local";
        public const string global_name = "global";

        public const string listfile = "listfile";

        public const string symbolRefShortcut = "@";

        public const string printCode_n = "n";
        public const string operator_p = "p";
        public const string operator_d = "d";
        public const string operator_dp = "dp";
        public const string operator_r = "r";
        public const string operator_rn = "rn";
        public const string operator_rp = "rp";
        public const string operator_rd = "rd";
        public const string operator_rdp = "rdp";
        public const string operator_m = "m";
        public const string operator_q = "q";
        public const string operator_mp = "mp";
        public const string operator_l = "l";
        public const string operator_dl = "dl";
        public const string operator_rl = "rl";
        public const string operator_rdl = "rdl";

        public const string fixedTimelessText = "all periods (timeless)";
        public const string fixedParameterText = "everything fixed (parameter)";
        
        public static string ttPath3 = "GekkoCS";  //or "GekkoCS"
        public static string ttPath2 = @"c:\Thomas\Gekko"; //used when unit testing        
        
        public static List<Action<string, GekkoTime>> predictActions = null;

        public static string functionTP1Cs = Globals.smpl + ", p";
        public static string functionTP2Cs = "GekkoTime " + Globals.smpl + ", P p";

        public static string functionT1Cs = Globals.smpl;
        public static string functionT2Cs = "GekkoTime " + Globals.smpl;

        public static string functionP1Cs = "p";
        public static string functionP2Cs = "P p";

        public const int timeStringsStart = 1900;
        public const int timeStringsEnd = 2500;
        public static string[] timeStrings = null;  //stores "1900" to "2500" for easy access and reuse
        
        public const string forLoopName = "forloop_xe7dke6cj_";  //collision probability = 0
        public const string functionArgName = "functionarg_xf7dke8cj_";  //collision probability = 0

        public static string startGekkoTimeIteratorCode = "{" + G.NL + "  t = t2; " + G.NL;
        public static string endGekkoTimeIteratorCode = "}" + G.NL + "t = GekkoTime.tNull; " + G.NL;

        public static string gekkoSmplIteratorName = "{__GekkoCounter__}";
        public static string startGekkoSmplIteratorCode = "for (int iSmpl" + gekkoSmplIteratorName + " = 0; iSmpl" + gekkoSmplIteratorName + " < int.MaxValue; iSmpl" + gekkoSmplIteratorName + "++) {" + G.NL;
        public static string endGekkoSmplIteratorCode = G.NL + "if (" + Globals.smpl + ".HasError()) O.TryNewSmpl(" + Globals.smpl + ", iSmpl" + gekkoSmplIteratorName + "); else break;" + G.NL + "}";
        
        //Seems this is used
        public const string labelCheatString = "[<{THIS IS A LABEL}>]";

        public const string firstCheatString = "[FIRST]";

        public static int freqASubperiods = 1;
        public static int freqQSubperiods = 4;
        public static int freqMSubperiods = 12;
        
        public static bool databanksAsProtobuffers = true;

        //See also #80927435209843
        public static string dateStamp = Program.GetDateStamp();

        public static StringBuilder errorMemory = null;

        public static bool unitTestIntegration = false;
        public static string unitTestIntegrationMessage = "Set Globals.unitTestIntegration = true to run integration tests";
        public static List<ToFrom> unitTestCopyHelper = null;
        public static bool unitTestCopyHelper2 = false;        

        public static bool alwaysEnablcPackForSimulation = false;  //if true, packing of non-failing simulations is easy (but this costs time)
        public static UndoSim undoSim = null;
        public static PackSim packSim = null;
                
        // ----
        //This is used for OPTION intellisense
        public static string xbool = "bool";
        public static string xstring = "string";
        public static string xint = "int"; // >= 0
        public static string xval = "val";
        public static string xval2String = "val2String";
        public static string xnameOrString = "nameOrString";
        public static string xnameOrString2Freq = "nameOrString2Freq";
        public static string xnameOrStringOrFilename = "nameOrStringOrFilename";
        public static string xoptionSeriesMissing = "optionSeriesMissing";
        public static string xsint = "sint";  //signed int            
        // ----
        public static List<List<string>> listSyntaxAlias = new List<List<string>>();
        public static List<List<string>> listSyntax = Options.Syntax();  //this is created once and for all and is used for the entire Gekko session (not redone in RESET/RESTART)
        
        public const string symbolTurtle = "___";

        public const string symbolBankColon = ":";
        public const char symbolBankColon2 = ':';
        public const char symbolCollection = '#';
        public const char symbolScalar = '%';
        public const string symbolDollar = "$";
        public const char symbolTilde = '~';
        public const char symbolLeftCurly = '{';
        public const char symbolRightCurly = '}';
        public const char symbolConcatenation = '|';
        public const char symbolGlueChar1 = 'จ';
        public const string symbolGlueChar2 = "ฃ";  //for '.'
        public const string symbolGlueChar3 = "ง";  //for '.'
        public const string symbolGlueChar4 = "ฝ";  //for '*' and '?'
        public const string symbolGlueChar5 = "<=<"; //for option fields like <2000 2012>, using <_< is not good, interferes with <_lev> etc.
        public const string symbolGlueChar6 = "[_["; //for a[1], not a [1]    
        public const string symbolGlueChar6a = "ASTLEFTBRACKETGLUE";  //same as above
        public const string symbolGlueChar7 = "[จ["; //for [a1*b*c2] that must be interpreted as a wildcard, not 1x1 array

        public static readonly ScalarVal scalarVal0 = new ScalarVal(0d);
        public static readonly ScalarVal scalarVal1 = new ScalarVal(1d);
        public static readonly ScalarVal scalarValMissing = new ScalarVal(double.NaN);
        public static readonly ScalarString scalarStringStar = new ScalarString("*");
        public static readonly ScalarString scalarStringYes = new ScalarString("yes");
        public static readonly ScalarString scalarStringNo = new ScalarString("no");
        

        public static bool fastGauss = true;  //Beware: RES command should switch the option off
                
        public static double[] scaleNewtonValues = new double[0];
        
        public static int freezeDecompRows = 1;
        public static int freezeDecompCols = 1;
        public static int guiTableCellWidth = 100;
        public static int guiTableCellHeight = 20;

        public static double pruneDecomp = 0.10d;
        public static double guiPruneDecomp = 0.20d;  //bind this to combobox in gui -- not used?
        
        public const string freelists = "|||";
        public static bool fixWildcardLabel = true;  //keep this variable, it points to something to bugfix
        public static string wildcardText = "wildcard";

        public static string reportInterior1 = "O.ReportInterior(" + Globals.smpl + ", ";
        public const string reportInterior2 = ")";
        public const string labelCounter = "labelCounter";

        public static string reportLabel1 = "O.ReportLabel(" + Globals.smpl + ", ";
        public const string reportLabel2 = ")";

        public static int guiTimerCounter = 0;
        public static System.Timers.Timer guiTimer = null;  //only runs when executing a command

        public static int guiTimerCounter2 = 0;
        public static System.Timers.Timer guiTimer2 = null;  //runs the entire time (listens for remote.gcm)

        public static bool remoteIsInvestigating = false;  //to provide some thread safety
        public static DateTime remoteFileStamp = new DateTime(0l);
        public static int remoteExists = -12345;  //unknown

        public static string listLoopInternalName = "listloop_";
        public static string listLoopMovedStuff = "listloopMovedStuff_";

        //public const string libraryZipfileIndicator = "//[zipfile]: ";

        public static string printNaNIndicator = "M";  //= AREMOS, could be "NaN" instead

        public static readonly string QT = "\"";  //QT = " (single quote)               

        public static StreamWriter screenOutput = null;

        public static StringBuilder unitTestScreenOutput = new StringBuilder();

        public static bool pipe = false;
        public static PipeFileHelper pipeFileHelper = new PipeFileHelper();

        public static bool pipe2 = false;
        public static PipeFileHelper pipeFileHelper2 = new PipeFileHelper();  //pipe 2 is for printing etc. when user choses "p fy file=myfile.txt"

        public static string localTempFilesLocation = System.Windows.Forms.Application.LocalUserAppDataPath + "\\tempfiles";
        
        public static int guiTextPaddingLeft = 6;
        public static int guiTextPaddingVertical = 6;

        //global time settings
        public static GekkoTime globalPeriodStart = GekkoTime.tNull;
        public static GekkoTime globalPeriodEnd = GekkoTime.tNull;
        public static GekkoTimeSpans globalPeriodTimeSpans = new GekkoTimeSpans();  //nothing in .data yet.
        public static GekkoTimeSpans globalPeriodTimeFilters = new GekkoTimeSpans();  //nothing in .data yet.
        public static List<GekkoTime> globalPeriodTimeFilters2 = new List<GekkoTime>();

        public static string decompText0 = "Expression";
        public static string decompText1 = "[Data error]";
        public static string decompText1a = "[Difference]";
        public static string decompText2 = "[Decomp. error]";
        public static string decompText2a = "[Right hand side]";
                
        public static bool solveNewtonOnlyFeedback = false;  //should always be false

        public static string gekkoExePath = "";  //probably strange when unit testing or calling Gekcel
        public static string gekkoVersion = "";
        
        public static double invertRelativeConvergence = 0.0001d;  //old val=0.003d
        public static double invertAbsoluteConvergence = 1.0e-8d;
        public static int invertIterations = 500;  //old val=1000000
        public static bool mayPrintConvergenceCheckVariableMissing = true;

        public static double newtonStartingValueProblemSearchParameter = 2.0d;  //used when starting vals cause trouble
        public static double newtonSmallNumber = 0.001d;  //used when starting vals cause trouble

        public static double missingValueSeedNumber = 0.12345432123;

        public static bool solveUseOnlyDenseInverse = false; //default: false (dense has ca. half speed compared to sparse on ADAM)
        public static bool solveUseOnlySparseInverse = false;  //default: false

        // see ----> Program.options.solve_newton_backtrack = true //default: true (set to false can be GOOD for some problems)        

        public static bool solveUseFastSteps = true;     //default: true (mostly good)        

        public static GekkoTime dispLastDispStart;  //kind of a hack to be able to click on variable links and print with same time settings
        public static GekkoTime dispLastDispEnd;  //kind of a hack        
        public static bool guiHomeMainEnabled = false;
        public static bool guiHomeMenuEnabled = false;
                
        public static double jacobiDeltaProbe = 1.0e-4; //1.0 e-4 stepsize for gradient computation        
        
        public static bool solveUseStrictCrits = true;

        public static List<string> checkoff = new List<string>();

        public static string userSettingsPath = "";

        public const string stopHelper = "calling_non_existing_function";

        public static string linkSeparator1 = "#";
        public static char linkSeparator2 = ':';

        public static string modelPathAndFileName = "";
        public static string modelFileName = "";
        public static List<string> modelFileLines = null;

        public static string cmdPathAndFileName = "";
        public static string cmdFileName = "";
        public static List<string> cmdFileLines = null;

        public static string tableOption = "";

        public static int modelRandomID = 12345678;  //used in order to make a unique name for a temp folder that is later zipped (and the folder is deleted)

        public static readonly Random random777 = new Random();
        public static readonly object randomSyncLock = new object();
        public static int tempVarIndexCounter = 0;

        public static Random random = new Random();  //for reuse in functions runif() and rnorm()

        public static string[] convergenceCheckVariables = new string[1];
        public static bool initializeDataArrayWithNaN = true;
        public static bool simulationCheckThatAllDataGetsFromBArrayToTimeSeries = true;

        public const string seriesArraySubName = "[array]";

        public static int simCounter = 0;

        /// <summary>
        /// A lag like fY(-2.000000004) --> fY(-2), to avoid rounding errors
        /// </summary>
        public static double toleranceRegardingBrokenLagsOrLeads = 0.000001;
        
        public static UserSettings userSettings = new UserSettings();

        /// <summary>
        /// Used when parsing, puts in extra blank lines for safety (to avoid out-of-array problems)
        /// </summary>
        public static int extra = 10;  //

        /// <summary>
        /// For internal use only, to set a default size for double[] arrays.
        /// </summary>
        public static int defaultPeriodsWhenCreatingTimeSeries = 200;  //200
        public static double defaultExpandRateForDataArrays = 1.5d;

        /// <summary>
        /// Used for kind of an internal hack
        /// </summary>
        public static int disableRationButtons = 0;
        
        public static Databank undoBank = null;
        public static int hasBeenEndoExoStatementsSinceLastSim = 0;

        public static O.HandleEndoHelper2 endo = null;
        public static O.HandleEndoHelper2 exo = null;

        public static string expressionText = null;
        public static Func<GekkoSmpl, IVariable> expression = null;  //old equations
        public static List<Func<GekkoSmpl, IVariable>> expressions = null;  //used for x[#i] kind of equations
                
        public static char parserErrorSeparator = 'ค';  //5 places: (1) model lex, (2) model syntax, (3) gcm lex, (4) gcm syntax, (5) run-time error
        public static char parserErrorSeparator2 = '*';  //in filenames like file1.gcm*13, this means that there is a line offset of 13 lines (used in libraries lazy loading)

        public static char parserExpressionSeparator = 'ค';
        public static string lagIndicator = "ค";
        public static string leftParenthesisIndicator = "[";
        public static string rightParenthesisIndicator = "]";
        public const char freqIndicator = '!';  //see also #09832752                

        public static string protectSymbol = "\u2714";
                
        public static bool doNotSaveUserSettings = false; 

        public static string ols1 = "OLS estimation";
        public static string ols2 = "Dep. variable = ";
                
        public static string compilerOptions32 = "/optimize /platform:x86";  //does this mean it runs under WoW on a 64-bit machine?
        public static string compilerOptions64 = "/optimize /platform:x64";
        
        public static bool btnStartThread = false;
        public static bool btnStopThread = false;
        public static LongProcess workerThread = null;
        public static Queue<string> tasks = new Queue<string>();

        public static List<Graph> windowsGraph = new List<Graph>();
        public static List<Window1> windowsDecomp = new List<Window1>();
        public static List<WindowDecomp> windowsDecomp2 = new List<WindowDecomp>();
        public static CounterHelper ch = new CounterHelper();
        
        public static string helpStartPage = "introduction";

        public static List<string> helpTopics = new List<string>() {  //this list corresponds to items in "Gekko commands" in the help files
            //done January 2021, see also Globals.extraNames and Globals.commandNames
            "ACCEPT",
            "ANALYZE",
            "BLOCK",
            "CHECKOFF",
            "CLEAR",
            "CLIP",
            "CLONE",
            "CLOSE",
            "CLS",
            "COLLAPSE",
            "COMPARE",
            "COPY",
            "COUNT",
            "CUT",
            "CREATE",
            "DATE",
            "DECOMP",
            "DELETE",
            "DISP",
            "DOC",
            "DOWNLOAD",
            "EDIT",
            "END",
            "ENDO",
            "EXIT",
            "EXO",
            "EXPORT",
            "FINDMISSINGDATA",
            "FOR",
            "FUNCTION",
            "GLOBAL",
            "GOTO",
            "HDG",
            "HELP",
            "IF",
            "IMPORT",
            "INDEX",
            "INI",
            "INTERPOLATE",
            "ITERSHOW",
            "LIBRARY",
            "LIST",
            "LOCAL",
            "LOCK",
            "MAP",
            "MATRIX",
            "MEM",
            "MENU",
            "MODE",
            "MODEL",
            "MULPRT",
            "OLS",
            "OPEN",
            "OPTION",
            "PAUSE",
            "PIPE",
            "PLOT",
            "PROCEDURE",
            "PRT",
            "PREDICT",            
            "R_RUN",
            "PYTHON_RUN",
            "REBASE",
            "READ",
            "RENAME",
            "RESET",
            "RESTART",
            "RETURN",
            "RUN",
            "SERIES",
            "SHEET",
            "SIGN",
            "SIM",
            "SMOOTH",
            "SPLICE",
            "STOP",
            "STRING",
            "SYS",
            "TABLE",
            "TARGET",
            "TELL",
            "TIME",
            "TIMEFILTER",
            "TRANSLATE",
            "TRUNCATE",
            "UNFIX",
            "UNLOCK",
            "VAL",
            "VAR",
            "WRITE",
            "X12A",
            "XEDIT"
        };

        public static List<string> extraNames = new List<string>() { "P", "PRI", "PRINT", "SER" };
        public static List<string> commandNames = Program.Add2Lists(Globals.helpTopics, Globals.extraNames);  //must be after the two lists
        
        public static Dictionary<string, string> createdVariables = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public static WindowRunStatus windowRunStatus = null;
        public static bool windowRunStatusIsStackTab = true;
        public static DateTime windowRunStatusLastCall = DateTime.Now;

        public static Program.Cache guiRecentFoldersCache = new Program.Cache(typeof(string));
        public static int guiGraphWindowTopDistance = 50;
        public static int guiGraphWindowLeftDistance = 100;
        public static int guiDecompWindowTopDistance = 50;
        public static int guiDecompWindowLeftDistance = 100;
        public static int guiErrorWindowTopDistance = 100;
        public static int guiErrorWindowLeftDistance = 150;
        public static int guiItershowWindowTopDistance = 50;
        public static int guiItershowWindowLeftDistance = 100;

        public const string linkActionStart = "{a{";  //these links take up chars, so both Wrap and Table may not be precise, if the link url is too long.
        public const string linkActionEnd = "}a}";    //this could be remedied by adjusting "real" length of line, but perhaps not worth the effort right now?
        public const char linkActionDelimiter = 'ค';

        //Perhaps merge all this stuff with GekkoAction?
        public static long linkContainerCounter = 0L;
        public static Dictionary<long, Program.LinkContainer> linkContainer = new Dictionary<long, Program.LinkContainer>();
        public static long outputTabTextCounter = 0L;
        public static Dictionary<string, Program.ErrorContainer> outputTabTextContainer = new Dictionary<string, Program.ErrorContainer>(StringComparer.OrdinalIgnoreCase);

        public static long linkActionCounter = 0L;
        public static Dictionary<long, GekkoAction> linkAction = new Dictionary<long, GekkoAction>();

        public static string gekkoExeParameters = null;

        public static bool runningOnTTComputer = false;        
        
        public static bool prettyTextTableRendering = false;  //see http://www.unicode.org/charts/PDF/U2500.pdf

        public static int numberOfErrors = 0;
        public static int numberOfWarnings = 0;
        public static int numberOfSkippedLines = 0;

        public static bool threadIsInProcessOfAborting = false;  //click on stop button
        public static bool applicationIsInProcessOfAborting = false;  //exit command issued
        public static bool applicationIsInProcessOfDying = false;  //exit command issued
        public static string lastDynamicCsCode = null;  //if it does not compile (internal error)

        public static int lockedCounter = 0;
        
        public static bool printAST = false;  //for debugging, gets true when "ast" is typed at command prompt        

        public static Excel.Application objApp = null;  //used for the Excel PIA interface (not used much anymore, after EPPlus).
        public static int excelLastThreadID = int.MinValue;

        public static int waitFileTotalTime = 600;  //600 s = 10 min
        public static int waitFileGap = 2;


        public static int guiMainLinePosition = 0;

        public static Dictionary<string, string> gekkoInbuiltFunctions = null;

        public static string autoExecCmdFileName = "gekko.ini";

        public static string detectedRPath = null;
        public static string detectedPythonPath = null;        

        public static int endOfLinePositionWhenLastEnterPressed = -12345;
        public static int startOfLinePositionWhenLastEnterPressed = -12345;
        
        public static bool debugTokens = false;  //tokens are printed 1 by 1, for debug purposes, show tokens, showtokens                

        public static bool printGrayLinesForDebugging = false;        

        public static bool noini = false;

        public static GAMS.GAMSWorkspace gamsWorkspace = null;
        public static string gamsWorkspaceHelper = null;
        public static double gamsEps = 5e300d;
        public static double gamsNegInf = 4e300d;
        public static double gamsPosInf = 3e300d;
        public static double gamsNA = 2e300d;
        public static double gamsUndf = 1e300d; //probably has this value in gdx, see https://github.com/NREL/gdx-pandas/blob/master/gdxpds/gdx.py

        public static int convertTableCounter = 0;
        public static int convertTableErrorCounter = 0;

        public static string tableConverterText1 = "// ----------------------------------------------------------------------";
        public static string tableConverterText2 = "// This file is auto-generated, made from a XML table file";
        public static string tableConverterText3 = "// XML table filename: ";
        public static string tableConverterText4 = "// ----------------------------------------------------------------------";
        public static string tableConverterText5 = "//";

        public static string lastCalledMenuTable = null;

        public static string htmlGekkoCommentary = "Gekko added these styles to help table layout";
        public static string htmlFileStart1 = "<!DOCTYPE html>" + G.NL + "<html>" + G.NL + "<head>" + G.NL;
        public static string htmlFileStart2 = "</head>" + G.NL + "<body>" + G.NL;
        public static string htmlFileEnd = "</body>" + G.NL + "</html>" + G.NL;

        public static int convertMenuCounter = 0;
        public static int convertMenuErrorCounter = 0;

        public static int convertTabToTextCounter = 0;
        public static int convertTabToTextErrorCounter = 0;

        public static string errorString = "*** ERROR: ";
        public static string warningString = "+++ WARNING: ";
        public static string noteString = "+++ NOTE: ";

        public static System.Drawing.Color warningColor = System.Drawing.Color.OrangeRed;
        public static System.Windows.Media.Color GrayExcelLine = System.Windows.Media.Color.FromArgb(255, 208, 215, 229); //as in Excel
        public static System.Windows.Media.Color GrayExcelSelect = System.Windows.Media.Color.FromArgb(255, 234, 236, 245);  //as in Excel
        public static System.Drawing.Color LightBlueWord = System.Drawing.Color.FromArgb(74, 130, 189);  //as auto-tables in Word                
        public static System.Windows.Media.Color LightGray = System.Windows.Media.Color.FromArgb(255, 248, 248, 248);  //same as this: #982354320985
        public static System.Windows.Media.Color MediumBlueDecompLink = System.Windows.Media.Color.FromArgb(255, 6, 69, 173); //same color as wikipedia links //see also http://www.colorhexa.com/3232bb                
        public static System.Windows.Media.Color LightRed = System.Windows.Media.Color.FromArgb(255, 255, 247, 237);

        public static System.Windows.Media.FontFamily decompFontFamily = new System.Windows.Media.FontFamily("Calibri");
        public static System.Windows.Media.SolidColorBrush decompSolidColorBrush = new System.Windows.Media.SolidColorBrush(Globals.GrayExcelLine);

        public static double missingVariableZero = 0d;
        public static double missingVariableArtificialNumber = 3e303d;  //max value for double is 1.7976931348623157E+308. GAMS uses e300, so we use e303
        public static double missingVariableArtificialNumberLow = 2.999e303d;
        public static double missingVariableArtificialNumberHigh = 3.001e303d;

        public static char pxInternalDelimiter = 'ค';

        public static string RunGekkoTabToTextStuff_folder = "";  //not too pretty, but only used to send stuff from menu into thread

        public static CodeDomProvider csCompiler = CodeDomProvider.CreateProvider("CSharp");
        public static ICodeCompiler iCodeCompiler = Globals.csCompiler.CreateCompiler();

        public static string gekkoSmplInit = "GekkoSmpl " + Globals.smpl + " = new GekkoSmpl(); O.InitSmpl(" + Globals.smpl + ", p);";
        public static string gekkoSmplInitCommand = "O.InitSmpl(" + Globals.smpl + ", p);";
        public static string GekkoSmplNull = "" + Globals.smpl + " = null;";

        public static string iniFileSecretName = "[[RunGekkoIniFile]]";

        public static bool isAutoExec = true;
        public static string sessionMemorySnapshot = null;
        public static string sessionMemoryHistory = null;

        public static WindowIntellisense windowIntellisense = null;

        public static Table lastPrtOrMulprtTable = null; 
        public static Table lastDecompTable = null;  //only used for unit tests
        public static bool showDecompTable = false;  //only used for unit tests
        public static string decompResidualName = "a_residual__";

        public static CommandMemory commandMemory = new CommandMemory();

        public static List<string> bugfixMissing1 = new List<string>();
        public static GekkoDictionary<string, string> bugfixMissing2 = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    }

    public class CommandMemory
    {
        public StringBuilder storage = new StringBuilder();
        public int lengthWhenLastEnterPressed = 0;
    }

    public class CounterHelper
    {
        public int windowsGraphCloseCounter = 0;
        public int windowsDecompCloseCounter = 0;
        public int windowsGraphUpdateCounter = 0;
        public int windowsDecompUpdateCounter = 0;
        public int windowsGraphUpdateFailedCounter = 0;
        public int windowsDecompUpdateFailedCounter = 0;
    }
}
