// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 GAMS.g 2022-03-24 00:13:39

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
		"ASTFRML", 
		"ASTLEFTSIDE", 
		"ASTFRMLCODE", 
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
		"SUM", 
		"DOUBLEDOT", 
		"EQUAL", 
		"SEMI", 
		"L1", 
		"R1", 
		"L2", 
		"R2", 
		"L3", 
		"R3", 
		"COMMA", 
		"PLUS", 
		"Integer", 
		"MINUS", 
		"Double", 
		"DOLLAR", 
		"MULT", 
		"DIV", 
		"MOD", 
		"Ident", 
		"STARS", 
		"FRML", 
		"LB", 
		"RB", 
		"DOT", 
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
		"Z"
    };

    public const int DOLLAR = 42;
    public const int ASTPOW = 15;
    public const int RB = 50;
    public const int ASTVARIABLE = 16;
    public const int LETTER = 54;
    public const int MOD = 45;
    public const int ASTFUNCTION = 12;
    public const int ASTINDEXES1 = 18;
    public const int Exponent = 53;
    public const int ASTCONDITIONAL = 25;
    public const int DOUBLEDOT = 28;
    public const int ASTINDEXES3 = 20;
    public const int R2 = 34;
    public const int ASTINDEXES2 = 19;
    public const int R3 = 36;
    public const int SUM = 27;
    public const int EOF = -1;
    public const int ASTINTEGER = 13;
    public const int ASTFRMLCODE = 7;
    public const int Comment = 59;
    public const int COMMA = 37;
    public const int R1 = 32;
    public const int ASTSIMPLEFUNCTION1 = 9;
    public const int EQUAL = 29;
    public const int ASTSIMPLEFUNCTION2 = 10;
    public const int ASTSIMPLEFUNCTION3 = 11;
    public const int ASTEND = 17;
    public const int PLUS = 38;
    public const int FRML = 48;
    public const int DIGIT = 52;
    public const int DOT = 51;
    public const int D = 64;
    public const int ASTEXPRESSION2 = 22;
    public const int Double = 41;
    public const int E = 65;
    public const int ASTEXPRESSION3 = 23;
    public const int F = 66;
    public const int G = 67;
    public const int A = 61;
    public const int B = 62;
    public const int C = 63;
    public const int L = 72;
    public const int M = 73;
    public const int N = 74;
    public const int NESTED_ML_COMMENT = 60;
    public const int O = 75;
    public const int H = 68;
    public const int I = 69;
    public const int J = 70;
    public const int NEWLINE2 = 57;
    public const int K = 71;
    public const int NEWLINE3 = 58;
    public const int ASTEXPRESSION1 = 21;
    public const int U = 81;
    public const int T = 80;
    public const int W = 83;
    public const int WHITESPACE = 55;
    public const int NEGATE = 4;
    public const int V = 82;
    public const int Q = 77;
    public const int P = 76;
    public const int S = 79;
    public const int ASTFRML = 5;
    public const int R = 78;
    public const int MINUS = 40;
    public const int MULT = 43;
    public const int SEMI = 30;
    public const int Y = 85;
    public const int X = 84;
    public const int Z = 86;
    public const int L1 = 31;
    public const int L2 = 33;
    public const int L3 = 35;
    public const int ASTLEFTSIDE = 6;
    public const int NEWLINE = 56;
    public const int Ident = 46;
    public const int ASTEXPRESSION = 8;
    public const int STARS = 47;
    public const int LB = 49;
    public const int ASTDOUBLE = 14;
    public const int ASTSUM = 26;
    public const int DIV = 44;
    public const int ASTDEFINITION = 24;
    public const int Integer = 39;

    // delegates
    // delegators



        public GAMSParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public GAMSParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();
            this.state.ruleMemo = new Hashtable[62+1];
             
             
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
    // GAMS.g:109:1: extraTokens : SUM ;
    public GAMSParser.extraTokens_return extraTokens() // throws RecognitionException [1]
    {   
        GAMSParser.extraTokens_return retval = new GAMSParser.extraTokens_return();
        retval.Start = input.LT(1);
        int extraTokens_StartIndex = input.Index();
        object root_0 = null;

        IToken SUM1 = null;

        object SUM1_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 1) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:109:13: ( SUM )
            // GAMS.g:110:5: SUM
            {
            	root_0 = (object)adaptor.GetNilNode();

            	SUM1=(IToken)Match(input,SUM,FOLLOW_SUM_in_extraTokens202); if (state.failed) return retval;
            	if ( state.backtracking == 0 )
            	{SUM1_tree = (object)adaptor.Create(SUM1);
            		adaptor.AddChild(root_0, SUM1_tree);
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
    // GAMS.g:117:1: expr : ( expr2 )+ EOF ;
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
            // GAMS.g:117:6: ( ( expr2 )+ EOF )
            // GAMS.g:117:8: ( expr2 )+ EOF
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// GAMS.g:117:8: ( expr2 )+
            	int cnt1 = 0;
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( (LA1_0 == SUM || LA1_0 == Ident) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // GAMS.g:117:9: expr2
            			    {
            			    	PushFollow(FOLLOW_expr2_in_expr220);
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

            	EOF3=(IToken)Match(input,EOF,FOLLOW_EOF_in_expr224); if (state.failed) return retval;
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
    // GAMS.g:119:1: expr2 : frml ;
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
            // GAMS.g:119:10: ( frml )
            // GAMS.g:120:3: frml
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_frml_in_expr2244);
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
    // GAMS.g:126:1: frml : definition DOUBLEDOT expression2 EQUAL expression2 SEMI -> ^( ASTFRML definition expression2 expression2 ) ;
    public GAMSParser.frml_return frml() // throws RecognitionException [1]
    {   
        GAMSParser.frml_return retval = new GAMSParser.frml_return();
        retval.Start = input.LT(1);
        int frml_StartIndex = input.Index();
        object root_0 = null;

        IToken DOUBLEDOT6 = null;
        IToken EQUAL8 = null;
        IToken SEMI10 = null;
        GAMSParser.definition_return definition5 = default(GAMSParser.definition_return);

        GAMSParser.expression2_return expression27 = default(GAMSParser.expression2_return);

        GAMSParser.expression2_return expression29 = default(GAMSParser.expression2_return);


        object DOUBLEDOT6_tree=null;
        object EQUAL8_tree=null;
        object SEMI10_tree=null;
        RewriteRuleTokenStream stream_DOUBLEDOT = new RewriteRuleTokenStream(adaptor,"token DOUBLEDOT");
        RewriteRuleTokenStream stream_SEMI = new RewriteRuleTokenStream(adaptor,"token SEMI");
        RewriteRuleTokenStream stream_EQUAL = new RewriteRuleTokenStream(adaptor,"token EQUAL");
        RewriteRuleSubtreeStream stream_definition = new RewriteRuleSubtreeStream(adaptor,"rule definition");
        RewriteRuleSubtreeStream stream_expression2 = new RewriteRuleSubtreeStream(adaptor,"rule expression2");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 4) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:126:9: ( definition DOUBLEDOT expression2 EQUAL expression2 SEMI -> ^( ASTFRML definition expression2 expression2 ) )
            // GAMS.g:126:11: definition DOUBLEDOT expression2 EQUAL expression2 SEMI
            {
            	PushFollow(FOLLOW_definition_in_frml264);
            	definition5 = definition();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_definition.Add(definition5.Tree);
            	DOUBLEDOT6=(IToken)Match(input,DOUBLEDOT,FOLLOW_DOUBLEDOT_in_frml266); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_DOUBLEDOT.Add(DOUBLEDOT6);

            	PushFollow(FOLLOW_expression2_in_frml268);
            	expression27 = expression2();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression2.Add(expression27.Tree);
            	EQUAL8=(IToken)Match(input,EQUAL,FOLLOW_EQUAL_in_frml270); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_EQUAL.Add(EQUAL8);

            	PushFollow(FOLLOW_expression2_in_frml272);
            	expression29 = expression2();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression2.Add(expression29.Tree);
            	SEMI10=(IToken)Match(input,SEMI,FOLLOW_SEMI_in_frml274); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_SEMI.Add(SEMI10);

            	if ( (state.backtracking==0) )
            	{
            	  frmlItems.Add(input.ToString((IToken)retval.Start,input.LT(-1)));
            	}


            	// AST REWRITE
            	// elements:          definition, expression2, expression2
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 127:3: -> ^( ASTFRML definition expression2 expression2 )
            	{
            	    // GAMS.g:127:6: ^( ASTFRML definition expression2 expression2 )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFRML, "ASTFRML"), root_1);

            	    adaptor.AddChild(root_1, stream_definition.NextTree());
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
            	Memoize(input, 4, frml_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "frml"

    public class definition_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "definition"
    // GAMS.g:129:1: definition : ( variable | variableWithIndexer );
    public GAMSParser.definition_return definition() // throws RecognitionException [1]
    {   
        GAMSParser.definition_return retval = new GAMSParser.definition_return();
        retval.Start = input.LT(1);
        int definition_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.variable_return variable11 = default(GAMSParser.variable_return);

        GAMSParser.variableWithIndexer_return variableWithIndexer12 = default(GAMSParser.variableWithIndexer_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 5) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:129:11: ( variable | variableWithIndexer )
            int alt2 = 2;
            alt2 = dfa2.Predict(input);
            switch (alt2) 
            {
                case 1 :
                    // GAMS.g:129:13: variable
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variable_in_definition298);
                    	variable11 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable11.Tree);

                    }
                    break;
                case 2 :
                    // GAMS.g:129:24: variableWithIndexer
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variableWithIndexer_in_definition302);
                    	variableWithIndexer12 = variableWithIndexer();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variableWithIndexer12.Tree);

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
            	Memoize(input, 5, definition_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "definition"

    public class variableWithIndexer_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variableWithIndexer"
    // GAMS.g:130:1: variableWithIndexer : variable ( indexer1 | indexer2 | indexer3 ) ;
    public GAMSParser.variableWithIndexer_return variableWithIndexer() // throws RecognitionException [1]
    {   
        GAMSParser.variableWithIndexer_return retval = new GAMSParser.variableWithIndexer_return();
        retval.Start = input.LT(1);
        int variableWithIndexer_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.variable_return variable13 = default(GAMSParser.variable_return);

        GAMSParser.indexer1_return indexer114 = default(GAMSParser.indexer1_return);

        GAMSParser.indexer2_return indexer215 = default(GAMSParser.indexer2_return);

        GAMSParser.indexer3_return indexer316 = default(GAMSParser.indexer3_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 6) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:130:21: ( variable ( indexer1 | indexer2 | indexer3 ) )
            // GAMS.g:130:23: variable ( indexer1 | indexer2 | indexer3 )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_variable_in_variableWithIndexer309);
            	variable13 = variable();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable13.Tree);
            	// GAMS.g:130:32: ( indexer1 | indexer2 | indexer3 )
            	int alt3 = 3;
            	switch ( input.LA(1) ) 
            	{
            	case L1:
            		{
            	    alt3 = 1;
            	    }
            	    break;
            	case L2:
            		{
            	    alt3 = 2;
            	    }
            	    break;
            	case L3:
            		{
            	    alt3 = 3;
            	    }
            	    break;
            		default:
            		    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            		    NoViableAltException nvae_d3s0 =
            		        new NoViableAltException("", 3, 0, input);

            		    throw nvae_d3s0;
            	}

            	switch (alt3) 
            	{
            	    case 1 :
            	        // GAMS.g:130:33: indexer1
            	        {
            	        	PushFollow(FOLLOW_indexer1_in_variableWithIndexer312);
            	        	indexer114 = indexer1();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, indexer114.Tree);

            	        }
            	        break;
            	    case 2 :
            	        // GAMS.g:130:44: indexer2
            	        {
            	        	PushFollow(FOLLOW_indexer2_in_variableWithIndexer316);
            	        	indexer215 = indexer2();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, indexer215.Tree);

            	        }
            	        break;
            	    case 3 :
            	        // GAMS.g:130:55: indexer3
            	        {
            	        	PushFollow(FOLLOW_indexer3_in_variableWithIndexer320);
            	        	indexer316 = indexer3();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, indexer316.Tree);

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
            	Memoize(input, 6, variableWithIndexer_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "variableWithIndexer"

    public class indexer1_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "indexer1"
    // GAMS.g:132:1: indexer1 : L1 indexerElements R1 -> ^( ASTINDEXES1 indexerElements ) ;
    public GAMSParser.indexer1_return indexer1() // throws RecognitionException [1]
    {   
        GAMSParser.indexer1_return retval = new GAMSParser.indexer1_return();
        retval.Start = input.LT(1);
        int indexer1_StartIndex = input.Index();
        object root_0 = null;

        IToken L117 = null;
        IToken R119 = null;
        GAMSParser.indexerElements_return indexerElements18 = default(GAMSParser.indexerElements_return);


        object L117_tree=null;
        object R119_tree=null;
        RewriteRuleTokenStream stream_L1 = new RewriteRuleTokenStream(adaptor,"token L1");
        RewriteRuleTokenStream stream_R1 = new RewriteRuleTokenStream(adaptor,"token R1");
        RewriteRuleSubtreeStream stream_indexerElements = new RewriteRuleSubtreeStream(adaptor,"rule indexerElements");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 7) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:132:10: ( L1 indexerElements R1 -> ^( ASTINDEXES1 indexerElements ) )
            // GAMS.g:132:12: L1 indexerElements R1
            {
            	L117=(IToken)Match(input,L1,FOLLOW_L1_in_indexer1329); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_L1.Add(L117);

            	PushFollow(FOLLOW_indexerElements_in_indexer1331);
            	indexerElements18 = indexerElements();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements18.Tree);
            	R119=(IToken)Match(input,R1,FOLLOW_R1_in_indexer1333); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_R1.Add(R119);



            	// AST REWRITE
            	// elements:          indexerElements
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 132:34: -> ^( ASTINDEXES1 indexerElements )
            	{
            	    // GAMS.g:132:37: ^( ASTINDEXES1 indexerElements )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTINDEXES1, "ASTINDEXES1"), root_1);

            	    adaptor.AddChild(root_1, stream_indexerElements.NextTree());

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
            	Memoize(input, 7, indexer1_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "indexer1"

    public class indexer2_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "indexer2"
    // GAMS.g:133:1: indexer2 : L2 indexerElements R2 -> ^( ASTINDEXES2 indexerElements ) ;
    public GAMSParser.indexer2_return indexer2() // throws RecognitionException [1]
    {   
        GAMSParser.indexer2_return retval = new GAMSParser.indexer2_return();
        retval.Start = input.LT(1);
        int indexer2_StartIndex = input.Index();
        object root_0 = null;

        IToken L220 = null;
        IToken R222 = null;
        GAMSParser.indexerElements_return indexerElements21 = default(GAMSParser.indexerElements_return);


        object L220_tree=null;
        object R222_tree=null;
        RewriteRuleTokenStream stream_L2 = new RewriteRuleTokenStream(adaptor,"token L2");
        RewriteRuleTokenStream stream_R2 = new RewriteRuleTokenStream(adaptor,"token R2");
        RewriteRuleSubtreeStream stream_indexerElements = new RewriteRuleSubtreeStream(adaptor,"rule indexerElements");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 8) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:133:10: ( L2 indexerElements R2 -> ^( ASTINDEXES2 indexerElements ) )
            // GAMS.g:133:12: L2 indexerElements R2
            {
            	L220=(IToken)Match(input,L2,FOLLOW_L2_in_indexer2348); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_L2.Add(L220);

            	PushFollow(FOLLOW_indexerElements_in_indexer2350);
            	indexerElements21 = indexerElements();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements21.Tree);
            	R222=(IToken)Match(input,R2,FOLLOW_R2_in_indexer2352); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_R2.Add(R222);



            	// AST REWRITE
            	// elements:          indexerElements
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 133:34: -> ^( ASTINDEXES2 indexerElements )
            	{
            	    // GAMS.g:133:37: ^( ASTINDEXES2 indexerElements )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTINDEXES2, "ASTINDEXES2"), root_1);

            	    adaptor.AddChild(root_1, stream_indexerElements.NextTree());

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
            	Memoize(input, 8, indexer2_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "indexer2"

    public class indexer3_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "indexer3"
    // GAMS.g:134:1: indexer3 : L3 indexerElements R3 -> ^( ASTINDEXES3 indexerElements ) ;
    public GAMSParser.indexer3_return indexer3() // throws RecognitionException [1]
    {   
        GAMSParser.indexer3_return retval = new GAMSParser.indexer3_return();
        retval.Start = input.LT(1);
        int indexer3_StartIndex = input.Index();
        object root_0 = null;

        IToken L323 = null;
        IToken R325 = null;
        GAMSParser.indexerElements_return indexerElements24 = default(GAMSParser.indexerElements_return);


        object L323_tree=null;
        object R325_tree=null;
        RewriteRuleTokenStream stream_L3 = new RewriteRuleTokenStream(adaptor,"token L3");
        RewriteRuleTokenStream stream_R3 = new RewriteRuleTokenStream(adaptor,"token R3");
        RewriteRuleSubtreeStream stream_indexerElements = new RewriteRuleSubtreeStream(adaptor,"rule indexerElements");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 9) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:134:10: ( L3 indexerElements R3 -> ^( ASTINDEXES3 indexerElements ) )
            // GAMS.g:134:12: L3 indexerElements R3
            {
            	L323=(IToken)Match(input,L3,FOLLOW_L3_in_indexer3367); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_L3.Add(L323);

            	PushFollow(FOLLOW_indexerElements_in_indexer3369);
            	indexerElements24 = indexerElements();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_indexerElements.Add(indexerElements24.Tree);
            	R325=(IToken)Match(input,R3,FOLLOW_R3_in_indexer3371); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_R3.Add(R325);



            	// AST REWRITE
            	// elements:          indexerElements
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 134:34: -> ^( ASTINDEXES3 indexerElements )
            	{
            	    // GAMS.g:134:37: ^( ASTINDEXES3 indexerElements )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTINDEXES3, "ASTINDEXES3"), root_1);

            	    adaptor.AddChild(root_1, stream_indexerElements.NextTree());

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
            	Memoize(input, 9, indexer3_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "indexer3"

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
    // GAMS.g:136:1: indexerElements : variableLagLead ( COMMA variableLagLead )* -> ( variableLagLead )+ ;
    public GAMSParser.indexerElements_return indexerElements() // throws RecognitionException [1]
    {   
        GAMSParser.indexerElements_return retval = new GAMSParser.indexerElements_return();
        retval.Start = input.LT(1);
        int indexerElements_StartIndex = input.Index();
        object root_0 = null;

        IToken COMMA27 = null;
        GAMSParser.variableLagLead_return variableLagLead26 = default(GAMSParser.variableLagLead_return);

        GAMSParser.variableLagLead_return variableLagLead28 = default(GAMSParser.variableLagLead_return);


        object COMMA27_tree=null;
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleSubtreeStream stream_variableLagLead = new RewriteRuleSubtreeStream(adaptor,"rule variableLagLead");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 10) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:136:16: ( variableLagLead ( COMMA variableLagLead )* -> ( variableLagLead )+ )
            // GAMS.g:136:18: variableLagLead ( COMMA variableLagLead )*
            {
            	PushFollow(FOLLOW_variableLagLead_in_indexerElements386);
            	variableLagLead26 = variableLagLead();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_variableLagLead.Add(variableLagLead26.Tree);
            	// GAMS.g:136:34: ( COMMA variableLagLead )*
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
            			    // GAMS.g:136:35: COMMA variableLagLead
            			    {
            			    	COMMA27=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_indexerElements389); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_COMMA.Add(COMMA27);

            			    	PushFollow(FOLLOW_variableLagLead_in_indexerElements391);
            			    	variableLagLead28 = variableLagLead();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_variableLagLead.Add(variableLagLead28.Tree);

            			    }
            			    break;

            			default:
            			    goto loop4;
            	    }
            	} while (true);

            	loop4:
            		;	// Stops C# compiler whining that label 'loop4' has no statements



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
            	// 136:59: -> ( variableLagLead )+
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
    // GAMS.g:137:1: variableLagLead : ( variable | variable PLUS Integer | variable MINUS Integer );
    public GAMSParser.variableLagLead_return variableLagLead() // throws RecognitionException [1]
    {   
        GAMSParser.variableLagLead_return retval = new GAMSParser.variableLagLead_return();
        retval.Start = input.LT(1);
        int variableLagLead_StartIndex = input.Index();
        object root_0 = null;

        IToken PLUS31 = null;
        IToken Integer32 = null;
        IToken MINUS34 = null;
        IToken Integer35 = null;
        GAMSParser.variable_return variable29 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable30 = default(GAMSParser.variable_return);

        GAMSParser.variable_return variable33 = default(GAMSParser.variable_return);


        object PLUS31_tree=null;
        object Integer32_tree=null;
        object MINUS34_tree=null;
        object Integer35_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 11) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:137:16: ( variable | variable PLUS Integer | variable MINUS Integer )
            int alt5 = 3;
            alt5 = dfa5.Predict(input);
            switch (alt5) 
            {
                case 1 :
                    // GAMS.g:137:18: variable
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variable_in_variableLagLead405);
                    	variable29 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable29.Tree);

                    }
                    break;
                case 2 :
                    // GAMS.g:137:29: variable PLUS Integer
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variable_in_variableLagLead409);
                    	variable30 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable30.Tree);
                    	PLUS31=(IToken)Match(input,PLUS,FOLLOW_PLUS_in_variableLagLead411); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{PLUS31_tree = (object)adaptor.Create(PLUS31);
                    		adaptor.AddChild(root_0, PLUS31_tree);
                    	}
                    	Integer32=(IToken)Match(input,Integer,FOLLOW_Integer_in_variableLagLead413); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Integer32_tree = (object)adaptor.Create(Integer32);
                    		adaptor.AddChild(root_0, Integer32_tree);
                    	}

                    }
                    break;
                case 3 :
                    // GAMS.g:137:53: variable MINUS Integer
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variable_in_variableLagLead417);
                    	variable33 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable33.Tree);
                    	MINUS34=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_variableLagLead419); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{MINUS34_tree = (object)adaptor.Create(MINUS34);
                    		adaptor.AddChild(root_0, MINUS34_tree);
                    	}
                    	Integer35=(IToken)Match(input,Integer,FOLLOW_Integer_in_variableLagLead421); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Integer35_tree = (object)adaptor.Create(Integer35);
                    		adaptor.AddChild(root_0, Integer35_tree);
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
    // GAMS.g:139:1: expression2 : expression -> ^( ASTEXPRESSION expression ) ;
    public GAMSParser.expression2_return expression2() // throws RecognitionException [1]
    {   
        GAMSParser.expression2_return retval = new GAMSParser.expression2_return();
        retval.Start = input.LT(1);
        int expression2_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.expression_return expression36 = default(GAMSParser.expression_return);


        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 12) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:139:13: ( expression -> ^( ASTEXPRESSION expression ) )
            // GAMS.g:139:15: expression
            {
            	PushFollow(FOLLOW_expression_in_expression2430);
            	expression36 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression36.Tree);


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
            	// 139:26: -> ^( ASTEXPRESSION expression )
            	{
            	    // GAMS.g:139:29: ^( ASTEXPRESSION expression )
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
    // GAMS.g:140:1: number : ( Double | Integer ) ;
    public GAMSParser.number_return number() // throws RecognitionException [1]
    {   
        GAMSParser.number_return retval = new GAMSParser.number_return();
        retval.Start = input.LT(1);
        int number_StartIndex = input.Index();
        object root_0 = null;

        IToken set37 = null;

        object set37_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 13) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:140:7: ( ( Double | Integer ) )
            // GAMS.g:140:9: ( Double | Integer )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set37 = (IToken)input.LT(1);
            	if ( input.LA(1) == Integer || input.LA(1) == Double ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set37));
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
    // GAMS.g:142:1: conditional : DOLLAR expression ;
    public GAMSParser.conditional_return conditional() // throws RecognitionException [1]
    {   
        GAMSParser.conditional_return retval = new GAMSParser.conditional_return();
        retval.Start = input.LT(1);
        int conditional_StartIndex = input.Index();
        object root_0 = null;

        IToken DOLLAR38 = null;
        GAMSParser.expression_return expression39 = default(GAMSParser.expression_return);


        object DOLLAR38_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 14) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:142:12: ( DOLLAR expression )
            // GAMS.g:142:14: DOLLAR expression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	DOLLAR38=(IToken)Match(input,DOLLAR,FOLLOW_DOLLAR_in_conditional455); if (state.failed) return retval;
            	if ( state.backtracking == 0 )
            	{DOLLAR38_tree = (object)adaptor.Create(DOLLAR38);
            		adaptor.AddChild(root_0, DOLLAR38_tree);
            	}
            	PushFollow(FOLLOW_expression_in_conditional457);
            	expression39 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression39.Tree);

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
    // GAMS.g:148:1: expression : expressionStart ( conditional )? -> ^( ASTCONDITIONAL expressionStart ( conditional )? ) ;
    public GAMSParser.expression_return expression() // throws RecognitionException [1]
    {   
        GAMSParser.expression_return retval = new GAMSParser.expression_return();
        retval.Start = input.LT(1);
        int expression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.expressionStart_return expressionStart40 = default(GAMSParser.expressionStart_return);

        GAMSParser.conditional_return conditional41 = default(GAMSParser.conditional_return);


        RewriteRuleSubtreeStream stream_expressionStart = new RewriteRuleSubtreeStream(adaptor,"rule expressionStart");
        RewriteRuleSubtreeStream stream_conditional = new RewriteRuleSubtreeStream(adaptor,"rule conditional");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 15) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:148:12: ( expressionStart ( conditional )? -> ^( ASTCONDITIONAL expressionStart ( conditional )? ) )
            // GAMS.g:148:15: expressionStart ( conditional )?
            {
            	PushFollow(FOLLOW_expressionStart_in_expression470);
            	expressionStart40 = expressionStart();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expressionStart.Add(expressionStart40.Tree);
            	// GAMS.g:148:31: ( conditional )?
            	int alt6 = 2;
            	int LA6_0 = input.LA(1);

            	if ( (LA6_0 == DOLLAR) )
            	{
            	    alt6 = 1;
            	}
            	switch (alt6) 
            	{
            	    case 1 :
            	        // GAMS.g:0:0: conditional
            	        {
            	        	PushFollow(FOLLOW_conditional_in_expression472);
            	        	conditional41 = conditional();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_conditional.Add(conditional41.Tree);

            	        }
            	        break;

            	}



            	// AST REWRITE
            	// elements:          expressionStart, conditional
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 148:44: -> ^( ASTCONDITIONAL expressionStart ( conditional )? )
            	{
            	    // GAMS.g:148:47: ^( ASTCONDITIONAL expressionStart ( conditional )? )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTCONDITIONAL, "ASTCONDITIONAL"), root_1);

            	    adaptor.AddChild(root_1, stream_expressionStart.NextTree());
            	    // GAMS.g:148:80: ( conditional )?
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
            	Memoize(input, 15, expression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "expression"

    public class expressionStart_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expressionStart"
    // GAMS.g:150:1: expressionStart : additiveExpression ;
    public GAMSParser.expressionStart_return expressionStart() // throws RecognitionException [1]
    {   
        GAMSParser.expressionStart_return retval = new GAMSParser.expressionStart_return();
        retval.Start = input.LT(1);
        int expressionStart_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.additiveExpression_return additiveExpression42 = default(GAMSParser.additiveExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 16) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:150:17: ( additiveExpression )
            // GAMS.g:150:21: additiveExpression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_additiveExpression_in_expressionStart494);
            	additiveExpression42 = additiveExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression42.Tree);

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
            	Memoize(input, 16, expressionStart_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "expressionStart"

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
    // GAMS.g:152:1: additiveExpression : multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* ;
    public GAMSParser.additiveExpression_return additiveExpression() // throws RecognitionException [1]
    {   
        GAMSParser.additiveExpression_return retval = new GAMSParser.additiveExpression_return();
        retval.Start = input.LT(1);
        int additiveExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set44 = null;
        GAMSParser.multiplicativeExpression_return multiplicativeExpression43 = default(GAMSParser.multiplicativeExpression_return);

        GAMSParser.multiplicativeExpression_return multiplicativeExpression45 = default(GAMSParser.multiplicativeExpression_return);


        object set44_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 17) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:152:21: ( multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* )
            // GAMS.g:152:23: multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression503);
            	multiplicativeExpression43 = multiplicativeExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression43.Tree);
            	// GAMS.g:152:48: ( ( PLUS | MINUS ) multiplicativeExpression )*
            	do 
            	{
            	    int alt7 = 2;
            	    alt7 = dfa7.Predict(input);
            	    switch (alt7) 
            		{
            			case 1 :
            			    // GAMS.g:152:50: ( PLUS | MINUS ) multiplicativeExpression
            			    {
            			    	set44=(IToken)input.LT(1);
            			    	set44 = (IToken)input.LT(1);
            			    	if ( input.LA(1) == PLUS || input.LA(1) == MINUS ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set44), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression514);
            			    	multiplicativeExpression45 = multiplicativeExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression45.Tree);

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
            	Memoize(input, 17, additiveExpression_StartIndex); 
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
    // GAMS.g:154:1: multiplicativeExpression : powerExpression ( ( MULT | DIV | MOD ) powerExpression )* ;
    public GAMSParser.multiplicativeExpression_return multiplicativeExpression() // throws RecognitionException [1]
    {   
        GAMSParser.multiplicativeExpression_return retval = new GAMSParser.multiplicativeExpression_return();
        retval.Start = input.LT(1);
        int multiplicativeExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set47 = null;
        GAMSParser.powerExpression_return powerExpression46 = default(GAMSParser.powerExpression_return);

        GAMSParser.powerExpression_return powerExpression48 = default(GAMSParser.powerExpression_return);


        object set47_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 18) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:154:28: ( powerExpression ( ( MULT | DIV | MOD ) powerExpression )* )
            // GAMS.g:154:30: powerExpression ( ( MULT | DIV | MOD ) powerExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression527);
            	powerExpression46 = powerExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression46.Tree);
            	// GAMS.g:154:46: ( ( MULT | DIV | MOD ) powerExpression )*
            	do 
            	{
            	    int alt8 = 2;
            	    alt8 = dfa8.Predict(input);
            	    switch (alt8) 
            		{
            			case 1 :
            			    // GAMS.g:154:48: ( MULT | DIV | MOD ) powerExpression
            			    {
            			    	set47=(IToken)input.LT(1);
            			    	set47 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= MULT && input.LA(1) <= MOD) ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set47), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression540);
            			    	powerExpression48 = powerExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression48.Tree);

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
            	Memoize(input, 18, multiplicativeExpression_StartIndex); 
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
    // GAMS.g:156:1: powerExpression : unaryExpression ( pow unaryExpression )* ;
    public GAMSParser.powerExpression_return powerExpression() // throws RecognitionException [1]
    {   
        GAMSParser.powerExpression_return retval = new GAMSParser.powerExpression_return();
        retval.Start = input.LT(1);
        int powerExpression_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.unaryExpression_return unaryExpression49 = default(GAMSParser.unaryExpression_return);

        GAMSParser.pow_return pow50 = default(GAMSParser.pow_return);

        GAMSParser.unaryExpression_return unaryExpression51 = default(GAMSParser.unaryExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 19) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:156:19: ( unaryExpression ( pow unaryExpression )* )
            // GAMS.g:156:21: unaryExpression ( pow unaryExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_unaryExpression_in_powerExpression553);
            	unaryExpression49 = unaryExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression49.Tree);
            	// GAMS.g:156:37: ( pow unaryExpression )*
            	do 
            	{
            	    int alt9 = 2;
            	    alt9 = dfa9.Predict(input);
            	    switch (alt9) 
            		{
            			case 1 :
            			    // GAMS.g:156:39: pow unaryExpression
            			    {
            			    	PushFollow(FOLLOW_pow_in_powerExpression557);
            			    	pow50 = pow();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot(pow50.Tree, root_0);
            			    	PushFollow(FOLLOW_unaryExpression_in_powerExpression560);
            			    	unaryExpression51 = unaryExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression51.Tree);

            			    }
            			    break;

            			default:
            			    goto loop9;
            	    }
            	} while (true);

            	loop9:
            		;	// Stops C# compiler whining that label 'loop9' has no statements


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
            	Memoize(input, 19, powerExpression_StartIndex); 
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
    // GAMS.g:158:1: unaryExpression : ( primaryExpression | MINUS primaryExpression -> ^( NEGATE primaryExpression ) );
    public GAMSParser.unaryExpression_return unaryExpression() // throws RecognitionException [1]
    {   
        GAMSParser.unaryExpression_return retval = new GAMSParser.unaryExpression_return();
        retval.Start = input.LT(1);
        int unaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken MINUS53 = null;
        GAMSParser.primaryExpression_return primaryExpression52 = default(GAMSParser.primaryExpression_return);

        GAMSParser.primaryExpression_return primaryExpression54 = default(GAMSParser.primaryExpression_return);


        object MINUS53_tree=null;
        RewriteRuleTokenStream stream_MINUS = new RewriteRuleTokenStream(adaptor,"token MINUS");
        RewriteRuleSubtreeStream stream_primaryExpression = new RewriteRuleSubtreeStream(adaptor,"rule primaryExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 20) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:158:19: ( primaryExpression | MINUS primaryExpression -> ^( NEGATE primaryExpression ) )
            int alt10 = 2;
            int LA10_0 = input.LA(1);

            if ( (LA10_0 == SUM || LA10_0 == L1 || LA10_0 == L2 || LA10_0 == L3 || LA10_0 == Integer || LA10_0 == Double || LA10_0 == Ident) )
            {
                alt10 = 1;
            }
            else if ( (LA10_0 == MINUS) )
            {
                alt10 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d10s0 =
                    new NoViableAltException("", 10, 0, input);

                throw nvae_d10s0;
            }
            switch (alt10) 
            {
                case 1 :
                    // GAMS.g:158:21: primaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_primaryExpression_in_unaryExpression574);
                    	primaryExpression52 = primaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, primaryExpression52.Tree);

                    }
                    break;
                case 2 :
                    // GAMS.g:159:10: MINUS primaryExpression
                    {
                    	MINUS53=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_unaryExpression585); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_MINUS.Add(MINUS53);

                    	PushFollow(FOLLOW_primaryExpression_in_unaryExpression587);
                    	primaryExpression54 = primaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_primaryExpression.Add(primaryExpression54.Tree);


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
                    	// 159:34: -> ^( NEGATE primaryExpression )
                    	{
                    	    // GAMS.g:159:37: ^( NEGATE primaryExpression )
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
            	Memoize(input, 20, unaryExpression_StartIndex); 
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
    // GAMS.g:161:1: primaryExpression : ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value );
    public GAMSParser.primaryExpression_return primaryExpression() // throws RecognitionException [1]
    {   
        GAMSParser.primaryExpression_return retval = new GAMSParser.primaryExpression_return();
        retval.Start = input.LT(1);
        int primaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken L155 = null;
        IToken R157 = null;
        IToken L258 = null;
        IToken R260 = null;
        IToken L361 = null;
        IToken R363 = null;
        GAMSParser.expression_return expression56 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression59 = default(GAMSParser.expression_return);

        GAMSParser.expression_return expression62 = default(GAMSParser.expression_return);

        GAMSParser.value_return value64 = default(GAMSParser.value_return);


        object L155_tree=null;
        object R157_tree=null;
        object L258_tree=null;
        object R260_tree=null;
        object L361_tree=null;
        object R363_tree=null;
        RewriteRuleTokenStream stream_L1 = new RewriteRuleTokenStream(adaptor,"token L1");
        RewriteRuleTokenStream stream_L2 = new RewriteRuleTokenStream(adaptor,"token L2");
        RewriteRuleTokenStream stream_L3 = new RewriteRuleTokenStream(adaptor,"token L3");
        RewriteRuleTokenStream stream_R2 = new RewriteRuleTokenStream(adaptor,"token R2");
        RewriteRuleTokenStream stream_R3 = new RewriteRuleTokenStream(adaptor,"token R3");
        RewriteRuleTokenStream stream_R1 = new RewriteRuleTokenStream(adaptor,"token R1");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 21) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:162:4: ( L1 expression R1 -> ^( ASTEXPRESSION1 expression ) | L2 expression R2 -> ^( ASTEXPRESSION2 expression ) | L3 expression R3 -> ^( ASTEXPRESSION3 expression ) | value )
            int alt11 = 4;
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
            case SUM:
            case Integer:
            case Double:
            case Ident:
            	{
                alt11 = 4;
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
                    // GAMS.g:162:6: L1 expression R1
                    {
                    	L155=(IToken)Match(input,L1,FOLLOW_L1_in_primaryExpression614); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L1.Add(L155);

                    	PushFollow(FOLLOW_expression_in_primaryExpression616);
                    	expression56 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression56.Tree);
                    	R157=(IToken)Match(input,R1,FOLLOW_R1_in_primaryExpression618); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R1.Add(R157);



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
                    	// 162:23: -> ^( ASTEXPRESSION1 expression )
                    	{
                    	    // GAMS.g:162:26: ^( ASTEXPRESSION1 expression )
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
                    // GAMS.g:163:6: L2 expression R2
                    {
                    	L258=(IToken)Match(input,L2,FOLLOW_L2_in_primaryExpression633); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L2.Add(L258);

                    	PushFollow(FOLLOW_expression_in_primaryExpression635);
                    	expression59 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression59.Tree);
                    	R260=(IToken)Match(input,R2,FOLLOW_R2_in_primaryExpression637); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R2.Add(R260);



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
                    	// 163:23: -> ^( ASTEXPRESSION2 expression )
                    	{
                    	    // GAMS.g:163:26: ^( ASTEXPRESSION2 expression )
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
                    // GAMS.g:164:8: L3 expression R3
                    {
                    	L361=(IToken)Match(input,L3,FOLLOW_L3_in_primaryExpression654); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_L3.Add(L361);

                    	PushFollow(FOLLOW_expression_in_primaryExpression656);
                    	expression62 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression62.Tree);
                    	R363=(IToken)Match(input,R3,FOLLOW_R3_in_primaryExpression658); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_R3.Add(R363);



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
                    	// 164:25: -> ^( ASTEXPRESSION3 expression )
                    	{
                    	    // GAMS.g:164:28: ^( ASTEXPRESSION3 expression )
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
                    // GAMS.g:165:6: value
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_value_in_primaryExpression673);
                    	value64 = value();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, value64.Tree);

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
            	Memoize(input, 21, primaryExpression_StartIndex); 
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
    // GAMS.g:167:1: value : ( Integer -> ^( ASTINTEGER Integer ) | Double -> ^( ASTDOUBLE Double ) | sum -> ^( ASTSUM sum ) | definition -> ^( ASTDEFINITION definition ) | ident -> ^( ASTVARIABLE ident ) );
    public GAMSParser.value_return value() // throws RecognitionException [1]
    {   
        GAMSParser.value_return retval = new GAMSParser.value_return();
        retval.Start = input.LT(1);
        int value_StartIndex = input.Index();
        object root_0 = null;

        IToken Integer65 = null;
        IToken Double66 = null;
        GAMSParser.sum_return sum67 = default(GAMSParser.sum_return);

        GAMSParser.definition_return definition68 = default(GAMSParser.definition_return);

        GAMSParser.ident_return ident69 = default(GAMSParser.ident_return);


        object Integer65_tree=null;
        object Double66_tree=null;
        RewriteRuleTokenStream stream_Double = new RewriteRuleTokenStream(adaptor,"token Double");
        RewriteRuleTokenStream stream_Integer = new RewriteRuleTokenStream(adaptor,"token Integer");
        RewriteRuleSubtreeStream stream_ident = new RewriteRuleSubtreeStream(adaptor,"rule ident");
        RewriteRuleSubtreeStream stream_definition = new RewriteRuleSubtreeStream(adaptor,"rule definition");
        RewriteRuleSubtreeStream stream_sum = new RewriteRuleSubtreeStream(adaptor,"rule sum");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 22) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:168:2: ( Integer -> ^( ASTINTEGER Integer ) | Double -> ^( ASTDOUBLE Double ) | sum -> ^( ASTSUM sum ) | definition -> ^( ASTDEFINITION definition ) | ident -> ^( ASTVARIABLE ident ) )
            int alt12 = 5;
            alt12 = dfa12.Predict(input);
            switch (alt12) 
            {
                case 1 :
                    // GAMS.g:168:5: Integer
                    {
                    	Integer65=(IToken)Match(input,Integer,FOLLOW_Integer_in_value687); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer65);



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
                    	// 168:15: -> ^( ASTINTEGER Integer )
                    	{
                    	    // GAMS.g:168:18: ^( ASTINTEGER Integer )
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
                    // GAMS.g:169:4: Double
                    {
                    	Double66=(IToken)Match(input,Double,FOLLOW_Double_in_value702); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Double.Add(Double66);



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
                    	// 169:15: -> ^( ASTDOUBLE Double )
                    	{
                    	    // GAMS.g:169:18: ^( ASTDOUBLE Double )
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
                    // GAMS.g:170:6: sum
                    {
                    	PushFollow(FOLLOW_sum_in_value721);
                    	sum67 = sum();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_sum.Add(sum67.Tree);


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
                    	// 170:17: -> ^( ASTSUM sum )
                    	{
                    	    // GAMS.g:170:20: ^( ASTSUM sum )
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
                    // GAMS.g:171:4: definition
                    {
                    	PushFollow(FOLLOW_definition_in_value741);
                    	definition68 = definition();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_definition.Add(definition68.Tree);


                    	// AST REWRITE
                    	// elements:          definition
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 171:15: -> ^( ASTDEFINITION definition )
                    	{
                    	    // GAMS.g:171:18: ^( ASTDEFINITION definition )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTDEFINITION, "ASTDEFINITION"), root_1);

                    	    adaptor.AddChild(root_1, stream_definition.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 5 :
                    // GAMS.g:172:6: ident
                    {
                    	PushFollow(FOLLOW_ident_in_value756);
                    	ident69 = ident();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_ident.Add(ident69.Tree);


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
                    	// 172:17: -> ^( ASTVARIABLE ident )
                    	{
                    	    // GAMS.g:172:20: ^( ASTVARIABLE ident )
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
            	Memoize(input, 22, value_StartIndex); 
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
    // GAMS.g:175:1: sum : ( SUM L1 sumControlled ( conditional )? COMMA expression R1 | SUM L2 sumControlled ( conditional )? COMMA expression R2 | SUM L3 sumControlled ( conditional )? COMMA expression R3 );
    public GAMSParser.sum_return sum() // throws RecognitionException [1]
    {   
        GAMSParser.sum_return retval = new GAMSParser.sum_return();
        retval.Start = input.LT(1);
        int sum_StartIndex = input.Index();
        object root_0 = null;

        IToken SUM70 = null;
        IToken L171 = null;
        IToken COMMA74 = null;
        IToken R176 = null;
        IToken SUM77 = null;
        IToken L278 = null;
        IToken COMMA81 = null;
        IToken R283 = null;
        IToken SUM84 = null;
        IToken L385 = null;
        IToken COMMA88 = null;
        IToken R390 = null;
        GAMSParser.sumControlled_return sumControlled72 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional73 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression75 = default(GAMSParser.expression_return);

        GAMSParser.sumControlled_return sumControlled79 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional80 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression82 = default(GAMSParser.expression_return);

        GAMSParser.sumControlled_return sumControlled86 = default(GAMSParser.sumControlled_return);

        GAMSParser.conditional_return conditional87 = default(GAMSParser.conditional_return);

        GAMSParser.expression_return expression89 = default(GAMSParser.expression_return);


        object SUM70_tree=null;
        object L171_tree=null;
        object COMMA74_tree=null;
        object R176_tree=null;
        object SUM77_tree=null;
        object L278_tree=null;
        object COMMA81_tree=null;
        object R283_tree=null;
        object SUM84_tree=null;
        object L385_tree=null;
        object COMMA88_tree=null;
        object R390_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 23) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:175:4: ( SUM L1 sumControlled ( conditional )? COMMA expression R1 | SUM L2 sumControlled ( conditional )? COMMA expression R2 | SUM L3 sumControlled ( conditional )? COMMA expression R3 )
            int alt16 = 3;
            int LA16_0 = input.LA(1);

            if ( (LA16_0 == SUM) )
            {
                switch ( input.LA(2) ) 
                {
                case L1:
                	{
                    alt16 = 1;
                    }
                    break;
                case L2:
                	{
                    alt16 = 2;
                    }
                    break;
                case L3:
                	{
                    alt16 = 3;
                    }
                    break;
                	default:
                	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                	    NoViableAltException nvae_d16s1 =
                	        new NoViableAltException("", 16, 1, input);

                	    throw nvae_d16s1;
                }

            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d16s0 =
                    new NoViableAltException("", 16, 0, input);

                throw nvae_d16s0;
            }
            switch (alt16) 
            {
                case 1 :
                    // GAMS.g:175:7: SUM L1 sumControlled ( conditional )? COMMA expression R1
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	SUM70=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum779); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{SUM70_tree = (object)adaptor.Create(SUM70);
                    		adaptor.AddChild(root_0, SUM70_tree);
                    	}
                    	L171=(IToken)Match(input,L1,FOLLOW_L1_in_sum781); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L171_tree = (object)adaptor.Create(L171);
                    		adaptor.AddChild(root_0, L171_tree);
                    	}
                    	PushFollow(FOLLOW_sumControlled_in_sum783);
                    	sumControlled72 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, sumControlled72.Tree);
                    	// GAMS.g:175:28: ( conditional )?
                    	int alt13 = 2;
                    	int LA13_0 = input.LA(1);

                    	if ( (LA13_0 == DOLLAR) )
                    	{
                    	    alt13 = 1;
                    	}
                    	switch (alt13) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:0:0: conditional
                    	        {
                    	        	PushFollow(FOLLOW_conditional_in_sum785);
                    	        	conditional73 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, conditional73.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA74=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum788); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{COMMA74_tree = (object)adaptor.Create(COMMA74);
                    		adaptor.AddChild(root_0, COMMA74_tree);
                    	}
                    	PushFollow(FOLLOW_expression_in_sum790);
                    	expression75 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression75.Tree);
                    	R176=(IToken)Match(input,R1,FOLLOW_R1_in_sum792); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R176_tree = (object)adaptor.Create(R176);
                    		adaptor.AddChild(root_0, R176_tree);
                    	}

                    }
                    break;
                case 2 :
                    // GAMS.g:176:7: SUM L2 sumControlled ( conditional )? COMMA expression R2
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	SUM77=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum800); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{SUM77_tree = (object)adaptor.Create(SUM77);
                    		adaptor.AddChild(root_0, SUM77_tree);
                    	}
                    	L278=(IToken)Match(input,L2,FOLLOW_L2_in_sum802); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L278_tree = (object)adaptor.Create(L278);
                    		adaptor.AddChild(root_0, L278_tree);
                    	}
                    	PushFollow(FOLLOW_sumControlled_in_sum804);
                    	sumControlled79 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, sumControlled79.Tree);
                    	// GAMS.g:176:28: ( conditional )?
                    	int alt14 = 2;
                    	int LA14_0 = input.LA(1);

                    	if ( (LA14_0 == DOLLAR) )
                    	{
                    	    alt14 = 1;
                    	}
                    	switch (alt14) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:0:0: conditional
                    	        {
                    	        	PushFollow(FOLLOW_conditional_in_sum806);
                    	        	conditional80 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, conditional80.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA81=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum809); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{COMMA81_tree = (object)adaptor.Create(COMMA81);
                    		adaptor.AddChild(root_0, COMMA81_tree);
                    	}
                    	PushFollow(FOLLOW_expression_in_sum811);
                    	expression82 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression82.Tree);
                    	R283=(IToken)Match(input,R2,FOLLOW_R2_in_sum813); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R283_tree = (object)adaptor.Create(R283);
                    		adaptor.AddChild(root_0, R283_tree);
                    	}

                    }
                    break;
                case 3 :
                    // GAMS.g:177:7: SUM L3 sumControlled ( conditional )? COMMA expression R3
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	SUM84=(IToken)Match(input,SUM,FOLLOW_SUM_in_sum821); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{SUM84_tree = (object)adaptor.Create(SUM84);
                    		adaptor.AddChild(root_0, SUM84_tree);
                    	}
                    	L385=(IToken)Match(input,L3,FOLLOW_L3_in_sum823); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L385_tree = (object)adaptor.Create(L385);
                    		adaptor.AddChild(root_0, L385_tree);
                    	}
                    	PushFollow(FOLLOW_sumControlled_in_sum825);
                    	sumControlled86 = sumControlled();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, sumControlled86.Tree);
                    	// GAMS.g:177:28: ( conditional )?
                    	int alt15 = 2;
                    	int LA15_0 = input.LA(1);

                    	if ( (LA15_0 == DOLLAR) )
                    	{
                    	    alt15 = 1;
                    	}
                    	switch (alt15) 
                    	{
                    	    case 1 :
                    	        // GAMS.g:0:0: conditional
                    	        {
                    	        	PushFollow(FOLLOW_conditional_in_sum827);
                    	        	conditional87 = conditional();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return retval;
                    	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, conditional87.Tree);

                    	        }
                    	        break;

                    	}

                    	COMMA88=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_sum830); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{COMMA88_tree = (object)adaptor.Create(COMMA88);
                    		adaptor.AddChild(root_0, COMMA88_tree);
                    	}
                    	PushFollow(FOLLOW_expression_in_sum832);
                    	expression89 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression89.Tree);
                    	R390=(IToken)Match(input,R3,FOLLOW_R3_in_sum834); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R390_tree = (object)adaptor.Create(R390);
                    		adaptor.AddChild(root_0, R390_tree);
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
            	Memoize(input, 23, sum_StartIndex); 
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
    // GAMS.g:179:1: sumControlled : ( variable | L1 indexerElements R1 | L2 indexerElements R2 | L3 indexerElements R3 );
    public GAMSParser.sumControlled_return sumControlled() // throws RecognitionException [1]
    {   
        GAMSParser.sumControlled_return retval = new GAMSParser.sumControlled_return();
        retval.Start = input.LT(1);
        int sumControlled_StartIndex = input.Index();
        object root_0 = null;

        IToken L192 = null;
        IToken R194 = null;
        IToken L295 = null;
        IToken R297 = null;
        IToken L398 = null;
        IToken R3100 = null;
        GAMSParser.variable_return variable91 = default(GAMSParser.variable_return);

        GAMSParser.indexerElements_return indexerElements93 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements96 = default(GAMSParser.indexerElements_return);

        GAMSParser.indexerElements_return indexerElements99 = default(GAMSParser.indexerElements_return);


        object L192_tree=null;
        object R194_tree=null;
        object L295_tree=null;
        object R297_tree=null;
        object L398_tree=null;
        object R3100_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 24) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:179:14: ( variable | L1 indexerElements R1 | L2 indexerElements R2 | L3 indexerElements R3 )
            int alt17 = 4;
            switch ( input.LA(1) ) 
            {
            case SUM:
            case Ident:
            	{
                alt17 = 1;
                }
                break;
            case L1:
            	{
                alt17 = 2;
                }
                break;
            case L2:
            	{
                alt17 = 3;
                }
                break;
            case L3:
            	{
                alt17 = 4;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d17s0 =
            	        new NoViableAltException("", 17, 0, input);

            	    throw nvae_d17s0;
            }

            switch (alt17) 
            {
                case 1 :
                    // GAMS.g:180:11: variable
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variable_in_sumControlled851);
                    	variable91 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable91.Tree);

                    }
                    break;
                case 2 :
                    // GAMS.g:181:5: L1 indexerElements R1
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	L192=(IToken)Match(input,L1,FOLLOW_L1_in_sumControlled858); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L192_tree = (object)adaptor.Create(L192);
                    		adaptor.AddChild(root_0, L192_tree);
                    	}
                    	PushFollow(FOLLOW_indexerElements_in_sumControlled860);
                    	indexerElements93 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, indexerElements93.Tree);
                    	R194=(IToken)Match(input,R1,FOLLOW_R1_in_sumControlled862); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R194_tree = (object)adaptor.Create(R194);
                    		adaptor.AddChild(root_0, R194_tree);
                    	}

                    }
                    break;
                case 3 :
                    // GAMS.g:182:5: L2 indexerElements R2
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	L295=(IToken)Match(input,L2,FOLLOW_L2_in_sumControlled868); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L295_tree = (object)adaptor.Create(L295);
                    		adaptor.AddChild(root_0, L295_tree);
                    	}
                    	PushFollow(FOLLOW_indexerElements_in_sumControlled870);
                    	indexerElements96 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, indexerElements96.Tree);
                    	R297=(IToken)Match(input,R2,FOLLOW_R2_in_sumControlled872); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R297_tree = (object)adaptor.Create(R297);
                    		adaptor.AddChild(root_0, R297_tree);
                    	}

                    }
                    break;
                case 4 :
                    // GAMS.g:183:5: L3 indexerElements R3
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	L398=(IToken)Match(input,L3,FOLLOW_L3_in_sumControlled878); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{L398_tree = (object)adaptor.Create(L398);
                    		adaptor.AddChild(root_0, L398_tree);
                    	}
                    	PushFollow(FOLLOW_indexerElements_in_sumControlled880);
                    	indexerElements99 = indexerElements();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, indexerElements99.Tree);
                    	R3100=(IToken)Match(input,R3,FOLLOW_R3_in_sumControlled882); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{R3100_tree = (object)adaptor.Create(R3100);
                    		adaptor.AddChild(root_0, R3100_tree);
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
            	Memoize(input, 24, sumControlled_StartIndex); 
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
    // GAMS.g:185:1: numberPlusMinus : ( MINUS | PLUS ) ( Integer | Double ) ;
    public GAMSParser.numberPlusMinus_return numberPlusMinus() // throws RecognitionException [1]
    {   
        GAMSParser.numberPlusMinus_return retval = new GAMSParser.numberPlusMinus_return();
        retval.Start = input.LT(1);
        int numberPlusMinus_StartIndex = input.Index();
        object root_0 = null;

        IToken set101 = null;
        IToken set102 = null;

        object set101_tree=null;
        object set102_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 25) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:185:18: ( ( MINUS | PLUS ) ( Integer | Double ) )
            // GAMS.g:185:21: ( MINUS | PLUS ) ( Integer | Double )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set101 = (IToken)input.LT(1);
            	if ( input.LA(1) == PLUS || input.LA(1) == MINUS ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set101));
            	    state.errorRecovery = false;state.failed = false;
            	}
            	else 
            	{
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}

            	set102 = (IToken)input.LT(1);
            	if ( input.LA(1) == Integer || input.LA(1) == Double ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set102));
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
            	Memoize(input, 25, numberPlusMinus_StartIndex); 
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
    // GAMS.g:187:1: ident : ( Ident | extraTokens );
    public GAMSParser.ident_return ident() // throws RecognitionException [1]
    {   
        GAMSParser.ident_return retval = new GAMSParser.ident_return();
        retval.Start = input.LT(1);
        int ident_StartIndex = input.Index();
        object root_0 = null;

        IToken Ident103 = null;
        GAMSParser.extraTokens_return extraTokens104 = default(GAMSParser.extraTokens_return);


        object Ident103_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 26) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:187:9: ( Ident | extraTokens )
            int alt18 = 2;
            int LA18_0 = input.LA(1);

            if ( (LA18_0 == Ident) )
            {
                alt18 = 1;
            }
            else if ( (LA18_0 == SUM) )
            {
                alt18 = 2;
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
                    // GAMS.g:187:12: Ident
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Ident103=(IToken)Match(input,Ident,FOLLOW_Ident_in_ident917); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Ident103_tree = (object)adaptor.Create(Ident103);
                    		adaptor.AddChild(root_0, Ident103_tree);
                    	}

                    }
                    break;
                case 2 :
                    // GAMS.g:187:20: extraTokens
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_extraTokens_in_ident921);
                    	extraTokens104 = extraTokens();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, extraTokens104.Tree);

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
            	Memoize(input, 26, ident_StartIndex); 
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
    // GAMS.g:188:1: variable : ident ;
    public GAMSParser.variable_return variable() // throws RecognitionException [1]
    {   
        GAMSParser.variable_return retval = new GAMSParser.variable_return();
        retval.Start = input.LT(1);
        int variable_StartIndex = input.Index();
        object root_0 = null;

        GAMSParser.ident_return ident105 = default(GAMSParser.ident_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 27) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:188:10: ( ident )
            // GAMS.g:188:12: ident
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_ident_in_variable928);
            	ident105 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, ident105.Tree);

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
            	Memoize(input, 27, variable_StartIndex); 
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
    // GAMS.g:190:1: pow : STARS -> ASTPOW ;
    public GAMSParser.pow_return pow() // throws RecognitionException [1]
    {   
        GAMSParser.pow_return retval = new GAMSParser.pow_return();
        retval.Start = input.LT(1);
        int pow_StartIndex = input.Index();
        object root_0 = null;

        IToken STARS106 = null;

        object STARS106_tree=null;
        RewriteRuleTokenStream stream_STARS = new RewriteRuleTokenStream(adaptor,"token STARS");

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 28) ) 
    	    {
    	    	return retval; 
    	    }
            // GAMS.g:190:9: ( STARS -> ASTPOW )
            // GAMS.g:190:17: STARS
            {
            	STARS106=(IToken)Match(input,STARS,FOLLOW_STARS_in_pow946); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_STARS.Add(STARS106);



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
            	// 190:24: -> ASTPOW
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
            	Memoize(input, 28, pow_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "pow"

    // $ANTLR start "synpred23_GAMS"
    public void synpred23_GAMS_fragment() {
        // GAMS.g:171:4: ( definition )
        // GAMS.g:171:4: definition
        {
        	PushFollow(FOLLOW_definition_in_synpred23_GAMS741);
        	definition();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred23_GAMS"

    // Delegated rules

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


   	protected DFA2 dfa2;
   	protected DFA5 dfa5;
   	protected DFA7 dfa7;
   	protected DFA8 dfa8;
   	protected DFA9 dfa9;
   	protected DFA12 dfa12;
	private void InitializeCyclicDFAs()
	{
    	this.dfa2 = new DFA2(this);
    	this.dfa5 = new DFA5(this);
    	this.dfa7 = new DFA7(this);
    	this.dfa8 = new DFA8(this);
    	this.dfa9 = new DFA9(this);
    	this.dfa12 = new DFA12(this);





	    this.dfa12.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA12_SpecialStateTransition);
	}

    const string DFA2_eotS =
        "\x21\uffff";
    const string DFA2_eofS =
        "\x01\uffff\x02\x06\x1e\uffff";
    const string DFA2_minS =
        "\x01\x1b\x02\x1c\x1e\uffff";
    const string DFA2_maxS =
        "\x01\x2e\x02\x2f\x1e\uffff";
    const string DFA2_acceptS =
        "\x03\uffff\x01\x02\x02\uffff\x01\x01\x1a\uffff";
    const string DFA2_specialS =
        "\x21\uffff}>";
    static readonly string[] DFA2_transitionS = {
            "\x01\x02\x12\uffff\x01\x01",
            "\x03\x06\x01\x03\x01\x06\x01\x03\x01\x06\x01\x03\x03\x06\x01"+
            "\uffff\x01\x06\x01\uffff\x04\x06\x01\uffff\x01\x06",
            "\x03\x06\x01\x03\x01\x06\x01\x03\x01\x06\x01\x03\x03\x06\x01"+
            "\uffff\x01\x06\x01\uffff\x04\x06\x01\uffff\x01\x06",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
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
            get { return "129:1: definition : ( variable | variableWithIndexer );"; }
        }

    }

    const string DFA5_eotS =
        "\x11\uffff";
    const string DFA5_eofS =
        "\x01\uffff\x02\x05\x0e\uffff";
    const string DFA5_minS =
        "\x01\x1b\x02\x20\x0e\uffff";
    const string DFA5_maxS =
        "\x01\x2e\x02\x28\x0e\uffff";
    const string DFA5_acceptS =
        "\x03\uffff\x01\x02\x01\x03\x01\x01\x0b\uffff";
    const string DFA5_specialS =
        "\x11\uffff}>";
    static readonly string[] DFA5_transitionS = {
            "\x01\x02\x12\uffff\x01\x01",
            "\x01\x05\x01\uffff\x01\x05\x01\uffff\x02\x05\x01\x03\x01\uffff"+
            "\x01\x04",
            "\x01\x05\x01\uffff\x01\x05\x01\uffff\x02\x05\x01\x03\x01\uffff"+
            "\x01\x04",
            "",
            "",
            "",
            "",
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
            get { return "137:1: variableLagLead : ( variable | variable PLUS Integer | variable MINUS Integer );"; }
        }

    }

    const string DFA7_eotS =
        "\x0a\uffff";
    const string DFA7_eofS =
        "\x01\x01\x09\uffff";
    const string DFA7_minS =
        "\x01\x1d\x09\uffff";
    const string DFA7_maxS =
        "\x01\x2a\x09\uffff";
    const string DFA7_acceptS =
        "\x01\uffff\x01\x02\x07\uffff\x01\x01";
    const string DFA7_specialS =
        "\x0a\uffff}>";
    static readonly string[] DFA7_transitionS = {
            "\x02\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x02"+
            "\x01\x01\x09\x01\uffff\x01\x09\x01\uffff\x01\x01",
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
            get { return "()* loopback of 152:48: ( ( PLUS | MINUS ) multiplicativeExpression )*"; }
        }

    }

    const string DFA8_eotS =
        "\x0b\uffff";
    const string DFA8_eofS =
        "\x01\x01\x0a\uffff";
    const string DFA8_minS =
        "\x01\x1d\x0a\uffff";
    const string DFA8_maxS =
        "\x01\x2d\x0a\uffff";
    const string DFA8_acceptS =
        "\x01\uffff\x01\x02\x08\uffff\x01\x01";
    const string DFA8_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA8_transitionS = {
            "\x02\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x03"+
            "\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x03\x0a",
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
            get { return "()* loopback of 154:46: ( ( MULT | DIV | MOD ) powerExpression )*"; }
        }

    }

    const string DFA9_eotS =
        "\x0c\uffff";
    const string DFA9_eofS =
        "\x01\x01\x0b\uffff";
    const string DFA9_minS =
        "\x01\x1d\x0b\uffff";
    const string DFA9_maxS =
        "\x01\x2f\x0b\uffff";
    const string DFA9_acceptS =
        "\x01\uffff\x01\x02\x09\uffff\x01\x01";
    const string DFA9_specialS =
        "\x0c\uffff}>";
    static readonly string[] DFA9_transitionS = {
            "\x02\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x01\uffff\x03"+
            "\x01\x01\uffff\x01\x01\x01\uffff\x04\x01\x01\uffff\x01\x0b",
            "",
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
            get { return "()* loopback of 156:37: ( pow unaryExpression )*"; }
        }

    }

    const string DFA12_eotS =
        "\x23\uffff";
    const string DFA12_eofS =
        "\x23\uffff";
    const string DFA12_minS =
        "\x01\x1b\x02\uffff\x01\x1f\x01\x00\x1e\uffff";
    const string DFA12_maxS =
        "\x01\x2e\x02\uffff\x01\x23\x01\x00\x1e\uffff";
    const string DFA12_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x02\uffff\x01\x03\x0d\uffff\x01\x04"+
        "\x01\x05\x0e\uffff";
    const string DFA12_specialS =
        "\x03\uffff\x01\x00\x01\x01\x1e\uffff}>";
    static readonly string[] DFA12_transitionS = {
            "\x01\x03\x0b\uffff\x01\x01\x01\uffff\x01\x02\x04\uffff\x01"+
            "\x04",
            "",
            "",
            "\x01\x05\x01\uffff\x01\x05\x01\uffff\x01\x05",
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
            get { return "167:1: value : ( Integer -> ^( ASTINTEGER Integer ) | Double -> ^( ASTDOUBLE Double ) | sum -> ^( ASTSUM sum ) | definition -> ^( ASTDEFINITION definition ) | ident -> ^( ASTVARIABLE ident ) );"; }
        }

    }


    protected internal int DFA12_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA12_3 = input.LA(1);

                   	 
                   	int index12_3 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA12_3 == L1 || LA12_3 == L2 || LA12_3 == L3) ) { s = 5; }

                   	else if ( (synpred23_GAMS()) ) { s = 19; }

                   	else if ( (true) ) { s = 20; }

                   	 
                   	input.Seek(index12_3);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA12_4 = input.LA(1);

                   	 
                   	int index12_4 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred23_GAMS()) ) { s = 19; }

                   	else if ( (true) ) { s = 20; }

                   	 
                   	input.Seek(index12_4);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae12 =
            new NoViableAltException(dfa.Description, 12, _s, input);
        dfa.Error(nvae12);
        throw nvae12;
    }
 

    public static readonly BitSet FOLLOW_SUM_in_extraTokens202 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr2_in_expr220 = new BitSet(new ulong[]{0x0000400008000000UL});
    public static readonly BitSet FOLLOW_EOF_in_expr224 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_frml_in_expr2244 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_definition_in_frml264 = new BitSet(new ulong[]{0x0000000010000000UL});
    public static readonly BitSet FOLLOW_DOUBLEDOT_in_frml266 = new BitSet(new ulong[]{0x0000438A88000000UL});
    public static readonly BitSet FOLLOW_expression2_in_frml268 = new BitSet(new ulong[]{0x0000000020000000UL});
    public static readonly BitSet FOLLOW_EQUAL_in_frml270 = new BitSet(new ulong[]{0x0000438A88000000UL});
    public static readonly BitSet FOLLOW_expression2_in_frml272 = new BitSet(new ulong[]{0x0000000040000000UL});
    public static readonly BitSet FOLLOW_SEMI_in_frml274 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_definition298 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithIndexer_in_definition302 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableWithIndexer309 = new BitSet(new ulong[]{0x0000000A80000000UL});
    public static readonly BitSet FOLLOW_indexer1_in_variableWithIndexer312 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_indexer2_in_variableWithIndexer316 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_indexer3_in_variableWithIndexer320 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_indexer1329 = new BitSet(new ulong[]{0x0000400008000000UL});
    public static readonly BitSet FOLLOW_indexerElements_in_indexer1331 = new BitSet(new ulong[]{0x0000000100000000UL});
    public static readonly BitSet FOLLOW_R1_in_indexer1333 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_indexer2348 = new BitSet(new ulong[]{0x0000400008000000UL});
    public static readonly BitSet FOLLOW_indexerElements_in_indexer2350 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_R2_in_indexer2352 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_indexer3367 = new BitSet(new ulong[]{0x0000400008000000UL});
    public static readonly BitSet FOLLOW_indexerElements_in_indexer3369 = new BitSet(new ulong[]{0x0000001000000000UL});
    public static readonly BitSet FOLLOW_R3_in_indexer3371 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableLagLead_in_indexerElements386 = new BitSet(new ulong[]{0x0000002000000002UL});
    public static readonly BitSet FOLLOW_COMMA_in_indexerElements389 = new BitSet(new ulong[]{0x0000400008000000UL});
    public static readonly BitSet FOLLOW_variableLagLead_in_indexerElements391 = new BitSet(new ulong[]{0x0000002000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead405 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead409 = new BitSet(new ulong[]{0x0000004000000000UL});
    public static readonly BitSet FOLLOW_PLUS_in_variableLagLead411 = new BitSet(new ulong[]{0x0000008000000000UL});
    public static readonly BitSet FOLLOW_Integer_in_variableLagLead413 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_variableLagLead417 = new BitSet(new ulong[]{0x0000010000000000UL});
    public static readonly BitSet FOLLOW_MINUS_in_variableLagLead419 = new BitSet(new ulong[]{0x0000008000000000UL});
    public static readonly BitSet FOLLOW_Integer_in_variableLagLead421 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_expression2430 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_number444 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DOLLAR_in_conditional455 = new BitSet(new ulong[]{0x0000438A88000000UL});
    public static readonly BitSet FOLLOW_expression_in_conditional457 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expressionStart_in_expression470 = new BitSet(new ulong[]{0x0000040000000002UL});
    public static readonly BitSet FOLLOW_conditional_in_expression472 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_expressionStart494 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression503 = new BitSet(new ulong[]{0x0000014000000002UL});
    public static readonly BitSet FOLLOW_set_in_additiveExpression507 = new BitSet(new ulong[]{0x0000438A88000000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression514 = new BitSet(new ulong[]{0x0000014000000002UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression527 = new BitSet(new ulong[]{0x0000380000000002UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpression531 = new BitSet(new ulong[]{0x0000438A88000000UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression540 = new BitSet(new ulong[]{0x0000380000000002UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression553 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_pow_in_powerExpression557 = new BitSet(new ulong[]{0x0000438A88000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression560 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_unaryExpression574 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MINUS_in_unaryExpression585 = new BitSet(new ulong[]{0x0000428A88000000UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_unaryExpression587 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_primaryExpression614 = new BitSet(new ulong[]{0x0000438A88000000UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression616 = new BitSet(new ulong[]{0x0000000100000000UL});
    public static readonly BitSet FOLLOW_R1_in_primaryExpression618 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_primaryExpression633 = new BitSet(new ulong[]{0x0000438A88000000UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression635 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_R2_in_primaryExpression637 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_primaryExpression654 = new BitSet(new ulong[]{0x0000438A88000000UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression656 = new BitSet(new ulong[]{0x0000001000000000UL});
    public static readonly BitSet FOLLOW_R3_in_primaryExpression658 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_value_in_primaryExpression673 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Integer_in_value687 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Double_in_value702 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_sum_in_value721 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_definition_in_value741 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_value756 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUM_in_sum779 = new BitSet(new ulong[]{0x0000000080000000UL});
    public static readonly BitSet FOLLOW_L1_in_sum781 = new BitSet(new ulong[]{0x0000400A88000000UL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum783 = new BitSet(new ulong[]{0x0000042000000000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum785 = new BitSet(new ulong[]{0x0000002000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum788 = new BitSet(new ulong[]{0x0000438A88000000UL});
    public static readonly BitSet FOLLOW_expression_in_sum790 = new BitSet(new ulong[]{0x0000000100000000UL});
    public static readonly BitSet FOLLOW_R1_in_sum792 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUM_in_sum800 = new BitSet(new ulong[]{0x0000000200000000UL});
    public static readonly BitSet FOLLOW_L2_in_sum802 = new BitSet(new ulong[]{0x0000400A88000000UL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum804 = new BitSet(new ulong[]{0x0000042000000000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum806 = new BitSet(new ulong[]{0x0000002000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum809 = new BitSet(new ulong[]{0x0000438A88000000UL});
    public static readonly BitSet FOLLOW_expression_in_sum811 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_R2_in_sum813 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUM_in_sum821 = new BitSet(new ulong[]{0x0000000800000000UL});
    public static readonly BitSet FOLLOW_L3_in_sum823 = new BitSet(new ulong[]{0x0000400A88000000UL});
    public static readonly BitSet FOLLOW_sumControlled_in_sum825 = new BitSet(new ulong[]{0x0000042000000000UL});
    public static readonly BitSet FOLLOW_conditional_in_sum827 = new BitSet(new ulong[]{0x0000002000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_sum830 = new BitSet(new ulong[]{0x0000438A88000000UL});
    public static readonly BitSet FOLLOW_expression_in_sum832 = new BitSet(new ulong[]{0x0000001000000000UL});
    public static readonly BitSet FOLLOW_R3_in_sum834 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_sumControlled851 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L1_in_sumControlled858 = new BitSet(new ulong[]{0x0000400008000000UL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled860 = new BitSet(new ulong[]{0x0000000100000000UL});
    public static readonly BitSet FOLLOW_R1_in_sumControlled862 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L2_in_sumControlled868 = new BitSet(new ulong[]{0x0000400008000000UL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled870 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_R2_in_sumControlled872 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_L3_in_sumControlled878 = new BitSet(new ulong[]{0x0000400008000000UL});
    public static readonly BitSet FOLLOW_indexerElements_in_sumControlled880 = new BitSet(new ulong[]{0x0000001000000000UL});
    public static readonly BitSet FOLLOW_R3_in_sumControlled882 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_numberPlusMinus893 = new BitSet(new ulong[]{0x0000028000000000UL});
    public static readonly BitSet FOLLOW_set_in_numberPlusMinus899 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Ident_in_ident917 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_extraTokens_in_ident921 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_variable928 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STARS_in_pow946 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_definition_in_synpred23_GAMS741 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}