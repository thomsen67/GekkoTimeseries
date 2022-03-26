// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 GAMS.g 2022-03-26 20:27:30

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
    public const int ASTPOW = 15;
    public const int COMMENT1 = 91;
    public const int COMMENT2 = 92;
    public const int ASTFUNCTION3 = 29;
    public const int ASTFUNCTION2 = 28;
    public const int ASTVARIABLE = 16;
    public const int LETTER = 96;
    public const int ASTFUNCTION1 = 27;
    public const int MOD = 80;
    public const int ASTIDX = 33;
    public const int LOG = 54;
    public const int ASTINDEXES1 = 18;
    public const int ASTCONDITIONAL = 25;
    public const int DOUBLEDOT = 61;
    public const int ASTINDEXES3 = 20;
    public const int NOT = 51;
    public const int ASTINDEXES2 = 19;
    public const int ASTVAR = 30;
    public const int EOF = -1;
    public const int NONEQUAL = 82;
    public const int ASTINTEGER = 13;
    public const int TANH = 59;
    public const int ASTEQU = 5;
    public const int EXP = 53;
    public const int Comment = 99;
    public const int EEQUAL = 62;
    public const int SQR = 58;
    public const int GREATERTHANOREQUAL = 84;
    public const int GREATERTHAN = 87;
    public const int Double = 77;
    public const int ASTEQU1 = 35;
    public const int D = 104;
    public const int ASTEQU2 = 36;
    public const int E = 105;
    public const int F = 106;
    public const int ASTEQU0 = 34;
    public const int G = 107;
    public const int A = 101;
    public const int ASTEQU3 = 37;
    public const int B = 102;
    public const int C = 103;
    public const int L = 112;
    public const int M = 113;
    public const int N = 114;
    public const int NESTED_ML_COMMENT = 100;
    public const int ASTVARWI4 = 43;
    public const int O = 115;
    public const int ASTVARWI3 = 42;
    public const int H = 108;
    public const int ASTVARWI2 = 41;
    public const int I = 109;
    public const int ASTVARWI1 = 40;
    public const int J = 110;
    public const int ASTVARWI0 = 39;
    public const int NEWLINE2 = 89;
    public const int K = 111;
    public const int NEWLINE3 = 90;
    public const int U = 121;
    public const int T = 120;
    public const int W = 123;
    public const int WHITESPACE = 97;
    public const int POWER = 57;
    public const int V = 122;
    public const int Q = 117;
    public const int P = 116;
    public const int S = 119;
    public const int R = 118;
    public const int MULT = 78;
    public const int ASTVARWI = 38;
    public const int Y = 125;
    public const int ASTIDXELEMENTS1 = 46;
    public const int X = 124;
    public const int ASTIDXELEMENTS0 = 47;
    public const int Z = 126;
    public const int ABS = 52;
    public const int Ident = 88;
    public const int ASTEXPRESSION = 8;
    public const int OR = 50;
    public const int StringInQuotes = 72;
    public const int ASTSUM = 26;
    public const int ASTDEFINITION = 24;
    public const int DOLLAR = 76;
    public const int ASTFUNCTION = 12;
    public const int ASTEQUCODE = 7;
    public const int MAX = 55;
    public const int Exponent = 95;
    public const int R2 = 68;
    public const int R3 = 70;
    public const int AND = 49;
    public const int SUM = 48;
    public const int COMMA = 71;
    public const int R1 = 66;
    public const int EQUAL = 85;
    public const int ASTSIMPLEFUNCTION1 = 9;
    public const int ASTSIMPLEFUNCTION2 = 10;
    public const int LESSTHANOREQUAL = 83;
    public const int ASTSIMPLEFUNCTION3 = 11;
    public const int PLUS = 73;
    public const int ASTEND = 17;
    public const int DIGIT = 94;
    public const int DOT = 64;
    public const int ASTEXPRESSION2 = 22;
    public const int ASTIDXELEMENTS = 45;
    public const int ASTEXPRESSION3 = 23;
    public const int LESSTHAN = 86;
    public const int ASTVARIABLEWITHINDEXERETC = 31;
    public const int ASTIDX0 = 44;
    public const int ASTEXPRESSION1 = 21;
    public const int NEGATE = 4;
    public const int SAMEAS = 60;
    public const int MIN = 56;
    public const int MINUS = 75;
    public const int SEMI = 63;
    public const int L1 = 65;
    public const int L2 = 67;
    public const int L3 = 69;
    public const int ASTLEFTSIDE = 6;
    public const int NEWLINE = 98;
    public const int EQU = 93;
    public const int STARS = 81;
    public const int ASTDOUBLE = 14;
    public const int DIV = 79;
    public const int Integer = 74;
    public const int ASTDOT = 32;

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

    // $ANTLR start "COMMENT1"
    public void mCOMMENT1() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMENT1;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:279:9: ( ( '#' ) (~ ( NEWLINE2 | NEWLINE3 ) )* )
            // GAMS.g:279:30: ( '#' ) (~ ( NEWLINE2 | NEWLINE3 ) )*
            {
            	// GAMS.g:279:30: ( '#' )
            	// GAMS.g:279:31: '#'
            	{
            		Match('#'); 

            	}

            	// GAMS.g:279:36: (~ ( NEWLINE2 | NEWLINE3 ) )*
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
            			    // GAMS.g:279:37: ~ ( NEWLINE2 | NEWLINE3 )
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
    // $ANTLR end "COMMENT1"

    // $ANTLR start "COMMENT2"
    public void mCOMMENT2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMENT2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:280:9: ( ( '!!' ) (~ ( NEWLINE2 | NEWLINE3 ) )* )
            // GAMS.g:280:30: ( '!!' ) (~ ( NEWLINE2 | NEWLINE3 ) )*
            {
            	// GAMS.g:280:30: ( '!!' )
            	// GAMS.g:280:31: '!!'
            	{
            		Match("!!"); 


            	}

            	// GAMS.g:280:37: (~ ( NEWLINE2 | NEWLINE3 ) )*
            	do 
            	{
            	    int alt2 = 2;
            	    int LA2_0 = input.LA(1);

            	    if ( ((LA2_0 >= '\u0000' && LA2_0 <= '\t') || (LA2_0 >= '\u000B' && LA2_0 <= '\uFFFF')) )
            	    {
            	        alt2 = 1;
            	    }


            	    switch (alt2) 
            		{
            			case 1 :
            			    // GAMS.g:280:38: ~ ( NEWLINE2 | NEWLINE3 )
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
            			    goto loop2;
            	    }
            	} while (true);

            	loop2:
            		;	// Stops C# compiler whining that label 'loop2' has no statements

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

    // $ANTLR start "EQU"
    public void mEQU() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EQU;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:283:5: ( 'EQU' )
            // GAMS.g:283:7: 'EQU'
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
            // GAMS.g:284:5: ( 'SUM' )
            // GAMS.g:284:7: 'SUM'
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
            // GAMS.g:285:3: ( 'OR' )
            // GAMS.g:285:5: 'OR'
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
            // GAMS.g:286:4: ( 'AND' )
            // GAMS.g:286:6: 'AND'
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
            // GAMS.g:287:4: ( 'NOT' )
            // GAMS.g:287:6: 'NOT'
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
            // GAMS.g:288:4: ( 'ABS' )
            // GAMS.g:288:6: 'ABS'
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
            // GAMS.g:289:4: ( 'EXP' )
            // GAMS.g:289:6: 'EXP'
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
            // GAMS.g:290:4: ( 'LOG' )
            // GAMS.g:290:6: 'LOG'
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
            // GAMS.g:291:4: ( 'MAX' )
            // GAMS.g:291:6: 'MAX'
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
            // GAMS.g:292:4: ( 'MIN' )
            // GAMS.g:292:6: 'MIN'
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
            // GAMS.g:293:6: ( 'POWER' )
            // GAMS.g:293:8: 'POWER'
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
            // GAMS.g:294:4: ( 'SQR' )
            // GAMS.g:294:6: 'SQR'
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
            // GAMS.g:295:7: ( 'SAMEAS' )
            // GAMS.g:295:9: 'SAMEAS'
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
            // GAMS.g:296:5: ( 'TANH' )
            // GAMS.g:296:7: 'TANH'
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

    // $ANTLR start "DOT"
    public void mDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:298:9: ( '.' )
            // GAMS.g:298:11: '.'
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
            // GAMS.g:299:6: ( '+' )
            // GAMS.g:299:8: '+'
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
            // GAMS.g:300:7: ( '-' )
            // GAMS.g:300:9: '-'
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
            // GAMS.g:301:6: ( ';' )
            // GAMS.g:301:8: ';'
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
            // GAMS.g:302:6: ( '*' )
            // GAMS.g:302:8: '*'
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
            // GAMS.g:303:5: ( '/' )
            // GAMS.g:303:7: '/'
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
            // GAMS.g:304:5: ( '%' )
            // GAMS.g:304:7: '%'
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
            // GAMS.g:305:7: ( ',' )
            // GAMS.g:305:9: ','
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
            // GAMS.g:307:8: ( '=e=' | '=E=' )
            int alt3 = 2;
            int LA3_0 = input.LA(1);

            if ( (LA3_0 == '=') )
            {
                int LA3_1 = input.LA(2);

                if ( (LA3_1 == 'e') )
                {
                    alt3 = 1;
                }
                else if ( (LA3_1 == 'E') )
                {
                    alt3 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d3s1 =
                        new NoViableAltException("", 3, 1, input);

                    throw nvae_d3s1;
                }
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
                    // GAMS.g:307:10: '=e='
                    {
                    	Match("=e="); 


                    }
                    break;
                case 2 :
                    // GAMS.g:307:18: '=E='
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
            // GAMS.g:308:9: ( '<>' )
            // GAMS.g:308:11: '<>'
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
            // GAMS.g:309:16: ( '<=' )
            // GAMS.g:309:18: '<='
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
            // GAMS.g:310:19: ( '>=' )
            // GAMS.g:310:21: '>='
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
            // GAMS.g:311:7: ( '=' )
            // GAMS.g:311:9: '='
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
            // GAMS.g:312:9: ( '<' )
            // GAMS.g:312:11: '<'
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
            // GAMS.g:313:12: ( '>' )
            // GAMS.g:313:14: '>'
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

    // $ANTLR start "STARS"
    public void mSTARS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STARS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:315:10: ( '**' )
            // GAMS.g:315:17: '**'
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

    // $ANTLR start "DOUBLEDOT"
    public void mDOUBLEDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOUBLEDOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:317:11: ( '..' )
            // GAMS.g:317:13: '..'
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
            // GAMS.g:319:9: ( ( DIGIT )+ )
            // GAMS.g:319:11: ( DIGIT )+
            {
            	// GAMS.g:319:11: ( DIGIT )+
            	int cnt4 = 0;
            	do 
            	{
            	    int alt4 = 2;
            	    int LA4_0 = input.LA(1);

            	    if ( ((LA4_0 >= '0' && LA4_0 <= '9')) )
            	    {
            	        alt4 = 1;
            	    }


            	    switch (alt4) 
            		{
            			case 1 :
            			    // GAMS.g:319:12: DIGIT
            			    {
            			    	mDIGIT(); 

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
    // $ANTLR end "Integer"

    // $ANTLR start "Double"
    public void mDouble() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Double;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:322:5: ( ( '0' .. '9' )+ DOT ( '0' .. '9' )* ( Exponent )? | DOT ( '0' .. '9' )+ ( Exponent )? | ( '0' .. '9' )+ Exponent )
            int alt11 = 3;
            alt11 = dfa11.Predict(input);
            switch (alt11) 
            {
                case 1 :
                    // GAMS.g:323:8: ( '0' .. '9' )+ DOT ( '0' .. '9' )* ( Exponent )?
                    {
                    	// GAMS.g:323:8: ( '0' .. '9' )+
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
                    			    // GAMS.g:323:9: '0' .. '9'
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

                    	mDOT(); 
                    	// GAMS.g:323:26: ( '0' .. '9' )*
                    	do 
                    	{
                    	    int alt6 = 2;
                    	    int LA6_0 = input.LA(1);

                    	    if ( ((LA6_0 >= '0' && LA6_0 <= '9')) )
                    	    {
                    	        alt6 = 1;
                    	    }


                    	    switch (alt6) 
                    		{
                    			case 1 :
                    			    // GAMS.g:323:27: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop6;
                    	    }
                    	} while (true);

                    	loop6:
                    		;	// Stops C# compiler whining that label 'loop6' has no statements

                    	// GAMS.g:323:40: ( Exponent )?
                    	int alt7 = 2;
                    	int LA7_0 = input.LA(1);

                    	if ( (LA7_0 == 'E' || LA7_0 == 'e') )
                    	{
                    	    alt7 = 1;
                    	}
                    	switch (alt7) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:323:40: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // GAMS.g:324:9: DOT ( '0' .. '9' )+ ( Exponent )?
                    {
                    	mDOT(); 
                    	// GAMS.g:324:13: ( '0' .. '9' )+
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
                    			    // GAMS.g:324:15: '0' .. '9'
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

                    	// GAMS.g:324:29: ( Exponent )?
                    	int alt9 = 2;
                    	int LA9_0 = input.LA(1);

                    	if ( (LA9_0 == 'E' || LA9_0 == 'e') )
                    	{
                    	    alt9 = 1;
                    	}
                    	switch (alt9) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:324:29: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 3 :
                    // GAMS.g:325:8: ( '0' .. '9' )+ Exponent
                    {
                    	// GAMS.g:325:8: ( '0' .. '9' )+
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
                    			    // GAMS.g:325:9: '0' .. '9'
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
            // GAMS.g:328:7: ( ( LETTER | '_' ) ( DIGIT | LETTER | '_' )* )
            // GAMS.g:328:9: ( LETTER | '_' ) ( DIGIT | LETTER | '_' )*
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

            	// GAMS.g:328:22: ( DIGIT | LETTER | '_' )*
            	do 
            	{
            	    int alt12 = 2;
            	    int LA12_0 = input.LA(1);

            	    if ( ((LA12_0 >= '0' && LA12_0 <= '9') || (LA12_0 >= 'A' && LA12_0 <= 'Z') || LA12_0 == '_' || (LA12_0 >= 'a' && LA12_0 <= 'z')) )
            	    {
            	        alt12 = 1;
            	    }


            	    switch (alt12) 
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
            			    goto loop12;
            	    }
            	} while (true);

            	loop12:
            		;	// Stops C# compiler whining that label 'loop12' has no statements

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
            // GAMS.g:330:12: ( ( '\\t' | ' ' | '\\u000C' )+ )
            // GAMS.g:330:14: ( '\\t' | ' ' | '\\u000C' )+
            {
            	// GAMS.g:330:14: ( '\\t' | ' ' | '\\u000C' )+
            	int cnt13 = 0;
            	do 
            	{
            	    int alt13 = 2;
            	    int LA13_0 = input.LA(1);

            	    if ( (LA13_0 == '\t' || LA13_0 == '\f' || LA13_0 == ' ') )
            	    {
            	        alt13 = 1;
            	    }


            	    switch (alt13) 
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
            			    if ( cnt13 >= 1 ) goto loop13;
            		            EarlyExitException eee13 =
            		                new EarlyExitException(13, input);
            		            throw eee13;
            	    }
            	    cnt13++;
            	} while (true);

            	loop13:
            		;	// Stops C# compiler whining that label 'loop13' has no statements

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
            // GAMS.g:332:12: ( ( ( '\\r' )? '\\n' ) )
            // GAMS.g:332:16: ( ( '\\r' )? '\\n' )
            {
            	// GAMS.g:332:16: ( ( '\\r' )? '\\n' )
            	// GAMS.g:332:17: ( '\\r' )? '\\n'
            	{
            		// GAMS.g:332:17: ( '\\r' )?
            		int alt14 = 2;
            		int LA14_0 = input.LA(1);

            		if ( (LA14_0 == '\r') )
            		{
            		    alt14 = 1;
            		}
            		switch (alt14) 
            		{
            		    case 1 :
            		        // GAMS.g:332:18: '\\r'
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
            // GAMS.g:334:12: ( ( '()' | '//' ) (~ ( NEWLINE2 | NEWLINE3 ) )* )
            // GAMS.g:334:14: ( '()' | '//' ) (~ ( NEWLINE2 | NEWLINE3 ) )*
            {
            	// GAMS.g:334:14: ( '()' | '//' )
            	int alt15 = 2;
            	int LA15_0 = input.LA(1);

            	if ( (LA15_0 == '(') )
            	{
            	    alt15 = 1;
            	}
            	else if ( (LA15_0 == '/') )
            	{
            	    alt15 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d15s0 =
            	        new NoViableAltException("", 15, 0, input);

            	    throw nvae_d15s0;
            	}
            	switch (alt15) 
            	{
            	    case 1 :
            	        // GAMS.g:334:15: '()'
            	        {
            	        	Match("()"); 


            	        }
            	        break;
            	    case 2 :
            	        // GAMS.g:334:22: '//'
            	        {
            	        	Match("//"); 


            	        }
            	        break;

            	}

            	// GAMS.g:334:28: (~ ( NEWLINE2 | NEWLINE3 ) )*
            	do 
            	{
            	    int alt16 = 2;
            	    int LA16_0 = input.LA(1);

            	    if ( ((LA16_0 >= '\u0000' && LA16_0 <= '\t') || (LA16_0 >= '\u000B' && LA16_0 <= '\uFFFF')) )
            	    {
            	        alt16 = 1;
            	    }


            	    switch (alt16) 
            		{
            			case 1 :
            			    // GAMS.g:334:29: ~ ( NEWLINE2 | NEWLINE3 )
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
            			    goto loop16;
            	    }
            	} while (true);

            	loop16:
            		;	// Stops C# compiler whining that label 'loop16' has no statements

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
            // GAMS.g:338:5: ( '/*' ( options {greedy=false; } : NESTED_ML_COMMENT | . )* '*/' )
            // GAMS.g:338:9: '/*' ( options {greedy=false; } : NESTED_ML_COMMENT | . )* '*/'
            {
            	Match("/*"); 

            	// GAMS.g:339:9: ( options {greedy=false; } : NESTED_ML_COMMENT | . )*
            	do 
            	{
            	    int alt17 = 3;
            	    int LA17_0 = input.LA(1);

            	    if ( (LA17_0 == '*') )
            	    {
            	        int LA17_1 = input.LA(2);

            	        if ( (LA17_1 == '/') )
            	        {
            	            alt17 = 3;
            	        }
            	        else if ( ((LA17_1 >= '\u0000' && LA17_1 <= '.') || (LA17_1 >= '0' && LA17_1 <= '\uFFFF')) )
            	        {
            	            alt17 = 2;
            	        }


            	    }
            	    else if ( (LA17_0 == '/') )
            	    {
            	        int LA17_2 = input.LA(2);

            	        if ( (LA17_2 == '*') )
            	        {
            	            alt17 = 1;
            	        }
            	        else if ( ((LA17_2 >= '\u0000' && LA17_2 <= ')') || (LA17_2 >= '+' && LA17_2 <= '\uFFFF')) )
            	        {
            	            alt17 = 2;
            	        }


            	    }
            	    else if ( ((LA17_0 >= '\u0000' && LA17_0 <= ')') || (LA17_0 >= '+' && LA17_0 <= '.') || (LA17_0 >= '0' && LA17_0 <= '\uFFFF')) )
            	    {
            	        alt17 = 2;
            	    }


            	    switch (alt17) 
            		{
            			case 1 :
            			    // GAMS.g:339:36: NESTED_ML_COMMENT
            			    {
            			    	mNESTED_ML_COMMENT(); 

            			    }
            			    break;
            			case 2 :
            			    // GAMS.g:339:56: .
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

    // $ANTLR start "L1"
    public void mL1() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = L1;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // GAMS.g:343:4: ( '(' )
            // GAMS.g:343:6: '('
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
            // GAMS.g:344:4: ( ')' )
            // GAMS.g:344:6: ')'
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
            // GAMS.g:345:4: ( '[' )
            // GAMS.g:345:6: '['
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
            // GAMS.g:346:4: ( ']' )
            // GAMS.g:346:6: ']'
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
            // GAMS.g:347:4: ( '{' )
            // GAMS.g:347:6: '{'
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
            // GAMS.g:348:4: ( '}' )
            // GAMS.g:348:6: '}'
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
            // GAMS.g:350:7: ( '$' )
            // GAMS.g:350:9: '$'
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
            // GAMS.g:352:15: ( ( '\\'' (~ ( '\\'' ) )* '\\'' ) )
            // GAMS.g:352:29: ( '\\'' (~ ( '\\'' ) )* '\\'' )
            {
            	// GAMS.g:352:29: ( '\\'' (~ ( '\\'' ) )* '\\'' )
            	// GAMS.g:352:30: '\\'' (~ ( '\\'' ) )* '\\''
            	{
            		Match('\''); 
            		// GAMS.g:352:35: (~ ( '\\'' ) )*
            		do 
            		{
            		    int alt18 = 2;
            		    int LA18_0 = input.LA(1);

            		    if ( ((LA18_0 >= '\u0000' && LA18_0 <= '&') || (LA18_0 >= '(' && LA18_0 <= '\uFFFF')) )
            		    {
            		        alt18 = 1;
            		    }


            		    switch (alt18) 
            			{
            				case 1 :
            				    // GAMS.g:352:36: ~ ( '\\'' )
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
            				    goto loop18;
            		    }
            		} while (true);

            		loop18:
            			;	// Stops C# compiler whining that label 'loop18' has no statements

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

    // $ANTLR start "NEWLINE2"
    public void mNEWLINE2() // throws RecognitionException [2]
    {
    		try
    		{
            // GAMS.g:354:19: ( '\\n' )
            // GAMS.g:354:21: '\\n'
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
            // GAMS.g:355:19: ( '\\r\\n' )
            // GAMS.g:355:21: '\\r\\n'
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
            // GAMS.g:356:16: ( '0' .. '9' )
            // GAMS.g:356:18: '0' .. '9'
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
            // GAMS.g:357:16: ( 'a' .. 'z' | 'A' .. 'Z' )
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
            // GAMS.g:360:5: ( ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+ )
            // GAMS.g:360:9: ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+
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

            	// GAMS.g:360:23: ( '+' | '-' )?
            	int alt19 = 2;
            	int LA19_0 = input.LA(1);

            	if ( (LA19_0 == '+' || LA19_0 == '-') )
            	{
            	    alt19 = 1;
            	}
            	switch (alt19) 
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

            	// GAMS.g:360:38: ( '0' .. '9' )+
            	int cnt20 = 0;
            	do 
            	{
            	    int alt20 = 2;
            	    int LA20_0 = input.LA(1);

            	    if ( ((LA20_0 >= '0' && LA20_0 <= '9')) )
            	    {
            	        alt20 = 1;
            	    }


            	    switch (alt20) 
            		{
            			case 1 :
            			    // GAMS.g:360:40: '0' .. '9'
            			    {
            			    	MatchRange('0','9'); 

            			    }
            			    break;

            			default:
            			    if ( cnt20 >= 1 ) goto loop20;
            		            EarlyExitException eee20 =
            		                new EarlyExitException(20, input);
            		            throw eee20;
            	    }
            	    cnt20++;
            	} while (true);

            	loop20:
            		;	// Stops C# compiler whining that label 'loop20' has no statements


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
            // GAMS.g:363:11: ( ( 'a' | 'A' ) )
            // GAMS.g:363:12: ( 'a' | 'A' )
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
            // GAMS.g:364:11: ( ( 'b' | 'B' ) )
            // GAMS.g:364:12: ( 'b' | 'B' )
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
            // GAMS.g:365:11: ( ( 'c' | 'C' ) )
            // GAMS.g:365:12: ( 'c' | 'C' )
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
            // GAMS.g:366:11: ( ( 'd' | 'D' ) )
            // GAMS.g:366:12: ( 'd' | 'D' )
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
            // GAMS.g:367:11: ( ( 'e' | 'E' ) )
            // GAMS.g:367:12: ( 'e' | 'E' )
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
            // GAMS.g:368:11: ( ( 'f' | 'F' ) )
            // GAMS.g:368:12: ( 'f' | 'F' )
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
            // GAMS.g:369:11: ( ( 'g' | 'G' ) )
            // GAMS.g:369:12: ( 'g' | 'G' )
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
            // GAMS.g:370:11: ( ( 'h' | 'H' ) )
            // GAMS.g:370:12: ( 'h' | 'H' )
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
            // GAMS.g:371:11: ( ( 'i' | 'I' ) )
            // GAMS.g:371:12: ( 'i' | 'I' )
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
            // GAMS.g:372:11: ( ( 'j' | 'J' ) )
            // GAMS.g:372:12: ( 'j' | 'J' )
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
            // GAMS.g:373:11: ( ( 'k' | 'K' ) )
            // GAMS.g:373:12: ( 'k' | 'K' )
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
            // GAMS.g:374:11: ( ( 'l' | 'L' ) )
            // GAMS.g:374:12: ( 'l' | 'L' )
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
            // GAMS.g:375:11: ( ( 'm' | 'M' ) )
            // GAMS.g:375:12: ( 'm' | 'M' )
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
            // GAMS.g:376:11: ( ( 'n' | 'N' ) )
            // GAMS.g:376:12: ( 'n' | 'N' )
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
            // GAMS.g:377:11: ( ( 'o' | 'O' ) )
            // GAMS.g:377:12: ( 'o' | 'O' )
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
            // GAMS.g:378:11: ( ( 'p' | 'P' ) )
            // GAMS.g:378:12: ( 'p' | 'P' )
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
            // GAMS.g:379:11: ( ( 'q' | 'Q' ) )
            // GAMS.g:379:12: ( 'q' | 'Q' )
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
            // GAMS.g:380:11: ( ( 'r' | 'R' ) )
            // GAMS.g:380:12: ( 'r' | 'R' )
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
            // GAMS.g:381:11: ( ( 's' | 'S' ) )
            // GAMS.g:381:12: ( 's' | 'S' )
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
            // GAMS.g:382:11: ( ( 't' | 'T' ) )
            // GAMS.g:382:12: ( 't' | 'T' )
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
            // GAMS.g:383:11: ( ( 'u' | 'U' ) )
            // GAMS.g:383:12: ( 'u' | 'U' )
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
            // GAMS.g:384:11: ( ( 'v' | 'V' ) )
            // GAMS.g:384:12: ( 'v' | 'V' )
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
            // GAMS.g:385:11: ( ( 'w' | 'W' ) )
            // GAMS.g:385:12: ( 'w' | 'W' )
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
            // GAMS.g:386:11: ( ( 'x' | 'X' ) )
            // GAMS.g:386:12: ( 'x' | 'X' )
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
            // GAMS.g:387:11: ( ( 'y' | 'Y' ) )
            // GAMS.g:387:12: ( 'y' | 'Y' )
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
            // GAMS.g:388:11: ( ( 'z' | 'Z' ) )
            // GAMS.g:388:12: ( 'z' | 'Z' )
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
        // GAMS.g:1:8: ( COMMENT1 | COMMENT2 | EQU | SUM | OR | AND | NOT | ABS | EXP | LOG | MAX | MIN | POWER | SQR | SAMEAS | TANH | DOT | PLUS | MINUS | SEMI | MULT | DIV | MOD | COMMA | EEQUAL | NONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL | EQUAL | LESSTHAN | GREATERTHAN | STARS | DOUBLEDOT | Integer | Double | Ident | WHITESPACE | NEWLINE | Comment | NESTED_ML_COMMENT | L1 | R1 | L2 | R2 | L3 | R3 | DOLLAR | StringInQuotes )
        int alt21 = 48;
        alt21 = dfa21.Predict(input);
        switch (alt21) 
        {
            case 1 :
                // GAMS.g:1:10: COMMENT1
                {
                	mCOMMENT1(); 

                }
                break;
            case 2 :
                // GAMS.g:1:19: COMMENT2
                {
                	mCOMMENT2(); 

                }
                break;
            case 3 :
                // GAMS.g:1:28: EQU
                {
                	mEQU(); 

                }
                break;
            case 4 :
                // GAMS.g:1:32: SUM
                {
                	mSUM(); 

                }
                break;
            case 5 :
                // GAMS.g:1:36: OR
                {
                	mOR(); 

                }
                break;
            case 6 :
                // GAMS.g:1:39: AND
                {
                	mAND(); 

                }
                break;
            case 7 :
                // GAMS.g:1:43: NOT
                {
                	mNOT(); 

                }
                break;
            case 8 :
                // GAMS.g:1:47: ABS
                {
                	mABS(); 

                }
                break;
            case 9 :
                // GAMS.g:1:51: EXP
                {
                	mEXP(); 

                }
                break;
            case 10 :
                // GAMS.g:1:55: LOG
                {
                	mLOG(); 

                }
                break;
            case 11 :
                // GAMS.g:1:59: MAX
                {
                	mMAX(); 

                }
                break;
            case 12 :
                // GAMS.g:1:63: MIN
                {
                	mMIN(); 

                }
                break;
            case 13 :
                // GAMS.g:1:67: POWER
                {
                	mPOWER(); 

                }
                break;
            case 14 :
                // GAMS.g:1:73: SQR
                {
                	mSQR(); 

                }
                break;
            case 15 :
                // GAMS.g:1:77: SAMEAS
                {
                	mSAMEAS(); 

                }
                break;
            case 16 :
                // GAMS.g:1:84: TANH
                {
                	mTANH(); 

                }
                break;
            case 17 :
                // GAMS.g:1:89: DOT
                {
                	mDOT(); 

                }
                break;
            case 18 :
                // GAMS.g:1:93: PLUS
                {
                	mPLUS(); 

                }
                break;
            case 19 :
                // GAMS.g:1:98: MINUS
                {
                	mMINUS(); 

                }
                break;
            case 20 :
                // GAMS.g:1:104: SEMI
                {
                	mSEMI(); 

                }
                break;
            case 21 :
                // GAMS.g:1:109: MULT
                {
                	mMULT(); 

                }
                break;
            case 22 :
                // GAMS.g:1:114: DIV
                {
                	mDIV(); 

                }
                break;
            case 23 :
                // GAMS.g:1:118: MOD
                {
                	mMOD(); 

                }
                break;
            case 24 :
                // GAMS.g:1:122: COMMA
                {
                	mCOMMA(); 

                }
                break;
            case 25 :
                // GAMS.g:1:128: EEQUAL
                {
                	mEEQUAL(); 

                }
                break;
            case 26 :
                // GAMS.g:1:135: NONEQUAL
                {
                	mNONEQUAL(); 

                }
                break;
            case 27 :
                // GAMS.g:1:144: LESSTHANOREQUAL
                {
                	mLESSTHANOREQUAL(); 

                }
                break;
            case 28 :
                // GAMS.g:1:160: GREATERTHANOREQUAL
                {
                	mGREATERTHANOREQUAL(); 

                }
                break;
            case 29 :
                // GAMS.g:1:179: EQUAL
                {
                	mEQUAL(); 

                }
                break;
            case 30 :
                // GAMS.g:1:185: LESSTHAN
                {
                	mLESSTHAN(); 

                }
                break;
            case 31 :
                // GAMS.g:1:194: GREATERTHAN
                {
                	mGREATERTHAN(); 

                }
                break;
            case 32 :
                // GAMS.g:1:206: STARS
                {
                	mSTARS(); 

                }
                break;
            case 33 :
                // GAMS.g:1:212: DOUBLEDOT
                {
                	mDOUBLEDOT(); 

                }
                break;
            case 34 :
                // GAMS.g:1:222: Integer
                {
                	mInteger(); 

                }
                break;
            case 35 :
                // GAMS.g:1:230: Double
                {
                	mDouble(); 

                }
                break;
            case 36 :
                // GAMS.g:1:237: Ident
                {
                	mIdent(); 

                }
                break;
            case 37 :
                // GAMS.g:1:243: WHITESPACE
                {
                	mWHITESPACE(); 

                }
                break;
            case 38 :
                // GAMS.g:1:254: NEWLINE
                {
                	mNEWLINE(); 

                }
                break;
            case 39 :
                // GAMS.g:1:262: Comment
                {
                	mComment(); 

                }
                break;
            case 40 :
                // GAMS.g:1:270: NESTED_ML_COMMENT
                {
                	mNESTED_ML_COMMENT(); 

                }
                break;
            case 41 :
                // GAMS.g:1:288: L1
                {
                	mL1(); 

                }
                break;
            case 42 :
                // GAMS.g:1:291: R1
                {
                	mR1(); 

                }
                break;
            case 43 :
                // GAMS.g:1:294: L2
                {
                	mL2(); 

                }
                break;
            case 44 :
                // GAMS.g:1:297: R2
                {
                	mR2(); 

                }
                break;
            case 45 :
                // GAMS.g:1:300: L3
                {
                	mL3(); 

                }
                break;
            case 46 :
                // GAMS.g:1:303: R3
                {
                	mR3(); 

                }
                break;
            case 47 :
                // GAMS.g:1:306: DOLLAR
                {
                	mDOLLAR(); 

                }
                break;
            case 48 :
                // GAMS.g:1:313: StringInQuotes
                {
                	mStringInQuotes(); 

                }
                break;

        }

    }


    protected DFA11 dfa11;
    protected DFA21 dfa21;
	private void InitializeCyclicDFAs()
	{
	    this.dfa11 = new DFA11(this);
	    this.dfa21 = new DFA21(this);


	}

    const string DFA11_eotS =
        "\x05\uffff";
    const string DFA11_eofS =
        "\x05\uffff";
    const string DFA11_minS =
        "\x02\x2e\x03\uffff";
    const string DFA11_maxS =
        "\x01\x39\x01\x65\x03\uffff";
    const string DFA11_acceptS =
        "\x02\uffff\x01\x02\x01\x03\x01\x01";
    const string DFA11_specialS =
        "\x05\uffff}>";
    static readonly string[] DFA11_transitionS = {
            "\x01\x02\x01\uffff\x0a\x01",
            "\x01\x04\x01\uffff\x0a\x01\x0b\uffff\x01\x03\x1f\uffff\x01"+
            "\x03",
            "",
            "",
            ""
    };

    static readonly short[] DFA11_eot = DFA.UnpackEncodedString(DFA11_eotS);
    static readonly short[] DFA11_eof = DFA.UnpackEncodedString(DFA11_eofS);
    static readonly char[] DFA11_min = DFA.UnpackEncodedStringToUnsignedChars(DFA11_minS);
    static readonly char[] DFA11_max = DFA.UnpackEncodedStringToUnsignedChars(DFA11_maxS);
    static readonly short[] DFA11_accept = DFA.UnpackEncodedString(DFA11_acceptS);
    static readonly short[] DFA11_special = DFA.UnpackEncodedString(DFA11_specialS);
    static readonly short[][] DFA11_transition = DFA.UnpackEncodedStringArray(DFA11_transitionS);

    protected class DFA11 : DFA
    {
        public DFA11(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 11;
            this.eot = DFA11_eot;
            this.eof = DFA11_eof;
            this.min = DFA11_min;
            this.max = DFA11_max;
            this.accept = DFA11_accept;
            this.special = DFA11_special;
            this.transition = DFA11_transition;

        }

        override public string Description
        {
            get { return "321:1: Double : ( ( '0' .. '9' )+ DOT ( '0' .. '9' )* ( Exponent )? | DOT ( '0' .. '9' )+ ( Exponent )? | ( '0' .. '9' )+ Exponent );"; }
        }

    }

    const string DFA21_eotS =
        "\x03\uffff\x09\x18\x01\x32\x03\uffff\x01\x35\x01\x38\x02\uffff"+
        "\x01\x3a\x01\x3d\x01\x3f\x01\x40\x03\uffff\x01\x41\x07\uffff\x05"+
        "\x18\x01\x47\x08\x18\x11\uffff\x01\x50\x01\x51\x01\x52\x01\x53\x01"+
        "\x18\x01\uffff\x01\x55\x01\x56\x01\x57\x01\x58\x01\x59\x01\x5a\x02"+
        "\x18\x04\uffff\x01\x18\x06\uffff\x01\x18\x01\x5f\x01\x18\x01\x61"+
        "\x01\uffff\x01\x62\x02\uffff";
    const string DFA21_eofS =
        "\x63\uffff";
    const string DFA21_minS =
        "\x01\x09\x02\uffff\x01\x51\x01\x41\x01\x52\x01\x42\x02\x4f\x01"+
        "\x41\x01\x4f\x01\x41\x01\x2e\x03\uffff\x02\x2a\x02\uffff\x01\x45"+
        "\x02\x3d\x01\x2e\x03\uffff\x01\x29\x07\uffff\x01\x55\x01\x50\x01"+
        "\x4d\x01\x52\x01\x4d\x01\x30\x01\x44\x01\x53\x01\x54\x01\x47\x01"+
        "\x58\x01\x4e\x01\x57\x01\x4e\x11\uffff\x04\x30\x01\x45\x01\uffff"+
        "\x06\x30\x01\x45\x01\x48\x04\uffff\x01\x41\x06\uffff\x01\x52\x01"+
        "\x30\x01\x53\x01\x30\x01\uffff\x01\x30\x02\uffff";
    const string DFA21_maxS =
        "\x01\x7d\x02\uffff\x01\x58\x01\x55\x01\x52\x01\x4e\x02\x4f\x01"+
        "\x49\x01\x4f\x01\x41\x01\x39\x03\uffff\x01\x2a\x01\x2f\x02\uffff"+
        "\x01\x65\x01\x3e\x01\x3d\x01\x65\x03\uffff\x01\x29\x07\uffff\x01"+
        "\x55\x01\x50\x01\x4d\x01\x52\x01\x4d\x01\x7a\x01\x44\x01\x53\x01"+
        "\x54\x01\x47\x01\x58\x01\x4e\x01\x57\x01\x4e\x11\uffff\x04\x7a\x01"+
        "\x45\x01\uffff\x06\x7a\x01\x45\x01\x48\x04\uffff\x01\x41\x06\uffff"+
        "\x01\x52\x01\x7a\x01\x53\x01\x7a\x01\uffff\x01\x7a\x02\uffff";
    const string DFA21_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x0a\uffff\x01\x12\x01\x13\x01\x14\x02"+
        "\uffff\x01\x17\x01\x18\x04\uffff\x01\x24\x01\x25\x01\x26\x01\uffff"+
        "\x01\x2a\x01\x2b\x01\x2c\x01\x2d\x01\x2e\x01\x2f\x01\x30\x0e\uffff"+
        "\x01\x21\x01\x11\x01\x23\x01\x20\x01\x15\x01\x27\x01\x28\x01\x16"+
        "\x01\x19\x01\x1d\x01\x1a\x01\x1b\x01\x1e\x01\x1c\x01\x1f\x01\x22"+
        "\x01\x29\x05\uffff\x01\x05\x08\uffff\x01\x03\x01\x09\x01\x04\x01"+
        "\x0e\x01\uffff\x01\x06\x01\x08\x01\x07\x01\x0a\x01\x0b\x01\x0c\x04"+
        "\uffff\x01\x10\x01\uffff\x01\x0d\x01\x0f";
    const string DFA21_specialS =
        "\x63\uffff}>";
    static readonly string[] DFA21_transitionS = {
            "\x01\x19\x01\x1a\x01\uffff\x01\x19\x01\x1a\x12\uffff\x01\x19"+
            "\x01\x02\x01\uffff\x01\x01\x01\x21\x01\x12\x01\uffff\x01\x22"+
            "\x01\x1b\x01\x1c\x01\x10\x01\x0d\x01\x13\x01\x0e\x01\x0c\x01"+
            "\x11\x0a\x17\x01\uffff\x01\x0f\x01\x15\x01\x14\x01\x16\x02\uffff"+
            "\x01\x06\x03\x18\x01\x03\x06\x18\x01\x08\x01\x09\x01\x07\x01"+
            "\x05\x01\x0a\x02\x18\x01\x04\x01\x0b\x06\x18\x01\x1d\x01\uffff"+
            "\x01\x1e\x01\uffff\x01\x18\x01\uffff\x1a\x18\x01\x1f\x01\uffff"+
            "\x01\x20",
            "",
            "",
            "\x01\x23\x06\uffff\x01\x24",
            "\x01\x27\x0f\uffff\x01\x26\x03\uffff\x01\x25",
            "\x01\x28",
            "\x01\x2a\x0b\uffff\x01\x29",
            "\x01\x2b",
            "\x01\x2c",
            "\x01\x2d\x07\uffff\x01\x2e",
            "\x01\x2f",
            "\x01\x30",
            "\x01\x31\x01\uffff\x0a\x33",
            "",
            "",
            "",
            "\x01\x34",
            "\x01\x37\x04\uffff\x01\x36",
            "",
            "",
            "\x01\x39\x1f\uffff\x01\x39",
            "\x01\x3c\x01\x3b",
            "\x01\x3e",
            "\x01\x33\x01\uffff\x0a\x17\x0b\uffff\x01\x33\x1f\uffff\x01"+
            "\x33",
            "",
            "",
            "",
            "\x01\x36",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x42",
            "\x01\x43",
            "\x01\x44",
            "\x01\x45",
            "\x01\x46",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "\x01\x48",
            "\x01\x49",
            "\x01\x4a",
            "\x01\x4b",
            "\x01\x4c",
            "\x01\x4d",
            "\x01\x4e",
            "\x01\x4f",
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
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "\x01\x54",
            "",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "\x01\x5b",
            "\x01\x5c",
            "",
            "",
            "",
            "",
            "\x01\x5d",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x5e",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "\x01\x60",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
            "",
            "\x0a\x18\x07\uffff\x1a\x18\x04\uffff\x01\x18\x01\uffff\x1a"+
            "\x18",
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
            get { return "1:1: Tokens : ( COMMENT1 | COMMENT2 | EQU | SUM | OR | AND | NOT | ABS | EXP | LOG | MAX | MIN | POWER | SQR | SAMEAS | TANH | DOT | PLUS | MINUS | SEMI | MULT | DIV | MOD | COMMA | EEQUAL | NONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL | EQUAL | LESSTHAN | GREATERTHAN | STARS | DOUBLEDOT | Integer | Double | Ident | WHITESPACE | NEWLINE | Comment | NESTED_ML_COMMENT | L1 | R1 | L2 | R2 | L3 | R3 | DOLLAR | StringInQuotes );"; }
        }

    }

 
    
}
}