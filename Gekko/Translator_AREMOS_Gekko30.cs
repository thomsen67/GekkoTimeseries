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

        public static string Translate(string input)
        {

            listMemory.Clear();
            matrixMemory.Clear();
            scalarMemory.Clear();

            string txt = input;            
            var tags2 = new List<string>() { "!" };

            TokenHelper tokens2 = StringTokenizer2.GetTokensWithLeftBlanksRecursive(txt, null, tags2, null, null);
            
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

            foreach (List<TokenHelper> line in statements)
            {
                //Takes care of the first part of the line,
                //option field etc.
                //Records names of assigns, lists and matrices
                HandleCommandName(line);                
            }            

            foreach (List<TokenHelper> line in statements)
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

        public static void HandleExpressionsRecursive(List<TokenHelper> line)
        {
            for (int i = 0; i < line.Count; i++)
            {
                if (line[i].HasChildren())
                {
                    HandleExpressionsRecursive(line[i].subnodes.storage);
                    continue;
                }

                // ------------- start of real stuff ---------------------------

                if (IsNamePartStart(line, i))
                {
                    int i2 = -12345;
                    for (int i1 = i + 1; i1 < line.Count; i1++)
                    {
                        if (!IsNamePartMiddle(line, i1))
                        {
                            i2 = i1;
                            break;
                        }
                    }
                    if (i2 > i + 1)
                    {
                        //this is a composed name
                        int iStart = i;
                        int iEnd = i2;
                        string s = null;
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
                        for (int i1 = iStart; i1 <= iEnd; i1++)
                        {
                            SetNull(line, i1);
                        }
                        line[i].s = s;
                        line[i].type = ETokenType.Unknown;
                    }
                }

                else if (GetS(line, i) == "#" && GetLeftblanks(line, i + 1) == null && GetType(line, i + 1) == ETokenType.Word)
                {
                    if (!(matrixMemory.ContainsKey(line[i + 1].s) || listMemory.ContainsKey(line[i + 1].s)))
                    {
                        line[i].s = "%";
                    }
                }

                else if (IsInsideOptionField(line, i) && Equal(line, i, FromTo("prim", "primary")))
                {
                    line[i].s = "edit";
                }

                else if (IsInsideOptionField(line, i) && Equal(line, i, FromTo("prot", "protect")))
                {
                    line[i].s = "";  //all banks are protected, unless <edit> or unlock.
                }
                else if (Equal(line, i, "="))
                {
                    if (Equal(line, i - 1, "<")) line[i].leftblanks = null;  //'<='
                    if (Equal(line, i - 1, "=")) line[i].leftblanks = null;  //'=='
                    if (Equal(line, i - 1, ">")) line[i].leftblanks = null;  //'>='  
                }
                else if (Equal(line, i, ">"))
                {
                    if (Equal(line, i - 1, ">")) line[i].leftblanks = null;  //'>'                
                }
                else if (Equal(line, i, "repeat"))
                {
                    line[i].s = "rep";
                }

                else if (line[i].type == ETokenType.Comment)
                {
                    if (line[i].s.StartsWith("!"))
                    {
                        line[i].s = "//" + line[i].s.Substring(1);
                    }
                }

                //double quotes to single quotes
                else if (line[i].s.StartsWith("\"") && line[i].s.EndsWith("\""))
                {
                    line[i].s = "'" + line[i].s.Substring(1, line[i].s.Length - 2) + "'";
                }

                //quotes                
                else if (line[i].s.StartsWith("'") && line[i].s.EndsWith("'"))
                {
                    string ss = line[i].s;
                    line[i].s = line[i].s.Replace("#", "%");
                    if (ss != line[i].s) AddComment(line, "You must change %x into {%x} inside strings");
                }

                else if (GetS(line, i) == "+")
                {
                    if (IsToken(line, i - 2) && IsToken(line, i + 2))
                    {
                        if (IsHashVariable(line, i - 2) && IsHashVariable(line, i + 1))
                        {
                            line[i].s = "||";  //#a + #b --> #a || #b
                        }
                    }
                }
                else if (GetS(line, i) == "*")
                {
                    if (IsToken(line, i - 2) && IsToken(line, i + 2))
                    {
                        if (IsHashVariable(line, i - 2) && IsHashVariable(line, i + 1))
                        {
                            line[i].s = "&&";  //#a * #b --> #a && #b
                        }
                    }
                }
                else if (GetType(line, i) == ETokenType.Word && IsToken(line, i + 1) && line[i + 1].SubnodesType() == "(")
                {
                    // log (x) --> log(x)                    
                    if (KnownFunction(GetS(line, i))) line[i + 1].subnodes[0].leftblanks = null;
                }
                else if (GetS(line, i) == "#" && GetS(line, i + 1) == "#")
                {
                    AddComment(line, "Please note that ##x in Gekko is %{%x} or #{%x}");
                }

                else if (!(GetS(line, i) == "#" || (GetS(line, i) == "%")) && GetType(line, i + 1) == ETokenType.Word)
                {
                    //normal variable/word

                    if (matrixMemory.ContainsKey(line[i + 1].s))
                    {
                        if (!(line[0].s == "set"))
                        {
                            //avoid set freq a --> set freq #a...
                            string lb = line[i + 1].leftblanks;
                            line[i + 1].leftblanks = null;
                            line.Insert(i + 1, new TokenHelper(lb, "#")); i++;
                        }
                    }
                }
                else if (GetS(line, i).Length > 1 && GetS(line, i).EndsWith("."))
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

                else if (GetS(line, i) == "|")
                {
                    //ASTNode2 x = node.GetNext();
                    string x = GetS(line, i + 1);
                    if (x != null)
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
                            if (x != null)
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

        private static bool IsNamePartStart(List<TokenHelper> line, int i)
        {
            return GetType(line, i) == ETokenType.Word || GetS(line, i) == "|" || GetS(line, i) == "#";
        }

        private static bool IsNamePartMiddle(List<TokenHelper> line, int i)
        {
            bool b = GetType(line, i) == ETokenType.Word || GetS(line, i) == "|" || GetS(line, i) == "#" || (GetType(line, i) == ETokenType.Word && !GetS(line, i).Contains("."));
            if (b && G.NullOrEmpty(GetLeftblanks(line, i))) return true;
            return false;
        }

        private static bool IsHashVariable(List<TokenHelper> line, int i)
        {
            //both #a and #(...), for instance #(listfile a)
            return GetS(line, i) == "#" && (GetType(line, i + 1) == ETokenType.Word || line[i + 1].SubnodesType() == "(") && GetLeftblanks(line, i + 1) == null;
        }

        public static void HandleCommandName(List<TokenHelper> line)
        {
            int pos = 0;
            bool hasCloseall = false;
            
            HandleExpressionsRecursive(line);

            if (G.Equal(line[pos].s, FromTo("ac", "accept")) != null)
            {
                line[pos].meta.aremosCommandName = "accept";
                line[pos].s = "accept";
                AddComment(line, "Note that ACCEPT in Gekko is \"ACCEPT type variable 'message';\"");
            }

            else if (G.Equal(line[pos].s, FromTo("as", "assign")) != null)
            {
                //AREMOS: assign variable type value
                //          0       1       2    3
                string name = line[pos + 1].s;
                if (!scalarMemory.ContainsKey(name)) scalarMemory.Add(name, "");
                line[pos].meta.aremosCommandName = "assign";
                line[pos].s = "%";
                line[pos + 1].leftblanks = null;
                line[pos + 2].s = "="; line[pos + 2].leftblanks = " ";
            }

            else if (G.Equal(line[pos].s, FromTo("cle", "clear")) != null)
            {
                line[pos].meta.aremosCommandName = "clear";
                line[pos].s = "clear";
            }

            else if (G.Equal(line[pos].s, FromTo("clo", "close")) != null)
            {
                line[pos].meta.aremosCommandName = "close";
                line[pos].s = "close";
            }

            else if (G.Equal(line[pos].s, FromTo("closeall", "closeall")) != null || G.Equal(line[pos].s, FromTo("closebanks", "closebanks")) != null)
            {
                line[pos].meta.aremosCommandName = "closeall";
                line[pos].s = Globals.restartSnippet;
                AddComment(line, "Note that in some cases, CLOSEALL is better replaced with \"CLOSE *; CLEAR;\" if scalars are to survive");
                hasCloseall = true;
            }

            else if (G.Equal(line[pos].s, FromTo("col", "collapse")) != null)
            {
                line[pos].meta.aremosCommandName = "collapse";
                line[pos].s = "collapse";
            }

            else if (G.Equal(line[pos].s, FromTo("convert", "convert")) != null)  //CONVERT is a procedure from AREMOS that seems to do the same as COLLAPSE.
            {
                line[pos].meta.aremosCommandName = "convert";
                line[pos].s = "collapse";
            }

            else if (G.Equal(line[pos].s, FromTo("comp", "compare")) != null)
            {
                line[pos].meta.aremosCommandName = "compare";
                line[pos].s = "compare";
            }

            else if (G.Equal(line[pos].s, FromTo("cou", "count")) != null)
            {
                line[pos].meta.aremosCommandName = "count";
                line[pos].s = "count";
            }

            else if (G.Equal(line[pos].s, FromTo("cop", "copy")) != null)
            {
                line[pos].meta.aremosCommandName = "copy";
                line[pos].s = "copy";
            }

            else if (G.Equal(line[pos].s, FromTo("de", "delete")) != null)
            {
                line[pos].meta.aremosCommandName = "delete";
                line[pos].s = "delete";
            }

            else if (G.Equal(line[pos].s, FromTo("disp", "display")) != null)
            {
                line[pos].meta.aremosCommandName = "display";
                line[pos].s = "disp";
            }
            
            else if (G.Equal(line[pos].s, FromTo("excelexport", "excelexport")) != null)
            {
                line[pos].meta.aremosCommandName = "excelexport";
                line[pos].s = "sheet";
                AddComment(line, "Note that the SHEET syntax is quite different, please see the Gekko help file");
            }

            else if (G.Equal(line[pos].s, FromTo("excelimport", "excelimport")) != null)
            {
                line[pos].meta.aremosCommandName = "excelimport";
                line[pos].s = "sheet";
                AddToOptionField(line, " ", "import");
                AddComment(line, "Note that the SHEET<import> syntax is quite different, please see the Gekko help file");
            }

            else if (G.Equal(line[pos].s, FromTo("for", "for")) != null)
            {
                string name = line[pos + 1].s;
                if (!scalarMemory.ContainsKey(name)) scalarMemory.Add(name, "");
                line[pos + 1].s = "%" + line[pos + 1].s;
                line[pos].meta.aremosCommandName = "for";
                line[pos].s = "for";
            }
                        
            else if (G.Equal(line[pos].s, FromTo("expo", "export")) != null)
            {
                line[pos].meta.aremosCommandName = "export";
                line[pos].s = "export";
            }
            
            else if (G.Equal(line[pos].s, FromTo("func", "function")) != null)
            {
                line[pos].meta.aremosCommandName = "function";
                line[pos].s = "function";
            }

            else if (G.Equal(line[pos].s, FromTo("got", "goto")) != null)
            {
                line[pos].meta.aremosCommandName = "goto";
                line[pos].s = "goto";
            }

            else if (G.Equal(line[pos].s, FromTo("gr", "graph")) != null)
            {
                line[pos].meta.aremosCommandName = "graph";
                line[pos].s = "plot";
            }

            else if (G.Equal(line[pos].s, FromTo("if", "if")) != null)
            {
                line[pos].meta.aremosCommandName = "if";
                line[pos].s = "if";

                if (!(line[pos + 1].SubnodesType() == "("))
                {
                    //will not become sub-node, but oh well...
                    line.Insert(pos + 1, new TokenHelper(" ", "("));
                    line.Insert(line.Count - 1, new TokenHelper(" ", ")"));
                }
            }

            else if (G.Equal(line[pos].s, FromTo("impor", "import")) != null)
            {
                line[pos].meta.aremosCommandName = "import";
                line[pos].s = "import";
            }

            else if (G.Equal(line[pos].s, FromTo("ind", "index")) != null)
            {
                line[pos].meta.aremosCommandName = "index";
                line[pos].s = "index";
                TokenHelper last = line[line.Count - 2];  //remember semicolon
                if (G.IsIdentTranslate(last.s) && !G.NullOrEmpty(last.leftblanks))
                {
                    if (!listMemory.ContainsKey(last.s)) listMemory.Add(last.s, "");
                    last.s = "to #" + last.s;                    
                }

                //TODO:

                //ASTNode2 xx = node.GetCommand3();
                //string last = null;
                //string leftBlanks = "";
                //last = xx.GetLastChild().Text;
                //leftBlanks = xx.GetLastChild().leftBlanks;                
                //if (G.IsIdentTranslate(last) && leftBlanks.Length > 0)  //so that "LIST *a;" does not put a in mem
                //{
                //    if (!listMemory.ContainsKey(last)) listMemory.Add(last, "");
                //}
            }

            else if (G.Equal(line[pos].s, FromTo("lis", "list")) != null)
            {
                string name = line[pos + 1].s;
                if (!listMemory.ContainsKey(name)) listMemory.Add(name, "");
                line[pos].meta.aremosCommandName = "list";

                if (Equal(line, 2, "="))
                {
                    line[pos].s = "#";
                    line[pos + 1].leftblanks = null;
                }
                else if (Equal(line, 1, "listfile") && Equal(line, 3, "="))
                {
                    line[pos].s = "#(listfile " + line[pos + 2].s + ")"; line[pos + 1].leftblanks = null;
                    SetNull(line, pos + 1);
                    SetNull(line, pos + 2);
                }
            }

            else if (G.Equal(line[pos].s, FromTo("ma", "matrix")) != null)
            {
                string name = line[pos + 1].s;
                if (!matrixMemory.ContainsKey(name)) matrixMemory.Add(name, "");
                line[pos].meta.aremosCommandName = "matrix";
                if (Equal(line, 2, "="))
                {
                    line[pos].s = "#";
                    line[pos + 1].leftblanks = null;
                }
            }

            else if (G.Equal(line[pos].s, FromTo("mo", "model")) != null)
            {
                line[pos].meta.aremosCommandName = "model";
                line[pos].s = "model";
            }

            else if (G.Equal(line[pos].s, FromTo("ob", "obey")) != null)
            {
                line[pos].meta.aremosCommandName = "obey";
                line[pos].s = "run";
            }

            else if (G.Equal(line[pos].s, FromTo("op", "open")) != null)
            {
                line[pos].meta.aremosCommandName = "open";
                line[pos].s = "open";
            }

            else if (G.Equal(line[pos].s, FromTo("pa", "pause")) != null)
            {
                line[pos].meta.aremosCommandName = "pause";
                line[pos].s = "pause";
            }

            else if (G.Equal(line[pos].s, FromTo("pl", "plot")) != null)
            {
                line[pos].meta.aremosCommandName = "plot";
                line[pos].s = "plot";
            }

            else if (G.Equal(line[pos].s, FromTo("pri", "print")) != null)
            {
                line[pos].meta.aremosCommandName = "print";
                line[pos].s = "prt";
            }

            else if (G.Equal(line[pos].s, FromTo("proc", "procedure")) != null)
            {
                line[pos].meta.aremosCommandName = "procedure";
                line[pos].s = "procedure";
                TokenHelper artificial = new TokenHelper(new TokenList(line), null);
                List<TokenHelperComma> split = artificial.SplitCommas();
                for (int i = 0; i < split.Count; i++)
                {
                    TokenHelperComma thc = split[i];
                    if (i == 0)
                    {
                        if (thc.list.Count() == 2)
                        {
                            thc.list[1].s = "[type] %" + thc.list[0].s;
                        }
                    }
                    else
                    {
                        if (thc.list.Count() == 1)
                        {
                            thc.list[0].s = "[type] %" + thc.list[0].s;
                        }
                    }
                }
                AddComment(line, "Please add [type], and perhaps alter '%'");

            }

            else if (G.Equal(line[pos].s, FromTo("ren", "rename")) != null)
            {
                line[pos].meta.aremosCommandName = "rename";
                line[pos].s = "rename";
            }

            else if (G.Equal(line[pos].s, FromTo("rest", "restore")) != null)
            {
                line[pos].meta.aremosCommandName = "restore";
                line[pos].s = "run";
                AddComment(line, "Use for instance 'RUN settings.opt;'");
            }

            else if (G.Equal(line[pos].s, FromTo("ret", "return")) != null)
            {
                line[pos].meta.aremosCommandName = "return";
                line[pos].s = "return";
            }

            else if (G.Equal(line[pos].s, FromTo("ser", "series")) != null)
            {
                line[pos].meta.aremosCommandName = "series";
                line[pos].s = null; //not needed
                line[pos + 1].leftblanks = null;

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
            }

            else if (G.Equal(line[pos].s, FromTo("set", "set")) != null)
            {
                if (G.Equal(line[pos + 1].s, FromTo("per", "period")) != null)
                {
                    line[pos].meta.aremosCommandName = "set period";
                    line[pos].s = "time";
                    SetNull(line, pos + 1);
                }
                else if (G.Equal(line[pos + 1].s, FromTo("freq", "frequency")) != null)
                {
                    line[pos].meta.aremosCommandName = "set";
                    line[pos].s = "option";
                    line[pos + 1].s = "freq";
                }
                else if (G.Equal(line[pos + 1].s, FromTo("savefile", "savefile")) != null)
                {
                    line[pos].meta.aremosCommandName = "set";
                    line[pos].s = "option";
                    AddComment(line, "'set savefile ...': use 'PIPE' and 'PIPE con' instead.");
                }
                else if (G.Equal(line[pos + 1].s, FromTo("rep", "report")) != null)
                {
                    line[pos].meta.aremosCommandName = "set";
                    line[pos].s = "option";
                    AddComment(line, "'set report ...': use 'option print fields...' instead.");
                }
                else
                {
                    line[pos].meta.aremosCommandName = "set";
                    line[pos].s = "option";
                }
            }

            else if (G.Equal(line[pos].s, FromTo("sm", "smooth")) != null)
            {
                line[pos].meta.aremosCommandName = "smooth";
                line[pos].s = "smooth";
            }

            else if (G.Equal(line[pos].s, FromTo("so", "solve")) != null)
            {
                line[pos].meta.aremosCommandName = "solve";
                line[pos].s = "sim";
                //AddOption(node, "fix");  //JUST TESTING ------------------- !
            }

            else if (G.Equal(line[pos].s, FromTo("spl", "splice")) != null)
            {
                line[pos].meta.aremosCommandName = "splice";
                line[pos].s = "splice";
            }

            else if (G.Equal(line[pos].s, FromTo("spool", "spool")) != null)
            {
                line[pos].meta.aremosCommandName = "spool";
                line[pos].s = "pipe";
                //AddOption(node, "fix");  //JUST TESTING ------------------- !
            }

            else if (G.Equal(line[pos].s, FromTo("stop", "stop")) != null)
            {
                line[pos].meta.aremosCommandName = "stop";
                line[pos].s = "exit";
            }

            else if (G.Equal(line[pos].s, FromTo("sys", "system")) != null)
            {
                line[pos].meta.aremosCommandName = "system";
                line[pos].s = "sys";
            }

            else if (G.Equal(line[pos].s, FromTo("tar", "target")) != null)
            {
                line[pos].meta.aremosCommandName = "target";
                line[pos].s = "target";
            }

            else if (G.Equal(line[pos].s, FromTo("tel", "tell")) != null)
            {
                line[pos].meta.aremosCommandName = "tell";
                line[pos].s = "tell";
            }

            else if (G.Equal(line[pos].s, FromTo("truncate", "truncate")) != null)
            {
                line[pos].meta.aremosCommandName = "truncate";
                line[pos].s = "truncate";
            }

            else if (G.Equal(line[pos].s, FromTo("unspool", "unspool")) != null)
            {
                line[pos].meta.aremosCommandName = "unspool";
                line[pos].s = "pipe con";
                //AddOption(node, "fix");  //JUST TESTING ------------------- !
            }

            else if (G.Equal(line[pos].s, FromTo("vis", "vis")) != null)
            {
                line[pos].meta.aremosCommandName = "vis";
                line[pos].s = "plot";
            }
        }

        private static void AddComment(List<TokenHelper> line, string s)
        {
            line.Add(new TokenHelper(" /* " + s + " */"));
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

        private static string GetLeftblanks(List<TokenHelper> line, int i)
        {
            if (i < 0 || i >= line.Count) return null;
            return line[i].leftblanks;
        }

        private static ETokenType GetType(List<TokenHelper> line, int i)
        {
            if (i < 0 || i >= line.Count) return ETokenType.Null;
            return line[i].type;
        }

        private static void SetNull(List<TokenHelper> line, int pos)
        {
            line[pos].s = ""; line[pos].leftblanks = null;
        }

        private static bool LineStartsWithWord(List<TokenHelper> line)
        {
            return line[0].type == ETokenType.Word;
        }

        private static bool IsInsideOptionField(List<TokenHelper> line, int i)
        {
            Tuple<int, int> tup = FindOptionField(line);
            if (tup.Item1 == -12345) return false;  //no <>-field
            if (i > tup.Item1 && i < tup.Item2) return true;
            return false;
        }

        private static Tuple<int, int> FindOptionField(List<TokenHelper> line)
        {            
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

        private static void AddToOptionField(List<TokenHelper> line, string leftblanks, string s)
        {
            Tuple<int, int> ii = FindOptionField(line);

            if (ii.Item1 == -12345)
            {
                TokenHelper th1 = new TokenHelper(" ", "<");
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
}
