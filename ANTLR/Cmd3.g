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
    NEGATE;
    ASTINDEXERELEMENT;
    ASTINDEXERELEMENTBANK;
    ASTPOW;
    ASTEMPTYRANGEELEMENT;
	ASTINDEXERALONE;
	ASTINDEXERELEMENTPLUS;
	ASTINTEGER;
	ASTVARIABLENAME;
	ASTPLACEHOLDER;
	ASTDOT;
	ASTFUNCTION;
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
                                      errors.Add(e.Line + "�" + e.CharPositionInLine + "�" + hdr + "�" + msg);
                                  }
                                  public System.Collections.Generic.List<string> GetErrors() {
                                      return errors;
                                  }


                              }


/*------------------------------------------------------------------
 * PARSER RULES
 *------------------------------------------------------------------*/

expr                      : expression ';'? EOF;  //EOF is necessary in order to force the whole file to be parsed


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

unaryExpression           : dollarExpression
                          | MINUS dollarExpression -> ^(NEGATE dollarExpression)
						  ;

dollarExpression          : indexerExpression;   //dollar!!!!

//indexerExpression         : dotExpression (leftBracketGlue^ (indexerExpressionHelper (','! indexerExpressionHelper)*)? RIGHTBRACKET!)*;

//dotExpression             : primaryExpression (GLUEDOT DOT^ expression)*;

indexerExpression         : primaryExpression (leftBracketGlue^ (indexerExpressionHelper (','! indexerExpressionHelper)*)? RIGHTBRACKET! | GLUEDOT DOT^ (variableName | function))*;

primaryExpression         : leftParen! expression RIGHTPAREN!
                          | value
						  ;

value                     : variable
						  | function
						  | Integer -> ^(ASTINTEGER Integer)
						  | (leftBracketNoGlue|leftBracketNoGlueWild) indexerExpressionHelper RIGHTBRACKET -> ^(ASTINDEXERALONE indexerExpressionHelper) //also see rule indexerExpression
						  ;

function                  : Ident leftParenGlue (expression (',' expression)*)? RIGHTPAREN -> ^(ASTFUNCTION Ident expression*);

variable                  : variableName (GLUEDOT DOT^ (variableName|function))*
						  ;

indexerExpressionHelper   : //range -> ^(ASTINDEXERELEMENT range)                             //fm1..fm5
                            expressionOrNothing doubleDot expressionOrNothing -> ^(ASTINDEXERELEMENT expressionOrNothing expressionOrNothing)     //'fm1'..'fm5'
						  | expression -> ^(ASTINDEXERELEMENT expression)                                     //'fm*' or -2 or 2000 or 2010q3
						  | PLUS expression -> ^(ASTINDEXERELEMENTPLUS expression)                            //+1   
                          ;
range                     : name doubleDot name -> name name;
doubleDot                 : GLUEDOT? DOT GLUEDOT DOT;
expressionOrNothing       : expression -> expression
						  | -> ASTEMPTYRANGEELEMENT
						  ;
name                      : Ident;

variableName              : sigil? name freq? -> ^(ASTVARIABLENAME ^(ASTPLACEHOLDER sigil?) ^(ASTPLACEHOLDER name) ^(ASTPLACEHOLDER freq?));

sigil                     : hashOrPercent GLUE -> hashOrPercent;

hashOrPercent             : HASH | PERCENT;

freq			   		  : GLUE TILDE GLUE name -> name;      //TODO: glue

starHelper				  : star
						  | DIV
						  | MOD						
						  ;

leftParen                 : (GLUE!)? LEFTPAREN;
leftParenGlue             : GLUE! LEFTPAREN;
leftBracketGlue           : LEFTBRACKETGLUE;
star                      : (GLUESTAR!)? STAR (GLUESTAR!)?;
pow                       : stars -> ASTPOW
                          | HAT -> ASTPOW
                          ;
stars                     : (GLUESTAR!)? STARS (GLUESTAR!)?;

leftBracketNoGlue         : LEFTBRACKET;
leftBracketNoGlueWild     : LEFTBRACKETWILD; 


/*------------------------------------------------------------------
 * LEXER RULES
 *------------------------------------------------------------------*/

LISTSTAR                  : '&*';
LISTPLUS                  : '&+';
LISTMINUS                 : '&-';


 //TODO: Clean up what is fragments and tokens. Stuff used inside lexer rules should be fragments for sure.
 //      Maybe special names for fragments like F_digit etc. And have for instance a F_glue for '�' that the
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
Ident                     : (LETTER|'_') (DIGIT|LETTER|'_')* ;
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
GLUE                      : '�';
GLUEDOT                   : '�';  //only relevant for '.', for instance a.b becomes a�.b, and x.1 becomes x�.1
GLUEDOTNUMBER             : '�';  //only relevant for '.', for instance 12.34 becomes 12�.34
GLUESTAR                  : '�';  //only relevant for '*' and '?', for instance a*b --> a�*�b
LEFTANGLESPECIAL          : '<=<';  //indicates that there are two idents following the '<' in the text input.
                                    // using <_< is not good, since it stumbles on mulprt<_lev>xx
MOD                       : '�';  //does not work with '%�%' ================> NOT DONE YET!!
GLUEBACKSLASH             : '�\\';
// -----------------------------------------------------------------------------------------------

ISEQUAL                   : '==';
ISNOTQUAL                 : '<>';
ISLARGEROREQUAL			  : '>=';			
ISSMALLEROREQUAL          : '<=';

TILDE					  : '~';
AT                        : '@';
HAT                       : '^';
SEMICOLON                 : ';';
COLONGLUE                 : ':|';
COLON                     : ':';
COMMA2                    : ',';
DOT                       : '.';
HASH                      : '#';
PERCENT                   : '%';
DOLLAR                    : '$';
LEFTCURLY                 : '{';
RIGHTCURLY                : '}';
LEFTPAREN                 : '(';
RIGHTPAREN                : ')';
LEFTBRACKETGLUE           : '[_[';
LEFTBRACKETWILD           : '[�[';  //indicates that this is probably a wildcard, not a 1x1 matrix
LEFTBRACKET               : '[';
RIGHTBRACKET              : ']';


LEFTANGLESIMPLE           : '<';
RIGHTANGLE                : '>';
STAR                      : '*';
DOUBLEVERTICALBAR1        : '||';
DOUBLEVERTICALBAR2        : '|�|';
//GLUEDOUBLEVERTICALBAR     : '�|�|';
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
//fragment AE_:('�'|'�');
//fragment OE_:('�'|'�');
//fragment AA_:('�'|'�');

