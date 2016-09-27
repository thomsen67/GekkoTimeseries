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

//
// identDigit          0, 1, 01a, a38, ab, _a                                            [ident that can start with digit]
// wildcard            a*b, *, ?, *a?, 0?*1, <identDigit>                                [generalizes identDigit]
// expression          'a*b', 'ab', '*a?', %s, %s1+%s2, a[%i]*3, f(a, %b), [*], [x?]     [does not accept wildcard or identDigit, wildcard only in []].
// name                a{b}c                                                             [made from ident, identDigit, curly etc.]
// listItem            <name>:<name>, <name>, <identDigit>, <expression>                 [%s1+':'+%s2 could be used, or maybe {s1}:{s2}]

// ========================= SHORTCUTS =============================================
// You can do shortcuts with:
// - listItems --> will hook up to o.listItems in O object
// - fileName --> will hook up to o.fileName in O object, if you use ^(ASTHANDLEFILENAME fileName)
// - dates? --> "(leftAngle dates? RIGHTANGLE)?" will hook up to o.t1 and o.t2 in O object, if you use ^(ASTDATES dates?)
// - options:
//   You may use ASTOPT_STRING_x or ASTOPT_DATE_y or ASTOPT_VAL_z, and these are auto-converted to
//	 o117.opt_x = O.GetString(...), o117.opt_y = O.GetDate(...), o117.opt_z = O.GetVal(...) and emitted.
//	 So you just need to define for instance ASTOPT_STRING_x in the .g file, and make a .x string field in the O class,
//	 and the rest is automatic. No need to handle ASTOPT_STRING_x in the switch or do anything with node.Code.
//	 Example (in the .g file): CAPS (EQUAL yesNo)? -> ^(ASTOPT_STRING_CAPS yesNo?)	
//							   will emit "o117.opt_caps = ..." to node.Cs automatically.
//	 where yesNo accepts yes/no without quotes (or any expression, including 'yes' and 'no')
//	 see #astopt
// =============================================================================


grammar Cmd2;

options {
  language=CSharp2;
  output = AST;
  backtrack = true;   //otherwise too many errors...
  memoize = true;
  //k=2;  //must be 2 to deal with genr statements etc. Compiling grammar with k=6 dies with memory error
          //not setting k is equivalent to LL(*) which is probably best in all cases.
}

//Token definitions I
tokens {
    ASTLIBRARY;
	ASTPRTUSING;
	ASTOR;
	ASTAND;
	ASTNOT;
	ASTABS;
    ASTACCEPT;
    ASTADD;
    ASTANALYZE;
    ASTAPPEND;
    ASTAS;
    ASTASSIGNSTATEMENT;
    ASTASSIGNVARIABLE;
    ASTAT;
    ASTAVG;
    ASTBANK;
    ASTBANK;
    ASTBANKISSTARCHEATCODE;
    ASTBASEBANK;
    ASTBOOL;
    ASTBRACKET;
    ASTCAPS;
    ASTCELL;
    ASTCHECKOFF;
    ASTCLEAR2;
    ASTCLEAR;
    ASTCLEARALL;
    ASTCLONE;
    ASTCLOSE;
    ASTCLOSEALL;
    ASTCLOSEBANKS;
    ASTCLOSESTAR;
    ASTCLS;
    ASTCOLLAPSE;
    ASTCOLORS;
    ASTCOMPARE;
    ASTCOMPARECOMMAND;
    ASTCOPY;
    ASTCOPYWILDCARD1;
    ASTCOPYWILDCARD2;
    ASTCOPYWILDCARD3;
    ASTCOPYWILDCARD4;
    ASTCOPYWILDCARD;
    ASTCOUNT;
    ASTCREATE;
    ASTCREATEEXPRESSION;
    ASTCREATEQUESTION;
    ASTCURLY;
    ASTCURLYSIMPLE;
    ASTD;
    ASTDATA;
    ASTDATAADVANCED;
    ASTDATAFORMAT;
    ASTDATAORIENTATION;
    ASTDATE2;
    ASTDATE;
    ASTDATE;
    ASTDATES;
    ASTDATES;
    ASTDATESSTATEMENT;
    ASTDATESTATEMENT;
    ASTDECOMP;
    ASTDECOMPITEMS;
    ASTDECOMPTYPE;
    ASTDELETE;
    ASTDELETEALL;
    ASTDIF;
    ASTDIFPRT;
    ASTDIRECT;
    ASTDISP;
    ASTDISPLAY;
    ASTDISPSEARCH;
    ASTDOC;
    ASTDOLLARHASHNAMESIMPLE;
    ASTDOLLARHASHPAREN;
    ASTDOLLARPERCENTNAMESIMPLE;
    ASTDOLLARPERCENTPAREN;
    ASTDOTINDEXER;
    ASTDOUBLE;
    ASTDOUBLE;
    ASTDOUBLENEGATIVE;
    ASTDOWNLOAD;
    ASTDP;
    ASTDUMOF;
    ASTDUMON;
    ASTEDIT;
    ASTEFTER;
    ASTELSESTATEMENTS;
    ASTEMPTY;
    ASTEMPTYRANGEELEMENT;
    ASTENDO;
    ASTENDOQUESTION;
    ASTEXIT;
    ASTEXO;
    ASTEXOQUESTION;
    ASTEXPRESSION;
    ASTEXPRESSIONTUPLE;
    ASTFILENAME1;
    ASTFILENAME2;
    ASTFILENAME;
    ASTFILENAME;
    ASTFILENAMEFIRST1;
    ASTFILENAMEFIRST2;
    ASTFILENAMEFIRST3;
    ASTFILENAMEPART;
    ASTFILENAMEPART;
    ASTFILENAMEPART;
    ASTFILENAMEPARTBACKSLASH;
    ASTFILENAMEQUOTES;
    ASTFILENAMESTAR;
    ASTFINDMISSINGDATA;
    ASTFLAT;
    ASTFOR;
    ASTFORDATE;
    ASTFORLEFTSIDE2;
    ASTFORLEFTSIDE;
    ASTFORNAME;
    ASTFORRIGHTSIDE2;
    ASTFORRIGHTSIDE;
    ASTFORSTATEMENTS;
    ASTFORSTRING;
    ASTFORVAL;
    ASTFREQ;
    ASTFRML;
    ASTFRMLCODE;
    ASTFRMLTUPLE;
    ASTFUNCTION;
    ASTFUNCTION;
    ASTFUNCTIONDEF;
    ASTFUNCTIONDEFARG;
    ASTFUNCTIONDEFARGS;
    ASTFUNCTIONDEFCODE;
    ASTFUNCTIONDEFLHSTUPLE;
    ASTFUNCTIONDEFNAME;
    ASTFUNCTIONDEFRHSSIMPLE;
    ASTFUNCTIONDEFRHSTUPLE;
    ASTFUNCTIONDEFTYPE;
    ASTFUNCTIONSCALAR;
    ASTGDIF;
    ASTGDIFF;
    ASTGEKKOLABEL;
    ASTGENERIC1;
    ASTGENR;
    ASTGENR;
    ASTGENRINDEXER;
    ASTGENRLHSFUNCTION;
    ASTGENRLISTINDEXER2;
    ASTGENRLISTINDEXER;
    ASTGOTO;
    ASTHANDLEFILENAME;
    ASTHASH;
    ASTHASHNAMESIMPLE;
    ASTHASHPAREN;
    ASTHDG;
    ASTHEADING;
    ASTHELP;
    ASTHPFILTER;
    ASTHPFILTERLAMBDA;
    ASTHPFILTERLOG;
    ASTHTTP;
    ASTIDENT;
    ASTIDENTADVANCEDDOT;
    ASTIDENTDIGIT;
    ASTIDENTITYCODE;
    ASTIF;
    ASTIFCONDITION;
    ASTIFFALSE;
    ASTIFOPERATOR1;
    ASTIFOPERATOR2;
    ASTIFOPERATOR3;
    ASTIFOPERATOR4;
    ASTIFOPERATOR5;
    ASTIFOPERATOR6;
    ASTIFOPERATOR;
    ASTIFSTATEMENTS;
    ASTIFTRUE;
    ASTINDEX;
    ASTINDEXER;
    ASTINDEXERALONE;
    ASTINDEXERELEMENT;
    ASTINDEXERELEMENTBANK;
    ASTINDEXERELEMENTPLUS;
    ASTINFO;
    ASTINI;
    ASTINTEGER;
    ASTINTEGER;
    ASTINTEGERNEGATIVE;
    ASTITERSHOW;
    ASTLABEL1;
    ASTLABEL2;
    ASTLABELS;
    ASTLABELS;
    ASTLAGORLEAD;
    ASTLEFTSIDE;
    ASTLEV;
    ASTLIST2;
    ASTLIST2OLD;
    ASTLIST3;
    ASTLIST4;
    ASTLIST;
    ASTLISTCONCATENATION;
    ASTLISTDIFFERENCE;
    ASTLISTFILE;
    ASTLISTINTERSECTION;
    ASTLISTITEM;
    ASTLISTITEMS0;
    ASTLISTITEMS1;
    ASTLISTITEMS2;
    ASTLISTITEMS;
    ASTLISTITEMSNEW;
    ASTLISTITEMWILDRANGE;
    ASTLISTITEMWILDRANGEBANK;
    ASTLISTPREFIX;
    ASTLISTSORT;
    ASTLISTSTRIP;
    ASTLISTSUFFIX;
    ASTLISTUNION;
    ASTLISTWITHBANK;
	ASTLOCK;
	ASTUNLOCK;
    ASTM;
    ASTMACRO;
    ASTMACROPLUS;
    ASTMATRIX;
    ASTMATRIXCOL;
    ASTMATRIXINDEXER;
    ASTMATRIXROW;
    ASTMEM;
    ASTMENUTABLE;
    ASTMERGE;
    ASTMETA;
    ASTMISSING;
    ASTMODE;
    ASTMODEL;
    ASTMODELFILE;
    ASTMODEQUESTION;
    ASTMP;
    ASTMULBK;
    ASTN;
    ASTNAME2;
    ASTNAME;
    ASTNAMEDIGIT;
    ASTNAMESLIST;
    ASTNAMESTATEMENT;
    ASTNAMESUBSIMPLE;
    ASTNAMEWITHBANK;
    ASTNAMEWITHDOT;
    ASTNEW;
    ASTNEWTABLE;
    ASTNO;
    ASTNULL;
    ASTNUMBER;
    ASTOBJFUNCTION;
    ASTOLS;
    ASTOLSELEMENT;
    ASTOLSELEMENTS;
    ASTOPD;
    ASTOPEN;
    ASTOPENHELPER;
    ASTOPERATOR;
    ASTOPERATORDOLLAR;
    ASTOPERATORNODOLLAR;
    ASTOPM;
    ASTOPMP;
    ASTOPN;
    ASTOPP;
    ASTOPQ;
    ASTOPT1;
    ASTOPT2;
    ASTOPT_;
	ASTOPT_STRING_ERROR;
	ASTOPT_STRING_USING;
    ASTOPT_STRING_ABS ;
    ASTOPT_STRING_AFTER;
    ASTOPT_STRING_APPEND;
    ASTOPT_STRING_AREMOS;
    ASTOPT_STRING_CAPS;
    ASTOPT_STRING_CELL;
    ASTOPT_STRING_COLLAPSE;
    ASTOPT_STRING_COLORS;
    ASTOPT_STRING_COLS;
    ASTOPT_STRING_CSV;
    ASTOPT_STRING_D;
    ASTOPT_STRING_DATES;
    ASTOPT_STRING_DIRECT;
	ASTOPT_STRING_EDIT;
	ASTOPT_STRING_FIRST;
	ASTOPT_STRING_LAST;
    ASTOPT_STRING_FIX;
    ASTOPT_STRING_FROM;
    ASTOPT_STRING_GBK;
    ASTOPT_STRING_GEKKO18;
    ASTOPT_STRING_GEOMETRIC;
    ASTOPT_STRING_GNUPLOT;
    ASTOPT_STRING_HEADING;
    ASTOPT_STRING_HTML;
    ASTOPT_STRING_INFO;
    ASTOPT_STRING_KEEP;
    ASTOPT_STRING_LABEL;
    ASTOPT_STRING_LABELS;
    ASTOPT_STRING_LINEAR;
    ASTOPT_STRING_M;
    ASTOPT_STRING_M;
    ASTOPT_STRING_MERGE;
    ASTOPT_STRING_MP;
    ASTOPT_STRING_MUTE ;
    ASTOPT_STRING_MUTE;
    ASTOPT_STRING_N;
    ASTOPT_STRING_NAMES;
    ASTOPT_STRING_NONMODEL;
    ASTOPT_STRING_P;
    ASTOPT_STRING_PARAM;
    ASTOPT_STRING_PCIM;
    ASTOPT_STRING_PLOTCODE;
    ASTOPT_STRING_PRESERVE;
    ASTOPT_STRING_PRIM;
    ASTOPT_STRING_PRN;
    ASTOPT_STRING_PROT;
    ASTOPT_STRING_Q;
    ASTOPT_STRING_REPEAT;
    ASTOPT_STRING_RES;
    ASTOPT_STRING_RESPECT;
    ASTOPT_STRING_ROWS;
    ASTOPT_STRING_S;
	ASTOPT_STRING_SAVE;
	ASTOPT_STRING_SEC;
    ASTOPT_STRING_REF;
    ASTOPT_STRING_SERIES;
    ASTOPT_STRING_SHEET;
    ASTOPT_STRING_SOURCE;
    ASTOPT_STRING_SPLINE;
    ASTOPT_STRING_STAMP;
    ASTOPT_STRING_STATIC ;
    ASTOPT_STRING_TARGET ;
    ASTOPT_STRING_TO;
    ASTOPT_STRING_TSD;
    ASTOPT_STRING_TSDX;
    ASTOPT_STRING_TSP;
    ASTOPT_STRING_WINDOW;
    ASTOPT_STRING_XLS;
    ASTOPT_STRING_XLSX;
    ASTOPT_VAL_LAG;
    ASTOPT_VAL_REPLACE;
    ASTOPT_VAL_YMAX;
    ASTOPT_VAL_YMIN;
	ASTOPT_VAL_POS;
    ASTOPTION;
    ASTP;
    ASTPAUSE;
    ASTPCH;
    ASTPERCENT;
    ASTPERCENTNAMESIMPLE;
    ASTPERCENTPAREN;
    ASTPIPE;
    ASTPLACEHOLDER;  //does nothing
    ASTPOW;
    ASTPOW;
    ASTPRT2;
    ASTPRT;
    ASTPRTELEMENT;
    ASTPRTELEMENTDEC;
    ASTPRTELEMENTNDEC;
    ASTPRTELEMENTNWIDTH;
    ASTPRTELEMENTOPTIONFIELD;
    ASTPRTELEMENTPDEC;
    ASTPRTELEMENTPWIDTH;
    ASTPRTELEMENTS;
    ASTPRTELEMENTWIDTH;
    ASTPRTHEADING;
    ASTPRTITEMS;
    ASTPRTOPTION;
    ASTPRTOPTIONFIELD2;
    ASTPRTOPTIONFIELD3;
    ASTPRTOPTIONFIELD;
    ASTPRTROWS;
    ASTPRTSTAMP;
    ASTPRTTIMEFILTER;
    ASTPRTTITLE;
    ASTPRTTYPE;
    ASTPRTTYPE;
    ASTQ;
    ASTR_EXPORT;
    ASTR_EXPORTITEMS ;
    ASTR_FILE;
    ASTR_RUN;
    ASTRANGEWITHBANK;
    ASTREAD;
    ASTREAD;
    ASTREADTO;
    ASTREADWITHOPTIONS;
    ASTRENAME;
    ASTREPLACE;
    ASTRES;
    ASTRESET;
    ASTRESTART;
    ASTRETURN;
    ASTRETURNTUPLE;
    ASTRUN;
    ASTS;
    ASTSCALAR;
    ASTSD;
    ASTSDP;
    ASTSERIESQUESTION;
    ASTSHEET;
    ASTSHEETIMPORT;
    ASTSHOW;
    ASTSIGN;
    ASTSIM;
    ASTSIMPLEFUNCTION;
    ASTSMOOTH;
    ASTSN;
    ASTSP;
    ASTSPLICE;
    ASTSTAMP;
    ASTSTAR;
    ASTSTAR;
    ASTSTOP;
    ASTSTRING;
    ASTSTRING;
    ASTSTRINGINQUOTES;
    ASTSTRINGSIMPLE;
    ASTSTRINGSTATEMENT;
    ASTSYS;
    ASTTABLE;
    ASTTABLEALIGNCENTER;
    ASTTABLEALIGNLEFT;
    ASTTABLEALIGNRIGHT;
    ASTTABLEHIDELEFTBORDER;
    ASTTABLEHIDERIGHTBORDER;
    ASTTABLEINPUTFILE;
    ASTTABLEINPUTFILE;
    ASTTABLEMERGECOLS;
    ASTTABLENEXT;
    ASTTABLEOLD;
    ASTTABLEOPTIONFIELD;
    ASTTABLEOPTIONFIELDWINDOW;
    ASTTABLEOUTPUTFILE;
    ASTTABLEOUTPUTTYPE;
    ASTTABLEPRINT;
    ASTTABLESETBORDER;
    ASTTABLESETBOTTOMBORDER;
    ASTTABLESETDATES;
    ASTTABLESETLEFTBORDER;
    ASTTABLESETRIGHTBORDER;
    ASTTABLESETTEXT;
    ASTTABLESETTOPBORDER;
    ASTTABLESETVALUES;
    ASTTABLESETVALUESELEMENT;
    ASTTABLESHOWBORDERS;
    ASTTARGET;
    ASTTARGET;
    ASTTELL;
    ASTTEST;
    ASTTESTRANDOMMODEL;
    ASTTESTRANDOMMODELCHECK;
    ASTTIME;
    ASTTIMEFILTER;
    ASTTIMEFILTERPERIOD;
    ASTTIMEFILTERPERIODS;
    ASTTIMEOPTIONFIELD;
    ASTTIMEPERIOD;
    ASTTIMEQUESTION;
    ASTTIMESPAN;
    ASTTOTAL;
    ASTTRANSLATE;
    ASTTRANSPOSE;
    ASTTRUNCATE;
    ASTTUPLE;
    ASTTUPLEFUNCTIONSIMPLE;
    ASTTUPLEITEM;
    ASTTUPLEITEMS;
    ASTTUPLESIMPLE;
    ASTUNDOSIM;
    ASTUNFIX;
    ASTUNSWAP;
    ASTUPD;
    ASTUPD;
    ASTUPDADVANCED;
    ASTUPDDATA;
    ASTUPDOPERATOR;
    ASTUPDOPERATOREQUAL;
    ASTUPDOPERATOREQUALDOLLAR;
    ASTUPDOPERATORHASH;
    ASTUPDOPERATORHASHDOLLAR;
    ASTUPDOPERATORHAT;
    ASTUPDOPERATORHATDOLLAR;
    ASTUPDOPERATORPERCENT;
    ASTUPDOPERATORPERCENTDOLLAR;
    ASTUPDOPERATORPLUS;
    ASTUPDOPERATORPLUSDOLLAR;
    ASTUPDOPERATORSTAR;
    ASTUPDOPERATORSTARDOLLAR;
    ASTUPDX;
    ASTURL;
    ASTURLFIRST1;
    ASTURLFIRST2;
    ASTURLFIRST3;
    ASTURLPART;
    ASTV;
    ASTVAL;
    ASTVALSTATEMENT;
    ASTVARIABLE;
    ASTVARIABLELAGLEAD;
    ASTVARNAMEORLIST;
    ASTVERS;
    ASTWILDCARD;
    ASTWILDCARDWITHBANK;
    ASTWILDQUESTION;
    ASTWILDSTAR;
    ASTWRITE;
    ASTWRITEOPTION;
    ASTWRITEWITHOPTIONS;
    ASTX12A;
    ASTYES;
    ASTYMAX;
    ASTYMIN;
    ASTZERO;

    USING = 'USING';
	A= 'A'               ;
	DEFAULT = 'DEFAULT';
	LOGIC = 'LOGIC';
    ABS              = 'ABS';
    ABSOLUTE = 'absolute';  //used to indicate fixed date, eg. fY(2005) or tg(2001q3)
    ACCEPT = 'ACCEPT';
    ADD              = 'ADD'             ;
    AFTER            = 'AFTER'           ;
    AFTER2           = 'AFTER2'          ;
    ALIGNCENTER      = 'ALIGNCENTER';
    ALIGNLEFT      = 'ALIGNLEFT';
    ALIGNRIGHT      = 'ALIGNRIGHT';
    ALL              = 'ALL'          ;
    ANALYZE = 'ANALYZE';
    AND              = 'AND';
    APPEND           = 'APPEND'          ;
    AREMOS = 'AREMOS';
    AS               = 'AS';
    AUTO = 'AUTO';
    AVG          = 'AVG';
    BACKTRACK        = 'BACKTRACK'       ;
    BANK             = 'BANK'            ;
    BANK1            = 'BANK1'           ;
    BANK2            = 'BANK2'           ;
    BOWL             = 'BOWL';
    BY               = 'BY';
    CACHE            = 'CACHE';
    CALC             = 'CALC';
    CAPS             = 'CAPS'            ;
    CELL             = 'CELL'            ;
    CHANGE           = 'CHANGE'            ;
    CHECKOFF         = 'CHECKOFF'            ;
    CLEAR            = 'CLEAR'            ;
    CLEAR2 = 'CLEAR2';
    CLIP            = 'CLIP'           ;
    CLIPBOARD            = 'CLIPBOARD'           ;
    CLONE = 'CLONE';
    CLOSE            = 'CLOSE';
    CLOSEALL         = 'CLOSEALL'        ;
    CLOSEBANKS       = 'CLOSEBANKS'        ;
    CLS              = 'CLS'             ;
    CODE = 'CODE';
    COLLAPSE         = 'COLLAPSE';
    COLORS           = 'COLORS'          ;
    COLS             = 'COLS';
    COMMA            = 'COMMA';
    COMMAND              = 'COMMAND'             ;
    COMMAND1             = 'COMMAND1'            ;
    COMMAND2             = 'COMMAND2'            ;
    COMPARE          = 'COMPARE'         ;
    COMPRESS         = 'COMPRESS';
    CONST             = 'CONST';
    CONV             = 'CONV';
    CONV1            = 'CONV1';
    CONV2            = 'CONV2';
    COPY = 'COPY';
    COPYLOCAL        = 'COPYLOCAL'       ;
    COUNT = 'COUNT';
    CPLOT            = 'CPLOT'       ;
    CREATE           = 'CREATE'          ;
    CREATEVARS       = 'CREATEVARS'      ;
    CSV              = 'CSV'             ;
    CURROW           = 'CURROW';
    D             = 'D'            ;
    DAMP             = 'DAMP'            ;
    DANISH           = 'DANISH';
    DATA             = 'DATA'            ;
    DATABANK         = 'DATABANK'        ;
    DATAWIDTH             = 'DATAWIDTH'            ;
    DATE             = 'DATE'            ;
    DATES            = 'DATES'           ;
    DEBUG            = 'DEBUG'     ;
    DEC              = 'DEC';
    DECIMALSEPARATOR = 'DECIMALSEPARATOR';
    DECOMP           = 'DECOMP'          ;
    DELETE           = 'DELETE'          ;
    DETAILS          = 'DETAILS';
    DIALOG           = 'DIALOG'          ;
    DIF              = 'DIF';
    DIFF              = 'DIFF';
    DIFPRT           = 'DIFPRT'          ;
    DING             = 'DING';
    DIRECT = 'DIRECT';
    DISP             = 'DISP'            ;
    DISPLAY          = 'DISPLAY'         ;
    DOC = 'DOC';
    DOWNLOAD = 'DOWNLOAD';
    DP               = 'DP'         ;
    DUMOF            = 'DUMOF'           ;
    DUMOFF           = 'DUMOFF'          ;
    DUMON            = 'DUMON'           ;
    DUMP             = 'DUMP'            ;
    EDIT = 'EDIT';
    EFTER            = 'EFTER'           ;
    ELSE             = 'ELSE'            ;
    END              = 'END'             ;
    ENDO             = 'ENDO'            ;
    ENGLISH          = 'ENGLISH';
	ERROR            = 'ERROR';
    EXCEL            = 'EXCEL'           ;
	EXE            = 'EXE'           ;
    EXIT             = 'EXIT';
    EXO              = 'EXO'             ;
    EXP              = 'EXP';
    EXPORT = 'EXPORT';
    EXTERNAL = 'EXTERNAL';
    FAILSAFE         = 'FAILSAFE'        ;
    FAIR         = 'FAIR'        ;
    FALSE = 'false' ;
    FAST             = 'FAST';
    FEED = 'FEED';
    FEEDBACK         = 'FEEDBACK'        ;
    FIELDS             = 'FIELDS'            ;
    FILE             = 'FILE'            ;
    FILEWIDTH        = 'FILEWIDTH'       ;
    FILTER        = 'FILTER'       ;
    FINDMISSINGDATA      = 'FINDMISSINGDATA'     ;
    FIRST            = 'FIRST';
    FIRSTCOLWIDTH = 'FIRSTCOLWIDTH';
    FIX = 'FIX';
    FLAT             = 'FLAT'            ;
    FOLDER           = 'FOLDER'          ;
    FONT           = 'FONT'          ;
    FONTSIZE           = 'FONTSIZE'          ;
    FOR              = 'FOR'             ;
    FORMAT           = 'FORMAT'          ;
    FORWARD          = 'FORWARD';
    FREQ             = 'FREQ'            ;
    FRML             = 'FRML'            ;
    FROM = 'FROM';
    FUNCTION = 'FUNCTION';
    GAUSS            = 'GAUSS'           ;
    GBK = 'GBK';
    GDIF           = 'GDIF';
    GDIFF           = 'GDIFF';
    GEKKO18 = 'GEKKO18';
    GENR             = 'GENR'            ;
    GEOMETRIC = 'GEOMETRIC';
    GMULPRT          = 'GMULPRT'         ;
    GNUPLOT = 'GNUPLOT';
    GOAL             = 'GOAL';
    GOTO = 'GOTO';
    GRAPH            = 'GRAPH';
    GROWTH           = 'GROWTH';
    HDG = 'HDG';
    HEADING          = 'HEADING'         ;
    HELP             = 'HELP'            ;
    HIDE             = 'HIDE';
    HIDELEFTBORDER   = 'HIDELEFTBORDER';
    HIDERIGHTBORDER   = 'HIDERIGHTBORDER';
    HORIZON = 'HORIZON';
    HPFILTER         = 'HPFILTER';
    HTML             = 'HTML';
    IF               = 'IF'              ;
    IGNOREMISSING    = 'IGNOREMISSING'   ;
    IGNOREMISSINGVARS = 'IGNOREMISSINGVARS';
    IGNOREVARS       = 'IGNOREVARS'      ;
    IMPORT='IMPORT';
    INDEX            = 'INDEX'           ;
    INFO             = 'INFO'            ;
	INFOFILE             = 'INFOFILE'            ;
    INI             = 'INI'            ;
    INIT             = 'INIT'            ;
    INTERFACE        = 'INTERFACE'       ;
    INTERNAL = 'INTERNAL';
    INVERT           = 'INVERT';
    ITER             = 'ITER'         ;
    ITERMAX          = 'ITERMAX'         ;
    ITERMIN          = 'ITERMIN'         ;
    ITERSHOW          = 'ITERSHOW'         ;
    KEEP = 'KEEP';
    LABEL = 'LABEL';
    LABELS           = 'LABELS'          ;
    LAG = 'LAG';
    LANGUAGE         = 'LANGUAGE';
    LAST             = 'LAST';
    LEV              = 'LEV';
    LINEAR ='LINEAR';
    LINES            = 'LINES';
    LIST             = 'LIST'            ;
    LISTFILE='LISTFILE';
    LOG              = 'LOG';
	LOCK_             = 'LOCK';
	UNLOCK_           = 'UNLOCK';
    LU               = 'LU';
    M= 'M'               ;
    MACRO2           = 'MACRO2'          ;
    MAIN             = 'MAIN';  //alternative could be 'table' (table tab) or 'new' (floating window)
    MAT = 'MAT';
    MATRIX = 'MATRIX';
    MAX              = 'MAX'               ;
    MAXLINES         = 'MAXLINES';
    MEM              = 'MEM'          ;
    MENU             = 'MENU'          ;
    MENUTABLE              = 'MENUTABLE'               ;
    MERGE            = 'MERGE'          ;
    MERGECOLS     = 'MERGECOLS'          ;
    MESSAGE = 'MESSAGE';
    METHOD           = 'METHOD'          ;
    MIN              = 'MIN'               ;
    MIXED = 'MIXED';
    MODE = 'MODE';
    MODEL            = 'MODEL'           ;
    MODERNLOOK       = 'MODERNLOOK'      ;
    MP               = 'MP'               ;
    MULBK            = 'MULBK'           ;
    MULPCT           = 'MULPCT'          ;
    MULPRT           = 'MULPRT'          ;
    MUTE = 'MUTE';
    N= 'N';
    NAME             = 'NAME';
    NAMES             = 'NAMES';
    NDEC             = 'NDEC';
    NDIFPRT          = 'NDIFPRT'         ;
    NEGATE;
    NEGATE;
    NEW              = 'NEW'          ;
    NEWTON           = 'NEWTON'          ;
    NEXT          = 'NEXT'         ;
    NFAIR = 'NFAIR';
    NO               = 'no'              ;
    NOABS            = 'NOABS'              ;
    NOCR             = 'NOCR';
    NODIF            = 'NODIF'              ;
    NODIFF            = 'NODIFF'              ;
    NOFILTER         = 'NOFILTER';
    NOGDIF      = 'NOGDIF';
    NOGDIFF      = 'NOGDIFF';
    NOLEV            = 'NOLEV'              ;
    NONE             = 'NONE'              ;
    NONMODEL = 'NONMODEL';
    NOPCH            = 'NOPCH'              ;
	SAVE            = 'SAVE'              ;
    NOT              = 'NOT';
    NOTIFY           = 'NOTIFY';
    NOV              = 'NOV'              ;
    NWIDTH           = 'NWIDTH';
    NYTVINDU         = 'NYTVINDU'        ;
    OLS = 'OLS';
    OPEN             = 'OPEN';
    OPTION           = 'OPTION'          ;
    OR               = 'OR';
    P= 'P'               ;
    PARAM = 'PARAM';
    PARAM;
    PATCH        = 'PATCH';
	PATH        = 'PATH';
    PAUSE            = 'PAUSE';
    PCH              = 'PCH'             ;     //  --> don't activate this function --> will give problems (DLOG etc. are not stated either, they are recognized as idents)
    PCIM             = 'PCIM'       ;
    PCIMSTYLE        = 'PCIMSTYLE'       ;
    PCTPRT           = 'PCTPRT'          ;
    PDEC             = 'PDEC';
    PERIOD           = 'PERIOD'          ;
    PIPE             = 'PIPE'            ;
    PLOT            = 'PLOT'           ;
    PLOTCODE = 'PLOTCODE';
    POINTS           = 'POINTS';
	POS           = 'POS';
    PREFIX = 'PREFIX';
    PRETTY = 'PRETTY';
    PRI              = 'PRI'             ;
    PRIM = 'PRIM';
    PRINT            = 'PRINT'           ;
    PRINTCODES = 'PRINTCODES';
    PRN = 'PRN';
    PROT = 'PROT';
    PRT              = 'PRT'             ;
    PRTX             = 'PRTX'             ;
    PUDVALG          = 'PUDVALG';
    PWIDTH           = 'PWIDTH';
    Q= 'Q'               ;
    R= 'R'       ;
    R_EXPORT = 'R_EXPORT';
    R_FILE = 'R_FILE';
    R_RUN = 'R_RUN';
    RD= 'RD'       ;
    RDP= 'RDP'       ;
    READ             = 'READ'            ;
    REF = 'REF';
    REL              = 'REL'            ;
    RENAME = 'RENAME';
    REORDER          = 'REORDER'            ;
    REP = 'REP';
    REPEAT = 'REPEAT';
    REPLACE = 'REPLACE';
    RES              = 'RES'             ;
    RESET = 'RESET';
    RESPECT = 'RESPECT';
    RESTART = 'RESTART';
    RETURN           = 'RETURN'          ;
    RING             = 'RING';
    RN= 'RN'       ;
    ROWS             = 'ROWS';
    RP= 'RP'       ;
    RUN              = 'RUN'             ;
	LIBRARY = 'LIBRARY';
    SEARCH = 'SEARCH';
    SEC = 'SEC';
	SECONDCOLWIDTH = 'SECONDCOLWIDTH';
    SER2 = 'S___ER';
    SER='SER';
    SERIES2 = 'S___ERIES';
    SERIES='SERIES';
    SET              = 'SET'             ;  //not used?
    SETBORDER        = 'SETBORDER';
    SETBOTTOMBORDER     = 'SETBOTTOMBORDER';
    SETDATES      = 'SETDATES';
    SETLEFTBORDER     = 'SETLEFTBORDER';
    SETRIGHTBORDER     = 'SETRIGHTBORDER';
    SETTEXT       = 'SETTEXT';
    SETTOPBORDER     = 'SETTOPBORDER';
    SETVALUES    = 'SETVALUES';
    SHEET            = 'SHEET'           ;
    SHOW          = 'SHOW'         ;
    SHOWBORDERS     = 'SHOWBORDERS';
    SHOWPCH          = 'SHOWPCH'         ;
    SIGN             = 'SIGN';
    SIM              = 'SIM'             ;
    SIMPLE = 'SIMPLE';
    SKIP            = 'SKIP';
    SMOOTH = 'SMOOTH';
    SOLVE            = 'SOLVE'           ;
    SOME            = 'SOME'           ;
    SORT = 'SORT';
    SOUND            = 'SOUND'           ;
    SOURCE = 'SOURCE';
    SPECIALMINUS = 'SPECIALMINUS';
    SPLICE = 'SPLICE';
    SPLINE = 'SPLINE';
    SPLIT = 'SPLIT';
    STACKED = 'STACKED';
    STAMP            = 'STAMP'           ;
    STARTFILE        = 'STARTFILE'           ;
    STATIC           = 'STATIC'          ;
    STEP             = 'STEP';
    STOP             = 'STOP'            ;
    STRING2          = 'STRING'         ;
    STRIP = 'STRIP';
    SUFFIX = 'SUFFIX';
    SUGGESTIONS      = 'SUGGESTIONS'     ;
    SWAP           = 'SWAP';
    SYS              = 'SYS'             ;
    SYSTEM = 'SYSTEM';
    TABLE            = 'TABLE'           ;
    TABLE1            = 'TABLE1'           ;
    TABLE2            = 'TABLE2'           ;
    TABLEOLD            = 'TABLEOLD'           ;
    TABS             = 'TABS'            ;
    TARGET = 'TARGET';
    TELL             = 'TELL';
	TEMP             = 'TEMP';
    TERMINAL         = 'TERMINAL';
    TEST             = 'TEST';
    TESTRANDOMMODEL  = 'TESTRANDOMMODEL'         ;
    TESTRANDOMMODELCHECK  = 'TESTRANDOMMODELCHECK'         ;
    TESTSIM          = 'TESTSIM'         ;
    TIME             = 'TIME'            ;
    TIMEFILTER         = 'TIMEFILTER'        ;
    TIMESPAN         = 'TIMESPAN'        ;
    TITLE         = 'TITLE'        ;
    TO               = 'TO'            ;
    TOTAL            = 'TOTAL';
    TRANSLATE = 'TRANSLATE';
    TRANSPOSE        = 'TRANSPOSE'       ;
    TREL             = 'TREL'            ;
    TRUE = 'true' ;
    TRUNCATE         = 'TRUNCATE';
    TSD              = 'TSD'            ;
    TSDX             = 'TSDX'            ;
    TSP              = 'TSP'            ;
    TXT             = 'TXT';
    TYPE             = 'TYPE';
    U= 'U'               ;
    UABS             = '_ABS';
    UDIF             = '_DIF';
    UDIFF             = '_DIFF';
    UDVALG           = 'UDVALG'          ;
    UGDIF       = '_GDIF';
    UGDIFF       = '_GDIFF';
    ULEV             = '_LEV';
    UNDO             = 'UNDO'            ;
    UNFIX             = 'UNFIX'            ;
    UNSWAP           = 'UNSWAP';
    UPCH             = '_PCH';
    UPDATEFREQ       = 'UPDATEFREQ'      ;
    UPDX             = 'UPDX'             ;
    V= 'V'             ;
    VAL              = 'VAL'             ;
    VALUE            = 'VALUE'             ;
    VERS             = 'VERS'            ;
    VERSION             = 'VERSION'            ;
    VPRT             = 'VPRT'            ;
    WAIT             = 'WAIT';
    WIDTH            = 'WIDTH'           ;
    WINDOW           = 'WINDOW';
    WORKING          = 'WORKING';
    WPLOT            = 'WPLOT'           ;
    WRITE            = 'WRITE'           ;
    WUDVALG          = 'WUDVALG'         ;
    X12A = 'X12A';
    XLS              = 'XLS'             ;
    XLSX             = 'XLSX'            ;
    YES              = 'yes'             ;
    YMAX = 'YMAX';
    YMIN = 'YMIN';
    ZERO             = 'ZERO'            ;
    ZOOM = 'ZOOM';
    ZVAR             = 'ZVAR'            ;
}

                              @parser::namespace { Gekko }

                              @lexer::namespace { Gekko }

                              @members {

                                 private CommonToken token(string text, int type, int line) {
                                                               CommonToken t = new CommonToken(type, text);
                                                           t.Line = line;
                                                               return t;
                                                            }
                                  private System.Collections.Generic.List<string> errors = new System.Collections.Generic.List<string>();
                                  private System.Collections.Generic.List<string> prtItems = new System.Collections.Generic.List<string>();
                                  public override void DisplayRecognitionError(string[] tokenNames,
                                                                      RecognitionException e) {
                                      string hdr = GetErrorHeader(e);
                                      string msg = GetErrorMessage(e, tokenNames);
                                      errors.Add(e.Line + "¤" + e.CharPositionInLine + "¤" + hdr + "¤" + msg);
                                  }
                                  public System.Collections.Generic.List<string> GetErrors() {
                                      return errors;
                                  }


                              }

                              @lexer::members {

                                public static System.Collections.Generic.Dictionary<string, int> kw = GetKw();

                                public static System.Collections.Generic.Dictionary<string, int> GetKw()
                                {
                                        System.Collections.Generic.Dictionary<string, int> d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
										d.Add("USING"    ,   USING     );
										d.Add("ERROR"    ,   ERROR     );
										d.Add("_ABS"    ,   UABS     );
                                        d.Add("_DIF"    ,   UDIF     );
                                        d.Add("_DIFF"   ,   UDIFF     );
                                        d.Add("_GDIF"              ,   UGDIF     );
                                        d.Add("_GDIFF"             ,   UGDIFF     );
                                        d.Add("_LEV"    ,   ULEV     );
                                        d.Add("_PCH"    ,   UPCH     );
                                        d.Add("a"       , A       );
										d.Add("DEFAULT"       , DEFAULT       );
										d.Add("LOGIC"       , LOGIC       );
                                        d.Add("abs"     , ABS       );
                                        d.Add("ACCEPT" ,ACCEPT);
                                        d.Add("add"     , ADD       );
                                        d.Add("after"   , AFTER     );
                                        d.Add("after2"  , AFTER2    );
                                        d.Add("ALIGNCENTER"             , ALIGNCENTER              );
                                        d.Add("ALIGNLEFT"             , ALIGNLEFT              );
                                        d.Add("ALIGNRIGHT"             , ALIGNRIGHT              );
                                        d.Add("all"  , ALL    );
                                        d.Add("ANALYZE" ,ANALYZE);
                                        d.Add("and"  , AND    );
                                        d.Add("append"  , APPEND     );
                                        d.Add("AREMOS", AREMOS);
                                        d.Add("as"  , AS    );
                                        d.Add("AUTO", AUTO);
                                        d.Add("avg"  , AVG    );
                                        d.Add("backtrack"               , BACKTRACK );
                                        d.Add("bank"    , BANK );
                                        d.Add("bank1"    , BANK1 );
                                        d.Add("bank2"    , BANK2 );
                                        d.Add("bowl"    , BOWL );
                                        d.Add("by"    , BY );
                                        d.Add("cache"    , CACHE );
                                        d.Add("calc"    , CALC );
                                        d.Add("caps"    , CAPS );
                                        d.Add("cell"    , CELL );
                                        d.Add("change"    , CHANGE );
                                        d.Add("checkoff"    , CHECKOFF );
                                        d.Add("clear"   , CLEAR  );
                                        d.Add("clear2"   , CLEAR2  );
                                        d.Add("clip"   , CLIP       );
                                        d.Add("clipboard"   , CLIPBOARD       );
                                        d.Add("CLONE", CLONE);
                                        d.Add("close"  , CLOSE );
                                        d.Add("closeall", CLOSEALL  );
                                        d.Add("closebanks", CLOSEBANKS  );
                                        d.Add("cls"     , CLS       );
                                        d.Add("CODE" ,CODE);
                                        d.Add("collapse"  , COLLAPSE );
                                        d.Add("colors"  , COLORS );
                                        d.Add("cols"  , COLS );
                                        d.Add("comma"   , COMMA               );
                                        d.Add("command"     , COMMAND       );
                                        d.Add("command1"     , COMMAND1       );
                                        d.Add("command2"     , COMMAND2       );
                                        d.Add("compare" , COMPARE );
                                        d.Add("compress" , COMPRESS );
                                        d.Add("const" , CONST );
                                        d.Add("conv"    , CONV );
                                        d.Add("conv1"    , CONV1 );
                                        d.Add("conv2"    , CONV2 );
                                        d.Add("copy",COPY);
                                        d.Add("copylocal"               , COPYLOCAL               );
                                        d.Add("COUNT", COUNT);
                                        d.Add("cplot"   , CPLOT               );
                                        d.Add("create"  , CREATE    );
                                        d.Add("createvars"              , CREATEVARS);
                                        d.Add("csv"     , CSV       );
                                        d.Add("currow"  , CURROW       );
                                        d.Add("d"    , D      );
                                        d.Add("damp"    , DAMP      );
                                        d.Add("danish"    , DANISH      );
                                        d.Add("data"    , DATA      );
                                        d.Add("databank", DATABANK      );
                                        d.Add("datawidth"    , DATAWIDTH      );
                                        d.Add("date"    , DATE      );
                                        d.Add("dates"   , DATES      );
                                        d.Add("debug"   , DEBUG     );
                                        d.Add("dec"     , DEC );
                                        d.Add("decimalseparator"       , DECIMALSEPARATOR    );
                                        d.Add("decomp"  , DECOMP    );
                                        d.Add("delete"  , DELETE    );
                                        d.Add("details"  , DETAILS   );
                                        d.Add("dialog"  , DIALOG      );
                                        d.Add("dif"  , DIF      );
                                        d.Add("diff"  , DIFF      );
                                        d.Add("difprt"  , DIFPRT      );
                                        d.Add("ding"    , DING      );
                                        d.Add("DIRECT", DIRECT);
                                        d.Add("disp"    , DISP      );
                                        d.Add("display" , DISPLAY   );
                                        d.Add("DOC" ,DOC);
                                        d.Add("DOWNLOAD" ,DOWNLOAD);
                                        d.Add("dp"      , DP   );
                                        d.Add("dumof"   , DUMOF     );
                                        d.Add("dumoff"  , DUMOFF    );
                                        d.Add("dumon"   , DUMON     );
                                        d.Add("dump"    , DUMP      );
                                        d.Add("EDIT" ,EDIT);
                                        d.Add("efter"   , EFTER     );
                                        d.Add("else"    , ELSE);
                                        d.Add("end"     , END      );
                                        d.Add("endo"    , ENDO      );
                                        d.Add("english"              , ENGLISH);
                                        d.Add("excel"   , EXCEL     );
										d.Add("exe"   , EXE     );
                                        d.Add("exit"    , EXIT       );
                                        d.Add("exo"     , EXO       );
                                        d.Add("exp"     , EXP       );
                                        d.Add("EXPORT", EXPORT);
                                        d.Add("EXTERNAL", EXTERNAL);
                                        d.Add("failsafe", FAILSAFE  );
                                        d.Add("fair", FAIR  );
                                        d.Add("fast", FAST  );
                                        d.Add("FEED", FEED);
                                        d.Add("feedback", FEEDBACK  );
                                        d.Add("fields"  , FIELDS  );
                                        d.Add("file"    , FILE  );
                                        d.Add("filewidth"               , FILEWIDTH  );
                                        d.Add("filter"               , FILTER  );
                                        d.Add("findmissingdata"         , FINDMISSINGDATA);
                                        d.Add("first"    , FIRST  );
                                        d.Add("FIRSTCOLWIDTH" ,FIRSTCOLWIDTH);
                                        d.Add("FIX", FIX);
                                        d.Add("flat"    , FLAT      );
                                        d.Add("folder"  , FOLDER  );
                                        d.Add("font"  , FONT  );
                                        d.Add("fontsize"  , FONTSIZE  );
                                        d.Add("for"    , FOR       );
                                        d.Add("format"  , FORMAT  );
                                        d.Add("forward"  , FORWARD  );
                                        d.Add("freq"    , FREQ      );
                                        d.Add("frml"    , FRML      );
                                        d.Add("FROM", FROM);
                                        d.Add("function", FUNCTION);
                                        d.Add("gauss"   , GAUSS     );
                                        d.Add("GBK" ,GBK);
                                        d.Add("gdif"    , GDIF   );
                                        d.Add("gdiff"    , GDIFF   );
                                        d.Add("GEKKO18", GEKKO18);
                                        d.Add("genr"    , GENR      );
                                        d.Add("GEOMETRIC", GEOMETRIC);
                                        d.Add("gmulprt" , GMULPRT   );
                                        d.Add("GNUPLOT" ,GNUPLOT);
                                        d.Add("goal"    , GOAL   );
                                        d.Add("GOTO", GOTO);
                                        d.Add("graph"    , GRAPH   );
                                        d.Add("growth"    , GROWTH   );
                                        d.Add("HDG",HDG);
                                        d.Add("heading" , HEADING   );
                                        d.Add("help"    , HELP      );
                                        d.Add("hide"    , HIDE      );
                                        d.Add("hideleftborder"          , HIDELEFTBORDER      );
                                        d.Add("hiderightborder"          , HIDERIGHTBORDER      );
                                        d.Add("HORIZON", HORIZON);
                                        d.Add("hpfilter"    , HPFILTER      );
                                        d.Add("html"    , HTML      );
                                        d.Add("if"      , IF      );
                                        d.Add("ignoremissing"           , IGNOREMISSING             );
                                        d.Add("IGNOREMISSINGVARS"           , IGNOREMISSINGVARS              );
                                        d.Add("ignorevars"              , IGNOREVARS             );
                                        d.Add("import", IMPORT);
                                        d.Add("index"   , INDEX      );
                                        d.Add("info"    , INFO      );
										d.Add("infofile"    , INFOFILE      );
                                        d.Add("ini"    , INI      );
                                        d.Add("init"    , INIT      );
                                        d.Add("interface"               , INTERFACE );
                                        d.Add("INTERNAL", INTERNAL);
                                        d.Add("INVERT"    , INVERT      );
                                        d.Add("iter"    , ITER   );
                                        d.Add("itermax" , ITERMAX   );
                                        d.Add("itermin" , ITERMIN   );
                                        d.Add("itershow" , ITERSHOW   );
                                        d.Add("KEEP", KEEP);
                                        d.Add("LABEL" ,LABEL);
                                        d.Add("labels"  , LABELS      );
                                        d.Add("LAG" ,LAG);
                                        d.Add("language"  , LANGUAGE      );
                                        d.Add("last"  , LAST      );
                                        d.Add("lev"  , LEV      );
                                        d.Add("LINEAR",LINEAR);
                                        d.Add("lines"    , LINES   );
                                        d.Add("list"    , LIST      );
                                        d.Add("listfile",LISTFILE);
                                        d.Add("log"  , LOG      );
										d.Add("lock"  , LOCK_      );
										d.Add("unlock"  , UNLOCK_      );
                                        d.Add("lu"    , LU      );
                                        d.Add("m"       , M    );
                                        d.Add("macro"   , MACRO2     );
                                        d.Add("main"   , MAIN     );
                                        d.Add("mat",MAT);
                                        d.Add("matrix",MATRIX);
                                        d.Add("max"   , MAX    );
                                        d.Add("maxlines", MAXLINES    );
                                        d.Add("mem"     , MEM     );
                                        d.Add("menu"     , MENU     );
                                        d.Add("menutable"     , MENUTABLE     );
                                        d.Add("merge"   , MERGE     );
                                        d.Add("MERGECOLS"            , MERGECOLS);
                                        d.Add("MESSAGE", MESSAGE);
                                        d.Add("method"  , METHOD    );
                                        d.Add("min"   , MIN    );
                                        d.Add("MIXED" ,MIXED);
                                        d.Add("MODE" ,MODE);
                                        d.Add("model"   , MODEL     );
                                        d.Add("modernlook"              , MODERNLOOK);
                                        d.Add("mp" , MP);
                                        d.Add("mulbk"   , MULBK     );
                                        d.Add("mulpct"  , MULPCT    );
                                        d.Add("mulprt"  , MULPRT    );
                                        d.Add("MUTE" , MUTE);
                                        d.Add("n"       , N    );
                                        d.Add("NAME",NAME);
                                        d.Add("NAMES",NAMES);
                                        d.Add("ndec"    , NDEC    );
                                        d.Add("ndifprt" , NDIFPRT    );
                                        d.Add("new"     , NEW       );
                                        d.Add("newton"  , NEWTON    );
                                        d.Add("NEXT" , NEXT    );
                                        d.Add("NFAIR", NFAIR);
                                        d.Add("no"      , NO        );
                                        d.Add("noabs"      , NOABS        );
                                        d.Add("nocr"      , NOCR        );
                                        d.Add("nodif"      , NODIF        );
                                        d.Add("nodiff"      , NODIFF        );
                                        d.Add("nofilter"      , NOFILTER        );
                                        d.Add("nogdif"      , NOGDIF        );
                                        d.Add("nogdiff"      , NOGDIFF        );
                                        d.Add("nolev"      , NOLEV        );
                                        d.Add("none"    , NONE        );
                                        d.Add("NONMODEL" ,NONMODEL);
                                        d.Add("nopch"      , NOPCH        );
										d.Add("save"      , SAVE        );
                                        d.Add("not"  , NOT        );
                                        d.Add("notify"  , NOTIFY        );
                                        d.Add("nov"      , NOV        );
                                        d.Add("nwidth"  , NWIDTH    );
                                        d.Add("nytvindu", NYTVINDU  );
                                        d.Add("ols", OLS);
                                        d.Add("open", OPEN  );
                                        d.Add("option"  , OPTION    );
                                        d.Add("or", OR  );
                                        d.Add("p"       , P        );
                                        d.Add("PARAM", PARAM);
                                        d.Add("PATCH"       , PATCH        );
										d.Add("PATH"       , PATH        );
                                        d.Add("pause"   , PAUSE        );
                                        d.Add("pch"     , PCH        );
                                        d.Add("pcim"    , PCIM );
                                        d.Add("pcimstyle"               , PCIMSTYLE );
                                        d.Add("pctprt"  , PCTPRT    );
                                        d.Add("pdec"    , PDEC    );
                                        d.Add("period"  , PERIOD    );
                                        d.Add("pipe"    , PIPE      );
                                        d.Add("plot"   , PLOT     );
                                        d.Add("PLOTCODE" ,PLOTCODE);
                                        d.Add("points"    , POINTS   );
										d.Add("pos"    , POS   );
                                        d.Add("prefix"    , PREFIX   );
                                        d.Add("PRETTY" ,PRETTY);
                                        d.Add("pri"     , PRI     );
                                        d.Add("PRIM", PRIM);
                                        d.Add("print"   , PRINT     );
                                        d.Add("PRINTCODES" ,PRINTCODES);
                                        d.Add("PRN", PRN);
                                        d.Add("PROT", PROT);
                                        d.Add("prt"     , PRT       );
                                        d.Add("prtx"    , PRTX       );
                                        d.Add("pudvalg" , PUDVALG       );
                                        d.Add("pwidth"  , PWIDTH    );
                                        d.Add("q"       , Q     );
                                        d.Add("r"               , R );
                                        d.Add("R_EXPORT" , R_EXPORT);
                                        d.Add("R_FILE" ,R_FILE);
                                        d.Add("R_RUN" , R_RUN);
                                        d.Add("rd"               , RD );
                                        d.Add("rdp"               , RDP );
                                        d.Add("read"    , READ      );
                                        d.Add("REF", REF);
                                        d.Add("rel"     , REL      );
                                        d.Add("rename",RENAME);
                                        d.Add("reorder" , REORDER      );
                                        d.Add("rep", REP);
                                        d.Add("REPEAT", REPEAT);
                                        d.Add("REPLACE"    , REPLACE     );
                                        d.Add("res"     , RES       );
                                        d.Add("RESET", RESET);
                                        d.Add("respect",RESPECT);
                                        d.Add("RESTART", RESTART);
                                        d.Add("return"  , RETURN    );
                                        d.Add("ring"    , RING    );
                                        d.Add("rn"               , RN );
                                        d.Add("rows"    , ROWS    );
                                        d.Add("rp"               , RP );
                                        d.Add("run"     , RUN       );
										d.Add("library"     , LIBRARY       );
                                        d.Add("S___ER" ,SER2);
                                        d.Add("S___ERIES" ,SERIES2);
                                        d.Add("SEARCH", SEARCH);
                                        d.Add("SECONDCOLWIDTH" ,SECONDCOLWIDTH);
										d.Add("SEC" ,SEC);
                                        d.Add("ser",SER);
                                        d.Add("series",SERIES);
                                        d.Add("set"     , SET       );
                                        d.Add("setborder"               , SETBORDER               );
                                        d.Add("SETBOTTOMBORDER"            , SETBOTTOMBORDER               );
                                        d.Add("SETDATES"             , SETDATES               );
                                        d.Add("SETLEFTBORDER"            , SETLEFTBORDER               );
                                        d.Add("SETRIGHTBORDER"            , SETRIGHTBORDER               );
                                        d.Add("SETTEXT"              , SETTEXT               );
                                        d.Add("SETTOPBORDER"            , SETTOPBORDER               );
                                        d.Add("SETVALUES"           , SETVALUES               );
                                        d.Add("sheet"   , SHEET       );
                                        d.Add("SHOW", SHOW);
                                        d.Add("showborders"          , SHOWBORDERS      );
                                        d.Add("showpch" , SHOWPCH   );
                                        d.Add("sign"     , SIGN       );
                                        d.Add("sim"     , SIM       );
                                        d.Add("SIMPLE" ,SIMPLE);
                                        d.Add("skip"          , SKIP      );
                                        d.Add("smooth", SMOOTH);
                                        d.Add("solve"   , SOLVE     );
                                        d.Add("some"   , SOME     );
                                        d.Add("sort"    , SORT   );
                                        d.Add("sound"   , SOUND     );
                                        d.Add("SOURCE" ,SOURCE);
                                        d.Add("SPECIALMINUS" ,SPECIALMINUS);
                                        d.Add("splice", SPLICE);
                                        d.Add("SPLINE", SPLINE);
                                        d.Add("SPLIT" ,SPLIT);
                                        d.Add("STACKED", STACKED);
                                        d.Add("stamp"   , STAMP    );
                                        d.Add("startfile"  , STARTFILE    );
                                        d.Add("static"  , STATIC    );
                                        d.Add("step"   , STEP    );
                                        d.Add("stop"    , STOP      );
                                        d.Add("string"    , STRING2      );
                                        d.Add("strip"    , STRIP   );
                                        d.Add("suffix"    , SUFFIX   );
                                        d.Add("suggestions"             , SUGGESTIONS               );
                                        d.Add("swap"  , SWAP      );
                                        d.Add("sys"     , SYS       );
                                        d.Add("SYSTEM" ,SYSTEM);
                                        d.Add("table"     , TABLE       );
                                        d.Add("table1"     , TABLE1       );
                                        d.Add("table2"     , TABLE2       );
                                        d.Add("tableold"     , TABLEOLD       );
                                        d.Add("tabs"     , TABS       );
                                        d.Add("TARGET" ,TARGET);
                                        d.Add("tell"     , TELL       );
										d.Add("temp"     , TEMP       );
                                        d.Add("terminal"     , TERMINAL       );
                                        d.Add("test"               , TEST               );
                                        d.Add("TESTRANDOMMODEL", TESTRANDOMMODEL);
                                        d.Add("TESTRANDOMMODELCHECK", TESTRANDOMMODELCHECK);
                                        d.Add("testsim" , TESTSIM   );
                                        d.Add("time"    , TIME      );
                                        d.Add("timefilter", TIMEFILTER      );
                                        d.Add("timespan", TIMESPAN      );
                                        d.Add("title"              , TITLE      );
                                        d.Add("to"      , TO        );
                                        d.Add("total"      , TOTAL        );
                                        d.Add("TRANSLATE", TRANSLATE);
                                        d.Add("transpose"               , TRANSPOSE       );
                                        d.Add("trel"     , TREL       );
                                        d.Add("truncate", TRUNCATE);
                                        d.Add("tsd"     , TSD       );
                                        d.Add("tsdx"    , TSDX       );
                                        d.Add("tsp"    , TSP       );
                                        d.Add("txt"    , TXT       );
                                        d.Add("type"    , TYPE       );
                                        d.Add("u"  , U   );
                                        d.Add("udvalg"  , UDVALG    );
                                        d.Add("undo"    , UNDO      );
                                        d.Add("unfix"    , UNFIX      );
                                        d.Add("unswap"  , UNSWAP      );
                                        d.Add("updatefreq"              , UPDATEFREQ);
                                        d.Add("updx"    , UPDX       );
                                        d.Add("v"    , V    );
                                        d.Add("val"     , VAL    );
                                        d.Add("value"   , VALUE    );
                                        d.Add("vers"    , VERS    );
                                        d.Add("version"    , VERSION    );
                                        d.Add("vprt"    , VPRT     );
                                        d.Add("wait"    , WAIT     );
                                        d.Add("width"   , WIDTH     );
                                        d.Add("window"   , WINDOW     );
                                        d.Add("working"   , WORKING     );
                                        d.Add("wplot"   , WPLOT     );
                                        d.Add("write"   , WRITE     );
                                        d.Add("wudvalg" , WUDVALG   );
                                        d.Add("X12A", X12A);
                                        d.Add("xls"     , XLS       );
                                        d.Add("xlsx"    , XLSX      );
                                        d.Add("yes"     , YES       );
                                        d.Add("ymax",YMAX);
                                        d.Add("ymin",YMIN);
                                        d.Add("zero"    , ZERO      );
                                        d.Add("ZOOM", ZOOM);
                                        d.Add("ZVAR"    , ZVAR     );
                                        return d;
                                }

                                public override void ReportError(RecognitionException e) {
                                  string hdr = GetErrorHeader(e);
                                  string msg = "Cmd lexer error: " + e.Message;
                                  throw new Exception(e.Line + "¤" + e.CharPositionInLine + "¤" + hdr + "¤" + msg);
                                }


                                    public int CheckKeywordsTable(string s)
                                    {

                                        int rv = Ident;
                                        if(kw.ContainsKey(s)) {
                                          rv = kw[s];
                                        }
                                        return rv;

                                    }
                              }


/*------------------------------------------------------------------
 * PARSER RULES
 *------------------------------------------------------------------*/

expr                      : expressions EOF;  //EOF is necessary in order to force the whole file to be parsed

expressions               : expr2*;

expr2                     :
                            SEMICOLON -> //stray semicolon is ok, nothing is written
                          | analyze        SEMICOLON!
						  | accept         SEMICOLON!
						  | checkoff       SEMICOLON!
						  | clear          SEMICOLON!						
						  | close          SEMICOLON!
						  | clone          SEMICOLON!
						  | cls            SEMICOLON!
                          | copy           SEMICOLON!
						  | collapse       SEMICOLON!
						  | compare        SEMICOLON!
						  | count          SEMICOLON!
                          | create         SEMICOLON!
                          | date           SEMICOLON!
						  | delete         SEMICOLON!
						  | disp           SEMICOLON!						
						  | doc            SEMICOLON!						
						  | edit           SEMICOLON!
						  | endo           SEMICOLON!
						  | exo            SEMICOLON!
						  | exit           SEMICOLON!
						  | findmissingdata SEMICOLON!
                          | for2
						  | functiondef    SEMICOLON!						
                          | genr           SEMICOLON   ->    ^({token("ASTMETA¤"+($genr.text), ASTMETA, 0)} genr)
						  | goto2          SEMICOLON!
						  | hdg            SEMICOLON!
						  | help           SEMICOLON!
						  | if2						
                          | index          SEMICOLON!
						  | ini            SEMICOLON!
						  | itershow       SEMICOLON!
						  | download       SEMICOLON!
						  | library        SEMICOLON!
                          | list           SEMICOLON!
						  | lock_          SEMICOLON!
						  | matrix         SEMICOLON!						
                          | mem            SEMICOLON!
						  | mode           SEMICOLON!
						  | model          SEMICOLON!						
						  | name2          SEMICOLON! 						
						  | ols            SEMICOLON!
                          | open           SEMICOLON!
						  | option         SEMICOLON!
						  | pause          SEMICOLON!
						  | pipe           SEMICOLON!
						  | sheetImport    SEMICOLON!
                          | prt            SEMICOLON!
						  | splice         SEMICOLON!
						  | r_file         SEMICOLON!
						  | r_export       SEMICOLON!
						  | r_run          SEMICOLON!
                          | read           SEMICOLON!
                          | rename         SEMICOLON!						
						  | restart        SEMICOLON!
						  | reset          SEMICOLON!
						  | return2        SEMICOLON!
                          | run            SEMICOLON!
						  | show           SEMICOLON!
						  | sim            SEMICOLON!
						  | sign           SEMICOLON!
						  | smooth         SEMICOLON!
                          | stamp          SEMICOLON!
						  | stop           SEMICOLON!
                          | string2        SEMICOLON!
						  | sys            SEMICOLON!
						  | table          SEMICOLON!
						  | translate      SEMICOLON!
						  | label2         SEMICOLON!
                          | test           SEMICOLON!
						  | tell           SEMICOLON!
                          | time           SEMICOLON!
						  | timefilter     SEMICOLON!						
						  | truncate       SEMICOLON!
						  | tuple          SEMICOLON!   //for instance (VAL y, VAL z) = f(%x);
						  | udvalg         SEMICOLON!
						  | unfix          SEMICOLON!
						  | unlock_        SEMICOLON!
						  | unswap         SEMICOLON!
                          | val            SEMICOLON!
						  | vers           SEMICOLON!
						  | write          SEMICOLON!
						  | x12a           SEMICOLON!
                          ;						

// --------------------------------------------------------------------------------------------------

analyze                   : ANALYZE analyzeOpt1? analyzeElements -> ^({token("ASTANALYZE", ASTANALYZE, $ANALYZE.Line)} analyzeOpt1? analyzeElements );
analyzeOpt1               : ISNOTQUAL 
						  | leftAngle2          analyzeOpt1h* RIGHTANGLE -> ^(ASTOPT1 analyzeOpt1h*)							
						  | leftAngleNo2 dates? analyzeOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) analyzeOpt1h*)
						  ;
analyzeOpt1h              : LAG EQUAL expression -> ^(ASTOPT_VAL_LAG expression)
						  ;						
analyzeElements           : olsElement (COMMA2 olsElement)* -> ^(ASTOLSELEMENTS olsElement+);

accept                    : ACCEPT acceptType name expression -> ^({token("ASTACCEPT", ASTACCEPT, $ACCEPT.Line)} acceptType name expression);
acceptType                : VAL | STRING2 | NAME | DATE | LIST;

checkoff				  : CHECKOFF listItems -> ^({token("ASTCHECKOFF", ASTCHECKOFF, $CHECKOFF.Line)} listItems)
						  | CHECKOFF '?' -> ^({token("ASTCHECKOFF", ASTCHECKOFF, $CHECKOFF.Line)} '?')
						  | CHECKOFF -> ^({token("ASTCHECKOFF", ASTCHECKOFF, $CHECKOFF.Line)});

clear                     : CLEAR clearOpt1? ident? -> ^({token("ASTCLEAR", ASTCLEAR, $CLEAR.Line)} ^(ASTPLACEHOLDER ident?) clearOpt1?);
clearOpt1                 : ISNOTQUAL | leftAngle clearOpt1h* RIGHTANGLE -> ^(ASTOPT1 clearOpt1h*);
clearOpt1h				  : PRIM (EQUAL yesNo)? -> ^(ASTOPT_STRING_PRIM yesNo?)  //obsolete
						  | FIRST (EQUAL yesNo)? -> ^(ASTOPT_STRING_FIRST yesNo?)
						  |	REF (EQUAL yesNo)? -> ^(ASTOPT_STRING_REF yesNo?)
						  ;
						
clone                     : CLONE -> ^({token("ASTCLONE", ASTCLONE, $CLONE.Line)});

close					  : CLOSE closeOpt1? ident -> ^({token("ASTCLOSE", ASTCLOSE, $CLOSE.Line)} ident closeOpt1?)
						  | CLOSE closeOpt1? star -> ^({token("ASTCLOSESTAR", ASTCLOSESTAR, $CLOSE.Line)} closeOpt1?)
						  ;
closeOpt1                 : ISNOTQUAL | leftAngle closeOpt1h* RIGHTANGLE -> ^(ASTOPT1 closeOpt1h*);
closeOpt1h				  : SAVE (EQUAL yesNo)? -> ^(ASTOPT_STRING_SAVE yesNo?)							
						  ;						

cls						  : CLS -> ^({token("ASTCLS", ASTCLS, $CLS.Line)});

						  //this seems the right way to do it, via an ASTOPT_ node, fix for read/open/mulbk...
copy                      : COPY copyOpt1? listItemsWildRange0 (TO listItemsWildRange1)? -> ^({token("ASTCOPY", ASTCOPY, $COPY.Line)} ^(ASTOPT_ copyOpt1?) listItemsWildRange0 listItemsWildRange1?);

doc                       : DOC listItemsWildRange0 docOpt2 -> ^({token("ASTDOC", ASTDOC, $DOC.Line)} listItemsWildRange0 ^(ASTOPT_ docOpt2?));
docOpt2                   : docOpt2h*;
docOpt2h                  : LABEL EQUAL expression -> ^(ASTOPT_STRING_LABEL expression)
						  | SOURCE EQUAL expression -> ^(ASTOPT_STRING_SOURCE expression)
						  | STAMP EQUAL expression -> ^(ASTOPT_STRING_STAMP expression)					
						  ;

collapse				  : COLLAPSE nameWithDot '=' nameWithDot collapseMethod? -> ^({token("ASTCOLLAPSE", ASTCOLLAPSE, $COLLAPSE.Line)} nameWithDot nameWithDot collapseMethod?);
collapseMethod			  : FIRST|LAST|AVG|TOTAL;

compare                   : COMPARE compareOpt1? listItems? (FILE '=' fileName)?-> ^({token("ASTCOMPARECOMMAND", ASTCOMPARECOMMAND, $COMPARE.Line)} listItems? ^(ASTOPT_ compareOpt1?) ^(ASTHANDLEFILENAME fileName?));
compareOpt1               : ISNOTQUAL 
						  | leftAngle2          compareOpt1h* RIGHTANGLE -> ^(ASTOPT1 compareOpt1h*)							
						  | leftAngleNo2 dates? compareOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) compareOpt1h*)
                          ;
compareOpt1h              : ABS (EQUAL yesNo)? -> ^(ASTOPT_STRING_ABS yesNo?);

//see index
count                     : COUNT SERIES? indexerAlone -> ^({token("ASTCOUNT", ASTCOUNT, $COUNT.Line)} ^(ASTINDEXERALONE indexerAlone));

create                    : CREATE nameWithBank '=' expression -> ^({token("ASTCREATEEXPRESSION", ASTCREATEEXPRESSION, $CREATE.Line)} nameWithBank expression)
						  | CREATE listItems -> ^({token("ASTCREATE", ASTCREATE, $CREATE.Line)} listItems)
                          | CREATE question -> ^({token("ASTCREATEQUESTION", ASTCREATEQUESTION, $CREATE.Line)})						
						  ;

date                      : DATE nameWithDot EQUAL expression -> ^({token("ASTDATE", ASTDATE, $DATE.Line)} nameWithDot expression)
						  | DATE question percentNoGlue GLUE ident -> ^({token("ASTDATE", ASTDATE, $DATE.Line)} question ident)
						  | DATE question -> ^({token("ASTDATE", ASTDATE, $DATE.Line)} question)
						  ;

delete					  : DELETE deleteOpt1? listItems? -> ^({token("ASTDELETE", ASTDELETE, $DELETE.Line)} listItems? deleteOpt1?)
        				  //| DELETE deleteOpt1? -> ^({token("ASTDELETE", ASTDELETE, $DELETE.Line)} deleteOpt1?)
						  ;
deleteOpt1                : ISNOTQUAL | leftAngle deleteOpt1h* RIGHTANGLE -> deleteOpt1h*;
deleteOpt1h               : NONMODEL (EQUAL yesNo)? -> ^(ASTOPT_STRING_NONMODEL yesNo?)
						  | SER -> //ignored for now
						  | SERIES ->  //ignored for now 
						  ;

disp					  : DISP StringInQuotes -> ^({token("ASTDISPSEARCH", ASTDISPSEARCH, $DISP.Line)} StringInQuotes)
						  | DISP dispOpt1? listItems -> ^({token("ASTDISP", ASTDISP, $DISP.Line)} ^(ASTOPT_ dispOpt1?) listItems)
						  ; 						
dispOpt1                  : ISNOTQUAL 
						  | leftAngle2          dispOpt1h* RIGHTANGLE -> ^(ASTOPT1 dispOpt1h*)							
						  | leftAngleNo2 dates? dispOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) dispOpt1h*)
                          ;
dispOpt1h                 : INFO (EQUAL yesNo)? -> ^(ASTOPT_STRING_INFO yesNo?);

edit                      : EDIT fileNameStar -> ^({token("ASTEDIT", ASTEDIT, $EDIT.Line)} ^(ASTHANDLEFILENAME fileNameStar));

endo					  : ENDO listItems -> ^({token("ASTENDO", ASTENDO, $ENDO.Line)} listItems)
						  | ENDO -> ^({token("ASTENDO", ASTENDO, $ENDO.Line)})
						  | ENDO question -> ^({token("ASTENDOQUESTION", ASTENDOQUESTION, $ENDO.Line)})
						  ;

exo 					  : EXO listItems -> ^({token("ASTEXO", ASTEXO, $EXO.Line)} listItems)
						  | EXO -> ^({token("ASTEXO", ASTEXO, $EXO.Line)})
						  | EXO question -> ^({token("ASTEXOQUESTION", ASTEXOQUESTION, $EXO.Line)})
						  ;

exit					  : EXIT -> ^({token("ASTEXIT", ASTEXIT, $EXIT.Line)});

findmissingdata			  : FINDMISSINGDATA findmissingdataOpt1? listItems? -> ^({token("ASTFINDMISSINGDATA", ASTFINDMISSINGDATA, $FINDMISSINGDATA.Line)} findmissingdataOpt1? listItems?);
findmissingdataOpt1       : ISNOTQUAL | leftAngle2          findmissingdataOpt1h* RIGHTANGLE -> ^(ASTOPT1 findmissingdataOpt1h*)							
						  | leftAngleNo2 dates? findmissingdataOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) findmissingdataOpt1h*)
                          ;
findmissingdataOpt1h      : REPLACE EQUAL expression -> ^(ASTOPT_VAL_REPLACE expression)
						  ;

for2                      : forValHelper
						  | forNameHelper
						  | forStringHelper
						  | forDateHelper
						  ;

functiondef               : FUNCTION functionDefLhsH1 uDotIdent leftParen functiondefRhsH1 rightParen SEMICOLON expressions? END -> ^({token("ASTFUNCTIONDEF", ASTFUNCTIONDEF, $FUNCTION.Line)} ^(ASTFUNCTIONDEFTYPE functionDefLhsH1) ^(ASTFUNCTIONDEFNAME uDotIdent) functiondefRhsH1 ^(ASTFUNCTIONDEFCODE expressions?));

genr                      : 						    				
							//------------------------- UPD with equal ------------------------------------------------------
                            // UPD: y1, y2 = 5; //at least 2 on lhs, and 1 or more on rhs
						  | genr2 seriesOpt1? listItemsUpd2 EQUAL updDataComplicated -> ^(ASTUPD listItemsUpd2 ^(ASTOPT_ seriesOpt1?) ^(ASTUPDOPERATOR ASTUPDOPERATOREQUAL) updDataComplicated )
						  | genr2 seriesOpt1? listItemsUpd2 EQUAL updDataSimple -> ^(ASTUPD listItemsUpd2 ^(ASTOPT_ seriesOpt1?) ^(ASTUPDOPERATOR ASTUPDOPERATOREQUAL) updDataSimple)

						    // UPD: y1, y2 = 5; //1 or more on lhs, and at least 2 on rhs
					      | genr2 seriesOpt1? listItemsUpd EQUAL updDataComplicated2 -> ^(ASTUPD listItemsUpd ^(ASTOPT_ seriesOpt1?) ^(ASTUPDOPERATOR ASTUPDOPERATOREQUAL) updDataComplicated2)
						    // UPD: y1 = 1 2; //must have > 1 on rhs
						  | genr2 seriesOpt1? listItemsUpd EQUAL updDataSimple2 -> ^(ASTUPD listItemsUpd ^(ASTOPT_ seriesOpt1?) ^(ASTUPDOPERATOR ASTUPDOPERATOREQUAL) updDataSimple2)			  						

						    // UPD: #m = 5; // #m = ... ------> gets special treatment
					      | genr3 seriesOpt1? listItemsUpd EQUAL updDataComplicated -> ^(ASTUPD listItemsUpd ^(ASTOPT_ seriesOpt1?) ^(ASTUPDOPERATOR ASTUPDOPERATOREQUAL) updDataComplicated)
						    // UPD: y1 = 1 2; //must have > 1 on rhs
						  | genr3 seriesOpt1? listItemsUpd EQUAL updDataSimple -> ^(ASTUPD listItemsUpd ^(ASTOPT_ seriesOpt1?) ^(ASTUPDOPERATOR ASTUPDOPERATOREQUAL) updDataSimple)			  						

						    // This is cheating, so we can handle these printcodes here, and not in the GENR section
							// UPD: SERIES <p> y1 = 5; //cheat, catches with mandatory <p> or others
					      | genr2 seriesOpt1Cheat listItemsUpd EQUAL updDataComplicated -> ^(ASTUPD listItemsUpd ^(ASTOPT_ seriesOpt1Cheat) ^(ASTUPDOPERATOR ASTUPDOPERATOREQUAL) updDataComplicated)						
						  | genr2 seriesOpt1Cheat listItemsUpd EQUAL updDataSimple -> ^(ASTUPD listItemsUpd ^(ASTOPT_ seriesOpt1Cheat) ^(ASTUPDOPERATOR ASTUPDOPERATOREQUAL) updDataSimple)			  						

						    //------------------------- UPD with non-equal ------------------------------------------------------
							// UPD: y1, y2 % 2; //operator is not =, handles anyting
						  | genr2 seriesOpt1? listItemsUpd updOperatorDollar updDataComplicated -> ^(ASTUPD listItemsUpd ^(ASTOPT_ seriesOpt1?) ^(ASTUPDOPERATOR updOperatorDollar) updDataComplicated)
						    // UPD: w:x1, w:x2 % 						
						  | genr2 seriesOpt1? listItemsUpd updOperatorDollar updDataSimple -> ^(ASTUPD listItemsUpd ^(ASTOPT_ seriesOpt1?) ^(ASTUPDOPERATOR updOperatorDollar) updDataSimple)						
					
						  						
							//-------------------------------------------------------------------------------
							//-------------------------------------------------------------------------------
						    // For all the following, any "rep *2 is silently ignored
							// GENR: #m[2] = x1 + x2
						  | genr2 (leftAngle dates? RIGHTANGLE)? listName leftBracketGlue expression RIGHTBRACKET EQUAL expression (REP star)* -> ^({token("ASTGENRLISTINDEXER", ASTGENRLISTINDEXER, $EQUAL.Line)}  ^(ASTDATES dates?) listName expression expression)
						    // GENR: #m[2][2010] = x1 + x2
						  | genr2 listName leftBracketGlue expression RIGHTBRACKET leftBracketGlue expression RIGHTBRACKET EQUAL expression  (REP star)* -> ^({token("ASTGENRLISTINDEXER2", ASTGENRLISTINDEXER2, $EQUAL.Line)}  listName expression expression expression)						
						    // GENR: %n = x1 + x2
						  | genr2 (leftAngle dates? RIGHTANGLE)? scalarWithBank EQUAL expression  (REP star)* -> ^({token("ASTGENR", ASTGENR, $EQUAL.Line)}  ^(ASTDATES dates?) scalarWithBank expression)
						    // GENR: pch(w:%n) = x1 + x2
						  | genr2 (leftAngle dates? RIGHTANGLE)? ident leftParenGlue scalarWithBank RIGHTPAREN EQUAL expression (REP star)* -> ^({token("ASTGENRLHSFUNCTION", ASTGENRLHSFUNCTION, $EQUAL.Line)}  ^(ASTDATES dates?) scalarWithBank expression)
						    // GENR: w:%n[2020] = x1 + x2
						  | genr2 scalarWithBank leftBracketGlue expression RIGHTBRACKET EQUAL expression (REP star)* -> ^({token("ASTGENRINDEXER", ASTGENRINDEXER, $EQUAL.Line)}  scalarWithBank expression expression)												
						    // GENR: w:y = x1 + x2
						  | genr2 (leftAngle dates? RIGHTANGLE)? nameWithBank EQUAL expression  (REP star)* -> ^({token("ASTGENR", ASTGENR, $EQUAL.Line)}  ^(ASTDATES dates?) nameWithBank expression)												  						  						
						    // GENR: pch(w:y) = x1 + x2
						  | genr2 (leftAngle dates? RIGHTANGLE)? ident leftParenGlue nameWithBank RIGHTPAREN EQUAL expression  (REP star)* -> ^({token("ASTGENRLHSFUNCTION", ASTGENRLHSFUNCTION, $EQUAL.Line)}  ^(ASTDATES dates?) nameWithBank expression ident)												
						    // GENR: w:y[2020] = x1 + x2
						  | genr2 nameWithBank leftBracketGlue expression RIGHTBRACKET EQUAL expression  (REP star)* -> ^({token("ASTGENRINDEXER", ASTGENRINDEXER, $EQUAL.Line)}  nameWithBank expression expression)						

						  | genr2 question -> ASTSERIESQUESTION                          					

						  ;

genr2                     : SER | SERIES;		

genr3                     : SER2 | SERIES2;

seriesOpt1                : ISNOTQUAL 
						  | leftAngle2          seriesOpt1h* RIGHTANGLE -> ^(ASTOPT1 seriesOpt1h*)
						  | leftAngleNo2 dates? seriesOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) seriesOpt1h*)
                          ;

seriesOpt1Cheat           : ISNOTQUAL 
						  | leftAngle2          seriesOpt1h+ RIGHTANGLE -> ^(ASTOPT1 seriesOpt1h+)							
						  | leftAngleNo2 dates? seriesOpt1h+ RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) seriesOpt1h+)
                          ;

seriesOpt1h               : D (EQUAL yesNo)? -> ^(ASTOPT_STRING_D yesNo?)
						  | P (EQUAL yesNo)? -> ^(ASTOPT_STRING_P yesNo?)
						  | M (EQUAL yesNo)? -> ^(ASTOPT_STRING_M yesNo?)
						  | Q (EQUAL yesNo)? -> ^(ASTOPT_STRING_Q yesNo?)
						  | MP (EQUAL yesNo)? -> ^(ASTOPT_STRING_MP yesNo?)						
						  | N (EQUAL yesNo)? -> ^(ASTOPT_STRING_N yesNo?)						
						  | KEEP EQUAL exportType -> ^(ASTOPT_STRING_KEEP exportType)
						  ;

goto2                     : GOTO ident -> ^({token("ASTGOTO", ASTGOTO, $GOTO.Line)} ident);

hdg						  : HDG expression -> ^({token("ASTHDG", ASTHDG, $HDG.Line)} expression);

help					  : HELP ident? -> ^({token("ASTHELP", ASTHELP, $HELP.Line)} ident?);

if2						  : IF leftParen logicalOr rightParen expressions (ELSE expressions)? END SEMICOLON -> ^({token("ASTIF", ASTIF, $IF.Line)} logicalOr ^(ASTIFSTATEMENTS expressions) ^(ASTELSESTATEMENTS expressions?));

download                  : DOWNLOAD HTTP? url fileName -> ^({token("ASTDOWNLOAD", ASTDOWNLOAD, $DOWNLOAD.Line)} ^(ASTHTTP HTTP?) url ^(ASTHANDLEFILENAME fileName));

index                     : INDEX indexOpt1? SERIES? listItemsWildRange0 nameWithDot? -> ^({token("ASTINDEX", ASTINDEX, $INDEX.Line)} listItemsWildRange0 ^(ASTPLACEHOLDER nameWithDot?) indexOpt1?);
//index                     : INDEX indexOpt1? SERIES? indexerAlone nameWithDot? -> ^({token("ASTINDEX", ASTINDEX, $INDEX.Line)} ^(ASTINDEXERALONE indexerAlone) ^(ASTPLACEHOLDER nameWithDot?) indexOpt1?);
indexOpt1                 : ISNOTQUAL | leftAngle indexOpt1h* RIGHTANGLE -> ^(ASTOPT1 indexOpt1h*);							
indexOpt1h                : MUTE (EQUAL yesNo)? -> ^(ASTOPT_STRING_MUTE yesNo?)	;

ini						  : INI -> ^({token("ASTINI", ASTINI, $INI.Line)});

itershow				  : ITERSHOW  (leftAngle dates? RIGHTANGLE)? listItems -> ^({token("ASTITERSHOW", ASTITERSHOW, $ITERSHOW.Line)} ^(ASTDATES dates?) listItems);

library                   : LIBRARY ident -> ^({token("ASTLIBRARY", ASTLIBRARY, $LIBRARY.Line)} ident);  //must keep the name simple, since the file is grabbed at parse time, in contrast to the RUN command

list					  :	LIST listOpt1? listNameHelper EQUAL listItems prefix? suffix? strip? sort? -> ^({token("ASTLIST¤"+($listItems.text)+"¤",  ASTLIST, $LIST.Line)} listNameHelper listItems listOpt1? prefix? suffix? strip? sort? )
		                  | LIST question hashNoGlue GLUE ident -> ^({token("ASTLIST",  ASTLIST, $LIST.Line)} question ident)
						  | LIST question -> ^({token("ASTLIST",  ASTLIST, $LIST.Line)} question)						
						  ;
listOpt1                  : ISNOTQUAL | leftAngle listOpt1h* RIGHTANGLE -> listOpt1h*
						  ;
listOpt1h                 : DIRECT -> ASTDIRECT
						  ;

lock_                     : LOCK_ ident -> ^({token("ASTLOCK", ASTLOCK, $LOCK_.Line)} ident);
unlock_                   : UNLOCK_ ident -> ^({token("ASTUNLOCK", ASTUNLOCK, $UNLOCK_.Line)} ident);

matrix                    : matrixHelper name leftBracketGlue expression ',' expression RIGHTBRACKET EQUAL expression -> ^({token("ASTMATRIXINDEXER", ASTMATRIXINDEXER, $EQUAL.Line)} name expression expression expression)
                          | matrixHelper name EQUAL expression -> ^({token("ASTMATRIX", ASTMATRIX, $EQUAL.Line)} name expression)
						  | matrixHelper question hashNoGlue GLUE ident -> ^(ASTMATRIX question ident)
						  | matrixHelper question -> ^(ASTMATRIX question)						  						
						  ;
matrixHelper              : MAT | MATRIX;

mem                       : MEM -> ^({token("ASTMEM", ASTMEM, $MEM.Line)});

mode                      : MODE mode2 -> ^({token("ASTMODE", ASTMODE, $MODE.Line)} mode2)
                          | MODE question -> ^({token("ASTMODEQUESTION", ASTMODEQUESTION, $MODE.Line)})	
						  ;	
mode2                     : MIXED | SIM | DATA;

model                     : MODEL modelOpt1? fileNameStar -> ^({token("ASTMODEL", ASTMODEL, $MODEL.Line)} ^(ASTHANDLEFILENAME fileNameStar) modelOpt1?);

ols                       : OLS olsOpt1? olsElements -> ^({token("ASTOLS", ASTOLS, $OLS.Line)} ^(ASTOPT_ olsOpt1?) olsElements);

open                      : OPEN openOpt1? openHelper (COMMA2 openHelper)* -> ^({token("ASTOPEN", ASTOPEN, $OPEN.Line)} openOpt1? openHelper+);
openHelper                : fileNameStar (AS ident)? -> ^(ASTOPENHELPER ^(ASTFILENAME fileNameStar) ^(ASTAS ident?));

option                    : OPTION optionType -> ^({token("ASTOPTION", ASTOPTION, $OPTION.Line)} optionType);

pause					  : PAUSE expression? -> ^({token("ASTPAUSE", ASTPAUSE, $PAUSE.Line)} expression?);

pipe					  : PIPE pipeOpt1? fileName ->^({token("ASTPIPE", ASTPIPE, $PIPE.Line)} pipeOpt1? ^(ASTHANDLEFILENAME fileName));

                          // This is SHEET<import> or SHEET<2010 2015 import>.
						  // The rule stipulates that import must be before other settings, and there must be file=, and there must be an option field.
						  // We also have a SHEET without import, see the prt rule
sheetImport               : SHEET sheetImportOpt1 listItems FILE '=' fileName -> ^(ASTSHEETIMPORT sheetImportOpt1 ^(ASTHANDLEFILENAME fileName?) listItems);
sheetImportOpt1           : ISNOTQUAL 
						  | leftAngle        IMPORT sheetImportOpt1h* RIGHTANGLE -> ASTPLACEHOLDER  sheetImportOpt1h*  //error here if the placeholder is not here
						  | leftAngle dates? IMPORT sheetImportOpt1h* RIGHTANGLE -> ASTPLACEHOLDER ^(ASTDATES dates?) sheetImportOpt1h*
						  ;
sheetImportOpt1h          : CELL '=' expression -> ^(ASTOPT_STRING_CELL expression)						
						  | COLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_COLS yesNo?)						
						  | ROWS (EQUAL yesNo)? -> ^(ASTOPT_STRING_ROWS yesNo?)
						  | SHEET '=' expression -> ^(ASTOPT_STRING_SHEET expression)
						  ;

							//Hmmm, not possible to use {token()} here unless we make 9 identical lines
prt                       : prtHelper prtOpt1? prtElements prtOpt2? -> ^(ASTPRT ^(ASTPRTTYPE prtHelper) prtOpt1? prtOpt2? prtElements);
prtHelper                 : P | PRT | PRI | PRINT | MULPRT | GMULPRT | SHEET | CLIP | PLOT;
//prtUsing                  : USING fileNameStar -> ^(ASTPRTUSING fileNameStar);
prtOpt1                   : ISNOTQUAL 
						  | leftAngle2          prtOpt1Helper* RIGHTANGLE -> ^(ASTOPT1 prtOpt1Helper*)							
						  | leftAngleNo2 dates? prtOpt1Helper* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) prtOpt1Helper*)
                          ;
prtOpt1Helper             : filter						
						  | prtOptionField4Helper						  												
						  | APPEND (EQUAL yesNo)? -> ^(ASTOPT_STRING_APPEND yesNo?)
						  | CELL '=' expression -> ^(ASTOPT_STRING_CELL expression)
						  | COLLAPSE (EQUAL prtOptCollapseHelper)? -> ^(ASTOPT_STRING_COLLAPSE prtOptCollapseHelper?)
						  | COLORS (EQUAL yesNo)? -> ^(ASTOPT_STRING_COLORS yesNo?)
						  | COLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_COLS yesNo?)
						  | DATES (EQUAL yesNo)? -> ^(ASTOPT_STRING_DATES yesNo?)
						  | HEADING '=' expression -> ^(ASTOPT_STRING_HEADING expression)
						  | NAMES (EQUAL yesNo)? -> ^(ASTOPT_STRING_NAMES yesNo?)
						  | PLOTCODE '=' expression -> ^(ASTOPT_STRING_PLOTCODE expression)
						  | ROWS (EQUAL yesNo)? -> ^(ASTOPT_STRING_ROWS yesNo?)
						  | SHEET '=' expression -> ^(ASTOPT_STRING_SHEET expression)
						  | STAMP (EQUAL yesNo)? -> ^(ASTOPT_STRING_STAMP yesNo?)							  
						  | USING EQUAL fileNameStar -> ^(ASTOPT_STRING_USING fileNameStar)							  
						  | YMAX EQUAL expression -> ^(ASTOPT_VAL_YMAX expression)
						  | YMIN EQUAL expression -> ^(ASTOPT_VAL_YMIN expression)
						  ;
prtOpt2                   : prtOpt2Helper+ -> ^(ASTOPT2 prtOpt2Helper);
prtOpt2Helper             : FILE '=' fileName -> ^(ASTHANDLEFILENAME fileName);
prtOptCollapseHelper      : AVG -> ASTAVG
                          | TOTAL -> ASTTOTAL
						  | expression -> expression						
						  ;

splice                    : SPLICE listItems0 EQUAL listItems1 expression listItems2 -> ^({token("ASTSPLICE", ASTSPLICE, $SPLICE.Line)} listItems0 listItems1 listItems2 expression     )
                          | SPLICE listItems0 EQUAL listItems1 listItems2            -> ^({token("ASTSPLICE", ASTSPLICE, $SPLICE.Line)} listItems0 listItems1 listItems2 )  //no date
						  ;
spliceOpt1                : ISNOTQUAL 
						  | leftAngle        spliceOpt1h* RIGHTANGLE -> spliceOpt1h*												  
                          ;
spliceOpt1h               : KEEP EQUAL spliceOptions -> ^(ASTOPT_STRING_KEEP spliceOptions);
spliceOptions             : FIRST | LAST;

						  //!!!Two identical lines ONLY because of token stuff
read                      : READ   readOpt1? fileNameStar (TO identOrStar)? -> ^({token("ASTREAD", ASTREAD, $READ.Line)}   READ   readOpt1? ^(ASTHANDLEFILENAME fileNameStar) ^(ASTREADTO identOrStar?))
                          | IMPORT readOpt1? fileNameStar (TO identOrStar)? -> ^({token("ASTREAD", ASTREAD, $IMPORT.Line)} IMPORT readOpt1? ^(ASTHANDLEFILENAME fileNameStar) ^(ASTREADTO identOrStar?))
						  ;

readOpt1                  : ISNOTQUAL 
						  | leftAngle        readOpt1h* RIGHTANGLE -> readOpt1h*						
						  | leftAngle dates? readOpt1h* RIGHTANGLE -> ^(ASTDATES dates?) readOpt1h*
                          ;
readOpt1h                 : MERGE (EQUAL yesNo)? -> ^(ASTOPT_STRING_MERGE yesNo?)
						  | PRIM (EQUAL yesNo)? -> ^(ASTOPT_STRING_PRIM yesNo?)  //obsolete
						  | FIRST (EQUAL yesNo)? -> ^(ASTOPT_STRING_FIRST yesNo?)
						  | REF (EQUAL yesNo)? -> ^(ASTOPT_STRING_REF yesNo?)												
						  | TSD (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSD yesNo?)
						  | TSDX (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSDX yesNo?)
						  | GBK (EQUAL yesNo)? -> ^(ASTOPT_STRING_GBK yesNo?)
						  | TSP (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSP yesNo?)
						  | PCIM (EQUAL yesNo)? -> ^(ASTOPT_STRING_PCIM yesNo?)
						  | CSV (EQUAL yesNo)? -> ^(ASTOPT_STRING_CSV yesNo?)
						  | PRN (EQUAL yesNo)? -> ^(ASTOPT_STRING_PRN yesNo?)
						  | XLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_XLS yesNo?)
  						  | XLSX (EQUAL yesNo)? -> ^(ASTOPT_STRING_XLSX yesNo?)
						  | COLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_COLS yesNo?)
						  ;
identOrStar               : ident -> ident
						  | star -> ASTBANKISSTARCHEATCODE
						  ;

r_file   				  : R_FILE fileName -> ^({token("ASTR_FILE", ASTR_FILE, $R_FILE.Line)} ^(ASTHANDLEFILENAME fileName?));

r_export  				  : R_EXPORT r_exportOpt1? r_exportItems -> ^({token("ASTR_EXPORT", ASTR_EXPORT, $R_EXPORT.Line)}  r_exportOpt1?  ^(ASTR_EXPORTITEMS r_exportItems ));
r_exportOpt1			  : ISNOTQUAL | leftAngle r_exportOpt1h* RIGHTANGLE -> r_exportOpt1h*;
r_exportOpt1h             : TARGET EQUAL expression -> ^(ASTOPT_STRING_TARGET expression);
r_exportItems             : r_exportItem (COMMA2 r_exportItem)* -> r_exportItem+;
r_exportItem              : hashNoGlue GLUE ident -> ident;

r_run  				      : R_RUN r_runOpt1? -> ^({token("ASTR_RUN", ASTR_RUN, $R_RUN.Line)}  r_runOpt1? );
r_runOpt1			      : ISNOTQUAL | leftAngle r_runOpt1h* RIGHTANGLE -> r_runOpt1h*;
r_runOpt1h                : MUTE (EQUAL yesNo)? -> ^(ASTOPT_STRING_MUTE yesNo?);

rename                    : RENAME listItems0 AS listItems1 -> ^({token("ASTRENAME", ASTRENAME, $RENAME.Line)} listItems0 listItems1);

res						  : RES (leftAngle dates? RIGHTANGLE)? -> ^({token("ASTRES", ASTRES, $RES.Line)} ^(ASTDATES dates?));

return2                   : RETURN expression -> ^({token("ASTRETURN", ASTRETURN, $RETURN.Line)} expression)  //used in functions
						  | RETURN leftParenNoGlue expression (COMMA2 expression)* RIGHTPAREN-> ^({token("ASTRETURNTUPLE", ASTRETURNTUPLE, $RETURN.Line)} expression+)  //used in functions, tuples return like RETURN (a, b);
						  | RETURN -> ^({token("ASTRETURN", ASTRETURN, $RETURN.Line)});

reset                     : RESET -> ^({token("ASTRESET", ASTRESET, $RESET.Line)});

restart                   : RESTART -> ^({token("ASTRESTART", ASTRESTART, $RESTART.Line)});

run                       : RUN fileNameStar -> ^({token("ASTRUN", ASTRUN, $RUN.Line)} fileNameStar);

show					  : SHOW expression gekkoLabel? -> ^({token("ASTSHOW¤"+($expression.text)+"¤"+($gekkoLabel.text), ASTSHOW, 0)} expression);

sign					  : SIGN -> ^({token("ASTSIGN", ASTSIGN, $SIGN.Line)});

sim                       : SIM simOpt1? -> ^({token("ASTSIM", ASTSIM, $SIM.Line)} simOpt1?);
simOpt1                   : ISNOTQUAL 
						  | leftAngle2          simOpt1h* RIGHTANGLE -> ^(ASTOPT1 simOpt1h*)							
						  | leftAngleNo2 dates? simOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) simOpt1h*)
                          ;
simOpt1h                  : FIX (EQUAL yesNo)? -> ^(ASTOPT_STRING_FIX yesNo?)
						  | STATIC (EQUAL yesNo)? -> ^(ASTOPT_STRING_STATIC yesNo?)
						  | AFTER (EQUAL yesNo)? -> ^(ASTOPT_STRING_AFTER yesNo?)
						  | RES (EQUAL yesNo)? -> ^(ASTOPT_STRING_RES yesNo?)
						  ;

smooth                    : SMOOTH listItems0 EQUAL listItems1 smoothOpt2? -> ^({token("ASTSMOOTH", ASTSMOOTH, $SMOOTH.Line)} listItems0 listItems1 smoothOpt2?);

stamp                     : STAMP -> ^({token("ASTSTAMP", ASTSTAMP, $STAMP.Line)});

string2                   : STRING2 nameWithDot EQUAL expression -> ^({token("ASTSTRING", ASTSTRING, $STRING2.Line)} nameWithDot expression)
						  | STRING2 question percentNoGlue GLUE ident -> ^(ASTSTRING question ident)
						  | STRING2 question -> ^(ASTSTRING question)
						  ;

name2                     : NAME nameWithDot EQUAL expression -> ^({token("ASTNAME2", ASTNAME2, $NAME.Line)} nameWithDot expression)
						  | NAME question percentNoGlue GLUE ident -> ^(ASTNAME2 question ident)
						  | NAME question -> ^(ASTNAME2 question)
						  ;

stop					  : STOP -> ^({token("ASTSTOP", ASTSTOP, $STOP.Line)});

sys						  : SYS -> ^({token("ASTSYS", ASTSYS, $SYS.Line)})
						  | SYS expression -> ^({token("ASTSYS", ASTSYS, $SYS.Line)} expression);

						  //TODO: use {token()} for line numbers...
table					  :	TABLE name EQUAL NEW TABLE leftParenGlue ')'  -> ^(ASTNEWTABLE name)
						  | TABLE name GLUEDOT DOT PRINT leftParenGlue expression? ')'  -> ^(ASTTABLEPRINT name expression?)
						  | tableCurrow SETTOPBORDER leftParenGlue expression ',' expression ')'  -> ^(ASTTABLESETTOPBORDER tableCurrow CURROW expression expression)
						  | tableCurrow SETBOTTOMBORDER leftParenGlue expression ',' expression ')'  -> ^(ASTTABLESETBOTTOMBORDER tableCurrow CURROW expression expression)
						  | tableCurrow SETLEFTBORDER leftParenGlue expression ')'  -> ^(ASTTABLESETLEFTBORDER tableCurrow CURROW expression)
						  | tableCurrow SETLEFTBORDER leftParenGlue expression ',' expression ')'  -> ^(ASTTABLESETLEFTBORDER tableCurrow CURROW expression expression)
						  | tableCurrow SETRIGHTBORDER leftParenGlue expression ')'  -> ^(ASTTABLESETRIGHTBORDER tableCurrow CURROW expression)
						  | tableCurrow SETRIGHTBORDER leftParenGlue expression ',' expression ')'  -> ^(ASTTABLESETRIGHTBORDER tableCurrow CURROW expression expression)						
						  | tableCurrow HIDELEFTBORDER leftParenGlue expression ')'  -> ^(ASTTABLEHIDELEFTBORDER tableCurrow CURROW expression)
						  | tableCurrow HIDERIGHTBORDER leftParenGlue expression ')'  -> ^(ASTTABLEHIDERIGHTBORDER tableCurrow CURROW expression)						
						  | tableCurrow SHOWBORDERS leftParenGlue ')'  -> ^(ASTTABLESHOWBORDERS tableCurrow CURROW)						
						  | tableCurrow NEXT leftParenGlue ')'  -> ^(ASTTABLENEXT tableCurrow)
						  | tableCurrow SETTEXT leftParenGlue expression ',' expression ')'  -> ^(ASTTABLESETTEXT tableCurrow CURROW expression expression)
			              | tableCurrow ALIGNLEFT leftParenGlue expression ')'  -> ^(ASTTABLEALIGNLEFT tableCurrow CURROW expression)
						  | tableCurrow ALIGNCENTER leftParenGlue expression ')'  -> ^(ASTTABLEALIGNCENTER tableCurrow CURROW expression)
						  | tableCurrow ALIGNRIGHT leftParenGlue expression ')'  -> ^(ASTTABLEALIGNRIGHT tableCurrow CURROW expression)
			              | tableCurrow MERGECOLS leftParenGlue expression ',' expression ')'  -> ^(ASTTABLEMERGECOLS tableCurrow CURROW expression expression)						
						  | tableCurrow SETDATES leftParenGlue expression ',' expression ',' expression ')'  -> ^(ASTTABLESETDATES tableCurrow CURROW  expression expression expression)
						
						    //col, t1, t2, expression, printcode, scale, format
						  | tableCurrow SETVALUES leftParenGlue expression ',' expression ',' expression ',' expression ',' expression ',' expression ',' expression ')'  -> ^(ASTTABLESETVALUES tableCurrow expression expression expression ^(ASTTABLESETVALUESELEMENT expression) expression expression expression)
						  | TABLE     tableOpt1? fileName -> ^(ASTTABLE     tableOpt1? ^(ASTHANDLEFILENAME fileName)) //!beware line below
						  | MENUTABLE tableOpt1? fileName -> ^(ASTMENUTABLE tableOpt1? ^(ASTHANDLEFILENAME fileName)) //!beware line above
						  ;

tableOpt1                 : ISNOTQUAL 
						  | leftAngle2          tableOpt1h* RIGHTANGLE -> ^(ASTOPT1 tableOpt1h*)							
						  | leftAngleNo2 dates? tableOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) tableOpt1h*)
                          ;

tableOpt1h                : HTML (EQUAL yesNo)? -> ^(ASTOPT_STRING_HTML yesNo?)
						  | WINDOW (EQUAL yesNo)? -> ^(ASTOPT_STRING_WINDOW yesNo?)						
  						  | optOld  //printcodes						
						  ;

tableCurrow: TABLE name GLUEDOT DOT CURROW GLUEDOT DOT  -> name;

label2                    : TARGET ident -> ^(ASTTARGET ident);

tell					  : TELL ('<' NOCR? '>')? expression -> ^({token("ASTTELL", ASTTELL, $TELL.Line)} expression NOCR?);

test                      : TEST ident ->  ^(ASTTEST ident);

time					  : TIME dates -> ^({token("ASTTIME", ASTTIME, $TIME.Line)} ^(ASTDATES dates))
						  | TIME question -> ^({token("ASTTIMEQUESTION", ASTTIMEQUESTION, $TIME.Line)})		
						  | TIME oneDate -> ^({token("ASTTIME", ASTTIME, $TIME.Line)} ^(ASTDATES oneDate oneDate))  //duplicating, TIME 2015 ==> TIME 2015 2015
						  ;
oneDate                   : expression;

timefilter				  : TIMEFILTER timefilterperiods -> ^({token("ASTTIMEFILTER", ASTTIMEFILTER, $TIMEFILTER.Line)} timefilterperiods);
timefilterperiods		  : (timefilterperiod (',' timefilterperiod)*)?  -> ^(ASTTIMEFILTERPERIODS timefilterperiod+);
timefilterperiod          : expression ((doubleDot | TO) expression (BY expression)?)? -> ^(ASTTIMEFILTERPERIOD expression (expression expression?)?);

translate    			  : TRANSLATE translateOpt1? fileName -> ^({token("ASTTRANSLATE", ASTTRANSLATE, $TRANSLATE.Line)} translateOpt1?  ^(ASTHANDLEFILENAME fileName?));
translateOpt1             : ISNOTQUAL | leftAngle        translateOpt1h* RIGHTANGLE -> translateOpt1h*;						
translateOpt1h            : GEKKO18 (EQUAL yesNo)? -> ^(ASTOPT_STRING_GEKKO18 yesNo?)
						  | AREMOS (EQUAL yesNo)? -> ^(ASTOPT_STRING_AREMOS yesNo?)
						  ;						

truncate                  : TRUNCATE truncateOpt1? listItems -> ^({token("ASTTRUNCATE", ASTTRUNCATE, $TRUNCATE.Line)} ^(ASTOPT_ truncateOpt1?) listItems);

udvalg					  : DECOMP (leftAngle dates? RIGHTANGLE)? udvalgElement -> ^({token("ASTDECOMP¤"+($udvalgElement.text), ASTDECOMP, $DECOMP.Line)} ^(ASTDATES dates?) ^(ASTDECOMPITEMS udvalgElement));

//#9837434 merge these
udvalgElement             : expression -> expression;

unfix					  : UNFIX -> ^({token("ASTUNFIX", ASTUNFIX, $UNFIX.Line)});

unswap					  : UNSWAP -> ^({token("ASTUNSWAP", ASTUNSWAP, $UNSWAP.Line)});

//updprt					  : UPDPRT (leftAngle dates? RIGHTANGLE)? listItems updOperator (FILE '=' fileName)? ->  ^({token("ASTUPDPRT", ASTUPDPRT, $UPDPRT.Line)} ^(ASTDATES dates?) listItems updOperator ^(ASTHANDLEFILENAME fileName?));

val                       : VAL nameWithDot EQUAL expression -> ^({token("ASTVAL", ASTVAL, $VAL.Line)} nameWithDot expression)
						  | VAL question percentNoGlue GLUE ident -> ^(ASTVAL question ident)
						  | VAL question -> ^(ASTVAL question)						  			
						  ;

vers					  : VERS -> ^({token("ASTVERS", ASTVERS, $VERS.Line)});

tuple                     : leftParenNoGlue tupleH1 RIGHTPAREN EQUAL function -> ^(ASTTUPLE tupleH1 function);  //can only allow function, for instance (VAL y, VAL z) = f(%x);
tupleH1                   : tupleH2 (COMMA2 tupleH2)* -> ^(ASTTUPLEITEMS tupleH2+);
tupleH2                   : VAL nameWithDot -> ^(ASTTUPLEITEM VAL nameWithDot)
                          | DATE nameWithDot -> ^(ASTTUPLEITEM DATE nameWithDot)
						  | STRING2 nameWithDot -> ^(ASTTUPLEITEM STRING2 nameWithDot)
						  | LIST listNameHelper -> ^(ASTTUPLEITEM LIST listNameHelper)
						  | SERIES (leftAngle dates? RIGHTANGLE)?  nameWithBank -> ^(ASTTUPLEITEM SERIES ^(ASTDATES dates?) nameWithBank)
						  ;

						  //!!!2x2 identical lines ONLY because of token stuff
write					  : WRITE  writeOpt1? listItems? FILE '=' fileName -> ^({token("ASTWRITE", ASTWRITE, $WRITE.Line)}  WRITE  writeOpt1?  ^(ASTHANDLEFILENAME fileName?) listItems?)
						  | EXPORT writeOpt1? listItems? FILE '=' fileName -> ^({token("ASTWRITE", ASTWRITE, $EXPORT.Line)} EXPORT writeOpt1?  ^(ASTHANDLEFILENAME fileName?) listItems?)
						  | WRITE  writeOpt1? fileName -> ^({token("ASTWRITE", ASTWRITE, $WRITE.Line)}  WRITE  writeOpt1?  ^(ASTHANDLEFILENAME fileName))
						  | EXPORT writeOpt1? fileName -> ^({token("ASTWRITE", ASTWRITE, $EXPORT.Line)} EXPORT writeOpt1?  ^(ASTHANDLEFILENAME fileName))
						  ;

writeOpt1                 : ISNOTQUAL 
						  | leftAngle        writeOpt1h* RIGHTANGLE -> writeOpt1h*
						  | leftAngle dates? writeOpt1h* RIGHTANGLE ->  ^(ASTDATES dates?) writeOpt1h*
						  ;
writeOpt1h                : TSD (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSD yesNo?)  //all these will fail, just to provide better error messages for WRITE<csv> etc.
						  | TSDX (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSDX yesNo?)
						  | GBK (EQUAL yesNo)? -> ^(ASTOPT_STRING_GBK yesNo?)
						  | TSP (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSP yesNo?)
						  | CSV (EQUAL yesNo)? -> ^(ASTOPT_STRING_CSV yesNo?)
						  | PRN (EQUAL yesNo)? -> ^(ASTOPT_STRING_PRN yesNo?)
						  | XLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_XLS yesNo?)
  						  | XLSX (EQUAL yesNo)? -> ^(ASTOPT_STRING_XLSX yesNo?)						
						  | CAPS (EQUAL yesNo)? -> ^(ASTOPT_STRING_CAPS yesNo?)		
						  | GNUPLOT (EQUAL yesNo)? -> ^(ASTOPT_STRING_GNUPLOT yesNo?)
						  | SERIES EQUAL exportType -> ^(ASTOPT_STRING_SERIES exportType)												
						  | SERIES -> ^(ASTOPT_STRING_SERIES ASTOPN)												  				
						  ;

exportType                : D -> ASTOPD
						  | P  -> ASTOPP
						  | M  -> ASTOPM
						  | Q  -> ASTOPQ
						  | MP -> ASTOPMP
						  | N -> ASTOPN
						  | expression  //will handle quotes etc.
						  ;


x12a					  : X12A x12aOpt1? listItems -> ^({token("ASTX12A", ASTX12A, $X12A.Line)} x12aOpt1? listItems?);
x12aOpt1                  : ISNOTQUAL 
						  | leftAngle2          x12aOpt1h* RIGHTANGLE -> x12aOpt1h*
						  | leftAngleNo2 dates? x12aOpt1h* RIGHTANGLE ->  ^(ASTDATES dates?) x12aOpt1h*
						  ;
x12aOpt1h                 : PARAM EQUAL expression -> ^(ASTOPT_STRING_PARAM expression)
						  ;


//--------------------------------------------------------------------------------------

logicalOr
  :  (logicalAnd        -> logicalAnd)
     (OR? lbla=logicalAnd -> ^(ASTOR $logicalOr $lbla))* 
  ;

logicalAnd
  :  (logicalNot        -> logicalNot)
     (AND? lbla=logicalNot -> ^(ASTAND $logicalAnd $lbla))* 
  ;

logicalNot				  :  NOT logicalAtom     -> ^(ASTNOT logicalAtom)
						  |  logicalAtom
						  ;

logicalAtom				  :  expression ifOperator expression -> ^(ASTCOMPARE ifOperator expression expression)
						  |  leftParen! logicalOr rightParen!           // omit both '(' and ')'
						  ;

ifOperator		          :  ISEQUAL -> ^(ASTIFOPERATOR ASTIFOPERATOR1)
						  |  ISNOTQUAL -> ^(ASTIFOPERATOR ASTIFOPERATOR2)
						  |  RIGHTANGLE -> ^(ASTIFOPERATOR ASTIFOPERATOR3)
						  |  leftAngle -> ^(ASTIFOPERATOR ASTIFOPERATOR4)
			              |  ISLARGEROREQUAL -> ^(ASTIFOPERATOR ASTIFOPERATOR5)
						  |  ISSMALLEROREQUAL -> ^(ASTIFOPERATOR ASTIFOPERATOR6)
			              ;

truncateOpt1              : ISNOTQUAL | leftAngle truncateOpt1h? RIGHTANGLE -> truncateOpt1h?;
truncateOpt1h             : dates -> ^(ASTDATES dates);

smoothOpt2                : smoothOpt2h;  //can only choose 1
smoothOpt2h               : SPLINE (EQUAL yesNo)? -> ^(ASTOPT_STRING_SPLINE yesNo?)
                          | REPEAT (EQUAL yesNo)? -> ^(ASTOPT_STRING_REPEAT yesNo?)
                          | GEOMETRIC (EQUAL yesNo)? -> ^(ASTOPT_STRING_GEOMETRIC yesNo?)
						  | LINEAR (EQUAL yesNo)? -> ^(ASTOPT_STRING_LINEAR yesNo?)
						  ;

pipeOpt1                  : ISNOTQUAL | leftAngle pipeOpt1h* RIGHTANGLE -> pipeOpt1h*;
pipeOpt1h                 : HTML (EQUAL yesNo)? -> ^(ASTOPT_STRING_HTML yesNo?)
						  | APPEND (EQUAL yesNo)? -> ^(ASTOPT_STRING_APPEND yesNo?)						
						  ;

modelOpt1                 : ISNOTQUAL | leftAngle modelOpt1h* RIGHTANGLE -> modelOpt1h*;
modelOpt1h                : INFO (EQUAL yesNo)? -> ^(ASTOPT_STRING_INFO yesNo?)
						  ;


openOpt1                  : ISNOTQUAL | leftAngle openOpt1h* RIGHTANGLE -> openOpt1h*;
openOpt1h                 : TSD (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSD yesNo?)
						  | TSDX (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSDX yesNo?)
						  | GBK (EQUAL yesNo)? -> ^(ASTOPT_STRING_GBK yesNo?)
						  | PCIM (EQUAL yesNo)? -> ^(ASTOPT_STRING_PCIM yesNo?)
						  | CSV (EQUAL yesNo)? -> ^(ASTOPT_STRING_CSV yesNo?)
						  | PRN (EQUAL yesNo)? -> ^(ASTOPT_STRING_PRN yesNo?)						
						  | XLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_XLS yesNo?)
  						  | XLSX (EQUAL yesNo)? -> ^(ASTOPT_STRING_XLSX yesNo?)
						  | COLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_COLS yesNo?)						
						  | PRIM (EQUAL yesNo)? -> ^(ASTOPT_STRING_PRIM yesNo?)  //obsolete						
						  | FIRST (EQUAL yesNo)? -> ^(ASTOPT_STRING_FIRST yesNo?)						
						  | SEC (EQUAL yesNo)? -> ^(ASTOPT_STRING_SEC yesNo?)						
						  | LAST (EQUAL yesNo)? -> ^(ASTOPT_STRING_LAST yesNo?)						
						  | EDIT (EQUAL yesNo)? -> ^(ASTOPT_STRING_EDIT yesNo?)						
						  | REF (EQUAL yesNo)? -> ^(ASTOPT_STRING_REF yesNo?)						
						  | PROT (EQUAL yesNo)? -> ^(ASTOPT_STRING_PROT yesNo?)	
						  | EDIT (EQUAL yesNo)? -> ^(ASTOPT_STRING_EDIT yesNo?)	
						  | SAVE (EQUAL yesNo)? -> ^(ASTOPT_STRING_SAVE yesNo?)
						  | POS EQUAL expression -> ^(ASTOPT_VAL_POS expression)
						  ;

olsOpt1                   : ISNOTQUAL | leftAngle olsOpt1h? RIGHTANGLE -> olsOpt1h?;
olsOpt1h                  : dates -> ^(ASTDATES dates);

mulbkOpt1                 : ISNOTQUAL | leftAngle mulbkOpt1h* RIGHTANGLE -> mulbkOpt1h*;
mulbkOpt1h                : TSD (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSD yesNo?)
						  | TSDX (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSDX yesNo?)
						  | GBK (EQUAL yesNo)? -> ^(ASTOPT_STRING_GBK yesNo?)
						  | PCIM (EQUAL yesNo)? -> ^(ASTOPT_STRING_PCIM yesNo?)
						  | CSV (EQUAL yesNo)? -> ^(ASTOPT_STRING_CSV yesNo?)
						  | XLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_XLS yesNo?)
  						  | XLSX (EQUAL yesNo)? -> ^(ASTOPT_STRING_XLSX yesNo?)
						  | COLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_COLS yesNo?)	
						  | MERGE (EQUAL yesNo)? -> ^(ASTOPT_STRING_MERGE yesNo?)					
						  ;

functiondefRhsH1          : (functiondefRhsH2 (COMMA2 functiondefRhsH2)*)? -> ^(ASTFUNCTIONDEFARGS functiondefRhsH2*);  //for instance "VAL x, DATE d, (VAL a, VAL b), STRING s"
functiondefRhsH2          : functiondefRhsH3 -> ^(ASTFUNCTIONDEFRHSSIMPLE functiondefRhsH3+)  //for instance "VAL x"
						  | leftParenNoGlue (functiondefRhsH3 (COMMA2 functiondefRhsH3)*) RIGHTPAREN -> ^(ASTFUNCTIONDEFRHSTUPLE functiondefRhsH3+) //for instance "(VAL a, VAL b)"
						  ;
functiondefRhsH3          : type ident -> ^(ASTFUNCTIONDEFARG type ident);  //for instance "VAL x"

functionDefLhsH1          : type  //for instance "VAL"						
						  | leftParenNoGlue (type (COMMA2 type)*) RIGHTPAREN -> ^(ASTFUNCTIONDEFLHSTUPLE type+) //for instance "(VAL, DATE)", but we allow "(VAL)" too.
						  ;

type                      : VAL | DATE | STRING2 | NAME | LIST | SERIES | SER | MATRIX | MAT;

copyOpt1                  : ISNOTQUAL | leftAngle copyOpt1h* RIGHTANGLE -> copyOpt1h*;
copyOpt1h                 : dates -> ^(ASTDATES dates)
						  | RESPECT (EQUAL yesNo)? -> ^(ASTOPT_STRING_RESPECT yesNo?)
						  | ERROR (EQUAL yesNo)? -> ^(ASTOPT_STRING_ERROR yesNo?)
						  | FROM EQUAL name -> ^(ASTOPT_STRING_FROM name)
						  | FROM EQUAL AT GLUE? -> ^(ASTOPT_STRING_FROM ASTAT)
						  | TO EQUAL name -> ^(ASTOPT_STRING_TO name)
						  | TO EQUAL AT GLUE? -> ^(ASTOPT_STRING_TO ASTAT)
						  ;

indexerAlone              : (leftBracketNoGlue|leftBracketNoGlueWild) wildcard RIGHTBRACKET            -> ^(ASTINDEXERELEMENT ^(ASTINDEXERELEMENTBANK) ^(ASTWILDCARD wildcard))             //wildcard
                          | (name COLON)? wildcard                             -> ^(ASTINDEXERELEMENT ^(ASTINDEXERELEMENTBANK name?) ^(ASTWILDCARD wildcard))             //wildcard						
						  | leftBracketNoGlue range RIGHTBRACKET -> ^(ASTINDEXERELEMENT ^(ASTINDEXERELEMENTBANK) range)     //range
						  | (name COLON)? range                  -> ^(ASTINDEXERELEMENT ^(ASTINDEXERELEMENTBANK name?) range)     //range
						  ;

listNameHelper            : LISTFILE fileName -> ^(ASTLISTFILE fileName)
						  | nameWithDot;

forNameHelper			  : FOR (NAME? nameWithDot '=' listItems)+ SEMICOLON                expressions? END SEMICOLON        ->  ^({token("ASTFORNAME", ASTFORNAME, $FOR.Line)} ^(ASTFORLEFTSIDE2 nameWithDot+) ^(ASTFORRIGHTSIDE2 listItems+) ^(ASTFORSTATEMENTS expressions?))
						  | FOR leftParen (NAME? nameWithDot '=' listItems)+ rightParen SEMICOLON? expressions? END SEMICOLON ->  ^({token("ASTFORNAME", ASTFORNAME, $FOR.Line)} ^(ASTFORLEFTSIDE2 nameWithDot+) ^(ASTFORRIGHTSIDE2 listItems+) ^(ASTFORSTATEMENTS expressions?))
                          ;

forStringHelper			  : FOR (STRING2 nameWithDot '=' listItems)+ SEMICOLON                expressions? END SEMICOLON        ->  ^({token("ASTFORSTRING", ASTFORSTRING, $FOR.Line)} ^(ASTFORLEFTSIDE2 nameWithDot+) ^(ASTFORRIGHTSIDE2 listItems+) ^(ASTFORSTATEMENTS expressions?))
						  | FOR leftParen (STRING2 nameWithDot '=' listItems)+ rightParen SEMICOLON? expressions? END SEMICOLON ->  ^({token("ASTFORSTRING", ASTFORSTRING, $FOR.Line)} ^(ASTFORLEFTSIDE2 nameWithDot+) ^(ASTFORRIGHTSIDE2 listItems+) ^(ASTFORSTATEMENTS expressions?))
                          ;

forValHelper              : FOR VAL nameWithDot '=' expression to expression (BY expression)* SEMICOLON                expressions? END SEMICOLON        ->  ^({token("ASTFORVAL", ASTFORVAL, $FOR.Line)} ^(ASTFORLEFTSIDE nameWithDot) ^(ASTFORRIGHTSIDE expression expression expression*) ^(ASTFORSTATEMENTS expressions?))
                          | FOR leftParen VAL nameWithDot '=' expression TO expression (BY expression)* rightParen SEMICOLON? expressions? END SEMICOLON ->  ^({token("ASTFORVAL", ASTFORVAL, $FOR.Line)} ^(ASTFORLEFTSIDE nameWithDot) ^(ASTFORRIGHTSIDE expression expression expression*) ^(ASTFORSTATEMENTS expressions?))
						  ;

forDateHelper             : FOR DATE nameWithDot '=' expression to expression (BY expression)* SEMICOLON                expressions? END SEMICOLON        ->  ^({token("ASTFORDATE", ASTFORDATE, $FOR.Line)} ^(ASTFORLEFTSIDE nameWithDot) ^(ASTFORRIGHTSIDE expression expression expression*) ^(ASTFORSTATEMENTS expressions?))
                          | FOR leftParen DATE nameWithDot '=' expression TO expression (BY expression)* rightParen SEMICOLON? expressions? END SEMICOLON ->  ^({token("ASTFORDATE", ASTFORDATE, $FOR.Line)} ^(ASTFORLEFTSIDE nameWithDot) ^(ASTFORRIGHTSIDE expression expression expression*) ^(ASTFORSTATEMENTS expressions?))
						  ;

to                        : TO | doubleDot;

prefix					  : PREFIX '=' listItem -> ^(ASTLISTPREFIX listItem);
suffix					  : SUFFIX '=' listItem -> ^(ASTLISTSUFFIX listItem);
strip					  : STRIP '=' listItem -> ^(ASTLISTSTRIP listItem);

sort					  : SORT '=' expression -> ^(ASTLISTSORT expression)
						  | SORT -> ^(ASTLISTSORT ^(ASTSTRINGINQUOTES 'yes'))
						  ;

listItems                 : listItem (COMMA2 listItem)*                   -> ^(ASTLISTITEMS (^(ASTLISTITEM listItem))+);   //puts in o.listItems
listItems0                : listItem (COMMA2 listItem)*                   -> ^(ASTLISTITEMS0 (^(ASTLISTITEM listItem))+);  //puts in o.listItems0
listItems1                : listItem (COMMA2 listItem)*                   -> ^(ASTLISTITEMS1 (^(ASTLISTITEM listItem))+);  //puts in o.listItems1
listItems2                : listItem (COMMA2 listItem)*                   -> ^(ASTLISTITEMS2 (^(ASTLISTITEM listItem))+);  //puts in o.listItems2
listItemsWildRange        : listItemWildRange (COMMA2 listItemWildRange)* -> ^(ASTLISTITEMS (^(ASTLISTITEM listItemWildRange))+);   //puts in o.listItems
listItemsWildRange0       : listItemWildRange (COMMA2 listItemWildRange)* -> ^(ASTLISTITEMS0 (^(ASTLISTITEM listItemWildRange))+);  //puts in o.listItems0
listItemsWildRange1       : listItemWildRange (COMMA2 listItemWildRange)* -> ^(ASTLISTITEMS1 (^(ASTLISTITEM listItemWildRange))+);  //puts in o.listItems1
listItemsWildRange2       : listItemWildRange (COMMA2 listItemWildRange)* -> ^(ASTLISTITEMS2 (^(ASTLISTITEM listItemWildRange))+);  //puts in o.listItems2

listItemsUpd              : listItemUpd (COMMA2 listItemUpd)*             -> ^(ASTLISTITEMS (^(ASTLISTITEM listItemUpd))+);  //one or more
listItemsUpd2             : listItemUpd (COMMA2 listItemUpd)+             -> ^(ASTLISTITEMS (^(ASTLISTITEM listItemUpd))+);  //more than one


                          //expression catches a, b:a, %s, 'a', 'b:a', the first two as nameWithBank, but cannot catch for instance 'b':'a' or %b:%a
						  //not much sense in allowing @ here
listItem                  : expression ->							   expression						
						  | identDigit  ->                             ^(ASTGENERIC1 identDigit)            //accepts stuff like 0e. Integers are caught via expression.												
						  ;
						  //generalizes listItem
listItemWildRange         : wildcardWithBank ->                        wildcardWithBank
						  | rangeWithBank ->                           rangeWithBank	
						  | (LEFTBRACKET|LEFTBRACKETWILD) wildcardWithBank RIGHTBRACKET -> wildcardWithBank
						  | (LEFTBRACKET|LEFTBRACKETWILD) rangeWithBank RIGHTBRACKET -> rangeWithBank	
						  | expression ->						       expression
						  | identDigit  ->                             ^(ASTGENERIC1 identDigit)   //accepts stuff like 0e. Integers are caught via expression.												
						  ;
listItemUpd               : value ->                                   value
						  ;

//#9837434 merge these
olsElements               : olsElement EQUAL olsElement (COMMA2 olsElement)* -> ^(ASTOLSELEMENTS olsElement olsElement olsElement*);
olsElement                : expression
                            gekkoLabel?
							-> ^({token("ASTOLSELEMENT¤"+($expression.text)+"¤"+($gekkoLabel.text), ASTOLSELEMENT, 0)} ^(ASTEXPRESSION expression) gekkoLabel?)
						  ;

//#9837434 merge these
prtElements               : prtElement (COMMA2 prtElement)* -> ^(ASTPRTELEMENTS prtElement+);


prtElement                : expression
                            gekkoLabel?
							prtElementOptionField?
							-> ^({token("ASTPRTELEMENT¤"+($expression.text)+"¤"+($gekkoLabel.text), ASTPRTELEMENT, 0)} ^(ASTEXPRESSION expression) gekkoLabel? prtElementOptionField?)
						  ;

prtElementOptionField     : leftAngle prtOptionField4Helper* RIGHTANGLE -> ^(ASTPRTELEMENTOPTIONFIELD prtOptionField4Helper*);

filter                    : FILTER '=' (  no   -> ^(ASTPRTTIMEFILTER NO)
										| yes  -> ^(ASTPRTTIMEFILTER YES)
										| HIDE -> ^(ASTPRTTIMEFILTER HIDE)
										| AVG  -> ^(ASTPRTTIMEFILTER AVG)    )
						  | FILTER -> ^(ASTPRTTIMEFILTER YES)
						  | NOFILTER -> ^(ASTPRTTIMEFILTER NO)
						  ;

prtOptionField4Helper     : width
						  | dec
						  | nwidth
						  | pwidth
						  | ndec
						  | pdec
						  | opt2 -> ^(ASTPRTOPTION opt2);

opt2                      : optNew | optOld;
							
optOld                    : N    ('=' yesNo -> ^(ASTN yesNo) | -> ^(ASTN ASTYES))
						  | D    ('=' yesNo -> ^(ASTD yesNo) | -> ^(ASTD  ASTYES))
						  | P    ('=' yesNo -> ^(ASTP yesNo) | -> ^(ASTP  ASTYES))
						  | DP    ('=' yesNo -> ^(ASTDP yesNo) | -> ^(ASTDP  ASTYES))
						  | R    ('=' yesNo -> ^(ASTS yesNo) | -> ^(ASTS  ASTYES))
						  | RN    ('=' yesNo -> ^(ASTSN yesNo) | -> ^(ASTSN  ASTYES))
						  | RD    ('=' yesNo -> ^(ASTSD yesNo) | -> ^(ASTSD  ASTYES))
						  | RP    ('=' yesNo -> ^(ASTSP yesNo) | -> ^(ASTSP  ASTYES))
						  | RDP    ('=' yesNo -> ^(ASTSDP yesNo) | -> ^(ASTSDP  ASTYES))
						  | M    ('=' yesNo -> ^(ASTM yesNo) | -> ^(ASTM  ASTYES))
						  | Q    ('=' yesNo -> ^(ASTQ yesNo) | -> ^(ASTQ  ASTYES))
						  | MP    ('=' yesNo -> ^(ASTMP yesNo) | -> ^(ASTMP  ASTYES))
						  ;

optNew                    : lev
						  | abs
						  | dif
						  | pch
						  | gdif
						  | v
                          ;

abs						  : ABS ('=' yesNoAppend -> ^(ASTABS yesNoAppend) |  -> ^(ASTABS ASTYES))
                          | NOABS -> ^(ASTABS ASTNO)
                          | UABS -> ^(ASTABS ASTAPPEND)
						  ;

lev						  : LEV ('=' yesNoAppend -> ^(ASTLEV yesNoAppend) |  -> ^(ASTLEV ASTYES))
                          | NOLEV -> ^(ASTLEV ASTNO)
                          | ULEV -> ^(ASTLEV ASTAPPEND)
						  ;

dif						  : (DIF|DIFF) ('=' yesNoAppend -> ^(ASTDIF yesNoAppend) | -> ^(ASTDIF ASTYES))
                          | (NODIF|NODIFF) -> ^(ASTDIF ASTNO)
                          | (UDIF|UDIFF) -> ^(ASTDIF ASTAPPEND)
                          ;

pch						  : PCH ('=' yesNoAppend -> ^(ASTPCH yesNoAppend) |  -> ^(ASTPCH ASTYES) )
                          | NOPCH -> ^(ASTPCH ASTNO)
                          | UPCH -> ^(ASTPCH ASTAPPEND)
						  ;

gdif					  : (GDIF|GDIFF) ('=' yesNoAppend -> ^(ASTGDIF yesNoAppend) | -> ^(ASTGDIF ASTYES) )
                          | (NOGDIF|NOGDIFF) -> ^(ASTGDIF ASTNO)
                          | (UGDIF|UGDIFF) -> ^(ASTGDIF ASTAPPEND)
						  ;

v    					  : V ('=' yesNo -> ^(ASTV yesNo) | -> ^(ASTV ASTYES))
                          | NOV -> ^(ASTV ASTNO)
						  ;

nwidth					  : NWIDTH '=' expression -> ^(ASTPRTELEMENTNWIDTH expression);
pwidth					  : PWIDTH '=' expression -> ^(ASTPRTELEMENTPWIDTH expression);
ndec					  : NDEC '=' expression -> ^(ASTPRTELEMENTNDEC expression);
pdec					  : PDEC '=' expression -> ^(ASTPRTELEMENTPDEC expression);
width					  : WIDTH '=' expression -> ^(ASTPRTELEMENTWIDTH expression);
dec						  : DEC '=' expression -> ^(ASTPRTELEMENTDEC expression);
yesNoAppend				  : yesNo
						  | append
						  ;
yesNo					  : yes
						  | no
						  | expression;
yesNoSimple					  : yes
						  | no
;
yes                       : YES -> ASTYES;
no                        : NO -> ASTNO;
append					  : APPEND -> ASTAPPEND;

gekkoLabel                : StringInQuotes -> ^(ASTGEKKOLABEL StringInQuotes);

//??? what to do with left-side function. Do we allow general expressions, like 1+log(y) = log(x) and y = exp(??-1).
//    possibly with the simple log(y)=... which are auto-resolved, maybe with y=exp(??) as an known resolve. Some
//    auto-math-solving would be cool...

updOperator               : // EQUAL -> ASTUPDOPERATOREQUAL
						    star -> ASTUPDOPERATORSTAR
						  | HAT -> ASTUPDOPERATORHAT
						  | PLUS -> ASTUPDOPERATORPLUS
						  | hash -> ASTUPDOPERATORHASH
						  | percentSimple -> ASTUPDOPERATORPERCENT
						  ;

updOperatorDollar         : EQUAL   DOLLAR -> ASTUPDOPERATOREQUALDOLLAR
						  | star    DOLLAR -> ASTUPDOPERATORSTARDOLLAR
						  | HAT     DOLLAR -> ASTUPDOPERATORHATDOLLAR
						  | PLUS    DOLLAR -> ASTUPDOPERATORPLUSDOLLAR
						  | hash    DOLLAR -> ASTUPDOPERATORHASHDOLLAR
						  | percentSimple DOLLAR -> ASTUPDOPERATORPERCENTDOLLAR
						  | updOperator
						  ;

updDataSimple             : updDataSimpleHelper+;  //one or more!
updDataSimple2            : updDataSimpleHelper updDataSimpleHelper+;  //at least two!
updDataSimpleHelper       : number -> ^(ASTUPDDATA number);
updDataComplicated        : updDataComplicatedHelper (COMMA2 updDataComplicatedHelper)* -> updDataComplicatedHelper+;  //one or more
updDataComplicated2       : updDataComplicatedHelper (COMMA2 updDataComplicatedHelper)+ -> updDataComplicatedHelper+;  //at least two!						
updDataComplicatedHelper  : updDataComplicatedHelper2 updDataComplicatedHelper3 -> ^(ASTUPDDATA updDataComplicatedHelper2 updDataComplicatedHelper3);  //the value and repetitions
updDataComplicatedHelper2 : number | expression;  //why not just expression?
updDataComplicatedHelper3 : REP expression -> expression
						  | REP star -> ASTSTAR
						  | -> ASTEMPTY
						  ;

dates                     : expression expression
					      //| expression -> expression expression
						  ;

//-----------------------------------------------------------------------------------------
//Name with {} and %, possibly nested
//% only allowed inside {}
//This kind of name can NOT be %a for instance, has to be "a{...}b{...}c" or "{...}a{...}b"
//-----------------------------------------------------------------------------------------

nameWithDot               : name GLUEDOT DOT name -> ^(ASTNAMEWITHDOT name name)                          //aa.bb
						  | name GLUEDOT DOT Integer -> ^(ASTNAMEWITHDOT name ^(ASTINTEGER Integer))      //aa2.1 (for lags)
						  | name -> ^(ASTNAMEWITHDOT name)                                                //aa (name with no dot)
						  ;

//a{%(d{%e})}d{%f+'e'}g, a%b, %b|a, but NOT %s
name                      : //vertical bar will only have glue before if there is no blank before OR after the bar
							//if vertical bar is there, no glue after it can be issued
							//if vertical bar is not there, the glue before namepart stems from glue before '{' or '%'.													   	
						    namePartStartAdvanced nameHelper2 nameHelper2* -> ^(ASTNAME namePartStartAdvanced nameHelper2+)  //this may begin with %s, for instance %s|a or %s%u
						  | namePartStartSimple -> ^(ASTNAME namePartStartSimple)  //this cannot be %s
						  ;

nameHelper2:(GLUE VERTICALBAR? namePart) -> namePart;

//a, 0g, {%e}, {%(d{%e})}
namePart                  : identDigit   //can be ab, 01, 5g0
						  | nameCurly        //	
						  | scalarName       //%a or %(...)
						  | listName         //#a or #(...)
						  ;

//a, {%e}, {%(d{%e})}, %s, #l
namePartStartAdvanced     : ident -> ^(ASTIDENT ident) //a1
						  | nameCurly        //{...}
						  | scalarName       //%a or %(...)
						  | listName         //#a or #(...)
						  ;

//a, {%e}, {%(d{%e})}
namePartStartSimple       : ident -> ^(ASTIDENT ident) //a1
						  | nameCurly
						  ;

//{%e}, {%(d{%e})}
nameCurly                 : leftCurlyNoGlue ident RIGHTCURLY -> ^(ASTCURLYSIMPLE ident)  //{a} really means {%a}. Without this rule you would never use 'a{b}c', that would be 'abc' anyway.
					      | leftCurlyNoGlue expression RIGHTCURLY -> ^(ASTCURLY expression)
						  ;

nameOrScalar              : nameWithDot
                          | scalarName
						  ; //a%b, %s, %(...)

//------------------------------------------------------------------------------------

date2                     : Integer | DateDef;

//------------------------------------------------------------------------------------

listName                  : simpleHashName -> ^(ASTHASH simpleHashName)
				          | listComplicated -> ^(ASTHASH listComplicated)
				          ;

simpleHashName            : hashNoGlue GLUE ident -> ^(ASTHASHNAMESIMPLE ident) //we can have #y, without parentheses
                          | dollarHashNoGlue GLUE ident -> ^(ASTDOLLARHASHNAMESIMPLE ident); //we can have #y, without parentheses
listComplicated           : hashNoGlue leftParenGlue nameOrScalar RIGHTPAREN -> ^(ASTHASHPAREN nameOrScalar)
                          | dollarHashNoGlue leftParenGlue nameOrScalar RIGHTPAREN -> ^(ASTDOLLARHASHPAREN nameOrScalar);

//-----------------------------------------------------------------------------------------
//Scalar name
//  %abc or %(a{%b}c) or %({%b}), you cannot use %%b.
//-----------------------------------------------------------------------------------------

scalarName                : simplePercentName -> ^(ASTSCALAR simplePercentName)
				          | scalarComplicated -> ^(ASTSCALAR scalarComplicated)
				          ;

simplePercentName         : percentNoGlue GLUE ident -> ^(ASTPERCENTNAMESIMPLE ident)  //same as scalarSimple
                          | dollarPercentNoGlue GLUE ident -> ^(ASTDOLLARPERCENTNAMESIMPLE ident); //same as scalarSimple
scalarComplicated         : percentNoGlue leftParenGlue nameOrScalar RIGHTPAREN -> ^(ASTPERCENTPAREN nameOrScalar)
                          | dollarPercentNoGlue leftParenGlue nameOrScalar RIGHTPAREN -> ^(ASTDOLLARPERCENTPAREN nameOrScalar);

//-----------------------------------------------------------------------------------------

nameOrString              : identDigit | expression;
doubleDot                 : GLUEDOT? DOT GLUEDOT DOT;
//doubleDot                 : GLUEDOT? DOUBLEDOT;

//-----------------------------------------------------------------------------------------
//The followin are math expressions
//If there are no parentheses, an expression like a*b^c+b*c+a^d*e*f^g is first chopped up after '+', next '*' and last '^'.
//--> a*b^c         +         b*c        +        a^d*e*f^g
//--> a  *  b^c     +         b * c      +        a^d * e * f^g
//This makes sense, and after parenthesis everything is possible again (expression)
//So therefore indexers arguments get expression
//-----------------------------------------------------------------------------------------
expression                : listExpression;

listExpression            : additiveExpression ( (LISTPLUS|LISTMINUS|LISTSTAR)^ additiveExpression )*;

additiveExpression        : multiplicativeExpression ( (PLUS|MINUS)^ multiplicativeExpression )*;

multiplicativeExpression  : powerExpression ( starHelper^ powerExpression )*;

powerExpression           : unaryExpression ( pow^ unaryExpression )*;

unaryExpression           : indexerExpression
                          | MINUS indexerExpression -> ^(NEGATE indexerExpression)
						  ;

indexerExpression         : primaryExpression ( leftBracketGlue^ (indexerExpressionHelper (','! indexerExpressionHelper)*)? RIGHTBRACKET!)*
						  ;

primaryExpression         : leftParen! expression RIGHTPAREN!
                          | value
						  ;

value                     :
					        function  //must be before nameWithBank, else it will try to find f as timeseries in f(x) and be confused                          						
						  | nameOrListOrScalarWithBank
						  //| nameWithBank //a timeseries name: a, b10, a{b}c, a%b|c, a{%b}c, and also {b}, %b, {%b}. So this covers scalars such as %s too which are emitted differently than the other nameWithBank possibilities						
						  | listName  //a list name: #a, #(a{%b}c), #({%b}) --> is handled with nameWithBank
						  | scalarName  //a scalar name: %a, %(a{%b}c), %({%b}) --> is handled with nameWithBank
						  | Integer -> ^(ASTINTEGER Integer)
						  | double2 -> double2						
						  | date2 -> ^(ASTDATE2 date2) //a date like: 2001q3 (luckily we do not have 'e' freq, then what about 2012e3 (in principle, = 2012000))
						  | StringInQuotes -> ^(ASTSTRINGINQUOTES StringInQuotes)
						  | listFile						
						  | matrixCol
						  | (leftBracketNoGlue|leftBracketNoGlueWild) indexerExpressionHelper RIGHTBRACKET -> ^(ASTINDEXERALONE indexerExpressionHelper) //also see rule indexerExpression
						  ;

matrixCol                 : leftBracketNoGlue matrixRow (doubleVerticalBar matrixRow)* RIGHTBRACKET -> ^(ASTMATRIXCOL matrixRow+);
matrixRow                 :  expression (',' expression)*  -> ^(ASTMATRIXROW expression+);

doubleVerticalBar         : GLUE? (DOUBLEVERTICALBAR1 | DOUBLEVERTICALBAR2);

//using rangeWithBank and wildcardWithBank in the last of value rule gives problems with PRT [pxa..pxb] etc.
indexerExpressionHelper   : range -> ^(ASTINDEXERELEMENT ^(ASTINDEXERELEMENTBANK) range)                             //fm1..fm5
                          | wildcard -> ^(ASTINDEXERELEMENT ^(ASTINDEXERELEMENTBANK) ^(ASTWILDCARD wildcard))                          //fm*
                          | expressionOrNothing doubleDot expressionOrNothing -> ^(ASTINDEXERELEMENT ^(ASTINDEXERELEMENTBANK) expressionOrNothing expressionOrNothing)     //'fm1'..'fm5'
						  | expression -> ^(ASTINDEXERELEMENT ^(ASTINDEXERELEMENTBANK) expression)                                     //'fm*' or -2 or 2000 or 2010q3
						  | PLUS expression -> ^(ASTINDEXERELEMENTPLUS ^(ASTINDEXERELEMENTBANK) expression)                            //+1
						  ;

expressionOrNothing       : expression -> expression
						  | -> ASTEMPTYRANGEELEMENT
						  ;

listFile                  : hash leftParenGlue LISTFILE fileName RIGHTPAREN -> ^(ASTHASH ^(ASTLISTFILE fileName));

						  //only allows *, a*, *a, a*b
copyWildcard              : star                                 -> ^(ASTCOPYWILDCARD1)
						  | starGlueRight identDigit             -> ^(ASTCOPYWILDCARD2 identDigit)
						  | identDigit starGlueLeft              -> ^(ASTCOPYWILDCARD3 identDigit)
						  | identDigit starGlueBoth identDigit   -> ^(ASTCOPYWILDCARD4 identDigit identDigit)
  					      ;

range                     : name doubleDot name -> name name;

wildcard                  : identDigit wildSymbolEnd  //a?						  	
						  | identDigit (wildSymbolMiddle identDigit)+ wildSymbolEnd?  //a?b a?b?, a?b?c a?b?c?, etc.
						  | wildSymbolStart identDigit (wildSymbolMiddle identDigit)* wildSymbolEnd?  //?a ?a? ?a?b ?a?b?, etc.
						  | wildSymbolFree //?
						  ;

wildSymbolFree            : star -> ASTWILDSTAR
						  | question -> ASTWILDQUESTION
						  ;

wildSymbolStart           : starGlueRight -> ASTWILDSTAR
						  | questionGlueRight -> ASTWILDQUESTION
						  ;

wildSymbolEnd             : starGlueLeft -> ASTWILDSTAR
                          | questionGlueLeft -> ASTWILDQUESTION
						  ;

wildSymbolMiddle          : starGlueBoth -> ASTWILDSTAR
                          | questionGlueBoth -> ASTWILDQUESTION
						  ;

function                  : uDotIdent leftParenGlue (functionH1 (',' functionH1)*)? RIGHTPAREN -> ^(ASTFUNCTION uDotIdent functionH1*);

uDotIdent                 : ident -> ident 
						  | U GLUEDOT DOT ident -> ident
						  ;

functionH1                : expression
						  | leftParenNoGlue expression (COMMA2 expression)* RIGHTPAREN -> ^(ASTEXPRESSIONTUPLE expression+)  //a tuple argument, like f(%a1, (%x1, %x2), %a2);
						  ;

// ============== name list scalar   start ===========================

bankColon                 : COLON GLUE VERTICALBAR
						  | COLON;

nameOrListOrScalarWithBank: name bankColon nameWithDot -> ^(ASTNAMEWITHBANK ^(ASTBANK name) nameWithDot)
						  | name bankColon listName -> ^(ASTLISTWITHBANK ^(ASTBANK name) listName)						  						
						  | name bankColon scalarName -> ^(ASTNAMEWITHBANK ^(ASTBANK name) scalarName)
						
						  | AT GLUE nameWithDot ->  ^(ASTNAMEWITHBANK ^(ASTBANK ASTAT) nameWithDot)
						  | AT GLUE listName ->  ^(ASTLISTWITHBANK ^(ASTBANK ASTAT) listName)	
						  | AT GLUE scalarName -> ^(ASTNAMEWITHBANK ^(ASTBANK ASTAT) scalarName)						
						  					  						
						  | nameWithDot -> ^(ASTNAMEWITHBANK ^(ASTBANK) nameWithDot)
						  ;						

nameWithBank              : name bankColon nameWithDot -> ^(ASTNAMEWITHBANK ^(ASTBANK name) nameWithDot)
						  | AT GLUE nameWithDot ->  ^(ASTNAMEWITHBANK ^(ASTBANK ASTAT) nameWithDot)
						  | nameWithDot -> ^(ASTNAMEWITHBANK ^(ASTBANK) nameWithDot)
						  ;

listWithBank              : name bankColon listName -> ^(ASTLISTWITHBANK ^(ASTBANK name) listName)
						  | AT GLUE listName ->  ^(ASTLISTWITHBANK ^(ASTBANK ASTAT) listName)						
						  ;

scalarWithBank            : name bankColon scalarName -> ^(ASTNAMEWITHBANK ^(ASTBANK name) scalarName)
						  | AT GLUE scalarName ->  ^(ASTNAMEWITHBANK ^(ASTBANK ASTAT) scalarName)
						  | scalarName -> ^(ASTNAMEWITHBANK ^(ASTBANK) scalarName)
						  ;

// ============== name list scalar   end ===========================


wildcardWithBank          : name COLON wildcard -> ^(ASTWILDCARDWITHBANK ^(ASTBANK name) ^(ASTWILDCARD wildcard))
						  | AT GLUE wildcard ->  ^(ASTWILDCARDWITHBANK ^(ASTBANK ASTAT) ^(ASTWILDCARD wildcard))
						  | wildcard -> ^(ASTWILDCARDWITHBANK ^(ASTBANK) ^(ASTWILDCARD wildcard))
						  ;						

rangeWithBank             : name COLON range -> ^(ASTRANGEWITHBANK ^(ASTBANK name) range)
						  | AT GLUE range -> ^(ASTRANGEWITHBANK ^(ASTBANK ASTAT) range)
						  | range -> ^(ASTRANGEWITHBANK ^(ASTBANK) range)
						  ;

numberIntegerOrDouble     : integer
						  | double2;

number                    : numberIntegerOrDouble
						  | integerNegative
						  | doubleNegative
						  | M -> ^(ASTMISSING M)
						  ;

fileNameStar              : fileName
						  | star -> ASTFILENAMESTAR
						  ;

//-----------------------------------------------------------------------------------------------------------
//---------------------------- Options ----------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------
						
//!! do not use '_' as a char in a an option name. 'failsafe' is fine, but fail_safe is not.
optionType :
               question -> question
             | CALC question -> CALC question
             | CALC IGNOREMISSINGVARS  '='? yesNoSimple -> CALC IGNOREMISSINGVARS ^(ASTBOOL yesNoSimple)  //addresses both UPD and GENR
			
			 | DATABANK question -> DATABANK question
             | DATABANK COMPARE TABS '='? numberIntegerOrDouble -> DATABANK COMPARE TABS numberIntegerOrDouble
             | DATABANK COMPARE TREL '='? numberIntegerOrDouble  -> DATABANK COMPARE TREL numberIntegerOrDouble
             | DATABANK FILE COPYLOCAL '='? yesNoSimple -> DATABANK FILE COPYLOCAL ^(ASTBOOL yesNoSimple )
             | DATABANK FILE FORMAT '='? optionDatabankFileFormatOptions ->  DATABANK FILE FORMAT ^(ASTSTRINGSIMPLE optionDatabankFileFormatOptions)
             | DATABANK FILE GBK COMPRESS '='? yesNoSimple -> DATABANK FILE GBK COMPRESS ^(ASTBOOL yesNoSimple)
             | DATABANK FILE GBK VERSION '='? numberIntegerOrDouble ->  DATABANK FILE GBK VERSION ^(ASTSTRINGSIMPLE numberIntegerOrDouble)  //NOTE: number converted to string
			 | DATABANK CREATE AUTO '='? yesNoSimple -> DATABANK CREATE AUTO ^(ASTBOOL yesNoSimple )
			 | DATABANK CREATE MESSAGE '='? yesNoSimple -> DATABANK CREATE MESSAGE ^(ASTBOOL yesNoSimple )			
			 | DATABANK SEARCH '='? yesNoSimple -> DATABANK SEARCH ^(ASTBOOL yesNoSimple )			
			 | DATABANK LOGIC '='? optionDatabankLogic -> DATABANK LOGIC ^(ASTSTRINGSIMPLE optionDatabankLogic )		

			 | FOLDER question -> FOLDER question
             | FOLDER '='? yesNoSimple -> FOLDER ^(ASTBOOL yesNoSimple)
             | FOLDER BANK    '='? fileName ->  FOLDER BANK ^(ASTSTRINGSIMPLE fileName)
             | FOLDER BANK1    '='? fileName ->  FOLDER BANK1 ^(ASTSTRINGSIMPLE fileName)
             | FOLDER BANK2    '='? fileName ->  FOLDER BANK2 ^(ASTSTRINGSIMPLE fileName)
             | FOLDER COMMAND    '='? fileName ->  FOLDER COMMAND ^(ASTSTRINGSIMPLE fileName)
             | FOLDER COMMAND1    '='? fileName ->  FOLDER COMMAND1 ^(ASTSTRINGSIMPLE fileName)
             | FOLDER COMMAND2    '='? fileName ->  FOLDER COMMAND2 ^(ASTSTRINGSIMPLE fileName)
             | FOLDER HELP   '='? fileName ->  FOLDER HELP ^(ASTSTRINGSIMPLE fileName)
             | FOLDER MENU     '='? fileName ->  FOLDER MENU ^(ASTSTRINGSIMPLE fileName)
             | FOLDER MODEL   '='? fileName ->  FOLDER MODEL ^(ASTSTRINGSIMPLE fileName)
             | FOLDER PIPE '='? fileName ->  FOLDER PIPE ^(ASTSTRINGSIMPLE fileName)			
             | FOLDER TABLE    '='? fileName ->  FOLDER TABLE ^(ASTSTRINGSIMPLE fileName)
             | FOLDER TABLE1   '='? fileName ->  FOLDER TABLE1 ^(ASTSTRINGSIMPLE fileName)
             | FOLDER TABLE2   '='? fileName ->  FOLDER TABLE2 ^(ASTSTRINGSIMPLE fileName)
             | FOLDER WORKING '='? fileName ->  FOLDER WORKING ^(ASTSTRINGSIMPLE fileName)

			 | FREQ question -> FREQ question
             | FREQ '='? optionFreq -> FREQ ^(ASTSTRINGSIMPLE optionFreq)

			 | INTERFACE question -> INTERFACE question
             | INTERFACE CLIPBOARD DECIMALSEPARATOR '='? optionInterfaceExcelDecimalseparator -> INTERFACE CLIPBOARD DECIMALSEPARATOR ^(ASTSTRINGSIMPLE optionInterfaceExcelDecimalseparator)
			 | INTERFACE CSV DECIMALSEPARATOR '='? optionInterfaceExcelDecimalseparator -> INTERFACE CSV DECIMALSEPARATOR ^(ASTSTRINGSIMPLE optionInterfaceExcelDecimalseparator)
			 | INTERFACE DATABANK SWAP '='? yesNoSimple -> INTERFACE DATABANK SWAP ^(ASTBOOL yesNoSimple)			
			 | INTERFACE DEBUG '='? optionInterfaceDebug -> INTERFACE DEBUG ^(ASTSTRINGSIMPLE optionInterfaceDebug)
             | INTERFACE MODE '='? mode2 -> INTERFACE MODE ^(ASTSTRINGSIMPLE mode2)
			 | INTERFACE EXCEL LANGUAGE '='? optionInterfaceExcelLanguage -> INTERFACE EXCEL LANGUAGE ^(ASTSTRINGSIMPLE optionInterfaceExcelLanguage)
             | INTERFACE EXCEL MODERNLOOK '='? yesNoSimple -> INTERFACE EXCEL MODERNLOOK ^(ASTBOOL yesNoSimple)
             | INTERFACE HELP COPYLOCAL '='? yesNoSimple -> INTERFACE HELP COPYLOCAL ^(ASTBOOL yesNoSimple)
             | INTERFACE SOUND '='? yesNoSimple -> INTERFACE SOUND ^(ASTBOOL yesNoSimple)
             | INTERFACE SOUND TYPE '='? optionInterfaceSound -> INTERFACE SOUND TYPE ^(ASTSTRINGSIMPLE optionInterfaceSound)
             | INTERFACE SOUND WAIT '='? Integer -> INTERFACE SOUND WAIT ^(ASTINTEGER Integer)
             | INTERFACE SUGGESTIONS '='? optionInterfaceSuggestions -> INTERFACE SUGGESTIONS ^(ASTSTRINGSIMPLE optionInterfaceSuggestions)             | MODEL question -> MODEL question
			 | INTERFACE TABLE PRINTCODES '='? yesNoSimple ->  INTERFACE TABLE PRINTCODES  ^(ASTBOOL yesNoSimple)
			 | INTERFACE ZOOM '='? Integer -> INTERFACE ZOOM ^(ASTINTEGER Integer)

			 | MENU question -> MENU question
			 | MENU STARTFILE '='? fileName ->  MENU STARTFILE ^(ASTSTRINGSIMPLE fileName)

			 | MODEL question -> MODEL question
             | MODEL CACHE MAX '='? Integer -> MODEL CACHE MAX  ^(ASTINTEGER Integer)
             | MODEL CACHE '='? yesNoSimple -> MODEL CACHE ^(ASTBOOL yesNoSimple)
			 | MODEL INFOFILE '='? optionModelInfoFile -> MODEL INFOFILE ^(ASTSTRINGSIMPLE optionModelInfoFile)

			 | PLOT question -> PLOT question
			 | PLOT LINES POINTS '='? yesNoSimple -> PLOT LINES POINTS ^(ASTBOOL yesNoSimple )			
			
			 | PRINT question -> PRINT question
			 | PRINT COLLAPSE '='? optionPrintCollapse ->  PRINT COLLAPSE ^(ASTSTRINGSIMPLE optionPrintCollapse)
			 | PRINT FREQ '='? optionPrintFreq ->  PRINT FREQ ^(ASTSTRINGSIMPLE optionPrintFreq)
             | PRINT DISP MAXLINES '='? '-'? Integer -> PRINT DISP MAXLINES ^(ASTINTEGER '-'? Integer)  //can be set to -1
             | PRINT FIELDS NDEC '='? Integer -> PRINT FIELDS NDEC ^(ASTINTEGER Integer)
             | PRINT FIELDS NWIDTH '='? Integer -> PRINT FIELDS NWIDTH ^(ASTINTEGER Integer)
             | PRINT FIELDS PDEC '='? Integer -> PRINT FIELDS PDEC ^(ASTINTEGER Integer)
             | PRINT FIELDS PWIDTH '='? Integer -> PRINT FIELDS PWIDTH ^(ASTINTEGER Integer)
             | PRINT FILEWIDTH '='? Integer -> PRINT FILEWIDTH ^(ASTINTEGER Integer)
			 | PRINT MULPRT (GDIF|GDIFF) '='? yesNoSimple -> PRINT MULPRT GDIF ^(ASTBOOL yesNoSimple)
             | PRINT MULPRT ABS '='? yesNoSimple -> PRINT MULPRT ABS ^(ASTBOOL yesNoSimple)
             | PRINT MULPRT LEV '='? yesNoSimple -> PRINT MULPRT LEV ^(ASTBOOL yesNoSimple)
             | PRINT MULPRT PCH '='? yesNoSimple -> PRINT MULPRT PCH ^(ASTBOOL yesNoSimple)
             | PRINT MULPRT V '='? yesNoSimple -> PRINT MULPRT V ^(ASTBOOL yesNoSimple)
             | PRINT PRT (DIF|DIFF) '='? yesNoSimple -> PRINT PRT DIF ^(ASTBOOL yesNoSimple)
             | PRINT PRT (GDIF|GDIFF) '='? yesNoSimple -> PRINT PRT GDIF ^(ASTBOOL yesNoSimple)
             | PRINT PRT ABS '='? yesNoSimple -> PRINT PRT ABS ^(ASTBOOL yesNoSimple)
             | PRINT PRT PCH '='? yesNoSimple -> PRINT PRT PCH ^(ASTBOOL yesNoSimple)
			 | PRINT WIDTH '='? Integer -> PRINT WIDTH ^(ASTINTEGER Integer)

			 | R EXE PATH '='? fileName -> R EXE PATH ^(ASTSTRINGSIMPLE fileName)

			 | SHEET question -> SHEET question
			 | SHEET MULPRT (GDIF|GDIFF) '='? yesNoSimple -> SHEET MULPRT GDIF ^(ASTBOOL yesNoSimple)
             | SHEET MULPRT ABS '='? yesNoSimple -> SHEET MULPRT ABS ^(ASTBOOL yesNoSimple)
             | SHEET MULPRT LEV '='? yesNoSimple -> SHEET MULPRT LEV ^(ASTBOOL yesNoSimple)
             | SHEET MULPRT PCH '='? yesNoSimple -> SHEET MULPRT PCH ^(ASTBOOL yesNoSimple)
             | SHEET MULPRT V '='? yesNoSimple -> SHEET MULPRT V ^(ASTBOOL yesNoSimple)
             | SHEET PRT (DIF|DIFF) '='? yesNoSimple -> SHEET PRT DIF ^(ASTBOOL yesNoSimple)
             | SHEET PRT (GDIF|GDIFF) '='? yesNoSimple -> SHEET PRT GDIF ^(ASTBOOL yesNoSimple)
             | SHEET PRT ABS '='? yesNoSimple -> SHEET PRT ABS ^(ASTBOOL yesNoSimple)
             | SHEET PRT PCH '='? yesNoSimple -> SHEET PRT PCH ^(ASTBOOL yesNoSimple)
			 | SHEET PRT PCH '='? yesNoSimple -> SHEET PRT PCH ^(ASTBOOL yesNoSimple)
			 | SHEET ROWS  '='? yesNoSimple -> SHEET ROWS ^(ASTBOOL yesNoSimple)
			 | SHEET COLS  '='? yesNoSimple -> SHEET COLS ^(ASTBOOL yesNoSimple)			

             | SOLVE question -> SOLVE question
             | SOLVE DATA CREATE AUTO '='? yesNoSimple -> SOLVE DATA CREATE AUTO ^(ASTBOOL yesNoSimple)
             | SOLVE DATA IGNOREMISSING '='? yesNoSimple -> SOLVE DATA IGNOREMISSING ^(ASTBOOL yesNoSimple)
             | SOLVE DATA INIT '='? yesNoSimple -> SOLVE DATA INIT ^(ASTBOOL yesNoSimple)
             | SOLVE DATA INIT GROWTH '='? yesNoSimple -> SOLVE DATA INIT GROWTH ^(ASTBOOL yesNoSimple)			
			 //should handle negative numbers:
			 | SOLVE DATA INIT GROWTH MAX '='? numberIntegerOrDouble -> SOLVE DATA INIT GROWTH MAX numberIntegerOrDouble
             | SOLVE DATA INIT GROWTH MIN '='? numberIntegerOrDouble -> SOLVE DATA INIT GROWTH MIN numberIntegerOrDouble
			 | SOLVE FAILSAFE '='? yesNoSimple -> SOLVE FAILSAFE ^(ASTBOOL yesNoSimple)
			 | SOLVE FORWARD question -> SOLVE FORWARD question
			 | SOLVE FORWARD DUMP '='? yesNoSimple -> SOLVE FORWARD DUMP ^(ASTBOOL yesNoSimple)  //common for fair and nfair
             | SOLVE FORWARD FAIR CONV '='? optionSolveGaussConv -> SOLVE FORWARD FAIR CONV ^(ASTSTRINGSIMPLE optionSolveGaussConv )
             | SOLVE FORWARD FAIR CONV1 ABS '='? numberIntegerOrDouble -> SOLVE FORWARD FAIR CONV1 ABS numberIntegerOrDouble
             | SOLVE FORWARD FAIR CONV1 REL '='? numberIntegerOrDouble -> SOLVE FORWARD FAIR CONV1 REL numberIntegerOrDouble
             | SOLVE FORWARD FAIR CONV2 TABS '='? numberIntegerOrDouble -> SOLVE FORWARD FAIR CONV2 TABS numberIntegerOrDouble
             | SOLVE FORWARD FAIR CONV2 TREL '='? numberIntegerOrDouble -> SOLVE FORWARD FAIR CONV2 TREL numberIntegerOrDouble
             | SOLVE FORWARD FAIR DAMP '='? numberIntegerOrDouble  -> SOLVE FORWARD FAIR DAMP numberIntegerOrDouble			
             | SOLVE FORWARD FAIR ITERMAX '='? Integer -> SOLVE FORWARD FAIR ITERMAX ^(ASTINTEGER Integer)
             | SOLVE FORWARD FAIR ITERMIN '='? Integer -> SOLVE FORWARD FAIR ITERMIN ^(ASTINTEGER Integer)			
			 | SOLVE FORWARD NFAIR CONV '='? optionSolveGaussConv -> SOLVE FORWARD NFAIR CONV ^(ASTSTRINGSIMPLE optionSolveGaussConv )
             | SOLVE FORWARD NFAIR CONV1 ABS '='? numberIntegerOrDouble -> SOLVE FORWARD NFAIR CONV1 ABS numberIntegerOrDouble
             | SOLVE FORWARD NFAIR CONV1 REL '='? numberIntegerOrDouble -> SOLVE FORWARD NFAIR CONV1 REL numberIntegerOrDouble
             | SOLVE FORWARD NFAIR CONV2 TABS '='? numberIntegerOrDouble -> SOLVE FORWARD NFAIR CONV2 TABS numberIntegerOrDouble
             | SOLVE FORWARD NFAIR CONV2 TREL '='? numberIntegerOrDouble -> SOLVE FORWARD NFAIR CONV2 TREL numberIntegerOrDouble
             | SOLVE FORWARD NFAIR DAMP '='? numberIntegerOrDouble  -> SOLVE FORWARD NFAIR DAMP numberIntegerOrDouble						
             | SOLVE FORWARD NFAIR ITERMAX '='? Integer -> SOLVE FORWARD NFAIR ITERMAX ^(ASTINTEGER Integer)
             | SOLVE FORWARD NFAIR ITERMIN '='? Integer -> SOLVE FORWARD NFAIR ITERMIN ^(ASTINTEGER Integer)			
			 | SOLVE FORWARD NFAIR UPDATEFREQ '='? Integer -> SOLVE FORWARD NFAIR UPDATEFREQ ^(ASTINTEGER Integer)						
			 | SOLVE FORWARD STACKED HORIZON '='? Integer -> SOLVE FORWARD STACKED HORIZON ^(ASTINTEGER Integer)
             | SOLVE FORWARD METHOD '='? optionSolveForwardMethodOptions -> SOLVE FORWARD METHOD ^(ASTSTRINGSIMPLE optionSolveForwardMethodOptions)
             | SOLVE FORWARD TERMINAL '='? optionSolveForwardTerminalOptions -> SOLVE FORWARD TERMINAL ^(ASTSTRINGSIMPLE optionSolveForwardTerminalOptions)
			 | SOLVE FORWARD TERMINAL FEED '='? optionSolveForwardTerminalfeedOptions -> SOLVE FORWARD TERMINAL FEED ^(ASTSTRINGSIMPLE optionSolveForwardTerminalfeedOptions)
			 | SOLVE GAUSS question -> SOLVE GAUSS question
			 | SOLVE GAUSS CONV '='? optionSolveGaussConv -> SOLVE GAUSS CONV ^(ASTSTRINGSIMPLE optionSolveGaussConv )
             | SOLVE GAUSS CONV IGNOREVARS '='? yesNoSimple -> SOLVE GAUSS CONV IGNOREVARS ^(ASTBOOL yesNoSimple)
             | SOLVE GAUSS CONV1 ABS '='? numberIntegerOrDouble -> SOLVE GAUSS CONV1 ABS numberIntegerOrDouble
             | SOLVE GAUSS CONV1 REL '='? numberIntegerOrDouble -> SOLVE GAUSS CONV1 REL numberIntegerOrDouble
             | SOLVE GAUSS CONV2 TABS '='? numberIntegerOrDouble -> SOLVE GAUSS CONV2 TABS numberIntegerOrDouble
             | SOLVE GAUSS CONV2 TREL '='? numberIntegerOrDouble -> SOLVE GAUSS CONV2 TREL numberIntegerOrDouble			
			 | SOLVE GAUSS DAMP '='? numberIntegerOrDouble  -> SOLVE GAUSS DAMP numberIntegerOrDouble
			 | SOLVE GAUSS DUMP '='? yesNoSimple -> SOLVE GAUSS DUMP ^(ASTBOOL yesNoSimple)
             | SOLVE GAUSS ITERMAX '='? Integer -> SOLVE GAUSS ITERMAX ^(ASTINTEGER Integer)
             | SOLVE GAUSS ITERMIN '='? Integer -> SOLVE GAUSS ITERMIN ^(ASTINTEGER Integer)
             | SOLVE GAUSS REORDER '='? yesNoSimple -> SOLVE GAUSS REORDER ^(ASTBOOL yesNoSimple)
             | SOLVE METHOD '='? optionSolveMethodOptions -> SOLVE METHOD ^(ASTSTRINGSIMPLE optionSolveMethodOptions)
             | SOLVE NEWTON question -> SOLVE NEWTON question
			 | SOLVE NEWTON BACKTRACK '='? yesNoSimple -> SOLVE NEWTON BACKTRACK ^(ASTBOOL yesNoSimple)
			 | SOLVE NEWTON CONV ABS '='? numberIntegerOrDouble -> SOLVE NEWTON CONV ABS numberIntegerOrDouble
             | SOLVE NEWTON INVERT '='? optionSolveNewtonInvert -> SOLVE NEWTON INVERT ^(ASTSTRINGSIMPLE optionSolveNewtonInvert)
             | SOLVE NEWTON ITERMAX '='? Integer -> SOLVE NEWTON ITERMAX ^(ASTINTEGER Integer)
             | SOLVE NEWTON UPDATEFREQ '='? Integer -> SOLVE NEWTON UPDATEFREQ ^(ASTINTEGER Integer)
             | SOLVE PRINT '='? yesNoSimple -> SOLVE PRINT ^(ASTBOOL yesNoSimple)  //obsolete
             | SOLVE PRINT DETAILS '='? yesNoSimple -> SOLVE PRINT DETAILS ^(ASTBOOL yesNoSimple)
             | SOLVE PRINT ITER '='? yesNoSimple -> SOLVE PRINT ITER ^(ASTBOOL yesNoSimple)
             | SOLVE STATIC '='? yesNoSimple -> SOLVE STATIC ^(ASTBOOL yesNoSimple)
             | SOLVE UNDO '='? yesNoSimple -> SOLVE UNDO ^(ASTBOOL yesNoSimple)
			
			 | SYSTEM CODE SPLIT '='? Integer -> SYSTEM CODE SPLIT ^(ASTINTEGER Integer)
			
			 | TABLE question -> TABLE question
             | TABLE HTML DATAWIDTH '='? numberIntegerOrDouble ->  TABLE HTML DATAWIDTH numberIntegerOrDouble
			 | TABLE HTML FIRSTCOLWIDTH '='? numberIntegerOrDouble ->  TABLE HTML FIRSTCOLWIDTH numberIntegerOrDouble
             | TABLE HTML FONT '='? fileName ->  TABLE HTML FONT ^(ASTSTRINGSIMPLE fileName)
             | TABLE HTML FONTSIZE '='? numberIntegerOrDouble ->  TABLE HTML FONTSIZE numberIntegerOrDouble		
			 | TABLE HTML SECONDCOLWIDTH '='? numberIntegerOrDouble ->  TABLE HTML SECONDCOLWIDTH numberIntegerOrDouble
			 | TABLE HTML SPECIALMINUS '='? yesNoSimple ->  TABLE HTML SPECIALMINUS ^(ASTBOOL yesNoSimple)
             | TABLE IGNOREMISSINGVARS '='? yesNoSimple ->  TABLE IGNOREMISSINGVARS ^(ASTBOOL yesNoSimple)			
             | TABLE TYPE '='? tableType ->  TABLE TYPE ^(ASTSTRINGSIMPLE tableType)
			
			 | TIMEFILTER question -> TIMEFILTER question
             | TIMEFILTER '='? yesNoSimple -> TIMEFILTER ^(ASTBOOL yesNoSimple)
             | TIMEFILTER TYPE '='? timefilterType -> TIMEFILTER TYPE ^(ASTSTRINGSIMPLE timefilterType)
            ;

			 optionModelInfoFile: YES | NO | TEMP;
			 timefilterType: HIDE | AVG;
			 tableType: TXT | HTML;
			 optionPrintCollapse: AVG | TOTAL | NONE;
			 optionPrintFreq: SIMPLE | PRETTY;
			 optionSolveMethodOptions : NEWTON | GAUSS ;
			 optionSolveGaussConv : CONV1 | CONV2;
			 optionDatabankFileFormatOptions : TSD | TSDX | GBK;
			 optionDatabankLogic : DEFAULT | AREMOS;
			 optionInterfaceDebug: NONE | DIALOG;
			 optionInterfaceSound: BOWL | DING | NOTIFY  | RING;
			 optionInterfaceSuggestions: NONE | OPTION; // | SOME | ALL;
			 optionInterfaceExcelLanguage: DANISH | ENGLISH;
			 optionInterfaceExcelDecimalseparator: PERIOD | COMMA;
			 optionSolveNewtonInvert: LU | ITER;
			 optionSolveForwardMethodOptions : STACKED | FAIR | NFAIR | NONE ;
			 optionSolveForwardTerminalOptions : EXO | CONST | GROWTH ;
			 optionSolveForwardTerminalfeedOptions : INTERNAL | EXTERNAL;
			 optionFreq      :       a | q | m | u;
			 a       :       A;
			 q       :       Q;
			 m       :       M;
			 u       :       U;

//-----------------------------------------------------------------------------------------------------------
//---------------------------- Semi lexer stuff -------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------

integer                   : Integer -> ^(ASTINTEGER Integer);
integerNegative           : MINUS integer -> ^(ASTINTEGERNEGATIVE integer);
doubleNegative            : MINUS double2 -> ^(ASTDOUBLENEGATIVE double2);

ident                     : Ident|
							USING|
							ERROR|
                            ABS|
							DEFAULT|
							LOGIC|
                            ACCEPT|
                            ADD|
                            AFTER2|
                            AFTER|
                            ALIGNCENTER|
                            ALIGNLEFT|
                            ALIGNRIGHT|
                            ALL|
                            ANALYZE|
                            AND|
                            APPEND|
                            AREMOS|
                            AS|
                            AUTO|
                            AVG|
                            A|
                            BACKTRACK|
                            BANK1|
                            BANK2|
                            BANK|
                            BOWL|
                            BY|
                            CACHE|
                            CALC|
                            CAPS|
                            CELL|
                            CHANGE|
                            CHECKOFF|
                            CLEAR2|
                            CLEAR|
                            CLIPBOARD|
                            CLIP|
                            CLONE|
                            CLOSEALL|
                            CLOSEBANKS|
                            CLOSE|
                            CLS|
                            CODE|
                            COLLAPSE|
                            COLORS|
                            COLS|
                            COMMAND1|
                            COMMAND2|
                            COMMAND|
                            COMMA|
                            COMPARE|
                            COMPRESS|
                            CONST|
                            CONV1|
                            CONV2|
                            CONV|
                            COPYLOCAL|
                            COPY|
                            COUNT|
                            CPLOT|
                            CREATEVARS|
                            CREATE|
                            CSV|
                            CURROW|
                            DAMP|
                            DANISH|
                            DATABANK|
                            DATAWIDTH|
                            DATA|
                            DATES|
                            DATE|
                            DEBUG|
                            DECIMALSEPARATOR|
                            DECOMP|
                            DEC|
                            DELETE|
                            DETAILS|
                            DIALOG|
                            DIFF|
                            DIFPRT|
                            DIF|
                            DING|
                            DIRECT|
                            DISPLAY|
                            DISP|
                            DOC|
                            DOWNLOAD|
                            DP|
                            DUMOFF|
                            DUMOF|
                            DUMON|
                            DUMP|
                            D|
                            EDIT|
                            EFTER|
                            ELSE|
                            ENDO|
                            END|
                            ENGLISH|
                            EXCEL|
							EXE|
                            EXIT|
                            EXO|
                            EXPORT|
                            EXP|
                            EXTERNAL|
                            FAILSAFE|
                            FAIR|
                            FAST|
                            FEEDBACK|
                            FEED|
                            FIELDS|
                            FILEWIDTH|
                            FILE|
                            FILTER|
                            FINDMISSINGDATA|
                            FIRSTCOLWIDTH|
                            FIRST|
                            FIX|
                            FLAT|
                            FOLDER|
                            FONTSIZE|
                            FONT|
                            FORMAT|
                            FORWARD|
                            FOR|
                            FREQ|
                            FRML|
                            FROM|
                            FUNCTION|
                            GAUSS|
                            GBK|
                            GDIFF|
                            GDIF|
                            GEKKO18|
                            GENR|
                            GMULPRT|
                            GNUPLOT|
                            GOAL|
                            GOTO|
                            GRAPH|
                            GROWTH|
                            HDG|
                            HEADING|
                            HELP|
                            HIDELEFTBORDER|
                            HIDERIGHTBORDER|
                            HIDE|
                            HORIZON|
                            HPFILTER|
                            HTML|
                            IF|
                            IGNOREMISSINGVARS|
                            IGNOREMISSING|
                            IGNOREVARS|
                            IMPORT|
                            INDEX|
                            INFO|
							INFOFILE|
                            INIT|
                            INI|
                            INTERFACE|
                            INTERNAL|
                            INVERT|
                            ITERMAX|
                            ITERMIN|
                            ITERSHOW|
                            ITER|
                            KEEP|
                            LABELS|
                            LABEL|
                            LAG|
                            LANGUAGE|
                            LAST|
                            LEV|
                            LINES|
                            LISTFILE|
                            LIST|
                            LOG|
							LOCK_|
							UNLOCK_|
                            LU|
                            MACRO2|
                            MAIN|
                            MATRIX|
                            MAXLINES|
                            MAX|
                            MEM|
                            MENUTABLE|
                            MENU|
                            MERGECOLS|
                            MERGE|
                            MESSAGE|
                            METHOD|
                            MIN|
                            MIXED|
                            MODEL|
                            MODERNLOOK|
                            MODE|
                            MP|
                            MULBK|
                            MULPCT|
                            MULPRT|
                            MUTE|
                            M|
                            NAMES|
                            NAME|
                            NDEC|
                            NDIFPRT|
                            NEWTON|
                            NEW|
                            NEXT|
                            NFAIR|
                            NOABS|
                            NOCR|
                            NODIFF|
                            NODIF|
                            NOFILTER|
                            NOGDIFF|
                            NOGDIF|
                            NOLEV|
                            NONE|
                            NONMODEL|
                            NOPCH|
							SAVE|
                            NOTIFY|
                            NOT|
                            NOV|
                            NO|
                            NWIDTH|
                            NYTVINDU|
                            N|
                            OLS|
                            OPEN|
                            OPTION|
                            OR|
                            PARAM|
                            PATCH|
							PATH|
                            PAUSE|
                            PCH|
                            PCIMSTYLE|
                            PCIM|
                            PCTPRT|
                            PDEC|
                            PERIOD|
                            PIPE|
                            PLOTCODE|
                            PLOT|
                            POINTS|
							POS|
                            PREFIX|
                            PRETTY|
                            PRIM|
                            PRINTCODES|
                            PRINT|
                            PRI|
                            PRN|
                            PROT|
                            PRTX|
                            PRT|
                            PUDVALG|
                            PWIDTH|
                            P|
                            Q|
                            R_EXPORT|
                            R_FILE|
                            R_RUN|
                            RDP|
                            RD|
                            READ|
                            REF|
                            REL|
                            RENAME|
                            REORDER|
                            REPLACE|
                            REP|
                            RESET|
                            RESPECT|
                            RESTART|
                            RES|
                            RETURN|
                            RING|
                            RN|
                            ROWS|
                            RP|
                            RUN|
                            R|
                            SEARCH|
                            SECONDCOLWIDTH|
							SEC|
                            SER2|
                            SERIES2|
                            SERIES|
                            SER|
                            SETBORDER|
                            SETBOTTOMBORDER|
                            SETDATES|
                            SETLEFTBORDER|
                            SETRIGHTBORDER|
                            SETTEXT|
                            SETTOPBORDER|
                            SETVALUES|
                            SET|
                            SHEET|
                            SHOWBORDERS|
                            SHOWPCH|
                            SHOW|
                            SIGN|
                            SIMPLE|
                            SIM|
                            SKIP|
                            SMOOTH|
                            SOLVE|
                            SOME|
                            SORT|
                            SOUND|
                            SOURCE|
                            SPECIALMINUS|
                            SPLICE|
                            SPLIT|
                            STACKED|
                            STAMP|
                            STARTFILE|
                            STATIC|
                            STEP|
                            STOP|
                            STRING2|
                            STRIP|
                            SUFFIX|
                            SUGGESTIONS|
                            SWAP|
                            SYSTEM|
                            SYS|
                            TABLE1|
                            TABLE2|
                            TABLEOLD|
                            TABLE|
                            TABS|
                            TARGET|
                            TELL|
							TEMP|
                            TERMINAL|
                            TESTRANDOMMODELCHECK|
                            TESTRANDOMMODEL|
                            TESTSIM|
                            TEST|
                            TIMEFILTER|
                            TIMESPAN|
                            TIME|
                            TITLE|
                            TOTAL|
                            TO|
                            TRANSLATE|
                            TRANSPOSE|
                            TREL|
                            TRUNCATE|
                            TSDX|
                            TSD|
                            TSP|
                            TXT|
                            TYPE|
                            UABS|
                            UDIFF|
                            UDIF|
                            UDVALG|
                            UGDIFF|
                            UGDIF|
                            ULEV|
                            UNDO|
                            UNFIX|
                            UNSWAP|
                            UPCH|
                            UPDATEFREQ|
                            UPDX|
                            U|
                            VALUE|
                            VAL|
                            VERSION|
                            VERS|
                            VPRT|
                            V|
                            WAIT|
                            WIDTH|
                            WINDOW|
                            WORKING|
                            WPLOT|
                            WRITE|
                            WUDVALG|
                            X12A|
                            XLSX|
                            XLS|
                            YES|
                            YMAX|
                            YMIN|
                            ZERO|
                            ZOOM|
                            ZVAR
;


identDigit                : identDigitHelper -> ^(ASTIDENTDIGIT identDigitHelper);
identDigitHelper
						  : ident                 //for instance ab27
						  | Integer               //for instance 0123
						  | DigitsEDigits         //for instance 25e12 (will end here, not in IdentStartingWithInt)
						  | DateDef               //for instance 2012q3 (will end here, not in IdentStartingWithInt)						  						
						  | IdentStartingWithInt  //for instance 0123ab27 (catches the rest of these cases)						  						
						  ;			
						
					//!!!	  extratokens maybe as Extratokens ident, or put directly in ident...		


LISTSTAR                  : '&*';
LISTPLUS                  : '&+';
LISTMINUS                 : '&-';


starHelper				  : star
						  | DIV
						  | MOD						
						  ;

pow                       : stars -> ASTPOW
                          | HAT -> ASTPOW
						  ;

double2                   : double2Helper -> ^(ASTDOUBLE double2Helper);
double2Helper             : Double            //0.123 or 25e+12
						  | DigitsEDigits     //for instance 25e12 which can also be a name chunk.
						  ;
						
						  //If æøåÆØÅ then you need to put inside ''. Also with blanks. And parts beginning with a digit will not work either (5file.7z)
fileName                  : fileNameFirstPart (GLUEBACKSLASH fileNamePart)* -> ^(ASTFILENAME fileNameFirstPart fileNamePart*)
						  | expression
						  ;
fileNameFirstPart         : fileNameFirstPart1  //   c:\xx
						  | fileNameFirstPart2  //   \xx
						  | fileNameFirstPart3  //   xx
						  ;
						  //For instance READ c:\a.b\c.d, cannot be c:a.b\c.d
						    //ok to use name before colon, drive indicator should start with a letter.
fileNameFirstPart1        : name ':' GLUEBACKSLASH fileNamePart -> ^(ASTFILENAMEFIRST1 name fileNamePart);
                          //For instance READ \a.b\c.d, cannot be READ\a.b\c.d
fileNameFirstPart2        : BACKSLASH fileNamePart -> ^(ASTFILENAMEFIRST2 fileNamePart);
                          //For instance READ a.b
fileNameFirstPart3        : fileNamePart -> ^(ASTFILENAMEFIRST3 fileNamePart);
							//stuff like 'a.7z' or 'a b.doc' or 'æøå.doc' must be in quotes.
fileNamePart              : fileNamePartHelper (GLUEDOT DOT fileNamePartHelper)* -> ^(ASTFILENAMEPART fileNamePartHelper+);

fileNamePartHelper        : name | scalarName;

						  //If æøåÆØÅ then you need to put inside ''. Also with blanks. And parts beginning with a digit will not work either (5file.7z)
url                       : urlFirstPart (DIV urlPart)* -> ^(ASTURL urlFirstPart urlPart*)
						  | expression
						  ;
urlFirstPart              : urlFirstPart1
						  | urlFirstPart3
						  ;
						  //For instance HTTP://a.b/c.d						
urlFirstPart1             : HTTP urlPart -> ^(ASTURLFIRST1 HTTP urlPart);
urlFirstPart3             : urlPart -> ^(ASTURLFIRST3 urlPart);
							//stuff like 'a.7z' or 'a b.doc' or 'æøå.doc' must be in quotes.
urlPart                   : name (GLUEDOT DOT name)* -> ^(ASTURLPART name+);

                          //You cannot use <%s1 %s2> with s1='m' and s2='d' to get a <m d> print. But you can use <m=%yesno1 d=%yesno2> to control stuff like that, where yesno1 and yesno2 are strings. In that case, quotes are stripped from the strings.
						  //leftAngle2 is if there are two idents following the '<', for instance <m d> or <xyz zyx> or <m d p>.
						  //leftAngleNo2 is if there are NOT two idents following the '<', for instance <2001m1 2002m12>, <%a %b>, <p>, <rows=yes d>.
					      //For general left angles, just use leftAngle that captures both types.
leftAngle                 : leftAngle2 | leftAngleNo2;
leftAngle2				  : LEFTANGLESPECIAL;
leftAngleNo2	          : LEFTANGLESIMPLE;

leftParen                 : (GLUE!)? LEFTPAREN;
leftParenGlue             : GLUE! LEFTPAREN;
leftParenNoGlue           : LEFTPAREN;

leftCurly                 : (GLUE!)? LEFTCURLY;
leftCurlyGlue             : GLUE! LEFTCURLY;
leftCurlyNoGlue           : LEFTCURLY;

leftBracket               : LEFTBRACKET | LEFTBRACKETGLUE;
leftBracketGlue           : LEFTBRACKETGLUE;
leftBracketNoGlue         : LEFTBRACKET;
leftBracketNoGlueWild     : LEFTBRACKETWILD;

rightParen                : RIGHTPAREN (GLUE!)?;

percentSimple             : (GLUE!)? PERCENT;

percent                   : (GLUE!)? (PERCENT);
percentGlue               : GLUE! (PERCENT);
percentNoGlue             : PERCENT;
dollarPercentNoGlue       : DOLLAR | DOLLARPERCENT;

hash                      : (GLUE!)? HASH;
hashGlue                  : GLUE! HASH;
hashNoGlue                : HASH;
dollarHashNoGlue          : DOLLARHASH;

star                      : (GLUESTAR!)? STAR (GLUESTAR!)?;
starGlueBoth              : GLUESTAR! STAR GLUESTAR!;
starGlueLeft              : GLUESTAR! STAR;
starGlueRight             : STAR GLUESTAR!;
starNoGlue                : STAR;
stars                     : (GLUESTAR!)? STARS (GLUESTAR!)?;

question                      : (GLUESTAR!)? QUESTION (GLUESTAR!)?;
questionGlueBoth              : GLUESTAR! QUESTION GLUESTAR!;
questionGlueLeft              : GLUESTAR! QUESTION;
questionGlueRight             : QUESTION GLUESTAR!;
questionNoGlue                : QUESTION;

/*------------------------------------------------------------------
 * LEXER RULES
 *------------------------------------------------------------------*/

 //TODO: Clean up what is fragments and tokens. Stuff used inside lexer rules should be fragments for sure.
 //      Maybe special names for fragments like F_digit etc. And have for instance a F_glue for '¨' that the
 //      GLUE token is defined from.

fragment NEWLINE2         : '\n' ;
fragment NEWLINE3         : '\r\n' ;
fragment DIGIT            : '0'..'9' ;
fragment LETTER           : 'a'..'z'|'A'..'Z';

HTTP                      : H_ T_ T_ P_  ':' ('//');  // 'catch HTTP://' before COMMENT interferes with the '//'

WHITESPACE                : ( '\t' | ' ' | '\u000C'| NEWLINE2 | NEWLINE3)+ { $channel=HIDDEN; } ;  //u000C is form feed

COMMENT                   : ('//') (~ (NEWLINE2|NEWLINE3))* { $channel=HIDDEN; };
COMMENT_MULTILINE         : '/*' (options {greedy=false;}: COMMENT_MULTILINE | . )* '*/' {$channel=HIDDEN;};

                            //for instance a38x
Ident                     : (LETTER|'_') (DIGIT|LETTER|'_')*  { $type = CheckKeywordsTable(Text); };
                            //for instance 12345
Integer                   : DIGIT+  ;
                            //for instance 25e12
DigitsEDigits             : DIGIT+  ( E_ )  DIGIT+;  //for instance 25e12, problem is this can also be a name chunk!
                            //for instance 2012q3
DateDef                   : DIGIT+  ( A_ | Q_ | M_ ) DIGIT+;  //for instance 2000q2 or 2003m11
                            //for instance 05a, everything not captured by Ident, Integer, DigitsEDigits, Datedef.
IdentStartingWithInt      : (DIGIT|LETTER|'_')+;

//It would not be practical to construct Double in the parser. We would like 2012q3 and 7e12 to be recognized as dates and number,
//and this seems hard to do without having the parser work on really small tokens. Would probably be confusing and slow, and we would need glue around + and - (think 1.2e+34...).
//Drawback is that we cannot handle a filenames like xx.7z, 01.txt, 12.13, but they can be put inside ''.
Double                    : DIGIT+ GLUEDOTNUMBER DOT DIGIT* Exponent?   //1.2e+12  Can be without the +
                          | DIGIT+ Exponent                             //25e12    DigitsEDigits captures the 25e12 (that is, not 25e+12) case before it could end here
						  | GLUEDOTNUMBER DOT DIGIT+ Exponent?          //.2e-13   Can be "x=.23" or "x= .23", so glue is not known. Will not read the .23 in a.23 because it will be 'a' GLUEDOT DOT '23'.
						  //| DIGIT+ GLUEDOT DOT                          //1234.    Ends with a dot
                          ;

fragment Exponent         : E_ ( '+' | '-' )? DIGIT+;

//Use ANTLR to resolve %x or %() inside a string
StringInQuotes            : ('\'' (~'\'')* '\'');

// --- These are done in Program.HandleObeyFilesNew() -------------------------------------------
GLUE                      : '¨';
GLUEDOT                   : '£';  //only relevant for '.', for instance a.b becomes a£.b, and x.1 becomes x£.1
GLUEDOTNUMBER             : '§';  //only relevant for '.', for instance 12.34 becomes 12§.34
GLUESTAR                  : '½';  //only relevant for '*' and '?', for instance a*b --> a½*½b
LEFTANGLESPECIAL          : '<=<';  //indicates that there are two idents following the '<' in the text input.
                                    // using <_< is not good, since it stumbles on mulprt<_lev>xx
MOD                       : '¤';  //does not work with '%¨%' ================> NOT DONE YET!!
GLUEBACKSLASH             : '¨\\';
// -----------------------------------------------------------------------------------------------

ISEQUAL                   : '==';
ISNOTQUAL                 : '<>';
ISLARGEROREQUAL			  : '>=';			 
ISSMALLEROREQUAL          : '<=';

AT                        : '@';
HAT                       : '^';
SEMICOLON                 : ';';
COLONGLUE                 : ':|';
COLON                     : ':';
COMMA2                    : ',';
DOT                       : '.';
HASH                      : '#';
DOLLARHASH                : '$#';
PERCENT                   : '%';
DOLLARPERCENT             : '$%';
DOLLAR                    : '$';
LEFTCURLY                 : '{';
RIGHTCURLY                : '}';
LEFTPAREN                 : '(';
RIGHTPAREN                : ')';
LEFTBRACKETGLUE           : '[_[';
LEFTBRACKETWILD           : '[¨[';  //indicates that this is probably a wildcard, not a 1x1 matrix
LEFTBRACKET               : '[';
RIGHTBRACKET              : ']';


LEFTANGLESIMPLE           : '<';
RIGHTANGLE                : '>';
STAR                      : '*';
DOUBLEVERTICALBAR1        : '||';
DOUBLEVERTICALBAR2        : '|¨|';
//GLUEDOUBLEVERTICALBAR     : '¨|¨|';
VERTICALBAR               : '|';
PLUS                      : '+';
MINUS                     : '-';
DIV                       : '/';
STARS                     : '**';
EQUAL                     : '=';
BACKSLASH                 : '\\';
QUESTION                  : '?';


fragment A_:('a'|'A');
fragment B_:('b'|'B');
fragment C_:('c'|'C');
fragment D_:('d'|'D');
fragment E_:('e'|'E');
fragment F_:('f'|'F');
fragment G_:('g'|'G');
fragment H_:('h'|'H');
fragment I_:('i'|'I');
fragment J_:('j'|'J');
fragment K_:('k'|'K');
fragment L_:('l'|'L');
fragment M_:('m'|'M');
fragment N_:('n'|'N');
fragment O_:('o'|'O');
fragment P_:('p'|'P');
fragment Q_:('q'|'Q');
fragment R_:('r'|'R');
fragment S_:('s'|'S');
fragment T_:('t'|'T');
fragment U_:('u'|'U');
fragment V_:('v'|'V');
fragment W_:('w'|'W');
fragment X_:('x'|'X');
fragment Y_:('y'|'Y');
fragment Z_:('z'|'Z');
//fragment AE_:('æ'|'Æ');
//fragment OE_:('ø'|'Ø');
//fragment AA_:('å'|'Å');

