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
        public static void DebugTokens(Cmd3Lexer lexer3)
        {
            G.Writeln("Debugging tokens (Globals.debugTokens)", Color.Orange);
            IToken token;
            for (int i = 0; i < 100; i++)
            {
                token = lexer3.NextToken();
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
    }

    public class ConvertHelper
    {
        public string code;
        public List<string> errors;
        public string commandsText;
        //public string codeUFunctions;        
    }
}
