// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 GAMS.g 2022-04-06 17:01:24

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
		"SAMEAS", 
		"VARIABLES", 
		"EQUATIONS", 
		"DOUBLEDOT", 
		"EEQUAL", 
		"SEMI", 
		"EQUAL", 
		"COMMA", 
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
		"DIV", 
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

    public const int ASTPOW = 20;
    public const int ASTFUNCTIONELEMENTS1 = 59;
    public const int COMMENT1 = 114;
    public const int COMMENT2 = 115;
    public const int ASTFUNCTION3 = 34;
    public const int ASTFUNCTION2 = 33;
    public const int ASTVARIABLE = 21;
    public const int LETTER = 121;
    public const int MOD = 118;
    public const int ASTFUNCTION1 = 32;
    public const int ASTIDX = 38;
    public const int ASTFUNCTION0 = 56;
    public const int LOG = 74;
    public const int EQUATIONS = 82;
    public const int ASTINDEXES1 = 23;
    public const int ASTCONDITIONAL = 30;
    public const int DOUBLEDOT = 83;
    public const int ASTINDEXES3 = 25;
    public const int NOT = 71;
    public const int ASTVAR = 35;
    public const int ASTINDEXES2 = 24;
    public const int EOF = -1;
    public const int NONEQUAL = 103;
    public const int ASTINTEGER = 18;
    public const int TANH = 79;
    public const int ASTEQU = 10;
    public const int Comment = 112;
    public const int EXP = 73;
    public const int ASTDOLLAREXPRESSION = 54;
    public const int EEQUAL = 84;
    public const int SQR = 78;
    public const int GREATERTHANOREQUAL = 105;
    public const int ASTFUNCTIONELEMENTS0 = 58;
    public const int GREATERTHAN = 107;
    public const int D = 126;
    public const int Double = 108;
    public const int ASTEQU1 = 40;
    public const int E = 127;
    public const int ASTEQU2 = 41;
    public const int F = 128;
    public const int G = 129;
    public const int ASTEQU0 = 39;
    public const int A = 123;
    public const int B = 124;
    public const int ASTEQU3 = 42;
    public const int C = 125;
    public const int L = 134;
    public const int M = 135;
    public const int N = 136;
    public const int NESTED_ML_COMMENT = 113;
    public const int ASTVARWI4 = 48;
    public const int O = 137;
    public const int ASTVARWI3 = 47;
    public const int H = 130;
    public const int ASTVARWI2 = 46;
    public const int ASTFUNCTIONELEMENTS = 57;
    public const int I = 131;
    public const int ASTVARWISIMPLE = 5;
    public const int ASTVARWI1 = 45;
    public const int J = 132;
    public const int NEWLINE2 = 110;
    public const int ASTVARWI0 = 44;
    public const int K = 133;
    public const int NEWLINE3 = 111;
    public const int U = 143;
    public const int T = 142;
    public const int W = 145;
    public const int WHITESPACE = 116;
    public const int POWER = 77;
    public const int V = 144;
    public const int Q = 139;
    public const int P = 138;
    public const int S = 141;
    public const int R = 140;
    public const int MULT = 100;
    public const int ASTVARWI = 43;
    public const int Y = 147;
    public const int ASTIDXELEMENTS1 = 51;
    public const int X = 146;
    public const int ASTIDXELEMENTS0 = 52;
    public const int Z = 148;
    public const int ABS = 72;
    public const int Ident = 109;
    public const int ASTEXPRESSION = 13;
    public const int OR = 70;
    public const int StringInQuotes = 95;
    public const int ASTSUM = 31;
    public const int ASTDEFINITION = 29;
    public const int DOLLAR = 99;
    public const int ASTFUNCTION = 17;
    public const int ASTEQUCODE = 12;
    public const int MAX = 75;
    public const int Exponent = 120;
    public const int R2 = 92;
    public const int R3 = 94;
    public const int SUM = 68;
    public const int AND = 69;
    public const int COMMA = 87;
    public const int R1 = 90;
    public const int ASTSIMPLEFUNCTION1 = 14;
    public const int EQUAL = 86;
    public const int ASTSIMPLEFUNCTION2 = 15;
    public const int LESSTHANOREQUAL = 104;
    public const int ASTSIMPLEFUNCTION3 = 16;
    public const int PLUS = 96;
    public const int ASTEND = 22;
    public const int DIGIT = 119;
    public const int ASTSUMCONTROLLED = 65;
    public const int DOT = 88;
    public const int ASTSUMCONTROLLEDSIMPLE = 64;
    public const int ASTEXPRESSION2 = 27;
    public const int ASTEXPRESSION3 = 28;
    public const int ASTIDXELEMENTS = 50;
    public const int LESSTHAN = 106;
    public const int ASTVALUE = 55;
    public const int ASTVARIABLEWITHINDEXERETC = 36;
    public const int ASTVARDEF = 6;
    public const int ASTIDX0 = 49;
    public const int ASTEXPRESSION1 = 26;
    public const int NEGATE = 4;
    public const int SAMEAS = 80;
    public const int MIN = 76;
    public const int MINUS = 98;
    public const int ASTVARDEF0 = 7;
    public const int ASTVARDEF1 = 8;
    public const int ASTVARIABLEANDLEAD = 53;
    public const int ASTVARDEF2 = 9;
    public const int SEMI = 85;
    public const int L1 = 89;
    public const int ASTSUM0 = 60;
    public const int L2 = 91;
    public const int L3 = 93;
    public const int ASTLEFTSIDE = 11;
    public const int NEWLINE = 122;
    public const int ASTSUM2 = 62;
    public const int ASTSUMCONTROLLED2 = 67;
    public const int ASTSUM1 = 61;
    public const int ASTSUMCONTROLLED0 = 66;
    public const int ASTSUM3 = 63;
    public const int VARIABLES = 81;
    public const int EQU = 117;
    public const int STARS = 102;
    public const int ASTDOUBLE = 19;
    public const int DIV = 101;
    public const int Integer = 97;
    public const int ASTDOT = 37;

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
    // GAMS.g:167:1: extraTokens : ( SUM | AND | OR | NOT | ABS | EXP | LOG | MAX | MIN | POWER | SQR | TANH | SAMEAS | VARIABLES | EQUATIONS );
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
            // GAMS.g:167:13: ( SUM | AND | OR | NOT | ABS | EXP | LOG | MAX | MIN | POWER | SQR | TANH | SAMEAS | VARIABLES | EQUATIONS )
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
    // GAMS.g:189:1: gams : ( expr )* EOF ;
    public GAMSParser.gams_return gams() // throws RecognitionException [1]
    {   
        GAMSParser.gams_return retval = new GAMSParser.gams_return();
        retval.Start = input.LT(1);
        int gams_StartIndex = input.Index();
        object root_0 = null;

        IToken EOF3 = null;
        GAMSParser.expr_return expr2 = default(GAMSParser.expr_return);


        object EOF3_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 2) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:189:5: ( ( expr )* EOF )
            // GAMS.g:189:7: ( expr )* EOF
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// GAMS.g:189:7: ( expr )*
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
            			    	PushFollow(FOLLOW_expr_in_gams481);
            			    	expr2 = expr();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expr2.Tree);

            			    }
            			    break;

            			default:
            			    goto loop1;
            	    }
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements

            	EOF3=(IToken)Match(input,EOF,FOLLOW_EOF_in_gams484); if (state.failed) return retval;
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
    // GAMS.g:191:1: expr : ( equ | vardef | variables | equations );
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



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 3) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:191:5: ( equ | vardef | variables | equations )
            int alt2 = 4;
            alt2 = dfa2.Predict(input);
            switch (alt2) 
            {
                case 1 :
                    // GAMS.g:191:7: equ
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_equ_in_expr496);
                    	equ4 = equ();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, equ4.Tree);

                    }
                    break;
                case 2 :
                    // GAMS.g:192:9: vardef
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_vardef_in_expr506);
                    	vardef5 = vardef();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, vardef5.Tree);

                    }
                    break;
                case 3 :
                    // GAMS.g:193:6: variables
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variables_in_expr513);
                    	variables6 = variables();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variables6.Tree);

                    }
                    break;
                case 4 :
                    // GAMS.g:194:6: equations
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_equations_in_expr520);
                    	equations7 = equations();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, equations7.Tree);

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
    // GAMS.g:197:1: equ : variableWithIndexerSimple DOUBLEDOT expression EEQUAL expression SEMI -> ^( ASTEQU ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI ) ^( ASTEQU1 variableWithIndexerSimple ) ^( ASTEQU2 expression ) ^( ASTEQU3 expression ) ) ;
    public GAMSParser.equ_return equ() // throws RecognitionException [1]
    {   
        GAMSParser.equ_return retval = new GAMSParser.equ_return();
        retval.Start = input.LT(1);
        int equ_StartIndex = input.Index();
        object root_0 = null;

        IToken DOUBLEDOT9 = null;
        IToken EEQUAL11 = null;
        IToken SEMI13 = null;
        GAMSParser.variableWithIndexerSimple_return variableWithIndexerSimple8 = default(GAMSParser.variableWithIndexerSimple_return);

        GAMSParser.expression_return expression10 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression12 = default(GAMSParser.expression_return);


        object DOUBLEDOT9_tree=null;
        object EEQUAL11_tree=null;
        object SEMI13_tree=null;
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
            // GAMS.g:197:4: ( variableWithIndexerSimple DOUBLEDOT expression EEQUAL expression SEMI -> ^( ASTEQU ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI ) ^( ASTEQU1 variableWithIndexerSimple ) ^( ASTEQU2 expression ) ^( ASTEQU3 expression ) ) )
            // GAMS.g:197:9: variableWithIndexerSimple DOUBLEDOT expression EEQUAL expression SEMI
            {
            	PushFollow(FOLLOW_variableWithIndexerSimple_in_equ534);
            	variableWithIndexerSimple8 = variableWithIndexerSimple();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableWithIndexerSimple.Add(variableWithIndexerSimple8.Tree);
            	DOUBLEDOT9=(IToken)Match(input,DOUBLEDOT,FOLLOW_DOUBLEDOT_in_equ536); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_DOUBLEDOT.Add(DOUBLEDOT9);

            	PushFollow(FOLLOW_expression_in_equ538);
            	expression10 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression10.Tree);
            	EEQUAL11=(IToken)Match(input,EEQUAL,FOLLOW_EEQUAL_in_equ540); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_EEQUAL.Add(EEQUAL11);

            	PushFollow(FOLLOW_expression_in_equ542);
            	expression12 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression12.Tree);
            	SEMI13=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_equ544); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI13);

            	if ( (state.backtracking==0) )
            	{
            	  equItems.Add(input.ToString((IToken)retval.Start,input.LT(-1)));
            	}


            	// AST REWRITE
            	// elements:          SEMI, EEQUAL, DOUBLEDOT, expression, expression, variableWithIndexerSimple
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 198:3: -> ^( ASTEQU ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI ) ^( ASTEQU1 variableWithIndexerSimple ) ^( ASTEQU2 expression ) ^( ASTEQU3 expression ) )
            	{
            	    // GAMS.g:198:6: ^( ASTEQU ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI ) ^( ASTEQU1 variableWithIndexerSimple ) ^( ASTEQU2 expression ) ^( ASTEQU3 expression ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU, "ASTEQU"), root_1);

            	    // GAMS.g:198:15: ^( ASTEQU0 DOUBLEDOT EEQUAL SEMI )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU0, "ASTEQU0"), root_2);

            	    adaptor.AddChild(root_2, stream_DOUBLEDOT.NextNode());
            	    adaptor.AddChild(root_2, stream_EEQUAL.NextNode());
            	    adaptor.AddChild(root_2, stream_SEMI.NextNode());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:198:48: ^( ASTEQU1 variableWithIndexerSimple )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU1, "ASTEQU1"), root_2);

            	    adaptor.AddChild(root_2, stream_variableWithIndexerSimple.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:198:85: ^( ASTEQU2 expression )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTEQU2, "ASTEQU2"), root_2);

            	    adaptor.AddChild(root_2, stream_expression.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:198:107: ^( ASTEQU3 expression )
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
    // GAMS.g:200:1: vardef : variableWithIndexerEtc EQUAL expression SEMI -> ^( ASTVARDEF ^( ASTVARDEF0 EQUAL SEMI ) ^( ASTVARDEF1 variableWithIndexerEtc ) ^( ASTVARDEF2 expression ) ) ;
    public GAMSParser.vardef_return vardef() // throws RecognitionException [1]
    {   
        GAMSParser.vardef_return retval = new GAMSParser.vardef_return();
        retval.Start = input.LT(1);
        int vardef_StartIndex = input.Index();
        object root_0 = null;

        IToken EQUAL15 = null;
        IToken SEMI17 = null;
        GAMSParser.variableWithIndexerEtc_return variableWithIndexerEtc14 = default(GAMSParser.variableWithIndexerEtc_return);

        GAMSParser.expression_return expression16 = default(GAMSParser.expression_return);


        object EQUAL15_tree=null;
        object SEMI17_tree=null;
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
            // GAMS.g:200:7: ( variableWithIndexerEtc EQUAL expression SEMI -> ^( ASTVARDEF ^( ASTVARDEF0 EQUAL SEMI ) ^( ASTVARDEF1 variableWithIndexerEtc ) ^( ASTVARDEF2 expression ) ) )
            // GAMS.g:200:11: variableWithIndexerEtc EQUAL expression SEMI
            {
            	PushFollow(FOLLOW_variableWithIndexerEtc_in_vardef592);
            	variableWithIndexerEtc14 = variableWithIndexerEtc();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableWithIndexerEtc.Add(variableWithIndexerEtc14.Tree);
            	EQUAL15=(IToken)Match(input,EQUAL,FOLLOW_EQUAL_in_vardef594); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_EQUAL.Add(EQUAL15);

            	PushFollow(FOLLOW_expression_in_vardef596);
            	expression16 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression16.Tree);
            	SEMI17=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_vardef598); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI17);



            	// AST REWRITE
            	// elements:          SEMI, expression, EQUAL, variableWithIndexerEtc
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 201:3: -> ^( ASTVARDEF ^( ASTVARDEF0 EQUAL SEMI ) ^( ASTVARDEF1 variableWithIndexerEtc ) ^( ASTVARDEF2 expression ) )
            	{
            	    // GAMS.g:201:6: ^( ASTVARDEF ^( ASTVARDEF0 EQUAL SEMI ) ^( ASTVARDEF1 variableWithIndexerEtc ) ^( ASTVARDEF2 expression ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARDEF, "ASTVARDEF"), root_1);

            	    // GAMS.g:201:18: ^( ASTVARDEF0 EQUAL SEMI )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARDEF0, "ASTVARDEF0"), root_2);

            	    adaptor.AddChild(root_2, stream_EQUAL.NextNode());
            	    adaptor.AddChild(root_2, stream_SEMI.NextNode());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:201:43: ^( ASTVARDEF1 variableWithIndexerEtc )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARDEF1, "ASTVARDEF1"), root_2);

            	    adaptor.AddChild(root_2, stream_variableWithIndexerEtc.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:201:80: ^( ASTVARDEF2 expression )
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
    // GAMS.g:203:1: variables : VARIABLES variableWithIndexerSimple ( COMMA variableWithIndexerSimple )* SEMI -> ( variableWithIndexerSimple )+ ;
    public GAMSParser.variables_return variables() // throws RecognitionException [1]
    {   
        GAMSParser.variables_return retval = new GAMSParser.variables_return();
        retval.Start = input.LT(1);
        int variables_StartIndex = input.Index();
        object root_0 = null;

        IToken VARIABLES18 = null;
        IToken COMMA20 = null;
        IToken SEMI22 = null;
        GAMSParser.variableWithIndexerSimple_return variableWithIndexerSimple19 = default(GAMSParser.variableWithIndexerSimple_return);

        GAMSParser.variableWithIndexerSimple_return variableWithIndexerSimple21 = default(GAMSParser.variableWithIndexerSimple_return);


        object VARIABLES18_tree=null;
        object COMMA20_tree=null;
        object SEMI22_tree=null;
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
            // GAMS.g:203:10: ( VARIABLES variableWithIndexerSimple ( COMMA variableWithIndexerSimple )* SEMI -> ( variableWithIndexerSimple )+ )
            // GAMS.g:203:12: VARIABLES variableWithIndexerSimple ( COMMA variableWithIndexerSimple )* SEMI
            {
            	VARIABLES18=(IToken)Match(input,VARIABLES,FOLLOW_VARIABLES_in_variables634); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_VARIABLES.Add(VARIABLES18);

            	PushFollow(FOLLOW_variableWithIndexerSimple_in_variables636);
            	variableWithIndexerSimple19 = variableWithIndexerSimple();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableWithIndexerSimple.Add(variableWithIndexerSimple19.Tree);
            	// GAMS.g:203:48: ( COMMA variableWithIndexerSimple )*
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
            			    // GAMS.g:203:49: COMMA variableWithIndexerSimple
            			    {
            			    	COMMA20=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_variables639); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA20);

            			    	PushFollow(FOLLOW_variableWithIndexerSimple_in_variables641);
            			    	variableWithIndexerSimple21 = variableWithIndexerSimple();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_variableWithIndexerSimple.Add(variableWithIndexerSimple21.Tree);

            			    }
            			    break;

            			default:
            			    goto loop3;
            	    }
            	} while (true);

            	loop3:
            		;	// Stops C# compiler whining that label 'loop3' has no statements

            	SEMI22=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_variables645); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI22);



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
            	// 203:88: -> ( variableWithIndexerSimple )+
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
    // GAMS.g:205:1: equations : VARIABLES variableWithIndexerSimple ( COMMA variableWithIndexerSimple )* SEMI -> ( variableWithIndexerSimple )+ ;
    public GAMSParser.equations_return equations() // throws RecognitionException [1]
    {   
        GAMSParser.equations_return retval = new GAMSParser.equations_return();
        retval.Start = input.LT(1);
        int equations_StartIndex = input.Index();
        object root_0 = null;

        IToken VARIABLES23 = null;
        IToken COMMA25 = null;
        IToken SEMI27 = null;
        GAMSParser.variableWithIndexerSimple_return variableWithIndexerSimple24 = default(GAMSParser.variableWithIndexerSimple_return);

        GAMSParser.variableWithIndexerSimple_return variableWithIndexerSimple26 = default(GAMSParser.variableWithIndexerSimple_return);


        object VARIABLES23_tree=null;
        object COMMA25_tree=null;
        object SEMI27_tree=null;
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
            // GAMS.g:205:10: ( VARIABLES variableWithIndexerSimple ( COMMA variableWithIndexerSimple )* SEMI -> ( variableWithIndexerSimple )+ )
            // GAMS.g:205:12: VARIABLES variableWithIndexerSimple ( COMMA variableWithIndexerSimple )* SEMI
            {
            	VARIABLES23=(IToken)Match(input,VARIABLES,FOLLOW_VARIABLES_in_equations657); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_VARIABLES.Add(VARIABLES23);

            	PushFollow(FOLLOW_variableWithIndexerSimple_in_equations659);
            	variableWithIndexerSimple24 = variableWithIndexerSimple();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableWithIndexerSimple.Add(variableWithIndexerSimple24.Tree);
            	// GAMS.g:205:48: ( COMMA variableWithIndexerSimple )*
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
            			    // GAMS.g:205:49: COMMA variableWithIndexerSimple
            			    {
            			    	COMMA25=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_equations662); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA25);

            			    	PushFollow(FOLLOW_variableWithIndexerSimple_in_equations664);
            			    	variableWithIndexerSimple26 = variableWithIndexerSimple();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_variableWithIndexerSimple.Add(variableWithIndexerSimple26.Tree);

            			    }
            			    break;

            			default:
            			    goto loop4;
            	    }
            	} while (true);

            	loop4:
            		;	// Stops C# compiler whining that label 'loop4' has no statements

            	SEMI27=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_equations668); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI27);



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
            	// 205:88: -> ( variableWithIndexerSimple )+
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
    // GAMS.g:212:1: variableWithIndexerSimple : variable ( idx )? -> ^( ASTVARWISIMPLE variable ( idx )? ) ;
    public GAMSParser.variableWithIndexerSimple_return variableWithIndexerSimple() // throws RecognitionException [1]
    {   
        GAMSParser.variableWithIndexerSimple_return retval = new GAMSParser.variableWithIndexerSimple_return();
        retval.Start = input.LT(1);
        int variableWithIndexerSimple_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.variable_return variable28 = default(GAMSParser.variable_return);

        GAMSParser.idx_return idx29 = default(GAMSParser.idx_return);


        RewriteRuleSubtreeStream stream_idx = new RewriteRuleSubtreeStream(adaptor,"rule idx");
        RewriteRuleSubtreeStream stream_variable = new RewriteRuleSubtreeStream(adaptor,"rule variable");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 8) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:212:26: ( variable ( idx )? -> ^( ASTVARWISIMPLE variable ( idx )? ) )
            // GAMS.g:212:28: variable ( idx )?
            {
            	PushFollow(FOLLOW_variable_in_variableWithIndexerSimple685);
            	variable28 = variable();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variable.Add(variable28.Tree);
            	// GAMS.g:212:37: ( idx )?
            	int alt5 = 2;
            	int LA5_0 = input.LA(1);

            	if ( (LA5_0 == L1 || LA5_0 == L2 || LA5_0 == L3) )
            	{
            	    alt5 = 1;
            	}
            	switch (alt5) 
            	{
            	    case 1 :
            	        // GAMS.g:0:0: idx
            	        {
            	        	PushFollow(FOLLOW_idx_in_variableWithIndexerSimple687);
            	        	idx29 = idx();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_idx.Add(idx29.Tree);

            	        }
            	        break;

            	}



            	// AST REWRITE
            	// elements:          variable, idx
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 212:42: -> ^( ASTVARWISIMPLE variable ( idx )? )
            	{
            	    // GAMS.g:212:45: ^( ASTVARWISIMPLE variable ( idx )? )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWISIMPLE, "ASTVARWISIMPLE"), root_1);

            	    adaptor.AddChild(root_1, stream_variable.NextTree());
            	    // GAMS.g:212:71: ( idx )?
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
            	Memoize(input, 8, variableWithIndexerSimple_StartIndex); 
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
    // GAMS.g:214:1: variableWithIndexerEtc : variable ( DOT variable )? ( idx )? ( conditional )? -> ^( ASTVARWI ^( ASTVARWI0 ( DOT )? ) ^( ASTVARWI1 variable ) ^( ASTVARWI2 ( variable )? ) ^( ASTVARWI3 ( idx )? ) ^( ASTVARWI4 ( conditional )? ) ) ;
    public GAMSParser.variableWithIndexerEtc_return variableWithIndexerEtc() // throws RecognitionException [1]
    {   
        GAMSParser.variableWithIndexerEtc_return retval = new GAMSParser.variableWithIndexerEtc_return();
        retval.Start = input.LT(1);
        int variableWithIndexerEtc_StartIndex = input.Index();
        object root_0 = null;

        IToken DOT31 = null;
        GAMSParser.variable_return variable30 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable32 = default(GAMSParser.variable_return);

        GAMSParser.idx_return idx33 = default(GAMSParser.idx_return);

        GAMSParser.conditional_return conditional34 = default(GAMSParser.conditional_return);


        object DOT31_tree=null;
        RewriteRuleTokenStream stream_DOT = new RewriteRuleTokenStream(adaptor,"token DOT");
        RewriteRuleSubtreeStream stream_idx = new RewriteRuleSubtreeStream(adaptor,"rule idx");
        RewriteRuleSubtreeStream stream_conditional = new RewriteRuleSubtreeStream(adaptor,"rule conditional");
        RewriteRuleSubtreeStream stream_variable = new RewriteRuleSubtreeStream(adaptor,"rule variable");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 9) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:214:23: ( variable ( DOT variable )? ( idx )? ( conditional )? -> ^( ASTVARWI ^( ASTVARWI0 ( DOT )? ) ^( ASTVARWI1 variable ) ^( ASTVARWI2 ( variable )? ) ^( ASTVARWI3 ( idx )? ) ^( ASTVARWI4 ( conditional )? ) ) )
            // GAMS.g:214:25: variable ( DOT variable )? ( idx )? ( conditional )?
            {
            	PushFollow(FOLLOW_variable_in_variableWithIndexerEtc706);
            	variable30 = variable();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variable.Add(variable30.Tree);
            	// GAMS.g:214:34: ( DOT variable )?
            	int alt6 = 2;
            	alt6 = dfa6.Predict(input);
            	switch (alt6) 
            	{
            	    case 1 :
            	        // GAMS.g:214:35: DOT variable
            	        {
            	        	DOT31=(IToken)Match(input,DOT,FOLLOW_DOT_in_variableWithIndexerEtc709); if (state.failed) return retval; 
            	        	if ( (state.backtracking==0) ) stream_DOT.Add(DOT31);

            	        	PushFollow(FOLLOW_variable_in_variableWithIndexerEtc711);
            	        	variable32 = variable();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_variable.Add(variable32.Tree);

            	        }
            	        break;

            	}

            	// GAMS.g:214:50: ( idx )?
            	int alt7 = 2;
            	alt7 = dfa7.Predict(input);
            	switch (alt7) 
            	{
            	    case 1 :
            	        // GAMS.g:0:0: idx
            	        {
            	        	PushFollow(FOLLOW_idx_in_variableWithIndexerEtc715);
            	        	idx33 = idx();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_idx.Add(idx33.Tree);

            	        }
            	        break;

            	}

            	// GAMS.g:214:55: ( conditional )?
            	int alt8 = 2;
            	alt8 = dfa8.Predict(input);
            	switch (alt8) 
            	{
            	    case 1 :
            	        // GAMS.g:0:0: conditional
            	        {
            	        	PushFollow(FOLLOW_conditional_in_variableWithIndexerEtc718);
            	        	conditional34 = conditional();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional34.Tree);

            	        }
            	        break;

            	}



            	// AST REWRITE
            	// elements:          idx, variable, conditional, variable, DOT
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 215:3: -> ^( ASTVARWI ^( ASTVARWI0 ( DOT )? ) ^( ASTVARWI1 variable ) ^( ASTVARWI2 ( variable )? ) ^( ASTVARWI3 ( idx )? ) ^( ASTVARWI4 ( conditional )? ) )
            	{
            	    // GAMS.g:215:6: ^( ASTVARWI ^( ASTVARWI0 ( DOT )? ) ^( ASTVARWI1 variable ) ^( ASTVARWI2 ( variable )? ) ^( ASTVARWI3 ( idx )? ) ^( ASTVARWI4 ( conditional )? ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI, "ASTVARWI"), root_1);

            	    // GAMS.g:215:17: ^( ASTVARWI0 ( DOT )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI0, "ASTVARWI0"), root_2);

            	    // GAMS.g:215:29: ( DOT )?
            	    if ( stream_DOT.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_DOT.NextNode());

            	    }
            	    stream_DOT.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:215:35: ^( ASTVARWI1 variable )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI1, "ASTVARWI1"), root_2);

            	    adaptor.AddChild(root_2, stream_variable.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:215:57: ^( ASTVARWI2 ( variable )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI2, "ASTVARWI2"), root_2);

            	    // GAMS.g:215:69: ( variable )?
            	    if ( stream_variable.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_variable.NextTree());

            	    }
            	    stream_variable.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:215:80: ^( ASTVARWI3 ( idx )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI3, "ASTVARWI3"), root_2);

            	    // GAMS.g:215:92: ( idx )?
            	    if ( stream_idx.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_idx.NextTree());

            	    }
            	    stream_idx.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:215:98: ^( ASTVARWI4 ( conditional )? )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARWI4, "ASTVARWI4"), root_2);

            	    // GAMS.g:215:110: ( conditional )?
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
            	Memoize(input, 9, variableWithIndexerEtc_StartIndex); 
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
    // GAMS.g:217:1: variable : ident ;
    public GAMSParser.variable_return variable() // throws RecognitionException [1]
    {   
        GAMSParser.variable_return retval = new GAMSParser.variable_return();
        retval.Start = input.LT(1);
        int variable_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.ident_return ident35 = default(GAMSParser.ident_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 10) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:217:10: ( ident )
            // GAMS.g:217:12: ident
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_ident_in_variable769);
            	ident35 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, ident35.Tree);

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
            	Memoize(input, 10, variable_StartIndex); 
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
    // GAMS.g:219:1: idx : ( L1 indexerElements R1 -> ^( ASTIDX ^( ASTIDX0 L1 R1 ) indexerElements ) | L2 indexerElements R2 -> ^( ASTIDX ^( ASTIDX0 L2 R2 ) indexerElements ) | L3 indexerElements R3 -> ^( ASTIDX ^( ASTIDX0 L3 R3 ) indexerElements ) );
    public GAMSParser.idx_return idx() // throws RecognitionException [1]
    {   
        GAMSParser.idx_return retval = new GAMSParser.idx_return();
        retval.Start = input.LT(1);
        int idx_StartIndex = input.Index();
        object root_0 = null;

        IToken L136 = null;
        IToken R138 = null;
        IToken L239 = null;
        IToken R241 = null;
        IToken L342 = null;
        IToken R344 = null;
        GAMSParser.indexerElements_return indexerElements37 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements40 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements43 = default(GAMSParser.indexerElements_return);


        object L136_tree=null;
        object R138_tree=null;
        object L239_tree=null;
        object R241_tree=null;
        object L342_tree=null;
        object R344_tree=null;
        RewriteRuleTokenStream stream_L1 = new RewriteRuleTokenStream(adaptor,"token L1");
        RewriteRuleTokenStream stream_L2 = new RewriteRuleTokenStream(adaptor,"token L2");
        RewriteRuleTokenStream stream_L3 = new RewriteRuleTokenStream(adaptor,"token L3");
        RewriteRuleTokenStream stream_R2 = new RewriteRuleTokenStream(adaptor,"token R2");
        RewriteRuleTokenStream stream_R3 = new RewriteRuleTokenStream(adaptor,"token R3");
        RewriteRuleTokenStream stream_R1 = new RewriteRuleTokenStream(adaptor,"token R1");
        RewriteRuleSubtreeStream stream_indexerElements = new RewriteRuleSubtreeStream(adaptor,"rule indexerElements");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 11) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:219:4: ( L1 indexerElements R1 -> ^( ASTIDX ^( ASTIDX0 L1 R1 ) indexerElements ) | L2 indexerElements R2 -> ^( ASTIDX ^( ASTIDX0 L2 R2 ) indexerElements ) | L3 indexerElements R3 -> ^( ASTIDX ^( ASTIDX0 L3 R3 ) indexerElements ) )
            int alt9 = 3;
            switch ( input.LA(1) ) 
            {
            case L1:
            	{
                alt9 = 1;
                }
                break;
            case L2:
            	{
                alt9 = 2;
                }
                break;
            case L3:
            	{
                alt9 = 3;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d9s0 =
            	        new NoViableAltException("", 9, 0, input);

            	    throw nvae_d9s0;
            }

            switch (alt9) 
            {
                case 1 :
                    // GAMS.g:219:6: L1 indexerElements R1
                    {
                    	L136=(IToken)Match(input,L1,FOLLOW_L1_in_idx776); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L136);

                    	PushFollow(FOLLOW_indexerElements_in_idx778);
                    	indexerElements37 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements37.Tree);
                    	R138=(IToken)Match(input,R1,FOLLOW_R1_in_idx780); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R138);



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
                    	// 219:28: -> ^( ASTIDX ^( ASTIDX0 L1 R1 ) indexerElements )
                    	{
                    	    // GAMS.g:219:31: ^( ASTIDX ^( ASTIDX0 L1 R1 ) indexerElements )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDX, "ASTIDX"), root_1);

                    	    // GAMS.g:219:40: ^( ASTIDX0 L1 R1 )
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
                    // GAMS.g:220:6: L2 indexerElements R2
                    {
                    	L239=(IToken)Match(input,L2,FOLLOW_L2_in_idx803); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L239);

                    	PushFollow(FOLLOW_indexerElements_in_idx805);
                    	indexerElements40 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements40.Tree);
                    	R241=(IToken)Match(input,R2,FOLLOW_R2_in_idx807); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R241);



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
                    	// 220:28: -> ^( ASTIDX ^( ASTIDX0 L2 R2 ) indexerElements )
                    	{
                    	    // GAMS.g:220:31: ^( ASTIDX ^( ASTIDX0 L2 R2 ) indexerElements )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDX, "ASTIDX"), root_1);

                    	    // GAMS.g:220:40: ^( ASTIDX0 L2 R2 )
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
                    // GAMS.g:221:6: L3 indexerElements R3
                    {
                    	L342=(IToken)Match(input,L3,FOLLOW_L3_in_idx830); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L342);

                    	PushFollow(FOLLOW_indexerElements_in_idx832);
                    	indexerElements43 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements43.Tree);
                    	R344=(IToken)Match(input,R3,FOLLOW_R3_in_idx834); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R344);



                    	// AST REWRITE
                    	// elements:          L3, indexerElements, R3
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 221:28: -> ^( ASTIDX ^( ASTIDX0 L3 R3 ) indexerElements )
                    	{
                    	    // GAMS.g:221:31: ^( ASTIDX ^( ASTIDX0 L3 R3 ) indexerElements )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDX, "ASTIDX"), root_1);

                    	    // GAMS.g:221:40: ^( ASTIDX0 L3 R3 )
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
            	Memoize(input, 11, idx_StartIndex); 
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
    // GAMS.g:224:1: indexerElements : variableLagLead ( COMMA variableLagLead )* -> ^( ASTIDXELEMENTS ^( ASTIDXELEMENTS0 ( COMMA )* ) ^( ASTIDXELEMENTS1 ( variableLagLead )+ ) ) ;
    public GAMSParser.indexerElements_return indexerElements() // throws RecognitionException [1]
    {   
        GAMSParser.indexerElements_return retval = new GAMSParser.indexerElements_return();
        retval.Start = input.LT(1);
        int indexerElements_StartIndex = input.Index();
        object root_0 = null;

        IToken COMMA46 = null;
        GAMSParser.variableLagLead_return variableLagLead45 = default(GAMSParser.variableLagLead_return);

        GAMSParser.variableLagLead_return variableLagLead47 = default(GAMSParser.variableLagLead_return);


        object COMMA46_tree=null;
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleSubtreeStream stream_variableLagLead = new RewriteRuleSubtreeStream(adaptor,"rule variableLagLead");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 12) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:224:16: ( variableLagLead ( COMMA variableLagLead )* -> ^( ASTIDXELEMENTS ^( ASTIDXELEMENTS0 ( COMMA )* ) ^( ASTIDXELEMENTS1 ( variableLagLead )+ ) ) )
            // GAMS.g:224:18: variableLagLead ( COMMA variableLagLead )*
            {
            	PushFollow(FOLLOW_variableLagLead_in_indexerElements858);
            	variableLagLead45 = variableLagLead();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableLagLead.Add(variableLagLead45.Tree);
            	// GAMS.g:224:34: ( COMMA variableLagLead )*
            	do 
            	{
            	    int alt10 = 2;
            	    int LA10_0 = input.LA(1);

            	    if ( (LA10_0 == COMMA) )
            	    {
            	        alt10 = 1;
            	    }


            	    switch (alt10) 
            		{
            			case 1 :
            			    // GAMS.g:224:35: COMMA variableLagLead
            			    {
            			    	COMMA46=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_indexerElements861); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA46);

            			    	PushFollow(FOLLOW_variableLagLead_in_indexerElements863);
            			    	variableLagLead47 = variableLagLead();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_variableLagLead.Add(variableLagLead47.Tree);

            			    }
            			    break;

            			default:
            			    goto loop10;
            	    }
            	} while (true);

            	loop10:
            		;	// Stops C# compiler whining that label 'loop10' has no statements



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
            	// 225:3: -> ^( ASTIDXELEMENTS ^( ASTIDXELEMENTS0 ( COMMA )* ) ^( ASTIDXELEMENTS1 ( variableLagLead )+ ) )
            	{
            	    // GAMS.g:225:6: ^( ASTIDXELEMENTS ^( ASTIDXELEMENTS0 ( COMMA )* ) ^( ASTIDXELEMENTS1 ( variableLagLead )+ ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDXELEMENTS, "ASTIDXELEMENTS"), root_1);

            	    // GAMS.g:225:23: ^( ASTIDXELEMENTS0 ( COMMA )* )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTIDXELEMENTS0, "ASTIDXELEMENTS0"), root_2);

            	    // GAMS.g:225:41: ( COMMA )*
            	    while ( stream_COMMA.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_COMMA.NextNode());

            	    }
            	    stream_COMMA.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:225:49: ^( ASTIDXELEMENTS1 ( variableLagLead )+ )
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
            	Memoize(input, 12, indexerElements_StartIndex); 
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
    // GAMS.g:227:1: variableLagLead : ( StringInQuotes -> ^( ASTVARIABLEANDLEAD StringInQuotes ) | variable PLUS Integer -> ^( ASTVARIABLEANDLEAD variable PLUS Integer ) | variable MINUS Integer -> ^( ASTVARIABLEANDLEAD variable MINUS Integer ) | variable -> ^( ASTVARIABLEANDLEAD variable ) );
    public GAMSParser.variableLagLead_return variableLagLead() // throws RecognitionException [1]
    {   
        GAMSParser.variableLagLead_return retval = new GAMSParser.variableLagLead_return();
        retval.Start = input.LT(1);
        int variableLagLead_StartIndex = input.Index();
        object root_0 = null;

        IToken StringInQuotes48 = null;
        IToken PLUS50 = null;
        IToken Integer51 = null;
        IToken MINUS53 = null;
        IToken Integer54 = null;
        GAMSParser.variable_return variable49 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable52 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable55 = default(GAMSParser.variable_return);


        object StringInQuotes48_tree=null;
        object PLUS50_tree=null;
        object Integer51_tree=null;
        object MINUS53_tree=null;
        object Integer54_tree=null;
        RewriteRuleTokenStream stream_StringInQuotes = new RewriteRuleTokenStream(adaptor,"token StringInQuotes");
        RewriteRuleTokenStream stream_PLUS = new RewriteRuleTokenStream(adaptor,"token PLUS");
        RewriteRuleTokenStream stream_MINUS = new RewriteRuleTokenStream(adaptor,"token MINUS");
        RewriteRuleTokenStream stream_Integer = new RewriteRuleTokenStream(adaptor,"token Integer");
        RewriteRuleSubtreeStream stream_variable = new RewriteRuleSubtreeStream(adaptor,"rule variable");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 13) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:227:16: ( StringInQuotes -> ^( ASTVARIABLEANDLEAD StringInQuotes ) | variable PLUS Integer -> ^( ASTVARIABLEANDLEAD variable PLUS Integer ) | variable MINUS Integer -> ^( ASTVARIABLEANDLEAD variable MINUS Integer ) | variable -> ^( ASTVARIABLEANDLEAD variable ) )
            int alt11 = 4;
            alt11 = dfa11.Predict(input);
            switch (alt11) 
            {
                case 1 :
                    // GAMS.g:227:18: StringInQuotes
                    {
                    	StringInQuotes48=(IToken)Match(input,StringInQuotes,FOLLOW_StringInQuotes_in_variableLagLead895); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_StringInQuotes.Add(StringInQuotes48);



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
                    	// 227:43: -> ^( ASTVARIABLEANDLEAD StringInQuotes )
                    	{
                    	    // GAMS.g:227:46: ^( ASTVARIABLEANDLEAD StringInQuotes )
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
                    // GAMS.g:228:18: variable PLUS Integer
                    {
                    	PushFollow(FOLLOW_variable_in_variableLagLead944);
                    	variable49 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variable.Add(variable49.Tree);
                    	PLUS50=(IToken)Match(input,PLUS,FOLLOW_PLUS_in_variableLagLead946); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_PLUS.Add(PLUS50);

                    	Integer51=(IToken)Match(input,Integer,FOLLOW_Integer_in_variableLagLead948); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer51);



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
                    	// 228:43: -> ^( ASTVARIABLEANDLEAD variable PLUS Integer )
                    	{
                    	    // GAMS.g:228:46: ^( ASTVARIABLEANDLEAD variable PLUS Integer )
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
                    // GAMS.g:229:18: variable MINUS Integer
                    {
                    	PushFollow(FOLLOW_variable_in_variableLagLead994);
                    	variable52 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variable.Add(variable52.Tree);
                    	MINUS53=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_variableLagLead996); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_MINUS.Add(MINUS53);

                    	Integer54=(IToken)Match(input,Integer,FOLLOW_Integer_in_variableLagLead998); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer54);



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
                    	// 229:43: -> ^( ASTVARIABLEANDLEAD variable MINUS Integer )
                    	{
                    	    // GAMS.g:229:46: ^( ASTVARIABLEANDLEAD variable MINUS Integer )
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
                    // GAMS.g:230:9: variable
                    {
                    	PushFollow(FOLLOW_variable_in_variableLagLead1034);
                    	variable55 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variable.Add(variable55.Tree);


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
                    	// 230:34: -> ^( ASTVARIABLEANDLEAD variable )
                    	{
                    	    // GAMS.g:230:37: ^( ASTVARIABLEANDLEAD variable )
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
            	Memoize(input, 13, variableLagLead_StartIndex); 
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
    // GAMS.g:233:1: conditional : DOLLAR expression -> ^( ASTCONDITIONAL DOLLAR expression ) ;
    public GAMSParser.conditional_return conditional() // throws RecognitionException [1]
    {   
        GAMSParser.conditional_return retval = new GAMSParser.conditional_return();
        retval.Start = input.LT(1);
        int conditional_StartIndex = input.Index();
        object root_0 = null;

        IToken DOLLAR56 = null;
        GAMSParser.expression_return expression57 = default(GAMSParser.expression_return);


        object DOLLAR56_tree=null;
        RewriteRuleTokenStream stream_DOLLAR = new RewriteRuleTokenStream(adaptor,"token DOLLAR");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 14) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:233:12: ( DOLLAR expression -> ^( ASTCONDITIONAL DOLLAR expression ) )
            // GAMS.g:233:14: DOLLAR expression
            {
            	DOLLAR56=(IToken)Match(input,DOLLAR,FOLLOW_DOLLAR_in_conditional1078); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_DOLLAR.Add(DOLLAR56);

            	PushFollow(FOLLOW_expression_in_conditional1080);
            	expression57 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression57.Tree);


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
            	// 233:32: -> ^( ASTCONDITIONAL DOLLAR expression )
            	{
            	    // GAMS.g:233:35: ^( ASTCONDITIONAL DOLLAR expression )
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
    // GAMS.g:251:1: expression : andExpression ( OR andExpression )* ;
    public GAMSParser.expression_return expression() // throws RecognitionException [1]
    {   
        GAMSParser.expression_return retval = new GAMSParser.expression_return();
        retval.Start = input.LT(1);
        int expression_StartIndex = input.Index();
        object root_0 = null;

        IToken OR59 = null;
        GAMSParser.andExpression_return andExpression58 = default(GAMSParser.andExpression_return);

        GAMSParser.andExpression_return andExpression60 = default(GAMSParser.andExpression_return);


        object OR59_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 15) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:251:11: ( andExpression ( OR andExpression )* )
            // GAMS.g:251:13: andExpression ( OR andExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_andExpression_in_expression1113);
            	andExpression58 = andExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, andExpression58.Tree);
            	// GAMS.g:251:27: ( OR andExpression )*
            	do 
            	{
            	    int alt12 = 2;
            	    alt12 = dfa12.Predict(input);
            	    switch (alt12) 
            		{
            			case 1 :
            			    // GAMS.g:251:28: OR andExpression
            			    {
            			    	OR59=(IToken)Match(input,OR,FOLLOW_OR_in_expression1116); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{OR59_tree = (object)adaptor.Create(OR59);
            			    		root_0 = (object)adaptor.BecomeRoot(OR59_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_andExpression_in_expression1119);
            			    	andExpression60 = andExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, andExpression60.Tree);

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
    // GAMS.g:253:1: andExpression : notExpression ( AND notExpression )* ;
    public GAMSParser.andExpression_return andExpression() // throws RecognitionException [1]
    {   
        GAMSParser.andExpression_return retval = new GAMSParser.andExpression_return();
        retval.Start = input.LT(1);
        int andExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken AND62 = null;
        GAMSParser.notExpression_return notExpression61 = default(GAMSParser.notExpression_return);

        GAMSParser.notExpression_return notExpression63 = default(GAMSParser.notExpression_return);


        object AND62_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 16) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:253:14: ( notExpression ( AND notExpression )* )
            // GAMS.g:253:16: notExpression ( AND notExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_notExpression_in_andExpression1128);
            	notExpression61 = notExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, notExpression61.Tree);
            	// GAMS.g:253:30: ( AND notExpression )*
            	do 
            	{
            	    int alt13 = 2;
            	    alt13 = dfa13.Predict(input);
            	    switch (alt13) 
            		{
            			case 1 :
            			    // GAMS.g:253:31: AND notExpression
            			    {
            			    	AND62=(IToken)Match(input,AND,FOLLOW_AND_in_andExpression1131); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{AND62_tree = (object)adaptor.Create(AND62);
            			    		root_0 = (object)adaptor.BecomeRoot(AND62_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_notExpression_in_andExpression1134);
            			    	notExpression63 = notExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, notExpression63.Tree);

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
    // GAMS.g:255:1: notExpression : ( NOT logicalExpression -> ^( NOT logicalExpression ) | logicalExpression );
    public GAMSParser.notExpression_return notExpression() // throws RecognitionException [1]
    {   
        GAMSParser.notExpression_return retval = new GAMSParser.notExpression_return();
        retval.Start = input.LT(1);
        int notExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken NOT64 = null;
        GAMSParser.logicalExpression_return logicalExpression65 = default(GAMSParser.logicalExpression_return);

        GAMSParser.logicalExpression_return logicalExpression66 = default(GAMSParser.logicalExpression_return);


        object NOT64_tree=null;
        RewriteRuleTokenStream stream_NOT = new RewriteRuleTokenStream(adaptor,"token NOT");
        RewriteRuleSubtreeStream stream_logicalExpression = new RewriteRuleSubtreeStream(adaptor,"rule logicalExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 17) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:255:14: ( NOT logicalExpression -> ^( NOT logicalExpression ) | logicalExpression )
            int alt14 = 2;
            alt14 = dfa14.Predict(input);
            switch (alt14) 
            {
                case 1 :
                    // GAMS.g:255:16: NOT logicalExpression
                    {
                    	NOT64=(IToken)Match(input,NOT,FOLLOW_NOT_in_notExpression1145); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_NOT.Add(NOT64);

                    	PushFollow(FOLLOW_logicalExpression_in_notExpression1147);
                    	logicalExpression65 = logicalExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_logicalExpression.Add(logicalExpression65.Tree);


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
                    	// 255:38: -> ^( NOT logicalExpression )
                    	{
                    	    // GAMS.g:255:41: ^( NOT logicalExpression )
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
                    // GAMS.g:256:10: logicalExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_logicalExpression_in_notExpression1167);
                    	logicalExpression66 = logicalExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, logicalExpression66.Tree);

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
    // GAMS.g:258:1: logicalExpression : additiveExpression ( logical additiveExpression )* ;
    public GAMSParser.logicalExpression_return logicalExpression() // throws RecognitionException [1]
    {   
        GAMSParser.logicalExpression_return retval = new GAMSParser.logicalExpression_return();
        retval.Start = input.LT(1);
        int logicalExpression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.additiveExpression_return additiveExpression67 = default(GAMSParser.additiveExpression_return);

        GAMSParser.logical_return logical68 = default(GAMSParser.logical_return);

        GAMSParser.additiveExpression_return additiveExpression69 = default(GAMSParser.additiveExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 18) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:258:18: ( additiveExpression ( logical additiveExpression )* )
            // GAMS.g:258:21: additiveExpression ( logical additiveExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_additiveExpression_in_logicalExpression1175);
            	additiveExpression67 = additiveExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression67.Tree);
            	// GAMS.g:258:40: ( logical additiveExpression )*
            	do 
            	{
            	    int alt15 = 2;
            	    alt15 = dfa15.Predict(input);
            	    switch (alt15) 
            		{
            			case 1 :
            			    // GAMS.g:258:41: logical additiveExpression
            			    {
            			    	PushFollow(FOLLOW_logical_in_logicalExpression1178);
            			    	logical68 = logical();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot(logical68.Tree, root_0);
            			    	PushFollow(FOLLOW_additiveExpression_in_logicalExpression1181);
            			    	additiveExpression69 = additiveExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression69.Tree);

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
    // GAMS.g:260:1: additiveExpression : multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* ;
    public GAMSParser.additiveExpression_return additiveExpression() // throws RecognitionException [1]
    {   
        GAMSParser.additiveExpression_return retval = new GAMSParser.additiveExpression_return();
        retval.Start = input.LT(1);
        int additiveExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set71 = null;
        GAMSParser.multiplicativeExpression_return multiplicativeExpression70 = default(GAMSParser.multiplicativeExpression_return);

        GAMSParser.multiplicativeExpression_return multiplicativeExpression72 = default(GAMSParser.multiplicativeExpression_return);


        object set71_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 19) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:260:19: ( multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* )
            // GAMS.g:260:21: multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression1190);
            	multiplicativeExpression70 = multiplicativeExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression70.Tree);
            	// GAMS.g:260:46: ( ( PLUS | MINUS ) multiplicativeExpression )*
            	do 
            	{
            	    int alt16 = 2;
            	    alt16 = dfa16.Predict(input);
            	    switch (alt16) 
            		{
            			case 1 :
            			    // GAMS.g:260:48: ( PLUS | MINUS ) multiplicativeExpression
            			    {
            			    	set71=(IToken)input.LT(1);
            			    	set71 = (IToken)input.LT(1);
            			    	if ( input.LA(1) == PLUS || input.LA(1) == MINUS ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set71), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression1201);
            			    	multiplicativeExpression72 = multiplicativeExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression72.Tree);

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
    // GAMS.g:262:1: multiplicativeExpression : powerExpression ( ( MULT | DIV ) powerExpression )* ;
    public GAMSParser.multiplicativeExpression_return multiplicativeExpression() // throws RecognitionException [1]
    {   
        GAMSParser.multiplicativeExpression_return retval = new GAMSParser.multiplicativeExpression_return();
        retval.Start = input.LT(1);
        int multiplicativeExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set74 = null;
        GAMSParser.powerExpression_return powerExpression73 = default(GAMSParser.powerExpression_return);

        GAMSParser.powerExpression_return powerExpression75 = default(GAMSParser.powerExpression_return);


        object set74_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 20) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:262:25: ( powerExpression ( ( MULT | DIV ) powerExpression )* )
            // GAMS.g:262:27: powerExpression ( ( MULT | DIV ) powerExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression1211);
            	powerExpression73 = powerExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression73.Tree);
            	// GAMS.g:262:43: ( ( MULT | DIV ) powerExpression )*
            	do 
            	{
            	    int alt17 = 2;
            	    alt17 = dfa17.Predict(input);
            	    switch (alt17) 
            		{
            			case 1 :
            			    // GAMS.g:262:45: ( MULT | DIV ) powerExpression
            			    {
            			    	set74=(IToken)input.LT(1);
            			    	set74 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= MULT && input.LA(1) <= DIV) ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set74), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression1222);
            			    	powerExpression75 = powerExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression75.Tree);

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
    // GAMS.g:264:1: powerExpression : unaryExpression ( STARS unaryExpression )* ;
    public GAMSParser.powerExpression_return powerExpression() // throws RecognitionException [1]
    {   
        GAMSParser.powerExpression_return retval = new GAMSParser.powerExpression_return();
        retval.Start = input.LT(1);
        int powerExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken STARS77 = null;
        GAMSParser.unaryExpression_return unaryExpression76 = default(GAMSParser.unaryExpression_return);

        GAMSParser.unaryExpression_return unaryExpression78 = default(GAMSParser.unaryExpression_return);


        object STARS77_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 21) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:264:16: ( unaryExpression ( STARS unaryExpression )* )
            // GAMS.g:264:18: unaryExpression ( STARS unaryExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_unaryExpression_in_powerExpression1232);
            	unaryExpression76 = unaryExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression76.Tree);
            	// GAMS.g:264:34: ( STARS unaryExpression )*
            	do 
            	{
            	    int alt18 = 2;
            	    alt18 = dfa18.Predict(input);
            	    switch (alt18) 
            		{
            			case 1 :
            			    // GAMS.g:264:36: STARS unaryExpression
            			    {
            			    	STARS77=(IToken)Match(input,STARS,FOLLOW_STARS_in_powerExpression1236); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{STARS77_tree = (object)adaptor.Create(STARS77);
            			    		root_0 = (object)adaptor.BecomeRoot(STARS77_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_unaryExpression_in_powerExpression1239);
            			    	unaryExpression78 = unaryExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression78.Tree);

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
    // GAMS.g:266:1: unaryExpression : ( MINUS dollarExpression -> ^( NEGATE dollarExpression ) | dollarExpression );
    public GAMSParser.unaryExpression_return unaryExpression() // throws RecognitionException [1]
    {   
        GAMSParser.unaryExpression_return retval = new GAMSParser.unaryExpression_return();
        retval.Start = input.LT(1);
        int unaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken MINUS79 = null;
        GAMSParser.dollarExpression_return dollarExpression80 = default(GAMSParser.dollarExpression_return);

        GAMSParser.dollarExpression_return dollarExpression81 = default(GAMSParser.dollarExpression_return);


        object MINUS79_tree=null;
        RewriteRuleTokenStream stream_MINUS = new RewriteRuleTokenStream(adaptor,"token MINUS");
        RewriteRuleSubtreeStream stream_dollarExpression = new RewriteRuleSubtreeStream(adaptor,"rule dollarExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 22) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:266:16: ( MINUS dollarExpression -> ^( NEGATE dollarExpression ) | dollarExpression )
            int alt19 = 2;
            alt19 = dfa19.Predict(input);
            switch (alt19) 
            {
                case 1 :
                    // GAMS.g:266:18: MINUS dollarExpression
                    {
                    	MINUS79=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_unaryExpression1250); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_MINUS.Add(MINUS79);

                    	PushFollow(FOLLOW_dollarExpression_in_unaryExpression1252);
                    	dollarExpression80 = dollarExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_dollarExpression.Add(dollarExpression80.Tree);


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
                    	// 266:41: -> ^( NEGATE dollarExpression )
                    	{
                    	    // GAMS.g:266:44: ^( NEGATE dollarExpression )
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
                    // GAMS.g:267:11: dollarExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_dollarExpression_in_unaryExpression1273);
                    	dollarExpression81 = dollarExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, dollarExpression81.Tree);

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
    // GAMS.g:275:1: dollarExpression : ( primaryExpression conditional -> ^( ASTDOLLAREXPRESSION primaryExpression conditional ) | primaryExpression );
    public GAMSParser.dollarExpression_return dollarExpression() // throws RecognitionException [1]
    {   
        GAMSParser.dollarExpression_return retval = new GAMSParser.dollarExpression_return();
        retval.Start = input.LT(1);
        int dollarExpression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.primaryExpression_return primaryExpression82 = default(GAMSParser.primaryExpression_return);

        GAMSParser.conditional_return conditional83 = default(GAMSParser.conditional_return);

        GAMSParser.primaryExpression_return primaryExpression84 = default(GAMSParser.primaryExpression_return);


        RewriteRuleSubtreeStream stream_conditional = new RewriteRuleSubtreeStream(adaptor,"rule conditional");
        RewriteRuleSubtreeStream stream_primaryExpression = new RewriteRuleSubtreeStream(adaptor,"rule primaryExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 23) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:275:17: ( primaryExpression conditional -> ^( ASTDOLLAREXPRESSION primaryExpression conditional ) | primaryExpression )
            int alt20 = 2;
            alt20 = dfa20.Predict(input);
            switch (alt20) 
            {
                case 1 :
                    // GAMS.g:276:9: primaryExpression conditional
                    {
                    	PushFollow(FOLLOW_primaryExpression_in_dollarExpression1298);
                    	primaryExpression82 = primaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_primaryExpression.Add(primaryExpression82.Tree);
                    	PushFollow(FOLLOW_conditional_in_dollarExpression1300);
                    	conditional83 = conditional();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_conditional.Add(conditional83.Tree);


                    	// AST REWRITE
                    	// elements:          primaryExpression, conditional
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 276:39: -> ^( ASTDOLLAREXPRESSION primaryExpression conditional )
                    	{
                    	    // GAMS.g:276:42: ^( ASTDOLLAREXPRESSION primaryExpression conditional )
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
                    // GAMS.g:277:9: primaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_primaryExpression_in_dollarExpression1320);
                    	primaryExpression84 = primaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, primaryExpression84.Tree);

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
    // GAMS.g:280:1: primaryExpression : ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value );
    public GAMSParser.primaryExpression_return primaryExpression() // throws RecognitionException [1]
    {   
        GAMSParser.primaryExpression_return retval = new GAMSParser.primaryExpression_return();
        retval.Start = input.LT(1);
        int primaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken L185 = null;
        IToken R187 = null;
        IToken L288 = null;
        IToken R290 = null;
        IToken L391 = null;
        IToken R393 = null;
        GAMSParser.expression_return expression86 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression89 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression92 = default(GAMSParser.expression_return);

        GAMSParser.value_return value94 = default(GAMSParser.value_return);


        object L185_tree=null;
        object R187_tree=null;
        object L288_tree=null;
        object R290_tree=null;
        object L391_tree=null;
        object R393_tree=null;
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
            // GAMS.g:280:18: ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value )
            int alt21 = 4;
            alt21 = dfa21.Predict(input);
            switch (alt21) 
            {
                case 1 :
                    // GAMS.g:281:5: L1 expression R1
                    {
                    	L185=(IToken)Match(input,L1,FOLLOW_L1_in_primaryExpression1341); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L185);

                    	PushFollow(FOLLOW_expression_in_primaryExpression1343);
                    	expression86 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression86.Tree);
                    	R187=(IToken)Match(input,R1,FOLLOW_R1_in_primaryExpression1345); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R187);



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
                    	// 281:22: -> ^( ASTEXPRESSION1 expression )
                    	{
                    	    // GAMS.g:281:25: ^( ASTEXPRESSION1 expression )
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
                    // GAMS.g:282:6: L2 expression R2
                    {
                    	L288=(IToken)Match(input,L2,FOLLOW_L2_in_primaryExpression1360); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L288);

                    	PushFollow(FOLLOW_expression_in_primaryExpression1362);
                    	expression89 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression89.Tree);
                    	R290=(IToken)Match(input,R2,FOLLOW_R2_in_primaryExpression1364); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R290);



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
                    	// 282:23: -> ^( ASTEXPRESSION2 expression )
                    	{
                    	    // GAMS.g:282:26: ^( ASTEXPRESSION2 expression )
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
                    // GAMS.g:283:8: L3 expression R3
                    {
                    	L391=(IToken)Match(input,L3,FOLLOW_L3_in_primaryExpression1381); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L391);

                    	PushFollow(FOLLOW_expression_in_primaryExpression1383);
                    	expression92 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression92.Tree);
                    	R393=(IToken)Match(input,R3,FOLLOW_R3_in_primaryExpression1385); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R393);



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
                    	// 283:25: -> ^( ASTEXPRESSION3 expression )
                    	{
                    	    // GAMS.g:283:28: ^( ASTEXPRESSION3 expression )
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
                    // GAMS.g:284:6: value
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_value_in_primaryExpression1400);
                    	value94 = value();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, value94.Tree);

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
    // GAMS.g:286:1: logical : ( NONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL | EQUAL | LESSTHAN | GREATERTHAN );
    public GAMSParser.logical_return logical() // throws RecognitionException [1]
    {   
        GAMSParser.logical_return retval = new GAMSParser.logical_return();
        retval.Start = input.LT(1);
        int logical_StartIndex = input.Index();
        object root_0 = null;

        IToken set95 = null;

        object set95_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 25) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:286:8: ( NONEQUAL | LESSTHANOREQUAL | GREATERTHANOREQUAL | EQUAL | LESSTHAN | GREATERTHAN )
            // GAMS.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set95 = (IToken)input.LT(1);
            	if ( input.LA(1) == EQUAL || (input.LA(1) >= NONEQUAL && input.LA(1) <= GREATERTHAN) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set95));
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
    // GAMS.g:288:1: value : ( Integer -> ^( ASTVALUE Integer ) | Double -> ^( ASTVALUE Double ) | sum -> ^( ASTVALUE sum ) | function -> ^( ASTVALUE function ) | variableWithIndexerEtc -> ^( ASTVALUE variableWithIndexerEtc ) );
    public GAMSParser.value_return value() // throws RecognitionException [1]
    {   
        GAMSParser.value_return retval = new GAMSParser.value_return();
        retval.Start = input.LT(1);
        int value_StartIndex = input.Index();
        object root_0 = null;

        IToken Integer96 = null;
        IToken Double97 = null;
        GAMSParser.sum_return sum98 = default(GAMSParser.sum_return);

        GAMSParser.function_return function99 = default(GAMSParser.function_return);

        GAMSParser.variableWithIndexerEtc_return variableWithIndexerEtc100 = default(GAMSParser.variableWithIndexerEtc_return);


        object Integer96_tree=null;
        object Double97_tree=null;
        RewriteRuleTokenStream stream_Double = new RewriteRuleTokenStream(adaptor,"token Double");
        RewriteRuleTokenStream stream_Integer = new RewriteRuleTokenStream(adaptor,"token Integer");
        RewriteRuleSubtreeStream stream_variableWithIndexerEtc = new RewriteRuleSubtreeStream(adaptor,"rule variableWithIndexerEtc");
        RewriteRuleSubtreeStream stream_sum = new RewriteRuleSubtreeStream(adaptor,"rule sum");
        RewriteRuleSubtreeStream stream_function = new RewriteRuleSubtreeStream(adaptor,"rule function");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 26) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:289:2: ( Integer -> ^( ASTVALUE Integer ) | Double -> ^( ASTVALUE Double ) | sum -> ^( ASTVALUE sum ) | function -> ^( ASTVALUE function ) | variableWithIndexerEtc -> ^( ASTVALUE variableWithIndexerEtc ) )
            int alt22 = 5;
            alt22 = dfa22.Predict(input);
            switch (alt22) 
            {
                case 1 :
                    // GAMS.g:289:5: Integer
                    {
                    	Integer96=(IToken)Match(input,Integer,FOLLOW_Integer_in_value1441); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer96);



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
                    	// 289:31: -> ^( ASTVALUE Integer )
                    	{
                    	    // GAMS.g:289:34: ^( ASTVALUE Integer )
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
                    // GAMS.g:290:4: Double
                    {
                    	Double97=(IToken)Match(input,Double,FOLLOW_Double_in_value1472); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Double.Add(Double97);



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
                    	// 290:30: -> ^( ASTVALUE Double )
                    	{
                    	    // GAMS.g:290:33: ^( ASTVALUE Double )
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
                    // GAMS.g:291:6: sum
                    {
                    	PushFollow(FOLLOW_sum_in_value1506);
                    	sum98 = sum();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sum.Add(sum98.Tree);


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
                    	// 291:32: -> ^( ASTVALUE sum )
                    	{
                    	    // GAMS.g:291:35: ^( ASTVALUE sum )
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
                    // GAMS.g:292:6: function
                    {
                    	PushFollow(FOLLOW_function_in_value1543);
                    	function99 = function();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_function.Add(function99.Tree);


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
                    	// 292:32: -> ^( ASTVALUE function )
                    	{
                    	    // GAMS.g:292:35: ^( ASTVALUE function )
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
                    // GAMS.g:293:4: variableWithIndexerEtc
                    {
                    	PushFollow(FOLLOW_variableWithIndexerEtc_in_value1591);
                    	variableWithIndexerEtc100 = variableWithIndexerEtc();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variableWithIndexerEtc.Add(variableWithIndexerEtc100.Tree);


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
                    	// 293:30: -> ^( ASTVALUE variableWithIndexerEtc )
                    	{
                    	    // GAMS.g:293:33: ^( ASTVALUE variableWithIndexerEtc )
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
            	Memoize(input, 26, value_StartIndex); 
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
    // GAMS.g:300:1: function : ( functionName L1 functionElements R1 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L1 R1 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) | functionName L2 functionElements R2 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L2 R2 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) | functionName L3 functionElements R3 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L3 R3 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) );
    public GAMSParser.function_return function() // throws RecognitionException [1]
    {   
        GAMSParser.function_return retval = new GAMSParser.function_return();
        retval.Start = input.LT(1);
        int function_StartIndex = input.Index();
        object root_0 = null;

        IToken L1102 = null;
        IToken R1104 = null;
        IToken L2106 = null;
        IToken R2108 = null;
        IToken L3110 = null;
        IToken R3112 = null;
        GAMSParser.functionName_return functionName101 = default(GAMSParser.functionName_return);

        GAMSParser.functionElements_return functionElements103 = default(GAMSParser.functionElements_return);

        GAMSParser.functionName_return functionName105 = default(GAMSParser.functionName_return);

        GAMSParser.functionElements_return functionElements107 = default(GAMSParser.functionElements_return);

        GAMSParser.functionName_return functionName109 = default(GAMSParser.functionName_return);

        GAMSParser.functionElements_return functionElements111 = default(GAMSParser.functionElements_return);


        object L1102_tree=null;
        object R1104_tree=null;
        object L2106_tree=null;
        object R2108_tree=null;
        object L3110_tree=null;
        object R3112_tree=null;
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
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 27) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:300:9: ( functionName L1 functionElements R1 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L1 R1 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) | functionName L2 functionElements R2 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L2 R2 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) | functionName L3 functionElements R3 -> ^( ASTFUNCTION ^( ASTFUNCTION0 L3 R3 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) ) )
            int alt23 = 3;
            int LA23_0 = input.LA(1);

            if ( ((LA23_0 >= ABS && LA23_0 <= SAMEAS)) )
            {
                switch ( input.LA(2) ) 
                {
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
                case L1:
                	{
                    alt23 = 1;
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
                    // GAMS.g:300:15: functionName L1 functionElements R1
                    {
                    	PushFollow(FOLLOW_functionName_in_function1623);
                    	functionName101 = functionName();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionName.Add(functionName101.Tree);
                    	L1102=(IToken)Match(input,L1,FOLLOW_L1_in_function1625); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L1102);

                    	PushFollow(FOLLOW_functionElements_in_function1627);
                    	functionElements103 = functionElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionElements.Add(functionElements103.Tree);
                    	R1104=(IToken)Match(input,R1,FOLLOW_R1_in_function1629); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R1104);



                    	// AST REWRITE
                    	// elements:          L1, functionElements, R1, functionName
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 300:51: -> ^( ASTFUNCTION ^( ASTFUNCTION0 L1 R1 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	{
                    	    // GAMS.g:300:54: ^( ASTFUNCTION ^( ASTFUNCTION0 L1 R1 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION, "ASTFUNCTION"), root_1);

                    	    // GAMS.g:300:68: ^( ASTFUNCTION0 L1 R1 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION0, "ASTFUNCTION0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L1.NextNode());
                    	    adaptor.AddChild(root_2, stream_R1.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:300:90: ^( ASTFUNCTION1 functionName )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION1, "ASTFUNCTION1"), root_2);

                    	    adaptor.AddChild(root_2, stream_functionName.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:300:119: ^( ASTFUNCTION2 functionElements )
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
                    // GAMS.g:301:15: functionName L2 functionElements R2
                    {
                    	PushFollow(FOLLOW_functionName_in_function1671);
                    	functionName105 = functionName();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionName.Add(functionName105.Tree);
                    	L2106=(IToken)Match(input,L2,FOLLOW_L2_in_function1673); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L2106);

                    	PushFollow(FOLLOW_functionElements_in_function1675);
                    	functionElements107 = functionElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionElements.Add(functionElements107.Tree);
                    	R2108=(IToken)Match(input,R2,FOLLOW_R2_in_function1677); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R2108);



                    	// AST REWRITE
                    	// elements:          functionElements, functionName, L2, R2
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 301:51: -> ^( ASTFUNCTION ^( ASTFUNCTION0 L2 R2 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	{
                    	    // GAMS.g:301:54: ^( ASTFUNCTION ^( ASTFUNCTION0 L2 R2 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION, "ASTFUNCTION"), root_1);

                    	    // GAMS.g:301:68: ^( ASTFUNCTION0 L2 R2 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION0, "ASTFUNCTION0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L2.NextNode());
                    	    adaptor.AddChild(root_2, stream_R2.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:301:90: ^( ASTFUNCTION1 functionName )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION1, "ASTFUNCTION1"), root_2);

                    	    adaptor.AddChild(root_2, stream_functionName.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:301:119: ^( ASTFUNCTION2 functionElements )
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
                    // GAMS.g:302:15: functionName L3 functionElements R3
                    {
                    	PushFollow(FOLLOW_functionName_in_function1719);
                    	functionName109 = functionName();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionName.Add(functionName109.Tree);
                    	L3110=(IToken)Match(input,L3,FOLLOW_L3_in_function1721); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L3110);

                    	PushFollow(FOLLOW_functionElements_in_function1723);
                    	functionElements111 = functionElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_functionElements.Add(functionElements111.Tree);
                    	R3112=(IToken)Match(input,R3,FOLLOW_R3_in_function1725); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R3112);



                    	// AST REWRITE
                    	// elements:          functionElements, functionName, R3, L3
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 302:51: -> ^( ASTFUNCTION ^( ASTFUNCTION0 L3 R3 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	{
                    	    // GAMS.g:302:54: ^( ASTFUNCTION ^( ASTFUNCTION0 L3 R3 ) ^( ASTFUNCTION1 functionName ) ^( ASTFUNCTION2 functionElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION, "ASTFUNCTION"), root_1);

                    	    // GAMS.g:302:68: ^( ASTFUNCTION0 L3 R3 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION0, "ASTFUNCTION0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L3.NextNode());
                    	    adaptor.AddChild(root_2, stream_R3.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:302:90: ^( ASTFUNCTION1 functionName )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION1, "ASTFUNCTION1"), root_2);

                    	    adaptor.AddChild(root_2, stream_functionName.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:302:119: ^( ASTFUNCTION2 functionElements )
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
            	Memoize(input, 27, function_StartIndex); 
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
    // GAMS.g:305:1: functionName : ( ABS | EXP | LOG | MAX | MIN | POWER | SQR | SAMEAS | TANH );
    public GAMSParser.functionName_return functionName() // throws RecognitionException [1]
    {   
        GAMSParser.functionName_return retval = new GAMSParser.functionName_return();
        retval.Start = input.LT(1);
        int functionName_StartIndex = input.Index();
        object root_0 = null;

        IToken set113 = null;

        object set113_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 28) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:305:13: ( ABS | EXP | LOG | MAX | MIN | POWER | SQR | SAMEAS | TANH )
            // GAMS.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set113 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= ABS && input.LA(1) <= SAMEAS) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set113));
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
            	Memoize(input, 28, functionName_StartIndex); 
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
    // GAMS.g:307:1: functionElements : expression ( COMMA expression )* -> ^( ASTFUNCTIONELEMENTS ^( ASTFUNCTIONELEMENTS0 ( COMMA )* ) ^( ASTFUNCTIONELEMENTS1 ( expression )+ ) ) ;
    public GAMSParser.functionElements_return functionElements() // throws RecognitionException [1]
    {   
        GAMSParser.functionElements_return retval = new GAMSParser.functionElements_return();
        retval.Start = input.LT(1);
        int functionElements_StartIndex = input.Index();
        object root_0 = null;

        IToken COMMA115 = null;
        GAMSParser.expression_return expression114 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression116 = default(GAMSParser.expression_return);


        object COMMA115_tree=null;
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 29) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:307:17: ( expression ( COMMA expression )* -> ^( ASTFUNCTIONELEMENTS ^( ASTFUNCTIONELEMENTS0 ( COMMA )* ) ^( ASTFUNCTIONELEMENTS1 ( expression )+ ) ) )
            // GAMS.g:307:19: expression ( COMMA expression )*
            {
            	PushFollow(FOLLOW_expression_in_functionElements1800);
            	expression114 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression114.Tree);
            	// GAMS.g:307:30: ( COMMA expression )*
            	do 
            	{
            	    int alt24 = 2;
            	    int LA24_0 = input.LA(1);

            	    if ( (LA24_0 == COMMA) )
            	    {
            	        alt24 = 1;
            	    }


            	    switch (alt24) 
            		{
            			case 1 :
            			    // GAMS.g:307:31: COMMA expression
            			    {
            			    	COMMA115=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_functionElements1803); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA115);

            			    	PushFollow(FOLLOW_expression_in_functionElements1805);
            			    	expression116 = expression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_expression.Add(expression116.Tree);

            			    }
            			    break;

            			default:
            			    goto loop24;
            	    }
            	} while (true);

            	loop24:
            		;	// Stops C# compiler whining that label 'loop24' has no statements



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
            	// 308:3: -> ^( ASTFUNCTIONELEMENTS ^( ASTFUNCTIONELEMENTS0 ( COMMA )* ) ^( ASTFUNCTIONELEMENTS1 ( expression )+ ) )
            	{
            	    // GAMS.g:308:6: ^( ASTFUNCTIONELEMENTS ^( ASTFUNCTIONELEMENTS0 ( COMMA )* ) ^( ASTFUNCTIONELEMENTS1 ( expression )+ ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTIONELEMENTS, "ASTFUNCTIONELEMENTS"), root_1);

            	    // GAMS.g:308:28: ^( ASTFUNCTIONELEMENTS0 ( COMMA )* )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTIONELEMENTS0, "ASTFUNCTIONELEMENTS0"), root_2);

            	    // GAMS.g:308:51: ( COMMA )*
            	    while ( stream_COMMA.HasNext() )
            	    {
            	        adaptor.AddChild(root_2, stream_COMMA.NextNode());

            	    }
            	    stream_COMMA.Reset();

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // GAMS.g:308:59: ^( ASTFUNCTIONELEMENTS1 ( expression )+ )
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
            	Memoize(input, 29, functionElements_StartIndex); 
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
    // GAMS.g:310:1: sum : ( SUM L1 sumControlled ( conditional )? COMMA expression R1 -> ^( ASTSUM ^( ASTSUM0 L1 COMMA R1 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) | SUM L2 sumControlled ( conditional )? COMMA expression R2 -> ^( ASTSUM ^( ASTSUM0 L2 COMMA R2 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) | SUM L3 sumControlled ( conditional )? COMMA expression R3 -> ^( ASTSUM ^( ASTSUM0 L3 COMMA R3 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) );
    public GAMSParser.sum_return sum() // throws RecognitionException [1]
    {   
        GAMSParser.sum_return retval = new GAMSParser.sum_return();
        retval.Start = input.LT(1);
        int sum_StartIndex = input.Index();
        object root_0 = null;

        IToken SUM117 = null;
        IToken L1118 = null;
        IToken COMMA121 = null;
        IToken R1123 = null;
        IToken SUM124 = null;
        IToken L2125 = null;
        IToken COMMA128 = null;
        IToken R2130 = null;
        IToken SUM131 = null;
        IToken L3132 = null;
        IToken COMMA135 = null;
        IToken R3137 = null;
        GAMSParser.sumControlled_return sumControlled119 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional120 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression122 = default(GAMSParser.expression_return);

        GAMSParser.sumControlled_return sumControlled126 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional127 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression129 = default(GAMSParser.expression_return);

        GAMSParser.sumControlled_return sumControlled133 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional134 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression136 = default(GAMSParser.expression_return);


        object SUM117_tree=null;
        object L1118_tree=null;
        object COMMA121_tree=null;
        object R1123_tree=null;
        object SUM124_tree=null;
        object L2125_tree=null;
        object COMMA128_tree=null;
        object R2130_tree=null;
        object SUM131_tree=null;
        object L3132_tree=null;
        object COMMA135_tree=null;
        object R3137_tree=null;
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
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 30) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:310:4: ( SUM L1 sumControlled ( conditional )? COMMA expression R1 -> ^( ASTSUM ^( ASTSUM0 L1 COMMA R1 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) | SUM L2 sumControlled ( conditional )? COMMA expression R2 -> ^( ASTSUM ^( ASTSUM0 L2 COMMA R2 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) | SUM L3 sumControlled ( conditional )? COMMA expression R3 -> ^( ASTSUM ^( ASTSUM0 L3 COMMA R3 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) ) )
            int alt28 = 3;
            int LA28_0 = input.LA(1);

            if ( (LA28_0 == SUM) )
            {
                switch ( input.LA(2) ) 
                {
                case L1:
                	{
                    alt28 = 1;
                    }
                    break;
                case L2:
                	{
                    alt28 = 2;
                    }
                    break;
                case L3:
                	{
                    alt28 = 3;
                    }
                    break;
                	default:
                	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                	    NoViableAltException nvae_d28s1 =
                	        new NoViableAltException("", 28, 1, input);

                	    throw nvae_d28s1;
                }

            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d28s0 =
                    new NoViableAltException("", 28, 0, input);

                throw nvae_d28s0;
            }
            switch (alt28) 
            {
                case 1 :
                    // GAMS.g:310:7: SUM L1 sumControlled ( conditional )? COMMA expression R1
                    {
                    	SUM117=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum1838); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_SUM.Add(SUM117);

                    	L1118=(IToken)Match(input,L1,FOLLOW_L1_in_sum1840); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L1118);

                    	PushFollow(FOLLOW_sumControlled_in_sum1842);
                    	sumControlled119 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sumControlled.Add(sumControlled119.Tree);
                    	// GAMS.g:310:28: ( conditional )?
                    	int alt25 = 2;
                    	int LA25_0 = input.LA(1);

                    	if ( (LA25_0 == DOLLAR) )
                    	{
                    	    alt25 = 1;
                    	}
                    	switch (alt25) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:0:0: conditional
                    	        {
                    	        	PushFollow(FOLLOW_conditional_in_sum1844);
                    	        	conditional120 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional120.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA121=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum1847); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA121);

                    	PushFollow(FOLLOW_expression_in_sum1849);
                    	expression122 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression122.Tree);
                    	R1123=(IToken)Match(input,R1,FOLLOW_R1_in_sum1851); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R1123);



                    	// AST REWRITE
                    	// elements:          R1, L1, sumControlled, COMMA, conditional, expression
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 310:61: -> ^( ASTSUM ^( ASTSUM0 L1 COMMA R1 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	{
                    	    // GAMS.g:310:64: ^( ASTSUM ^( ASTSUM0 L1 COMMA R1 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM, "ASTSUM"), root_1);

                    	    // GAMS.g:310:73: ^( ASTSUM0 L1 COMMA R1 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM0, "ASTSUM0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L1.NextNode());
                    	    adaptor.AddChild(root_2, stream_COMMA.NextNode());
                    	    adaptor.AddChild(root_2, stream_R1.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:310:96: ^( ASTSUM1 sumControlled )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM1, "ASTSUM1"), root_2);

                    	    adaptor.AddChild(root_2, stream_sumControlled.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:310:121: ^( ASTSUM2 ( conditional )? )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM2, "ASTSUM2"), root_2);

                    	    // GAMS.g:310:131: ( conditional )?
                    	    if ( stream_conditional.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_2, stream_conditional.NextTree());

                    	    }
                    	    stream_conditional.Reset();

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:310:146: ^( ASTSUM3 expression )
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
                    // GAMS.g:311:7: SUM L2 sumControlled ( conditional )? COMMA expression R2
                    {
                    	SUM124=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum1895); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_SUM.Add(SUM124);

                    	L2125=(IToken)Match(input,L2,FOLLOW_L2_in_sum1897); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L2125);

                    	PushFollow(FOLLOW_sumControlled_in_sum1899);
                    	sumControlled126 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sumControlled.Add(sumControlled126.Tree);
                    	// GAMS.g:311:28: ( conditional )?
                    	int alt26 = 2;
                    	int LA26_0 = input.LA(1);

                    	if ( (LA26_0 == DOLLAR) )
                    	{
                    	    alt26 = 1;
                    	}
                    	switch (alt26) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:0:0: conditional
                    	        {
                    	        	PushFollow(FOLLOW_conditional_in_sum1901);
                    	        	conditional127 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional127.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA128=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum1904); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA128);

                    	PushFollow(FOLLOW_expression_in_sum1906);
                    	expression129 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression129.Tree);
                    	R2130=(IToken)Match(input,R2,FOLLOW_R2_in_sum1908); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R2130);



                    	// AST REWRITE
                    	// elements:          COMMA, R2, conditional, sumControlled, L2, expression
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 311:61: -> ^( ASTSUM ^( ASTSUM0 L2 COMMA R2 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	{
                    	    // GAMS.g:311:64: ^( ASTSUM ^( ASTSUM0 L2 COMMA R2 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM, "ASTSUM"), root_1);

                    	    // GAMS.g:311:73: ^( ASTSUM0 L2 COMMA R2 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM0, "ASTSUM0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L2.NextNode());
                    	    adaptor.AddChild(root_2, stream_COMMA.NextNode());
                    	    adaptor.AddChild(root_2, stream_R2.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:311:96: ^( ASTSUM1 sumControlled )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM1, "ASTSUM1"), root_2);

                    	    adaptor.AddChild(root_2, stream_sumControlled.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:311:121: ^( ASTSUM2 ( conditional )? )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM2, "ASTSUM2"), root_2);

                    	    // GAMS.g:311:131: ( conditional )?
                    	    if ( stream_conditional.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_2, stream_conditional.NextTree());

                    	    }
                    	    stream_conditional.Reset();

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:311:146: ^( ASTSUM3 expression )
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
                    // GAMS.g:312:7: SUM L3 sumControlled ( conditional )? COMMA expression R3
                    {
                    	SUM131=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum1952); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_SUM.Add(SUM131);

                    	L3132=(IToken)Match(input,L3,FOLLOW_L3_in_sum1954); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L3132);

                    	PushFollow(FOLLOW_sumControlled_in_sum1956);
                    	sumControlled133 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sumControlled.Add(sumControlled133.Tree);
                    	// GAMS.g:312:28: ( conditional )?
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
                    	        	PushFollow(FOLLOW_conditional_in_sum1958);
                    	        	conditional134 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional134.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA135=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum1961); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA135);

                    	PushFollow(FOLLOW_expression_in_sum1963);
                    	expression136 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression136.Tree);
                    	R3137=(IToken)Match(input,R3,FOLLOW_R3_in_sum1965); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R3137);



                    	// AST REWRITE
                    	// elements:          R3, L3, COMMA, conditional, sumControlled, expression
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 312:61: -> ^( ASTSUM ^( ASTSUM0 L3 COMMA R3 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	{
                    	    // GAMS.g:312:64: ^( ASTSUM ^( ASTSUM0 L3 COMMA R3 ) ^( ASTSUM1 sumControlled ) ^( ASTSUM2 ( conditional )? ) ^( ASTSUM3 expression ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM, "ASTSUM"), root_1);

                    	    // GAMS.g:312:73: ^( ASTSUM0 L3 COMMA R3 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM0, "ASTSUM0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L3.NextNode());
                    	    adaptor.AddChild(root_2, stream_COMMA.NextNode());
                    	    adaptor.AddChild(root_2, stream_R3.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:312:96: ^( ASTSUM1 sumControlled )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM1, "ASTSUM1"), root_2);

                    	    adaptor.AddChild(root_2, stream_sumControlled.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:312:121: ^( ASTSUM2 ( conditional )? )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUM2, "ASTSUM2"), root_2);

                    	    // GAMS.g:312:131: ( conditional )?
                    	    if ( stream_conditional.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_2, stream_conditional.NextTree());

                    	    }
                    	    stream_conditional.Reset();

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:312:146: ^( ASTSUM3 expression )
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
            	Memoize(input, 30, sum_StartIndex); 
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
    // GAMS.g:315:1: sumControlled : ( variable -> ^( ASTSUMCONTROLLEDSIMPLE variable ) | L1 indexerElements R1 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L1 R1 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) | L2 indexerElements R2 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L2 R2 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) | L3 indexerElements R3 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L3 R3 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) );
    public GAMSParser.sumControlled_return sumControlled() // throws RecognitionException [1]
    {   
        GAMSParser.sumControlled_return retval = new GAMSParser.sumControlled_return();
        retval.Start = input.LT(1);
        int sumControlled_StartIndex = input.Index();
        object root_0 = null;

        IToken L1139 = null;
        IToken R1141 = null;
        IToken L2142 = null;
        IToken R2144 = null;
        IToken L3145 = null;
        IToken R3147 = null;
        GAMSParser.variable_return variable138 = default(GAMSParser.variable_return);

        GAMSParser.indexerElements_return indexerElements140 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements143 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements146 = default(GAMSParser.indexerElements_return);


        object L1139_tree=null;
        object R1141_tree=null;
        object L2142_tree=null;
        object R2144_tree=null;
        object L3145_tree=null;
        object R3147_tree=null;
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
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 31) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:315:14: ( variable -> ^( ASTSUMCONTROLLEDSIMPLE variable ) | L1 indexerElements R1 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L1 R1 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) | L2 indexerElements R2 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L2 R2 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) | L3 indexerElements R3 -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L3 R3 ) ^( ASTSUMCONTROLLED2 indexerElements ) ) )
            int alt29 = 4;
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
            case VARIABLES:
            case EQUATIONS:
            case Ident:
            	{
                alt29 = 1;
                }
                break;
            case L1:
            	{
                alt29 = 2;
                }
                break;
            case L2:
            	{
                alt29 = 3;
                }
                break;
            case L3:
            	{
                alt29 = 4;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d29s0 =
            	        new NoViableAltException("", 29, 0, input);

            	    throw nvae_d29s0;
            }

            switch (alt29) 
            {
                case 1 :
                    // GAMS.g:316:11: variable
                    {
                    	PushFollow(FOLLOW_variable_in_sumControlled2020);
                    	variable138 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_variable.Add(variable138.Tree);


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
                    	// 316:33: -> ^( ASTSUMCONTROLLEDSIMPLE variable )
                    	{
                    	    // GAMS.g:316:36: ^( ASTSUMCONTROLLEDSIMPLE variable )
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
                    // GAMS.g:317:5: L1 indexerElements R1
                    {
                    	L1139=(IToken)Match(input,L1,FOLLOW_L1_in_sumControlled2047); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L1139);

                    	PushFollow(FOLLOW_indexerElements_in_sumControlled2049);
                    	indexerElements140 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements140.Tree);
                    	R1141=(IToken)Match(input,R1,FOLLOW_R1_in_sumControlled2051); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R1141);



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
                    	// 317:27: -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L1 R1 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	{
                    	    // GAMS.g:317:30: ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L1 R1 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED, "ASTSUMCONTROLLED"), root_1);

                    	    // GAMS.g:317:49: ^( ASTSUMCONTROLLED0 L1 R1 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED0, "ASTSUMCONTROLLED0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L1.NextNode());
                    	    adaptor.AddChild(root_2, stream_R1.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:317:76: ^( ASTSUMCONTROLLED2 indexerElements )
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
                    // GAMS.g:318:5: L2 indexerElements R2
                    {
                    	L2142=(IToken)Match(input,L2,FOLLOW_L2_in_sumControlled2077); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L2142);

                    	PushFollow(FOLLOW_indexerElements_in_sumControlled2079);
                    	indexerElements143 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements143.Tree);
                    	R2144=(IToken)Match(input,R2,FOLLOW_R2_in_sumControlled2081); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R2144);



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
                    	// 318:27: -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L2 R2 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	{
                    	    // GAMS.g:318:30: ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L2 R2 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED, "ASTSUMCONTROLLED"), root_1);

                    	    // GAMS.g:318:49: ^( ASTSUMCONTROLLED0 L2 R2 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED0, "ASTSUMCONTROLLED0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L2.NextNode());
                    	    adaptor.AddChild(root_2, stream_R2.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:318:76: ^( ASTSUMCONTROLLED2 indexerElements )
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
                    // GAMS.g:319:5: L3 indexerElements R3
                    {
                    	L3145=(IToken)Match(input,L3,FOLLOW_L3_in_sumControlled2107); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L3145);

                    	PushFollow(FOLLOW_indexerElements_in_sumControlled2109);
                    	indexerElements146 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements146.Tree);
                    	R3147=(IToken)Match(input,R3,FOLLOW_R3_in_sumControlled2111); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R3147);



                    	// AST REWRITE
                    	// elements:          L3, indexerElements, R3
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 319:27: -> ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L3 R3 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	{
                    	    // GAMS.g:319:30: ^( ASTSUMCONTROLLED ^( ASTSUMCONTROLLED0 L3 R3 ) ^( ASTSUMCONTROLLED2 indexerElements ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED, "ASTSUMCONTROLLED"), root_1);

                    	    // GAMS.g:319:49: ^( ASTSUMCONTROLLED0 L3 R3 )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSUMCONTROLLED0, "ASTSUMCONTROLLED0"), root_2);

                    	    adaptor.AddChild(root_2, stream_L3.NextNode());
                    	    adaptor.AddChild(root_2, stream_R3.NextNode());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // GAMS.g:319:76: ^( ASTSUMCONTROLLED2 indexerElements )
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
            	Memoize(input, 31, sumControlled_StartIndex); 
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
    // GAMS.g:326:1: ident : ( Ident | extraTokens );
    public GAMSParser.ident_return ident() // throws RecognitionException [1]
    {   
        GAMSParser.ident_return retval = new GAMSParser.ident_return();
        retval.Start = input.LT(1);
        int ident_StartIndex = input.Index();
        object root_0 = null;

        IToken Ident148 = null;
        GAMSParser.extraTokens_return extraTokens149 = default(GAMSParser.extraTokens_return);


        object Ident148_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 32) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:326:9: ( Ident | extraTokens )
            int alt30 = 2;
            int LA30_0 = input.LA(1);

            if ( (LA30_0 == Ident) )
            {
                alt30 = 1;
            }
            else if ( ((LA30_0 >= SUM && LA30_0 <= EQUATIONS)) )
            {
                alt30 = 2;
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
                    // GAMS.g:326:12: Ident
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Ident148=(IToken)Match(input,Ident,FOLLOW_Ident_in_ident2147); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Ident148_tree = (object)adaptor.Create(Ident148);
                    		adaptor.AddChild(root_0, Ident148_tree);
                    	}

                    }
                    break;
                case 2 :
                    // GAMS.g:326:20: extraTokens
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_extraTokens_in_ident2151);
                    	extraTokens149 = extraTokens();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, extraTokens149.Tree);

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
            	Memoize(input, 32, ident_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "ident"

    // $ANTLR start "synpred16_GAMS"
    public void synpred16_GAMS_fragment() {
        // GAMS.g:191:7: ( equ )
        // GAMS.g:191:7: equ
        {
        	PushFollow(FOLLOW_equ_in_synpred16_GAMS496);
        	equ();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred16_GAMS"

    // $ANTLR start "synpred17_GAMS"
    public void synpred17_GAMS_fragment() {
        // GAMS.g:192:9: ( vardef )
        // GAMS.g:192:9: vardef
        {
        	PushFollow(FOLLOW_vardef_in_synpred17_GAMS506);
        	vardef();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred17_GAMS"

    // $ANTLR start "synpred18_GAMS"
    public void synpred18_GAMS_fragment() {
        // GAMS.g:193:6: ( variables )
        // GAMS.g:193:6: variables
        {
        	PushFollow(FOLLOW_variables_in_synpred18_GAMS513);
        	variables();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred18_GAMS"

    // $ANTLR start "synpred24_GAMS"
    public void synpred24_GAMS_fragment() {
        // GAMS.g:214:55: ( conditional )
        // GAMS.g:214:55: conditional
        {
        	PushFollow(FOLLOW_conditional_in_synpred24_GAMS718);
        	conditional();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred24_GAMS"

    // $ANTLR start "synpred31_GAMS"
    public void synpred31_GAMS_fragment() {
        // GAMS.g:251:28: ( OR andExpression )
        // GAMS.g:251:28: OR andExpression
        {
        	Match(input,OR,FOLLOW_OR_in_synpred31_GAMS1116); if (state.failed) return ;
        	PushFollow(FOLLOW_andExpression_in_synpred31_GAMS1119);
        	andExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred31_GAMS"

    // $ANTLR start "synpred32_GAMS"
    public void synpred32_GAMS_fragment() {
        // GAMS.g:253:31: ( AND notExpression )
        // GAMS.g:253:31: AND notExpression
        {
        	Match(input,AND,FOLLOW_AND_in_synpred32_GAMS1131); if (state.failed) return ;
        	PushFollow(FOLLOW_notExpression_in_synpred32_GAMS1134);
        	notExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred32_GAMS"

    // $ANTLR start "synpred33_GAMS"
    public void synpred33_GAMS_fragment() {
        // GAMS.g:255:16: ( NOT logicalExpression )
        // GAMS.g:255:16: NOT logicalExpression
        {
        	Match(input,NOT,FOLLOW_NOT_in_synpred33_GAMS1145); if (state.failed) return ;
        	PushFollow(FOLLOW_logicalExpression_in_synpred33_GAMS1147);
        	logicalExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred33_GAMS"

    // $ANTLR start "synpred34_GAMS"
    public void synpred34_GAMS_fragment() {
        // GAMS.g:258:41: ( logical additiveExpression )
        // GAMS.g:258:41: logical additiveExpression
        {
        	PushFollow(FOLLOW_logical_in_synpred34_GAMS1178);
        	logical();
        	state.followingStackPointer--;
        	if (state.failed) return ;
        	PushFollow(FOLLOW_additiveExpression_in_synpred34_GAMS1181);
        	additiveExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred34_GAMS"

    // $ANTLR start "synpred36_GAMS"
    public void synpred36_GAMS_fragment() {
        // GAMS.g:260:48: ( ( PLUS | MINUS ) multiplicativeExpression )
        // GAMS.g:260:48: ( PLUS | MINUS ) multiplicativeExpression
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

        	PushFollow(FOLLOW_multiplicativeExpression_in_synpred36_GAMS1201);
        	multiplicativeExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred36_GAMS"

    // $ANTLR start "synpred38_GAMS"
    public void synpred38_GAMS_fragment() {
        // GAMS.g:262:45: ( ( MULT | DIV ) powerExpression )
        // GAMS.g:262:45: ( MULT | DIV ) powerExpression
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

        	PushFollow(FOLLOW_powerExpression_in_synpred38_GAMS1222);
        	powerExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred38_GAMS"

    // $ANTLR start "synpred39_GAMS"
    public void synpred39_GAMS_fragment() {
        // GAMS.g:264:36: ( STARS unaryExpression )
        // GAMS.g:264:36: STARS unaryExpression
        {
        	Match(input,STARS,FOLLOW_STARS_in_synpred39_GAMS1236); if (state.failed) return ;
        	PushFollow(FOLLOW_unaryExpression_in_synpred39_GAMS1239);
        	unaryExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred39_GAMS"

    // $ANTLR start "synpred41_GAMS"
    public void synpred41_GAMS_fragment() {
        // GAMS.g:276:9: ( primaryExpression conditional )
        // GAMS.g:276:9: primaryExpression conditional
        {
        	PushFollow(FOLLOW_primaryExpression_in_synpred41_GAMS1298);
        	primaryExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;
        	PushFollow(FOLLOW_conditional_in_synpred41_GAMS1300);
        	conditional();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred41_GAMS"

    // $ANTLR start "synpred52_GAMS"
    public void synpred52_GAMS_fragment() {
        // GAMS.g:291:6: ( sum )
        // GAMS.g:291:6: sum
        {
        	PushFollow(FOLLOW_sum_in_synpred52_GAMS1506);
        	sum();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred52_GAMS"

    // $ANTLR start "synpred53_GAMS"
    public void synpred53_GAMS_fragment() {
        // GAMS.g:292:6: ( function )
        // GAMS.g:292:6: function
        {
        	PushFollow(FOLLOW_function_in_synpred53_GAMS1543);
        	function();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred53_GAMS"

    // Delegated rules

   	public bool synpred33_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred33_GAMS_fragment(); // can never throw exception
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
   	public bool synpred16_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred16_GAMS_fragment(); // can never throw exception
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
   	public bool synpred34_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred34_GAMS_fragment(); // can never throw exception
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
   	public bool synpred17_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred17_GAMS_fragment(); // can never throw exception
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
   	public bool synpred52_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred52_GAMS_fragment(); // can never throw exception
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
   	public bool synpred53_GAMS() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred53_GAMS_fragment(); // can never throw exception
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
   	protected DFA6 dfa6;
   	protected DFA7 dfa7;
   	protected DFA8 dfa8;
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
   	protected DFA21 dfa21;
   	protected DFA22 dfa22;
	private void InitializeCyclicDFAs()
	{
    	this.dfa2 = new DFA2(this);
    	this.dfa6 = new DFA6(this);
    	this.dfa7 = new DFA7(this);
    	this.dfa8 = new DFA8(this);
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
    	this.dfa21 = new DFA21(this);
    	this.dfa22 = new DFA22(this);
	    this.dfa2.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA2_SpecialStateTransition);


	    this.dfa8.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA8_SpecialStateTransition);

	    this.dfa12.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA12_SpecialStateTransition);
	    this.dfa13.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA13_SpecialStateTransition);
	    this.dfa14.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA14_SpecialStateTransition);
	    this.dfa15.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA15_SpecialStateTransition);
	    this.dfa16.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA16_SpecialStateTransition);
	    this.dfa17.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA17_SpecialStateTransition);
	    this.dfa18.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA18_SpecialStateTransition);

	    this.dfa20.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA20_SpecialStateTransition);

	    this.dfa22.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA22_SpecialStateTransition);
	}

    const string DFA2_eotS =
        "\x1d\uffff";
    const string DFA2_eofS =
        "\x1d\uffff";
    const string DFA2_minS =
        "\x01\x44\x01\x53\x01\x44\x01\x53\x03\x00\x04\uffff\x02\x00\x01"+
        "\uffff\x03\x00\x04\uffff\x03\x00\x05\uffff";
    const string DFA2_maxS =
        "\x01\x6d\x01\x63\x01\x6d\x01\x63\x03\x00\x04\uffff\x02\x00\x01"+
        "\uffff\x03\x00\x04\uffff\x03\x00\x05\uffff";
    const string DFA2_acceptS =
        "\x07\uffff\x01\x01\x01\x02\x12\uffff\x01\x03\x01\x04";
    const string DFA2_specialS =
        "\x04\uffff\x01\x00\x01\x01\x01\x02\x04\uffff\x01\x03\x01\x04\x01"+
        "\uffff\x01\x05\x01\x06\x01\x07\x04\uffff\x01\x08\x01\x09\x01\x0a"+
        "\x05\uffff}>";
    static readonly string[] DFA2_transitionS = {
            "\x0d\x03\x01\x02\x01\x03\x1a\uffff\x01\x01",
            "\x01\x07\x02\uffff\x01\x08\x01\uffff\x01\x08\x01\x04\x01\uffff"+
            "\x01\x05\x01\uffff\x01\x06\x05\uffff\x01\x08",
            "\x0f\x0c\x01\x07\x02\uffff\x01\x08\x01\uffff\x01\x08\x01\x0e"+
            "\x01\uffff\x01\x0f\x01\uffff\x01\x10\x05\uffff\x01\x08\x09\uffff"+
            "\x01\x0b",
            "\x01\x07\x02\uffff\x01\x08\x01\uffff\x01\x08\x01\x15\x01\uffff"+
            "\x01\x16\x01\uffff\x01\x17\x05\uffff\x01\x08",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "",
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
            get { return "191:1: expr : ( equ | vardef | variables | equations );"; }
        }

    }


    protected internal int DFA2_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA2_4 = input.LA(1);

                   	 
                   	int index2_4 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred16_GAMS()) ) { s = 7; }

                   	else if ( (synpred17_GAMS()) ) { s = 8; }

                   	 
                   	input.Seek(index2_4);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA2_5 = input.LA(1);

                   	 
                   	int index2_5 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred16_GAMS()) ) { s = 7; }

                   	else if ( (synpred17_GAMS()) ) { s = 8; }

                   	 
                   	input.Seek(index2_5);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA2_6 = input.LA(1);

                   	 
                   	int index2_6 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred16_GAMS()) ) { s = 7; }

                   	else if ( (synpred17_GAMS()) ) { s = 8; }

                   	 
                   	input.Seek(index2_6);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA2_11 = input.LA(1);

                   	 
                   	int index2_11 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 28; }

                   	 
                   	input.Seek(index2_11);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA2_12 = input.LA(1);

                   	 
                   	int index2_12 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred18_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 28; }

                   	 
                   	input.Seek(index2_12);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA2_14 = input.LA(1);

                   	 
                   	int index2_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred16_GAMS()) ) { s = 7; }

                   	else if ( (synpred17_GAMS()) ) { s = 8; }

                   	 
                   	input.Seek(index2_14);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 6 : 
                   	int LA2_15 = input.LA(1);

                   	 
                   	int index2_15 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred16_GAMS()) ) { s = 7; }

                   	else if ( (synpred17_GAMS()) ) { s = 8; }

                   	 
                   	input.Seek(index2_15);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 7 : 
                   	int LA2_16 = input.LA(1);

                   	 
                   	int index2_16 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred16_GAMS()) ) { s = 7; }

                   	else if ( (synpred17_GAMS()) ) { s = 8; }

                   	 
                   	input.Seek(index2_16);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 8 : 
                   	int LA2_21 = input.LA(1);

                   	 
                   	int index2_21 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred16_GAMS()) ) { s = 7; }

                   	else if ( (synpred17_GAMS()) ) { s = 8; }

                   	 
                   	input.Seek(index2_21);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 9 : 
                   	int LA2_22 = input.LA(1);

                   	 
                   	int index2_22 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred16_GAMS()) ) { s = 7; }

                   	else if ( (synpred17_GAMS()) ) { s = 8; }

                   	 
                   	input.Seek(index2_22);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 10 : 
                   	int LA2_23 = input.LA(1);

                   	 
                   	int index2_23 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred16_GAMS()) ) { s = 7; }

                   	else if ( (synpred17_GAMS()) ) { s = 8; }

                   	 
                   	input.Seek(index2_23);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae2 =
            new NoViableAltException(dfa.Description, 2, _s, input);
        dfa.Error(nvae2);
        throw nvae2;
    }
    const string DFA6_eotS =
        "\x14\uffff";
    const string DFA6_eofS =
        "\x01\x02\x13\uffff";
    const string DFA6_minS =
        "\x01\x45\x13\uffff";
    const string DFA6_maxS =
        "\x01\x6b\x13\uffff";
    const string DFA6_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x11\uffff";
    const string DFA6_specialS =
        "\x14\uffff}>";
    static readonly string[] DFA6_transitionS = {
            "\x02\x02\x0d\uffff\x04\x02\x01\x01\x06\x02\x01\uffff\x01\x02"+
            "\x01\uffff\x0a\x02",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
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
            get { return "214:34: ( DOT variable )?"; }
        }

    }

    const string DFA7_eotS =
        "\x13\uffff";
    const string DFA7_eofS =
        "\x01\x04\x12\uffff";
    const string DFA7_minS =
        "\x01\x45\x12\uffff";
    const string DFA7_maxS =
        "\x01\x6b\x12\uffff";
    const string DFA7_acceptS =
        "\x01\uffff\x01\x01\x02\uffff\x01\x02\x0e\uffff";
    const string DFA7_specialS =
        "\x13\uffff}>";
    static readonly string[] DFA7_transitionS = {
            "\x02\x04\x0d\uffff\x04\x04\x01\uffff\x01\x01\x01\x04\x01\x01"+
            "\x01\x04\x01\x01\x01\x04\x01\uffff\x01\x04\x01\uffff\x0a\x04",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
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
            get { return "214:50: ( idx )?"; }
        }

    }

    const string DFA8_eotS =
        "\x1c\uffff";
    const string DFA8_eofS =
        "\x01\x02\x1b\uffff";
    const string DFA8_minS =
        "\x01\x45\x01\x44\x0e\uffff\x0b\x00\x01\uffff";
    const string DFA8_maxS =
        "\x01\x6b\x01\x6d\x0e\uffff\x0b\x00\x01\uffff";
    const string DFA8_acceptS =
        "\x02\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA8_specialS =
        "\x10\uffff\x01\x00\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01"+
        "\x06\x01\x07\x01\x08\x01\x09\x01\x0a\x01\uffff}>";
    static readonly string[] DFA8_transitionS = {
            "\x02\x02\x0d\uffff\x04\x02\x02\uffff\x01\x02\x01\uffff\x01"+
            "\x02\x01\uffff\x01\x02\x01\uffff\x01\x02\x01\uffff\x01\x02\x01"+
            "\x01\x08\x02",
            "\x01\x17\x02\x1a\x01\x10\x09\x18\x02\x1a\x06\uffff\x01\x12"+
            "\x01\uffff\x01\x13\x01\uffff\x01\x14\x03\uffff\x01\x15\x01\x11"+
            "\x09\uffff\x01\x16\x01\x19",
            "",
            "",
            "",
            "",
            "",
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
            get { return "214:55: ( conditional )?"; }
        }

    }


    protected internal int DFA8_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA8_16 = input.LA(1);

                   	 
                   	int index8_16 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index8_16);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA8_17 = input.LA(1);

                   	 
                   	int index8_17 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index8_17);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA8_18 = input.LA(1);

                   	 
                   	int index8_18 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index8_18);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA8_19 = input.LA(1);

                   	 
                   	int index8_19 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index8_19);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA8_20 = input.LA(1);

                   	 
                   	int index8_20 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index8_20);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA8_21 = input.LA(1);

                   	 
                   	int index8_21 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index8_21);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 6 : 
                   	int LA8_22 = input.LA(1);

                   	 
                   	int index8_22 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index8_22);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 7 : 
                   	int LA8_23 = input.LA(1);

                   	 
                   	int index8_23 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index8_23);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 8 : 
                   	int LA8_24 = input.LA(1);

                   	 
                   	int index8_24 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index8_24);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 9 : 
                   	int LA8_25 = input.LA(1);

                   	 
                   	int index8_25 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index8_25);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 10 : 
                   	int LA8_26 = input.LA(1);

                   	 
                   	int index8_26 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred24_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index8_26);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae8 =
            new NoViableAltException(dfa.Description, 8, _s, input);
        dfa.Error(nvae8);
        throw nvae8;
    }
    const string DFA11_eotS =
        "\x12\uffff";
    const string DFA11_eofS =
        "\x02\uffff\x02\x06\x0e\uffff";
    const string DFA11_minS =
        "\x01\x44\x01\uffff\x02\x57\x0e\uffff";
    const string DFA11_maxS =
        "\x01\x6d\x01\uffff\x02\x62\x0e\uffff";
    const string DFA11_acceptS =
        "\x01\uffff\x01\x01\x02\uffff\x01\x02\x01\x03\x01\x04\x0b\uffff";
    const string DFA11_specialS =
        "\x12\uffff}>";
    static readonly string[] DFA11_transitionS = {
            "\x0f\x03\x0c\uffff\x01\x01\x0d\uffff\x01\x02",
            "",
            "\x01\x06\x02\uffff\x01\x06\x01\uffff\x01\x06\x01\uffff\x01"+
            "\x06\x01\uffff\x01\x04\x01\uffff\x01\x05",
            "\x01\x06\x02\uffff\x01\x06\x01\uffff\x01\x06\x01\uffff\x01"+
            "\x06\x01\uffff\x01\x04\x01\uffff\x01\x05",
            "",
            "",
            "",
            "",
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
            get { return "227:1: variableLagLead : ( StringInQuotes -> ^( ASTVARIABLEANDLEAD StringInQuotes ) | variable PLUS Integer -> ^( ASTVARIABLEANDLEAD variable PLUS Integer ) | variable MINUS Integer -> ^( ASTVARIABLEANDLEAD variable MINUS Integer ) | variable -> ^( ASTVARIABLEANDLEAD variable ) );"; }
        }

    }

    const string DFA12_eotS =
        "\x1c\uffff";
    const string DFA12_eofS =
        "\x01\x01\x1b\uffff";
    const string DFA12_minS =
        "\x01\x45\x09\uffff\x01\x00\x11\uffff";
    const string DFA12_maxS =
        "\x01\x6b\x09\uffff\x01\x00\x11\uffff";
    const string DFA12_acceptS =
        "\x01\uffff\x01\x02\x19\uffff\x01\x01";
    const string DFA12_specialS =
        "\x0a\uffff\x01\x00\x11\uffff}>";
    static readonly string[] DFA12_transitionS = {
            "\x01\x01\x01\x0a\x0d\uffff\x04\x01\x02\uffff\x01\x01\x01\uffff"+
            "\x01\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x0a\x01",
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
            get { return "()* loopback of 251:27: ( OR andExpression )*"; }
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
                   	if ( (synpred31_GAMS()) ) { s = 27; }

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
        "\x1c\uffff";
    const string DFA13_eofS =
        "\x01\x01\x1b\uffff";
    const string DFA13_minS =
        "\x01\x45\x09\uffff\x01\x00\x11\uffff";
    const string DFA13_maxS =
        "\x01\x6b\x09\uffff\x01\x00\x11\uffff";
    const string DFA13_acceptS =
        "\x01\uffff\x01\x02\x19\uffff\x01\x01";
    const string DFA13_specialS =
        "\x0a\uffff\x01\x00\x11\uffff}>";
    static readonly string[] DFA13_transitionS = {
            "\x01\x0a\x01\x01\x0d\uffff\x04\x01\x02\uffff\x01\x01\x01\uffff"+
            "\x01\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x0a\x01",
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
            get { return "()* loopback of 253:30: ( AND notExpression )*"; }
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
                   	if ( (synpred32_GAMS()) ) { s = 27; }

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
        "\x26\uffff";
    const string DFA14_eofS =
        "\x01\uffff\x01\x02\x24\uffff";
    const string DFA14_minS =
        "\x02\x44\x0b\uffff\x03\x00\x03\uffff\x01\x00\x01\uffff\x02\x00"+
        "\x0f\uffff";
    const string DFA14_maxS =
        "\x02\x6d\x0b\uffff\x03\x00\x03\uffff\x01\x00\x01\uffff\x02\x00"+
        "\x0f\uffff";
    const string DFA14_acceptS =
        "\x02\uffff\x01\x02\x1d\uffff\x01\x01\x05\uffff";
    const string DFA14_specialS =
        "\x0d\uffff\x01\x00\x01\x01\x01\x02\x03\uffff\x01\x03\x01\uffff"+
        "\x01\x04\x01\x05\x0f\uffff}>";
    static readonly string[] DFA14_transitionS = {
            "\x03\x02\x01\x01\x0b\x02\x06\uffff\x01\x02\x01\uffff\x01\x02"+
            "\x01\uffff\x01\x02\x03\uffff\x02\x02\x09\uffff\x02\x02",
            "\x01\x20\x01\x15\x01\x16\x0c\x20\x01\uffff\x05\x02\x01\x0d"+
            "\x01\x02\x01\x0e\x01\x02\x01\x0f\x01\x02\x01\uffff\x01\x02\x01"+
            "\x20\x01\x13\x09\x02\x02\x20",
            "",
            "",
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
            get { return "255:1: notExpression : ( NOT logicalExpression -> ^( NOT logicalExpression ) | logicalExpression );"; }
        }

    }


    protected internal int DFA14_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA14_13 = input.LA(1);

                   	 
                   	int index14_13 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred33_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index14_13);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA14_14 = input.LA(1);

                   	 
                   	int index14_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred33_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index14_14);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA14_15 = input.LA(1);

                   	 
                   	int index14_15 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred33_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index14_15);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA14_19 = input.LA(1);

                   	 
                   	int index14_19 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred33_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index14_19);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA14_21 = input.LA(1);

                   	 
                   	int index14_21 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred33_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index14_21);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA14_22 = input.LA(1);

                   	 
                   	int index14_22 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred33_GAMS()) ) { s = 32; }

                   	else if ( (true) ) { s = 2; }

                   	 
                   	input.Seek(index14_22);
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
        "\x26\uffff";
    const string DFA15_eofS =
        "\x01\x01\x25\uffff";
    const string DFA15_minS =
        "\x01\x45\x04\uffff\x01\x00\x04\uffff\x01\x00\x1b\uffff";
    const string DFA15_maxS =
        "\x01\x6b\x04\uffff\x01\x00\x04\uffff\x01\x00\x1b\uffff";
    const string DFA15_acceptS =
        "\x01\uffff\x01\x02\x19\uffff\x01\x01\x0a\uffff";
    const string DFA15_specialS =
        "\x05\uffff\x01\x00\x04\uffff\x01\x01\x1b\uffff}>";
    static readonly string[] DFA15_transitionS = {
            "\x02\x01\x0d\uffff\x02\x01\x01\x05\x01\x01\x02\uffff\x01\x01"+
            "\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff"+
            "\x05\x01\x05\x0a",
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
            get { return "()* loopback of 258:40: ( logical additiveExpression )*"; }
        }

    }


    protected internal int DFA15_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA15_5 = input.LA(1);

                   	 
                   	int index15_5 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred34_GAMS()) ) { s = 27; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index15_5);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA15_10 = input.LA(1);

                   	 
                   	int index15_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred34_GAMS()) ) { s = 27; }

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
        "\x1b\uffff";
    const string DFA16_eofS =
        "\x01\x01\x1a\uffff";
    const string DFA16_minS =
        "\x01\x45\x09\uffff\x01\x00\x10\uffff";
    const string DFA16_maxS =
        "\x01\x6b\x09\uffff\x01\x00\x10\uffff";
    const string DFA16_acceptS =
        "\x01\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA16_specialS =
        "\x0a\uffff\x01\x00\x10\uffff}>";
    static readonly string[] DFA16_transitionS = {
            "\x02\x01\x0d\uffff\x04\x01\x02\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x01\x01\x01\uffff\x01\x0a\x01\uffff\x01\x0a\x09"+
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
            get { return "()* loopback of 260:46: ( ( PLUS | MINUS ) multiplicativeExpression )*"; }
        }

    }


    protected internal int DFA16_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA16_10 = input.LA(1);

                   	 
                   	int index16_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred36_GAMS()) ) { s = 26; }

                   	else if ( (true) ) { s = 1; }

                   	 
                   	input.Seek(index16_10);
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
        "\x1b\uffff";
    const string DFA17_eofS =
        "\x01\x01\x1a\uffff";
    const string DFA17_minS =
        "\x01\x45\x09\uffff\x01\x00\x10\uffff";
    const string DFA17_maxS =
        "\x01\x6b\x09\uffff\x01\x00\x10\uffff";
    const string DFA17_acceptS =
        "\x01\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA17_specialS =
        "\x0a\uffff\x01\x00\x10\uffff}>";
    static readonly string[] DFA17_transitionS = {
            "\x02\x01\x0d\uffff\x04\x01\x02\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x02\x01\x02"+
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
            get { return "()* loopback of 262:43: ( ( MULT | DIV ) powerExpression )*"; }
        }

    }


    protected internal int DFA17_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA17_10 = input.LA(1);

                   	 
                   	int index17_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred38_GAMS()) ) { s = 26; }

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
        "\x01\x45\x09\uffff\x01\x00\x10\uffff";
    const string DFA18_maxS =
        "\x01\x6b\x09\uffff\x01\x00\x10\uffff";
    const string DFA18_acceptS =
        "\x01\uffff\x01\x02\x18\uffff\x01\x01";
    const string DFA18_specialS =
        "\x0a\uffff\x01\x00\x10\uffff}>";
    static readonly string[] DFA18_transitionS = {
            "\x02\x01\x0d\uffff\x04\x01\x02\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x04\x01\x01"+
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
            get { return "()* loopback of 264:34: ( STARS unaryExpression )*"; }
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
                   	if ( (synpred39_GAMS()) ) { s = 26; }

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
        "\x0b\uffff";
    const string DFA19_eofS =
        "\x0b\uffff";
    const string DFA19_minS =
        "\x01\x44\x0a\uffff";
    const string DFA19_maxS =
        "\x01\x6d\x0a\uffff";
    const string DFA19_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x08\uffff";
    const string DFA19_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA19_transitionS = {
            "\x0f\x02\x06\uffff\x01\x02\x01\uffff\x01\x02\x01\uffff\x01"+
            "\x02\x03\uffff\x01\x02\x01\x01\x09\uffff\x02\x02",
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
            get { return "266:1: unaryExpression : ( MINUS dollarExpression -> ^( NEGATE dollarExpression ) | dollarExpression );"; }
        }

    }

    const string DFA20_eotS =
        "\u0097\uffff";
    const string DFA20_eofS =
        "\u0097\uffff";
    const string DFA20_minS =
        "\x04\x44\x27\x00\x6c\uffff";
    const string DFA20_maxS =
        "\x04\x6d\x27\x00\x6c\uffff";
    const string DFA20_acceptS =
        "\x3a\uffff\x01\x01\x01\x02\x5b\uffff";
    const string DFA20_specialS =
        "\x04\uffff\x01\x00\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01"+
        "\x06\x01\x07\x01\x08\x01\x09\x01\x0a\x01\x0b\x01\x0c\x01\x0d\x01"+
        "\x0e\x01\x0f\x01\x10\x01\x11\x01\x12\x01\x13\x01\x14\x01\x15\x01"+
        "\x16\x01\x17\x01\x18\x01\x19\x01\x1a\x01\x1b\x01\x1c\x01\x1d\x01"+
        "\x1e\x01\x1f\x01\x20\x01\x21\x01\x22\x01\x23\x01\x24\x01\x25\x01"+
        "\x26\x6c\uffff}>";
    static readonly string[] DFA20_transitionS = {
            "\x01\x06\x03\x09\x09\x07\x02\x09\x06\uffff\x01\x01\x01\uffff"+
            "\x01\x02\x01\uffff\x01\x03\x03\uffff\x01\x04\x0a\uffff\x01\x05"+
            "\x01\x08",
            "\x01\x11\x02\x14\x01\x0a\x09\x12\x02\x14\x06\uffff\x01\x0c"+
            "\x01\uffff\x01\x0d\x01\uffff\x01\x0e\x03\uffff\x01\x0f\x01\x0b"+
            "\x09\uffff\x01\x10\x01\x13",
            "\x01\x1c\x02\x1f\x01\x15\x09\x1d\x02\x1f\x06\uffff\x01\x17"+
            "\x01\uffff\x01\x18\x01\uffff\x01\x19\x03\uffff\x01\x1a\x01\x16"+
            "\x09\uffff\x01\x1b\x01\x1e",
            "\x01\x27\x02\x2a\x01\x20\x09\x28\x02\x2a\x06\uffff\x01\x22"+
            "\x01\uffff\x01\x23\x01\uffff\x01\x24\x03\uffff\x01\x25\x01\x21"+
            "\x09\uffff\x01\x26\x01\x29",
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
            get { return "275:1: dollarExpression : ( primaryExpression conditional -> ^( ASTDOLLAREXPRESSION primaryExpression conditional ) | primaryExpression );"; }
        }

    }


    protected internal int DFA20_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA20_4 = input.LA(1);

                   	 
                   	int index20_4 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_4);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA20_5 = input.LA(1);

                   	 
                   	int index20_5 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_5);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA20_6 = input.LA(1);

                   	 
                   	int index20_6 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_6);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA20_7 = input.LA(1);

                   	 
                   	int index20_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_7);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA20_8 = input.LA(1);

                   	 
                   	int index20_8 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_8);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA20_9 = input.LA(1);

                   	 
                   	int index20_9 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_9);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 6 : 
                   	int LA20_10 = input.LA(1);

                   	 
                   	int index20_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_10);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 7 : 
                   	int LA20_11 = input.LA(1);

                   	 
                   	int index20_11 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_11);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 8 : 
                   	int LA20_12 = input.LA(1);

                   	 
                   	int index20_12 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_12);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 9 : 
                   	int LA20_13 = input.LA(1);

                   	 
                   	int index20_13 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_13);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 10 : 
                   	int LA20_14 = input.LA(1);

                   	 
                   	int index20_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_14);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 11 : 
                   	int LA20_15 = input.LA(1);

                   	 
                   	int index20_15 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_15);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 12 : 
                   	int LA20_16 = input.LA(1);

                   	 
                   	int index20_16 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_16);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 13 : 
                   	int LA20_17 = input.LA(1);

                   	 
                   	int index20_17 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_17);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 14 : 
                   	int LA20_18 = input.LA(1);

                   	 
                   	int index20_18 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_18);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 15 : 
                   	int LA20_19 = input.LA(1);

                   	 
                   	int index20_19 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_19);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 16 : 
                   	int LA20_20 = input.LA(1);

                   	 
                   	int index20_20 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_20);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 17 : 
                   	int LA20_21 = input.LA(1);

                   	 
                   	int index20_21 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_21);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 18 : 
                   	int LA20_22 = input.LA(1);

                   	 
                   	int index20_22 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_22);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 19 : 
                   	int LA20_23 = input.LA(1);

                   	 
                   	int index20_23 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_23);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 20 : 
                   	int LA20_24 = input.LA(1);

                   	 
                   	int index20_24 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_24);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 21 : 
                   	int LA20_25 = input.LA(1);

                   	 
                   	int index20_25 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_25);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 22 : 
                   	int LA20_26 = input.LA(1);

                   	 
                   	int index20_26 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_26);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 23 : 
                   	int LA20_27 = input.LA(1);

                   	 
                   	int index20_27 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_27);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 24 : 
                   	int LA20_28 = input.LA(1);

                   	 
                   	int index20_28 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_28);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 25 : 
                   	int LA20_29 = input.LA(1);

                   	 
                   	int index20_29 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_29);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 26 : 
                   	int LA20_30 = input.LA(1);

                   	 
                   	int index20_30 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_30);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 27 : 
                   	int LA20_31 = input.LA(1);

                   	 
                   	int index20_31 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_31);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 28 : 
                   	int LA20_32 = input.LA(1);

                   	 
                   	int index20_32 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_32);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 29 : 
                   	int LA20_33 = input.LA(1);

                   	 
                   	int index20_33 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_33);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 30 : 
                   	int LA20_34 = input.LA(1);

                   	 
                   	int index20_34 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_34);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 31 : 
                   	int LA20_35 = input.LA(1);

                   	 
                   	int index20_35 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_35);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 32 : 
                   	int LA20_36 = input.LA(1);

                   	 
                   	int index20_36 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_36);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 33 : 
                   	int LA20_37 = input.LA(1);

                   	 
                   	int index20_37 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_37);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 34 : 
                   	int LA20_38 = input.LA(1);

                   	 
                   	int index20_38 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_38);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 35 : 
                   	int LA20_39 = input.LA(1);

                   	 
                   	int index20_39 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_39);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 36 : 
                   	int LA20_40 = input.LA(1);

                   	 
                   	int index20_40 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_40);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 37 : 
                   	int LA20_41 = input.LA(1);

                   	 
                   	int index20_41 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_41);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 38 : 
                   	int LA20_42 = input.LA(1);

                   	 
                   	int index20_42 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred41_GAMS()) ) { s = 58; }

                   	else if ( (true) ) { s = 59; }

                   	 
                   	input.Seek(index20_42);
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
        "\x0a\uffff";
    const string DFA21_eofS =
        "\x0a\uffff";
    const string DFA21_minS =
        "\x01\x44\x09\uffff";
    const string DFA21_maxS =
        "\x01\x6d\x09\uffff";
    const string DFA21_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x05\uffff";
    const string DFA21_specialS =
        "\x0a\uffff}>";
    static readonly string[] DFA21_transitionS = {
            "\x0f\x04\x06\uffff\x01\x01\x01\uffff\x01\x02\x01\uffff\x01"+
            "\x03\x03\uffff\x01\x04\x0a\uffff\x02\x04",
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
            get { return "280:1: primaryExpression : ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value );"; }
        }

    }

    const string DFA22_eotS =
        "\x2f\uffff";
    const string DFA22_eofS =
        "\x03\uffff\x02\x05\x2a\uffff";
    const string DFA22_minS =
        "\x01\x44\x02\uffff\x02\x45\x02\uffff\x03\x00\x10\uffff\x02\x00"+
        "\x01\uffff\x01\x00\x11\uffff";
    const string DFA22_maxS =
        "\x01\x6d\x02\uffff\x02\x6b\x02\uffff\x03\x00\x10\uffff\x02\x00"+
        "\x01\uffff\x01\x00\x11\uffff";
    const string DFA22_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x02\uffff\x01\x05\x27\uffff\x01\x03"+
        "\x01\x04";
    const string DFA22_specialS =
        "\x07\uffff\x01\x00\x01\x01\x01\x02\x10\uffff\x01\x03\x01\x04\x01"+
        "\uffff\x01\x05\x11\uffff}>";
    static readonly string[] DFA22_transitionS = {
            "\x01\x03\x03\x05\x09\x04\x02\x05\x0e\uffff\x01\x01\x0a\uffff"+
            "\x01\x02\x01\x05",
            "",
            "",
            "\x02\x05\x0d\uffff\x05\x05\x01\x07\x01\x05\x01\x08\x01\x05"+
            "\x01\x09\x01\x05\x01\uffff\x01\x05\x01\uffff\x0a\x05",
            "\x02\x05\x0d\uffff\x05\x05\x01\x1d\x01\x05\x01\x1b\x01\x05"+
            "\x01\x1a\x01\x05\x01\uffff\x01\x05\x01\uffff\x0a\x05",
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
            get { return "288:1: value : ( Integer -> ^( ASTVALUE Integer ) | Double -> ^( ASTVALUE Double ) | sum -> ^( ASTVALUE sum ) | function -> ^( ASTVALUE function ) | variableWithIndexerEtc -> ^( ASTVALUE variableWithIndexerEtc ) );"; }
        }

    }


    protected internal int DFA22_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA22_7 = input.LA(1);

                   	 
                   	int index22_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred52_GAMS()) ) { s = 45; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index22_7);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA22_8 = input.LA(1);

                   	 
                   	int index22_8 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred52_GAMS()) ) { s = 45; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index22_8);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA22_9 = input.LA(1);

                   	 
                   	int index22_9 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred52_GAMS()) ) { s = 45; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index22_9);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA22_26 = input.LA(1);

                   	 
                   	int index22_26 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred53_GAMS()) ) { s = 46; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index22_26);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA22_27 = input.LA(1);

                   	 
                   	int index22_27 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred53_GAMS()) ) { s = 46; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index22_27);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA22_29 = input.LA(1);

                   	 
                   	int index22_29 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred53_GAMS()) ) { s = 46; }

                   	else if ( (true) ) { s = 5; }

                   	 
                   	input.Seek(index22_29);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae22 =
            new NoViableAltException(dfa.Description, 22, _s, input);
        dfa.Error(nvae22);
        throw nvae22;
    }
 

    public static readonly BitSet FOLLOW_set_in_extraTokens0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr_in_gams481 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020000007FFF0UL});
    public static readonly BitSet FOLLOW_EOF_in_gams484 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_equ_in_expr496 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_vardef_in_expr506 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variables_in_expr513 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_equations_in_expr520 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithIndexerSimple_in_equ534 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000080000UL});
    public static readonly BitSet FOLLOW_DOUBLEDOT_in_equ536 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_expression_in_equ538 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000100000UL});
    public static readonly BitSet FOLLOW_EEQUAL_in_equ540 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_expression_in_equ542 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000200000UL});
    public static readonly BitSet FOLLOW_SEMI_in_equ544 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithIndexerEtc_in_vardef592 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_EQUAL_in_vardef594 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_expression_in_vardef596 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000200000UL});
    public static readonly BitSet FOLLOW_SEMI_in_vardef598 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_VARIABLES_in_variables634 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020000007FFF0UL});
    public static readonly BitSet FOLLOW_variableWithIndexerSimple_in_variables636 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000A00000UL});
    public static readonly BitSet FOLLOW_COMMA_in_variables639 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020000007FFF0UL});
    public static readonly BitSet FOLLOW_variableWithIndexerSimple_in_variables641 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000A00000UL});
    public static readonly BitSet FOLLOW_SEMI_in_variables645 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_VARIABLES_in_equations657 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020000007FFF0UL});
    public static readonly BitSet FOLLOW_variableWithIndexerSimple_in_equations659 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000A00000UL});
    public static readonly BitSet FOLLOW_COMMA_in_equations662 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020000007FFF0UL});
    public static readonly BitSet FOLLOW_variableWithIndexerSimple_in_equations664 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000A00000UL});
    public static readonly BitSet FOLLOW_SEMI_in_equations668 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableWithIndexerSimple685 = new BitSet(new ulong[]{0x0000000000000002UL,0x000000002A000000UL});
    public static readonly BitSet FOLLOW_idx_in_variableWithIndexerSimple687 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableWithIndexerEtc706 = new BitSet(new ulong[]{0x0000000000000002UL,0x000000082B000000UL});
    public static readonly BitSet FOLLOW_DOT_in_variableWithIndexerEtc709 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020000007FFF0UL});
    public static readonly BitSet FOLLOW_variable_in_variableWithIndexerEtc711 = new BitSet(new ulong[]{0x0000000000000002UL,0x000000082A000000UL});
    public static readonly BitSet FOLLOW_idx_in_variableWithIndexerEtc715 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000800000000UL});
    public static readonly BitSet FOLLOW_conditional_in_variableWithIndexerEtc718 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_variable769 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_idx776 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020008007FFF0UL});
    public static readonly BitSet FOLLOW_indexerElements_in_idx778 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_R1_in_idx780 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_idx803 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020008007FFF0UL});
    public static readonly BitSet FOLLOW_indexerElements_in_idx805 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000010000000UL});
    public static readonly BitSet FOLLOW_R2_in_idx807 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_idx830 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020008007FFF0UL});
    public static readonly BitSet FOLLOW_indexerElements_in_idx832 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000040000000UL});
    public static readonly BitSet FOLLOW_R3_in_idx834 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableLagLead_in_indexerElements858 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COMMA_in_indexerElements861 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020008007FFF0UL});
    public static readonly BitSet FOLLOW_variableLagLead_in_indexerElements863 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000800000UL});
    public static readonly BitSet FOLLOW_StringInQuotes_in_variableLagLead895 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead944 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000100000000UL});
    public static readonly BitSet FOLLOW_PLUS_in_variableLagLead946 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000200000000UL});
    public static readonly BitSet FOLLOW_Integer_in_variableLagLead948 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead994 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000400000000UL});
    public static readonly BitSet FOLLOW_MINUS_in_variableLagLead996 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000200000000UL});
    public static readonly BitSet FOLLOW_Integer_in_variableLagLead998 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead1034 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DOLLAR_in_conditional1078 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_expression_in_conditional1080 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_andExpression_in_expression1113 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000040UL});
    public static readonly BitSet FOLLOW_OR_in_expression1116 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_andExpression_in_expression1119 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000040UL});
    public static readonly BitSet FOLLOW_notExpression_in_andExpression1128 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_AND_in_andExpression1131 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_notExpression_in_andExpression1134 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000020UL});
    public static readonly BitSet FOLLOW_NOT_in_notExpression1145 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_logicalExpression_in_notExpression1147 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_logicalExpression_in_notExpression1167 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_logicalExpression1175 = new BitSet(new ulong[]{0x0000000000000002UL,0x00000F8000400000UL});
    public static readonly BitSet FOLLOW_logical_in_logicalExpression1178 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_logicalExpression1181 = new BitSet(new ulong[]{0x0000000000000002UL,0x00000F8000400000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression1190 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000500000000UL});
    public static readonly BitSet FOLLOW_set_in_additiveExpression1194 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression1201 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000500000000UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression1211 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000003000000000UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpression1215 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression1222 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000003000000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression1232 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000004000000000UL});
    public static readonly BitSet FOLLOW_STARS_in_powerExpression1236 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression1239 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000004000000000UL});
    public static readonly BitSet FOLLOW_MINUS_in_unaryExpression1250 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_dollarExpression_in_unaryExpression1252 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_dollarExpression_in_unaryExpression1273 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_dollarExpression1298 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000800000000UL});
    public static readonly BitSet FOLLOW_conditional_in_dollarExpression1300 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_dollarExpression1320 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_primaryExpression1341 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression1343 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_R1_in_primaryExpression1345 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_primaryExpression1360 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression1362 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000010000000UL});
    public static readonly BitSet FOLLOW_R2_in_primaryExpression1364 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_primaryExpression1381 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression1383 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000040000000UL});
    public static readonly BitSet FOLLOW_R3_in_primaryExpression1385 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_value_in_primaryExpression1400 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_logical0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Integer_in_value1441 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Double_in_value1472 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_sum_in_value1506 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_value1543 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithIndexerEtc_in_value1591 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_function1623 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_L1_in_function1625 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_functionElements_in_function1627 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_R1_in_function1629 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_function1671 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_L2_in_function1673 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_functionElements_in_function1675 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000010000000UL});
    public static readonly BitSet FOLLOW_R2_in_function1677 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_function1719 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000020000000UL});
    public static readonly BitSet FOLLOW_L3_in_function1721 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_functionElements_in_function1723 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000040000000UL});
    public static readonly BitSet FOLLOW_R3_in_function1725 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_functionName0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_functionElements1800 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COMMA_in_functionElements1803 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_expression_in_functionElements1805 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000800000UL});
    public static readonly BitSet FOLLOW_SUM_in_sum1838 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_L1_in_sum1840 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020002A07FFF0UL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum1842 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000800800000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum1844 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum1847 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_expression_in_sum1849 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_R1_in_sum1851 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUM_in_sum1895 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_L2_in_sum1897 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020002A07FFF0UL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum1899 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000800800000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum1901 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum1904 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_expression_in_sum1906 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000010000000UL});
    public static readonly BitSet FOLLOW_R2_in_sum1908 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUM_in_sum1952 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000020000000UL});
    public static readonly BitSet FOLLOW_L3_in_sum1954 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020002A07FFF0UL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum1956 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000800800000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum1958 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum1961 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_expression_in_sum1963 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000040000000UL});
    public static readonly BitSet FOLLOW_R3_in_sum1965 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_sumControlled2020 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_sumControlled2047 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020008007FFF0UL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled2049 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_R1_in_sumControlled2051 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_sumControlled2077 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020008007FFF0UL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled2079 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000010000000UL});
    public static readonly BitSet FOLLOW_R2_in_sumControlled2081 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_sumControlled2107 = new BitSet(new ulong[]{0x0000000000000000UL,0x000020008007FFF0UL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled2109 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000040000000UL});
    public static readonly BitSet FOLLOW_R3_in_sumControlled2111 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Ident_in_ident2147 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_extraTokens_in_ident2151 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_equ_in_synpred16_GAMS496 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_vardef_in_synpred17_GAMS506 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variables_in_synpred18_GAMS513 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_conditional_in_synpred24_GAMS718 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_OR_in_synpred31_GAMS1116 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_andExpression_in_synpred31_GAMS1119 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_AND_in_synpred32_GAMS1131 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_notExpression_in_synpred32_GAMS1134 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NOT_in_synpred33_GAMS1145 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_logicalExpression_in_synpred33_GAMS1147 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_logical_in_synpred34_GAMS1178 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_synpred34_GAMS1181 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_synpred36_GAMS1194 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_synpred36_GAMS1201 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_synpred38_GAMS1215 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_powerExpression_in_synpred38_GAMS1222 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STARS_in_synpred39_GAMS1236 = new BitSet(new ulong[]{0x0000000000000000UL,0x000030062A07FFF0UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_synpred39_GAMS1239 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_synpred41_GAMS1298 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000800000000UL});
    public static readonly BitSet FOLLOW_conditional_in_synpred41_GAMS1300 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_sum_in_synpred52_GAMS1506 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_synpred53_GAMS1543 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}