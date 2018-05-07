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
	/// <summary>
	/// StringTokenizer tokenized string (or stream) into tokens.
	/// </summary>
	public class StringTokenizer2
	{
		const char EOF = (char)0;

		int line;
		int column;
		int pos;	// position within data

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
				return data[pos+count];
		}

		protected char Consume()
		{
			char ret = data[pos];
			pos++;
			column++;
			return ret;
		}

		protected Token CreateToken(TokenKind kind, string value)
		{
			return new Token(kind, value, line, column);
		}

		protected Token CreateToken(TokenKind kind)
		{
			string tokenData = data.Substring(savePos, pos-savePos);
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

			switch (ch)
			{
				case EOF:
					return CreateToken(TokenKind.EOF, string.Empty);

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
						Consume();	// on DOS/Windows we have \r\n for new line
					line++;
					column=1;
					return CreateToken(TokenKind.EOL);
				}
				case '\n':
				{
					StartRead();
					Consume();
					line++;
					column=1;					
					return CreateToken(TokenKind.EOL);
				}

				case '"':
				{
                    if (treatQuotesAsUnknown)
                    {                        
                        StartRead();
                        Consume();
                        return CreateToken(TokenKind.Unknown);
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
                            return CreateToken(TokenKind.Unknown);
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
						return CreateToken(TokenKind.Symbol);
					}
					else
					{
						StartRead();
						Consume();
						return CreateToken(TokenKind.Unknown);						
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

			return CreateToken(TokenKind.WhiteSpace);
			
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

			return CreateToken(TokenKind.Number);
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

			return CreateToken(TokenKind.Word);
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

            return CreateToken(TokenKind.QuotedString);
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

            return CreateToken(TokenKind.Comment);
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
				else if (ch == '\r')	// handle CR in strings
				{
					Consume();
					if (LA(0) == '\n')	// for DOS & windows
						Consume();
					line++;
					column = 1;
				}
				else if (ch == '\n')	// new line in quoted string
				{
					Consume();
					line++;
					column = 1;
				}
				else if (ch == '\'')
				{
					Consume();
					if (LA(0) != '\'')
						break;	// done reading, and this quotes does not have escape character
					else
						Consume(); // consume second ", because first was just an escape
				}
				else
					Consume();
			}
			return CreateToken(TokenKind.QuotedString);
		}

		/// <summary>
		/// checks whether c is a symbol character.
		/// </summary>
		protected bool IsSymbol(char c)
		{
			for (int i=0; i<symbolChars.Length; i++)
				if (symbolChars[i] == c)
					return true;

			return false;
		}
	}
}
