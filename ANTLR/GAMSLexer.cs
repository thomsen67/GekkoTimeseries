// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 GAMS.g 2022-03-23 17:49:48

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
    public const int ASTPOW = 25;
    public const int RB = 11;
    public const int ASTSIMPLEFUNCTION = 21;
    public const int ASTVARIABLE = 26;
    public const int RP = 12;
    public const int LETTER = 38;
    public const int MOD = 32;
    public const int LOG = 4;
    public const int ASTFUNCTION = 22;
    public const int Exponent = 37;
    public const int EOF = -1;
    public const int ASTINTEGER = 23;
    public const int ASTFRMLCODE = 19;
    public const int EXP = 5;
    public const int Comment = 43;
    public const int ASTEND = 27;
    public const int PLUS = 6;
    public const int FRML = 28;
    public const int DIGIT = 36;
    public const int DOT = 13;
    public const int D = 48;
    public const int Double = 30;
    public const int E = 49;
    public const int F = 50;
    public const int G = 51;
    public const int A = 45;
    public const int B = 46;
    public const int C = 47;
    public const int L = 56;
    public const int M = 57;
    public const int N = 58;
    public const int NESTED_ML_COMMENT = 44;
    public const int O = 59;
    public const int H = 52;
    public const int I = 53;
    public const int J = 54;
    public const int NEWLINE2 = 41;
    public const int K = 55;
    public const int NEWLINE3 = 42;
    public const int U = 65;
    public const int T = 64;
    public const int WHITESPACE = 39;
    public const int NEGATE = 16;
    public const int W = 67;
    public const int V = 66;
    public const int Q = 61;
    public const int P = 60;
    public const int ASTFRML = 17;
    public const int S = 63;
    public const int MINUS = 7;
    public const int MULT = 8;
    public const int R = 62;
    public const int SEMI = 29;
    public const int TRUE = 14;
    public const int Y = 69;
    public const int X = 68;
    public const int Z = 70;
    public const int T__71 = 71;
    public const int ASTLEFTSIDE = 18;
    public const int T__72 = 72;
    public const int NEWLINE = 40;
    public const int Ident = 33;
    public const int ASTEXPRESSION = 20;
    public const int Doubledot = 35;
    public const int STARS = 34;
    public const int LB = 10;
    public const int ASTDOUBLE = 24;
    public const int DIV = 9;
    public const int FALSE = 15;
    public const int T__73 = 73;
    public const int Integer = 31;

      public override void ReportError(RecognitionException e) {
            string hdr = GetErrorHeader(e);
            string msg = "GAMS lexer error: " + e.Message;
            throw new Exception(e.Line + "�" + e.CharPositionInLine + "�" + hdr + "�" + msg);
      } 
      
      public static System.Collections.Generic.Dictionary<string, int> kw = GetKw();

      public static System.Collections.Generic.Dictionary<string, int> GetKw()
      {
         System.Collections.Generic.Dictionary<string, int> d = new System.Collections.Generic.Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
         d.Add("EXP"                    ,   EXP                     );                                        
         d.Add("LOG"                    ,   LOG                     );
    	 d.Add("FRML"                    ,   FRML                     );
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

    // $ANTLR start "LOG"
    public void mLOG() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LOG;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:36:5: ( 'LOG' )
            // GAMS.g:36:7: 'LOG'
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

    // $ANTLR start "EXP"
    public void mEXP() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EXP;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:37:5: ( 'EXP' )
            // GAMS.g:37:7: 'EXP'
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

    // $ANTLR start "LB"
    public void mLB() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LB;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:38:4: ( '<' )
            // GAMS.g:38:6: '<'
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
    // $ANTLR end "LB"

    // $ANTLR start "RB"
    public void mRB() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RB;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:39:4: ( '>' )
            // GAMS.g:39:6: '>'
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
    // $ANTLR end "RB"

    // $ANTLR start "RP"
    public void mRP() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RP;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:40:4: ( ')' )
            // GAMS.g:40:6: ')'
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
    // $ANTLR end "RP"

    // $ANTLR start "DOT"
    public void mDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:41:5: ( '.' )
            // GAMS.g:41:7: '.'
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

    // $ANTLR start "TRUE"
    public void mTRUE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TRUE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:42:6: ( 'true' )
            // GAMS.g:42:8: 'true'
            {
            	Match("true"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "TRUE"

    // $ANTLR start "FALSE"
    public void mFALSE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FALSE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:43:7: ( 'false' )
            // GAMS.g:43:9: 'false'
            {
            	Match("false"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "FALSE"

    // $ANTLR start "T__71"
    public void mT__71() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__71;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:44:7: ( '=' )
            // GAMS.g:44:9: '='
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
    // $ANTLR end "T__71"

    // $ANTLR start "T__72"
    public void mT__72() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__72;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:45:7: ( '(' )
            // GAMS.g:45:9: '('
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
    // $ANTLR end "T__72"

    // $ANTLR start "T__73"
    public void mT__73() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__73;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:46:7: ( ',' )
            // GAMS.g:46:9: ','
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
    // $ANTLR end "T__73"

    // $ANTLR start "PLUS"
    public void mPLUS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = PLUS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:174:6: ( '+' )
            // GAMS.g:174:8: '+'
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
            // GAMS.g:175:7: ( '-' )
            // GAMS.g:175:9: '-'
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
            // GAMS.g:176:6: ( ';' )
            // GAMS.g:176:8: ';'
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

    // $ANTLR start "MULT"
    public void mMULT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MULT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:177:6: ( '*' )
            // GAMS.g:177:8: '*'
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

    // $ANTLR start "DIV"
    public void mDIV() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DIV;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:178:5: ( '/' )
            // GAMS.g:178:7: '/'
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
            // GAMS.g:179:5: ( '%' )
            // GAMS.g:179:7: '%'
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

    // $ANTLR start "FRML"
    public void mFRML() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FRML;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:180:6: ( 'FRML' )
            // GAMS.g:180:8: 'FRML'
            {
            	Match("FRML"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "FRML"

    // $ANTLR start "STARS"
    public void mSTARS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STARS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:183:10: ( '**' )
            // GAMS.g:183:17: '**'
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

    // $ANTLR start "Doubledot"
    public void mDoubledot() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Doubledot;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:195:11: ( '..' )
            // GAMS.g:195:13: '..'
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
    // $ANTLR end "Doubledot"

    // $ANTLR start "Integer"
    public void mInteger() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Integer;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:197:9: ( ( DIGIT )+ )
            // GAMS.g:197:11: ( DIGIT )+
            {
            	// GAMS.g:197:11: ( DIGIT )+
            	int cnt1 = 0;
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( ((LA1_0 >= '0' && LA1_0 <= '9')) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // GAMS.g:197:12: DIGIT
            			    {
            			    	mDIGIT(); 

            			    }
            			    break;

            			default:
            			    if ( cnt1 >= 1 ) goto loop1;
            		            EarlyExitException eee1 =
            		                new EarlyExitException(1, input);
            		            throw eee1;
            	    }
            	    cnt1++;
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements


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
            // GAMS.g:200:5: ( ( '0' .. '9' )+ DOT ( '0' .. '9' )* ( Exponent )? | DOT ( '0' .. '9' )+ ( Exponent )? | ( '0' .. '9' )+ Exponent )
            int alt8 = 3;
            alt8 = dfa8.Predict(input);
            switch (alt8) 
            {
                case 1 :
                    // GAMS.g:201:8: ( '0' .. '9' )+ DOT ( '0' .. '9' )* ( Exponent )?
                    {
                    	// GAMS.g:201:8: ( '0' .. '9' )+
                    	int cnt2 = 0;
                    	do 
                    	{
                    	    int alt2 = 2;
                    	    int LA2_0 = input.LA(1);

                    	    if ( ((LA2_0 >= '0' && LA2_0 <= '9')) )
                    	    {
                    	        alt2 = 1;
                    	    }


                    	    switch (alt2) 
                    		{
                    			case 1 :
                    			    // GAMS.g:201:9: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt2 >= 1 ) goto loop2;
                    		            EarlyExitException eee2 =
                    		                new EarlyExitException(2, input);
                    		            throw eee2;
                    	    }
                    	    cnt2++;
                    	} while (true);

                    	loop2:
                    		;	// Stops C# compiler whining that label 'loop2' has no statements

                    	mDOT(); 
                    	// GAMS.g:201:26: ( '0' .. '9' )*
                    	do 
                    	{
                    	    int alt3 = 2;
                    	    int LA3_0 = input.LA(1);

                    	    if ( ((LA3_0 >= '0' && LA3_0 <= '9')) )
                    	    {
                    	        alt3 = 1;
                    	    }


                    	    switch (alt3) 
                    		{
                    			case 1 :
                    			    // GAMS.g:201:27: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop3;
                    	    }
                    	} while (true);

                    	loop3:
                    		;	// Stops C# compiler whining that label 'loop3' has no statements

                    	// GAMS.g:201:40: ( Exponent )?
                    	int alt4 = 2;
                    	int LA4_0 = input.LA(1);

                    	if ( (LA4_0 == 'E' || LA4_0 == 'e') )
                    	{
                    	    alt4 = 1;
                    	}
                    	switch (alt4) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:201:40: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // GAMS.g:202:9: DOT ( '0' .. '9' )+ ( Exponent )?
                    {
                    	mDOT(); 
                    	// GAMS.g:202:13: ( '0' .. '9' )+
                    	int cnt5 = 0;
                    	do 
                    	{
                    	    int alt5 = 2;
                    	    int LA5_0 = input.LA(1);

                    	    if ( ((LA5_0 >= '0' && LA5_0 <= '9')) )
                    	    {
                    	        alt5 = 1;
                    	    }


                    	    switch (alt5) 
                    		{
                    			case 1 :
                    			    // GAMS.g:202:15: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

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

                    	// GAMS.g:202:29: ( Exponent )?
                    	int alt6 = 2;
                    	int LA6_0 = input.LA(1);

                    	if ( (LA6_0 == 'E' || LA6_0 == 'e') )
                    	{
                    	    alt6 = 1;
                    	}
                    	switch (alt6) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:202:29: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 3 :
                    // GAMS.g:203:8: ( '0' .. '9' )+ Exponent
                    {
                    	// GAMS.g:203:8: ( '0' .. '9' )+
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
                    			    // GAMS.g:203:9: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

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

    // $ANTLR start "Exponent"
    public void mExponent() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:208:5: ( ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+ )
            // GAMS.g:208:9: ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+
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

            	// GAMS.g:208:23: ( '+' | '-' )?
            	int alt9 = 2;
            	int LA9_0 = input.LA(1);

            	if ( (LA9_0 == '+' || LA9_0 == '-') )
            	{
            	    alt9 = 1;
            	}
            	switch (alt9) 
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

            	// GAMS.g:208:38: ( '0' .. '9' )+
            	int cnt10 = 0;
            	do 
            	{
            	    int alt10 = 2;
            	    int LA10_0 = input.LA(1);

            	    if ( ((LA10_0 >= '0' && LA10_0 <= '9')) )
            	    {
            	        alt10 = 1;
            	    }


            	    switch (alt10) 
            		{
            			case 1 :
            			    // GAMS.g:208:40: '0' .. '9'
            			    {
            			    	MatchRange('0','9'); 

            			    }
            			    break;

            			default:
            			    if ( cnt10 >= 1 ) goto loop10;
            		            EarlyExitException eee10 =
            		                new EarlyExitException(10, input);
            		            throw eee10;
            	    }
            	    cnt10++;
            	} while (true);

            	loop10:
            		;	// Stops C# compiler whining that label 'loop10' has no statements


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "Exponent"

    // $ANTLR start "Ident"
    public void mIdent() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Ident;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:211:7: ( ( LETTER | '_' ) ( DIGIT | LETTER | '_' )* )
            // GAMS.g:211:9: ( LETTER | '_' ) ( DIGIT | LETTER | '_' )*
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

            	// GAMS.g:211:22: ( DIGIT | LETTER | '_' )*
            	do 
            	{
            	    int alt11 = 2;
            	    int LA11_0 = input.LA(1);

            	    if ( ((LA11_0 >= '0' && LA11_0 <= '9') || (LA11_0 >= 'A' && LA11_0 <= 'Z') || LA11_0 == '_' || (LA11_0 >= 'a' && LA11_0 <= 'z')) )
            	    {
            	        alt11 = 1;
            	    }


            	    switch (alt11) 
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
            			    goto loop11;
            	    }
            	} while (true);

            	loop11:
            		;	// Stops C# compiler whining that label 'loop11' has no statements

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

    // $ANTLR start "WHITESPACE"
    public void mWHITESPACE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WHITESPACE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:213:12: ( ( '\\t' | ' ' | '\\u000C' )+ )
            // GAMS.g:213:14: ( '\\t' | ' ' | '\\u000C' )+
            {
            	// GAMS.g:213:14: ( '\\t' | ' ' | '\\u000C' )+
            	int cnt12 = 0;
            	do 
            	{
            	    int alt12 = 2;
            	    int LA12_0 = input.LA(1);

            	    if ( (LA12_0 == '\t' || LA12_0 == '\f' || LA12_0 == ' ') )
            	    {
            	        alt12 = 1;
            	    }


            	    switch (alt12) 
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
            			    if ( cnt12 >= 1 ) goto loop12;
            		            EarlyExitException eee12 =
            		                new EarlyExitException(12, input);
            		            throw eee12;
            	    }
            	    cnt12++;
            	} while (true);

            	loop12:
            		;	// Stops C# compiler whining that label 'loop12' has no statements

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

    // $ANTLR start "NEWLINE"
    public void mNEWLINE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NEWLINE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:215:12: ( ( ( '\\r' )? '\\n' ) )
            // GAMS.g:215:16: ( ( '\\r' )? '\\n' )
            {
            	// GAMS.g:215:16: ( ( '\\r' )? '\\n' )
            	// GAMS.g:215:17: ( '\\r' )? '\\n'
            	{
            		// GAMS.g:215:17: ( '\\r' )?
            		int alt13 = 2;
            		int LA13_0 = input.LA(1);

            		if ( (LA13_0 == '\r') )
            		{
            		    alt13 = 1;
            		}
            		switch (alt13) 
            		{
            		    case 1 :
            		        // GAMS.g:215:18: '\\r'
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

    // $ANTLR start "Comment"
    public void mComment() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Comment;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:217:12: ( ( '()' | '//' ) (~ ( NEWLINE2 | NEWLINE3 ) )* )
            // GAMS.g:217:14: ( '()' | '//' ) (~ ( NEWLINE2 | NEWLINE3 ) )*
            {
            	// GAMS.g:217:14: ( '()' | '//' )
            	int alt14 = 2;
            	int LA14_0 = input.LA(1);

            	if ( (LA14_0 == '(') )
            	{
            	    alt14 = 1;
            	}
            	else if ( (LA14_0 == '/') )
            	{
            	    alt14 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d14s0 =
            	        new NoViableAltException("", 14, 0, input);

            	    throw nvae_d14s0;
            	}
            	switch (alt14) 
            	{
            	    case 1 :
            	        // GAMS.g:217:15: '()'
            	        {
            	        	Match("()"); 


            	        }
            	        break;
            	    case 2 :
            	        // GAMS.g:217:22: '//'
            	        {
            	        	Match("//"); 


            	        }
            	        break;

            	}

            	// GAMS.g:217:28: (~ ( NEWLINE2 | NEWLINE3 ) )*
            	do 
            	{
            	    int alt15 = 2;
            	    int LA15_0 = input.LA(1);

            	    if ( ((LA15_0 >= '\u0000' && LA15_0 <= '\t') || (LA15_0 >= '\u000B' && LA15_0 <= '\uFFFF')) )
            	    {
            	        alt15 = 1;
            	    }


            	    switch (alt15) 
            		{
            			case 1 :
            			    // GAMS.g:217:29: ~ ( NEWLINE2 | NEWLINE3 )
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
            			    goto loop15;
            	    }
            	} while (true);

            	loop15:
            		;	// Stops C# compiler whining that label 'loop15' has no statements

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
            // GAMS.g:221:5: ( '/*' ( options {greedy=false; } : NESTED_ML_COMMENT | . )* '*/' )
            // GAMS.g:221:9: '/*' ( options {greedy=false; } : NESTED_ML_COMMENT | . )* '*/'
            {
            	Match("/*"); 

            	// GAMS.g:222:9: ( options {greedy=false; } : NESTED_ML_COMMENT | . )*
            	do 
            	{
            	    int alt16 = 3;
            	    int LA16_0 = input.LA(1);

            	    if ( (LA16_0 == '*') )
            	    {
            	        int LA16_1 = input.LA(2);

            	        if ( (LA16_1 == '/') )
            	        {
            	            alt16 = 3;
            	        }
            	        else if ( ((LA16_1 >= '\u0000' && LA16_1 <= '.') || (LA16_1 >= '0' && LA16_1 <= '\uFFFF')) )
            	        {
            	            alt16 = 2;
            	        }


            	    }
            	    else if ( (LA16_0 == '/') )
            	    {
            	        int LA16_2 = input.LA(2);

            	        if ( (LA16_2 == '*') )
            	        {
            	            alt16 = 1;
            	        }
            	        else if ( ((LA16_2 >= '\u0000' && LA16_2 <= ')') || (LA16_2 >= '+' && LA16_2 <= '\uFFFF')) )
            	        {
            	            alt16 = 2;
            	        }


            	    }
            	    else if ( ((LA16_0 >= '\u0000' && LA16_0 <= ')') || (LA16_0 >= '+' && LA16_0 <= '.') || (LA16_0 >= '0' && LA16_0 <= '\uFFFF')) )
            	    {
            	        alt16 = 2;
            	    }


            	    switch (alt16) 
            		{
            			case 1 :
            			    // GAMS.g:222:36: NESTED_ML_COMMENT
            			    {
            			    	mNESTED_ML_COMMENT(); 

            			    }
            			    break;
            			case 2 :
            			    // GAMS.g:222:56: .
            			    {
            			    	MatchAny(); 

            			    }
            			    break;

            			default:
            			    goto loop16;
            	    }
            	} while (true);

            	loop16:
            		;	// Stops C# compiler whining that label 'loop16' has no statements

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

    // $ANTLR start "NEWLINE2"
    public void mNEWLINE2() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:226:19: ( '\\n' )
            // GAMS.g:226:21: '\\n'
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
            // GAMS.g:227:19: ( '\\r\\n' )
            // GAMS.g:227:21: '\\r\\n'
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
            // GAMS.g:228:16: ( '0' .. '9' )
            // GAMS.g:228:18: '0' .. '9'
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
            // GAMS.g:229:16: ( 'a' .. 'z' | 'A' .. 'Z' )
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

    // $ANTLR start "A"
    public void mA() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:231:11: ( ( 'a' | 'A' ) )
            // GAMS.g:231:12: ( 'a' | 'A' )
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
            // GAMS.g:232:11: ( ( 'b' | 'B' ) )
            // GAMS.g:232:12: ( 'b' | 'B' )
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
            // GAMS.g:233:11: ( ( 'c' | 'C' ) )
            // GAMS.g:233:12: ( 'c' | 'C' )
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
            // GAMS.g:234:11: ( ( 'd' | 'D' ) )
            // GAMS.g:234:12: ( 'd' | 'D' )
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
            // GAMS.g:235:11: ( ( 'e' | 'E' ) )
            // GAMS.g:235:12: ( 'e' | 'E' )
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
            // GAMS.g:236:11: ( ( 'f' | 'F' ) )
            // GAMS.g:236:12: ( 'f' | 'F' )
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
            // GAMS.g:237:11: ( ( 'g' | 'G' ) )
            // GAMS.g:237:12: ( 'g' | 'G' )
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
            // GAMS.g:238:11: ( ( 'h' | 'H' ) )
            // GAMS.g:238:12: ( 'h' | 'H' )
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
            // GAMS.g:239:11: ( ( 'i' | 'I' ) )
            // GAMS.g:239:12: ( 'i' | 'I' )
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
            // GAMS.g:240:11: ( ( 'j' | 'J' ) )
            // GAMS.g:240:12: ( 'j' | 'J' )
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
            // GAMS.g:241:11: ( ( 'k' | 'K' ) )
            // GAMS.g:241:12: ( 'k' | 'K' )
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
            // GAMS.g:242:11: ( ( 'l' | 'L' ) )
            // GAMS.g:242:12: ( 'l' | 'L' )
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
            // GAMS.g:243:11: ( ( 'm' | 'M' ) )
            // GAMS.g:243:12: ( 'm' | 'M' )
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
            // GAMS.g:244:11: ( ( 'n' | 'N' ) )
            // GAMS.g:244:12: ( 'n' | 'N' )
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
            // GAMS.g:245:11: ( ( 'o' | 'O' ) )
            // GAMS.g:245:12: ( 'o' | 'O' )
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
            // GAMS.g:246:11: ( ( 'p' | 'P' ) )
            // GAMS.g:246:12: ( 'p' | 'P' )
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
            // GAMS.g:247:11: ( ( 'q' | 'Q' ) )
            // GAMS.g:247:12: ( 'q' | 'Q' )
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
            // GAMS.g:248:11: ( ( 'r' | 'R' ) )
            // GAMS.g:248:12: ( 'r' | 'R' )
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
            // GAMS.g:249:11: ( ( 's' | 'S' ) )
            // GAMS.g:249:12: ( 's' | 'S' )
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
            // GAMS.g:250:11: ( ( 't' | 'T' ) )
            // GAMS.g:250:12: ( 't' | 'T' )
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
            // GAMS.g:251:11: ( ( 'u' | 'U' ) )
            // GAMS.g:251:12: ( 'u' | 'U' )
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
            // GAMS.g:252:11: ( ( 'v' | 'V' ) )
            // GAMS.g:252:12: ( 'v' | 'V' )
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
            // GAMS.g:253:11: ( ( 'w' | 'W' ) )
            // GAMS.g:253:12: ( 'w' | 'W' )
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
            // GAMS.g:254:11: ( ( 'x' | 'X' ) )
            // GAMS.g:254:12: ( 'x' | 'X' )
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
            // GAMS.g:255:11: ( ( 'y' | 'Y' ) )
            // GAMS.g:255:12: ( 'y' | 'Y' )
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
            // GAMS.g:256:11: ( ( 'z' | 'Z' ) )
            // GAMS.g:256:12: ( 'z' | 'Z' )
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
        // GAMS.g:1:8: ( LOG | EXP | LB | RB | RP | DOT | TRUE | FALSE | T__71 | T__72 | T__73 | PLUS | MINUS | SEMI | MULT | DIV | MOD | FRML | STARS | Doubledot | Integer | Double | Ident | WHITESPACE | NEWLINE | Comment | NESTED_ML_COMMENT )
        int alt17 = 27;
        alt17 = dfa17.Predict(input);
        switch (alt17) 
        {
            case 1 :
                // GAMS.g:1:10: LOG
                {
                	mLOG(); 

                }
                break;
            case 2 :
                // GAMS.g:1:14: EXP
                {
                	mEXP(); 

                }
                break;
            case 3 :
                // GAMS.g:1:18: LB
                {
                	mLB(); 

                }
                break;
            case 4 :
                // GAMS.g:1:21: RB
                {
                	mRB(); 

                }
                break;
            case 5 :
                // GAMS.g:1:24: RP
                {
                	mRP(); 

                }
                break;
            case 6 :
                // GAMS.g:1:27: DOT
                {
                	mDOT(); 

                }
                break;
            case 7 :
                // GAMS.g:1:31: TRUE
                {
                	mTRUE(); 

                }
                break;
            case 8 :
                // GAMS.g:1:36: FALSE
                {
                	mFALSE(); 

                }
                break;
            case 9 :
                // GAMS.g:1:42: T__71
                {
                	mT__71(); 

                }
                break;
            case 10 :
                // GAMS.g:1:48: T__72
                {
                	mT__72(); 

                }
                break;
            case 11 :
                // GAMS.g:1:54: T__73
                {
                	mT__73(); 

                }
                break;
            case 12 :
                // GAMS.g:1:60: PLUS
                {
                	mPLUS(); 

                }
                break;
            case 13 :
                // GAMS.g:1:65: MINUS
                {
                	mMINUS(); 

                }
                break;
            case 14 :
                // GAMS.g:1:71: SEMI
                {
                	mSEMI(); 

                }
                break;
            case 15 :
                // GAMS.g:1:76: MULT
                {
                	mMULT(); 

                }
                break;
            case 16 :
                // GAMS.g:1:81: DIV
                {
                	mDIV(); 

                }
                break;
            case 17 :
                // GAMS.g:1:85: MOD
                {
                	mMOD(); 

                }
                break;
            case 18 :
                // GAMS.g:1:89: FRML
                {
                	mFRML(); 

                }
                break;
            case 19 :
                // GAMS.g:1:94: STARS
                {
                	mSTARS(); 

                }
                break;
            case 20 :
                // GAMS.g:1:100: Doubledot
                {
                	mDoubledot(); 

                }
                break;
            case 21 :
                // GAMS.g:1:110: Integer
                {
                	mInteger(); 

                }
                break;
            case 22 :
                // GAMS.g:1:118: Double
                {
                	mDouble(); 

                }
                break;
            case 23 :
                // GAMS.g:1:125: Ident
                {
                	mIdent(); 

                }
                break;
            case 24 :
                // GAMS.g:1:131: WHITESPACE
                {
                	mWHITESPACE(); 

                }
                break;
            case 25 :
                // GAMS.g:1:142: NEWLINE
                {
                	mNEWLINE(); 

                }
                break;
            case 26 :
                // GAMS.g:1:150: Comment
                {
                	mComment(); 

                }
                break;
            case 27 :
                // GAMS.g:1:158: NESTED_ML_COMMENT
                {
                	mNESTED_ML_COMMENT(); 

                }
                break;

        }

    }


    protected DFA8 dfa8;
    protected DFA17 dfa17;
	private void InitializeCyclicDFAs()
	{
	    this.dfa8 = new DFA8(this);
	    this.dfa17 = new DFA17(this);


	}

    const string DFA8_eotS =
        "\x05\uffff";
    const string DFA8_eofS =
        "\x05\uffff";
    const string DFA8_minS =
        "\x02\x2e\x03\uffff";
    const string DFA8_maxS =
        "\x01\x39\x01\x65\x03\uffff";
    const string DFA8_acceptS =
        "\x02\uffff\x01\x02\x01\x03\x01\x01";
    const string DFA8_specialS =
        "\x05\uffff}>";
    static readonly string[] DFA8_transitionS = {
            "\x01\x02\x01\uffff\x0a\x01",
            "\x01\x04\x01\uffff\x0a\x01\x0b\uffff\x01\x03\x1f\uffff\x01"+
            "\x03",
            "",
            "",
            ""
    };

    static readonly short[] DFA8_eot = DFA.UnpackEncodedString(DFA8_eotS);
    static readonly short[] DFA8_eof = DFA.UnpackEncodedString(DFA8_eofS);
    static readonly char[] DFA8_min = DFA.UnpackEncodedStringToUnsignedChars(DFA8_minS);
    static readonly char[] DFA8_max = DFA.UnpackEncodedStringToUnsignedChars(DFA8_maxS);
    static readonly short[] DFA8_accept = DFA.UnpackEncodedString(DFA8_acceptS);
    static readonly short[] DFA8_special = DFA.UnpackEncodedString(DFA8_specialS);
    static readonly short[][] DFA8_transition = DFA.UnpackEncodedStringArray(DFA8_transitionS);

    protected class DFA8 : DFA
    {
        public DFA8(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 8;
            this.eot = DFA8_eot;
            this.eof = DFA8_eof;
            this.min = DFA8_min;
            this.max = DFA8_max;
            this.accept = DFA8_accept;
            this.special = DFA8_special;
            this.transition = DFA8_transition;

        }

        override public string Description
        {
            get { return "199:1: Double : ( ( '0' .. '9' )+ DOT ( '0' .. '9' )* ( Exponent )? | DOT ( '0' .. '9' )+ ( Exponent )? | ( '0' .. '9' )+ Exponent );"; }
        }

    }

    const string DFA17_eotS =
        "\x01\uffff\x02\x14\x03\uffff\x01\x1a\x02\x14\x01\uffff\x01\x1f"+
        "\x04\uffff\x01\x21\x01\x23\x01\uffff\x01\x14\x01\x25\x03\uffff\x02"+
        "\x14\x03\uffff\x02\x14\x06\uffff\x01\x14\x01\uffff\x01\x2b\x01\x2c"+
        "\x03\x14\x02\uffff\x01\x30\x01\x14\x01\x32\x01\uffff\x01\x33\x02"+
        "\uffff";
    const string DFA17_eofS =
        "\x34\uffff";
    const string DFA17_minS =
        "\x01\x09\x01\x4f\x01\x58\x03\uffff\x01\x2e\x01\x72\x01\x61\x01"+
        "\uffff\x01\x29\x04\uffff\x02\x2a\x01\uffff\x01\x52\x01\x2e\x03\uffff"+
        "\x01\x47\x01\x50\x03\uffff\x01\x75\x01\x6c\x06\uffff\x01\x4d\x01"+
        "\uffff\x02\x30\x01\x65\x01\x73\x01\x4c\x02\uffff\x01\x30\x01\x65"+
        "\x01\x30\x01\uffff\x01\x30\x02\uffff";
    const string DFA17_maxS =
        "\x01\x7a\x01\x4f\x01\x58\x03\uffff\x01\x39\x01\x72\x01\x61\x01"+
        "\uffff\x01\x29\x04\uffff\x01\x2a\x01\x2f\x01\uffff\x01\x52\x01\x65"+
        "\x03\uffff\x01\x47\x01\x50\x03\uffff\x01\x75\x01\x6c\x06\uffff\x01"+
        "\x4d\x01\uffff\x02\x7a\x01\x65\x01\x73\x01\x4c\x02\uffff\x01\x7a"+
        "\x01\x65\x01\x7a\x01\uffff\x01\x7a\x02\uffff";
    const string DFA17_acceptS =
        "\x03\uffff\x01\x03\x01\x04\x01\x05\x03\uffff\x01\x09\x01\uffff"+
        "\x01\x0b\x01\x0c\x01\x0d\x01\x0e\x02\uffff\x01\x11\x02\uffff\x01"+
        "\x17\x01\x18\x01\x19\x02\uffff\x01\x14\x01\x06\x01\x16\x02\uffff"+
        "\x01\x1a\x01\x0a\x01\x13\x01\x0f\x01\x1b\x01\x10\x01\uffff\x01\x15"+
        "\x05\uffff\x01\x01\x01\x02\x03\uffff\x01\x07\x01\uffff\x01\x12\x01"+
        "\x08";
    const string DFA17_specialS =
        "\x34\uffff}>";
    static readonly string[] DFA17_transitionS = {
            "\x01\x15\x01\x16\x01\uffff\x01\x15\x01\x16\x12\uffff\x01\x15"+
            "\x04\uffff\x01\x11\x02\uffff\x01\x0a\x01\x05\x01\x0f\x01\x0c"+
            "\x01\x0b\x01\x0d\x01\x06\x01\x10\x0a\x13\x01\uffff\x01\x0e\x01"+
            "\x03\x01\x09\x01\x04\x02\uffff\x04\x14\x01\x02\x01\x12\x05\x14"+
            "\x01\x01\x0e\x14\x04\uffff\x01\x14\x01\uffff\x05\x14\x01\x08"+
            "\x0d\x14\x01\x07\x06\x14",
            "\x01\x17",
            "\x01\x18",
            "",
            "",
            "",
            "\x01\x19\x01\uffff\x0a\x1b",
            "\x01\x1c",
            "\x01\x1d",
            "",
            "\x01\x1e",
            "",
            "",
            "",
            "",
            "\x01\x20",
            "\x01\x22\x04\uffff\x01\x1e",
            "",
            "\x01\x24",
            "\x01\x1b\x01\uffff\x0a\x13\x0b\uffff\x01\x1b\x1f\uffff\x01"+
            "\x1b",
            "",
            "",
            "",
            "\x01\x26",
            "\x01\x27",
            "",
            "",
            "",
            "\x01\x28",
            "\x01\x29",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x2a",
            "",
            "\x0a\x14\x07\uffff\x1a\x14\x04\uffff\x01\x14\x01\uffff\x1a"+
            "\x14",
            "\x0a\x14\x07\uffff\x1a\x14\x04\uffff\x01\x14\x01\uffff\x1a"+
            "\x14",
            "\x01\x2d",
            "\x01\x2e",
            "\x01\x2f",
            "",
            "",
            "\x0a\x14\x07\uffff\x1a\x14\x04\uffff\x01\x14\x01\uffff\x1a"+
            "\x14",
            "\x01\x31",
            "\x0a\x14\x07\uffff\x1a\x14\x04\uffff\x01\x14\x01\uffff\x1a"+
            "\x14",
            "",
            "\x0a\x14\x07\uffff\x1a\x14\x04\uffff\x01\x14\x01\uffff\x1a"+
            "\x14",
            "",
            ""
    };

    static readonly short[] DFA17_eot = DFA.UnpackEncodedString(DFA17_eotS);
    static readonly short[] DFA17_eof = DFA.UnpackEncodedString(DFA17_eofS);
    static readonly char[] DFA17_min = DFA.UnpackEncodedStringToUnsignedChars(DFA17_minS);
    static readonly char[] DFA17_max = DFA.UnpackEncodedStringToUnsignedChars(DFA17_maxS);
    static readonly short[] DFA17_accept = DFA.UnpackEncodedString(DFA17_acceptS);
    static readonly short[] DFA17_special = DFA.UnpackEncodedString(DFA17_specialS);
    static readonly short[][] DFA17_transition = DFA.UnpackEncodedStringArray(DFA17_transitionS);

    protected class DFA17 : DFA
    {
        public DFA17(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 17;
            this.eot = DFA17_eot;
            this.eof = DFA17_eof;
            this.min = DFA17_min;
            this.max = DFA17_max;
            this.accept = DFA17_accept;
            this.special = DFA17_special;
            this.transition = DFA17_transition;

        }

        override public string Description
        {
            get { return "1:1: Tokens : ( LOG | EXP | LB | RB | RP | DOT | TRUE | FALSE | T__71 | T__72 | T__73 | PLUS | MINUS | SEMI | MULT | DIV | MOD | FRML | STARS | Doubledot | Integer | Double | Ident | WHITESPACE | NEWLINE | Comment | NESTED_ML_COMMENT );"; }
        }

    }

 
    
}
}