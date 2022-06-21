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
    
    public class ErrorMessagesHelper
    {
        public ConvertHelper parseOutput;
        public string textWithExtraLines;
        public CommonTree t;
    }
    
    /// <summary>
    /// This class is used to parse commands and create an ASTtree, and walk it.There is a similar class for model files.
    /// </summary>
    public class ParserGekCreateAST
    {
        /// <summary>
        /// In Cmd3.g/Cmd4.g rules, Normal = start, OnlyAssignment = startB, OnlyProcedureCallEtc = startC
        /// </summary>
        public enum EParserType
        {            
            Normal,                                   //start            
            OnlyAssignment,                           //startB
            OnlyProcedureCallEtc ,                     //startC
        }

        public static ConvertHelper ParseAndCallWalkAndEmit(ParseHelper ph, P p)
        {            
            ph.isDebugMode = false;
            ph.syntaxType = EParserType.Normal;
            ph.nicerErrors = false;
            if (G.Equal(Program.options.interface_errors, "normal")) ph.nicerErrors = true;

            ConvertHelper parseOutput; string textWithExtraLines; CommonTree t;
            LexerAndParserErrors lexerAndParserErrors = ParseAndSyntaxErrors(out parseOutput, out textWithExtraLines, out t, ph);

            if (lexerAndParserErrors.lexerErrors != null)
            {
                string input2 = ph.commandsText + "\r\n";
                HandleCommandLexerErrors(lexerAndParserErrors.lexerErrors, Stringlist.CreateListOfStringsFromFile(input2), ph);
                throw new GekkoException(); //this will make a double error -- but the other one will be identified later on (both text and filename are null) and skipped -- a little bit hacky, but oh well...
            }
            else if (lexerAndParserErrors.parserErrors != null)
            {                
                if (ph.nicerErrors)
                {
                    ErrorMessagesHelper helper = new ErrorMessagesHelper();
                    helper.parseOutput = parseOutput;
                    helper.textWithExtraLines = textWithExtraLines;
                    helper.t = t;
                    int nMax = 10;  //can be int.MaxValue
                    bool fail = ParserGekErrors.ErrorMessages(helper, ph, nMax);
                    if (fail)
                    {
                        //use old
                        HandleCommandParserErrors(lexerAndParserErrors.parserErrors, Stringlist.CreateListOfStringsFromFile(ph.commandsText), ph);
                    }
                }
                else
                {
                    HandleCommandParserErrors(lexerAndParserErrors.parserErrors, Stringlist.CreateListOfStringsFromFile(ph.commandsText), ph);
                }
                throw new GekkoException();
            }

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
            wh2.libraryName = ph.libraryName;

            ASTNode root = new ASTNode(null);
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("Create AST start: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);
            CreateASTNodesForCmd(t, root, 0);
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("Create AST end: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);

            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("EMIT cs start: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);

            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("WALK START");
            Gekko.Parser.Gek.ParserGekWalkASTAndEmit.WalkASTAndEmitUnfold(root, 0);
            Gekko.Parser.Gek.ParserGekWalkASTAndEmit.WalkASTAndEmit(root, 0, 0, textWithExtraLines, wh2, p);
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

            s2.AppendLine("public static void CodeLines(P p)");
            s2.AppendLine("{");
            s2.AppendLine(Globals.gekkoSmplInit);
            //The generated code, but much of it may be in codeCi
            s2.AppendLine(csCode);

            s2.AppendLine("}");  //method CodeLines()
            s2.AppendLine("}");  //class TranslatedCode
            s2.AppendLine("}");  //namespace Gekko            

            parseOutput.code = s2.ToString().Replace("`", Globals.QT).Replace(Globals.smpl, "smpl");

            //parseOutput.errors = errors;  //not used?

            if (Globals.printAST)
            {
                G.Writeln(parseOutput.code);
            }

            return parseOutput;
        }        

        public static LexerAndParserErrors ParseAndSyntaxErrors(out ConvertHelper ch2, out string textInput, out CommonTree t, ParseHelper ph)
        {
            LexerAndParserErrors lexerAndParserErrors = new LexerAndParserErrors();
            ch2 = new ConvertHelper();
            textInput = ph.commandsText + "\r\n" + "\r\n";
            ANTLRStringStream input = new ANTLRStringStream(textInput);
            
            t = null;
            if (Globals.printAST) G.Writeln2(ph.commandsText, Color.Green);
            Cmd3Parser parser3 = null;
            Cmd3Lexer lexer3 = null;
            Cmd4Parser parser4 = null;
            Cmd4Lexer lexer4 = null;

            if (!ph.isDebugMode) lexer3 = new Cmd3Lexer(input);
            else lexer4 = new Cmd4Lexer(input);
            
            if (Globals.runningOnTTComputer && Globals.debugTokens)
            {
                //usually debugTokens=false, and this is stepped into manually (otherwise the tokens are consumed and preliminary steps cannot be run)
                ParserCommon.DebugTokens(lexer3, lexer4);
            }

            CommonTokenStream tokens3 = null;
            CommonTokenStream tokens4 = null;
            Cmd3Parser.start_return r3 = null;
            Cmd3Parser.startB_return r3B = null;
            Cmd3Parser.startC_return r3C = null;
            Cmd4Parser.start_return r4 = null;
            Cmd4Parser.startB_return r4B = null;
            Cmd4Parser.startC_return r4C = null;

            if (!ph.isDebugMode)
            {
                tokens3 = new CommonTokenStream(lexer3);
                parser3 = new Cmd3Parser(tokens3);
            }
            else
            {
                tokens4 = new CommonTokenStream(lexer4);
                parser4 = new Cmd4Parser(tokens4);
            }

            try
            {
                if (ph.syntaxType == EParserType.Normal)
                {
                    if (!ph.isDebugMode) r3 = parser3.start();
                    else r4 = parser4.start();
                }
                else if (ph.syntaxType == EParserType.OnlyAssignment)
                {
                    if (!ph.isDebugMode) r3B = parser3.startB();
                    else r4B = parser4.startB();
                }
                else if (ph.syntaxType == EParserType.OnlyProcedureCallEtc)
                {
                    if (!ph.isDebugMode) r3C = parser3.startC();
                    else r4C = parser4.startC();
                }
                else new Error("Parser types");
            }
            catch (Exception e)
            {
                List<string> temp = new List<string>();
                temp.Add(e.Message);
                lexerAndParserErrors.lexerErrors = temp;
                return lexerAndParserErrors;
            }

            List<string> parserErrors = null;
            if (!ph.isDebugMode) parserErrors = parser3.GetErrors();
            else parserErrors = parser4.GetErrors();

            if (parserErrors.Count > 0)
            {
                lexerAndParserErrors.parserErrors = parserErrors;
                return lexerAndParserErrors;
            }

            if (ph.syntaxType == EParserType.Normal)
            {
                if (!ph.isDebugMode) t = (CommonTree)r3.Tree;
                else t = (CommonTree)r4.Tree;
            }
            else if (ph.syntaxType == EParserType.OnlyAssignment)
            {
                if (!ph.isDebugMode) t = (CommonTree)r3B.Tree;
                else t = (CommonTree)r4B.Tree;
            }
            else if (ph.syntaxType == EParserType.OnlyProcedureCallEtc)
            {
                if (!ph.isDebugMode) t = (CommonTree)r3C.Tree;
                else t = (CommonTree)r4C.Tree;
            }
            else new Error("Parser types");

            return lexerAndParserErrors;
        }

        /// <summary>
        /// Method to quick test if a string of Gekko 3.0 command(s) is legal syntax (parses ok in ANTLR). Nothing is executed.
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public static bool IsValid3_0Syntax(string commands)
        {
            bool ok = true;
            string s2a = Program.HandleGekkoCommands(commands);
            string textInput = s2a + "\r\n" + "\r\n"; //newlines for ease of use of ANTLR
            ANTLRStringStream input = new ANTLRStringStream(textInput);
            Cmd3Parser parser3 = null;
            Cmd3Lexer lexer3 = new Cmd3Lexer(input);
            CommonTokenStream tokens3 = new CommonTokenStream(lexer3);
            parser3 = new Cmd3Parser(tokens3);
            Cmd3Parser.start_return r3 = null;

            try
            {
                r3 = parser3.start();  //may crash due to lexer
            }
            catch
            {
                ok = false;
            }

            if (ok)
            {
                if (parser3.GetErrors().Count > 0)
                {
                    ok = false;
                }
            }

            return ok;
        }

        /// <summary>
        /// Method to quick test if a string of Gekko 2.4 command(s) is legal syntax (parses ok in ANTLR). Nothing is executed.
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public static bool IsValid2_4Syntax(string commands)
        {

            //CompileAndRunAST

            bool ok = true;

            string s2a = Stringlist.ExtractTextFromLines(HandleGekkoCommandsFromGekko2_4(Stringlist.ExtractLinesFromText(commands))).ToString();
            string textInput = s2a + "\r\n" + "\r\n"; //newlines for ease of use of ANTLR
            ANTLRStringStream input = new ANTLRStringStream(textInput);
            //List<string> errors = null;
            CommonTree t = null;
            // Create a lexer attached to that input
            Cmd2Parser parser2 = null;
            Cmd2Lexer lexer2 = new Cmd2Lexer(input);                     
            CommonTokenStream tokens = new CommonTokenStream(lexer2);
            // Create a parser attached to the token stream
            parser2 = new Cmd2Parser(tokens);
            Cmd2Parser.expr_return r = null;

            try
            {
                r = parser2.expr(); //may crash due to lexer
            }
            catch
            {
                ok = false;
            }

            if (ok)
            {
                if (parser2.GetErrors().Count > 0)
                {
                    ok = false;
                }
            }

            return ok;
        }

        private static List<string> HandleGekkoCommandsFromGekko2_4(List<string> inputFileLines)
        {
            //!!!
            //!!!
            //!!! has a lot of local methods stashed inside, so these do not pollute anything else!
            //!!!
            //!!!

            int GetNextComment(string lineNewVersion, int i)
            {
                //-12345 if no hit, else the last position without the comment
                //dows not require spaces
                int rv = -12345;
                for (int ii = i; ii < lineNewVersion.Length - 1; ii++)
                {
                    if (lineNewVersion[ii] == '/' && (lineNewVersion[ii + 1] == '/' || lineNewVersion[ii + 1] == '*')) return ii - 1;
                }
                return rv;
            }


            int GetNextIdent(string lineNewVersion, int i, out string ident)
            {
                //-12345 if no hit, else the first position to read for next thing to read
                ident = null;
                int rv = -12345;
                bool first = true;
                int start = -12345;
                for (int ii = i; ii < lineNewVersion.Length; ii++)
                {
                    if (start == -12345 && lineNewVersion[ii] == ' ') continue;
                    if (first) start = ii;
                    if (first && !G.IsLetterOrUnderscore(lineNewVersion[ii]))
                    {
                        return -12345;  //first letter is non-valid
                    }
                    if (!first && !G.IsLetterOrDigitOrUnderscore(lineNewVersion[ii]))
                    {
                        ident = lineNewVersion.Substring(start, ii - start);
                        return ii;  //second+ letter is non-valid
                    }
                    first = false;
                }

                return rv;
            }

            int GetNextEquals(string lineNewVersion, int i)
            {
                //-12345 if no hit, else the first position to read for next thing to read
                int rv = -12345;
                for (int ii = i; ii < lineNewVersion.Length; ii++)
                {
                    if (lineNewVersion[ii] == ' ') continue;
                    if (lineNewVersion[ii] == '=')
                    {
                        return ii + 1;
                    }
                    return -12345;
                }
                return rv;
            }

            int GetNextEquals2(string lineNewVersion, int i)
            {
                for (int ii = i; ii < lineNewVersion.Length; ii++)
                {
                    if (lineNewVersion[ii] != '=') continue;
                    return ii + 1;
                }
                return -12345;
            }

            int GetNextHash(string lineNewVersion, int i)
            {
                //-12345 if no hit, else the first position to read for next thing to read
                int rv = -12345;
                for (int ii = i; ii < lineNewVersion.Length; ii++)
                {
                    if (lineNewVersion[ii] == ' ') continue;
                    if (lineNewVersion[ii] == '#')
                    {
                        return ii + 1;
                    }
                    return -12345;
                }
                return rv;
            }


            int skipSpaces777(char[] c, int ii)
            {
                int i;
                //skip spaces (tab is included counted)
                for (i = ii; i < int.MaxValue; i++)
                {
                    if (c[i] == ' ' || c[i] == '\t')     //'\t' is tab
                    {
                        //do nothing
                    }
                    else return i;
                }
                return -12345;
            }

            int skipSpaces(string c, int ii)
            {
                int i;
                //skip spaces (tab is included counted)
                for (i = ii; i < c.Length; i++)
                {
                    if (c[i] == ' ' || c[i] == '\t')     //'\t' is tab
                    {
                        //do nothing
                    }
                    else return i;
                }
                return -12345;
            }


            bool Has2IdentsFollowing(string lineNewVersion, int start)
            {
                //The method looks for two idents like "a1 b2 " or "a1 b2>". Any spaces before, in middle or after are ok.
                //A '=' right after the second token is ok too.
                int j = skipSpaces(lineNewVersion, start);
                if (j == -12345) return false;
                if (!G.IsLetterOrUnderscore(lineNewVersion[j])) return false;
                int blank = -12345;
                for (int k = j + 1; k < lineNewVersion.Length; k++)
                {
                    if (lineNewVersion[k] == ' ')
                    {
                        blank = k;
                        break;
                    }
                    if (!G.IsLetterOrDigitOrUnderscore(lineNewVersion[k]))
                    {
                        return false;
                    }
                }
                j = skipSpaces(lineNewVersion, blank);
                if (j == -12345) return false;
                if (!G.IsLetterOrUnderscore(lineNewVersion[j])) return false;
                for (int k = j + 1; k < lineNewVersion.Length; k++)
                {
                    if (lineNewVersion[k] == ' ' || lineNewVersion[k] == '=' || lineNewVersion[k] == '>')
                    {
                        //these are ok: <m p q>, <stamp rows=yes>, <m p>
                        //so the first two tokens will not by tried to be interpreted as two dates
                        break;
                    }
                    if (!G.IsLetterOrDigitOrUnderscore(lineNewVersion[k]))
                    {
                        return false;
                    }
                }
                return true;
            }

            List<string> inputFileLines2 = new List<string>();
            int lineCounter = 0;
            foreach (string line in inputFileLines)
            {
                lineCounter++;
                string lineNewVersion = line;

                if (lineNewVersion == Globals.iniFileSecretName)  //this strange name is made in GuiAutoExecStuff()
                {
                    //lineNewVersion = "run '" + Globals.autoExecCmdFileName + "';";
                    lineNewVersion = "ini;";
                }                

                string lineComment = lineNewVersion.Trim();

                if (true)
                {

                    // Special rule to make sure PRT<m d> is not interpreted as time period, so in that case we get
                    // PRT<m d> --> PRT <?<m d>
                    //
                    // Else: (ldu is letterDigitUnderscore)
                    // For every ldu, '(', '[', '{', '%', '#'
                    //   see if preceding char is ldu, ')', ']', '}', '%' or '#'.
                    //   if so, put a glue in between.
                    //   EXCEPTION: ldu before ldu gets no glue (of course)!
                    // For '|' there is glue before, UNLESS there is a blank before OR after the '|'
                    // For '.' ...
                    //
                    // For "<m d>" kind of options, we use a special kind of marker ('<<<' instead of '<') to indicate that it is
                    // an "<ident ident..." type.
                    //List<char> glued1 = new List<char> { ')', ']', '}', '%', '#' };
                    //List<char> glued2 = new List<char> { '(', '[', '{', '%', '#' };
                    List<char> glued3 = new List<char> { '|', '\\' };  //note special rules for '.', see glued3a
                    List<char> glued3a = new List<char> { '=', '+', '-', '/', '*', '^', '(', '{', '[', '<', '>', ',', ':', ' ' };  // "=.12", "+.12", "-.12" etc.
                    List<char> glued4 = new List<char> { '@' };  //only checked if no blank right of this
                    List<char> glued5 = new List<char> { '.' };  //only checked if no blank right of this
                    List<char> glued6 = new List<char> { '*', '?' };  //wildcards: a*b and a?b cannot have blanks.

                    //=========== note =========================
                    // [c1] [c2] [c3], where c2 is the char analyzed.
                    //==========================================

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < lineNewVersion.Length; i++)  //ignore if first on line
                    {
                        char c1 = '\n';
                        if (i > 0) c1 = lineNewVersion[i - 1];
                        char c2 = lineNewVersion[i];
                        char c3 = '\n';
                        if (i < lineNewVersion.Length - 1) c3 = lineNewVersion[i + 1];
                        char c4 = '\n';
                        if (i < lineNewVersion.Length - 2) c4 = lineNewVersion[i + 2];

                        // -------------------------------------------------------------
                        // Handle .1, .2, etc. For instance y.1 --> y[-1]
                        // -------------------------------------------------------------
                        if (c2 == '.' && char.IsDigit(c3) && !G.IsLetterOrDigitOrUnderscore(c4) && c4 != '.')
                        {
                            //now we have stuff like .1, and we need to check the chars before the '.'
                            bool good = false;
                            for (int ii = i - 1; ii >= 0; ii--)
                            {
                                //it may be for instance y.1 or y12345.1, so we run it backwards looking for
                                //digits only. When there are no more digits, it MUST be a letter or underscore
                                if (char.IsDigit(lineNewVersion[ii])) continue;
                                if (G.IsLetterOrUnderscore(lineNewVersion[ii])) good = true;
                                break;
                            }
                            //not ok: fy.11  fy.1a  fy.1_  fy.1.    All else is ok for fy[-1] translation, also if fy were fy12345 instead
                            //this will also get translated: %n.1  {s}.1   #m.1
                            if (good)
                            {
                                if (!lineNewVersion.Contains("'"))
                                {
                                    //You can have stuff like TABLE xx.currow.setvalues(1,2000,2010,1,'n',0.001,'f10.3'), where 'f10.3' should not become 'f10[-3]' !!!
                                    //This seems hard to solve properly, so the stuff here is only temporary
                                    //CONCLUSION: should be solved in the PARSER in the long run
                                    sb.Append(Globals.symbolGlueChar6 + "-" + c3 + "]");
                                    i++;
                                    continue;
                                }
                            }
                        }

                        // -------------------------------------------------------------
                        // Handle PRT<m d> etc.
                        // -------------------------------------------------------------
                        if (c2 == '<')
                        {
                            //Special rule to make sure the first two tokens inside <> in PRT<m d> or PRT<stamp row=yes> are not interpreted as time period
                            //If we have for instance "<m d>", the below will return true, and it will be transformed
                            //into "<?<m d>". This makes it easier to identify such cases in the parser. We would not like
                            //to try to interpret <m d> as a time period, so two raw idents will never be thought of as a
                            //period. That way, missspellings like "<m dd>" will be caught in syntax, and it will not try
                            //to understand <filter row> as dates either. But <%t1 %t2> will be just fine as dates, as will
                            //<2010 2012>. Expressions can also be used for dates.
                            //So all in there is special treatment of the first two items in <>, since this is the only place
                            //a date is allowed.
                            if (Has2IdentsFollowing(lineNewVersion, i + 1))
                            {
                                sb.Append(Globals.symbolGlueChar5);
                                continue;
                            }
                        }

                        // -------------------------------------------------------------
                        // Handle x(, x[, x{, %x, #x
                        // c2 is current char, c1 is previous
                        // -------------------------------------------------------------
                        if (c1 != '\n')
                        {
                            /*
                                   a(      --->   a?(     and same for the others
                                   a[      --->   special [_[ symbol
                                   a{
                                   a%  //part of name
                                   a#  //part of name

                                   )a  //for instance a%(%b)c, not need for glue here like this: a%(%b)|c as in a%d|c.
                                   ){  //same logic
                                   )%  //same logic
                                   )#  //same logic
                                   )[  //for instance #(list%i)[2] --> special [_[ symbol

                                   }a
                                   }{
                                   }%
                                   }#
                                   }[  //for instance {%a}[2000] or {a}[2000], --> special [_[ symbol

                                   %a
                                   %(
                                   %{

                                   ][  //for instance #m[3][2001q3], --> special [_[ symbol

                                   #(
                                   #{



                                   ...#a --> what is that??



                            */
                            bool glue = false;
                            bool glue2 = false;

                            if (G.IsLetterOrDigitOrUnderscore(c1) && c2 == '(') glue = true;
                            else if (G.IsLetterOrDigitOrUnderscore(c1) && c2 == '[') glue2 = true;
                            else if (G.IsLetterOrDigitOrUnderscore(c1) && c2 == '{') glue = true;
                            else if (G.IsLetterOrDigitOrUnderscore(c1) && c2 == '%') glue = true;
                            else if (G.IsLetterOrDigitOrUnderscore(c1) && c2 == '#') glue = true;

                            else if (c1 == ')' && G.IsLetterOrDigitOrUnderscore(c2)) glue = true;
                            else if (c1 == ')' && c2 == '{') glue = true;
                            else if (c1 == ')' && c2 == '%') glue = true;
                            else if (c1 == ')' && c2 == '#') glue = true;
                            else if (c1 == ')' && c2 == '[') glue2 = true;

                            else if (c1 == '}' && G.IsLetterOrDigitOrUnderscore(c2)) glue = true;
                            else if (c1 == '}' && c2 == '{') glue = true;
                            else if (c1 == '}' && c2 == '%') glue = true;
                            else if (c1 == '}' && c2 == '#') glue = true;
                            else if (c1 == '}' && c2 == '[') glue2 = true;

                            else if (c1 == '%' && G.IsLetterOrDigitOrUnderscore(c2)) glue = true;
                            else if (c1 == '%' && c2 == '(') glue = true;
                            else if (c1 == '%' && c2 == '{') glue = true;

                            //else if (c1 == Globals.symbolDollar[0] && G.IsLetterOrDigitOrUnderscore(c2)) glue = true;
                            //else if (c1 == Globals.symbolDollar[0] && c2 == '(') glue = true;
                            //else if (c1 == Globals.symbolDollar[0] && c2 == '{') glue = true;

                            else if (c1 == '#' && G.IsLetterOrDigitOrUnderscore(c2)) glue = true;
                            else if (c1 == '#' && c2 == '(') glue = true;
                            else if (c1 == '#' && c2 == '{') glue = true;

                            else if (c1 == ']' && c2 == '[') glue2 = true;

                            if (glue)
                            {
                                sb.Append(Globals.symbolGlueChar1);
                                sb.Append(c2);
                                continue;
                            }
                            else if (glue2)
                            {
                                sb.Append(Globals.symbolGlueChar6);
                                continue;
                            }

                            // -------------------------------------------------------------
                            // Handle x|x, x\\x
                            // c2 is current char, c1 is previous
                            // -------------------------------------------------------------
                            //glued3: '|', '\\'
                            else if (glued3.Contains(c2)) //add glue if "xx|yy", but not "xx| yy" or "xx |yy" or "xx | yy", and same regarding "\\"
                            {
                                //Handling '|' and '\\'
                                if (c3 != '\n')
                                {
                                    if (c1 != ' ' && c3 != ' ')
                                    {
                                        sb.Append(Globals.symbolGlueChar1); //12|34 --> 12?|34, and 12\\34 --> 12?\\34
                                        sb.Append(c2);
                                        continue;
                                    }
                                }
                            }
                        }

                        // -------------------------------------------------------------
                        // Handle stand-alone [a*b*c*d] that may look like a 1x1 matrix
                        // -------------------------------------------------------------
                        if (c2 == '[')
                        {
                            bool isProbablyStandAloneWildcardWithStars = false;
                            //if this '[' is glued to a name just before, it will have been handled above
                            //and given symbolGlueChar6: '[_[', and we would not end here
                            //So this must be a stand-alone '[', not an indexer on a name
                            for (int ii = i + 1; ii < lineNewVersion.Length; ii++)
                            {
                                if (lineNewVersion[ii] == ']')
                                {
                                    string s = lineNewVersion.Substring(i + 1, ii - (i + 1));
                                    if (s.Contains("*"))
                                    {
                                        string[] ss = s.Split('*');
                                        if (ss.Length > 1)
                                        {
                                            isProbablyStandAloneWildcardWithStars = true;  //looks good, we just need to check the bits
                                            //now we try to falsify it
                                            foreach (string sss in ss)
                                            {
                                                foreach (char c in sss)
                                                {
                                                    if (!G.IsLetterOrDigitOrUnderscore(c))
                                                    {
                                                        isProbablyStandAloneWildcardWithStars = false;
                                                    }
                                                }
                                            }
                                            //now it could be [a*b] or [*a] or [a*] or [a**b] or [1*2] or [a*5]
                                            if (s.StartsWith("*")) isProbablyStandAloneWildcardWithStars = false; //[*a]
                                            if (s.EndsWith("*")) isProbablyStandAloneWildcardWithStars = false;  //[a*]
                                            if (s.Contains("**")) isProbablyStandAloneWildcardWithStars = false;  //[a**b]
                                            if (char.IsDigit(s[0])) isProbablyStandAloneWildcardWithStars = false;  //[1*a]
                                        }
                                    }
                                }
                            }
                            if (isProbablyStandAloneWildcardWithStars)
                            {
                                sb.Append(Globals.symbolGlueChar7);
                                continue;
                            }
                        }


                        // -------------------------------------------------------------
                        // Handle @
                        // -------------------------------------------------------------
                        if (glued4.Contains(c2))
                        {
                            //handling '@'
                            if (c3 != '\n')
                            {
                                if (c3 == ' ')
                                {
                                    //ignore
                                }
                                else
                                {
                                    //PRT @x --> PRT @?x, but PRT @ x --> PRT @ x.
                                    //Note that the glue is AFTER the @.
                                    sb.Append(c2);
                                    sb.Append(Globals.symbolGlueChar1);
                                    continue;
                                }
                            }
                        }

                        // -------------------------------------------------------------
                        // Handle wildcards a*b, a?b -> a?*?b, a???b
                        // A * will get glue (?) to the left if there is ldu to the left. a* -> a?*
                        // A * will get glue (?) to the right if there is ldu the right.  *b -> *?b
                        // -------------------------------------------------------------
                        if (glued6.Contains(c2))
                        {
                            if (G.IsLetterOrDigitOrUnderscore(c1))
                            {
                                sb.Append(Globals.symbolGlueChar4);
                            }
                            sb.Append(c2);
                            if (G.IsLetterOrDigitOrUnderscore(c3))
                            {
                                sb.Append(Globals.symbolGlueChar4);
                            }
                            continue;
                        }

                        // -------------------------------------------------------------
                        // Handle SERIES #m = ...
                        // Problem is that it needs to be interpreted as UPD-type, not GENR-type
                        // -------------------------------------------------------------
                        if (true)
                        {
                            if (!G.IsEnglishLetter(c1) && (c2 == 's' || c2 == 'S') && (c3 == 'e' || c3 == 'E'))
                            {
                                bool success = false;
                                string ident1 = null;
                                int j1 = GetNextIdent(lineNewVersion, i, out ident1);
                                if (j1 != -12345)
                                {
                                    if (ident1.ToLower() == "ser" || ident1.ToLower() == "series")
                                    {


                                        //==========================================
                                        bool hasStarted = false;
                                        for (int ii = j1; ii < lineNewVersion.Length; ii++)
                                        {
                                            if (lineNewVersion[ii] == ' ') continue;
                                            if (lineNewVersion[ii] == '<')
                                            {
                                                hasStarted = true;
                                                continue;
                                            }
                                            if (hasStarted && lineNewVersion[ii] == '>')
                                            {
                                                j1 = ii + 1;
                                                break;
                                            }
                                        }
                                        //==========================================


                                        int j2 = GetNextHash(lineNewVersion, j1);
                                        if (j2 != -12345)
                                        {
                                            string ident3 = null;
                                            int j3 = GetNextIdent(lineNewVersion, j2, out ident3);
                                            if (j3 != -12345)
                                            {
                                                int j4 = GetNextEquals(lineNewVersion, j3);
                                                if (j4 != -12345)
                                                {
                                                    success = true;
                                                    sb.Append(c2);
                                                    sb.Append("_");
                                                    sb.Append("_");
                                                    sb.Append("_");  //SER -> S___ER, SERIES -> S___ERIES, see also //#098275432874
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // -------------------------------------------------------------
                        // Handle SERIES y = 1 -2 -2 -1 and SERIES y = 1 -2 -2 -1*2
                        // Problem is that the second is genr type, but the parser starts treating it like upd type.
                        // -------------------------------------------------------------
                        if (true)
                        {
                            if (!G.IsEnglishLetter(c1) && (c2 == 's' || c2 == 'S') && (c3 == 'e' || c3 == 'E'))
                            {
                                bool success = false;
                                string ident1 = null;
                                int j1 = GetNextIdent(lineNewVersion, i, out ident1);
                                if (j1 != -12345)
                                {
                                    if (ident1.ToLower() == "ser" || ident1.ToLower() == "series")
                                    {


                                        //==========================================
                                        bool hasStarted = false;
                                        for (int ii = j1; ii < lineNewVersion.Length; ii++)
                                        {
                                            if (lineNewVersion[ii] == ' ') continue;
                                            if (lineNewVersion[ii] == '<')
                                            {
                                                hasStarted = true;
                                                continue;
                                            }
                                            if (hasStarted && lineNewVersion[ii] == '>')
                                            {
                                                j1 = ii + 1;
                                                break;
                                            }
                                        }
                                        //==========================================


                                        int j3 = GetNextEquals2(lineNewVersion, j1);
                                        if (j3 != -12345)
                                        {
                                            int j4 = GetNextComment(lineNewVersion, j3);
                                            string rest = null;
                                            if (j4 == -12345) rest = lineNewVersion.Substring(j3, lineNewVersion.Length - j3);
                                            else rest = lineNewVersion.Substring(j3, j4 - j3);

                                            int semi = rest.IndexOf(";");
                                            if (semi != -1)
                                            {
                                                rest = rest.Substring(0, semi);
                                            }

                                            StringTokenizer tok = new StringTokenizer(rest, false, false);
                                            tok.IgnoreWhiteSpace = false;
                                            tok.SymbolChars = new char[] { '%', '&', '/', '(', ')', '=', '?', '@', '$', '{', '[', ']', '}', '+', '|', '^', '*', '<', '>', ';', ',', ':', '-' };
                                            Token token;
                                            int numberCounter = 0;
                                            bool simpleBlankSeparatedUpd = true;
                                            do
                                            {
                                                token = tok.Next();
                                                string value = token.Value;
                                                string kind = token.Kind.ToString();
                                                if (kind == "EOF" || kind == "WhiteSpace" || kind == "Number" || (kind == "Word" && value.ToLower() == "m") || (kind == "Symbol" && value == "-"))
                                                {
                                                    //continue
                                                }
                                                else
                                                {
                                                    simpleBlankSeparatedUpd = false;
                                                    break;
                                                }
                                                if (kind == "Number") numberCounter++;
                                            } while (token.Kind != ETokenType.EOF); //TokenKind.EOF

                                            //Must have at least two numbers to be activated
                                            if (simpleBlankSeparatedUpd && numberCounter == 1)
                                            {
                                                //number = 0: if the numbers are on the next line. That is typically upd type then.
                                                //it must be rather seldom that we have 1 number on the SERIES line, and the rest
                                                //on the next line, and that such a line is upd type.
                                                simpleBlankSeparatedUpd = false;
                                            }

                                            if (simpleBlankSeparatedUpd)
                                            {
                                                //will be of the form "1 -1 -2 3 4 -5 1e6 5" (with at least 2 numbers)
                                                //if (Globals.runningOnTTComputer) G.Writeln("DETECTED simpleBlankSeparatedUpd!");
                                                //really ugly hack
                                                success = true;
                                                sb.Append(c2);
                                                sb.Append("_");
                                                sb.Append("_");
                                                sb.Append("_");
                                                sb.Append("_");  //SER -> S____ER, SERIES -> S____ERIES, see also //#098275432874
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // -------------------------------------------------------------
                        // Handle dots (.)
                        // -------------------------------------------------------------
                        if (glued5.Contains(c2))
                        {
                            //c2 is a '.'
                            if (c1 != '\n' && c3 != '\n')
                            {
                                if (c3 != '\n')
                                {
                                    if (c3 == ' ')
                                    {
                                        //do nothing, normal dot, for instance 12. 34
                                    }
                                    else if (char.IsDigit(c3))
                                    {
                                        if (glued3a.Contains(c1))
                                        {
                                            //  +.12, **.12, >.12, (.12, etc.
                                            sb.Append(Globals.symbolGlueChar3);  //GLUEDOTNUMBER
                                            sb.Append(c2);
                                            continue;
                                        }
                                        else if (char.IsDigit(c1))
                                        {
                                            //in stuff like 12.34 the dot becomes a GLUEDOTNUMBER
                                            //but only if stuff before 12 is not ident, for instance
                                            //x12.34. We could have hgn2.1, and that is not a number.
                                            bool number = true;
                                            for (int ii = i - 1 - 1; ii >= 0; ii--)
                                            {
                                                //.... +123.45 loops through pure digits until + is met. Here number would be true.
                                                if (glued3a.Contains(lineNewVersion[ii])) break;  //for instance a "," or "+" to delimit the number ('token')
                                                if (!char.IsDigit(lineNewVersion[ii]))
                                                {
                                                    number = false;
                                                    break;
                                                }
                                            }
                                            if (number)
                                            {
                                                sb.Append(Globals.symbolGlueChar3);  //GLUEDOTNUMBER
                                                sb.Append(c2);
                                                continue;
                                            }
                                            else
                                            {
                                                sb.Append(Globals.symbolGlueChar2);  //GLUEDOT
                                                sb.Append(c2);
                                                continue;
                                            }
                                        }
                                    }
                                    if (c1 != ' ' && c2 != ' ')
                                    {
                                        sb.Append(Globals.symbolGlueChar2);  //GLUEDOT
                                        sb.Append(c2);
                                        continue;
                                    }


                                }
                                else
                                {
                                    //ending with a dot
                                }
                            }
                            else
                            {
                                if (c3 != '\n')
                                {
                                    if (char.IsDigit(c3))
                                    {
                                        //if line starts with .1, the dot is a GLUEDOTNUMBER
                                        sb.Append(Globals.symbolGlueChar3);  //GLUEDOTNUMBER
                                        continue;
                                    }
                                }
                                else
                                {
                                    //ending with a dot
                                }
                            }
                        }

                        sb.Append(c2);
                    }
                    lineNewVersion = sb.ToString();
                }
                inputFileLines2.Add(lineNewVersion);
            }
            if (inputFileLines.Count != inputFileLines2.Count) throw new GekkoException();

            List<string> inputFileLines3 = new List<string>();

            if (Globals.runningOnTTComputer)
            {
                if (Globals.printAST)
                {
                    foreach (string s in inputFileLines2)
                    {
                        G.Writeln("-debug- " + s, Color.Orange);

                    }
                }
            }

            return inputFileLines2;
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
            List<string> lines = Stringlist.ExtractLinesFromText(csCode);
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
                new Error("Technical issue with code-splitting, please use OPTION system code split = 0");
                //throw new GekkoException();
            }

            csCode = Stringlist.ExtractTextFromLines(mainCs).ToString();
            csMethods = Stringlist.ExtractTextFromLines(methodsCs).ToString();

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
                    if (Globals.useTrace)
                    {
                        //if needed, we could decorate ASTREAD... etc with the command.
                        //maybe not necessary.
                        //if (ast.Text.StartsWith("ASTREAD" + Globals.parserExpressionSeparator))
                        //{
                        //    flag = true;
                        //}
                        //else if (ast.Text.StartsWith("ASTCOPY" + Globals.parserExpressionSeparator))
                        //{
                        //    flag = true;
                        //}
                    }

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
                    cmdNode.Text = G.ReplaceGlueSymbols(ss[0]);
                    cmdNode.specialExpressionAndLabelInfo = ss;
                }
            }

            if (cmdNode.Text != null)
            {
                cmdNode.Text = G.ReplaceGlueSymbols(cmdNode.Text);
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

        /// <summary>
        /// This method prints syntax errors (of the type with illegal symbols)
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="inputFileLines"></param>
        /// <param name="ph"></param>
        public static void HandleCommandLexerErrors(List<string> errors, List<string> inputFileLines, ParseHelper ph)
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
                string fileName = null;
                try
                {
                    lineNumber = int.Parse(ss[0]) - 1;  //seems 1-based before subtract 1
                    fileName = ph.fileName;
                    Program.CorrectLineNumber(ref fileName, ref lineNumber);
                }
                catch (Exception e)
                {
                    new Error("The parser stumbled unexpectedly with the message: " + s);
                }
                int lineNo = lineNumber + 1;  //1-based
                int positionNo = -12345;
                try
                {
                    positionNo = int.Parse(ss[1]) + 1;  //1-based
                }
                catch (Exception e)
                {
                    new Error("The parser stumbled unexpectedly with the message: " + s);
                }

                string errorMessage = ss[3];

                errorMessage = errorMessage.Replace(@"'\\r\\n'", "<newline>");  //easier to understand
                errorMessage = errorMessage.Replace("Antlr.Runtime.NoViableAltException", "Illegal characters"); //easier to understand

                if (lineNo > inputFileLines.Count)
                {
                    {
                        new Error(errorMessage, false);
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
                    G.Writeln("    " + G.ReplaceGlueSymbols(inputFileLines[0]), Color.Blue);
                    G.Writeln("*** ERROR: " + errorMessage);
                }
                else
                {
                    if (ph.isOneLinerFromGui == false)
                    {
                        {
                            string fn = fileName;
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

                    lineTemp = G.ReplaceGlueSymbols(lineTemp);

                    string line0 = lineTemp.Substring(0, positionNo - 1);
                    string line1 = lineTemp.Substring(positionNo - 1, 1);
                    string line2 = lineTemp.Substring(positionNo - 1 + 1);

                    if (previousLineProbablyCulprit && lineNo > 1)
                    {
                        G.Writeln("    " + "Line " + (lineNo - 1) + " may be the real cause of the problem");
                        string lineBefore = inputFileLines[lineNo - 1 - 1];
                        G.Writeln("    " + "[" + G.IntFormat(lineNo - 1, 4) + "]:" + "   " + G.ReplaceGlueSymbols(lineBefore), Color.Blue);
                    }

                    G.Write("    " + "[" + G.IntFormat(lineNo, 4) + "]:" + "   " + line0, Color.Blue);
                    G.Write(line1, Color.Red);
                    G.Writeln(line2, Color.Blue);

                    G.Writeln(G.Blanks(positionNo - 1 + 4 + 5 + 5) + "^", Color.Blue);
                    G.Writeln(G.Blanks(positionNo - 1 + 4 + 5 + 5) + "^", Color.Blue);                    
                }

                if (paranthesesError != "") G.Writeln(paranthesesError);

                if (ph.isModel == false && previousLineProbablyCulprit == false)
                {
                    WriteLinkToHelpFile2(G.ReplaceGlueSymbols(line));
                    if (number == 1) ExtraErrorMessages(G.ReplaceGlueSymbols(line));
                }
            }
            if (errors.Count > 1) G.Writeln("--------------------- end of " + errors.Count + " errors --------------");

            using (Note txt = new Note())
            {
                txt.MainAdd("Illegal chars/symbols encountered, beware of non-matching quotes.");
                txt.MoreAdd("Quotes like ' or \" can trigger these kinds of errors. But they may also");
                txt.MoreAdd("stem from 'funny' characters/symbols, like '€' or '½' etc.");
                txt.MoreAdd("The hardest cases are characters that show up as blanks, but are in reality some other code. This mostly happens while copy-pasting code.");
                txt.MoreNewLine();
                txt.MoreAdd("The best thing to do is to start checking if quotes ' and \" match up. If so, you can perhaps run the commands line by line to find the offending character/symbol.");
            }
        }

        /// <summary>
        /// This method prints syntax errors
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="inputFileLines"></param>
        /// <param name="ph"></param>
        public static void HandleCommandParserErrors(List<string> errors, List<string> inputFileLines, ParseHelper ph)
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
                }
                else G.Writeln();
                
                int lineNumber, positionNumber;
                string errorMessage, fileName;
                ExtractParserErrorLineAndPos(s, ph.fileName, out lineNumber, out positionNumber, out errorMessage, out fileName);

                errorMessage = G.ReplaceGlueSymbols(errorMessage);

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


                if (lineNumber > inputFileLines.Count)
                {
                    {
                        G.Writeln("*** ERROR: " + errorMessage);
                    }

                    continue;  //doesn't give meaning
                }
                string line = "";
                int firstWordPosInLine = -12345;
                bool previousLineProbablyCulprit = false;
                if (lineNumber > 0)
                {
                    line = inputFileLines[lineNumber - 1];
                    firstWordPosInLine = line.Length - line.TrimStart().Length + 1;
                }

                if (true)
                {
                    if (positionNumber == firstWordPosInLine && errorMessage.Contains("no viable"))
                    {
                        //get preceding line (or really: statement) -- most probably the culprit.
                        previousLineProbablyCulprit = true;
                    }

                    if (ph.isOneLinerFromGui == true && lineNumber != 1)
                    {
                        G.Writeln("*** ERROR: Parsing this line:");
                        G.Writeln("    " + G.ReplaceGlueSymbols(inputFileLines[0]), Color.Blue);
                        G.Writeln("*** ERROR: " + errorMessage);
                    }
                    else
                    {
                        if (ph.isOneLinerFromGui == false)
                        {
                            string fn = fileName;
                            string extra = "";
                            if (lineNumber >= 1 && positionNumber > 0)
                            {
                                extra = " line " + lineNumber + " pos " + positionNumber;
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
                            if (positionNumber > 0)
                            {
                                G.Writeln("*** ERROR: Parsing pos " + positionNumber + ":  " + errorMessage);
                            }
                            else G.Writeln("*** ERROR: " + errorMessage);
                        }
                        line = line + "  ";  //hack to avoid ending problems.....

                        if (positionNumber - 1 >= 0)
                        {
                            string lineTemp = line;
                            if (lineTemp != null && lineTemp != "")
                            {
                                lineTemp2.Add(lineTemp);  //used for suggestions later on
                                lineTemp2Numbers.Add("    " + "[" + G.IntFormat(lineNumber, 4) + "]:");
                            }

                            lineTemp = G.ReplaceGlueSymbols(lineTemp);

                            //try: not the end of the world if one of these fails
                            string line0 = "";
                            string line1 = "";
                            string line2 = "";
                            try { line0 = lineTemp.Substring(0, positionNumber - 1); } catch { };
                            try { line1 = lineTemp.Substring(positionNumber - 1, 1); } catch { };
                            try { line2 = lineTemp.Substring(positionNumber - 1 + 1); } catch { };

                            if (previousLineProbablyCulprit && lineNumber > 1)
                            {
                                G.Writeln("    " + "Line " + (lineNumber - 1) + " may be the real cause of the problem");
                                string lineBefore = inputFileLines[lineNumber - 1 - 1];
                                G.Writeln("    " + "[" + G.IntFormat(lineNumber - 1, 4) + "]:" + "   " + G.ReplaceGlueSymbols(lineBefore), Color.Blue);
                            }

                            G.Write("    " + "[" + G.IntFormat(lineNumber, 4) + "]:" + "   " + line0, Color.Blue);
                            G.Write(line1, Color.Red);
                            G.Writeln(line2, Color.Blue);

                            G.Writeln(G.Blanks(positionNumber - 1 + 4 + 5 + 5) + "^", Color.Blue);
                            G.Writeln(G.Blanks(positionNumber - 1 + 4 + 5 + 5) + "^", Color.Blue);

                            CheckForBadDouble(lineTemp);
                        }
                    }
                }

                if (ph.isModel == false)
                {
                    WriteLinkToHelpFile2(G.ReplaceGlueSymbols(line));
                    if (number == 1) ExtraErrorMessages(G.ReplaceGlueSymbols(line));
                }
            }
            if (errors.Count > 1) G.Writeln("--------------------- end of " + errors.Count + " errors --------------");

        }

        public static void ExtractParserErrorLineAndPos(string s, string fileName2, out int lineNumber, out int positionNo, out string errorMessage, out string fileName)
        {
            string[] ss = s.Split(Globals.parserErrorSeparator);
            int lineNumberTemp = 0;
            lineNumber = 0;
            positionNo = 0;
            errorMessage = "General error";
            fileName = null;
            try
            {
                lineNumberTemp = int.Parse(ss[0]) - 1;  //seems 1-based before subtract 1                
                lineNumber = lineNumberTemp + 1;  //1-based
                positionNo = int.Parse(ss[1]) + 1;  //1-based                               
                errorMessage = ss[3];
                fileName = fileName2;
                Program.CorrectLineNumber(ref fileName, ref lineNumber);
            }
            catch
            {

            }
        }

        private static void CheckForBadDouble(string lineTemp)
        {
            string xx = G.ReplaceGlueSymbols(lineTemp.Trim());
            StringTokenizer tok = new StringTokenizer(xx, false, true);
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
                    using (Note n = new Note())
                    {
                        n.MainAdd("You cannot write a number like '" + al[i] + "' ending with period, please use");
                        n.MainAdd("'" + al[i] + "0'. Such numbers interfere really poorly with the range ");
                        n.MainAdd("indicator '..' used in Gekko, but the numbers are legal in the model");
                        n.MainAdd("(.frm) file, at least for the time being.");
                    }                        
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
                new Writeln("See {a{" + firstWord.ToUpper() + "¤" + firstWord + ".htm" + "}a} in help system.");               
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


