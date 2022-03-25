// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 GAMS.g 2022-03-25 12:05:31

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
public partial class GAMSParser : Parser
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"NEGATE", 
		"ASTEQU", 
		"ASTLEFTSIDE", 
		"ASTEQUCODE", 
		"ASTEXPRESSION", 
		"ASTSIMPLEFUNCTION1", 
		"ASTSIMPLEFUNCTION2", 
		"ASTSIMPLEFUNCTION3", 
		"ASTFUNCTION", 
		"ASTINTEGER", 
		"ASTDOUBLE", 
		"ASTPOW", 
		"ASTVARIABLE", 
		"ASTEND", 
		"ASTINDEXES1", 
		"ASTINDEXES2", 
		"ASTINDEXES3", 
		"ASTEXPRESSION1", 
		"ASTEXPRESSION2", 
		"ASTEXPRESSION3", 
		"ASTDEFINITION", 
		"ASTCONDITIONAL", 
		"ASTSUM", 
		"ASTFUNCTION1", 
		"ASTFUNCTION2", 
		"ASTFUNCTION3", 
		"SUM", 
		"AND", 
		"OR", 
		"NOT", 
		"ABS", 
		"EXP", 
		"LOG", 
		"MAX", 
		"MIN", 
		"POWER", 
		"SQR", 
		"TANH", 
		"SAMEAS", 
		"DOUBLEDOT", 
		"EEQUAL", 
		"SEMI", 
		"L1", 
		"R1", 
		"L2", 
		"R2", 
		"L3", 
		"R3", 
		"COMMA", 
		"DOT", 
		"StringInQuotes", 
		"PLUS", 
		"Integer", 
		"MINUS", 
		"Double", 
		"DOLLAR", 
		"MULT", 
		"DIV", 
		"MOD", 
		"NONEQUAL", 
		"LESSTHANOREQUAL", 
		"GREATERTHANOREQUAL", 
		"EQUAL", 
		"LESSTHAN", 
		"GREATERTHAN", 
		"Ident", 
		"STARS", 
		"NEWLINE2", 
		"NEWLINE3", 
		"COMMENT1", 
		"COMMENT2", 
		"EQU", 
		"DIGIT", 
		"Exponent", 
		"LETTER", 
		"WHITESPACE", 
		"NEWLINE", 
		"Comment", 
		"NESTED_ML_COMMENT", 
		"A", 
		"B", 
		"C", 
		"D", 
		"E", 
		"F", 
		"G", 
		"H", 
		"I", 
		"J", 
		"K", 
		"L", 
		"M", 
		"N", 
		"O", 
		"P", 
		"Q", 
		"R", 
		"S", 
		"T", 
		"U", 
		"V", 
		"W", 
		"X", 
		"Y", 
		"Z"
    };

    public const int ASTPOW = 15;
    public const int COMMENT1 = 73;
    public const int COMMENT2 = 74;
    public const int ASTFUNCTION3 = 29;
    public const int ASTFUNCTION2 = 28;
    public const int ASTVARIABLE = 16;
    public const int ASTFUNCTION1 = 27;
    public const int MOD = 62;
    public const int LETTER = 78;
    public const int LOG = 36;
    public const int ASTINDEXES1 = 18;
    public const int ASTCONDITIONAL = 25;
    public const int DOUBLEDOT = 43;
    public const int NOT = 33;
    public const int ASTINDEXES3 = 20;
    public const int ASTINDEXES2 = 19;
    public const int EOF = -1;
    public const int NONEQUAL = 63;
    public const int ASTINTEGER = 13;
    public const int TANH = 41;
    public const int ASTEQU = 5;
    public const int EXP = 35;
    public const int Comment = 81;
    public const int EEQUAL = 44;
    public const int SQR = 40;
    public const int GREATERTHANOREQUAL = 65;
    public const int GREATERTHAN = 68;
    public const int Double = 58;
    public const int D = 86;
    public const int E = 87;
    public const int F = 88;
    public const int G = 89;
    public const int A = 83;
    public const int B = 84;
    public const int C = 85;
    public const int L = 94;
    public const int M = 95;
    public const int N = 96;
    public const int NESTED_ML_COMMENT = 82;
    public const int O = 97;
    public const int H = 90;
    public const int I = 91;
    public const int J = 92;
    public const int NEWLINE2 = 71;
    public const int K = 93;
    public const int NEWLINE3 = 72;
    public const int U = 103;
    public const int T = 102;
    public const int W = 105;
    public const int POWER = 39;
    public const int WHITESPACE = 79;
    public const int V = 104;
    public const int Q = 99;
    public const int P = 98;
    public const int S = 101;
    public const int R = 100;
    public const int MULT = 60;
    public const int Y = 107;
    public const int X = 106;
    public const int Z = 108;
    public const int ABS = 34;
    public const int Ident = 69;
    public const int OR = 32;
    public const int ASTEXPRESSION = 8;
    public const int StringInQuotes = 54;
    public const int ASTSUM = 26;
    public const int ASTDEFINITION = 24;
    public const int DOLLAR = 59;
    public const int ASTFUNCTION = 12;
    public const int ASTEQUCODE = 7;
    public const int MAX = 37;
    public const int Exponent = 77;
    public const int R2 = 49;
    public const int R3 = 51;
    public const int AND = 31;
    public const int SUM = 30;
    public const int COMMA = 52;
    public const int R1 = 47;
    public const int ASTSIMPLEFUNCTION1 = 9;
    public const int EQUAL = 66;
    public const int ASTSIMPLEFUNCTION2 = 10;
    public const int ASTSIMPLEFUNCTION3 = 11;
    public const int LESSTHANOREQUAL = 64;
    public const int ASTEND = 17;
    public const int PLUS = 55;
    public const int DIGIT = 76;
    public const int DOT = 53;
    public const int ASTEXPRESSION2 = 22;
    public const int ASTEXPRESSION3 = 23;
    public const int LESSTHAN = 67;
    public const int ASTEXPRESSION1 = 21;
    public const int NEGATE = 4;
    public const int SAMEAS = 42;
    public const int MIN = 38;
    public const int MINUS = 57;
    public const int SEMI = 45;
    public const int L1 = 46;
    public const int L2 = 48;
    public const int L3 = 50;
    public const int ASTLEFTSIDE = 6;
    public const int NEWLINE = 80;
    public const int EQU = 75;
    public const int STARS = 70;
    public const int ASTDOUBLE = 14;
    public const int DIV = 61;
    public const int Integer = 56;

    // delegates
    // delegators



        public GAMSParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public GAMSParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();
            this.state.ruleMemo = new Hashtable[105+1];
             
             
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
		get { return GAMSParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "GAMS.g"; }
    }


        private  System.Collections.Generic.List<string> errors = new  System.Collections.Generic.List<string>();
        private System.Collections.Generic.List<string> equItems = new System.Collections.Generic.List<string>();
        public override void DisplayRecognitionError(string[] tokenNames,
                                            RecognitionException e) {
            string hdr = GetErrorHeader(e);
            string msg = GetErrorMessage(e, tokenNames);
            errors.Add(e.Line + "¤" + e.CharPositionInLine + "¤" + hdr + "¤" + msg);
        }
        public  System.Collections.Generic.List<string> GetErrors() {
            return errors;
        }
        public System.Collections.Generic.List<string> GetEquItems() {
          return equItems;
        }    


    public class extraTokens_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "extraTokens"
    // GAMS.g:125:1: extraTokens : ( SUM | AND | OR | NOT | ABS | EXP | LOG | MAX | MIN | POWER | SQR | TANH | SAMEAS );
    public GAMSParser.extraTokens_return extraTokens() // throws RecognitionException [1]
    {   
        GAMSParser.extraTokens_return retval = new GAMSParser.extraTokens_return();
        retval.Start = input.LT(1);
        int extraTokens_StartIndex = input.Index();
        object root_0 = null;

        IToken set1 = null;

        object set1_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 1) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:125:13: ( SUM | AND | OR | NOT | ABS | EXP | LOG | MAX | MIN | POWER | SQR | TANH | SAMEAS )
            // GAMS.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set1 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= SUM && input.LA(1) <= SAMEAS) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set1));
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
            	Memoize(input, 1, extraTokens_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "extraTokens"

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
    // GAMS.g:145:1: expr : ( expr2 )+ EOF ;
    public GAMSParser.expr_return expr() // throws RecognitionException [1]
    {   
        GAMSParser.expr_return retval = new GAMSParser.expr_return();
        retval.Start = input.LT(1);
        int expr_StartIndex = input.Index();
        object root_0 = null;

        IToken EOF3 = null;
        GAMSParser.expr2_return expr22 = default(GAMSParser.expr2_return);


        object EOF3_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 2) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:145:6: ( ( expr2 )+ EOF )
            // GAMS.g:145:8: ( expr2 )+ EOF
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// GAMS.g:145:8: ( expr2 )+
            	int cnt1 = 0;
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( ((LA1_0 >= SUM && LA1_0 <= SAMEAS) || LA1_0 == Ident) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // GAMS.g:145:9: expr2
            			    {
            			    	PushFollow(FOLLOW_expr2_in_expr288);
            			    	expr22 = expr2();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expr22.Tree);

            			    }
            			    break;

            			default:
            			    if ( cnt1 >= 1 ) goto loop1;
            			    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            		            EarlyExitException eee1 =
            		                new EarlyExitException(1, input);
            		            throw eee1;
            	    }
            	    cnt1++;
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements

            	EOF3=(IToken)Match(input,EOF,FOLLOW_EOF_in_expr292); if (state.failed) return retval;
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
            	Memoize(input, 2, expr_StartIndex); 
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
    // GAMS.g:147:1: expr2 : equ ;
    public GAMSParser.expr2_return expr2() // throws RecognitionException [1]
    {   
        GAMSParser.expr2_return retval = new GAMSParser.expr2_return();
        retval.Start = input.LT(1);
        int expr2_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.equ_return equ4 = default(GAMSParser.equ_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 3) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:147:10: ( equ )
            // GAMS.g:148:3: equ
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_equ_in_expr2312);
            	equ4 = equ();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, equ4.Tree);

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
            	Memoize(input, 3, expr2_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "expr2"

    public class equ_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "equ"
    // GAMS.g:151:1: equ : variableWithIndexerEtc DOUBLEDOT expression2 EEQUAL expression2 SEMI -> ^( ASTEQU variableWithIndexerEtc expression2 expression2 ) ;
    public GAMSParser.equ_return equ() // throws RecognitionException [1]
    {   
        GAMSParser.equ_return retval = new GAMSParser.equ_return();
        retval.Start = input.LT(1);
        int equ_StartIndex = input.Index();
        object root_0 = null;

        IToken DOUBLEDOT6 = null;
        IToken EEQUAL8 = null;
        IToken SEMI10 = null;
        GAMSParser.variableWithIndexerEtc_return variableWithIndexerEtc5 = default(GAMSParser.variableWithIndexerEtc_return);

        GAMSParser.expression2_return expression27 = default(GAMSParser.expression2_return);

        GAMSParser.expression2_return expression29 = default(GAMSParser.expression2_return);


        object DOUBLEDOT6_tree=null;
        object EEQUAL8_tree=null;
        object SEMI10_tree=null;
        RewriteRuleTokenStream stream_EEQUAL = new RewriteRuleTokenStream(adaptor,"token EEQUAL");
        RewriteRuleTokenStream stream_DOUBLEDOT = new RewriteRuleTokenStream(adaptor,"token DOUBLEDOT");
        RewriteRuleTokenStream stream_SEMI = new RewriteRuleTokenStream(adaptor,"token SEMI");
        RewriteRuleSubtreeStream stream_variableWithIndexerEtc = new RewriteRuleSubtreeStream(adaptor,"rule variableWithIndexerEtc");
        RewriteRuleSubtreeStream stream_expression2 = new RewriteRuleSubtreeStream(adaptor,"rule expression2");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 4) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:151:8: ( variableWithIndexerEtc DOUBLEDOT expression2 EEQUAL expression2 SEMI -> ^( ASTEQU variableWithIndexerEtc expression2 expression2 ) )
            // GAMS.g:151:10: variableWithIndexerEtc DOUBLEDOT expression2 EEQUAL expression2 SEMI
            {
            	PushFollow(FOLLOW_variableWithIndexerEtc_in_equ329);
            	variableWithIndexerEtc5 = variableWithIndexerEtc();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableWithIndexerEtc.Add(variableWithIndexerEtc5.Tree);
            	DOUBLEDOT6=(IToken)Match(input,DOUBLEDOT,FOLLOW_DOUBLEDOT_in_equ331); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_DOUBLEDOT.Add(DOUBLEDOT6);

            	PushFollow(FOLLOW_expression2_in_equ333);
            	expression27 = expression2();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression2.Add(expression27.Tree);
            	EEQUAL8=(IToken)Match(input,EEQUAL,FOLLOW_EEQUAL_in_equ335); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_EEQUAL.Add(EEQUAL8);

            	PushFollow(FOLLOW_expression2_in_equ337);
            	expression29 = expression2();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression2.Add(expression29.Tree);
            	SEMI10=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_equ339); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI10);

            	if ( (state.backtracking==0) )
            	{
            	  equItems.Add(input.ToString((IToken)retval.Start,input.LT(-1)));
            	}


            	// AST REWRITE
            	// elements:          variableWithIndexerEtc, expression2, expression2
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 152:3: -> ^( ASTEQU variableWithIndexerEtc expression2 expression2 )
            	{
            	    // GAMS.g:152:6: ^( ASTEQU variableWithIndexerEtc expression2 expression2 )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU, "ASTEQU"), root_1);

            	    adaptor.AddChild(root_1, stream_variableWithIndexerEtc.NextTree());
            	    adaptor.AddChild(root_1, stream_expression2.NextTree());
            	    adaptor.AddChild(root_1, stream_expression2.NextTree());

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
            	Memoize(input, 4, equ_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "equ"

    public class function_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "function"
    // GAMS.g:154:1: function : ( functionName L1 functionElements R1 -> ^( ASTFUNCTION1 functionElements ) | functionName L2 functionElements R2 -> ^( ASTFUNCTION2 functionElements ) | functionName L3 functionElements R3 -> ^( ASTFUNCTION3 functionElements ) );
    public GAMSParser.function_return function() // throws RecognitionException [1]
    {   
        GAMSParser.function_return retval = new GAMSParser.function_return();
        retval.Start = input.LT(1);
        int function_StartIndex = input.Index();
        object root_0 = null;

        IToken L112 = null;
        IToken R114 = null;
        IToken L216 = null;
        IToken R218 = null;
        IToken L320 = null;
        IToken R322 = null;
        GAMSParser.functionName_return functionName11 = default(GAMSParser.functionName_return);

        GAMSParser.functionElements_return functionElements13 = default(GAMSParser.functionElements_return);

        GAMSParser.functionName_return functionName15 = default(GAMSParser.functionName_return);

        GAMSParser.functionElements_return functionElements17 = default(GAMSParser.functionElements_return);

        GAMSParser.functionName_return functionName19 = default(GAMSParser.functionName_return);

        GAMSParser.functionElements_return functionElements21 = default(GAMSParser.functionElements_return);


        object L112_tree=null;
        object R114_tree=null;
        object L216_tree=null;
        object R218_tree=null;
        object L320_tree=null;
        object R322_tree=null;
        RewriteRuleTokenStream stream_L1 = new RewriteRuleTokenStream(adaptor,"token L1");
        RewriteRuleTokenStream stream_L2 = new RewriteRuleTokenStream(adaptor,"token L2");
        RewriteRuleTokenStream stream_L3 = new RewriteRuleTokenStream(adaptor,"token L3");
        RewriteRuleTokenStream stream_R2 = new RewriteRuleTokenStream(adaptor,"token R2");
        RewriteRuleTokenStream stream_R3 = new RewriteRuleTokenStream(adaptor,"token R3");
        RewriteRuleTokenStream stream_R1 = new RewriteRuleTokenStream(adaptor,"token R1");
        RewriteRuleSubtreeStream stream_functionName = new RewriteRuleSubtreeStream(adaptor,"rule functionName");
        RewriteRuleSubtreeStream stream_functionElements = new RewriteRuleSubtreeStream(adaptor,"rule functionElements");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 5) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:154:9: ( functionName L1 functionElements R1 -> ^( ASTFUNCTION1 functionElements ) | functionName L2 functionElements R2 -> ^( ASTFUNCTION2 functionElements ) | functionName L3 functionElements R3 -> ^( ASTFUNCTION3 functionElements ) )
            int alt2 = 3;
            int LA2_0 = input.LA(1);

            if ( ((LA2_0 >= ABS && LA2_0 <= SAMEAS)) )
            {
                switch ( input.LA(2) ) 
                {
                case L2:
                	{
                    alt2 = 2;
                    }
                    break;
                case L3:
                	{
                    alt2 = 3;
                    }
                    break;
                case L1:
                	{
                    alt2 = 1;
                    }
                    break;
                	default:
                	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                	    NoViableAltException nvae_d2s1 =
                	        new NoViableAltException("", 2, 1, input);

                	    throw nvae_d2s1;
                }

            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d2s0 =
                    new NoViableAltException("", 2, 0, input);

                throw nvae_d2s0;
            }
            switch (alt2) 
            {
                case 1 :
                    // GAMS.g:154:15: functionName L1 functionElements R1
                    {
                    	PushFollow(FOLLOW_functionName_in_function367);
                    	functionName11 = functionName();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionName.Add(functionName11.Tree);
                    	L112=(IToken)Match(input,L1,FOLLOW_L1_in_function369); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L112);

                    	PushFollow(FOLLOW_functionElements_in_function371);
                    	functionElements13 = functionElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionElements.Add(functionElements13.Tree);
                    	R114=(IToken)Match(input,R1,FOLLOW_R1_in_function373); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R114);



                    	// AST REWRITE
                    	// elements:          functionElements
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 154:51: -> ^( ASTFUNCTION1 functionElements )
                    	{
                    	    // GAMS.g:154:54: ^( ASTFUNCTION1 functionElements )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION1, "ASTFUNCTION1"), root_1);

                    	    adaptor.AddChild(root_1, stream_functionElements.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // GAMS.g:155:15: functionName L2 functionElements R2
                    {
                    	PushFollow(FOLLOW_functionName_in_function397);
                    	functionName15 = functionName();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionName.Add(functionName15.Tree);
                    	L216=(IToken)Match(input,L2,FOLLOW_L2_in_function399); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L216);

                    	PushFollow(FOLLOW_functionElements_in_function401);
                    	functionElements17 = functionElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionElements.Add(functionElements17.Tree);
                    	R218=(IToken)Match(input,R2,FOLLOW_R2_in_function403); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R218);



                    	// AST REWRITE
                    	// elements:          functionElements
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 155:51: -> ^( ASTFUNCTION2 functionElements )
                    	{
                    	    // GAMS.g:155:54: ^( ASTFUNCTION2 functionElements )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION2, "ASTFUNCTION2"), root_1);

                    	    adaptor.AddChild(root_1, stream_functionElements.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 3 :
                    // GAMS.g:156:15: functionName L3 functionElements R3
                    {
                    	PushFollow(FOLLOW_functionName_in_function427);
                    	functionName19 = functionName();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionName.Add(functionName19.Tree);
                    	L320=(IToken)Match(input,L3,FOLLOW_L3_in_function429); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L320);

                    	PushFollow(FOLLOW_functionElements_in_function431);
                    	functionElements21 = functionElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionElements.Add(functionElements21.Tree);
                    	R322=(IToken)Match(input,R3,FOLLOW_R3_in_function433); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R322);



                    	// AST REWRITE
                    	// elements:          functionElements
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 156:51: -> ^( ASTFUNCTION3 functionElements )
                    	{
                    	    // GAMS.g:156:54: ^( ASTFUNCTION3 functionElements )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION3, "ASTFUNCTION3"), root_1);

                    	    adaptor.AddChild(root_1, stream_functionElements.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
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
            	Memoize(input, 5, function_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "function"

    public class functionName_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "functionName"
    // GAMS.g:158:1: functionName : ( ABS | EXP | LOG | MAX | MIN | POWER | SQR | SAMEAS | TANH );
    public GAMSParser.functionName_return functionName() // throws RecognitionException [1]
    {   
        GAMSParser.functionName_return retval = new GAMSParser.functionName_return();
        retval.Start = input.LT(1);
        int functionName_StartIndex = input.Index();
        object root_0 = null;

        IToken set23 = null;

        object set23_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 6) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:158:13: ( ABS | EXP | LOG | MAX | MIN | POWER | SQR | SAMEAS | TANH )
            // GAMS.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set23 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= ABS && input.LA(1) <= SAMEAS) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set23));
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
            	Memoize(input, 6, functionName_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "functionName"

    public class functionElements_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "functionElements"
    // GAMS.g:160:1: functionElements : expression ( COMMA expression )* -> ( expression )+ ;
    public GAMSParser.functionElements_return functionElements() // throws RecognitionException [1]
    {   
        GAMSParser.functionElements_return retval = new GAMSParser.functionElements_return();
        retval.Start = input.LT(1);
        int functionElements_StartIndex = input.Index();
        object root_0 = null;

        IToken COMMA25 = null;
        GAMSParser.expression_return expression24 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression26 = default(GAMSParser.expression_return);


        object COMMA25_tree=null;
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 7) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:160:17: ( expression ( COMMA expression )* -> ( expression )+ )
            // GAMS.g:160:19: expression ( COMMA expression )*
            {
            	PushFollow(FOLLOW_expression_in_functionElements489);
            	expression24 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression24.Tree);
            	// GAMS.g:160:30: ( COMMA expression )*
            	do 
            	{
            	    int alt3 = 2;
            	    int LA3_0 = input.LA(1);

            	    if ( (LA3_0 == COMMA) )
            	    {
            	        alt3 = 1;
            	    }


            	    switch (alt3) 
            		{
            			case 1 :
            			    // GAMS.g:160:31: COMMA expression
            			    {
            			    	COMMA25=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_functionElements492); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA25);

            			    	PushFollow(FOLLOW_expression_in_functionElements494);
            			    	expression26 = expression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_expression.Add(expression26.Tree);

            			    }
            			    break;

            			default:
            			    goto loop3;
            	    }
            	} while (true);

            	loop3:
            		;	// Stops C# compiler whining that label 'loop3' has no statements



            	// AST REWRITE
            	// elements:          expression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 160:50: -> ( expression )+
            	{
            	    if ( !(stream_expression.HasNext()) ) {
            	        throw new RewriteEarlyExitException();
            	    }
            	    while ( stream_expression.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_expression.NextTree());

            	    }
            	    stream_expression.Reset();

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
            	Memoize(input, 7, functionElements_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "functionElements"

    public class variableWithIndexerEtc_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variableWithIndexerEtc"
    // GAMS.g:162:1: variableWithIndexerEtc : variable ( DOT variable )? ( idx )? ( conditional )? ;
    public GAMSParser.variableWithIndexerEtc_return variableWithIndexerEtc() // throws RecognitionException [1]
    {   
        GAMSParser.variableWithIndexerEtc_return retval = new GAMSParser.variableWithIndexerEtc_return();
        retval.Start = input.LT(1);
        int variableWithIndexerEtc_StartIndex = input.Index();
        object root_0 = null;

        IToken DOT28 = null;
        GAMSParser.variable_return variable27 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable29 = default(GAMSParser.variable_return);

        GAMSParser.idx_return idx30 = default(GAMSParser.idx_return);

        GAMSParser.conditional_return conditional31 = default(GAMSParser.conditional_return);


        object DOT28_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 8) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:162:23: ( variable ( DOT variable )? ( idx )? ( conditional )? )
            // GAMS.g:163:3: variable ( DOT variable )? ( idx )? ( conditional )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_variable_in_variableWithIndexerEtc511);
            	variable27 = variable();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable27.Tree);
            	// GAMS.g:163:12: ( DOT variable )?
            	int alt4 = 2;
            	alt4 = dfa4.Predict(input);
            	switch (alt4) 
            	{
            	    case 1 :
            	        // GAMS.g:163:13: DOT variable
            	        {
            	        	DOT28=(IToken)Match(input,DOT,FOLLOW_DOT_in_variableWithIndexerEtc514); if (state.failed) return retval;
            	        	if ( state.backtracking == 0 )
            	        	{DOT28_tree = (object)adaptor.Create(DOT28);
            	        		adaptor.AddChild(root_0, DOT28_tree);
            	        	}
            	        	PushFollow(FOLLOW_variable_in_variableWithIndexerEtc516);
            	        	variable29 = variable();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable29.Tree);

            	        }
            	        break;

            	}

            	// GAMS.g:163:28: ( idx )?
            	int alt5 = 2;
            	alt5 = dfa5.Predict(input);
            	switch (alt5) 
            	{
            	    case 1 :
            	        // GAMS.g:0:0: idx
            	        {
            	        	PushFollow(FOLLOW_idx_in_variableWithIndexerEtc520);
            	        	idx30 = idx();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, idx30.Tree);

            	        }
            	        break;

            	}

            	// GAMS.g:163:33: ( conditional )?
            	int alt6 = 2;
            	alt6 = dfa6.Predict(input);
            	switch (alt6) 
            	{
            	    case 1 :
            	        // GAMS.g:0:0: conditional
            	        {
            	        	PushFollow(FOLLOW_conditional_in_variableWithIndexerEtc523);
            	        	conditional31 = conditional();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, conditional31.Tree);

            	        }
            	        break;

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
            	Memoize(input, 8, variableWithIndexerEtc_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "variableWithIndexerEtc"

    public class idx_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "idx"
    // GAMS.g:166:1: idx : ( L1 indexerElements R1 | L2 indexerElements R2 | L3 indexerElements R3 );
    public GAMSParser.idx_return idx() // throws RecognitionException [1]
    {   
        GAMSParser.idx_return retval = new GAMSParser.idx_return();
        retval.Start = input.LT(1);
        int idx_StartIndex = input.Index();
        object root_0 = null;

        IToken L132 = null;
        IToken R134 = null;
        IToken L235 = null;
        IToken R237 = null;
        IToken L338 = null;
        IToken R340 = null;
        GAMSParser.indexerElements_return indexerElements33 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements36 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements39 = default(GAMSParser.indexerElements_return);


        object L132_tree=null;
        object R134_tree=null;
        object L235_tree=null;
        object R237_tree=null;
        object L338_tree=null;
        object R340_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 9) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:166:4: ( L1 indexerElements R1 | L2 indexerElements R2 | L3 indexerElements R3 )
            int alt7 = 3;
            switch ( input.LA(1) ) 
            {
            case L1:
            	{
                alt7 = 1;
                }
                break;
            case L2:
            	{
                alt7 = 2;
                }
                break;
            case L3:
            	{
                alt7 = 3;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d7s0 =
            	        new NoViableAltException("", 7, 0, input);

            	    throw nvae_d7s0;
            }

            switch (alt7) 
            {
                case 1 :
                    // GAMS.g:166:6: L1 indexerElements R1
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	L132=(IToken)Match(input,L1,FOLLOW_L1_in_idx532); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L132_tree = (object)adaptor.Create(L132);
                    		adaptor.AddChild(root_0, L132_tree);
                    	}
                    	PushFollow(FOLLOW_indexerElements_in_idx534);
                    	indexerElements33 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, indexerElements33.Tree);
                    	R134=(IToken)Match(input,R1,FOLLOW_R1_in_idx536); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R134_tree = (object)adaptor.Create(R134);
                    		adaptor.AddChild(root_0, R134_tree);
                    	}

                    }
                    break;
                case 2 :
                    // GAMS.g:167:6: L2 indexerElements R2
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	L235=(IToken)Match(input,L2,FOLLOW_L2_in_idx543); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L235_tree = (object)adaptor.Create(L235);
                    		adaptor.AddChild(root_0, L235_tree);
                    	}
                    	PushFollow(FOLLOW_indexerElements_in_idx545);
                    	indexerElements36 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, indexerElements36.Tree);
                    	R237=(IToken)Match(input,R2,FOLLOW_R2_in_idx547); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R237_tree = (object)adaptor.Create(R237);
                    		adaptor.AddChild(root_0, R237_tree);
                    	}

                    }
                    break;
                case 3 :
                    // GAMS.g:168:6: L3 indexerElements R3
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	L338=(IToken)Match(input,L3,FOLLOW_L3_in_idx554); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L338_tree = (object)adaptor.Create(L338);
                    		adaptor.AddChild(root_0, L338_tree);
                    	}
                    	PushFollow(FOLLOW_indexerElements_in_idx556);
                    	indexerElements39 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, indexerElements39.Tree);
                    	R340=(IToken)Match(input,R3,FOLLOW_R3_in_idx558); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R340_tree = (object)adaptor.Create(R340);
                    		adaptor.AddChild(root_0, R340_tree);
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
            	Memoize(input, 9, idx_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "idx"

    public class indexerElements_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "indexerElements"
    // GAMS.g:189:1: indexerElements : variableLagLead ( COMMA variableLagLead )* -> ( variableLagLead )+ ;
    public GAMSParser.indexerElements_return indexerElements() // throws RecognitionException [1]
    {   
        GAMSParser.indexerElements_return retval = new GAMSParser.indexerElements_return();
        retval.Start = input.LT(1);
        int indexerElements_StartIndex = input.Index();
        object root_0 = null;

        IToken COMMA42 = null;
        GAMSParser.variableLagLead_return variableLagLead41 = default(GAMSParser.variableLagLead_return);

        GAMSParser.variableLagLead_return variableLagLead43 = default(GAMSParser.variableLagLead_return);


        object COMMA42_tree=null;
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleSubtreeStream stream_variableLagLead = new RewriteRuleSubtreeStream(adaptor,"rule variableLagLead");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 10) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:189:16: ( variableLagLead ( COMMA variableLagLead )* -> ( variableLagLead )+ )
            // GAMS.g:189:18: variableLagLead ( COMMA variableLagLead )*
            {
            	PushFollow(FOLLOW_variableLagLead_in_indexerElements584);
            	variableLagLead41 = variableLagLead();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableLagLead.Add(variableLagLead41.Tree);
            	// GAMS.g:189:34: ( COMMA variableLagLead )*
            	do 
            	{
            	    int alt8 = 2;
            	    int LA8_0 = input.LA(1);

            	    if ( (LA8_0 == COMMA) )
            	    {
            	        alt8 = 1;
            	    }


            	    switch (alt8) 
            		{
            			case 1 :
            			    // GAMS.g:189:35: COMMA variableLagLead
            			    {
            			    	COMMA42=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_indexerElements587); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA42);

            			    	PushFollow(FOLLOW_variableLagLead_in_indexerElements589);
            			    	variableLagLead43 = variableLagLead();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_variableLagLead.Add(variableLagLead43.Tree);

            			    }
            			    break;

            			default:
            			    goto loop8;
            	    }
            	} while (true);

            	loop8:
            		;	// Stops C# compiler whining that label 'loop8' has no statements



            	// AST REWRITE
            	// elements:          variableLagLead
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 189:59: -> ( variableLagLead )+
            	{
            	    if ( !(stream_variableLagLead.HasNext()) ) {
            	        throw new RewriteEarlyExitException();
            	    }
            	    while ( stream_variableLagLead.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_variableLagLead.NextTree());

            	    }
            	    stream_variableLagLead.Reset();

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
            	Memoize(input, 10, indexerElements_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "indexerElements"

    public class variableLagLead_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variableLagLead"
    // GAMS.g:190:1: variableLagLead : ( StringInQuotes | variable | variable PLUS Integer | variable MINUS Integer );
    public GAMSParser.variableLagLead_return variableLagLead() // throws RecognitionException [1]
    {   
        GAMSParser.variableLagLead_return retval = new GAMSParser.variableLagLead_return();
        retval.Start = input.LT(1);
        int variableLagLead_StartIndex = input.Index();
        object root_0 = null;

        IToken StringInQuotes44 = null;
        IToken PLUS47 = null;
        IToken Integer48 = null;
        IToken MINUS50 = null;
        IToken Integer51 = null;
        GAMSParser.variable_return variable45 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable46 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable49 = default(GAMSParser.variable_return);


        object StringInQuotes44_tree=null;
        object PLUS47_tree=null;
        object Integer48_tree=null;
        object MINUS50_tree=null;
        object Integer51_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 11) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:190:16: ( StringInQuotes | variable | variable PLUS Integer | variable MINUS Integer )
            int alt9 = 4;
            alt9 = dfa9.Predict(input);
            switch (alt9) 
            {
                case 1 :
                    // GAMS.g:190:18: StringInQuotes
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	StringInQuotes44=(IToken)Match(input,StringInQuotes,FOLLOW_StringInQuotes_in_variableLagLead603); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{StringInQuotes44_tree = (object)adaptor.Create(StringInQuotes44);
                    		adaptor.AddChild(root_0, StringInQuotes44_tree);
                    	}

                    }
                    break;
                case 2 :
                    // GAMS.g:190:35: variable
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variable_in_variableLagLead607);
                    	variable45 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable45.Tree);

                    }
                    break;
                case 3 :
                    // GAMS.g:190:46: variable PLUS Integer
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variable_in_variableLagLead611);
                    	variable46 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable46.Tree);
                    	PLUS47=(IToken)Match(input,PLUS,FOLLOW_PLUS_in_variableLagLead613); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{PLUS47_tree = (object)adaptor.Create(PLUS47);
                    		adaptor.AddChild(root_0, PLUS47_tree);
                    	}
                    	Integer48=(IToken)Match(input,Integer,FOLLOW_Integer_in_variableLagLead615); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Integer48_tree = (object)adaptor.Create(Integer48);
                    		adaptor.AddChild(root_0, Integer48_tree);
                    	}

                    }
                    break;
                case 4 :
                    // GAMS.g:190:70: variable MINUS Integer
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variable_in_variableLagLead619);
                    	variable49 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable49.Tree);
                    	MINUS50=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_variableLagLead621); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{MINUS50_tree = (object)adaptor.Create(MINUS50);
                    		adaptor.AddChild(root_0, MINUS50_tree);
                    	}
                    	Integer51=(IToken)Match(input,Integer,FOLLOW_Integer_in_variableLagLead623); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Integer51_tree = (object)adaptor.Create(Integer51);
                    		adaptor.AddChild(root_0, Integer51_tree);
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
            	Memoize(input, 11, variableLagLead_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "variableLagLead"

    public class expression2_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expression2"
    // GAMS.g:192:1: expression2 : expression -> ^( ASTEXPRESSION expression ) ;
    public GAMSParser.expression2_return expression2() // throws RecognitionException [1]
    {   
        GAMSParser.expression2_return retval = new GAMSParser.expression2_return();
        retval.Start = input.LT(1);
        int expression2_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.expression_return expression52 = default(GAMSParser.expression_return);


        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 12) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:192:13: ( expression -> ^( ASTEXPRESSION expression ) )
            // GAMS.g:192:15: expression
            {
            	PushFollow(FOLLOW_expression_in_expression2632);
            	expression52 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression52.Tree);


            	// AST REWRITE
            	// elements:          expression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 192:26: -> ^( ASTEXPRESSION expression )
            	{
            	    // GAMS.g:192:29: ^( ASTEXPRESSION expression )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEXPRESSION, "ASTEXPRESSION"), root_1);

            	    adaptor.AddChild(root_1, stream_expression.NextTree());

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
            	Memoize(input, 12, expression2_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "expression2"

    public class number_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "number"
    // GAMS.g:193:1: number : ( Double | Integer ) ;
    public GAMSParser.number_return number() // throws RecognitionException [1]
    {   
        GAMSParser.number_return retval = new GAMSParser.number_return();
        retval.Start = input.LT(1);
        int number_StartIndex = input.Index();
        object root_0 = null;

        IToken set53 = null;

        object set53_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 13) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:193:7: ( ( Double | Integer ) )
            // GAMS.g:193:9: ( Double | Integer )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set53 = (IToken)input.LT(1);
            	if ( input.LA(1) == Integer || input.LA(1) == Double ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set53));
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
            	Memoize(input, 13, number_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "number"

    public class conditional_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "conditional"
    // GAMS.g:195:1: conditional : DOLLAR expression ;
    public GAMSParser.conditional_return conditional() // throws RecognitionException [1]
    {   
        GAMSParser.conditional_return retval = new GAMSParser.conditional_return();
        retval.Start = input.LT(1);
        int conditional_StartIndex = input.Index();
        object root_0 = null;

        IToken DOLLAR54 = null;
        GAMSParser.expression_return expression55 = default(GAMSParser.expression_return);


        object DOLLAR54_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 14) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:195:12: ( DOLLAR expression )
            // GAMS.g:195:14: DOLLAR expression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	DOLLAR54=(IToken)Match(input,DOLLAR,FOLLOW_DOLLAR_in_conditional657); if (state.failed) return retval;
            	if ( state.backtracking == 0 )
            	{DOLLAR54_tree = (object)adaptor.Create(DOLLAR54);
            		adaptor.AddChild(root_0, DOLLAR54_tree);
            	}
            	PushFollow(FOLLOW_expression_in_conditional659);
            	expression55 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression55.Tree);

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
            	Memoize(input, 14, conditional_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "conditional"

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
    // GAMS.g:213:1: expression : andExpression ( OR andExpression )* ;
    public GAMSParser.expression_return expression() // throws RecognitionException [1]
    {   
        GAMSParser.expression_return retval = new GAMSParser.expression_return();
        retval.Start = input.LT(1);
        int expression_StartIndex = input.Index();
        object root_0 = null;

        IToken OR57 = null;
        GAMSParser.andExpression_return andExpression56 = default(GAMSParser.andExpression_return);

        GAMSParser.andExpression_return andExpression58 = default(GAMSParser.andExpression_return);


        object OR57_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 15) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:213:11: ( andExpression ( OR andExpression )* )
            // GAMS.g:213:13: andExpression ( OR andExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_andExpression_in_expression682);
            	andExpression56 = andExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, andExpression56.Tree);
            	// GAMS.g:213:27: ( OR andExpression )*
            	do 
            	{
            	    int alt10 = 2;
            	    alt10 = dfa10.Predict(input);
            	    switch (alt10) 
            		{
            			case 1 :
            			    // GAMS.g:213:28: OR andExpression
            			    {
            			    	OR57=(IToken)Match(input,OR,FOLLOW_OR_in_expression685); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{OR57_tree = (object)adaptor.Create(OR57);
            			    		root_0 = (object)adaptor.BecomeRoot(OR57_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_andExpression_in_expression688);
            			    	andExpression58 = andExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, andExpression58.Tree);

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
            	Memoize(input, 15, expression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "expression"

    public class andExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "andExpression"
    // GAMS.g:214:1: andExpression : notExpression ( AND notExpression )* ;
    public GAMSParser.andExpression_return andExpression() // throws RecognitionException [1]
    {   
        GAMSParser.andExpression_return retval = new GAMSParser.andExpression_return();
        retval.Start = input.LT(1);
        int andExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken AND60 = null;
        GAMSParser.notExpression_return notExpression59 = default(GAMSParser.notExpression_return);

        GAMSParser.notExpression_return notExpression61 = default(GAMSParser.notExpression_return);


        object AND60_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 16) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:214:14: ( notExpression ( AND notExpression )* )
            // GAMS.g:214:16: notExpression ( AND notExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_notExpression_in_andExpression698);
            	notExpression59 = notExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, notExpression59.Tree);
            	// GAMS.g:214:30: ( AND notExpression )*
            	do 
            	{
            	    int alt11 = 2;
            	    alt11 = dfa11.Predict(input);
            	    switch (alt11) 
            		{
            			case 1 :
            			    // GAMS.g:214:31: AND notExpression
            			    {
            			    	AND60=(IToken)Match(input,AND,FOLLOW_AND_in_andExpression701); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{AND60_tree = (object)adaptor.Create(AND60);
            			    		root_0 = (object)adaptor.BecomeRoot(AND60_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_notExpression_in_andExpression704);
            			    	notExpression61 = notExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, notExpression61.Tree);

            			    }
            			    break;

            			default:
            			    goto loop11;
            	    }
            	} while (true);

            	loop11:
            		;	// Stops C# compiler whining that label 'loop11' has no statements


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
            	Memoize(input, 16, andExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "andExpression"

    public class notExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "notExpression"
    // GAMS.g:215:1: notExpression : ( logicalExpression | NOT logicalExpression -> ^( NOT logicalExpression ) );
    public GAMSParser.notExpression_return notExpression() // throws RecognitionException [1]
    {   
        GAMSParser.notExpression_return retval = new GAMSParser.notExpression_return();
        retval.Start = input.LT(1);
        int notExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken NOT63 = null;
        GAMSParser.logicalExpression_return logicalExpression62 = default(GAMSParser.logicalExpression_return);

        GAMSParser.logicalExpression_return logicalExpression64 = default(GAMSParser.logicalExpression_return);


        object NOT63_tree=null;
        RewriteRuleTokenStream stream_NOT = new RewriteRuleTokenStream(adaptor,"token NOT");
        RewriteRuleSubtreeStream stream_logicalExpression = new RewriteRuleSubtreeStream(adaptor,"rule logicalExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 17) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:215:14: ( logicalExpression | NOT logicalExpression -> ^( NOT logicalExpression ) )
            int alt12 = 2;
            alt12 = dfa12.Predict(input);
            switch (alt12) 
            {
                case 1 :
                    // GAMS.g:215:16: logicalExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_logicalExpression_in_notExpression714);
                    	logicalExpression62 = logicalExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, logicalExpression62.Tree);

                    }
                    break;
                case 2 :
                    // GAMS.g:216:10: NOT logicalExpression
                    {
                    	NOT63=(IToken)Match(input,NOT,FOLLOW_NOT_in_notExpression726); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_NOT.Add(NOT63);

                    	PushFollow(FOLLOW_logicalExpression_in_notExpression728);
                    	logicalExpression64 = logicalExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_logicalExpression.Add(logicalExpression64.Tree);


                    	// AST REWRITE
                    	// elements:          NOT, logicalExpression
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 216:32: -> ^( NOT logicalExpression )
                    	{
                    	    // GAMS.g:216:35: ^( NOT logicalExpression )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot(stream_NOT.NextNode(), root_1);

                    	    adaptor.AddChild(root_1, stream_logicalExpression.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
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
            	Memoize(input, 17, notExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "notExpression"

    public class logicalExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "logicalExpression"
    // GAMS.g:217:1: logicalExpression : additiveExpression ( logical additiveExpression )* ;
    public GAMSParser.logicalExpression_return logicalExpression() // throws RecognitionException [1]
    {   
        GAMSParser.logicalExpression_return retval = new GAMSParser.logicalExpression_return();
        retval.Start = input.LT(1);
        int logicalExpression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.additiveExpression_return additiveExpression65 = default(GAMSParser.additiveExpression_return);

        GAMSParser.logical_return logical66 = default(GAMSParser.logical_return);

        GAMSParser.additiveExpression_return additiveExpression67 = default(GAMSParser.additiveExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 18) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:217:18: ( additiveExpression ( logical additiveExpression )* )
            // GAMS.g:217:21: additiveExpression ( logical additiveExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_additiveExpression_in_logicalExpression743);
            	additiveExpression65 = additiveExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression65.Tree);
            	// GAMS.g:217:40: ( logical additiveExpression )*
            	do 
            	{
            	    int alt13 = 2;
            	    alt13 = dfa13.Predict(input);
            	    switch (alt13) 
            		{
            			case 1 :
            			    // GAMS.g:217:41: logical additiveExpression
            			    {
            			    	PushFollow(FOLLOW_logical_in_logicalExpression746);
            			    	logical66 = logical();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot(logical66.Tree, root_0);
            			    	PushFollow(FOLLOW_additiveExpression_in_logicalExpression749);
            			    	additiveExpression67 = additiveExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression67.Tree);

            			    }
            			    break;

            			default:
            			    goto loop13;
            	    }
            	} while (true);

            	loop13:
            		;	// Stops C# compiler whining that label 'loop13' has no statements


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
            	Memoize(input, 18, logicalExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "logicalExpression"

    public class additiveExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "additiveExpression"
    // GAMS.g:219:1: additiveExpression : multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* ;
    public GAMSParser.additiveExpression_return additiveExpression() // throws RecognitionException [1]
    {   
        GAMSParser.additiveExpression_return retval = new GAMSParser.additiveExpression_return();
        retval.Start = input.LT(1);
        int additiveExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set69 = null;
        GAMSParser.multiplicativeExpression_return multiplicativeExpression68 = default(GAMSParser.multiplicativeExpression_return);

        GAMSParser.multiplicativeExpression_return multiplicativeExpression70 = default(GAMSParser.multiplicativeExpression_return);


        object set69_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 19) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:219:21: ( multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* )
            // GAMS.g:219:23: multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression760);
            	multiplicativeExpression68 = multiplicativeExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression68.Tree);
            	// GAMS.g:219:48: ( ( PLUS | MINUS ) multiplicativeExpression )*
            	do 
            	{
            	    int alt14 = 2;
            	    alt14 = dfa14.Predict(input);
            	    switch (alt14) 
            		{
            			case 1 :
            			    // GAMS.g:219:50: ( PLUS | MINUS ) multiplicativeExpression
            			    {
            			    	set69=(IToken)input.LT(1);
            			    	set69 = (IToken)input.LT(1);
            			    	if ( input.LA(1) == PLUS || input.LA(1) == MINUS ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set69), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression771);
            			    	multiplicativeExpression70 = multiplicativeExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression70.Tree);

            			    }
            			    break;

            			default:
            			    goto loop14;
            	    }
            	} while (true);

            	loop14:
            		;	// Stops C# compiler whining that label 'loop14' has no statements


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
            	Memoize(input, 19, additiveExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "additiveExpression"

    public class multiplicativeExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "multiplicativeExpression"
    // GAMS.g:221:1: multiplicativeExpression : powerExpression ( ( MULT | DIV | MOD ) powerExpression )* ;
    public GAMSParser.multiplicativeExpression_return multiplicativeExpression() // throws RecognitionException [1]
    {   
        GAMSParser.multiplicativeExpression_return retval = new GAMSParser.multiplicativeExpression_return();
        retval.Start = input.LT(1);
        int multiplicativeExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set72 = null;
        GAMSParser.powerExpression_return powerExpression71 = default(GAMSParser.powerExpression_return);

        GAMSParser.powerExpression_return powerExpression73 = default(GAMSParser.powerExpression_return);


        object set72_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 20) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:221:28: ( powerExpression ( ( MULT | DIV | MOD ) powerExpression )* )
            // GAMS.g:221:30: powerExpression ( ( MULT | DIV | MOD ) powerExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression784);
            	powerExpression71 = powerExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression71.Tree);
            	// GAMS.g:221:46: ( ( MULT | DIV | MOD ) powerExpression )*
            	do 
            	{
            	    int alt15 = 2;
            	    alt15 = dfa15.Predict(input);
            	    switch (alt15) 
            		{
            			case 1 :
            			    // GAMS.g:221:48: ( MULT | DIV | MOD ) powerExpression
            			    {
            			    	set72=(IToken)input.LT(1);
            			    	set72 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= MULT && input.LA(1) <= MOD) ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set72), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression797);
            			    	powerExpression73 = powerExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression73.Tree);

            			    }
            			    break;

            			default:
            			    goto loop15;
            	    }
            	} while (true);

            	loop15:
            		;	// Stops C# compiler whining that label 'loop15' has no statements


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
            	Memoize(input, 20, multiplicativeExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "multiplicativeExpression"

    public class powerExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "powerExpression"
    // GAMS.g:223:1: powerExpression : unaryExpression ( pow unaryExpression )* ;
    public GAMSParser.powerExpression_return powerExpression() // throws RecognitionException [1]
    {   
        GAMSParser.powerExpression_return retval = new GAMSParser.powerExpression_return();
        retval.Start = input.LT(1);
        int powerExpression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.unaryExpression_return unaryExpression74 = default(GAMSParser.unaryExpression_return);

        GAMSParser.pow_return pow75 = default(GAMSParser.pow_return);

        GAMSParser.unaryExpression_return unaryExpression76 = default(GAMSParser.unaryExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 21) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:223:19: ( unaryExpression ( pow unaryExpression )* )
            // GAMS.g:223:21: unaryExpression ( pow unaryExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_unaryExpression_in_powerExpression810);
            	unaryExpression74 = unaryExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression74.Tree);
            	// GAMS.g:223:37: ( pow unaryExpression )*
            	do 
            	{
            	    int alt16 = 2;
            	    alt16 = dfa16.Predict(input);
            	    switch (alt16) 
            		{
            			case 1 :
            			    // GAMS.g:223:39: pow unaryExpression
            			    {
            			    	PushFollow(FOLLOW_pow_in_powerExpression814);
            			    	pow75 = pow();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot(pow75.Tree, root_0);
            			    	PushFollow(FOLLOW_unaryExpression_in_powerExpression817);
            			    	unaryExpression76 = unaryExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression76.Tree);

            			    }
            			    break;

            			default:
            			    goto loop16;
            	    }
            	} while (true);

            	loop16:
            		;	// Stops C# compiler whining that label 'loop16' has no statements


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
            	Memoize(input, 21, powerExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "powerExpression"

    public class unaryExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "unaryExpression"
    // GAMS.g:225:1: unaryExpression : ( dollarExpression | MINUS dollarExpression -> ^( NEGATE dollarExpression ) );
    public GAMSParser.unaryExpression_return unaryExpression() // throws RecognitionException [1]
    {   
        GAMSParser.unaryExpression_return retval = new GAMSParser.unaryExpression_return();
        retval.Start = input.LT(1);
        int unaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken MINUS78 = null;
        GAMSParser.dollarExpression_return dollarExpression77 = default(GAMSParser.dollarExpression_return);

        GAMSParser.dollarExpression_return dollarExpression79 = default(GAMSParser.dollarExpression_return);


        object MINUS78_tree=null;
        RewriteRuleTokenStream stream_MINUS = new RewriteRuleTokenStream(adaptor,"token MINUS");
        RewriteRuleSubtreeStream stream_dollarExpression = new RewriteRuleSubtreeStream(adaptor,"rule dollarExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 22) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:225:19: ( dollarExpression | MINUS dollarExpression -> ^( NEGATE dollarExpression ) )
            int alt17 = 2;
            alt17 = dfa17.Predict(input);
            switch (alt17) 
            {
                case 1 :
                    // GAMS.g:225:21: dollarExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_dollarExpression_in_unaryExpression831);
                    	dollarExpression77 = dollarExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, dollarExpression77.Tree);

                    }
                    break;
                case 2 :
                    // GAMS.g:226:10: MINUS dollarExpression
                    {
                    	MINUS78=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_unaryExpression842); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_MINUS.Add(MINUS78);

                    	PushFollow(FOLLOW_dollarExpression_in_unaryExpression844);
                    	dollarExpression79 = dollarExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_dollarExpression.Add(dollarExpression79.Tree);


                    	// AST REWRITE
                    	// elements:          dollarExpression
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 226:33: -> ^( NEGATE dollarExpression )
                    	{
                    	    // GAMS.g:226:36: ^( NEGATE dollarExpression )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(NEGATE, "NEGATE"), root_1);

                    	    adaptor.AddChild(root_1, stream_dollarExpression.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
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
            	Memoize(input, 22, unaryExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "unaryExpression"

    public class dollarExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "dollarExpression"
    // GAMS.g:228:1: dollarExpression : primaryExpression ( conditional )? -> ^( ASTCONDITIONAL primaryExpression ( conditional )? ) ;
    public GAMSParser.dollarExpression_return dollarExpression() // throws RecognitionException [1]
    {   
        GAMSParser.dollarExpression_return retval = new GAMSParser.dollarExpression_return();
        retval.Start = input.LT(1);
        int dollarExpression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.primaryExpression_return primaryExpression80 = default(GAMSParser.primaryExpression_return);

        GAMSParser.conditional_return conditional81 = default(GAMSParser.conditional_return);


        RewriteRuleSubtreeStream stream_conditional = new RewriteRuleSubtreeStream(adaptor,"rule conditional");
        RewriteRuleSubtreeStream stream_primaryExpression = new RewriteRuleSubtreeStream(adaptor,"rule primaryExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 23) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:228:17: ( primaryExpression ( conditional )? -> ^( ASTCONDITIONAL primaryExpression ( conditional )? ) )
            // GAMS.g:228:19: primaryExpression ( conditional )?
            {
            	PushFollow(FOLLOW_primaryExpression_in_dollarExpression863);
            	primaryExpression80 = primaryExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_primaryExpression.Add(primaryExpression80.Tree);
            	// GAMS.g:228:37: ( conditional )?
            	int alt18 = 2;
            	alt18 = dfa18.Predict(input);
            	switch (alt18) 
            	{
            	    case 1 :
            	        // GAMS.g:0:0: conditional
            	        {
            	        	PushFollow(FOLLOW_conditional_in_dollarExpression865);
            	        	conditional81 = conditional();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional81.Tree);

            	        }
            	        break;

            	}



            	// AST REWRITE
            	// elements:          conditional, primaryExpression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 228:50: -> ^( ASTCONDITIONAL primaryExpression ( conditional )? )
            	{
            	    // GAMS.g:228:53: ^( ASTCONDITIONAL primaryExpression ( conditional )? )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTCONDITIONAL, "ASTCONDITIONAL"), root_1);

            	    adaptor.AddChild(root_1, stream_primaryExpression.NextTree());
            	    // GAMS.g:228:88: ( conditional )?
            	    if ( stream_conditional.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_conditional.NextTree());

            	    }
            	    stream_conditional.Reset();

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
            	Memoize(input, 23, dollarExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "dollarExpression"

    public class primaryExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "primaryExpression"
    // GAMS.g:230:1: primaryExpression : ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value );
    public GAMSParser.primaryExpression_return primaryExpression() // throws RecognitionException [1]
    {   
        GAMSParser.primaryExpression_return retval = new GAMSParser.primaryExpression_return();
        retval.Start = input.LT(1);
        int primaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken L182 = null;
        IToken R184 = null;
        IToken L285 = null;
        IToken R287 = null;
        IToken L388 = null;
        IToken R390 = null;
        GAMSParser.expression_return expression83 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression86 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression89 = default(GAMSParser.expression_return);

        GAMSParser.value_return value91 = default(GAMSParser.value_return);


        object L182_tree=null;
        object R184_tree=null;
        object L285_tree=null;
        object R287_tree=null;
        object L388_tree=null;
        object R390_tree=null;
        RewriteRuleTokenStream stream_L1 = new RewriteRuleTokenStream(adaptor,"token L1");
        RewriteRuleTokenStream stream_L2 = new RewriteRuleTokenStream(adaptor,"token L2");
        RewriteRuleTokenStream stream_L3 = new RewriteRuleTokenStream(adaptor,"token L3");
        RewriteRuleTokenStream stream_R2 = new RewriteRuleTokenStream(adaptor,"token R2");
        RewriteRuleTokenStream stream_R3 = new RewriteRuleTokenStream(adaptor,"token R3");
        RewriteRuleTokenStream stream_R1 = new RewriteRuleTokenStream(adaptor,"token R1");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 24) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:231:4: ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value )
            int alt19 = 4;
            alt19 = dfa19.Predict(input);
            switch (alt19) 
            {
                case 1 :
                    // GAMS.g:231:6: L1 expression R1
                    {
                    	L182=(IToken)Match(input,L1,FOLLOW_L1_in_primaryExpression892); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L182);

                    	PushFollow(FOLLOW_expression_in_primaryExpression894);
                    	expression83 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression83.Tree);
                    	R184=(IToken)Match(input,R1,FOLLOW_R1_in_primaryExpression896); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R184);



                    	// AST REWRITE
                    	// elements:          expression
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 231:23: -> ^( ASTEXPRESSION1 expression )
                    	{
                    	    // GAMS.g:231:26: ^( ASTEXPRESSION1 expression )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEXPRESSION1, "ASTEXPRESSION1"), root_1);

                    	    adaptor.AddChild(root_1, stream_expression.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // GAMS.g:232:6: L2 expression R2
                    {
                    	L285=(IToken)Match(input,L2,FOLLOW_L2_in_primaryExpression911); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L285);

                    	PushFollow(FOLLOW_expression_in_primaryExpression913);
                    	expression86 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression86.Tree);
                    	R287=(IToken)Match(input,R2,FOLLOW_R2_in_primaryExpression915); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R287);



                    	// AST REWRITE
                    	// elements:          expression
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 232:23: -> ^( ASTEXPRESSION2 expression )
                    	{
                    	    // GAMS.g:232:26: ^( ASTEXPRESSION2 expression )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEXPRESSION2, "ASTEXPRESSION2"), root_1);

                    	    adaptor.AddChild(root_1, stream_expression.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 3 :
                    // GAMS.g:233:8: L3 expression R3
                    {
                    	L388=(IToken)Match(input,L3,FOLLOW_L3_in_primaryExpression932); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L388);

                    	PushFollow(FOLLOW_expression_in_primaryExpression934);
                    	expression89 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression89.Tree);
                    	R390=(IToken)Match(input,R3,FOLLOW_R3_in_primaryExpression936); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R390);



                    	// AST REWRITE
                    	// elements:          expression
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 233:25: -> ^( ASTEXPRESSION3 expression )
                    	{
                    	    // GAMS.g:233:28: ^( ASTEXPRESSION3 expression )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEXPRESSION3, "ASTEXPRESSION3"), root_1);

                    	    adaptor.AddChild(root_1, stream_expression.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 4 :
                    // GAMS.g:234:6: value
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_value_in_primaryExpression951);
                    	value91 = value();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, value91.Tree);

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
            	Memoize(input, 24, primaryExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "primaryExpression"

    public class logical_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "logical"
    // GAMS.g:236:1: logical : ( NONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL | EQUAL | LESSTHAN | GREATERTHAN );
    public GAMSParser.logical_return logical() // throws RecognitionException [1]
    {   
        GAMSParser.logical_return retval = new GAMSParser.logical_return();
        retval.Start = input.LT(1);
        int logical_StartIndex = input.Index();
        object root_0 = null;

        IToken set92 = null;

        object set92_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 25) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:236:8: ( NONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL | EQUAL | LESSTHAN | GREATERTHAN )
            // GAMS.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set92 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= NONEQUAL && input.LA(1) <= GREATERTHAN) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set92));
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
            	Memoize(input, 25, logical_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "logical"

    public class value_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "value"
    // GAMS.g:238:1: value : ( Integer -> ^( ASTINTEGER Integer ) | Double -> ^( ASTDOUBLE Double ) | sum -> ^( ASTSUM sum ) | function | variableWithIndexerEtc -> ^( ASTDEFINITION variableWithIndexerEtc ) | ident -> ^( ASTVARIABLE ident ) | StringInQuotes );
    public GAMSParser.value_return value() // throws RecognitionException [1]
    {   
        GAMSParser.value_return retval = new GAMSParser.value_return();
        retval.Start = input.LT(1);
        int value_StartIndex = input.Index();
        object root_0 = null;

        IToken Integer93 = null;
        IToken Double94 = null;
        IToken StringInQuotes99 = null;
        GAMSParser.sum_return sum95 = default(GAMSParser.sum_return);

        GAMSParser.function_return function96 = default(GAMSParser.function_return);

        GAMSParser.variableWithIndexerEtc_return variableWithIndexerEtc97 = default(GAMSParser.variableWithIndexerEtc_return);

        GAMSParser.ident_return ident98 = default(GAMSParser.ident_return);


        object Integer93_tree=null;
        object Double94_tree=null;
        object StringInQuotes99_tree=null;
        RewriteRuleTokenStream stream_Double = new RewriteRuleTokenStream(adaptor,"token Double");
        RewriteRuleTokenStream stream_Integer = new RewriteRuleTokenStream(adaptor,"token Integer");
        RewriteRuleSubtreeStream stream_variableWithIndexerEtc = new RewriteRuleSubtreeStream(adaptor,"rule variableWithIndexerEtc");
        RewriteRuleSubtreeStream stream_ident = new RewriteRuleSubtreeStream(adaptor,"rule ident");
        RewriteRuleSubtreeStream stream_sum = new RewriteRuleSubtreeStream(adaptor,"rule sum");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 26) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:239:2: ( Integer -> ^( ASTINTEGER Integer ) | Double -> ^( ASTDOUBLE Double ) | sum -> ^( ASTSUM sum ) | function | variableWithIndexerEtc -> ^( ASTDEFINITION variableWithIndexerEtc ) | ident -> ^( ASTVARIABLE ident ) | StringInQuotes )
            int alt20 = 7;
            alt20 = dfa20.Predict(input);
            switch (alt20) 
            {
                case 1 :
                    // GAMS.g:239:5: Integer
                    {
                    	Integer93=(IToken)Match(input,Integer,FOLLOW_Integer_in_value992); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer93);



                    	// AST REWRITE
                    	// elements:          Integer
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 239:15: -> ^( ASTINTEGER Integer )
                    	{
                    	    // GAMS.g:239:18: ^( ASTINTEGER Integer )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTINTEGER, "ASTINTEGER"), root_1);

                    	    adaptor.AddChild(root_1, stream_Integer.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // GAMS.g:240:4: Double
                    {
                    	Double94=(IToken)Match(input,Double,FOLLOW_Double_in_value1007); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Double.Add(Double94);



                    	// AST REWRITE
                    	// elements:          Double
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 240:15: -> ^( ASTDOUBLE Double )
                    	{
                    	    // GAMS.g:240:18: ^( ASTDOUBLE Double )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTDOUBLE, "ASTDOUBLE"), root_1);

                    	    adaptor.AddChild(root_1, stream_Double.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 3 :
                    // GAMS.g:241:6: sum
                    {
                    	PushFollow(FOLLOW_sum_in_value1026);
                    	sum95 = sum();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sum.Add(sum95.Tree);


                    	// AST REWRITE
                    	// elements:          sum
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 241:17: -> ^( ASTSUM sum )
                    	{
                    	    // GAMS.g:241:20: ^( ASTSUM sum )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM, "ASTSUM"), root_1);

                    	    adaptor.AddChild(root_1, stream_sum.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 4 :
                    // GAMS.g:242:6: function
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_function_in_value1048);
                    	function96 = function();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, function96.Tree);

                    }
                    break;
                case 5 :
                    // GAMS.g:243:4: variableWithIndexerEtc
                    {
                    	PushFollow(FOLLOW_variableWithIndexerEtc_in_value1053);
                    	variableWithIndexerEtc97 = variableWithIndexerEtc();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variableWithIndexerEtc.Add(variableWithIndexerEtc97.Tree);


                    	// AST REWRITE
                    	// elements:          variableWithIndexerEtc
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 243:27: -> ^( ASTDEFINITION variableWithIndexerEtc )
                    	{
                    	    // GAMS.g:243:30: ^( ASTDEFINITION variableWithIndexerEtc )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTDEFINITION, "ASTDEFINITION"), root_1);

                    	    adaptor.AddChild(root_1, stream_variableWithIndexerEtc.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 6 :
                    // GAMS.g:244:6: ident
                    {
                    	PushFollow(FOLLOW_ident_in_value1068);
                    	ident98 = ident();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_ident.Add(ident98.Tree);


                    	// AST REWRITE
                    	// elements:          ident
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 244:17: -> ^( ASTVARIABLE ident )
                    	{
                    	    // GAMS.g:244:20: ^( ASTVARIABLE ident )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARIABLE, "ASTVARIABLE"), root_1);

                    	    adaptor.AddChild(root_1, stream_ident.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 7 :
                    // GAMS.g:245:6: StringInQuotes
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	StringInQuotes99=(IToken)Match(input,StringInQuotes,FOLLOW_StringInQuotes_in_value1088); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{StringInQuotes99_tree = (object)adaptor.Create(StringInQuotes99);
                    		adaptor.AddChild(root_0, StringInQuotes99_tree);
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
            	Memoize(input, 26, value_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "value"

    public class sum_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "sum"
    // GAMS.g:248:1: sum : ( SUM L1 sumControlled ( conditional )? COMMA expression R1 | SUM L2 sumControlled ( conditional )? COMMA expression R2 | SUM L3 sumControlled ( conditional )? COMMA expression R3 );
    public GAMSParser.sum_return sum() // throws RecognitionException [1]
    {   
        GAMSParser.sum_return retval = new GAMSParser.sum_return();
        retval.Start = input.LT(1);
        int sum_StartIndex = input.Index();
        object root_0 = null;

        IToken SUM100 = null;
        IToken L1101 = null;
        IToken COMMA104 = null;
        IToken R1106 = null;
        IToken SUM107 = null;
        IToken L2108 = null;
        IToken COMMA111 = null;
        IToken R2113 = null;
        IToken SUM114 = null;
        IToken L3115 = null;
        IToken COMMA118 = null;
        IToken R3120 = null;
        GAMSParser.sumControlled_return sumControlled102 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional103 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression105 = default(GAMSParser.expression_return);

        GAMSParser.sumControlled_return sumControlled109 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional110 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression112 = default(GAMSParser.expression_return);

        GAMSParser.sumControlled_return sumControlled116 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional117 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression119 = default(GAMSParser.expression_return);


        object SUM100_tree=null;
        object L1101_tree=null;
        object COMMA104_tree=null;
        object R1106_tree=null;
        object SUM107_tree=null;
        object L2108_tree=null;
        object COMMA111_tree=null;
        object R2113_tree=null;
        object SUM114_tree=null;
        object L3115_tree=null;
        object COMMA118_tree=null;
        object R3120_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 27) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:248:4: ( SUM L1 sumControlled ( conditional )? COMMA expression R1 | SUM L2 sumControlled ( conditional )? COMMA expression R2 | SUM L3 sumControlled ( conditional )? COMMA expression R3 )
            int alt24 = 3;
            int LA24_0 = input.LA(1);

            if ( (LA24_0 == SUM) )
            {
                switch ( input.LA(2) ) 
                {
                case L1:
                	{
                    alt24 = 1;
                    }
                    break;
                case L2:
                	{
                    alt24 = 2;
                    }
                    break;
                case L3:
                	{
                    alt24 = 3;
                    }
                    break;
                	default:
                	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                	    NoViableAltException nvae_d24s1 =
                	        new NoViableAltException("", 24, 1, input);

                	    throw nvae_d24s1;
                }

            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d24s0 =
                    new NoViableAltException("", 24, 0, input);

                throw nvae_d24s0;
            }
            switch (alt24) 
            {
                case 1 :
                    // GAMS.g:248:7: SUM L1 sumControlled ( conditional )? COMMA expression R1
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	SUM100=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum1098); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{SUM100_tree = (object)adaptor.Create(SUM100);
                    		adaptor.AddChild(root_0, SUM100_tree);
                    	}
                    	L1101=(IToken)Match(input,L1,FOLLOW_L1_in_sum1100); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L1101_tree = (object)adaptor.Create(L1101);
                    		adaptor.AddChild(root_0, L1101_tree);
                    	}
                    	PushFollow(FOLLOW_sumControlled_in_sum1102);
                    	sumControlled102 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, sumControlled102.Tree);
                    	// GAMS.g:248:28: ( conditional )?
                    	int alt21 = 2;
                    	int LA21_0 = input.LA(1);

                    	if ( (LA21_0 == DOLLAR) )
                    	{
                    	    alt21 = 1;
                    	}
                    	switch (alt21) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:0:0: conditional
                    	        {
                    	        	PushFollow(FOLLOW_conditional_in_sum1104);
                    	        	conditional103 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, conditional103.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA104=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum1107); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{COMMA104_tree = (object)adaptor.Create(COMMA104);
                    		adaptor.AddChild(root_0, COMMA104_tree);
                    	}
                    	PushFollow(FOLLOW_expression_in_sum1109);
                    	expression105 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression105.Tree);
                    	R1106=(IToken)Match(input,R1,FOLLOW_R1_in_sum1111); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R1106_tree = (object)adaptor.Create(R1106);
                    		adaptor.AddChild(root_0, R1106_tree);
                    	}

                    }
                    break;
                case 2 :
                    // GAMS.g:249:7: SUM L2 sumControlled ( conditional )? COMMA expression R2
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	SUM107=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum1119); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{SUM107_tree = (object)adaptor.Create(SUM107);
                    		adaptor.AddChild(root_0, SUM107_tree);
                    	}
                    	L2108=(IToken)Match(input,L2,FOLLOW_L2_in_sum1121); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L2108_tree = (object)adaptor.Create(L2108);
                    		adaptor.AddChild(root_0, L2108_tree);
                    	}
                    	PushFollow(FOLLOW_sumControlled_in_sum1123);
                    	sumControlled109 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, sumControlled109.Tree);
                    	// GAMS.g:249:28: ( conditional )?
                    	int alt22 = 2;
                    	int LA22_0 = input.LA(1);

                    	if ( (LA22_0 == DOLLAR) )
                    	{
                    	    alt22 = 1;
                    	}
                    	switch (alt22) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:0:0: conditional
                    	        {
                    	        	PushFollow(FOLLOW_conditional_in_sum1125);
                    	        	conditional110 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, conditional110.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA111=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum1128); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{COMMA111_tree = (object)adaptor.Create(COMMA111);
                    		adaptor.AddChild(root_0, COMMA111_tree);
                    	}
                    	PushFollow(FOLLOW_expression_in_sum1130);
                    	expression112 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression112.Tree);
                    	R2113=(IToken)Match(input,R2,FOLLOW_R2_in_sum1132); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R2113_tree = (object)adaptor.Create(R2113);
                    		adaptor.AddChild(root_0, R2113_tree);
                    	}

                    }
                    break;
                case 3 :
                    // GAMS.g:250:7: SUM L3 sumControlled ( conditional )? COMMA expression R3
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	SUM114=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum1140); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{SUM114_tree = (object)adaptor.Create(SUM114);
                    		adaptor.AddChild(root_0, SUM114_tree);
                    	}
                    	L3115=(IToken)Match(input,L3,FOLLOW_L3_in_sum1142); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L3115_tree = (object)adaptor.Create(L3115);
                    		adaptor.AddChild(root_0, L3115_tree);
                    	}
                    	PushFollow(FOLLOW_sumControlled_in_sum1144);
                    	sumControlled116 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, sumControlled116.Tree);
                    	// GAMS.g:250:28: ( conditional )?
                    	int alt23 = 2;
                    	int LA23_0 = input.LA(1);

                    	if ( (LA23_0 == DOLLAR) )
                    	{
                    	    alt23 = 1;
                    	}
                    	switch (alt23) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:0:0: conditional
                    	        {
                    	        	PushFollow(FOLLOW_conditional_in_sum1146);
                    	        	conditional117 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, conditional117.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA118=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum1149); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{COMMA118_tree = (object)adaptor.Create(COMMA118);
                    		adaptor.AddChild(root_0, COMMA118_tree);
                    	}
                    	PushFollow(FOLLOW_expression_in_sum1151);
                    	expression119 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression119.Tree);
                    	R3120=(IToken)Match(input,R3,FOLLOW_R3_in_sum1153); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R3120_tree = (object)adaptor.Create(R3120);
                    		adaptor.AddChild(root_0, R3120_tree);
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
            	Memoize(input, 27, sum_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "sum"

    public class variableWithDot_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variableWithDot"
    // GAMS.g:252:1: variableWithDot : variable ( DOT variable )? ;
    public GAMSParser.variableWithDot_return variableWithDot() // throws RecognitionException [1]
    {   
        GAMSParser.variableWithDot_return retval = new GAMSParser.variableWithDot_return();
        retval.Start = input.LT(1);
        int variableWithDot_StartIndex = input.Index();
        object root_0 = null;

        IToken DOT122 = null;
        GAMSParser.variable_return variable121 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable123 = default(GAMSParser.variable_return);


        object DOT122_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 28) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:252:16: ( variable ( DOT variable )? )
            // GAMS.g:252:18: variable ( DOT variable )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_variable_in_variableWithDot1160);
            	variable121 = variable();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable121.Tree);
            	// GAMS.g:252:27: ( DOT variable )?
            	int alt25 = 2;
            	int LA25_0 = input.LA(1);

            	if ( (LA25_0 == DOT) )
            	{
            	    alt25 = 1;
            	}
            	switch (alt25) 
            	{
            	    case 1 :
            	        // GAMS.g:252:28: DOT variable
            	        {
            	        	DOT122=(IToken)Match(input,DOT,FOLLOW_DOT_in_variableWithDot1163); if (state.failed) return retval;
            	        	if ( state.backtracking == 0 )
            	        	{DOT122_tree = (object)adaptor.Create(DOT122);
            	        		adaptor.AddChild(root_0, DOT122_tree);
            	        	}
            	        	PushFollow(FOLLOW_variable_in_variableWithDot1165);
            	        	variable123 = variable();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable123.Tree);

            	        }
            	        break;

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
            	Memoize(input, 28, variableWithDot_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "variableWithDot"

    public class sumControlled_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "sumControlled"
    // GAMS.g:254:1: sumControlled : ( variable | L1 indexerElements R1 | L2 indexerElements R2 | L3 indexerElements R3 );
    public GAMSParser.sumControlled_return sumControlled() // throws RecognitionException [1]
    {   
        GAMSParser.sumControlled_return retval = new GAMSParser.sumControlled_return();
        retval.Start = input.LT(1);
        int sumControlled_StartIndex = input.Index();
        object root_0 = null;

        IToken L1125 = null;
        IToken R1127 = null;
        IToken L2128 = null;
        IToken R2130 = null;
        IToken L3131 = null;
        IToken R3133 = null;
        GAMSParser.variable_return variable124 = default(GAMSParser.variable_return);

        GAMSParser.indexerElements_return indexerElements126 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements129 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements132 = default(GAMSParser.indexerElements_return);


        object L1125_tree=null;
        object R1127_tree=null;
        object L2128_tree=null;
        object R2130_tree=null;
        object L3131_tree=null;
        object R3133_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 29) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:254:14: ( variable | L1 indexerElements R1 | L2 indexerElements R2 | L3 indexerElements R3 )
            int alt26 = 4;
            switch ( input.LA(1) ) 
            {
            case SUM:
            case AND:
            case OR:
            case NOT:
            case ABS:
            case EXP:
            case LOG:
            case MAX:
            case MIN:
            case POWER:
            case SQR:
            case TANH:
            case SAMEAS:
            case Ident:
            	{
                alt26 = 1;
                }
                break;
            case L1:
            	{
                alt26 = 2;
                }
                break;
            case L2:
            	{
                alt26 = 3;
                }
                break;
            case L3:
            	{
                alt26 = 4;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d26s0 =
            	        new NoViableAltException("", 26, 0, input);

            	    throw nvae_d26s0;
            }

            switch (alt26) 
            {
                case 1 :
                    // GAMS.g:255:11: variable
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variable_in_sumControlled1184);
                    	variable124 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable124.Tree);

                    }
                    break;
                case 2 :
                    // GAMS.g:256:5: L1 indexerElements R1
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	L1125=(IToken)Match(input,L1,FOLLOW_L1_in_sumControlled1191); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L1125_tree = (object)adaptor.Create(L1125);
                    		adaptor.AddChild(root_0, L1125_tree);
                    	}
                    	PushFollow(FOLLOW_indexerElements_in_sumControlled1193);
                    	indexerElements126 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, indexerElements126.Tree);
                    	R1127=(IToken)Match(input,R1,FOLLOW_R1_in_sumControlled1195); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R1127_tree = (object)adaptor.Create(R1127);
                    		adaptor.AddChild(root_0, R1127_tree);
                    	}

                    }
                    break;
                case 3 :
                    // GAMS.g:257:5: L2 indexerElements R2
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	L2128=(IToken)Match(input,L2,FOLLOW_L2_in_sumControlled1201); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L2128_tree = (object)adaptor.Create(L2128);
                    		adaptor.AddChild(root_0, L2128_tree);
                    	}
                    	PushFollow(FOLLOW_indexerElements_in_sumControlled1203);
                    	indexerElements129 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, indexerElements129.Tree);
                    	R2130=(IToken)Match(input,R2,FOLLOW_R2_in_sumControlled1205); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R2130_tree = (object)adaptor.Create(R2130);
                    		adaptor.AddChild(root_0, R2130_tree);
                    	}

                    }
                    break;
                case 4 :
                    // GAMS.g:258:5: L3 indexerElements R3
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	L3131=(IToken)Match(input,L3,FOLLOW_L3_in_sumControlled1211); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L3131_tree = (object)adaptor.Create(L3131);
                    		adaptor.AddChild(root_0, L3131_tree);
                    	}
                    	PushFollow(FOLLOW_indexerElements_in_sumControlled1213);
                    	indexerElements132 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, indexerElements132.Tree);
                    	R3133=(IToken)Match(input,R3,FOLLOW_R3_in_sumControlled1215); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R3133_tree = (object)adaptor.Create(R3133);
                    		adaptor.AddChild(root_0, R3133_tree);
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
            	Memoize(input, 29, sumControlled_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "sumControlled"

    public class numberPlusMinus_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "numberPlusMinus"
    // GAMS.g:260:1: numberPlusMinus : ( MINUS | PLUS ) ( Integer | Double ) ;
    public GAMSParser.numberPlusMinus_return numberPlusMinus() // throws RecognitionException [1]
    {   
        GAMSParser.numberPlusMinus_return retval = new GAMSParser.numberPlusMinus_return();
        retval.Start = input.LT(1);
        int numberPlusMinus_StartIndex = input.Index();
        object root_0 = null;

        IToken set134 = null;
        IToken set135 = null;

        object set134_tree=null;
        object set135_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 30) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:260:18: ( ( MINUS | PLUS ) ( Integer | Double ) )
            // GAMS.g:260:21: ( MINUS | PLUS ) ( Integer | Double )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set134 = (IToken)input.LT(1);
            	if ( input.LA(1) == PLUS || input.LA(1) == MINUS ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set134));
            	    state.errorRecovery = false;state.failed = false;
            	}
            	else 
            	{
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}

            	set135 = (IToken)input.LT(1);
            	if ( input.LA(1) == Integer || input.LA(1) == Double ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set135));
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
            	Memoize(input, 30, numberPlusMinus_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "numberPlusMinus"

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
    // GAMS.g:262:1: ident : ( Ident | extraTokens );
    public GAMSParser.ident_return ident() // throws RecognitionException [1]
    {   
        GAMSParser.ident_return retval = new GAMSParser.ident_return();
        retval.Start = input.LT(1);
        int ident_StartIndex = input.Index();
        object root_0 = null;

        IToken Ident136 = null;
        GAMSParser.extraTokens_return extraTokens137 = default(GAMSParser.extraTokens_return);


        object Ident136_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 31) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:262:9: ( Ident | extraTokens )
            int alt27 = 2;
            int LA27_0 = input.LA(1);

            if ( (LA27_0 == Ident) )
            {
                alt27 = 1;
            }
            else if ( ((LA27_0 >= SUM && LA27_0 <= SAMEAS)) )
            {
                alt27 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d27s0 =
                    new NoViableAltException("", 27, 0, input);

                throw nvae_d27s0;
            }
            switch (alt27) 
            {
                case 1 :
                    // GAMS.g:262:12: Ident
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Ident136=(IToken)Match(input,Ident,FOLLOW_Ident_in_ident1250); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Ident136_tree = (object)adaptor.Create(Ident136);
                    		adaptor.AddChild(root_0, Ident136_tree);
                    	}

                    }
                    break;
                case 2 :
                    // GAMS.g:262:20: extraTokens
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_extraTokens_in_ident1254);
                    	extraTokens137 = extraTokens();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, extraTokens137.Tree);

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
            	Memoize(input, 31, ident_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "ident"

    public class variable_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variable"
    // GAMS.g:263:1: variable : ident ;
    public GAMSParser.variable_return variable() // throws RecognitionException [1]
    {   
        GAMSParser.variable_return retval = new GAMSParser.variable_return();
        retval.Start = input.LT(1);
        int variable_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.ident_return ident138 = default(GAMSParser.ident_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 32) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:263:10: ( ident )
            // GAMS.g:263:12: ident
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_ident_in_variable1261);
            	ident138 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, ident138.Tree);

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
            	Memoize(input, 32, variable_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "variable"

    public class pow_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "pow"
    // GAMS.g:265:1: pow : STARS -> ASTPOW ;
    public GAMSParser.pow_return pow() // throws RecognitionException [1]
    {   
        GAMSParser.pow_return retval = new GAMSParser.pow_return();
        retval.Start = input.LT(1);
        int pow_StartIndex = input.Index();
        object root_0 = null;

        IToken STARS139 = null;

        object STARS139_tree=null;
        RewriteRuleTokenStream stream_STARS = new RewriteRuleTokenStream(adaptor,"token STARS");

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 33) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:265:9: ( STARS -> ASTPOW )
            // GAMS.g:265:17: STARS
            {
            	STARS139=(IToken)Match(input,STARS,FOLLOW_STARS_in_pow1279); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_STARS.Add(STARS139);



            	// AST REWRITE
            	// elements:          
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 265:24: -> ASTPOW
            	{
            	    adaptor.AddChild(root_0, (object)adaptor.Create(ASTPOW, "ASTPOW"));

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
            	Memoize(input, 33, pow_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "pow"

    // $ANTLR start "synpred27_GAMS"
    public void synpred27_GAMS_fragment() {
        // GAMS.g:163:33: ( conditional )
        // GAMS.g:163:33: conditional
        {
        	PushFollow(FOLLOW_conditional_in_synpred27_GAMS523);
        	conditional();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred27_GAMS"

    // $ANTLR start "synpred35_GAMS"
    public void synpred35_GAMS_fragment() {
        // GAMS.g:213:28: ( OR andExpression )
        // GAMS.g:213:28: OR andExpression
        {
        	Match(input,OR,FOLLOW_OR_in_synpred35_GAMS685); if (state.failed) return ;
        	PushFollow(FOLLOW_andExpression_in_synpred35_GAMS688);
        	andExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred35_GAMS"

    // $ANTLR start "synpred36_GAMS"
    public void synpred36_GAMS_fragment() {
        // GAMS.g:214:31: ( AND notExpression )
        // GAMS.g:214:31: AND notExpression
        {
        	Match(input,AND,FOLLOW_AND_in_synpred36_GAMS701); if (state.failed) return ;
        	PushFollow(FOLLOW_notExpression_in_synpred36_GAMS704);
        	notExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred36_GAMS"

    // $ANTLR start "synpred37_GAMS"
    public void synpred37_GAMS_fragment() {
        // GAMS.g:215:16: ( logicalExpression )
        // GAMS.g:215:16: logicalExpression
        {
        	PushFollow(FOLLOW_logicalExpression_in_synpred37_GAMS714);
        	logicalExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred37_GAMS"

    // $ANTLR start "synpred38_GAMS"
    public void synpred38_GAMS_fragment() {
        // GAMS.g:217:41: ( logical additiveExpression )
        // GAMS.g:217:41: logical additiveExpression
        {
        	PushFollow(FOLLOW_logical_in_synpred38_GAMS746);
        	logical();
        	state.followingStackPointer--;
        	if (state.failed) return ;
        	PushFollow(FOLLOW_additiveExpression_in_synpred38_GAMS749);
        	additiveExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred38_GAMS"

    // $ANTLR start "synpred40_GAMS"
    public void synpred40_GAMS_fragment() {
        // GAMS.g:219:50: ( ( PLUS | MINUS ) multiplicativeExpression )
        // GAMS.g:219:50: ( PLUS | MINUS ) multiplicativeExpression
        {
        	if ( input.LA(1) == PLUS || input.LA(1) == MINUS ) 
        	{
        	    input.Consume();
        	    state.errorRecovery = false;state.failed = false;
        	}
        	else 
        	{
        	    if ( state.backtracking > 0 ) {state.failed = true; return ;}
        	    MismatchedSetException mse = new MismatchedSetException(null,input);
        	    throw mse;
        	}

        	PushFollow(FOLLOW_multiplicativeExpression_in_synpred40_GAMS771);
        	multiplicativeExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred40_GAMS"

    // $ANTLR start "synpred43_GAMS"
    public void synpred43_GAMS_fragment() {
        // GAMS.g:221:48: ( ( MULT | DIV | MOD ) powerExpression )
        // GAMS.g:221:48: ( MULT | DIV | MOD ) powerExpression
        {
        	if ( (input.LA(1) >= MULT && input.LA(1) <= MOD) ) 
        	{
        	    input.Consume();
        	    state.errorRecovery = false;state.failed = false;
        	}
        	else 
        	{
        	    if ( state.backtracking > 0 ) {state.failed = true; return ;}
        	    MismatchedSetException mse = new MismatchedSetException(null,input);
        	    throw mse;
        	}

        	PushFollow(FOLLOW_powerExpression_in_synpred43_GAMS797);
        	powerExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred43_GAMS"

    // $ANTLR start "synpred44_GAMS"
    public void synpred44_GAMS_fragment() {
        // GAMS.g:223:39: ( pow unaryExpression )
        // GAMS.g:223:39: pow unaryExpression
        {
        	PushFollow(FOLLOW_pow_in_synpred44_GAMS814);
        	pow();
        	state.followingStackPointer--;
        	if (state.failed) return ;
        	PushFollow(FOLLOW_unaryExpression_in_synpred44_GAMS817);
        	unaryExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred44_GAMS"

    // $ANTLR start "synpred46_GAMS"
    public void synpred46_GAMS_fragment() {
        // GAMS.g:228:37: ( conditional )
        // GAMS.g:228:37: conditional
        {
        	PushFollow(FOLLOW_conditional_in_synpred46_GAMS865);
        	conditional();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred46_GAMS"

    // $ANTLR start "synpred59_GAMS"
    public void synpred59_GAMS_fragment() {
        // GAMS.g:243:4: ( variableWithIndexerEtc )
        // GAMS.g:243:4: variableWithIndexerEtc
        {
        	PushFollow(FOLLOW_variableWithIndexerEtc_in_synpred59_GAMS1053);
        	variableWithIndexerEtc();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred59_GAMS"

    // $ANTLR start "synpred60_GAMS"
    public void synpred60_GAMS_fragment() {
        // GAMS.g:244:6: ( ident )
        // GAMS.g:244:6: ident
        {
        	PushFollow(FOLLOW_ident_in_synpred60_GAMS1068);
        	ident();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred60_GAMS"

    // Delegated rules

   	public bool synpred60_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred60_GAMS_fragment(); // can never throw exception
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
   	public bool synpred37_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred37_GAMS_fragment(); // can never throw exception
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
   	public bool synpred59_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred59_GAMS_fragment(); // can never throw exception
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
   	public bool synpred36_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred36_GAMS_fragment(); // can never throw exception
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
   	public bool synpred44_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred44_GAMS_fragment(); // can never throw exception
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
   	public bool synpred40_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred40_GAMS_fragment(); // can never throw exception
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
   	public bool synpred38_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred38_GAMS_fragment(); // can never throw exception
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
   	public bool synpred27_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred27_GAMS_fragment(); // can never throw exception
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
   	public bool synpred46_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred46_GAMS_fragment(); // can never throw exception
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
   	public bool synpred35_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred35_GAMS_fragment(); // can never throw exception
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
   	public bool synpred43_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred43_GAMS_fragment(); // can never throw exception
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


   	protected DFA4 dfa4;
   	protected DFA5 dfa5;
   	protected DFA6 dfa6;
   	protected DFA9 dfa9;
   	protected DFA10 dfa10;
   	protected DFA11 dfa11;
   	protected DFA12 dfa12;
   	protected DFA13 dfa13;
   	protected DFA14 dfa14;
   	protected DFA15 dfa15;
   	protected DFA16 dfa16;
   	protected DFA17 dfa17;
   	protected DFA18 dfa18;
   	protected DFA19 dfa19;
   	protected DFA20 dfa20;
	private void InitializeCyclicDFAs()
	{
    	this.dfa4 = new DFA4(this);
    	this.dfa5 = new DFA5(this);
    	this.dfa6 = new DFA6(this);
    	this.dfa9 = new DFA9(this);
    	this.dfa10 = new DFA10(this);
    	this.dfa11 = new DFA11(this);
    	this.dfa12 = new DFA12(this);
    	this.dfa13 = new DFA13(this);
    	this.dfa14 = new DFA14(this);
    	this.dfa15 = new DFA15(this);
    	this.dfa16 = new DFA16(this);
    	this.dfa17 = new DFA17(this);
    	this.dfa18 = new DFA18(this);
    	this.dfa19 = new DFA19(this);
    	this.dfa20 = new DFA20(this);


	    this.dfa6.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA6_SpecialStateTransition);

	    this.dfa10.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA10_SpecialStateTransition);
	    this.dfa11.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA11_SpecialStateTransition);
	    this.dfa12.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA12_SpecialStateTransition);
	    this.dfa13.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA13_SpecialStateTransition);
	    this.dfa14.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA14_SpecialStateTransition);
	    this.dfa15.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA15_SpecialStateTransition);
	    this.dfa16.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA16_SpecialStateTransition);

	    this.dfa18.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA18_SpecialStateTransition);

	    this.dfa20.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA20_SpecialStateTransition);
	}

    const string DFA4_eotS =
        "\x14\uffff";
    const string DFA4_eofS =
        "\x01\x02\x13\uffff";
    const string DFA4_minS =
        "\x01\x1f\x13\uffff";
    const string DFA4_maxS =
        "\x01\x46\x13\uffff";
    const string DFA4_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x11\uffff";
    const string DFA4_specialS =
        "\x14\uffff}>";
    static readonly string[] DFA4_transitionS = {
            "\x02\x02\x0a\uffff\x0a\x02\x01\x01\x01\uffff\x01\x02\x01\uffff"+
            "\x01\x02\x01\uffff\x0a\x02\x01\uffff\x01\x02",
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
            ""
    };

    static readonly short[] DFA4_eot = DFA.UnpackEncodedString(DFA4_eotS);
    static readonly short[] DFA4_eof = DFA.UnpackEncodedString(DFA4_eofS);
    static readonly char[] DFA4_min = DFA.UnpackEncodedStringToUnsignedChars(DFA4_minS);
    static readonly char[] DFA4_max = DFA.UnpackEncodedStringToUnsignedChars(DFA4_maxS);
    static readonly short[] DFA4_accept = DFA.UnpackEncodedString(DFA4_acceptS);
    static readonly short[] DFA4_special = DFA.UnpackEncodedString(DFA4_specialS);
    static readonly short[][] DFA4_transition = DFA.UnpackEncodedStringArray(DFA4_transitionS);

    protected class DFA4 : DFA
    {
        public DFA4(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 4;
            this.eot = DFA4_eot;
            this.eof = DFA4_eof;
            this.min = DFA4_min;
            this.max = DFA4_max;
            this.accept = DFA4_accept;
            this.special = DFA4_special;
            this.transition = DFA4_transition;

        }

        override public string Description
        {
            get { return "163:12: ( DOT variable )?"; }
        }

    }

    const string DFA5_eotS =
        "\x13\uffff";
    const string DFA5_eofS =
        "\x01\x04\x12\uffff";
    const string DFA5_minS =
        "\x01\x1f\x12\uffff";
    const string DFA5_maxS =
        "\x01\x46\x12\uffff";
    const string DFA5_acceptS =
        "\x01\uffff\x01\x01\x02\uffff\x01\x02\x0e\uffff";
    const string DFA5_specialS =
        "\x13\uffff}>";
    static readonly string[] DFA5_transitionS = {
            "\x02\x04\x0a\uffff\x03\x04\x01\x01\x01\x04\x01\x01\x01\x04"+
            "\x01\x01\x02\x04\x02\uffff\x01\x04\x01\uffff\x01\x04\x01\uffff"+
            "\x0a\x04\x01\uffff\x01\x04",
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
            ""
    };

    static readonly short[] DFA5_eot = DFA.UnpackEncodedString(DFA5_eotS);
    static readonly short[] DFA5_eof = DFA.UnpackEncodedString(DFA5_eofS);
    static readonly char[] DFA5_min = DFA.UnpackEncodedStringToUnsignedChars(DFA5_minS);
    static readonly char[] DFA5_max = DFA.UnpackEncodedStringToUnsignedChars(DFA5_maxS);
    static readonly short[] DFA5_accept = DFA.UnpackEncodedString(DFA5_acceptS);
    static readonly short[] DFA5_special = DFA.UnpackEncodedString(DFA5_specialS);
    static readonly short[][] DFA5_transition = DFA.UnpackEncodedStringArray(DFA5_transitionS);

    protected class DFA5 : DFA
    {
        public DFA5(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 5;
            this.eot = DFA5_eot;
            this.eof = DFA5_eof;
            this.min = DFA5_min;
            this.max = DFA5_max;
            this.accept = DFA5_accept;
            this.special = DFA5_special;
            this.transition = DFA5_transition;

        }

        override public string Description
        {
            get { return "163:28: ( idx )?"; }
        }

    }

    const string DFA6_eotS =
        "\x1d\uffff";
    const string DFA6_eofS =
        "\x01\x02\x1c\uffff";
    const string DFA6_minS =
        "\x01\x1f\x01\x1e\x0e\uffff\x0c\x00\x01\uffff";
    const string DFA6_maxS =
        "\x01\x46\x01\x45\x0e\uffff\x0c\x00\x01\uffff";
    const string DFA6_acceptS =
        "\x02\uffff\x01\x02\x19\uffff\x01\x01";
    const string DFA6_specialS =
        "\x10\uffff\x01\x00\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01"+
        "\x06\x01\x07\x01\x08\x01\x09\x01\x0a\x01\x0b\x01\uffff}>";
    static readonly string[] DFA6_transitionS = {
            "\x02\x02\x0a\uffff\x03\x02\x01\uffff\x01\x02\x01\uffff\x01"+
            "\x02\x01\uffff\x02\x02\x02\uffff\x01\x02\x01\uffff\x01\x02\x01"+
            "\uffff\x01\x01\x09\x02\x01\uffff\x01\x02",
            "\x01\x15\x02\x1b\x01\x18\x09\x16\x03\uffff\x01\x10\x01\uffff"+
            "\x01\x11\x01\uffff\x01\x12\x03\uffff\x01\x19\x01\uffff\x01\x13"+
            "\x01\x1a\x01\x14\x0a\uffff\x01\x17",
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
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            ""
    };

    static readonly short[] DFA6_eot = DFA.UnpackEncodedString(DFA6_eotS);
    static readonly short[] DFA6_eof = DFA.UnpackEncodedString(DFA6_eofS);
    static readonly char[] DFA6_min = DFA.UnpackEncodedStringToUnsignedChars(DFA6_minS);
    static readonly char[] DFA6_max = DFA.UnpackEncodedStringToUnsignedChars(DFA6_maxS);
    static readonly short[] DFA6_accept = DFA.UnpackEncodedString(DFA6_acceptS);
    static readonly short[] DFA6_special = DFA.UnpackEncodedString(DFA6_specialS);
    static readonly short[][] DFA6_transition = DFA.UnpackEncodedStringArray(DFA6_transitionS);

    protected class DFA6 : DFA
    {
        public DFA6(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 6;
            this.eot = DFA6_eot;
            this.eof = DFA6_eof;
            this.min = DFA6_min;
            this.max = DFA6_max;
            this.accept = DFA6_accept;
            this.special = DFA6_special;
            this.transition = DFA6_transition;

        }

        override public string Description
        {
            get { return "163:33: ( conditional )?"; }
        }

    }


    protected internal int DFA6_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA6_16 = input.LA(1);

                   	 
                   	int index6_16 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index6_16);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA6_17 = input.LA(1);

                   	 
                   	int index6_17 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index6_17);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA6_18 = input.LA(1);

                   	 
                   	int index6_18 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index6_18);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA6_19 = input.LA(1);

                   	 
                   	int index6_19 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index6_19);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA6_20 = input.LA(1);

                   	 
                   	int index6_20 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index6_20);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA6_21 = input.LA(1);

                   	 
                   	int index6_21 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index6_21);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 6 : 
                   	int LA6_22 = input.LA(1);

                   	 
                   	int index6_22 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index6_22);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 7 : 
                   	int LA6_23 = input.LA(1);

                   	 
                   	int index6_23 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index6_23);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 8 : 
                   	int LA6_24 = input.LA(1);

                   	 
                   	int index6_24 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index6_24);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 9 : 
                   	int LA6_25 = input.LA(1);

                   	 
                   	int index6_25 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index6_25);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 10 : 
                   	int LA6_26 = input.LA(1);

                   	 
                   	int index6_26 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index6_26);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 11 : 
                   	int LA6_27 = input.LA(1);

                   	 
                   	int index6_27 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index6_27);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae6 =
            new NoViableAltException(dfa.Description, 6, _s, input);
        dfa.Error(nvae6);
        throw nvae6;
    }
    const string DFA9_eotS =
        "\x12\uffff";
    const string DFA9_eofS =
        "\x02\uffff\x02\x06\x0e\uffff";
    const string DFA9_minS =
        "\x01\x1e\x01\uffff\x02\x2f\x0e\uffff";
    const string DFA9_maxS =
        "\x01\x45\x01\uffff\x02\x39\x0e\uffff";
    const string DFA9_acceptS =
        "\x01\uffff\x01\x01\x02\uffff\x01\x03\x01\x04\x01\x02\x0b\uffff";
    const string DFA9_specialS =
        "\x12\uffff}>";
    static readonly string[] DFA9_transitionS = {
            "\x0d\x03\x0b\uffff\x01\x01\x0e\uffff\x01\x02",
            "",
            "\x01\x06\x01\uffff\x01\x06\x01\uffff\x02\x06\x02\uffff\x01"+
            "\x04\x01\uffff\x01\x05",
            "\x01\x06\x01\uffff\x01\x06\x01\uffff\x02\x06\x02\uffff\x01"+
            "\x04\x01\uffff\x01\x05",
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

    static readonly short[] DFA9_eot = DFA.UnpackEncodedString(DFA9_eotS);
    static readonly short[] DFA9_eof = DFA.UnpackEncodedString(DFA9_eofS);
    static readonly char[] DFA9_min = DFA.UnpackEncodedStringToUnsignedChars(DFA9_minS);
    static readonly char[] DFA9_max = DFA.UnpackEncodedStringToUnsignedChars(DFA9_maxS);
    static readonly short[] DFA9_accept = DFA.UnpackEncodedString(DFA9_acceptS);
    static readonly short[] DFA9_special = DFA.UnpackEncodedString(DFA9_specialS);
    static readonly short[][] DFA9_transition = DFA.UnpackEncodedStringArray(DFA9_transitionS);

    protected class DFA9 : DFA
    {
        public DFA9(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 9;
            this.eot = DFA9_eot;
            this.eof = DFA9_eof;
            this.min = DFA9_min;
            this.max = DFA9_max;
            this.accept = DFA9_accept;
            this.special = DFA9_special;
            this.transition = DFA9_transition;

        }

        override public string Description
        {
            get { return "190:1: variableLagLead : ( StringInQuotes | variable | variable PLUS Integer | variable MINUS Integer );"; }
        }

    }

    const string DFA10_eotS =
        "\x1d\uffff";
    const string DFA10_eofS =
        "\x01\x01\x1c\uffff";
    const string DFA10_minS =
        "\x01\x1f\x0d\uffff\x01\x00\x0e\uffff";
    const string DFA10_maxS =
        "\x01\x46\x0d\uffff\x01\x00\x0e\uffff";
    const string DFA10_acceptS =
        "\x01\uffff\x01\x02\x1a\uffff\x01\x01";
    const string DFA10_specialS =
        "\x0e\uffff\x01\x00\x0e\uffff}>";
    static readonly string[] DFA10_transitionS = {
            "\x01\x01\x01\x0e\x0a\uffff\x03\x01\x01\uffff\x01\x01\x01\uffff"+
            "\x01\x01\x01\uffff\x02\x01\x02\uffff\x01\x01\x01\uffff\x01\x01"+
            "\x01\uffff\x0a\x01\x01\uffff\x01\x01",
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
            ""
    };

    static readonly short[] DFA10_eot = DFA.UnpackEncodedString(DFA10_eotS);
    static readonly short[] DFA10_eof = DFA.UnpackEncodedString(DFA10_eofS);
    static readonly char[] DFA10_min = DFA.UnpackEncodedStringToUnsignedChars(DFA10_minS);
    static readonly char[] DFA10_max = DFA.UnpackEncodedStringToUnsignedChars(DFA10_maxS);
    static readonly short[] DFA10_accept = DFA.UnpackEncodedString(DFA10_acceptS);
    static readonly short[] DFA10_special = DFA.UnpackEncodedString(DFA10_specialS);
    static readonly short[][] DFA10_transition = DFA.UnpackEncodedStringArray(DFA10_transitionS);

    protected class DFA10 : DFA
    {
        public DFA10(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 10;
            this.eot = DFA10_eot;
            this.eof = DFA10_eof;
            this.min = DFA10_min;
            this.max = DFA10_max;
            this.accept = DFA10_accept;
            this.special = DFA10_special;
            this.transition = DFA10_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 213:27: ( OR andExpression )*"; }
        }

    }


    protected internal int DFA10_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA10_14 = input.LA(1);

                   	 
                   	int index10_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred35_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index10_14);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae10 =
            new NoViableAltException(dfa.Description, 10, _s, input);
        dfa.Error(nvae10);
        throw nvae10;
    }
    const string DFA11_eotS =
        "\x1d\uffff";
    const string DFA11_eofS =
        "\x01\x01\x1c\uffff";
    const string DFA11_minS =
        "\x01\x1f\x0d\uffff\x01\x00\x0e\uffff";
    const string DFA11_maxS =
        "\x01\x46\x0d\uffff\x01\x00\x0e\uffff";
    const string DFA11_acceptS =
        "\x01\uffff\x01\x02\x1a\uffff\x01\x01";
    const string DFA11_specialS =
        "\x0e\uffff\x01\x00\x0e\uffff}>";
    static readonly string[] DFA11_transitionS = {
            "\x01\x0e\x01\x01\x0a\uffff\x03\x01\x01\uffff\x01\x01\x01\uffff"+
            "\x01\x01\x01\uffff\x02\x01\x02\uffff\x01\x01\x01\uffff\x01\x01"+
            "\x01\uffff\x0a\x01\x01\uffff\x01\x01",
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
            get { return "()* loopback of 214:30: ( AND notExpression )*"; }
        }

    }


    protected internal int DFA11_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA11_14 = input.LA(1);

                   	 
                   	int index11_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred36_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index11_14);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae11 =
            new NoViableAltException(dfa.Description, 11, _s, input);
        dfa.Error(nvae11);
        throw nvae11;
    }
    const string DFA12_eotS =
        "\x28\uffff";
    const string DFA12_eofS =
        "\x09\uffff\x01\x01\x1e\uffff";
    const string DFA12_minS =
        "\x01\x1e\x08\uffff\x01\x1e\x03\uffff\x03\x00\x05\uffff\x01\x00"+
        "\x01\uffff\x01\x00\x05\uffff\x01\x00\x0a\uffff";
    const string DFA12_maxS =
        "\x01\x45\x08\uffff\x01\x46\x03\uffff\x03\x00\x05\uffff\x01\x00"+
        "\x01\uffff\x01\x00\x05\uffff\x01\x00\x0a\uffff";
    const string DFA12_acceptS =
        "\x01\uffff\x01\x01\x0e\uffff\x01\x02\x17\uffff";
    const string DFA12_specialS =
        "\x0d\uffff\x01\x00\x01\x01\x01\x02\x05\uffff\x01\x03\x01\uffff"+
        "\x01\x04\x05\uffff\x01\x05\x0a\uffff}>";
    static readonly string[] DFA12_transitionS = {
            "\x03\x01\x01\x09\x09\x01\x03\uffff\x01\x01\x01\uffff\x01\x01"+
            "\x01\uffff\x01\x01\x03\uffff\x01\x01\x01\uffff\x03\x01\x0a\uffff"+
            "\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x10\x01\x15\x01\x1d\x0a\x10\x03\x01\x01\x0d\x01\x01\x01"+
            "\x0e\x01\x01\x01\x0f\x03\x01\x01\x10\x01\x01\x01\x10\x01\x17"+
            "\x01\x10\x0a\x01\x01\x10\x01\x01",
            "",
            "",
            "",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "",
            "",
            "",
            "",
            "",
            "\x01\uffff",
            "",
            "\x01\uffff",
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
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA12_eot = DFA.UnpackEncodedString(DFA12_eotS);
    static readonly short[] DFA12_eof = DFA.UnpackEncodedString(DFA12_eofS);
    static readonly char[] DFA12_min = DFA.UnpackEncodedStringToUnsignedChars(DFA12_minS);
    static readonly char[] DFA12_max = DFA.UnpackEncodedStringToUnsignedChars(DFA12_maxS);
    static readonly short[] DFA12_accept = DFA.UnpackEncodedString(DFA12_acceptS);
    static readonly short[] DFA12_special = DFA.UnpackEncodedString(DFA12_specialS);
    static readonly short[][] DFA12_transition = DFA.UnpackEncodedStringArray(DFA12_transitionS);

    protected class DFA12 : DFA
    {
        public DFA12(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 12;
            this.eot = DFA12_eot;
            this.eof = DFA12_eof;
            this.min = DFA12_min;
            this.max = DFA12_max;
            this.accept = DFA12_accept;
            this.special = DFA12_special;
            this.transition = DFA12_transition;

        }

        override public string Description
        {
            get { return "215:1: notExpression : ( logicalExpression | NOT logicalExpression -> ^( NOT logicalExpression ) );"; }
        }

    }


    protected internal int DFA12_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA12_13 = input.LA(1);

                   	 
                   	int index12_13 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred37_GAMS()) ) { s = 1; }

                   	else if ( (true) ) { s = 16; }

                   	 
                   	input.Seek(index12_13);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA12_14 = input.LA(1);

                   	 
                   	int index12_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred37_GAMS()) ) { s = 1; }

                   	else if ( (true) ) { s = 16; }

                   	 
                   	input.Seek(index12_14);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA12_15 = input.LA(1);

                   	 
                   	int index12_15 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred37_GAMS()) ) { s = 1; }

                   	else if ( (true) ) { s = 16; }

                   	 
                   	input.Seek(index12_15);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA12_21 = input.LA(1);

                   	 
                   	int index12_21 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred37_GAMS()) ) { s = 1; }

                   	else if ( (true) ) { s = 16; }

                   	 
                   	input.Seek(index12_21);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA12_23 = input.LA(1);

                   	 
                   	int index12_23 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred37_GAMS()) ) { s = 1; }

                   	else if ( (true) ) { s = 16; }

                   	 
                   	input.Seek(index12_23);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA12_29 = input.LA(1);

                   	 
                   	int index12_29 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred37_GAMS()) ) { s = 1; }

                   	else if ( (true) ) { s = 16; }

                   	 
                   	input.Seek(index12_29);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae12 =
            new NoViableAltException(dfa.Description, 12, _s, input);
        dfa.Error(nvae12);
        throw nvae12;
    }
    const string DFA13_eotS =
        "\x1c\uffff";
    const string DFA13_eofS =
        "\x01\x01\x1b\uffff";
    const string DFA13_minS =
        "\x01\x1f\x0d\uffff\x01\x00\x0d\uffff";
    const string DFA13_maxS =
        "\x01\x46\x0d\uffff\x01\x00\x0d\uffff";
    const string DFA13_acceptS =
        "\x01\uffff\x01\x02\x19\uffff\x01\x01";
    const string DFA13_specialS =
        "\x0e\uffff\x01\x00\x0d\uffff}>";
    static readonly string[] DFA13_transitionS = {
            "\x02\x01\x0a\uffff\x03\x01\x01\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x02\x01\x02\uffff\x01\x01\x01\uffff\x01\x01\x01"+
            "\uffff\x04\x01\x06\x0e\x01\uffff\x01\x01",
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

    static readonly short[] DFA13_eot = DFA.UnpackEncodedString(DFA13_eotS);
    static readonly short[] DFA13_eof = DFA.UnpackEncodedString(DFA13_eofS);
    static readonly char[] DFA13_min = DFA.UnpackEncodedStringToUnsignedChars(DFA13_minS);
    static readonly char[] DFA13_max = DFA.UnpackEncodedStringToUnsignedChars(DFA13_maxS);
    static readonly short[] DFA13_accept = DFA.UnpackEncodedString(DFA13_acceptS);
    static readonly short[] DFA13_special = DFA.UnpackEncodedString(DFA13_specialS);
    static readonly short[][] DFA13_transition = DFA.UnpackEncodedStringArray(DFA13_transitionS);

    protected class DFA13 : DFA
    {
        public DFA13(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 13;
            this.eot = DFA13_eot;
            this.eof = DFA13_eof;
            this.min = DFA13_min;
            this.max = DFA13_max;
            this.accept = DFA13_accept;
            this.special = DFA13_special;
            this.transition = DFA13_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 217:40: ( logical additiveExpression )*"; }
        }

    }


    protected internal int DFA13_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA13_14 = input.LA(1);

                   	 
                   	int index13_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred38_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index13_14);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae13 =
            new NoViableAltException(dfa.Description, 13, _s, input);
        dfa.Error(nvae13);
        throw nvae13;
    }
    const string DFA14_eotS =
        "\x1c\uffff";
    const string DFA14_eofS =
        "\x01\x01\x1b\uffff";
    const string DFA14_minS =
        "\x01\x1f\x0d\uffff\x01\x00\x0d\uffff";
    const string DFA14_maxS =
        "\x01\x46\x0d\uffff\x01\x00\x0d\uffff";
    const string DFA14_acceptS =
        "\x01\uffff\x01\x02\x19\uffff\x01\x01";
    const string DFA14_specialS =
        "\x0e\uffff\x01\x00\x0d\uffff}>";
    static readonly string[] DFA14_transitionS = {
            "\x02\x01\x0a\uffff\x03\x01\x01\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x02\x01\x02\uffff\x01\x0e\x01\uffff\x01\x0e\x01"+
            "\uffff\x0a\x01\x01\uffff\x01\x01",
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
            get { return "()* loopback of 219:48: ( ( PLUS | MINUS ) multiplicativeExpression )*"; }
        }

    }


    protected internal int DFA14_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA14_14 = input.LA(1);

                   	 
                   	int index14_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred40_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index14_14);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae14 =
            new NoViableAltException(dfa.Description, 14, _s, input);
        dfa.Error(nvae14);
        throw nvae14;
    }
    const string DFA15_eotS =
        "\x1c\uffff";
    const string DFA15_eofS =
        "\x01\x01\x1b\uffff";
    const string DFA15_minS =
        "\x01\x1f\x0d\uffff\x01\x00\x0d\uffff";
    const string DFA15_maxS =
        "\x01\x46\x0d\uffff\x01\x00\x0d\uffff";
    const string DFA15_acceptS =
        "\x01\uffff\x01\x02\x19\uffff\x01\x01";
    const string DFA15_specialS =
        "\x0e\uffff\x01\x00\x0d\uffff}>";
    static readonly string[] DFA15_transitionS = {
            "\x02\x01\x0a\uffff\x03\x01\x01\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x02\x01\x02\uffff\x01\x01\x01\uffff\x01\x01\x01"+
            "\uffff\x01\x01\x03\x0e\x06\x01\x01\uffff\x01\x01",
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
            get { return "()* loopback of 221:46: ( ( MULT | DIV | MOD ) powerExpression )*"; }
        }

    }


    protected internal int DFA15_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA15_14 = input.LA(1);

                   	 
                   	int index15_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred43_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index15_14);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae15 =
            new NoViableAltException(dfa.Description, 15, _s, input);
        dfa.Error(nvae15);
        throw nvae15;
    }
    const string DFA16_eotS =
        "\x1c\uffff";
    const string DFA16_eofS =
        "\x01\x01\x1b\uffff";
    const string DFA16_minS =
        "\x01\x1f\x0d\uffff\x01\x00\x0d\uffff";
    const string DFA16_maxS =
        "\x01\x46\x0d\uffff\x01\x00\x0d\uffff";
    const string DFA16_acceptS =
        "\x01\uffff\x01\x02\x19\uffff\x01\x01";
    const string DFA16_specialS =
        "\x0e\uffff\x01\x00\x0d\uffff}>";
    static readonly string[] DFA16_transitionS = {
            "\x02\x01\x0a\uffff\x03\x01\x01\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x02\x01\x02\uffff\x01\x01\x01\uffff\x01\x01\x01"+
            "\uffff\x0a\x01\x01\uffff\x01\x0e",
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

    static readonly short[] DFA16_eot = DFA.UnpackEncodedString(DFA16_eotS);
    static readonly short[] DFA16_eof = DFA.UnpackEncodedString(DFA16_eofS);
    static readonly char[] DFA16_min = DFA.UnpackEncodedStringToUnsignedChars(DFA16_minS);
    static readonly char[] DFA16_max = DFA.UnpackEncodedStringToUnsignedChars(DFA16_maxS);
    static readonly short[] DFA16_accept = DFA.UnpackEncodedString(DFA16_acceptS);
    static readonly short[] DFA16_special = DFA.UnpackEncodedString(DFA16_specialS);
    static readonly short[][] DFA16_transition = DFA.UnpackEncodedStringArray(DFA16_transitionS);

    protected class DFA16 : DFA
    {
        public DFA16(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 16;
            this.eot = DFA16_eot;
            this.eof = DFA16_eof;
            this.min = DFA16_min;
            this.max = DFA16_max;
            this.accept = DFA16_accept;
            this.special = DFA16_special;
            this.transition = DFA16_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 223:37: ( pow unaryExpression )*"; }
        }

    }


    protected internal int DFA16_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA16_14 = input.LA(1);

                   	 
                   	int index16_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred44_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index16_14);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae16 =
            new NoViableAltException(dfa.Description, 16, _s, input);
        dfa.Error(nvae16);
        throw nvae16;
    }
    const string DFA17_eotS =
        "\x0c\uffff";
    const string DFA17_eofS =
        "\x0c\uffff";
    const string DFA17_minS =
        "\x01\x1e\x0b\uffff";
    const string DFA17_maxS =
        "\x01\x45\x0b\uffff";
    const string DFA17_acceptS =
        "\x01\uffff\x01\x01\x09\uffff\x01\x02";
    const string DFA17_specialS =
        "\x0c\uffff}>";
    static readonly string[] DFA17_transitionS = {
            "\x0d\x01\x03\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x03\uffff\x01\x01\x01\uffff\x01\x01\x01\x0b\x01\x01\x0a"+
            "\uffff\x01\x01",
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
            get { return "225:1: unaryExpression : ( dollarExpression | MINUS dollarExpression -> ^( NEGATE dollarExpression ) );"; }
        }

    }

    const string DFA18_eotS =
        "\x1d\uffff";
    const string DFA18_eofS =
        "\x01\x02\x1c\uffff";
    const string DFA18_minS =
        "\x01\x1f\x01\x00\x1b\uffff";
    const string DFA18_maxS =
        "\x01\x46\x01\x00\x1b\uffff";
    const string DFA18_acceptS =
        "\x02\uffff\x01\x02\x19\uffff\x01\x01";
    const string DFA18_specialS =
        "\x01\uffff\x01\x00\x1b\uffff}>";
    static readonly string[] DFA18_transitionS = {
            "\x02\x02\x0a\uffff\x03\x02\x01\uffff\x01\x02\x01\uffff\x01"+
            "\x02\x01\uffff\x02\x02\x02\uffff\x01\x02\x01\uffff\x01\x02\x01"+
            "\uffff\x01\x01\x09\x02\x01\uffff\x01\x02",
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

    static readonly short[] DFA18_eot = DFA.UnpackEncodedString(DFA18_eotS);
    static readonly short[] DFA18_eof = DFA.UnpackEncodedString(DFA18_eofS);
    static readonly char[] DFA18_min = DFA.UnpackEncodedStringToUnsignedChars(DFA18_minS);
    static readonly char[] DFA18_max = DFA.UnpackEncodedStringToUnsignedChars(DFA18_maxS);
    static readonly short[] DFA18_accept = DFA.UnpackEncodedString(DFA18_acceptS);
    static readonly short[] DFA18_special = DFA.UnpackEncodedString(DFA18_specialS);
    static readonly short[][] DFA18_transition = DFA.UnpackEncodedStringArray(DFA18_transitionS);

    protected class DFA18 : DFA
    {
        public DFA18(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 18;
            this.eot = DFA18_eot;
            this.eof = DFA18_eof;
            this.min = DFA18_min;
            this.max = DFA18_max;
            this.accept = DFA18_accept;
            this.special = DFA18_special;
            this.transition = DFA18_transition;

        }

        override public string Description
        {
            get { return "228:37: ( conditional )?"; }
        }

    }


    protected internal int DFA18_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA18_1 = input.LA(1);

                   	 
                   	int index18_1 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred46_GAMS()) ) { s = 28; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index18_1);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae18 =
            new NoViableAltException(dfa.Description, 18, _s, input);
        dfa.Error(nvae18);
        throw nvae18;
    }
    const string DFA19_eotS =
        "\x0b\uffff";
    const string DFA19_eofS =
        "\x0b\uffff";
    const string DFA19_minS =
        "\x01\x1e\x0a\uffff";
    const string DFA19_maxS =
        "\x01\x45\x0a\uffff";
    const string DFA19_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x06\uffff";
    const string DFA19_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA19_transitionS = {
            "\x0d\x04\x03\uffff\x01\x01\x01\uffff\x01\x02\x01\uffff\x01"+
            "\x03\x03\uffff\x01\x04\x01\uffff\x01\x04\x01\uffff\x01\x04\x0a"+
            "\uffff\x01\x04",
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
            get { return "230:1: primaryExpression : ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value );"; }
        }

    }

    const string DFA20_eotS =
        "\x56\uffff";
    const string DFA20_eofS =
        "\x56\uffff";
    const string DFA20_minS =
        "\x01\x1e\x02\uffff\x02\x2e\x02\x00\x4f\uffff";
    const string DFA20_maxS =
        "\x01\x45\x02\uffff\x02\x32\x02\x00\x4f\uffff";
    const string DFA20_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x04\uffff\x01\x07\x01\x03\x12\uffff"+
        "\x01\x05\x01\x06\x01\x04\x38\uffff";
    const string DFA20_specialS =
        "\x03\uffff\x01\x00\x01\x01\x01\x02\x01\x03\x4f\uffff}>";
    static readonly string[] DFA20_transitionS = {
            "\x01\x03\x03\x06\x09\x04\x0b\uffff\x01\x07\x01\uffff\x01\x01"+
            "\x01\uffff\x01\x02\x0a\uffff\x01\x05",
            "",
            "",
            "\x01\x08\x01\uffff\x01\x08\x01\uffff\x01\x08",
            "\x01\x1d\x01\uffff\x01\x1d\x01\uffff\x01\x1d",
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
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA20_eot = DFA.UnpackEncodedString(DFA20_eotS);
    static readonly short[] DFA20_eof = DFA.UnpackEncodedString(DFA20_eofS);
    static readonly char[] DFA20_min = DFA.UnpackEncodedStringToUnsignedChars(DFA20_minS);
    static readonly char[] DFA20_max = DFA.UnpackEncodedStringToUnsignedChars(DFA20_maxS);
    static readonly short[] DFA20_accept = DFA.UnpackEncodedString(DFA20_acceptS);
    static readonly short[] DFA20_special = DFA.UnpackEncodedString(DFA20_specialS);
    static readonly short[][] DFA20_transition = DFA.UnpackEncodedStringArray(DFA20_transitionS);

    protected class DFA20 : DFA
    {
        public DFA20(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 20;
            this.eot = DFA20_eot;
            this.eof = DFA20_eof;
            this.min = DFA20_min;
            this.max = DFA20_max;
            this.accept = DFA20_accept;
            this.special = DFA20_special;
            this.transition = DFA20_transition;

        }

        override public string Description
        {
            get { return "238:1: value : ( Integer -> ^( ASTINTEGER Integer ) | Double -> ^( ASTDOUBLE Double ) | sum -> ^( ASTSUM sum ) | function | variableWithIndexerEtc -> ^( ASTDEFINITION variableWithIndexerEtc ) | ident -> ^( ASTVARIABLE ident ) | StringInQuotes );"; }
        }

    }


    protected internal int DFA20_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA20_3 = input.LA(1);

                   	 
                   	int index20_3 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA20_3 == L1 || LA20_3 == L2 || LA20_3 == L3) ) { s = 8; }

                   	else if ( (synpred59_GAMS()) ) { s = 27; }

                   	else if ( (synpred60_GAMS()) ) { s = 28; }

                   	 
                   	input.Seek(index20_3);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA20_4 = input.LA(1);

                   	 
                   	int index20_4 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA20_4 == L1 || LA20_4 == L2 || LA20_4 == L3) ) { s = 29; }

                   	else if ( (synpred59_GAMS()) ) { s = 27; }

                   	else if ( (synpred60_GAMS()) ) { s = 28; }

                   	 
                   	input.Seek(index20_4);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA20_5 = input.LA(1);

                   	 
                   	int index20_5 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred59_GAMS()) ) { s = 27; }

                   	else if ( (synpred60_GAMS()) ) { s = 28; }

                   	 
                   	input.Seek(index20_5);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA20_6 = input.LA(1);

                   	 
                   	int index20_6 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred59_GAMS()) ) { s = 27; }

                   	else if ( (synpred60_GAMS()) ) { s = 28; }

                   	 
                   	input.Seek(index20_6);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae20 =
            new NoViableAltException(dfa.Description, 20, _s, input);
        dfa.Error(nvae20);
        throw nvae20;
    }
 

    public static readonly BitSet FOLLOW_set_in_extraTokens0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr2_in_expr288 = new BitSet(new ulong[]{0x000007FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_EOF_in_expr292 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_equ_in_expr2312 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithIndexerEtc_in_equ329 = new BitSet(new ulong[]{0x0000080000000000UL});
    public static readonly BitSet FOLLOW_DOUBLEDOT_in_equ331 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_expression2_in_equ333 = new BitSet(new ulong[]{0x0000100000000000UL});
    public static readonly BitSet FOLLOW_EEQUAL_in_equ335 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_expression2_in_equ337 = new BitSet(new ulong[]{0x0000200000000000UL});
    public static readonly BitSet FOLLOW_SEMI_in_equ339 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_function367 = new BitSet(new ulong[]{0x0000400000000000UL});
    public static readonly BitSet FOLLOW_L1_in_function369 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_functionElements_in_function371 = new BitSet(new ulong[]{0x0000800000000000UL});
    public static readonly BitSet FOLLOW_R1_in_function373 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_function397 = new BitSet(new ulong[]{0x0001000000000000UL});
    public static readonly BitSet FOLLOW_L2_in_function399 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_functionElements_in_function401 = new BitSet(new ulong[]{0x0002000000000000UL});
    public static readonly BitSet FOLLOW_R2_in_function403 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_function427 = new BitSet(new ulong[]{0x0004000000000000UL});
    public static readonly BitSet FOLLOW_L3_in_function429 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_functionElements_in_function431 = new BitSet(new ulong[]{0x0008000000000000UL});
    public static readonly BitSet FOLLOW_R3_in_function433 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_functionName0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_functionElements489 = new BitSet(new ulong[]{0x0010000000000002UL});
    public static readonly BitSet FOLLOW_COMMA_in_functionElements492 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_expression_in_functionElements494 = new BitSet(new ulong[]{0x0010000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableWithIndexerEtc511 = new BitSet(new ulong[]{0x0825400000000002UL});
    public static readonly BitSet FOLLOW_DOT_in_variableWithIndexerEtc514 = new BitSet(new ulong[]{0x000007FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_variable_in_variableWithIndexerEtc516 = new BitSet(new ulong[]{0x0805400000000002UL});
    public static readonly BitSet FOLLOW_idx_in_variableWithIndexerEtc520 = new BitSet(new ulong[]{0x0800000000000002UL});
    public static readonly BitSet FOLLOW_conditional_in_variableWithIndexerEtc523 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_idx532 = new BitSet(new ulong[]{0x004007FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_indexerElements_in_idx534 = new BitSet(new ulong[]{0x0000800000000000UL});
    public static readonly BitSet FOLLOW_R1_in_idx536 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_idx543 = new BitSet(new ulong[]{0x004007FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_indexerElements_in_idx545 = new BitSet(new ulong[]{0x0002000000000000UL});
    public static readonly BitSet FOLLOW_R2_in_idx547 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_idx554 = new BitSet(new ulong[]{0x004007FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_indexerElements_in_idx556 = new BitSet(new ulong[]{0x0008000000000000UL});
    public static readonly BitSet FOLLOW_R3_in_idx558 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableLagLead_in_indexerElements584 = new BitSet(new ulong[]{0x0010000000000002UL});
    public static readonly BitSet FOLLOW_COMMA_in_indexerElements587 = new BitSet(new ulong[]{0x004007FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_variableLagLead_in_indexerElements589 = new BitSet(new ulong[]{0x0010000000000002UL});
    public static readonly BitSet FOLLOW_StringInQuotes_in_variableLagLead603 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead607 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead611 = new BitSet(new ulong[]{0x0080000000000000UL});
    public static readonly BitSet FOLLOW_PLUS_in_variableLagLead613 = new BitSet(new ulong[]{0x0100000000000000UL});
    public static readonly BitSet FOLLOW_Integer_in_variableLagLead615 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead619 = new BitSet(new ulong[]{0x0200000000000000UL});
    public static readonly BitSet FOLLOW_MINUS_in_variableLagLead621 = new BitSet(new ulong[]{0x0100000000000000UL});
    public static readonly BitSet FOLLOW_Integer_in_variableLagLead623 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_expression2632 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_number646 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DOLLAR_in_conditional657 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_expression_in_conditional659 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_andExpression_in_expression682 = new BitSet(new ulong[]{0x0000000100000002UL});
    public static readonly BitSet FOLLOW_OR_in_expression685 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_andExpression_in_expression688 = new BitSet(new ulong[]{0x0000000100000002UL});
    public static readonly BitSet FOLLOW_notExpression_in_andExpression698 = new BitSet(new ulong[]{0x0000000080000002UL});
    public static readonly BitSet FOLLOW_AND_in_andExpression701 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_notExpression_in_andExpression704 = new BitSet(new ulong[]{0x0000000080000002UL});
    public static readonly BitSet FOLLOW_logicalExpression_in_notExpression714 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NOT_in_notExpression726 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_logicalExpression_in_notExpression728 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_logicalExpression743 = new BitSet(new ulong[]{0x8000000000000002UL,0x000000000000001FUL});
    public static readonly BitSet FOLLOW_logical_in_logicalExpression746 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_logicalExpression749 = new BitSet(new ulong[]{0x8000000000000002UL,0x000000000000001FUL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression760 = new BitSet(new ulong[]{0x0280000000000002UL});
    public static readonly BitSet FOLLOW_set_in_additiveExpression764 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression771 = new BitSet(new ulong[]{0x0280000000000002UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression784 = new BitSet(new ulong[]{0x7000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpression788 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression797 = new BitSet(new ulong[]{0x7000000000000002UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression810 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000040UL});
    public static readonly BitSet FOLLOW_pow_in_powerExpression814 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression817 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000040UL});
    public static readonly BitSet FOLLOW_dollarExpression_in_unaryExpression831 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MINUS_in_unaryExpression842 = new BitSet(new ulong[]{0x054547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_dollarExpression_in_unaryExpression844 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_dollarExpression863 = new BitSet(new ulong[]{0x0800000000000002UL});
    public static readonly BitSet FOLLOW_conditional_in_dollarExpression865 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_primaryExpression892 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression894 = new BitSet(new ulong[]{0x0000800000000000UL});
    public static readonly BitSet FOLLOW_R1_in_primaryExpression896 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_primaryExpression911 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression913 = new BitSet(new ulong[]{0x0002000000000000UL});
    public static readonly BitSet FOLLOW_R2_in_primaryExpression915 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_primaryExpression932 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression934 = new BitSet(new ulong[]{0x0008000000000000UL});
    public static readonly BitSet FOLLOW_R3_in_primaryExpression936 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_value_in_primaryExpression951 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_logical0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Integer_in_value992 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Double_in_value1007 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_sum_in_value1026 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_value1048 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithIndexerEtc_in_value1053 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_value1068 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_StringInQuotes_in_value1088 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUM_in_sum1098 = new BitSet(new ulong[]{0x0000400000000000UL});
    public static readonly BitSet FOLLOW_L1_in_sum1100 = new BitSet(new ulong[]{0x000547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum1102 = new BitSet(new ulong[]{0x0810000000000000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum1104 = new BitSet(new ulong[]{0x0010000000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum1107 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_expression_in_sum1109 = new BitSet(new ulong[]{0x0000800000000000UL});
    public static readonly BitSet FOLLOW_R1_in_sum1111 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUM_in_sum1119 = new BitSet(new ulong[]{0x0001000000000000UL});
    public static readonly BitSet FOLLOW_L2_in_sum1121 = new BitSet(new ulong[]{0x000547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum1123 = new BitSet(new ulong[]{0x0810000000000000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum1125 = new BitSet(new ulong[]{0x0010000000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum1128 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_expression_in_sum1130 = new BitSet(new ulong[]{0x0002000000000000UL});
    public static readonly BitSet FOLLOW_R2_in_sum1132 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUM_in_sum1140 = new BitSet(new ulong[]{0x0004000000000000UL});
    public static readonly BitSet FOLLOW_L3_in_sum1142 = new BitSet(new ulong[]{0x000547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum1144 = new BitSet(new ulong[]{0x0810000000000000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum1146 = new BitSet(new ulong[]{0x0010000000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum1149 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_expression_in_sum1151 = new BitSet(new ulong[]{0x0008000000000000UL});
    public static readonly BitSet FOLLOW_R3_in_sum1153 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableWithDot1160 = new BitSet(new ulong[]{0x0020000000000002UL});
    public static readonly BitSet FOLLOW_DOT_in_variableWithDot1163 = new BitSet(new ulong[]{0x000007FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_variable_in_variableWithDot1165 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_sumControlled1184 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_sumControlled1191 = new BitSet(new ulong[]{0x004007FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled1193 = new BitSet(new ulong[]{0x0000800000000000UL});
    public static readonly BitSet FOLLOW_R1_in_sumControlled1195 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_sumControlled1201 = new BitSet(new ulong[]{0x004007FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled1203 = new BitSet(new ulong[]{0x0002000000000000UL});
    public static readonly BitSet FOLLOW_R2_in_sumControlled1205 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_sumControlled1211 = new BitSet(new ulong[]{0x004007FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled1213 = new BitSet(new ulong[]{0x0008000000000000UL});
    public static readonly BitSet FOLLOW_R3_in_sumControlled1215 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_numberPlusMinus1226 = new BitSet(new ulong[]{0x0500000000000000UL});
    public static readonly BitSet FOLLOW_set_in_numberPlusMinus1232 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Ident_in_ident1250 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_extraTokens_in_ident1254 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_variable1261 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STARS_in_pow1279 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_conditional_in_synpred27_GAMS523 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_OR_in_synpred35_GAMS685 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_andExpression_in_synpred35_GAMS688 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_AND_in_synpred36_GAMS701 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_notExpression_in_synpred36_GAMS704 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_logicalExpression_in_synpred37_GAMS714 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_logical_in_synpred38_GAMS746 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_synpred38_GAMS749 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_synpred40_GAMS764 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_synpred40_GAMS771 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_synpred43_GAMS788 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_powerExpression_in_synpred43_GAMS797 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_pow_in_synpred44_GAMS814 = new BitSet(new ulong[]{0x074547FFC0000000UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_synpred44_GAMS817 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_conditional_in_synpred46_GAMS865 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithIndexerEtc_in_synpred59_GAMS1053 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_synpred60_GAMS1068 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}