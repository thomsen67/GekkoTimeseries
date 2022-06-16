using System;
using System.Collections.Generic;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace Gekko.Parser
{
    public class ParserCommon
    {
        /// <summary>
        /// Show info on individual tokens. If lexer3==null, it takes lexer4.
        /// </summary>
        /// <param name="lexer3"></param>
        /// <param name="lexer4"></param>
        public static void DebugTokens(Cmd3Lexer lexer3, Cmd4Lexer lexer4)
        {
            G.Writeln("Debugging tokens (Globals.debugTokens)", Color.Orange);
            IToken token;
            for (int i = 0; i < 100; i++)
            {
                token = null;
                if (lexer3 != null) token = lexer3.NextToken();
                else token = lexer4.NextToken();
                string s1 = token.Text;
                if (s1 == "{") s1 = "[leftcurly]";
                if (s1 == "}") s1 = "[rightcurly]";
                int i2 = token.Type;
                string s = "Token " + i + ": '" + s1 + "' " + i2.ToString();
                Console.WriteLine(s, Color.Orange);
            }
            MessageBox.Show("See console, 100 tokens printed. See start of Cmd3Lexer.cs to translate numbers.");
        }

    }

    public class ParseHelper
    {
        public bool isOneLinerFromGui = false;
        public string commandsText = null;
        public string fileName = null;
        public bool isModel = false;
        public string libraryName = null;
    }

    public class ConvertHelper
    {
        public string code;
        public List<string> errors;
        public string commandsText;
        //public string codeUFunctions;        
    }
}
