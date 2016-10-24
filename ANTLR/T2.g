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
// Translate from AREMOS to Gekko 2.0
// The translator is a bit more advanced than the
// Gekko 1.8 translator (regarding whitespace).
// -------------------------------------------------------------------

grammar T2;

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
  ASTNEGATE;
  ASTSTAR;
  ASTSTARS;
  ASTIDENT;
  ASTPLUS;
  ASTMINUS;
  ASTADD;
  ASTMULTIPLY;
  ASTCOMMAND;
  ASTCOMMAND1;
  ASTCOMMAND2;
  ASTCOMMAND3;
  ASTCOMPARECOMMAND;
  ASTSERIES;
  ASTPAREN;
  ASTANGLE;
  ASTBRACKET;
  ASTCURLY;
  ASTLIST;
  FIX = 'FIX';
  GENR = 'GENR';
  LIST = 'LIST';
  SKIP = 'SKIP';
  UPD = 'UPD';
  TIME = 'TIME';
  IF = 'IF';
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
										d.Add("IF", IF);
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

expr					  : expr2* n? EOF;

expr2					  : command semi						  					  
						  | n? COMMENT n? EOL
						  | n? EOL
						  ;
				
command                   : commandName commandOptions? commandRest? -> ^(ASTCOMMAND ^(ASTCOMMAND1 commandName) ^(ASTCOMMAND2 commandOptions?) ^(ASTCOMMAND3 commandRest?))
						  ;		
						 						
commandName               : n? ident;
commandOptions            : angle;
commandRest               : expressionAngle*;

semi                      : n? SEMICOLON -> n? SEMICOLON						  
						  ;

n                         : WHITESPACE;
n1                        : WHITESPACE;
n2                        : WHITESPACE;
n3                        : WHITESPACE;
n4                        : WHITESPACE;
n5                        : WHITESPACE;

//-----------------------------------------------------------------------------------------

expressionAngle           : expression
                          | n? LEFTANGLE
						  | n? RIGHTANGLE
						  ;

expression                : paren 
						  | angle 
						  | bracket 
						  | curly
						  | term
						  ;

paren                     : n? LEFTPAREN expression* n? RIGHTPAREN -> ^(ASTPAREN  n? LEFTPAREN expression* n? RIGHTPAREN);
angle                     : n? LEFTANGLE expression* n? RIGHTANGLE -> ^(ASTANGLE n? LEFTANGLE expression* n? RIGHTANGLE);
bracket                   : n? LEFTBRACKET expression* n? RIGHTBRACKET -> ^(ASTBRACKET n? LEFTBRACKET  expression* n? RIGHTBRACKET);
curly                     : n? LEFTCURLY expression* n? RIGHTCURLY -> ^(ASTCURLY n? LEFTCURLY  expression* n? RIGHTCURLY);

//-----------------------------------------------------------------------------------------------------------
//---------------------------- Semi lexer stuff -------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------

term                      : n? ident 
						  | n? leaf						  
						  | n? StringInQuotes
						  | n? StringInQuotes2
						  | n? Integer
						  | n? Double
						  | n? DigitsEDigits
						  | n? DateDef
						  | n? IdentStartingWithInt
						  | n? Double
						  ;

ident                     : Ident |
						    FIX |
							LIST|
							GENR|
							UPD|
							TIME|
							IF|
							SKIP
;

leaf					  : TILDE                     |       //no ',' or ';' or '!'
                            AND                       |
                            AT                        |
                            HAT                       |                            
                            COLON                     |                            
                            DOT                       |
                            HASH                      |
                            PERCENT                   |
                            DOLLAR                    |                                                      
                            STAR                      |
							STARS                     |
                            VERTICALBAR               |
                            PLUS                      |
                            MINUS                     |
                            DIV                       |
                            EQUAL                     |
                            BACKSLASH                 |
                            QUESTION                  |
							COMMA                     |
							//LEFTANGLE                 |
							//RIGHTANGLE                |
							EOL                       |
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

COMMENT                   : ('!') (~ (NEWLINE2|NEWLINE3))*;

                            //for instance a38x
Ident                     : (LETTER|'_') (DIGIT|LETTER|'_')*  { $type = CheckKeywordsTable(Text); };
                            //for instance 12345

StringInQuotes            : ('\'' (~'\'')* '\'');
StringInQuotes2            : ('\"' (~'\"')* '\"');


Integer                   : DIGIT+  ;
                            //for instance 25e12
DigitsEDigits             : DIGIT+  ( E_ )  DIGIT+;  //for instance 25e12, problem is this can also be a name chunk!
                            //for instance 2012q3
DateDef                   : DIGIT+  ( A_ | Q_ | M_ ) DIGIT+;  //for instance 2000q2 or 2003m11
                            //for instance 05a, everything not captured by Ident, Integer, DigitsEDigits, Datedef.
IdentStartingWithInt      : (DIGIT|LETTER|'_')+;

Double                    : DIGIT+ DOT DIGIT* Exponent?           //1.2e+12  Can be without the +
                          | DIGIT+ Exponent                       //25e12    DigitsEDigits captures the 25e12 (that is, not 25e+12) case before it could end here
						  | DOT DIGIT+ Exponent?                  //.2e-13   Can be "x=.23" or "x= .23", so glue is not known. Will not read the .23 in a.23 because it will be 'a' GLUEDOT DOT '23'.
                          ;

TILDE                     : '~';
AND                       : '&';
//EXCL                      : '!';
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
STARS                     : '**';
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


