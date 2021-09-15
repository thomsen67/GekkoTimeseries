using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    
    class Translator_AREMOS_Gekko30
    {
        public static GekkoDictionary<string, string> listMemory = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public static GekkoDictionary<string, string> matrixMemory = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public static GekkoDictionary<string, string> scalarMemory = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        //This class translates from AREMOS to Gekko 3.0

        public static string Translate(string input)
        {

            listMemory.Clear();
            matrixMemory.Clear();
            scalarMemory.Clear();

            string txt = input;            
            var tags2 = new List<string>() { "!" };

            TokenHelper tokens2 = StringTokenizer.GetTokensWithLeftBlanksRecursive(txt, null, tags2, null, null);
            
            int counter = 0;

            StringBuilder rv = new StringBuilder();
            List<List<TokenHelper>> statements2 = new List<List<TokenHelper>>();
            List<TokenHelper> statement = new List<TokenHelper>();
            foreach (TokenHelper tok in tokens2.subnodes.storage)
            {
                statement.Add(tok);
                if (tok.s == ";")
                {
                    statements2.Add(statement);
                    statement = new List<TokenHelper>();
                }                
            }
            statements2.Add(statement);

            List<List<TokenHelper>> statements = new List<List<TokenHelper>>();
            foreach (List<TokenHelper> line in statements2)
            {
                if (LineStartsWithWord(line))
                {
                    statements.Add(line);
                }
                else
                {
                    int iStop = -12345;
                    List<TokenHelper> line2 = new List<TokenHelper>();
                    for (int i = 0; i < line.Count; i++)
                    {
                        if (GetType(line, i) == ETokenType.Word)
                        {
                            iStop = i;
                            break;
                        }
                        line2.Add(line[i]);
                    }
                    statements.Add(line2);

                    if (iStop == -12345)
                    {
                        //end of file
                    }
                    else
                    {
                        List<TokenHelper> line3 = new List<TokenHelper>();
                        for (int i = iStop; i < line.Count; i++)
                        {
                            line3.Add(line[i]);
                        }
                        statements.Add(line3);
                    }
                }
            }

            List<List<TokenHelper>> temp = new List<List<TokenHelper>>();

            foreach (List<TokenHelper> line in statements)
            {
                if (Equal(line, 0, "else"))
                {
                    int ii = 1;
                    List<TokenHelper> line2 = new List<TokenHelper>();
                    line2.Add(line[0]);
                    line2.Add(new TokenHelper(0, ";", ETokenType.Symbol));
                    if (GetS(line, 1) == "\r\n")
                    {
                        line2.Add(line[1]); ii++;
                    }                    
                    temp.Add(line2);

                    List<TokenHelper> line3 = new List<TokenHelper>();
                    for (int i = ii; i < line.Count; i++)
                    {
                        //if (IsEmptyToken(line, i)) continue;  //skip blank tokens
                        line3.Add(line[i]);
                    }
                    if (line3[line3.Count - 1].s != ";") line3.Add(new TokenHelper(";"));
                    temp.Add(line3);
                }
                else
                {
                    temp.Add(line);
                }
            }

            //TODO
            //TODO
            //TODO
            //TODO
            //TODO Maybe we need to reorganize with OrganizeSubnodes(), cf. #807508925428903
            //TODO
            //TODO
            //TODO
            //TODO
            //TODO

            foreach (List<TokenHelper> line in temp)
            {
                //Takes care of the first part of the line,
                //option field etc.
                //Records names of assigns, lists and matrices
                HandleExpressionsRecursiveBefore(line);
                HandleCommandName(line);
                HandleExpressionsRecursive(line, line);
            }            

            foreach (List<TokenHelper> line in temp)
            {
                StringBuilder sb = new StringBuilder();
                foreach (TokenHelper tok in line)
                {
                    sb.Append(tok.ToString());
                }
                rv.Append(sb);
            }

            return rv.ToString();
        }

        private static void SetLineStartRecursive(List<TokenHelper> line, List<TokenHelper> pointer)
        {
            for (int i = 0; i < line.Count; i++)
            {
                if (line[i].HasChildren())
                {
                    SetLineStartRecursive(line[i].subnodes.storage, pointer);
                    continue;
                }
                line[i].meta.commandLine = pointer;
            }
        }

        public static void HandleExpressionsRecursiveBefore(List<TokenHelper> line)
        {
            for (int i = 0; i < line.Count; i++)
            {
                if (line[i].HasChildren())
                {
                    HandleExpressionsRecursiveBefore(line[i].subnodes.storage);
                    continue;
                }

                if (Equal(line, i - 1, "listfile") && GetType(line, i) == ETokenType.Word)
                {
                    // listfile a --> (listfile a), where a has a blank after
                    //   -1     0
                    if (!Equal(line, i - 2, "("))
                    {
                        line[i - 1].leftblanks = 0;  //no blanks before 'listfile'
                        line.Insert(i - 1, new TokenHelper(0, "(", ETokenType.Symbol));
                        line.Insert(i - 1, new TokenHelper(1, "#", ETokenType.Symbol));
                        line.Insert(i + 3, new TokenHelper(0, ")", ETokenType.Symbol));
                    }
                }

            }
        }

        public static void HandleExpressionsRecursive(List<TokenHelper> line, List<TokenHelper> topline)
        {
            for (int i = 0; i < line.Count; i++)
            {
                if (line[i].HasChildren())
                {
                    HandleExpressionsRecursive(line[i].subnodes.storage, topline);
                    continue;
                }

                //bool isSeries = G.Equal(line[0].s, FromTo("ser", "series")) != null;

                // ------------- start of real stuff ---------------------------

                
                SetCurliesAroundNakedHash(line, i, topline[0].meta.commandName);
                
                if (GetS(line, i) == "#" && GetLeftblanks(line, i + 1) == 0 && GetType(line, i + 1) == ETokenType.Word)
                {
                    if (!(matrixMemory.ContainsKey(line[i + 1].s) || listMemory.ContainsKey(line[i + 1].s)))
                    {
                        line[i].s = "%";
                    }
                }

                if (Equal(line, i, "average") && topline[0].meta.commandName == "collapse")
                {
                    line[i].s = "avg";
                }

                if (Equal(line, i, "sum") && line[i + 1].SubnodesType() == "(")
                {
                    if (Equal(line[i + 1].subnodes.storage, 1, "0") && Equal(line[i + 1].subnodes.storage, 2, ","))
                    {
                        SetNull(line[i + 1].subnodes.storage, 1);
                        SetNull(line[i + 1].subnodes.storage, 2);
                        line[i + 1].subnodes.storage[3].leftblanks = 0;
                    }
                    //else if (Equal(line[i + 1].subnodes.storage, 1, "#") && line[i + 1].subnodes.storage.Count == 4)
                    //{
                    //    //4 items including ( and ), for instance '#' and 'x' (will probably work with listfile too)
                    //    line[i + 1].subnodes.storage.Insert(1, new TokenHelper(0, "{", ETokenType.Symbol));
                    //    line[i + 1].subnodes.storage.Insert(line[i + 1].subnodes.storage.Count - 1, new TokenHelper(0, "}", ETokenType.Symbol));
                    //}
                }

                if (Equal(line, i, "."))
                {
                    if (Equal(line, i + 1, new List<string>() { "a", "q", "m" }) && GetLeftblanks(line, i + 1) == 0)
                    {
                        //x.q --> x!q
                        line[i].s = "!";
                    }
                }

                if (IsInsideOptionField(line, i) && Equal(line, i, FromTo("pri", "primary")))
                {
                    line[i].s = "edit";
                }

                if (Equal(line, i, FromTo("strip", "strip")))
                {
                    if (topline[0].meta.commandName == "list")
                    {
                        if (GetS(line, i + 1) != "(")
                        {
                            AddComment(topline, "For #m1 = #m2 strip %s, use #m1 = #m2.replaceinside(%s, '', 1)");
                        }
                    }
                }

                if (IsInsideOptionField(line, i) && Equal(line, i, FromTo("pro", "protect")))
                {
                    line[i].s = "";  //all banks are protected, unless <edit> or unlock.
                }
                if (Equal(line, i, "="))
                {
                    if (Equal(line, i - 1, "<")) line[i].leftblanks = 0;  //'<='
                    if (Equal(line, i - 1, "=")) line[i].leftblanks = 0;  //'=='
                    if (Equal(line, i - 1, ">")) line[i].leftblanks = 0;  //'>='  
                }
                if (Equal(line, i, ">"))
                {
                    if (Equal(line, i - 1, ">")) line[i].leftblanks = 0;  //'>'                
                }
                if (Equal(line, i, "repeat"))
                {
                    line[i].s = "rep";
                }

                if (line[i].type == ETokenType.Comment)
                {
                    if (line[i].s.StartsWith("!"))
                    {
                        line[i].s = "//" + line[i].s.Substring(1);
                    }
                }

                //double quotes to single quotes
                if (line[i].s.StartsWith("\"") && line[i].s.EndsWith("\""))
                {
                    line[i].s = "'" + line[i].s.Substring(1, line[i].s.Length - 2) + "'";
                }

                //quotes                
                if (line[i].s.StartsWith("'") && line[i].s.EndsWith("'"))
                {
                    string ss = line[i].s;
                    string ss2 = "";
                    for (int ci = 0; ci < ss.Length; ci++)
                    {
                        if (ss[ci] == '#')
                        {
                            ss2 += '{';
                            ss2 += '%'; ci++;
                            //ss2 += ss[ci]; ci++;
                            for (int cii = ci; cii < ss.Length; cii++)
                            {
                                if (G.IsLetterOrDigitOrUnderscore(ss[cii]))
                                {
                                    //good
                                    ss2 += ss[cii];
                                }
                                else
                                {
                                    ss2 += '}';
                                    ci = cii;
                                    break;
                                }
                            }
                        }
                        ss2 += ss[ci];
                    }
                    line[i].s = ss2;
                }

                if (GetS(line, i) == "+")
                {
                    if (IsToken(line, i - 2) && IsToken(line, i + 2))
                    {
                        if (IsHashVariable(line, i - 2) && IsHashVariable(line, i + 1))
                        {
                            line[i].s = "||";  //#a + #b --> #a || #b
                        }
                    }
                }
                if (GetS(line, i) == "*")
                {
                    if (IsToken(line, i - 2) && IsToken(line, i + 2))
                    {
                        if (IsHashVariable(line, i - 2) && IsHashVariable(line, i + 1))
                        {
                            line[i].s = "&&";  //#a * #b --> #a && #b
                        }
                    }
                }
                if (GetType(line, i) == ETokenType.Word && IsToken(line, i + 1) && line[i + 1].SubnodesType() == "(")
                {
                    // log (x) --> log(x)                    
                    if (KnownFunction(GetS(line, i))) line[i + 1].subnodes[0].leftblanks = 0;
                }
                if (GetS(line, i) == "#" && GetS(line, i + 1) == "#")
                {
                    AddComment(topline, "Note that ##x in Gekko is written %{%x} or #{%x}");
                }

                if (!(GetS(line, i) == "#" || (GetS(line, i) == "%")) && GetType(line, i + 1) == ETokenType.Word)
                {
                    //normal variable/word

                    if (matrixMemory.ContainsKey(line[i + 1].s))
                    {
                        if (!(line[0].s == "set"))
                        {
                            //avoid set freq a --> set freq #a...
                            int lb = line[i + 1].leftblanks;
                            line[i + 1].leftblanks = 0;
                            line.Insert(i + 1, new TokenHelper(lb, "#")); i++;
                        }
                    }
                }

                if (GetS(line, i).Length > 1 && GetS(line, i).EndsWith("."))
                {
                    //Handle '123.' etc.
                    if (G.IsIntegerTranslate(GetS(line, i).Substring(0, GetS(line, i).Length - 1)))
                    {
                        //is like '12345.' or '5.' 
                        line[i].s += "0";
                        //transforms '12345.' into '12345.0'
                        //Gekko does not like these end dots, they interfere with range (..) indicator.
                    }
                }

                if (GetS(line, i) == "|")
                {
                    //ASTNode2 x = node.GetNext();
                    string x = GetS(line, i + 1);
                    if (x != "")
                    {
                        bool hit = false;
                        if (x == "+" || x == "-" || x == "*" || x == "/" || x == "**" || x == "^" || x == ")" || x == "]" || x == "=" || x == ">" || x == "<" || x == "," || x == ":" || x == ";")
                        {
                            SetNull(line, i);
                            //remove it, for instance a%s|*fY or a%s,b%s
                            hit = true;
                        }
                        if (!hit)
                        {
                            x = x = GetS(line, i - 1);
                            if (x != "")
                            {
                                if (x == "+" || x == "-" || x == "*" || x == "/" || x == "**" || x == "^" || x == "(" || x == "[" || x == "=" || x == ">" || x == "<" || x == "," || x == ":" || x == ";")
                                {
                                    SetNull(line, i); ;  //remove it, for instance fy+|%s+...
                                }
                            }
                        }
                    }
                    else
                    {
                        SetNull(line, i); ;  //last token is just before the ';', for instance ...+a%s|;
                    }
                }

                
                
            }
        }

        private static void SetCurliesAroundNakedHash(List<TokenHelper> line, int i, string command)
        {
            if (IsNamePartStart(line, i))
            {
                int i2 = i;
                for (int i1 = i + 1; i1 < line.Count; i1++)
                {
                    if (!IsNamePartMiddle(line, i1))
                    {
                        break;
                    }
                    i2 = i1;
                }
                int tokens = i2 - i + 1;

                bool setCurlies = false;
                if (tokens > 2) setCurlies = true;
                else if (tokens == 1) setCurlies = true;
                else
                {
                    //tokens == 2
                    bool isHashIdent = GetS(line, i) == "#" && GetType(line, i + 1) == ETokenType.Word;
                    List<string> commands = new List<string>();
                    commands.Add("series");
                    commands.Add("open");
                    commands.Add("close");
                    if (!IsInsideOptionField(line, i) && commands.Contains(command))
                    {
                        setCurlies = true;
                    }
                }

                if (setCurlies) //if 2 tokens or more, unless it is these two tokens: '#' + Word 
                {
                    //this is a composed name
                    int iStart = i;
                    int iEnd = i2;
                    string s = "";
                    for (int i1 = iStart; i1 <= iEnd; i1++)
                    {
                        if (GetS(line, i1) == "#" && GetType(line, i1 + 1) == ETokenType.Word)
                        {
                            s += "{%" + GetS(line, i1 + 1) + "}";
                            i1++;
                        }
                        else if (GetS(line, i1) == "|")
                        {
                            //skip
                        }
                        else
                        {
                            s += GetS(line, i1);
                        }
                    }
                    int lb = GetLeftblanks(line, iStart);
                    for (int i1 = iStart; i1 <= iEnd; i1++)
                    {
                        SetNull(line, i1);
                    }
                    line[i].s = s;
                    line[i].type = ETokenType.Unknown;
                    line[i].leftblanks = lb;
                }
            }
        }

        private static bool IsNamePartStart(List<TokenHelper> line, int i)
        {
            return GetType(line, i) == ETokenType.Word || GetS(line, i) == "|" || GetS(line, i) == "#";
        }

        private static bool IsNamePartMiddle(List<TokenHelper> line, int i)
        {
            bool b = GetType(line, i) == ETokenType.Word || GetS(line, i) == "|" || GetS(line, i) == "#" || (GetType(line, i) == ETokenType.Number && !GetS(line, i).Contains("."));
            if (b && GetLeftblanks(line, i) == 0) return true;
            return false;
        }

        private static bool IsHashVariable(List<TokenHelper> line, int i)
        {
            //both #a and #(...), for instance #(listfile a)
            return GetS(line, i) == "#" && (GetType(line, i + 1) == ETokenType.Word || line[i + 1].SubnodesType() == "(") && GetLeftblanks(line, i + 1) == 0;
        }

        public static void HandleCommandName(List<TokenHelper> line)
        {
            int pos = 0;
            bool hasCloseall = false;

            if (G.Equal(line[pos].s, FromTo("ac", "accept")) != null)
            {
                line[pos].meta.commandName = "accept";
                line[pos].s = "accept";
                AddComment(line, "Note that ACCEPT in Gekko is \"ACCEPT type variable 'message';\"");
            }

            else if (G.Equal(line[pos].s, FromTo("as", "assign")) != null)
            {
                //AREMOS: assign variable type value
                //AREMOS: assign variable value
                //          0       1       2    3
                string name = line[pos + 1].s;
                if (!scalarMemory.ContainsKey(name)) scalarMemory.Add(name, "");
                line[pos].meta.commandName = "assign";
                line[pos].s = "%";
                line[pos + 1].leftblanks = 0;
                List<string> x = new List<string>();
                x.Add("bank");
                x.Add("current");
                x.Add("date");
                x.Add("direct");
                x.Add("exist");
                x.Add("for");
                x.Add("from");
                x.Add("index");
                x.Add("integer");
                x.Add("literal");
                x.Add("period");
                x.Add("string");
                x.Add("time");
                x.Add("value");
                if (Equal(line, pos + 2, x))
                {
                    line[pos + 2].s = "="; line[pos + 2].leftblanks = 1;
                }
                else
                {
                    line.Insert(pos + 2, new TokenHelper(1, "="));
                }
            }

            else if (G.Equal(line[pos].s, FromTo("cle", "clear")) != null)
            {
                line[pos].meta.commandName = "clear";
                line[pos].s = "clear";
            }

            else if (G.Equal(line[pos].s, FromTo("clo", "close")) != null)
            {
                line[pos].meta.commandName = "close";
                line[pos].s = "close";
            }

            else if (G.Equal(line[pos].s, FromTo("closeall", "closeall")) != null || G.Equal(line[pos].s, FromTo("closebanks", "closebanks")) != null)
            {
                line[pos].meta.commandName = "closeall";
                line[pos].s = Globals.restartSnippet;
                AddComment(line, "Note that in some cases, CLOSEALL is better replaced with \"CLOSE *; CLEAR;\" if scalars are to survive");
                hasCloseall = true;
            }

            else if (G.Equal(line[pos].s, FromTo("col", "collapse")) != null)
            {
                line[pos].meta.commandName = "collapse";
                line[pos].s = "collapse";
            }

            else if (G.Equal(line[pos].s, FromTo("convert", "convert")) != null)  //CONVERT is a procedure from AREMOS that seems to do the same as COLLAPSE.
            {
                line[pos].meta.commandName = "convert";
                line[pos].s = "collapse";
            }

            else if (G.Equal(line[pos].s, FromTo("comp", "compare")) != null)
            {
                line[pos].meta.commandName = "compare";
                line[pos].s = "compare";
            }

            else if (G.Equal(line[pos].s, FromTo("cou", "count")) != null)
            {
                line[pos].meta.commandName = "count";
                line[pos].s = "count";
            }

            else if (G.Equal(line[pos].s, FromTo("cop", "copy")) != null)
            {
                line[pos].meta.commandName = "copy";
                line[pos].s = "copy";
            }

            else if (G.Equal(line[pos].s, FromTo("de", "delete")) != null)
            {
                line[pos].meta.commandName = "delete";
                line[pos].s = "delete";
            }

            else if (G.Equal(line[pos].s, FromTo("disp", "display")) != null)
            {
                line[pos].meta.commandName = "display";
                line[pos].s = "disp";
            }

            else if (G.Equal(line[pos].s, FromTo("else", "else")) != null)
            {
                line[pos].meta.commandName = "else";
                line[pos].s = "else";
            }

            else if (G.Equal(line[pos].s, FromTo("excelexport", "excelexport")) != null)
            {
                line[pos].meta.commandName = "excelexport";
                line[pos].s = "sheet";
                AddComment(line, "Note that the SHEET syntax is quite different, please see the Gekko help file");
            }

            else if (G.Equal(line[pos].s, FromTo("excelimport", "excelimport")) != null)
            {
                line[pos].meta.commandName = "excelimport";
                line[pos].s = "sheet";
                AddToOptionField(line, 1, "import");
                AddComment(line, "Note that the SHEET<import> syntax is quite different, please see the Gekko help file");
            }

            else if (G.Equal(line[pos].s, FromTo("for", "for")) != null)
            {
                string name = line[pos + 1].s;
                if (!scalarMemory.ContainsKey(name)) scalarMemory.Add(name, "");
                bool hasTo = false;
                for (int ii = 1; ii < line.Count; ii++)
                {
                    if (Equal(line, ii, "to") && line[ii].leftblanks > 0)
                    {
                        hasTo = true; break;
                    }
                }
                string t = "string";
                if (hasTo) t = "val";  //could be date...
                line[pos + 1].s = t + " " + "%" + line[pos + 1].s;
                line[pos].meta.commandName = "for";
                line[pos].s = "for";
                if (hasTo) AddComment(line, "Check that val type is ok (could be date)");
                else AddComment(line, "Check that string type is ok");
            }

            else if (G.Equal(line[pos].s, FromTo("expo", "export")) != null)
            {
                line[pos].meta.commandName = "export";
                line[pos].s = "export";
            }

            else if (G.Equal(line[pos].s, FromTo("func", "function")) != null)
            {
                line[pos].meta.commandName = "function";
                line[pos].s = "function";
                AddComment(line, "Please add types and symbols, for instance '... string %s, list #m, ...' etc.");
            }

            else if (G.Equal(line[pos].s, FromTo("got", "goto")) != null)
            {
                line[pos].meta.commandName = "goto";
                line[pos].s = "goto";
            }

            else if (G.Equal(line[pos].s, FromTo("gr", "graph")) != null)
            {
                line[pos].meta.commandName = "graph";
                line[pos].s = "plot";
            }

            else if (G.Equal(line[pos].s, FromTo("if", "if")) != null)
            {
                line[pos].meta.commandName = "if";
                line[pos].s = "if";

                if (!(line[pos + 1].SubnodesType() == "("))
                {
                    //will not become sub-node, but oh well...
                    line.Insert(pos + 1, new TokenHelper(1, "("));
                    line.Insert(line.Count - 1, new TokenHelper(1, ")"));
                }
            }

            else if (G.Equal(line[pos].s, FromTo("impor", "import")) != null)
            {
                line[pos].meta.commandName = "import";
                line[pos].s = "import";
            }

            else if (G.Equal(line[pos].s, FromTo("ind", "index")) != null)
            {
                line[pos].meta.commandName = "index";
                line[pos].s = "index";
                TokenHelper last = line[line.Count - 2];  //remember semicolon
                int start = pos + 1;

                Tuple<int, int> tup = FindOptionField(line);
                if (tup.Item2 != -12345) start = tup.Item2 + 1;

                if (Equal(line, start, "series")) start++;

                int end = line.Count - 2;
                if (G.IsIdentTranslate(last.s) && last.leftblanks > 0)
                {
                    if (!listMemory.ContainsKey(last.s)) listMemory.Add(last.s, "");
                    last.s = "to #" + last.s;
                    end = line.Count - 3;
                }

                AddBracesAroundWildcard(line, start, end);

                AddToOptionField(line, 1, "showbank=no showfreq=no");

            }

            else if (G.Equal(line[pos].s, FromTo("lis", "list")) != null)
            {
                string name = line[pos + 1].s;
                if (!listMemory.ContainsKey(name)) listMemory.Add(name, "");
                line[pos].meta.commandName = "list";

                if (Equal(line, 2, "="))
                {
                    line[pos].s = "#";
                    line[pos + 1].leftblanks = 0;
                }
                else if (Equal(line, 1, "listfile") && Equal(line, 3, "="))
                {
                    line[pos].s = "#(listfile " + line[pos + 2].s + ")"; line[pos + 1].leftblanks = 0;
                    SetNull(line, pos + 1);
                    SetNull(line, pos + 2);
                }
            }

            else if (G.Equal(line[pos].s, FromTo("ma", "matrix")) != null)
            {
                string name = line[pos + 1].s;
                if (!matrixMemory.ContainsKey(name)) matrixMemory.Add(name, "");
                line[pos].meta.commandName = "matrix";
                if (Equal(line, 2, "="))
                {
                    line[pos].s = "#";
                    line[pos + 1].leftblanks = 0;
                }
            }

            else if (G.Equal(line[pos].s, FromTo("mo", "model")) != null)
            {
                line[pos].meta.commandName = "model";
                line[pos].s = "model";
            }

            else if (G.Equal(line[pos].s, FromTo("ob", "obey")) != null)
            {
                line[pos].meta.commandName = "obey";
                line[pos].s = "run";
            }

            else if (G.Equal(line[pos].s, FromTo("op", "open")) != null)
            {
                line[pos].meta.commandName = "open";
                line[pos].s = "open";
            }

            else if (G.Equal(line[pos].s, FromTo("pa", "pause")) != null)
            {
                line[pos].meta.commandName = "pause";
                line[pos].s = "pause";
            }

            else if (G.Equal(line[pos].s, FromTo("pl", "plot")) != null)
            {
                line[pos].meta.commandName = "plot";
                line[pos].s = "plot";
            }

            else if (G.Equal(line[pos].s, FromTo("pri", "print")) != null)
            {
                line[pos].meta.commandName = "print";
                line[pos].s = "prt";
            }

            else if (G.Equal(line[pos].s, FromTo("proc", "procedure")) != null)
            {
                line[pos].meta.commandName = "procedure";
                line[pos].s = "procedure";
                AddComment(line, "Please add types and symbols, for instance '... string %s, list #m, ...' etc.");

            }

            else if (G.Equal(line[pos].s, FromTo("ren", "rename")) != null)
            {
                line[pos].meta.commandName = "rename";
                line[pos].s = "rename";
            }

            else if (G.Equal(line[pos].s, FromTo("rest", "restore")) != null)
            {
                line[pos].meta.commandName = "restore";
                line[pos].s = "run";
                AddComment(line, "Use for instance 'RUN settings.opt;'");
            }

            else if (G.Equal(line[pos].s, FromTo("ret", "return")) != null)
            {
                line[pos].meta.commandName = "return";
                line[pos].s = "return";
            }

            else if (G.Equal(line[pos].s, FromTo("ser", "series")) != null)
            {
                int lb = line[pos].leftblanks;

                line[pos].meta.commandName = "series";                

                int count1 = 0;
                int count2 = 0;
                foreach (TokenHelper th in line)
                {
                    if (G.Equal(th.s, "if")) count1++;
                    if (G.Equal(th.s, "then")) count2++;
                }

                if (count1 > 0 && count2 > 0)
                {
                    AddComment(line, "Please use iif() function");
                }

                bool optionField = false;
                int i = FindS(line, "=");
                if (i != -12345 && line[i - 1].type == ETokenType.QuotedString)
                {
                    //SERIES y 'label' = ... --> <label = 'label'> y = ...
                    string label = line[i - 1].s;
                    SetNull(line, i - 1);
                    AddToOptionField(line, 1, "label = " + label);
                    optionField = true;
                }

                if (true)
                {
                    try
                    {
                        int ii = 1; //skip series
                        Tuple<int, int> tup = FindOptionField(line);
                        if (tup.Item1 != -12345) ii = tup.Item2 + 1;
                        
                        int op_i = FindS(line, "=");

                        string name = null;
                        for (int i5 = ii; i5 < op_i; i5++) name += line[i5].ToString();
                        name = name.Trim();

                        string rhs = null;
                        for (int i5 = op_i + 1; i5 < line.Count; i5++) rhs += line[i5].ToString();
                        rhs = rhs.Trim();

                        int start = rhs.IndexOf(name, 0, StringComparison.OrdinalIgnoreCase);

                        while (start >= 0)
                        {
                            if (start - 1 >= 0 && (G.IsLetterOrDigitOrUnderscore(rhs[start - 1]) || rhs[start - 1] == '|' || rhs[start - 1] == '#'))
                            {
                                //ignore
                            }
                            else
                            {
                                if (rhs[start + name.Length - 1 + 1] == '.' || (rhs[start + name.Length - 1 + 1] == '[' && rhs[start + name.Length - 1 + 2] == '-'))
                                {
                                    //lhs is seen on rhs --> use <dynamic> for safety
                                    //will have no effect on y = y + 1, only on y = y[-1] + 1, or lag functions, y[2010] etc.
                                    AddToOptionField(line, 1, "dyn");
                                    AddComment(line, "Note: <dyn> added");
                                    break;
                                }
                            }
                            start = rhs.IndexOf(name, start + 1, StringComparison.OrdinalIgnoreCase);
                        }
                    }
                    catch { };
                }


                if (optionField)
                {                    
                }
                else
                {
                    line[pos].s = ""; //not needed
                    line[pos + 1].leftblanks = 0;
                }

                Translator_Gekko20_Gekko30_ALMOST_NOT_USED_ANYMORE.MoveOptionField(line, lb);
            }

            else if (G.Equal(line[pos].s, FromTo("set", "set")) != null)
            {
                if (G.Equal(line[pos + 1].s, FromTo("per", "period")) != null)
                {
                    line[pos].meta.commandName = "set period";
                    line[pos].s = "time";
                    SetNull(line, pos + 1);
                }
                else if (G.Equal(line[pos + 1].s, FromTo("freq", "frequency")) != null)
                {
                    line[pos].meta.commandName = "set";
                    line[pos].s = "option";
                    line[pos + 1].s = "freq";
                }
                else if (G.Equal(line[pos + 1].s, FromTo("savefile", "savefile")) != null)
                {
                    line[pos].meta.commandName = "set";
                    line[pos].s = "option";
                    AddComment(line, "'set savefile ...': use 'PIPE' and 'PIPE con' instead.");
                }
                else if (G.Equal(line[pos + 1].s, FromTo("rep", "report")) != null)
                {
                    line[pos].meta.commandName = "set";
                    line[pos].s = "option";
                    AddComment(line, "'set report ...': use 'option print fields...' instead.");
                }
                else
                {
                    line[pos].meta.commandName = "set";
                    line[pos].s = "option";
                }
            }

            else if (G.Equal(line[pos].s, FromTo("sm", "smooth")) != null)
            {
                line[pos].meta.commandName = "smooth";
                line[pos].s = "smooth";
            }

            else if (G.Equal(line[pos].s, FromTo("so", "solve")) != null)
            {
                line[pos].meta.commandName = "solve";
                line[pos].s = "sim";
                //AddOption(node, "fix");  //JUST TESTING ------------------- !
            }

            else if (G.Equal(line[pos].s, FromTo("spl", "splice")) != null)
            {
                line[pos].meta.commandName = "splice";
                line[pos].s = "splice";
            }

            else if (G.Equal(line[pos].s, FromTo("spool", "spool")) != null)
            {
                line[pos].meta.commandName = "spool";
                line[pos].s = "pipe";
                //AddOption(node, "fix");  //JUST TESTING ------------------- !
            }

            else if (G.Equal(line[pos].s, FromTo("stop", "stop")) != null)
            {
                line[pos].meta.commandName = "stop";
                line[pos].s = "exit";
            }

            else if (G.Equal(line[pos].s, FromTo("sys", "system")) != null)
            {
                line[pos].meta.commandName = "system";
                line[pos].s = "sys";
            }

            else if (G.Equal(line[pos].s, FromTo("tar", "target")) != null)
            {
                line[pos].meta.commandName = "target";
                line[pos].s = "target";
            }

            else if (G.Equal(line[pos].s, FromTo("tel", "tell")) != null)
            {
                line[pos].meta.commandName = "tell";
                line[pos].s = "tell";
            }

            else if (G.Equal(line[pos].s, FromTo("truncate", "truncate")) != null)
            {
                line[pos].meta.commandName = "truncate";
                line[pos].s = "truncate";
            }

            else if (G.Equal(line[pos].s, FromTo("unspool", "unspool")) != null)
            {
                line[pos].meta.commandName = "unspool";
                line[pos].s = "pipe con";
                //AddOption(node, "fix");  //JUST TESTING ------------------- !
            }

            else if (G.Equal(line[pos].s, FromTo("vis", "vis")) != null)
            {
                line[pos].meta.commandName = "vis";
                line[pos].s = "plot";
            }

            SetLineStartRecursive(line, line);            

        }

        private static int FindS(List<TokenHelper> line, string s)
        {
            int rv = -12345;
            for (int i = 1; i < line.Count; i++)
            {
                if (Equal(line, i, s))
                {
                    rv = i;
                    break;
                }
            }
            return rv;
        }

        private static void AddBracesAroundWildcard(List<TokenHelper> line, int start, int end)
        {
            bool ok = true;
            if (end - start > 1)
            {
                for (int i = start + 1; i <= end; i++)
                {
                    if (GetLeftblanks(line, i) > 0)
                    {
                        ok = false;
                        break;
                    }
                }
            }

            if (ok)
            {
                line.Insert(start, new TokenHelper(1, "{'"));
                line[start + 1].leftblanks = 0;
                line.Insert(end + 2, new TokenHelper(0, "'}"));
                AddComment(line, "{'...'}-braces mandatory, will be fixed");
            }
        }

        private static bool IsEmptyToken(List<TokenHelper> line, int i)
        {
            if (i < 0 || i >= line.Count) return false;
            TokenHelper th = line[i];
            if (th.HasChildren()) return false;
            if (th.s != "") return false;
            return true;                
        }

        private static void AddComment(List<TokenHelper> line, string s)
        {
            
            string s2 = " /* TRANSLATE: " + s + " */";
            TokenHelper th = new TokenHelper(s2);
            bool ok = true;
            foreach(TokenHelper th2 in line)
            {
                if (th2.s == s2)
                {
                    ok = false;
                    break;
                }
            }
            if (ok) line.Add(th);  //avoid dublets
        }

        private static string GetAremosCommandName(List<TokenHelper> line2)
        {
            List<TokenHelper> line = GetCommandLine(line2);
            TokenHelper th = line[0];
            if (th.meta != null) return th.meta.commandName;
            return null;
        }

        private static bool Equal(List<TokenHelper> line, int i, string s)
        {
            return G.Equal(GetS(line, i), s);
        }

        private static bool Equal(List<TokenHelper> line, int i, List<string> ss)
        {
            return G.Equal(GetS(line, i), ss) != null;
        }

        private static string GetS(List<TokenHelper> line, int i)
        {
            if (i < 0 || i >= line.Count) return null;
            return line[i].s;
        }

        private static bool IsToken(List<TokenHelper> line, int i)
        {
            if (i < 0 || i >= line.Count) return false;
            return true;
        }

        private static int GetLeftblanks(List<TokenHelper> line, int i)
        {
            if (i < 0 || i >= line.Count) return 0;
            return line[i].leftblanks;
        }

        private static ETokenType GetType(List<TokenHelper> line, int i)
        {
            if (i < 0 || i >= line.Count) return ETokenType.Null;
            return line[i].type;
        }

        private static void SetNull(List<TokenHelper> line, int pos)
        {
            line[pos].s = ""; line[pos].leftblanks = 0; line[pos].subnodes = null;
        }

        private static bool LineStartsWithWord(List<TokenHelper> line)
        {
            return line[0].type == ETokenType.Word;
        }

        private static bool IsInsideOptionField(List<TokenHelper> line2, int i)
        {
            List<TokenHelper> line = GetCommandLine(line2);
            Tuple<int, int> tup = FindOptionField(line);  //could be COMMAND <... ( ..something.. ) ...>
            if (tup.Item1 == -12345) return false;  //no <>-field
            if (i > tup.Item1 && i < tup.Item2) return true;
            return false;
        }

        private static List<TokenHelper> GetCommandLine(List<TokenHelper> line)
        {
            return line;
        }


        private static Tuple<int, int> FindOptionField(List<TokenHelper> line2)
        {
            List<TokenHelper> line = GetCommandLine(line2);
            int i1 = 1;  //always
            if (!Equal(line, 1, "<")) return new Tuple<int, int>(-12345, -12345);
            int i2 = -12345;
            for (int i = i1 + 1; i < line.Count; i++)
            {
                if (line[i].s == ">")
                {
                    i2 = i;
                    break;
                }
            }
            if (i2 == -12345) return new Tuple<int, int>(-12345, -12345);
            return new Tuple<int, int>(i1, i2);
        }

        private static void AddToOptionField(List<TokenHelper> line2, int leftblanks, string s)
        {
            List<TokenHelper> line = GetCommandLine(line2);
            Tuple<int, int> ii = FindOptionField(line);

            if (ii.Item1 == -12345)
            {
                TokenHelper th1 = new TokenHelper(1, "<");
                TokenHelper th2 = new TokenHelper(s);
                TokenHelper th3 = new TokenHelper(">");
                line.Insert(1, th3);
                line.Insert(1, th2);
                line.Insert(1, th1);
            }
            else
            {
                line.Insert(ii.Item2, new TokenHelper(leftblanks, s));
            }
        }

        /// <summary>
        /// AREMOS specific, because you can use SER, SERI, SERIES...
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static List<string> FromTo(string s1, string s2)
        {
            List<string> s3 = new List<string>();
            if (!s2.StartsWith(s1)) throw new GekkoException();
            string temp = s1;
            for (int i = s1.Length; i < s2.Length + 1; i++)
            {
                string s = s2.Substring(0, i);
                s3.Add(s);
            }
            return s3;
        }

        private static bool KnownFunction(string ss2)
        {
            string ss = ss2.Trim().ToLower();
            return ss == "log" || ss == "exp" || ss == "pow" || ss == "abs" || ss == "pch" || ss == "dlog";
        }
    }


    class Translator_Gekko20_Gekko30_ALMOST_NOT_USED_ANYMORE
    {
        //This class translates from Gekko 2.0 to 3.0

        public static string Translate(string input)
        {
            //difference() --> except()
            //#m sort
            //#m trim
            //#m prefix = suffix = 
            //#m strip
            //<direct>

            string txt = input;
            var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
            var tags2 = new List<string>() { "//" };

            TokenHelper tokens2 = StringTokenizer.GetTokensWithLeftBlanksRecursive(txt, tags1, tags2, null, null);

            int counter = 0;

            StringBuilder rv = new StringBuilder();
            List<List<TokenHelper>> statements2 = new List<List<TokenHelper>>();
            List<TokenHelper> statement = new List<TokenHelper>();
            foreach (TokenHelper tok in tokens2.subnodes.storage)
            {
                statement.Add(tok);
                if (tok.s == ";")
                {
                    statements2.Add(statement);
                    statement = new List<TokenHelper>();
                }
            }
            statements2.Add(statement);

            List<List<TokenHelper>> statements = new List<List<TokenHelper>>();
            foreach (List<TokenHelper> line in statements2)
            {
                if (LineStartsWithWord(line) || (line[0].subnodes != null && line[0].subnodes[0].s == "("))  //second one is (series x1, series x2) = ...
                {
                    statements.Add(line);
                }
                else
                {
                    int iStop = -12345;
                    List<TokenHelper> line2 = new List<TokenHelper>();
                    for (int i = 0; i < line.Count; i++)
                    {
                        if (GetType(line, i) == ETokenType.Word || (line[i].subnodes != null && line[i].subnodes[0].s == "("))
                        {
                            iStop = i;
                            break;
                        }
                        line2.Add(line[i]);
                    }
                    statements.Add(line2);

                    if (iStop == -12345)
                    {
                        //end of file
                    }
                    else
                    {
                        List<TokenHelper> line3 = new List<TokenHelper>();
                        for (int i = iStop; i < line.Count; i++)
                        {
                            line3.Add(line[i]);
                        }
                        statements.Add(line3);
                    }
                }
            }

            List<List<TokenHelper>> temp = new List<List<TokenHelper>>();

            foreach (List<TokenHelper> line in statements)
            {
                if (Equal(line, 0, "else"))
                {
                    int ii = 1;
                    List<TokenHelper> line2 = new List<TokenHelper>();
                    line2.Add(line[0]);
                    line2.Add(new TokenHelper(0, ";", ETokenType.Symbol));
                    if (GetS(line, 1) == "\r\n")
                    {
                        line2.Add(line[1]); ii++;
                    }
                    temp.Add(line2);

                    List<TokenHelper> line3 = new List<TokenHelper>();
                    for (int i = ii; i < line.Count; i++)
                    {
                        //if (IsEmptyToken(line, i)) continue;  //skip blank tokens
                        line3.Add(line[i]);
                    }
                    if (line3[line3.Count - 1].s != ";") line3.Add(new TokenHelper(";"));
                    temp.Add(line3);
                }
                else
                {
                    temp.Add(line);
                }
            }

            foreach (List<TokenHelper> line in temp)
            {
                //Takes care of the first part of the line,
                //option field etc.
                //Records names of assigns, lists and matrices     
                try
                {
                    HandleCommandName(line);
                    HandleExpressionsRecursive(line, line);
                }
                catch
                {
                    new Error("The translator crashed unexpectedly on line " + line[0].line + ". You may try commenting out that line with //");
                    //throw new GekkoException();
                }
            }

            foreach (List<TokenHelper> line in temp)
            {
                StringBuilder sb = new StringBuilder();
                foreach (TokenHelper tok in line)
                {
                    sb.Append(tok.ToString());
                }
                rv.Append(sb);
            }

            return rv.ToString();
        }

        public static string Move(string input)  //for translate<move>
        {
            string txt = input;
            var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
            var tags2 = new List<string>() { "//" };

            TokenHelper tokens2 = StringTokenizer.GetTokensWithLeftBlanksRecursive(txt, tags1, tags2, null, null);

            int counter = 0;

            StringBuilder rv = new StringBuilder();
            List<List<TokenHelper>> statements2 = new List<List<TokenHelper>>();
            List<TokenHelper> statement = new List<TokenHelper>();
            foreach (TokenHelper tok in tokens2.subnodes.storage)
            {
                statement.Add(tok);
                if (tok.s == ";")
                {
                    statements2.Add(statement);
                    statement = new List<TokenHelper>();
                }
            }
            statements2.Add(statement);

            foreach (List<TokenHelper> line in statements2)
            {

                try
                {
                    HandleMove(line);
                }
                catch
                {
                    new Error("The translator crashed unexpectedly on line " + line[0].line + ". You may try commenting out that line with //");

                    //throw new GekkoException();
                }
            }

            foreach (List<TokenHelper> line in statements2)
            {
                StringBuilder sb = new StringBuilder();
                foreach (TokenHelper tok in line)
                {
                    sb.Append(tok.ToString());
                }
                rv.Append(sb);
            }

            return rv.ToString();
        }

        public static string Remove(string input)  //for translate<move>
        {
            string txt = input;
            var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
            var tags2 = new List<string>() { "//" };

            TokenHelper tokens2 = StringTokenizer.GetTokensWithLeftBlanksRecursive(txt, tags1, tags2, null, null);

            int counter = 0;

            StringBuilder rv = new StringBuilder();
            List<List<TokenHelper>> statements2 = new List<List<TokenHelper>>();
            List<TokenHelper> statement = new List<TokenHelper>();
            foreach (TokenHelper tok in tokens2.subnodes.storage)
            {
                statement.Add(tok);
                if (tok.s == ";")
                {
                    statements2.Add(statement);
                    statement = new List<TokenHelper>();
                }
            }
            statements2.Add(statement);

            foreach (List<TokenHelper> line in statements2)
            {

                try
                {
                    HandleRemove(line);
                }
                catch
                {
                    new Error("The translator crashed unexpectedly on line " + line[0].line + ". You may try commenting out that line with //");
                    //throw new GekkoException();
                }
            }

            foreach (List<TokenHelper> line in statements2)
            {
                StringBuilder sb = new StringBuilder();
                foreach (TokenHelper tok in line)
                {
                    sb.Append(tok.ToString());
                }
                rv.Append(sb);
            }

            return rv.ToString();
        }

        public static void SetLineStartRecursive(List<TokenHelper> line, List<TokenHelper> pointer)
        {
            for (int i = 0; i < line.Count; i++)
            {
                if (line[i].HasChildren())
                {
                    SetLineStartRecursive(line[i].subnodes.storage, pointer);
                    continue;
                }
                line[i].meta.commandLine = pointer;
            }
        }

        public static void HandleExpressionsRecursiveBefore(List<TokenHelper> line)
        {
            for (int i = 0; i < line.Count; i++)
            {
                if (line[i].HasChildren())
                {
                    HandleExpressionsRecursiveBefore(line[i].subnodes.storage);
                    continue;
                }

                //add stuff here

            }
        }

        public static void HandleExpressionsRecursive(List<TokenHelper> line, List<TokenHelper> topline)
        {

            for (int i = 0; i < line.Count; i++)
            {
                if (line[i].HasChildren())
                {
                    HandleExpressionsRecursive(line[i].subnodes.storage, topline);
                    continue;
                }

                try
                {
                    if (line[i + 0].leftblanks == 0 && line[i + 0].s == "[" && line[i + 1].s == "0" && line[i + 2].s == "]")
                    {
                        line[i + 0].leftblanks = 0;
                        line[i + 0].s = ".length()";
                        line[i + 1].s = "";
                        line[i + 1].leftblanks = 0;
                        line[i + 2].s = "";
                        line[i + 2].leftblanks = 0;
                    }
                }
                catch { };

                try
                {
                    if (line[i + 0].s == "%" && line[i + 1].type == ETokenType.Word && line[i + 2].s == "\\")
                    {
                        line[i + 0].s = "";
                        line[i + 1].s = "{%" + line[i + 1].s + "}";
                    }

                    if (line[i + 0].s == "\\" && line[i + 1].s == "%" && line[i + 2].type == ETokenType.Word)
                    {
                        line[i + 1].s = "";
                        line[i + 2].s = "{%" + line[i + 2].s + "}";
                    }
                }
                catch { };

                //quotes, interpolate                
                if (line[i].s.StartsWith("'") && line[i].s.EndsWith("'"))
                {
                    string ss = line[i].s;
                    string ss2 = "";
                    for (int ci = 0; ci < ss.Length; ci++)
                    {
                        bool curly = false;
                        if (ci > 0 && ss[ci - 1] == '{') curly = true;
                        if (ss[ci] == '%' && !curly)
                        {
                            ss2 += '{';
                            ss2 += '%'; ci++;
                            //ss2 += ss[ci]; ci++;
                            for (int cii = ci; cii < ss.Length; cii++)
                            {
                                if (G.IsLetterOrDigitOrUnderscore(ss[cii]))
                                {
                                    //good
                                    ss2 += ss[cii];
                                }
                                else
                                {
                                    ss2 += '}';
                                    ci = cii;
                                    break;
                                }
                            }
                        }
                        ss2 += ss[ci];
                    }
                    line[i].s = ss2;
                }

                if (GetS(line, i) == "&" && GetS(line, i + 1) == "+" && line[i + 1].leftblanks == 0)
                {

                    line[i].s = "||";  //#a &+ #b --> #a || #b
                    line[i + 1].s = "";

                }
                else if (GetS(line, i) == "&" && GetS(line, i + 1) == "*" && line[i + 1].leftblanks == 0)
                {
                    line[i].s = "&&";  //#a &* #b --> #a && #b
                    line[i + 1].s = "";
                }
                else if (GetS(line, i) == "&" && GetS(line, i + 1) == "-" && line[i + 1].leftblanks == 0)
                {
                    line[i].s = "-";  //#a &- #b --> #a - #b
                    line[i + 1].s = "";
                }
                else if (GetS(line, i) == "|" && GetS(line, i + 1) == "|" && line[i + 1].leftblanks == 0)
                {
                    line[i].s = ";";  //... || ...  --> ... ; ...
                    line[i + 1].s = "";
                }

                if (Equal(line, i, "difference") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    line[i].s = "except";
                }
                else if (Equal(line, i, "search") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    line[i].s = "index";
                }
                else if (Equal(line, i, "piece") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    line[i].s = "substring";
                }
                else if (Equal(line, i, "strip") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    line[i].s = "replace";
                    AddComment(line, "strip(x) is replace(x, '')");
                }
                else if (Equal(line, i, "trim") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    line[i].s = "strip";
                }
                else if (Equal(line, i, "hpfilter") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    AddComment(line, "hpfilter(): note changed arg order regarding periods");
                }
                else if (Equal(line, i, "unpack") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    AddComment(line, "unpack(): note changed arg order regarding periods");
                }
                else if (Equal(line, i, "avgt") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    AddComment(line, "avgt(): note changed arg order regarding periods");
                }
                else if (Equal(line, i, "sumt") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    AddComment(line, "sumt(): note changed arg order regarding periods");
                }

            }
        }

        public static void HandleCommandName(List<TokenHelper> line)
        {
            int pos = 0;

            line[pos].meta.commandName = line[pos].s.ToLower();  //right most of the time, exceptions P, PRI, PRT, ...


            if (G.Equal(line[pos].s, FromTo("compare", "compare")) != null)
            {
                AddComment(line, "COMPARE has changed syntax, see the help files");
            }

            else if (G.Equal(line[pos].s, "collapse"))
            {

                for (int i = 0; i < line.Count; i++)
                {
                    if (G.Equal(line[i].s, "."))
                    {
                        line[i].s = "!";
                    }
                }
            }

            else if (G.Equal(line[pos].s, FromTo("create", "create")) != null)
            {

                for (int i = 0; i < line.Count; i++)
                {
                    if (G.Equal(line[i].s, "hpfilter") || G.Equal(line[i].s, "unpack"))
                    {
                        if (G.Equal(line[0].s, "create"))
                        {
                            line[0].s = "";
                            try { line[1].leftblanks = 0; } catch { };
                        }
                    }
                }
            }

            else if (G.Equal(line[pos].s, "date"))
            {
                string name = line[pos + 1].s;

                if (Equal(line, 2, "="))
                {
                    line[pos].s = "%";
                    line[pos + 1].leftblanks = 0;
                }
            }

            else if (G.Equal(line[pos].s, "download"))
            {
                AddComment(line, "DOWNLOAD requires quotes around url");
            }

            else if (G.Equal(line[pos].s, "export"))
            {
                AddComment(line, "For EXPORT without dates, use EXPORT<all>");
            }

            else if (G.Equal(line[pos].s, "function"))
            {

            }


            else if (G.Equal(line[pos].s, FromTo("if", "if")) != null)
            {
                //line[pos].s = "if";

                //if (!(line[pos + 1].SubnodesType() == "("))
                //{
                //    //will not become sub-node, but oh well...
                //    line.Insert(pos + 1, new TokenHelper(1, "("));
                //    line.Insert(line.Count - 1, new TokenHelper(1, ")"));
                //}
            }

            else if (G.Equal(line[pos].s, "import"))
            {
                AddComment(line, "For IMPORT without dates, use IMPORT<all>");
            }

            else if (G.Equal(line[pos].s, FromTo("ind", "index")) != null)
            {
                TokenHelper last = line[line.Count - 2];  //remember semicolon

                if (G.IsIdentTranslate(last.s) && last.leftblanks > 0)
                {
                    last.s = "to #" + last.s;
                }

                AddToOptionField(line, 1, "showbank=no showfreq=no"); //Gekko 2.2 never shows banks? Certainly never freqs.

            }

            else if (G.Equal(line[pos].s, "list") || G.Equal(line[pos].s, "for"))  //NOTE: for must have been done above
            {
                bool list = false;
                if (G.Equal(line[pos].s, "list")) list = true;

                bool isParallel = false;

                if (!list)
                {

                    int eq = FindS(line, "=");

                    if (eq == 2)
                    {
                        //either FOR s = a, b, c...
                        string type = "string";
                        string name = line[pos + 1].s;
                        line[pos + 1].s = type + " " + "%" + name;

                        while (true)
                        {
                            eq = FindS(line, eq + 1, "=");
                            if (eq == -12345) break;
                            isParallel = true;
                            type = "string";
                            name = line[eq - 1].s;
                            line[eq - 1].s = type + " " + "%" + name;
                        }
                    }
                    else
                    {
                        //or     FOR date d = 100...
                        line[pos + 2].s = "%" + line[pos + 2].s;
                    }

                }

                //remove list<direct>
                if (line.Count > 3 && line[1].s == "<" && G.Equal(line[2].s, "direct") && line[3].s == ">")
                {
                    line.RemoveAt(1);
                    line.RemoveAt(1);
                    line.RemoveAt(1);
                }

                //so much is changed here that we have to run this one manually first
                HandleExpressionsRecursive(line, line);

                //TODO: list<direct>

                List<TokenHelper> l1 = new List<TokenHelper>();
                List<TokenHelper> l2 = new List<TokenHelper>();
                List<TokenHelper> l3 = new List<TokenHelper>();

                string result1 = "";
                string result2 = "";
                string result3 = "";

                int i1 = FindS(line, "=");
                if (i1 > -12345)
                {
                    for (int i = 0; i <= i1; i++) l1.Add(line[i]);
                    int i2 = FindS(line, i1 + 1, new string[] { "prefix", "suffix", "trim", "sort", "strip" });
                    if (i2 != -12345)
                    {
                        for (int i = i1 + 1; i < i2; i++) l2.Add(line[i]);
                        for (int i = i2; i < line.Count; i++) l3.Add(line[i]);
                    }
                    else
                    {
                        for (int i = i1 + 1; i < line.Count - 1; i++) l2.Add(line[i]);
                        if (GetS(line, line.Count - 1) == ";")
                        {
                            //should be so
                            l3.Add(line[line.Count - 1]);
                        }
                        else
                        {
                            //hmmm?
                            l2.Add(line[line.Count - 1]);
                        }
                    }

                    //l1, l2, l3 have been done

                    List<string> items = new List<string>();
                    List<string> itemsExtra = new List<string>();
                    string s = "";
                    string sExtra = "";
                    foreach (TokenHelper item in l2)
                    {
                        if (item.s == ",")
                        {
                            int count = s.TakeWhile(Char.IsWhiteSpace).Count();
                            items.Add(s.Trim());
                            itemsExtra.Add(sExtra + G.Blanks(count));
                            s = "";
                            sExtra = "";
                        }
                        else
                        {
                            if (item.type == ETokenType.EOL || item.type == ETokenType.EOF || item.type == ETokenType.Comment)
                            {
                                sExtra += item.ToString();
                            }
                            else
                            {
                                s += item.ToString();
                            }
                        }
                    }
                    items.Add(s);  //last item
                    itemsExtra.Add(sExtra);

                    //items are elements from l2

                    //test if items are simple
                    bool simple = true;
                    foreach (string s3 in items)
                    {
                        string s2 = s3.Trim();
                        bool curly = false;
                        for (int ic = 0; ic < s2.Length; ic++)
                        {
                            if (s2[ic] == '{') curly = true;
                            if (curly || G.IsLetterOrDigitOrUnderscore(s2[ic]) || s2[ic] == '-' || s2[ic] == '\r' || s2[ic] == '\n')
                            {
                                //ok
                            }
                            else
                            {
                                simple = false;  //could break here but never mind
                            }
                            if (s2[ic] == '}') curly = false;
                        }
                    }

                    //doing result1 here
                    if (list)
                    {
                        if (Equal(l1, 1, "listfile"))
                        {
                            //list listfile m = ...  --> #(listfile m) = ... 
                            result1 = "#(listfile " + l1[2] + ") = ";
                        }
                        else
                        {
                            for (int i = 1; i < l1.Count; i++) result1 += l1[i].ToString();
                            result1 = "#" + result1.TrimStart();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < l1.Count; i++) result1 += l1[i].ToString();
                    }

                    //do result3 here
                    int iSpecial = -12345;
                    for (int i = 0; i < l3.Count; i++)
                    {
                        int j = l3.Count - 1;
                        if (l3[i].s == "prefix")
                        {
                            iSpecial = i;
                            j = FindS(l3, "suffix");
                            int j0 = j;
                            if (j == -12345) j = l3.Count - 1;
                            l3[i].leftblanks = 0;
                            l3[i].s = "." + l3[i].s + "(";
                            l3[i + 1].s = "";
                            l3[j].s = ")" + l3[j].s;

                            if (j0 != -12345)
                            {
                                l3[j].leftblanks = 0;
                                l3[j].s = "." + l3[j].s + "(";
                                if (l3[j].s.StartsWith(".)")) l3[j].s = ")." + l3[j].s.Substring(2);  //a hack
                                l3[j + 1].s = "";
                                l3[l3.Count - 1].s = ")" + l3[l3.Count - 1].s;
                            }
                        }
                        else if (l3[i].s == "suffix")
                        {
                            iSpecial = i;
                            j = FindS(l3, "prefix");
                            int j0 = j;
                            if (j == -12345) j = l3.Count - 1;
                            l3[i].leftblanks = 0;
                            l3[i].s = "." + l3[i].s + "(";
                            l3[i + 1].s = "";
                            l3[j].s = ")" + l3[j].s;

                            if (j0 != -12345)
                            {
                                l3[j].leftblanks = 0;
                                l3[j].s = "." + l3[j].s + "(";
                                if (l3[j].s.StartsWith(".)")) l3[j].s = ")." + l3[j].s.Substring(2);  //a hack
                                l3[j + 1].s = "";
                                l3[l3.Count - 1].s = ")" + l3[l3.Count - 1].s;
                            }
                        }
                        else if (l3[i].s == "trim")
                        {
                            iSpecial = i;
                            l3[i].leftblanks = 0;
                            l3[i].s = "." + "unique" + "(";
                            l3[j].s = ")" + l3[j].s;
                        }
                        else if (l3[i].s == "sort")
                        {
                            iSpecial = i;
                            l3[i].leftblanks = 0;
                            l3[i].s = "." + l3[i].s + "(";
                            l3[j].s = ")" + l3[j].s;
                        }
                        else if (l3[i].s == "strip")
                        {
                            iSpecial = i;
                            l3[i].leftblanks = 0;
                            l3[i].s = "." + "replaceinside" + "(";
                            l3[i + 1].s = "";
                            l3[j].s = ", '')" + l3[j].s;
                        }
                    }
                    for (int i = 0; i < l3.Count; i++)
                    {
                        result3 += l3[i].ToString();
                    }

                    if ((simple || isParallel) && result3.Trim() == ";")  //no prefix etc.
                    {
                        if (items.Count == 1 && G.IsSimpleToken(items[0].Trim()))  //test of issimple... probably superfluous
                        {
                            //one-element list like list m = a;
                            result2 = itemsExtra[0] + "(" + "'" + items[0].Trim() + "',)";
                        }
                        else
                        {
                            bool first = true;
                            for (int ij = 0; ij < items.Count; ij++)
                            {
                                string s2 = items[ij];
                                if (!first) result2 += ",";
                                result2 += itemsExtra[ij] + s2;
                                first = false;
                            }
                        }
                    }
                    else
                    {
                        bool first = true;
                        for (int ij = 0; ij < items.Count; ij++)
                        {
                            string s2 = items[ij];
                            if (!first) result2 += "+";
                            if (G.IsSimpleToken(s2.Trim()))
                            {
                                result2 += itemsExtra[ij] + "('" + s2.Trim() + "',)";
                            }
                            else result2 += itemsExtra[ij] + s2;
                            first = false;
                        }
                    }

                    for (int i = 1; i < line.Count; i++)
                    {
                        line[i].s = ""; line[i].leftblanks = 0; line[i].subnodes = null;

                    }
                    line[0].leftblanks = 0;


                    if (result3.Trim() == ";")
                    {
                        line[0].s = result1 + result2 + result3;
                    }
                    else
                    {
                        line[0].s = result1 + " (" + result2.TrimStart(new char[] { ' ' }) + ")" + result3;
                    }
                }
            }



            else if (G.Equal(line[pos].s, FromTo("mat", "matrix")) != null)
            {
                line[pos].meta.commandName = "matrix";

                string name = line[pos + 1].s;

                if (Equal(line, 2, "="))
                {
                    line[pos].s = "#";
                    line[pos + 1].leftblanks = 0;
                }
            }

            else if (G.Equal(line[pos].s, "p") || G.Equal(line[pos].s, "prt") || G.Equal(line[pos].s, "pri") || G.Equal(line[pos].s, "print") || G.Equal(line[pos].s, "show"))
            {
                //Also renames SHOW --> PRT

                line[pos].meta.commandName = "prt";

                line[pos].s = "prt";
                string name = line[pos + 1].s;

                //TODO: add list syntax ()...
            }


            else if (line[pos].subnodes != null && line[pos].subnodes[0].s == "(")
            {
                //(series x1, series x2) = laspchain(...) --> x1 = laspchain(...).p; x2 = laspchain(...).q;
                //0   1   2 3   4    5 6 1     2

                if (Equal(line, 2, "laspchain") || Equal(line, 2, "laspfixed"))
                {
                    string s = null;
                    for (int i = 2; i < line.Count; i++) s += line[i].ToString();
                    s = s.Trim(); if (s.EndsWith(";")) s = s.Substring(0, s.Length - 1);
                    int j = FindS(line[pos].subnodes.storage, ",");
                    string s1 = null;
                    string s2 = null;
                    for (int i = 2; i < j; i++) s1 += line[pos].subnodes[i].ToString(); s1 = s1.Trim();
                    for (int i = j + 2; i < line[pos].subnodes.Count() - 1; i++) s2 += line[pos].subnodes[i].ToString(); s2 = s2.Trim();
                    for (int i = 0; i < line.Count; i++)
                    {
                        line[i].s = ""; line[i].leftblanks = 0;
                        line[i].subnodes = null;
                    }
                    line[0].s = s1 + " = " + s + ".p;"; line[0].leftblanks = 0;
                    line[1].s = s2 + " = " + s + ".q;"; line[1].leftblanks = 1;
                }
            }


            else if (G.Equal(line[pos].s, FromTo("ser", "series")) != null)
            {
                line[pos].meta.commandName = "series";

                //SER x = y; ok
                //SER x = x[-1] + 1; --> <dynamic>
                //SER y = 1, 2 rep 3, 3 rep * -> parenteses
                //SER y = 1 rep *; --> 1
                //SER y[2000] = ... --> ok
                //SER %m = ... --> {%m}
                //SER %m|x = ... --> {%m}x
                //SER x = x[-1] + ... --> <dynamic>

                int lb = line[pos].leftblanks;

                line[pos].s = ""; line[pos + 1].leftblanks = 0;

                int ii = 1;
                var o = FindOptionField(line);
                if (o.Item1 != -12345) ii = o.Item2 + 1;  //for instance the '=' series <2010 2020> x = 

                int op_i = -12345;
                op_i = FindS(line, ii, new string[] { "=", "^", "%", "+", "*", "#" });  //cannot match series #m = ... or series <2010 2020> #m = ...

                if (op_i != -12345)
                {
                    if (op_i == 3)
                    {
                        if (line[1].s == "#" && line[2].type == ETokenType.Word)
                        {
                            //series #m = --> series {#m} = ...
                            line[1].s = "{" + line[1].s;
                            line[2].s += "}";
                        }
                        else if (line[1].s == "%" && line[2].type == ETokenType.Word)
                        {
                            //series %m = --> series {%m} = ...
                            line[1].s = "{" + line[1].s;
                            line[2].s += "}";
                        }
                    }
                    else
                    {
                        //series %i|x = ... --> series {%x}x = ...
                        if (line[1].s == "%" && line[2].type == ETokenType.Word && line[3].s == "|")
                        {
                            line[1].s = "{" + line[1].s;
                            line[2].s += "}";
                            line[3].s = ""; line[3].leftblanks = 0;
                        }
                    }

                    int itemp = FindS(line, op_i + 1, "=");
                    if (itemp == -12345)
                    {
                        //there is not an '=' following, so it is the last equals sign (or other operator)
                        //x%y = will not have % replaced with %=
                        //problem: x%y % 3 will be wrong
                        if (line[op_i].s == "^") line[op_i].s = "^=";
                        else if (line[op_i].s == "%") line[op_i].s = "%=";
                        else if (line[op_i].s == "+") line[op_i].s = "+=";
                        else if (line[op_i].s == "*") line[op_i].s = "*=";
                        else if (line[op_i].s == "#") line[op_i].s = "#=";
                    }
                    else
                    {
                        op_i = itemp;
                    }


                    if (true)
                    {
                        try
                        {
                            string name = null;
                            for (int i = ii; i < op_i; i++) name += line[i].ToString();
                            name = name.Trim();
                            string rhs = null;
                            for (int i = op_i + 1; i < line.Count; i++) rhs += line[i].ToString();
                            rhs = rhs.Trim();

                            //if (name == "ua_s")
                            //{
                            //}

                            int start = rhs.IndexOf(name, 0, StringComparison.OrdinalIgnoreCase);

                            while (start >= 0)
                            {
                                if (start - 1 >= 0 && (G.IsLetterOrDigitOrUnderscore(rhs[start - 1]) || rhs[start - 1] == '}' || rhs[start - 1] == '|' || rhs[start - 1] == '%'))
                                {
                                    //ignore
                                }
                                else
                                {
                                    if (rhs[start + name.Length - 1 + 1] == '.' || (rhs[start + name.Length - 1 + 1] == '[' && rhs[start + name.Length - 1 + 2] == '-'))
                                    {
                                        //lhs is seen on rhs --> use <dynamic> for safety
                                        //will have no effect on y = y + 1, only on y = y[-1] + 1, or lag functions, y[2010] etc.
                                        AddToOptionField(line, 1, "dyn");
                                        AddComment(line, "Note: <dyn> added");
                                        break;
                                    }
                                }
                                start = rhs.IndexOf(name, start + 1, StringComparison.OrdinalIgnoreCase);
                            }
                        }
                        catch { };
                    }

                    bool comma = false;
                    for (int iii = op_i + 1; iii < line.Count; iii++)
                    {
                        if (line[iii].s == ",")
                        {
                            comma = true;
                            break;
                        }
                    }

                    if (comma)
                    {
                        //comma in real input, not inside function etc.
                        line[op_i].s += " (";
                        line[line.Count - 1].s = ")" + line[line.Count - 1].s;
                    }
                    else
                    {
                        //handle rep, there is no comma
                        for (int iii = op_i + 1; iii < line.Count; iii++)
                        {
                            if (G.Equal(line[iii].s, "rep") && G.Equal(line[iii + 1].s, "*"))
                            {
                                line[iii].s = "";
                                line[iii + 1].s = "";
                                line[iii + 1].leftblanks = 0;
                            }
                        }
                    }
                    if (line.Count > 1 && line[0].s == "" && line[0].subnodes == null)
                    {
                        line[0].leftblanks = 0;
                        if (line[1].subnodes != null)
                        {
                            line[1].subnodes[0].leftblanks = 0;
                        }
                        else
                        {
                            line[1].leftblanks = 0;
                        }
                    }
                }

                //move the option field
                MoveOptionField(line, lb);

            }

            else if (G.Equal(line[pos].s, "val"))
            {
                string name = line[pos + 1].s;

                if (Equal(line, 2, "="))
                {
                    line[pos].s = "%";
                    line[pos + 1].leftblanks = 0;
                }
            }

            else if (G.Equal(line[pos].s, "name") || G.Equal(line[pos].s, "string"))
            {
                string name = line[pos + 1].s;

                if (Equal(line, 2, "="))
                {
                    line[pos].s = "%";
                    line[pos + 1].leftblanks = 0;
                }
            }

            SetLineStartRecursive(line, line);


        }

        public static void HandleMove(List<TokenHelper> line)
        {
            int pos = 0;

            if (true)
            {
                //move the option field
                MoveOptionField(line, -12345); //signals that it is for translate<move>
            }
        }

        public static void HandleRemove(List<TokenHelper> line)
        {
            int pos = 0;

            if (true)
            {
                //move the option field
                RemoveParentheses(line);
            }
        }

        public static void MoveOptionField(List<TokenHelper> line, int lb)
        {
            bool move = false;
            if (lb == -12345)
            {
                move = true;
                lb = 0;
            }

            if (move && G.Equal(line[0].s, "if")) return;  //for instance if(x<1 && y>2 && < == 2)...  Probably superfluous, since the (...) is a sub-nest.

            int op_1 = -12345;
            int op_2 = -12345;
            int op_3 = -12345;
            op_1 = FindS(line, 0, new string[] { "<" });
            if (op_1 != -12345) op_2 = FindS(line, op_1, new string[] { ">" });
            if (op_2 != -12345) op_3 = FindS(line, op_2, new string[] { "=" });

            if (op_1 == -12345 || op_2 == -12345 || op_3 == -12345)
            {
                //do nothing
            }
            else
            {
                if (FindS(line, op_3 + 1, new string[] { "=" }) != -12345) return;  //no meaning if there is an extra '=' after the assignment '='
                //now we have ... < ... > ... = ...                    
                //                1     2     3
                //we need to move the stuff between 2 and 3 to before 1.
                List<TokenHelper> clone = new List<TokenHelper>();
                clone.AddRange(line);
                line.Clear();

                if (clone[op_1].leftblanks == 0) clone[op_1].leftblanks = 1;
                if (clone[op_3].leftblanks == 0) clone[op_3].leftblanks = 1;

                line.AddRange(clone.GetRange(0, op_1 - 0));
                line.AddRange(clone.GetRange(op_2 + 1, op_3 - op_2 - 1));
                line.AddRange(clone.GetRange(op_1, op_2 - op_1 + 1));
                line.AddRange(clone.GetRange(op_3, clone.Count - op_3));

                if (true)
                {
                    if (line[0].s == null || line[0].s == "")
                    {
                        line[0].leftblanks = 0;
                        line[1].leftblanks = lb;
                    }
                    else
                    {
                        line[0].leftblanks = lb;

                    }
                }
            }
        }

        /// <summary>
        /// For moving option field.
        /// </summary>
        /// <param name="line"></param>
        public static void RemoveParentheses(List<TokenHelper> line)
        {
            int op_1 = FindS(line, 0, new string[] { "=" });
            if (op_1 != -12345)
            {
                if (FindS(line, op_1 + 1, new string[] { "=" }) != -12345) return;  //this would be strange
                if (line[op_1 + 1].SubnodesType() == "(")
                {
                    if (AreAllItemsSimpleNumbers(line[op_1 + 1].SplitCommas(true)))
                    {
                        //more than 1 items, and alle are simple numbers
                        line[op_1 + 1].subnodes.storage[0].s = "";
                        line[op_1 + 1].subnodes.storage[line[op_1 + 1].subnodes.storage.Count - 1].s = "";
                    }
                }
            }
        }

        private static bool AreAllItemsSimpleNumbers(List<TokenHelperComma> xx)
        {
            bool ok = true;
            if (xx.Count < 2) ok = false;  //only > 1 item
            if (ok)
            {
                foreach (TokenHelperComma xxx in xx)
                {
                    string s = xxx.list.ToString();
                    double slet;
                    if (!double.TryParse(s, out slet))
                    {
                        ok = false;
                        break;
                    }
                }
            }
            return ok;
        }

        private static int FindS(List<TokenHelper> line, string s)
        {
            return FindS(line, 1, s);
        }

        private static int FindS(List<TokenHelper> line, int start, string s)
        {
            return FindS(line, start, new string[] { s });
        }

        private static int FindS(List<TokenHelper> line, int start, string[] ss)
        {
            int rv = -12345;
            for (int i = start; i < line.Count; i++)
            {
                foreach (string s in ss)
                {
                    if (Equal(line, i, s))
                    {
                        rv = i;
                        return i;  //break will not work
                    }
                }
            }
            return rv;
        }

        private static void AddComment(List<TokenHelper> line, string s)
        {
            string s2 = " /* TRANSLATE: " + s + " */";
            TokenHelper th = new TokenHelper(s2);
            bool ok = true;
            foreach (TokenHelper th2 in line)
            {
                if (th2.s == s2)
                {
                    ok = false;
                    break;
                }
            }
            if (ok) line.Add(th);  //avoid dublets
        }


        private static bool Equal(List<TokenHelper> line, int i, string s)
        {
            return G.Equal(GetS(line, i), s);
        }

        private static bool Equal(List<TokenHelper> line, int i, List<string> ss)
        {
            return G.Equal(GetS(line, i), ss) != null;
        }

        private static string GetS(List<TokenHelper> line, int i)
        {
            if (i < 0 || i >= line.Count) return null;
            return line[i].s;
        }

        private static int GetLeftblanks(List<TokenHelper> line, int i)
        {
            if (i < 0 || i >= line.Count) return 0;
            return line[i].leftblanks;
        }

        private static ETokenType GetType(List<TokenHelper> line, int i)
        {
            if (i < 0 || i >= line.Count) return ETokenType.Null;
            return line[i].type;
        }

        private static void SetNull(List<TokenHelper> line, int pos)
        {
            line[pos].s = ""; line[pos].leftblanks = 0; line[pos].subnodes = null;
        }

        private static bool LineStartsWithWord(List<TokenHelper> line)
        {
            return line[0].type == ETokenType.Word;
        }

        private static List<TokenHelper> GetCommandLine(List<TokenHelper> line)
        {
            return line;
        }

        private static Tuple<int, int> FindOptionField(List<TokenHelper> line2)
        {
            List<TokenHelper> line = GetCommandLine(line2);
            int i1 = 1;  //always
            if (!Equal(line, 1, "<")) return new Tuple<int, int>(-12345, -12345);
            int i2 = -12345;
            for (int i = i1 + 1; i < line.Count; i++)
            {
                if (line[i].s == ">")
                {
                    i2 = i;
                    break;
                }
            }
            if (i2 == -12345) return new Tuple<int, int>(-12345, -12345);
            return new Tuple<int, int>(i1, i2);
        }

        private static void AddToOptionField(List<TokenHelper> line2, int leftblanks, string s)
        {
            List<TokenHelper> line = GetCommandLine(line2);
            Tuple<int, int> ii = FindOptionField(line);

            if (ii.Item1 == -12345)
            {
                TokenHelper th1 = new TokenHelper(1, "<");
                TokenHelper th2 = new TokenHelper(s);
                TokenHelper th3 = new TokenHelper(">");
                line.Insert(1, th3);
                line.Insert(1, th2);
                line.Insert(1, th1);
            }
            else
            {
                line.Insert(ii.Item2, new TokenHelper(leftblanks, s));
            }
        }

        /// <summary>
        /// Should not be here...!
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static List<string> FromTo(string s1, string s2)
        {
            List<string> s3 = new List<string>();
            if (!s2.StartsWith(s1)) throw new GekkoException();
            string temp = s1;
            for (int i = s1.Length; i < s2.Length + 1; i++)
            {
                string s = s2.Substring(0, i);
                s3.Add(s);
            }
            return s3;
        }
    }
}
