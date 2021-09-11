using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Gekko
{
    public class Translate_2_4_to_3_0
    {              

        //This class translates from Gekko 2.0 to 3.0

        public class Info
        {
            public GekkoDictionary<string, string> collectionMemory = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);            
            public GekkoDictionary<string, string> scalarMemory = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public bool useGlobalBankForNonSeries = true;
            public bool keepTypes = false;
            public int globalCounter = 0;
            
        }

        public static string Translate(string input, Info info)
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

            //We have to reorganize... (#807508925428903)            
            foreach (List<TokenHelper> line in statements)
            {
                TokenHelper topnode = new TokenHelper();
                topnode.artificialTopNode = true;
                topnode.type = ETokenType.Unknown;
                topnode.subnodes = new TokenList();
                foreach (TokenHelper x in line)
                {                    
                    topnode.subnodes.storage.Add(x);                    
                }
                topnode.OrganizeSubnodes(); //must do this manually
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
                catch (Exception e)
                {
                    new Error("The translator crashed unexpectedly on line " + line[0].line + ". You may try commenting out that line with //");
                }
            }

            foreach (List<TokenHelper> line in statements)
            {
                StringBuilder sb = new StringBuilder();
                List<TokenHelper> reorderTokens = new List<TokenHelper>();
                foreach (TokenHelper tok in line)
                {
                    if (tok.s.Contains("/* TRANSLATE:")) reorderTokens.Add(tok);
                    else sb.Append(tok.ToString());
                }
                foreach (TokenHelper tok in reorderTokens)
                {
                    sb.Append(tok.ToString());
                }
                rv.Append(sb);
            }

            string output = rv.ToString();
            output = output.Replace("|{", "{").Replace("}|", "}");  //not x|{%i}|y. Probably no {...}|| or ||{...}, would not make sense

            return output;
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
            List<string> typeMinusPlus = new List<string>() { "analyze", "clip", "ols", "prt", "sheet", "plot", "mulprt", "gmulprt" };
            List<string> typePlusPlus = new List<string>() { "checkoff", "close", "compare", "copy", "create", "delete", "disp", "doc", "endo", "exo", "export", "findmissingdata", "import", "itershow", "rebase", "read", "rename", "smooth", "splice", "truncate", "write", "x12a" };

            TokenHelper supreme = null;
            List<TokenHelper> nesting = StringTokenizer.GetNesting(line);
            string commandName = "";
            for (int i = 0; i < line.Count; i++)
            {
                int ii = i;
                
                if (nesting.Count == 0)
                {                    
                    try
                    {
                        supreme = line[i].parent.subnodes[0];
                    } catch { }
                }
                else
                {
                    try
                    {
                        //either the token from the highgest command line, or a "(", "[", "{" from a sub-line just under the command line                    
                        supreme = nesting[nesting.Count - 1].parent.parent.subnodes[0];
                    } catch { }
                    ii = nesting[nesting.Count - 1].parent.id;
                }

                if (commandName == "" && supreme != null)
                {
                    //some of the last nodes may be without .parent (comments etc.). This is a hack to handle it.
                    commandName = supreme.meta.commandName;
                }

                if (line[i].HasChildren())
                {
                    HandleExpressionsRecursive(line[i].subnodes.storage, topline, info);
                    continue;
                }

                try
                {
                    //¤034 
                    //[a*b?c] --> {'a*b?c}, #m[a*b?c] --> #m['a*b?c'].
                    if (line[0].s == "[")
                    {
                        TokenHelper parent = line[0].parent;
                        TokenHelper previous = parent.SiblingBefore(1, false);
                        int blanks = line[0].leftblanks;
                        char c = previous.s[previous.s.Length - 1];
                        bool seemsAfterVariable = false;
                        if (c == '}' || G.IsLetterOrDigitOrUnderscore(c)) seemsAfterVariable = true;  //does not test glue
                        if (previous.id == 0) seemsAfterVariable = false;  //something like delete[*]
                        bool looksLikeWildcard = true;
                        string s = StringTokenizer.GetTextFromLeftBlanksTokens(line, 1, line.Count - 2);
                        for (int i7 = 0; i7 < s.Length; i7++)
                        {
                            char c7 = s[i7];
                            if (G.IsLetterOrDigitOrUnderscore(c7) || c7 == '*' || c7 == '?')
                            {
                                //good
                            }
                            else looksLikeWildcard = false;
                        }
                        if (!(s.Contains("*") || s.Contains("?"))) looksLikeWildcard = false; //#m[3] should not be #m['3']
                        bool looksLikeRange = false;
                        if (s.Contains("..") && !s.Contains("'")) looksLikeRange = true;
                        bool mustBeCurly = false;
                        if (typeMinusPlus.Contains(commandName.ToLower()) || typePlusPlus.Contains(commandName.ToLower())) mustBeCurly = true;
                        int dots = -12345;
                        for (int jj = 1; jj < line.Count; jj++)
                        {
                            if (line[jj - 1].s == "." && line[jj].s == "." && line[jj].leftblanks == 0)
                            {
                                dots = jj - 1;
                                break;
                            }
                        }

                        // ============

                        if (blanks == 0 && seemsAfterVariable)
                        {
                            //something like abc[...] or {%s}[...] with no blanks between

                            if (looksLikeWildcard)
                            {
                                line[0].s = "['";
                                line[line.Count - 1].s = "']";
                            }
                            else if (looksLikeRange)
                            {
                                //abc[a..z] --> abc['a'..'z']
                                line[0].s = "['";
                                line[line.Count - 1].s = "']";
                                line[dots].s = "'" + line[dots].s;
                                line[dots + 1].s = line[dots + 1].s + "'";
                            }
                        }
                        else
                        {
                            if (mustBeCurly)
                            {
                                //prt [a*b] --> prt {'a*b'}
                                if (looksLikeWildcard)
                                {
                                    line[0].s = "{'";
                                    line[line.Count - 1].s = "'}";
                                }
                                else if (looksLikeRange)
                                {
                                    //prt [a..z] --> prt {'a'..'z'}
                                    line[0].s = "{'";
                                    line[line.Count - 1].s = "'}";
                                    line[dots].s = "'" + line[dots].s;
                                    line[dots + 1].s = line[dots + 1].s + "'";
                                }
                            }
                            else
                            {
                                //list m = [a*b] --> #m = ['a*b'].
                                if (looksLikeWildcard)
                                {
                                    line[0].s = "['";
                                    line[line.Count - 1].s = "']";
                                }
                                else if (looksLikeRange)
                                {
                                    //list m = [a..z] --> #m = ['a'..'z']
                                    line[0].s = "['";
                                    line[line.Count - 1].s = "']";
                                    line[dots].s = "'" + line[dots].s;
                                    line[dots + 1].s = line[dots + 1].s + "'";
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

                try
                {
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
                } catch { }

                //¤0038
                try
                {
                    if (line[0].s == "(")
                    {
                        TokenHelper before = line[0].parent.SiblingBefore();
                        if ((before.s == "%" || before.s == "#") && line[0].leftblanks == 0)
                        {
                            //¤0039 stuff like %(a%b), but not #(listfile ...).
                            bool good = true;
                            for (int i7 = 0; i7 < line.Count; i7++)
                            {
                                if (G.Equal(line[i7].s, "listfile")) good = false;
                            }
                            //if (good) AddComment(line, "In general, expressions like %(...) or #(...) are better written with {}-curlies. For instance, %(a%b) can be written as %a{%b}.");
                        }
                    }
                } catch { }

                // ¤004
                try
                {
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
                        AddComment(line, "The first argument in fromseries() must be a series, not a string. Use x instead of 'x' and {%x} instead of %x.");
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
                } catch { }

                try
                {
                    //all {i} --> {%i}
                    if (line[i + 0].s == "{" && line[i + 1].type == ETokenType.Word)
                    {                        
                        line[i + 1].s = "%" + line[i + 1].s;
                    }
                    if (line[i + 0].type == ETokenType.QuotedString && line[i + 0].s.StartsWith("'") && line[i + 0].s.EndsWith("'"))
                    {                        
                        if (line[i + 0].s.Contains("{"))
                        {
                            //hack because I was lazy... (avoids replacing {%x})
                            line[i + 0].s = line[i + 0].s.Replace("{a", "{%a");
                            line[i + 0].s = line[i + 0].s.Replace("{b", "{%b");
                            line[i + 0].s = line[i + 0].s.Replace("{c", "{%c");
                            line[i + 0].s = line[i + 0].s.Replace("{d", "{%d");
                            line[i + 0].s = line[i + 0].s.Replace("{e", "{%e");
                            line[i + 0].s = line[i + 0].s.Replace("{f", "{%f");
                            line[i + 0].s = line[i + 0].s.Replace("{g", "{%g");
                            line[i + 0].s = line[i + 0].s.Replace("{h", "{%h");
                            line[i + 0].s = line[i + 0].s.Replace("{i", "{%i");
                            line[i + 0].s = line[i + 0].s.Replace("{j", "{%j");
                            line[i + 0].s = line[i + 0].s.Replace("{k", "{%k");
                            line[i + 0].s = line[i + 0].s.Replace("{l", "{%l");
                            line[i + 0].s = line[i + 0].s.Replace("{m", "{%m");
                            line[i + 0].s = line[i + 0].s.Replace("{n", "{%n");
                            line[i + 0].s = line[i + 0].s.Replace("{o", "{%o");
                            line[i + 0].s = line[i + 0].s.Replace("{p", "{%p");
                            line[i + 0].s = line[i + 0].s.Replace("{q", "{%q");
                            line[i + 0].s = line[i + 0].s.Replace("{r", "{%r");
                            line[i + 0].s = line[i + 0].s.Replace("{s", "{%s");
                            line[i + 0].s = line[i + 0].s.Replace("{t", "{%t");
                            line[i + 0].s = line[i + 0].s.Replace("{u", "{%u");
                            line[i + 0].s = line[i + 0].s.Replace("{v", "{%v");
                            line[i + 0].s = line[i + 0].s.Replace("{w", "{%w");
                            line[i + 0].s = line[i + 0].s.Replace("{x", "{%x");
                            line[i + 0].s = line[i + 0].s.Replace("{y", "{%y");
                            line[i + 0].s = line[i + 0].s.Replace("{z", "{%z");

                        }
                    }
                } catch { }

                try
                {
                    bool upgrade = false;
                    bool neverUpgrade = false;
                    bool overriding = false;
                    string var = null;
                    int type = 0;  //1 is scalar, 2 is collection
                    bool isLhs = false; //token is between start and a "="                   


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

                    if (supreme.Search(ii, new List<string>() { "=" }, true, true) == -12345 && supreme.Search(ii, new List<string>() { "=" }, false, true) != -12345)
                    {
                        //we are on lhs of a "="
                        isLhs = true;
                    }

                    if (type != 0)
                    {
                        //is a %- or #-variable
                        //handle different commands.                        

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
                            int j1 = supreme.Search(ii, new List<string>() { "=", "^=", "%=", "+=", "*=", "#=" }, true, true);
                            int j2 = supreme.Search(ii, new List<string>() { "=", "^=", "%=", "+=", "*=", "#=" }, false, true);
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

                        try
                        {
                            TokenHelper parent = line[i + 0].parent;
                            if (!parent.artificialTopNode)
                            {
                                if (parent.subnodes[0].s == "(")
                                {
                                    //¤0036
                                    TokenHelper before = parent.SiblingBefore();
                                    if (before != null && (G.Equal(before.s, "avg") || G.Equal(before.s, "sum")))
                                    {
                                        upgrade = true;
                                    }
                                }
                            }
                        }
                        catch { }

                        // --------------------------------------------------
                        // -- now we do the neverUpgrade --------------------
                        // --------------------------------------------------                                                

                        //Do not upgrade something like prt <color = %s> ... ;                        
                        int i1 = supreme.Search(ii, new List<string>() { "<" }, true, false);
                        int i2 = supreme.Search(ii, new List<string>() { ">" }, false, false);
                        if (i1 != -12345 && i2 != -12345)
                        {
                            if (i1 < i && i2 > i)  //we are inside <...> . It COULD be an IF(...<...  or ...>...) but oh well
                            {
                                //there is a < at pos 1 when going left, and a > when going right.
                                //then pretty sure we are inside local options.
                                neverUpgrade = true;
                            }
                        }

                        //never upgrade for option, list, matrix, show
                        if (commandName == "option" || commandName == "matrix" || commandName == "show" || commandName == "list") neverUpgrade = true;

                        //something like PLOT x, y, z file = %x; should never upgrade %x.
                        int ii1 = supreme.Search(ii, new List<string>() { "=" }, true, true);
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
                            if (!lhsSpecial) overriding = true;
                        }


                        try
                        {
                            if (type == 1)
                            {
                                if (G.IsIdentTranslate(line[i + 1].s) && line[i + 2].s == ":")
                                {
                                    overriding = true;  //something like ....%x:....
                                }
                            }
                        }
                        catch { }

                        try
                        {
                            if (line[i - 1].s == ":")
                            {
                                overriding = true;  //something like ....:%x....  or ....:#x....
                            }
                        }
                        catch { }

                        try
                        {
                            if (type == 1)
                            {
                                if (line[i].leftblanks == 0)
                                {
                                    string before1 = null;
                                    string before2 = null;
                                    int glue = 0;
                                    try { before1 = line[i - 1].s; } catch { }
                                    try { before2 = line[i - 2].s; } catch { }
                                    try { glue = line[i - 1].leftblanks; } catch { }
                                    if (before1 == "|" && before2 == "|")
                                    {
                                        //do nothing, could be [1||%x]
                                    }
                                    else
                                    {
                                        string x = before1;  //a%s --> "a"
                                        if (glue == 0 && before1 == "|") x = before2;  //a|%s --> "a"
                                        char c = x[x.Length - 1];
                                        if (G.IsLetterOrDigitOrUnderscoreOrExclamation(c) || c == '}')
                                        {
                                            //  a%s
                                            //  a|%s
                                            //  }%s
                                            //  }|%s
                                            overriding = true;
                                        }
                                    }
                                }
                            }
                        }
                        catch { }

                        // =====================
                        // =====================
                        // =====================

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

                        //make sure that  #m inside [] are NEVER ever upgraded                                                
                        foreach (TokenHelper th in nesting)
                        {
                            //never upgrade if already inside [], like sum(#i, x[#i]). Also covers expression inside {}.
                            if (th.s == "[")
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

            if (G.Equal(line[pos0].s, "accept"))
            {
                try
                {
                    string sigil = "%";
                    if (G.Equal(line[pos0 + 1].s, "list")) sigil = "#";
                    line[pos0 + 2].s = sigil + line[pos0 + 2].s;
                }
                catch { }
            }

            else if (G.Equal(line[pos0].s, "compare"))
            {
                //¤019
                AddComment(line, "COMPARE has changed syntax, see the help files");
            }

            else if (G.Equal(line[pos0].s, "collapse") || G.Equal(line[pos0].s, "interpolate"))
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
                Tuple<int, int> tup = FindOptionField(line);
                if (tup.Item1 != -12345 && tup.Item2 != -12345)
                {
                    for (int i11 = tup.Item1 + 1; i11 < tup.Item2; i11++)
                    {
                        if (G.Equal(line[i11].s, "from") && line[i11 + 1].s == "=") line[i11].s = "frombank";
                        else if (G.Equal(line[i11].s, "to") && line[i11 + 1].s == "=") line[i11].s = "tobank";
                    }
                }
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
                //¤022 ¤041
                string name = line[pos0 + 1].s;
                if (!info.scalarMemory.ContainsKey("%" + name)) info.scalarMemory.Add("%" + name, "date");

                if (StringTokenizer.Equal(line, 2, "="))
                {
                    HandleCommandNameLeftSide(line, pos0, "%", info);
                    line[pos0 + 1].leftblanks = 0;
                }

                if (StringTokenizer.Equal(line, pos0 + 1, "?"))
                {
                    line[pos0].s = "";
                    line[pos0 + 1].s = "prt ";
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
                //AddComment(line, "For EXPORT without dates, use EXPORT<all>");
                try
                {
                    bool dates = true;
                    Tuple<int, int> tup = FindOptionField(line);
                    if (tup.Item1 != -12345 && tup.Item2 != -12345)
                    {
                        for (int i11 = tup.Item1; i11 <= tup.Item2; i11++)
                        {
                            if (G.Equal(line[i11].s, "ser")) line[i11].s = "flat";  //¤0040
                            else if (G.Equal(line[i11].s, "series")) line[i11].s = "gcm";  //¤0040                            
                        }
                    }
                }
                catch { }

                try
                {                    
                    HandleImportExportAll(line);
                }
                catch { }
            }

            else if (G.Equal(line[pos0].s, "function"))
            {
                AddComment(line, "Note: you may need to adjust the syntax, cf. FUNCTION.");
            }

            else if (G.Equal(line[pos0].s, "procedure"))
            {
                AddComment(line, "Note: you may need to adjust the syntax, cf. PROCEDURE.");
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
                //AddComment(line, "For IMPORT without dates, use IMPORT<all>");
                try
                {
                    Tuple<int, int> tup = FindOptionField(line);
                    if (tup.Item1 != -12345 && tup.Item2 != -12345)
                    {
                        for (int i11 = tup.Item1; i11 <= tup.Item2; i11++)
                        {
                            if (G.Equal(line[i11].s, "ser")) line[i11].s = "flat";  //¤0040
                        }
                    }
                }
                catch { }

                try
                {
                    HandleImportExportAll(line);
                }
                catch { }
            }

            else if (G.Equal(line[pos0].s, "index"))
            {
                TokenHelper last = line[line.Count - 2];  //remember semicolon
                int i5 = -12345;
                for (int i = 0; i < line.Count; i++)
                {
                    if (G.Equal(line[i].s, "listfile"))
                    {
                        i5 = i;
                        break;
                    }
                }

                if (i5 == -12345)
                {
                    //¤025                    
                    if (G.IsIdentTranslate(last.s) && last.leftblanks > 0)
                    {
                        last.s = "to global:#" + last.s;
                    }
                }
                else
                {
                    line[i5].s = "to #(" + line[i5].s;
                    last.s = last.s + ")";
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
                HandleCommandNameListElements(line, list, isParallel, info);

                if (list)
                {
                    if (StringTokenizer.Equal(line, pos0 + 1, "?"))
                    {
                        line[pos0].s = "";
                        line[pos0 + 1].s = "prt ";
                    }
                    else if (line[pos0].s.ToLower().Contains("null"))
                    {
                        //hacky...
                        line[pos0].s = G.Replace(line[pos0].s, "null ,", "list()", StringComparison.OrdinalIgnoreCase, 1);
                        line[pos0].s = G.Replace(line[pos0].s, "null,", "list()", StringComparison.OrdinalIgnoreCase, 1);
                        line[pos0].s = G.Replace(line[pos0].s, "null", "list()", StringComparison.OrdinalIgnoreCase, 1);

                    }
                }
            }

            else if (G.Equal(line[pos0].s, "matrix"))
            {
                //¤029
                line[pos0].meta.commandName = "matrix";

                string name = line[pos0 + 1].s;
                if (!info.collectionMemory.ContainsKey("#" + name)) info.collectionMemory.Add("#" + name, "matrix");

                if (StringTokenizer.Equal(line, 2, "="))
                {
                    HandleCommandNameLeftSide(line, pos0, "#", info);
                    line[pos0 + 1].leftblanks = 0;
                }

                if (StringTokenizer.Equal(line, pos0 + 1, "?"))
                {
                    line[pos0].s = "";
                    line[pos0 + 1].s = "prt ";
                }
            }

            else if (G.Equal(line[pos0].s, "open"))
            {
                Tuple<int, int> tup = FindOptionField(line);
                if (tup.Item1 != -12345 && tup.Item2 != -12345)
                {
                    for (int i11 = tup.Item1 + 1; i11 < tup.Item2; i11++)
                    {
                        if (G.Equal(line[i11].s, "sec")) line[i11].s = "pos=2";
                    }
                }
            }

            else if (G.Equal(line[pos0].s, "option"))
            {
                //option bugfix download = yes; --> remvoed
                //option bugfix px = yes; --> removed
                //option databank compare tabs = 1.0; --> removed, see COMPAre
                //option databank compare trel = 0.0001; --> removed, see COMPARE
                //option databank create message = yes; --> remvoed
                //option databank logic = default; --> remvoed
                //option importexport = no; --> removed
                //option interface databank swap = no; --> removed
                //option interface table printcodes = yes; --> "operators"
                //option library file = [empty]; --> removed
                //option plot new = yes; --> removed
                //option print filewidth = 130; --> removed
                //option series array ignoremissing = no; --> note that new options regarding this

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "bugfix") && G.Equal(line[pos0 + 2].s, "download"))
                    {
                        line[pos0].s = "//" + line[pos0].s;
                        AddComment(line, "Option obsolete");
                    }
                }
                catch { }

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "bugfix") && G.Equal(line[pos0 + 2].s, "px"))
                    {
                        line[pos0].s = "//" + line[pos0].s;
                        AddComment(line, "Option obsolete");
                    }
                }
                catch { }

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "databank") && G.Equal(line[pos0 + 2].s, "compare") && G.Equal(line[pos0 + 3].s, "tabs"))
                    {
                        line[pos0].s = "//" + line[pos0].s;
                        AddComment(line, "Option obsolete, use COMPARE command");
                    }
                }
                catch { }

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "databank") && G.Equal(line[pos0 + 2].s, "compare") && G.Equal(line[pos0 + 3].s, "trel"))
                    {
                        line[pos0].s = "//" + line[pos0].s;
                        AddComment(line, "Option obsolete, use COMPARE command");
                    }
                }
                catch { }

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "databank") && G.Equal(line[pos0 + 2].s, "create") && G.Equal(line[pos0 + 3].s, "message"))
                    {
                        line[pos0].s = "//" + line[pos0].s;
                        AddComment(line, "Option not implemented in Gekko 3.0 yet");
                    }
                }
                catch { }

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "databank") && G.Equal(line[pos0 + 2].s, "logic"))
                    {
                        line[pos0].s = "//" + line[pos0].s;
                        AddComment(line, "Option obsolete");
                    }
                }
                catch { }

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "importexport"))
                    {
                        line[pos0].s = "//" + line[pos0].s;
                        AddComment(line, "Option obsolete");
                    }
                }
                catch { }

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "interface") && G.Equal(line[pos0 + 2].s, "databank") && G.Equal(line[pos0 + 3].s, "swap"))
                    {
                        line[pos0].s = "//" + line[pos0].s;
                        AddComment(line, "Option obsolete");
                    }
                }
                catch { }

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "interface") && G.Equal(line[pos0 + 2].s, "table") && G.Equal(line[pos0 + 3].s, "printcodes"))
                    {
                        line[pos0 + 3].s = "operators";
                    }
                }
                catch { }

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "library") && G.Equal(line[pos0 + 2].s, "file"))
                    {
                        line[pos0].s = "//" + line[pos0].s;
                        AddComment(line, "Option obsolete");
                    }
                }
                catch { }

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "plot") && G.Equal(line[pos0 + 2].s, "new"))
                    {
                        line[pos0].s = "//" + line[pos0].s;
                        AddComment(line, "Option obsolete");
                    }
                }
                catch { }

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "print") && G.Equal(line[pos0 + 2].s, "filewidth"))
                    {
                        line[pos0].s = "//" + line[pos0].s;
                        AddComment(line, "Option obsolete");
                    }
                }
                catch { }

                try
                {
                    if (G.Equal(line[pos0 + 1].s, "series") && G.Equal(line[pos0 + 2].s, "array") && G.Equal(line[pos0 + 2].s, "ignoremissing"))
                    {
                        line[pos0].s = "//" + line[pos0].s;
                        AddComment(line, "Use 'option series array calc/print/table missing = ...' instead");
                    }
                }
                catch { }

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
                HandleCommandNameSeries(line, pos0, info);
            }

            else if (G.Equal(line[pos0].s, "val"))
            {
                //¤022 ¤041
                string name = line[pos0 + 1].s;
                if (!info.scalarMemory.ContainsKey("%" + name)) info.scalarMemory.Add("%" + name, "val");

                if (StringTokenizer.Equal(line, 2, "="))
                {
                    HandleCommandNameLeftSide(line, pos0, "%", info);
                    line[pos0 + 1].leftblanks = 0;
                }

                if (StringTokenizer.Equal(line, pos0 + 1, "?"))
                {
                    line[pos0].s = "";
                    line[pos0 + 1].s = "prt ";
                }
            }

            else if (G.Equal(line[pos0].s, "name") || G.Equal(line[pos0].s, "string"))
            {
                //¤022 ¤041
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
                    HandleCommandNameLeftSide(line, pos0, "%", info);
                    line[pos0 + 1].leftblanks = 0;
                }

                if (StringTokenizer.Equal(line, pos0 + 1, "?"))
                {
                    line[pos0].s = "";
                    line[pos0 + 1].s = "prt ";
                }
            }

            SetLineStartRecursive(line, line);

        }

        private static void HandleImportExportAll(List<TokenHelper> line)
        {
            bool? dates = null;
            Tuple<int, int> tup = FindOptionField(line);            
            if (tup.Item1 != -12345 && tup.Item2 != -12345)
            {
                if (tup.Item2 - tup.Item1 - 1 < 2)
                {
                    dates = false;
                }
                else
                {
                    dates = true;  //possibly...
                    int datesCounter = 0;
                    TokenHelper token1 = null;
                    TokenHelper token2 = null;
                    for (int i11 = tup.Item1 + 1; i11 <= tup.Item2 - 1; i11++)
                    {
                        datesCounter++;
                        if (datesCounter == 1)
                        {
                            token1 = line[i11];
                        }
                        else if (datesCounter == 2)
                        {
                            token2 = line[i11];
                            if (G.IsIdentTranslate(token1.ToStringTrim()) && (G.IsIdentTranslate(token2.ToStringTrim()) || token2.ToStringTrim() == "="))
                            {
                                //something like <opt1 opt2 ...> or <opt1 = ...>. Cannot be dates.
                                //remember 2020 or 2020q1 are not idents, neither %t1.
                                dates = false;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                dates = false;
            }

            if (dates == false)
            {
                //We must add a <all>. Done in a hacky way.
                if (tup.Item1 == -12345 && tup.Item2 == -12345)
                {
                    line[0].s = line[0].s + "<all>";
                }
                else
                {
                    line[tup.Item2 - 1].s = line[tup.Item2 - 1].s + " all";
                }
            }
        }

        private static void HandleCommandNameLeftSide(List<TokenHelper> line, int pos0, string sigil, Info info)
        {
            //see also #09835324985
            string extra = null;
            if (info.keepTypes) extra = line[pos0].s + " ";
            if (info.useGlobalBankForNonSeries)
            {
                line[pos0].s = extra + "global:" + sigil;
                info.globalCounter++;
            }
            else line[pos0].s = extra + sigil;
        }

        private static void HandleCommandNameSeries(List<TokenHelper> line, int pos, Info info)
        {
            //¤032

            string seriesString = line[0].s;  //series or SERIES or ser etc.

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

                //dynamic <dyn>
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
                    try
                    {
                        //comma in real input, not inside function etc. --> no need to set parentheses
                        //line[op_i].s += " (";
                        //line[line.Count - 1].s = ")" + line[line.Count - 1].s;
                        for (int iii = op_i + 1; iii < line.Count; iii++)
                        {
                            if (G.Equal(line[iii].s, "m")) line[iii].s = "m()";
                        }
                    } catch { }
                }
                else
                {
                    //handle rep, there is no comma
                    try
                    {
                        for (int iii = op_i + 1; iii < line.Count; iii++)
                        {
                            if (G.Equal(line[iii].s, "rep") && G.Equal(line[iii + 1].s, "*"))
                            {
                                line[iii].s = "";
                                line[iii + 1].s = "";
                                line[iii + 1].leftblanks = 0;
                            }
                        }
                    } catch { }
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

                try
                {
                    //stuff like series x = 1 m 1e7 -2
                    if (op_i != -12345)
                    {
                        bool ok = true;
                        int last = -12345;
                        for (int i9 = op_i + 1; i9 < line.Count; i9++)
                        {
                            if (line[i9].s == ";")
                            {
                                //good
                            }
                            else if (line[i9].s == "-")
                            {
                                //good
                            }
                            else if (G.Equal(line[i9].s, "m"))
                            {
                                //good
                            }
                            else if (line[i9].type == ETokenType.Number)
                            {
                                //good
                                last = i9;
                            }
                            else if (line[i9].type == ETokenType.EOL || line[i9].type == ETokenType.Comment || line[i9].type == ETokenType.WhiteSpace)
                            {
                                //ok
                            }
                            else ok = false;
                        }
                        if (ok && last != -12345)
                        {
                            for (int i9 = op_i + 1; i9 < line.Count; i9++)
                            {
                                if (line[i9].type == ETokenType.Number || G.Equal(line[i9].s, "m"))
                                {
                                    if (G.Equal(line[i9].s, "m")) line[i9].s = "m()";
                                    if (i9 != last)
                                    {
                                        line[i9].s = line[i9].s + ",";
                                    }
                                }
                            }
                        }
                    }
                } catch { }

            }

            //move the option field
            MoveOptionField(line, lb);

            if (info.keepTypes)
            {
                line[0].s = seriesString + " " + line[0].s;
            }
        }

        private static void HandleCommandNameListElements(List<TokenHelper> line, bool list, bool isParallel, Info info)
        {
            //¤028
            List<TokenHelper> l1 = new List<TokenHelper>();
            List<TokenHelper> l2 = new List<TokenHelper>();
            List<TokenHelper> l3 = new List<TokenHelper>();

            List<TokenHelper> comments = new List<TokenHelper>();
            foreach (TokenHelper th in line)
            {
                if (th.s.Contains(" /* TRANSLATE: "))
                {
                    TokenHelper th2 = new TokenHelper(th.s);
                    comments.Add(th2);
                    th.s = "";
                }
            }

            string result1 = "";
            string result2 = "";
            string result3 = "";

            string[] keywords = new string[] { "prefix", "suffix", "trim", "sort", "strip" };

            int i1 = StringTokenizer.FindS(line, "=");
            if (i1 > -12345)
            {
                for (int i = 0; i <= i1; i++) l1.Add(line[i]);
                int i2 = StringTokenizer.FindS(line, i1 + 1, keywords);
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
                    string extra = null;
                    if (info.keepTypes) extra = l1[0].s + " ";

                    if (StringTokenizer.Equal(l1, 1, "listfile"))
                    {
                        //list listfile m = ...  --> #(listfile m) = ... 
                        result1 = extra + "#(listfile ";

                        int counter = 0;
                        foreach (TokenHelper th in l1)
                        {
                            counter++;
                            if (counter > 2)
                            {
                                if (th.s == "=") result1 += ") =";
                                else result1 += th.ToString();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i < l1.Count; i++) result1 += l1[i].ToString();
                        //see also #09835324985                        
                        if (info.useGlobalBankForNonSeries)
                        {
                            result1 = extra + "global:" + "#" + result1.TrimStart();
                            info.globalCounter++;
                        }
                        else result1 = extra + "#" + result1;
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
                    if (l3[i].s.ToLower() == "prefix")
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
                    else if (l3[i].s.ToLower() == "suffix")
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
                    else if (l3[i].s.ToLower() == "trim")
                    {
                        iSpecial = i;
                        l3[i].leftblanks = 0;
                        l3[i].s = "." + "unique" + "(";
                        l3[j].s = ")" + l3[j].s;
                    }
                    else if (l3[i].s.ToLower() == "sort")
                    {
                        iSpecial = i;
                        l3[i].leftblanks = 0;
                        l3[i].s = "." + l3[i].s + "(";
                        l3[j].s = ")" + l3[j].s;
                    }
                    else if (l3[i].s.ToLower() == "strip")
                    {
                        iSpecial = i;
                        l3[i].leftblanks = 0;
                        l3[i].s = "." + "replaceinside" + "(";
                        l3[i + 1].s = "";
                        l3[j].s = ", '')" + l3[j].s;
                    }
                }

                if (true)
                {
                    bool omitQuotes = false;
                    for (int i = 0; i < l3.Count; i++)
                    {                       

                        string ss = l3[i].ToStringTrim();

                        if (ss.StartsWith(")") || ss.StartsWith(","))
                        {
                            if (!omitQuotes) ss = "'" + ss;
                            omitQuotes = false;
                        }

                        if (ss.EndsWith("("))  //for instance "prefix("
                        {
                            string scout = null;
                            for (int ii = i + 1; ii < l3.Count; ii++)
                            {
                                if (l3[ii].ToStringTrim() == "") continue;
                                scout = l3[ii].ToStringTrim();
                                break;
                            }

                            if (scout.StartsWith("%") || scout.StartsWith("'") || scout.Contains(")")) omitQuotes = true;
                            if (!omitQuotes) ss = ss + "'";
                        }
                        

                        //if (keywords.Contains(ss.ToLower()) || !G.IsIdentTranslate(ss))
                        //{
                        //    //do not touch keywords like prefix, suffix, etc.
                        //    //do not touch anything like %x, {%x} etc.
                        //    s11 = ss;
                        //}
                        //else
                        //{
                        //    s11 = "'" + ss + "'";  //add plings
                        //}

                        result3 += ss;
                    }
                }

                //test if simple. Simple is stuff like a, b, c5, 0d, 1, 2, #s, #m.
                
                bool simple = true;
                for (int ij = 0; ij < items.Count; ij++)
                {
                    string s7 = items[ij].Trim();
                    if (s7.StartsWith("'") && s7.EndsWith("'")) continue;
                    if (s7.StartsWith("{") && s7.EndsWith("}"))
                    {
                        s7 = s7.Substring(1, s7.Length - 2);
                    }                    
                    bool first = true;
                    foreach (char c in s7)
                    {
                        if (G.IsLetterOrDigitOrUnderscore(c) || (first && (c == '%' || c == '#')))
                        {
                            //good: a, b, 007, a38, 7z, %i, #i, 'xx'
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
                        if (items[0].StartsWith("#"))
                        {
                            //do not translate list m = #mm --> #m = {#mm},;
                            result2 = itemsExtra[0] + items[0];
                        }
                        else
                        {
                            result2 = itemsExtra[0] + UpgradeString(items[0], line) + ",";
                        }
                    }
                    else
                    {
                        bool first = true;
                        for (int ij = 0; ij < items.Count; ij++)
                        {
                            string s2 = items[ij];
                            if (!first) result2 += ",";
                            result2 += itemsExtra[ij] + UpgradeString(s2, line);
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
                    if (!line[i].s.Contains(" /* TRANSLATE: "))
                    {
                        //hack to avoid a comment being zapped
                        line[i].s = ""; line[i].leftblanks = 0; line[i].subnodes = null;
                    }                  
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

                //if(isParallel) AddComment(line, "Parallel loops may not be translated properly, including missing {}-curlies on elements");
            }

            foreach (TokenHelper th in comments)
            {
                line.Add(th);  //get them in again
            }
        }

        /// <summary>
        /// Transforms %x --> {%x} and #m --> {#m}
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string UpgradeString(string s, List<TokenHelper> line)
        {
            string s5 = s.Trim();
            if (s5.StartsWith("'") && s5.EndsWith("'"))
            {
                string s6 = s5.Substring(1, s5.Length - 2);
                bool good = true;
                foreach (char c in s6)
                {
                    if(G.IsLetterOrDigitOrUnderscore(c))
                    {
                        //good
                    }
                    else
                    {
                        good = false;
                    }
                }
                if (good) return s6;
                else
                {                    
                    AddComment(line, "One or more elements are quoted ('). You should use a list definition with parentheses, like #m = (...). For instance: list m = a, 'b', 'c d'; becomes #m = ('a', 'b', 'c d');.");                    
                    return s5;
                }
            }            
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

                //kind of hacky
                line[0].parent.subnodes.storage.Clear();
                line[0].parent.subnodes.storage.AddRange(line);
                line[0].parent.OrganizeSubnodes();  //because a lot has been moved around

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

        /// <summary>
        /// Searches for &lt; and >. Returns the indexes corresponding to these.
        /// </summary>
        /// <param name="line2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// test how the translation performs (internal use)
        /// </summary>
        public static void TestTranslation()
        {

            Insert();

            //string fname = @"c:\Thomas\Desktop\gekko\testing\Translate\05banker\afledte.tcm";            
            //string contents = Program.GetTextFromFileWithWait(fname);
            //string fname2 = fname + "_bitness";
            //using (FileStream fs = Program.WaitForFileStream(fname2, Program.GekkoFileReadOrWrite.Write))
            //using (StreamWriter file = G.GekkoStreamWriter(fs))
            //{
            //    file.Write(contents);
            //}
            //return;

            //string folder = @"c:\Thomas\Desktop\gekko\testing\Translate";
            //WalkInfo wi = new WalkInfo();
            ////WalkFolderHelper(new DirectoryInfo(folder), wi);
            //WalkFolderHelper2(new DirectoryInfo(folder), wi);
            //new Writeln("All lines = " + wi.lines + ", error lines = " + wi.errorLines + " ratio = " + (double)wi.errorLines / (double)wi.lines);
            //new Writeln("Files = " + wi.filesCounter + ", bad files = " + wi.badFilesCounter + ", whole bad files = " + wi.wholeBadFilesCounter);
            

            //StringBuilder sb = new StringBuilder();
            //foreach (string s in wi.storage)
            //{
            //    sb.AppendLine(s);
            //}
            //File.WriteAllText(@"c:\tools\abc123.txt", sb.ToString());

        }

        public static void Insert()
        {
            //kaldes med tell'datopgek3_agh7xvslke3jfhqp';

            string temp = @"g:\datopgek_temp";
            string g3 = @"g:\datopgek3";
            string ext = ".gcm_gek2";            

            string logfile = Program.options.folder_working + "\\insert_log.txt";

            if (!Directory.Exists(temp))
            {
                new Error("Directory '" + temp + "' does not exist");
            }

            if (!Directory.Exists(g3))
            {
                new Error("Directory '" + g3 + "' does not exist");
            }

            new Writeln("From directory: " + temp);
            new Writeln("To directory: " + g3);
            new Writeln("Insert started... ");

            List<string> log = new List<string>();
            List<string> files = GetList();

            int good = 0;
            int bad = 0;
            foreach (string file in files)
            {
                string fileWithTempPath = temp + "\\" + file;
                string fileWithG3Path = g3 + "\\" + file;

                if (File.Exists(fileWithG3Path))
                {
                    try
                    {
                        string newFileName = Path.ChangeExtension(fileWithG3Path, ext);
                        File.Move(fileWithG3Path, newFileName); //rename existing from .gcm to .gcm_gekko2
                        File.Copy(fileWithTempPath, fileWithG3Path);
                        log.Add("OK. File [ROOT]\\'" + file + "' inserted.");
                        good++;
                    }
                    catch
                    {
                        log.Add("*** ERROR: Unexpected exception regarding file [ROOT]\\'" + file + "'.");
                        bad++;
                    }
                }
                else
                {
                    log.Add("*** ERROR: Could not move file [ROOT]\\'" + file + "'. Corresponding file or folder not found");
                    bad++;
                }
            }
            new Writeln("... insert ended");
            new Writeln("There are " + files.Count + " translated files. " + good + " files were copied successfully, with " + bad + " errors.");
            new Writeln("The old files are renamed from *.gcm til *" + ext + ".");
            new Writeln("Remember to insert a root.ini at the root of the new system.");

            using (FileStream fs = Program.WaitForFileStream(logfile, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter file2 = G.GekkoStreamWriter(fs))
            {
                foreach (string s in log)
                {
                    file2.WriteLine(s);
                }
            }
            new Writeln("You may inspect the log-file here: " + logfile);
        }

        public static List<string> GetList()
        {
            List<string> m = new List<string>()
            {
                @"periode.gcm",
@"periode_Gv3.gcm",
@"05banker\afledte.gcm",
@"05banker\datop.gcm",
@"05banker\gekarem.gcm",
@"05banker\konsist.gcm",
@"05banker\lonsumsafgift.gcm",
@"05banker\lonsumsafgift2obk.gcm",
@"05banker\off26.gcm",
@"05banker\periode.gcm",
@"05banker\sdr.gcm",
@"05banker\sektor.gcm",
@"05banker\sektor.jun16.gcm",
@"05banker\skat.bak.gcm",
@"05banker\skat.gcm",
@"05banker\subsid.gcm",
@"05banker\tilobk.gcm",
@"05banker\totaltjek.gcm",
@"05banker\ty.09nov17.gcm",
@"05banker\ty.gcm",
@"05banker\tyny.gcm",
@"05banker\ty_tilbage.gcm",
@"05banker\undersek.gcm",
@"05banker\finansiel\datop.gcm",
@"05banker\finansiel\egen.bak.gcm",
@"05banker\finansiel\egen.bk1.gcm",
@"05banker\finansiel\egen.gcm",
@"05banker\finansiel\fkadamo.gcm",
@"05banker\finansiel\wobank1.gcm",
@"05banker\Lonsumsafgift\Lonsumsafgift.gcm",
@"05banker\Off10\TY2_undersek.gcm",
@"05banker\Off10\ty_tilbage.gcm",
@"05banker\Off17\datop.gcm",
@"05banker\Off17\subschk.gcm",
@"05banker\off3_26\datop.gcm",
@"05banker\off3_26\off26.gcm",
@"05banker\off3_26\off2620.gcm",
@"05banker\off3_26\off3.gcm",
@"05banker\off3_26\off320.gcm",
@"05banker\off3_26\off3chk.gcm",
@"05banker\pension\del_tilobk.gcm",
@"05banker\pension\s125.gcm",
@"05banker\pension\s125_20170811.gcm",
@"05banker\pension\s125_r3.gcm",
@"05banker\pension\tipOgWp_cf_x_tilobk.gcm",
@"05banker\pension\tip_tilobk.gcm",
@"05banker\pension\tip_tilps.gcm",
@"05banker\renter\datop.gcm",
@"05banker\Saerlige\datop.gcm",
@"05banker\SKAT\Regneark\Datop2.gcm",
@"05banker\SKAT\Regneark\skatchk.gcm",
@"05banker\SKAT\Regneark\skatialt.gcm",
@"05banker\Sywp\datop.gcm",
@"05banker\Sywp\datop_gl.gcm",
@"05banker\Sywp\datop_r.gcm",
@"05banker\Sywp\datop_r2_t2.1.3.gcm",
@"05banker\Tyændringer nov17\tilobk.gcm",
@"05banker\Tyændringer nov17\tjkobk.gcm",
@"05banker\Tyændringer nov17\Tyer.bak.gcm",
@"05banker\Tyændringer nov17\Tyer.gcm",
@"05banker\Tyændringer nov17\tytilbage.bak.gcm",
@"05banker\Tyændringer nov17\tytilbage.gcm",
@"abase\abase_cq.gcm",
@"abase\abase_hq.gcm",
@"abase\abase_q.gcm",
@"abase\abase_v.gcm",
@"abase\abase_xspz.gcm",
@"abase\abase_yw.gcm",
@"abase\abase_zq.gcm",
@"abase\afledte.gcm",
@"abase\a_key_c.gcm",
@"abase\a_key_hq.gcm",
@"abase\a_key_q.gcm",
@"abase\a_key_v.gcm",
@"abase\a_key_x.gcm",
@"abase\bb_abaseq.gcm",
@"abase\DanFra_AbaseOgPaaOBK.gcm",
@"abase\Dan_Ess.gcm",
@"abase\datarev.gcm",
@"abase\inv_a.gcm",
@"abase\io1ting.gcm",
@"abase\kaede.gcm",
@"abase\load3_abq.gcm",
@"abase\MEMOQ.gcm",
@"abase\NytEndAar.gcm",
@"abase\ObkKlar2AREMOS.gcm",
@"abase\tilobk.gcm",
@"abase\tt_tjek.gcm",
@"abase\uh_abq.gcm",
@"abase\uh_abq_nmh.gcm",
@"abase_ny\AbaseRun.gcm",
@"abase_ny\lavarem117q.gcm",
@"afgift\afgift.gcm",
@"afgift\datarev.gcm",
@"afgift\efterinvkap.gcm",
@"afgift\efterinvkaptest.gcm",
@"afgift\histbk.gcm",
@"afgift\nrafgift.gcm",
@"afgift\pso.gcm",
@"afgift\psohist.gcm",
@"afgift\psotjk.gcm",
@"afgift\spg.gcm",
@"afgift\spggl.gcm",
@"afgift\spggltjk.gcm",
@"afgift\spgtjk.gcm",
@"afgift\spgwa.gcm",
@"afgift\spg_jnr.gcm",
@"afgift\spg_shg.gcm",
@"afgift\spm.gcm",
@"afgift\spmtjk.gcm",
@"afgift\spp.gcm",
@"afgift\spphist.gcm",
@"afgift\sppm.gcm",
@"afgift\sppmtjk.gcm",
@"afgift\spptjk.gcm",
@"afgift\spzejh.gcm",
@"afgift\tilobk.gcm",
@"afgift\basta17\okt20_eksemsam\Bastany.gcm",
@"afgift\bspz\bspz.gcm",
@"afgift\bspz\copy201802.gcm",
@"afgift\bspz\spzart13.gcm",
@"afgift\bspz\spzart14.gcm",
@"afgift\bspz\spzart15.gcm",
@"afgift\bspz\spzart16.gcm",
@"afgift\bspz\spzart17.gcm",
@"afgift\bspz\tilobk.gcm",
@"afgift\bspz\tilobk13.gcm",
@"afgift\bspz\tilobk14.gcm",
@"afgift\bspz\tjkspz.gcm",
@"afgift\histdata\histdata.gcm",
@"afgift\histdata\histdata_shg.gcm",
@"afgift\histdata\spr_co.gcm",
@"afgift\til_skm\basta2xlsx.gcm",
@"afgift\til_skm\basta2xlsx_m.gcm",
@"basis\basis.gcm",
@"basis\basis_step1.gcm",
@"basis\basis_step2.gcm",
@"basis\datop.gcm",
@"basis\invest_afl.gcm",
@"basis\o_basis_step1.gcm",
@"basis\o_basis_step2.gcm",
@"basis\preBASIS.gcm",
@"basis\til_obk.gcm",
@"basis\til_obk_hist.gcm",
@"basis\til_obk_per0ogper0-1.gcm",
@"basis\til_obk_per0ogper0-3.gcm",
@"Biver\alfa.gcm",
@"Biver\bivbp.gcm",
@"Biver\bivmp.gcm",
@"Biver\datrev.gcm",
@"Biver\datrev1.gcm",
@"Biver\datrev2.gcm",
@"Biver\datrev3.gcm",
@"Biver\datrev4.gcm",
@"Biver\datrev5.gcm",
@"Biver\ivps.gcm",
@"Biver\sy_c.gcm",
@"Biver\til_obk.gcm",
@"Biver\tsy_cu.gcm",
@"Biver\ivsk\histdata.gcm",
@"Biver\ivsk\til_obk.gcm",
@"DELFREK\deltid.gcm",
@"DELFREK\tilobk.gcm",
@"divbanker\Clear_bank.gcm",
@"divbanker\datarev.gcm",
@"divbanker\Datatjek_Overlap.gcm",
@"divbanker\misstjk.gcm",
@"divbanker\tilobk.gcm",
@"divbanker\aktiekurs\pws.gcm",
@"divbanker\aktiekurs\tildivbank.gcm",
@"divbanker\betbal\datrev.gcm",
@"divbanker\betbal\tildivbank.gcm",
@"divbanker\betbal\Tilbage\enlgl.gcm",
@"divbanker\load4\hostkor.gcm",
@"divbanker\load4\load4.gcm",
@"divbanker\load4\load4_shg.gcm",
@"divbanker\load4\tildivbank.gcm",
@"divbanker\load5\load5.gcm",
@"divbanker\load5\tildivbank.gcm",
@"divbanker\load6\load6.gcm",
@"divbanker\load6\tildivbank.gcm",
@"divbanker\load7\boligvar.gcm",
@"divbanker\load7\datarev.gcm",
@"divbanker\load7\knbhk_h.gcm",
@"divbanker\load7\lndeu.gcm",
@"divbanker\load7\load7.gcm",
@"divbanker\load7\tildivbank.gcm",
@"divbanker\natbankfk\datop.gcm",
@"divbanker\natbankfk\fkadamchk.gcm",
@"divbanker\omu\datarev.gcm",
@"divbanker\omu\tildivbank.gcm",
@"divbanker\Realkredit\datop.gcm",
@"divbanker\Realkredit\Gekko_nwlm.gcm",
@"divbanker\Realkredit\Gekko_wlm.gcm",
@"divbanker\Realkredit\lavwlm.gcm",
@"divbanker\Realkredit\Tiimkox.gcm",
@"divbanker\Realkredit\tildivbank.gcm",
@"divbanker\Realkredit\wlmkurs.gcm",
@"divbanker\Realkredit\wlmkurs2.gcm",
@"divbanker\Realkredit\bak\Tiimstump.gcm",
@"divbanker\Realkredit\Tiim\dantiimapril.gcm",
@"divbanker\Realkredit\Tiim\Tiimkox.gcm",
@"divbanker\slog\datrev.gcm",
@"divbanker\slog\pwbse.gcm",
@"divbanker\slog\slog.gcm",
@"divbanker\statslaan\datarev.gcm",
@"divbanker\Tilbageførsel\Datatjek1947_2017.gcm",
@"divbanker\Tilbageførsel\DIVBKNY.gcm",
@"divbanker\Tilbageførsel\rettelser_i_obk.gcm",
@"divbanker\udlandsgald\dntal\datop.gcm",
@"divbanker\udlandsgald\dntal\datopkox.gcm",
@"divbanker\vardipapir\datarev.gcm",
@"divbanker\vardipapir\tildivbank.gcm",
@"divbanker\vardipapir\nb\datarev.gcm",
@"divbanker\vardipapir\nb\dnvpdks.gcm",
@"Eksport\tilobk.gcm",
@"Eksport\tilobk20201117.gcm",
@"Eksport\bak\201703\tilobk.gcm",
@"Energi\datarevjao.gcm",
@"Energi\datop.gcm",
@"Energi\kox18.gcm",
@"Energi\kox18okt.gcm",
@"Energi\koxkox.gcm",
@"Energi\koxkox2.gcm",
@"Energi\pvekox.gcm",
@"Energi\pvekoxjao.gcm",
@"Energi\tilobk.gcm",
@"Energi\Endel\datop.gcm",
@"Energi\Endel\engny.bak2.gcm",
@"Energi\Endel\engny.gcm",
@"Energi\Endel\fvmpvm.gcm",
@"Energi\Endel\fvmpvm18.bak.gcm",
@"Energi\Endel\fvmpvm18.gcm",
@"Energi\Endel\kox18.gcm",
@"Energi\Endel\tilobk.gcm",
@"Energi\Endel\tilobkkox.gcm",
@"Energi\Endel\vetilbage.gcm",
@"Energi\Endel\bak\fvmpvm18.gcm",
@"fk\datop.BK2.gcm",
@"fk\datop.gcm",
@"fk\datop0.gcm",
@"fk\faketalapril.gcm",
@"fk\fkadam.gcm",
@"fk\fkadam.mol.gcm",
@"fk\fkadamchk.gcm",
@"fk\fkadamchk2.gcm",
@"fk\fkadamchko.gcm",
@"fk\fkadamplus.gcm",
@"fk\fkadampluso.gcm",
@"fk\gekarem.gcm",
@"fk\nbtal.bak.gcm",
@"fk\nbtal.f1.gcm",
@"fk\nbtal.gcm",
@"fk\nyetal.gcm",
@"fk\nyetal19.gcm",
@"fk\osrev.gcm",
@"fk\owfyldud.gcm",
@"fk\tilobk.gcm",
@"fk\wseer.gcm",
@"hoved\2arem.gcm",
@"hoved\axqzstump.gcm",
@"hoved\balancer.gcm",
@"hoved\datop.gcm",
@"hoved\faktorer.gcm",
@"hoved\faktorp.gcm",
@"hoved\faktorw.gcm",
@"hoved\fdil1.gcm",
@"hoved\hoved.gcm",
@"hoved\hoved0.gcm",
@"hoved\import0.gcm",
@"hoved\imptil_obk.gcm",
@"hoved\Klar2Estbk.gcm",
@"hoved\lager.gcm",
@"hoved\lister.gcm",
@"hoved\prissamdata.gcm",
@"hoved\prissamdata2.gcm",
@"hoved\prissamf.gcm",
@"hoved\tmmer.gcm",
@"hoved\udvid.gcm",
@"hoved\xqzxco.bak.gcm",
@"hoved\xqzxco.gcm",
@"IMPEKS\abasafst.gcm",
@"IMPEKS\abaseagg.gcm",
@"IMPEKS\adamkfu.gcm",
@"IMPEKS\afstemm.gcm",
@"IMPEKS\bunkringm.gcm",
@"IMPEKS\datop.gcm",
@"IMPEKS\evmvfrem.gcm",
@"IMPEKS\figurer.gcm",
@"IMPEKS\fraadambk.gcm",
@"IMPEKS\gekarem.gcm",
@"IMPEKS\genrvar.gcm",
@"IMPEKS\gmdatrev.gcm",
@"IMPEKS\kfulabel.gcm",
@"IMPEKS\lavess.gcm",
@"IMPEKS\lavfemv.gcm",
@"IMPEKS\mdatrev.gcm",
@"IMPEKS\mtilsim.gcm",
@"IMPEKS\nr_ind.gcm",
@"IMPEKS\tilobk.gcm",
@"IMPEKS\totaler1.gcm",
@"IMPEKS\udrind.gcm",
@"IMPEKS\udrogbop_ind.gcm",
@"IMPEKS\UH_til_NR.gcm",
@"IMPEKS\visalt.gcm",
@"IMPEKS\BAK\logs.gcm",
@"IMPEKS\data\datop.gcm",
@"IMPEKS\KT12PRIS\Gek\tilGek.gcm",
@"IMPEKS\Sammenligning\graf.gcm",
@"IMPEKS\Sammenligning\hjemmemarked\hjemme.gcm",
@"IMPEKS\Sammenligning\nyUVI\a_copy_uhvppi.gcm",
@"IMPEKS\Sammenligning\nyUVI\sletmig.gcm",
@"IMPEKS\Sammenligning\nyUVI\impeks\udrogbop_ind.gcm",
@"invkap\ADAM72.gcm",
@"invkap\copy201802.gcm",
@"invkap\datarev.gcm",
@"invkap\datatjek.gcm",
@"invkap\eudbk.gcm",
@"invkap\fknbhle.gcm",
@"invkap\foraar2.gcm",
@"invkap\ifo1.gcm",
@"invkap\ifo1april2018.gcm",
@"invkap\kapadam.gcm",
@"invkap\listart.gcm",
@"invkap\lister.gcm",
@"invkap\listnr72.gcm",
@"invkap\myfunctions.gcm",
@"invkap\prisret.gcm",
@"invkap\pristjek.gcm",
@"invkap\rpipe.gcm",
@"invkap\tilobk.gcm",
@"invkap\bhew\bhew.gcm",
@"invkap\biler\bildata.gcm",
@"invkap\biler\datarev.gcm",
@"invkap\biler\myfunctions.gcm",
@"IO\axqzstump.gcm",
@"IO\celbk.gcm",
@"IO\celbk2017_komplet.gcm",
@"IO\celbk_komplet.gcm",
@"IO\DanEss.gcm",
@"IO\DanEssKomp_Eaar.gcm",
@"IO\DanEssKomp_Faar.gcm",
@"IO\DanEssKomp_Faar_gl.gcm",
@"IO\Dan_Ess_bop.gcm",
@"IO\datop_ess.gcm",
@"IO\datop_iokoef.gcm",
@"IO\efteriokoef.gcm",
@"IO\ess_nr_66t12.gcm",
@"IO\ess_nr_66t13.gcm",
@"IO\ess_nr_66t14.gcm",
@"IO\ess_nr_66t15.gcm",
@"IO\ess_nr_66t16.gcm",
@"IO\ess_nr_66t17.gcm",
@"IO\ess_nr_66t18.gcm",
@"IO\FletO_celbkPaaCelbk.gcm",
@"IO\iobank.gcm",
@"IO\iokoef.gcm",
@"IO\iokoef_gl.gcm",
@"IO\iokoef_tilobk.gcm",
@"IO\o_celbk.gcm",
@"IO\o_iobank.gcm",
@"IO\o_xqzxco_io_EndAar.gcm",
@"IO\til_celbk_fraOgMed2017.gcm",
@"IO\til_histcelbk_per0.gcm",
@"IO\til_histcelbk_per0plus.gcm",
@"IO\til_histcelbk_per0plus2017.gcm",
@"IO\til_obk.gcm",
@"IO\til_obk_2012.gcm",
@"IO\til_obk_per0.gcm",
@"IO\til_obk_per0ogper0-1.gcm",
@"IO\til_obk_per0tilbage til1995.gcm",
@"IO\til_obk_per0tilbagetil1995.gcm",
@"IO\TjekEogMSPMceller2010-.gcm",
@"IO\TjekEogMSPMceller2010-.raa.gcm",
@"IO\tjek_o_celbk.gcm",
@"IO\xqzxco_io.bak.gcm",
@"IO\xqzxco_io.gcm",
@"IO\xqzxco_io_EndAar.gcm",
@"IO\xqzxco_tom20180718.gcm",
@"IO\prog\celbk_et_fraAREMOS20172gekko.gcm",
@"IO\prog\celbk_et_fraAREMOS2gekko.gcm",
@"IO\prog\celbk_et_fraAREMOS2gekko2017.gcm",
@"IO\prog\lister.gcm",
@"IO\prog\lister2017.gcm",
@"IO\prog\lister_raa.gcm",
@"IO\sideordnet\celbk_et_fraAREMOS2gekko.gcm",
@"IO\sideordnet\celbk_komplet.gcm",
@"IO\sideordnet\til_histcelbk_per0.gcm",
@"KONTROL\2fordel2.gcm",
@"KONTROL\2fordel3.gcm",
@"KONTROL\DanAnsvLister.gcm",
@"KONTROL\DanModulUdLister.gcm",
@"KONTROL\datakontrol_pension.gcm",
@"KONTROL\fordel3.gcm",
@"KONTROL\klar2mistjek_jul17.gcm",
@"KONTROL\klar2mistjek_okt15r.gcm",
@"KONTROL\klar2mistjek_okt16.gcm",
@"KONTROL\klar2mistjek_okt18.gcm",
@"KONTROL\kontrolYd_okt15.gcm",
@"KONTROL\kontrol_energi.gcm",
@"KONTROL\Kontrol_ov.gcm",
@"KONTROL\mistjek_jul17.gcm",
@"KONTROL\mistjek_okt15r.gcm",
@"KONTROL\mistjek_okt15r_2008-2012.gcm",
@"KONTROL\mistjek_okt16.gcm",
@"KONTROL\tjekpvar.gcm",
@"KONTROL\t_fordel2.gcm",
@"KONTROL\t_fordel3.gcm",
@"KONTROL\tt\Kontrol_ov.gcm",
@"lager\apr20\afstem.gcm",
@"lager\apr20\danexolager.gcm",
@"lager\apr20\Datarev.gcm",
@"lager\apr20\grund.gcm",
@"lager\apr20\lala.gcm",
@"lager\apr20\lavbank.gcm",
@"lager\apr20\periode.gcm",
@"lager\apr20\til_obk.gcm",
@"lager\okt18\afstem.gcm",
@"lager\okt18\danexolager.gcm",
@"lager\okt18\Datarev.gcm",
@"lager\okt18\grund.gcm",
@"lager\okt18\lala.gcm",
@"lager\okt18\lavbank.gcm",
@"lager\okt18\periode.gcm",
@"lager\okt18\til_obk.gcm",
@"lager\okt20\afstem.gcm",
@"lager\okt20\danexolager.gcm",
@"lager\okt20\Datarev.gcm",
@"lager\okt20\grund.gcm",
@"lager\okt20\lala.gcm",
@"lager\okt20\lavbank.gcm",
@"lager\okt20\periode.gcm",
@"lager\okt20\tilobk.gcm",
@"lager\til_okt16\afstem.gcm",
@"lager\til_okt16\danexolager.gcm",
@"lager\til_okt16\Datarev.gcm",
@"lager\til_okt16\grund.gcm",
@"lager\til_okt16\lala.gcm",
@"lager\til_okt16\lavbank.gcm",
@"lager\til_okt16\periode.gcm",
@"lager\til_okt16\til_obk.gcm",
@"LISTER\lister.gcm",
@"nrbanker\DanAbase97.gcm",
@"nrbanker\efternrbanker.gcm",
@"nrbanker\lav_abase97.gcm",
@"nrbanker\NRdatarev.gcm",
@"nrbanker\NRdatarev_old.gcm",
@"nrbanker\Perioder.gcm",
@"nrbanker\apisumdb\gosumdb.gcm",
@"nrbanker\apisumdb\sumdb.gcm",
@"nrbanker\apisumdb\sumdbpx.gcm",
@"nrbanker\divnr\datarev.gcm",
@"nrbanker\divnr\datarev_nof1.gcm",
@"nrbanker\divnr\datarev_xo1i.gcm",
@"nrbanker\divnr\datarev_xo1_p.gcm",
@"nrbanker\divnr\fou.gcm",
@"nrbanker\divnr\nof1.gcm",
@"nrbanker\divnr\pxo1i.gcm",
@"nrbanker\divnr\test_tilobk.gcm",
@"nrbanker\divnr\tilobk.gcm",
@"nrbanker\divnr\tilobk_nof1.gcm",
@"nrbanker\divnr\tilobk_xo1i.gcm",
@"nrbanker\divnr\xo1_p.gcm",
@"nrbanker\divnr\histpxo1i\til_obkhist.gcm",
@"nrbanker\divnr\histxo1_p\coimtilbage.gcm",
@"nrbanker\divnr\histxo1_p\fXoz.gcm",
@"nrbanker\divnr\histxo1_p\tilestbk.gcm",
@"nrbanker\divnr\histxo1_p\xo1_ptilbage.gcm",
@"nrbanker\erhverv\adam118.gcm",
@"nrbanker\erhverv\adamerh.gcm",
@"nrbanker\erhverv\byrhh.gcm",
@"nrbanker\erhverv\danerhverv.gcm",
@"nrbanker\erhverv\datarev.gcm",
@"nrbanker\erhverv\kildedata.gcm",
@"nrbanker\erhverv\tilobk.gcm",
@"nrbanker\erhverv\tjek.gcm",
@"nrbanker\erhverv\tjek_abase.gcm",
@"nrbanker\erhverv\total_tjek.gcm",
@"nrbanker\erhverv\strejke\QMR.gcm",
@"nrbanker\erhverv\strejke\tilobk.gcm",
@"nrbanker\erhverv\tjeklister\genlist.gcm",
@"nrbanker\Finans\datop.gcm",
@"nrbanker\Finans\egen.bak.gcm",
@"nrbanker\Finans\egen.gcm",
@"nrbanker\Finans\finans.gcm",
@"nrbanker\Finans\finansbk.bk.gcm",
@"nrbanker\Finans\finansbk.bk2.gcm",
@"nrbanker\Finans\finansbk.bk3.gcm",
@"nrbanker\Finans\finansbk.gcm",
@"nrbanker\Finans\fkadam.bk.gcm",
@"nrbanker\Finans\fkadam.bk6.gcm",
@"nrbanker\Finans\fkadam.gcm",
@"nrbanker\Finans\fkadamchk.bak.gcm",
@"nrbanker\Finans\fkadamchk.bk6.gcm",
@"nrbanker\Finans\fkadamchk.gcm",
@"nrbanker\Finans\gfkadam.gcm",
@"nrbanker\Finans\gkadamchk.gcm",
@"nrbanker\Finans\koc.gcm",
@"nrbanker\Finans\lister.gcm",
@"nrbanker\Finans\omdoeb.gcm",
@"nrbanker\Finans\primo.gcm",
@"nrbanker\Finans\priwse.gcm",
@"nrbanker\Finans\tjkfinan.gcm",
@"nrbanker\Finans\wseer.gcm",
@"nrbanker\Finans\wseer16.gcm",
@"nrbanker\Finans\NASF\datop.gcm",
@"nrbanker\Finans\NASF\egen.gcm",
@"nrbanker\Finans\NASF\egen_h.gcm",
@"nrbanker\Finans\NASF\finans.gcm",
@"nrbanker\Finans\NASF\finansbk.gcm",
@"nrbanker\Finans\NASF\fkadam.gcm",
@"nrbanker\Finans\NASF\fkadamchk.gcm",
@"nrbanker\Finans\NASF\fkadam_gl.gcm",
@"nrbanker\Finans\NASF\gekarem.gcm",
@"nrbanker\Finans\NASF\kox.gcm",
@"nrbanker\Finans\NASF\lister.gcm",
@"nrbanker\Finans\NASF\omdoeb.gcm",
@"nrbanker\Finans\NASF\omdoeb.ufr.gcm",
@"nrbanker\Finans\NASF\tjkfinan.gcm",
@"nrbanker\Forbrug\c_lister.gcm",
@"nrbanker\Forbrug\datarev.gcm",
@"nrbanker\Forbrug\forbrug.gcm",
@"nrbanker\Forbrug\kildedata.gcm",
@"nrbanker\Forbrug\kildedataABASE.gcm",
@"nrbanker\Forbrug\myfunctions.gcm",
@"nrbanker\Forbrug\tilobk.gcm",
@"nrbanker\Hoved\Datop.bak.gcm",
@"nrbanker\Hoved\Datop.gcm",
@"nrbanker\Hoved\evmv.gcm",
@"nrbanker\Hoved\fevmv6607.gcm",
@"nrbanker\Hoved\gekarem.gcm",
@"nrbanker\Hoved\tilobk.gcm",
@"nrbanker\Hoved\tilobk6607.gcm",
@"nrbanker\invest\datarev.gcm",
@"nrbanker\invest\fra_abasei.gcm",
@"nrbanker\invest\fra_abasek.gcm",
@"nrbanker\invest\lister.gcm",
@"nrbanker\invest\nabk69.gcm",
@"nrbanker\invest\nahi.gcm",
@"nrbanker\invest\nahk.gcm",
@"nrbanker\kapital\datarev.gcm",
@"nrbanker\kapital\Datatjek.gcm",
@"nrbanker\kapital\kapital.gcm",
@"nrbanker\kapital\lister.gcm",
@"nrbanker\kapital\Perioder.gcm",
@"nrbanker\Kapitalsektor\datop.bak.gcm",
@"nrbanker\Kapitalsektor\datop.bk2.gcm",
@"nrbanker\Kapitalsektor\datop.gcm",
@"nrbanker\Kapitalsektor\gekarem.gcm",
@"nrbanker\Kapitalsektor\tilobk.gcm",
@"nrbanker\Sektor\check1.gcm",
@"nrbanker\Sektor\datop.gcm",
@"nrbanker\Sektor\gekarem.gcm",
@"nrbanker\Sektor\nasbnk.gcm",
@"nrbanker\Sektor\sektjek.gcm",
@"okt16\faktor\FAKTORG.gcm",
@"okt16\faktor\FAKTORP.gcm",
@"okt16\faktor\FAKTORT.gcm",
@"okt16\faktor\FAKTORW.gcm",
@"okt16\faktor\hoved_faktor.gcm",
@"okt16\o1\dto1.gcm",
@"okt18\databank\estbk.gcm",
@"okt18\databank\finansdata.gcm",
@"okt18\databank\fm7yim.gcm",
@"okt18\databank\pxe_e3.gcm",
@"okt18\faktor\estbk_faktor.gcm",
@"okt18\faktor\FAKTORG.gcm",
@"okt18\faktor\FAKTORP.gcm",
@"okt18\faktor\FAKTORT.gcm",
@"okt18\faktor\FAKTORW.gcm",
@"okt18\faktor\hoved_faktor.gcm",
@"pension\periode.gcm",
@"pension\periode_20191119_tom20200626.gcm",
@"pension\periode_tom201907231600.gcm",
@"pension\bak\periode_pension.gcm",
@"pension\kontrol\DanUdvidetPbank.gcm",
@"pension\kontrol\sml_fk.gcm",
@"pension\kontrol\sml_obk.gcm",
@"pension\kontrol\sml_PENSdelbanker.gcm",
@"pension\kontrol\sml_sektor.gcm",
@"pension\kontrol\sml_sywp.gcm",
@"pension\prog\atpmv_tilobk.gcm",
@"pension\prog\atpmv_tilobk_mLDhist.gcm",
@"pension\prog\atpmv_tilps.gcm",
@"pension\prog\bsyptypkoef.gcm",
@"pension\prog\bsyptypkoef_gl.gcm",
@"pension\prog\bsyptypkoef_per2.gcm",
@"pension\prog\bsyptyp_ld_tilobk.gcm",
@"pension\prog\bsyptyp_tilobk.gcm",
@"pension\prog\bsyptyp_tilps.gcm",
@"pension\prog\dan_wp_h.gcm",
@"pension\prog\dan_wp_h_apr.gcm",
@"pension\prog\datop_wp_h_mfl.gcm",
@"pension\prog\datop_wp_h_mfl_apr.gcm",
@"pension\prog\datop_wp_h_mfl_jun.gcm",
@"pension\prog\extras.gcm",
@"pension\prog\extras2obk.gcm",
@"pension\prog\extras2ps.gcm",
@"pension\prog\foerFK.gcm",
@"pension\prog\foerSEKTOR.gcm",
@"pension\prog\fraESTBK_JUL17.gcm",
@"pension\prog\HurtigAprilPension.gcm",
@"pension\prog\HurtigAprilPension2plus_202003261139.gcm",
@"pension\prog\HurtigAprilPension2_202003260958.gcm",
@"pension\prog\HurtigAprilPension_202003251849.gcm",
@"pension\prog\HurtigJuniiPension2.gcm",
@"pension\prog\HurtigJuniPension2plus.gcm",
@"pension\prog\LDhist_tilobk.gcm",
@"pension\prog\mere.gcm",
@"pension\prog\Owp_bf_tilpasning.gcm",
@"pension\prog\SkafFremskrivBank.gcm",
@"pension\prog\tilobk3.gcm",
@"pension\prog\tilps3.gcm",
@"pension\prog\tipc2obk.gcm",
@"pension\prog\tipc2obk_kildearem.gcm",
@"pension\prog\tipc2ps.gcm",
@"pension\prog\tipc2ps_kildearem.gcm",
@"pension\prog\tipc_cf_h.gcm",
@"pension\prog\wp_h_afledte.gcm",
@"pension\prog\wp_h_afledte_foer_fk.gcm",
@"pension\prog\wp_h_afledte_foer_fk_apr.gcm",
@"pension\prog\wp_h_mfl_2obk.gcm",
@"pension\prog\wp_h_mfl_2obk_apr.gcm",
@"pension\prog\wp_h_mfl_2ps.gcm",
@"pension\prog\atpmv\atpmv2ud.gcm",
@"pension\prog\atpmv\diverse.gcm",
@"pension\prog\atpmv\LD.gcm",
@"pension\prog\atpmv\pbank_atpmv_sk0.gcm",
@"pension\prog\atpmv\pbank_atpmv_sk1.gcm",
@"pension\prog\atpmv\pbank_atpmv_sk2.gcm",
@"pension\prog\atpmv\pbank_atpmv_sk2apr.gcm",
@"pension\prog\atpmv\pbank_atpmv_sk2apr_v2.gcm",
@"pension\prog\atpmv\pbank_atpmv_sk3.gcm",
@"pension\prog\sys0\2ud.gcm",
@"pension\prog\sys0\CPSjul13.gcm",
@"pension\prog\sys0\pbank_sk1.gcm",
@"pension\prog\sys0\pbank_sk2a.gcm",
@"pension\prog\sys0\pbank_sk2b.gcm",
@"pension\prog\sys0\pbank_sk3.gcm",
@"pension\prog\sys2\dandata3_f.gcm",
@"pension\prog\sys2\dandata6a_b.gcm",
@"pension\prog\sys2\dandata6b_b.gcm",
@"pension\prog\sys2\dandata_afledte.gcm",
@"pension\prog\sys2\DanData_head_xp.gcm",
@"pension\prog\sys2\dandata_tmk.gcm",
@"pension\prog\sys2\DanPension2.gcm",
@"pension\prog\sys2\DanPension3.gcm",
@"pension\prog\sys2\DanPension4.gcm",
@"pension\prog\sys2\datakontrol_head.gcm",
@"pension\prog\sys2\data_start_okt15.gcm",
@"pension\prog\sys2\grund_okt15.gcm",
@"pension\prog\sys2\pplus.gcm",
@"ras\datarev.gcm",
@"ras\ras.gcm",
@"ras\rastjek.gcm",
@"ras\tilobk.gcm",
@"ras\tjkinput.bak.gcm",
@"ras\tjkinput.gcm",
@"residualer\hentres_jul17.gcm",
@"residualer\hentres_JUN19.gcm",
@"residualer\hentres_OKT18.gcm",
@"residualer\hentres_OKT20beta.gcm",
@"Sektor\datop.gcm",
@"Sektor\gekarem.gcm",
@"Sektor\nah4.gcm",
@"Sektor\nasl2.gcm",
@"Sektor\renter.gcm",
@"Sektor\sdata.bak.gcm",
@"Sektor\sdata.bk2.gcm",
@"Sektor\sdata.gcm",
@"Sektor\tilkox.gcm",
@"Sektor\tilobk.gcm",
@"Skat2\bysser_tilbage.gcm",
@"Skat2\datrev.gcm",
@"Skat2\datrev_mm.gcm",
@"Skat2\datrev_ssy.gcm",
@"Skat2\datrev_ys.gcm",
@"Skat2\datrev_ysp.gcm",
@"Skat2\satser.gcm",
@"Skat2\skatper.gcm",
@"Skat2\ssy.gcm",
@"Skat2\til_obk.gcm",
@"Skat2\tsuih4899.gcm",
@"Skat2\usy7099.gcm",
@"Skat2\ys.gcm",
@"Skat2\Ys7499.gcm",
@"Skat2\ysp.gcm",
@"Skat2\ys_faar.gcm",
@"START\DanInitialBank.gcm",
@"Uadam\Databank\adam.gcm",
@"Uadam\Databank\agg.gcm",
@"Uadam\Databank\agg_q.gcm",
@"Uadam\Databank\befolk.gcm",
@"Uadam\Databank\bu.gcm",
@"Uadam\Databank\datrev.gcm",
@"Uadam\Databank\figur1.gcm",
@"Uadam\Databank\figur1_translate.gcm",
@"Uadam\Databank\figur2.gcm",
@"Uadam\Databank\foy.gcm",
@"Uadam\Databank\Freq_a.gcm",
@"Uadam\Databank\Freq_q.gcm",
@"Uadam\Databank\f_ras_1.gcm",
@"Uadam\Databank\f_ras_2.gcm",
@"Uadam\Databank\MyFunctions.gcm",
@"Uadam\Databank\nr.gcm",
@"Uadam\Databank\opdatper.gcm",
@"Uadam\Databank\opdatper_translate.gcm",
@"Uadam\Databank\puob.gcm",
@"Uadam\Databank\ram.gcm",
@"Uadam\Databank\ram_f.gcm",
@"Uadam\Databank\ras.gcm",
@"Uadam\Databank\ras_h.gcm",
@"Uadam\Databank\sam.gcm",
@"Uadam\Databank\test.gcm",
@"Uadam\Databank\til_obk.gcm",
@"Uadam\Databank\til_obk2.gcm",
@"Uadam\Databank\usp.gcm",
@"Uadam\Databank\Aremos_til_Gekko\til_ebk.gcm",
@"Uadam\Databank\Aremos_til_Gekko\til_ebk2.gcm",
@"Uadam\Databank\Befolk\befolk.gcm",
@"Uadam\Databank\Befolk\input.gcm",
@"Uadam\Databank\Befolk\hist\befolk17.gcm",
@"Uadam\Databank\Cram\RAM.gcm",
@"Uadam\Databank\Cram\RAM_1948_1984.gcm",
@"Uadam\Databank\Cram\RAM_1985_2006.gcm",
@"Uadam\Databank\Cram\RAM_2001_2006q.gcm",
@"Uadam\Databank\Cram\RAM_faktiske.gcm",
@"Uadam\Databank\Cram\Brutto19782007\ulb19782006.gcm",
@"Uadam\Databank\Cram\Cram_1985_2007\cram_1985_2007.gcm",
@"Uadam\Databank\Figur\ADAM1\Gekkoplot\figur1.gcm",
@"Uadam\Databank\Figur\Bef1\Gekkoplot\figur1.gcm",
@"Uadam\Databank\Figur\OF1\Gekkoplot\figur1.gcm",
@"Uadam\Databank\Figur\RAM1\Gekkoplot\figur1.gcm",
@"Uadam\Databank\Figur\RAS1\Gekkoplot\figur1.gcm",
@"Uadam\Databank\foy\foy.gcm",
@"Uadam\Databank\NR\nr.gcm",
@"Uadam\Databank\Pension\pens.gcm",
@"Uadam\Databank\Pension\pens1967_2016.gcm",
@"Uadam\Databank\Puob\freqa.gcm",
@"Uadam\Databank\Puob\freqq.gcm",
@"Uadam\Databank\Puob\puob2007.gcm",
@"Uadam\Databank\Puob\puob2008.gcm",
@"Uadam\Databank\Puob\puob2009.gcm",
@"Uadam\Databank\Puob\puob2010.gcm",
@"Uadam\Databank\Puob\puob2011.gcm",
@"Uadam\Databank\Puob\puob2012.gcm",
@"Uadam\Databank\Puob\puob2013.gcm",
@"Uadam\Databank\Puob\puob2014.gcm",
@"Uadam\Databank\Puob\puob2015.gcm",
@"Uadam\Databank\Puob\puob2016.gcm",
@"Uadam\Databank\Puob\puob2017.gcm",
@"Uadam\Databank\Puob\puob2018.gcm",
@"Uadam\Databank\Puob\puob_tilbage.gcm",
@"Uadam\Databank\Puob\Uddannelsesordning\udd.gcm",
@"Uadam\Databank\Puob\Uddannelsesordning\udd_.gcm",
@"Uadam\Databank\Puob\Ufly\ufly.gcm",
@"Uadam\Databank\Puob\Ufry\ufry.gcm",
@"Uadam\Databank\Puob\Ufs\ufs.gcm",
@"Uadam\Databank\RASn\fra_excel.gcm",
@"Uadam\Databank\RASn\fra_excel_k.gcm",
@"Uadam\Databank\RASn\fra_excel_k2.gcm",
@"Uadam\Databank\RASn\fra_excel_k3.gcm",
@"Uadam\Databank\RASn\ras1981_2007.gcm",
@"Uadam\Databank\RASn\ras2008_2015.gcm",
@"Uadam\Databank\RASn\ras2016.gcm",
@"Uadam\Databank\RASn\ras2017.gcm",
@"Uadam\Databank\RASn\Urpt.gcm",
@"Uadam\Databank\Samsoc\ret_sss.gcm",
@"Uadam\Formelfil\Uadam16\kvoter16.gcm",
@"Uadam\Formelfil\Uadam17\kvoter17.gcm",
@"Uadam\Formelfil\Uadam18\kvoter18.gcm",
@"Uadam\Formelfil\Uadam19a\kvoter19a.gcm",
@"Uadam\Formelfil\Uadam19a\Lister\Lister.gcm",
@"Uadam\Satser\satser.gcm",
@"Uadam\Satser\Satser_projektgruppe\grafer.gcm",
@"Uadam\Satser\Satser_projektgruppe\satser.gcm",
@"Uadam\Satser\Satser_projektgruppe\sygedagpenge_rettelser.gcm",
@"Uadam\Satser\Satser_projektgruppe\Faktiske_satser\faktiskesatser.gcm",
@"Uadam\UAbank\Uadam16\kvoter16.gcm",
@"Uadam\UAbank\Uadam16\Ua0416.gcm",
@"Uadam\UAbank\Uadam16\Ua0417.gcm",
@"Uadam\UAbank\Uadam16\Ua0616.gcm",
@"Uadam\UAbank\Uadam16\Ua1115.gcm",
@"Uadam\UAbank\Uadam16\Ua1116.gcm",
@"Uadam\UAbank\Uadam17\a_p17.gcm",
@"Uadam\UAbank\Uadam17\kvoter17.gcm",
@"Uadam\UAbank\Uadam17\Ua0418.gcm",
@"Uadam\UAbank\Uadam17\Ua0618.gcm",
@"Uadam\UAbank\Uadam17\Ua0717.gcm",
@"Uadam\UAbank\Uadam17\Ua1117.gcm",
@"Uadam\UAbank\Uadam17\Ua1118_jul17x.gcm",
@"Uadam\UAbank\Uadam18\a_p18.gcm",
@"Uadam\UAbank\Uadam18\kvoter18.gcm",
@"Uadam\UAbank\Uadam18\Ua0419.gcm",
@"Uadam\UAbank\Uadam18\Ua0619.gcm",
@"Uadam\UAbank\Uadam18\Ua1118_okt18.gcm",
@"Uadam\UAbank\Uadam18\Ua1119.gcm",
@"Uadam\UAbank\Uadam19a\a_p19a.gcm",
@"Uadam\UAbank\Uadam19a\kvoter19a.gcm",
@"Uadam\UAbank\Uadam19a\NR_frem.gcm",
@"Uadam\UAbank\Uadam19a\Ras_frem.gcm",
@"Uadam\UAbank\Uadam19a\Ua0619.gcm",
@"Uadam\UAbank\Uadam19a\Ua1119.gcm"

            };

            return m;
        }

        public static void WalkFolderHelper2(DirectoryInfo directoryInfo, WalkInfo wi)
        {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (G.Equal(file.Extension, ".gcm"))
                {
                    wi.storage.Add(file.FullName.Replace(@"c:\Thomas\Desktop\gekko\testing\Translate\", ""));
                }
                else
                {
                    new Writeln("!!! HOVSA " + file.FullName);
                }
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                WalkFolderHelper2(subfolder, wi);
            }
        }

        public static void WalkFolderHelper(DirectoryInfo directoryInfo, WalkInfo wi)
        {
            if (true)
            {
                
                foreach (FileInfo file in directoryInfo.GetFiles())
                {                       
                    bool skip = false;                    
                    if (file.Extension.ToLower() != ".gcm") skip = true;                    
                    if (skip) continue;

                    wi.filesCounter++;

                    bool missing1line = false;
                    bool good = false;
                    string translated2 = null;
                    try
                    {
                        translated2 = Program.GetTextFromFileWithWait(file.FullName.Replace(".gcm", ".ucm").Replace(".GCM", ".ucm"));
                        if (!translated2.Contains("++++ TTH check ++++")) missing1line = true;
                        if (Gekko.Parser.Gek.ParserGekCreateAST.IsValid3_0Syntax(translated2))
                        {
                            good = true;
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        bool skip2 = false;
                        if (wi.filesCounter == 54) skip2 = true;
                        if (wi.filesCounter == 91) skip2 = true;
                        if (wi.filesCounter == 120) skip2 = true;
                        if (wi.filesCounter == 121) skip2 = true;
                        if (wi.filesCounter == 122) skip2 = true;
                        if (wi.filesCounter == 251) skip2 = true;
                        if (wi.filesCounter == 252) skip2 = true;
                        if (wi.filesCounter == 253) skip2 = true;
                        if (wi.filesCounter == 254) skip2 = true;
                        if (wi.filesCounter == 255) skip2 = true;
                        if (wi.filesCounter == 256) skip2 = true;
                        if (wi.filesCounter == 260) skip2 = true;
                        if (wi.filesCounter == 261) skip2 = true;
                        if (wi.filesCounter == 265) skip2 = true;
                        if (wi.filesCounter == 303) skip2 = true;
                        if (wi.filesCounter == 310) skip2 = true;
                        if (wi.filesCounter == 311) skip2 = true;
                        if (wi.filesCounter == 313) skip2 = true;
                        if (wi.filesCounter == 314) skip2 = true;
                        if (wi.filesCounter == 319) skip2 = true;
                        if (wi.filesCounter == 320) skip2 = true;
                        if (wi.filesCounter == 322) skip2 = true;
                        if (wi.filesCounter == 323) skip2 = true;
                        if (wi.filesCounter == 324) skip2 = true;
                        if (wi.filesCounter == 326) skip2 = true;
                        if (wi.filesCounter == 327) skip2 = true;
                        if (wi.filesCounter == 328) skip2 = true;
                        if (wi.filesCounter == 332) skip2 = true;
                        if (wi.filesCounter == 333) skip2 = true;
                        if (wi.filesCounter == 372) skip2 = true;
                        if (wi.filesCounter == 388) skip2 = true;
                        if (wi.filesCounter == 389) skip2 = true;
                        if (wi.filesCounter == 392) skip2 = true;
                        if (wi.filesCounter == 407) skip2 = true;
                        if (wi.filesCounter == 426) skip2 = true;
                        if (wi.filesCounter == 429) skip2 = true;
                        if (wi.filesCounter == 437) skip2 = true;
                        if (wi.filesCounter == 448) skip2 = true;
                        if (wi.filesCounter == 457) skip2 = true;
                        if (wi.filesCounter == 466) skip2 = true;
                        if (wi.filesCounter == 475) skip2 = true;
                        if (wi.filesCounter == 508) skip2 = true;
                        if (wi.filesCounter == 540) skip2 = true;
                        if (wi.filesCounter == 541) skip2 = true;
                        if (wi.filesCounter == 598) skip2 = true;
                        if (wi.filesCounter == 599) skip2 = true;
                        if (wi.filesCounter == 600) skip2 = true;
                        if (wi.filesCounter == 652) skip2 = true;
                        if (wi.filesCounter == 705) skip2 = true;
                        if (wi.filesCounter == 706) skip2 = true;
                        if (wi.filesCounter == 707) skip2 = true;
                        if (wi.filesCounter == 708) skip2 = true;
                        if (wi.filesCounter >= 823 && wi.filesCounter <= 885) skip2 = true;

                        if (skip2)
                        {
                            //delete tcm file if it exists
                        }

                        if (skip2) good = true;

                        string missing = "";
                        if (missing1line && !skip2) missing = "-----> !!!!! ";
                        string s = "     ";
                        if (!good) s = " BAD ";                        
                        new Writeln(". " + missing + G.IntFormat(wi.filesCounter, 5) + s + file.FullName);

                        //if (!skip2)
                        //{
                        //    using (FileStream fs = Program.WaitForFileStream(file.FullName.Replace(".gcm", ".ucm").Replace(".GCM", ".ucm"), Program.GekkoFileReadOrWrite.Write))
                        //    using (StreamWriter file2 = G.GekkoStreamWriter(fs))
                        //    {
                        //        file2.Write(G.Replace(translated2, @"g:\datopgek", @"{root()}", StringComparison.OrdinalIgnoreCase, int.MaxValue));
                        //    }                            
                        //}

                        //if (translated2 != null)
                        //{
                        //    List<string> lines = Stringlist.ExtractLinesFromText(translated2);
                        //    foreach(string line in lines)
                        //    {
                        //        if (line.ToLower().Contains("datopgek")) new Writeln(line);
                        //    }                            
                        //}

                        //wi.storage
                    }
                }

                foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
                {
                    WalkFolderHelper(subfolder, wi);
                }
            }

            //if (false)
            //{
            //    int max = int.MaxValue;
            //    if (wi.filesCounter >= max) return;
            //    Info info = new Info();
            //    info.keepTypes = true;
            //    foreach (FileInfo file in directoryInfo.GetFiles())
            //    {
            //        bool skip = false;
            //        foreach (string s in wi.omits)
            //        {
            //            if (file.FullName.ToLower().Contains(s)) skip = true;
            //        }
            //        if (file.Extension.ToLower() != ".gcm") skip = true;
            //        if (skip) continue;

            //        List<string> doc = new List<string>();

            //        try
            //        {
            //            string linesString = Program.GetTextFromFileWithWait(file.FullName);
            //            string name = Path.GetFileNameWithoutExtension(file.FullName);
            //            string path = Path.GetDirectoryName(file.FullName);
            //            string newFile = path + "\\" + name + ".tcm";
            //            string ss = G.IntFormat(wi.storage.Count, 4).Replace(" ", "0") + " " + file.FullName;
            //            using (var w = new Writeln())
            //            {
            //                w.MainAdd(ss);
            //            }
            //            string wholeBad = "z_";
            //            string translated2 = Translate(linesString, info);

            //            try
            //            {
            //                if (!Gekko.Parser.Gek.ParserGekCreateAST.IsValid3_0Syntax(translated2))
            //                {
            //                    wi.wholeBadFilesCounter++;
            //                    wholeBad = "b_";
            //                    //File.Copy(newFile, @"c:\Thomas\Desktop\gekko\testing\TranslateLog\Files\" + wholeBad + name + ".tcm", true);
            //                }
            //            }
            //            catch
            //            {
            //                wi.wholeBadFilesCounter++;
            //                wholeBad = "b_";
            //                //File.Copy(newFile, @"c:\Thomas\Desktop\gekko\testing\TranslateLog\Files\" + wholeBad + name + ".tcm", true);
            //            }
            //            finally
            //            {
            //                if (wholeBad == "b_")
            //                    File.WriteAllText(newFile + "_failing", translated2);
            //                else
            //                    File.WriteAllText(newFile, translated2);
            //                wi.filesCounter++;
            //                int kb = (int)(file.Length);
            //                doc.Add(wi.filesCounter.ToString());
            //                string file2 = newFile.Replace(@"c:\Thomas\Desktop\gekko\testing\Translate\", "");
            //                doc.Add(file2);
            //                if (wholeBad == "b_")
            //                    doc.Add("FAILING");
            //                else
            //                    doc.Add("ok");
            //                doc.Add(kb.ToString());
            //                wi.docs.Add(doc);
            //            }
            //        }
            //        catch
            //        {
            //            //we may survive if this fails
            //        }
            //    }
            //    foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            //    {
            //        WalkFolderHelper(subfolder, wi);
            //    }
            //}

            //if (false)
            //{
            //    int max = int.MaxValue;
            //    if (wi.filesCounter >= max) return;
            //    Info info = new Info();
            //    info.keepTypes = true;
            //    foreach (FileInfo file in directoryInfo.GetFiles())
            //    {
            //        bool skip = false;
            //        foreach (string s in wi.omits)
            //        {
            //            if (file.FullName.ToLower().Contains(s)) skip = true;
            //        }
            //        if (file.Extension.ToLower() != ".gcm") skip = true;
            //        if (skip) continue;

            //        List<string> doc = new List<string>();

            //        try
            //        {
            //            string linesString = Program.GetTextFromFileWithWait(file.FullName);
            //            string name = Path.GetFileNameWithoutExtension(file.FullName);
            //            string path = Path.GetDirectoryName(file.FullName);
            //            string newFile = path + "\\" + name + ".tcm";
            //            string ss = G.IntFormat(wi.storage.Count, 4).Replace(" ", "0") + " " + file.FullName;
            //            using (var w = new Writeln())
            //            {
            //                w.MainAdd(ss);
            //            }
            //            string wholeBad = "z_";
            //            string translated2 = Translate(linesString, info);

            //            try
            //            {
            //                if (!Gekko.Parser.Gek.ParserGekCreateAST.IsValid3_0Syntax(translated2))
            //                {
            //                    wi.wholeBadFilesCounter++;
            //                    wholeBad = "b_";
            //                    //File.Copy(newFile, @"c:\Thomas\Desktop\gekko\testing\TranslateLog\Files\" + wholeBad + name + ".tcm", true);
            //                }
            //            }
            //            catch
            //            {
            //                wi.wholeBadFilesCounter++;
            //                wholeBad = "b_";
            //                //File.Copy(newFile, @"c:\Thomas\Desktop\gekko\testing\TranslateLog\Files\" + wholeBad + name + ".tcm", true);
            //            }
            //            finally
            //            {
            //                if (wholeBad == "b_")
            //                    File.WriteAllText(newFile + "_failing", translated2);
            //                else
            //                    File.WriteAllText(newFile, translated2);
            //                wi.filesCounter++;
            //                int kb = (int)(file.Length);
            //                doc.Add(wi.filesCounter.ToString());
            //                string file2 = newFile.Replace(@"c:\Thomas\Desktop\gekko\testing\Translate\", "");
            //                doc.Add(file2);
            //                if (wholeBad == "b_")
            //                    doc.Add("FAILING");
            //                else
            //                    doc.Add("ok");
            //                doc.Add(kb.ToString());
            //                wi.docs.Add(doc);
            //            }
            //        }
            //        catch
            //        {
            //            //we may survive if this fails
            //        }
            //    }
            //    foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            //    {
            //        WalkFolderHelper(subfolder, wi);
            //    }
            //}

            //if (false)
            //{
            //    int max = int.MaxValue;
            //    if (wi.storage.Count >= max) return;
            //    Info info = new Info();
            //    foreach (FileInfo file in directoryInfo.GetFiles())
            //    {
            //        bool skip = false;
            //        foreach (string s in wi.omits)
            //        {
            //            if (file.FullName.ToLower().Contains(s)) skip = true;
            //        }
            //        if (file.Extension.ToLower() != ".gcm") skip = true;
            //        if (skip) continue;

            //        try
            //        {
            //            string linesString = Program.GetTextFromFileWithWait(file.FullName);
            //            List<string> lines = Stringlist.ExtractLinesFromText(linesString);
            //            int counter = 0;
            //            int errorCounter = 0;
            //            List<string> output = new List<string>();
            //            foreach (string line in lines)
            //            {
            //                counter++;
            //                wi.lines++;
            //                if (line == null || line.Trim() == "")
            //                {
            //                    output.Add("");
            //                    continue;
            //                }

            //                string translated = Translate(line, info);
            //                //string translated = Translator_Gekko20_Gekko30_OLD_REMOVE_SOON.Translate(line);
            //                //string translated = line;


            //                if (true)
            //                {
            //                    if (translated.Trim().ToLower().StartsWith("for ") || translated.Trim().ToLower().StartsWith("for(") || translated.Trim().ToLower().StartsWith("if ") || translated.Trim().ToLower().StartsWith("if("))
            //                    {
            //                        if (!(translated.Trim().ToLower().EndsWith("end;") || translated.Trim().ToLower().EndsWith("end ;")))
            //                        {
            //                            translated = translated + " end;";
            //                        }
            //                    }
            //                    else if (translated.Trim().ToLower() == "end;" || translated.Trim().ToLower() == "else;" || translated.Trim().ToLower() == "end ;" || translated.Trim().ToLower() == "else ;")
            //                    {
            //                        translated = "// " + translated;
            //                    }
            //                }

            //                string start = "    ";
            //                try
            //                {
            //                    if (!Gekko.Parser.Gek.ParserGekCreateAST.IsValid3_0Syntax(translated))
            //                    {
            //                        start = "!!! ";
            //                        errorCounter++;
            //                        wi.errorLines++;
            //                    }
            //                }
            //                catch { }
            //                output.Add(start + translated);
            //            }

            //            string name = Path.GetFileNameWithoutExtension(file.FullName);
            //            string path = Path.GetDirectoryName(file.FullName);
            //            string newFile = path + "\\" + name + ".tcm";
            //            File.WriteAllText(newFile, Stringlist.ExtractTextFromLines(output).ToString());
            //            string ss = G.IntFormat(wi.storage.Count, 4).Replace(" ", "0") + " " + G.IntFormat(errorCounter, 4) + "/" + G.IntFormat(counter, 4) + " = " + file.FullName;
            //            using (var w = new Writeln())
            //            {
            //                w.MainAdd(ss);
            //            }
            //            wi.storage.Add(ss);

            //            string wholeBad = "z_";
            //            string translated2 = Translate(linesString, info);
            //            try
            //            {
            //                if (!Gekko.Parser.Gek.ParserGekCreateAST.IsValid3_0Syntax(translated2))
            //                {
            //                    wi.wholeBadFilesCounter++;
            //                    wholeBad = "b_";
            //                }
            //            }
            //            catch { }


            //            wi.filesCounter++;
            //            if (errorCounter > 1)
            //            {
            //                //G.DeleteFolder(@"c:\Thomas\Desktop\gekko\testing\TranslateLog\Files\", true);
            //                File.Copy(newFile, @"c:\Thomas\Desktop\gekko\testing\TranslateLog\Files\" + wholeBad + G.IntFormat(errorCounter, 4).Replace(" ", "0") + name + ".tcm", true);
            //                wi.badFilesCounter++;
            //            }


            //            if (wi.storage.Count >= max) return;
            //        }
            //        catch
            //        {
            //            //we may survive if this fails
            //        }
            //    }
            //    foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            //    {
            //        WalkFolderHelper(subfolder, wi);
            //    }
            //}
        }

        public class WalkInfo
        {
            public List<string> omits = new List<string>(); // { "fra_excel", "uadam" };  //must not be null            
            public List<string> storage = new List<string>();
            public int lines = 0;
            public int errorLines = 0;
            public int filesCounter = 0;
            public int badFilesCounter = 0;
            public int wholeBadFilesCounter = 0;
            public List<List<string>> docs = new List<List<string>>();
            public bool replaceDatopgek = false;
        }


    }
}
