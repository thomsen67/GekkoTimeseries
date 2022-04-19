// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 GAMS.g 2022-04-18 18:21:48

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
		"ASTEXPR", 
		"ASTVARWISIMPLE", 
		"ASTVARDEF", 
		"ASTVARDEF0", 
		"ASTVARDEF1", 
		"ASTVARDEF2", 
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
		"MODEL", 
		"SOLVE", 
		"SAMEAS", 
		"VARIABLES", 
		"EQUATIONS", 
		"DOUBLEDOT", 
		"EEQUAL", 
		"SEMI", 
		"EQUAL", 
		"COMMA", 
		"DIV", 
		"DOT", 
		"L1", 
		"R1", 
		"L2", 
		"R2", 
		"L3", 
		"R3", 
		"StringInQuotes", 
		"PLUS", 
		"Integer", 
		"MINUS", 
		"DOLLAR", 
		"MULT", 
		"STARS", 
		"NONEQUAL", 
		"LESSTHANOREQUAL", 
		"GREATERTHANOREQUAL", 
		"LESSTHAN", 
		"GREATERTHAN", 
		"Double", 
		"Ident", 
		"NEWLINE2", 
		"NEWLINE3", 
		"Comment", 
		"NESTED_ML_COMMENT", 
		"COMMENT1", 
		"COMMENT2", 
		"WHITESPACE", 
		"EQU", 
		"MOD", 
		"DIGIT", 
		"Exponent", 
		"LETTER", 
		"NEWLINE", 
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

    public const int ASTPOW = 21;
    public const int ASTFUNCTIONELEMENTS1 = 60;
    public const int COMMENT1 = 117;
    public const int COMMENT2 = 118;
    public const int ASTFUNCTION3 = 35;
    public const int ASTFUNCTION2 = 34;
    public const int ASTVARIABLE = 22;
    public const int LETTER = 124;
    public const int MOD = 121;
    public const int ASTFUNCTION1 = 33;
    public const int ASTIDX = 39;
    public const int ASTFUNCTION0 = 57;
    public const int LOG = 75;
    public const int EQUATIONS = 85;
    public const int ASTINDEXES1 = 24;
    public const int ASTCONDITIONAL = 31;
    public const int DOUBLEDOT = 86;
    public const int ASTINDEXES3 = 26;
    public const int NOT = 72;
    public const int ASTVAR = 36;
    public const int ASTINDEXES2 = 25;
    public const int EOF = -1;
    public const int NONEQUAL = 106;
    public const int ASTINTEGER = 19;
    public const int TANH = 80;
    public const int ASTEQU = 11;
    public const int Comment = 115;
    public const int EXP = 74;
    public const int ASTDOLLAREXPRESSION = 55;
    public const int EEQUAL = 87;
    public const int SQR = 79;
    public const int GREATERTHANOREQUAL = 108;
    public const int ASTFUNCTIONELEMENTS0 = 59;
    public const int GREATERTHAN = 110;
    public const int D = 129;
    public const int Double = 111;
    public const int ASTEQU1 = 41;
    public const int E = 130;
    public const int ASTEQU2 = 42;
    public const int F = 131;
    public const int G = 132;
    public const int ASTEQU0 = 40;
    public const int A = 126;
    public const int B = 127;
    public const int ASTEQU3 = 43;
    public const int ASTEXPR = 5;
    public const int C = 128;
    public const int L = 137;
    public const int M = 138;
    public const int N = 139;
    public const int NESTED_ML_COMMENT = 116;
    public const int ASTVARWI4 = 49;
    public const int O = 140;
    public const int ASTVARWI3 = 48;
    public const int H = 133;
    public const int ASTVARWI2 = 47;
    public const int ASTFUNCTIONELEMENTS = 58;
    public const int I = 134;
    public const int ASTVARWISIMPLE = 6;
    public const int ASTVARWI1 = 46;
    public const int J = 135;
    public const int NEWLINE2 = 113;
    public const int ASTVARWI0 = 45;
    public const int K = 136;
    public const int NEWLINE3 = 114;
    public const int U = 146;
    public const int T = 145;
    public const int W = 148;
    public const int WHITESPACE = 119;
    public const int POWER = 78;
    public const int V = 147;
    public const int Q = 142;
    public const int P = 141;
    public const int S = 144;
    public const int R = 143;
    public const int MULT = 104;
    public const int ASTVARWI = 44;
    public const int Y = 150;
    public const int ASTIDXELEMENTS1 = 52;
    public const int X = 149;
    public const int ASTIDXELEMENTS0 = 53;
    public const int Z = 151;
    public const int ABS = 73;
    public const int Ident = 112;
    public const int ASTEXPRESSION = 14;
    public const int OR = 71;
    public const int StringInQuotes = 99;
    public const int ASTSUM = 32;
    public const int ASTDEFINITION = 30;
    public const int SOLVE = 82;
    public const int DOLLAR = 103;
    public const int ASTFUNCTION = 18;
    public const int ASTEQUCODE = 13;
    public const int MAX = 76;
    public const int Exponent = 123;
    public const int R2 = 96;
    public const int R3 = 98;
    public const int SUM = 69;
    public const int AND = 70;
    public const int COMMA = 90;
    public const int R1 = 94;
    public const int ASTSIMPLEFUNCTION1 = 15;
    public const int EQUAL = 89;
    public const int ASTSIMPLEFUNCTION2 = 16;
    public const int LESSTHANOREQUAL = 107;
    public const int ASTSIMPLEFUNCTION3 = 17;
    public const int PLUS = 100;
    public const int ASTEND = 23;
    public const int MODEL = 81;
    public const int DIGIT = 122;
    public const int DOT = 92;
    public const int ASTSUMCONTROLLED = 66;
    public const int ASTSUMCONTROLLEDSIMPLE = 65;
    public const int ASTEXPRESSION2 = 28;
    public const int ASTEXPRESSION3 = 29;
    public const int ASTIDXELEMENTS = 51;
    public const int LESSTHAN = 109;
    public const int ASTVALUE = 56;
    public const int ASTVARIABLEWITHINDEXERETC = 37;
    public const int ASTVARDEF = 7;
    public const int ASTIDX0 = 50;
    public const int ASTEXPRESSION1 = 27;
    public const int NEGATE = 4;
    public const int SAMEAS = 83;
    public const int MIN = 77;
    public const int MINUS = 102;
    public const int ASTVARDEF0 = 8;
    public const int ASTVARDEF1 = 9;
    public const int ASTVARIABLEANDLEAD = 54;
    public const int ASTVARDEF2 = 10;
    public const int SEMI = 88;
    public const int L1 = 93;
    public const int L2 = 95;
    public const int ASTSUM0 = 61;
    public const int L3 = 97;
    public const int ASTLEFTSIDE = 12;
    public const int NEWLINE = 125;
    public const int ASTSUM2 = 63;
    public const int ASTSUMCONTROLLED2 = 68;
    public const int ASTSUM1 = 62;
    public const int ASTSUMCONTROLLED0 = 67;
    public const int ASTSUM3 = 64;
    public const int VARIABLES = 84;
    public const int EQU = 120;
    public const int STARS = 105;
    public const int ASTDOUBLE = 20;
    public const int DIV = 91;
    public const int Integer = 101;
    public const int ASTDOT = 38;

    // delegates
    // delegators



        public GAMSParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public GAMSParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();
            this.state.ruleMemo = new Hashtable[114+1];
             
             
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
    // GAMS.g:170:1: extraTokens : ( SUM | AND | OR | NOT | ABS | EXP | LOG | MAX | MIN | POWER | SQR | TANH | MODEL | SOLVE | SAMEAS | VARIABLES | EQUATIONS );
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
            // GAMS.g:170:13: ( SUM | AND | OR | NOT | ABS | EXP | LOG | MAX | MIN | POWER | SQR | TANH | MODEL | SOLVE | SAMEAS | VARIABLES | EQUATIONS )
            // GAMS.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set1 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= SUM && input.LA(1) <= EQUATIONS) ) 
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

    public class gams_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "gams"
    // GAMS.g:194:1: gams : ( expr )* EOF -> ^( ASTEXPR ( expr )* ) ;
    public GAMSParser.gams_return gams() // throws RecognitionException [1]
    {   
        GAMSParser.gams_return retval = new GAMSParser.gams_return();
        retval.Start = input.LT(1);
        int gams_StartIndex = input.Index();
        object root_0 = null;

        IToken EOF3 = null;
        GAMSParser.expr_return expr2 = default(GAMSParser.expr_return);


        object EOF3_tree=null;
        RewriteRuleTokenStream stream_EOF = new RewriteRuleTokenStream(adaptor,"token EOF");
        RewriteRuleSubtreeStream stream_expr = new RewriteRuleSubtreeStream(adaptor,"rule expr");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 2) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:194:5: ( ( expr )* EOF -> ^( ASTEXPR ( expr )* ) )
            // GAMS.g:194:7: ( expr )* EOF
            {
            	// GAMS.g:194:7: ( expr )*
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( ((LA1_0 >= SUM && LA1_0 <= EQUATIONS) || LA1_0 == Ident) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // GAMS.g:0:0: expr
            			    {
            			    	PushFollow(FOLLOW_expr_in_gams493);
            			    	expr2 = expr();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_expr.Add(expr2.Tree);

            			    }
            			    break;

            			default:
            			    goto loop1;
            	    }
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements

            	EOF3=(IToken)Match(input,EOF,FOLLOW_EOF_in_gams496); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_EOF.Add(EOF3);



            	// AST REWRITE
            	// elements:          expr
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 194:17: -> ^( ASTEXPR ( expr )* )
            	{
            	    // GAMS.g:194:20: ^( ASTEXPR ( expr )* )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEXPR, "ASTEXPR"), root_1);

            	    // GAMS.g:194:30: ( expr )*
            	    while ( stream_expr.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_expr.NextTree());

            	    }
            	    stream_expr.Reset();

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
            	Memoize(input, 2, gams_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "gams"

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
    // GAMS.g:196:1: expr : ( equ | vardef | variables | equations | model | solve );
    public GAMSParser.expr_return expr() // throws RecognitionException [1]
    {   
        GAMSParser.expr_return retval = new GAMSParser.expr_return();
        retval.Start = input.LT(1);
        int expr_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.equ_return equ4 = default(GAMSParser.equ_return);

        GAMSParser.vardef_return vardef5 = default(GAMSParser.vardef_return);

        GAMSParser.variables_return variables6 = default(GAMSParser.variables_return);

        GAMSParser.equations_return equations7 = default(GAMSParser.equations_return);

        GAMSParser.model_return model8 = default(GAMSParser.model_return);

        GAMSParser.solve_return solve9 = default(GAMSParser.solve_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 3) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:196:5: ( equ | vardef | variables | equations | model | solve )
            int alt2 = 6;
            alt2 = dfa2.Predict(input);
            switch (alt2) 
            {
                case 1 :
                    // GAMS.g:196:7: equ
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_equ_in_expr517);
                    	equ4 = equ();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, equ4.Tree);

                    }
                    break;
                case 2 :
                    // GAMS.g:197:9: vardef
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_vardef_in_expr527);
                    	vardef5 = vardef();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, vardef5.Tree);

                    }
                    break;
                case 3 :
                    // GAMS.g:198:6: variables
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variables_in_expr534);
                    	variables6 = variables();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variables6.Tree);

                    }
                    break;
                case 4 :
                    // GAMS.g:199:6: equations
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_equations_in_expr541);
                    	equations7 = equations();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, equations7.Tree);

                    }
                    break;
                case 5 :
                    // GAMS.g:200:6: model
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_model_in_expr548);
                    	model8 = model();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, model8.Tree);

                    }
                    break;
                case 6 :
                    // GAMS.g:201:6: solve
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_solve_in_expr555);
                    	solve9 = solve();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, solve9.Tree);

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
            	Memoize(input, 3, expr_StartIndex); 
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
    // GAMS.g:204:1: equ : variableWithIndexerSimple DOUBLEDOT expression EEQUAL expression SEMI -> ^( ASTEQU ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI ) ^( ASTEQU1 variableWithIndexerSimple ) ^( ASTEQU2 expression ) ^( ASTEQU3 expression ) ) ;
    public GAMSParser.equ_return equ() // throws RecognitionException [1]
    {   
        GAMSParser.equ_return retval = new GAMSParser.equ_return();
        retval.Start = input.LT(1);
        int equ_StartIndex = input.Index();
        object root_0 = null;

        IToken DOUBLEDOT11 = null;
        IToken EEQUAL13 = null;
        IToken SEMI15 = null;
        GAMSParser.variableWithIndexerSimple_return variableWithIndexerSimple10 = default(GAMSParser.variableWithIndexerSimple_return);

        GAMSParser.expression_return expression12 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression14 = default(GAMSParser.expression_return);


        object DOUBLEDOT11_tree=null;
        object EEQUAL13_tree=null;
        object SEMI15_tree=null;
        RewriteRuleTokenStream stream_EEQUAL = new RewriteRuleTokenStream(adaptor,"token EEQUAL");
        RewriteRuleTokenStream stream_DOUBLEDOT = new RewriteRuleTokenStream(adaptor,"token DOUBLEDOT");
        RewriteRuleTokenStream stream_SEMI = new RewriteRuleTokenStream(adaptor,"token SEMI");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_variableWithIndexerSimple = new RewriteRuleSubtreeStream(adaptor,"rule variableWithIndexerSimple");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 4) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:204:4: ( variableWithIndexerSimple DOUBLEDOT expression EEQUAL expression SEMI -> ^( ASTEQU ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI ) ^( ASTEQU1 variableWithIndexerSimple ) ^( ASTEQU2 expression ) ^( ASTEQU3 expression ) ) )
            // GAMS.g:204:9: variableWithIndexerSimple DOUBLEDOT expression EEQUAL expression SEMI
            {
            	PushFollow(FOLLOW_variableWithIndexerSimple_in_equ569);
            	variableWithIndexerSimple10 = variableWithIndexerSimple();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableWithIndexerSimple.Add(variableWithIndexerSimple10.Tree);
            	DOUBLEDOT11=(IToken)Match(input,DOUBLEDOT,FOLLOW_DOUBLEDOT_in_equ571); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_DOUBLEDOT.Add(DOUBLEDOT11);

            	PushFollow(FOLLOW_expression_in_equ573);
            	expression12 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression12.Tree);
            	EEQUAL13=(IToken)Match(input,EEQUAL,FOLLOW_EEQUAL_in_equ575); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_EEQUAL.Add(EEQUAL13);

            	PushFollow(FOLLOW_expression_in_equ577);
            	expression14 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression14.Tree);
            	SEMI15=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_equ579); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI15);

            	if ( (state.backtracking==0) )
            	{
            	  equItems.Add(input.ToString((IToken)retval.Start,input.LT(-1)));
            	}


            	// AST REWRITE
            	// elements:          DOUBLEDOT, expression, EEQUAL, SEMI, variableWithIndexerSimple, expression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 205:3: -> ^( ASTEQU ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI ) ^( ASTEQU1 variableWithIndexerSimple ) ^( ASTEQU2 expression ) ^( ASTEQU3 expression ) )
            	{
            	    // GAMS.g:205:6: ^( ASTEQU ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI ) ^( ASTEQU1 variableWithIndexerSimple ) ^( ASTEQU2 expression ) ^( ASTEQU3 expression ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU, "ASTEQU"), root_1);

            	    // GAMS.g:205:15: ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU0, "ASTEQU0"), root_2);

            	    adaptor.AddChild(root_2, stream_DOUBLEDOT.NextNode());
            	    adaptor.AddChild(root_2, stream_EEQUAL.NextNode());
            	    adaptor.AddChild(root_2, stream_SEMI.NextNode());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:205:48: ^( ASTEQU1 variableWithIndexerSimple )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU1, "ASTEQU1"), root_2);

            	    adaptor.AddChild(root_2, stream_variableWithIndexerSimple.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:205:85: ^( ASTEQU2 expression )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU2, "ASTEQU2"), root_2);

            	    adaptor.AddChild(root_2, stream_expression.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:205:107: ^( ASTEQU3 expression )
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
            	Memoize(input, 4, equ_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "equ"

    public class vardef_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "vardef"
    // GAMS.g:207:1: vardef : variableWithIndexerEtc EQUAL expression SEMI -> ^( ASTVARDEF ^( ASTVARDEF0 EQUAL SEMI ) ^( ASTVARDEF1 variableWithIndexerEtc ) ^( ASTVARDEF2 expression ) ) ;
    public GAMSParser.vardef_return vardef() // throws RecognitionException [1]
    {   
        GAMSParser.vardef_return retval = new GAMSParser.vardef_return();
        retval.Start = input.LT(1);
        int vardef_StartIndex = input.Index();
        object root_0 = null;

        IToken EQUAL17 = null;
        IToken SEMI19 = null;
        GAMSParser.variableWithIndexerEtc_return variableWithIndexerEtc16 = default(GAMSParser.variableWithIndexerEtc_return);

        GAMSParser.expression_return expression18 = default(GAMSParser.expression_return);


        object EQUAL17_tree=null;
        object SEMI19_tree=null;
        RewriteRuleTokenStream stream_SEMI = new RewriteRuleTokenStream(adaptor,"token SEMI");
        RewriteRuleTokenStream stream_EQUAL = new RewriteRuleTokenStream(adaptor,"token EQUAL");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_variableWithIndexerEtc = new RewriteRuleSubtreeStream(adaptor,"rule variableWithIndexerEtc");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 5) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:207:7: ( variableWithIndexerEtc EQUAL expression SEMI -> ^( ASTVARDEF ^( ASTVARDEF0 EQUAL SEMI ) ^( ASTVARDEF1 variableWithIndexerEtc ) ^( ASTVARDEF2 expression ) ) )
            // GAMS.g:207:11: variableWithIndexerEtc EQUAL expression SEMI
            {
            	PushFollow(FOLLOW_variableWithIndexerEtc_in_vardef627);
            	variableWithIndexerEtc16 = variableWithIndexerEtc();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableWithIndexerEtc.Add(variableWithIndexerEtc16.Tree);
            	EQUAL17=(IToken)Match(input,EQUAL,FOLLOW_EQUAL_in_vardef629); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_EQUAL.Add(EQUAL17);

            	PushFollow(FOLLOW_expression_in_vardef631);
            	expression18 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression18.Tree);
            	SEMI19=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_vardef633); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI19);



            	// AST REWRITE
            	// elements:          SEMI, EQUAL, expression, variableWithIndexerEtc
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 208:3: -> ^( ASTVARDEF ^( ASTVARDEF0 EQUAL SEMI ) ^( ASTVARDEF1 variableWithIndexerEtc ) ^( ASTVARDEF2 expression ) )
            	{
            	    // GAMS.g:208:6: ^( ASTVARDEF ^( ASTVARDEF0 EQUAL SEMI ) ^( ASTVARDEF1 variableWithIndexerEtc ) ^( ASTVARDEF2 expression ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARDEF, "ASTVARDEF"), root_1);

            	    // GAMS.g:208:18: ^( ASTVARDEF0 EQUAL SEMI )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARDEF0, "ASTVARDEF0"), root_2);

            	    adaptor.AddChild(root_2, stream_EQUAL.NextNode());
            	    adaptor.AddChild(root_2, stream_SEMI.NextNode());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:208:43: ^( ASTVARDEF1 variableWithIndexerEtc )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARDEF1, "ASTVARDEF1"), root_2);

            	    adaptor.AddChild(root_2, stream_variableWithIndexerEtc.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:208:80: ^( ASTVARDEF2 expression )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARDEF2, "ASTVARDEF2"), root_2);

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
            	Memoize(input, 5, vardef_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "vardef"

    public class variables_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variables"
    // GAMS.g:210:1: variables : VARIABLES variableWithIndexerSimple ( COMMA variableWithIndexerSimple )* SEMI -> ( variableWithIndexerSimple )+ ;
    public GAMSParser.variables_return variables() // throws RecognitionException [1]
    {   
        GAMSParser.variables_return retval = new GAMSParser.variables_return();
        retval.Start = input.LT(1);
        int variables_StartIndex = input.Index();
        object root_0 = null;

        IToken VARIABLES20 = null;
        IToken COMMA22 = null;
        IToken SEMI24 = null;
        GAMSParser.variableWithIndexerSimple_return variableWithIndexerSimple21 = default(GAMSParser.variableWithIndexerSimple_return);

        GAMSParser.variableWithIndexerSimple_return variableWithIndexerSimple23 = default(GAMSParser.variableWithIndexerSimple_return);


        object VARIABLES20_tree=null;
        object COMMA22_tree=null;
        object SEMI24_tree=null;
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleTokenStream stream_SEMI = new RewriteRuleTokenStream(adaptor,"token SEMI");
        RewriteRuleTokenStream stream_VARIABLES = new RewriteRuleTokenStream(adaptor,"token VARIABLES");
        RewriteRuleSubtreeStream stream_variableWithIndexerSimple = new RewriteRuleSubtreeStream(adaptor,"rule variableWithIndexerSimple");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 6) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:210:10: ( VARIABLES variableWithIndexerSimple ( COMMA variableWithIndexerSimple )* SEMI -> ( variableWithIndexerSimple )+ )
            // GAMS.g:210:12: VARIABLES variableWithIndexerSimple ( COMMA variableWithIndexerSimple )* SEMI
            {
            	VARIABLES20=(IToken)Match(input,VARIABLES,FOLLOW_VARIABLES_in_variables669); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_VARIABLES.Add(VARIABLES20);

            	PushFollow(FOLLOW_variableWithIndexerSimple_in_variables671);
            	variableWithIndexerSimple21 = variableWithIndexerSimple();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableWithIndexerSimple.Add(variableWithIndexerSimple21.Tree);
            	// GAMS.g:210:48: ( COMMA variableWithIndexerSimple )*
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
            			    // GAMS.g:210:49: COMMA variableWithIndexerSimple
            			    {
            			    	COMMA22=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_variables674); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA22);

            			    	PushFollow(FOLLOW_variableWithIndexerSimple_in_variables676);
            			    	variableWithIndexerSimple23 = variableWithIndexerSimple();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_variableWithIndexerSimple.Add(variableWithIndexerSimple23.Tree);

            			    }
            			    break;

            			default:
            			    goto loop3;
            	    }
            	} while (true);

            	loop3:
            		;	// Stops C# compiler whining that label 'loop3' has no statements

            	SEMI24=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_variables680); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI24);



            	// AST REWRITE
            	// elements:          variableWithIndexerSimple
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 210:88: -> ( variableWithIndexerSimple )+
            	{
            	    if ( !(stream_variableWithIndexerSimple.HasNext()) ) {
            	        throw new RewriteEarlyExitException();
            	    }
            	    while ( stream_variableWithIndexerSimple.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_variableWithIndexerSimple.NextTree());

            	    }
            	    stream_variableWithIndexerSimple.Reset();

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
            	Memoize(input, 6, variables_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "variables"

    public class equations_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "equations"
    // GAMS.g:212:1: equations : VARIABLES variableWithIndexerSimple ( COMMA variableWithIndexerSimple )* SEMI -> ( variableWithIndexerSimple )+ ;
    public GAMSParser.equations_return equations() // throws RecognitionException [1]
    {   
        GAMSParser.equations_return retval = new GAMSParser.equations_return();
        retval.Start = input.LT(1);
        int equations_StartIndex = input.Index();
        object root_0 = null;

        IToken VARIABLES25 = null;
        IToken COMMA27 = null;
        IToken SEMI29 = null;
        GAMSParser.variableWithIndexerSimple_return variableWithIndexerSimple26 = default(GAMSParser.variableWithIndexerSimple_return);

        GAMSParser.variableWithIndexerSimple_return variableWithIndexerSimple28 = default(GAMSParser.variableWithIndexerSimple_return);


        object VARIABLES25_tree=null;
        object COMMA27_tree=null;
        object SEMI29_tree=null;
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleTokenStream stream_SEMI = new RewriteRuleTokenStream(adaptor,"token SEMI");
        RewriteRuleTokenStream stream_VARIABLES = new RewriteRuleTokenStream(adaptor,"token VARIABLES");
        RewriteRuleSubtreeStream stream_variableWithIndexerSimple = new RewriteRuleSubtreeStream(adaptor,"rule variableWithIndexerSimple");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 7) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:212:10: ( VARIABLES variableWithIndexerSimple ( COMMA variableWithIndexerSimple )* SEMI -> ( variableWithIndexerSimple )+ )
            // GAMS.g:212:12: VARIABLES variableWithIndexerSimple ( COMMA variableWithIndexerSimple )* SEMI
            {
            	VARIABLES25=(IToken)Match(input,VARIABLES,FOLLOW_VARIABLES_in_equations692); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_VARIABLES.Add(VARIABLES25);

            	PushFollow(FOLLOW_variableWithIndexerSimple_in_equations694);
            	variableWithIndexerSimple26 = variableWithIndexerSimple();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableWithIndexerSimple.Add(variableWithIndexerSimple26.Tree);
            	// GAMS.g:212:48: ( COMMA variableWithIndexerSimple )*
            	do 
            	{
            	    int alt4 = 2;
            	    int LA4_0 = input.LA(1);

            	    if ( (LA4_0 == COMMA) )
            	    {
            	        alt4 = 1;
            	    }


            	    switch (alt4) 
            		{
            			case 1 :
            			    // GAMS.g:212:49: COMMA variableWithIndexerSimple
            			    {
            			    	COMMA27=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_equations697); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA27);

            			    	PushFollow(FOLLOW_variableWithIndexerSimple_in_equations699);
            			    	variableWithIndexerSimple28 = variableWithIndexerSimple();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_variableWithIndexerSimple.Add(variableWithIndexerSimple28.Tree);

            			    }
            			    break;

            			default:
            			    goto loop4;
            	    }
            	} while (true);

            	loop4:
            		;	// Stops C# compiler whining that label 'loop4' has no statements

            	SEMI29=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_equations703); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI29);



            	// AST REWRITE
            	// elements:          variableWithIndexerSimple
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 212:88: -> ( variableWithIndexerSimple )+
            	{
            	    if ( !(stream_variableWithIndexerSimple.HasNext()) ) {
            	        throw new RewriteEarlyExitException();
            	    }
            	    while ( stream_variableWithIndexerSimple.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_variableWithIndexerSimple.NextTree());

            	    }
            	    stream_variableWithIndexerSimple.Reset();

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
            	Memoize(input, 7, equations_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "equations"

    public class model_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "model"
    // GAMS.g:214:1: model : MODEL ( ident | DIV )* SEMI ->;
    public GAMSParser.model_return model() // throws RecognitionException [1]
    {   
        GAMSParser.model_return retval = new GAMSParser.model_return();
        retval.Start = input.LT(1);
        int model_StartIndex = input.Index();
        object root_0 = null;

        IToken MODEL30 = null;
        IToken DIV32 = null;
        IToken SEMI33 = null;
        GAMSParser.ident_return ident31 = default(GAMSParser.ident_return);


        object MODEL30_tree=null;
        object DIV32_tree=null;
        object SEMI33_tree=null;
        RewriteRuleTokenStream stream_MODEL = new RewriteRuleTokenStream(adaptor,"token MODEL");
        RewriteRuleTokenStream stream_DIV = new RewriteRuleTokenStream(adaptor,"token DIV");
        RewriteRuleTokenStream stream_SEMI = new RewriteRuleTokenStream(adaptor,"token SEMI");
        RewriteRuleSubtreeStream stream_ident = new RewriteRuleSubtreeStream(adaptor,"rule ident");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 8) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:214:6: ( MODEL ( ident | DIV )* SEMI ->)
            // GAMS.g:214:8: MODEL ( ident | DIV )* SEMI
            {
            	MODEL30=(IToken)Match(input,MODEL,FOLLOW_MODEL_in_model715); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_MODEL.Add(MODEL30);

            	// GAMS.g:214:14: ( ident | DIV )*
            	do 
            	{
            	    int alt5 = 3;
            	    int LA5_0 = input.LA(1);

            	    if ( ((LA5_0 >= SUM && LA5_0 <= EQUATIONS) || LA5_0 == Ident) )
            	    {
            	        alt5 = 1;
            	    }
            	    else if ( (LA5_0 == DIV) )
            	    {
            	        alt5 = 2;
            	    }


            	    switch (alt5) 
            		{
            			case 1 :
            			    // GAMS.g:214:15: ident
            			    {
            			    	PushFollow(FOLLOW_ident_in_model718);
            			    	ident31 = ident();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_ident.Add(ident31.Tree);

            			    }
            			    break;
            			case 2 :
            			    // GAMS.g:214:23: DIV
            			    {
            			    	DIV32=(IToken)Match(input,DIV,FOLLOW_DIV_in_model722); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_DIV.Add(DIV32);


            			    }
            			    break;

            			default:
            			    goto loop5;
            	    }
            	} while (true);

            	loop5:
            		;	// Stops C# compiler whining that label 'loop5' has no statements

            	SEMI33=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_model726); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI33);



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
            	// 214:34: ->
            	{
            	    root_0 = null;
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
            	Memoize(input, 8, model_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "model"

    public class solve_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "solve"
    // GAMS.g:216:1: solve : SOLVE ( ident )* SEMI ->;
    public GAMSParser.solve_return solve() // throws RecognitionException [1]
    {   
        GAMSParser.solve_return retval = new GAMSParser.solve_return();
        retval.Start = input.LT(1);
        int solve_StartIndex = input.Index();
        object root_0 = null;

        IToken SOLVE34 = null;
        IToken SEMI36 = null;
        GAMSParser.ident_return ident35 = default(GAMSParser.ident_return);


        object SOLVE34_tree=null;
        object SEMI36_tree=null;
        RewriteRuleTokenStream stream_SOLVE = new RewriteRuleTokenStream(adaptor,"token SOLVE");
        RewriteRuleTokenStream stream_SEMI = new RewriteRuleTokenStream(adaptor,"token SEMI");
        RewriteRuleSubtreeStream stream_ident = new RewriteRuleSubtreeStream(adaptor,"rule ident");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 9) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:216:6: ( SOLVE ( ident )* SEMI ->)
            // GAMS.g:216:8: SOLVE ( ident )* SEMI
            {
            	SOLVE34=(IToken)Match(input,SOLVE,FOLLOW_SOLVE_in_solve736); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SOLVE.Add(SOLVE34);

            	// GAMS.g:216:14: ( ident )*
            	do 
            	{
            	    int alt6 = 2;
            	    int LA6_0 = input.LA(1);

            	    if ( ((LA6_0 >= SUM && LA6_0 <= EQUATIONS) || LA6_0 == Ident) )
            	    {
            	        alt6 = 1;
            	    }


            	    switch (alt6) 
            		{
            			case 1 :
            			    // GAMS.g:0:0: ident
            			    {
            			    	PushFollow(FOLLOW_ident_in_solve738);
            			    	ident35 = ident();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_ident.Add(ident35.Tree);

            			    }
            			    break;

            			default:
            			    goto loop6;
            	    }
            	} while (true);

            	loop6:
            		;	// Stops C# compiler whining that label 'loop6' has no statements

            	SEMI36=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_solve741); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI36);



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
            	// 216:26: ->
            	{
            	    root_0 = null;
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
            	Memoize(input, 9, solve_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "solve"

    public class variableWithIndexerSimple_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variableWithIndexerSimple"
    // GAMS.g:222:1: variableWithIndexerSimple : variable ( idx )? -> ^( ASTVARWISIMPLE variable ( idx )? ) ;
    public GAMSParser.variableWithIndexerSimple_return variableWithIndexerSimple() // throws RecognitionException [1]
    {   
        GAMSParser.variableWithIndexerSimple_return retval = new GAMSParser.variableWithIndexerSimple_return();
        retval.Start = input.LT(1);
        int variableWithIndexerSimple_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.variable_return variable37 = default(GAMSParser.variable_return);

        GAMSParser.idx_return idx38 = default(GAMSParser.idx_return);


        RewriteRuleSubtreeStream stream_idx = new RewriteRuleSubtreeStream(adaptor,"rule idx");
        RewriteRuleSubtreeStream stream_variable = new RewriteRuleSubtreeStream(adaptor,"rule variable");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 10) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:222:26: ( variable ( idx )? -> ^( ASTVARWISIMPLE variable ( idx )? ) )
            // GAMS.g:222:28: variable ( idx )?
            {
            	PushFollow(FOLLOW_variable_in_variableWithIndexerSimple755);
            	variable37 = variable();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variable.Add(variable37.Tree);
            	// GAMS.g:222:37: ( idx )?
            	int alt7 = 2;
            	int LA7_0 = input.LA(1);

            	if ( (LA7_0 == L1 || LA7_0 == L2 || LA7_0 == L3) )
            	{
            	    alt7 = 1;
            	}
            	switch (alt7) 
            	{
            	    case 1 :
            	        // GAMS.g:0:0: idx
            	        {
            	        	PushFollow(FOLLOW_idx_in_variableWithIndexerSimple757);
            	        	idx38 = idx();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_idx.Add(idx38.Tree);

            	        }
            	        break;

            	}



            	// AST REWRITE
            	// elements:          idx, variable
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 222:42: -> ^( ASTVARWISIMPLE variable ( idx )? )
            	{
            	    // GAMS.g:222:45: ^( ASTVARWISIMPLE variable ( idx )? )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWISIMPLE, "ASTVARWISIMPLE"), root_1);

            	    adaptor.AddChild(root_1, stream_variable.NextTree());
            	    // GAMS.g:222:71: ( idx )?
            	    if ( stream_idx.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_idx.NextTree());

            	    }
            	    stream_idx.Reset();

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
            	Memoize(input, 10, variableWithIndexerSimple_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "variableWithIndexerSimple"

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
    // GAMS.g:224:1: variableWithIndexerEtc : variable ( DOT variable )? ( idx )? ( conditional )? -> ^( ASTVARWI ^( ASTVARWI0 ( DOT )? ) ^( ASTVARWI1 variable ) ^( ASTVARWI2 ( variable )? ) ^( ASTVARWI3 ( idx )? ) ^( ASTVARWI4 ( conditional )? ) ) ;
    public GAMSParser.variableWithIndexerEtc_return variableWithIndexerEtc() // throws RecognitionException [1]
    {   
        GAMSParser.variableWithIndexerEtc_return retval = new GAMSParser.variableWithIndexerEtc_return();
        retval.Start = input.LT(1);
        int variableWithIndexerEtc_StartIndex = input.Index();
        object root_0 = null;

        IToken DOT40 = null;
        GAMSParser.variable_return variable39 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable41 = default(GAMSParser.variable_return);

        GAMSParser.idx_return idx42 = default(GAMSParser.idx_return);

        GAMSParser.conditional_return conditional43 = default(GAMSParser.conditional_return);


        object DOT40_tree=null;
        RewriteRuleTokenStream stream_DOT = new RewriteRuleTokenStream(adaptor,"token DOT");
        RewriteRuleSubtreeStream stream_idx = new RewriteRuleSubtreeStream(adaptor,"rule idx");
        RewriteRuleSubtreeStream stream_conditional = new RewriteRuleSubtreeStream(adaptor,"rule conditional");
        RewriteRuleSubtreeStream stream_variable = new RewriteRuleSubtreeStream(adaptor,"rule variable");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 11) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:224:23: ( variable ( DOT variable )? ( idx )? ( conditional )? -> ^( ASTVARWI ^( ASTVARWI0 ( DOT )? ) ^( ASTVARWI1 variable ) ^( ASTVARWI2 ( variable )? ) ^( ASTVARWI3 ( idx )? ) ^( ASTVARWI4 ( conditional )? ) ) )
            // GAMS.g:224:25: variable ( DOT variable )? ( idx )? ( conditional )?
            {
            	PushFollow(FOLLOW_variable_in_variableWithIndexerEtc776);
            	variable39 = variable();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variable.Add(variable39.Tree);
            	// GAMS.g:224:34: ( DOT variable )?
            	int alt8 = 2;
            	alt8 = dfa8.Predict(input);
            	switch (alt8) 
            	{
            	    case 1 :
            	        // GAMS.g:224:35: DOT variable
            	        {
            	        	DOT40=(IToken)Match(input,DOT,FOLLOW_DOT_in_variableWithIndexerEtc779); if (state.failed) return retval; 
            	        	if ( (state.backtracking==0) ) stream_DOT.Add(DOT40);

            	        	PushFollow(FOLLOW_variable_in_variableWithIndexerEtc781);
            	        	variable41 = variable();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_variable.Add(variable41.Tree);

            	        }
            	        break;

            	}

            	// GAMS.g:224:50: ( idx )?
            	int alt9 = 2;
            	alt9 = dfa9.Predict(input);
            	switch (alt9) 
            	{
            	    case 1 :
            	        // GAMS.g:0:0: idx
            	        {
            	        	PushFollow(FOLLOW_idx_in_variableWithIndexerEtc785);
            	        	idx42 = idx();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_idx.Add(idx42.Tree);

            	        }
            	        break;

            	}

            	// GAMS.g:224:55: ( conditional )?
            	int alt10 = 2;
            	alt10 = dfa10.Predict(input);
            	switch (alt10) 
            	{
            	    case 1 :
            	        // GAMS.g:0:0: conditional
            	        {
            	        	PushFollow(FOLLOW_conditional_in_variableWithIndexerEtc788);
            	        	conditional43 = conditional();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional43.Tree);

            	        }
            	        break;

            	}



            	// AST REWRITE
            	// elements:          idx, variable, variable, conditional, DOT
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 225:3: -> ^( ASTVARWI ^( ASTVARWI0 ( DOT )? ) ^( ASTVARWI1 variable ) ^( ASTVARWI2 ( variable )? ) ^( ASTVARWI3 ( idx )? ) ^( ASTVARWI4 ( conditional )? ) )
            	{
            	    // GAMS.g:225:6: ^( ASTVARWI ^( ASTVARWI0 ( DOT )? ) ^( ASTVARWI1 variable ) ^( ASTVARWI2 ( variable )? ) ^( ASTVARWI3 ( idx )? ) ^( ASTVARWI4 ( conditional )? ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI, "ASTVARWI"), root_1);

            	    // GAMS.g:225:17: ^( ASTVARWI0 ( DOT )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI0, "ASTVARWI0"), root_2);

            	    // GAMS.g:225:29: ( DOT )?
            	    if ( stream_DOT.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_DOT.NextNode());

            	    }
            	    stream_DOT.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:225:35: ^( ASTVARWI1 variable )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI1, "ASTVARWI1"), root_2);

            	    adaptor.AddChild(root_2, stream_variable.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:225:57: ^( ASTVARWI2 ( variable )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI2, "ASTVARWI2"), root_2);

            	    // GAMS.g:225:69: ( variable )?
            	    if ( stream_variable.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_variable.NextTree());

            	    }
            	    stream_variable.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:225:80: ^( ASTVARWI3 ( idx )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI3, "ASTVARWI3"), root_2);

            	    // GAMS.g:225:92: ( idx )?
            	    if ( stream_idx.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_idx.NextTree());

            	    }
            	    stream_idx.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:225:98: ^( ASTVARWI4 ( conditional )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI4, "ASTVARWI4"), root_2);

            	    // GAMS.g:225:110: ( conditional )?
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
            	Memoize(input, 11, variableWithIndexerEtc_StartIndex); 
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
    // GAMS.g:227:1: variable : ident ;
    public GAMSParser.variable_return variable() // throws RecognitionException [1]
    {   
        GAMSParser.variable_return retval = new GAMSParser.variable_return();
        retval.Start = input.LT(1);
        int variable_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.ident_return ident44 = default(GAMSParser.ident_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 12) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:227:10: ( ident )
            // GAMS.g:227:12: ident
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_ident_in_variable839);
            	ident44 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, ident44.Tree);

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
            	Memoize(input, 12, variable_StartIndex); 
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
    // GAMS.g:229:1: idx : ( L1 indexerElements R1 -> ^( ASTIDX ^( ASTIDX0 L1 R1 ) indexerElements ) | L2 indexerElements R2 -> ^( ASTIDX ^( ASTIDX0 L2 R2 ) indexerElements ) | L3 indexerElements R3 -> ^( ASTIDX ^( ASTIDX0 L3 R3 ) indexerElements ) );
    public GAMSParser.idx_return idx() // throws RecognitionException [1]
    {   
        GAMSParser.idx_return retval = new GAMSParser.idx_return();
        retval.Start = input.LT(1);
        int idx_StartIndex = input.Index();
        object root_0 = null;

        IToken L145 = null;
        IToken R147 = null;
        IToken L248 = null;
        IToken R250 = null;
        IToken L351 = null;
        IToken R353 = null;
        GAMSParser.indexerElements_return indexerElements46 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements49 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements52 = default(GAMSParser.indexerElements_return);


        object L145_tree=null;
        object R147_tree=null;
        object L248_tree=null;
        object R250_tree=null;
        object L351_tree=null;
        object R353_tree=null;
        RewriteRuleTokenStream stream_L1 = new RewriteRuleTokenStream(adaptor,"token L1");
        RewriteRuleTokenStream stream_L2 = new RewriteRuleTokenStream(adaptor,"token L2");
        RewriteRuleTokenStream stream_L3 = new RewriteRuleTokenStream(adaptor,"token L3");
        RewriteRuleTokenStream stream_R2 = new RewriteRuleTokenStream(adaptor,"token R2");
        RewriteRuleTokenStream stream_R3 = new RewriteRuleTokenStream(adaptor,"token R3");
        RewriteRuleTokenStream stream_R1 = new RewriteRuleTokenStream(adaptor,"token R1");
        RewriteRuleSubtreeStream stream_indexerElements = new RewriteRuleSubtreeStream(adaptor,"rule indexerElements");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 13) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:229:4: ( L1 indexerElements R1 -> ^( ASTIDX ^( ASTIDX0 L1 R1 ) indexerElements ) | L2 indexerElements R2 -> ^( ASTIDX ^( ASTIDX0 L2 R2 ) indexerElements ) | L3 indexerElements R3 -> ^( ASTIDX ^( ASTIDX0 L3 R3 ) indexerElements ) )
            int alt11 = 3;
            switch ( input.LA(1) ) 
            {
            case L1:
            	{
                alt11 = 1;
                }
                break;
            case L2:
            	{
                alt11 = 2;
                }
                break;
            case L3:
            	{
                alt11 = 3;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d11s0 =
            	        new NoViableAltException("", 11, 0, input);

            	    throw nvae_d11s0;
            }

            switch (alt11) 
            {
                case 1 :
                    // GAMS.g:229:6: L1 indexerElements R1
                    {
                    	L145=(IToken)Match(input,L1,FOLLOW_L1_in_idx846); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L145);

                    	PushFollow(FOLLOW_indexerElements_in_idx848);
                    	indexerElements46 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements46.Tree);
                    	R147=(IToken)Match(input,R1,FOLLOW_R1_in_idx850); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R147);



                    	// AST REWRITE
                    	// elements:          R1, indexerElements, L1
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 229:28: -> ^( ASTIDX ^( ASTIDX0 L1 R1 ) indexerElements )
                    	{
                    	    // GAMS.g:229:31: ^( ASTIDX ^( ASTIDX0 L1 R1 ) indexerElements )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDX, "ASTIDX"), root_1);

                    	    // GAMS.g:229:40: ^( ASTIDX0 L1 R1 )
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
                    // GAMS.g:230:6: L2 indexerElements R2
                    {
                    	L248=(IToken)Match(input,L2,FOLLOW_L2_in_idx873); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L248);

                    	PushFollow(FOLLOW_indexerElements_in_idx875);
                    	indexerElements49 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements49.Tree);
                    	R250=(IToken)Match(input,R2,FOLLOW_R2_in_idx877); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R250);



                    	// AST REWRITE
                    	// elements:          R2, indexerElements, L2
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 230:28: -> ^( ASTIDX ^( ASTIDX0 L2 R2 ) indexerElements )
                    	{
                    	    // GAMS.g:230:31: ^( ASTIDX ^( ASTIDX0 L2 R2 ) indexerElements )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDX, "ASTIDX"), root_1);

                    	    // GAMS.g:230:40: ^( ASTIDX0 L2 R2 )
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
                    // GAMS.g:231:6: L3 indexerElements R3
                    {
                    	L351=(IToken)Match(input,L3,FOLLOW_L3_in_idx900); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L351);

                    	PushFollow(FOLLOW_indexerElements_in_idx902);
                    	indexerElements52 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements52.Tree);
                    	R353=(IToken)Match(input,R3,FOLLOW_R3_in_idx904); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R353);



                    	// AST REWRITE
                    	// elements:          R3, indexerElements, L3
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 231:28: -> ^( ASTIDX ^( ASTIDX0 L3 R3 ) indexerElements )
                    	{
                    	    // GAMS.g:231:31: ^( ASTIDX ^( ASTIDX0 L3 R3 ) indexerElements )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDX, "ASTIDX"), root_1);

                    	    // GAMS.g:231:40: ^( ASTIDX0 L3 R3 )
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
            	Memoize(input, 13, idx_StartIndex); 
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
    // GAMS.g:234:1: indexerElements : variableLagLead ( COMMA variableLagLead )* -> ^( ASTIDXELEMENTS ^( ASTIDXELEMENTS0 ( COMMA )* ) ^( ASTIDXELEMENTS1 ( variableLagLead )+ ) ) ;
    public GAMSParser.indexerElements_return indexerElements() // throws RecognitionException [1]
    {   
        GAMSParser.indexerElements_return retval = new GAMSParser.indexerElements_return();
        retval.Start = input.LT(1);
        int indexerElements_StartIndex = input.Index();
        object root_0 = null;

        IToken COMMA55 = null;
        GAMSParser.variableLagLead_return variableLagLead54 = default(GAMSParser.variableLagLead_return);

        GAMSParser.variableLagLead_return variableLagLead56 = default(GAMSParser.variableLagLead_return);


        object COMMA55_tree=null;
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleSubtreeStream stream_variableLagLead = new RewriteRuleSubtreeStream(adaptor,"rule variableLagLead");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 14) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:234:16: ( variableLagLead ( COMMA variableLagLead )* -> ^( ASTIDXELEMENTS ^( ASTIDXELEMENTS0 ( COMMA )* ) ^( ASTIDXELEMENTS1 ( variableLagLead )+ ) ) )
            // GAMS.g:234:18: variableLagLead ( COMMA variableLagLead )*
            {
            	PushFollow(FOLLOW_variableLagLead_in_indexerElements928);
            	variableLagLead54 = variableLagLead();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableLagLead.Add(variableLagLead54.Tree);
            	// GAMS.g:234:34: ( COMMA variableLagLead )*
            	do 
            	{
            	    int alt12 = 2;
            	    int LA12_0 = input.LA(1);

            	    if ( (LA12_0 == COMMA) )
            	    {
            	        alt12 = 1;
            	    }


            	    switch (alt12) 
            		{
            			case 1 :
            			    // GAMS.g:234:35: COMMA variableLagLead
            			    {
            			    	COMMA55=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_indexerElements931); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA55);

            			    	PushFollow(FOLLOW_variableLagLead_in_indexerElements933);
            			    	variableLagLead56 = variableLagLead();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_variableLagLead.Add(variableLagLead56.Tree);

            			    }
            			    break;

            			default:
            			    goto loop12;
            	    }
            	} while (true);

            	loop12:
            		;	// Stops C# compiler whining that label 'loop12' has no statements



            	// AST REWRITE
            	// elements:          COMMA, variableLagLead
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 235:3: -> ^( ASTIDXELEMENTS ^( ASTIDXELEMENTS0 ( COMMA )* ) ^( ASTIDXELEMENTS1 ( variableLagLead )+ ) )
            	{
            	    // GAMS.g:235:6: ^( ASTIDXELEMENTS ^( ASTIDXELEMENTS0 ( COMMA )* ) ^( ASTIDXELEMENTS1 ( variableLagLead )+ ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDXELEMENTS, "ASTIDXELEMENTS"), root_1);

            	    // GAMS.g:235:23: ^( ASTIDXELEMENTS0 ( COMMA )* )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDXELEMENTS0, "ASTIDXELEMENTS0"), root_2);

            	    // GAMS.g:235:41: ( COMMA )*
            	    while ( stream_COMMA.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_COMMA.NextNode());

            	    }
            	    stream_COMMA.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:235:49: ^( ASTIDXELEMENTS1 ( variableLagLead )+ )
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
            	Memoize(input, 14, indexerElements_StartIndex); 
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
    // GAMS.g:237:1: variableLagLead : ( StringInQuotes -> ^( ASTVARIABLEANDLEAD StringInQuotes ) | variable PLUS Integer -> ^( ASTVARIABLEANDLEAD variable PLUS Integer ) | variable MINUS Integer -> ^( ASTVARIABLEANDLEAD variable MINUS Integer ) | variable -> ^( ASTVARIABLEANDLEAD variable ) );
    public GAMSParser.variableLagLead_return variableLagLead() // throws RecognitionException [1]
    {   
        GAMSParser.variableLagLead_return retval = new GAMSParser.variableLagLead_return();
        retval.Start = input.LT(1);
        int variableLagLead_StartIndex = input.Index();
        object root_0 = null;

        IToken StringInQuotes57 = null;
        IToken PLUS59 = null;
        IToken Integer60 = null;
        IToken MINUS62 = null;
        IToken Integer63 = null;
        GAMSParser.variable_return variable58 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable61 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable64 = default(GAMSParser.variable_return);


        object StringInQuotes57_tree=null;
        object PLUS59_tree=null;
        object Integer60_tree=null;
        object MINUS62_tree=null;
        object Integer63_tree=null;
        RewriteRuleTokenStream stream_StringInQuotes = new RewriteRuleTokenStream(adaptor,"token StringInQuotes");
        RewriteRuleTokenStream stream_PLUS = new RewriteRuleTokenStream(adaptor,"token PLUS");
        RewriteRuleTokenStream stream_MINUS = new RewriteRuleTokenStream(adaptor,"token MINUS");
        RewriteRuleTokenStream stream_Integer = new RewriteRuleTokenStream(adaptor,"token Integer");
        RewriteRuleSubtreeStream stream_variable = new RewriteRuleSubtreeStream(adaptor,"rule variable");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 15) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:237:16: ( StringInQuotes -> ^( ASTVARIABLEANDLEAD StringInQuotes ) | variable PLUS Integer -> ^( ASTVARIABLEANDLEAD variable PLUS Integer ) | variable MINUS Integer -> ^( ASTVARIABLEANDLEAD variable MINUS Integer ) | variable -> ^( ASTVARIABLEANDLEAD variable ) )
            int alt13 = 4;
            alt13 = dfa13.Predict(input);
            switch (alt13) 
            {
                case 1 :
                    // GAMS.g:237:18: StringInQuotes
                    {
                    	StringInQuotes57=(IToken)Match(input,StringInQuotes,FOLLOW_StringInQuotes_in_variableLagLead965); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_StringInQuotes.Add(StringInQuotes57);



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
                    	// 237:43: -> ^( ASTVARIABLEANDLEAD StringInQuotes )
                    	{
                    	    // GAMS.g:237:46: ^( ASTVARIABLEANDLEAD StringInQuotes )
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
                    // GAMS.g:238:18: variable PLUS Integer
                    {
                    	PushFollow(FOLLOW_variable_in_variableLagLead1014);
                    	variable58 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variable.Add(variable58.Tree);
                    	PLUS59=(IToken)Match(input,PLUS,FOLLOW_PLUS_in_variableLagLead1016); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_PLUS.Add(PLUS59);

                    	Integer60=(IToken)Match(input,Integer,FOLLOW_Integer_in_variableLagLead1018); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer60);



                    	// AST REWRITE
                    	// elements:          PLUS, Integer, variable
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 238:43: -> ^( ASTVARIABLEANDLEAD variable PLUS Integer )
                    	{
                    	    // GAMS.g:238:46: ^( ASTVARIABLEANDLEAD variable PLUS Integer )
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
                    // GAMS.g:239:18: variable MINUS Integer
                    {
                    	PushFollow(FOLLOW_variable_in_variableLagLead1064);
                    	variable61 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variable.Add(variable61.Tree);
                    	MINUS62=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_variableLagLead1066); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_MINUS.Add(MINUS62);

                    	Integer63=(IToken)Match(input,Integer,FOLLOW_Integer_in_variableLagLead1068); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer63);



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
                    	// 239:43: -> ^( ASTVARIABLEANDLEAD variable MINUS Integer )
                    	{
                    	    // GAMS.g:239:46: ^( ASTVARIABLEANDLEAD variable MINUS Integer )
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
                    // GAMS.g:240:9: variable
                    {
                    	PushFollow(FOLLOW_variable_in_variableLagLead1104);
                    	variable64 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variable.Add(variable64.Tree);


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
                    	// 240:34: -> ^( ASTVARIABLEANDLEAD variable )
                    	{
                    	    // GAMS.g:240:37: ^( ASTVARIABLEANDLEAD variable )
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
            	Memoize(input, 15, variableLagLead_StartIndex); 
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
    // GAMS.g:243:1: conditional : DOLLAR expression -> ^( ASTCONDITIONAL DOLLAR expression ) ;
    public GAMSParser.conditional_return conditional() // throws RecognitionException [1]
    {   
        GAMSParser.conditional_return retval = new GAMSParser.conditional_return();
        retval.Start = input.LT(1);
        int conditional_StartIndex = input.Index();
        object root_0 = null;

        IToken DOLLAR65 = null;
        GAMSParser.expression_return expression66 = default(GAMSParser.expression_return);


        object DOLLAR65_tree=null;
        RewriteRuleTokenStream stream_DOLLAR = new RewriteRuleTokenStream(adaptor,"token DOLLAR");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 16) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:243:12: ( DOLLAR expression -> ^( ASTCONDITIONAL DOLLAR expression ) )
            // GAMS.g:243:14: DOLLAR expression
            {
            	DOLLAR65=(IToken)Match(input,DOLLAR,FOLLOW_DOLLAR_in_conditional1148); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_DOLLAR.Add(DOLLAR65);

            	PushFollow(FOLLOW_expression_in_conditional1150);
            	expression66 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression66.Tree);


            	// AST REWRITE
            	// elements:          expression, DOLLAR
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 243:32: -> ^( ASTCONDITIONAL DOLLAR expression )
            	{
            	    // GAMS.g:243:35: ^( ASTCONDITIONAL DOLLAR expression )
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
            	Memoize(input, 16, conditional_StartIndex); 
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
    // GAMS.g:261:1: expression : andExpression ( OR andExpression )* ;
    public GAMSParser.expression_return expression() // throws RecognitionException [1]
    {   
        GAMSParser.expression_return retval = new GAMSParser.expression_return();
        retval.Start = input.LT(1);
        int expression_StartIndex = input.Index();
        object root_0 = null;

        IToken OR68 = null;
        GAMSParser.andExpression_return andExpression67 = default(GAMSParser.andExpression_return);

        GAMSParser.andExpression_return andExpression69 = default(GAMSParser.andExpression_return);


        object OR68_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 17) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:261:11: ( andExpression ( OR andExpression )* )
            // GAMS.g:261:13: andExpression ( OR andExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_andExpression_in_expression1183);
            	andExpression67 = andExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, andExpression67.Tree);
            	// GAMS.g:261:27: ( OR andExpression )*
            	do 
            	{
            	    int alt14 = 2;
            	    alt14 = dfa14.Predict(input);
            	    switch (alt14) 
            		{
            			case 1 :
            			    // GAMS.g:261:28: OR andExpression
            			    {
            			    	OR68=(IToken)Match(input,OR,FOLLOW_OR_in_expression1186); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{OR68_tree = (object)adaptor.Create(OR68);
            			    		root_0 = (object)adaptor.BecomeRoot(OR68_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_andExpression_in_expression1189);
            			    	andExpression69 = andExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, andExpression69.Tree);

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
            	Memoize(input, 17, expression_StartIndex); 
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
    // GAMS.g:263:1: andExpression : notExpression ( AND notExpression )* ;
    public GAMSParser.andExpression_return andExpression() // throws RecognitionException [1]
    {   
        GAMSParser.andExpression_return retval = new GAMSParser.andExpression_return();
        retval.Start = input.LT(1);
        int andExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken AND71 = null;
        GAMSParser.notExpression_return notExpression70 = default(GAMSParser.notExpression_return);

        GAMSParser.notExpression_return notExpression72 = default(GAMSParser.notExpression_return);


        object AND71_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 18) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:263:14: ( notExpression ( AND notExpression )* )
            // GAMS.g:263:16: notExpression ( AND notExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_notExpression_in_andExpression1198);
            	notExpression70 = notExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, notExpression70.Tree);
            	// GAMS.g:263:30: ( AND notExpression )*
            	do 
            	{
            	    int alt15 = 2;
            	    alt15 = dfa15.Predict(input);
            	    switch (alt15) 
            		{
            			case 1 :
            			    // GAMS.g:263:31: AND notExpression
            			    {
            			    	AND71=(IToken)Match(input,AND,FOLLOW_AND_in_andExpression1201); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{AND71_tree = (object)adaptor.Create(AND71);
            			    		root_0 = (object)adaptor.BecomeRoot(AND71_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_notExpression_in_andExpression1204);
            			    	notExpression72 = notExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, notExpression72.Tree);

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
            	Memoize(input, 18, andExpression_StartIndex); 
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
    // GAMS.g:265:1: notExpression : ( NOT logicalExpression -> ^( NOT logicalExpression ) | logicalExpression );
    public GAMSParser.notExpression_return notExpression() // throws RecognitionException [1]
    {   
        GAMSParser.notExpression_return retval = new GAMSParser.notExpression_return();
        retval.Start = input.LT(1);
        int notExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken NOT73 = null;
        GAMSParser.logicalExpression_return logicalExpression74 = default(GAMSParser.logicalExpression_return);

        GAMSParser.logicalExpression_return logicalExpression75 = default(GAMSParser.logicalExpression_return);


        object NOT73_tree=null;
        RewriteRuleTokenStream stream_NOT = new RewriteRuleTokenStream(adaptor,"token NOT");
        RewriteRuleSubtreeStream stream_logicalExpression = new RewriteRuleSubtreeStream(adaptor,"rule logicalExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 19) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:265:14: ( NOT logicalExpression -> ^( NOT logicalExpression ) | logicalExpression )
            int alt16 = 2;
            alt16 = dfa16.Predict(input);
            switch (alt16) 
            {
                case 1 :
                    // GAMS.g:265:16: NOT logicalExpression
                    {
                    	NOT73=(IToken)Match(input,NOT,FOLLOW_NOT_in_notExpression1215); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_NOT.Add(NOT73);

                    	PushFollow(FOLLOW_logicalExpression_in_notExpression1217);
                    	logicalExpression74 = logicalExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_logicalExpression.Add(logicalExpression74.Tree);


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
                    	// 265:38: -> ^( NOT logicalExpression )
                    	{
                    	    // GAMS.g:265:41: ^( NOT logicalExpression )
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
                    // GAMS.g:266:10: logicalExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_logicalExpression_in_notExpression1237);
                    	logicalExpression75 = logicalExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, logicalExpression75.Tree);

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
            	Memoize(input, 19, notExpression_StartIndex); 
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
    // GAMS.g:268:1: logicalExpression : additiveExpression ( logical additiveExpression )* ;
    public GAMSParser.logicalExpression_return logicalExpression() // throws RecognitionException [1]
    {   
        GAMSParser.logicalExpression_return retval = new GAMSParser.logicalExpression_return();
        retval.Start = input.LT(1);
        int logicalExpression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.additiveExpression_return additiveExpression76 = default(GAMSParser.additiveExpression_return);

        GAMSParser.logical_return logical77 = default(GAMSParser.logical_return);

        GAMSParser.additiveExpression_return additiveExpression78 = default(GAMSParser.additiveExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 20) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:268:18: ( additiveExpression ( logical additiveExpression )* )
            // GAMS.g:268:21: additiveExpression ( logical additiveExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_additiveExpression_in_logicalExpression1245);
            	additiveExpression76 = additiveExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression76.Tree);
            	// GAMS.g:268:40: ( logical additiveExpression )*
            	do 
            	{
            	    int alt17 = 2;
            	    alt17 = dfa17.Predict(input);
            	    switch (alt17) 
            		{
            			case 1 :
            			    // GAMS.g:268:41: logical additiveExpression
            			    {
            			    	PushFollow(FOLLOW_logical_in_logicalExpression1248);
            			    	logical77 = logical();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot(logical77.Tree, root_0);
            			    	PushFollow(FOLLOW_additiveExpression_in_logicalExpression1251);
            			    	additiveExpression78 = additiveExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression78.Tree);

            			    }
            			    break;

            			default:
            			    goto loop17;
            	    }
            	} while (true);

            	loop17:
            		;	// Stops C# compiler whining that label 'loop17' has no statements


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
            	Memoize(input, 20, logicalExpression_StartIndex); 
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
    // GAMS.g:270:1: additiveExpression : multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* ;
    public GAMSParser.additiveExpression_return additiveExpression() // throws RecognitionException [1]
    {   
        GAMSParser.additiveExpression_return retval = new GAMSParser.additiveExpression_return();
        retval.Start = input.LT(1);
        int additiveExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set80 = null;
        GAMSParser.multiplicativeExpression_return multiplicativeExpression79 = default(GAMSParser.multiplicativeExpression_return);

        GAMSParser.multiplicativeExpression_return multiplicativeExpression81 = default(GAMSParser.multiplicativeExpression_return);


        object set80_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 21) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:270:19: ( multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* )
            // GAMS.g:270:21: multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression1260);
            	multiplicativeExpression79 = multiplicativeExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression79.Tree);
            	// GAMS.g:270:46: ( ( PLUS | MINUS ) multiplicativeExpression )*
            	do 
            	{
            	    int alt18 = 2;
            	    alt18 = dfa18.Predict(input);
            	    switch (alt18) 
            		{
            			case 1 :
            			    // GAMS.g:270:48: ( PLUS | MINUS ) multiplicativeExpression
            			    {
            			    	set80=(IToken)input.LT(1);
            			    	set80 = (IToken)input.LT(1);
            			    	if ( input.LA(1) == PLUS || input.LA(1) == MINUS ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set80), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression1271);
            			    	multiplicativeExpression81 = multiplicativeExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression81.Tree);

            			    }
            			    break;

            			default:
            			    goto loop18;
            	    }
            	} while (true);

            	loop18:
            		;	// Stops C# compiler whining that label 'loop18' has no statements


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
            	Memoize(input, 21, additiveExpression_StartIndex); 
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
    // GAMS.g:272:1: multiplicativeExpression : powerExpression ( ( MULT | DIV ) powerExpression )* ;
    public GAMSParser.multiplicativeExpression_return multiplicativeExpression() // throws RecognitionException [1]
    {   
        GAMSParser.multiplicativeExpression_return retval = new GAMSParser.multiplicativeExpression_return();
        retval.Start = input.LT(1);
        int multiplicativeExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set83 = null;
        GAMSParser.powerExpression_return powerExpression82 = default(GAMSParser.powerExpression_return);

        GAMSParser.powerExpression_return powerExpression84 = default(GAMSParser.powerExpression_return);


        object set83_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 22) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:272:25: ( powerExpression ( ( MULT | DIV ) powerExpression )* )
            // GAMS.g:272:27: powerExpression ( ( MULT | DIV ) powerExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression1281);
            	powerExpression82 = powerExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression82.Tree);
            	// GAMS.g:272:43: ( ( MULT | DIV ) powerExpression )*
            	do 
            	{
            	    int alt19 = 2;
            	    alt19 = dfa19.Predict(input);
            	    switch (alt19) 
            		{
            			case 1 :
            			    // GAMS.g:272:45: ( MULT | DIV ) powerExpression
            			    {
            			    	set83=(IToken)input.LT(1);
            			    	set83 = (IToken)input.LT(1);
            			    	if ( input.LA(1) == DIV || input.LA(1) == MULT ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set83), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression1292);
            			    	powerExpression84 = powerExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression84.Tree);

            			    }
            			    break;

            			default:
            			    goto loop19;
            	    }
            	} while (true);

            	loop19:
            		;	// Stops C# compiler whining that label 'loop19' has no statements


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
            	Memoize(input, 22, multiplicativeExpression_StartIndex); 
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
    // GAMS.g:274:1: powerExpression : unaryExpression ( STARS unaryExpression )* ;
    public GAMSParser.powerExpression_return powerExpression() // throws RecognitionException [1]
    {   
        GAMSParser.powerExpression_return retval = new GAMSParser.powerExpression_return();
        retval.Start = input.LT(1);
        int powerExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken STARS86 = null;
        GAMSParser.unaryExpression_return unaryExpression85 = default(GAMSParser.unaryExpression_return);

        GAMSParser.unaryExpression_return unaryExpression87 = default(GAMSParser.unaryExpression_return);


        object STARS86_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 23) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:274:16: ( unaryExpression ( STARS unaryExpression )* )
            // GAMS.g:274:18: unaryExpression ( STARS unaryExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_unaryExpression_in_powerExpression1302);
            	unaryExpression85 = unaryExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression85.Tree);
            	// GAMS.g:274:34: ( STARS unaryExpression )*
            	do 
            	{
            	    int alt20 = 2;
            	    alt20 = dfa20.Predict(input);
            	    switch (alt20) 
            		{
            			case 1 :
            			    // GAMS.g:274:36: STARS unaryExpression
            			    {
            			    	STARS86=(IToken)Match(input,STARS,FOLLOW_STARS_in_powerExpression1306); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{STARS86_tree = (object)adaptor.Create(STARS86);
            			    		root_0 = (object)adaptor.BecomeRoot(STARS86_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_unaryExpression_in_powerExpression1309);
            			    	unaryExpression87 = unaryExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression87.Tree);

            			    }
            			    break;

            			default:
            			    goto loop20;
            	    }
            	} while (true);

            	loop20:
            		;	// Stops C# compiler whining that label 'loop20' has no statements


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
            	Memoize(input, 23, powerExpression_StartIndex); 
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
    // GAMS.g:276:1: unaryExpression : ( MINUS dollarExpression -> ^( NEGATE dollarExpression ) | dollarExpression );
    public GAMSParser.unaryExpression_return unaryExpression() // throws RecognitionException [1]
    {   
        GAMSParser.unaryExpression_return retval = new GAMSParser.unaryExpression_return();
        retval.Start = input.LT(1);
        int unaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken MINUS88 = null;
        GAMSParser.dollarExpression_return dollarExpression89 = default(GAMSParser.dollarExpression_return);

        GAMSParser.dollarExpression_return dollarExpression90 = default(GAMSParser.dollarExpression_return);


        object MINUS88_tree=null;
        RewriteRuleTokenStream stream_MINUS = new RewriteRuleTokenStream(adaptor,"token MINUS");
        RewriteRuleSubtreeStream stream_dollarExpression = new RewriteRuleSubtreeStream(adaptor,"rule dollarExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 24) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:276:16: ( MINUS dollarExpression -> ^( NEGATE dollarExpression ) | dollarExpression )
            int alt21 = 2;
            alt21 = dfa21.Predict(input);
            switch (alt21) 
            {
                case 1 :
                    // GAMS.g:276:18: MINUS dollarExpression
                    {
                    	MINUS88=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_unaryExpression1320); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_MINUS.Add(MINUS88);

                    	PushFollow(FOLLOW_dollarExpression_in_unaryExpression1322);
                    	dollarExpression89 = dollarExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_dollarExpression.Add(dollarExpression89.Tree);


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
                    	// 276:41: -> ^( NEGATE dollarExpression )
                    	{
                    	    // GAMS.g:276:44: ^( NEGATE dollarExpression )
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
                    // GAMS.g:277:11: dollarExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_dollarExpression_in_unaryExpression1343);
                    	dollarExpression90 = dollarExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, dollarExpression90.Tree);

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
            	Memoize(input, 24, unaryExpression_StartIndex); 
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
    // GAMS.g:285:1: dollarExpression : ( primaryExpression conditional -> ^( ASTDOLLAREXPRESSION primaryExpression conditional ) | primaryExpression );
    public GAMSParser.dollarExpression_return dollarExpression() // throws RecognitionException [1]
    {   
        GAMSParser.dollarExpression_return retval = new GAMSParser.dollarExpression_return();
        retval.Start = input.LT(1);
        int dollarExpression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.primaryExpression_return primaryExpression91 = default(GAMSParser.primaryExpression_return);

        GAMSParser.conditional_return conditional92 = default(GAMSParser.conditional_return);

        GAMSParser.primaryExpression_return primaryExpression93 = default(GAMSParser.primaryExpression_return);


        RewriteRuleSubtreeStream stream_conditional = new RewriteRuleSubtreeStream(adaptor,"rule conditional");
        RewriteRuleSubtreeStream stream_primaryExpression = new RewriteRuleSubtreeStream(adaptor,"rule primaryExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 25) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:285:17: ( primaryExpression conditional -> ^( ASTDOLLAREXPRESSION primaryExpression conditional ) | primaryExpression )
            int alt22 = 2;
            alt22 = dfa22.Predict(input);
            switch (alt22) 
            {
                case 1 :
                    // GAMS.g:286:9: primaryExpression conditional
                    {
                    	PushFollow(FOLLOW_primaryExpression_in_dollarExpression1368);
                    	primaryExpression91 = primaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_primaryExpression.Add(primaryExpression91.Tree);
                    	PushFollow(FOLLOW_conditional_in_dollarExpression1370);
                    	conditional92 = conditional();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_conditional.Add(conditional92.Tree);


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
                    	// 286:39: -> ^( ASTDOLLAREXPRESSION primaryExpression conditional )
                    	{
                    	    // GAMS.g:286:42: ^( ASTDOLLAREXPRESSION primaryExpression conditional )
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
                    // GAMS.g:287:9: primaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_primaryExpression_in_dollarExpression1390);
                    	primaryExpression93 = primaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, primaryExpression93.Tree);

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
            	Memoize(input, 25, dollarExpression_StartIndex); 
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
    // GAMS.g:290:1: primaryExpression : ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value );
    public GAMSParser.primaryExpression_return primaryExpression() // throws RecognitionException [1]
    {   
        GAMSParser.primaryExpression_return retval = new GAMSParser.primaryExpression_return();
        retval.Start = input.LT(1);
        int primaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken L194 = null;
        IToken R196 = null;
        IToken L297 = null;
        IToken R299 = null;
        IToken L3100 = null;
        IToken R3102 = null;
        GAMSParser.expression_return expression95 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression98 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression101 = default(GAMSParser.expression_return);

        GAMSParser.value_return value103 = default(GAMSParser.value_return);


        object L194_tree=null;
        object R196_tree=null;
        object L297_tree=null;
        object R299_tree=null;
        object L3100_tree=null;
        object R3102_tree=null;
        RewriteRuleTokenStream stream_L1 = new RewriteRuleTokenStream(adaptor,"token L1");
        RewriteRuleTokenStream stream_L2 = new RewriteRuleTokenStream(adaptor,"token L2");
        RewriteRuleTokenStream stream_L3 = new RewriteRuleTokenStream(adaptor,"token L3");
        RewriteRuleTokenStream stream_R2 = new RewriteRuleTokenStream(adaptor,"token R2");
        RewriteRuleTokenStream stream_R3 = new RewriteRuleTokenStream(adaptor,"token R3");
        RewriteRuleTokenStream stream_R1 = new RewriteRuleTokenStream(adaptor,"token R1");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 26) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:290:18: ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value )
            int alt23 = 4;
            alt23 = dfa23.Predict(input);
            switch (alt23) 
            {
                case 1 :
                    // GAMS.g:291:5: L1 expression R1
                    {
                    	L194=(IToken)Match(input,L1,FOLLOW_L1_in_primaryExpression1411); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L194);

                    	PushFollow(FOLLOW_expression_in_primaryExpression1413);
                    	expression95 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression95.Tree);
                    	R196=(IToken)Match(input,R1,FOLLOW_R1_in_primaryExpression1415); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R196);



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
                    	// 291:22: -> ^( ASTEXPRESSION1 expression )
                    	{
                    	    // GAMS.g:291:25: ^( ASTEXPRESSION1 expression )
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
                    // GAMS.g:292:6: L2 expression R2
                    {
                    	L297=(IToken)Match(input,L2,FOLLOW_L2_in_primaryExpression1430); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L297);

                    	PushFollow(FOLLOW_expression_in_primaryExpression1432);
                    	expression98 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression98.Tree);
                    	R299=(IToken)Match(input,R2,FOLLOW_R2_in_primaryExpression1434); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R299);



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
                    	// 292:23: -> ^( ASTEXPRESSION2 expression )
                    	{
                    	    // GAMS.g:292:26: ^( ASTEXPRESSION2 expression )
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
                    // GAMS.g:293:8: L3 expression R3
                    {
                    	L3100=(IToken)Match(input,L3,FOLLOW_L3_in_primaryExpression1451); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L3100);

                    	PushFollow(FOLLOW_expression_in_primaryExpression1453);
                    	expression101 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression101.Tree);
                    	R3102=(IToken)Match(input,R3,FOLLOW_R3_in_primaryExpression1455); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R3102);



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
                    	// 293:25: -> ^( ASTEXPRESSION3 expression )
                    	{
                    	    // GAMS.g:293:28: ^( ASTEXPRESSION3 expression )
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
                    // GAMS.g:294:6: value
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_value_in_primaryExpression1470);
                    	value103 = value();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, value103.Tree);

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
            	Memoize(input, 26, primaryExpression_StartIndex); 
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
    // GAMS.g:296:1: logical : ( NONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL | EQUAL | LESSTHAN | GREATERTHAN );
    public GAMSParser.logical_return logical() // throws RecognitionException [1]
    {   
        GAMSParser.logical_return retval = new GAMSParser.logical_return();
        retval.Start = input.LT(1);
        int logical_StartIndex = input.Index();
        object root_0 = null;

        IToken set104 = null;

        object set104_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 27) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:296:8: ( NONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL | EQUAL | LESSTHAN | GREATERTHAN )
            // GAMS.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set104 = (IToken)input.LT(1);
            	if ( input.LA(1) == EQUAL || (input.LA(1) >= NONEQUAL && input.LA(1) <= GREATERTHAN) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set104));
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
            	Memoize(input, 27, logical_StartIndex); 
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
    // GAMS.g:298:1: value : ( Integer -> ^( ASTVALUE Integer ) | Double -> ^( ASTVALUE Double ) | sum -> ^( ASTVALUE sum ) | function -> ^( ASTVALUE function ) | variableWithIndexerEtc -> ^( ASTVALUE variableWithIndexerEtc ) );
    public GAMSParser.value_return value() // throws RecognitionException [1]
    {   
        GAMSParser.value_return retval = new GAMSParser.value_return();
        retval.Start = input.LT(1);
        int value_StartIndex = input.Index();
        object root_0 = null;

        IToken Integer105 = null;
        IToken Double106 = null;
        GAMSParser.sum_return sum107 = default(GAMSParser.sum_return);

        GAMSParser.function_return function108 = default(GAMSParser.function_return);

        GAMSParser.variableWithIndexerEtc_return variableWithIndexerEtc109 = default(GAMSParser.variableWithIndexerEtc_return);


        object Integer105_tree=null;
        object Double106_tree=null;
        RewriteRuleTokenStream stream_Double = new RewriteRuleTokenStream(adaptor,"token Double");
        RewriteRuleTokenStream stream_Integer = new RewriteRuleTokenStream(adaptor,"token Integer");
        RewriteRuleSubtreeStream stream_variableWithIndexerEtc = new RewriteRuleSubtreeStream(adaptor,"rule variableWithIndexerEtc");
        RewriteRuleSubtreeStream stream_sum = new RewriteRuleSubtreeStream(adaptor,"rule sum");
        RewriteRuleSubtreeStream stream_function = new RewriteRuleSubtreeStream(adaptor,"rule function");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 28) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:299:2: ( Integer -> ^( ASTVALUE Integer ) | Double -> ^( ASTVALUE Double ) | sum -> ^( ASTVALUE sum ) | function -> ^( ASTVALUE function ) | variableWithIndexerEtc -> ^( ASTVALUE variableWithIndexerEtc ) )
            int alt24 = 5;
            alt24 = dfa24.Predict(input);
            switch (alt24) 
            {
                case 1 :
                    // GAMS.g:299:5: Integer
                    {
                    	Integer105=(IToken)Match(input,Integer,FOLLOW_Integer_in_value1511); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer105);



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
                    	// 299:31: -> ^( ASTVALUE Integer )
                    	{
                    	    // GAMS.g:299:34: ^( ASTVALUE Integer )
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
                    // GAMS.g:300:4: Double
                    {
                    	Double106=(IToken)Match(input,Double,FOLLOW_Double_in_value1542); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Double.Add(Double106);



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
                    	// 300:30: -> ^( ASTVALUE Double )
                    	{
                    	    // GAMS.g:300:33: ^( ASTVALUE Double )
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
                    // GAMS.g:301:6: sum
                    {
                    	PushFollow(FOLLOW_sum_in_value1576);
                    	sum107 = sum();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sum.Add(sum107.Tree);


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
                    	// 301:32: -> ^( ASTVALUE sum )
                    	{
                    	    // GAMS.g:301:35: ^( ASTVALUE sum )
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
                    // GAMS.g:302:6: function
                    {
                    	PushFollow(FOLLOW_function_in_value1613);
                    	function108 = function();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_function.Add(function108.Tree);


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
                    	// 302:32: -> ^( ASTVALUE function )
                    	{
                    	    // GAMS.g:302:35: ^( ASTVALUE function )
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
                    // GAMS.g:303:4: variableWithIndexerEtc
                    {
                    	PushFollow(FOLLOW_variableWithIndexerEtc_in_value1661);
                    	variableWithIndexerEtc109 = variableWithIndexerEtc();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variableWithIndexerEtc.Add(variableWithIndexerEtc109.Tree);


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
                    	// 303:30: -> ^( ASTVALUE variableWithIndexerEtc )
                    	{
                    	    // GAMS.g:303:33: ^( ASTVALUE variableWithIndexerEtc )
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
            	Memoize(input, 28, value_StartIndex); 
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
    // GAMS.g:310:1: function : ( functionName L1 functionElements R1 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L1 R1 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) | functionName L2 functionElements R2 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L2 R2 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) | functionName L3 functionElements R3 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L3 R3 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) );
    public GAMSParser.function_return function() // throws RecognitionException [1]
    {   
        GAMSParser.function_return retval = new GAMSParser.function_return();
        retval.Start = input.LT(1);
        int function_StartIndex = input.Index();
        object root_0 = null;

        IToken L1111 = null;
        IToken R1113 = null;
        IToken L2115 = null;
        IToken R2117 = null;
        IToken L3119 = null;
        IToken R3121 = null;
        GAMSParser.functionName_return functionName110 = default(GAMSParser.functionName_return);

        GAMSParser.functionElements_return functionElements112 = default(GAMSParser.functionElements_return);

        GAMSParser.functionName_return functionName114 = default(GAMSParser.functionName_return);

        GAMSParser.functionElements_return functionElements116 = default(GAMSParser.functionElements_return);

        GAMSParser.functionName_return functionName118 = default(GAMSParser.functionName_return);

        GAMSParser.functionElements_return functionElements120 = default(GAMSParser.functionElements_return);


        object L1111_tree=null;
        object R1113_tree=null;
        object L2115_tree=null;
        object R2117_tree=null;
        object L3119_tree=null;
        object R3121_tree=null;
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
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 29) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:310:9: ( functionName L1 functionElements R1 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L1 R1 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) | functionName L2 functionElements R2 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L2 R2 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) | functionName L3 functionElements R3 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L3 R3 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) )
            int alt25 = 3;
            int LA25_0 = input.LA(1);

            if ( ((LA25_0 >= ABS && LA25_0 <= TANH) || LA25_0 == SAMEAS) )
            {
                switch ( input.LA(2) ) 
                {
                case L3:
                	{
                    alt25 = 3;
                    }
                    break;
                case L1:
                	{
                    alt25 = 1;
                    }
                    break;
                case L2:
                	{
                    alt25 = 2;
                    }
                    break;
                	default:
                	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                	    NoViableAltException nvae_d25s1 =
                	        new NoViableAltException("", 25, 1, input);

                	    throw nvae_d25s1;
                }

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
                    // GAMS.g:310:15: functionName L1 functionElements R1
                    {
                    	PushFollow(FOLLOW_functionName_in_function1693);
                    	functionName110 = functionName();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionName.Add(functionName110.Tree);
                    	L1111=(IToken)Match(input,L1,FOLLOW_L1_in_function1695); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L1111);

                    	PushFollow(FOLLOW_functionElements_in_function1697);
                    	functionElements112 = functionElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionElements.Add(functionElements112.Tree);
                    	R1113=(IToken)Match(input,R1,FOLLOW_R1_in_function1699); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R1113);



                    	// AST REWRITE
                    	// elements:          R1, L1, functionElements, functionName
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 310:51: -> ^( ASTFUNCTION ^( ASTFUNCTION0 L1 R1 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	{
                    	    // GAMS.g:310:54: ^( ASTFUNCTION ^( ASTFUNCTION0 L1 R1 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION, "ASTFUNCTION"), root_1);

                    	    // GAMS.g:310:68: ^( ASTFUNCTION0 L1 R1 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION0, "ASTFUNCTION0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L1.NextNode());
                    	    adaptor.AddChild(root_2, stream_R1.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:310:90: ^( ASTFUNCTION1 functionName )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION1, "ASTFUNCTION1"), root_2);

                    	    adaptor.AddChild(root_2, stream_functionName.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:310:119: ^( ASTFUNCTION2 functionElements )
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
                    // GAMS.g:311:15: functionName L2 functionElements R2
                    {
                    	PushFollow(FOLLOW_functionName_in_function1741);
                    	functionName114 = functionName();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionName.Add(functionName114.Tree);
                    	L2115=(IToken)Match(input,L2,FOLLOW_L2_in_function1743); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L2115);

                    	PushFollow(FOLLOW_functionElements_in_function1745);
                    	functionElements116 = functionElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionElements.Add(functionElements116.Tree);
                    	R2117=(IToken)Match(input,R2,FOLLOW_R2_in_function1747); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R2117);



                    	// AST REWRITE
                    	// elements:          functionElements, R2, functionName, L2
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 311:51: -> ^( ASTFUNCTION ^( ASTFUNCTION0 L2 R2 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	{
                    	    // GAMS.g:311:54: ^( ASTFUNCTION ^( ASTFUNCTION0 L2 R2 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION, "ASTFUNCTION"), root_1);

                    	    // GAMS.g:311:68: ^( ASTFUNCTION0 L2 R2 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION0, "ASTFUNCTION0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L2.NextNode());
                    	    adaptor.AddChild(root_2, stream_R2.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:311:90: ^( ASTFUNCTION1 functionName )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION1, "ASTFUNCTION1"), root_2);

                    	    adaptor.AddChild(root_2, stream_functionName.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:311:119: ^( ASTFUNCTION2 functionElements )
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
                    // GAMS.g:312:15: functionName L3 functionElements R3
                    {
                    	PushFollow(FOLLOW_functionName_in_function1789);
                    	functionName118 = functionName();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionName.Add(functionName118.Tree);
                    	L3119=(IToken)Match(input,L3,FOLLOW_L3_in_function1791); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L3119);

                    	PushFollow(FOLLOW_functionElements_in_function1793);
                    	functionElements120 = functionElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionElements.Add(functionElements120.Tree);
                    	R3121=(IToken)Match(input,R3,FOLLOW_R3_in_function1795); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R3121);



                    	// AST REWRITE
                    	// elements:          functionName, L3, R3, functionElements
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 312:51: -> ^( ASTFUNCTION ^( ASTFUNCTION0 L3 R3 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	{
                    	    // GAMS.g:312:54: ^( ASTFUNCTION ^( ASTFUNCTION0 L3 R3 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION, "ASTFUNCTION"), root_1);

                    	    // GAMS.g:312:68: ^( ASTFUNCTION0 L3 R3 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION0, "ASTFUNCTION0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L3.NextNode());
                    	    adaptor.AddChild(root_2, stream_R3.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:312:90: ^( ASTFUNCTION1 functionName )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION1, "ASTFUNCTION1"), root_2);

                    	    adaptor.AddChild(root_2, stream_functionName.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:312:119: ^( ASTFUNCTION2 functionElements )
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
            	Memoize(input, 29, function_StartIndex); 
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
    // GAMS.g:315:1: functionName : ( ABS | EXP | LOG | MAX | MIN | POWER | SQR | SAMEAS | TANH );
    public GAMSParser.functionName_return functionName() // throws RecognitionException [1]
    {   
        GAMSParser.functionName_return retval = new GAMSParser.functionName_return();
        retval.Start = input.LT(1);
        int functionName_StartIndex = input.Index();
        object root_0 = null;

        IToken set122 = null;

        object set122_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 30) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:315:13: ( ABS | EXP | LOG | MAX | MIN | POWER | SQR | SAMEAS | TANH )
            // GAMS.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set122 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= ABS && input.LA(1) <= TANH) || input.LA(1) == SAMEAS ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set122));
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
            	Memoize(input, 30, functionName_StartIndex); 
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
    // GAMS.g:317:1: functionElements : expression ( COMMA expression )* -> ^( ASTFUNCTIONELEMENTS ^( ASTFUNCTIONELEMENTS0 ( COMMA )* ) ^( ASTFUNCTIONELEMENTS1 ( expression )+ ) ) ;
    public GAMSParser.functionElements_return functionElements() // throws RecognitionException [1]
    {   
        GAMSParser.functionElements_return retval = new GAMSParser.functionElements_return();
        retval.Start = input.LT(1);
        int functionElements_StartIndex = input.Index();
        object root_0 = null;

        IToken COMMA124 = null;
        GAMSParser.expression_return expression123 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression125 = default(GAMSParser.expression_return);


        object COMMA124_tree=null;
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 31) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:317:17: ( expression ( COMMA expression )* -> ^( ASTFUNCTIONELEMENTS ^( ASTFUNCTIONELEMENTS0 ( COMMA )* ) ^( ASTFUNCTIONELEMENTS1 ( expression )+ ) ) )
            // GAMS.g:317:19: expression ( COMMA expression )*
            {
            	PushFollow(FOLLOW_expression_in_functionElements1870);
            	expression123 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression123.Tree);
            	// GAMS.g:317:30: ( COMMA expression )*
            	do 
            	{
            	    int alt26 = 2;
            	    int LA26_0 = input.LA(1);

            	    if ( (LA26_0 == COMMA) )
            	    {
            	        alt26 = 1;
            	    }


            	    switch (alt26) 
            		{
            			case 1 :
            			    // GAMS.g:317:31: COMMA expression
            			    {
            			    	COMMA124=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_functionElements1873); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA124);

            			    	PushFollow(FOLLOW_expression_in_functionElements1875);
            			    	expression125 = expression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_expression.Add(expression125.Tree);

            			    }
            			    break;

            			default:
            			    goto loop26;
            	    }
            	} while (true);

            	loop26:
            		;	// Stops C# compiler whining that label 'loop26' has no statements



            	// AST REWRITE
            	// elements:          expression, COMMA
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 318:3: -> ^( ASTFUNCTIONELEMENTS ^( ASTFUNCTIONELEMENTS0 ( COMMA )* ) ^( ASTFUNCTIONELEMENTS1 ( expression )+ ) )
            	{
            	    // GAMS.g:318:6: ^( ASTFUNCTIONELEMENTS ^( ASTFUNCTIONELEMENTS0 ( COMMA )* ) ^( ASTFUNCTIONELEMENTS1 ( expression )+ ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTIONELEMENTS, "ASTFUNCTIONELEMENTS"), root_1);

            	    // GAMS.g:318:28: ^( ASTFUNCTIONELEMENTS0 ( COMMA )* )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTIONELEMENTS0, "ASTFUNCTIONELEMENTS0"), root_2);

            	    // GAMS.g:318:51: ( COMMA )*
            	    while ( stream_COMMA.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_COMMA.NextNode());

            	    }
            	    stream_COMMA.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:318:59: ^( ASTFUNCTIONELEMENTS1 ( expression )+ )
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
            	Memoize(input, 31, functionElements_StartIndex); 
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
    // GAMS.g:320:1: sum : ( SUM L1 sumControlled ( conditional )? COMMA expression R1 -> ^( ASTSUM ^( ASTSUM0 L1 COMMA R1 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) | SUM L2 sumControlled ( conditional )? COMMA expression R2 -> ^( ASTSUM ^( ASTSUM0 L2 COMMA R2 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) | SUM L3 sumControlled ( conditional )? COMMA expression R3 -> ^( ASTSUM ^( ASTSUM0 L3 COMMA R3 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) );
    public GAMSParser.sum_return sum() // throws RecognitionException [1]
    {   
        GAMSParser.sum_return retval = new GAMSParser.sum_return();
        retval.Start = input.LT(1);
        int sum_StartIndex = input.Index();
        object root_0 = null;

        IToken SUM126 = null;
        IToken L1127 = null;
        IToken COMMA130 = null;
        IToken R1132 = null;
        IToken SUM133 = null;
        IToken L2134 = null;
        IToken COMMA137 = null;
        IToken R2139 = null;
        IToken SUM140 = null;
        IToken L3141 = null;
        IToken COMMA144 = null;
        IToken R3146 = null;
        GAMSParser.sumControlled_return sumControlled128 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional129 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression131 = default(GAMSParser.expression_return);

        GAMSParser.sumControlled_return sumControlled135 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional136 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression138 = default(GAMSParser.expression_return);

        GAMSParser.sumControlled_return sumControlled142 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional143 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression145 = default(GAMSParser.expression_return);


        object SUM126_tree=null;
        object L1127_tree=null;
        object COMMA130_tree=null;
        object R1132_tree=null;
        object SUM133_tree=null;
        object L2134_tree=null;
        object COMMA137_tree=null;
        object R2139_tree=null;
        object SUM140_tree=null;
        object L3141_tree=null;
        object COMMA144_tree=null;
        object R3146_tree=null;
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
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 32) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:320:4: ( SUM L1 sumControlled ( conditional )? COMMA expression R1 -> ^( ASTSUM ^( ASTSUM0 L1 COMMA R1 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) | SUM L2 sumControlled ( conditional )? COMMA expression R2 -> ^( ASTSUM ^( ASTSUM0 L2 COMMA R2 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) | SUM L3 sumControlled ( conditional )? COMMA expression R3 -> ^( ASTSUM ^( ASTSUM0 L3 COMMA R3 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) )
            int alt30 = 3;
            int LA30_0 = input.LA(1);

            if ( (LA30_0 == SUM) )
            {
                switch ( input.LA(2) ) 
                {
                case L1:
                	{
                    alt30 = 1;
                    }
                    break;
                case L2:
                	{
                    alt30 = 2;
                    }
                    break;
                case L3:
                	{
                    alt30 = 3;
                    }
                    break;
                	default:
                	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                	    NoViableAltException nvae_d30s1 =
                	        new NoViableAltException("", 30, 1, input);

                	    throw nvae_d30s1;
                }

            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d30s0 =
                    new NoViableAltException("", 30, 0, input);

                throw nvae_d30s0;
            }
            switch (alt30) 
            {
                case 1 :
                    // GAMS.g:320:7: SUM L1 sumControlled ( conditional )? COMMA expression R1
                    {
                    	SUM126=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum1908); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_SUM.Add(SUM126);

                    	L1127=(IToken)Match(input,L1,FOLLOW_L1_in_sum1910); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L1127);

                    	PushFollow(FOLLOW_sumControlled_in_sum1912);
                    	sumControlled128 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sumControlled.Add(sumControlled128.Tree);
                    	// GAMS.g:320:28: ( conditional )?
                    	int alt27 = 2;
                    	int LA27_0 = input.LA(1);

                    	if ( (LA27_0 == DOLLAR) )
                    	{
                    	    alt27 = 1;
                    	}
                    	switch (alt27) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:0:0: conditional
                    	        {
                    	        	PushFollow(FOLLOW_conditional_in_sum1914);
                    	        	conditional129 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional129.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA130=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum1917); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA130);

                    	PushFollow(FOLLOW_expression_in_sum1919);
                    	expression131 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression131.Tree);
                    	R1132=(IToken)Match(input,R1,FOLLOW_R1_in_sum1921); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R1132);



                    	// AST REWRITE
                    	// elements:          expression, L1, COMMA, sumControlled, R1, conditional
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 320:61: -> ^( ASTSUM ^( ASTSUM0 L1 COMMA R1 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	{
                    	    // GAMS.g:320:64: ^( ASTSUM ^( ASTSUM0 L1 COMMA R1 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM, "ASTSUM"), root_1);

                    	    // GAMS.g:320:73: ^( ASTSUM0 L1 COMMA R1 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM0, "ASTSUM0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L1.NextNode());
                    	    adaptor.AddChild(root_2, stream_COMMA.NextNode());
                    	    adaptor.AddChild(root_2, stream_R1.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:320:96: ^( ASTSUM1 sumControlled )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM1, "ASTSUM1"), root_2);

                    	    adaptor.AddChild(root_2, stream_sumControlled.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:320:121: ^( ASTSUM2 ( conditional )? )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM2, "ASTSUM2"), root_2);

                    	    // GAMS.g:320:131: ( conditional )?
                    	    if ( stream_conditional.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_2, stream_conditional.NextTree());

                    	    }
                    	    stream_conditional.Reset();

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:320:146: ^( ASTSUM3 expression )
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
                    // GAMS.g:321:7: SUM L2 sumControlled ( conditional )? COMMA expression R2
                    {
                    	SUM133=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum1965); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_SUM.Add(SUM133);

                    	L2134=(IToken)Match(input,L2,FOLLOW_L2_in_sum1967); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L2134);

                    	PushFollow(FOLLOW_sumControlled_in_sum1969);
                    	sumControlled135 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sumControlled.Add(sumControlled135.Tree);
                    	// GAMS.g:321:28: ( conditional )?
                    	int alt28 = 2;
                    	int LA28_0 = input.LA(1);

                    	if ( (LA28_0 == DOLLAR) )
                    	{
                    	    alt28 = 1;
                    	}
                    	switch (alt28) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:0:0: conditional
                    	        {
                    	        	PushFollow(FOLLOW_conditional_in_sum1971);
                    	        	conditional136 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional136.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA137=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum1974); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA137);

                    	PushFollow(FOLLOW_expression_in_sum1976);
                    	expression138 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression138.Tree);
                    	R2139=(IToken)Match(input,R2,FOLLOW_R2_in_sum1978); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R2139);



                    	// AST REWRITE
                    	// elements:          COMMA, L2, sumControlled, R2, expression, conditional
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 321:61: -> ^( ASTSUM ^( ASTSUM0 L2 COMMA R2 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	{
                    	    // GAMS.g:321:64: ^( ASTSUM ^( ASTSUM0 L2 COMMA R2 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM, "ASTSUM"), root_1);

                    	    // GAMS.g:321:73: ^( ASTSUM0 L2 COMMA R2 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM0, "ASTSUM0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L2.NextNode());
                    	    adaptor.AddChild(root_2, stream_COMMA.NextNode());
                    	    adaptor.AddChild(root_2, stream_R2.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:321:96: ^( ASTSUM1 sumControlled )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM1, "ASTSUM1"), root_2);

                    	    adaptor.AddChild(root_2, stream_sumControlled.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:321:121: ^( ASTSUM2 ( conditional )? )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM2, "ASTSUM2"), root_2);

                    	    // GAMS.g:321:131: ( conditional )?
                    	    if ( stream_conditional.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_2, stream_conditional.NextTree());

                    	    }
                    	    stream_conditional.Reset();

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:321:146: ^( ASTSUM3 expression )
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
                    // GAMS.g:322:7: SUM L3 sumControlled ( conditional )? COMMA expression R3
                    {
                    	SUM140=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum2022); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_SUM.Add(SUM140);

                    	L3141=(IToken)Match(input,L3,FOLLOW_L3_in_sum2024); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L3141);

                    	PushFollow(FOLLOW_sumControlled_in_sum2026);
                    	sumControlled142 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sumControlled.Add(sumControlled142.Tree);
                    	// GAMS.g:322:28: ( conditional )?
                    	int alt29 = 2;
                    	int LA29_0 = input.LA(1);

                    	if ( (LA29_0 == DOLLAR) )
                    	{
                    	    alt29 = 1;
                    	}
                    	switch (alt29) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:0:0: conditional
                    	        {
                    	        	PushFollow(FOLLOW_conditional_in_sum2028);
                    	        	conditional143 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional143.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA144=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum2031); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA144);

                    	PushFollow(FOLLOW_expression_in_sum2033);
                    	expression145 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression145.Tree);
                    	R3146=(IToken)Match(input,R3,FOLLOW_R3_in_sum2035); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R3146);



                    	// AST REWRITE
                    	// elements:          COMMA, sumControlled, R3, expression, conditional, L3
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 322:61: -> ^( ASTSUM ^( ASTSUM0 L3 COMMA R3 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	{
                    	    // GAMS.g:322:64: ^( ASTSUM ^( ASTSUM0 L3 COMMA R3 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM, "ASTSUM"), root_1);

                    	    // GAMS.g:322:73: ^( ASTSUM0 L3 COMMA R3 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM0, "ASTSUM0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L3.NextNode());
                    	    adaptor.AddChild(root_2, stream_COMMA.NextNode());
                    	    adaptor.AddChild(root_2, stream_R3.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:322:96: ^( ASTSUM1 sumControlled )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM1, "ASTSUM1"), root_2);

                    	    adaptor.AddChild(root_2, stream_sumControlled.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:322:121: ^( ASTSUM2 ( conditional )? )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM2, "ASTSUM2"), root_2);

                    	    // GAMS.g:322:131: ( conditional )?
                    	    if ( stream_conditional.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_2, stream_conditional.NextTree());

                    	    }
                    	    stream_conditional.Reset();

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:322:146: ^( ASTSUM3 expression )
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
            	Memoize(input, 32, sum_StartIndex); 
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
    // GAMS.g:325:1: sumControlled : ( variable -> ^( ASTSUMCONTROLLEDSIMPLE variable ) | L1 indexerElements R1 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L1 R1 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) | L2 indexerElements R2 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L2 R2 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) | L3 indexerElements R3 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L3 R3 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) );
    public GAMSParser.sumControlled_return sumControlled() // throws RecognitionException [1]
    {   
        GAMSParser.sumControlled_return retval = new GAMSParser.sumControlled_return();
        retval.Start = input.LT(1);
        int sumControlled_StartIndex = input.Index();
        object root_0 = null;

        IToken L1148 = null;
        IToken R1150 = null;
        IToken L2151 = null;
        IToken R2153 = null;
        IToken L3154 = null;
        IToken R3156 = null;
        GAMSParser.variable_return variable147 = default(GAMSParser.variable_return);

        GAMSParser.indexerElements_return indexerElements149 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements152 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements155 = default(GAMSParser.indexerElements_return);


        object L1148_tree=null;
        object R1150_tree=null;
        object L2151_tree=null;
        object R2153_tree=null;
        object L3154_tree=null;
        object R3156_tree=null;
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
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 33) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:325:14: ( variable -> ^( ASTSUMCONTROLLEDSIMPLE variable ) | L1 indexerElements R1 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L1 R1 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) | L2 indexerElements R2 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L2 R2 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) | L3 indexerElements R3 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L3 R3 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) )
            int alt31 = 4;
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
            case MODEL:
            case SOLVE:
            case SAMEAS:
            case VARIABLES:
            case EQUATIONS:
            case Ident:
            	{
                alt31 = 1;
                }
                break;
            case L1:
            	{
                alt31 = 2;
                }
                break;
            case L2:
            	{
                alt31 = 3;
                }
                break;
            case L3:
            	{
                alt31 = 4;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d31s0 =
            	        new NoViableAltException("", 31, 0, input);

            	    throw nvae_d31s0;
            }

            switch (alt31) 
            {
                case 1 :
                    // GAMS.g:326:11: variable
                    {
                    	PushFollow(FOLLOW_variable_in_sumControlled2090);
                    	variable147 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variable.Add(variable147.Tree);


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
                    	// 326:33: -> ^( ASTSUMCONTROLLEDSIMPLE variable )
                    	{
                    	    // GAMS.g:326:36: ^( ASTSUMCONTROLLEDSIMPLE variable )
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
                    // GAMS.g:327:5: L1 indexerElements R1
                    {
                    	L1148=(IToken)Match(input,L1,FOLLOW_L1_in_sumControlled2117); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L1148);

                    	PushFollow(FOLLOW_indexerElements_in_sumControlled2119);
                    	indexerElements149 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements149.Tree);
                    	R1150=(IToken)Match(input,R1,FOLLOW_R1_in_sumControlled2121); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R1150);



                    	// AST REWRITE
                    	// elements:          R1, L1, indexerElements
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 327:27: -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L1 R1 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	{
                    	    // GAMS.g:327:30: ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L1 R1 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED, "ASTSUMCONTROLLED"), root_1);

                    	    // GAMS.g:327:49: ^( ASTSUMCONTROLLED0 L1 R1 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED0, "ASTSUMCONTROLLED0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L1.NextNode());
                    	    adaptor.AddChild(root_2, stream_R1.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:327:76: ^( ASTSUMCONTROLLED2 indexerElements )
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
                    // GAMS.g:328:5: L2 indexerElements R2
                    {
                    	L2151=(IToken)Match(input,L2,FOLLOW_L2_in_sumControlled2147); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L2151);

                    	PushFollow(FOLLOW_indexerElements_in_sumControlled2149);
                    	indexerElements152 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements152.Tree);
                    	R2153=(IToken)Match(input,R2,FOLLOW_R2_in_sumControlled2151); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R2153);



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
                    	// 328:27: -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L2 R2 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	{
                    	    // GAMS.g:328:30: ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L2 R2 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED, "ASTSUMCONTROLLED"), root_1);

                    	    // GAMS.g:328:49: ^( ASTSUMCONTROLLED0 L2 R2 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED0, "ASTSUMCONTROLLED0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L2.NextNode());
                    	    adaptor.AddChild(root_2, stream_R2.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:328:76: ^( ASTSUMCONTROLLED2 indexerElements )
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
                    // GAMS.g:329:5: L3 indexerElements R3
                    {
                    	L3154=(IToken)Match(input,L3,FOLLOW_L3_in_sumControlled2177); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L3154);

                    	PushFollow(FOLLOW_indexerElements_in_sumControlled2179);
                    	indexerElements155 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements155.Tree);
                    	R3156=(IToken)Match(input,R3,FOLLOW_R3_in_sumControlled2181); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R3156);



                    	// AST REWRITE
                    	// elements:          R3, indexerElements, L3
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 329:27: -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L3 R3 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	{
                    	    // GAMS.g:329:30: ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L3 R3 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED, "ASTSUMCONTROLLED"), root_1);

                    	    // GAMS.g:329:49: ^( ASTSUMCONTROLLED0 L3 R3 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED0, "ASTSUMCONTROLLED0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L3.NextNode());
                    	    adaptor.AddChild(root_2, stream_R3.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:329:76: ^( ASTSUMCONTROLLED2 indexerElements )
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
            	Memoize(input, 33, sumControlled_StartIndex); 
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
    // GAMS.g:336:1: ident : ( Ident | extraTokens );
    public GAMSParser.ident_return ident() // throws RecognitionException [1]
    {   
        GAMSParser.ident_return retval = new GAMSParser.ident_return();
        retval.Start = input.LT(1);
        int ident_StartIndex = input.Index();
        object root_0 = null;

        IToken Ident157 = null;
        GAMSParser.extraTokens_return extraTokens158 = default(GAMSParser.extraTokens_return);


        object Ident157_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 34) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:336:9: ( Ident | extraTokens )
            int alt32 = 2;
            int LA32_0 = input.LA(1);

            if ( (LA32_0 == Ident) )
            {
                alt32 = 1;
            }
            else if ( ((LA32_0 >= SUM && LA32_0 <= EQUATIONS)) )
            {
                alt32 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d32s0 =
                    new NoViableAltException("", 32, 0, input);

                throw nvae_d32s0;
            }
            switch (alt32) 
            {
                case 1 :
                    // GAMS.g:336:12: Ident
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Ident157=(IToken)Match(input,Ident,FOLLOW_Ident_in_ident2217); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Ident157_tree = (object)adaptor.Create(Ident157);
                    		adaptor.AddChild(root_0, Ident157_tree);
                    	}

                    }
                    break;
                case 2 :
                    // GAMS.g:336:20: extraTokens
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_extraTokens_in_ident2221);
                    	extraTokens158 = extraTokens();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, extraTokens158.Tree);

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
            	Memoize(input, 34, ident_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "ident"

    // $ANTLR start "synpred18_GAMS"
    public void synpred18_GAMS_fragment() {
        // GAMS.g:196:7: ( equ )
        // GAMS.g:196:7: equ
        {
        	PushFollow(FOLLOW_equ_in_synpred18_GAMS517);
        	equ();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred18_GAMS"

    // $ANTLR start "synpred19_GAMS"
    public void synpred19_GAMS_fragment() {
        // GAMS.g:197:9: ( vardef )
        // GAMS.g:197:9: vardef
        {
        	PushFollow(FOLLOW_vardef_in_synpred19_GAMS527);
        	vardef();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred19_GAMS"

    // $ANTLR start "synpred20_GAMS"
    public void synpred20_GAMS_fragment() {
        // GAMS.g:198:6: ( variables )
        // GAMS.g:198:6: variables
        {
        	PushFollow(FOLLOW_variables_in_synpred20_GAMS534);
        	variables();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred20_GAMS"

    // $ANTLR start "synpred21_GAMS"
    public void synpred21_GAMS_fragment() {
        // GAMS.g:199:6: ( equations )
        // GAMS.g:199:6: equations
        {
        	PushFollow(FOLLOW_equations_in_synpred21_GAMS541);
        	equations();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred21_GAMS"

    // $ANTLR start "synpred31_GAMS"
    public void synpred31_GAMS_fragment() {
        // GAMS.g:224:55: ( conditional )
        // GAMS.g:224:55: conditional
        {
        	PushFollow(FOLLOW_conditional_in_synpred31_GAMS788);
        	conditional();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred31_GAMS"

    // $ANTLR start "synpred38_GAMS"
    public void synpred38_GAMS_fragment() {
        // GAMS.g:261:28: ( OR andExpression )
        // GAMS.g:261:28: OR andExpression
        {
        	Match(input,OR,FOLLOW_OR_in_synpred38_GAMS1186); if (state.failed) return ;
        	PushFollow(FOLLOW_andExpression_in_synpred38_GAMS1189);
        	andExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred38_GAMS"

    // $ANTLR start "synpred39_GAMS"
    public void synpred39_GAMS_fragment() {
        // GAMS.g:263:31: ( AND notExpression )
        // GAMS.g:263:31: AND notExpression
        {
        	Match(input,AND,FOLLOW_AND_in_synpred39_GAMS1201); if (state.failed) return ;
        	PushFollow(FOLLOW_notExpression_in_synpred39_GAMS1204);
        	notExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred39_GAMS"

    // $ANTLR start "synpred40_GAMS"
    public void synpred40_GAMS_fragment() {
        // GAMS.g:265:16: ( NOT logicalExpression )
        // GAMS.g:265:16: NOT logicalExpression
        {
        	Match(input,NOT,FOLLOW_NOT_in_synpred40_GAMS1215); if (state.failed) return ;
        	PushFollow(FOLLOW_logicalExpression_in_synpred40_GAMS1217);
        	logicalExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred40_GAMS"

    // $ANTLR start "synpred41_GAMS"
    public void synpred41_GAMS_fragment() {
        // GAMS.g:268:41: ( logical additiveExpression )
        // GAMS.g:268:41: logical additiveExpression
        {
        	PushFollow(FOLLOW_logical_in_synpred41_GAMS1248);
        	logical();
        	state.followingStackPointer--;
        	if (state.failed) return ;
        	PushFollow(FOLLOW_additiveExpression_in_synpred41_GAMS1251);
        	additiveExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred41_GAMS"

    // $ANTLR start "synpred43_GAMS"
    public void synpred43_GAMS_fragment() {
        // GAMS.g:270:48: ( ( PLUS | MINUS ) multiplicativeExpression )
        // GAMS.g:270:48: ( PLUS | MINUS ) multiplicativeExpression
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

        	PushFollow(FOLLOW_multiplicativeExpression_in_synpred43_GAMS1271);
        	multiplicativeExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred43_GAMS"

    // $ANTLR start "synpred45_GAMS"
    public void synpred45_GAMS_fragment() {
        // GAMS.g:272:45: ( ( MULT | DIV ) powerExpression )
        // GAMS.g:272:45: ( MULT | DIV ) powerExpression
        {
        	if ( input.LA(1) == DIV || input.LA(1) == MULT ) 
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

        	PushFollow(FOLLOW_powerExpression_in_synpred45_GAMS1292);
        	powerExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred45_GAMS"

    // $ANTLR start "synpred46_GAMS"
    public void synpred46_GAMS_fragment() {
        // GAMS.g:274:36: ( STARS unaryExpression )
        // GAMS.g:274:36: STARS unaryExpression
        {
        	Match(input,STARS,FOLLOW_STARS_in_synpred46_GAMS1306); if (state.failed) return ;
        	PushFollow(FOLLOW_unaryExpression_in_synpred46_GAMS1309);
        	unaryExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred46_GAMS"

    // $ANTLR start "synpred48_GAMS"
    public void synpred48_GAMS_fragment() {
        // GAMS.g:286:9: ( primaryExpression conditional )
        // GAMS.g:286:9: primaryExpression conditional
        {
        	PushFollow(FOLLOW_primaryExpression_in_synpred48_GAMS1368);
        	primaryExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;
        	PushFollow(FOLLOW_conditional_in_synpred48_GAMS1370);
        	conditional();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred48_GAMS"

    // $ANTLR start "synpred59_GAMS"
    public void synpred59_GAMS_fragment() {
        // GAMS.g:301:6: ( sum )
        // GAMS.g:301:6: sum
        {
        	PushFollow(FOLLOW_sum_in_synpred59_GAMS1576);
        	sum();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred59_GAMS"

    // $ANTLR start "synpred60_GAMS"
    public void synpred60_GAMS_fragment() {
        // GAMS.g:302:6: ( function )
        // GAMS.g:302:6: function
        {
        	PushFollow(FOLLOW_function_in_synpred60_GAMS1613);
        	function();
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
   	public bool synpred20_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred20_GAMS_fragment(); // can never throw exception
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
   	public bool synpred19_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred19_GAMS_fragment(); // can never throw exception
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
   	public bool synpred18_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred18_GAMS_fragment(); // can never throw exception
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
   	public bool synpred45_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred45_GAMS_fragment(); // can never throw exception
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
   	public bool synpred48_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred48_GAMS_fragment(); // can never throw exception
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
   	public bool synpred31_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred31_GAMS_fragment(); // can never throw exception
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
   	public bool synpred21_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred21_GAMS_fragment(); // can never throw exception
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
   	public bool synpred39_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred39_GAMS_fragment(); // can never throw exception
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
   	public bool synpred41_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred41_GAMS_fragment(); // can never throw exception
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


   	protected DFA2 dfa2;
   	protected DFA8 dfa8;
   	protected DFA9 dfa9;
   	protected DFA10 dfa10;
   	protected DFA13 dfa13;
   	protected DFA14 dfa14;
   	protected DFA15 dfa15;
   	protected DFA16 dfa16;
   	protected DFA17 dfa17;
   	protected DFA18 dfa18;
   	protected DFA19 dfa19;
   	protected DFA20 dfa20;
   	protected DFA21 dfa21;
   	protected DFA22 dfa22;
   	protected DFA23 dfa23;
   	protected DFA24 dfa24;
	private void InitializeCyclicDFAs()
	{
    	this.dfa2 = new DFA2(this);
    	this.dfa8 = new DFA8(this);
    	this.dfa9 = new DFA9(this);
    	this.dfa10 = new DFA10(this);
    	this.dfa13 = new DFA13(this);
    	this.dfa14 = new DFA14(this);
    	this.dfa15 = new DFA15(this);
    	this.dfa16 = new DFA16(this);
    	this.dfa17 = new DFA17(this);
    	this.dfa18 = new DFA18(this);
    	this.dfa19 = new DFA19(this);
    	this.dfa20 = new DFA20(this);
    	this.dfa21 = new DFA21(this);
    	this.dfa22 = new DFA22(this);
    	this.dfa23 = new DFA23(this);
    	this.dfa24 = new DFA24(this);
	    this.dfa2.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA2_SpecialStateTransition);


	    this.dfa10.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA10_SpecialStateTransition);

	    this.dfa14.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA14_SpecialStateTransition);
	    this.dfa15.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA15_SpecialStateTransition);
	    this.dfa16.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA16_SpecialStateTransition);
	    this.dfa17.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA17_SpecialStateTransition);
	    this.dfa18.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA18_SpecialStateTransition);
	    this.dfa19.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA19_SpecialStateTransition);
	    this.dfa20.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA20_SpecialStateTransition);

	    this.dfa22.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA22_SpecialStateTransition);

	    this.dfa24.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA24_SpecialStateTransition);
	}

    const string DFA2_eotS =
        "\x34\uffff";
    const string DFA2_eofS =
        "\x34\uffff";
    const string DFA2_minS =
        "\x01\x45\x01\x56\x03\x45\x01\x56\x01\uffff\x03\x00\x03\uffff\x02"+
        "\x00\x01\uffff\x03\x00\x04\uffff\x03\x00\x08\uffff\x03\x00\x07\uffff"+
        "\x03\x00\x05\uffff";
    const string DFA2_maxS =
        "\x01\x70\x01\x67\x03\x70\x01\x67\x01\uffff\x03\x00\x03\uffff\x02"+
        "\x00\x01\uffff\x03\x00\x04\uffff\x03\x00\x08\uffff\x03\x00\x07\uffff"+
        "\x03\x00\x05\uffff";
    const string DFA2_acceptS =
        "\x06\uffff\x01\x02\x05\uffff\x01\x01\x10\uffff\x01\x05\x0a\uffff"+
        "\x01\x06\x09\uffff\x01\x03\x01\x04";
    const string DFA2_specialS =
        "\x07\uffff\x01\x00\x01\x01\x01\x02\x03\uffff\x01\x03\x01\x04\x01"+
        "\uffff\x01\x05\x01\x06\x01\x07\x04\uffff\x01\x08\x01\x09\x01\x0a"+
        "\x08\uffff\x01\x0b\x01\x0c\x01\x0d\x07\uffff\x01\x0e\x01\x0f\x01"+
        "\x10\x05\uffff}>";
    static readonly string[] DFA2_transitionS = {
            "\x0c\x05\x01\x03\x01\x04\x01\x05\x01\x02\x01\x05\x1a\uffff"+
            "\x01\x01",
            "\x01\x0c\x02\uffff\x01\x06\x02\uffff\x01\x06\x01\x07\x01\uffff"+
            "\x01\x08\x01\uffff\x01\x09\x05\uffff\x01\x06",
            "\x11\x0e\x01\x0c\x02\uffff\x01\x06\x02\uffff\x01\x06\x01\x10"+
            "\x01\uffff\x01\x11\x01\uffff\x01\x12\x05\uffff\x01\x06\x08\uffff"+
            "\x01\x0d",
            "\x11\x1d\x01\x0c\x01\uffff\x01\x1d\x01\x06\x01\uffff\x01\x1d"+
            "\x01\x06\x01\x17\x01\uffff\x01\x18\x01\uffff\x01\x19\x05\uffff"+
            "\x01\x06\x08\uffff\x01\x1d",
            "\x11\x28\x01\x0c\x01\uffff\x01\x28\x01\x06\x02\uffff\x01\x06"+
            "\x01\x22\x01\uffff\x01\x23\x01\uffff\x01\x24\x05\uffff\x01\x06"+
            "\x08\uffff\x01\x28",
            "\x01\x0c\x02\uffff\x01\x06\x02\uffff\x01\x06\x01\x2c\x01\uffff"+
            "\x01\x2d\x01\uffff\x01\x2e\x05\uffff\x01\x06",
            "",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "",
            "",
            "",
            "\x01\uffff",
            "\x01\uffff",
            "",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
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
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
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
            get { return "196:1: expr : ( equ | vardef | variables | equations | model | solve );"; }
        }

    }


    protected internal int DFA2_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA2_7 = input.LA(1);

                   	 
                   	int index2_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_7);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA2_8 = input.LA(1);

                   	 
                   	int index2_8 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_8);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA2_9 = input.LA(1);

                   	 
                   	int index2_9 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_9);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA2_13 = input.LA(1);

                   	 
                   	int index2_13 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred20_GAMS()) ) { s = 50; }

                   	else if ( (synpred21_GAMS()) ) { s = 51; }

                   	 
                   	input.Seek(index2_13);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA2_14 = input.LA(1);

                   	 
                   	int index2_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred20_GAMS()) ) { s = 50; }

                   	else if ( (synpred21_GAMS()) ) { s = 51; }

                   	 
                   	input.Seek(index2_14);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA2_16 = input.LA(1);

                   	 
                   	int index2_16 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_16);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 6 : 
                   	int LA2_17 = input.LA(1);

                   	 
                   	int index2_17 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_17);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 7 : 
                   	int LA2_18 = input.LA(1);

                   	 
                   	int index2_18 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_18);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 8 : 
                   	int LA2_23 = input.LA(1);

                   	 
                   	int index2_23 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_23);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 9 : 
                   	int LA2_24 = input.LA(1);

                   	 
                   	int index2_24 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_24);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 10 : 
                   	int LA2_25 = input.LA(1);

                   	 
                   	int index2_25 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_25);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 11 : 
                   	int LA2_34 = input.LA(1);

                   	 
                   	int index2_34 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_34);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 12 : 
                   	int LA2_35 = input.LA(1);

                   	 
                   	int index2_35 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_35);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 13 : 
                   	int LA2_36 = input.LA(1);

                   	 
                   	int index2_36 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_36);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 14 : 
                   	int LA2_44 = input.LA(1);

                   	 
                   	int index2_44 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_44);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 15 : 
                   	int LA2_45 = input.LA(1);

                   	 
                   	int index2_45 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_45);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 16 : 
                   	int LA2_46 = input.LA(1);

                   	 
                   	int index2_46 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 12; }

                   	else if ( (synpred19_GAMS()) ) { s = 6; }

                   	 
                   	input.Seek(index2_46);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae2 =
            new NoViableAltException(dfa.Description, 2, _s, input);
        dfa.Error(nvae2);
        throw nvae2;
    }
    const string DFA8_eotS =
        "\x14\uffff";
    const string DFA8_eofS =
        "\x01\x02\x13\uffff";
    const string DFA8_minS =
        "\x01\x46\x13\uffff";
    const string DFA8_maxS =
        "\x01\x6e\x13\uffff";
    const string DFA8_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x11\uffff";
    const string DFA8_specialS =
        "\x14\uffff}>";
    static readonly string[] DFA8_transitionS = {
            "\x02\x02\x0f\uffff\x05\x02\x01\x01\x06\x02\x01\uffff\x01\x02"+
            "\x01\uffff\x09\x02",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
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
            get { return "224:34: ( DOT variable )?"; }
        }

    }

    const string DFA9_eotS =
        "\x13\uffff";
    const string DFA9_eofS =
        "\x01\x04\x12\uffff";
    const string DFA9_minS =
        "\x01\x46\x12\uffff";
    const string DFA9_maxS =
        "\x01\x6e\x12\uffff";
    const string DFA9_acceptS =
        "\x01\uffff\x01\x01\x02\uffff\x01\x02\x0e\uffff";
    const string DFA9_specialS =
        "\x13\uffff}>";
    static readonly string[] DFA9_transitionS = {
            "\x02\x04\x0f\uffff\x05\x04\x01\uffff\x01\x01\x01\x04\x01\x01"+
            "\x01\x04\x01\x01\x01\x04\x01\uffff\x01\x04\x01\uffff\x09\x04",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
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
            get { return "224:50: ( idx )?"; }
        }

    }

    const string DFA10_eotS =
        "\x1c\uffff";
    const string DFA10_eofS =
        "\x01\x02\x1b\uffff";
    const string DFA10_minS =
        "\x01\x46\x01\x45\x0e\uffff\x0b\x00\x01\uffff";
    const string DFA10_maxS =
        "\x01\x6e\x01\x70\x0e\uffff\x0b\x00\x01\uffff";
    const string DFA10_acceptS =
        "\x02\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA10_specialS =
        "\x10\uffff\x01\x00\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01"+
        "\x06\x01\x07\x01\x08\x01\x09\x01\x0a\x01\uffff}>";
    static readonly string[] DFA10_transitionS = {
            "\x02\x02\x0f\uffff\x05\x02\x02\uffff\x01\x02\x01\uffff\x01"+
            "\x02\x01\uffff\x01\x02\x01\uffff\x01\x02\x01\uffff\x01\x02\x01"+
            "\x01\x07\x02",
            "\x01\x17\x02\x1a\x01\x10\x08\x18\x02\x1a\x01\x18\x02\x1a\x07"+
            "\uffff\x01\x12\x01\uffff\x01\x13\x01\uffff\x01\x14\x03\uffff"+
            "\x01\x15\x01\x11\x08\uffff\x01\x16\x01\x19",
            "",
            "",
            "",
            "",
            "",
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
            get { return "224:55: ( conditional )?"; }
        }

    }


    protected internal int DFA10_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA10_16 = input.LA(1);

                   	 
                   	int index10_16 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred31_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index10_16);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA10_17 = input.LA(1);

                   	 
                   	int index10_17 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred31_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index10_17);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA10_18 = input.LA(1);

                   	 
                   	int index10_18 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred31_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index10_18);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA10_19 = input.LA(1);

                   	 
                   	int index10_19 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred31_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index10_19);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA10_20 = input.LA(1);

                   	 
                   	int index10_20 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred31_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index10_20);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA10_21 = input.LA(1);

                   	 
                   	int index10_21 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred31_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index10_21);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 6 : 
                   	int LA10_22 = input.LA(1);

                   	 
                   	int index10_22 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred31_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index10_22);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 7 : 
                   	int LA10_23 = input.LA(1);

                   	 
                   	int index10_23 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred31_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index10_23);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 8 : 
                   	int LA10_24 = input.LA(1);

                   	 
                   	int index10_24 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred31_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index10_24);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 9 : 
                   	int LA10_25 = input.LA(1);

                   	 
                   	int index10_25 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred31_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index10_25);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 10 : 
                   	int LA10_26 = input.LA(1);

                   	 
                   	int index10_26 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred31_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index10_26);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae10 =
            new NoViableAltException(dfa.Description, 10, _s, input);
        dfa.Error(nvae10);
        throw nvae10;
    }
    const string DFA13_eotS =
        "\x12\uffff";
    const string DFA13_eofS =
        "\x02\uffff\x02\x05\x0e\uffff";
    const string DFA13_minS =
        "\x01\x45\x01\uffff\x02\x5a\x0e\uffff";
    const string DFA13_maxS =
        "\x01\x70\x01\uffff\x02\x66\x0e\uffff";
    const string DFA13_acceptS =
        "\x01\uffff\x01\x01\x02\uffff\x01\x03\x01\x04\x04\uffff\x01\x02"+
        "\x07\uffff";
    const string DFA13_specialS =
        "\x12\uffff}>";
    static readonly string[] DFA13_transitionS = {
            "\x11\x03\x0d\uffff\x01\x01\x0c\uffff\x01\x02",
            "",
            "\x01\x05\x03\uffff\x01\x05\x01\uffff\x01\x05\x01\uffff\x01"+
            "\x05\x01\uffff\x01\x0a\x01\uffff\x01\x04",
            "\x01\x05\x03\uffff\x01\x05\x01\uffff\x01\x05\x01\uffff\x01"+
            "\x05\x01\uffff\x01\x0a\x01\uffff\x01\x04",
            "",
            "",
            "",
            "",
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
            get { return "237:1: variableLagLead : ( StringInQuotes -> ^( ASTVARIABLEANDLEAD StringInQuotes ) | variable PLUS Integer -> ^( ASTVARIABLEANDLEAD variable PLUS Integer ) | variable MINUS Integer -> ^( ASTVARIABLEANDLEAD variable MINUS Integer ) | variable -> ^( ASTVARIABLEANDLEAD variable ) );"; }
        }

    }

    const string DFA14_eotS =
        "\x1c\uffff";
    const string DFA14_eofS =
        "\x01\x01\x1b\uffff";
    const string DFA14_minS =
        "\x01\x46\x09\uffff\x01\x00\x11\uffff";
    const string DFA14_maxS =
        "\x01\x6e\x09\uffff\x01\x00\x11\uffff";
    const string DFA14_acceptS =
        "\x01\uffff\x01\x02\x19\uffff\x01\x01";
    const string DFA14_specialS =
        "\x0a\uffff\x01\x00\x11\uffff}>";
    static readonly string[] DFA14_transitionS = {
            "\x01\x01\x01\x0a\x0f\uffff\x05\x01\x02\uffff\x01\x01\x01\uffff"+
            "\x01\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x09\x01",
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
            get { return "()* loopback of 261:27: ( OR andExpression )*"; }
        }

    }


    protected internal int DFA14_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA14_10 = input.LA(1);

                   	 
                   	int index14_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred38_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index14_10);
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
        "\x01\x46\x09\uffff\x01\x00\x11\uffff";
    const string DFA15_maxS =
        "\x01\x6e\x09\uffff\x01\x00\x11\uffff";
    const string DFA15_acceptS =
        "\x01\uffff\x01\x02\x19\uffff\x01\x01";
    const string DFA15_specialS =
        "\x0a\uffff\x01\x00\x11\uffff}>";
    static readonly string[] DFA15_transitionS = {
            "\x01\x0a\x01\x01\x0f\uffff\x05\x01\x02\uffff\x01\x01\x01\uffff"+
            "\x01\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x09\x01",
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
            get { return "()* loopback of 263:30: ( AND notExpression )*"; }
        }

    }


    protected internal int DFA15_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA15_10 = input.LA(1);

                   	 
                   	int index15_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred39_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index15_10);
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
        "\x26\uffff";
    const string DFA16_eofS =
        "\x01\uffff\x01\x02\x24\uffff";
    const string DFA16_minS =
        "\x02\x45\x0b\uffff\x03\x00\x03\uffff\x01\x00\x01\uffff\x02\x00"+
        "\x0f\uffff";
    const string DFA16_maxS =
        "\x02\x70\x0b\uffff\x03\x00\x03\uffff\x01\x00\x01\uffff\x02\x00"+
        "\x0f\uffff";
    const string DFA16_acceptS =
        "\x02\uffff\x01\x02\x1d\uffff\x01\x01\x05\uffff";
    const string DFA16_specialS =
        "\x0d\uffff\x01\x00\x01\x01\x01\x02\x03\uffff\x01\x03\x01\uffff"+
        "\x01\x04\x01\x05\x0f\uffff}>";
    static readonly string[] DFA16_transitionS = {
            "\x03\x02\x01\x01\x0d\x02\x07\uffff\x01\x02\x01\uffff\x01\x02"+
            "\x01\uffff\x01\x02\x03\uffff\x02\x02\x08\uffff\x02\x02",
            "\x01\x20\x01\x15\x01\x16\x0e\x20\x01\uffff\x06\x02\x01\x0d"+
            "\x01\x02\x01\x0e\x01\x02\x01\x0f\x01\x02\x01\uffff\x01\x02\x01"+
            "\x20\x01\x13\x08\x02\x02\x20",
            "",
            "",
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
            get { return "265:1: notExpression : ( NOT logicalExpression -> ^( NOT logicalExpression ) | logicalExpression );"; }
        }

    }


    protected internal int DFA16_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA16_13 = input.LA(1);

                   	 
                   	int index16_13 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred40_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index16_13);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA16_14 = input.LA(1);

                   	 
                   	int index16_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred40_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index16_14);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA16_15 = input.LA(1);

                   	 
                   	int index16_15 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred40_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index16_15);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA16_19 = input.LA(1);

                   	 
                   	int index16_19 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred40_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index16_19);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA16_21 = input.LA(1);

                   	 
                   	int index16_21 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred40_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index16_21);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA16_22 = input.LA(1);

                   	 
                   	int index16_22 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred40_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index16_22);
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
        "\x26\uffff";
    const string DFA17_eofS =
        "\x01\x01\x25\uffff";
    const string DFA17_minS =
        "\x01\x46\x04\uffff\x01\x00\x04\uffff\x01\x00\x1b\uffff";
    const string DFA17_maxS =
        "\x01\x6e\x04\uffff\x01\x00\x04\uffff\x01\x00\x1b\uffff";
    const string DFA17_acceptS =
        "\x01\uffff\x01\x02\x19\uffff\x01\x01\x0a\uffff";
    const string DFA17_specialS =
        "\x05\uffff\x01\x00\x04\uffff\x01\x01\x1b\uffff}>";
    static readonly string[] DFA17_transitionS = {
            "\x02\x01\x0f\uffff\x02\x01\x01\x05\x02\x01\x02\uffff\x01\x01"+
            "\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff"+
            "\x04\x01\x05\x0a",
            "",
            "",
            "",
            "",
            "\x01\uffff",
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
            "",
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
            get { return "()* loopback of 268:40: ( logical additiveExpression )*"; }
        }

    }


    protected internal int DFA17_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA17_5 = input.LA(1);

                   	 
                   	int index17_5 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index17_5);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA17_10 = input.LA(1);

                   	 
                   	int index17_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index17_10);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae17 =
            new NoViableAltException(dfa.Description, 17, _s, input);
        dfa.Error(nvae17);
        throw nvae17;
    }
    const string DFA18_eotS =
        "\x1b\uffff";
    const string DFA18_eofS =
        "\x01\x01\x1a\uffff";
    const string DFA18_minS =
        "\x01\x46\x09\uffff\x01\x00\x10\uffff";
    const string DFA18_maxS =
        "\x01\x6e\x09\uffff\x01\x00\x10\uffff";
    const string DFA18_acceptS =
        "\x01\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA18_specialS =
        "\x0a\uffff\x01\x00\x10\uffff}>";
    static readonly string[] DFA18_transitionS = {
            "\x02\x01\x0f\uffff\x05\x01\x02\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x01\x01\x01\uffff\x01\x0a\x01\uffff\x01\x0a\x08"+
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
            get { return "()* loopback of 270:46: ( ( PLUS | MINUS ) multiplicativeExpression )*"; }
        }

    }


    protected internal int DFA18_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA18_10 = input.LA(1);

                   	 
                   	int index18_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred43_GAMS()) ) { s = 26; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index18_10);
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
        "\x1b\uffff";
    const string DFA19_eofS =
        "\x01\x01\x1a\uffff";
    const string DFA19_minS =
        "\x01\x46\x09\uffff\x01\x00\x10\uffff";
    const string DFA19_maxS =
        "\x01\x6e\x09\uffff\x01\x00\x10\uffff";
    const string DFA19_acceptS =
        "\x01\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA19_specialS =
        "\x0a\uffff\x01\x00\x10\uffff}>";
    static readonly string[] DFA19_transitionS = {
            "\x02\x01\x0f\uffff\x04\x01\x01\x0a\x02\uffff\x01\x01\x01\uffff"+
            "\x01\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x02\x01"+
            "\x01\x0a\x06\x01",
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
            get { return "()* loopback of 272:43: ( ( MULT | DIV ) powerExpression )*"; }
        }

    }


    protected internal int DFA19_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA19_10 = input.LA(1);

                   	 
                   	int index19_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred45_GAMS()) ) { s = 26; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index19_10);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae19 =
            new NoViableAltException(dfa.Description, 19, _s, input);
        dfa.Error(nvae19);
        throw nvae19;
    }
    const string DFA20_eotS =
        "\x1b\uffff";
    const string DFA20_eofS =
        "\x01\x01\x1a\uffff";
    const string DFA20_minS =
        "\x01\x46\x09\uffff\x01\x00\x10\uffff";
    const string DFA20_maxS =
        "\x01\x6e\x09\uffff\x01\x00\x10\uffff";
    const string DFA20_acceptS =
        "\x01\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA20_specialS =
        "\x0a\uffff\x01\x00\x10\uffff}>";
    static readonly string[] DFA20_transitionS = {
            "\x02\x01\x0f\uffff\x05\x01\x02\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x03\x01\x01"+
            "\x0a\x05\x01",
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
            get { return "()* loopback of 274:34: ( STARS unaryExpression )*"; }
        }

    }


    protected internal int DFA20_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA20_10 = input.LA(1);

                   	 
                   	int index20_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred46_GAMS()) ) { s = 26; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index20_10);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae20 =
            new NoViableAltException(dfa.Description, 20, _s, input);
        dfa.Error(nvae20);
        throw nvae20;
    }
    const string DFA21_eotS =
        "\x0b\uffff";
    const string DFA21_eofS =
        "\x0b\uffff";
    const string DFA21_minS =
        "\x01\x45\x0a\uffff";
    const string DFA21_maxS =
        "\x01\x70\x0a\uffff";
    const string DFA21_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x08\uffff";
    const string DFA21_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA21_transitionS = {
            "\x11\x02\x07\uffff\x01\x02\x01\uffff\x01\x02\x01\uffff\x01"+
            "\x02\x03\uffff\x01\x02\x01\x01\x08\uffff\x02\x02",
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
            get { return "276:1: unaryExpression : ( MINUS dollarExpression -> ^( NEGATE dollarExpression ) | dollarExpression );"; }
        }

    }

    const string DFA22_eotS =
        "\u0097\uffff";
    const string DFA22_eofS =
        "\u0097\uffff";
    const string DFA22_minS =
        "\x04\x45\x27\x00\x6c\uffff";
    const string DFA22_maxS =
        "\x04\x70\x27\x00\x6c\uffff";
    const string DFA22_acceptS =
        "\x3a\uffff\x01\x01\x01\x02\x5b\uffff";
    const string DFA22_specialS =
        "\x04\uffff\x01\x00\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01"+
        "\x06\x01\x07\x01\x08\x01\x09\x01\x0a\x01\x0b\x01\x0c\x01\x0d\x01"+
        "\x0e\x01\x0f\x01\x10\x01\x11\x01\x12\x01\x13\x01\x14\x01\x15\x01"+
        "\x16\x01\x17\x01\x18\x01\x19\x01\x1a\x01\x1b\x01\x1c\x01\x1d\x01"+
        "\x1e\x01\x1f\x01\x20\x01\x21\x01\x22\x01\x23\x01\x24\x01\x25\x01"+
        "\x26\x6c\uffff}>";
    static readonly string[] DFA22_transitionS = {
            "\x01\x06\x03\x09\x08\x07\x02\x09\x01\x07\x02\x09\x07\uffff"+
            "\x01\x01\x01\uffff\x01\x02\x01\uffff\x01\x03\x03\uffff\x01\x04"+
            "\x09\uffff\x01\x05\x01\x08",
            "\x01\x11\x02\x14\x01\x0a\x08\x12\x02\x14\x01\x12\x02\x14\x07"+
            "\uffff\x01\x0c\x01\uffff\x01\x0d\x01\uffff\x01\x0e\x03\uffff"+
            "\x01\x0f\x01\x0b\x08\uffff\x01\x10\x01\x13",
            "\x01\x1c\x02\x1f\x01\x15\x08\x1d\x02\x1f\x01\x1d\x02\x1f\x07"+
            "\uffff\x01\x17\x01\uffff\x01\x18\x01\uffff\x01\x19\x03\uffff"+
            "\x01\x1a\x01\x16\x08\uffff\x01\x1b\x01\x1e",
            "\x01\x27\x02\x2a\x01\x20\x08\x28\x02\x2a\x01\x28\x02\x2a\x07"+
            "\uffff\x01\x22\x01\uffff\x01\x23\x01\uffff\x01\x24\x03\uffff"+
            "\x01\x25\x01\x21\x08\uffff\x01\x26\x01\x29",
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
            get { return "285:1: dollarExpression : ( primaryExpression conditional -> ^( ASTDOLLAREXPRESSION primaryExpression conditional ) | primaryExpression );"; }
        }

    }


    protected internal int DFA22_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA22_4 = input.LA(1);

                   	 
                   	int index22_4 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_4);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA22_5 = input.LA(1);

                   	 
                   	int index22_5 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_5);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA22_6 = input.LA(1);

                   	 
                   	int index22_6 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_6);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA22_7 = input.LA(1);

                   	 
                   	int index22_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_7);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA22_8 = input.LA(1);

                   	 
                   	int index22_8 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_8);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA22_9 = input.LA(1);

                   	 
                   	int index22_9 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_9);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 6 : 
                   	int LA22_10 = input.LA(1);

                   	 
                   	int index22_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_10);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 7 : 
                   	int LA22_11 = input.LA(1);

                   	 
                   	int index22_11 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_11);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 8 : 
                   	int LA22_12 = input.LA(1);

                   	 
                   	int index22_12 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_12);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 9 : 
                   	int LA22_13 = input.LA(1);

                   	 
                   	int index22_13 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_13);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 10 : 
                   	int LA22_14 = input.LA(1);

                   	 
                   	int index22_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_14);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 11 : 
                   	int LA22_15 = input.LA(1);

                   	 
                   	int index22_15 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_15);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 12 : 
                   	int LA22_16 = input.LA(1);

                   	 
                   	int index22_16 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_16);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 13 : 
                   	int LA22_17 = input.LA(1);

                   	 
                   	int index22_17 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_17);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 14 : 
                   	int LA22_18 = input.LA(1);

                   	 
                   	int index22_18 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_18);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 15 : 
                   	int LA22_19 = input.LA(1);

                   	 
                   	int index22_19 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_19);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 16 : 
                   	int LA22_20 = input.LA(1);

                   	 
                   	int index22_20 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_20);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 17 : 
                   	int LA22_21 = input.LA(1);

                   	 
                   	int index22_21 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_21);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 18 : 
                   	int LA22_22 = input.LA(1);

                   	 
                   	int index22_22 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_22);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 19 : 
                   	int LA22_23 = input.LA(1);

                   	 
                   	int index22_23 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_23);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 20 : 
                   	int LA22_24 = input.LA(1);

                   	 
                   	int index22_24 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_24);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 21 : 
                   	int LA22_25 = input.LA(1);

                   	 
                   	int index22_25 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_25);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 22 : 
                   	int LA22_26 = input.LA(1);

                   	 
                   	int index22_26 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_26);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 23 : 
                   	int LA22_27 = input.LA(1);

                   	 
                   	int index22_27 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_27);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 24 : 
                   	int LA22_28 = input.LA(1);

                   	 
                   	int index22_28 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_28);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 25 : 
                   	int LA22_29 = input.LA(1);

                   	 
                   	int index22_29 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_29);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 26 : 
                   	int LA22_30 = input.LA(1);

                   	 
                   	int index22_30 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_30);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 27 : 
                   	int LA22_31 = input.LA(1);

                   	 
                   	int index22_31 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_31);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 28 : 
                   	int LA22_32 = input.LA(1);

                   	 
                   	int index22_32 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_32);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 29 : 
                   	int LA22_33 = input.LA(1);

                   	 
                   	int index22_33 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_33);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 30 : 
                   	int LA22_34 = input.LA(1);

                   	 
                   	int index22_34 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_34);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 31 : 
                   	int LA22_35 = input.LA(1);

                   	 
                   	int index22_35 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_35);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 32 : 
                   	int LA22_36 = input.LA(1);

                   	 
                   	int index22_36 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_36);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 33 : 
                   	int LA22_37 = input.LA(1);

                   	 
                   	int index22_37 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_37);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 34 : 
                   	int LA22_38 = input.LA(1);

                   	 
                   	int index22_38 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_38);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 35 : 
                   	int LA22_39 = input.LA(1);

                   	 
                   	int index22_39 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_39);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 36 : 
                   	int LA22_40 = input.LA(1);

                   	 
                   	int index22_40 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_40);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 37 : 
                   	int LA22_41 = input.LA(1);

                   	 
                   	int index22_41 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_41);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 38 : 
                   	int LA22_42 = input.LA(1);

                   	 
                   	int index22_42 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred48_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index22_42);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae22 =
            new NoViableAltException(dfa.Description, 22, _s, input);
        dfa.Error(nvae22);
        throw nvae22;
    }
    const string DFA23_eotS =
        "\x0a\uffff";
    const string DFA23_eofS =
        "\x0a\uffff";
    const string DFA23_minS =
        "\x01\x45\x09\uffff";
    const string DFA23_maxS =
        "\x01\x70\x09\uffff";
    const string DFA23_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x05\uffff";
    const string DFA23_specialS =
        "\x0a\uffff}>";
    static readonly string[] DFA23_transitionS = {
            "\x11\x04\x07\uffff\x01\x01\x01\uffff\x01\x02\x01\uffff\x01"+
            "\x03\x03\uffff\x01\x04\x09\uffff\x02\x04",
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

    static readonly short[] DFA23_eot = DFA.UnpackEncodedString(DFA23_eotS);
    static readonly short[] DFA23_eof = DFA.UnpackEncodedString(DFA23_eofS);
    static readonly char[] DFA23_min = DFA.UnpackEncodedStringToUnsignedChars(DFA23_minS);
    static readonly char[] DFA23_max = DFA.UnpackEncodedStringToUnsignedChars(DFA23_maxS);
    static readonly short[] DFA23_accept = DFA.UnpackEncodedString(DFA23_acceptS);
    static readonly short[] DFA23_special = DFA.UnpackEncodedString(DFA23_specialS);
    static readonly short[][] DFA23_transition = DFA.UnpackEncodedStringArray(DFA23_transitionS);

    protected class DFA23 : DFA
    {
        public DFA23(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 23;
            this.eot = DFA23_eot;
            this.eof = DFA23_eof;
            this.min = DFA23_min;
            this.max = DFA23_max;
            this.accept = DFA23_accept;
            this.special = DFA23_special;
            this.transition = DFA23_transition;

        }

        override public string Description
        {
            get { return "290:1: primaryExpression : ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value );"; }
        }

    }

    const string DFA24_eotS =
        "\x2f\uffff";
    const string DFA24_eofS =
        "\x03\uffff\x02\x05\x2a\uffff";
    const string DFA24_minS =
        "\x01\x45\x02\uffff\x02\x46\x02\uffff\x03\x00\x10\uffff\x01\x00"+
        "\x01\uffff\x02\x00\x11\uffff";
    const string DFA24_maxS =
        "\x01\x70\x02\uffff\x02\x6e\x02\uffff\x03\x00\x10\uffff\x01\x00"+
        "\x01\uffff\x02\x00\x11\uffff";
    const string DFA24_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x02\uffff\x01\x05\x27\uffff\x01\x03"+
        "\x01\x04";
    const string DFA24_specialS =
        "\x07\uffff\x01\x00\x01\x01\x01\x02\x10\uffff\x01\x03\x01\uffff"+
        "\x01\x04\x01\x05\x11\uffff}>";
    static readonly string[] DFA24_transitionS = {
            "\x01\x03\x03\x05\x08\x04\x02\x05\x01\x04\x02\x05\x0f\uffff"+
            "\x01\x01\x09\uffff\x01\x02\x01\x05",
            "",
            "",
            "\x02\x05\x0f\uffff\x06\x05\x01\x07\x01\x05\x01\x08\x01\x05"+
            "\x01\x09\x01\x05\x01\uffff\x01\x05\x01\uffff\x09\x05",
            "\x02\x05\x0f\uffff\x06\x05\x01\x1c\x01\x05\x01\x1d\x01\x05"+
            "\x01\x1a\x01\x05\x01\uffff\x01\x05\x01\uffff\x09\x05",
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
            "",
            "",
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
            get { return "298:1: value : ( Integer -> ^( ASTVALUE Integer ) | Double -> ^( ASTVALUE Double ) | sum -> ^( ASTVALUE sum ) | function -> ^( ASTVALUE function ) | variableWithIndexerEtc -> ^( ASTVALUE variableWithIndexerEtc ) );"; }
        }

    }


    protected internal int DFA24_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA24_7 = input.LA(1);

                   	 
                   	int index24_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred59_GAMS()) ) { s = 45; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index24_7);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA24_8 = input.LA(1);

                   	 
                   	int index24_8 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred59_GAMS()) ) { s = 45; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index24_8);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA24_9 = input.LA(1);

                   	 
                   	int index24_9 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred59_GAMS()) ) { s = 45; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index24_9);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA24_26 = input.LA(1);

                   	 
                   	int index24_26 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred60_GAMS()) ) { s = 46; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index24_26);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA24_28 = input.LA(1);

                   	 
                   	int index24_28 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred60_GAMS()) ) { s = 46; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index24_28);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA24_29 = input.LA(1);

                   	 
                   	int index24_29 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred60_GAMS()) ) { s = 46; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index24_29);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae24 =
            new NoViableAltException(dfa.Description, 24, _s, input);
        dfa.Error(nvae24);
        throw nvae24;
    }
 

    public static readonly BitSet FOLLOW_set_in_extraTokens0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr_in_gams493 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010000003FFFE0UL});
    public static readonly BitSet FOLLOW_EOF_in_gams496 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_equ_in_expr517 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_vardef_in_expr527 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variables_in_expr534 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_equations_in_expr541 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_model_in_expr548 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_solve_in_expr555 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithIndexerSimple_in_equ569 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_DOUBLEDOT_in_equ571 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_expression_in_equ573 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000800000UL});
    public static readonly BitSet FOLLOW_EEQUAL_in_equ575 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_expression_in_equ577 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_SEMI_in_equ579 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithIndexerEtc_in_vardef627 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_EQUAL_in_vardef629 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_expression_in_vardef631 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_SEMI_in_vardef633 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_VARIABLES_in_variables669 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010000003FFFE0UL});
    public static readonly BitSet FOLLOW_variableWithIndexerSimple_in_variables671 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000005000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_variables674 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010000003FFFE0UL});
    public static readonly BitSet FOLLOW_variableWithIndexerSimple_in_variables676 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000005000000UL});
    public static readonly BitSet FOLLOW_SEMI_in_variables680 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_VARIABLES_in_equations692 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010000003FFFE0UL});
    public static readonly BitSet FOLLOW_variableWithIndexerSimple_in_equations694 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000005000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_equations697 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010000003FFFE0UL});
    public static readonly BitSet FOLLOW_variableWithIndexerSimple_in_equations699 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000005000000UL});
    public static readonly BitSet FOLLOW_SEMI_in_equations703 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MODEL_in_model715 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010000093FFFE0UL});
    public static readonly BitSet FOLLOW_ident_in_model718 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010000093FFFE0UL});
    public static readonly BitSet FOLLOW_DIV_in_model722 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010000093FFFE0UL});
    public static readonly BitSet FOLLOW_SEMI_in_model726 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SOLVE_in_solve736 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010000013FFFE0UL});
    public static readonly BitSet FOLLOW_ident_in_solve738 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010000013FFFE0UL});
    public static readonly BitSet FOLLOW_SEMI_in_solve741 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableWithIndexerSimple755 = new BitSet(new ulong[]{0x0000000000000002UL,0x00000002A0000000UL});
    public static readonly BitSet FOLLOW_idx_in_variableWithIndexerSimple757 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableWithIndexerEtc776 = new BitSet(new ulong[]{0x0000000000000002UL,0x00000082B0000000UL});
    public static readonly BitSet FOLLOW_DOT_in_variableWithIndexerEtc779 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010000003FFFE0UL});
    public static readonly BitSet FOLLOW_variable_in_variableWithIndexerEtc781 = new BitSet(new ulong[]{0x0000000000000002UL,0x00000082A0000000UL});
    public static readonly BitSet FOLLOW_idx_in_variableWithIndexerEtc785 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000008000000000UL});
    public static readonly BitSet FOLLOW_conditional_in_variableWithIndexerEtc788 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_variable839 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_idx846 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010008003FFFE0UL});
    public static readonly BitSet FOLLOW_indexerElements_in_idx848 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000040000000UL});
    public static readonly BitSet FOLLOW_R1_in_idx850 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_idx873 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010008003FFFE0UL});
    public static readonly BitSet FOLLOW_indexerElements_in_idx875 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000100000000UL});
    public static readonly BitSet FOLLOW_R2_in_idx877 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_idx900 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010008003FFFE0UL});
    public static readonly BitSet FOLLOW_indexerElements_in_idx902 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000400000000UL});
    public static readonly BitSet FOLLOW_R3_in_idx904 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableLagLead_in_indexerElements928 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_indexerElements931 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010008003FFFE0UL});
    public static readonly BitSet FOLLOW_variableLagLead_in_indexerElements933 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_StringInQuotes_in_variableLagLead965 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead1014 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000001000000000UL});
    public static readonly BitSet FOLLOW_PLUS_in_variableLagLead1016 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000002000000000UL});
    public static readonly BitSet FOLLOW_Integer_in_variableLagLead1018 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead1064 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000004000000000UL});
    public static readonly BitSet FOLLOW_MINUS_in_variableLagLead1066 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000002000000000UL});
    public static readonly BitSet FOLLOW_Integer_in_variableLagLead1068 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead1104 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DOLLAR_in_conditional1148 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_expression_in_conditional1150 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_andExpression_in_expression1183 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000080UL});
    public static readonly BitSet FOLLOW_OR_in_expression1186 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_andExpression_in_expression1189 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000080UL});
    public static readonly BitSet FOLLOW_notExpression_in_andExpression1198 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000040UL});
    public static readonly BitSet FOLLOW_AND_in_andExpression1201 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_notExpression_in_andExpression1204 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000040UL});
    public static readonly BitSet FOLLOW_NOT_in_notExpression1215 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_logicalExpression_in_notExpression1217 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_logicalExpression_in_notExpression1237 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_logicalExpression1245 = new BitSet(new ulong[]{0x0000000000000002UL,0x00007C0002000000UL});
    public static readonly BitSet FOLLOW_logical_in_logicalExpression1248 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_logicalExpression1251 = new BitSet(new ulong[]{0x0000000000000002UL,0x00007C0002000000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression1260 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000005000000000UL});
    public static readonly BitSet FOLLOW_set_in_additiveExpression1264 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression1271 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000005000000000UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression1281 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000010008000000UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpression1285 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression1292 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000010008000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression1302 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000020000000000UL});
    public static readonly BitSet FOLLOW_STARS_in_powerExpression1306 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression1309 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000020000000000UL});
    public static readonly BitSet FOLLOW_MINUS_in_unaryExpression1320 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_dollarExpression_in_unaryExpression1322 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_dollarExpression_in_unaryExpression1343 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_dollarExpression1368 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000008000000000UL});
    public static readonly BitSet FOLLOW_conditional_in_dollarExpression1370 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_dollarExpression1390 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_primaryExpression1411 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression1413 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000040000000UL});
    public static readonly BitSet FOLLOW_R1_in_primaryExpression1415 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_primaryExpression1430 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression1432 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000100000000UL});
    public static readonly BitSet FOLLOW_R2_in_primaryExpression1434 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_primaryExpression1451 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression1453 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000400000000UL});
    public static readonly BitSet FOLLOW_R3_in_primaryExpression1455 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_value_in_primaryExpression1470 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_logical0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Integer_in_value1511 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Double_in_value1542 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_sum_in_value1576 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_value1613 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithIndexerEtc_in_value1661 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_function1693 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000020000000UL});
    public static readonly BitSet FOLLOW_L1_in_function1695 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_functionElements_in_function1697 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000040000000UL});
    public static readonly BitSet FOLLOW_R1_in_function1699 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_function1741 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000080000000UL});
    public static readonly BitSet FOLLOW_L2_in_function1743 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_functionElements_in_function1745 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000100000000UL});
    public static readonly BitSet FOLLOW_R2_in_function1747 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_function1789 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000200000000UL});
    public static readonly BitSet FOLLOW_L3_in_function1791 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_functionElements_in_function1793 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000400000000UL});
    public static readonly BitSet FOLLOW_R3_in_function1795 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_functionName0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_functionElements1870 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_functionElements1873 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_expression_in_functionElements1875 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_SUM_in_sum1908 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000020000000UL});
    public static readonly BitSet FOLLOW_L1_in_sum1910 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010002A03FFFE0UL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum1912 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000008004000000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum1914 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum1917 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_expression_in_sum1919 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000040000000UL});
    public static readonly BitSet FOLLOW_R1_in_sum1921 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUM_in_sum1965 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000080000000UL});
    public static readonly BitSet FOLLOW_L2_in_sum1967 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010002A03FFFE0UL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum1969 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000008004000000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum1971 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum1974 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_expression_in_sum1976 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000100000000UL});
    public static readonly BitSet FOLLOW_R2_in_sum1978 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUM_in_sum2022 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000200000000UL});
    public static readonly BitSet FOLLOW_L3_in_sum2024 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010002A03FFFE0UL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum2026 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000008004000000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum2028 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum2031 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_expression_in_sum2033 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000400000000UL});
    public static readonly BitSet FOLLOW_R3_in_sum2035 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_sumControlled2090 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_sumControlled2117 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010008003FFFE0UL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled2119 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000040000000UL});
    public static readonly BitSet FOLLOW_R1_in_sumControlled2121 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_sumControlled2147 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010008003FFFE0UL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled2149 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000100000000UL});
    public static readonly BitSet FOLLOW_R2_in_sumControlled2151 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_sumControlled2177 = new BitSet(new ulong[]{0x0000000000000000UL,0x00010008003FFFE0UL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled2179 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000400000000UL});
    public static readonly BitSet FOLLOW_R3_in_sumControlled2181 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Ident_in_ident2217 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_extraTokens_in_ident2221 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_equ_in_synpred18_GAMS517 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_vardef_in_synpred19_GAMS527 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variables_in_synpred20_GAMS534 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_equations_in_synpred21_GAMS541 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_conditional_in_synpred31_GAMS788 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_OR_in_synpred38_GAMS1186 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_andExpression_in_synpred38_GAMS1189 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_AND_in_synpred39_GAMS1201 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_notExpression_in_synpred39_GAMS1204 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NOT_in_synpred40_GAMS1215 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_logicalExpression_in_synpred40_GAMS1217 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_logical_in_synpred41_GAMS1248 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_synpred41_GAMS1251 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_synpred43_GAMS1264 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_synpred43_GAMS1271 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_synpred45_GAMS1285 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_powerExpression_in_synpred45_GAMS1292 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STARS_in_synpred46_GAMS1306 = new BitSet(new ulong[]{0x0000000000000000UL,0x00018062A03FFFE0UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_synpred46_GAMS1309 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_synpred48_GAMS1368 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000008000000000UL});
    public static readonly BitSet FOLLOW_conditional_in_synpred48_GAMS1370 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_sum_in_synpred59_GAMS1576 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_synpred60_GAMS1613 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}