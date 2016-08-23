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
using System.Collections;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;
using System.Threading;
using System.CodeDom;
using System.CodeDom.Compiler;
//using System.Windows.Media.FontFamily;


namespace Gekko
{   
    
    /// <summary>
    /// Contains global variables, settings etc.
    /// </summary>
    public class Globals        
    {
        public static bool testFileChange = true;

        public static string extensionCommand = "gcm";
        public const string defaultCommandFileExtension = "gcm";  //merge this with the above...
        public static string extensionDatabank = "gbk";
        public static string extensionTable = "gtb";

        public static string serviceMessage = "[service message]";
        
        public static bool nameFix = true;
        
        public static bool readImportFilter = false;

        public static bool newSplit = true;
        public static string splitSTART2 = "//[[splitSTART]]";
        public static string splitSTOP2 = "//[[splitSTOP]]";
        public static string splitSTART = G.NL + splitSTART2 + G.NL;        
        public static string splitSTOP = G.NL + splitSTOP2 + G.NL;
        public static string functionParameterCode = "param_";
        
        public static string splitCommandBlockStart = "//[[commandStart]]";
        public static string splitCommandBlockEnd = "//[[commandEnd]]";        
        
        public static bool showTimings = false;  //use comand TIMINGS
        
        //public static int stackedTimePeriods = 5;
        public static string stackedTimeSeparator = "___";
        public static bool stackedPrintTimings = false;

        public static bool useRfFr = false;

        public static bool fixIndexerMaybeTransform = false;

        public static string r_fileName = null;
        public static List<string> r_fileContent = null;
                
        public static bool useCache = false;  //also makes sure vars GetTimeSeries is outside time loop in SERIES statement! See #9875235      
        public static bool useDotFunctionalityInParser = false;
        
        public static bool UNITTESTFOLLOWUP = false;

        public static string restartSnippet = "restart; mode data;";

        public static bool isAlphaVersion = false;
        public static bool isBetaVersion = false; 
        public static bool isGammaVersion = false; 
        public static bool isPreviewVersion = false; //for preview of 2.0        
        public static bool testVersion = false;  //SEEMS TO NOT WORK for errors: false in deployment version                                
        public static bool testing = true;  //just provides a pointer to remove temporary testing stuff  

        public static string Work = "Work";
        public static string Ref = "Ref";

        public const string printCode_s = "r";
        public const string printCode_sn = "rn";
        public const string printCode_sp = "rp";
        public const string printCode_sd = "rd";
        public const string printCode_sdp = "rdp";
                
        public List<Databank> bankOpen = new List<Databank>();
        
        public static string ttPath3 = "GekkoCS";  //or "GekkoCS"
        public static string ttPath2 = @"c:\Thomas\Gekko"; //used when unit testing        

        public static bool smart1 = true;

        public static string functionT1Cs = "t";
        public static string functionT2Cs = "GekkoTime t";

        public static string functionP1Cs = "p";
        public static string functionP2Cs = "P p";
                
        public static bool useTestParser = true;  //for debugging, use trial parser        
        public static bool substituteAssignVars = false;

        public static string clearTsCsCode = "ClearTS(p);";  //so it is easier to track the location of these

        //Must be near the top of Globals.cs
        public static GekkoTime tNull = new GekkoTime(EFreq.Annual, -12345, 1);  //think of it as a 'null' object (but it is a struct)
        public static int counter = 0;  //used when emitting C# code to avoid name collisions
                
        public static string startGekkoTimeIteratorCode = "{" + G.NL + "  t = t2; " + G.NL;
        public static string endGekkoTimeIteratorCode = "}" + G.NL + "t = Globals.tNull; " + G.NL;
        
        public const string indexerAloneCheatString = "[<{THIS IS AN IndexerAlone CALL}>]";
        
        public const string firstCheatString = "[FIRST]";        
        
        public static bool fixReturnProblem = true;

        public static bool databanksAsProtobuffers = true;

        //See also #80927435209843
        public static string dateStamp = Program.GetDateStamp();
        
        public static StringBuilder errorMemory = null;
        
        public static bool unitTestIntegration = false;
        public static SimpleUnitTestWindow unitTestWindow = null;
        public static string unitTestIntegrationMessage = "Set Globals.unitTestIntegration = true to run integration tests";
                
        public static Table unitTestTablePointer = null;  //used to see if table generated in PRT etc looks ok (regardless of how it actually prints)

        public static bool alwaysEnablcPackForSimulation = false;  //if true, packing of non-failing simulations is easy (but this costs time)
        public static UndoSim undoSim = null;
        public static PackSim packSim = null;

        public static bool patch_zvar = true;

        public const char symbolTilde = '~';
        public const char symbolList = '#';
        public const char symbolMemvar = '%';
        public const string symbolDollar = "$";
        public const char symbolConcatenation = '|';
        public const char symbolGlueChar1 = '¨';
        public const string symbolGlueChar2 = "£";  //for '.'
        public const string symbolGlueChar3 = "§";  //for '.'
        public const string symbolGlueChar4 = "½";  //for '*' and '?'
        public const string symbolGlueChar5 = "<=<"; //for option fields like <2000 2012>, using <_< is not good, interferes with <_lev> etc.
        public const string symbolGlueChar6 = "[_["; //for a[1], not a [1]    
        public const string symbolGlueChar7 = "[¨["; //for [a1*b*c2] that must be interpreted as a wildcard, not 1x1 array
        
        public static bool poolGenrLines = false;

        public static bool fastGauss = true;  //Beware: RES command should switch it off

        public static bool useNewRTFVersionForHelpTab = false; //handles links inside rtf files. but a bit unstable.        

        public static double[] scaleNewtonValues = new double[0];
        public static bool emitRCode = false;

        public static int freezeDecompRows = 1;
        public static int freezeDecompCols = 1;
        public static int guiTableCellWidth = 100;
        public static int guiTableCellHeight = 20;        
                
        public static double pruneDecomp = 0.10d;
        public static double guiPruneDecomp = 0.20d;  //bind this to combobox in gui -- not used?

        public static bool samAbsolute = false;  //normally false
        public static bool samOnlyExo = false;  //normally false, prblem is: Z and J-vars pollute this
        public static int samNumber = 50;  //normally 100        
        public static bool obeyStepByStep = false;  //normally false
        public static bool obeyEcho = false;  //normally false
        public static bool printTimerResults = false;  //normally false
        public static bool printStopWhenErrors = false;  //normally false
        public static bool printOnlyErrors = true;  //normally false

        public static int guiTimerCounter = 0;
        public static System.Timers.Timer aTimer = null;

        public static string printNaNIndicator = "M";  //= AREMOS, could be "NaN" instead

        public static readonly string QT = "\"";  //QT = " (single quote)               

        public static StreamWriter screenOutput = null;

        public static StringBuilder unitTestScreenOutput = new StringBuilder();

        public static bool pipe = false;        
        public static PipeFileHelper pipeFileHelper = new PipeFileHelper();            

        public static bool pipe2 = false;
        public static PipeFileHelper pipeFileHelper2 = new PipeFileHelper();  //pipe 2 is for printing etc. when user choses "p fy file=myfile.txt"
        
        public static string localTempFilesLocation = System.Windows.Forms.Application.LocalUserAppDataPath + "\\tempfiles";
        
        public static bool saffierPrintIterations = false;        

        public static List<string>[] globalAl;
        public static List<string>[] globalAlType;

        public static List<string> globalAlAFTER;
        public static List<string> globalAlTypeAFTER;

        public static string blankUsedAsPadding = " ";  //to be able to remove that stuff sometime when I understand padding in richtextbox!

        public static int globalMax;        

        public static string expectedStatement = "";
        
        //global time settings

        public static GekkoTime globalPeriodStart = Globals.tNull;
        public static GekkoTime globalPeriodEnd = Globals.tNull;
        public static GekkoTimeSpans globalPeriodTimeSpans = new GekkoTimeSpans();  //nothing in .data yet.
        public static GekkoTimeSpans globalPeriodTimeFilters = new GekkoTimeSpans();  //nothing in .data yet.
        public static List<GekkoTime> globalPeriodTimeFilters2 = new List<GekkoTime>();        

        public static string decompText0 = "Expression";
        public static string decompText1 = "[Data error]";
        public static string decompText1a = "[Difference]";
        public static string decompText2 = "[Decomp. error]";
        public static string decompText2a = "[Right hand side]";
        
        public static int solveJacobiSparse = 0;
        public static int disableStartingValuesFix = 1;

        public static int solveNewtonSimpleBacktrack = 1;
        public static bool solveNewtonOnlyFeedback = false;  //should always be false

        public static string gekkoVersion = "";        

        public static bool useDfsane = false;        
        
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
        public static bool solveScaleNewton = false;     //default: false (can be used for eg nested CES)        

        public static GekkoTime dispLastDispStart;  //kind of a hack to be able to click on variable links and print with same time settings
        public static GekkoTime dispLastDispEnd;  //kind of a hack        
        public static bool guiHomeMainEnabled = false;
        public static bool guiHomeMenuEnabled = false;

        public static int printLevelWidth = 14;
        public static int printRelativeWidthSmall = 7;  //this way, it can print -112.00% for instance, 6 is too small
        public static int printRelativeWidthLarge = 10;  //to give more room for variable names

        public static int printFrnnFile = 1;        

        public static double NewtonAbsoluteCrit = 1.0e-4;  //1.0 e-4 tol for newton algorithm  
        public static double jacobiDeltaProbe = 1.0e-4; //1.0 e-4 stepsize for gradient computation        

        public static double solveFastCritFactor = 4d;  //optimum seems around 30, but that seems a bit wild
        public static bool solveUseFastCrits = true;
        public static bool solveUseStrictCrits = true;
        public static bool solveCheckThatAllDataGetsFromBArrayToTimeSeries = true;

        public static List<string> checkoff = new List<string>();        

        public static string userSettingsPath = "";

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
        
        public static string[] convergenceCheckVariables = new string[1];        
        public static bool initializeDataArrayWithNaN = true;
        public static bool simulationCheckThatAllDataGetsFromBArrayToTimeSeries = true;

        public static int simCounter = 0;

        /// <summary>
        /// A lag like fY(-2.000000004) --> fY(-2), to avoid rounding errors
        /// </summary>
        public static double toleranceRegardingBrokenLagsOrLeads = 0.000001;

        public static String modelFileExtension = "frm";
        
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

        /// <summary>
        /// For global access when compiling genr's
        /// </summary>
        public static System.IO.StringWriter res;

        public static Databank undoBank = null;
        public static int hasBeenEndoExoStatementsSinceLastSim = 0;
        
        public static List<string> tsdxVersions = new List<string> { "1.0", "1.1" };

        public static int removeAllLags = 0;
        public static char parserErrorSeparator = '¤';
        public static string lagIndicator = "¤";
        public static string leftParenthesisIndicator = "(";
        public static string freqIndicator = "%";  //see also #09832752
        public static string afterModelJIndicator = "Y";
        public static string reverseIndicator1 = "REVERSE1";
        public static string reverseIndicator2 = "REVERSE2";

        public static string protectSymbol = "\u2714";

        public static Timer timer = new Timer();        

        public static bool setPrintMute = false;
        public static bool doNotSaveUserSettings = false;

        public static CompilerOptions co = new CompilerOptions();
        public static string compilerOptions = "/optimize /platform:x86";  //does this mean it runs under WoW on a 64-bit machine?

        public static ArrayList alFunctions;
        public static CaseInsensitiveHashtable userFunctions;

        public static bool btnStartThread = false;
        public static bool btnStopThread = false;
        public static LongProcess workerThread = null;
        public static Queue<string> tasks = new Queue<string>();
                
        public static List<Graph> windowsGraph = new List<Graph>();
        public static List<Window1> windowsDecomp = new List<Window1>();
        public static CounterHelper ch = new CounterHelper();

        public static bool revertSimpleJ = true;        

        public static string[] helpTopics = {  //this list corresponds to items in "Gekko commands" in the help files

                                                "ACCEPT",
                                                "ANALYZE",
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
"GOTO",
"HDG",
"HELP",
"IF",
"IMPORT",
"INDEX",
"INI",
"ITERSHOW",
"LIST",
"MATRIX",
"MEM",
"MENU",
"MODE",
"MODEL",
"MULPRT",
"NAME",
"OLS",
"OPEN",
"OPTION",
"PAUSE",
"PIPE",
"PLOT",
"PRT",
"R_EXPORT",
"R_FILE",
"R_RUN",
"READ",
"RENAME",
"RESET",
"RESTART",
"RETURN",
"RUN",
"SERIES",
"SHEET",
"SHOW",
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
"UNSWAP",
"VAL",
"WRITE",
"X12A"
};

        public static bool showZero = true;
        
        public static int yearIndicator = 1500;

        public static int _tmptmpCounter = 0;

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
        
        //Perhaps merge all this stuff in LinkContainer
        public static long linkContainerCounter = 0L;
        public static Dictionary<long, Program.LinkContainer> linkContainer = new Dictionary<long, Program.LinkContainer>();
        public static long outputTabTextCounter = 0L;
        public static Dictionary<string, Program.ErrorContainer> outputTabTextContainer = new Dictionary<string, Program.ErrorContainer>(StringComparer.OrdinalIgnoreCase);

        public static string gekkoExeParameters = null;
        
        public static bool runningOnTTComputer = false;
        public static DateTime timeHelper = DateTime.Now;
        
        public static bool splitCsCodeIntoChunks = true; //can be switched with "ssplit"
        public static int splitCsCodeIntoChunksLinesPerChunk = 10;
        public static bool simpleCode = false;  //activates ast_upd(), can be switched with "ssimple"

        public static bool prettyTextTableRendering = false;  //see http://www.unicode.org/charts/PDF/U2500.pdf

        public static int numberOfErrors = 0;
        public static int numberOfWarnings = 0;
        public static int numberOfSkippedLines = 0;

        public static bool threadIsInProcessOfAborting = false;  //click on stop button
        public static bool applicationIsInProcessOfAborting = false;  //exit command issued
        public static bool applicationIsInProcessOfDying = false;  //exit command issued
        public static string lastDynamicCsCode = null;  //if it does not compile (internal error)

        public static int lockedCounter = 0;

        public static bool histo = false;

        public static bool printAST = false;  //for debugging, gets true when "ast" is typed at command prompt        
        
        public static Excel.Application objApp = null;
        public static int excelLastThreadID = int.MinValue;

        public static int waitFileTotalTime = 600;  //600 s = 10 min
        public static int waitFileGap = 2;

        public static int guiMainLinePosition = 0;

        public static string autoExecCmdFileName = "gekko.ini";

        public static string detectedRPath = null;
        
        public static string guiDialogErrorText = "ERROR: Would you like to abort (Y), or ignore the error (N)";
        public static string guiDialogErrorCaption = "Error handling";

        public static int endOfLinePositionWhenLastEnterPressed = -12345;
        public static int startOfLinePositionWhenLastEnterPressed = -12345;        

        public static bool timing = true;

        public static bool debugTokens = false;  //tokens are printed 1 by 1, for debug purposes, show tokens, showtokens
        public static bool printGlue = false;  //for debugging purpose
        public static bool addGlue = true;
        public static string dotGlue = "¨";     //3.14 --> 3¨14. But 3. 14 --> 3. 14
        public static string lparGlue = "§";    //f(x) --> f§(x). But f (x) --> f (x) and f((3+1)-2) --> f§(3+1)-2)
                                                //so glue is only when preceeding chars is ident
        public static string lbrackGlue = "½";  //f[x] --> f€x]. But f [x] --> f [x] and f[(3+1)-2] --> f€(3+1)-2)
                                                //so glue is only when preceeding chars is ident
        
        public static bool printGrayLinesForDebugging = false;
        public static int debugCounter = 0;

        public static bool noini = false;

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
        
        public static System.Drawing.Color warningColor = System.Drawing.Color.OrangeRed;
        public static System.Windows.Media.Color GrayExcelLine = System.Windows.Media.Color.FromArgb(255, 208, 215, 229); //as in Excel
        public static System.Windows.Media.Color GrayExcelSelect = System.Windows.Media.Color.FromArgb(255, 234, 236, 245);  //as in Excel
        public static System.Drawing.Color LightBlueWord = System.Drawing.Color.FromArgb(74, 130, 189);  //as auto-tables in Word                
        public static System.Windows.Media.Color LightGray = System.Windows.Media.Color.FromArgb(255, 248, 248, 248);  //seems ok        
        public static System.Windows.Media.Color MediumBlueDecompLink = System.Windows.Media.Color.FromArgb(255, 6, 69, 173); //same color as wikipedia links //see also http://www.colorhexa.com/3232bb                
        public static System.Windows.Media.Color LightRed = System.Windows.Media.Color.FromArgb(255, 255, 247, 237);

        public static System.Windows.Media.FontFamily decompFontFamily = new System.Windows.Media.FontFamily("Calibri");
        public static System.Windows.Media.SolidColorBrush decompSolidColorBrush = new System.Windows.Media.SolidColorBrush(Globals.GrayExcelLine);
        
        public static double missingVariableArtificialNumber = 3e300d;  //max value for double is 1.7976931348623157E+308        
        public static double missingDatabankArtificialNumber = 4e300d;  //max value for double is 1.7976931348623157E+308        

        public static string RunGekkoTabToTextStuff_folder = "";  //not too pretty, but only used to send stuff from menu into thread

        public static CodeDomProvider csCompiler = CodeDomProvider.CreateProvider("CSharp");
        public static ICodeCompiler iCodeCompiler = Globals.csCompiler.CreateCompiler();

        public static Dictionary<long, string> prtCsSnippets = new Dictionary<long, string>();
        public static Dictionary<long, string> prtCsSnippetsHeaders = new Dictionary<long, string>();
        public static long prtCsSnippetsCounter = 0;
        
        public static string gekkoTimeIniCs = "GekkoTime t = Globals.tNull;";

        public static string iniFileSecretName = "[[RunGekkoIniFile]]";

        public static WindowIntellisense windowIntellisense = null;

        public static Table lastPrtOrMulprtTable = null;

        public static CommandMemory commandMemory = new CommandMemory();

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
