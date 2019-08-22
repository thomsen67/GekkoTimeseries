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
	naked
    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.
*/


grammar Cmd3;

options {
  language=CSharp2;
  output = AST;
  backtrack = true;   //otherwise too many errors...
  memoize = true;
  //k=2;  //must be 2 to deal with genr statements etc. Compiling grammar with k=6 dies with memory error
          //not setting k is equivalent to LL(*) which is probably best in all cases.
}

tokens {
	ASTDOLLAR;
	ASTFORTYPE1;
	ASTNUMBERMINUS;
	ASTDECOMPWHERE;
	ASTDECOMPGROUP;
	ASTDECOMPLINK;
	ASTDECOMPLINK1;
	ASTDECOMPWHERE2;
	ASTDECOMPCOLS;
	ASTDECOMPGROUP1;
	ASTDECOMPGROUP1c;
	ASTDECOMPGROUP1d;
	ASTFORTYPE2;
	ASTNAKEDLISTMISS;
	ASTLOCAL;
	ASTARGS;
	ASTCOLON;
	ASTL0;
	ASTBLOCKOPTION;
	ASTNAKEDLISTITEM;
	ASTNAKEDLIST;
	ASTL1;
	ASTGLOBAL;
	ASTIN;
	ASTASSIGNMENTEXPRESSION;
	ASTCURLYALONE;
	ASTNUMBERLIST;
	ASTOBJECTFUNCTION;
	ASTLEFTSIDE;
	ASTASSIGNMENT;
	ASTCNAME;
	ASTHASH2;
	ASTPERCENT2;
	ASTSPECIALARGS;
	ASTDOTORINDEXER;
	ASTSPECIALARGSDEF;
	ASTBANKVARNAME;
	ASTBLOCK;
	ASTBANKVARNAME2;
	ASTHASH;
	ASTPERCENT;
	ASTSEQ7;
	ASTPLUS2;
	ASTMINUS2;
	ASTSTAR2;
	ASTDIV2;
	ASTVERTICALBAR;
    ASTINDEXERELEMENT;
    ASTINDEXERELEMENTBANK;
	ASTDATES_BLOCK;
	ASTSTARS;
	ASTTRIPLESTARS;
    ASTPOW;
	ASTREPSTAR;
    ASTEMPTYRANGEELEMENT;
	ASTSTAR5;
	ASTINDEXERALONE;
	ASTINDEXERELEMENTPLUS;
	ASTSTRINGINQUOTESWITHCURLIES;
	ASTINTEGER;
	ASTRANGEGENERAL;
	ASTLISTAND;
	ASTLISTOR;
	ASTVARNAME;
	ASTPLACEHOLDER;
	ASTDOT;
	ASTFUNCTION;
	ASTLOGICALIN;
	ASTDATE2;
	ASTSTRINGINQUOTES;
	ASTOPT_STRING_DYN;
	ASTOPT_STRING_BROWSER;
    ASTOPT_STRING_PRINT;
	ASTOPT_STRING_TOBANK;
	ASTOPT_STRING_FROMBANK;
	ASTOPT_STRING_UNITS;
	ASTOPT_STRING_SORT;
	ASTOPT_STRING_ALL;
	ASTOPT_STRING_NAMECELL;
	ASTOPT_STRING_DATECELL;
	ASTOPT_STRING_METHOD;

	ASTOPT_VAR_DEP;

	ASTDOUBLE;
	ASTDOLLARCONDITIONALVARIABLE;
	ASTINDEXERELEMENTIDENT;
	ASTHAT2;
	ASTSEQITEMMINUS;
	ASTPRINT;
	ASTIFSTATEMENTS;
	ASTELSESTATEMENTS;
	ASTDATES2;
	ASTNAME;
	ASTEXPRESSIONNEW;
	ASTFLEXIBLELIST;
	ASTNAMESLIST;
	ASTPROCEDURE;
ASTNAME;
ASTIDENT;
ASTCURLYSIMPLE;
ASTCURLY;
ASTIDENTDIGIT;	
	ASTPLUS;
	ASTMINUS;	
	ASTSTAR;
	ASTDIV;
	ASTPOWER;
	ASTNEGATE;
	ASTINDEXER;
	ASTMATRIXCOL;
	ASTMATRIXROW;
	ASTLISTFILE;
	ASTMAPDEF;
	ASTMAPITEM;
	ASTFILENAMELIST;
	ASTDOLLARCONDITIONAL;
	ASTLISTDEF;
	ASTLISTDEFITEM;
	ASTOBJECTFUNCTIONNAKED;
ASTOR;
ASTAND;
ASTNOT;
ASTCOMPARE;
ASTIF;
ASTIFOPERATOR;
ASTIFOPERATOR1;
ASTIFOPERATOR;
ASTIFOPERATOR2;
ASTIFOPERATOR;
ASTIFOPERATOR3;
ASTIFOPERATOR;
ASTIFOPERATOR4;
ASTIFOPERATOR;
ASTIFOPERATOR5;
ASTIFOPERATOR;
ASTIFOPERATOR6;
ASTIFOPERATOR7;
ASTCOMPARE2;
ASTRUN;
ASTFUNCTIONDEF2;
ASTRESET;
ASTRETURN;
ASTFOR;
ASTIF;
ASTCOMPARE2;
	ASTPROCEDURE;
	ASTPROCEDUREDEFTYPE;
	ASTPROCEDUREDEFNAME;
	ASTPROCEDUREDEFCODE;
	ASTPROCEDUREDEF;
    ASTPROCEDUREDEFARGS;
    ASTPROCEDUREDEFRHSSIMPLE;
    ASTPROCEDUREDEFARG;
	ASTCUT;
	ASTLEFTBRACKETGLUE;
	ASTSERIESOPERATOR;
	ASTSERIESDOLLARCONDITION;
	ASTSERIES;
	ASTSERIESLHS;
	ASTSERIESRHS;
	ASTOPT_STRING_GMS;
	ASTOPT_STRING_PX;
	ASTOPT_STRING_NOMAX;
	ASTOPT_STRING_BOLD;
	ASTOPT_STRING_ITALIC;
	ASTOPT_STRING_GRIDSTYLE;
	ASTOPT_STRING_GCM;
	ASTOPT_STRING_OP;
	ASTOPT_STRING_PREFIX;
	ASTOPT_STRING_GDX;
	ASTOPT_STRING_GDXOPT;
	ASTOPT_STRING_R;
	ASTOPT_VAL_INDEX;
	ASTOPT_VAL_ABS;
	ASTOPT_VAL_REL;
	ASTOPT_VAL_PCH;
	ASTOPT_STRING_FILE;
	ASTOPT_STRING_TYPE;
	ASTOPT_STRING_ARRAY;
	ASTOPT_STRING_SPLIT;
	ASTXLINE;
	ASTYLINE;
	ASTREBASE;
    ASTLINESPOINTS;
	ASTLINES;
	ASTBOXES;
	ASTFILLEDCURVES;
	ASTSTEPS;
	ASTPOINTS;
	ASTDOTS;
	ASTIMPULSES;
    ASTLIBRARY;
	ASTIMPOSE;
	ASTNAMEHELPER;
	ASTINTERPOLATE;
	ASTTABLEMAIN;
	ASTLISTTRIM;
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
	ASTDATES_TYPE2;  //for copy, read/write
    ASTDATES;
    ASTDATESSTATEMENT;
    ASTDATESTATEMENT;
	ASTEVAL;
    ASTDECOMP;
	ASTDECOMP2;
    ASTDECOMPITEMS;
	ASTDECOMPITEMS2;
	ASTDECOMPITEMSNAME;
	ASTDECOMPITEMSEXPR;
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
	ASTDOLLARCONDITIONAL;
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
	ASTBANKVARNAMELIST;
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
	ASTEXPORTR;
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
	ASTHANDLEFILENAME2;
    ASTHASH;
    ASTHASHNAMESIMPLE;
    ASTHASHPAREN;
    ASTHDG;
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
	ASTEXCLAMATION;
    ASTM;
    ASTMACRO;
    ASTMACROPLUS;
	ASTOPT_LIST_ROWNAMES;
	ASTOPT_LIST_COLNAMES;
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
	ASTL;
	ASTDL;
	ASTSL;
	ASTSDL;
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
	ASTOLSEXPRESSION;
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
	ASTOPT_STRING_SIZE;
	ASTOPT_STRING_PAUSE;
	ASTOPT_STRING_CONTINUE;
	ASTOPT_STRING_STOP;
ASTOPT_STRING_TITLE;
ASTOPT_STRING_SUBTITLE;
ASTOPT_STRING_FONT;
ASTOPT_VAL_FONTSIZE;
ASTOPT_STRING_TICS;
ASTOPT_STRING_GRID;
ASTOPT_STRING_KEY;
ASTOPT_STRING_PALETTE;
ASTOPT_STRING_STACK;
ASTOPT_VAL_BOXWIDTH;
ASTOPT_VAL_BOXGAP;
ASTOPT_STRING_SEPARATE;
ASTOPT_DATE_XLINE;
ASTOPT_DATE_XLINEBEFORE;
ASTOPT_DATE_XLINEAFTER;
ASTOPT_STRING_YMIRROR;
ASTOPT_STRING_YTITLE;
ASTOPT_VAL_YLINE;
ASTOPT_VAL_YMAX;
ASTOPT_VAL_YMAXHARD;
ASTOPT_VAL_YMAXSOFT;
ASTOPT_VAL_YMIN;
ASTOPT_VAL_YMINHARD;
ASTOPT_VAL_YMINSOFT;
ASTOPT_STRING_XZEROAXIS;
ASTOPT_STRING_Y2TITLE;
ASTOPT_VAL_Y2LINE;
ASTOPT_VAL_Y2MAX;
ASTOPT_VAL_Y2MAXHARD;
ASTOPT_VAL_Y2MAXSOFT;
ASTOPT_VAL_Y2MIN;
ASTOPT_VAL_Y2MINHARD;
ASTOPT_VAL_Y2MINSOFT;
ASTOPT_STRING_X2ZEROAXIS;
ASTOPT_STRING_LABEL;
ASTOPT_STRING_ARROW;
ASTOPT_STRING_LINETYPE;
ASTOPT_STRING_DASHTYPE;
ASTOPT_VAL_LINEWIDTH;
ASTOPT_STRING_LINECOLOR;
ASTOPT_STRING_POINTTYPE;
ASTOPT_VAL_POINTSIZE;
ASTOPT_STRING_FILLSTYLE;
ASTOPT_STRING_LABEL;
ASTOPT_STRING_Y2;

	ASTOPT_STRING_DUMP;
	ASTOPT_STRING_BANK;
	ASTOPT_STRING_MOVE;
	ASTOPT_STRING_REMOVE;
	ASTOPT_STRING_ADDBANK;
	ASTOPT_STRING_SHOWBANK;
	ASTOPT_STRING_SHOWFREQ;
	ASTOPT_STRING_ERROR;
	ASTOPT_STRING_USING;
    ASTOPT_STRING_ABS ;
    ASTOPT_STRING_AFTER;
    ASTOPT_STRING_APPEND;
    ASTOPT_STRING_AREMOS;
    ASTOPT_STRING_CAPS;
    ASTOPT_STRING_CELL;
	ASTOPT_STRING_CONSTANT;
    ASTOPT_STRING_COLLAPSE;
    ASTOPT_STRING_COLORS;
    ASTOPT_STRING_COLS;
    ASTOPT_STRING_CSV;
    ASTOPT_STRING_D;
    ASTOPT_STRING_DATES;
    ASTOPT_STRING_DIRECT;
	ASTOPT_STRING_EDIT;
	ASTOPT_STRING_FIRST;
	ASTOPT_STRING_CREATE;
	ASTOPT_STRING_LAST;
	ASTOPT_STRING_FILENAME;
    ASTOPT_STRING_FIX;
    ASTOPT_STRING_FROM;
    ASTOPT_STRING_GBK;
    ASTOPT_STRING_GEKKO18;
	ASTOPT_STRING_GEKKO20;
    ASTOPT_STRING_GEOMETRIC;
	ASTOPT_STRING_OVERLAY;
    ASTOPT_STRING_GNUPLOT;
    ASTOPT_STRING_TITLE;
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
	ASTOPT_STRING_L;
	ASTOPT_STRING_DL;
    ASTOPT_STRING_MUTE ;
	ASTNONCURLY;
    ASTOPT_STRING_MUTE;
    ASTOPT_STRING_N;
    ASTOPT_STRING_NAMES;
    ASTOPT_STRING_NONMODEL;
    ASTOPT_STRING_P;
    ASTOPT_STRING_PARAM;
    ASTOPT_STRING_PCIM;
    ASTOPT_STRING_PLOTCODE;
	ASTOPT_STRING_PRTCODE;
    ASTOPT_STRING_PRESERVE;
	ASTOPT_STRING_MISSING;
    ASTOPT_STRING_PRIM;
    ASTOPT_STRING_PRN;
	ASTOPT_STRING_MATRIX;
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
	ASTOPT_STRING_FLAT;
	ASTOPT_STRING_AREMOS;
	ASTOPT_VAL_WIDTH;
	ASTOPT_VAL_DEC;
	ASTOPT_VAL_NWIDTH;
	ASTOPT_VAL_PWIDTH;
	ASTOPT_VAL_NDEC;
	ASTOPT_VAL_PDEC;

    ASTOPT_VAL_LAG;
    ASTOPT_VAL_REPLACE;
    ASTOPT_VAL_YMAX;
    ASTOPT_VAL_YMIN;
	ASTOPT_VAL_Y2MAX;
    ASTOPT_VAL_Y2MIN;
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
	ASTPRTELEMENTLINETYPE;
	ASTPRTELEMENTDASHTYPE;
	ASTPRTELEMENTLINEWIDTH;
	ASTPRTELEMENTLINECOLOR;
	ASTPRTELEMENTPOINTTYPE;
	ASTPRTELEMENTPOINTSIZE;
	ASTPRTELEMENTFILLSTYLE;
	ASTPRTELEMENTY2;
    ASTPRTITEMS;
    ASTPRTOPTION;
    ASTPRTOPTIONFIELD2;
    ASTPRTOPTIONFIELD3;
    ASTPRTOPTIONFIELD;
    ASTPRTROWS;
    ASTPRTSTAMP;
    ASTPRTTIMEFILTER;
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
	ASTFUNCTIONNAKED;
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
	ASTYES2;
    ASTYMAX;
    ASTYMIN;
    ASTZERO;
	ASTXEDIT;

	// --- tokens1 start ---
	
	GMS = 'GMS';
	ELEMENTS = 'ELEMENTS';
	NOMAX = 'NOMAX';
	RETURN2 = 'RETURN';
	IN = 'IN';
	MAP = 'MAP';
	STRING2 = 'STRING';
		     REMOTE = 'REMOTE';
			 ALIAS = 'ALIAS';
			 OFFSET = 'OFFSET';
			 DETECT = 'DETECT';
			 GRIDSTYLE = 'GRIDSTYLE';
            BOLD = 'BOLD';
            ITALIC = 'ITALIC';
	            ASER = 'ASER';
            ASERIES = 'ASERIES';
	            XLABELS = 'XLABELS';
				YLABELS = 'YLABELS';
            ANNUAL = 'ANNUAL';
            AT2 = 'AT';
            BETWEEN = 'BETWEEN';
            NONANNUAL = 'NONANNUAL';
			CURRENT = 'CURRENT';
            DIGITS = 'DIGITS';
			GAMS = 'GAMS';
			GDX = 'GDX';
			GDXOPT = 'GDXOPT';
	LAGFIX = 'LAGFIX';
	ADDBANK = 'ADDBANK';
	SHOWBANK = 'SHOWBANK';
	SHOWFREQ = 'SHOWFREQ';
	REBASE = 'REBASE';
	LINESPOINTS = 'LINESPOINTS';
//LINES = 'LINES';
			BOXES = 'BOXES';
		FILLEDCURVES = 'FILLEDCURVES';
		STEPS = 'STEPS';
		//POINTS = 'POINTS';
		DOTS = 'DOTS';
		IMPULSES = 'IMPULSES';
CONTINUE              = 'CONTINUE';
VOID              = 'VOID';
PROCEDURE              = 'PROCEDURE';
SIZE                  = 'SIZE'                     ;
//TITLE                 = 'TITLE'                    ;
SUBTITLE              = 'SUBTITLE'                 ;
//FONT                  = 'FONT'                     ;
//FONTSIZE              = 'FONTSIZE'                 ;
TICS                  = 'TICS'                     ;
GRID                  = 'GRID'                     ;
KEY                   = 'KEY'                      ;
PALETTE               = 'PALETTE'                  ;
STACK                 = 'STACK'                    ;
BOXWIDTH              = 'BOXWIDTH'                 ;
BOXGAP                = 'BOXGAP'                   ;
SEPARATE              = 'SEPARATE'                 ;
XLINE                 = 'XLINE'                    ;
XLINEBEFORE           = 'XLINEBEFORE'              ;
XLINEAFTER            = 'XLINEAFTER'               ;
YMIRROR               = 'YMIRROR'                  ;
YTITLE                = 'YTITLE'                   ;
YLINE                 = 'YLINE'                    ;
//YMAX                  = 'YMAX'                     ;
YMAXHARD              = 'YMAXHARD'                 ;
YMAXSOFT              = 'YMAXSOFT'                 ;
//YMIN                  = 'YMIN'                     ;
YMINHARD              = 'YMINHARD'                 ;
YMINSOFT              = 'YMINSOFT'                 ;
XZEROAXIS             = 'XZEROAXIS'                ;
Y2TITLE               = 'Y2TITLE'                  ;
Y2LINE                = 'Y2LINE'                   ;
//Y2MAX                 = 'Y2MAX'                    ;
Y2MAXHARD             = 'Y2MAXHARD'                ;
Y2MAXSOFT             = 'Y2MAXSOFT'                ;
//Y2MIN                 = 'Y2MIN'                    ;
Y2MINHARD             = 'Y2MINHARD'                ;
Y2MINSOFT             = 'Y2MINSOFT'                ;
X2ZEROAXIS            = 'X2ZEROAXIS'               ;
//LABEL                 = 'LABEL'                    ;
ARROW                 = 'ARROW'                    ;
//LINETYPE              = 'LINETYPE'                 ;
DASHTYPE              = 'DASHTYPE'                 ;
LINEWIDTH             = 'LINEWIDTH'                ;
LINECOLOR             = 'LINECOLOR'                ;
POINTTYPE             = 'POINTTYPE'                ;
POINTSIZE             = 'POINTSIZE'                ;
FILLSTYLE             = 'FILLSTYLE'                ;
LABEL                 = 'LABEL'                    ;
Y2                    = 'Y2'                       ;

    X = 'X';
	Y = 'Y';
	MDATEFORMAT = 'MDATEFORMAT';
	THOUSANDSSEPARATOR = 'THOUSANDSSEPARATOR';
	XEDIT = 'XEDIT';
	IMPOSE = 'IMPOSE';
	CONSTANT = 'CONSTANT';
	INTERPOLATE = 'INTERPOLATE';
	PRORATE = 'PRORATE';
	TRIM = 'TRIM';
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
	OP = 'OP';
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
	MOVE = 'MOVE';
	REMOVE = 'REMOVE';
    CLOSE            = 'CLOSE';
    CLOSEALL         = 'CLOSEALL'        ;
    CLOSEBANKS       = 'CLOSEBANKS'        ;
    CLS              = 'CLS'             ;
    CODE = 'CODE';
    COLLAPSE         = 'COLLAPSE';
	PX = 'PX';
	ARRAY = 'ARRAY';
	BUGFIX = 'BUGFIX';
	GCM = 'GCM';
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
	LOCAL        = 'LOCAL'       ;
	GLOBAL        = 'GLOBAL'       ;
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
	DECOMP2           = 'DECOMP2'          ;
	EVAL           = 'EVAL'          ;
    DELETE           = 'DELETE'          ;
    DETAILS          = 'DETAILS';
    DIALOG           = 'DIALOG'          ;
    DIF              = 'DIF';
    DIFF              = 'DIFF';
    DIFPRT           = 'DIFPRT'          ;
    DING             = 'DING';
	DYN             = 'DYN';
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
	DEP = 'DEP';
	BROWSER      = 'BROWSER'     ;
    FIRST            = 'FIRST';
    FIRSTCOLWIDTH = 'FIRSTCOLWIDTH';
    FIX = 'FIX';
    FLAT             = 'FLAT'            ;
	DATECELL             = 'DATECELL'            ;
	NAMECELL             = 'NAMECELL'            ;

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
	GEKKO20 = 'GEKKO20';
    GENR             = 'GENR'            ;
    GEOMETRIC = 'GEOMETRIC';
	OVERLAY = 'OVERLAY';
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
	ASBANK = 'ASBANK';
	TOBANK = 'TOBANK';
	FROMBANK = 'FROMBANK';
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
	ROBUST           = 'ROBUST';
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
	ROWNAMES = 'ROWNAMES';
	COLNAMES = 'COLNAMES';
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
	MISSING = 'MISSING';
    MODE = 'MODE';
    MODEL            = 'MODEL'           ;
    MODERNLOOK       = 'MODERNLOOK'      ;
    MP               = 'MP'               ;
	DL               = 'DL'               ;
	L               = 'L'               ;	
	RDL               = 'RDL'               ;
	RL               = 'RL'               ;
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
	CUT = 'CUT';
    POINTS           = 'POINTS';
	POS           = 'POS';
    PREFIX = 'PREFIX';
    PRETTY = 'PRETTY';
    PRI              = 'PRI'             ;
    PRIM = 'PRIM';
    PRINT            = 'PRINT'           ;
    OPERATORS = 'OPERATORS';
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
	BLOCK = 'BLOCK';
    RESPECT = 'RESPECT';
    RESTART = 'RESTART';
    //RETURN           = 'RETURN'          ;
    RING             = 'RING';
    RN= 'RN'       ;
    ROWS             = 'ROWS';
	GROUP             = 'GROUP';
	LEVEL = 'LEVEL';
	LINK = 'LINK';
	WHERE = 'WHERE';
    RP= 'RP'       ;
    RUN              = 'RUN'             ;
	LIBRARY = 'LIBRARY';
    SEARCH = 'SEARCH';
    SEC = 'SEC';
	SECONDCOLWIDTH = 'SECONDCOLWIDTH';
    SER2 = 'S___ER';
	SER3 = 'S____ER';
    SER='SER';
    SERIES2 = 'S___ERIES';
	SERIES3 = 'S____ERIES';
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
	NAN            = 'NAN';
	ENGINE = 'ENGINE';
	NORMAL            = 'NORMAL';
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
	UNITS = 'UNITS';
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
	SPEEDUP          = 'SPEEDUP';
    UNSWAP           = 'UNSWAP';
    UPCH             = '_PCH';
    UPDATEFREQ       = 'UPDATEFREQ'      ;
    UPDX             = 'UPDX'             ;
    V= 'V'             ;
    VAL              = 'VAL'             ;
	VAR              = 'VAR'             ;
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
	Y2MAX = 'Y2MAX';
    Y2MIN = 'Y2MIN';
    ZERO             = 'ZERO'            ;
    ZOOM = 'ZOOM';
    ZVAR             = 'ZVAR'            ;
	// --- tokens1 end --- 
}

                              @parser::namespace { Gekko }

                              @lexer::namespace { Gekko }

                              @members {							                      
								 
								  private bool LB()
								 {            
						            return input.LT(-1).TokenIndex + 1 != input.LT(1).TokenIndex;
								 }
								 
								 private bool RB()
								 {            
						            return input.LT(1).TokenIndex + 1 != input.LT(2).TokenIndex;
								 }									

								 private bool LS()
								 {
                                    IToken i1 = input.LT(-1);
                                    IToken i2 = input.LT(1);
                                    if (i1.TokenIndex + 1 != i2.TokenIndex) return true;
                                    string s = i1.Text;
                                    if (s == "/" || s == "[" || s == "(" || s == "+" || s == "*" || s == ";" || s == "," || s == "-" || s == "<" || s == ">" || s == "{")
                                    {
                                       return true;
                                    }
                                    return false;                                   
                                 }							

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

                                private int stringCounter = 0;
								
								public static System.Collections.Generic.Dictionary<string, int> kw = GetKw();

                                public static System.Collections.Generic.Dictionary<string, int> GetKw()
                                {
                                        System.Collections.Generic.Dictionary<string, int> d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
										
										// --- tokens2 start ---
            			
d.Add("REMOTE", REMOTE);
d.Add("ALIAS", ALIAS);
            d.Add("GRIDSTYLE", GRIDSTYLE);
            d.Add("BOLD", BOLD);
            d.Add("ITALIC", ITALIC);
            			
			d.Add("ASER", ASER);
            d.Add("ASERIES", ASERIES);
		    d.Add("XLABELS", XLABELS);
			d.Add("YLABELS", YLABELS);
            d.Add("ANNUAL", ANNUAL);
            d.Add("AT", AT2);
            d.Add("BETWEEN", BETWEEN);
            d.Add("NONANNUAL", NONANNUAL);
			d.Add("CURRENT", CURRENT);
            d.Add("DIGITS", DIGITS);            
d.Add("X" ,X);
d.Add("Y" ,Y);
		d.Add("LAGFIX" ,LAGFIX);
		d.Add("ADDBANK" ,ADDBANK);
		d.Add("SHOWBANK" ,SHOWBANK);
		d.Add("SHOWFREQ" ,SHOWFREQ);
		d.Add("LINESPOINTS" ,LINESPOINTS);
		d.Add("REBASE", REBASE);
//d.Add("LINES" , LINES);
			d.Add("BOXES" , BOXES);
		d.Add("FILLEDCURVES" , FILLEDCURVES);
		d.Add("STEPS" , STEPS);
		//d.Add("POINTS" , POINTS);
		d.Add("DOTS" , DOTS);
		d.Add("IMPULSES" , IMPULSES);
		d.Add("OFFSET" , OFFSET);
		d.Add("DETECT" , DETECT);
		
										d.Add("SIZE",SIZE);
										d.Add("CONTINUE",CONTINUE);
										d.Add("VOID",VOID);
										d.Add("PROCEDURE",PROCEDURE);
										//d.Add("TITLE",TITLE);
										d.Add("SUBTITLE",SUBTITLE);
										//d.Add("FONT",FONT);
										//d.Add("FONTSIZE",FONTSIZE);
										d.Add("TICS",TICS);
										d.Add("GRID",GRID);
										d.Add("KEY",KEY);
										d.Add("PALETTE",PALETTE);
										d.Add("STACK",STACK);
										d.Add("BOXWIDTH",BOXWIDTH);
										d.Add("BOXGAP",BOXGAP);
										d.Add("SEPARATE",SEPARATE);
										d.Add("XLINE",XLINE);
										d.Add("XLINEBEFORE",XLINEBEFORE);
										d.Add("XLINEAFTER",XLINEAFTER);
										d.Add("YMIRROR",YMIRROR);
										d.Add("YTITLE",YTITLE);
										d.Add("YLINE",YLINE);
										//d.Add("YMAX",YMAX);
										d.Add("YMAXHARD",YMAXHARD);
										d.Add("YMAXSOFT",YMAXSOFT);
										//d.Add("YMIN",YMIN);
										d.Add("YMINHARD",YMINHARD);
										d.Add("YMINSOFT",YMINSOFT);
										d.Add("XZEROAXIS",XZEROAXIS);
										d.Add("Y2TITLE",Y2TITLE);
										d.Add("Y2LINE",Y2LINE);
										//d.Add("Y2MAX",Y2MAX);
										d.Add("Y2MAXHARD",Y2MAXHARD);
										d.Add("Y2MAXSOFT",Y2MAXSOFT);
										//d.Add("Y2MIN",Y2MIN);
										d.Add("Y2MINHARD",Y2MINHARD);
										d.Add("Y2MINSOFT",Y2MINSOFT);
										d.Add("X2ZEROAXIS",X2ZEROAXIS);
										//d.Add("LABEL",LABEL);
										d.Add("ARROW",ARROW);
										//d.Add("LINETYPE",LINETYPE);
										d.Add("DASHTYPE",DASHTYPE);
										d.Add("LINEWIDTH",LINEWIDTH);
										d.Add("LINECOLOR",LINECOLOR);
										d.Add("POINTTYPE",POINTTYPE);
										d.Add("POINTSIZE",POINTSIZE);
										d.Add("FILLSTYLE",FILLSTYLE);
										//d.Add("LABEL",LABEL);
										d.Add("Y2",Y2);
																				
										d.Add("INTERPOLATE"    ,   INTERPOLATE     );
										d.Add("XEDIT"    ,   XEDIT     );

										d.Add("MDATEFORMAT"    ,   MDATEFORMAT     );
										d.Add("THOUSANDSSEPARATOR"    ,   THOUSANDSSEPARATOR     );
											
										d.Add("MISSING"    ,   MISSING     );
										d.Add("CONSTANT", CONSTANT);
										d.Add("IMPOSE", IMPOSE);
										d.Add("PRORATE"    ,   PRORATE     );
										d.Add("TRIM"    ,   TRIM     );
										d.Add("USING"    ,   USING     );
										d.Add("ERROR"    ,   ERROR     );
										d.Add("_ABS"    ,   UABS     );
                                        d.Add("_DIF"    ,   UDIF     );
                                        d.Add("_DIFF"   ,   UDIFF     );
                                        d.Add("_GDIF"              ,   UGDIF     );
                                        d.Add("_GDIFF"             ,   UGDIFF     );
                                        d.Add("_LEV"    ,   ULEV     );
                                        d.Add("_PCH"    ,   UPCH     );
										d.Add("UNITS"    ,   UNITS     );
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
										d.Add("OP", OP);
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
										d.Add("MOVE", MOVE);
										d.Add("REMOVE", REMOVE);
                                        d.Add("close"  , CLOSE );
                                        d.Add("closeall", CLOSEALL  );
                                        d.Add("closebanks", CLOSEBANKS  );
                                        d.Add("cls"     , CLS       );
                                        d.Add("CODE" ,CODE);
										d.Add("px"  , PX );
										d.Add("array"  , ARRAY );
										d.Add("bugfix"  , BUGFIX );
										d.Add("gcm"  , GCM );
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
										d.Add("local"               , LOCAL               );
                                        d.Add("global"               , GLOBAL               );
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
										d.Add("decomp2"  , DECOMP2    );
										d.Add("eval"  , EVAL    );
                                        d.Add("delete"  , DELETE    );
                                        d.Add("details"  , DETAILS   );
                                        d.Add("dialog"  , DIALOG      );
                                        d.Add("dif"  , DIF      );
                                        d.Add("diff"  , DIFF      );
                                        d.Add("difprt"  , DIFPRT      );
                                        d.Add("ding"    , DING      );
										d.Add("dyn"    , DYN      );
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
										d.Add("findmissingdata"               , FINDMISSINGDATA  );
										d.Add("dep"               , DEP  );
                                        d.Add("browser"         , BROWSER);
                                        d.Add("first"    , FIRST  );
                                        d.Add("FIRSTCOLWIDTH" ,FIRSTCOLWIDTH);
                                        d.Add("FIX", FIX);
                                        d.Add("flat"    , FLAT      );
										d.Add("namecell"    , NAMECELL      );
										d.Add("datecell"    , DATECELL      );
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
										d.Add("GAMS", GAMS);	
										d.Add("GDX", GDX);
										d.Add("GDXOPT", GDXOPT);
                                        d.Add("gdif"    , GDIF   );
                                        d.Add("gdiff"    , GDIFF   );
                                        d.Add("GEKKO18", GEKKO18);
										d.Add("GEKKO20", GEKKO20);
                                        d.Add("genr"    , GENR      );
                                        d.Add("GEOMETRIC", GEOMETRIC);
										d.Add("OVERLAY", OVERLAY);
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

										d.Add("asbank"      , ASBANK      );
										d.Add("tobank"      , TOBANK      );
										d.Add("frombank"      , FROMBANK      );

                                        d.Add("ignoremissing"           , IGNOREMISSING             );
                                        d.Add("IGNOREMISSINGVARS"           , IGNOREMISSINGVARS              );
                                        d.Add("ignorevars"              , IGNOREVARS             );
                                        d.Add("import", IMPORT);
										d.Add("in", IN);
                                        d.Add("index"   , INDEX      );
                                        d.Add("info"    , INFO      );
										d.Add("infofile"    , INFOFILE      );
                                        d.Add("ini"    , INI      );
                                        d.Add("init"    , INIT      );
                                        d.Add("interface"               , INTERFACE );
                                        d.Add("INTERNAL", INTERNAL);
                                        d.Add("INVERT"    , INVERT      );
										d.Add("ROBUST"    , ROBUST      );
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
										d.Add("map"    , MAP      );
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
										d.Add("colnames",COLNAMES);
										d.Add("rownames",ROWNAMES);
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
										d.Add("dl" , DL);
										d.Add("l" , L);
										d.Add("rdl" , RDL);
										d.Add("rl" , RL);
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
										d.Add("cut"   , CUT     );
                                        d.Add("PLOTCODE" ,PLOTCODE);
                                        d.Add("points"    , POINTS   );
										d.Add("pos"    , POS   );
                                        d.Add("prefix"    , PREFIX   );
                                        d.Add("PRETTY" ,PRETTY);
                                        d.Add("pri"     , PRI     );
                                        d.Add("PRIM", PRIM);
                                        d.Add("print"   , PRINT     );
                                        d.Add("OPERATORS" ,OPERATORS);
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
										d.Add("BLOCK", BLOCK);
                                        d.Add("respect",RESPECT);
                                        d.Add("RESTART", RESTART);
										d.Add("elements"  , ELEMENTS    );
										d.Add("nomax"  , NOMAX    );
										d.Add("gms"  , GMS    );
                                        d.Add("return"  , RETURN2    );
                                        d.Add("ring"    , RING    );
                                        d.Add("rn"               , RN );
                                        d.Add("rows"    , ROWS    );
										d.Add("group"    , GROUP    );
										d.Add("level"    , LEVEL    );
										d.Add("link"    , LINK    );
										d.Add("where"    , WHERE    );
                                        d.Add("rp"               , RP );
                                        d.Add("run"     , RUN       );
										d.Add("library"     , LIBRARY       );
                                        d.Add("S___ER" ,SER2);
                                        d.Add("S___ERIES" ,SERIES2);
										d.Add("S____ER" ,SER3);
                                        d.Add("S____ERIES" ,SERIES3);
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
										d.Add("nan"          , NAN      );
										d.Add("engine"          , ENGINE      );
										d.Add("normal"          , NORMAL      );
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
										d.Add("speedup"    , SPEEDUP      );
                                        d.Add("unswap"  , UNSWAP      );
                                        d.Add("updatefreq"              , UPDATEFREQ);
                                        d.Add("updx"    , UPDX       );
                                        d.Add("v"    , V    );
                                        d.Add("val"     , VAL    );
										d.Add("var"     , VAR    );
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
										d.Add("y2max",Y2MAX);
                                        d.Add("y2min",Y2MIN);
                                        d.Add("zero"    , ZERO      );
                                        d.Add("ZOOM", ZOOM);
                                        d.Add("ZVAR"    , ZVAR     );
										// --- tokens2 end ---
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


// ------------------------------------------------------------------------------------------------------------------
// ------------------- expression START -------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

expression:                 additiveExpression;

additiveExpression:         (multiplicativeExpression -> multiplicativeExpression)
							( (PLUS lbla=multiplicativeExpression -> ^(ASTPLUS $additiveExpression $lbla))
							| (MINUS lblb=multiplicativeExpression -> ^(ASTMINUS $additiveExpression  $lblb)) 
							| (doubleVerticalBar lblb=multiplicativeExpression -> ^(ASTLISTOR $additiveExpression  $lblb)) 
							| (DOUBLEAND lblb=multiplicativeExpression -> ^(ASTLISTAND $additiveExpression  $lblb)) 							
							)*;

multiplicativeExpression:   (powerExpression -> powerExpression)
						    ( (star lbla=powerExpression -> ^(ASTSTAR $multiplicativeExpression $lbla))
						    | (DIV lblb=powerExpression -> ^(ASTDIV $multiplicativeExpression  $lblb)) )*
						    ;

powerExpression:			(unaryExpression -> unaryExpression)
						    (pow lbla=unaryExpression -> ^(ASTPOWER $powerExpression $lbla))*	
						    ;

unaryExpression:            dollarExpression -> dollarExpression
					      | MINUS dollarExpression -> ^(ASTNEGATE dollarExpression)
						    ;						

dollarExpression:		    (indexerExpression -> indexerExpression)
						    (DOLLAR lbla=dollarConditional -> ^(ASTDOLLAR $dollarExpression $lbla))*	
						    ; 						

indexerExpression:          (primaryExpression -> primaryExpression)
						    (lbla=dotOrIndexer -> ^(ASTDOTORINDEXER $indexerExpression $lbla))*
						    ;

primaryExpression:          leftParen! expression RIGHTPAREN!
                          | value
						    ;


value:                      function //must be before varname
						  | bankvarname						
						  | Integer -> ^(ASTINTEGER Integer)
					//	  | indexerAlone
						  | double2 -> double2						
						  | date2 -> ^(ASTDATE2 date2) //a date like: 2001q3 (luckily we do not have 'e' freq, then what about 2012e3 (in principle, = 2012000))
						  | StringInQuotes -> ^(ASTSTRINGINQUOTES StringInQuotes)
					 	  | stringInQuotesWithCurlies
						  | listFile						
						  | leftBracketNoGlue wildRange RIGHTBRACKET -> ^(ASTINDEXERALONE wildRange) //also see rule indexerExpression
						  | leftCurlyNoGlue wildRange RIGHTCURLY ->   ^(ASTBANKVARNAME ASTPLACEHOLDER ^(ASTVARNAME ASTPLACEHOLDER ^(ASTPLACEHOLDER ^(ASTNAME ^(ASTCURLY ^(ASTINDEXERALONE wildRange  )  ) ) )ASTPLACEHOLDER))
						  | matrix
						  | list
						  | map						  
						    ;

stringInQuotesWithCurlies:  stringInQuotesWithCurliesA stringInQuotesWithCurliesB* stringInQuotesWithCurliesC -> ^(ASTSTRINGINQUOTESWITHCURLIES stringInQuotesWithCurliesA stringInQuotesWithCurliesB* stringInQuotesWithCurliesC);
stringInQuotesWithCurliesA: StringInQuotes1 expression;
stringInQuotesWithCurliesB: StringInQuotes2 expression;
stringInQuotesWithCurliesC: StringInQuotes3;


//wildRange:                  wildcardWithBank | rangeWithBank;					    
wildRange:                  expression doubleDot2 expression -> ^(ASTRANGEGENERAL expression expression);  //for some reason, [a .. b] does not get glue between the dots, therefore doubleDot2

leftSide:                   leftSideDollarExpression -> leftSideDollarExpression;

leftSideDollarExpression:   listFile 
                          | (bankvarnameIndexer -> bankvarnameIndexer) (DOLLAR lbla=dollarConditional -> ^(ASTDOLLAR $leftSideDollarExpression $lbla))*								
						    ; 						

bankvarnameIndexer:         (bankvarname -> bankvarname)
						    (lbla=dotOrIndexer -> ^(ASTDOTORINDEXER $bankvarnameIndexer $lbla))*
						    ;

// ------------------------------------------------------------------------------------------------------------------
// ------------------- expression END -------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------
	
//indexerAlone:			    (leftBracketNoGlue|leftBracketNoGlueWild) indexerExpressionHelper RIGHTBRACKET -> ^(ASTINDEXERALONE indexerExpressionHelper); //also see rule indexerExpression
						
dotOrIndexer:               GLUEDOT DOT dotHelper -> ^(ASTDOT dotHelper)			
						  | leftBracketGlue indexerExpressionHelper2 RIGHTBRACKET -> ^(ASTINDEXER indexerExpressionHelper2)
						    ;

						    //just like b1:fy!q, we can use #m.fy!q, where fy!q is the varname.
dotHelper:				    objectFunction | varname | integer;
indexerExpressionHelper2:   (indexerExpressionHelper (',' indexerExpressionHelper)*) -> indexerExpressionHelper+;

matrix:                     matrixCol;
matrixCol:                  leftBracketNoGlue matrixRow (SEMICOLON matrixRow)* RIGHTBRACKET -> ^(ASTMATRIXCOL matrixRow+);
matrixRow:                  expression (',' expression)*  -> ^(ASTMATRIXROW expression+);

						    //trailing ',' is allowed, for instance ('a', 'b', ). This is Python style: ('a',) will then be a lists, not just a.
list:                       leftParenNoGlue repExpression ',' listHelper RIGHTPAREN -> ^(ASTLISTDEF repExpression listHelper)
                          | leftParenNoGlue repExpression ',' RIGHTPAREN -> ^(ASTLISTDEF repExpression)
						    ;
listHelper:                 listHelper1 | listHelper2;
listHelper1:                (repExpression ',')* repExpression -> repExpression+;
listHelper2:                (repExpression ',')+ -> repExpression+;
repExpression:              expression (REP repN)? -> ^(ASTLISTDEFITEM expression repN?);
repN:						expression 
						  | star -> ASTREPSTAR
						    ;

repStar:					REP star;  //only used to allow y = 1 rep * without error

map:                        leftParenNoGlue mapItem ',' mapHelper RIGHTPAREN -> ^(ASTMAPDEF mapItem mapHelper)
                          | leftParenNoGlue mapItem ','? RIGHTPAREN -> ^(ASTMAPDEF mapItem)   //the comma here is optional, not so for a list def.
						    ;
mapHelper:                  mapHelper1 | mapHelper2;
mapHelper1:                 (mapItem ',')* mapItem -> mapItem+;
mapHelper2:                 (mapItem ',')+ -> mapItem+;
mapItem:                    assignmentMap2 -> ^(ASTMAPITEM assignmentMap2);

//listFile:                   HASH leftParenGlue LISTFILE name RIGHTPAREN -> ^(ASTLISTFILE name);
listFile:                   HASH leftParenGlue LISTFILE fileName RIGHTPAREN -> ^(ASTBANKVARNAME2 ASTPLACEHOLDER ^(ASTVARNAME ^(ASTPLACEHOLDER ASTHASH)  ^(ASTHANDLEFILENAME fileName) ASTPLACEHOLDER) );

function:                   ident leftParenGlue fargs RIGHTPAREN -> ^(ASTFUNCTION ident fargs);
objectFunction:             ident leftParenGlue fargs RIGHTPAREN -> ^(ASTOBJECTFUNCTION ident fargs);
specialArg: 			    ISNOTQUAL -> ^(ASTSPECIALARGS)						  					
						  | leftAngleNo2 dates? RIGHTANGLE -> ^(ASTSPECIALARGS dates?)
						    ;
fargs1:    					specialArg | expression;						    

fargs:                      -> ^(ASTSPECIALARGS)
						  | ISNOTQUAL -> ^(ASTSPECIALARGS)
						  | ISNOTQUAL (',' expression)* -> ^(ASTSPECIALARGS) expression*
						  | leftAngleNo2 dates? RIGHTANGLE (',' expression)* -> ^(ASTSPECIALARGS dates?) expression*
						  | expression (',' expression)* -> ^(ASTSPECIALARGS) expression+
						    ;

fargs_proc:                  -> ^(ASTSPECIALARGS)
						  | ISNOTQUAL -> ^(ASTSPECIALARGS)
						  | ISNOTQUAL expression* -> ^(ASTSPECIALARGS) expression*
						  | leftAngleNo2 dates? RIGHTANGLE expression* -> ^(ASTSPECIALARGS dates?) expression*
						  | expression+ -> ^(ASTSPECIALARGS) expression+
						    ;
					
dollarConditional:          LEFTPAREN logicalOr RIGHTPAREN -> ^(ASTDOLLARCONDITIONAL logicalOr)  //must use parentheses now, else stuff like y $ x = 100 is too confusing.
					//	  | bankvarnameindex -> ^(ASTDOLLARCONDITIONALVARIABLE bankvarnameindex)  //does not need parenthesis								
						    ;

//bankvarnameindex:           bankvarname ( leftBracketGlue expression RIGHTBRACKET ) -> ^({token("ASTCOMPARE2¤"+($expression.text)+"¤"+($expression.start)+"¤"+($expression.stop), ASTCOMPARE2, 0)} bankvarname expression);    //should catch #i0[#i] or #i0['a'], does not need a parenthesis!
					
indexerExpressionHelper:    function //else the stupid parser stumbles on f() etc.
						  | ident -> ^({token("ASTINDEXERELEMENTIDENT¤"+($ident.text)+"¤"+($ident.start)+"¤"+($ident.stop), ASTINDEXERELEMENTIDENT, 0)} ident)
						  | expressionOrNothing doubleDot expressionOrNothing -> ^(ASTINDEXERELEMENT expressionOrNothing expressionOrNothing)     //'fm1'..'fm5'
						  | expression -> ^({token("ASTINDEXERELEMENT¤"+($expression.text)+"¤"+($expression.start)+"¤"+($expression.stop), ASTINDEXERELEMENT, 0)} expression)                                     //'fm*' or -2 or 2000 or 2010q3
						  | PLUS expression -> ^(ASTINDEXERELEMENTPLUS expression)                            //+1
                            ;



expressionOrNothing:        expression -> expression
						  | -> ASTEMPTYRANGEELEMENT
						    ;

// ------------------------------------------------------------------------------------------------------------------
// ------------------- naked list, used in assignment and FOR -------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

nakedList:					  seqItemNaked (COMMA2 seqItemNaked)+ ->  ^(ASTNAKEDLIST seqItemNaked+)
                            | seqItemNaked COMMA2 -> ^(ASTNAKEDLIST seqItemNaked)							
							  ;

seqItemNaked:                 MINUS seqItem7Naked 	(REP repN)?	-> ^(ASTNAKEDLISTITEM ^(ASTSEQITEMMINUS seqItem7Naked) repN?)
							| seqItem7Naked	(REP repN)?	-> ^(ASTNAKEDLISTITEM seqItem7Naked repN?)
							| seqNumber (REP repN)? -> ^(ASTNAKEDLISTITEM seqNumber repN?)
							| ident leftParenGlue RIGHTPAREN -> ^(ASTNAKEDLISTMISS ident) //must catch m() or miss()
						      ;

seqItem7Naked:                bank7Naked? name7 indexer7Naked? -> ^(ASTSEQ7 ^(ASTPLACEHOLDER bank7Naked?) ^(ASTPLACEHOLDER name7) ^(ASTPLACEHOLDER indexer7Naked?));
bank7Naked:					  AT GLUE -> ASTAT
							| name7 COLON -> name7 ASTCOLON
							  ;

freq7Naked:				      GLUE EXCLAMATION GLUE name7 -> ASTEXCLAMATION name7;  
//indexer7Naked:			      leftBracket (name7 (',' name7)*) RIGHTBRACKET -> ^(ASTL0 name7+);
indexer7Naked:				  leftBracket (indexerHelper7 (',' indexerHelper7)*) RIGHTBRACKET -> ^(ASTL0 indexerHelper7+);

// ------------------------------------------------------------------------------------------------------------------
// ------------------- flexible list --------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

minus:                        MINUS -> ASTNUMBERMINUS;
seqNumber:                    minus? double2 -> ^(ASTNUMBER double2 minus?)
						    | minus? Integer -> ^(ASTNUMBER Integer minus?)
							  ;

// ----------------------------------------------------------------------------------

seqItem:                      MINUS seqItem7 -> ^(ASTSEQITEMMINUS seqItem7)
							| seqItem7 doubleDot2 seqItem7 -> ^(ASTRANGEWITHBANK seqItem7 seqItem7)
							| seqItem7							
						      ;

seqItem7:                     listFile
							| bank7? wildcard7 indexer7? -> ^(ASTSEQ7 ^(ASTPLACEHOLDER bank7?) ^(ASTPLACEHOLDER wildcard7) ^(ASTPLACEHOLDER indexer7?));

bank7:						  AT GLUE -> ASTAT
							| wildcard7 COLON -> wildcard7 ASTCOLON
							  ;

freq7:						  GLUE EXCLAMATION GLUE wildcard7 -> ASTEXCLAMATION wildcard7;  
indexer7:					  leftBracket (indexerHelper7 (',' indexerHelper7)*) RIGHTBRACKET -> ^(ASTL0 indexerHelper7+);  

indexerHelper7:				  
						      wildcardIndexer7 -> ^(ASTL1 wildcardIndexer7)	 //stuff like x[a], x[a*b]						 					
							| expression -> ^(ASTL1 expression)              //stuff like x['a'], x['a'+'b'], 
							  ;

name7:						  (identDigit | nameCurlyStart) (GLUE! identDigit | nameCurly)*;  //was: name, but name7 can be 117, 007, 1e10, 1a, etc.

wildcardIndexer7:  		      name7 (wildSymbolMiddle name7)* wildSymbolEnd? //a?b a?b?, a?b?c a?b?c?, etc.
						    | name7 wildSymbolEnd //a?						  	
						    | wildSymbolStart name7 (wildSymbolMiddle name7)* wildSymbolEnd? //?a ?a? ?a?b ?a?b?, etc.
						    | wildSymbolFree //?	
							  ;

wildcard7:         		      triplestars -> ASTTRIPLESTARS  //everything
							| stars -> ASTSTARS		         //everything in a bank
							| sigil? name7 (wildSymbolMiddle name7)* wildSymbolEnd? freq7? //a?b a?b?, a?b?c a?b?c?, etc.
						    | sigil? name7 wildSymbolEnd freq7? //a?						  	
						    | sigilNoGlue? wildSymbolStart name7 (wildSymbolMiddle name7)* wildSymbolEnd? freq7? //?a ?a? ?a?b ?a?b?, etc.
						    | sigilNoGlue? wildSymbolFree freq7? //?							
						      ;

seqOfBankvarnames:          seqItem (COMMA2 seqItem)* ->  ^(ASTBANKVARNAMELIST seqItem+);
seqOfBankvarnames2:         seqOfBankvarnames;  //alias
seqOfBankvarnamesOnly1:     seqItem -> ^(ASTBANKVARNAMELIST seqItem);

//accepts filenames without hyphens, but also strings (for instance 'text' or %s). So data.gbk or 'data.gbk' but not {'data.gbk'}.
//distinguishing between a or 'a' is not interesting here, as it is for seqOfBankvarnames
//accepts a, c:\a.b, '...', %s, #m.
seqOfFileNames:             fileName (COMMA2 fileName)* ->  ^(ASTFILENAMELIST fileName+);
seqOfFileNamesStar:         star -> ^(ASTFILENAMELIST ASTFILENAMESTAR)
						  | fileName (COMMA2 fileName)* ->  ^(ASTFILENAMELIST fileName+)
						    ;

// ------------------------------------------------------------------------------------------------------------------
// ------------------- wildcards, ranges --------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

listItemsWildRange:         listItemWildRange (COMMA2 listItemWildRange)* -> ^(ASTLISTITEMS (^(ASTLISTITEM listItemWildRange))+);   //puts in o.listItems

listItemWildRange:          rangeWithBank ->                           rangeWithBank							 
						  | wildcardWithBank ->                        wildcardWithBank
						  | expression ->						       expression
						  | identDigit  ->                             ^(ASTGENERIC1 identDigit)   //accepts stuff like 0e. Integers are caught via expression.												
						  ;

varnameOrWildcard:           wildcard | varname;

wildcardWithBank:           AT GLUE varnameOrWildcard -> ^(ASTWILDCARDWITHBANK ^(ASTNAME ^(ASTIDENT REF)) varnameOrWildcard)						  
                          | varnameOrWildcard COLON varnameOrWildcard -> ^(ASTWILDCARDWITHBANK varnameOrWildcard varnameOrWildcard)						  
						  | varnameOrWildcard -> ^(ASTWILDCARDWITHBANK ^(ASTPLACEHOLDER) varnameOrWildcard)
						  ;

rangeWithBank             : range -> ^(ASTRANGEWITHBANK range)						  ;

range:                      wildcardWithBank doubleDot2 wildcardWithBank -> wildcardWithBank wildcardWithBank;

wildcard:                   wildcard3 -> ^(ASTWILDCARD wildcard3);

wildcardFreq:			    GLUE EXCLAMATION GLUE wildcard2 -> ASTEXCLAMATION wildcard2;

wildcard3:                  sigil? wildcard2 wildcardFreq?; 

wildcard2:         		   
						    identDigit (wildSymbolMiddle identDigit)* wildSymbolEnd?  //a?b a?b?, a?b?c a?b?c?, etc.
						  | identDigit wildSymbolEnd  //a?						  	
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
						  						   

// ------------------------------------------------------------------------------------------------------------------
// ------------------- name START -------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

name:                       name2 -> ^(ASTNAME name2);

						    //name is without sigil, name is in principle just like characters, excluding sigils. Kind of like an advanced ident.
name2:                      (ident | nameCurlyStart) (GLUE! identDigit | nameCurly)* ;

nameCurlyStart:             
					        leftCurlyNoGlue expression RIGHTCURLY -> ^({token("ASTCURLY¤"+($expression.text)+"¤"+($expression.start)+"¤"+($expression.stop), ASTCURLY, 0)} expression)
						    ;

nameCurly:                  
					        leftCurlyGlue expression RIGHTCURLY -> ^({token("ASTCURLY¤"+($expression.text)+"¤"+($expression.start)+"¤"+($expression.stop), ASTCURLY, 0)} expression)
						    ;

cname:                      name cnameHelper+ -> ^(ASTCNAME name cnameHelper+);
cnameHelper:                GLUE sigil name -> ^(ASTCURLY ^(ASTBANKVARNAME ASTPLACEHOLDER  ^(ASTVARNAME ^(ASTPLACEHOLDER sigil) ^(ASTPLACEHOLDER  name )  ASTPLACEHOLDER   )     )  )                       
						  | GLUE VERTICALBAR name -> name //does not have glue after
						    ;

nameOrCname:                cname | name;  //cname must be before name

varname:                    nameOrCname freq? -> ^(ASTVARNAME ^(ASTPLACEHOLDER) ^(ASTPLACEHOLDER nameOrCname) ^(ASTPLACEHOLDER freq?))
						  | sigil name -> ^(ASTVARNAME ^(ASTPLACEHOLDER sigil) ^(ASTPLACEHOLDER name) ^(ASTPLACEHOLDER))
						  | sigil leftParen cname rightParen -> ^(ASTVARNAME ^(ASTPLACEHOLDER sigil) ^(ASTPLACEHOLDER cname) ^(ASTPLACEHOLDER))						  
						    ;

bankvarname:                bankColon? varname -> ^(ASTBANKVARNAME ^(ASTPLACEHOLDER bankColon?) varname);

bankColon:                  AT GLUE -> ^(ASTNAME ^(ASTIDENT REF))            
				          | nameOrCname COLON -> nameOrCname
						    ;

svarname:                   sigil? ident -> ^(ASTPLACEHOLDER ^(ASTPLACEHOLDER sigil?) ident);

//used for lists: LIST a, b, c instead of LIST 'a', 'b', 'c'
bankseriesname:             bankColon? seriesname -> ^(ASTBANKVARNAME ^(ASTPLACEHOLDER bankColon?) seriesname);
seriesname:                 nameOrCname freq? -> ^(ASTVARNAME ^(ASTPLACEHOLDER) ^(ASTPLACEHOLDER nameOrCname) ^(ASTPLACEHOLDER freq?));

// ------------------------------------------------------------------------------------------------------------------
// ------------------- name END -------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

sigil:                      HASH GLUE -> ASTHASH
						  | PERCENT GLUE -> ASTPERCENT
						    ;

sigilNoGlue:                HASH -> ASTHASH
						  | PERCENT -> ASTPERCENT
						    ;

sigilOrVertical:            sigil
						  | VERTICALBAR -> ASTVERTICALBAR  //does not have glue after
						    ;

freq:			   		   GLUE EXCLAMATION GLUE name -> name;

// ------------------------------------------------------------------------------------------------------------------
// ------------------- logical START -------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

logical:                    logicalOr;

logicalOr:				    (logicalAnd -> logicalAnd)
							(OR lbla=logicalAnd -> ^(ASTOR $logicalOr $lbla))*
						  ;

logicalAnd:				    (logicalNot -> logicalNot)
							(AND lbla=logicalNot -> ^(ASTAND $logicalAnd $lbla))*
						  ;

logicalNot:				    NOT logicalAtom     -> ^(ASTNOT logicalAtom)
						  | logicalAtom
						  ;

logicalAtom:				expression ifOperator expression -> ^(ASTCOMPARE ifOperator expression expression)
						  | leftParen! logicalOr rightParen!           // omit both '(' and ')'
//						  | bankvarnameindex			
//						  | expression IN expression -> ^(ASTIN expression expression)
						  | expression -> expression
						  ;

ifOperator:		            ISEQUAL -> ^(ASTIFOPERATOR ASTIFOPERATOR1)
						  | ISNOTQUAL -> ^(ASTIFOPERATOR ASTIFOPERATOR2)
						  | RIGHTANGLE -> ^(ASTIFOPERATOR ASTIFOPERATOR3)
						  | leftAngle -> ^(ASTIFOPERATOR ASTIFOPERATOR4)
			              | ISLARGEROREQUAL -> ^(ASTIFOPERATOR ASTIFOPERATOR5)
						  | ISSMALLEROREQUAL -> ^(ASTIFOPERATOR ASTIFOPERATOR6)
						  | IN -> ^(ASTIFOPERATOR ASTIFOPERATOR7)
			                ;

//                          Arguments of type f(), f(<2001 2002>), f(2), f(<2001 2002>, 2), ...

						  
/*------------------------------------------------------------------
 * PARSER RULES
 *------------------------------------------------------------------*/

start:                      statements EOF;  //EOF is necessary in order to force the whole file to be parsed

statements:                 statements2*;

statements2:                SEMICOLON -> //stray semicolon is ok, nothing is written                     
						  | assignment2          SEMICOLON!
						  | accept               SEMICOLON!
						  | analyze              SEMICOLON!		
						  | block                
						  | checkoff             SEMICOLON!	
			              | clear                SEMICOLON!		
						  | clone                SEMICOLON!
						  | close                SEMICOLON!
						  | cls                  SEMICOLON!
						  | collapse             SEMICOLON!
						  | compare              SEMICOLON!
						  | copy                 SEMICOLON!
						  | count                SEMICOLON!
						  | create               SEMICOLON!
						  | cut                  SEMICOLON!
						  | decomp               SEMICOLON!
						  | decomp2              SEMICOLON!
						  | eval                 SEMICOLON!
						  | delete               SEMICOLON!
						  | disp                 SEMICOLON!
						  | doc                  SEMICOLON!
						  | download             SEMICOLON!
						  | edit                 SEMICOLON!
						  | endo                 SEMICOLON!
						  | exo                  SEMICOLON!
						  | exit                 SEMICOLON!
						  | findmissingdata      SEMICOLON!
						  | for2
						  | functionDef          SEMICOLON!
						  | global               SEMICOLON!
						  | goto2                SEMICOLON!
						  | hdg                  SEMICOLON!
					      | help                 SEMICOLON!
						  | if2
						  | index                SEMICOLON!
						  | ini                  SEMICOLON!
						  | interpolate          SEMICOLON!
						  | itershow             SEMICOLON!	
						  | local                SEMICOLON!
						  | lock_                SEMICOLON!
						  | mem                  SEMICOLON!
						  | model                SEMICOLON!
		   			      | mode                 SEMICOLON!
						  | ols  				 SEMICOLON!
						  | open                 SEMICOLON!
						  | option				 SEMICOLON!
						  | pause                SEMICOLON!
						  | pipe				 SEMICOLON!
						  | sheetImport          SEMICOLON!  //maybe necessary that it is before print
						  | print                SEMICOLON!
						  | procedureDef         SEMICOLON!
						  | r_file               SEMICOLON!
						  | r_export             SEMICOLON!
						  | r_run                SEMICOLON!
						  | read                 SEMICOLON!
						  | rebase               SEMICOLON!
						  | rename               SEMICOLON!
						  | reset                SEMICOLON!
						  | restart              SEMICOLON!
						  | return2              SEMICOLON!
						  | run                  SEMICOLON!
						  | sign                 SEMICOLON!	
						  | sim                  SEMICOLON!	
						  | smooth               SEMICOLON!
						  | splice               SEMICOLON!
						  | stop                 SEMICOLON!
						  | sys                  SEMICOLON!
						  | table                SEMICOLON!
						  | target2              SEMICOLON!
						  | tell                 SEMICOLON!
						  | time                 SEMICOLON!
						  | timefilter           SEMICOLON!
						  | translate            SEMICOLON!
						  | truncate             SEMICOLON!
						  | unlock_              SEMICOLON!
						  | unfix                SEMICOLON!
						  | write                SEMICOLON!	
						  | x12a                 SEMICOLON!
						  | xedit                SEMICOLON!					  
						  | functionNaked        SEMICOLON!   //naked function outside expression
						  | objectFunctionNaked  SEMICOLON!   //naked object function outside expression
						  | procedure            SEMICOLON!   //procedure call
						    ;

//procedure: ident expression* -> ^(ASTPROCEDURE expression*);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// ASSIGNMENT, VAL, STRING, DATE, SERIES, LIST, MATRIX, MAP, VAR
// ---------------------------------------------------------------------------------------------------------------------------------------------------

assignment2:               assignment -> ^({token("ASTASSIGNMENT¤"+($assignment.text), ASTASSIGNMENT, input.LT(1).Line)} assignment);
assignmentMap2:            assignmentMap -> ^({token("ASTASSIGNMENT¤"+($assignmentMap.text), ASTASSIGNMENT, input.LT(1).Line)} assignmentMap);

//NOTE: ASTLEFTSIDE must always have ASTASSIGNMENT as parent, cf. #324683532
//NOTE: instead of expression, we could use prtElement, and get stuff in-substituted the same way. For now we do it more simple.

assignment:				    assignmentType seriesOpt1? leftSide EQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) nakedList ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTPLACEHOLDER 
						  | assignmentType seriesOpt1? leftSide EQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) expression ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTPLACEHOLDER
						  | assignmentType seriesOpt1? leftSide PLUSEQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) ^(ASTPLUS leftSide nakedList) ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTPLUS2
						  | assignmentType seriesOpt1? leftSide PLUSEQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) ^(ASTPLUS leftSide expression) ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTPLUS2
						  | assignmentType seriesOpt1? leftSide MINUSEQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) ^(ASTMINUS leftSide nakedList) ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTMINUS2   
						  | assignmentType seriesOpt1? leftSide MINUSEQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) ^(ASTMINUS leftSide expression) ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTMINUS2
						  | assignmentType seriesOpt1? leftSide STAREQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) ^(ASTSTAR leftSide nakedList) ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTSTAR2   
						  | assignmentType seriesOpt1? leftSide STAREQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) ^(ASTSTAR leftSide expression) ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTSTAR2
						  | assignmentType seriesOpt1? leftSide DIVEQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) ^(ASTDIV leftSide nakedList) ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTDIV2   
						  | assignmentType seriesOpt1? leftSide DIVEQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) ^(ASTDIV leftSide expression) ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTDIV2
						  | assignmentType seriesOpt1? leftSide percentEqual nakedList -> ^(ASTLEFTSIDE leftSide?) nakedList ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTPERCENT2
						  | assignmentType seriesOpt1? leftSide percentEqual expression repStar? -> ^(ASTLEFTSIDE leftSide?) expression ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTPERCENT2
						  | assignmentType seriesOpt1? leftSide HATEQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) nakedList ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTHAT2  
						  | assignmentType seriesOpt1? leftSide HATEQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) expression ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTHAT2
						  | assignmentType seriesOpt1? leftSide hashEqual nakedList -> ^(ASTLEFTSIDE leftSide?) nakedList ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTHASH2  
						  | assignmentType seriesOpt1? leftSide hashEqual expression repStar? -> ^(ASTLEFTSIDE leftSide?) expression ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTHASH2

						  
						  | assignmentType leftSide seriesOpt1 EQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) nakedList ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTPLACEHOLDER
						  | assignmentType leftSide seriesOpt1 EQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) expression ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTPLACEHOLDER
						  | assignmentType leftSide seriesOpt1 PLUSEQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) ^(ASTPLUS leftSide nakedList) ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTPLUS2
						  | assignmentType leftSide seriesOpt1 PLUSEQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) ^(ASTPLUS leftSide expression) ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTPLUS2
						  | assignmentType leftSide seriesOpt1 MINUSEQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) ^(ASTMINUS leftSide nakedList) ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTMINUS2  
						  | assignmentType leftSide seriesOpt1 MINUSEQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) ^(ASTMINUS leftSide expression) ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTMINUS2
						  | assignmentType leftSide seriesOpt1 STAREQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) ^(ASTSTAR leftSide nakedList) ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTSTAR2   
						  | assignmentType leftSide seriesOpt1 STAREQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) ^(ASTSTAR leftSide expression) ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTSTAR2
						  | assignmentType leftSide seriesOpt1 DIVEQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) ^(ASTDIV leftSide nakedList) ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTDIV2   
						  | assignmentType leftSide seriesOpt1 DIVEQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) ^(ASTDIV leftSide expression) ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTDIV2
						  | assignmentType leftSide seriesOpt1 percentEqual nakedList -> ^(ASTLEFTSIDE leftSide?) nakedList ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTPERCENT2   
						  | assignmentType leftSide seriesOpt1 percentEqual expression repStar? -> ^(ASTLEFTSIDE leftSide?) expression ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTPERCENT2
						  | assignmentType leftSide seriesOpt1 HATEQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) nakedList ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTHAT2   
						  | assignmentType leftSide seriesOpt1 HATEQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) expression ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTHAT2
						  | assignmentType leftSide seriesOpt1 hashEqual nakedList -> ^(ASTLEFTSIDE leftSide?) nakedList ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTHASH2  
						  | assignmentType leftSide seriesOpt1 hashEqual expression repStar? -> ^(ASTLEFTSIDE leftSide?) expression ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTHASH2
						  
						    //handle y<2001 2005>=100, where '>=' is a token, also with pch(y) etc. on left-hand side (naked list not allowed in that case)
						  | assignmentType leftSide seriesOpt1a ISLARGEROREQUAL nakedList -> ^(ASTLEFTSIDE leftSide?) nakedList ^(ASTPLACEHOLDER seriesOpt1a) assignmentType ASTPLACEHOLDER 
						  | assignmentType leftSide seriesOpt1a ISLARGEROREQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide?) expression ^(ASTPLACEHOLDER seriesOpt1a) assignmentType ASTPLACEHOLDER						  
						  | assignmentType ident leftParenGlue leftSide RIGHTPAREN seriesOpt1a ISLARGEROREQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide? ident) expression ^(ASTPLACEHOLDER seriesOpt1a) assignmentType ASTPLACEHOLDER
                            
							//handle dlog(x) = ... etc. Nakedlist not allowed here, only expression.
						  | assignmentType seriesOpt1? ident leftParenGlue leftSide RIGHTPAREN EQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide? ident) expression ^(ASTPLACEHOLDER seriesOpt1?) assignmentType ASTPLACEHOLDER						  
						  | assignmentType ident leftParenGlue leftSide RIGHTPAREN seriesOpt1 EQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide? ident) expression ^(ASTPLACEHOLDER seriesOpt1) assignmentType ASTPLACEHOLDER						  						  						  
						    ;

							//using += etc. will not be good in map def, too confusing. You can use #m.ts += 1 or dlog(#m.ts) = ... just fine which is enough.
assignmentMap:				assignmentType seriesOpt1? leftSide EQUAL expression repStar? -> ^(ASTLEFTSIDE leftSide) expression ^(ASTPLACEHOLDER seriesOpt1?) assignmentType				                         
						    ;

percentEqual : GLUE? PERCENTEQUAL;
hashEqual: GLUE? HASHEQUAL;


assignmentType:             SER 
					 	  | SERIES 
						  | STRING2 
						  | VAL 
						  | DATE 
						  | LIST 
						  | MAP 
						  | MATRIX 
						  | VAR 
						  | -> ASTPLACEHOLDER  //may be empty
						    ;

seriesOpt1a:                //so that we can handle y <2001 2005>=100; where '>=' is parsed as a token
						  | leftAngle2          seriesOpt1h* -> ^(ASTOPT1 seriesOpt1h*)
						  | leftAngleNo2 dates? seriesOpt1h* -> ^(ASTOPT1 ^(ASTDATES dates?) seriesOpt1h*)
                          ;

seriesOpt1:                 ISNOTQUAL
						  | leftAngle2          seriesOpt1h* RIGHTANGLE -> ^(ASTOPT1 seriesOpt1h*)
						  | leftAngleNo2 dates? seriesOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) seriesOpt1h*)
                          ;

seriesOpt1h:                D (EQUAL yesNo)? -> ^(ASTOPT_STRING_D yesNo?)
						  | P (EQUAL yesNo)? -> ^(ASTOPT_STRING_P yesNo?)
						  | M (EQUAL yesNo)? -> ^(ASTOPT_STRING_M yesNo?)
						  | Q (EQUAL yesNo)? -> ^(ASTOPT_STRING_Q yesNo?)
						  | MP (EQUAL yesNo)? -> ^(ASTOPT_STRING_MP yesNo?)						
						  | N (EQUAL yesNo)? -> ^(ASTOPT_STRING_N yesNo?)						
						  | DL (EQUAL yesNo)? -> ^(ASTOPT_STRING_DL yesNo?)	
						  | L (EQUAL yesNo)? -> ^(ASTOPT_STRING_L yesNo?)	
						  | KEEP EQUAL exportType -> ^(ASTOPT_STRING_KEEP exportType)						  
                          | ROWNAMES EQUAL expression -> ^(ASTOPT_LIST_ROWNAMES expression)
                          | COLNAMES EQUAL expression -> ^(ASTOPT_LIST_COLNAMES expression)
						  | LABEL EQUAL expression -> ^(ASTOPT_STRING_LABEL expression)
						  | SOURCE EQUAL expression -> ^(ASTOPT_STRING_SOURCE expression)
						  | STAMP EQUAL expression -> ^(ASTOPT_STRING_STAMP expression)							  
					      | UNITS EQUAL expression -> ^(ASTOPT_STRING_UNITS expression)							  				
						  | DYN (EQUAL yesNo)? -> ^(ASTOPT_STRING_DYN yesNo?)	
						  | MISSING EQUAL name -> ^(ASTOPT_STRING_MISSING name)
						  ;						  

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// ACCEPT
// ---------------------------------------------------------------------------------------------------------------------------------------------------

accept:                     ACCEPT acceptType varname expression -> ^({token("ASTACCEPT", ASTACCEPT, input.LT(1).Line)} acceptType varname expression);
acceptType:                 VAL | STRING2 | DATE;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// ANALYZE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

analyze:                    ANALYZE analyzeOpt1? expression -> ^({token("ASTANALYZE", ASTANALYZE, input.LT(1).Line)} analyzeOpt1? expression);
analyzeOpt1:                ISNOTQUAL
						  | leftAngle2          analyzeOpt1h* RIGHTANGLE -> ^(ASTOPT1 analyzeOpt1h*)							
						  | leftAngleNo2 dates? analyzeOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) analyzeOpt1h*)
						    ;
analyzeOpt1h:               LAG EQUAL expression -> ^(ASTOPT_VAL_LAG expression)
						    ;		
// ---------------------------------------------------------------------------------------------------------------------------------------------------
// CHECKOFF
// ---------------------------------------------------------------------------------------------------------------------------------------------------

checkoff:				    CHECKOFF seqOfBankvarnames -> ^({token("ASTCHECKOFF", ASTCHECKOFF, input.LT(1).Line)} seqOfBankvarnames)
						  | CHECKOFF '?' -> ^({token("ASTCHECKOFF", ASTCHECKOFF, input.LT(1).Line)} '?')
						  | CHECKOFF -> ^({token("ASTCHECKOFF", ASTCHECKOFF, input.LT(1).Line)})
						    ;
							
// ---------------------------------------------------------------------------------------------------------------------------------------------------
// CLEAR
// ---------------------------------------------------------------------------------------------------------------------------------------------------
							
clear:					    CLEAR clearOpt1? seqOfBankvarnames? -> ^({token("ASTCLEAR", ASTCLEAR, input.LT(1).Line)} ^(ASTPLACEHOLDER clearOpt1?) ^(ASTPLACEHOLDER seqOfBankvarnames?));
clearOpt1:				    ISNOTQUAL | leftAngle clearOpt1h* RIGHTANGLE -> ^(ASTOPT1 clearOpt1h*);
clearOpt1h:				    FIRST (EQUAL yesNo)? -> ^(ASTOPT_STRING_FIRST yesNo?)	
                          | REF (EQUAL yesNo)? -> ^(ASTOPT_STRING_REF yesNo?)
						    ;	

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// CLONE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

clone:                      CLONE -> ^({token("ASTCLONE", ASTCLONE, input.LT(1).Line)});		

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// CLS
// ---------------------------------------------------------------------------------------------------------------------------------------------------

cls:					    CLS -> ^({token("ASTCLS", ASTCLS, input.LT(1).Line)});

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// CLOSE
// ---------------------------------------------------------------------------------------------------------------------------------------------------
							
close:					    CLOSE closeOpt1? seqOfBankvarnames -> ^({token("ASTCLOSE", ASTCLOSE, input.LT(1).Line)} seqOfBankvarnames closeOpt1?)
						  | CLOSE closeOpt1? star -> ^({token("ASTCLOSESTAR", ASTCLOSESTAR, input.LT(1).Line)} closeOpt1?)
						    ;
closeOpt1:				    ISNOTQUAL | leftAngle closeOpt1h* RIGHTANGLE -> ^(ASTOPT1 closeOpt1h*);
closeOpt1h:				    SAVE (EQUAL yesNo)? -> ^(ASTOPT_STRING_SAVE yesNo?)							
						    ;		
												
// ---------------------------------------------------------------------------------------------------------------------------------------------------
// COLLAPSE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

collapse:				    COLLAPSE seqOfBankvarnames '=' seqOfBankvarnames collapseMethod? -> ^({token("ASTCOLLAPSE", ASTCOLLAPSE, input.LT(1).Line)} seqOfBankvarnames seqOfBankvarnames collapseMethod?);
collapseMethod:			    name;
//collapseMethod:			    FIRST|LAST|AVG|TOTAL;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// COMPARE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

compare:   				    COMPARE compareOpt1? seqOfBankvarnames? (FILE EQUAL fileName)? -> ^({token("ASTCOMPARECOMMAND", ASTCOMPARECOMMAND, input.LT(1).Line)} ^(ASTOPT_ compareOpt1?) ^(ASTPLACEHOLDER seqOfBankvarnames?) ^(ASTPLACEHOLDER fileName?));
compareOpt1:			    ISNOTQUAL
						  | leftAngle2          compareOpt1h* RIGHTANGLE -> ^(ASTOPT1 compareOpt1h*)							
						  | leftAngleNo2 dates? compareOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) compareOpt1h*)
                            ;
compareOpt1h:				ABS EQUAL expression -> ^(ASTOPT_VAL_ABS expression)
						  | DUMP (EQUAL yesNo)? -> ^(ASTOPT_STRING_DUMP yesNo?)						  
						  | INFO (EQUAL yesNo)? -> ^(ASTOPT_STRING_INFO yesNo?)
						  | REL EQUAL expression -> ^(ASTOPT_VAL_REL expression)
						  | SORT EQUAL name -> ^(ASTOPT_STRING_SORT name?)  //alpha, rel, abs
						  | PCH EQUAL expression -> ^(ASTOPT_VAL_PCH expression)
						    ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// COPY
// ---------------------------------------------------------------------------------------------------------------------------------------------------

copy:                       COPY copyOpt1? assignmentType seqOfBankvarnames (asOrTo seqOfBankvarnames)? -> ^({token("ASTCOPY", ASTCOPY, input.LT(1).Line)} ^(ASTPLACEHOLDER assignmentType) ^(ASTPLACEHOLDER ^(ASTOPT_ copyOpt1?)) seqOfBankvarnames seqOfBankvarnames?);
copyOpt1                  : ISNOTQUAL
						  | leftAngle2          copyOpt1h* RIGHTANGLE -> ^(ASTOPT1 copyOpt1h*)		
						  | leftAngleNo2 dates? copyOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES_TYPE2 dates?) copyOpt1h*)
						  ;
copyOpt1h                 : RESPECT (EQUAL yesNo)? -> ^(ASTOPT_STRING_RESPECT yesNo?)
						  | ERROR (EQUAL yesNo)? -> ^(ASTOPT_STRING_ERROR yesNo?)
						  | FROMBANK EQUAL name -> ^(ASTOPT_STRING_FROMBANK name)						  
						  | asOrToBank EQUAL name -> ^(ASTOPT_STRING_TOBANK name)
						  | BANK EQUAL name -> ^(ASTOPT_STRING_BANK name)
						  | PRINT (EQUAL yesNo)? -> ^(ASTOPT_STRING_PRINT yesNo?)
					//	  | TYPE EQUAL name -> ^(ASTOPT_STRING_TYPE name)
						  ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// COUNT
// ---------------------------------------------------------------------------------------------------------------------------------------------------

count:                      COUNT countOpt1? assignmentType seqOfBankvarnames2 -> ^({token("ASTCOUNT", ASTCOUNT, input.LT(1).Line)} ^(ASTPLACEHOLDER countOpt1?) ASTPLACEHOLDER ^(ASTPLACEHOLDER assignmentType) seqOfBankvarnames2);
countOpt1:                  ISNOTQUAL | leftAngle countOpt1h* RIGHTANGLE -> ^(ASTOPT1 countOpt1h*);							
countOpt1h:                 BANK EQUAL name -> ^(ASTOPT_STRING_BANK name)  //name can be without quotes											  
						    ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// CREATE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

create:					    CREATE seqOfBankvarnames -> ^({token("ASTCREATE", ASTCREATE, input.LT(1).Line)} ^(ASTPLACEHOLDER seqOfBankvarnames));

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// CUT
// ---------------------------------------------------------------------------------------------------------------------------------------------------

cut:					    CUT -> ^({token("ASTCUT", ASTCUT, input.LT(1).Line)});

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// DECOMP
// ---------------------------------------------------------------------------------------------------------------------------------------------------

decomp:						
DECOMP decompOpt1? seqOfBankvarnames -> ^({token("ASTDECOMP¤"+($seqOfBankvarnames.text), ASTDECOMP, input.LT(1).Line)} ^(ASTOPT_ decompOpt1?) ^(ASTDECOMPITEMS2 seqOfBankvarnames))
| DECOMP decompOpt1? decompExpression -> ^({token("ASTDECOMP¤"+($decompExpression.text), ASTDECOMP, input.LT(1).Line)} ^(ASTOPT_ decompOpt1?) ^(ASTDECOMPITEMS decompExpression))
;
decompExpression:           expression;

decomp2:               	  	DECOMP2 decompOpt1? decompVar1 decompWhere decompGroup decompLink decompCols         -> ^({token("ASTDECOMP2¤"+($decompVar1.text), ASTDECOMP2, input.LT(1).Line)} ^(ASTOPT_ decompOpt1?)  decompVar1 decompWhere decompGroup decompLink)
                       	  |	DECOMP2 decompOpt1? decompVar2 decompWhere decompGroup decompLink decompCols         -> ^({token("ASTDECOMP2¤"+($decompVar2.text), ASTDECOMP2, input.LT(1).Line)} ^(ASTOPT_ decompOpt1?)  decompVar2 decompWhere decompGroup decompLink)
						    ;

seqOfBankvarnamesOnly1Alias: seqOfBankvarnamesOnly1;

decompVar1:                 (seqOfBankvarnamesOnly1Alias IN)? seqOfBankvarnamesOnly1  -> ^(ASTDECOMPITEMSNAME ^(ASTPLACEHOLDER seqOfBankvarnamesOnly1Alias?) ^(ASTPLACEHOLDER seqOfBankvarnamesOnly1));
decompVar2:                 (seqOfBankvarnamesOnly1Alias IN)? decompExpression2       -> ^(ASTDECOMPITEMSEXPR ^(ASTPLACEHOLDER seqOfBankvarnamesOnly1Alias?) ^(ASTPLACEHOLDER decompExpression2));

decompExpression2:          (expression EQUAL)? expression -> expression+;

decompWhere:                WHERE decompWhere2 (COMMA2 decompWhere2)*-> ^(ASTDECOMPWHERE decompWhere2+)
						  | -> ASTDECOMPWHERE
							;
decompGroup:                GROUP decompGroup1 (COMMA2 decompGroup1)* -> ^(ASTDECOMPGROUP decompGroup1+)
					      | -> ASTDECOMPGROUP
							;
decompLink:                 LINK decompLink1 (COMMA2 decompLink1)* -> ^(ASTDECOMPLINK decompLink1+) 
						  | -> ASTDECOMPLINK
						    ;
decompCols:                 COLS seqOfBankvarnames -> ^(ASTDECOMPCOLS seqOfBankvarnames)
                          | -> ASTDECOMPCOLS
							;

decompWhere2:               decompWhere1 IN seqOfBankvarnamesOnly1 -> ^(ASTDECOMPWHERE2 decompWhere1 seqOfBankvarnamesOnly1);
decompWhere1:               seqOfBankvarnamesOnly1 | expression;

decompGroup1:               seqOfBankvarnamesOnly1 AS seqOfBankvarnamesOnly1 (LEVEL expression)? (ZOOM expression2)? -> ^(ASTDECOMPGROUP1 seqOfBankvarnamesOnly1 seqOfBankvarnamesOnly1 ^(ASTDECOMPGROUP1c expression?) ^(ASTDECOMPGROUP1d expression2?));

decompLink1:                decompVar1 -> ^(ASTDECOMPLINK1 decompVar1)
                          | decompVar2 -> ^(ASTDECOMPLINK1 decompVar2)
						    ;

decompOpt1:					ISNOTQUAL
						  | leftAngle2          decompOpt1h* RIGHTANGLE -> ^(ASTOPT1 decompOpt1h*)							
						  | leftAngleNo2 dates? decompOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) decompOpt1h*)
                            ;
decompOpt1h:				name -> ^(ASTOPT_STRING_PRTCODE name);

eval:						EVAL expression -> ^({token("ASTEVAL¤"+($expression.text), ASTEVAL, input.LT(1).Line)} expression);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// DELETE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

delete:						DELETE deleteOpt1? seqOfBankvarnames? -> ^({token("ASTDELETE", ASTDELETE, input.LT(1).Line)} ^(ASTOPT_ deleteOpt1?) ^(ASTPLACEHOLDER seqOfBankvarnames?));

deleteOpt1:					ISNOTQUAL
						  | leftAngle          deleteOpt1h* RIGHTANGLE -> ^(ASTOPT1 deleteOpt1h*)
                            ;
deleteOpt1h:			    NONMODEL (EQUAL yesNo)? -> ^(ASTOPT_STRING_NONMODEL yesNo?);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// DISP
// ---------------------------------------------------------------------------------------------------------------------------------------------------

disp:						//DISP stringInQuotes -> ^({token("ASTDISPSEARCH", ASTDISPSEARCH, input.LT(1).Line)} stringInQuotes)
						    DISP dispOpt1? assignmentType seqOfBankvarnames -> ^({token("ASTDISP", ASTDISP, input.LT(1).Line)} ^(ASTOPT_ dispOpt1?) ^(ASTPLACEHOLDER assignmentType) seqOfBankvarnames) //varnameslist				
						    ;

dispOpt1:					ISNOTQUAL
						  | leftAngle2          dispOpt1h* RIGHTANGLE -> ^(ASTOPT1 dispOpt1h*)							
						  | leftAngleNo2 dates? dispOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) dispOpt1h*)
                            ;
dispOpt1h:				    INFO (EQUAL yesNo)? -> ^(ASTOPT_STRING_INFO yesNo?);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// DOC
// ---------------------------------------------------------------------------------------------------------------------------------------------------

doc:                        DOC docOpt1? seqOfBankvarnames? docOpt2 -> ^({token("ASTDOC", ASTDOC, input.LT(1).Line)} ^(ASTOPT_ docOpt1?) ^(ASTPLACEHOLDER seqOfBankvarnames?) ^(ASTOPT_ docOpt2?))
						    ;
docOpt1:					ISNOTQUAL 
						  | leftAngle docOpt1h* RIGHTANGLE -> ^(ASTOPT1 docOpt1h*)						  
                            ;
docOpt1h:				    BROWSER (EQUAL yesNo)? -> ^(ASTOPT_STRING_BROWSER yesNo?);
docOpt2:                    docOpt2h*;
docOpt2h:                   LABEL EQUAL expression -> ^(ASTOPT_STRING_LABEL expression)
						  | SOURCE EQUAL expression -> ^(ASTOPT_STRING_SOURCE expression)
						  | STAMP EQUAL expression -> ^(ASTOPT_STRING_STAMP expression)							  
					      | UNITS EQUAL expression -> ^(ASTOPT_STRING_UNITS expression)							  				
						    ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// DOWNLOAD
// ---------------------------------------------------------------------------------------------------------------------------------------------------

download:                   DOWNLOAD downloadOpt1? url fileName (DUMP '=' fileName)* -> ^({token("ASTDOWNLOAD", ASTDOWNLOAD, input.LT(1).Line)} url ^(ASTHANDLEFILENAME fileName) ^(ASTHANDLEFILENAME2 fileName?) downloadOpt1?);
downloadOpt1:               ISNOTQUAL | leftAngle downloadOpt1h* RIGHTANGLE -> ^(ASTOPT1 downloadOpt1h*);							
downloadOpt1h:              ARRAY (EQUAL yesNo)? -> ^(ASTOPT_STRING_ARRAY yesNo?)	
						    ;

url:                        expression;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// EDIT
// ---------------------------------------------------------------------------------------------------------------------------------------------------

edit:                       EDIT fileNameStar -> ^({token("ASTEDIT", ASTEDIT, input.LT(1).Line)} ^(ASTHANDLEFILENAME fileNameStar));

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// ENDO/EXO
// ---------------------------------------------------------------------------------------------------------------------------------------------------

endo:                       ENDO eeOpt1? eeHelper -> ^({token("ASTENDO", ASTENDO, input.LT(1).Line)} ^(ASTPLACEHOLDER eeOpt1?) eeHelper);
exo:                        EXO eeOpt1? eeHelper -> ^({token("ASTEXO", ASTEXO, input.LT(1).Line)} ^(ASTPLACEHOLDER eeOpt1?) eeHelper);

eeOpt1:				     ISNOTQUAL
						 | leftAngleNo2 dates? RIGHTANGLE -> ^(ASTDATES2 dates?);

eeHelper:              eeHelper2 (COMMA2 eeHelper2)* -> eeHelper2+;
eeHelper2:             indexerExpression eeOpt1?  -> ^(ASTPLACEHOLDER indexerExpression eeOpt1?);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// EXIT
// ---------------------------------------------------------------------------------------------------------------------------------------------------

exit:					    EXIT -> ^({token("ASTEXIT", ASTEXIT, input.LT(1).Line)});

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// FINDMISSINGDATA
// ---------------------------------------------------------------------------------------------------------------------------------------------------


findmissingdata:			FINDMISSINGDATA findmissingdataOpt1? seqOfBankvarnames? -> ^({token("ASTFINDMISSINGDATA", ASTFINDMISSINGDATA, input.LT(1).Line)} ^(ASTPLACEHOLDER findmissingdataOpt1?) ^(ASTPLACEHOLDER seqOfBankvarnames?));
findmissingdataOpt1:        ISNOTQUAL | leftAngle2          findmissingdataOpt1h* RIGHTANGLE -> ^(ASTOPT1 findmissingdataOpt1h*)							
						  | leftAngleNo2 dates? findmissingdataOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) findmissingdataOpt1h*)
                            ;
findmissingdataOpt1h:       REPLACE EQUAL expression -> ^(ASTOPT_VAL_REPLACE expression);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// FOR
// ---------------------------------------------------------------------------------------------------------------------------------------------------

for2:                       FOR           (forHelper2 ','?)+     SEMICOLON  functionStatements END -> ^({token("ASTFOR", ASTFOR, input.LT(1).Line)} ^(ASTPLACEHOLDER forHelper2+) functionStatements)
						  | FOR leftParen (forHelper2 ','?)+ ')' SEMICOLON? functionStatements END -> ^({token("ASTFOR", ASTFOR, input.LT(1).Line)} ^(ASTPLACEHOLDER forHelper2+) functionStatements)
						    ;

forHelper2:                 forLhs expression TO expression2 (BY expression3)? -> ^(ASTFORTYPE1 forLhs ^(ASTPLACEHOLDER expression) ^(ASTPLACEHOLDER expression2) ^(ASTPLACEHOLDER expression3?))
                          | forLhs nakedList -> ^(ASTFORTYPE2 forLhs ^(ASTPLACEHOLDER nakedList) ^(ASTPLACEHOLDER) ^(ASTPLACEHOLDER))
                          | forLhs expression -> ^(ASTFORTYPE2 forLhs ^(ASTPLACEHOLDER expression) ^(ASTPLACEHOLDER) ^(ASTPLACEHOLDER))
                            ;

forLhs:                         type svarname EQUAL -> ^(ASTPLACEHOLDER type) ^(ASTPLACEHOLDER svarname);
                          
// ---------------------------------------------------------------------------------------------------------------------------------------------------
// FUNCTION
// ---------------------------------------------------------------------------------------------------------------------------------------------------

functionDef:				FUNCTION typeRv ident leftParenGlue functionArg RIGHTPAREN SEMICOLON functionStatements END -> ^({token("ASTFUNCTIONDEF2", ASTFUNCTIONDEF2, input.LT(1).Line)} typeRv ident functionArg functionStatements);
functionArg:                (functionArgElement1? (',' functionArgElement)*)? tripleDot? -> ^(ASTPLACEHOLDER functionArgElement1? functionArgElement* tripleDot?);						  
functionArgElement1:        functionArgTime | functionArgElement;
functionArgTime:            leftAngleNo2 functionArgElement ',' functionArgElement RIGHTANGLE -> ^(ASTSPECIALARGSDEF functionArgElement functionArgElement);
functionArgElement:         typeArg svarname -> ^(ASTPLACEHOLDER typeArg svarname);
functionStatements:         statements2* -> ^(ASTFUNCTIONDEFCODE statements2*);
functionStatements2:        functionStatements;  //alias in IF 
typeRv: 				    VAL | STRING2 | DATE | SERIES | LIST | MAP | MATRIX | VOID;
typeArg:				    VAL | STRING2 | DATE | SERIES | LIST | MAP | MATRIX | NAME;
type:					    VAL | STRING2 | DATE | SERIES | LIST | MAP | MATRIX;

objectFunctionNaked:        bankvarname GLUEDOT DOT ident leftParenGlue fargs RIGHTPAREN -> ^(ASTDOTORINDEXER bankvarname ^(ASTDOT ^(ASTOBJECTFUNCTIONNAKED  ident fargs)));


// ---------------------------------------------------------------------------------------------------------------------------------------------------
// GOTO
// ---------------------------------------------------------------------------------------------------------------------------------------------------

goto2:                      GOTO ident -> ^({token("ASTGOTO", ASTGOTO, input.LT(1).Line)} ident);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// HDG
// ---------------------------------------------------------------------------------------------------------------------------------------------------

hdg:						HDG expression -> ^({token("ASTHDG", ASTHDG, input.LT(1).Line)} expression);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// HELP
// ---------------------------------------------------------------------------------------------------------------------------------------------------

help:					    HELP  name? -> ^({token("ASTHELP", ASTHELP, input.LT(1).Line)} name?);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// IF
// ---------------------------------------------------------------------------------------------------------------------------------------------------

if2:						IF leftParen logical rightParen functionStatements (ELSE functionStatements2)? END SEMICOLON -> ^({token("ASTIF", ASTIF, input.LT(1).Line)} logical ^(ASTIFSTATEMENTS functionStatements) ^(ASTELSESTATEMENTS functionStatements2?));

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// BLOCK
// ---------------------------------------------------------------------------------------------------------------------------------------------------

block:						BLOCK blockOpt1 SEMICOLON functionStatements END SEMICOLON -> ^({token("ASTBLOCK", ASTBLOCK, input.LT(1).Line)} ^(ASTPLACEHOLDER blockOpt1?) ^(ASTPLACEHOLDER functionStatements));

blockOpt1:                  (TIME dates COMMA2) (blockOpt1h (COMMA2 blockOpt1h)*)? -> ^(ASTDATES_BLOCK dates) blockOpt1h*
						  | (TIME dates) -> ^(ASTDATES_BLOCK dates?)
						  | (blockOpt1h (COMMA2 blockOpt1h)*)? -> ^(ASTDATES_BLOCK) blockOpt1h*
						    ;

blockOpt1h:                 SERIES DYN '='? yesNoSimple -> ^(ASTBLOCKOPTION SERIES DYN ^(ASTBOOL yesNoSimple))
						  | optionType -> ^(ASTBLOCKOPTION optionType)														
							;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// INDEX
// ---------------------------------------------------------------------------------------------------------------------------------------------------

index:                      INDEX indexOpt1? assignmentType seqOfBankvarnames2 (TO seqOfBankvarnames)?  -> ^({token("ASTINDEX", ASTINDEX, input.LT(1).Line)} ^(ASTPLACEHOLDER indexOpt1?) ^(ASTPLACEHOLDER seqOfBankvarnames?) ^(ASTPLACEHOLDER assignmentType) seqOfBankvarnames2);
indexOpt1:                  ISNOTQUAL | leftAngle indexOpt1h* RIGHTANGLE -> ^(ASTOPT1 indexOpt1h*);							
indexOpt1h:                 MUTE (EQUAL yesNo)? -> ^(ASTOPT_STRING_MUTE yesNo?)							  
						  | BANK EQUAL name -> ^(ASTOPT_STRING_BANK name)  //name can be without quotes					
						  |	SHOWBANK EQUAL name -> ^(ASTOPT_STRING_SHOWBANK name)	//yes|no|all
						  |	SHOWFREQ EQUAL name -> ^(ASTOPT_STRING_SHOWFREQ name)	//yes|no|all
						    ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// INI
// ---------------------------------------------------------------------------------------------------------------------------------------------------

ini:					    INI -> ^({token("ASTINI", ASTINI, input.LT(1).Line)});

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// INTERPOLATE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

interpolate:				INTERPOLATE seqOfBankvarnames '=' seqOfBankvarnames interpolateMethod? -> ^({token("ASTINTERPOLATE", ASTINTERPOLATE, input.LT(1).Line)} seqOfBankvarnames seqOfBankvarnames interpolateMethod?);
interpolateMethod:			name;
//interpolateMethod:			REPEAT | PRORATE;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// ITERSHOW
// ---------------------------------------------------------------------------------------------------------------------------------------------------

itershow:				     ITERSHOW  (leftAngle dates? RIGHTANGLE)? seqOfBankvarnames -> ^({token("ASTITERSHOW", ASTITERSHOW, input.LT(1).Line)} ^(ASTDATES dates?) seqOfBankvarnames);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// LOCAL/GLOBAL
// ---------------------------------------------------------------------------------------------------------------------------------------------------

local:					    LOCAL localOpt1? seqOfBankvarnames? -> ^({token("ASTLOCAL", ASTLOCAL, input.LT(1).Line)} ^(ASTPLACEHOLDER localOpt1?) ^(ASTPLACEHOLDER seqOfBankvarnames?));
localOpt1:				    ISNOTQUAL | leftAngle localOpt1h* RIGHTANGLE -> ^(ASTOPT1 localOpt1h*);
localOpt1h:				    ALL (EQUAL yesNo)? -> ^(ASTOPT_STRING_ALL yesNo?)	
						    ;	

global:					    GLOBAL globalOpt1? seqOfBankvarnames? -> ^({token("ASTGLOBAL", ASTGLOBAL, input.LT(1).Line)} ^(ASTPLACEHOLDER globalOpt1?) ^(ASTPLACEHOLDER seqOfBankvarnames?));
globalOpt1:				    ISNOTQUAL | leftAngle globalOpt1h* RIGHTANGLE -> ^(ASTOPT1 globalOpt1h*);
globalOpt1h:				ALL (EQUAL yesNo)? -> ^(ASTOPT_STRING_ALL yesNo?)	
						    ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// LOCK, UNLOCK
// ---------------------------------------------------------------------------------------------------------------------------------------------------

lock_:                      LOCK_ name -> ^({token("ASTLOCK", ASTLOCK, input.LT(1).Line)} name);
unlock_:                    UNLOCK_ name -> ^({token("ASTUNLOCK", ASTUNLOCK, input.LT(1).Line)} name);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// MEM
// ---------------------------------------------------------------------------------------------------------------------------------------------------

mem:                        MEM -> ^({token("ASTMEM", ASTMEM, input.LT(1).Line)});

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// MODE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

mode:                       MODE mode2 -> ^({token("ASTMODE", ASTMODE, input.LT(1).Line)} mode2)
                          | MODE question -> ^({token("ASTMODEQUESTION", ASTMODEQUESTION, input.LT(1).Line)})	
						    ;	
mode2:                      MIXED | SIM | DATA;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// MODEL
// ---------------------------------------------------------------------------------------------------------------------------------------------------

model:                      MODEL modelOpt1? fileNameStar -> ^({token("ASTMODEL", ASTMODEL, input.LT(1).Line)} ^(ASTHANDLEFILENAME fileNameStar) modelOpt1?);
modelOpt1:                  ISNOTQUAL | leftAngle modelOpt1h* RIGHTANGLE -> modelOpt1h*;
modelOpt1h:                 INFO (EQUAL yesNo)? -> ^(ASTOPT_STRING_INFO yesNo?)
						  |	GMS (EQUAL yesNo)? -> ^(ASTOPT_STRING_GMS yesNo?)
						  |	DEP EQUAL expression -> ^(ASTOPT_VAR_DEP expression)
						  |	DUMP (EQUAL yesNo)? -> ^(ASTOPT_STRING_DUMP yesNo?)
						    ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// OLS
// ---------------------------------------------------------------------------------------------------------------------------------------------------

ols:                        OLS olsOpt1? olsExpression EQUAL olsExpression (',' olsExpression)* olsImpose? -> ^({token("ASTOLS", ASTOLS, input.LT(1).Line)} ^(ASTOPT_ olsOpt1?) ^(ASTNAMEHELPER) ^(ASTPLACEHOLDER olsImpose?) olsExpression*)						  
                          | OLS olsOpt1? name? olsExpression EQUAL olsExpression (',' olsExpression)* olsImpose? -> ^({token("ASTOLS", ASTOLS, input.LT(1).Line)} ^(ASTOPT_ olsOpt1?) ^(ASTNAMEHELPER name?) ^(ASTPLACEHOLDER olsImpose?) olsExpression*)						  
						    ;

olsExpression:				expression -> ^({token("ASTOLSEXPRESSION¤"+($expression.text), ASTOLSEXPRESSION, 0)} expression);

olsImpose:                  IMPOSE EQUAL expression -> ^(ASTIMPOSE expression?);
olsOpt1:                    ISNOTQUAL | leftAngle olsOpt1h* RIGHTANGLE -> olsOpt1h*;
olsOpt1h:                   dates -> ^(ASTDATES dates)
						  | CONSTANT (EQUAL yesNo)? -> ^(ASTOPT_STRING_CONSTANT yesNo?)
						    ;
							
// ---------------------------------------------------------------------------------------------------------------------------------------------------
// OPEN
// ---------------------------------------------------------------------------------------------------------------------------------------------------

open:                       OPEN openOpt1? openHelper (COMMA2 openHelper)* -> ^({token("ASTOPEN", ASTOPEN, input.LT(1).Line)} openOpt1? openHelper+);
openHelper:                 seqOfFileNamesStar (AS seqOfBankvarnames)? -> ^(ASTOPENHELPER ^(ASTFILENAME seqOfFileNamesStar) ^(ASTAS seqOfBankvarnames?));
openOpt1:                   ISNOTQUAL | leftAngle openOpt1h* RIGHTANGLE -> openOpt1h*;
openOpt1h:                  TSD (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSD yesNo?)
						  | TSDX (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSDX yesNo?)
						  | GBK (EQUAL yesNo)? -> ^(ASTOPT_STRING_GBK yesNo?)
						  | GDX (EQUAL yesNo)? -> ^(ASTOPT_STRING_GDX yesNo?)
						  | GDXOPT EQUAL expression -> ^(ASTOPT_STRING_GDXOPT expression)
						  | PCIM (EQUAL yesNo)? -> ^(ASTOPT_STRING_PCIM yesNo?)
						  | CSV (EQUAL yesNo)? -> ^(ASTOPT_STRING_CSV yesNo?)
						  | PRN (EQUAL yesNo)? -> ^(ASTOPT_STRING_PRN yesNo?)	
						  | PX (EQUAL yesNo)? -> ^(ASTOPT_STRING_PX yesNo?)					
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
						  | CREATE (EQUAL yesNo)? -> ^(ASTOPT_STRING_CREATE yesNo?)
						    ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// OPTION
// ---------------------------------------------------------------------------------------------------------------------------------------------------

option:                     OPTION optionType -> ^({token("ASTOPTION", ASTOPTION, input.LT(1).Line)} optionType);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// PAUSE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

pause:                      PAUSE expression? -> ^({token("ASTPAUSE", ASTPAUSE, input.LT(1).Line)} expression?);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// PIPE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

pipe:                       PIPE pipeOpt1? fileName? ->^({token("ASTPIPE", ASTPIPE, input.LT(1).Line)} pipeOpt1? ^(ASTHANDLEFILENAME fileName?));
pipeOpt1:                   ISNOTQUAL | leftAngle pipeOpt1h* RIGHTANGLE -> pipeOpt1h*;
pipeOpt1h:                  HTML (EQUAL yesNo)? -> ^(ASTOPT_STRING_HTML yesNo?)
						  | APPEND (EQUAL yesNo)? -> ^(ASTOPT_STRING_APPEND yesNo?)	
						  | PAUSE (EQUAL yesNo)? -> ^(ASTOPT_STRING_PAUSE yesNo?)						
						  | CONTINUE (EQUAL yesNo)? -> ^(ASTOPT_STRING_CONTINUE yesNo?)						
						  | STOP (EQUAL yesNo)? -> ^(ASTOPT_STRING_STOP yesNo?)											
						    ;
// ---------------------------------------------------------------------------------------------------------------------------------------------------
// PROCEDURE CALL
// ---------------------------------------------------------------------------------------------------------------------------------------------------

procedure:					identWithoutCommand fargs_proc -> ^({token("ASTPROCEDURE", ASTPROCEDURE, input.LT(1).Line)} identWithoutCommand fargs_proc);  
functionNaked:              ident leftParenGlue fargs RIGHTPAREN -> ^({token("ASTFUNCTIONNAKED", ASTFUNCTIONNAKED, input.LT(1).Line)} ident fargs);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// PROCEDURE DEFINITION
// ---------------------------------------------------------------------------------------------------------------------------------------------------

//procedureDef:				PROCEDURE identWithoutCommand procedureArg SEMICOLON procedureStatements END -> ^({token("ASTPROCEDUREDEF", ASTPROCEDUREDEF, input.LT(1).Line)} ASTPLACEHOLDER identWithoutCommand procedureArg procedureStatements);
//procedureArg:               (procedureArgElement? (',' procedureArgElement)*)? -> ^(ASTPLACEHOLDER procedureArgElement*);
//procedureArgElement:        typeArg svarname -> ^(ASTPLACEHOLDER typeArg svarname);
//procedureStatements:        statements2* -> ^(ASTPROCEDUREDEFCODE statements2*);

procedureDef:				PROCEDURE identWithoutCommand procedureArg SEMICOLON procedureStatements END -> ^({token("ASTPROCEDUREDEF", ASTPROCEDUREDEF, input.LT(1).Line)} ASTPLACEHOLDER identWithoutCommand procedureArg procedureStatements);
procedureArg:                (procedureArgElement1? (',' procedureArgElement)*)? tripleDot? -> ^(ASTPLACEHOLDER procedureArgElement1? procedureArgElement* tripleDot?);						  
procedureArgElement1:        procedureArgTime | procedureArgElement;
procedureArgTime:            leftAngleNo2 procedureArgElement ',' procedureArgElement RIGHTANGLE -> ^(ASTSPECIALARGSDEF procedureArgElement procedureArgElement);
procedureArgElement:         typeArg svarname -> ^(ASTPLACEHOLDER typeArg svarname);
procedureStatements:         statements2* -> ^(ASTPROCEDUREDEFCODE statements2*);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// PRINT, PLOT, SHEET, CLIP
// ---------------------------------------------------------------------------------------------------------------------------------------------------

//print:					prtHelper expression -> ^(ASTPRINT expression);
print:                      prtHelper prtOpt1? prtElements prtOpt2? -> ^({token("ASTPRT", ASTPRT, input.LT(1).Line)} ^(ASTPRTTYPE prtHelper) ^(ASTPLACEHOLDER prtOpt1?) ^(ASTPLACEHOLDER prtOpt2?) prtElements);
prtHelper:				    P | PRT | PRI | PRINT | MULPRT | GMULPRT | SHEET | CLIP | PLOT;
prtElements:                prtElement (COMMA2 prtElement)* -> ^(ASTPRTELEMENTS prtElement+);

prtElement:                 expression
                            gekkoLabel?
							prtElementOptionField?
							-> ^({token("ASTPRTELEMENT¤"+($expression.text)+"¤"+($expression.start)+"¤"+($expression.stop), ASTPRTELEMENT, 0)} ^(ASTEXPRESSION expression) gekkoLabel? prtElementOptionField?)
						    ;

prtElementOptionField:      leftAngle prtOptionField4Helper* RIGHTANGLE -> ^(ASTPRTELEMENTOPTIONFIELD prtOptionField4Helper*);
prtOpt1:					ISNOTQUAL
						  | leftAngle2          prtOpt1Helper* RIGHTANGLE -> ^(ASTOPT1 prtOpt1Helper*)							
						  | leftAngleNo2 dates? prtOpt1Helper* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) prtOpt1Helper*)
                            ;
prtOptionField4Helper:      width
						  | dec
						  | nwidth
						  | pwidth
						  | ndec
						  | pdec
						  | opt2 -> ^(ASTPRTOPTION opt2)
						  | opt2a -> ^(ASTPRTOPTION opt2a)
						  | TYPE '=' linetypeHelper -> ^(ASTPRTELEMENTLINETYPE linetypeHelper)
						  | DASHTYPE '=' expression -> ^(ASTPRTELEMENTDASHTYPE expression)
						  | LINEWIDTH '=' expression -> ^(ASTPRTELEMENTLINEWIDTH expression)
						  | LINECOLOR '=' expression -> ^(ASTPRTELEMENTLINECOLOR expression)
						  | POINTTYPE '=' expression -> ^(ASTPRTELEMENTPOINTTYPE expression)
						  | POINTSIZE '=' expression -> ^(ASTPRTELEMENTPOINTSIZE expression)
						  | FILLSTYLE '=' expression -> ^(ASTPRTELEMENTFILLSTYLE expression)						
						  | Y2 -> ^(ASTPRTELEMENTY2)
						    ;
prtOpt1Helper:              filter						
						  | opt2 -> ^(ASTPRTOPTION opt2)
						  | opt2a -> ^(ASTPRTOPTION opt2a)
						  | WIDTH EQUAL expression -> ^(ASTOPT_VAL_WIDTH expression)
						  | DEC EQUAL expression -> ^(ASTOPT_VAL_DEC expression)
						  | NWIDTH EQUAL expression -> ^(ASTOPT_VAL_NWIDTH expression)
						  | PWIDTH EQUAL expression -> ^(ASTOPT_VAL_PWIDTH expression)
						  | NDEC EQUAL expression -> ^(ASTOPT_VAL_NDEC expression)
						  | PDEC EQUAL expression -> ^(ASTOPT_VAL_PDEC expression)						
						  | APPEND (EQUAL yesNo)? -> ^(ASTOPT_STRING_APPEND yesNo?)
						  | BOXWIDTH '=' expression -> ^(ASTOPT_VAL_BOXWIDTH expression)  //PLOT
						  | BOXGAP '=' expression -> ^(ASTOPT_VAL_BOXGAP expression)  //PLOT
						  | CELL '=' expression -> ^(ASTOPT_STRING_CELL expression)
						  | COLLAPSE (EQUAL prtOptCollapseHelper)? -> ^(ASTOPT_STRING_COLLAPSE prtOptCollapseHelper?)
						  | COLORS (EQUAL yesNo)? -> ^(ASTOPT_STRING_COLORS yesNo?)
						  | COLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_COLS yesNo?)
						  | DATES (EQUAL yesNo)? -> ^(ASTOPT_STRING_DATES yesNo?)						
						  | DUMP (EQUAL yesNo)? -> ^(ASTOPT_STRING_DUMP yesNo?)
						  | FONT '=' expression -> ^(ASTOPT_STRING_FONT expression)  //PLOT
						  | FONTSIZE '=' expression -> ^(ASTOPT_VAL_FONTSIZE expression)  //PLOT						
						  | GRID '=' gridHelper -> ^(ASTOPT_STRING_GRID gridHelper)
						  | HEADING '=' expression -> ^(ASTOPT_STRING_TITLE expression)
						  | KEY '=' expression -> ^(ASTOPT_STRING_KEY expression)  //PLOT
						  | NOMAX (EQUAL yesNo)? -> ^(ASTOPT_STRING_NOMAX yesNo?)	
						  | TITLE '=' expression -> ^(ASTOPT_STRING_TITLE expression)
						  | NAMES (EQUAL yesNo)? -> ^(ASTOPT_STRING_NAMES yesNo?)	
						  | PALETTE '=' expression -> ^(ASTOPT_STRING_PALETTE expression)  //PLOT					
						  | PLOTCODE '=' expression -> ^(ASTOPT_STRING_PLOTCODE expression)
						  | ROWS (EQUAL yesNo)? -> ^(ASTOPT_STRING_ROWS yesNo?)						
						  | SEPARATE (EQUAL yesNo)? -> ^(ASTOPT_STRING_SEPARATE yesNo?)  //PLOT
						  | SHEET '=' expression -> ^(ASTOPT_STRING_SHEET expression)
						  | SIZE '=' expression -> ^(ASTOPT_STRING_SIZE expression)  //PLOT						
						  | STACK (EQUAL yesNo)? -> ^(ASTOPT_STRING_STACK yesNo?)  //PLOT
						  | STAMP (EQUAL yesNo)? -> ^(ASTOPT_STRING_STAMP yesNo?)	
						  | SUBTITLE '=' expression -> ^(ASTOPT_STRING_SUBTITLE expression)	  //PLOT	
						  | TICS '=' expression -> ^(ASTOPT_STRING_TICS expression)  //PLOT			
						  | USING EQUAL fileNameStar -> ^(ASTOPT_STRING_USING fileNameStar)		
						  | XLINE '=' expression -> ^(ASTOPT_DATE_XLINE expression)  //PLOT	
						  | XLINEBEFORE '=' expression -> ^(ASTOPT_DATE_XLINEBEFORE expression)  //PLOT	
						  | XLINEAFTER '=' expression -> ^(ASTOPT_DATE_XLINEAFTER expression)  //PLOT							  						
						  | X2ZEROAXIS (EQUAL yesNo)? -> ^(ASTOPT_STRING_X2ZEROAXIS yesNo?)
						  | Y2LINE EQUAL expression -> ^(ASTOPT_VAL_Y2LINE expression)  //PLOT						
						  | Y2MAX EQUAL expression -> ^(ASTOPT_VAL_Y2MAX expression)  //PLOT	
						  | Y2MIN EQUAL expression -> ^(ASTOPT_VAL_Y2MIN expression)  //PLOT	
						  | Y2MAXHARD EQUAL expression -> ^(ASTOPT_VAL_Y2MAXHARD expression)  //PLOT	
						  | Y2MINHARD EQUAL expression -> ^(ASTOPT_VAL_Y2MINHARD expression)  //PLOT	
						  | Y2MAXSOFT EQUAL expression -> ^(ASTOPT_VAL_Y2MAXSOFT expression)  //PLOT	
						  | Y2MINSOFT EQUAL expression -> ^(ASTOPT_VAL_Y2MINSOFT expression)  //PLOT	
						  | XZEROAXIS (EQUAL yesNo)? -> ^(ASTOPT_STRING_XZEROAXIS yesNo?)		  						  				
						  | YLINE EQUAL expression -> ^(ASTOPT_VAL_YLINE expression)  //PLOT
						  | YMAX EQUAL expression -> ^(ASTOPT_VAL_YMAX expression)  //PLOT	
						  | YMIN EQUAL expression -> ^(ASTOPT_VAL_YMIN expression)  //PLOT	
						  | YMAXHARD EQUAL expression -> ^(ASTOPT_VAL_YMAXHARD expression)  //PLOT	
						  | YMINHARD EQUAL expression -> ^(ASTOPT_VAL_YMINHARD expression)  //PLOT	
						  | YMAXSOFT EQUAL expression -> ^(ASTOPT_VAL_YMAXSOFT expression)  //PLOT	
						  | YMINSOFT EQUAL expression -> ^(ASTOPT_VAL_YMINSOFT expression)  //PLOT						
						  | YMIRROR '=' expression -> ^(ASTOPT_STRING_YMIRROR expression)  //PLOT
						  | YTITLE EQUAL expression -> ^(ASTOPT_STRING_YTITLE expression)  //PLOT
						  | Y2TITLE EQUAL expression -> ^(ASTOPT_STRING_Y2TITLE expression)  //PLOT
						  | GRIDSTYLE EQUAL expression -> ^(ASTOPT_STRING_GRIDSTYLE expression)  //PLOT
						  | BOLD EQUAL expression -> ^(ASTOPT_STRING_BOLD expression)  //PLOT
						  | ITALIC EQUAL expression -> ^(ASTOPT_STRING_ITALIC expression)  //PLOT						
						  | TYPE '=' linetypeHelper -> ^(ASTOPT_STRING_LINETYPE linetypeHelper)
						  | DASHTYPE '=' expression -> ^(ASTOPT_STRING_DASHTYPE expression)
						  | LINEWIDTH '=' expression -> ^(ASTOPT_VAL_LINEWIDTH expression)
						  | LINECOLOR '=' expression -> ^(ASTOPT_STRING_LINECOLOR expression)
						  | POINTTYPE '=' expression -> ^(ASTOPT_STRING_POINTTYPE expression)
						  | POINTSIZE '=' expression -> ^(ASTOPT_VAL_POINTSIZE expression)
						  | FILLSTYLE '=' expression -> ^(ASTOPT_STRING_FILLSTYLE expression)	
						  | BANK EQUAL name -> ^(ASTOPT_STRING_BANK name)						  
						  | REF EQUAL name -> ^(ASTOPT_STRING_REF name)
						  | MISSING EQUAL name -> ^(ASTOPT_STRING_MISSING name)
						  | SPLIT (EQUAL yesNo)? -> ^(ASTOPT_STRING_SPLIT yesNo?)
						    ;
linetypeHelper:             LINESPOINTS -> ASTLINESPOINTS
						  | LINES -> ASTLINES
						  | BOXES -> ASTBOXES
						  | FILLEDCURVES -> ASTFILLEDCURVES
						  | STEPS -> ASTSTEPS
						  | POINTS -> ASTPOINTS
						  | DOTS -> ASTDOTS
						  | IMPULSES -> ASTIMPULSES
						  | expression
						    ;
gridHelper:                 YLINE -> ASTYLINE
						  | XLINE -> ASTXLINE
						  | yesNo					
						    ;
prtOpt2:                    prtOpt2Helper+ -> ^(ASTOPT2 prtOpt2Helper+);
prtOpt2Helper:              FILE '=' fileName -> ^(ASTOPT_STRING_FILENAME fileName)
						  | USING '=' fileNameStar -> ^(ASTOPT_STRING_USING fileNameStar)
						    ;
prtOptCollapseHelper:       AVG -> ASTAVG
                          | TOTAL -> ASTTOTAL
						  | expression -> expression						
						    ;
opt2:                       optNew | optOld;							
optOld:                     N    ('=' yesNo -> ^(ASTN yesNo) | -> ^(ASTN ASTYES))
                   
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



opt2a:                        L    ('=' yesNo -> ^(ASTL yesNo) | -> ^(ASTL ASTYES))
	                       | DL    ('=' yesNo -> ^(ASTDL yesNo) | -> ^(ASTDL  ASTYES))
                     	  | RL    ('=' yesNo -> ^(ASTSL yesNo) | -> ^(ASTSL  ASTYES))	
	                      | RDL    ('=' yesNo -> ^(ASTSDL yesNo) | -> ^(ASTSDL  ASTYES))
						    ;


optNew:                     lev
						  | abs
						  | dif
						  | pch
						  | gdif
						  | v
                          ;
abs:					    ABS ('=' yesNoAppend -> ^(ASTABS yesNoAppend) |  -> ^(ASTABS ASTYES))
                          | NOABS -> ^(ASTABS ASTNO)
                          | UABS -> ^(ASTABS ASTAPPEND)
						    ;
lev:						LEV ('=' yesNoAppend -> ^(ASTLEV yesNoAppend) |  -> ^(ASTLEV ASTYES))
                          | NOLEV -> ^(ASTLEV ASTNO)
                          | ULEV -> ^(ASTLEV ASTAPPEND)
						    ;
dif:						(DIF|DIFF) ('=' yesNoAppend -> ^(ASTDIF yesNoAppend) | -> ^(ASTDIF ASTYES))
                          | (NODIF|NODIFF) -> ^(ASTDIF ASTNO)
                          | (UDIF|UDIFF) -> ^(ASTDIF ASTAPPEND)
                            ;
pch:						PCH ('=' yesNoAppend -> ^(ASTPCH yesNoAppend) |  -> ^(ASTPCH ASTYES) )
                          | NOPCH -> ^(ASTPCH ASTNO)
                          | UPCH -> ^(ASTPCH ASTAPPEND)
						    ;
gdif:					    (GDIF|GDIFF) ('=' yesNoAppend -> ^(ASTGDIF yesNoAppend) | -> ^(ASTGDIF ASTYES) )
                          | (NOGDIF|NOGDIFF) -> ^(ASTGDIF ASTNO)
                          | (UGDIF|UGDIFF) -> ^(ASTGDIF ASTAPPEND)
						    ;
v:    					    V ('=' yesNo -> ^(ASTV yesNo) | -> ^(ASTV ASTYES))
                          | NOV -> ^(ASTV ASTNO)
						    ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// R_FILE, R_EXPORT, R_RUN
// ---------------------------------------------------------------------------------------------------------------------------------------------------

r_file:   				    R_FILE fileName -> ^({token("ASTR_FILE", ASTR_FILE, input.LT(1).Line)} ^(ASTPLACEHOLDER fileName?));

r_export:  				    R_EXPORT r_exportOpt1? seqOfBankvarnames -> ^({token("ASTR_EXPORT", ASTR_EXPORT, input.LT(1).Line)}  ^(ASTPLACEHOLDER r_exportOpt1?) ^(ASTPLACEHOLDER seqOfBankvarnames));
r_exportOpt1:			    ISNOTQUAL | leftAngle r_exportOpt1h* RIGHTANGLE -> r_exportOpt1h*;
r_exportOpt1h:              TARGET EQUAL expression -> ^(ASTOPT_STRING_TARGET expression);

r_run:  				    R_RUN r_runOpt1? -> ^({token("ASTR_RUN", ASTR_RUN, input.LT(1).Line)}  r_runOpt1? );
r_runOpt1:			        ISNOTQUAL | leftAngle r_runOpt1h* RIGHTANGLE -> r_runOpt1h*;
r_runOpt1h:                 MUTE (EQUAL yesNo)? -> ^(ASTOPT_STRING_MUTE yesNo?);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// REBASE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

rebase:                     REBASE rebaseOpt1? seqOfBankvarnames rebaseDate1? rebaseDate2? -> ^({token("ASTREBASE", ASTREBASE, input.LT(1).Line)} seqOfBankvarnames ^(ASTPLACEHOLDER rebaseDate1? rebaseDate2?) rebaseOpt1?);
rebaseDate1:                expression;
rebaseDate2:                expression;
rebaseOpt1:                 ISNOTQUAL | leftAngle rebaseOpt1h* RIGHTANGLE -> ^(ASTOPT1 rebaseOpt1h*);							
rebaseOpt1h:                BANK EQUAL name -> ^(ASTOPT_STRING_BANK name)  //name can be without quotes
						  | PREFIX EQUAL name -> ^(ASTOPT_STRING_PREFIX name) //name can be without quotes
						  | INDEX EQUAL expression -> ^(ASTOPT_VAL_INDEX expression)
						    ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// RENAME
// ---------------------------------------------------------------------------------------------------------------------------------------------------

rename:                     RENAME renameOpt1? assignmentType seqOfBankvarnames asOrTo seqOfBankvarnames -> ^({token("ASTRENAME", ASTRENAME, input.LT(1).Line)} ^(ASTPLACEHOLDER assignmentType) seqOfBankvarnames seqOfBankvarnames renameOpt1?);
renameOpt1:                 ISNOTQUAL | leftAngle renameOpt1h* RIGHTANGLE -> renameOpt1h*;
renameOpt1h:                FROMBANK EQUAL name -> ^(ASTOPT_STRING_FROMBANK name)
						  |	asOrToBank EQUAL name -> ^(ASTOPT_STRING_TOBANK name)
						  | BANK EQUAL name -> ^(ASTOPT_STRING_BANK name)
						  | PRINT (EQUAL yesNo)? -> ^(ASTOPT_STRING_PRINT yesNo?)
					//	  | TYPE EQUAL name -> ^(ASTOPT_STRING_TYPE name)
							;

asOrTo:						AS | TO;
asOrToBank:					ASBANK | TOBANK;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// READ and IMPORT
// ---------------------------------------------------------------------------------------------------------------------------------------------------

						    //!!!Two identical lines ONLY because of token stuff
read:                       readHelper   readOpt1? fileNameStar (TO nameOrStar)? -> ^({token("ASTREAD", ASTREAD, input.LT(1).Line)} readHelper readOpt1? ^(ASTHANDLEFILENAME fileNameStar) ^(ASTREADTO nameOrStar?))
						    ;

readHelper:                 READ | IMPORT;

readOpt1:                   ISNOTQUAL
						  | leftAngle2          readOpt1h* RIGHTANGLE -> readOpt1h*						
						  | leftAngleNo2 dates? readOpt1h* RIGHTANGLE -> ^(ASTDATES_TYPE2 dates?) readOpt1h*
                            ;

readOpt1h:                  MERGE (EQUAL yesNo)? -> ^(ASTOPT_STRING_MERGE yesNo?)
						  | PRIM (EQUAL yesNo)? -> ^(ASTOPT_STRING_PRIM yesNo?)  //obsolete
						  | FIRST (EQUAL yesNo)? -> ^(ASTOPT_STRING_FIRST yesNo?)
						  | REF (EQUAL yesNo)? -> ^(ASTOPT_STRING_REF yesNo?)												
						  | TSD (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSD yesNo?)
						  | TSDX (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSDX yesNo?)
						  | GBK (EQUAL yesNo)? -> ^(ASTOPT_STRING_GBK yesNo?)
						  | GDX (EQUAL yesNo)? -> ^(ASTOPT_STRING_GDX yesNo?)
						  | GDXOPT EQUAL expression -> ^(ASTOPT_STRING_GDXOPT expression)
						  | TSP (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSP yesNo?)
						  | PCIM (EQUAL yesNo)? -> ^(ASTOPT_STRING_PCIM yesNo?)
						  | CSV (EQUAL yesNo)? -> ^(ASTOPT_STRING_CSV yesNo?)
						  | PRN (EQUAL yesNo)? -> ^(ASTOPT_STRING_PRN yesNo?)
						  | PX (EQUAL yesNo)? -> ^(ASTOPT_STRING_PX yesNo?)
						  | XLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_XLS yesNo?)
  						  | XLSX (EQUAL yesNo)? -> ^(ASTOPT_STRING_XLSX yesNo?)
						  | COLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_COLS yesNo?)
						  | ARRAY (EQUAL yesNo)? -> ^(ASTOPT_STRING_ARRAY yesNo?)
						  | FLAT (EQUAL yesNo)? -> ^(ASTOPT_STRING_FLAT yesNo?)
						  | AREMOS (EQUAL yesNo)? -> ^(ASTOPT_STRING_AREMOS yesNo?)						  
						  | SHEET '=' expression -> ^(ASTOPT_STRING_SHEET expression)
						  | CELL '=' expression -> ^(ASTOPT_STRING_CELL expression)
						  | NAMECELL '=' expression -> ^(ASTOPT_STRING_NAMECELL expression)
						  | DATECELL '=' expression -> ^(ASTOPT_STRING_DATECELL expression)
						  | METHOD '=' name -> ^(ASTOPT_STRING_METHOD name)
						  | COLLAPSE '=' name -> ^(ASTOPT_STRING_COLLAPSE name)						  
						  | RESPECT (EQUAL yesNo)? -> ^(ASTOPT_STRING_RESPECT yesNo?)	
						  | ALL (EQUAL yesNo)? -> ^(ASTOPT_STRING_ALL yesNo?)	
						    ;
							
// ---------------------------------------------------------------------------------------------------------------------------------------------------
// RESTART
// ---------------------------------------------------------------------------------------------------------------------------------------------------

restart:                    RESTART -> ^({token("ASTRESTART", ASTRESTART, input.LT(1).Line)});

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// RUN
// ---------------------------------------------------------------------------------------------------------------------------------------------------

run:                        RUN fileNameStar -> ^({token("ASTRUN", ASTRUN, input.LT(1).Line)} fileNameStar);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// SHEET<import>
// ---------------------------------------------------------------------------------------------------------------------------------------------------

                          // This is SHEET<import> or SHEET<2010 2015 import>.
						  // The rule stipulates that import must be before other settings, and there must be file=, and there must be an option field.
						  // We also have a SHEET without import, see the prt rule
sheetImport               : SHEET sheetImportOpt1 seqOfBankvarnames FILE '=' fileName -> ^({token("ASTSHEETIMPORT", ASTSHEETIMPORT, input.LT(1).Line)} ^(ASTPLACEHOLDER sheetImportOpt1) ^(ASTHANDLEFILENAME fileName?) seqOfBankvarnames);
sheetImportOpt1           : ISNOTQUAL
						  | leftAngle2          IMPORT sheetImportOpt1h* RIGHTANGLE -> ASTPLACEHOLDER  sheetImportOpt1h*  //error here if the placeholder is not here
						  | leftAngleNo2 dates? IMPORT sheetImportOpt1h* RIGHTANGLE -> ASTPLACEHOLDER ^(ASTDATES dates?) sheetImportOpt1h*
						  ;

sheetImportOpt1h          : CELL '=' expression -> ^(ASTOPT_STRING_CELL expression)						
						  | COLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_COLS yesNo?)						
						  | ROWS (EQUAL yesNo)? -> ^(ASTOPT_STRING_ROWS yesNo?)
						  | SHEET '=' expression -> ^(ASTOPT_STRING_SHEET expression)
						  | MATRIX (EQUAL yesNo)? -> ^(ASTOPT_STRING_MATRIX yesNo?)		
						  | MISSING (EQUAL yesNo)? -> ^(ASTOPT_STRING_MISSING yesNo?)		
						  ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// SIGN
// ---------------------------------------------------------------------------------------------------------------------------------------------------

sign:					    SIGN -> ^({token("ASTSIGN", ASTSIGN, input.LT(1).Line)});

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// SIM
// ---------------------------------------------------------------------------------------------------------------------------------------------------

sim:                        SIM simOpt1? -> ^({token("ASTSIM", ASTSIM, input.LT(1).Line)} simOpt1?);
simOpt1:                    ISNOTQUAL
						  | leftAngle2          simOpt1h* RIGHTANGLE -> ^(ASTOPT1 simOpt1h*)							
						  | leftAngleNo2 dates? simOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) simOpt1h*)
                            ;
simOpt1h:                   FIX (EQUAL yesNo)? -> ^(ASTOPT_STRING_FIX yesNo?)
						  | STATIC (EQUAL yesNo)? -> ^(ASTOPT_STRING_STATIC yesNo?)
						  | AFTER (EQUAL yesNo)? -> ^(ASTOPT_STRING_AFTER yesNo?)
						  | RES (EQUAL yesNo)? -> ^(ASTOPT_STRING_RES yesNo?)
						    ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// SMOOTH
// ---------------------------------------------------------------------------------------------------------------------------------------------------

smooth:                     SMOOTH seqOfBankvarnames EQUAL seqOfBankvarnames smoothOpt2? seqOfBankvarnames? -> ^({token("ASTSMOOTH", ASTSMOOTH, input.LT(1).Line)} ^(ASTPLACEHOLDER smoothOpt2?) seqOfBankvarnames seqOfBankvarnames seqOfBankvarnames?);
smoothOpt2:                 smoothOpt2h;  //can only choose 1
smoothOpt2h:                SPLINE (EQUAL yesNo)? -> ^(ASTOPT_STRING_SPLINE yesNo?)
                          | REPEAT (EQUAL yesNo)? -> ^(ASTOPT_STRING_REPEAT yesNo?)
                          | GEOMETRIC (EQUAL yesNo)? -> ^(ASTOPT_STRING_GEOMETRIC yesNo?)
						  | LINEAR (EQUAL yesNo)? -> ^(ASTOPT_STRING_LINEAR yesNo?)
						  | OVERLAY (EQUAL yesNo)? -> ^(ASTOPT_STRING_OVERLAY yesNo?)
						    ;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// SPLICE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

splice:                     SPLICE seqOfBankvarnames EQUAL seqOfBankvarnames expression seqOfBankvarnames -> ^({token("ASTSPLICE", ASTSPLICE, input.LT(1).Line)} seqOfBankvarnames seqOfBankvarnames seqOfBankvarnames expression     )
                          | SPLICE seqOfBankvarnames EQUAL seqOfBankvarnames seqOfBankvarnames            -> ^({token("ASTSPLICE", ASTSPLICE, input.LT(1).Line)} seqOfBankvarnames seqOfBankvarnames seqOfBankvarnames )  //no date
						    ;
spliceOpt1:                 ISNOTQUAL
						  | leftAngle        spliceOpt1h* RIGHTANGLE -> spliceOpt1h*												
                            ;
spliceOpt1h:                KEEP EQUAL spliceOptions -> ^(ASTOPT_STRING_KEEP spliceOptions);
spliceOptions:              FIRST | LAST;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// STOP
// ---------------------------------------------------------------------------------------------------------------------------------------------------

stop:					    STOP -> ^({token("ASTSTOP", ASTSTOP, input.LT(1).Line)});

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// SYS
// ---------------------------------------------------------------------------------------------------------------------------------------------------

sys:						SYS -> ^({token("ASTSYS", ASTSYS, input.LT(1).Line)})
						  | SYS sysOpt1? expression -> ^({token("ASTSYS", ASTSYS, $SYS.Line)} expression sysOpt1?)
						    ;
sysOpt1:			        ISNOTQUAL | leftAngle sysOpt1h* RIGHTANGLE -> sysOpt1h*;
sysOpt1h:                   MUTE (EQUAL yesNo)? -> ^(ASTOPT_STRING_MUTE yesNo?);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// TABLE
// ---------------------------------------------------------------------------------------------------------------------------------------------------
						  //TODO: use {token()} for line numbers...
table:					    TABLE name EQUAL NEW TABLE leftParenGlue ')'  -> ^(ASTNEWTABLE name)
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
						    //col, t1, t2, expression, operator, scale, format
						  						  
						  | tableCurrow SETVALUES leftParenGlue expression ',' expression ',' expression ',' expression ',' expression ',' expression ',' expression ')'  -> ^(ASTTABLESETVALUES tableCurrow expression expression expression ^(ASTTABLESETVALUESELEMENT expression ASTPLACEHOLDER ASTPLACEHOLDER) expression expression expression)
						  
						  | TABLE     tableOpt1? fileName -> ^(ASTTABLE     tableOpt1? ^(ASTHANDLEFILENAME fileName)) //!beware line below
						  | MENUTABLE tableOpt1? fileName -> ^(ASTMENUTABLE tableOpt1? ^(ASTHANDLEFILENAME fileName)) //!beware line above
						    ;
tableOpt1:                  ISNOTQUAL
						  | leftAngle2          tableOpt1h* RIGHTANGLE -> ^(ASTOPT1 tableOpt1h*)							
						  | leftAngleNo2 dates? tableOpt1h* RIGHTANGLE -> ^(ASTOPT1 ^(ASTDATES dates?) tableOpt1h*)
                            ;
tableOpt1h:                 HTML (EQUAL yesNo)? -> ^(ASTOPT_STRING_HTML yesNo?)
						  | WINDOW EQUAL MAIN -> ^(ASTOPT_STRING_WINDOW ASTTABLEMAIN)						
  						  | optOld  //ops						
						    ;

tableCurrow:			    TABLE name GLUEDOT DOT CURROW GLUEDOT DOT  -> name;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// TARGET
// ---------------------------------------------------------------------------------------------------------------------------------------------------

target2:                    TARGET ident -> ^(ASTTARGET ident);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// TELL
// ---------------------------------------------------------------------------------------------------------------------------------------------------

tell:					    TELL ('<' NOCR? '>')? expression? -> ^({token("ASTTELL", ASTTELL, input.LT(1).Line)} ^(ASTPLACEHOLDER expression?) NOCR?);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// TIME
// ---------------------------------------------------------------------------------------------------------------------------------------------------

time:                       TIME dates -> ^({token("ASTTIME", ASTTIME, input.LT(1).Line)} ^(ASTDATES dates))
						  | TIME question -> ^({token("ASTTIMEQUESTION", ASTTIMEQUESTION, input.LT(1).Line)})		
						  | TIME oneDate -> ^({token("ASTTIME", ASTTIME, input.LT(1).Line)} ^(ASTDATES oneDate oneDate))  //duplicating, TIME 2015 ==> TIME 2015 2015
						    ;
oneDate:                    expression;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// TIMEFILTER
// ---------------------------------------------------------------------------------------------------------------------------------------------------

timefilter:                 TIMEFILTER timefilterperiods -> ^({token("ASTTIMEFILTER", ASTTIMEFILTER, input.LT(1).Line)} timefilterperiods);
timefilterperiods:		    (timefilterperiod (',' timefilterperiod)*)?  -> ^(ASTTIMEFILTERPERIODS timefilterperiod+);
timefilterperiod:           expression ((doubleDot | TO) expression (BY expression)?)? -> ^(ASTTIMEFILTERPERIOD expression (expression expression?)?);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// UNFIX
// ---------------------------------------------------------------------------------------------------------------------------------------------------

unfix:					    UNFIX -> ^({token("ASTUNFIX", ASTUNFIX, input.LT(1).Line)});

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// TRANSLATE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

translate: TRANSLATE translateOpt1? fileName -> ^({token("ASTTRANSLATE", ASTTRANSLATE, input.LT(1).Line)} translateOpt1?  ^(ASTHANDLEFILENAME fileName?));
translateOpt1: ISNOTQUAL | leftAngle        translateOpt1h* RIGHTANGLE -> translateOpt1h*;						
translateOpt1h:             GEKKO18 (EQUAL yesNo)? -> ^(ASTOPT_STRING_GEKKO18 yesNo?)
						  | GEKKO20 (EQUAL yesNo)? -> ^(ASTOPT_STRING_GEKKO20 yesNo?)
						  | AREMOS (EQUAL yesNo)? -> ^(ASTOPT_STRING_AREMOS yesNo?)
						  | MOVE (EQUAL yesNo)? -> ^(ASTOPT_STRING_MOVE yesNo?)  //not shown in documentation etc.
						  | REMOVE (EQUAL yesNo)? -> ^(ASTOPT_STRING_REMOVE yesNo?)  //not shown in documentation etc.
						  ;	

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// WRITE and EXPORT
// ---------------------------------------------------------------------------------------------------------------------------------------------------

						    //!!!2x2 identical lines ONLY because of token stuff
write:					    writeHelper writeOpt1? seqOfBankvarnames (asOrTo seqOfBankvarnames)? FILE '=' fileName -> ^({token("ASTWRITE", ASTWRITE, input.LT(1).Line)}  writeHelper ^(ASTPLACEHOLDER writeOpt1?) ^(ASTHANDLEFILENAME fileName) ^(ASTNAMESLIST seqOfBankvarnames) ^(ASTNAMESLIST seqOfBankvarnames))						  
						  | writeHelper writeOpt1? fileName -> ^({token("ASTWRITE", ASTWRITE, input.LT(1).Line)} writeHelper ^(ASTPLACEHOLDER writeOpt1?)  ^(ASTHANDLEFILENAME fileName) ^(ASTNAMESLIST) ^(ASTNAMESLIST))						  
						    ;

writeHelper:                WRITE | EXPORT;
writeOpt1:                  ISNOTQUAL
						  | leftAngle2          writeOpt1h* RIGHTANGLE -> writeOpt1h*
						  | leftAngleNo2 dates? writeOpt1h* RIGHTANGLE ->  ^(ASTDATES_TYPE2 dates?) writeOpt1h*
						    ;

writeOpt1h:                 TSD (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSD yesNo?)  //all these will fail, just to provide better error messages for WRITE<csv> etc.
						  | TSDX (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSDX yesNo?)
						  | FROMBANK EQUAL name -> ^(ASTOPT_STRING_FROMBANK name)	
						  | GBK (EQUAL yesNo)? -> ^(ASTOPT_STRING_GBK yesNo?)
						  | GDX (EQUAL yesNo)? -> ^(ASTOPT_STRING_GDX yesNo?)
						  | GDXOPT EQUAL expression -> ^(ASTOPT_STRING_GDXOPT expression)
						  | TSP (EQUAL yesNo)? -> ^(ASTOPT_STRING_TSP yesNo?)
						  | CSV (EQUAL yesNo)? -> ^(ASTOPT_STRING_CSV yesNo?)
						  | PRN (EQUAL yesNo)? -> ^(ASTOPT_STRING_PRN yesNo?)
						  | R (EQUAL yesNo)? -> ^(ASTOPT_STRING_R yesNo?)
						  | XLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_XLS yesNo?)
  						  | XLSX (EQUAL yesNo)? -> ^(ASTOPT_STRING_XLSX yesNo?)						
						  | CAPS (EQUAL yesNo)? -> ^(ASTOPT_STRING_CAPS yesNo?)		
						  | GNUPLOT (EQUAL yesNo)? -> ^(ASTOPT_STRING_GNUPLOT yesNo?)						  
						  | GCM (EQUAL yesNo)? -> ^(ASTOPT_STRING_GCM yesNo?)
						  | OP EQUAL exportType -> ^(ASTOPT_STRING_OP exportType)							  				
						  | FLAT (EQUAL yesNo)? -> ^(ASTOPT_STRING_FLAT yesNo?)
						  | COLS (EQUAL yesNo)? -> ^(ASTOPT_STRING_COLS yesNo?)	
						  | RESPECT (EQUAL yesNo)? -> ^(ASTOPT_STRING_RESPECT yesNo?)	
						  | ALL (EQUAL yesNo)? -> ^(ASTOPT_STRING_ALL yesNo?)	
						  ;						  

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// RESET
// ---------------------------------------------------------------------------------------------------------------------------------------------------

reset:					    RESET -> ^({token("ASTRESET", ASTRESET, input.LT(1).Line)});

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// RETURN
// ---------------------------------------------------------------------------------------------------------------------------------------------------

return2:                    RETURN2 expression? -> ^({token("ASTRETURN", ASTRETURN, input.LT(1).Line)} expression?); //used in functions

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// TRUNCATE
// ---------------------------------------------------------------------------------------------------------------------------------------------------

truncate:                   TRUNCATE truncateOpt1? seqOfBankvarnames -> ^({token("ASTTRUNCATE", ASTTRUNCATE, input.LT(1).Line)} ^(ASTPLACEHOLDER ^(ASTOPT_ truncateOpt1?)) seqOfBankvarnames);
truncateOpt1:               ISNOTQUAL | leftAngle truncateOpt1h? RIGHTANGLE -> truncateOpt1h?;
truncateOpt1h:              dates -> ^(ASTDATES dates);

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// XEDIT
// ---------------------------------------------------------------------------------------------------------------------------------------------------

xedit:                      XEDIT fileNameStar -> ^({token("ASTXEDIT", ASTXEDIT, input.LT(1).Line)} ^(ASTHANDLEFILENAME fileNameStar));

// ---------------------------------------------------------------------------------------------------------------------------------------------------
// X12A
// ---------------------------------------------------------------------------------------------------------------------------------------------------

x12a:					    X12A x12aOpt1? seqOfBankvarnames -> ^({token("ASTX12A", ASTX12A, input.LT(1).Line)} ^(ASTPLACEHOLDER ^(ASTOPT_ x12aOpt1?)) seqOfBankvarnames);
x12aOpt1:                   ISNOTQUAL
						  | leftAngle2          x12aOpt1h* RIGHTANGLE -> x12aOpt1h*
						  | leftAngleNo2 dates? x12aOpt1h* RIGHTANGLE ->  ^(ASTDATES dates?) x12aOpt1h*
						    ;
x12aOpt1h:                  PARAM EQUAL expression -> ^(ASTOPT_STRING_PARAM expression)
						  | BANK EQUAL name -> ^(ASTOPT_STRING_BANK name)  //name can be without quotes
						    ;

//-----------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------
//---------------------------- Options ----------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------
						
//!! do not use '_' as a char in a an option name. 'failsafe' is fine, but fail_safe is not.
optionType:			   
			   question -> question

			 | BUGFIX IMPORT EXPORT '='? yesNoSimple -> BUGFIX IMPORT EXPORT ^(ASTBOOL yesNoSimple)
			 | BUGFIX PX '='? yesNoSimple -> BUGFIX PX ^(ASTBOOL yesNoSimple)
			 | BUGFIX DOWNLOAD '='? yesNoSimple -> BUGFIX DOWNLOAD ^(ASTBOOL yesNoSimple)
		//	 | BUGFIX SPEEDUP '='? yesNoSimple -> BUGFIX SPEEDUP ^(ASTBOOL yesNoSimple)
			 | BUGFIX GBK '='? yesNoSimple -> BUGFIX GBK ^(ASTBOOL yesNoSimple)

             //| CALC question -> CALC question
             //| CALC IGNOREMISSINGVARS  '='? yesNoSimple -> CALC IGNOREMISSINGVARS ^(ASTBOOL yesNoSimple)  //addresses both UPD and GENR
			
			 | DATABANK question -> DATABANK question
             | DATABANK COMPARE TABS '='? numberIntegerOrDouble -> DATABANK COMPARE TABS numberIntegerOrDouble
             | DATABANK COMPARE TREL '='? numberIntegerOrDouble  -> DATABANK COMPARE TREL numberIntegerOrDouble
             | DATABANK FILE COPYLOCAL '='? yesNoSimple -> DATABANK FILE COPYLOCAL ^(ASTBOOL yesNoSimple )
             | DATABANK FILE FORMAT '='? optionDatabankFileFormatOptions ->  DATABANK FILE FORMAT ^(ASTSTRINGSIMPLE optionDatabankFileFormatOptions)
             | DATABANK FILE GBK COMPRESS '='? yesNoSimple -> DATABANK FILE GBK COMPRESS ^(ASTBOOL yesNoSimple)
             | DATABANK FILE GBK VERSION '='? numberIntegerOrDouble ->  DATABANK FILE GBK VERSION ^(ASTSTRINGSIMPLE numberIntegerOrDouble)  //NOTE: number converted to string
			 | DATABANK FILE GBK INTERNAL '='? expression ->  DATABANK FILE GBK INTERNAL ^(ASTSTRINGSIMPLE expression)
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
             | FREQ '='? name -> FREQ ^(ASTSTRINGSIMPLE name)

			 | GAMS TIME DETECT AUTO '='? yesNoSimple -> GAMS TIME DETECT AUTO ^(ASTBOOL yesNoSimple)
			 | GAMS EXE FOLDER '='? fileName -> GAMS EXE FOLDER ^(ASTSTRINGSIMPLE fileName)
			 | GAMS FAST '='? yesNoSimple -> GAMS FAST ^(ASTBOOL yesNoSimple)
			 | GAMS TIME FREQ '='? expression -> GAMS TIME FREQ ^(ASTSTRINGSIMPLE expression)
			 | GAMS TIME OFFSET '='? Integer -> GAMS TIME OFFSET ^(ASTINTEGER Integer)
			 | GAMS TIME PREFIX '='? expression -> GAMS TIME PREFIX ^(ASTSTRINGSIMPLE expression)
			 | GAMS TIME SET '='? expression -> GAMS TIME SET ^(ASTSTRINGSIMPLE expression)			 

			 | INTERFACE question -> INTERFACE question
			 | INTERFACE ALIAS '='? yesNoSimple -> INTERFACE ALIAS ^(ASTBOOL yesNoSimple)			 
             | INTERFACE CLIPBOARD DECIMALSEPARATOR '='? optionInterfaceExcelDecimalseparator -> INTERFACE CLIPBOARD DECIMALSEPARATOR ^(ASTSTRINGSIMPLE optionInterfaceExcelDecimalseparator)
			 | INTERFACE CSV DECIMALSEPARATOR '='? optionInterfaceExcelDecimalseparator -> INTERFACE CSV DECIMALSEPARATOR ^(ASTSTRINGSIMPLE optionInterfaceExcelDecimalseparator)
			 | INTERFACE DATABANK SWAP '='? yesNoSimple -> INTERFACE DATABANK SWAP ^(ASTBOOL yesNoSimple)			
			 | INTERFACE DEBUG '='? optionInterfaceDebug -> INTERFACE DEBUG ^(ASTSTRINGSIMPLE optionInterfaceDebug)
             | INTERFACE MODE '='? mode2 -> INTERFACE MODE ^(ASTSTRINGSIMPLE mode2)
			 | INTERFACE EXCEL LANGUAGE '='? optionInterfaceExcelLanguage -> INTERFACE EXCEL LANGUAGE ^(ASTSTRINGSIMPLE optionInterfaceExcelLanguage)
             | INTERFACE EXCEL MODERNLOOK '='? yesNoSimple -> INTERFACE EXCEL MODERNLOOK ^(ASTBOOL yesNoSimple)
             | INTERFACE HELP COPYLOCAL '='? yesNoSimple -> INTERFACE HELP COPYLOCAL ^(ASTBOOL yesNoSimple)
			 | INTERFACE LAGFIX '='? yesNoSimple -> INTERFACE LAGFIX ^(ASTBOOL yesNoSimple)
			 | INTERFACE REMOTE '='? yesNoSimple -> INTERFACE REMOTE ^(ASTBOOL yesNoSimple)
			 | INTERFACE REMOTE FILE '='? fileName -> INTERFACE REMOTE FILE ^(ASTSTRINGSIMPLE fileName)
             | INTERFACE SOUND '='? yesNoSimple -> INTERFACE SOUND ^(ASTBOOL yesNoSimple)
             | INTERFACE SOUND TYPE '='? optionInterfaceSound -> INTERFACE SOUND TYPE ^(ASTSTRINGSIMPLE optionInterfaceSound)
             | INTERFACE SOUND WAIT '='? Integer -> INTERFACE SOUND WAIT ^(ASTINTEGER Integer)
             | INTERFACE SUGGESTIONS '='? optionInterfaceSuggestions -> INTERFACE SUGGESTIONS ^(ASTSTRINGSIMPLE optionInterfaceSuggestions)             | MODEL question -> MODEL question
			 | INTERFACE TABLE OPERATORS '='? yesNoSimple ->  INTERFACE TABLE OPERATORS  ^(ASTBOOL yesNoSimple)
			 | INTERFACE ZOOM '='? Integer -> INTERFACE ZOOM ^(ASTINTEGER Integer)

			 | MENU question -> MENU question
			 | MENU STARTFILE '='? fileName ->  MENU STARTFILE ^(ASTSTRINGSIMPLE fileName)

			 | MODEL question -> MODEL question
             | MODEL CACHE MAX '='? Integer -> MODEL CACHE MAX  ^(ASTINTEGER Integer)
             | MODEL CACHE '='? yesNoSimple -> MODEL CACHE ^(ASTBOOL yesNoSimple)
			 | MODEL GAMS DEP CURRENT '='? yesNoSimple -> MODEL GAMS DEP CURRENT ^(ASTBOOL yesNoSimple)
			 | MODEL GAMS DEP METHOD '='? name -> MODEL GAMS DEP METHOD ^(ASTSTRINGSIMPLE name)
			 | MODEL INFOFILE '='? optionModelInfoFile -> MODEL INFOFILE ^(ASTSTRINGSIMPLE optionModelInfoFile)
			 | MODEL TYPE '='? name -> MODEL TYPE ^(ASTSTRINGSIMPLE name)
			 

			 | PLOT question -> PLOT question	
			 | PLOT ELEMENTS MAX '='? Integer -> PLOT ELEMENTS MAX ^(ASTINTEGER Integer)		 
			 | PLOT LINES POINTS '='? yesNoSimple -> PLOT LINES POINTS ^(ASTBOOL yesNoSimple )	
			 | PLOT XLABELS ANNUAL '='? optionPlotXlabels ->  PLOT XLABELS ANNUAL ^(ASTSTRINGSIMPLE optionPlotXlabels)
			 | PLOT XLABELS DIGITS '='? Integer ->  PLOT XLABELS DIGITS  ^(ASTINTEGER Integer)
			 | PLOT XLABELS NONANNUAL '='? optionPlotXlabels ->  PLOT XLABELS NONANNUAL ^(ASTSTRINGSIMPLE optionPlotXlabels)
			 | PLOT DECIMALSEPARATOR '='? optionInterfaceExcelDecimalseparator ->  PLOT DECIMALSEPARATOR ^(ASTSTRINGSIMPLE optionInterfaceExcelDecimalseparator)
			 | PLOT USING '='? fileName -> PLOT USING ^(ASTSTRINGSIMPLE fileName)

			 | PLOT NEW '='? yesNoSimple -> PLOT NEW ^(ASTBOOL yesNoSimple )		
			
			 | PRINT question -> PRINT question
			 | PRINT COLLAPSE '='? optionPrintCollapse ->  PRINT COLLAPSE ^(ASTSTRINGSIMPLE optionPrintCollapse)
			 | PRINT ELEMENTS MAX '='? Integer -> PRINT ELEMENTS MAX ^(ASTINTEGER Integer)		 
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
			 | PRINT SPLIT '='? yesNoSimple -> PRINT SPLIT ^(ASTBOOL yesNoSimple)

			 | R EXE FOLDER '='? fileName -> R EXE FOLDER ^(ASTSTRINGSIMPLE fileName)
			 | R EXE PATH '='? fileName -> R EXE PATH ^(ASTSTRINGSIMPLE fileName)  //obsolete, same as above and for legacy

             | SERIES DYN '='? yesNoSimple -> SERIES DYN ^(ASTBOOL yesNoSimple)
			 | SERIES FAILSAFE '='? yesNoSimple -> SERIES FAILSAFE ^(ASTBOOL yesNoSimple)

			 | SERIES ARRAY IGNOREMISSING '='? yesNoSimple -> SERIES ARRAY IGNOREMISSING ^(ASTBOOL yesNoSimple)	
			 | SERIES DATA IGNOREMISSING '='? yesNoSimple -> SERIES DATA IGNOREMISSING ^(ASTBOOL yesNoSimple)	

			 | SERIES NORMAL PRINT MISSING '=' optionSeriesMissing -> SERIES NORMAL PRINT MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)			 
             | SERIES NORMAL CALC MISSING '=' optionSeriesMissing -> SERIES NORMAL CALC MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)
			 | SERIES NORMAL TABLE MISSING '=' optionSeriesMissing -> SERIES NORMAL TABLE MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)

             | SERIES ARRAY PRINT MISSING '=' optionSeriesMissing -> SERIES ARRAY PRINT MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)
             | SERIES ARRAY CALC MISSING '=' optionSeriesMissing -> SERIES ARRAY CALC MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)
			 | SERIES ARRAY TABLE MISSING '=' optionSeriesMissing -> SERIES ARRAY TABLE MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)

			 | SERIES DATA MISSING '=' optionSeriesMissing -> SERIES DATA MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)	

			 //These are not used
			 | SERIES DATA PRINT MISSING '=' optionSeriesMissing -> SERIES DATA PRINT MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)
			 | SERIES DATA CALC MISSING '=' optionSeriesMissing -> SERIES DATA CALC MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)     //sumt(...)
			 | SERIES DATA TABLE MISSING '=' optionSeriesMissing -> SERIES DATA TABLE MISSING ^(ASTSTRINGSIMPLE optionSeriesMissing)
			 
			 | SHEET question -> SHEET question
			 | SHEET COLLAPSE '='? optionPrintCollapse ->  SHEET COLLAPSE ^(ASTSTRINGSIMPLE optionPrintCollapse)			 
			 | SHEET FREQ '='? optionPrintFreq ->  SHEET FREQ ^(ASTSTRINGSIMPLE optionPrintFreq)             
			 | SHEET ENGINE '='? optionSheetEngine -> SHEET ENGINE ^(ASTSTRINGSIMPLE optionSheetEngine)
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
			 | SOLVE NEWTON ROBUST '='? yesNoSimple -> SOLVE NEWTON ROBUST ^(ASTBOOL yesNoSimple)	
             | SOLVE NEWTON ITERMAX '='? Integer -> SOLVE NEWTON ITERMAX ^(ASTINTEGER Integer)
             | SOLVE NEWTON UPDATEFREQ '='? Integer -> SOLVE NEWTON UPDATEFREQ ^(ASTINTEGER Integer)
             | SOLVE PRINT '='? yesNoSimple -> SOLVE PRINT ^(ASTBOOL yesNoSimple)  //obsolete
             | SOLVE PRINT DETAILS '='? yesNoSimple -> SOLVE PRINT DETAILS ^(ASTBOOL yesNoSimple)
             | SOLVE PRINT ITER '='? yesNoSimple -> SOLVE PRINT ITER ^(ASTBOOL yesNoSimple)
             | SOLVE STATIC '='? yesNoSimple -> SOLVE STATIC ^(ASTBOOL yesNoSimple)
             | SOLVE UNDO '='? yesNoSimple -> SOLVE UNDO ^(ASTBOOL yesNoSimple)

			 | STRING2 INTERPOLATE FORMAT VAL '='? expression -> STRING2 INTERPOLATE FORMAT VAL ^(ASTSTRINGSIMPLE expression)
			
			 | SYSTEM CODE SPLIT '='? Integer -> SYSTEM CODE SPLIT ^(ASTINTEGER Integer)
			 | SYSTEM CLONE '='? yesNoSimple -> SYSTEM CLONE ^(ASTBOOL yesNoSimple)
			
			 | TABLE question -> TABLE question
             | TABLE HTML DATAWIDTH '='? numberIntegerOrDouble ->  TABLE HTML DATAWIDTH numberIntegerOrDouble
			 | TABLE HTML FIRSTCOLWIDTH '='? numberIntegerOrDouble ->  TABLE HTML FIRSTCOLWIDTH numberIntegerOrDouble
             | TABLE HTML FONT '='? fileName ->  TABLE HTML FONT ^(ASTSTRINGSIMPLE fileName)
             | TABLE HTML FONTSIZE '='? numberIntegerOrDouble ->  TABLE HTML FONTSIZE numberIntegerOrDouble		
			 | TABLE HTML SECONDCOLWIDTH '='? numberIntegerOrDouble ->  TABLE HTML SECONDCOLWIDTH numberIntegerOrDouble
			 | TABLE HTML SPECIALMINUS '='? yesNoSimple ->  TABLE HTML SPECIALMINUS ^(ASTBOOL yesNoSimple)
             | TABLE IGNOREMISSINGVARS '='? yesNoSimple ->  TABLE IGNOREMISSINGVARS ^(ASTBOOL yesNoSimple)			
             | TABLE TYPE '='? tableType ->  TABLE TYPE ^(ASTSTRINGSIMPLE tableType)
			
		     | TABLE MDATEFORMAT '='? expression ->  TABLE MDATEFORMAT ^(ASTSTRINGSIMPLE expression)
			 | TABLE DECIMALSEPARATOR '='? optionInterfaceExcelDecimalseparator ->  TABLE DECIMALSEPARATOR ^(ASTSTRINGSIMPLE optionInterfaceExcelDecimalseparator)
			 | TABLE THOUSANDSSEPARATOR '='? yesNoSimple ->  TABLE THOUSANDSSEPARATOR ^(ASTBOOL yesNoSimple)
			 | TABLE STAMP '='? yesNoSimple ->  TABLE STAMP ^(ASTBOOL yesNoSimple)
			
			 | TIMEFILTER question -> TIMEFILTER question
             | TIMEFILTER '='? yesNoSimple -> TIMEFILTER ^(ASTBOOL yesNoSimple)
             | TIMEFILTER TYPE '='? timefilterType -> TIMEFILTER TYPE ^(ASTSTRINGSIMPLE timefilterType)
            ;

optionModelInfoFile: YES | NO | TEMP;
timefilterType: HIDE | AVG;
tableType: TXT | HTML;
optionPlotXlabels: AT2 | BETWEEN ;
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
optionSeriesMissing : ERROR | M | ZERO | SKIP;
optionSheetEngine: EXCEL | INTERNAL;


// ------------------------------------------------------------------------------------------------------------------
// ------------------- logical END -------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

// ------------------------------------
// Semi lexer stuff here
// ------------------------------------

integer:                    Integer -> ^(ASTINTEGER Integer);

numberIntegerOrDouble:      integer
						  | double2;

gekkoLabel:                 expression -> ^(ASTGEKKOLABEL expression);

yesNoAppend:			    yesNo
						  | append
						    ;

nwidth:					    NWIDTH '=' expression -> ^(ASTPRTELEMENTNWIDTH expression);
pwidth:					    PWIDTH '=' expression -> ^(ASTPRTELEMENTPWIDTH expression);
ndec:					    NDEC '=' expression -> ^(ASTPRTELEMENTNDEC expression);
pdec:					    PDEC '=' expression -> ^(ASTPRTELEMENTPDEC expression);
width:					    WIDTH '=' expression -> ^(ASTPRTELEMENTWIDTH expression);
dec:						DEC '=' expression -> ^(ASTPRTELEMENTDEC expression);

append:					    APPEND -> ASTAPPEND;

filter:                     FILTER '=' (  no   -> ^(ASTPRTTIMEFILTER NO)
										| yes  -> ^(ASTPRTTIMEFILTER YES)
										| HIDE -> ^(ASTPRTTIMEFILTER HIDE)
										| AVG  -> ^(ASTPRTTIMEFILTER AVG)    )
						  | FILTER -> ^(ASTPRTTIMEFILTER YES)
						  | NOFILTER -> ^(ASTPRTTIMEFILTER NO)
						    ;

optionFreq:                 a | q | m | u;
a:                          A;
q:                          Q;
m:                          M;
u:                          U;

						    //If æøåÆØÅ then you need to put inside ''. Also with blanks. And parts beginning with a digit will not work either (5file.7z)
fileName:                   fileNameFirstPart (GLUEBACKSLASH fileNamePart)* -> ^(ASTFILENAME fileNameFirstPart fileNamePart*)
						  | expression
						    ;
fileNameFirstPart:          fileNameFirstPart1  //   c:\xx
						  | fileNameFirstPart2  //   \xx
						  | fileNameFirstPart3  //   xx
						    ;
						    //For instance READ c:\a.b\c.d, cannot be c:a.b\c.d
						    //ok to use name before colon, drive indicator should start with a letter.
fileNameFirstPart1:         name ':' slashHelper1 fileNamePart -> ^(ASTFILENAMEFIRST1 name fileNamePart);
                            //For instance READ \a.b\c.d, cannot be READ\a.b\c.d
fileNameFirstPart2:         slashHelper2 fileNamePart -> ^(ASTFILENAMEFIRST2 fileNamePart);
                            //For instance READ a.b
fileNameFirstPart3:         fileNamePart -> ^(ASTFILENAMEFIRST3 fileNamePart);
							//stuff like 'a.7z' or 'a b.doc' or 'æøå.doc' must be in quotes.
fileNamePart:               fileNamePartHelper (GLUEDOT DOT fileNamePartHelper)* -> ^(ASTFILENAMEPART fileNamePartHelper+);

fileNamePartHelper:         name
						  | identDigit  //cathes stuff like \05banker\bank etc.
						    ;

slashHelper1:               GLUEBACKSLASH | DIV;
slashHelper2:               BACKSLASH | DIV;

fileNameStar:               fileName
						  | star -> ASTFILENAMESTAR
						    ;


exportType:                 D -> ASTOPD
						  | P  -> ASTOPP
						  | M  -> ASTOPM
						  | Q  -> ASTOPQ
						  | MP -> ASTOPMP
						  | N -> ASTOPN
						  | expression  //will handle quotes etc.
						    ;

nameOrStar:                 name -> name
						  | star -> ASTBANKISSTARCHEATCODE
						    ;


dates: expression expression;

yesNo:					    yes
						  | no
						  | expression
						    ;
yesNoSimple:			    yes
						  | no
						    ;
yes:                        YES -> ASTYES;
no:                         NO -> ASTNO;


leftParen:                  (GLUE!)? LEFTPAREN;
leftParenGlue:              GLUE! LEFTPAREN;
leftParenNoGlue:            LEFTPAREN;

leftBracketGlue:            LEFTBRACKETGLUE;
leftBracket:                LEFTBRACKETGLUE | LEFTBRACKET;

pow:                        stars -> ASTPOW
                          | HAT -> ASTPOW
                            ;

leftAngle:                  leftAngle2 | leftAngleNo2;
leftAngle2:				    LEFTANGLESPECIAL; // <=<
leftAngleNo2:	            LEFTANGLESIMPLE;  // <

rightParen:                 RIGHTPAREN (GLUE!)?;

leftBracketNoGlue:          LEFTBRACKET;
leftBracketNoGlueWild:      LEFTBRACKETWILD;

identDigit:                 identDigitHelper -> ^(ASTIDENTDIGIT identDigitHelper);
identDigitHelper:		    ident                 //for instance ab27
						  | Integer               //for instance 0123
						  | DigitsEDigits         //for instance 25e12 (will end here, not in IdentStartingWithInt)
						  | DateDef               //for instance 2012q3 (will end here, not in IdentStartingWithInt)						  						
						  | IdentStartingWithInt  //for instance 0123ab27 (catches the rest of these cases)								  			  												  
					//	  | HASH
					//	  | PERCENT
					//	  | EXCLAMATION
					//	  | name                  //for instance {%s}
						    ;
						
leftCurly:                  (GLUE!)? LEFTCURLY;
leftCurlyGlue:              GLUE! LEFTCURLY;
leftCurlyNoGlue:            LEFTCURLY;

doubleVerticalBar:          GLUE? (DOUBLEVERTICALBAR1 | DOUBLEVERTICALBAR2);

doubleDot:                  GLUEDOT? DOT GLUEDOT DOT;
doubleDot2:                 GLUEDOT? DOT GLUEDOT? DOT; //can accept two dots with space between
tripleDot:                  GLUEDOT? DOT GLUEDOT DOT GLUEDOT DOT;

double2:                    double2Helper -> ^(ASTDOUBLE double2Helper);
double2Helper:              Double            //0.123 or 25e+12
						  | DigitsEDigits     //for instance 25e12 which can also be a name chunk.
						    ;

star:                       (GLUESTAR!)? STAR (GLUESTAR!)?;
starGlueBoth:               GLUESTAR! STAR GLUESTAR!;
starGlueLeft:               GLUESTAR! STAR;
starGlueRight:              STAR GLUESTAR!;
starNoGlue:                 STAR;
stars:                      (GLUESTAR!)? STARS (GLUESTAR!)?;
triplestars:                (GLUESTAR!)? TRIPLESTARS (GLUESTAR!)?;

question:                   (GLUESTAR!)? QUESTION (GLUESTAR!)?;
questionGlueBoth:           GLUESTAR! QUESTION GLUESTAR!;
questionGlueLeft:           GLUESTAR! QUESTION;
questionGlueRight:          QUESTION GLUESTAR!;
questionNoGlue:             QUESTION;

date2:                      Integer | DateDef;

ident:                      ident2 -> ^(ASTIDENT ident2);  //same as ident3, but includes command names

identWithoutCommand:        ident3 -> ^(ASTIDENT ident3);  //same as ident2, but without command names

ident2: 					Ident |
					        // --- tokens3 start ---		
							
  ACCEPT|
  ANALYZE|
  CHECKOFF|
  CLEAR|
  CLIP|
  CLONE|  
  CLOSE|
  CLS|
  COLLAPSE|
  COMPARE|
  COPY|
  COUNT|
  CREATE|
  CUT|
  DATE|
  DECOMP|
  DECOMP2|
  EVAL|
  DELETE|
  DISP|
  DOC|
  DOWNLOAD|
  EDIT|
  ENDO|
  END|
  EXIT|
  EXO|
  EXPORT|
  FINDMISSINGDATA|
  BROWSER|
  FOR|
  FUNCTION|
  GOTO|
  GLOBAL|
  HDG|
  HELP|
  IF|
  IMPORT|
  INDEX|
  INI|
  INTERPOLATE|
  ITERSHOW|
  LIST|
  LOCK_|
  LOCAL|  
  MATRIX|
  MEM|
  MENU|
  MODEL|
  MODE|
  MULPRT|
  NAME|
  OLS|
  OPEN|
  OPTION|
  PAUSE|
  PIPE|
  PLOT|
  PRINT|
  PRI|
  PROCEDURE|
  PRT|
  P|
  R_EXPORT|
  R_FILE|
  R_RUN|
  READ|
  REBASE|
  RENAME|
  RESET|
  BLOCK|
  RESTART|
  RETURN2|
  RUN|
  SERIES|
  SER|
  SHEET|
  SHOW|
  SIGN|
  SIM|
  SMOOTH|
  SPLICE|
  STOP|
  STRING2|
  SYS|
  TABLE|
  TARGET|
  TELL|
  TIMEFILTER|
  TIME|
  TRANSLATE|
  TRUNCATE|
  UNFIX|
  SPEEDUP|
  UNLOCK_|
  UNSWAP|
  VAL|
  WRITE|
  X12A|
  XEDIT|
  // ---------------------------------------
  ABS|
  GEOMETRIC|
  OVERLAY|
  ADDBANK|
  SHOWBANK|
  SHOWFREQ|
  ADD|
  AFTER2|
  AFTER|
  ALIGNCENTER|
  ALIGNLEFT|
  ALIGNRIGHT|
  ALL|
  AND|
  ANNUAL|
  APPEND|
  AREMOS|
  OP|
  ARRAY|
  ARROW|
  ASERIES|
  ASER|
  AS|
  AT2|
  AUTO|
  AVG|
  A|
  BACKTRACK|
  BANK1|
  BANK2|
  BANK|
  BETWEEN|
  BOLD|
  BOWL|
  BOXES|
  BOXGAP|
  BOXWIDTH|
  BUGFIX|
  GCM|
  BY|
  CACHE|
  CALC|
  CAPS|
  CELL|
  CHANGE|
  CLEAR2|
  CLIPBOARD|
  CLOSEALL|
  CLOSEBANKS|
  CODE|
  MOVE|
  REMOVE|
  COLNAMES|
  COLORS|
  COLS|
  COMMAND1|
  COMMAND2|
  COMMAND|
  COMMA|
  COMPRESS|
  CONSTANT|
  CONST|
  CONTINUE|
  CONV1|
  CONV2|
  CONV|
  COPYLOCAL|  
  CPLOT|
  CREATEVARS|
  CSV|
  CURROW|
  DAMP|
  DANISH|
  DASHTYPE|
  DATABANK|
  DATAWIDTH|
  DATA|
  DATES|
  DEBUG|
  DECIMALSEPARATOR|
  DEC|
  DEFAULT|
  DETAILS|
  DETECT|
  DIALOG|
  DIFF|
  DIFPRT|
  DIF|
  DIGITS|
  DING|
  DYN|
  DIRECT|
  DISPLAY|
  DOTS|
  DP|
  DUMOFF|
  DUMOF|
  DUMON|
  DUMP|
  D|
  EFTER|
  ELEMENTS|
  ELSE|
  ENGLISH|
  ERROR|
  EXCEL|
  EXE|
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
  FILLEDCURVES|
  FILLSTYLE|
  FILTER|
  FIRSTCOLWIDTH|
  FIRST|
  FIX|
  FLAT|
  NAMECELL|
  DATECELL|
  DEP|
  FOLDER|
  FONTSIZE|
  FONT|
  FORMAT|
  FORWARD|
  FREQ|
  FRML|
  FROM|
  GAMS|
  GAUSS|
  GBK|
  GDIFF|
  GDIF|
  GDXOPT|
  GDX|
  GEKKO18|
  GEKKO20|
  GENR|
  GMS|
  GMULPRT|
  GNUPLOT|
  GOAL|
  GRAPH|
  GRIDSTYLE|
  GRID|
  GROWTH|
  HEADING|
  HIDELEFTBORDER|
  HIDERIGHTBORDER|
  HIDE|
  HORIZON|
  HPFILTER|
  HTML|
  ASBANK|
  TOBANK|
  FROMBANK|
  IGNOREMISSINGVARS|
  IGNOREMISSING|
  IGNOREVARS|
  IMPOSE|
  IMPULSES|
  IN|
  INFOFILE|
  INFO|
  INIT|
  INTERFACE|
  INTERNAL|
  INVERT|
  ROBUST|
  ITALIC|
  ITERMAX|
  ITERMIN|
  ITER|
  KEEP|
  MAP|
  KEY|
  LABELS|
  LABEL|
  LAGFIX|
  LAG|
  LANGUAGE|
  LAST|
  LEV|
  LINECOLOR|
  LINESPOINTS|
  LINES|
  LINEWIDTH|
  LISTFILE|
  LOGIC|
  LOG|
  LU|
  MACRO2|
  MAIN|
  MAXLINES|
  MAX|
  MDATEFORMAT|
  MENUTABLE|
  MERGECOLS|
  MERGE|
  MESSAGE|
  METHOD|
  MIN|
  MISSING|
  MIXED|
  MODERNLOOK|
  MP|
  DL|
  L|
  RDL|
  RL|
  MULBK|
  MULPCT|
  MUTE|
  M|
  NAMES|
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
  NOMAX|
  NONANNUAL|
  CURRENT|
  NONE|
  NONMODEL|
  NOPCH|
  NOTIFY|
  NOT|
  NOV|
  NO|
  NWIDTH|
  NYTVINDU|
  N|
  OFFSET|
  OR|
  PALETTE|
  PARAM|
  PATCH|
  PATH|
  PCH|
  PCIMSTYLE|
  PCIM|
  PCTPRT|
  PDEC|
  PERIOD|
  PLOTCODE|
  POINTSIZE|
  POINTS|
  POINTTYPE|
  POS|
  PREFIX|
  PRETTY|
  PRIM|
  OPERATORS|
  PRN|
  PRORATE|
  PROT|
  PRTX|
  PUDVALG|
  PWIDTH|
  PX|
  Q|
  RDP|
  RD|
  REF|
  REL|
  REMOTE|
  ALIAS|
  REORDER|
  REPLACE|
  REPEAT|
  REP|
  RESPECT|
  RES|
  RING|
  RN|
  ROWNAMES|
  ROWS|
  GROUP|
  LEVEL|
  LINK|
  WHERE|
  RP|
  R|
  SAVE|
  SEARCH|
  SECONDCOLWIDTH|
  SEC|
  SEPARATE|
  SER2|
  SER3|
  SERIES2|
  SERIES3|
  SETBORDER|
  SETBOTTOMBORDER|
  SETDATES|
  SETLEFTBORDER|
  SETRIGHTBORDER|
  SETTEXT|
  SETTOPBORDER|
  SETVALUES|
  SET|
  SHOWBORDERS|
  SHOWPCH|
  SIMPLE|
  SIZE|
  SKIP|
  NAN|
  ENGINE|
  NORMAL|
  SOLVE|
  SOME|
  SORT|
  SOUND|
  SOURCE|
  SPECIALMINUS|
  SPLIT|
  STACKED|
  STACK|
  STAMP|
  STARTFILE|
  STATIC|
  STEPS|
  STEP|
  STRIP|
  SUBTITLE|
  SUFFIX|
  SUGGESTIONS|
  SWAP|
  SYSTEM|
  TABLE1|
  TABLE2|
  TABLEOLD|
  TABS|
  TEMP|
  TERMINAL|
  TESTRANDOMMODELCHECK|
  TESTRANDOMMODEL|
  TESTSIM|
  TEST|
  THOUSANDSSEPARATOR|
  TICS|
  TIMESPAN|
  TITLE|
  TOTAL|
  TO|
  TRANSPOSE|
  TREL|
  TRIM|
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
  UPCH|
  UPDATEFREQ|
  UPDX|
  USING|
  U|
  VALUE|
  VAR|
  VERSION|
  VERS|
  VOID|
  VPRT|
  V|
  WAIT|
  WIDTH|
  WINDOW|
  WORKING|
  WPLOT|
  WUDVALG|
  X2ZEROAXIS|
  XLABELS|
  XLINEAFTER|
  XLINEBEFORE|
  XLINE|
  XLSX|
  XLS|
  XZEROAXIS|
  X|
  Y2LINE|
  Y2MAXHARD|
  Y2MAXSOFT|
  Y2MAX|
  Y2MINHARD|
  Y2MINSOFT|
  Y2MIN|
  Y2TITLE|
  Y2|
  YES|
  YLABELS|
  YLINE|
  YMAXHARD|
  YMAXSOFT|
  YMAX|
  YMINHARD|
  YMINSOFT|
  YMIN|
  YMIRROR|
  YTITLE|
  Y|
  ZERO|
  ZOOM|
  ZVAR

							

							// --- tokens3 end ---
							;



//ident3 excludes command names (for the procedure logic)
ident3: 					Ident |
					        // --- tokens4 start ---		
						
  
  ASBANK|
  TOBANK|
  FROMBANK|
  ABS|
  GEOMETRIC|
  OVERLAY|
  ADDBANK|
  SHOWBANK|
  SHOWFREQ|
  ADD|
  AFTER2|
  AFTER|
  ALIGNCENTER|
  ALIGNLEFT|
  ALIGNRIGHT|
  ALL|
  AND|
  ANNUAL|
  APPEND|
  AREMOS|
  OP|
  ARRAY|
  ARROW|
  ASERIES|
  ASER|
  AS|
  AT2|
  AUTO|
  AVG|
  A|
  BACKTRACK|
  BANK1|
  BANK2|
  BANK|
  BETWEEN|
  BOLD|
  BOWL|
  BOXES|
  BOXGAP|
  BOXWIDTH|
  BUGFIX|
  GCM|
  BY|
  CACHE|
  CALC|
  CAPS|
  CELL|
  CHANGE|
  //CLEAR2|
  CLIPBOARD|
  CLOSEALL|
  CLOSEBANKS|
  CODE|
  MOVE|
  REMOVE|
  COLNAMES|
  COLORS|
  COLS|
  COMMAND1|
  COMMAND2|
  COMMAND|
  COMMA|
  COMPRESS|
  CONSTANT|
  CONST|
  CONTINUE|
  CONV1|
  CONV2|
  CONV|
  COPYLOCAL|
  CPLOT|
  CREATEVARS|
  CSV|
  CURROW|
  DAMP|
  DANISH|
  DASHTYPE|
  DATABANK|
  DATAWIDTH|
  DATA|
  DATES|
  DEBUG|
  DECIMALSEPARATOR|
  DEC|
  DEFAULT|
  DETAILS|
  DETECT|
  DIALOG|
  DIFF|
  DIFPRT|
  DIF|
  DIGITS|
  DING|
  DYN|
  DIRECT|
  DISPLAY|
  DOTS|
  DP|
  //DUMOFF|
  //DUMOF|
  //DUMON|
  //DUMP|
  D|
  EFTER|
  ELEMENTS|
  //ELSE|
  ENGLISH|
  ERROR|
  EXCEL|
  EXE|
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
  FILLEDCURVES|
  FILLSTYLE|
  FILTER|
  FIRSTCOLWIDTH|
  FIRST|
  FIX|
  FLAT|
  DATECELL|
  DEP|
  NAMECELL|
  FOLDER|
  FONTSIZE|
  FONT|
  FORMAT|
  FORWARD|
  FREQ|
  FRML|
  FROM|
  GAMS|
  GAUSS|
  GBK|
  GDIFF|
  GDIF|
  GDXOPT|
  GDX|
  GEKKO18|
  GEKKO20|
  GENR|
  GMS|
  //GMULPRT|
  GNUPLOT|
  GOAL|
  GRAPH|
  GRIDSTYLE|
  GRID|
  GROWTH|
  HEADING|
  HIDELEFTBORDER|
  HIDERIGHTBORDER|
  HIDE|
  HORIZON|
  HPFILTER|
  HTML|
  IGNOREMISSINGVARS|
  IGNOREMISSING|
  IGNOREVARS|
  IMPOSE|
  IMPULSES|
  IN|
  INFOFILE|
  INFO|
  INIT|
  INTERFACE|
  INTERNAL|
  INVERT|
  ROBUST|
  ITALIC|
  ITERMAX|
  ITERMIN|
  ITER|
  KEEP|
  KEY|
  LABELS|
  LABEL|
  LAGFIX|
  LAG|
  LANGUAGE|
  LAST|
  LEV|
  LINECOLOR|
  LINESPOINTS|
  LINES|
  LINEWIDTH|
  LISTFILE|
  LOGIC|
  LOG|
  LU|
  MACRO2|
  MAIN|
  MAXLINES|
  MAX|
  MDATEFORMAT|
  MENUTABLE|
  MERGECOLS|
  MERGE|
  MESSAGE|
  METHOD|
  MIN|
  MISSING|
  MIXED|
  MODERNLOOK|
  MP|
  DL|
  L|
  RDL|
  RL|
  MULBK|
  MULPCT|
  MUTE|
  M|
  NAMES|
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
  NOMAX|
  NONANNUAL|
  CURRENT|
  NONE|
  NONMODEL|
  NOPCH|
  NOTIFY|
  NOT|
  NOV|
  NO|
  NWIDTH|
  NYTVINDU|
  N|
  OFFSET|
  OR|
  PALETTE|
  PARAM|
  PATCH|
  PATH|
  PCH|
  PCIMSTYLE|
  PCIM|
  PCTPRT|
  PDEC|
  PERIOD|
  PLOTCODE|
  POINTSIZE|
  POINTS|
  POINTTYPE|
  POS|
  PREFIX|
  PRETTY|
  PRIM|
  OPERATORS|
  PRN|
  PRORATE|
  PROT|
  PRTX|
  PUDVALG|
  PWIDTH|
  PX|
  Q|
  RDP|
  RD|
  REF|
  REL|
  REMOTE|
  ALIAS|
  REORDER|
  REPLACE|
  REPEAT|
  REP|
  RESPECT|
  RES|
  RING|
  RN|
  ROWNAMES|
  ROWS|
  GROUP|
  LEVEL|
  LINK|
  WHERE|
  RP|
  R|
  SAVE|
  SEARCH|
  SECONDCOLWIDTH|
  SEC|
  SEPARATE|
  //SER2|
  //SER3|
  //SERIES2|
  //SERIES3|
  SETBORDER|
  SETBOTTOMBORDER|
  SETDATES|
  SETLEFTBORDER|
  SETRIGHTBORDER|
  SETTEXT|
  SETTOPBORDER|
  SETVALUES|
  SET|
  SHOWBORDERS|
  SHOWPCH|
  SIMPLE|
  SIZE|
  SKIP|
  NAN|
  ENGINE|
  NORMAL|
  SOLVE|
  SOME|
  SORT|
  SOUND|
  SOURCE|
  SPECIALMINUS|
  SPLIT|
  STACKED|
  STACK|
  STAMP|
  STARTFILE|
  STATIC|
  STEPS|
  STEP|
  STRIP|
  SUBTITLE|
  SUFFIX|
  SUGGESTIONS|
  SWAP|
  SYSTEM|
  TABLE1|
  TABLE2|
  TABLEOLD|
  TABS|
  TEMP|
  TERMINAL|
  TESTRANDOMMODELCHECK|
  TESTRANDOMMODEL|
  TESTSIM|
  TEST|
  THOUSANDSSEPARATOR|
  TICS|
  TIMESPAN|
  TITLE|
  TOTAL|
  TO|
  TRANSPOSE|
  TREL|
  TRIM|
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
  UPCH|
  UPDATEFREQ|
  UPDX|
  USING|
  U|
  VALUE|
  VAR|
  VERSION|
  VERS|
  VOID|
  VPRT|
  V|
  WAIT|
  WIDTH|
  WINDOW|
  WORKING|
  WPLOT|
  WUDVALG|
  X2ZEROAXIS|
  XLABELS|
  XLINEAFTER|
  XLINEBEFORE|
  XLINE|
  XLSX|
  XLS|
  XZEROAXIS|
  X|
  Y2LINE|
  Y2MAXHARD|
  Y2MAXSOFT|
  Y2MAX|
  Y2MINHARD|
  Y2MINSOFT|
  Y2MIN|
  Y2TITLE|
  Y2|
  YES|
  YLABELS|
  YLINE|
  YMAXHARD|
  YMAXSOFT|
  YMAX|
  YMINHARD|
  YMINSOFT|
  YMIN|
  YMIRROR|
  YTITLE|
  Y|
  ZERO|
  ZOOM|
  ZVAR
  					

							// --- tokens4 end ---
							;









/*------------------------------------------------------------------
 * LEXER RULES
 *------------------------------------------------------------------*/

expression2:                expression;  //just an alias
expression3:                expression;  //just an alias
expression4:                expression | ;  //alias

stringInQuotes:             StringInQuotes -> ^(ASTSTRINGINQUOTES StringInQuotes)
						  | stringInQuotesWithCurlies
						    ;

 //TODO: Clean up what is fragments and tokens. Stuff used inside lexer rules should be fragments for sure.
 //      Maybe special names for fragments like F_digit etc. And have for instance a F_glue for '¨' that the
 //      GLUE token is defined from.

fragment NEWLINE2:          '\n' ;
fragment NEWLINE3:          '\r\n' ;
fragment DIGIT:             '0'..'9' ;
fragment LETTER:            'a'..'z'|'A'..'Z';

HTTP:                       H_ T_ T_ P_  ':' ('//');  // 'catch HTTP://' before COMMENT interferes with the '//'
HTTPS:                      H_ T_ T_ P_ S_  ':' ('//');  // 'catch HTTPS://' before COMMENT interferes with the '//'

WHITESPACE:                 ( '\t' | ' ' | '\u000C'| NEWLINE2 | NEWLINE3)+ { $channel=HIDDEN; } ;  //u000C is form feed

COMMENT:                    ('//') (~ (NEWLINE2|NEWLINE3))* { $channel=HIDDEN; };
COMMENT_MULTILINE:          '/*' (options {greedy=false;}: COMMENT_MULTILINE | . )* '*/' {$channel=HIDDEN;};

                            //for instance a38x
Ident:                      (LETTER|'_') (DIGIT|LETTER|'_')*  { $type = CheckKeywordsTable(Text); };
                            //for instance 12345
Integer:                    DIGIT+;
                            //for instance 25e12, but not 007
DigitsEDigits:              DIGIT+  ( E_ )  DIGIT+;  //for instance 25e12, problem is this can also be a name chunk! Does not allow 007e10, but 2e0015 is ok
                            //for instance 2012q3
DateDef:                    DIGIT+ ( A_ | Q_ | M_ | U_ ) DIGIT+  //for instance 2000q2 or 2003m11
					      | DIGIT+ ( A_ | U_ )  //2010a or 18u
						    ;  
                            //for instance 05a, everything not captured by Ident, Integer, DigitsEDigits, Datedef.
IdentStartingWithInt:       (DIGIT|LETTER|'_')+;

//It would not be practical to construct Double in the parser. We would like 2012q3 and 7e12 to be recognized as dates and number,
//and this seems hard to do without having the parser work on really small tokens. Would probably be confusing and slow, and we would need glue around + and - (think 1.2e+34...).
//Drawback is that we cannot handle a filenames like xx.7z, 01.txt, 12.13, but they can be put inside ''.
Double:                     DIGIT+ GLUEDOTNUMBER DOT DIGIT* Exponent?   //1.2e+12  Can be without the +
                          | DIGIT+ Exponent                             //25e12    DigitsEDigits captures the 25e12 (that is, not 25e+12) case before it could end here
						  | GLUEDOTNUMBER DOT DIGIT+ Exponent?          //.2e-13   Can be "x=.23" or "x= .23", so glue is not known. Will not read the .23 in a.23 because it will be 'a' GLUEDOT DOT '23'.						  
                            ;

fragment Exponent:          E_ ( '+' | '-' )? DIGIT+;

//String interpolation: 'this is a red old car' 
//                      00000000000000000000000
//String interpolation: 'this is a {'r' + 'ed'} old car' 
//                      111111111111          3333333333
//String interpolation: 'this is a {'r' + 'ed'} old {'car'} from 1980' 
//                      111111111111          2222222     333333333333
//starts in state 0. aaa turns on state 1. Because of state 1, bbb can be matched. Because bbb ends with
//{ state 2 is set, else it would be state 1. State 2 allows match of naked expression. After that
//state is set to 1.

//Uses lexer gates to solve the problem. 
//Problem is that we are going to match "} car'", but it should not match that pattern in for instance "PRT {%i} 'label';". 
//Therefore, when "'this is a {" is matched, a counter is set. Then the "} car'" token is only allowed to match if the couter is set.
//See: https://theantlrguy.atlassian.net/wiki/spaces/ANTLR3/pages/2687108/1.+Lexer
//The rules below do not allow nested interpolations, for instance %s = 'a{'b{'c'}d'}e'; That is crazy anyway.
//StringInQuotes2: only allowed if after a StringInQuotes1, else normal '}' matches.
//                 allows ~' but stops at ' or {.

StringInQuotes:             ('\'' ('~\'' | '~{' | ~('\'' | '{'))* '\'');
StringInQuotes1:            { stringCounter == 0 }?=> ('\'' (~('{' | '\''))* '{') { stringCounter++; };
StringInQuotes2:            { stringCounter == 1 }?=> ('}' (~('{' | '\''))* '{');
StringInQuotes3:            { stringCounter == 1 }?=> ('}' (~('{' | '\''))* '\'') { stringCounter--; };

//moved up here, because some of them start with glue, so better before GLUE token
PLUSEQUAL:                  '+='; //<m>
STAREQUAL:                  '*='; //<q>
//GLUEPERCENTEQUAL:           'x%='; //<p>
PERCENTEQUAL:               '%='; //<p>
//GLUEHASHEQUAL:              'x#='; //<mp>
HASHEQUAL:                  '#='; //<mp>
HATEQUAL:                   '^='; //<d> 

// --- These are done in Program.HandleObeyFilesNew() -------------------------------------------
GLUE:                       '¨';
GLUEDOT:                    '£';  //only relevant for '.', for instance a.b becomes a£.b, and x.1 becomes x£.1
GLUEDOTNUMBER:              '§';  //only relevant for '.', for instance 12.34 becomes 12§.34
GLUESTAR:                   '½';  //only relevant for '*' and '?', for instance a*b --> a½*½b
LEFTANGLESPECIAL:           '<=<';  //indicates that there are two idents following the '<' in the text input.
                                    // using <_< is not good, since it stumbles on mulprt<_lev>xx
//MOD                       '¤';  //does not work with '%¨%' ================> NOT DONE YET!!
GLUEBACKSLASH:              '¨\\';
// -----------------------------------------------------------------------------------------------



ISEQUAL:                    '==';
ISNOTQUAL:                  '<>';
ISLARGEROREQUAL:			'>=';			
ISSMALLEROREQUAL:           '<=';

EXCLAMATION:                '!';
TILDE:					    '~';
AT:                         '@';
HAT:                        '^';
SEMICOLON:                  ';';
COLONGLUE:                  ':|';
COLON:                      ':';
COMMA2:                     ',';
DOT:                        '.';
HASH:                       '#';
PERCENT:                    '%';
DOLLAR:                     '$';
LEFTCURLY:                  '{';
RIGHTCURLY:                 '}';
LEFTPAREN:                  '(';
RIGHTPAREN:                 ')';
LEFTBRACKETGLUE:            '[_[';
LEFTBRACKETWILD:            '[¨[';  //indicates that this is probably a wildcard, not a 1x1 matrix
LEFTBRACKET:                '[';
RIGHTBRACKET:               ']';


LEFTANGLESIMPLE:            '<';
RIGHTANGLE:                 '>';
STAR:                       '*';
DOUBLEVERTICALBAR1:         '||';
DOUBLEAND:                  '&&';
DOUBLEVERTICALBAR2:         '|¨|';
//GLUEDOUBLEVERTICALBAR:    '¨|¨|';
VERTICALBAR:                '|';
PLUS:                       '+';
MINUS:                      '-';
DIV:                        '/';
TRIPLESTARS:                '***';
STARS:                      '**';

EQUAL:                      '=';
MINUSEQUAL:                 '-='; 
DIVEQUAL:                   '/=';

BACKSLASH:                  '\\';
QUESTION:                   '?';

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
//fragment L_:('l'|'L');
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

