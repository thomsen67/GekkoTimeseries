// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 GAMS.g 2022-04-20 16:58:19

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;


namespace  Gekko 
{
public partial class GAMSLexer : Lexer {
    public const int ASTFUNCTIONELEMENTS1 = 61;
    public const int ASTPOW = 22;
    public const int COMMENT1 = 118;
    public const int COMMENT2 = 119;
    public const int ASTFUNCTION3 = 36;
    public const int ASTFUNCTION2 = 35;
    public const int ASTVARIABLE = 23;
    public const int LETTER = 125;
    public const int MOD = 122;
    public const int ASTFUNCTION1 = 34;
    public const int ASTIDX = 40;
    public const int ASTFUNCTION0 = 58;
    public const int LOG = 76;
    public const int EQUATIONS = 86;
    public const int ASTINDEXES1 = 25;
    public const int ASTCONDITIONAL = 32;
    public const int DOUBLEDOT = 87;
    public const int NOT = 73;
    public const int ASTINDEXES3 = 27;
    public const int ASTINDEXES2 = 26;
    public const int ASTVAR = 37;
    public const int EOF = -1;
    public const int NONEQUAL = 107;
    public const int ASTINTEGER = 20;
    public const int TANH = 81;
    public const int ASTEQU = 12;
    public const int EXP = 75;
    public const int Comment = 116;
    public const int ASTDOLLAREXPRESSION = 56;
    public const int EEQUAL = 88;
    public const int SQR = 80;
    public const int GREATERTHANOREQUAL = 109;
    public const int ASTFUNCTIONELEMENTS0 = 60;
    public const int GREATERTHAN = 111;
    public const int ASTEQU1 = 42;
    public const int Double = 112;
    public const int D = 130;
    public const int ASTEQU2 = 43;
    public const int E = 131;
    public const int F = 132;
    public const int G = 133;
    public const int ASTEQU0 = 41;
    public const int A = 127;
    public const int B = 128;
    public const int ASTEXPR = 5;
    public const int ASTEQU3 = 44;
    public const int C = 129;
    public const int L = 138;
    public const int M = 139;
    public const int N = 140;
    public const int NESTED_ML_COMMENT = 117;
    public const int ASTVARWI4 = 50;
    public const int O = 141;
    public const int ASTVARWI3 = 49;
    public const int H = 134;
    public const int ASTVARWI2 = 48;
    public const int ASTFUNCTIONELEMENTS = 59;
    public const int I = 135;
    public const int ASTVARWISIMPLE = 7;
    public const int ASTVARWI1 = 47;
    public const int J = 136;
    public const int NEWLINE2 = 114;
    public const int ASTVARWI0 = 46;
    public const int K = 137;
    public const int NEWLINE3 = 115;
    public const int U = 147;
    public const int T = 146;
    public const int W = 149;
    public const int WHITESPACE = 120;
    public const int POWER = 79;
    public const int V = 148;
    public const int Q = 143;
    public const int P = 142;
    public const int S = 145;
    public const int R = 144;
    public const int MULT = 105;
    public const int ASTVARWI = 45;
    public const int Y = 151;
    public const int ASTIDXELEMENTS1 = 53;
    public const int X = 150;
    public const int ASTIDXELEMENTS0 = 54;
    public const int Z = 152;
    public const int ABS = 74;
    public const int Ident = 113;
    public const int ASTEXPRESSION = 15;
    public const int OR = 72;
    public const int StringInQuotes = 100;
    public const int ASTSUM = 33;
    public const int ASTDEFINITION = 31;
    public const int SOLVE = 83;
    public const int DOLLAR = 104;
    public const int ASTFUNCTION = 19;
    public const int ASTEQUCODE = 14;
    public const int MAX = 77;
    public const int Exponent = 124;
    public const int R2 = 97;
    public const int R3 = 99;
    public const int AND = 71;
    public const int SUM = 70;
    public const int COMMA = 91;
    public const int R1 = 95;
    public const int EQUAL = 90;
    public const int ASTSIMPLEFUNCTION1 = 16;
    public const int ASTSIMPLEFUNCTION2 = 17;
    public const int ASTSIMPLEFUNCTION3 = 18;
    public const int LESSTHANOREQUAL = 108;
    public const int MODEL = 82;
    public const int ASTEND = 24;
    public const int PLUS = 101;
    public const int DIGIT = 123;
    public const int ASTSUMCONTROLLED = 67;
    public const int DOT = 93;
    public const int ASTSUMCONTROLLEDSIMPLE = 66;
    public const int ASTGAMS = 6;
    public const int ASTEXPRESSION2 = 29;
    public const int ASTIDXELEMENTS = 52;
    public const int ASTEXPRESSION3 = 30;
    public const int ASTVALUE = 57;
    public const int LESSTHAN = 110;
    public const int ASTVARIABLEWITHINDEXERETC = 38;
    public const int ASTVARDEF = 8;
    public const int ASTIDX0 = 51;
    public const int ASTEXPRESSION1 = 28;
    public const int NEGATE = 4;
    public const int SAMEAS = 84;
    public const int MIN = 78;
    public const int MINUS = 103;
    public const int ASTVARDEF0 = 9;
    public const int ASTVARIABLEANDLEAD = 55;
    public const int ASTVARDEF1 = 10;
    public const int SEMI = 89;
    public const int ASTVARDEF2 = 11;
    public const int L1 = 94;
    public const int ASTSUM0 = 62;
    public const int L2 = 96;
    public const int L3 = 98;
    public const int ASTLEFTSIDE = 13;
    public const int NEWLINE = 126;
    public const int ASTSUMCONTROLLED2 = 69;
    public const int ASTSUM2 = 64;
    public const int ASTSUM1 = 63;
    public const int ASTSUMCONTROLLED0 = 68;
    public const int VARIABLES = 85;
    public const int ASTSUM3 = 65;
    public const int EQU = 121;
    public const int STARS = 106;
    public const int ASTDOUBLE = 21;
    public const int DIV = 92;
    public const int Integer = 102;
    public const int ASTDOT = 39;

      public override void ReportError(RecognitionException e) {
            string hdr = GetErrorHeader(e);
            string msg = "GAMS lexer error: " + e.Message;
            throw new Exception(e.Line + "¤" + e.CharPositionInLine + "¤" + hdr + "¤" + msg);
      } 
      
      public static System.Collections.Generic.Dictionary<string, int> kw = GetKw();

      public static System.Collections.Generic.Dictionary<string, int> GetKw()
      {
         System.Collections.Generic.Dictionary<string, int> d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);     
    	 d.Add("EQU"                    ,   EQU                     );
    	 d.Add("SUM"                    ,   SUM                     );
    	 d.Add("OR"                    ,   OR                     );
    	 d.Add("AND"                    ,   AND                     );
    	 d.Add("NOT"                    ,   NOT                     );
    	 d.Add("ABS"                    ,   ABS                     );
    	 d.Add("EXP"                    ,   EXP                     );
    	 d.Add("LOG"                    ,   LOG                     );
    	 d.Add("MAX"                    ,   MAX                     );
    	 d.Add("MIN"                    ,   MIN                     );
    	 d.Add("POWER"                    ,   POWER                     );
    	 d.Add("SQR"                    ,   SQR                     );
    	 d.Add("TANH"                    ,   TANH                     );
    	 d.Add("MODEL"                    ,   MODEL                     );
    	 d.Add("SOLVE"                    ,   SOLVE                     );
    	 d.Add("VARIABLES"                    , VARIABLES                       );
    	 d.Add("EQUATIONS"                    ,  EQUATIONS                      );
    	 d.Add("SAMEAS"                    ,   SAMEAS                     );

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


    // delegates
    // delegators

    public GAMSLexer() 
    {
		InitializeCyclicDFAs();
    }
    public GAMSLexer(ICharStream input)
		: this(input, null) {
    }
    public GAMSLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "GAMS.g";} 
    }

    // $ANTLR start "Comment"
    public void mComment() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Comment;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:348:12: ( ( '//' ) (~ ( NEWLINE2 | NEWLINE3 ) )* )
            // GAMS.g:348:14: ( '//' ) (~ ( NEWLINE2 | NEWLINE3 ) )*
            {
            	// GAMS.g:348:14: ( '//' )
            	// GAMS.g:348:15: '//'
            	{
            		Match("//"); 


            	}

            	// GAMS.g:348:21: (~ ( NEWLINE2 | NEWLINE3 ) )*
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( ((LA1_0 >= '\u0000' && LA1_0 <= '\t') || (LA1_0 >= '\u000B' && LA1_0 <= '\uFFFF')) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // GAMS.g:348:22: ~ ( NEWLINE2 | NEWLINE3 )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\uFFFF') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop1;
            	    }
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements

            	_channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "Comment"

    // $ANTLR start "NESTED_ML_COMMENT"
    public void mNESTED_ML_COMMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NESTED_ML_COMMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:349:18: ( '/*' ( options {greedy=false; } : NESTED_ML_COMMENT | . )* '*/' )
            // GAMS.g:349:20: '/*' ( options {greedy=false; } : NESTED_ML_COMMENT | . )* '*/'
            {
            	Match("/*"); 

            	// GAMS.g:349:25: ( options {greedy=false; } : NESTED_ML_COMMENT | . )*
            	do 
            	{
            	    int alt2 = 3;
            	    int LA2_0 = input.LA(1);

            	    if ( (LA2_0 == '*') )
            	    {
            	        int LA2_1 = input.LA(2);

            	        if ( (LA2_1 == '/') )
            	        {
            	            alt2 = 3;
            	        }
            	        else if ( ((LA2_1 >= '\u0000' && LA2_1 <= '.') || (LA2_1 >= '0' && LA2_1 <= '\uFFFF')) )
            	        {
            	            alt2 = 2;
            	        }


            	    }
            	    else if ( (LA2_0 == '/') )
            	    {
            	        int LA2_2 = input.LA(2);

            	        if ( (LA2_2 == '*') )
            	        {
            	            alt2 = 1;
            	        }
            	        else if ( ((LA2_2 >= '\u0000' && LA2_2 <= ')') || (LA2_2 >= '+' && LA2_2 <= '\uFFFF')) )
            	        {
            	            alt2 = 2;
            	        }


            	    }
            	    else if ( ((LA2_0 >= '\u0000' && LA2_0 <= ')') || (LA2_0 >= '+' && LA2_0 <= '.') || (LA2_0 >= '0' && LA2_0 <= '\uFFFF')) )
            	    {
            	        alt2 = 2;
            	    }


            	    switch (alt2) 
            		{
            			case 1 :
            			    // GAMS.g:349:52: NESTED_ML_COMMENT
            			    {
            			    	mNESTED_ML_COMMENT(); 

            			    }
            			    break;
            			case 2 :
            			    // GAMS.g:349:72: .
            			    {
            			    	MatchAny(); 

            			    }
            			    break;

            			default:
            			    goto loop2;
            	    }
            	} while (true);

            	loop2:
            		;	// Stops C# compiler whining that label 'loop2' has no statements

            	Match("*/"); 

            	_channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NESTED_ML_COMMENT"

    // $ANTLR start "COMMENT1"
    public void mCOMMENT1() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMENT1;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:351:9: ( ( '#' ) (~ ( NEWLINE2 | NEWLINE3 ) )* )
            // GAMS.g:351:30: ( '#' ) (~ ( NEWLINE2 | NEWLINE3 ) )*
            {
            	// GAMS.g:351:30: ( '#' )
            	// GAMS.g:351:31: '#'
            	{
            		Match('#'); 

            	}

            	// GAMS.g:351:36: (~ ( NEWLINE2 | NEWLINE3 ) )*
            	do 
            	{
            	    int alt3 = 2;
            	    int LA3_0 = input.LA(1);

            	    if ( ((LA3_0 >= '\u0000' && LA3_0 <= '\t') || (LA3_0 >= '\u000B' && LA3_0 <= '\uFFFF')) )
            	    {
            	        alt3 = 1;
            	    }


            	    switch (alt3) 
            		{
            			case 1 :
            			    // GAMS.g:351:37: ~ ( NEWLINE2 | NEWLINE3 )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\uFFFF') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop3;
            	    }
            	} while (true);

            	loop3:
            		;	// Stops C# compiler whining that label 'loop3' has no statements

            	 _channel=HIDDEN; 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMMENT1"

    // $ANTLR start "COMMENT2"
    public void mCOMMENT2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMENT2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:352:9: ( ( '!!' ) (~ ( NEWLINE2 | NEWLINE3 ) )* )
            // GAMS.g:352:30: ( '!!' ) (~ ( NEWLINE2 | NEWLINE3 ) )*
            {
            	// GAMS.g:352:30: ( '!!' )
            	// GAMS.g:352:31: '!!'
            	{
            		Match("!!"); 


            	}

            	// GAMS.g:352:37: (~ ( NEWLINE2 | NEWLINE3 ) )*
            	do 
            	{
            	    int alt4 = 2;
            	    int LA4_0 = input.LA(1);

            	    if ( ((LA4_0 >= '\u0000' && LA4_0 <= '\t') || (LA4_0 >= '\u000B' && LA4_0 <= '\uFFFF')) )
            	    {
            	        alt4 = 1;
            	    }


            	    switch (alt4) 
            		{
            			case 1 :
            			    // GAMS.g:352:38: ~ ( NEWLINE2 | NEWLINE3 )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\uFFFF') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop4;
            	    }
            	} while (true);

            	loop4:
            		;	// Stops C# compiler whining that label 'loop4' has no statements

            	 _channel=HIDDEN; 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMMENT2"

    // $ANTLR start "WHITESPACE"
    public void mWHITESPACE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WHITESPACE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:354:12: ( ( '\\t' | ' ' | '\\u000C' )+ )
            // GAMS.g:354:14: ( '\\t' | ' ' | '\\u000C' )+
            {
            	// GAMS.g:354:14: ( '\\t' | ' ' | '\\u000C' )+
            	int cnt5 = 0;
            	do 
            	{
            	    int alt5 = 2;
            	    int LA5_0 = input.LA(1);

            	    if ( (LA5_0 == '\t' || LA5_0 == '\f' || LA5_0 == ' ') )
            	    {
            	        alt5 = 1;
            	    }


            	    switch (alt5) 
            		{
            			case 1 :
            			    // GAMS.g:
            			    {
            			    	if ( input.LA(1) == '\t' || input.LA(1) == '\f' || input.LA(1) == ' ' ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    if ( cnt5 >= 1 ) goto loop5;
            		            EarlyExitException eee5 =
            		                new EarlyExitException(5, input);
            		            throw eee5;
            	    }
            	    cnt5++;
            	} while (true);

            	loop5:
            		;	// Stops C# compiler whining that label 'loop5' has no statements

            	 _channel=HIDDEN; 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WHITESPACE"

    // $ANTLR start "STARS"
    public void mSTARS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STARS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:357:10: ( '**' )
            // GAMS.g:357:17: '**'
            {
            	Match("**"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "STARS"

    // $ANTLR start "MULT"
    public void mMULT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MULT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:358:6: ( '*' )
            // GAMS.g:358:8: '*'
            {
            	Match('*'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MULT"

    // $ANTLR start "EQU"
    public void mEQU() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EQU;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:360:5: ( 'EQU' )
            // GAMS.g:360:7: 'EQU'
            {
            	Match("EQU"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EQU"

    // $ANTLR start "SUM"
    public void mSUM() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SUM;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:361:5: ( 'SUM' )
            // GAMS.g:361:7: 'SUM'
            {
            	Match("SUM"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SUM"

    // $ANTLR start "OR"
    public void mOR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = OR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:362:3: ( 'OR' )
            // GAMS.g:362:5: 'OR'
            {
            	Match("OR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "OR"

    // $ANTLR start "AND"
    public void mAND() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AND;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:363:4: ( 'AND' )
            // GAMS.g:363:6: 'AND'
            {
            	Match("AND"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "AND"

    // $ANTLR start "NOT"
    public void mNOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:364:4: ( 'NOT' )
            // GAMS.g:364:6: 'NOT'
            {
            	Match("NOT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NOT"

    // $ANTLR start "ABS"
    public void mABS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ABS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:365:4: ( 'ABS' )
            // GAMS.g:365:6: 'ABS'
            {
            	Match("ABS"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ABS"

    // $ANTLR start "EXP"
    public void mEXP() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EXP;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:366:4: ( 'EXP' )
            // GAMS.g:366:6: 'EXP'
            {
            	Match("EXP"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EXP"

    // $ANTLR start "LOG"
    public void mLOG() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LOG;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:367:4: ( 'LOG' )
            // GAMS.g:367:6: 'LOG'
            {
            	Match("LOG"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LOG"

    // $ANTLR start "MAX"
    public void mMAX() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAX;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:368:4: ( 'MAX' )
            // GAMS.g:368:6: 'MAX'
            {
            	Match("MAX"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAX"

    // $ANTLR start "MIN"
    public void mMIN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MIN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:369:4: ( 'MIN' )
            // GAMS.g:369:6: 'MIN'
            {
            	Match("MIN"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MIN"

    // $ANTLR start "POWER"
    public void mPOWER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = POWER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:370:6: ( 'POWER' )
            // GAMS.g:370:8: 'POWER'
            {
            	Match("POWER"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "POWER"

    // $ANTLR start "SQR"
    public void mSQR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SQR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:371:4: ( 'SQR' )
            // GAMS.g:371:6: 'SQR'
            {
            	Match("SQR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SQR"

    // $ANTLR start "SAMEAS"
    public void mSAMEAS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SAMEAS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:372:7: ( 'SAMEAS' )
            // GAMS.g:372:9: 'SAMEAS'
            {
            	Match("SAMEAS"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SAMEAS"

    // $ANTLR start "TANH"
    public void mTANH() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TANH;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:373:5: ( 'TANH' )
            // GAMS.g:373:7: 'TANH'
            {
            	Match("TANH"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "TANH"

    // $ANTLR start "MODEL"
    public void mMODEL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MODEL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:374:6: ( 'MODEL' )
            // GAMS.g:374:8: 'MODEL'
            {
            	Match("MODEL"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MODEL"

    // $ANTLR start "SOLVE"
    public void mSOLVE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SOLVE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:375:6: ( 'SOLVE' )
            // GAMS.g:375:8: 'SOLVE'
            {
            	Match("SOLVE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SOLVE"

    // $ANTLR start "VARIABLES"
    public void mVARIABLES() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VARIABLES;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:376:10: ( 'VARIABLES' )
            // GAMS.g:376:12: 'VARIABLES'
            {
            	Match("VARIABLES"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VARIABLES"

    // $ANTLR start "EQUATIONS"
    public void mEQUATIONS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EQUATIONS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:377:10: ( 'EQUATIONS' )
            // GAMS.g:377:12: 'EQUATIONS'
            {
            	Match("EQUATIONS"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EQUATIONS"

    // $ANTLR start "DOT"
    public void mDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:379:9: ( '.' )
            // GAMS.g:379:11: '.'
            {
            	Match('.'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DOT"

    // $ANTLR start "PLUS"
    public void mPLUS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = PLUS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:380:6: ( '+' )
            // GAMS.g:380:8: '+'
            {
            	Match('+'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "PLUS"

    // $ANTLR start "MINUS"
    public void mMINUS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MINUS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:381:7: ( '-' )
            // GAMS.g:381:9: '-'
            {
            	Match('-'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MINUS"

    // $ANTLR start "SEMI"
    public void mSEMI() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SEMI;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:382:6: ( ';' )
            // GAMS.g:382:8: ';'
            {
            	Match(';'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SEMI"

    // $ANTLR start "DIV"
    public void mDIV() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DIV;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:383:5: ( '/' )
            // GAMS.g:383:7: '/'
            {
            	Match('/'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DIV"

    // $ANTLR start "MOD"
    public void mMOD() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MOD;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:384:5: ( '%' )
            // GAMS.g:384:7: '%'
            {
            	Match('%'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MOD"

    // $ANTLR start "COMMA"
    public void mCOMMA() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMA;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:385:7: ( ',' )
            // GAMS.g:385:9: ','
            {
            	Match(','); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMMA"

    // $ANTLR start "EEQUAL"
    public void mEEQUAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EEQUAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:387:8: ( '=e=' | '=E=' )
            int alt6 = 2;
            int LA6_0 = input.LA(1);

            if ( (LA6_0 == '=') )
            {
                int LA6_1 = input.LA(2);

                if ( (LA6_1 == 'e') )
                {
                    alt6 = 1;
                }
                else if ( (LA6_1 == 'E') )
                {
                    alt6 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d6s1 =
                        new NoViableAltException("", 6, 1, input);

                    throw nvae_d6s1;
                }
            }
            else 
            {
                NoViableAltException nvae_d6s0 =
                    new NoViableAltException("", 6, 0, input);

                throw nvae_d6s0;
            }
            switch (alt6) 
            {
                case 1 :
                    // GAMS.g:387:10: '=e='
                    {
                    	Match("=e="); 


                    }
                    break;
                case 2 :
                    // GAMS.g:387:18: '=E='
                    {
                    	Match("=E="); 


                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EEQUAL"

    // $ANTLR start "NONEQUAL"
    public void mNONEQUAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NONEQUAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:388:9: ( '<>' )
            // GAMS.g:388:11: '<>'
            {
            	Match("<>"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NONEQUAL"

    // $ANTLR start "LESSTHANOREQUAL"
    public void mLESSTHANOREQUAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LESSTHANOREQUAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:389:16: ( '<=' )
            // GAMS.g:389:18: '<='
            {
            	Match("<="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LESSTHANOREQUAL"

    // $ANTLR start "GREATERTHANOREQUAL"
    public void mGREATERTHANOREQUAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = GREATERTHANOREQUAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:390:19: ( '>=' )
            // GAMS.g:390:21: '>='
            {
            	Match(">="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "GREATERTHANOREQUAL"

    // $ANTLR start "EQUAL"
    public void mEQUAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EQUAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:391:7: ( '=' )
            // GAMS.g:391:9: '='
            {
            	Match('='); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EQUAL"

    // $ANTLR start "LESSTHAN"
    public void mLESSTHAN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LESSTHAN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:392:9: ( '<' )
            // GAMS.g:392:11: '<'
            {
            	Match('<'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LESSTHAN"

    // $ANTLR start "GREATERTHAN"
    public void mGREATERTHAN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = GREATERTHAN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:393:12: ( '>' )
            // GAMS.g:393:14: '>'
            {
            	Match('>'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "GREATERTHAN"

    // $ANTLR start "DOUBLEDOT"
    public void mDOUBLEDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOUBLEDOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:395:11: ( '..' )
            // GAMS.g:395:13: '..'
            {
            	Match(".."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DOUBLEDOT"

    // $ANTLR start "Integer"
    public void mInteger() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Integer;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:397:9: ( ( DIGIT )+ )
            // GAMS.g:397:11: ( DIGIT )+
            {
            	// GAMS.g:397:11: ( DIGIT )+
            	int cnt7 = 0;
            	do 
            	{
            	    int alt7 = 2;
            	    int LA7_0 = input.LA(1);

            	    if ( ((LA7_0 >= '0' && LA7_0 <= '9')) )
            	    {
            	        alt7 = 1;
            	    }


            	    switch (alt7) 
            		{
            			case 1 :
            			    // GAMS.g:397:12: DIGIT
            			    {
            			    	mDIGIT(); 

            			    }
            			    break;

            			default:
            			    if ( cnt7 >= 1 ) goto loop7;
            		            EarlyExitException eee7 =
            		                new EarlyExitException(7, input);
            		            throw eee7;
            	    }
            	    cnt7++;
            	} while (true);

            	loop7:
            		;	// Stops C# compiler whining that label 'loop7' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "Integer"

    // $ANTLR start "Double"
    public void mDouble() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Double;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:400:5: ( ( '0' .. '9' )+ DOT ( '0' .. '9' )* ( Exponent )? | DOT ( '0' .. '9' )+ ( Exponent )? | ( '0' .. '9' )+ Exponent )
            int alt14 = 3;
            alt14 = dfa14.Predict(input);
            switch (alt14) 
            {
                case 1 :
                    // GAMS.g:401:8: ( '0' .. '9' )+ DOT ( '0' .. '9' )* ( Exponent )?
                    {
                    	// GAMS.g:401:8: ( '0' .. '9' )+
                    	int cnt8 = 0;
                    	do 
                    	{
                    	    int alt8 = 2;
                    	    int LA8_0 = input.LA(1);

                    	    if ( ((LA8_0 >= '0' && LA8_0 <= '9')) )
                    	    {
                    	        alt8 = 1;
                    	    }


                    	    switch (alt8) 
                    		{
                    			case 1 :
                    			    // GAMS.g:401:9: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt8 >= 1 ) goto loop8;
                    		            EarlyExitException eee8 =
                    		                new EarlyExitException(8, input);
                    		            throw eee8;
                    	    }
                    	    cnt8++;
                    	} while (true);

                    	loop8:
                    		;	// Stops C# compiler whining that label 'loop8' has no statements

                    	mDOT(); 
                    	// GAMS.g:401:26: ( '0' .. '9' )*
                    	do 
                    	{
                    	    int alt9 = 2;
                    	    int LA9_0 = input.LA(1);

                    	    if ( ((LA9_0 >= '0' && LA9_0 <= '9')) )
                    	    {
                    	        alt9 = 1;
                    	    }


                    	    switch (alt9) 
                    		{
                    			case 1 :
                    			    // GAMS.g:401:27: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop9;
                    	    }
                    	} while (true);

                    	loop9:
                    		;	// Stops C# compiler whining that label 'loop9' has no statements

                    	// GAMS.g:401:40: ( Exponent )?
                    	int alt10 = 2;
                    	int LA10_0 = input.LA(1);

                    	if ( (LA10_0 == 'E' || LA10_0 == 'e') )
                    	{
                    	    alt10 = 1;
                    	}
                    	switch (alt10) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:401:40: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // GAMS.g:402:9: DOT ( '0' .. '9' )+ ( Exponent )?
                    {
                    	mDOT(); 
                    	// GAMS.g:402:13: ( '0' .. '9' )+
                    	int cnt11 = 0;
                    	do 
                    	{
                    	    int alt11 = 2;
                    	    int LA11_0 = input.LA(1);

                    	    if ( ((LA11_0 >= '0' && LA11_0 <= '9')) )
                    	    {
                    	        alt11 = 1;
                    	    }


                    	    switch (alt11) 
                    		{
                    			case 1 :
                    			    // GAMS.g:402:15: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt11 >= 1 ) goto loop11;
                    		            EarlyExitException eee11 =
                    		                new EarlyExitException(11, input);
                    		            throw eee11;
                    	    }
                    	    cnt11++;
                    	} while (true);

                    	loop11:
                    		;	// Stops C# compiler whining that label 'loop11' has no statements

                    	// GAMS.g:402:29: ( Exponent )?
                    	int alt12 = 2;
                    	int LA12_0 = input.LA(1);

                    	if ( (LA12_0 == 'E' || LA12_0 == 'e') )
                    	{
                    	    alt12 = 1;
                    	}
                    	switch (alt12) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:402:29: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 3 :
                    // GAMS.g:403:8: ( '0' .. '9' )+ Exponent
                    {
                    	// GAMS.g:403:8: ( '0' .. '9' )+
                    	int cnt13 = 0;
                    	do 
                    	{
                    	    int alt13 = 2;
                    	    int LA13_0 = input.LA(1);

                    	    if ( ((LA13_0 >= '0' && LA13_0 <= '9')) )
                    	    {
                    	        alt13 = 1;
                    	    }


                    	    switch (alt13) 
                    		{
                    			case 1 :
                    			    // GAMS.g:403:9: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt13 >= 1 ) goto loop13;
                    		            EarlyExitException eee13 =
                    		                new EarlyExitException(13, input);
                    		            throw eee13;
                    	    }
                    	    cnt13++;
                    	} while (true);

                    	loop13:
                    		;	// Stops C# compiler whining that label 'loop13' has no statements

                    	mExponent(); 

                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "Double"

    // $ANTLR start "Ident"
    public void mIdent() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Ident;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:406:7: ( ( LETTER | '_' ) ( DIGIT | LETTER | '_' )* )
            // GAMS.g:406:9: ( LETTER | '_' ) ( DIGIT | LETTER | '_' )*
            {
            	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// GAMS.g:406:22: ( DIGIT | LETTER | '_' )*
            	do 
            	{
            	    int alt15 = 2;
            	    int LA15_0 = input.LA(1);

            	    if ( ((LA15_0 >= '0' && LA15_0 <= '9') || (LA15_0 >= 'A' && LA15_0 <= 'Z') || LA15_0 == '_' || (LA15_0 >= 'a' && LA15_0 <= 'z')) )
            	    {
            	        alt15 = 1;
            	    }


            	    switch (alt15) 
            		{
            			case 1 :
            			    // GAMS.g:
            			    {
            			    	if ( (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop15;
            	    }
            	} while (true);

            	loop15:
            		;	// Stops C# compiler whining that label 'loop15' has no statements

            	 _type = CheckKeywordsTable(Text); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "Ident"

    // $ANTLR start "NEWLINE"
    public void mNEWLINE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NEWLINE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:408:12: ( ( ( '\\r' )? '\\n' ) )
            // GAMS.g:408:16: ( ( '\\r' )? '\\n' )
            {
            	// GAMS.g:408:16: ( ( '\\r' )? '\\n' )
            	// GAMS.g:408:17: ( '\\r' )? '\\n'
            	{
            		// GAMS.g:408:17: ( '\\r' )?
            		int alt16 = 2;
            		int LA16_0 = input.LA(1);

            		if ( (LA16_0 == '\r') )
            		{
            		    alt16 = 1;
            		}
            		switch (alt16) 
            		{
            		    case 1 :
            		        // GAMS.g:408:18: '\\r'
            		        {
            		        	Match('\r'); 

            		        }
            		        break;

            		}

            		Match('\n'); 

            	}

            	 _channel=HIDDEN; 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NEWLINE"

    // $ANTLR start "L1"
    public void mL1() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = L1;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:410:4: ( '(' )
            // GAMS.g:410:6: '('
            {
            	Match('('); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "L1"

    // $ANTLR start "R1"
    public void mR1() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = R1;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:411:4: ( ')' )
            // GAMS.g:411:6: ')'
            {
            	Match(')'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "R1"

    // $ANTLR start "L2"
    public void mL2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = L2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:412:4: ( '[' )
            // GAMS.g:412:6: '['
            {
            	Match('['); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "L2"

    // $ANTLR start "R2"
    public void mR2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = R2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:413:4: ( ']' )
            // GAMS.g:413:6: ']'
            {
            	Match(']'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "R2"

    // $ANTLR start "L3"
    public void mL3() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = L3;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:414:4: ( '{' )
            // GAMS.g:414:6: '{'
            {
            	Match('{'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "L3"

    // $ANTLR start "R3"
    public void mR3() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = R3;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:415:4: ( '}' )
            // GAMS.g:415:6: '}'
            {
            	Match('}'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "R3"

    // $ANTLR start "DOLLAR"
    public void mDOLLAR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOLLAR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:417:7: ( '$' )
            // GAMS.g:417:9: '$'
            {
            	Match('$'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DOLLAR"

    // $ANTLR start "StringInQuotes"
    public void mStringInQuotes() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = StringInQuotes;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:419:15: ( ( '\\'' (~ ( '\\'' ) )* '\\'' ) | ( '\"' (~ ( '\"' ) )* '\"' ) )
            int alt19 = 2;
            int LA19_0 = input.LA(1);

            if ( (LA19_0 == '\'') )
            {
                alt19 = 1;
            }
            else if ( (LA19_0 == '\"') )
            {
                alt19 = 2;
            }
            else 
            {
                NoViableAltException nvae_d19s0 =
                    new NoViableAltException("", 19, 0, input);

                throw nvae_d19s0;
            }
            switch (alt19) 
            {
                case 1 :
                    // GAMS.g:419:29: ( '\\'' (~ ( '\\'' ) )* '\\'' )
                    {
                    	// GAMS.g:419:29: ( '\\'' (~ ( '\\'' ) )* '\\'' )
                    	// GAMS.g:419:30: '\\'' (~ ( '\\'' ) )* '\\''
                    	{
                    		Match('\''); 
                    		// GAMS.g:419:35: (~ ( '\\'' ) )*
                    		do 
                    		{
                    		    int alt17 = 2;
                    		    int LA17_0 = input.LA(1);

                    		    if ( ((LA17_0 >= '\u0000' && LA17_0 <= '&') || (LA17_0 >= '(' && LA17_0 <= '\uFFFF')) )
                    		    {
                    		        alt17 = 1;
                    		    }


                    		    switch (alt17) 
                    			{
                    				case 1 :
                    				    // GAMS.g:419:36: ~ ( '\\'' )
                    				    {
                    				    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '&') || (input.LA(1) >= '(' && input.LA(1) <= '\uFFFF') ) 
                    				    	{
                    				    	    input.Consume();

                    				    	}
                    				    	else 
                    				    	{
                    				    	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    				    	    Recover(mse);
                    				    	    throw mse;}


                    				    }
                    				    break;

                    				default:
                    				    goto loop17;
                    		    }
                    		} while (true);

                    		loop17:
                    			;	// Stops C# compiler whining that label 'loop17' has no statements

                    		Match('\''); 

                    	}


                    }
                    break;
                case 2 :
                    // GAMS.g:419:60: ( '\"' (~ ( '\"' ) )* '\"' )
                    {
                    	// GAMS.g:419:60: ( '\"' (~ ( '\"' ) )* '\"' )
                    	// GAMS.g:419:61: '\"' (~ ( '\"' ) )* '\"'
                    	{
                    		Match('\"'); 
                    		// GAMS.g:419:65: (~ ( '\"' ) )*
                    		do 
                    		{
                    		    int alt18 = 2;
                    		    int LA18_0 = input.LA(1);

                    		    if ( ((LA18_0 >= '\u0000' && LA18_0 <= '!') || (LA18_0 >= '#' && LA18_0 <= '\uFFFF')) )
                    		    {
                    		        alt18 = 1;
                    		    }


                    		    switch (alt18) 
                    			{
                    				case 1 :
                    				    // GAMS.g:419:66: ~ ( '\"' )
                    				    {
                    				    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '!') || (input.LA(1) >= '#' && input.LA(1) <= '\uFFFF') ) 
                    				    	{
                    				    	    input.Consume();

                    				    	}
                    				    	else 
                    				    	{
                    				    	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    				    	    Recover(mse);
                    				    	    throw mse;}


                    				    }
                    				    break;

                    				default:
                    				    goto loop18;
                    		    }
                    		} while (true);

                    		loop18:
                    			;	// Stops C# compiler whining that label 'loop18' has no statements

                    		Match('\"'); 

                    	}


                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "StringInQuotes"

    // $ANTLR start "NEWLINE2"
    public void mNEWLINE2() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:421:19: ( '\\n' )
            // GAMS.g:421:21: '\\n'
            {
            	Match('\n'); 

            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "NEWLINE2"

    // $ANTLR start "NEWLINE3"
    public void mNEWLINE3() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:422:19: ( '\\r\\n' )
            // GAMS.g:422:21: '\\r\\n'
            {
            	Match("\r\n"); 


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "NEWLINE3"

    // $ANTLR start "DIGIT"
    public void mDIGIT() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:423:16: ( '0' .. '9' )
            // GAMS.g:423:18: '0' .. '9'
            {
            	MatchRange('0','9'); 

            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "DIGIT"

    // $ANTLR start "LETTER"
    public void mLETTER() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:424:16: ( 'a' .. 'z' | 'A' .. 'Z' )
            // GAMS.g:
            {
            	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || (input.LA(1) >= 'a' && input.LA(1) <= 'z') ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "LETTER"

    // $ANTLR start "Exponent"
    public void mExponent() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:427:5: ( ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+ )
            // GAMS.g:427:9: ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+
            {
            	if ( input.LA(1) == 'E' || input.LA(1) == 'e' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// GAMS.g:427:23: ( '+' | '-' )?
            	int alt20 = 2;
            	int LA20_0 = input.LA(1);

            	if ( (LA20_0 == '+' || LA20_0 == '-') )
            	{
            	    alt20 = 1;
            	}
            	switch (alt20) 
            	{
            	    case 1 :
            	        // GAMS.g:
            	        {
            	        	if ( input.LA(1) == '+' || input.LA(1) == '-' ) 
            	        	{
            	        	    input.Consume();

            	        	}
            	        	else 
            	        	{
            	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	        	    Recover(mse);
            	        	    throw mse;}


            	        }
            	        break;

            	}

            	// GAMS.g:427:38: ( '0' .. '9' )+
            	int cnt21 = 0;
            	do 
            	{
            	    int alt21 = 2;
            	    int LA21_0 = input.LA(1);

            	    if ( ((LA21_0 >= '0' && LA21_0 <= '9')) )
            	    {
            	        alt21 = 1;
            	    }


            	    switch (alt21) 
            		{
            			case 1 :
            			    // GAMS.g:427:40: '0' .. '9'
            			    {
            			    	MatchRange('0','9'); 

            			    }
            			    break;

            			default:
            			    if ( cnt21 >= 1 ) goto loop21;
            		            EarlyExitException eee21 =
            		                new EarlyExitException(21, input);
            		            throw eee21;
            	    }
            	    cnt21++;
            	} while (true);

            	loop21:
            		;	// Stops C# compiler whining that label 'loop21' has no statements


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "Exponent"

    // $ANTLR start "A"
    public void mA() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:430:11: ( ( 'a' | 'A' ) )
            // GAMS.g:430:12: ( 'a' | 'A' )
            {
            	if ( input.LA(1) == 'A' || input.LA(1) == 'a' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "A"

    // $ANTLR start "B"
    public void mB() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:431:11: ( ( 'b' | 'B' ) )
            // GAMS.g:431:12: ( 'b' | 'B' )
            {
            	if ( input.LA(1) == 'B' || input.LA(1) == 'b' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "B"

    // $ANTLR start "C"
    public void mC() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:432:11: ( ( 'c' | 'C' ) )
            // GAMS.g:432:12: ( 'c' | 'C' )
            {
            	if ( input.LA(1) == 'C' || input.LA(1) == 'c' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "C"

    // $ANTLR start "D"
    public void mD() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:433:11: ( ( 'd' | 'D' ) )
            // GAMS.g:433:12: ( 'd' | 'D' )
            {
            	if ( input.LA(1) == 'D' || input.LA(1) == 'd' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "D"

    // $ANTLR start "E"
    public void mE() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:434:11: ( ( 'e' | 'E' ) )
            // GAMS.g:434:12: ( 'e' | 'E' )
            {
            	if ( input.LA(1) == 'E' || input.LA(1) == 'e' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "E"

    // $ANTLR start "F"
    public void mF() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:435:11: ( ( 'f' | 'F' ) )
            // GAMS.g:435:12: ( 'f' | 'F' )
            {
            	if ( input.LA(1) == 'F' || input.LA(1) == 'f' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "F"

    // $ANTLR start "G"
    public void mG() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:436:11: ( ( 'g' | 'G' ) )
            // GAMS.g:436:12: ( 'g' | 'G' )
            {
            	if ( input.LA(1) == 'G' || input.LA(1) == 'g' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "G"

    // $ANTLR start "H"
    public void mH() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:437:11: ( ( 'h' | 'H' ) )
            // GAMS.g:437:12: ( 'h' | 'H' )
            {
            	if ( input.LA(1) == 'H' || input.LA(1) == 'h' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "H"

    // $ANTLR start "I"
    public void mI() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:438:11: ( ( 'i' | 'I' ) )
            // GAMS.g:438:12: ( 'i' | 'I' )
            {
            	if ( input.LA(1) == 'I' || input.LA(1) == 'i' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "I"

    // $ANTLR start "J"
    public void mJ() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:439:11: ( ( 'j' | 'J' ) )
            // GAMS.g:439:12: ( 'j' | 'J' )
            {
            	if ( input.LA(1) == 'J' || input.LA(1) == 'j' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "J"

    // $ANTLR start "K"
    public void mK() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:440:11: ( ( 'k' | 'K' ) )
            // GAMS.g:440:12: ( 'k' | 'K' )
            {
            	if ( input.LA(1) == 'K' || input.LA(1) == 'k' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "K"

    // $ANTLR start "L"
    public void mL() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:441:11: ( ( 'l' | 'L' ) )
            // GAMS.g:441:12: ( 'l' | 'L' )
            {
            	if ( input.LA(1) == 'L' || input.LA(1) == 'l' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "L"

    // $ANTLR start "M"
    public void mM() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:442:11: ( ( 'm' | 'M' ) )
            // GAMS.g:442:12: ( 'm' | 'M' )
            {
            	if ( input.LA(1) == 'M' || input.LA(1) == 'm' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "M"

    // $ANTLR start "N"
    public void mN() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:443:11: ( ( 'n' | 'N' ) )
            // GAMS.g:443:12: ( 'n' | 'N' )
            {
            	if ( input.LA(1) == 'N' || input.LA(1) == 'n' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "N"

    // $ANTLR start "O"
    public void mO() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:444:11: ( ( 'o' | 'O' ) )
            // GAMS.g:444:12: ( 'o' | 'O' )
            {
            	if ( input.LA(1) == 'O' || input.LA(1) == 'o' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "O"

    // $ANTLR start "P"
    public void mP() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:445:11: ( ( 'p' | 'P' ) )
            // GAMS.g:445:12: ( 'p' | 'P' )
            {
            	if ( input.LA(1) == 'P' || input.LA(1) == 'p' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "P"

    // $ANTLR start "Q"
    public void mQ() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:446:11: ( ( 'q' | 'Q' ) )
            // GAMS.g:446:12: ( 'q' | 'Q' )
            {
            	if ( input.LA(1) == 'Q' || input.LA(1) == 'q' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "Q"

    // $ANTLR start "R"
    public void mR() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:447:11: ( ( 'r' | 'R' ) )
            // GAMS.g:447:12: ( 'r' | 'R' )
            {
            	if ( input.LA(1) == 'R' || input.LA(1) == 'r' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "R"

    // $ANTLR start "S"
    public void mS() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:448:11: ( ( 's' | 'S' ) )
            // GAMS.g:448:12: ( 's' | 'S' )
            {
            	if ( input.LA(1) == 'S' || input.LA(1) == 's' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "S"

    // $ANTLR start "T"
    public void mT() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:449:11: ( ( 't' | 'T' ) )
            // GAMS.g:449:12: ( 't' | 'T' )
            {
            	if ( input.LA(1) == 'T' || input.LA(1) == 't' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "T"

    // $ANTLR start "U"
    public void mU() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:450:11: ( ( 'u' | 'U' ) )
            // GAMS.g:450:12: ( 'u' | 'U' )
            {
            	if ( input.LA(1) == 'U' || input.LA(1) == 'u' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "U"

    // $ANTLR start "V"
    public void mV() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:451:11: ( ( 'v' | 'V' ) )
            // GAMS.g:451:12: ( 'v' | 'V' )
            {
            	if ( input.LA(1) == 'V' || input.LA(1) == 'v' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "V"

    // $ANTLR start "W"
    public void mW() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:452:11: ( ( 'w' | 'W' ) )
            // GAMS.g:452:12: ( 'w' | 'W' )
            {
            	if ( input.LA(1) == 'W' || input.LA(1) == 'w' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "W"

    // $ANTLR start "X"
    public void mX() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:453:11: ( ( 'x' | 'X' ) )
            // GAMS.g:453:12: ( 'x' | 'X' )
            {
            	if ( input.LA(1) == 'X' || input.LA(1) == 'x' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "X"

    // $ANTLR start "Y"
    public void mY() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:454:11: ( ( 'y' | 'Y' ) )
            // GAMS.g:454:12: ( 'y' | 'Y' )
            {
            	if ( input.LA(1) == 'Y' || input.LA(1) == 'y' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "Y"

    // $ANTLR start "Z"
    public void mZ() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:455:11: ( ( 'z' | 'Z' ) )
            // GAMS.g:455:12: ( 'z' | 'Z' )
            {
            	if ( input.LA(1) == 'Z' || input.LA(1) == 'z' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "Z"

    override public void mTokens() // throws RecognitionException 
    {
        // GAMS.g:1:8: ( Comment | NESTED_ML_COMMENT | COMMENT1 | COMMENT2 | WHITESPACE | STARS | MULT | EQU | SUM | OR | AND | NOT | ABS | EXP | LOG | MAX | MIN | POWER | SQR | SAMEAS | TANH | MODEL | SOLVE | VARIABLES | EQUATIONS | DOT | PLUS | MINUS | SEMI | DIV | MOD | COMMA | EEQUAL | NONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL | EQUAL | LESSTHAN | GREATERTHAN | DOUBLEDOT | Integer | Double | Ident | NEWLINE | L1 | R1 | L2 | R2 | L3 | R3 | DOLLAR | StringInQuotes )
        int alt22 = 52;
        alt22 = dfa22.Predict(input);
        switch (alt22) 
        {
            case 1 :
                // GAMS.g:1:10: Comment
                {
                	mComment(); 

                }
                break;
            case 2 :
                // GAMS.g:1:18: NESTED_ML_COMMENT
                {
                	mNESTED_ML_COMMENT(); 

                }
                break;
            case 3 :
                // GAMS.g:1:36: COMMENT1
                {
                	mCOMMENT1(); 

                }
                break;
            case 4 :
                // GAMS.g:1:45: COMMENT2
                {
                	mCOMMENT2(); 

                }
                break;
            case 5 :
                // GAMS.g:1:54: WHITESPACE
                {
                	mWHITESPACE(); 

                }
                break;
            case 6 :
                // GAMS.g:1:65: STARS
                {
                	mSTARS(); 

                }
                break;
            case 7 :
                // GAMS.g:1:71: MULT
                {
                	mMULT(); 

                }
                break;
            case 8 :
                // GAMS.g:1:76: EQU
                {
                	mEQU(); 

                }
                break;
            case 9 :
                // GAMS.g:1:80: SUM
                {
                	mSUM(); 

                }
                break;
            case 10 :
                // GAMS.g:1:84: OR
                {
                	mOR(); 

                }
                break;
            case 11 :
                // GAMS.g:1:87: AND
                {
                	mAND(); 

                }
                break;
            case 12 :
                // GAMS.g:1:91: NOT
                {
                	mNOT(); 

                }
                break;
            case 13 :
                // GAMS.g:1:95: ABS
                {
                	mABS(); 

                }
                break;
            case 14 :
                // GAMS.g:1:99: EXP
                {
                	mEXP(); 

                }
                break;
            case 15 :
                // GAMS.g:1:103: LOG
                {
                	mLOG(); 

                }
                break;
            case 16 :
                // GAMS.g:1:107: MAX
                {
                	mMAX(); 

                }
                break;
            case 17 :
                // GAMS.g:1:111: MIN
                {
                	mMIN(); 

                }
                break;
            case 18 :
                // GAMS.g:1:115: POWER
                {
                	mPOWER(); 

                }
                break;
            case 19 :
                // GAMS.g:1:121: SQR
                {
                	mSQR(); 

                }
                break;
            case 20 :
                // GAMS.g:1:125: SAMEAS
                {
                	mSAMEAS(); 

                }
                break;
            case 21 :
                // GAMS.g:1:132: TANH
                {
                	mTANH(); 

                }
                break;
            case 22 :
                // GAMS.g:1:137: MODEL
                {
                	mMODEL(); 

                }
                break;
            case 23 :
                // GAMS.g:1:143: SOLVE
                {
                	mSOLVE(); 

                }
                break;
            case 24 :
                // GAMS.g:1:149: VARIABLES
                {
                	mVARIABLES(); 

                }
                break;
            case 25 :
                // GAMS.g:1:159: EQUATIONS
                {
                	mEQUATIONS(); 

                }
                break;
            case 26 :
                // GAMS.g:1:169: DOT
                {
                	mDOT(); 

                }
                break;
            case 27 :
                // GAMS.g:1:173: PLUS
                {
                	mPLUS(); 

                }
                break;
            case 28 :
                // GAMS.g:1:178: MINUS
                {
                	mMINUS(); 

                }
                break;
            case 29 :
                // GAMS.g:1:184: SEMI
                {
                	mSEMI(); 

                }
                break;
            case 30 :
                // GAMS.g:1:189: DIV
                {
                	mDIV(); 

                }
                break;
            case 31 :
                // GAMS.g:1:193: MOD
                {
                	mMOD(); 

                }
                break;
            case 32 :
                // GAMS.g:1:197: COMMA
                {
                	mCOMMA(); 

                }
                break;
            case 33 :
                // GAMS.g:1:203: EEQUAL
                {
                	mEEQUAL(); 

                }
                break;
            case 34 :
                // GAMS.g:1:210: NONEQUAL
                {
                	mNONEQUAL(); 

                }
                break;
            case 35 :
                // GAMS.g:1:219: LESSTHANOREQUAL
                {
                	mLESSTHANOREQUAL(); 

                }
                break;
            case 36 :
                // GAMS.g:1:235: GREATERTHANOREQUAL
                {
                	mGREATERTHANOREQUAL(); 

                }
                break;
            case 37 :
                // GAMS.g:1:254: EQUAL
                {
                	mEQUAL(); 

                }
                break;
            case 38 :
                // GAMS.g:1:260: LESSTHAN
                {
                	mLESSTHAN(); 

                }
                break;
            case 39 :
                // GAMS.g:1:269: GREATERTHAN
                {
                	mGREATERTHAN(); 

                }
                break;
            case 40 :
                // GAMS.g:1:281: DOUBLEDOT
                {
                	mDOUBLEDOT(); 

                }
                break;
            case 41 :
                // GAMS.g:1:291: Integer
                {
                	mInteger(); 

                }
                break;
            case 42 :
                // GAMS.g:1:299: Double
                {
                	mDouble(); 

                }
                break;
            case 43 :
                // GAMS.g:1:306: Ident
                {
                	mIdent(); 

                }
                break;
            case 44 :
                // GAMS.g:1:312: NEWLINE
                {
                	mNEWLINE(); 

                }
                break;
            case 45 :
                // GAMS.g:1:320: L1
                {
                	mL1(); 

                }
                break;
            case 46 :
                // GAMS.g:1:323: R1
                {
                	mR1(); 

                }
                break;
            case 47 :
                // GAMS.g:1:326: L2
                {
                	mL2(); 

                }
                break;
            case 48 :
                // GAMS.g:1:329: R2
                {
                	mR2(); 

                }
                break;
            case 49 :
                // GAMS.g:1:332: L3
                {
                	mL3(); 

                }
                break;
            case 50 :
                // GAMS.g:1:335: R3
                {
                	mR3(); 

                }
                break;
            case 51 :
                // GAMS.g:1:338: DOLLAR
                {
                	mDOLLAR(); 

                }
                break;
            case 52 :
                // GAMS.g:1:345: StringInQuotes
                {
                	mStringInQuotes(); 

                }
                break;

        }

    }


    protected DFA14 dfa14;
    protected DFA22 dfa22;
	private void InitializeCyclicDFAs()
	{
	    this.dfa14 = new DFA14(this);
	    this.dfa22 = new DFA22(this);


	}

    const string DFA14_eotS =
        "\x05\uffff";
    const string DFA14_eofS =
        "\x05\uffff";
    const string DFA14_minS =
        "\x02\x2e\x03\uffff";
    const string DFA14_maxS =
        "\x01\x39\x01\x65\x03\uffff";
    const string DFA14_acceptS =
        "\x02\uffff\x01\x02\x01\x03\x01\x01";
    const string DFA14_specialS =
        "\x05\uffff}>";
    static readonly string[] DFA14_transitionS = {
            "\x01\x02\x01\uffff\x0a\x01",
            "\x01\x04\x01\uffff\x0a\x01\x0b\uffff\x01\x03\x1f\uffff\x01"+
            "\x03",
            "",
            "",
            ""
    };

    static readonly short[] DFA14_eot = DFA.UnpackEncodedString(DFA14_eotS);
    static readonly short[] DFA14_eof = DFA.UnpackEncodedString(DFA14_eofS);
    static readonly char[] DFA14_min = DFA.UnpackEncodedStringToUnsignedChars(DFA14_minS);
    static readonly char[] DFA14_max = DFA.UnpackEncodedStringToUnsignedChars(DFA14_maxS);
    static readonly short[] DFA14_accept = DFA.UnpackEncodedString(DFA14_acceptS);
    static readonly short[] DFA14_special = DFA.UnpackEncodedString(DFA14_specialS);
    static readonly short[][] DFA14_transition = DFA.UnpackEncodedStringArray(DFA14_transitionS);

    protected class DFA14 : DFA
    {
        public DFA14(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 14;
            this.eot = DFA14_eot;
            this.eof = DFA14_eof;
            this.min = DFA14_min;
            this.max = DFA14_max;
            this.accept = DFA14_accept;
            this.special = DFA14_special;
            this.transition = DFA14_transition;

        }

        override public string Description
        {
            get { return "399:1: Double : ( ( '0' .. '9' )+ DOT ( '0' .. '9' )* ( Exponent )? | DOT ( '0' .. '9' )+ ( Exponent )? | ( '0' .. '9' )+ Exponent );"; }
        }

    }

    const string DFA22_eotS =
        "\x01\uffff\x01\x26\x03\uffff\x01\x28\x0a\x1a\x01\x3b\x05\uffff"+
        "\x01\x3e\x01\x41\x01\x43\x01\x44\x0f\uffff\x06\x1a\x01\x4b\x0a\x1a"+
        "\x0b\uffff\x01\x57\x01\x58\x01\x59\x01\x5a\x02\x1a\x01\uffff\x01"+
        "\x5d\x01\x5e\x01\x5f\x01\x60\x01\x61\x01\x62\x05\x1a\x04\uffff\x02"+
        "\x1a\x06\uffff\x02\x1a\x01\x6c\x03\x1a\x01\x70\x01\x71\x01\x72\x01"+
        "\uffff\x02\x1a\x01\x75\x03\uffff\x02\x1a\x01\uffff\x03\x1a\x01\x7b"+
        "\x01\x7c\x02\uffff";
    const string DFA22_eofS =
        "\x7d\uffff";
    const string DFA22_minS =
        "\x01\x09\x01\x2a\x03\uffff\x01\x2a\x01\x51\x01\x41\x01\x52\x01"+
        "\x42\x02\x4f\x01\x41\x01\x4f\x02\x41\x01\x2e\x05\uffff\x01\x45\x02"+
        "\x3d\x01\x2e\x0f\uffff\x01\x55\x01\x50\x01\x4d\x01\x52\x01\x4d\x01"+
        "\x4c\x01\x30\x01\x44\x01\x53\x01\x54\x01\x47\x01\x58\x01\x4e\x01"+
        "\x44\x01\x57\x01\x4e\x01\x52\x0b\uffff\x04\x30\x01\x45\x01\x56\x01"+
        "\uffff\x06\x30\x02\x45\x01\x48\x01\x49\x01\x54\x04\uffff\x01\x41"+
        "\x01\x45\x06\uffff\x01\x4c\x01\x52\x01\x30\x01\x41\x01\x49\x01\x53"+
        "\x03\x30\x01\uffff\x01\x42\x01\x4f\x01\x30\x03\uffff\x01\x4c\x01"+
        "\x4e\x01\uffff\x01\x45\x02\x53\x02\x30\x02\uffff";
    const string DFA22_maxS =
        "\x01\x7d\x01\x2f\x03\uffff\x01\x2a\x01\x58\x01\x55\x01\x52\x01"+
        "\x4e\x04\x4f\x02\x41\x01\x39\x05\uffff\x01\x65\x01\x3e\x01\x3d\x01"+
        "\x65\x0f\uffff\x01\x55\x01\x50\x01\x4d\x01\x52\x01\x4d\x01\x4c\x01"+
        "\x7a\x01\x44\x01\x53\x01\x54\x01\x47\x01\x58\x01\x4e\x01\x44\x01"+
        "\x57\x01\x4e\x01\x52\x0b\uffff\x04\x7a\x01\x45\x01\x56\x01\uffff"+
        "\x06\x7a\x02\x45\x01\x48\x01\x49\x01\x54\x04\uffff\x01\x41\x01\x45"+
        "\x06\uffff\x01\x4c\x01\x52\x01\x7a\x01\x41\x01\x49\x01\x53\x03\x7a"+
        "\x01\uffff\x01\x42\x01\x4f\x01\x7a\x03\uffff\x01\x4c\x01\x4e\x01"+
        "\uffff\x01\x45\x02\x53\x02\x7a\x02\uffff";
    const string DFA22_acceptS =
        "\x02\uffff\x01\x03\x01\x04\x01\x05\x0c\uffff\x01\x1b\x01\x1c\x01"+
        "\x1d\x01\x1f\x01\x20\x04\uffff\x01\x2b\x01\x2c\x01\x2d\x01\x2e\x01"+
        "\x2f\x01\x30\x01\x31\x01\x32\x01\x33\x01\x34\x01\x01\x01\x02\x01"+
        "\x1e\x01\x06\x01\x07\x11\uffff\x01\x28\x01\x1a\x01\x2a\x01\x21\x01"+
        "\x25\x01\x22\x01\x23\x01\x26\x01\x24\x01\x27\x01\x29\x06\uffff\x01"+
        "\x0a\x0b\uffff\x01\x08\x01\x0e\x01\x09\x01\x13\x02\uffff\x01\x0b"+
        "\x01\x0d\x01\x0c\x01\x0f\x01\x10\x01\x11\x09\uffff\x01\x15\x03\uffff"+
        "\x01\x17\x01\x16\x01\x12\x02\uffff\x01\x14\x05\uffff\x01\x19\x01"+
        "\x18";
    const string DFA22_specialS =
        "\x7d\uffff}>";
    static readonly string[] DFA22_transitionS = {
            "\x01\x04\x01\x1b\x01\uffff\x01\x04\x01\x1b\x12\uffff\x01\x04"+
            "\x01\x03\x01\x23\x01\x02\x01\x22\x01\x14\x01\uffff\x01\x23\x01"+
            "\x1c\x01\x1d\x01\x05\x01\x11\x01\x15\x01\x12\x01\x10\x01\x01"+
            "\x0a\x19\x01\uffff\x01\x13\x01\x17\x01\x16\x01\x18\x02\uffff"+
            "\x01\x09\x03\x1a\x01\x06\x06\x1a\x01\x0b\x01\x0c\x01\x0a\x01"+
            "\x08\x01\x0d\x02\x1a\x01\x07\x01\x0e\x01\x1a\x01\x0f\x04\x1a"+
            "\x01\x1e\x01\uffff\x01\x1f\x01\uffff\x01\x1a\x01\uffff\x1a\x1a"+
            "\x01\x20\x01\uffff\x01\x21",
            "\x01\x25\x04\uffff\x01\x24",
            "",
            "",
            "",
            "\x01\x27",
            "\x01\x29\x06\uffff\x01\x2a",
            "\x01\x2d\x0d\uffff\x01\x2e\x01\uffff\x01\x2c\x03\uffff\x01"+
            "\x2b",
            "\x01\x2f",
            "\x01\x31\x0b\uffff\x01\x30",
            "\x01\x32",
            "\x01\x33",
            "\x01\x34\x07\uffff\x01\x35\x05\uffff\x01\x36",
            "\x01\x37",
            "\x01\x38",
            "\x01\x39",
            "\x01\x3a\x01\uffff\x0a\x3c",
            "",
            "",
            "",
            "",
            "",
            "\x01\x3d\x1f\uffff\x01\x3d",
            "\x01\x40\x01\x3f",
            "\x01\x42",
            "\x01\x3c\x01\uffff\x0a\x19\x0b\uffff\x01\x3c\x1f\uffff\x01"+
            "\x3c",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x45",
            "\x01\x46",
            "\x01\x47",
            "\x01\x48",
            "\x01\x49",
            "\x01\x4a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x01\x4c",
            "\x01\x4d",
            "\x01\x4e",
            "\x01\x4f",
            "\x01\x50",
            "\x01\x51",
            "\x01\x52",
            "\x01\x53",
            "\x01\x54",
            "\x01\x55",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x0a\x1a\x07\uffff\x01\x56\x19\x1a\x04\uffff\x01\x1a\x01\uffff"+
            "\x1a\x1a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x01\x5b",
            "\x01\x5c",
            "",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x01\x63",
            "\x01\x64",
            "\x01\x65",
            "\x01\x66",
            "\x01\x67",
            "",
            "",
            "",
            "",
            "\x01\x68",
            "\x01\x69",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x6a",
            "\x01\x6b",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x01\x6d",
            "\x01\x6e",
            "\x01\x6f",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "",
            "\x01\x73",
            "\x01\x74",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "",
            "",
            "",
            "\x01\x76",
            "\x01\x77",
            "",
            "\x01\x78",
            "\x01\x79",
            "\x01\x7a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "",
            ""
    };

    static readonly short[] DFA22_eot = DFA.UnpackEncodedString(DFA22_eotS);
    static readonly short[] DFA22_eof = DFA.UnpackEncodedString(DFA22_eofS);
    static readonly char[] DFA22_min = DFA.UnpackEncodedStringToUnsignedChars(DFA22_minS);
    static readonly char[] DFA22_max = DFA.UnpackEncodedStringToUnsignedChars(DFA22_maxS);
    static readonly short[] DFA22_accept = DFA.UnpackEncodedString(DFA22_acceptS);
    static readonly short[] DFA22_special = DFA.UnpackEncodedString(DFA22_specialS);
    static readonly short[][] DFA22_transition = DFA.UnpackEncodedStringArray(DFA22_transitionS);

    protected class DFA22 : DFA
    {
        public DFA22(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 22;
            this.eot = DFA22_eot;
            this.eof = DFA22_eof;
            this.min = DFA22_min;
            this.max = DFA22_max;
            this.accept = DFA22_accept;
            this.special = DFA22_special;
            this.transition = DFA22_transition;

        }

        override public string Description
        {
            get { return "1:1: Tokens : ( Comment | NESTED_ML_COMMENT | COMMENT1 | COMMENT2 | WHITESPACE | STARS | MULT | EQU | SUM | OR | AND | NOT | ABS | EXP | LOG | MAX | MIN | POWER | SQR | SAMEAS | TANH | MODEL | SOLVE | VARIABLES | EQUATIONS | DOT | PLUS | MINUS | SEMI | DIV | MOD | COMMA | EEQUAL | NONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL | EQUAL | LESSTHAN | GREATERTHAN | DOUBLEDOT | Integer | Double | Ident | NEWLINE | L1 | R1 | L2 | R2 | L3 | R3 | DOLLAR | StringInQuotes );"; }
        }

    }

 
    
}
}