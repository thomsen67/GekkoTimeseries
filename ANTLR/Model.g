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

grammar Model;

//(1) set -debug=no in preferences --> 'ANTLR options' when using for C#
//(2) set language to Java when you want to debug in ANTLRWorks (perhaps also undo the above (1))
//    also comment the section 'Comment for debug start' to 'Comment for debug end'

options {
//  language=Java;
  language=CSharp2;
  output = AST;
  backtrack = true;
  memoize = true;
  k=2;
}

tokens {
	LOG = 'LOG';
	EXP = 'EXP';
	FRML = 'FRML';		
    AFTER = 'AFTER';
    AFTER2 = 'AFTER2';
    VAL	= 'VAL';
    VARLIST	= 'VARLIST';
    
	PLUS 	= '+' ;
	MINUS	= '-' ;
	NEGATE;
	MULT	= '*' ;
	DIV	= '/' ;
	LB      = '<' ;
	RB      = '>' ;
	RP      = ')' ;
	DOT     = '.';
	TRUE = 'true' ;
	FALSE = 'false' ;
    ASTMODELBLOCK;	
    ASTAFTER;
    ASTAFTER2;
    ASTASSIGNVAR;
	ASTINTEGER;
	ASTDOUBLE;
	ASTVARIABLE;
	ASTEXPRESSION;
	ASTFRMLCODE;
	ASTFUNCTION;
	ASTLAGFUNCTION;
	ASTLEFTSIDE;
	ASTSIMPLEFUNCTION;	
	ASTVARIABLELAGLEAD;
	ASTLAGORLEAD;
	ASTFRML;
	ASTPOW;
	ASTVAL;
	ASTVARLIST;
}

@parser::namespace { Gekko }
@lexer::namespace { Gekko }
@members {
    private  System.Collections.Generic.List<string> errors = new  System.Collections.Generic.List<string>();
    private System.Collections.Generic.List<string> frmlItems = new System.Collections.Generic.List<string>();
    public override void DisplayRecognitionError(string[] tokenNames,
                                        RecognitionException e) {
        string hdr = GetErrorHeader(e);
        string msg = GetErrorMessage(e, tokenNames);
        errors.Add(e.Line + "¤" + e.CharPositionInLine + "¤" + hdr + "¤" + msg);
    }
    public  System.Collections.Generic.List<string> GetErrors() {
        return errors;
    }
    public System.Collections.Generic.List<string> GetFrmlItems() {
      return frmlItems;
    }    
}

@lexer::members {
  public override void ReportError(RecognitionException e) {
        string hdr = GetErrorHeader(e);
        string msg = "Model lexer error: " + e.Message;
        throw new Exception(e.Line + "¤" + e.CharPositionInLine + "¤" + hdr + "¤" + msg);
  } 
  
  public static System.Collections.Generic.Dictionary<string, int> kw = GetKw();

  public static System.Collections.Generic.Dictionary<string, int> GetKw()
  {
     System.Collections.Generic.Dictionary<string, int> d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
       d.Add("EXP"                    ,   EXP                     );                                        
       d.Add("LOG"                    ,   LOG                     );                                                      
       d.Add("FRML"                   ,   FRML                     );                                        
       d.Add("AFTER"                  ,   AFTER                     );                                        
       d.Add("AFTER2"                 ,   AFTER2                     );                                        
       d.Add("VAL"                ,   VAL                     );       
       d.Add("VARLIST"                ,   VARLIST                     );       
       return d;
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

//Comment for debug end

extraTokens :
     LOG|
     EXP|
     FRML|
     AFTER|
     AFTER2|
     VAL|
     VARLIST
     ;

/*------------------------------------------------------------------
 * PARSER RULES
 *------------------------------------------------------------------*/

expr	:	(expr2)+ EOF;    //EOF is necessary in order to force the whole file to be parsed

expr2    :			
		frml
		| val
		| Modelblock -> ^(ASTMODELBLOCK Modelblock)		
		| AFTER frmlEnding ->  ^(ASTAFTER)
		| AFTER2 frmlEnding ->  ^(ASTAFTER2)		
		| frmlEnding ->              //is just ignored
	 	;		

frml    :	FRML code genrLeftSide '=' expression2 frmlEnding
//TTH:
		{frmlItems.Add($frml.text);}
		-> ^(ASTFRML code genrLeftSide expression2)     		
 ;

genrLeftSide	:	genrLeftSide2 -> ^(ASTLEFTSIDE genrLeftSide2);
genrLeftSide2	:	(ident|simpleFunction);
code	:	ident -> ^(ASTFRMLCODE ident);
expression2	:	expression -> ^(ASTEXPRESSION expression);

number: (Double|Integer);

val : VAL Ident '=' MINUS? number -> ^(ASTVAL Ident number MINUS?);

//-------------------------------------------------------------------------
//----------------------------- expressions start -------------------------
//-------------------------------------------------------------------------	
	
simpleFunction
	:	ident '(' ident ')' -> ^(ASTSIMPLEFUNCTION ident ident)
	;
	
function	:	ident '(' ( expression (',' expression)* )? ')' -> ^(ASTFUNCTION ident expression expression*)	;
functionLogExp: logExp '(' ( expression (',' expression)* )? ')' -> ^(ASTFUNCTION logExp expression expression*)	;   

lagFunction	:	'(' expression ')' '(' numberPlusMinus ')' -> ^(ASTLAGFUNCTION expression numberPlusMinus);

expression	:	
			//lagFunction
			additiveExpression;

additiveExpression		:	multiplicativeExpression ( (PLUS|MINUS)^ multiplicativeExpression )*;

multiplicativeExpression 		:	powerExpression ( (MULT|DIV|MOD)^ powerExpression )*;

powerExpression 		:	unaryExpression ( pow^ unaryExpression )*	;

unaryExpression
			:	primaryExpression
		    	|	MINUS primaryExpression -> ^(NEGATE primaryExpression)   	;
   	
primaryExpression
			:	'('! expression ')'!
			|	value	;	
	
value	
	: 	Integer   -> ^(ASTINTEGER Integer)
	|	Double     -> ^(ASTDOUBLE Double)
	|	functionLogExp 	                 //log(+0.5) or exp(-1.5) gets special treatment (reserved keywords)
	|	variableWithLagOrLead       //this must be before function, in order to match x(-1) as a lag and not a function
	|	function 
	|   assignVar	
	;

assignVar : AssignVar -> ^(ASTASSIGNVAR AssignVar);

variableWithLagOrLead  //IdentLP includes left paranthesis
			:	ident  -> ^(ASTVARIABLE ident FALSE)
			|   ident '(' numberPlusMinus ')' -> ^(ASTVARIABLELAGLEAD ident numberPlusMinus FALSE)	//accepted for now						
			|   ident '[' numberPlusMinus ']' -> ^(ASTVARIABLELAGLEAD ident numberPlusMinus FALSE)
			;							
	
numberPlusMinus 	:	 (MINUS|PLUS) (Integer | Double) ;

logExp: LOG | EXP;

ident   :		Ident | extraTokens;

PLUS	:	'+';
MINUS	:	'-';
frmlEnding	:	'$'|';';
MULT	:	'*';
DIV	:	'/';
MOD	:	'%';

//POW     :       '^' | '**';   --> seems or operator on such tokens does not work as intended
STARS    :      '**';
HAT    :        '^';
pow     :       STARS  -> ASTPOW
              | HAT    -> ASTPOW;

//-------------------------------------------------------------------------
//----------------------------- expressions start -------------------------
//-------------------------------------------------------------------------	


/*------------------------------------------------------------------
 * LEXER RULES
 *------------------------------------------------------------------*/

Integer	: (DIGIT)+  ;

Double
    :
       ('0' .. '9')+ DOT ('0' .. '9')* Exponent?
    |   DOT ( '0' .. '9' )+ Exponent?
    |  ('0' .. '9')+ Exponent
    ;


fragment Exponent
    :   ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+
    ;

DATE	:	  ('0' .. '9')+ (A|Q|M) ('0' .. '9')+ 	;

AssignVar : '%' (LETTER|'_') (DIGIT|LETTER|'_')*;

Ident	: (LETTER|'_') (DIGIT|LETTER|'_')* { $type = CheckKeywordsTable(Text); };

WHITESPACE : ( '\t' | ' ' | '\u000C' )+ 	{ $channel=HIDDEN; } ;  //u000C is form feed

NEWLINE    :   (('\r')? '\n' )  { $channel=HIDDEN; }  ;

Modelblock : ('()' | '//') (~(NEWLINE2|NEWLINE3|'#'))*  '###' (~(NEWLINE2|NEWLINE3|'#'))* '###' (~(NEWLINE2|NEWLINE3|'#'))* ;

Comment    : ('()' | '//') (~ (NEWLINE2|NEWLINE3))* { $channel=HIDDEN; };

// ANTLR 3
NESTED_ML_COMMENT
    :   '/*'
        (options {greedy=false;} : NESTED_ML_COMMENT | . )*
        '*/' {$channel=HIDDEN;}
    ;


fragment NEWLINE2 : '\n' ;
fragment NEWLINE3 : '\r\n' ;
fragment DIGIT	: '0'..'9' ;
fragment LETTER: 'a'..'z'|'A'..'Z';

fragment A:('a'|'A');
fragment B:('b'|'B');
fragment C:('c'|'C');
fragment D:('d'|'D');
fragment E:('e'|'E');
fragment F:('f'|'F');
fragment G:('g'|'G');
fragment H:('h'|'H');
fragment I:('i'|'I');
fragment J:('j'|'J');
fragment K:('k'|'K');
fragment L:('l'|'L');
fragment M:('m'|'M');
fragment N:('n'|'N');
fragment O:('o'|'O');
fragment P:('p'|'P');
fragment Q:('q'|'Q');
fragment R:('r'|'R');
fragment S:('s'|'S');
fragment T:('t'|'T');
fragment U:('u'|'U');
fragment V:('v'|'V');
fragment W:('w'|'W');
fragment X:('x'|'X');
fragment Y:('y'|'Y');
fragment Z:('z'|'Z');

