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
	ASTBANKVARIABLENAME;
	ASTHASH;
	ASTPERCENT;
	ASTVERTICALBAR;
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
	ASTLOGICALIN;
	ASTDATE2;
	ASTSTRINGINQUOTES;
	ASTDOUBLE;
	ASTDOLLARCONDITIONALVARIABLE;

	ASTNAME;
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

	ASTDOLLARCONDITIONAL;
ASTOR;
ASTAND;
ASTNOT;
ASTCOMPARE;
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


	AND              = 'AND';    
	NOT              = 'NOT';    
	OR              = 'OR';   
	IN             = 'IN'; 
	LISTFILE = 'LISTFILE';
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


/*------------------------------------------------------------------
 * PARSER RULES
 *------------------------------------------------------------------*/

expr                      : expression ';'? EOF;  //EOF is necessary in order to force the whole file to be parsed

// ------------------------------------------------------------------------------------------------------------------
// ------------------- expression START -------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

expression                : additiveExpression; 

additiveExpression        : (multiplicativeExpression -> multiplicativeExpression)
							( (PLUS lbla=multiplicativeExpression -> ^(ASTPLUS $additiveExpression $lbla))
							| (MINUS lblb=multiplicativeExpression -> ^(ASTMINUS $additiveExpression  $lblb)) )*
						  ;  

multiplicativeExpression  : (powerExpression -> powerExpression)
						    ( (star lbla=powerExpression -> ^(ASTSTAR $multiplicativeExpression $lbla))
						    | (DIV lblb=powerExpression -> ^(ASTDIV $multiplicativeExpression  $lblb)) )*  
						  ;  

powerExpression			  : (unaryExpression -> unaryExpression)
						    (pow lbla=unaryExpression -> ^(ASTPOWER $powerExpression $lbla))*	 
						  ; 
  
unaryExpression           : dollarExpression -> dollarExpression
					      | MINUS dollarExpression -> ^(ASTNEGATE dollarExpression)
						  ;						 

dollarExpression		  : (indexerExpression -> indexerExpression)
						    (DOLLAR lbla=dollarConditional -> ^(ASTPOWER $dollarExpression $lbla))*	 
						  ; 						  

indexerExpression         : primaryExpression
						    dotOrIndexer*
						  ;

primaryExpression         : leftParen! expression RIGHTPAREN!
                          | value
						  ;

value                     : function //must be before variableName
						  | bankVariableName						  
						  | Integer -> ^(ASTINTEGER Integer)
						  | (leftBracketNoGlue|leftBracketNoGlueWild) indexerExpressionHelper RIGHTBRACKET -> ^(ASTINDEXERALONE indexerExpressionHelper) //also see rule indexerExpression
						  | double2 -> double2						
						  | date2 -> ^(ASTDATE2 date2) //a date like: 2001q3 (luckily we do not have 'e' freq, then what about 2012e3 (in principle, = 2012000))
						  | StringInQuotes -> ^(ASTSTRINGINQUOTES StringInQuotes)
						  | listFile						
						  | matrix
						  ;

// ------------------------------------------------------------------------------------------------------------------
// ------------------- expression END -------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------
						  
dotOrIndexer              : GLUEDOT DOT dotHelper -> ^(ASTDOT dotHelper)
						  | leftBracketGlue indexerExpressionHelper2 RIGHTBRACKET -> ^(ASTINDEXER indexerExpressionHelper2)
						  ;

						  //just like b1:fy~q, we can use #m.fy~q, where fy~q is the variableName.
dotHelper				  : variableName | function | Integer;
indexerExpressionHelper2  : (indexerExpressionHelper (',' indexerExpressionHelper)*) -> indexerExpressionHelper+;

matrix                    : matrixCol;
matrixCol                 : leftBracketNoGlue matrixRow (doubleVerticalBar matrixRow)* RIGHTBRACKET -> ^(ASTMATRIXCOL matrixRow+);
matrixRow                 : expression (',' expression)*  -> ^(ASTMATRIXROW expression+);

//FIXME
//FIXME
//FIXME ident -> fileName
//FIXME
//FIXME
listFile                  : HASH leftParenGlue LISTFILE ident RIGHTPAREN -> ^(ASTLISTFILE ident);

function                  : ident leftParenGlue (expression (',' expression)*)? RIGHTPAREN -> ^(ASTFUNCTION ident expression*);
					  
dollarConditional         : LEFTPAREN logicalOr RIGHTPAREN -> ^(ASTDOLLARCONDITIONAL logicalOr)  //logicalOr can contain a listWithIndexer
						  | variableWithIndexer -> ^(ASTDOLLARCONDITIONALVARIABLE variableWithIndexer)  //does not need parenthesis						
						  ; 

variableWithIndexer       : variableName ( leftBracketGlue expression RIGHTBRACKET ) -> ^(ASTCOMPARE2 variableName expression);    //should catch #i0[#i] or #i0['a'], does not need a parenthesis!  //should catch #i0[#i], does not need a parenthesis!						
					  
indexerExpressionHelper   : expressionOrNothing doubleDot expressionOrNothing -> ^(ASTINDEXERELEMENT expressionOrNothing expressionOrNothing)     //'fm1'..'fm5'
						  | expression -> ^(ASTINDEXERELEMENT expression)                                     //'fm*' or -2 or 2000 or 2010q3
						  | PLUS expression -> ^(ASTINDEXERELEMENTPLUS expression)                            //+1   
                          ;

expressionOrNothing       : expression -> expression
						  | -> ASTEMPTYRANGEELEMENT
						  ;


// ------------------------------------------------------------------------------------------------------------------
// ------------------- name START -------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

						  //name is without sigil, name is in principle just like characters, excluding sigils. Kind of like an advanced ident.
name                      : (ident | nameCurlyStart) 
							(nameCurly | GLUE! sigilOrVertical? identDigit)*
						  ;

nameCurlyStart            : leftCurlyNoGlue ident RIGHTCURLY -> ^(ASTCURLYSIMPLE ident)
					      | leftCurlyNoGlue expression RIGHTCURLY -> ^(ASTCURLY expression)
						  ;

nameCurly                 : leftCurlyGlue ident RIGHTCURLY -> ^(ASTCURLYSIMPLE ident)
					      | leftCurlyGlue expression RIGHTCURLY -> ^(ASTCURLY expression)
						  ;

						  //includes sigil and freq
variableName              : sigil ident freq? -> ^(ASTVARIABLENAME ^(ASTPLACEHOLDER sigil) ^(ASTPLACEHOLDER ident) ^(ASTPLACEHOLDER freq?))
						  | sigil leftParen name rightParen freq? -> ^(ASTVARIABLENAME ^(ASTPLACEHOLDER sigil) ^(ASTPLACEHOLDER name) ^(ASTPLACEHOLDER freq?))
						  | ident freq? -> ^(ASTVARIABLENAME ^(ASTPLACEHOLDER) ^(ASTPLACEHOLDER ident) ^(ASTPLACEHOLDER freq?))						  
						  | name freq? -> ^(ASTVARIABLENAME ^(ASTPLACEHOLDER) ^(ASTPLACEHOLDER name) ^(ASTPLACEHOLDER freq?))
						  ;

bankVariableName          : (name COLON)? variableName -> ^(ASTBANKVARIABLENAME ^(ASTPLACEHOLDER name?) ^(ASTPLACEHOLDER variableName));

// ------------------------------------------------------------------------------------------------------------------
// ------------------- name END -------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

sigil                     : HASH GLUE -> ASTHASH
						  | PERCENT GLUE -> ASTPERCENT
						  ;

sigilOrVertical           : sigil
						  | VERTICALBAR -> ASTVERTICALBAR  //does not have glue after
						  ;

freq			   		  : GLUE TILDE GLUE name -> name;

// ------------------------------------------------------------------------------------------------------------------
// ------------------- logical START -------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

logicalOr				  : (logicalAnd -> logicalAnd)
							(OR lbla=logicalAnd -> ^(ASTOR $logicalOr $lbla))*  
						  ;

logicalAnd				  : (logicalNot -> logicalNot)
							(AND lbla=logicalNot -> ^(ASTAND $logicalAnd $lbla))*
						  ;

logicalNot				  :  NOT logicalAtom     -> ^(ASTNOT logicalAtom)
						  |  logicalAtom
						  ;

logicalAtom				  :  expression ifOperator expression -> ^(ASTCOMPARE ifOperator expression expression)
						  |  leftParen! logicalOr rightParen!           // omit both '(' and ')'
						  |  variableWithIndexer						  
						  ;

ifOperator		          :  ISEQUAL -> ^(ASTIFOPERATOR ASTIFOPERATOR1)
						  |  ISNOTQUAL -> ^(ASTIFOPERATOR ASTIFOPERATOR2)
						  |  RIGHTANGLE -> ^(ASTIFOPERATOR ASTIFOPERATOR3)
						  |  leftAngle -> ^(ASTIFOPERATOR ASTIFOPERATOR4)
			              |  ISLARGEROREQUAL -> ^(ASTIFOPERATOR ASTIFOPERATOR5)
						  |  ISSMALLEROREQUAL -> ^(ASTIFOPERATOR ASTIFOPERATOR6)
						  |  IN -> ^(ASTIFOPERATOR ASTIFOPERATOR7)
			              ;

// ------------------------------------------------------------------------------------------------------------------
// ------------------- logical END -------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------





leftParen                 : (GLUE!)? LEFTPAREN;
leftParenGlue             : GLUE! LEFTPAREN;
leftBracketGlue           : LEFTBRACKETGLUE;
star                      : (GLUESTAR!)? STAR (GLUESTAR!)?;
pow                       : stars -> ASTPOW
                          | HAT -> ASTPOW
                          ;

leftAngle                 : leftAngle2 | leftAngleNo2;
leftAngle2				  : LEFTANGLESPECIAL;
leftAngleNo2	          : LEFTANGLESIMPLE; 

rightParen                : RIGHTPAREN (GLUE!)?;

stars                     : (GLUESTAR!)? STARS (GLUESTAR!)?;

leftBracketNoGlue         : LEFTBRACKET;
leftBracketNoGlueWild     : LEFTBRACKETWILD; 

identDigit                : identDigitHelper -> ^(ASTIDENTDIGIT identDigitHelper);
identDigitHelper
						  : ident                 //for instance ab27
						  | Integer               //for instance 0123
						  | DigitsEDigits         //for instance 25e12 (will end here, not in IdentStartingWithInt)
						  | DateDef               //for instance 2012q3 (will end here, not in IdentStartingWithInt)						  						
						  | IdentStartingWithInt  //for instance 0123ab27 (catches the rest of these cases)						  						
						  ;			
						
leftCurly                 : (GLUE!)? LEFTCURLY;
leftCurlyGlue             : GLUE! LEFTCURLY;
leftCurlyNoGlue           : LEFTCURLY;

doubleVerticalBar         : GLUE? (DOUBLEVERTICALBAR1 | DOUBLEVERTICALBAR2);

doubleDot                 : GLUEDOT? DOT GLUEDOT DOT;

double2                   : double2Helper -> ^(ASTDOUBLE double2Helper);
double2Helper             : Double            //0.123 or 25e+12
						  | DigitsEDigits     //for instance 25e12 which can also be a name chunk.
						  ;

date2                     : Integer | DateDef;

ident                     : ident2 -> ^(ASTIDENT ident2);

//fixme
//fixme
//fixme
//fixme
//fixme
ident2 					  : Ident
						  | NOT
						  ;


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
GLUE                      : '¨';
GLUEDOT                   : '£';  //only relevant for '.', for instance a.b becomes a£.b, and x.1 becomes x£.1
GLUEDOTNUMBER             : '§';  //only relevant for '.', for instance 12.34 becomes 12§.34
GLUESTAR                  : '½';  //only relevant for '*' and '?', for instance a*b --> a½*½b
LEFTANGLESPECIAL          : '<=<';  //indicates that there are two idents following the '<' in the text input.
                                    // using <_< is not good, since it stumbles on mulprt<_lev>xx
//MOD                       : '¤';  //does not work with '%¨%' ================> NOT DONE YET!!
GLUEBACKSLASH             : '¨\\';
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

