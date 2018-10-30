/********************************************************8
 *	Author: Andrew Deren
 *	Date: July, 2004
 *	http://www.adersoftware.com
 * 
 *	StringTokenizer class. You can use this class in any way you want
 * as long as this header remains in this file.
 * 
 * TT changed the following: it reads Cp4xh1 as one word (not word + number + word + number)
 * And it reads .24232 as a number, and not as "." + "24232"
 * And it reads .254e-03 etc.
 * 
 **********************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gekko
{
    public class TokenList
    {
        //This is basically just a very simple list

        public List<TokenHelper> storage = new List<TokenHelper>();        

        public TokenList()
        {         
        }

        public TokenList(List<TokenHelper> x)
        {
            this.storage = x;
        }

        public int Count()
        {
            return this.storage.Count;
        }
        
        public TokenHelper this[int index]
        {

            // The get accessor.
            get
            {
                if (this.storage == null || index < 0 || index >= this.storage.Count) return null;
                return this.storage[index];
            }
        }

        public TokenList DeepClone(TokenHelper parent)
        {
            TokenList tsh = new TokenList();
            if (this.storage != null)
            {
                List<TokenHelper> xx = new List<TokenHelper>();
                foreach (TokenHelper th in this.storage)
                {
                    TokenHelper yy = th.DeepClone(parent);
                    xx.Add(yy);
                }
                tsh.storage = xx;
            }
            return tsh;
        }

        public override string ToString()
        {
            string s = null;
            foreach (TokenHelper th in this.storage)
            {
                s += th.ToString();
            }
            return s;
        }

        public string ToStringTrim()
        {            
            return ToString().Trim();
        }        
    }

    public class TokenHelperComma
    {
        public TokenHelper comma = null;
        public TokenList list = null;
        public TokenHelperComma(TokenHelper comma, TokenList list)
        {
            this.comma = comma;
            this.list = list;
        }

        public override string ToString()
        {
            string s = null;
            if (this.comma != null) s += this.comma.ToString();
            s += this.list.ToString();
            return s;
        }
    }

    public class TokenHelperMeta
    {
        //stash extra info here
        public string aremosCommandName = null;
    }

    public class TokenHelper
    {
        public string s = "";  //note that if subnodes != null, any string s here will be ignored. So you cannot BOTH has a string here, and a TokenList with subtokens. In this sense, the token containging the subtokens needs to be an empty placeholder.
        public ETokenType type = ETokenType.Unknown;
        public int leftblanks = 0; //if subnodes != null, leftblanks will always be = 0.
        public int line = -12345;
        public int column = -12345;
        //below is advanced (recursive) stuff        
        public TokenList subnodes = null;
        public TokenHelper parent = null;        
        public int id = -12345;
        public bool artificialTopNode = false; //if true, it has subnodes, and the subnodes are not enclosed in parentheses
        public TokenHelperMeta meta = new TokenHelperMeta(); //stash extra info here

        public TokenHelper DeepClone(TokenHelper parent)
        {
            TokenHelper th = new TokenHelper();
            th.s = this.s;
            th.type = this.type;
            th.leftblanks = this.leftblanks;
            th.column = this.column;
            th.parent = parent;
            th.id = this.id;
            th.artificialTopNode = this.artificialTopNode;             
            th.subnodes = this.subnodes;
            if (th.HasChildren()) th.subnodes = this.subnodes.DeepClone(th);
            return th;
        }       


        public TokenHelper()
        {
            
        }

        public TokenHelper(string s)
        {
            this.s = s;
            this.type = ETokenType.Word;
        }

        public TokenHelper(int leftblanks, string s)
        {
            this.leftblanks = leftblanks;
            this.s = s;
            this.type = ETokenType.Word;
        }

        public TokenHelper(int leftblanks, string s, ETokenType type)
        {
            this.leftblanks = leftblanks;
            this.s = s;
            this.type = type;
        }

        public TokenHelper(TokenList tl, string type)
        {
            //TokenHelper parent = new TokenHelper();
            this.subnodes = tl;
            if (type == null)
            {
                this.artificialTopNode = true;
            }
            else
            {
                //this.subnodesType = type;
            }
            OrganizeSubnodes(this);
        }

        /// <summary>
        /// Returns first item of subnodes which is also the type
        /// </summary>
        /// <returns></returns>
        public string SubnodesType()
        {
            if (!this.HasChildren()) return null;  //should not be called like this
            if (this.artificialTopNode) return Globals.artificial;
            return this.subnodes[0].s;  //the first item is also the subnode-type      
        }

        /// <summary>
        /// Returns true if the subnodes start with a left-parenthesis (, [ or {
        /// </summary>
        /// <returns></returns>
        public bool SubnodesTypeParenthesisStart()
        {
            if (this.SubnodesType() == "(" || this.SubnodesType() == "[" || this.SubnodesType() == "{") return true;
            return false;
        }

        /// <summary>
        /// Returns true if the subnodes end with a right-parenthesis ), ] or }
        /// </summary>
        /// <returns></returns>
        public bool SubnodesTypeParenthesisEnd()
        {
            if (this.SubnodesType() == ")" || this.SubnodesType() == "]" || this.SubnodesType() == "}") return true;
            return false;
        }

        /// <summary>
        /// True if subnodes == null
        /// </summary>
        /// <returns></returns>
        public bool HasNoChildren()
        {
            if (this.subnodes == null)
            {
                //leaf node
                //this.s can be = null here, if node has been cleared
                return true;
            }
            else
            {
                //internal node
                if (this.s != "")
                {
                    throw new GekkoException();  //must no contain anything
                }
                return false;
            }
        }

        /// <summary>
        /// False if subnodes == null
        /// </summary>
        /// <returns></returns>
        public bool HasChildren()
        {
            return !HasNoChildren();
        }

        public void Clear()
        {
            this.s = "";
            this.leftblanks = 0;
            this.subnodes = null;            
        }

        //if (ii - 1 >= 0) subnode.siblingBefore = temp.subnodes[ii - 1];
        //if (ii + 1 < temp.subnodes.storage.Count) subnode.siblingAfter = temp.subnodes[ii + 1];

        /// <summary>
        /// Splits up in bits, depending on commas. For instance [1, 2] is split in 1 and 2. But [1, [2, 3]] is split in 1 and [2, 3].            
        /// Note that the commas are preserved as the second part of the tuple.
        /// The last item (or the 1 item if there is only 1) will have null as second part of tuple.
        /// </summary>
        /// <returns></returns>

            

        public List<TokenHelperComma> SplitCommas(bool firstLast)
        {
            
            if (this.subnodes == null) return null;
            List<TokenHelperComma> temp = new List<TokenHelperComma>();
            TokenList temp2 = new TokenList();
            TokenHelper previousComma = null;
            int d = 0;
            if (firstLast) d = 1;
            for (int i = 0 + d; i < this.subnodes.storage.Count - d; i++)  //omit the parentheses
            {
                if (this.subnodes[i].s == ",")
                {
                    //TokenHelper thisComma = this.subnodes[i];
                    temp.Add(new TokenHelperComma(previousComma, temp2));
                    temp2 = new TokenList();
                    previousComma = this.subnodes[i];
                }
                else
                {
                    temp2.storage.Add(this.subnodes[i]);
                }
            }
            temp.Add(new TokenHelperComma(previousComma, temp2));
            return temp;
        }

        /// <summary>
        /// Returns the token to the left (null if not possible)
        /// </summary>
        /// <returns></returns>
        public TokenHelper SiblingBefore()
        {
            return this.Offset(-1);
        }

        /// <summary>
        /// /// Returns the token to the right (null if not possible)
        /// </summary>
        /// <returns></returns>
        public TokenHelper SiblingAfter()
        {
            return this.Offset(1);
        }

        public string LineAndPosText()
        {
            return "line " + this.line + " pos " + this.column;
        }

        /// <summary>
        /// Searches for specific strings
        /// </summary>
        /// <param name="i1Start"></param>
        /// <param name="ss"></param>
        /// <returns></returns>
        public int Search(int i1Start, List<string>ss)
        {
            int j = -12345;
            for (j = 0; j < int.MaxValue; j++)
            {                
                bool ok = true;
                for (int i = 0; i < ss.Count; i++)
                {
                    TokenHelper xx = this.Offset(i1Start + j + i);
                    if (xx == null)
                    {
                        return -12345;  //end of tokens
                    }
                    if (!G.Equal(xx.s, ss[i]))
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok) break;
            }
            return i1Start + j;
        }

        private static void OrganizeSubnodes(TokenHelper temp)
        {
            //temp is an empty node (.s == null) with subnodes
            int counter = -1;
            for (int ii = 0; ii < temp.subnodes.Count(); ii++)
            {
                TokenHelper subnode = temp.subnodes[ii];
                counter++;
                subnode.id = counter;
                subnode.parent = temp;
            }
            temp.line = temp.subnodes[0].line;
            temp.column = temp.subnodes[0].column;
        }
        
        /// <summary>
        /// Jump to previous/later tokens
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public TokenHelper Offset(int offset)
        {
            //-1 is left sibling, +1 is right sibling
            int ii = this.id + offset;
            if (ii < 0 || ii >= this.parent.subnodes.Count())
            {
                return null;
            }
            return this.parent.subnodes[ii];
        }

        /// <summary>
        /// Get an interval of tokens, put it into TokenHelper
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public TokenHelper OffsetInterval(int start, int end)
        {
            TokenList rv2 = new TokenList();
            for (int i = start; i <= end; i++)
            {
                rv2.storage.Add(this.parent.subnodes[this.id+i]);
            }
            TokenHelper rv = new TokenHelper(rv2, null);
            return rv;
        }

        /// <summary>
        /// Trimmed version of ToString()
        /// </summary>
        /// <returns></returns>
        public string ToStringTrim()
        {
            return this.ToString().Trim();
        }

        /// <summary>
        /// Loops through all subnodes to get string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (subnodes != null)
            {
                if (s != "" && !(s == Globals.artificial))
                {
                    G.Writeln2("*** ERROR: #875627897");
                    throw new GekkoException();
                }
                string ss = null;
                foreach (TokenHelper tha in subnodes.storage)
                {
                    ss += tha.ToString();
                }
                return ss;
            }
            else return G.Blanks(leftblanks) + s;
        }
                
        /// <summary>
        /// For debugging
        /// </summary>
        /// <param name="x"></param>
        /// <param name="level"></param>
        public static void Print(TokenList x, int level)
        {
            foreach (TokenHelper th in x.storage)
            {
                if (th.subnodes != null)
                {
                    Print(th.subnodes, level + 1);
                }
                else
                {
                    int b = 0;
                    if (th.leftblanks != 0) b = th.leftblanks;
                    string bb = null;
                    if (b > 0) bb = " [lb " + b.ToString() + "]";
                    G.Writeln(G.Blanks(2 * level) + th.ToString() + "                    " + th.type.ToString() + bb);
                }
            }
        }
    }

    /// <summary>
    /// StringTokenizer tokenized string (or stream) into tokens.
    /// </summary>
    public class StringTokenizer2
    {
        const char EOF = (char)0;

        int line;
        int column;
        int pos;    // position within data

        string data;

        bool ignoreWhiteSpace;
        char[] symbolChars;

        int saveLine;
        int saveCol;
        int savePos;

        bool specialLoopSignsAcceptedAsWords;
        bool treatQuotesAsUnknown;

        public List<Tuple<string, string>> commentsClosed = new List<Tuple<string, string>>();  //for instance /* ... */
        public List<string> commentsNonClosed = new List<string>();  //for instance //, or !! in GAMS 
        public List<Tuple<string, string>> commentsClosedOnlyStartOfLine = new List<Tuple<string, string>>(); //only at start of line, for instance $ontext ... $offtext
        public List<string> commentsNonClosedOnlyStartOfLine = new List<string>();  //only at start of line, for instance * in GAMS

        public StringTokenizer2(TextReader reader, bool specialLoopSignsAcceptedAsWords, bool treatQuotesAsUnknown)
        {
            this.specialLoopSignsAcceptedAsWords = specialLoopSignsAcceptedAsWords;
            this.treatQuotesAsUnknown = treatQuotesAsUnknown;
            if (reader == null)
                throw new ArgumentNullException("reader");
            data = reader.ReadToEnd();
            Reset();
        }

        public StringTokenizer2(string data, bool specialLoopSignsAcceptedAsWords, bool treatQuotesAsUnknown)
        {
            this.specialLoopSignsAcceptedAsWords = specialLoopSignsAcceptedAsWords;
            this.treatQuotesAsUnknown = treatQuotesAsUnknown;
            if (data == null)
                throw new ArgumentNullException("data");
            this.data = data;
            Reset();
        }

        /// <summary>
        /// gets or sets which characters are part of TokenKind.Symbol
        /// </summary>
        public char[] SymbolChars
        {
            get { return this.symbolChars; }
            set { this.symbolChars = value; }
        }

        /// <summary>
        /// if set to true, white space characters will be ignored,
        /// but EOL and whitespace inside of string will still be tokenized
        /// </summary>
        public bool IgnoreWhiteSpace
        {
            get { return this.ignoreWhiteSpace; }
            set { this.ignoreWhiteSpace = value; }
        }

        private void Reset()
        {
            this.ignoreWhiteSpace = false;
            this.symbolChars = new char[] { '=', '+', '-', '/', ',', '.', '*', '~', '!', '@', '#', '$', '%', '^', '&', '(', ')', '{', '}', '[', ']', ':', ';', '<', '>', '?', '|', '\\' };
            line = 1;
            column = 1;
            pos = 0;
        }

        protected char LA(int count)
        {
            if (pos + count < 0 || pos + count >= data.Length)
                return EOF;
            else
                return data[pos + count];
        }

        protected char Consume()
        {
            char ret = data[pos];
            pos++;
            column++;
            return ret;
        }

        protected Token CreateToken(ETokenType kind, string value)
        {
            return new Token(kind, value, line, column);
        }

        protected Token CreateToken(ETokenType kind)
        {
            string tokenData = data.Substring(savePos, pos - savePos);
            return new Token(kind, tokenData, saveLine, saveCol);
        }

        public bool MatchString(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (char.ToUpperInvariant(LA(i)) != char.ToUpperInvariant(s[i])) return false;
            }
            return true;
        }

        public Token Next()
        {
            ReadToken:
            char ch = LA(0);
            //if (ch == '\x0000') ch = '\x0001';

            foreach (Tuple<string, string> tags in commentsClosed)
            {
                if (MatchString(tags.Item1))
                {
                    //for instance '/*', look for the matching end, '*/'
                    return ReadCommentClosed(tags);
                }
            }

            foreach (string tag in commentsNonClosed)
            {
                if (MatchString(tag))
                {
                    //for instance '//'
                    return ReadCommentNonClosed(tag);
                }
            }

            //find \r, \rn or \n
            if (LA(-1) == EOF || LA(-1) == '\r' || LA(-1) == '\n' || (LA(-2) == '\r' && LA(-1) == 'n'))
            {
                //previous token was a newline or non-existing (so we are at the first char of the file)
                foreach (Tuple<string, string> tags in commentsClosedOnlyStartOfLine)
                {
                    if (MatchString(tags.Item1))
                    {
                        //for instance '$ontext', look for the matching end, '$offtext' (GAMS)
                        return ReadCommentClosed(tags);
                    }
                }

                foreach (string tag in commentsNonClosedOnlyStartOfLine)
                {
                    if (MatchString(tag))
                    {
                        //for instance '*' (GAMS)
                        return ReadCommentNonClosed(tag);
                    }
                }
            }

            switch (ch)
            {
                case EOF:
                    return CreateToken(ETokenType.EOF, string.Empty);

                case ' ':
                case '\t':
                    {
                        if (this.ignoreWhiteSpace)
                        {
                            Consume();
                            goto ReadToken;
                        }
                        else
                            return ReadWhitespace();
                    }
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ReadNumber(false);

                case '\r':
                    {
                        StartRead();
                        Consume();
                        if (LA(0) == '\n')
                            Consume();  // on DOS/Windows we have \r\n for new line
                        line++;
                        column = 1;
                        return CreateToken(ETokenType.EOL);
                    }
                case '\n':
                    {
                        StartRead();
                        Consume();
                        line++;
                        column = 1;
                        return CreateToken(ETokenType.EOL);
                    }

                case '"':
                    {
                        if (treatQuotesAsUnknown)
                        {
                            StartRead();
                            Consume();
                            return CreateToken(ETokenType.Unknown);
                        }
                        else
                        {
                            return ReadString();
                        }
                    }

                case '\'':
                    {
                        if (treatQuotesAsUnknown)
                        {
                            StartRead();
                            Consume();
                            return CreateToken(ETokenType.Unknown);
                        }
                        else
                        {
                            return ReadStringSingleQuotes();
                        }
                    }

                default:
                    {
                        if (ch == '.')
                        {
                            //Code added by TT
                            //In order to read .1234 as 0.1234
                            char ch1 = LA(1);
                            if (ch1 == '0' || ch1 == '1' || ch1 == '2' || ch1 == '3' || ch1 == '4' || ch1 == '5' || ch1 == '6' || ch1 == '7' || ch1 == '8' || ch1 == '9')
                            {
                                //we have a "." followed by a digit
                                return ReadNumber(true);
                            }
                        }

                        if (Char.IsLetter(ch) || ch == '_'
                            || (this.specialLoopSignsAcceptedAsWords == true && (ch == '#' || ch == '|'))) //TT added this
                            return ReadWord();
                        else if (IsSymbol(ch))
                        {
                            StartRead();
                            Consume();
                            return CreateToken(ETokenType.Symbol);
                        }
                        else
                        {
                            StartRead();
                            Consume();
                            return CreateToken(ETokenType.Unknown);
                        }
                    }

            }
        }

        /// <summary>
        /// save read point positions so that CreateToken can use those
        /// </summary>
        private void StartRead()
        {
            saveLine = line;
            saveCol = column;
            savePos = pos;
        }

        /// <summary>
        /// reads all whitespace characters (does not include newline)
        /// </summary>
        /// <returns></returns>
        protected Token ReadWhitespace()
        {
            StartRead();

            Consume(); // consume the looked-ahead whitespace char

            while (true)
            {
                char ch = LA(0);
                if (ch == '\t' || ch == ' ')
                    Consume();
                else
                    break;
            }

            return CreateToken(ETokenType.WhiteSpace);

        }

        /// <summary>
        /// reads number. Number is: DIGIT+ ("." DIGIT*)?
        /// </summary>
        /// <returns></returns>
        protected Token ReadNumber(bool hadDot)
        {
            StartRead();

            //hadDot introduced as method argument by TT
            //bool hadDot = false;

            Boolean hadExponent = false;
            Boolean hadPlusMinus = false;

            Consume(); // read first digit

            while (true)
            {

                char ch = LA(0);
                if (Char.IsDigit(ch))
                    Consume();
                else if (ch == '.' && !hadDot)
                {
                    hadDot = true;
                    Consume();
                }
                else if ((ch == 'e' || ch == 'E' || ch == 'd' || ch == 'D') && !hadExponent)  //Code (d/D) added by TT
                {
                    hadExponent = true;
                    Consume();
                }
                else if ((ch == '+' || ch == '-') && hadExponent && !hadPlusMinus)
                {
                    hadPlusMinus = true;
                    Consume();
                }
                else
                    break;
            }

            return CreateToken(ETokenType.Number);
        }

        /// <summary>
        /// reads word. Word contains any alpha character or _
        /// </summary>
        protected Token ReadWord()
        {
            StartRead();

            Consume(); // consume first character of the word

            while (true)
            {
                char ch = LA(0);
                if (Char.IsLetter(ch) || ch == '_'
                    || Char.IsDigit(ch)               //TT added this, in order to allow variable names like Cp4xh1
                    || (this.specialLoopSignsAcceptedAsWords == true && (ch == '#' || ch == '|'))  //TT added this
                    )
                    Consume();
                else
                    break;
            }

            return CreateToken(ETokenType.Word);
        }

        /// <summary>
		/// reads all characters until next " is found.
		/// If "" (2 quotes) are found, then they are consumed as
		/// part of the string
		/// </summary>
		/// <returns></returns>
		protected Token ReadString()
        {
            StartRead();
            Consume(); // read "
            while (true)
            {
                char ch = LA(0);
                if (ch == EOF)
                    break;
                else if (ch == '\r')    // handle CR in strings
                {
                    Consume();
                    if (LA(0) == '\n')  // for DOS & windows
                        Consume();
                    line++;
                    column = 1;
                }
                else if (ch == '\n')    // new line in quoted string
                {
                    Consume();
                    line++;
                    column = 1;
                }
                else if (ch == '"')
                {
                    Consume();
                    if (LA(0) != '"')
                        break;  // done reading, and this quotes does not have escape character
                    else
                        Consume(); // consume second ", because first was just an escape
                }
                else
                    Consume();
            }

            return CreateToken(ETokenType.QuotedString);
        }

        /// <summary>
		/// reads all characters until end of comment
		/// </summary>
		/// <returns></returns>
		protected Token ReadCommentClosed(Tuple<string, string> tags)
        {
            StartRead();
            for (int i = 0; i < tags.Item1.Length; i++)
            {
                Consume(); // consume tag, for instance '/*'
            }
            int nestingLevel = 1;
            while (true)
            {
                char ch = LA(0);
                if (ch == EOF)
                    break;
                else if (ch == '\r')    // handle CR in comments
                {
                    Consume();
                    if (LA(0) == '\n')  // for DOS & windows
                        Consume();
                    line++;
                    column = 1;
                }
                else if (ch == '\n')    // new line in comment
                {
                    Consume();
                    line++;
                    column = 1;
                }
                else if (MatchString(tags.Item1))
                {
                    //nested comment
                    nestingLevel++;
                    for (int i = 0; i < tags.Item1.Length; i++)
                    {
                        Consume(); // consume tag, for instance '/*'
                    }
                    //we are continuing from here
                }
                else if (MatchString(tags.Item2))
                {
                    //endtag found
                    nestingLevel--;
                    for (int i = 0; i < tags.Item2.Length; i++)
                    {
                        Consume(); // consume tag, for instance '*/'
                    }
                    if (nestingLevel == 0)
                    {
                        break;
                    }
                }
                else
                {
                    Consume();
                }
            }

            return CreateToken(ETokenType.Comment);
        }

        /// <summary>
		/// reads all characters until end of comment
		/// </summary>
		/// <returns></returns>
		protected Token ReadCommentNonClosed(string tag)
        {
            StartRead();
            for (int i = 0; i < tag.Length; i++)
            {
                Consume(); // consume tag, for instance '//'
            }
            while (true)
            {
                char ch = LA(0);
                if (ch == EOF)
                    break;
                else if (ch == '\r')    // handle CR in comment
                {
                    //Consume();
                    //if (LA(0) == '\n')  // for DOS & windows
                    //    Consume();
                    //line++;
                    //column = 1;
                    break;
                }
                else if (ch == '\n')    // new line in quoted string
                {
                    //Consume();
                    //line++;
                    //column = 1;
                    break;
                }
                else
                {
                    Consume();
                }
            }
            return CreateToken(ETokenType.Comment);
        }

        /// <summary>
        /// reads all characters until next ' is found.
        /// If '' (2 single quotes) are found, then they are consumed as
        /// part of the string
        /// </summary>
        /// <returns></returns>
        protected Token ReadStringSingleQuotes()
        {
            StartRead();
            Consume(); // read '
            while (true)
            {
                char ch = LA(0);
                if (ch == EOF)
                    break;
                else if (ch == '\r')    // handle CR in strings
                {
                    Consume();
                    if (LA(0) == '\n')  // for DOS & windows
                        Consume();
                    line++;
                    column = 1;
                }
                else if (ch == '\n')    // new line in quoted string
                {
                    Consume();
                    line++;
                    column = 1;
                }
                else if (ch == '\'')
                {
                    Consume();
                    if (LA(0) != '\'')
                        break;  // done reading, and this quotes does not have escape character
                    else
                        Consume(); // consume second ", because first was just an escape
                }
                else
                    Consume();
            }
            return CreateToken(ETokenType.QuotedString);
        }

        /// <summary>
        /// checks whether c is a symbol character.
        /// </summary>
        protected bool IsSymbol(char c)
        {
            for (int i = 0; i < symbolChars.Length; i++)
                if (symbolChars[i] == c)
                    return true;

            return false;
        }

        public static TokenList GetTokensWithLeftBlanks(string s)
        {
            return GetTokensWithLeftBlanks(s, 0);
        }

        public static TokenList GetTokensWithLeftBlanks(string s, int emptyTokensAtEnd)
        {
            return GetTokensWithLeftBlanks(s, emptyTokensAtEnd, null, null, null, null);
        }

        public static TokenList GetTokensWithLeftBlanks(string s, int emptyTokensAtEnd, List<Tuple<string, string>> commentsClosed, List<string> commentsNonClosed, List<Tuple<string, string>> commentsClosedOnlyStartOfLine, List<string> commentsNonClosedOnlyStartOfLine)
        {
            StringTokenizer2 tok = new StringTokenizer2(s, false, false);
            if (commentsClosed != null) tok.commentsClosed = commentsClosed;
            if (commentsNonClosed != null) tok.commentsNonClosed = commentsNonClosed;
            if (commentsClosedOnlyStartOfLine != null) tok.commentsClosedOnlyStartOfLine = commentsClosedOnlyStartOfLine;
            if (commentsNonClosedOnlyStartOfLine != null) tok.commentsNonClosedOnlyStartOfLine = commentsNonClosedOnlyStartOfLine;

            tok.IgnoreWhiteSpace = false;
            tok.SymbolChars = new char[] { '!', '#', '%', '&', '/', '(', ')', '=', '?', '@', '$', '{', '[', ']', '}', '+', '|', '^', '¨', '~', '*', '<', '>', '\\', ';', ',', ':', '.', '-' };
            Token token;
            int numberCounter = 0;
            List<TokenHelper> a = new List<TokenHelper>();
            int white = 0;
            do
            {
                token = tok.Next();  //this is where the action is!
                string value = token.Value;
                ETokenType kind = token.Kind;
                TokenHelper two = new TokenHelper();
                two.s = value;
                two.type = kind;
                two.leftblanks = white;

                if (kind == ETokenType.WhiteSpace)
                {
                    white = value.Length;
                }
                else
                {
                    two.line = token.Line;
                    two.column = token.Column;
                    a.Add(two);
                    white = 0;
                }

            } while (token.Kind != ETokenType.EOF);
            for (int i = 0; i < emptyTokensAtEnd; i++) a.Add(new TokenHelper());
            return new TokenList(a);
        }

        //public static TokenHelper GetTokensWithLeftBlanksRecursive(string textInputRaw)
        //{
        //    return GetTokensWithLeftBlanksRecursive(textInputRaw, null, null, null, null);
        //}

        public static TokenHelper GetTokensWithLeftBlanksRecursive(string textInputRaw, List<Tuple<string, string>> commentsClosed, List<string> commentsNonClosed, List<Tuple<string, string>> commentsClosedOnlyStartOfLine, List<string> commentsNonClosedOnlyStartOfLine)
        {
            //TokenList is basically just a very simple List<TokenHelper>
            //TokenHelper contains the token and other info, and may contain a TokenList (for instance the contents of a (...) parenthesis)
            //
            //             TokenHelper                    this node is artificial and contains nothing but a TokenList
            //                 |
            //                 |
            //              TokenList                     a + (b + c) --> at this level only a + (...) is seen
            //              /     \
            //             /       \
            //      TokenHelper    TokenHelper            the TokenHelper with children will be empty with a TokenList containing what is inside the parenthesis. Before and after this token there will be '(' and ')'
            //                        /
            //                       /
            //                  TokenList                 b + c --> at this level only b + c is seen
            //                  /       \
            //                 /         \
            //          TokenHelper    TokenHelper


            int i = 0;
            TokenList tokens = GetTokensWithLeftBlanks(textInputRaw, 0, commentsClosed, commentsNonClosed, commentsClosedOnlyStartOfLine, commentsNonClosedOnlyStartOfLine);
            TokenList tokens2 = GetTokensWithLeftBlanksRecursiveHelper(tokens, ref i, null);
            //the first-level elements of the TokenList do not have any parent. This is fixed here:
            TokenHelper parent = new TokenHelper(tokens2, null);
            //parent.subnodes = tokens2;
            //parent.subnodesType = "artificial_parent_at_the_top_of_the_node_tree";
            //OrganizeSubnodes(parent);

            //int counter = -1;
            //foreach (TokenHelper token in parent.subnodes.storage)
            //{
            //    counter++;
            //    token.parent = parent;
            //    token.id = counter;
            //}
            return parent;
        }

        

        public static TokenList GetTokensWithLeftBlanksRecursiveHelper(TokenList input, ref int startI, TokenHelper startparen)
        {
            TokenList rv = new TokenList();  
            
            
                      
            List<TokenHelper> output = new List<TokenHelper>();
            //if (first != null) output.Add(first);  //a left parenthesis      
            string endparen = null;
            if (startparen != null)
            {
                Globals.parentheses.TryGetValue(startparen.s, out endparen);
                output.Add(input.storage[startI - 1]);  //add the left parenthesis here
            }
            for (int i = startI; i < input.storage.Count; i++)
            {
                if (Globals.parentheses.ContainsKey(input[i].s))
                {
                    //found a new left parenthesis                          
                    startI = i + 1;
                    TokenList sub = GetTokensWithLeftBlanksRecursiveHelper(input, ref startI, input[i]);
                    TokenHelper temp = new TokenHelper(sub, input.storage[i].s);  //new empty/placeholder TokenHelper with a list of TokenHelpers
                    //temp.subnodes = sub;
                    //temp.subnodesType =;
                    //OrganizeSubnodes(temp);
                    output.Add(temp);
                    i = startI;
                }
                else if (endparen != null && input[i].s == endparen)
                {
                    //got to the end
                    startI = i;
                    output.Add(input[i]);  //add the right parenthesis here                                                         
                    return new TokenList(output);
                }
                else
                {
                    if (Globals.parenthesesInvert.ContainsKey(input.storage[i].s))
                    {
                        G.Writeln2("*** ERROR: The '" + input[i].s + "' parenthesis at " + input[i].LineAndPosText() + " does not have a corresponding '" + Globals.parenthesesInvert[input[i].s] + "'");
                        throw new GekkoException();
                    }
                    output.Add(input[i]);
                    //input[i].parent = rv;
                }
            }
            if (endparen != null)
            {
                G.Writeln2("*** ERROR: The '" + startparen.s + "' parenthesis at " + startparen.LineAndPosText() + " does not have a corresponding '" + endparen + "'");
                throw new GekkoException();
            }

            TokenList temp3 = new TokenList(output);
            
            return temp3;
        }

        
    }
}
