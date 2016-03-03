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

// -------------------------------------------------------------------
// Translate from Gekko 1.8 to Gekko 2.0
// The translator is a bit less advanced than the
// AREMOS translator (regarding whitespace).
// -------------------------------------------------------------------

grammar T1;

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
  ASTCOMMAND;
  ASTCOMMAND1;
  ASTCOMMAND2;
  ASTCOMMAND3;
  ASTCOMPARECOMMAND;  
  ASTSERIES;
  FIX = 'FIX';
  GENR = 'GENR';
  LIST = 'LIST';
  SKIP = 'SKIP';
  UPD = 'UPD';
  TIME = 'TIME';
  ASTSERIES;
  AST1;
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

										d.Add("FIX", FIX);
										d.Add("GENR", GENR);
										d.Add("LIST", LIST);
										d.Add("SKIP", SKIP);
										d.Add("UPD", UPD);
										d.Add("TIME", TIME);
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

// this parses fix+fix+fix+fix+fix+fix+fix+fix -> AT
// this parses fix+fix+fix+fix+fix+fix+fix-fix -> HAT
// this parses fix+fix+fix+fix++fix+fix+fix+fix -> fix+fix+fix+fix+fix+fix+fix+fix
//
//expr                      : expressions EOF;  //EOF is necessary in order to force the whole file to be parsed
//expressions               : expression+;
//expression                : FIX PLUS FIX PLUS FIX PLUS FIX PLUS FIX PLUS FIX PLUS FIX PLUS FIX EOL -> AT
//						    | FIX PLUS FIX PLUS FIX PLUS FIX PLUS FIX PLUS FIX PLUS FIX MINUS FIX EOL -> HAT
//				       	    | FIX						  
//						    | PLUS
//						    | MINUS
//						    | EOL
//						    ;


expr                      : expressions EOF;  //EOF is necessary in order to force the whole file to be parsed

expressions               : command+;
				  
command                   : tryFirst
						  | commandName n? commandOptions? n? commandRest? chop -> ^(ASTCOMMAND ^(ASTCOMMAND1 commandName) ^(ASTCOMMAND2 commandOptions?) ^(ASTCOMMAND3 commandRest?)) chop
						  |	general* chop -> general* chop
						  ;		

tryFirst                  : n? SKIP general* chopSkip -> n? AST1 DIV STAR general* AST1 STAR DIV AST1 chopSkip
						  | n? LIST n? PLUS n? HASH ident n? listItem* n? ident n? chop -> n? LIST AST1 ident AST1 EQUAL AST1 listItem* AST1 ident SEMICOLON chop						  
						  | n? UPD n? HASH? ident n? Integer n? Integer general* chop -> n? TIME AST1 Integer AST1 Integer SEMICOLON AST1 ASTSERIES AST1 HASH? ident AST1 general* chop						  						  
						  | n? GENR general* genrHelper* n? chopGenr n? EOL? -> n? ASTSERIES general* genrHelper* SEMICOLON EOL?
						  | n? COMMENT_MULTILINE EOL -> n? COMMENT_MULTILINE EOL
						  | n? COMMENT EOL -> n? COMMENT EOL
						  | n? EOL -> n? EOL 
						  ;
						  
commandName               : n? ident;
commandOptions            : LEFTANGLE atomOptionField* RIGHTANGLE;
commandRest               : general*;

updHelper                 : general | DOLLAR;	

genrHelper                :	EOL general*;

chopGenr                  : SEMICOLON
						  | DOLLAR
						  ;

chop                      : SEMICOLON n? EOL -> SEMICOLON EOL
						  | SEMICOLON -> SEMICOLON
						  | EOL -> SEMICOLON EOL 
						  | DOLLAR n? EOL -> SEMICOLON EOL  //we allow $ instead of ;
						  | DOLLAR -> SEMICOLON             //we allow $ instead of ;
						  ;

chopSkip                  : SEMICOLON n? EOL -> SEMICOLON EOL
						  | SEMICOLON -> SEMICOLON
						  | EOL -> SEMICOLON EOL 						  
						  ;

listItem                  : n? listItemH2? ident listItemH1? -> n? listItemH2? ident COMMA listItemH1?;
listItemH1                : n? lineGlue;
listItemH2                : HASH;

general                   : ident LEFTPAREN n1? plusMinus? n2? Integer n3? RIGHTPAREN  -> ident LEFTBRACKET n1? plusMinus? n2? Integer n3? RIGHTBRACKET
						  | atom
						  ;

atomOptionField           : 						  
						    lineGlue  //&
						  | symbolOptionField 
						  | ident
						  | Double
						  | Integer
						  | n -> n						  
						  | COMMENT_MULTILINE						  
						  | StringInQuotes						  
						  ;

atom                      : 						  
						    lineGlue  //&
						  | symbol 
						  | ident
						  | Double
						  | Integer
						  | n -> n						  
						  | COMMENT_MULTILINE
						  | COMMENT
						  | DisplayExpression
						  | HdgExpression
						  | StringInQuotes						  
						  ;

plusMinus                 : PLUS|MINUS;

n                         : WHITESPACE;
n1                        : WHITESPACE;
n2                        : WHITESPACE;
n3                        : WHITESPACE;
n4                        : WHITESPACE;
n5                        : WHITESPACE;

identComma                : ident n? -> ident COMMA n?;

plusOrMinusX              : plusOrMinus;

plusOrMinus               : PLUS | MINUS;

lineGlue                  : AND n? EOL -> n? EOL;  //make the glue disappear

//-----------------------------------------------------------------------------------------------------------
//---------------------------- Semi lexer stuff -------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------

ident                     : Ident |
						    FIX |
							LIST|
							GENR|
							UPD|
							TIME|
							SKIP
;

symbol                    : symbolOptionField | LEFTANGLE | RIGHTANGLE;

symbolOptionField         : TILDE                     |
                            AND                       |
                            EXCL                      |
                            AT                        |
                            HAT                       |
                            //SEMICOLON                 |
                            COLON                     |
                            COMMA                     |
                            DOT                       |
                            HASH                      |
                            PERCENT                   |
                            //DOLLAR                    |
                            LEFTCURLY                 |
                            RIGHTCURLY                |
                            LEFTPAREN                 |
                            RIGHTPAREN                |
                            LEFTBRACKET               |
                            RIGHTBRACKET              |
                            //LEFTANGLE                 |
                            //RIGHTANGLE                |
                            STAR                      |
                            VERTICALBAR               |
                            PLUS                      |
                            MINUS                     |
                            DIV                       |
                            EQUAL                     |
                            BACKSLASH                 |
                            QUESTION                  |
							ANYTHING
							;


/*------------------------------------------------------------------
 * LEXER RULES
 *------------------------------------------------------------------*/

fragment NEWLINE2         : '\n' ;
fragment NEWLINE3         : '\r\n' ;
fragment DIGIT            : '0'..'9' ;
fragment LETTER           : 'a'..'z'|'A'..'Z';
fragment Exponent         : E_ ( '+' | '-' )? DIGIT+;

EOL                       : NEWLINE2 | NEWLINE3 ;

WHITESPACE                : ( '\t' | ' ' | '\u000C')+ ;  //u000C is form feed

COMMENT                   : ('//') (~ (NEWLINE2|NEWLINE3))* 
						  | ('()') (~ (NEWLINE2|NEWLINE3))*
						  ;

COMMENT_MULTILINE         : '/*' (options {greedy=false;}: COMMENT_MULTILINE | . )* '*/';

                            //for instance a38x
Ident                     : (LETTER|'_') (DIGIT|LETTER|'_')*  { $type = CheckKeywordsTable(Text); };
                            //for instance 12345

StringInQuotes            : ('\'' (~'\'')* '\'');

Double                    : DIGIT+ DOT DIGIT* Exponent?           //1.2e+12 1.e+12 1.e12 1.
                          | DIGIT+ Exponent                       //25e12    DigitsEDigits captures the 25e12 (that is, not 25e+12) case before it could end here						  
                          ;

Integer                   : DIGIT+;

DisplayExpression
: D_ I_ S_ P_ L_ A_ Y_ (' ') (options {
                greedy=false;
           }:.)* EOL ;

HdgExpression
: H_ D_ G_ (' ') (options {
                greedy=false;
           }:.)* EOL ;

TILDE                     : '~';
AND                       : '&';
EXCL                      : '!';
AT                        : '@';
HAT                       : '^';
SEMICOLON                 : ';';
COLON                     : ':';
COMMA                     : ',';
DOT                       : '.';
HASH                      : '#';
PERCENT                   : '%';
DOLLAR                    : '$';
LEFTCURLY                 : '{';
RIGHTCURLY                : '}';
LEFTPAREN                 : '(';
RIGHTPAREN                : ')';
LEFTBRACKET               : '[';
RIGHTBRACKET              : ']';
LEFTANGLE                 : '<';
RIGHTANGLE                : '>';
STAR                      : '*';
VERTICALBAR               : '|';
PLUS                      : '+';
MINUS                     : '-';
DIV                       : '/';
EQUAL                     : '=';
BACKSLASH                 : '\\';
QUESTION                  : '?';

ANYTHING                  : . ;

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


