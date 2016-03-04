// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 T1.g 2016-03-03 11:35:48

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
public partial class T1Lexer : Lexer {
    public const int DOLLAR = 25;
    public const int LEFTANGLE = 23;
    public const int Y_ = 68;
    public const int AST1 = 16;
    public const int D_ = 62;
    public const int FIX = 10;
    public const int GENR = 11;
    public const int STAR = 49;
    public const int LETTER = 59;
    public const int Exponent = 61;
    public const int EXCL = 38;
    public const int H_ = 69;
    public const int U_ = 82;
    public const int AND = 35;
    public const int Q_ = 79;
    public const int EOF = -1;
    public const int L_ = 66;
    public const int UPD = 14;
    public const int E_ = 60;
    public const int AT = 39;
    public const int HAT = 40;
    public const int X_ = 85;
    public const int TIME = 15;
    public const int LEFTPAREN = 27;
    public const int VERTICALBAR = 50;
    public const int EOL = 20;
    public const int LEFTCURLY = 45;
    public const int HdgExpression = 32;
    public const int COMMA = 42;
    public const int A_ = 67;
    public const int EQUAL = 52;
    public const int TILDE = 37;
    public const int RIGHTCURLY = 46;
    public const int PLUS = 17;
    public const int T_ = 81;
    public const int DIGIT = 58;
    public const int I_ = 63;
    public const int DOT = 43;
    public const int ASTCOMMAND2 = 6;
    public const int COMMENT = 22;
    public const int ASTCOMMAND1 = 5;
    public const int M_ = 76;
    public const int ASTCOMMAND3 = 7;
    public const int P_ = 65;
    public const int Double = 29;
    public const int F_ = 73;
    public const int ASTCOMPARECOMMAND = 8;
    public const int PERCENT = 44;
    public const int ASTCOMMAND = 4;
    public const int B_ = 71;
    public const int NEWLINE2 = 56;
    public const int NEWLINE3 = 57;
    public const int DisplayExpression = 31;
    public const int HASH = 18;
    public const int RIGHTBRACKET = 48;
    public const int WHITESPACE = 34;
    public const int J_ = 74;
    public const int W_ = 84;
    public const int SEMICOLON = 26;
    public const int MINUS = 33;
    public const int ANYTHING = 55;
    public const int LIST = 12;
    public const int S_ = 64;
    public const int SKIP = 13;
    public const int N_ = 77;
    public const int COLON = 41;
    public const int ASTSERIES = 9;
    public const int QUESTION = 54;
    public const int G_ = 70;
    public const int Ident = 36;
    public const int Z_ = 86;
    public const int RIGHTPAREN = 28;
    public const int C_ = 72;
    public const int V_ = 83;
    public const int StringInQuotes = 30;
    public const int K_ = 75;
    public const int RIGHTANGLE = 24;
    public const int DIV = 51;
    public const int Integer = 19;
    public const int O_ = 78;
    public const int COMMENT_MULTILINE = 21;
    public const int R_ = 80;
    public const int LEFTBRACKET = 47;
    public const int BACKSLASH = 53;


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

                                  

    // delegates
    // delegators

    public T1Lexer() 
    {
		InitializeCyclicDFAs();
    }
    public T1Lexer(ICharStream input)
		: this(input, null) {
    }
    public T1Lexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "T1.g";} 
    }

    // $ANTLR start "FIX"
    public void mFIX() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FIX;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:45:5: ( 'FIX' )
            // T1.g:45:7: 'FIX'
            {
            	Match("FIX"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "FIX"

    // $ANTLR start "GENR"
    public void mGENR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = GENR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:46:6: ( 'GENR' )
            // T1.g:46:8: 'GENR'
            {
            	Match("GENR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "GENR"

    // $ANTLR start "LIST"
    public void mLIST() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LIST;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:47:6: ( 'LIST' )
            // T1.g:47:8: 'LIST'
            {
            	Match("LIST"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LIST"

    // $ANTLR start "SKIP"
    public void mSKIP() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SKIP;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:48:6: ( 'SKIP' )
            // T1.g:48:8: 'SKIP'
            {
            	Match("SKIP"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SKIP"

    // $ANTLR start "UPD"
    public void mUPD() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = UPD;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:49:5: ( 'UPD' )
            // T1.g:49:7: 'UPD'
            {
            	Match("UPD"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "UPD"

    // $ANTLR start "TIME"
    public void mTIME() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TIME;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:50:6: ( 'TIME' )
            // T1.g:50:8: 'TIME'
            {
            	Match("TIME"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "TIME"

    // $ANTLR start "NEWLINE2"
    public void mNEWLINE2() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:281:27: ( '\\n' )
            // T1.g:281:29: '\\n'
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
            // T1.g:282:27: ( '\\r\\n' )
            // T1.g:282:29: '\\r\\n'
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
            // T1.g:283:27: ( '0' .. '9' )
            // T1.g:283:29: '0' .. '9'
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
            // T1.g:284:27: ( 'a' .. 'z' | 'A' .. 'Z' )
            // T1.g:
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
            // T1.g:285:27: ( E_ ( '+' | '-' )? ( DIGIT )+ )
            // T1.g:285:29: E_ ( '+' | '-' )? ( DIGIT )+
            {
            	mE_(); 
            	// T1.g:285:32: ( '+' | '-' )?
            	int alt1 = 2;
            	int LA1_0 = input.LA(1);

            	if ( (LA1_0 == '+' || LA1_0 == '-') )
            	{
            	    alt1 = 1;
            	}
            	switch (alt1) 
            	{
            	    case 1 :
            	        // T1.g:
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

            	// T1.g:285:47: ( DIGIT )+
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
            			    // T1.g:285:47: DIGIT
            			    {
            			    	mDIGIT(); 

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


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "Exponent"

    // $ANTLR start "EOL"
    public void mEOL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EOL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:287:27: ( NEWLINE2 | NEWLINE3 )
            int alt3 = 2;
            int LA3_0 = input.LA(1);

            if ( (LA3_0 == '\n') )
            {
                alt3 = 1;
            }
            else if ( (LA3_0 == '\r') )
            {
                alt3 = 2;
            }
            else 
            {
                NoViableAltException nvae_d3s0 =
                    new NoViableAltException("", 3, 0, input);

                throw nvae_d3s0;
            }
            switch (alt3) 
            {
                case 1 :
                    // T1.g:287:29: NEWLINE2
                    {
                    	mNEWLINE2(); 

                    }
                    break;
                case 2 :
                    // T1.g:287:40: NEWLINE3
                    {
                    	mNEWLINE3(); 

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
    // $ANTLR end "EOL"

    // $ANTLR start "WHITESPACE"
    public void mWHITESPACE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WHITESPACE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:289:27: ( ( '\\t' | ' ' | '\\u000C' )+ )
            // T1.g:289:29: ( '\\t' | ' ' | '\\u000C' )+
            {
            	// T1.g:289:29: ( '\\t' | ' ' | '\\u000C' )+
            	int cnt4 = 0;
            	do 
            	{
            	    int alt4 = 2;
            	    int LA4_0 = input.LA(1);

            	    if ( (LA4_0 == '\t' || LA4_0 == '\f' || LA4_0 == ' ') )
            	    {
            	        alt4 = 1;
            	    }


            	    switch (alt4) 
            		{
            			case 1 :
            			    // T1.g:
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
            			    if ( cnt4 >= 1 ) goto loop4;
            		            EarlyExitException eee4 =
            		                new EarlyExitException(4, input);
            		            throw eee4;
            	    }
            	    cnt4++;
            	} while (true);

            	loop4:
            		;	// Stops C# compiler whining that label 'loop4' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WHITESPACE"

    // $ANTLR start "COMMENT"
    public void mCOMMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:291:27: ( ( '//' ) (~ ( NEWLINE2 | NEWLINE3 ) )* | ( '()' ) (~ ( NEWLINE2 | NEWLINE3 ) )* )
            int alt7 = 2;
            int LA7_0 = input.LA(1);

            if ( (LA7_0 == '/') )
            {
                alt7 = 1;
            }
            else if ( (LA7_0 == '(') )
            {
                alt7 = 2;
            }
            else 
            {
                NoViableAltException nvae_d7s0 =
                    new NoViableAltException("", 7, 0, input);

                throw nvae_d7s0;
            }
            switch (alt7) 
            {
                case 1 :
                    // T1.g:291:29: ( '//' ) (~ ( NEWLINE2 | NEWLINE3 ) )*
                    {
                    	// T1.g:291:29: ( '//' )
                    	// T1.g:291:30: '//'
                    	{
                    		Match("//"); 


                    	}

                    	// T1.g:291:36: (~ ( NEWLINE2 | NEWLINE3 ) )*
                    	do 
                    	{
                    	    int alt5 = 2;
                    	    int LA5_0 = input.LA(1);

                    	    if ( ((LA5_0 >= '\u0000' && LA5_0 <= '\t') || (LA5_0 >= '\u000B' && LA5_0 <= '\uFFFF')) )
                    	    {
                    	        alt5 = 1;
                    	    }


                    	    switch (alt5) 
                    		{
                    			case 1 :
                    			    // T1.g:291:37: ~ ( NEWLINE2 | NEWLINE3 )
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
                    			    goto loop5;
                    	    }
                    	} while (true);

                    	loop5:
                    		;	// Stops C# compiler whining that label 'loop5' has no statements


                    }
                    break;
                case 2 :
                    // T1.g:292:11: ( '()' ) (~ ( NEWLINE2 | NEWLINE3 ) )*
                    {
                    	// T1.g:292:11: ( '()' )
                    	// T1.g:292:12: '()'
                    	{
                    		Match("()"); 


                    	}

                    	// T1.g:292:18: (~ ( NEWLINE2 | NEWLINE3 ) )*
                    	do 
                    	{
                    	    int alt6 = 2;
                    	    int LA6_0 = input.LA(1);

                    	    if ( ((LA6_0 >= '\u0000' && LA6_0 <= '\t') || (LA6_0 >= '\u000B' && LA6_0 <= '\uFFFF')) )
                    	    {
                    	        alt6 = 1;
                    	    }


                    	    switch (alt6) 
                    		{
                    			case 1 :
                    			    // T1.g:292:19: ~ ( NEWLINE2 | NEWLINE3 )
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
                    			    goto loop6;
                    	    }
                    	} while (true);

                    	loop6:
                    		;	// Stops C# compiler whining that label 'loop6' has no statements


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
    // $ANTLR end "COMMENT"

    // $ANTLR start "COMMENT_MULTILINE"
    public void mCOMMENT_MULTILINE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMENT_MULTILINE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:295:27: ( '/*' ( options {greedy=false; } : COMMENT_MULTILINE | . )* '*/' )
            // T1.g:295:29: '/*' ( options {greedy=false; } : COMMENT_MULTILINE | . )* '*/'
            {
            	Match("/*"); 

            	// T1.g:295:34: ( options {greedy=false; } : COMMENT_MULTILINE | . )*
            	do 
            	{
            	    int alt8 = 3;
            	    int LA8_0 = input.LA(1);

            	    if ( (LA8_0 == '*') )
            	    {
            	        int LA8_1 = input.LA(2);

            	        if ( (LA8_1 == '/') )
            	        {
            	            alt8 = 3;
            	        }
            	        else if ( ((LA8_1 >= '\u0000' && LA8_1 <= '.') || (LA8_1 >= '0' && LA8_1 <= '\uFFFF')) )
            	        {
            	            alt8 = 2;
            	        }


            	    }
            	    else if ( (LA8_0 == '/') )
            	    {
            	        int LA8_2 = input.LA(2);

            	        if ( (LA8_2 == '*') )
            	        {
            	            alt8 = 1;
            	        }
            	        else if ( ((LA8_2 >= '\u0000' && LA8_2 <= ')') || (LA8_2 >= '+' && LA8_2 <= '\uFFFF')) )
            	        {
            	            alt8 = 2;
            	        }


            	    }
            	    else if ( ((LA8_0 >= '\u0000' && LA8_0 <= ')') || (LA8_0 >= '+' && LA8_0 <= '.') || (LA8_0 >= '0' && LA8_0 <= '\uFFFF')) )
            	    {
            	        alt8 = 2;
            	    }


            	    switch (alt8) 
            		{
            			case 1 :
            			    // T1.g:295:60: COMMENT_MULTILINE
            			    {
            			    	mCOMMENT_MULTILINE(); 

            			    }
            			    break;
            			case 2 :
            			    // T1.g:295:80: .
            			    {
            			    	MatchAny(); 

            			    }
            			    break;

            			default:
            			    goto loop8;
            	    }
            	} while (true);

            	loop8:
            		;	// Stops C# compiler whining that label 'loop8' has no statements

            	Match("*/"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMMENT_MULTILINE"

    // $ANTLR start "Ident"
    public void mIdent() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Ident;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:298:27: ( ( LETTER | '_' ) ( DIGIT | LETTER | '_' )* )
            // T1.g:298:29: ( LETTER | '_' ) ( DIGIT | LETTER | '_' )*
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

            	// T1.g:298:42: ( DIGIT | LETTER | '_' )*
            	do 
            	{
            	    int alt9 = 2;
            	    int LA9_0 = input.LA(1);

            	    if ( ((LA9_0 >= '0' && LA9_0 <= '9') || (LA9_0 >= 'A' && LA9_0 <= 'Z') || LA9_0 == '_' || (LA9_0 >= 'a' && LA9_0 <= 'z')) )
            	    {
            	        alt9 = 1;
            	    }


            	    switch (alt9) 
            		{
            			case 1 :
            			    // T1.g:
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
            			    goto loop9;
            	    }
            	} while (true);

            	loop9:
            		;	// Stops C# compiler whining that label 'loop9' has no statements

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

    // $ANTLR start "StringInQuotes"
    public void mStringInQuotes() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = StringInQuotes;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:301:27: ( ( '\\'' (~ '\\'' )* '\\'' ) )
            // T1.g:301:29: ( '\\'' (~ '\\'' )* '\\'' )
            {
            	// T1.g:301:29: ( '\\'' (~ '\\'' )* '\\'' )
            	// T1.g:301:30: '\\'' (~ '\\'' )* '\\''
            	{
            		Match('\''); 
            		// T1.g:301:35: (~ '\\'' )*
            		do 
            		{
            		    int alt10 = 2;
            		    int LA10_0 = input.LA(1);

            		    if ( ((LA10_0 >= '\u0000' && LA10_0 <= '&') || (LA10_0 >= '(' && LA10_0 <= '\uFFFF')) )
            		    {
            		        alt10 = 1;
            		    }


            		    switch (alt10) 
            			{
            				case 1 :
            				    // T1.g:301:36: ~ '\\''
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
            				    goto loop10;
            		    }
            		} while (true);

            		loop10:
            			;	// Stops C# compiler whining that label 'loop10' has no statements

            		Match('\''); 

            	}


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "StringInQuotes"

    // $ANTLR start "Double"
    public void mDouble() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Double;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:303:27: ( ( DIGIT )+ DOT ( DIGIT )* ( Exponent )? | ( DIGIT )+ Exponent )
            int alt15 = 2;
            alt15 = dfa15.Predict(input);
            switch (alt15) 
            {
                case 1 :
                    // T1.g:303:29: ( DIGIT )+ DOT ( DIGIT )* ( Exponent )?
                    {
                    	// T1.g:303:29: ( DIGIT )+
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
                    			    // T1.g:303:29: DIGIT
                    			    {
                    			    	mDIGIT(); 

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

                    	mDOT(); 
                    	// T1.g:303:40: ( DIGIT )*
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
                    			    // T1.g:303:40: DIGIT
                    			    {
                    			    	mDIGIT(); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop12;
                    	    }
                    	} while (true);

                    	loop12:
                    		;	// Stops C# compiler whining that label 'loop12' has no statements

                    	// T1.g:303:47: ( Exponent )?
                    	int alt13 = 2;
                    	int LA13_0 = input.LA(1);

                    	if ( (LA13_0 == 'E' || LA13_0 == 'e') )
                    	{
                    	    alt13 = 1;
                    	}
                    	switch (alt13) 
                    	{
                    	    case 1 :
                    	        // T1.g:303:47: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // T1.g:304:29: ( DIGIT )+ Exponent
                    {
                    	// T1.g:304:29: ( DIGIT )+
                    	int cnt14 = 0;
                    	do 
                    	{
                    	    int alt14 = 2;
                    	    int LA14_0 = input.LA(1);

                    	    if ( ((LA14_0 >= '0' && LA14_0 <= '9')) )
                    	    {
                    	        alt14 = 1;
                    	    }


                    	    switch (alt14) 
                    		{
                    			case 1 :
                    			    // T1.g:304:29: DIGIT
                    			    {
                    			    	mDIGIT(); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt14 >= 1 ) goto loop14;
                    		            EarlyExitException eee14 =
                    		                new EarlyExitException(14, input);
                    		            throw eee14;
                    	    }
                    	    cnt14++;
                    	} while (true);

                    	loop14:
                    		;	// Stops C# compiler whining that label 'loop14' has no statements

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

    // $ANTLR start "Integer"
    public void mInteger() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Integer;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:307:27: ( ( DIGIT )+ )
            // T1.g:307:29: ( DIGIT )+
            {
            	// T1.g:307:29: ( DIGIT )+
            	int cnt16 = 0;
            	do 
            	{
            	    int alt16 = 2;
            	    int LA16_0 = input.LA(1);

            	    if ( ((LA16_0 >= '0' && LA16_0 <= '9')) )
            	    {
            	        alt16 = 1;
            	    }


            	    switch (alt16) 
            		{
            			case 1 :
            			    // T1.g:307:29: DIGIT
            			    {
            			    	mDIGIT(); 

            			    }
            			    break;

            			default:
            			    if ( cnt16 >= 1 ) goto loop16;
            		            EarlyExitException eee16 =
            		                new EarlyExitException(16, input);
            		            throw eee16;
            	    }
            	    cnt16++;
            	} while (true);

            	loop16:
            		;	// Stops C# compiler whining that label 'loop16' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "Integer"

    // $ANTLR start "DisplayExpression"
    public void mDisplayExpression() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DisplayExpression;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:310:1: ( D_ I_ S_ P_ L_ A_ Y_ ( ' ' ) ( options {greedy=false; } : . )* EOL )
            // T1.g:310:3: D_ I_ S_ P_ L_ A_ Y_ ( ' ' ) ( options {greedy=false; } : . )* EOL
            {
            	mD_(); 
            	mI_(); 
            	mS_(); 
            	mP_(); 
            	mL_(); 
            	mA_(); 
            	mY_(); 
            	// T1.g:310:24: ( ' ' )
            	// T1.g:310:25: ' '
            	{
            		Match(' '); 

            	}

            	// T1.g:310:30: ( options {greedy=false; } : . )*
            	do 
            	{
            	    int alt17 = 2;
            	    int LA17_0 = input.LA(1);

            	    if ( (LA17_0 == '\n') )
            	    {
            	        alt17 = 2;
            	    }
            	    else if ( (LA17_0 == '\r') )
            	    {
            	        int LA17_2 = input.LA(2);

            	        if ( (LA17_2 == '\n') )
            	        {
            	            alt17 = 2;
            	        }
            	        else if ( ((LA17_2 >= '\u0000' && LA17_2 <= '\t') || (LA17_2 >= '\u000B' && LA17_2 <= '\uFFFF')) )
            	        {
            	            alt17 = 1;
            	        }


            	    }
            	    else if ( ((LA17_0 >= '\u0000' && LA17_0 <= '\t') || (LA17_0 >= '\u000B' && LA17_0 <= '\f') || (LA17_0 >= '\u000E' && LA17_0 <= '\uFFFF')) )
            	    {
            	        alt17 = 1;
            	    }


            	    switch (alt17) 
            		{
            			case 1 :
            			    // T1.g:312:14: .
            			    {
            			    	MatchAny(); 

            			    }
            			    break;

            			default:
            			    goto loop17;
            	    }
            	} while (true);

            	loop17:
            		;	// Stops C# compiler whining that label 'loop17' has no statements

            	mEOL(); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DisplayExpression"

    // $ANTLR start "HdgExpression"
    public void mHdgExpression() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = HdgExpression;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:315:1: ( H_ D_ G_ ( ' ' ) ( options {greedy=false; } : . )* EOL )
            // T1.g:315:3: H_ D_ G_ ( ' ' ) ( options {greedy=false; } : . )* EOL
            {
            	mH_(); 
            	mD_(); 
            	mG_(); 
            	// T1.g:315:12: ( ' ' )
            	// T1.g:315:13: ' '
            	{
            		Match(' '); 

            	}

            	// T1.g:315:18: ( options {greedy=false; } : . )*
            	do 
            	{
            	    int alt18 = 2;
            	    int LA18_0 = input.LA(1);

            	    if ( (LA18_0 == '\n') )
            	    {
            	        alt18 = 2;
            	    }
            	    else if ( (LA18_0 == '\r') )
            	    {
            	        int LA18_2 = input.LA(2);

            	        if ( (LA18_2 == '\n') )
            	        {
            	            alt18 = 2;
            	        }
            	        else if ( ((LA18_2 >= '\u0000' && LA18_2 <= '\t') || (LA18_2 >= '\u000B' && LA18_2 <= '\uFFFF')) )
            	        {
            	            alt18 = 1;
            	        }


            	    }
            	    else if ( ((LA18_0 >= '\u0000' && LA18_0 <= '\t') || (LA18_0 >= '\u000B' && LA18_0 <= '\f') || (LA18_0 >= '\u000E' && LA18_0 <= '\uFFFF')) )
            	    {
            	        alt18 = 1;
            	    }


            	    switch (alt18) 
            		{
            			case 1 :
            			    // T1.g:317:14: .
            			    {
            			    	MatchAny(); 

            			    }
            			    break;

            			default:
            			    goto loop18;
            	    }
            	} while (true);

            	loop18:
            		;	// Stops C# compiler whining that label 'loop18' has no statements

            	mEOL(); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "HdgExpression"

    // $ANTLR start "TILDE"
    public void mTILDE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TILDE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:319:27: ( '~' )
            // T1.g:319:29: '~'
            {
            	Match('~'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "TILDE"

    // $ANTLR start "AND"
    public void mAND() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AND;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:320:27: ( '&' )
            // T1.g:320:29: '&'
            {
            	Match('&'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "AND"

    // $ANTLR start "EXCL"
    public void mEXCL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EXCL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:321:27: ( '!' )
            // T1.g:321:29: '!'
            {
            	Match('!'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EXCL"

    // $ANTLR start "AT"
    public void mAT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:322:27: ( '@' )
            // T1.g:322:29: '@'
            {
            	Match('@'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "AT"

    // $ANTLR start "HAT"
    public void mHAT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = HAT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:323:27: ( '^' )
            // T1.g:323:29: '^'
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

    // $ANTLR start "SEMICOLON"
    public void mSEMICOLON() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SEMICOLON;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:324:27: ( ';' )
            // T1.g:324:29: ';'
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
    // $ANTLR end "SEMICOLON"

    // $ANTLR start "COLON"
    public void mCOLON() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COLON;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:325:27: ( ':' )
            // T1.g:325:29: ':'
            {
            	Match(':'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COLON"

    // $ANTLR start "COMMA"
    public void mCOMMA() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMA;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:326:27: ( ',' )
            // T1.g:326:29: ','
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

    // $ANTLR start "DOT"
    public void mDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:327:27: ( '.' )
            // T1.g:327:29: '.'
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

    // $ANTLR start "HASH"
    public void mHASH() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = HASH;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:328:27: ( '#' )
            // T1.g:328:29: '#'
            {
            	Match('#'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "HASH"

    // $ANTLR start "PERCENT"
    public void mPERCENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = PERCENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:329:27: ( '%' )
            // T1.g:329:29: '%'
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
    // $ANTLR end "PERCENT"

    // $ANTLR start "DOLLAR"
    public void mDOLLAR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOLLAR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:330:27: ( '$' )
            // T1.g:330:29: '$'
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

    // $ANTLR start "LEFTCURLY"
    public void mLEFTCURLY() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LEFTCURLY;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:331:27: ( '{' )
            // T1.g:331:29: '{'
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
    // $ANTLR end "LEFTCURLY"

    // $ANTLR start "RIGHTCURLY"
    public void mRIGHTCURLY() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RIGHTCURLY;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:332:27: ( '}' )
            // T1.g:332:29: '}'
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
    // $ANTLR end "RIGHTCURLY"

    // $ANTLR start "LEFTPAREN"
    public void mLEFTPAREN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LEFTPAREN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:333:27: ( '(' )
            // T1.g:333:29: '('
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
    // $ANTLR end "LEFTPAREN"

    // $ANTLR start "RIGHTPAREN"
    public void mRIGHTPAREN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RIGHTPAREN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:334:27: ( ')' )
            // T1.g:334:29: ')'
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
    // $ANTLR end "RIGHTPAREN"

    // $ANTLR start "LEFTBRACKET"
    public void mLEFTBRACKET() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LEFTBRACKET;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:335:27: ( '[' )
            // T1.g:335:29: '['
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
    // $ANTLR end "LEFTBRACKET"

    // $ANTLR start "RIGHTBRACKET"
    public void mRIGHTBRACKET() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RIGHTBRACKET;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:336:27: ( ']' )
            // T1.g:336:29: ']'
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
    // $ANTLR end "RIGHTBRACKET"

    // $ANTLR start "LEFTANGLE"
    public void mLEFTANGLE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LEFTANGLE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:337:27: ( '<' )
            // T1.g:337:29: '<'
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
    // $ANTLR end "LEFTANGLE"

    // $ANTLR start "RIGHTANGLE"
    public void mRIGHTANGLE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RIGHTANGLE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:338:27: ( '>' )
            // T1.g:338:29: '>'
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
    // $ANTLR end "RIGHTANGLE"

    // $ANTLR start "STAR"
    public void mSTAR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STAR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:339:27: ( '*' )
            // T1.g:339:29: '*'
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
    // $ANTLR end "STAR"

    // $ANTLR start "VERTICALBAR"
    public void mVERTICALBAR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VERTICALBAR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:340:27: ( '|' )
            // T1.g:340:29: '|'
            {
            	Match('|'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VERTICALBAR"

    // $ANTLR start "PLUS"
    public void mPLUS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = PLUS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:341:27: ( '+' )
            // T1.g:341:29: '+'
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
            // T1.g:342:27: ( '-' )
            // T1.g:342:29: '-'
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

    // $ANTLR start "DIV"
    public void mDIV() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DIV;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:343:27: ( '/' )
            // T1.g:343:29: '/'
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

    // $ANTLR start "EQUAL"
    public void mEQUAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EQUAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:344:27: ( '=' )
            // T1.g:344:29: '='
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

    // $ANTLR start "BACKSLASH"
    public void mBACKSLASH() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = BACKSLASH;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:345:27: ( '\\\\' )
            // T1.g:345:29: '\\\\'
            {
            	Match('\\'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "BACKSLASH"

    // $ANTLR start "QUESTION"
    public void mQUESTION() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = QUESTION;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:346:27: ( '?' )
            // T1.g:346:29: '?'
            {
            	Match('?'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "QUESTION"

    // $ANTLR start "ANYTHING"
    public void mANYTHING() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ANYTHING;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T1.g:348:27: ( . )
            // T1.g:348:29: .
            {
            	MatchAny(); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ANYTHING"

    // $ANTLR start "A_"
    public void mA_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:350:12: ( ( 'a' | 'A' ) )
            // T1.g:350:13: ( 'a' | 'A' )
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
    // $ANTLR end "A_"

    // $ANTLR start "B_"
    public void mB_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:351:12: ( ( 'b' | 'B' ) )
            // T1.g:351:13: ( 'b' | 'B' )
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
    // $ANTLR end "B_"

    // $ANTLR start "C_"
    public void mC_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:352:12: ( ( 'c' | 'C' ) )
            // T1.g:352:13: ( 'c' | 'C' )
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
    // $ANTLR end "C_"

    // $ANTLR start "D_"
    public void mD_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:353:12: ( ( 'd' | 'D' ) )
            // T1.g:353:13: ( 'd' | 'D' )
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
    // $ANTLR end "D_"

    // $ANTLR start "E_"
    public void mE_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:354:12: ( ( 'e' | 'E' ) )
            // T1.g:354:13: ( 'e' | 'E' )
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
    // $ANTLR end "E_"

    // $ANTLR start "F_"
    public void mF_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:355:12: ( ( 'f' | 'F' ) )
            // T1.g:355:13: ( 'f' | 'F' )
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
    // $ANTLR end "F_"

    // $ANTLR start "G_"
    public void mG_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:356:12: ( ( 'g' | 'G' ) )
            // T1.g:356:13: ( 'g' | 'G' )
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
    // $ANTLR end "G_"

    // $ANTLR start "H_"
    public void mH_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:357:12: ( ( 'h' | 'H' ) )
            // T1.g:357:13: ( 'h' | 'H' )
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
    // $ANTLR end "H_"

    // $ANTLR start "I_"
    public void mI_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:358:12: ( ( 'i' | 'I' ) )
            // T1.g:358:13: ( 'i' | 'I' )
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
    // $ANTLR end "I_"

    // $ANTLR start "J_"
    public void mJ_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:359:12: ( ( 'j' | 'J' ) )
            // T1.g:359:13: ( 'j' | 'J' )
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
    // $ANTLR end "J_"

    // $ANTLR start "K_"
    public void mK_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:360:12: ( ( 'k' | 'K' ) )
            // T1.g:360:13: ( 'k' | 'K' )
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
    // $ANTLR end "K_"

    // $ANTLR start "L_"
    public void mL_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:361:12: ( ( 'l' | 'L' ) )
            // T1.g:361:13: ( 'l' | 'L' )
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
    // $ANTLR end "L_"

    // $ANTLR start "M_"
    public void mM_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:362:12: ( ( 'm' | 'M' ) )
            // T1.g:362:13: ( 'm' | 'M' )
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
    // $ANTLR end "M_"

    // $ANTLR start "N_"
    public void mN_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:363:12: ( ( 'n' | 'N' ) )
            // T1.g:363:13: ( 'n' | 'N' )
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
    // $ANTLR end "N_"

    // $ANTLR start "O_"
    public void mO_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:364:12: ( ( 'o' | 'O' ) )
            // T1.g:364:13: ( 'o' | 'O' )
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
    // $ANTLR end "O_"

    // $ANTLR start "P_"
    public void mP_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:365:12: ( ( 'p' | 'P' ) )
            // T1.g:365:13: ( 'p' | 'P' )
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
    // $ANTLR end "P_"

    // $ANTLR start "Q_"
    public void mQ_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:366:12: ( ( 'q' | 'Q' ) )
            // T1.g:366:13: ( 'q' | 'Q' )
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
    // $ANTLR end "Q_"

    // $ANTLR start "R_"
    public void mR_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:367:12: ( ( 'r' | 'R' ) )
            // T1.g:367:13: ( 'r' | 'R' )
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
    // $ANTLR end "R_"

    // $ANTLR start "S_"
    public void mS_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:368:12: ( ( 's' | 'S' ) )
            // T1.g:368:13: ( 's' | 'S' )
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
    // $ANTLR end "S_"

    // $ANTLR start "T_"
    public void mT_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:369:12: ( ( 't' | 'T' ) )
            // T1.g:369:13: ( 't' | 'T' )
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
    // $ANTLR end "T_"

    // $ANTLR start "U_"
    public void mU_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:370:12: ( ( 'u' | 'U' ) )
            // T1.g:370:13: ( 'u' | 'U' )
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
    // $ANTLR end "U_"

    // $ANTLR start "V_"
    public void mV_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:371:12: ( ( 'v' | 'V' ) )
            // T1.g:371:13: ( 'v' | 'V' )
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
    // $ANTLR end "V_"

    // $ANTLR start "W_"
    public void mW_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:372:12: ( ( 'w' | 'W' ) )
            // T1.g:372:13: ( 'w' | 'W' )
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
    // $ANTLR end "W_"

    // $ANTLR start "X_"
    public void mX_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:373:12: ( ( 'x' | 'X' ) )
            // T1.g:373:13: ( 'x' | 'X' )
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
    // $ANTLR end "X_"

    // $ANTLR start "Y_"
    public void mY_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:374:12: ( ( 'y' | 'Y' ) )
            // T1.g:374:13: ( 'y' | 'Y' )
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
    // $ANTLR end "Y_"

    // $ANTLR start "Z_"
    public void mZ_() // throws RecognitionException [2]
    {
    		try
    		{
            // T1.g:375:12: ( ( 'z' | 'Z' ) )
            // T1.g:375:13: ( 'z' | 'Z' )
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
    // $ANTLR end "Z_"

    override public void mTokens() // throws RecognitionException 
    {
        // T1.g:1:8: ( FIX | GENR | LIST | SKIP | UPD | TIME | EOL | WHITESPACE | COMMENT | COMMENT_MULTILINE | Ident | StringInQuotes | Double | Integer | DisplayExpression | HdgExpression | TILDE | AND | EXCL | AT | HAT | SEMICOLON | COLON | COMMA | DOT | HASH | PERCENT | DOLLAR | LEFTCURLY | RIGHTCURLY | LEFTPAREN | RIGHTPAREN | LEFTBRACKET | RIGHTBRACKET | LEFTANGLE | RIGHTANGLE | STAR | VERTICALBAR | PLUS | MINUS | DIV | EQUAL | BACKSLASH | QUESTION | ANYTHING )
        int alt19 = 45;
        alt19 = dfa19.Predict(input);
        switch (alt19) 
        {
            case 1 :
                // T1.g:1:10: FIX
                {
                	mFIX(); 

                }
                break;
            case 2 :
                // T1.g:1:14: GENR
                {
                	mGENR(); 

                }
                break;
            case 3 :
                // T1.g:1:19: LIST
                {
                	mLIST(); 

                }
                break;
            case 4 :
                // T1.g:1:24: SKIP
                {
                	mSKIP(); 

                }
                break;
            case 5 :
                // T1.g:1:29: UPD
                {
                	mUPD(); 

                }
                break;
            case 6 :
                // T1.g:1:33: TIME
                {
                	mTIME(); 

                }
                break;
            case 7 :
                // T1.g:1:38: EOL
                {
                	mEOL(); 

                }
                break;
            case 8 :
                // T1.g:1:42: WHITESPACE
                {
                	mWHITESPACE(); 

                }
                break;
            case 9 :
                // T1.g:1:53: COMMENT
                {
                	mCOMMENT(); 

                }
                break;
            case 10 :
                // T1.g:1:61: COMMENT_MULTILINE
                {
                	mCOMMENT_MULTILINE(); 

                }
                break;
            case 11 :
                // T1.g:1:79: Ident
                {
                	mIdent(); 

                }
                break;
            case 12 :
                // T1.g:1:85: StringInQuotes
                {
                	mStringInQuotes(); 

                }
                break;
            case 13 :
                // T1.g:1:100: Double
                {
                	mDouble(); 

                }
                break;
            case 14 :
                // T1.g:1:107: Integer
                {
                	mInteger(); 

                }
                break;
            case 15 :
                // T1.g:1:115: DisplayExpression
                {
                	mDisplayExpression(); 

                }
                break;
            case 16 :
                // T1.g:1:133: HdgExpression
                {
                	mHdgExpression(); 

                }
                break;
            case 17 :
                // T1.g:1:147: TILDE
                {
                	mTILDE(); 

                }
                break;
            case 18 :
                // T1.g:1:153: AND
                {
                	mAND(); 

                }
                break;
            case 19 :
                // T1.g:1:157: EXCL
                {
                	mEXCL(); 

                }
                break;
            case 20 :
                // T1.g:1:162: AT
                {
                	mAT(); 

                }
                break;
            case 21 :
                // T1.g:1:165: HAT
                {
                	mHAT(); 

                }
                break;
            case 22 :
                // T1.g:1:169: SEMICOLON
                {
                	mSEMICOLON(); 

                }
                break;
            case 23 :
                // T1.g:1:179: COLON
                {
                	mCOLON(); 

                }
                break;
            case 24 :
                // T1.g:1:185: COMMA
                {
                	mCOMMA(); 

                }
                break;
            case 25 :
                // T1.g:1:191: DOT
                {
                	mDOT(); 

                }
                break;
            case 26 :
                // T1.g:1:195: HASH
                {
                	mHASH(); 

                }
                break;
            case 27 :
                // T1.g:1:200: PERCENT
                {
                	mPERCENT(); 

                }
                break;
            case 28 :
                // T1.g:1:208: DOLLAR
                {
                	mDOLLAR(); 

                }
                break;
            case 29 :
                // T1.g:1:215: LEFTCURLY
                {
                	mLEFTCURLY(); 

                }
                break;
            case 30 :
                // T1.g:1:225: RIGHTCURLY
                {
                	mRIGHTCURLY(); 

                }
                break;
            case 31 :
                // T1.g:1:236: LEFTPAREN
                {
                	mLEFTPAREN(); 

                }
                break;
            case 32 :
                // T1.g:1:246: RIGHTPAREN
                {
                	mRIGHTPAREN(); 

                }
                break;
            case 33 :
                // T1.g:1:257: LEFTBRACKET
                {
                	mLEFTBRACKET(); 

                }
                break;
            case 34 :
                // T1.g:1:269: RIGHTBRACKET
                {
                	mRIGHTBRACKET(); 

                }
                break;
            case 35 :
                // T1.g:1:282: LEFTANGLE
                {
                	mLEFTANGLE(); 

                }
                break;
            case 36 :
                // T1.g:1:292: RIGHTANGLE
                {
                	mRIGHTANGLE(); 

                }
                break;
            case 37 :
                // T1.g:1:303: STAR
                {
                	mSTAR(); 

                }
                break;
            case 38 :
                // T1.g:1:308: VERTICALBAR
                {
                	mVERTICALBAR(); 

                }
                break;
            case 39 :
                // T1.g:1:320: PLUS
                {
                	mPLUS(); 

                }
                break;
            case 40 :
                // T1.g:1:325: MINUS
                {
                	mMINUS(); 

                }
                break;
            case 41 :
                // T1.g:1:331: DIV
                {
                	mDIV(); 

                }
                break;
            case 42 :
                // T1.g:1:335: EQUAL
                {
                	mEQUAL(); 

                }
                break;
            case 43 :
                // T1.g:1:341: BACKSLASH
                {
                	mBACKSLASH(); 

                }
                break;
            case 44 :
                // T1.g:1:351: QUESTION
                {
                	mQUESTION(); 

                }
                break;
            case 45 :
                // T1.g:1:360: ANYTHING
                {
                	mANYTHING(); 

                }
                break;

        }

    }


    protected DFA15 dfa15;
    protected DFA19 dfa19;
	private void InitializeCyclicDFAs()
	{
	    this.dfa15 = new DFA15(this);
	    this.dfa19 = new DFA19(this);

	    this.dfa19.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA19_SpecialStateTransition);
	}

    const string DFA15_eotS =
        "\x04\uffff";
    const string DFA15_eofS =
        "\x04\uffff";
    const string DFA15_minS =
        "\x01\x30\x01\x2e\x02\uffff";
    const string DFA15_maxS =
        "\x01\x39\x01\x65\x02\uffff";
    const string DFA15_acceptS =
        "\x02\uffff\x01\x02\x01\x01";
    const string DFA15_specialS =
        "\x04\uffff}>";
    static readonly string[] DFA15_transitionS = {
            "\x0a\x01",
            "\x01\x03\x01\uffff\x0a\x01\x0b\uffff\x01\x02\x1f\uffff\x01"+
            "\x02",
            "",
            ""
    };

    static readonly short[] DFA15_eot = DFA.UnpackEncodedString(DFA15_eotS);
    static readonly short[] DFA15_eof = DFA.UnpackEncodedString(DFA15_eofS);
    static readonly char[] DFA15_min = DFA.UnpackEncodedStringToUnsignedChars(DFA15_minS);
    static readonly char[] DFA15_max = DFA.UnpackEncodedStringToUnsignedChars(DFA15_maxS);
    static readonly short[] DFA15_accept = DFA.UnpackEncodedString(DFA15_acceptS);
    static readonly short[] DFA15_special = DFA.UnpackEncodedString(DFA15_specialS);
    static readonly short[][] DFA15_transition = DFA.UnpackEncodedStringArray(DFA15_transitionS);

    protected class DFA15 : DFA
    {
        public DFA15(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 15;
            this.eot = DFA15_eot;
            this.eof = DFA15_eof;
            this.min = DFA15_min;
            this.max = DFA15_max;
            this.accept = DFA15_accept;
            this.special = DFA15_special;
            this.transition = DFA15_transition;

        }

        override public string Description
        {
            get { return "303:1: Double : ( ( DIGIT )+ DOT ( DIGIT )* ( Exponent )? | ( DIGIT )+ Exponent );"; }
        }

    }

    const string DFA19_eotS =
        "\x01\uffff\x06\x2d\x01\uffff\x01\x2b\x01\uffff\x01\x37\x01\x38"+
        "\x01\x2d\x01\x2b\x01\x3b\x01\x2d\x1c\uffff\x01\x2d\x01\uffff\x05"+
        "\x2d\x06\uffff\x01\x2d\x03\uffff\x01\x3b\x01\x2d\x1a\uffff\x01\x61"+
        "\x03\x2d\x01\x65\x03\x2d\x01\uffff\x01\x69\x01\x6a\x01\x6b\x01\uffff"+
        "\x01\x6c\x01\x2d\x05\uffff\x03\x2d\x01\uffff";
    const string DFA19_eofS =
        "\x71\uffff";
    const string DFA19_minS =
        "\x01\x00\x01\x49\x01\x45\x01\x49\x01\x4b\x01\x50\x01\x49\x01\uffff"+
        "\x01\x0a\x01\uffff\x01\x2a\x01\x29\x01\x49\x01\x00\x01\x2e\x01\x44"+
        "\x1c\uffff\x01\x58\x01\uffff\x01\x4e\x01\x53\x01\x49\x01\x44\x01"+
        "\x4d\x06\uffff\x01\x53\x03\uffff\x01\x2e\x01\x47\x1a\uffff\x01\x30"+
        "\x01\x52\x01\x54\x01\x50\x01\x30\x01\x45\x01\x50\x01\x20\x01\uffff"+
        "\x03\x30\x01\uffff\x01\x30\x01\x4c\x05\uffff\x01\x41\x01\x59\x01"+
        "\x20\x01\uffff";
    const string DFA19_maxS =
        "\x01\uffff\x01\x49\x01\x45\x01\x49\x01\x4b\x01\x50\x01\x49\x01"+
        "\uffff\x01\x0a\x01\uffff\x01\x2f\x01\x29\x01\x69\x01\uffff\x01\x65"+
        "\x01\x64\x1c\uffff\x01\x58\x01\uffff\x01\x4e\x01\x53\x01\x49\x01"+
        "\x44\x01\x4d\x06\uffff\x01\x73\x03\uffff\x01\x65\x01\x67\x1a\uffff"+
        "\x01\x7a\x01\x52\x01\x54\x01\x50\x01\x7a\x01\x45\x01\x70\x01\x20"+
        "\x01\uffff\x03\x7a\x01\uffff\x01\x7a\x01\x6c\x05\uffff\x01\x61\x01"+
        "\x79\x01\x20\x01\uffff";
    const string DFA19_acceptS =
        "\x07\uffff\x01\x07\x01\uffff\x01\x08\x06\uffff\x01\x0b\x01\x11"+
        "\x01\x12\x01\x13\x01\x14\x01\x15\x01\x16\x01\x17\x01\x18\x01\x19"+
        "\x01\x1a\x01\x1b\x01\x1c\x01\x1d\x01\x1e\x01\x20\x01\x21\x01\x22"+
        "\x01\x23\x01\x24\x01\x25\x01\x26\x01\x27\x01\x28\x01\x2a\x01\x2b"+
        "\x01\x2c\x01\x2d\x01\uffff\x01\x0b\x05\uffff\x01\x07\x01\x08\x01"+
        "\x09\x01\x0a\x01\x29\x01\x1f\x01\uffff\x01\x0c\x01\x0e\x01\x0d\x02"+
        "\uffff\x01\x11\x01\x12\x01\x13\x01\x14\x01\x15\x01\x16\x01\x17\x01"+
        "\x18\x01\x19\x01\x1a\x01\x1b\x01\x1c\x01\x1d\x01\x1e\x01\x20\x01"+
        "\x21\x01\x22\x01\x23\x01\x24\x01\x25\x01\x26\x01\x27\x01\x28\x01"+
        "\x2a\x01\x2b\x01\x2c\x08\uffff\x01\x01\x03\uffff\x01\x05\x02\uffff"+
        "\x01\x10\x01\x02\x01\x03\x01\x04\x01\x06\x03\uffff\x01\x0f";
    const string DFA19_specialS =
        "\x01\x00\x0c\uffff\x01\x01\x63\uffff}>";
    static readonly string[] DFA19_transitionS = {
            "\x09\x2b\x01\x09\x01\x07\x01\x2b\x01\x09\x01\x08\x12\x2b\x01"+
            "\x09\x01\x13\x01\x2b\x01\x1a\x01\x1c\x01\x1b\x01\x12\x01\x0d"+
            "\x01\x0b\x01\x1f\x01\x24\x01\x26\x01\x18\x01\x27\x01\x19\x01"+
            "\x0a\x0a\x0e\x01\x17\x01\x16\x01\x22\x01\x28\x01\x23\x01\x2a"+
            "\x01\x14\x03\x10\x01\x0c\x01\x10\x01\x01\x01\x02\x01\x0f\x03"+
            "\x10\x01\x03\x06\x10\x01\x04\x01\x06\x01\x05\x05\x10\x01\x20"+
            "\x01\x29\x01\x21\x01\x15\x01\x10\x01\x2b\x03\x10\x01\x0c\x03"+
            "\x10\x01\x0f\x12\x10\x01\x1d\x01\x25\x01\x1e\x01\x11\uff81\x2b",
            "\x01\x2c",
            "\x01\x2e",
            "\x01\x2f",
            "\x01\x30",
            "\x01\x31",
            "\x01\x32",
            "",
            "\x01\x33",
            "",
            "\x01\x36\x04\uffff\x01\x35",
            "\x01\x35",
            "\x01\x39\x1f\uffff\x01\x39",
            "\x00\x3a",
            "\x01\x3c\x01\uffff\x0a\x3d\x0b\uffff\x01\x3c\x1f\uffff\x01"+
            "\x3c",
            "\x01\x3e\x1f\uffff\x01\x3e",
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
            "\x01\x59",
            "",
            "\x01\x5a",
            "\x01\x5b",
            "\x01\x5c",
            "\x01\x5d",
            "\x01\x5e",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x5f\x1f\uffff\x01\x5f",
            "",
            "",
            "",
            "\x01\x3c\x01\uffff\x0a\x3d\x0b\uffff\x01\x3c\x1f\uffff\x01"+
            "\x3c",
            "\x01\x60\x1f\uffff\x01\x60",
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
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\x62",
            "\x01\x63",
            "\x01\x64",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\x66",
            "\x01\x67\x1f\uffff\x01\x67",
            "\x01\x68",
            "",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\x6d\x1f\uffff\x01\x6d",
            "",
            "",
            "",
            "",
            "",
            "\x01\x6e\x1f\uffff\x01\x6e",
            "\x01\x6f\x1f\uffff\x01\x6f",
            "\x01\x70",
            ""
    };

    static readonly short[] DFA19_eot = DFA.UnpackEncodedString(DFA19_eotS);
    static readonly short[] DFA19_eof = DFA.UnpackEncodedString(DFA19_eofS);
    static readonly char[] DFA19_min = DFA.UnpackEncodedStringToUnsignedChars(DFA19_minS);
    static readonly char[] DFA19_max = DFA.UnpackEncodedStringToUnsignedChars(DFA19_maxS);
    static readonly short[] DFA19_accept = DFA.UnpackEncodedString(DFA19_acceptS);
    static readonly short[] DFA19_special = DFA.UnpackEncodedString(DFA19_specialS);
    static readonly short[][] DFA19_transition = DFA.UnpackEncodedStringArray(DFA19_transitionS);

    protected class DFA19 : DFA
    {
        public DFA19(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 19;
            this.eot = DFA19_eot;
            this.eof = DFA19_eof;
            this.min = DFA19_min;
            this.max = DFA19_max;
            this.accept = DFA19_accept;
            this.special = DFA19_special;
            this.transition = DFA19_transition;

        }

        override public string Description
        {
            get { return "1:1: Tokens : ( FIX | GENR | LIST | SKIP | UPD | TIME | EOL | WHITESPACE | COMMENT | COMMENT_MULTILINE | Ident | StringInQuotes | Double | Integer | DisplayExpression | HdgExpression | TILDE | AND | EXCL | AT | HAT | SEMICOLON | COLON | COMMA | DOT | HASH | PERCENT | DOLLAR | LEFTCURLY | RIGHTCURLY | LEFTPAREN | RIGHTPAREN | LEFTBRACKET | RIGHTBRACKET | LEFTANGLE | RIGHTANGLE | STAR | VERTICALBAR | PLUS | MINUS | DIV | EQUAL | BACKSLASH | QUESTION | ANYTHING );"; }
        }

    }


    protected internal int DFA19_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            IIntStream input = _input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA19_0 = input.LA(1);

                   	s = -1;
                   	if ( (LA19_0 == 'F') ) { s = 1; }

                   	else if ( (LA19_0 == 'G') ) { s = 2; }

                   	else if ( (LA19_0 == 'L') ) { s = 3; }

                   	else if ( (LA19_0 == 'S') ) { s = 4; }

                   	else if ( (LA19_0 == 'U') ) { s = 5; }

                   	else if ( (LA19_0 == 'T') ) { s = 6; }

                   	else if ( (LA19_0 == '\n') ) { s = 7; }

                   	else if ( (LA19_0 == '\r') ) { s = 8; }

                   	else if ( (LA19_0 == '\t' || LA19_0 == '\f' || LA19_0 == ' ') ) { s = 9; }

                   	else if ( (LA19_0 == '/') ) { s = 10; }

                   	else if ( (LA19_0 == '(') ) { s = 11; }

                   	else if ( (LA19_0 == 'D' || LA19_0 == 'd') ) { s = 12; }

                   	else if ( (LA19_0 == '\'') ) { s = 13; }

                   	else if ( ((LA19_0 >= '0' && LA19_0 <= '9')) ) { s = 14; }

                   	else if ( (LA19_0 == 'H' || LA19_0 == 'h') ) { s = 15; }

                   	else if ( ((LA19_0 >= 'A' && LA19_0 <= 'C') || LA19_0 == 'E' || (LA19_0 >= 'I' && LA19_0 <= 'K') || (LA19_0 >= 'M' && LA19_0 <= 'R') || (LA19_0 >= 'V' && LA19_0 <= 'Z') || LA19_0 == '_' || (LA19_0 >= 'a' && LA19_0 <= 'c') || (LA19_0 >= 'e' && LA19_0 <= 'g') || (LA19_0 >= 'i' && LA19_0 <= 'z')) ) { s = 16; }

                   	else if ( (LA19_0 == '~') ) { s = 17; }

                   	else if ( (LA19_0 == '&') ) { s = 18; }

                   	else if ( (LA19_0 == '!') ) { s = 19; }

                   	else if ( (LA19_0 == '@') ) { s = 20; }

                   	else if ( (LA19_0 == '^') ) { s = 21; }

                   	else if ( (LA19_0 == ';') ) { s = 22; }

                   	else if ( (LA19_0 == ':') ) { s = 23; }

                   	else if ( (LA19_0 == ',') ) { s = 24; }

                   	else if ( (LA19_0 == '.') ) { s = 25; }

                   	else if ( (LA19_0 == '#') ) { s = 26; }

                   	else if ( (LA19_0 == '%') ) { s = 27; }

                   	else if ( (LA19_0 == '$') ) { s = 28; }

                   	else if ( (LA19_0 == '{') ) { s = 29; }

                   	else if ( (LA19_0 == '}') ) { s = 30; }

                   	else if ( (LA19_0 == ')') ) { s = 31; }

                   	else if ( (LA19_0 == '[') ) { s = 32; }

                   	else if ( (LA19_0 == ']') ) { s = 33; }

                   	else if ( (LA19_0 == '<') ) { s = 34; }

                   	else if ( (LA19_0 == '>') ) { s = 35; }

                   	else if ( (LA19_0 == '*') ) { s = 36; }

                   	else if ( (LA19_0 == '|') ) { s = 37; }

                   	else if ( (LA19_0 == '+') ) { s = 38; }

                   	else if ( (LA19_0 == '-') ) { s = 39; }

                   	else if ( (LA19_0 == '=') ) { s = 40; }

                   	else if ( (LA19_0 == '\\') ) { s = 41; }

                   	else if ( (LA19_0 == '?') ) { s = 42; }

                   	else if ( ((LA19_0 >= '\u0000' && LA19_0 <= '\b') || LA19_0 == '\u000B' || (LA19_0 >= '\u000E' && LA19_0 <= '\u001F') || LA19_0 == '\"' || LA19_0 == '`' || (LA19_0 >= '\u007F' && LA19_0 <= '\uFFFF')) ) { s = 43; }

                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA19_13 = input.LA(1);

                   	s = -1;
                   	if ( ((LA19_13 >= '\u0000' && LA19_13 <= '\uFFFF')) ) { s = 58; }

                   	else s = 43;

                   	if ( s >= 0 ) return s;
                   	break;
        }
        NoViableAltException nvae19 =
            new NoViableAltException(dfa.Description, 19, _s, input);
        dfa.Error(nvae19);
        throw nvae19;
    }
 
    
}
}