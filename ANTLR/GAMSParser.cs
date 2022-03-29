// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 GAMS.g 2022-03-28 02:09:14

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
		"ASTVAR", 
		"ASTVARIABLEWITHINDEXERETC", 
		"ASTDOT", 
		"ASTIDX", 
		"ASTEQU0", 
		"ASTEQU1", 
		"ASTEQU2", 
		"ASTEQU3", 
		"ASTVARWI", 
		"ASTVARWI0", 
		"ASTVARWI1", 
		"ASTVARWI2", 
		"ASTVARWI3", 
		"ASTVARWI4", 
		"ASTIDX0", 
		"ASTIDXELEMENTS", 
		"ASTIDXELEMENTS1", 
		"ASTIDXELEMENTS0", 
		"ASTVARIABLEANDLEAD", 
		"ASTDOLLAREXPRESSION", 
		"ASTVALUE", 
		"ASTFUNCTION0", 
		"ASTFUNCTIONELEMENTS", 
		"ASTFUNCTIONELEMENTS0", 
		"ASTFUNCTIONELEMENTS1", 
		"ASTSUM0", 
		"ASTSUM1", 
		"ASTSUM2", 
		"ASTSUM3", 
		"ASTSUMCONTROLLEDSIMPLE", 
		"ASTSUMCONTROLLED", 
		"ASTSUMCONTROLLED0", 
		"ASTSUMCONTROLLED2", 
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
		"DOT", 
		"L1", 
		"R1", 
		"L2", 
		"R2", 
		"L3", 
		"R3", 
		"COMMA", 
		"StringInQuotes", 
		"PLUS", 
		"Integer", 
		"MINUS", 
		"DOLLAR", 
		"MULT", 
		"DIV", 
		"STARS", 
		"NONEQUAL", 
		"LESSTHANOREQUAL", 
		"GREATERTHANOREQUAL", 
		"EQUAL", 
		"LESSTHAN", 
		"GREATERTHAN", 
		"Double", 
		"Ident", 
		"NEWLINE2", 
		"NEWLINE3", 
		"COMMENT1", 
		"COMMENT2", 
		"EQU", 
		"MOD", 
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
    public const int ASTFUNCTIONELEMENTS1 = 54;
    public const int COMMENT1 = 105;
    public const int COMMENT2 = 106;
    public const int ASTFUNCTION3 = 29;
    public const int ASTFUNCTION2 = 28;
    public const int ASTVARIABLE = 16;
    public const int LETTER = 111;
    public const int MOD = 108;
    public const int ASTFUNCTION1 = 27;
    public const int ASTIDX = 33;
    public const int ASTFUNCTION0 = 51;
    public const int LOG = 69;
    public const int ASTINDEXES1 = 18;
    public const int ASTCONDITIONAL = 25;
    public const int DOUBLEDOT = 76;
    public const int ASTINDEXES3 = 20;
    public const int NOT = 66;
    public const int ASTVAR = 30;
    public const int ASTINDEXES2 = 19;
    public const int EOF = -1;
    public const int NONEQUAL = 95;
    public const int ASTINTEGER = 13;
    public const int TANH = 74;
    public const int ASTEQU = 5;
    public const int Comment = 114;
    public const int EXP = 68;
    public const int ASTDOLLAREXPRESSION = 49;
    public const int EEQUAL = 77;
    public const int SQR = 73;
    public const int GREATERTHANOREQUAL = 97;
    public const int ASTFUNCTIONELEMENTS0 = 53;
    public const int GREATERTHAN = 100;
    public const int D = 119;
    public const int Double = 101;
    public const int ASTEQU1 = 35;
    public const int E = 120;
    public const int ASTEQU2 = 36;
    public const int F = 121;
    public const int G = 122;
    public const int ASTEQU0 = 34;
    public const int A = 116;
    public const int B = 117;
    public const int ASTEQU3 = 37;
    public const int C = 118;
    public const int L = 127;
    public const int M = 128;
    public const int N = 129;
    public const int NESTED_ML_COMMENT = 115;
    public const int ASTVARWI4 = 43;
    public const int O = 130;
    public const int ASTVARWI3 = 42;
    public const int H = 123;
    public const int ASTVARWI2 = 41;
    public const int ASTFUNCTIONELEMENTS = 52;
    public const int I = 124;
    public const int ASTVARWI1 = 40;
    public const int J = 125;
    public const int NEWLINE2 = 103;
    public const int ASTVARWI0 = 39;
    public const int K = 126;
    public const int NEWLINE3 = 104;
    public const int U = 136;
    public const int T = 135;
    public const int W = 138;
    public const int WHITESPACE = 112;
    public const int POWER = 72;
    public const int V = 137;
    public const int Q = 132;
    public const int P = 131;
    public const int S = 134;
    public const int R = 133;
    public const int MULT = 92;
    public const int ASTVARWI = 38;
    public const int Y = 140;
    public const int ASTIDXELEMENTS1 = 46;
    public const int X = 139;
    public const int ASTIDXELEMENTS0 = 47;
    public const int Z = 141;
    public const int ABS = 67;
    public const int Ident = 102;
    public const int ASTEXPRESSION = 8;
    public const int OR = 65;
    public const int StringInQuotes = 87;
    public const int ASTSUM = 26;
    public const int ASTDEFINITION = 24;
    public const int DOLLAR = 91;
    public const int ASTFUNCTION = 12;
    public const int ASTEQUCODE = 7;
    public const int MAX = 70;
    public const int Exponent = 110;
    public const int R2 = 83;
    public const int R3 = 85;
    public const int SUM = 63;
    public const int AND = 64;
    public const int COMMA = 86;
    public const int R1 = 81;
    public const int EQUAL = 98;
    public const int ASTSIMPLEFUNCTION1 = 9;
    public const int ASTSIMPLEFUNCTION2 = 10;
    public const int LESSTHANOREQUAL = 96;
    public const int ASTSIMPLEFUNCTION3 = 11;
    public const int ASTEND = 17;
    public const int PLUS = 88;
    public const int DIGIT = 109;
    public const int ASTSUMCONTROLLED = 60;
    public const int DOT = 79;
    public const int ASTSUMCONTROLLEDSIMPLE = 59;
    public const int ASTEXPRESSION2 = 22;
    public const int ASTEXPRESSION3 = 23;
    public const int ASTIDXELEMENTS = 45;
    public const int LESSTHAN = 99;
    public const int ASTVALUE = 50;
    public const int ASTVARIABLEWITHINDEXERETC = 31;
    public const int ASTIDX0 = 44;
    public const int ASTEXPRESSION1 = 21;
    public const int NEGATE = 4;
    public const int SAMEAS = 75;
    public const int MIN = 71;
    public const int MINUS = 90;
    public const int ASTVARIABLEANDLEAD = 48;
    public const int SEMI = 78;
    public const int L1 = 80;
    public const int ASTSUM0 = 55;
    public const int L2 = 82;
    public const int L3 = 84;
    public const int ASTLEFTSIDE = 6;
    public const int NEWLINE = 113;
    public const int ASTSUM2 = 57;
    public const int ASTSUMCONTROLLED2 = 62;
    public const int ASTSUM1 = 56;
    public const int ASTSUMCONTROLLED0 = 61;
    public const int ASTSUM3 = 58;
    public const int EQU = 107;
    public const int STARS = 94;
    public const int ASTDOUBLE = 14;
    public const int DIV = 93;
    public const int Integer = 89;
    public const int ASTDOT = 32;

    // delegates
    // delegators



        public GAMSParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public GAMSParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();
            this.state.ruleMemo = new Hashtable[91+1];
             
             
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
            errors.Add(e.Line + "�" + e.CharPositionInLine + "�" + hdr + "�" + msg);
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
    // GAMS.g:160:1: extraTokens : ( SUM | AND | OR | NOT | ABS | EXP | LOG | MAX | MIN | POWER | SQR | TANH | SAMEAS );
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
            // GAMS.g:160:13: ( SUM | AND | OR | NOT | ABS | EXP | LOG | MAX | MIN | POWER | SQR | TANH | SAMEAS )
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
    // GAMS.g:180:1: expr : equ EOF ;
    public GAMSParser.expr_return expr() // throws RecognitionException [1]
    {   
        GAMSParser.expr_return retval = new GAMSParser.expr_return();
        retval.Start = input.LT(1);
        int expr_StartIndex = input.Index();
        object root_0 = null;

        IToken EOF3 = null;
        GAMSParser.equ_return equ2 = default(GAMSParser.equ_return);


        object EOF3_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 2) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:180:5: ( equ EOF )
            // GAMS.g:180:7: equ EOF
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_equ_in_expr453);
            	equ2 = equ();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, equ2.Tree);
            	EOF3=(IToken)Match(input,EOF,FOLLOW_EOF_in_expr455); if (state.failed) return retval;
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
    // GAMS.g:182:1: equ : variableWithIndexerEtc DOUBLEDOT expression EEQUAL expression SEMI -> ^( ASTEQU ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI ) ^( ASTEQU1 variableWithIndexerEtc ) ^( ASTEQU2 expression ) ^( ASTEQU3 expression ) ) ;
    public GAMSParser.equ_return equ() // throws RecognitionException [1]
    {   
        GAMSParser.equ_return retval = new GAMSParser.equ_return();
        retval.Start = input.LT(1);
        int equ_StartIndex = input.Index();
        object root_0 = null;

        IToken DOUBLEDOT5 = null;
        IToken EEQUAL7 = null;
        IToken SEMI9 = null;
        GAMSParser.variableWithIndexerEtc_return variableWithIndexerEtc4 = default(GAMSParser.variableWithIndexerEtc_return);

        GAMSParser.expression_return expression6 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression8 = default(GAMSParser.expression_return);


        object DOUBLEDOT5_tree=null;
        object EEQUAL7_tree=null;
        object SEMI9_tree=null;
        RewriteRuleTokenStream stream_EEQUAL = new RewriteRuleTokenStream(adaptor,"token EEQUAL");
        RewriteRuleTokenStream stream_DOUBLEDOT = new RewriteRuleTokenStream(adaptor,"token DOUBLEDOT");
        RewriteRuleTokenStream stream_SEMI = new RewriteRuleTokenStream(adaptor,"token SEMI");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_variableWithIndexerEtc = new RewriteRuleSubtreeStream(adaptor,"rule variableWithIndexerEtc");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 3) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:182:4: ( variableWithIndexerEtc DOUBLEDOT expression EEQUAL expression SEMI -> ^( ASTEQU ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI ) ^( ASTEQU1 variableWithIndexerEtc ) ^( ASTEQU2 expression ) ^( ASTEQU3 expression ) ) )
            // GAMS.g:182:9: variableWithIndexerEtc DOUBLEDOT expression EEQUAL expression SEMI
            {
            	PushFollow(FOLLOW_variableWithIndexerEtc_in_equ469);
            	variableWithIndexerEtc4 = variableWithIndexerEtc();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableWithIndexerEtc.Add(variableWithIndexerEtc4.Tree);
            	DOUBLEDOT5=(IToken)Match(input,DOUBLEDOT,FOLLOW_DOUBLEDOT_in_equ471); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_DOUBLEDOT.Add(DOUBLEDOT5);

            	PushFollow(FOLLOW_expression_in_equ473);
            	expression6 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression6.Tree);
            	EEQUAL7=(IToken)Match(input,EEQUAL,FOLLOW_EEQUAL_in_equ475); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_EEQUAL.Add(EEQUAL7);

            	PushFollow(FOLLOW_expression_in_equ477);
            	expression8 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression8.Tree);
            	SEMI9=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_equ479); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI9);

            	if ( (state.backtracking==0) )
            	{
            	  equItems.Add(input.ToString((IToken)retval.Start,input.LT(-1)));
            	}


            	// AST REWRITE
            	// elements:          DOUBLEDOT, SEMI, expression, EEQUAL, variableWithIndexerEtc, expression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 183:3: -> ^( ASTEQU ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI ) ^( ASTEQU1 variableWithIndexerEtc ) ^( ASTEQU2 expression ) ^( ASTEQU3 expression ) )
            	{
            	    // GAMS.g:183:6: ^( ASTEQU ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI ) ^( ASTEQU1 variableWithIndexerEtc ) ^( ASTEQU2 expression ) ^( ASTEQU3 expression ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU, "ASTEQU"), root_1);

            	    // GAMS.g:183:15: ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU0, "ASTEQU0"), root_2);

            	    adaptor.AddChild(root_2, stream_DOUBLEDOT.NextNode());
            	    adaptor.AddChild(root_2, stream_EEQUAL.NextNode());
            	    adaptor.AddChild(root_2, stream_SEMI.NextNode());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:183:48: ^( ASTEQU1 variableWithIndexerEtc )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU1, "ASTEQU1"), root_2);

            	    adaptor.AddChild(root_2, stream_variableWithIndexerEtc.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:183:82: ^( ASTEQU2 expression )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU2, "ASTEQU2"), root_2);

            	    adaptor.AddChild(root_2, stream_expression.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:183:104: ^( ASTEQU3 expression )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU3, "ASTEQU3"), root_2);

            	    adaptor.AddChild(root_2, stream_expression.NextTree());

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
            	Memoize(input, 3, equ_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "equ"

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
    // GAMS.g:189:1: variableWithIndexerEtc : variable ( DOT variable )? ( idx )? ( conditional )? -> ^( ASTVARWI ^( ASTVARWI0 ( DOT )? ) ^( ASTVARWI1 variable ) ^( ASTVARWI2 ( variable )? ) ^( ASTVARWI3 ( idx )? ) ^( ASTVARWI4 ( conditional )? ) ) ;
    public GAMSParser.variableWithIndexerEtc_return variableWithIndexerEtc() // throws RecognitionException [1]
    {   
        GAMSParser.variableWithIndexerEtc_return retval = new GAMSParser.variableWithIndexerEtc_return();
        retval.Start = input.LT(1);
        int variableWithIndexerEtc_StartIndex = input.Index();
        object root_0 = null;

        IToken DOT11 = null;
        GAMSParser.variable_return variable10 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable12 = default(GAMSParser.variable_return);

        GAMSParser.idx_return idx13 = default(GAMSParser.idx_return);

        GAMSParser.conditional_return conditional14 = default(GAMSParser.conditional_return);


        object DOT11_tree=null;
        RewriteRuleTokenStream stream_DOT = new RewriteRuleTokenStream(adaptor,"token DOT");
        RewriteRuleSubtreeStream stream_idx = new RewriteRuleSubtreeStream(adaptor,"rule idx");
        RewriteRuleSubtreeStream stream_conditional = new RewriteRuleSubtreeStream(adaptor,"rule conditional");
        RewriteRuleSubtreeStream stream_variable = new RewriteRuleSubtreeStream(adaptor,"rule variable");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 4) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:189:23: ( variable ( DOT variable )? ( idx )? ( conditional )? -> ^( ASTVARWI ^( ASTVARWI0 ( DOT )? ) ^( ASTVARWI1 variable ) ^( ASTVARWI2 ( variable )? ) ^( ASTVARWI3 ( idx )? ) ^( ASTVARWI4 ( conditional )? ) ) )
            // GAMS.g:189:25: variable ( DOT variable )? ( idx )? ( conditional )?
            {
            	PushFollow(FOLLOW_variable_in_variableWithIndexerEtc529);
            	variable10 = variable();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variable.Add(variable10.Tree);
            	// GAMS.g:189:34: ( DOT variable )?
            	int alt1 = 2;
            	alt1 = dfa1.Predict(input);
            	switch (alt1) 
            	{
            	    case 1 :
            	        // GAMS.g:189:35: DOT variable
            	        {
            	        	DOT11=(IToken)Match(input,DOT,FOLLOW_DOT_in_variableWithIndexerEtc532); if (state.failed) return retval; 
            	        	if ( (state.backtracking==0) ) stream_DOT.Add(DOT11);

            	        	PushFollow(FOLLOW_variable_in_variableWithIndexerEtc534);
            	        	variable12 = variable();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_variable.Add(variable12.Tree);

            	        }
            	        break;

            	}

            	// GAMS.g:189:50: ( idx )?
            	int alt2 = 2;
            	alt2 = dfa2.Predict(input);
            	switch (alt2) 
            	{
            	    case 1 :
            	        // GAMS.g:0:0: idx
            	        {
            	        	PushFollow(FOLLOW_idx_in_variableWithIndexerEtc538);
            	        	idx13 = idx();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_idx.Add(idx13.Tree);

            	        }
            	        break;

            	}

            	// GAMS.g:189:55: ( conditional )?
            	int alt3 = 2;
            	alt3 = dfa3.Predict(input);
            	switch (alt3) 
            	{
            	    case 1 :
            	        // GAMS.g:0:0: conditional
            	        {
            	        	PushFollow(FOLLOW_conditional_in_variableWithIndexerEtc541);
            	        	conditional14 = conditional();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional14.Tree);

            	        }
            	        break;

            	}



            	// AST REWRITE
            	// elements:          variable, variable, DOT, conditional, idx
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 190:3: -> ^( ASTVARWI ^( ASTVARWI0 ( DOT )? ) ^( ASTVARWI1 variable ) ^( ASTVARWI2 ( variable )? ) ^( ASTVARWI3 ( idx )? ) ^( ASTVARWI4 ( conditional )? ) )
            	{
            	    // GAMS.g:190:6: ^( ASTVARWI ^( ASTVARWI0 ( DOT )? ) ^( ASTVARWI1 variable ) ^( ASTVARWI2 ( variable )? ) ^( ASTVARWI3 ( idx )? ) ^( ASTVARWI4 ( conditional )? ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI, "ASTVARWI"), root_1);

            	    // GAMS.g:190:17: ^( ASTVARWI0 ( DOT )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI0, "ASTVARWI0"), root_2);

            	    // GAMS.g:190:29: ( DOT )?
            	    if ( stream_DOT.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_DOT.NextNode());

            	    }
            	    stream_DOT.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:190:35: ^( ASTVARWI1 variable )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI1, "ASTVARWI1"), root_2);

            	    adaptor.AddChild(root_2, stream_variable.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:190:57: ^( ASTVARWI2 ( variable )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI2, "ASTVARWI2"), root_2);

            	    // GAMS.g:190:69: ( variable )?
            	    if ( stream_variable.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_variable.NextTree());

            	    }
            	    stream_variable.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:190:80: ^( ASTVARWI3 ( idx )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI3, "ASTVARWI3"), root_2);

            	    // GAMS.g:190:92: ( idx )?
            	    if ( stream_idx.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_idx.NextTree());

            	    }
            	    stream_idx.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:190:98: ^( ASTVARWI4 ( conditional )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI4, "ASTVARWI4"), root_2);

            	    // GAMS.g:190:110: ( conditional )?
            	    if ( stream_conditional.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_conditional.NextTree());

            	    }
            	    stream_conditional.Reset();

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
            	Memoize(input, 4, variableWithIndexerEtc_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "variableWithIndexerEtc"

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
    // GAMS.g:192:1: variable : ident ;
    public GAMSParser.variable_return variable() // throws RecognitionException [1]
    {   
        GAMSParser.variable_return retval = new GAMSParser.variable_return();
        retval.Start = input.LT(1);
        int variable_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.ident_return ident15 = default(GAMSParser.ident_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 5) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:192:10: ( ident )
            // GAMS.g:192:12: ident
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_ident_in_variable592);
            	ident15 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, ident15.Tree);

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
            	Memoize(input, 5, variable_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "variable"

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
    // GAMS.g:194:1: idx : ( L1 indexerElements R1 -> ^( ASTIDX ^( ASTIDX0 L1 R1 ) indexerElements ) | L2 indexerElements R2 -> ^( ASTIDX ^( ASTIDX0 L2 R2 ) indexerElements ) | L3 indexerElements R3 -> ^( ASTIDX ^( ASTIDX0 L3 R3 ) indexerElements ) );
    public GAMSParser.idx_return idx() // throws RecognitionException [1]
    {   
        GAMSParser.idx_return retval = new GAMSParser.idx_return();
        retval.Start = input.LT(1);
        int idx_StartIndex = input.Index();
        object root_0 = null;

        IToken L116 = null;
        IToken R118 = null;
        IToken L219 = null;
        IToken R221 = null;
        IToken L322 = null;
        IToken R324 = null;
        GAMSParser.indexerElements_return indexerElements17 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements20 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements23 = default(GAMSParser.indexerElements_return);


        object L116_tree=null;
        object R118_tree=null;
        object L219_tree=null;
        object R221_tree=null;
        object L322_tree=null;
        object R324_tree=null;
        RewriteRuleTokenStream stream_L1 = new RewriteRuleTokenStream(adaptor,"token L1");
        RewriteRuleTokenStream stream_L2 = new RewriteRuleTokenStream(adaptor,"token L2");
        RewriteRuleTokenStream stream_L3 = new RewriteRuleTokenStream(adaptor,"token L3");
        RewriteRuleTokenStream stream_R2 = new RewriteRuleTokenStream(adaptor,"token R2");
        RewriteRuleTokenStream stream_R3 = new RewriteRuleTokenStream(adaptor,"token R3");
        RewriteRuleTokenStream stream_R1 = new RewriteRuleTokenStream(adaptor,"token R1");
        RewriteRuleSubtreeStream stream_indexerElements = new RewriteRuleSubtreeStream(adaptor,"rule indexerElements");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 6) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:194:4: ( L1 indexerElements R1 -> ^( ASTIDX ^( ASTIDX0 L1 R1 ) indexerElements ) | L2 indexerElements R2 -> ^( ASTIDX ^( ASTIDX0 L2 R2 ) indexerElements ) | L3 indexerElements R3 -> ^( ASTIDX ^( ASTIDX0 L3 R3 ) indexerElements ) )
            int alt4 = 3;
            switch ( input.LA(1) ) 
            {
            case L1:
            	{
                alt4 = 1;
                }
                break;
            case L2:
            	{
                alt4 = 2;
                }
                break;
            case L3:
            	{
                alt4 = 3;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d4s0 =
            	        new NoViableAltException("", 4, 0, input);

            	    throw nvae_d4s0;
            }

            switch (alt4) 
            {
                case 1 :
                    // GAMS.g:194:6: L1 indexerElements R1
                    {
                    	L116=(IToken)Match(input,L1,FOLLOW_L1_in_idx599); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L116);

                    	PushFollow(FOLLOW_indexerElements_in_idx601);
                    	indexerElements17 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements17.Tree);
                    	R118=(IToken)Match(input,R1,FOLLOW_R1_in_idx603); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R118);



                    	// AST REWRITE
                    	// elements:          indexerElements, L1, R1
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 194:28: -> ^( ASTIDX ^( ASTIDX0 L1 R1 ) indexerElements )
                    	{
                    	    // GAMS.g:194:31: ^( ASTIDX ^( ASTIDX0 L1 R1 ) indexerElements )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDX, "ASTIDX"), root_1);

                    	    // GAMS.g:194:40: ^( ASTIDX0 L1 R1 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDX0, "ASTIDX0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L1.NextNode());
                    	    adaptor.AddChild(root_2, stream_R1.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    adaptor.AddChild(root_1, stream_indexerElements.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // GAMS.g:195:6: L2 indexerElements R2
                    {
                    	L219=(IToken)Match(input,L2,FOLLOW_L2_in_idx626); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L219);

                    	PushFollow(FOLLOW_indexerElements_in_idx628);
                    	indexerElements20 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements20.Tree);
                    	R221=(IToken)Match(input,R2,FOLLOW_R2_in_idx630); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R221);



                    	// AST REWRITE
                    	// elements:          indexerElements, L2, R2
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 195:28: -> ^( ASTIDX ^( ASTIDX0 L2 R2 ) indexerElements )
                    	{
                    	    // GAMS.g:195:31: ^( ASTIDX ^( ASTIDX0 L2 R2 ) indexerElements )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDX, "ASTIDX"), root_1);

                    	    // GAMS.g:195:40: ^( ASTIDX0 L2 R2 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDX0, "ASTIDX0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L2.NextNode());
                    	    adaptor.AddChild(root_2, stream_R2.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    adaptor.AddChild(root_1, stream_indexerElements.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 3 :
                    // GAMS.g:196:6: L3 indexerElements R3
                    {
                    	L322=(IToken)Match(input,L3,FOLLOW_L3_in_idx653); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L322);

                    	PushFollow(FOLLOW_indexerElements_in_idx655);
                    	indexerElements23 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements23.Tree);
                    	R324=(IToken)Match(input,R3,FOLLOW_R3_in_idx657); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R324);



                    	// AST REWRITE
                    	// elements:          indexerElements, R3, L3
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 196:28: -> ^( ASTIDX ^( ASTIDX0 L3 R3 ) indexerElements )
                    	{
                    	    // GAMS.g:196:31: ^( ASTIDX ^( ASTIDX0 L3 R3 ) indexerElements )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDX, "ASTIDX"), root_1);

                    	    // GAMS.g:196:40: ^( ASTIDX0 L3 R3 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDX0, "ASTIDX0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L3.NextNode());
                    	    adaptor.AddChild(root_2, stream_R3.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    adaptor.AddChild(root_1, stream_indexerElements.NextTree());

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
            	Memoize(input, 6, idx_StartIndex); 
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
    // GAMS.g:199:1: indexerElements : variableLagLead ( COMMA variableLagLead )* -> ^( ASTIDXELEMENTS ^( ASTIDXELEMENTS0 ( COMMA )* ) ^( ASTIDXELEMENTS1 ( variableLagLead )+ ) ) ;
    public GAMSParser.indexerElements_return indexerElements() // throws RecognitionException [1]
    {   
        GAMSParser.indexerElements_return retval = new GAMSParser.indexerElements_return();
        retval.Start = input.LT(1);
        int indexerElements_StartIndex = input.Index();
        object root_0 = null;

        IToken COMMA26 = null;
        GAMSParser.variableLagLead_return variableLagLead25 = default(GAMSParser.variableLagLead_return);

        GAMSParser.variableLagLead_return variableLagLead27 = default(GAMSParser.variableLagLead_return);


        object COMMA26_tree=null;
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleSubtreeStream stream_variableLagLead = new RewriteRuleSubtreeStream(adaptor,"rule variableLagLead");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 7) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:199:16: ( variableLagLead ( COMMA variableLagLead )* -> ^( ASTIDXELEMENTS ^( ASTIDXELEMENTS0 ( COMMA )* ) ^( ASTIDXELEMENTS1 ( variableLagLead )+ ) ) )
            // GAMS.g:199:18: variableLagLead ( COMMA variableLagLead )*
            {
            	PushFollow(FOLLOW_variableLagLead_in_indexerElements681);
            	variableLagLead25 = variableLagLead();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableLagLead.Add(variableLagLead25.Tree);
            	// GAMS.g:199:34: ( COMMA variableLagLead )*
            	do 
            	{
            	    int alt5 = 2;
            	    int LA5_0 = input.LA(1);

            	    if ( (LA5_0 == COMMA) )
            	    {
            	        alt5 = 1;
            	    }


            	    switch (alt5) 
            		{
            			case 1 :
            			    // GAMS.g:199:35: COMMA variableLagLead
            			    {
            			    	COMMA26=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_indexerElements684); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA26);

            			    	PushFollow(FOLLOW_variableLagLead_in_indexerElements686);
            			    	variableLagLead27 = variableLagLead();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_variableLagLead.Add(variableLagLead27.Tree);

            			    }
            			    break;

            			default:
            			    goto loop5;
            	    }
            	} while (true);

            	loop5:
            		;	// Stops C# compiler whining that label 'loop5' has no statements



            	// AST REWRITE
            	// elements:          variableLagLead, COMMA
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 200:3: -> ^( ASTIDXELEMENTS ^( ASTIDXELEMENTS0 ( COMMA )* ) ^( ASTIDXELEMENTS1 ( variableLagLead )+ ) )
            	{
            	    // GAMS.g:200:6: ^( ASTIDXELEMENTS ^( ASTIDXELEMENTS0 ( COMMA )* ) ^( ASTIDXELEMENTS1 ( variableLagLead )+ ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDXELEMENTS, "ASTIDXELEMENTS"), root_1);

            	    // GAMS.g:200:23: ^( ASTIDXELEMENTS0 ( COMMA )* )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDXELEMENTS0, "ASTIDXELEMENTS0"), root_2);

            	    // GAMS.g:200:41: ( COMMA )*
            	    while ( stream_COMMA.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_COMMA.NextNode());

            	    }
            	    stream_COMMA.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:200:49: ^( ASTIDXELEMENTS1 ( variableLagLead )+ )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDXELEMENTS1, "ASTIDXELEMENTS1"), root_2);

            	    if ( !(stream_variableLagLead.HasNext()) ) {
            	        throw new RewriteEarlyExitException();
            	    }
            	    while ( stream_variableLagLead.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_variableLagLead.NextTree());

            	    }
            	    stream_variableLagLead.Reset();

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
            	Memoize(input, 7, indexerElements_StartIndex); 
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
    // GAMS.g:202:1: variableLagLead : ( StringInQuotes -> ^( ASTVARIABLEANDLEAD StringInQuotes ) | variable PLUS Integer -> ^( ASTVARIABLEANDLEAD variable PLUS Integer ) | variable MINUS Integer -> ^( ASTVARIABLEANDLEAD variable MINUS Integer ) | variable -> ^( ASTVARIABLEANDLEAD variable ) );
    public GAMSParser.variableLagLead_return variableLagLead() // throws RecognitionException [1]
    {   
        GAMSParser.variableLagLead_return retval = new GAMSParser.variableLagLead_return();
        retval.Start = input.LT(1);
        int variableLagLead_StartIndex = input.Index();
        object root_0 = null;

        IToken StringInQuotes28 = null;
        IToken PLUS30 = null;
        IToken Integer31 = null;
        IToken MINUS33 = null;
        IToken Integer34 = null;
        GAMSParser.variable_return variable29 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable32 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable35 = default(GAMSParser.variable_return);


        object StringInQuotes28_tree=null;
        object PLUS30_tree=null;
        object Integer31_tree=null;
        object MINUS33_tree=null;
        object Integer34_tree=null;
        RewriteRuleTokenStream stream_StringInQuotes = new RewriteRuleTokenStream(adaptor,"token StringInQuotes");
        RewriteRuleTokenStream stream_PLUS = new RewriteRuleTokenStream(adaptor,"token PLUS");
        RewriteRuleTokenStream stream_MINUS = new RewriteRuleTokenStream(adaptor,"token MINUS");
        RewriteRuleTokenStream stream_Integer = new RewriteRuleTokenStream(adaptor,"token Integer");
        RewriteRuleSubtreeStream stream_variable = new RewriteRuleSubtreeStream(adaptor,"rule variable");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 8) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:202:16: ( StringInQuotes -> ^( ASTVARIABLEANDLEAD StringInQuotes ) | variable PLUS Integer -> ^( ASTVARIABLEANDLEAD variable PLUS Integer ) | variable MINUS Integer -> ^( ASTVARIABLEANDLEAD variable MINUS Integer ) | variable -> ^( ASTVARIABLEANDLEAD variable ) )
            int alt6 = 4;
            alt6 = dfa6.Predict(input);
            switch (alt6) 
            {
                case 1 :
                    // GAMS.g:202:18: StringInQuotes
                    {
                    	StringInQuotes28=(IToken)Match(input,StringInQuotes,FOLLOW_StringInQuotes_in_variableLagLead718); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_StringInQuotes.Add(StringInQuotes28);



                    	// AST REWRITE
                    	// elements:          StringInQuotes
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 202:43: -> ^( ASTVARIABLEANDLEAD StringInQuotes )
                    	{
                    	    // GAMS.g:202:46: ^( ASTVARIABLEANDLEAD StringInQuotes )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARIABLEANDLEAD, "ASTVARIABLEANDLEAD"), root_1);

                    	    adaptor.AddChild(root_1, stream_StringInQuotes.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // GAMS.g:203:18: variable PLUS Integer
                    {
                    	PushFollow(FOLLOW_variable_in_variableLagLead767);
                    	variable29 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variable.Add(variable29.Tree);
                    	PLUS30=(IToken)Match(input,PLUS,FOLLOW_PLUS_in_variableLagLead769); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_PLUS.Add(PLUS30);

                    	Integer31=(IToken)Match(input,Integer,FOLLOW_Integer_in_variableLagLead771); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer31);



                    	// AST REWRITE
                    	// elements:          Integer, PLUS, variable
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 203:43: -> ^( ASTVARIABLEANDLEAD variable PLUS Integer )
                    	{
                    	    // GAMS.g:203:46: ^( ASTVARIABLEANDLEAD variable PLUS Integer )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARIABLEANDLEAD, "ASTVARIABLEANDLEAD"), root_1);

                    	    adaptor.AddChild(root_1, stream_variable.NextTree());
                    	    adaptor.AddChild(root_1, stream_PLUS.NextNode());
                    	    adaptor.AddChild(root_1, stream_Integer.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 3 :
                    // GAMS.g:204:18: variable MINUS Integer
                    {
                    	PushFollow(FOLLOW_variable_in_variableLagLead817);
                    	variable32 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variable.Add(variable32.Tree);
                    	MINUS33=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_variableLagLead819); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_MINUS.Add(MINUS33);

                    	Integer34=(IToken)Match(input,Integer,FOLLOW_Integer_in_variableLagLead821); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer34);



                    	// AST REWRITE
                    	// elements:          variable, MINUS, Integer
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 204:43: -> ^( ASTVARIABLEANDLEAD variable MINUS Integer )
                    	{
                    	    // GAMS.g:204:46: ^( ASTVARIABLEANDLEAD variable MINUS Integer )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARIABLEANDLEAD, "ASTVARIABLEANDLEAD"), root_1);

                    	    adaptor.AddChild(root_1, stream_variable.NextTree());
                    	    adaptor.AddChild(root_1, stream_MINUS.NextNode());
                    	    adaptor.AddChild(root_1, stream_Integer.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 4 :
                    // GAMS.g:205:9: variable
                    {
                    	PushFollow(FOLLOW_variable_in_variableLagLead857);
                    	variable35 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variable.Add(variable35.Tree);


                    	// AST REWRITE
                    	// elements:          variable
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 205:34: -> ^( ASTVARIABLEANDLEAD variable )
                    	{
                    	    // GAMS.g:205:37: ^( ASTVARIABLEANDLEAD variable )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARIABLEANDLEAD, "ASTVARIABLEANDLEAD"), root_1);

                    	    adaptor.AddChild(root_1, stream_variable.NextTree());

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
            	Memoize(input, 8, variableLagLead_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "variableLagLead"

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
    // GAMS.g:208:1: conditional : DOLLAR expression -> ^( ASTCONDITIONAL DOLLAR expression ) ;
    public GAMSParser.conditional_return conditional() // throws RecognitionException [1]
    {   
        GAMSParser.conditional_return retval = new GAMSParser.conditional_return();
        retval.Start = input.LT(1);
        int conditional_StartIndex = input.Index();
        object root_0 = null;

        IToken DOLLAR36 = null;
        GAMSParser.expression_return expression37 = default(GAMSParser.expression_return);


        object DOLLAR36_tree=null;
        RewriteRuleTokenStream stream_DOLLAR = new RewriteRuleTokenStream(adaptor,"token DOLLAR");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 9) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:208:12: ( DOLLAR expression -> ^( ASTCONDITIONAL DOLLAR expression ) )
            // GAMS.g:208:14: DOLLAR expression
            {
            	DOLLAR36=(IToken)Match(input,DOLLAR,FOLLOW_DOLLAR_in_conditional901); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_DOLLAR.Add(DOLLAR36);

            	PushFollow(FOLLOW_expression_in_conditional903);
            	expression37 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression37.Tree);


            	// AST REWRITE
            	// elements:          DOLLAR, expression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 208:32: -> ^( ASTCONDITIONAL DOLLAR expression )
            	{
            	    // GAMS.g:208:35: ^( ASTCONDITIONAL DOLLAR expression )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTCONDITIONAL, "ASTCONDITIONAL"), root_1);

            	    adaptor.AddChild(root_1, stream_DOLLAR.NextNode());
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
            	Memoize(input, 9, conditional_StartIndex); 
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
    // GAMS.g:226:1: expression : andExpression ( OR andExpression )* ;
    public GAMSParser.expression_return expression() // throws RecognitionException [1]
    {   
        GAMSParser.expression_return retval = new GAMSParser.expression_return();
        retval.Start = input.LT(1);
        int expression_StartIndex = input.Index();
        object root_0 = null;

        IToken OR39 = null;
        GAMSParser.andExpression_return andExpression38 = default(GAMSParser.andExpression_return);

        GAMSParser.andExpression_return andExpression40 = default(GAMSParser.andExpression_return);


        object OR39_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 10) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:226:11: ( andExpression ( OR andExpression )* )
            // GAMS.g:226:13: andExpression ( OR andExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_andExpression_in_expression936);
            	andExpression38 = andExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, andExpression38.Tree);
            	// GAMS.g:226:27: ( OR andExpression )*
            	do 
            	{
            	    int alt7 = 2;
            	    alt7 = dfa7.Predict(input);
            	    switch (alt7) 
            		{
            			case 1 :
            			    // GAMS.g:226:28: OR andExpression
            			    {
            			    	OR39=(IToken)Match(input,OR,FOLLOW_OR_in_expression939); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{OR39_tree = (object)adaptor.Create(OR39);
            			    		root_0 = (object)adaptor.BecomeRoot(OR39_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_andExpression_in_expression942);
            			    	andExpression40 = andExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, andExpression40.Tree);

            			    }
            			    break;

            			default:
            			    goto loop7;
            	    }
            	} while (true);

            	loop7:
            		;	// Stops C# compiler whining that label 'loop7' has no statements


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
            	Memoize(input, 10, expression_StartIndex); 
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
    // GAMS.g:228:1: andExpression : notExpression ( AND notExpression )* ;
    public GAMSParser.andExpression_return andExpression() // throws RecognitionException [1]
    {   
        GAMSParser.andExpression_return retval = new GAMSParser.andExpression_return();
        retval.Start = input.LT(1);
        int andExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken AND42 = null;
        GAMSParser.notExpression_return notExpression41 = default(GAMSParser.notExpression_return);

        GAMSParser.notExpression_return notExpression43 = default(GAMSParser.notExpression_return);


        object AND42_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 11) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:228:14: ( notExpression ( AND notExpression )* )
            // GAMS.g:228:16: notExpression ( AND notExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_notExpression_in_andExpression951);
            	notExpression41 = notExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, notExpression41.Tree);
            	// GAMS.g:228:30: ( AND notExpression )*
            	do 
            	{
            	    int alt8 = 2;
            	    alt8 = dfa8.Predict(input);
            	    switch (alt8) 
            		{
            			case 1 :
            			    // GAMS.g:228:31: AND notExpression
            			    {
            			    	AND42=(IToken)Match(input,AND,FOLLOW_AND_in_andExpression954); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{AND42_tree = (object)adaptor.Create(AND42);
            			    		root_0 = (object)adaptor.BecomeRoot(AND42_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_notExpression_in_andExpression957);
            			    	notExpression43 = notExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, notExpression43.Tree);

            			    }
            			    break;

            			default:
            			    goto loop8;
            	    }
            	} while (true);

            	loop8:
            		;	// Stops C# compiler whining that label 'loop8' has no statements


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
            	Memoize(input, 11, andExpression_StartIndex); 
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
    // GAMS.g:230:1: notExpression : ( NOT logicalExpression -> ^( NOT logicalExpression ) | logicalExpression );
    public GAMSParser.notExpression_return notExpression() // throws RecognitionException [1]
    {   
        GAMSParser.notExpression_return retval = new GAMSParser.notExpression_return();
        retval.Start = input.LT(1);
        int notExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken NOT44 = null;
        GAMSParser.logicalExpression_return logicalExpression45 = default(GAMSParser.logicalExpression_return);

        GAMSParser.logicalExpression_return logicalExpression46 = default(GAMSParser.logicalExpression_return);


        object NOT44_tree=null;
        RewriteRuleTokenStream stream_NOT = new RewriteRuleTokenStream(adaptor,"token NOT");
        RewriteRuleSubtreeStream stream_logicalExpression = new RewriteRuleSubtreeStream(adaptor,"rule logicalExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 12) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:230:14: ( NOT logicalExpression -> ^( NOT logicalExpression ) | logicalExpression )
            int alt9 = 2;
            alt9 = dfa9.Predict(input);
            switch (alt9) 
            {
                case 1 :
                    // GAMS.g:230:16: NOT logicalExpression
                    {
                    	NOT44=(IToken)Match(input,NOT,FOLLOW_NOT_in_notExpression968); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_NOT.Add(NOT44);

                    	PushFollow(FOLLOW_logicalExpression_in_notExpression970);
                    	logicalExpression45 = logicalExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_logicalExpression.Add(logicalExpression45.Tree);


                    	// AST REWRITE
                    	// elements:          logicalExpression, NOT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 230:38: -> ^( NOT logicalExpression )
                    	{
                    	    // GAMS.g:230:41: ^( NOT logicalExpression )
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
                case 2 :
                    // GAMS.g:231:10: logicalExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_logicalExpression_in_notExpression990);
                    	logicalExpression46 = logicalExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, logicalExpression46.Tree);

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
            	Memoize(input, 12, notExpression_StartIndex); 
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
    // GAMS.g:233:1: logicalExpression : additiveExpression ( logical additiveExpression )* ;
    public GAMSParser.logicalExpression_return logicalExpression() // throws RecognitionException [1]
    {   
        GAMSParser.logicalExpression_return retval = new GAMSParser.logicalExpression_return();
        retval.Start = input.LT(1);
        int logicalExpression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.additiveExpression_return additiveExpression47 = default(GAMSParser.additiveExpression_return);

        GAMSParser.logical_return logical48 = default(GAMSParser.logical_return);

        GAMSParser.additiveExpression_return additiveExpression49 = default(GAMSParser.additiveExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 13) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:233:18: ( additiveExpression ( logical additiveExpression )* )
            // GAMS.g:233:21: additiveExpression ( logical additiveExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_additiveExpression_in_logicalExpression998);
            	additiveExpression47 = additiveExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression47.Tree);
            	// GAMS.g:233:40: ( logical additiveExpression )*
            	do 
            	{
            	    int alt10 = 2;
            	    alt10 = dfa10.Predict(input);
            	    switch (alt10) 
            		{
            			case 1 :
            			    // GAMS.g:233:41: logical additiveExpression
            			    {
            			    	PushFollow(FOLLOW_logical_in_logicalExpression1001);
            			    	logical48 = logical();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot(logical48.Tree, root_0);
            			    	PushFollow(FOLLOW_additiveExpression_in_logicalExpression1004);
            			    	additiveExpression49 = additiveExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression49.Tree);

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
            	Memoize(input, 13, logicalExpression_StartIndex); 
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
    // GAMS.g:235:1: additiveExpression : multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* ;
    public GAMSParser.additiveExpression_return additiveExpression() // throws RecognitionException [1]
    {   
        GAMSParser.additiveExpression_return retval = new GAMSParser.additiveExpression_return();
        retval.Start = input.LT(1);
        int additiveExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set51 = null;
        GAMSParser.multiplicativeExpression_return multiplicativeExpression50 = default(GAMSParser.multiplicativeExpression_return);

        GAMSParser.multiplicativeExpression_return multiplicativeExpression52 = default(GAMSParser.multiplicativeExpression_return);


        object set51_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 14) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:235:19: ( multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* )
            // GAMS.g:235:21: multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression1013);
            	multiplicativeExpression50 = multiplicativeExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression50.Tree);
            	// GAMS.g:235:46: ( ( PLUS | MINUS ) multiplicativeExpression )*
            	do 
            	{
            	    int alt11 = 2;
            	    alt11 = dfa11.Predict(input);
            	    switch (alt11) 
            		{
            			case 1 :
            			    // GAMS.g:235:48: ( PLUS | MINUS ) multiplicativeExpression
            			    {
            			    	set51=(IToken)input.LT(1);
            			    	set51 = (IToken)input.LT(1);
            			    	if ( input.LA(1) == PLUS || input.LA(1) == MINUS ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set51), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression1024);
            			    	multiplicativeExpression52 = multiplicativeExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression52.Tree);

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
            	Memoize(input, 14, additiveExpression_StartIndex); 
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
    // GAMS.g:237:1: multiplicativeExpression : powerExpression ( ( MULT | DIV ) powerExpression )* ;
    public GAMSParser.multiplicativeExpression_return multiplicativeExpression() // throws RecognitionException [1]
    {   
        GAMSParser.multiplicativeExpression_return retval = new GAMSParser.multiplicativeExpression_return();
        retval.Start = input.LT(1);
        int multiplicativeExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set54 = null;
        GAMSParser.powerExpression_return powerExpression53 = default(GAMSParser.powerExpression_return);

        GAMSParser.powerExpression_return powerExpression55 = default(GAMSParser.powerExpression_return);


        object set54_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 15) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:237:25: ( powerExpression ( ( MULT | DIV ) powerExpression )* )
            // GAMS.g:237:27: powerExpression ( ( MULT | DIV ) powerExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression1034);
            	powerExpression53 = powerExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression53.Tree);
            	// GAMS.g:237:43: ( ( MULT | DIV ) powerExpression )*
            	do 
            	{
            	    int alt12 = 2;
            	    alt12 = dfa12.Predict(input);
            	    switch (alt12) 
            		{
            			case 1 :
            			    // GAMS.g:237:45: ( MULT | DIV ) powerExpression
            			    {
            			    	set54=(IToken)input.LT(1);
            			    	set54 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= MULT && input.LA(1) <= DIV) ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set54), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression1045);
            			    	powerExpression55 = powerExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression55.Tree);

            			    }
            			    break;

            			default:
            			    goto loop12;
            	    }
            	} while (true);

            	loop12:
            		;	// Stops C# compiler whining that label 'loop12' has no statements


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
            	Memoize(input, 15, multiplicativeExpression_StartIndex); 
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
    // GAMS.g:239:1: powerExpression : unaryExpression ( STARS unaryExpression )* ;
    public GAMSParser.powerExpression_return powerExpression() // throws RecognitionException [1]
    {   
        GAMSParser.powerExpression_return retval = new GAMSParser.powerExpression_return();
        retval.Start = input.LT(1);
        int powerExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken STARS57 = null;
        GAMSParser.unaryExpression_return unaryExpression56 = default(GAMSParser.unaryExpression_return);

        GAMSParser.unaryExpression_return unaryExpression58 = default(GAMSParser.unaryExpression_return);


        object STARS57_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 16) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:239:16: ( unaryExpression ( STARS unaryExpression )* )
            // GAMS.g:239:18: unaryExpression ( STARS unaryExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_unaryExpression_in_powerExpression1055);
            	unaryExpression56 = unaryExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression56.Tree);
            	// GAMS.g:239:34: ( STARS unaryExpression )*
            	do 
            	{
            	    int alt13 = 2;
            	    alt13 = dfa13.Predict(input);
            	    switch (alt13) 
            		{
            			case 1 :
            			    // GAMS.g:239:36: STARS unaryExpression
            			    {
            			    	STARS57=(IToken)Match(input,STARS,FOLLOW_STARS_in_powerExpression1059); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{STARS57_tree = (object)adaptor.Create(STARS57);
            			    		root_0 = (object)adaptor.BecomeRoot(STARS57_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_unaryExpression_in_powerExpression1062);
            			    	unaryExpression58 = unaryExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression58.Tree);

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
            	Memoize(input, 16, powerExpression_StartIndex); 
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
    // GAMS.g:241:1: unaryExpression : ( MINUS dollarExpression -> ^( NEGATE dollarExpression ) | dollarExpression );
    public GAMSParser.unaryExpression_return unaryExpression() // throws RecognitionException [1]
    {   
        GAMSParser.unaryExpression_return retval = new GAMSParser.unaryExpression_return();
        retval.Start = input.LT(1);
        int unaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken MINUS59 = null;
        GAMSParser.dollarExpression_return dollarExpression60 = default(GAMSParser.dollarExpression_return);

        GAMSParser.dollarExpression_return dollarExpression61 = default(GAMSParser.dollarExpression_return);


        object MINUS59_tree=null;
        RewriteRuleTokenStream stream_MINUS = new RewriteRuleTokenStream(adaptor,"token MINUS");
        RewriteRuleSubtreeStream stream_dollarExpression = new RewriteRuleSubtreeStream(adaptor,"rule dollarExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 17) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:241:16: ( MINUS dollarExpression -> ^( NEGATE dollarExpression ) | dollarExpression )
            int alt14 = 2;
            alt14 = dfa14.Predict(input);
            switch (alt14) 
            {
                case 1 :
                    // GAMS.g:241:18: MINUS dollarExpression
                    {
                    	MINUS59=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_unaryExpression1073); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_MINUS.Add(MINUS59);

                    	PushFollow(FOLLOW_dollarExpression_in_unaryExpression1075);
                    	dollarExpression60 = dollarExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_dollarExpression.Add(dollarExpression60.Tree);


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
                    	// 241:41: -> ^( NEGATE dollarExpression )
                    	{
                    	    // GAMS.g:241:44: ^( NEGATE dollarExpression )
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
                case 2 :
                    // GAMS.g:242:11: dollarExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_dollarExpression_in_unaryExpression1096);
                    	dollarExpression61 = dollarExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, dollarExpression61.Tree);

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
            	Memoize(input, 17, unaryExpression_StartIndex); 
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
    // GAMS.g:244:1: dollarExpression : ( primaryExpression conditional -> ^( ASTDOLLAREXPRESSION primaryExpression conditional ) | primaryExpression );
    public GAMSParser.dollarExpression_return dollarExpression() // throws RecognitionException [1]
    {   
        GAMSParser.dollarExpression_return retval = new GAMSParser.dollarExpression_return();
        retval.Start = input.LT(1);
        int dollarExpression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.primaryExpression_return primaryExpression62 = default(GAMSParser.primaryExpression_return);

        GAMSParser.conditional_return conditional63 = default(GAMSParser.conditional_return);

        GAMSParser.primaryExpression_return primaryExpression64 = default(GAMSParser.primaryExpression_return);


        RewriteRuleSubtreeStream stream_conditional = new RewriteRuleSubtreeStream(adaptor,"rule conditional");
        RewriteRuleSubtreeStream stream_primaryExpression = new RewriteRuleSubtreeStream(adaptor,"rule primaryExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 18) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:244:17: ( primaryExpression conditional -> ^( ASTDOLLAREXPRESSION primaryExpression conditional ) | primaryExpression )
            int alt15 = 2;
            alt15 = dfa15.Predict(input);
            switch (alt15) 
            {
                case 1 :
                    // GAMS.g:245:9: primaryExpression conditional
                    {
                    	PushFollow(FOLLOW_primaryExpression_in_dollarExpression1115);
                    	primaryExpression62 = primaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_primaryExpression.Add(primaryExpression62.Tree);
                    	PushFollow(FOLLOW_conditional_in_dollarExpression1117);
                    	conditional63 = conditional();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_conditional.Add(conditional63.Tree);


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
                    	// 245:39: -> ^( ASTDOLLAREXPRESSION primaryExpression conditional )
                    	{
                    	    // GAMS.g:245:42: ^( ASTDOLLAREXPRESSION primaryExpression conditional )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTDOLLAREXPRESSION, "ASTDOLLAREXPRESSION"), root_1);

                    	    adaptor.AddChild(root_1, stream_primaryExpression.NextTree());
                    	    adaptor.AddChild(root_1, stream_conditional.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // GAMS.g:246:9: primaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_primaryExpression_in_dollarExpression1137);
                    	primaryExpression64 = primaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, primaryExpression64.Tree);

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
            	Memoize(input, 18, dollarExpression_StartIndex); 
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
    // GAMS.g:249:1: primaryExpression : ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value );
    public GAMSParser.primaryExpression_return primaryExpression() // throws RecognitionException [1]
    {   
        GAMSParser.primaryExpression_return retval = new GAMSParser.primaryExpression_return();
        retval.Start = input.LT(1);
        int primaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken L165 = null;
        IToken R167 = null;
        IToken L268 = null;
        IToken R270 = null;
        IToken L371 = null;
        IToken R373 = null;
        GAMSParser.expression_return expression66 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression69 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression72 = default(GAMSParser.expression_return);

        GAMSParser.value_return value74 = default(GAMSParser.value_return);


        object L165_tree=null;
        object R167_tree=null;
        object L268_tree=null;
        object R270_tree=null;
        object L371_tree=null;
        object R373_tree=null;
        RewriteRuleTokenStream stream_L1 = new RewriteRuleTokenStream(adaptor,"token L1");
        RewriteRuleTokenStream stream_L2 = new RewriteRuleTokenStream(adaptor,"token L2");
        RewriteRuleTokenStream stream_L3 = new RewriteRuleTokenStream(adaptor,"token L3");
        RewriteRuleTokenStream stream_R2 = new RewriteRuleTokenStream(adaptor,"token R2");
        RewriteRuleTokenStream stream_R3 = new RewriteRuleTokenStream(adaptor,"token R3");
        RewriteRuleTokenStream stream_R1 = new RewriteRuleTokenStream(adaptor,"token R1");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 19) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:249:18: ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value )
            int alt16 = 4;
            alt16 = dfa16.Predict(input);
            switch (alt16) 
            {
                case 1 :
                    // GAMS.g:250:5: L1 expression R1
                    {
                    	L165=(IToken)Match(input,L1,FOLLOW_L1_in_primaryExpression1158); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L165);

                    	PushFollow(FOLLOW_expression_in_primaryExpression1160);
                    	expression66 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression66.Tree);
                    	R167=(IToken)Match(input,R1,FOLLOW_R1_in_primaryExpression1162); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R167);



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
                    	// 250:22: -> ^( ASTEXPRESSION1 expression )
                    	{
                    	    // GAMS.g:250:25: ^( ASTEXPRESSION1 expression )
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
                    // GAMS.g:251:6: L2 expression R2
                    {
                    	L268=(IToken)Match(input,L2,FOLLOW_L2_in_primaryExpression1177); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L268);

                    	PushFollow(FOLLOW_expression_in_primaryExpression1179);
                    	expression69 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression69.Tree);
                    	R270=(IToken)Match(input,R2,FOLLOW_R2_in_primaryExpression1181); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R270);



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
                    	// 251:23: -> ^( ASTEXPRESSION2 expression )
                    	{
                    	    // GAMS.g:251:26: ^( ASTEXPRESSION2 expression )
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
                    // GAMS.g:252:8: L3 expression R3
                    {
                    	L371=(IToken)Match(input,L3,FOLLOW_L3_in_primaryExpression1198); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L371);

                    	PushFollow(FOLLOW_expression_in_primaryExpression1200);
                    	expression72 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression72.Tree);
                    	R373=(IToken)Match(input,R3,FOLLOW_R3_in_primaryExpression1202); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R373);



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
                    	// 252:25: -> ^( ASTEXPRESSION3 expression )
                    	{
                    	    // GAMS.g:252:28: ^( ASTEXPRESSION3 expression )
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
                    // GAMS.g:253:6: value
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_value_in_primaryExpression1217);
                    	value74 = value();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, value74.Tree);

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
            	Memoize(input, 19, primaryExpression_StartIndex); 
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
    // GAMS.g:255:1: logical : ( NONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL | EQUAL | LESSTHAN | GREATERTHAN );
    public GAMSParser.logical_return logical() // throws RecognitionException [1]
    {   
        GAMSParser.logical_return retval = new GAMSParser.logical_return();
        retval.Start = input.LT(1);
        int logical_StartIndex = input.Index();
        object root_0 = null;

        IToken set75 = null;

        object set75_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 20) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:255:8: ( NONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL | EQUAL | LESSTHAN | GREATERTHAN )
            // GAMS.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set75 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= NONEQUAL && input.LA(1) <= GREATERTHAN) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set75));
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
            	Memoize(input, 20, logical_StartIndex); 
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
    // GAMS.g:257:1: value : ( Integer -> ^( ASTVALUE Integer ) | Double -> ^( ASTVALUE Double ) | sum -> ^( ASTVALUE sum ) | function -> ^( ASTVALUE function ) | variableWithIndexerEtc -> ^( ASTVALUE variableWithIndexerEtc ) );
    public GAMSParser.value_return value() // throws RecognitionException [1]
    {   
        GAMSParser.value_return retval = new GAMSParser.value_return();
        retval.Start = input.LT(1);
        int value_StartIndex = input.Index();
        object root_0 = null;

        IToken Integer76 = null;
        IToken Double77 = null;
        GAMSParser.sum_return sum78 = default(GAMSParser.sum_return);

        GAMSParser.function_return function79 = default(GAMSParser.function_return);

        GAMSParser.variableWithIndexerEtc_return variableWithIndexerEtc80 = default(GAMSParser.variableWithIndexerEtc_return);


        object Integer76_tree=null;
        object Double77_tree=null;
        RewriteRuleTokenStream stream_Double = new RewriteRuleTokenStream(adaptor,"token Double");
        RewriteRuleTokenStream stream_Integer = new RewriteRuleTokenStream(adaptor,"token Integer");
        RewriteRuleSubtreeStream stream_variableWithIndexerEtc = new RewriteRuleSubtreeStream(adaptor,"rule variableWithIndexerEtc");
        RewriteRuleSubtreeStream stream_sum = new RewriteRuleSubtreeStream(adaptor,"rule sum");
        RewriteRuleSubtreeStream stream_function = new RewriteRuleSubtreeStream(adaptor,"rule function");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 21) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:258:2: ( Integer -> ^( ASTVALUE Integer ) | Double -> ^( ASTVALUE Double ) | sum -> ^( ASTVALUE sum ) | function -> ^( ASTVALUE function ) | variableWithIndexerEtc -> ^( ASTVALUE variableWithIndexerEtc ) )
            int alt17 = 5;
            alt17 = dfa17.Predict(input);
            switch (alt17) 
            {
                case 1 :
                    // GAMS.g:258:5: Integer
                    {
                    	Integer76=(IToken)Match(input,Integer,FOLLOW_Integer_in_value1258); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer76);



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
                    	// 258:31: -> ^( ASTVALUE Integer )
                    	{
                    	    // GAMS.g:258:34: ^( ASTVALUE Integer )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVALUE, "ASTVALUE"), root_1);

                    	    adaptor.AddChild(root_1, stream_Integer.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // GAMS.g:259:4: Double
                    {
                    	Double77=(IToken)Match(input,Double,FOLLOW_Double_in_value1289); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Double.Add(Double77);



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
                    	// 259:30: -> ^( ASTVALUE Double )
                    	{
                    	    // GAMS.g:259:33: ^( ASTVALUE Double )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVALUE, "ASTVALUE"), root_1);

                    	    adaptor.AddChild(root_1, stream_Double.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 3 :
                    // GAMS.g:260:6: sum
                    {
                    	PushFollow(FOLLOW_sum_in_value1323);
                    	sum78 = sum();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sum.Add(sum78.Tree);


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
                    	// 260:32: -> ^( ASTVALUE sum )
                    	{
                    	    // GAMS.g:260:35: ^( ASTVALUE sum )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVALUE, "ASTVALUE"), root_1);

                    	    adaptor.AddChild(root_1, stream_sum.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 4 :
                    // GAMS.g:261:6: function
                    {
                    	PushFollow(FOLLOW_function_in_value1360);
                    	function79 = function();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_function.Add(function79.Tree);


                    	// AST REWRITE
                    	// elements:          function
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 261:32: -> ^( ASTVALUE function )
                    	{
                    	    // GAMS.g:261:35: ^( ASTVALUE function )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVALUE, "ASTVALUE"), root_1);

                    	    adaptor.AddChild(root_1, stream_function.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 5 :
                    // GAMS.g:262:4: variableWithIndexerEtc
                    {
                    	PushFollow(FOLLOW_variableWithIndexerEtc_in_value1408);
                    	variableWithIndexerEtc80 = variableWithIndexerEtc();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variableWithIndexerEtc.Add(variableWithIndexerEtc80.Tree);


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
                    	// 262:30: -> ^( ASTVALUE variableWithIndexerEtc )
                    	{
                    	    // GAMS.g:262:33: ^( ASTVALUE variableWithIndexerEtc )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVALUE, "ASTVALUE"), root_1);

                    	    adaptor.AddChild(root_1, stream_variableWithIndexerEtc.NextTree());

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
            	Memoize(input, 21, value_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "value"

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
    // GAMS.g:269:1: function : ( functionName L1 functionElements R1 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L1 R1 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) | functionName L2 functionElements R2 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L2 R2 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) | functionName L3 functionElements R3 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L3 R3 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) );
    public GAMSParser.function_return function() // throws RecognitionException [1]
    {   
        GAMSParser.function_return retval = new GAMSParser.function_return();
        retval.Start = input.LT(1);
        int function_StartIndex = input.Index();
        object root_0 = null;

        IToken L182 = null;
        IToken R184 = null;
        IToken L286 = null;
        IToken R288 = null;
        IToken L390 = null;
        IToken R392 = null;
        GAMSParser.functionName_return functionName81 = default(GAMSParser.functionName_return);

        GAMSParser.functionElements_return functionElements83 = default(GAMSParser.functionElements_return);

        GAMSParser.functionName_return functionName85 = default(GAMSParser.functionName_return);

        GAMSParser.functionElements_return functionElements87 = default(GAMSParser.functionElements_return);

        GAMSParser.functionName_return functionName89 = default(GAMSParser.functionName_return);

        GAMSParser.functionElements_return functionElements91 = default(GAMSParser.functionElements_return);


        object L182_tree=null;
        object R184_tree=null;
        object L286_tree=null;
        object R288_tree=null;
        object L390_tree=null;
        object R392_tree=null;
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
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 22) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:269:9: ( functionName L1 functionElements R1 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L1 R1 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) | functionName L2 functionElements R2 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L2 R2 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) | functionName L3 functionElements R3 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L3 R3 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) )
            int alt18 = 3;
            int LA18_0 = input.LA(1);

            if ( ((LA18_0 >= ABS && LA18_0 <= SAMEAS)) )
            {
                switch ( input.LA(2) ) 
                {
                case L1:
                	{
                    alt18 = 1;
                    }
                    break;
                case L3:
                	{
                    alt18 = 3;
                    }
                    break;
                case L2:
                	{
                    alt18 = 2;
                    }
                    break;
                	default:
                	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                	    NoViableAltException nvae_d18s1 =
                	        new NoViableAltException("", 18, 1, input);

                	    throw nvae_d18s1;
                }

            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d18s0 =
                    new NoViableAltException("", 18, 0, input);

                throw nvae_d18s0;
            }
            switch (alt18) 
            {
                case 1 :
                    // GAMS.g:269:15: functionName L1 functionElements R1
                    {
                    	PushFollow(FOLLOW_functionName_in_function1440);
                    	functionName81 = functionName();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionName.Add(functionName81.Tree);
                    	L182=(IToken)Match(input,L1,FOLLOW_L1_in_function1442); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L182);

                    	PushFollow(FOLLOW_functionElements_in_function1444);
                    	functionElements83 = functionElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionElements.Add(functionElements83.Tree);
                    	R184=(IToken)Match(input,R1,FOLLOW_R1_in_function1446); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R184);



                    	// AST REWRITE
                    	// elements:          functionElements, R1, L1, functionName
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 269:51: -> ^( ASTFUNCTION ^( ASTFUNCTION0 L1 R1 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	{
                    	    // GAMS.g:269:54: ^( ASTFUNCTION ^( ASTFUNCTION0 L1 R1 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION, "ASTFUNCTION"), root_1);

                    	    // GAMS.g:269:68: ^( ASTFUNCTION0 L1 R1 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION0, "ASTFUNCTION0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L1.NextNode());
                    	    adaptor.AddChild(root_2, stream_R1.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:269:90: ^( ASTFUNCTION1 functionName )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION1, "ASTFUNCTION1"), root_2);

                    	    adaptor.AddChild(root_2, stream_functionName.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:269:119: ^( ASTFUNCTION2 functionElements )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION2, "ASTFUNCTION2"), root_2);

                    	    adaptor.AddChild(root_2, stream_functionElements.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // GAMS.g:270:15: functionName L2 functionElements R2
                    {
                    	PushFollow(FOLLOW_functionName_in_function1488);
                    	functionName85 = functionName();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionName.Add(functionName85.Tree);
                    	L286=(IToken)Match(input,L2,FOLLOW_L2_in_function1490); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L286);

                    	PushFollow(FOLLOW_functionElements_in_function1492);
                    	functionElements87 = functionElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionElements.Add(functionElements87.Tree);
                    	R288=(IToken)Match(input,R2,FOLLOW_R2_in_function1494); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R288);



                    	// AST REWRITE
                    	// elements:          R2, functionElements, L2, functionName
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 270:51: -> ^( ASTFUNCTION ^( ASTFUNCTION0 L2 R2 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	{
                    	    // GAMS.g:270:54: ^( ASTFUNCTION ^( ASTFUNCTION0 L2 R2 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION, "ASTFUNCTION"), root_1);

                    	    // GAMS.g:270:68: ^( ASTFUNCTION0 L2 R2 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION0, "ASTFUNCTION0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L2.NextNode());
                    	    adaptor.AddChild(root_2, stream_R2.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:270:90: ^( ASTFUNCTION1 functionName )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION1, "ASTFUNCTION1"), root_2);

                    	    adaptor.AddChild(root_2, stream_functionName.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:270:119: ^( ASTFUNCTION2 functionElements )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION2, "ASTFUNCTION2"), root_2);

                    	    adaptor.AddChild(root_2, stream_functionElements.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 3 :
                    // GAMS.g:271:15: functionName L3 functionElements R3
                    {
                    	PushFollow(FOLLOW_functionName_in_function1536);
                    	functionName89 = functionName();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionName.Add(functionName89.Tree);
                    	L390=(IToken)Match(input,L3,FOLLOW_L3_in_function1538); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L390);

                    	PushFollow(FOLLOW_functionElements_in_function1540);
                    	functionElements91 = functionElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionElements.Add(functionElements91.Tree);
                    	R392=(IToken)Match(input,R3,FOLLOW_R3_in_function1542); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R392);



                    	// AST REWRITE
                    	// elements:          functionName, R3, functionElements, L3
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 271:51: -> ^( ASTFUNCTION ^( ASTFUNCTION0 L3 R3 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	{
                    	    // GAMS.g:271:54: ^( ASTFUNCTION ^( ASTFUNCTION0 L3 R3 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION, "ASTFUNCTION"), root_1);

                    	    // GAMS.g:271:68: ^( ASTFUNCTION0 L3 R3 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION0, "ASTFUNCTION0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L3.NextNode());
                    	    adaptor.AddChild(root_2, stream_R3.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:271:90: ^( ASTFUNCTION1 functionName )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION1, "ASTFUNCTION1"), root_2);

                    	    adaptor.AddChild(root_2, stream_functionName.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:271:119: ^( ASTFUNCTION2 functionElements )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION2, "ASTFUNCTION2"), root_2);

                    	    adaptor.AddChild(root_2, stream_functionElements.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }

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
            	Memoize(input, 22, function_StartIndex); 
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
    // GAMS.g:274:1: functionName : ( ABS | EXP | LOG | MAX | MIN | POWER | SQR | SAMEAS | TANH );
    public GAMSParser.functionName_return functionName() // throws RecognitionException [1]
    {   
        GAMSParser.functionName_return retval = new GAMSParser.functionName_return();
        retval.Start = input.LT(1);
        int functionName_StartIndex = input.Index();
        object root_0 = null;

        IToken set93 = null;

        object set93_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 23) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:274:13: ( ABS | EXP | LOG | MAX | MIN | POWER | SQR | SAMEAS | TANH )
            // GAMS.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set93 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= ABS && input.LA(1) <= SAMEAS) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set93));
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
            	Memoize(input, 23, functionName_StartIndex); 
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
    // GAMS.g:276:1: functionElements : expression ( COMMA expression )* -> ^( ASTFUNCTIONELEMENTS ^( ASTFUNCTIONELEMENTS0 ( COMMA )* ) ^( ASTFUNCTIONELEMENTS1 ( expression )+ ) ) ;
    public GAMSParser.functionElements_return functionElements() // throws RecognitionException [1]
    {   
        GAMSParser.functionElements_return retval = new GAMSParser.functionElements_return();
        retval.Start = input.LT(1);
        int functionElements_StartIndex = input.Index();
        object root_0 = null;

        IToken COMMA95 = null;
        GAMSParser.expression_return expression94 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression96 = default(GAMSParser.expression_return);


        object COMMA95_tree=null;
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 24) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:276:17: ( expression ( COMMA expression )* -> ^( ASTFUNCTIONELEMENTS ^( ASTFUNCTIONELEMENTS0 ( COMMA )* ) ^( ASTFUNCTIONELEMENTS1 ( expression )+ ) ) )
            // GAMS.g:276:19: expression ( COMMA expression )*
            {
            	PushFollow(FOLLOW_expression_in_functionElements1617);
            	expression94 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression94.Tree);
            	// GAMS.g:276:30: ( COMMA expression )*
            	do 
            	{
            	    int alt19 = 2;
            	    int LA19_0 = input.LA(1);

            	    if ( (LA19_0 == COMMA) )
            	    {
            	        alt19 = 1;
            	    }


            	    switch (alt19) 
            		{
            			case 1 :
            			    // GAMS.g:276:31: COMMA expression
            			    {
            			    	COMMA95=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_functionElements1620); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA95);

            			    	PushFollow(FOLLOW_expression_in_functionElements1622);
            			    	expression96 = expression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_expression.Add(expression96.Tree);

            			    }
            			    break;

            			default:
            			    goto loop19;
            	    }
            	} while (true);

            	loop19:
            		;	// Stops C# compiler whining that label 'loop19' has no statements



            	// AST REWRITE
            	// elements:          COMMA, expression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 277:3: -> ^( ASTFUNCTIONELEMENTS ^( ASTFUNCTIONELEMENTS0 ( COMMA )* ) ^( ASTFUNCTIONELEMENTS1 ( expression )+ ) )
            	{
            	    // GAMS.g:277:6: ^( ASTFUNCTIONELEMENTS ^( ASTFUNCTIONELEMENTS0 ( COMMA )* ) ^( ASTFUNCTIONELEMENTS1 ( expression )+ ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTIONELEMENTS, "ASTFUNCTIONELEMENTS"), root_1);

            	    // GAMS.g:277:28: ^( ASTFUNCTIONELEMENTS0 ( COMMA )* )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTIONELEMENTS0, "ASTFUNCTIONELEMENTS0"), root_2);

            	    // GAMS.g:277:51: ( COMMA )*
            	    while ( stream_COMMA.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_COMMA.NextNode());

            	    }
            	    stream_COMMA.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:277:59: ^( ASTFUNCTIONELEMENTS1 ( expression )+ )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTIONELEMENTS1, "ASTFUNCTIONELEMENTS1"), root_2);

            	    if ( !(stream_expression.HasNext()) ) {
            	        throw new RewriteEarlyExitException();
            	    }
            	    while ( stream_expression.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_expression.NextTree());

            	    }
            	    stream_expression.Reset();

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
            	Memoize(input, 24, functionElements_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "functionElements"

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
    // GAMS.g:279:1: sum : ( SUM L1 sumControlled ( conditional )? COMMA expression R1 -> ^( ASTSUM ^( ASTSUM0 L1 COMMA R1 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) | SUM L2 sumControlled ( conditional )? COMMA expression R2 -> ^( ASTSUM ^( ASTSUM0 L2 COMMA R2 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) | SUM L3 sumControlled ( conditional )? COMMA expression R3 -> ^( ASTSUM ^( ASTSUM0 L3 COMMA R3 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) );
    public GAMSParser.sum_return sum() // throws RecognitionException [1]
    {   
        GAMSParser.sum_return retval = new GAMSParser.sum_return();
        retval.Start = input.LT(1);
        int sum_StartIndex = input.Index();
        object root_0 = null;

        IToken SUM97 = null;
        IToken L198 = null;
        IToken COMMA101 = null;
        IToken R1103 = null;
        IToken SUM104 = null;
        IToken L2105 = null;
        IToken COMMA108 = null;
        IToken R2110 = null;
        IToken SUM111 = null;
        IToken L3112 = null;
        IToken COMMA115 = null;
        IToken R3117 = null;
        GAMSParser.sumControlled_return sumControlled99 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional100 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression102 = default(GAMSParser.expression_return);

        GAMSParser.sumControlled_return sumControlled106 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional107 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression109 = default(GAMSParser.expression_return);

        GAMSParser.sumControlled_return sumControlled113 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional114 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression116 = default(GAMSParser.expression_return);


        object SUM97_tree=null;
        object L198_tree=null;
        object COMMA101_tree=null;
        object R1103_tree=null;
        object SUM104_tree=null;
        object L2105_tree=null;
        object COMMA108_tree=null;
        object R2110_tree=null;
        object SUM111_tree=null;
        object L3112_tree=null;
        object COMMA115_tree=null;
        object R3117_tree=null;
        RewriteRuleTokenStream stream_L1 = new RewriteRuleTokenStream(adaptor,"token L1");
        RewriteRuleTokenStream stream_L2 = new RewriteRuleTokenStream(adaptor,"token L2");
        RewriteRuleTokenStream stream_L3 = new RewriteRuleTokenStream(adaptor,"token L3");
        RewriteRuleTokenStream stream_R2 = new RewriteRuleTokenStream(adaptor,"token R2");
        RewriteRuleTokenStream stream_R3 = new RewriteRuleTokenStream(adaptor,"token R3");
        RewriteRuleTokenStream stream_SUM = new RewriteRuleTokenStream(adaptor,"token SUM");
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleTokenStream stream_R1 = new RewriteRuleTokenStream(adaptor,"token R1");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_sumControlled = new RewriteRuleSubtreeStream(adaptor,"rule sumControlled");
        RewriteRuleSubtreeStream stream_conditional = new RewriteRuleSubtreeStream(adaptor,"rule conditional");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 25) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:279:4: ( SUM L1 sumControlled ( conditional )? COMMA expression R1 -> ^( ASTSUM ^( ASTSUM0 L1 COMMA R1 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) | SUM L2 sumControlled ( conditional )? COMMA expression R2 -> ^( ASTSUM ^( ASTSUM0 L2 COMMA R2 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) | SUM L3 sumControlled ( conditional )? COMMA expression R3 -> ^( ASTSUM ^( ASTSUM0 L3 COMMA R3 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) )
            int alt23 = 3;
            int LA23_0 = input.LA(1);

            if ( (LA23_0 == SUM) )
            {
                switch ( input.LA(2) ) 
                {
                case L1:
                	{
                    alt23 = 1;
                    }
                    break;
                case L2:
                	{
                    alt23 = 2;
                    }
                    break;
                case L3:
                	{
                    alt23 = 3;
                    }
                    break;
                	default:
                	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                	    NoViableAltException nvae_d23s1 =
                	        new NoViableAltException("", 23, 1, input);

                	    throw nvae_d23s1;
                }

            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d23s0 =
                    new NoViableAltException("", 23, 0, input);

                throw nvae_d23s0;
            }
            switch (alt23) 
            {
                case 1 :
                    // GAMS.g:279:7: SUM L1 sumControlled ( conditional )? COMMA expression R1
                    {
                    	SUM97=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum1655); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_SUM.Add(SUM97);

                    	L198=(IToken)Match(input,L1,FOLLOW_L1_in_sum1657); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L198);

                    	PushFollow(FOLLOW_sumControlled_in_sum1659);
                    	sumControlled99 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sumControlled.Add(sumControlled99.Tree);
                    	// GAMS.g:279:28: ( conditional )?
                    	int alt20 = 2;
                    	int LA20_0 = input.LA(1);

                    	if ( (LA20_0 == DOLLAR) )
                    	{
                    	    alt20 = 1;
                    	}
                    	switch (alt20) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:0:0: conditional
                    	        {
                    	        	PushFollow(FOLLOW_conditional_in_sum1661);
                    	        	conditional100 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional100.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA101=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum1664); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA101);

                    	PushFollow(FOLLOW_expression_in_sum1666);
                    	expression102 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression102.Tree);
                    	R1103=(IToken)Match(input,R1,FOLLOW_R1_in_sum1668); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R1103);



                    	// AST REWRITE
                    	// elements:          L1, expression, COMMA, conditional, R1, sumControlled
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 279:61: -> ^( ASTSUM ^( ASTSUM0 L1 COMMA R1 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	{
                    	    // GAMS.g:279:64: ^( ASTSUM ^( ASTSUM0 L1 COMMA R1 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM, "ASTSUM"), root_1);

                    	    // GAMS.g:279:73: ^( ASTSUM0 L1 COMMA R1 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM0, "ASTSUM0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L1.NextNode());
                    	    adaptor.AddChild(root_2, stream_COMMA.NextNode());
                    	    adaptor.AddChild(root_2, stream_R1.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:279:96: ^( ASTSUM1 sumControlled )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM1, "ASTSUM1"), root_2);

                    	    adaptor.AddChild(root_2, stream_sumControlled.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:279:121: ^( ASTSUM2 ( conditional )? )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM2, "ASTSUM2"), root_2);

                    	    // GAMS.g:279:131: ( conditional )?
                    	    if ( stream_conditional.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_2, stream_conditional.NextTree());

                    	    }
                    	    stream_conditional.Reset();

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:279:146: ^( ASTSUM3 expression )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM3, "ASTSUM3"), root_2);

                    	    adaptor.AddChild(root_2, stream_expression.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // GAMS.g:280:7: SUM L2 sumControlled ( conditional )? COMMA expression R2
                    {
                    	SUM104=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum1712); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_SUM.Add(SUM104);

                    	L2105=(IToken)Match(input,L2,FOLLOW_L2_in_sum1714); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L2105);

                    	PushFollow(FOLLOW_sumControlled_in_sum1716);
                    	sumControlled106 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sumControlled.Add(sumControlled106.Tree);
                    	// GAMS.g:280:28: ( conditional )?
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
                    	        	PushFollow(FOLLOW_conditional_in_sum1718);
                    	        	conditional107 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional107.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA108=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum1721); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA108);

                    	PushFollow(FOLLOW_expression_in_sum1723);
                    	expression109 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression109.Tree);
                    	R2110=(IToken)Match(input,R2,FOLLOW_R2_in_sum1725); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R2110);



                    	// AST REWRITE
                    	// elements:          L2, R2, conditional, expression, sumControlled, COMMA
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 280:61: -> ^( ASTSUM ^( ASTSUM0 L2 COMMA R2 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	{
                    	    // GAMS.g:280:64: ^( ASTSUM ^( ASTSUM0 L2 COMMA R2 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM, "ASTSUM"), root_1);

                    	    // GAMS.g:280:73: ^( ASTSUM0 L2 COMMA R2 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM0, "ASTSUM0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L2.NextNode());
                    	    adaptor.AddChild(root_2, stream_COMMA.NextNode());
                    	    adaptor.AddChild(root_2, stream_R2.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:280:96: ^( ASTSUM1 sumControlled )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM1, "ASTSUM1"), root_2);

                    	    adaptor.AddChild(root_2, stream_sumControlled.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:280:121: ^( ASTSUM2 ( conditional )? )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM2, "ASTSUM2"), root_2);

                    	    // GAMS.g:280:131: ( conditional )?
                    	    if ( stream_conditional.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_2, stream_conditional.NextTree());

                    	    }
                    	    stream_conditional.Reset();

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:280:146: ^( ASTSUM3 expression )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM3, "ASTSUM3"), root_2);

                    	    adaptor.AddChild(root_2, stream_expression.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 3 :
                    // GAMS.g:281:7: SUM L3 sumControlled ( conditional )? COMMA expression R3
                    {
                    	SUM111=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum1769); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_SUM.Add(SUM111);

                    	L3112=(IToken)Match(input,L3,FOLLOW_L3_in_sum1771); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L3112);

                    	PushFollow(FOLLOW_sumControlled_in_sum1773);
                    	sumControlled113 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sumControlled.Add(sumControlled113.Tree);
                    	// GAMS.g:281:28: ( conditional )?
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
                    	        	PushFollow(FOLLOW_conditional_in_sum1775);
                    	        	conditional114 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional114.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA115=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum1778); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA115);

                    	PushFollow(FOLLOW_expression_in_sum1780);
                    	expression116 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression116.Tree);
                    	R3117=(IToken)Match(input,R3,FOLLOW_R3_in_sum1782); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R3117);



                    	// AST REWRITE
                    	// elements:          L3, expression, sumControlled, conditional, COMMA, R3
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 281:61: -> ^( ASTSUM ^( ASTSUM0 L3 COMMA R3 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	{
                    	    // GAMS.g:281:64: ^( ASTSUM ^( ASTSUM0 L3 COMMA R3 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM, "ASTSUM"), root_1);

                    	    // GAMS.g:281:73: ^( ASTSUM0 L3 COMMA R3 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM0, "ASTSUM0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L3.NextNode());
                    	    adaptor.AddChild(root_2, stream_COMMA.NextNode());
                    	    adaptor.AddChild(root_2, stream_R3.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:281:96: ^( ASTSUM1 sumControlled )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM1, "ASTSUM1"), root_2);

                    	    adaptor.AddChild(root_2, stream_sumControlled.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:281:121: ^( ASTSUM2 ( conditional )? )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM2, "ASTSUM2"), root_2);

                    	    // GAMS.g:281:131: ( conditional )?
                    	    if ( stream_conditional.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_2, stream_conditional.NextTree());

                    	    }
                    	    stream_conditional.Reset();

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:281:146: ^( ASTSUM3 expression )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM3, "ASTSUM3"), root_2);

                    	    adaptor.AddChild(root_2, stream_expression.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }

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
            	Memoize(input, 25, sum_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "sum"

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
    // GAMS.g:284:1: sumControlled : ( variable -> ^( ASTSUMCONTROLLEDSIMPLE variable ) | L1 indexerElements R1 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L1 R1 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) | L2 indexerElements R2 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L2 R2 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) | L3 indexerElements R3 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L3 R3 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) );
    public GAMSParser.sumControlled_return sumControlled() // throws RecognitionException [1]
    {   
        GAMSParser.sumControlled_return retval = new GAMSParser.sumControlled_return();
        retval.Start = input.LT(1);
        int sumControlled_StartIndex = input.Index();
        object root_0 = null;

        IToken L1119 = null;
        IToken R1121 = null;
        IToken L2122 = null;
        IToken R2124 = null;
        IToken L3125 = null;
        IToken R3127 = null;
        GAMSParser.variable_return variable118 = default(GAMSParser.variable_return);

        GAMSParser.indexerElements_return indexerElements120 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements123 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements126 = default(GAMSParser.indexerElements_return);


        object L1119_tree=null;
        object R1121_tree=null;
        object L2122_tree=null;
        object R2124_tree=null;
        object L3125_tree=null;
        object R3127_tree=null;
        RewriteRuleTokenStream stream_L1 = new RewriteRuleTokenStream(adaptor,"token L1");
        RewriteRuleTokenStream stream_L2 = new RewriteRuleTokenStream(adaptor,"token L2");
        RewriteRuleTokenStream stream_L3 = new RewriteRuleTokenStream(adaptor,"token L3");
        RewriteRuleTokenStream stream_R2 = new RewriteRuleTokenStream(adaptor,"token R2");
        RewriteRuleTokenStream stream_R3 = new RewriteRuleTokenStream(adaptor,"token R3");
        RewriteRuleTokenStream stream_R1 = new RewriteRuleTokenStream(adaptor,"token R1");
        RewriteRuleSubtreeStream stream_indexerElements = new RewriteRuleSubtreeStream(adaptor,"rule indexerElements");
        RewriteRuleSubtreeStream stream_variable = new RewriteRuleSubtreeStream(adaptor,"rule variable");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 26) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:284:14: ( variable -> ^( ASTSUMCONTROLLEDSIMPLE variable ) | L1 indexerElements R1 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L1 R1 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) | L2 indexerElements R2 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L2 R2 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) | L3 indexerElements R3 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L3 R3 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) )
            int alt24 = 4;
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
                alt24 = 1;
                }
                break;
            case L1:
            	{
                alt24 = 2;
                }
                break;
            case L2:
            	{
                alt24 = 3;
                }
                break;
            case L3:
            	{
                alt24 = 4;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d24s0 =
            	        new NoViableAltException("", 24, 0, input);

            	    throw nvae_d24s0;
            }

            switch (alt24) 
            {
                case 1 :
                    // GAMS.g:285:11: variable
                    {
                    	PushFollow(FOLLOW_variable_in_sumControlled1837);
                    	variable118 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variable.Add(variable118.Tree);


                    	// AST REWRITE
                    	// elements:          variable
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 285:33: -> ^( ASTSUMCONTROLLEDSIMPLE variable )
                    	{
                    	    // GAMS.g:285:36: ^( ASTSUMCONTROLLEDSIMPLE variable )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLEDSIMPLE, "ASTSUMCONTROLLEDSIMPLE"), root_1);

                    	    adaptor.AddChild(root_1, stream_variable.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // GAMS.g:286:5: L1 indexerElements R1
                    {
                    	L1119=(IToken)Match(input,L1,FOLLOW_L1_in_sumControlled1864); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L1119);

                    	PushFollow(FOLLOW_indexerElements_in_sumControlled1866);
                    	indexerElements120 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements120.Tree);
                    	R1121=(IToken)Match(input,R1,FOLLOW_R1_in_sumControlled1868); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R1121);



                    	// AST REWRITE
                    	// elements:          L1, R1, indexerElements
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 286:27: -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L1 R1 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	{
                    	    // GAMS.g:286:30: ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L1 R1 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED, "ASTSUMCONTROLLED"), root_1);

                    	    // GAMS.g:286:49: ^( ASTSUMCONTROLLED0 L1 R1 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED0, "ASTSUMCONTROLLED0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L1.NextNode());
                    	    adaptor.AddChild(root_2, stream_R1.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:286:76: ^( ASTSUMCONTROLLED2 indexerElements )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED2, "ASTSUMCONTROLLED2"), root_2);

                    	    adaptor.AddChild(root_2, stream_indexerElements.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 3 :
                    // GAMS.g:287:5: L2 indexerElements R2
                    {
                    	L2122=(IToken)Match(input,L2,FOLLOW_L2_in_sumControlled1894); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L2122);

                    	PushFollow(FOLLOW_indexerElements_in_sumControlled1896);
                    	indexerElements123 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements123.Tree);
                    	R2124=(IToken)Match(input,R2,FOLLOW_R2_in_sumControlled1898); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R2124);



                    	// AST REWRITE
                    	// elements:          L2, indexerElements, R2
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 287:27: -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L2 R2 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	{
                    	    // GAMS.g:287:30: ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L2 R2 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED, "ASTSUMCONTROLLED"), root_1);

                    	    // GAMS.g:287:49: ^( ASTSUMCONTROLLED0 L2 R2 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED0, "ASTSUMCONTROLLED0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L2.NextNode());
                    	    adaptor.AddChild(root_2, stream_R2.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:287:76: ^( ASTSUMCONTROLLED2 indexerElements )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED2, "ASTSUMCONTROLLED2"), root_2);

                    	    adaptor.AddChild(root_2, stream_indexerElements.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 4 :
                    // GAMS.g:288:5: L3 indexerElements R3
                    {
                    	L3125=(IToken)Match(input,L3,FOLLOW_L3_in_sumControlled1924); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L3125);

                    	PushFollow(FOLLOW_indexerElements_in_sumControlled1926);
                    	indexerElements126 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements126.Tree);
                    	R3127=(IToken)Match(input,R3,FOLLOW_R3_in_sumControlled1928); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R3127);



                    	// AST REWRITE
                    	// elements:          L3, R3, indexerElements
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 288:27: -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L3 R3 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	{
                    	    // GAMS.g:288:30: ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L3 R3 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED, "ASTSUMCONTROLLED"), root_1);

                    	    // GAMS.g:288:49: ^( ASTSUMCONTROLLED0 L3 R3 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED0, "ASTSUMCONTROLLED0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L3.NextNode());
                    	    adaptor.AddChild(root_2, stream_R3.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:288:76: ^( ASTSUMCONTROLLED2 indexerElements )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED2, "ASTSUMCONTROLLED2"), root_2);

                    	    adaptor.AddChild(root_2, stream_indexerElements.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }

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
            	Memoize(input, 26, sumControlled_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "sumControlled"

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
    // GAMS.g:295:1: ident : ( Ident | extraTokens );
    public GAMSParser.ident_return ident() // throws RecognitionException [1]
    {   
        GAMSParser.ident_return retval = new GAMSParser.ident_return();
        retval.Start = input.LT(1);
        int ident_StartIndex = input.Index();
        object root_0 = null;

        IToken Ident128 = null;
        GAMSParser.extraTokens_return extraTokens129 = default(GAMSParser.extraTokens_return);


        object Ident128_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 27) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:295:9: ( Ident | extraTokens )
            int alt25 = 2;
            int LA25_0 = input.LA(1);

            if ( (LA25_0 == Ident) )
            {
                alt25 = 1;
            }
            else if ( ((LA25_0 >= SUM && LA25_0 <= SAMEAS)) )
            {
                alt25 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d25s0 =
                    new NoViableAltException("", 25, 0, input);

                throw nvae_d25s0;
            }
            switch (alt25) 
            {
                case 1 :
                    // GAMS.g:295:12: Ident
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Ident128=(IToken)Match(input,Ident,FOLLOW_Ident_in_ident1964); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Ident128_tree = (object)adaptor.Create(Ident128);
                    		adaptor.AddChild(root_0, Ident128_tree);
                    	}

                    }
                    break;
                case 2 :
                    // GAMS.g:295:20: extraTokens
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_extraTokens_in_ident1968);
                    	extraTokens129 = extraTokens();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, extraTokens129.Tree);

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
            	Memoize(input, 27, ident_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "ident"

    // $ANTLR start "synpred15_GAMS"
    public void synpred15_GAMS_fragment() {
        // GAMS.g:189:55: ( conditional )
        // GAMS.g:189:55: conditional
        {
        	PushFollow(FOLLOW_conditional_in_synpred15_GAMS541);
        	conditional();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred15_GAMS"

    // $ANTLR start "synpred22_GAMS"
    public void synpred22_GAMS_fragment() {
        // GAMS.g:226:28: ( OR andExpression )
        // GAMS.g:226:28: OR andExpression
        {
        	Match(input,OR,FOLLOW_OR_in_synpred22_GAMS939); if (state.failed) return ;
        	PushFollow(FOLLOW_andExpression_in_synpred22_GAMS942);
        	andExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred22_GAMS"

    // $ANTLR start "synpred23_GAMS"
    public void synpred23_GAMS_fragment() {
        // GAMS.g:228:31: ( AND notExpression )
        // GAMS.g:228:31: AND notExpression
        {
        	Match(input,AND,FOLLOW_AND_in_synpred23_GAMS954); if (state.failed) return ;
        	PushFollow(FOLLOW_notExpression_in_synpred23_GAMS957);
        	notExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred23_GAMS"

    // $ANTLR start "synpred24_GAMS"
    public void synpred24_GAMS_fragment() {
        // GAMS.g:230:16: ( NOT logicalExpression )
        // GAMS.g:230:16: NOT logicalExpression
        {
        	Match(input,NOT,FOLLOW_NOT_in_synpred24_GAMS968); if (state.failed) return ;
        	PushFollow(FOLLOW_logicalExpression_in_synpred24_GAMS970);
        	logicalExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred24_GAMS"

    // $ANTLR start "synpred25_GAMS"
    public void synpred25_GAMS_fragment() {
        // GAMS.g:233:41: ( logical additiveExpression )
        // GAMS.g:233:41: logical additiveExpression
        {
        	PushFollow(FOLLOW_logical_in_synpred25_GAMS1001);
        	logical();
        	state.followingStackPointer--;
        	if (state.failed) return ;
        	PushFollow(FOLLOW_additiveExpression_in_synpred25_GAMS1004);
        	additiveExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred25_GAMS"

    // $ANTLR start "synpred27_GAMS"
    public void synpred27_GAMS_fragment() {
        // GAMS.g:235:48: ( ( PLUS | MINUS ) multiplicativeExpression )
        // GAMS.g:235:48: ( PLUS | MINUS ) multiplicativeExpression
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

        	PushFollow(FOLLOW_multiplicativeExpression_in_synpred27_GAMS1024);
        	multiplicativeExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred27_GAMS"

    // $ANTLR start "synpred29_GAMS"
    public void synpred29_GAMS_fragment() {
        // GAMS.g:237:45: ( ( MULT | DIV ) powerExpression )
        // GAMS.g:237:45: ( MULT | DIV ) powerExpression
        {
        	if ( (input.LA(1) >= MULT && input.LA(1) <= DIV) ) 
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

        	PushFollow(FOLLOW_powerExpression_in_synpred29_GAMS1045);
        	powerExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred29_GAMS"

    // $ANTLR start "synpred30_GAMS"
    public void synpred30_GAMS_fragment() {
        // GAMS.g:239:36: ( STARS unaryExpression )
        // GAMS.g:239:36: STARS unaryExpression
        {
        	Match(input,STARS,FOLLOW_STARS_in_synpred30_GAMS1059); if (state.failed) return ;
        	PushFollow(FOLLOW_unaryExpression_in_synpred30_GAMS1062);
        	unaryExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred30_GAMS"

    // $ANTLR start "synpred32_GAMS"
    public void synpred32_GAMS_fragment() {
        // GAMS.g:245:9: ( primaryExpression conditional )
        // GAMS.g:245:9: primaryExpression conditional
        {
        	PushFollow(FOLLOW_primaryExpression_in_synpred32_GAMS1115);
        	primaryExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;
        	PushFollow(FOLLOW_conditional_in_synpred32_GAMS1117);
        	conditional();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred32_GAMS"

    // $ANTLR start "synpred43_GAMS"
    public void synpred43_GAMS_fragment() {
        // GAMS.g:260:6: ( sum )
        // GAMS.g:260:6: sum
        {
        	PushFollow(FOLLOW_sum_in_synpred43_GAMS1323);
        	sum();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred43_GAMS"

    // $ANTLR start "synpred44_GAMS"
    public void synpred44_GAMS_fragment() {
        // GAMS.g:261:6: ( function )
        // GAMS.g:261:6: function
        {
        	PushFollow(FOLLOW_function_in_synpred44_GAMS1360);
        	function();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred44_GAMS"

    // Delegated rules

   	public bool synpred22_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred22_GAMS_fragment(); // can never throw exception
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
   	public bool synpred29_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred29_GAMS_fragment(); // can never throw exception
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
   	public bool synpred23_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred23_GAMS_fragment(); // can never throw exception
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
   	public bool synpred24_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred24_GAMS_fragment(); // can never throw exception
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
   	public bool synpred30_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred30_GAMS_fragment(); // can never throw exception
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
   	public bool synpred25_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred25_GAMS_fragment(); // can never throw exception
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
   	public bool synpred32_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred32_GAMS_fragment(); // can never throw exception
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
   	public bool synpred15_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred15_GAMS_fragment(); // can never throw exception
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


   	protected DFA1 dfa1;
   	protected DFA2 dfa2;
   	protected DFA3 dfa3;
   	protected DFA6 dfa6;
   	protected DFA7 dfa7;
   	protected DFA8 dfa8;
   	protected DFA9 dfa9;
   	protected DFA10 dfa10;
   	protected DFA11 dfa11;
   	protected DFA12 dfa12;
   	protected DFA13 dfa13;
   	protected DFA14 dfa14;
   	protected DFA15 dfa15;
   	protected DFA16 dfa16;
   	protected DFA17 dfa17;
	private void InitializeCyclicDFAs()
	{
    	this.dfa1 = new DFA1(this);
    	this.dfa2 = new DFA2(this);
    	this.dfa3 = new DFA3(this);
    	this.dfa6 = new DFA6(this);
    	this.dfa7 = new DFA7(this);
    	this.dfa8 = new DFA8(this);
    	this.dfa9 = new DFA9(this);
    	this.dfa10 = new DFA10(this);
    	this.dfa11 = new DFA11(this);
    	this.dfa12 = new DFA12(this);
    	this.dfa13 = new DFA13(this);
    	this.dfa14 = new DFA14(this);
    	this.dfa15 = new DFA15(this);
    	this.dfa16 = new DFA16(this);
    	this.dfa17 = new DFA17(this);


	    this.dfa3.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA3_SpecialStateTransition);

	    this.dfa7.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA7_SpecialStateTransition);
	    this.dfa8.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA8_SpecialStateTransition);
	    this.dfa9.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA9_SpecialStateTransition);
	    this.dfa10.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA10_SpecialStateTransition);
	    this.dfa11.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA11_SpecialStateTransition);
	    this.dfa12.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA12_SpecialStateTransition);
	    this.dfa13.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA13_SpecialStateTransition);

	    this.dfa15.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA15_SpecialStateTransition);

	    this.dfa17.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA17_SpecialStateTransition);
	}

    const string DFA1_eotS =
        "\x14\uffff";
    const string DFA1_eofS =
        "\x01\x02\x13\uffff";
    const string DFA1_minS =
        "\x01\x40\x13\uffff";
    const string DFA1_maxS =
        "\x01\x64\x13\uffff";
    const string DFA1_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x11\uffff";
    const string DFA1_specialS =
        "\x14\uffff}>";
    static readonly string[] DFA1_transitionS = {
            "\x02\x02\x0a\uffff\x03\x02\x01\x01\x07\x02\x01\uffff\x01\x02"+
            "\x01\uffff\x0b\x02",
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

    static readonly short[] DFA1_eot = DFA.UnpackEncodedString(DFA1_eotS);
    static readonly short[] DFA1_eof = DFA.UnpackEncodedString(DFA1_eofS);
    static readonly char[] DFA1_min = DFA.UnpackEncodedStringToUnsignedChars(DFA1_minS);
    static readonly char[] DFA1_max = DFA.UnpackEncodedStringToUnsignedChars(DFA1_maxS);
    static readonly short[] DFA1_accept = DFA.UnpackEncodedString(DFA1_acceptS);
    static readonly short[] DFA1_special = DFA.UnpackEncodedString(DFA1_specialS);
    static readonly short[][] DFA1_transition = DFA.UnpackEncodedStringArray(DFA1_transitionS);

    protected class DFA1 : DFA
    {
        public DFA1(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 1;
            this.eot = DFA1_eot;
            this.eof = DFA1_eof;
            this.min = DFA1_min;
            this.max = DFA1_max;
            this.accept = DFA1_accept;
            this.special = DFA1_special;
            this.transition = DFA1_transition;

        }

        override public string Description
        {
            get { return "189:34: ( DOT variable )?"; }
        }

    }

    const string DFA2_eotS =
        "\x13\uffff";
    const string DFA2_eofS =
        "\x01\x04\x12\uffff";
    const string DFA2_minS =
        "\x01\x40\x12\uffff";
    const string DFA2_maxS =
        "\x01\x64\x12\uffff";
    const string DFA2_acceptS =
        "\x01\uffff\x01\x01\x02\uffff\x01\x02\x0e\uffff";
    const string DFA2_specialS =
        "\x13\uffff}>";
    static readonly string[] DFA2_transitionS = {
            "\x02\x04\x0a\uffff\x03\x04\x01\uffff\x01\x01\x01\x04\x01\x01"+
            "\x01\x04\x01\x01\x02\x04\x01\uffff\x01\x04\x01\uffff\x0b\x04",
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

    static readonly short[] DFA2_eot = DFA.UnpackEncodedString(DFA2_eotS);
    static readonly short[] DFA2_eof = DFA.UnpackEncodedString(DFA2_eofS);
    static readonly char[] DFA2_min = DFA.UnpackEncodedStringToUnsignedChars(DFA2_minS);
    static readonly char[] DFA2_max = DFA.UnpackEncodedStringToUnsignedChars(DFA2_maxS);
    static readonly short[] DFA2_accept = DFA.UnpackEncodedString(DFA2_acceptS);
    static readonly short[] DFA2_special = DFA.UnpackEncodedString(DFA2_specialS);
    static readonly short[][] DFA2_transition = DFA.UnpackEncodedStringArray(DFA2_transitionS);

    protected class DFA2 : DFA
    {
        public DFA2(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 2;
            this.eot = DFA2_eot;
            this.eof = DFA2_eof;
            this.min = DFA2_min;
            this.max = DFA2_max;
            this.accept = DFA2_accept;
            this.special = DFA2_special;
            this.transition = DFA2_transition;

        }

        override public string Description
        {
            get { return "189:50: ( idx )?"; }
        }

    }

    const string DFA3_eotS =
        "\x1c\uffff";
    const string DFA3_eofS =
        "\x01\x02\x1b\uffff";
    const string DFA3_minS =
        "\x01\x40\x01\x3f\x0e\uffff\x0b\x00\x01\uffff";
    const string DFA3_maxS =
        "\x01\x64\x01\x66\x0e\uffff\x0b\x00\x01\uffff";
    const string DFA3_acceptS =
        "\x02\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA3_specialS =
        "\x10\uffff\x01\x00\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01"+
        "\x06\x01\x07\x01\x08\x01\x09\x01\x0a\x01\uffff}>";
    static readonly string[] DFA3_transitionS = {
            "\x02\x02\x0a\uffff\x03\x02\x02\uffff\x01\x02\x01\uffff\x01"+
            "\x02\x01\uffff\x02\x02\x01\uffff\x01\x02\x01\uffff\x01\x02\x01"+
            "\x01\x09\x02",
            "\x01\x17\x02\x1a\x01\x10\x09\x18\x04\uffff\x01\x12\x01\uffff"+
            "\x01\x13\x01\uffff\x01\x14\x04\uffff\x01\x15\x01\x11\x0a\uffff"+
            "\x01\x16\x01\x19",
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
            ""
    };

    static readonly short[] DFA3_eot = DFA.UnpackEncodedString(DFA3_eotS);
    static readonly short[] DFA3_eof = DFA.UnpackEncodedString(DFA3_eofS);
    static readonly char[] DFA3_min = DFA.UnpackEncodedStringToUnsignedChars(DFA3_minS);
    static readonly char[] DFA3_max = DFA.UnpackEncodedStringToUnsignedChars(DFA3_maxS);
    static readonly short[] DFA3_accept = DFA.UnpackEncodedString(DFA3_acceptS);
    static readonly short[] DFA3_special = DFA.UnpackEncodedString(DFA3_specialS);
    static readonly short[][] DFA3_transition = DFA.UnpackEncodedStringArray(DFA3_transitionS);

    protected class DFA3 : DFA
    {
        public DFA3(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 3;
            this.eot = DFA3_eot;
            this.eof = DFA3_eof;
            this.min = DFA3_min;
            this.max = DFA3_max;
            this.accept = DFA3_accept;
            this.special = DFA3_special;
            this.transition = DFA3_transition;

        }

        override public string Description
        {
            get { return "189:55: ( conditional )?"; }
        }

    }


    protected internal int DFA3_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA3_16 = input.LA(1);

                   	 
                   	int index3_16 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred15_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index3_16);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA3_17 = input.LA(1);

                   	 
                   	int index3_17 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred15_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index3_17);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA3_18 = input.LA(1);

                   	 
                   	int index3_18 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred15_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index3_18);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA3_19 = input.LA(1);

                   	 
                   	int index3_19 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred15_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index3_19);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA3_20 = input.LA(1);

                   	 
                   	int index3_20 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred15_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index3_20);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA3_21 = input.LA(1);

                   	 
                   	int index3_21 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred15_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index3_21);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 6 : 
                   	int LA3_22 = input.LA(1);

                   	 
                   	int index3_22 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred15_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index3_22);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 7 : 
                   	int LA3_23 = input.LA(1);

                   	 
                   	int index3_23 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred15_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index3_23);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 8 : 
                   	int LA3_24 = input.LA(1);

                   	 
                   	int index3_24 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred15_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index3_24);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 9 : 
                   	int LA3_25 = input.LA(1);

                   	 
                   	int index3_25 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred15_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index3_25);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 10 : 
                   	int LA3_26 = input.LA(1);

                   	 
                   	int index3_26 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred15_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index3_26);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae3 =
            new NoViableAltException(dfa.Description, 3, _s, input);
        dfa.Error(nvae3);
        throw nvae3;
    }
    const string DFA6_eotS =
        "\x12\uffff";
    const string DFA6_eofS =
        "\x02\uffff\x02\x05\x0e\uffff";
    const string DFA6_minS =
        "\x01\x3f\x01\uffff\x02\x51\x0e\uffff";
    const string DFA6_maxS =
        "\x01\x66\x01\uffff\x02\x5a\x0e\uffff";
    const string DFA6_acceptS =
        "\x01\uffff\x01\x01\x02\uffff\x01\x03\x01\x04\x04\uffff\x01\x02"+
        "\x07\uffff";
    const string DFA6_specialS =
        "\x12\uffff}>";
    static readonly string[] DFA6_transitionS = {
            "\x0d\x03\x0b\uffff\x01\x01\x0e\uffff\x01\x02",
            "",
            "\x01\x05\x01\uffff\x01\x05\x01\uffff\x02\x05\x01\uffff\x01"+
            "\x0a\x01\uffff\x01\x04",
            "\x01\x05\x01\uffff\x01\x05\x01\uffff\x02\x05\x01\uffff\x01"+
            "\x0a\x01\uffff\x01\x04",
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
            get { return "202:1: variableLagLead : ( StringInQuotes -> ^( ASTVARIABLEANDLEAD StringInQuotes ) | variable PLUS Integer -> ^( ASTVARIABLEANDLEAD variable PLUS Integer ) | variable MINUS Integer -> ^( ASTVARIABLEANDLEAD variable MINUS Integer ) | variable -> ^( ASTVARIABLEANDLEAD variable ) );"; }
        }

    }

    const string DFA7_eotS =
        "\x1c\uffff";
    const string DFA7_eofS =
        "\x01\x01\x1b\uffff";
    const string DFA7_minS =
        "\x01\x40\x09\uffff\x01\x00\x11\uffff";
    const string DFA7_maxS =
        "\x01\x64\x09\uffff\x01\x00\x11\uffff";
    const string DFA7_acceptS =
        "\x01\uffff\x01\x02\x19\uffff\x01\x01";
    const string DFA7_specialS =
        "\x0a\uffff\x01\x00\x11\uffff}>";
    static readonly string[] DFA7_transitionS = {
            "\x01\x01\x01\x0a\x0a\uffff\x03\x01\x02\uffff\x01\x01\x01\uffff"+
            "\x01\x01\x01\uffff\x02\x01\x01\uffff\x01\x01\x01\uffff\x0b\x01",
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
            get { return "()* loopback of 226:27: ( OR andExpression )*"; }
        }

    }


    protected internal int DFA7_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA7_10 = input.LA(1);

                   	 
                   	int index7_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred22_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index7_10);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae7 =
            new NoViableAltException(dfa.Description, 7, _s, input);
        dfa.Error(nvae7);
        throw nvae7;
    }
    const string DFA8_eotS =
        "\x1c\uffff";
    const string DFA8_eofS =
        "\x01\x01\x1b\uffff";
    const string DFA8_minS =
        "\x01\x40\x09\uffff\x01\x00\x11\uffff";
    const string DFA8_maxS =
        "\x01\x64\x09\uffff\x01\x00\x11\uffff";
    const string DFA8_acceptS =
        "\x01\uffff\x01\x02\x19\uffff\x01\x01";
    const string DFA8_specialS =
        "\x0a\uffff\x01\x00\x11\uffff}>";
    static readonly string[] DFA8_transitionS = {
            "\x01\x0a\x01\x01\x0a\uffff\x03\x01\x02\uffff\x01\x01\x01\uffff"+
            "\x01\x01\x01\uffff\x02\x01\x01\uffff\x01\x01\x01\uffff\x0b\x01",
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
            "",
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
            get { return "()* loopback of 228:30: ( AND notExpression )*"; }
        }

    }


    protected internal int DFA8_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA8_10 = input.LA(1);

                   	 
                   	int index8_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred23_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index8_10);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae8 =
            new NoViableAltException(dfa.Description, 8, _s, input);
        dfa.Error(nvae8);
        throw nvae8;
    }
    const string DFA9_eotS =
        "\x26\uffff";
    const string DFA9_eofS =
        "\x01\uffff\x01\x02\x24\uffff";
    const string DFA9_minS =
        "\x02\x3f\x0b\uffff\x03\x00\x03\uffff\x01\x00\x01\uffff\x02\x00"+
        "\x0f\uffff";
    const string DFA9_maxS =
        "\x02\x66\x0b\uffff\x03\x00\x03\uffff\x01\x00\x01\uffff\x02\x00"+
        "\x0f\uffff";
    const string DFA9_acceptS =
        "\x02\uffff\x01\x02\x1d\uffff\x01\x01\x05\uffff";
    const string DFA9_specialS =
        "\x0d\uffff\x01\x00\x01\x01\x01\x02\x03\uffff\x01\x03\x01\uffff"+
        "\x01\x04\x01\x05\x0f\uffff}>";
    static readonly string[] DFA9_transitionS = {
            "\x03\x02\x01\x01\x09\x02\x04\uffff\x01\x02\x01\uffff\x01\x02"+
            "\x01\uffff\x01\x02\x04\uffff\x02\x02\x0a\uffff\x02\x02",
            "\x01\x20\x01\x15\x01\x16\x0a\x20\x04\x02\x01\x0d\x01\x02\x01"+
            "\x0e\x01\x02\x01\x0f\x02\x02\x01\uffff\x01\x02\x01\x20\x01\x13"+
            "\x0a\x02\x02\x20",
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
            "",
            "",
            "",
            "\x01\uffff",
            "",
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
            get { return "230:1: notExpression : ( NOT logicalExpression -> ^( NOT logicalExpression ) | logicalExpression );"; }
        }

    }


    protected internal int DFA9_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA9_13 = input.LA(1);

                   	 
                   	int index9_13 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index9_13);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA9_14 = input.LA(1);

                   	 
                   	int index9_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index9_14);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA9_15 = input.LA(1);

                   	 
                   	int index9_15 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index9_15);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA9_19 = input.LA(1);

                   	 
                   	int index9_19 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index9_19);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA9_21 = input.LA(1);

                   	 
                   	int index9_21 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index9_21);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA9_22 = input.LA(1);

                   	 
                   	int index9_22 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index9_22);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae9 =
            new NoViableAltException(dfa.Description, 9, _s, input);
        dfa.Error(nvae9);
        throw nvae9;
    }
    const string DFA10_eotS =
        "\x1b\uffff";
    const string DFA10_eofS =
        "\x01\x01\x1a\uffff";
    const string DFA10_minS =
        "\x01\x40\x09\uffff\x01\x00\x10\uffff";
    const string DFA10_maxS =
        "\x01\x64\x09\uffff\x01\x00\x10\uffff";
    const string DFA10_acceptS =
        "\x01\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA10_specialS =
        "\x0a\uffff\x01\x00\x10\uffff}>";
    static readonly string[] DFA10_transitionS = {
            "\x02\x01\x0a\uffff\x03\x01\x02\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x02\x01\x01\uffff\x01\x01\x01\uffff\x05\x01\x06"+
            "\x0a",
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
            get { return "()* loopback of 233:40: ( logical additiveExpression )*"; }
        }

    }


    protected internal int DFA10_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA10_10 = input.LA(1);

                   	 
                   	int index10_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred25_GAMS()) ) { s = 26; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index10_10);
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
        "\x1b\uffff";
    const string DFA11_eofS =
        "\x01\x01\x1a\uffff";
    const string DFA11_minS =
        "\x01\x40\x09\uffff\x01\x00\x10\uffff";
    const string DFA11_maxS =
        "\x01\x64\x09\uffff\x01\x00\x10\uffff";
    const string DFA11_acceptS =
        "\x01\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA11_specialS =
        "\x0a\uffff\x01\x00\x10\uffff}>";
    static readonly string[] DFA11_transitionS = {
            "\x02\x01\x0a\uffff\x03\x01\x02\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x02\x01\x01\uffff\x01\x0a\x01\uffff\x01\x0a\x0a"+
            "\x01",
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
            get { return "()* loopback of 235:46: ( ( PLUS | MINUS ) multiplicativeExpression )*"; }
        }

    }


    protected internal int DFA11_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA11_10 = input.LA(1);

                   	 
                   	int index11_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred27_GAMS()) ) { s = 26; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index11_10);
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
        "\x1b\uffff";
    const string DFA12_eofS =
        "\x01\x01\x1a\uffff";
    const string DFA12_minS =
        "\x01\x40\x09\uffff\x01\x00\x10\uffff";
    const string DFA12_maxS =
        "\x01\x64\x09\uffff\x01\x00\x10\uffff";
    const string DFA12_acceptS =
        "\x01\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA12_specialS =
        "\x0a\uffff\x01\x00\x10\uffff}>";
    static readonly string[] DFA12_transitionS = {
            "\x02\x01\x0a\uffff\x03\x01\x02\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x02\x01\x01\uffff\x01\x01\x01\uffff\x02\x01\x02"+
            "\x0a\x07\x01",
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
            get { return "()* loopback of 237:43: ( ( MULT | DIV ) powerExpression )*"; }
        }

    }


    protected internal int DFA12_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA12_10 = input.LA(1);

                   	 
                   	int index12_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred29_GAMS()) ) { s = 26; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index12_10);
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
        "\x1b\uffff";
    const string DFA13_eofS =
        "\x01\x01\x1a\uffff";
    const string DFA13_minS =
        "\x01\x40\x09\uffff\x01\x00\x10\uffff";
    const string DFA13_maxS =
        "\x01\x64\x09\uffff\x01\x00\x10\uffff";
    const string DFA13_acceptS =
        "\x01\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA13_specialS =
        "\x0a\uffff\x01\x00\x10\uffff}>";
    static readonly string[] DFA13_transitionS = {
            "\x02\x01\x0a\uffff\x03\x01\x02\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x02\x01\x01\uffff\x01\x01\x01\uffff\x04\x01\x01"+
            "\x0a\x06\x01",
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
            get { return "()* loopback of 239:34: ( STARS unaryExpression )*"; }
        }

    }


    protected internal int DFA13_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA13_10 = input.LA(1);

                   	 
                   	int index13_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred30_GAMS()) ) { s = 26; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index13_10);
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
        "\x0b\uffff";
    const string DFA14_eofS =
        "\x0b\uffff";
    const string DFA14_minS =
        "\x01\x3f\x0a\uffff";
    const string DFA14_maxS =
        "\x01\x66\x0a\uffff";
    const string DFA14_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x08\uffff";
    const string DFA14_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA14_transitionS = {
            "\x0d\x02\x04\uffff\x01\x02\x01\uffff\x01\x02\x01\uffff\x01"+
            "\x02\x04\uffff\x01\x02\x01\x01\x0a\uffff\x02\x02",
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
            get { return "241:1: unaryExpression : ( MINUS dollarExpression -> ^( NEGATE dollarExpression ) | dollarExpression );"; }
        }

    }

    const string DFA15_eotS =
        "\u0097\uffff";
    const string DFA15_eofS =
        "\u0097\uffff";
    const string DFA15_minS =
        "\x04\x3f\x27\x00\x6c\uffff";
    const string DFA15_maxS =
        "\x04\x66\x27\x00\x6c\uffff";
    const string DFA15_acceptS =
        "\x3a\uffff\x01\x01\x01\x02\x5b\uffff";
    const string DFA15_specialS =
        "\x04\uffff\x01\x00\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01"+
        "\x06\x01\x07\x01\x08\x01\x09\x01\x0a\x01\x0b\x01\x0c\x01\x0d\x01"+
        "\x0e\x01\x0f\x01\x10\x01\x11\x01\x12\x01\x13\x01\x14\x01\x15\x01"+
        "\x16\x01\x17\x01\x18\x01\x19\x01\x1a\x01\x1b\x01\x1c\x01\x1d\x01"+
        "\x1e\x01\x1f\x01\x20\x01\x21\x01\x22\x01\x23\x01\x24\x01\x25\x01"+
        "\x26\x6c\uffff}>";
    static readonly string[] DFA15_transitionS = {
            "\x01\x06\x03\x09\x09\x07\x04\uffff\x01\x01\x01\uffff\x01\x02"+
            "\x01\uffff\x01\x03\x04\uffff\x01\x04\x0b\uffff\x01\x05\x01\x08",
            "\x01\x11\x02\x14\x01\x0a\x09\x12\x04\uffff\x01\x0c\x01\uffff"+
            "\x01\x0d\x01\uffff\x01\x0e\x04\uffff\x01\x0f\x01\x0b\x0a\uffff"+
            "\x01\x10\x01\x13",
            "\x01\x1c\x02\x1f\x01\x15\x09\x1d\x04\uffff\x01\x17\x01\uffff"+
            "\x01\x18\x01\uffff\x01\x19\x04\uffff\x01\x1a\x01\x16\x0a\uffff"+
            "\x01\x1b\x01\x1e",
            "\x01\x27\x02\x2a\x01\x20\x09\x28\x04\uffff\x01\x22\x01\uffff"+
            "\x01\x23\x01\uffff\x01\x24\x04\uffff\x01\x25\x01\x21\x0a\uffff"+
            "\x01\x26\x01\x29",
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
            "\x01\uffff",
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
            get { return "244:1: dollarExpression : ( primaryExpression conditional -> ^( ASTDOLLAREXPRESSION primaryExpression conditional ) | primaryExpression );"; }
        }

    }


    protected internal int DFA15_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA15_4 = input.LA(1);

                   	 
                   	int index15_4 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_4);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA15_5 = input.LA(1);

                   	 
                   	int index15_5 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_5);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA15_6 = input.LA(1);

                   	 
                   	int index15_6 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_6);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA15_7 = input.LA(1);

                   	 
                   	int index15_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_7);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA15_8 = input.LA(1);

                   	 
                   	int index15_8 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_8);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA15_9 = input.LA(1);

                   	 
                   	int index15_9 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_9);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 6 : 
                   	int LA15_10 = input.LA(1);

                   	 
                   	int index15_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_10);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 7 : 
                   	int LA15_11 = input.LA(1);

                   	 
                   	int index15_11 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_11);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 8 : 
                   	int LA15_12 = input.LA(1);

                   	 
                   	int index15_12 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_12);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 9 : 
                   	int LA15_13 = input.LA(1);

                   	 
                   	int index15_13 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_13);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 10 : 
                   	int LA15_14 = input.LA(1);

                   	 
                   	int index15_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_14);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 11 : 
                   	int LA15_15 = input.LA(1);

                   	 
                   	int index15_15 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_15);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 12 : 
                   	int LA15_16 = input.LA(1);

                   	 
                   	int index15_16 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_16);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 13 : 
                   	int LA15_17 = input.LA(1);

                   	 
                   	int index15_17 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_17);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 14 : 
                   	int LA15_18 = input.LA(1);

                   	 
                   	int index15_18 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_18);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 15 : 
                   	int LA15_19 = input.LA(1);

                   	 
                   	int index15_19 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_19);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 16 : 
                   	int LA15_20 = input.LA(1);

                   	 
                   	int index15_20 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_20);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 17 : 
                   	int LA15_21 = input.LA(1);

                   	 
                   	int index15_21 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_21);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 18 : 
                   	int LA15_22 = input.LA(1);

                   	 
                   	int index15_22 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_22);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 19 : 
                   	int LA15_23 = input.LA(1);

                   	 
                   	int index15_23 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_23);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 20 : 
                   	int LA15_24 = input.LA(1);

                   	 
                   	int index15_24 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_24);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 21 : 
                   	int LA15_25 = input.LA(1);

                   	 
                   	int index15_25 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_25);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 22 : 
                   	int LA15_26 = input.LA(1);

                   	 
                   	int index15_26 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_26);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 23 : 
                   	int LA15_27 = input.LA(1);

                   	 
                   	int index15_27 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_27);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 24 : 
                   	int LA15_28 = input.LA(1);

                   	 
                   	int index15_28 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_28);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 25 : 
                   	int LA15_29 = input.LA(1);

                   	 
                   	int index15_29 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_29);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 26 : 
                   	int LA15_30 = input.LA(1);

                   	 
                   	int index15_30 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_30);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 27 : 
                   	int LA15_31 = input.LA(1);

                   	 
                   	int index15_31 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_31);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 28 : 
                   	int LA15_32 = input.LA(1);

                   	 
                   	int index15_32 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_32);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 29 : 
                   	int LA15_33 = input.LA(1);

                   	 
                   	int index15_33 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_33);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 30 : 
                   	int LA15_34 = input.LA(1);

                   	 
                   	int index15_34 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_34);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 31 : 
                   	int LA15_35 = input.LA(1);

                   	 
                   	int index15_35 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_35);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 32 : 
                   	int LA15_36 = input.LA(1);

                   	 
                   	int index15_36 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_36);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 33 : 
                   	int LA15_37 = input.LA(1);

                   	 
                   	int index15_37 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_37);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 34 : 
                   	int LA15_38 = input.LA(1);

                   	 
                   	int index15_38 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_38);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 35 : 
                   	int LA15_39 = input.LA(1);

                   	 
                   	int index15_39 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_39);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 36 : 
                   	int LA15_40 = input.LA(1);

                   	 
                   	int index15_40 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_40);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 37 : 
                   	int LA15_41 = input.LA(1);

                   	 
                   	int index15_41 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_41);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 38 : 
                   	int LA15_42 = input.LA(1);

                   	 
                   	int index15_42 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index15_42);
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
        "\x0a\uffff";
    const string DFA16_eofS =
        "\x0a\uffff";
    const string DFA16_minS =
        "\x01\x3f\x09\uffff";
    const string DFA16_maxS =
        "\x01\x66\x09\uffff";
    const string DFA16_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x05\uffff";
    const string DFA16_specialS =
        "\x0a\uffff}>";
    static readonly string[] DFA16_transitionS = {
            "\x0d\x04\x04\uffff\x01\x01\x01\uffff\x01\x02\x01\uffff\x01"+
            "\x03\x04\uffff\x01\x04\x0b\uffff\x02\x04",
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
            get { return "249:1: primaryExpression : ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value );"; }
        }

    }

    const string DFA17_eotS =
        "\x2f\uffff";
    const string DFA17_eofS =
        "\x03\uffff\x02\x05\x2a\uffff";
    const string DFA17_minS =
        "\x01\x3f\x02\uffff\x02\x40\x02\uffff\x03\x00\x11\uffff\x03\x00"+
        "\x11\uffff";
    const string DFA17_maxS =
        "\x01\x66\x02\uffff\x02\x64\x02\uffff\x03\x00\x11\uffff\x03\x00"+
        "\x11\uffff";
    const string DFA17_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x02\uffff\x01\x05\x27\uffff\x01\x03"+
        "\x01\x04";
    const string DFA17_specialS =
        "\x07\uffff\x01\x00\x01\x01\x01\x02\x11\uffff\x01\x03\x01\x04\x01"+
        "\x05\x11\uffff}>";
    static readonly string[] DFA17_transitionS = {
            "\x01\x03\x03\x05\x09\x04\x0d\uffff\x01\x01\x0b\uffff\x01\x02"+
            "\x01\x05",
            "",
            "",
            "\x02\x05\x0a\uffff\x04\x05\x01\x07\x01\x05\x01\x08\x01\x05"+
            "\x01\x09\x02\x05\x01\uffff\x01\x05\x01\uffff\x0b\x05",
            "\x02\x05\x0a\uffff\x04\x05\x01\x1b\x01\x05\x01\x1c\x01\x05"+
            "\x01\x1d\x02\x05\x01\uffff\x01\x05\x01\uffff\x0b\x05",
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
            get { return "257:1: value : ( Integer -> ^( ASTVALUE Integer ) | Double -> ^( ASTVALUE Double ) | sum -> ^( ASTVALUE sum ) | function -> ^( ASTVALUE function ) | variableWithIndexerEtc -> ^( ASTVALUE variableWithIndexerEtc ) );"; }
        }

    }


    protected internal int DFA17_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA17_7 = input.LA(1);

                   	 
                   	int index17_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred43_GAMS()) ) { s = 45; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index17_7);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA17_8 = input.LA(1);

                   	 
                   	int index17_8 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred43_GAMS()) ) { s = 45; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index17_8);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA17_9 = input.LA(1);

                   	 
                   	int index17_9 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred43_GAMS()) ) { s = 45; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index17_9);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA17_27 = input.LA(1);

                   	 
                   	int index17_27 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred44_GAMS()) ) { s = 46; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index17_27);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA17_28 = input.LA(1);

                   	 
                   	int index17_28 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred44_GAMS()) ) { s = 46; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index17_28);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA17_29 = input.LA(1);

                   	 
                   	int index17_29 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred44_GAMS()) ) { s = 46; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index17_29);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae17 =
            new NoViableAltException(dfa.Description, 17, _s, input);
        dfa.Error(nvae17);
        throw nvae17;
    }
 

    public static readonly BitSet FOLLOW_set_in_extraTokens0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_equ_in_expr453 = new BitSet(new ulong[]{0x0000000000000000UL});
    public static readonly BitSet FOLLOW_EOF_in_expr455 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithIndexerEtc_in_equ469 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000001000UL});
    public static readonly BitSet FOLLOW_DOUBLEDOT_in_equ471 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_expression_in_equ473 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000002000UL});
    public static readonly BitSet FOLLOW_EEQUAL_in_equ475 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_expression_in_equ477 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000004000UL});
    public static readonly BitSet FOLLOW_SEMI_in_equ479 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableWithIndexerEtc529 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000008158000UL});
    public static readonly BitSet FOLLOW_DOT_in_variableWithIndexerEtc532 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006002150FFFUL});
    public static readonly BitSet FOLLOW_variable_in_variableWithIndexerEtc534 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000008150000UL});
    public static readonly BitSet FOLLOW_idx_in_variableWithIndexerEtc538 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_conditional_in_variableWithIndexerEtc541 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_variable592 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_idx599 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006002950FFFUL});
    public static readonly BitSet FOLLOW_indexerElements_in_idx601 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000020000UL});
    public static readonly BitSet FOLLOW_R1_in_idx603 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_idx626 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006002950FFFUL});
    public static readonly BitSet FOLLOW_indexerElements_in_idx628 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000080000UL});
    public static readonly BitSet FOLLOW_R2_in_idx630 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_idx653 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006002950FFFUL});
    public static readonly BitSet FOLLOW_indexerElements_in_idx655 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000200000UL});
    public static readonly BitSet FOLLOW_R3_in_idx657 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableLagLead_in_indexerElements681 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_COMMA_in_indexerElements684 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006002950FFFUL});
    public static readonly BitSet FOLLOW_variableLagLead_in_indexerElements686 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_StringInQuotes_in_variableLagLead718 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead767 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_PLUS_in_variableLagLead769 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_Integer_in_variableLagLead771 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead817 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_MINUS_in_variableLagLead819 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_Integer_in_variableLagLead821 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead857 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DOLLAR_in_conditional901 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_expression_in_conditional903 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_andExpression_in_expression936 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000002UL});
    public static readonly BitSet FOLLOW_OR_in_expression939 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_andExpression_in_expression942 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000002UL});
    public static readonly BitSet FOLLOW_notExpression_in_andExpression951 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000001UL});
    public static readonly BitSet FOLLOW_AND_in_andExpression954 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_notExpression_in_andExpression957 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000001UL});
    public static readonly BitSet FOLLOW_NOT_in_notExpression968 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_logicalExpression_in_notExpression970 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_logicalExpression_in_notExpression990 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_logicalExpression998 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000001F80000000UL});
    public static readonly BitSet FOLLOW_logical_in_logicalExpression1001 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_additiveExpression_in_logicalExpression1004 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000001F80000000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression1013 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000005000000UL});
    public static readonly BitSet FOLLOW_set_in_additiveExpression1017 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression1024 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000005000000UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression1034 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000030000000UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpression1038 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression1045 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000030000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression1055 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000040000000UL});
    public static readonly BitSet FOLLOW_STARS_in_powerExpression1059 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression1062 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000040000000UL});
    public static readonly BitSet FOLLOW_MINUS_in_unaryExpression1073 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_dollarExpression_in_unaryExpression1075 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_dollarExpression_in_unaryExpression1096 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_dollarExpression1115 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_conditional_in_dollarExpression1117 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_dollarExpression1137 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_primaryExpression1158 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression1160 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000020000UL});
    public static readonly BitSet FOLLOW_R1_in_primaryExpression1162 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_primaryExpression1177 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression1179 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000080000UL});
    public static readonly BitSet FOLLOW_R2_in_primaryExpression1181 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_primaryExpression1198 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression1200 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000200000UL});
    public static readonly BitSet FOLLOW_R3_in_primaryExpression1202 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_value_in_primaryExpression1217 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_logical0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Integer_in_value1258 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Double_in_value1289 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_sum_in_value1323 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_value1360 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithIndexerEtc_in_value1408 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_function1440 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000010000UL});
    public static readonly BitSet FOLLOW_L1_in_function1442 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_functionElements_in_function1444 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000020000UL});
    public static readonly BitSet FOLLOW_R1_in_function1446 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_function1488 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000040000UL});
    public static readonly BitSet FOLLOW_L2_in_function1490 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_functionElements_in_function1492 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000080000UL});
    public static readonly BitSet FOLLOW_R2_in_function1494 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_function1536 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000100000UL});
    public static readonly BitSet FOLLOW_L3_in_function1538 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_functionElements_in_function1540 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000200000UL});
    public static readonly BitSet FOLLOW_R3_in_function1542 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_functionName0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_functionElements1617 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_COMMA_in_functionElements1620 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_expression_in_functionElements1622 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SUM_in_sum1655 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000010000UL});
    public static readonly BitSet FOLLOW_L1_in_sum1657 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006002150FFFUL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum1659 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008400000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum1661 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum1664 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_expression_in_sum1666 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000020000UL});
    public static readonly BitSet FOLLOW_R1_in_sum1668 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUM_in_sum1712 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000040000UL});
    public static readonly BitSet FOLLOW_L2_in_sum1714 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006002150FFFUL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum1716 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008400000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum1718 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum1721 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_expression_in_sum1723 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000080000UL});
    public static readonly BitSet FOLLOW_R2_in_sum1725 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUM_in_sum1769 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000100000UL});
    public static readonly BitSet FOLLOW_L3_in_sum1771 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006002150FFFUL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum1773 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008400000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum1775 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum1778 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_expression_in_sum1780 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000200000UL});
    public static readonly BitSet FOLLOW_R3_in_sum1782 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_sumControlled1837 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_sumControlled1864 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006002950FFFUL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled1866 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000020000UL});
    public static readonly BitSet FOLLOW_R1_in_sumControlled1868 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_sumControlled1894 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006002950FFFUL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled1896 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000080000UL});
    public static readonly BitSet FOLLOW_R2_in_sumControlled1898 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_sumControlled1924 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006002950FFFUL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled1926 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000200000UL});
    public static readonly BitSet FOLLOW_R3_in_sumControlled1928 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Ident_in_ident1964 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_extraTokens_in_ident1968 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_conditional_in_synpred15_GAMS541 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_OR_in_synpred22_GAMS939 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_andExpression_in_synpred22_GAMS942 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_AND_in_synpred23_GAMS954 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_notExpression_in_synpred23_GAMS957 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NOT_in_synpred24_GAMS968 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_logicalExpression_in_synpred24_GAMS970 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_logical_in_synpred25_GAMS1001 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_additiveExpression_in_synpred25_GAMS1004 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_synpred27_GAMS1017 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_synpred27_GAMS1024 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_synpred29_GAMS1038 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_powerExpression_in_synpred29_GAMS1045 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STARS_in_synpred30_GAMS1059 = new BitSet(new ulong[]{0x8000000000000000UL,0x0000006006150FFFUL});
    public static readonly BitSet FOLLOW_unaryExpression_in_synpred30_GAMS1062 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_synpred32_GAMS1115 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_conditional_in_synpred32_GAMS1117 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_sum_in_synpred43_GAMS1323 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_synpred44_GAMS1360 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}