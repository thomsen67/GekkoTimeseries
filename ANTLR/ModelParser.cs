// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 Model.g 2019-10-09 14:11:25

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
public partial class ModelParser : Parser
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"LOG", 
		"EXP", 
		"FRML", 
		"AFTER", 
		"AFTER2", 
		"VAL", 
		"VARLIST", 
		"PLUS", 
		"MINUS", 
		"NEGATE", 
		"MULT", 
		"DIV", 
		"LB", 
		"RB", 
		"RP", 
		"DOT", 
		"TRUE", 
		"FALSE", 
		"ASTMODELBLOCK", 
		"ASTMODELBLOCKEND", 
		"ASTAFTER", 
		"ASTAFTER2", 
		"ASTASSIGNVAR", 
		"ASTINTEGER", 
		"ASTDOUBLE", 
		"ASTVARIABLE", 
		"ASTEXPRESSION", 
		"ASTFRMLCODE", 
		"ASTFUNCTION", 
		"ASTLAGFUNCTION", 
		"ASTLEFTSIDE", 
		"ASTSIMPLEFUNCTION", 
		"ASTVARIABLELAGLEAD", 
		"ASTLAGORLEAD", 
		"ASTFRML", 
		"ASTPOW", 
		"ASTVAL", 
		"ASTVARLIST", 
		"Modelblock", 
		"ModelblockEnd", 
		"Double", 
		"Integer", 
		"Ident", 
		"MOD", 
		"AssignVar", 
		"STARS", 
		"HAT", 
		"DIGIT", 
		"Exponent", 
		"A", 
		"Q", 
		"M", 
		"DATE", 
		"LETTER", 
		"WHITESPACE", 
		"NEWLINE", 
		"NEWLINE2", 
		"NEWLINE3", 
		"Comment", 
		"NESTED_ML_COMMENT", 
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
		"N", 
		"O", 
		"P", 
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
		"','", 
		"'['", 
		"']'", 
		"'$'", 
		"';'"
    };

    public const int ASTPOW = 39;
    public const int ASTMODELBLOCK = 22;
    public const int ASTVARIABLE = 29;
    public const int MOD = 47;
    public const int LETTER = 57;
    public const int LOG = 4;
    public const int AFTER2 = 8;
    public const int EOF = -1;
    public const int ASTINTEGER = 27;
    public const int ASTASSIGNVAR = 26;
    public const int T__93 = 93;
    public const int HAT = 50;
    public const int T__91 = 91;
    public const int T__92 = 92;
    public const int T__90 = 90;
    public const int EXP = 5;
    public const int Comment = 62;
    public const int ASTVAL = 40;
    public const int VAL = 9;
    public const int Double = 44;
    public const int D = 66;
    public const int E = 67;
    public const int F = 68;
    public const int G = 69;
    public const int A = 53;
    public const int B = 64;
    public const int C = 65;
    public const int L = 74;
    public const int M = 55;
    public const int NESTED_ML_COMMENT = 63;
    public const int N = 75;
    public const int O = 76;
    public const int H = 70;
    public const int I = 71;
    public const int NEWLINE2 = 60;
    public const int J = 72;
    public const int NEWLINE3 = 61;
    public const int K = 73;
    public const int U = 81;
    public const int T = 80;
    public const int WHITESPACE = 58;
    public const int W = 83;
    public const int V = 82;
    public const int Q = 54;
    public const int P = 77;
    public const int ASTFRML = 38;
    public const int S = 79;
    public const int MULT = 14;
    public const int R = 78;
    public const int T__87 = 87;
    public const int Y = 85;
    public const int T__89 = 89;
    public const int T__88 = 88;
    public const int X = 84;
    public const int Z = 86;
    public const int ASTAFTER2 = 25;
    public const int Ident = 46;
    public const int ASTEXPRESSION = 30;
    public const int VARLIST = 10;
    public const int ASTLAGFUNCTION = 33;
    public const int ASTLAGORLEAD = 37;
    public const int FALSE = 21;
    public const int Modelblock = 42;
    public const int ASTSIMPLEFUNCTION = 35;
    public const int RB = 17;
    public const int RP = 18;
    public const int ASTVARIABLELAGLEAD = 36;
    public const int ASTFUNCTION = 32;
    public const int ASTAFTER = 24;
    public const int AssignVar = 48;
    public const int Exponent = 52;
    public const int ASTMODELBLOCKEND = 23;
    public const int ModelblockEnd = 43;
    public const int ASTVARLIST = 41;
    public const int ASTFRMLCODE = 31;
    public const int PLUS = 11;
    public const int FRML = 6;
    public const int DIGIT = 51;
    public const int DOT = 19;
    public const int NEGATE = 13;
    public const int AFTER = 7;
    public const int MINUS = 12;
    public const int TRUE = 20;
    public const int ASTLEFTSIDE = 34;
    public const int NEWLINE = 59;
    public const int STARS = 49;
    public const int LB = 16;
    public const int ASTDOUBLE = 28;
    public const int DIV = 15;
    public const int DATE = 56;
    public const int Integer = 45;

    // delegates
    // delegators



        public ModelParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public ModelParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();
            this.state.ruleMemo = new Hashtable[69+1];
             
             
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
		get { return ModelParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "Model.g"; }
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
    // Model.g:130:1: extraTokens : ( LOG | EXP | FRML | AFTER | AFTER2 | VAL | VARLIST );
    public ModelParser.extraTokens_return extraTokens() // throws RecognitionException [1]
    {   
        ModelParser.extraTokens_return retval = new ModelParser.extraTokens_return();
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
            // Model.g:130:13: ( LOG | EXP | FRML | AFTER | AFTER2 | VAL | VARLIST )
            // Model.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set1 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= LOG && input.LA(1) <= VARLIST) ) 
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
    // Model.g:144:1: expr : ( expr2 )+ EOF ;
    public ModelParser.expr_return expr() // throws RecognitionException [1]
    {   
        ModelParser.expr_return retval = new ModelParser.expr_return();
        retval.Start = input.LT(1);
        int expr_StartIndex = input.Index();
        object root_0 = null;

        IToken EOF3 = null;
        ModelParser.expr2_return expr22 = default(ModelParser.expr2_return);


        object EOF3_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 2) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:144:6: ( ( expr2 )+ EOF )
            // Model.g:144:8: ( expr2 )+ EOF
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// Model.g:144:8: ( expr2 )+
            	int cnt1 = 0;
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( ((LA1_0 >= FRML && LA1_0 <= VAL) || (LA1_0 >= Modelblock && LA1_0 <= ModelblockEnd) || (LA1_0 >= 92 && LA1_0 <= 93)) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // Model.g:144:9: expr2
            			    {
            			    	PushFollow(FOLLOW_expr2_in_expr454);
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

            	EOF3=(IToken)Match(input,EOF,FOLLOW_EOF_in_expr458); if (state.failed) return retval;
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
    // Model.g:146:1: expr2 : ( frml | val | Modelblock -> ^( ASTMODELBLOCK Modelblock ) | ModelblockEnd -> ^( ASTMODELBLOCKEND ModelblockEnd ) | AFTER frmlEnding -> ^( ASTAFTER ) | AFTER2 frmlEnding -> ^( ASTAFTER2 ) | frmlEnding ->);
    public ModelParser.expr2_return expr2() // throws RecognitionException [1]
    {   
        ModelParser.expr2_return retval = new ModelParser.expr2_return();
        retval.Start = input.LT(1);
        int expr2_StartIndex = input.Index();
        object root_0 = null;

        IToken Modelblock6 = null;
        IToken ModelblockEnd7 = null;
        IToken AFTER8 = null;
        IToken AFTER210 = null;
        ModelParser.frml_return frml4 = default(ModelParser.frml_return);

        ModelParser.val_return val5 = default(ModelParser.val_return);

        ModelParser.frmlEnding_return frmlEnding9 = default(ModelParser.frmlEnding_return);

        ModelParser.frmlEnding_return frmlEnding11 = default(ModelParser.frmlEnding_return);

        ModelParser.frmlEnding_return frmlEnding12 = default(ModelParser.frmlEnding_return);


        object Modelblock6_tree=null;
        object ModelblockEnd7_tree=null;
        object AFTER8_tree=null;
        object AFTER210_tree=null;
        RewriteRuleTokenStream stream_AFTER2 = new RewriteRuleTokenStream(adaptor,"token AFTER2");
        RewriteRuleTokenStream stream_ModelblockEnd = new RewriteRuleTokenStream(adaptor,"token ModelblockEnd");
        RewriteRuleTokenStream stream_AFTER = new RewriteRuleTokenStream(adaptor,"token AFTER");
        RewriteRuleTokenStream stream_Modelblock = new RewriteRuleTokenStream(adaptor,"token Modelblock");
        RewriteRuleSubtreeStream stream_frmlEnding = new RewriteRuleSubtreeStream(adaptor,"rule frmlEnding");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 3) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:146:10: ( frml | val | Modelblock -> ^( ASTMODELBLOCK Modelblock ) | ModelblockEnd -> ^( ASTMODELBLOCKEND ModelblockEnd ) | AFTER frmlEnding -> ^( ASTAFTER ) | AFTER2 frmlEnding -> ^( ASTAFTER2 ) | frmlEnding ->)
            int alt2 = 7;
            switch ( input.LA(1) ) 
            {
            case FRML:
            	{
                alt2 = 1;
                }
                break;
            case VAL:
            	{
                alt2 = 2;
                }
                break;
            case Modelblock:
            	{
                alt2 = 3;
                }
                break;
            case ModelblockEnd:
            	{
                alt2 = 4;
                }
                break;
            case AFTER:
            	{
                alt2 = 5;
                }
                break;
            case AFTER2:
            	{
                alt2 = 6;
                }
                break;
            case 92:
            case 93:
            	{
                alt2 = 7;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d2s0 =
            	        new NoViableAltException("", 2, 0, input);

            	    throw nvae_d2s0;
            }

            switch (alt2) 
            {
                case 1 :
                    // Model.g:147:3: frml
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_frml_in_expr2478);
                    	frml4 = frml();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, frml4.Tree);

                    }
                    break;
                case 2 :
                    // Model.g:148:5: val
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_val_in_expr2484);
                    	val5 = val();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, val5.Tree);

                    }
                    break;
                case 3 :
                    // Model.g:149:5: Modelblock
                    {
                    	Modelblock6=(IToken)Match(input,Modelblock,FOLLOW_Modelblock_in_expr2490); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Modelblock.Add(Modelblock6);



                    	// AST REWRITE
                    	// elements:          Modelblock
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 149:16: -> ^( ASTMODELBLOCK Modelblock )
                    	{
                    	    // Model.g:149:19: ^( ASTMODELBLOCK Modelblock )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTMODELBLOCK, "ASTMODELBLOCK"), root_1);

                    	    adaptor.AddChild(root_1, stream_Modelblock.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 4 :
                    // Model.g:150:5: ModelblockEnd
                    {
                    	ModelblockEnd7=(IToken)Match(input,ModelblockEnd,FOLLOW_ModelblockEnd_in_expr2504); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_ModelblockEnd.Add(ModelblockEnd7);



                    	// AST REWRITE
                    	// elements:          ModelblockEnd
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 150:19: -> ^( ASTMODELBLOCKEND ModelblockEnd )
                    	{
                    	    // Model.g:150:22: ^( ASTMODELBLOCKEND ModelblockEnd )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTMODELBLOCKEND, "ASTMODELBLOCKEND"), root_1);

                    	    adaptor.AddChild(root_1, stream_ModelblockEnd.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 5 :
                    // Model.g:151:5: AFTER frmlEnding
                    {
                    	AFTER8=(IToken)Match(input,AFTER,FOLLOW_AFTER_in_expr2518); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_AFTER.Add(AFTER8);

                    	PushFollow(FOLLOW_frmlEnding_in_expr2520);
                    	frmlEnding9 = frmlEnding();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_frmlEnding.Add(frmlEnding9.Tree);


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
                    	// 151:22: -> ^( ASTAFTER )
                    	{
                    	    // Model.g:151:26: ^( ASTAFTER )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTAFTER, "ASTAFTER"), root_1);

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 6 :
                    // Model.g:152:5: AFTER2 frmlEnding
                    {
                    	AFTER210=(IToken)Match(input,AFTER2,FOLLOW_AFTER2_in_expr2533); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_AFTER2.Add(AFTER210);

                    	PushFollow(FOLLOW_frmlEnding_in_expr2535);
                    	frmlEnding11 = frmlEnding();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_frmlEnding.Add(frmlEnding11.Tree);


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
                    	// 152:23: -> ^( ASTAFTER2 )
                    	{
                    	    // Model.g:152:27: ^( ASTAFTER2 )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTAFTER2, "ASTAFTER2"), root_1);

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 7 :
                    // Model.g:153:5: frmlEnding
                    {
                    	PushFollow(FOLLOW_frmlEnding_in_expr2550);
                    	frmlEnding12 = frmlEnding();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_frmlEnding.Add(frmlEnding12.Tree);


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
                    	// 153:16: ->
                    	{
                    	    root_0 = null;
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
    // Model.g:156:1: frml : FRML code genrLeftSide '=' expression2 frmlEnding -> ^( ASTFRML code genrLeftSide expression2 ) ;
    public ModelParser.frml_return frml() // throws RecognitionException [1]
    {   
        ModelParser.frml_return retval = new ModelParser.frml_return();
        retval.Start = input.LT(1);
        int frml_StartIndex = input.Index();
        object root_0 = null;

        IToken FRML13 = null;
        IToken char_literal16 = null;
        ModelParser.code_return code14 = default(ModelParser.code_return);

        ModelParser.genrLeftSide_return genrLeftSide15 = default(ModelParser.genrLeftSide_return);

        ModelParser.expression2_return expression217 = default(ModelParser.expression2_return);

        ModelParser.frmlEnding_return frmlEnding18 = default(ModelParser.frmlEnding_return);


        object FRML13_tree=null;
        object char_literal16_tree=null;
        RewriteRuleTokenStream stream_FRML = new RewriteRuleTokenStream(adaptor,"token FRML");
        RewriteRuleTokenStream stream_87 = new RewriteRuleTokenStream(adaptor,"token 87");
        RewriteRuleSubtreeStream stream_frmlEnding = new RewriteRuleSubtreeStream(adaptor,"rule frmlEnding");
        RewriteRuleSubtreeStream stream_genrLeftSide = new RewriteRuleSubtreeStream(adaptor,"rule genrLeftSide");
        RewriteRuleSubtreeStream stream_expression2 = new RewriteRuleSubtreeStream(adaptor,"rule expression2");
        RewriteRuleSubtreeStream stream_code = new RewriteRuleSubtreeStream(adaptor,"rule code");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 4) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:156:9: ( FRML code genrLeftSide '=' expression2 frmlEnding -> ^( ASTFRML code genrLeftSide expression2 ) )
            // Model.g:156:11: FRML code genrLeftSide '=' expression2 frmlEnding
            {
            	FRML13=(IToken)Match(input,FRML,FOLLOW_FRML_in_frml583); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_FRML.Add(FRML13);

            	PushFollow(FOLLOW_code_in_frml585);
            	code14 = code();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_code.Add(code14.Tree);
            	PushFollow(FOLLOW_genrLeftSide_in_frml587);
            	genrLeftSide15 = genrLeftSide();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_genrLeftSide.Add(genrLeftSide15.Tree);
            	char_literal16=(IToken)Match(input,87,FOLLOW_87_in_frml589); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_87.Add(char_literal16);

            	PushFollow(FOLLOW_expression2_in_frml591);
            	expression217 = expression2();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression2.Add(expression217.Tree);
            	PushFollow(FOLLOW_frmlEnding_in_frml593);
            	frmlEnding18 = frmlEnding();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_frmlEnding.Add(frmlEnding18.Tree);
            	if ( (state.backtracking==0) )
            	{
            	  frmlItems.Add(input.ToString((IToken)retval.Start,input.LT(-1)));
            	}


            	// AST REWRITE
            	// elements:          expression2, genrLeftSide, code
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 159:3: -> ^( ASTFRML code genrLeftSide expression2 )
            	{
            	    // Model.g:159:6: ^( ASTFRML code genrLeftSide expression2 )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFRML, "ASTFRML"), root_1);

            	    adaptor.AddChild(root_1, stream_code.NextTree());
            	    adaptor.AddChild(root_1, stream_genrLeftSide.NextTree());
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
    // Model.g:162:1: genrLeftSide : genrLeftSide2 -> ^( ASTLEFTSIDE genrLeftSide2 ) ;
    public ModelParser.genrLeftSide_return genrLeftSide() // throws RecognitionException [1]
    {   
        ModelParser.genrLeftSide_return retval = new ModelParser.genrLeftSide_return();
        retval.Start = input.LT(1);
        int genrLeftSide_StartIndex = input.Index();
        object root_0 = null;

        ModelParser.genrLeftSide2_return genrLeftSide219 = default(ModelParser.genrLeftSide2_return);


        RewriteRuleSubtreeStream stream_genrLeftSide2 = new RewriteRuleSubtreeStream(adaptor,"rule genrLeftSide2");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 5) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:162:14: ( genrLeftSide2 -> ^( ASTLEFTSIDE genrLeftSide2 ) )
            // Model.g:162:16: genrLeftSide2
            {
            	PushFollow(FOLLOW_genrLeftSide2_in_genrLeftSide629);
            	genrLeftSide219 = genrLeftSide2();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_genrLeftSide2.Add(genrLeftSide219.Tree);


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
            	// 162:30: -> ^( ASTLEFTSIDE genrLeftSide2 )
            	{
            	    // Model.g:162:33: ^( ASTLEFTSIDE genrLeftSide2 )
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
    // Model.g:163:1: genrLeftSide2 : ( ident | simpleFunction ) ;
    public ModelParser.genrLeftSide2_return genrLeftSide2() // throws RecognitionException [1]
    {   
        ModelParser.genrLeftSide2_return retval = new ModelParser.genrLeftSide2_return();
        retval.Start = input.LT(1);
        int genrLeftSide2_StartIndex = input.Index();
        object root_0 = null;

        ModelParser.ident_return ident20 = default(ModelParser.ident_return);

        ModelParser.simpleFunction_return simpleFunction21 = default(ModelParser.simpleFunction_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 6) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:163:15: ( ( ident | simpleFunction ) )
            // Model.g:163:17: ( ident | simpleFunction )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// Model.g:163:17: ( ident | simpleFunction )
            	int alt3 = 2;
            	int LA3_0 = input.LA(1);

            	if ( (LA3_0 == Ident) )
            	{
            	    int LA3_1 = input.LA(2);

            	    if ( (LA3_1 == 87) )
            	    {
            	        alt3 = 1;
            	    }
            	    else if ( (LA3_1 == 88) )
            	    {
            	        alt3 = 2;
            	    }
            	    else 
            	    {
            	        if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	        NoViableAltException nvae_d3s1 =
            	            new NoViableAltException("", 3, 1, input);

            	        throw nvae_d3s1;
            	    }
            	}
            	else if ( ((LA3_0 >= LOG && LA3_0 <= VARLIST)) )
            	{
            	    int LA3_2 = input.LA(2);

            	    if ( (LA3_2 == 87) )
            	    {
            	        alt3 = 1;
            	    }
            	    else if ( (LA3_2 == 88) )
            	    {
            	        alt3 = 2;
            	    }
            	    else 
            	    {
            	        if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	        NoViableAltException nvae_d3s2 =
            	            new NoViableAltException("", 3, 2, input);

            	        throw nvae_d3s2;
            	    }
            	}
            	else 
            	{
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d3s0 =
            	        new NoViableAltException("", 3, 0, input);

            	    throw nvae_d3s0;
            	}
            	switch (alt3) 
            	{
            	    case 1 :
            	        // Model.g:163:18: ident
            	        {
            	        	PushFollow(FOLLOW_ident_in_genrLeftSide2645);
            	        	ident20 = ident();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, ident20.Tree);

            	        }
            	        break;
            	    case 2 :
            	        // Model.g:163:24: simpleFunction
            	        {
            	        	PushFollow(FOLLOW_simpleFunction_in_genrLeftSide2647);
            	        	simpleFunction21 = simpleFunction();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, simpleFunction21.Tree);

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
    // Model.g:164:1: code : ident -> ^( ASTFRMLCODE ident ) ;
    public ModelParser.code_return code() // throws RecognitionException [1]
    {   
        ModelParser.code_return retval = new ModelParser.code_return();
        retval.Start = input.LT(1);
        int code_StartIndex = input.Index();
        object root_0 = null;

        ModelParser.ident_return ident22 = default(ModelParser.ident_return);


        RewriteRuleSubtreeStream stream_ident = new RewriteRuleSubtreeStream(adaptor,"rule ident");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 7) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:164:6: ( ident -> ^( ASTFRMLCODE ident ) )
            // Model.g:164:8: ident
            {
            	PushFollow(FOLLOW_ident_in_code655);
            	ident22 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_ident.Add(ident22.Tree);


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
            	// 164:14: -> ^( ASTFRMLCODE ident )
            	{
            	    // Model.g:164:17: ^( ASTFRMLCODE ident )
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
    // Model.g:165:1: expression2 : expression -> ^( ASTEXPRESSION expression ) ;
    public ModelParser.expression2_return expression2() // throws RecognitionException [1]
    {   
        ModelParser.expression2_return retval = new ModelParser.expression2_return();
        retval.Start = input.LT(1);
        int expression2_StartIndex = input.Index();
        object root_0 = null;

        ModelParser.expression_return expression23 = default(ModelParser.expression_return);


        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 8) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:165:13: ( expression -> ^( ASTEXPRESSION expression ) )
            // Model.g:165:15: expression
            {
            	PushFollow(FOLLOW_expression_in_expression2670);
            	expression23 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression23.Tree);


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
            	// 165:26: -> ^( ASTEXPRESSION expression )
            	{
            	    // Model.g:165:29: ^( ASTEXPRESSION expression )
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
    // Model.g:167:1: number : ( Double | Integer ) ;
    public ModelParser.number_return number() // throws RecognitionException [1]
    {   
        ModelParser.number_return retval = new ModelParser.number_return();
        retval.Start = input.LT(1);
        int number_StartIndex = input.Index();
        object root_0 = null;

        IToken set24 = null;

        object set24_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 9) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:167:7: ( ( Double | Integer ) )
            // Model.g:167:9: ( Double | Integer )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set24 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= Double && input.LA(1) <= Integer) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set24));
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

    public class val_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "val"
    // Model.g:169:1: val : VAL Ident '=' ( MINUS )? number -> ^( ASTVAL Ident number ( MINUS )? ) ;
    public ModelParser.val_return val() // throws RecognitionException [1]
    {   
        ModelParser.val_return retval = new ModelParser.val_return();
        retval.Start = input.LT(1);
        int val_StartIndex = input.Index();
        object root_0 = null;

        IToken VAL25 = null;
        IToken Ident26 = null;
        IToken char_literal27 = null;
        IToken MINUS28 = null;
        ModelParser.number_return number29 = default(ModelParser.number_return);


        object VAL25_tree=null;
        object Ident26_tree=null;
        object char_literal27_tree=null;
        object MINUS28_tree=null;
        RewriteRuleTokenStream stream_VAL = new RewriteRuleTokenStream(adaptor,"token VAL");
        RewriteRuleTokenStream stream_Ident = new RewriteRuleTokenStream(adaptor,"token Ident");
        RewriteRuleTokenStream stream_MINUS = new RewriteRuleTokenStream(adaptor,"token MINUS");
        RewriteRuleTokenStream stream_87 = new RewriteRuleTokenStream(adaptor,"token 87");
        RewriteRuleSubtreeStream stream_number = new RewriteRuleSubtreeStream(adaptor,"rule number");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 10) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:169:5: ( VAL Ident '=' ( MINUS )? number -> ^( ASTVAL Ident number ( MINUS )? ) )
            // Model.g:169:7: VAL Ident '=' ( MINUS )? number
            {
            	VAL25=(IToken)Match(input,VAL,FOLLOW_VAL_in_val697); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_VAL.Add(VAL25);

            	Ident26=(IToken)Match(input,Ident,FOLLOW_Ident_in_val699); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_Ident.Add(Ident26);

            	char_literal27=(IToken)Match(input,87,FOLLOW_87_in_val701); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_87.Add(char_literal27);

            	// Model.g:169:21: ( MINUS )?
            	int alt4 = 2;
            	int LA4_0 = input.LA(1);

            	if ( (LA4_0 == MINUS) )
            	{
            	    alt4 = 1;
            	}
            	switch (alt4) 
            	{
            	    case 1 :
            	        // Model.g:0:0: MINUS
            	        {
            	        	MINUS28=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_val703); if (state.failed) return retval; 
            	        	if ( (state.backtracking==0) ) stream_MINUS.Add(MINUS28);


            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_number_in_val706);
            	number29 = number();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_number.Add(number29.Tree);


            	// AST REWRITE
            	// elements:          Ident, number, MINUS
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 169:35: -> ^( ASTVAL Ident number ( MINUS )? )
            	{
            	    // Model.g:169:38: ^( ASTVAL Ident number ( MINUS )? )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVAL, "ASTVAL"), root_1);

            	    adaptor.AddChild(root_1, stream_Ident.NextNode());
            	    adaptor.AddChild(root_1, stream_number.NextTree());
            	    // Model.g:169:60: ( MINUS )?
            	    if ( stream_MINUS.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_MINUS.NextNode());

            	    }
            	    stream_MINUS.Reset();

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
            	Memoize(input, 10, val_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "val"

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
    // Model.g:175:1: simpleFunction : ident '(' ident ')' -> ^( ASTSIMPLEFUNCTION ident ident ) ;
    public ModelParser.simpleFunction_return simpleFunction() // throws RecognitionException [1]
    {   
        ModelParser.simpleFunction_return retval = new ModelParser.simpleFunction_return();
        retval.Start = input.LT(1);
        int simpleFunction_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal31 = null;
        IToken char_literal33 = null;
        ModelParser.ident_return ident30 = default(ModelParser.ident_return);

        ModelParser.ident_return ident32 = default(ModelParser.ident_return);


        object char_literal31_tree=null;
        object char_literal33_tree=null;
        RewriteRuleTokenStream stream_RP = new RewriteRuleTokenStream(adaptor,"token RP");
        RewriteRuleTokenStream stream_88 = new RewriteRuleTokenStream(adaptor,"token 88");
        RewriteRuleSubtreeStream stream_ident = new RewriteRuleSubtreeStream(adaptor,"rule ident");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 11) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:176:2: ( ident '(' ident ')' -> ^( ASTSIMPLEFUNCTION ident ident ) )
            // Model.g:176:4: ident '(' ident ')'
            {
            	PushFollow(FOLLOW_ident_in_simpleFunction733);
            	ident30 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_ident.Add(ident30.Tree);
            	char_literal31=(IToken)Match(input,88,FOLLOW_88_in_simpleFunction735); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_88.Add(char_literal31);

            	PushFollow(FOLLOW_ident_in_simpleFunction737);
            	ident32 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_ident.Add(ident32.Tree);
            	char_literal33=(IToken)Match(input,RP,FOLLOW_RP_in_simpleFunction739); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_RP.Add(char_literal33);



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
            	// 176:24: -> ^( ASTSIMPLEFUNCTION ident ident )
            	{
            	    // Model.g:176:27: ^( ASTSIMPLEFUNCTION ident ident )
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
            	Memoize(input, 11, simpleFunction_StartIndex); 
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
    // Model.g:179:1: function : ident '(' ( expression ( ',' expression )* )? ')' -> ^( ASTFUNCTION ident expression ( expression )* ) ;
    public ModelParser.function_return function() // throws RecognitionException [1]
    {   
        ModelParser.function_return retval = new ModelParser.function_return();
        retval.Start = input.LT(1);
        int function_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal35 = null;
        IToken char_literal37 = null;
        IToken char_literal39 = null;
        ModelParser.ident_return ident34 = default(ModelParser.ident_return);

        ModelParser.expression_return expression36 = default(ModelParser.expression_return);

        ModelParser.expression_return expression38 = default(ModelParser.expression_return);


        object char_literal35_tree=null;
        object char_literal37_tree=null;
        object char_literal39_tree=null;
        RewriteRuleTokenStream stream_RP = new RewriteRuleTokenStream(adaptor,"token RP");
        RewriteRuleTokenStream stream_88 = new RewriteRuleTokenStream(adaptor,"token 88");
        RewriteRuleTokenStream stream_89 = new RewriteRuleTokenStream(adaptor,"token 89");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_ident = new RewriteRuleSubtreeStream(adaptor,"rule ident");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 12) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:179:10: ( ident '(' ( expression ( ',' expression )* )? ')' -> ^( ASTFUNCTION ident expression ( expression )* ) )
            // Model.g:179:12: ident '(' ( expression ( ',' expression )* )? ')'
            {
            	PushFollow(FOLLOW_ident_in_function760);
            	ident34 = ident();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_ident.Add(ident34.Tree);
            	char_literal35=(IToken)Match(input,88,FOLLOW_88_in_function762); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_88.Add(char_literal35);

            	// Model.g:179:22: ( expression ( ',' expression )* )?
            	int alt6 = 2;
            	alt6 = dfa6.Predict(input);
            	switch (alt6) 
            	{
            	    case 1 :
            	        // Model.g:179:24: expression ( ',' expression )*
            	        {
            	        	PushFollow(FOLLOW_expression_in_function766);
            	        	expression36 = expression();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_expression.Add(expression36.Tree);
            	        	// Model.g:179:35: ( ',' expression )*
            	        	do 
            	        	{
            	        	    int alt5 = 2;
            	        	    int LA5_0 = input.LA(1);

            	        	    if ( (LA5_0 == 89) )
            	        	    {
            	        	        alt5 = 1;
            	        	    }


            	        	    switch (alt5) 
            	        		{
            	        			case 1 :
            	        			    // Model.g:179:36: ',' expression
            	        			    {
            	        			    	char_literal37=(IToken)Match(input,89,FOLLOW_89_in_function769); if (state.failed) return retval; 
            	        			    	if ( (state.backtracking==0) ) stream_89.Add(char_literal37);

            	        			    	PushFollow(FOLLOW_expression_in_function771);
            	        			    	expression38 = expression();
            	        			    	state.followingStackPointer--;
            	        			    	if (state.failed) return retval;
            	        			    	if ( (state.backtracking==0) ) stream_expression.Add(expression38.Tree);

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

            	}

            	char_literal39=(IToken)Match(input,RP,FOLLOW_RP_in_function778); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_RP.Add(char_literal39);



            	// AST REWRITE
            	// elements:          expression, expression, ident
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 179:60: -> ^( ASTFUNCTION ident expression ( expression )* )
            	{
            	    // Model.g:179:63: ^( ASTFUNCTION ident expression ( expression )* )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION, "ASTFUNCTION"), root_1);

            	    adaptor.AddChild(root_1, stream_ident.NextTree());
            	    adaptor.AddChild(root_1, stream_expression.NextTree());
            	    // Model.g:179:94: ( expression )*
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
            	Memoize(input, 12, function_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "function"

    public class functionLogExp_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "functionLogExp"
    // Model.g:180:1: functionLogExp : logExp '(' ( expression ( ',' expression )* )? ')' -> ^( ASTFUNCTION logExp expression ( expression )* ) ;
    public ModelParser.functionLogExp_return functionLogExp() // throws RecognitionException [1]
    {   
        ModelParser.functionLogExp_return retval = new ModelParser.functionLogExp_return();
        retval.Start = input.LT(1);
        int functionLogExp_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal41 = null;
        IToken char_literal43 = null;
        IToken char_literal45 = null;
        ModelParser.logExp_return logExp40 = default(ModelParser.logExp_return);

        ModelParser.expression_return expression42 = default(ModelParser.expression_return);

        ModelParser.expression_return expression44 = default(ModelParser.expression_return);


        object char_literal41_tree=null;
        object char_literal43_tree=null;
        object char_literal45_tree=null;
        RewriteRuleTokenStream stream_RP = new RewriteRuleTokenStream(adaptor,"token RP");
        RewriteRuleTokenStream stream_88 = new RewriteRuleTokenStream(adaptor,"token 88");
        RewriteRuleTokenStream stream_89 = new RewriteRuleTokenStream(adaptor,"token 89");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_logExp = new RewriteRuleSubtreeStream(adaptor,"rule logExp");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 13) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:180:15: ( logExp '(' ( expression ( ',' expression )* )? ')' -> ^( ASTFUNCTION logExp expression ( expression )* ) )
            // Model.g:180:17: logExp '(' ( expression ( ',' expression )* )? ')'
            {
            	PushFollow(FOLLOW_logExp_in_functionLogExp798);
            	logExp40 = logExp();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_logExp.Add(logExp40.Tree);
            	char_literal41=(IToken)Match(input,88,FOLLOW_88_in_functionLogExp800); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_88.Add(char_literal41);

            	// Model.g:180:28: ( expression ( ',' expression )* )?
            	int alt8 = 2;
            	alt8 = dfa8.Predict(input);
            	switch (alt8) 
            	{
            	    case 1 :
            	        // Model.g:180:30: expression ( ',' expression )*
            	        {
            	        	PushFollow(FOLLOW_expression_in_functionLogExp804);
            	        	expression42 = expression();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) ) stream_expression.Add(expression42.Tree);
            	        	// Model.g:180:41: ( ',' expression )*
            	        	do 
            	        	{
            	        	    int alt7 = 2;
            	        	    int LA7_0 = input.LA(1);

            	        	    if ( (LA7_0 == 89) )
            	        	    {
            	        	        alt7 = 1;
            	        	    }


            	        	    switch (alt7) 
            	        		{
            	        			case 1 :
            	        			    // Model.g:180:42: ',' expression
            	        			    {
            	        			    	char_literal43=(IToken)Match(input,89,FOLLOW_89_in_functionLogExp807); if (state.failed) return retval; 
            	        			    	if ( (state.backtracking==0) ) stream_89.Add(char_literal43);

            	        			    	PushFollow(FOLLOW_expression_in_functionLogExp809);
            	        			    	expression44 = expression();
            	        			    	state.followingStackPointer--;
            	        			    	if (state.failed) return retval;
            	        			    	if ( (state.backtracking==0) ) stream_expression.Add(expression44.Tree);

            	        			    }
            	        			    break;

            	        			default:
            	        			    goto loop7;
            	        	    }
            	        	} while (true);

            	        	loop7:
            	        		;	// Stops C# compiler whining that label 'loop7' has no statements


            	        }
            	        break;

            	}

            	char_literal45=(IToken)Match(input,RP,FOLLOW_RP_in_functionLogExp816); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_RP.Add(char_literal45);



            	// AST REWRITE
            	// elements:          logExp, expression, expression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 180:66: -> ^( ASTFUNCTION logExp expression ( expression )* )
            	{
            	    // Model.g:180:69: ^( ASTFUNCTION logExp expression ( expression )* )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTFUNCTION, "ASTFUNCTION"), root_1);

            	    adaptor.AddChild(root_1, stream_logExp.NextTree());
            	    adaptor.AddChild(root_1, stream_expression.NextTree());
            	    // Model.g:180:101: ( expression )*
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
            	Memoize(input, 13, functionLogExp_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "functionLogExp"

    public class lagFunction_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "lagFunction"
    // Model.g:182:1: lagFunction : '(' expression ')' '(' numberPlusMinus ')' -> ^( ASTLAGFUNCTION expression numberPlusMinus ) ;
    public ModelParser.lagFunction_return lagFunction() // throws RecognitionException [1]
    {   
        ModelParser.lagFunction_return retval = new ModelParser.lagFunction_return();
        retval.Start = input.LT(1);
        int lagFunction_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal46 = null;
        IToken char_literal48 = null;
        IToken char_literal49 = null;
        IToken char_literal51 = null;
        ModelParser.expression_return expression47 = default(ModelParser.expression_return);

        ModelParser.numberPlusMinus_return numberPlusMinus50 = default(ModelParser.numberPlusMinus_return);


        object char_literal46_tree=null;
        object char_literal48_tree=null;
        object char_literal49_tree=null;
        object char_literal51_tree=null;
        RewriteRuleTokenStream stream_RP = new RewriteRuleTokenStream(adaptor,"token RP");
        RewriteRuleTokenStream stream_88 = new RewriteRuleTokenStream(adaptor,"token 88");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_numberPlusMinus = new RewriteRuleSubtreeStream(adaptor,"rule numberPlusMinus");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 14) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:182:13: ( '(' expression ')' '(' numberPlusMinus ')' -> ^( ASTLAGFUNCTION expression numberPlusMinus ) )
            // Model.g:182:15: '(' expression ')' '(' numberPlusMinus ')'
            {
            	char_literal46=(IToken)Match(input,88,FOLLOW_88_in_lagFunction841); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_88.Add(char_literal46);

            	PushFollow(FOLLOW_expression_in_lagFunction843);
            	expression47 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression47.Tree);
            	char_literal48=(IToken)Match(input,RP,FOLLOW_RP_in_lagFunction845); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_RP.Add(char_literal48);

            	char_literal49=(IToken)Match(input,88,FOLLOW_88_in_lagFunction847); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_88.Add(char_literal49);

            	PushFollow(FOLLOW_numberPlusMinus_in_lagFunction849);
            	numberPlusMinus50 = numberPlusMinus();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_numberPlusMinus.Add(numberPlusMinus50.Tree);
            	char_literal51=(IToken)Match(input,RP,FOLLOW_RP_in_lagFunction851); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_RP.Add(char_literal51);



            	// AST REWRITE
            	// elements:          expression, numberPlusMinus
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 182:58: -> ^( ASTLAGFUNCTION expression numberPlusMinus )
            	{
            	    // Model.g:182:61: ^( ASTLAGFUNCTION expression numberPlusMinus )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTLAGFUNCTION, "ASTLAGFUNCTION"), root_1);

            	    adaptor.AddChild(root_1, stream_expression.NextTree());
            	    adaptor.AddChild(root_1, stream_numberPlusMinus.NextTree());

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
            	Memoize(input, 14, lagFunction_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "lagFunction"

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
    // Model.g:184:1: expression : additiveExpression ;
    public ModelParser.expression_return expression() // throws RecognitionException [1]
    {   
        ModelParser.expression_return retval = new ModelParser.expression_return();
        retval.Start = input.LT(1);
        int expression_StartIndex = input.Index();
        object root_0 = null;

        ModelParser.additiveExpression_return additiveExpression52 = default(ModelParser.additiveExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 15) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:184:12: ( additiveExpression )
            // Model.g:186:4: additiveExpression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_additiveExpression_in_expression877);
            	additiveExpression52 = additiveExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression52.Tree);

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
    // Model.g:188:1: additiveExpression : multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* ;
    public ModelParser.additiveExpression_return additiveExpression() // throws RecognitionException [1]
    {   
        ModelParser.additiveExpression_return retval = new ModelParser.additiveExpression_return();
        retval.Start = input.LT(1);
        int additiveExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set54 = null;
        ModelParser.multiplicativeExpression_return multiplicativeExpression53 = default(ModelParser.multiplicativeExpression_return);

        ModelParser.multiplicativeExpression_return multiplicativeExpression55 = default(ModelParser.multiplicativeExpression_return);


        object set54_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 16) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:188:21: ( multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )* )
            // Model.g:188:23: multiplicativeExpression ( ( PLUS | MINUS ) multiplicativeExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression886);
            	multiplicativeExpression53 = multiplicativeExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression53.Tree);
            	// Model.g:188:48: ( ( PLUS | MINUS ) multiplicativeExpression )*
            	do 
            	{
            	    int alt9 = 2;
            	    int LA9_0 = input.LA(1);

            	    if ( ((LA9_0 >= PLUS && LA9_0 <= MINUS)) )
            	    {
            	        alt9 = 1;
            	    }


            	    switch (alt9) 
            		{
            			case 1 :
            			    // Model.g:188:50: ( PLUS | MINUS ) multiplicativeExpression
            			    {
            			    	set54=(IToken)input.LT(1);
            			    	set54 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= PLUS && input.LA(1) <= MINUS) ) 
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

            			    	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression897);
            			    	multiplicativeExpression55 = multiplicativeExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression55.Tree);

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
            	Memoize(input, 16, additiveExpression_StartIndex); 
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
    // Model.g:190:1: multiplicativeExpression : powerExpression ( ( MULT | DIV | MOD ) powerExpression )* ;
    public ModelParser.multiplicativeExpression_return multiplicativeExpression() // throws RecognitionException [1]
    {   
        ModelParser.multiplicativeExpression_return retval = new ModelParser.multiplicativeExpression_return();
        retval.Start = input.LT(1);
        int multiplicativeExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set57 = null;
        ModelParser.powerExpression_return powerExpression56 = default(ModelParser.powerExpression_return);

        ModelParser.powerExpression_return powerExpression58 = default(ModelParser.powerExpression_return);


        object set57_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 17) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:190:28: ( powerExpression ( ( MULT | DIV | MOD ) powerExpression )* )
            // Model.g:190:30: powerExpression ( ( MULT | DIV | MOD ) powerExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression910);
            	powerExpression56 = powerExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression56.Tree);
            	// Model.g:190:46: ( ( MULT | DIV | MOD ) powerExpression )*
            	do 
            	{
            	    int alt10 = 2;
            	    int LA10_0 = input.LA(1);

            	    if ( ((LA10_0 >= MULT && LA10_0 <= DIV) || LA10_0 == MOD) )
            	    {
            	        alt10 = 1;
            	    }


            	    switch (alt10) 
            		{
            			case 1 :
            			    // Model.g:190:48: ( MULT | DIV | MOD ) powerExpression
            			    {
            			    	set57=(IToken)input.LT(1);
            			    	set57 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= MULT && input.LA(1) <= DIV) || input.LA(1) == MOD ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set57), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression923);
            			    	powerExpression58 = powerExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, powerExpression58.Tree);

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
            	Memoize(input, 17, multiplicativeExpression_StartIndex); 
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
    // Model.g:192:1: powerExpression : unaryExpression ( pow unaryExpression )* ;
    public ModelParser.powerExpression_return powerExpression() // throws RecognitionException [1]
    {   
        ModelParser.powerExpression_return retval = new ModelParser.powerExpression_return();
        retval.Start = input.LT(1);
        int powerExpression_StartIndex = input.Index();
        object root_0 = null;

        ModelParser.unaryExpression_return unaryExpression59 = default(ModelParser.unaryExpression_return);

        ModelParser.pow_return pow60 = default(ModelParser.pow_return);

        ModelParser.unaryExpression_return unaryExpression61 = default(ModelParser.unaryExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 18) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:192:19: ( unaryExpression ( pow unaryExpression )* )
            // Model.g:192:21: unaryExpression ( pow unaryExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_unaryExpression_in_powerExpression936);
            	unaryExpression59 = unaryExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression59.Tree);
            	// Model.g:192:37: ( pow unaryExpression )*
            	do 
            	{
            	    int alt11 = 2;
            	    int LA11_0 = input.LA(1);

            	    if ( ((LA11_0 >= STARS && LA11_0 <= HAT)) )
            	    {
            	        alt11 = 1;
            	    }


            	    switch (alt11) 
            		{
            			case 1 :
            			    // Model.g:192:39: pow unaryExpression
            			    {
            			    	PushFollow(FOLLOW_pow_in_powerExpression940);
            			    	pow60 = pow();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot(pow60.Tree, root_0);
            			    	PushFollow(FOLLOW_unaryExpression_in_powerExpression943);
            			    	unaryExpression61 = unaryExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression61.Tree);

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
            	Memoize(input, 18, powerExpression_StartIndex); 
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
    // Model.g:194:1: unaryExpression : ( primaryExpression | MINUS primaryExpression -> ^( NEGATE primaryExpression ) );
    public ModelParser.unaryExpression_return unaryExpression() // throws RecognitionException [1]
    {   
        ModelParser.unaryExpression_return retval = new ModelParser.unaryExpression_return();
        retval.Start = input.LT(1);
        int unaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken MINUS63 = null;
        ModelParser.primaryExpression_return primaryExpression62 = default(ModelParser.primaryExpression_return);

        ModelParser.primaryExpression_return primaryExpression64 = default(ModelParser.primaryExpression_return);


        object MINUS63_tree=null;
        RewriteRuleTokenStream stream_MINUS = new RewriteRuleTokenStream(adaptor,"token MINUS");
        RewriteRuleSubtreeStream stream_primaryExpression = new RewriteRuleSubtreeStream(adaptor,"rule primaryExpression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 19) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:195:4: ( primaryExpression | MINUS primaryExpression -> ^( NEGATE primaryExpression ) )
            int alt12 = 2;
            int LA12_0 = input.LA(1);

            if ( ((LA12_0 >= LOG && LA12_0 <= VARLIST) || (LA12_0 >= Double && LA12_0 <= Ident) || LA12_0 == AssignVar || LA12_0 == 88) )
            {
                alt12 = 1;
            }
            else if ( (LA12_0 == MINUS) )
            {
                alt12 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d12s0 =
                    new NoViableAltException("", 12, 0, input);

                throw nvae_d12s0;
            }
            switch (alt12) 
            {
                case 1 :
                    // Model.g:195:6: primaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_primaryExpression_in_unaryExpression958);
                    	primaryExpression62 = primaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, primaryExpression62.Tree);

                    }
                    break;
                case 2 :
                    // Model.g:196:10: MINUS primaryExpression
                    {
                    	MINUS63=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_unaryExpression969); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_MINUS.Add(MINUS63);

                    	PushFollow(FOLLOW_primaryExpression_in_unaryExpression971);
                    	primaryExpression64 = primaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_primaryExpression.Add(primaryExpression64.Tree);


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
                    	// 196:34: -> ^( NEGATE primaryExpression )
                    	{
                    	    // Model.g:196:37: ^( NEGATE primaryExpression )
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
            	Memoize(input, 19, unaryExpression_StartIndex); 
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
    // Model.g:198:1: primaryExpression : ( '(' expression ')' | value );
    public ModelParser.primaryExpression_return primaryExpression() // throws RecognitionException [1]
    {   
        ModelParser.primaryExpression_return retval = new ModelParser.primaryExpression_return();
        retval.Start = input.LT(1);
        int primaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal65 = null;
        IToken char_literal67 = null;
        ModelParser.expression_return expression66 = default(ModelParser.expression_return);

        ModelParser.value_return value68 = default(ModelParser.value_return);


        object char_literal65_tree=null;
        object char_literal67_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 20) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:199:4: ( '(' expression ')' | value )
            int alt13 = 2;
            int LA13_0 = input.LA(1);

            if ( (LA13_0 == 88) )
            {
                alt13 = 1;
            }
            else if ( ((LA13_0 >= LOG && LA13_0 <= VARLIST) || (LA13_0 >= Double && LA13_0 <= Ident) || LA13_0 == AssignVar) )
            {
                alt13 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d13s0 =
                    new NoViableAltException("", 13, 0, input);

                throw nvae_d13s0;
            }
            switch (alt13) 
            {
                case 1 :
                    // Model.g:199:6: '(' expression ')'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal65=(IToken)Match(input,88,FOLLOW_88_in_primaryExpression998); if (state.failed) return retval;
                    	PushFollow(FOLLOW_expression_in_primaryExpression1001);
                    	expression66 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression66.Tree);
                    	char_literal67=(IToken)Match(input,RP,FOLLOW_RP_in_primaryExpression1003); if (state.failed) return retval;

                    }
                    break;
                case 2 :
                    // Model.g:200:6: value
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_value_in_primaryExpression1011);
                    	value68 = value();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, value68.Tree);

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
            	Memoize(input, 20, primaryExpression_StartIndex); 
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
    // Model.g:202:1: value : ( Integer -> ^( ASTINTEGER Integer ) | Double -> ^( ASTDOUBLE Double ) | functionLogExp | variableWithLagOrLead | function | assignVar );
    public ModelParser.value_return value() // throws RecognitionException [1]
    {   
        ModelParser.value_return retval = new ModelParser.value_return();
        retval.Start = input.LT(1);
        int value_StartIndex = input.Index();
        object root_0 = null;

        IToken Integer69 = null;
        IToken Double70 = null;
        ModelParser.functionLogExp_return functionLogExp71 = default(ModelParser.functionLogExp_return);

        ModelParser.variableWithLagOrLead_return variableWithLagOrLead72 = default(ModelParser.variableWithLagOrLead_return);

        ModelParser.function_return function73 = default(ModelParser.function_return);

        ModelParser.assignVar_return assignVar74 = default(ModelParser.assignVar_return);


        object Integer69_tree=null;
        object Double70_tree=null;
        RewriteRuleTokenStream stream_Double = new RewriteRuleTokenStream(adaptor,"token Double");
        RewriteRuleTokenStream stream_Integer = new RewriteRuleTokenStream(adaptor,"token Integer");

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 21) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:203:2: ( Integer -> ^( ASTINTEGER Integer ) | Double -> ^( ASTDOUBLE Double ) | functionLogExp | variableWithLagOrLead | function | assignVar )
            int alt14 = 6;
            alt14 = dfa14.Predict(input);
            switch (alt14) 
            {
                case 1 :
                    // Model.g:203:5: Integer
                    {
                    	Integer69=(IToken)Match(input,Integer,FOLLOW_Integer_in_value1025); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Integer.Add(Integer69);



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
                    	// 203:15: -> ^( ASTINTEGER Integer )
                    	{
                    	    // Model.g:203:18: ^( ASTINTEGER Integer )
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
                    // Model.g:204:4: Double
                    {
                    	Double70=(IToken)Match(input,Double,FOLLOW_Double_in_value1040); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Double.Add(Double70);



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
                    	// 204:15: -> ^( ASTDOUBLE Double )
                    	{
                    	    // Model.g:204:18: ^( ASTDOUBLE Double )
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
                    // Model.g:205:4: functionLogExp
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_functionLogExp_in_value1057);
                    	functionLogExp71 = functionLogExp();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, functionLogExp71.Tree);

                    }
                    break;
                case 4 :
                    // Model.g:206:4: variableWithLagOrLead
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variableWithLagOrLead_in_value1081);
                    	variableWithLagOrLead72 = variableWithLagOrLead();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variableWithLagOrLead72.Tree);

                    }
                    break;
                case 5 :
                    // Model.g:207:4: function
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_function_in_value1093);
                    	function73 = function();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, function73.Tree);

                    }
                    break;
                case 6 :
                    // Model.g:208:6: assignVar
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_assignVar_in_value1101);
                    	assignVar74 = assignVar();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, assignVar74.Tree);

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

    public class assignVar_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "assignVar"
    // Model.g:211:1: assignVar : AssignVar -> ^( ASTASSIGNVAR AssignVar ) ;
    public ModelParser.assignVar_return assignVar() // throws RecognitionException [1]
    {   
        ModelParser.assignVar_return retval = new ModelParser.assignVar_return();
        retval.Start = input.LT(1);
        int assignVar_StartIndex = input.Index();
        object root_0 = null;

        IToken AssignVar75 = null;

        object AssignVar75_tree=null;
        RewriteRuleTokenStream stream_AssignVar = new RewriteRuleTokenStream(adaptor,"token AssignVar");

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 22) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:211:11: ( AssignVar -> ^( ASTASSIGNVAR AssignVar ) )
            // Model.g:211:13: AssignVar
            {
            	AssignVar75=(IToken)Match(input,AssignVar,FOLLOW_AssignVar_in_assignVar1112); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_AssignVar.Add(AssignVar75);



            	// AST REWRITE
            	// elements:          AssignVar
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 211:23: -> ^( ASTASSIGNVAR AssignVar )
            	{
            	    // Model.g:211:26: ^( ASTASSIGNVAR AssignVar )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTASSIGNVAR, "ASTASSIGNVAR"), root_1);

            	    adaptor.AddChild(root_1, stream_AssignVar.NextNode());

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
            	Memoize(input, 22, assignVar_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "assignVar"

    public class variableWithLagOrLead_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variableWithLagOrLead"
    // Model.g:213:1: variableWithLagOrLead : ( ident -> ^( ASTVARIABLE ident FALSE ) | ident '(' numberPlusMinus ')' -> ^( ASTVARIABLELAGLEAD ident numberPlusMinus FALSE ) | ident '[' numberPlusMinus ']' -> ^( ASTVARIABLELAGLEAD ident numberPlusMinus FALSE ) );
    public ModelParser.variableWithLagOrLead_return variableWithLagOrLead() // throws RecognitionException [1]
    {   
        ModelParser.variableWithLagOrLead_return retval = new ModelParser.variableWithLagOrLead_return();
        retval.Start = input.LT(1);
        int variableWithLagOrLead_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal78 = null;
        IToken char_literal80 = null;
        IToken char_literal82 = null;
        IToken char_literal84 = null;
        ModelParser.ident_return ident76 = default(ModelParser.ident_return);

        ModelParser.ident_return ident77 = default(ModelParser.ident_return);

        ModelParser.numberPlusMinus_return numberPlusMinus79 = default(ModelParser.numberPlusMinus_return);

        ModelParser.ident_return ident81 = default(ModelParser.ident_return);

        ModelParser.numberPlusMinus_return numberPlusMinus83 = default(ModelParser.numberPlusMinus_return);


        object char_literal78_tree=null;
        object char_literal80_tree=null;
        object char_literal82_tree=null;
        object char_literal84_tree=null;
        RewriteRuleTokenStream stream_91 = new RewriteRuleTokenStream(adaptor,"token 91");
        RewriteRuleTokenStream stream_90 = new RewriteRuleTokenStream(adaptor,"token 90");
        RewriteRuleTokenStream stream_RP = new RewriteRuleTokenStream(adaptor,"token RP");
        RewriteRuleTokenStream stream_88 = new RewriteRuleTokenStream(adaptor,"token 88");
        RewriteRuleSubtreeStream stream_ident = new RewriteRuleSubtreeStream(adaptor,"rule ident");
        RewriteRuleSubtreeStream stream_numberPlusMinus = new RewriteRuleSubtreeStream(adaptor,"rule numberPlusMinus");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 23) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:214:4: ( ident -> ^( ASTVARIABLE ident FALSE ) | ident '(' numberPlusMinus ')' -> ^( ASTVARIABLELAGLEAD ident numberPlusMinus FALSE ) | ident '[' numberPlusMinus ']' -> ^( ASTVARIABLELAGLEAD ident numberPlusMinus FALSE ) )
            int alt15 = 3;
            alt15 = dfa15.Predict(input);
            switch (alt15) 
            {
                case 1 :
                    // Model.g:214:6: ident
                    {
                    	PushFollow(FOLLOW_ident_in_variableWithLagOrLead1133);
                    	ident76 = ident();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_ident.Add(ident76.Tree);


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
                    	// 214:13: -> ^( ASTVARIABLE ident FALSE )
                    	{
                    	    // Model.g:214:16: ^( ASTVARIABLE ident FALSE )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARIABLE, "ASTVARIABLE"), root_1);

                    	    adaptor.AddChild(root_1, stream_ident.NextTree());
                    	    adaptor.AddChild(root_1, (object)adaptor.Create(FALSE, "FALSE"));

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // Model.g:215:8: ident '(' numberPlusMinus ')'
                    {
                    	PushFollow(FOLLOW_ident_in_variableWithLagOrLead1153);
                    	ident77 = ident();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_ident.Add(ident77.Tree);
                    	char_literal78=(IToken)Match(input,88,FOLLOW_88_in_variableWithLagOrLead1155); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_88.Add(char_literal78);

                    	PushFollow(FOLLOW_numberPlusMinus_in_variableWithLagOrLead1157);
                    	numberPlusMinus79 = numberPlusMinus();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_numberPlusMinus.Add(numberPlusMinus79.Tree);
                    	char_literal80=(IToken)Match(input,RP,FOLLOW_RP_in_variableWithLagOrLead1159); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_RP.Add(char_literal80);



                    	// AST REWRITE
                    	// elements:          ident, numberPlusMinus
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 215:38: -> ^( ASTVARIABLELAGLEAD ident numberPlusMinus FALSE )
                    	{
                    	    // Model.g:215:41: ^( ASTVARIABLELAGLEAD ident numberPlusMinus FALSE )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARIABLELAGLEAD, "ASTVARIABLELAGLEAD"), root_1);

                    	    adaptor.AddChild(root_1, stream_ident.NextTree());
                    	    adaptor.AddChild(root_1, stream_numberPlusMinus.NextTree());
                    	    adaptor.AddChild(root_1, (object)adaptor.Create(FALSE, "FALSE"));

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 3 :
                    // Model.g:216:8: ident '[' numberPlusMinus ']'
                    {
                    	PushFollow(FOLLOW_ident_in_variableWithLagOrLead1181);
                    	ident81 = ident();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_ident.Add(ident81.Tree);
                    	char_literal82=(IToken)Match(input,90,FOLLOW_90_in_variableWithLagOrLead1183); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_90.Add(char_literal82);

                    	PushFollow(FOLLOW_numberPlusMinus_in_variableWithLagOrLead1185);
                    	numberPlusMinus83 = numberPlusMinus();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_numberPlusMinus.Add(numberPlusMinus83.Tree);
                    	char_literal84=(IToken)Match(input,91,FOLLOW_91_in_variableWithLagOrLead1187); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_91.Add(char_literal84);



                    	// AST REWRITE
                    	// elements:          numberPlusMinus, ident
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 216:38: -> ^( ASTVARIABLELAGLEAD ident numberPlusMinus FALSE )
                    	{
                    	    // Model.g:216:41: ^( ASTVARIABLELAGLEAD ident numberPlusMinus FALSE )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ASTVARIABLELAGLEAD, "ASTVARIABLELAGLEAD"), root_1);

                    	    adaptor.AddChild(root_1, stream_ident.NextTree());
                    	    adaptor.AddChild(root_1, stream_numberPlusMinus.NextTree());
                    	    adaptor.AddChild(root_1, (object)adaptor.Create(FALSE, "FALSE"));

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
            	Memoize(input, 23, variableWithLagOrLead_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "variableWithLagOrLead"

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
    // Model.g:219:1: numberPlusMinus : ( MINUS | PLUS ) ( Integer | Double ) ;
    public ModelParser.numberPlusMinus_return numberPlusMinus() // throws RecognitionException [1]
    {   
        ModelParser.numberPlusMinus_return retval = new ModelParser.numberPlusMinus_return();
        retval.Start = input.LT(1);
        int numberPlusMinus_StartIndex = input.Index();
        object root_0 = null;

        IToken set85 = null;
        IToken set86 = null;

        object set85_tree=null;
        object set86_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 24) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:219:18: ( ( MINUS | PLUS ) ( Integer | Double ) )
            // Model.g:219:21: ( MINUS | PLUS ) ( Integer | Double )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set85 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= PLUS && input.LA(1) <= MINUS) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set85));
            	    state.errorRecovery = false;state.failed = false;
            	}
            	else 
            	{
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}

            	set86 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= Double && input.LA(1) <= Integer) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set86));
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
            	Memoize(input, 24, numberPlusMinus_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "numberPlusMinus"

    public class logExp_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "logExp"
    // Model.g:221:1: logExp : ( LOG | EXP );
    public ModelParser.logExp_return logExp() // throws RecognitionException [1]
    {   
        ModelParser.logExp_return retval = new ModelParser.logExp_return();
        retval.Start = input.LT(1);
        int logExp_StartIndex = input.Index();
        object root_0 = null;

        IToken set87 = null;

        object set87_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 25) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:221:7: ( LOG | EXP )
            // Model.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set87 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= LOG && input.LA(1) <= EXP) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set87));
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
            	Memoize(input, 25, logExp_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "logExp"

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
    // Model.g:223:1: ident : ( Ident | extraTokens );
    public ModelParser.ident_return ident() // throws RecognitionException [1]
    {   
        ModelParser.ident_return retval = new ModelParser.ident_return();
        retval.Start = input.LT(1);
        int ident_StartIndex = input.Index();
        object root_0 = null;

        IToken Ident88 = null;
        ModelParser.extraTokens_return extraTokens89 = default(ModelParser.extraTokens_return);


        object Ident88_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 26) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:223:9: ( Ident | extraTokens )
            int alt16 = 2;
            int LA16_0 = input.LA(1);

            if ( (LA16_0 == Ident) )
            {
                alt16 = 1;
            }
            else if ( ((LA16_0 >= LOG && LA16_0 <= VARLIST)) )
            {
                alt16 = 2;
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
                    // Model.g:223:12: Ident
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Ident88=(IToken)Match(input,Ident,FOLLOW_Ident_in_ident1256); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Ident88_tree = (object)adaptor.Create(Ident88);
                    		adaptor.AddChild(root_0, Ident88_tree);
                    	}

                    }
                    break;
                case 2 :
                    // Model.g:223:20: extraTokens
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_extraTokens_in_ident1260);
                    	extraTokens89 = extraTokens();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, extraTokens89.Tree);

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

    public class frmlEnding_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "frmlEnding"
    // Model.g:227:1: frmlEnding : ( '$' | ';' );
    public ModelParser.frmlEnding_return frmlEnding() // throws RecognitionException [1]
    {   
        ModelParser.frmlEnding_return retval = new ModelParser.frmlEnding_return();
        retval.Start = input.LT(1);
        int frmlEnding_StartIndex = input.Index();
        object root_0 = null;

        IToken set90 = null;

        object set90_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 27) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:227:12: ( '$' | ';' )
            // Model.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set90 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= 92 && input.LA(1) <= 93) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set90));
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
            	Memoize(input, 27, frmlEnding_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "frmlEnding"

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
    // Model.g:235:1: pow : ( STARS -> ASTPOW | HAT -> ASTPOW );
    public ModelParser.pow_return pow() // throws RecognitionException [1]
    {   
        ModelParser.pow_return retval = new ModelParser.pow_return();
        retval.Start = input.LT(1);
        int pow_StartIndex = input.Index();
        object root_0 = null;

        IToken STARS91 = null;
        IToken HAT92 = null;

        object STARS91_tree=null;
        object HAT92_tree=null;
        RewriteRuleTokenStream stream_HAT = new RewriteRuleTokenStream(adaptor,"token HAT");
        RewriteRuleTokenStream stream_STARS = new RewriteRuleTokenStream(adaptor,"token STARS");

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 28) ) 
    	    {
    	    	return retval; 
    	    }
            // Model.g:235:9: ( STARS -> ASTPOW | HAT -> ASTPOW )
            int alt17 = 2;
            int LA17_0 = input.LA(1);

            if ( (LA17_0 == STARS) )
            {
                alt17 = 1;
            }
            else if ( (LA17_0 == HAT) )
            {
                alt17 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d17s0 =
                    new NoViableAltException("", 17, 0, input);

                throw nvae_d17s0;
            }
            switch (alt17) 
            {
                case 1 :
                    // Model.g:235:17: STARS
                    {
                    	STARS91=(IToken)Match(input,STARS,FOLLOW_STARS_in_pow1356); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_STARS.Add(STARS91);



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
                    	// 235:24: -> ASTPOW
                    	{
                    	    adaptor.AddChild(root_0, (object)adaptor.Create(ASTPOW, "ASTPOW"));

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // Model.g:236:17: HAT
                    {
                    	HAT92=(IToken)Match(input,HAT,FOLLOW_HAT_in_pow1379); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_HAT.Add(HAT92);



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
                    	// 236:24: -> ASTPOW
                    	{
                    	    adaptor.AddChild(root_0, (object)adaptor.Create(ASTPOW, "ASTPOW"));

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
            	Memoize(input, 28, pow_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "pow"

    // $ANTLR start "synpred31_Model"
    public void synpred31_Model_fragment() {
        // Model.g:205:4: ( functionLogExp )
        // Model.g:205:4: functionLogExp
        {
        	PushFollow(FOLLOW_functionLogExp_in_synpred31_Model1057);
        	functionLogExp();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred31_Model"

    // $ANTLR start "synpred32_Model"
    public void synpred32_Model_fragment() {
        // Model.g:206:4: ( variableWithLagOrLead )
        // Model.g:206:4: variableWithLagOrLead
        {
        	PushFollow(FOLLOW_variableWithLagOrLead_in_synpred32_Model1081);
        	variableWithLagOrLead();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred32_Model"

    // $ANTLR start "synpred33_Model"
    public void synpred33_Model_fragment() {
        // Model.g:207:4: ( function )
        // Model.g:207:4: function
        {
        	PushFollow(FOLLOW_function_in_synpred33_Model1093);
        	function();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred33_Model"

    // Delegated rules

   	public bool synpred32_Model() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred32_Model_fragment(); // can never throw exception
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
   	public bool synpred31_Model() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred31_Model_fragment(); // can never throw exception
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
   	public bool synpred33_Model() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred33_Model_fragment(); // can never throw exception
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


   	protected DFA6 dfa6;
   	protected DFA8 dfa8;
   	protected DFA14 dfa14;
   	protected DFA15 dfa15;
	private void InitializeCyclicDFAs()
	{
    	this.dfa6 = new DFA6(this);
    	this.dfa8 = new DFA8(this);
    	this.dfa14 = new DFA14(this);
    	this.dfa15 = new DFA15(this);


	    this.dfa14.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA14_SpecialStateTransition);

	}

    const string DFA6_eotS =
        "\x0a\uffff";
    const string DFA6_eofS =
        "\x0a\uffff";
    const string DFA6_minS =
        "\x01\x04\x09\uffff";
    const string DFA6_maxS =
        "\x01\x58\x09\uffff";
    const string DFA6_acceptS =
        "\x01\uffff\x01\x01\x07\uffff\x01\x02";
    const string DFA6_specialS =
        "\x0a\uffff}>";
    static readonly string[] DFA6_transitionS = {
            "\x07\x01\x01\uffff\x01\x01\x05\uffff\x01\x09\x19\uffff\x03"+
            "\x01\x01\uffff\x01\x01\x27\uffff\x01\x01",
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
            get { return "179:22: ( expression ( ',' expression )* )?"; }
        }

    }

    const string DFA8_eotS =
        "\x0a\uffff";
    const string DFA8_eofS =
        "\x0a\uffff";
    const string DFA8_minS =
        "\x01\x04\x09\uffff";
    const string DFA8_maxS =
        "\x01\x58\x09\uffff";
    const string DFA8_acceptS =
        "\x01\uffff\x01\x01\x07\uffff\x01\x02";
    const string DFA8_specialS =
        "\x0a\uffff}>";
    static readonly string[] DFA8_transitionS = {
            "\x07\x01\x01\uffff\x01\x01\x05\uffff\x01\x09\x19\uffff\x03"+
            "\x01\x01\uffff\x01\x01\x27\uffff\x01\x01",
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
            get { return "180:28: ( expression ( ',' expression )* )?"; }
        }

    }

    const string DFA14_eotS =
        "\x27\uffff";
    const string DFA14_eofS =
        "\x03\uffff\x03\x08\x21\uffff";
    const string DFA14_minS =
        "\x01\x04\x02\uffff\x03\x0b\x01\uffff\x01\x00\x12\uffff\x01\x00"+
        "\x08\uffff\x01\x00\x03\uffff";
    const string DFA14_maxS =
        "\x01\x30\x02\uffff\x03\x5d\x01\uffff\x01\x00\x12\uffff\x01\x00"+
        "\x08\uffff\x01\x00\x03\uffff";
    const string DFA14_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x03\uffff\x01\x06\x01\uffff\x01\x04"+
        "\x1c\uffff\x01\x03\x01\x05";
    const string DFA14_specialS =
        "\x07\uffff\x01\x00\x12\uffff\x01\x01\x08\uffff\x01\x02\x03\uffff}>";
    static readonly string[] DFA14_transitionS = {
            "\x02\x03\x05\x05\x21\uffff\x01\x02\x01\x01\x01\x04\x01\uffff"+
            "\x01\x06",
            "",
            "",
            "\x02\x08\x01\uffff\x02\x08\x02\uffff\x01\x08\x1c\uffff\x01"+
            "\x08\x01\uffff\x02\x08\x25\uffff\x01\x07\x02\x08\x01\uffff\x02"+
            "\x08",
            "\x02\x08\x01\uffff\x02\x08\x02\uffff\x01\x08\x1c\uffff\x01"+
            "\x08\x01\uffff\x02\x08\x25\uffff\x01\x1a\x02\x08\x01\uffff\x02"+
            "\x08",
            "\x02\x08\x01\uffff\x02\x08\x02\uffff\x01\x08\x1c\uffff\x01"+
            "\x08\x01\uffff\x02\x08\x25\uffff\x01\x23\x02\x08\x01\uffff\x02"+
            "\x08",
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
            get { return "202:1: value : ( Integer -> ^( ASTINTEGER Integer ) | Double -> ^( ASTDOUBLE Double ) | functionLogExp | variableWithLagOrLead | function | assignVar );"; }
        }

    }


    protected internal int DFA14_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA14_7 = input.LA(1);

                   	 
                   	int index14_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred31_Model()) ) { s = 37; }

                   	else if ( (synpred32_Model()) ) { s = 8; }

                   	else if ( (synpred33_Model()) ) { s = 38; }

                   	 
                   	input.Seek(index14_7);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA14_26 = input.LA(1);

                   	 
                   	int index14_26 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_Model()) ) { s = 8; }

                   	else if ( (synpred33_Model()) ) { s = 38; }

                   	 
                   	input.Seek(index14_26);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA14_35 = input.LA(1);

                   	 
                   	int index14_35 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred32_Model()) ) { s = 8; }

                   	else if ( (synpred33_Model()) ) { s = 38; }

                   	 
                   	input.Seek(index14_35);
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
        "\x17\uffff";
    const string DFA15_eofS =
        "\x01\uffff\x02\x05\x14\uffff";
    const string DFA15_minS =
        "\x01\x04\x02\x0b\x14\uffff";
    const string DFA15_maxS =
        "\x01\x2e\x02\x5d\x14\uffff";
    const string DFA15_acceptS =
        "\x03\uffff\x01\x02\x01\x03\x01\x01\x11\uffff";
    const string DFA15_specialS =
        "\x17\uffff}>";
    static readonly string[] DFA15_transitionS = {
            "\x07\x02\x23\uffff\x01\x01",
            "\x02\x05\x01\uffff\x02\x05\x02\uffff\x01\x05\x1c\uffff\x01"+
            "\x05\x01\uffff\x02\x05\x25\uffff\x01\x03\x01\x05\x01\x04\x01"+
            "\uffff\x02\x05",
            "\x02\x05\x01\uffff\x02\x05\x02\uffff\x01\x05\x1c\uffff\x01"+
            "\x05\x01\uffff\x02\x05\x25\uffff\x01\x03\x01\x05\x01\x04\x01"+
            "\uffff\x02\x05",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
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
            get { return "213:1: variableWithLagOrLead : ( ident -> ^( ASTVARIABLE ident FALSE ) | ident '(' numberPlusMinus ')' -> ^( ASTVARIABLELAGLEAD ident numberPlusMinus FALSE ) | ident '[' numberPlusMinus ']' -> ^( ASTVARIABLELAGLEAD ident numberPlusMinus FALSE ) );"; }
        }

    }

 

    public static readonly BitSet FOLLOW_set_in_extraTokens0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr2_in_expr454 = new BitSet(new ulong[]{0x00000C00000003C0UL,0x0000000030000000UL});
    public static readonly BitSet FOLLOW_EOF_in_expr458 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_frml_in_expr2478 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_val_in_expr2484 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Modelblock_in_expr2490 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ModelblockEnd_in_expr2504 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_AFTER_in_expr2518 = new BitSet(new ulong[]{0x00000C00000003C0UL,0x0000000030000000UL});
    public static readonly BitSet FOLLOW_frmlEnding_in_expr2520 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_AFTER2_in_expr2533 = new BitSet(new ulong[]{0x00000C00000003C0UL,0x0000000030000000UL});
    public static readonly BitSet FOLLOW_frmlEnding_in_expr2535 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_frmlEnding_in_expr2550 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FRML_in_frml583 = new BitSet(new ulong[]{0x00004000000007F0UL});
    public static readonly BitSet FOLLOW_code_in_frml585 = new BitSet(new ulong[]{0x00004000000007F0UL});
    public static readonly BitSet FOLLOW_genrLeftSide_in_frml587 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000800000UL});
    public static readonly BitSet FOLLOW_87_in_frml589 = new BitSet(new ulong[]{0x00017000000017F0UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_expression2_in_frml591 = new BitSet(new ulong[]{0x00000C00000003C0UL,0x0000000030000000UL});
    public static readonly BitSet FOLLOW_frmlEnding_in_frml593 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_genrLeftSide2_in_genrLeftSide629 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_genrLeftSide2645 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_simpleFunction_in_genrLeftSide2647 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_code655 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_expression2670 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_number685 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_VAL_in_val697 = new BitSet(new ulong[]{0x0000400000000000UL});
    public static readonly BitSet FOLLOW_Ident_in_val699 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000800000UL});
    public static readonly BitSet FOLLOW_87_in_val701 = new BitSet(new ulong[]{0x0000300000001000UL});
    public static readonly BitSet FOLLOW_MINUS_in_val703 = new BitSet(new ulong[]{0x0000300000001000UL});
    public static readonly BitSet FOLLOW_number_in_val706 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_simpleFunction733 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_88_in_simpleFunction735 = new BitSet(new ulong[]{0x00004000000007F0UL});
    public static readonly BitSet FOLLOW_ident_in_simpleFunction737 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_RP_in_simpleFunction739 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_function760 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_88_in_function762 = new BitSet(new ulong[]{0x00017000000417F0UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_expression_in_function766 = new BitSet(new ulong[]{0x0000000000040000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_89_in_function769 = new BitSet(new ulong[]{0x00017000000017F0UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_expression_in_function771 = new BitSet(new ulong[]{0x0000000000040000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_RP_in_function778 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_logExp_in_functionLogExp798 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_88_in_functionLogExp800 = new BitSet(new ulong[]{0x00017000000417F0UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_expression_in_functionLogExp804 = new BitSet(new ulong[]{0x0000000000040000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_89_in_functionLogExp807 = new BitSet(new ulong[]{0x00017000000017F0UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_expression_in_functionLogExp809 = new BitSet(new ulong[]{0x0000000000040000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_RP_in_functionLogExp816 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_88_in_lagFunction841 = new BitSet(new ulong[]{0x00017000000017F0UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_expression_in_lagFunction843 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_RP_in_lagFunction845 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_88_in_lagFunction847 = new BitSet(new ulong[]{0x0000000000001800UL});
    public static readonly BitSet FOLLOW_numberPlusMinus_in_lagFunction849 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_RP_in_lagFunction851 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_expression877 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression886 = new BitSet(new ulong[]{0x0000000000001802UL});
    public static readonly BitSet FOLLOW_set_in_additiveExpression890 = new BitSet(new ulong[]{0x00017000000017F0UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression897 = new BitSet(new ulong[]{0x0000000000001802UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression910 = new BitSet(new ulong[]{0x000080000000C002UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpression914 = new BitSet(new ulong[]{0x00017000000017F0UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression923 = new BitSet(new ulong[]{0x000080000000C002UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression936 = new BitSet(new ulong[]{0x0006000000000002UL});
    public static readonly BitSet FOLLOW_pow_in_powerExpression940 = new BitSet(new ulong[]{0x00017000000017F0UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression943 = new BitSet(new ulong[]{0x0006000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_unaryExpression958 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MINUS_in_unaryExpression969 = new BitSet(new ulong[]{0x00017000000007F0UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_unaryExpression971 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_88_in_primaryExpression998 = new BitSet(new ulong[]{0x00017000000017F0UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression1001 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_RP_in_primaryExpression1003 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_value_in_primaryExpression1011 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Integer_in_value1025 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Double_in_value1040 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionLogExp_in_value1057 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithLagOrLead_in_value1081 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_value1093 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_assignVar_in_value1101 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_AssignVar_in_assignVar1112 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_variableWithLagOrLead1133 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_variableWithLagOrLead1153 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000001000000UL});
    public static readonly BitSet FOLLOW_88_in_variableWithLagOrLead1155 = new BitSet(new ulong[]{0x0000000000001800UL});
    public static readonly BitSet FOLLOW_numberPlusMinus_in_variableWithLagOrLead1157 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_RP_in_variableWithLagOrLead1159 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ident_in_variableWithLagOrLead1181 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_90_in_variableWithLagOrLead1183 = new BitSet(new ulong[]{0x0000000000001800UL});
    public static readonly BitSet FOLLOW_numberPlusMinus_in_variableWithLagOrLead1185 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_91_in_variableWithLagOrLead1187 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_numberPlusMinus1221 = new BitSet(new ulong[]{0x0000300000000000UL});
    public static readonly BitSet FOLLOW_set_in_numberPlusMinus1227 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_logExp0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Ident_in_ident1256 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_extraTokens_in_ident1260 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_frmlEnding0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STARS_in_pow1356 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_HAT_in_pow1379 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionLogExp_in_synpred31_Model1057 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableWithLagOrLead_in_synpred32_Model1081 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_synpred33_Model1093 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}