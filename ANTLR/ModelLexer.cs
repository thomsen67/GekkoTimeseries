// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 Model.g 2019-03-04 11:30:51

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
public partial class ModelLexer : Lexer {
    public const int ASTPOW = 38;
    public const int ASTMODELBLOCK = 22;
    public const int ASTVARIABLE = 28;
    public const int MOD = 46;
    public const int LETTER = 55;
    public const int LOG = 4;
    public const int AFTER2 = 8;
    public const int EOF = -1;
    public const int ASTINTEGER = 26;
    public const int ASTASSIGNVAR = 25;
    public const int HAT = 48;
    public const int T__91 = 91;
    public const int T__90 = 90;
    public const int EXP = 5;
    public const int Comment = 60;
    public const int ASTVAL = 39;
    public const int VAL = 9;
    public const int D = 64;
    public const int Double = 42;
    public const int E = 65;
    public const int F = 66;
    public const int G = 67;
    public const int A = 51;
    public const int B = 62;
    public const int C = 63;
    public const int L = 72;
    public const int M = 53;
    public const int N = 73;
    public const int NESTED_ML_COMMENT = 61;
    public const int O = 74;
    public const int H = 68;
    public const int I = 69;
    public const int J = 70;
    public const int NEWLINE2 = 58;
    public const int K = 71;
    public const int NEWLINE3 = 59;
    public const int U = 79;
    public const int T = 78;
    public const int W = 81;
    public const int WHITESPACE = 56;
    public const int V = 80;
    public const int Q = 52;
    public const int P = 75;
    public const int ASTFRML = 37;
    public const int S = 77;
    public const int MULT = 14;
    public const int R = 76;
    public const int T__85 = 85;
    public const int T__87 = 87;
    public const int T__86 = 86;
    public const int T__89 = 89;
    public const int Y = 83;
    public const int T__88 = 88;
    public const int X = 82;
    public const int ASTAFTER2 = 24;
    public const int Z = 84;
    public const int Ident = 45;
    public const int ASTEXPRESSION = 29;
    public const int VARLIST = 10;
    public const int ASTLAGFUNCTION = 32;
    public const int ASTLAGORLEAD = 36;
    public const int FALSE = 21;
    public const int Modelblock = 41;
    public const int ASTSIMPLEFUNCTION = 34;
    public const int RB = 17;
    public const int RP = 18;
    public const int ASTVARIABLELAGLEAD = 35;
    public const int ASTFUNCTION = 31;
    public const int ASTAFTER = 23;
    public const int AssignVar = 44;
    public const int Exponent = 50;
    public const int ASTVARLIST = 40;
    public const int ASTFRMLCODE = 30;
    public const int PLUS = 11;
    public const int FRML = 6;
    public const int DIGIT = 49;
    public const int DOT = 19;
    public const int NEGATE = 13;
    public const int AFTER = 7;
    public const int MINUS = 12;
    public const int TRUE = 20;
    public const int ASTLEFTSIDE = 33;
    public const int NEWLINE = 57;
    public const int STARS = 47;
    public const int LB = 16;
    public const int ASTDOUBLE = 27;
    public const int DIV = 15;
    public const int DATE = 54;
    public const int Integer = 43;

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


    // delegates
    // delegators

    public ModelLexer() 
    {
		InitializeCyclicDFAs();
    }
    public ModelLexer(ICharStream input)
		: this(input, null) {
    }
    public ModelLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "Model.g";} 
    }

    // $ANTLR start "LOG"
    public void mLOG() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LOG;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:40:5: ( 'LOG' )
            // Model.g:40:7: 'LOG'
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
            // Model.g:41:5: ( 'EXP' )
            // Model.g:41:7: 'EXP'
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

    // $ANTLR start "FRML"
    public void mFRML() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FRML;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:42:6: ( 'FRML' )
            // Model.g:42:8: 'FRML'
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

    // $ANTLR start "AFTER"
    public void mAFTER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AFTER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:43:7: ( 'AFTER' )
            // Model.g:43:9: 'AFTER'
            {
            	Match("AFTER"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "AFTER"

    // $ANTLR start "AFTER2"
    public void mAFTER2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AFTER2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:44:8: ( 'AFTER2' )
            // Model.g:44:10: 'AFTER2'
            {
            	Match("AFTER2"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "AFTER2"

    // $ANTLR start "VAL"
    public void mVAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:45:5: ( 'VAL' )
            // Model.g:45:7: 'VAL'
            {
            	Match("VAL"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VAL"

    // $ANTLR start "VARLIST"
    public void mVARLIST() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VARLIST;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:46:9: ( 'VARLIST' )
            // Model.g:46:11: 'VARLIST'
            {
            	Match("VARLIST"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VARLIST"

    // $ANTLR start "LB"
    public void mLB() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LB;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:47:4: ( '<' )
            // Model.g:47:6: '<'
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
            // Model.g:48:4: ( '>' )
            // Model.g:48:6: '>'
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
            // Model.g:49:4: ( ')' )
            // Model.g:49:6: ')'
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
            // Model.g:50:5: ( '.' )
            // Model.g:50:7: '.'
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
            // Model.g:51:6: ( 'true' )
            // Model.g:51:8: 'true'
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
            // Model.g:52:7: ( 'false' )
            // Model.g:52:9: 'false'
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

    // $ANTLR start "T__85"
    public void mT__85() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__85;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:53:7: ( '=' )
            // Model.g:53:9: '='
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
    // $ANTLR end "T__85"

    // $ANTLR start "T__86"
    public void mT__86() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__86;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:54:7: ( '(' )
            // Model.g:54:9: '('
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
    // $ANTLR end "T__86"

    // $ANTLR start "T__87"
    public void mT__87() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__87;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:55:7: ( ',' )
            // Model.g:55:9: ','
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
    // $ANTLR end "T__87"

    // $ANTLR start "T__88"
    public void mT__88() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__88;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:56:7: ( '[' )
            // Model.g:56:9: '['
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
    // $ANTLR end "T__88"

    // $ANTLR start "T__89"
    public void mT__89() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__89;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:57:7: ( ']' )
            // Model.g:57:9: ']'
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
    // $ANTLR end "T__89"

    // $ANTLR start "T__90"
    public void mT__90() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__90;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:58:7: ( '$' )
            // Model.g:58:9: '$'
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
    // $ANTLR end "T__90"

    // $ANTLR start "T__91"
    public void mT__91() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__91;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:59:7: ( ';' )
            // Model.g:59:9: ';'
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
    // $ANTLR end "T__91"

    // $ANTLR start "PLUS"
    public void mPLUS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = PLUS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:224:6: ( '+' )
            // Model.g:224:8: '+'
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
            // Model.g:225:7: ( '-' )
            // Model.g:225:9: '-'
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

    // $ANTLR start "MULT"
    public void mMULT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MULT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:227:6: ( '*' )
            // Model.g:227:8: '*'
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
            // Model.g:228:5: ( '/' )
            // Model.g:228:7: '/'
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
            // Model.g:229:5: ( '%' )
            // Model.g:229:7: '%'
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

    // $ANTLR start "STARS"
    public void mSTARS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STARS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:232:10: ( '**' )
            // Model.g:232:17: '**'
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

    // $ANTLR start "HAT"
    public void mHAT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = HAT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:233:8: ( '^' )
            // Model.g:233:17: '^'
            {
            	Match('^'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "HAT"

    // $ANTLR start "Integer"
    public void mInteger() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Integer;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:246:9: ( ( DIGIT )+ )
            // Model.g:246:11: ( DIGIT )+
            {
            	// Model.g:246:11: ( DIGIT )+
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
            			    // Model.g:246:12: DIGIT
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
            // Model.g:249:5: ( ( '0' .. '9' )+ DOT ( '0' .. '9' )* ( Exponent )? | DOT ( '0' .. '9' )+ ( Exponent )? | ( '0' .. '9' )+ Exponent )
            int alt8 = 3;
            alt8 = dfa8.Predict(input);
            switch (alt8) 
            {
                case 1 :
                    // Model.g:250:8: ( '0' .. '9' )+ DOT ( '0' .. '9' )* ( Exponent )?
                    {
                    	// Model.g:250:8: ( '0' .. '9' )+
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
                    			    // Model.g:250:9: '0' .. '9'
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
                    	// Model.g:250:26: ( '0' .. '9' )*
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
                    			    // Model.g:250:27: '0' .. '9'
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

                    	// Model.g:250:40: ( Exponent )?
                    	int alt4 = 2;
                    	int LA4_0 = input.LA(1);

                    	if ( (LA4_0 == 'E' || LA4_0 == 'e') )
                    	{
                    	    alt4 = 1;
                    	}
                    	switch (alt4) 
                    	{
                    	    case 1 :
                    	        // Model.g:250:40: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // Model.g:251:9: DOT ( '0' .. '9' )+ ( Exponent )?
                    {
                    	mDOT(); 
                    	// Model.g:251:13: ( '0' .. '9' )+
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
                    			    // Model.g:251:15: '0' .. '9'
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

                    	// Model.g:251:29: ( Exponent )?
                    	int alt6 = 2;
                    	int LA6_0 = input.LA(1);

                    	if ( (LA6_0 == 'E' || LA6_0 == 'e') )
                    	{
                    	    alt6 = 1;
                    	}
                    	switch (alt6) 
                    	{
                    	    case 1 :
                    	        // Model.g:251:29: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 3 :
                    // Model.g:252:8: ( '0' .. '9' )+ Exponent
                    {
                    	// Model.g:252:8: ( '0' .. '9' )+
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
                    			    // Model.g:252:9: '0' .. '9'
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
            // Model.g:257:5: ( ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+ )
            // Model.g:257:9: ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+
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

            	// Model.g:257:23: ( '+' | '-' )?
            	int alt9 = 2;
            	int LA9_0 = input.LA(1);

            	if ( (LA9_0 == '+' || LA9_0 == '-') )
            	{
            	    alt9 = 1;
            	}
            	switch (alt9) 
            	{
            	    case 1 :
            	        // Model.g:
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

            	// Model.g:257:38: ( '0' .. '9' )+
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
            			    // Model.g:257:40: '0' .. '9'
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

    // $ANTLR start "DATE"
    public void mDATE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DATE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:260:6: ( ( '0' .. '9' )+ ( A | Q | M ) ( '0' .. '9' )+ )
            // Model.g:260:10: ( '0' .. '9' )+ ( A | Q | M ) ( '0' .. '9' )+
            {
            	// Model.g:260:10: ( '0' .. '9' )+
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
            			    // Model.g:260:11: '0' .. '9'
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

            	if ( input.LA(1) == 'A' || input.LA(1) == 'M' || input.LA(1) == 'Q' || input.LA(1) == 'a' || input.LA(1) == 'm' || input.LA(1) == 'q' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// Model.g:260:32: ( '0' .. '9' )+
            	int cnt12 = 0;
            	do 
            	{
            	    int alt12 = 2;
            	    int LA12_0 = input.LA(1);

            	    if ( ((LA12_0 >= '0' && LA12_0 <= '9')) )
            	    {
            	        alt12 = 1;
            	    }


            	    switch (alt12) 
            		{
            			case 1 :
            			    // Model.g:260:33: '0' .. '9'
            			    {
            			    	MatchRange('0','9'); 

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


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DATE"

    // $ANTLR start "AssignVar"
    public void mAssignVar() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AssignVar;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:262:11: ( '%' ( LETTER | '_' ) ( DIGIT | LETTER | '_' )* )
            // Model.g:262:13: '%' ( LETTER | '_' ) ( DIGIT | LETTER | '_' )*
            {
            	Match('%'); 
            	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// Model.g:262:30: ( DIGIT | LETTER | '_' )*
            	do 
            	{
            	    int alt13 = 2;
            	    int LA13_0 = input.LA(1);

            	    if ( ((LA13_0 >= '0' && LA13_0 <= '9') || (LA13_0 >= 'A' && LA13_0 <= 'Z') || LA13_0 == '_' || (LA13_0 >= 'a' && LA13_0 <= 'z')) )
            	    {
            	        alt13 = 1;
            	    }


            	    switch (alt13) 
            		{
            			case 1 :
            			    // Model.g:
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
            			    goto loop13;
            	    }
            	} while (true);

            	loop13:
            		;	// Stops C# compiler whining that label 'loop13' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "AssignVar"

    // $ANTLR start "Ident"
    public void mIdent() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Ident;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:264:7: ( ( LETTER | '_' ) ( DIGIT | LETTER | '_' )* )
            // Model.g:264:9: ( LETTER | '_' ) ( DIGIT | LETTER | '_' )*
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

            	// Model.g:264:22: ( DIGIT | LETTER | '_' )*
            	do 
            	{
            	    int alt14 = 2;
            	    int LA14_0 = input.LA(1);

            	    if ( ((LA14_0 >= '0' && LA14_0 <= '9') || (LA14_0 >= 'A' && LA14_0 <= 'Z') || LA14_0 == '_' || (LA14_0 >= 'a' && LA14_0 <= 'z')) )
            	    {
            	        alt14 = 1;
            	    }


            	    switch (alt14) 
            		{
            			case 1 :
            			    // Model.g:
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
            			    goto loop14;
            	    }
            	} while (true);

            	loop14:
            		;	// Stops C# compiler whining that label 'loop14' has no statements

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
            // Model.g:266:12: ( ( '\\t' | ' ' | '\\u000C' )+ )
            // Model.g:266:14: ( '\\t' | ' ' | '\\u000C' )+
            {
            	// Model.g:266:14: ( '\\t' | ' ' | '\\u000C' )+
            	int cnt15 = 0;
            	do 
            	{
            	    int alt15 = 2;
            	    int LA15_0 = input.LA(1);

            	    if ( (LA15_0 == '\t' || LA15_0 == '\f' || LA15_0 == ' ') )
            	    {
            	        alt15 = 1;
            	    }


            	    switch (alt15) 
            		{
            			case 1 :
            			    // Model.g:
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
            			    if ( cnt15 >= 1 ) goto loop15;
            		            EarlyExitException eee15 =
            		                new EarlyExitException(15, input);
            		            throw eee15;
            	    }
            	    cnt15++;
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
    // $ANTLR end "WHITESPACE"

    // $ANTLR start "NEWLINE"
    public void mNEWLINE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NEWLINE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:268:12: ( ( ( '\\r' )? '\\n' ) )
            // Model.g:268:16: ( ( '\\r' )? '\\n' )
            {
            	// Model.g:268:16: ( ( '\\r' )? '\\n' )
            	// Model.g:268:17: ( '\\r' )? '\\n'
            	{
            		// Model.g:268:17: ( '\\r' )?
            		int alt16 = 2;
            		int LA16_0 = input.LA(1);

            		if ( (LA16_0 == '\r') )
            		{
            		    alt16 = 1;
            		}
            		switch (alt16) 
            		{
            		    case 1 :
            		        // Model.g:268:18: '\\r'
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

    // $ANTLR start "Modelblock"
    public void mModelblock() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Modelblock;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:270:12: ( ( '()' | '//' ) (~ ( NEWLINE2 | NEWLINE3 | '#' ) )* '###' (~ ( NEWLINE2 | NEWLINE3 | '#' ) )* '###' (~ ( NEWLINE2 | NEWLINE3 | '#' ) )* )
            // Model.g:270:14: ( '()' | '//' ) (~ ( NEWLINE2 | NEWLINE3 | '#' ) )* '###' (~ ( NEWLINE2 | NEWLINE3 | '#' ) )* '###' (~ ( NEWLINE2 | NEWLINE3 | '#' ) )*
            {
            	// Model.g:270:14: ( '()' | '//' )
            	int alt17 = 2;
            	int LA17_0 = input.LA(1);

            	if ( (LA17_0 == '(') )
            	{
            	    alt17 = 1;
            	}
            	else if ( (LA17_0 == '/') )
            	{
            	    alt17 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d17s0 =
            	        new NoViableAltException("", 17, 0, input);

            	    throw nvae_d17s0;
            	}
            	switch (alt17) 
            	{
            	    case 1 :
            	        // Model.g:270:15: '()'
            	        {
            	        	Match("()"); 


            	        }
            	        break;
            	    case 2 :
            	        // Model.g:270:22: '//'
            	        {
            	        	Match("//"); 


            	        }
            	        break;

            	}

            	// Model.g:270:28: (~ ( NEWLINE2 | NEWLINE3 | '#' ) )*
            	do 
            	{
            	    int alt18 = 2;
            	    int LA18_0 = input.LA(1);

            	    if ( ((LA18_0 >= '\u0000' && LA18_0 <= '\t') || (LA18_0 >= '\u000B' && LA18_0 <= '\"') || (LA18_0 >= '$' && LA18_0 <= '\uFFFF')) )
            	    {
            	        alt18 = 1;
            	    }


            	    switch (alt18) 
            		{
            			case 1 :
            			    // Model.g:270:29: ~ ( NEWLINE2 | NEWLINE3 | '#' )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\"') || (input.LA(1) >= '$' && input.LA(1) <= '\uFFFF') ) 
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

            	Match("###"); 

            	// Model.g:270:63: (~ ( NEWLINE2 | NEWLINE3 | '#' ) )*
            	do 
            	{
            	    int alt19 = 2;
            	    int LA19_0 = input.LA(1);

            	    if ( ((LA19_0 >= '\u0000' && LA19_0 <= '\t') || (LA19_0 >= '\u000B' && LA19_0 <= '\"') || (LA19_0 >= '$' && LA19_0 <= '\uFFFF')) )
            	    {
            	        alt19 = 1;
            	    }


            	    switch (alt19) 
            		{
            			case 1 :
            			    // Model.g:270:64: ~ ( NEWLINE2 | NEWLINE3 | '#' )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\"') || (input.LA(1) >= '$' && input.LA(1) <= '\uFFFF') ) 
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
            			    goto loop19;
            	    }
            	} while (true);

            	loop19:
            		;	// Stops C# compiler whining that label 'loop19' has no statements

            	Match("###"); 

            	// Model.g:270:97: (~ ( NEWLINE2 | NEWLINE3 | '#' ) )*
            	do 
            	{
            	    int alt20 = 2;
            	    int LA20_0 = input.LA(1);

            	    if ( ((LA20_0 >= '\u0000' && LA20_0 <= '\t') || (LA20_0 >= '\u000B' && LA20_0 <= '\"') || (LA20_0 >= '$' && LA20_0 <= '\uFFFF')) )
            	    {
            	        alt20 = 1;
            	    }


            	    switch (alt20) 
            		{
            			case 1 :
            			    // Model.g:270:98: ~ ( NEWLINE2 | NEWLINE3 | '#' )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\"') || (input.LA(1) >= '$' && input.LA(1) <= '\uFFFF') ) 
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
            			    goto loop20;
            	    }
            	} while (true);

            	loop20:
            		;	// Stops C# compiler whining that label 'loop20' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "Modelblock"

    // $ANTLR start "Comment"
    public void mComment() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Comment;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Model.g:272:12: ( ( '()' | '//' ) (~ ( NEWLINE2 | NEWLINE3 ) )* )
            // Model.g:272:14: ( '()' | '//' ) (~ ( NEWLINE2 | NEWLINE3 ) )*
            {
            	// Model.g:272:14: ( '()' | '//' )
            	int alt21 = 2;
            	int LA21_0 = input.LA(1);

            	if ( (LA21_0 == '(') )
            	{
            	    alt21 = 1;
            	}
            	else if ( (LA21_0 == '/') )
            	{
            	    alt21 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d21s0 =
            	        new NoViableAltException("", 21, 0, input);

            	    throw nvae_d21s0;
            	}
            	switch (alt21) 
            	{
            	    case 1 :
            	        // Model.g:272:15: '()'
            	        {
            	        	Match("()"); 


            	        }
            	        break;
            	    case 2 :
            	        // Model.g:272:22: '//'
            	        {
            	        	Match("//"); 


            	        }
            	        break;

            	}

            	// Model.g:272:28: (~ ( NEWLINE2 | NEWLINE3 ) )*
            	do 
            	{
            	    int alt22 = 2;
            	    int LA22_0 = input.LA(1);

            	    if ( ((LA22_0 >= '\u0000' && LA22_0 <= '\t') || (LA22_0 >= '\u000B' && LA22_0 <= '\uFFFF')) )
            	    {
            	        alt22 = 1;
            	    }


            	    switch (alt22) 
            		{
            			case 1 :
            			    // Model.g:272:29: ~ ( NEWLINE2 | NEWLINE3 )
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
            			    goto loop22;
            	    }
            	} while (true);

            	loop22:
            		;	// Stops C# compiler whining that label 'loop22' has no statements

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
            // Model.g:276:5: ( '/*' ( options {greedy=false; } : NESTED_ML_COMMENT | . )* '*/' )
            // Model.g:276:9: '/*' ( options {greedy=false; } : NESTED_ML_COMMENT | . )* '*/'
            {
            	Match("/*"); 

            	// Model.g:277:9: ( options {greedy=false; } : NESTED_ML_COMMENT | . )*
            	do 
            	{
            	    int alt23 = 3;
            	    int LA23_0 = input.LA(1);

            	    if ( (LA23_0 == '*') )
            	    {
            	        int LA23_1 = input.LA(2);

            	        if ( (LA23_1 == '/') )
            	        {
            	            alt23 = 3;
            	        }
            	        else if ( ((LA23_1 >= '\u0000' && LA23_1 <= '.') || (LA23_1 >= '0' && LA23_1 <= '\uFFFF')) )
            	        {
            	            alt23 = 2;
            	        }


            	    }
            	    else if ( (LA23_0 == '/') )
            	    {
            	        int LA23_2 = input.LA(2);

            	        if ( (LA23_2 == '*') )
            	        {
            	            alt23 = 1;
            	        }
            	        else if ( ((LA23_2 >= '\u0000' && LA23_2 <= ')') || (LA23_2 >= '+' && LA23_2 <= '\uFFFF')) )
            	        {
            	            alt23 = 2;
            	        }


            	    }
            	    else if ( ((LA23_0 >= '\u0000' && LA23_0 <= ')') || (LA23_0 >= '+' && LA23_0 <= '.') || (LA23_0 >= '0' && LA23_0 <= '\uFFFF')) )
            	    {
            	        alt23 = 2;
            	    }


            	    switch (alt23) 
            		{
            			case 1 :
            			    // Model.g:277:36: NESTED_ML_COMMENT
            			    {
            			    	mNESTED_ML_COMMENT(); 

            			    }
            			    break;
            			case 2 :
            			    // Model.g:277:56: .
            			    {
            			    	MatchAny(); 

            			    }
            			    break;

            			default:
            			    goto loop23;
            	    }
            	} while (true);

            	loop23:
            		;	// Stops C# compiler whining that label 'loop23' has no statements

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
            // Model.g:282:19: ( '\\n' )
            // Model.g:282:21: '\\n'
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
            // Model.g:283:19: ( '\\r\\n' )
            // Model.g:283:21: '\\r\\n'
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
            // Model.g:284:16: ( '0' .. '9' )
            // Model.g:284:18: '0' .. '9'
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
            // Model.g:285:16: ( 'a' .. 'z' | 'A' .. 'Z' )
            // Model.g:
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
            // Model.g:287:11: ( ( 'a' | 'A' ) )
            // Model.g:287:12: ( 'a' | 'A' )
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
            // Model.g:288:11: ( ( 'b' | 'B' ) )
            // Model.g:288:12: ( 'b' | 'B' )
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
            // Model.g:289:11: ( ( 'c' | 'C' ) )
            // Model.g:289:12: ( 'c' | 'C' )
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
            // Model.g:290:11: ( ( 'd' | 'D' ) )
            // Model.g:290:12: ( 'd' | 'D' )
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
            // Model.g:291:11: ( ( 'e' | 'E' ) )
            // Model.g:291:12: ( 'e' | 'E' )
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
            // Model.g:292:11: ( ( 'f' | 'F' ) )
            // Model.g:292:12: ( 'f' | 'F' )
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
            // Model.g:293:11: ( ( 'g' | 'G' ) )
            // Model.g:293:12: ( 'g' | 'G' )
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
            // Model.g:294:11: ( ( 'h' | 'H' ) )
            // Model.g:294:12: ( 'h' | 'H' )
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
            // Model.g:295:11: ( ( 'i' | 'I' ) )
            // Model.g:295:12: ( 'i' | 'I' )
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
            // Model.g:296:11: ( ( 'j' | 'J' ) )
            // Model.g:296:12: ( 'j' | 'J' )
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
            // Model.g:297:11: ( ( 'k' | 'K' ) )
            // Model.g:297:12: ( 'k' | 'K' )
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
            // Model.g:298:11: ( ( 'l' | 'L' ) )
            // Model.g:298:12: ( 'l' | 'L' )
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
            // Model.g:299:11: ( ( 'm' | 'M' ) )
            // Model.g:299:12: ( 'm' | 'M' )
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
            // Model.g:300:11: ( ( 'n' | 'N' ) )
            // Model.g:300:12: ( 'n' | 'N' )
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
            // Model.g:301:11: ( ( 'o' | 'O' ) )
            // Model.g:301:12: ( 'o' | 'O' )
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
            // Model.g:302:11: ( ( 'p' | 'P' ) )
            // Model.g:302:12: ( 'p' | 'P' )
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
            // Model.g:303:11: ( ( 'q' | 'Q' ) )
            // Model.g:303:12: ( 'q' | 'Q' )
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
            // Model.g:304:11: ( ( 'r' | 'R' ) )
            // Model.g:304:12: ( 'r' | 'R' )
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
            // Model.g:305:11: ( ( 's' | 'S' ) )
            // Model.g:305:12: ( 's' | 'S' )
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
            // Model.g:306:11: ( ( 't' | 'T' ) )
            // Model.g:306:12: ( 't' | 'T' )
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
            // Model.g:307:11: ( ( 'u' | 'U' ) )
            // Model.g:307:12: ( 'u' | 'U' )
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
            // Model.g:308:11: ( ( 'v' | 'V' ) )
            // Model.g:308:12: ( 'v' | 'V' )
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
            // Model.g:309:11: ( ( 'w' | 'W' ) )
            // Model.g:309:12: ( 'w' | 'W' )
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
            // Model.g:310:11: ( ( 'x' | 'X' ) )
            // Model.g:310:12: ( 'x' | 'X' )
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
            // Model.g:311:11: ( ( 'y' | 'Y' ) )
            // Model.g:311:12: ( 'y' | 'Y' )
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
            // Model.g:312:11: ( ( 'z' | 'Z' ) )
            // Model.g:312:12: ( 'z' | 'Z' )
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
        // Model.g:1:8: ( LOG | EXP | FRML | AFTER | AFTER2 | VAL | VARLIST | LB | RB | RP | DOT | TRUE | FALSE | T__85 | T__86 | T__87 | T__88 | T__89 | T__90 | T__91 | PLUS | MINUS | MULT | DIV | MOD | STARS | HAT | Integer | Double | DATE | AssignVar | Ident | WHITESPACE | NEWLINE | Modelblock | Comment | NESTED_ML_COMMENT )
        int alt24 = 37;
        alt24 = dfa24.Predict(input);
        switch (alt24) 
        {
            case 1 :
                // Model.g:1:10: LOG
                {
                	mLOG(); 

                }
                break;
            case 2 :
                // Model.g:1:14: EXP
                {
                	mEXP(); 

                }
                break;
            case 3 :
                // Model.g:1:18: FRML
                {
                	mFRML(); 

                }
                break;
            case 4 :
                // Model.g:1:23: AFTER
                {
                	mAFTER(); 

                }
                break;
            case 5 :
                // Model.g:1:29: AFTER2
                {
                	mAFTER2(); 

                }
                break;
            case 6 :
                // Model.g:1:36: VAL
                {
                	mVAL(); 

                }
                break;
            case 7 :
                // Model.g:1:40: VARLIST
                {
                	mVARLIST(); 

                }
                break;
            case 8 :
                // Model.g:1:48: LB
                {
                	mLB(); 

                }
                break;
            case 9 :
                // Model.g:1:51: RB
                {
                	mRB(); 

                }
                break;
            case 10 :
                // Model.g:1:54: RP
                {
                	mRP(); 

                }
                break;
            case 11 :
                // Model.g:1:57: DOT
                {
                	mDOT(); 

                }
                break;
            case 12 :
                // Model.g:1:61: TRUE
                {
                	mTRUE(); 

                }
                break;
            case 13 :
                // Model.g:1:66: FALSE
                {
                	mFALSE(); 

                }
                break;
            case 14 :
                // Model.g:1:72: T__85
                {
                	mT__85(); 

                }
                break;
            case 15 :
                // Model.g:1:78: T__86
                {
                	mT__86(); 

                }
                break;
            case 16 :
                // Model.g:1:84: T__87
                {
                	mT__87(); 

                }
                break;
            case 17 :
                // Model.g:1:90: T__88
                {
                	mT__88(); 

                }
                break;
            case 18 :
                // Model.g:1:96: T__89
                {
                	mT__89(); 

                }
                break;
            case 19 :
                // Model.g:1:102: T__90
                {
                	mT__90(); 

                }
                break;
            case 20 :
                // Model.g:1:108: T__91
                {
                	mT__91(); 

                }
                break;
            case 21 :
                // Model.g:1:114: PLUS
                {
                	mPLUS(); 

                }
                break;
            case 22 :
                // Model.g:1:119: MINUS
                {
                	mMINUS(); 

                }
                break;
            case 23 :
                // Model.g:1:125: MULT
                {
                	mMULT(); 

                }
                break;
            case 24 :
                // Model.g:1:130: DIV
                {
                	mDIV(); 

                }
                break;
            case 25 :
                // Model.g:1:134: MOD
                {
                	mMOD(); 

                }
                break;
            case 26 :
                // Model.g:1:138: STARS
                {
                	mSTARS(); 

                }
                break;
            case 27 :
                // Model.g:1:144: HAT
                {
                	mHAT(); 

                }
                break;
            case 28 :
                // Model.g:1:148: Integer
                {
                	mInteger(); 

                }
                break;
            case 29 :
                // Model.g:1:156: Double
                {
                	mDouble(); 

                }
                break;
            case 30 :
                // Model.g:1:163: DATE
                {
                	mDATE(); 

                }
                break;
            case 31 :
                // Model.g:1:168: AssignVar
                {
                	mAssignVar(); 

                }
                break;
            case 32 :
                // Model.g:1:178: Ident
                {
                	mIdent(); 

                }
                break;
            case 33 :
                // Model.g:1:184: WHITESPACE
                {
                	mWHITESPACE(); 

                }
                break;
            case 34 :
                // Model.g:1:195: NEWLINE
                {
                	mNEWLINE(); 

                }
                break;
            case 35 :
                // Model.g:1:203: Modelblock
                {
                	mModelblock(); 

                }
                break;
            case 36 :
                // Model.g:1:214: Comment
                {
                	mComment(); 

                }
                break;
            case 37 :
                // Model.g:1:222: NESTED_ML_COMMENT
                {
                	mNESTED_ML_COMMENT(); 

                }
                break;

        }

    }


    protected DFA8 dfa8;
    protected DFA24 dfa24;
	private void InitializeCyclicDFAs()
	{
	    this.dfa8 = new DFA8(this);
	    this.dfa24 = new DFA24(this);

	    this.dfa24.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA24_SpecialStateTransition);
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
        "\x02\uffff\x01\x02\x01\x01\x01\x03";
    const string DFA8_specialS =
        "\x05\uffff}>";
    static readonly string[] DFA8_transitionS = {
            "\x01\x02\x01\uffff\x0a\x01",
            "\x01\x03\x01\uffff\x0a\x01\x0b\uffff\x01\x04\x1f\uffff\x01"+
            "\x04",
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
            get { return "248:1: Double : ( ( '0' .. '9' )+ DOT ( '0' .. '9' )* ( Exponent )? | DOT ( '0' .. '9' )+ ( Exponent )? | ( '0' .. '9' )+ Exponent );"; }
        }

    }

    const string DFA24_eotS =
        "\x01\uffff\x05\x1a\x03\uffff\x01\x22\x02\x1a\x01\uffff\x01\x27"+
        "\x07\uffff\x01\x29\x01\x2c\x01\x2e\x01\uffff\x01\x2f\x03\uffff\x05"+
        "\x1a\x02\uffff\x02\x1a\x01\x3a\x03\uffff\x01\x3a\x06\uffff\x01\x3c"+
        "\x01\x3d\x02\x1a\x01\x40\x03\x1a\x01\x3a\x01\uffff\x01\x3a\x02\uffff"+
        "\x01\x45\x01\x1a\x01\uffff\x01\x1a\x01\x48\x01\x1a\x01\x3a\x01\uffff"+
        "\x01\x4c\x01\x1a\x01\uffff\x01\x4e\x01\x3a\x01\x51\x01\uffff\x01"+
        "\x1a\x01\uffff\x02\x3a\x01\uffff\x01\x54\x01\x3a\x01\uffff\x02\x57"+
        "\x01\uffff";
    const string DFA24_eofS =
        "\x58\uffff";
    const string DFA24_minS =
        "\x01\x09\x01\x4f\x01\x58\x01\x52\x01\x46\x01\x41\x03\uffff\x01"+
        "\x30\x01\x72\x01\x61\x01\uffff\x01\x29\x07\uffff\x02\x2a\x01\x41"+
        "\x01\uffff\x01\x2e\x03\uffff\x01\x47\x01\x50\x01\x4d\x01\x54\x01"+
        "\x4c\x02\uffff\x01\x75\x01\x6c\x01\x00\x03\uffff\x01\x00\x06\uffff"+
        "\x02\x30\x01\x4c\x01\x45\x01\x30\x01\x4c\x01\x65\x01\x73\x01\x00"+
        "\x01\uffff\x01\x23\x02\uffff\x01\x30\x01\x52\x01\uffff\x01\x49\x01"+
        "\x30\x01\x65\x01\x23\x01\uffff\x01\x30\x01\x53\x01\uffff\x01\x30"+
        "\x01\x00\x01\x30\x01\uffff\x01\x54\x01\uffff\x01\x00\x01\x23\x01"+
        "\uffff\x01\x30\x01\x23\x01\uffff\x02\x00\x01\uffff";
    const string DFA24_maxS =
        "\x01\x7a\x01\x4f\x01\x58\x01\x52\x01\x46\x01\x41\x03\uffff\x01"+
        "\x39\x01\x72\x01\x61\x01\uffff\x01\x29\x07\uffff\x01\x2a\x01\x2f"+
        "\x01\x7a\x01\uffff\x01\x71\x03\uffff\x01\x47\x01\x50\x01\x4d\x01"+
        "\x54\x01\x52\x02\uffff\x01\x75\x01\x6c\x01\uffff\x03\uffff\x01\uffff"+
        "\x06\uffff\x02\x7a\x01\x4c\x01\x45\x01\x7a\x01\x4c\x01\x65\x01\x73"+
        "\x01\uffff\x01\uffff\x01\x23\x02\uffff\x01\x7a\x01\x52\x01\uffff"+
        "\x01\x49\x01\x7a\x01\x65\x01\x23\x01\uffff\x01\x7a\x01\x53\x01\uffff"+
        "\x01\x7a\x01\uffff\x01\x7a\x01\uffff\x01\x54\x01\uffff\x01\uffff"+
        "\x01\x23\x01\uffff\x01\x7a\x01\x23\x01\uffff\x02\uffff\x01\uffff";
    const string DFA24_acceptS =
        "\x06\uffff\x01\x08\x01\x09\x01\x0a\x03\uffff\x01\x0e\x01\uffff"+
        "\x01\x10\x01\x11\x01\x12\x01\x13\x01\x14\x01\x15\x01\x16\x03\uffff"+
        "\x01\x1b\x01\uffff\x01\x20\x01\x21\x01\x22\x05\uffff\x01\x0b\x01"+
        "\x1d\x03\uffff\x01\x0f\x01\x1a\x01\x17\x01\uffff\x01\x25\x01\x18"+
        "\x01\x1f\x01\x19\x01\x1c\x01\x1e\x09\uffff\x01\x24\x01\uffff\x01"+
        "\x01\x01\x02\x02\uffff\x01\x06\x04\uffff\x01\x03\x02\uffff\x01\x0c"+
        "\x03\uffff\x01\x04\x01\uffff\x01\x0d\x02\uffff\x01\x05\x02\uffff"+
        "\x01\x07\x02\uffff\x01\x23";
    const string DFA24_specialS =
        "\x26\uffff\x01\x04\x03\uffff\x01\x05\x0e\uffff\x01\x03\x10\uffff"+
        "\x01\x06\x04\uffff\x01\x01\x05\uffff\x01\x00\x01\x02\x01\uffff}>";
    static readonly string[] DFA24_transitionS = {
            "\x01\x1b\x01\x1c\x01\uffff\x01\x1b\x01\x1c\x12\uffff\x01\x1b"+
            "\x03\uffff\x01\x11\x01\x17\x02\uffff\x01\x0d\x01\x08\x01\x15"+
            "\x01\x13\x01\x0e\x01\x14\x01\x09\x01\x16\x0a\x19\x01\uffff\x01"+
            "\x12\x01\x06\x01\x0c\x01\x07\x02\uffff\x01\x04\x03\x1a\x01\x02"+
            "\x01\x03\x05\x1a\x01\x01\x09\x1a\x01\x05\x04\x1a\x01\x0f\x01"+
            "\uffff\x01\x10\x01\x18\x01\x1a\x01\uffff\x05\x1a\x01\x0b\x0d"+
            "\x1a\x01\x0a\x06\x1a",
            "\x01\x1d",
            "\x01\x1e",
            "\x01\x1f",
            "\x01\x20",
            "\x01\x21",
            "",
            "",
            "",
            "\x0a\x23",
            "\x01\x24",
            "\x01\x25",
            "",
            "\x01\x26",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x28",
            "\x01\x2b\x04\uffff\x01\x2a",
            "\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a\x2d",
            "",
            "\x01\x23\x01\uffff\x0a\x19\x07\uffff\x01\x30\x03\uffff\x01"+
            "\x23\x07\uffff\x01\x30\x03\uffff\x01\x30\x0f\uffff\x01\x30\x03"+
            "\uffff\x01\x23\x07\uffff\x01\x30\x03\uffff\x01\x30",
            "",
            "",
            "",
            "\x01\x31",
            "\x01\x32",
            "\x01\x33",
            "\x01\x34",
            "\x01\x35\x05\uffff\x01\x36",
            "",
            "",
            "\x01\x37",
            "\x01\x38",
            "\x0a\x39\x01\uffff\x18\x39\x01\x3b\uffdc\x39",
            "",
            "",
            "",
            "\x0a\x39\x01\uffff\x18\x39\x01\x3b\uffdc\x39",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x01\x3e",
            "\x01\x3f",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x01\x41",
            "\x01\x42",
            "\x01\x43",
            "\x0a\x39\x01\uffff\x18\x39\x01\x3b\uffdc\x39",
            "",
            "\x01\x44",
            "",
            "",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x01\x46",
            "",
            "\x01\x47",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x01\x49",
            "\x01\x4a",
            "",
            "\x02\x1a\x01\x4b\x07\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a"+
            "\x01\uffff\x1a\x1a",
            "\x01\x4d",
            "",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x0a\x4f\x01\uffff\x18\x4f\x01\x50\uffdc\x4f",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "",
            "\x01\x52",
            "",
            "\x0a\x4f\x01\uffff\x18\x4f\x01\x50\uffdc\x4f",
            "\x01\x53",
            "",
            "\x0a\x1a\x07\uffff\x1a\x1a\x04\uffff\x01\x1a\x01\uffff\x1a"+
            "\x1a",
            "\x01\x55",
            "",
            "\x0a\x56\x01\uffff\x18\x56\x01\x3a\uffdc\x56",
            "\x0a\x56\x01\uffff\x18\x56\x01\x3a\uffdc\x56",
            ""
    };

    static readonly short[] DFA24_eot = DFA.UnpackEncodedString(DFA24_eotS);
    static readonly short[] DFA24_eof = DFA.UnpackEncodedString(DFA24_eofS);
    static readonly char[] DFA24_min = DFA.UnpackEncodedStringToUnsignedChars(DFA24_minS);
    static readonly char[] DFA24_max = DFA.UnpackEncodedStringToUnsignedChars(DFA24_maxS);
    static readonly short[] DFA24_accept = DFA.UnpackEncodedString(DFA24_acceptS);
    static readonly short[] DFA24_special = DFA.UnpackEncodedString(DFA24_specialS);
    static readonly short[][] DFA24_transition = DFA.UnpackEncodedStringArray(DFA24_transitionS);

    protected class DFA24 : DFA
    {
        public DFA24(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 24;
            this.eot = DFA24_eot;
            this.eof = DFA24_eof;
            this.min = DFA24_min;
            this.max = DFA24_max;
            this.accept = DFA24_accept;
            this.special = DFA24_special;
            this.transition = DFA24_transition;

        }

        override public string Description
        {
            get { return "1:1: Tokens : ( LOG | EXP | FRML | AFTER | AFTER2 | VAL | VARLIST | LB | RB | RP | DOT | TRUE | FALSE | T__85 | T__86 | T__87 | T__88 | T__89 | T__90 | T__91 | PLUS | MINUS | MULT | DIV | MOD | STARS | HAT | Integer | Double | DATE | AssignVar | Ident | WHITESPACE | NEWLINE | Modelblock | Comment | NESTED_ML_COMMENT );"; }
        }

    }


    protected internal int DFA24_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            IIntStream input = _input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA24_85 = input.LA(1);

                   	s = -1;
                   	if ( ((LA24_85 >= '\u0000' && LA24_85 <= '\t') || (LA24_85 >= '\u000B' && LA24_85 <= '\"') || (LA24_85 >= '$' && LA24_85 <= '\uFFFF')) ) { s = 86; }

                   	else if ( (LA24_85 == '#') ) { s = 58; }

                   	else s = 87;

                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA24_79 = input.LA(1);

                   	s = -1;
                   	if ( (LA24_79 == '#') ) { s = 80; }

                   	else if ( ((LA24_79 >= '\u0000' && LA24_79 <= '\t') || (LA24_79 >= '\u000B' && LA24_79 <= '\"') || (LA24_79 >= '$' && LA24_79 <= '\uFFFF')) ) { s = 79; }

                   	else s = 58;

                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA24_86 = input.LA(1);

                   	s = -1;
                   	if ( ((LA24_86 >= '\u0000' && LA24_86 <= '\t') || (LA24_86 >= '\u000B' && LA24_86 <= '\"') || (LA24_86 >= '$' && LA24_86 <= '\uFFFF')) ) { s = 86; }

                   	else if ( (LA24_86 == '#') ) { s = 58; }

                   	else s = 87;

                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA24_57 = input.LA(1);

                   	s = -1;
                   	if ( (LA24_57 == '#') ) { s = 59; }

                   	else if ( ((LA24_57 >= '\u0000' && LA24_57 <= '\t') || (LA24_57 >= '\u000B' && LA24_57 <= '\"') || (LA24_57 >= '$' && LA24_57 <= '\uFFFF')) ) { s = 57; }

                   	else s = 58;

                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA24_38 = input.LA(1);

                   	s = -1;
                   	if ( ((LA24_38 >= '\u0000' && LA24_38 <= '\t') || (LA24_38 >= '\u000B' && LA24_38 <= '\"') || (LA24_38 >= '$' && LA24_38 <= '\uFFFF')) ) { s = 57; }

                   	else if ( (LA24_38 == '#') ) { s = 59; }

                   	else s = 58;

                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA24_42 = input.LA(1);

                   	s = -1;
                   	if ( ((LA24_42 >= '\u0000' && LA24_42 <= '\t') || (LA24_42 >= '\u000B' && LA24_42 <= '\"') || (LA24_42 >= '$' && LA24_42 <= '\uFFFF')) ) { s = 57; }

                   	else if ( (LA24_42 == '#') ) { s = 59; }

                   	else s = 58;

                   	if ( s >= 0 ) return s;
                   	break;
               	case 6 : 
                   	int LA24_74 = input.LA(1);

                   	s = -1;
                   	if ( ((LA24_74 >= '\u0000' && LA24_74 <= '\t') || (LA24_74 >= '\u000B' && LA24_74 <= '\"') || (LA24_74 >= '$' && LA24_74 <= '\uFFFF')) ) { s = 79; }

                   	else if ( (LA24_74 == '#') ) { s = 80; }

                   	else s = 58;

                   	if ( s >= 0 ) return s;
                   	break;
        }
        NoViableAltException nvae24 =
            new NoViableAltException(dfa.Description, 24, _s, input);
        dfa.Error(nvae24);
        throw nvae24;
    }
 
    
}
}