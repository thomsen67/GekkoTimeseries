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
        public ParserGekCreateAST.EParserType type = ParserGekCreateAST.EParserType.OnlyAssignment;  //Set to OnlyAssignment to start out because it is hardest to determine
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

                int extras = 5;  //easier to do "leads"
                List<TokenHelper> m = new List<TokenHelper>();
                foreach (TokenHelper th in statement.tokens)
                {
                    //ignores any "noise" from syntax
                    if (th.type == ETokenType.Word || th.type == ETokenType.Number || th.type == ETokenType.QuotedString || th.type == ETokenType.Symbol) m.Add(th);                    
                }
                for (int i = 0; i < extras; i++) m.Add(new TokenHelper(""));                                                
                
                long    start = (long)1e9 * statement.tokens[0].line + statement.tokens[0].column;
                long    end = (long)1e9 * statement.tokens[statement.tokens.Count - 1].line + statement.tokens[statement.tokens.Count - 1].column;                                

                bool hasSigil = m[0].s == Globals.symbolScalar.ToString() || m[0].s == Globals.symbolCollection.ToString();
                if (!hasSigil)
                {
                    //We are trying to find commands here, to distinguish them from assignments.
                    //function and procedure calls like f(...), p ..., m.f(...), m.p ... are 
                    //dealt with already, so below we try to identify normal commands like "prt ...", "read ..." etc.
                    //because else they become assignments.
                    //
                    //Default is assignment, because it is most difficult to determine.
                    //
                    //We must guard against the firstWord being for instance "global" (command name) but it is a bank or something,
                    //like global:%x = ... etc.
                    //
                    // ------------------------------------------------------
                    // THE FOLLOWING ARE ASSIGNMENTS and should be removed
                    // even if they start with a command name like for
                    // instance "global".
                    // ------------------------------------------------------
                    // Maybe with blanks
                    // ------------------------------------
                    // :             global : x, global:x
                    // =             global = x, global=x
                    // $             global $ x, global$x
                    // .             global . x, global.x  (last: global£.x)
                    // !             global ! x, global!x  (last: global¨!¨x)   
                    // |             global | x, global|x  (last: global¨|x)
                    // +             global +=, global+=                       1 for these 5 also check =
                    // -=            global -=, global-=                       2
                    // *=            global *=, global*=   (last: global½*=)   3
                    // /=            global /=, global/=                       4
                    // ^=            global ^=, global/=                       5
                    // <             global <, global<      only if first > is followed by = (in principle also += etc but oh well...)
                    // ------------------------------------
                    // No blanks
                    // ------------------------------------                              
                    // #             global#x               (global¨#¨x)              
                    // %             global%x               (global¨%¨x)
                    // {             global{x               (global¨{x)
                    // [             global[x               (global[_[x)
                    // ------------------------------------                  

                    bool seemsAssignment = false;
                    if (m[0].type == ETokenType.Word)
                    {
                        if (
                            (m[1].s == ":") ||
                            (m[1].s == "=") ||
                            (m[1].s == "$") ||
                            (m[1].s == ".") ||
                            (m[1].s == "£" && m[1].leftblanks == 0 && m[2].s == "." && m[2].leftblanks == 0) ||
                            (m[1].s == "!") ||
                            (m[1].s == "¨" && m[1].leftblanks == 0 && m[2].s == "!" && m[2].leftblanks == 0) ||
                            (m[1].s == "|") ||
                            (m[1].s == "¨" && m[1].leftblanks == 0 && m[2].s == "|" && m[2].leftblanks == 0) ||
                            (m[1].s == "+" && m[2].s == "=" && m[2].leftblanks == 0) ||
                            (m[1].s == "-" && m[2].s == "=" && m[2].leftblanks == 0) ||
                            (m[1].s == "*" && m[2].s == "=" && m[2].leftblanks == 0) || // global *
                            (m[1].s == "½" && m[1].leftblanks == 0 && m[2].s == "*" && m[2].leftblanks == 0) || // global*
                            (m[1].s == "/" && m[2].s == "=" && m[2].leftblanks == 0) ||
                            (m[1].s == "^" && m[2].s == "=" && m[2].leftblanks == 0) ||                            
                            (m[1].s == "<" && HasLargerThanAndEqual(m)) ||              // identifies global <2001 2002> = 100; for instance
                            (m[1].s == "¨" && m[1].leftblanks == 0 && m[2].s == "%" && m[2].leftblanks == 0) ||
                            (m[1].s == "¨" && m[1].leftblanks == 0 && m[2].s == "#" && m[2].leftblanks == 0) ||
                            (m[1].s == "¨" && m[1].leftblanks == 0 && m[2].s == "{" && m[2].leftblanks == 0) ||
                            (m[1].s == "[" && m[1].leftblanks == 0 && m[2].s == "_" && m[2].leftblanks == 0 && m[3].s == "[" && m[3].leftblanks == 0)
                        ) seemsAssignment = true;
                    }
                    if (statement.type == ParserGekCreateAST.EParserType.OnlyProcedureCallEtc || seemsAssignment)
                    {
                        //always keep type as it is
                    }
                    else
                    {
                        //may be a statement
                        if (Globals.commandNames.Contains(m[0].s.ToUpper())) statement.type = ParserGekCreateAST.EParserType.Normal;
                    }

                    if (G.Equal(m[0].s, "end") && m[1].s == ";") continue;

                    if (G.Equal(m[0].s, "else") && m[1].s == ";") continue;

                    if (G.Equal(m[0].s, "for") || G.Equal(m[0].s, "if") || G.Equal(m[0].s, "block") || G.Equal(m[0].s, "function") || G.Equal(m[0].s, "procedure"))
                    {
                        s7 += "end;";  //to get it to parse
                    }
                }

                ph7.commandsText = s7;
                ph7.syntaxType = statement.type;

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
                                        s = ParserGekCreateAST.ReplaceTokenNamesWithMeaningfulStrings(s);
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
                                        s = ParserGekCreateAST.ReplaceTokenNamesWithMeaningfulStrings(s);
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

                        string h = statement.text.Trim();

                        if (statement.type == ParserGekCreateAST.EParserType.OnlyAssignment)
                        {
                            h = "var";
                        }
                        else if (statement.type == ParserGekCreateAST.EParserType.OnlyProcedureCallEtc)
                        {
                            h = "";  //could point to help file regarding procedure or function, but typically it is just a question of parentheses etc. that do not match
                        }

                        string help = ParserGekCreateAST.GetLinkToHelpFile2(h);

                        try
                        {                            

                            //we actually already have tokens, but not recursive tokens, so we do them here.
                            string ss = G.ReplaceGlueSymbols(helper.textWithExtraLines);
                            var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
                            var tags2 = new List<string>() { "//" };
                            TokenHelper tokens2 = StringTokenizer.GetTokensWithLeftBlanksRecursive(statement.text, tags1, tags2, null, null);

                            List<TokenHelper> list = tokens2.subnodes.storage;

                            int iStart = -1;
                            for (int i = 0; i < tokens2.subnodes.storage.Count; i++)
                            {
                                if (tokens2.subnodes.storage[i].s == ";")
                                {
                                    List<TokenHelper> listTemp = list.GetRange(iStart + 1, i - iStart);
                                    ErrorMessagesHelperNakedList(listTemp, i, widthRemember);
                                    iStart = i;
                                }
                            }
                        }
                        catch { }

                        try
                        {
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
                        }
                        catch { }

                        string gekko2_4 = null;
                        try
                        {
                            string code = statement.text;
                            bool is2_4 = ParserGekCreateAST.IsValid2_4Syntax(code);
                            Translate_2_4_to_3_0.Info info = new Translate_2_4_to_3_0.Info();
                            string translated = Translate_2_4_to_3_0.Translate(code, info);
                            bool is3_0 = ParserGekCreateAST.IsValid3_0Syntax(translated);
                            bool equal = code.ToLower().Replace(" ", "") == translated.ToLower().Replace(" ", ""); //ignore case and whitespace
                            if (is2_4 && !equal)
                            {
                                using (Note txt = new Note())
                                {
                                    txt.MainAdd("The statement is valid Gekko 2.x syntax, and Gekko has translated it: see the link.");
                                    if (is3_0)
                                    {
                                        txt.MoreAdd("Gekko has {a{translated¤translate.htm}a} the statement into the following valid Gekko 3.x code:");
                                        txt.MoreNewLine();
                                        txt.MoreAdd(translated);
                                    }
                                    else
                                    {
                                        txt.MoreAdd("The in-built {a{translator¤translate.htm}a} tried to translate the statement, but could not produce valid 3.x code.");
                                        txt.MoreNewLine();
                                        txt.MoreAdd("The translated code is the following (the code is invalid but may provide some hints):");
                                        txt.MoreNewLine();
                                        txt.MoreAdd(translated);
                                    }
                                    txt.MoreNewLine();
                                    txt.MoreAdd("Note that automatically translated code is not guaranteed to replicate the original code.");
                                }
                            }
                        }
                        catch { };

                        //if (Globals.runningOnTTComputer) new Writeln("TTH --> TYPE: " + statement.type);

                        if (CountErrors(statement.errorDictionary) != CountErrors(originalErrors))
                        {
                            if (Globals.runningOnTTComputer) new Error("Dict mismatch");
                        }

                        if (ph.isOneLinerFromGui) WritelnError("", true);
                        
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

        /// <summary>
        /// Searches for the ">=" in a line like x &lt;2001 2005>= 100 ; to distinguish this
        /// from being a command like PRT &lt;2001 2005> x file = out.txt; In the latter, the "=" is
        /// separated from the ">".
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static bool HasLargerThanAndEqual(List<TokenHelper> m)
        {
            bool b = false;
            int found = -12345;
            for (int i = 0; i < m.Count; i++)
            {
                if (m[i].s == ">")
                {
                    found = i; //first one
                    break;
                }
            }
            if (found != -12345 && m[found + 1].s == "=") return true;
            return b;
        }

        /// <summary>
        /// Pattern "list #m = ... , .... ;" or "#m = ... , .... ;" or "for string ... = ... , .... ;" or "for (string ... = ... , .... ;" . In that case, a note is written if there are % or # variables without {}-curlies.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="iStart"></param>
        /// <param name="widthRemember"></param>
        private static void ErrorMessagesHelperNakedList(List<TokenHelper> list, int iStart, int widthRemember)
        {
            int commaCount = 0;
            int equalCount = 0;
            bool isLhsList = false;
            bool isLhsFor = false;
            bool hashProblem = false;
            bool percentProblem = false;
            bool hasParenthesis = false;

            if (list.Count >= 2 && G.Equal(list[0].s, "for") && list[1].subnodes != null && list[1].subnodes.storage[0].s == "(")
            {
                list = list[1].subnodes.storage.GetRange(1, list[1].subnodes.storage.Count - 2);   //inside a for(...), includes start/end parentheses
                isLhsFor = true;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].s == ",") commaCount++; //does not count commas inside in parentheses                                
                if (list[i].s == "=") equalCount++; //does not count = inside in parentheses                                
            }

            if (list.Count >= 2 && G.Equal(list[0].s, "list") && G.Equal(list[1].s, "#")) isLhsList = true;
            if (list.Count >= 1 && G.Equal(list[0].s, "#")) isLhsList = true;

            if (list.Count >= 2 && G.Equal(list[0].s, "for") && G.Equal(list[1].s, "string")) isLhsFor = true;
                        
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i].s == "=" && list[i + 1].s == "%") percentProblem = true;
                if (list[i].s == "," && list[i + 1].s == "%") percentProblem = true;
                if (list[i].s == "=" && list[i + 1].s == "#") hashProblem = true;
                if (list[i].s == "," && list[i + 1].s == "#") hashProblem = true;
                if (list[i].s == "=" && list[i + 1].s == "(") hasParenthesis = true;
            }

            if (equalCount == 1 && commaCount > 0 && (isLhsList || isLhsFor) && !hasParenthesis && (percentProblem || hashProblem))
            {
                int widthRemember2 = Program.options.print_width; //just to set a normal width here
                Program.options.print_width = widthRemember;
                try
                {
                    using (Writeln txt = new Writeln("+++ ", -12345, Color.Red, true, ETabs.Main))
                    {
                        string x = "#y";
                        if (isLhsFor) x = "for string %y";
                        txt.MainAdd("Note that in a parenthesis-free {a{naked list¤i_naked_list.htm}a}, a string %x or list of strings #x must be enclosed in {}-curlies, for instance: " + x + " = a, {%x}, {#x}, b;.");
                    }
                }
                finally
                {
                    Program.options.print_width = widthRemember2;
                }
            }

            return;
        }

        private static string HandleGlueSymbols(Statement statement, string statementLine, string statementLine2, StringBuilder statementLine3)
        {
            List<string> glues1 = new List<string>() { Globals.symbolGlueChar2, Globals.symbolGlueChar3, Globals.symbolGlueChar4, Globals.symbolGlueChar5, Globals.symbolGlueChar6, Globals.symbolGlueChar7, Globals.symbolGlueChar1.ToString()};            
            //int[] glue = new int[statementLine.Length];  //1 means it is being removed
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

                    //for instance, the statement "#¨m = list¨('a',);"
                    //has "¨" in pos 2 and 11 (string starts at 1).
                    //errorDict has 100..0011, so only an error at the last "¨"
                    //when we meet the first "¨" at col2=2, all errors with col >= 3 should be moved 1 to the left.
                    //when we meet the ssecont "¨" at col2=11, all errors with col >= 12 should be moved 1 to the left.
                    //So move them when col >= col2+1, that is, no move when col < col2+1.

                    //move pointers 1 pos to the left 
                    foreach (KeyValuePair<long, ErrorHelper> kvp in errorDict2)
                    {
                        int ln = (int)(kvp.Key / (long)1e9);
                        int col = (int)(kvp.Key % (long)1e9);
                        if (col < col2 + 1) continue;  //only move after '£'                        
                        kvp.Value.offset -= 1;
                    }
                }
                else
                {
                    sb.Append(statementLine[i]);
                }
            }
            
            statement.errorDictionary = errorDict2;
            statementLine = sb.ToString();
            statement.text = statementLine;

            //Replace any '¨' in the error text with a suitable character from the statement line (first non-blank character after the '¨').
            foreach (KeyValuePair<long, ErrorHelper> kvp in statement.errorDictionary)
            {
                for (int i = 0; i < kvp.Value.errors.Count; i++)
                {
                    if (kvp.Value.errors[i].Contains("¨"))
                    {
                        int col = (int)(kvp.Key % (long)1e9);
                        col += kvp.Value.offset;
                        if (col < statementLine.Length)
                        {
                            char c = '¨';
                            for (int ii = col - 1; ii < statementLine.Length; ii++)
                            {
                                if (statementLine[ii] != ' ')
                                {
                                    c = statementLine[ii];
                                    break;
                                }
                            }                            
                            kvp.Value.errors[i] = kvp.Value.errors[i].Replace("'¨'", "'" + c + "'");
                        }
                    }
                }                
            }

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
            if (last_parenthesis > 0) statement.parenthesisErrors2.Add("Too many '(' in statement");
            if (last_bracket > 0) statement.parenthesisErrors2.Add("Too many '[' in statement");
            if (last_curly > 0) statement.parenthesisErrors2.Add("Too many '{' in statement");

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
            
            bool isInsideOptionField = false;
            int iFirstWord = -12345;

            //foreach (TokenHelper tok in tokens2.storage)
            for(int i = 0;i<tokens2.storage.Count;i++)
            {
                TokenHelper tok = tokens2.storage[i];
            
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

                string x1 = null; string x2 = null;
                try
                {
                    x1 = tokens2.storage[i - 1].s;
                    x2 = tokens2.storage[i - 2].s;
                }
                catch { };

                if (tok.s == "[" && !((x1 == "_" || x1 == "¨") && x2 == "["))  //do not count the second bracket in a "[_[" or a "[¨[".
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

                if (tok.s == "<")  //if we have "<=<" glue symbols, with tokens "<", "=", "<", the second "<" cannot turn on isInsideOptionField (and even if it did: ok)
                {
                    if (i == iFirstWord + 1) isInsideOptionField = true;  //will not handle /* ... */ in between
                }
                else if (tok.s == ">")
                {
                    //will not handle a logical < or > inside an option field, but these do not exist anyway
                    isInsideOptionField = false;
                }                

                statement.tokens.Add(tok);

                if (tok.s == ";")  // && (n_paren == 0 && n_bracket == 0 && n_curly == 0))
                {
                    //Next statement. We make sure that for instance #m = [1, 2; 3, 4]; does not break into two.                                                        
                    statement.text = StringTokenizer.GetTextFromLeftBlanksTokens(statement.tokens, true);
                    if (isInsideOptionField) statement.parenthesisErrors2.Add("Unclosed <> option field");

                    if (true)
                    {

                        int j1 = -12345; string s1 = StringTokenizer.GetFirstTokenReal(statement.tokens, out j1);
                        int j2 = -12345; string s2 = null; if (s1 != null) StringTokenizer.OffsetTokensRightReal(statement.tokens, j1, 1, out j2);
                        int j3 = -12345; string s3 = null; if (s2 != null) StringTokenizer.OffsetTokensRightReal(statement.tokens, j2, 1, out j3);
                        int j4 = -12345; string s4 = null; if (s3 != null) StringTokenizer.OffsetTokensRightReal(statement.tokens, j3, 1, out j4);
                        int j5 = -12345; string s5 = null; if (s4 != null) StringTokenizer.OffsetTokensRightReal(statement.tokens, j4, 1, out j5);

                        bool flag = false;

                        if (j1 != -12345 && j2 != -12345 && j3 != -12345 && statement.tokens[j1].type == ETokenType.Word && statement.tokens[j2].s == Globals.symbolGlueChar1.ToString() && statement.tokens[j3].s == "(")
                        {
                            //f¨(
                            if (!Globals.leftSideFunctions.Contains(s1.ToLower())) flag = true;
                        }
                        else if (j1 != -12345 && j2 != -12345 && j3 != -12345 && j4 != -12345 && j5 != -12345 && statement.tokens[j1].type == ETokenType.Word && statement.tokens[j2].s == ":" && statement.tokens[j3].type == ETokenType.Word && statement.tokens[j4].s == Globals.symbolGlueChar1.ToString() && statement.tokens[j5].s == "(")
                        {
                            //b:f¨(
                            flag = true;
                        }
                        else if (j1 != -12345 && statement.tokens[j1].type == ETokenType.Word)
                        {
                            //f
                            if (!Globals.commandNames.Contains(s1.ToUpper())) flag = true;
                        }
                        else if (j1 != -12345 && j2 != -12345 && j3 != -12345 && statement.tokens[j1].type == ETokenType.Word && statement.tokens[j2].s == ":" && statement.tokens[j3].type == ETokenType.Word)
                        {
                            //b:f                            
                            flag = true;
                        }

                        int eq = StringTokenizer.FindS(statement.tokens, "=");

                        if (flag && eq == -12345) statement.type = ParserGekCreateAST.EParserType.OnlyProcedureCallEtc;
                                                
                    }

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
