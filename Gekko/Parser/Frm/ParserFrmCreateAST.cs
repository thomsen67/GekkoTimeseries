using System;
using System.Collections.Generic;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Antlr.Runtime.Debug;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using CT = Antlr.Runtime.Tree.CommonTree;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Gekko.Parser.Frm
{
    /// <summary>
    /// This class is used to create an AST from a .frm (model) file. There is a similar class for .get (script) files.
    /// </summary>    
    class ParserFrmCreateAST
    {
        public static GekkoDictionary<string, string> ParserFrmCreateASTHelper(string textInput, string modelName)
        {
            //[[2]]
            ANTLRStringStream input = new ANTLRStringStream(textInput + "\n");  //a newline for ease of use of ANTLR

            List<string> errors = null;
            CommonTree t = null;

            // Create a lexer attached to that input
            ModelLexer lexer = new ModelLexer(input);
            // Create a stream of tokens pulled from the lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            // Create a parser attached to the token stream
            ModelParser parser = new ModelParser(tokens);
            // Invoke the program rule in get return value
            ModelParser.expr_return r = null;
            DateTime t0 = DateTime.Now;

            //takes 0.5 sec for dec09
            r = parser.expr();

            errors = parser.GetErrors();
            t = (CommonTree)r.Tree;

            if (Globals.printAST)
            {
                AST(t, 0);
            }

            WalkHelper wh = CreateWalkHelper(textInput);
            wh.frmlItems = parser.GetFrmlItems();

            List<string> inputFileLines = wh.inputFileLines;

            ParseHelper ph = new ParseHelper();
            ph.isOneLinerFromGui = false;
            ph.isModel = true;
            ph.fileName = modelName;

            if (errors.Count > 0)
            {
                PrintModelParserErrors(errors, inputFileLines, ph);
                throw new GekkoException();
            }
            else
            {
                //G.Writeln("No errors when parsing");
            }

            GekkoDictionary<string, string> vals = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (CT child in t.Children)
            {
                if (child.Text == "ASTVAL")
                {
                    string key = child.GetChild(0).Text;
                    string value = child.GetChild(1).Text;
                    if (child.GetChild(2) != null)
                    {
                        if (G.Equal(child.GetChild(2).Text, "-"))
                        {
                            value = "-" + value;
                        }
                    }

                    if (vals.ContainsKey(key))
                    {
                        G.Writeln2("*** ERROR: Model parsing error: seems VAL '" + key + "' is defined several times.");
                        throw new GekkoException();
                    }
                    else
                    {
                        string key2 = key;
                        if (!key.StartsWith(Globals.symbolScalar.ToString())) key2 = Globals.symbolScalar + key;
                        vals.Add(key2, value);
                    }
                }
            }

            ASTNodeSimple equationNode = new ASTNodeSimple(null);  //unknown text for now
            wh.print = false;
            //takes 0.1 sec for dec09
            CreateASTNodesForModel(t, equationNode, 0, wh, Program.model.modelGekko); //creates a List<> of equations, with a tree of EquationNodes for each equation

            if (wh.print) wh.writer.Close();
            return vals;
        }

        public static void CreateASTNodesForModel(CT ast, ASTNodeSimple equationNode, int depth, WalkHelper wh, ModelGekko model)
        {
            //[[3]]
            if (ast.Text == "ASTVAL")
            {
                //nothing done here
            }
            if (ast.Text == "ASTMODELBLOCK")
            {
                string s = ast.GetChild(0).Text;
                string[] ss = s.Split(new string[] { "###" }, StringSplitOptions.None);
                wh.modelBlock = ss[1].Trim();  //since it comes from the parser, there are always 2 or 3 elements in ss
                if (wh.modelBlock.Length > 0)
                {
                    //first letter always set to upper-case
                    wh.modelBlock = char.ToUpper(wh.modelBlock[0]) + wh.modelBlock.Substring(1);
                }
            }
            if (ast.Text == "ASTAFTER")
            {
                if (wh.after2Encountered)
                {
                    G.Writeln2("*** ERROR: Expected AFTER$ before AFTER2$ in model");
                    throw new GekkoException();
                }
                if (wh.afterEncountered)
                {
                    G.Writeln2("*** ERROR: It seems there are more than one AFTER$ in model");
                    throw new GekkoException();
                }
                wh.afterEncountered = true;
            }
            if (ast.Text == "ASTAFTER2")
            {
                if (wh.after2Encountered)
                {
                    G.Writeln2("*** ERROR: It seems there are more than one AFTER2$ in model");
                    throw new GekkoException();
                }
                wh.after2Encountered = true;
            }
            equationNode.Text = ast.Text;
            EquationHelper helper = null;
            if (depth == 1 && ast.Text == "ASTFRML")
            {
                wh.frmlItemsCounter++;
                equationNode = new ASTNodeSimple(ast.Text);  //the root node for this equation
                equationNode.parent = null;  //has no parent
                helper = new EquationHelper();
                helper.modelBlock = wh.modelBlock;
                helper.equationNumber = model.equations.Count;
                helper.equationsNodeRoot = equationNode;
                //if( helper.isAfterModel
                if (wh.afterEncountered) helper.isAfterModel = true;
                if (wh.after2Encountered) helper.isAfter2Model = true;  //can be both at same time
                model.equations.Add(helper);
                //wh.eqs++;
                wh.positionInFileStart.line = ast.Line - 1; //subtract 1 to make it 0-based
                wh.positionInFileStart.charPosition = ast.CharPositionInLine;
            }

            wh.positionInFileEnd.LookForLargerPosition(ast);

            if (wh.print) for (int i = 0; i < depth; i++) Console.Write("    ");
            if (wh.print) Console.WriteLine(ast.Text);

            if (wh.printFile) for (int i = 0; i < depth; i++) wh.writer.Write("    ");
            if (wh.printFile) wh.writer.WriteLine(ast.Text);

            if (ast.Children == null)
            {
                return;
            }

            int num = ast.Children.Count;
            equationNode.Children = new List<ASTNodeSimple>(num);

            //Console.WriteLine();
            for (int i = 0; i < num; ++i)
            {
                CT d = (CT)(ast.Children[i]);
                ASTNodeSimple equationNodeChild = new ASTNodeSimple(null);  //unknown text
                equationNodeChild.parent = equationNode;
                equationNode.Add(equationNodeChild);
                CreateASTNodesForModel(d, equationNodeChild, depth + 1, wh, model);
                if (i < num - 1)
                {
                    //Console.WriteLine();
                }
            }

            if (depth == 1 && ast.Text == "ASTFRML")
            {
                helper.equationText = wh.frmlItems[wh.frmlItemsCounter];
            }
        }

        public static void AST(CT node, int depth)
        {
            G.Writeln(G.Blanks(depth * 2) + node.Text);
            //G.Writeln(G.Blanks(depth * 2) + node.Text + "    line:" + node.Line);
            if (node.Children != null)
            {
                for (int i = 0; i < node.Children.Count; ++i)
                {
                    CT child = (CT)(node.Children[i]);
                    AST(child, depth + 1);
                }
            }
        }

        private static WalkHelper CreateWalkHelper(string textInput)
        {
            WalkHelper wh = new WalkHelper();
            wh.print = false;
            wh.printFile = false;
            wh.inputFile = textInput;
            wh.inputFileLines = Program.CreateListOfStringsFromString(wh.inputFile);
            return wh;
        }

    }
}
