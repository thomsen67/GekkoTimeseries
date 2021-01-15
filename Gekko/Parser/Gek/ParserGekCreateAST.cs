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
using System.Reflection;
using System.Reflection.Emit;

namespace Gekko.Parser.Gek
{
    /// <summary>
    /// This class is used to create an AST from a .get (script) file. There is a similar class for model files.
    /// </summary>
    public class ParserGekCreateAST
    {

        public static ConvertHelper CreateAST(ParseHelper ph, P p)
        {
            //Q q = new Q();  //make a fresh container for method argument helpers
            //p.SetQ(q);

            ConvertHelper ch2 = new ConvertHelper();

            string textInput = ph.commandsText + "\r\n" + "\r\n"; //newlines for ease of use of ANTLR
            
            ANTLRStringStream input = new ANTLRStringStream(textInput);

            List<string> errors = null;
            CommonTree t = null;

            if (Globals.printAST) G.Writeln2(ph.commandsText, Color.Green);
            // Create a lexer attached to that input            
            Cmd3Parser parser3 = null;
            Cmd3Lexer lexer3 = new Cmd3Lexer(input);
            //usually debugTokens=false, and this is stepped into manually (otherwise the tokens are consumed and preliminary steps cannot be run)
            if (Globals.runningOnTTComputer && Globals.debugTokens)
            {
                Gekko.Parser.ParserCommon.DebugTokens(lexer3);
            }

            // Create a stream of tokens pulled from the lexer
            CommonTokenStream tokens3 = new CommonTokenStream(lexer3);
            // Create a parser attached to the token stream
            parser3 = new Cmd3Parser(tokens3);
            // Invoke the program rule in get return value
            Cmd3Parser.start_return r3 = null;

            try
            {
                r3 = parser3.start();
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

            //if (parser3.NumberOfSyntaxErrors > 0) G.Writeln2("ERROR!!");                                              

            if (parser3.GetErrors().Count > 0)
            {
                PrintParserErrors(parser3.GetErrors(), Program.CreateListOfStringsFromString(textInput), ph);
                throw new GekkoException();
            }
            t = (CommonTree)r3.Tree;

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
            Gekko.Parser.Gek.ParserGekWalkASTAndEmit.WalkASTAndEmitUnfold(root);
            Gekko.Parser.Gek.ParserGekWalkASTAndEmit.WalkASTAndEmit(root, 0, 0, textInput, wh2, p);
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("WALK END");

            //PrintAST2(root, 0);

            s.AppendLine(root.Code.ToString());

            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("EMIT cs end: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);

            //if (Globals.testing) G.Writeln(s.ToString(), Color.Gray);

            string csCode = s.ToString();
            string csMethods = null;

            DateTime dt = DateTime.Now;
            //#8750932875984325
            if (Program.options.system_code_split > 0) CodeSplit(ref csCode, ref csMethods, "C");

            string csCode2 = wh2.headerCs.ToString(); string csMethods2 = "";
            if (Program.options.system_code_split > 0) CodeSplit(ref csCode2, ref csMethods2, "CC");

            if (Program.options.system_code_split > 0 && Globals.showTimings && !G.IsUnitTesting())
            {
                G.Writeln("Splitting took " + (DateTime.Now - dt).TotalMilliseconds / 1000d + "s", Color.Gray);
            }

            StringBuilder s2 = new StringBuilder();
            s2.AppendLine("using System;");
            s2.AppendLine("using System.Collections.Generic;");
            s2.AppendLine("using System.Text;");
            s2.AppendLine("using System.Windows.Forms;");
            s2.AppendLine("using System.Drawing;");  //to use Color.Red in G.Writeln()
            //s2.AppendLine("using System.Core;");  //seems necessary in order to use functions/procedures with > 6 parameters... ?
            s2.AppendLine("using Gekko.Parser;"); //all the AST_Xxx() methods are found in Gekko.Parser.AST.cs
            s2.AppendLine("namespace Gekko");
            s2.AppendLine("{");
            
            s2.AppendLine("public class TranslatedCode");
            s2.AppendLine("{");

            s2.AppendLine("public static GekkoTime globalGekkoTimeIterator = GekkoTime.tNull;");
            s2.AppendLine("public static int " + Globals.labelCounter + ";");

            s2.AppendLine(csMethods);  //definitions of C1(), C2(), etc. -- may be empty
            s2.AppendLine(csMethods2);  //definitions of CC1(), CC2(), etc. -- may be empty

            s2.AppendLine(csCode2);
            
            s2.Append(wh2.headerExpressions);  //definitions of GekkoExpression1(), ...

            s2.AppendLine("public static void CodeLines(P p)");
            s2.AppendLine("{");
            s2.AppendLine(Globals.gekkoSmplInit);
            //The generated code, but much of it may be in codeCi
            s2.AppendLine(csCode);
            
            s2.AppendLine("}");  //method CodeLines()
            s2.AppendLine("}");  //class TranslatedCode
            s2.AppendLine("}");  //namespace Gekko            

            ch2.code = s2.ToString().Replace("`", Globals.QT).Replace(Globals.smpl, "smpl");

            ch2.errors = errors;  //not used?

            //if (Globals.uFunctionStorageCs.Count > 0)
            //{
            //    StringBuilder s3 = new StringBuilder();
            //    foreach (string sCode in Globals.uFunctionStorageCs.Values)
            //    {
            //        s3.Append(sCode);
            //    }
            //    ch2.codeUFunctions = s3.ToString().Replace("`", Globals.QT);
            //}

            if (Globals.printAST)
            {
                //G.Writeln(ch2.codeUFunctions);
                //G.Writeln("===============================");
                //G.Writeln("===============================");
                //G.Writeln("===============================");
                G.Writeln(ch2.code);
            }

            return ch2;
        }        

        private static int FindEndMarker(List<string> lines, int i, GekkoDictionary<string, string> controlVars, List<string> markers)
        {
            //findes suitable end marker if it exists for the command
            string id = lines[i].Substring(Globals.splitStart.Length);                        
            for (int j = i + 1; j < lines.Count; j++)
            {                                
                FindEndMarkerHelper(lines, controlVars, j, markers);
                
                if (lines[j].StartsWith(Globals.splitEnd))
                {
                    string id2 = lines[j].Substring(Globals.splitEnd.Length);
                    if (id == id2) return j;
                    else return -12345;
                }
                if (lines[j].StartsWith(Globals.splitBit))
                {
                    return -12345;
                }
            }
            return -12345;
        }

        private static void FindEndMarkerHelper(List<string> lines, GekkoDictionary<string, string> controlVars, int j, List<string> markers)
        {
            //Looks for control variables that must be used as args in C() and CC() methods
                        
            foreach (string s in markers)
            {
                int ii = 0;
                while (ii != -1)
                {
                    ii = lines[j].IndexOf(s, ii, StringComparison.Ordinal);
                    if (ii != -1)
                    {
                        //this is a bit slow, but control vars are rare
                        for (int ii2 = ii + s.Length; ii2 < lines[j].Length + 1; ii2++)
                        {
                            if (ii2 == lines[j].Length)
                            {
                                //if ii2 == lines[j].Length, the number ends with a newline
                                //see below
                                string sub = lines[j].Substring(ii, ii2 - ii);
                                if (!controlVars.ContainsKey(sub)) controlVars.Add(sub, "");                                
                                break;
                            }
                            else if (!char.IsNumber(lines[j][ii2]))
                            {
                                //see above
                                string sub = lines[j].Substring(ii, ii2 - ii);
                                if (!controlVars.ContainsKey(sub)) controlVars.Add(sub, "");
                                break;
                            }
                        }
                        ii++;  //to get it moving, in principle it could move further but oh well: IndexOf is fast anyway
                    }
                }
            }
        }

        private static void CodeSplit(ref string csCode, ref string csMethods, string type)
        {
            //Experiment (seconds/lines), using this kind of file:
            //
            //------------------------------------------
            //time 2000 2020;
            //x = 0;
            //tell'Start';
            //x = x + x - x + x - x + x - x + 1;
            // ...
            // ...
            //x = x + x - x + x - x + x - x + 1;
            //p < 2010 2011 > x;
            //------------------------------------------
            //
            //There is about 1s deadweight.
            //
            //Using split = 10, we get --> 3.67 (1000), 5.59 (2000), 10.64 (4000), 20.76 (8000)
            //Using split =  0, we get --> 4.69 (1000), 11.22 (2000), 36.30 (4000), 171.12 (8000)
            //
            //Both are out of RAM on 16000 lines
            //
            List<string> lines = G.ExtractLinesFromText(csCode);
            int nGoal = lines.Count;  //check that no lines are forgotten
            int nExtra = 0;
            List<string> tempHelperForMethodsCs = new List<string>();  //temp storage  
            List<string> mainCs = new List<string>();  //this is the rest                      
            List<string> methodsCs = new List<string>();  //this is the rest

            List<string> markers = new List<string>(); markers.Add(Globals.forLoopName); markers.Add(Globals.functionArgName);

            //a valid block is like this:
            //    [[commandStart]]117, where 117 is a number
            //        ......
            //    [[commandEnd]]117, where 117 is a number
            //    [[commandStart]]118, where 118 is a number
            //        ......
            //    [[commandEnd]]118, where 118 is a number
            //Between Start and End there can be no lines starting with "[[command"            
            //This may go on, until the above rule breaks, or for instance 126, that is, 10 commands. Then there is a FLUSH
            //

            int ciCounter = 0;
            
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                if (lines[i].StartsWith(Globals.splitStart))
                {
                    GekkoDictionary<string, string> controlVars = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    int j = FindEndMarker(lines, i, controlVars, markers);
                    if (j != -12345)
                    {
                        //now we know that there is an ok command starting at i and ending at j
                        //see if we can find more of these
                                                
                        for (int k = 0; k < Program.options.system_code_split; k++)
                        {
                            int j6 = j;
                            for (int i6 = j + 1; i6 < lines.Count; i6++)
                            {
                                string temp = lines[i6].Trim();
                                if (temp == "" || temp == ";" || temp.StartsWith("//"))
                                {
                                    //fine
                                    j6 = i6;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            
                            if (j6 < lines.Count &&  lines[j6].StartsWith(Globals.splitStart))
                            {
                                int j2 = FindEndMarker(lines, j6, controlVars, markers);
                                if (j2 == -12345) break;
                                else j = j2;
                            }
                            else break;
                        }

                        //now j is pointing to the end of a valid block of max commands, without control variables
                        if (true)
                        {
                            string s1 = null;
                            string s2 = null;
                            foreach (string s in controlVars.Keys)
                            {                                
                                s1 = s1 + ", ref " + s; //must allow a new object to be returned via an argument
                                s2 = s2 + ", ref " + "IVariable " + "x" + s; //must allow a new object to be returned via an argument
                            }
                            
                            methodsCs.Add("public static void " + type + "" + ciCounter + "(GekkoSmpl smpl, P p" + s2 + ") {");

                            foreach (string s in controlVars.Keys)
                            {
                                //Otherwise you get this error: CS1628: Cannot use in ref or out parameter 'parameter' inside an anonymous method, lambda expression, or query expression
                                methodsCs.Add("IVariable " + s + " = " + "x" + s + ";" + G.NL);
                                nExtra++;                          
                            }

                            nExtra++;
                            for (int ii = i; ii <= j; ii++)
                            {
                                methodsCs.Add(lines[ii]);                                
                            }

                            foreach (string s in controlVars.Keys)
                            {
                                //Otherwise you get this error: CS1628: Cannot use in ref or out parameter 'parameter' inside an anonymous method, lambda expression, or query expression
                                methodsCs.Add("x" + s + " = " + s + ";" + G.NL);
                                nExtra++;
                            }

                            methodsCs.Add("}");
                            nExtra++;
                            mainCs.Add("" + type + "" + ciCounter + "(smpl, p" + s1 + ");");
                            nExtra++;
                            i = j; ciCounter++;
                            continue;
                        }
                    }                    
                }                
                mainCs.Add(lines[i]);                
            }

            if (nGoal != mainCs.Count + methodsCs.Count - nExtra)
            {
                //Sanity check that nothing is forgotten
                G.Writeln2("*** ERROR: Technical issue with code-splitting, please use OPTION system code split = 0");
                throw new GekkoException();
            }

            csCode = G.ExtractTextFromLines(mainCs).ToString();
            csMethods = G.ExtractTextFromLines(methodsCs).ToString();

            // -------------------------------------
            // -------------------------------------
            // -------------------------------------

            
            
        }

        private static void CodeSplitFlush(List<string> tempHelperForMethodsCs, List<string> mainCs, List<string> methodsCs, ref int nCi, ref int counterExtra)
        {
            methodsCs.Add("public static void C" + nCi + "(P p) {" + G.NL); counterExtra++;
            //methodsCs.Add(Globals.gekkoSmplInit + G.NL); counterExtra++;
            methodsCs.Add(Globals.gekkoSmplInitCommand + G.NL); counterExtra++;
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

                if (ast.Text.Contains(Globals.parserExpressionSeparator.ToString()))
                {
                    if (ast.Text.StartsWith("ASTMETA" + Globals.parserExpressionSeparator))  //Handles SERIES, that is ASTGENR/ASTUPD
                    {
                        flag = true;
                    }                    
                    else if (ast.Text.StartsWith("ASTSHOW" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTPRTELEMENT" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTOLSEXPRESSION" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTANALYZEEXPRESSION" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTASSIGNMENT" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTDECOMP" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTDECOMP2" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTDECOMP3" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTEVAL" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTLIST" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTCURLY" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTINDEXERELEMENT" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTINDEXERELEMENTIDENT" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                    else if (ast.Text.StartsWith("ASTCOMPARE2" + Globals.parserExpressionSeparator))
                    {
                        flag = true;
                    }
                }                
                
                if (flag)
                {
                    string[] ss = ast.Text.Split(Globals.parserExpressionSeparator);
                    //for (int i = 0; i < ss.Length; i++)
                    //{                        
                    //    ss[i] = G.ReplaceGlueNew(ss[i]);                        
                    //}
                    cmdNode.Text = G.ReplaceGlueNew(ss[0]);
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
            Program.StopPipeAndMute(2);
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
                int lineNumber = -12345;
                try
                {
                    lineNumber = int.Parse(ss[0]) - 1;  //seems 1-based before subtract 1
                }
                catch (Exception e)
                {
                    G.Writeln2("*** ERROR: The parser stumbled unexpectedly with the message: " + s);
                    throw new GekkoException();
                }
                int lineNo = lineNumber + 1;  //1-based
                int positionNo = -12345;
                try
                {
                    positionNo = int.Parse(ss[1]) + 1;  //1-based
                }
                catch (Exception e)
                {
                    G.Writeln2("*** ERROR: The parser stumbled unexpectedly with the message: " + s);
                    throw new GekkoException();
                }

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

                    lineTemp = G.ReplaceGlueNew(lineTemp);

                    string line0 = lineTemp.Substring(0, positionNo - 1);
                    string line1 = lineTemp.Substring(positionNo - 1, 1);
                    string line2 = lineTemp.Substring(positionNo - 1 + 1);

                    if (previousLineProbablyCulprit && lineNo > 1)
                    {
                        G.Writeln("    " + "Line " + (lineNo - 1) + " may be the real cause of the problem");
                        string lineBefore = inputFileLines[lineNo - 1 - 1];
                        G.Writeln("    " + "[" + G.IntFormat(lineNo - 1, 4) + "]:" + "   " + G.ReplaceGlueNew(lineBefore), Color.Blue);
                    }

                    G.Write("    " + "[" + G.IntFormat(lineNo, 4) + "]:" + "   " + line0, Color.Blue);
                    G.Write(line1, Color.Red);
                    G.Writeln(line2, Color.Blue);

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
            Program.StopPipeAndMute(2);
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

                            lineTemp = G.ReplaceGlueNew(lineTemp);

                            //try: not the end of the world if one of these fails
                            string line0 = "";
                            string line1 = "";
                            string line2 = "";
                            try  { line0 = lineTemp.Substring(0, positionNo - 1); } catch { };
                            try  { line1 = lineTemp.Substring(positionNo - 1, 1); } catch { };
                            try  { line2 = lineTemp.Substring(positionNo - 1 + 1); } catch { };

                            if (previousLineProbablyCulprit && lineNo > 1)
                            {
                                G.Writeln("    " + "Line " + (lineNo - 1) + " may be the real cause of the problem");
                                string lineBefore = inputFileLines[lineNo - 1 - 1];
                                G.Writeln("    " + "[" + G.IntFormat(lineNo - 1, 4) + "]:" + "   " + G.ReplaceGlueNew(lineBefore), Color.Blue);
                            }

                            G.Write("    " + "[" + G.IntFormat(lineNo, 4) + "]:" + "   " + line0, Color.Blue);
                            G.Write(line1, Color.Red);
                            G.Writeln(line2, Color.Blue);

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
            } while (token.Kind != ETokenType.EOF);            
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

        public static void PrintAST2(ASTNode node, int depth)
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
            if (node.ChildrenCount() > 0)
            {
                for (int i = 0; i < node.ChildrenCount(); ++i)
                {
                    ASTNode child = node[i];
                    PrintAST2(child, depth + 1);
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

            if (G.Equal(firstWord, "p")) firstWord = "prt";  //synonym
            if (G.Equal(firstWord, "pri")) firstWord = "prt";  //synonym
            if (G.Equal(firstWord, "print")) firstWord = "prt";  //synonym            
            if (G.Equal(firstWord, "ser")) firstWord = "series";  //synonym   
            //TODO: should check keyword list

            bool flag = false;
            foreach (string s in Globals.helpTopics)
            {
                if (G.Equal(s, firstWord))
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

            //if ((lowLine.StartsWith("for") && lowLine.Contains(" to ")) && (!lowLine.Contains("val") && !lowLine.Contains("date")))
            //{
            //    G.Writeln2("+++ NOTE: FOR statements should contain the type of the loop variable.", Color.Red);
            //    G.Writeln("          For instance 'FOR name n = ...', 'FOR val v = ... TO ...' or 'FOR date d = ... TO ...'.", Color.Red);
            //    G.Writeln("          This is to improve readability of command files and avoid ambiguities.", Color.Red);
            //    G.Writeln("          The type can optionally be omitted for 'name' loops, however.", Color.Red);
            //}

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


