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


////!! do not use '_' as a char in a an option name. 'failsafe' is fine, but fail_safe is not.
//optionType2:			    
//			   question -> question
             
//			 //NOTE: THESE LINES CORRESPOND TO LINES IN Options.cs, line for line
			 
//			 | BUGFIX IMPORT EXPORT '='? yesNoSimple -> BUGFIX IMPORT EXPORT ^(ASTBOOL yesNoSimple)	//not mentioned in help		 			 	
//			 | BUGFIX GBK '='? yesNoSimple -> BUGFIX GBK ^(ASTBOOL yesNoSimple)                     //not mentioned in help
//			 | BUGFIX MISSING '='? yesNoSimple -> BUGFIX MISSING ^(ASTBOOL yesNoSimple)             //not mentioned in help
			
//			 | DATABANK question -> DATABANK question
//             | DATABANK COMPARE TABS '='? numberIntegerOrDouble -> DATABANK COMPARE TABS numberIntegerOrDouble
//             | DATABANK COMPARE TREL '='? numberIntegerOrDouble  -> DATABANK COMPARE TREL numberIntegerOrDouble
//             | DATABANK CREATE AUTO '='? yesNoSimple -> DATABANK CREATE AUTO ^(ASTBOOL yesNoSimple )			 
//			 | DATABANK FILE COPYLOCAL '='? yesNoSimple -> DATABANK FILE COPYLOCAL ^(ASTBOOL yesNoSimple )             
//             | DATABANK FILE GBK COMPRESS '='? yesNoSimple -> DATABANK FILE GBK COMPRESS ^(ASTBOOL yesNoSimple)
//             | DATABANK FILE GBK VERSION '='? numberIntegerOrDouble ->  DATABANK FILE GBK VERSION ^(ASTSTRINGSIMPLE numberIntegerOrDouble)  //NOTE: number converted to string
//			 | DATABANK FILE GBK INTERNAL '='? expression ->  DATABANK FILE GBK INTERNAL ^(ASTSTRINGSIMPLE expression)			 
//			 | DATABANK SEARCH '='? yesNoSimple -> DATABANK SEARCH ^(ASTBOOL yesNoSimple )	
			 
//			 | DECOMP MAXLAG '='? numberIntegerOrDouble ->  DECOMP MAXLAG numberIntegerOrDouble
//             | DECOMP MAXLEAD '='? numberIntegerOrDouble  -> DECOMP MAXLEAD numberIntegerOrDouble

//			 | FIT OLS REKUR DFMIN '='? numberIntegerOrDouble  -> FIT OLS REKUR DFMIN numberIntegerOrDouble

//			 | FOLDER question -> FOLDER question
//             | FOLDER '='? yesNoSimple -> FOLDER ^(ASTBOOL yesNoSimple)
//             | FOLDER BANK    '='? fileName ->  FOLDER BANK ^(ASTSTRINGSIMPLE fileName)
//             | FOLDER BANK1    '='? fileName ->  FOLDER BANK1 ^(ASTSTRINGSIMPLE fileName)
//             | FOLDER BANK2    '='? fileName ->  FOLDER BANK2 ^(ASTSTRINGSIMPLE fileName)
//             | FOLDER COMMAND    '='? fileName ->  FOLDER COMMAND ^(ASTSTRINGSIMPLE fileName)
//             | FOLDER COMMAND1    '='? fileName ->  FOLDER COMMAND1 ^(ASTSTRINGSIMPLE fileName)
//             | FOLDER COMMAND2    '='? fileName ->  FOLDER COMMAND2 ^(ASTSTRINGSIMPLE fileName)
//             | FOLDER HELP   '='? fileName ->  FOLDER HELP ^(ASTSTRINGSIMPLE fileName)
//             | FOLDER MENU     '='? fileName ->  FOLDER MENU ^(ASTSTRINGSIMPLE fileName)
//             | FOLDER MODEL   '='? fileName ->  FOLDER MODEL ^(ASTSTRINGSIMPLE fileName)
//             | FOLDER PIPE '='? fileName ->  FOLDER PIPE ^(ASTSTRINGSIMPLE fileName)			
//             | FOLDER TABLE    '='? fileName ->  FOLDER TABLE ^(ASTSTRINGSIMPLE fileName)
//             | FOLDER TABLE1   '='? fileName ->  FOLDER TABLE1 ^(ASTSTRINGSIMPLE fileName)
//             | FOLDER TABLE2   '='? fileName ->  FOLDER TABLE2 ^(ASTSTRINGSIMPLE fileName)
//             | FOLDER WORKING '='? fileName ->  FOLDER WORKING ^(ASTSTRINGSIMPLE fileName)

//			 | FREQ question -> FREQ question
//             | FREQ '='? name -> FREQ ^(ASTSTRINGSIMPLE name)
			 
//			 | GAMS EXE FOLDER '='? fileName -> GAMS EXE FOLDER ^(ASTSTRINGSIMPLE fileName)
//			 | GAMS FAST '='? yesNoSimple -> GAMS FAST ^(ASTBOOL yesNoSimple)
//			 | GAMS TIME DETECT AUTO '='? yesNoSimple -> GAMS TIME DETECT AUTO ^(ASTBOOL yesNoSimple)
//			 | GAMS TIME FREQ '='? expression -> GAMS TIME FREQ ^(ASTSTRINGSIMPLE expression)
//			 | GAMS TIME OFFSET '='? Integer -> GAMS TIME OFFSET ^(ASTINTEGER Integer)
//			 | GAMS TIME PREFIX '='? expression -> GAMS TIME PREFIX ^(ASTSTRINGSIMPLE expression)
//			 | GAMS TIME SET '='? expression -> GAMS TIME SET ^(ASTSTRINGSIMPLE expression)			 

//			 | INTERFACE question -> INTERFACE question
//			 | INTERFACE ALIAS '='? yesNoSimple -> INTERFACE ALIAS ^(ASTBOOL yesNoSimple)			 
//             | INTERFACE CLIPBOARD DECIMALSEPARATOR '='? optionInterfaceExcelDecimalseparator -> INTERFACE CLIPBOARD DECIMALSEPARATOR ^(ASTSTRINGSIMPLE optionInterfaceExcelDecimalseparator)
//			 | INTERFACE CSV DECIMALSEPARATOR '='? optionInterfaceExcelDecimalseparator -> INTERFACE CSV DECIMALSEPARATOR ^(ASTSTRINGSIMPLE optionInterfaceExcelDecimalseparator)
//			 | INTERFACE CSV DELIMITER '='? name -> INTERFACE CSV DELIMITER ^(ASTSTRINGSIMPLE name)
//			 | INTERFACE CSV NDEC '='? Integer -> INTERFACE CSV NDEC ^(ASTINTEGER Integer)
//			 | INTERFACE CSV PDEC '='? Integer -> INTERFACE CSV PDEC ^(ASTINTEGER Integer)			 
//			 | INTERFACE DEBUG '='? optionInterfaceDebug -> INTERFACE DEBUG ^(ASTSTRINGSIMPLE optionInterfaceDebug)
//			 | INTERFACE EDIT STYLE '='? name -> INTERFACE EDIT STYLE ^(ASTSTRINGSIMPLE name)
//			 | INTERFACE EXCEL LANGUAGE '='? optionInterfaceExcelLanguage -> INTERFACE EXCEL LANGUAGE ^(ASTSTRINGSIMPLE optionInterfaceExcelLanguage)
//             | INTERFACE EXCEL MODERNLOOK '='? yesNoSimple -> INTERFACE EXCEL MODERNLOOK ^(ASTBOOL yesNoSimple)             		 
//             | INTERFACE HELP COPYLOCAL '='? yesNoSimple -> INTERFACE HELP COPYLOCAL ^(ASTBOOL yesNoSimple)			 
//			 | INTERFACE MODE '='? mode2 -> INTERFACE MODE ^(ASTSTRINGSIMPLE mode2)
//			 | INTERFACE MUTE '='? name -> INTERFACE MUTE ^(ASTSTRINGSIMPLE name)	
//			 | INTERFACE REMOTE '='? yesNoSimple -> INTERFACE REMOTE ^(ASTBOOL yesNoSimple)
//			 | INTERFACE REMOTE FILE '='? fileName -> INTERFACE REMOTE FILE ^(ASTSTRINGSIMPLE fileName)
//             | INTERFACE SOUND '='? yesNoSimple -> INTERFACE SOUND ^(ASTBOOL yesNoSimple)
//             | INTERFACE SOUND TYPE '='? optionInterfaceSound -> INTERFACE SOUND TYPE ^(ASTSTRINGSIMPLE optionInterfaceSound)
//             | INTERFACE SOUND WAIT '='? Integer -> INTERFACE SOUND WAIT ^(ASTINTEGER Integer)
//             | INTERFACE SUGGESTIONS '='? optionInterfaceSuggestions -> INTERFACE SUGGESTIONS ^(ASTSTRINGSIMPLE optionInterfaceSuggestions)             | MODEL question -> MODEL question
//			 | INTERFACE TABLE OPERATORS '='? yesNoSimple ->  INTERFACE TABLE OPERATORS  ^(ASTBOOL yesNoSimple)
//			 | INTERFACE ZOOM '='? Integer -> INTERFACE ZOOM ^(ASTINTEGER Integer)

//			 | MENU question -> MENU question
//			 | MENU STARTFILE '='? fileName ->  MENU STARTFILE ^(ASTSTRINGSIMPLE fileName)

//			 | MODEL question -> MODEL question
//             | MODEL CACHE MAX '='? Integer -> MODEL CACHE MAX  ^(ASTINTEGER Integer)
//             | MODEL CACHE '='? yesNoSimple -> MODEL CACHE ^(ASTBOOL yesNoSimple)
//			 | MODEL GAMS DEP CURRENT '='? yesNoSimple -> MODEL GAMS DEP CURRENT ^(ASTBOOL yesNoSimple)
//			 | MODEL GAMS DEP METHOD '='? name -> MODEL GAMS DEP METHOD ^(ASTSTRINGSIMPLE name)
//			 | MODEL INFOFILE '='? optionModelInfoFile -> MODEL INFOFILE ^(ASTSTRINGSIMPLE optionModelInfoFile)
//			 | MODEL TYPE '='? name -> MODEL TYPE ^(ASTSTRINGSIMPLE name)			 

//			 | PLOT question -> PLOT question
//			 | PLOT ELEMENTS MAX '='? Integer -> PLOT ELEMENTS MAX ^(ASTINTEGER Integer)		 
//			 | PLOT LINES POINTS '='? yesNoSimple -> PLOT LINES POINTS ^(ASTBOOL yesNoSimple )	
//			 | PLOT XLABELS ANNUAL '='? optionPlotXlabels ->  PLOT XLABELS ANNUAL ^(ASTSTRINGSIMPLE optionPlotXlabels)
//			 | PLOT XLABELS DIGITS '='? Integer ->  PLOT XLABELS DIGITS  ^(ASTINTEGER Integer)
//			 | PLOT XLABELS NONANNUAL '='? optionPlotXlabels ->  PLOT XLABELS NONANNUAL ^(ASTSTRINGSIMPLE optionPlotXlabels)
//			 | PLOT DECIMALSEPARATOR '='? optionInterfaceExcelDecimalseparator ->  PLOT DECIMALSEPARATOR ^(ASTSTRINGSIMPLE optionInterfaceExcelDecimalseparator)
//			 | PLOT USING '='? fileName -> PLOT USING ^(ASTSTRINGSIMPLE fileName)			 
			
//			 | PRINT question -> PRINT question
//			 | PRINT COLLAPSE '='? optionPrintCollapse ->  PRINT COLLAPSE ^(ASTSTRINGSIMPLE optionPrintCollapse)
//			 | PRINT ELEMENTS MAX '='? Integer -> PRINT ELEMENTS MAX ^(ASTINTEGER Integer)		 
//			 | PRINT FREQ '='? optionPrintFreq ->  PRINT FREQ ^(ASTSTRINGSIMPLE optionPrintFreq)
//             | PRINT DISP MAXLINES '='? '-'? Integer -> PRINT DISP MAXLINES ^(ASTINTEGER '-'? Integer)  //can be set to -1
//             | PRINT FIELDS NDEC '='? Integer -> PRINT FIELDS NDEC ^(ASTINTEGER Integer)
//             | PRINT FIELDS NWIDTH '='? Integer -> PRINT FIELDS NWIDTH ^(ASTINTEGER Integer)
//             | PRINT FIELDS PDEC '='? Integer -> PRINT FIELDS PDEC ^(ASTINTEGER Integer)
//             | PRINT FIELDS PWIDTH '='? Integer -> PRINT FIELDS PWIDTH ^(ASTINTEGER Integer)
//             | PRINT FILEWIDTH '='? Integer -> PRINT FILEWIDTH ^(ASTINTEGER Integer)
//			 | PRINT MULPRT(GDIF|GDIFF) '='? yesNoSimple -> PRINT MULPRT GDIF ^(ASTBOOL yesNoSimple)
//             | PRINT MULPRT ABS '='? yesNoSimple -> PRINT MULPRT ABS ^(ASTBOOL yesNoSimple)
//             | PRINT MULPRT LEV '='? yesNoSimple -> PRINT MULPRT LEV ^(ASTBOOL yesNoSimple)
//             | PRINT MULPRT PCH '='? yesNoSimple -> PRINT MULPRT PCH ^(ASTBOOL yesNoSimple)
//             | PRINT MULPRT V '='? yesNoSimple -> PRINT MULPRT V ^(ASTBOOL yesNoSimple)
//             | PRINT PRT(DIF|DIFF) '='? yesNoSimple -> PRINT PRT DIF ^(ASTBOOL yesNoSimple)
//             | PRINT PRT(GDIF|GDIFF) '='? yesNoSimple -> PRINT PRT GDIF ^(ASTBOOL yesNoSimple)
//             | PRINT PRT ABS '='? yesNoSimple -> PRINT PRT ABS ^(ASTBOOL yesNoSimple)
//             | PRINT PRT PCH '='? yesNoSimple -> PRINT PRT PCH ^(ASTBOOL yesNoSimple)
//			 | PRINT WIDTH '='? Integer -> PRINT WIDTH ^(ASTINTEGER Integer)
//			 | PRINT SPLIT '='? yesNoSimple -> PRINT SPLIT ^(ASTBOOL yesNoSimple)

//			 | PYTHON EXE FOLDER '='? fileName -> PYTHON EXE FOLDER ^(ASTSTRINGSIMPLE fileName)

//			 | R EXE FOLDER '='? fileName -> R EXE FOLDER ^(ASTSTRINGSIMPLE fileName)
//			 | R EXE PATH '='? fileName -> R EXE PATH ^(ASTSTRINGSIMPLE fileName)  //obsolete, same as above and for legacy

//             | SERIES ARRAY IGNOREMISSING '='? yesNoSimple -> SERIES ARRAY IGNOREMISSING ^(ASTBOOL yesNoSimple)	//obsolete, delete in 3.3.x versions
//			 | SERIES DATA IGNOREMISSING '='? yesNoSimple -> SERIES DATA IGNOREMISSING ^(ASTBOOL yesNoSimple)	//obsolete, delete in 3.3.x versions

//			 | SERIES DYN '='? yesNoSimple -> SERIES DYN ^(ASTBOOL yesNoSimple)
//			 | SERIES DYN CHECK '='? yesNoSimple -> SERIES DYN  CHECK ^(ASTBOOL yesNoSimple)
//			 | SERIES FAILSAFE '='? yesNoSimple -> SERIES FAILSAFE ^(ASTBOOL yesNoSimple)			 			 
//			 | SERIES NORMAL PRINT MISSING '=' optionSeriesMissing -> SERIES NORMAL PRINT MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)			 
//             | SERIES NORMAL CALC MISSING '=' optionSeriesMissing -> SERIES NORMAL CALC MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)
//			 | SERIES NORMAL TABLE MISSING '=' optionSeriesMissing -> SERIES NORMAL TABLE MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)
//             | SERIES ARRAY PRINT MISSING '=' optionSeriesMissing -> SERIES ARRAY PRINT MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)
//             | SERIES ARRAY CALC MISSING '=' optionSeriesMissing -> SERIES ARRAY CALC MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)
//			 | SERIES ARRAY TABLE MISSING '=' optionSeriesMissing -> SERIES ARRAY TABLE MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)
//			 | SERIES DATA MISSING '=' optionSeriesMissing -> SERIES DATA MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)			 
			 
//			 | SHEET question -> SHEET question
//			 | SHEET COLLAPSE '='? optionPrintCollapse ->  SHEET COLLAPSE ^(ASTSTRINGSIMPLE optionPrintCollapse)			 
//			 | SHEET ENGINE '='? optionSheetEngine -> SHEET ENGINE ^(ASTSTRINGSIMPLE optionSheetEngine)
//			 | SHEET FREQ '='? optionPrintFreq ->  SHEET FREQ ^(ASTSTRINGSIMPLE optionPrintFreq)       			 
//			 | SHEET MULPRT(GDIF|GDIFF) '='? yesNoSimple -> SHEET MULPRT GDIF ^(ASTBOOL yesNoSimple)
//             | SHEET MULPRT ABS '='? yesNoSimple -> SHEET MULPRT ABS ^(ASTBOOL yesNoSimple)
//             | SHEET MULPRT LEV '='? yesNoSimple -> SHEET MULPRT LEV ^(ASTBOOL yesNoSimple)
//             | SHEET MULPRT PCH '='? yesNoSimple -> SHEET MULPRT PCH ^(ASTBOOL yesNoSimple)
//             | SHEET MULPRT V '='? yesNoSimple -> SHEET MULPRT V ^(ASTBOOL yesNoSimple)
//             | SHEET PRT(DIF|DIFF) '='? yesNoSimple -> SHEET PRT DIF ^(ASTBOOL yesNoSimple)
//             | SHEET PRT(GDIF|GDIFF) '='? yesNoSimple -> SHEET PRT GDIF ^(ASTBOOL yesNoSimple)
//             | SHEET PRT ABS '='? yesNoSimple -> SHEET PRT ABS ^(ASTBOOL yesNoSimple)
//             | SHEET PRT PCH '='? yesNoSimple -> SHEET PRT PCH ^(ASTBOOL yesNoSimple)			 
//			 | SHEET ROWS  '='? yesNoSimple -> SHEET ROWS ^(ASTBOOL yesNoSimple)
//			 | SHEET COLS  '='? yesNoSimple -> SHEET COLS ^(ASTBOOL yesNoSimple)		
			 			 
//             | SOLVE question -> SOLVE question
//             | SOLVE DATA CREATE AUTO '='? yesNoSimple -> SOLVE DATA CREATE AUTO ^(ASTBOOL yesNoSimple)
//             | SOLVE DATA IGNOREMISSING '='? yesNoSimple -> SOLVE DATA IGNOREMISSING ^(ASTBOOL yesNoSimple)
//             | SOLVE DATA INIT '='? yesNoSimple -> SOLVE DATA INIT ^(ASTBOOL yesNoSimple)
//             | SOLVE DATA INIT GROWTH '='? yesNoSimple -> SOLVE DATA INIT GROWTH ^(ASTBOOL yesNoSimple)						 
//			 | SOLVE DATA INIT GROWTH MAX '='? numberIntegerOrDouble -> SOLVE DATA INIT GROWTH MAX numberIntegerOrDouble
//             | SOLVE DATA INIT GROWTH MIN '='? numberIntegerOrDouble -> SOLVE DATA INIT GROWTH MIN numberIntegerOrDouble
//			 | SOLVE FAILSAFE '='? yesNoSimple -> SOLVE FAILSAFE ^(ASTBOOL yesNoSimple)
//			 | SOLVE FORWARD question -> SOLVE FORWARD question
//			 | SOLVE FORWARD DUMP '='? yesNoSimple -> SOLVE FORWARD DUMP ^(ASTBOOL yesNoSimple)  //common for fair and nfair
//             | SOLVE FORWARD FAIR CONV '='? optionSolveGaussConv -> SOLVE FORWARD FAIR CONV ^(ASTSTRINGSIMPLE optionSolveGaussConv )
//             | SOLVE FORWARD FAIR CONV1 ABS '='? numberIntegerOrDouble -> SOLVE FORWARD FAIR CONV1 ABS numberIntegerOrDouble
//             | SOLVE FORWARD FAIR CONV1 REL '='? numberIntegerOrDouble -> SOLVE FORWARD FAIR CONV1 REL numberIntegerOrDouble
//             | SOLVE FORWARD FAIR CONV2 TABS '='? numberIntegerOrDouble -> SOLVE FORWARD FAIR CONV2 TABS numberIntegerOrDouble
//             | SOLVE FORWARD FAIR CONV2 TREL '='? numberIntegerOrDouble -> SOLVE FORWARD FAIR CONV2 TREL numberIntegerOrDouble
//             | SOLVE FORWARD FAIR DAMP '='? numberIntegerOrDouble  -> SOLVE FORWARD FAIR DAMP numberIntegerOrDouble			
//             | SOLVE FORWARD FAIR ITERMAX '='? Integer -> SOLVE FORWARD FAIR ITERMAX ^(ASTINTEGER Integer)
//             | SOLVE FORWARD FAIR ITERMIN '='? Integer -> SOLVE FORWARD FAIR ITERMIN ^(ASTINTEGER Integer)			
//			 | SOLVE FORWARD NFAIR CONV '='? optionSolveGaussConv -> SOLVE FORWARD NFAIR CONV ^(ASTSTRINGSIMPLE optionSolveGaussConv )
//             | SOLVE FORWARD NFAIR CONV1 ABS '='? numberIntegerOrDouble -> SOLVE FORWARD NFAIR CONV1 ABS numberIntegerOrDouble
//             | SOLVE FORWARD NFAIR CONV1 REL '='? numberIntegerOrDouble -> SOLVE FORWARD NFAIR CONV1 REL numberIntegerOrDouble
//             | SOLVE FORWARD NFAIR CONV2 TABS '='? numberIntegerOrDouble -> SOLVE FORWARD NFAIR CONV2 TABS numberIntegerOrDouble
//             | SOLVE FORWARD NFAIR CONV2 TREL '='? numberIntegerOrDouble -> SOLVE FORWARD NFAIR CONV2 TREL numberIntegerOrDouble
//             | SOLVE FORWARD NFAIR DAMP '='? numberIntegerOrDouble  -> SOLVE FORWARD NFAIR DAMP numberIntegerOrDouble						
//             | SOLVE FORWARD NFAIR ITERMAX '='? Integer -> SOLVE FORWARD NFAIR ITERMAX ^(ASTINTEGER Integer)
//             | SOLVE FORWARD NFAIR ITERMIN '='? Integer -> SOLVE FORWARD NFAIR ITERMIN ^(ASTINTEGER Integer)			
//			 | SOLVE FORWARD NFAIR UPDATEFREQ '='? Integer -> SOLVE FORWARD NFAIR UPDATEFREQ ^(ASTINTEGER Integer)						
//			 | SOLVE FORWARD STACKED HORIZON '='? Integer -> SOLVE FORWARD STACKED HORIZON ^(ASTINTEGER Integer)
//             | SOLVE FORWARD METHOD '='? optionSolveForwardMethodOptions -> SOLVE FORWARD METHOD ^(ASTSTRINGSIMPLE optionSolveForwardMethodOptions)
//             | SOLVE FORWARD TERMINAL '='? optionSolveForwardTerminalOptions -> SOLVE FORWARD TERMINAL ^(ASTSTRINGSIMPLE optionSolveForwardTerminalOptions)
//			 | SOLVE FORWARD TERMINAL FEED '='? optionSolveForwardTerminalfeedOptions -> SOLVE FORWARD TERMINAL FEED ^(ASTSTRINGSIMPLE optionSolveForwardTerminalfeedOptions)
//			 | SOLVE GAUSS question -> SOLVE GAUSS question
//			 | SOLVE GAUSS CONV '='? optionSolveGaussConv -> SOLVE GAUSS CONV ^(ASTSTRINGSIMPLE optionSolveGaussConv )
//             | SOLVE GAUSS CONV IGNOREVARS '='? yesNoSimple -> SOLVE GAUSS CONV IGNOREVARS ^(ASTBOOL yesNoSimple)
//             | SOLVE GAUSS CONV1 ABS '='? numberIntegerOrDouble -> SOLVE GAUSS CONV1 ABS numberIntegerOrDouble
//             | SOLVE GAUSS CONV1 REL '='? numberIntegerOrDouble -> SOLVE GAUSS CONV1 REL numberIntegerOrDouble
//             | SOLVE GAUSS CONV2 TABS '='? numberIntegerOrDouble -> SOLVE GAUSS CONV2 TABS numberIntegerOrDouble
//             | SOLVE GAUSS CONV2 TREL '='? numberIntegerOrDouble -> SOLVE GAUSS CONV2 TREL numberIntegerOrDouble			
//			 | SOLVE GAUSS DAMP '='? numberIntegerOrDouble  -> SOLVE GAUSS DAMP numberIntegerOrDouble
//			 | SOLVE GAUSS DUMP '='? yesNoSimple -> SOLVE GAUSS DUMP ^(ASTBOOL yesNoSimple)
//             | SOLVE GAUSS ITERMAX '='? Integer -> SOLVE GAUSS ITERMAX ^(ASTINTEGER Integer)
//             | SOLVE GAUSS ITERMIN '='? Integer -> SOLVE GAUSS ITERMIN ^(ASTINTEGER Integer)
//             | SOLVE GAUSS REORDER '='? yesNoSimple -> SOLVE GAUSS REORDER ^(ASTBOOL yesNoSimple)
//             | SOLVE METHOD '='? optionSolveMethodOptions -> SOLVE METHOD ^(ASTSTRINGSIMPLE optionSolveMethodOptions)
//             | SOLVE NEWTON question -> SOLVE NEWTON question
//			 | SOLVE NEWTON BACKTRACK '='? yesNoSimple -> SOLVE NEWTON BACKTRACK ^(ASTBOOL yesNoSimple)
//			 | SOLVE NEWTON CONV ABS '='? numberIntegerOrDouble -> SOLVE NEWTON CONV ABS numberIntegerOrDouble
//             | SOLVE NEWTON INVERT '='? optionSolveNewtonInvert -> SOLVE NEWTON INVERT ^(ASTSTRINGSIMPLE optionSolveNewtonInvert)
//			 | SOLVE NEWTON ROBUST '='? yesNoSimple -> SOLVE NEWTON ROBUST ^(ASTBOOL yesNoSimple)	
//             | SOLVE NEWTON ITERMAX '='? Integer -> SOLVE NEWTON ITERMAX ^(ASTINTEGER Integer)
//             | SOLVE NEWTON UPDATEFREQ '='? Integer -> SOLVE NEWTON UPDATEFREQ ^(ASTINTEGER Integer)             
//             | SOLVE PRINT DETAILS '='? yesNoSimple -> SOLVE PRINT DETAILS ^(ASTBOOL yesNoSimple)
//             | SOLVE PRINT ITER '='? yesNoSimple -> SOLVE PRINT ITER ^(ASTBOOL yesNoSimple)
//             | SOLVE STATIC '='? yesNoSimple -> SOLVE STATIC ^(ASTBOOL yesNoSimple)             

//			 | STRING2 INTERPOLATE FORMAT VAL '='? expression -> STRING2 INTERPOLATE FORMAT VAL ^(ASTSTRINGSIMPLE expression)
			
//			 | SYSTEM CODE SPLIT '='? Integer -> SYSTEM CODE SPLIT ^(ASTINTEGER Integer)
//			 | SYSTEM CLONE '='? yesNoSimple -> SYSTEM CLONE ^(ASTBOOL yesNoSimple)
			
//			 | TABLE question -> TABLE question
//			 | TABLE DECIMALSEPARATOR '='? optionInterfaceExcelDecimalseparator ->  TABLE DECIMALSEPARATOR ^(ASTSTRINGSIMPLE optionInterfaceExcelDecimalseparator)
//             | TABLE HTML DATAWIDTH '='? numberIntegerOrDouble ->  TABLE HTML DATAWIDTH numberIntegerOrDouble
//			 | TABLE HTML FIRSTCOLWIDTH '='? numberIntegerOrDouble ->  TABLE HTML FIRSTCOLWIDTH numberIntegerOrDouble
//             | TABLE HTML FONT '='? fileName ->  TABLE HTML FONT ^(ASTSTRINGSIMPLE fileName)
//             | TABLE HTML FONTSIZE '='? numberIntegerOrDouble ->  TABLE HTML FONTSIZE numberIntegerOrDouble
//			 | TABLE HTML SECONDCOLWIDTH '='? numberIntegerOrDouble ->  TABLE HTML SECONDCOLWIDTH numberIntegerOrDouble
//			 | TABLE HTML SPECIALMINUS '='? yesNoSimple ->  TABLE HTML SPECIALMINUS ^(ASTBOOL yesNoSimple)
//             | TABLE IGNOREMISSINGVARS '='? yesNoSimple ->  TABLE IGNOREMISSINGVARS ^(ASTBOOL yesNoSimple) //obsolete, delete in 3.3.x versions			
//			 | TABLE MDATEFORMAT '='? expression ->  TABLE MDATEFORMAT ^(ASTSTRINGSIMPLE expression)			 
//             | TABLE STAMP '='? yesNoSimple ->  TABLE STAMP ^(ASTBOOL yesNoSimple)
//			 | TABLE THOUSANDSSEPARATOR '='? yesNoSimple ->  TABLE THOUSANDSSEPARATOR ^(ASTBOOL yesNoSimple)
//			 | TABLE TYPE '='? tableType ->  TABLE TYPE ^(ASTSTRINGSIMPLE tableType)			
			
//			 | TIMEFILTER question -> TIMEFILTER question
//             | TIMEFILTER '='? yesNoSimple -> TIMEFILTER ^(ASTBOOL yesNoSimple)
//             | TIMEFILTER TYPE '='? timefilterType -> TIMEFILTER TYPE ^(ASTSTRINGSIMPLE timefilterType)
//            ;

//optionModelInfoFile: YES | NO | TEMP;
//timefilterType: HIDE | AVG;
//tableType: TXT | HTML;
//optionPlotXlabels: AT2 | BETWEEN ;
//optionPrintCollapse: AVG | TOTAL | NONE;
//optionPrintFreq: SIMPLE | PRETTY;
//optionSolveMethodOptions : NEWTON | GAUSS ;
//optionSolveGaussConv : CONV1 | CONV2;
//optionDatabankFileFormatOptions : TSD | TSDX | GBK;
//optionDatabankLogic : DEFAULT | AREMOS;
//optionInterfaceDebug: NONE | DIALOG;
//optionInterfaceSound: BOWL | DING | NOTIFY  | RING;
//optionInterfaceSuggestions: NONE | OPTION; // | SOME | ALL;
//optionInterfaceExcelLanguage: DANISH | ENGLISH;
//optionInterfaceExcelDecimalseparator: PERIOD | COMMA;
//optionSolveNewtonInvert: LU | ITER;
//optionSolveForwardMethodOptions : STACKED | FAIR | NFAIR | NONE ;
//optionSolveForwardTerminalOptions : EXO | CONST | GROWTH ;
//optionSolveForwardTerminalfeedOptions : INTERNAL | EXTERNAL;
//optionSeriesMissing : ERROR | M | ZERO | SKIP;
//optionSheetEngine: EXCEL | INTERNAL;


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

            string xbool = "bool";
            string xstring = "string";
            string xint = "int"; // >= 0
            string xval = "val";
            string xval2String = "val2String";
            string xnameOrString = "nameOrString";
            string xnameOrString2Freq = "nameOrString2Freq";
            string xnameOrStringOrFilename = "nameOrStringOrFilename";
            string xoptionSeriesMissing = "optionSeriesMissing";
            string xsint = "sint";  //signed int            

            List<List<string>> rv = new List<List<string>>();

            List<string> types = new List<string>();
            types.Add(xbool);
            types.Add(xstring);
            types.Add(xint); // >= 0
            types.Add(xval);
            types.Add(xval2String);
            types.Add(xnameOrString);
            types.Add(xnameOrStringOrFilename);            
            types.Add(xnameOrString2Freq);
            types.Add(xoptionSeriesMissing);
            types.Add(xsint);  //signed int

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
            // + capitals (only so it is easier to see visually)
            // + 1 blank to separate idents            
            // + type can be: "", "", "", "", "", ""
            // option folder <enter> suggests = , but after =, there is bool but also other options.
            //option? check question at all places, implement
            //for int, check that -1 is ok, for instance PRINT DISP MAXLINES = -1
            
            //GO THROUGH EACH OPTION ONE BY ONE!!                        
            
            Add("BUGFIX IMPORT EXPORT", xbool);
            Add("BUGFIX MISSING", xbool);

            Add("DATABANK CREATE AUTO", xbool);
            Add("DATABANK FILE COPYLOCAL", xbool);
            Add("DATABANK FILE GBK COMPRESS", xbool);            
            Add("DATABANK FILE GBK VERSION", xval2String);
            Add("DATABANK FILE GBK INTERNAL", xnameOrStringOrFilename);
            Add("DATABANK SEARCH", xbool);
            Add("DECOMP MAXLAG", xint);
            Add("DECOMP MAXLEAD", xint);
            Add("FIT OLS REKUR DFMIN", xint);
            Add("FOLDER", xbool);
            Add("FOLDER BANK", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER BANK1", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER BANK2", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER COMMAND", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER COMMAND1", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER COMMAND2", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER HELP", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER MENU", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER MODEL", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER PIPE", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER TABLE", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER TABLE1", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER TABLE2", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FOLDER WORKING", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("FREQ", xnameOrString2Freq, "a", "q", "m", "d", "u");
            Add("GAMS EXE FOLDER", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("GAMS FAST", xbool);
            Add("GAMS TIME DETECT AUTO", xbool);
            Add("GAMS TIME FREQ", xnameOrString);
            Add("GAMS TIME OFFSET", xint);
            Add("GAMS TIME PREFIX", xnameOrString);
            Add("GAMS TIME SET", xnameOrString);
            Add("INTERFACE ALIAS", xbool);
            Add("INTERFACE CLIPBOARD DECIMALSEPARATOR", xnameOrString, "period", "comma");    //#kljsdfasfdlkj
            Add("INTERFACE CSV DECIMALSEPARATOR", xnameOrString, "period", "comma");          //#kljsdfasfdlkj
            Add("INTERFACE CSV DELIMITER", xnameOrString, "semicolon", "comma");
            Add("INTERFACE CSV NDEC", xint);
            Add("INTERFACE CSV PDEC", xint);
            Add("INTERFACE DEBUG", xnameOrString, "none", "dialog");
            Add("INTERFACE EDIT STYLE", xnameOrString, "gekko", "gekko2", "rstudio", "rstudio2");
            Add("INTERFACE EXCEL LANGUAGE", xnameOrString, "danish", "english");
            Add("INTERFACE EXCEL MODERNLOOK", xbool);
            Add("INTERFACE HELP COPYLOCAL", xbool);
            Add("INTERFACE MODE", xnameOrString, "mixed", "sim", "data");
            Add("INTERFACE MUTE", xnameOrString, xbool);
            Add("INTERFACE REMOTE", xbool);
            Add("INTERFACE REMOTE FILE", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("INTERFACE SOUND", xbool);
            Add("INTERFACE SOUND TYPE", xnameOrString, "bowl", "ding", "notify", "ring");
            Add("INTERFACE SOUND WAIT", xint);
            Add("INTERFACE SUGGESTIONS", xnameOrString, "none", "option");
            Add("INTERFACE TABLE OPERATORS", xbool);
            Add("INTERFACE ZOOM", xint);
            Add("MENU STARTFILE", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("MODEL CACHE MAX", xint);
            Add("MODEL CACHE", xbool);
            Add("MODEL GAMS DEP CURRENT", xbool);
            Add("MODEL GAMS DEP METHOD", xnameOrString, "lhs", "eqname");
            Add("MODEL INFOFILE", xnameOrString, "yes", "no", "temp");
            Add("MODEL TYPE", xnameOrString, "default", "gams");
            Add("PLOT ELEMENTS MAX", xint);
            Add("PLOT LINES POINTS", xbool);
            Add("PLOT XLABELS ANNUAL", xnameOrString, "at", "between");  //#hsfsksgsdfg
            Add("PLOT XLABELS DIGITS", xint);
            Add("PLOT XLABELS NONANNUAL", xnameOrString, "at", "between");    //#hsfsksgsdfg
            Add("PLOT DECIMALSEPARATOR", xnameOrString, "period", "comma");                   //#kljsdfasfdlkj
            Add("PLOT USING", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("PRINT COLLAPSE", xnameOrString, "avg", "total", "none");                      //#kllæksdfgsdg
            Add("PRINT ELEMENTS MAX", xint);
            Add("PRINT FREQ", xnameOrString, "simple", "pretty");
            Add("PRINT DISP MAXLINES", xsint);
            Add("PRINT FIELDS NDEC", xint);
            Add("PRINT FIELDS NWIDTH", xint);
            Add("PRINT FIELDS PDEC", xint);
            Add("PRINT FIELDS PWIDTH", xint);
            Add("PRINT FILEWIDTH", xint);
            {
                Add("PRINT MULPRT GDIF", xbool);
                Alias("PRINT MULPRT GDIFF", "PRINT MULPRT GDIF");
            }
            Add("PRINT MULPRT ABS", xbool);
            Add("PRINT MULPRT LEV", xbool);
            Add("PRINT MULPRT PCH", xbool);
            Add("PRINT MULPRT V", xbool);
            {
                Add("PRINT PRT DIF", xbool);
                Alias("PRINT PRT DIFF", "PRINT PRT DIF");
            }
            {
                Add("PRINT PRT GDIF", xbool);
                Alias("PRINT PRT GDIFF", "PRINT PRT GDIF");
            }
            Add("PRINT PRT ABS", xbool);
            Add("PRINT PRT PCH", xbool);
            Add("PRINT WIDTH", xint);
            Add("PRINT SPLIT", xbool);
            Add("PYTHON EXE FOLDER", xnameOrStringOrFilename); //cf. #jsadklgasj4j
            Add("R EXE FOLDER", xnameOrStringOrFilename); //cf. #jsadklgasj4j                                    
            Add("SERIES DYN", xbool);
            Add("SERIES DYN CHECK", xbool);
            Add("SERIES FAILSAFE", xbool);
            Add("SERIES NORMAL PRINT MISSING", xoptionSeriesMissing, "ERROR", "M", "ZERO", "SKIP");    //#ljfdssdfgsh
            Add("SERIES NORMAL CALC MISSING", xoptionSeriesMissing, "ERROR", "M", "ZERO", "SKIP");     //#ljfdssdfgsh
            Add("SERIES NORMAL TABLE MISSING", xoptionSeriesMissing, "ERROR", "M", "ZERO", "SKIP");    //#ljfdssdfgsh
            Add("SERIES ARRAY PRINT MISSING", xoptionSeriesMissing, "ERROR", "M", "ZERO", "SKIP");     //#ljfdssdfgsh
            Add("SERIES ARRAY CALC MISSING", xoptionSeriesMissing, "ERROR", "M", "ZERO", "SKIP");      //#ljfdssdfgsh
            Add("SERIES ARRAY TABLE MISSING", xoptionSeriesMissing, "ERROR", "M", "ZERO", "SKIP");     //#ljfdssdfgsh
            Add("SERIES DATA MISSING", xoptionSeriesMissing, "ERROR", "M", "ZERO", "SKIP");            //#ljfdssdfgsh
            Add("SHEET COLLAPSE", xnameOrString, "avg", "total", "none");         //#kllæksdfgsdg
            Add("SHEET ENGINE", xnameOrString, "excel", "internal");
            Add("SHEET FREQ", xnameOrString, "simple", "pretty");
            {
                Add("SHEET MULPRT GDIF", xbool);
                Alias("SHEET MULPRT GDIFF", "SHEET MULPRT GDIF");
            }
            Add("SHEET MULPRT ABS", xbool);
            Add("SHEET MULPRT LEV", xbool);
            Add("SHEET MULPRT PCH", xbool);
            Add("SHEET MULPRT V", xbool);            
            {
                Add("SHEET PRT DIF", xbool);
                Alias("SHEET PRT DIFF", "SHEET PRT DIF");
            }            
            {
                Add("SHEET PRT GDIF", xbool);
                Alias("SHEET PRT GDIFF", "SHEET PRT GDIF");
            }
            Add("SHEET PRT ABS", xbool);
            Add("SHEET PRT PCH", xbool);
            Add("SHEET ROWS", xbool);
            Add("SHEET COLS", xbool);
            Add("SOLVE DATA CREATE AUTO", xbool);
            Add("SOLVE DATA IGNOREMISSING", xbool);
            Add("SOLVE DATA INIT", xbool);
            Add("SOLVE DATA INIT GROWTH", xbool);
            Add("SOLVE DATA INIT GROWTH MAX", xval);
            Add("SOLVE DATA INIT GROWTH MIN", xval);
            Add("SOLVE FAILSAFE", xbool);
            Add("SOLVE FORWARD DUMP", xbool);
            Add("SOLVE FORWARD FAIR CONV", xnameOrString, "conv1", "conv2");   //#fxlsjffhsdks
            Add("SOLVE FORWARD FAIR CONV1 ABS", xval);
            Add("SOLVE FORWARD FAIR CONV1 REL", xval);
            Add("SOLVE FORWARD FAIR CONV2 TABS", xval);
            Add("SOLVE FORWARD FAIR CONV2 TREL", xval);
            Add("SOLVE FORWARD FAIR DAMP", xval);
            Add("SOLVE FORWARD FAIR ITERMAX", xint);
            Add("SOLVE FORWARD FAIR ITERMIN", xint);
            Add("SOLVE FORWARD NFAIR CONV", xnameOrString, "conv1", "conv2");   //#fxlsjffhsdks
            Add("SOLVE FORWARD NFAIR CONV1 ABS", xval);
            Add("SOLVE FORWARD NFAIR CONV1 REL", xval);
            Add("SOLVE FORWARD NFAIR CONV2 TABS", xval);
            Add("SOLVE FORWARD NFAIR CONV2 TREL", xval);
            Add("SOLVE FORWARD NFAIR DAMP", xval);
            Add("SOLVE FORWARD NFAIR ITERMAX", xint);
            Add("SOLVE FORWARD NFAIR ITERMIN", xint);
            Add("SOLVE FORWARD NFAIR UPDATEFREQ", xint);
            Add("SOLVE FORWARD STACKED HORIZON", xint);
            Add("SOLVE FORWARD METHOD", xnameOrString, "stacked", "fair", "nfair", "none");
            Add("SOLVE FORWARD TERMINAL", xnameOrString, "exo", "const", "growth");
            Add("SOLVE FORWARD TERMINAL FEED", xnameOrString, "internal", "external");
            Add("SOLVE GAUSS CONV", xnameOrString, "conv1", "conv2");   //#fxlsjffhsdks
            Add("SOLVE GAUSS CONV IGNOREVARS", xbool);
            Add("SOLVE GAUSS CONV1 ABS", xval);
            Add("SOLVE GAUSS CONV1 REL", xval);
            Add("SOLVE GAUSS CONV2 TABS", xval);
            Add("SOLVE GAUSS CONV2 TREL", xval);
            Add("SOLVE GAUSS DAMP", xval);
            Add("SOLVE GAUSS DUMP", xbool);
            Add("SOLVE GAUSS ITERMAX", xint);
            Add("SOLVE GAUSS ITERMIN", xint);
            Add("SOLVE GAUSS REORDER", xbool);
            Add("SOLVE METHOD", xnameOrString, "newton", "gauss");
            Add("SOLVE NEWTON BACKTRACK", xbool);
            Add("SOLVE NEWTON CONV ABS", xval);
            Add("SOLVE NEWTON INVERT", xnameOrString, "lu", "iter");
            Add("SOLVE NEWTON ROBUST", xbool);
            Add("SOLVE NEWTON ITERMAX", xint);
            Add("SOLVE NEWTON UPDATEFREQ", xint);
            Add("SOLVE PRINT DETAILS", xbool);
            Add("SOLVE PRINT ITER", xbool);
            Add("SOLVE STATIC", xbool);
            Add("STRING INTERPOLATE FORMAT VAL", xstring);
            Add("SYSTEM CODE SPLIT", xint);
            Add("SYSTEM CLONE", xbool);
            Add("TABLE DECIMALSEPARATOR", xnameOrString, "period", "comma");                  //#kljsdfasfdlkj
            Add("TABLE HTML DATAWIDTH", xval);
            Add("TABLE HTML FIRSTCOLWIDTH", xval);
            Add("TABLE HTML FONT", xnameOrString);
            Add("TABLE HTML FONTSIZE", xval);
            Add("TABLE HTML SECONDCOLWIDTH", xval);
            Add("TABLE HTML SPECIALMINUS", xbool);
            Add("TABLE IGNOREMISSINGVARS", xbool);
            Add("TABLE MDATEFORMAT", xstring);
            Add("TABLE STAMP", xbool);
            Add("TABLE THOUSANDSSEPARATOR", xbool);
            Add("TABLE TYPE", xnameOrString, "txt", "html");
            Add("TIMEFILTER", xbool);
            Add("TIMEFILTER TYPE", xnameOrString, "hide", "avg");

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

                ////These are so we can skip undocumented options (that are not mentioned in help or intellisense, but can still be used for instance to (try) to fix bugs
                //if (s_listFromReflection == "bugfix import export") { i2++; continue; }                
                //else if (s_listFromReflection == "bugfix missing") { i2++; continue; }                

                if (s_handmadeList != s_listFromReflection)
                {
                    G.Writeln2("*** ERROR: Mismatch regarding options, cf. Options.Syntax()");
                    throw new GekkoException();
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
                    if (!hasSeenEqual && !rv.Contains("=")) rv.Add("=");
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
            Write(null);
        }

        public void Write(string optionName5)
        {
            string optionName = optionName5.Replace("Program.options.", "");
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
                    string d = G.GetFreq(value);
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
                G.Write("Advice on finding and setting options ");
                G.WriteLink("here", "outputtab:" + lc.counter); G.Writeln(".");
            }
        }
    }
}
