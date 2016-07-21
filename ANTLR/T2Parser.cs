// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 T2.g 2016-07-21 17:41:43

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;

using IDictionary	= System.Collections.IDictionary;
using Hashtable 	= System.Collections.Hashtable;

using Antlr.Runtime.Tree;

namespace  Gekko 
{
public partial class T2Parser : Parser
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"ASTNEGATE", 
		"ASTSTAR", 
		"ASTSTARS", 
		"ASTIDENT", 
		"ASTPLUS", 
		"ASTMINUS", 
		"ASTADD", 
		"ASTMULTIPLY", 
		"ASTCOMMAND", 
		"ASTCOMMAND1", 
		"ASTCOMMAND2", 
		"ASTCOMMAND3", 
		"ASTCOMPARECOMMAND", 
		"ASTSERIES", 
		"ASTPAREN", 
		"ASTANGLE", 
		"ASTBRACKET", 
		"ASTCURLY", 
		"ASTLIST", 
		"FIX", 
		"GENR", 
		"LIST", 
		"SKIP", 
		"UPD", 
		"TIME", 
		"AST1", 
		"COMMENT", 
		"EOL", 
		"SEMICOLON", 
		"WHITESPACE", 
		"LEFTANGLE", 
		"RIGHTANGLE", 
		"LEFTPAREN", 
		"RIGHTPAREN", 
		"LEFTBRACKET", 
		"RIGHTBRACKET", 
		"LEFTCURLY", 
		"RIGHTCURLY", 
		"StringInQuotes", 
		"StringInQuotes2", 
		"Integer", 
		"Double", 
		"DigitsEDigits", 
		"DateDef", 
		"IdentStartingWithInt", 
		"Ident", 
		"TILDE", 
		"AND", 
		"AT", 
		"HAT", 
		"COLON", 
		"DOT", 
		"HASH", 
		"PERCENT", 
		"DOLLAR", 
		"STAR", 
		"STARS", 
		"VERTICALBAR", 
		"PLUS", 
		"MINUS", 
		"DIV", 
		"EQUAL", 
		"BACKSLASH", 
		"QUESTION", 
		"COMMA", 
		"ANYTHING", 
		"NEWLINE2", 
		"NEWLINE3", 
		"DIGIT", 
		"LETTER", 
		"E_", 
		"Exponent", 
		"A_", 
		"Q_", 
		"M_", 
		"B_", 
		"C_", 
		"D_", 
		"F_", 
		"G_", 
		"H_", 
		"I_", 
		"J_", 
		"K_", 
		"L_", 
		"N_", 
		"O_", 
		"P_", 
		"R_", 
		"S_", 
		"T_", 
		"U_", 
		"V_", 
		"W_", 
		"X_", 
		"Y_", 
		"Z_"
    };

    public const int D_ = 81;
    public const int FIX = 23;
    public const int STAR = 59;
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
    public const int ASTSTARS = 6;
    public const int F_ = 82;
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
    public const int SKIP = 26;
    public const int N_ = 89;
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
    public const int P_ = 91;
    public const int ASTCOMMAND3 = 15;
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
    public const int STARS = 60;
    public const int K_ = 87;
    public const int RIGHTANGLE = 35;
    public const int DIV = 64;
    public const int ASTIDENT = 7;
    public const int Integer = 44;
    public const int R_ = 92;

    // delegates
    // delegators



        public T2Parser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public T2Parser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();
            this.state.ruleMemo = new Hashtable[99+1];
             
             
        }
        
    protected ITreeAdaptor adaptor = new CommonTreeAdaptor();

    public ITreeAdaptor TreeAdaptor
    {
        get { return this.adaptor; }
        set {
    	this.adaptor = value;
    	}
    }

    override public string[] TokenNames {
		get { return T2Parser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "T2.g"; }
    }



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


                                  

    public class expr_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expr"
    // T2.g:150:1: expr : ( expr2 )* ( n )? EOF ;
    public T2Parser.expr_return expr() // throws RecognitionException [1]
    {   
        T2Parser.expr_return retval = new T2Parser.expr_return();
        retval.Start = input.LT(1);
        int expr_StartIndex = input.Index();
        object root_0 = null;

        IToken EOF3 = null;
        T2Parser.expr2_return expr21 = default(T2Parser.expr2_return);

        T2Parser.n_return n2 = default(T2Parser.n_return);


        object EOF3_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 1) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:150:12: ( ( expr2 )* ( n )? EOF )
            // T2.g:150:14: ( expr2 )* ( n )? EOF
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// T2.g:150:14: ( expr2 )*
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( (LA1_0 == WHITESPACE) )
            	    {
            	        int LA1_1 = input.LA(2);

            	        if ( ((LA1_1 >= FIX && LA1_1 <= TIME) || (LA1_1 >= COMMENT && LA1_1 <= EOL) || LA1_1 == Ident) )
            	        {
            	            alt1 = 1;
            	        }


            	    }
            	    else if ( ((LA1_0 >= FIX && LA1_0 <= TIME) || (LA1_0 >= COMMENT && LA1_0 <= EOL) || LA1_0 == Ident) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // T2.g:0:0: expr2
            			    {
            			    	PushFollow(FOLLOW_expr2_in_expr417);
            			    	expr21 = expr2();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expr21.Tree);

            			    }
            			    break;

            			default:
            			    goto loop1;
            	    }
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements

            	// T2.g:150:21: ( n )?
            	int alt2 = 2;
            	int LA2_0 = input.LA(1);

            	if ( (LA2_0 == WHITESPACE) )
            	{
            	    alt2 = 1;
            	}
            	switch (alt2) 
            	{
            	    case 1 :
            	        // T2.g:0:0: n
            	        {
            	        	PushFollow(FOLLOW_n_in_expr420);
            	        	n2 = n();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n2.Tree);

            	        }
            	        break;

            	}

            	EOF3=(IToken)Match(input,EOF,FOLLOW_EOF_in_expr423); if (state.failed) return retval;
            	if ( state.backtracking == 0 )
            	{EOF3_tree = (object)adaptor.Create(EOF3);
            		adaptor.AddChild(root_0, EOF3_tree);
            	}

            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 1, expr_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "expr"

    public class expr2_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expr2"
    // T2.g:152:1: expr2 : ( command semi | ( n )? COMMENT ( n )? EOL | ( n )? EOL );
    public T2Parser.expr2_return expr2() // throws RecognitionException [1]
    {   
        T2Parser.expr2_return retval = new T2Parser.expr2_return();
        retval.Start = input.LT(1);
        int expr2_StartIndex = input.Index();
        object root_0 = null;

        IToken COMMENT7 = null;
        IToken EOL9 = null;
        IToken EOL11 = null;
        T2Parser.command_return command4 = default(T2Parser.command_return);

        T2Parser.semi_return semi5 = default(T2Parser.semi_return);

        T2Parser.n_return n6 = default(T2Parser.n_return);

        T2Parser.n_return n8 = default(T2Parser.n_return);

        T2Parser.n_return n10 = default(T2Parser.n_return);


        object COMMENT7_tree=null;
        object EOL9_tree=null;
        object EOL11_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 2) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:152:13: ( command semi | ( n )? COMMENT ( n )? EOL | ( n )? EOL )
            int alt6 = 3;
            switch ( input.LA(1) ) 
            {
            case WHITESPACE:
            	{
                switch ( input.LA(2) ) 
                {
                case EOL:
                	{
                    alt6 = 3;
                    }
                    break;
                case COMMENT:
                	{
                    alt6 = 2;
                    }
                    break;
                case FIX:
                case GENR:
                case LIST:
                case SKIP:
                case UPD:
                case TIME:
                case Ident:
                	{
                    alt6 = 1;
                    }
                    break;
                	default:
                	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                	    NoViableAltException nvae_d6s1 =
                	        new NoViableAltException("", 6, 1, input);

                	    throw nvae_d6s1;
                }

                }
                break;
            case FIX:
            case GENR:
            case LIST:
            case SKIP:
            case UPD:
            case TIME:
            case Ident:
            	{
                alt6 = 1;
                }
                break;
            case COMMENT:
            	{
                alt6 = 2;
                }
                break;
            case EOL:
            	{
                alt6 = 3;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d6s0 =
            	        new NoViableAltException("", 6, 0, input);

            	    throw nvae_d6s0;
            }

            switch (alt6) 
            {
                case 1 :
                    // T2.g:152:15: command semi
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_command_in_expr2437);
                    	command4 = command();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, command4.Tree);
                    	PushFollow(FOLLOW_semi_in_expr2439);
                    	semi5 = semi();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, semi5.Tree);

                    }
                    break;
                case 2 :
                    // T2.g:153:11: ( n )? COMMENT ( n )? EOL
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:153:11: ( n )?
                    	int alt3 = 2;
                    	int LA3_0 = input.LA(1);

                    	if ( (LA3_0 == WHITESPACE) )
                    	{
                    	    alt3 = 1;
                    	}
                    	switch (alt3) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_expr2466);
                    	        	n6 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n6.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMENT7=(IToken)Match(input,COMMENT,FOLLOW_COMMENT_in_expr2469); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{COMMENT7_tree = (object)adaptor.Create(COMMENT7);
                    		adaptor.AddChild(root_0, COMMENT7_tree);
                    	}
                    	// T2.g:153:22: ( n )?
                    	int alt4 = 2;
                    	int LA4_0 = input.LA(1);

                    	if ( (LA4_0 == WHITESPACE) )
                    	{
                    	    alt4 = 1;
                    	}
                    	switch (alt4) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_expr2471);
                    	        	n8 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n8.Tree);

                    	        }
                    	        break;

                    	}

                    	EOL9=(IToken)Match(input,EOL,FOLLOW_EOL_in_expr2474); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{EOL9_tree = (object)adaptor.Create(EOL9);
                    		adaptor.AddChild(root_0, EOL9_tree);
                    	}

                    }
                    break;
                case 3 :
                    // T2.g:154:11: ( n )? EOL
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:154:11: ( n )?
                    	int alt5 = 2;
                    	int LA5_0 = input.LA(1);

                    	if ( (LA5_0 == WHITESPACE) )
                    	{
                    	    alt5 = 1;
                    	}
                    	switch (alt5) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_expr2486);
                    	        	n10 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n10.Tree);

                    	        }
                    	        break;

                    	}

                    	EOL11=(IToken)Match(input,EOL,FOLLOW_EOL_in_expr2489); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{EOL11_tree = (object)adaptor.Create(EOL11);
                    		adaptor.AddChild(root_0, EOL11_tree);
                    	}

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 2, expr2_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "expr2"

    public class command_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "command"
    // T2.g:157:1: command : commandName ( commandOptions )? ( commandRest )? -> ^( ASTCOMMAND ^( ASTCOMMAND1 commandName ) ^( ASTCOMMAND2 ( commandOptions )? ) ^( ASTCOMMAND3 ( commandRest )? ) ) ;
    public T2Parser.command_return command() // throws RecognitionException [1]
    {   
        T2Parser.command_return retval = new T2Parser.command_return();
        retval.Start = input.LT(1);
        int command_StartIndex = input.Index();
        object root_0 = null;

        T2Parser.commandName_return commandName12 = default(T2Parser.commandName_return);

        T2Parser.commandOptions_return commandOptions13 = default(T2Parser.commandOptions_return);

        T2Parser.commandRest_return commandRest14 = default(T2Parser.commandRest_return);


        RewriteRuleSubtreeStream stream_commandOptions = new RewriteRuleSubtreeStream(adaptor,"rule commandOptions");
        RewriteRuleSubtreeStream stream_commandName = new RewriteRuleSubtreeStream(adaptor,"rule commandName");
        RewriteRuleSubtreeStream stream_commandRest = new RewriteRuleSubtreeStream(adaptor,"rule commandRest");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 3) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:157:27: ( commandName ( commandOptions )? ( commandRest )? -> ^( ASTCOMMAND ^( ASTCOMMAND1 commandName ) ^( ASTCOMMAND2 ( commandOptions )? ) ^( ASTCOMMAND3 ( commandRest )? ) ) )
            // T2.g:157:29: commandName ( commandOptions )? ( commandRest )?
            {
            	PushFollow(FOLLOW_commandName_in_command528);
            	commandName12 = commandName();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_commandName.Add(commandName12.Tree);
            	// T2.g:157:41: ( commandOptions )?
            	int alt7 = 2;
            	alt7 = dfa7.Predict(input);
            	switch (alt7) 
            	{
            	    case 1 :
            	        // T2.g:0:0: commandOptions
            	        {
            	        	PushFollow(FOLLOW_commandOptions_in_command530);
            	        	commandOptions13 = commandOptions();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_commandOptions.Add(commandOptions13.Tree);

            	        }
            	        break;

            	}

            	// T2.g:157:57: ( commandRest )?
            	int alt8 = 2;
            	switch ( input.LA(1) ) 
            	{
            	    case WHITESPACE:
            	    	{
            	        int LA8_1 = input.LA(2);

            	        if ( (synpred9_T2()) )
            	        {
            	            alt8 = 1;
            	        }
            	        }
            	        break;
            	    case FIX:
            	    case GENR:
            	    case LIST:
            	    case SKIP:
            	    case UPD:
            	    case TIME:
            	    case EOL:
            	    case LEFTANGLE:
            	    case RIGHTANGLE:
            	    case LEFTPAREN:
            	    case LEFTBRACKET:
            	    case LEFTCURLY:
            	    case StringInQuotes:
            	    case StringInQuotes2:
            	    case Integer:
            	    case Double:
            	    case DigitsEDigits:
            	    case DateDef:
            	    case IdentStartingWithInt:
            	    case Ident:
            	    case TILDE:
            	    case AND:
            	    case AT:
            	    case HAT:
            	    case COLON:
            	    case DOT:
            	    case HASH:
            	    case PERCENT:
            	    case DOLLAR:
            	    case STAR:
            	    case STARS:
            	    case VERTICALBAR:
            	    case PLUS:
            	    case MINUS:
            	    case DIV:
            	    case EQUAL:
            	    case BACKSLASH:
            	    case QUESTION:
            	    case COMMA:
            	    case ANYTHING:
            	    	{
            	        alt8 = 1;
            	        }
            	        break;
            	    case SEMICOLON:
            	    	{
            	        int LA8_3 = input.LA(2);

            	        if ( (synpred9_T2()) )
            	        {
            	            alt8 = 1;
            	        }
            	        }
            	        break;
            	}

            	switch (alt8) 
            	{
            	    case 1 :
            	        // T2.g:0:0: commandRest
            	        {
            	        	PushFollow(FOLLOW_commandRest_in_command533);
            	        	commandRest14 = commandRest();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_commandRest.Add(commandRest14.Tree);

            	        }
            	        break;

            	}



            	// AST REWRITE
            	// elements:          commandOptions, commandName, commandRest
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 157:70: -> ^( ASTCOMMAND ^( ASTCOMMAND1 commandName ) ^( ASTCOMMAND2 ( commandOptions )? ) ^( ASTCOMMAND3 ( commandRest )? ) )
            	{
            	    // T2.g:157:73: ^( ASTCOMMAND ^( ASTCOMMAND1 commandName ) ^( ASTCOMMAND2 ( commandOptions )? ) ^( ASTCOMMAND3 ( commandRest )? ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTCOMMAND, "ASTCOMMAND"), root_1);

            	    // T2.g:157:86: ^( ASTCOMMAND1 commandName )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTCOMMAND1, "ASTCOMMAND1"), root_2);

            	    adaptor.AddChild(root_2, stream_commandName.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // T2.g:157:113: ^( ASTCOMMAND2 ( commandOptions )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTCOMMAND2, "ASTCOMMAND2"), root_2);

            	    // T2.g:157:127: ( commandOptions )?
            	    if ( stream_commandOptions.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_commandOptions.NextTree());

            	    }
            	    stream_commandOptions.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // T2.g:157:144: ^( ASTCOMMAND3 ( commandRest )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTCOMMAND3, "ASTCOMMAND3"), root_2);

            	    // T2.g:157:158: ( commandRest )?
            	    if ( stream_commandRest.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_commandRest.NextTree());

            	    }
            	    stream_commandRest.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;}
            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 3, command_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "command"

    public class commandName_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "commandName"
    // T2.g:160:1: commandName : ( n )? ident ;
    public T2Parser.commandName_return commandName() // throws RecognitionException [1]
    {   
        T2Parser.commandName_return retval = new T2Parser.commandName_return();
        retval.Start = input.LT(1);
        int commandName_StartIndex = input.Index();
        object root_0 = null;

        T2Parser.n_return n15 = default(T2Parser.n_return);

        T2Parser.ident_return ident16 = default(T2Parser.ident_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 4) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:160:27: ( ( n )? ident )
            // T2.g:160:29: ( n )? ident
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// T2.g:160:29: ( n )?
            	int alt9 = 2;
            	int LA9_0 = input.LA(1);

            	if ( (LA9_0 == WHITESPACE) )
            	{
            	    alt9 = 1;
            	}
            	switch (alt9) 
            	{
            	    case 1 :
            	        // T2.g:0:0: n
            	        {
            	        	PushFollow(FOLLOW_n_in_commandName606);
            	        	n15 = n();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n15.Tree);

            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_ident_in_commandName609);
            	ident16 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, ident16.Tree);

            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 4, commandName_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "commandName"

    public class commandOptions_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "commandOptions"
    // T2.g:161:1: commandOptions : angle ;
    public T2Parser.commandOptions_return commandOptions() // throws RecognitionException [1]
    {   
        T2Parser.commandOptions_return retval = new T2Parser.commandOptions_return();
        retval.Start = input.LT(1);
        int commandOptions_StartIndex = input.Index();
        object root_0 = null;

        T2Parser.angle_return angle17 = default(T2Parser.angle_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 5) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:161:27: ( angle )
            // T2.g:161:29: angle
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_angle_in_commandOptions627);
            	angle17 = angle();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, angle17.Tree);

            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 5, commandOptions_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "commandOptions"

    public class commandRest_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "commandRest"
    // T2.g:162:1: commandRest : ( expressionAngle )* ;
    public T2Parser.commandRest_return commandRest() // throws RecognitionException [1]
    {   
        T2Parser.commandRest_return retval = new T2Parser.commandRest_return();
        retval.Start = input.LT(1);
        int commandRest_StartIndex = input.Index();
        object root_0 = null;

        T2Parser.expressionAngle_return expressionAngle18 = default(T2Parser.expressionAngle_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 6) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:162:27: ( ( expressionAngle )* )
            // T2.g:162:29: ( expressionAngle )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// T2.g:162:29: ( expressionAngle )*
            	do 
            	{
            	    int alt10 = 2;
            	    int LA10_0 = input.LA(1);

            	    if ( (LA10_0 == WHITESPACE) )
            	    {
            	        int LA10_1 = input.LA(2);

            	        if ( ((LA10_1 >= FIX && LA10_1 <= TIME) || LA10_1 == EOL || (LA10_1 >= LEFTANGLE && LA10_1 <= LEFTPAREN) || LA10_1 == LEFTBRACKET || LA10_1 == LEFTCURLY || (LA10_1 >= StringInQuotes && LA10_1 <= ANYTHING)) )
            	        {
            	            alt10 = 1;
            	        }


            	    }
            	    else if ( ((LA10_0 >= FIX && LA10_0 <= TIME) || LA10_0 == EOL || (LA10_0 >= LEFTANGLE && LA10_0 <= LEFTPAREN) || LA10_0 == LEFTBRACKET || LA10_0 == LEFTCURLY || (LA10_0 >= StringInQuotes && LA10_0 <= ANYTHING)) )
            	    {
            	        alt10 = 1;
            	    }


            	    switch (alt10) 
            		{
            			case 1 :
            			    // T2.g:0:0: expressionAngle
            			    {
            			    	PushFollow(FOLLOW_expressionAngle_in_commandRest648);
            			    	expressionAngle18 = expressionAngle();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expressionAngle18.Tree);

            			    }
            			    break;

            			default:
            			    goto loop10;
            	    }
            	} while (true);

            	loop10:
            		;	// Stops C# compiler whining that label 'loop10' has no statements


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 6, commandRest_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "commandRest"

    public class semi_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "semi"
    // T2.g:164:1: semi : ( n )? SEMICOLON -> ( n )? SEMICOLON ;
    public T2Parser.semi_return semi() // throws RecognitionException [1]
    {   
        T2Parser.semi_return retval = new T2Parser.semi_return();
        retval.Start = input.LT(1);
        int semi_StartIndex = input.Index();
        object root_0 = null;

        IToken SEMICOLON20 = null;
        T2Parser.n_return n19 = default(T2Parser.n_return);


        object SEMICOLON20_tree=null;
        RewriteRuleTokenStream stream_SEMICOLON = new RewriteRuleTokenStream(adaptor,"token SEMICOLON");
        RewriteRuleSubtreeStream stream_n = new RewriteRuleSubtreeStream(adaptor,"rule n");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 7) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:164:27: ( ( n )? SEMICOLON -> ( n )? SEMICOLON )
            // T2.g:164:29: ( n )? SEMICOLON
            {
            	// T2.g:164:29: ( n )?
            	int alt11 = 2;
            	int LA11_0 = input.LA(1);

            	if ( (LA11_0 == WHITESPACE) )
            	{
            	    alt11 = 1;
            	}
            	switch (alt11) 
            	{
            	    case 1 :
            	        // T2.g:0:0: n
            	        {
            	        	PushFollow(FOLLOW_n_in_semi678);
            	        	n19 = n();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_n.Add(n19.Tree);

            	        }
            	        break;

            	}

            	SEMICOLON20=(IToken)Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_semi681); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMICOLON.Add(SEMICOLON20);



            	// AST REWRITE
            	// elements:          SEMICOLON, n
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 164:42: -> ( n )? SEMICOLON
            	{
            	    // T2.g:164:45: ( n )?
            	    if ( stream_n.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_n.NextTree());

            	    }
            	    stream_n.Reset();
            	    adaptor.AddChild(root_0, stream_SEMICOLON.NextNode());

            	}

            	retval.Tree = root_0;retval.Tree = root_0;}
            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 7, semi_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "semi"

    public class n_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "n"
    // T2.g:167:1: n : WHITESPACE ;
    public T2Parser.n_return n() // throws RecognitionException [1]
    {   
        T2Parser.n_return retval = new T2Parser.n_return();
        retval.Start = input.LT(1);
        int n_StartIndex = input.Index();
        object root_0 = null;

        IToken WHITESPACE21 = null;

        object WHITESPACE21_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 8) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:167:27: ( WHITESPACE )
            // T2.g:167:29: WHITESPACE
            {
            	root_0 = (object)adaptor.GetNilNode();

            	WHITESPACE21=(IToken)Match(input,WHITESPACE,FOLLOW_WHITESPACE_in_n737); if (state.failed) return retval;
            	if ( state.backtracking == 0 )
            	{WHITESPACE21_tree = (object)adaptor.Create(WHITESPACE21);
            		adaptor.AddChild(root_0, WHITESPACE21_tree);
            	}

            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 8, n_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "n"

    public class n1_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "n1"
    // T2.g:168:1: n1 : WHITESPACE ;
    public T2Parser.n1_return n1() // throws RecognitionException [1]
    {   
        T2Parser.n1_return retval = new T2Parser.n1_return();
        retval.Start = input.LT(1);
        int n1_StartIndex = input.Index();
        object root_0 = null;

        IToken WHITESPACE22 = null;

        object WHITESPACE22_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 9) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:168:27: ( WHITESPACE )
            // T2.g:168:29: WHITESPACE
            {
            	root_0 = (object)adaptor.GetNilNode();

            	WHITESPACE22=(IToken)Match(input,WHITESPACE,FOLLOW_WHITESPACE_in_n1767); if (state.failed) return retval;
            	if ( state.backtracking == 0 )
            	{WHITESPACE22_tree = (object)adaptor.Create(WHITESPACE22);
            		adaptor.AddChild(root_0, WHITESPACE22_tree);
            	}

            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 9, n1_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "n1"

    public class n2_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "n2"
    // T2.g:169:1: n2 : WHITESPACE ;
    public T2Parser.n2_return n2() // throws RecognitionException [1]
    {   
        T2Parser.n2_return retval = new T2Parser.n2_return();
        retval.Start = input.LT(1);
        int n2_StartIndex = input.Index();
        object root_0 = null;

        IToken WHITESPACE23 = null;

        object WHITESPACE23_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 10) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:169:27: ( WHITESPACE )
            // T2.g:169:29: WHITESPACE
            {
            	root_0 = (object)adaptor.GetNilNode();

            	WHITESPACE23=(IToken)Match(input,WHITESPACE,FOLLOW_WHITESPACE_in_n2797); if (state.failed) return retval;
            	if ( state.backtracking == 0 )
            	{WHITESPACE23_tree = (object)adaptor.Create(WHITESPACE23);
            		adaptor.AddChild(root_0, WHITESPACE23_tree);
            	}

            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 10, n2_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "n2"

    public class n3_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "n3"
    // T2.g:170:1: n3 : WHITESPACE ;
    public T2Parser.n3_return n3() // throws RecognitionException [1]
    {   
        T2Parser.n3_return retval = new T2Parser.n3_return();
        retval.Start = input.LT(1);
        int n3_StartIndex = input.Index();
        object root_0 = null;

        IToken WHITESPACE24 = null;

        object WHITESPACE24_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 11) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:170:27: ( WHITESPACE )
            // T2.g:170:29: WHITESPACE
            {
            	root_0 = (object)adaptor.GetNilNode();

            	WHITESPACE24=(IToken)Match(input,WHITESPACE,FOLLOW_WHITESPACE_in_n3827); if (state.failed) return retval;
            	if ( state.backtracking == 0 )
            	{WHITESPACE24_tree = (object)adaptor.Create(WHITESPACE24);
            		adaptor.AddChild(root_0, WHITESPACE24_tree);
            	}

            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 11, n3_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "n3"

    public class n4_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "n4"
    // T2.g:171:1: n4 : WHITESPACE ;
    public T2Parser.n4_return n4() // throws RecognitionException [1]
    {   
        T2Parser.n4_return retval = new T2Parser.n4_return();
        retval.Start = input.LT(1);
        int n4_StartIndex = input.Index();
        object root_0 = null;

        IToken WHITESPACE25 = null;

        object WHITESPACE25_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 12) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:171:27: ( WHITESPACE )
            // T2.g:171:29: WHITESPACE
            {
            	root_0 = (object)adaptor.GetNilNode();

            	WHITESPACE25=(IToken)Match(input,WHITESPACE,FOLLOW_WHITESPACE_in_n4857); if (state.failed) return retval;
            	if ( state.backtracking == 0 )
            	{WHITESPACE25_tree = (object)adaptor.Create(WHITESPACE25);
            		adaptor.AddChild(root_0, WHITESPACE25_tree);
            	}

            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 12, n4_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "n4"

    public class n5_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "n5"
    // T2.g:172:1: n5 : WHITESPACE ;
    public T2Parser.n5_return n5() // throws RecognitionException [1]
    {   
        T2Parser.n5_return retval = new T2Parser.n5_return();
        retval.Start = input.LT(1);
        int n5_StartIndex = input.Index();
        object root_0 = null;

        IToken WHITESPACE26 = null;

        object WHITESPACE26_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 13) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:172:27: ( WHITESPACE )
            // T2.g:172:29: WHITESPACE
            {
            	root_0 = (object)adaptor.GetNilNode();

            	WHITESPACE26=(IToken)Match(input,WHITESPACE,FOLLOW_WHITESPACE_in_n5887); if (state.failed) return retval;
            	if ( state.backtracking == 0 )
            	{WHITESPACE26_tree = (object)adaptor.Create(WHITESPACE26);
            		adaptor.AddChild(root_0, WHITESPACE26_tree);
            	}

            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 13, n5_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "n5"

    public class expressionAngle_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expressionAngle"
    // T2.g:176:1: expressionAngle : ( expression | ( n )? LEFTANGLE | ( n )? RIGHTANGLE );
    public T2Parser.expressionAngle_return expressionAngle() // throws RecognitionException [1]
    {   
        T2Parser.expressionAngle_return retval = new T2Parser.expressionAngle_return();
        retval.Start = input.LT(1);
        int expressionAngle_StartIndex = input.Index();
        object root_0 = null;

        IToken LEFTANGLE29 = null;
        IToken RIGHTANGLE31 = null;
        T2Parser.expression_return expression27 = default(T2Parser.expression_return);

        T2Parser.n_return n28 = default(T2Parser.n_return);

        T2Parser.n_return n30 = default(T2Parser.n_return);


        object LEFTANGLE29_tree=null;
        object RIGHTANGLE31_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 14) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:176:27: ( expression | ( n )? LEFTANGLE | ( n )? RIGHTANGLE )
            int alt14 = 3;
            alt14 = dfa14.Predict(input);
            switch (alt14) 
            {
                case 1 :
                    // T2.g:176:29: expression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_expression_in_expressionAngle907);
                    	expression27 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression27.Tree);

                    }
                    break;
                case 2 :
                    // T2.g:177:29: ( n )? LEFTANGLE
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:177:29: ( n )?
                    	int alt12 = 2;
                    	int LA12_0 = input.LA(1);

                    	if ( (LA12_0 == WHITESPACE) )
                    	{
                    	    alt12 = 1;
                    	}
                    	switch (alt12) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_expressionAngle937);
                    	        	n28 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n28.Tree);

                    	        }
                    	        break;

                    	}

                    	LEFTANGLE29=(IToken)Match(input,LEFTANGLE,FOLLOW_LEFTANGLE_in_expressionAngle940); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{LEFTANGLE29_tree = (object)adaptor.Create(LEFTANGLE29);
                    		adaptor.AddChild(root_0, LEFTANGLE29_tree);
                    	}

                    }
                    break;
                case 3 :
                    // T2.g:178:11: ( n )? RIGHTANGLE
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:178:11: ( n )?
                    	int alt13 = 2;
                    	int LA13_0 = input.LA(1);

                    	if ( (LA13_0 == WHITESPACE) )
                    	{
                    	    alt13 = 1;
                    	}
                    	switch (alt13) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_expressionAngle952);
                    	        	n30 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n30.Tree);

                    	        }
                    	        break;

                    	}

                    	RIGHTANGLE31=(IToken)Match(input,RIGHTANGLE,FOLLOW_RIGHTANGLE_in_expressionAngle955); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{RIGHTANGLE31_tree = (object)adaptor.Create(RIGHTANGLE31);
                    		adaptor.AddChild(root_0, RIGHTANGLE31_tree);
                    	}

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 14, expressionAngle_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "expressionAngle"

    public class expression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expression"
    // T2.g:181:1: expression : ( paren | angle | bracket | curly | term );
    public T2Parser.expression_return expression() // throws RecognitionException [1]
    {   
        T2Parser.expression_return retval = new T2Parser.expression_return();
        retval.Start = input.LT(1);
        int expression_StartIndex = input.Index();
        object root_0 = null;

        T2Parser.paren_return paren32 = default(T2Parser.paren_return);

        T2Parser.angle_return angle33 = default(T2Parser.angle_return);

        T2Parser.bracket_return bracket34 = default(T2Parser.bracket_return);

        T2Parser.curly_return curly35 = default(T2Parser.curly_return);

        T2Parser.term_return term36 = default(T2Parser.term_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 15) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:181:27: ( paren | angle | bracket | curly | term )
            int alt15 = 5;
            switch ( input.LA(1) ) 
            {
            case WHITESPACE:
            	{
                switch ( input.LA(2) ) 
                {
                case FIX:
                case GENR:
                case LIST:
                case SKIP:
                case UPD:
                case TIME:
                case EOL:
                case StringInQuotes:
                case StringInQuotes2:
                case Integer:
                case Double:
                case DigitsEDigits:
                case DateDef:
                case IdentStartingWithInt:
                case Ident:
                case TILDE:
                case AND:
                case AT:
                case HAT:
                case COLON:
                case DOT:
                case HASH:
                case PERCENT:
                case DOLLAR:
                case STAR:
                case STARS:
                case VERTICALBAR:
                case PLUS:
                case MINUS:
                case DIV:
                case EQUAL:
                case BACKSLASH:
                case QUESTION:
                case COMMA:
                case ANYTHING:
                	{
                    alt15 = 5;
                    }
                    break;
                case LEFTANGLE:
                	{
                    alt15 = 2;
                    }
                    break;
                case LEFTPAREN:
                	{
                    alt15 = 1;
                    }
                    break;
                case LEFTCURLY:
                	{
                    alt15 = 4;
                    }
                    break;
                case LEFTBRACKET:
                	{
                    alt15 = 3;
                    }
                    break;
                	default:
                	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                	    NoViableAltException nvae_d15s1 =
                	        new NoViableAltException("", 15, 1, input);

                	    throw nvae_d15s1;
                }

                }
                break;
            case LEFTPAREN:
            	{
                alt15 = 1;
                }
                break;
            case LEFTANGLE:
            	{
                alt15 = 2;
                }
                break;
            case LEFTBRACKET:
            	{
                alt15 = 3;
                }
                break;
            case LEFTCURLY:
            	{
                alt15 = 4;
                }
                break;
            case FIX:
            case GENR:
            case LIST:
            case SKIP:
            case UPD:
            case TIME:
            case EOL:
            case StringInQuotes:
            case StringInQuotes2:
            case Integer:
            case Double:
            case DigitsEDigits:
            case DateDef:
            case IdentStartingWithInt:
            case Ident:
            case TILDE:
            case AND:
            case AT:
            case HAT:
            case COLON:
            case DOT:
            case HASH:
            case PERCENT:
            case DOLLAR:
            case STAR:
            case STARS:
            case VERTICALBAR:
            case PLUS:
            case MINUS:
            case DIV:
            case EQUAL:
            case BACKSLASH:
            case QUESTION:
            case COMMA:
            case ANYTHING:
            	{
                alt15 = 5;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d15s0 =
            	        new NoViableAltException("", 15, 0, input);

            	    throw nvae_d15s0;
            }

            switch (alt15) 
            {
                case 1 :
                    // T2.g:181:29: paren
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_paren_in_expression987);
                    	paren32 = paren();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, paren32.Tree);

                    }
                    break;
                case 2 :
                    // T2.g:182:11: angle
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_angle_in_expression1000);
                    	angle33 = angle();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, angle33.Tree);

                    }
                    break;
                case 3 :
                    // T2.g:183:11: bracket
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_bracket_in_expression1013);
                    	bracket34 = bracket();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, bracket34.Tree);

                    }
                    break;
                case 4 :
                    // T2.g:184:11: curly
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_curly_in_expression1026);
                    	curly35 = curly();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, curly35.Tree);

                    }
                    break;
                case 5 :
                    // T2.g:185:11: term
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_term_in_expression1038);
                    	term36 = term();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, term36.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 15, expression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "expression"

    public class paren_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "paren"
    // T2.g:188:1: paren : ( n )? LEFTPAREN ( expression )* ( n )? RIGHTPAREN -> ^( ASTPAREN ( n )? LEFTPAREN ( expression )* ( n )? RIGHTPAREN ) ;
    public T2Parser.paren_return paren() // throws RecognitionException [1]
    {   
        T2Parser.paren_return retval = new T2Parser.paren_return();
        retval.Start = input.LT(1);
        int paren_StartIndex = input.Index();
        object root_0 = null;

        IToken LEFTPAREN38 = null;
        IToken RIGHTPAREN41 = null;
        T2Parser.n_return n37 = default(T2Parser.n_return);

        T2Parser.expression_return expression39 = default(T2Parser.expression_return);

        T2Parser.n_return n40 = default(T2Parser.n_return);


        object LEFTPAREN38_tree=null;
        object RIGHTPAREN41_tree=null;
        RewriteRuleTokenStream stream_LEFTPAREN = new RewriteRuleTokenStream(adaptor,"token LEFTPAREN");
        RewriteRuleTokenStream stream_RIGHTPAREN = new RewriteRuleTokenStream(adaptor,"token RIGHTPAREN");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_n = new RewriteRuleSubtreeStream(adaptor,"rule n");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 16) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:188:27: ( ( n )? LEFTPAREN ( expression )* ( n )? RIGHTPAREN -> ^( ASTPAREN ( n )? LEFTPAREN ( expression )* ( n )? RIGHTPAREN ) )
            // T2.g:188:29: ( n )? LEFTPAREN ( expression )* ( n )? RIGHTPAREN
            {
            	// T2.g:188:29: ( n )?
            	int alt16 = 2;
            	int LA16_0 = input.LA(1);

            	if ( (LA16_0 == WHITESPACE) )
            	{
            	    alt16 = 1;
            	}
            	switch (alt16) 
            	{
            	    case 1 :
            	        // T2.g:0:0: n
            	        {
            	        	PushFollow(FOLLOW_n_in_paren1075);
            	        	n37 = n();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_n.Add(n37.Tree);

            	        }
            	        break;

            	}

            	LEFTPAREN38=(IToken)Match(input,LEFTPAREN,FOLLOW_LEFTPAREN_in_paren1078); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_LEFTPAREN.Add(LEFTPAREN38);

            	// T2.g:188:42: ( expression )*
            	do 
            	{
            	    int alt17 = 2;
            	    int LA17_0 = input.LA(1);

            	    if ( (LA17_0 == WHITESPACE) )
            	    {
            	        int LA17_1 = input.LA(2);

            	        if ( ((LA17_1 >= FIX && LA17_1 <= TIME) || LA17_1 == EOL || LA17_1 == LEFTANGLE || LA17_1 == LEFTPAREN || LA17_1 == LEFTBRACKET || LA17_1 == LEFTCURLY || (LA17_1 >= StringInQuotes && LA17_1 <= ANYTHING)) )
            	        {
            	            alt17 = 1;
            	        }


            	    }
            	    else if ( ((LA17_0 >= FIX && LA17_0 <= TIME) || LA17_0 == EOL || LA17_0 == LEFTANGLE || LA17_0 == LEFTPAREN || LA17_0 == LEFTBRACKET || LA17_0 == LEFTCURLY || (LA17_0 >= StringInQuotes && LA17_0 <= ANYTHING)) )
            	    {
            	        alt17 = 1;
            	    }


            	    switch (alt17) 
            		{
            			case 1 :
            			    // T2.g:0:0: expression
            			    {
            			    	PushFollow(FOLLOW_expression_in_paren1080);
            			    	expression39 = expression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_expression.Add(expression39.Tree);

            			    }
            			    break;

            			default:
            			    goto loop17;
            	    }
            	} while (true);

            	loop17:
            		;	// Stops C# compiler whining that label 'loop17' has no statements

            	// T2.g:188:54: ( n )?
            	int alt18 = 2;
            	int LA18_0 = input.LA(1);

            	if ( (LA18_0 == WHITESPACE) )
            	{
            	    alt18 = 1;
            	}
            	switch (alt18) 
            	{
            	    case 1 :
            	        // T2.g:0:0: n
            	        {
            	        	PushFollow(FOLLOW_n_in_paren1083);
            	        	n40 = n();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_n.Add(n40.Tree);

            	        }
            	        break;

            	}

            	RIGHTPAREN41=(IToken)Match(input,RIGHTPAREN,FOLLOW_RIGHTPAREN_in_paren1086); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_RIGHTPAREN.Add(RIGHTPAREN41);



            	// AST REWRITE
            	// elements:          expression, n, RIGHTPAREN, LEFTPAREN, n
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 188:68: -> ^( ASTPAREN ( n )? LEFTPAREN ( expression )* ( n )? RIGHTPAREN )
            	{
            	    // T2.g:188:71: ^( ASTPAREN ( n )? LEFTPAREN ( expression )* ( n )? RIGHTPAREN )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTPAREN, "ASTPAREN"), root_1);

            	    // T2.g:188:83: ( n )?
            	    if ( stream_n.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_n.NextTree());

            	    }
            	    stream_n.Reset();
            	    adaptor.AddChild(root_1, stream_LEFTPAREN.NextNode());
            	    // T2.g:188:96: ( expression )*
            	    while ( stream_expression.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_expression.NextTree());

            	    }
            	    stream_expression.Reset();
            	    // T2.g:188:108: ( n )?
            	    if ( stream_n.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_n.NextTree());

            	    }
            	    stream_n.Reset();
            	    adaptor.AddChild(root_1, stream_RIGHTPAREN.NextNode());

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;}
            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 16, paren_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "paren"

    public class angle_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "angle"
    // T2.g:189:1: angle : ( n )? LEFTANGLE ( expression )* ( n )? RIGHTANGLE -> ^( ASTANGLE ( n )? LEFTANGLE ( expression )* ( n )? RIGHTANGLE ) ;
    public T2Parser.angle_return angle() // throws RecognitionException [1]
    {   
        T2Parser.angle_return retval = new T2Parser.angle_return();
        retval.Start = input.LT(1);
        int angle_StartIndex = input.Index();
        object root_0 = null;

        IToken LEFTANGLE43 = null;
        IToken RIGHTANGLE46 = null;
        T2Parser.n_return n42 = default(T2Parser.n_return);

        T2Parser.expression_return expression44 = default(T2Parser.expression_return);

        T2Parser.n_return n45 = default(T2Parser.n_return);


        object LEFTANGLE43_tree=null;
        object RIGHTANGLE46_tree=null;
        RewriteRuleTokenStream stream_LEFTANGLE = new RewriteRuleTokenStream(adaptor,"token LEFTANGLE");
        RewriteRuleTokenStream stream_RIGHTANGLE = new RewriteRuleTokenStream(adaptor,"token RIGHTANGLE");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_n = new RewriteRuleSubtreeStream(adaptor,"rule n");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 17) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:189:27: ( ( n )? LEFTANGLE ( expression )* ( n )? RIGHTANGLE -> ^( ASTANGLE ( n )? LEFTANGLE ( expression )* ( n )? RIGHTANGLE ) )
            // T2.g:189:29: ( n )? LEFTANGLE ( expression )* ( n )? RIGHTANGLE
            {
            	// T2.g:189:29: ( n )?
            	int alt19 = 2;
            	int LA19_0 = input.LA(1);

            	if ( (LA19_0 == WHITESPACE) )
            	{
            	    alt19 = 1;
            	}
            	switch (alt19) 
            	{
            	    case 1 :
            	        // T2.g:0:0: n
            	        {
            	        	PushFollow(FOLLOW_n_in_angle1133);
            	        	n42 = n();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_n.Add(n42.Tree);

            	        }
            	        break;

            	}

            	LEFTANGLE43=(IToken)Match(input,LEFTANGLE,FOLLOW_LEFTANGLE_in_angle1136); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_LEFTANGLE.Add(LEFTANGLE43);

            	// T2.g:189:42: ( expression )*
            	do 
            	{
            	    int alt20 = 2;
            	    int LA20_0 = input.LA(1);

            	    if ( (LA20_0 == WHITESPACE) )
            	    {
            	        int LA20_1 = input.LA(2);

            	        if ( ((LA20_1 >= FIX && LA20_1 <= TIME) || LA20_1 == EOL || LA20_1 == LEFTANGLE || LA20_1 == LEFTPAREN || LA20_1 == LEFTBRACKET || LA20_1 == LEFTCURLY || (LA20_1 >= StringInQuotes && LA20_1 <= ANYTHING)) )
            	        {
            	            alt20 = 1;
            	        }


            	    }
            	    else if ( ((LA20_0 >= FIX && LA20_0 <= TIME) || LA20_0 == EOL || LA20_0 == LEFTANGLE || LA20_0 == LEFTPAREN || LA20_0 == LEFTBRACKET || LA20_0 == LEFTCURLY || (LA20_0 >= StringInQuotes && LA20_0 <= ANYTHING)) )
            	    {
            	        alt20 = 1;
            	    }


            	    switch (alt20) 
            		{
            			case 1 :
            			    // T2.g:0:0: expression
            			    {
            			    	PushFollow(FOLLOW_expression_in_angle1138);
            			    	expression44 = expression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_expression.Add(expression44.Tree);

            			    }
            			    break;

            			default:
            			    goto loop20;
            	    }
            	} while (true);

            	loop20:
            		;	// Stops C# compiler whining that label 'loop20' has no statements

            	// T2.g:189:54: ( n )?
            	int alt21 = 2;
            	int LA21_0 = input.LA(1);

            	if ( (LA21_0 == WHITESPACE) )
            	{
            	    alt21 = 1;
            	}
            	switch (alt21) 
            	{
            	    case 1 :
            	        // T2.g:0:0: n
            	        {
            	        	PushFollow(FOLLOW_n_in_angle1141);
            	        	n45 = n();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_n.Add(n45.Tree);

            	        }
            	        break;

            	}

            	RIGHTANGLE46=(IToken)Match(input,RIGHTANGLE,FOLLOW_RIGHTANGLE_in_angle1144); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_RIGHTANGLE.Add(RIGHTANGLE46);



            	// AST REWRITE
            	// elements:          n, n, RIGHTANGLE, LEFTANGLE, expression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 189:68: -> ^( ASTANGLE ( n )? LEFTANGLE ( expression )* ( n )? RIGHTANGLE )
            	{
            	    // T2.g:189:71: ^( ASTANGLE ( n )? LEFTANGLE ( expression )* ( n )? RIGHTANGLE )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTANGLE, "ASTANGLE"), root_1);

            	    // T2.g:189:82: ( n )?
            	    if ( stream_n.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_n.NextTree());

            	    }
            	    stream_n.Reset();
            	    adaptor.AddChild(root_1, stream_LEFTANGLE.NextNode());
            	    // T2.g:189:95: ( expression )*
            	    while ( stream_expression.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_expression.NextTree());

            	    }
            	    stream_expression.Reset();
            	    // T2.g:189:107: ( n )?
            	    if ( stream_n.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_n.NextTree());

            	    }
            	    stream_n.Reset();
            	    adaptor.AddChild(root_1, stream_RIGHTANGLE.NextNode());

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;}
            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 17, angle_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "angle"

    public class bracket_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "bracket"
    // T2.g:190:1: bracket : ( n )? LEFTBRACKET ( expression )* ( n )? RIGHTBRACKET -> ^( ASTBRACKET ( n )? LEFTBRACKET ( expression )* ( n )? RIGHTBRACKET ) ;
    public T2Parser.bracket_return bracket() // throws RecognitionException [1]
    {   
        T2Parser.bracket_return retval = new T2Parser.bracket_return();
        retval.Start = input.LT(1);
        int bracket_StartIndex = input.Index();
        object root_0 = null;

        IToken LEFTBRACKET48 = null;
        IToken RIGHTBRACKET51 = null;
        T2Parser.n_return n47 = default(T2Parser.n_return);

        T2Parser.expression_return expression49 = default(T2Parser.expression_return);

        T2Parser.n_return n50 = default(T2Parser.n_return);


        object LEFTBRACKET48_tree=null;
        object RIGHTBRACKET51_tree=null;
        RewriteRuleTokenStream stream_RIGHTBRACKET = new RewriteRuleTokenStream(adaptor,"token RIGHTBRACKET");
        RewriteRuleTokenStream stream_LEFTBRACKET = new RewriteRuleTokenStream(adaptor,"token LEFTBRACKET");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_n = new RewriteRuleSubtreeStream(adaptor,"rule n");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 18) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:190:27: ( ( n )? LEFTBRACKET ( expression )* ( n )? RIGHTBRACKET -> ^( ASTBRACKET ( n )? LEFTBRACKET ( expression )* ( n )? RIGHTBRACKET ) )
            // T2.g:190:29: ( n )? LEFTBRACKET ( expression )* ( n )? RIGHTBRACKET
            {
            	// T2.g:190:29: ( n )?
            	int alt22 = 2;
            	int LA22_0 = input.LA(1);

            	if ( (LA22_0 == WHITESPACE) )
            	{
            	    alt22 = 1;
            	}
            	switch (alt22) 
            	{
            	    case 1 :
            	        // T2.g:0:0: n
            	        {
            	        	PushFollow(FOLLOW_n_in_bracket1188);
            	        	n47 = n();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_n.Add(n47.Tree);

            	        }
            	        break;

            	}

            	LEFTBRACKET48=(IToken)Match(input,LEFTBRACKET,FOLLOW_LEFTBRACKET_in_bracket1191); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_LEFTBRACKET.Add(LEFTBRACKET48);

            	// T2.g:190:44: ( expression )*
            	do 
            	{
            	    int alt23 = 2;
            	    int LA23_0 = input.LA(1);

            	    if ( (LA23_0 == WHITESPACE) )
            	    {
            	        int LA23_1 = input.LA(2);

            	        if ( ((LA23_1 >= FIX && LA23_1 <= TIME) || LA23_1 == EOL || LA23_1 == LEFTANGLE || LA23_1 == LEFTPAREN || LA23_1 == LEFTBRACKET || LA23_1 == LEFTCURLY || (LA23_1 >= StringInQuotes && LA23_1 <= ANYTHING)) )
            	        {
            	            alt23 = 1;
            	        }


            	    }
            	    else if ( ((LA23_0 >= FIX && LA23_0 <= TIME) || LA23_0 == EOL || LA23_0 == LEFTANGLE || LA23_0 == LEFTPAREN || LA23_0 == LEFTBRACKET || LA23_0 == LEFTCURLY || (LA23_0 >= StringInQuotes && LA23_0 <= ANYTHING)) )
            	    {
            	        alt23 = 1;
            	    }


            	    switch (alt23) 
            		{
            			case 1 :
            			    // T2.g:0:0: expression
            			    {
            			    	PushFollow(FOLLOW_expression_in_bracket1193);
            			    	expression49 = expression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_expression.Add(expression49.Tree);

            			    }
            			    break;

            			default:
            			    goto loop23;
            	    }
            	} while (true);

            	loop23:
            		;	// Stops C# compiler whining that label 'loop23' has no statements

            	// T2.g:190:56: ( n )?
            	int alt24 = 2;
            	int LA24_0 = input.LA(1);

            	if ( (LA24_0 == WHITESPACE) )
            	{
            	    alt24 = 1;
            	}
            	switch (alt24) 
            	{
            	    case 1 :
            	        // T2.g:0:0: n
            	        {
            	        	PushFollow(FOLLOW_n_in_bracket1196);
            	        	n50 = n();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_n.Add(n50.Tree);

            	        }
            	        break;

            	}

            	RIGHTBRACKET51=(IToken)Match(input,RIGHTBRACKET,FOLLOW_RIGHTBRACKET_in_bracket1199); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_RIGHTBRACKET.Add(RIGHTBRACKET51);



            	// AST REWRITE
            	// elements:          n, RIGHTBRACKET, LEFTBRACKET, expression, n
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 190:72: -> ^( ASTBRACKET ( n )? LEFTBRACKET ( expression )* ( n )? RIGHTBRACKET )
            	{
            	    // T2.g:190:75: ^( ASTBRACKET ( n )? LEFTBRACKET ( expression )* ( n )? RIGHTBRACKET )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTBRACKET, "ASTBRACKET"), root_1);

            	    // T2.g:190:88: ( n )?
            	    if ( stream_n.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_n.NextTree());

            	    }
            	    stream_n.Reset();
            	    adaptor.AddChild(root_1, stream_LEFTBRACKET.NextNode());
            	    // T2.g:190:104: ( expression )*
            	    while ( stream_expression.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_expression.NextTree());

            	    }
            	    stream_expression.Reset();
            	    // T2.g:190:116: ( n )?
            	    if ( stream_n.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_n.NextTree());

            	    }
            	    stream_n.Reset();
            	    adaptor.AddChild(root_1, stream_RIGHTBRACKET.NextNode());

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;}
            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 18, bracket_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "bracket"

    public class curly_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "curly"
    // T2.g:191:1: curly : ( n )? LEFTCURLY ( expression )* ( n )? RIGHTCURLY -> ^( ASTCURLY ( n )? LEFTCURLY ( expression )* ( n )? RIGHTCURLY ) ;
    public T2Parser.curly_return curly() // throws RecognitionException [1]
    {   
        T2Parser.curly_return retval = new T2Parser.curly_return();
        retval.Start = input.LT(1);
        int curly_StartIndex = input.Index();
        object root_0 = null;

        IToken LEFTCURLY53 = null;
        IToken RIGHTCURLY56 = null;
        T2Parser.n_return n52 = default(T2Parser.n_return);

        T2Parser.expression_return expression54 = default(T2Parser.expression_return);

        T2Parser.n_return n55 = default(T2Parser.n_return);


        object LEFTCURLY53_tree=null;
        object RIGHTCURLY56_tree=null;
        RewriteRuleTokenStream stream_RIGHTCURLY = new RewriteRuleTokenStream(adaptor,"token RIGHTCURLY");
        RewriteRuleTokenStream stream_LEFTCURLY = new RewriteRuleTokenStream(adaptor,"token LEFTCURLY");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_n = new RewriteRuleSubtreeStream(adaptor,"rule n");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 19) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:191:27: ( ( n )? LEFTCURLY ( expression )* ( n )? RIGHTCURLY -> ^( ASTCURLY ( n )? LEFTCURLY ( expression )* ( n )? RIGHTCURLY ) )
            // T2.g:191:29: ( n )? LEFTCURLY ( expression )* ( n )? RIGHTCURLY
            {
            	// T2.g:191:29: ( n )?
            	int alt25 = 2;
            	int LA25_0 = input.LA(1);

            	if ( (LA25_0 == WHITESPACE) )
            	{
            	    alt25 = 1;
            	}
            	switch (alt25) 
            	{
            	    case 1 :
            	        // T2.g:0:0: n
            	        {
            	        	PushFollow(FOLLOW_n_in_curly1246);
            	        	n52 = n();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_n.Add(n52.Tree);

            	        }
            	        break;

            	}

            	LEFTCURLY53=(IToken)Match(input,LEFTCURLY,FOLLOW_LEFTCURLY_in_curly1249); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_LEFTCURLY.Add(LEFTCURLY53);

            	// T2.g:191:42: ( expression )*
            	do 
            	{
            	    int alt26 = 2;
            	    int LA26_0 = input.LA(1);

            	    if ( (LA26_0 == WHITESPACE) )
            	    {
            	        int LA26_1 = input.LA(2);

            	        if ( ((LA26_1 >= FIX && LA26_1 <= TIME) || LA26_1 == EOL || LA26_1 == LEFTANGLE || LA26_1 == LEFTPAREN || LA26_1 == LEFTBRACKET || LA26_1 == LEFTCURLY || (LA26_1 >= StringInQuotes && LA26_1 <= ANYTHING)) )
            	        {
            	            alt26 = 1;
            	        }


            	    }
            	    else if ( ((LA26_0 >= FIX && LA26_0 <= TIME) || LA26_0 == EOL || LA26_0 == LEFTANGLE || LA26_0 == LEFTPAREN || LA26_0 == LEFTBRACKET || LA26_0 == LEFTCURLY || (LA26_0 >= StringInQuotes && LA26_0 <= ANYTHING)) )
            	    {
            	        alt26 = 1;
            	    }


            	    switch (alt26) 
            		{
            			case 1 :
            			    // T2.g:0:0: expression
            			    {
            			    	PushFollow(FOLLOW_expression_in_curly1251);
            			    	expression54 = expression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_expression.Add(expression54.Tree);

            			    }
            			    break;

            			default:
            			    goto loop26;
            	    }
            	} while (true);

            	loop26:
            		;	// Stops C# compiler whining that label 'loop26' has no statements

            	// T2.g:191:54: ( n )?
            	int alt27 = 2;
            	int LA27_0 = input.LA(1);

            	if ( (LA27_0 == WHITESPACE) )
            	{
            	    alt27 = 1;
            	}
            	switch (alt27) 
            	{
            	    case 1 :
            	        // T2.g:0:0: n
            	        {
            	        	PushFollow(FOLLOW_n_in_curly1254);
            	        	n55 = n();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_n.Add(n55.Tree);

            	        }
            	        break;

            	}

            	RIGHTCURLY56=(IToken)Match(input,RIGHTCURLY,FOLLOW_RIGHTCURLY_in_curly1257); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_RIGHTCURLY.Add(RIGHTCURLY56);



            	// AST REWRITE
            	// elements:          RIGHTCURLY, expression, n, LEFTCURLY, n
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 191:68: -> ^( ASTCURLY ( n )? LEFTCURLY ( expression )* ( n )? RIGHTCURLY )
            	{
            	    // T2.g:191:71: ^( ASTCURLY ( n )? LEFTCURLY ( expression )* ( n )? RIGHTCURLY )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTCURLY, "ASTCURLY"), root_1);

            	    // T2.g:191:82: ( n )?
            	    if ( stream_n.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_n.NextTree());

            	    }
            	    stream_n.Reset();
            	    adaptor.AddChild(root_1, stream_LEFTCURLY.NextNode());
            	    // T2.g:191:96: ( expression )*
            	    while ( stream_expression.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_expression.NextTree());

            	    }
            	    stream_expression.Reset();
            	    // T2.g:191:108: ( n )?
            	    if ( stream_n.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_n.NextTree());

            	    }
            	    stream_n.Reset();
            	    adaptor.AddChild(root_1, stream_RIGHTCURLY.NextNode());

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;}
            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 19, curly_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "curly"

    public class term_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "term"
    // T2.g:197:1: term : ( ( n )? ident | ( n )? leaf | ( n )? StringInQuotes | ( n )? StringInQuotes2 | ( n )? Integer | ( n )? Double | ( n )? DigitsEDigits | ( n )? DateDef | ( n )? IdentStartingWithInt | ( n )? Double );
    public T2Parser.term_return term() // throws RecognitionException [1]
    {   
        T2Parser.term_return retval = new T2Parser.term_return();
        retval.Start = input.LT(1);
        int term_StartIndex = input.Index();
        object root_0 = null;

        IToken StringInQuotes62 = null;
        IToken StringInQuotes264 = null;
        IToken Integer66 = null;
        IToken Double68 = null;
        IToken DigitsEDigits70 = null;
        IToken DateDef72 = null;
        IToken IdentStartingWithInt74 = null;
        IToken Double76 = null;
        T2Parser.n_return n57 = default(T2Parser.n_return);

        T2Parser.ident_return ident58 = default(T2Parser.ident_return);

        T2Parser.n_return n59 = default(T2Parser.n_return);

        T2Parser.leaf_return leaf60 = default(T2Parser.leaf_return);

        T2Parser.n_return n61 = default(T2Parser.n_return);

        T2Parser.n_return n63 = default(T2Parser.n_return);

        T2Parser.n_return n65 = default(T2Parser.n_return);

        T2Parser.n_return n67 = default(T2Parser.n_return);

        T2Parser.n_return n69 = default(T2Parser.n_return);

        T2Parser.n_return n71 = default(T2Parser.n_return);

        T2Parser.n_return n73 = default(T2Parser.n_return);

        T2Parser.n_return n75 = default(T2Parser.n_return);


        object StringInQuotes62_tree=null;
        object StringInQuotes264_tree=null;
        object Integer66_tree=null;
        object Double68_tree=null;
        object DigitsEDigits70_tree=null;
        object DateDef72_tree=null;
        object IdentStartingWithInt74_tree=null;
        object Double76_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 20) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:197:27: ( ( n )? ident | ( n )? leaf | ( n )? StringInQuotes | ( n )? StringInQuotes2 | ( n )? Integer | ( n )? Double | ( n )? DigitsEDigits | ( n )? DateDef | ( n )? IdentStartingWithInt | ( n )? Double )
            int alt38 = 10;
            alt38 = dfa38.Predict(input);
            switch (alt38) 
            {
                case 1 :
                    // T2.g:197:29: ( n )? ident
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:197:29: ( n )?
                    	int alt28 = 2;
                    	int LA28_0 = input.LA(1);

                    	if ( (LA28_0 == WHITESPACE) )
                    	{
                    	    alt28 = 1;
                    	}
                    	switch (alt28) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_term1310);
                    	        	n57 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n57.Tree);

                    	        }
                    	        break;

                    	}

                    	PushFollow(FOLLOW_ident_in_term1313);
                    	ident58 = ident();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, ident58.Tree);

                    }
                    break;
                case 2 :
                    // T2.g:198:11: ( n )? leaf
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:198:11: ( n )?
                    	int alt29 = 2;
                    	int LA29_0 = input.LA(1);

                    	if ( (LA29_0 == WHITESPACE) )
                    	{
                    	    alt29 = 1;
                    	}
                    	switch (alt29) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_term1326);
                    	        	n59 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n59.Tree);

                    	        }
                    	        break;

                    	}

                    	PushFollow(FOLLOW_leaf_in_term1329);
                    	leaf60 = leaf();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, leaf60.Tree);

                    }
                    break;
                case 3 :
                    // T2.g:199:11: ( n )? StringInQuotes
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:199:11: ( n )?
                    	int alt30 = 2;
                    	int LA30_0 = input.LA(1);

                    	if ( (LA30_0 == WHITESPACE) )
                    	{
                    	    alt30 = 1;
                    	}
                    	switch (alt30) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_term1349);
                    	        	n61 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n61.Tree);

                    	        }
                    	        break;

                    	}

                    	StringInQuotes62=(IToken)Match(input,StringInQuotes,FOLLOW_StringInQuotes_in_term1352); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{StringInQuotes62_tree = (object)adaptor.Create(StringInQuotes62);
                    		adaptor.AddChild(root_0, StringInQuotes62_tree);
                    	}

                    }
                    break;
                case 4 :
                    // T2.g:200:11: ( n )? StringInQuotes2
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:200:11: ( n )?
                    	int alt31 = 2;
                    	int LA31_0 = input.LA(1);

                    	if ( (LA31_0 == WHITESPACE) )
                    	{
                    	    alt31 = 1;
                    	}
                    	switch (alt31) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_term1364);
                    	        	n63 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n63.Tree);

                    	        }
                    	        break;

                    	}

                    	StringInQuotes264=(IToken)Match(input,StringInQuotes2,FOLLOW_StringInQuotes2_in_term1367); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{StringInQuotes264_tree = (object)adaptor.Create(StringInQuotes264);
                    		adaptor.AddChild(root_0, StringInQuotes264_tree);
                    	}

                    }
                    break;
                case 5 :
                    // T2.g:201:11: ( n )? Integer
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:201:11: ( n )?
                    	int alt32 = 2;
                    	int LA32_0 = input.LA(1);

                    	if ( (LA32_0 == WHITESPACE) )
                    	{
                    	    alt32 = 1;
                    	}
                    	switch (alt32) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_term1379);
                    	        	n65 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n65.Tree);

                    	        }
                    	        break;

                    	}

                    	Integer66=(IToken)Match(input,Integer,FOLLOW_Integer_in_term1382); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Integer66_tree = (object)adaptor.Create(Integer66);
                    		adaptor.AddChild(root_0, Integer66_tree);
                    	}

                    }
                    break;
                case 6 :
                    // T2.g:202:11: ( n )? Double
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:202:11: ( n )?
                    	int alt33 = 2;
                    	int LA33_0 = input.LA(1);

                    	if ( (LA33_0 == WHITESPACE) )
                    	{
                    	    alt33 = 1;
                    	}
                    	switch (alt33) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_term1394);
                    	        	n67 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n67.Tree);

                    	        }
                    	        break;

                    	}

                    	Double68=(IToken)Match(input,Double,FOLLOW_Double_in_term1397); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Double68_tree = (object)adaptor.Create(Double68);
                    		adaptor.AddChild(root_0, Double68_tree);
                    	}

                    }
                    break;
                case 7 :
                    // T2.g:203:11: ( n )? DigitsEDigits
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:203:11: ( n )?
                    	int alt34 = 2;
                    	int LA34_0 = input.LA(1);

                    	if ( (LA34_0 == WHITESPACE) )
                    	{
                    	    alt34 = 1;
                    	}
                    	switch (alt34) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_term1409);
                    	        	n69 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n69.Tree);

                    	        }
                    	        break;

                    	}

                    	DigitsEDigits70=(IToken)Match(input,DigitsEDigits,FOLLOW_DigitsEDigits_in_term1412); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{DigitsEDigits70_tree = (object)adaptor.Create(DigitsEDigits70);
                    		adaptor.AddChild(root_0, DigitsEDigits70_tree);
                    	}

                    }
                    break;
                case 8 :
                    // T2.g:204:11: ( n )? DateDef
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:204:11: ( n )?
                    	int alt35 = 2;
                    	int LA35_0 = input.LA(1);

                    	if ( (LA35_0 == WHITESPACE) )
                    	{
                    	    alt35 = 1;
                    	}
                    	switch (alt35) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_term1424);
                    	        	n71 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n71.Tree);

                    	        }
                    	        break;

                    	}

                    	DateDef72=(IToken)Match(input,DateDef,FOLLOW_DateDef_in_term1427); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{DateDef72_tree = (object)adaptor.Create(DateDef72);
                    		adaptor.AddChild(root_0, DateDef72_tree);
                    	}

                    }
                    break;
                case 9 :
                    // T2.g:205:11: ( n )? IdentStartingWithInt
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:205:11: ( n )?
                    	int alt36 = 2;
                    	int LA36_0 = input.LA(1);

                    	if ( (LA36_0 == WHITESPACE) )
                    	{
                    	    alt36 = 1;
                    	}
                    	switch (alt36) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_term1439);
                    	        	n73 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n73.Tree);

                    	        }
                    	        break;

                    	}

                    	IdentStartingWithInt74=(IToken)Match(input,IdentStartingWithInt,FOLLOW_IdentStartingWithInt_in_term1442); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{IdentStartingWithInt74_tree = (object)adaptor.Create(IdentStartingWithInt74);
                    		adaptor.AddChild(root_0, IdentStartingWithInt74_tree);
                    	}

                    }
                    break;
                case 10 :
                    // T2.g:206:11: ( n )? Double
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// T2.g:206:11: ( n )?
                    	int alt37 = 2;
                    	int LA37_0 = input.LA(1);

                    	if ( (LA37_0 == WHITESPACE) )
                    	{
                    	    alt37 = 1;
                    	}
                    	switch (alt37) 
                    	{
                    	    case 1 :
                    	        // T2.g:0:0: n
                    	        {
                    	        	PushFollow(FOLLOW_n_in_term1454);
                    	        	n75 = n();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, n75.Tree);

                    	        }
                    	        break;

                    	}

                    	Double76=(IToken)Match(input,Double,FOLLOW_Double_in_term1457); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Double76_tree = (object)adaptor.Create(Double76);
                    		adaptor.AddChild(root_0, Double76_tree);
                    	}

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 20, term_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "term"

    public class ident_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "ident"
    // T2.g:209:1: ident : ( Ident | FIX | LIST | GENR | UPD | TIME | SKIP );
    public T2Parser.ident_return ident() // throws RecognitionException [1]
    {   
        T2Parser.ident_return retval = new T2Parser.ident_return();
        retval.Start = input.LT(1);
        int ident_StartIndex = input.Index();
        object root_0 = null;

        IToken set77 = null;

        object set77_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 21) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:209:27: ( Ident | FIX | LIST | GENR | UPD | TIME | SKIP )
            // T2.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set77 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= FIX && input.LA(1) <= TIME) || input.LA(1) == Ident ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set77));
            	    state.errorRecovery = false;state.failed = false;
            	}
            	else 
            	{
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 21, ident_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "ident"

    public class leaf_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "leaf"
    // T2.g:218:1: leaf : ( TILDE | AND | AT | HAT | COLON | DOT | HASH | PERCENT | DOLLAR | STAR | STARS | VERTICALBAR | PLUS | MINUS | DIV | EQUAL | BACKSLASH | QUESTION | COMMA | EOL | ANYTHING );
    public T2Parser.leaf_return leaf() // throws RecognitionException [1]
    {   
        T2Parser.leaf_return retval = new T2Parser.leaf_return();
        retval.Start = input.LT(1);
        int leaf_StartIndex = input.Index();
        object root_0 = null;

        IToken set78 = null;

        object set78_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 22) ) 
    	    {
    	    	return retval; 
    	    }
            // T2.g:218:12: ( TILDE | AND | AT | HAT | COLON | DOT | HASH | PERCENT | DOLLAR | STAR | STARS | VERTICALBAR | PLUS | MINUS | DIV | EQUAL | BACKSLASH | QUESTION | COMMA | EOL | ANYTHING )
            // T2.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set78 = (IToken)input.LT(1);
            	if ( input.LA(1) == EOL || (input.LA(1) >= TILDE && input.LA(1) <= ANYTHING) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set78));
            	    state.errorRecovery = false;state.failed = false;
            	}
            	else 
            	{
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 22, leaf_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "leaf"

    // $ANTLR start "synpred8_T2"
    public void synpred8_T2_fragment() {
        // T2.g:157:41: ( commandOptions )
        // T2.g:157:41: commandOptions
        {
        	PushFollow(FOLLOW_commandOptions_in_synpred8_T2530);
        	commandOptions();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred8_T2"

    // $ANTLR start "synpred9_T2"
    public void synpred9_T2_fragment() {
        // T2.g:157:57: ( commandRest )
        // T2.g:157:57: commandRest
        {
        	PushFollow(FOLLOW_commandRest_in_synpred9_T2533);
        	commandRest();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred9_T2"

    // $ANTLR start "synpred13_T2"
    public void synpred13_T2_fragment() {
        // T2.g:176:29: ( expression )
        // T2.g:176:29: expression
        {
        	PushFollow(FOLLOW_expression_in_synpred13_T2907);
        	expression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred13_T2"

    // $ANTLR start "synpred15_T2"
    public void synpred15_T2_fragment() {
        // T2.g:177:29: ( ( n )? LEFTANGLE )
        // T2.g:177:29: ( n )? LEFTANGLE
        {
        	// T2.g:177:29: ( n )?
        	int alt41 = 2;
        	int LA41_0 = input.LA(1);

        	if ( (LA41_0 == WHITESPACE) )
        	{
        	    alt41 = 1;
        	}
        	switch (alt41) 
        	{
        	    case 1 :
        	        // T2.g:0:0: n
        	        {
        	        	PushFollow(FOLLOW_n_in_synpred15_T2937);
        	        	n();
        	        	state.followingStackPointer--;
        	        	if (state.failed) return ;

        	        }
        	        break;

        	}

        	Match(input,LEFTANGLE,FOLLOW_LEFTANGLE_in_synpred15_T2940); if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred15_T2"

    // $ANTLR start "synpred44_T2"
    public void synpred44_T2_fragment() {
        // T2.g:202:11: ( ( n )? Double )
        // T2.g:202:11: ( n )? Double
        {
        	// T2.g:202:11: ( n )?
        	int alt47 = 2;
        	int LA47_0 = input.LA(1);

        	if ( (LA47_0 == WHITESPACE) )
        	{
        	    alt47 = 1;
        	}
        	switch (alt47) 
        	{
        	    case 1 :
        	        // T2.g:0:0: n
        	        {
        	        	PushFollow(FOLLOW_n_in_synpred44_T21394);
        	        	n();
        	        	state.followingStackPointer--;
        	        	if (state.failed) return ;

        	        }
        	        break;

        	}

        	Match(input,Double,FOLLOW_Double_in_synpred44_T21397); if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred44_T2"

    // Delegated rules

   	public bool synpred8_T2() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred8_T2_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred13_T2() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred13_T2_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred9_T2() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred9_T2_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred15_T2() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred15_T2_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred44_T2() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred44_T2_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}


   	protected DFA7 dfa7;
   	protected DFA14 dfa14;
   	protected DFA38 dfa38;
	private void InitializeCyclicDFAs()
	{
    	this.dfa7 = new DFA7(this);
    	this.dfa14 = new DFA14(this);
    	this.dfa38 = new DFA38(this);
	    this.dfa7.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA7_SpecialStateTransition);
	    this.dfa14.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA14_SpecialStateTransition);
	    this.dfa38.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA38_SpecialStateTransition);
	}

    const string DFA7_eotS =
        "\x12\uffff";
    const string DFA7_eofS =
        "\x12\uffff";
    const string DFA7_minS =
        "\x01\x17\x02\x00\x0f\uffff";
    const string DFA7_maxS =
        "\x01\x45\x02\x00\x0f\uffff";
    const string DFA7_acceptS =
        "\x03\uffff\x01\x02\x0d\uffff\x01\x01";
    const string DFA7_specialS =
        "\x01\uffff\x01\x00\x01\x01\x0f\uffff}>";
    static readonly string[] DFA7_transitionS = {
            "\x06\x03\x02\uffff\x02\x03\x01\x01\x01\x02\x02\x03\x01\uffff"+
            "\x01\x03\x01\uffff\x01\x03\x01\uffff\x1c\x03",
            "\x01\uffff",
            "\x01\uffff",
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
            ""
    };

    static readonly short[] DFA7_eot = DFA.UnpackEncodedString(DFA7_eotS);
    static readonly short[] DFA7_eof = DFA.UnpackEncodedString(DFA7_eofS);
    static readonly char[] DFA7_min = DFA.UnpackEncodedStringToUnsignedChars(DFA7_minS);
    static readonly char[] DFA7_max = DFA.UnpackEncodedStringToUnsignedChars(DFA7_maxS);
    static readonly short[] DFA7_accept = DFA.UnpackEncodedString(DFA7_acceptS);
    static readonly short[] DFA7_special = DFA.UnpackEncodedString(DFA7_specialS);
    static readonly short[][] DFA7_transition = DFA.UnpackEncodedStringArray(DFA7_transitionS);

    protected class DFA7 : DFA
    {
        public DFA7(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 7;
            this.eot = DFA7_eot;
            this.eof = DFA7_eof;
            this.min = DFA7_min;
            this.max = DFA7_max;
            this.accept = DFA7_accept;
            this.special = DFA7_special;
            this.transition = DFA7_transition;

        }

        override public string Description
        {
            get { return "157:41: ( commandOptions )?"; }
        }

    }


    protected internal int DFA7_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA7_1 = input.LA(1);

                   	 
                   	int index7_1 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred8_T2()) ) { s = 17; }

                   	else if ( (true) ) { s = 3; }

                   	 
                   	input.Seek(index7_1);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA7_2 = input.LA(1);

                   	 
                   	int index7_2 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred8_T2()) ) { s = 17; }

                   	else if ( (true) ) { s = 3; }

                   	 
                   	input.Seek(index7_2);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae7 =
            new NoViableAltException(dfa.Description, 7, _s, input);
        dfa.Error(nvae7);
        throw nvae7;
    }
    const string DFA14_eotS =
        "\x11\uffff";
    const string DFA14_eofS =
        "\x11\uffff";
    const string DFA14_minS =
        "\x01\x17\x01\x00\x01\uffff\x01\x00\x0d\uffff";
    const string DFA14_maxS =
        "\x01\x45\x01\x00\x01\uffff\x01\x00\x0d\uffff";
    const string DFA14_acceptS =
        "\x02\uffff\x01\x01\x0c\uffff\x01\x03\x01\x02";
    const string DFA14_specialS =
        "\x01\uffff\x01\x00\x01\uffff\x01\x01\x0d\uffff}>";
    static readonly string[] DFA14_transitionS = {
            "\x06\x02\x02\uffff\x01\x02\x01\uffff\x01\x01\x01\x03\x01\x0f"+
            "\x01\x02\x01\uffff\x01\x02\x01\uffff\x01\x02\x01\uffff\x1c\x02",
            "\x01\uffff",
            "",
            "\x01\uffff",
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
            get { return "176:1: expressionAngle : ( expression | ( n )? LEFTANGLE | ( n )? RIGHTANGLE );"; }
        }

    }


    protected internal int DFA14_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA14_1 = input.LA(1);

                   	 
                   	int index14_1 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_T2()) ) { s = 2; }

                   	else if ( (synpred15_T2()) ) { s = 16; }

                   	else if ( (true) ) { s = 15; }

                   	 
                   	input.Seek(index14_1);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA14_3 = input.LA(1);

                   	 
                   	int index14_3 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_T2()) ) { s = 2; }

                   	else if ( (synpred15_T2()) ) { s = 16; }

                   	 
                   	input.Seek(index14_3);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae14 =
            new NoViableAltException(dfa.Description, 14, _s, input);
        dfa.Error(nvae14);
        throw nvae14;
    }
    const string DFA38_eotS =
        "\x0d\uffff";
    const string DFA38_eofS =
        "\x0d\uffff";
    const string DFA38_minS =
        "\x02\x17\x05\uffff\x01\x00\x05\uffff";
    const string DFA38_maxS =
        "\x02\x45\x05\uffff\x01\x00\x05\uffff";
    const string DFA38_acceptS =
        "\x02\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01\uffff\x01"+
        "\x07\x01\x08\x01\x09\x01\x06\x01\x0a";
    const string DFA38_specialS =
        "\x07\uffff\x01\x00\x05\uffff}>";
    static readonly string[] DFA38_transitionS = {
            "\x06\x02\x02\uffff\x01\x03\x01\uffff\x01\x01\x08\uffff\x01"+
            "\x04\x01\x05\x01\x06\x01\x07\x01\x08\x01\x09\x01\x0a\x01\x02"+
            "\x14\x03",
            "\x06\x02\x02\uffff\x01\x03\x0a\uffff\x01\x04\x01\x05\x01\x06"+
            "\x01\x07\x01\x08\x01\x09\x01\x0a\x01\x02\x14\x03",
            "",
            "",
            "",
            "",
            "",
            "\x01\uffff",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA38_eot = DFA.UnpackEncodedString(DFA38_eotS);
    static readonly short[] DFA38_eof = DFA.UnpackEncodedString(DFA38_eofS);
    static readonly char[] DFA38_min = DFA.UnpackEncodedStringToUnsignedChars(DFA38_minS);
    static readonly char[] DFA38_max = DFA.UnpackEncodedStringToUnsignedChars(DFA38_maxS);
    static readonly short[] DFA38_accept = DFA.UnpackEncodedString(DFA38_acceptS);
    static readonly short[] DFA38_special = DFA.UnpackEncodedString(DFA38_specialS);
    static readonly short[][] DFA38_transition = DFA.UnpackEncodedStringArray(DFA38_transitionS);

    protected class DFA38 : DFA
    {
        public DFA38(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 38;
            this.eot = DFA38_eot;
            this.eof = DFA38_eof;
            this.min = DFA38_min;
            this.max = DFA38_max;
            this.accept = DFA38_accept;
            this.special = DFA38_special;
            this.transition = DFA38_transition;

        }

        override public string Description
        {
            get { return "197:1: term : ( ( n )? ident | ( n )? leaf | ( n )? StringInQuotes | ( n )? StringInQuotes2 | ( n )? Integer | ( n )? Double | ( n )? DigitsEDigits | ( n )? DateDef | ( n )? IdentStartingWithInt | ( n )? Double );"; }
        }

    }


    protected internal int DFA38_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA38_7 = input.LA(1);

                   	 
                   	int index38_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred44_T2()) ) { s = 11; }

                   	else if ( (true) ) { s = 12; }

                   	 
                   	input.Seek(index38_7);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae38 =
            new NoViableAltException(dfa.Description, 38, _s, input);
        dfa.Error(nvae38);
        throw nvae38;
    }
 

    public static readonly BitSet FOLLOW_expr2_in_expr417 = new BitSet(new ulong[]{0x00020002DF800000UL});
    public static readonly BitSet FOLLOW_n_in_expr420 = new BitSet(new ulong[]{0x0000000000000000UL});
    public static readonly BitSet FOLLOW_EOF_in_expr423 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_command_in_expr2437 = new BitSet(new ulong[]{0x0000000300000000UL});
    public static readonly BitSet FOLLOW_semi_in_expr2439 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_expr2466 = new BitSet(new ulong[]{0x0000000040000000UL});
    public static readonly BitSet FOLLOW_COMMENT_in_expr2469 = new BitSet(new ulong[]{0x0000000280000000UL});
    public static readonly BitSet FOLLOW_n_in_expr2471 = new BitSet(new ulong[]{0x0000000080000000UL});
    public static readonly BitSet FOLLOW_EOL_in_expr2474 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_expr2486 = new BitSet(new ulong[]{0x0000000080000000UL});
    public static readonly BitSet FOLLOW_EOL_in_expr2489 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_commandName_in_command528 = new BitSet(new ulong[]{0xFFFFFD5E9F800000UL,0x000000000000003FUL});
    public static readonly BitSet FOLLOW_commandOptions_in_command530 = new BitSet(new ulong[]{0xFFFFFD5E9F800000UL,0x000000000000003FUL});
    public static readonly BitSet FOLLOW_commandRest_in_command533 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_commandName606 = new BitSet(new ulong[]{0x000200021F800000UL});
    public static readonly BitSet FOLLOW_ident_in_commandName609 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_angle_in_commandOptions627 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expressionAngle_in_commandRest648 = new BitSet(new ulong[]{0xFFFFFD5E9F800002UL,0x000000000000003FUL});
    public static readonly BitSet FOLLOW_n_in_semi678 = new BitSet(new ulong[]{0x0000000100000000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_semi681 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_WHITESPACE_in_n737 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_WHITESPACE_in_n1767 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_WHITESPACE_in_n2797 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_WHITESPACE_in_n3827 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_WHITESPACE_in_n4857 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_WHITESPACE_in_n5887 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_expressionAngle907 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_expressionAngle937 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_LEFTANGLE_in_expressionAngle940 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_expressionAngle952 = new BitSet(new ulong[]{0x0000000800000000UL});
    public static readonly BitSet FOLLOW_RIGHTANGLE_in_expressionAngle955 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_paren_in_expression987 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_angle_in_expression1000 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_bracket_in_expression1013 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_curly_in_expression1026 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_term_in_expression1038 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_paren1075 = new BitSet(new ulong[]{0x0000001000000000UL});
    public static readonly BitSet FOLLOW_LEFTPAREN_in_paren1078 = new BitSet(new ulong[]{0xFFFFFD769F800000UL,0x000000000000003FUL});
    public static readonly BitSet FOLLOW_expression_in_paren1080 = new BitSet(new ulong[]{0xFFFFFD769F800000UL,0x000000000000003FUL});
    public static readonly BitSet FOLLOW_n_in_paren1083 = new BitSet(new ulong[]{0x0000002000000000UL});
    public static readonly BitSet FOLLOW_RIGHTPAREN_in_paren1086 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_angle1133 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_LEFTANGLE_in_angle1136 = new BitSet(new ulong[]{0xFFFFFD5E9F800000UL,0x000000000000003FUL});
    public static readonly BitSet FOLLOW_expression_in_angle1138 = new BitSet(new ulong[]{0xFFFFFD5E9F800000UL,0x000000000000003FUL});
    public static readonly BitSet FOLLOW_n_in_angle1141 = new BitSet(new ulong[]{0x0000000800000000UL});
    public static readonly BitSet FOLLOW_RIGHTANGLE_in_angle1144 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_bracket1188 = new BitSet(new ulong[]{0x0000004000000000UL});
    public static readonly BitSet FOLLOW_LEFTBRACKET_in_bracket1191 = new BitSet(new ulong[]{0xFFFFFDD69F800000UL,0x000000000000003FUL});
    public static readonly BitSet FOLLOW_expression_in_bracket1193 = new BitSet(new ulong[]{0xFFFFFDD69F800000UL,0x000000000000003FUL});
    public static readonly BitSet FOLLOW_n_in_bracket1196 = new BitSet(new ulong[]{0x0000008000000000UL});
    public static readonly BitSet FOLLOW_RIGHTBRACKET_in_bracket1199 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_curly1246 = new BitSet(new ulong[]{0x0000010000000000UL});
    public static readonly BitSet FOLLOW_LEFTCURLY_in_curly1249 = new BitSet(new ulong[]{0xFFFFFF569F800000UL,0x000000000000003FUL});
    public static readonly BitSet FOLLOW_expression_in_curly1251 = new BitSet(new ulong[]{0xFFFFFF569F800000UL,0x000000000000003FUL});
    public static readonly BitSet FOLLOW_n_in_curly1254 = new BitSet(new ulong[]{0x0000020000000000UL});
    public static readonly BitSet FOLLOW_RIGHTCURLY_in_curly1257 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_term1310 = new BitSet(new ulong[]{0x000200021F800000UL});
    public static readonly BitSet FOLLOW_ident_in_term1313 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_term1326 = new BitSet(new ulong[]{0xFFFC000280000000UL,0x000000000000003FUL});
    public static readonly BitSet FOLLOW_leaf_in_term1329 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_term1349 = new BitSet(new ulong[]{0x0000040000000000UL});
    public static readonly BitSet FOLLOW_StringInQuotes_in_term1352 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_term1364 = new BitSet(new ulong[]{0x0000080000000000UL});
    public static readonly BitSet FOLLOW_StringInQuotes2_in_term1367 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_term1379 = new BitSet(new ulong[]{0x0000100000000000UL});
    public static readonly BitSet FOLLOW_Integer_in_term1382 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_term1394 = new BitSet(new ulong[]{0x0000200000000000UL});
    public static readonly BitSet FOLLOW_Double_in_term1397 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_term1409 = new BitSet(new ulong[]{0x0000400000000000UL});
    public static readonly BitSet FOLLOW_DigitsEDigits_in_term1412 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_term1424 = new BitSet(new ulong[]{0x0000800000000000UL});
    public static readonly BitSet FOLLOW_DateDef_in_term1427 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_term1439 = new BitSet(new ulong[]{0x0001000000000000UL});
    public static readonly BitSet FOLLOW_IdentStartingWithInt_in_term1442 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_term1454 = new BitSet(new ulong[]{0x0000200000000000UL});
    public static readonly BitSet FOLLOW_Double_in_term1457 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_ident0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_leaf0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_commandOptions_in_synpred8_T2530 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_commandRest_in_synpred9_T2533 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_synpred13_T2907 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_synpred15_T2937 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_LEFTANGLE_in_synpred15_T2940 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_n_in_synpred44_T21394 = new BitSet(new ulong[]{0x0000200000000000UL});
    public static readonly BitSet FOLLOW_Double_in_synpred44_T21397 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}