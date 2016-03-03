using System;
using System.Collections.Generic;
using System.Text;

namespace Gekko.Parser
{
    /// <summary>
    /// This is not the ANTLR stringtemplate engine, but something a bit like it. The idea is that is should be easy to
    /// write stuff like "Prt({0}, {1});" and have the {0} and {1} filled with stuff from deeper children AST nodes. This fill-in
    /// will be done when the last of these children are finished.
    /// </summary>
    public class GekkoST
    {
        //private StringBuilder container = new StringBuilder();
        List<GekkoSTHelper> container = new List<GekkoSTHelper>();
        
        public void Append(string s)
        {
            container.Add(new GekkoSTHelper(s, null));
        }

        public void Append(string s, ASTNode node)
        {
            container.Add(new GekkoSTHelper(s, new List<ASTNode>() { node }));
        }

        public void Append(string s, ASTNode node0, ASTNode node1)
        {
            container.Add(new GekkoSTHelper(s, new List<ASTNode>() { node0, node1 }));
        }
    }

    public class GekkoSTHelper
    {
        private string s;
        private List<ASTNode> nodes;
        public GekkoSTHelper(string s2, List<ASTNode> nodes2)
        {
            s2 = s2.Replace("{#}", Globals.counter.ToString());
            s = s2;
            nodes = nodes2;
        }        
    }
}
