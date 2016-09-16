/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2016, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.        
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Gekko.Parser;
using System.Collections;
using System.Text.RegularExpressions;

//
//                              TRANSLATING FROM AREMOS, see T2.g
//

namespace Gekko
{
    
    public class Translator2
    {
        public static bool hasCloseall = false;
        public static void TranslateTree(ASTNode2 node, int depth, StringBuilder sb, GekkoDictionary<string, string> listMemory, GekkoDictionary<string, string> matrixMemory,  GekkoDictionary<string, string> scalarMemory)
        {
            if (node.Parent == null && node.Text == null)
            {
                //Root node
            }
            else
            {
                if (G.equal(node.Text, "cmd") && node.GetPrevious() != null && node.GetPrevious().Text == ".")
                {
                    node.Text = Globals.extensionCommand;
                }

                if (node.Parent.Text == "ASTCOMMAND1")
                {
                    HandleCommand1(node, listMemory, matrixMemory, scalarMemory);
                }
                else if ((node.Parent.Parent != null && node.Parent.Parent.Text == "ASTCOMMAND2") && node.Parent.Text == "ASTANGLE")
                {
                    HandleCommand2(node);
                }
                else //not ASTCOMMAND1 or 2, often ASTCOMMAND3, but could be ASTLEFTPAREN etc.
                {
                    HandleCommand3(node, listMemory, matrixMemory, scalarMemory);                    
                }                
            }            

            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            // ------------------ sub start -----------------------
            for (int i = 0; i < node.ChildrenCount(); ++i)
            {
                TranslateTree(node[i], depth + 1, sb, listMemory, matrixMemory, scalarMemory);
            }
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
            // ------------------ sub end -----------------------
                        
            bool nodeIsFinished = HandleLastStuffInCode(node);
            if (!nodeIsFinished) GetCodeFromAllChildren(node);
            node.Code += node.rightTextExtra;
        }

        private static bool HandleLastStuffInCode(ASTNode2 node)
        {
            bool nodeIsFinished = false;
            node.Code += node.leftBlanks;
            node.Code += node.leftTextExtra;
            if (node.ChildrenCount() == 0)
            {
                if (!node.Text.StartsWith("AST")) node.Code += node.Text;
            }

            {
                //not a leaf node
                if (node.Text == "ASTCOMMAND3")
                {
                    //rest of command
                    ASTNode2 parent = node.Parent;  //ASTCOMMAND                                                          

                    if (parent.commandTypeAremosLower == "assign")
                    {
                        bool found = false;
                        string varName = node[0].Code;
                        string assignType = null;

                        string s = HandleAssign1FromTo(node, "lit", "literal");
                        if (s != null)
                        {
                            assignType = HandleAssign2(node, assignType, s, "name");
                            found = true;
                        }

                        s = HandleAssign1FromTo(node, "str", "string");
                        if (s != null)
                        {
                            assignType = HandleAssign2(node, assignType, s, "string");
                            found = true;
                        }

                        //
                        //
                        // --------->  todo........ DATE. Convert ASSIGN < GLOBAL > PER1 LITERAL "1966A1"; ---> to DATE per1 = 1966A1!
                        //
                        //

                        s = HandleAssign1FromTo(node, "val", "value");
                        if (s != null)
                        {
                            assignType = HandleAssign2(node, assignType, s, "val");
                            found = true;
                        }

                        s = HandleAssign1FromTo(node, "int", "integer");
                        if (s != null)
                        {
                            assignType = HandleAssign2(node, assignType, s, "val");
                            found = true;
                        }

                        if (found)
                        {

                            string rhs = null;
                            for (int i = 2; i < node.ChildrenCount(); i++)
                            {
                                rhs += node[i].Code;
                            }
                            node.Code = varName + " = " + rhs;
                            ASTNode2 xx = node.GetCommand1();

                            xx.Code = xx.leftBlanks + xx[0].leftBlanks + assignType;  //changing in COMMAND1, hack with blanks...
                            string dateHit = HandleDateHiddenAsLiteral(assignType, rhs);
                            if (dateHit == null) dateHit = HandleDateHiddenAsInteger(assignType, rhs);
                            if (dateHit != null)
                            {
                                node.Code = varName + " = " + dateHit;
                                node.GetCommand1().Code = "date";  //changing in COMMAND1
                            }

                            nodeIsFinished = true;  //because we fetch sub-nodes manually
                        }
                        else
                        {
                            //if assigntype is not found, it will not be transformed but stay as "assign... "
                            //note: nodeIsFinished is not touched here
                        }
                    }  //end of assign

                }  //end of if (node.Text == "ASTCOMMAND3")              
            }
            return nodeIsFinished;
        }

        private static void HandleCommand3(ASTNode2 node, GekkoDictionary<string, string> listMemory, GekkoDictionary<string, string> matrixMemory, GekkoDictionary<string, string> scalarMemory)
        {
            if (GetCommandType(node) == "delete" && G.equal(node.Text, "series"))
            {
                ASTNode2 xx = node.GetCommand2();
                node.GetCommand2().AddOptionAfterVisitor("series");
                node.Text = "";         
            }

            //Handle '123.' etc.
            if (node.Text.Length > 1 && node.Text.EndsWith("."))
            {
                if (G.IsIntegerTranslate(node.Text.Substring(0, node.Text.Length - 1)))
                {
                    //is like '12345.' or '5.' 
                    node.Text += "0";
                    //transforms '12345.' into '12345.0'
                    //Gekko does not like these end dots, they interfere with range (..) indicator.
                }
            }

            //Handle '123.' etc.
            if (node.Text == "(")
            {
                ASTNode2 temp = node.GetPrevious();
                if (temp != null)
                {                    
                    if (KnownFunction(temp.Text))
                    {
                        if (node.leftBlanks != "") node.leftBlanks = "";
                    }
                }
                else
                {
                    if (node.Parent != null)
                    {
                        if (node.Parent.Text == "ASTPAREN")
                        {
                            ASTNode2 temp2 = node.Parent.GetPrevious();
                            if (temp2 != null)
                            {
                                if (KnownFunction(temp2.Text))
                                {
                                    node.leftBlanks = "";  //log () --> log()
                                }
                            }
                        }
                    }
                }
            }           


            if (G.equal(node.Text, "#"))
            {
                // #
                bool isLeftHandSideOfEquals = false;
                //if (node.id == 0)
                //{
                //    if (GetCommandType(node) == "assign" || GetCommandType(node) == "matrix" || GetCommandType(node) == "series")
                //    {
                //        isLeftHandSideOfEquals = true;
                //    }
                //}                  
                             

                if (!isLeftHandSideOfEquals) // not the left-hand side 
                {
                    ASTNode2 next = node.Parent.GetChild(node.id + 1);
                    if (next != null && G.IsIdentTranslate(next.Text))
                    {
                        if (GetCommandType(node) == "list")
                        {
                            //For a LIST statement, the logic is a bit inverse. Here we will expect lists 
                            //per default, and they must be proven to be scalars to avoid that
                            if (scalarMemory.ContainsKey(next.Text))
                            {
                                node.Text = "%";  //assign-vars in aremos
                            }
                            else
                            {
                                //do nothing, we presume it is a list when it is not a scalar
                            }
                        }
                        else
                        {
                            if (listMemory.ContainsKey(next.Text))
                            {
                                //do nothing, it is a list
                            }
                            else
                            {
                                node.Text = "%";  //assign-vars in aremos
                            }
                        }
                    }
                    if (next != null && next.Text == "#")
                    {
                        node.Text = "%";
                    }
                }
            }

            if (G.equal(node.Text, "+"))
            {
                if (GetCommandType(node) == "list") node.Text = "&+";  //                        
            }

            if (G.equal(node.Text, "*"))
            {
                if (GetCommandType(node) == "list") node.Text = "&*";  //                        
            }

            if (G.equal(node.Text, "-"))
            {
                if (GetCommandType(node) == "list") node.Text = "&-";  //                        
            }

            //if (G.Equal(node.Text, FromTo("rep", "repeat")) != null && node.GetNext().Text == "*")
            //{
            //    // rep *
            //    // TODO: Check if any ',' tokens here, if so keep *                        
            //    node.Text = "";
            //    node.GetNext().Text = "";
            //}

            if (G.IsIdentTranslate(node.Text) && matrixMemory.ContainsKey(node.Text))
            {
                // abc --> #abc, if we have seen a matrix abc = ...
                node.Text = "#" + node.Text;
            }

            if (node.Text == "|")
            {
                ASTNode2 x = node.GetNext();
                if (x != null)
                {

                    bool hit = false;
                    if (x.Text == "+" || x.Text == "-" || x.Text == "*" || x.Text == "/" || x.Text == "**" || x.Text == "^" || x.Text == ")" || x.Text == "]" || x.Text == "=" || x.Text == ">" || x.Text == "<" || x.Text == "," || x.Text == ":" || x.Text == ";")
                    {
                        node.Text = "";  //remove it, for instance a%s|*fY or a%s,b%s
                        hit = true;
                    }
                    if (!hit)
                    {
                        x = node.GetPrevious();
                        if (x != null)
                        {
                            if (x.Text == "+" || x.Text == "-" || x.Text == "*" || x.Text == "/" || x.Text == "**" || x.Text == "^" || x.Text == "(" || x.Text == "[" || x.Text == "=" || x.Text == ">" || x.Text == "<" || x.Text == "," || x.Text == ":" || x.Text == ";")
                            {
                                node.Text = "";  //remove it, for instance fy+|%s+...
                            }
                        }
                    }
                }
                else
                {
                    node.Text = "";  //last token is just before the ';', for instance ...+a%s|;
                }
            }

            if (false)  //messes up filenames with extensions.....
            {
                if (node.Text == "." && G.IsIdentTranslate(node.GetNext().Text) && node.GetNext().leftBlanks == "")
                {
                    // .tot etc.
                    if (G.equal(node.GetNext().Text, "a") || G.equal(node.GetNext().Text, "q") || G.equal(node.GetNext().Text, "m"))
                    {
                        //do nothing
                    }
                    else
                    {
                        node.Text = "__";  //we use this instead
                    }
                }
            }

            //quotes
            if (node.Text.StartsWith("!"))
            {
                node.Text = "//" + node.Text.Substring(1);
            }

            //double quotes to single quotes
            if (node.Text.StartsWith("\"") && node.Text.EndsWith("\""))
            {
                node.Text = "'" + node.Text.Substring(1, node.Text.Length - 2) + "'";
            }

            //subst inside quotes
            if (node.Text.StartsWith("'") && node.Text.EndsWith("'"))
            {
                //quote-string, replace TELL'value: #c' ==> TELL'value: %c' 
                node.Text = node.Text.Replace("#", "%");
            }

            if (G.equal(node.Text, "repeat"))
            {
                node.Text = Cap("rep", node.Text);
            }

            if (node.Text == "ASTCOMMAND3" && node.Parent.commandTypeAremosLower == "restore")
            {
                ASTNode2 xx = node.GetLastChild();
                if (xx != null && !xx.Text.ToLower().EndsWith("opt")) xx.Text = xx.Text + ".opt";
            }

            if (false)  //only if we need to change soft parentheses...
            {
                if (node.Text == "ASTPAREN")
                {
                    if (node.GetPrevious() != null && G.IsIdentTranslate(node.GetPrevious().Text))
                    {
                        //is like this: abc(...), now we check the inside
                        if (node.ChildrenCount() == 4)
                        {
                            if (node.GetChild(1).Text == "+" || node.GetChild(1).Text == "-")
                            {
                                if (G.IsIntegerTranslate(node.GetChild(2).Text))
                                {
                                    //is like this: abc(-12) or abc(+5), seems like a lag/lead
                                    if (G.equal(node.GetPrevious().Text, "log") || G.equal(node.GetPrevious().Text, "exp"))
                                    {
                                        //do nothing
                                    }
                                    else
                                    {
                                        if (node.GetChild(0).Text == "(" && node.GetChild(3).Text == ")")
                                        {
                                            //must be so, but checking for safety
                                            node.GetChild(0).Text = "[";
                                            node.GetChild(3).Text = "]";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Add IF-parenteses, for instance IF #v > 100; SERIES x = 200; END; ==> IF (#v > 100); SERIES x = 200; END
            if (node.Text == "ASTCOMMAND3" && node.Parent.commandTypeAremosLower == "if")
            {
                ASTNode2 temp1 = node.GetFirstChild();
                ASTNode2 temp2 = node.GetLastChild();
                if (temp1.Text != "ASTPAREN")
                {
                    if (!temp1.Text.StartsWith("AST"))
                    {                        
                        temp1.AddToLeftTextExtra("(");
                        temp2.AddToRightTextExtra(")");                        
                    }
                }                
            }

            //finds SERIES with if-then-else
            if (node.Text == "ASTCOMMAND3" && node.Parent.commandTypeAremosLower == "series")
            {
                int count1 = 0;
                int count2 = 0;
                foreach (ASTNode2 xx in node.ChildrenIterator())
                {
                    if (G.equal(xx.Text, "if")) count1++;
                    if (G.equal(xx.Text, "then")) count2++;
                }
                if (count1 > 0 && count2 > 0)
                {
                    node.GetCommand3().AddComment("Please use iif() function");
                }
            }

            //finds COPY .... TO e:; ==> COPY .... TO e:*;
            if (node.Text == "ASTCOMMAND3" && node.Parent.commandTypeAremosLower == "copy")
            {
                ASTNode2 temp = node.GetLastChild();
                if (temp != null && temp.Text == ":")
                {
                    ASTNode2 temp2 = temp.GetPrevious();
                    if (temp2 != null && G.IsIdentTranslate(temp2.Text))
                    {
                        ASTNode2 temp3 = temp2.GetPrevious();
                        if (temp3 != null && G.equal(temp3.Text, "to"))
                        {
                            temp.AddToRightTextExtra("*");
                        }
                    }
                }
            }
                       


            //finds FOR ... TO ... and replaces with "FOR date ..." or "FOR val ..."
            if (node.Text == "ASTCOMMAND3" && node.Parent.commandTypeAremosLower == "for")
            {
                int perCount = 0;
                int dateCount = 0;
                int goodDateCount = 0;
                int badValCount = 0;
                bool by = false;
                bool to = false;
                bool equal = false;
                bool comma = false;
                foreach (ASTNode2 xx in node.ChildrenIterator())
                {
                    if (G.equal(xx.Text, "by")) by = true;
                    if (G.equal(xx.Text, "to")) to = true;
                    if (xx.Text == "=") equal = true;
                    if (xx.Text == ",") comma = true;
                    if (equal && !by)
                    {
                        ASTNode2 prev = xx.GetPrevious();
                        if (prev != null && prev.Text == "#")
                        {
                            if (xx.Text.ToLower().StartsWith("per")) perCount++;
                        }
                        if (IsDate(xx.Text))  //eats integer
                        {
                            dateCount++;
                            if (xx.Text.Contains("a") || xx.Text.Contains("A") || xx.Text.Contains("q") || xx.Text.Contains("Q") || xx.Text.Contains("m") || xx.Text.Contains("M")) goodDateCount++;
                        }
                        else
                        {
                            double slet = double.NaN;
                            if (double.TryParse(xx.Text, out slet))  //2005.01
                            {
                                badValCount++;
                            }
                        }
                    }
                }
                if (to && !comma)
                {
                    if (goodDateCount > 0 || ((perCount > 0 || dateCount > 0) && badValCount == 0))
                    {
                        node.GetCommand1().Code += " date ";
                    }
                    else
                    {
                        node.GetCommand1().Code += " val ";
                    }
                }
            }

            //Handle '< =', '= =', '> =', '< >', with blanks, 
            if (node.Text == "=")
            {
                ASTNode2 temp = node.GetPrevious();
                if (temp != null)
                {
                    if (temp.Text == "<") node.leftBlanks = "";  //'<='
                    else if (temp.Text == "=") node.leftBlanks = "";  //'=='
                    else if (temp.Text == ">") node.leftBlanks = "";  //'>='
                }
            }
            if (node.Text == ">")
            {
                ASTNode2 temp = node.GetPrevious();
                if (temp != null)
                {
                    if (temp.Text == "<") node.leftBlanks = "";  //'<>'                    
                }
            }
        }        

        private static bool KnownFunction(string ss2)
        {
            string ss = ss2.Trim().ToLower();
            return ss == "log" || ss == "exp" || ss == "pow" || ss == "abs" || ss == "pch" || ss == "dlog";
        }

        private static void HandleCommand2(ASTNode2 node)
        {
            //options
            if (G.equal(node.Text, "#"))
            {
                //Always transformed to %, we do not expect list or matrix in option field!
                ASTNode2 next = node.Parent.GetChild(node.id + 1);
                if (next != null && (G.IsIdentTranslate(next.Text) || next.Text == "#"))
                {
                    node.Text = "%";
                }
                node.Text = "%";
            }
            
            if(G.Equal(node.Text, FromTo("prim", "primary")) != null)
            {
                node.Text = Cap("edit", node.Text);
            }

            if (G.Equal(node.Text, FromTo("prot", "protect")) != null)
            {
                node.Text = ""; //<prot> is dead
            }
        }

        private static void HandleCommand1(ASTNode2 node, GekkoDictionary<string, string> listMemory, GekkoDictionary<string, string> matrixMemory, GekkoDictionary<string, string> scalarMemory)
        {
            //First part of command
            SetCommandType(node, node.Text);  //may be overwritten later on

            if (G.Equal(node.Text, FromTo("ac", "accept")) != null)
            {
                SetCommandType(node, "accept");
                SetCommandText(node, "accept");
            }

            if (G.Equal(node.Text, FromTo("as", "assign")) != null)
            {
                string name = node.GetCommand3()[0].Text;
                if (!scalarMemory.ContainsKey(name)) scalarMemory.Add(name, "");
                SetCommandType(node, "assign");
                SetCommandText(node, "assign"); //renamed into VAL, DATE or STRING/NAME                        
            }

            if (G.Equal(node.Text, FromTo("cle", "clear")) != null)
            {
                SetCommandType(node, "clear");
                SetCommandText(node, "clear");
            }

            if (G.Equal(node.Text, FromTo("clo", "close")) != null)
            {
                SetCommandType(node, "close");
                SetCommandText(node, "close");
            }

            if (G.Equal(node.Text, FromTo("closeall", "closeall")) != null || G.Equal(node.Text, FromTo("closebanks", "closebanks")) != null)
            {
                SetCommandType(node, "closeall");
                SetCommandText(node, Globals.restartSnippet);
                hasCloseall = true;
            }

            if (G.Equal(node.Text, FromTo("col", "collapse")) != null)
            {
                SetCommandType(node, "collapse");
                SetCommandText(node, "collapse");
            }

            if (G.Equal(node.Text, FromTo("convert", "convert")) != null)  //CONVERT is a procedure from AREMOS that seems to do the same as COLLAPSE.
            {
                SetCommandType(node, "convert");
                SetCommandText(node, "collapse");
            }

            if (G.Equal(node.Text, FromTo("comp", "compare")) != null)
            {
                SetCommandType(node, "compare");
                SetCommandText(node, "compare");
            }

            if (G.Equal(node.Text, FromTo("cou", "count")) != null)
            {
                SetCommandType(node, "count");
                SetCommandText(node, "count");
            }

            if (G.Equal(node.Text, FromTo("cop", "copy")) != null)
            {
                SetCommandType(node, "copy");
                SetCommandText(node, "copy");
                ASTNode2 xx = node.GetCommand3();
                foreach (ASTNode2 child in xx.ChildrenIterator())
                {
                    if (G.equal(child.Text, "as")) child.Text = Cap("to", child.Text);
                }
            }

            if (G.Equal(node.Text, FromTo("de", "delete")) != null)
            {
                SetCommandType(node, "delete");
                SetCommandText(node, "delete");
            }

            if (G.Equal(node.Text, FromTo("disp", "display")) != null)
            {
                SetCommandType(node, "display");
                SetCommandText(node, "disp");
            }

            if (G.Equal(node.Text, FromTo("for", "for")) != null)
            {
                string name = node.GetCommand3()[0].Text;
                if (!scalarMemory.ContainsKey(name)) scalarMemory.Add(name, "");
                SetCommandType(node, "for");
                SetCommandText(node, "for");
            }

            if (G.Equal(node.Text, FromTo("excelexport", "excelexport")) != null)
            {
                SetCommandType(node, "excelexport");
                SetCommandText(node, "sheet");
            }

            if (G.Equal(node.Text, FromTo("excelimport", "excelimport")) != null)
            {
                SetCommandType(node, "excelimport");
                SetCommandText(node, "sheet");
                node.GetCommand2().AddOptionBeforeVisitor("import");
            }

            if (G.Equal(node.Text, FromTo("expo", "export")) != null)
            {
                SetCommandType(node, "export");
                SetCommandText(node, "export");
            }

            if (G.Equal(node.Text, FromTo("for", "for")) != null)
            {
                SetCommandType(node, "for");
                SetCommandText(node, "for");
            }

            if (G.Equal(node.Text, FromTo("func", "function")) != null)
            {
                SetCommandType(node, "function");
                SetCommandText(node, "function");
            }

            if (G.Equal(node.Text, FromTo("got", "goto")) != null)
            {
                SetCommandType(node, "goto");
                SetCommandText(node, "goto");
            }

            if (G.Equal(node.Text, FromTo("gr", "graph")) != null)
            {
                SetCommandType(node, "graph");
                SetCommandText(node, "plot");
            }

            if (G.Equal(node.Text, FromTo("if", "if")) != null)
            {
                SetCommandType(node, "if");
                SetCommandText(node, "if");
            }

            if (G.Equal(node.Text, FromTo("impor", "import")) != null)
            {
                SetCommandType(node, "import");
                SetCommandText(node, "import");
            }

            if (G.Equal(node.Text, FromTo("ind", "index")) != null)
            {
                SetCommandType(node, "index");
                SetCommandText(node, "index");
                ASTNode2 xx = node.GetCommand3();
                string last = null;
                string leftBlanks = "";
                last = xx.GetLastChild().Text;
                leftBlanks = xx.GetLastChild().leftBlanks;
                //foreach (ASTNode2 child in xx.ChildrenIterator())
                //{
                //    last = child.Text;
                //    leftBlanks = child.leftBlanks;
                //}
                if (G.IsIdentTranslate(last) && leftBlanks.Length > 0)  //so that "LIST *a;" does not put a in mem
                {
                    if (!listMemory.ContainsKey(last)) listMemory.Add(last, "");
                }
            }

            if (G.Equal(node.Text, FromTo("lis", "list")) != null)
            {
                string name = node.GetCommand3()[0].Text;
                if (!listMemory.ContainsKey(name)) listMemory.Add(name, "");
                SetCommandType(node, "list");
                SetCommandText(node, "list");
            }

            if (G.Equal(node.Text, FromTo("ma", "matrix")) != null)
            {
                string name = node.GetCommand3()[0].Text;
                if (!matrixMemory.ContainsKey(name)) matrixMemory.Add(name, "");
                SetCommandType(node, "matrix");
                SetCommandText(node, "matrix");
            }

            if (G.Equal(node.Text, FromTo("mo", "model")) != null)
            {
                SetCommandType(node, "model");
                SetCommandText(node, "model");
            }

            if (G.Equal(node.Text, FromTo("ob", "obey")) != null)
            {
                SetCommandType(node, "obey");
                SetCommandText(node, "run");
            }

            if (G.Equal(node.Text, FromTo("op", "open")) != null)
            {
                SetCommandType(node, "open");
                SetCommandText(node, "open");
            }

            if (G.Equal(node.Text, FromTo("pa", "pause")) != null)
            {
                SetCommandType(node, "pause");
                SetCommandText(node, "pause");
            }

            if (G.Equal(node.Text, FromTo("pl", "plot")) != null)
            {
                SetCommandType(node, "plot");
                SetCommandText(node, "plot");
            }

            if (G.Equal(node.Text, FromTo("pri", "print")) != null)
            {
                SetCommandType(node, "print");
                SetCommandText(node, "prt");
            }

            if (G.Equal(node.Text, FromTo("ren", "rename")) != null)
            {
                SetCommandType(node, "rename");
                SetCommandText(node, "rename");
            }

            if (G.Equal(node.Text, FromTo("rest", "restore")) != null)
            {
                SetCommandType(node, "restore");
                SetCommandText(node, "run");
            }

            if (G.Equal(node.Text, FromTo("ret", "return")) != null)
            {
                SetCommandType(node, "return");
                SetCommandText(node, "return");
            }

            if (G.Equal(node.Text, FromTo("ser", "series")) != null)
            {
                SetCommandType(node, "series");
                SetCommandText(node, "series");
            }

            if (G.Equal(node.Text, FromTo("set", "set")) != null)
            {
                ASTNode2 xx = node.GetCommand3()[0];  //first element
                if (xx != null && G.Equal(xx.Text, FromTo("per", "period")) != null)
                {
                    SetCommandType(node, "set period");
                    SetCommandText(node, "time");
                    xx.Text = "";
                    ASTNode2 yy = xx.GetNext();
                    if (yy != null) yy.leftBlanks = "";
                }
                else
                {
                    SetCommandType(node, "set");
                    SetCommandText(node, "option");
                }
            }

            if (G.Equal(node.Text, FromTo("sm", "smooth")) != null)
            {
                SetCommandType(node, "smooth");
                SetCommandText(node, "smooth");
            }

            if (G.Equal(node.Text, FromTo("so", "solve")) != null)
            {
                SetCommandType(node, "solve");
                SetCommandText(node, "sim");
                //AddOption(node, "fix");  //JUST TESTING ------------------- !
            }

            if (G.Equal(node.Text, FromTo("spl", "splice")) != null)
            {
                SetCommandType(node, "splice");
                SetCommandText(node, "splice");
            }

            if (G.Equal(node.Text, FromTo("spool", "spool")) != null)
            {
                SetCommandType(node, "spool");
                SetCommandText(node, "pipe");
                //AddOption(node, "fix");  //JUST TESTING ------------------- !
            }

            if (G.Equal(node.Text, FromTo("stop", "stop")) != null)
            {
                SetCommandType(node, "stop");
                SetCommandText(node, "exit");
            }

            if (G.Equal(node.Text, FromTo("sys", "system")) != null)
            {
                SetCommandType(node, "system");
                SetCommandText(node, "sys");
            }

            if (G.Equal(node.Text, FromTo("tar", "target")) != null)
            {
                SetCommandType(node, "target");
                SetCommandText(node, "target");
            }

            if (G.Equal(node.Text, FromTo("tel", "tell")) != null)
            {
                SetCommandType(node, "tell");
                SetCommandText(node, "tell");
            }

            if (G.Equal(node.Text, FromTo("truncate", "truncate")) != null)
            {
                SetCommandType(node, "truncate");
                SetCommandText(node, "truncate");
            }

            if (G.Equal(node.Text, FromTo("unspool", "unspool")) != null)
            {
                SetCommandType(node, "unspool");
                SetCommandText(node, "pipe con");
                //AddOption(node, "fix");  //JUST TESTING ------------------- !
            }
        }

        private static string GetCommandType(ASTNode2 current)
        {
            string commandTypeAremosLower = null;            
            while (true)
            {
                if (current.commandTypeAremosLower != null)
                {
                    commandTypeAremosLower = current.commandTypeAremosLower;
                    break;
                }
                if (current.Parent == null) break;
                current = current.Parent;
            }
            return commandTypeAremosLower;
        }

        private static string HandleDateHiddenAsLiteral(string assignType, string rhs)
        {
            string dateHit = null;
            if (G.Equals(assignType.ToLower().Trim(), "name"))
            {
                string date = rhs.Trim();
                //TODO have " replaced with ' more generally in the tree, perhaps in a first pass
                if ((date.StartsWith("'") && date.EndsWith("'")) || (date.StartsWith("\"") && date.EndsWith("\"")))
                {
                    string date2 = date.Substring(1, date.Length - 2);
                    string date3 = date2.Trim();
                    //this should be e.g. 2006A1
                    if (IsDate(date3)) dateHit = date3;
                }
            }
            return dateHit;
        }

        private static bool IsDate(string date3)
        {
            string date4 = RemoveDateEnding(date3);  //2006a1 --> 2006
            if ((date4.Length == 2 || date4.Length == 4) && G.IsIntegerTranslate(date4))
            {
                int n = int.Parse(date4);
                if (DateRange(n))
                {
                    return true;
                }
            }
            return false;
        }

        private static string RemoveDateEnding(string date3)
        {
            string date4 = date3;
            foreach (string d in AllDateEndings())
            {
                if (date3.EndsWith(d, StringComparison.OrdinalIgnoreCase))
                {
                    date4 = date3.Substring(0, date3.Length - d.Length);
                }
            }
            return date4;
        }

        private static string[] AllDateEndings()
        {
            return new string[] { "a1", "q1", "q2", "q3", "q4", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "m10", "m11", "m12" };
        }

        private static bool DateRange(int n)
        {
            return (n >= 1800 && n < 2200) || (n >= 45 && n <= 99);
        }

        private static string HandleDateHiddenAsInteger(string assignType, string rhs)
        {
            string dateHit = null;
            if (G.Equals(assignType.ToLower().Trim(), "val"))
            {
                string date4 = rhs.Trim();
                if ((date4.Length == 2 || date4.Length == 4) && G.IsIntegerTranslate(date4))
                {
                    int n = int.Parse(date4);
                    if (DateRange(n))
                    {
                        dateHit = rhs;
                    }
                }
            }
            return dateHit;
        }

        private static string HandleAssign1FromTo(ASTNode2 node, string s1, string s2)
        {
            return G.Equal(node[1].Code.Trim(), FromTo(s1, s2));
        }

        private static string HandleAssign2(ASTNode2 node, string gekkoType, string s, string s2)
        {            
            var regex = new Regex(s, RegexOptions.IgnoreCase);
            gekkoType = regex.Replace(node[1].Code, Cap(s2, node.Parent.commandTypeAremos));
            gekkoType = gekkoType.TrimStart();
            return gekkoType;
        }            

        private static List<string> FromTo(string s1, string s2)
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

        private static void SetCommandType(ASTNode2 node, string s)
        {
            node.Parent.Parent.commandTypeAremosLower = s.ToLower();
            node.Parent.Parent.commandTypeAremos = Cap(s, node.Text);
        }

        private static void SetCommandText(ASTNode2 node, string s)
        {
            node.Text = Cap(s, node.Text);     
        }        

        private static void GetCodeFromAllChildren(ASTNode2 node)
        {
            string s = null;
            for (int i = 0; i < node.ChildrenCount(); ++i)
            {
                s += node[i].Code;
            }
            node.Code += s;
        }                

        private static string Cap(string input, string model)
        {
            model = model.Trim();
            bool upper = false;
            if (model != null && model.Length > 0 && char.IsUpper(model[0])) upper = true;
            if (upper) input = input.ToUpper();
            else input = input.ToLower();
            return input;
        }                                        

        public static string Translate2(bool file, List<string> inputFileLines)
        {
            string textInput = null;
            textInput = G.ExtractTextFromLines(inputFileLines).ToString();           

            ANTLRStringStream input = new ANTLRStringStream(textInput);
            CommonTree t = null;
            // Create a lexer attached to that input
            T2Parser parser2 = null;
            T2Lexer lexer2 = new T2Lexer(input);
            //usually debugTokens=false, and this is stepped into manually (otherwise the tokens are consumed and preliminary steps cannot be run)
            // Create a stream of tokens pulled from the lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer2);
            // Create a parser attached to the token stream
            parser2 = new T2Parser(tokens);
            // Invoke the program rule in get return value
            T2Parser.expr_return r = null;
            DateTime t0 = DateTime.Now;

            try
            {
                r = parser2.expr();
            }
            catch (Exception e)
            {
                List<string> temp = new List<string>();
                temp.Add(e.Message);
                string input2 = textInput + "\r\n";
                if (file)
                {
                    PrintError(e.Message, true);
                    G.Writeln2("*** ERROR: Could not parse the file");
                    throw new GekkoException(); //this will make a double error -- but the other one will be identified later on (both text and filename are null) and skipped -- a little bit hacky, but oh well...
                }
                else return "";
            }

            if (parser2.GetErrors().Count > 0)
            {
                List<string> errors = parser2.GetErrors();                
                if (file)
                {
                    foreach (string s in errors)
                    {
                        PrintError(s, false);
                    }                    
                    G.Writeln2("*** ERROR: Could not parse the file");
                    throw new GekkoException(); //this will make a double error -- but the other one will be identified later on (both text and filename are null) and skipped -- a little bit hacky, but oh well...
                }
                else return "";
            }
            t = (CommonTree)r.Tree;
            StringBuilder sb = new StringBuilder();
            GekkoDictionary<string, string> lists = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            GekkoDictionary<string, string> matrices = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            GekkoDictionary<string, string> scalars = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (Globals.printAST) Parser.Gek.ParserGekCreateAST.PrintAST(t, 0);
            ASTNode2 root = new ASTNode2(null);
            CreateASTNodesForTranslate2(t, root, 0);

            if (Globals.printAST)
            {
                G.Writeln2("==========================================================");
                G.Writeln("==========================================================");
                G.Writeln("==========================================================");
                G.Writeln();
                root.PrintAST2(root, 0);
            }
            TranslateTree(root, 0, sb, lists, matrices, scalars);            
            
            List<string> xxx = G.ExtractLinesFromText(root.Code);
            string sss = null;
            foreach (string s in xxx)
            {
                string s2 = s;
                if (s2 == ";") s2 = "";
                s2 = s2.Replace(";;", ";");
                s2 = s2.Replace("; ;", ";");
                string alphabet = "abcdefghijklmnopqrstuvwxyz";
                foreach (char c in alphabet)
                {
                    string s5 = c.ToString();
                    string s6 = c.ToString().ToUpper();
                    s2 = s2.Replace(";" + s5, "; " + s5);
                    s2 = s2.Replace(";" + s6, "; " + s6);
                }
                sss += s2 + G.NL;
            }
            if (!hasCloseall) sss = Globals.restartSnippet + G.NL + sss;
            return sss;
        }

        public static void PrintError(string s, bool lexer)
        {
            string[] ss = s.Split(Globals.parserErrorSeparator);
            int lineNumber = 0;
            int lineNo = 0;
            int positionNo = 0;
            string errorMessage = "General error";
            try
            {
                lineNumber = int.Parse(ss[0]) - 1;  //seems 1-based before subtract 1                
                lineNo = lineNumber + 1;  //1-based
                positionNo = int.Parse(ss[1]) + 1;  //1-based                               
                errorMessage = ss[3];
            }
            catch { };
            G.Writeln2("*** ERROR: Translation error:");
            G.Writeln("    The translator had problems with line " + lineNo + " pos " + positionNo);
            G.Writeln("    The error is the following:");
            G.Writeln("    " + errorMessage);
            if (lexer)
            {
                G.Writeln2("+++ NOTE: this is a error from the so-called lexer, so the error has to do with some of the");
                G.Writeln("          characters in the file. Please check for strange/funny characters, and please also");
                G.Writeln("          check for single quotes (') that are not closed with a corresponding single quote.");
                G.Writeln("          Also check for out-commented sections that are not closed (for instance a /* without");
                G.Writeln("          a corresponding */");
            }
        }

        private static bool DetectNullNode(CommonTree ast)
        {             
            bool isNull = ast.Text == null && !(ast.Children != null && ast.Children.Count > 0);
            //if (isNull) throw new GekkoException();
            return isNull;
        }

        public static void CreateASTNodesForTranslate2(CommonTree ast, ASTNode2 cmdNode, int depth)
        {
            if (DetectNullNode(ast))
            {                
                return;
            }

            cmdNode.Text = ast.Text;

            if (cmdNode.Text != null)
            {
                if (Globals.addGlue)
                {
                    cmdNode.Text = G.ReplaceGlueNew(cmdNode.Text);
                }
            }
            cmdNode.Line = ast.Line;
            if (ast.Children == null)
            {
                return;
            }
            int num = ast.Children.Count;
            cmdNode.CreateChildren(num);
            string storeBlanks = null;            
            for (int i = 0; i < num; ++i)
            {
                CommonTree d = (CommonTree)(ast.Children[i]);
                if (DetectNullNode(d))
                {
                    //Seems to be last node in first level, can be ignored
                    continue;
                }

                if (IsBlanks(d))
                {
                    storeBlanks += d.Text;
                }
                else
                {
                    //first                        
                    ASTNode2 cmdNodeChild = new ASTNode2(null);  //unknown text                    
                    cmdNodeChild.leftBlanks += storeBlanks;
                    cmdNodeChild.Parent = cmdNode;
                    cmdNode.Add(cmdNodeChild);
                    cmdNodeChild.id = cmdNode.ChildrenCount() - 1;
                    CreateASTNodesForTranslate2(d, cmdNodeChild, depth + 1);
                    storeBlanks = null;
                }                
            }
        }

        private static bool IsBlanks(CommonTree d)
        {
            return d.Text.Trim() == "";
        }

        
    }

    public class ASTNode2
    {
        /// <summary>
        /// Iterates the children of the ASTNode.
        /// One good thing about this iterator is that you can use
        /// foreach (ASTNode2 child in node.ChildrenIterator())
        /// even if node.Children = null. In that case, nothing
        /// is iterated, just as if node.Children.Count was 0.           
        /// Another benefit is that we may set .Children private at some
        /// point, so that its implementation may change (we use a List&lt;&gt; now).
        /// </summary>
        /// <returns></returns>
        public IEnumerable ChildrenIterator()
        {
            if (this.children != null)
            {
                foreach (ASTNode2 child in this.children)
                {
                    yield return child;
                }
            }
        }

        public void AddToLeftTextExtra(string s)
        {
            this.leftTextExtra = s + this.leftTextExtra;
        }

        public void AddToRightTextExtra(string s)
        {
            this.rightTextExtra = this.rightTextExtra + s;
        }

        public void RemoveLast()
        {
            this.children.RemoveAt(this.children.Count - 1);
        }

        public void AddOptionAfterVisitor(string s)
        {
            string code = this.Code;
            if (code == null) code = "";
            code = code.Trim();
            if (code == "")
            {
                code = "<" + s + ">";
            }
            else
            {
                if (code.EndsWith(">"))
                {
                    code = code.Substring(0, code.Length - 1) + " " + s + ">";
                }
                else { };  //ignore, should not be possible  
            }
            this.Code = code;         
        }

        public void AddOptionBeforeVisitor(string s)
        {
            ASTNode2 child = this[0];                        
            if (child == null)
            {
                ASTNode2 temp2 = new ASTNode2("ASTANGLE"); temp2.Parent = this;
                this.children = new List<ASTNode2>();
                this.children.Add(temp2);
                child = temp2;
            }
            if (child.children == null)
            {
                //"ASTANGLE"
                child.children = new List<ASTNode2>();
                ASTNode2 c1 = new ASTNode2("<"); c1.leftBlanks = " ";  c1.Parent = child;
                ASTNode2 c2 = new ASTNode2(">"); c2.leftBlanks = " "; c2.Parent = child;
                child.children.Add(c1);
                child.children.Add(c2);                
            }
            List<ASTNode2> temp = new List<ASTNode2>();
            ASTNode2 x = new ASTNode2(s); x.Parent = child; x.leftBlanks = " ";                       
            bool flag = false;
            foreach (ASTNode2 z in child.children)
            {
                if (z.Text == ">")
                {
                    temp.Add(x);                    
                }                
                temp.Add(z);
            }            
            child.children = temp;
        }

        public void AddComment(string s)
        {
            s = "/* TRANSLATE: " + s + " */";
            ASTNode2 child = this[0];
            if (child == null)
            {                
                this.children = new List<ASTNode2>();                
            }
            ASTNode2 x = new ASTNode2(s); x.Parent = this;
            x.leftBlanks = "  ";
            this.children.Add(x);
        }

        private List<ASTNode2> children = null; //private so that the implementation might change (for instance LinkedList etc.)
        //public GekkoStringBuilder code = new GekkoStringBuilder();  //the C# code produced while walking the tree
        public string Code = null; //the C# code produced while walking the tree
        public string CodeSentFromSubTree = null; //the C# code produced while walking the tree
        //public GekkoStringBuilder s = null;
        public ASTNode2 Parent = null;        
        public int id = -12345;
        public string Text = null;  //ANTLR decoration of the node (for instance 'ASTPRT' or '1.45').        
        public bool IgnoreNegate = false;  //can be set true in some case
        public string nameSimpleIdent = null; //used to make fast pointers to VALs. Same idea could be used for TimeSeries, but the name of these are often composed (unsimple), and TimeSeries is put outside time loop in GENR, so that helps anyway.
        public string dotNumber = null;  //indicator as to whether a fY.1 is present        
        public int Line = 0;
        public int Number = 0;  //used to check position among siblings
        public string commandLinesCounter = "0";
        public int expressionCounter = 0;
        public string[] specialExpressionAndLabelInfo = null;
        //----- special
        public string leftBlanks = "";
        public string leftTextExtra = "";
        public string rightTextExtra = "";
        public string commandTypeAremosLower = null;
        public string commandTypeAremos = null;

        public ASTNode2 GetCommand1()
        {
            ASTNode2 current = this;            
            while (true)
            {
                if (current.Parent == null) break;
                if (current.Parent.Text == "ASTCOMMAND") return current.Parent[0];
                current = current.Parent;
            }
            return null;            
        }
        public ASTNode2 GetCommand2()
        {
            ASTNode2 current = this;
            while (true)
            {
                if (current.Parent == null) break;
                if (current.Parent.Text == "ASTCOMMAND") return current.Parent[1];
                current = current.Parent;
            }
            return null;            
        }
        public ASTNode2 GetCommand3()
        {
            ASTNode2 current = this;
            while (true)
            {
                if (current.Parent == null) break;
                if (current.Parent.Text == "ASTCOMMAND") return current.Parent[2];
                current = current.Parent;
            }
            return null;               
        }
        
        public ASTNode2 GetChild(string s)
        {
            foreach (ASTNode2 child in this.ChildrenIterator())
            {
                if (child.Text == s) return child;
            }
            return null;
        }

        public ASTNode2 this[int i]
        {
            get
            {
                return this.GetChild(i);
            }
        }

        public int ChildrenCount()
        {
            if (children == null) return 0;
            return children.Count;
        }

        //Gets the C# code of child i.
        public string GetChildCode(int i)
        {
            ASTNode2 child = this.GetChild(i);
            if (child == null) return null;
            else return child.Code;
        }

        //Prepares an AST node to have children
        public void CreateChildren(int n)
        {
            this.children = new List<ASTNode2>(n);
        }

        public bool IsLastChild()
        {
            if (this.Parent == null) return true;
            if (this.Number == this.Parent.ChildrenCount() - 1) return true;  //should not be possible to be >
            return false;
        }

        public bool IsFirstChild()
        {
            if (this.Parent == null) return true;
            if (this.Number == 0) return true;
            return false;
        }

        ////Prepares an AST node to have children
        //public List<ASTNode2> GetChildren() {
        //    return children;
        //}

        //Sets the text of the AST node
        public ASTNode2(string text)
        {
            this.Text = text;
        }

        //Sets the text of the AST node
        public ASTNode2(string text, string leftBlanks)
        {
            this.Text = text;
            this.leftBlanks = leftBlanks;
        }

        //Sets the text of the AST node, and augments with children.
        public ASTNode2(string text, bool withChildren)
        {
            this.Text = text;
            if (withChildren)
            {
                this.children = new List<ASTNode2>();
            }
        }
                

        public ASTNode2 GetChild(int i)
        {
            if (this.children == null) return null;
            if (i < 0) return null;
            if (i >= this.children.Count) return null;  //does not exist
            return this.children[i];
        }

        public ASTNode2 GetNext()
        {
            ASTNode2 next = this.Parent.GetChild(this.id + 1);
            return next;
        }

        public ASTNode2 GetLastChild()
        {
            if (this.children == null) return null;
            return this.children[this.children.Count - 1];
        }

        public ASTNode2 GetFirstChild()
        {
            if (this.children == null) return null;
            return this.children[0];
        }

        public ASTNode2 GetPrevious()
        {
            ASTNode2 next = this.Parent.GetChild(this.id - 1);
            return next;
        }        

        public void Add(ASTNode2 child)
        {
            this.children.Add(child);
            child.Parent = this;
            child.Number = children.Count - 1;
        }

        public string ToString()
        {
            return this.Text;
        }

        public void PrintAST2(ASTNode2 node, int depth)
        {
            G.Writeln(G.Blanks(depth * 2) + node.Text);
            if (node.children != null)
            {
                for (int i = 0; i < node.children.Count; ++i)
                {
                    ASTNode2 child = (ASTNode2)(node.children[i]);
                    PrintAST2(child, depth + 1);
                }
            }
        }
    }
}




