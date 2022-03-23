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
		"LOG", 
		"EXP", 
		"PLUS", 
		"MINUS", 
		"MULT", 
		"DIV", 
		"LB", 
		"RB", 
		"RP", 
		"DOT", 
		"TRUE", 
		"FALSE", 
		"NEGATE", 
		"ASTFRML", 
		"ASTLEFTSIDE", 
		"ASTFRMLCODE", 
		"ASTEXPRESSION", 
		"ASTSIMPLEFUNCTION", 
		"ASTFUNCTION", 
		"ASTINTEGER", 
		"ASTDOUBLE", 
		"ASTPOW", 
		"ASTVARIABLE", 
		"ASTEND", 
		"FRML", 
		"SEMI", 
		"Double", 
		"Integer", 
		"MOD", 
		"Ident", 
		"STARS", 
		"Doubledot", 
		"DIGIT", 
		"Exponent", 
		"LETTER", 
		"WHITESPACE", 
		"NEWLINE", 
		"NEWLINE2", 
		"NEWLINE3", 
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
		"Z", 
		"'='", 
		"'('", 
		"','"
    };

    public const int ASTPOW = 25;
    public const int RB = 11;
    public const int ASTSIMPLEFUNCTION = 21;
    public const int ASTVARIABLE = 26;
    public const int RP = 12;
    public const int MOD = 32;
    public const int LETTER = 38;
    public const int LOG = 4;
    public const int ASTFUNCTION = 22;
    public const int Exponent = 37;
    public const int EOF = -1;
    public const int ASTINTEGER = 23;
    public const int ASTFRMLCODE = 19;
    public const int Comment = 43;
    public const int EXP = 5;
    public const int PLUS = 6;
    public const int ASTEND = 27;
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
    public const int W = 67;
    public const int NEGATE = 16;
    public const int WHITESPACE = 39;
    public const int V = 66;
    public const int Q = 61;
    public const int P = 60;
    public const int S = 63;
    public const int ASTFRML = 17;
    public const int R = 62;
    public const int MULT = 8;
    public const int MINUS = 7;
    public const int TRUE = 14;
    public const int SEMI = 29;
    public const int Y = 69;
    public const int X = 68;
    public const int Z = 70;
    public const int T__71 = 71;
    public const int T__72 = 72;
    public const int ASTLEFTSIDE = 18;
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

    // delegates
    // delegators



        public GAMSParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public GAMSParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();
            this.state.ruleMemo = new Hashtable[41+1];
             
             
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
        private System.Collections.Generic.List<string> frmlItems = new System.Collections.Generic.List<string>();
        public override void DisplayRecognitionError(string[] tokenNames,
                                            RecognitionException e) {
            string hdr = GetErrorHeader(e);
            string msg = GetErrorMessage(e, tokenNames);
            errors.Add(e.Line + "¤" + e.CharPositionInLine + "¤" + hdr + "¤" + msg);
        }
        public  System.Collections.Generic.List<string> GetErrors() {
            return errors;
        }
        public System.Collections.Generic.List<string> GetFrmlItems() {
          return frmlItems;
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
    // GAMS.g:111:1: extraTokens : ( LOG | EXP );
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
            // GAMS.g:111:13: ( LOG | EXP )
            // GAMS.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set1 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= LOG && input.LA(1) <= EXP) ) 
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
    // GAMS.g:120:1: expr : ( expr2 )+ EOF ;
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
            // GAMS.g:120:6: ( ( expr2 )+ EOF )
            // GAMS.g:120:8: ( expr2 )+ EOF
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// GAMS.g:120:8: ( expr2 )+
            	int cnt1 = 0;
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( (LA1_0 == FRML) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // GAMS.g:120:9: expr2
            			    {
            			    	PushFollow(FOLLOW_expr2_in_expr310);
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

            	EOF3=(IToken)Match(input,EOF,FOLLOW_EOF_in_expr314); if (state.failed) return retval;
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
    // GAMS.g:122:1: expr2 : frml ;
    public GAMSParser.expr2_return expr2() // throws RecognitionException [1]
    {   
        GAMSParser.expr2_return retval = new GAMSParser.expr2_return();
        retval.Start = input.LT(1);
        int expr2_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.frml_return frml4 = default(GAMSParser.frml_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 3) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:122:10: ( frml )
            // GAMS.g:123:3: frml
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_frml_in_expr2334);
            	frml4 = frml();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, frml4.Tree);

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

    public class frml_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "frml"
    // GAMS.g:126:1: frml : FRML code genrLeftSide '=' expression2 SEMI -> ^( ASTFRML code genrLeftSide expression2 ASTEND ) ;
    public GAMSParser.frml_return frml() // throws RecognitionException [1]
    {   
        GAMSParser.frml_return retval = new GAMSParser.frml_return();
        retval.Start = input.LT(1);
        int frml_StartIndex = input.Index();
        object root_0 = null;

        IToken FRML5 = null;
        IToken char_literal8 = null;
        IToken SEMI10 = null;
        GAMSParser.code_return code6 = default(GAMSParser.code_return);

        GAMSParser.genrLeftSide_return genrLeftSide7 = default(GAMSParser.genrLeftSide_return);

        GAMSParser.expression2_return expression29 = default(GAMSParser.expression2_return);


        object FRML5_tree=null;
        object char_literal8_tree=null;
        object SEMI10_tree=null;
        RewriteRuleTokenStream stream_FRML = new RewriteRuleTokenStream(adaptor,"token FRML");
        RewriteRuleTokenStream stream_SEMI = new RewriteRuleTokenStream(adaptor,"token SEMI");
        RewriteRuleTokenStream stream_71 = new RewriteRuleTokenStream(adaptor,"token 71");
        RewriteRuleSubtreeStream stream_genrLeftSide = new RewriteRuleSubtreeStream(adaptor,"rule genrLeftSide");
        RewriteRuleSubtreeStream stream_expression2 = new RewriteRuleSubtreeStream(adaptor,"rule expression2");
        RewriteRuleSubtreeStream stream_code = new RewriteRuleSubtreeStream(adaptor,"rule code");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 4) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:126:9: ( FRML code genrLeftSide '=' expression2 SEMI -> ^( ASTFRML code genrLeftSide expression2 ASTEND ) )
            // GAMS.g:126:11: FRML code genrLeftSide '=' expression2 SEMI
            {
            	FRML5=(IToken)Match(input,FRML,FOLLOW_FRML_in_frml351); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_FRML.Add(FRML5);

            	PushFollow(FOLLOW_code_in_frml353);
            	code6 = code();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_code.Add(code6.Tree);
            	PushFollow(FOLLOW_genrLeftSide_in_frml355);
            	genrLeftSide7 = genrLeftSide();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_genrLeftSide.Add(genrLeftSide7.Tree);
            	char_literal8=(IToken)Match(input,71,FOLLOW_71_in_frml357); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_71.Add(char_literal8);

            	PushFollow(FOLLOW_expression2_in_frml359);
            	expression29 = expression2();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression2.Add(expression29.Tree);
            	SEMI10=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_frml361); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI10);

            	if ( (state.backtracking==0) )
            	{
            	  frmlItems.Add(input.ToString((IToken)retval.Start,input.LT(-1)));
            	}


            	// AST REWRITE
            	// elements:          genrLeftSide, code, expression2
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 129:3: -> ^( ASTFRML code genrLeftSide expression2 ASTEND )
            	{
            	    // GAMS.g:129:6: ^( ASTFRML code genrLeftSide expression2 ASTEND )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFRML, "ASTFRML"), root_1);

            	    adaptor.AddChild(root_1, stream_code.NextTree());
            	    adaptor.AddChild(root_1, stream_genrLeftSide.NextTree());
            	    adaptor.AddChild(root_1, stream_expression2.NextTree());
            	    adaptor.AddChild(root_1, (object)adaptor.Create(ASTEND, "ASTEND"));

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
            	Memoize(input, 4, frml_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "frml"

    public class genrLeftSide_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "genrLeftSide"
    // GAMS.g:132:1: genrLeftSide : genrLeftSide2 -> ^( ASTLEFTSIDE genrLeftSide2 ) ;
    public GAMSParser.genrLeftSide_return genrLeftSide() // throws RecognitionException [1]
    {   
        GAMSParser.genrLeftSide_return retval = new GAMSParser.genrLeftSide_return();
        retval.Start = input.LT(1);
        int genrLeftSide_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.genrLeftSide2_return genrLeftSide211 = default(GAMSParser.genrLeftSide2_return);


        RewriteRuleSubtreeStream stream_genrLeftSide2 = new RewriteRuleSubtreeStream(adaptor,"rule genrLeftSide2");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 5) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:132:14: ( genrLeftSide2 -> ^( ASTLEFTSIDE genrLeftSide2 ) )
            // GAMS.g:132:16: genrLeftSide2
            {
            	PushFollow(FOLLOW_genrLeftSide2_in_genrLeftSide399);
            	genrLeftSide211 = genrLeftSide2();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_genrLeftSide2.Add(genrLeftSide211.Tree);


            	// AST REWRITE
            	// elements:          genrLeftSide2
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 132:30: -> ^( ASTLEFTSIDE genrLeftSide2 )
            	{
            	    // GAMS.g:132:33: ^( ASTLEFTSIDE genrLeftSide2 )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTLEFTSIDE, "ASTLEFTSIDE"), root_1);

            	    adaptor.AddChild(root_1, stream_genrLeftSide2.NextTree());

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
            	Memoize(input, 5, genrLeftSide_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "genrLeftSide"

    public class genrLeftSide2_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "genrLeftSide2"
    // GAMS.g:133:1: genrLeftSide2 : ( ident | simpleFunction ) ;
    public GAMSParser.genrLeftSide2_return genrLeftSide2() // throws RecognitionException [1]
    {   
        GAMSParser.genrLeftSide2_return retval = new GAMSParser.genrLeftSide2_return();
        retval.Start = input.LT(1);
        int genrLeftSide2_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.ident_return ident12 = default(GAMSParser.ident_return);

        GAMSParser.simpleFunction_return simpleFunction13 = default(GAMSParser.simpleFunction_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 6) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:133:15: ( ( ident | simpleFunction ) )
            // GAMS.g:133:17: ( ident | simpleFunction )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// GAMS.g:133:17: ( ident | simpleFunction )
            	int alt2 = 2;
            	int LA2_0 = input.LA(1);

            	if ( (LA2_0 == Ident) )
            	{
            	    int LA2_1 = input.LA(2);

            	    if ( (LA2_1 == 72) )
            	    {
            	        alt2 = 2;
            	    }
            	    else if ( (LA2_1 == 71) )
            	    {
            	        alt2 = 1;
            	    }
            	    else 
            	    {
            	        if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	        NoViableAltException nvae_d2s1 =
            	            new NoViableAltException("", 2, 1, input);

            	        throw nvae_d2s1;
            	    }
            	}
            	else if ( ((LA2_0 >= LOG && LA2_0 <= EXP)) )
            	{
            	    int LA2_2 = input.LA(2);

            	    if ( (LA2_2 == 72) )
            	    {
            	        alt2 = 2;
            	    }
            	    else if ( (LA2_2 == 71) )
            	    {
            	        alt2 = 1;
            	    }
            	    else 
            	    {
            	        if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	        NoViableAltException nvae_d2s2 =
            	            new NoViableAltException("", 2, 2, input);

            	        throw nvae_d2s2;
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
            	        // GAMS.g:133:18: ident
            	        {
            	        	PushFollow(FOLLOW_ident_in_genrLeftSide2415);
            	        	ident12 = ident();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, ident12.Tree);

            	        }
            	        break;
            	    case 2 :
            	        // GAMS.g:133:24: simpleFunction
            	        {
            	        	PushFollow(FOLLOW_simpleFunction_in_genrLeftSide2417);
            	        	simpleFunction13 = simpleFunction();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, simpleFunction13.Tree);

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
            	Memoize(input, 6, genrLeftSide2_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "genrLeftSide2"

    public class code_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "code"
    // GAMS.g:134:1: code : ident -> ^( ASTFRMLCODE ident ) ;
    public GAMSParser.code_return code() // throws RecognitionException [1]
    {   
        GAMSParser.code_return retval = new GAMSParser.code_return();
        retval.Start = input.LT(1);
        int code_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.ident_return ident14 = default(GAMSParser.ident_return);


        RewriteRuleSubtreeStream stream_ident = new RewriteRuleSubtreeStream(adaptor,"rule ident");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 7) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:134:6: ( ident -> ^( ASTFRMLCODE ident ) )
            // GAMS.g:134:8: ident
            {
            	PushFollow(FOLLOW_ident_in_code425);
            	ident14 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_ident.Add(ident14.Tree);


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
            	// 134:14: -> ^( ASTFRMLCODE ident )
            	{
            	    // GAMS.g:134:17: ^( ASTFRMLCODE ident )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFRMLCODE, "ASTFRMLCODE"), root_1);

            	    adaptor.AddChild(root_1, stream_ident.NextTree());

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
            	Memoize(input, 7, code_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "code"

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
    // GAMS.g:135:1: expression2 : expression -> ^( ASTEXPRESSION expression ) ;
    public GAMSParser.expression2_return expression2() // throws RecognitionException [1]
    {   
        GAMSParser.expression2_return retval = new GAMSParser.expression2_return();
        retval.Start = input.LT(1);
        int expression2_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.expression_return expression15 = default(GAMSParser.expression_return);


        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 8) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:135:13: ( expression -> ^( ASTEXPRESSION expression ) )
            // GAMS.g:135:15: expression
            {
            	PushFollow(FOLLOW_expression_in_expression2440);
            	expression15 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression15.Tree);


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
            	// 135:26: -> ^( ASTEXPRESSION expression )
            	{
            	    // GAMS.g:135:29: ^( ASTEXPRESSION expression )
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
            	Memoize(input, 8, expression2_StartIndex); 
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
    // GAMS.g:136:1: number : ( Double | Integer ) ;
    public GAMSParser.number_return number() // throws RecognitionException [1]
    {   
        GAMSParser.number_return retval = new GAMSParser.number_return();
        retval.Start = input.LT(1);
        int number_StartIndex = input.Index();
        object root_0 = null;

        IToken set16 = null;

        object set16_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 9) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:136:7: ( ( Double | Integer ) )
            // GAMS.g:136:9: ( Double | Integer )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set16 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= Double && input.LA(1) <= Integer) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set16));
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
            	Memoize(input, 9, number_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "number"

    public class simpleFunction_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "simpleFunction"
    // GAMS.g:142:1: simpleFunction : ident '(' ident ')' -> ^( ASTSIMPLEFUNCTION ident ident ) ;
    public GAMSParser.simpleFunction_return simpleFunction() // throws RecognitionException [1]
    {   
        GAMSParser.simpleFunction_return retval = new GAMSParser.simpleFunction_return();
        retval.Start = input.LT(1);
        int simpleFunction_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal18 = null;
        IToken char_literal20 = null;
        GAMSParser.ident_return ident17 = default(GAMSParser.ident_return);

        GAMSParser.ident_return ident19 = default(GAMSParser.ident_return);


        object char_literal18_tree=null;
        object char_literal20_tree=null;
        RewriteRuleTokenStream stream_RP = new RewriteRuleTokenStream(adaptor,"token RP");
        RewriteRuleTokenStream stream_72 = new RewriteRuleTokenStream(adaptor,"token 72");
        RewriteRuleSubtreeStream stream_ident = new RewriteRuleSubtreeStream(adaptor,"rule ident");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 10) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:143:2: ( ident '(' ident ')' -> ^( ASTSIMPLEFUNCTION ident ident ) )
            // GAMS.g:143:4: ident '(' ident ')'
            {
            	PushFollow(FOLLOW_ident_in_simpleFunction472);
            	ident17 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_ident.Add(ident17.Tree);
            	char_literal18=(IToken)Match(input,72,FOLLOW_72_in_simpleFunction474); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_72.Add(char_literal18);

            	PushFollow(FOLLOW_ident_in_simpleFunction476);
            	ident19 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_ident.Add(ident19.Tree);
            	char_literal20=(IToken)Match(input,RP,FOLLOW_RP_in_simpleFunction478); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_RP.Add(char_literal20);



            	// AST REWRITE
            	// elements:          ident, ident
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 143:24: -> ^( ASTSIMPLEFUNCTION ident ident )
            	{
            	    // GAMS.g:143:27: ^( ASTSIMPLEFUNCTION ident ident )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTSIMPLEFUNCTION, "ASTSIMPLEFUNCTION"), root_1);

            	    adaptor.AddChild(root_1, stream_ident.NextTree());
            	    adaptor.AddChild(root_1, stream_ident.NextTree());

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
            	Memoize(input, 10, simpleFunction_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "simpleFunction"

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
    // GAMS.g:146:1: function : ident '(' ( expression ( ',' expression )* )? ')' -> ^( ASTFUNCTION ident expression ( expression )* ) ;
    public GAMSParser.function_return function() // throws RecognitionException [1]
    {   
        GAMSParser.function_return retval = new GAMSParser.function_return();
        retval.Start = input.LT(1);
        int function_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal22 = null;
        IToken char_literal24 = null;
        IToken char_literal26 = null;
        GAMSParser.ident_return ident21 = default(GAMSParser.ident_return);

        GAMSParser.expression_return expression23 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression25 = default(GAMSParser.expression_return);


        object char_literal22_tree=null;
        object char_literal24_tree=null;
        object char_literal26_tree=null;
        RewriteRuleTokenStream stream_RP = new RewriteRuleTokenStream(adaptor,"token RP");
        RewriteRuleTokenStream stream_72 = new RewriteRuleTokenStream(adaptor,"token 72");
        RewriteRuleTokenStream stream_73 = new RewriteRuleTokenStream(adaptor,"token 73");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_ident = new RewriteRuleSubtreeStream(adaptor,"rule ident");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 11) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:146:10: ( ident '(' ( expression ( ',' expression )* )? ')' -> ^( ASTFUNCTION ident expression ( expression )* ) )
            // GAMS.g:146:12: ident '(' ( expression ( ',' expression )* )? ')'
            {
            	PushFollow(FOLLOW_ident_in_function499);
            	ident21 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_ident.Add(ident21.Tree);
            	char_literal22=(IToken)Match(input,72,FOLLOW_72_in_function501); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_72.Add(char_literal22);

            	// GAMS.g:146:22: ( expression ( ',' expression )* )?
            	int alt4 = 2;
            	int LA4_0 = input.LA(1);

            	if ( ((LA4_0 >= LOG && LA4_0 <= EXP) || LA4_0 == MINUS || (LA4_0 >= Double && LA4_0 <= Integer) || LA4_0 == Ident || LA4_0 == 72) )
            	{
            	    alt4 = 1;
            	}
            	switch (alt4) 
            	{
            	    case 1 :
            	        // GAMS.g:146:24: expression ( ',' expression )*
            	        {
            	        	PushFollow(FOLLOW_expression_in_function505);
            	        	expression23 = expression();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_expression.Add(expression23.Tree);
            	        	// GAMS.g:146:35: ( ',' expression )*
            	        	do 
            	        	{
            	        	    int alt3 = 2;
            	        	    int LA3_0 = input.LA(1);

            	        	    if ( (LA3_0 == 73) )
            	        	    {
            	        	        alt3 = 1;
            	        	    }


            	        	    switch (alt3) 
            	        		{
            	        			case 1 :
            	        			    // GAMS.g:146:36: ',' expression
            	        			    {
            	        			    	char_literal24=(IToken)Match(input,73,FOLLOW_73_in_function508); if (state.failed) return retval; 
            	        			    	if ( (state.backtracking==0) ) stream_73.Add(char_literal24);

            	        			    	PushFollow(FOLLOW_expression_in_function510);
            	        			    	expression25 = expression();
            	        			    	state.followingStackPointer--;
            	        			    	if (state.failed) return retval;
            	        			    	if ( (state.backtracking==0) ) stream_expression.Add(expression25.Tree);

            	        			    }
            	        			    break;

            	        			default:
            	        			    goto loop3;
            	        	    }
            	        	} while (true);

            	        	loop3:
            	        		;	// Stops C# compiler whining that label 'loop3' has no statements


            	        }
            	        break;

            	}

            	char_literal26=(IToken)Match(input,RP,FOLLOW_RP_in_function517); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_RP.Add(char_literal26);



            	// AST REWRITE
            	// elements:          ident, expression, expression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 146:60: -> ^( ASTFUNCTION ident expression ( expression )* )
            	{
            	    // GAMS.g:146:63: ^( ASTFUNCTION ident expression ( expression )* )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION, "ASTFUNCTION"), root_1);

            	    adaptor.AddChild(root_1, stream_ident.NextTree());
            	    adaptor.AddChild(root_1, stream_expression.NextTree());
            	    // GAMS.g:146:94: ( expression )*
            	    while ( stream_expression.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_expression.NextTree());

            	    }
            	    stream_expression.Reset();

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
            	Memoize(input, 11, function_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "function"

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
    // GAMS.g:148:1: expression : additiveExpression ;
    public GAMSParser.expression_return expression() // throws RecognitionException [1]
    {   
        GAMSParser.expression_return retval = new GAMSParser.expression_return();
        retval.Start = input.LT(1);
        int expression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.additiveExpression_return additiveExpression27 = default(GAMSParser.additiveExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 12) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:148:12: ( additiveExpression )
            // GAMS.g:148:16: additiveExpression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_additiveExpression_in_expression541);
            	additiveExpression27 = additiveExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression27.Tree);

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
            	Memoize(input, 12, expression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "expression"

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
    // GAMS.g:150:1: additiveExpression : multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* ;
    public GAMSParser.additiveExpression_return additiveExpression() // throws RecognitionException [1]
    {   
        GAMSParser.additiveExpression_return retval = new GAMSParser.additiveExpression_return();
        retval.Start = input.LT(1);
        int additiveExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set29 = null;
        GAMSParser.multiplicativeExpression_return multiplicativeExpression28 = default(GAMSParser.multiplicativeExpression_return);

        GAMSParser.multiplicativeExpression_return multiplicativeExpression30 = default(GAMSParser.multiplicativeExpression_return);


        object set29_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 13) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:150:21: ( multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* )
            // GAMS.g:150:23: multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression550);
            	multiplicativeExpression28 = multiplicativeExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression28.Tree);
            	// GAMS.g:150:48: ( ( PLUS | MINUS ) multiplicativeExpression )*
            	do 
            	{
            	    int alt5 = 2;
            	    int LA5_0 = input.LA(1);

            	    if ( ((LA5_0 >= PLUS && LA5_0 <= MINUS)) )
            	    {
            	        alt5 = 1;
            	    }


            	    switch (alt5) 
            		{
            			case 1 :
            			    // GAMS.g:150:50: ( PLUS | MINUS ) multiplicativeExpression
            			    {
            			    	set29=(IToken)input.LT(1);
            			    	set29 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= PLUS && input.LA(1) <= MINUS) ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set29), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression561);
            			    	multiplicativeExpression30 = multiplicativeExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression30.Tree);

            			    }
            			    break;

            			default:
            			    goto loop5;
            	    }
            	} while (true);

            	loop5:
            		;	// Stops C# compiler whining that label 'loop5' has no statements


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
            	Memoize(input, 13, additiveExpression_StartIndex); 
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
    // GAMS.g:152:1: multiplicativeExpression : powerExpression ( ( MULT | DIV | MOD ) powerExpression )* ;
    public GAMSParser.multiplicativeExpression_return multiplicativeExpression() // throws RecognitionException [1]
    {   
        GAMSParser.multiplicativeExpression_return retval = new GAMSParser.multiplicativeExpression_return();
        retval.Start = input.LT(1);
        int multiplicativeExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set32 = null;
        GAMSParser.powerExpression_return powerExpression31 = default(GAMSParser.powerExpression_return);

        GAMSParser.powerExpression_return powerExpression33 = default(GAMSParser.powerExpression_return);


        object set32_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 14) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:152:28: ( powerExpression ( ( MULT | DIV | MOD ) powerExpression )* )
            // GAMS.g:152:30: powerExpression ( ( MULT | DIV | MOD ) powerExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression574);
            	powerExpression31 = powerExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression31.Tree);
            	// GAMS.g:152:46: ( ( MULT | DIV | MOD ) powerExpression )*
            	do 
            	{
            	    int alt6 = 2;
            	    int LA6_0 = input.LA(1);

            	    if ( ((LA6_0 >= MULT && LA6_0 <= DIV) || LA6_0 == MOD) )
            	    {
            	        alt6 = 1;
            	    }


            	    switch (alt6) 
            		{
            			case 1 :
            			    // GAMS.g:152:48: ( MULT | DIV | MOD ) powerExpression
            			    {
            			    	set32=(IToken)input.LT(1);
            			    	set32 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= MULT && input.LA(1) <= DIV) || input.LA(1) == MOD ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set32), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression587);
            			    	powerExpression33 = powerExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression33.Tree);

            			    }
            			    break;

            			default:
            			    goto loop6;
            	    }
            	} while (true);

            	loop6:
            		;	// Stops C# compiler whining that label 'loop6' has no statements


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
            	Memoize(input, 14, multiplicativeExpression_StartIndex); 
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
    // GAMS.g:154:1: powerExpression : unaryExpression ( pow unaryExpression )* ;
    public GAMSParser.powerExpression_return powerExpression() // throws RecognitionException [1]
    {   
        GAMSParser.powerExpression_return retval = new GAMSParser.powerExpression_return();
        retval.Start = input.LT(1);
        int powerExpression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.unaryExpression_return unaryExpression34 = default(GAMSParser.unaryExpression_return);

        GAMSParser.pow_return pow35 = default(GAMSParser.pow_return);

        GAMSParser.unaryExpression_return unaryExpression36 = default(GAMSParser.unaryExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 15) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:154:19: ( unaryExpression ( pow unaryExpression )* )
            // GAMS.g:154:21: unaryExpression ( pow unaryExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_unaryExpression_in_powerExpression600);
            	unaryExpression34 = unaryExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression34.Tree);
            	// GAMS.g:154:37: ( pow unaryExpression )*
            	do 
            	{
            	    int alt7 = 2;
            	    int LA7_0 = input.LA(1);

            	    if ( (LA7_0 == STARS) )
            	    {
            	        alt7 = 1;
            	    }


            	    switch (alt7) 
            		{
            			case 1 :
            			    // GAMS.g:154:39: pow unaryExpression
            			    {
            			    	PushFollow(FOLLOW_pow_in_powerExpression604);
            			    	pow35 = pow();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot(pow35.Tree, root_0);
            			    	PushFollow(FOLLOW_unaryExpression_in_powerExpression607);
            			    	unaryExpression36 = unaryExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression36.Tree);

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
            	Memoize(input, 15, powerExpression_StartIndex); 
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
    // GAMS.g:156:1: unaryExpression : ( primaryExpression | MINUS primaryExpression -> ^( NEGATE primaryExpression ) );
    public GAMSParser.unaryExpression_return unaryExpression() // throws RecognitionException [1]
    {   
        GAMSParser.unaryExpression_return retval = new GAMSParser.unaryExpression_return();
        retval.Start = input.LT(1);
        int unaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken MINUS38 = null;
        GAMSParser.primaryExpression_return primaryExpression37 = default(GAMSParser.primaryExpression_return);

        GAMSParser.primaryExpression_return primaryExpression39 = default(GAMSParser.primaryExpression_return);


        object MINUS38_tree=null;
        RewriteRuleTokenStream stream_MINUS = new RewriteRuleTokenStream(adaptor,"token MINUS");
        RewriteRuleSubtreeStream stream_primaryExpression = new RewriteRuleSubtreeStream(adaptor,"rule primaryExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 16) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:156:19: ( primaryExpression | MINUS primaryExpression -> ^( NEGATE primaryExpression ) )
            int alt8 = 2;
            int LA8_0 = input.LA(1);

            if ( ((LA8_0 >= LOG && LA8_0 <= EXP) || (LA8_0 >= Double && LA8_0 <= Integer) || LA8_0 == Ident || LA8_0 == 72) )
            {
                alt8 = 1;
            }
            else if ( (LA8_0 == MINUS) )
            {
                alt8 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d8s0 =
                    new NoViableAltException("", 8, 0, input);

                throw nvae_d8s0;
            }
            switch (alt8) 
            {
                case 1 :
                    // GAMS.g:156:21: primaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_primaryExpression_in_unaryExpression621);
                    	primaryExpression37 = primaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, primaryExpression37.Tree);

                    }
                    break;
                case 2 :
                    // GAMS.g:157:10: MINUS primaryExpression
                    {
                    	MINUS38=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_unaryExpression632); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_MINUS.Add(MINUS38);

                    	PushFollow(FOLLOW_primaryExpression_in_unaryExpression634);
                    	primaryExpression39 = primaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_primaryExpression.Add(primaryExpression39.Tree);


                    	// AST REWRITE
                    	// elements:          primaryExpression
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 157:34: -> ^( NEGATE primaryExpression )
                    	{
                    	    // GAMS.g:157:37: ^( NEGATE primaryExpression )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(NEGATE, "NEGATE"), root_1);

                    	    adaptor.AddChild(root_1, stream_primaryExpression.NextTree());

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
            	Memoize(input, 16, unaryExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "unaryExpression"

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
    // GAMS.g:159:1: primaryExpression : ( '(' expression ')' | value );
    public GAMSParser.primaryExpression_return primaryExpression() // throws RecognitionException [1]
    {   
        GAMSParser.primaryExpression_return retval = new GAMSParser.primaryExpression_return();
        retval.Start = input.LT(1);
        int primaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal40 = null;
        IToken char_literal42 = null;
        GAMSParser.expression_return expression41 = default(GAMSParser.expression_return);

        GAMSParser.value_return value43 = default(GAMSParser.value_return);


        object char_literal40_tree=null;
        object char_literal42_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 17) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:160:4: ( '(' expression ')' | value )
            int alt9 = 2;
            int LA9_0 = input.LA(1);

            if ( (LA9_0 == 72) )
            {
                alt9 = 1;
            }
            else if ( ((LA9_0 >= LOG && LA9_0 <= EXP) || (LA9_0 >= Double && LA9_0 <= Integer) || LA9_0 == Ident) )
            {
                alt9 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d9s0 =
                    new NoViableAltException("", 9, 0, input);

                throw nvae_d9s0;
            }
            switch (alt9) 
            {
                case 1 :
                    // GAMS.g:160:6: '(' expression ')'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal40=(IToken)Match(input,72,FOLLOW_72_in_primaryExpression661); if (state.failed) return retval;
                    	PushFollow(FOLLOW_expression_in_primaryExpression664);
                    	expression41 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression41.Tree);
                    	char_literal42=(IToken)Match(input,RP,FOLLOW_RP_in_primaryExpression666); if (state.failed) return retval;

                    }
                    break;
                case 2 :
                    // GAMS.g:161:6: value
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_value_in_primaryExpression674);
                    	value43 = value();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, value43.Tree);

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
            	Memoize(input, 17, primaryExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "primaryExpression"

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
    // GAMS.g:163:1: value : ( Integer -> ^( ASTINTEGER Integer ) | Double -> ^( ASTDOUBLE Double ) | function | ident -> ^( ASTVARIABLE ident ) );
    public GAMSParser.value_return value() // throws RecognitionException [1]
    {   
        GAMSParser.value_return retval = new GAMSParser.value_return();
        retval.Start = input.LT(1);
        int value_StartIndex = input.Index();
        object root_0 = null;

        IToken Integer44 = null;
        IToken Double45 = null;
        GAMSParser.function_return function46 = default(GAMSParser.function_return);

        GAMSParser.ident_return ident47 = default(GAMSParser.ident_return);


        object Integer44_tree=null;
        object Double45_tree=null;
        RewriteRuleTokenStream stream_Double = new RewriteRuleTokenStream(adaptor,"token Double");
        RewriteRuleTokenStream stream_Integer = new RewriteRuleTokenStream(adaptor,"token Integer");
        RewriteRuleSubtreeStream stream_ident = new RewriteRuleSubtreeStream(adaptor,"rule ident");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 18) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:164:2: ( Integer -> ^( ASTINTEGER Integer ) | Double -> ^( ASTDOUBLE Double ) | function | ident -> ^( ASTVARIABLE ident ) )
            int alt10 = 4;
            alt10 = dfa10.Predict(input);
            switch (alt10) 
            {
                case 1 :
                    // GAMS.g:164:5: Integer
                    {
                    	Integer44=(IToken)Match(input,Integer,FOLLOW_Integer_in_value688); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer44);



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
                    	// 164:15: -> ^( ASTINTEGER Integer )
                    	{
                    	    // GAMS.g:164:18: ^( ASTINTEGER Integer )
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
                    // GAMS.g:165:4: Double
                    {
                    	Double45=(IToken)Match(input,Double,FOLLOW_Double_in_value703); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Double.Add(Double45);



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
                    	// 165:15: -> ^( ASTDOUBLE Double )
                    	{
                    	    // GAMS.g:165:18: ^( ASTDOUBLE Double )
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
                    // GAMS.g:166:4: function
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_function_in_value720);
                    	function46 = function();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, function46.Tree);

                    }
                    break;
                case 4 :
                    // GAMS.g:167:6: ident
                    {
                    	PushFollow(FOLLOW_ident_in_value729);
                    	ident47 = ident();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_ident.Add(ident47.Tree);


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
                    	// 167:17: -> ^( ASTVARIABLE ident )
                    	{
                    	    // GAMS.g:167:20: ^( ASTVARIABLE ident )
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
            	Memoize(input, 18, value_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "value"

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
    // GAMS.g:170:1: numberPlusMinus : ( MINUS | PLUS ) ( Integer | Double ) ;
    public GAMSParser.numberPlusMinus_return numberPlusMinus() // throws RecognitionException [1]
    {   
        GAMSParser.numberPlusMinus_return retval = new GAMSParser.numberPlusMinus_return();
        retval.Start = input.LT(1);
        int numberPlusMinus_StartIndex = input.Index();
        object root_0 = null;

        IToken set48 = null;
        IToken set49 = null;

        object set48_tree=null;
        object set49_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 19) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:170:18: ( ( MINUS | PLUS ) ( Integer | Double ) )
            // GAMS.g:170:21: ( MINUS | PLUS ) ( Integer | Double )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set48 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= PLUS && input.LA(1) <= MINUS) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set48));
            	    state.errorRecovery = false;state.failed = false;
            	}
            	else 
            	{
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}

            	set49 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= Double && input.LA(1) <= Integer) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set49));
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
            	Memoize(input, 19, numberPlusMinus_StartIndex); 
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
    // GAMS.g:172:1: ident : ( Ident | extraTokens );
    public GAMSParser.ident_return ident() // throws RecognitionException [1]
    {   
        GAMSParser.ident_return retval = new GAMSParser.ident_return();
        retval.Start = input.LT(1);
        int ident_StartIndex = input.Index();
        object root_0 = null;

        IToken Ident50 = null;
        GAMSParser.extraTokens_return extraTokens51 = default(GAMSParser.extraTokens_return);


        object Ident50_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 20) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:172:9: ( Ident | extraTokens )
            int alt11 = 2;
            int LA11_0 = input.LA(1);

            if ( (LA11_0 == Ident) )
            {
                alt11 = 1;
            }
            else if ( ((LA11_0 >= LOG && LA11_0 <= EXP)) )
            {
                alt11 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d11s0 =
                    new NoViableAltException("", 11, 0, input);

                throw nvae_d11s0;
            }
            switch (alt11) 
            {
                case 1 :
                    // GAMS.g:172:12: Ident
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Ident50=(IToken)Match(input,Ident,FOLLOW_Ident_in_ident779); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Ident50_tree = (object)adaptor.Create(Ident50);
                    		adaptor.AddChild(root_0, Ident50_tree);
                    	}

                    }
                    break;
                case 2 :
                    // GAMS.g:172:20: extraTokens
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_extraTokens_in_ident783);
                    	extraTokens51 = extraTokens();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, extraTokens51.Tree);

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
            	Memoize(input, 20, ident_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "ident"

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
    // GAMS.g:184:1: pow : STARS -> ASTPOW ;
    public GAMSParser.pow_return pow() // throws RecognitionException [1]
    {   
        GAMSParser.pow_return retval = new GAMSParser.pow_return();
        retval.Start = input.LT(1);
        int pow_StartIndex = input.Index();
        object root_0 = null;

        IToken STARS52 = null;

        object STARS52_tree=null;
        RewriteRuleTokenStream stream_STARS = new RewriteRuleTokenStream(adaptor,"token STARS");

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 21) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:184:9: ( STARS -> ASTPOW )
            // GAMS.g:184:17: STARS
            {
            	STARS52=(IToken)Match(input,STARS,FOLLOW_STARS_in_pow869); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_STARS.Add(STARS52);



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
            	// 184:24: -> ASTPOW
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
            	Memoize(input, 21, pow_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "pow"

    // Delegated rules


   	protected DFA10 dfa10;
	private void InitializeCyclicDFAs()
	{
    	this.dfa10 = new DFA10(this);
	}

    const string DFA10_eotS =
        "\x15\uffff";
    const string DFA10_eofS =
        "\x03\uffff\x02\x05\x10\uffff";
    const string DFA10_minS =
        "\x01\x04\x02\uffff\x02\x06\x10\uffff";
    const string DFA10_maxS =
        "\x01\x21\x02\uffff\x02\x49\x10\uffff";
    const string DFA10_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x02\uffff\x01\x04\x06\uffff\x01\x03"+
        "\x08\uffff";
    const string DFA10_specialS =
        "\x15\uffff}>";
    static readonly string[] DFA10_transitionS = {
            "\x02\x04\x18\uffff\x01\x02\x01\x01\x01\uffff\x01\x03",
            "",
            "",
            "\x04\x05\x02\uffff\x01\x05\x10\uffff\x01\x05\x02\uffff\x01"+
            "\x05\x01\uffff\x01\x05\x25\uffff\x01\x0c\x01\x05",
            "\x04\x05\x02\uffff\x01\x05\x10\uffff\x01\x05\x02\uffff\x01"+
            "\x05\x01\uffff\x01\x05\x25\uffff\x01\x0c\x01\x05",
            "",
            "",
            "",
            "",
            "",
            "",
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
            get { return "163:1: value : ( Integer -> ^( ASTINTEGER Integer ) | Double -> ^( ASTDOUBLE Double ) | function | ident -> ^( ASTVARIABLE ident ) );"; }
        }

    }

 

    public static readonly BitSet FOLLOW_set_in_extraTokens0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr2_in_expr310 = new BitSet(new ulong[]{0x0000000010000000UL});
    public static readonly BitSet FOLLOW_EOF_in_expr314 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_frml_in_expr2334 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FRML_in_frml351 = new BitSet(new ulong[]{0x0000000200000030UL});
    public static readonly BitSet FOLLOW_code_in_frml353 = new BitSet(new ulong[]{0x0000000200000030UL});
    public static readonly BitSet FOLLOW_genrLeftSide_in_frml355 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000000080UL});
    public static readonly BitSet FOLLOW_71_in_frml357 = new BitSet(new ulong[]{0x00000002C00000B0UL,0x0000000000000100UL});
    public static readonly BitSet FOLLOW_expression2_in_frml359 = new BitSet(new ulong[]{0x0000000020000000UL});
    public static readonly BitSet FOLLOW_SEMI_in_frml361 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_genrLeftSide2_in_genrLeftSide399 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_genrLeftSide2415 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_simpleFunction_in_genrLeftSide2417 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_code425 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_expression2440 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_number454 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_simpleFunction472 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000000100UL});
    public static readonly BitSet FOLLOW_72_in_simpleFunction474 = new BitSet(new ulong[]{0x0000000200000030UL});
    public static readonly BitSet FOLLOW_ident_in_simpleFunction476 = new BitSet(new ulong[]{0x0000000000001000UL});
    public static readonly BitSet FOLLOW_RP_in_simpleFunction478 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_function499 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000000100UL});
    public static readonly BitSet FOLLOW_72_in_function501 = new BitSet(new ulong[]{0x00000002C00010B0UL,0x0000000000000100UL});
    public static readonly BitSet FOLLOW_expression_in_function505 = new BitSet(new ulong[]{0x0000000000001000UL,0x0000000000000200UL});
    public static readonly BitSet FOLLOW_73_in_function508 = new BitSet(new ulong[]{0x00000002C00000B0UL,0x0000000000000100UL});
    public static readonly BitSet FOLLOW_expression_in_function510 = new BitSet(new ulong[]{0x0000000000001000UL,0x0000000000000200UL});
    public static readonly BitSet FOLLOW_RP_in_function517 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_expression541 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression550 = new BitSet(new ulong[]{0x00000000000000C2UL});
    public static readonly BitSet FOLLOW_set_in_additiveExpression554 = new BitSet(new ulong[]{0x00000002C00000B0UL,0x0000000000000100UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression561 = new BitSet(new ulong[]{0x00000000000000C2UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression574 = new BitSet(new ulong[]{0x0000000100000302UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpression578 = new BitSet(new ulong[]{0x00000002C00000B0UL,0x0000000000000100UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression587 = new BitSet(new ulong[]{0x0000000100000302UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression600 = new BitSet(new ulong[]{0x0000000400000002UL});
    public static readonly BitSet FOLLOW_pow_in_powerExpression604 = new BitSet(new ulong[]{0x00000002C00000B0UL,0x0000000000000100UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression607 = new BitSet(new ulong[]{0x0000000400000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_unaryExpression621 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MINUS_in_unaryExpression632 = new BitSet(new ulong[]{0x00000002C0000030UL,0x0000000000000100UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_unaryExpression634 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_72_in_primaryExpression661 = new BitSet(new ulong[]{0x00000002C00000B0UL,0x0000000000000100UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression664 = new BitSet(new ulong[]{0x0000000000001000UL});
    public static readonly BitSet FOLLOW_RP_in_primaryExpression666 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_value_in_primaryExpression674 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Integer_in_value688 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Double_in_value703 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_value720 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_value729 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_numberPlusMinus755 = new BitSet(new ulong[]{0x00000000C0000000UL});
    public static readonly BitSet FOLLOW_set_in_numberPlusMinus761 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Ident_in_ident779 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_extraTokens_in_ident783 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STARS_in_pow869 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}