using System;
using System.Collections.Generic;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Linq.Expressions;
using System.Drawing;

namespace Gekko.Parser.Gek
{
    /// <summary>
    /// This class is used to create an AST from a .get (script) file. There is a similar class for model files.
    /// </summary>
    public class ParserGekCreateAST
    {

        public static ConvertHelper CreateAST(ParseHelper ph, P p)
        {
            Q q = new Q();  //make a fresh container for method argument helpers
            p.SetQ(q);

            ConvertHelper ch2 = new ConvertHelper();

            string textInput = ph.commandsText + "\r\n" + "\r\n"; //newlines for ease of use of ANTLR

            ANTLRStringStream input = new ANTLRStringStream(textInput);

            List<string> errors = null;
            CommonTree t = null;
            // Create a lexer attached to that input            
            Cmd2Parser parser2 = null;
            Cmd2Lexer lexer2 = new Cmd2Lexer(input);
            //usually debugTokens=false, and this is stepped into manually (otherwise the tokens are consumed and preliminary steps cannot be run)
            if (Globals.runningOnTTComputer && Globals.debugTokens) Gekko.Parser.ParserCommon.DebugTokens(lexer2);
            // Create a stream of tokens pulled from the lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer2);
            // Create a parser attached to the token stream
            parser2 = new Cmd2Parser(tokens);
            // Invoke the program rule in get return value
            Cmd2Parser.expr_return r = null;
            DateTime t0 = DateTime.Now;

            try
            {
                r = parser2.expr();
            }
            catch (Exception e)
            {
                //G.Writeln(e.Message);
                List<string> temp = new List<string>();
                temp.Add(e.Message);
                //string textInput = ph.commandsText + "\r\n";
                string input2 = textInput + "\r\n";
                PrintLexerErrors(temp, Program.CreateListOfStringsFromString(input2), ph);
                throw new GekkoException(); //this will make a double error -- but the other one will be identified later on (both text and filename are null) and skipped -- a little bit hacky, but oh well...
            }

            if (parser2.GetErrors().Count > 0)
            {
                PrintParserErrors(parser2.GetErrors(), Program.CreateListOfStringsFromString(textInput), ph);
                throw new GekkoException();
            }
            t = (CommonTree)r.Tree;


            if (Globals.printAST)
            {
                PrintAST(t, 0);
            }

            GekkoStringBuilder s = new GekkoStringBuilder();
            W wh2 = new W();

            //TODO
            //TODO
            //TODO  one list for each command line
            //TODO  
            //TODO
            //EmitCsCodeForCmdHelperStatic helperStatic = new EmitCsCodeForCmdHelperStatic(textInput);
                        
            wh2.fileNameContainingParsedCode = ph.fileName;

            ASTNode root = new ASTNode(null);
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("Create AST start: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);
            CreateASTNodesForCmd(t, root, 0);
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("Create AST end: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);

            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("EMIT cs start: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);

            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("WALK START");
            Gekko.Parser.Gek.ParserGekWalkASTAndEmit.WalkASTAndEmit(root, 0, 0, textInput, wh2, p);
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("WALK END");

            s.AppendLine(root.Code.ToString());

            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("EMIT cs end: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);

            //if (Globals.testing) G.Writeln(s.ToString(), Color.Gray);

            string csCode = s.ToString();
            string csMethods = null;            
                        
            if (Program.options.system_code_split > 0) CodeSplit(ref csCode, ref csMethods);            

            StringBuilder s2 = new StringBuilder();
            s2.AppendLine("using System;");
            s2.AppendLine("using System.Collections.Generic;");
            s2.AppendLine("using System.Text;");
            s2.AppendLine("using System.Windows.Forms;");
            s2.AppendLine("using System.Drawing;");  //to use Color.Red in G.Writeln()
            s2.AppendLine("using Gekko.Parser;"); //all the AST_Xxx() methods are found in Gekko.Parser.AST.cs
            s2.AppendLine("namespace Gekko");
            s2.AppendLine("{");            

            s2.AppendLine("public class TranslatedCode");
            s2.AppendLine("{");

            s2.AppendLine("public static GekkoTime globalGekkoTimeIterator = Globals.tNull;");
            s2.Append(wh2.headerCs);            

            s2.AppendLine("public static void ClearTS(P p) {");
            s2.Append(wh2.headerMethodTsCs);
            s2.AppendLine("}");

            s2.AppendLine("public static void ClearScalar(P p) {");
            s2.Append(wh2.headerMethodScalarCs);
            s2.AppendLine("}");
            
            s2.AppendLine(csMethods);  //definitions of C1(), C2(), etc. -- may be empty
            
            s2.AppendLine("public static void CodeLines(P p)");
            s2.AppendLine("{");
            s2.AppendLine(Globals.gekkoTimeIniCs);
            //The generated code, but much of it may be in codeCi
            s2.AppendLine(csCode);

            s2.AppendLine("}");  //method CodeLines()
            s2.AppendLine("}");  //class TranslatedCode

            s2.AppendLine("}");  //namespace Gekko            

            ch2.code = s2.ToString().Replace("`", Globals.QT);
            ch2.errors = errors;  //not used?
            
            if(wh2.uHeaderCs.Length != 0)
            {
                StringBuilder s3 = new StringBuilder();                
                s3.AppendLine("using System;");
                s3.AppendLine("using System.Collections.Generic;");
                s3.AppendLine("using System.Text;");
                s3.AppendLine("using System.Windows.Forms;");
                s3.AppendLine("using System.Drawing;");  //to use Color.Red in G.Writeln()
                s3.AppendLine("using Gekko.Parser;"); //all the AST_Xxx() methods are found in Gekko.Parser.AST.cs
                s3.AppendLine("namespace Gekko");
                s3.AppendLine("{");
                s3.AppendLine("public class UProc");
                s3.AppendLine("{");
                s3.AppendLine("public static GekkoTime globalGekkoTimeIterator = Globals.tNull;");
                s3.Append(wh2.uHeaderCs);                
                s3.AppendLine("}");  //class UProc
                s3.AppendLine("}");  //namespace Gekko
                ch2.codeUFunctions = s3.ToString().Replace("`", Globals.QT);
            }

            if (Globals.printAST)
            {
                G.Writeln(ch2.codeUFunctions);
                G.Writeln("===============================");
                G.Writeln("===============================");
                G.Writeln("===============================");
                G.Writeln(ch2.code);
            }

            return ch2;
        }

        private static void CodeSplit(ref string csCode, ref string csMethods)
        {
            List<string> csLines = G.ExtractLinesFromText(csCode);
            List<string> tempHelperForMethodsCs = new List<string>();  //temp storage  
            List<string> mainCs = new List<string>();  //this is the rest                      
            List<string> methodsCs = new List<string>();  //this is the rest                      

            int state = 0;            
            int nCi = 0; //The Ci number            
            int counterComments = 0; //These are skipped and counted           
            int counterExtra = 0;  //This is extra lines in Ci-definitions etc.

            int commandCounter = 0;
            int commandState = 0;

            foreach (string line in csLines)
            {
                if (line.StartsWith(Globals.splitSTART2))
                {
                    if (state == 1)  //should alternate
                    {
                        G.Writeln2("*** ERROR: Codesplitting, state 1");
                        throw new GekkoException();
                    }
                    state = 1;
                    counterComments++;
                    continue;
                }
                else if (line.StartsWith(Globals.splitSTOP2))
                {
                    if (state == 2)  //should alternate
                    {
                        G.Writeln2("*** ERROR: Codesplitting, state 2");
                        throw new GekkoException();
                    }
                    state = 2;
                    //adds a Ci() line instead of the comment, so counterComments is not touched
                    CodeSplitFlush(tempHelperForMethodsCs, mainCs, methodsCs, ref nCi, ref counterExtra);
                    continue;
                }
                else if (line.StartsWith(Globals.splitCommandBlockStart))
                {
                    if (commandState == 100)  //should alternate
                    {
                        G.Writeln2("*** ERROR: Codesplitting, state 100");
                        throw new GekkoException();
                    }
                    commandState = 100;
                    commandCounter++;
                    
                    if (commandCounter >= Program.options.system_code_split)  //this will put at most that amount of commands inside each Ci()
                    {
                        //adds a Ci() line instead of the comment, so counterComments is not touched
                        CodeSplitFlush(tempHelperForMethodsCs, mainCs, methodsCs, ref nCi, ref counterExtra);
                        commandCounter = 0;
                    }
                    else
                    {
                        counterComments++;
                    }                    
                    continue;
                }
                else if (line.StartsWith(Globals.splitCommandBlockEnd))
                {
                    if (commandState == 200)  //should alternate
                    {
                        G.Writeln2("*** ERROR: Codesplitting, state 200");
                        throw new GekkoException();
                    }
                    commandState = 200;                    
                    counterComments++;
                    continue;
                }
                else
                {
                    if (state == 0 || state == 2)
                    {
                        //Not allowed to put into Ci()
                        mainCs.Add(line);
                    }
                    else if (state == 1)
                    {
                        //Allowed to put into Ci()
                        tempHelperForMethodsCs.Add(line);
                    }
                }
            }
            if (csLines.Count + counterExtra - counterComments - mainCs.Count - methodsCs.Count  != 0)
            {
                G.Writeln2("*** ERROR: Codesplitting");
                throw new GekkoException();
            }
            csCode = G.ExtractTextFromLines(mainCs).ToString();
            csMethods = G.ExtractTextFromLines(methodsCs).ToString();            
        }

        private static void CodeSplitFlush(List<string> tempHelperForMethodsCs, List<string> mainCs, List<string> methodsCs, ref int nCi, ref int counterExtra)
        {
            methodsCs.Add("public static void C" + nCi + "(P p) {" + G.NL); counterExtra++;
            methodsCs.Add(Globals.gekkoTimeIniCs + G.NL); counterExtra++;
            methodsCs.AddRange(tempHelperForMethodsCs);
            methodsCs.Add("}" + G.NL); counterExtra++;
            mainCs.Add("C" + nCi + "(p);" + G.NL);
            tempHelperForMethodsCs.Clear();
            nCi++;
        }        

        public static void CreateASTNodesForCmd(CommonTree ast, ASTNode cmdNode, int depth)
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

            if (ast.Text != null)
            {
                bool flag = false;

                if (ast.Text.Contains("¤"))
                {
                    if (ast.Text.StartsWith("ASTMETA" + "¤"))  //Handles SERIES, that is ASTGENR/ASTUPD
                    {
                        flag = true;
                    }                    
                    else if (ast.Text.StartsWith("ASTSHOW" + "¤"))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTPRTELEMENT" + "¤"))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTOLSELEMENT" + "¤"))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTDECOMP" + "¤"))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTLIST" + "¤"))
                    {
                        flag = true;
                    }
                }                
                
                if (flag)
                {
                    string[] ss = ast.Text.Split('¤');
                    for (int i = 0; i < ss.Length; i++)
                    {                        
                        ss[i] = G.ReplaceGlueNew(ss[i]);                        
                    }
                    cmdNode.Text = ss[0];
                    cmdNode.specialExpressionAndLabelInfo = ss;
                }
            }

            if (cmdNode.Text != null)
            {
                if (Globals.addGlue)
                {
                    cmdNode.Text = G.ReplaceGlueNew(cmdNode.Text);
                }
            }
            cmdNode.Line = ast.Line;
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
                ASTNode cmdNodeChild = new ASTNode(null);  //unknown text
                cmdNodeChild.Parent = cmdNode;
                cmdNode.Add(cmdNodeChild);
                CreateASTNodesForCmd(d, cmdNodeChild, depth + 1);
            }
        }

        private static bool DetectNullNode(CommonTree ast)
        {
            return ast.Text == null && !(ast.Children != null && ast.Children.Count > 0);
        }

        public static void PrintLexerErrors(List<string> errors, List<string> inputFileLines, ParseHelper ph)
        {
            if (Globals.threadIsInProcessOfAborting) return;
            if (ph.fileName == null && ph.commandsText == null)
            {
                //ignore, probably an error dublet
                return;
            }
            if (Globals.pipe == true) Program.Pipe("con", null);
            int number = 0;
            foreach (string s in errors)
            {
                number++;
                if (errors.Count > 1)  //always just one
                {
                    if (number == 1) G.Writeln();
                    G.Writeln("--------------------- error #" + number + " of " + errors.Count + "-----------------");
                    //G.Writeln();
                }
                else G.Writeln();

                string[] ss = s.Split(Globals.parserErrorSeparator);
                int lineNumber = int.Parse(ss[0]) - 1;  //seems 1-based before subtract 1
                int lineNo = lineNumber + 1;  //1-based
                int positionNo = int.Parse(ss[1]) + 1;  //1-based

                string errorMessage = ss[3];

                //if (!Globals.useTestParser)
                //{
                //    errorMessage = errorMessage.Replace(" RPGLUE", " ')'");
                //    errorMessage = errorMessage.Replace(" LPGLUE", " '('");
                //    errorMessage = errorMessage.Replace(" RP", " ')'");
                //    errorMessage = errorMessage.Replace(" LP", " '('");
                //    //errorMessage = errorMessage.Replace("expecting set", "");  //remove this, is confusing
                //}

                errorMessage = errorMessage.Replace(@"'\\r\\n'", "<newline>");  //easier to understand

                if (lineNo > inputFileLines.Count)
                {
                    {
                        G.Writeln("*** ERROR: " + errorMessage);
                    }

                    continue;  //doesn't give meaning
                }
                string line = inputFileLines[lineNo - 1];
                int firstWordPosInLine = line.Length - line.TrimStart().Length + 1;

                bool previousLineProbablyCulprit = false;
                if (positionNo == firstWordPosInLine && errorMessage.Contains("no viable"))
                {
                    //get preceding line (or really: statement) -- most probably the culprit.
                    previousLineProbablyCulprit = true;
                }

                string paranthesesError = "";

                if (ph.isOneLinerFromGui == true && lineNo != 1)
                {
                    G.Writeln("*** ERROR: Parsing this line:");
                    G.Writeln("    " + G.ReplaceGlueNew(inputFileLines[0]), Color.Blue);
                    G.Writeln("*** ERROR: " + errorMessage);
                }
                else
                {
                    if (ph.isOneLinerFromGui == false)
                    {
                        {
                            string fn = ph.fileName;
                            if (fn == null || fn == "")
                            {
                                G.Writeln("*** ERROR: Parsing user input block, line " + lineNo + " pos " + positionNo);
                            }
                            else
                            {
                                G.Writeln("*** ERROR: Parsing file: " + fn + " line " + lineNo + " pos " + positionNo);
                            }

                            string e2 = errorMessage.Replace("Der blev udløst en undtagelse af typen ", "");
                            G.Writeln("           " + e2);
                        }
                    }
                    else
                    {
                        G.Writeln("*** ERROR: Parsing pos " + positionNo + ":  " + errorMessage);
                    }
                    line = line + "  ";  //hack to avoid ending problems.....
                    string lineTemp = line;
                    string line0 = lineTemp.Substring(0, positionNo - 1);
                    string line1 = lineTemp.Substring(positionNo - 1, 1);
                    string line2 = lineTemp.Substring(positionNo - 1 + 1);

                    if (previousLineProbablyCulprit && lineNo > 1)
                    {
                        G.Writeln("    " + "Line " + (lineNo - 1) + " may be the real cause of the problem");
                        string lineBefore = inputFileLines[lineNo - 1 - 1];
                        G.Writeln("    " + "[" + G.IntFormat(lineNo - 1, 4) + "]:" + "   " + G.ReplaceGlueNew(lineBefore), Color.Blue);
                    }

                    G.Write("    " + "[" + G.IntFormat(lineNo, 4) + "]:" + "   " + G.ReplaceGlueNew(line0), Color.Blue);
                    G.Write(G.ReplaceGlueNew(line1), Color.Red);
                    G.Writeln(G.ReplaceGlueNew(line2), Color.Blue);

                    G.Writeln(G.Blanks(positionNo - 1 + 4 + 5 + 5) + "^", Color.Blue);
                    G.Writeln(G.Blanks(positionNo - 1 + 4 + 5 + 5) + "^", Color.Blue);
                    //G.Writeln();
                }

                if (paranthesesError != "") G.Writeln(paranthesesError);

                if (ph.isModel == false && previousLineProbablyCulprit == false)
                {
                    WriteLinkToHelpFile2(G.ReplaceGlueNew(line));
                    if (number == 1) ExtraErrorMessages(G.ReplaceGlueNew(line));
                }
            }
            if (errors.Count > 1) G.Writeln("--------------------- end of " + errors.Count + " errors --------------");
        }

        public static void PrintParserErrors(List<string> errors, List<string> inputFileLines, ParseHelper ph)
        {            
            List<string> lineTemp2 = new List<string>();
            List<string> lineTemp2Numbers = new List<string>();
            if (Globals.threadIsInProcessOfAborting) return;
            if (Globals.pipe == true) Program.Pipe("con", null);
            int number = 0;
            foreach (string s in errors)
            {
                number++;
                if (errors.Count > 1)
                {
                    if (number == 1) G.Writeln();
                    G.Writeln("--------------------- error #" + number + " of " + errors.Count + "-----------------");
                    //G.Writeln();
                }
                else G.Writeln();


                string[] ss = s.Split(Globals.parserErrorSeparator);
                int lineNumber = 0;
                int lineNo = 0;
                int positionNo = 0;
                string errorMessage = "General error";

                try
                {
                    lineNumber = int.Parse(ss[0]) - 1;  //seems 1-based before subtract 1                
                    lineNo = lineNumber + 1;  //1-based
                    positionNo = int.Parse(ss[1]) + 1;  //1-based                               
                    errorMessage = ss[3];
                }
                catch
                {

                }

                if (Globals.addGlue)
                {
                    errorMessage = G.ReplaceGlueNew(errorMessage);
                }

                if (true)
                {
                    errorMessage = errorMessage.Replace(" AT", " '@'");
                    errorMessage = errorMessage.Replace(" HAT", " '^'");
                    errorMessage = errorMessage.Replace(" SEMICOLON", " ';'");
                    errorMessage = errorMessage.Replace(" COLON", " ':'");
                    errorMessage = errorMessage.Replace(" COMMA2", " ','");
                    errorMessage = errorMessage.Replace(" DOT", " '.'");
                    errorMessage = errorMessage.Replace(" HASH", " '#'");
                    errorMessage = errorMessage.Replace(" PERCENT", " '%'");
                    errorMessage = errorMessage.Replace(" LEFTCURLY", " '{'");
                    errorMessage = errorMessage.Replace(" RIGHTCURLY", " '}'");
                    errorMessage = errorMessage.Replace(" LEFTPAREN", " '('");
                    errorMessage = errorMessage.Replace(" RIGHTPAREN", " ')'");
                    errorMessage = errorMessage.Replace(" LEFTBRACKETGLUE", " '['");                    
                    errorMessage = errorMessage.Replace(" LEFTBRACKET", " '['");
                    errorMessage = errorMessage.Replace(" RIGHTBRACKET", " ']'");
                    errorMessage = errorMessage.Replace(" LEFTANGLESIMPLE", " '<'");
                    errorMessage = errorMessage.Replace(" RIGHTANGLE", " '>'");
                    errorMessage = errorMessage.Replace(" STAR", " '*'");
                    errorMessage = errorMessage.Replace(" VERTICALBAR", " '|'");
                    errorMessage = errorMessage.Replace(" PLUS", " '+'");
                    errorMessage = errorMessage.Replace(" MINUS", " '-'");
                    errorMessage = errorMessage.Replace(" DIV", " '/'");
                    errorMessage = errorMessage.Replace(" STARS", " '**'");
                    errorMessage = errorMessage.Replace(" EQUAL", " '='");
                    errorMessage = errorMessage.Replace(" BACKSLASH", " '\\'");
                    errorMessage = errorMessage.Replace(" DOLLAR", " '$'");
                    errorMessage = errorMessage.Replace(" QUESTION", " '?'");                                       
                    
                    errorMessage = errorMessage.Replace("EOF", "[End of input]");
                    errorMessage = errorMessage.Replace(@"'\\r\\n'", "[Newline]");  //easier to understand
                    errorMessage = errorMessage.Replace("expecting set", "");  //not meningful                
                    errorMessage = errorMessage.Replace("required (...)+ loop did not match anything at input", "unexpected input");  //different phrase in order to distinguish these two
                    errorMessage = errorMessage.Replace("no viable alternative at input", "did not expect input");  //different phrase in order to distinguish these two
                    
                    if (errorMessage.Contains("'<[End of input]>' expecting END"))
                    {
                        errorMessage += G.NL + "  Check your loop structures";
                        errorMessage += G.NL + "  (FOR) and conditionals (IF).";
                        errorMessage += G.NL + "  For each FOR or IF, an";
                        errorMessage += G.NL + "  END is expected.";                        
                    }
                    
                }


                if (lineNo > inputFileLines.Count)
                {
                    {
                        G.Writeln("*** ERROR: " + errorMessage);
                    }

                    continue;  //doesn't give meaning
                }
                string line = "";
                int firstWordPosInLine = -12345;
                bool previousLineProbablyCulprit = false;
                if (lineNo > 0)
                {
                    line = inputFileLines[lineNo - 1];
                    firstWordPosInLine = line.Length - line.TrimStart().Length + 1;
                }

                if (true)
                {
                    if (positionNo == firstWordPosInLine && errorMessage.Contains("no viable"))
                    {
                        //get preceding line (or really: statement) -- most probably the culprit.
                        previousLineProbablyCulprit = true;
                    }

                    if (ph.isOneLinerFromGui == true && lineNo != 1)
                    {
                        G.Writeln("*** ERROR: Parsing this line:");
                        G.Writeln("    " + G.ReplaceGlueNew(inputFileLines[0]), Color.Blue);
                        G.Writeln("*** ERROR: " + errorMessage);
                    }
                    else
                    {
                        if (ph.isOneLinerFromGui == false)
                        {
                            string fn = ph.fileName;
                            string extra = "";
                            if (lineNo >= 1 && positionNo > 0)
                            {
                                extra = " line " + lineNo + " pos " + positionNo;
                            }

                            if (fn == null || fn == "")
                            {
                                G.Writeln("*** ERROR: User input block," + extra);
                            }
                            else
                            {
                                G.Writeln("*** ERROR: Parsing file: " + fn + extra);
                            }
                            G.Writeln("           " + errorMessage);
                        }
                        else
                        {
                            if (positionNo > 0)
                            {
                                G.Writeln("*** ERROR: Parsing pos " + positionNo + ":  " + errorMessage);
                            }
                            else G.Writeln("*** ERROR: " + errorMessage);
                        }
                        line = line + "  ";  //hack to avoid ending problems.....

                        if (positionNo - 1 >= 0)
                        {
                            string lineTemp = line;
                            if (lineTemp != null && lineTemp != "")
                            {
                                lineTemp2.Add(lineTemp);  //used for suggestions later on
                                lineTemp2Numbers.Add("    " + "[" + G.IntFormat(lineNo, 4) + "]:");
                            }
                            string line0 = lineTemp.Substring(0, positionNo - 1);
                            string line1 = lineTemp.Substring(positionNo - 1, 1);
                            string line2 = lineTemp.Substring(positionNo - 1 + 1);

                            if (previousLineProbablyCulprit && lineNo > 1)
                            {
                                G.Writeln("    " + "Line " + (lineNo - 1) + " may be the real cause of the problem");
                                string lineBefore = inputFileLines[lineNo - 1 - 1];
                                G.Writeln("    " + "[" + G.IntFormat(lineNo - 1, 4) + "]:" + "   " + G.ReplaceGlueNew(lineBefore), Color.Blue);
                            }

                            G.Write("    " + "[" + G.IntFormat(lineNo, 4) + "]:" + "   " + G.ReplaceGlueNew(line0), Color.Blue);
                            G.Write(G.ReplaceGlueNew(line1), Color.Red);
                            G.Writeln(G.ReplaceGlueNew(line2), Color.Blue);

                            G.Writeln(G.Blanks(positionNo - 1 + 4 + 5 + 5) + "^", Color.Blue);
                            G.Writeln(G.Blanks(positionNo - 1 + 4 + 5 + 5) + "^", Color.Blue);

                            CheckForBadDouble(lineTemp);
                        }

                    }
                }

                if (ph.isModel == false)
                {
                    WriteLinkToHelpFile2(G.ReplaceGlueNew(line));
                    if (number == 1) ExtraErrorMessages(G.ReplaceGlueNew(line));
                }
            }
            if (errors.Count > 1) G.Writeln("--------------------- end of " + errors.Count + " errors --------------");

            if (Globals.runningOnTTComputer)
            {                
                List<string> xxxx = new List<string>();                
                if (ph.isOneLinerFromGui == false)
                {
                    //xxxx.Add(lineTemp2);
                    for (int i = 0; i < lineTemp2.Count; i++)
                    {
                        string s = G.ReplaceGlueNew(lineTemp2[i]);
                        List<string> yy = new List<string>();
                        yy.Add(s);
                        TranslateLine(yy, lineTemp2Numbers[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < inputFileLines.Count; i++)
                    {
                        xxxx.Add(G.ReplaceGlueNew(inputFileLines[i]));
                    }
                    TranslateLine(xxxx, null);
                }                
            }                       
        }

        private static void CheckForBadDouble(string lineTemp)
        {
            string xx = G.ReplaceGlueNew(lineTemp.Trim());
            StringTokenizer2 tok = new StringTokenizer2(xx, false, true);
            tok.IgnoreWhiteSpace = false;
            tok.SymbolChars = new char[] { '%', '&', '/', '(', ')', '=', '?', '@', '$', '{', '[', ']', '}', '+', '|', '^', '*', '<', '>', ';', ',', ':', '-' };
            Token token;
            List<string> al = new List<string>();
            List<string> alType = new List<string>();
            do
            {
                token = tok.Next();
                al.Add(token.Value); alType.Add(token.Kind.ToString());
            } while (token.Kind != TokenKind.EOF);            
            for (int i = 0; i < al.Count; i++)
            {
                if (alType[i] == "Number" && al[i].EndsWith("."))
                {
                    G.Writeln2("*** ERROR: You cannot write a number like '" + al[i] + "' ending with period, please use");
                    G.Writeln("            '" + al[i] + "0'. Such numbers interfere really poorly with the range ", Color.Red);
                    G.Writeln("           indicator '..' used in Gekko, but the numbers are legal in the model", Color.Red);
                    G.Writeln("           (.frm) file, at least for the time being.", Color.Red);
                    G.Writeln();
                    break;  //no more of these messages
                }
            }
        }

        private static void TranslateLine(List<string> xxxx, string lineNumber)
        {
            string translated = null;
            bool isGekko18 = false;

            if (G.equal(Program.options.interface_mode, "sim"))
            {
                translated = Translators.Translate1(false, xxxx);
                isGekko18 = true;
            }
            else
            {
                translated = Translator2.Translate2(false, xxxx);
            }
            
            List<string> translatedLines = G.RemoveEmptyLines(G.ExtractLinesFromText(translated));

            string before = G.ExtractTextFromLines(xxxx).ToString();
            string after = G.ExtractTextFromLines(translatedLines).ToString();

            after = after.Replace(Globals.restartSnippet, "");
            before = before.Replace(" ", "");
            after = after.Replace(" ", "");
            before = before.Replace("\r\n", "");
            after = after.Replace("\r\n", "");            
            if (before.EndsWith(";")) before = before.Substring(0, before.Length - 1);
            if (after.EndsWith(";")) after = after.Substring(0, after.Length - 1);
            before = before.ToLower().Trim();
            after = after.ToLower().Trim();

            if (before == "" || after == "")
            {
                //do nothing
            }
            else
            {
                if (before == after)
                {
                    //do nothing
                }
                else
                {
                    if (isGekko18) G.Writeln2("    Suggestion (translating from Gekko 1.8 syntax):");
                    else G.Writeln2("    Suggestion (translating from AREMOS syntax):");
                    foreach (string s12 in translatedLines)
                    {
                        if (s12.Trim() == Globals.restartSnippet) continue; //skip that here, only good for files
                        G.Writeln("    --> " + lineNumber + " " + s12, Color.Blue);
                    }
                    G.Writeln();
                }
            }
        }        

        public static void PrintAST(CommonTree node, int depth)
        {
            string txt = node.Text;
            if (node.Text != null)
            {                
                if (node.Text == "\r\n") txt = "<nl>";                
                if (node.Text == "\n") txt = "<nl>";
                txt = txt.Replace("\r\n", "<NL>");
                txt = txt.Replace("\n", "<NL>");
            }
            if (depth > 0) G.Writeln(G.Blanks(depth * 2) + txt + " [" + node.Line + "]");
            //G.Writeln(G.Blanks(depth * 2) + node.Text + "    line:" + node.Line);
            if (node.Children != null)
            {
                for (int i = 0; i < node.Children.Count; ++i)
                {
                    CommonTree child = (CommonTree)(node.Children[i]);
                    PrintAST(child, depth + 1);
                }
            }
        }        

        private static void WriteLinkToHelpFile2(string line)
        {
            char[] splits = new char[] { ' ', '<' };  //"<" catches option field
            string line10 = line.Trim();
            string[] line11 = line10.Split(splits);
            string firstWord = line11[0].ToLower();
            string firstWordUpper = firstWord.ToUpper();
            string indent = "    ";
            WriteLinkToHelpFile(firstWord, indent);
        }

        private static void WriteLinkToHelpFile(string firstWord, string indent)
        {
            if (firstWord == null || firstWord == "") return;

            if (G.equal(firstWord, "p")) firstWord = "prt";  //synonym
            if (G.equal(firstWord, "pri")) firstWord = "prt";  //synonym
            if (G.equal(firstWord, "print")) firstWord = "prt";  //synonym            
            if (G.equal(firstWord, "ser")) firstWord = "series";  //synonym   
            //TODO: should check keyword list

            bool flag = false;
            foreach (string s in Globals.helpTopics)
            {
                if (G.equal(s, firstWord))
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                G.Write(indent + "See help file: "); G.WriteLink(firstWord.ToUpper(), "help:" + firstWord); G.Writeln();
            }
        }

        private static void ExtraErrorMessages(string line)
        {
            string lowLine = line.ToLower().Trim();

            //#7436534

            //TODO: could benefit from tokenization

            if (false)
            {
                //This is around 100 characters -- do not exceed
                G.Writeln("--------------------------------------------------------------------------------------------------------");
            }

            if (lowLine.Contains("collapse") && lowLine.Contains("average"))
            {
                G.Writeln2("+++ NOTE: From Gekko 1.5.10 and up, 'average' has been abbreviated into 'avg', for instance", Color.Red);
                G.Writeln("          COLLAPSE x2 = x1 avg;", Color.Red);
                G.Writeln("          Please adjust the syntax, and sorry for the inconvenience.", Color.Red);
            }            

            if (lowLine.StartsWith("exogenize"))
            {
                G.Writeln2("+++ NOTE: You should use EXO instead of EXOGENIZE.", Color.Red);
                G.Writeln("          Please adjust the syntax, and sorry for the inconvenience.", Color.Red);
            }

            if (lowLine.StartsWith("endogenize"))
            {
                G.Writeln2("+++ NOTE: You should use ENDO instead of ENDOGENIZE.", Color.Red);
                G.Writeln("          Please adjust the syntax, and sorry for the inconvenience.", Color.Red);
            }

            if (lowLine.StartsWith("undo") && lowLine.Contains("sim"))
            {
                G.Writeln2("+++ NOTE: UNDO SIM has been removed. When a simulation fails, you may click a link", Color.Red);
                G.Writeln("          to have the simulation undone.", Color.Red);
                G.Writeln("          Please adjust the syntax, and sorry for the inconvenience.", Color.Red);
            }

            if (lowLine.StartsWith("pctprt"))
            {
                G.Writeln2("+++ NOTE: From Gekko 1.5.9 and onwards, PCTPRT is obsolete.", Color.Red);
                G.Writeln("          Please use PRT<pch> instead.", Color.Red);
            }

            if (lowLine.StartsWith("mulpct"))
            {
                G.Writeln2("+++ NOTE: From Gekko 1.5.9 and onwards, MULPCT is obsolete.", Color.Red);
                G.Writeln("          Please use MULPRT<pch> instead.", Color.Red);
            }
            
            if (lowLine.StartsWith("nytvindu"))
            {
                G.Writeln2("+++ NOTE: From Gekko 1.5.11 and up, NYTVINDU has been obsoleted. Please use CLS.", Color.Red);
                G.Writeln("          Please adjust the syntax, and sorry for the inconvenience.", Color.Red);
            }

            if (lowLine.StartsWith("option") && lowLine.Contains("switchback"))
            {
                G.Writeln2("+++ NOTE: Since Gekko 1.5.11, 'OPTION solve gauss goal switchback' has been removed. Gekko no longer", Color.Red);
                G.Writeln("          switches solve method permanently (to Newton) when ENDO/EXO goals are set, so the", Color.Red);
                G.Writeln("          switchback option is no longer needed.", Color.Red);
            }

            if (lowLine.StartsWith("cleargoals"))
            {
                G.Writeln2("+++ NOTE: Since Gekko 1.5.11, CLEARGOALS has been replaced by UNFIX. This is just a change of", Color.Red);
                G.Writeln("          terminology: easier to write and perhaps remember.", Color.Red);
            }

            if (lowLine.StartsWith("gauss"))
            {
                G.Writeln2("+++ NOTE: Since Gekko 1.5.11, the GAUSS command is not used anymore. Gekko does not change solve", Color.Red);
                G.Writeln("          method permanently anymore when solving goals, so the GAUSS command is not necessary.", Color.Red);
                G.Writeln("          Use UNFIX instead in order to clear goals and revert to Gauss solving.", Color.Red);
            }

            if (lowLine.StartsWith("if") && (lowLine.Contains("=") && !lowLine.Contains("==")))
            {
                G.Writeln2("+++ NOTE: Since Gekko 1.5.11, the logical comparison operator can only be '==' and not '=',", Color.Red);
                G.Writeln("          for instance: IF '#x1' == '#x2'... Could this be the problem here?", Color.Red);
            }

            if ((lowLine.StartsWith("for") && lowLine.Contains(" to ")) && (!lowLine.Contains("val") && !lowLine.Contains("date")))
            {
                G.Writeln2("+++ NOTE: FOR statements should contain the type of the loop variable.", Color.Red);
                G.Writeln("          For instance 'FOR name n = ...', 'FOR val v = ... TO ...' or 'FOR date d = ... TO ...'.", Color.Red);
                G.Writeln("          This is to improve readability of command files and avoid ambiguities.", Color.Red);
                G.Writeln("          The type can optionally be omitted for 'name' loops, however.", Color.Red);
            }

            if (lowLine.StartsWith("new"))
            {
                G.Writeln2("+++ NOTE: Please use RESET instead of NEW.", Color.Red);
            }

            if (lowLine.StartsWith("option") && lowLine.Contains("table") && lowLine.Contains("startfile"))
            {
                G.Writeln2("+++ NOTE: Since Gekko 1.6.4, 'OPTION table startfile = ...' has been renamed to", Color.Red);
                G.Writeln("          'OPTION menu startfile = ...'.", Color.Red);
            }
        }

    }  //end of class
}


