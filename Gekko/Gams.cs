using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using GAMS;
using ProtoBuf;
using ProtoBuf.Meta;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Antlr.Runtime.Debug;
using System.Collections;
using System.Windows.Forms;

namespace Gekko
{
    public class ASTNodeGAMS
    {
        /// <summary>
        /// See comments for very similar and more complicated ASTNode class for .gcm file reading.
        /// </summary>
        /// <returns></returns>

        private List<ASTNodeGAMS> children = null; //private so that the implementation might change (for instance LinkedList etc.)
        public Parser.Gek.GekkoSB Code = new Parser.Gek.GekkoSB(); //the C# code produced while walking the tree
        public Parser.Gek.GekkoSB Gekko = new Parser.Gek.GekkoSB(); //the Gekko code produced while walking the tree
        public Parser.Gek.GekkoSB GAMS = new Parser.Gek.GekkoSB(); //the unfolded GAMS code produced while walking the tree
        public ASTNodeGAMS Parent = null;
        public string Text = null;  //ANTLR decoration of the node (for instance 'ASTPRT' or '1.45').
        public int Line = 0;
        public int Number = 0;  //used to check position among siblings
        public string leftBlanks = null;

        public IEnumerable ChildrenIterator()
        {
            if (this.children != null)
            {
                foreach (ASTNodeGAMS child in this.children)
                {
                    yield return child;
                }
            }
        }

        public void RemoveLast()
        {
            this.children.RemoveAt(this.children.Count - 1);
        }

        public ASTNodeGAMS GetChild(string s)
        {
            foreach (ASTNodeGAMS child in this.ChildrenIterator())
            {
                if (child.Text == s) return child;
            }
            return null;
        }

        public ASTNodeGAMS this[int i]
        {
            get
            {
                return this.GetChild(i);
            }
            set
            {
                this.children[i] = value;
            }
        }

        public int ChildrenCount()
        {
            if (children == null) return 0;
            return children.Count;
        }

        //Gets the C# code of child i.
        public Parser.Gek.GekkoSB GetChildCode(int i)
        {
            ASTNodeGAMS child = this.GetChild(i);
            if (child == null)
            {
                Parser.Gek.GekkoSB xx = new Parser.Gek.GekkoSB();
                return xx;
            }
            else return child.Code;
        }

        //Prepares an AST node to have children
        public void CreateChildren(int n)
        {
            this.children = new List<ASTNodeGAMS>(n);
        }

        public bool IsLastChild()
        {
            if (this.Parent == null) return true;
            if (this.Number == this.Parent.ChildrenCount() - 1) return true;  //should not be possible to be >
            return false;
        }

        public bool IsFirstChild()
        {
            if (this.Parent == null) return true;
            if (this.Number == 0) return true;
            return false;
        }

        //Sets the text of the AST node
        public ASTNodeGAMS(string text)
        {
            this.Text = text;
        }

        //Sets the text of the AST node
        public ASTNodeGAMS(string text, string leftBlanks)
        {
            this.Text = text;
            this.leftBlanks = leftBlanks;
        }

        //Sets the text of the AST node, and augments with children.
        public ASTNodeGAMS(string text, bool withChildren)
        {
            this.Text = text;
            if (withChildren)
            {
                this.children = new List<ASTNodeGAMS>();
            }
        }

        public ASTNodeGAMS GetChild(int i)
        {
            if (this.children == null) return null;
            if (i >= this.children.Count) return null;  //does not exist
            return this.children[i];
        }

        public void Add(ASTNodeGAMS child)
        {
            this.children.Add(child);
            child.Parent = this;
            child.Number = children.Count - 1;
        }

        public string ToString()
        {
            return this.Text;
        }

        public void PrintAST2(ASTNodeGAMS node, int depth)
        {
            G.Writeln(G.Blanks(depth * 2) + node.Text);
            if (node.children != null)
            {
                for (int i = 0; i < node.children.Count; ++i)
                {
                    ASTNodeGAMS child = (ASTNodeGAMS)(node.children[i]);
                    PrintAST2(child, depth + 1);
                }
            }
        }
    }

    public static class GamsModel
    {

        public static void ParserGAMSCreateASTHelper(string textInput)
        {

            ANTLRStringStream input = new ANTLRStringStream(textInput + "\n");  //a newline for ease of use of ANTLR

            List<string> errors = null;
            CommonTree t = null;

            // Create a lexer attached to that input
            GAMSLexer lexer = new GAMSLexer(input);
            // Create a stream of tokens pulled from the lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            // Create a parser attached to the token stream
            GAMSParser parser = new GAMSParser(tokens);
            // Invoke the program rule in get return value
            GAMSParser.expr_return r = null;
            DateTime t0 = DateTime.Now;

            bool print = true;
            ASTNodeGAMS root = new ASTNodeGAMS(null);

            try
            {
                r = parser.expr();

                errors = parser.GetErrors();
                t = (CommonTree)r.Tree;

                CreateASTNodesForGAMS(t, root, 0, tokens, print);

                if (errors.Count > 0)
                {
                    new Writeln(textInput);
                    new Warning("GAMS parse error");
                }
            }
            catch (Exception e)
            {
                new Writeln(textInput);
                new Warning("GAMS other error");
            }

            WalkHelper wh = new WalkHelper();
            Controlled controlled = new Controlled();
            WalkASTAndEmit(root, 0, wh, controlled);

            return;
        }

        private static bool DetectNullNode(CommonTree ast)
        {
            return ast.Text == null && !(ast.Children != null && ast.Children.Count > 0);
        }

        public static void CreateASTNodesForGAMS(CommonTree ast, ASTNodeGAMS cmdNode, int depth, CommonTokenStream tokens, bool print)
        {
            if (DetectNullNode(ast))
            {
                //not sure why this happens in ANTLR: some empty CommonTree nodes
                //we filter them out: otherwise they just create empty
                //lines in the generated C# code (with linenumber=0 which is no good either)
                //No children = we are not cutting anything real from the AST tree anyway
                return;
            }

            cmdNode.Text = ast.Text;
            cmdNode.Line = ast.Line;

            CommonTree xx = (CommonTree)ast;
            int iStart = xx.TokenStartIndex;
            if (iStart - 1 >= 0)
            {
                CommonToken xxx = (CommonToken)tokens.Get(iStart - 1);
                if (xxx.Text.Trim() == "")
                {
                    cmdNode.leftBlanks = xxx.Text;
                }
            }

            if (print)
            {
                int length = 0;
                if (cmdNode.leftBlanks != null) length = cmdNode.leftBlanks.Length;
                using (Writeln text = new Writeln())
                {
                    text.MainAdd("|" + G.Blanks(depth * 2) + cmdNode.Text + "     [" + length + "]");
                    text.MainOmitVeryFirstNewLine();
                }
            }

            if (ast.Children == null)
            {
                return;
            }

            int num = ast.Children.Count;
            cmdNode.CreateChildren(num);
            for (int i = 0; i < num; ++i)
            {
                CommonTree d = (CommonTree)(ast.Children[i]);
                if (DetectNullNode(d)) continue;
                ASTNodeGAMS cmdNodeChild = new ASTNodeGAMS(null);  //unknown text
                cmdNodeChild.Parent = cmdNode;
                cmdNode.Add(cmdNodeChild);
                CreateASTNodesForGAMS(d, cmdNodeChild, depth + 1, tokens, print);
            }
        }

        public class WalkHelper
        {

        }

        public static void WalkASTAndEmit(ASTNodeGAMS node, int depth, WalkHelper wh, Controlled controlled)
        {
            WalkASTAndEmitBefore(node, wh, controlled);

            if (false)
            {
                controlled = controlled.Clone();
                controlled.names.Add("a");
                controlled.elements.Add("55");
            }

            foreach (ASTNodeGAMS child in node.ChildrenIterator())
            {
                WalkASTAndEmit(child, depth + 1, wh, controlled);
            }

            WalkASTAndEmitAfter(node, wh, controlled);

        }

        private static void WalkASTAndEmitBefore(ASTNodeGAMS node, WalkHelper wh, Controlled controlled)
        {

            //Before sub-nodes
            switch (node.Text?.ToUpper())
            {

                case "XXXXXXX":
                    {
                        //w.wh.seriesHelper = WalkHelper.seriesType.SeriesLhs;
                    }
                    break;
            }
        }

        private static void WalkASTAndEmitAfter(ASTNodeGAMS node, WalkHelper wh, Controlled controlled)
        {
            switch (node.Text?.ToUpper())
            {

                case "ASTEQU":
                    {
                    }
                    break;
                case "ASTVARWI":
                    {
                        //Can be both in ASTEQU -> ASTEQU1, and in ASTVALUE. The former case is more restricte syntax-wise.
                        if (node.Parent.Text == "ASTEQU1")
                        {
                            List<string> sets = new List<string>();
                            //equation definition, defining sets that the eq is looping over
                            string eqName = node[1].Text;  //[2] is not used here
                            ASTNodeGAMS child = node?[3]?[0]?[1]?[1];
                            if (child != null && child.ChildrenCount() > 0)
                            {
                                //the equation has indexes (controlled sets)
                                foreach (ASTNodeGAMS child2 in child.ChildrenIterator())
                                {
                                    if (child2.ChildrenCount() != 1)
                                    {
                                        new Error("In equation '" + eqName + "': expected simple all controlled sets to be simple names");
                                    }
                                    string s = child2?[0].Text;
                                    if (G.IsIdent(s))
                                    {
                                        sets.Add(s);
                                    }
                                    else
                                    {
                                        new Error("Expected simple set name, not this: " + s);
                                    }
                                }
                            }
                            ASTNodeGAMS childDollar = node?[4]?[0]?[1];

                            //Imagine we have e1[i, j] $ (i0(i) and (i.val > 30 or j.val > 40)) ..
                            //The logical values are backed up, resulting into for instance
                            //true and (false or true)


                            //maybe get this as C# code, depending on i and j sets and returning true/false.
                            //from List<string>sets, we can get the combinations of i and j elements.


                        }
                        else
                        {
                            //defining a variable or parameter (or even set condition like tx0(t))
                        }
                    }
                    break;
                case "ASTIDX":
                    {
                    }
                    break;
                case "ASTIDXELEMENTS":
                    {
                    }
                    break;
                case "ASTVARIABLEANDLEAD":
                    {
                        if (node.ChildrenCount() > 1)
                        {
                            //x[a+1], x[a-1]
                            //control...
                        }
                        else
                        {
                            if (node.Text.StartsWith("'") || node.Text.StartsWith("\""))
                            {
                                //x['a'], x["a"]
                                node.Code.A(node.Text.Substring(1, node.Text.Length - 2));
                            }
                            else
                            {
                                //control...
                            }
                        }
                    }
                    break;
                case "ASTCONDITIONAL":
                    {
                    }
                    break;
                case "OR":
                    {
                    }
                    break;
                case "AND":
                    {
                    }
                    break;
                case "NOT":
                    {
                    }
                    break;
                case "NONEQUAL":
                    {
                    }
                    break;
                case "LESSTHANOREQUAL":
                    {
                    }
                    break;
                case "GREATERTHANOREQUAL":
                    {
                    }
                    break;
                case "EQUAL":
                    {
                    }
                    break;
                case "LESSTHAN":
                    {
                    }
                    break;
                case "GREATERTHAN":
                    {
                    }
                    break;
                case "+":
                    {
                        //node.Code.A("GAMS.Add(" + node[0].Code + ", " + node[1].Code + ")");
                        //node.GAMS.A(node[0].Code + "+" + node[1].Code);
                        //node.Gekko.A(node[0].Code + "+" + node[1].Code);
                    }
                    break;
                case "-":
                    {
                    }
                    break;
                case "*":
                    {
                    }
                    break;
                case "/":
                    {
                    }
                    break;
                case "**":
                    {
                    }
                    break;
                case "NEGATE":
                    {
                    }
                    break;
                case "ASTDOLLAREXPRESSION":
                    {
                    }
                    break;
                case "ASTEXPRESSION1":
                    {
                    }
                    break;
                case "ASTEXPRESSION2":
                    {
                    }
                    break;
                case "ASTEXPRESSION3":
                    {
                    }
                    break;
                case "ASTVALUE":
                    {
                    }
                    break;
                case "ASTFUNCTION":
                    {
                    }
                    break;
                case "ASTFUNCTIONELEMENTS":
                    {
                    }
                    break;
                case "ASTSUM":
                    {
                    }
                    break;
                case "ASTSUMCONTROLLED":
                    {
                    }
                    break;

            }
        }

        

        public static void Xxx()
        {
            if (true)
            {
                string msg2 = null;
                string gams = @"c:\Program Files\GAMS\38\";
                //string gams = @"c:\Program Files(x86)\GAMS\29.1\";
                Directory.SetCurrentDirectory(gams);  //necessary for some odd reason
                string control = @"c:\Thomas\Gekko\GekkoCS\Diverse\GAMS\225a\gamscntr.dat";
                gevmcs gev = new gevmcs(gams, ref msg2);
                gev.gevInitEnvironmentLegacy(control);
                gmomcs gmo = new gmomcs(gams, ref msg2);
                gmo.gmoRegisterEnvironment(gev.GetgevPtr(), ref msg2);
                gmo.gmoLoadDataLegacy(ref msg2);

                int ncols = gmo.gmoN();
                double[] x = new double[ncols];
                gmo.gmoGetVarL(ref x);
                for (int i = 0; i < ncols; i++)
                {
                    string varname = gmo.gmoGetVarNameOne(i);
                }

                int nrows = gmo.gmoM();
                int numerr = -12345;
                double lhs = double.NaN;
                for (int i = 0; i < nrows; i++)
                {
                    gmo.gmoEvalFunc(i, x, ref lhs, ref numerr);
                    double rhs = gmo.gmoGetRhsOne(i);
                    double residual = lhs - rhs;
                    string eqname = gmo.gmoGetEquNameOne(i);
                }
            }
        }

        public static void ReadGamsModel(string textInputRaw, string fileName, O.Model o)
        {

            if (G.Equal(Path.GetExtension(fileName), ".csv"))
            {
                new Error("The former .csv reader for GAMS models is obsolete");
                //throw new GekkoException();
            }
            else
            {
                ReadGamsModelNormal(textInputRaw, fileName, o);
            }
        }

        /// <summary>
        /// Read a GAMS model from a .gms/.gmy model. No real reading done here: deals with possible cached version etc.
        /// </summary>
        /// <param name="textInputRaw"></param>
        /// <param name="fileName"></param>
        /// <param name="o"></param>
        private static void ReadGamsModelNormal(string textInputRaw, string fileName, O.Model o)
        {
            //these objects typically get overridden soon
            Program.model = new Model();
            Program.model.modelGams = new ModelGams();

            Tuple<GekkoDictionary<string, string>, StringBuilder> tup = GetDependentsGams(o);
            GekkoDictionary<string, string> dependents = tup.Item1;
            string dependentsHash = tup.Item2.ToString();
            string modelHash = HandleModelFilesGams(textInputRaw + dependentsHash);

            string mdlFileNameAndPath = Globals.localTempFilesLocation + "\\" + Globals.gekkoVersion + "_" + "gams" + "_" + modelHash + ".mdl";

            if (Program.options.model_cache == true)
            {
                if (File.Exists(mdlFileNameAndPath))
                {
                    try
                    {
                        DateTime dt1 = DateTime.Now;
                        using (FileStream fs = Program.WaitForFileStream(mdlFileNameAndPath, Program.GekkoFileReadOrWrite.Read))
                        {
                            Program.model.modelGams = Serializer.Deserialize<ModelGams>(fs);
                            Program.model.modelGams.modelInfo.loadedFromMdlFile = true;
                        }
                        G.WritelnGray("Loaded known model from cache in: " + G.SecondsFormat((DateTime.Now - dt1).TotalMilliseconds));
                    }
                    catch (Exception e)
                    {
                        if (G.IsUnitTesting())
                        {
                            throw;
                        }
                        else
                        {
                            //do nothing, we then have to parse the file
                            Program.model.modelGams.modelInfo.loadedFromMdlFile = false;
                        }
                    }
                }
            }
            else
            {
                Program.model.modelGams.modelInfo.loadedFromMdlFile = false;
            }

            if (Program.model.modelGams.modelInfo.loadedFromMdlFile)
            {
                //do nothing, also no writing of .mdl file of course
            }
            else
            {

                ReadGamsModelHelper(textInputRaw, fileName, dependents, o);
                if (false && Globals.runningOnTTComputer) Sniff2();

                DateTime t1 = DateTime.Now;

                try //not the end of world if it fails (should never be done if model is read from zipped protobuffer (would be waste of time))
                {
                    DateTime dt1 = DateTime.Now;

                    //May take a little time to create: so use static serializer if doing serialize on a lot of small objects
                    RuntimeTypeModel serializer = TypeModel.Create();
                    serializer.UseImplicitZeroDefaults = false;  //otherwise an int that has default constructor value -12345 but is set to 0 will reappear as a -12345 (instead of 0). For int, 0 is default, false for bools etc.

                    // ----- SERIALIZE
                    string protobufFileName = Globals.gekkoVersion + "_" + "gams" + "_" + modelHash + ".mdl";
                    string pathAndFilename = Globals.localTempFilesLocation + "\\" + protobufFileName;
                    using (FileStream fs = Program.WaitForFileStream(pathAndFilename, Program.GekkoFileReadOrWrite.Write))
                    {
                        serializer.Serialize(fs, Program.model.modelGams);
                    }
                    G.WritelnGray("Created model cache file in " + G.SecondsFormat((DateTime.Now - dt1).TotalMilliseconds));
                }
                catch (Exception e)
                {
                    //do nothing, not the end of the world if it fails
                }
            }

        }

        /// <summary>
        /// Read (parse) a .gms/.gmy GAMS model, transforming it into Gekko-understandable equations.
        /// Calls ReadGamsEquation() for each equation.
        /// </summary>
        /// <param name="textInputRaw"></param>
        /// <param name="fileName"></param>
        /// <param name="dependents"></param>
        /// <param name="o"></param>
        private static void ReadGamsModelHelper(string textInputRaw, string fileName, GekkoDictionary<string, string> dependents, O.Model o)
        {
            StringBuilder sb1 = new StringBuilder();
            sb1.AppendLine();

            StringBuilder sb2 = new StringBuilder();
            sb2.AppendLine();

            int eqCounter = 0;

            //GAMS comments: star as first char, $ontext/offtext, # as end of line, /* */,

            //string txt = GetTextFromFileWithWait(Program.options.folder_working + "\\" + "model.gms");
            string txt = textInputRaw;
            var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
            var tags2 = new List<string>() { "!!", "#" };
            var tags3 = new List<Tuple<string, string>>() { new Tuple<string, string>("$ontext", "$offtext") };
            var tags4 = new List<string>() { "*" };

            TokenHelper tokens2 = StringTokenizer.GetTokensWithLeftBlanksRecursive(txt, tags1, tags2, tags3, tags4);
            GekkoDictionary<string, List<ModelGamsEquation>> equationsByVarname = new GekkoDictionary<string, List<ModelGamsEquation>>(StringComparer.OrdinalIgnoreCase);
            GekkoDictionary<string, List<ModelGamsEquation>> equationsByEqname = new GekkoDictionary<string, List<ModelGamsEquation>>(StringComparer.OrdinalIgnoreCase);

            List<string> problems = new List<string>();

            int counter = 0;

            foreach (TokenHelper tok in tokens2.subnodes.storage)
            {
                if (tok.type == ETokenType.EOL)
                {
                    counter++;
                }

                if (tok.s == "." && tok.Offset(1).s == "." && tok.Offset(1).leftblanks == 0)
                {
                    if (tok.Offset(2).s == "\\" || tok.Offset(2).s == "/")
                    {
                        //this may be part of a path, $GDXIN ..\Data\ADAM\estbk_okt16.gdx
                    }
                    else
                    {
                        eqCounter = ReadGamsEquation(sb1, sb2, eqCounter, equationsByVarname, equationsByEqname, tok, dependents, problems, G.Equal(o.opt_dump, "yes"));
                    }
                }
            }
            Program.model.modelGams = new ModelGams();
            Program.model.modelGams.equationsByVarname = equationsByVarname;
            Program.model.modelGams.equationsByEqname = equationsByEqname;
            Program.model.modelGams.modelInfo = new ModelInfoGams();

            G.Writeln2("MODEL: " + Path.GetFileNameWithoutExtension(fileName));
            G.Writeln("Read " + counter + " lines from " + fileName);
            G.Writeln("Found " + equationsByVarname.Count + " distinct equations (use DISP to display them)");
            if (problems.Count > 0)
            {
                G.Writeln("There were the following problems while reading the model:");
                foreach (string s in problems) G.Writeln("+++ " + s);
            }

            if (G.Equal(o.opt_dump, "yes"))
            {
                using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\dump.gcm", Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    sw.Write(sb1);
                }

                using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\dump.gms", Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    sw.Write(sb2);
                }
            }
        }

        /// <summary>
        /// Read (parse) a .gms/.gmy GAMS equation, translating it into an equivalent Gekko equation.
        /// The result is put into a ModelGamsEquation object.
        /// </summary>
        private static int ReadGamsEquation(StringBuilder sb1, StringBuilder sb2, int eqCounter, Dictionary<string, List<ModelGamsEquation>> equationsByVarname, Dictionary<string, List<ModelGamsEquation>> equationsByEqname, TokenHelper tok, GekkoDictionary<string, string> dependents, List<string> problems, bool dump)
        {
            WalkTokensHelper wh = new WalkTokensHelper();

            int iEqStart = 0;
            //searches for '..' with no blank between (could be improved)
            //now we search backwards for start of line
            for (int i2 = -1; i2 > -int.MaxValue; i2--)
            {
                int iLineStart = -12345;
                if (tok.Offset(i2) == null || tok.Offset(i2).type == ETokenType.EOL)
                {
                    iEqStart = i2 + 1;
                    break;
                }
            }

            int i = iEqStart;

            //-----------------------------------------------
            //now we are ready for the equation definition
            //-----------------------------------------------

            //The equation is of this form:

            //e_pi(i,ds,t) $ (tx0(t) and d1i(i,ds,t)) .. pI(i,ds,t)*qI(i,ds,t) =E= vI(i,ds,t);

            //Tokenized in tree structure it looks like this:

            //e_pi(...) $ (...) .. pI(...)*qI(...) =E= vI(...);

            //So the following:
            // eqname
            // maybe a set parenthesis
            // maybe a dollar
            //     if so either a (...) or a variable with a (...)
            // a '..' always
            // a leftside until '=e='
            // a rightside after'=e=' until semicolon

            string eqnameGams = null;
            string conditionalsGams = null;
            string conditionalsCs = null;
            string setsGams = null;
            List<string> setsGamsList = new List<string>();
            string lhsGams = null;
            string rhsGams = null;
            TokenHelper lhsTokensGams = null;
            TokenHelper rhsTokensGams = null;

            string dollar = null;

            eqnameGams = tok.Offset(i)?.s;

            i++;

            //this may be parentheses
            TokenHelper tok2 = tok.Offset(i);
            if (tok2.SubnodesTypeParenthesisStart())
            {
                setsGams = tok2.subnodes.ToString();

                List<TokenHelperComma> split = tok2.SplitCommas(true);
                foreach (TokenHelperComma item in split)
                {
                    string set = item.list.ToString();
                    setsGamsList.Add(set.Trim());
                }

                i++;

                if (tok.Offset(i).s == "$")
                {
                    i++;
                    TokenHelper tok3 = tok.Offset(i);
                    if (tok3.subnodes != null)
                    {
                        //Gekko syntax
                        conditionalsGams = tok3.subnodes.ToString();

                        //C# syntax
                        TokenHelper conditionalsTokensCs = tok3.DeepClone(null);
                        WalkTokensHandleParentheses(conditionalsTokensCs); //changes '[' and '{' into '('
                        WalkTokensHelper temp = new WalkTokensHelper();
                        WalkTokensCsSyntax(conditionalsTokensCs, temp, null);
                        conditionalsCs = conditionalsTokensCs.ToStringTrim();
                    }

                    // see also #9872034985732, removing stray " and"
                    if (tok3.SubnodesTypeParenthesisStart())
                    {

                        TokenList list = new TokenList();
                        for (int ii = 0; ii < tok3.subnodes.storage.Count; ii++)
                        {
                            if (ii < tok3.subnodes.Count() - 1 && tok3.subnodes[ii].HasNoChildren() && tok3.subnodes[ii + 1] != null && tok3.subnodes[ii + 1].HasChildren())
                            {
                                //Remove anything that looks like time restriction
                                List<TokenHelperComma> temp = tok3.subnodes[ii + 1].SplitCommas(true);
                                if (temp.Count == 1 && G.Equal(temp[0].list.ToString().Trim(), wh.t))
                                {
                                    ii += 2;
                                    if (G.Equal(tok3.subnodes[ii]?.s, "and"))
                                    {
                                        ii++;  //also check before
                                    }
                                    ii--;  //will get 1 added at loop start
                                    continue;
                                }
                            }
                            list.storage.Add(tok3.subnodes[ii]);
                        }

                        WalkTokensHelper wh2 = new WalkTokensHelper();
                        wh2.checkIfVariableIsASet = true;

                        WalkTokensHandleParentheses(list);
                        WalkTokensGekkoSyntax(list, wh2);

                        dollar = list.ToStringTrim();

                        if (dollar.StartsWith("(") && dollar.EndsWith(")"))
                        {
                            dollar = dollar.Substring(1, dollar.Length - 2).Trim();
                            if (dollar.StartsWith("and ")) dollar = dollar.Substring("and ".Length).Trim();
                        }

                        i++;
                    }
                    else
                    {
                        string s7 = tok.Offset(i).ToString();
                        if (!G.IsIdent(s7))
                        {
                            new Error("Expected a name instead of '" + s7 + "' , " + tok.Offset(i).LineAndPosText());
                            //throw new GekkoException();
                        }
                        i++;

                        string s8 = tok.Offset(i).ToString();
                        if (!(tok.Offset(i).SubnodesTypeParenthesisStart()))
                        {
                            new Error("Expected a (...) parenthesis instead of '" + s8 + "' , " + tok.Offset(i).LineAndPosText());
                            //throw new GekkoException();
                        }
                        i++;
                    }
                }
            }

            if (tok.Offset(i)?.s == "." && tok.Offset(i + 1)?.s == ".")
            {
                //good, we are at the '..' part, now comes the LHS expression
            }
            else
            {
                new Error("Expected '..' in eq definition, " + tok.Offset(i).LineAndPosText());
                //throw new GekkoException();
            }
            i++;
            i++;

            //now ready for the contents of the equation

            //find lhs of equation -----------------------------------------
            int i1Start = i;

            List<string> eqsign = new List<string>() { "=", "e", "=" };

            int iEqual = tok.Search(i1Start, eqsign, false, false);

            if (iEqual == -12345)
            {
                new Error("Could not find '=e=' in eq definition, " + tok.Offset(i).LineAndPosText());
            }

            int i1End = iEqual - 1;
            int i2Start = i1End + eqsign.Count + 1;
            int iSemi = tok.Search(i2Start, new List<string>() { ";" }, false, false);

            if (iSemi == -12345)
            {
                new Error("Could not find ending ';' in eq definition, " + tok.Offset(i).LineAndPosText());
            }

            int iEqEnd = iSemi;

            lhsGams = tok.OffsetInterval(i1Start, i1End).ToString().Trim();
            lhsTokensGams = tok.OffsetInterval(i1Start, i1End);

            rhsGams = tok.OffsetInterval(i2Start, iSemi - 1).ToString().Trim();
            rhsTokensGams = tok.OffsetInterval(i2Start, iSemi - 1);

            eqCounter++;

            if (false && eqCounter < 10)
            {
                G.Writeln2("Eqname:  " + eqnameGams);
                G.Writeln("Sets:    " + setsGams);
                G.Writeln("Condit.: " + conditionalsGams);
                G.Writeln("LHS:     " + lhsGams);
                G.Writeln("RHS:     " + rhsGams);
            }

            ModelGamsEquation equation = new ModelGamsEquation();

            equation.nameGams = eqnameGams;
            equation.setsGams = setsGams;
            equation.setsGamsList = setsGamsList;
            equation.conditionalsGams = conditionalsGams;
            equation.lhsGams = lhsGams;
            equation.rhsGams = rhsGams;
            equation.lhsTokensGams = lhsTokensGams;
            equation.rhsTokensGams = rhsTokensGams;

            //Gekko syntax

            TokenHelper lhsTokensGekko = equation.lhsTokensGams.DeepClone(null);
            WalkTokensHandleParentheses(lhsTokensGekko); //changes '[' and '{' into '('
            WalkTokensHelper wt1Gekko = new WalkTokensHelper();
            WalkTokensGekkoSyntax(lhsTokensGekko, wt1Gekko);
            string lhsGekko = lhsTokensGekko.ToStringTrim();

            TokenHelper rhsTokensGekko = equation.rhsTokensGams.DeepClone(null);
            WalkTokensHandleParentheses(rhsTokensGekko); //changes '[' and '{' into '('
            WalkTokensHelper wt2Gekko = new WalkTokensHelper();
            WalkTokensGekkoSyntax(rhsTokensGekko, wt2Gekko);
            string rhsGekko = rhsTokensGekko.ToStringTrim();

            ////C# syntax

            //TokenHelper lhsTokensCs = equation.lhsTokensGams.DeepClone(null);
            //WalkTokensHandleParentheses(lhsTokensCs); //changes '[' and '{' into '('
            //WalkTokensHelper wt1Cs= new WalkTokensHelper();
            //Controlled controlledLhs = new Controlled();
            //WalkTokensCsSyntax(lhsTokensCs, wt1Cs, controlledLhs);
            //string lhsCs = lhsTokensCs.ToStringTrim();

            //TokenHelper rhsTokensCs = equation.rhsTokensGams.DeepClone(null);
            //WalkTokensHandleParentheses(rhsTokensCs); //changes '[' and '{' into '('
            //WalkTokensHelper wt2Cs = new WalkTokensHelper();
            //Controlled controlledRhs = new Controlled();
            //WalkTokensCsSyntax(rhsTokensCs, wt2Cs, controlledRhs);
            //string rhsCs = rhsTokensCs.ToStringTrim();

            if (true)
            {
                int v = 3;
                if (v == 1)
                {
                    sb1.Append("PRT " + lhsGekko + ";" + G.NL);
                    sb1.Append("PRT " + rhsGekko + ";" + G.NL);
                    sb1.AppendLine();
                }
                else if (v == 2)
                {
                    sb1.Append("PRT<n> " + lhsGekko + " - ( " + rhsGekko + " );" + G.NL);
                }
                else
                {
                    string dollar2 = null;
                    if (dollar != null && dollar.Trim() != "" && dollar.Trim() != "()")
                    {
                        dollar2 = dollar.Trim();
                    }

                    sb1.AppendLine("Equation: " + eqnameGams);
                    if (dollar2 != null)
                    {
                        sb1.Append("(" + lhsGekko + ") $ (" + dollar2 + ") = " + rhsGekko + ";" + G.NL);  //always add parentheses
                    }
                    else
                    {
                        sb1.Append(lhsGekko + " = " + rhsGekko + ";" + G.NL);
                    }

                    sb2.AppendLine("" + equation.nameGams);
                    sb2.AppendLine("" + equation.setsGams);
                    sb2.AppendLine("" + equation.conditionalsGams);
                    sb2.AppendLine(equation.lhsGams + "  =  ");
                    sb2.AppendLine(equation.rhsGams);
                    sb2.AppendLine();
                    sb2.AppendLine("--------------------------------------");
                    sb2.AppendLine();

                    //if (dollar2 != null)
                    //{
                    //    sb.Append("(" + lhs + ") $ (" + dollar2 + ")" + G.NL);  //always add parentheses
                    //}
                    //else
                    //{
                    //    sb.Append(lhs + " = " + G.NL);
                    //}
                }
            }

            if (true)
            {
                equation.lhs = lhsGekko;
                equation.rhs = rhsGekko;

                //equation.lhsCs = lhsCs;
                //equation.rhsCs = rhsCs;
                equation.conditionalsCs = conditionalsCs;

                // ------------- conditionals ---------------
                // see also #9872034985732

                string conditionals2 = null;
                if (dollar != null) conditionals2 = dollar.Trim();
                if (!G.NullOrEmpty(conditionals2))
                {
                    //removes a stray ending " and" that may be left after removing time conditionals
                    if (conditionals2.EndsWith(" and", StringComparison.OrdinalIgnoreCase)) conditionals2 = conditionals2.Substring(0, conditionals2.Length - " and".Length);
                }
                equation.conditionals = conditionals2;
            }

            bool fromList = false;
            string lhsVariable = ReadGamsModelGetLhsName(equationsByVarname, equationsByEqname, lhsTokensGekko, equation, eqnameGams, dependents, problems, ref fromList);
            string s = null;
            if (fromList) s = ", designated from list";
            if (lhsVariable == null) lhsVariable = "[not identified]";
            sb1.AppendLine("--> " + lhsVariable + " (dependent" + s + ")");
            sb1.AppendLine();
            sb1.AppendLine("----------------------------------------------------------------------------------------------------------------");
            sb1.AppendLine();

            return eqCounter;
        }

        /// <summary>
        /// Tries to identify what is the LHS variable in the GAMS equation, and puts this into dictionaries for later retrieval by variable name or equation name.
        /// </summary>
        private static string ReadGamsModelGetLhsName(Dictionary<string, List<ModelGamsEquation>> equationsByVarname, Dictionary<string, List<ModelGamsEquation>> equationsByEqname, TokenHelper lhsTokensGams2, ModelGamsEquation e, string eqnameGams, GekkoDictionary<string, string> dependents, List<string> problems, ref bool fromList)
        {
            string lhs = null;

            if (G.Equal(Program.options.model_gams_dep_method, "lhs"))
            {
                Program.GetLhsVariable(lhsTokensGams2, ref lhs);
            }
            else if (G.Equal(Program.options.model_gams_dep_method, "eqname"))
            {
                if (eqnameGams.Contains("__"))
                {
                    new Error("Eqname '" + eqnameGams + "': did not expect '__' substring in name");
                }
                string[] ss = eqnameGams.Split('_');
                if (ss.Length <= 1)
                {
                    new Error("Eqname '" + eqnameGams + "': did not find any '_' separators");
                }
                if (!G.Equal(ss[0], "e"))
                {
                    new Error("Eqname '" + eqnameGams + "': expected it to start with 'e_'");
                }
                if (!G.IsIdent(ss[1]))  //we use the e_{here}_..._..._... part
                {
                    new Error("Eqname '" + eqnameGams + "': could not resolve variable name");
                }
                lhs = ss[1];
            }
            else
            {
                new Error("option model gams dep method = lhs|eqname.");
            }

            string d = null; if (dependents != null) dependents.TryGetValue(eqnameGams, out d);
            string varnameFound = null;
            if (d != null)
            {
                //found in #dependents
                varnameFound = d;
                fromList = true;
            }

            if (varnameFound == null && lhs != null) varnameFound = lhs;

            if (varnameFound == null)
            {
                problems.Add("Could not find lhs variable in equation '" + eqnameGams + "' (line " + lhsTokensGams2.line + ")");
            }
            else
            {
                if (equationsByVarname.ContainsKey(varnameFound))
                {
                    equationsByVarname[varnameFound].Add(e);  //can have more than one eq with same lhs variable
                }
                else
                {
                    List<ModelGamsEquation> e2 = new List<ModelGamsEquation>();
                    e2.Add(e);
                    equationsByVarname.Add(varnameFound, e2);
                }

                if (equationsByEqname.ContainsKey(eqnameGams))
                {
                    new Error("The equation name '" + eqnameGams + "' appears multiple times");
                }
                else
                {
                    List<ModelGamsEquation> e2 = new List<ModelGamsEquation>();
                    e2.Add(e);
                    equationsByEqname.Add(eqnameGams, e2);
                }
            }
            return varnameFound;
        }

        public static ModelGamsEquation DecompEvalGams(string eqname, string varname)
        {
            //find the equation, either looking up eqname or varname

            List<ModelGamsEquation> eqs = null;
            ModelGamsEquation found = null;
            if (eqname != null)
            {
                eqs = GetGamsEquationsByEqname(eqname);
                if (eqs == null || eqs.Count == 0)
                {
                    new Error("Equation '" + eqname + "' was not found");
                    //throw new GekkoException();
                }
                if (eqs.Count > 1)
                {
                    new Error("Internal error #809735208375", false);
                }
                found = eqs[0];  //pick the first one, probably always only one here, cf. #820948324:
            }
            else
            {
                eqs = GetGamsEquationsByVarname(varname);
                if (eqs == null || eqs.Count == 0)
                {
                    new Error("Variable '" + varname + "' was not found");
                    //throw new GekkoException();
                }
                if (eqs.Count > 1)
                {
                    new Warning("Variable '" + varname + "' appears in several equations, first one is picked");
                }
                found = eqs[0];  //#820948324: pick the first one, a variable name may point to several equations, for instance if y is present on the lhs in several equations.
            }

            string rhs = found.rhs.Trim();
            string lhs = found.lhs.Trim();

            string s1 = Program.EquationLhsRhs(lhs, rhs, true) + ";";  //this is a generic method, not just a GAMS method

            if (found.expressions == null || found.expressions.Count == 0)
            {
                Globals.expressions = null;  //maybe not necessary
                Program.CallEval(found.conditionals, s1);
                found.expressions = new List<Func<GekkoSmpl, IVariable>>(Globals.expressions);  //probably needs cloning/copying as it is done here, similar to found.expressions = Globals.expressions
                Globals.expressions = null;  //maybe not necessary
            }
            else
            {
                //has already been done
            }

            return found;
        }

        private static void Sniff2()
        {
            DateTime dt = DateTime.Now;
            double ms1 = 0;
            double ms2 = 0;
            int n1 = 0;
            int n2 = 0;
            int n3 = 0;

            int counterA = 0;
            int counterError1 = 0;
            int counterError2 = 0;

            foreach (KeyValuePair<string, List<ModelGamsEquation>> kvp in Program.model.modelGams.equationsByEqname)
            {
                //if (counterA > 6) break;
                if (counterA % 50 == 0) G.Writeln2("--> " + counterA);

                counterA++;
                ModelGamsEquation eq = kvp.Value[0];

                eq.expressionVariablesWithSets = new List<EquationVariablesGams>();

                string rhs = eq.rhs.Trim();
                string lhs = eq.lhs.Trim();
                string s1 = Program.EquationLhsRhs(lhs, rhs, true) + ";";

                if (eq.expressions == null || eq.expressions.Count == 0)
                {
                    Globals.expressions = null;  //maybe not necessary

                    try
                    {
                        DateTime dt1 = DateTime.Now;
                        Program.CallEval(eq.conditionals, s1);
                        ms1 += (dt1 - DateTime.Now).TotalMilliseconds;
                        n1++;
                    }
                    catch (Exception e)
                    {
                        counterError1++;
                        if (e.Message.Contains("System.OutOfMemoryException"))
                        {
                            G.Writeln2("+++ ERROR: MEMORY in equation (type 2): " + eq.nameGams);
                        }
                        else
                        {
                            G.Writeln2("+++ ERROR: in equation  (type 2): " + eq.nameGams);
                        }
                        continue;
                    }
                    eq.expressions = new List<Func<GekkoSmpl, IVariable>>(Globals.expressions);  //probably needs cloning/copying as it is done here

                    DateTime dt2 = DateTime.Now;
                    foreach (Func<GekkoSmpl, IVariable> expression in eq.expressions)
                    {

                        //Function call start --------------
                        //O.AdjustSmplForDecomp(smpl, 0);
                        //TODO: can be deleted, #p24234oi32

                        try
                        {
                            string op = "d";
                            GekkoTime per1 = new GekkoTime(EFreq.A, 2020, 1);
                            GekkoTime per2 = new GekkoTime(EFreq.A, 2020, 1);
                            string residualName = "residual___";
                            int funcCounter = 0;
                            DecompData dd = Gekko.Decomp.DecompLowLevel(per1, per2, expression, Gekko.Decomp.DecompBanks(op), residualName, ref funcCounter);

                            List<string> m1 = new List<string>();
                            List<string> m2 = new List<string>();
                            foreach (string s in dd.cellsContribD.storage.Keys)
                            {
                                string ss5 = Program.DecompGetNameFromContrib(s);
                                if (!m1.Contains(ss5, StringComparer.OrdinalIgnoreCase))
                                {
                                    m1.Add(ss5);
                                }
                            }
                            EquationVariablesGams temp = new EquationVariablesGams();
                            temp.equationVariables = m1;
                            eq.expressionVariablesWithSets.Add(temp);
                        }
                        catch (Exception e)
                        {
                            counterError2++;
                            eq.expressionVariablesWithSets.Add(null); //keep alignment
                            if (e.Message.Contains("System.OutOfMemoryException"))
                            {
                                G.Writeln2("+++ ERROR: MEMORY in equation: " + eq.nameGams);
                            }
                            else
                            {
                                G.Writeln2("+++ ERROR: in equation: " + eq.nameGams);
                            }
                            break;
                        }
                        n2++;
                    }
                    ms2 += (dt2 - DateTime.Now).TotalMilliseconds;
                    n3++;
                    Globals.expressions = null;  //maybe not necessary
                }
            }
            G.Writeln2("EVAL on " + counterA + " eqs, errors in " + counterError1 + "/" + counterError2 + " of these, " + (dt - DateTime.Now).TotalMilliseconds / 1000d + " " + (-ms1 / 1000d) + " " + (-ms2 / 1000d));
            G.Writeln2("n1 " + n1 + " n2 " + n2 + " n3 " + n3);
        }

        private static List<ModelGamsEquation> GetGamsEquationsByEqname(string variable)
        {
            if (Program.model.modelGams.equationsByEqname == null || Program.model.modelGams.equationsByEqname.Count == 0)
            {
                new Error("No GAMS equations found");
                //throw new GekkoException();
            }
            List<ModelGamsEquation> eqs = null; Program.model.modelGams.equationsByEqname.TryGetValue(variable, out eqs);
            return eqs;
        }

        public static List<ModelGamsEquation> GetGamsEquationsByVarname(string variable)
        {
            if (Program.model.modelGams.equationsByVarname == null || Program.model.modelGams.equationsByVarname.Count == 0)
            {
                new Error("No GAMS equations found");
                //throw new GekkoException();
            }
            List<ModelGamsEquation> eqs = null; Program.model.modelGams.equationsByVarname.TryGetValue(variable, out eqs);
            return eqs;
        }

        private static Tuple<GekkoDictionary<string, string>, StringBuilder> GetDependentsGams(O.Model o)
        {
            GekkoDictionary<string, string> dependents = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            //hashHelper: will get the format: "--- dependents ---<NL>a;b;c<NL>c,d,e<NL>"
            //the dependents list does not change the model per se, but it changes how DISP and other commands
            //like DECOMP show stuff.
            StringBuilder hashHelper = new StringBuilder();
            hashHelper.AppendLine();
            hashHelper.AppendLine("--- dependents ---");

            IVariable lhsList = o.opt_dep;
            if (lhsList != null)
            {
                List lhsList_list = lhsList as List;
                if (lhsList_list == null)
                {
                    new Error("Variable #dependents should be of list type");
                    //throw new GekkoException();
                }
                int c = 0;
                foreach (IVariable x in lhsList_list.list)
                {
                    c++;
                    if (x.Type() != EVariableType.List)
                    {
                        new Error("#dependents sublist line " + c + ": should be of list type");
                        //throw new GekkoException();
                    }
                    List x_list = x as List;

                    List<string> ss = null;

                    try
                    {
                        ss = Stringlist.GetListOfStringsFromList(x_list);
                    }
                    catch
                    {
                        new Error("#dependents sublist line " + c + ": all elements should be strings");
                        throw;
                    }

                    foreach (string s in ss)
                    {
                        hashHelper.Append(s.ToLower()).Append(";");
                    }
                    hashHelper.AppendLine();

                    if (ss.Count < 2)
                    {
                        new Error("#dependents sublist line " + c + ": must have > 1 elements");
                        //throw new GekkoException();
                    }
                    string lhs = ss[0];
                    for (int i = 1; i < ss.Count; i++)
                    {
                        //The ss list has this form for each line:
                        //qG; E_qG; E_qG_tot    --> first the lhs name, then the equations where it is a lhs variable
                        //Since each equation can only have 1 lhs, the eqnames (E_qG etc.) can at most appear 1 time in
                        //the ss list.

                        string temp = null; dependents.TryGetValue(ss[i], out temp);
                        if (temp != null)
                        {
                            new Error("#dependents sublist line " + c + ": The equation '" + ss[i] + "' already assigns '" + temp + "' as lhs");
                            //throw new GekkoException();
                        }
                        dependents.Add(ss[i], lhs);
                    }
                }
            }

            return new Tuple<GekkoDictionary<string, string>, StringBuilder>(dependents, hashHelper);
        }

        private static bool CheckIfVarIsASet(string name, WalkTokensHelper th)
        {
            bool isSetWithIndexer = false;
            if (th.checkIfVariableIsASet)
            {

                IVariable iv = Program.databanks.GetFirst().GetIVariable("#" + name);
                if (iv != null && iv.Type() == EVariableType.List)
                {
                    isSetWithIndexer = true;
                }
            }
            return isSetWithIndexer;
        }

        /// <summary>
        /// Helper
        /// </summary>
        public static void WalkTokensGekkoSyntax(TokenList nodes, WalkTokensHelper th)
        {
            foreach (TokenHelper child in nodes.storage)
            {
                WalkTokensGekkoSyntax(child, th);
            }
        }

        /// <summary>
        /// Actual transformation of GAMS equations into Gekko statements. IMPORTANT: Keep this 100% synchronized with WalkTokensCsSyntax
        /// </summary>
        /// <param name="node"></param>
        /// <param name="th"></param>
        public static void WalkTokensGekkoSyntax(TokenHelper node, WalkTokensHelper th)
        {
            //Performs these transformations:
            //- GAMS functions are not touched (log, etc)
            //-      but sqr() becomes sqrt()
            //- sum() function has # put in on sets
            //- parameter t is removed, and lags/leads like t-1 are transformed into [-1] etc. So x(a, t) --> x[#a], and x(t) --> x not x().
            //- tBase handled
            //- strings have quotes removed, x['a'] --> x[a]
            //- stuff like a.val becomes #a.val(), whereas t.val is ignored for now
            //- sameas(i,j) and sameas(i,'a') become #i==#j and #i=='a'
            //- single '=' becomes '=='
            //
            //- all t or t+1 or t-1 etc. are recorded, together with any tBase

            if (node.HasNoChildren())
            {
                //not a sub-node
                if (node.s != "" && node.type == ETokenType.Word)
                {
                    //an IDENT-type leaf node, not symbols etc.
                    //patterns like "log(" or "exp(" or "sum(" are skipped, also stuff like "*(" is avoided

                    string word = node.s;

                    TokenHelper nextNode = node.Offset(1);
                    if (nextNode != null && nextNode.HasChildren() && nextNode.SubnodesType() == "(" && nextNode.subnodes[0].leftblanks == 0)
                    {
                        //a pattern like "x(" with no blanks in between

                        if (G.Equal(node.s, "sameas"))
                        {
                            List<TokenHelperComma> split = nextNode.SplitCommas(false);
                            if (split.Count != 2)
                            {
                                new Error("Expected sameas() function with 2 arguments");
                            }

                            node.s = "";
                            split[1].comma.s = "==";
                        }
                        else if (Globals.gamsFunctions.ContainsKey(node.s))
                        {
                            string x = Globals.gamsFunctions[node.s];
                            if (x != null)
                            {
                                node.s = x;  //sqr() --> sqrt()
                            }

                            //"sum(" or "log(" or "exp(" etc.
                            if (G.Equal(node.s, "sum"))
                            {
                                if (nextNode.subnodes.Count() > 0)
                                {
                                    if (nextNode.subnodes[1].HasNoChildren())
                                    {
                                        //stuff like "sum(i, x(i))"
                                        if (nextNode.subnodes[1].type == ETokenType.Word && (G.Equal(nextNode.subnodes[2].s, ",") || G.Equal(nextNode.subnodes[2].s, "$")))
                                        {
                                            //checks that it has "sum(x," or sum(x$" pattern
                                            nextNode.subnodes[1].s = "#" + nextNode.subnodes[1].s;
                                        }
                                    }
                                    else
                                    {
                                        //stuff like "sum((i, j), x(i, j))"
                                        List<TokenHelperComma> list2 = nextNode.subnodes[1].SplitCommas(true);
                                        foreach (TokenHelperComma item in list2)
                                        {
                                            if (item.list.Count() == 1 && item.list[0].type == ETokenType.Word)
                                            {
                                                item.list[0].s = "#" + item.list[0].s;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //a "sum()" --> not handled
                                }
                            }
                        }
                        else
                        {
                            //first we check for stuff like a15t100(a), where a15t100 is a set, not a variable
                            //so it should be #a15t100[#a], not a15t100[#a]

                            bool isSetWithIndexer = CheckIfVarIsASet(node.s, th);
                            if (isSetWithIndexer) node.s = "#" + node.s;

                            bool removeParenthesis = false;
                            if (true)
                            {
                                //now we look at the arguments, x(a1, a2, 's', t) or x(a1, a2, 's', t-1) or x(a1, a2, 's')
                                List<TokenHelperComma> split = nextNode.SplitCommas(true);

                                for (int iSplit = 0; iSplit < split.Count; iSplit++)
                                {
                                    TokenHelperComma helper = split[iSplit];
                                    if (helper.list.storage.Count == 0)
                                    {
                                        //empty parenthesis, how is that possible?
                                    }
                                    else if (helper.list.storage.Count == 1)
                                    {
                                        //a single token in the slot , .... , so this is not an expression like t+1 etc.

                                        if (helper.list[0].type == ETokenType.Word)
                                        {
                                            //helper.list[0] is the single token

                                            if (iSplit == split.Count - 1 && (G.Equal(helper.list[0].s, th.t) || G.Equal(helper.list[0].s, th.tBase)))
                                            {
                                                //t or tBase at last position

                                                if (G.Equal(helper.list[0].s, th.t))
                                                {
                                                    //normal t
                                                    //remove the trailing t
                                                    helper.list[0].Clear();
                                                    if (helper.comma == null)
                                                    {
                                                        removeParenthesis = true;  //t is the only argument as in "x(t)" which becomes "x" not "x()"
                                                    }
                                                    else
                                                    {
                                                        helper.comma.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    //tBase
                                                    //x(i, tBase) --> x[#i][%tBase]
                                                    //we need to transform one []-subnode into two consequtive
                                                    //see also #89075203489

                                                    TokenHelper nextNode2 = new TokenHelper(); nextNode2.subnodes = new TokenList();
                                                    //[%tBase]
                                                    nextNode2.subnodes.storage.Add(new TokenHelper("["));
                                                    nextNode2.subnodes.storage.Add(new TokenHelper(Globals.symbolScalar + helper.list[0].s));
                                                    nextNode2.subnodes.storage.Add(new TokenHelper("]"));

                                                    TokenHelper nextNode1 = new TokenHelper(); nextNode1.subnodes = new TokenList();
                                                    if (split.Count > 1)
                                                    {
                                                        nextNode1.subnodes.storage.Add(new TokenHelper("["));
                                                        for (int iii = 0; iii < split.Count - 1; iii++)
                                                        {
                                                            if (split[iii].comma != null) nextNode1.subnodes.storage.Add(split[iii].comma);
                                                            nextNode1.subnodes.storage.AddRange(split[iii].list.storage);
                                                        }
                                                        nextNode1.subnodes.storage.Add(new TokenHelper("]"));
                                                    }
                                                    else
                                                    {
                                                        //x(i, tBase) --> x[#i][%tBase], but x(tBase) --> x[%tBase]
                                                    }

                                                    int id = nextNode.id;
                                                    TokenHelper parent = nextNode.parent;

                                                    parent.subnodes.storage.RemoveAt(id);
                                                    parent.subnodes.storage.Insert(id, nextNode2);
                                                    parent.subnodes.storage.Insert(id, nextNode1);
                                                    parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok

                                                }
                                            }
                                            else
                                            {
                                                //x(i) --> x(#i) --actually--> x[#i]
                                                helper.list[0].s = "#" + helper.list[0].s;
                                            }
                                        }
                                        else if (helper.list[0].type == ETokenType.QuotedString)
                                        {
                                            //remove the quotes
                                            helper.list[0].s = G.StripQuotes(helper.list[0].s);
                                        }
                                    }
                                    else if (helper.list.storage.Count == 3)  //x and plusminus and number
                                    {

                                        //the ... argument in (... , ... , ... , ...) is an expression, for instance t-1 etc.
                                        if (helper.list[0].type == ETokenType.Word)
                                        {
                                            //if (iSplit == split.Count - 1 && helper.list[0].s == "t")
                                            if (true)
                                            {
                                                //does not need to be last. Can be "t" in "x(a, 'b', t-1)", but also "a" in "x(y, a-1, t)"
                                                if (helper.list[1] != null && (helper.list[1].s == "-" || helper.list[1].s == "+"))
                                                {
                                                    //...t+... or ...t-...
                                                    if (helper.list[2] != null && (helper.list[2].type == ETokenType.Number))
                                                    {
                                                        string plusMinus = helper.list[1].s;
                                                        if (plusMinus != "+" && plusMinus != "-")
                                                        {
                                                            new Error("Expected t plus/minus an integer, " + helper.list[2].LineAndPosText());
                                                            //throw new GekkoException();
                                                        }
                                                        string number = helper.list[2].s;
                                                        int iNumber = -12345;
                                                        bool ok = int.TryParse(number, out iNumber);
                                                        if (!ok)
                                                        {
                                                            new Error("Expected '" + number + "' to be an integer, " + helper.list[2].LineAndPosText());
                                                            //throw new GekkoException();
                                                        }
                                                        //if (plusMinus == "-") iNumber = -iNumber;

                                                        if (iSplit == split.Count - 1 && G.Equal(helper.list[0].s, th.t))
                                                        {
                                                            if (iSplit == 0)
                                                            {
                                                                //x(t-1) --> x[-1]
                                                                //helper.comma will be = null
                                                                helper.list[0].Clear(); //kill the 't'completely including blanks
                                                                helper.list[1].leftblanks = 0; //no blanks to the left of for instance '-1'
                                                            }
                                                            else
                                                            {
                                                                //x(i, t-1) --> x[#i][-1]
                                                                //we need to transform one []-subnode into two consequtive
                                                                //see also #89075203489
                                                                TokenHelper nextNode2 = new TokenHelper(); nextNode2.subnodes = new TokenList();
                                                                nextNode2.subnodes.storage.Add(new TokenHelper("["));
                                                                for (int iii = 1; iii < helper.list.storage.Count; iii++)
                                                                {
                                                                    nextNode2.subnodes.storage.Add(helper.list[iii]);
                                                                }
                                                                nextNode2.subnodes.storage.Add(new TokenHelper("]"));

                                                                TokenHelper nextNode1 = new TokenHelper(); nextNode1.subnodes = new TokenList();
                                                                nextNode1.subnodes.storage.Add(new TokenHelper("["));
                                                                for (int iii = 0; iii < split.Count - 1; iii++)
                                                                {
                                                                    if (split[iii].comma != null) nextNode1.subnodes.storage.Add(split[iii].comma);
                                                                    nextNode1.subnodes.storage.AddRange(split[iii].list.storage);
                                                                }
                                                                nextNode1.subnodes.storage.Add(new TokenHelper("]"));

                                                                int id = nextNode.id;
                                                                TokenHelper parent = nextNode.parent;

                                                                parent.subnodes.storage.RemoveAt(id);
                                                                parent.subnodes.storage.Insert(id, nextNode2);
                                                                parent.subnodes.storage.Insert(id, nextNode1);
                                                                parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok
                                                            }
                                                        }
                                                        else
                                                        {
                                                            helper.list[0].s = "#" + helper.list[0].s;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (removeParenthesis)
                            {
                                nextNode.subnodes[0].Clear();
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].Clear();
                            }
                            else
                            {
                                nextNode.subnodes[0].s = "[";
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].s = "]";
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].leftblanks = 0; //we do not want x[#i, #j ], x[#i, #j] is nicer.
                            }
                        }
                    }
                    else
                    {

                        //could be a standalone a here: ... $ (sameas(a, '15'))
                        bool isSetWithIndexer = CheckIfVarIsASet(node.s, th);
                        if (isSetWithIndexer) node.s = "#" + node.s;

                        TokenHelper nextNode1 = node.Offset(1);
                        TokenHelper nextNode2 = node.Offset(2);

                        if (nextNode1 != null && nextNode2 != null)
                        {

                            if (nextNode1.s == "." && G.Equal(nextNode2.s, "val"))
                            {
                                //a pattern like a.val or t.val, used in for instance a.val > 15 etc.
                                //now we transform a.val into #a.val().
                                //it must use val(), since the #a elements are strings.
                                //the fact that x[#a+1] works is a special exception.
                                //node.s = "#" + node.s;
                                nextNode2.s = nextNode2.s + "()";
                            }
                        }
                    }
                }
                else if (node.s == "=")
                {
                    TokenHelper prevNode1 = node.Offset(-1);
                    if (prevNode1 != null && (prevNode1.s == "<" || prevNode1.s == ">"))
                    {
                        //do nothing, we do not want <= to become <== !
                    }
                    else
                    {
                        node.s = "==";  //stuff like ... $ (a.val = 15)
                    }
                }
                else if (node.s == "$")
                {
                    TokenHelper nextNode = node.Offset(1);  //b
                    TokenHelper nextNode2 = node.Offset(2);  //(i, j)
                    //We look for the pattern "a $ b(i, j)", where Gekko does not allow simply a $ b[#i, #j], but must use a $ (b[#i, #j])
                    if (nextNode != null && nextNode.s != "" && nextNode.type == ETokenType.Word)
                    {
                        if (nextNode2 != null && nextNode2.HasChildren() && nextNode2.SubnodesType() == "(" && nextNode2.subnodes[0].leftblanks == 0)
                        {
                            int id = nextNode.id;
                            TokenHelper parent = nextNode.parent;
                            TokenHelper newNode = new TokenHelper(); newNode.subnodes = new TokenList();
                            newNode.subnodes.storage.Add(new TokenHelper("("));
                            newNode.subnodes.storage.Add(nextNode);
                            newNode.subnodes.storage.Add(nextNode2);
                            newNode.subnodes.storage.Add(new TokenHelper(")"));
                            parent.subnodes.storage.RemoveAt(id);
                            parent.subnodes.storage.Insert(id, newNode);
                            parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok
                        }
                    }
                }
            }
            else
            {
                //an empty node with children
                for (int i = 0; i < node.subnodes.storage.Count; i++)  //the count may increase, because subnodes may be added dynamically (translating x[i, t-1] into x[#i][-1])
                {
                    WalkTokensGekkoSyntax(node.subnodes.storage[i], th);
                }
            }
        }

        /// <summary>
        /// Actual transformation of GAMS equations into Gekko statements. IMPORTANT: Keep this 100% synchronized with WalkTokensGekkoSyntax
        /// </summary>
        /// <param name="node"></param>
        /// <param name="th"></param>
        public static void WalkTokensCsSyntax(TokenHelper node, WalkTokensHelper th, Controlled controlled)
        {
            //Performs these transformations:
            //- GAMS functions are not touched (log, etc)
            //-      but sqr() becomes sqrt()
            //- sum() function has # put in on sets
            //- parameter t is removed, and lags/leads like t-1 are transformed into [-1] etc. So x(a, t) --> x[#a], and x(t) --> x not x().
            //- tBase handled
            //- strings have quotes removed, x['a'] --> x[a]
            //- stuff like a.val becomes #a.val(), whereas t.val is ignored for now
            //- sameas(i,j) and sameas(i,'a') become #i==#j and #i=='a'
            //- single '=' becomes '=='
            //
            //- all t or t+1 or t-1 etc. are recorded, together with any tBase

            if (node.HasNoChildren())
            {
                //not a sub-node
                if (node.s != "" && node.type == ETokenType.Word)
                {
                    //an IDENT-type leaf node, not symbols etc.
                    //patterns like "log(" or "exp(" or "sum(" are skipped, also stuff like "*(" is avoided

                    string word = node.s;

                    TokenHelper nextNode = node.Offset(1);
                    if (nextNode != null && nextNode.HasChildren() && nextNode.SubnodesType() == "(" && nextNode.subnodes[0].leftblanks == 0)
                    {
                        //a pattern like "x(" with no blanks in between

                        if (G.Equal(node.s, "sameas"))
                        {
                            List<TokenHelperComma> split = nextNode.SplitCommas(false);
                            if (split.Count != 2)
                            {
                                new Error("Expected sameas() function with 2 arguments");
                            }

                            node.s = "";
                            split[1].comma.s = "==";
                        }
                        else if (Globals.gamsFunctions.ContainsKey(node.s))
                        {
                            string x = Globals.gamsFunctions[node.s];
                            if (x != null)
                            {
                                node.s = x;  //sqr() --> sqrt()
                            }

                            //"sum(" or "log(" or "exp(" etc.
                            if (G.Equal(node.s, "sum"))
                            {
                                List<string> names = new List<string>();
                                List<List<string>> elements = new List<List<string>>();
                                string condition = null;
                                string content = null;

                                if (nextNode.subnodes.Count() > 0)
                                {
                                    if (nextNode.subnodes[1].HasNoChildren())
                                    {
                                        //stuff like "sum(i, x(i))"
                                        if (nextNode.subnodes[1].type == ETokenType.Word && (G.Equal(nextNode.subnodes[2].s, ",") || G.Equal(nextNode.subnodes[2].s, "$")))
                                        {
                                            //if it has "sum(x," or sum(x$" pattern
                                            if (G.Equal(nextNode.subnodes[2].s, "$"))
                                            {
                                                int i = StringTokenizer.FindS(nextNode.subnodes.storage, 3, ",");
                                                condition = StringTokenizer.GetTextFromLeftBlanksTokens(nextNode.subnodes.storage, 3, i - 1, true).Trim();
                                            }
                                            WalkTokensCsSyntaxHelper1(names, elements, nextNode.subnodes[1].s);

                                        }
                                    }
                                    else
                                    {
                                        //stuff like "sum((i, j), x(i, j))"
                                        if (G.Equal(nextNode.subnodes[2].s, ",") || G.Equal(nextNode.subnodes[2].s, "$"))
                                        {
                                            if (G.Equal(nextNode.subnodes[2].s, "$"))
                                            {
                                                int i = StringTokenizer.FindS(nextNode.subnodes.storage, 3, ",");
                                                condition = StringTokenizer.GetTextFromLeftBlanksTokens(nextNode.subnodes.storage, 3, i - 1, true).Trim();
                                            }
                                            List<TokenHelperComma> list2 = nextNode.subnodes[1].SplitCommas(true);
                                            foreach (TokenHelperComma item in list2)
                                            {
                                                if (item.list.Count() == 1 && item.list[0].type == ETokenType.Word)
                                                {
                                                    WalkTokensCsSyntaxHelper1(names, elements, item.list[0].s);
                                                }
                                            }
                                        }
                                    }

                                    //Now, we do the combinations ... + ... + ... + ...

                                    Controlled controlledNew = controlled.Clone();
                                    int depth = 0;
                                    Loop(depth, names, elements, controlledNew);
                                }
                                else
                                {
                                    //a "sum()" --> not handled
                                }
                            }
                        }
                        else
                        {
                            //first we check for stuff like a15t100(a), where a15t100 is a set, not a variable
                            //so it should be #a15t100[#a], not a15t100[#a]

                            bool isSetWithIndexer = CheckIfVarIsASet(node.s, th);
                            if (isSetWithIndexer) node.s = "#" + node.s;

                            bool removeParenthesis = false;
                            if (true)
                            {
                                //now we look at the arguments, x(a1, a2, 's', t) or x(a1, a2, 's', t-1) or x(a1, a2, 's')
                                List<TokenHelperComma> split = nextNode.SplitCommas(true);

                                for (int iSplit = 0; iSplit < split.Count; iSplit++)
                                {
                                    TokenHelperComma helper = split[iSplit];
                                    if (helper.list.storage.Count == 0)
                                    {
                                        //empty parenthesis, how is that possible?
                                    }
                                    else if (helper.list.storage.Count == 1)
                                    {
                                        //a single token in the slot , .... , so this is not an expression like t+1 etc.

                                        if (helper.list[0].type == ETokenType.Word)
                                        {
                                            //helper.list[0] is the single token

                                            if (iSplit == split.Count - 1 && (G.Equal(helper.list[0].s, th.t) || G.Equal(helper.list[0].s, th.tBase)))
                                            {
                                                //t or tBase at last position

                                                if (G.Equal(helper.list[0].s, th.t))
                                                {
                                                    //normal t
                                                    //remove the trailing t
                                                    helper.list[0].Clear();
                                                    if (helper.comma == null)
                                                    {
                                                        removeParenthesis = true;  //t is the only argument as in "x(t)" which becomes "x" not "x()"
                                                    }
                                                    else
                                                    {
                                                        helper.comma.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    //tBase
                                                    //x(i, tBase) --> x[#i][%tBase]
                                                    //we need to transform one []-subnode into two consequtive
                                                    //see also #89075203489

                                                    TokenHelper nextNode2 = new TokenHelper(); nextNode2.subnodes = new TokenList();
                                                    //[%tBase]
                                                    nextNode2.subnodes.storage.Add(new TokenHelper("["));
                                                    nextNode2.subnodes.storage.Add(new TokenHelper(Globals.symbolScalar + helper.list[0].s));
                                                    nextNode2.subnodes.storage.Add(new TokenHelper("]"));

                                                    TokenHelper nextNode1 = new TokenHelper(); nextNode1.subnodes = new TokenList();
                                                    if (split.Count > 1)
                                                    {
                                                        nextNode1.subnodes.storage.Add(new TokenHelper("["));
                                                        for (int iii = 0; iii < split.Count - 1; iii++)
                                                        {
                                                            if (split[iii].comma != null) nextNode1.subnodes.storage.Add(split[iii].comma);
                                                            nextNode1.subnodes.storage.AddRange(split[iii].list.storage);
                                                        }
                                                        nextNode1.subnodes.storage.Add(new TokenHelper("]"));
                                                    }
                                                    else
                                                    {
                                                        //x(i, tBase) --> x[#i][%tBase], but x(tBase) --> x[%tBase]
                                                    }

                                                    int id = nextNode.id;
                                                    TokenHelper parent = nextNode.parent;

                                                    parent.subnodes.storage.RemoveAt(id);
                                                    parent.subnodes.storage.Insert(id, nextNode2);
                                                    parent.subnodes.storage.Insert(id, nextNode1);
                                                    parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok

                                                }
                                            }
                                            else
                                            {
                                                //x(i) --> x(#i) --actually--> x[#i]
                                                helper.list[0].s = "#" + helper.list[0].s;
                                            }
                                        }
                                        else if (helper.list[0].type == ETokenType.QuotedString)
                                        {
                                            //remove the quotes
                                            helper.list[0].s = G.StripQuotes(helper.list[0].s);
                                        }
                                    }
                                    else if (helper.list.storage.Count == 3)  //x and plusminus and number
                                    {

                                        //the ... argument in (... , ... , ... , ...) is an expression, for instance t-1 etc.
                                        if (helper.list[0].type == ETokenType.Word)
                                        {
                                            //if (iSplit == split.Count - 1 && helper.list[0].s == "t")
                                            if (true)
                                            {
                                                //does not need to be last. Can be "t" in "x(a, 'b', t-1)", but also "a" in "x(y, a-1, t)"
                                                if (helper.list[1] != null && (helper.list[1].s == "-" || helper.list[1].s == "+"))
                                                {
                                                    //...t+... or ...t-...
                                                    if (helper.list[2] != null && (helper.list[2].type == ETokenType.Number))
                                                    {
                                                        string plusMinus = helper.list[1].s;
                                                        if (plusMinus != "+" && plusMinus != "-")
                                                        {
                                                            new Error("Expected t plus/minus an integer, " + helper.list[2].LineAndPosText());
                                                            //throw new GekkoException();
                                                        }
                                                        string number = helper.list[2].s;
                                                        int iNumber = -12345;
                                                        bool ok = int.TryParse(number, out iNumber);
                                                        if (!ok)
                                                        {
                                                            new Error("Expected '" + number + "' to be an integer, " + helper.list[2].LineAndPosText());
                                                            //throw new GekkoException();
                                                        }
                                                        //if (plusMinus == "-") iNumber = -iNumber;

                                                        if (iSplit == split.Count - 1 && G.Equal(helper.list[0].s, th.t))
                                                        {
                                                            if (iSplit == 0)
                                                            {
                                                                //x(t-1) --> x[-1]
                                                                //helper.comma will be = null
                                                                helper.list[0].Clear(); //kill the 't'completely including blanks
                                                                helper.list[1].leftblanks = 0; //no blanks to the left of for instance '-1'
                                                            }
                                                            else
                                                            {
                                                                //x(i, t-1) --> x[#i][-1]
                                                                //we need to transform one []-subnode into two consequtive
                                                                //see also #89075203489
                                                                TokenHelper nextNode2 = new TokenHelper(); nextNode2.subnodes = new TokenList();
                                                                nextNode2.subnodes.storage.Add(new TokenHelper("["));
                                                                for (int iii = 1; iii < helper.list.storage.Count; iii++)
                                                                {
                                                                    nextNode2.subnodes.storage.Add(helper.list[iii]);
                                                                }
                                                                nextNode2.subnodes.storage.Add(new TokenHelper("]"));

                                                                TokenHelper nextNode1 = new TokenHelper(); nextNode1.subnodes = new TokenList();
                                                                nextNode1.subnodes.storage.Add(new TokenHelper("["));
                                                                for (int iii = 0; iii < split.Count - 1; iii++)
                                                                {
                                                                    if (split[iii].comma != null) nextNode1.subnodes.storage.Add(split[iii].comma);
                                                                    nextNode1.subnodes.storage.AddRange(split[iii].list.storage);
                                                                }
                                                                nextNode1.subnodes.storage.Add(new TokenHelper("]"));

                                                                int id = nextNode.id;
                                                                TokenHelper parent = nextNode.parent;

                                                                parent.subnodes.storage.RemoveAt(id);
                                                                parent.subnodes.storage.Insert(id, nextNode2);
                                                                parent.subnodes.storage.Insert(id, nextNode1);
                                                                parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok
                                                            }
                                                        }
                                                        else
                                                        {
                                                            helper.list[0].s = "#" + helper.list[0].s;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (removeParenthesis)
                            {
                                nextNode.subnodes[0].Clear();
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].Clear();
                            }
                            else
                            {
                                nextNode.subnodes[0].s = "[";
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].s = "]";
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].leftblanks = 0; //we do not want x[#i, #j ], x[#i, #j] is nicer.
                            }
                        }
                    }
                    else
                    {

                        //could be a standalone a here: ... $ (sameas(a, '15'))
                        bool isSetWithIndexer = CheckIfVarIsASet(node.s, th);
                        if (isSetWithIndexer) node.s = "#" + node.s;

                        TokenHelper nextNode1 = node.Offset(1);
                        TokenHelper nextNode2 = node.Offset(2);

                        if (nextNode1 != null && nextNode2 != null)
                        {

                            if (nextNode1.s == "." && G.Equal(nextNode2.s, "val"))
                            {
                                //a pattern like a.val or t.val, used in for instance a.val > 15 etc.
                                //now we transform a.val into #a.val().
                                //it must use val(), since the #a elements are strings.
                                //the fact that x[#a+1] works is a special exception.
                                //node.s = "#" + node.s;
                                nextNode2.s = nextNode2.s + "()";
                            }
                        }
                    }
                }
                else if (node.s == "=")
                {
                    TokenHelper prevNode1 = node.Offset(-1);
                    if (prevNode1 != null && (prevNode1.s == "<" || prevNode1.s == ">"))
                    {
                        //do nothing, we do not want <= to become <== !
                    }
                    else
                    {
                        node.s = "==";  //stuff like ... $ (a.val = 15)
                    }
                }
                else if (node.s == "$")
                {
                    TokenHelper nextNode = node.Offset(1);  //b
                    TokenHelper nextNode2 = node.Offset(2);  //(i, j)
                    //We look for the pattern "a $ b(i, j)", where Gekko does not allow simply a $ b[#i, #j], but must use a $ (b[#i, #j])
                    if (nextNode != null && nextNode.s != "" && nextNode.type == ETokenType.Word)
                    {
                        if (nextNode2 != null && nextNode2.HasChildren() && nextNode2.SubnodesType() == "(" && nextNode2.subnodes[0].leftblanks == 0)
                        {
                            int id = nextNode.id;
                            TokenHelper parent = nextNode.parent;
                            TokenHelper newNode = new TokenHelper(); newNode.subnodes = new TokenList();
                            newNode.subnodes.storage.Add(new TokenHelper("("));
                            newNode.subnodes.storage.Add(nextNode);
                            newNode.subnodes.storage.Add(nextNode2);
                            newNode.subnodes.storage.Add(new TokenHelper(")"));
                            parent.subnodes.storage.RemoveAt(id);
                            parent.subnodes.storage.Insert(id, newNode);
                            parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok
                        }
                    }
                }
            }
            else
            {
                //an empty node with children
                for (int i = 0; i < node.subnodes.storage.Count; i++)  //the count may increase, because subnodes may be added dynamically (translating x[i, t-1] into x[#i][-1])
                {
                    WalkTokensCsSyntax(node.subnodes.storage[i], th, controlled);
                }
            }
        }

        public static void Loop(int depth, List<string> names, List<List<string>> elements, Controlled controlled)
        {
            for (int i = 0; i < elements[depth].Count; i++)
            {
                controlled.names.Add(names[depth]);
                controlled.elements.Add(elements[depth][i]);
                if (depth + 1 < names.Count)
                {
                    Loop(depth + 1, names, elements, controlled);
                }
                else
                {
                    //walk......

                    new Writeln(Stringlist.GetListWithCommas(controlled.names) + " ----- " + Stringlist.GetListWithCommas(controlled.elements));

                    controlled.names.RemoveAt(controlled.names.Count - 1);
                    controlled.elements.RemoveAt(controlled.elements.Count - 1);
                }
            }
            if (depth > 0)
            {
                controlled.names.RemoveAt(controlled.names.Count - 1);
                controlled.elements.RemoveAt(controlled.elements.Count - 1);
            }
        }

        private static void WalkTokensCsSyntaxHelper1(List<string> controlled1, List<List<string>> controlled2, string name)
        {
            IVariable m = O.GetIVariableFromString("#" + name, O.ECreatePossibilities.NoneReturnNull);
            if (m == null) new Error("Cannot find the list #" + name + " (representing the GAMS set " + name + ")");
            controlled1.Add(name);
            controlled2.Add(new List<string>());
            foreach (IVariable iv in O.ConvertToList(m))
            {
                controlled2[controlled2.Count - 1].Add(O.ConvertToString(iv));
            }
        }

        public static void WalkTokensHandleParentheses(TokenList nodes)
        {
            foreach (TokenHelper child in nodes.storage)
            {
                WalkTokensHandleParentheses(child);
            }
        }


        public static void WalkTokensHandleParentheses(TokenHelper node)
        {
            //All [, {, } and ] are changed into soft parentheses ( )
            if (node.HasNoChildren())
            {
                //not a sub-node
                if (node.s == "[") node.s = "(";
                else if (node.s == "{") node.s = "(";
                else if (node.s == "]") node.s = ")";
                else if (node.s == "}") node.s = ")";
                return;
            }
            else
            {
                //an empty node with children

                foreach (TokenHelper child in node.subnodes.storage)
                {
                    WalkTokensHandleParentheses(child);
                }
            }
        }


        public static string HandleModelFilesGams(string input)
        {
            List<string> lines = Stringlist.ExtractLinesFromText(input);
            return GetModelHashGams(lines);
        }

        private static string GetModelHashGams(List<string> lines)
        {
            string trueHash = Program.GetMD5Hash(Stringlist.ExtractTextFromLines(lines).ToString()); //Pretty unlikely that two different gams files could produce the same hash.
            trueHash = trueHash.Trim();  //probably not necessary
            return trueHash;
        }
    }

    public static class GamsData
    {
        public static void ReadGdx(Databank databank, Program.ReadInfo readInfo, string fileLocal)
        {
            //merge and date truncation:
            //do this by first reading into a Gekko databank, and then merge that with the merge facilities from gbk read

            // ---------------------------------------
            // gdx              no t               has t
            // dims
            // ---------------------------------------
            // 0                normal timeless    NA
            //
            //
            // 1                gdim = 1           normal series
            //                  timeless
            //
            // 2                gdim = 2           gdim = 1
            //                  timeless
            //
            //3                 gdim = 3           gdim = 2
            //                  timeless

            // gdxdim = gdim + (1-istimeless)

            // only complication is that Gekko may mix timeless and non-timeless
            // subseries, maybe that should not be allowed?
            // maybe the array-superseries should know if it is timeless or not?

            string prefix = Program.options.gams_time_prefix.Trim().ToLower();
            bool hasPrefix = prefix.Length > 0;
            string file = G.AddExtension(fileLocal, "." + "gdx");
            int offset = (int)Program.options.gams_time_offset;
            DateTime dt1 = DateTime.Now;
            int skippedSets = 0;
            int importedSets = 0;
            int counterVariables = 0;
            int counterParameters = 0;
            int yearMax = int.MinValue;
            int yearMin = int.MaxValue;

            int counterFixed = 0;

            EFreq freq = EFreq.A;
            if (G.Equal(Program.options.gams_time_freq, "u")) freq = EFreq.U;
            else if (G.Equal(Program.options.gams_time_freq, "q")) freq = EFreq.Q;
            else if (G.Equal(Program.options.gams_time_freq, "m")) freq = EFreq.M;

            string gamsDir = null; GAMSWorkspace ws = null;
            GetGAMSWorkspace(ref gamsDir, ref ws);

            if (Program.options.gams_fast)
            {
                ReadGdxFast(databank, prefix, hasPrefix, file, offset, ref skippedSets, ref importedSets, ref counterVariables, ref counterParameters, ref yearMax, ref yearMin, freq, ref gamsDir);
            }
            else
            {
                new Error("The slow gdx reader is not maintained, try the faster GDX reader with: OPTION gams fast = yes;");
            }
            G.Writeln2("Finished GAMS import of " + counterVariables + " variables, " + counterParameters + " parameters and " + importedSets + " sets (" + G.Seconds(dt1) + ")");
            if (skippedSets > 0) new Note(skippedSets + " sets with dim > 1 were not imported");

            readInfo.startPerInFile = yearMin;
            readInfo.endPerInFile = yearMax;
            readInfo.nanCounter = 0;

            readInfo.variables = counterVariables + counterParameters;
            readInfo.time = (DateTime.Now - dt1).TotalMilliseconds;

            readInfo.startPerResultingBank = readInfo.startPerInFile;
            readInfo.endPerResultingBank = readInfo.endPerInFile;

            databank.FileNameWithPath = readInfo.fileName; databank.FileNameWithPathPretty = readInfo.fileNamePretty;

            //TODO: Maybe only do this on the gdx variables if possible
            //Anyway, the speed penalty is small anyway.
            databank.Trim();

            //if (Globals.runningOnTTComputer) G.Writeln2("FIXED::: " + counterFixed);

        }

        private static void ReadGdxFast(Databank databank, string prefix, bool hasPrefix, string file, int offset, ref int skippedSets, ref int importedSets, ref int counterVariables, ref int counterParameters, ref int yearMax, ref int yearMin, EFreq freq, ref string gamsDir)
        {
            if (Program.options.gams_time_detect_auto)
            {
                new Note("'OPTION gams time detect_auto = yes' ignored in 'OPTION gams fast = yes' mode");
            }
            try
            {
                string msg = string.Empty;
                string producer = string.Empty;
                int errNr = 0;
                int rc;
                int[] index = new int[gamsglobals.maxdim];
                string[] indexString = new string[gamsglobals.maxdim];
                double[] values = new double[gamsglobals.val_max];
                int[] domainSyNrs = new int[gamsglobals.maxdim];
                string[] domainStrings = new string[gamsglobals.maxdim];
                int varNr = 0;
                int nrRecs = 0;
                int n = 0;
                int gdxDimensions = 0;
                string varName = string.Empty;
                int varType = 0;
                int d;
                if (gamsDir == null) gamsDir = "";

                gdxcs gdx = new gdxcs(gamsDir, ref msg);  //it seems ok if gamsSysDir = "", then it will autolocate it (but there may be a 64-bit problem...)
                if (msg != string.Empty)
                {
                    new Error("Could not load GDX library. Message: " + msg, false);
                    GdxErrorMessage();
                    throw new GekkoException();
                }
                if (true)
                {
                    rc = gdx.gdxOpenRead(file, ref errNr);
                    if (errNr != 0)
                    {
                        {
                            new Error("gdx io error");
                            //throw new GekkoException();
                        }
                    }
                    int timeIndex = -12345;
                    int uelCount = -1; int uelHighest = -1;
                    gdx.gdxUMUelInfo(ref uelCount, ref uelHighest);
                    if (uelHighest != 0)
                    {
                        new Error("Internal UEL problem (GDX)");
                        //throw new GekkoException();
                    }
                    string[] uel = new string[uelCount + 1];
                    for (int u = 1; u <= uelCount; u++)
                    {
                        string s = null;
                        int error = -1;
                        int error2 = gdx.gdxUMUelGet(u, ref s, ref error);
                        uel[u] = s;  //remember that uel[0] is empty and not meaningful
                    }

                    timeIndex = -12345; gdx.gdxFindSymbol(Program.options.gams_time_set, ref timeIndex);
                    if (timeIndex == 0 || Program.options.gams_time_set == "")
                    {
                        new Error("Could not find the time set ('" + Program.options.gams_time_set + "')");
                        //throw new GekkoException();
                    }

                    //varType = 0: SET
                    //varType = 1: PARAM
                    //varType = 2: VARIABLE
                    //varType = 3: EQU
                    //varType = 4: ALIAS
                    for (int i = 1; i < int.MaxValue; i++)
                    {

                        gdx.gdxSymbolInfo(i, ref varName, ref gdxDimensions, ref varType);

                        string label = null; int records = -12345; int userInfo = -12345;
                        gdx.gdxSymbolInfoX(i, ref records, ref userInfo, ref label);

                        if (gdxDimensions == -1)
                        {
                            break;  //no more symbols
                        }
                        if (varType == 0 || varType == 4)
                        {
                            //
                            //  ======================================
                            //              sets
                            //  ======================================
                            //

                            if (gdxDimensions != 1)
                            {
                                skippedSets++;
                                continue;
                            }
                            List<string> setData = new List<string>();  //contains names of sets (entryNr --> symbolName)
                            if (gdx.gdxDataReadRawStart(i, ref nrRecs) == 0)
                            {
                                new Error("gdx error");
                                //throw new GekkoException();
                            }
                            while (gdx.gdxDataReadRaw(ref index, ref values, ref n) != 0)
                            {
                                string s = null;
                                s = uel[index[0]];
                                setData.Add(s);

                            }
                            gdx.gdxDataReadDone();

                            //add the list to databank
                            string name = Globals.symbolCollection + varName;
                            if (databank.ContainsIVariable(name))
                            {
                                databank.RemoveIVariable(name);
                            }
                            List ml = new List(setData);
                            databank.AddIVariable(name, ml);

                            importedSets++;
                        }
                        else if (varType == 1 || varType == 2) //parameter or variable
                        {
                            //
                            //  ======================================
                            //       parameters (1) and variables (2)
                            //  ======================================
                            //

                            string varNameWithFreq = varName + Globals.freqIndicator + G.ConvertFreq(freq);

                            //if (varName.ToLower().Contains("d10"))
                            //{

                            //}

                            //always fetched, since we use it for domains
                            gdx.gdxSymbolGetDomainX(i, ref domainStrings);
                            int timeDimNr = GdxGetTimeDimNumber(ref domainSyNrs, domainStrings, gdxDimensions, gdx, timeIndex, i);

                            if (gdx.gdxDataReadRawStart(i, ref nrRecs) == 0)
                            {
                                new Error("gdx error");
                                //throw new GekkoException();
                            }

                            int hasTimeDimension = 0;
                            if (timeDimNr != -12345) hasTimeDimension = 1;

                            int gekkoDimensions = gdxDimensions - hasTimeDimension;

                            bool isMultiDim = true;
                            if (gekkoDimensions == 0)
                            {
                                isMultiDim = false;
                            }

                            Series ts = null;
                            if (isMultiDim)
                            {
                                //Multi-dim timeseries
                                string[] domains = new string[gekkoDimensions];
                                int counter = 0;
                                for (d = 0; d < gdxDimensions; d++)
                                {
                                    if (d == timeDimNr) continue; //skipping time dimension
                                    if (domainStrings[counter] == "*") domains[counter] = domainStrings[d];
                                    else domains[counter] = Globals.symbolCollection + domainStrings[d];
                                    counter++;
                                }
                                if (databank.ContainsIVariable(varNameWithFreq)) databank.RemoveIVariable(varNameWithFreq);  //should not be possible, since merging is not allowed...
                                ts = new Series(freq, varNameWithFreq);
                                ts.meta.label = label;
                                ts.meta.domains = domains;
                                if (hasTimeDimension == 0) ts.type = ESeriesType.Timeless;
                                ts.SetArrayTimeseries(gdxDimensions, hasTimeDimension == 1);
                                if (varType == 1) ts.meta.fix = EFixedType.Parameter;
                                databank.AddIVariable(ts.name, ts);
                            }
                            else
                            {
                                //Zero-dimensional timeseries (that is, normal timeseries)
                                //A zero-dim timeseries in the Gekko sense can be timeless (scalar) or non-timeless (normal timeseries)
                                //in this case, we just construct a normal timeseries
                                if (databank.ContainsIVariable(varNameWithFreq)) databank.RemoveIVariable(varNameWithFreq);  //should not be possible, since merging is not allowed...
                                ts = new Series(freq, varNameWithFreq);
                                ts.meta.label = label;
                                if (hasTimeDimension == 0) ts.type = ESeriesType.Timeless;
                                if (varType == 1) ts.meta.fix = EFixedType.Parameter;
                                databank.AddIVariable(ts.name, ts);
                            }

                            if (varType == 1)
                            {
                                counterParameters++;
                            }
                            if (varType == 2)
                            {
                                counterVariables++;
                            }

                            List<string> oldDims = new List<string>() { "     " }; //will not match anything

                            Series ts2 = null;  //the subseries in one of the dimension coordinates

                            while (gdx.gdxDataReadRaw(ref index, ref values, ref n) != 0)
                            {
                                //Reading the dimension coordinates

                                int tt = -12345;
                                //StringBuilder sb = new StringBuilder();
                                List<string> dims = new List<string>();
                                for (d = 0; d < gdxDimensions; d++)
                                {
                                    if (d == timeDimNr)
                                    {
                                        //FIXME
                                        //FIXME
                                        //FIXME
                                        //FIXME pre-construct an uel_time with uel --> GekkoTime.
                                        //FIXME if there is a prefix and offset, handle that too!
                                        //FIXME
                                        //FIXME

                                        string timeElement = uel[index[d]];
                                        if (hasPrefix)
                                        {
                                            if (!timeElement.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                                            {
                                                using (Error e = new Error())
                                                {
                                                    e.MainAdd("GAMS variable/parameter " + varName + " has element '" + timeElement + "' in the time dimension (" + Program.options.gams_time_set + ").");
                                                    e.MainAdd("The time elements are expected to start with '" + prefix + "'.");
                                                    e.MainAdd("See 'OPTION gams time set' and 'OPTION gams time prefix.");
                                                }
                                            }
                                            timeElement = timeElement.Substring(prefix.Length);
                                        }

                                        tt = G.IntParse(timeElement);
                                        if (tt == -12345)
                                        {
                                            string txt = null;
                                            if (hasPrefix)
                                            {
                                                txt = ". Original time element name: '" + uel[index[d]] + "'";
                                            }
                                            new Error("Could not convert '" + timeElement + "' into an annual time period" + txt);
                                        }
                                        tt = tt + offset;
                                        continue;  //do not add it to the dims
                                    }
                                    string s = uel[index[d]];

                                    dims.Add(s);
                                }

                                bool equal = CompareDims(oldDims, dims);

                                if (equal)
                                {
                                    //keep the same ts2
                                    //if time is the last dimension, the hash is the same for all periods
                                    //this avoids getting the same Gekko variable over and over
                                }
                                else
                                {
                                    //create it
                                    if (isMultiDim)
                                    {
                                        MultidimItem mmi = new MultidimItem(dims.ToArray(), ts);
                                        IVariable iv = null; ts.dimensionsStorage.TryGetValue(mmi, out iv); //probably never present, if merging is not allowed
                                        if (iv == null)
                                        {
                                            ts2 = new Series(ESeriesType.Normal, freq, Globals.seriesArraySubName + Globals.freqIndicator + G.ConvertFreq(freq));
                                            if (timeDimNr == -12345) ts2.type = ESeriesType.Timeless;
                                            ts.dimensionsStorage.AddIVariableWithOverwrite(mmi, ts2);
                                        }
                                        else
                                        {
                                            ts2 = iv as Series;
                                        }
                                    }
                                    else
                                    {
                                        //zero-dimensional series
                                        ts2 = ts;  //just use that for this purpose
                                    }
                                }

                                double value = values[gamsglobals.val_level];

                                if (value == Globals.gamsEps)
                                {
                                    value = 0d;  //infinitely small value, in Gekko it is a real zero
                                }
                                else if (value == Globals.gamsNegInf)
                                {
                                    value = double.NegativeInfinity;
                                }
                                else if (value == Globals.gamsPosInf)
                                {
                                    value = double.PositiveInfinity;
                                }
                                else if (value == Globals.gamsNA)
                                {
                                    value = double.NaN;
                                }
                                else if (value == Globals.gamsUndf)
                                {
                                    value = double.NaN;
                                }

                                if (tt == -12345)
                                {
                                    ts2.SetTimelessData(value);
                                    if (GamsIsFixed(values, value))
                                    {
                                        ts2.meta.fix = EFixedType.Timeless;
                                    }
                                }
                                else
                                {
                                    //TODO
                                    //TODO
                                    //TODO record data in an array, and use setDataSequence().
                                    //TODO
                                    //TODO

                                    GekkoTime gt = new GekkoTime(freq, tt, 1);
                                    ts2.SetData(gt, value);
                                    yearMax = Math.Max(tt, yearMax);
                                    yearMin = Math.Min(tt, yearMin);

                                    if (varType == 2 && GamsIsFixed(values, value))  //not for varType == 1 (parameter)
                                    {
                                        ts2.meta.fix = EFixedType.Normal;  //will overwrite a lot, but never mind it is fast
                                        if (ts2.meta.fixedNormal == null) ts2.meta.fixedNormal = new GekkoTimeSpans();
                                        if (ts2.meta.fixedNormal.data.Count == 0)
                                        {
                                            //the very first
                                            ts2.meta.fixedNormal.data.Add(new GekkoTimeSpan(gt, gt));
                                        }
                                        else
                                        {
                                            GekkoTimeSpan gts = ts2.meta.fixedNormal.data[ts2.meta.fixedNormal.data.Count - 1];
                                            if (gts.tEnd.EqualsGekkoTime(gt.Add(-1)))
                                            {
                                                gts.tEnd = gt;
                                            }
                                            else
                                            {
                                                ts2.meta.fixedNormal.data.Add(new GekkoTimeSpan(gt, gt));
                                            }
                                        }
                                    }
                                }

                                oldDims = dims; //ok to point, dims will be created from scratch at beginning of loop
                            }  //end of records/dimensions for the variable or parameter

                            gdx.gdxDataReadDone();

                        }
                        else
                        {
                            //do nothing, skip this symbol
                        }
                    }
                }
                errNr = gdx.gdxClose();
                if (errNr != 0)
                {
                    new Error("gdx io error");
                    //throw new GekkoException();
                }
            }
            catch (Exception e)
            {
                new Error("GDX import failed with an unexpected error.");
            }
        }

        public static void WriteGdx(Databank databank, GekkoTime t1, GekkoTime t2, string pathAndFilename, List<ToFrom> list)
        {
            //merge and date truncation:
            //do this by first reading into a Gekko databank, and then merge that with the merge facilities from gbk read

            DateTime t = DateTime.Now;
            double[] gdxValues = G.CreateArrayDouble(gamsglobals.val_max, 0d);
            gdxValues[gamsglobals.val_scale] = 1d;

            string prefix = Program.options.gams_time_prefix.Trim().ToLower();
            bool hasPrefix = prefix.Length > 0;
            //string file = AddExtension(file2, "." + "gdx");
            int offset = (int)Program.options.gams_time_offset;
            DateTime dt1 = DateTime.Now;
            int skippedSets = 0;
            int exportedSets = 0;
            int counterVariables = 0;
            int counterParameters = 0;
            int yearMax = int.MinValue;
            int yearMin = int.MaxValue;

            string gamsDir = null; GAMSWorkspace ws = null;
            GetGAMSWorkspace(ref gamsDir, ref ws);

            EFreq freq = EFreq.A;
            if (G.Equal(Program.options.gams_time_freq, "u")) freq = EFreq.U;
            else if (G.Equal(Program.options.gams_time_freq, "q")) freq = EFreq.Q;
            else if (G.Equal(Program.options.gams_time_freq, "m")) freq = EFreq.M;

            double[] d = new double[1];  //used for sets

            int syCnt = 0, uelCnt = 0;

            //GAMSWorkspace ws = null;

            if (true)
            {
                string Msg = string.Empty;

                string Sysdir;
                string Producer = string.Empty;
                int ErrNr = 0;
                int rc;
                string[] Indx = new string[gamsglobals.maxdim];
                double[] Values = new double[gamsglobals.val_max];
                int VarNr = 0;
                int NrRecs = 0;
                int N = 0;
                int Dimen = 0;
                string VarName = string.Empty;
                int VarTyp = 0;
                int D;

                gdxcs gdx = new gdxcs(gamsDir, ref Msg);  //it seems ok if gamsSysDir = "", then it will autolocate it (but there may be a 64-bit problem...)
                //GdxFast gdx = new gdxcs(Sysdir, ref Msg);
                if (Msg != string.Empty)
                {
                    Console.WriteLine("**** Could not load GDX library");
                    Console.WriteLine("**** " + Msg);
                    //return 1;
                }
                gdx.gdxGetDLLVersion(ref Msg);
                Console.WriteLine("Using GDX DLL version: " + Msg);

                if (false)
                {
                    int n = 3000;

                    DateTime tt = DateTime.Now;

                    //write demand data
                    gdx.gdxOpenWrite("demanddata.gdx", "Gekko", ref ErrNr);
                    if (ErrNr != 0)
                    {
                        //xp_example1.ReportIOError(ErrNr);
                        throw new GekkoException();
                    }

                    if (gdx.gdxDataWriteStrStart("x", "label", 2, gamsglobals.dt_var, 0) == 0)
                    {
                        //ReportGDXError();
                        throw new GekkoException();
                    }

                    string[] Indx2 = new string[2];
                    int[] index = new int[2];
                    for (int i = 1; i < n + 1; i++)
                    {
                        for (int j = 1; j < n + 1; j++)
                        {
                            Values[gamsglobals.val_level] = (n * 1000) * i + j;

                            if (true)
                            {
                                Indx2[0] = i.ToString();
                                Indx2[1] = j.ToString();
                                if (gdx.gdxDataWriteStr(Indx2, Values) == 0)
                                {
                                    G.Writeln2("OOPS");
                                }
                            }
                            else
                            {
                                index[0] = i;
                                index[1] = j;
                                if (gdx.gdxDataWriteRaw(index, Values) == 0)
                                {
                                    G.Writeln2("OOPS");
                                }
                            }

                            //gdx.gdxDataWriteRaw()
                        }
                    }

                    if (gdx.gdxDataWriteDone() == 0)
                    {
                        //ReportGDXError();
                        throw new GekkoException();
                    }
                    gdx.gdxClose();
                    //Console.WriteLine("Demand data written by xp_example1");

                    G.Writeln2("TIME: " + G.Seconds(tt));

                    return;
                }


                if (true)
                {
                    gdx.gdxOpenWrite(pathAndFilename, "Gekko", ref ErrNr);
                    if (ErrNr != 0)
                    {
                        //xp_example1.ReportIOError(ErrNr);
                        throw new GekkoException();
                    }
                    //int counter = 0;
                    foreach (ToFrom bnv in list)
                    {

                        IVariable iv = O.GetIVariableFromString(bnv.s1, O.ECreatePossibilities.NoneReportError, true);

                        string name = bnv.s2;
                        string nameWithoutFreq = G.Chop_GetName(name);

                        if (iv.Type() == EVariableType.Series)
                        {

                            Series ts = iv as Series;

                            string label = ""; if (ts.meta?.label != null) label = ts.meta.label;  //label = null will fail with weird error later on

                            int timeDimension = 1;
                            if (ts.type == ESeriesType.Timeless)
                            {
                                timeDimension = 0;
                            }
                            else if (ts.type == ESeriesType.ArraySuper)
                            {
                                int ntimeless = 0;
                                int nnontimeless = 0;
                                foreach (IVariable iv2 in ts.dimensionsStorage.storage.Values)
                                {
                                    if ((iv2 as Series).type == ESeriesType.Timeless) ntimeless++;
                                    else nnontimeless++;
                                }
                                if (ntimeless > 0 && nnontimeless > 0)
                                {
                                    new Error("The array-timeseries " + ts.name + " has subseries that are both timeless and non-timeless --> cannot write to GDX.");
                                }
                                if (ntimeless > 0) timeDimension = 0;
                                //if ntimeless + nnontimeless == 0 it will be assumed to have time-dim in GAMS --> hard to know.
                            }

                            string[] domains = new string[ts.dimensions + timeDimension];
                            for (int i = 0; i < domains.Length; i++) domains[i] = "*";
                            if (timeDimension == 1) domains[domains.Length - 1] = Program.options.gams_time_set;  //we alway put the t domain last

                            //counter++;

                            if (gdx.gdxDataWriteStrStart(nameWithoutFreq, label, domains.Length, gamsglobals.dt_var, 0) == 0)
                            {
                                //ReportGDXError();
                                throw new GekkoException();
                            }

                            gdx.gdxSystemInfo(ref syCnt, ref uelCnt);

                            if (gdx.gdxSymbolSetDomainX(syCnt, domains) == 0)
                            {
                                new Error("Could not write domain names");
                                //throw new GekkoException();
                            }

                            if (ts.type == ESeriesType.ArraySuper)
                            {
                                foreach (KeyValuePair<MultidimItem, IVariable> kvp in ts.dimensionsStorage.storage)
                                {
                                    string[] ss = kvp.Key.storage;
                                    WriteGdxHelper2(t1, t2, hasPrefix, gdx, kvp.Value as Series, ss, gdxValues);
                                }
                            }
                            else
                            {
                                //normal timeseries
                                WriteGdxHelper2(t1, t2, hasPrefix, gdx, ts, new string[0], gdxValues);
                            }

                            if (gdx.gdxDataWriteDone() == 0)
                            {
                                //ReportGDXError();
                                throw new GekkoException();
                            }
                            counterVariables++;
                        }
                        else if (iv.Type() == EVariableType.List)
                        {
                            if (gdx.gdxDataWriteStrStart(nameWithoutFreq.Replace(Globals.symbolCollection.ToString(), ""), "", 1, gamsglobals.dt_set, 0) == 0)
                            {
                                //ReportGDXError();
                                throw new GekkoException();
                            }

                            List l = iv as List;

                            foreach (string s in Stringlist.GetListOfStringsFromListOfIvariables(l.list.ToArray()))
                            {
                                if (gdx.gdxDataWriteStr(new string[] { s }, d) == 0)
                                {
                                    new Error("Problem writing set for gdx");
                                    //throw new GekkoException();
                                }
                            }

                            if (gdx.gdxDataWriteDone() == 0)
                            {
                                //ReportGDXError();
                                throw new GekkoException();
                            }
                            exportedSets++;
                        }
                        else continue;


                    }

                }


                ErrNr = gdx.gdxClose();
                if (ErrNr != 0)
                {
                    //ReportIOError(ErrNr);
                    throw new GekkoException();
                }

                G.Writeln2("Wrote " + counterVariables + " variables and " + exportedSets + " sets to " + pathAndFilename + " (" + G.Seconds(t) + ")");
                if (skippedSets > 0) new Note(skippedSets + " sets with dim > 1 were not imported");


            }


        }

        public static void WriteGdxSlow(Databank databank, GekkoTime t1, GekkoTime t2, string pathAndFilename, List<ToFrom> list)
        {
            //TODO: try-catch if writing fails

            bool usePrefix = false;
            if (Program.options.gams_time_prefix.Length > 0) usePrefix = true;

            DateTime t00 = DateTime.Now;
            int counterVariables = 0;
            int timelessCounter = 0;

            DateTime dt1 = DateTime.Now;

            string gamsDir = Program.options.gams_exe_folder.Trim();
            if (gamsDir.EndsWith("\\")) gamsDir = gamsDir.Substring(0, gamsDir.Length - "\\".Length);
            if (gamsDir.Trim() == "") gamsDir = null;  //must be so and not an empty string in the GAMSWorkspace call later on

            GAMSWorkspace ws = null;
            try
            {
                ws = new GAMSWorkspace(workingDirectory: Program.options.folder_working, systemDirectory: gamsDir);
            }
            catch (Exception e)
            {
                using (Error err = new Error())
                {
                    err.MainAdd("*** ERROR: Import of gdx file (GAMS) failed. GAMSWorkspace problem.");
                    err.MainNewLineTight();
                    err.MainAdd("Technical error:");
                    err.MainNewLineTight();
                    err.MainAdd(e.Message);
                    err.MainNewLineTight();
                    err.MainAdd("Note: you may manually indicate the GAMS program folder with 'OPTION gams exe folder = ...;'");
                }
            }

            GAMSDatabase db = ws.AddDatabase();

            foreach (ToFrom bnv in list)
            {
                string name = bnv.s2;  // bnv.name;


                string nameWithoutFreq = G.Chop_RemoveFreq(name);


                IVariable iv = O.GetIVariableFromString(bnv.s1, O.ECreatePossibilities.NoneReportError, true);


                Series ts = iv as Series;
                if (ts == null) continue;  //only write timeseries at the moment

                string label = ""; if (ts.meta?.label != null) label = ts.meta.label;  //label = null will fail with weird error later on

                int timeDimension = 1;
                if (ts.type == ESeriesType.Timeless)
                {
                    timeDimension = 0;
                }
                else if (ts.type == ESeriesType.ArraySuper)
                {
                    int ntimeless = 0;
                    int nnontimeless = 0;
                    foreach (IVariable iv2 in ts.dimensionsStorage.storage.Values)
                    {
                        if ((iv2 as Series).type == ESeriesType.Timeless) ntimeless++;
                        else nnontimeless++;
                    }
                    if (ntimeless > 0 && nnontimeless > 0)
                    {
                        new Error("The array-timeseries " + ts.name + " has subseries that are both timeless and non-timeless --> cannot write to GDX.");

                        //throw new GekkoException();
                    }
                    if (ntimeless > 0) timeDimension = 0;
                    //if ntimeless + nnontimeless == 0 it will be assumed to have time-dim in GAMS --> hard to know.
                }

                string[] domains = new string[ts.dimensions + timeDimension];
                for (int i = 0; i < domains.Length; i++) domains[i] = "*";
                if (timeDimension == 1) domains[domains.Length - 1] = Program.options.gams_time_set;  //we alway put the t domain last

                GAMSVariable gvar = db.AddVariable(nameWithoutFreq, VarType.Free, label, domains);

                counterVariables = WriteGdxHelperSlow(t1, t2, usePrefix, counterVariables, ts, gvar);

            }

            db.Export(pathAndFilename);

            G.Writeln2("Exported " + counterVariables + " variables to " + pathAndFilename + " (" + G.SecondsFormat((DateTime.Now - t00).TotalMilliseconds) + ")");
            if (timelessCounter > 0) new Note(timelessCounter + " timeless timeseries skipped");
        }

        private static void WriteGdxHelper2(GekkoTime t1, GekkoTime t2, bool usePrefix, gdxcs gdx, Series ts2, string[] ss, double[] gdxValues)
        {

            if (ts2.type == ESeriesType.Timeless)
            {
                try
                {
                    gdxValues[gamsglobals.val_level] = ts2.GetTimelessData();
                    gdx.gdxDataWriteStr(ss, gdxValues);
                    //gvar.AddRecord(ss).Level = ts2.GetTimelessData();  //timeless data location
                }
                catch
                {

                }
            }
            else
            {
                GekkoTime gt1 = t1;
                GekkoTime gt2 = t2;
                if (t1.IsNull())
                {
                    gt1 = ts2.GetRealDataPeriodFirst();
                    gt2 = ts2.GetRealDataPeriodLast();
                }
                if (gt1.IsNull())
                {
                    //do not write a weird record if the timeseries has no data
                }
                else
                {
                    string[] ss2 = new string[ss.Length + 1];
                    foreach (GekkoTime t in new GekkoTimeIterator(gt1, gt2))
                    {
                        Array.Copy(ss, 0, ss2, 0, ss.Length);
                        string date = null;
                        if (usePrefix && t.freq == EFreq.A)
                        {
                            date = Program.options.gams_time_prefix + (t.super - (int)Program.options.gams_time_offset).ToString();
                        }
                        else
                        {
                            date = t.ToString();
                        }
                        ss2[ss2.Length - 1] = date;

                        gdxValues[gamsglobals.val_level] = ts2.GetDataSimple(t);
                        gdx.gdxDataWriteStr(ss2, gdxValues);

                        //gdx.gdxDataWriteRaw()  --> more efficient, see https://www.gams.com/~bussieck/LohBusWesReb.pdf, but then we need to maintain an UEL (each label has a number).
                        //and it seems that gdxDataWriteRaw() recuires lexical ordering of the array of indices??

                    }
                }
            }

            return;
        }

        private static int WriteGdxHelperSlow(GekkoTime t1, GekkoTime t2, bool usePrefix, int counterVariables, Series ts, GAMSVariable gvar)
        {

            if (ts.type == ESeriesType.ArraySuper)
            {
                foreach (KeyValuePair<MultidimItem, IVariable> kvp in ts.dimensionsStorage.storage)
                {
                    string[] ss = kvp.Key.storage;
                    WriteGdxHelperSlow2(t1, t2, usePrefix, gvar, kvp.Value as Series, ss);
                }
            }

            else
            {
                //normal timeseries
                WriteGdxHelperSlow2(t1, t2, usePrefix, gvar, ts, new string[0]);
            }
            counterVariables++;
            return counterVariables;
        }

        private static void WriteGdxHelperSlow2(GekkoTime t1, GekkoTime t2, bool usePrefix, GAMSVariable gvar, Series ts2, string[] ss)
        {
            if (ts2.type == ESeriesType.Timeless)
            {
                try
                {
                    gvar.AddRecord(ss).Level = ts2.GetTimelessData();  //timeless data location
                }
                catch
                {

                }
            }
            else
            {
                GekkoTime gt1 = t1;
                GekkoTime gt2 = t2;
                if (t1.IsNull())
                {
                    gt1 = ts2.GetRealDataPeriodFirst();
                    gt2 = ts2.GetRealDataPeriodLast();
                }
                if (gt1.IsNull())
                {
                    //do not write a weird record if the timeseries has no data
                }
                else
                {
                    foreach (GekkoTime t in new GekkoTimeIterator(gt1, gt2))
                    {
                        string[] ss2 = new string[ss.Length + 1];
                        Array.Copy(ss, 0, ss2, 0, ss.Length);
                        string date = null;
                        if (usePrefix && t.freq == EFreq.A)
                        {
                            date = Program.options.gams_time_prefix + (t.super - (int)Program.options.gams_time_offset).ToString();
                        }
                        else
                        {
                            date = t.ToString();
                        }
                        ss2[ss2.Length - 1] = date;

                        gvar.AddRecord(ss2).Level = ts2.GetDataSimple(t);

                    }
                }
            }

            return;
        }

        private static bool CompareDims(List<string> oldDims, List<string> dims)
        {
            //no test if they are null
            if (dims.Count != oldDims.Count) return false;
            for (int i = 0; i < dims.Count; i++)
            {
                if (!G.Equal(dims[i], oldDims[i])) return false;
            }
            return true;
        }


        private static bool GamsIsFixed(double[] values, double value)
        {
            return value == values[gamsglobals.val_lower] || value == values[gamsglobals.val_upper];
        }

        private static void GdxErrorMessage()
        {
            using (Note n = new Note())
            {
                n.MainAdd("You may manually indicate the GAMS program folder with 'OPTION gams exe folder',");
                n.MainAdd("for instance 'OPTION gams exe folder = c:\\GAMS\\win32\\24.8;'. In general, the");
                n.MainAdd("GAMS component is pretty good at auto-detecting the location of GAMS on the pc,");
                n.MainAdd("including finding a 32-bit GAMS if 32-bit Gekko is used, and a 64-bit GAMS if 64-bit");
                n.MainAdd("Gekko is used. It is probably not possible to use a 32-bit GAMS from a 64-bit Gekko,");
                n.MainAdd("but the inverse may be possible. In general, consider the bitness of both GAMS and");
                n.MainAdd("Gekko. Newer GAMS versions are 64-bit only, and in general, using Gekko 64-bit is");
                n.MainAdd("advised, too.");
                n.MainAdd("Bitness info: " + Program.Get64Bitness(0) + ".");
            }
        }

        private static void GetGAMSWorkspace(ref string gamsDir, ref GAMSWorkspace ws)
        {
            gamsDir = Program.options.gams_exe_folder.Trim();
            if (gamsDir.EndsWith("\\")) gamsDir = gamsDir.Substring(0, gamsDir.Length - "\\".Length);
            if (gamsDir.Trim() == "") gamsDir = null;  //must be so and not an empty string in the GAMSWorkspace call later on
            if (Program.options.gams_fast && gamsDir != null)
            {
                //do nothing
            }
            else
            {
                try
                {
                    if (Globals.gamsWorkspace == null || Globals.gamsWorkspaceHelper != gamsDir)
                    {
                        ws = new GAMSWorkspace(workingDirectory: Program.options.folder_working, systemDirectory: gamsDir);
                        Globals.gamsWorkspace = ws;
                        Globals.gamsWorkspaceHelper = gamsDir;  //record the param it was called with
                    }
                    else ws = Globals.gamsWorkspace;
                    gamsDir = ws.SystemDirectory;
                }
                catch (Exception e)
                {
                    using (Error err = new Error())
                    {
                        err.MainAdd("*** ERROR: Import of gdx file (GAMS) failed. Could not locate GAMS (GAMSWorkspace problem).");
                        err.MainNewLineTight();
                        err.MainAdd("Technical error:");
                        err.MainNewLineTight();
                        err.MainAdd(e.Message);
                        err.ThrowNoException();
                    }
                    GdxErrorMessage();
                    throw;
                }
            }
        }

        private static int GdxGetTimeDimNumber(ref int[] domainSyNrs, string[] domainStrings, int dimensions, gdxcs gdx, int timeIndex, int i)
        {
            int timeDimNr = -12345;
            gdx.gdxSymbolGetDomain(i, ref domainSyNrs);
            //only way to check it properly:
            int success = 1;
            for (int d2 = 0; d2 < dimensions; d2++)
            {
                if (domainSyNrs[d2] == 0)
                {
                    success = 0;
                    break;
                }
            }

            if (success == 1)
            {
                for (int d2 = dimensions - 1; d2 >= 0; d2--)  //backwards is faster since t is typically there
                {
                    if (domainSyNrs[d2] == timeIndex)
                    {
                        timeDimNr = d2;
                        break;
                    }
                }
            }
            else
            {
                //slower, but still not in the innermost loop
                //gdx.gdxSymbolGetDomainX(i, ref domainStrings);
                for (int d2 = dimensions - 1; d2 >= 0; d2--)  //backwards is faster since t is typically there
                {
                    if (G.Equal(domainStrings[d2], Program.options.gams_time_set))
                    {
                        timeDimNr = d2;
                        break;
                    }
                }
            }

            return timeDimNr;
        }



    }

    // C#  procedure wrapper generated by apiwrapper for GAMS Version 34.2.0
    //
    // GAMS - Loading mechanism for GAMS Expert-Level APIs
    //
    // Copyright (c) 2016-2021 GAMS Software GmbH <support@gams.com>
    // Copyright (c) 2016-2021 GAMS Development Corp. <support@gams.com>
    //
    // Permission is hereby granted, free of charge, to any person obtaining a copy
    // of this software and associated documentation files (the "Software"), to deal
    // in the Software without restriction, including without limitation the rights
    // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    // copies of the Software, and to permit persons to whom the Software is
    // furnished to do so, subject to the following conditions:
    //
    // The above copyright notice and this permission notice shall be included in all
    // copies or substantial portions of the Software.
    //
    // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    // SOFTWARE.

    internal class gdxcs : IDisposable  //found in folder c:\Program Files\GAMS\34.2\apifiles\CSharp\api\
    {
        private IntPtr pgdx;
        private bool extHandle;
        private bool _disposed;

#if __MonoCS__
    private delegate IntPtr DelLoadLibrary (string dllName, int flag);
    private delegate IntPtr DelGetProcAddress (IntPtr hModule, string procedureName);
    private delegate bool DelFreeLibrary (IntPtr hModul);

#if __APPLE__
    [DllImport("libdl.dylib")]
    internal static extern IntPtr dlopen(String dllname, int flags);

    [DllImport("libdl.dylib")]
    internal static extern IntPtr dlsym(IntPtr hModule, String procedureName);

    [DllImport("libdl.dylib")] //int
    internal static extern bool dlclose (IntPtr hModul);
#else
    [DllImport("libdl.so")]
    internal static extern IntPtr dlopen(String dllname, int flags);

    [DllImport("libdl.so")]
    internal static extern IntPtr dlsym(IntPtr hModule, String procedureName);

    [DllImport("libdl.so")]
    internal static extern bool dlclose (IntPtr hModul);
#endif

    DelLoadLibrary LoadLibrary = new DelLoadLibrary(dlopen);
    DelGetProcAddress GetProcAddress = new DelGetProcAddress (dlsym);
    DelFreeLibrary FreeLibrary = new DelFreeLibrary (dlclose);
#else
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
#endif


        public delegate void TDataStoreProc(IntPtr Indx, IntPtr Vals);
        public delegate int TDataStoreFiltProc(IntPtr Indx, IntPtr Vals, IntPtr Uptr);
        public delegate void TDomainIndexProc(int RawIndex, int MappedIndex, IntPtr Uptr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gdxSetLoadPath_t(string s);
        private static gdxSetLoadPath_t dll_gdxSetLoadPath;
        private static void d_gdxSetLoadPath(string s)
        { }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gdxGetLoadPath_t(ref byte s);
        private static gdxGetLoadPath_t dll_gdxGetLoadPath;
        private static void d_gdxGetLoadPath(ref byte s)
        { }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymAdd_t(IntPtr pgdx, string AName, string Txt, int AIndx);
        private static gdxAcronymAdd_t dll_gdxAcronymAdd;
        private static int d_gdxAcronymAdd(IntPtr pgdx, string AName, string Txt, int AIndx)
        { gdxErrorHandling("gdxAcronymAdd could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymCount_t(IntPtr pgdx);
        private static gdxAcronymCount_t dll_gdxAcronymCount;
        private static int d_gdxAcronymCount(IntPtr pgdx)
        { gdxErrorHandling("gdxAcronymCount could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymGetInfo_t(IntPtr pgdx, int N, StringBuilder AName, StringBuilder Txt, ref int AIndx);
        private static gdxAcronymGetInfo_t dll_gdxAcronymGetInfo;
        private static int d_gdxAcronymGetInfo(IntPtr pgdx, int N, StringBuilder AName, StringBuilder Txt, ref int AIndx)
        { gdxErrorHandling("gdxAcronymGetInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymGetMapping_t(IntPtr pgdx, int N, ref int orgIndx, ref int newIndx, ref int autoIndex);
        private static gdxAcronymGetMapping_t dll_gdxAcronymGetMapping;
        private static int d_gdxAcronymGetMapping(IntPtr pgdx, int N, ref int orgIndx, ref int newIndx, ref int autoIndex)
        { gdxErrorHandling("gdxAcronymGetMapping could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymIndex_t(IntPtr pgdx, double V);
        private static gdxAcronymIndex_t dll_gdxAcronymIndex;
        private static int d_gdxAcronymIndex(IntPtr pgdx, double V)
        { gdxErrorHandling("gdxAcronymIndex could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymName_t(IntPtr pgdx, double V, StringBuilder AName);
        private static gdxAcronymName_t dll_gdxAcronymName;
        private static int d_gdxAcronymName(IntPtr pgdx, double V, StringBuilder AName)
        { gdxErrorHandling("gdxAcronymName could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymNextNr_t(IntPtr pgdx, int NV);
        private static gdxAcronymNextNr_t dll_gdxAcronymNextNr;
        private static int d_gdxAcronymNextNr(IntPtr pgdx, int NV)
        { gdxErrorHandling("gdxAcronymNextNr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAcronymSetInfo_t(IntPtr pgdx, int N, string AName, string Txt, int AIndx);
        private static gdxAcronymSetInfo_t dll_gdxAcronymSetInfo;
        private static int d_gdxAcronymSetInfo(IntPtr pgdx, int N, string AName, string Txt, int AIndx)
        { gdxErrorHandling("gdxAcronymSetInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gdxAcronymValue_t(IntPtr pgdx, int AIndx);
        private static gdxAcronymValue_t dll_gdxAcronymValue;
        private static double d_gdxAcronymValue(IntPtr pgdx, int AIndx)
        { gdxErrorHandling("gdxAcronymValue could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAddAlias_t(IntPtr pgdx, string Id1, string Id2);
        private static gdxAddAlias_t dll_gdxAddAlias;
        private static int d_gdxAddAlias(IntPtr pgdx, string Id1, string Id2)
        { gdxErrorHandling("gdxAddAlias could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAddSetText_t(IntPtr pgdx, string Txt, ref int TxtNr);
        private static gdxAddSetText_t dll_gdxAddSetText;
        private static int d_gdxAddSetText(IntPtr pgdx, string Txt, ref int TxtNr)
        { gdxErrorHandling("gdxAddSetText could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxAutoConvert_t(IntPtr pgdx, int NV);
        private static gdxAutoConvert_t dll_gdxAutoConvert;
        private static int d_gdxAutoConvert(IntPtr pgdx, int NV)
        { gdxErrorHandling("gdxAutoConvert could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxClose_t(IntPtr pgdx);
        private static gdxClose_t dll_gdxClose;
        private static int d_gdxClose(IntPtr pgdx)
        { gdxErrorHandling("gdxClose could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataErrorCount_t(IntPtr pgdx);
        private static gdxDataErrorCount_t dll_gdxDataErrorCount;
        private static int d_gdxDataErrorCount(IntPtr pgdx)
        { gdxErrorHandling("gdxDataErrorCount could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataErrorRecord_t(IntPtr pgdx, int RecNr, int[] KeyInt, double[] Values);
        private static gdxDataErrorRecord_t dll_gdxDataErrorRecord;
        private static int d_gdxDataErrorRecord(IntPtr pgdx, int RecNr, int[] KeyInt, double[] Values)
        { gdxErrorHandling("gdxDataErrorRecord could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataErrorRecordX_t(IntPtr pgdx, int RecNr, int[] KeyInt, double[] Values);
        private static gdxDataErrorRecordX_t dll_gdxDataErrorRecordX;
        private static int d_gdxDataErrorRecordX(IntPtr pgdx, int RecNr, int[] KeyInt, double[] Values)
        { gdxErrorHandling("gdxDataErrorRecordX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadDone_t(IntPtr pgdx);
        private static gdxDataReadDone_t dll_gdxDataReadDone;
        private static int d_gdxDataReadDone(IntPtr pgdx)
        { gdxErrorHandling("gdxDataReadDone could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadFilteredStart_t(IntPtr pgdx, int SyNr, int[] FilterAction, ref int NrRecs);
        private static gdxDataReadFilteredStart_t dll_gdxDataReadFilteredStart;
        private static int d_gdxDataReadFilteredStart(IntPtr pgdx, int SyNr, int[] FilterAction, ref int NrRecs)
        { gdxErrorHandling("gdxDataReadFilteredStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadMap_t(IntPtr pgdx, int RecNr, int[] KeyInt, double[] Values, ref int DimFrst);
        private static gdxDataReadMap_t dll_gdxDataReadMap;
        private static int d_gdxDataReadMap(IntPtr pgdx, int RecNr, int[] KeyInt, double[] Values, ref int DimFrst)
        { gdxErrorHandling("gdxDataReadMap could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadMapStart_t(IntPtr pgdx, int SyNr, ref int NrRecs);
        private static gdxDataReadMapStart_t dll_gdxDataReadMapStart;
        private static int d_gdxDataReadMapStart(IntPtr pgdx, int SyNr, ref int NrRecs)
        { gdxErrorHandling("gdxDataReadMapStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadRaw_t(IntPtr pgdx, int[] KeyInt, double[] Values, ref int DimFrst);
        private static gdxDataReadRaw_t dll_gdxDataReadRaw;
        private static int d_gdxDataReadRaw(IntPtr pgdx, int[] KeyInt, double[] Values, ref int DimFrst)
        { gdxErrorHandling("gdxDataReadRaw could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadRawFast_t(IntPtr pgdx, int SyNr, TDataStoreProc DP, ref int NrRecs);
        private static gdxDataReadRawFast_t dll_gdxDataReadRawFast;
        private static int d_gdxDataReadRawFast(IntPtr pgdx, int SyNr, TDataStoreProc DP, ref int NrRecs)
        { gdxErrorHandling("gdxDataReadRawFast could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadRawFastFilt_t(IntPtr pgdx, int SyNr, string[] UelFilterStr, TDataStoreFiltProc DP);
        private static gdxDataReadRawFastFilt_t dll_gdxDataReadRawFastFilt;
        private static int d_gdxDataReadRawFastFilt(IntPtr pgdx, int SyNr, string[] UelFilterStr, TDataStoreFiltProc DP)
        { gdxErrorHandling("gdxDataReadRawFastFilt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadRawStart_t(IntPtr pgdx, int SyNr, ref int NrRecs);
        private static gdxDataReadRawStart_t dll_gdxDataReadRawStart;
        private static int d_gdxDataReadRawStart(IntPtr pgdx, int SyNr, ref int NrRecs)
        { gdxErrorHandling("gdxDataReadRawStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadSlice_t(IntPtr pgdx, string[] UelFilterStr, ref int Dimen, TDataStoreProc DP);
        private static gdxDataReadSlice_t dll_gdxDataReadSlice;
        private static int d_gdxDataReadSlice(IntPtr pgdx, string[] UelFilterStr, ref int Dimen, TDataStoreProc DP)
        { gdxErrorHandling("gdxDataReadSlice could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadSliceStart_t(IntPtr pgdx, int SyNr, int[] ElemCounts);
        private static gdxDataReadSliceStart_t dll_gdxDataReadSliceStart;
        private static int d_gdxDataReadSliceStart(IntPtr pgdx, int SyNr, int[] ElemCounts)
        { gdxErrorHandling("gdxDataReadSliceStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadStr_t(IntPtr pgdx, byte[,] KeyStr, double[] Values, ref int DimFrst);
        private static gdxDataReadStr_t dll_gdxDataReadStr;
        private static int d_gdxDataReadStr(IntPtr pgdx, byte[,] KeyStr, double[] Values, ref int DimFrst)
        { gdxErrorHandling("gdxDataReadStr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataReadStrStart_t(IntPtr pgdx, int SyNr, ref int NrRecs);
        private static gdxDataReadStrStart_t dll_gdxDataReadStrStart;
        private static int d_gdxDataReadStrStart(IntPtr pgdx, int SyNr, ref int NrRecs)
        { gdxErrorHandling("gdxDataReadStrStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataSliceUELS_t(IntPtr pgdx, int[] SliceKeyInt, byte[,] KeyStr);
        private static gdxDataSliceUELS_t dll_gdxDataSliceUELS;
        private static int d_gdxDataSliceUELS(IntPtr pgdx, int[] SliceKeyInt, byte[,] KeyStr)
        { gdxErrorHandling("gdxDataSliceUELS could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteDone_t(IntPtr pgdx);
        private static gdxDataWriteDone_t dll_gdxDataWriteDone;
        private static int d_gdxDataWriteDone(IntPtr pgdx)
        { gdxErrorHandling("gdxDataWriteDone could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteMap_t(IntPtr pgdx, int[] KeyInt, double[] Values);
        private static gdxDataWriteMap_t dll_gdxDataWriteMap;
        private static int d_gdxDataWriteMap(IntPtr pgdx, int[] KeyInt, double[] Values)
        { gdxErrorHandling("gdxDataWriteMap could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteMapStart_t(IntPtr pgdx, string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo);
        private static gdxDataWriteMapStart_t dll_gdxDataWriteMapStart;
        private static int d_gdxDataWriteMapStart(IntPtr pgdx, string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo)
        { gdxErrorHandling("gdxDataWriteMapStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteRaw_t(IntPtr pgdx, int[] KeyInt, double[] Values);
        private static gdxDataWriteRaw_t dll_gdxDataWriteRaw;
        private static int d_gdxDataWriteRaw(IntPtr pgdx, int[] KeyInt, double[] Values)
        { gdxErrorHandling("gdxDataWriteRaw could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteRawStart_t(IntPtr pgdx, string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo);
        private static gdxDataWriteRawStart_t dll_gdxDataWriteRawStart;
        private static int d_gdxDataWriteRawStart(IntPtr pgdx, string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo)
        { gdxErrorHandling("gdxDataWriteRawStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteStr_t(IntPtr pgdx, string[] KeyStr, double[] Values);
        private static gdxDataWriteStr_t dll_gdxDataWriteStr;
        private static int d_gdxDataWriteStr(IntPtr pgdx, string[] KeyStr, double[] Values)
        { gdxErrorHandling("gdxDataWriteStr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxDataWriteStrStart_t(IntPtr pgdx, string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo);
        private static gdxDataWriteStrStart_t dll_gdxDataWriteStrStart;
        private static int d_gdxDataWriteStrStart(IntPtr pgdx, string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo)
        { gdxErrorHandling("gdxDataWriteStrStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxGetDLLVersion_t(IntPtr pgdx, StringBuilder V);
        private static gdxGetDLLVersion_t dll_gdxGetDLLVersion;
        private static int d_gdxGetDLLVersion(IntPtr pgdx, StringBuilder V)
        { gdxErrorHandling("gdxGetDLLVersion could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxErrorCount_t(IntPtr pgdx);
        private static gdxErrorCount_t dll_gdxErrorCount;
        private static int d_gdxErrorCount(IntPtr pgdx)
        { gdxErrorHandling("gdxErrorCount could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxErrorStr_t(IntPtr pgdx, int ErrNr, StringBuilder ErrMsg);
        private static gdxErrorStr_t dll_gdxErrorStr;
        private static int d_gdxErrorStr(IntPtr pgdx, int ErrNr, StringBuilder ErrMsg)
        { gdxErrorHandling("gdxErrorStr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFileInfo_t(IntPtr pgdx, ref int FileVer, ref int ComprLev);
        private static gdxFileInfo_t dll_gdxFileInfo;
        private static int d_gdxFileInfo(IntPtr pgdx, ref int FileVer, ref int ComprLev)
        { gdxErrorHandling("gdxFileInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFileVersion_t(IntPtr pgdx, StringBuilder FileStr, StringBuilder ProduceStr);
        private static gdxFileVersion_t dll_gdxFileVersion;
        private static int d_gdxFileVersion(IntPtr pgdx, StringBuilder FileStr, StringBuilder ProduceStr)
        { gdxErrorHandling("gdxFileVersion could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFilterExists_t(IntPtr pgdx, int FilterNr);
        private static gdxFilterExists_t dll_gdxFilterExists;
        private static int d_gdxFilterExists(IntPtr pgdx, int FilterNr)
        { gdxErrorHandling("gdxFilterExists could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFilterRegister_t(IntPtr pgdx, int UelMap);
        private static gdxFilterRegister_t dll_gdxFilterRegister;
        private static int d_gdxFilterRegister(IntPtr pgdx, int UelMap)
        { gdxErrorHandling("gdxFilterRegister could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFilterRegisterDone_t(IntPtr pgdx);
        private static gdxFilterRegisterDone_t dll_gdxFilterRegisterDone;
        private static int d_gdxFilterRegisterDone(IntPtr pgdx)
        { gdxErrorHandling("gdxFilterRegisterDone could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFilterRegisterStart_t(IntPtr pgdx, int FilterNr);
        private static gdxFilterRegisterStart_t dll_gdxFilterRegisterStart;
        private static int d_gdxFilterRegisterStart(IntPtr pgdx, int FilterNr)
        { gdxErrorHandling("gdxFilterRegisterStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxFindSymbol_t(IntPtr pgdx, string SyId, ref int SyNr);
        private static gdxFindSymbol_t dll_gdxFindSymbol;
        private static int d_gdxFindSymbol(IntPtr pgdx, string SyId, ref int SyNr)
        { gdxErrorHandling("gdxFindSymbol could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxGetElemText_t(IntPtr pgdx, int TxtNr, StringBuilder Txt, ref int Node);
        private static gdxGetElemText_t dll_gdxGetElemText;
        private static int d_gdxGetElemText(IntPtr pgdx, int TxtNr, StringBuilder Txt, ref int Node)
        { gdxErrorHandling("gdxGetElemText could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxGetLastError_t(IntPtr pgdx);
        private static gdxGetLastError_t dll_gdxGetLastError;
        private static int d_gdxGetLastError(IntPtr pgdx)
        { gdxErrorHandling("gdxGetLastError could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Int64 gdxGetMemoryUsed_t(IntPtr pgdx);
        private static gdxGetMemoryUsed_t dll_gdxGetMemoryUsed;
        private static Int64 d_gdxGetMemoryUsed(IntPtr pgdx)
        { gdxErrorHandling("gdxGetMemoryUsed could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxGetSpecialValues_t(IntPtr pgdx, double[] AVals);
        private static gdxGetSpecialValues_t dll_gdxGetSpecialValues;
        private static int d_gdxGetSpecialValues(IntPtr pgdx, double[] AVals)
        { gdxErrorHandling("gdxGetSpecialValues could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxGetUEL_t(IntPtr pgdx, int UelNr, StringBuilder Uel);
        private static gdxGetUEL_t dll_gdxGetUEL;
        private static int d_gdxGetUEL(IntPtr pgdx, int UelNr, StringBuilder Uel)
        { gdxErrorHandling("gdxGetUEL could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxMapValue_t(IntPtr pgdx, double D, ref int sv);
        private static gdxMapValue_t dll_gdxMapValue;
        private static int d_gdxMapValue(IntPtr pgdx, double D, ref int sv)
        { gdxErrorHandling("gdxMapValue could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxOpenAppend_t(IntPtr pgdx, string FileName, string Producer, ref int ErrNr);
        private static gdxOpenAppend_t dll_gdxOpenAppend;
        private static int d_gdxOpenAppend(IntPtr pgdx, string FileName, string Producer, ref int ErrNr)
        { gdxErrorHandling("gdxOpenAppend could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxOpenRead_t(IntPtr pgdx, string FileName, ref int ErrNr);
        private static gdxOpenRead_t dll_gdxOpenRead;
        private static int d_gdxOpenRead(IntPtr pgdx, string FileName, ref int ErrNr)
        { gdxErrorHandling("gdxOpenRead could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxOpenWrite_t(IntPtr pgdx, string FileName, string Producer, ref int ErrNr);
        private static gdxOpenWrite_t dll_gdxOpenWrite;
        private static int d_gdxOpenWrite(IntPtr pgdx, string FileName, string Producer, ref int ErrNr)
        { gdxErrorHandling("gdxOpenWrite could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxOpenWriteEx_t(IntPtr pgdx, string FileName, string Producer, int Compr, ref int ErrNr);
        private static gdxOpenWriteEx_t dll_gdxOpenWriteEx;
        private static int d_gdxOpenWriteEx(IntPtr pgdx, string FileName, string Producer, int Compr, ref int ErrNr)
        { gdxErrorHandling("gdxOpenWriteEx could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxResetSpecialValues_t(IntPtr pgdx);
        private static gdxResetSpecialValues_t dll_gdxResetSpecialValues;
        private static int d_gdxResetSpecialValues(IntPtr pgdx)
        { gdxErrorHandling("gdxResetSpecialValues could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSetHasText_t(IntPtr pgdx, int SyNr);
        private static gdxSetHasText_t dll_gdxSetHasText;
        private static int d_gdxSetHasText(IntPtr pgdx, int SyNr)
        { gdxErrorHandling("gdxSetHasText could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSetReadSpecialValues_t(IntPtr pgdx, double[] AVals);
        private static gdxSetReadSpecialValues_t dll_gdxSetReadSpecialValues;
        private static int d_gdxSetReadSpecialValues(IntPtr pgdx, double[] AVals)
        { gdxErrorHandling("gdxSetReadSpecialValues could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSetSpecialValues_t(IntPtr pgdx, double[] AVals);
        private static gdxSetSpecialValues_t dll_gdxSetSpecialValues;
        private static int d_gdxSetSpecialValues(IntPtr pgdx, double[] AVals)
        { gdxErrorHandling("gdxSetSpecialValues could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSetTextNodeNr_t(IntPtr pgdx, int TxtNr, int Node);
        private static gdxSetTextNodeNr_t dll_gdxSetTextNodeNr;
        private static int d_gdxSetTextNodeNr(IntPtr pgdx, int TxtNr, int Node)
        { gdxErrorHandling("gdxSetTextNodeNr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSetTraceLevel_t(IntPtr pgdx, int N, string s);
        private static gdxSetTraceLevel_t dll_gdxSetTraceLevel;
        private static int d_gdxSetTraceLevel(IntPtr pgdx, int N, string s)
        { gdxErrorHandling("gdxSetTraceLevel could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbIndxMaxLength_t(IntPtr pgdx, int SyNr, int[] LengthInfo);
        private static gdxSymbIndxMaxLength_t dll_gdxSymbIndxMaxLength;
        private static int d_gdxSymbIndxMaxLength(IntPtr pgdx, int SyNr, int[] LengthInfo)
        { gdxErrorHandling("gdxSymbIndxMaxLength could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbMaxLength_t(IntPtr pgdx);
        private static gdxSymbMaxLength_t dll_gdxSymbMaxLength;
        private static int d_gdxSymbMaxLength(IntPtr pgdx)
        { gdxErrorHandling("gdxSymbMaxLength could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolAddComment_t(IntPtr pgdx, int SyNr, string Txt);
        private static gdxSymbolAddComment_t dll_gdxSymbolAddComment;
        private static int d_gdxSymbolAddComment(IntPtr pgdx, int SyNr, string Txt)
        { gdxErrorHandling("gdxSymbolAddComment could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolGetComment_t(IntPtr pgdx, int SyNr, int N, StringBuilder Txt);
        private static gdxSymbolGetComment_t dll_gdxSymbolGetComment;
        private static int d_gdxSymbolGetComment(IntPtr pgdx, int SyNr, int N, StringBuilder Txt)
        { gdxErrorHandling("gdxSymbolGetComment could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolGetDomain_t(IntPtr pgdx, int SyNr, int[] DomainSyNrs);
        private static gdxSymbolGetDomain_t dll_gdxSymbolGetDomain;
        private static int d_gdxSymbolGetDomain(IntPtr pgdx, int SyNr, int[] DomainSyNrs)
        { gdxErrorHandling("gdxSymbolGetDomain could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolGetDomainX_t(IntPtr pgdx, int SyNr, byte[,] DomainIDs);
        private static gdxSymbolGetDomainX_t dll_gdxSymbolGetDomainX;
        private static int d_gdxSymbolGetDomainX(IntPtr pgdx, int SyNr, byte[,] DomainIDs)
        { gdxErrorHandling("gdxSymbolGetDomainX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolDim_t(IntPtr pgdx, int SyNr);
        private static gdxSymbolDim_t dll_gdxSymbolDim;
        private static int d_gdxSymbolDim(IntPtr pgdx, int SyNr)
        { gdxErrorHandling("gdxSymbolDim could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolInfo_t(IntPtr pgdx, int SyNr, StringBuilder SyId, ref int Dimen, ref int Typ);
        private static gdxSymbolInfo_t dll_gdxSymbolInfo;
        private static int d_gdxSymbolInfo(IntPtr pgdx, int SyNr, StringBuilder SyId, ref int Dimen, ref int Typ)
        { gdxErrorHandling("gdxSymbolInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolInfoX_t(IntPtr pgdx, int SyNr, ref int RecCnt, ref int UserInfo, StringBuilder ExplTxt);
        private static gdxSymbolInfoX_t dll_gdxSymbolInfoX;
        private static int d_gdxSymbolInfoX(IntPtr pgdx, int SyNr, ref int RecCnt, ref int UserInfo, StringBuilder ExplTxt)
        { gdxErrorHandling("gdxSymbolInfoX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolSetDomain_t(IntPtr pgdx, string[] DomainIDs);
        private static gdxSymbolSetDomain_t dll_gdxSymbolSetDomain;
        private static int d_gdxSymbolSetDomain(IntPtr pgdx, string[] DomainIDs)
        { gdxErrorHandling("gdxSymbolSetDomain could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSymbolSetDomainX_t(IntPtr pgdx, int SyNr, string[] DomainIDs);
        private static gdxSymbolSetDomainX_t dll_gdxSymbolSetDomainX;
        private static int d_gdxSymbolSetDomainX(IntPtr pgdx, int SyNr, string[] DomainIDs)
        { gdxErrorHandling("gdxSymbolSetDomainX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxSystemInfo_t(IntPtr pgdx, ref int SyCnt, ref int UelCnt);
        private static gdxSystemInfo_t dll_gdxSystemInfo;
        private static int d_gdxSystemInfo(IntPtr pgdx, ref int SyCnt, ref int UelCnt)
        { gdxErrorHandling("gdxSystemInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELMaxLength_t(IntPtr pgdx);
        private static gdxUELMaxLength_t dll_gdxUELMaxLength;
        private static int d_gdxUELMaxLength(IntPtr pgdx)
        { gdxErrorHandling("gdxUELMaxLength could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterDone_t(IntPtr pgdx);
        private static gdxUELRegisterDone_t dll_gdxUELRegisterDone;
        private static int d_gdxUELRegisterDone(IntPtr pgdx)
        { gdxErrorHandling("gdxUELRegisterDone could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterMap_t(IntPtr pgdx, int UMap, string Uel);
        private static gdxUELRegisterMap_t dll_gdxUELRegisterMap;
        private static int d_gdxUELRegisterMap(IntPtr pgdx, int UMap, string Uel)
        { gdxErrorHandling("gdxUELRegisterMap could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterMapStart_t(IntPtr pgdx);
        private static gdxUELRegisterMapStart_t dll_gdxUELRegisterMapStart;
        private static int d_gdxUELRegisterMapStart(IntPtr pgdx)
        { gdxErrorHandling("gdxUELRegisterMapStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterRaw_t(IntPtr pgdx, string Uel);
        private static gdxUELRegisterRaw_t dll_gdxUELRegisterRaw;
        private static int d_gdxUELRegisterRaw(IntPtr pgdx, string Uel)
        { gdxErrorHandling("gdxUELRegisterRaw could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterRawStart_t(IntPtr pgdx);
        private static gdxUELRegisterRawStart_t dll_gdxUELRegisterRawStart;
        private static int d_gdxUELRegisterRawStart(IntPtr pgdx)
        { gdxErrorHandling("gdxUELRegisterRawStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterStr_t(IntPtr pgdx, string Uel, ref int UelNr);
        private static gdxUELRegisterStr_t dll_gdxUELRegisterStr;
        private static int d_gdxUELRegisterStr(IntPtr pgdx, string Uel, ref int UelNr)
        { gdxErrorHandling("gdxUELRegisterStr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUELRegisterStrStart_t(IntPtr pgdx);
        private static gdxUELRegisterStrStart_t dll_gdxUELRegisterStrStart;
        private static int d_gdxUELRegisterStrStart(IntPtr pgdx)
        { gdxErrorHandling("gdxUELRegisterStrStart could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUMFindUEL_t(IntPtr pgdx, string Uel, ref int UelNr, ref int UelMap);
        private static gdxUMFindUEL_t dll_gdxUMFindUEL;
        private static int d_gdxUMFindUEL(IntPtr pgdx, string Uel, ref int UelNr, ref int UelMap)
        { gdxErrorHandling("gdxUMFindUEL could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUMUelGet_t(IntPtr pgdx, int UelNr, StringBuilder Uel, ref int UelMap);
        private static gdxUMUelGet_t dll_gdxUMUelGet;
        private static int d_gdxUMUelGet(IntPtr pgdx, int UelNr, StringBuilder Uel, ref int UelMap)
        { gdxErrorHandling("gdxUMUelGet could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxUMUelInfo_t(IntPtr pgdx, ref int UelCnt, ref int HighMap);
        private static gdxUMUelInfo_t dll_gdxUMUelInfo;
        private static int d_gdxUMUelInfo(IntPtr pgdx, ref int UelCnt, ref int HighMap)
        { gdxErrorHandling("gdxUMUelInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxGetDomainElements_t(IntPtr pgdx, int SyNr, int DimPos, int FilterNr, TDomainIndexProc DP, ref int NrElem, IntPtr Uptr);
        private static gdxGetDomainElements_t dll_gdxGetDomainElements;
        private static int d_gdxGetDomainElements(IntPtr pgdx, int SyNr, int DimPos, int FilterNr, TDomainIndexProc DP, ref int NrElem, IntPtr Uptr)
        { gdxErrorHandling("gdxGetDomainElements could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxCurrentDim_t(IntPtr pgdx);
        private static gdxCurrentDim_t dll_gdxCurrentDim;
        private static int d_gdxCurrentDim(IntPtr pgdx)
        { gdxErrorHandling("gdxCurrentDim could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gdxRenameUEL_t(IntPtr pgdx, string OldName, string NewName);
        private static gdxRenameUEL_t dll_gdxRenameUEL;
        private static int d_gdxRenameUEL(IntPtr pgdx, string OldName, string NewName)
        { gdxErrorHandling("gdxRenameUEL could not be loaded"); return 0; }


        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void xcreate_t(ref IntPtr pgdx);
        private static xcreate_t xcreate;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void xfree_t(ref IntPtr pgdx);
        private static xfree_t xfree;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int xapiversion_t(int api, StringBuilder msg, ref int cl);
        private static xapiversion_t dll_xapiversion;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int xcheck_t(string ep, int nargs, int[] s, StringBuilder msg);
        private static xcheck_t dll_xcheck;

        public delegate bool gdxErrorCallback_t(int ErrCount, string Msg);

        static bool isLoaded = false;
        static IntPtr h;
        static bool ScreenIndicator = true;
        static bool ExceptionIndicator = false;
        static bool ExitIndicator = true;
        static gdxErrorCallback_t ErrorCallBack = null;
        static int APIErrorCount = 0;

        private bool XLibraryLoad(string dllName, ref string errBuf)
        {
            string symName;
            int cl = 0;
            IntPtr pAddressOfFunctionToCall;

            if (isLoaded)
                return true;

#if __MonoCS__
        h = LoadLibrary(@dllName,2);
#else
            h = LoadLibrary(@dllName);
#endif

            if (IntPtr.Zero == h)
            {
                errBuf = "Could not load shared library " + dllName;
                return false;
            }

            pAddressOfFunctionToCall = GetProcAddress(h, "xcreate");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                xcreate = (xcreate_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(xcreate_t));
            else
            {
                symName = "xcreate"; goto symMissing;
            }
            pAddressOfFunctionToCall = GetProcAddress(h, "xfree");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                xfree = (xfree_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(xfree_t));
            else
            {
                symName = "xfree"; goto symMissing;
            }

            pAddressOfFunctionToCall = GetProcAddress(h, "cxcheck");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                dll_xcheck = (xcheck_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(xcheck_t));
            else
            {
                symName = "cxcheck"; goto symMissing;
            }
            pAddressOfFunctionToCall = GetProcAddress(h, "cxapiversion");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                dll_xapiversion = (xapiversion_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(xapiversion_t));
            else
            {
                symName = "cxapiversion"; goto symMissing;
            }

            if (xapiversion(7, ref errBuf, ref cl) == 0)
                return false;

            pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsetloadpath");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                dll_gdxSetLoadPath = (gdxSetLoadPath_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSetLoadPath_t));
            pAddressOfFunctionToCall = GetProcAddress(h, "cgdxgetloadpath");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                dll_gdxGetLoadPath = (gdxGetLoadPath_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetLoadPath_t));
            {
                int[] s = { 3, 11, 11, 3 };
                if (xcheck("gdxAcronymAdd", 3, s, ref errBuf) == 0)
                    dll_gdxAcronymAdd = d_gdxAcronymAdd;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxacronymadd");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymAdd = (gdxAcronymAdd_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymAdd_t));
                    else
                    {
                        symName = "cgdxAcronymAdd"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxAcronymCount", 0, s, ref errBuf) == 0)
                    dll_gdxAcronymCount = d_gdxAcronymCount;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxacronymcount");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymCount = (gdxAcronymCount_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymCount_t));
                    else
                    {
                        symName = "gdxAcronymCount"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12, 12, 4 };
                if (xcheck("gdxAcronymGetInfo", 4, s, ref errBuf) == 0)
                    dll_gdxAcronymGetInfo = d_gdxAcronymGetInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxacronymgetinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymGetInfo = (gdxAcronymGetInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymGetInfo_t));
                    else
                    {
                        symName = "cgdxAcronymGetInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4, 4, 4 };
                if (xcheck("gdxAcronymGetMapping", 4, s, ref errBuf) == 0)
                    dll_gdxAcronymGetMapping = d_gdxAcronymGetMapping;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxacronymgetmapping");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymGetMapping = (gdxAcronymGetMapping_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymGetMapping_t));
                    else
                    {
                        symName = "gdxAcronymGetMapping"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 13 };
                if (xcheck("gdxAcronymIndex", 1, s, ref errBuf) == 0)
                    dll_gdxAcronymIndex = d_gdxAcronymIndex;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxacronymindex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymIndex = (gdxAcronymIndex_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymIndex_t));
                    else
                    {
                        symName = "gdxAcronymIndex"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 13, 12 };
                if (xcheck("gdxAcronymName", 2, s, ref errBuf) == 0)
                    dll_gdxAcronymName = d_gdxAcronymName;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxacronymname");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymName = (gdxAcronymName_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymName_t));
                    else
                    {
                        symName = "cgdxAcronymName"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxAcronymNextNr", 1, s, ref errBuf) == 0)
                    dll_gdxAcronymNextNr = d_gdxAcronymNextNr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxacronymnextnr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymNextNr = (gdxAcronymNextNr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymNextNr_t));
                    else
                    {
                        symName = "gdxAcronymNextNr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 11, 11, 3 };
                if (xcheck("gdxAcronymSetInfo", 4, s, ref errBuf) == 0)
                    dll_gdxAcronymSetInfo = d_gdxAcronymSetInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxacronymsetinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymSetInfo = (gdxAcronymSetInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymSetInfo_t));
                    else
                    {
                        symName = "cgdxAcronymSetInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (xcheck("gdxAcronymValue", 1, s, ref errBuf) == 0)
                    dll_gdxAcronymValue = d_gdxAcronymValue;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxacronymvalue");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAcronymValue = (gdxAcronymValue_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAcronymValue_t));
                    else
                    {
                        symName = "gdxAcronymValue"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11 };
                if (xcheck("gdxAddAlias", 2, s, ref errBuf) == 0)
                    dll_gdxAddAlias = d_gdxAddAlias;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxaddalias");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAddAlias = (gdxAddAlias_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAddAlias_t));
                    else
                    {
                        symName = "cgdxAddAlias"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 4 };
                if (xcheck("gdxAddSetText", 2, s, ref errBuf) == 0)
                    dll_gdxAddSetText = d_gdxAddSetText;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxaddsettext");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAddSetText = (gdxAddSetText_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAddSetText_t));
                    else
                    {
                        symName = "cgdxAddSetText"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxAutoConvert", 1, s, ref errBuf) == 0)
                    dll_gdxAutoConvert = d_gdxAutoConvert;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxautoconvert");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxAutoConvert = (gdxAutoConvert_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxAutoConvert_t));
                    else
                    {
                        symName = "gdxAutoConvert"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxClose", 0, s, ref errBuf) == 0)
                    dll_gdxClose = d_gdxClose;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxclose");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxClose = (gdxClose_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxClose_t));
                    else
                    {
                        symName = "gdxClose"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxDataErrorCount", 0, s, ref errBuf) == 0)
                    dll_gdxDataErrorCount = d_gdxDataErrorCount;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdataerrorcount");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataErrorCount = (gdxDataErrorCount_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataErrorCount_t));
                    else
                    {
                        symName = "gdxDataErrorCount"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 52, 54 };
                if (xcheck("gdxDataErrorRecord", 3, s, ref errBuf) == 0)
                    dll_gdxDataErrorRecord = d_gdxDataErrorRecord;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdataerrorrecord");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataErrorRecord = (gdxDataErrorRecord_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataErrorRecord_t));
                    else
                    {
                        symName = "gdxDataErrorRecord"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 52, 54 };
                if (xcheck("gdxDataErrorRecordX", 3, s, ref errBuf) == 0)
                    dll_gdxDataErrorRecordX = d_gdxDataErrorRecordX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdataerrorrecordx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataErrorRecordX = (gdxDataErrorRecordX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataErrorRecordX_t));
                    else
                    {
                        symName = "gdxDataErrorRecordX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxDataReadDone", 0, s, ref errBuf) == 0)
                    dll_gdxDataReadDone = d_gdxDataReadDone;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareaddone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadDone = (gdxDataReadDone_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadDone_t));
                    else
                    {
                        symName = "gdxDataReadDone"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 51, 4 };
                if (xcheck("gdxDataReadFilteredStart", 3, s, ref errBuf) == 0)
                    dll_gdxDataReadFilteredStart = d_gdxDataReadFilteredStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadfilteredstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadFilteredStart = (gdxDataReadFilteredStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadFilteredStart_t));
                    else
                    {
                        symName = "gdxDataReadFilteredStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 52, 54, 4 };
                if (xcheck("gdxDataReadMap", 4, s, ref errBuf) == 0)
                    dll_gdxDataReadMap = d_gdxDataReadMap;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadmap");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadMap = (gdxDataReadMap_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadMap_t));
                    else
                    {
                        symName = "gdxDataReadMap"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4 };
                if (xcheck("gdxDataReadMapStart", 2, s, ref errBuf) == 0)
                    dll_gdxDataReadMapStart = d_gdxDataReadMapStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadmapstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadMapStart = (gdxDataReadMapStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadMapStart_t));
                    else
                    {
                        symName = "gdxDataReadMapStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 52, 54, 4 };
                if (xcheck("gdxDataReadRaw", 3, s, ref errBuf) == 0)
                    dll_gdxDataReadRaw = d_gdxDataReadRaw;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadraw");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadRaw = (gdxDataReadRaw_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadRaw_t));
                    else
                    {
                        symName = "gdxDataReadRaw"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 59, 4 };
                if (xcheck("gdxDataReadRawFast", 3, s, ref errBuf) == 0)
                    dll_gdxDataReadRawFast = d_gdxDataReadRawFast;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadrawfast");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadRawFast = (gdxDataReadRawFast_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadRawFast_t));
                    else
                    {
                        symName = "gdxDataReadRawFast"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 55, 59 };
                if (xcheck("gdxDataReadRawFastFilt", 3, s, ref errBuf) == 0)
                    dll_gdxDataReadRawFastFilt = d_gdxDataReadRawFastFilt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxdatareadrawfastfilt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadRawFastFilt = (gdxDataReadRawFastFilt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadRawFastFilt_t));
                    else
                    {
                        symName = "cgdxDataReadRawFastFilt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4 };
                if (xcheck("gdxDataReadRawStart", 2, s, ref errBuf) == 0)
                    dll_gdxDataReadRawStart = d_gdxDataReadRawStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadrawstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadRawStart = (gdxDataReadRawStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadRawStart_t));
                    else
                    {
                        symName = "gdxDataReadRawStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 55, 4, 59 };
                if (xcheck("gdxDataReadSlice", 3, s, ref errBuf) == 0)
                    dll_gdxDataReadSlice = d_gdxDataReadSlice;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxdatareadslice");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadSlice = (gdxDataReadSlice_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadSlice_t));
                    else
                    {
                        symName = "cgdxDataReadSlice"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 52 };
                if (xcheck("gdxDataReadSliceStart", 2, s, ref errBuf) == 0)
                    dll_gdxDataReadSliceStart = d_gdxDataReadSliceStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadslicestart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadSliceStart = (gdxDataReadSliceStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadSliceStart_t));
                    else
                    {
                        symName = "gdxDataReadSliceStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 56, 54, 4 };
                if (xcheck("gdxDataReadStr", 3, s, ref errBuf) == 0)
                    dll_gdxDataReadStr = d_gdxDataReadStr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "bgdxdatareadstr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadStr = (gdxDataReadStr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadStr_t));
                    else
                    {
                        symName = "bgdxDataReadStr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4 };
                if (xcheck("gdxDataReadStrStart", 2, s, ref errBuf) == 0)
                    dll_gdxDataReadStrStart = d_gdxDataReadStrStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatareadstrstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataReadStrStart = (gdxDataReadStrStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataReadStrStart_t));
                    else
                    {
                        symName = "gdxDataReadStrStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 51, 56 };
                if (xcheck("gdxDataSliceUELS", 2, s, ref errBuf) == 0)
                    dll_gdxDataSliceUELS = d_gdxDataSliceUELS;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "bgdxdatasliceuels");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataSliceUELS = (gdxDataSliceUELS_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataSliceUELS_t));
                    else
                    {
                        symName = "bgdxDataSliceUELS"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxDataWriteDone", 0, s, ref errBuf) == 0)
                    dll_gdxDataWriteDone = d_gdxDataWriteDone;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatawritedone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteDone = (gdxDataWriteDone_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteDone_t));
                    else
                    {
                        symName = "gdxDataWriteDone"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 51, 53 };
                if (xcheck("gdxDataWriteMap", 2, s, ref errBuf) == 0)
                    dll_gdxDataWriteMap = d_gdxDataWriteMap;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatawritemap");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteMap = (gdxDataWriteMap_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteMap_t));
                    else
                    {
                        symName = "gdxDataWriteMap"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 3, 3, 3 };
                if (xcheck("gdxDataWriteMapStart", 5, s, ref errBuf) == 0)
                    dll_gdxDataWriteMapStart = d_gdxDataWriteMapStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxdatawritemapstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteMapStart = (gdxDataWriteMapStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteMapStart_t));
                    else
                    {
                        symName = "cgdxDataWriteMapStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 51, 53 };
                if (xcheck("gdxDataWriteRaw", 2, s, ref errBuf) == 0)
                    dll_gdxDataWriteRaw = d_gdxDataWriteRaw;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxdatawriteraw");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteRaw = (gdxDataWriteRaw_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteRaw_t));
                    else
                    {
                        symName = "gdxDataWriteRaw"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 3, 3, 3 };
                if (xcheck("gdxDataWriteRawStart", 5, s, ref errBuf) == 0)
                    dll_gdxDataWriteRawStart = d_gdxDataWriteRawStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxdatawriterawstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteRawStart = (gdxDataWriteRawStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteRawStart_t));
                    else
                    {
                        symName = "cgdxDataWriteRawStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 55, 53 };
                if (xcheck("gdxDataWriteStr", 2, s, ref errBuf) == 0)
                    dll_gdxDataWriteStr = d_gdxDataWriteStr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxdatawritestr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteStr = (gdxDataWriteStr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteStr_t));
                    else
                    {
                        symName = "cgdxDataWriteStr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 3, 3, 3 };
                if (xcheck("gdxDataWriteStrStart", 5, s, ref errBuf) == 0)
                    dll_gdxDataWriteStrStart = d_gdxDataWriteStrStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxdatawritestrstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxDataWriteStrStart = (gdxDataWriteStrStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxDataWriteStrStart_t));
                    else
                    {
                        symName = "cgdxDataWriteStrStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 12 };
                if (xcheck("gdxGetDLLVersion", 1, s, ref errBuf) == 0)
                    dll_gdxGetDLLVersion = d_gdxGetDLLVersion;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxgetdllversion");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetDLLVersion = (gdxGetDLLVersion_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetDLLVersion_t));
                    else
                    {
                        symName = "cgdxGetDLLVersion"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxErrorCount", 0, s, ref errBuf) == 0)
                    dll_gdxErrorCount = d_gdxErrorCount;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxerrorcount");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxErrorCount = (gdxErrorCount_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxErrorCount_t));
                    else
                    {
                        symName = "gdxErrorCount"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12 };
                if (xcheck("gdxErrorStr", 2, s, ref errBuf) == 0)
                    dll_gdxErrorStr = d_gdxErrorStr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxerrorstr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxErrorStr = (gdxErrorStr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxErrorStr_t));
                    else
                    {
                        symName = "cgdxErrorStr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 4, 4 };
                if (xcheck("gdxFileInfo", 2, s, ref errBuf) == 0)
                    dll_gdxFileInfo = d_gdxFileInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxfileinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFileInfo = (gdxFileInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFileInfo_t));
                    else
                    {
                        symName = "gdxFileInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 12, 12 };
                if (xcheck("gdxFileVersion", 2, s, ref errBuf) == 0)
                    dll_gdxFileVersion = d_gdxFileVersion;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxfileversion");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFileVersion = (gdxFileVersion_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFileVersion_t));
                    else
                    {
                        symName = "cgdxFileVersion"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxFilterExists", 1, s, ref errBuf) == 0)
                    dll_gdxFilterExists = d_gdxFilterExists;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxfilterexists");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFilterExists = (gdxFilterExists_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFilterExists_t));
                    else
                    {
                        symName = "gdxFilterExists"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxFilterRegister", 1, s, ref errBuf) == 0)
                    dll_gdxFilterRegister = d_gdxFilterRegister;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxfilterregister");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFilterRegister = (gdxFilterRegister_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFilterRegister_t));
                    else
                    {
                        symName = "gdxFilterRegister"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxFilterRegisterDone", 0, s, ref errBuf) == 0)
                    dll_gdxFilterRegisterDone = d_gdxFilterRegisterDone;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxfilterregisterdone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFilterRegisterDone = (gdxFilterRegisterDone_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFilterRegisterDone_t));
                    else
                    {
                        symName = "gdxFilterRegisterDone"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxFilterRegisterStart", 1, s, ref errBuf) == 0)
                    dll_gdxFilterRegisterStart = d_gdxFilterRegisterStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxfilterregisterstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFilterRegisterStart = (gdxFilterRegisterStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFilterRegisterStart_t));
                    else
                    {
                        symName = "gdxFilterRegisterStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 4 };
                if (xcheck("gdxFindSymbol", 2, s, ref errBuf) == 0)
                    dll_gdxFindSymbol = d_gdxFindSymbol;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxfindsymbol");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxFindSymbol = (gdxFindSymbol_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxFindSymbol_t));
                    else
                    {
                        symName = "cgdxFindSymbol"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12, 4 };
                if (xcheck("gdxGetElemText", 3, s, ref errBuf) == 0)
                    dll_gdxGetElemText = d_gdxGetElemText;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxgetelemtext");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetElemText = (gdxGetElemText_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetElemText_t));
                    else
                    {
                        symName = "cgdxGetElemText"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxGetLastError", 0, s, ref errBuf) == 0)
                    dll_gdxGetLastError = d_gdxGetLastError;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxgetlasterror");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetLastError = (gdxGetLastError_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetLastError_t));
                    else
                    {
                        symName = "gdxGetLastError"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 23 };
                if (xcheck("gdxGetMemoryUsed", 0, s, ref errBuf) == 0)
                    dll_gdxGetMemoryUsed = d_gdxGetMemoryUsed;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxgetmemoryused");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetMemoryUsed = (gdxGetMemoryUsed_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetMemoryUsed_t));
                    else
                    {
                        symName = "gdxGetMemoryUsed"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 58 };
                if (xcheck("gdxGetSpecialValues", 1, s, ref errBuf) == 0)
                    dll_gdxGetSpecialValues = d_gdxGetSpecialValues;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxgetspecialvalues");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetSpecialValues = (gdxGetSpecialValues_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetSpecialValues_t));
                    else
                    {
                        symName = "gdxGetSpecialValues"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12 };
                if (xcheck("gdxGetUEL", 2, s, ref errBuf) == 0)
                    dll_gdxGetUEL = d_gdxGetUEL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxgetuel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetUEL = (gdxGetUEL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetUEL_t));
                    else
                    {
                        symName = "cgdxGetUEL"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 13, 4 };
                if (xcheck("gdxMapValue", 2, s, ref errBuf) == 0)
                    dll_gdxMapValue = d_gdxMapValue;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxmapvalue");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxMapValue = (gdxMapValue_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxMapValue_t));
                    else
                    {
                        symName = "gdxMapValue"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 4 };
                if (xcheck("gdxOpenAppend", 3, s, ref errBuf) == 0)
                    dll_gdxOpenAppend = d_gdxOpenAppend;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxopenappend");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxOpenAppend = (gdxOpenAppend_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxOpenAppend_t));
                    else
                    {
                        symName = "cgdxOpenAppend"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 4 };
                if (xcheck("gdxOpenRead", 2, s, ref errBuf) == 0)
                    dll_gdxOpenRead = d_gdxOpenRead;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxopenread");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxOpenRead = (gdxOpenRead_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxOpenRead_t));
                    else
                    {
                        symName = "cgdxOpenRead"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 4 };
                if (xcheck("gdxOpenWrite", 3, s, ref errBuf) == 0)
                    dll_gdxOpenWrite = d_gdxOpenWrite;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxopenwrite");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxOpenWrite = (gdxOpenWrite_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxOpenWrite_t));
                    else
                    {
                        symName = "cgdxOpenWrite"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 3, 4 };
                if (xcheck("gdxOpenWriteEx", 4, s, ref errBuf) == 0)
                    dll_gdxOpenWriteEx = d_gdxOpenWriteEx;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxopenwriteex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxOpenWriteEx = (gdxOpenWriteEx_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxOpenWriteEx_t));
                    else
                    {
                        symName = "cgdxOpenWriteEx"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxResetSpecialValues", 0, s, ref errBuf) == 0)
                    dll_gdxResetSpecialValues = d_gdxResetSpecialValues;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxresetspecialvalues");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxResetSpecialValues = (gdxResetSpecialValues_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxResetSpecialValues_t));
                    else
                    {
                        symName = "gdxResetSpecialValues"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxSetHasText", 1, s, ref errBuf) == 0)
                    dll_gdxSetHasText = d_gdxSetHasText;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsethastext");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSetHasText = (gdxSetHasText_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSetHasText_t));
                    else
                    {
                        symName = "gdxSetHasText"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 57 };
                if (xcheck("gdxSetReadSpecialValues", 1, s, ref errBuf) == 0)
                    dll_gdxSetReadSpecialValues = d_gdxSetReadSpecialValues;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsetreadspecialvalues");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSetReadSpecialValues = (gdxSetReadSpecialValues_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSetReadSpecialValues_t));
                    else
                    {
                        symName = "gdxSetReadSpecialValues"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 57 };
                if (xcheck("gdxSetSpecialValues", 1, s, ref errBuf) == 0)
                    dll_gdxSetSpecialValues = d_gdxSetSpecialValues;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsetspecialvalues");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSetSpecialValues = (gdxSetSpecialValues_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSetSpecialValues_t));
                    else
                    {
                        symName = "gdxSetSpecialValues"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 3 };
                if (xcheck("gdxSetTextNodeNr", 2, s, ref errBuf) == 0)
                    dll_gdxSetTextNodeNr = d_gdxSetTextNodeNr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsettextnodenr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSetTextNodeNr = (gdxSetTextNodeNr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSetTextNodeNr_t));
                    else
                    {
                        symName = "gdxSetTextNodeNr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 11 };
                if (xcheck("gdxSetTraceLevel", 2, s, ref errBuf) == 0)
                    dll_gdxSetTraceLevel = d_gdxSetTraceLevel;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsettracelevel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSetTraceLevel = (gdxSetTraceLevel_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSetTraceLevel_t));
                    else
                    {
                        symName = "cgdxSetTraceLevel"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 52 };
                if (xcheck("gdxSymbIndxMaxLength", 2, s, ref errBuf) == 0)
                    dll_gdxSymbIndxMaxLength = d_gdxSymbIndxMaxLength;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsymbindxmaxlength");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbIndxMaxLength = (gdxSymbIndxMaxLength_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbIndxMaxLength_t));
                    else
                    {
                        symName = "gdxSymbIndxMaxLength"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxSymbMaxLength", 0, s, ref errBuf) == 0)
                    dll_gdxSymbMaxLength = d_gdxSymbMaxLength;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsymbmaxlength");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbMaxLength = (gdxSymbMaxLength_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbMaxLength_t));
                    else
                    {
                        symName = "gdxSymbMaxLength"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 11 };
                if (xcheck("gdxSymbolAddComment", 2, s, ref errBuf) == 0)
                    dll_gdxSymbolAddComment = d_gdxSymbolAddComment;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsymboladdcomment");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolAddComment = (gdxSymbolAddComment_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolAddComment_t));
                    else
                    {
                        symName = "cgdxSymbolAddComment"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 3, 12 };
                if (xcheck("gdxSymbolGetComment", 3, s, ref errBuf) == 0)
                    dll_gdxSymbolGetComment = d_gdxSymbolGetComment;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsymbolgetcomment");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolGetComment = (gdxSymbolGetComment_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolGetComment_t));
                    else
                    {
                        symName = "cgdxSymbolGetComment"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 52 };
                if (xcheck("gdxSymbolGetDomain", 2, s, ref errBuf) == 0)
                    dll_gdxSymbolGetDomain = d_gdxSymbolGetDomain;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsymbolgetdomain");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolGetDomain = (gdxSymbolGetDomain_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolGetDomain_t));
                    else
                    {
                        symName = "gdxSymbolGetDomain"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 56 };
                if (xcheck("gdxSymbolGetDomainX", 2, s, ref errBuf) == 0)
                    dll_gdxSymbolGetDomainX = d_gdxSymbolGetDomainX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "bgdxsymbolgetdomainx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolGetDomainX = (gdxSymbolGetDomainX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolGetDomainX_t));
                    else
                    {
                        symName = "bgdxSymbolGetDomainX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (xcheck("gdxSymbolDim", 1, s, ref errBuf) == 0)
                    dll_gdxSymbolDim = d_gdxSymbolDim;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsymboldim");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolDim = (gdxSymbolDim_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolDim_t));
                    else
                    {
                        symName = "gdxSymbolDim"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12, 4, 4 };
                if (xcheck("gdxSymbolInfo", 4, s, ref errBuf) == 0)
                    dll_gdxSymbolInfo = d_gdxSymbolInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsymbolinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolInfo = (gdxSymbolInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolInfo_t));
                    else
                    {
                        symName = "cgdxSymbolInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4, 4, 12 };
                if (xcheck("gdxSymbolInfoX", 4, s, ref errBuf) == 0)
                    dll_gdxSymbolInfoX = d_gdxSymbolInfoX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsymbolinfox");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolInfoX = (gdxSymbolInfoX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolInfoX_t));
                    else
                    {
                        symName = "cgdxSymbolInfoX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 55 };
                if (xcheck("gdxSymbolSetDomain", 1, s, ref errBuf) == 0)
                    dll_gdxSymbolSetDomain = d_gdxSymbolSetDomain;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsymbolsetdomain");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolSetDomain = (gdxSymbolSetDomain_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolSetDomain_t));
                    else
                    {
                        symName = "cgdxSymbolSetDomain"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 55 };
                if (xcheck("gdxSymbolSetDomainX", 2, s, ref errBuf) == 0)
                    dll_gdxSymbolSetDomainX = d_gdxSymbolSetDomainX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxsymbolsetdomainx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSymbolSetDomainX = (gdxSymbolSetDomainX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSymbolSetDomainX_t));
                    else
                    {
                        symName = "cgdxSymbolSetDomainX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 4, 4 };
                if (xcheck("gdxSystemInfo", 2, s, ref errBuf) == 0)
                    dll_gdxSystemInfo = d_gdxSystemInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxsysteminfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxSystemInfo = (gdxSystemInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxSystemInfo_t));
                    else
                    {
                        symName = "gdxSystemInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxUELMaxLength", 0, s, ref errBuf) == 0)
                    dll_gdxUELMaxLength = d_gdxUELMaxLength;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxuelmaxlength");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELMaxLength = (gdxUELMaxLength_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELMaxLength_t));
                    else
                    {
                        symName = "gdxUELMaxLength"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxUELRegisterDone", 0, s, ref errBuf) == 0)
                    dll_gdxUELRegisterDone = d_gdxUELRegisterDone;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxuelregisterdone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterDone = (gdxUELRegisterDone_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterDone_t));
                    else
                    {
                        symName = "gdxUELRegisterDone"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 11 };
                if (xcheck("gdxUELRegisterMap", 2, s, ref errBuf) == 0)
                    dll_gdxUELRegisterMap = d_gdxUELRegisterMap;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxuelregistermap");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterMap = (gdxUELRegisterMap_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterMap_t));
                    else
                    {
                        symName = "cgdxUELRegisterMap"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxUELRegisterMapStart", 0, s, ref errBuf) == 0)
                    dll_gdxUELRegisterMapStart = d_gdxUELRegisterMapStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxuelregistermapstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterMapStart = (gdxUELRegisterMapStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterMapStart_t));
                    else
                    {
                        symName = "gdxUELRegisterMapStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11 };
                if (xcheck("gdxUELRegisterRaw", 1, s, ref errBuf) == 0)
                    dll_gdxUELRegisterRaw = d_gdxUELRegisterRaw;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxuelregisterraw");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterRaw = (gdxUELRegisterRaw_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterRaw_t));
                    else
                    {
                        symName = "cgdxUELRegisterRaw"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxUELRegisterRawStart", 0, s, ref errBuf) == 0)
                    dll_gdxUELRegisterRawStart = d_gdxUELRegisterRawStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxuelregisterrawstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterRawStart = (gdxUELRegisterRawStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterRawStart_t));
                    else
                    {
                        symName = "gdxUELRegisterRawStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 4 };
                if (xcheck("gdxUELRegisterStr", 2, s, ref errBuf) == 0)
                    dll_gdxUELRegisterStr = d_gdxUELRegisterStr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxuelregisterstr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterStr = (gdxUELRegisterStr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterStr_t));
                    else
                    {
                        symName = "cgdxUELRegisterStr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxUELRegisterStrStart", 0, s, ref errBuf) == 0)
                    dll_gdxUELRegisterStrStart = d_gdxUELRegisterStrStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxuelregisterstrstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUELRegisterStrStart = (gdxUELRegisterStrStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUELRegisterStrStart_t));
                    else
                    {
                        symName = "gdxUELRegisterStrStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 4, 4 };
                if (xcheck("gdxUMFindUEL", 3, s, ref errBuf) == 0)
                    dll_gdxUMFindUEL = d_gdxUMFindUEL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxumfinduel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUMFindUEL = (gdxUMFindUEL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUMFindUEL_t));
                    else
                    {
                        symName = "cgdxUMFindUEL"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12, 4 };
                if (xcheck("gdxUMUelGet", 3, s, ref errBuf) == 0)
                    dll_gdxUMUelGet = d_gdxUMUelGet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxumuelget");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUMUelGet = (gdxUMUelGet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUMUelGet_t));
                    else
                    {
                        symName = "cgdxUMUelGet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 4, 4 };
                if (xcheck("gdxUMUelInfo", 2, s, ref errBuf) == 0)
                    dll_gdxUMUelInfo = d_gdxUMUelInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxumuelinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxUMUelInfo = (gdxUMUelInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxUMUelInfo_t));
                    else
                    {
                        symName = "gdxUMUelInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 3, 3, 59, 4, 1 };
                if (xcheck("gdxGetDomainElements", 6, s, ref errBuf) == 0)
                    dll_gdxGetDomainElements = d_gdxGetDomainElements;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxgetdomainelements");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxGetDomainElements = (gdxGetDomainElements_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxGetDomainElements_t));
                    else
                    {
                        symName = "gdxGetDomainElements"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (xcheck("gdxCurrentDim", 0, s, ref errBuf) == 0)
                    dll_gdxCurrentDim = d_gdxCurrentDim;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gdxcurrentdim");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxCurrentDim = (gdxCurrentDim_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxCurrentDim_t));
                    else
                    {
                        symName = "gdxCurrentDim"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11 };
                if (xcheck("gdxRenameUEL", 2, s, ref errBuf) == 0)
                    dll_gdxRenameUEL = d_gdxRenameUEL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgdxrenameuel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gdxRenameUEL = (gdxRenameUEL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gdxRenameUEL_t));
                    else
                    {
                        symName = "cgdxRenameUEL"; goto symMissing;
                    }
                }
            }

            return true;

        symMissing:
            errBuf = "Could not load symbol '" + symName + "'";
            return false;

        } /* XLibraryLoad */

        private bool libloader(string dllPath, string dllName, ref string msgBuf)
        {
#if __MonoCS__
#if __APPLE__
        const string libStem = "libgdxdclib", libExt = ".dylib";
#else
        const string libStem = "libgdxdclib", libExt = ".so";
#endif
#else
            const string libStem = "gdxdclib", libExt = ".dll";
#endif
            string dllNameBuf = string.Empty;
            int myrc = 0;
            string GMS_DLL_SUFFIX = string.Empty;

            msgBuf = string.Empty;
            if (!isLoaded)
            {
                if (string.Empty != dllPath)
                {
                    dllNameBuf = dllPath;
                    if (Path.DirectorySeparatorChar != dllNameBuf[dllNameBuf.Length - 1]) dllNameBuf = dllNameBuf + Path.DirectorySeparatorChar;
                }
                if (string.Empty != dllName)
                    dllNameBuf = dllNameBuf + dllName;
                else
                {
                    if (8 == IntPtr.Size)
                        GMS_DLL_SUFFIX = "64";
                    dllNameBuf = dllNameBuf + libStem + GMS_DLL_SUFFIX + libExt;
                }
                isLoaded = XLibraryLoad(dllNameBuf, ref msgBuf);
                if (isLoaded)
                {
                    if (null != dll_gdxSetLoadPath && string.Empty != dllPath)
                    {
                        gdxSetLoadPath(dllPath);
                    }
                    else
                    {                            /* no setLoadPath call found */
                        myrc |= 2;
                    }
                }
                else                          /* library load failed */
                    myrc |= 1;
            }
            return (myrc & 1) == 0;
        } /* libloader */

        public bool gdxGetReady(ref string msgBuf)
        {
            return libloader(string.Empty, string.Empty, ref msgBuf);
        }
        public bool gdxGetReadyD(string dirName, ref string msgBuf)
        {
            return libloader(dirName, string.Empty, ref msgBuf);
        }
        public bool gdxGetReadyL(string dirName, string libName, ref string msgBuf)
        {
            return libloader(dirName, libName, ref msgBuf);
        }

        public gdxcs(ref string msgBuf)
        {
            bool gdxIsReady;

            extHandle = false;
            _disposed = false;
            gdxIsReady = gdxGetReady(ref msgBuf);
            if (!gdxIsReady) return;
            xcreate(ref pgdx);
            if (pgdx != IntPtr.Zero)
            {
                msgBuf = String.Empty;
                return;
            }
            msgBuf = "Error while creating object";
        }
        public gdxcs(string dirName, ref string msgBuf)
        {
            bool gdxIsReady;

            extHandle = false;
            _disposed = false;
            gdxIsReady = gdxGetReadyD(dirName, ref msgBuf);
            if (!gdxIsReady) return;
            xcreate(ref pgdx);
            if (pgdx != IntPtr.Zero)
            {
                msgBuf = String.Empty;
                return;
            }
            msgBuf = "Error while creating object";
        }
        public gdxcs(string dirName, string libName, ref string msgBuf)
        {
            bool gdxIsReady;

            extHandle = false;
            _disposed = false;
            gdxIsReady = gdxGetReadyL(dirName, libName, ref msgBuf);
            if (!gdxIsReady) return;
            xcreate(ref pgdx);
            if (pgdx != IntPtr.Zero)
            {
                msgBuf = String.Empty;
                return;
            }
            msgBuf = "Error while creating object";
        }
        public gdxcs(IntPtr gdxHandle, ref string msgBuf)
        {
            bool gdxIsReady;

            if (gdxHandle == IntPtr.Zero)
            {
                msgBuf = "gdxHandle is empty";
                return;
            }
            extHandle = true;
            _disposed = false;
            gdxIsReady = gdxGetReady(ref msgBuf);
            if (!gdxIsReady) return;
            pgdx = gdxHandle;
        }
        public gdxcs(IntPtr gdxHandle, string dirName, ref string msgBuf)
        {
            bool gdxIsReady;

            if (gdxHandle == IntPtr.Zero)
            {
                msgBuf = "gdxHandle is empty";
                return;
            }
            extHandle = true;
            _disposed = false;
            gdxIsReady = gdxGetReadyD(dirName, ref msgBuf);
            if (!gdxIsReady) return;
            pgdx = gdxHandle;
        }

        ~gdxcs()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (pgdx != IntPtr.Zero)
                        gdxFree();
                }
                // Indicate that the instance has been disposed.
                _disposed = true;
            }
            GC.KeepAlive(this);
        }

        public int gdxFree()
        {
            if (!extHandle && pgdx != IntPtr.Zero) xfree(ref pgdx);
            return 1;
        }

        public bool gdxLibraryUnload()
        {
            return FreeLibrary(h);
        }

        public IntPtr GetgdxPtr()
        {
            return pgdx;
        }

        public bool gdxGetScreenIndicator()
        {
            return ScreenIndicator;
        }

        public void gdxSetScreenIndicator(bool scrind)
        {
            ScreenIndicator = scrind;
        }

        public bool gdxGetExceptionIndicator()
        {
            return ExceptionIndicator;
        }

        public void gdxSetExceptionIndicator(bool excind)
        {
            ExceptionIndicator = excind;
        }

        public bool gdxGetExitIndicator()
        {
            return ExitIndicator;
        }

        public void gdxSetExitIndicator(bool extind)
        {
            ExitIndicator = extind;
        }

        public gdxErrorCallback_t gdxGetErrorCallback()
        {
            return ErrorCallBack;
        }

        public void gdxSetErrorCallback(gdxErrorCallback_t func)
        {
            ErrorCallBack = func;
        }

        public int gdxGetAPIErrorCount()
        {
            return APIErrorCount;
        }

        public void gdxSetAPIErrorCount(int ecnt)
        {
            APIErrorCount = ecnt;
        }

        private static void gdxErrorHandling(string Msg)
        {
            APIErrorCount++;
            if (ScreenIndicator) Console.WriteLine(Msg);
            if (ErrorCallBack != null)
                if (ErrorCallBack(APIErrorCount, Msg)) Environment.Exit(123);
            if (ExceptionIndicator) throw new ArgumentNullException();
            if (ExitIndicator) Environment.Exit(123);
        }

        private void ConvertC2CS(byte[] b, ref string s)
        {
            int i;
            s = "";
            i = 0;
            while (b[i] != 0)
            {
                s = s + (char)(b[i]);
                i = i + 1;
            }
        }

        private void ConvertArrayC2CS(byte[,] b, ref string s, int k)
        {
            int i;
            s = "";
            i = 0;
            while (b[k, i] != 0)
            {
                s = s + (char)(b[k, i]);
                i = i + 1;
            }
        }

        private int xapiversion(int api, ref string msg, ref int cl)
        {
            int rc_xapiversion;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_xapiversion = dll_xapiversion(api, cpy_msg, ref cl);
            msg = cpy_msg.ToString();
            return rc_xapiversion;
        }

        private int xcheck(string ep, int nargs, int[] s, ref string msg)
        {
            int rc_xcheck;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_xcheck = dll_xcheck(ep, nargs, s, cpy_msg);
            msg = cpy_msg.ToString();
            return rc_xcheck;
        }

        public void gdxSetLoadPath(string s)
        {
            dll_gdxSetLoadPath(s);
        }

        public void gdxGetLoadPath(ref string s)
        {
            byte[] cpy_s = new byte[gamsglobals.str_len];
            dll_gdxGetLoadPath(ref cpy_s[0]);
            ConvertC2CS(cpy_s, ref s);
        }

        public int gdxAcronymAdd(string AName, string Txt, int AIndx)
        {
            return dll_gdxAcronymAdd(pgdx, AName, Txt, AIndx);
        }

        public int gdxAcronymCount()
        {
            return dll_gdxAcronymCount(pgdx);
        }

        public int gdxAcronymGetInfo(int N, ref string AName, ref string Txt, ref int AIndx)
        {
            int rc_gdxAcronymGetInfo;
            StringBuilder cpy_AName = new StringBuilder(gamsglobals.str_len);
            StringBuilder cpy_Txt = new StringBuilder(gamsglobals.str_len);
            rc_gdxAcronymGetInfo = dll_gdxAcronymGetInfo(pgdx, N, cpy_AName, cpy_Txt, ref AIndx);
            AName = cpy_AName.ToString();
            Txt = cpy_Txt.ToString();
            return rc_gdxAcronymGetInfo;
        }

        public int gdxAcronymGetMapping(int N, ref int orgIndx, ref int newIndx, ref int autoIndex)
        {
            return dll_gdxAcronymGetMapping(pgdx, N, ref orgIndx, ref newIndx, ref autoIndex);
        }

        public int gdxAcronymIndex(double V)
        {
            return dll_gdxAcronymIndex(pgdx, V);
        }

        public int gdxAcronymName(double V, ref string AName)
        {
            int rc_gdxAcronymName;
            StringBuilder cpy_AName = new StringBuilder(gamsglobals.str_len);
            rc_gdxAcronymName = dll_gdxAcronymName(pgdx, V, cpy_AName);
            AName = cpy_AName.ToString();
            return rc_gdxAcronymName;
        }

        public int gdxAcronymNextNr(int NV)
        {
            return dll_gdxAcronymNextNr(pgdx, NV);
        }

        public int gdxAcronymSetInfo(int N, string AName, string Txt, int AIndx)
        {
            return dll_gdxAcronymSetInfo(pgdx, N, AName, Txt, AIndx);
        }

        public double gdxAcronymValue(int AIndx)
        {
            return dll_gdxAcronymValue(pgdx, AIndx);
        }

        public int gdxAddAlias(string Id1, string Id2)
        {
            return dll_gdxAddAlias(pgdx, Id1, Id2);
        }

        public int gdxAddSetText(string Txt, ref int TxtNr)
        {
            return dll_gdxAddSetText(pgdx, Txt, ref TxtNr);
        }

        public int gdxAutoConvert(int NV)
        {
            return dll_gdxAutoConvert(pgdx, NV);
        }

        public int gdxClose()
        {
            return dll_gdxClose(pgdx);
        }

        public int gdxDataErrorCount()
        {
            return dll_gdxDataErrorCount(pgdx);
        }

        public int gdxDataErrorRecord(int RecNr, ref int[] KeyInt, ref double[] Values)
        {
            return dll_gdxDataErrorRecord(pgdx, RecNr, KeyInt, Values);
        }

        public int gdxDataErrorRecordX(int RecNr, ref int[] KeyInt, ref double[] Values)
        {
            return dll_gdxDataErrorRecordX(pgdx, RecNr, KeyInt, Values);
        }

        public int gdxDataReadDone()
        {
            return dll_gdxDataReadDone(pgdx);
        }

        public int gdxDataReadFilteredStart(int SyNr, int[] FilterAction, ref int NrRecs)
        {
            return dll_gdxDataReadFilteredStart(pgdx, SyNr, FilterAction, ref NrRecs);
        }

        public int gdxDataReadMap(int RecNr, ref int[] KeyInt, ref double[] Values, ref int DimFrst)
        {
            return dll_gdxDataReadMap(pgdx, RecNr, KeyInt, Values, ref DimFrst);
        }

        public int gdxDataReadMapStart(int SyNr, ref int NrRecs)
        {
            return dll_gdxDataReadMapStart(pgdx, SyNr, ref NrRecs);
        }

        public int gdxDataReadRaw(ref int[] KeyInt, ref double[] Values, ref int DimFrst)
        {
            return dll_gdxDataReadRaw(pgdx, KeyInt, Values, ref DimFrst);
        }

        public int gdxDataReadRawFast(int SyNr, TDataStoreProc DP, ref int NrRecs)
        {
            return dll_gdxDataReadRawFast(pgdx, SyNr, DP, ref NrRecs);
        }

        public int gdxDataReadRawFastFilt(int SyNr, string[] UelFilterStr, TDataStoreFiltProc DP)
        {
            return dll_gdxDataReadRawFastFilt(pgdx, SyNr, UelFilterStr, DP);
        }

        public int gdxDataReadRawStart(int SyNr, ref int NrRecs)
        {
            return dll_gdxDataReadRawStart(pgdx, SyNr, ref NrRecs);
        }

        public int gdxDataReadSlice(string[] UelFilterStr, ref int Dimen, TDataStoreProc DP)
        {
            return dll_gdxDataReadSlice(pgdx, UelFilterStr, ref Dimen, DP);
        }

        public int gdxDataReadSliceStart(int SyNr, ref int[] ElemCounts)
        {
            return dll_gdxDataReadSliceStart(pgdx, SyNr, ElemCounts);
        }

        public int gdxDataReadStr(ref string[] KeyStr, ref double[] Values, ref int DimFrst)
        {
            int rc_gdxDataReadStr;
            byte[,] cpy_KeyStr = new byte[gamsglobals.maxdim, gamsglobals.str_len];
            int i_KeyStr;
            int sidim_KeyStr;
            rc_gdxDataReadStr = dll_gdxDataReadStr(pgdx, cpy_KeyStr, Values, ref DimFrst);
            sidim_KeyStr = dll_gdxCurrentDim(pgdx);
            if (rc_gdxDataReadStr != 0)
                for (i_KeyStr = 0; i_KeyStr < sidim_KeyStr; i_KeyStr++)
                    ConvertArrayC2CS(cpy_KeyStr, ref KeyStr[i_KeyStr], i_KeyStr);
            return rc_gdxDataReadStr;
        }

        public int gdxDataReadStrStart(int SyNr, ref int NrRecs)
        {
            return dll_gdxDataReadStrStart(pgdx, SyNr, ref NrRecs);
        }

        public int gdxDataSliceUELS(int[] SliceKeyInt, ref string[] KeyStr)
        {
            int rc_gdxDataSliceUELS;
            byte[,] cpy_KeyStr = new byte[gamsglobals.maxdim, gamsglobals.str_len];
            int i_KeyStr;
            int sidim_KeyStr;
            rc_gdxDataSliceUELS = dll_gdxDataSliceUELS(pgdx, SliceKeyInt, cpy_KeyStr);
            sidim_KeyStr = dll_gdxCurrentDim(pgdx);
            if (rc_gdxDataSliceUELS != 0)
                for (i_KeyStr = 0; i_KeyStr < sidim_KeyStr; i_KeyStr++)
                    ConvertArrayC2CS(cpy_KeyStr, ref KeyStr[i_KeyStr], i_KeyStr);
            return rc_gdxDataSliceUELS;
        }

        public int gdxDataWriteDone()
        {
            return dll_gdxDataWriteDone(pgdx);
        }

        public int gdxDataWriteMap(int[] KeyInt, double[] Values)
        {
            return dll_gdxDataWriteMap(pgdx, KeyInt, Values);
        }

        public int gdxDataWriteMapStart(string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo)
        {
            return dll_gdxDataWriteMapStart(pgdx, SyId, ExplTxt, Dimen, Typ, UserInfo);
        }

        public int gdxDataWriteRaw(int[] KeyInt, double[] Values)
        {
            return dll_gdxDataWriteRaw(pgdx, KeyInt, Values);
        }

        public int gdxDataWriteRawStart(string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo)
        {
            return dll_gdxDataWriteRawStart(pgdx, SyId, ExplTxt, Dimen, Typ, UserInfo);
        }

        public int gdxDataWriteStr(string[] KeyStr, double[] Values)
        {
            return dll_gdxDataWriteStr(pgdx, KeyStr, Values);
        }

        public int gdxDataWriteStrStart(string SyId, string ExplTxt, int Dimen, int Typ, int UserInfo)
        {
            return dll_gdxDataWriteStrStart(pgdx, SyId, ExplTxt, Dimen, Typ, UserInfo);
        }

        public int gdxGetDLLVersion(ref string V)
        {
            int rc_gdxGetDLLVersion;
            StringBuilder cpy_V = new StringBuilder(gamsglobals.str_len);
            rc_gdxGetDLLVersion = dll_gdxGetDLLVersion(pgdx, cpy_V);
            V = cpy_V.ToString();
            return rc_gdxGetDLLVersion;
        }

        public int gdxErrorCount()
        {
            return dll_gdxErrorCount(pgdx);
        }

        public int gdxErrorStr(int ErrNr, ref string ErrMsg)
        {
            int rc_gdxErrorStr;
            StringBuilder cpy_ErrMsg = new StringBuilder(gamsglobals.str_len);
            rc_gdxErrorStr = dll_gdxErrorStr(pgdx, ErrNr, cpy_ErrMsg);
            ErrMsg = cpy_ErrMsg.ToString();
            return rc_gdxErrorStr;
        }

        public int gdxFileInfo(ref int FileVer, ref int ComprLev)
        {
            return dll_gdxFileInfo(pgdx, ref FileVer, ref ComprLev);
        }

        public int gdxFileVersion(ref string FileStr, ref string ProduceStr)
        {
            int rc_gdxFileVersion;
            StringBuilder cpy_FileStr = new StringBuilder(gamsglobals.str_len);
            StringBuilder cpy_ProduceStr = new StringBuilder(gamsglobals.str_len);
            rc_gdxFileVersion = dll_gdxFileVersion(pgdx, cpy_FileStr, cpy_ProduceStr);
            FileStr = cpy_FileStr.ToString();
            ProduceStr = cpy_ProduceStr.ToString();
            return rc_gdxFileVersion;
        }

        public int gdxFilterExists(int FilterNr)
        {
            return dll_gdxFilterExists(pgdx, FilterNr);
        }

        public int gdxFilterRegister(int UelMap)
        {
            return dll_gdxFilterRegister(pgdx, UelMap);
        }

        public int gdxFilterRegisterDone()
        {
            return dll_gdxFilterRegisterDone(pgdx);
        }

        public int gdxFilterRegisterStart(int FilterNr)
        {
            return dll_gdxFilterRegisterStart(pgdx, FilterNr);
        }

        public int gdxFindSymbol(string SyId, ref int SyNr)
        {
            return dll_gdxFindSymbol(pgdx, SyId, ref SyNr);
        }

        public int gdxGetElemText(int TxtNr, ref string Txt, ref int Node)
        {
            int rc_gdxGetElemText;
            StringBuilder cpy_Txt = new StringBuilder(gamsglobals.str_len);
            rc_gdxGetElemText = dll_gdxGetElemText(pgdx, TxtNr, cpy_Txt, ref Node);
            Txt = cpy_Txt.ToString();
            return rc_gdxGetElemText;
        }

        public int gdxGetLastError()
        {
            return dll_gdxGetLastError(pgdx);
        }

        public Int64 gdxGetMemoryUsed()
        {
            return dll_gdxGetMemoryUsed(pgdx);
        }

        public int gdxGetSpecialValues(ref double[] AVals)
        {
            return dll_gdxGetSpecialValues(pgdx, AVals);
        }

        public int gdxGetUEL(int UelNr, ref string Uel)
        {
            int rc_gdxGetUEL;
            StringBuilder cpy_Uel = new StringBuilder(gamsglobals.str_len);
            rc_gdxGetUEL = dll_gdxGetUEL(pgdx, UelNr, cpy_Uel);
            Uel = cpy_Uel.ToString();
            return rc_gdxGetUEL;
        }

        public int gdxMapValue(double D, ref int sv)
        {
            return dll_gdxMapValue(pgdx, D, ref sv);
        }

        public int gdxOpenAppend(string FileName, string Producer, ref int ErrNr)
        {
            return dll_gdxOpenAppend(pgdx, FileName, Producer, ref ErrNr);
        }

        public int gdxOpenRead(string FileName, ref int ErrNr)
        {
            return dll_gdxOpenRead(pgdx, FileName, ref ErrNr);
        }

        public int gdxOpenWrite(string FileName, string Producer, ref int ErrNr)
        {
            return dll_gdxOpenWrite(pgdx, FileName, Producer, ref ErrNr);
        }

        public int gdxOpenWriteEx(string FileName, string Producer, int Compr, ref int ErrNr)
        {
            return dll_gdxOpenWriteEx(pgdx, FileName, Producer, Compr, ref ErrNr);
        }

        public int gdxResetSpecialValues()
        {
            return dll_gdxResetSpecialValues(pgdx);
        }

        public int gdxSetHasText(int SyNr)
        {
            return dll_gdxSetHasText(pgdx, SyNr);
        }

        public int gdxSetReadSpecialValues(double[] AVals)
        {
            return dll_gdxSetReadSpecialValues(pgdx, AVals);
        }

        public int gdxSetSpecialValues(double[] AVals)
        {
            return dll_gdxSetSpecialValues(pgdx, AVals);
        }

        public int gdxSetTextNodeNr(int TxtNr, int Node)
        {
            return dll_gdxSetTextNodeNr(pgdx, TxtNr, Node);
        }

        public int gdxSetTraceLevel(int N, string s)
        {
            return dll_gdxSetTraceLevel(pgdx, N, s);
        }

        public int gdxSymbIndxMaxLength(int SyNr, ref int[] LengthInfo)
        {
            return dll_gdxSymbIndxMaxLength(pgdx, SyNr, LengthInfo);
        }

        public int gdxSymbMaxLength()
        {
            return dll_gdxSymbMaxLength(pgdx);
        }

        public int gdxSymbolAddComment(int SyNr, string Txt)
        {
            return dll_gdxSymbolAddComment(pgdx, SyNr, Txt);
        }

        public int gdxSymbolGetComment(int SyNr, int N, ref string Txt)
        {
            int rc_gdxSymbolGetComment;
            StringBuilder cpy_Txt = new StringBuilder(gamsglobals.str_len);
            rc_gdxSymbolGetComment = dll_gdxSymbolGetComment(pgdx, SyNr, N, cpy_Txt);
            Txt = cpy_Txt.ToString();
            return rc_gdxSymbolGetComment;
        }

        public int gdxSymbolGetDomain(int SyNr, ref int[] DomainSyNrs)
        {
            return dll_gdxSymbolGetDomain(pgdx, SyNr, DomainSyNrs);
        }

        public int gdxSymbolGetDomainX(int SyNr, ref string[] DomainIDs)
        {
            int rc_gdxSymbolGetDomainX;
            byte[,] cpy_DomainIDs = new byte[gamsglobals.maxdim, gamsglobals.str_len];
            int i_DomainIDs;
            int sidim_DomainIDs;
            rc_gdxSymbolGetDomainX = dll_gdxSymbolGetDomainX(pgdx, SyNr, cpy_DomainIDs);
            sidim_DomainIDs = dll_gdxSymbolDim(pgdx, SyNr);
            if (rc_gdxSymbolGetDomainX != 0)
                for (i_DomainIDs = 0; i_DomainIDs < sidim_DomainIDs; i_DomainIDs++)
                    ConvertArrayC2CS(cpy_DomainIDs, ref DomainIDs[i_DomainIDs], i_DomainIDs);
            return rc_gdxSymbolGetDomainX;
        }

        public int gdxSymbolDim(int SyNr)
        {
            return dll_gdxSymbolDim(pgdx, SyNr);
        }

        public int gdxSymbolInfo(int SyNr, ref string SyId, ref int Dimen, ref int Typ)
        {
            int rc_gdxSymbolInfo;
            StringBuilder cpy_SyId = new StringBuilder(gamsglobals.str_len);
            rc_gdxSymbolInfo = dll_gdxSymbolInfo(pgdx, SyNr, cpy_SyId, ref Dimen, ref Typ);
            SyId = cpy_SyId.ToString();
            return rc_gdxSymbolInfo;
        }

        public int gdxSymbolInfoX(int SyNr, ref int RecCnt, ref int UserInfo, ref string ExplTxt)
        {
            int rc_gdxSymbolInfoX;
            StringBuilder cpy_ExplTxt = new StringBuilder(gamsglobals.str_len);
            rc_gdxSymbolInfoX = dll_gdxSymbolInfoX(pgdx, SyNr, ref RecCnt, ref UserInfo, cpy_ExplTxt);
            ExplTxt = cpy_ExplTxt.ToString();
            return rc_gdxSymbolInfoX;
        }

        public int gdxSymbolSetDomain(string[] DomainIDs)
        {
            return dll_gdxSymbolSetDomain(pgdx, DomainIDs);
        }

        public int gdxSymbolSetDomainX(int SyNr, string[] DomainIDs)
        {
            return dll_gdxSymbolSetDomainX(pgdx, SyNr, DomainIDs);
        }

        public int gdxSystemInfo(ref int SyCnt, ref int UelCnt)
        {
            return dll_gdxSystemInfo(pgdx, ref SyCnt, ref UelCnt);
        }

        public int gdxUELMaxLength()
        {
            return dll_gdxUELMaxLength(pgdx);
        }

        public int gdxUELRegisterDone()
        {
            return dll_gdxUELRegisterDone(pgdx);
        }

        public int gdxUELRegisterMap(int UMap, string Uel)
        {
            return dll_gdxUELRegisterMap(pgdx, UMap, Uel);
        }

        public int gdxUELRegisterMapStart()
        {
            return dll_gdxUELRegisterMapStart(pgdx);
        }

        public int gdxUELRegisterRaw(string Uel)
        {
            return dll_gdxUELRegisterRaw(pgdx, Uel);
        }

        public int gdxUELRegisterRawStart()
        {
            return dll_gdxUELRegisterRawStart(pgdx);
        }

        public int gdxUELRegisterStr(string Uel, ref int UelNr)
        {
            return dll_gdxUELRegisterStr(pgdx, Uel, ref UelNr);
        }

        public int gdxUELRegisterStrStart()
        {
            return dll_gdxUELRegisterStrStart(pgdx);
        }

        public int gdxUMFindUEL(string Uel, ref int UelNr, ref int UelMap)
        {
            return dll_gdxUMFindUEL(pgdx, Uel, ref UelNr, ref UelMap);
        }

        public int gdxUMUelGet(int UelNr, ref string Uel, ref int UelMap)
        {
            int rc_gdxUMUelGet;
            StringBuilder cpy_Uel = new StringBuilder(gamsglobals.str_len);
            rc_gdxUMUelGet = dll_gdxUMUelGet(pgdx, UelNr, cpy_Uel, ref UelMap);
            Uel = cpy_Uel.ToString();
            return rc_gdxUMUelGet;
        }

        public int gdxUMUelInfo(ref int UelCnt, ref int HighMap)
        {
            return dll_gdxUMUelInfo(pgdx, ref UelCnt, ref HighMap);
        }

        public int gdxGetDomainElements(int SyNr, int DimPos, int FilterNr, TDomainIndexProc DP, ref int NrElem, IntPtr Uptr)
        {
            return dll_gdxGetDomainElements(pgdx, SyNr, DimPos, FilterNr, DP, ref NrElem, Uptr);
        }

        public int gdxCurrentDim()
        {
            return dll_gdxCurrentDim(pgdx);
        }

        public int gdxRenameUEL(string OldName, string NewName)
        {
            return dll_gdxRenameUEL(pgdx, OldName, NewName);
        }

    }

    // C#  procedure wrapper generated by apiwrapper for GAMS Version 38.3.0
    //
    // GAMS - Loading mechanism for GAMS Expert-Level APIs
    //
    // Copyright (c) 2016-2022 GAMS Software GmbH <support@gams.com>
    // Copyright (c) 2016-2022 GAMS Development Corp. <support@gams.com>
    //
    // Permission is hereby granted, free of charge, to any person obtaining a copy
    // of this software and associated documentation files (the "Software"), to deal
    // in the Software without restriction, including without limitation the rights
    // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    // copies of the Software, and to permit persons to whom the Software is
    // furnished to do so, subject to the following conditions:
    //
    // The above copyright notice and this permission notice shall be included in all
    // copies or substantial portions of the Software.
    //
    // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    // SOFTWARE.

    //Got this from GAMS people, was omitted in the GAMS program folder
    internal class gmomcs : IDisposable
    {
        private IntPtr pgmo;
        private bool extHandle;
        private bool _disposed;

#if __MonoCS__ || __APPLE__
    private delegate IntPtr DelLoadLibrary (string dllName, int flag);
    private delegate IntPtr DelGetProcAddress (IntPtr hModule, string procedureName);
    private delegate bool DelFreeLibrary (IntPtr hModul);

#if __APPLE__
    [DllImport("libdl.dylib")]
    internal static extern IntPtr dlopen(String dllname, int flags);

    [DllImport("libdl.dylib")]
    internal static extern IntPtr dlsym(IntPtr hModule, String procedureName);

    [DllImport("libdl.dylib")] //int
    internal static extern bool dlclose (IntPtr hModul);
#else
    [DllImport("libdl.so")]
    internal static extern IntPtr dlopen(String dllname, int flags);

    [DllImport("libdl.so")]
    internal static extern IntPtr dlsym(IntPtr hModule, String procedureName);

    [DllImport("libdl.so")]
    internal static extern bool dlclose (IntPtr hModul);
#endif

    DelLoadLibrary LoadLibrary = new DelLoadLibrary(dlopen);
    DelGetProcAddress GetProcAddress = new DelGetProcAddress (dlsym);
    DelFreeLibrary FreeLibrary = new DelFreeLibrary (dlclose);
#else
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
#endif


        public const int gmoequ_E = 0;  // gmoEquType
        public const int gmoequ_G = 1;
        public const int gmoequ_L = 2;
        public const int gmoequ_N = 3;
        public const int gmoequ_X = 4;
        public const int gmoequ_C = 5;
        public const int gmoequ_B = 6;

        public const int gmovar_X = 0;  // gmoVarType
        public const int gmovar_B = 1;
        public const int gmovar_I = 2;
        public const int gmovar_S1 = 3;
        public const int gmovar_S2 = 4;
        public const int gmovar_SC = 5;
        public const int gmovar_SI = 6;

        public const int gmoorder_ERR = 0;  // gmoEquOrder
        public const int gmoorder_L = 1;
        public const int gmoorder_Q = 2;
        public const int gmoorder_NL = 3;

        public const int gmovar_X_F = 0;  // gmoVarFreeType
        public const int gmovar_X_N = 1;
        public const int gmovar_X_P = 2;

        public const int gmoBstat_Lower = 0;  // gmoVarEquBasisStatus
        public const int gmoBstat_Upper = 1;
        public const int gmoBstat_Basic = 2;
        public const int gmoBstat_Super = 3;

        public const int gmoCstat_OK = 0;  // gmoVarEquStatus
        public const int gmoCstat_NonOpt = 1;
        public const int gmoCstat_Infeas = 2;
        public const int gmoCstat_UnBnd = 3;

        public const int gmoObjType_Var = 0;  // gmoObjectiveType
        public const int gmoObjType_Fun = 2;

        public const int gmoIFace_Processed = 0;  // gmoInterfaceType
        public const int gmoIFace_Raw = 1;

        public const int gmoObj_Min = 0;  // gmoObjectiveSense
        public const int gmoObj_Max = 1;

        public const int gmoSolveStat_Normal = 1;  // gmoSolverStatus
        public const int gmoSolveStat_Iteration = 2;
        public const int gmoSolveStat_Resource = 3;
        public const int gmoSolveStat_Solver = 4;
        public const int gmoSolveStat_EvalError = 5;
        public const int gmoSolveStat_Capability = 6;
        public const int gmoSolveStat_License = 7;
        public const int gmoSolveStat_User = 8;
        public const int gmoSolveStat_SetupErr = 9;
        public const int gmoSolveStat_SolverErr = 10;
        public const int gmoSolveStat_InternalErr = 11;
        public const int gmoSolveStat_Skipped = 12;
        public const int gmoSolveStat_SystemErr = 13;

        public const int gmoModelStat_OptimalGlobal = 1;  // gmoModelStatus
        public const int gmoModelStat_OptimalLocal = 2;
        public const int gmoModelStat_Unbounded = 3;
        public const int gmoModelStat_InfeasibleGlobal = 4;
        public const int gmoModelStat_InfeasibleLocal = 5;
        public const int gmoModelStat_InfeasibleIntermed = 6;
        public const int gmoModelStat_Feasible = 7;
        public const int gmoModelStat_Integer = 8;
        public const int gmoModelStat_NonIntegerIntermed = 9;
        public const int gmoModelStat_IntegerInfeasible = 10;
        public const int gmoModelStat_LicenseError = 11;
        public const int gmoModelStat_ErrorUnknown = 12;
        public const int gmoModelStat_ErrorNoSolution = 13;
        public const int gmoModelStat_NoSolutionReturned = 14;
        public const int gmoModelStat_SolvedUnique = 15;
        public const int gmoModelStat_Solved = 16;
        public const int gmoModelStat_SolvedSingular = 17;
        public const int gmoModelStat_UnboundedNoSolution = 18;
        public const int gmoModelStat_InfeasibleNoSolution = 19;

        public const int gmoHiterused = 3;  // gmoHeadnTail
        public const int gmoHresused = 4;
        public const int gmoHobjval = 5;
        public const int gmoHdomused = 6;
        public const int gmoHmarginals = 9;
        public const int gmoHetalg = 10;
        public const int gmoTmipnod = 11;
        public const int gmoTninf = 12;
        public const int gmoTnopt = 13;
        public const int gmoTmipbest = 15;
        public const int gmoTsinf = 20;
        public const int gmoTrobj = 22;

        public const int gmonumheader = 10;  // gmoHTcard
        public const int gmonumtail = 12;

        public const int gmoProc_none = 0;  // gmoProcType
        public const int gmoProc_lp = 1;
        public const int gmoProc_mip = 2;
        public const int gmoProc_rmip = 3;
        public const int gmoProc_nlp = 4;
        public const int gmoProc_mcp = 5;
        public const int gmoProc_mpec = 6;
        public const int gmoProc_rmpec = 7;
        public const int gmoProc_cns = 8;
        public const int gmoProc_dnlp = 9;
        public const int gmoProc_rminlp = 10;
        public const int gmoProc_minlp = 11;
        public const int gmoProc_qcp = 12;
        public const int gmoProc_miqcp = 13;
        public const int gmoProc_rmiqcp = 14;
        public const int gmoProc_emp = 15;
        public const int gmoProc_nrofmodeltypes = 16;

        public const int gmoMinAgent = 0;  // gmoEMPAgentType
        public const int gmoMaxAgent = 1;
        public const int gmoVIAgent = 2;

        public const int gmoEVALERRORMETHOD_KEEPGOING = 0;  // gmoEvalErrorMethodNum
        public const int gmoEVALERRORMETHOD_FASTSTOP = 1;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoInitData_t(IntPtr pgmo, int rows, int cols, int codelen);
        private static gmoInitData_t dll_gmoInitData;
        private static int d_gmoInitData(IntPtr pgmo, int rows, int cols, int codelen)
        { gmoErrorHandling("gmoInitData could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoAddRow_t(IntPtr pgmo, int etyp, int ematch, double eslack, double escale, double erhs, double emarg, int ebas, int enz, int[] colidx, double[] jacval, int[] nlflag);
        private static gmoAddRow_t dll_gmoAddRow;
        private static int d_gmoAddRow(IntPtr pgmo, int etyp, int ematch, double eslack, double escale, double erhs, double emarg, int ebas, int enz, int[] colidx, double[] jacval, int[] nlflag)
        { gmoErrorHandling("gmoAddRow could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoAddCol_t(IntPtr pgmo, int vtyp, double vlo, double vl, double vup, double vmarg, int vbas, int vsos, double vprior, double vscale, int vnz, int[] rowidx, double[] jacval, int[] nlflag);
        private static gmoAddCol_t dll_gmoAddCol;
        private static int d_gmoAddCol(IntPtr pgmo, int vtyp, double vlo, double vl, double vup, double vmarg, int vbas, int vsos, double vprior, double vscale, int vnz, int[] rowidx, double[] jacval, int[] nlflag)
        { gmoErrorHandling("gmoAddCol could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoCompleteData_t(IntPtr pgmo, StringBuilder msg);
        private static gmoCompleteData_t dll_gmoCompleteData;
        private static int d_gmoCompleteData(IntPtr pgmo, StringBuilder msg)
        { gmoErrorHandling("gmoCompleteData could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoLoadDataLegacy_t(IntPtr pgmo, StringBuilder msg);
        private static gmoLoadDataLegacy_t dll_gmoLoadDataLegacy;
        private static int d_gmoLoadDataLegacy(IntPtr pgmo, StringBuilder msg)
        { gmoErrorHandling("gmoLoadDataLegacy could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoRegisterEnvironment_t(IntPtr pgmo, IntPtr gevptr, StringBuilder msg);
        private static gmoRegisterEnvironment_t dll_gmoRegisterEnvironment;
        private static int d_gmoRegisterEnvironment(IntPtr pgmo, IntPtr gevptr, StringBuilder msg)
        { gmoErrorHandling("gmoRegisterEnvironment could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr gmoEnvironment_t(IntPtr pgmo);
        private static gmoEnvironment_t dll_gmoEnvironment;
        private static IntPtr d_gmoEnvironment(IntPtr pgmo)
        { gmoErrorHandling("gmoEnvironment could not be loaded"); return IntPtr.Zero; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr gmoViewStore_t(IntPtr pgmo);
        private static gmoViewStore_t dll_gmoViewStore;
        private static IntPtr d_gmoViewStore(IntPtr pgmo)
        { gmoErrorHandling("gmoViewStore could not be loaded"); return IntPtr.Zero; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoViewRestore_t(IntPtr pgmo, ref IntPtr viewptr);
        private static gmoViewRestore_t dll_gmoViewRestore;
        private static void d_gmoViewRestore(IntPtr pgmo, ref IntPtr viewptr)
        { gmoErrorHandling("gmoViewRestore could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoViewDump_t(IntPtr pgmo);
        private static gmoViewDump_t dll_gmoViewDump;
        private static void d_gmoViewDump(IntPtr pgmo)
        { gmoErrorHandling("gmoViewDump could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetiSolver_t(IntPtr pgmo, int mi);
        private static gmoGetiSolver_t dll_gmoGetiSolver;
        private static int d_gmoGetiSolver(IntPtr pgmo, int mi)
        { gmoErrorHandling("gmoGetiSolver could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetjSolver_t(IntPtr pgmo, int mj);
        private static gmoGetjSolver_t dll_gmoGetjSolver;
        private static int d_gmoGetjSolver(IntPtr pgmo, int mj)
        { gmoErrorHandling("gmoGetjSolver could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetiSolverQuiet_t(IntPtr pgmo, int mi);
        private static gmoGetiSolverQuiet_t dll_gmoGetiSolverQuiet;
        private static int d_gmoGetiSolverQuiet(IntPtr pgmo, int mi)
        { gmoErrorHandling("gmoGetiSolverQuiet could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetjSolverQuiet_t(IntPtr pgmo, int mj);
        private static gmoGetjSolverQuiet_t dll_gmoGetjSolverQuiet;
        private static int d_gmoGetjSolverQuiet(IntPtr pgmo, int mj)
        { gmoErrorHandling("gmoGetjSolverQuiet could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetiModel_t(IntPtr pgmo, int si);
        private static gmoGetiModel_t dll_gmoGetiModel;
        private static int d_gmoGetiModel(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetiModel could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetjModel_t(IntPtr pgmo, int sj);
        private static gmoGetjModel_t dll_gmoGetjModel;
        private static int d_gmoGetjModel(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetjModel could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetEquPermutation_t(IntPtr pgmo, int[] permut);
        private static gmoSetEquPermutation_t dll_gmoSetEquPermutation;
        private static int d_gmoSetEquPermutation(IntPtr pgmo, int[] permut)
        { gmoErrorHandling("gmoSetEquPermutation could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetRvEquPermutation_t(IntPtr pgmo, int[] rvpermut, int len);
        private static gmoSetRvEquPermutation_t dll_gmoSetRvEquPermutation;
        private static int d_gmoSetRvEquPermutation(IntPtr pgmo, int[] rvpermut, int len)
        { gmoErrorHandling("gmoSetRvEquPermutation could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetVarPermutation_t(IntPtr pgmo, int[] permut);
        private static gmoSetVarPermutation_t dll_gmoSetVarPermutation;
        private static int d_gmoSetVarPermutation(IntPtr pgmo, int[] permut)
        { gmoErrorHandling("gmoSetVarPermutation could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetRvVarPermutation_t(IntPtr pgmo, int[] rvpermut, int len);
        private static gmoSetRvVarPermutation_t dll_gmoSetRvVarPermutation;
        private static int d_gmoSetRvVarPermutation(IntPtr pgmo, int[] rvpermut, int len)
        { gmoErrorHandling("gmoSetRvVarPermutation could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetNRowPerm_t(IntPtr pgmo);
        private static gmoSetNRowPerm_t dll_gmoSetNRowPerm;
        private static int d_gmoSetNRowPerm(IntPtr pgmo)
        { gmoErrorHandling("gmoSetNRowPerm could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarTypeCnt_t(IntPtr pgmo, int vtyp);
        private static gmoGetVarTypeCnt_t dll_gmoGetVarTypeCnt;
        private static int d_gmoGetVarTypeCnt(IntPtr pgmo, int vtyp)
        { gmoErrorHandling("gmoGetVarTypeCnt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquTypeCnt_t(IntPtr pgmo, int etyp);
        private static gmoGetEquTypeCnt_t dll_gmoGetEquTypeCnt;
        private static int d_gmoGetEquTypeCnt(IntPtr pgmo, int etyp)
        { gmoErrorHandling("gmoGetEquTypeCnt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetObjStat_t(IntPtr pgmo, ref int nz, ref int qnz, ref int nlnz);
        private static gmoGetObjStat_t dll_gmoGetObjStat;
        private static int d_gmoGetObjStat(IntPtr pgmo, ref int nz, ref int qnz, ref int nlnz)
        { gmoErrorHandling("gmoGetObjStat could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetRowStat_t(IntPtr pgmo, int si, ref int nz, ref int qnz, ref int nlnz);
        private static gmoGetRowStat_t dll_gmoGetRowStat;
        private static int d_gmoGetRowStat(IntPtr pgmo, int si, ref int nz, ref int qnz, ref int nlnz)
        { gmoErrorHandling("gmoGetRowStat could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetRowStatEx_t(IntPtr pgmo, int si, ref int nz, ref int lnz, ref int qnz, ref int nlnz);
        private static gmoGetRowStatEx_t dll_gmoGetRowStatEx;
        private static int d_gmoGetRowStatEx(IntPtr pgmo, int si, ref int nz, ref int lnz, ref int qnz, ref int nlnz)
        { gmoErrorHandling("gmoGetRowStatEx could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetColStat_t(IntPtr pgmo, int sj, ref int nz, ref int qnz, ref int nlnz, ref int objnz);
        private static gmoGetColStat_t dll_gmoGetColStat;
        private static int d_gmoGetColStat(IntPtr pgmo, int sj, ref int nz, ref int qnz, ref int nlnz, ref int objnz)
        { gmoErrorHandling("gmoGetColStat could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetRowQNZOne_t(IntPtr pgmo, int si);
        private static gmoGetRowQNZOne_t dll_gmoGetRowQNZOne;
        private static int d_gmoGetRowQNZOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetRowQNZOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Int64 gmoGetRowQNZOne64_t(IntPtr pgmo, int si);
        private static gmoGetRowQNZOne64_t dll_gmoGetRowQNZOne64;
        private static Int64 d_gmoGetRowQNZOne64(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetRowQNZOne64 could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetRowQDiagNZOne_t(IntPtr pgmo, int si);
        private static gmoGetRowQDiagNZOne_t dll_gmoGetRowQDiagNZOne;
        private static int d_gmoGetRowQDiagNZOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetRowQDiagNZOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetRowCVecNZOne_t(IntPtr pgmo, int si);
        private static gmoGetRowCVecNZOne_t dll_gmoGetRowCVecNZOne;
        private static int d_gmoGetRowCVecNZOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetRowCVecNZOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetSosCounts_t(IntPtr pgmo, ref int numsos1, ref int numsos2, ref int nzsos);
        private static gmoGetSosCounts_t dll_gmoGetSosCounts;
        private static void d_gmoGetSosCounts(IntPtr pgmo, ref int numsos1, ref int numsos2, ref int nzsos)
        { gmoErrorHandling("gmoGetSosCounts could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetXLibCounts_t(IntPtr pgmo, ref int rows, ref int cols, ref int nz, int[] orgcolind);
        private static gmoGetXLibCounts_t dll_gmoGetXLibCounts;
        private static void d_gmoGetXLibCounts(IntPtr pgmo, ref int rows, ref int cols, ref int nz, int[] orgcolind)
        { gmoErrorHandling("gmoGetXLibCounts could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetActiveModelType_t(IntPtr pgmo, int[] checkv, ref int actModelType);
        private static gmoGetActiveModelType_t dll_gmoGetActiveModelType;
        private static int d_gmoGetActiveModelType(IntPtr pgmo, int[] checkv, ref int actModelType)
        { gmoErrorHandling("gmoGetActiveModelType could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetMatrixRow_t(IntPtr pgmo, int[] rowstart, int[] colidx, double[] jacval, int[] nlflag);
        private static gmoGetMatrixRow_t dll_gmoGetMatrixRow;
        private static int d_gmoGetMatrixRow(IntPtr pgmo, int[] rowstart, int[] colidx, double[] jacval, int[] nlflag)
        { gmoErrorHandling("gmoGetMatrixRow could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetMatrixCol_t(IntPtr pgmo, int[] colstart, int[] rowidx, double[] jacval, int[] nlflag);
        private static gmoGetMatrixCol_t dll_gmoGetMatrixCol;
        private static int d_gmoGetMatrixCol(IntPtr pgmo, int[] colstart, int[] rowidx, double[] jacval, int[] nlflag)
        { gmoErrorHandling("gmoGetMatrixCol could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetMatrixCplex_t(IntPtr pgmo, int[] colstart, int[] collength, int[] rowidx, double[] jacval);
        private static gmoGetMatrixCplex_t dll_gmoGetMatrixCplex;
        private static int d_gmoGetMatrixCplex(IntPtr pgmo, int[] colstart, int[] collength, int[] rowidx, double[] jacval)
        { gmoErrorHandling("gmoGetMatrixCplex could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetObjName_t(IntPtr pgmo, StringBuilder sst_result);
        private static gmoGetObjName_t dll_gmoGetObjName;
        private static void d_gmoGetObjName(IntPtr pgmo, StringBuilder sst_result)
        { gmoErrorHandling("gmoGetObjName could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetObjNameCustom_t(IntPtr pgmo, string suffix, StringBuilder sst_result);
        private static gmoGetObjNameCustom_t dll_gmoGetObjNameCustom;
        private static void d_gmoGetObjNameCustom(IntPtr pgmo, string suffix, StringBuilder sst_result)
        { gmoErrorHandling("gmoGetObjNameCustom could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetObjVector_t(IntPtr pgmo, double[] jacval, int[] nlflag);
        private static gmoGetObjVector_t dll_gmoGetObjVector;
        private static int d_gmoGetObjVector(IntPtr pgmo, double[] jacval, int[] nlflag)
        { gmoErrorHandling("gmoGetObjVector could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetObjSparse_t(IntPtr pgmo, int[] colidx, double[] jacval, int[] nlflag, ref int nz, ref int nlnz);
        private static gmoGetObjSparse_t dll_gmoGetObjSparse;
        private static int d_gmoGetObjSparse(IntPtr pgmo, int[] colidx, double[] jacval, int[] nlflag, ref int nz, ref int nlnz)
        { gmoErrorHandling("gmoGetObjSparse could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetObjSparseEx_t(IntPtr pgmo, int[] colidx, double[] gradval, int[] nlflag, ref int nz, ref int qnz, ref int nlnz);
        private static gmoGetObjSparseEx_t dll_gmoGetObjSparseEx;
        private static int d_gmoGetObjSparseEx(IntPtr pgmo, int[] colidx, double[] gradval, int[] nlflag, ref int nz, ref int qnz, ref int nlnz)
        { gmoErrorHandling("gmoGetObjSparseEx could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetObjQMat_t(IntPtr pgmo, int[] varidx1, int[] varidx2, double[] coefs);
        private static gmoGetObjQMat_t dll_gmoGetObjQMat;
        private static int d_gmoGetObjQMat(IntPtr pgmo, int[] varidx1, int[] varidx2, double[] coefs)
        { gmoErrorHandling("gmoGetObjQMat could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetObjQ_t(IntPtr pgmo, int[] varidx1, int[] varidx2, double[] coefs);
        private static gmoGetObjQ_t dll_gmoGetObjQ;
        private static int d_gmoGetObjQ(IntPtr pgmo, int[] varidx1, int[] varidx2, double[] coefs)
        { gmoErrorHandling("gmoGetObjQ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetObjCVec_t(IntPtr pgmo, int[] varidx, double[] coefs);
        private static gmoGetObjCVec_t dll_gmoGetObjCVec;
        private static int d_gmoGetObjCVec(IntPtr pgmo, int[] varidx, double[] coefs)
        { gmoErrorHandling("gmoGetObjCVec could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquL_t(IntPtr pgmo, double[] e);
        private static gmoGetEquL_t dll_gmoGetEquL;
        private static int d_gmoGetEquL(IntPtr pgmo, double[] e)
        { gmoErrorHandling("gmoGetEquL could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetEquLOne_t(IntPtr pgmo, int si);
        private static gmoGetEquLOne_t dll_gmoGetEquLOne;
        private static double d_gmoGetEquLOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetEquLOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetEquL_t(IntPtr pgmo, double[] el);
        private static gmoSetEquL_t dll_gmoSetEquL;
        private static int d_gmoSetEquL(IntPtr pgmo, double[] el)
        { gmoErrorHandling("gmoSetEquL could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetEquLOne_t(IntPtr pgmo, int si, double el);
        private static gmoSetEquLOne_t dll_gmoSetEquLOne;
        private static void d_gmoSetEquLOne(IntPtr pgmo, int si, double el)
        { gmoErrorHandling("gmoSetEquLOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquM_t(IntPtr pgmo, double[] pi);
        private static gmoGetEquM_t dll_gmoGetEquM;
        private static int d_gmoGetEquM(IntPtr pgmo, double[] pi)
        { gmoErrorHandling("gmoGetEquM could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetEquMOne_t(IntPtr pgmo, int si);
        private static gmoGetEquMOne_t dll_gmoGetEquMOne;
        private static double d_gmoGetEquMOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetEquMOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetEquM_t(IntPtr pgmo, double[] emarg);
        private static gmoSetEquM_t dll_gmoSetEquM;
        private static int d_gmoSetEquM(IntPtr pgmo, double[] emarg)
        { gmoErrorHandling("gmoSetEquM could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetEquNameOne_t(IntPtr pgmo, int si, StringBuilder sst_result);
        private static gmoGetEquNameOne_t dll_gmoGetEquNameOne;
        private static void d_gmoGetEquNameOne(IntPtr pgmo, int si, StringBuilder sst_result)
        { gmoErrorHandling("gmoGetEquNameOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetEquNameCustomOne_t(IntPtr pgmo, int si, string suffix, StringBuilder sst_result);
        private static gmoGetEquNameCustomOne_t dll_gmoGetEquNameCustomOne;
        private static void d_gmoGetEquNameCustomOne(IntPtr pgmo, int si, string suffix, StringBuilder sst_result)
        { gmoErrorHandling("gmoGetEquNameCustomOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetRhs_t(IntPtr pgmo, double[] mdblvec);
        private static gmoGetRhs_t dll_gmoGetRhs;
        private static int d_gmoGetRhs(IntPtr pgmo, double[] mdblvec)
        { gmoErrorHandling("gmoGetRhs could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetRhsOne_t(IntPtr pgmo, int si);
        private static gmoGetRhsOne_t dll_gmoGetRhsOne;
        private static double d_gmoGetRhsOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetRhsOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetRhsOneEx_t(IntPtr pgmo, int si);
        private static gmoGetRhsOneEx_t dll_gmoGetRhsOneEx;
        private static double d_gmoGetRhsOneEx(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetRhsOneEx could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetAltRHS_t(IntPtr pgmo, double[] mdblvec);
        private static gmoSetAltRHS_t dll_gmoSetAltRHS;
        private static int d_gmoSetAltRHS(IntPtr pgmo, double[] mdblvec)
        { gmoErrorHandling("gmoSetAltRHS could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetAltRHSOne_t(IntPtr pgmo, int si, double erhs);
        private static gmoSetAltRHSOne_t dll_gmoSetAltRHSOne;
        private static void d_gmoSetAltRHSOne(IntPtr pgmo, int si, double erhs)
        { gmoErrorHandling("gmoSetAltRHSOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquSlack_t(IntPtr pgmo, double[] mdblvec);
        private static gmoGetEquSlack_t dll_gmoGetEquSlack;
        private static int d_gmoGetEquSlack(IntPtr pgmo, double[] mdblvec)
        { gmoErrorHandling("gmoGetEquSlack could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetEquSlackOne_t(IntPtr pgmo, int si);
        private static gmoGetEquSlackOne_t dll_gmoGetEquSlackOne;
        private static double d_gmoGetEquSlackOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetEquSlackOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetEquSlack_t(IntPtr pgmo, double[] mdblvec);
        private static gmoSetEquSlack_t dll_gmoSetEquSlack;
        private static int d_gmoSetEquSlack(IntPtr pgmo, double[] mdblvec)
        { gmoErrorHandling("gmoSetEquSlack could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquType_t(IntPtr pgmo, int[] mintvec);
        private static gmoGetEquType_t dll_gmoGetEquType;
        private static int d_gmoGetEquType(IntPtr pgmo, int[] mintvec)
        { gmoErrorHandling("gmoGetEquType could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquTypeOne_t(IntPtr pgmo, int si);
        private static gmoGetEquTypeOne_t dll_gmoGetEquTypeOne;
        private static int d_gmoGetEquTypeOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetEquTypeOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetEquStat_t(IntPtr pgmo, int[] mintvec);
        private static gmoGetEquStat_t dll_gmoGetEquStat;
        private static void d_gmoGetEquStat(IntPtr pgmo, int[] mintvec)
        { gmoErrorHandling("gmoGetEquStat could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquStatOne_t(IntPtr pgmo, int si);
        private static gmoGetEquStatOne_t dll_gmoGetEquStatOne;
        private static int d_gmoGetEquStatOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetEquStatOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetEquStat_t(IntPtr pgmo, int[] mintvec);
        private static gmoSetEquStat_t dll_gmoSetEquStat;
        private static void d_gmoSetEquStat(IntPtr pgmo, int[] mintvec)
        { gmoErrorHandling("gmoSetEquStat could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetEquCStat_t(IntPtr pgmo, int[] mintvec);
        private static gmoGetEquCStat_t dll_gmoGetEquCStat;
        private static void d_gmoGetEquCStat(IntPtr pgmo, int[] mintvec)
        { gmoErrorHandling("gmoGetEquCStat could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquCStatOne_t(IntPtr pgmo, int si);
        private static gmoGetEquCStatOne_t dll_gmoGetEquCStatOne;
        private static int d_gmoGetEquCStatOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetEquCStatOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetEquCStat_t(IntPtr pgmo, int[] mintvec);
        private static gmoSetEquCStat_t dll_gmoSetEquCStat;
        private static void d_gmoSetEquCStat(IntPtr pgmo, int[] mintvec)
        { gmoErrorHandling("gmoSetEquCStat could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquMatch_t(IntPtr pgmo, int[] mintvec);
        private static gmoGetEquMatch_t dll_gmoGetEquMatch;
        private static int d_gmoGetEquMatch(IntPtr pgmo, int[] mintvec)
        { gmoErrorHandling("gmoGetEquMatch could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquMatchOne_t(IntPtr pgmo, int si);
        private static gmoGetEquMatchOne_t dll_gmoGetEquMatchOne;
        private static int d_gmoGetEquMatchOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetEquMatchOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquScale_t(IntPtr pgmo, double[] mdblvec);
        private static gmoGetEquScale_t dll_gmoGetEquScale;
        private static int d_gmoGetEquScale(IntPtr pgmo, double[] mdblvec)
        { gmoErrorHandling("gmoGetEquScale could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetEquScaleOne_t(IntPtr pgmo, int si);
        private static gmoGetEquScaleOne_t dll_gmoGetEquScaleOne;
        private static double d_gmoGetEquScaleOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetEquScaleOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquStage_t(IntPtr pgmo, double[] mdblvec);
        private static gmoGetEquStage_t dll_gmoGetEquStage;
        private static int d_gmoGetEquStage(IntPtr pgmo, double[] mdblvec)
        { gmoErrorHandling("gmoGetEquStage could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetEquStageOne_t(IntPtr pgmo, int si);
        private static gmoGetEquStageOne_t dll_gmoGetEquStageOne;
        private static double d_gmoGetEquStageOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetEquStageOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquOrderOne_t(IntPtr pgmo, int si);
        private static gmoGetEquOrderOne_t dll_gmoGetEquOrderOne;
        private static int d_gmoGetEquOrderOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetEquOrderOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetRowSparse_t(IntPtr pgmo, int si, int[] colidx, double[] jacval, int[] nlflag, ref int nz, ref int nlnz);
        private static gmoGetRowSparse_t dll_gmoGetRowSparse;
        private static int d_gmoGetRowSparse(IntPtr pgmo, int si, int[] colidx, double[] jacval, int[] nlflag, ref int nz, ref int nlnz)
        { gmoErrorHandling("gmoGetRowSparse could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetRowSparseEx_t(IntPtr pgmo, int si, int[] colidx, double[] jacval, int[] nlflag, ref int nz, ref int qnz, ref int nlnz);
        private static gmoGetRowSparseEx_t dll_gmoGetRowSparseEx;
        private static int d_gmoGetRowSparseEx(IntPtr pgmo, int si, int[] colidx, double[] jacval, int[] nlflag, ref int nz, ref int qnz, ref int nlnz)
        { gmoErrorHandling("gmoGetRowSparseEx could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetRowJacInfoOne_t(IntPtr pgmo, int si, ref IntPtr jacptr, ref double jacval, ref int colidx, ref int nlflag);
        private static gmoGetRowJacInfoOne_t dll_gmoGetRowJacInfoOne;
        private static void d_gmoGetRowJacInfoOne(IntPtr pgmo, int si, ref IntPtr jacptr, ref double jacval, ref int colidx, ref int nlflag)
        { gmoErrorHandling("gmoGetRowJacInfoOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetRowQMat_t(IntPtr pgmo, int si, int[] varidx1, int[] varidx2, double[] coefs);
        private static gmoGetRowQMat_t dll_gmoGetRowQMat;
        private static int d_gmoGetRowQMat(IntPtr pgmo, int si, int[] varidx1, int[] varidx2, double[] coefs)
        { gmoErrorHandling("gmoGetRowQMat could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetRowQ_t(IntPtr pgmo, int si, int[] varidx1, int[] varidx2, double[] coefs);
        private static gmoGetRowQ_t dll_gmoGetRowQ;
        private static int d_gmoGetRowQ(IntPtr pgmo, int si, int[] varidx1, int[] varidx2, double[] coefs)
        { gmoErrorHandling("gmoGetRowQ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetRowCVec_t(IntPtr pgmo, int si, int[] varidx, double[] coefs);
        private static gmoGetRowCVec_t dll_gmoGetRowCVec;
        private static int d_gmoGetRowCVec(IntPtr pgmo, int si, int[] varidx, double[] coefs)
        { gmoErrorHandling("gmoGetRowCVec could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetRowQConst_t(IntPtr pgmo, int si);
        private static gmoGetRowQConst_t dll_gmoGetRowQConst;
        private static double d_gmoGetRowQConst(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetRowQConst could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquIntDotOpt_t(IntPtr pgmo, IntPtr optptr, string dotopt, int[] optvals);
        private static gmoGetEquIntDotOpt_t dll_gmoGetEquIntDotOpt;
        private static int d_gmoGetEquIntDotOpt(IntPtr pgmo, IntPtr optptr, string dotopt, int[] optvals)
        { gmoErrorHandling("gmoGetEquIntDotOpt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquDblDotOpt_t(IntPtr pgmo, IntPtr optptr, string dotopt, double[] optvals);
        private static gmoGetEquDblDotOpt_t dll_gmoGetEquDblDotOpt;
        private static int d_gmoGetEquDblDotOpt(IntPtr pgmo, IntPtr optptr, string dotopt, double[] optvals)
        { gmoErrorHandling("gmoGetEquDblDotOpt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarL_t(IntPtr pgmo, double[] x);
        private static gmoGetVarL_t dll_gmoGetVarL;
        private static int d_gmoGetVarL(IntPtr pgmo, double[] x)
        { gmoErrorHandling("gmoGetVarL could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetVarLOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarLOne_t dll_gmoGetVarLOne;
        private static double d_gmoGetVarLOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarLOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetVarL_t(IntPtr pgmo, double[] x);
        private static gmoSetVarL_t dll_gmoSetVarL;
        private static int d_gmoSetVarL(IntPtr pgmo, double[] x)
        { gmoErrorHandling("gmoSetVarL could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetVarLOne_t(IntPtr pgmo, int sj, double vl);
        private static gmoSetVarLOne_t dll_gmoSetVarLOne;
        private static void d_gmoSetVarLOne(IntPtr pgmo, int sj, double vl)
        { gmoErrorHandling("gmoSetVarLOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarM_t(IntPtr pgmo, double[] dj);
        private static gmoGetVarM_t dll_gmoGetVarM;
        private static int d_gmoGetVarM(IntPtr pgmo, double[] dj)
        { gmoErrorHandling("gmoGetVarM could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetVarMOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarMOne_t dll_gmoGetVarMOne;
        private static double d_gmoGetVarMOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarMOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetVarM_t(IntPtr pgmo, double[] dj);
        private static gmoSetVarM_t dll_gmoSetVarM;
        private static int d_gmoSetVarM(IntPtr pgmo, double[] dj)
        { gmoErrorHandling("gmoSetVarM could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetVarMOne_t(IntPtr pgmo, int sj, double vmarg);
        private static gmoSetVarMOne_t dll_gmoSetVarMOne;
        private static void d_gmoSetVarMOne(IntPtr pgmo, int sj, double vmarg)
        { gmoErrorHandling("gmoSetVarMOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetVarNameOne_t(IntPtr pgmo, int sj, StringBuilder sst_result);
        private static gmoGetVarNameOne_t dll_gmoGetVarNameOne;
        private static void d_gmoGetVarNameOne(IntPtr pgmo, int sj, StringBuilder sst_result)
        { gmoErrorHandling("gmoGetVarNameOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetVarNameCustomOne_t(IntPtr pgmo, int sj, string suffix, StringBuilder sst_result);
        private static gmoGetVarNameCustomOne_t dll_gmoGetVarNameCustomOne;
        private static void d_gmoGetVarNameCustomOne(IntPtr pgmo, int sj, string suffix, StringBuilder sst_result)
        { gmoErrorHandling("gmoGetVarNameCustomOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarLower_t(IntPtr pgmo, double[] lovec);
        private static gmoGetVarLower_t dll_gmoGetVarLower;
        private static int d_gmoGetVarLower(IntPtr pgmo, double[] lovec)
        { gmoErrorHandling("gmoGetVarLower could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetVarLowerOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarLowerOne_t dll_gmoGetVarLowerOne;
        private static double d_gmoGetVarLowerOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarLowerOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarUpper_t(IntPtr pgmo, double[] upvec);
        private static gmoGetVarUpper_t dll_gmoGetVarUpper;
        private static int d_gmoGetVarUpper(IntPtr pgmo, double[] upvec)
        { gmoErrorHandling("gmoGetVarUpper could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetVarUpperOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarUpperOne_t dll_gmoGetVarUpperOne;
        private static double d_gmoGetVarUpperOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarUpperOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetAltVarBounds_t(IntPtr pgmo, double[] lovec, double[] upvec);
        private static gmoSetAltVarBounds_t dll_gmoSetAltVarBounds;
        private static int d_gmoSetAltVarBounds(IntPtr pgmo, double[] lovec, double[] upvec)
        { gmoErrorHandling("gmoSetAltVarBounds could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetAltVarLowerOne_t(IntPtr pgmo, int sj, double vlo);
        private static gmoSetAltVarLowerOne_t dll_gmoSetAltVarLowerOne;
        private static void d_gmoSetAltVarLowerOne(IntPtr pgmo, int sj, double vlo)
        { gmoErrorHandling("gmoSetAltVarLowerOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetAltVarUpperOne_t(IntPtr pgmo, int sj, double vup);
        private static gmoSetAltVarUpperOne_t dll_gmoSetAltVarUpperOne;
        private static void d_gmoSetAltVarUpperOne(IntPtr pgmo, int sj, double vup)
        { gmoErrorHandling("gmoSetAltVarUpperOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarType_t(IntPtr pgmo, int[] nintvec);
        private static gmoGetVarType_t dll_gmoGetVarType;
        private static int d_gmoGetVarType(IntPtr pgmo, int[] nintvec)
        { gmoErrorHandling("gmoGetVarType could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarTypeOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarTypeOne_t dll_gmoGetVarTypeOne;
        private static int d_gmoGetVarTypeOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarTypeOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetAltVarType_t(IntPtr pgmo, int[] nintvec);
        private static gmoSetAltVarType_t dll_gmoSetAltVarType;
        private static int d_gmoSetAltVarType(IntPtr pgmo, int[] nintvec)
        { gmoErrorHandling("gmoSetAltVarType could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetAltVarTypeOne_t(IntPtr pgmo, int sj, int vtyp);
        private static gmoSetAltVarTypeOne_t dll_gmoSetAltVarTypeOne;
        private static void d_gmoSetAltVarTypeOne(IntPtr pgmo, int sj, int vtyp)
        { gmoErrorHandling("gmoSetAltVarTypeOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetVarStat_t(IntPtr pgmo, int[] nintvec);
        private static gmoGetVarStat_t dll_gmoGetVarStat;
        private static void d_gmoGetVarStat(IntPtr pgmo, int[] nintvec)
        { gmoErrorHandling("gmoGetVarStat could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarStatOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarStatOne_t dll_gmoGetVarStatOne;
        private static int d_gmoGetVarStatOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarStatOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetVarStat_t(IntPtr pgmo, int[] nintvec);
        private static gmoSetVarStat_t dll_gmoSetVarStat;
        private static void d_gmoSetVarStat(IntPtr pgmo, int[] nintvec)
        { gmoErrorHandling("gmoSetVarStat could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetVarStatOne_t(IntPtr pgmo, int sj, int vstat);
        private static gmoSetVarStatOne_t dll_gmoSetVarStatOne;
        private static void d_gmoSetVarStatOne(IntPtr pgmo, int sj, int vstat)
        { gmoErrorHandling("gmoSetVarStatOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetVarCStat_t(IntPtr pgmo, int[] nintvec);
        private static gmoGetVarCStat_t dll_gmoGetVarCStat;
        private static void d_gmoGetVarCStat(IntPtr pgmo, int[] nintvec)
        { gmoErrorHandling("gmoGetVarCStat could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarCStatOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarCStatOne_t dll_gmoGetVarCStatOne;
        private static int d_gmoGetVarCStatOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarCStatOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetVarCStat_t(IntPtr pgmo, int[] nintvec);
        private static gmoSetVarCStat_t dll_gmoSetVarCStat;
        private static void d_gmoSetVarCStat(IntPtr pgmo, int[] nintvec)
        { gmoErrorHandling("gmoSetVarCStat could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarMatch_t(IntPtr pgmo, int[] nintvec);
        private static gmoGetVarMatch_t dll_gmoGetVarMatch;
        private static int d_gmoGetVarMatch(IntPtr pgmo, int[] nintvec)
        { gmoErrorHandling("gmoGetVarMatch could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarMatchOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarMatchOne_t dll_gmoGetVarMatchOne;
        private static int d_gmoGetVarMatchOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarMatchOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarPrior_t(IntPtr pgmo, double[] ndblvec);
        private static gmoGetVarPrior_t dll_gmoGetVarPrior;
        private static int d_gmoGetVarPrior(IntPtr pgmo, double[] ndblvec)
        { gmoErrorHandling("gmoGetVarPrior could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetVarPriorOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarPriorOne_t dll_gmoGetVarPriorOne;
        private static double d_gmoGetVarPriorOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarPriorOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarScale_t(IntPtr pgmo, double[] ndblvec);
        private static gmoGetVarScale_t dll_gmoGetVarScale;
        private static int d_gmoGetVarScale(IntPtr pgmo, double[] ndblvec)
        { gmoErrorHandling("gmoGetVarScale could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetVarScaleOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarScaleOne_t dll_gmoGetVarScaleOne;
        private static double d_gmoGetVarScaleOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarScaleOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarStage_t(IntPtr pgmo, double[] ndblvec);
        private static gmoGetVarStage_t dll_gmoGetVarStage;
        private static int d_gmoGetVarStage(IntPtr pgmo, double[] ndblvec)
        { gmoErrorHandling("gmoGetVarStage could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetVarStageOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarStageOne_t dll_gmoGetVarStageOne;
        private static double d_gmoGetVarStageOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarStageOne could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetSosConstraints_t(IntPtr pgmo, int[] sostype, int[] sosbeg, int[] sosind, double[] soswt);
        private static gmoGetSosConstraints_t dll_gmoGetSosConstraints;
        private static int d_gmoGetSosConstraints(IntPtr pgmo, int[] sostype, int[] sosbeg, int[] sosind, double[] soswt)
        { gmoErrorHandling("gmoGetSosConstraints could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarSosSetOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarSosSetOne_t dll_gmoGetVarSosSetOne;
        private static int d_gmoGetVarSosSetOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarSosSetOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetColSparse_t(IntPtr pgmo, int sj, int[] rowidx, double[] jacval, int[] nlflag, ref int nz, ref int nlnz);
        private static gmoGetColSparse_t dll_gmoGetColSparse;
        private static int d_gmoGetColSparse(IntPtr pgmo, int sj, int[] rowidx, double[] jacval, int[] nlflag, ref int nz, ref int nlnz)
        { gmoErrorHandling("gmoGetColSparse could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetColJacInfoOne_t(IntPtr pgmo, int sj, ref IntPtr jacptr, ref double jacval, ref int rowidx, ref int nlflag);
        private static gmoGetColJacInfoOne_t dll_gmoGetColJacInfoOne;
        private static void d_gmoGetColJacInfoOne(IntPtr pgmo, int sj, ref IntPtr jacptr, ref double jacval, ref int rowidx, ref int nlflag)
        { gmoErrorHandling("gmoGetColJacInfoOne could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarIntDotOpt_t(IntPtr pgmo, IntPtr optptr, string dotopt, int[] optvals);
        private static gmoGetVarIntDotOpt_t dll_gmoGetVarIntDotOpt;
        private static int d_gmoGetVarIntDotOpt(IntPtr pgmo, IntPtr optptr, string dotopt, int[] optvals)
        { gmoErrorHandling("gmoGetVarIntDotOpt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarDblDotOpt_t(IntPtr pgmo, IntPtr optptr, string dotopt, double[] optvals);
        private static gmoGetVarDblDotOpt_t dll_gmoGetVarDblDotOpt;
        private static int d_gmoGetVarDblDotOpt(IntPtr pgmo, IntPtr optptr, string dotopt, double[] optvals)
        { gmoErrorHandling("gmoGetVarDblDotOpt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoEvalErrorMsg_t(IntPtr pgmo, int domsg);
        private static gmoEvalErrorMsg_t dll_gmoEvalErrorMsg;
        private static void d_gmoEvalErrorMsg(IntPtr pgmo, int domsg)
        { gmoErrorHandling("gmoEvalErrorMsg could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoEvalErrorMsg_MT_t(IntPtr pgmo, int domsg, int tidx);
        private static gmoEvalErrorMsg_MT_t dll_gmoEvalErrorMsg_MT;
        private static void d_gmoEvalErrorMsg_MT(IntPtr pgmo, int domsg, int tidx)
        { gmoErrorHandling("gmoEvalErrorMsg_MT could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoEvalErrorMaskLevel_t(IntPtr pgmo, int MaskLevel);
        private static gmoEvalErrorMaskLevel_t dll_gmoEvalErrorMaskLevel;
        private static void d_gmoEvalErrorMaskLevel(IntPtr pgmo, int MaskLevel)
        { gmoErrorHandling("gmoEvalErrorMaskLevel could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoEvalErrorMaskLevel_MT_t(IntPtr pgmo, int MaskLevel, int tidx);
        private static gmoEvalErrorMaskLevel_MT_t dll_gmoEvalErrorMaskLevel_MT;
        private static void d_gmoEvalErrorMaskLevel_MT(IntPtr pgmo, int MaskLevel, int tidx)
        { gmoErrorHandling("gmoEvalErrorMaskLevel_MT could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalNewPoint_t(IntPtr pgmo, double[] x);
        private static gmoEvalNewPoint_t dll_gmoEvalNewPoint;
        private static int d_gmoEvalNewPoint(IntPtr pgmo, double[] x)
        { gmoErrorHandling("gmoEvalNewPoint could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetExtFuncs_t(IntPtr pgmo, IntPtr extfunmgr);
        private static gmoSetExtFuncs_t dll_gmoSetExtFuncs;
        private static void d_gmoSetExtFuncs(IntPtr pgmo, IntPtr extfunmgr)
        { gmoErrorHandling("gmoSetExtFuncs could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFunc_t(IntPtr pgmo, int si, double[] x, ref double f, ref int numerr);
        private static gmoEvalFunc_t dll_gmoEvalFunc;
        private static int d_gmoEvalFunc(IntPtr pgmo, int si, double[] x, ref double f, ref int numerr)
        { gmoErrorHandling("gmoEvalFunc could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFunc_MT_t(IntPtr pgmo, int si, double[] x, ref double f, ref int numerr, int tidx);
        private static gmoEvalFunc_MT_t dll_gmoEvalFunc_MT;
        private static int d_gmoEvalFunc_MT(IntPtr pgmo, int si, double[] x, ref double f, ref int numerr, int tidx)
        { gmoErrorHandling("gmoEvalFunc_MT could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFuncInt_t(IntPtr pgmo, int si, ref double f, ref int numerr);
        private static gmoEvalFuncInt_t dll_gmoEvalFuncInt;
        private static int d_gmoEvalFuncInt(IntPtr pgmo, int si, ref double f, ref int numerr)
        { gmoErrorHandling("gmoEvalFuncInt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFuncInt_MT_t(IntPtr pgmo, int si, ref double f, ref int numerr, int tidx);
        private static gmoEvalFuncInt_MT_t dll_gmoEvalFuncInt_MT;
        private static int d_gmoEvalFuncInt_MT(IntPtr pgmo, int si, ref double f, ref int numerr, int tidx)
        { gmoErrorHandling("gmoEvalFuncInt_MT could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFuncNL_t(IntPtr pgmo, int si, double[] x, ref double fnl, ref int numerr);
        private static gmoEvalFuncNL_t dll_gmoEvalFuncNL;
        private static int d_gmoEvalFuncNL(IntPtr pgmo, int si, double[] x, ref double fnl, ref int numerr)
        { gmoErrorHandling("gmoEvalFuncNL could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFuncNL_MT_t(IntPtr pgmo, int si, double[] x, ref double fnl, ref int numerr, int tidx);
        private static gmoEvalFuncNL_MT_t dll_gmoEvalFuncNL_MT;
        private static int d_gmoEvalFuncNL_MT(IntPtr pgmo, int si, double[] x, ref double fnl, ref int numerr, int tidx)
        { gmoErrorHandling("gmoEvalFuncNL_MT could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFuncObj_t(IntPtr pgmo, double[] x, ref double f, ref int numerr);
        private static gmoEvalFuncObj_t dll_gmoEvalFuncObj;
        private static int d_gmoEvalFuncObj(IntPtr pgmo, double[] x, ref double f, ref int numerr)
        { gmoErrorHandling("gmoEvalFuncObj could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFuncNLObj_t(IntPtr pgmo, double[] x, ref double fnl, ref int numerr);
        private static gmoEvalFuncNLObj_t dll_gmoEvalFuncNLObj;
        private static int d_gmoEvalFuncNLObj(IntPtr pgmo, double[] x, ref double fnl, ref int numerr)
        { gmoErrorHandling("gmoEvalFuncNLObj could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFuncInterval_t(IntPtr pgmo, int si, double[] xmin, double[] xmax, ref double fmin, ref double fmax, ref int numerr);
        private static gmoEvalFuncInterval_t dll_gmoEvalFuncInterval;
        private static int d_gmoEvalFuncInterval(IntPtr pgmo, int si, double[] xmin, double[] xmax, ref double fmin, ref double fmax, ref int numerr)
        { gmoErrorHandling("gmoEvalFuncInterval could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFuncInterval_MT_t(IntPtr pgmo, int si, double[] xmin, double[] xmax, ref double fmin, ref double fmax, ref int numerr, int tidx);
        private static gmoEvalFuncInterval_MT_t dll_gmoEvalFuncInterval_MT;
        private static int d_gmoEvalFuncInterval_MT(IntPtr pgmo, int si, double[] xmin, double[] xmax, ref double fmin, ref double fmax, ref int numerr, int tidx)
        { gmoErrorHandling("gmoEvalFuncInterval_MT could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFuncNLCluster_t(IntPtr pgmo, int si, double[] x, int[] cluster, int ncluster, double[] fnl, ref int numerr);
        private static gmoEvalFuncNLCluster_t dll_gmoEvalFuncNLCluster;
        private static int d_gmoEvalFuncNLCluster(IntPtr pgmo, int si, double[] x, int[] cluster, int ncluster, double[] fnl, ref int numerr)
        { gmoErrorHandling("gmoEvalFuncNLCluster could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFuncNLCluster_MT_t(IntPtr pgmo, int si, double[] x, int[] cluster, int ncluster, double[] fnl, ref int numerr, int tidx);
        private static gmoEvalFuncNLCluster_MT_t dll_gmoEvalFuncNLCluster_MT;
        private static int d_gmoEvalFuncNLCluster_MT(IntPtr pgmo, int si, double[] x, int[] cluster, int ncluster, double[] fnl, ref int numerr, int tidx)
        { gmoErrorHandling("gmoEvalFuncNLCluster_MT could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalGrad_t(IntPtr pgmo, int si, double[] x, ref double f, double[] g, ref double gx, ref int numerr);
        private static gmoEvalGrad_t dll_gmoEvalGrad;
        private static int d_gmoEvalGrad(IntPtr pgmo, int si, double[] x, ref double f, double[] g, ref double gx, ref int numerr)
        { gmoErrorHandling("gmoEvalGrad could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalGrad_MT_t(IntPtr pgmo, int si, double[] x, ref double f, double[] g, ref double gx, ref int numerr, int tidx);
        private static gmoEvalGrad_MT_t dll_gmoEvalGrad_MT;
        private static int d_gmoEvalGrad_MT(IntPtr pgmo, int si, double[] x, ref double f, double[] g, ref double gx, ref int numerr, int tidx)
        { gmoErrorHandling("gmoEvalGrad_MT could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalGradNL_t(IntPtr pgmo, int si, double[] x, ref double fnl, double[] g, ref double gxnl, ref int numerr);
        private static gmoEvalGradNL_t dll_gmoEvalGradNL;
        private static int d_gmoEvalGradNL(IntPtr pgmo, int si, double[] x, ref double fnl, double[] g, ref double gxnl, ref int numerr)
        { gmoErrorHandling("gmoEvalGradNL could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalGradNL_MT_t(IntPtr pgmo, int si, double[] x, ref double fnl, double[] g, ref double gxnl, ref int numerr, int tidx);
        private static gmoEvalGradNL_MT_t dll_gmoEvalGradNL_MT;
        private static int d_gmoEvalGradNL_MT(IntPtr pgmo, int si, double[] x, ref double fnl, double[] g, ref double gxnl, ref int numerr, int tidx)
        { gmoErrorHandling("gmoEvalGradNL_MT could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalGradObj_t(IntPtr pgmo, double[] x, ref double f, double[] g, ref double gx, ref int numerr);
        private static gmoEvalGradObj_t dll_gmoEvalGradObj;
        private static int d_gmoEvalGradObj(IntPtr pgmo, double[] x, ref double f, double[] g, ref double gx, ref int numerr)
        { gmoErrorHandling("gmoEvalGradObj could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalGradNLObj_t(IntPtr pgmo, double[] x, ref double fnl, double[] g, ref double gxnl, ref int numerr);
        private static gmoEvalGradNLObj_t dll_gmoEvalGradNLObj;
        private static int d_gmoEvalGradNLObj(IntPtr pgmo, double[] x, ref double fnl, double[] g, ref double gxnl, ref int numerr)
        { gmoErrorHandling("gmoEvalGradNLObj could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalGradInterval_t(IntPtr pgmo, int si, double[] xmin, double[] xmax, ref double fmin, ref double fmax, double[] gmin, double[] gmax, ref int numerr);
        private static gmoEvalGradInterval_t dll_gmoEvalGradInterval;
        private static int d_gmoEvalGradInterval(IntPtr pgmo, int si, double[] xmin, double[] xmax, ref double fmin, ref double fmax, double[] gmin, double[] gmax, ref int numerr)
        { gmoErrorHandling("gmoEvalGradInterval could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalGradInterval_MT_t(IntPtr pgmo, int si, double[] xmin, double[] xmax, ref double fmin, ref double fmax, double[] gmin, double[] gmax, ref int numerr, int tidx);
        private static gmoEvalGradInterval_MT_t dll_gmoEvalGradInterval_MT;
        private static int d_gmoEvalGradInterval_MT(IntPtr pgmo, int si, double[] xmin, double[] xmax, ref double fmin, ref double fmax, double[] gmin, double[] gmax, ref int numerr, int tidx)
        { gmoErrorHandling("gmoEvalGradInterval_MT could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalGradNLUpdate_t(IntPtr pgmo, double[] rhsdelta, int dojacupd, ref int numerr);
        private static gmoEvalGradNLUpdate_t dll_gmoEvalGradNLUpdate;
        private static int d_gmoEvalGradNLUpdate(IntPtr pgmo, double[] rhsdelta, int dojacupd, ref int numerr)
        { gmoErrorHandling("gmoEvalGradNLUpdate could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetJacUpdate_t(IntPtr pgmo, int[] rowidx, int[] colidx, double[] jacval, ref int len);
        private static gmoGetJacUpdate_t dll_gmoGetJacUpdate;
        private static int d_gmoGetJacUpdate(IntPtr pgmo, int[] rowidx, int[] colidx, double[] jacval, ref int len)
        { gmoErrorHandling("gmoGetJacUpdate could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessLoad_t(IntPtr pgmo, double maxJacMult, ref int do2dir, ref int doHess);
        private static gmoHessLoad_t dll_gmoHessLoad;
        private static int d_gmoHessLoad(IntPtr pgmo, double maxJacMult, ref int do2dir, ref int doHess)
        { gmoErrorHandling("gmoHessLoad could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessUnload_t(IntPtr pgmo);
        private static gmoHessUnload_t dll_gmoHessUnload;
        private static int d_gmoHessUnload(IntPtr pgmo)
        { gmoErrorHandling("gmoHessUnload could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessDim_t(IntPtr pgmo, int si);
        private static gmoHessDim_t dll_gmoHessDim;
        private static int d_gmoHessDim(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoHessDim could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessNz_t(IntPtr pgmo, int si);
        private static gmoHessNz_t dll_gmoHessNz;
        private static int d_gmoHessNz(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoHessNz could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessStruct_t(IntPtr pgmo, int si, int[] hridx, int[] hcidx, ref int hessdim, ref int hessnz);
        private static gmoHessStruct_t dll_gmoHessStruct;
        private static int d_gmoHessStruct(IntPtr pgmo, int si, int[] hridx, int[] hcidx, ref int hessdim, ref int hessnz)
        { gmoErrorHandling("gmoHessStruct could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessValue_t(IntPtr pgmo, int si, int[] hridx, int[] hcidx, ref int hessdim, ref int hessnz, double[] x, double[] hessval, ref int numerr);
        private static gmoHessValue_t dll_gmoHessValue;
        private static int d_gmoHessValue(IntPtr pgmo, int si, int[] hridx, int[] hcidx, ref int hessdim, ref int hessnz, double[] x, double[] hessval, ref int numerr)
        { gmoErrorHandling("gmoHessValue could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessVec_t(IntPtr pgmo, int si, double[] x, double[] dx, double[] Wdx, ref int numerr);
        private static gmoHessVec_t dll_gmoHessVec;
        private static int d_gmoHessVec(IntPtr pgmo, int si, double[] x, double[] dx, double[] Wdx, ref int numerr)
        { gmoErrorHandling("gmoHessVec could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessLagStruct_t(IntPtr pgmo, int[] WRindex, int[] WCindex);
        private static gmoHessLagStruct_t dll_gmoHessLagStruct;
        private static int d_gmoHessLagStruct(IntPtr pgmo, int[] WRindex, int[] WCindex)
        { gmoErrorHandling("gmoHessLagStruct could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessLagValue_t(IntPtr pgmo, double[] x, double[] pi, double[] w, double objweight, double conweight, ref int numerr);
        private static gmoHessLagValue_t dll_gmoHessLagValue;
        private static int d_gmoHessLagValue(IntPtr pgmo, double[] x, double[] pi, double[] w, double objweight, double conweight, ref int numerr)
        { gmoErrorHandling("gmoHessLagValue could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessLagVec_t(IntPtr pgmo, double[] x, double[] pi, double[] dx, double[] Wdx, double objweight, double conweight, ref int numerr);
        private static gmoHessLagVec_t dll_gmoHessLagVec;
        private static int d_gmoHessLagVec(IntPtr pgmo, double[] x, double[] pi, double[] dx, double[] Wdx, double objweight, double conweight, ref int numerr)
        { gmoErrorHandling("gmoHessLagVec could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoLoadEMPInfo_t(IntPtr pgmo, string empinfofname);
        private static gmoLoadEMPInfo_t dll_gmoLoadEMPInfo;
        private static int d_gmoLoadEMPInfo(IntPtr pgmo, string empinfofname)
        { gmoErrorHandling("gmoLoadEMPInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquVI_t(IntPtr pgmo, int[] mintvec);
        private static gmoGetEquVI_t dll_gmoGetEquVI;
        private static int d_gmoGetEquVI(IntPtr pgmo, int[] mintvec)
        { gmoErrorHandling("gmoGetEquVI could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquVIOne_t(IntPtr pgmo, int si);
        private static gmoGetEquVIOne_t dll_gmoGetEquVIOne;
        private static int d_gmoGetEquVIOne(IntPtr pgmo, int si)
        { gmoErrorHandling("gmoGetEquVIOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarVI_t(IntPtr pgmo, int[] nintvec);
        private static gmoGetVarVI_t dll_gmoGetVarVI;
        private static int d_gmoGetVarVI(IntPtr pgmo, int[] nintvec)
        { gmoErrorHandling("gmoGetVarVI could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarVIOne_t(IntPtr pgmo, int sj);
        private static gmoGetVarVIOne_t dll_gmoGetVarVIOne;
        private static int d_gmoGetVarVIOne(IntPtr pgmo, int sj)
        { gmoErrorHandling("gmoGetVarVIOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetAgentType_t(IntPtr pgmo, int[] agentvec);
        private static gmoGetAgentType_t dll_gmoGetAgentType;
        private static int d_gmoGetAgentType(IntPtr pgmo, int[] agentvec)
        { gmoErrorHandling("gmoGetAgentType could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetAgentTypeOne_t(IntPtr pgmo, int aidx);
        private static gmoGetAgentTypeOne_t dll_gmoGetAgentTypeOne;
        private static int d_gmoGetAgentTypeOne(IntPtr pgmo, int aidx)
        { gmoErrorHandling("gmoGetAgentTypeOne could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetBiLevelInfo_t(IntPtr pgmo, int[] nintvec, int[] mintvec);
        private static gmoGetBiLevelInfo_t dll_gmoGetBiLevelInfo;
        private static int d_gmoGetBiLevelInfo(IntPtr pgmo, int[] nintvec, int[] mintvec)
        { gmoErrorHandling("gmoGetBiLevelInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoDumpEMPInfoToGDX_t(IntPtr pgmo, string gdxfname);
        private static gmoDumpEMPInfoToGDX_t dll_gmoDumpEMPInfoToGDX;
        private static int d_gmoDumpEMPInfoToGDX(IntPtr pgmo, string gdxfname)
        { gmoErrorHandling("gmoDumpEMPInfoToGDX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetHeadnTail_t(IntPtr pgmo, int htrec);
        private static gmoGetHeadnTail_t dll_gmoGetHeadnTail;
        private static double d_gmoGetHeadnTail(IntPtr pgmo, int htrec)
        { gmoErrorHandling("gmoGetHeadnTail could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSetHeadnTail_t(IntPtr pgmo, int htrec, double dval);
        private static gmoSetHeadnTail_t dll_gmoSetHeadnTail;
        private static void d_gmoSetHeadnTail(IntPtr pgmo, int htrec, double dval)
        { gmoErrorHandling("gmoSetHeadnTail could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetSolutionPrimal_t(IntPtr pgmo, double[] x);
        private static gmoSetSolutionPrimal_t dll_gmoSetSolutionPrimal;
        private static int d_gmoSetSolutionPrimal(IntPtr pgmo, double[] x)
        { gmoErrorHandling("gmoSetSolutionPrimal could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetSolution2_t(IntPtr pgmo, double[] x, double[] pi);
        private static gmoSetSolution2_t dll_gmoSetSolution2;
        private static int d_gmoSetSolution2(IntPtr pgmo, double[] x, double[] pi)
        { gmoErrorHandling("gmoSetSolution2 could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetSolution_t(IntPtr pgmo, double[] x, double[] dj, double[] pi, double[] e);
        private static gmoSetSolution_t dll_gmoSetSolution;
        private static int d_gmoSetSolution(IntPtr pgmo, double[] x, double[] dj, double[] pi, double[] e)
        { gmoErrorHandling("gmoSetSolution could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetSolution8_t(IntPtr pgmo, double[] x, double[] dj, double[] pi, double[] e, int[] xb, int[] xs, int[] yb, int[] ys);
        private static gmoSetSolution8_t dll_gmoSetSolution8;
        private static int d_gmoSetSolution8(IntPtr pgmo, double[] x, double[] dj, double[] pi, double[] e, int[] xb, int[] xs, int[] yb, int[] ys)
        { gmoErrorHandling("gmoSetSolution8 could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetSolutionFixer_t(IntPtr pgmo, int modelstathint, double[] x, double[] pi, int[] xb, int[] yb, double infTol, double optTol);
        private static gmoSetSolutionFixer_t dll_gmoSetSolutionFixer;
        private static int d_gmoSetSolutionFixer(IntPtr pgmo, int modelstathint, double[] x, double[] pi, int[] xb, int[] yb, double infTol, double optTol)
        { gmoErrorHandling("gmoSetSolutionFixer could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetSolutionVarRec_t(IntPtr pgmo, int sj, ref double vl, ref double vmarg, ref int vstat, ref int vcstat);
        private static gmoGetSolutionVarRec_t dll_gmoGetSolutionVarRec;
        private static int d_gmoGetSolutionVarRec(IntPtr pgmo, int sj, ref double vl, ref double vmarg, ref int vstat, ref int vcstat)
        { gmoErrorHandling("gmoGetSolutionVarRec could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetSolutionVarRec_t(IntPtr pgmo, int sj, double vl, double vmarg, int vstat, int vcstat);
        private static gmoSetSolutionVarRec_t dll_gmoSetSolutionVarRec;
        private static int d_gmoSetSolutionVarRec(IntPtr pgmo, int sj, double vl, double vmarg, int vstat, int vcstat)
        { gmoErrorHandling("gmoSetSolutionVarRec could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetSolutionEquRec_t(IntPtr pgmo, int si, ref double el, ref double emarg, ref int estat, ref int ecstat);
        private static gmoGetSolutionEquRec_t dll_gmoGetSolutionEquRec;
        private static int d_gmoGetSolutionEquRec(IntPtr pgmo, int si, ref double el, ref double emarg, ref int estat, ref int ecstat)
        { gmoErrorHandling("gmoGetSolutionEquRec could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetSolutionEquRec_t(IntPtr pgmo, int si, double el, double emarg, int estat, int ecstat);
        private static gmoSetSolutionEquRec_t dll_gmoSetSolutionEquRec;
        private static int d_gmoSetSolutionEquRec(IntPtr pgmo, int si, double el, double emarg, int estat, int ecstat)
        { gmoErrorHandling("gmoSetSolutionEquRec could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetSolutionStatus_t(IntPtr pgmo, int[] xb, int[] xs, int[] yb, int[] ys);
        private static gmoSetSolutionStatus_t dll_gmoSetSolutionStatus;
        private static int d_gmoSetSolutionStatus(IntPtr pgmo, int[] xb, int[] xs, int[] yb, int[] ys)
        { gmoErrorHandling("gmoSetSolutionStatus could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoCompleteObjective_t(IntPtr pgmo, double locobjval);
        private static gmoCompleteObjective_t dll_gmoCompleteObjective;
        private static void d_gmoCompleteObjective(IntPtr pgmo, double locobjval)
        { gmoErrorHandling("gmoCompleteObjective could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoCompleteSolution_t(IntPtr pgmo);
        private static gmoCompleteSolution_t dll_gmoCompleteSolution;
        private static int d_gmoCompleteSolution(IntPtr pgmo)
        { gmoErrorHandling("gmoCompleteSolution could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetAbsoluteGap_t(IntPtr pgmo);
        private static gmoGetAbsoluteGap_t dll_gmoGetAbsoluteGap;
        private static double d_gmoGetAbsoluteGap(IntPtr pgmo)
        { gmoErrorHandling("gmoGetAbsoluteGap could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoGetRelativeGap_t(IntPtr pgmo);
        private static gmoGetRelativeGap_t dll_gmoGetRelativeGap;
        private static double d_gmoGetRelativeGap(IntPtr pgmo)
        { gmoErrorHandling("gmoGetRelativeGap could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoLoadSolutionLegacy_t(IntPtr pgmo);
        private static gmoLoadSolutionLegacy_t dll_gmoLoadSolutionLegacy;
        private static int d_gmoLoadSolutionLegacy(IntPtr pgmo)
        { gmoErrorHandling("gmoLoadSolutionLegacy could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoUnloadSolutionLegacy_t(IntPtr pgmo);
        private static gmoUnloadSolutionLegacy_t dll_gmoUnloadSolutionLegacy;
        private static int d_gmoUnloadSolutionLegacy(IntPtr pgmo)
        { gmoErrorHandling("gmoUnloadSolutionLegacy could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoLoadSolutionGDX_t(IntPtr pgmo, string gdxfname, int dorows, int docols, int doht);
        private static gmoLoadSolutionGDX_t dll_gmoLoadSolutionGDX;
        private static int d_gmoLoadSolutionGDX(IntPtr pgmo, string gdxfname, int dorows, int docols, int doht)
        { gmoErrorHandling("gmoLoadSolutionGDX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoUnloadSolutionGDX_t(IntPtr pgmo, string gdxfname, int dorows, int docols, int doht);
        private static gmoUnloadSolutionGDX_t dll_gmoUnloadSolutionGDX;
        private static int d_gmoUnloadSolutionGDX(IntPtr pgmo, string gdxfname, int dorows, int docols, int doht)
        { gmoErrorHandling("gmoUnloadSolutionGDX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoPrepareAllSolToGDX_t(IntPtr pgmo, string gdxfname, IntPtr scengdx, int dictid);
        private static gmoPrepareAllSolToGDX_t dll_gmoPrepareAllSolToGDX;
        private static int d_gmoPrepareAllSolToGDX(IntPtr pgmo, string gdxfname, IntPtr scengdx, int dictid)
        { gmoErrorHandling("gmoPrepareAllSolToGDX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoAddSolutionToGDX_t(IntPtr pgmo, string[] scenuel);
        private static gmoAddSolutionToGDX_t dll_gmoAddSolutionToGDX;
        private static int d_gmoAddSolutionToGDX(IntPtr pgmo, string[] scenuel)
        { gmoErrorHandling("gmoAddSolutionToGDX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoWriteSolDone_t(IntPtr pgmo, StringBuilder msg);
        private static gmoWriteSolDone_t dll_gmoWriteSolDone;
        private static int d_gmoWriteSolDone(IntPtr pgmo, StringBuilder msg)
        { gmoErrorHandling("gmoWriteSolDone could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoCheckSolPoolUEL_t(IntPtr pgmo, string prefix, ref int numsym);
        private static gmoCheckSolPoolUEL_t dll_gmoCheckSolPoolUEL;
        private static int d_gmoCheckSolPoolUEL(IntPtr pgmo, string prefix, ref int numsym)
        { gmoErrorHandling("gmoCheckSolPoolUEL could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr gmoPrepareSolPoolMerge_t(IntPtr pgmo, string gdxfname, int numsol, string prefix);
        private static gmoPrepareSolPoolMerge_t dll_gmoPrepareSolPoolMerge;
        private static IntPtr d_gmoPrepareSolPoolMerge(IntPtr pgmo, string gdxfname, int numsol, string prefix)
        { gmoErrorHandling("gmoPrepareSolPoolMerge could not be loaded"); return IntPtr.Zero; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoPrepareSolPoolNextSym_t(IntPtr pgmo, IntPtr handle);
        private static gmoPrepareSolPoolNextSym_t dll_gmoPrepareSolPoolNextSym;
        private static int d_gmoPrepareSolPoolNextSym(IntPtr pgmo, IntPtr handle)
        { gmoErrorHandling("gmoPrepareSolPoolNextSym could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoUnloadSolPoolSolution_t(IntPtr pgmo, IntPtr handle, int numsol);
        private static gmoUnloadSolPoolSolution_t dll_gmoUnloadSolPoolSolution;
        private static int d_gmoUnloadSolPoolSolution(IntPtr pgmo, IntPtr handle, int numsol)
        { gmoErrorHandling("gmoUnloadSolPoolSolution could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoFinalizeSolPoolMerge_t(IntPtr pgmo, IntPtr handle);
        private static gmoFinalizeSolPoolMerge_t dll_gmoFinalizeSolPoolMerge;
        private static int d_gmoFinalizeSolPoolMerge(IntPtr pgmo, IntPtr handle)
        { gmoErrorHandling("gmoFinalizeSolPoolMerge could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarTypeTxt_t(IntPtr pgmo, int sj, StringBuilder s);
        private static gmoGetVarTypeTxt_t dll_gmoGetVarTypeTxt;
        private static int d_gmoGetVarTypeTxt(IntPtr pgmo, int sj, StringBuilder s)
        { gmoErrorHandling("gmoGetVarTypeTxt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetEquTypeTxt_t(IntPtr pgmo, int si, StringBuilder s);
        private static gmoGetEquTypeTxt_t dll_gmoGetEquTypeTxt;
        private static int d_gmoGetEquTypeTxt(IntPtr pgmo, int si, StringBuilder s)
        { gmoErrorHandling("gmoGetEquTypeTxt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetSolveStatusTxt_t(IntPtr pgmo, int solvestat, StringBuilder s);
        private static gmoGetSolveStatusTxt_t dll_gmoGetSolveStatusTxt;
        private static int d_gmoGetSolveStatusTxt(IntPtr pgmo, int solvestat, StringBuilder s)
        { gmoErrorHandling("gmoGetSolveStatusTxt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetModelStatusTxt_t(IntPtr pgmo, int modelstat, StringBuilder s);
        private static gmoGetModelStatusTxt_t dll_gmoGetModelStatusTxt;
        private static int d_gmoGetModelStatusTxt(IntPtr pgmo, int modelstat, StringBuilder s)
        { gmoErrorHandling("gmoGetModelStatusTxt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetModelTypeTxt_t(IntPtr pgmo, int modeltype, StringBuilder s);
        private static gmoGetModelTypeTxt_t dll_gmoGetModelTypeTxt;
        private static int d_gmoGetModelTypeTxt(IntPtr pgmo, int modeltype, StringBuilder s)
        { gmoErrorHandling("gmoGetModelTypeTxt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetHeadNTailTxt_t(IntPtr pgmo, int htrec, StringBuilder s);
        private static gmoGetHeadNTailTxt_t dll_gmoGetHeadNTailTxt;
        private static int d_gmoGetHeadNTailTxt(IntPtr pgmo, int htrec, StringBuilder s)
        { gmoErrorHandling("gmoGetHeadNTailTxt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoMemUsed_t(IntPtr pgmo);
        private static gmoMemUsed_t dll_gmoMemUsed;
        private static double d_gmoMemUsed(IntPtr pgmo)
        { gmoErrorHandling("gmoMemUsed could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoPeakMemUsed_t(IntPtr pgmo);
        private static gmoPeakMemUsed_t dll_gmoPeakMemUsed;
        private static double d_gmoPeakMemUsed(IntPtr pgmo)
        { gmoErrorHandling("gmoPeakMemUsed could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSetNLObject_t(IntPtr pgmo, IntPtr nlobject, IntPtr nlpool);
        private static gmoSetNLObject_t dll_gmoSetNLObject;
        private static int d_gmoSetNLObject(IntPtr pgmo, IntPtr nlobject, IntPtr nlpool)
        { gmoErrorHandling("gmoSetNLObject could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoDumpQMakerGDX_t(IntPtr pgmo, string gdxfname);
        private static gmoDumpQMakerGDX_t dll_gmoDumpQMakerGDX;
        private static int d_gmoDumpQMakerGDX(IntPtr pgmo, string gdxfname)
        { gmoErrorHandling("gmoDumpQMakerGDX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetVarEquMap_t(IntPtr pgmo, int maptype, IntPtr optptr, int strict, ref int nmappings, int[] rowindex, int[] colindex, int[] mapval);
        private static gmoGetVarEquMap_t dll_gmoGetVarEquMap;
        private static int d_gmoGetVarEquMap(IntPtr pgmo, int maptype, IntPtr optptr, int strict, ref int nmappings, int[] rowindex, int[] colindex, int[] mapval)
        { gmoErrorHandling("gmoGetVarEquMap could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetIndicatorMap_t(IntPtr pgmo, IntPtr optptr, int indicstrict, ref int numindic, int[] rowindic, int[] colindic, int[] indiconval);
        private static gmoGetIndicatorMap_t dll_gmoGetIndicatorMap;
        private static int d_gmoGetIndicatorMap(IntPtr pgmo, IntPtr optptr, int indicstrict, ref int numindic, int[] rowindic, int[] colindic, int[] indiconval)
        { gmoErrorHandling("gmoGetIndicatorMap could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoCrudeness_t(IntPtr pgmo);
        private static gmoCrudeness_t dll_gmoCrudeness;
        private static int d_gmoCrudeness(IntPtr pgmo)
        { gmoErrorHandling("gmoCrudeness could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoDirtyGetRowFNLInstr_t(IntPtr pgmo, int si, ref int len, int[] opcode, int[] field);
        private static gmoDirtyGetRowFNLInstr_t dll_gmoDirtyGetRowFNLInstr;
        private static int d_gmoDirtyGetRowFNLInstr(IntPtr pgmo, int si, ref int len, int[] opcode, int[] field)
        { gmoErrorHandling("gmoDirtyGetRowFNLInstr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoDirtyGetObjFNLInstr_t(IntPtr pgmo, ref int len, int[] opcode, int[] field);
        private static gmoDirtyGetObjFNLInstr_t dll_gmoDirtyGetObjFNLInstr;
        private static int d_gmoDirtyGetObjFNLInstr(IntPtr pgmo, ref int len, int[] opcode, int[] field)
        { gmoErrorHandling("gmoDirtyGetObjFNLInstr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoDirtySetRowFNLInstr_t(IntPtr pgmo, int si, int len, int[] opcode, int[] field, IntPtr nlpool, double[] nlpoolvec, int len2);
        private static gmoDirtySetRowFNLInstr_t dll_gmoDirtySetRowFNLInstr;
        private static int d_gmoDirtySetRowFNLInstr(IntPtr pgmo, int si, int len, int[] opcode, int[] field, IntPtr nlpool, double[] nlpoolvec, int len2)
        { gmoErrorHandling("gmoDirtySetRowFNLInstr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetExtrLibName_t(IntPtr pgmo, int libidx, StringBuilder sst_result);
        private static gmoGetExtrLibName_t dll_gmoGetExtrLibName;
        private static void d_gmoGetExtrLibName(IntPtr pgmo, int libidx, StringBuilder sst_result)
        { gmoErrorHandling("gmoGetExtrLibName could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr gmoGetExtrLibObjPtr_t(IntPtr pgmo, int libidx);
        private static gmoGetExtrLibObjPtr_t dll_gmoGetExtrLibObjPtr;
        private static IntPtr d_gmoGetExtrLibObjPtr(IntPtr pgmo, int libidx)
        { gmoErrorHandling("gmoGetExtrLibObjPtr could not be loaded"); return IntPtr.Zero; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoGetExtrLibFuncName_t(IntPtr pgmo, int libidx, int funcidx, StringBuilder sst_result);
        private static gmoGetExtrLibFuncName_t dll_gmoGetExtrLibFuncName;
        private static void d_gmoGetExtrLibFuncName(IntPtr pgmo, int libidx, int funcidx, StringBuilder sst_result)
        { gmoErrorHandling("gmoGetExtrLibFuncName could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr gmoLoadExtrLibEntry_t(IntPtr pgmo, int libidx, string name, StringBuilder msg);
        private static gmoLoadExtrLibEntry_t dll_gmoLoadExtrLibEntry;
        private static IntPtr d_gmoLoadExtrLibEntry(IntPtr pgmo, int libidx, string name, StringBuilder msg)
        { gmoErrorHandling("gmoLoadExtrLibEntry could not be loaded"); return IntPtr.Zero; }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr gmoDict_t(IntPtr pgmo);
        private static gmoDict_t dll_gmoDict;
        private static IntPtr d_gmoDict(IntPtr pgmo)
        { gmoErrorHandling("gmoDict could not be loaded"); return IntPtr.Zero; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoDictSet_t(IntPtr pgmo, IntPtr x);
        private static gmoDictSet_t dll_gmoDictSet;
        private static void d_gmoDictSet(IntPtr pgmo, IntPtr x)
        { gmoErrorHandling("gmoDictSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameModel_t(IntPtr pgmo, StringBuilder sst_result);
        private static gmoNameModel_t dll_gmoNameModel;
        private static void d_gmoNameModel(IntPtr pgmo, StringBuilder sst_result)
        { gmoErrorHandling("gmoNameModel could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameModelSet_t(IntPtr pgmo, string x);
        private static gmoNameModelSet_t dll_gmoNameModelSet;
        private static void d_gmoNameModelSet(IntPtr pgmo, string x)
        { gmoErrorHandling("gmoNameModelSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoModelSeqNr_t(IntPtr pgmo);
        private static gmoModelSeqNr_t dll_gmoModelSeqNr;
        private static int d_gmoModelSeqNr(IntPtr pgmo)
        { gmoErrorHandling("gmoModelSeqNr could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoModelSeqNrSet_t(IntPtr pgmo, int x);
        private static gmoModelSeqNrSet_t dll_gmoModelSeqNrSet;
        private static void d_gmoModelSeqNrSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoModelSeqNrSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoModelType_t(IntPtr pgmo);
        private static gmoModelType_t dll_gmoModelType;
        private static int d_gmoModelType(IntPtr pgmo)
        { gmoErrorHandling("gmoModelType could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoModelTypeSet_t(IntPtr pgmo, int x);
        private static gmoModelTypeSet_t dll_gmoModelTypeSet;
        private static void d_gmoModelTypeSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoModelTypeSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNLModelType_t(IntPtr pgmo);
        private static gmoNLModelType_t dll_gmoNLModelType;
        private static int d_gmoNLModelType(IntPtr pgmo)
        { gmoErrorHandling("gmoNLModelType could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSense_t(IntPtr pgmo);
        private static gmoSense_t dll_gmoSense;
        private static int d_gmoSense(IntPtr pgmo)
        { gmoErrorHandling("gmoSense could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSenseSet_t(IntPtr pgmo, int x);
        private static gmoSenseSet_t dll_gmoSenseSet;
        private static void d_gmoSenseSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoSenseSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoIsQP_t(IntPtr pgmo);
        private static gmoIsQP_t dll_gmoIsQP;
        private static int d_gmoIsQP(IntPtr pgmo)
        { gmoErrorHandling("gmoIsQP could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoOptFile_t(IntPtr pgmo);
        private static gmoOptFile_t dll_gmoOptFile;
        private static int d_gmoOptFile(IntPtr pgmo)
        { gmoErrorHandling("gmoOptFile could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoOptFileSet_t(IntPtr pgmo, int x);
        private static gmoOptFileSet_t dll_gmoOptFileSet;
        private static void d_gmoOptFileSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoOptFileSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoDictionary_t(IntPtr pgmo);
        private static gmoDictionary_t dll_gmoDictionary;
        private static int d_gmoDictionary(IntPtr pgmo)
        { gmoErrorHandling("gmoDictionary could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoDictionarySet_t(IntPtr pgmo, int x);
        private static gmoDictionarySet_t dll_gmoDictionarySet;
        private static void d_gmoDictionarySet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoDictionarySet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoScaleOpt_t(IntPtr pgmo);
        private static gmoScaleOpt_t dll_gmoScaleOpt;
        private static int d_gmoScaleOpt(IntPtr pgmo)
        { gmoErrorHandling("gmoScaleOpt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoScaleOptSet_t(IntPtr pgmo, int x);
        private static gmoScaleOptSet_t dll_gmoScaleOptSet;
        private static void d_gmoScaleOptSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoScaleOptSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoPriorOpt_t(IntPtr pgmo);
        private static gmoPriorOpt_t dll_gmoPriorOpt;
        private static int d_gmoPriorOpt(IntPtr pgmo)
        { gmoErrorHandling("gmoPriorOpt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoPriorOptSet_t(IntPtr pgmo, int x);
        private static gmoPriorOptSet_t dll_gmoPriorOptSet;
        private static void d_gmoPriorOptSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoPriorOptSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHaveBasis_t(IntPtr pgmo);
        private static gmoHaveBasis_t dll_gmoHaveBasis;
        private static int d_gmoHaveBasis(IntPtr pgmo)
        { gmoErrorHandling("gmoHaveBasis could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoHaveBasisSet_t(IntPtr pgmo, int x);
        private static gmoHaveBasisSet_t dll_gmoHaveBasisSet;
        private static void d_gmoHaveBasisSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoHaveBasisSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoModelStat_t(IntPtr pgmo);
        private static gmoModelStat_t dll_gmoModelStat;
        private static int d_gmoModelStat(IntPtr pgmo)
        { gmoErrorHandling("gmoModelStat could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoModelStatSet_t(IntPtr pgmo, int x);
        private static gmoModelStatSet_t dll_gmoModelStatSet;
        private static void d_gmoModelStatSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoModelStatSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoSolveStat_t(IntPtr pgmo);
        private static gmoSolveStat_t dll_gmoSolveStat;
        private static int d_gmoSolveStat(IntPtr pgmo)
        { gmoErrorHandling("gmoSolveStat could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoSolveStatSet_t(IntPtr pgmo, int x);
        private static gmoSolveStatSet_t dll_gmoSolveStatSet;
        private static void d_gmoSolveStatSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoSolveStatSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoIsMPSGE_t(IntPtr pgmo);
        private static gmoIsMPSGE_t dll_gmoIsMPSGE;
        private static int d_gmoIsMPSGE(IntPtr pgmo)
        { gmoErrorHandling("gmoIsMPSGE could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoIsMPSGESet_t(IntPtr pgmo, int x);
        private static gmoIsMPSGESet_t dll_gmoIsMPSGESet;
        private static void d_gmoIsMPSGESet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoIsMPSGESet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjStyle_t(IntPtr pgmo);
        private static gmoObjStyle_t dll_gmoObjStyle;
        private static int d_gmoObjStyle(IntPtr pgmo)
        { gmoErrorHandling("gmoObjStyle could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoObjStyleSet_t(IntPtr pgmo, int x);
        private static gmoObjStyleSet_t dll_gmoObjStyleSet;
        private static void d_gmoObjStyleSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoObjStyleSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoInterface_t(IntPtr pgmo);
        private static gmoInterface_t dll_gmoInterface;
        private static int d_gmoInterface(IntPtr pgmo)
        { gmoErrorHandling("gmoInterface could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoInterfaceSet_t(IntPtr pgmo, int x);
        private static gmoInterfaceSet_t dll_gmoInterfaceSet;
        private static void d_gmoInterfaceSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoInterfaceSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoIndexBase_t(IntPtr pgmo);
        private static gmoIndexBase_t dll_gmoIndexBase;
        private static int d_gmoIndexBase(IntPtr pgmo)
        { gmoErrorHandling("gmoIndexBase could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoIndexBaseSet_t(IntPtr pgmo, int x);
        private static gmoIndexBaseSet_t dll_gmoIndexBaseSet;
        private static void d_gmoIndexBaseSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoIndexBaseSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjReform_t(IntPtr pgmo);
        private static gmoObjReform_t dll_gmoObjReform;
        private static int d_gmoObjReform(IntPtr pgmo)
        { gmoErrorHandling("gmoObjReform could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoObjReformSet_t(IntPtr pgmo, int x);
        private static gmoObjReformSet_t dll_gmoObjReformSet;
        private static void d_gmoObjReformSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoObjReformSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEmptyOut_t(IntPtr pgmo);
        private static gmoEmptyOut_t dll_gmoEmptyOut;
        private static int d_gmoEmptyOut(IntPtr pgmo)
        { gmoErrorHandling("gmoEmptyOut could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoEmptyOutSet_t(IntPtr pgmo, int x);
        private static gmoEmptyOutSet_t dll_gmoEmptyOutSet;
        private static void d_gmoEmptyOutSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoEmptyOutSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoIgnXCDeriv_t(IntPtr pgmo);
        private static gmoIgnXCDeriv_t dll_gmoIgnXCDeriv;
        private static int d_gmoIgnXCDeriv(IntPtr pgmo)
        { gmoErrorHandling("gmoIgnXCDeriv could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoIgnXCDerivSet_t(IntPtr pgmo, int x);
        private static gmoIgnXCDerivSet_t dll_gmoIgnXCDerivSet;
        private static void d_gmoIgnXCDerivSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoIgnXCDerivSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoUseQ_t(IntPtr pgmo);
        private static gmoUseQ_t dll_gmoUseQ;
        private static int d_gmoUseQ(IntPtr pgmo)
        { gmoErrorHandling("gmoUseQ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoUseQSet_t(IntPtr pgmo, int x);
        private static gmoUseQSet_t dll_gmoUseQSet;
        private static void d_gmoUseQSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoUseQSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoQExtractAlg_t(IntPtr pgmo);
        private static gmoQExtractAlg_t dll_gmoQExtractAlg;
        private static int d_gmoQExtractAlg(IntPtr pgmo)
        { gmoErrorHandling("gmoQExtractAlg could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoQExtractAlgSet_t(IntPtr pgmo, int x);
        private static gmoQExtractAlgSet_t dll_gmoQExtractAlgSet;
        private static void d_gmoQExtractAlgSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoQExtractAlgSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoAltBounds_t(IntPtr pgmo);
        private static gmoAltBounds_t dll_gmoAltBounds;
        private static int d_gmoAltBounds(IntPtr pgmo)
        { gmoErrorHandling("gmoAltBounds could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoAltBoundsSet_t(IntPtr pgmo, int x);
        private static gmoAltBoundsSet_t dll_gmoAltBoundsSet;
        private static void d_gmoAltBoundsSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoAltBoundsSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoAltRHS_t(IntPtr pgmo);
        private static gmoAltRHS_t dll_gmoAltRHS;
        private static int d_gmoAltRHS(IntPtr pgmo)
        { gmoErrorHandling("gmoAltRHS could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoAltRHSSet_t(IntPtr pgmo, int x);
        private static gmoAltRHSSet_t dll_gmoAltRHSSet;
        private static void d_gmoAltRHSSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoAltRHSSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoAltVarTypes_t(IntPtr pgmo);
        private static gmoAltVarTypes_t dll_gmoAltVarTypes;
        private static int d_gmoAltVarTypes(IntPtr pgmo)
        { gmoErrorHandling("gmoAltVarTypes could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoAltVarTypesSet_t(IntPtr pgmo, int x);
        private static gmoAltVarTypesSet_t dll_gmoAltVarTypesSet;
        private static void d_gmoAltVarTypesSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoAltVarTypesSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoForceLinear_t(IntPtr pgmo);
        private static gmoForceLinear_t dll_gmoForceLinear;
        private static int d_gmoForceLinear(IntPtr pgmo)
        { gmoErrorHandling("gmoForceLinear could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoForceLinearSet_t(IntPtr pgmo, int x);
        private static gmoForceLinearSet_t dll_gmoForceLinearSet;
        private static void d_gmoForceLinearSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoForceLinearSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoForceCont_t(IntPtr pgmo);
        private static gmoForceCont_t dll_gmoForceCont;
        private static int d_gmoForceCont(IntPtr pgmo)
        { gmoErrorHandling("gmoForceCont could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoForceContSet_t(IntPtr pgmo, int x);
        private static gmoForceContSet_t dll_gmoForceContSet;
        private static void d_gmoForceContSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoForceContSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoPermuteCols_t(IntPtr pgmo);
        private static gmoPermuteCols_t dll_gmoPermuteCols;
        private static int d_gmoPermuteCols(IntPtr pgmo)
        { gmoErrorHandling("gmoPermuteCols could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoPermuteColsSet_t(IntPtr pgmo, int x);
        private static gmoPermuteColsSet_t dll_gmoPermuteColsSet;
        private static void d_gmoPermuteColsSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoPermuteColsSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoPermuteRows_t(IntPtr pgmo);
        private static gmoPermuteRows_t dll_gmoPermuteRows;
        private static int d_gmoPermuteRows(IntPtr pgmo)
        { gmoErrorHandling("gmoPermuteRows could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoPermuteRowsSet_t(IntPtr pgmo, int x);
        private static gmoPermuteRowsSet_t dll_gmoPermuteRowsSet;
        private static void d_gmoPermuteRowsSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoPermuteRowsSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoPinf_t(IntPtr pgmo);
        private static gmoPinf_t dll_gmoPinf;
        private static double d_gmoPinf(IntPtr pgmo)
        { gmoErrorHandling("gmoPinf could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoPinfSet_t(IntPtr pgmo, double x);
        private static gmoPinfSet_t dll_gmoPinfSet;
        private static void d_gmoPinfSet(IntPtr pgmo, double x)
        { gmoErrorHandling("gmoPinfSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoMinf_t(IntPtr pgmo);
        private static gmoMinf_t dll_gmoMinf;
        private static double d_gmoMinf(IntPtr pgmo)
        { gmoErrorHandling("gmoMinf could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoMinfSet_t(IntPtr pgmo, double x);
        private static gmoMinfSet_t dll_gmoMinfSet;
        private static void d_gmoMinfSet(IntPtr pgmo, double x)
        { gmoErrorHandling("gmoMinfSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoQNaN_t(IntPtr pgmo);
        private static gmoQNaN_t dll_gmoQNaN;
        private static double d_gmoQNaN(IntPtr pgmo)
        { gmoErrorHandling("gmoQNaN could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoValNA_t(IntPtr pgmo);
        private static gmoValNA_t dll_gmoValNA;
        private static double d_gmoValNA(IntPtr pgmo)
        { gmoErrorHandling("gmoValNA could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoValNAInt_t(IntPtr pgmo);
        private static gmoValNAInt_t dll_gmoValNAInt;
        private static int d_gmoValNAInt(IntPtr pgmo)
        { gmoErrorHandling("gmoValNAInt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoValUndf_t(IntPtr pgmo);
        private static gmoValUndf_t dll_gmoValUndf;
        private static double d_gmoValUndf(IntPtr pgmo)
        { gmoErrorHandling("gmoValUndf could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoM_t(IntPtr pgmo);
        private static gmoM_t dll_gmoM;
        private static int d_gmoM(IntPtr pgmo)
        { gmoErrorHandling("gmoM could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoQM_t(IntPtr pgmo);
        private static gmoQM_t dll_gmoQM;
        private static int d_gmoQM(IntPtr pgmo)
        { gmoErrorHandling("gmoQM could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNLM_t(IntPtr pgmo);
        private static gmoNLM_t dll_gmoNLM;
        private static int d_gmoNLM(IntPtr pgmo)
        { gmoErrorHandling("gmoNLM could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNRowMatch_t(IntPtr pgmo);
        private static gmoNRowMatch_t dll_gmoNRowMatch;
        private static int d_gmoNRowMatch(IntPtr pgmo)
        { gmoErrorHandling("gmoNRowMatch could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoN_t(IntPtr pgmo);
        private static gmoN_t dll_gmoN;
        private static int d_gmoN(IntPtr pgmo)
        { gmoErrorHandling("gmoN could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNLN_t(IntPtr pgmo);
        private static gmoNLN_t dll_gmoNLN;
        private static int d_gmoNLN(IntPtr pgmo)
        { gmoErrorHandling("gmoNLN could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNDisc_t(IntPtr pgmo);
        private static gmoNDisc_t dll_gmoNDisc;
        private static int d_gmoNDisc(IntPtr pgmo)
        { gmoErrorHandling("gmoNDisc could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNFixed_t(IntPtr pgmo);
        private static gmoNFixed_t dll_gmoNFixed;
        private static int d_gmoNFixed(IntPtr pgmo)
        { gmoErrorHandling("gmoNFixed could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNColMatch_t(IntPtr pgmo);
        private static gmoNColMatch_t dll_gmoNColMatch;
        private static int d_gmoNColMatch(IntPtr pgmo)
        { gmoErrorHandling("gmoNColMatch could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNZ_t(IntPtr pgmo);
        private static gmoNZ_t dll_gmoNZ;
        private static int d_gmoNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Int64 gmoNZ64_t(IntPtr pgmo);
        private static gmoNZ64_t dll_gmoNZ64;
        private static Int64 d_gmoNZ64(IntPtr pgmo)
        { gmoErrorHandling("gmoNZ64 could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNLNZ_t(IntPtr pgmo);
        private static gmoNLNZ_t dll_gmoNLNZ;
        private static int d_gmoNLNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoNLNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Int64 gmoNLNZ64_t(IntPtr pgmo);
        private static gmoNLNZ64_t dll_gmoNLNZ64;
        private static Int64 d_gmoNLNZ64(IntPtr pgmo)
        { gmoErrorHandling("gmoNLNZ64 could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoLNZEx_t(IntPtr pgmo);
        private static gmoLNZEx_t dll_gmoLNZEx;
        private static int d_gmoLNZEx(IntPtr pgmo)
        { gmoErrorHandling("gmoLNZEx could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Int64 gmoLNZEx64_t(IntPtr pgmo);
        private static gmoLNZEx64_t dll_gmoLNZEx64;
        private static Int64 d_gmoLNZEx64(IntPtr pgmo)
        { gmoErrorHandling("gmoLNZEx64 could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoLNZ_t(IntPtr pgmo);
        private static gmoLNZ_t dll_gmoLNZ;
        private static int d_gmoLNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoLNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Int64 gmoLNZ64_t(IntPtr pgmo);
        private static gmoLNZ64_t dll_gmoLNZ64;
        private static Int64 d_gmoLNZ64(IntPtr pgmo)
        { gmoErrorHandling("gmoLNZ64 could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoQNZ_t(IntPtr pgmo);
        private static gmoQNZ_t dll_gmoQNZ;
        private static int d_gmoQNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoQNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGNLNZ_t(IntPtr pgmo);
        private static gmoGNLNZ_t dll_gmoGNLNZ;
        private static int d_gmoGNLNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoGNLNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoMaxQNZ_t(IntPtr pgmo);
        private static gmoMaxQNZ_t dll_gmoMaxQNZ;
        private static int d_gmoMaxQNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoMaxQNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Int64 gmoMaxQNZ64_t(IntPtr pgmo);
        private static gmoMaxQNZ64_t dll_gmoMaxQNZ64;
        private static Int64 d_gmoMaxQNZ64(IntPtr pgmo)
        { gmoErrorHandling("gmoMaxQNZ64 could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjNZ_t(IntPtr pgmo);
        private static gmoObjNZ_t dll_gmoObjNZ;
        private static int d_gmoObjNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoObjNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjLNZ_t(IntPtr pgmo);
        private static gmoObjLNZ_t dll_gmoObjLNZ;
        private static int d_gmoObjLNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoObjLNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjQNZEx_t(IntPtr pgmo);
        private static gmoObjQNZEx_t dll_gmoObjQNZEx;
        private static int d_gmoObjQNZEx(IntPtr pgmo)
        { gmoErrorHandling("gmoObjQNZEx could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjNLNZ_t(IntPtr pgmo);
        private static gmoObjNLNZ_t dll_gmoObjNLNZ;
        private static int d_gmoObjNLNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoObjNLNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjNLNZEx_t(IntPtr pgmo);
        private static gmoObjNLNZEx_t dll_gmoObjNLNZEx;
        private static int d_gmoObjNLNZEx(IntPtr pgmo)
        { gmoErrorHandling("gmoObjNLNZEx could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjQMatNZ_t(IntPtr pgmo);
        private static gmoObjQMatNZ_t dll_gmoObjQMatNZ;
        private static int d_gmoObjQMatNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoObjQMatNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Int64 gmoObjQMatNZ64_t(IntPtr pgmo);
        private static gmoObjQMatNZ64_t dll_gmoObjQMatNZ64;
        private static Int64 d_gmoObjQMatNZ64(IntPtr pgmo)
        { gmoErrorHandling("gmoObjQMatNZ64 could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjQNZ_t(IntPtr pgmo);
        private static gmoObjQNZ_t dll_gmoObjQNZ;
        private static int d_gmoObjQNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoObjQNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjQDiagNZ_t(IntPtr pgmo);
        private static gmoObjQDiagNZ_t dll_gmoObjQDiagNZ;
        private static int d_gmoObjQDiagNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoObjQDiagNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjCVecNZ_t(IntPtr pgmo);
        private static gmoObjCVecNZ_t dll_gmoObjCVecNZ;
        private static int d_gmoObjCVecNZ(IntPtr pgmo)
        { gmoErrorHandling("gmoObjCVecNZ could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNLConst_t(IntPtr pgmo);
        private static gmoNLConst_t dll_gmoNLConst;
        private static int d_gmoNLConst(IntPtr pgmo)
        { gmoErrorHandling("gmoNLConst could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNLConstSet_t(IntPtr pgmo, int x);
        private static gmoNLConstSet_t dll_gmoNLConstSet;
        private static void d_gmoNLConstSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoNLConstSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNLCodeSize_t(IntPtr pgmo);
        private static gmoNLCodeSize_t dll_gmoNLCodeSize;
        private static int d_gmoNLCodeSize(IntPtr pgmo)
        { gmoErrorHandling("gmoNLCodeSize could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNLCodeSizeSet_t(IntPtr pgmo, int x);
        private static gmoNLCodeSizeSet_t dll_gmoNLCodeSizeSet;
        private static void d_gmoNLCodeSizeSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoNLCodeSizeSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNLCodeSizeMaxRow_t(IntPtr pgmo);
        private static gmoNLCodeSizeMaxRow_t dll_gmoNLCodeSizeMaxRow;
        private static int d_gmoNLCodeSizeMaxRow(IntPtr pgmo)
        { gmoErrorHandling("gmoNLCodeSizeMaxRow could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjVar_t(IntPtr pgmo);
        private static gmoObjVar_t dll_gmoObjVar;
        private static int d_gmoObjVar(IntPtr pgmo)
        { gmoErrorHandling("gmoObjVar could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoObjVarSet_t(IntPtr pgmo, int x);
        private static gmoObjVarSet_t dll_gmoObjVarSet;
        private static void d_gmoObjVarSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoObjVarSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoObjRow_t(IntPtr pgmo);
        private static gmoObjRow_t dll_gmoObjRow;
        private static int d_gmoObjRow(IntPtr pgmo)
        { gmoErrorHandling("gmoObjRow could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoGetObjOrder_t(IntPtr pgmo);
        private static gmoGetObjOrder_t dll_gmoGetObjOrder;
        private static int d_gmoGetObjOrder(IntPtr pgmo)
        { gmoErrorHandling("gmoGetObjOrder could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoObjConst_t(IntPtr pgmo);
        private static gmoObjConst_t dll_gmoObjConst;
        private static double d_gmoObjConst(IntPtr pgmo)
        { gmoErrorHandling("gmoObjConst could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoObjConstEx_t(IntPtr pgmo);
        private static gmoObjConstEx_t dll_gmoObjConstEx;
        private static double d_gmoObjConstEx(IntPtr pgmo)
        { gmoErrorHandling("gmoObjConstEx could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoObjQConst_t(IntPtr pgmo);
        private static gmoObjQConst_t dll_gmoObjQConst;
        private static double d_gmoObjQConst(IntPtr pgmo)
        { gmoErrorHandling("gmoObjQConst could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoObjJacVal_t(IntPtr pgmo);
        private static gmoObjJacVal_t dll_gmoObjJacVal;
        private static double d_gmoObjJacVal(IntPtr pgmo)
        { gmoErrorHandling("gmoObjJacVal could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalErrorMethod_t(IntPtr pgmo);
        private static gmoEvalErrorMethod_t dll_gmoEvalErrorMethod;
        private static int d_gmoEvalErrorMethod(IntPtr pgmo)
        { gmoErrorHandling("gmoEvalErrorMethod could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoEvalErrorMethodSet_t(IntPtr pgmo, int x);
        private static gmoEvalErrorMethodSet_t dll_gmoEvalErrorMethodSet;
        private static void d_gmoEvalErrorMethodSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoEvalErrorMethodSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalMaxThreads_t(IntPtr pgmo);
        private static gmoEvalMaxThreads_t dll_gmoEvalMaxThreads;
        private static int d_gmoEvalMaxThreads(IntPtr pgmo)
        { gmoErrorHandling("gmoEvalMaxThreads could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoEvalMaxThreadsSet_t(IntPtr pgmo, int x);
        private static gmoEvalMaxThreadsSet_t dll_gmoEvalMaxThreadsSet;
        private static void d_gmoEvalMaxThreadsSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoEvalMaxThreadsSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalFuncCount_t(IntPtr pgmo);
        private static gmoEvalFuncCount_t dll_gmoEvalFuncCount;
        private static int d_gmoEvalFuncCount(IntPtr pgmo)
        { gmoErrorHandling("gmoEvalFuncCount could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoEvalFuncTimeUsed_t(IntPtr pgmo);
        private static gmoEvalFuncTimeUsed_t dll_gmoEvalFuncTimeUsed;
        private static double d_gmoEvalFuncTimeUsed(IntPtr pgmo)
        { gmoErrorHandling("gmoEvalFuncTimeUsed could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoEvalGradCount_t(IntPtr pgmo);
        private static gmoEvalGradCount_t dll_gmoEvalGradCount;
        private static int d_gmoEvalGradCount(IntPtr pgmo)
        { gmoErrorHandling("gmoEvalGradCount could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gmoEvalGradTimeUsed_t(IntPtr pgmo);
        private static gmoEvalGradTimeUsed_t dll_gmoEvalGradTimeUsed;
        private static double d_gmoEvalGradTimeUsed(IntPtr pgmo)
        { gmoErrorHandling("gmoEvalGradTimeUsed could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessMaxDim_t(IntPtr pgmo);
        private static gmoHessMaxDim_t dll_gmoHessMaxDim;
        private static int d_gmoHessMaxDim(IntPtr pgmo)
        { gmoErrorHandling("gmoHessMaxDim could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessMaxNz_t(IntPtr pgmo);
        private static gmoHessMaxNz_t dll_gmoHessMaxNz;
        private static int d_gmoHessMaxNz(IntPtr pgmo)
        { gmoErrorHandling("gmoHessMaxNz could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessLagDim_t(IntPtr pgmo);
        private static gmoHessLagDim_t dll_gmoHessLagDim;
        private static int d_gmoHessLagDim(IntPtr pgmo)
        { gmoErrorHandling("gmoHessLagDim could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessLagNz_t(IntPtr pgmo);
        private static gmoHessLagNz_t dll_gmoHessLagNz;
        private static int d_gmoHessLagNz(IntPtr pgmo)
        { gmoErrorHandling("gmoHessLagNz could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessLagDiagNz_t(IntPtr pgmo);
        private static gmoHessLagDiagNz_t dll_gmoHessLagDiagNz;
        private static int d_gmoHessLagDiagNz(IntPtr pgmo)
        { gmoErrorHandling("gmoHessLagDiagNz could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoHessInclQRows_t(IntPtr pgmo);
        private static gmoHessInclQRows_t dll_gmoHessInclQRows;
        private static int d_gmoHessInclQRows(IntPtr pgmo)
        { gmoErrorHandling("gmoHessInclQRows could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoHessInclQRowsSet_t(IntPtr pgmo, int x);
        private static gmoHessInclQRowsSet_t dll_gmoHessInclQRowsSet;
        private static void d_gmoHessInclQRowsSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoHessInclQRowsSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNumVIFunc_t(IntPtr pgmo);
        private static gmoNumVIFunc_t dll_gmoNumVIFunc;
        private static int d_gmoNumVIFunc(IntPtr pgmo)
        { gmoErrorHandling("gmoNumVIFunc could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoNumAgents_t(IntPtr pgmo);
        private static gmoNumAgents_t dll_gmoNumAgents;
        private static int d_gmoNumAgents(IntPtr pgmo)
        { gmoErrorHandling("gmoNumAgents could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameOptFile_t(IntPtr pgmo, StringBuilder sst_result);
        private static gmoNameOptFile_t dll_gmoNameOptFile;
        private static void d_gmoNameOptFile(IntPtr pgmo, StringBuilder sst_result)
        { gmoErrorHandling("gmoNameOptFile could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameOptFileSet_t(IntPtr pgmo, string x);
        private static gmoNameOptFileSet_t dll_gmoNameOptFileSet;
        private static void d_gmoNameOptFileSet(IntPtr pgmo, string x)
        { gmoErrorHandling("gmoNameOptFileSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameSolFile_t(IntPtr pgmo, StringBuilder sst_result);
        private static gmoNameSolFile_t dll_gmoNameSolFile;
        private static void d_gmoNameSolFile(IntPtr pgmo, StringBuilder sst_result)
        { gmoErrorHandling("gmoNameSolFile could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameSolFileSet_t(IntPtr pgmo, string x);
        private static gmoNameSolFileSet_t dll_gmoNameSolFileSet;
        private static void d_gmoNameSolFileSet(IntPtr pgmo, string x)
        { gmoErrorHandling("gmoNameSolFileSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameXLib_t(IntPtr pgmo, StringBuilder sst_result);
        private static gmoNameXLib_t dll_gmoNameXLib;
        private static void d_gmoNameXLib(IntPtr pgmo, StringBuilder sst_result)
        { gmoErrorHandling("gmoNameXLib could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameXLibSet_t(IntPtr pgmo, string x);
        private static gmoNameXLibSet_t dll_gmoNameXLibSet;
        private static void d_gmoNameXLibSet(IntPtr pgmo, string x)
        { gmoErrorHandling("gmoNameXLibSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameMatrix_t(IntPtr pgmo, StringBuilder sst_result);
        private static gmoNameMatrix_t dll_gmoNameMatrix;
        private static void d_gmoNameMatrix(IntPtr pgmo, StringBuilder sst_result)
        { gmoErrorHandling("gmoNameMatrix could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameDict_t(IntPtr pgmo, StringBuilder sst_result);
        private static gmoNameDict_t dll_gmoNameDict;
        private static void d_gmoNameDict(IntPtr pgmo, StringBuilder sst_result)
        { gmoErrorHandling("gmoNameDict could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameDictSet_t(IntPtr pgmo, string x);
        private static gmoNameDictSet_t dll_gmoNameDictSet;
        private static void d_gmoNameDictSet(IntPtr pgmo, string x)
        { gmoErrorHandling("gmoNameDictSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameInput_t(IntPtr pgmo, StringBuilder sst_result);
        private static gmoNameInput_t dll_gmoNameInput;
        private static void d_gmoNameInput(IntPtr pgmo, StringBuilder sst_result)
        { gmoErrorHandling("gmoNameInput could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameInputSet_t(IntPtr pgmo, string x);
        private static gmoNameInputSet_t dll_gmoNameInputSet;
        private static void d_gmoNameInputSet(IntPtr pgmo, string x)
        { gmoErrorHandling("gmoNameInputSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoNameOutput_t(IntPtr pgmo, StringBuilder sst_result);
        private static gmoNameOutput_t dll_gmoNameOutput;
        private static void d_gmoNameOutput(IntPtr pgmo, StringBuilder sst_result)
        { gmoErrorHandling("gmoNameOutput could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr gmoPPool_t(IntPtr pgmo);
        private static gmoPPool_t dll_gmoPPool;
        private static IntPtr d_gmoPPool(IntPtr pgmo)
        { gmoErrorHandling("gmoPPool could not be loaded"); return IntPtr.Zero; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr gmoIOMutex_t(IntPtr pgmo);
        private static gmoIOMutex_t dll_gmoIOMutex;
        private static IntPtr d_gmoIOMutex(IntPtr pgmo)
        { gmoErrorHandling("gmoIOMutex could not be loaded"); return IntPtr.Zero; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoError_t(IntPtr pgmo);
        private static gmoError_t dll_gmoError;
        private static int d_gmoError(IntPtr pgmo)
        { gmoErrorHandling("gmoError could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoErrorSet_t(IntPtr pgmo, int x);
        private static gmoErrorSet_t dll_gmoErrorSet;
        private static void d_gmoErrorSet(IntPtr pgmo, int x)
        { gmoErrorHandling("gmoErrorSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoErrorMessage_t(IntPtr pgmo, StringBuilder sst_result);
        private static gmoErrorMessage_t dll_gmoErrorMessage;
        private static void d_gmoErrorMessage(IntPtr pgmo, StringBuilder sst_result)
        { gmoErrorHandling("gmoErrorMessage could not be loaded"); }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoxcreate_t(ref IntPtr pgmo);
        private static gmoxcreate_t gmoxcreate;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoxcreated_t(ref IntPtr pgmo, string dirName);
        private static gmoxcreated_t gmoxcreated;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gmoxfree_t(ref IntPtr pgmo);
        private static gmoxfree_t gmoxfree;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoxapiversion_t(int api, StringBuilder msg, ref int cl);
        private static gmoxapiversion_t dll_gmoxapiversion;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gmoxcheck_t(string ep, int nargs, int[] s, StringBuilder msg);
        private static gmoxcheck_t dll_gmoxcheck;

        public delegate bool gmoErrorCallback_t(int ErrCount, string Msg);

        static bool isLoaded = false;
        static IntPtr h;
        static bool ScreenIndicator = true;
        static bool ExceptionIndicator = false;
        static bool ExitIndicator = true;
        static gmoErrorCallback_t ErrorCallBack = null;
        static int APIErrorCount = 0;

        private bool XLibraryLoad(string dllName, ref string errBuf)
        {
            string symName;
            int cl = 0;
            IntPtr pAddressOfFunctionToCall;

            if (isLoaded)
                return true;

#if __MonoCS__ || __APPLE__
        h = LoadLibrary(@dllName,2);
#else
            h = LoadLibrary(@dllName);
#endif

            if (IntPtr.Zero == h)
            {
                errBuf = "Could not load shared library " + dllName;
                return false;
            }

            pAddressOfFunctionToCall = GetProcAddress(h, "gmoxcreate");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                gmoxcreate = (gmoxcreate_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoxcreate_t));
            else
            {
                symName = "gmoxcreate"; goto symMissing;
            }
            pAddressOfFunctionToCall = GetProcAddress(h, "cgmoxcreated");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                gmoxcreated = (gmoxcreated_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoxcreated_t));
            else
            {
                symName = "cgmoxcreated"; goto symMissing;
            }
            pAddressOfFunctionToCall = GetProcAddress(h, "gmoxfree");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                gmoxfree = (gmoxfree_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoxfree_t));
            else
            {
                symName = "gmoxfree"; goto symMissing;
            }
            pAddressOfFunctionToCall = GetProcAddress(h, "cgmoxcheck");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                dll_gmoxcheck = (gmoxcheck_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoxcheck_t));
            else
            {
                symName = "cgmoxcheck"; goto symMissing;
            }
            pAddressOfFunctionToCall = GetProcAddress(h, "cgmoxapiversion");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                dll_gmoxapiversion = (gmoxapiversion_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoxapiversion_t));
            else
            {
                symName = "cgmoxapiversion"; goto symMissing;
            }
            if (gmoxapiversion(23, ref errBuf, ref cl) == 0)
                return false;

            {
                int[] s = { 3, 3, 3, 3 };
                if (gmoxcheck("gmoInitData", 3, s, ref errBuf) == 0)
                    dll_gmoInitData = d_gmoInitData;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoinitdata");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoInitData = (gmoInitData_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoInitData_t));
                    else
                    {
                        symName = "gmoInitData"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 3, 13, 13, 13, 13, 3, 3, 7, 5, 7 };
                if (gmoxcheck("gmoAddRow", 11, s, ref errBuf) == 0)
                    dll_gmoAddRow = d_gmoAddRow;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoaddrow");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoAddRow = (gmoAddRow_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoAddRow_t));
                    else
                    {
                        symName = "gmoAddRow"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 13, 13, 13, 13, 3, 3, 13, 13, 3, 7, 5, 7 };
                if (gmoxcheck("gmoAddCol", 13, s, ref errBuf) == 0)
                    dll_gmoAddCol = d_gmoAddCol;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoaddcol");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoAddCol = (gmoAddCol_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoAddCol_t));
                    else
                    {
                        symName = "gmoAddCol"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 12 };
                if (gmoxcheck("gmoCompleteData", 1, s, ref errBuf) == 0)
                    dll_gmoCompleteData = d_gmoCompleteData;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmocompletedata");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoCompleteData = (gmoCompleteData_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoCompleteData_t));
                    else
                    {
                        symName = "cgmoCompleteData"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 12 };
                if (gmoxcheck("gmoLoadDataLegacy", 1, s, ref errBuf) == 0)
                    dll_gmoLoadDataLegacy = d_gmoLoadDataLegacy;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmoloaddatalegacy");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoLoadDataLegacy = (gmoLoadDataLegacy_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoLoadDataLegacy_t));
                    else
                    {
                        symName = "cgmoLoadDataLegacy"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 12 };
                if (gmoxcheck("gmoRegisterEnvironment", 2, s, ref errBuf) == 0)
                    dll_gmoRegisterEnvironment = d_gmoRegisterEnvironment;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmoregisterenvironment");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoRegisterEnvironment = (gmoRegisterEnvironment_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoRegisterEnvironment_t));
                    else
                    {
                        symName = "cgmoRegisterEnvironment"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 1 };
                if (gmoxcheck("gmoEnvironment", 0, s, ref errBuf) == 0)
                    dll_gmoEnvironment = d_gmoEnvironment;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoenvironment");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEnvironment = (gmoEnvironment_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEnvironment_t));
                    else
                    {
                        symName = "gmoEnvironment"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 1 };
                if (gmoxcheck("gmoViewStore", 0, s, ref errBuf) == 0)
                    dll_gmoViewStore = d_gmoViewStore;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoviewstore");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoViewStore = (gmoViewStore_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoViewStore_t));
                    else
                    {
                        symName = "gmoViewStore"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 2 };
                if (gmoxcheck("gmoViewRestore", 1, s, ref errBuf) == 0)
                    dll_gmoViewRestore = d_gmoViewRestore;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoviewrestore");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoViewRestore = (gmoViewRestore_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoViewRestore_t));
                    else
                    {
                        symName = "gmoViewRestore"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gmoxcheck("gmoViewDump", 0, s, ref errBuf) == 0)
                    dll_gmoViewDump = d_gmoViewDump;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoviewdump");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoViewDump = (gmoViewDump_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoViewDump_t));
                    else
                    {
                        symName = "gmoViewDump"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetiSolver", 1, s, ref errBuf) == 0)
                    dll_gmoGetiSolver = d_gmoGetiSolver;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetisolver");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetiSolver = (gmoGetiSolver_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetiSolver_t));
                    else
                    {
                        symName = "gmoGetiSolver"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetjSolver", 1, s, ref errBuf) == 0)
                    dll_gmoGetjSolver = d_gmoGetjSolver;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetjsolver");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetjSolver = (gmoGetjSolver_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetjSolver_t));
                    else
                    {
                        symName = "gmoGetjSolver"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetiSolverQuiet", 1, s, ref errBuf) == 0)
                    dll_gmoGetiSolverQuiet = d_gmoGetiSolverQuiet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetisolverquiet");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetiSolverQuiet = (gmoGetiSolverQuiet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetiSolverQuiet_t));
                    else
                    {
                        symName = "gmoGetiSolverQuiet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetjSolverQuiet", 1, s, ref errBuf) == 0)
                    dll_gmoGetjSolverQuiet = d_gmoGetjSolverQuiet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetjsolverquiet");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetjSolverQuiet = (gmoGetjSolverQuiet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetjSolverQuiet_t));
                    else
                    {
                        symName = "gmoGetjSolverQuiet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetiModel", 1, s, ref errBuf) == 0)
                    dll_gmoGetiModel = d_gmoGetiModel;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetimodel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetiModel = (gmoGetiModel_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetiModel_t));
                    else
                    {
                        symName = "gmoGetiModel"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetjModel", 1, s, ref errBuf) == 0)
                    dll_gmoGetjModel = d_gmoGetjModel;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetjmodel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetjModel = (gmoGetjModel_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetjModel_t));
                    else
                    {
                        symName = "gmoGetjModel"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8 };
                if (gmoxcheck("gmoSetEquPermutation", 1, s, ref errBuf) == 0)
                    dll_gmoSetEquPermutation = d_gmoSetEquPermutation;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetequpermutation");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetEquPermutation = (gmoSetEquPermutation_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetEquPermutation_t));
                    else
                    {
                        symName = "gmoSetEquPermutation"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 3 };
                if (gmoxcheck("gmoSetRvEquPermutation", 2, s, ref errBuf) == 0)
                    dll_gmoSetRvEquPermutation = d_gmoSetRvEquPermutation;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetrvequpermutation");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetRvEquPermutation = (gmoSetRvEquPermutation_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetRvEquPermutation_t));
                    else
                    {
                        symName = "gmoSetRvEquPermutation"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8 };
                if (gmoxcheck("gmoSetVarPermutation", 1, s, ref errBuf) == 0)
                    dll_gmoSetVarPermutation = d_gmoSetVarPermutation;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetvarpermutation");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetVarPermutation = (gmoSetVarPermutation_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetVarPermutation_t));
                    else
                    {
                        symName = "gmoSetVarPermutation"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 3 };
                if (gmoxcheck("gmoSetRvVarPermutation", 2, s, ref errBuf) == 0)
                    dll_gmoSetRvVarPermutation = d_gmoSetRvVarPermutation;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetrvvarpermutation");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetRvVarPermutation = (gmoSetRvVarPermutation_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetRvVarPermutation_t));
                    else
                    {
                        symName = "gmoSetRvVarPermutation"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoSetNRowPerm", 0, s, ref errBuf) == 0)
                    dll_gmoSetNRowPerm = d_gmoSetNRowPerm;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetnrowperm");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetNRowPerm = (gmoSetNRowPerm_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetNRowPerm_t));
                    else
                    {
                        symName = "gmoSetNRowPerm"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetVarTypeCnt", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarTypeCnt = d_gmoGetVarTypeCnt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvartypecnt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarTypeCnt = (gmoGetVarTypeCnt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarTypeCnt_t));
                    else
                    {
                        symName = "gmoGetVarTypeCnt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetEquTypeCnt", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquTypeCnt = d_gmoGetEquTypeCnt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequtypecnt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquTypeCnt = (gmoGetEquTypeCnt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquTypeCnt_t));
                    else
                    {
                        symName = "gmoGetEquTypeCnt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 4, 4, 4 };
                if (gmoxcheck("gmoGetObjStat", 3, s, ref errBuf) == 0)
                    dll_gmoGetObjStat = d_gmoGetObjStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetobjstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetObjStat = (gmoGetObjStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetObjStat_t));
                    else
                    {
                        symName = "gmoGetObjStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4, 4, 4 };
                if (gmoxcheck("gmoGetRowStat", 4, s, ref errBuf) == 0)
                    dll_gmoGetRowStat = d_gmoGetRowStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowStat = (gmoGetRowStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowStat_t));
                    else
                    {
                        symName = "gmoGetRowStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4, 4, 4, 4 };
                if (gmoxcheck("gmoGetRowStatEx", 5, s, ref errBuf) == 0)
                    dll_gmoGetRowStatEx = d_gmoGetRowStatEx;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowstatex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowStatEx = (gmoGetRowStatEx_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowStatEx_t));
                    else
                    {
                        symName = "gmoGetRowStatEx"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4, 4, 4, 4 };
                if (gmoxcheck("gmoGetColStat", 5, s, ref errBuf) == 0)
                    dll_gmoGetColStat = d_gmoGetColStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetcolstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetColStat = (gmoGetColStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetColStat_t));
                    else
                    {
                        symName = "gmoGetColStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetRowQNZOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetRowQNZOne = d_gmoGetRowQNZOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowqnzone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowQNZOne = (gmoGetRowQNZOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowQNZOne_t));
                    else
                    {
                        symName = "gmoGetRowQNZOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 23, 3 };
                if (gmoxcheck("gmoGetRowQNZOne64", 1, s, ref errBuf) == 0)
                    dll_gmoGetRowQNZOne64 = d_gmoGetRowQNZOne64;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowqnzone64");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowQNZOne64 = (gmoGetRowQNZOne64_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowQNZOne64_t));
                    else
                    {
                        symName = "gmoGetRowQNZOne64"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetRowQDiagNZOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetRowQDiagNZOne = d_gmoGetRowQDiagNZOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowqdiagnzone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowQDiagNZOne = (gmoGetRowQDiagNZOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowQDiagNZOne_t));
                    else
                    {
                        symName = "gmoGetRowQDiagNZOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetRowCVecNZOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetRowCVecNZOne = d_gmoGetRowCVecNZOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowcvecnzone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowCVecNZOne = (gmoGetRowCVecNZOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowCVecNZOne_t));
                    else
                    {
                        symName = "gmoGetRowCVecNZOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 4, 4, 4 };
                if (gmoxcheck("gmoGetSosCounts", 3, s, ref errBuf) == 0)
                    dll_gmoGetSosCounts = d_gmoGetSosCounts;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetsoscounts");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetSosCounts = (gmoGetSosCounts_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetSosCounts_t));
                    else
                    {
                        symName = "gmoGetSosCounts"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 4, 4, 4, 8 };
                if (gmoxcheck("gmoGetXLibCounts", 4, s, ref errBuf) == 0)
                    dll_gmoGetXLibCounts = d_gmoGetXLibCounts;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetxlibcounts");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetXLibCounts = (gmoGetXLibCounts_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetXLibCounts_t));
                    else
                    {
                        symName = "gmoGetXLibCounts"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 4 };
                if (gmoxcheck("gmoGetActiveModelType", 2, s, ref errBuf) == 0)
                    dll_gmoGetActiveModelType = d_gmoGetActiveModelType;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetactivemodeltype");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetActiveModelType = (gmoGetActiveModelType_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetActiveModelType_t));
                    else
                    {
                        symName = "gmoGetActiveModelType"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 8, 6, 8 };
                if (gmoxcheck("gmoGetMatrixRow", 4, s, ref errBuf) == 0)
                    dll_gmoGetMatrixRow = d_gmoGetMatrixRow;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetmatrixrow");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetMatrixRow = (gmoGetMatrixRow_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetMatrixRow_t));
                    else
                    {
                        symName = "gmoGetMatrixRow"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 8, 6, 8 };
                if (gmoxcheck("gmoGetMatrixCol", 4, s, ref errBuf) == 0)
                    dll_gmoGetMatrixCol = d_gmoGetMatrixCol;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetmatrixcol");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetMatrixCol = (gmoGetMatrixCol_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetMatrixCol_t));
                    else
                    {
                        symName = "gmoGetMatrixCol"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 8, 8, 6 };
                if (gmoxcheck("gmoGetMatrixCplex", 4, s, ref errBuf) == 0)
                    dll_gmoGetMatrixCplex = d_gmoGetMatrixCplex;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetmatrixcplex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetMatrixCplex = (gmoGetMatrixCplex_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetMatrixCplex_t));
                    else
                    {
                        symName = "gmoGetMatrixCplex"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12 };
                if (gmoxcheck("gmoGetObjName", 0, s, ref errBuf) == 0)
                    dll_gmoGetObjName = d_gmoGetObjName;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetobjname");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetObjName = (gmoGetObjName_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetObjName_t));
                    else
                    {
                        symName = "cgmoGetObjName"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 11 };
                if (gmoxcheck("gmoGetObjNameCustom", 1, s, ref errBuf) == 0)
                    dll_gmoGetObjNameCustom = d_gmoGetObjNameCustom;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetobjnamecustom");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetObjNameCustom = (gmoGetObjNameCustom_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetObjNameCustom_t));
                    else
                    {
                        symName = "cgmoGetObjNameCustom"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6, 8 };
                if (gmoxcheck("gmoGetObjVector", 2, s, ref errBuf) == 0)
                    dll_gmoGetObjVector = d_gmoGetObjVector;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetobjvector");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetObjVector = (gmoGetObjVector_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetObjVector_t));
                    else
                    {
                        symName = "gmoGetObjVector"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 6, 8, 4, 4 };
                if (gmoxcheck("gmoGetObjSparse", 5, s, ref errBuf) == 0)
                    dll_gmoGetObjSparse = d_gmoGetObjSparse;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetobjsparse");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetObjSparse = (gmoGetObjSparse_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetObjSparse_t));
                    else
                    {
                        symName = "gmoGetObjSparse"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 6, 8, 4, 4, 4 };
                if (gmoxcheck("gmoGetObjSparseEx", 6, s, ref errBuf) == 0)
                    dll_gmoGetObjSparseEx = d_gmoGetObjSparseEx;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetobjsparseex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetObjSparseEx = (gmoGetObjSparseEx_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetObjSparseEx_t));
                    else
                    {
                        symName = "gmoGetObjSparseEx"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 8, 6 };
                if (gmoxcheck("gmoGetObjQMat", 3, s, ref errBuf) == 0)
                    dll_gmoGetObjQMat = d_gmoGetObjQMat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetobjqmat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetObjQMat = (gmoGetObjQMat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetObjQMat_t));
                    else
                    {
                        symName = "gmoGetObjQMat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 8, 6 };
                if (gmoxcheck("gmoGetObjQ", 3, s, ref errBuf) == 0)
                    dll_gmoGetObjQ = d_gmoGetObjQ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetobjq");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetObjQ = (gmoGetObjQ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetObjQ_t));
                    else
                    {
                        symName = "gmoGetObjQ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 6 };
                if (gmoxcheck("gmoGetObjCVec", 2, s, ref errBuf) == 0)
                    dll_gmoGetObjCVec = d_gmoGetObjCVec;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetobjcvec");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetObjCVec = (gmoGetObjCVec_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetObjCVec_t));
                    else
                    {
                        symName = "gmoGetObjCVec"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetEquL", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquL = d_gmoGetEquL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequl");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquL = (gmoGetEquL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquL_t));
                    else
                    {
                        symName = "gmoGetEquL"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetEquLOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquLOne = d_gmoGetEquLOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequlone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquLOne = (gmoGetEquLOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquLOne_t));
                    else
                    {
                        symName = "gmoGetEquLOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5 };
                if (gmoxcheck("gmoSetEquL", 1, s, ref errBuf) == 0)
                    dll_gmoSetEquL = d_gmoSetEquL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetequl");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetEquL = (gmoSetEquL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetEquL_t));
                    else
                    {
                        symName = "gmoSetEquL"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 13 };
                if (gmoxcheck("gmoSetEquLOne", 2, s, ref errBuf) == 0)
                    dll_gmoSetEquLOne = d_gmoSetEquLOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetequlone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetEquLOne = (gmoSetEquLOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetEquLOne_t));
                    else
                    {
                        symName = "gmoSetEquLOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetEquM", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquM = d_gmoGetEquM;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequm");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquM = (gmoGetEquM_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquM_t));
                    else
                    {
                        symName = "gmoGetEquM"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetEquMOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquMOne = d_gmoGetEquMOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequmone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquMOne = (gmoGetEquMOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquMOne_t));
                    else
                    {
                        symName = "gmoGetEquMOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5 };
                if (gmoxcheck("gmoSetEquM", 1, s, ref errBuf) == 0)
                    dll_gmoSetEquM = d_gmoSetEquM;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetequm");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetEquM = (gmoSetEquM_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetEquM_t));
                    else
                    {
                        symName = "gmoSetEquM"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 3 };
                if (gmoxcheck("gmoGetEquNameOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquNameOne = d_gmoGetEquNameOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetequnameone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquNameOne = (gmoGetEquNameOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquNameOne_t));
                    else
                    {
                        symName = "cgmoGetEquNameOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 3, 11 };
                if (gmoxcheck("gmoGetEquNameCustomOne", 2, s, ref errBuf) == 0)
                    dll_gmoGetEquNameCustomOne = d_gmoGetEquNameCustomOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetequnamecustomone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquNameCustomOne = (gmoGetEquNameCustomOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquNameCustomOne_t));
                    else
                    {
                        symName = "cgmoGetEquNameCustomOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetRhs", 1, s, ref errBuf) == 0)
                    dll_gmoGetRhs = d_gmoGetRhs;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrhs");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRhs = (gmoGetRhs_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRhs_t));
                    else
                    {
                        symName = "gmoGetRhs"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetRhsOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetRhsOne = d_gmoGetRhsOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrhsone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRhsOne = (gmoGetRhsOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRhsOne_t));
                    else
                    {
                        symName = "gmoGetRhsOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetRhsOneEx", 1, s, ref errBuf) == 0)
                    dll_gmoGetRhsOneEx = d_gmoGetRhsOneEx;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrhsoneex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRhsOneEx = (gmoGetRhsOneEx_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRhsOneEx_t));
                    else
                    {
                        symName = "gmoGetRhsOneEx"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5 };
                if (gmoxcheck("gmoSetAltRHS", 1, s, ref errBuf) == 0)
                    dll_gmoSetAltRHS = d_gmoSetAltRHS;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetaltrhs");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetAltRHS = (gmoSetAltRHS_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetAltRHS_t));
                    else
                    {
                        symName = "gmoSetAltRHS"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 13 };
                if (gmoxcheck("gmoSetAltRHSOne", 2, s, ref errBuf) == 0)
                    dll_gmoSetAltRHSOne = d_gmoSetAltRHSOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetaltrhsone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetAltRHSOne = (gmoSetAltRHSOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetAltRHSOne_t));
                    else
                    {
                        symName = "gmoSetAltRHSOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetEquSlack", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquSlack = d_gmoGetEquSlack;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequslack");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquSlack = (gmoGetEquSlack_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquSlack_t));
                    else
                    {
                        symName = "gmoGetEquSlack"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetEquSlackOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquSlackOne = d_gmoGetEquSlackOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequslackone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquSlackOne = (gmoGetEquSlackOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquSlackOne_t));
                    else
                    {
                        symName = "gmoGetEquSlackOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5 };
                if (gmoxcheck("gmoSetEquSlack", 1, s, ref errBuf) == 0)
                    dll_gmoSetEquSlack = d_gmoSetEquSlack;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetequslack");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetEquSlack = (gmoSetEquSlack_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetEquSlack_t));
                    else
                    {
                        symName = "gmoSetEquSlack"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8 };
                if (gmoxcheck("gmoGetEquType", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquType = d_gmoGetEquType;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequtype");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquType = (gmoGetEquType_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquType_t));
                    else
                    {
                        symName = "gmoGetEquType"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetEquTypeOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquTypeOne = d_gmoGetEquTypeOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequtypeone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquTypeOne = (gmoGetEquTypeOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquTypeOne_t));
                    else
                    {
                        symName = "gmoGetEquTypeOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 8 };
                if (gmoxcheck("gmoGetEquStat", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquStat = d_gmoGetEquStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquStat = (gmoGetEquStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquStat_t));
                    else
                    {
                        symName = "gmoGetEquStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetEquStatOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquStatOne = d_gmoGetEquStatOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequstatone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquStatOne = (gmoGetEquStatOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquStatOne_t));
                    else
                    {
                        symName = "gmoGetEquStatOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 7 };
                if (gmoxcheck("gmoSetEquStat", 1, s, ref errBuf) == 0)
                    dll_gmoSetEquStat = d_gmoSetEquStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetequstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetEquStat = (gmoSetEquStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetEquStat_t));
                    else
                    {
                        symName = "gmoSetEquStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 8 };
                if (gmoxcheck("gmoGetEquCStat", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquCStat = d_gmoGetEquCStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequcstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquCStat = (gmoGetEquCStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquCStat_t));
                    else
                    {
                        symName = "gmoGetEquCStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetEquCStatOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquCStatOne = d_gmoGetEquCStatOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequcstatone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquCStatOne = (gmoGetEquCStatOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquCStatOne_t));
                    else
                    {
                        symName = "gmoGetEquCStatOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 7 };
                if (gmoxcheck("gmoSetEquCStat", 1, s, ref errBuf) == 0)
                    dll_gmoSetEquCStat = d_gmoSetEquCStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetequcstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetEquCStat = (gmoSetEquCStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetEquCStat_t));
                    else
                    {
                        symName = "gmoSetEquCStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8 };
                if (gmoxcheck("gmoGetEquMatch", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquMatch = d_gmoGetEquMatch;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequmatch");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquMatch = (gmoGetEquMatch_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquMatch_t));
                    else
                    {
                        symName = "gmoGetEquMatch"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetEquMatchOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquMatchOne = d_gmoGetEquMatchOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequmatchone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquMatchOne = (gmoGetEquMatchOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquMatchOne_t));
                    else
                    {
                        symName = "gmoGetEquMatchOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetEquScale", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquScale = d_gmoGetEquScale;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequscale");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquScale = (gmoGetEquScale_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquScale_t));
                    else
                    {
                        symName = "gmoGetEquScale"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetEquScaleOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquScaleOne = d_gmoGetEquScaleOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequscaleone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquScaleOne = (gmoGetEquScaleOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquScaleOne_t));
                    else
                    {
                        symName = "gmoGetEquScaleOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetEquStage", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquStage = d_gmoGetEquStage;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequstage");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquStage = (gmoGetEquStage_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquStage_t));
                    else
                    {
                        symName = "gmoGetEquStage"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetEquStageOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquStageOne = d_gmoGetEquStageOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequstageone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquStageOne = (gmoGetEquStageOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquStageOne_t));
                    else
                    {
                        symName = "gmoGetEquStageOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetEquOrderOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquOrderOne = d_gmoGetEquOrderOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequorderone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquOrderOne = (gmoGetEquOrderOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquOrderOne_t));
                    else
                    {
                        symName = "gmoGetEquOrderOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 8, 6, 8, 4, 4 };
                if (gmoxcheck("gmoGetRowSparse", 6, s, ref errBuf) == 0)
                    dll_gmoGetRowSparse = d_gmoGetRowSparse;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowsparse");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowSparse = (gmoGetRowSparse_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowSparse_t));
                    else
                    {
                        symName = "gmoGetRowSparse"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 8, 6, 8, 4, 4, 4 };
                if (gmoxcheck("gmoGetRowSparseEx", 7, s, ref errBuf) == 0)
                    dll_gmoGetRowSparseEx = d_gmoGetRowSparseEx;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowsparseex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowSparseEx = (gmoGetRowSparseEx_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowSparseEx_t));
                    else
                    {
                        symName = "gmoGetRowSparseEx"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 2, 14, 4, 4 };
                if (gmoxcheck("gmoGetRowJacInfoOne", 5, s, ref errBuf) == 0)
                    dll_gmoGetRowJacInfoOne = d_gmoGetRowJacInfoOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowjacinfoone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowJacInfoOne = (gmoGetRowJacInfoOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowJacInfoOne_t));
                    else
                    {
                        symName = "gmoGetRowJacInfoOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 8, 8, 6 };
                if (gmoxcheck("gmoGetRowQMat", 4, s, ref errBuf) == 0)
                    dll_gmoGetRowQMat = d_gmoGetRowQMat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowqmat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowQMat = (gmoGetRowQMat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowQMat_t));
                    else
                    {
                        symName = "gmoGetRowQMat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 8, 8, 6 };
                if (gmoxcheck("gmoGetRowQ", 4, s, ref errBuf) == 0)
                    dll_gmoGetRowQ = d_gmoGetRowQ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowq");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowQ = (gmoGetRowQ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowQ_t));
                    else
                    {
                        symName = "gmoGetRowQ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 8, 6 };
                if (gmoxcheck("gmoGetRowCVec", 3, s, ref errBuf) == 0)
                    dll_gmoGetRowCVec = d_gmoGetRowCVec;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowcvec");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowCVec = (gmoGetRowCVec_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowCVec_t));
                    else
                    {
                        symName = "gmoGetRowCVec"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetRowQConst", 1, s, ref errBuf) == 0)
                    dll_gmoGetRowQConst = d_gmoGetRowQConst;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrowqconst");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRowQConst = (gmoGetRowQConst_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRowQConst_t));
                    else
                    {
                        symName = "gmoGetRowQConst"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 11, 8 };
                if (gmoxcheck("gmoGetEquIntDotOpt", 3, s, ref errBuf) == 0)
                    dll_gmoGetEquIntDotOpt = d_gmoGetEquIntDotOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetequintdotopt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquIntDotOpt = (gmoGetEquIntDotOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquIntDotOpt_t));
                    else
                    {
                        symName = "cgmoGetEquIntDotOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 11, 6 };
                if (gmoxcheck("gmoGetEquDblDotOpt", 3, s, ref errBuf) == 0)
                    dll_gmoGetEquDblDotOpt = d_gmoGetEquDblDotOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetequdbldotopt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquDblDotOpt = (gmoGetEquDblDotOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquDblDotOpt_t));
                    else
                    {
                        symName = "cgmoGetEquDblDotOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetVarL", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarL = d_gmoGetVarL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarl");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarL = (gmoGetVarL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarL_t));
                    else
                    {
                        symName = "gmoGetVarL"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetVarLOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarLOne = d_gmoGetVarLOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarlone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarLOne = (gmoGetVarLOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarLOne_t));
                    else
                    {
                        symName = "gmoGetVarLOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5 };
                if (gmoxcheck("gmoSetVarL", 1, s, ref errBuf) == 0)
                    dll_gmoSetVarL = d_gmoSetVarL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetvarl");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetVarL = (gmoSetVarL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetVarL_t));
                    else
                    {
                        symName = "gmoSetVarL"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 13 };
                if (gmoxcheck("gmoSetVarLOne", 2, s, ref errBuf) == 0)
                    dll_gmoSetVarLOne = d_gmoSetVarLOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetvarlone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetVarLOne = (gmoSetVarLOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetVarLOne_t));
                    else
                    {
                        symName = "gmoSetVarLOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetVarM", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarM = d_gmoGetVarM;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarm");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarM = (gmoGetVarM_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarM_t));
                    else
                    {
                        symName = "gmoGetVarM"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetVarMOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarMOne = d_gmoGetVarMOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarmone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarMOne = (gmoGetVarMOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarMOne_t));
                    else
                    {
                        symName = "gmoGetVarMOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5 };
                if (gmoxcheck("gmoSetVarM", 1, s, ref errBuf) == 0)
                    dll_gmoSetVarM = d_gmoSetVarM;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetvarm");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetVarM = (gmoSetVarM_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetVarM_t));
                    else
                    {
                        symName = "gmoSetVarM"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 13 };
                if (gmoxcheck("gmoSetVarMOne", 2, s, ref errBuf) == 0)
                    dll_gmoSetVarMOne = d_gmoSetVarMOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetvarmone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetVarMOne = (gmoSetVarMOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetVarMOne_t));
                    else
                    {
                        symName = "gmoSetVarMOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 3 };
                if (gmoxcheck("gmoGetVarNameOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarNameOne = d_gmoGetVarNameOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetvarnameone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarNameOne = (gmoGetVarNameOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarNameOne_t));
                    else
                    {
                        symName = "cgmoGetVarNameOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 3, 11 };
                if (gmoxcheck("gmoGetVarNameCustomOne", 2, s, ref errBuf) == 0)
                    dll_gmoGetVarNameCustomOne = d_gmoGetVarNameCustomOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetvarnamecustomone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarNameCustomOne = (gmoGetVarNameCustomOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarNameCustomOne_t));
                    else
                    {
                        symName = "cgmoGetVarNameCustomOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetVarLower", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarLower = d_gmoGetVarLower;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarlower");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarLower = (gmoGetVarLower_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarLower_t));
                    else
                    {
                        symName = "gmoGetVarLower"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetVarLowerOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarLowerOne = d_gmoGetVarLowerOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarlowerone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarLowerOne = (gmoGetVarLowerOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarLowerOne_t));
                    else
                    {
                        symName = "gmoGetVarLowerOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetVarUpper", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarUpper = d_gmoGetVarUpper;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarupper");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarUpper = (gmoGetVarUpper_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarUpper_t));
                    else
                    {
                        symName = "gmoGetVarUpper"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetVarUpperOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarUpperOne = d_gmoGetVarUpperOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarupperone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarUpperOne = (gmoGetVarUpperOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarUpperOne_t));
                    else
                    {
                        symName = "gmoGetVarUpperOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5, 5 };
                if (gmoxcheck("gmoSetAltVarBounds", 2, s, ref errBuf) == 0)
                    dll_gmoSetAltVarBounds = d_gmoSetAltVarBounds;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetaltvarbounds");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetAltVarBounds = (gmoSetAltVarBounds_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetAltVarBounds_t));
                    else
                    {
                        symName = "gmoSetAltVarBounds"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 13 };
                if (gmoxcheck("gmoSetAltVarLowerOne", 2, s, ref errBuf) == 0)
                    dll_gmoSetAltVarLowerOne = d_gmoSetAltVarLowerOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetaltvarlowerone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetAltVarLowerOne = (gmoSetAltVarLowerOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetAltVarLowerOne_t));
                    else
                    {
                        symName = "gmoSetAltVarLowerOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 13 };
                if (gmoxcheck("gmoSetAltVarUpperOne", 2, s, ref errBuf) == 0)
                    dll_gmoSetAltVarUpperOne = d_gmoSetAltVarUpperOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetaltvarupperone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetAltVarUpperOne = (gmoSetAltVarUpperOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetAltVarUpperOne_t));
                    else
                    {
                        symName = "gmoSetAltVarUpperOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8 };
                if (gmoxcheck("gmoGetVarType", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarType = d_gmoGetVarType;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvartype");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarType = (gmoGetVarType_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarType_t));
                    else
                    {
                        symName = "gmoGetVarType"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetVarTypeOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarTypeOne = d_gmoGetVarTypeOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvartypeone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarTypeOne = (gmoGetVarTypeOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarTypeOne_t));
                    else
                    {
                        symName = "gmoGetVarTypeOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 7 };
                if (gmoxcheck("gmoSetAltVarType", 1, s, ref errBuf) == 0)
                    dll_gmoSetAltVarType = d_gmoSetAltVarType;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetaltvartype");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetAltVarType = (gmoSetAltVarType_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetAltVarType_t));
                    else
                    {
                        symName = "gmoSetAltVarType"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 3 };
                if (gmoxcheck("gmoSetAltVarTypeOne", 2, s, ref errBuf) == 0)
                    dll_gmoSetAltVarTypeOne = d_gmoSetAltVarTypeOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetaltvartypeone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetAltVarTypeOne = (gmoSetAltVarTypeOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetAltVarTypeOne_t));
                    else
                    {
                        symName = "gmoSetAltVarTypeOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 8 };
                if (gmoxcheck("gmoGetVarStat", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarStat = d_gmoGetVarStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarStat = (gmoGetVarStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarStat_t));
                    else
                    {
                        symName = "gmoGetVarStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetVarStatOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarStatOne = d_gmoGetVarStatOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarstatone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarStatOne = (gmoGetVarStatOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarStatOne_t));
                    else
                    {
                        symName = "gmoGetVarStatOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 7 };
                if (gmoxcheck("gmoSetVarStat", 1, s, ref errBuf) == 0)
                    dll_gmoSetVarStat = d_gmoSetVarStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetvarstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetVarStat = (gmoSetVarStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetVarStat_t));
                    else
                    {
                        symName = "gmoSetVarStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 3 };
                if (gmoxcheck("gmoSetVarStatOne", 2, s, ref errBuf) == 0)
                    dll_gmoSetVarStatOne = d_gmoSetVarStatOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetvarstatone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetVarStatOne = (gmoSetVarStatOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetVarStatOne_t));
                    else
                    {
                        symName = "gmoSetVarStatOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 8 };
                if (gmoxcheck("gmoGetVarCStat", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarCStat = d_gmoGetVarCStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarcstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarCStat = (gmoGetVarCStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarCStat_t));
                    else
                    {
                        symName = "gmoGetVarCStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetVarCStatOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarCStatOne = d_gmoGetVarCStatOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarcstatone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarCStatOne = (gmoGetVarCStatOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarCStatOne_t));
                    else
                    {
                        symName = "gmoGetVarCStatOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 7 };
                if (gmoxcheck("gmoSetVarCStat", 1, s, ref errBuf) == 0)
                    dll_gmoSetVarCStat = d_gmoSetVarCStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetvarcstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetVarCStat = (gmoSetVarCStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetVarCStat_t));
                    else
                    {
                        symName = "gmoSetVarCStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8 };
                if (gmoxcheck("gmoGetVarMatch", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarMatch = d_gmoGetVarMatch;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarmatch");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarMatch = (gmoGetVarMatch_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarMatch_t));
                    else
                    {
                        symName = "gmoGetVarMatch"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetVarMatchOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarMatchOne = d_gmoGetVarMatchOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarmatchone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarMatchOne = (gmoGetVarMatchOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarMatchOne_t));
                    else
                    {
                        symName = "gmoGetVarMatchOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetVarPrior", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarPrior = d_gmoGetVarPrior;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarprior");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarPrior = (gmoGetVarPrior_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarPrior_t));
                    else
                    {
                        symName = "gmoGetVarPrior"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetVarPriorOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarPriorOne = d_gmoGetVarPriorOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarpriorone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarPriorOne = (gmoGetVarPriorOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarPriorOne_t));
                    else
                    {
                        symName = "gmoGetVarPriorOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetVarScale", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarScale = d_gmoGetVarScale;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarscale");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarScale = (gmoGetVarScale_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarScale_t));
                    else
                    {
                        symName = "gmoGetVarScale"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetVarScaleOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarScaleOne = d_gmoGetVarScaleOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarscaleone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarScaleOne = (gmoGetVarScaleOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarScaleOne_t));
                    else
                    {
                        symName = "gmoGetVarScaleOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6 };
                if (gmoxcheck("gmoGetVarStage", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarStage = d_gmoGetVarStage;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarstage");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarStage = (gmoGetVarStage_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarStage_t));
                    else
                    {
                        symName = "gmoGetVarStage"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetVarStageOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarStageOne = d_gmoGetVarStageOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarstageone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarStageOne = (gmoGetVarStageOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarStageOne_t));
                    else
                    {
                        symName = "gmoGetVarStageOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 8, 8, 6 };
                if (gmoxcheck("gmoGetSosConstraints", 4, s, ref errBuf) == 0)
                    dll_gmoGetSosConstraints = d_gmoGetSosConstraints;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetsosconstraints");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetSosConstraints = (gmoGetSosConstraints_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetSosConstraints_t));
                    else
                    {
                        symName = "gmoGetSosConstraints"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetVarSosSetOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarSosSetOne = d_gmoGetVarSosSetOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarsossetone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarSosSetOne = (gmoGetVarSosSetOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarSosSetOne_t));
                    else
                    {
                        symName = "gmoGetVarSosSetOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 8, 6, 8, 4, 4 };
                if (gmoxcheck("gmoGetColSparse", 6, s, ref errBuf) == 0)
                    dll_gmoGetColSparse = d_gmoGetColSparse;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetcolsparse");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetColSparse = (gmoGetColSparse_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetColSparse_t));
                    else
                    {
                        symName = "gmoGetColSparse"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 2, 14, 4, 4 };
                if (gmoxcheck("gmoGetColJacInfoOne", 5, s, ref errBuf) == 0)
                    dll_gmoGetColJacInfoOne = d_gmoGetColJacInfoOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetcoljacinfoone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetColJacInfoOne = (gmoGetColJacInfoOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetColJacInfoOne_t));
                    else
                    {
                        symName = "gmoGetColJacInfoOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 11, 8 };
                if (gmoxcheck("gmoGetVarIntDotOpt", 3, s, ref errBuf) == 0)
                    dll_gmoGetVarIntDotOpt = d_gmoGetVarIntDotOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetvarintdotopt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarIntDotOpt = (gmoGetVarIntDotOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarIntDotOpt_t));
                    else
                    {
                        symName = "cgmoGetVarIntDotOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 11, 6 };
                if (gmoxcheck("gmoGetVarDblDotOpt", 3, s, ref errBuf) == 0)
                    dll_gmoGetVarDblDotOpt = d_gmoGetVarDblDotOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetvardbldotopt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarDblDotOpt = (gmoGetVarDblDotOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarDblDotOpt_t));
                    else
                    {
                        symName = "cgmoGetVarDblDotOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoEvalErrorMsg", 1, s, ref errBuf) == 0)
                    dll_gmoEvalErrorMsg = d_gmoEvalErrorMsg;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalerrormsg");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalErrorMsg = (gmoEvalErrorMsg_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalErrorMsg_t));
                    else
                    {
                        symName = "gmoEvalErrorMsg"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15, 3 };
                if (gmoxcheck("gmoEvalErrorMsg_MT", 2, s, ref errBuf) == 0)
                    dll_gmoEvalErrorMsg_MT = d_gmoEvalErrorMsg_MT;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalerrormsg_mt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalErrorMsg_MT = (gmoEvalErrorMsg_MT_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalErrorMsg_MT_t));
                    else
                    {
                        symName = "gmoEvalErrorMsg_MT"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoEvalErrorMaskLevel", 1, s, ref errBuf) == 0)
                    dll_gmoEvalErrorMaskLevel = d_gmoEvalErrorMaskLevel;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalerrormasklevel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalErrorMaskLevel = (gmoEvalErrorMaskLevel_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalErrorMaskLevel_t));
                    else
                    {
                        symName = "gmoEvalErrorMaskLevel"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 3 };
                if (gmoxcheck("gmoEvalErrorMaskLevel_MT", 2, s, ref errBuf) == 0)
                    dll_gmoEvalErrorMaskLevel_MT = d_gmoEvalErrorMaskLevel_MT;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalerrormasklevel_mt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalErrorMaskLevel_MT = (gmoEvalErrorMaskLevel_MT_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalErrorMaskLevel_MT_t));
                    else
                    {
                        symName = "gmoEvalErrorMaskLevel_MT"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5 };
                if (gmoxcheck("gmoEvalNewPoint", 1, s, ref errBuf) == 0)
                    dll_gmoEvalNewPoint = d_gmoEvalNewPoint;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalnewpoint");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalNewPoint = (gmoEvalNewPoint_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalNewPoint_t));
                    else
                    {
                        symName = "gmoEvalNewPoint"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 1 };
                if (gmoxcheck("gmoSetExtFuncs", 1, s, ref errBuf) == 0)
                    dll_gmoSetExtFuncs = d_gmoSetExtFuncs;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetextfuncs");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetExtFuncs = (gmoSetExtFuncs_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetExtFuncs_t));
                    else
                    {
                        symName = "gmoSetExtFuncs"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 14, 4 };
                if (gmoxcheck("gmoEvalFunc", 4, s, ref errBuf) == 0)
                    dll_gmoEvalFunc = d_gmoEvalFunc;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfunc");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFunc = (gmoEvalFunc_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFunc_t));
                    else
                    {
                        symName = "gmoEvalFunc"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 14, 4, 3 };
                if (gmoxcheck("gmoEvalFunc_MT", 5, s, ref errBuf) == 0)
                    dll_gmoEvalFunc_MT = d_gmoEvalFunc_MT;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfunc_mt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFunc_MT = (gmoEvalFunc_MT_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFunc_MT_t));
                    else
                    {
                        symName = "gmoEvalFunc_MT"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 14, 4 };
                if (gmoxcheck("gmoEvalFuncInt", 3, s, ref errBuf) == 0)
                    dll_gmoEvalFuncInt = d_gmoEvalFuncInt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfuncint");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFuncInt = (gmoEvalFuncInt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFuncInt_t));
                    else
                    {
                        symName = "gmoEvalFuncInt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 14, 4, 3 };
                if (gmoxcheck("gmoEvalFuncInt_MT", 4, s, ref errBuf) == 0)
                    dll_gmoEvalFuncInt_MT = d_gmoEvalFuncInt_MT;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfuncint_mt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFuncInt_MT = (gmoEvalFuncInt_MT_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFuncInt_MT_t));
                    else
                    {
                        symName = "gmoEvalFuncInt_MT"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 14, 4 };
                if (gmoxcheck("gmoEvalFuncNL", 4, s, ref errBuf) == 0)
                    dll_gmoEvalFuncNL = d_gmoEvalFuncNL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfuncnl");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFuncNL = (gmoEvalFuncNL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFuncNL_t));
                    else
                    {
                        symName = "gmoEvalFuncNL"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 14, 4, 3 };
                if (gmoxcheck("gmoEvalFuncNL_MT", 5, s, ref errBuf) == 0)
                    dll_gmoEvalFuncNL_MT = d_gmoEvalFuncNL_MT;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfuncnl_mt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFuncNL_MT = (gmoEvalFuncNL_MT_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFuncNL_MT_t));
                    else
                    {
                        symName = "gmoEvalFuncNL_MT"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5, 14, 4 };
                if (gmoxcheck("gmoEvalFuncObj", 3, s, ref errBuf) == 0)
                    dll_gmoEvalFuncObj = d_gmoEvalFuncObj;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfuncobj");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFuncObj = (gmoEvalFuncObj_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFuncObj_t));
                    else
                    {
                        symName = "gmoEvalFuncObj"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5, 14, 4 };
                if (gmoxcheck("gmoEvalFuncNLObj", 3, s, ref errBuf) == 0)
                    dll_gmoEvalFuncNLObj = d_gmoEvalFuncNLObj;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfuncnlobj");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFuncNLObj = (gmoEvalFuncNLObj_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFuncNLObj_t));
                    else
                    {
                        symName = "gmoEvalFuncNLObj"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 5, 14, 14, 4 };
                if (gmoxcheck("gmoEvalFuncInterval", 6, s, ref errBuf) == 0)
                    dll_gmoEvalFuncInterval = d_gmoEvalFuncInterval;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfuncinterval");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFuncInterval = (gmoEvalFuncInterval_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFuncInterval_t));
                    else
                    {
                        symName = "gmoEvalFuncInterval"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 5, 14, 14, 4, 3 };
                if (gmoxcheck("gmoEvalFuncInterval_MT", 7, s, ref errBuf) == 0)
                    dll_gmoEvalFuncInterval_MT = d_gmoEvalFuncInterval_MT;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfuncinterval_mt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFuncInterval_MT = (gmoEvalFuncInterval_MT_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFuncInterval_MT_t));
                    else
                    {
                        symName = "gmoEvalFuncInterval_MT"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 7, 3, 6, 4 };
                if (gmoxcheck("gmoEvalFuncNLCluster", 6, s, ref errBuf) == 0)
                    dll_gmoEvalFuncNLCluster = d_gmoEvalFuncNLCluster;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfuncnlcluster");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFuncNLCluster = (gmoEvalFuncNLCluster_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFuncNLCluster_t));
                    else
                    {
                        symName = "gmoEvalFuncNLCluster"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 7, 3, 6, 4, 3 };
                if (gmoxcheck("gmoEvalFuncNLCluster_MT", 7, s, ref errBuf) == 0)
                    dll_gmoEvalFuncNLCluster_MT = d_gmoEvalFuncNLCluster_MT;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfuncnlcluster_mt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFuncNLCluster_MT = (gmoEvalFuncNLCluster_MT_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFuncNLCluster_MT_t));
                    else
                    {
                        symName = "gmoEvalFuncNLCluster_MT"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 14, 6, 14, 4 };
                if (gmoxcheck("gmoEvalGrad", 6, s, ref errBuf) == 0)
                    dll_gmoEvalGrad = d_gmoEvalGrad;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalgrad");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalGrad = (gmoEvalGrad_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalGrad_t));
                    else
                    {
                        symName = "gmoEvalGrad"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 14, 6, 14, 4, 3 };
                if (gmoxcheck("gmoEvalGrad_MT", 7, s, ref errBuf) == 0)
                    dll_gmoEvalGrad_MT = d_gmoEvalGrad_MT;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalgrad_mt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalGrad_MT = (gmoEvalGrad_MT_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalGrad_MT_t));
                    else
                    {
                        symName = "gmoEvalGrad_MT"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 14, 6, 14, 4 };
                if (gmoxcheck("gmoEvalGradNL", 6, s, ref errBuf) == 0)
                    dll_gmoEvalGradNL = d_gmoEvalGradNL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalgradnl");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalGradNL = (gmoEvalGradNL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalGradNL_t));
                    else
                    {
                        symName = "gmoEvalGradNL"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 14, 6, 14, 4, 3 };
                if (gmoxcheck("gmoEvalGradNL_MT", 7, s, ref errBuf) == 0)
                    dll_gmoEvalGradNL_MT = d_gmoEvalGradNL_MT;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalgradnl_mt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalGradNL_MT = (gmoEvalGradNL_MT_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalGradNL_MT_t));
                    else
                    {
                        symName = "gmoEvalGradNL_MT"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5, 14, 6, 14, 4 };
                if (gmoxcheck("gmoEvalGradObj", 5, s, ref errBuf) == 0)
                    dll_gmoEvalGradObj = d_gmoEvalGradObj;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalgradobj");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalGradObj = (gmoEvalGradObj_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalGradObj_t));
                    else
                    {
                        symName = "gmoEvalGradObj"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5, 14, 6, 14, 4 };
                if (gmoxcheck("gmoEvalGradNLObj", 5, s, ref errBuf) == 0)
                    dll_gmoEvalGradNLObj = d_gmoEvalGradNLObj;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalgradnlobj");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalGradNLObj = (gmoEvalGradNLObj_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalGradNLObj_t));
                    else
                    {
                        symName = "gmoEvalGradNLObj"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 5, 14, 14, 6, 6, 4 };
                if (gmoxcheck("gmoEvalGradInterval", 8, s, ref errBuf) == 0)
                    dll_gmoEvalGradInterval = d_gmoEvalGradInterval;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalgradinterval");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalGradInterval = (gmoEvalGradInterval_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalGradInterval_t));
                    else
                    {
                        symName = "gmoEvalGradInterval"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 5, 14, 14, 6, 6, 4, 3 };
                if (gmoxcheck("gmoEvalGradInterval_MT", 9, s, ref errBuf) == 0)
                    dll_gmoEvalGradInterval_MT = d_gmoEvalGradInterval_MT;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalgradinterval_mt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalGradInterval_MT = (gmoEvalGradInterval_MT_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalGradInterval_MT_t));
                    else
                    {
                        symName = "gmoEvalGradInterval_MT"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 6, 15, 4 };
                if (gmoxcheck("gmoEvalGradNLUpdate", 3, s, ref errBuf) == 0)
                    dll_gmoEvalGradNLUpdate = d_gmoEvalGradNLUpdate;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalgradnlupdate");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalGradNLUpdate = (gmoEvalGradNLUpdate_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalGradNLUpdate_t));
                    else
                    {
                        symName = "gmoEvalGradNLUpdate"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 8, 6, 21 };
                if (gmoxcheck("gmoGetJacUpdate", 4, s, ref errBuf) == 0)
                    dll_gmoGetJacUpdate = d_gmoGetJacUpdate;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetjacupdate");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetJacUpdate = (gmoGetJacUpdate_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetJacUpdate_t));
                    else
                    {
                        symName = "gmoGetJacUpdate"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 13, 21, 21 };
                if (gmoxcheck("gmoHessLoad", 3, s, ref errBuf) == 0)
                    dll_gmoHessLoad = d_gmoHessLoad;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohessload");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessLoad = (gmoHessLoad_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessLoad_t));
                    else
                    {
                        symName = "gmoHessLoad"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoHessUnload", 0, s, ref errBuf) == 0)
                    dll_gmoHessUnload = d_gmoHessUnload;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohessunload");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessUnload = (gmoHessUnload_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessUnload_t));
                    else
                    {
                        symName = "gmoHessUnload"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoHessDim", 1, s, ref errBuf) == 0)
                    dll_gmoHessDim = d_gmoHessDim;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohessdim");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessDim = (gmoHessDim_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessDim_t));
                    else
                    {
                        symName = "gmoHessDim"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoHessNz", 1, s, ref errBuf) == 0)
                    dll_gmoHessNz = d_gmoHessNz;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohessnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessNz = (gmoHessNz_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessNz_t));
                    else
                    {
                        symName = "gmoHessNz"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 8, 8, 4, 4 };
                if (gmoxcheck("gmoHessStruct", 5, s, ref errBuf) == 0)
                    dll_gmoHessStruct = d_gmoHessStruct;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohessstruct");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessStruct = (gmoHessStruct_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessStruct_t));
                    else
                    {
                        symName = "gmoHessStruct"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 8, 8, 4, 4, 5, 6, 4 };
                if (gmoxcheck("gmoHessValue", 8, s, ref errBuf) == 0)
                    dll_gmoHessValue = d_gmoHessValue;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohessvalue");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessValue = (gmoHessValue_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessValue_t));
                    else
                    {
                        symName = "gmoHessValue"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 5, 6, 4 };
                if (gmoxcheck("gmoHessVec", 5, s, ref errBuf) == 0)
                    dll_gmoHessVec = d_gmoHessVec;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohessvec");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessVec = (gmoHessVec_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessVec_t));
                    else
                    {
                        symName = "gmoHessVec"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 8 };
                if (gmoxcheck("gmoHessLagStruct", 2, s, ref errBuf) == 0)
                    dll_gmoHessLagStruct = d_gmoHessLagStruct;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohesslagstruct");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessLagStruct = (gmoHessLagStruct_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessLagStruct_t));
                    else
                    {
                        symName = "gmoHessLagStruct"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5, 5, 6, 13, 13, 4 };
                if (gmoxcheck("gmoHessLagValue", 6, s, ref errBuf) == 0)
                    dll_gmoHessLagValue = d_gmoHessLagValue;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohesslagvalue");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessLagValue = (gmoHessLagValue_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessLagValue_t));
                    else
                    {
                        symName = "gmoHessLagValue"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5, 5, 5, 6, 13, 13, 4 };
                if (gmoxcheck("gmoHessLagVec", 7, s, ref errBuf) == 0)
                    dll_gmoHessLagVec = d_gmoHessLagVec;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohesslagvec");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessLagVec = (gmoHessLagVec_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessLagVec_t));
                    else
                    {
                        symName = "gmoHessLagVec"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11 };
                if (gmoxcheck("gmoLoadEMPInfo", 1, s, ref errBuf) == 0)
                    dll_gmoLoadEMPInfo = d_gmoLoadEMPInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmoloadempinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoLoadEMPInfo = (gmoLoadEMPInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoLoadEMPInfo_t));
                    else
                    {
                        symName = "cgmoLoadEMPInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8 };
                if (gmoxcheck("gmoGetEquVI", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquVI = d_gmoGetEquVI;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequvi");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquVI = (gmoGetEquVI_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquVI_t));
                    else
                    {
                        symName = "gmoGetEquVI"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetEquVIOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetEquVIOne = d_gmoGetEquVIOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetequvione");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquVIOne = (gmoGetEquVIOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquVIOne_t));
                    else
                    {
                        symName = "gmoGetEquVIOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8 };
                if (gmoxcheck("gmoGetVarVI", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarVI = d_gmoGetVarVI;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarvi");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarVI = (gmoGetVarVI_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarVI_t));
                    else
                    {
                        symName = "gmoGetVarVI"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetVarVIOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetVarVIOne = d_gmoGetVarVIOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarvione");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarVIOne = (gmoGetVarVIOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarVIOne_t));
                    else
                    {
                        symName = "gmoGetVarVIOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8 };
                if (gmoxcheck("gmoGetAgentType", 1, s, ref errBuf) == 0)
                    dll_gmoGetAgentType = d_gmoGetAgentType;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetagenttype");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetAgentType = (gmoGetAgentType_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetAgentType_t));
                    else
                    {
                        symName = "gmoGetAgentType"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3 };
                if (gmoxcheck("gmoGetAgentTypeOne", 1, s, ref errBuf) == 0)
                    dll_gmoGetAgentTypeOne = d_gmoGetAgentTypeOne;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetagenttypeone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetAgentTypeOne = (gmoGetAgentTypeOne_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetAgentTypeOne_t));
                    else
                    {
                        symName = "gmoGetAgentTypeOne"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 8 };
                if (gmoxcheck("gmoGetBiLevelInfo", 2, s, ref errBuf) == 0)
                    dll_gmoGetBiLevelInfo = d_gmoGetBiLevelInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetbilevelinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetBiLevelInfo = (gmoGetBiLevelInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetBiLevelInfo_t));
                    else
                    {
                        symName = "gmoGetBiLevelInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11 };
                if (gmoxcheck("gmoDumpEMPInfoToGDX", 1, s, ref errBuf) == 0)
                    dll_gmoDumpEMPInfoToGDX = d_gmoDumpEMPInfoToGDX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmodumpempinfotogdx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoDumpEMPInfoToGDX = (gmoDumpEMPInfoToGDX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoDumpEMPInfoToGDX_t));
                    else
                    {
                        symName = "cgmoDumpEMPInfoToGDX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 3 };
                if (gmoxcheck("gmoGetHeadnTail", 1, s, ref errBuf) == 0)
                    dll_gmoGetHeadnTail = d_gmoGetHeadnTail;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetheadntail");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetHeadnTail = (gmoGetHeadnTail_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetHeadnTail_t));
                    else
                    {
                        symName = "gmoGetHeadnTail"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 13 };
                if (gmoxcheck("gmoSetHeadnTail", 2, s, ref errBuf) == 0)
                    dll_gmoSetHeadnTail = d_gmoSetHeadnTail;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetheadntail");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetHeadnTail = (gmoSetHeadnTail_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetHeadnTail_t));
                    else
                    {
                        symName = "gmoSetHeadnTail"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5 };
                if (gmoxcheck("gmoSetSolutionPrimal", 1, s, ref errBuf) == 0)
                    dll_gmoSetSolutionPrimal = d_gmoSetSolutionPrimal;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetsolutionprimal");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetSolutionPrimal = (gmoSetSolutionPrimal_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetSolutionPrimal_t));
                    else
                    {
                        symName = "gmoSetSolutionPrimal"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5, 5 };
                if (gmoxcheck("gmoSetSolution2", 2, s, ref errBuf) == 0)
                    dll_gmoSetSolution2 = d_gmoSetSolution2;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetsolution2");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetSolution2 = (gmoSetSolution2_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetSolution2_t));
                    else
                    {
                        symName = "gmoSetSolution2"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5, 5, 5, 5 };
                if (gmoxcheck("gmoSetSolution", 4, s, ref errBuf) == 0)
                    dll_gmoSetSolution = d_gmoSetSolution;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetsolution");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetSolution = (gmoSetSolution_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetSolution_t));
                    else
                    {
                        symName = "gmoSetSolution"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 5, 5, 5, 5, 8, 8, 8, 8 };
                if (gmoxcheck("gmoSetSolution8", 8, s, ref errBuf) == 0)
                    dll_gmoSetSolution8 = d_gmoSetSolution8;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetsolution8");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetSolution8 = (gmoSetSolution8_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetSolution8_t));
                    else
                    {
                        symName = "gmoSetSolution8"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 5, 5, 7, 7, 13, 13 };
                if (gmoxcheck("gmoSetSolutionFixer", 7, s, ref errBuf) == 0)
                    dll_gmoSetSolutionFixer = d_gmoSetSolutionFixer;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetsolutionfixer");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetSolutionFixer = (gmoSetSolutionFixer_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetSolutionFixer_t));
                    else
                    {
                        symName = "gmoSetSolutionFixer"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 14, 14, 4, 4 };
                if (gmoxcheck("gmoGetSolutionVarRec", 5, s, ref errBuf) == 0)
                    dll_gmoGetSolutionVarRec = d_gmoGetSolutionVarRec;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetsolutionvarrec");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetSolutionVarRec = (gmoGetSolutionVarRec_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetSolutionVarRec_t));
                    else
                    {
                        symName = "gmoGetSolutionVarRec"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 13, 13, 3, 3 };
                if (gmoxcheck("gmoSetSolutionVarRec", 5, s, ref errBuf) == 0)
                    dll_gmoSetSolutionVarRec = d_gmoSetSolutionVarRec;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetsolutionvarrec");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetSolutionVarRec = (gmoSetSolutionVarRec_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetSolutionVarRec_t));
                    else
                    {
                        symName = "gmoSetSolutionVarRec"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 14, 14, 4, 4 };
                if (gmoxcheck("gmoGetSolutionEquRec", 5, s, ref errBuf) == 0)
                    dll_gmoGetSolutionEquRec = d_gmoGetSolutionEquRec;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetsolutionequrec");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetSolutionEquRec = (gmoGetSolutionEquRec_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetSolutionEquRec_t));
                    else
                    {
                        symName = "gmoGetSolutionEquRec"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 13, 13, 3, 3 };
                if (gmoxcheck("gmoSetSolutionEquRec", 5, s, ref errBuf) == 0)
                    dll_gmoSetSolutionEquRec = d_gmoSetSolutionEquRec;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetsolutionequrec");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetSolutionEquRec = (gmoSetSolutionEquRec_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetSolutionEquRec_t));
                    else
                    {
                        symName = "gmoSetSolutionEquRec"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 8, 8, 8, 8 };
                if (gmoxcheck("gmoSetSolutionStatus", 4, s, ref errBuf) == 0)
                    dll_gmoSetSolutionStatus = d_gmoSetSolutionStatus;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetsolutionstatus");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetSolutionStatus = (gmoSetSolutionStatus_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetSolutionStatus_t));
                    else
                    {
                        symName = "gmoSetSolutionStatus"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 13 };
                if (gmoxcheck("gmoCompleteObjective", 1, s, ref errBuf) == 0)
                    dll_gmoCompleteObjective = d_gmoCompleteObjective;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmocompleteobjective");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoCompleteObjective = (gmoCompleteObjective_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoCompleteObjective_t));
                    else
                    {
                        symName = "gmoCompleteObjective"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoCompleteSolution", 0, s, ref errBuf) == 0)
                    dll_gmoCompleteSolution = d_gmoCompleteSolution;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmocompletesolution");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoCompleteSolution = (gmoCompleteSolution_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoCompleteSolution_t));
                    else
                    {
                        symName = "gmoCompleteSolution"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoGetAbsoluteGap", 0, s, ref errBuf) == 0)
                    dll_gmoGetAbsoluteGap = d_gmoGetAbsoluteGap;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetabsolutegap");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetAbsoluteGap = (gmoGetAbsoluteGap_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetAbsoluteGap_t));
                    else
                    {
                        symName = "gmoGetAbsoluteGap"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoGetRelativeGap", 0, s, ref errBuf) == 0)
                    dll_gmoGetRelativeGap = d_gmoGetRelativeGap;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetrelativegap");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetRelativeGap = (gmoGetRelativeGap_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetRelativeGap_t));
                    else
                    {
                        symName = "gmoGetRelativeGap"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoLoadSolutionLegacy", 0, s, ref errBuf) == 0)
                    dll_gmoLoadSolutionLegacy = d_gmoLoadSolutionLegacy;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoloadsolutionlegacy");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoLoadSolutionLegacy = (gmoLoadSolutionLegacy_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoLoadSolutionLegacy_t));
                    else
                    {
                        symName = "gmoLoadSolutionLegacy"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoUnloadSolutionLegacy", 0, s, ref errBuf) == 0)
                    dll_gmoUnloadSolutionLegacy = d_gmoUnloadSolutionLegacy;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmounloadsolutionlegacy");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoUnloadSolutionLegacy = (gmoUnloadSolutionLegacy_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoUnloadSolutionLegacy_t));
                    else
                    {
                        symName = "gmoUnloadSolutionLegacy"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 15, 15, 15 };
                if (gmoxcheck("gmoLoadSolutionGDX", 4, s, ref errBuf) == 0)
                    dll_gmoLoadSolutionGDX = d_gmoLoadSolutionGDX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmoloadsolutiongdx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoLoadSolutionGDX = (gmoLoadSolutionGDX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoLoadSolutionGDX_t));
                    else
                    {
                        symName = "cgmoLoadSolutionGDX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 15, 15, 15 };
                if (gmoxcheck("gmoUnloadSolutionGDX", 4, s, ref errBuf) == 0)
                    dll_gmoUnloadSolutionGDX = d_gmoUnloadSolutionGDX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmounloadsolutiongdx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoUnloadSolutionGDX = (gmoUnloadSolutionGDX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoUnloadSolutionGDX_t));
                    else
                    {
                        symName = "cgmoUnloadSolutionGDX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 1, 3 };
                if (gmoxcheck("gmoPrepareAllSolToGDX", 3, s, ref errBuf) == 0)
                    dll_gmoPrepareAllSolToGDX = d_gmoPrepareAllSolToGDX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmoprepareallsoltogdx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPrepareAllSolToGDX = (gmoPrepareAllSolToGDX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPrepareAllSolToGDX_t));
                    else
                    {
                        symName = "cgmoPrepareAllSolToGDX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 55 };
                if (gmoxcheck("gmoAddSolutionToGDX", 1, s, ref errBuf) == 0)
                    dll_gmoAddSolutionToGDX = d_gmoAddSolutionToGDX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmoaddsolutiontogdx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoAddSolutionToGDX = (gmoAddSolutionToGDX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoAddSolutionToGDX_t));
                    else
                    {
                        symName = "cgmoAddSolutionToGDX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 12 };
                if (gmoxcheck("gmoWriteSolDone", 1, s, ref errBuf) == 0)
                    dll_gmoWriteSolDone = d_gmoWriteSolDone;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmowritesoldone");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoWriteSolDone = (gmoWriteSolDone_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoWriteSolDone_t));
                    else
                    {
                        symName = "cgmoWriteSolDone"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 21 };
                if (gmoxcheck("gmoCheckSolPoolUEL", 2, s, ref errBuf) == 0)
                    dll_gmoCheckSolPoolUEL = d_gmoCheckSolPoolUEL;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmochecksolpooluel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoCheckSolPoolUEL = (gmoCheckSolPoolUEL_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoCheckSolPoolUEL_t));
                    else
                    {
                        symName = "cgmoCheckSolPoolUEL"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 1, 11, 3, 11 };
                if (gmoxcheck("gmoPrepareSolPoolMerge", 3, s, ref errBuf) == 0)
                    dll_gmoPrepareSolPoolMerge = d_gmoPrepareSolPoolMerge;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmopreparesolpoolmerge");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPrepareSolPoolMerge = (gmoPrepareSolPoolMerge_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPrepareSolPoolMerge_t));
                    else
                    {
                        symName = "cgmoPrepareSolPoolMerge"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1 };
                if (gmoxcheck("gmoPrepareSolPoolNextSym", 1, s, ref errBuf) == 0)
                    dll_gmoPrepareSolPoolNextSym = d_gmoPrepareSolPoolNextSym;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmopreparesolpoolnextsym");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPrepareSolPoolNextSym = (gmoPrepareSolPoolNextSym_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPrepareSolPoolNextSym_t));
                    else
                    {
                        symName = "gmoPrepareSolPoolNextSym"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 3 };
                if (gmoxcheck("gmoUnloadSolPoolSolution", 2, s, ref errBuf) == 0)
                    dll_gmoUnloadSolPoolSolution = d_gmoUnloadSolPoolSolution;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmounloadsolpoolsolution");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoUnloadSolPoolSolution = (gmoUnloadSolPoolSolution_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoUnloadSolPoolSolution_t));
                    else
                    {
                        symName = "gmoUnloadSolPoolSolution"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1 };
                if (gmoxcheck("gmoFinalizeSolPoolMerge", 1, s, ref errBuf) == 0)
                    dll_gmoFinalizeSolPoolMerge = d_gmoFinalizeSolPoolMerge;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmofinalizesolpoolmerge");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoFinalizeSolPoolMerge = (gmoFinalizeSolPoolMerge_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoFinalizeSolPoolMerge_t));
                    else
                    {
                        symName = "gmoFinalizeSolPoolMerge"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12 };
                if (gmoxcheck("gmoGetVarTypeTxt", 2, s, ref errBuf) == 0)
                    dll_gmoGetVarTypeTxt = d_gmoGetVarTypeTxt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetvartypetxt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarTypeTxt = (gmoGetVarTypeTxt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarTypeTxt_t));
                    else
                    {
                        symName = "cgmoGetVarTypeTxt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12 };
                if (gmoxcheck("gmoGetEquTypeTxt", 2, s, ref errBuf) == 0)
                    dll_gmoGetEquTypeTxt = d_gmoGetEquTypeTxt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetequtypetxt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetEquTypeTxt = (gmoGetEquTypeTxt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetEquTypeTxt_t));
                    else
                    {
                        symName = "cgmoGetEquTypeTxt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12 };
                if (gmoxcheck("gmoGetSolveStatusTxt", 2, s, ref errBuf) == 0)
                    dll_gmoGetSolveStatusTxt = d_gmoGetSolveStatusTxt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetsolvestatustxt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetSolveStatusTxt = (gmoGetSolveStatusTxt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetSolveStatusTxt_t));
                    else
                    {
                        symName = "cgmoGetSolveStatusTxt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12 };
                if (gmoxcheck("gmoGetModelStatusTxt", 2, s, ref errBuf) == 0)
                    dll_gmoGetModelStatusTxt = d_gmoGetModelStatusTxt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetmodelstatustxt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetModelStatusTxt = (gmoGetModelStatusTxt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetModelStatusTxt_t));
                    else
                    {
                        symName = "cgmoGetModelStatusTxt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12 };
                if (gmoxcheck("gmoGetModelTypeTxt", 2, s, ref errBuf) == 0)
                    dll_gmoGetModelTypeTxt = d_gmoGetModelTypeTxt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetmodeltypetxt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetModelTypeTxt = (gmoGetModelTypeTxt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetModelTypeTxt_t));
                    else
                    {
                        symName = "cgmoGetModelTypeTxt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 12 };
                if (gmoxcheck("gmoGetHeadNTailTxt", 2, s, ref errBuf) == 0)
                    dll_gmoGetHeadNTailTxt = d_gmoGetHeadNTailTxt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetheadntailtxt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetHeadNTailTxt = (gmoGetHeadNTailTxt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetHeadNTailTxt_t));
                    else
                    {
                        symName = "cgmoGetHeadNTailTxt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoMemUsed", 0, s, ref errBuf) == 0)
                    dll_gmoMemUsed = d_gmoMemUsed;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmomemused");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoMemUsed = (gmoMemUsed_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoMemUsed_t));
                    else
                    {
                        symName = "gmoMemUsed"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoPeakMemUsed", 0, s, ref errBuf) == 0)
                    dll_gmoPeakMemUsed = d_gmoPeakMemUsed;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmopeakmemused");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPeakMemUsed = (gmoPeakMemUsed_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPeakMemUsed_t));
                    else
                    {
                        symName = "gmoPeakMemUsed"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 1 };
                if (gmoxcheck("gmoSetNLObject", 2, s, ref errBuf) == 0)
                    dll_gmoSetNLObject = d_gmoSetNLObject;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosetnlobject");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSetNLObject = (gmoSetNLObject_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSetNLObject_t));
                    else
                    {
                        symName = "gmoSetNLObject"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11 };
                if (gmoxcheck("gmoDumpQMakerGDX", 1, s, ref errBuf) == 0)
                    dll_gmoDumpQMakerGDX = d_gmoDumpQMakerGDX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmodumpqmakergdx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoDumpQMakerGDX = (gmoDumpQMakerGDX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoDumpQMakerGDX_t));
                    else
                    {
                        symName = "cgmoDumpQMakerGDX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 1, 3, 21, 8, 8, 8 };
                if (gmoxcheck("gmoGetVarEquMap", 7, s, ref errBuf) == 0)
                    dll_gmoGetVarEquMap = d_gmoGetVarEquMap;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetvarequmap");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetVarEquMap = (gmoGetVarEquMap_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetVarEquMap_t));
                    else
                    {
                        symName = "gmoGetVarEquMap"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 3, 21, 8, 8, 8 };
                if (gmoxcheck("gmoGetIndicatorMap", 6, s, ref errBuf) == 0)
                    dll_gmoGetIndicatorMap = d_gmoGetIndicatorMap;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetindicatormap");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetIndicatorMap = (gmoGetIndicatorMap_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetIndicatorMap_t));
                    else
                    {
                        symName = "gmoGetIndicatorMap"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoCrudeness", 0, s, ref errBuf) == 0)
                    dll_gmoCrudeness = d_gmoCrudeness;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmocrudeness");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoCrudeness = (gmoCrudeness_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoCrudeness_t));
                    else
                    {
                        symName = "gmoCrudeness"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 4, 8, 8 };
                if (gmoxcheck("gmoDirtyGetRowFNLInstr", 4, s, ref errBuf) == 0)
                    dll_gmoDirtyGetRowFNLInstr = d_gmoDirtyGetRowFNLInstr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmodirtygetrowfnlinstr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoDirtyGetRowFNLInstr = (gmoDirtyGetRowFNLInstr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoDirtyGetRowFNLInstr_t));
                    else
                    {
                        symName = "gmoDirtyGetRowFNLInstr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 4, 8, 8 };
                if (gmoxcheck("gmoDirtyGetObjFNLInstr", 3, s, ref errBuf) == 0)
                    dll_gmoDirtyGetObjFNLInstr = d_gmoDirtyGetObjFNLInstr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmodirtygetobjfnlinstr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoDirtyGetObjFNLInstr = (gmoDirtyGetObjFNLInstr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoDirtyGetObjFNLInstr_t));
                    else
                    {
                        symName = "gmoDirtyGetObjFNLInstr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 3, 7, 7, 1, 6, 3 };
                if (gmoxcheck("gmoDirtySetRowFNLInstr", 7, s, ref errBuf) == 0)
                    dll_gmoDirtySetRowFNLInstr = d_gmoDirtySetRowFNLInstr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmodirtysetrowfnlinstr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoDirtySetRowFNLInstr = (gmoDirtySetRowFNLInstr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoDirtySetRowFNLInstr_t));
                    else
                    {
                        symName = "gmoDirtySetRowFNLInstr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 3 };
                if (gmoxcheck("gmoGetExtrLibName", 1, s, ref errBuf) == 0)
                    dll_gmoGetExtrLibName = d_gmoGetExtrLibName;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetextrlibname");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetExtrLibName = (gmoGetExtrLibName_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetExtrLibName_t));
                    else
                    {
                        symName = "cgmoGetExtrLibName"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 1, 3 };
                if (gmoxcheck("gmoGetExtrLibObjPtr", 1, s, ref errBuf) == 0)
                    dll_gmoGetExtrLibObjPtr = d_gmoGetExtrLibObjPtr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetextrlibobjptr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetExtrLibObjPtr = (gmoGetExtrLibObjPtr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetExtrLibObjPtr_t));
                    else
                    {
                        symName = "gmoGetExtrLibObjPtr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 3, 3 };
                if (gmoxcheck("gmoGetExtrLibFuncName", 2, s, ref errBuf) == 0)
                    dll_gmoGetExtrLibFuncName = d_gmoGetExtrLibFuncName;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmogetextrlibfuncname");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetExtrLibFuncName = (gmoGetExtrLibFuncName_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetExtrLibFuncName_t));
                    else
                    {
                        symName = "cgmoGetExtrLibFuncName"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 1, 3, 11, 12 };
                if (gmoxcheck("gmoLoadExtrLibEntry", 3, s, ref errBuf) == 0)
                    dll_gmoLoadExtrLibEntry = d_gmoLoadExtrLibEntry;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmoloadextrlibentry");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoLoadExtrLibEntry = (gmoLoadExtrLibEntry_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoLoadExtrLibEntry_t));
                    else
                    {
                        symName = "cgmoLoadExtrLibEntry"; goto symMissing;
                    }
                }
            }

            {
                int[] s = { 1 };
                if (gmoxcheck("gmoDict", 0, s, ref errBuf) == 0)
                    dll_gmoDict = d_gmoDict;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmodict");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoDict = (gmoDict_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoDict_t));
                    else
                    {
                        symName = "gmoDict"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 1 };
                if (gmoxcheck("gmoDictSet", 1, s, ref errBuf) == 0)
                    dll_gmoDictSet = d_gmoDictSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmodictset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoDictSet = (gmoDictSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoDictSet_t));
                    else
                    {
                        symName = "gmoDictSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12 };
                if (gmoxcheck("gmoNameModel", 0, s, ref errBuf) == 0)
                    dll_gmoNameModel = d_gmoNameModel;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonamemodel");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameModel = (gmoNameModel_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameModel_t));
                    else
                    {
                        symName = "cgmoNameModel"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 12 };
                if (gmoxcheck("gmoNameModelSet", 1, s, ref errBuf) == 0)
                    dll_gmoNameModelSet = d_gmoNameModelSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonamemodelset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameModelSet = (gmoNameModelSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameModelSet_t));
                    else
                    {
                        symName = "cgmoNameModelSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoModelSeqNr", 0, s, ref errBuf) == 0)
                    dll_gmoModelSeqNr = d_gmoModelSeqNr;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmomodelseqnr");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoModelSeqNr = (gmoModelSeqNr_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoModelSeqNr_t));
                    else
                    {
                        symName = "gmoModelSeqNr"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoModelSeqNrSet", 1, s, ref errBuf) == 0)
                    dll_gmoModelSeqNrSet = d_gmoModelSeqNrSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmomodelseqnrset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoModelSeqNrSet = (gmoModelSeqNrSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoModelSeqNrSet_t));
                    else
                    {
                        symName = "gmoModelSeqNrSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoModelType", 0, s, ref errBuf) == 0)
                    dll_gmoModelType = d_gmoModelType;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmomodeltype");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoModelType = (gmoModelType_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoModelType_t));
                    else
                    {
                        symName = "gmoModelType"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoModelTypeSet", 1, s, ref errBuf) == 0)
                    dll_gmoModelTypeSet = d_gmoModelTypeSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmomodeltypeset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoModelTypeSet = (gmoModelTypeSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoModelTypeSet_t));
                    else
                    {
                        symName = "gmoModelTypeSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoNLModelType", 0, s, ref errBuf) == 0)
                    dll_gmoNLModelType = d_gmoNLModelType;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonlmodeltype");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNLModelType = (gmoNLModelType_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNLModelType_t));
                    else
                    {
                        symName = "gmoNLModelType"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoSense", 0, s, ref errBuf) == 0)
                    dll_gmoSense = d_gmoSense;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosense");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSense = (gmoSense_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSense_t));
                    else
                    {
                        symName = "gmoSense"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoSenseSet", 1, s, ref errBuf) == 0)
                    dll_gmoSenseSet = d_gmoSenseSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosenseset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSenseSet = (gmoSenseSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSenseSet_t));
                    else
                    {
                        symName = "gmoSenseSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoIsQP", 0, s, ref errBuf) == 0)
                    dll_gmoIsQP = d_gmoIsQP;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoisqp");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoIsQP = (gmoIsQP_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoIsQP_t));
                    else
                    {
                        symName = "gmoIsQP"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoOptFile", 0, s, ref errBuf) == 0)
                    dll_gmoOptFile = d_gmoOptFile;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmooptfile");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoOptFile = (gmoOptFile_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoOptFile_t));
                    else
                    {
                        symName = "gmoOptFile"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoOptFileSet", 1, s, ref errBuf) == 0)
                    dll_gmoOptFileSet = d_gmoOptFileSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmooptfileset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoOptFileSet = (gmoOptFileSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoOptFileSet_t));
                    else
                    {
                        symName = "gmoOptFileSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoDictionary", 0, s, ref errBuf) == 0)
                    dll_gmoDictionary = d_gmoDictionary;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmodictionary");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoDictionary = (gmoDictionary_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoDictionary_t));
                    else
                    {
                        symName = "gmoDictionary"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoDictionarySet", 1, s, ref errBuf) == 0)
                    dll_gmoDictionarySet = d_gmoDictionarySet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmodictionaryset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoDictionarySet = (gmoDictionarySet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoDictionarySet_t));
                    else
                    {
                        symName = "gmoDictionarySet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoScaleOpt", 0, s, ref errBuf) == 0)
                    dll_gmoScaleOpt = d_gmoScaleOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoscaleopt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoScaleOpt = (gmoScaleOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoScaleOpt_t));
                    else
                    {
                        symName = "gmoScaleOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoScaleOptSet", 1, s, ref errBuf) == 0)
                    dll_gmoScaleOptSet = d_gmoScaleOptSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoscaleoptset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoScaleOptSet = (gmoScaleOptSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoScaleOptSet_t));
                    else
                    {
                        symName = "gmoScaleOptSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoPriorOpt", 0, s, ref errBuf) == 0)
                    dll_gmoPriorOpt = d_gmoPriorOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoprioropt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPriorOpt = (gmoPriorOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPriorOpt_t));
                    else
                    {
                        symName = "gmoPriorOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoPriorOptSet", 1, s, ref errBuf) == 0)
                    dll_gmoPriorOptSet = d_gmoPriorOptSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoprioroptset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPriorOptSet = (gmoPriorOptSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPriorOptSet_t));
                    else
                    {
                        symName = "gmoPriorOptSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoHaveBasis", 0, s, ref errBuf) == 0)
                    dll_gmoHaveBasis = d_gmoHaveBasis;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohavebasis");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHaveBasis = (gmoHaveBasis_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHaveBasis_t));
                    else
                    {
                        symName = "gmoHaveBasis"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoHaveBasisSet", 1, s, ref errBuf) == 0)
                    dll_gmoHaveBasisSet = d_gmoHaveBasisSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohavebasisset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHaveBasisSet = (gmoHaveBasisSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHaveBasisSet_t));
                    else
                    {
                        symName = "gmoHaveBasisSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoModelStat", 0, s, ref errBuf) == 0)
                    dll_gmoModelStat = d_gmoModelStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmomodelstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoModelStat = (gmoModelStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoModelStat_t));
                    else
                    {
                        symName = "gmoModelStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoModelStatSet", 1, s, ref errBuf) == 0)
                    dll_gmoModelStatSet = d_gmoModelStatSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmomodelstatset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoModelStatSet = (gmoModelStatSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoModelStatSet_t));
                    else
                    {
                        symName = "gmoModelStatSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoSolveStat", 0, s, ref errBuf) == 0)
                    dll_gmoSolveStat = d_gmoSolveStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosolvestat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSolveStat = (gmoSolveStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSolveStat_t));
                    else
                    {
                        symName = "gmoSolveStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoSolveStatSet", 1, s, ref errBuf) == 0)
                    dll_gmoSolveStatSet = d_gmoSolveStatSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmosolvestatset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoSolveStatSet = (gmoSolveStatSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoSolveStatSet_t));
                    else
                    {
                        symName = "gmoSolveStatSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoIsMPSGE", 0, s, ref errBuf) == 0)
                    dll_gmoIsMPSGE = d_gmoIsMPSGE;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoismpsge");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoIsMPSGE = (gmoIsMPSGE_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoIsMPSGE_t));
                    else
                    {
                        symName = "gmoIsMPSGE"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoIsMPSGESet", 1, s, ref errBuf) == 0)
                    dll_gmoIsMPSGESet = d_gmoIsMPSGESet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoismpsgeset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoIsMPSGESet = (gmoIsMPSGESet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoIsMPSGESet_t));
                    else
                    {
                        symName = "gmoIsMPSGESet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoObjStyle", 0, s, ref errBuf) == 0)
                    dll_gmoObjStyle = d_gmoObjStyle;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjstyle");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjStyle = (gmoObjStyle_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjStyle_t));
                    else
                    {
                        symName = "gmoObjStyle"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoObjStyleSet", 1, s, ref errBuf) == 0)
                    dll_gmoObjStyleSet = d_gmoObjStyleSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjstyleset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjStyleSet = (gmoObjStyleSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjStyleSet_t));
                    else
                    {
                        symName = "gmoObjStyleSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoInterface", 0, s, ref errBuf) == 0)
                    dll_gmoInterface = d_gmoInterface;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmointerface");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoInterface = (gmoInterface_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoInterface_t));
                    else
                    {
                        symName = "gmoInterface"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoInterfaceSet", 1, s, ref errBuf) == 0)
                    dll_gmoInterfaceSet = d_gmoInterfaceSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmointerfaceset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoInterfaceSet = (gmoInterfaceSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoInterfaceSet_t));
                    else
                    {
                        symName = "gmoInterfaceSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoIndexBase", 0, s, ref errBuf) == 0)
                    dll_gmoIndexBase = d_gmoIndexBase;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoindexbase");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoIndexBase = (gmoIndexBase_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoIndexBase_t));
                    else
                    {
                        symName = "gmoIndexBase"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoIndexBaseSet", 1, s, ref errBuf) == 0)
                    dll_gmoIndexBaseSet = d_gmoIndexBaseSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoindexbaseset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoIndexBaseSet = (gmoIndexBaseSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoIndexBaseSet_t));
                    else
                    {
                        symName = "gmoIndexBaseSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoObjReform", 0, s, ref errBuf) == 0)
                    dll_gmoObjReform = d_gmoObjReform;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjreform");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjReform = (gmoObjReform_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjReform_t));
                    else
                    {
                        symName = "gmoObjReform"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoObjReformSet", 1, s, ref errBuf) == 0)
                    dll_gmoObjReformSet = d_gmoObjReformSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjreformset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjReformSet = (gmoObjReformSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjReformSet_t));
                    else
                    {
                        symName = "gmoObjReformSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoEmptyOut", 0, s, ref errBuf) == 0)
                    dll_gmoEmptyOut = d_gmoEmptyOut;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoemptyout");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEmptyOut = (gmoEmptyOut_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEmptyOut_t));
                    else
                    {
                        symName = "gmoEmptyOut"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoEmptyOutSet", 1, s, ref errBuf) == 0)
                    dll_gmoEmptyOutSet = d_gmoEmptyOutSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoemptyoutset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEmptyOutSet = (gmoEmptyOutSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEmptyOutSet_t));
                    else
                    {
                        symName = "gmoEmptyOutSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoIgnXCDeriv", 0, s, ref errBuf) == 0)
                    dll_gmoIgnXCDeriv = d_gmoIgnXCDeriv;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoignxcderiv");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoIgnXCDeriv = (gmoIgnXCDeriv_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoIgnXCDeriv_t));
                    else
                    {
                        symName = "gmoIgnXCDeriv"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoIgnXCDerivSet", 1, s, ref errBuf) == 0)
                    dll_gmoIgnXCDerivSet = d_gmoIgnXCDerivSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoignxcderivset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoIgnXCDerivSet = (gmoIgnXCDerivSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoIgnXCDerivSet_t));
                    else
                    {
                        symName = "gmoIgnXCDerivSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoUseQ", 0, s, ref errBuf) == 0)
                    dll_gmoUseQ = d_gmoUseQ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmouseq");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoUseQ = (gmoUseQ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoUseQ_t));
                    else
                    {
                        symName = "gmoUseQ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoUseQSet", 1, s, ref errBuf) == 0)
                    dll_gmoUseQSet = d_gmoUseQSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmouseqset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoUseQSet = (gmoUseQSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoUseQSet_t));
                    else
                    {
                        symName = "gmoUseQSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoQExtractAlg", 0, s, ref errBuf) == 0)
                    dll_gmoQExtractAlg = d_gmoQExtractAlg;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoqextractalg");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoQExtractAlg = (gmoQExtractAlg_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoQExtractAlg_t));
                    else
                    {
                        symName = "gmoQExtractAlg"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoQExtractAlgSet", 1, s, ref errBuf) == 0)
                    dll_gmoQExtractAlgSet = d_gmoQExtractAlgSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoqextractalgset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoQExtractAlgSet = (gmoQExtractAlgSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoQExtractAlgSet_t));
                    else
                    {
                        symName = "gmoQExtractAlgSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoAltBounds", 0, s, ref errBuf) == 0)
                    dll_gmoAltBounds = d_gmoAltBounds;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoaltbounds");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoAltBounds = (gmoAltBounds_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoAltBounds_t));
                    else
                    {
                        symName = "gmoAltBounds"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoAltBoundsSet", 1, s, ref errBuf) == 0)
                    dll_gmoAltBoundsSet = d_gmoAltBoundsSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoaltboundsset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoAltBoundsSet = (gmoAltBoundsSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoAltBoundsSet_t));
                    else
                    {
                        symName = "gmoAltBoundsSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoAltRHS", 0, s, ref errBuf) == 0)
                    dll_gmoAltRHS = d_gmoAltRHS;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoaltrhs");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoAltRHS = (gmoAltRHS_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoAltRHS_t));
                    else
                    {
                        symName = "gmoAltRHS"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoAltRHSSet", 1, s, ref errBuf) == 0)
                    dll_gmoAltRHSSet = d_gmoAltRHSSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoaltrhsset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoAltRHSSet = (gmoAltRHSSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoAltRHSSet_t));
                    else
                    {
                        symName = "gmoAltRHSSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoAltVarTypes", 0, s, ref errBuf) == 0)
                    dll_gmoAltVarTypes = d_gmoAltVarTypes;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoaltvartypes");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoAltVarTypes = (gmoAltVarTypes_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoAltVarTypes_t));
                    else
                    {
                        symName = "gmoAltVarTypes"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoAltVarTypesSet", 1, s, ref errBuf) == 0)
                    dll_gmoAltVarTypesSet = d_gmoAltVarTypesSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoaltvartypesset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoAltVarTypesSet = (gmoAltVarTypesSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoAltVarTypesSet_t));
                    else
                    {
                        symName = "gmoAltVarTypesSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoForceLinear", 0, s, ref errBuf) == 0)
                    dll_gmoForceLinear = d_gmoForceLinear;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoforcelinear");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoForceLinear = (gmoForceLinear_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoForceLinear_t));
                    else
                    {
                        symName = "gmoForceLinear"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoForceLinearSet", 1, s, ref errBuf) == 0)
                    dll_gmoForceLinearSet = d_gmoForceLinearSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoforcelinearset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoForceLinearSet = (gmoForceLinearSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoForceLinearSet_t));
                    else
                    {
                        symName = "gmoForceLinearSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoForceCont", 0, s, ref errBuf) == 0)
                    dll_gmoForceCont = d_gmoForceCont;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoforcecont");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoForceCont = (gmoForceCont_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoForceCont_t));
                    else
                    {
                        symName = "gmoForceCont"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoForceContSet", 1, s, ref errBuf) == 0)
                    dll_gmoForceContSet = d_gmoForceContSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoforcecontset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoForceContSet = (gmoForceContSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoForceContSet_t));
                    else
                    {
                        symName = "gmoForceContSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoPermuteCols", 0, s, ref errBuf) == 0)
                    dll_gmoPermuteCols = d_gmoPermuteCols;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmopermutecols");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPermuteCols = (gmoPermuteCols_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPermuteCols_t));
                    else
                    {
                        symName = "gmoPermuteCols"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoPermuteColsSet", 1, s, ref errBuf) == 0)
                    dll_gmoPermuteColsSet = d_gmoPermuteColsSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmopermutecolsset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPermuteColsSet = (gmoPermuteColsSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPermuteColsSet_t));
                    else
                    {
                        symName = "gmoPermuteColsSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoPermuteRows", 0, s, ref errBuf) == 0)
                    dll_gmoPermuteRows = d_gmoPermuteRows;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmopermuterows");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPermuteRows = (gmoPermuteRows_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPermuteRows_t));
                    else
                    {
                        symName = "gmoPermuteRows"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoPermuteRowsSet", 1, s, ref errBuf) == 0)
                    dll_gmoPermuteRowsSet = d_gmoPermuteRowsSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmopermuterowsset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPermuteRowsSet = (gmoPermuteRowsSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPermuteRowsSet_t));
                    else
                    {
                        symName = "gmoPermuteRowsSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoPinf", 0, s, ref errBuf) == 0)
                    dll_gmoPinf = d_gmoPinf;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmopinf");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPinf = (gmoPinf_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPinf_t));
                    else
                    {
                        symName = "gmoPinf"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 13 };
                if (gmoxcheck("gmoPinfSet", 1, s, ref errBuf) == 0)
                    dll_gmoPinfSet = d_gmoPinfSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmopinfset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPinfSet = (gmoPinfSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPinfSet_t));
                    else
                    {
                        symName = "gmoPinfSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoMinf", 0, s, ref errBuf) == 0)
                    dll_gmoMinf = d_gmoMinf;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmominf");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoMinf = (gmoMinf_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoMinf_t));
                    else
                    {
                        symName = "gmoMinf"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 13 };
                if (gmoxcheck("gmoMinfSet", 1, s, ref errBuf) == 0)
                    dll_gmoMinfSet = d_gmoMinfSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmominfset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoMinfSet = (gmoMinfSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoMinfSet_t));
                    else
                    {
                        symName = "gmoMinfSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoQNaN", 0, s, ref errBuf) == 0)
                    dll_gmoQNaN = d_gmoQNaN;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoqnan");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoQNaN = (gmoQNaN_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoQNaN_t));
                    else
                    {
                        symName = "gmoQNaN"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoValNA", 0, s, ref errBuf) == 0)
                    dll_gmoValNA = d_gmoValNA;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmovalna");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoValNA = (gmoValNA_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoValNA_t));
                    else
                    {
                        symName = "gmoValNA"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoValNAInt", 0, s, ref errBuf) == 0)
                    dll_gmoValNAInt = d_gmoValNAInt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmovalnaint");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoValNAInt = (gmoValNAInt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoValNAInt_t));
                    else
                    {
                        symName = "gmoValNAInt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoValUndf", 0, s, ref errBuf) == 0)
                    dll_gmoValUndf = d_gmoValUndf;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmovalundf");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoValUndf = (gmoValUndf_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoValUndf_t));
                    else
                    {
                        symName = "gmoValUndf"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoM", 0, s, ref errBuf) == 0)
                    dll_gmoM = d_gmoM;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmom");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoM = (gmoM_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoM_t));
                    else
                    {
                        symName = "gmoM"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoQM", 0, s, ref errBuf) == 0)
                    dll_gmoQM = d_gmoQM;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoqm");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoQM = (gmoQM_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoQM_t));
                    else
                    {
                        symName = "gmoQM"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNLM", 0, s, ref errBuf) == 0)
                    dll_gmoNLM = d_gmoNLM;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonlm");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNLM = (gmoNLM_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNLM_t));
                    else
                    {
                        symName = "gmoNLM"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNRowMatch", 0, s, ref errBuf) == 0)
                    dll_gmoNRowMatch = d_gmoNRowMatch;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonrowmatch");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNRowMatch = (gmoNRowMatch_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNRowMatch_t));
                    else
                    {
                        symName = "gmoNRowMatch"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoN", 0, s, ref errBuf) == 0)
                    dll_gmoN = d_gmoN;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmon");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoN = (gmoN_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoN_t));
                    else
                    {
                        symName = "gmoN"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNLN", 0, s, ref errBuf) == 0)
                    dll_gmoNLN = d_gmoNLN;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonln");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNLN = (gmoNLN_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNLN_t));
                    else
                    {
                        symName = "gmoNLN"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNDisc", 0, s, ref errBuf) == 0)
                    dll_gmoNDisc = d_gmoNDisc;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmondisc");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNDisc = (gmoNDisc_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNDisc_t));
                    else
                    {
                        symName = "gmoNDisc"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNFixed", 0, s, ref errBuf) == 0)
                    dll_gmoNFixed = d_gmoNFixed;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonfixed");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNFixed = (gmoNFixed_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNFixed_t));
                    else
                    {
                        symName = "gmoNFixed"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNColMatch", 0, s, ref errBuf) == 0)
                    dll_gmoNColMatch = d_gmoNColMatch;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoncolmatch");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNColMatch = (gmoNColMatch_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNColMatch_t));
                    else
                    {
                        symName = "gmoNColMatch"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNZ", 0, s, ref errBuf) == 0)
                    dll_gmoNZ = d_gmoNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNZ = (gmoNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNZ_t));
                    else
                    {
                        symName = "gmoNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 23 };
                if (gmoxcheck("gmoNZ64", 0, s, ref errBuf) == 0)
                    dll_gmoNZ64 = d_gmoNZ64;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonz64");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNZ64 = (gmoNZ64_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNZ64_t));
                    else
                    {
                        symName = "gmoNZ64"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNLNZ", 0, s, ref errBuf) == 0)
                    dll_gmoNLNZ = d_gmoNLNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonlnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNLNZ = (gmoNLNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNLNZ_t));
                    else
                    {
                        symName = "gmoNLNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 23 };
                if (gmoxcheck("gmoNLNZ64", 0, s, ref errBuf) == 0)
                    dll_gmoNLNZ64 = d_gmoNLNZ64;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonlnz64");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNLNZ64 = (gmoNLNZ64_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNLNZ64_t));
                    else
                    {
                        symName = "gmoNLNZ64"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoLNZEx", 0, s, ref errBuf) == 0)
                    dll_gmoLNZEx = d_gmoLNZEx;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmolnzex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoLNZEx = (gmoLNZEx_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoLNZEx_t));
                    else
                    {
                        symName = "gmoLNZEx"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 23 };
                if (gmoxcheck("gmoLNZEx64", 0, s, ref errBuf) == 0)
                    dll_gmoLNZEx64 = d_gmoLNZEx64;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmolnzex64");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoLNZEx64 = (gmoLNZEx64_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoLNZEx64_t));
                    else
                    {
                        symName = "gmoLNZEx64"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoLNZ", 0, s, ref errBuf) == 0)
                    dll_gmoLNZ = d_gmoLNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmolnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoLNZ = (gmoLNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoLNZ_t));
                    else
                    {
                        symName = "gmoLNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 23 };
                if (gmoxcheck("gmoLNZ64", 0, s, ref errBuf) == 0)
                    dll_gmoLNZ64 = d_gmoLNZ64;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmolnz64");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoLNZ64 = (gmoLNZ64_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoLNZ64_t));
                    else
                    {
                        symName = "gmoLNZ64"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoQNZ", 0, s, ref errBuf) == 0)
                    dll_gmoQNZ = d_gmoQNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoqnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoQNZ = (gmoQNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoQNZ_t));
                    else
                    {
                        symName = "gmoQNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoGNLNZ", 0, s, ref errBuf) == 0)
                    dll_gmoGNLNZ = d_gmoGNLNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmognlnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGNLNZ = (gmoGNLNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGNLNZ_t));
                    else
                    {
                        symName = "gmoGNLNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoMaxQNZ", 0, s, ref errBuf) == 0)
                    dll_gmoMaxQNZ = d_gmoMaxQNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmomaxqnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoMaxQNZ = (gmoMaxQNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoMaxQNZ_t));
                    else
                    {
                        symName = "gmoMaxQNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 23 };
                if (gmoxcheck("gmoMaxQNZ64", 0, s, ref errBuf) == 0)
                    dll_gmoMaxQNZ64 = d_gmoMaxQNZ64;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmomaxqnz64");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoMaxQNZ64 = (gmoMaxQNZ64_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoMaxQNZ64_t));
                    else
                    {
                        symName = "gmoMaxQNZ64"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoObjNZ", 0, s, ref errBuf) == 0)
                    dll_gmoObjNZ = d_gmoObjNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjNZ = (gmoObjNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjNZ_t));
                    else
                    {
                        symName = "gmoObjNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoObjLNZ", 0, s, ref errBuf) == 0)
                    dll_gmoObjLNZ = d_gmoObjLNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjlnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjLNZ = (gmoObjLNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjLNZ_t));
                    else
                    {
                        symName = "gmoObjLNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoObjQNZEx", 0, s, ref errBuf) == 0)
                    dll_gmoObjQNZEx = d_gmoObjQNZEx;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjqnzex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjQNZEx = (gmoObjQNZEx_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjQNZEx_t));
                    else
                    {
                        symName = "gmoObjQNZEx"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoObjNLNZ", 0, s, ref errBuf) == 0)
                    dll_gmoObjNLNZ = d_gmoObjNLNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjnlnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjNLNZ = (gmoObjNLNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjNLNZ_t));
                    else
                    {
                        symName = "gmoObjNLNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoObjNLNZEx", 0, s, ref errBuf) == 0)
                    dll_gmoObjNLNZEx = d_gmoObjNLNZEx;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjnlnzex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjNLNZEx = (gmoObjNLNZEx_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjNLNZEx_t));
                    else
                    {
                        symName = "gmoObjNLNZEx"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoObjQMatNZ", 0, s, ref errBuf) == 0)
                    dll_gmoObjQMatNZ = d_gmoObjQMatNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjqmatnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjQMatNZ = (gmoObjQMatNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjQMatNZ_t));
                    else
                    {
                        symName = "gmoObjQMatNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 23 };
                if (gmoxcheck("gmoObjQMatNZ64", 0, s, ref errBuf) == 0)
                    dll_gmoObjQMatNZ64 = d_gmoObjQMatNZ64;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjqmatnz64");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjQMatNZ64 = (gmoObjQMatNZ64_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjQMatNZ64_t));
                    else
                    {
                        symName = "gmoObjQMatNZ64"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoObjQNZ", 0, s, ref errBuf) == 0)
                    dll_gmoObjQNZ = d_gmoObjQNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjqnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjQNZ = (gmoObjQNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjQNZ_t));
                    else
                    {
                        symName = "gmoObjQNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoObjQDiagNZ", 0, s, ref errBuf) == 0)
                    dll_gmoObjQDiagNZ = d_gmoObjQDiagNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjqdiagnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjQDiagNZ = (gmoObjQDiagNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjQDiagNZ_t));
                    else
                    {
                        symName = "gmoObjQDiagNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoObjCVecNZ", 0, s, ref errBuf) == 0)
                    dll_gmoObjCVecNZ = d_gmoObjCVecNZ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjcvecnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjCVecNZ = (gmoObjCVecNZ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjCVecNZ_t));
                    else
                    {
                        symName = "gmoObjCVecNZ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNLConst", 0, s, ref errBuf) == 0)
                    dll_gmoNLConst = d_gmoNLConst;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonlconst");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNLConst = (gmoNLConst_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNLConst_t));
                    else
                    {
                        symName = "gmoNLConst"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoNLConstSet", 1, s, ref errBuf) == 0)
                    dll_gmoNLConstSet = d_gmoNLConstSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonlconstset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNLConstSet = (gmoNLConstSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNLConstSet_t));
                    else
                    {
                        symName = "gmoNLConstSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNLCodeSize", 0, s, ref errBuf) == 0)
                    dll_gmoNLCodeSize = d_gmoNLCodeSize;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonlcodesize");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNLCodeSize = (gmoNLCodeSize_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNLCodeSize_t));
                    else
                    {
                        symName = "gmoNLCodeSize"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoNLCodeSizeSet", 1, s, ref errBuf) == 0)
                    dll_gmoNLCodeSizeSet = d_gmoNLCodeSizeSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonlcodesizeset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNLCodeSizeSet = (gmoNLCodeSizeSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNLCodeSizeSet_t));
                    else
                    {
                        symName = "gmoNLCodeSizeSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNLCodeSizeMaxRow", 0, s, ref errBuf) == 0)
                    dll_gmoNLCodeSizeMaxRow = d_gmoNLCodeSizeMaxRow;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonlcodesizemaxrow");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNLCodeSizeMaxRow = (gmoNLCodeSizeMaxRow_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNLCodeSizeMaxRow_t));
                    else
                    {
                        symName = "gmoNLCodeSizeMaxRow"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoObjVar", 0, s, ref errBuf) == 0)
                    dll_gmoObjVar = d_gmoObjVar;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjvar");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjVar = (gmoObjVar_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjVar_t));
                    else
                    {
                        symName = "gmoObjVar"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoObjVarSet", 1, s, ref errBuf) == 0)
                    dll_gmoObjVarSet = d_gmoObjVarSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjvarset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjVarSet = (gmoObjVarSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjVarSet_t));
                    else
                    {
                        symName = "gmoObjVarSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoObjRow", 0, s, ref errBuf) == 0)
                    dll_gmoObjRow = d_gmoObjRow;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjrow");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjRow = (gmoObjRow_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjRow_t));
                    else
                    {
                        symName = "gmoObjRow"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoGetObjOrder", 0, s, ref errBuf) == 0)
                    dll_gmoGetObjOrder = d_gmoGetObjOrder;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmogetobjorder");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoGetObjOrder = (gmoGetObjOrder_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoGetObjOrder_t));
                    else
                    {
                        symName = "gmoGetObjOrder"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoObjConst", 0, s, ref errBuf) == 0)
                    dll_gmoObjConst = d_gmoObjConst;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjconst");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjConst = (gmoObjConst_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjConst_t));
                    else
                    {
                        symName = "gmoObjConst"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoObjConstEx", 0, s, ref errBuf) == 0)
                    dll_gmoObjConstEx = d_gmoObjConstEx;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjconstex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjConstEx = (gmoObjConstEx_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjConstEx_t));
                    else
                    {
                        symName = "gmoObjConstEx"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoObjQConst", 0, s, ref errBuf) == 0)
                    dll_gmoObjQConst = d_gmoObjQConst;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjqconst");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjQConst = (gmoObjQConst_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjQConst_t));
                    else
                    {
                        symName = "gmoObjQConst"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoObjJacVal", 0, s, ref errBuf) == 0)
                    dll_gmoObjJacVal = d_gmoObjJacVal;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoobjjacval");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoObjJacVal = (gmoObjJacVal_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoObjJacVal_t));
                    else
                    {
                        symName = "gmoObjJacVal"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoEvalErrorMethod", 0, s, ref errBuf) == 0)
                    dll_gmoEvalErrorMethod = d_gmoEvalErrorMethod;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalerrormethod");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalErrorMethod = (gmoEvalErrorMethod_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalErrorMethod_t));
                    else
                    {
                        symName = "gmoEvalErrorMethod"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoEvalErrorMethodSet", 1, s, ref errBuf) == 0)
                    dll_gmoEvalErrorMethodSet = d_gmoEvalErrorMethodSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalerrormethodset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalErrorMethodSet = (gmoEvalErrorMethodSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalErrorMethodSet_t));
                    else
                    {
                        symName = "gmoEvalErrorMethodSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoEvalMaxThreads", 0, s, ref errBuf) == 0)
                    dll_gmoEvalMaxThreads = d_gmoEvalMaxThreads;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalmaxthreads");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalMaxThreads = (gmoEvalMaxThreads_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalMaxThreads_t));
                    else
                    {
                        symName = "gmoEvalMaxThreads"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoEvalMaxThreadsSet", 1, s, ref errBuf) == 0)
                    dll_gmoEvalMaxThreadsSet = d_gmoEvalMaxThreadsSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalmaxthreadsset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalMaxThreadsSet = (gmoEvalMaxThreadsSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalMaxThreadsSet_t));
                    else
                    {
                        symName = "gmoEvalMaxThreadsSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoEvalFuncCount", 0, s, ref errBuf) == 0)
                    dll_gmoEvalFuncCount = d_gmoEvalFuncCount;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfunccount");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFuncCount = (gmoEvalFuncCount_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFuncCount_t));
                    else
                    {
                        symName = "gmoEvalFuncCount"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoEvalFuncTimeUsed", 0, s, ref errBuf) == 0)
                    dll_gmoEvalFuncTimeUsed = d_gmoEvalFuncTimeUsed;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalfunctimeused");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalFuncTimeUsed = (gmoEvalFuncTimeUsed_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalFuncTimeUsed_t));
                    else
                    {
                        symName = "gmoEvalFuncTimeUsed"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoEvalGradCount", 0, s, ref errBuf) == 0)
                    dll_gmoEvalGradCount = d_gmoEvalGradCount;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalgradcount");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalGradCount = (gmoEvalGradCount_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalGradCount_t));
                    else
                    {
                        symName = "gmoEvalGradCount"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gmoxcheck("gmoEvalGradTimeUsed", 0, s, ref errBuf) == 0)
                    dll_gmoEvalGradTimeUsed = d_gmoEvalGradTimeUsed;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoevalgradtimeused");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoEvalGradTimeUsed = (gmoEvalGradTimeUsed_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoEvalGradTimeUsed_t));
                    else
                    {
                        symName = "gmoEvalGradTimeUsed"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoHessMaxDim", 0, s, ref errBuf) == 0)
                    dll_gmoHessMaxDim = d_gmoHessMaxDim;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohessmaxdim");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessMaxDim = (gmoHessMaxDim_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessMaxDim_t));
                    else
                    {
                        symName = "gmoHessMaxDim"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoHessMaxNz", 0, s, ref errBuf) == 0)
                    dll_gmoHessMaxNz = d_gmoHessMaxNz;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohessmaxnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessMaxNz = (gmoHessMaxNz_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessMaxNz_t));
                    else
                    {
                        symName = "gmoHessMaxNz"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoHessLagDim", 0, s, ref errBuf) == 0)
                    dll_gmoHessLagDim = d_gmoHessLagDim;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohesslagdim");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessLagDim = (gmoHessLagDim_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessLagDim_t));
                    else
                    {
                        symName = "gmoHessLagDim"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoHessLagNz", 0, s, ref errBuf) == 0)
                    dll_gmoHessLagNz = d_gmoHessLagNz;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohesslagnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessLagNz = (gmoHessLagNz_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessLagNz_t));
                    else
                    {
                        symName = "gmoHessLagNz"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoHessLagDiagNz", 0, s, ref errBuf) == 0)
                    dll_gmoHessLagDiagNz = d_gmoHessLagDiagNz;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohesslagdiagnz");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessLagDiagNz = (gmoHessLagDiagNz_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessLagDiagNz_t));
                    else
                    {
                        symName = "gmoHessLagDiagNz"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gmoxcheck("gmoHessInclQRows", 0, s, ref errBuf) == 0)
                    dll_gmoHessInclQRows = d_gmoHessInclQRows;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohessinclqrows");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessInclQRows = (gmoHessInclQRows_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessInclQRows_t));
                    else
                    {
                        symName = "gmoHessInclQRows"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 15 };
                if (gmoxcheck("gmoHessInclQRowsSet", 1, s, ref errBuf) == 0)
                    dll_gmoHessInclQRowsSet = d_gmoHessInclQRowsSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmohessinclqrowsset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoHessInclQRowsSet = (gmoHessInclQRowsSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoHessInclQRowsSet_t));
                    else
                    {
                        symName = "gmoHessInclQRowsSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNumVIFunc", 0, s, ref errBuf) == 0)
                    dll_gmoNumVIFunc = d_gmoNumVIFunc;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonumvifunc");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNumVIFunc = (gmoNumVIFunc_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNumVIFunc_t));
                    else
                    {
                        symName = "gmoNumVIFunc"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoNumAgents", 0, s, ref errBuf) == 0)
                    dll_gmoNumAgents = d_gmoNumAgents;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmonumagents");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNumAgents = (gmoNumAgents_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNumAgents_t));
                    else
                    {
                        symName = "gmoNumAgents"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12 };
                if (gmoxcheck("gmoNameOptFile", 0, s, ref errBuf) == 0)
                    dll_gmoNameOptFile = d_gmoNameOptFile;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonameoptfile");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameOptFile = (gmoNameOptFile_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameOptFile_t));
                    else
                    {
                        symName = "cgmoNameOptFile"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 12 };
                if (gmoxcheck("gmoNameOptFileSet", 1, s, ref errBuf) == 0)
                    dll_gmoNameOptFileSet = d_gmoNameOptFileSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonameoptfileset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameOptFileSet = (gmoNameOptFileSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameOptFileSet_t));
                    else
                    {
                        symName = "cgmoNameOptFileSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12 };
                if (gmoxcheck("gmoNameSolFile", 0, s, ref errBuf) == 0)
                    dll_gmoNameSolFile = d_gmoNameSolFile;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonamesolfile");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameSolFile = (gmoNameSolFile_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameSolFile_t));
                    else
                    {
                        symName = "cgmoNameSolFile"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 12 };
                if (gmoxcheck("gmoNameSolFileSet", 1, s, ref errBuf) == 0)
                    dll_gmoNameSolFileSet = d_gmoNameSolFileSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonamesolfileset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameSolFileSet = (gmoNameSolFileSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameSolFileSet_t));
                    else
                    {
                        symName = "cgmoNameSolFileSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12 };
                if (gmoxcheck("gmoNameXLib", 0, s, ref errBuf) == 0)
                    dll_gmoNameXLib = d_gmoNameXLib;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonamexlib");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameXLib = (gmoNameXLib_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameXLib_t));
                    else
                    {
                        symName = "cgmoNameXLib"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 12 };
                if (gmoxcheck("gmoNameXLibSet", 1, s, ref errBuf) == 0)
                    dll_gmoNameXLibSet = d_gmoNameXLibSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonamexlibset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameXLibSet = (gmoNameXLibSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameXLibSet_t));
                    else
                    {
                        symName = "cgmoNameXLibSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12 };
                if (gmoxcheck("gmoNameMatrix", 0, s, ref errBuf) == 0)
                    dll_gmoNameMatrix = d_gmoNameMatrix;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonamematrix");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameMatrix = (gmoNameMatrix_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameMatrix_t));
                    else
                    {
                        symName = "cgmoNameMatrix"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12 };
                if (gmoxcheck("gmoNameDict", 0, s, ref errBuf) == 0)
                    dll_gmoNameDict = d_gmoNameDict;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonamedict");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameDict = (gmoNameDict_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameDict_t));
                    else
                    {
                        symName = "cgmoNameDict"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 12 };
                if (gmoxcheck("gmoNameDictSet", 1, s, ref errBuf) == 0)
                    dll_gmoNameDictSet = d_gmoNameDictSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonamedictset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameDictSet = (gmoNameDictSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameDictSet_t));
                    else
                    {
                        symName = "cgmoNameDictSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12 };
                if (gmoxcheck("gmoNameInput", 0, s, ref errBuf) == 0)
                    dll_gmoNameInput = d_gmoNameInput;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonameinput");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameInput = (gmoNameInput_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameInput_t));
                    else
                    {
                        symName = "cgmoNameInput"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 12 };
                if (gmoxcheck("gmoNameInputSet", 1, s, ref errBuf) == 0)
                    dll_gmoNameInputSet = d_gmoNameInputSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonameinputset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameInputSet = (gmoNameInputSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameInputSet_t));
                    else
                    {
                        symName = "cgmoNameInputSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12 };
                if (gmoxcheck("gmoNameOutput", 0, s, ref errBuf) == 0)
                    dll_gmoNameOutput = d_gmoNameOutput;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmonameoutput");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoNameOutput = (gmoNameOutput_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoNameOutput_t));
                    else
                    {
                        symName = "cgmoNameOutput"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 1 };
                if (gmoxcheck("gmoPPool", 0, s, ref errBuf) == 0)
                    dll_gmoPPool = d_gmoPPool;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoppool");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoPPool = (gmoPPool_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoPPool_t));
                    else
                    {
                        symName = "gmoPPool"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 1 };
                if (gmoxcheck("gmoIOMutex", 0, s, ref errBuf) == 0)
                    dll_gmoIOMutex = d_gmoIOMutex;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoiomutex");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoIOMutex = (gmoIOMutex_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoIOMutex_t));
                    else
                    {
                        symName = "gmoIOMutex"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gmoxcheck("gmoError", 0, s, ref errBuf) == 0)
                    dll_gmoError = d_gmoError;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoerror");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoError = (gmoError_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoError_t));
                    else
                    {
                        symName = "gmoError"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3 };
                if (gmoxcheck("gmoErrorSet", 1, s, ref errBuf) == 0)
                    dll_gmoErrorSet = d_gmoErrorSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gmoerrorset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoErrorSet = (gmoErrorSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoErrorSet_t));
                    else
                    {
                        symName = "gmoErrorSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12 };
                if (gmoxcheck("gmoErrorMessage", 0, s, ref errBuf) == 0)
                    dll_gmoErrorMessage = d_gmoErrorMessage;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgmoerrormessage");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gmoErrorMessage = (gmoErrorMessage_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gmoErrorMessage_t));
                    else
                    {
                        symName = "cgmoErrorMessage"; goto symMissing;
                    }
                }
            }

            return true;

        symMissing:
            errBuf = "Could not load symbol '" + symName + "'";
            return false;

        } /* XLibraryLoad */

        private bool libloader(string dllPath, string dllName, ref string msgBuf)
        {
#if __MonoCS__ || __APPLE__
#if __APPLE__
        const string libStem = "libjoatdclib", libExt = ".dylib";
#else
        const string libStem = "libjoatdclib", libExt = ".so";
#endif
#else
            const string libStem = "joatdclib", libExt = ".dll";
#endif
            string dllNameBuf = string.Empty;
            int myrc = 0;
            string GMS_DLL_SUFFIX = string.Empty;

            msgBuf = string.Empty;
            if (!isLoaded)
            {
                if (string.Empty != dllPath)
                {
                    dllNameBuf = dllPath;
                    if (Path.DirectorySeparatorChar != dllNameBuf[dllNameBuf.Length - 1]) dllNameBuf = dllNameBuf + Path.DirectorySeparatorChar;
                }
                if (string.Empty != dllName)
                    dllNameBuf = dllNameBuf + dllName;
                else
                {
                    if (8 == IntPtr.Size)
                        GMS_DLL_SUFFIX = "64";
                    dllNameBuf = dllNameBuf + libStem + GMS_DLL_SUFFIX + libExt;
                }
                isLoaded = XLibraryLoad(dllNameBuf, ref msgBuf);
                if (isLoaded)
                {
                }
                else                          /* library load failed */
                    myrc |= 1;
            }
            return (myrc & 1) == 0;
        } /* libloader */

        public bool gmoGetReady(ref string msgBuf)
        {
            return libloader(string.Empty, string.Empty, ref msgBuf);
        }
        public bool gmoGetReadyD(string dirName, ref string msgBuf)
        {
            return libloader(dirName, string.Empty, ref msgBuf);
        }
        public bool gmoGetReadyL(string dirName, string libName, ref string msgBuf)
        {
            return libloader(dirName, libName, ref msgBuf);
        }

        public gmomcs(ref string msgBuf)
        {
            bool gmoIsReady;

            extHandle = false;
            _disposed = false;
            gmoIsReady = gmoGetReady(ref msgBuf);
            if (!gmoIsReady) return;
            gmoxcreate(ref pgmo);
            if (pgmo != IntPtr.Zero)
            {
                msgBuf = String.Empty;
                return;
            }
            msgBuf = "Error while creating object";
        }
        public gmomcs(string dirName, ref string msgBuf, bool passDN = false)
        {
            bool gmoIsReady;

            extHandle = false;
            _disposed = false;
            gmoIsReady = gmoGetReadyD(dirName, ref msgBuf);
            if (!gmoIsReady) return;
            if (passDN)
                gmoxcreated(ref pgmo, dirName);
            else
                gmoxcreate(ref pgmo);
            if (pgmo != IntPtr.Zero)
            {
                msgBuf = String.Empty;
                return;
            }
            msgBuf = "Error while creating object";
        }
        public gmomcs(string dirName, string libName, ref string msgBuf, bool passDN = false)
        {
            bool gmoIsReady;

            extHandle = false;
            _disposed = false;
            gmoIsReady = gmoGetReadyL(dirName, libName, ref msgBuf);
            if (!gmoIsReady) return;
            if (passDN)
                gmoxcreated(ref pgmo, dirName);
            else
                gmoxcreate(ref pgmo);
            if (pgmo != IntPtr.Zero)
            {
                msgBuf = String.Empty;
                return;
            }
            msgBuf = "Error while creating object";
        }
        public gmomcs(IntPtr gmoHandle, ref string msgBuf)
        {
            bool gmoIsReady;

            if (gmoHandle == IntPtr.Zero)
            {
                msgBuf = "gmoHandle is empty";
                return;
            }
            extHandle = true;
            _disposed = false;
            gmoIsReady = gmoGetReady(ref msgBuf);
            if (!gmoIsReady) return;
            pgmo = gmoHandle;
        }
        public gmomcs(IntPtr gmoHandle, string dirName, ref string msgBuf)
        {
            bool gmoIsReady;

            if (gmoHandle == IntPtr.Zero)
            {
                msgBuf = "gmoHandle is empty";
                return;
            }
            extHandle = true;
            _disposed = false;
            gmoIsReady = gmoGetReadyD(dirName, ref msgBuf);
            if (!gmoIsReady) return;
            pgmo = gmoHandle;
        }

        ~gmomcs()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (pgmo != IntPtr.Zero)
                        gmoFree();
                }
                // Indicate that the instance has been disposed.
                _disposed = true;
            }
            GC.KeepAlive(this);
        }

        public int gmoFree()
        {
            if (!extHandle && pgmo != IntPtr.Zero) gmoxfree(ref pgmo);
            return 1;
        }

        public bool gmoLibraryUnload()
        {
            return FreeLibrary(h);
        }

        public IntPtr GetgmoPtr()
        {
            return pgmo;
        }

        public bool gmoGetScreenIndicator()
        {
            return ScreenIndicator;
        }

        public void gmoSetScreenIndicator(bool scrind)
        {
            ScreenIndicator = scrind;
        }

        public bool gmoGetExceptionIndicator()
        {
            return ExceptionIndicator;
        }

        public void gmoSetExceptionIndicator(bool excind)
        {
            ExceptionIndicator = excind;
        }

        public bool gmoGetExitIndicator()
        {
            return ExitIndicator;
        }

        public void gmoSetExitIndicator(bool extind)
        {
            ExitIndicator = extind;
        }

        public gmoErrorCallback_t gmoGetErrorCallback()
        {
            return ErrorCallBack;
        }

        public void gmoSetErrorCallback(gmoErrorCallback_t func)
        {
            ErrorCallBack = func;
        }

        public int gmoGetAPIErrorCount()
        {
            return APIErrorCount;
        }

        public void gmoSetAPIErrorCount(int ecnt)
        {
            APIErrorCount = ecnt;
        }

        private static void gmoErrorHandling(string Msg)
        {
            APIErrorCount++;
            if (ScreenIndicator) Console.WriteLine(Msg);
            if (ErrorCallBack != null)
                if (ErrorCallBack(APIErrorCount, Msg)) Environment.Exit(123);
            if (ExceptionIndicator) throw new ArgumentNullException();
            if (ExitIndicator) Environment.Exit(123);
        }

        private void ConvertC2CS(byte[] b, ref string s)
        {
            int i;
            s = "";
            i = 0;
            while (b[i] != 0)
            {
                s = s + (char)(b[i]);
                i = i + 1;
            }
        }

        private int gmoxapiversion(int api, ref string msg, ref int cl)
        {
            int rc_gmoxapiversion;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_gmoxapiversion = dll_gmoxapiversion(api, cpy_msg, ref cl);
            msg = cpy_msg.ToString();
            return rc_gmoxapiversion;
        }

        private int gmoxcheck(string ep, int nargs, int[] s, ref string msg)
        {
            int rc_gmoxcheck;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_gmoxcheck = dll_gmoxcheck(ep, nargs, s, cpy_msg);
            msg = cpy_msg.ToString();
            return rc_gmoxcheck;
        }
        /// <summary>
        /// Initialize GMO data
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="cols">Number of columns</param>
        /// <param name="codelen">length of NL code</param>
        public int gmoInitData(int rows, int cols, int codelen)
        {
            return dll_gmoInitData(pgmo, rows, cols, codelen);
        }
        /// <summary>
        /// Add a row
        /// </summary>
        /// <param name="etyp">Type of equation (see enumerated constants)</param>
        /// <param name="ematch">Index of matching variable of equation</param>
        /// <param name="eslack">Slack of equation</param>
        /// <param name="escale">Scale of equation</param>
        /// <param name="erhs">RHS of equation</param>
        /// <param name="emarg">Marginal of equation</param>
        /// <param name="ebas">Basis flag of equation (0=basic)</param>
        /// <param name="enz">Number of nonzeros in row</param>
        /// <param name="colidx">Column index/indices of Jacobian(s)</param>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        /// <param name="nlflag">NL flag(s) of Jacobian(s) (0 : linear,!=0 : nonlinear)</param>
        public int gmoAddRow(int etyp, int ematch, double eslack, double escale, double erhs, double emarg, int ebas, int enz, int[] colidx, double[] jacval, int[] nlflag)
        {
            return dll_gmoAddRow(pgmo, etyp, ematch, eslack, escale, erhs, emarg, ebas, enz, colidx, jacval, nlflag);
        }
        /// <summary>
        /// Add a column
        /// </summary>
        /// <param name="vtyp">Type of variable (see enumerated constants)</param>
        /// <param name="vlo">Lower bound of variable</param>
        /// <param name="vl">Level of variable</param>
        /// <param name="vup">Upper bound of variable</param>
        /// <param name="vmarg">Marginal of variable</param>
        /// <param name="vbas">Basis flag of variable (0=basic)</param>
        /// <param name="vsos">SOS set variable belongs to</param>
        /// <param name="vprior">riority value of variable</param>
        /// <param name="vscale">Scale of variable</param>
        /// <param name="vnz">Number of nonzeros in column</param>
        /// <param name="rowidx">Row index/indices of Jacobian</param>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        /// <param name="nlflag">NL flag(s) of Jacobian(s) (0 : linear,!=0 : nonlinear)</param>
        public int gmoAddCol(int vtyp, double vlo, double vl, double vup, double vmarg, int vbas, int vsos, double vprior, double vscale, int vnz, int[] rowidx, double[] jacval, int[] nlflag)
        {
            return dll_gmoAddCol(pgmo, vtyp, vlo, vl, vup, vmarg, vbas, vsos, vprior, vscale, vnz, rowidx, jacval, nlflag);
        }
        /// <summary>
        /// Complete GMO data instance
        /// </summary>
        /// <param name="msg">Message</param>
        public int gmoCompleteData(ref string msg)
        {
            int rc_gmoCompleteData;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_gmoCompleteData = dll_gmoCompleteData(pgmo, cpy_msg);
            msg = cpy_msg.ToString();
            return rc_gmoCompleteData;
        }
        /// <summary>
        /// Read instance from scratch files - Legacy Mode
        /// </summary>
        /// <param name="msg">Message</param>
        public int gmoLoadDataLegacy(ref string msg)
        {
            int rc_gmoLoadDataLegacy;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_gmoLoadDataLegacy = dll_gmoLoadDataLegacy(pgmo, cpy_msg);
            msg = cpy_msg.ToString();
            return rc_gmoLoadDataLegacy;
        }
        /// <summary>
        /// Register GAMS environment
        /// </summary>
        /// <param name="gevptr"></param>
        /// <param name="msg">Message</param>
        public int gmoRegisterEnvironment(IntPtr gevptr, ref string msg)
        {
            int rc_gmoRegisterEnvironment;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_gmoRegisterEnvironment = dll_gmoRegisterEnvironment(pgmo, gevptr, cpy_msg);
            msg = cpy_msg.ToString();
            return rc_gmoRegisterEnvironment;
        }
        /// <summary>
        /// Get GAMS environment object pointer
        /// </summary>
        public IntPtr gmoEnvironment()
        {
            return dll_gmoEnvironment(pgmo);
        }
        /// <summary>
        /// Store current view in view object
        /// </summary>
        public IntPtr gmoViewStore()
        {
            return dll_gmoViewStore(pgmo);
        }
        /// <summary>
        /// Restore view
        /// </summary>
        /// <param name="viewptr">Pointer to structure storing the view of a model</param>
        public void gmoViewRestore(ref IntPtr viewptr)
        {
            dll_gmoViewRestore(pgmo, ref viewptr);
        }
        /// <summary>
        /// Dump current view to stdout
        /// </summary>
        public void gmoViewDump()
        {
            dll_gmoViewDump(pgmo);
        }
        /// <summary>
        /// Get equation index in solver space
        /// </summary>
        /// <param name="mi">Index of row in original/GAMS space</param>
        public int gmoGetiSolver(int mi)
        {
            return dll_gmoGetiSolver(pgmo, mi);
        }
        /// <summary>
        /// Get variable index in solver space
        /// </summary>
        /// <param name="mj">Index of column original/GAMS client space</param>
        public int gmoGetjSolver(int mj)
        {
            return dll_gmoGetjSolver(pgmo, mj);
        }
        /// <summary>
        /// Get equation index in solver space (without error message; negative if it fails)
        /// </summary>
        /// <param name="mi">Index of row in original/GAMS space</param>
        public int gmoGetiSolverQuiet(int mi)
        {
            return dll_gmoGetiSolverQuiet(pgmo, mi);
        }
        /// <summary>
        /// Get variable index in solver space (without error message; negative if it fails)
        /// </summary>
        /// <param name="mj">Index of column original/GAMS client space</param>
        public int gmoGetjSolverQuiet(int mj)
        {
            return dll_gmoGetjSolverQuiet(pgmo, mj);
        }
        /// <summary>
        /// Get equation index in model (original) space
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public int gmoGetiModel(int si)
        {
            return dll_gmoGetiModel(pgmo, si);
        }
        /// <summary>
        /// Get variable index in model (original) space
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public int gmoGetjModel(int sj)
        {
            return dll_gmoGetjModel(pgmo, sj);
        }
        /// <summary>
        /// Set Permutation vectors for equations (model view)
        /// </summary>
        /// <param name="permut">Permutation vector (original/GAMS to client)</param>
        public int gmoSetEquPermutation(ref int[] permut)
        {
            return dll_gmoSetEquPermutation(pgmo, permut);
        }
        /// <summary>
        /// Set Permutation vectors for equations (solver view)
        /// </summary>
        /// <param name="rvpermut">Reverse permutation vector (client to original/GAMS)</param>
        /// <param name="len">Length of array</param>
        public int gmoSetRvEquPermutation(ref int[] rvpermut, int len)
        {
            return dll_gmoSetRvEquPermutation(pgmo, rvpermut, len);
        }
        /// <summary>
        /// Set Permutation vectors for variables (model view)
        /// </summary>
        /// <param name="permut">Permutation vector (original/GAMS to client)</param>
        public int gmoSetVarPermutation(ref int[] permut)
        {
            return dll_gmoSetVarPermutation(pgmo, permut);
        }
        /// <summary>
        /// Set Permutation vectors for variables (solver view)
        /// </summary>
        /// <param name="rvpermut">Reverse permutation vector (client to original/GAMS)</param>
        /// <param name="len">Length of array</param>
        public int gmoSetRvVarPermutation(ref int[] rvpermut, int len)
        {
            return dll_gmoSetRvVarPermutation(pgmo, rvpermut, len);
        }
        /// <summary>
        /// Set Permutation to skip =n= rows
        /// </summary>
        public int gmoSetNRowPerm()
        {
            return dll_gmoSetNRowPerm(pgmo);
        }
        /// <summary>
        /// Get variable type count
        /// </summary>
        /// <param name="vtyp">Type of variable (see enumerated constants)</param>
        public int gmoGetVarTypeCnt(int vtyp)
        {
            return dll_gmoGetVarTypeCnt(pgmo, vtyp);
        }
        /// <summary>
        /// Get equation type count
        /// </summary>
        /// <param name="etyp">Type of equation (see enumerated constants)</param>
        public int gmoGetEquTypeCnt(int etyp)
        {
            return dll_gmoGetEquTypeCnt(pgmo, etyp);
        }
        /// <summary>
        /// Get obj counts
        /// </summary>
        /// <param name="nz">Number of nonzeros</param>
        /// <param name="qnz">Number of quadratic nonzeros in Jacobian matrix</param>
        /// <param name="nlnz">Number of nonlinear nonzeros</param>
        public int gmoGetObjStat(ref int nz, ref int qnz, ref int nlnz)
        {
            return dll_gmoGetObjStat(pgmo, ref nz, ref qnz, ref nlnz);
        }
        /// <summary>
        /// Get row counts
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="nz">Number of nonzeros</param>
        /// <param name="qnz">Number of quadratic nonzeros in Jacobian matrix</param>
        /// <param name="nlnz">Number of nonlinear nonzeros</param>
        public int gmoGetRowStat(int si, ref int nz, ref int qnz, ref int nlnz)
        {
            return dll_gmoGetRowStat(pgmo, si, ref nz, ref qnz, ref nlnz);
        }
        /// <summary>
        /// Get Jacobian row NZ counts: total and by GMOORDER_XX
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="nz">Number of nonzeros</param>
        /// <param name="lnz"></param>
        /// <param name="qnz">Number of quadratic nonzeros in Jacobian matrix</param>
        /// <param name="nlnz">Number of nonlinear nonzeros</param>
        public int gmoGetRowStatEx(int si, ref int nz, ref int lnz, ref int qnz, ref int nlnz)
        {
            return dll_gmoGetRowStatEx(pgmo, si, ref nz, ref lnz, ref qnz, ref nlnz);
        }
        /// <summary>
        /// Get column counts objnz = -1 if linear +1 if non-linear 0 otherwise
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="nz">Number of nonzeros</param>
        /// <param name="qnz">Number of quadratic nonzeros in Jacobian matrix</param>
        /// <param name="nlnz">Number of nonlinear nonzeros</param>
        /// <param name="objnz">Nonzeros in objective</param>
        public int gmoGetColStat(int sj, ref int nz, ref int qnz, ref int nlnz, ref int objnz)
        {
            return dll_gmoGetColStat(pgmo, sj, ref nz, ref qnz, ref nlnz, ref objnz);
        }
        /// <summary>
        /// Number of NZ in Q matrix of row si (-1 if Q information not used or overflow)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public int gmoGetRowQNZOne(int si)
        {
            return dll_gmoGetRowQNZOne(pgmo, si);
        }
        /// <summary>
        /// Number of NZ in Q matrix of row si (-1 if Q information not used)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public Int64 gmoGetRowQNZOne64(int si)
        {
            return dll_gmoGetRowQNZOne64(pgmo, si);
        }
        /// <summary>
        /// Number of NZ on diagonal of Q matrix of row si (-1 if Q information not used)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public int gmoGetRowQDiagNZOne(int si)
        {
            return dll_gmoGetRowQDiagNZOne(pgmo, si);
        }
        /// <summary>
        /// Number of NZ in c vector of row si (-1 if Q information not used)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public int gmoGetRowCVecNZOne(int si)
        {
            return dll_gmoGetRowCVecNZOne(pgmo, si);
        }
        /// <summary>
        /// Get SOS count information
        /// </summary>
        /// <param name="numsos1">Number of SOS1 sets</param>
        /// <param name="numsos2">Number of SOS2 sets</param>
        /// <param name="nzsos">Number of variables in SOS1/2 sets</param>
        public void gmoGetSosCounts(ref int numsos1, ref int numsos2, ref int nzsos)
        {
            dll_gmoGetSosCounts(pgmo, ref numsos1, ref numsos2, ref nzsos);
        }
        /// <summary>
        /// Get external function information
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="cols">Number of columns</param>
        /// <param name="nz">Number of nonzeros</param>
        /// <param name="orgcolind"></param>
        public void gmoGetXLibCounts(ref int rows, ref int cols, ref int nz, ref int[] orgcolind)
        {
            dll_gmoGetXLibCounts(pgmo, ref rows, ref cols, ref nz, orgcolind);
        }
        /// <summary>
        /// Get model type in case of scenario solve generated models
        /// </summary>
        /// <param name="checkv">a vector with column indicators to be treated as constant</param>
        /// <param name="actModelType">active model type in case of scenario dict type emp model</param>
        public int gmoGetActiveModelType(ref int[] checkv, ref int actModelType)
        {
            return dll_gmoGetActiveModelType(pgmo, checkv, ref actModelType);
        }
        /// <summary>
        /// Get constraint matrix in row order with row start only and NL indicator
        /// </summary>
        /// <param name="rowstart">Index of Jacobian row starts with</param>
        /// <param name="colidx">Column index/indices of Jacobian(s)</param>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        /// <param name="nlflag">NL flag(s) of Jacobian(s) (0 : linear,!=0 : nonlinear)</param>
        public int gmoGetMatrixRow(ref int[] rowstart, ref int[] colidx, ref double[] jacval, ref int[] nlflag)
        {
            return dll_gmoGetMatrixRow(pgmo, rowstart, colidx, jacval, nlflag);
        }
        /// <summary>
        /// Get constraint matrix in column order with columns start only and NL indicator
        /// </summary>
        /// <param name="colstart">Index of Jacobian column starts with</param>
        /// <param name="rowidx">Row index/indices of Jacobian</param>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        /// <param name="nlflag">NL flag(s) of Jacobian(s) (0 : linear,!=0 : nonlinear)</param>
        public int gmoGetMatrixCol(ref int[] colstart, ref int[] rowidx, ref double[] jacval, ref int[] nlflag)
        {
            return dll_gmoGetMatrixCol(pgmo, colstart, rowidx, jacval, nlflag);
        }
        /// <summary>
        /// Get constraint matrix in column order with column start and end (colstart length is n+1)
        /// </summary>
        /// <param name="colstart">Index of Jacobian column starts with</param>
        /// <param name="collength">Number of Jacobians in column</param>
        /// <param name="rowidx">Row index/indices of Jacobian</param>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        public int gmoGetMatrixCplex(ref int[] colstart, ref int[] collength, ref int[] rowidx, ref double[] jacval)
        {
            return dll_gmoGetMatrixCplex(pgmo, colstart, collength, rowidx, jacval);
        }
        /// <summary>
        /// Get name of objective
        /// </summary>
        public string gmoGetObjName()
        {
            string rc_gmoGetObjName = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoGetObjName(pgmo, sst_result);
            rc_gmoGetObjName = sst_result.ToString();
            return rc_gmoGetObjName;
        }
        /// <summary>
        /// Get name of objective with user specified suffix
        /// </summary>
        /// <param name="suffix">Suffix appended to name, could be .l, .m etc.</param>
        public string gmoGetObjNameCustom(string suffix)
        {
            string rc_gmoGetObjNameCustom = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoGetObjNameCustom(pgmo, suffix, sst_result);
            rc_gmoGetObjNameCustom = sst_result.ToString();
            return rc_gmoGetObjNameCustom;
        }
        /// <summary>
        /// Get objective function vector (dense)
        /// </summary>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        /// <param name="nlflag">NL flag(s) of Jacobian(s) (0 : linear,!=0 : nonlinear)</param>
        public int gmoGetObjVector(ref double[] jacval, ref int[] nlflag)
        {
            return dll_gmoGetObjVector(pgmo, jacval, nlflag);
        }
        /// <summary>
        /// Get Jacobians information of objective function (sparse)
        /// </summary>
        /// <param name="colidx">Column index/indices of Jacobian(s)</param>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        /// <param name="nlflag">NL flag(s) of Jacobian(s) (0 : linear,!=0 : nonlinear)</param>
        /// <param name="nz">Number of nonzeros</param>
        /// <param name="nlnz">Number of nonlinear nonzeros</param>
        public int gmoGetObjSparse(ref int[] colidx, ref double[] jacval, ref int[] nlflag, ref int nz, ref int nlnz)
        {
            return dll_gmoGetObjSparse(pgmo, colidx, jacval, nlflag, ref nz, ref nlnz);
        }
        /// <summary>
        /// Get information for gradient of objective function (sparse)
        /// </summary>
        /// <param name="colidx">Column index/indices of Jacobian(s)</param>
        /// <param name="gradval"></param>
        /// <param name="nlflag">NL flag(s) of Jacobian(s) (0 : linear,!=0 : nonlinear)</param>
        /// <param name="nz">Number of nonzeros</param>
        /// <param name="qnz">Number of quadratic nonzeros in Jacobian matrix</param>
        /// <param name="nlnz">Number of nonlinear nonzeros</param>
        public int gmoGetObjSparseEx(ref int[] colidx, ref double[] gradval, ref int[] nlflag, ref int nz, ref int qnz, ref int nlnz)
        {
            return dll_gmoGetObjSparseEx(pgmo, colidx, gradval, nlflag, ref nz, ref qnz, ref nlnz);
        }
        /// <summary>
        /// Get lower triangle of Q matrix of objective
        /// </summary>
        /// <param name="varidx1">First variable indices</param>
        /// <param name="varidx2">Second variable indices</param>
        /// <param name="coefs">Coefficients</param>
        public int gmoGetObjQMat(ref int[] varidx1, ref int[] varidx2, ref double[] coefs)
        {
            return dll_gmoGetObjQMat(pgmo, varidx1, varidx2, coefs);
        }
        /// <summary>
        /// deprecated synonym for gmoGetObjQMat
        /// </summary>
        /// <param name="varidx1">First variable indices</param>
        /// <param name="varidx2">Second variable indices</param>
        /// <param name="coefs">Coefficients</param>
        public int gmoGetObjQ(ref int[] varidx1, ref int[] varidx2, ref double[] coefs)
        {
            return dll_gmoGetObjQ(pgmo, varidx1, varidx2, coefs);
        }
        /// <summary>
        /// Get c vector of quadratic objective
        /// </summary>
        /// <param name="varidx"></param>
        /// <param name="coefs">Coefficients</param>
        public int gmoGetObjCVec(ref int[] varidx, ref double[] coefs)
        {
            return dll_gmoGetObjCVec(pgmo, varidx, coefs);
        }
        /// <summary>
        /// Get equation activity levels
        /// </summary>
        /// <param name="e">Level values of equations</param>
        public int gmoGetEquL(ref double[] e)
        {
            return dll_gmoGetEquL(pgmo, e);
        }
        /// <summary>
        /// Get individual equation activity levels
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public double gmoGetEquLOne(int si)
        {
            return dll_gmoGetEquLOne(pgmo, si);
        }
        /// <summary>
        /// Set equation activity levels
        /// </summary>
        /// <param name="el">Level of equation</param>
        public int gmoSetEquL(double[] el)
        {
            return dll_gmoSetEquL(pgmo, el);
        }
        /// <summary>
        /// Set individual equation activity levels
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="el">Level of equation</param>
        public void gmoSetEquLOne(int si, double el)
        {
            dll_gmoSetEquLOne(pgmo, si, el);
        }
        /// <summary>
        /// Get equation marginals
        /// </summary>
        /// <param name="pi">Marginal values of equations</param>
        public int gmoGetEquM(ref double[] pi)
        {
            return dll_gmoGetEquM(pgmo, pi);
        }
        /// <summary>
        /// Get individual equation marginal
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public double gmoGetEquMOne(int si)
        {
            return dll_gmoGetEquMOne(pgmo, si);
        }
        /// <summary>
        /// Set equation marginals (pass NULL to set to NA)
        /// </summary>
        /// <param name="emarg">Marginal of equation</param>
        public int gmoSetEquM(double[] emarg)
        {
            return dll_gmoSetEquM(pgmo, emarg);
        }
        /// <summary>
        /// Get individual equation name
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public string gmoGetEquNameOne(int si)
        {
            string rc_gmoGetEquNameOne = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoGetEquNameOne(pgmo, si, sst_result);
            rc_gmoGetEquNameOne = sst_result.ToString();
            return rc_gmoGetEquNameOne;
        }
        /// <summary>
        /// Get individual equation name with quotes and user specified suffix
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="suffix">Suffix appended to name, could be .l, .m etc.</param>
        public string gmoGetEquNameCustomOne(int si, string suffix)
        {
            string rc_gmoGetEquNameCustomOne = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoGetEquNameCustomOne(pgmo, si, suffix, sst_result);
            rc_gmoGetEquNameCustomOne = sst_result.ToString();
            return rc_gmoGetEquNameCustomOne;
        }
        /// <summary>
        /// Get right hand sides
        /// </summary>
        /// <param name="mdblvec">Array of doubles, len=number of rows in user view</param>
        public int gmoGetRhs(ref double[] mdblvec)
        {
            return dll_gmoGetRhs(pgmo, mdblvec);
        }
        /// <summary>
        /// Get individual equation right hand side
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public double gmoGetRhsOne(int si)
        {
            return dll_gmoGetRhsOne(pgmo, si);
        }
        /// <summary>
        /// Get individual equation RHS - independent of useQ'
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public double gmoGetRhsOneEx(int si)
        {
            return dll_gmoGetRhsOneEx(pgmo, si);
        }
        /// <summary>
        /// Set alternative RHS
        /// </summary>
        /// <param name="mdblvec">Array of doubles, len=number of rows in user view</param>
        public int gmoSetAltRHS(double[] mdblvec)
        {
            return dll_gmoSetAltRHS(pgmo, mdblvec);
        }
        /// <summary>
        /// Set individual alternative RHS
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="erhs">RHS of equation</param>
        public void gmoSetAltRHSOne(int si, double erhs)
        {
            dll_gmoSetAltRHSOne(pgmo, si, erhs);
        }
        /// <summary>
        /// Get equation slacks
        /// </summary>
        /// <param name="mdblvec">Array of doubles, len=number of rows in user view</param>
        public int gmoGetEquSlack(ref double[] mdblvec)
        {
            return dll_gmoGetEquSlack(pgmo, mdblvec);
        }
        /// <summary>
        /// Get individual equation slack
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public double gmoGetEquSlackOne(int si)
        {
            return dll_gmoGetEquSlackOne(pgmo, si);
        }
        /// <summary>
        /// Set equation slacks
        /// </summary>
        /// <param name="mdblvec">Array of doubles, len=number of rows in user view</param>
        public int gmoSetEquSlack(double[] mdblvec)
        {
            return dll_gmoSetEquSlack(pgmo, mdblvec);
        }
        /// <summary>
        /// Get equation type
        /// </summary>
        /// <param name="mintvec">Array of integers, len=number of rows in user view</param>
        public int gmoGetEquType(ref int[] mintvec)
        {
            return dll_gmoGetEquType(pgmo, mintvec);
        }
        /// <summary>
        /// Get individual equation type
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public int gmoGetEquTypeOne(int si)
        {
            return dll_gmoGetEquTypeOne(pgmo, si);
        }
        /// <summary>
        /// Get equation basis status
        /// </summary>
        /// <param name="mintvec">Array of integers, len=number of rows in user view</param>
        public void gmoGetEquStat(ref int[] mintvec)
        {
            dll_gmoGetEquStat(pgmo, mintvec);
        }
        /// <summary>
        /// Get individual basis equation status
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public int gmoGetEquStatOne(int si)
        {
            return dll_gmoGetEquStatOne(pgmo, si);
        }
        /// <summary>
        /// Set equation basis status
        /// </summary>
        /// <param name="mintvec">Array of integers, len=number of rows in user view</param>
        public void gmoSetEquStat(int[] mintvec)
        {
            dll_gmoSetEquStat(pgmo, mintvec);
        }
        /// <summary>
        /// Get equation status
        /// </summary>
        /// <param name="mintvec">Array of integers, len=number of rows in user view</param>
        public void gmoGetEquCStat(ref int[] mintvec)
        {
            dll_gmoGetEquCStat(pgmo, mintvec);
        }
        /// <summary>
        /// Get individual equation status
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public int gmoGetEquCStatOne(int si)
        {
            return dll_gmoGetEquCStatOne(pgmo, si);
        }
        /// <summary>
        /// Set equation status
        /// </summary>
        /// <param name="mintvec">Array of integers, len=number of rows in user view</param>
        public void gmoSetEquCStat(int[] mintvec)
        {
            dll_gmoSetEquCStat(pgmo, mintvec);
        }
        /// <summary>
        /// Get equation match
        /// </summary>
        /// <param name="mintvec">Array of integers, len=number of rows in user view</param>
        public int gmoGetEquMatch(ref int[] mintvec)
        {
            return dll_gmoGetEquMatch(pgmo, mintvec);
        }
        /// <summary>
        /// Get individual equation match
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public int gmoGetEquMatchOne(int si)
        {
            return dll_gmoGetEquMatchOne(pgmo, si);
        }
        /// <summary>
        /// Get equation scale
        /// </summary>
        /// <param name="mdblvec">Array of doubles, len=number of rows in user view</param>
        public int gmoGetEquScale(ref double[] mdblvec)
        {
            return dll_gmoGetEquScale(pgmo, mdblvec);
        }
        /// <summary>
        /// Get individual equation scale
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public double gmoGetEquScaleOne(int si)
        {
            return dll_gmoGetEquScaleOne(pgmo, si);
        }
        /// <summary>
        /// Get equation stage
        /// </summary>
        /// <param name="mdblvec">Array of doubles, len=number of rows in user view</param>
        public int gmoGetEquStage(ref double[] mdblvec)
        {
            return dll_gmoGetEquStage(pgmo, mdblvec);
        }
        /// <summary>
        /// Get individual equation stage
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public double gmoGetEquStageOne(int si)
        {
            return dll_gmoGetEquStageOne(pgmo, si);
        }
        /// <summary>
        /// Returns 0 on error, 1 linear, 2 quadratic, 3 nonlinear'
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public int gmoGetEquOrderOne(int si)
        {
            return dll_gmoGetEquOrderOne(pgmo, si);
        }
        /// <summary>
        /// Get Jacobians information of row (sparse)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="colidx">Column index/indices of Jacobian(s)</param>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        /// <param name="nlflag">NL flag(s) of Jacobian(s) (0 : linear,!=0 : nonlinear)</param>
        /// <param name="nz">Number of nonzeros</param>
        /// <param name="nlnz">Number of nonlinear nonzeros</param>
        public int gmoGetRowSparse(int si, ref int[] colidx, ref double[] jacval, ref int[] nlflag, ref int nz, ref int nlnz)
        {
            return dll_gmoGetRowSparse(pgmo, si, colidx, jacval, nlflag, ref nz, ref nlnz);
        }
        /// <summary>
        /// Get info for one row of Jacobian (sparse)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="colidx">Column index/indices of Jacobian(s)</param>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        /// <param name="nlflag">NL flag(s) of Jacobian(s) (0 : linear,!=0 : nonlinear)</param>
        /// <param name="nz">Number of nonzeros</param>
        /// <param name="qnz">Number of quadratic nonzeros in Jacobian matrix</param>
        /// <param name="nlnz">Number of nonlinear nonzeros</param>
        public int gmoGetRowSparseEx(int si, ref int[] colidx, ref double[] jacval, ref int[] nlflag, ref int nz, ref int qnz, ref int nlnz)
        {
            return dll_gmoGetRowSparseEx(pgmo, si, colidx, jacval, nlflag, ref nz, ref qnz, ref nlnz);
        }
        /// <summary>
        /// Get Jacobian information of row one by one
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="jacptr">Pointer to next Jacobian</param>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        /// <param name="colidx">Column index/indices of Jacobian(s)</param>
        /// <param name="nlflag">NL flag(s) of Jacobian(s) (0 : linear,!=0 : nonlinear)</param>
        public void gmoGetRowJacInfoOne(int si, ref IntPtr jacptr, ref double jacval, ref int colidx, ref int nlflag)
        {
            dll_gmoGetRowJacInfoOne(pgmo, si, ref jacptr, ref jacval, ref colidx, ref nlflag);
        }
        /// <summary>
        /// Get lower triangle of Q matrix of row si
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="varidx1">First variable indices</param>
        /// <param name="varidx2">Second variable indices</param>
        /// <param name="coefs">Coefficients</param>
        public int gmoGetRowQMat(int si, ref int[] varidx1, ref int[] varidx2, ref double[] coefs)
        {
            return dll_gmoGetRowQMat(pgmo, si, varidx1, varidx2, coefs);
        }
        /// <summary>
        /// deprecated synonym for gmoGetRowQMat
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="varidx1">First variable indices</param>
        /// <param name="varidx2">Second variable indices</param>
        /// <param name="coefs">Coefficients</param>
        public int gmoGetRowQ(int si, ref int[] varidx1, ref int[] varidx2, ref double[] coefs)
        {
            return dll_gmoGetRowQ(pgmo, si, varidx1, varidx2, coefs);
        }
        /// <summary>
        /// Get c vector of the quadratic form for row si
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="varidx"></param>
        /// <param name="coefs">Coefficients</param>
        public int gmoGetRowCVec(int si, ref int[] varidx, ref double[] coefs)
        {
            return dll_gmoGetRowCVec(pgmo, si, varidx, coefs);
        }
        /// <summary>
        /// Get the constant of the quadratic form for row si
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public double gmoGetRowQConst(int si)
        {
            return dll_gmoGetRowQConst(pgmo, si);
        }
        /// <summary>
        /// Get equation integer values for dot optio
        /// </summary>
        /// <param name="optptr">Option object pointer</param>
        /// <param name="dotopt">Dot option name</param>
        /// <param name="optvals">Option values</param>
        public int gmoGetEquIntDotOpt(IntPtr optptr, string dotopt, ref int[] optvals)
        {
            return dll_gmoGetEquIntDotOpt(pgmo, optptr, dotopt, optvals);
        }
        /// <summary>
        /// Get equation double values for dot optio
        /// </summary>
        /// <param name="optptr">Option object pointer</param>
        /// <param name="dotopt">Dot option name</param>
        /// <param name="optvals">Option values</param>
        public int gmoGetEquDblDotOpt(IntPtr optptr, string dotopt, ref double[] optvals)
        {
            return dll_gmoGetEquDblDotOpt(pgmo, optptr, dotopt, optvals);
        }
        /// <summary>
        /// Get variable level values
        /// </summary>
        /// <param name="x">Level values of variables</param>
        public int gmoGetVarL(ref double[] x)
        {
            return dll_gmoGetVarL(pgmo, x);
        }
        /// <summary>
        /// Get individual variable level
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public double gmoGetVarLOne(int sj)
        {
            return dll_gmoGetVarLOne(pgmo, sj);
        }
        /// <summary>
        /// Set variable level values
        /// </summary>
        /// <param name="x">Level values of variables</param>
        public int gmoSetVarL(double[] x)
        {
            return dll_gmoSetVarL(pgmo, x);
        }
        /// <summary>
        /// Set individual variable level
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="vl">Level of variable</param>
        public void gmoSetVarLOne(int sj, double vl)
        {
            dll_gmoSetVarLOne(pgmo, sj, vl);
        }
        /// <summary>
        /// Get variable marginals
        /// </summary>
        /// <param name="dj">Marginal values of variables</param>
        public int gmoGetVarM(ref double[] dj)
        {
            return dll_gmoGetVarM(pgmo, dj);
        }
        /// <summary>
        /// Get individual variable marginal
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public double gmoGetVarMOne(int sj)
        {
            return dll_gmoGetVarMOne(pgmo, sj);
        }
        /// <summary>
        /// Set variable marginals (pass null to set to NA)'
        /// </summary>
        /// <param name="dj">Marginal values of variables</param>
        public int gmoSetVarM(double[] dj)
        {
            return dll_gmoSetVarM(pgmo, dj);
        }
        /// <summary>
        /// Set individual variable marginal
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="vmarg">Marginal of variable</param>
        public void gmoSetVarMOne(int sj, double vmarg)
        {
            dll_gmoSetVarMOne(pgmo, sj, vmarg);
        }
        /// <summary>
        /// Get individual column name
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public string gmoGetVarNameOne(int sj)
        {
            string rc_gmoGetVarNameOne = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoGetVarNameOne(pgmo, sj, sst_result);
            rc_gmoGetVarNameOne = sst_result.ToString();
            return rc_gmoGetVarNameOne;
        }
        /// <summary>
        /// Get individual column name with quotes and user specified suffix
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="suffix">Suffix appended to name, could be .l, .m etc.</param>
        public string gmoGetVarNameCustomOne(int sj, string suffix)
        {
            string rc_gmoGetVarNameCustomOne = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoGetVarNameCustomOne(pgmo, sj, suffix, sst_result);
            rc_gmoGetVarNameCustomOne = sst_result.ToString();
            return rc_gmoGetVarNameCustomOne;
        }
        /// <summary>
        /// Get variable lower bounds
        /// </summary>
        /// <param name="lovec">Lower bound values of variables</param>
        public int gmoGetVarLower(ref double[] lovec)
        {
            return dll_gmoGetVarLower(pgmo, lovec);
        }
        /// <summary>
        /// Get individual variable lower bound
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public double gmoGetVarLowerOne(int sj)
        {
            return dll_gmoGetVarLowerOne(pgmo, sj);
        }
        /// <summary>
        /// Get variable upper bounds
        /// </summary>
        /// <param name="upvec">Upper bound values of variables</param>
        public int gmoGetVarUpper(ref double[] upvec)
        {
            return dll_gmoGetVarUpper(pgmo, upvec);
        }
        /// <summary>
        /// Get individual variable upper bound
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public double gmoGetVarUpperOne(int sj)
        {
            return dll_gmoGetVarUpperOne(pgmo, sj);
        }
        /// <summary>
        /// Set alternative variable lower and upper bounds
        /// </summary>
        /// <param name="lovec">Lower bound values of variables</param>
        /// <param name="upvec">Upper bound values of variables</param>
        public int gmoSetAltVarBounds(double[] lovec, double[] upvec)
        {
            return dll_gmoSetAltVarBounds(pgmo, lovec, upvec);
        }
        /// <summary>
        /// Set individual alternative variable lower bound
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="vlo">Lower bound of variable</param>
        public void gmoSetAltVarLowerOne(int sj, double vlo)
        {
            dll_gmoSetAltVarLowerOne(pgmo, sj, vlo);
        }
        /// <summary>
        /// Set individual alternative variable upper bound
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="vup">Upper bound of variable</param>
        public void gmoSetAltVarUpperOne(int sj, double vup)
        {
            dll_gmoSetAltVarUpperOne(pgmo, sj, vup);
        }
        /// <summary>
        /// Get variable type
        /// </summary>
        /// <param name="nintvec">Array of integers, len=number of columns in user view</param>
        public int gmoGetVarType(ref int[] nintvec)
        {
            return dll_gmoGetVarType(pgmo, nintvec);
        }
        /// <summary>
        /// Get individual variable type
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public int gmoGetVarTypeOne(int sj)
        {
            return dll_gmoGetVarTypeOne(pgmo, sj);
        }
        /// <summary>
        /// Set alternative variable type
        /// </summary>
        /// <param name="nintvec">Array of integers, len=number of columns in user view</param>
        public int gmoSetAltVarType(int[] nintvec)
        {
            return dll_gmoSetAltVarType(pgmo, nintvec);
        }
        /// <summary>
        /// Set individual alternative variable type
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="vtyp">Type of variable (see enumerated constants)</param>
        public void gmoSetAltVarTypeOne(int sj, int vtyp)
        {
            dll_gmoSetAltVarTypeOne(pgmo, sj, vtyp);
        }
        /// <summary>
        /// Get variable basis status
        /// </summary>
        /// <param name="nintvec">Array of integers, len=number of columns in user view</param>
        public void gmoGetVarStat(ref int[] nintvec)
        {
            dll_gmoGetVarStat(pgmo, nintvec);
        }
        /// <summary>
        /// Get individual variable basis status
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public int gmoGetVarStatOne(int sj)
        {
            return dll_gmoGetVarStatOne(pgmo, sj);
        }
        /// <summary>
        /// Set variable basis status
        /// </summary>
        /// <param name="nintvec">Array of integers, len=number of columns in user view</param>
        public void gmoSetVarStat(int[] nintvec)
        {
            dll_gmoSetVarStat(pgmo, nintvec);
        }
        /// <summary>
        /// Set individual variable basis status
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="vstat">Basis status of variable (see enumerated constants)</param>
        public void gmoSetVarStatOne(int sj, int vstat)
        {
            dll_gmoSetVarStatOne(pgmo, sj, vstat);
        }
        /// <summary>
        /// Get variable status
        /// </summary>
        /// <param name="nintvec">Array of integers, len=number of columns in user view</param>
        public void gmoGetVarCStat(ref int[] nintvec)
        {
            dll_gmoGetVarCStat(pgmo, nintvec);
        }
        /// <summary>
        /// Get individual variable status
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public int gmoGetVarCStatOne(int sj)
        {
            return dll_gmoGetVarCStatOne(pgmo, sj);
        }
        /// <summary>
        /// Set variable status
        /// </summary>
        /// <param name="nintvec">Array of integers, len=number of columns in user view</param>
        public void gmoSetVarCStat(int[] nintvec)
        {
            dll_gmoSetVarCStat(pgmo, nintvec);
        }
        /// <summary>
        /// Get variable match
        /// </summary>
        /// <param name="nintvec">Array of integers, len=number of columns in user view</param>
        public int gmoGetVarMatch(ref int[] nintvec)
        {
            return dll_gmoGetVarMatch(pgmo, nintvec);
        }
        /// <summary>
        /// Get individual variable match
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public int gmoGetVarMatchOne(int sj)
        {
            return dll_gmoGetVarMatchOne(pgmo, sj);
        }
        /// <summary>
        /// Get variable branching priority
        /// </summary>
        /// <param name="ndblvec">Array of doubles, len=number of columns in user view</param>
        public int gmoGetVarPrior(ref double[] ndblvec)
        {
            return dll_gmoGetVarPrior(pgmo, ndblvec);
        }
        /// <summary>
        /// Get individual variable branching priority
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public double gmoGetVarPriorOne(int sj)
        {
            return dll_gmoGetVarPriorOne(pgmo, sj);
        }
        /// <summary>
        /// Get variable scale
        /// </summary>
        /// <param name="ndblvec">Array of doubles, len=number of columns in user view</param>
        public int gmoGetVarScale(ref double[] ndblvec)
        {
            return dll_gmoGetVarScale(pgmo, ndblvec);
        }
        /// <summary>
        /// Get individual variable scale
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public double gmoGetVarScaleOne(int sj)
        {
            return dll_gmoGetVarScaleOne(pgmo, sj);
        }
        /// <summary>
        /// Get variable stage
        /// </summary>
        /// <param name="ndblvec">Array of doubles, len=number of columns in user view</param>
        public int gmoGetVarStage(ref double[] ndblvec)
        {
            return dll_gmoGetVarStage(pgmo, ndblvec);
        }
        /// <summary>
        /// Get individual variable stage
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public double gmoGetVarStageOne(int sj)
        {
            return dll_gmoGetVarStageOne(pgmo, sj);
        }
        /// <summary>
        /// Get SOS constraints
        /// </summary>
        /// <param name="sostype">SOS type 1 or 2</param>
        /// <param name="sosbeg">Variable index start of SOS set</param>
        /// <param name="sosind">Variable indices</param>
        /// <param name="soswt">Weight in SOS set</param>
        public int gmoGetSosConstraints(ref int[] sostype, ref int[] sosbeg, ref int[] sosind, ref double[] soswt)
        {
            return dll_gmoGetSosConstraints(pgmo, sostype, sosbeg, sosind, soswt);
        }
        /// <summary>
        /// Get SOS set for individual variable
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public int gmoGetVarSosSetOne(int sj)
        {
            return dll_gmoGetVarSosSetOne(pgmo, sj);
        }
        /// <summary>
        /// Get Jacobians information of column (sparse)
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="rowidx">Row index/indices of Jacobian</param>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        /// <param name="nlflag">NL flag(s) of Jacobian(s) (0 : linear,!=0 : nonlinear)</param>
        /// <param name="nz">Number of nonzeros</param>
        /// <param name="nlnz">Number of nonlinear nonzeros</param>
        public int gmoGetColSparse(int sj, ref int[] rowidx, ref double[] jacval, ref int[] nlflag, ref int nz, ref int nlnz)
        {
            return dll_gmoGetColSparse(pgmo, sj, rowidx, jacval, nlflag, ref nz, ref nlnz);
        }
        /// <summary>
        /// Get Jacobian information of column one by one
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="jacptr">Pointer to next Jacobian</param>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        /// <param name="rowidx">Row index/indices of Jacobian</param>
        /// <param name="nlflag">NL flag(s) of Jacobian(s) (0 : linear,!=0 : nonlinear)</param>
        public void gmoGetColJacInfoOne(int sj, ref IntPtr jacptr, ref double jacval, ref int rowidx, ref int nlflag)
        {
            dll_gmoGetColJacInfoOne(pgmo, sj, ref jacptr, ref jacval, ref rowidx, ref nlflag);
        }
        /// <summary>
        /// Get variable integer values for dot option
        /// </summary>
        /// <param name="optptr">Option object pointer</param>
        /// <param name="dotopt">Dot option name</param>
        /// <param name="optvals">Option values</param>
        public int gmoGetVarIntDotOpt(IntPtr optptr, string dotopt, ref int[] optvals)
        {
            return dll_gmoGetVarIntDotOpt(pgmo, optptr, dotopt, optvals);
        }
        /// <summary>
        /// Get variable double values for dot option
        /// </summary>
        /// <param name="optptr">Option object pointer</param>
        /// <param name="dotopt">Dot option name</param>
        /// <param name="optvals">Option values</param>
        public int gmoGetVarDblDotOpt(IntPtr optptr, string dotopt, ref double[] optvals)
        {
            return dll_gmoGetVarDblDotOpt(pgmo, optptr, dotopt, optvals);
        }
        /// <summary>
        /// Control writing messages for evaluation errors, default=true
        /// </summary>
        /// <param name="domsg">Flag whether to write messages</param>
        public void gmoEvalErrorMsg(bool domsg)
        {
            int ib_domsg = 0;
            if (domsg) ib_domsg = 1;
            dll_gmoEvalErrorMsg(pgmo, ib_domsg);
        }
        /// <summary>
        /// Control writing messages for evaluation errors, default=true
        /// </summary>
        /// <param name="domsg">Flag whether to write messages</param>
        /// <param name="tidx">Index of thread</param>
        public void gmoEvalErrorMsg_MT(bool domsg, int tidx)
        {
            int ib_domsg = 0;
            if (domsg) ib_domsg = 1;
            dll_gmoEvalErrorMsg_MT(pgmo, ib_domsg, tidx);
        }
        /// <summary>
        /// Set mask to ignore errors >= evalErrorMaskLevel when incrementing numerr
        /// </summary>
        /// <param name="MaskLevel">Ignore evaluation errors less that this value</param>
        public void gmoEvalErrorMaskLevel(int MaskLevel)
        {
            dll_gmoEvalErrorMaskLevel(pgmo, MaskLevel);
        }
        /// <summary>
        /// Set mask to ignore errors >= evalErrorMaskLevel when incrementing numerr
        /// </summary>
        /// <param name="MaskLevel">Ignore evaluation errors less that this value</param>
        /// <param name="tidx">Index of thread</param>
        public void gmoEvalErrorMaskLevel_MT(int MaskLevel, int tidx)
        {
            dll_gmoEvalErrorMaskLevel_MT(pgmo, MaskLevel, tidx);
        }
        /// <summary>
        /// New point for the next evaluation call
        /// </summary>
        /// <param name="x">Level values of variables</param>
        public int gmoEvalNewPoint(double[] x)
        {
            return dll_gmoEvalNewPoint(pgmo, x);
        }
        /// <summary>
        /// Set external function manager object
        /// </summary>
        /// <param name="extfunmgr"></param>
        public void gmoSetExtFuncs(IntPtr extfunmgr)
        {
            dll_gmoSetExtFuncs(pgmo, extfunmgr);
        }
        /// <summary>
        /// Evaluate the constraint si (excluding RHS)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="f">Function value</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalFunc(int si, double[] x, ref double f, ref int numerr)
        {
            return dll_gmoEvalFunc(pgmo, si, x, ref f, ref numerr);
        }
        /// <summary>
        /// Evaluate the constraint si (excluding RHS)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="f">Function value</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        /// <param name="tidx">Index of thread</param>
        public int gmoEvalFunc_MT(int si, double[] x, ref double f, ref int numerr, int tidx)
        {
            return dll_gmoEvalFunc_MT(pgmo, si, x, ref f, ref numerr, tidx);
        }
        /// <summary>
        /// Evaluate the constraint si using the GMO internal variable levels (excluding RHS)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="f">Function value</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalFuncInt(int si, ref double f, ref int numerr)
        {
            return dll_gmoEvalFuncInt(pgmo, si, ref f, ref numerr);
        }
        /// <summary>
        /// Evaluate the constraint si using the GMO internal variable levels (excluding RHS)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="f">Function value</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        /// <param name="tidx">Index of thread</param>
        public int gmoEvalFuncInt_MT(int si, ref double f, ref int numerr, int tidx)
        {
            return dll_gmoEvalFuncInt_MT(pgmo, si, ref f, ref numerr, tidx);
        }
        /// <summary>
        /// Evaluate the nonlinear function component of constraint si
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="fnl">Part of the function value depending on the nonlinear variables</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalFuncNL(int si, double[] x, ref double fnl, ref int numerr)
        {
            return dll_gmoEvalFuncNL(pgmo, si, x, ref fnl, ref numerr);
        }
        /// <summary>
        /// Evaluate the nonlinear function component of constraint si
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="fnl">Part of the function value depending on the nonlinear variables</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        /// <param name="tidx">Index of thread</param>
        public int gmoEvalFuncNL_MT(int si, double[] x, ref double fnl, ref int numerr, int tidx)
        {
            return dll_gmoEvalFuncNL_MT(pgmo, si, x, ref fnl, ref numerr, tidx);
        }
        /// <summary>
        /// Evaluate objective function component
        /// </summary>
        /// <param name="x">Level values of variables</param>
        /// <param name="f">Function value</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalFuncObj(double[] x, ref double f, ref int numerr)
        {
            return dll_gmoEvalFuncObj(pgmo, x, ref f, ref numerr);
        }
        /// <summary>
        /// Evaluate nonlinear objective function component
        /// </summary>
        /// <param name="x">Level values of variables</param>
        /// <param name="fnl">Part of the function value depending on the nonlinear variables</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalFuncNLObj(double[] x, ref double fnl, ref int numerr)
        {
            return dll_gmoEvalFuncNLObj(pgmo, x, ref fnl, ref numerr);
        }
        /// <summary>
        /// Evaluate the function value of constraint si on the giving interval
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="xmin">Minimum input level values of variables</param>
        /// <param name="xmax">Maximum input level values of variables</param>
        /// <param name="fmin">Minimum function value</param>
        /// <param name="fmax">Maximum function value</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalFuncInterval(int si, double[] xmin, double[] xmax, ref double fmin, ref double fmax, ref int numerr)
        {
            return dll_gmoEvalFuncInterval(pgmo, si, xmin, xmax, ref fmin, ref fmax, ref numerr);
        }
        /// <summary>
        /// Evaluate the function value of constraint si on the giving interval
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="xmin">Minimum input level values of variables</param>
        /// <param name="xmax">Maximum input level values of variables</param>
        /// <param name="fmin">Minimum function value</param>
        /// <param name="fmax">Maximum function value</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        /// <param name="tidx">Index of thread</param>
        public int gmoEvalFuncInterval_MT(int si, double[] xmin, double[] xmax, ref double fmin, ref double fmax, ref int numerr, int tidx)
        {
            return dll_gmoEvalFuncInterval_MT(pgmo, si, xmin, xmax, ref fmin, ref fmax, ref numerr, tidx);
        }
        /// <summary>
        /// Evaluate the nonlinear function value component of clusters of variables of constraint si
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="cluster">Array defining the clustering of variables</param>
        /// <param name="ncluster"></param>
        /// <param name="fnl">Part of the function value depending on the nonlinear variables</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalFuncNLCluster(int si, double[] x, int[] cluster, int ncluster, ref double[] fnl, ref int numerr)
        {
            return dll_gmoEvalFuncNLCluster(pgmo, si, x, cluster, ncluster, fnl, ref numerr);
        }
        /// <summary>
        /// Evaluate the nonlinear function value component of clusters of variables of constraint si
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="cluster">Array defining the clustering of variables</param>
        /// <param name="ncluster"></param>
        /// <param name="fnl">Part of the function value depending on the nonlinear variables</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        /// <param name="tidx">Index of thread</param>
        public int gmoEvalFuncNLCluster_MT(int si, double[] x, int[] cluster, int ncluster, ref double[] fnl, ref int numerr, int tidx)
        {
            return dll_gmoEvalFuncNLCluster_MT(pgmo, si, x, cluster, ncluster, fnl, ref numerr, tidx);
        }
        /// <summary>
        /// Update the nonlinear gradients of constraint si and evaluate function value
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="f">Function value</param>
        /// <param name="g">Gradient values</param>
        /// <param name="gx">Inner product of the gradient with the input variables</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalGrad(int si, double[] x, ref double f, ref double[] g, ref double gx, ref int numerr)
        {
            return dll_gmoEvalGrad(pgmo, si, x, ref f, g, ref gx, ref numerr);
        }
        /// <summary>
        /// Update the nonlinear gradients of constraint si and evaluate function value
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="f">Function value</param>
        /// <param name="g">Gradient values</param>
        /// <param name="gx">Inner product of the gradient with the input variables</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        /// <param name="tidx">Index of thread</param>
        public int gmoEvalGrad_MT(int si, double[] x, ref double f, ref double[] g, ref double gx, ref int numerr, int tidx)
        {
            return dll_gmoEvalGrad_MT(pgmo, si, x, ref f, g, ref gx, ref numerr, tidx);
        }
        /// <summary>
        /// Update the nonlinear gradients of constraint si and evaluate nonlinear function and gradient value
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="fnl">Part of the function value depending on the nonlinear variables</param>
        /// <param name="g">Gradient values</param>
        /// <param name="gxnl">Inner product of the gradient with the input variables, nonlinear variables only</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalGradNL(int si, double[] x, ref double fnl, ref double[] g, ref double gxnl, ref int numerr)
        {
            return dll_gmoEvalGradNL(pgmo, si, x, ref fnl, g, ref gxnl, ref numerr);
        }
        /// <summary>
        /// Update the nonlinear gradients of constraint si and evaluate nonlinear function and gradient value
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="fnl">Part of the function value depending on the nonlinear variables</param>
        /// <param name="g">Gradient values</param>
        /// <param name="gxnl">Inner product of the gradient with the input variables, nonlinear variables only</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        /// <param name="tidx">Index of thread</param>
        public int gmoEvalGradNL_MT(int si, double[] x, ref double fnl, ref double[] g, ref double gxnl, ref int numerr, int tidx)
        {
            return dll_gmoEvalGradNL_MT(pgmo, si, x, ref fnl, g, ref gxnl, ref numerr, tidx);
        }
        /// <summary>
        /// Update the gradients of the objective function and evaluate function and gradient value
        /// </summary>
        /// <param name="x">Level values of variables</param>
        /// <param name="f">Function value</param>
        /// <param name="g">Gradient values</param>
        /// <param name="gx">Inner product of the gradient with the input variables</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalGradObj(double[] x, ref double f, ref double[] g, ref double gx, ref int numerr)
        {
            return dll_gmoEvalGradObj(pgmo, x, ref f, g, ref gx, ref numerr);
        }
        /// <summary>
        /// Update the nonlinear gradients of the objective function and evaluate function and gradient value
        /// </summary>
        /// <param name="x">Level values of variables</param>
        /// <param name="fnl">Part of the function value depending on the nonlinear variables</param>
        /// <param name="g">Gradient values</param>
        /// <param name="gxnl">Inner product of the gradient with the input variables, nonlinear variables only</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalGradNLObj(double[] x, ref double fnl, ref double[] g, ref double gxnl, ref int numerr)
        {
            return dll_gmoEvalGradNLObj(pgmo, x, ref fnl, g, ref gxnl, ref numerr);
        }
        /// <summary>
        /// Evaluate the function and gradient value of constraint si on the giving interval
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="xmin">Minimum input level values of variables</param>
        /// <param name="xmax">Maximum input level values of variables</param>
        /// <param name="fmin">Minimum function value</param>
        /// <param name="fmax">Maximum function value</param>
        /// <param name="gmin">Minimum gradient values</param>
        /// <param name="gmax">Maximum gradient values</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalGradInterval(int si, double[] xmin, double[] xmax, ref double fmin, ref double fmax, ref double[] gmin, ref double[] gmax, ref int numerr)
        {
            return dll_gmoEvalGradInterval(pgmo, si, xmin, xmax, ref fmin, ref fmax, gmin, gmax, ref numerr);
        }
        /// <summary>
        /// Evaluate the function and gradient value of constraint si on the giving interval
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="xmin">Minimum input level values of variables</param>
        /// <param name="xmax">Maximum input level values of variables</param>
        /// <param name="fmin">Minimum function value</param>
        /// <param name="fmax">Maximum function value</param>
        /// <param name="gmin">Minimum gradient values</param>
        /// <param name="gmax">Maximum gradient values</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        /// <param name="tidx">Index of thread</param>
        public int gmoEvalGradInterval_MT(int si, double[] xmin, double[] xmax, ref double fmin, ref double fmax, ref double[] gmin, ref double[] gmax, ref int numerr, int tidx)
        {
            return dll_gmoEvalGradInterval_MT(pgmo, si, xmin, xmax, ref fmin, ref fmax, gmin, gmax, ref numerr, tidx);
        }
        /// <summary>
        /// Evaluate all nonlinear gradients and return change vector plus optional update of Jacobians
        /// </summary>
        /// <param name="rhsdelta">Taylor expansion constants</param>
        /// <param name="dojacupd">Flag whether to update Jacobians</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoEvalGradNLUpdate(ref double[] rhsdelta, bool dojacupd, ref int numerr)
        {
            int ib_dojacupd = 0;
            if (dojacupd) ib_dojacupd = 1;
            return dll_gmoEvalGradNLUpdate(pgmo, rhsdelta, ib_dojacupd, ref numerr);
        }
        /// <summary>
        /// Retrieve the updated Jacobian elements
        /// </summary>
        /// <param name="rowidx">Row index/indices of Jacobian</param>
        /// <param name="colidx">Column index/indices of Jacobian(s)</param>
        /// <param name="jacval">Value(s) of Jacobian(s)</param>
        /// <param name="len">Length of array</param>
        public int gmoGetJacUpdate(ref int[] rowidx, ref int[] colidx, ref double[] jacval, ref int len)
        {
            return dll_gmoGetJacUpdate(pgmo, rowidx, colidx, jacval, ref len);
        }
        /// <summary>
        /// Initialize Hessians
        /// </summary>
        /// <param name="maxJacMult">Multiplier to define memory limit for Hessian (0=no limit)</param>
        /// <param name="do2dir">Flag whether 2nd derivatives are wanted/available</param>
        /// <param name="doHess">Flag whether Hessians are wanted/available</param>
        public int gmoHessLoad(double maxJacMult, ref int do2dir, ref int doHess)
        {
            return dll_gmoHessLoad(pgmo, maxJacMult, ref do2dir, ref doHess);
        }
        /// <summary>
        /// Unload Hessians
        /// </summary>
        public int gmoHessUnload()
        {
            return dll_gmoHessUnload(pgmo);
        }
        /// <summary>
        /// Hessian dimension of row
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public int gmoHessDim(int si)
        {
            return dll_gmoHessDim(pgmo, si);
        }
        /// <summary>
        /// Hessian nonzeros of row
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public int gmoHessNz(int si)
        {
            return dll_gmoHessNz(pgmo, si);
        }
        /// <summary>
        /// Get Hessian Structure
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="hridx">Hessian row indices</param>
        /// <param name="hcidx">Hessian column indices</param>
        /// <param name="hessdim">Dimension of Hessian</param>
        /// <param name="hessnz">Number of nonzeros in Hessian</param>
        public int gmoHessStruct(int si, ref int[] hridx, ref int[] hcidx, ref int hessdim, ref int hessnz)
        {
            return dll_gmoHessStruct(pgmo, si, hridx, hcidx, ref hessdim, ref hessnz);
        }
        /// <summary>
        /// Get Hessian Value
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="hridx">Hessian row indices</param>
        /// <param name="hcidx">Hessian column indices</param>
        /// <param name="hessdim">Dimension of Hessian</param>
        /// <param name="hessnz">Number of nonzeros in Hessian</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="hessval"></param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoHessValue(int si, ref int[] hridx, ref int[] hcidx, ref int hessdim, ref int hessnz, double[] x, ref double[] hessval, ref int numerr)
        {
            return dll_gmoHessValue(pgmo, si, hridx, hcidx, ref hessdim, ref hessnz, x, hessval, ref numerr);
        }
        /// <summary>
        /// Get Hessian-vector product
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="dx">Direction in x-space for directional derivative of Lagrangian (W*dx)</param>
        /// <param name="Wdx">Directional derivative of the Lagrangian in direction dx (W*dx)</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoHessVec(int si, double[] x, double[] dx, ref double[] Wdx, ref int numerr)
        {
            return dll_gmoHessVec(pgmo, si, x, dx, Wdx, ref numerr);
        }
        /// <summary>
        /// Get Hessian of the Lagrangian Value structure
        /// </summary>
        /// <param name="WRindex">Row indices for the upper triangle of the symmetric matrix W (the Hessian of the Lagrangian), ordered by rows and within rows by columns</param>
        /// <param name="WCindex">Col indices for the upper triangle of the symmetric matrix W (the Hessian of the Lagrangian), ordered by rows and within rows by columns</param>
        public int gmoHessLagStruct(ref int[] WRindex, ref int[] WCindex)
        {
            return dll_gmoHessLagStruct(pgmo, WRindex, WCindex);
        }
        /// <summary>
        /// Get Hessian of the Lagrangian Value
        /// </summary>
        /// <param name="x">Level values of variables</param>
        /// <param name="pi">Marginal values of equations</param>
        /// <param name="w">Values for the structural nonzeros of the upper triangle of the symmetric matrix W (the Hessian of the Lagrangian), ordered by rows and within rows by columns</param>
        /// <param name="objweight">Weight for objective in Hessian of the Lagrangian (=1 for GAMS convention)</param>
        /// <param name="conweight">Weight for constraints in Hessian of the Lagrangian (=1 for GAMS convention)</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoHessLagValue(double[] x, double[] pi, ref double[] w, double objweight, double conweight, ref int numerr)
        {
            return dll_gmoHessLagValue(pgmo, x, pi, w, objweight, conweight, ref numerr);
        }
        /// <summary>
        /// Get Hessian of the Lagrangian-vector product
        /// </summary>
        /// <param name="x">Level values of variables</param>
        /// <param name="pi">Marginal values of equations</param>
        /// <param name="dx">Direction in x-space for directional derivative of Lagrangian (W*dx)</param>
        /// <param name="Wdx">Directional derivative of the Lagrangian in direction dx (W*dx)</param>
        /// <param name="objweight">Weight for objective in Hessian of the Lagrangian (=1 for GAMS convention)</param>
        /// <param name="conweight">Weight for constraints in Hessian of the Lagrangian (=1 for GAMS convention)</param>
        /// <param name="numerr">Number of errors evaluating the nonlinear function</param>
        public int gmoHessLagVec(double[] x, double[] pi, double[] dx, ref double[] Wdx, double objweight, double conweight, ref int numerr)
        {
            return dll_gmoHessLagVec(pgmo, x, pi, dx, Wdx, objweight, conweight, ref numerr);
        }
        /// <summary>
        /// Load EMP information
        /// </summary>
        /// <param name="empinfofname">Name of EMP information file, if empty assume the default name and location</param>
        public int gmoLoadEMPInfo(string empinfofname)
        {
            return dll_gmoLoadEMPInfo(pgmo, empinfofname);
        }
        /// <summary>
        /// Get VI mapping for all rows (-1 if not a VI function)
        /// </summary>
        /// <param name="mintvec">Array of integers, len=number of rows in user view</param>
        public int gmoGetEquVI(ref int[] mintvec)
        {
            return dll_gmoGetEquVI(pgmo, mintvec);
        }
        /// <summary>
        /// Get VI mapping for individual row (-1 if not a VI function)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        public int gmoGetEquVIOne(int si)
        {
            return dll_gmoGetEquVIOne(pgmo, si);
        }
        /// <summary>
        /// Get VI mapping for all cols (-1 if not a VI variable)
        /// </summary>
        /// <param name="nintvec">Array of integers, len=number of columns in user view</param>
        public int gmoGetVarVI(ref int[] nintvec)
        {
            return dll_gmoGetVarVI(pgmo, nintvec);
        }
        /// <summary>
        /// Get VI mapping for individual cols (-1 if not a VI variable)
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        public int gmoGetVarVIOne(int sj)
        {
            return dll_gmoGetVarVIOne(pgmo, sj);
        }
        /// <summary>
        /// Get Agent Type of all agent (see gmoNumAgents)
        /// </summary>
        /// <param name="agentvec">Array of agent types of length gmoNumAgents</param>
        public int gmoGetAgentType(ref int[] agentvec)
        {
            return dll_gmoGetAgentType(pgmo, agentvec);
        }
        /// <summary>
        /// Get Agent Type of agent
        /// </summary>
        /// <param name="aidx">Index of agent</param>
        public int gmoGetAgentTypeOne(int aidx)
        {
            return dll_gmoGetAgentTypeOne(pgmo, aidx);
        }
        /// <summary>
        /// Get equation and variable mapping to agents
        /// </summary>
        /// <param name="nintvec">Array of integers, len=number of columns in user view</param>
        /// <param name="mintvec">Array of integers, len=number of rows in user view</param>
        public int gmoGetBiLevelInfo(ref int[] nintvec, ref int[] mintvec)
        {
            return dll_gmoGetBiLevelInfo(pgmo, nintvec, mintvec);
        }
        /// <summary>
        /// Dump EMPInfo GDX File
        /// </summary>
        /// <param name="gdxfname">Name of GDX file</param>
        public int gmoDumpEMPInfoToGDX(string gdxfname)
        {
            return dll_gmoDumpEMPInfoToGDX(pgmo, gdxfname);
        }
        /// <summary>
        /// Get value of solution head or tail record, except for modelstat and solvestat (see enumerated constants)
        /// </summary>
        /// <param name="htrec">Solution head or tail record, (see enumerated constants)</param>
        public double gmoGetHeadnTail(int htrec)
        {
            return dll_gmoGetHeadnTail(pgmo, htrec);
        }
        /// <summary>
        /// Set value of solution head or tail record, except for modelstat and solvestat (see enumerated constants)
        /// </summary>
        /// <param name="htrec">Solution head or tail record, (see enumerated constants)</param>
        /// <param name="dval">Double value</param>
        public void gmoSetHeadnTail(int htrec, double dval)
        {
            dll_gmoSetHeadnTail(pgmo, htrec, dval);
        }
        /// <summary>
        /// Set solution values for variable levels
        /// </summary>
        /// <param name="x">Level values of variables</param>
        public int gmoSetSolutionPrimal(double[] x)
        {
            return dll_gmoSetSolutionPrimal(pgmo, x);
        }
        /// <summary>
        /// Set solution values for variable levels and equation marginals
        /// </summary>
        /// <param name="x">Level values of variables</param>
        /// <param name="pi">Marginal values of equations</param>
        public int gmoSetSolution2(double[] x, double[] pi)
        {
            return dll_gmoSetSolution2(pgmo, x, pi);
        }
        /// <summary>
        /// Set solution values for variable and equation levels as well as marginals
        /// </summary>
        /// <param name="x">Level values of variables</param>
        /// <param name="dj">Marginal values of variables</param>
        /// <param name="pi">Marginal values of equations</param>
        /// <param name="e">Level values of equations</param>
        public int gmoSetSolution(double[] x, double[] dj, double[] pi, double[] e)
        {
            return dll_gmoSetSolution(pgmo, x, dj, pi, e);
        }
        /// <summary>
        /// Set solution values for variable and equation levels, marginals and statuses
        /// </summary>
        /// <param name="x">Level values of variables</param>
        /// <param name="dj">Marginal values of variables</param>
        /// <param name="pi">Marginal values of equations</param>
        /// <param name="e">Level values of equations</param>
        /// <param name="xb">Basis statuses of variables (see enumerated constants)</param>
        /// <param name="xs">Statuses of variables (see enumerated constants)</param>
        /// <param name="yb">Basis statuses of equations (see enumerated constants)</param>
        /// <param name="ys">Statuses of equation (see enumerated constants)</param>
        public int gmoSetSolution8(double[] x, double[] dj, double[] pi, double[] e, ref int[] xb, ref int[] xs, ref int[] yb, ref int[] ys)
        {
            return dll_gmoSetSolution8(pgmo, x, dj, pi, e, xb, xs, yb, ys);
        }
        /// <summary>
        /// Construct and set solution based on available inputs
        /// </summary>
        /// <param name="modelstathint">Model status used as a hint</param>
        /// <param name="x">Level values of variables</param>
        /// <param name="pi">Marginal values of equations</param>
        /// <param name="xb">Basis statuses of variables (see enumerated constants)</param>
        /// <param name="yb">Basis statuses of equations (see enumerated constants)</param>
        /// <param name="infTol">Infeasibility tolerance</param>
        /// <param name="optTol">Optimality tolerance</param>
        public int gmoSetSolutionFixer(int modelstathint, double[] x, double[] pi, int[] xb, int[] yb, double infTol, double optTol)
        {
            return dll_gmoSetSolutionFixer(pgmo, modelstathint, x, pi, xb, yb, infTol, optTol);
        }
        /// <summary>
        /// Get variable solution values (level, marginals and statuses)
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="vl">Level of variable</param>
        /// <param name="vmarg">Marginal of variable</param>
        /// <param name="vstat">Basis status of variable (see enumerated constants)</param>
        /// <param name="vcstat">Status of variable (see enumerated constants)</param>
        public int gmoGetSolutionVarRec(int sj, ref double vl, ref double vmarg, ref int vstat, ref int vcstat)
        {
            return dll_gmoGetSolutionVarRec(pgmo, sj, ref vl, ref vmarg, ref vstat, ref vcstat);
        }
        /// <summary>
        /// Set variable solution values (level, marginals and statuses)
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="vl">Level of variable</param>
        /// <param name="vmarg">Marginal of variable</param>
        /// <param name="vstat">Basis status of variable (see enumerated constants)</param>
        /// <param name="vcstat">Status of variable (see enumerated constants)</param>
        public int gmoSetSolutionVarRec(int sj, double vl, double vmarg, int vstat, int vcstat)
        {
            return dll_gmoSetSolutionVarRec(pgmo, sj, vl, vmarg, vstat, vcstat);
        }
        /// <summary>
        /// Get equation solution values (level, marginals and statuses)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="el">Level of equation</param>
        /// <param name="emarg">Marginal of equation</param>
        /// <param name="estat">Basis status of equation (see enumerated constants)</param>
        /// <param name="ecstat">Status of equation (see enumerated constants)</param>
        public int gmoGetSolutionEquRec(int si, ref double el, ref double emarg, ref int estat, ref int ecstat)
        {
            return dll_gmoGetSolutionEquRec(pgmo, si, ref el, ref emarg, ref estat, ref ecstat);
        }
        /// <summary>
        /// Set equation solution values (level, marginals and statuses)
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="el">Level of equation</param>
        /// <param name="emarg">Marginal of equation</param>
        /// <param name="estat">Basis status of equation (see enumerated constants)</param>
        /// <param name="ecstat">Status of equation (see enumerated constants)</param>
        public int gmoSetSolutionEquRec(int si, double el, double emarg, int estat, int ecstat)
        {
            return dll_gmoSetSolutionEquRec(pgmo, si, el, emarg, estat, ecstat);
        }
        /// <summary>
        /// Set solution values sfor variable and equation statuses
        /// </summary>
        /// <param name="xb">Basis statuses of variables (see enumerated constants)</param>
        /// <param name="xs">Statuses of variables (see enumerated constants)</param>
        /// <param name="yb">Basis statuses of equations (see enumerated constants)</param>
        /// <param name="ys">Statuses of equation (see enumerated constants)</param>
        public int gmoSetSolutionStatus(ref int[] xb, ref int[] xs, ref int[] yb, ref int[] ys)
        {
            return dll_gmoSetSolutionStatus(pgmo, xb, xs, yb, ys);
        }
        /// <summary>
        /// Complete objective row/col for models with objective function
        /// </summary>
        /// <param name="locobjval">Objective value</param>
        public void gmoCompleteObjective(double locobjval)
        {
            dll_gmoCompleteObjective(pgmo, locobjval);
        }
        /// <summary>
        /// Complete solution (e.g. for cols/rows not in view)
        /// </summary>
        public int gmoCompleteSolution()
        {
            return dll_gmoCompleteSolution(pgmo);
        }
        /// <summary>
        /// Compute absolute gap w.r.t. objective value and objective estimate in head or tail records
        /// </summary>
        public double gmoGetAbsoluteGap()
        {
            return dll_gmoGetAbsoluteGap(pgmo);
        }
        /// <summary>
        /// Compute relative gap w.r.t. objective value and objective estimate in head or tail records
        /// </summary>
        public double gmoGetRelativeGap()
        {
            return dll_gmoGetRelativeGap(pgmo);
        }
        /// <summary>
        /// Load solution from legacy solution file
        /// </summary>
        public int gmoLoadSolutionLegacy()
        {
            return dll_gmoLoadSolutionLegacy(pgmo);
        }
        /// <summary>
        /// Unload solution to legacy solution file
        /// </summary>
        public int gmoUnloadSolutionLegacy()
        {
            return dll_gmoUnloadSolutionLegacy(pgmo);
        }
        /// <summary>
        /// Load solution to GDX solution file (optional: rows, cols and-or header and tail info)
        /// </summary>
        /// <param name="gdxfname">Name of GDX file</param>
        /// <param name="dorows">Flag whether to read/write row information from/to solution file</param>
        /// <param name="docols">Flag whether to read/write column information from/to solution file</param>
        /// <param name="doht">Flag whether to read/write head and tail information from/to solution file</param>
        public int gmoLoadSolutionGDX(string gdxfname, bool dorows, bool docols, bool doht)
        {
            int ib_dorows = 0;
            int ib_docols = 0;
            int ib_doht = 0;
            if (dorows) ib_dorows = 1;
            if (docols) ib_docols = 1;
            if (doht) ib_doht = 1;
            return dll_gmoLoadSolutionGDX(pgmo, gdxfname, ib_dorows, ib_docols, ib_doht);
        }
        /// <summary>
        /// Unload solution to GDX solution file (optional: rows, cols and-or header and tail info)
        /// </summary>
        /// <param name="gdxfname">Name of GDX file</param>
        /// <param name="dorows">Flag whether to read/write row information from/to solution file</param>
        /// <param name="docols">Flag whether to read/write column information from/to solution file</param>
        /// <param name="doht">Flag whether to read/write head and tail information from/to solution file</param>
        public int gmoUnloadSolutionGDX(string gdxfname, bool dorows, bool docols, bool doht)
        {
            int ib_dorows = 0;
            int ib_docols = 0;
            int ib_doht = 0;
            if (dorows) ib_dorows = 1;
            if (docols) ib_docols = 1;
            if (doht) ib_doht = 1;
            return dll_gmoUnloadSolutionGDX(pgmo, gdxfname, ib_dorows, ib_docols, ib_doht);
        }
        /// <summary>
        /// Initialize writing of multiple solutions (e.g. scenarios) to a GDX file
        /// </summary>
        /// <param name="gdxfname">Name of GDX file</param>
        /// <param name="scengdx">Pointer to GDX solution file containing multiple solutions, e.g. scenarios</param>
        /// <param name="dictid">GDX symbol number of dict</param>
        public int gmoPrepareAllSolToGDX(string gdxfname, IntPtr scengdx, int dictid)
        {
            return dll_gmoPrepareAllSolToGDX(pgmo, gdxfname, scengdx, dictid);
        }
        /// <summary>
        /// Add a solution (e.g. scenario) to the GDX file'
        /// </summary>
        /// <param name="scenuel">Scenario labels</param>
        public int gmoAddSolutionToGDX(string[] scenuel)
        {
            return dll_gmoAddSolutionToGDX(pgmo, scenuel);
        }
        /// <summary>
        /// Finalize writing of multiple solutions (e.g. scenarios) to a GDX file
        /// </summary>
        /// <param name="msg">Message</param>
        public int gmoWriteSolDone(ref string msg)
        {
            int rc_gmoWriteSolDone;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_gmoWriteSolDone = dll_gmoWriteSolDone(pgmo, cpy_msg);
            msg = cpy_msg.ToString();
            return rc_gmoWriteSolDone;
        }
        /// <summary>
        /// heck scenario UEL against dictionary uels and report number of varaible symbols
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="numsym">Number of symbols</param>
        public int gmoCheckSolPoolUEL(string prefix, ref int numsym)
        {
            return dll_gmoCheckSolPoolUEL(pgmo, prefix, ref numsym);
        }
        /// <summary>
        /// Prepare merged solution pool GDX file
        /// </summary>
        /// <param name="gdxfname">Name of GDX file</param>
        /// <param name="numsol">Number of solutions in solution pool</param>
        /// <param name="prefix"></param>
        public IntPtr gmoPrepareSolPoolMerge(string gdxfname, int numsol, string prefix)
        {
            return dll_gmoPrepareSolPoolMerge(pgmo, gdxfname, numsol, prefix);
        }
        /// <summary>
        /// Write solution to merged solution pool GDX file
        /// </summary>
        /// <param name="handle"></param>
        public int gmoPrepareSolPoolNextSym(IntPtr handle)
        {
            return dll_gmoPrepareSolPoolNextSym(pgmo, handle);
        }
        /// <summary>
        /// Write solution to merged solution pool GDX file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="numsol">Number of solutions in solution pool</param>
        public int gmoUnloadSolPoolSolution(IntPtr handle, int numsol)
        {
            return dll_gmoUnloadSolPoolSolution(pgmo, handle, numsol);
        }
        /// <summary>
        /// Finalize merged solution pool GDX file
        /// </summary>
        /// <param name="handle"></param>
        public int gmoFinalizeSolPoolMerge(IntPtr handle)
        {
            return dll_gmoFinalizeSolPoolMerge(pgmo, handle);
        }
        /// <summary>
        /// String for variable type
        /// </summary>
        /// <param name="sj">Index of column in client space</param>
        /// <param name="s">String</param>
        public int gmoGetVarTypeTxt(int sj, ref string s)
        {
            int rc_gmoGetVarTypeTxt;
            StringBuilder cpy_s = new StringBuilder(gamsglobals.str_len);
            rc_gmoGetVarTypeTxt = dll_gmoGetVarTypeTxt(pgmo, sj, cpy_s);
            s = cpy_s.ToString();
            return rc_gmoGetVarTypeTxt;
        }
        /// <summary>
        /// String for equation type
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="s">String</param>
        public int gmoGetEquTypeTxt(int si, ref string s)
        {
            int rc_gmoGetEquTypeTxt;
            StringBuilder cpy_s = new StringBuilder(gamsglobals.str_len);
            rc_gmoGetEquTypeTxt = dll_gmoGetEquTypeTxt(pgmo, si, cpy_s);
            s = cpy_s.ToString();
            return rc_gmoGetEquTypeTxt;
        }
        /// <summary>
        /// String for solvestatus
        /// </summary>
        /// <param name="solvestat">Solver status</param>
        /// <param name="s">String</param>
        public int gmoGetSolveStatusTxt(int solvestat, ref string s)
        {
            int rc_gmoGetSolveStatusTxt;
            StringBuilder cpy_s = new StringBuilder(gamsglobals.str_len);
            rc_gmoGetSolveStatusTxt = dll_gmoGetSolveStatusTxt(pgmo, solvestat, cpy_s);
            s = cpy_s.ToString();
            return rc_gmoGetSolveStatusTxt;
        }
        /// <summary>
        /// String for modelstatus
        /// </summary>
        /// <param name="modelstat">Model status</param>
        /// <param name="s">String</param>
        public int gmoGetModelStatusTxt(int modelstat, ref string s)
        {
            int rc_gmoGetModelStatusTxt;
            StringBuilder cpy_s = new StringBuilder(gamsglobals.str_len);
            rc_gmoGetModelStatusTxt = dll_gmoGetModelStatusTxt(pgmo, modelstat, cpy_s);
            s = cpy_s.ToString();
            return rc_gmoGetModelStatusTxt;
        }
        /// <summary>
        /// String for modeltype
        /// </summary>
        /// <param name="modeltype"></param>
        /// <param name="s">String</param>
        public int gmoGetModelTypeTxt(int modeltype, ref string s)
        {
            int rc_gmoGetModelTypeTxt;
            StringBuilder cpy_s = new StringBuilder(gamsglobals.str_len);
            rc_gmoGetModelTypeTxt = dll_gmoGetModelTypeTxt(pgmo, modeltype, cpy_s);
            s = cpy_s.ToString();
            return rc_gmoGetModelTypeTxt;
        }
        /// <summary>
        /// String for solution head or tail record
        /// </summary>
        /// <param name="htrec">Solution head or tail record, (see enumerated constants)</param>
        /// <param name="s">String</param>
        public int gmoGetHeadNTailTxt(int htrec, ref string s)
        {
            int rc_gmoGetHeadNTailTxt;
            StringBuilder cpy_s = new StringBuilder(gamsglobals.str_len);
            rc_gmoGetHeadNTailTxt = dll_gmoGetHeadNTailTxt(pgmo, htrec, cpy_s);
            s = cpy_s.ToString();
            return rc_gmoGetHeadNTailTxt;
        }
        /// <summary>
        /// Get current memory consumption of GMO in MB
        /// </summary>
        public double gmoMemUsed()
        {
            return dll_gmoMemUsed(pgmo);
        }
        /// <summary>
        /// Get peak memory consumption of GMO in MB
        /// </summary>
        public double gmoPeakMemUsed()
        {
            return dll_gmoPeakMemUsed(pgmo);
        }
        /// <summary>
        /// Set NL Object and constant pool
        /// </summary>
        /// <param name="nlobject">Object of nonlinear instructions</param>
        /// <param name="nlpool">Constant pool object for constants in nonlinear instruction</param>
        public int gmoSetNLObject(IntPtr nlobject, IntPtr nlpool)
        {
            return dll_gmoSetNLObject(pgmo, nlobject, nlpool);
        }
        /// <summary>
        /// Dump QMaker GDX File
        /// </summary>
        /// <param name="gdxfname">Name of GDX file</param>
        public int gmoDumpQMakerGDX(string gdxfname)
        {
            return dll_gmoDumpQMakerGDX(pgmo, gdxfname);
        }
        /// <summary>
        /// Get variable equation mapping list
        /// </summary>
        /// <param name="maptype">Type of variable equation mapping</param>
        /// <param name="optptr">Option object pointer</param>
        /// <param name="strict"></param>
        /// <param name="nmappings"></param>
        /// <param name="rowindex"></param>
        /// <param name="colindex"></param>
        /// <param name="mapval"></param>
        public int gmoGetVarEquMap(int maptype, IntPtr optptr, int strict, ref int nmappings, ref int[] rowindex, ref int[] colindex, ref int[] mapval)
        {
            return dll_gmoGetVarEquMap(pgmo, maptype, optptr, strict, ref nmappings, rowindex, colindex, mapval);
        }
        /// <summary>
        /// Get indicator constraint list
        /// </summary>
        /// <param name="optptr">Option object pointer</param>
        /// <param name="indicstrict">1: Make the indicator reading strict. 0: accept duplicates, unmatched vars and equs, etc</param>
        /// <param name="numindic">Number of indicator constraints</param>
        /// <param name="rowindic">map with row indicies</param>
        /// <param name="colindic">map with column indicies</param>
        /// <param name="indiconval">0 or 1 value for binary variable to activate the constraint</param>
        public int gmoGetIndicatorMap(IntPtr optptr, int indicstrict, ref int numindic, ref int[] rowindic, ref int[] colindic, ref int[] indiconval)
        {
            return dll_gmoGetIndicatorMap(pgmo, optptr, indicstrict, ref numindic, rowindic, colindic, indiconval);
        }
        /// <summary>
        /// mature = 0 ... 100 = crude/not secure evaluations (non-GAMS evaluators)
        /// </summary>
        public int gmoCrudeness()
        {
            return dll_gmoCrudeness(pgmo);
        }
        /// <summary>
        /// Temporary function to get row function only code, do not use it
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="len">Length of array</param>
        /// <param name="opcode">Nonlinear code operation</param>
        /// <param name="field">Nonlinear code field</param>
        public int gmoDirtyGetRowFNLInstr(int si, ref int len, ref int[] opcode, ref int[] field)
        {
            return dll_gmoDirtyGetRowFNLInstr(pgmo, si, ref len, opcode, field);
        }
        /// <summary>
        /// Temporary function to get row function only code, do not use it
        /// </summary>
        /// <param name="len">Length of array</param>
        /// <param name="opcode">Nonlinear code operation</param>
        /// <param name="field">Nonlinear code field</param>
        public int gmoDirtyGetObjFNLInstr(ref int len, ref int[] opcode, ref int[] field)
        {
            return dll_gmoDirtyGetObjFNLInstr(pgmo, ref len, opcode, field);
        }
        /// <summary>
        /// Temporary function to set row function only code, do not use it
        /// </summary>
        /// <param name="si">Index of row in client space</param>
        /// <param name="len">Length of array</param>
        /// <param name="opcode">Nonlinear code operation</param>
        /// <param name="field">Nonlinear code field</param>
        /// <param name="nlpool">Constant pool object for constants in nonlinear instruction</param>
        /// <param name="nlpoolvec">Constant pool array for constants in nonlinear instruction</param>
        /// <param name="len2">Length of second array</param>
        public int gmoDirtySetRowFNLInstr(int si, int len, int[] opcode, int[] field, IntPtr nlpool, ref double[] nlpoolvec, int len2)
        {
            return dll_gmoDirtySetRowFNLInstr(pgmo, si, len, opcode, field, nlpool, nlpoolvec, len2);
        }
        /// <summary>
        /// Get file name stub of extrinsic function library
        /// </summary>
        /// <param name="libidx">Library index</param>
        public string gmoGetExtrLibName(int libidx)
        {
            string rc_gmoGetExtrLibName = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoGetExtrLibName(pgmo, libidx, sst_result);
            rc_gmoGetExtrLibName = sst_result.ToString();
            return rc_gmoGetExtrLibName;
        }
        /// <summary>
        /// Get data object pointer of extrinsic function library
        /// </summary>
        /// <param name="libidx">Library index</param>
        public IntPtr gmoGetExtrLibObjPtr(int libidx)
        {
            return dll_gmoGetExtrLibObjPtr(pgmo, libidx);
        }
        /// <summary>
        /// Get name of extrinsic function
        /// </summary>
        /// <param name="libidx">Library index</param>
        /// <param name="funcidx">Function index</param>
        public string gmoGetExtrLibFuncName(int libidx, int funcidx)
        {
            string rc_gmoGetExtrLibFuncName = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoGetExtrLibFuncName(pgmo, libidx, funcidx, sst_result);
            rc_gmoGetExtrLibFuncName = sst_result.ToString();
            return rc_gmoGetExtrLibFuncName;
        }
        /// <summary>
        /// Load a function from an extrinsic function library
        /// </summary>
        /// <param name="libidx">Library index</param>
        /// <param name="name"></param>
        /// <param name="msg">Message</param>
        public IntPtr gmoLoadExtrLibEntry(int libidx, string name, ref string msg)
        {
            IntPtr rc_gmoLoadExtrLibEntry;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_gmoLoadExtrLibEntry = dll_gmoLoadExtrLibEntry(pgmo, libidx, name, cpy_msg);
            msg = cpy_msg.ToString();
            return rc_gmoLoadExtrLibEntry;
        }

        /// <summary>
        /// Load GAMS dictionary object and obtain pointer to it
        /// </summary>
        public IntPtr gmoDict()
        {
            return dll_gmoDict(pgmo);
        }
        /// <summary>
        /// Load GAMS dictionary object and obtain pointer to it
        /// </summary>
        public void gmoDictSet(IntPtr x)
        {
            dll_gmoDictSet(pgmo, x);
        }
        /// <summary>
        /// Name of model
        /// </summary>
        public string gmoNameModel()
        {
            string tmp_result = "";
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoNameModel(pgmo, sst_result);
            tmp_result = sst_result.ToString();
            return tmp_result;
        }
        /// <summary>
        /// Name of model
        /// </summary>
        public void gmoNameModelSet(string x)
        {
            dll_gmoNameModelSet(pgmo, x);
        }
        /// <summary>
        /// Sequence number of model (0..n)
        /// </summary>
        public int gmoModelSeqNr()
        {
            return dll_gmoModelSeqNr(pgmo);
        }
        /// <summary>
        /// Sequence number of model (0..n)
        /// </summary>
        public void gmoModelSeqNrSet(int x)
        {
            dll_gmoModelSeqNrSet(pgmo, x);
        }
        /// <summary>
        /// Type of Model
        /// </summary>
        public int gmoModelType()
        {
            return dll_gmoModelType(pgmo);
        }
        /// <summary>
        /// Type of Model
        /// </summary>
        public void gmoModelTypeSet(int x)
        {
            dll_gmoModelTypeSet(pgmo, x);
        }
        /// <summary>
        /// Type of Model
        /// </summary>
        public bool gmoNLModelType()
        {
            return dll_gmoNLModelType(pgmo) != 0;
        }
        /// <summary>
        /// Direction of optimization, see enumerated constants
        /// </summary>
        public int gmoSense()
        {
            return dll_gmoSense(pgmo);
        }
        /// <summary>
        /// Direction of optimization, see enumerated constants
        /// </summary>
        public void gmoSenseSet(int x)
        {
            dll_gmoSenseSet(pgmo, x);
        }
        /// <summary>
        /// Is this a QP or not
        /// </summary>
        public bool gmoIsQP()
        {
            return dll_gmoIsQP(pgmo) != 0;
        }
        /// <summary>
        /// Number of option file
        /// </summary>
        public int gmoOptFile()
        {
            return dll_gmoOptFile(pgmo);
        }
        /// <summary>
        /// Number of option file
        /// </summary>
        public void gmoOptFileSet(int x)
        {
            dll_gmoOptFileSet(pgmo, x);
        }
        /// <summary>
        /// Dictionary flag
        /// </summary>
        public int gmoDictionary()
        {
            return dll_gmoDictionary(pgmo);
        }
        /// <summary>
        /// Dictionary flag
        /// </summary>
        public void gmoDictionarySet(int x)
        {
            dll_gmoDictionarySet(pgmo, x);
        }
        /// <summary>
        /// Scaling flag
        /// </summary>
        public int gmoScaleOpt()
        {
            return dll_gmoScaleOpt(pgmo);
        }
        /// <summary>
        /// Scaling flag
        /// </summary>
        public void gmoScaleOptSet(int x)
        {
            dll_gmoScaleOptSet(pgmo, x);
        }
        /// <summary>
        /// Priority Flag
        /// </summary>
        public int gmoPriorOpt()
        {
            return dll_gmoPriorOpt(pgmo);
        }
        /// <summary>
        /// Priority Flag
        /// </summary>
        public void gmoPriorOptSet(int x)
        {
            dll_gmoPriorOptSet(pgmo, x);
        }
        /// <summary>
        /// Do we have basis
        /// </summary>
        public int gmoHaveBasis()
        {
            return dll_gmoHaveBasis(pgmo);
        }
        /// <summary>
        /// Do we have basis
        /// </summary>
        public void gmoHaveBasisSet(int x)
        {
            dll_gmoHaveBasisSet(pgmo, x);
        }
        /// <summary>
        /// Model status, see enumerated constants
        /// </summary>
        public int gmoModelStat()
        {
            return dll_gmoModelStat(pgmo);
        }
        /// <summary>
        /// Model status, see enumerated constants
        /// </summary>
        public void gmoModelStatSet(int x)
        {
            dll_gmoModelStatSet(pgmo, x);
        }
        /// <summary>
        /// Solver status, see enumerated constants
        /// </summary>
        public int gmoSolveStat()
        {
            return dll_gmoSolveStat(pgmo);
        }
        /// <summary>
        /// Solver status, see enumerated constants
        /// </summary>
        public void gmoSolveStatSet(int x)
        {
            dll_gmoSolveStatSet(pgmo, x);
        }
        /// <summary>
        /// Is this an MPSGE model
        /// </summary>
        public bool gmoIsMPSGE()
        {
            return dll_gmoIsMPSGE(pgmo) != 0;
        }
        /// <summary>
        /// Is this an MPSGE model
        /// </summary>
        public void gmoIsMPSGESet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoIsMPSGESet(pgmo, ib_x);
        }
        /// <summary>
        /// Style of objective, see enumerated constants
        /// </summary>
        public int gmoObjStyle()
        {
            return dll_gmoObjStyle(pgmo);
        }
        /// <summary>
        /// Style of objective, see enumerated constants
        /// </summary>
        public void gmoObjStyleSet(int x)
        {
            dll_gmoObjStyleSet(pgmo, x);
        }
        /// <summary>
        /// Interface type (raw vs. processed), see enumerated constants
        /// </summary>
        public int gmoInterface()
        {
            return dll_gmoInterface(pgmo);
        }
        /// <summary>
        /// Interface type (raw vs. processed), see enumerated constants
        /// </summary>
        public void gmoInterfaceSet(int x)
        {
            dll_gmoInterfaceSet(pgmo, x);
        }
        /// <summary>
        /// User array index base (0 or 1
        /// </summary>
        public int gmoIndexBase()
        {
            return dll_gmoIndexBase(pgmo);
        }
        /// <summary>
        /// User array index base (0 or 1
        /// </summary>
        public void gmoIndexBaseSet(int x)
        {
            dll_gmoIndexBaseSet(pgmo, x);
        }
        /// <summary>
        /// Reformulate objective if possibl
        /// </summary>
        public bool gmoObjReform()
        {
            return dll_gmoObjReform(pgmo) != 0;
        }
        /// <summary>
        /// Reformulate objective if possibl
        /// </summary>
        public void gmoObjReformSet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoObjReformSet(pgmo, ib_x);
        }
        /// <summary>
        /// Reformulate objective even if objective variable is the only variabl
        /// </summary>
        public bool gmoEmptyOut()
        {
            return dll_gmoEmptyOut(pgmo) != 0;
        }
        /// <summary>
        /// Reformulate objective even if objective variable is the only variabl
        /// </summary>
        public void gmoEmptyOutSet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoEmptyOutSet(pgmo, ib_x);
        }
        /// <summary>
        /// Consider constant derivatives in external functions or no
        /// </summary>
        public bool gmoIgnXCDeriv()
        {
            return dll_gmoIgnXCDeriv(pgmo) != 0;
        }
        /// <summary>
        /// Consider constant derivatives in external functions or no
        /// </summary>
        public void gmoIgnXCDerivSet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoIgnXCDerivSet(pgmo, ib_x);
        }
        /// <summary>
        /// Toggle Q-mode
        /// </summary>
        public bool gmoUseQ()
        {
            return dll_gmoUseQ(pgmo) != 0;
        }
        /// <summary>
        /// Toggle Q-mode
        /// </summary>
        public void gmoUseQSet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoUseQSet(pgmo, ib_x);
        }
        /// <summary>
        /// Choose Q extraction algorithm (must be set before UseQ; 0: automatic; 1: ThreePass; 2: DoubleForward)
        /// </summary>
        public int gmoQExtractAlg()
        {
            return dll_gmoQExtractAlg(pgmo);
        }
        /// <summary>
        /// Choose Q extraction algorithm (must be set before UseQ; 0: automatic; 1: ThreePass; 2: DoubleForward)
        /// </summary>
        public void gmoQExtractAlgSet(int x)
        {
            dll_gmoQExtractAlgSet(pgmo, x);
        }
        /// <summary>
        /// Use alternative bound
        /// </summary>
        public bool gmoAltBounds()
        {
            return dll_gmoAltBounds(pgmo) != 0;
        }
        /// <summary>
        /// Use alternative bound
        /// </summary>
        public void gmoAltBoundsSet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoAltBoundsSet(pgmo, ib_x);
        }
        /// <summary>
        /// Use alternative RH
        /// </summary>
        public bool gmoAltRHS()
        {
            return dll_gmoAltRHS(pgmo) != 0;
        }
        /// <summary>
        /// Use alternative RH
        /// </summary>
        public void gmoAltRHSSet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoAltRHSSet(pgmo, ib_x);
        }
        /// <summary>
        /// Use alternative variable type
        /// </summary>
        public bool gmoAltVarTypes()
        {
            return dll_gmoAltVarTypes(pgmo) != 0;
        }
        /// <summary>
        /// Use alternative variable type
        /// </summary>
        public void gmoAltVarTypesSet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoAltVarTypesSet(pgmo, ib_x);
        }
        /// <summary>
        /// Force linear representation of mode
        /// </summary>
        public bool gmoForceLinear()
        {
            return dll_gmoForceLinear(pgmo) != 0;
        }
        /// <summary>
        /// Force linear representation of mode
        /// </summary>
        public void gmoForceLinearSet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoForceLinearSet(pgmo, ib_x);
        }
        /// <summary>
        /// Force continuous relaxation of mode
        /// </summary>
        public bool gmoForceCont()
        {
            return dll_gmoForceCont(pgmo) != 0;
        }
        /// <summary>
        /// Force continuous relaxation of mode
        /// </summary>
        public void gmoForceContSet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoForceContSet(pgmo, ib_x);
        }
        /// <summary>
        /// Column permutation fla
        /// </summary>
        public bool gmoPermuteCols()
        {
            return dll_gmoPermuteCols(pgmo) != 0;
        }
        /// <summary>
        /// Column permutation fla
        /// </summary>
        public void gmoPermuteColsSet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoPermuteColsSet(pgmo, ib_x);
        }
        /// <summary>
        /// Row permutation fla
        /// </summary>
        public bool gmoPermuteRows()
        {
            return dll_gmoPermuteRows(pgmo) != 0;
        }
        /// <summary>
        /// Row permutation fla
        /// </summary>
        public void gmoPermuteRowsSet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoPermuteRowsSet(pgmo, ib_x);
        }
        /// <summary>
        /// Value for plus infinit
        /// </summary>
        public double gmoPinf()
        {
            return dll_gmoPinf(pgmo);
        }
        /// <summary>
        /// Value for plus infinit
        /// </summary>
        public void gmoPinfSet(double x)
        {
            dll_gmoPinfSet(pgmo, x);
        }
        /// <summary>
        /// Value for minus infinit
        /// </summary>
        public double gmoMinf()
        {
            return dll_gmoMinf(pgmo);
        }
        /// <summary>
        /// Value for minus infinit
        /// </summary>
        public void gmoMinfSet(double x)
        {
            dll_gmoMinfSet(pgmo, x);
        }
        /// <summary>
        /// quiet IEEE NaN
        /// </summary>
        public double gmoQNaN()
        {
            return dll_gmoQNaN(pgmo);
        }
        /// <summary>
        /// Double Value of N/A
        /// </summary>
        public double gmoValNA()
        {
            return dll_gmoValNA(pgmo);
        }
        /// <summary>
        /// Integer Value of N/A
        /// </summary>
        public int gmoValNAInt()
        {
            return dll_gmoValNAInt(pgmo);
        }
        public double gmoValUndf()
        {
            return dll_gmoValUndf(pgmo);
        }
        /// <summary>
        /// Number of rows
        /// </summary>
        public int gmoM()
        {
            return dll_gmoM(pgmo);
        }
        /// <summary>
        /// Number of quadratic rows (-1 if Q information not used)
        /// </summary>
        public int gmoQM()
        {
            return dll_gmoQM(pgmo);
        }
        /// <summary>
        /// Number of nonlinear rows
        /// </summary>
        public int gmoNLM()
        {
            return dll_gmoNLM(pgmo);
        }
        /// <summary>
        /// Number of matched rows
        /// </summary>
        public int gmoNRowMatch()
        {
            return dll_gmoNRowMatch(pgmo);
        }
        /// <summary>
        /// Number of columns
        /// </summary>
        public int gmoN()
        {
            return dll_gmoN(pgmo);
        }
        /// <summary>
        /// Number of nonlinear columns
        /// </summary>
        public int gmoNLN()
        {
            return dll_gmoNLN(pgmo);
        }
        /// <summary>
        /// Number of discontinuous column
        /// </summary>
        public int gmoNDisc()
        {
            return dll_gmoNDisc(pgmo);
        }
        /// <summary>
        /// Number of fixed column
        /// </summary>
        public int gmoNFixed()
        {
            return dll_gmoNFixed(pgmo);
        }
        /// <summary>
        /// Number of matched column
        /// </summary>
        public int gmoNColMatch()
        {
            return dll_gmoNColMatch(pgmo);
        }
        /// <summary>
        /// Number of nonzeros in Jacobian matrix
        /// </summary>
        public int gmoNZ()
        {
            return dll_gmoNZ(pgmo);
        }
        /// <summary>
        /// Number of nonzeros in Jacobian matrix
        /// </summary>
        public Int64 gmoNZ64()
        {
            return dll_gmoNZ64(pgmo);
        }
        /// <summary>
        /// Number of nonlinear nonzeros in Jacobian matrix
        /// </summary>
        public int gmoNLNZ()
        {
            return dll_gmoNLNZ(pgmo);
        }
        /// <summary>
        /// Number of nonlinear nonzeros in Jacobian matrix
        /// </summary>
        public Int64 gmoNLNZ64()
        {
            return dll_gmoNLNZ64(pgmo);
        }
        /// <summary>
        /// Number of linear nonzeros in Jacobian matrix
        /// </summary>
        public int gmoLNZEx()
        {
            return dll_gmoLNZEx(pgmo);
        }
        /// <summary>
        /// Number of linear nonzeros in Jacobian matrix
        /// </summary>
        public Int64 gmoLNZEx64()
        {
            return dll_gmoLNZEx64(pgmo);
        }
        /// <summary>
        /// Legacy overestimate for the count of linear nonzeros in Jacobian matrix, especially if gmoUseQ is true
        /// </summary>
        public int gmoLNZ()
        {
            return dll_gmoLNZ(pgmo);
        }
        /// <summary>
        /// Legacy overestimate for the count of linear nonzeros in Jacobian matrix, especially if gmoUseQ is true
        /// </summary>
        public Int64 gmoLNZ64()
        {
            return dll_gmoLNZ64(pgmo);
        }
        /// <summary>
        /// Number of quadratic nonzeros in Jacobian matrix, 0 if gmoUseQ is false
        /// </summary>
        public int gmoQNZ()
        {
            return dll_gmoQNZ(pgmo);
        }
        /// <summary>
        /// Number of general nonlinear nonzeros in Jacobian matrix, equals gmoNLNZ if gmoUseQ is false
        /// </summary>
        public int gmoGNLNZ()
        {
            return dll_gmoGNLNZ(pgmo);
        }
        /// <summary>
        /// Maximum number of nonzeros in single Q matrix (-1 on overflow)
        /// </summary>
        public int gmoMaxQNZ()
        {
            return dll_gmoMaxQNZ(pgmo);
        }
        /// <summary>
        /// Maximum number of nonzeros in single Q matrix
        /// </summary>
        public Int64 gmoMaxQNZ64()
        {
            return dll_gmoMaxQNZ64(pgmo);
        }
        /// <summary>
        /// Number of nonzeros in objective gradient
        /// </summary>
        public int gmoObjNZ()
        {
            return dll_gmoObjNZ(pgmo);
        }
        /// <summary>
        /// Number of linear nonzeros in objective gradient
        /// </summary>
        public int gmoObjLNZ()
        {
            return dll_gmoObjLNZ(pgmo);
        }
        /// <summary>
        /// Number of GMOORDER_Q nonzeros in objective gradient
        /// </summary>
        public int gmoObjQNZEx()
        {
            return dll_gmoObjQNZEx(pgmo);
        }
        /// <summary>
        /// Number of nonlinear nonzeros in objective gradient
        /// </summary>
        public int gmoObjNLNZ()
        {
            return dll_gmoObjNLNZ(pgmo);
        }
        /// <summary>
        /// Number of GMOORDER_NL nonzeros in objective gradient
        /// </summary>
        public int gmoObjNLNZEx()
        {
            return dll_gmoObjNLNZEx(pgmo);
        }
        /// <summary>
        /// Number of nonzeros in lower triangle of Q matrix of objective (-1 if useQ false or overflow)
        /// </summary>
        public int gmoObjQMatNZ()
        {
            return dll_gmoObjQMatNZ(pgmo);
        }
        /// <summary>
        /// Number of nonzeros in lower triangle of Q matrix of objective (-1 if useQ false)
        /// </summary>
        public Int64 gmoObjQMatNZ64()
        {
            return dll_gmoObjQMatNZ64(pgmo);
        }
        /// <summary>
        /// deprecated synonym for gmoObjQMatNZ
        /// </summary>
        public int gmoObjQNZ()
        {
            return dll_gmoObjQNZ(pgmo);
        }
        /// <summary>
        /// Number of nonzeros on diagonal of Q matrix of objective (-1 if useQ false)
        /// </summary>
        public int gmoObjQDiagNZ()
        {
            return dll_gmoObjQDiagNZ(pgmo);
        }
        /// <summary>
        /// Number of nonzeros in c vector of objective (-1 if Q information not used)
        /// </summary>
        public int gmoObjCVecNZ()
        {
            return dll_gmoObjCVecNZ(pgmo);
        }
        /// <summary>
        /// Length of constant pool in nonlinear code
        /// </summary>
        public int gmoNLConst()
        {
            return dll_gmoNLConst(pgmo);
        }
        /// <summary>
        /// Length of constant pool in nonlinear code
        /// </summary>
        public void gmoNLConstSet(int x)
        {
            dll_gmoNLConstSet(pgmo, x);
        }
        /// <summary>
        /// Nonlinear code siz
        /// </summary>
        public int gmoNLCodeSize()
        {
            return dll_gmoNLCodeSize(pgmo);
        }
        /// <summary>
        /// Nonlinear code siz
        /// </summary>
        public void gmoNLCodeSizeSet(int x)
        {
            dll_gmoNLCodeSizeSet(pgmo, x);
        }
        /// <summary>
        /// Maximum nonlinear code size for row
        /// </summary>
        public int gmoNLCodeSizeMaxRow()
        {
            return dll_gmoNLCodeSizeMaxRow(pgmo);
        }
        /// <summary>
        /// Index of objective variable
        /// </summary>
        public int gmoObjVar()
        {
            return dll_gmoObjVar(pgmo);
        }
        /// <summary>
        /// Index of objective variable
        /// </summary>
        public void gmoObjVarSet(int x)
        {
            dll_gmoObjVarSet(pgmo, x);
        }
        /// <summary>
        /// Index of objective row
        /// </summary>
        public int gmoObjRow()
        {
            return dll_gmoObjRow(pgmo);
        }
        /// <summary>
        /// Order of Objective, see enumerated constants
        /// </summary>
        public int gmoGetObjOrder()
        {
            return dll_gmoGetObjOrder(pgmo);
        }
        /// <summary>
        /// Objective constant
        /// </summary>
        public double gmoObjConst()
        {
            return dll_gmoObjConst(pgmo);
        }
        /// <summary>
        /// Objective constant - this is independent of useQ
        /// </summary>
        public double gmoObjConstEx()
        {
            return dll_gmoObjConstEx(pgmo);
        }
        /// <summary>
        /// Get constant in solvers quadratic objective
        /// </summary>
        public double gmoObjQConst()
        {
            return dll_gmoObjQConst(pgmo);
        }
        /// <summary>
        /// Value of Jacobian element of objective variable in objective
        /// </summary>
        public double gmoObjJacVal()
        {
            return dll_gmoObjJacVal(pgmo);
        }
        /// <summary>
        /// Method for returning on nonlinear evaluation errors
        /// </summary>
        public int gmoEvalErrorMethod()
        {
            return dll_gmoEvalErrorMethod(pgmo);
        }
        /// <summary>
        /// Method for returning on nonlinear evaluation errors
        /// </summary>
        public void gmoEvalErrorMethodSet(int x)
        {
            dll_gmoEvalErrorMethodSet(pgmo, x);
        }
        /// <summary>
        /// Maximum number of threads that can be used for evaluation
        /// </summary>
        public int gmoEvalMaxThreads()
        {
            return dll_gmoEvalMaxThreads(pgmo);
        }
        /// <summary>
        /// Maximum number of threads that can be used for evaluation
        /// </summary>
        public void gmoEvalMaxThreadsSet(int x)
        {
            dll_gmoEvalMaxThreadsSet(pgmo, x);
        }
        /// <summary>
        /// Number of function evaluations
        /// </summary>
        public int gmoEvalFuncCount()
        {
            return dll_gmoEvalFuncCount(pgmo);
        }
        /// <summary>
        /// Time used for function evaluations in s
        /// </summary>
        public double gmoEvalFuncTimeUsed()
        {
            return dll_gmoEvalFuncTimeUsed(pgmo);
        }
        /// <summary>
        /// Number of gradient evaluations
        /// </summary>
        public int gmoEvalGradCount()
        {
            return dll_gmoEvalGradCount(pgmo);
        }
        /// <summary>
        /// Time used for gradient evaluations in s
        /// </summary>
        public double gmoEvalGradTimeUsed()
        {
            return dll_gmoEvalGradTimeUsed(pgmo);
        }
        /// <summary>
        /// Maximum dimension of Hessian
        /// </summary>
        public int gmoHessMaxDim()
        {
            return dll_gmoHessMaxDim(pgmo);
        }
        /// <summary>
        /// Maximum number of nonzeros in Hessian
        /// </summary>
        public int gmoHessMaxNz()
        {
            return dll_gmoHessMaxNz(pgmo);
        }
        /// <summary>
        /// Dimension of Hessian of the Lagrangian
        /// </summary>
        public int gmoHessLagDim()
        {
            return dll_gmoHessLagDim(pgmo);
        }
        /// <summary>
        /// Nonzeros in Hessian of the Lagrangian
        /// </summary>
        public int gmoHessLagNz()
        {
            return dll_gmoHessLagNz(pgmo);
        }
        /// <summary>
        /// Nonzeros on Diagonal of Hessian of the Lagrangian
        /// </summary>
        public int gmoHessLagDiagNz()
        {
            return dll_gmoHessLagDiagNz(pgmo);
        }
        /// <summary>
        /// if useQ is true, still include GMOORDER_Q rows in the Hessian
        /// </summary>
        public bool gmoHessInclQRows()
        {
            return dll_gmoHessInclQRows(pgmo) != 0;
        }
        /// <summary>
        /// if useQ is true, still include GMOORDER_Q rows in the Hessian
        /// </summary>
        public void gmoHessInclQRowsSet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gmoHessInclQRowsSet(pgmo, ib_x);
        }
        /// <summary>
        /// EMP: Number of variational inequalities in model rim
        /// </summary>
        public int gmoNumVIFunc()
        {
            return dll_gmoNumVIFunc(pgmo);
        }
        /// <summary>
        /// EMP: Number of Agents/Followers
        /// </summary>
        public int gmoNumAgents()
        {
            return dll_gmoNumAgents(pgmo);
        }
        /// <summary>
        /// Name of option file
        /// </summary>
        public string gmoNameOptFile()
        {
            string tmp_result = "";
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoNameOptFile(pgmo, sst_result);
            tmp_result = sst_result.ToString();
            return tmp_result;
        }
        /// <summary>
        /// Name of option file
        /// </summary>
        public void gmoNameOptFileSet(string x)
        {
            dll_gmoNameOptFileSet(pgmo, x);
        }
        /// <summary>
        /// Name of solution file
        /// </summary>
        public string gmoNameSolFile()
        {
            string tmp_result = "";
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoNameSolFile(pgmo, sst_result);
            tmp_result = sst_result.ToString();
            return tmp_result;
        }
        /// <summary>
        /// Name of solution file
        /// </summary>
        public void gmoNameSolFileSet(string x)
        {
            dll_gmoNameSolFileSet(pgmo, x);
        }
        /// <summary>
        /// Name of external function library
        /// </summary>
        public string gmoNameXLib()
        {
            string tmp_result = "";
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoNameXLib(pgmo, sst_result);
            tmp_result = sst_result.ToString();
            return tmp_result;
        }
        /// <summary>
        /// Name of external function library
        /// </summary>
        public void gmoNameXLibSet(string x)
        {
            dll_gmoNameXLibSet(pgmo, x);
        }
        /// <summary>
        /// Name of matrix file
        /// </summary>
        public string gmoNameMatrix()
        {
            string tmp_result = "";
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoNameMatrix(pgmo, sst_result);
            tmp_result = sst_result.ToString();
            return tmp_result;
        }
        /// <summary>
        /// Name of dictionary file
        /// </summary>
        public string gmoNameDict()
        {
            string tmp_result = "";
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoNameDict(pgmo, sst_result);
            tmp_result = sst_result.ToString();
            return tmp_result;
        }
        /// <summary>
        /// Name of dictionary file
        /// </summary>
        public void gmoNameDictSet(string x)
        {
            dll_gmoNameDictSet(pgmo, x);
        }
        /// <summary>
        /// Name of input file (with .gms stripped)
        /// </summary>
        public string gmoNameInput()
        {
            string tmp_result = "";
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoNameInput(pgmo, sst_result);
            tmp_result = sst_result.ToString();
            return tmp_result;
        }
        /// <summary>
        /// Name of input file (with .gms stripped)
        /// </summary>
        public void gmoNameInputSet(string x)
        {
            dll_gmoNameInputSet(pgmo, x);
        }
        /// <summary>
        /// Name of output file (with .dat stripped)
        /// </summary>
        public string gmoNameOutput()
        {
            string tmp_result = "";
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoNameOutput(pgmo, sst_result);
            tmp_result = sst_result.ToString();
            return tmp_result;
        }
        /// <summary>
        /// Pointer to constant pool
        /// </summary>
        public IntPtr gmoPPool()
        {
            return dll_gmoPPool(pgmo);
        }
        /// <summary>
        /// IO mutex
        /// </summary>
        public IntPtr gmoIOMutex()
        {
            return dll_gmoIOMutex(pgmo);
        }
        /// <summary>
        /// Access to error indicator
        /// </summary>
        public int gmoError()
        {
            return dll_gmoError(pgmo);
        }
        /// <summary>
        /// Access to error indicator
        /// </summary>
        public void gmoErrorSet(int x)
        {
            dll_gmoErrorSet(pgmo, x);
        }
        /// <summary>
        /// Provide the last error message
        /// </summary>
        public string gmoErrorMessage()
        {
            string tmp_result = "";
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gmoErrorMessage(pgmo, sst_result);
            tmp_result = sst_result.ToString();
            return tmp_result;
        }
    }

    // C#  procedure wrapper generated by apiwrapper for GAMS Version 38.3.0
    //
    // GAMS - Loading mechanism for GAMS Expert-Level APIs
    //
    // Copyright (c) 2016-2022 GAMS Software GmbH <support@gams.com>
    // Copyright (c) 2016-2022 GAMS Development Corp. <support@gams.com>
    //
    // Permission is hereby granted, free of charge, to any person obtaining a copy
    // of this software and associated documentation files (the "Software"), to deal
    // in the Software without restriction, including without limitation the rights
    // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    // copies of the Software, and to permit persons to whom the Software is
    // furnished to do so, subject to the following conditions:
    //
    // The above copyright notice and this permission notice shall be included in all
    // copies or substantial portions of the Software.
    //
    // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    // SOFTWARE.

    //Got this from GAMS people, was omitted in the GAMS program folder
    internal class gevmcs : IDisposable
    {
        private IntPtr pgev;
        private bool extHandle;
        private bool _disposed;

#if __MonoCS__ || __APPLE__
    private delegate IntPtr DelLoadLibrary (string dllName, int flag);
    private delegate IntPtr DelGetProcAddress (IntPtr hModule, string procedureName);
    private delegate bool DelFreeLibrary (IntPtr hModul);

#if __APPLE__
    [DllImport("libdl.dylib")]
    internal static extern IntPtr dlopen(String dllname, int flags);

    [DllImport("libdl.dylib")]
    internal static extern IntPtr dlsym(IntPtr hModule, String procedureName);

    [DllImport("libdl.dylib")] //int
    internal static extern bool dlclose (IntPtr hModul);
#else
    [DllImport("libdl.so")]
    internal static extern IntPtr dlopen(String dllname, int flags);

    [DllImport("libdl.so")]
    internal static extern IntPtr dlsym(IntPtr hModule, String procedureName);

    [DllImport("libdl.so")]
    internal static extern bool dlclose (IntPtr hModul);
#endif

    DelLoadLibrary LoadLibrary = new DelLoadLibrary(dlopen);
    DelGetProcAddress GetProcAddress = new DelGetProcAddress (dlsym);
    DelFreeLibrary FreeLibrary = new DelFreeLibrary (dlclose);
#else
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
#endif


        public const int gevdoErr = 0;  // gevLogStatMode
        public const int gevdoStat = 1;
        public const int gevdoLog = 2;

        public const int gevSolverSameStreams = 0;  // gevCallSolverMode
        public const int gevSolverQuiet = 1;
        public const int gevSolverOwnFile = 2;

        public const int gevSolveLinkCallScript = 1;  // gevCallSolverSolveLink
        public const int gevSolveLinkCallModule = 2;
        public const int gevSolveLinkAsyncGrid = 3;
        public const int gevSolveLinkAsyncSimulate = 4;
        public const int gevSolveLinkLoadLibrary = 5;

        public const string gevPageWidth = "PageWidth";  // gevOptions
        public const string gevPageSize = "PageSize";
        public const string gevsubsysFile = "subsysFile";
        public const string gevNameScrDir = "NameScrDir";
        public const string gevNameSysDir = "NameSysDir";
        public const string gevNameCurDir = "NameCurDir";
        public const string gevNameWrkDir = "NameWrkDir";
        public const string gevLogOption = "LogOption";
        public const string gevNameLogFile = "NameLogFile";
        public const string gevNameCtrFile = "NameCtrFile";
        public const string gevNameMatrix = "NameMatrix";
        public const string gevNameInstr = "NameInstr";
        public const string gevNameStaFile = "NameStaFile";
        public const string gevlicenseFile = "licenseFile";
        public const string gevKeep = "Keep";
        public const string gevIDEFlag = "IDEFlag";
        public const string gevIterLim = "IterLim";
        public const string gevDomLim = "DomLim";
        public const string gevResLim = "ResLim";
        public const string gevOptCR = "OptCR";
        public const string gevOptCA = "OptCA";
        public const string gevSysOut = "SysOut";
        public const string gevNodeLim = "NodeLim";
        public const string gevWorkFactor = "WorkFactor";
        public const string gevWorkSpace = "WorkSpace";
        public const string gevSavePoint = "SavePoint";
        public const string gevHeapLimit = "HeapLimit";
        public const string gevNameScrExt = "NameScrExt";
        public const string gevInteger1 = "Integer1";
        public const string gevInteger2 = "Integer2";
        public const string gevInteger3 = "Integer3";
        public const string gevInteger4 = "Integer4";
        public const string gevInteger5 = "Integer5";
        public const string gevFDDelta = "FDDelta";
        public const string gevFDOpt = "FDOpt";
        public const string gevAlgFileType = "AlgFileType";
        public const string gevGamsVersion = "GamsVersion";
        public const string gevGenSolver = "GenSolver";
        public const string gevCurSolver = "CurSolver";
        public const string gevThreadsRaw = "ThreadsRaw";
        public const string gevUseCutOff = "UseCutOff";
        public const string gevUseCheat = "UseCheat";
        public const string gevNameGamsDate = "NameGamsDate";
        public const string gevNameGamsTime = "NameGamsTime";
        public const string gevLicense1 = "License1";
        public const string gevLicense2 = "License2";
        public const string gevLicense3 = "License3";
        public const string gevLicense4 = "License4";
        public const string gevLicense5 = "License5";
        public const string gevLicense6 = "License6";
        public const string gevNameParams = "NameParams";
        public const string gevNameScenFile = "NameScenFile";
        public const string gevNameExtFFile = "NameExtFFile";
        public const string gevisDefaultLicense = "isDefaultLicense";
        public const string gevisDefaultSubsys = "isDefaultSubsys";
        public const string gevCheat = "Cheat";
        public const string gevCutOff = "CutOff";
        public const string gevReal1 = "Real1";
        public const string gevReal2 = "Real2";
        public const string gevReal3 = "Real3";
        public const string gevReal4 = "Real4";
        public const string gevReal5 = "Real5";
        public const string gevReform = "Reform";
        public const string gevTryInt = "TryInt";
        public delegate void Tgevlswrite(string msg, int mode, IntPtr usrmem);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevRegisterWriteCallback_t(IntPtr pgev, Tgevlswrite lsw, int logenabled, IntPtr usrmem);
        private static gevRegisterWriteCallback_t dll_gevRegisterWriteCallback;
        private static void d_gevRegisterWriteCallback(IntPtr pgev, Tgevlswrite lsw, int logenabled, IntPtr usrmem)
        { gevErrorHandling("gevRegisterWriteCallback could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevCompleteEnvironment_t(IntPtr pgev, IntPtr palg, IntPtr ivec, IntPtr rvec, IntPtr svec);
        private static gevCompleteEnvironment_t dll_gevCompleteEnvironment;
        private static void d_gevCompleteEnvironment(IntPtr pgev, IntPtr palg, IntPtr ivec, IntPtr rvec, IntPtr svec)
        { gevErrorHandling("gevCompleteEnvironment could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevInitEnvironmentLegacy_t(IntPtr pgev, string cntrfn);
        private static gevInitEnvironmentLegacy_t dll_gevInitEnvironmentLegacy;
        private static int d_gevInitEnvironmentLegacy(IntPtr pgev, string cntrfn)
        { gevErrorHandling("gevInitEnvironmentLegacy could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevSwitchLogStat_t(IntPtr pgev, int lo, string logfn, int logappend, string statfn, int statappend, Tgevlswrite lsw, IntPtr usrmem, ref IntPtr lshandle);
        private static gevSwitchLogStat_t dll_gevSwitchLogStat;
        private static int d_gevSwitchLogStat(IntPtr pgev, int lo, string logfn, int logappend, string statfn, int statappend, Tgevlswrite lsw, IntPtr usrmem, ref IntPtr lshandle)
        { gevErrorHandling("gevSwitchLogStat could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr gevGetLShandle_t(IntPtr pgev);
        private static gevGetLShandle_t dll_gevGetLShandle;
        private static IntPtr d_gevGetLShandle(IntPtr pgev)
        { gevErrorHandling("gevGetLShandle could not be loaded"); return IntPtr.Zero; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevRestoreLogStat_t(IntPtr pgev, ref IntPtr lshandle);
        private static gevRestoreLogStat_t dll_gevRestoreLogStat;
        private static int d_gevRestoreLogStat(IntPtr pgev, ref IntPtr lshandle)
        { gevErrorHandling("gevRestoreLogStat could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevRestoreLogStatRewrite_t(IntPtr pgev, ref IntPtr lshandle);
        private static gevRestoreLogStatRewrite_t dll_gevRestoreLogStatRewrite;
        private static int d_gevRestoreLogStatRewrite(IntPtr pgev, ref IntPtr lshandle)
        { gevErrorHandling("gevRestoreLogStatRewrite could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevLog_t(IntPtr pgev, string s);
        private static gevLog_t dll_gevLog;
        private static void d_gevLog(IntPtr pgev, string s)
        { gevErrorHandling("gevLog could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevLogPChar_t(IntPtr pgev, byte[] p);
        private static gevLogPChar_t dll_gevLogPChar;
        private static void d_gevLogPChar(IntPtr pgev, byte[] p)
        { gevErrorHandling("gevLogPChar could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStat_t(IntPtr pgev, string s);
        private static gevStat_t dll_gevStat;
        private static void d_gevStat(IntPtr pgev, string s)
        { gevErrorHandling("gevStat could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatC_t(IntPtr pgev, string s);
        private static gevStatC_t dll_gevStatC;
        private static void d_gevStatC(IntPtr pgev, string s)
        { gevErrorHandling("gevStatC could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatPChar_t(IntPtr pgev, byte[] p);
        private static gevStatPChar_t dll_gevStatPChar;
        private static void d_gevStatPChar(IntPtr pgev, byte[] p)
        { gevErrorHandling("gevStatPChar could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatAudit_t(IntPtr pgev, string s);
        private static gevStatAudit_t dll_gevStatAudit;
        private static void d_gevStatAudit(IntPtr pgev, string s)
        { gevErrorHandling("gevStatAudit could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatCon_t(IntPtr pgev);
        private static gevStatCon_t dll_gevStatCon;
        private static void d_gevStatCon(IntPtr pgev)
        { gevErrorHandling("gevStatCon could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatCoff_t(IntPtr pgev);
        private static gevStatCoff_t dll_gevStatCoff;
        private static void d_gevStatCoff(IntPtr pgev)
        { gevErrorHandling("gevStatCoff could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatEOF_t(IntPtr pgev);
        private static gevStatEOF_t dll_gevStatEOF;
        private static void d_gevStatEOF(IntPtr pgev)
        { gevErrorHandling("gevStatEOF could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatSysout_t(IntPtr pgev);
        private static gevStatSysout_t dll_gevStatSysout;
        private static void d_gevStatSysout(IntPtr pgev)
        { gevErrorHandling("gevStatSysout could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatAddE_t(IntPtr pgev, int mi, string s);
        private static gevStatAddE_t dll_gevStatAddE;
        private static void d_gevStatAddE(IntPtr pgev, int mi, string s)
        { gevErrorHandling("gevStatAddE could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatAddV_t(IntPtr pgev, int mj, string s);
        private static gevStatAddV_t dll_gevStatAddV;
        private static void d_gevStatAddV(IntPtr pgev, int mj, string s)
        { gevErrorHandling("gevStatAddV could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatAddJ_t(IntPtr pgev, int mi, int mj, string s);
        private static gevStatAddJ_t dll_gevStatAddJ;
        private static void d_gevStatAddJ(IntPtr pgev, int mi, int mj, string s)
        { gevErrorHandling("gevStatAddJ could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatEject_t(IntPtr pgev);
        private static gevStatEject_t dll_gevStatEject;
        private static void d_gevStatEject(IntPtr pgev)
        { gevErrorHandling("gevStatEject could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatEdit_t(IntPtr pgev, char c);
        private static gevStatEdit_t dll_gevStatEdit;
        private static void d_gevStatEdit(IntPtr pgev, char c)
        { gevErrorHandling("gevStatEdit could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatE_t(IntPtr pgev, string s, int mi, string s2);
        private static gevStatE_t dll_gevStatE;
        private static void d_gevStatE(IntPtr pgev, string s, int mi, string s2)
        { gevErrorHandling("gevStatE could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatV_t(IntPtr pgev, string s, int mj, string s2);
        private static gevStatV_t dll_gevStatV;
        private static void d_gevStatV(IntPtr pgev, string s, int mj, string s2)
        { gevErrorHandling("gevStatV could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatT_t(IntPtr pgev);
        private static gevStatT_t dll_gevStatT;
        private static void d_gevStatT(IntPtr pgev)
        { gevErrorHandling("gevStatT could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatA_t(IntPtr pgev, string s);
        private static gevStatA_t dll_gevStatA;
        private static void d_gevStatA(IntPtr pgev, string s)
        { gevErrorHandling("gevStatA could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevStatB_t(IntPtr pgev, string s);
        private static gevStatB_t dll_gevStatB;
        private static void d_gevStatB(IntPtr pgev, string s)
        { gevErrorHandling("gevStatB could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevLogStat_t(IntPtr pgev, string s);
        private static gevLogStat_t dll_gevLogStat;
        private static void d_gevLogStat(IntPtr pgev, string s)
        { gevErrorHandling("gevLogStat could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevLogStatNoC_t(IntPtr pgev, string s);
        private static gevLogStatNoC_t dll_gevLogStatNoC;
        private static void d_gevLogStatNoC(IntPtr pgev, string s)
        { gevErrorHandling("gevLogStatNoC could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevLogStatPChar_t(IntPtr pgev, byte[] p);
        private static gevLogStatPChar_t dll_gevLogStatPChar;
        private static void d_gevLogStatPChar(IntPtr pgev, byte[] p)
        { gevErrorHandling("gevLogStatPChar could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevLogStatFlush_t(IntPtr pgev);
        private static gevLogStatFlush_t dll_gevLogStatFlush;
        private static void d_gevLogStatFlush(IntPtr pgev)
        { gevErrorHandling("gevLogStatFlush could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevGetAnchor_t(IntPtr pgev, string s, StringBuilder sst_result);
        private static gevGetAnchor_t dll_gevGetAnchor;
        private static void d_gevGetAnchor(IntPtr pgev, string s, StringBuilder sst_result)
        { gevErrorHandling("gevGetAnchor could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevLSTAnchor_t(IntPtr pgev, string s);
        private static gevLSTAnchor_t dll_gevLSTAnchor;
        private static void d_gevLSTAnchor(IntPtr pgev, string s)
        { gevErrorHandling("gevLSTAnchor could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevStatAppend_t(IntPtr pgev, string statfn, StringBuilder msg);
        private static gevStatAppend_t dll_gevStatAppend;
        private static int d_gevStatAppend(IntPtr pgev, string statfn, StringBuilder msg)
        { gevErrorHandling("gevStatAppend could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevMIPReport_t(IntPtr pgev, IntPtr gmoptr, double fixobj, int fixiter, double agap, double rgap);
        private static gevMIPReport_t dll_gevMIPReport;
        private static void d_gevMIPReport(IntPtr pgev, IntPtr gmoptr, double fixobj, int fixiter, double agap, double rgap)
        { gevErrorHandling("gevMIPReport could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevGetSlvExeInfo_t(IntPtr pgev, string solvername, StringBuilder exename);
        private static gevGetSlvExeInfo_t dll_gevGetSlvExeInfo;
        private static int d_gevGetSlvExeInfo(IntPtr pgev, string solvername, StringBuilder exename)
        { gevErrorHandling("gevGetSlvExeInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevGetSlvLibInfo_t(IntPtr pgev, string solvername, StringBuilder libname, StringBuilder prefix, ref int ifversion);
        private static gevGetSlvLibInfo_t dll_gevGetSlvLibInfo;
        private static int d_gevGetSlvLibInfo(IntPtr pgev, string solvername, StringBuilder libname, StringBuilder prefix, ref int ifversion)
        { gevErrorHandling("gevGetSlvLibInfo could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevCapabilityCheck_t(IntPtr pgev, int modeltype, string solvername, ref int capable);
        private static gevCapabilityCheck_t dll_gevCapabilityCheck;
        private static int d_gevCapabilityCheck(IntPtr pgev, int modeltype, string solvername, ref int capable)
        { gevErrorHandling("gevCapabilityCheck could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevSolverVisibility_t(IntPtr pgev, string solvername, ref int hidden, ref int defaultok);
        private static gevSolverVisibility_t dll_gevSolverVisibility;
        private static int d_gevSolverVisibility(IntPtr pgev, string solvername, ref int hidden, ref int defaultok)
        { gevErrorHandling("gevSolverVisibility could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevNumSolvers_t(IntPtr pgev);
        private static gevNumSolvers_t dll_gevNumSolvers;
        private static int d_gevNumSolvers(IntPtr pgev)
        { gevErrorHandling("gevNumSolvers could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevGetSolver_t(IntPtr pgev, int modeltype, StringBuilder sst_result);
        private static gevGetSolver_t dll_gevGetSolver;
        private static void d_gevGetSolver(IntPtr pgev, int modeltype, StringBuilder sst_result)
        { gevErrorHandling("gevGetSolver could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevGetCurrentSolver_t(IntPtr pgev, IntPtr gmoptr, StringBuilder sst_result);
        private static gevGetCurrentSolver_t dll_gevGetCurrentSolver;
        private static void d_gevGetCurrentSolver(IntPtr pgev, IntPtr gmoptr, StringBuilder sst_result)
        { gevErrorHandling("gevGetCurrentSolver could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevGetSolverDefault_t(IntPtr pgev, int modeltype, StringBuilder sst_result);
        private static gevGetSolverDefault_t dll_gevGetSolverDefault;
        private static void d_gevGetSolverDefault(IntPtr pgev, int modeltype, StringBuilder sst_result)
        { gevErrorHandling("gevGetSolverDefault could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevSolver2Id_t(IntPtr pgev, string solvername);
        private static gevSolver2Id_t dll_gevSolver2Id;
        private static int d_gevSolver2Id(IntPtr pgev, string solvername)
        { gevErrorHandling("gevSolver2Id could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevId2Solver_t(IntPtr pgev, int solverid, StringBuilder sst_result);
        private static gevId2Solver_t dll_gevId2Solver;
        private static void d_gevId2Solver(IntPtr pgev, int solverid, StringBuilder sst_result)
        { gevErrorHandling("gevId2Solver could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevCallSolverNextGridDir_t(IntPtr pgev, StringBuilder sst_result);
        private static gevCallSolverNextGridDir_t dll_gevCallSolverNextGridDir;
        private static void d_gevCallSolverNextGridDir(IntPtr pgev, StringBuilder sst_result)
        { gevErrorHandling("gevCallSolverNextGridDir could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevCallSolver_t(IntPtr pgev, IntPtr gmoptr, string cntrfn, string solvername, int solvelink, int Logging, string logfn, string statfn, double reslim, int iterlim, int domlim, double optcr, double optca, ref IntPtr jobhandle, StringBuilder msg);
        private static gevCallSolver_t dll_gevCallSolver;
        private static int d_gevCallSolver(IntPtr pgev, IntPtr gmoptr, string cntrfn, string solvername, int solvelink, int Logging, string logfn, string statfn, double reslim, int iterlim, int domlim, double optcr, double optca, ref IntPtr jobhandle, StringBuilder msg)
        { gevErrorHandling("gevCallSolver could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevCallSolverHandleStatus_t(IntPtr pgev, IntPtr jobhandle);
        private static gevCallSolverHandleStatus_t dll_gevCallSolverHandleStatus;
        private static int d_gevCallSolverHandleStatus(IntPtr pgev, IntPtr jobhandle)
        { gevErrorHandling("gevCallSolverHandleStatus could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevCallSolverHandleDelete_t(IntPtr pgev, ref IntPtr jobhandle);
        private static gevCallSolverHandleDelete_t dll_gevCallSolverHandleDelete;
        private static int d_gevCallSolverHandleDelete(IntPtr pgev, ref IntPtr jobhandle)
        { gevErrorHandling("gevCallSolverHandleDelete could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevCallSolverHandleCollect_t(IntPtr pgev, ref IntPtr jobhandle, IntPtr gmoptr);
        private static gevCallSolverHandleCollect_t dll_gevCallSolverHandleCollect;
        private static int d_gevCallSolverHandleCollect(IntPtr pgev, ref IntPtr jobhandle, IntPtr gmoptr)
        { gevErrorHandling("gevCallSolverHandleCollect could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevGetIntOpt_t(IntPtr pgev, string optname);
        private static gevGetIntOpt_t dll_gevGetIntOpt;
        private static int d_gevGetIntOpt(IntPtr pgev, string optname)
        { gevErrorHandling("gevGetIntOpt could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gevGetDblOpt_t(IntPtr pgev, string optname);
        private static gevGetDblOpt_t dll_gevGetDblOpt;
        private static double d_gevGetDblOpt(IntPtr pgev, string optname)
        { gevErrorHandling("gevGetDblOpt could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevGetStrOpt_t(IntPtr pgev, string optname, StringBuilder sst_result);
        private static gevGetStrOpt_t dll_gevGetStrOpt;
        private static void d_gevGetStrOpt(IntPtr pgev, string optname, StringBuilder sst_result)
        { gevErrorHandling("gevGetStrOpt could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevSetIntOpt_t(IntPtr pgev, string optname, int ival);
        private static gevSetIntOpt_t dll_gevSetIntOpt;
        private static void d_gevSetIntOpt(IntPtr pgev, string optname, int ival)
        { gevErrorHandling("gevSetIntOpt could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevSetDblOpt_t(IntPtr pgev, string optname, double rval);
        private static gevSetDblOpt_t dll_gevSetDblOpt;
        private static void d_gevSetDblOpt(IntPtr pgev, string optname, double rval)
        { gevErrorHandling("gevSetDblOpt could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevSetStrOpt_t(IntPtr pgev, string optname, string sval);
        private static gevSetStrOpt_t dll_gevSetStrOpt;
        private static void d_gevSetStrOpt(IntPtr pgev, string optname, string sval)
        { gevErrorHandling("gevSetStrOpt could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevSynchronizeOpt_t(IntPtr pgev, IntPtr optptr);
        private static gevSynchronizeOpt_t dll_gevSynchronizeOpt;
        private static void d_gevSynchronizeOpt(IntPtr pgev, IntPtr optptr)
        { gevErrorHandling("gevSynchronizeOpt could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gevTimeJNow_t(IntPtr pgev);
        private static gevTimeJNow_t dll_gevTimeJNow;
        private static double d_gevTimeJNow(IntPtr pgev)
        { gevErrorHandling("gevTimeJNow could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gevTimeDiff_t(IntPtr pgev);
        private static gevTimeDiff_t dll_gevTimeDiff;
        private static double d_gevTimeDiff(IntPtr pgev)
        { gevErrorHandling("gevTimeDiff could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gevTimeDiffStart_t(IntPtr pgev);
        private static gevTimeDiffStart_t dll_gevTimeDiffStart;
        private static double d_gevTimeDiffStart(IntPtr pgev)
        { gevErrorHandling("gevTimeDiffStart could not be loaded"); return 0.0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevTimeSetStart_t(IntPtr pgev);
        private static gevTimeSetStart_t dll_gevTimeSetStart;
        private static void d_gevTimeSetStart(IntPtr pgev)
        { gevErrorHandling("gevTimeSetStart could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevTerminateUninstall_t(IntPtr pgev);
        private static gevTerminateUninstall_t dll_gevTerminateUninstall;
        private static void d_gevTerminateUninstall(IntPtr pgev)
        { gevErrorHandling("gevTerminateUninstall could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevTerminateInstall_t(IntPtr pgev);
        private static gevTerminateInstall_t dll_gevTerminateInstall;
        private static void d_gevTerminateInstall(IntPtr pgev)
        { gevErrorHandling("gevTerminateInstall could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevTerminateSet_t(IntPtr pgev, IntPtr intr, IntPtr ehdler);
        private static gevTerminateSet_t dll_gevTerminateSet;
        private static void d_gevTerminateSet(IntPtr pgev, IntPtr intr, IntPtr ehdler)
        { gevErrorHandling("gevTerminateSet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevTerminateGet_t(IntPtr pgev);
        private static gevTerminateGet_t dll_gevTerminateGet;
        private static int d_gevTerminateGet(IntPtr pgev)
        { gevErrorHandling("gevTerminateGet could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevTerminateClear_t(IntPtr pgev);
        private static gevTerminateClear_t dll_gevTerminateClear;
        private static void d_gevTerminateClear(IntPtr pgev)
        { gevErrorHandling("gevTerminateClear could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevTerminateRaise_t(IntPtr pgev);
        private static gevTerminateRaise_t dll_gevTerminateRaise;
        private static void d_gevTerminateRaise(IntPtr pgev)
        { gevErrorHandling("gevTerminateRaise could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevTerminateGetHandler_t(IntPtr pgev, ref IntPtr intr, ref IntPtr ehdler);
        private static gevTerminateGetHandler_t dll_gevTerminateGetHandler;
        private static void d_gevTerminateGetHandler(IntPtr pgev, ref IntPtr intr, ref IntPtr ehdler)
        { gevErrorHandling("gevTerminateGetHandler could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevGetScratchName_t(IntPtr pgev, string s, StringBuilder sst_result);
        private static gevGetScratchName_t dll_gevGetScratchName;
        private static void d_gevGetScratchName(IntPtr pgev, string s, StringBuilder sst_result)
        { gevErrorHandling("gevGetScratchName could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevWriteModelInstance_t(IntPtr pgev, string mifn, IntPtr gmoptr, ref int nlcodelen);
        private static gevWriteModelInstance_t dll_gevWriteModelInstance;
        private static int d_gevWriteModelInstance(IntPtr pgev, string mifn, IntPtr gmoptr, ref int nlcodelen)
        { gevErrorHandling("gevWriteModelInstance could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevDuplicateScratchDir_t(IntPtr pgev, string scrdir, string logfn, StringBuilder cntrfn);
        private static gevDuplicateScratchDir_t dll_gevDuplicateScratchDir;
        private static int d_gevDuplicateScratchDir(IntPtr pgev, string scrdir, string logfn, StringBuilder cntrfn)
        { gevErrorHandling("gevDuplicateScratchDir could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevInitJacLegacy_t(IntPtr pgev, ref IntPtr evalptr, IntPtr gmoptr);
        private static gevInitJacLegacy_t dll_gevInitJacLegacy;
        private static int d_gevInitJacLegacy(IntPtr pgev, ref IntPtr evalptr, IntPtr gmoptr)
        { gevErrorHandling("gevInitJacLegacy could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevSetColRowPermLegacy_t(IntPtr pgev, IntPtr evalptr, int n, int[] cgms2slv, int m, int[] rgms2slv);
        private static gevSetColRowPermLegacy_t dll_gevSetColRowPermLegacy;
        private static void d_gevSetColRowPermLegacy(IntPtr pgev, IntPtr evalptr, int n, int[] cgms2slv, int m, int[] rgms2slv)
        { gevErrorHandling("gevSetColRowPermLegacy could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevSetJacPermLegacy_t(IntPtr pgev, IntPtr evalptr, int njacs, int[] jacs, int[] jgms2slv);
        private static gevSetJacPermLegacy_t dll_gevSetJacPermLegacy;
        private static void d_gevSetJacPermLegacy(IntPtr pgev, IntPtr evalptr, int njacs, int[] jacs, int[] jgms2slv)
        { gevErrorHandling("gevSetJacPermLegacy could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevEvalNewPointLegacy_t(IntPtr pgev, IntPtr evalptr, double[] x);
        private static gevEvalNewPointLegacy_t dll_gevEvalNewPointLegacy;
        private static int d_gevEvalNewPointLegacy(IntPtr pgev, IntPtr evalptr, double[] x)
        { gevErrorHandling("gevEvalNewPointLegacy could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevEvalJacLegacy_t(IntPtr pgev, IntPtr evalptr, int si, double[] x, ref double f, double[] jac, ref int domviol, ref int njacsupd);
        private static gevEvalJacLegacy_t dll_gevEvalJacLegacy;
        private static int d_gevEvalJacLegacy(IntPtr pgev, IntPtr evalptr, int si, double[] x, ref double f, double[] jac, ref int domviol, ref int njacsupd)
        { gevErrorHandling("gevEvalJacLegacy could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevEvalJacLegacyX_t(IntPtr pgev, IntPtr evalptr, int cnt, int[] rowidx, double[] x, double[] fvec, double[] jac, ref int domviol, ref int njacsupd);
        private static gevEvalJacLegacyX_t dll_gevEvalJacLegacyX;
        private static int d_gevEvalJacLegacyX(IntPtr pgev, IntPtr evalptr, int cnt, int[] rowidx, double[] x, double[] fvec, double[] jac, ref int domviol, ref int njacsupd)
        { gevErrorHandling("gevEvalJacLegacyX could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevNextNLLegacy_t(IntPtr pgev, IntPtr evalptr, int si);
        private static gevNextNLLegacy_t dll_gevNextNLLegacy;
        private static int d_gevNextNLLegacy(IntPtr pgev, IntPtr evalptr, int si)
        { gevErrorHandling("gevNextNLLegacy could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevRowGms2SlvLegacy_t(IntPtr pgev, IntPtr evalptr, int si);
        private static gevRowGms2SlvLegacy_t dll_gevRowGms2SlvLegacy;
        private static int d_gevRowGms2SlvLegacy(IntPtr pgev, IntPtr evalptr, int si)
        { gevErrorHandling("gevRowGms2SlvLegacy could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevFreeJacLegacy_t(IntPtr pgev, ref IntPtr evalptr);
        private static gevFreeJacLegacy_t dll_gevFreeJacLegacy;
        private static void d_gevFreeJacLegacy(IntPtr pgev, ref IntPtr evalptr)
        { gevErrorHandling("gevFreeJacLegacy could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr gevGetALGX_t(IntPtr pgev);
        private static gevGetALGX_t dll_gevGetALGX;
        private static IntPtr d_gevGetALGX(IntPtr pgev)
        { gevErrorHandling("gevGetALGX could not be loaded"); return IntPtr.Zero; }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevSkipIOLegacySet_t(IntPtr pgev, int x);
        private static gevSkipIOLegacySet_t dll_gevSkipIOLegacySet;
        private static void d_gevSkipIOLegacySet(IntPtr pgev, int x)
        { gevErrorHandling("gevSkipIOLegacySet could not be loaded"); }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevThreads_t(IntPtr pgev);
        private static gevThreads_t dll_gevThreads;
        private static int d_gevThreads(IntPtr pgev)
        { gevErrorHandling("gevThreads could not be loaded"); return 0; }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate double gevNSolves_t(IntPtr pgev);
        private static gevNSolves_t dll_gevNSolves;
        private static double d_gevNSolves(IntPtr pgev)
        { gevErrorHandling("gevNSolves could not be loaded"); return 0.0; }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevxcreate_t(ref IntPtr pgev);
        private static gevxcreate_t gevxcreate;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevxcreated_t(ref IntPtr pgev, string dirName);
        private static gevxcreated_t gevxcreated;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void gevxfree_t(ref IntPtr pgev);
        private static gevxfree_t gevxfree;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevxapiversion_t(int api, StringBuilder msg, ref int cl);
        private static gevxapiversion_t dll_gevxapiversion;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int gevxcheck_t(string ep, int nargs, int[] s, StringBuilder msg);
        private static gevxcheck_t dll_gevxcheck;

        public delegate bool gevErrorCallback_t(int ErrCount, string Msg);

        static bool isLoaded = false;
        static IntPtr h;
        static bool ScreenIndicator = true;
        static bool ExceptionIndicator = false;
        static bool ExitIndicator = true;
        static gevErrorCallback_t ErrorCallBack = null;
        static int APIErrorCount = 0;

        private bool XLibraryLoad(string dllName, ref string errBuf)
        {
            string symName;
            int cl = 0;
            IntPtr pAddressOfFunctionToCall;

            if (isLoaded)
                return true;

#if __MonoCS__ || __APPLE__
        h = LoadLibrary(@dllName,2);
#else
            h = LoadLibrary(@dllName);
#endif

            if (IntPtr.Zero == h)
            {
                errBuf = "Could not load shared library " + dllName;
                return false;
            }

            pAddressOfFunctionToCall = GetProcAddress(h, "gevxcreate");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                gevxcreate = (gevxcreate_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevxcreate_t));
            else
            {
                symName = "gevxcreate"; goto symMissing;
            }
            pAddressOfFunctionToCall = GetProcAddress(h, "cgevxcreated");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                gevxcreated = (gevxcreated_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevxcreated_t));
            else
            {
                symName = "cgevxcreated"; goto symMissing;
            }
            pAddressOfFunctionToCall = GetProcAddress(h, "gevxfree");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                gevxfree = (gevxfree_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevxfree_t));
            else
            {
                symName = "gevxfree"; goto symMissing;
            }
            pAddressOfFunctionToCall = GetProcAddress(h, "cgevxcheck");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                dll_gevxcheck = (gevxcheck_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevxcheck_t));
            else
            {
                symName = "cgevxcheck"; goto symMissing;
            }
            pAddressOfFunctionToCall = GetProcAddress(h, "cgevxapiversion");
            if (pAddressOfFunctionToCall != IntPtr.Zero)
                dll_gevxapiversion = (gevxapiversion_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevxapiversion_t));
            else
            {
                symName = "cgevxapiversion"; goto symMissing;
            }
            if (gevxapiversion(7, ref errBuf, ref cl) == 0)
                return false;

            {
                int[] s = { 0, 59, 15, 1 };
                if (gevxcheck("gevRegisterWriteCallback", 3, s, ref errBuf) == 0)
                    dll_gevRegisterWriteCallback = d_gevRegisterWriteCallback;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevregisterwritecallback");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevRegisterWriteCallback = (gevRegisterWriteCallback_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevRegisterWriteCallback_t));
                    else
                    {
                        symName = "gevRegisterWriteCallback"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 1, 1, 1, 1 };
                if (gevxcheck("gevCompleteEnvironment", 4, s, ref errBuf) == 0)
                    dll_gevCompleteEnvironment = d_gevCompleteEnvironment;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevcompleteenvironment");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevCompleteEnvironment = (gevCompleteEnvironment_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevCompleteEnvironment_t));
                    else
                    {
                        symName = "gevCompleteEnvironment"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11 };
                if (gevxcheck("gevInitEnvironmentLegacy", 1, s, ref errBuf) == 0)
                    dll_gevInitEnvironmentLegacy = d_gevInitEnvironmentLegacy;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevinitenvironmentlegacy");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevInitEnvironmentLegacy = (gevInitEnvironmentLegacy_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevInitEnvironmentLegacy_t));
                    else
                    {
                        symName = "cgevInitEnvironmentLegacy"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15, 3, 11, 15, 11, 15, 59, 1, 2 };
                if (gevxcheck("gevSwitchLogStat", 8, s, ref errBuf) == 0)
                    dll_gevSwitchLogStat = d_gevSwitchLogStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevswitchlogstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevSwitchLogStat = (gevSwitchLogStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevSwitchLogStat_t));
                    else
                    {
                        symName = "cgevSwitchLogStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 1 };
                if (gevxcheck("gevGetLShandle", 0, s, ref errBuf) == 0)
                    dll_gevGetLShandle = d_gevGetLShandle;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevgetlshandle");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevGetLShandle = (gevGetLShandle_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevGetLShandle_t));
                    else
                    {
                        symName = "gevGetLShandle"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15, 2 };
                if (gevxcheck("gevRestoreLogStat", 1, s, ref errBuf) == 0)
                    dll_gevRestoreLogStat = d_gevRestoreLogStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevrestorelogstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevRestoreLogStat = (gevRestoreLogStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevRestoreLogStat_t));
                    else
                    {
                        symName = "gevRestoreLogStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15, 2 };
                if (gevxcheck("gevRestoreLogStatRewrite", 1, s, ref errBuf) == 0)
                    dll_gevRestoreLogStatRewrite = d_gevRestoreLogStatRewrite;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevrestorelogstatrewrite");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevRestoreLogStatRewrite = (gevRestoreLogStatRewrite_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevRestoreLogStatRewrite_t));
                    else
                    {
                        symName = "gevRestoreLogStatRewrite"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11 };
                if (gevxcheck("gevLog", 1, s, ref errBuf) == 0)
                    dll_gevLog = d_gevLog;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevlog");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevLog = (gevLog_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevLog_t));
                    else
                    {
                        symName = "cgevLog"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 9 };
                if (gevxcheck("gevLogPChar", 1, s, ref errBuf) == 0)
                    dll_gevLogPChar = d_gevLogPChar;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevlogpchar");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevLogPChar = (gevLogPChar_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevLogPChar_t));
                    else
                    {
                        symName = "gevLogPChar"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11 };
                if (gevxcheck("gevStat", 1, s, ref errBuf) == 0)
                    dll_gevStat = d_gevStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStat = (gevStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStat_t));
                    else
                    {
                        symName = "cgevStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11 };
                if (gevxcheck("gevStatC", 1, s, ref errBuf) == 0)
                    dll_gevStatC = d_gevStatC;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevstatc");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatC = (gevStatC_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatC_t));
                    else
                    {
                        symName = "cgevStatC"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 9 };
                if (gevxcheck("gevStatPChar", 1, s, ref errBuf) == 0)
                    dll_gevStatPChar = d_gevStatPChar;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevstatpchar");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatPChar = (gevStatPChar_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatPChar_t));
                    else
                    {
                        symName = "gevStatPChar"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11 };
                if (gevxcheck("gevStatAudit", 1, s, ref errBuf) == 0)
                    dll_gevStatAudit = d_gevStatAudit;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevstataudit");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatAudit = (gevStatAudit_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatAudit_t));
                    else
                    {
                        symName = "cgevStatAudit"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gevxcheck("gevStatCon", 0, s, ref errBuf) == 0)
                    dll_gevStatCon = d_gevStatCon;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevstatcon");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatCon = (gevStatCon_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatCon_t));
                    else
                    {
                        symName = "gevStatCon"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gevxcheck("gevStatCoff", 0, s, ref errBuf) == 0)
                    dll_gevStatCoff = d_gevStatCoff;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevstatcoff");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatCoff = (gevStatCoff_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatCoff_t));
                    else
                    {
                        symName = "gevStatCoff"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gevxcheck("gevStatEOF", 0, s, ref errBuf) == 0)
                    dll_gevStatEOF = d_gevStatEOF;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevstateof");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatEOF = (gevStatEOF_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatEOF_t));
                    else
                    {
                        symName = "gevStatEOF"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gevxcheck("gevStatSysout", 0, s, ref errBuf) == 0)
                    dll_gevStatSysout = d_gevStatSysout;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevstatsysout");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatSysout = (gevStatSysout_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatSysout_t));
                    else
                    {
                        symName = "gevStatSysout"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 11 };
                if (gevxcheck("gevStatAddE", 2, s, ref errBuf) == 0)
                    dll_gevStatAddE = d_gevStatAddE;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevstatadde");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatAddE = (gevStatAddE_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatAddE_t));
                    else
                    {
                        symName = "cgevStatAddE"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 11 };
                if (gevxcheck("gevStatAddV", 2, s, ref errBuf) == 0)
                    dll_gevStatAddV = d_gevStatAddV;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevstataddv");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatAddV = (gevStatAddV_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatAddV_t));
                    else
                    {
                        symName = "cgevStatAddV"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 3, 3, 11 };
                if (gevxcheck("gevStatAddJ", 3, s, ref errBuf) == 0)
                    dll_gevStatAddJ = d_gevStatAddJ;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevstataddj");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatAddJ = (gevStatAddJ_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatAddJ_t));
                    else
                    {
                        symName = "cgevStatAddJ"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gevxcheck("gevStatEject", 0, s, ref errBuf) == 0)
                    dll_gevStatEject = d_gevStatEject;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevstateject");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatEject = (gevStatEject_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatEject_t));
                    else
                    {
                        symName = "gevStatEject"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 18 };
                if (gevxcheck("gevStatEdit", 1, s, ref errBuf) == 0)
                    dll_gevStatEdit = d_gevStatEdit;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevstatedit");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatEdit = (gevStatEdit_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatEdit_t));
                    else
                    {
                        symName = "gevStatEdit"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11, 3, 11 };
                if (gevxcheck("gevStatE", 3, s, ref errBuf) == 0)
                    dll_gevStatE = d_gevStatE;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevstate");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatE = (gevStatE_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatE_t));
                    else
                    {
                        symName = "cgevStatE"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11, 3, 11 };
                if (gevxcheck("gevStatV", 3, s, ref errBuf) == 0)
                    dll_gevStatV = d_gevStatV;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevstatv");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatV = (gevStatV_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatV_t));
                    else
                    {
                        symName = "cgevStatV"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gevxcheck("gevStatT", 0, s, ref errBuf) == 0)
                    dll_gevStatT = d_gevStatT;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevstatt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatT = (gevStatT_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatT_t));
                    else
                    {
                        symName = "gevStatT"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11 };
                if (gevxcheck("gevStatA", 1, s, ref errBuf) == 0)
                    dll_gevStatA = d_gevStatA;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevstata");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatA = (gevStatA_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatA_t));
                    else
                    {
                        symName = "cgevStatA"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11 };
                if (gevxcheck("gevStatB", 1, s, ref errBuf) == 0)
                    dll_gevStatB = d_gevStatB;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevstatb");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatB = (gevStatB_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatB_t));
                    else
                    {
                        symName = "cgevStatB"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11 };
                if (gevxcheck("gevLogStat", 1, s, ref errBuf) == 0)
                    dll_gevLogStat = d_gevLogStat;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevlogstat");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevLogStat = (gevLogStat_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevLogStat_t));
                    else
                    {
                        symName = "cgevLogStat"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11 };
                if (gevxcheck("gevLogStatNoC", 1, s, ref errBuf) == 0)
                    dll_gevLogStatNoC = d_gevLogStatNoC;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevlogstatnoc");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevLogStatNoC = (gevLogStatNoC_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevLogStatNoC_t));
                    else
                    {
                        symName = "cgevLogStatNoC"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 9 };
                if (gevxcheck("gevLogStatPChar", 1, s, ref errBuf) == 0)
                    dll_gevLogStatPChar = d_gevLogStatPChar;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevlogstatpchar");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevLogStatPChar = (gevLogStatPChar_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevLogStatPChar_t));
                    else
                    {
                        symName = "gevLogStatPChar"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gevxcheck("gevLogStatFlush", 0, s, ref errBuf) == 0)
                    dll_gevLogStatFlush = d_gevLogStatFlush;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevlogstatflush");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevLogStatFlush = (gevLogStatFlush_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevLogStatFlush_t));
                    else
                    {
                        symName = "gevLogStatFlush"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 11 };
                if (gevxcheck("gevGetAnchor", 1, s, ref errBuf) == 0)
                    dll_gevGetAnchor = d_gevGetAnchor;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevgetanchor");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevGetAnchor = (gevGetAnchor_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevGetAnchor_t));
                    else
                    {
                        symName = "cgevGetAnchor"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11 };
                if (gevxcheck("gevLSTAnchor", 1, s, ref errBuf) == 0)
                    dll_gevLSTAnchor = d_gevLSTAnchor;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevlstanchor");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevLSTAnchor = (gevLSTAnchor_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevLSTAnchor_t));
                    else
                    {
                        symName = "cgevLSTAnchor"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 12 };
                if (gevxcheck("gevStatAppend", 2, s, ref errBuf) == 0)
                    dll_gevStatAppend = d_gevStatAppend;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevstatappend");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevStatAppend = (gevStatAppend_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevStatAppend_t));
                    else
                    {
                        symName = "cgevStatAppend"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 1, 13, 3, 13, 13 };
                if (gevxcheck("gevMIPReport", 5, s, ref errBuf) == 0)
                    dll_gevMIPReport = d_gevMIPReport;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevmipreport");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevMIPReport = (gevMIPReport_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevMIPReport_t));
                    else
                    {
                        symName = "gevMIPReport"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 12 };
                if (gevxcheck("gevGetSlvExeInfo", 2, s, ref errBuf) == 0)
                    dll_gevGetSlvExeInfo = d_gevGetSlvExeInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevgetslvexeinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevGetSlvExeInfo = (gevGetSlvExeInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevGetSlvExeInfo_t));
                    else
                    {
                        symName = "cgevGetSlvExeInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 12, 12, 4 };
                if (gevxcheck("gevGetSlvLibInfo", 4, s, ref errBuf) == 0)
                    dll_gevGetSlvLibInfo = d_gevGetSlvLibInfo;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevgetslvlibinfo");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevGetSlvLibInfo = (gevGetSlvLibInfo_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevGetSlvLibInfo_t));
                    else
                    {
                        symName = "cgevGetSlvLibInfo"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 3, 11, 20 };
                if (gevxcheck("gevCapabilityCheck", 3, s, ref errBuf) == 0)
                    dll_gevCapabilityCheck = d_gevCapabilityCheck;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevcapabilitycheck");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevCapabilityCheck = (gevCapabilityCheck_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevCapabilityCheck_t));
                    else
                    {
                        symName = "cgevCapabilityCheck"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 20, 20 };
                if (gevxcheck("gevSolverVisibility", 3, s, ref errBuf) == 0)
                    dll_gevSolverVisibility = d_gevSolverVisibility;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevsolvervisibility");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevSolverVisibility = (gevSolverVisibility_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevSolverVisibility_t));
                    else
                    {
                        symName = "cgevSolverVisibility"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gevxcheck("gevNumSolvers", 0, s, ref errBuf) == 0)
                    dll_gevNumSolvers = d_gevNumSolvers;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevnumsolvers");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevNumSolvers = (gevNumSolvers_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevNumSolvers_t));
                    else
                    {
                        symName = "gevNumSolvers"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 3 };
                if (gevxcheck("gevGetSolver", 1, s, ref errBuf) == 0)
                    dll_gevGetSolver = d_gevGetSolver;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevgetsolver");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevGetSolver = (gevGetSolver_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevGetSolver_t));
                    else
                    {
                        symName = "cgevGetSolver"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 1 };
                if (gevxcheck("gevGetCurrentSolver", 1, s, ref errBuf) == 0)
                    dll_gevGetCurrentSolver = d_gevGetCurrentSolver;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevgetcurrentsolver");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevGetCurrentSolver = (gevGetCurrentSolver_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevGetCurrentSolver_t));
                    else
                    {
                        symName = "cgevGetCurrentSolver"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 3 };
                if (gevxcheck("gevGetSolverDefault", 1, s, ref errBuf) == 0)
                    dll_gevGetSolverDefault = d_gevGetSolverDefault;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevgetsolverdefault");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevGetSolverDefault = (gevGetSolverDefault_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevGetSolverDefault_t));
                    else
                    {
                        symName = "cgevGetSolverDefault"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11 };
                if (gevxcheck("gevSolver2Id", 1, s, ref errBuf) == 0)
                    dll_gevSolver2Id = d_gevSolver2Id;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevsolver2id");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevSolver2Id = (gevSolver2Id_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevSolver2Id_t));
                    else
                    {
                        symName = "cgevSolver2Id"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 3 };
                if (gevxcheck("gevId2Solver", 1, s, ref errBuf) == 0)
                    dll_gevId2Solver = d_gevId2Solver;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevid2solver");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevId2Solver = (gevId2Solver_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevId2Solver_t));
                    else
                    {
                        symName = "cgevId2Solver"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12 };
                if (gevxcheck("gevCallSolverNextGridDir", 0, s, ref errBuf) == 0)
                    dll_gevCallSolverNextGridDir = d_gevCallSolverNextGridDir;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevcallsolvernextgriddir");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevCallSolverNextGridDir = (gevCallSolverNextGridDir_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevCallSolverNextGridDir_t));
                    else
                    {
                        symName = "cgevCallSolverNextGridDir"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 11, 11, 3, 3, 11, 11, 13, 3, 3, 13, 13, 2, 12 };
                if (gevxcheck("gevCallSolver", 14, s, ref errBuf) == 0)
                    dll_gevCallSolver = d_gevCallSolver;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevcallsolver");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevCallSolver = (gevCallSolver_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevCallSolver_t));
                    else
                    {
                        symName = "cgevCallSolver"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1 };
                if (gevxcheck("gevCallSolverHandleStatus", 1, s, ref errBuf) == 0)
                    dll_gevCallSolverHandleStatus = d_gevCallSolverHandleStatus;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevcallsolverhandlestatus");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevCallSolverHandleStatus = (gevCallSolverHandleStatus_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevCallSolverHandleStatus_t));
                    else
                    {
                        symName = "gevCallSolverHandleStatus"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 2 };
                if (gevxcheck("gevCallSolverHandleDelete", 1, s, ref errBuf) == 0)
                    dll_gevCallSolverHandleDelete = d_gevCallSolverHandleDelete;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevcallsolverhandledelete");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevCallSolverHandleDelete = (gevCallSolverHandleDelete_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevCallSolverHandleDelete_t));
                    else
                    {
                        symName = "gevCallSolverHandleDelete"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 2, 1 };
                if (gevxcheck("gevCallSolverHandleCollect", 2, s, ref errBuf) == 0)
                    dll_gevCallSolverHandleCollect = d_gevCallSolverHandleCollect;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevcallsolverhandlecollect");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevCallSolverHandleCollect = (gevCallSolverHandleCollect_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevCallSolverHandleCollect_t));
                    else
                    {
                        symName = "gevCallSolverHandleCollect"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11 };
                if (gevxcheck("gevGetIntOpt", 1, s, ref errBuf) == 0)
                    dll_gevGetIntOpt = d_gevGetIntOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevgetintopt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevGetIntOpt = (gevGetIntOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevGetIntOpt_t));
                    else
                    {
                        symName = "cgevGetIntOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13, 11 };
                if (gevxcheck("gevGetDblOpt", 1, s, ref errBuf) == 0)
                    dll_gevGetDblOpt = d_gevGetDblOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevgetdblopt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevGetDblOpt = (gevGetDblOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevGetDblOpt_t));
                    else
                    {
                        symName = "cgevGetDblOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 11 };
                if (gevxcheck("gevGetStrOpt", 1, s, ref errBuf) == 0)
                    dll_gevGetStrOpt = d_gevGetStrOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevgetstropt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevGetStrOpt = (gevGetStrOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevGetStrOpt_t));
                    else
                    {
                        symName = "cgevGetStrOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11, 3 };
                if (gevxcheck("gevSetIntOpt", 2, s, ref errBuf) == 0)
                    dll_gevSetIntOpt = d_gevSetIntOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevsetintopt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevSetIntOpt = (gevSetIntOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevSetIntOpt_t));
                    else
                    {
                        symName = "cgevSetIntOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11, 13 };
                if (gevxcheck("gevSetDblOpt", 2, s, ref errBuf) == 0)
                    dll_gevSetDblOpt = d_gevSetDblOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevsetdblopt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevSetDblOpt = (gevSetDblOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevSetDblOpt_t));
                    else
                    {
                        symName = "cgevSetDblOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 11, 11 };
                if (gevxcheck("gevSetStrOpt", 2, s, ref errBuf) == 0)
                    dll_gevSetStrOpt = d_gevSetStrOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevsetstropt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevSetStrOpt = (gevSetStrOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevSetStrOpt_t));
                    else
                    {
                        symName = "cgevSetStrOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 1 };
                if (gevxcheck("gevSynchronizeOpt", 1, s, ref errBuf) == 0)
                    dll_gevSynchronizeOpt = d_gevSynchronizeOpt;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevsynchronizeopt");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevSynchronizeOpt = (gevSynchronizeOpt_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevSynchronizeOpt_t));
                    else
                    {
                        symName = "gevSynchronizeOpt"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gevxcheck("gevTimeJNow", 0, s, ref errBuf) == 0)
                    dll_gevTimeJNow = d_gevTimeJNow;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevtimejnow");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevTimeJNow = (gevTimeJNow_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevTimeJNow_t));
                    else
                    {
                        symName = "gevTimeJNow"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gevxcheck("gevTimeDiff", 0, s, ref errBuf) == 0)
                    dll_gevTimeDiff = d_gevTimeDiff;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevtimediff");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevTimeDiff = (gevTimeDiff_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevTimeDiff_t));
                    else
                    {
                        symName = "gevTimeDiff"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gevxcheck("gevTimeDiffStart", 0, s, ref errBuf) == 0)
                    dll_gevTimeDiffStart = d_gevTimeDiffStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevtimediffstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevTimeDiffStart = (gevTimeDiffStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevTimeDiffStart_t));
                    else
                    {
                        symName = "gevTimeDiffStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gevxcheck("gevTimeSetStart", 0, s, ref errBuf) == 0)
                    dll_gevTimeSetStart = d_gevTimeSetStart;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevtimesetstart");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevTimeSetStart = (gevTimeSetStart_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevTimeSetStart_t));
                    else
                    {
                        symName = "gevTimeSetStart"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gevxcheck("gevTerminateUninstall", 0, s, ref errBuf) == 0)
                    dll_gevTerminateUninstall = d_gevTerminateUninstall;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevterminateuninstall");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevTerminateUninstall = (gevTerminateUninstall_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevTerminateUninstall_t));
                    else
                    {
                        symName = "gevTerminateUninstall"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gevxcheck("gevTerminateInstall", 0, s, ref errBuf) == 0)
                    dll_gevTerminateInstall = d_gevTerminateInstall;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevterminateinstall");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevTerminateInstall = (gevTerminateInstall_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevTerminateInstall_t));
                    else
                    {
                        symName = "gevTerminateInstall"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 1, 1 };
                if (gevxcheck("gevTerminateSet", 2, s, ref errBuf) == 0)
                    dll_gevTerminateSet = d_gevTerminateSet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevterminateset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevTerminateSet = (gevTerminateSet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevTerminateSet_t));
                    else
                    {
                        symName = "gevTerminateSet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 15 };
                if (gevxcheck("gevTerminateGet", 0, s, ref errBuf) == 0)
                    dll_gevTerminateGet = d_gevTerminateGet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevterminateget");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevTerminateGet = (gevTerminateGet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevTerminateGet_t));
                    else
                    {
                        symName = "gevTerminateGet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gevxcheck("gevTerminateClear", 0, s, ref errBuf) == 0)
                    dll_gevTerminateClear = d_gevTerminateClear;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevterminateclear");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevTerminateClear = (gevTerminateClear_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevTerminateClear_t));
                    else
                    {
                        symName = "gevTerminateClear"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0 };
                if (gevxcheck("gevTerminateRaise", 0, s, ref errBuf) == 0)
                    dll_gevTerminateRaise = d_gevTerminateRaise;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevterminateraise");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevTerminateRaise = (gevTerminateRaise_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevTerminateRaise_t));
                    else
                    {
                        symName = "gevTerminateRaise"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 2, 2 };
                if (gevxcheck("gevTerminateGetHandler", 2, s, ref errBuf) == 0)
                    dll_gevTerminateGetHandler = d_gevTerminateGetHandler;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevterminategethandler");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevTerminateGetHandler = (gevTerminateGetHandler_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevTerminateGetHandler_t));
                    else
                    {
                        symName = "gevTerminateGetHandler"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 12, 11 };
                if (gevxcheck("gevGetScratchName", 1, s, ref errBuf) == 0)
                    dll_gevGetScratchName = d_gevGetScratchName;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevgetscratchname");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevGetScratchName = (gevGetScratchName_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevGetScratchName_t));
                    else
                    {
                        symName = "cgevGetScratchName"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 1, 21 };
                if (gevxcheck("gevWriteModelInstance", 3, s, ref errBuf) == 0)
                    dll_gevWriteModelInstance = d_gevWriteModelInstance;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevwritemodelinstance");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevWriteModelInstance = (gevWriteModelInstance_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevWriteModelInstance_t));
                    else
                    {
                        symName = "cgevWriteModelInstance"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 11, 11, 12 };
                if (gevxcheck("gevDuplicateScratchDir", 3, s, ref errBuf) == 0)
                    dll_gevDuplicateScratchDir = d_gevDuplicateScratchDir;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "cgevduplicatescratchdir");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevDuplicateScratchDir = (gevDuplicateScratchDir_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevDuplicateScratchDir_t));
                    else
                    {
                        symName = "cgevDuplicateScratchDir"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 2, 1 };
                if (gevxcheck("gevInitJacLegacy", 2, s, ref errBuf) == 0)
                    dll_gevInitJacLegacy = d_gevInitJacLegacy;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevinitjaclegacy");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevInitJacLegacy = (gevInitJacLegacy_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevInitJacLegacy_t));
                    else
                    {
                        symName = "gevInitJacLegacy"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 1, 3, 8, 3, 8 };
                if (gevxcheck("gevSetColRowPermLegacy", 5, s, ref errBuf) == 0)
                    dll_gevSetColRowPermLegacy = d_gevSetColRowPermLegacy;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevsetcolrowpermlegacy");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevSetColRowPermLegacy = (gevSetColRowPermLegacy_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevSetColRowPermLegacy_t));
                    else
                    {
                        symName = "gevSetColRowPermLegacy"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 1, 3, 8, 8 };
                if (gevxcheck("gevSetJacPermLegacy", 4, s, ref errBuf) == 0)
                    dll_gevSetJacPermLegacy = d_gevSetJacPermLegacy;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevsetjacpermlegacy");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevSetJacPermLegacy = (gevSetJacPermLegacy_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevSetJacPermLegacy_t));
                    else
                    {
                        symName = "gevSetJacPermLegacy"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 6 };
                if (gevxcheck("gevEvalNewPointLegacy", 2, s, ref errBuf) == 0)
                    dll_gevEvalNewPointLegacy = d_gevEvalNewPointLegacy;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevevalnewpointlegacy");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevEvalNewPointLegacy = (gevEvalNewPointLegacy_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevEvalNewPointLegacy_t));
                    else
                    {
                        symName = "gevEvalNewPointLegacy"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 3, 6, 22, 6, 21, 21 };
                if (gevxcheck("gevEvalJacLegacy", 7, s, ref errBuf) == 0)
                    dll_gevEvalJacLegacy = d_gevEvalJacLegacy;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevevaljaclegacy");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevEvalJacLegacy = (gevEvalJacLegacy_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevEvalJacLegacy_t));
                    else
                    {
                        symName = "gevEvalJacLegacy"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 3, 8, 6, 6, 6, 21, 21 };
                if (gevxcheck("gevEvalJacLegacyX", 8, s, ref errBuf) == 0)
                    dll_gevEvalJacLegacyX = d_gevEvalJacLegacyX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevevaljaclegacyx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevEvalJacLegacyX = (gevEvalJacLegacyX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevEvalJacLegacyX_t));
                    else
                    {
                        symName = "gevEvalJacLegacyX"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 3 };
                if (gevxcheck("gevNextNLLegacy", 2, s, ref errBuf) == 0)
                    dll_gevNextNLLegacy = d_gevNextNLLegacy;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevnextnllegacy");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevNextNLLegacy = (gevNextNLLegacy_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevNextNLLegacy_t));
                    else
                    {
                        symName = "gevNextNLLegacy"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3, 1, 3 };
                if (gevxcheck("gevRowGms2SlvLegacy", 2, s, ref errBuf) == 0)
                    dll_gevRowGms2SlvLegacy = d_gevRowGms2SlvLegacy;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevrowgms2slvlegacy");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevRowGms2SlvLegacy = (gevRowGms2SlvLegacy_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevRowGms2SlvLegacy_t));
                    else
                    {
                        symName = "gevRowGms2SlvLegacy"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 0, 2 };
                if (gevxcheck("gevFreeJacLegacy", 1, s, ref errBuf) == 0)
                    dll_gevFreeJacLegacy = d_gevFreeJacLegacy;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevfreejaclegacy");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevFreeJacLegacy = (gevFreeJacLegacy_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevFreeJacLegacy_t));
                    else
                    {
                        symName = "gevFreeJacLegacy"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 1 };
                if (gevxcheck("gevGetALGX", 0, s, ref errBuf) == 0)
                    dll_gevGetALGX = d_gevGetALGX;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevgetalgx");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevGetALGX = (gevGetALGX_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevGetALGX_t));
                    else
                    {
                        symName = "gevGetALGX"; goto symMissing;
                    }
                }
            }

            {
                int[] s = { 0, 15 };
                if (gevxcheck("gevSkipIOLegacySet", 1, s, ref errBuf) == 0)
                    dll_gevSkipIOLegacySet = d_gevSkipIOLegacySet;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevskipiolegacyset");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevSkipIOLegacySet = (gevSkipIOLegacySet_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevSkipIOLegacySet_t));
                    else
                    {
                        symName = "gevSkipIOLegacySet"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 3 };
                if (gevxcheck("gevThreads", 0, s, ref errBuf) == 0)
                    dll_gevThreads = d_gevThreads;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevthreads");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevThreads = (gevThreads_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevThreads_t));
                    else
                    {
                        symName = "gevThreads"; goto symMissing;
                    }
                }
            }
            {
                int[] s = { 13 };
                if (gevxcheck("gevNSolves", 0, s, ref errBuf) == 0)
                    dll_gevNSolves = d_gevNSolves;
                else
                {
                    pAddressOfFunctionToCall = GetProcAddress(h, "gevnsolves");
                    if (pAddressOfFunctionToCall != IntPtr.Zero)
                        dll_gevNSolves = (gevNSolves_t)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(gevNSolves_t));
                    else
                    {
                        symName = "gevNSolves"; goto symMissing;
                    }
                }
            }

            return true;

        symMissing:
            errBuf = "Could not load symbol '" + symName + "'";
            return false;

        } /* XLibraryLoad */

        private bool libloader(string dllPath, string dllName, ref string msgBuf)
        {
#if __MonoCS__ || __APPLE__
#if __APPLE__
        const string libStem = "libjoatdclib", libExt = ".dylib";
#else
        const string libStem = "libjoatdclib", libExt = ".so";
#endif
#else
            const string libStem = "joatdclib", libExt = ".dll";
#endif
            string dllNameBuf = string.Empty;
            int myrc = 0;
            string GMS_DLL_SUFFIX = string.Empty;

            msgBuf = string.Empty;
            if (!isLoaded)
            {
                if (string.Empty != dllPath)
                {
                    dllNameBuf = dllPath;
                    if (Path.DirectorySeparatorChar != dllNameBuf[dllNameBuf.Length - 1]) dllNameBuf = dllNameBuf + Path.DirectorySeparatorChar;
                }
                if (string.Empty != dllName)
                    dllNameBuf = dllNameBuf + dllName;
                else
                {
                    if (8 == IntPtr.Size)
                        GMS_DLL_SUFFIX = "64";
                    dllNameBuf = dllNameBuf + libStem + GMS_DLL_SUFFIX + libExt;
                }
                isLoaded = XLibraryLoad(dllNameBuf, ref msgBuf);
                if (isLoaded)
                {
                }
                else                          /* library load failed */
                    myrc |= 1;
            }
            return (myrc & 1) == 0;
        } /* libloader */

        public bool gevGetReady(ref string msgBuf)
        {
            return libloader(string.Empty, string.Empty, ref msgBuf);
        }
        public bool gevGetReadyD(string dirName, ref string msgBuf)
        {
            return libloader(dirName, string.Empty, ref msgBuf);
        }
        public bool gevGetReadyL(string dirName, string libName, ref string msgBuf)
        {
            return libloader(dirName, libName, ref msgBuf);
        }

        public gevmcs(ref string msgBuf)
        {
            bool gevIsReady;

            extHandle = false;
            _disposed = false;
            gevIsReady = gevGetReady(ref msgBuf);
            if (!gevIsReady) return;
            gevxcreate(ref pgev);
            if (pgev != IntPtr.Zero)
            {
                msgBuf = String.Empty;
                return;
            }
            msgBuf = "Error while creating object";
        }
        public gevmcs(string dirName, ref string msgBuf, bool passDN = false)
        {
            bool gevIsReady;

            extHandle = false;
            _disposed = false;
            gevIsReady = gevGetReadyD(dirName, ref msgBuf);
            if (!gevIsReady) return;
            if (passDN)
                gevxcreated(ref pgev, dirName);
            else
                gevxcreate(ref pgev);
            if (pgev != IntPtr.Zero)
            {
                msgBuf = String.Empty;
                return;
            }
            msgBuf = "Error while creating object";
        }
        public gevmcs(string dirName, string libName, ref string msgBuf, bool passDN = false)
        {
            bool gevIsReady;

            extHandle = false;
            _disposed = false;
            gevIsReady = gevGetReadyL(dirName, libName, ref msgBuf);
            if (!gevIsReady) return;
            if (passDN)
                gevxcreated(ref pgev, dirName);
            else
                gevxcreate(ref pgev);
            if (pgev != IntPtr.Zero)
            {
                msgBuf = String.Empty;
                return;
            }
            msgBuf = "Error while creating object";
        }
        public gevmcs(IntPtr gevHandle, ref string msgBuf)
        {
            bool gevIsReady;

            if (gevHandle == IntPtr.Zero)
            {
                msgBuf = "gevHandle is empty";
                return;
            }
            extHandle = true;
            _disposed = false;
            gevIsReady = gevGetReady(ref msgBuf);
            if (!gevIsReady) return;
            pgev = gevHandle;
        }
        public gevmcs(IntPtr gevHandle, string dirName, ref string msgBuf)
        {
            bool gevIsReady;

            if (gevHandle == IntPtr.Zero)
            {
                msgBuf = "gevHandle is empty";
                return;
            }
            extHandle = true;
            _disposed = false;
            gevIsReady = gevGetReadyD(dirName, ref msgBuf);
            if (!gevIsReady) return;
            pgev = gevHandle;
        }

        ~gevmcs()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (pgev != IntPtr.Zero)
                        gevFree();
                }
                // Indicate that the instance has been disposed.
                _disposed = true;
            }
            GC.KeepAlive(this);
        }

        public int gevFree()
        {
            if (!extHandle && pgev != IntPtr.Zero) gevxfree(ref pgev);
            return 1;
        }

        public bool gevLibraryUnload()
        {
            return FreeLibrary(h);
        }

        public IntPtr GetgevPtr()
        {
            return pgev;
        }

        public bool gevGetScreenIndicator()
        {
            return ScreenIndicator;
        }

        public void gevSetScreenIndicator(bool scrind)
        {
            ScreenIndicator = scrind;
        }

        public bool gevGetExceptionIndicator()
        {
            return ExceptionIndicator;
        }

        public void gevSetExceptionIndicator(bool excind)
        {
            ExceptionIndicator = excind;
        }

        public bool gevGetExitIndicator()
        {
            return ExitIndicator;
        }

        public void gevSetExitIndicator(bool extind)
        {
            ExitIndicator = extind;
        }

        public gevErrorCallback_t gevGetErrorCallback()
        {
            return ErrorCallBack;
        }

        public void gevSetErrorCallback(gevErrorCallback_t func)
        {
            ErrorCallBack = func;
        }

        public int gevGetAPIErrorCount()
        {
            return APIErrorCount;
        }

        public void gevSetAPIErrorCount(int ecnt)
        {
            APIErrorCount = ecnt;
        }

        private static void gevErrorHandling(string Msg)
        {
            APIErrorCount++;
            if (ScreenIndicator) Console.WriteLine(Msg);
            if (ErrorCallBack != null)
                if (ErrorCallBack(APIErrorCount, Msg)) Environment.Exit(123);
            if (ExceptionIndicator) throw new ArgumentNullException();
            if (ExitIndicator) Environment.Exit(123);
        }

        private void ConvertC2CS(byte[] b, ref string s)
        {
            int i;
            s = "";
            i = 0;
            while (b[i] != 0)
            {
                s = s + (char)(b[i]);
                i = i + 1;
            }
        }

        private int gevxapiversion(int api, ref string msg, ref int cl)
        {
            int rc_gevxapiversion;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_gevxapiversion = dll_gevxapiversion(api, cpy_msg, ref cl);
            msg = cpy_msg.ToString();
            return rc_gevxapiversion;
        }

        private int gevxcheck(string ep, int nargs, int[] s, ref string msg)
        {
            int rc_gevxcheck;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_gevxcheck = dll_gevxcheck(ep, nargs, s, cpy_msg);
            msg = cpy_msg.ToString();
            return rc_gevxcheck;
        }
        /// <summary>
        /// Register callback for log and status streams
        /// </summary>
        /// <param name="lsw">Pointer to callback for log and status streams</param>
        /// <param name="logenabled">Flag to enable log or not</param>
        /// <param name="usrmem">User memory</param>
        public void gevRegisterWriteCallback(Tgevlswrite lsw, bool logenabled, IntPtr usrmem)
        {
            int ib_logenabled = 0;
            if (logenabled) ib_logenabled = 1;
            dll_gevRegisterWriteCallback(pgev, lsw, ib_logenabled, usrmem);
        }
        /// <summary>
        /// Complete initialization of environment
        /// </summary>
        /// <param name="palg">Pointer to ALGX structure (GAMS Internal)</param>
        /// <param name="ivec">Array of integer options</param>
        /// <param name="rvec">Array of real/double options</param>
        /// <param name="svec">Array of string options</param>
        public void gevCompleteEnvironment(IntPtr palg, IntPtr ivec, IntPtr rvec, IntPtr svec)
        {
            dll_gevCompleteEnvironment(pgev, palg, ivec, rvec, svec);
        }
        /// <summary>
        /// Initialization in legacy mode (from control file)
        /// </summary>
        /// <param name="cntrfn">Name of control file</param>
        public int gevInitEnvironmentLegacy(string cntrfn)
        {
            return dll_gevInitEnvironmentLegacy(pgev, cntrfn);
        }
        /// <summary>
        /// Switch log and status streams to another file or callback
        /// </summary>
        /// <param name="lo">logoption (0..3)</param>
        /// <param name="logfn">Log file name</param>
        /// <param name="logappend">Flag whether to append to log stream or not</param>
        /// <param name="statfn">Status file name</param>
        /// <param name="statappend">Flag whether to append to status stream or not</param>
        /// <param name="lsw">Pointer to callback for log and status streams</param>
        /// <param name="usrmem">User memory</param>
        /// <param name="lshandle">Log and status handle for later restoring</param>
        public bool gevSwitchLogStat(int lo, string logfn, bool logappend, string statfn, bool statappend, Tgevlswrite lsw, IntPtr usrmem, ref IntPtr lshandle)
        {
            int ib_logappend = 0;
            int ib_statappend = 0;
            if (logappend) ib_logappend = 1;
            if (statappend) ib_statappend = 1;
            return dll_gevSwitchLogStat(pgev, lo, logfn, ib_logappend, statfn, ib_statappend, lsw, usrmem, ref lshandle) != 0;
        }
        /// <summary>
        /// Returns handle to last log and status stream stored by gevSwitchLogStat (Workaround for problem with vptr in Python)
        /// </summary>
        public IntPtr gevGetLShandle()
        {
            return dll_gevGetLShandle(pgev);
        }
        /// <summary>
        /// Restore log status stream settings
        /// </summary>
        /// <param name="lshandle">Log and status handle for later restoring</param>
        public bool gevRestoreLogStat(ref IntPtr lshandle)
        {
            return dll_gevRestoreLogStat(pgev, ref lshandle) != 0;
        }
        /// <summary>
        /// Restore log status stream settings but never append to former log
        /// </summary>
        /// <param name="lshandle">Log and status handle for later restoring</param>
        public bool gevRestoreLogStatRewrite(ref IntPtr lshandle)
        {
            return dll_gevRestoreLogStatRewrite(pgev, ref lshandle) != 0;
        }
        /// <summary>
        /// Send string to log stream
        /// </summary>
        /// <param name="s">String</param>
        public void gevLog(string s)
        {
            dll_gevLog(pgev, s);
        }
        /// <summary>
        /// Send PChar to log stream, no newline added
        /// </summary>
        /// <param name="p">Pointer to array of characters</param>
        public void gevLogPChar(byte[] p)
        {
            dll_gevLogPChar(pgev, p);
        }
        /// <summary>
        /// Send string to status stream
        /// </summary>
        /// <param name="s">String</param>
        public void gevStat(string s)
        {
            dll_gevStat(pgev, s);
        }
        /// <summary>
        /// Send string to status and copy to listing file
        /// </summary>
        /// <param name="s">String</param>
        public void gevStatC(string s)
        {
            dll_gevStatC(pgev, s);
        }
        /// <summary>
        /// Send PChar to status stream, no newline added
        /// </summary>
        /// <param name="p">Pointer to array of characters</param>
        public void gevStatPChar(byte[] p)
        {
            dll_gevStatPChar(pgev, p);
        }
        /// <summary>
        /// GAMS internal status stream operation {=0}
        /// </summary>
        /// <param name="s">String</param>
        public void gevStatAudit(string s)
        {
            dll_gevStatAudit(pgev, s);
        }
        /// <summary>
        /// GAMS internal status stream operation {=1}
        /// </summary>
        public void gevStatCon()
        {
            dll_gevStatCon(pgev);
        }
        /// <summary>
        /// GAMS internal status stream operation {=2}
        /// </summary>
        public void gevStatCoff()
        {
            dll_gevStatCoff(pgev);
        }
        /// <summary>
        /// GAMS internal status stream operation {=3}
        /// </summary>
        public void gevStatEOF()
        {
            dll_gevStatEOF(pgev);
        }
        /// <summary>
        /// GAMS internal status stream operation {=4}
        /// </summary>
        public void gevStatSysout()
        {
            dll_gevStatSysout(pgev);
        }
        /// <summary>
        /// GAMS internal status stream operation {=5}
        /// </summary>
        /// <param name="mi">Index or constraint</param>
        /// <param name="s">String</param>
        public void gevStatAddE(int mi, string s)
        {
            dll_gevStatAddE(pgev, mi, s);
        }
        /// <summary>
        /// GAMS internal status stream operation {=6}
        /// </summary>
        /// <param name="mj">Index or variable</param>
        /// <param name="s">String</param>
        public void gevStatAddV(int mj, string s)
        {
            dll_gevStatAddV(pgev, mj, s);
        }
        /// <summary>
        /// GAMS internal status stream operation {=7}
        /// </summary>
        /// <param name="mi">Index or constraint</param>
        /// <param name="mj">Index or variable</param>
        /// <param name="s">String</param>
        public void gevStatAddJ(int mi, int mj, string s)
        {
            dll_gevStatAddJ(pgev, mi, mj, s);
        }
        /// <summary>
        /// GAMS internal status stream operation {=8}
        /// </summary>
        public void gevStatEject()
        {
            dll_gevStatEject(pgev);
        }
        /// <summary>
        /// GAMS internal status stream operation {=9}
        /// </summary>
        /// <param name="c">Character</param>
        public void gevStatEdit(char c)
        {
            dll_gevStatEdit(pgev, c);
        }
        /// <summary>
        /// GAMS internal status stream operation {=E}
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="mi">Index or constraint</param>
        /// <param name="s2">String</param>
        public void gevStatE(string s, int mi, string s2)
        {
            dll_gevStatE(pgev, s, mi, s2);
        }
        /// <summary>
        /// GAMS internal status stream operation {=V}
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="mj">Index or variable</param>
        /// <param name="s2">String</param>
        public void gevStatV(string s, int mj, string s2)
        {
            dll_gevStatV(pgev, s, mj, s2);
        }
        /// <summary>
        /// GAMS internal status stream operation {=T}
        /// </summary>
        public void gevStatT()
        {
            dll_gevStatT(pgev);
        }
        /// <summary>
        /// GAMS internal status stream operation {=A}
        /// </summary>
        /// <param name="s">String</param>
        public void gevStatA(string s)
        {
            dll_gevStatA(pgev, s);
        }
        /// <summary>
        /// GAMS internal status stream operation {=B}
        /// </summary>
        /// <param name="s">String</param>
        public void gevStatB(string s)
        {
            dll_gevStatB(pgev, s);
        }
        /// <summary>
        /// Send string to log and status streams and copy to listing file
        /// </summary>
        /// <param name="s">String</param>
        public void gevLogStat(string s)
        {
            dll_gevLogStat(pgev, s);
        }
        /// <summary>
        /// Send string to log and status streams
        /// </summary>
        /// <param name="s">String</param>
        public void gevLogStatNoC(string s)
        {
            dll_gevLogStatNoC(pgev, s);
        }
        /// <summary>
        /// Send string to log and status streams, no newline added
        /// </summary>
        /// <param name="p">Pointer to array of characters</param>
        public void gevLogStatPChar(byte[] p)
        {
            dll_gevLogStatPChar(pgev, p);
        }
        /// <summary>
        /// Flush status streams (does not work with callback)
        /// </summary>
        public void gevLogStatFlush()
        {
            dll_gevLogStatFlush(pgev);
        }
        /// <summary>
        /// Get anchor line for log (points to file and is clickable in GAMS IDE)
        /// </summary>
        /// <param name="s">String</param>
        public string gevGetAnchor(string s)
        {
            string rc_gevGetAnchor = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gevGetAnchor(pgev, s, sst_result);
            rc_gevGetAnchor = sst_result.ToString();
            return rc_gevGetAnchor;
        }
        /// <summary>
        /// Put a line to log that points to the current lst line"
        /// </summary>
        /// <param name="s">String</param>
        public void gevLSTAnchor(string s)
        {
            dll_gevLSTAnchor(pgev, s);
        }
        /// <summary>
        /// Append status file to current status file
        /// </summary>
        /// <param name="statfn">Status file name</param>
        /// <param name="msg">Message</param>
        public int gevStatAppend(string statfn, ref string msg)
        {
            int rc_gevStatAppend;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_gevStatAppend = dll_gevStatAppend(pgev, statfn, cpy_msg);
            msg = cpy_msg.ToString();
            return rc_gevStatAppend;
        }
        /// <summary>
        /// Print MIP report to log and lst
        /// </summary>
        /// <param name="gmoptr">Pointer to GAMS modeling object</param>
        /// <param name="fixobj"></param>
        /// <param name="fixiter"></param>
        /// <param name="agap"></param>
        /// <param name="rgap"></param>
        public void gevMIPReport(IntPtr gmoptr, double fixobj, int fixiter, double agap, double rgap)
        {
            dll_gevMIPReport(pgev, gmoptr, fixobj, fixiter, agap, rgap);
        }
        /// <summary>
        /// Name of solver executable
        /// </summary>
        /// <param name="solvername">Name of solver</param>
        /// <param name="exename">Name of solver executable</param>
        public int gevGetSlvExeInfo(string solvername, ref string exename)
        {
            int rc_gevGetSlvExeInfo;
            StringBuilder cpy_exename = new StringBuilder(gamsglobals.str_len);
            rc_gevGetSlvExeInfo = dll_gevGetSlvExeInfo(pgev, solvername, cpy_exename);
            exename = cpy_exename.ToString();
            return rc_gevGetSlvExeInfo;
        }
        /// <summary>
        /// Solver library name, prefix, and API version
        /// </summary>
        /// <param name="solvername">Name of solver</param>
        /// <param name="libname">Name of solver library</param>
        /// <param name="prefix">Prefix of solver</param>
        /// <param name="ifversion">Version of solver interface</param>
        public int gevGetSlvLibInfo(string solvername, ref string libname, ref string prefix, ref int ifversion)
        {
            int rc_gevGetSlvLibInfo;
            StringBuilder cpy_libname = new StringBuilder(gamsglobals.str_len);
            StringBuilder cpy_prefix = new StringBuilder(gamsglobals.str_len);
            rc_gevGetSlvLibInfo = dll_gevGetSlvLibInfo(pgev, solvername, cpy_libname, cpy_prefix, ref ifversion);
            libname = cpy_libname.ToString();
            prefix = cpy_prefix.ToString();
            return rc_gevGetSlvLibInfo;
        }
        /// <summary>
        /// Check if solver is capable to handle model type given
        /// </summary>
        /// <param name="modeltype">Modeltype</param>
        /// <param name="solvername">Name of solver</param>
        /// <param name="capable">Flag whether solver is capable or not</param>
        public int gevCapabilityCheck(int modeltype, string solvername, ref bool capable)
        {
            int rc_gevCapabilityCheck;
            int ib_capable = 0;
            if (capable) ib_capable = 1;
            rc_gevCapabilityCheck = dll_gevCapabilityCheck(pgev, modeltype, solvername, ref ib_capable);
            capable = ib_capable != 0;
            return rc_gevCapabilityCheck;
        }
        /// <summary>
        /// Provide information if solver is hidden
        /// </summary>
        /// <param name="solvername">Name of solver</param>
        /// <param name="hidden"></param>
        /// <param name="defaultok"></param>
        public int gevSolverVisibility(string solvername, ref bool hidden, ref bool defaultok)
        {
            int rc_gevSolverVisibility;
            int ib_hidden = 0;
            int ib_defaultok = 0;
            if (hidden) ib_hidden = 1;
            if (defaultok) ib_defaultok = 1;
            rc_gevSolverVisibility = dll_gevSolverVisibility(pgev, solvername, ref ib_hidden, ref ib_defaultok);
            hidden = ib_hidden != 0;
            defaultok = ib_defaultok != 0;
            return rc_gevSolverVisibility;
        }
        /// <summary>
        /// Number of solvers in the system
        /// </summary>
        public int gevNumSolvers()
        {
            return dll_gevNumSolvers(pgev);
        }
        /// <summary>
        /// Name of the solver chosen for modeltype (if non is chosen, it is the default)
        /// </summary>
        /// <param name="modeltype">Modeltype</param>
        public string gevGetSolver(int modeltype)
        {
            string rc_gevGetSolver = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gevGetSolver(pgev, modeltype, sst_result);
            rc_gevGetSolver = sst_result.ToString();
            return rc_gevGetSolver;
        }
        /// <summary>
        /// Name of the select solver
        /// </summary>
        /// <param name="gmoptr">Pointer to GAMS modeling object</param>
        public string gevGetCurrentSolver(IntPtr gmoptr)
        {
            string rc_gevGetCurrentSolver = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gevGetCurrentSolver(pgev, gmoptr, sst_result);
            rc_gevGetCurrentSolver = sst_result.ToString();
            return rc_gevGetCurrentSolver;
        }
        /// <summary>
        /// Name of the default solver for modeltype
        /// </summary>
        /// <param name="modeltype">Modeltype</param>
        public string gevGetSolverDefault(int modeltype)
        {
            string rc_gevGetSolverDefault = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gevGetSolverDefault(pgev, modeltype, sst_result);
            rc_gevGetSolverDefault = sst_result.ToString();
            return rc_gevGetSolverDefault;
        }
        /// <summary>
        /// Internal ID of solver, 0 for failure
        /// </summary>
        /// <param name="solvername">Name of solver</param>
        public int gevSolver2Id(string solvername)
        {
            return dll_gevSolver2Id(pgev, solvername);
        }
        /// <summary>
        /// Solver name
        /// </summary>
        /// <param name="solverid">Internal ID of solver</param>
        public string gevId2Solver(int solverid)
        {
            string rc_gevId2Solver = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gevId2Solver(pgev, solverid, sst_result);
            rc_gevId2Solver = sst_result.ToString();
            return rc_gevId2Solver;
        }
        /// <summary>
        /// Creates grid directory for next gevCallSolver call and returns name (if called with gevSolveLinkAsyncGrid or gevSolveLinkAsyncSimulate)
        /// </summary>
        public string gevCallSolverNextGridDir()
        {
            string rc_gevCallSolverNextGridDir = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gevCallSolverNextGridDir(pgev, sst_result);
            rc_gevCallSolverNextGridDir = sst_result.ToString();
            return rc_gevCallSolverNextGridDir;
        }
        /// <summary>
        /// Call GAMS solver on GMO model or control file
        /// </summary>
        /// <param name="gmoptr">Pointer to GAMS modeling object</param>
        /// <param name="cntrfn">Name of control file</param>
        /// <param name="solvername">Name of solver</param>
        /// <param name="solvelink">Solvelink option for solver called through gevCallSolver (see enumerated constants)</param>
        /// <param name="Logging">Log option for solver called through gevCallSolver (see enumerated constants)</param>
        /// <param name="logfn">Log file name</param>
        /// <param name="statfn">Status file name</param>
        /// <param name="reslim">Resource limit</param>
        /// <param name="iterlim">Iteration limit</param>
        /// <param name="domlim">Domain violation limit</param>
        /// <param name="optcr">Optimality criterion for relative gap</param>
        /// <param name="optca">Optimality criterion for absolute gap</param>
        /// <param name="jobhandle">Handle to solver job in case of solvelink=gevSolveLinkAsyncGrid</param>
        /// <param name="msg">Message</param>
        public int gevCallSolver(IntPtr gmoptr, string cntrfn, string solvername, int solvelink, int Logging, string logfn, string statfn, double reslim, int iterlim, int domlim, double optcr, double optca, ref IntPtr jobhandle, ref string msg)
        {
            int rc_gevCallSolver;
            StringBuilder cpy_msg = new StringBuilder(gamsglobals.str_len);
            rc_gevCallSolver = dll_gevCallSolver(pgev, gmoptr, cntrfn, solvername, solvelink, Logging, logfn, statfn, reslim, iterlim, domlim, optcr, optca, ref jobhandle, cpy_msg);
            msg = cpy_msg.ToString();
            return rc_gevCallSolver;
        }
        /// <summary>
        /// Check status of solver job if called with gevSolveLinkAsyncGrid (0 job is done, 1 unknown job handle, 2 job is running)
        /// </summary>
        /// <param name="jobhandle">Handle to solver job in case of solvelink=gevSolveLinkAsyncGrid</param>
        public int gevCallSolverHandleStatus(IntPtr jobhandle)
        {
            return dll_gevCallSolverHandleStatus(pgev, jobhandle);
        }
        /// <summary>
        /// Delete instance of solver job if called with gevSolveLinkAsyncGrid (0 deleted, 1 unknown job handle, 2 deletion failed)
        /// </summary>
        /// <param name="jobhandle">Handle to solver job in case of solvelink=gevSolveLinkAsyncGrid</param>
        public int gevCallSolverHandleDelete(ref IntPtr jobhandle)
        {
            return dll_gevCallSolverHandleDelete(pgev, ref jobhandle);
        }
        /// <summary>
        /// Collect solution from solver job if called with gevSolveLinkAsyncGrid (0 loaded, 1 unknown job handle, 2 job is running, 3 other error), delete instance
        /// </summary>
        /// <param name="jobhandle">Handle to solver job in case of solvelink=gevSolveLinkAsyncGrid</param>
        /// <param name="gmoptr">Pointer to GAMS modeling object</param>
        public int gevCallSolverHandleCollect(ref IntPtr jobhandle, IntPtr gmoptr)
        {
            return dll_gevCallSolverHandleCollect(pgev, ref jobhandle, gmoptr);
        }
        /// <summary>
        /// Get integer valued option (see enumerated constants)
        /// </summary>
        /// <param name="optname">Name of option (see enumerated constants)</param>
        public int gevGetIntOpt(string optname)
        {
            return dll_gevGetIntOpt(pgev, optname);
        }
        /// <summary>
        /// Get double valued option (see enumerated constants)
        /// </summary>
        /// <param name="optname">Name of option (see enumerated constants)</param>
        public double gevGetDblOpt(string optname)
        {
            return dll_gevGetDblOpt(pgev, optname);
        }
        /// <summary>
        /// Get string valued option (see enumerated constants)
        /// </summary>
        /// <param name="optname">Name of option (see enumerated constants)</param>
        public string gevGetStrOpt(string optname)
        {
            string rc_gevGetStrOpt = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gevGetStrOpt(pgev, optname, sst_result);
            rc_gevGetStrOpt = sst_result.ToString();
            return rc_gevGetStrOpt;
        }
        /// <summary>
        /// Set integer valued option (see enumerated constants)
        /// </summary>
        /// <param name="optname">Name of option (see enumerated constants)</param>
        /// <param name="ival">Integer value</param>
        public void gevSetIntOpt(string optname, int ival)
        {
            dll_gevSetIntOpt(pgev, optname, ival);
        }
        /// <summary>
        /// Set double valued option (see enumerated constants)
        /// </summary>
        /// <param name="optname">Name of option (see enumerated constants)</param>
        /// <param name="rval">Real/Double value</param>
        public void gevSetDblOpt(string optname, double rval)
        {
            dll_gevSetDblOpt(pgev, optname, rval);
        }
        /// <summary>
        /// Set string valued option (see enumerated constants)
        /// </summary>
        /// <param name="optname">Name of option (see enumerated constants)</param>
        /// <param name="sval">String value</param>
        public void gevSetStrOpt(string optname, string sval)
        {
            dll_gevSetStrOpt(pgev, optname, sval);
        }
        /// <summary>
        /// Copy environment options to passed in option object
        /// </summary>
        /// <param name="optptr">Pointer to option object</param>
        public void gevSynchronizeOpt(IntPtr optptr)
        {
            dll_gevSynchronizeOpt(pgev, optptr);
        }
        /// <summary>
        /// GAMS Julian time
        /// </summary>
        public double gevTimeJNow()
        {
            return dll_gevTimeJNow(pgev);
        }
        /// <summary>
        /// Time difference in seconds since creation or last call to gevTimeDiff
        /// </summary>
        public double gevTimeDiff()
        {
            return dll_gevTimeDiff(pgev);
        }
        /// <summary>
        /// Time difference in seconds since creation of object
        /// </summary>
        public double gevTimeDiffStart()
        {
            return dll_gevTimeDiffStart(pgev);
        }
        /// <summary>
        /// Reset timer (overwrites time stamp from creation)
        /// </summary>
        public void gevTimeSetStart()
        {
            dll_gevTimeSetStart(pgev);
        }
        /// <summary>
        /// Uninstalls an already registered interrupt handler
        /// </summary>
        public void gevTerminateUninstall()
        {
            dll_gevTerminateUninstall(pgev);
        }
        /// <summary>
        /// Installs an already registered interrupt handler
        /// </summary>
        public void gevTerminateInstall()
        {
            dll_gevTerminateInstall(pgev);
        }
        /// <summary>
        /// Register a pointer to some memory that will indicate an interrupt and the pointer to a interrupt handler and installs it
        /// </summary>
        /// <param name="intr">Pointer to some memory indicating an interrupt</param>
        /// <param name="ehdler">Pointer to interrupt handler</param>
        public void gevTerminateSet(IntPtr intr, IntPtr ehdler)
        {
            dll_gevTerminateSet(pgev, intr, ehdler);
        }
        /// <summary>
        /// Check if one should interrupt
        /// </summary>
        public bool gevTerminateGet()
        {
            return dll_gevTerminateGet(pgev) != 0;
        }
        /// <summary>
        /// Resets the interrupt counter
        /// </summary>
        public void gevTerminateClear()
        {
            dll_gevTerminateClear(pgev);
        }
        /// <summary>
        /// Increases the interrupt counter
        /// </summary>
        public void gevTerminateRaise()
        {
            dll_gevTerminateRaise(pgev);
        }
        /// <summary>
        /// Get installed termination handler
        /// </summary>
        /// <param name="intr">Pointer to some memory indicating an interrupt</param>
        /// <param name="ehdler">Pointer to interrupt handler</param>
        public void gevTerminateGetHandler(ref IntPtr intr, ref IntPtr ehdler)
        {
            dll_gevTerminateGetHandler(pgev, ref intr, ref ehdler);
        }
        /// <summary>
        /// Get scratch file name plus scratch extension including path of scratch directory
        /// </summary>
        /// <param name="s">String</param>
        public string gevGetScratchName(string s)
        {
            string rc_gevGetScratchName = ""; ;
            StringBuilder sst_result = new StringBuilder(gamsglobals.str_len);
            dll_gevGetScratchName(pgev, s, sst_result);
            rc_gevGetScratchName = sst_result.ToString();
            return rc_gevGetScratchName;
        }
        /// <summary>
        /// Creates model instance file
        /// </summary>
        /// <param name="mifn">Model instance file name</param>
        /// <param name="gmoptr">Pointer to GAMS modeling object</param>
        /// <param name="nlcodelen">Length of nonlinear code</param>
        public int gevWriteModelInstance(string mifn, IntPtr gmoptr, ref int nlcodelen)
        {
            return dll_gevWriteModelInstance(pgev, mifn, gmoptr, ref nlcodelen);
        }
        /// <summary>
        /// Duplicates a scratch directory and points to read only files in source scratch directory
        /// </summary>
        /// <param name="scrdir">Scratch directory</param>
        /// <param name="logfn">Log file name</param>
        /// <param name="cntrfn">Name of control file</param>
        public int gevDuplicateScratchDir(string scrdir, string logfn, ref string cntrfn)
        {
            int rc_gevDuplicateScratchDir;
            StringBuilder cpy_cntrfn = new StringBuilder(gamsglobals.str_len);
            rc_gevDuplicateScratchDir = dll_gevDuplicateScratchDir(pgev, scrdir, logfn, cpy_cntrfn);
            cntrfn = cpy_cntrfn.ToString();
            return rc_gevDuplicateScratchDir;
        }
        /// <summary>
        /// Legacy Jacobian Evaluation: Initialize row wise Jacobian structure
        /// </summary>
        /// <param name="evalptr">Pointer to structure for legacy Jacobian evaluation</param>
        /// <param name="gmoptr">Pointer to GAMS modeling object</param>
        public int gevInitJacLegacy(ref IntPtr evalptr, IntPtr gmoptr)
        {
            return dll_gevInitJacLegacy(pgev, ref evalptr, gmoptr);
        }
        /// <summary>
        /// Legacy Jacobian Evaluation: Set column and row permutation GAMS to solver
        /// </summary>
        /// <param name="evalptr">Pointer to structure for legacy Jacobian evaluation</param>
        /// <param name="n">Number of variables</param>
        /// <param name="cgms2slv">GAMS to solver permutation of columns</param>
        /// <param name="m">Number of constraints</param>
        /// <param name="rgms2slv">GAMS to solver permutation of rows</param>
        public void gevSetColRowPermLegacy(IntPtr evalptr, int n, ref int[] cgms2slv, int m, ref int[] rgms2slv)
        {
            dll_gevSetColRowPermLegacy(pgev, evalptr, n, cgms2slv, m, rgms2slv);
        }
        /// <summary>
        /// Legacy Jacobian Evaluation: Set Jacobian permutation GAMS to solver
        /// </summary>
        /// <param name="evalptr">Pointer to structure for legacy Jacobian evaluation</param>
        /// <param name="njacs">Number of Jacobian elements in jacs and jgms2slv arrays</param>
        /// <param name="jacs">Array of original indices of Jacobian elements (1-based), length njacs</param>
        /// <param name="jgms2slv">GAMS to solver permutation of Jacobian elements, length njacs</param>
        public void gevSetJacPermLegacy(IntPtr evalptr, int njacs, ref int[] jacs, ref int[] jgms2slv)
        {
            dll_gevSetJacPermLegacy(pgev, evalptr, njacs, jacs, jgms2slv);
        }
        /// <summary>
        /// Legacy Jacobian Evaluation: Set new point and do point copy magic
        /// </summary>
        /// <param name="evalptr">Pointer to structure for legacy Jacobian evaluation</param>
        /// <param name="x">Input values for variables</param>
        public int gevEvalNewPointLegacy(IntPtr evalptr, ref double[] x)
        {
            return dll_gevEvalNewPointLegacy(pgev, evalptr, x);
        }
        /// <summary>
        /// Legacy Jacobian Evaluation: Evaluate row and store in Jacobian structure
        /// </summary>
        /// <param name="evalptr">Pointer to structure for legacy Jacobian evaluation</param>
        /// <param name="si">Solve index for row i</param>
        /// <param name="x">Input values for variables</param>
        /// <param name="f">Function value</param>
        /// <param name="jac">Array to store the gradients</param>
        /// <param name="domviol">Domain violations</param>
        /// <param name="njacsupd">Number of Jacobian elements updated</param>
        public int gevEvalJacLegacy(IntPtr evalptr, int si, ref double[] x, ref double f, ref double[] jac, ref int domviol, ref int njacsupd)
        {
            return dll_gevEvalJacLegacy(pgev, evalptr, si, x, ref f, jac, ref domviol, ref njacsupd);
        }
        /// <summary>
        /// Legacy Jacobian Evaluation: Evaluate set of rows and store in Jacobian structure
        /// </summary>
        /// <param name="evalptr">Pointer to structure for legacy Jacobian evaluation</param>
        /// <param name="cnt">count</param>
        /// <param name="rowidx">Vector of row indicies</param>
        /// <param name="x">Input values for variables</param>
        /// <param name="fvec">Vector of function values</param>
        /// <param name="jac">Array to store the gradients</param>
        /// <param name="domviol">Domain violations</param>
        /// <param name="njacsupd">Number of Jacobian elements updated</param>
        public int gevEvalJacLegacyX(IntPtr evalptr, int cnt, ref int[] rowidx, ref double[] x, ref double[] fvec, ref double[] jac, ref int domviol, ref int njacsupd)
        {
            return dll_gevEvalJacLegacyX(pgev, evalptr, cnt, rowidx, x, fvec, jac, ref domviol, ref njacsupd);
        }
        /// <summary>
        /// Legacy Jacobian Evaluation: Provide next nonlinear row, start with M
        /// </summary>
        /// <param name="evalptr">Pointer to structure for legacy Jacobian evaluation</param>
        /// <param name="si">Solve index for row i</param>
        public int gevNextNLLegacy(IntPtr evalptr, int si)
        {
            return dll_gevNextNLLegacy(pgev, evalptr, si);
        }
        /// <summary>
        /// Legacy Jacobian Evaluation: Provide permuted row index
        /// </summary>
        /// <param name="evalptr">Pointer to structure for legacy Jacobian evaluation</param>
        /// <param name="si">Solve index for row i</param>
        public int gevRowGms2SlvLegacy(IntPtr evalptr, int si)
        {
            return dll_gevRowGms2SlvLegacy(pgev, evalptr, si);
        }
        /// <summary>
        /// Legacy Jacobian Evaluation: Free row wise Jacobian structure
        /// </summary>
        /// <param name="evalptr">Pointer to structure for legacy Jacobian evaluation</param>
        public void gevFreeJacLegacy(ref IntPtr evalptr)
        {
            dll_gevFreeJacLegacy(pgev, ref evalptr);
        }
        /// <summary>
        /// Pass pointer to ALGX structure
        /// </summary>
        public IntPtr gevGetALGX()
        {
            return dll_gevGetALGX(pgev);
        }

        /// <summary>
        /// Prevent log and status file to be opened
        /// </summary>
        public void gevSkipIOLegacySet(bool x)
        {
            int ib_x = 0;
            if (x) ib_x = 1;
            dll_gevSkipIOLegacySet(pgev, ib_x);
        }
        /// <summary>
        /// Number of threads (1..n)
        /// </summary>
        public int gevThreads()
        {
            return dll_gevThreads(pgev);
        }
        /// <summary>
        /// Number of solves
        /// </summary>
        public double gevNSolves()
        {
            return dll_gevNSolves(pgev);
        }

    }
}

