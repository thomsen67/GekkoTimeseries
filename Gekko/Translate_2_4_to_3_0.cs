﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gekko
{
    public class Translate_2_4_to_3_0
    {

        

        //This class translates from Gekko 2.0 to 3.0

        public class Info
        {
            public GekkoDictionary<string, string> collectionMemory = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);            
            public GekkoDictionary<string, string> scalarMemory = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public static string Translate(string input)
        {
            Info info = new Info();

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

            List<List<TokenHelper>> statements3 = new List<List<TokenHelper>>();

            //puts into "lines", where (series x, series y) = ... is also legal.
            foreach (List<TokenHelper> line in statements2)
            {
                if (LineStartsWithWord(line) || (line[0].subnodes != null && line[0].subnodes[0].s == "("))  //second one is (series x1, series x2) = ...
                {
                    statements3.Add(line);
                }
                else
                {
                    int iStop = -12345;
                    List<TokenHelper> line2 = new List<TokenHelper>();
                    for (int i = 0; i < line.Count; i++)
                    {
                        if (StringTokenizer.GetType(line, i) == ETokenType.Word || (line[i].subnodes != null && line[i].subnodes[0].s == "("))
                        {
                            iStop = i;
                            break;
                        }
                        line2.Add(line[i]);
                    }
                    statements3.Add(line2);

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
                        statements3.Add(line3);
                    }
                }
            }

            List<List<TokenHelper>> statements = new List<List<TokenHelper>>();

            //In Gekko 2.4, an ELSE statement could end without a ";". Therefore, the semicolon is addede here, and the ELS
            //gets its own statement.
            foreach (List<TokenHelper> line in statements3)
            {
                if (StringTokenizer.Equal(line, 0, "else"))
                {
                    int ii = 1;
                    List<TokenHelper> line2 = new List<TokenHelper>();
                    line2.Add(line[0]);
                    line2.Add(new TokenHelper(0, ";", ETokenType.Symbol));
                    if (StringTokenizer.GetS(line, 1) == "\r\n")
                    {
                        line2.Add(line[1]); ii++;
                    }
                    statements.Add(line2);

                    List<TokenHelper> line3 = new List<TokenHelper>();
                    for (int i = ii; i < line.Count; i++)
                    {
                        //if (IsEmptyToken(line, i)) continue;  //skip blank tokens
                        line3.Add(line[i]);
                    }
                    if (line3[line3.Count - 1].s != ";") line3.Add(new TokenHelper(";"));
                    statements.Add(line3);
                }
                else
                {
                    statements.Add(line);
                }
            }

            //We have to reorganize...
            int counter2 = -1;
            foreach (List<TokenHelper> line in statements)
            {
                TokenHelper topnode = new TokenHelper();
                topnode.artificialTopNode = true;
                topnode.type = ETokenType.Unknown;
                topnode.subnodes = new TokenList();
                foreach (TokenHelper x in line)
                {
                    counter2++;
                    topnode.subnodes.storage.Add(x);
                    x.id = counter2;
                    x.parent = topnode;
                }
            }

            foreach (List<TokenHelper> line in statements)
            {
                //Takes care of the first part of the line,
                //option field etc.
                //Records names of assigns, lists and matrices     
                try
                {
                    HandleCommandName(line, info);
                    HandleExpressionsRecursive(line, line, info);
                }
                catch
                {
                    new Error("The translator crashed unexpectedly on line " + line[0].line + ". You may try commenting out that line with //");
                }
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

        /// <summary>
        /// Expressions often have deeper structure, inside (), [], and {}.
        /// This method gets to the bottom of any expression.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="topline"></param>
        public static void HandleExpressionsRecursive(List<TokenHelper> line, List<TokenHelper> topline, Info info)
        {

            for (int i = 0; i < line.Count; i++)
            {
                if (line[i].HasChildren())
                {
                    HandleExpressionsRecursive(line[i].subnodes.storage, topline, info);
                    continue;
                }

                try
                {
                    //¤034 [a*b?c] --> {'a*b?c}, #m[a*b?c] --> #m['a*b?c'].
                    if (line[0].s == "[")
                    {
                        TokenHelper parent = line[0].parent;
                        if (parent != null)
                        {
                            if (line[0].leftblanks == 0)
                            {
                                TokenHelper previous = parent.SiblingBefore(1, true);
                                if (previous != null)
                                {
                                    char c = previous.s[previous.s.Length - 1];
                                    if (c == '}' || G.IsLetterOrDigitOrUnderscore(c))
                                    {
                                        //something like abc[...] or {%s}[...] with no blanks between
                                        bool good = true;
                                        string s = StringTokenizer.GetTextFromLeftBlanksTokens(line, 1, line.Count - 2);
                                        for (int i7 = 0; i7 < s.Length; i7++)
                                        {
                                            char c7 = s[i7];
                                            if (G.IsLetterOrDigitOrUnderscore(c7) || c7 == '*' || c7 == '?')
                                            {
                                                //good
                                            }
                                            else good = false;
                                        }
                                        if (good)
                                        {
                                            line[1].s = "'" + line[1].s;
                                            line[line.Count - 2].s = line[line.Count - 2].s + "'";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch { };

                try
                {
                    //¤001 [0] --> .length()
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
                    // ¤002 %x\ --> {%x}\ for path
                    if (line[i + 0].s == "%" && line[i + 1].type == ETokenType.Word && line[i + 2].s == "\\")
                    {
                        line[i + 0].s = "";
                        line[i + 1].s = "{%" + line[i + 1].s + "}";
                    }

                    // \%x --> \{%x}
                    if (line[i + 0].s == "\\" && line[i + 1].s == "%" && line[i + 2].type == ETokenType.Word)
                    {
                        line[i + 1].s = "";
                        line[i + 2].s = "{%" + line[i + 2].s + "}";
                    }
                }
                catch { };

                //¤003 quotes, interpolate. Stuff like 'a%x%y|z ~%x {%y}' --> 'a{%x}{%y}z %x {%y}'
                if (line[i].s.StartsWith("'") && line[i].s.EndsWith("'"))
                {
                    string ss = line[i].s;
                    string ss2 = "";                
                    for (int ci = 0; ci < ss.Length; ci++)
                    {
                        bool curly = false;
                        bool tilde = false;
                        if (ci > 0 && ss[ci - 1] == '{') curly = true;  //a % with a { before
                        if (ci > 0 && ss[ci - 1] == '~') tilde = true;  //a % with a ~ before                        
                        if (ss[ci] == '%' && !curly && !tilde)
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
                                    ci = cii - 1;
                                    goto Flag1;
                                }
                            }
                        }
                        if (ss[ci] == '|' || ss[ci] == '~')
                        {
                            //skip
                        }
                        else
                        {
                            ss2 += ss[ci];
                        }
                    Flag1:;
                    }
                    line[i].s = ss2;
                }

                // ¤004
                if (StringTokenizer.GetS(line, i) == "&" && StringTokenizer.GetS(line, i + 1) == "+" && line[i + 1].leftblanks == 0)
                {
                    //list operator 1
                    line[i].s = "||";  //#a &+ #b --> #a || #b
                    line[i + 1].s = "";

                }
                else if (StringTokenizer.GetS(line, i) == "&" && StringTokenizer.GetS(line, i + 1) == "*" && line[i + 1].leftblanks == 0)
                {
                    //list operator 2
                    line[i].s = "&&";  //#a &* #b --> #a && #b
                    line[i + 1].s = "";
                }
                else if (StringTokenizer.GetS(line, i) == "&" && StringTokenizer.GetS(line, i + 1) == "-" && line[i + 1].leftblanks == 0)
                {
                    //list operator 3
                    line[i].s = "-";  //#a &- #b --> #a - #b
                    line[i + 1].s = "";
                }
                else if (StringTokenizer.GetS(line, i) == "|" && StringTokenizer.GetS(line, i + 1) == "|" && line[i + 1].leftblanks == 0)
                {
                    //¤005
                    line[i].s = ";";  //... || ...  --> ... ; ...
                    line[i + 1].s = "";
                }

                if (StringTokenizer.Equal(line, i, "avgt") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤006
                    AddComment(line, "avgt(): if local time is given, use <%t1, %t2> syntax with <>-brackets");
                }
                else if (StringTokenizer.Equal(line, i, "difference") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤007
                    line[i].s = "except";
                }
                else if (StringTokenizer.Equal(line, i, "endswith") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤008
                    AddComment(line, "endswith() is case-insensitive in Gekko 3.0");
                }
                else if (StringTokenizer.Equal(line, i, "fromseries") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //TODO: if fromseries('x', ...) or fromseries(%x, ...) --> fromseries(x, ...) or fromseries({%x}, ...) 
                }
                else if (StringTokenizer.Equal(line, i, "hpfilter") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤009
                    AddComment(line, "hpfilter(): if local time is given, use <%t1, %t2> syntax with <>-brackets");
                }
                else if (StringTokenizer.Equal(line, i, "pack") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤010
                    AddComment(line, "pack(): if local time is given, use <%t1, %t2> syntax with <>-brackets");
                }
                else if (StringTokenizer.Equal(line, i, "piece") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤11
                    line[i].s = "substring";
                }
                else if (StringTokenizer.Equal(line, i, "replace") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤012
                    AddComment(line, "replace() is case-insensitive in Gekko 3.0");
                }
                else if (StringTokenizer.Equal(line, i, "search") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤013
                    line[i].s = "index";
                }
                else if (StringTokenizer.Equal(line, i, "startswith") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤014
                    AddComment(line, "startswith() is case-insensitive in Gekko 3.0");
                }
                else if (StringTokenizer.Equal(line, i, "strip") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤015
                    line[i].s = "replace";
                    AddComment(line, "strip(%x) is now replace(%x, '')");
                }
                else if (StringTokenizer.Equal(line, i, "sumt") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤016
                    AddComment(line, "sumt(): if local time is given, use <%t1, %t2> syntax with <>-brackets");
                }
                else if (StringTokenizer.Equal(line, i, "trim") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤017
                    line[i].s = "strip";
                }                
                else if (StringTokenizer.Equal(line, i, "unpack") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //¤018
                    AddComment(line, "unpack(): if local time is given, use <%t1, %t2> syntax with <>-brackets");
                }

                
                try
                {
                    bool upgrade = false;
                    bool neverUpgrade = false;
                    bool overriding = false;
                    string var = null;
                    int type = 0;  //1 is scalar, 2 is collection
                    bool isLhs = false; //token is between start and a "="                   

                    List<TokenHelper> nesting = StringTokenizer.GetNesting(line);
                    TokenHelper start = line[i];
                    if (nesting.Count > 0) start = nesting[nesting.Count - 1];  //uppermost token
                    //start is not necessarily the first token at this level
                    TokenHelper supreme = start.parent.subnodes[0];
                    string commandName = supreme.meta.commandName;

                    if (line[i + 0].s == "%" && line[i + 1].type == ETokenType.Word)
                    {
                        var = "%" + line[i + 1].s;
                        type = 1;
                    }
                    else if (line[i + 0].s == "#" && line[i + 1].type == ETokenType.Word)
                    {
                        var = "#" + line[i + 1].s;
                        type = 2;
                    }
                    
                    if (supreme.Search(i, new List<string>() { "=" }, true, true) == -12345 && supreme.Search(i, new List<string>() { "=" }, false, true) != -12345)
                    {
                        //we are on lhs of a "="
                        isLhs = true;
                    }

                    if (type != 0)
                    {
                        //is a %- or #-variable
                        //handle different commands.                        
                        
                        List<string> typeMinusPlus = new List<string>() { "analyze", "clip", "ols", "prt", "sheet", "plot", "mulprt", "gmulprt" };
                        List<string> typePlusPlus = new List<string>() { "checkoff", "close", "compare", "copy", "create", "delete", "disp", "doc", "endo", "exo", "export", "findmissingdata", "import", "itershow", "rebase", "read", "rename", "smooth", "splice", "truncate", "write", "x12a" };
                        if (type == 1)
                        {
                            if (typePlusPlus.Contains(commandName)) upgrade = true;
                        }
                        else if (type == 2)
                        {
                            if (typeMinusPlus.Contains(commandName)) upgrade = true;
                            if (typePlusPlus.Contains(commandName)) upgrade = true;
                        }

                        if (commandName == "series")
                        {
                            //conversion from for instance * into *= has been done already in translation.
                            int j1 = supreme.Search(i, new List<string>() { "=", "^=", "%=", "+=", "*=", "#=" }, true, true);
                            int j2 = supreme.Search(i, new List<string>() { "=", "^=", "%=", "+=", "*=", "#=" }, false, true);
                            if (j1 != -12345 && j2 == -12345)
                            {
                                //we are on rhs
                                if (type == 2) upgrade = true;  //stuff like y = #m, or maybe it covers y = #m[2] stuff.
                            }
                            else if (j1 == -12345 && j2 != -12345)
                            {
                                //we are on lhs
                                bool good = true;
                                foreach (TokenHelper th in nesting)
                                {                                    
                                    if (th.s == "[")
                                    {
                                        good = false;
                                    }
                                }

                                if (good)
                                {
                                    //we will not upgrade %t in y[%t] = ... .
                                    upgrade = true;
                                }                                
                            }
                        }

                        // --------------------------------------------------
                        // -- now we do the neverUpgrade --------------------
                        // --------------------------------------------------                                                
                        
                        //Do not upgrade something like prt <color = %s> ... ;                        
                        int i1 = supreme.Search(i, new List<string>() { "<" }, true, false);
                        int i2 = supreme.Search(i, new List<string>() { ">" }, false, false);
                        if (i1 == 1 && i2 > i)
                        {
                            //there is a < at pos 1 when going left, and a > when going right.
                            //then pretty sure we are inside local options.
                            neverUpgrade = true;
                        }

                        //never upgrade for option, list, matrix, show
                        if (commandName == "option" || commandName == "matrix" || commandName == "show" || commandName == "list") neverUpgrade = true;
                                               
                        //something like PLOT x, y, z file = %x; should never upgrade %x.
                        int ii1 = supreme.Search(i, new List<string>() { "=" }, true, true);
                        if (ii1 != -12345)
                        {
                            List<string> xx = new List<string>() { "series", "val", "date", "string", "name", "list", "matrix", "collapse", "for", "if", "interpolate", "ols", "smooth", "splice" };
                            if (xx.Contains(commandName))
                            {
                                //do nothing for these, where "=" occurs naturally as a non-option.
                            }
                            else
                            {
                                neverUpgrade = true;
                            }
                        }

                        //never upgrade a known val or date
                        string scalar = null; info.scalarMemory.TryGetValue(var, out scalar);
                        if (scalar == "val" || scalar == "date")
                        {
                            neverUpgrade = true;
                        }

                        //never upgrade a known matrix
                        string collection = null; info.collectionMemory.TryGetValue(var, out collection);
                        if (collection == "matrix")
                        {
                            neverUpgrade = true;
                        }

                        bool lhsSpecial = false;
                        if (isLhs)
                        {
                            if (commandName == "string" || commandName == "name" || commandName == "val" || commandName == "date" || commandName == "for")
                            {
                                //never upgrade something like string %s = ... 
                                //also not for %i = ...
                                neverUpgrade = true;
                                lhsSpecial = true;
                            }
                        }

                        // -------------------------------------------
                        // -- Overriding -----------------------------
                        // -------------------------------------------

                        //Override: if %x is known to be NAME type, or FOR name x = ...   or FOR x = ..., 
                        //it must always be upgraded.                        
                        if (scalar == "name" || scalar == "forname")
                        {
                            if(!lhsSpecial) overriding = true;
                        }



                    }

                    //at the end
                    if (neverUpgrade) upgrade = false;
                    if (overriding) upgrade = true;

                    //make sure that %s or #m inside curlies are NEVER ever upgraded                                                
                    foreach (TokenHelper th in nesting)
                    {
                        //never upgrade if already inside {}, like {#m}. Also covers expression inside {}.
                        if (th.s == "{")
                        {
                            upgrade = false;
                            break;
                        }
                    }

                    if (upgrade)
                    {
                        //upgrade
                        line[i + 0].s = "{" + line[i + 0].s;
                        line[i + 1].s = line[i + 1].s + "}";                        
                    }

                }
                catch { }
            }
        }


        /// <summary>
        /// Stuff here only looks at one sentence, does not go inside any recursive (), [] or {}.
        /// </summary>
        /// <param name="line"></param>
        public static void HandleCommandName(List<TokenHelper> line, Info info)
        {
            int pos0 = 0;

            line[pos0].meta.commandName = line[pos0].s.ToLower();  //right most of the time, exceptions P, PRI, PRT, ...

            if (G.Equal(line[pos0].s, "compare"))
            {
                //¤019
                AddComment(line, "COMPARE has changed syntax, see the help files");
            }
            else if (G.Equal(line[pos0].s, "collapse"))
            {
                //¤020
                for (int i = 0; i < line.Count; i++)
                {
                    if (G.Equal(line[i].s, "."))
                    {
                        line[i].s = "!";
                    }
                }
            }

            else if (G.Equal(line[pos0].s, "copy"))
            {
                //from/to --> frombank/tobank
                //Must use COPY {#m}, not COPY #m
            }


            else if (G.Equal(line[pos0].s, "create"))
            {
                //¤021
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

            else if (G.Equal(line[pos0].s, "date"))
            {
                //¤022
                string name = line[pos0 + 1].s;
                if (!info.scalarMemory.ContainsKey("%" + name)) info.scalarMemory.Add("%" + name, "date");

                if (StringTokenizer.Equal(line, 2, "="))
                {
                    line[pos0].s = "%";
                    line[pos0 + 1].leftblanks = 0;
                }
            }

            else if (G.Equal(line[pos0].s, "download"))
            {
                //¤023
                AddComment(line, "DOWNLOAD requires quotes around the URL");
            }

            else if (G.Equal(line[pos0].s, "export"))
            {
                //¤024
                AddComment(line, "For EXPORT without dates, use EXPORT<all>");
            }

            else if (G.Equal(line[pos0].s, "function"))
            {

            }

            else if (G.Equal(line[pos0].s, "if"))
            {
                //line[pos].s = "if";

                //if (!(line[pos + 1].SubnodesType() == "("))
                //{
                //    //will not become sub-node, but oh well...
                //    line.Insert(pos + 1, new TokenHelper(1, "("));
                //    line.Insert(line.Count - 1, new TokenHelper(1, ")"));
                //}
            }

            else if (G.Equal(line[pos0].s, "import"))
            {
                //¤024
                AddComment(line, "For IMPORT without dates, use IMPORT<all>");
            }

            else if (G.Equal(line[pos0].s, "index"))
            {
                //¤025
                TokenHelper last = line[line.Count - 2];  //remember semicolon

                if (G.IsIdentTranslate(last.s) && last.leftblanks > 0)
                {
                    last.s = "to #" + last.s;
                }

                AddToOptionField(line, 1, "showbank=no showfreq=no"); //Gekko 2.2 never shows banks? Certainly never freqs.

            }

            else if (G.Equal(line[pos0].s, "list") || G.Equal(line[pos0].s, "for"))  //NOTE: for must have been done above
            {
                bool list = false;
                if (G.Equal(line[pos0].s, "list"))
                {
                    list = true;
                    if (!info.collectionMemory.ContainsKey("#" + line[pos0 + 1].s)) info.collectionMemory.Add("#" + line[pos0 + 1].s, "list");
                }                

                bool isParallel = false;

                if (!list)
                {
                    //¤026
                    int eq = StringTokenizer.FindS(line, "=");

                    if (eq == 2)
                    {
                        //either FOR s = a, b, c...
                        string type = "string";
                        string name = line[pos0 + 1].s;
                        line[pos0 + 1].s = type + " " + "%" + name;                        
                        if (!info.scalarMemory.ContainsKey("%" + name)) info.scalarMemory.Add("%" + name, "forname");                        

                        while (true)
                        {
                            eq = StringTokenizer.FindS(line, eq + 1, "=");
                            if (eq == -12345) break;
                            isParallel = true;
                            type = "string";
                            name = line[eq - 1].s;
                            line[eq - 1].s = type + " " + "%" + name;
                            if (G.Equal(line[eq - 2].s, "string"))
                            {
                                if (!info.scalarMemory.ContainsKey("%" + name)) info.scalarMemory.Add("%" + name, "forstring");
                            }
                            else
                            {
                                if (!info.scalarMemory.ContainsKey("%" + name)) info.scalarMemory.Add("%" + name, "forname");
                            }
                        }
                    }
                    else
                    {
                        //or     FOR date d = 100...
                        string type = line[pos0 + 1].s.ToLower();                        
                        string name = line[pos0 + 2].s;
                        if (!info.scalarMemory.ContainsKey("%" + name)) info.scalarMemory.Add("%" + name, "for" + type);
                        line[pos0 + 2].s = "%" + line[pos0 + 2].s;
                        if (G.Equal(line[pos0 + 1].s, "name")) line[pos0 + 1].s = "string";
                    }
                }

                //remove list<direct>
                if (line.Count > 3 && line[1].s == "<" && G.Equal(line[2].s, "direct") && line[3].s == ">")
                {
                    //¤027
                    line.RemoveAt(1);
                    line.RemoveAt(1);
                    line.RemoveAt(1);
                }

                //so much is changed here that we have to run this one manually first
                HandleExpressionsRecursive(line, line, info);

                HandleCommandNameListElements(line, list, isParallel);                
            }

            else if (G.Equal(line[pos0].s, "matrix"))
            {
                //¤029
                line[pos0].meta.commandName = "matrix";

                string name = line[pos0 + 1].s;
                if (!info.collectionMemory.ContainsKey("#" + name)) info.collectionMemory.Add("#" + name, "matrix");

                if (StringTokenizer.Equal(line, 2, "="))
                {
                    line[pos0].s = "#";
                    line[pos0 + 1].leftblanks = 0;
                }
            }            

            else if (G.Equal(line[pos0].s, "p") || G.Equal(line[pos0].s, "prt") || G.Equal(line[pos0].s, "pri") || G.Equal(line[pos0].s, "print") || G.Equal(line[pos0].s, "show"))
            {
                //Also renames SHOW --> PRT
                line[pos0].meta.commandName = "prt";
                //¤030
                if (G.Equal(line[pos0].s, "show"))
                {
                    line[pos0].s = "prt";  //don't change for the PRT variants
                    line[pos0].meta.commandName = "show";
                }
                string name = line[pos0 + 1].s;
                //TODO: add list syntax ()...
            }

            else if (line[pos0].subnodes != null && line[pos0].subnodes[0].s == "(")
            {
                //¤031
                //(series x1, series x2) = laspchain(...) --> x1 = laspchain(...).p; x2 = laspchain(...).q;
                //0   1   2 3   4    5 6 1     2

                if (StringTokenizer.Equal(line, 2, "laspchain") || StringTokenizer.Equal(line, 2, "laspfixed"))
                {
                    string s = null;
                    for (int i = 2; i < line.Count; i++) s += line[i].ToString();
                    s = s.Trim(); if (s.EndsWith(";")) s = s.Substring(0, s.Length - 1);
                    int j = StringTokenizer.FindS(line[pos0].subnodes.storage, ",");
                    string s1 = null;
                    string s2 = null;
                    for (int i = 2; i < j; i++) s1 += line[pos0].subnodes[i].ToString(); s1 = s1.Trim();
                    for (int i = j + 2; i < line[pos0].subnodes.Count() - 1; i++) s2 += line[pos0].subnodes[i].ToString(); s2 = s2.Trim();
                    for (int i = 0; i < line.Count; i++)
                    {
                        line[i].s = ""; line[i].leftblanks = 0;
                        line[i].subnodes = null;
                    }
                    line[0].s = s1 + " = " + s + ".p;"; line[0].leftblanks = 0;
                    line[1].s = s2 + " = " + s + ".q;"; line[1].leftblanks = 1;
                }
            }

            else if (G.Equal(line[pos0].s, "ser") || G.Equal(line[pos0].s, "series"))
            {
                HandleCommandNameSeries(line, pos0);
            }

            else if (G.Equal(line[pos0].s, "val"))
            {
                //¤022
                string name = line[pos0 + 1].s;
                if (!info.scalarMemory.ContainsKey("%" + name)) info.scalarMemory.Add("%" + name, "val");

                if (StringTokenizer.Equal(line, 2, "="))
                {
                    line[pos0].s = "%";
                    line[pos0 + 1].leftblanks = 0;
                }
            }

            else if (G.Equal(line[pos0].s, "name") || G.Equal(line[pos0].s, "string"))
            {
                //¤022
                string name = line[pos0 + 1].s;

                if (G.Equal(line[pos0].s, "name"))
                {
                    if (!info.scalarMemory.ContainsKey("%" + name)) info.scalarMemory.Add("%" + name, "name");
                }
                else
                {
                    if (!info.scalarMemory.ContainsKey("%" + name)) info.scalarMemory.Add("%" + name, "string");
                }

                if (StringTokenizer.Equal(line, 2, "="))
                {
                    line[pos0].s = "%";
                    line[pos0 + 1].leftblanks = 0;
                }
            }

            SetLineStartRecursive(line, line);


        }

        private static void HandleCommandNameSeries(List<TokenHelper> line, int pos)
        {
            //¤032

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
            op_i = StringTokenizer.FindS(line, ii, new string[] { "=", "^", "%", "+", "*", "#" });  //cannot match series #m = ... or series <2010 2020> #m = ...

            if (op_i != -12345)
            {
                //if (op_i == 3)
                //{
                //    if (line[1].s == "#" && line[2].type == ETokenType.Word)
                //    {
                //        //series #m = --> series {#m} = ...
                //        line[1].s = "{" + line[1].s;
                //        line[2].s += "}";
                //    }
                //    else if (line[1].s == "%" && line[2].type == ETokenType.Word)
                //    {
                //        //series %m = --> series {%m} = ...
                //        line[1].s = "{" + line[1].s;
                //        line[2].s += "}";
                //    }
                //}
                //else
                //{
                //    //series %i|x = ... --> series {%x}x = ...
                //    if (line[1].s == "%" && line[2].type == ETokenType.Word && line[3].s == "|")
                //    {
                //        line[1].s = "{" + line[1].s;
                //        line[2].s += "}";
                //        line[3].s = ""; line[3].leftblanks = 0;
                //    }
                //}

                int itemp = StringTokenizer.FindS(line, op_i + 1, "=");
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

        private static void HandleCommandNameListElements(List<TokenHelper> line, bool list, bool isParallel)
        {
            //¤028
            List<TokenHelper> l1 = new List<TokenHelper>();
            List<TokenHelper> l2 = new List<TokenHelper>();
            List<TokenHelper> l3 = new List<TokenHelper>();

            string result1 = "";
            string result2 = "";
            string result3 = "";

            int i1 = StringTokenizer.FindS(line, "=");
            if (i1 > -12345)
            {
                for (int i = 0; i <= i1; i++) l1.Add(line[i]);
                int i2 = StringTokenizer.FindS(line, i1 + 1, new string[] { "prefix", "suffix", "trim", "sort", "strip" });
                if (i2 != -12345)
                {
                    for (int i = i1 + 1; i < i2; i++) l2.Add(line[i]);
                    for (int i = i2; i < line.Count; i++) l3.Add(line[i]);
                }
                else
                {
                    for (int i = i1 + 1; i < line.Count - 1; i++) l2.Add(line[i]);
                    if (StringTokenizer.GetS(line, line.Count - 1) == ";")
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
                items.Add(s.Trim());  //last item
                itemsExtra.Add(sExtra);
                //The following is a BIG HACK to handle blanks for the last element.
                if (itemsExtra.Count > 1 && itemsExtra[itemsExtra.Count - 1] == "" && itemsExtra[itemsExtra.Count - 2].Length > 0) itemsExtra[itemsExtra.Count - 1] = " ";
                                
                //items are elements from l2
                //doing result1 here
                if (list)
                {
                    if (StringTokenizer.Equal(l1, 1, "listfile"))
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
                        j = StringTokenizer.FindS(l3, "suffix");
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
                        j = StringTokenizer.FindS(l3, "prefix");
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

                //test if simple. Simple is stuff like a, b, c5, 0d, 1, 2, #s, #m.
                
                bool simple = true;
                for (int ij = 0; ij < items.Count; ij++)
                {
                    string s7 = items[ij];
                    if (s7.StartsWith("{") && s7.EndsWith("}"))
                    {
                        s7 = s7.Substring(1, s7.Length - 2);
                    }
                    bool first = true;
                    foreach (char c in s7)
                    {
                        if (G.IsLetterOrDigitOrUnderscore(c) || (first && (c == '%' || c == '#')))
                        {
                            //good
                        }
                        else
                        {
                            simple = false;
                        }
                        first = false;
                    }
                }

                if (isParallel || (simple && result3.Trim() == ";"))  //no prefix etc.
                {
                    if (items.Count == 1)  //test of issimple... probably superfluous
                    {
                        //one-element list like list m = a;                        
                        result2 = itemsExtra[0] + UpgradeString(items[0]) + ",";
                    }
                    else
                    {
                        bool first = true;
                        for (int ij = 0; ij < items.Count; ij++)
                        {
                            string s2 = items[ij];
                            if (!first) result2 += ",";
                            result2 += itemsExtra[ij] + UpgradeString(s2);
                            first = false;
                        }
                    }
                }
                else
                {
                    //--> dont do this: it will pollute all the functions etc. that return a list not a string.
                    //if (items.Count == 1)  //test of issimple... probably superfluous
                    //{
                    //    result2 = "(" + itemsExtra[0] + items[0] + ",)";
                    //}
                    //else
                    {
                        bool first = true;
                        for (int ij = 0; ij < items.Count; ij++)
                        {
                            string s2 = items[ij];
                            if (!first) result2 += "+";
                            if (IsVerySimple(s2.Trim()))
                            {
                                result2 += itemsExtra[ij] + "('" + s2.Trim() + "',)";  //a becomes ('a',)
                            }
                            else
                            {
                                result2 += itemsExtra[ij] + s2;  //stuff like %s or #m.
                            }
                            first = false;
                        }
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

                if(isParallel) AddComment(line, "Parallel loops may not be translated properly, including missing {}-curlies on elements");
            }
        }

        /// <summary>
        /// Transforms %x --> {%x} and #m --> {#m}
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string UpgradeString(string s)
        {
            string s5 = s.Trim();
            if (!s5.Contains(" ") && (s5.StartsWith("%") || s5.StartsWith("#"))) s5 = "{" + s5 + "}";
            return s5;
        }

        /// <summary>
        /// Must be like a38, f16, var2, _var3, x_y, 007. Will also trim the string first, just in case. 
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="allowFreqIndicator"></param>
        /// <returns></returns>
        private static bool IsVerySimple(string varName2)
        {
            if (varName2 == null) return false;
            string varName = varName2.Trim();
            foreach (char c in varName)
            {
                if (G.IsLetterOrDigitOrUnderscore(c))  //also allows 007.
                {
                    //good
                }
                else return false;
            }
            return true;
        }

        /// <summary>
        /// Primarily transforms &lt;2000 2010&gt; x = 5; into x &lt;2000 2010&gt; x = 5; . This
        /// is because SERIES is removed from series &lt;2000 2010&gt; x = 5;.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lb"></param>
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
            op_1 = StringTokenizer.FindS(line, 0, new string[] { "<" });
            if (op_1 != -12345) op_2 = StringTokenizer.FindS(line, op_1, new string[] { ">" });
            if (op_2 != -12345) op_3 = StringTokenizer.FindS(line, op_2, new string[] { "=" });

            if (op_1 == -12345 || op_2 == -12345 || op_3 == -12345)
            {
                //do nothing
            }
            else
            {
                if (StringTokenizer.FindS(line, op_3 + 1, new string[] { "=" }) != -12345) return;  //no meaning if there is an extra '=' after the assignment '='
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
            if (!StringTokenizer.Equal(line, 1, "<")) return new Tuple<int, int>(-12345, -12345);
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
    }
}
