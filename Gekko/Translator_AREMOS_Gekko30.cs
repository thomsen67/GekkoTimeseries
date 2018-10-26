﻿using System;
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

            //string txt = GetTextFromFileWithWait(Program.options.folder_working + "\\" + "model.gms");
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
                if (GetS(line, i) == "#" && GetLeftblanks(line, i + 1) == null && GetType(line, i + 1) == ETokenType.Word)
                {
                    if (scalarMemory.ContainsKey(line[i + 1].s))
                    {
                        line[i].s = "%";
                    }
                }
                if (!(GetS(line, i) == "#" || (GetS(line, i) == "%")) && GetType(line, i + 1) == ETokenType.Word)
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
            }
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
                string name = line[pos+1].s;
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

            else if (G.Equal(line[pos].s, FromTo("for", "for")) != null)
            {
                string name = line[pos + 1].s;
                if (!scalarMemory.ContainsKey(name)) scalarMemory.Add(name, "");
                line[pos].meta.aremosCommandName = "for";
                line[pos].s = "for";
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

            else if (G.Equal(line[pos].s, FromTo("vis", "vis")) != null)
            {
                line[pos].meta.aremosCommandName = "vis";
                line[pos].s = "plot";
            }

            else if (G.Equal(line[pos].s, FromTo("expo", "export")) != null)
            {
                line[pos].meta.aremosCommandName = "export";
                line[pos].s = "export";
            }

            else if (G.Equal(line[pos].s, FromTo("for", "for")) != null)
            {
                line[pos].meta.aremosCommandName = "for";
                line[pos].s = "for";
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

            else if (G.Equal(line[pos].s, FromTo("ren", "rename")) != null)
            {
                line[pos].meta.aremosCommandName = "rename";
                line[pos].s = "rename";
            }

            else if (G.Equal(line[pos].s, FromTo("rest", "restore")) != null)
            {
                line[pos].meta.aremosCommandName = "restore";
                line[pos].s = "run";
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
        }

        private static void AddComment(List<TokenHelper> line, string s)
        {
            line.Add(new TokenHelper(" /*" + s + " */"));
        }

        private static bool Equal(List<TokenHelper> line, int i, string s)
        {
            return G.Equal(GetS(line, i), s);
        }

        private static string Equal(List<TokenHelper> line, int i, List<string> ss)
        {
            return G.Equal(GetS(line, i), ss);
        }

        private static string GetS(List<TokenHelper> line, int i)
        {
            if (i < 0 || i >= line.Count) return null;
            return line[i].s;
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

        private static Tuple<int, int> FindOptionField(List<TokenHelper> line)
        {
            int i1 = 1;  //always
            int i2 = -12345;
            for (int i = i1 + 1; i < line.Count; i++)
            {
                if (line[i].s == ">")
                {
                    i2 = i;
                    break;
                }
            }
            if (i2 == -12345) i1 = -12345;
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
    }
}
