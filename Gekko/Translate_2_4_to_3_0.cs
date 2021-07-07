using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gekko
{
    public class Translate_2_4_to_3_0
    {

        //public static GekkoDictionary<string, string> listMemory = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        //public static GekkoDictionary<string, string> matrixMemory = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        //public static GekkoDictionary<string, string> scalarMemory = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        //This class translates from Gekko 2.0 to 3.0

        public static string Translate(string input)
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

            foreach (List<TokenHelper> line in statements)
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
                    //[0] --> .length()
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
                    //  %x\ --> {%x}\ for path
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

                //quotes, interpolate. Stuff like 'a%x%y|z ~%x {%y}' --> 'a{%x}{%y}z %x {%y}'
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
                    line[i].s = ";";  //... || ...  --> ... ; ...
                    line[i + 1].s = "";
                }

                if (StringTokenizer.Equal(line, i, "avgt") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    AddComment(line, "avgt(): if local time is given, use <%t1, %t2> syntax with <>-brackets");
                }
                else if (StringTokenizer.Equal(line, i, "difference") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    line[i].s = "except";
                }
                else if (StringTokenizer.Equal(line, i, "endswith") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    AddComment(line, "endswith() is case-insensitive in Gekko 3.0");
                }
                else if (StringTokenizer.Equal(line, i, "fromseries") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    //TODO: if fromseries('x', ...) or fromseries(%x, ...) --> fromseries(x, ...) or fromseries({%x}, ...) 
                }
                else if (StringTokenizer.Equal(line, i, "hpfilter") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    AddComment(line, "hpfilter(): if local time is given, use <%t1, %t2> syntax with <>-brackets");
                }
                else if (StringTokenizer.Equal(line, i, "pack") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    AddComment(line, "pack(): if local time is given, use <%t1, %t2> syntax with <>-brackets");
                }
                else if (StringTokenizer.Equal(line, i, "piece") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    line[i].s = "substring";
                }
                else if (StringTokenizer.Equal(line, i, "replace") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    AddComment(line, "replace() is case-insensitive in Gekko 3.0");
                }
                else if (StringTokenizer.Equal(line, i, "search") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    line[i].s = "index";
                }
                else if (StringTokenizer.Equal(line, i, "startswith") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    AddComment(line, "startswith() is case-insensitive in Gekko 3.0");
                }
                else if (StringTokenizer.Equal(line, i, "strip") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    line[i].s = "replace";
                    AddComment(line, "strip(%x) is now replace(%x, '')");
                }
                else if (StringTokenizer.Equal(line, i, "sumt") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    AddComment(line, "sumt(): if local time is given, use <%t1, %t2> syntax with <>-brackets");
                }
                else if (StringTokenizer.Equal(line, i, "trim") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    line[i].s = "strip";
                }                
                else if (StringTokenizer.Equal(line, i, "unpack") && i + 1 < line.Count && line[i + 1].subnodes != null && line[i + 1].subnodes[0].s == "(")
                {
                    AddComment(line, "unpack(): if local time is given, use <%t1, %t2> syntax with <>-brackets");
                }                                
            }
        }

        /// <summary>
        /// Stuff here only looks at one sentence, does not go inside any recursive (), [] or {}.
        /// </summary>
        /// <param name="line"></param>
        public static void HandleCommandName(List<TokenHelper> line)
        {
            int pos = 0;

            line[pos].meta.commandName = line[pos].s.ToLower();  //right most of the time, exceptions P, PRI, PRT, ...

            if (G.Equal(line[pos].s, "compare"))
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

            else if (G.Equal(line[pos].s, "copy"))
            {
                //from/to --> frombank/tobank
                //Must use COPY {#m}, not COPY #m
            }


            else if (G.Equal(line[pos].s, "create"))
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

                if (StringTokenizer.Equal(line, 2, "="))
                {
                    line[pos].s = "%";
                    line[pos + 1].leftblanks = 0;
                }
            }

            else if (G.Equal(line[pos].s, "download"))
            {
                AddComment(line, "DOWNLOAD requires quotes around the URL");
            }

            else if (G.Equal(line[pos].s, "export"))
            {
                AddComment(line, "For EXPORT without dates, use EXPORT<all>");
            }

            else if (G.Equal(line[pos].s, "function"))
            {

            }


            else if (G.Equal(line[pos].s, "if"))
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

            else if (G.Equal(line[pos].s, "index"))
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

                    int eq = StringTokenizer.FindS(line, "=");

                    if (eq == 2)
                    {
                        //either FOR s = a, b, c...
                        string type = "string";
                        string name = line[pos + 1].s;
                        line[pos + 1].s = type + " " + "%" + name;

                        while (true)
                        {
                            eq = StringTokenizer.FindS(line, eq + 1, "=");
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
            else if (G.Equal(line[pos].s, "matrix"))
            {
                line[pos].meta.commandName = "matrix";

                string name = line[pos + 1].s;

                if (StringTokenizer.Equal(line, 2, "="))
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

                if (StringTokenizer.Equal(line, 2, "laspchain") || StringTokenizer.Equal(line, 2, "laspfixed"))
                {
                    string s = null;
                    for (int i = 2; i < line.Count; i++) s += line[i].ToString();
                    s = s.Trim(); if (s.EndsWith(";")) s = s.Substring(0, s.Length - 1);
                    int j = StringTokenizer.FindS(line[pos].subnodes.storage, ",");
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

            else if (G.Equal(line[pos].s, "ser") || G.Equal(line[pos].s, "series"))
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
                op_i = StringTokenizer.FindS(line, ii, new string[] { "=", "^", "%", "+", "*", "#" });  //cannot match series #m = ... or series <2010 2020> #m = ...

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

            else if (G.Equal(line[pos].s, "val"))
            {
                string name = line[pos + 1].s;

                if (StringTokenizer.Equal(line, 2, "="))
                {
                    line[pos].s = "%";
                    line[pos + 1].leftblanks = 0;
                }
            }

            else if (G.Equal(line[pos].s, "name") || G.Equal(line[pos].s, "string"))
            {
                string name = line[pos + 1].s;

                if (StringTokenizer.Equal(line, 2, "="))
                {
                    line[pos].s = "%";
                    line[pos + 1].leftblanks = 0;
                }
            }

            SetLineStartRecursive(line, line);


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
