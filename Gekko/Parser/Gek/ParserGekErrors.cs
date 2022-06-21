using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System.Drawing;
using System.IO;

namespace Gekko.Parser.Gek
{
    /// <summary>
    /// Assembles errors.
    /// </summary>
    public class LexerAndParserErrors
    {
        public List<string> lexerErrors = null;
        public List<string> parserErrors = null;
    }

    public class ErrorHelper
    {
        public List<string> errors = null;
        public int offset = 0;
    }

    /// <summary>
    /// Gathers error info on 1 Gekko statement
    /// </summary>
    public class Statement
    {
        public List<TokenHelper> tokens = new List<TokenHelper>();
        public List<int?> paren_parentheses = new List<int?>();
        public List<int?> paren_brackets = new List<int?>();
        public List<int?> paren_curlies = new List<int?>();
        public List<List<string>> parenthesisErrors = new List<List<string>>();  //assigned to tokens
        public List<string> parenthesisErrors2 = new List<string>();             //assigned to statement
        public string text = null;
        public int type = 2;  //0 normal 1 series 2 naked func procedure. Set to 2 to start out because it is hardest to determine (we test for 0 or 1)
        public SortedDictionary<long, ErrorHelper> errorDictionary = null;
    }

    /// <summary>
    /// Tries to give resonable error messages
    /// </summary>
    public class ParserGekErrors
    {
        /// <summary>
        /// Gets error messages statement by statement, so that these do not get mixed up. Skips "end;" statements,
        /// and adds "end;" on for/if/block/procedure/function. For eatch statement, the parser is called individually.
        /// If it does not encounter any errors (for instance a file like "x = 2; y = 3" where only 1 correct statement is 
        /// identified), it reverts to the "old" error messages.
        /// </summary>
        /// <param name="ph"></param>
        /// <param name="parseOutput"></param>
        /// <param name="textWithExtraLines"></param>
        /// <param name="t"></param>
        /// <param name="errorStatements"></param>
        public static bool ErrorMessages(ErrorMessagesHelper helper, ParseHelper ph, int maxCalc, int maxShow)
        {
            bool showLetters = false;
            bool condense = true;
            if (!showLetters) condense = true;

            List<string> originalText = Stringlist.ExtractLinesFromText(ph.commandsText);

            List<Statement> statements = GetStatements(ph);

            int linesWithErrors = 0;
            foreach (Statement statement in statements)
            {
                bool startFor = false;
                bool startIf = false;
                ParseHelper ph7 = ph.Clone();
                ph7.isDebugMode = true;

                string s7 = statement.text;

                TokenHelper firstWord = null;
                TokenHelper next = null;
                long start = -12345; long end = -12345;
                foreach (TokenHelper th in statement.tokens)
                {
                    if (start == -12345) start = (long)1e9 * th.line + th.column;
                    end = (long)1e9 * th.line + th.column;
                    if (firstWord != null && next == null && (th.type != ETokenType.Comment))
                    {
                        next = th;
                        break;
                    }
                    if (firstWord == null && th.type == ETokenType.Word)
                    {
                        firstWord = th;
                    }
                }

                if (Globals.commandNames.Contains(firstWord.s.ToUpper())) statement.type = 0;

                if (G.Equal(firstWord.s, "end") && next != null && next.s == ";") continue;

                if (G.Equal(firstWord.s, "for") || G.Equal(firstWord.s, "if") || G.Equal(firstWord.s, "block") || G.Equal(firstWord.s, "function") || G.Equal(firstWord.s, "procedure"))
                {
                    s7 += "end;";
                }
                ph7.commandsText = s7;
                ph7.syntaxType = ParserGekCreateAST.EParserType.Normal;
                if (statement.type == 1) ph7.syntaxType = ParserGekCreateAST.EParserType.OnlyAssignment;
                else if (statement.type == 2) ph7.syntaxType = ParserGekCreateAST.EParserType.OnlyProcedureCallEtc;

                // ========================================
                // calling parser
                // ========================================
                ConvertHelper parseOutput7; string textWithExtraLines7; CommonTree t7;
                LexerAndParserErrors lexerAndParserErrors7 = ParserGekCreateAST.ParseAndSyntaxErrors(out parseOutput7, out textWithExtraLines7, out t7, ph7);

                if (lexerAndParserErrors7.parserErrors != null && lexerAndParserErrors7.parserErrors.Count > 0)
                {
                    statement.errorDictionary = GetErrorsFromOneStatement(ph, maxCalc, statement, ph7, lexerAndParserErrors7);
                }

                if (statement.errorDictionary != null) linesWithErrors++;                
                if (linesWithErrors >= maxCalc) break;  //no need to carry on

            } //end loop over statements and calling the parser

            if (linesWithErrors == 0) return true;  //signals a problem, and we revert to "old" error messages.            
            
            WritelnError("", true);

            if (true)
            {
                using (Error txt = new Error())
                {
                    //will be shown even if piping or muting

                    string fileNameWithoutPath = "";
                    fileNameWithoutPath = "'" + Path.GetFileName(ph.fileName) + "'";
                    string part = "in " + fileNameWithoutPath;
                    if (fileNameWithoutPath == "''") part = "[screen input]";

                    txt.ThrowNoException();

                    if (true)
                    {
                        string plus = null;
                        if (linesWithErrors >= maxCalc) plus = "+";

                        Action<GAO> a = (gao) =>
                        {
                            ErrorMessages(helper, ph, maxCalc, maxCalc - 1);
                        };

                        int number = linesWithErrors;
                        if (plus != null) number--;

                        string link = null;
                        string x = null;
                        if (number > 1 && maxShow == 1) link = " (" + G.GetLinkAction("show " + number + "", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ")";
                        if (maxShow == 1) x = " 1 of " + number;
                        txt.MainAdd("Showing syntax errors from" + x + plus + " erroneous statements " + part + link);
                    }
                }
            }

            int widthRemember = Program.options.print_width;  //reset width afterwards (in finally {})
            Program.options.print_width = int.MaxValue;

            try
            {
                int counter = 0;
                foreach (Statement statement in statements)
                {
                    if (statement.errorDictionary != null)
                    {
                        SortedDictionary<long, ErrorHelper> originalErrors = CloneErrorDictionary(statement.errorDictionary);
                        counter++;                        

                        SortedDictionary<int, string> split = new SortedDictionary<int, string>();  //relevant lines for multi-line errors
                        foreach (KeyValuePair<long, ErrorHelper> kvp in statement.errorDictionary)
                        {
                            int line = (int)(kvp.Key / (long)1e9);
                            if (!split.ContainsKey(line)) split.Add(line, null);
                        }

                        string start2 = null;
                        foreach (int line2 in split.Keys)
                        {
                            //
                            // ---------- print a line from the statement
                            //

                            //Also see replacement: #9j5n34jererjn
                            string start = "[" + line2 + "]: ";
                            start2 = G.Blanks(start.Length);
                            WritelnError("", true);
                            string statementLine = originalText[line2 - 1];
                            string statementLine2 = statementLine + "   ";  //easier
                            StringBuilder statementLine3 = new StringBuilder();
                            
                            //
                            // handle glue symbols
                            //
                            
                            statementLine = HandleGlueSymbols(statement, statementLine, statementLine2, statementLine3);                            

                            //
                            // print statement
                            //

                            WritelnError(start + statementLine, false);

                            //
                            // make ^ visual pointers
                            //

                            int cOld = 0; int errorCounter = 0;
                            string s1 = start2;
                            string s2 = start2;
                            foreach (KeyValuePair<long, ErrorHelper> kvp in statement.errorDictionary)
                            {
                                int ln = (int)(kvp.Key / (long)1e9);
                                int col = (int)(kvp.Key % (long)1e9);
                                col += kvp.Value.offset;  //kind of like cheating, but much easier because keys of a dict cannot be changed
                                if (ln != line2) continue;
                                errorCounter++;
                                char letter = (char)(97 + errorCounter - 1);
                                int c = col - 1 - cOld;
                                s1 += G.Blanks(c) + "^";
                                s2 += G.Blanks(c) + letter;
                                cOld = col;
                            }

                            //
                            // ---------- print ^ pointers below the statement line
                            //

                            WritelnError(s1, true);
                            if (showLetters) WritelnError(s2, true);

                            //The list of errors line by line

                            string indent = start2;
                            indent = "  - ";

                            //
                            // ---------- print the detailed info on each ^ as lines below
                            //

                            errorCounter = 0;
                            foreach (KeyValuePair<long, ErrorHelper> kvp in statement.errorDictionary)
                            {
                                int ln = (int)(kvp.Key / (long)1e9);
                                int col = (int)(kvp.Key % (long)1e9);
                                if (ln != line2) continue;
                                errorCounter++;
                                char letter = (char)(97 + errorCounter - 1);
                                if (condense)
                                {
                                    string s3 = null;
                                    string error = null;
                                    foreach (string s9 in kvp.Value.errors)
                                    {
                                        string s = G.ReplaceGlueSymbols(s9);
                                        error += G.FirstCharToUpper(s) + ". ";
                                    }
                                    error = error.Substring(0, error.Length - ". ".Length);
                                    if (showLetters) s3 = "(" + letter + "): " + G.FirstCharToUpper(error);
                                    else
                                    {
                                        s3 = indent + G.FirstCharToUpper(error);
                                        //s3 = "(^): " + G.FirstCharToUpper(error);
                                    }
                                    if (!s3.EndsWith(".")) s3 = s3 + ".";
                                    s3 += " [" + ln + ":" + (col + kvp.Value.offset) + "]";
                                    WritelnError(s3, true);
                                }
                                else
                                {
                                    foreach (string s9 in kvp.Value.errors)
                                    {
                                        string s = G.ReplaceGlueSymbols(s9);
                                        string s3 = null;
                                        if (showLetters) s3 = "(" + letter + "): " + G.FirstCharToUpper(s);
                                        else
                                        {
                                            s3 = indent + G.FirstCharToUpper(s);
                                            //s3 = "(^): " + G.FirstCharToUpper(s);
                                        }
                                        if (!s3.EndsWith(".")) s3 = s3 + ".";
                                        s3 += " [" + ln + ":" + (col + kvp.Value.offset) + "]";
                                        WritelnError(s3, true);
                                    }
                                }
                            }
                        }
                        string extra = null;
                        foreach (string s8 in statement.parenthesisErrors2)
                        {
                            extra += G.FirstCharToUpper(s8) + ". ";
                        }

                        string help = ParserGekCreateAST.GetLinkToHelpFile2(statement.text.Trim());

                        if (extra != null || help != null)
                        {
                            if (extra != null)
                            {
                                extra = extra.Substring(0, extra.Length - ". ".Length);
                                if (!extra.EndsWith(".")) extra = extra + ". ";
                            }

                            string indent = start2;
                            indent = "";                            

                            using (Writeln txt = new Writeln("+++ ", -12345, Color.Red, true, ETabs.Main))
                            {
                                txt.MainAdd("Statement note: " + extra + help);
                            }
                        }

                        if (CountErrors(statement.errorDictionary) != CountErrors(originalErrors))
                        {
                            if (Globals.runningOnTTComputer) new Error("Dict mismatch");
                        }

                        if (false)
                        {
                            G.Writeln();
                            foreach (KeyValuePair<long, ErrorHelper> kvp in originalErrors)
                            {
                                int line = (int)(kvp.Key / (long)1e9);
                                int pos = (int)(kvp.Key % (long)1e9);
                                foreach (string s8 in kvp.Value.errors)
                                {
                                    G.Writeln("ln " + line + " col " + pos + ": " + s8);
                                }
                            }
                            foreach (string s8 in statement.parenthesisErrors2)
                            {
                                G.Writeln(s8);
                            }
                        }
                    }
                    if (counter >= maxShow) break;
                }
            }
            finally
            {
                Program.options.print_width = widthRemember;
            }
            G.Write("");  //removes marking
            return false;
        }

        private static string HandleGlueSymbols(Statement statement, string statementLine, string statementLine2, StringBuilder statementLine3)
        {
            List<string> glues1 = new List<string>() { Globals.symbolGlueChar2, Globals.symbolGlueChar3, Globals.symbolGlueChar4, Globals.symbolGlueChar5, Globals.symbolGlueChar6, Globals.symbolGlueChar7, Globals.symbolGlueChar1.ToString()};            
            int[] glue = new int[statementLine.Length];  //1 means it is being removed
            int collapse = 0;
            for (int i = 0; i < statementLine.Length; i++)
            {
                bool hit = false;
                foreach (string g in glues1)
                {
                    if (statementLine2.Substring(i, g.Length) == g)
                    {
                        //we put '£' symbols in

                        hit = true;
                        bool big = false;
                        if (g.Length > 1) big = true;

                        if (big)
                        {
                            statementLine3.Append('£');
                            statementLine3.Append('£');
                            statementLine3.Append(statementLine2[i + 2]);  //only the third
                            i += 2;
                        }
                        else
                        {
                            statementLine3.Append('£');
                        }
                        break;
                    }
                }
                if (!hit)
                {
                    statementLine3.Append(statementLine2[i]);
                }
            }

            statementLine = statementLine3.ToString();
            StringBuilder statementLine4 = new StringBuilder();

            //we move all pointers away from glue

            SortedDictionary<long, ErrorHelper> errorDict2 = new SortedDictionary<long, ErrorHelper>();
            for (int i = 0; i < statementLine.Length; i++)
            {
                int col2 = i + 1;
                int col3 = col2;
                if (statementLine[i] == '£')
                {
                    for (int j = i; j < statementLine.Length; j++)
                    {
                        if (statementLine[j] != '£')
                        {
                            col3 = j + 1;
                            break;
                        }
                    }
                }

                //move to safety
                foreach (KeyValuePair<long, ErrorHelper> kvp in statement.errorDictionary)
                {
                    int ln = (int)(kvp.Key / (long)1e9);
                    int col = (int)(kvp.Key % (long)1e9);
                    if (col != col2) continue;
                    col = col3;
                    ErrorsAddOrMerge(errorDict2, (long)1e9 * ln + col, kvp.Value.errors);
                }
            }

            //if (CountErrors(statement.errorDictionary) != CountErrors(errorDict2)) new Error("Hov");

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < statementLine.Length; i++)
            {
                int col2 = i + 1;
                if (statementLine[i] == '£')
                {
                    //move pointers 1 pos to the left
                    foreach (KeyValuePair<long, ErrorHelper> kvp in errorDict2)
                    {
                        int ln = (int)(kvp.Key / (long)1e9);
                        int col = (int)(kvp.Key % (long)1e9);
                        if (col < col2) continue;  //only move after '£'
                        kvp.Value.offset -= 1;
                    }
                }
                else
                {
                    sb.Append(statementLine[i]);
                }
            }

            //if (CountErrors(statement.errorDictionary) != CountErrors(errorDict2)) new Error("Hov");

            statement.errorDictionary = errorDict2;
            statementLine = sb.ToString();
            statement.text = statementLine;
            return statementLine;
        }

        private static int CountErrors(SortedDictionary<long, ErrorHelper> errorDictionary)
        {
            int n1 = 0;
            foreach (KeyValuePair<long, ErrorHelper> kvp in errorDictionary)
            {
                n1 += kvp.Value.errors.Count;
            }
            return n1;
        }

        private static SortedDictionary<long, ErrorHelper> CloneErrorDictionary(SortedDictionary<long, ErrorHelper> errorsX)
        {
            if (errorsX == null)
                return null;

            SortedDictionary<long, ErrorHelper> errors2 = new SortedDictionary<long, ErrorHelper>();
            foreach (KeyValuePair<long, ErrorHelper> kvp in errorsX)
            {
                ErrorHelper eh = new ErrorHelper();
                eh.errors = new List<string>();
                eh.errors.AddRange(kvp.Value.errors);
                errors2.Add(kvp.Key, kvp.Value);
            }
            return errors2;
        }

        private static void WritelnError(string s, bool red)
        {
            Color c = Color.Black;
            if (red) c = Color.Red;
            G.Writeln(s, c, true);
        }

        /// <summary>
        /// Assembles error messages from 1 Gekko statement with syntax errors (statement may run over several lines).
        /// It returns a dictionary where the keys point to the exact position, and the values are a list of errors
        /// for that exact position (coordinate). After each error, with a "¤", the statement is glued on, too.
        /// </summary>
        /// <param name="ph"></param>
        /// <param name="errorStatements"></param>
        /// <param name="statement"></param>
        /// <param name="ph7"></param>
        /// <param name="lexerAndParserErrors7"></param>
        private static SortedDictionary<long, ErrorHelper> GetErrorsFromOneStatement(ParseHelper ph, int errorStatements, Statement statement, ParseHelper ph7, LexerAndParserErrors lexerAndParserErrors7)
        {
            int last_parenthesis = 0;
            int last_bracket = 0;
            int last_curly = 0;

            for (int i7 = 0; i7 < statement.tokens.Count; i7++)
            {
                statement.parenthesisErrors.Add(new List<string>());
            }

            for (int i7 = 0; i7 < statement.tokens.Count; i7++)
            {
                if (statement.paren_parentheses[i7] != null) last_parenthesis = (int)statement.paren_parentheses[i7];
                if (statement.paren_brackets[i7] != null) last_bracket = (int)statement.paren_brackets[i7];
                if (statement.paren_curlies[i7] != null) last_curly = (int)statement.paren_curlies[i7];

                if (statement.paren_parentheses[i7] != null && statement.paren_parentheses[i7] < 0)
                {
                    statement.parenthesisErrors[i7].Add("extraneous ')'");
                }

                if (statement.paren_brackets[i7] != null && statement.paren_brackets[i7] < 0)
                {
                    statement.parenthesisErrors[i7].Add("extraneous ']'");
                }

                if (statement.paren_curlies[i7] != null && statement.paren_curlies[i7] < 0)
                {
                    statement.parenthesisErrors[i7].Add("extraneous '}'");
                }
            }
            if (last_parenthesis > 0) statement.parenthesisErrors2.Add("missing " + last_parenthesis + " ')' in statement");
            if (last_bracket > 0) statement.parenthesisErrors2.Add("missing " + last_bracket + " ']' in statement");
            if (last_curly > 0) statement.parenthesisErrors2.Add("missing " + last_curly + " '}' in statement");

            SortedDictionary<long, ErrorHelper> errors = new SortedDictionary<long, ErrorHelper>();

            for (int i7 = 0; i7 < statement.tokens.Count; i7++)
            {
                if (statement.parenthesisErrors[i7].Count > 0)
                {
                    int ln; string lineText;
                    AdjustLine(ph, 1, statement.tokens[i7].line, out ln, out lineText);

                    long n = (long)1e9 * statement.tokens[i7].line + statement.tokens[i7].column;
                    ErrorsAddOrMerge(errors, n, statement.parenthesisErrors[i7]);
                }
            }

            foreach (string ss7 in lexerAndParserErrors7.parserErrors)
            {
                int lineNumber, col;
                string errorMessage, fileName;
                ParserGekCreateAST.ExtractParserErrorLineAndPos(ss7, ph7.fileName, out lineNumber, out col, out errorMessage, out fileName);

                int ln; string lineText;
                AdjustLine(ph, statement.tokens[0].line, lineNumber, out ln, out lineText);                

                bool show = false;
                if (errorStatements == int.MaxValue) show = true;

                long n = (long)1e9 * ln + col;
                ErrorHelper errorsn = null; errors.TryGetValue(n, out errorsn);
                if (errorsn != null)
                {
                    errorsn.errors.Add(errorMessage);
                }
                else
                {
                    ErrorHelper e = new ErrorHelper();
                    e.errors = new List<string>() { errorMessage };
                    //e.oneLineOfText = lineText;
                    errors.Add(n, e);
                }
            }

            return errors;
            
        }

        private static void ErrorsAddOrMerge(SortedDictionary<long, ErrorHelper> errors, long n, List<string> extra)
        {
            ErrorHelper errorsn = null; errors.TryGetValue(n, out errorsn);
            if (errorsn != null)
            {
                errorsn.errors.AddRange(extra);
            }
            else
            {
                ErrorHelper e = new ErrorHelper();
                e.errors = new List<string>();
                e.errors.AddRange(extra);
                errors.Add(n, e);
            }
        }

        private static void AdjustLine(ParseHelper ph, int line0, int lineNumber, out int ln, out string lineText)
        {
            List<string> text = Stringlist.ExtractLinesFromText(ph.commandsText);
            ln = line0 + lineNumber - 1;
            if (false)
            {
                //this is not good here. Probably something from the old error messages.
                if (ph.isOneLinerFromGui) ln += -1;
            }
            //lineText = "[" + ln.ToString() + "]: " + text[ln - 1];
            lineText = text[ln - 1];
        }

        private static List<Statement> GetStatements(ParseHelper ph)
        {
            List<Statement> statements = new List<Statement>();
            string txt = ph.commandsText;
            var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
            var tags2 = new List<string>() { "//" };
            //TokenHelper tokens2 = StringTokenizer.GetTokensWithLeftBlanksRecursive(txt, tags1, tags2, null, null);                    
            TokenList tokens2 = StringTokenizer.GetTokensWithLeftBlanks(txt, 0, tags1, tags2, null, null);
            StringBuilder rv = new StringBuilder();

            int n_paren = 0; int n_bracket = 0; int n_curly = 0;
            List<string> comments = new List<string>();
            Statement statement = new Statement();

            statement.type = 2;
            int i = -1;
            bool isInsideOptionField = false;
            int iFirstWord = -12345;

            foreach (TokenHelper tok in tokens2.storage)
            {
                i++;
                if (iFirstWord == -12345 && tok.type == ETokenType.Word) iFirstWord = i;
                if (tok.s == "(")
                {
                    n_paren++;
                    statement.paren_parentheses.Add(n_paren);
                }
                else if (tok.s == ")")
                {
                    n_paren--;
                    statement.paren_parentheses.Add(n_paren);
                }
                else
                {
                    statement.paren_parentheses.Add(null);
                }

                if (tok.s == "[")
                {
                    n_bracket++;
                    statement.paren_brackets.Add(n_bracket);
                }
                else if (tok.s == "]")
                {
                    n_bracket--;
                    statement.paren_brackets.Add(n_bracket);
                }
                else
                {
                    statement.paren_brackets.Add(null);
                }

                if (tok.s == "{")
                {
                    n_curly++;
                    statement.paren_curlies.Add(n_curly);
                }
                else if (tok.s == "}")
                {
                    n_curly--;
                    statement.paren_curlies.Add(n_curly);
                }
                else
                {
                    statement.paren_curlies.Add(null);
                }

                if (tok.s == "<")
                {
                    if (i == iFirstWord + 1) isInsideOptionField = true;  //will not handle /* ... */ in between
                }
                else if (tok.s == ">")
                {
                    //will not handle a logical < or > inside an option field, but these do not exist anyway
                    isInsideOptionField = false;
                }

                if (tok.s == "=" && tok.SiblingAfter(1, true) != null && tok.SiblingAfter(1, true).s != "=")
                {
                    if (n_paren == 0 && n_bracket == 0 && n_curly == 0 && !isInsideOptionField) statement.type = 1;
                }

                statement.tokens.Add(tok);

                if (tok.s == ";")  // && (n_paren == 0 && n_bracket == 0 && n_curly == 0))
                {
                    //Next statement. We make sure that for instance #m = [1, 2; 3, 4]; does not break into two.                                                        
                    statement.text = StringTokenizer.GetTextFromLeftBlanksTokens(statement.tokens, true);
                    if (isInsideOptionField) statement.parenthesisErrors2.Add("Unclosed <> option field");
                    statements.Add(statement);
                    //
                    //reset
                    //
                    n_paren = 0; n_bracket = 0; n_curly = 0;  //reset 
                    isInsideOptionField = false;
                    iFirstWord = -12345;
                    statement = new Statement();  //ready for the next
                }
            }
            return statements;
        }
    }
}
