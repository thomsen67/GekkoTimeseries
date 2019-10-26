using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace Gekko.Parser
{

    public class ArgHelper
    {
        public string type = null;
        public string internalName = null;
        public string labelCode = null;
        public string defaultValueCode = null;

        public ArgHelper(string type, string internalName, string label, string defaultValue)
        {
            this.type = type;
            this.internalName = internalName;
            this.labelCode = label;
            this.defaultValueCode = defaultValue;
        }
    }

    public class ASTNode
    {
        /// <summary>
        /// Iterates the children of the ASTNode.
        /// One good thing about this iterator is that you can use
        /// foreach (ASTNode child in node.ChildrenIterator())
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
                foreach (ASTNode child in this.children)
                {
                    yield return child;
                }
            }
        }

        public void RemoveLast()
        {
            this.children.RemoveAt(this.children.Count - 1);
        }

        private List<ASTNode> children = null; //private so that the implementation might change (for instance LinkedList etc.)
        //public GekkoStringBuilder code = new GekkoStringBuilder();  //the C# code produced while walking the tree

        //public string Code = null; //the C# code produced while walking the tree
        public Parser.Gek.GekkoSB Code = new Parser.Gek.GekkoSB(); //the C# code produced while walking the tree

        public Parser.Gek.GekkoSB AlternativeCode = null;  //used for ASTVARIABLEANDNAME

        public string CodeSentFromSubTree = null; //the C# code produced while walking the tree
        //public GekkoStringBuilder s = null;
        public ASTNode Parent = null;
        public string Text = null;  //ANTLR decoration of the node (for instance 'ASTPRT' or '1.45').        
        public bool IgnoreNegate = false;  //can be set true in some case
        public string nameSimpleIdent = null; //used to make fast pointers to VALs. Same idea could be used for Series, but the name of these are often composed (unsimple), and Series is put outside time loop in GENR, so that helps anyway.
        public string dotNumber = null;  //indicator as to whether a fY.1 is present        
        public int Line = 0;
        public int Number = 0;  //used to check position among siblings
        public string commandLinesCounter = "0";
        public int expressionCounter = 0;        
        public string[] specialExpressionAndLabelInfo = null;
        public string leftBlanks = null;
        public string timeLoopNestCode = null; //code delivered from sub-tree
        public GekkoDictionary<string, TwoStrings> listLoopAnchor = null;
        public string listLoopAnchor2 = null;  //lookups that need to be moved outside listloop, like sum(#i, x[#i]) where the lookup of x can be moved out
        public GekkoDictionary<string, string> functionDefAnchor = null;
        public List<ArgHelper> functionDef = null;  //1: , 2: , 3: label, 4: def.value
        public string functionType = null;
        public GekkoDictionary<string, string> forLoopAnchor = null;
        public GekkoDictionary<string, string> freeIndexedLists = null; //like x[#m], need to unfold for PRT/PLOT
        //public GekkoDictionary<string, string> freeIndexedListsLeftSide = null; //like series x[#m] = 1
        public List<Tuple<string, string>> forLoop = null;
        public string localInsideLoopVariablesCs = null;

        //public string listLoopNestCode = null; //code delivered from sub-tree
        public string ivTempVarName = null;
        public string mapTempVarName = null;
        

        public ASTNode GetChild(string s)
        {
            foreach (ASTNode child in this.ChildrenIterator())
            {
                if (child.Text == s) return child;
            }
            return null;
        }

        public ASTNode this[int i]
        {
            get
            {                
                return this.GetChild(i);
            }
            set
            {
                this.children[i] = value;
            }
        }

        public int ChildrenCount()
        {
            if (children == null) return 0;
            return children.Count;
        }

        //Gets the C# code of child i.
        public Parser.Gek.GekkoSB GetChildCode(int i)
        {
            ASTNode child = this.GetChild(i);
            if (child == null)
            {
                Parser.Gek.GekkoSB xx = new Parser.Gek.GekkoSB();
                //xx.storage = null;  //to signal null
                //xx.SetNull();
                return xx;
                //return null;
            }
            else return child.Code;            
        }
        

        //Prepares an AST node to have children
        public void CreateChildren(int n) {
            this.children = new List<ASTNode>(n);
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
        //public List<ASTNode> GetChildren() {
        //    return children;
        //}
        
        //Sets the text of the AST node
        public ASTNode(string text)
        {
            this.Text = text;
        }

        //Sets the text of the AST node
        public ASTNode(string text, string leftBlanks)
        {
            this.Text = text;
            this.leftBlanks = leftBlanks;
        }

        //Sets the text of the AST node, and augments with children.
        public ASTNode(string text, bool withChildren)
        {
            this.Text = text;
            if (withChildren)
            {
                this.children = new List<ASTNode>();                
            }            
        }

        ////Adds a node with text and a single child with textChild
        //public ASTNode(string text, string textChild)
        //{
            
        //    this.Text = text;
        //    this.children = new List<ASTNode>();
        //    this.children.Add(new ASTNode(textChild));
        //}

        //public ASTNode(string text, string textChild1, string textChild2)
        //{
        //    //adds a node with text and a singla child with textChild
        //    this.Text = text;
        //    this.children = new List<ASTNode>();
        //    this.children.Add(new ASTNode(textChild1));
        //    this.children.Add(new ASTNode(textChild2));
        //}

        public ASTNode GetChild(int i)
        {
            if (this.children == null) return null;
            if (i >= this.children.Count) return null;  //does not exist
            return this.children[i];
        }                

        public void Add(ASTNode child)
        {
            this.children.Add(child);
            child.Parent = this;
            child.Number = children.Count - 1;
        }

        public string ToString()
        {
            return this.Text;
        }

        public void PrintAST2(ASTNode node, int depth)
        {
            G.Writeln(G.Blanks(depth * 2) + node.Text);
            if (node.children != null)
            {
                for (int i = 0; i < node.children.Count; ++i)
                {
                    ASTNode child = (ASTNode)(node.children[i]);
                    PrintAST2(child, depth + 1);
                }
            }
        }
    }

    public class GekkoStringBuilder
    {
        StringBuilder container = null;  //starts out as null in order to save RAM in a large AST tree
        
        public void AppendLine(string s)
        {
            if (container == null) container = new StringBuilder();
            this.container.AppendLine(s);
        }

        public void Append(string s)
        {
            if (container == null) container = new StringBuilder();
            this.container.Append(s);
        }

        public void Append(GekkoStringBuilder s)
        {
            if (container == null) container = new StringBuilder();
            this.container.Append(s);
        }

        public void Replace(string s1, string s2)
        {
            if (container == null) return;
            this.container.Replace(s1, s2);
        }

        public override string ToString()
        {
            if (container == null) return null;
            return this.container.ToString();
        }

    }
}
