// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 T2.g 2016-10-23 22:35:04

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
public partial class T2Lexer : Lexer {
    public const int D_ = 81;
    public const int STAR = 59;
    public const int FIX = 23;
    public const int LETTER = 73;
    public const int ASTMINUS = 9;
    public const int ASTPAREN = 18;
    public const int U_ = 95;
    public const int EOF = -1;
    public const int L_ = 88;
    public const int ASTNEGATE = 4;
    public const int UPD = 27;
    public const int HAT = 53;
    public const int E_ = 74;
    public const int LEFTPAREN = 36;
    public const int EOL = 31;
    public const int VERTICALBAR = 61;
    public const int LEFTCURLY = 40;
    public const int RIGHTCURLY = 41;
    public const int T_ = 94;
    public const int COMMENT = 30;
    public const int M_ = 78;
    public const int Double = 45;
    public const int F_ = 82;
    public const int ASTSTARS = 6;
    public const int ASTCOMPARECOMMAND = 16;
    public const int ASTLIST = 22;
    public const int ASTADD = 10;
    public const int NEWLINE2 = 70;
    public const int NEWLINE3 = 71;
    public const int RIGHTBRACKET = 39;
    public const int WHITESPACE = 33;
    public const int W_ = 97;
    public const int SEMICOLON = 32;
    public const int LIST = 25;
    public const int N_ = 89;
    public const int SKIP = 26;
    public const int ASTSERIES = 17;
    public const int G_ = 83;
    public const int Ident = 49;
    public const int ASTBRACKET = 20;
    public const int DigitsEDigits = 46;
    public const int V_ = 96;
    public const int StringInQuotes = 42;
    public const int ASTCURLY = 21;
    public const int ASTANGLE = 19;
    public const int O_ = 90;
    public const int LEFTBRACKET = 38;
    public const int BACKSLASH = 66;
    public const int DOLLAR = 58;
    public const int LEFTANGLE = 34;
    public const int Y_ = 99;
    public const int AST1 = 29;
    public const int GENR = 24;
    public const int DateDef = 47;
    public const int Exponent = 75;
    public const int H_ = 84;
    public const int AND = 51;
    public const int Q_ = 77;
    public const int AT = 52;
    public const int X_ = 98;
    public const int TIME = 28;
    public const int COMMA = 68;
    public const int IdentStartingWithInt = 48;
    public const int EQUAL = 65;
    public const int A_ = 76;
    public const int TILDE = 50;
    public const int PLUS = 62;
    public const int DIGIT = 72;
    public const int I_ = 85;
    public const int ASTCOMMAND2 = 14;
    public const int DOT = 55;
    public const int ASTCOMMAND1 = 13;
    public const int ASTCOMMAND3 = 15;
    public const int P_ = 91;
    public const int ASTMULTIPLY = 11;
    public const int PERCENT = 57;
    public const int ASTCOMMAND = 12;
    public const int B_ = 79;
    public const int HASH = 56;
    public const int J_ = 86;
    public const int ASTPLUS = 8;
    public const int MINUS = 63;
    public const int ANYTHING = 69;
    public const int S_ = 93;
    public const int COLON = 54;
    public const int QUESTION = 67;
    public const int Z_ = 100;
    public const int RIGHTPAREN = 37;
    public const int StringInQuotes2 = 43;
    public const int C_ = 80;
    public const int ASTSTAR = 5;
    public const int K_ = 87;
    public const int STARS = 60;
    public const int RIGHTANGLE = 35;
    public const int DIV = 64;
    public const int ASTIDENT = 7;
    public const int Integer = 44;
    public const int R_ = 92;


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

    public T2Lexer() 
    {
		InitializeCyclicDFAs();
    }
    public T2Lexer(ICharStream input)
		: this(input, null) {
    }
    public T2Lexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "T2.g";} 
    }

    // $ANTLR start "FIX"
    public void mFIX() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FIX;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T2.g:45:5: ( 'FIX' )
            // T2.g:45:7: 'FIX'
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
            // T2.g:46:6: ( 'GENR' )
            // T2.g:46:8: 'GENR'
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
            // T2.g:47:6: ( 'LIST' )
            // T2.g:47:8: 'LIST'
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
            // T2.g:48:6: ( 'SKIP' )
            // T2.g:48:8: 'SKIP'
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
            // T2.g:49:5: ( 'UPD' )
            // T2.g:49:7: 'UPD'
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
            // T2.g:50:6: ( 'TIME' )
            // T2.g:50:8: 'TIME'
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
            // T2.g:249:27: ( '\\n' )
            // T2.g:249:29: '\\n'
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
            // T2.g:250:27: ( '\\r\\n' )
            // T2.g:250:29: '\\r\\n'
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
            // T2.g:251:27: ( '0' .. '9' )
            // T2.g:251:29: '0' .. '9'
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
            // T2.g:252:27: ( 'a' .. 'z' | 'A' .. 'Z' )
            // T2.g:
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
            // T2.g:253:27: ( E_ ( '+' | '-' )? ( DIGIT )+ )
            // T2.g:253:29: E_ ( '+' | '-' )? ( DIGIT )+
            {
            	mE_(); 
            	// T2.g:253:32: ( '+' | '-' )?
            	int alt1 = 2;
            	int LA1_0 = input.LA(1);

            	if ( (LA1_0 == '+' || LA1_0 == '-') )
            	{
            	    alt1 = 1;
            	}
            	switch (alt1) 
            	{
            	    case 1 :
            	        // T2.g:
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

            	// T2.g:253:47: ( DIGIT )+
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
            			    // T2.g:253:47: DIGIT
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
            // T2.g:255:27: ( NEWLINE2 | NEWLINE3 )
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
                    // T2.g:255:29: NEWLINE2
                    {
                    	mNEWLINE2(); 

                    }
                    break;
                case 2 :
                    // T2.g:255:40: NEWLINE3
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
            // T2.g:257:27: ( ( '\\t' | ' ' | '\\u000C' )+ )
            // T2.g:257:29: ( '\\t' | ' ' | '\\u000C' )+
            {
            	// T2.g:257:29: ( '\\t' | ' ' | '\\u000C' )+
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
            			    // T2.g:
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
            // T2.g:259:27: ( ( '!' ) (~ ( NEWLINE2 | NEWLINE3 ) )* )
            // T2.g:259:29: ( '!' ) (~ ( NEWLINE2 | NEWLINE3 ) )*
            {
            	// T2.g:259:29: ( '!' )
            	// T2.g:259:30: '!'
            	{
            		Match('!'); 

            	}

            	// T2.g:259:35: (~ ( NEWLINE2 | NEWLINE3 ) )*
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
            			    // T2.g:259:36: ~ ( NEWLINE2 | NEWLINE3 )
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

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMMENT"

    // $ANTLR start "Ident"
    public void mIdent() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Ident;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T2.g:262:27: ( ( LETTER | '_' ) ( DIGIT | LETTER | '_' )* )
            // T2.g:262:29: ( LETTER | '_' ) ( DIGIT | LETTER | '_' )*
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

            	// T2.g:262:42: ( DIGIT | LETTER | '_' )*
            	do 
            	{
            	    int alt6 = 2;
            	    int LA6_0 = input.LA(1);

            	    if ( ((LA6_0 >= '0' && LA6_0 <= '9') || (LA6_0 >= 'A' && LA6_0 <= 'Z') || LA6_0 == '_' || (LA6_0 >= 'a' && LA6_0 <= 'z')) )
            	    {
            	        alt6 = 1;
            	    }


            	    switch (alt6) 
            		{
            			case 1 :
            			    // T2.g:
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
            			    goto loop6;
            	    }
            	} while (true);

            	loop6:
            		;	// Stops C# compiler whining that label 'loop6' has no statements

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
            // T2.g:265:27: ( ( '\\'' (~ '\\'' )* '\\'' ) )
            // T2.g:265:29: ( '\\'' (~ '\\'' )* '\\'' )
            {
            	// T2.g:265:29: ( '\\'' (~ '\\'' )* '\\'' )
            	// T2.g:265:30: '\\'' (~ '\\'' )* '\\''
            	{
            		Match('\''); 
            		// T2.g:265:35: (~ '\\'' )*
            		do 
            		{
            		    int alt7 = 2;
            		    int LA7_0 = input.LA(1);

            		    if ( ((LA7_0 >= '\u0000' && LA7_0 <= '&') || (LA7_0 >= '(' && LA7_0 <= '\uFFFF')) )
            		    {
            		        alt7 = 1;
            		    }


            		    switch (alt7) 
            			{
            				case 1 :
            				    // T2.g:265:36: ~ '\\''
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
            				    goto loop7;
            		    }
            		} while (true);

            		loop7:
            			;	// Stops C# compiler whining that label 'loop7' has no statements

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

    // $ANTLR start "StringInQuotes2"
    public void mStringInQuotes2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = StringInQuotes2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T2.g:266:28: ( ( '\\\"' (~ '\\\"' )* '\\\"' ) )
            // T2.g:266:30: ( '\\\"' (~ '\\\"' )* '\\\"' )
            {
            	// T2.g:266:30: ( '\\\"' (~ '\\\"' )* '\\\"' )
            	// T2.g:266:31: '\\\"' (~ '\\\"' )* '\\\"'
            	{
            		Match('\"'); 
            		// T2.g:266:36: (~ '\\\"' )*
            		do 
            		{
            		    int alt8 = 2;
            		    int LA8_0 = input.LA(1);

            		    if ( ((LA8_0 >= '\u0000' && LA8_0 <= '!') || (LA8_0 >= '#' && LA8_0 <= '\uFFFF')) )
            		    {
            		        alt8 = 1;
            		    }


            		    switch (alt8) 
            			{
            				case 1 :
            				    // T2.g:266:37: ~ '\\\"'
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
            				    goto loop8;
            		    }
            		} while (true);

            		loop8:
            			;	// Stops C# compiler whining that label 'loop8' has no statements

            		Match('\"'); 

            	}


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "StringInQuotes2"

    // $ANTLR start "Integer"
    public void mInteger() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Integer;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T2.g:269:27: ( ( DIGIT )+ )
            // T2.g:269:29: ( DIGIT )+
            {
            	// T2.g:269:29: ( DIGIT )+
            	int cnt9 = 0;
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
            			    // T2.g:269:29: DIGIT
            			    {
            			    	mDIGIT(); 

            			    }
            			    break;

            			default:
            			    if ( cnt9 >= 1 ) goto loop9;
            		            EarlyExitException eee9 =
            		                new EarlyExitException(9, input);
            		            throw eee9;
            	    }
            	    cnt9++;
            	} while (true);

            	loop9:
            		;	// Stops C# compiler whining that label 'loop9' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "Integer"

    // $ANTLR start "DigitsEDigits"
    public void mDigitsEDigits() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DigitsEDigits;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T2.g:271:27: ( ( DIGIT )+ ( E_ ) ( DIGIT )+ )
            // T2.g:271:29: ( DIGIT )+ ( E_ ) ( DIGIT )+
            {
            	// T2.g:271:29: ( DIGIT )+
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
            			    // T2.g:271:29: DIGIT
            			    {
            			    	mDIGIT(); 

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

            	// T2.g:271:37: ( E_ )
            	// T2.g:271:39: E_
            	{
            		mE_(); 

            	}

            	// T2.g:271:45: ( DIGIT )+
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
            			    // T2.g:271:45: DIGIT
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


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DigitsEDigits"

    // $ANTLR start "DateDef"
    public void mDateDef() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DateDef;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T2.g:273:27: ( ( DIGIT )+ ( A_ | Q_ | M_ ) ( DIGIT )+ )
            // T2.g:273:29: ( DIGIT )+ ( A_ | Q_ | M_ ) ( DIGIT )+
            {
            	// T2.g:273:29: ( DIGIT )+
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
            			    // T2.g:273:29: DIGIT
            			    {
            			    	mDIGIT(); 

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

            	if ( input.LA(1) == 'A' || input.LA(1) == 'M' || input.LA(1) == 'Q' || input.LA(1) == 'a' || input.LA(1) == 'm' || input.LA(1) == 'q' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// T2.g:273:54: ( DIGIT )+
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
            			    // T2.g:273:54: DIGIT
            			    {
            			    	mDIGIT(); 

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


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DateDef"

    // $ANTLR start "IdentStartingWithInt"
    public void mIdentStartingWithInt() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = IdentStartingWithInt;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T2.g:275:27: ( ( DIGIT | LETTER | '_' )+ )
            // T2.g:275:29: ( DIGIT | LETTER | '_' )+
            {
            	// T2.g:275:29: ( DIGIT | LETTER | '_' )+
            	int cnt14 = 0;
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
            			    // T2.g:
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
            			    if ( cnt14 >= 1 ) goto loop14;
            		            EarlyExitException eee14 =
            		                new EarlyExitException(14, input);
            		            throw eee14;
            	    }
            	    cnt14++;
            	} while (true);

            	loop14:
            		;	// Stops C# compiler whining that label 'loop14' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "IdentStartingWithInt"

    // $ANTLR start "Double"
    public void mDouble() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Double;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T2.g:277:27: ( ( DIGIT )+ DOT ( DIGIT )* ( Exponent )? | ( DIGIT )+ Exponent | DOT ( DIGIT )+ ( Exponent )? )
            int alt21 = 3;
            alt21 = dfa21.Predict(input);
            switch (alt21) 
            {
                case 1 :
                    // T2.g:277:29: ( DIGIT )+ DOT ( DIGIT )* ( Exponent )?
                    {
                    	// T2.g:277:29: ( DIGIT )+
                    	int cnt15 = 0;
                    	do 
                    	{
                    	    int alt15 = 2;
                    	    int LA15_0 = input.LA(1);

                    	    if ( ((LA15_0 >= '0' && LA15_0 <= '9')) )
                    	    {
                    	        alt15 = 1;
                    	    }


                    	    switch (alt15) 
                    		{
                    			case 1 :
                    			    // T2.g:277:29: DIGIT
                    			    {
                    			    	mDIGIT(); 

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

                    	mDOT(); 
                    	// T2.g:277:40: ( DIGIT )*
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
                    			    // T2.g:277:40: DIGIT
                    			    {
                    			    	mDIGIT(); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop16;
                    	    }
                    	} while (true);

                    	loop16:
                    		;	// Stops C# compiler whining that label 'loop16' has no statements

                    	// T2.g:277:47: ( Exponent )?
                    	int alt17 = 2;
                    	int LA17_0 = input.LA(1);

                    	if ( (LA17_0 == 'E' || LA17_0 == 'e') )
                    	{
                    	    alt17 = 1;
                    	}
                    	switch (alt17) 
                    	{
                    	    case 1 :
                    	        // T2.g:277:47: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // T2.g:278:29: ( DIGIT )+ Exponent
                    {
                    	// T2.g:278:29: ( DIGIT )+
                    	int cnt18 = 0;
                    	do 
                    	{
                    	    int alt18 = 2;
                    	    int LA18_0 = input.LA(1);

                    	    if ( ((LA18_0 >= '0' && LA18_0 <= '9')) )
                    	    {
                    	        alt18 = 1;
                    	    }


                    	    switch (alt18) 
                    		{
                    			case 1 :
                    			    // T2.g:278:29: DIGIT
                    			    {
                    			    	mDIGIT(); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt18 >= 1 ) goto loop18;
                    		            EarlyExitException eee18 =
                    		                new EarlyExitException(18, input);
                    		            throw eee18;
                    	    }
                    	    cnt18++;
                    	} while (true);

                    	loop18:
                    		;	// Stops C# compiler whining that label 'loop18' has no statements

                    	mExponent(); 

                    }
                    break;
                case 3 :
                    // T2.g:279:11: DOT ( DIGIT )+ ( Exponent )?
                    {
                    	mDOT(); 
                    	// T2.g:279:15: ( DIGIT )+
                    	int cnt19 = 0;
                    	do 
                    	{
                    	    int alt19 = 2;
                    	    int LA19_0 = input.LA(1);

                    	    if ( ((LA19_0 >= '0' && LA19_0 <= '9')) )
                    	    {
                    	        alt19 = 1;
                    	    }


                    	    switch (alt19) 
                    		{
                    			case 1 :
                    			    // T2.g:279:15: DIGIT
                    			    {
                    			    	mDIGIT(); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt19 >= 1 ) goto loop19;
                    		            EarlyExitException eee19 =
                    		                new EarlyExitException(19, input);
                    		            throw eee19;
                    	    }
                    	    cnt19++;
                    	} while (true);

                    	loop19:
                    		;	// Stops C# compiler whining that label 'loop19' has no statements

                    	// T2.g:279:22: ( Exponent )?
                    	int alt20 = 2;
                    	int LA20_0 = input.LA(1);

                    	if ( (LA20_0 == 'E' || LA20_0 == 'e') )
                    	{
                    	    alt20 = 1;
                    	}
                    	switch (alt20) 
                    	{
                    	    case 1 :
                    	        // T2.g:279:22: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

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
    // $ANTLR end "Double"

    // $ANTLR start "TILDE"
    public void mTILDE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TILDE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T2.g:282:27: ( '~' )
            // T2.g:282:29: '~'
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
            // T2.g:283:27: ( '&' )
            // T2.g:283:29: '&'
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

    // $ANTLR start "AT"
    public void mAT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T2.g:285:27: ( '@' )
            // T2.g:285:29: '@'
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
            // T2.g:286:27: ( '^' )
            // T2.g:286:29: '^'
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
            // T2.g:287:27: ( ';' )
            // T2.g:287:29: ';'
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
            // T2.g:288:27: ( ':' )
            // T2.g:288:29: ':'
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
            // T2.g:289:27: ( ',' )
            // T2.g:289:29: ','
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
            // T2.g:290:27: ( '.' )
            // T2.g:290:29: '.'
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
            // T2.g:291:27: ( '#' )
            // T2.g:291:29: '#'
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
            // T2.g:292:27: ( '%' )
            // T2.g:292:29: '%'
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
            // T2.g:293:27: ( '$' )
            // T2.g:293:29: '$'
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
            // T2.g:294:27: ( '{' )
            // T2.g:294:29: '{'
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
            // T2.g:295:27: ( '}' )
            // T2.g:295:29: '}'
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
            // T2.g:296:27: ( '(' )
            // T2.g:296:29: '('
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
            // T2.g:297:27: ( ')' )
            // T2.g:297:29: ')'
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
            // T2.g:298:27: ( '[' )
            // T2.g:298:29: '['
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
            // T2.g:299:27: ( ']' )
            // T2.g:299:29: ']'
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
            // T2.g:300:27: ( '<' )
            // T2.g:300:29: '<'
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
            // T2.g:301:27: ( '>' )
            // T2.g:301:29: '>'
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

    // $ANTLR start "STARS"
    public void mSTARS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STARS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T2.g:302:27: ( '**' )
            // T2.g:302:29: '**'
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

    // $ANTLR start "STAR"
    public void mSTAR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STAR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // T2.g:303:27: ( '*' )
            // T2.g:303:29: '*'
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
            // T2.g:304:27: ( '|' )
            // T2.g:304:29: '|'
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
            // T2.g:305:27: ( '+' )
            // T2.g:305:29: '+'
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
            // T2.g:306:27: ( '-' )
            // T2.g:306:29: '-'
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
            // T2.g:307:27: ( '/' )
            // T2.g:307:29: '/'
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
            // T2.g:308:27: ( '=' )
            // T2.g:308:29: '='
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
            // T2.g:309:27: ( '\\\\' )
            // T2.g:309:29: '\\\\'
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
            // T2.g:310:27: ( '?' )
            // T2.g:310:29: '?'
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
            // T2.g:312:27: ( . )
            // T2.g:312:29: .
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
            // T2.g:314:12: ( ( 'a' | 'A' ) )
            // T2.g:314:13: ( 'a' | 'A' )
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
            // T2.g:315:12: ( ( 'b' | 'B' ) )
            // T2.g:315:13: ( 'b' | 'B' )
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
            // T2.g:316:12: ( ( 'c' | 'C' ) )
            // T2.g:316:13: ( 'c' | 'C' )
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
            // T2.g:317:12: ( ( 'd' | 'D' ) )
            // T2.g:317:13: ( 'd' | 'D' )
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
            // T2.g:318:12: ( ( 'e' | 'E' ) )
            // T2.g:318:13: ( 'e' | 'E' )
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
            // T2.g:319:12: ( ( 'f' | 'F' ) )
            // T2.g:319:13: ( 'f' | 'F' )
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
            // T2.g:320:12: ( ( 'g' | 'G' ) )
            // T2.g:320:13: ( 'g' | 'G' )
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
            // T2.g:321:12: ( ( 'h' | 'H' ) )
            // T2.g:321:13: ( 'h' | 'H' )
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
            // T2.g:322:12: ( ( 'i' | 'I' ) )
            // T2.g:322:13: ( 'i' | 'I' )
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
            // T2.g:323:12: ( ( 'j' | 'J' ) )
            // T2.g:323:13: ( 'j' | 'J' )
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
            // T2.g:324:12: ( ( 'k' | 'K' ) )
            // T2.g:324:13: ( 'k' | 'K' )
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
            // T2.g:325:12: ( ( 'l' | 'L' ) )
            // T2.g:325:13: ( 'l' | 'L' )
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
            // T2.g:326:12: ( ( 'm' | 'M' ) )
            // T2.g:326:13: ( 'm' | 'M' )
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
            // T2.g:327:12: ( ( 'n' | 'N' ) )
            // T2.g:327:13: ( 'n' | 'N' )
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
            // T2.g:328:12: ( ( 'o' | 'O' ) )
            // T2.g:328:13: ( 'o' | 'O' )
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
            // T2.g:329:12: ( ( 'p' | 'P' ) )
            // T2.g:329:13: ( 'p' | 'P' )
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
            // T2.g:330:12: ( ( 'q' | 'Q' ) )
            // T2.g:330:13: ( 'q' | 'Q' )
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
            // T2.g:331:12: ( ( 'r' | 'R' ) )
            // T2.g:331:13: ( 'r' | 'R' )
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
            // T2.g:332:12: ( ( 's' | 'S' ) )
            // T2.g:332:13: ( 's' | 'S' )
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
            // T2.g:333:12: ( ( 't' | 'T' ) )
            // T2.g:333:13: ( 't' | 'T' )
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
            // T2.g:334:12: ( ( 'u' | 'U' ) )
            // T2.g:334:13: ( 'u' | 'U' )
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
            // T2.g:335:12: ( ( 'v' | 'V' ) )
            // T2.g:335:13: ( 'v' | 'V' )
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
            // T2.g:336:12: ( ( 'w' | 'W' ) )
            // T2.g:336:13: ( 'w' | 'W' )
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
            // T2.g:337:12: ( ( 'x' | 'X' ) )
            // T2.g:337:13: ( 'x' | 'X' )
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
            // T2.g:338:12: ( ( 'y' | 'Y' ) )
            // T2.g:338:13: ( 'y' | 'Y' )
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
            // T2.g:339:12: ( ( 'z' | 'Z' ) )
            // T2.g:339:13: ( 'z' | 'Z' )
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
        // T2.g:1:8: ( FIX | GENR | LIST | SKIP | UPD | TIME | EOL | WHITESPACE | COMMENT | Ident | StringInQuotes | StringInQuotes2 | Integer | DigitsEDigits | DateDef | IdentStartingWithInt | Double | TILDE | AND | AT | HAT | SEMICOLON | COLON | COMMA | DOT | HASH | PERCENT | DOLLAR | LEFTCURLY | RIGHTCURLY | LEFTPAREN | RIGHTPAREN | LEFTBRACKET | RIGHTBRACKET | LEFTANGLE | RIGHTANGLE | STARS | STAR | VERTICALBAR | PLUS | MINUS | DIV | EQUAL | BACKSLASH | QUESTION | ANYTHING )
        int alt22 = 46;
        alt22 = dfa22.Predict(input);
        switch (alt22) 
        {
            case 1 :
                // T2.g:1:10: FIX
                {
                	mFIX(); 

                }
                break;
            case 2 :
                // T2.g:1:14: GENR
                {
                	mGENR(); 

                }
                break;
            case 3 :
                // T2.g:1:19: LIST
                {
                	mLIST(); 

                }
                break;
            case 4 :
                // T2.g:1:24: SKIP
                {
                	mSKIP(); 

                }
                break;
            case 5 :
                // T2.g:1:29: UPD
                {
                	mUPD(); 

                }
                break;
            case 6 :
                // T2.g:1:33: TIME
                {
                	mTIME(); 

                }
                break;
            case 7 :
                // T2.g:1:38: EOL
                {
                	mEOL(); 

                }
                break;
            case 8 :
                // T2.g:1:42: WHITESPACE
                {
                	mWHITESPACE(); 

                }
                break;
            case 9 :
                // T2.g:1:53: COMMENT
                {
                	mCOMMENT(); 

                }
                break;
            case 10 :
                // T2.g:1:61: Ident
                {
                	mIdent(); 

                }
                break;
            case 11 :
                // T2.g:1:67: StringInQuotes
                {
                	mStringInQuotes(); 

                }
                break;
            case 12 :
                // T2.g:1:82: StringInQuotes2
                {
                	mStringInQuotes2(); 

                }
                break;
            case 13 :
                // T2.g:1:98: Integer
                {
                	mInteger(); 

                }
                break;
            case 14 :
                // T2.g:1:106: DigitsEDigits
                {
                	mDigitsEDigits(); 

                }
                break;
            case 15 :
                // T2.g:1:120: DateDef
                {
                	mDateDef(); 

                }
                break;
            case 16 :
                // T2.g:1:128: IdentStartingWithInt
                {
                	mIdentStartingWithInt(); 

                }
                break;
            case 17 :
                // T2.g:1:149: Double
                {
                	mDouble(); 

                }
                break;
            case 18 :
                // T2.g:1:156: TILDE
                {
                	mTILDE(); 

                }
                break;
            case 19 :
                // T2.g:1:162: AND
                {
                	mAND(); 

                }
                break;
            case 20 :
                // T2.g:1:166: AT
                {
                	mAT(); 

                }
                break;
            case 21 :
                // T2.g:1:169: HAT
                {
                	mHAT(); 

                }
                break;
            case 22 :
                // T2.g:1:173: SEMICOLON
                {
                	mSEMICOLON(); 

                }
                break;
            case 23 :
                // T2.g:1:183: COLON
                {
                	mCOLON(); 

                }
                break;
            case 24 :
                // T2.g:1:189: COMMA
                {
                	mCOMMA(); 

                }
                break;
            case 25 :
                // T2.g:1:195: DOT
                {
                	mDOT(); 

                }
                break;
            case 26 :
                // T2.g:1:199: HASH
                {
                	mHASH(); 

                }
                break;
            case 27 :
                // T2.g:1:204: PERCENT
                {
                	mPERCENT(); 

                }
                break;
            case 28 :
                // T2.g:1:212: DOLLAR
                {
                	mDOLLAR(); 

                }
                break;
            case 29 :
                // T2.g:1:219: LEFTCURLY
                {
                	mLEFTCURLY(); 

                }
                break;
            case 30 :
                // T2.g:1:229: RIGHTCURLY
                {
                	mRIGHTCURLY(); 

                }
                break;
            case 31 :
                // T2.g:1:240: LEFTPAREN
                {
                	mLEFTPAREN(); 

                }
                break;
            case 32 :
                // T2.g:1:250: RIGHTPAREN
                {
                	mRIGHTPAREN(); 

                }
                break;
            case 33 :
                // T2.g:1:261: LEFTBRACKET
                {
                	mLEFTBRACKET(); 

                }
                break;
            case 34 :
                // T2.g:1:273: RIGHTBRACKET
                {
                	mRIGHTBRACKET(); 

                }
                break;
            case 35 :
                // T2.g:1:286: LEFTANGLE
                {
                	mLEFTANGLE(); 

                }
                break;
            case 36 :
                // T2.g:1:296: RIGHTANGLE
                {
                	mRIGHTANGLE(); 

                }
                break;
            case 37 :
                // T2.g:1:307: STARS
                {
                	mSTARS(); 

                }
                break;
            case 38 :
                // T2.g:1:313: STAR
                {
                	mSTAR(); 

                }
                break;
            case 39 :
                // T2.g:1:318: VERTICALBAR
                {
                	mVERTICALBAR(); 

                }
                break;
            case 40 :
                // T2.g:1:330: PLUS
                {
                	mPLUS(); 

                }
                break;
            case 41 :
                // T2.g:1:335: MINUS
                {
                	mMINUS(); 

                }
                break;
            case 42 :
                // T2.g:1:341: DIV
                {
                	mDIV(); 

                }
                break;
            case 43 :
                // T2.g:1:345: EQUAL
                {
                	mEQUAL(); 

                }
                break;
            case 44 :
                // T2.g:1:351: BACKSLASH
                {
                	mBACKSLASH(); 

                }
                break;
            case 45 :
                // T2.g:1:361: QUESTION
                {
                	mQUESTION(); 

                }
                break;
            case 46 :
                // T2.g:1:370: ANYTHING
                {
                	mANYTHING(); 

                }
                break;

        }

    }


    protected DFA21 dfa21;
    protected DFA22 dfa22;
	private void InitializeCyclicDFAs()
	{
	    this.dfa21 = new DFA21(this);
	    this.dfa22 = new DFA22(this);

	    this.dfa22.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA22_SpecialStateTransition);
	}

    const string DFA21_eotS =
        "\x05\uffff";
    const string DFA21_eofS =
        "\x05\uffff";
    const string DFA21_minS =
        "\x02\x2e\x03\uffff";
    const string DFA21_maxS =
        "\x01\x39\x01\x65\x03\uffff";
    const string DFA21_acceptS =
        "\x02\uffff\x01\x03\x01\x01\x01\x02";
    const string DFA21_specialS =
        "\x05\uffff}>";
    static readonly string[] DFA21_transitionS = {
            "\x01\x02\x01\uffff\x0a\x01",
            "\x01\x03\x01\uffff\x0a\x01\x0b\uffff\x01\x04\x1f\uffff\x01"+
            "\x04",
            "",
            "",
            ""
    };

    static readonly short[] DFA21_eot = DFA.UnpackEncodedString(DFA21_eotS);
    static readonly short[] DFA21_eof = DFA.UnpackEncodedString(DFA21_eofS);
    static readonly char[] DFA21_min = DFA.UnpackEncodedStringToUnsignedChars(DFA21_minS);
    static readonly char[] DFA21_max = DFA.UnpackEncodedStringToUnsignedChars(DFA21_maxS);
    static readonly short[] DFA21_accept = DFA.UnpackEncodedString(DFA21_acceptS);
    static readonly short[] DFA21_special = DFA.UnpackEncodedString(DFA21_specialS);
    static readonly short[][] DFA21_transition = DFA.UnpackEncodedStringArray(DFA21_transitionS);

    protected class DFA21 : DFA
    {
        public DFA21(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 21;
            this.eot = DFA21_eot;
            this.eof = DFA21_eof;
            this.min = DFA21_min;
            this.max = DFA21_max;
            this.accept = DFA21_accept;
            this.special = DFA21_special;
            this.transition = DFA21_transition;

        }

        override public string Description
        {
            get { return "277:1: Double : ( ( DIGIT )+ DOT ( DIGIT )* ( Exponent )? | ( DIGIT )+ Exponent | DOT ( DIGIT )+ ( Exponent )? );"; }
        }

    }

    const string DFA22_eotS =
        "\x01\uffff\x06\x2c\x01\uffff\x01\x2a\x02\uffff\x01\x2c\x02\x2a"+
        "\x01\x38\x01\x3e\x12\uffff\x01\x52\x08\uffff\x01\x2c\x01\uffff\x06"+
        "\x2c\x06\uffff\x01\x3d\x01\x38\x01\x3d\x1e\uffff\x01\x62\x03\x2c"+
        "\x01\x66\x01\x2c\x01\x68\x01\x69\x01\uffff\x01\x6a\x01\x6b\x01\x6c"+
        "\x01\uffff\x01\x6d\x06\uffff";
    const string DFA22_eofS =
        "\x6e\uffff";
    const string DFA22_minS =
        "\x01\x00\x06\x30\x01\uffff\x01\x0a\x02\uffff\x01\x30\x02\x00\x01"+
        "\x2e\x01\x30\x12\uffff\x01\x2a\x08\uffff\x01\x30\x01\uffff\x06\x30"+
        "\x06\uffff\x01\x2b\x01\x2e\x01\x30\x1e\uffff\x08\x30\x01\uffff\x03"+
        "\x30\x01\uffff\x01\x30\x06\uffff";
    const string DFA22_maxS =
        "\x01\uffff\x06\x7a\x01\uffff\x01\x0a\x02\uffff\x01\x7a\x02\uffff"+
        "\x01\x7a\x01\x39\x12\uffff\x01\x2a\x08\uffff\x01\x7a\x01\uffff\x06"+
        "\x7a\x06\uffff\x01\x39\x01\x7a\x01\x39\x1e\uffff\x08\x7a\x01\uffff"+
        "\x03\x7a\x01\uffff\x01\x7a\x06\uffff";
    const string DFA22_acceptS =
        "\x07\uffff\x01\x07\x01\uffff\x01\x08\x01\x09\x05\uffff\x01\x12"+
        "\x01\x13\x01\x14\x01\x15\x01\x16\x01\x17\x01\x18\x01\x1a\x01\x1b"+
        "\x01\x1c\x01\x1d\x01\x1e\x01\x1f\x01\x20\x01\x21\x01\x22\x01\x23"+
        "\x01\x24\x01\uffff\x01\x27\x01\x28\x01\x29\x01\x2a\x01\x2b\x01\x2c"+
        "\x01\x2d\x01\x2e\x01\uffff\x01\x0a\x06\uffff\x01\x07\x01\x08\x01"+
        "\x09\x01\x0b\x01\x0c\x01\x0d\x03\uffff\x01\x11\x01\x10\x01\x19\x01"+
        "\x12\x01\x13\x01\x14\x01\x15\x01\x16\x01\x17\x01\x18\x01\x1a\x01"+
        "\x1b\x01\x1c\x01\x1d\x01\x1e\x01\x1f\x01\x20\x01\x21\x01\x22\x01"+
        "\x23\x01\x24\x01\x25\x01\x26\x01\x27\x01\x28\x01\x29\x01\x2a\x01"+
        "\x2b\x01\x2c\x01\x2d\x08\uffff\x01\x01\x03\uffff\x01\x05\x01\uffff"+
        "\x01\x0e\x01\x0f\x01\x02\x01\x03\x01\x04\x01\x06";
    const string DFA22_specialS =
        "\x01\x02\x0b\uffff\x01\x00\x01\x01\x60\uffff}>";
    static readonly string[] DFA22_transitionS = {
            "\x09\x2a\x01\x09\x01\x07\x01\x2a\x01\x09\x01\x08\x12\x2a\x01"+
            "\x09\x01\x0a\x01\x0d\x01\x17\x01\x19\x01\x18\x01\x11\x01\x0c"+
            "\x01\x1c\x01\x1d\x01\x22\x01\x24\x01\x16\x01\x25\x01\x0f\x01"+
            "\x26\x0a\x0e\x01\x15\x01\x14\x01\x20\x01\x27\x01\x21\x01\x29"+
            "\x01\x12\x05\x0b\x01\x01\x01\x02\x04\x0b\x01\x03\x06\x0b\x01"+
            "\x04\x01\x06\x01\x05\x05\x0b\x01\x1e\x01\x28\x01\x1f\x01\x13"+
            "\x01\x0b\x01\x2a\x1a\x0b\x01\x1a\x01\x23\x01\x1b\x01\x10\uff81"+
            "\x2a",
            "\x0a\x2d\x07\uffff\x08\x2d\x01\x2b\x11\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x04\x2d\x01\x2e\x15\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x08\x2d\x01\x2f\x11\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x0a\x2d\x01\x30\x0f\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x0f\x2d\x01\x31\x0a\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x08\x2d\x01\x32\x11\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "",
            "\x01\x33",
            "",
            "",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x00\x36",
            "\x00\x37",
            "\x01\x3c\x01\uffff\x0a\x3a\x07\uffff\x01\x3b\x03\x3d\x01\x39"+
            "\x07\x3d\x01\x3b\x03\x3d\x01\x3b\x09\x3d\x04\uffff\x01\x3d\x01"+
            "\uffff\x01\x3b\x03\x3d\x01\x39\x07\x3d\x01\x3b\x03\x3d\x01\x3b"+
            "\x09\x3d",
            "\x0a\x3c",
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
            "\x01\x51",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x0a\x2d\x07\uffff\x17\x2d\x01\x5a\x02\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x0d\x2d\x01\x5b\x0c\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x12\x2d\x01\x5c\x07\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x08\x2d\x01\x5d\x11\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x03\x2d\x01\x5e\x16\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x0c\x2d\x01\x5f\x0d\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x3c\x01\uffff\x01\x3c\x02\uffff\x0a\x60",
            "\x01\x3c\x01\uffff\x0a\x3a\x07\uffff\x01\x3b\x03\x3d\x01\x39"+
            "\x07\x3d\x01\x3b\x03\x3d\x01\x3b\x09\x3d\x04\uffff\x01\x3d\x01"+
            "\uffff\x01\x3b\x03\x3d\x01\x39\x07\x3d\x01\x3b\x03\x3d\x01\x3b"+
            "\x09\x3d",
            "\x0a\x61",
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
            "",
            "",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x11\x2d\x01\x63\x08\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x13\x2d\x01\x64\x06\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x0f\x2d\x01\x65\x0a\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x04\x2d\x01\x67\x15\x2d\x04\uffff\x01\x2d"+
            "\x01\uffff\x1a\x2d",
            "\x0a\x60\x07\uffff\x1a\x3d\x04\uffff\x01\x3d\x01\uffff\x1a"+
            "\x3d",
            "\x0a\x61\x07\uffff\x1a\x3d\x04\uffff\x01\x3d\x01\uffff\x1a"+
            "\x3d",
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
            "",
            "",
            "",
            "",
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
            get { return "1:1: Tokens : ( FIX | GENR | LIST | SKIP | UPD | TIME | EOL | WHITESPACE | COMMENT | Ident | StringInQuotes | StringInQuotes2 | Integer | DigitsEDigits | DateDef | IdentStartingWithInt | Double | TILDE | AND | AT | HAT | SEMICOLON | COLON | COMMA | DOT | HASH | PERCENT | DOLLAR | LEFTCURLY | RIGHTCURLY | LEFTPAREN | RIGHTPAREN | LEFTBRACKET | RIGHTBRACKET | LEFTANGLE | RIGHTANGLE | STARS | STAR | VERTICALBAR | PLUS | MINUS | DIV | EQUAL | BACKSLASH | QUESTION | ANYTHING );"; }
        }

    }


    protected internal int DFA22_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            IIntStream input = _input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA22_12 = input.LA(1);

                   	s = -1;
                   	if ( ((LA22_12 >= '\u0000' && LA22_12 <= '\uFFFF')) ) { s = 54; }

                   	else s = 42;

                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA22_13 = input.LA(1);

                   	s = -1;
                   	if ( ((LA22_13 >= '\u0000' && LA22_13 <= '\uFFFF')) ) { s = 55; }

                   	else s = 42;

                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA22_0 = input.LA(1);

                   	s = -1;
                   	if ( (LA22_0 == 'F') ) { s = 1; }

                   	else if ( (LA22_0 == 'G') ) { s = 2; }

                   	else if ( (LA22_0 == 'L') ) { s = 3; }

                   	else if ( (LA22_0 == 'S') ) { s = 4; }

                   	else if ( (LA22_0 == 'U') ) { s = 5; }

                   	else if ( (LA22_0 == 'T') ) { s = 6; }

                   	else if ( (LA22_0 == '\n') ) { s = 7; }

                   	else if ( (LA22_0 == '\r') ) { s = 8; }

                   	else if ( (LA22_0 == '\t' || LA22_0 == '\f' || LA22_0 == ' ') ) { s = 9; }

                   	else if ( (LA22_0 == '!') ) { s = 10; }

                   	else if ( ((LA22_0 >= 'A' && LA22_0 <= 'E') || (LA22_0 >= 'H' && LA22_0 <= 'K') || (LA22_0 >= 'M' && LA22_0 <= 'R') || (LA22_0 >= 'V' && LA22_0 <= 'Z') || LA22_0 == '_' || (LA22_0 >= 'a' && LA22_0 <= 'z')) ) { s = 11; }

                   	else if ( (LA22_0 == '\'') ) { s = 12; }

                   	else if ( (LA22_0 == '\"') ) { s = 13; }

                   	else if ( ((LA22_0 >= '0' && LA22_0 <= '9')) ) { s = 14; }

                   	else if ( (LA22_0 == '.') ) { s = 15; }

                   	else if ( (LA22_0 == '~') ) { s = 16; }

                   	else if ( (LA22_0 == '&') ) { s = 17; }

                   	else if ( (LA22_0 == '@') ) { s = 18; }

                   	else if ( (LA22_0 == '^') ) { s = 19; }

                   	else if ( (LA22_0 == ';') ) { s = 20; }

                   	else if ( (LA22_0 == ':') ) { s = 21; }

                   	else if ( (LA22_0 == ',') ) { s = 22; }

                   	else if ( (LA22_0 == '#') ) { s = 23; }

                   	else if ( (LA22_0 == '%') ) { s = 24; }

                   	else if ( (LA22_0 == '$') ) { s = 25; }

                   	else if ( (LA22_0 == '{') ) { s = 26; }

                   	else if ( (LA22_0 == '}') ) { s = 27; }

                   	else if ( (LA22_0 == '(') ) { s = 28; }

                   	else if ( (LA22_0 == ')') ) { s = 29; }

                   	else if ( (LA22_0 == '[') ) { s = 30; }

                   	else if ( (LA22_0 == ']') ) { s = 31; }

                   	else if ( (LA22_0 == '<') ) { s = 32; }

                   	else if ( (LA22_0 == '>') ) { s = 33; }

                   	else if ( (LA22_0 == '*') ) { s = 34; }

                   	else if ( (LA22_0 == '|') ) { s = 35; }

                   	else if ( (LA22_0 == '+') ) { s = 36; }

                   	else if ( (LA22_0 == '-') ) { s = 37; }

                   	else if ( (LA22_0 == '/') ) { s = 38; }

                   	else if ( (LA22_0 == '=') ) { s = 39; }

                   	else if ( (LA22_0 == '\\') ) { s = 40; }

                   	else if ( (LA22_0 == '?') ) { s = 41; }

                   	else if ( ((LA22_0 >= '\u0000' && LA22_0 <= '\b') || LA22_0 == '\u000B' || (LA22_0 >= '\u000E' && LA22_0 <= '\u001F') || LA22_0 == '`' || (LA22_0 >= '\u007F' && LA22_0 <= '\uFFFF')) ) { s = 42; }

                   	if ( s >= 0 ) return s;
                   	break;
        }
        NoViableAltException nvae22 =
            new NoViableAltException(dfa.Description, 22, _s, input);
        dfa.Error(nvae22);
        throw nvae22;
    }
 
    
}
}