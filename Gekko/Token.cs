/********************************************************
 *	Author: Andrew Deren
 *	Date: July, 2004
 *	http://www.adersoftware.com
 * 
 *	StringTokenizer class. You can use this class in any way you want
 * as long as this header remains in this file.
 * 
 **********************************************************/

using System;

namespace Gekko
{
	public enum ETokenType
	{
		Unknown,
		Word,
		Number,
		QuotedString,
        Comment,
		WhiteSpace,
		Symbol,
		EOL,
		EOF,
        Null  //used to signal out of range
	}

	public class Token
	{
		int line;
		int column;
		string value;
		ETokenType kind;

		public Token(ETokenType kind, string value, int line, int column)
		{
			this.kind = kind;
			this.value = value;
			this.line = line;
			this.column = column;
		}

		public int Column
		{
			get { return this.column; }
		}

		public ETokenType Kind
		{
			get { return this.kind; }
		}

		public int Line
		{
			get { return this.line; }
		}

		public string Value
		{
			get { return this.value; }
		}
	}
}
