using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace Gekko.Parser
{
    public class ASTNodeSimple
    {
        public IEnumerable ChildrenIterator()
        {
            //One good thing about this iterator is that you can use
            //  foreach (ASTNode child in node.ChildrenIterator())
            //even if node.Children = null. In that case, nothing
            //is iterated, just as if node.Children.Count was 0.
            //This is practical.
            //Another benefit is that we may set .Children private at some
            //point, so that its implementation may change (we use a List<> now).
            if (this.Children != null)
            {
                foreach (ASTNodeSimple child in this.Children)
                {
                    yield return child;
                }
            }
        }

        public List<ASTNodeSimple> Children = null;  //make this private at some point
        public ASTNodeSimple parent = null;
        public string Text = null;
        public int Line = 0;
        public ASTNodeSimple(string text)
        {
            this.Text = text;
        }
        public ASTNodeSimple(string text, bool withChildren)
        {
            this.Text = text;
            if (withChildren)
            {
                this.Children = new List<ASTNodeSimple>();
            }
        }

        public ASTNodeSimple(string text, string textChild)
        {
            //adds a node with text and a singla child with textChild
            this.Text = text;
            this.Children = new List<ASTNodeSimple>();
            this.Children.Add(new ASTNodeSimple(textChild));
        }

        public ASTNodeSimple(string text, string textChild1, string textChild2)
        {
            //adds a node with text and a singla child with textChild
            this.Text = text;
            this.Children = new List<ASTNodeSimple>();
            this.Children.Add(new ASTNodeSimple(textChild1));
            this.Children.Add(new ASTNodeSimple(textChild2));
        }

        public ASTNodeSimple GetChild(int i)
        {
            if (this.Children == null) return null;
            if (i >= this.Children.Count) return null;  //does not exist
            return this.Children[i];
        }

        public string GetChildString(int i)
        {
            //will be obsolete
            //---> "@`stringcontent`"
            ASTNodeSimple node = this.GetChild(i);
            if (node == null) return "null";
            else return "@`" + node.Text + "`";
        }

        public string GetChildText(int i)
        {
            ASTNodeSimple node = this.GetChild(i);
            if (node == null) return null;
            else return node.Text;
        }

        public void Add(ASTNodeSimple child)
        {
            this.Children.Add(child);
            child.parent = this;
        }

        public string ToString()
        {
            return this.Text;
        }
    }
}
