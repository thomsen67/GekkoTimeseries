using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace Gekko.Parser.Gek
{
    public class LexerAndParserErrors
    {
        public List<string> lexerErrors = null;
        public List<string> parserErrors = null;
    }

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
    }

    public class ParserGekErrors
    {
        public static void ErrorMessages(ParseHelper ph, ref ConvertHelper parseOutput, ref string textWithExtraLines, ref CommonTree t, int errorStatements)
        {
            List<Statement> statements = new List<Statement>();

            if (true)
            {
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
            }

            for (int ii = 0; ii < statements.Count; ii++)
            {
                Statement statement = statements[ii];

                bool startFor = false;
                bool startIf = false;
                ParseHelper ph7 = ph.Clone();
                ph7.isDebugMode = true;

                string s7 = statement.text.Trim();
                string s7a = s7;

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

                if (G.Equal(firstWord.s, "for"))
                {
                    startFor = true;
                    s7 += "end;";
                }
                else if (G.Equal(firstWord.s, "if"))
                {
                    startIf = true;
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

                    SortedDictionary<long, List<string>> errors = new SortedDictionary<long, List<string>>();

                    for (int i7 = 0; i7 < statement.tokens.Count; i7++)
                    {
                        if (statement.parenthesisErrors[i7].Count > 0)
                        {
                            long n = (long)1e9 * statement.tokens[i7].line + statement.tokens[i7].column;
                            List<string> errorsn = null; errors.TryGetValue(n, out errorsn);
                            if (errorsn != null) errorsn.AddRange(statement.parenthesisErrors[i7]);
                            else errors.Add(n, statement.parenthesisErrors[i7]);
                        }
                    }

                    foreach (string ss7 in lexerAndParserErrors7.parserErrors)
                    {
                        int lineNumber, col;
                        string errorMessage, fileName;
                        ParserGekCreateAST.ExtractParserErrorLineAndPos(ss7, ph7.fileName, out lineNumber, out col, out errorMessage, out fileName);

                        List<string> text = Stringlist.ExtractLinesFromText(ph.commandsText);
                        int ln = statement.tokens[0].line + lineNumber - 1 + 1;
                        if (ph.isOneLinerFromGui) ln += -1;
                        string lineText = text[ln - 1];
                        string lineString = "[" + ln.ToString() + "]: ";

                        bool show = false;
                        if (errorStatements == int.MaxValue) show = true;

                        long n = (long)1e9 * ln + col;
                        List<string> errorsn = null; errors.TryGetValue(n, out errorsn);
                        if (errorsn != null) errorsn.Add(errorMessage);
                        else errors.Add(n, new List<string>() { errorMessage });

                        //new Writeln(s7a + " --> type " + ph7.syntaxType + " fn " + fileName + " line " + lineNumber + " pos " + positionNumber + " error: " + errorMessage);

                    }

                    //G.Writeln("");
                    //G.Writeln(errorMessage, Color.Red, true);
                    //G.Writeln(lineString + lineText, Color.Red, true);
                    //G.Writeln(G.Blanks(lineString.Length) + G.Blanks(col - 1) + "^", Color.Red, true);

                    G.Writeln();
                    foreach (KeyValuePair<long, List<string>> kvp in errors)
                    {
                        int line = (int)(kvp.Key / (long)1e9);
                        int pos = (int)(kvp.Key % (long)1e9);
                        foreach (string s8 in kvp.Value)
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
}
