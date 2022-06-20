using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System.Drawing;

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
        //public string oneLineOfText = null;
        //public string start = null;        
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
        public SortedDictionary<long, ErrorHelper> errors = null;
    }

    /// <summary>
    /// Tries to give resonable error messages
    /// </summary>
    public class ParserGekErrors
    {
        /// <summary>
        /// Gets error messages statement by statement, so that these do not get mixed up. Skips "end;" statements,
        /// and adds "end;" on for/if/block/procedure/function. For eatch statement, the parser is called individually.
        /// </summary>
        /// <param name="ph"></param>
        /// <param name="parseOutput"></param>
        /// <param name="textWithExtraLines"></param>
        /// <param name="t"></param>
        /// <param name="errorStatements"></param>
        public static void ErrorMessages(ParseHelper ph, ref ConvertHelper parseOutput, ref string textWithExtraLines, ref CommonTree t, int errorStatements)
        {
            int numberOfErroneousStatementsShownInDetail = 100;
            bool showLetters = true;
            bool condense = true;
            if (!showLetters) condense = true;

            List<string> originalText = Stringlist.ExtractLinesFromText(ph.commandsText);

            List<Statement> statements = GetStatements(ph);

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

                ConvertHelper parseOutput7; string textWithExtraLines7; CommonTree t7;
                LexerAndParserErrors lexerAndParserErrors7 = ParserGekCreateAST.ParseAndSyntaxErrors(out parseOutput, out textWithExtraLines, out t, ph7);

                if (lexerAndParserErrors7.parserErrors != null && lexerAndParserErrors7.parserErrors.Count > 0)
                {
                    statement.errors = GetErrorsFromOneStatement(ph, errorStatements, statement, ph7, lexerAndParserErrors7);                    
                }
            } //end loop over statements

            //TODO infinite line length + show both on screen and in pipe
            //TODO infinite line length + show both on screen and in pipe
            //TODO infinite line length + show both on screen and in pipe
            //TODO infinite line length + show both on screen and in pipe
            //TODO infinite line length + show both on screen and in pipe
            G.Writeln2("*** ERROR: Syntax errors encountered in file '" + ph.fileName + "'");
            int counter = 0;
            foreach (Statement statement in statements)
            {                          
                if (statement.errors != null)
                {
                    counter++;
                    if (counter > numberOfErroneousStatementsShownInDetail) break;
                    SortedDictionary<int, string> split = new SortedDictionary<int, string>();  //relevant lines for multi-line errors
                    foreach (KeyValuePair<long, ErrorHelper> kvp in statement.errors)
                    {
                        int line = (int)(kvp.Key / (long)1e9);
                        if (!split.ContainsKey(line)) split.Add(line, null);
                    }

                    foreach (int line2 in split.Keys)
                    {                        
                        string start = "[" + line2 + "]: ";
                        string start2 = G.Blanks(start.Length);
                        G.Writeln();
                        G.Writeln(start + originalText[line2 - 1]);
                        int cOld = 0; int errorCounter = 0;
                        string s1 = start2;
                        string s2 = start2;
                        foreach (KeyValuePair<long, ErrorHelper> kvp in statement.errors)
                        {                            
                            int ln = (int)(kvp.Key / (long)1e9);
                            int col = (int)(kvp.Key % (long)1e9);
                            if (ln != line2) continue;
                            errorCounter++;
                            char letter = (char)(97 + errorCounter - 1);
                            int c = col - 1 - cOld;                            
                            s1 += G.Blanks(c) + "^";
                            s2 += G.Blanks(c) + letter;
                            cOld = col;
                        }
                        G.Writeln(s1, Color.Red);
                        if (showLetters) G.Writeln(s2, Color.Red);
                        G.Writeln();
                        errorCounter = 0;
                        foreach (KeyValuePair<long, ErrorHelper> kvp in statement.errors)
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
                                foreach (string s in kvp.Value.errors)
                                {
                                    error += G.FirstCharToUpper(s) + ". ";
                                }
                                error = error.Substring(0, error.Length - ". ".Length);
                                if (showLetters) s3 = "(" + letter + "): " + G.FirstCharToUpper(error);
                                else s3 = "(^): " + G.FirstCharToUpper(error);
                                if (!s3.EndsWith(".")) s3 = s3 + ".";
                                G.Writeln(s3, Color.Red);
                            }
                            else
                            {
                                foreach (string s in kvp.Value.errors)
                                {                                    
                                    string s3 = null;
                                    if (showLetters) s3 = "(" + letter + "): " + G.FirstCharToUpper(s);
                                    else s3 = "(^): " + G.FirstCharToUpper(s);
                                    if (!s3.EndsWith(".")) s3 = s3 + ".";
                                    G.Writeln(s3, Color.Red);
                                }
                            }
                        }                        
                    }
                    foreach (string s8 in statement.parenthesisErrors2)
                    {
                        G.Writeln("(*): " + s8, Color.Red);
                    }

                    if (true)
                    {
                        G.Writeln();
                        foreach (KeyValuePair<long, ErrorHelper> kvp in statement.errors)
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
            }
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
                    ErrorHelper errorsn = null; errors.TryGetValue(n, out errorsn);
                    if (errorsn != null)
                    {
                        errorsn.errors.AddRange(statement.parenthesisErrors[i7]);
                    }
                    else
                    {
                        ErrorHelper e = new ErrorHelper();
                        e.errors = statement.parenthesisErrors[i7];
                        //e.oneLineOfText = lineText;
                        errors.Add(n, e);
                    }
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

        private static void AdjustLine(ParseHelper ph, int line0, int lineNumber, out int ln, out string lineText)
        {
            List<string> text = Stringlist.ExtractLinesFromText(ph.commandsText);
            ln = line0 + lineNumber - 1;
            if (ph.isOneLinerFromGui) ln += -1;
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
